using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Students;

public interface IStudentAppService :
    ICrudAppService<
        StudentDto,
        Guid,
        GetStudentListDto,
        CreateUpdateStudentDto>
{
    Task<StudentDto> RegisterStudentAsync([FromForm] RegisterStudentDto input, [FromForm] List<IFormFile> files);
    Task<StudentDto> GetByEmailAsync(string email);
    Task<ProfileStudentDto> GetProfileStudentByEmailAsync(string email);
    Task<bool> IsUserExistsAsync(string email);
    Task<List<StudentAttachmentDto>> GetAttachmentsAsync(Guid studentId);
    Task<StudentAttachmentDto> UploadAttachmentAsync(Guid studentId, IFormFile file, string description);
    Task DeleteAttachmentAsync(Guid attachmentId);
    Task<StudentDto> UpdateAsync(Guid id, CreateUpdateStudentDto input);
    Task<PagedResultDto<AdminViewStudentDto>> GetStudentsWithCoursesAsync(GetStudentListDto input);
    Task<UpdateStudentCourseDto> GetStudentCourseAsync(Guid studentId, Guid courseId);
    Task UpdateStudentCourseAsync(UpdateStudentCourseDto input);
} 