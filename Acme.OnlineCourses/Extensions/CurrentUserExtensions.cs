using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;

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

    public static async Task<Guid?> GetAgencyIdAsync(this ICurrentUser currentUser, IIdentityUserRepository userRepository)
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

        var agencyIdStr = user.GetProperty<string>("AgencyId");
        if (string.IsNullOrEmpty(agencyIdStr))
        {
            return null;
        }

        return Guid.Parse(agencyIdStr);
    }
} 