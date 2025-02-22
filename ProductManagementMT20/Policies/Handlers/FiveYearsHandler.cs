using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductManagementMT20.Models;
using ProductManagementMT20.Policies.Requirements;
using System.Security.Claims;

namespace ProductManagementMT20.Policies.Handlers
{
    public class FiveYearsHandler : AuthorizationHandler<FiveYearsRequirement>
    {
        private readonly ApplicationDbContext _context;

        public FiveYearsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FiveYearsRequirement requirement)
        {
            var yearsWorkedClaim = context.User.FindFirst("FiveYearsWorked");

            var daysWorded = 0;

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                context.User.FindFirst("UserId");

            var user = _context.Users.FirstOrDefault(x => x.Id == userIdClaim.Value);

            if (user != null)
                daysWorded = (DateTime.Now - user.RegistrationDate).Days;

            if(yearsWorkedClaim != null)
            {
                if(daysWorded >= requirement.MinimumYears * 365)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
