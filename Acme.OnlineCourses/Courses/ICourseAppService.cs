using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Courses.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Courses;

public interface ICourseAppService : IApplicationService
{
    Task<List<CourseDto>> GetListAsync();
} 