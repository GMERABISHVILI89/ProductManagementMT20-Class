using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementMT20.Interfaces;
using ProductManagementMT20.Models;
using ProductManagementMT20.Models.Entities;
using ProductManagementMT20.Models.VM;

namespace ProductManagementMT20.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i =>
                new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            return View(registerViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.RoleSelected != null)
                    {
                        await _userManager.AddToRoleAsync(user, model.RoleSelected);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    await _context.SaveChangesAsync();
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new
                    {
                        userId = user.Id,
                        code
                    }, protocol: HttpContext.Request.Scheme);

                    //await _emailService.SendEmail(model.Email, "Confirm-email",
                    //    $"Please confirm your email by clicking here: <a href='{callbackUrl}'>Link</a>");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(callbackUrl);
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.RoleList = _roleManager.Roles.Select(x => x.Name).Select(i =>
            new SelectListItem
            {
                Text = i,
                Value = i
            });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
