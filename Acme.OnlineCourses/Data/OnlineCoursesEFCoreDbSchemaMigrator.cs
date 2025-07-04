﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Acme.OnlineCourses.Data;

public class OnlineCoursesEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public OnlineCoursesEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the OnlineCoursesDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OnlineCoursesDbContext>()
            .Database
            .MigrateAsync();
    }
}
