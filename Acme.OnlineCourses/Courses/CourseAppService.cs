using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Courses.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Acme.OnlineCourses.Courses;

public class CourseAppService : ApplicationService, ICourseAppService
{
    private readonly IRepository<Course, System.Guid> _courseRepository;
    private readonly IObjectMapper _objectMapper;

    public CourseAppService(
        IRepository<Course, System.Guid> courseRepository,
        IObjectMapper objectMapper)
    {
        _courseRepository = courseRepository;
        _objectMapper = objectMapper;
    }

    public async Task<List<CourseDto>> GetListAsync()
    {
        var courses = await _courseRepository.GetListAsync();
        return _objectMapper.Map<List<Course>, List<CourseDto>>(courses);
    }
} 