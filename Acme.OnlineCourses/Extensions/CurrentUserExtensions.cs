using System;
using System.Threading.Tasks;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Acme.OnlineCourses.Agencies;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Extensions;

public static class CurrentUserExtensions
{
    public static async Task<IdentityUser> GetUserAsync(this ICurrentUser currentUser, IIdentityUserRepository userRepository)
    {
        if (currentUser == null || !currentUser.IsAuthenticated || !currentUser.Id.HasValue)
        {
            return null;
        }

        return await userRepository.FindAsync(currentUser.Id.Value);
    }

    public static async Task<Guid?> GetAgencyIdAsync(this ICurrentUser currentUser, IIdentityUserRepository userRepository,
        IRepository<Agency, Guid> agencyRepository)
    {
        if (currentUser == null || userRepository == null)
        {
            return null;
        }

        var user = await currentUser.GetUserAsync(userRepository);
        if (user == null)
        {
            return null;
        }

        // First try to get from user properties
        var agencyIdStr = user.GetProperty<string>("AgencyId");
        if (!string.IsNullOrEmpty(agencyIdStr) &&  Guid.TryParse(agencyIdStr, out Guid agencyId) && agencyId != Guid.Empty)
        {
            return agencyId;
        }

        // If not found in properties, try to get from agencies table by email
        if (agencyRepository != null)
        {
            var agency = await agencyRepository.FirstOrDefaultAsync(x => x.ContactEmail == user.Email);
            if (agency != null)
            {
                // Store the agency ID in user properties for future use
                user.SetProperty("AgencyId", agency.Id.ToString());
                await userRepository.UpdateAsync(user);
                return agency.Id;
            }
        }

        return null;
    }
} 