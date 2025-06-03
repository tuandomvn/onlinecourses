using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using static Acme.OnlineCourses.Students.StudentAppService;
using UpdateStudentProfileDto = Acme.OnlineCourses.Students.StudentAppService.UpdateStudentProfileDto;

namespace Acme.OnlineCourses.Students;

public interface IStudentAppService :
    ICrudAppService<
        StudentDto,
        Guid,
        GetStudentListDto,
        CreateUpdateStudentDto>
{
    Task<StudentDto> RegisterStudentAsync(RegisterStudentDto input);
    Task<StudentDto> GetByEmailAsync(string email);
    Task<bool> IsUserExistsAsync(string email);
    Task<List<StudentAttachmentDto>> GetAttachmentsAsync(Guid studentId);
    Task<StudentAttachmentDto> UploadAttachmentAsync(Guid studentId, IFormFile file, string description);
    Task DeleteAttachmentAsync(Guid attachmentId);
    //Task<StudentDto> UpdateProfileAsync([FromQuery] Guid id, [FromBody] CreateUpdateStudentDto input);
    Task<StudentDto> UpdateAsync(Guid id, CreateUpdateStudentDto input);
} 