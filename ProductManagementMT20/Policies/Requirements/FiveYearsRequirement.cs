using Microsoft.AspNetCore.Authorization;

namespace ProductManagementMT20.Policies.Requirements
{
    public class FiveYearsRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; set; }
        public FiveYearsRequirement(int minimumYears)
        {
            MinimumYears = minimumYears;
        }
    }
}
