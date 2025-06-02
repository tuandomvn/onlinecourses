using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Services;

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
} 