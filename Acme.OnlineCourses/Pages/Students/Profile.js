$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var profileService = acme.onlineCourses.students.student;
    var $form = $('#ProfileForm');
    var $attachments = $('#Attachments');
    var $attachmentsList = $('#AttachmentsList');

    $form.on('submit', function (e) {
        e.preventDefault();
        var studentId = $('#Student_Id').val();
        
        // Validate required fields
        if (!$('#Student_FirstName').val() || !$('#Student_LastName').val()) {
            abp.notify.error('First name and last name are required');
            return;
        }

        // Create data object with PascalCase properties to match C# DTO
        var data = {
            Id: studentId,
            FirstName: $('#Student_FirstName').val(),
            LastName: $('#Student_LastName').val(),
            PhoneNumber: $('#Student_PhoneNumber').val() || null,
            DateOfBirth: $('#Student_DateOfBirth').val() ? new Date($('#Student_DateOfBirth').val()).toISOString() : null,
            IdentityNumber: $('#Student_IdentityNumber').val() || null,
            Address: $('#Student_Address').val() || null,
            Email: $('#Student_Email').val(),
            AgencyId: $('#Student_AgencyId').val() || null,
            TestStatus: $('#Student_TestStatus').val(),
            PaymentStatus: $('#Student_PaymentStatus').val(),
            AccountStatus: $('#Student_AccountStatus').val(),
            CourseStatus: $('#Student_CourseStatus').val(),
            AgreeToTerms: $('#Student_AgreeToTerms').is(':checked'),
            RegistrationDate: $('#Student_RegistrationDate').val() ? new Date($('#Student_RegistrationDate').val()).toISOString() : null
        };

        data.Attachments = [];

        abp.ui.block();
        $.ajax({
            url: abp.appPath + 'api/app/student/' + studentId,
            type: 'PUT',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (result) {
                console.log(result);
                abp.notify.info(l('SuccessfullyUpdated'));
                location.reload();
            },
            error: function (error) {
                console.log(error);
                if (error.responseJSON && error.responseJSON.error) {
                    abp.notify.error(error.responseJSON.error.message);
                } else {
                    abp.notify.error('An error occurred while updating the profile');
                }
            },
            complete: function () {
                abp.ui.unblock();
            }
        });
    });

    $('.delete-attachment').on('click', function () {
        var id = $(this).data('id');
        var $item = $(this).closest('.list-group-item');

        abp.message.confirm(l('AreYouSureToDelete'), null, function (confirmed) {
            if (confirmed) {
                profileService.deleteAttachment(id).then(function () {
                    $item.remove();
                    abp.notify.info(l('SuccessfullyDeleted'));
                });
            }
        });
    });
}); 