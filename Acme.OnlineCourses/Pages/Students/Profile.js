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
        if (!$('#Student_FullName').val()) {
            abp.notify.error('Full name is required');
            return;
        }

        var formData = new FormData();

        // Add form fields to FormData
        formData.append('Id', studentId);
        formData.append('FullName', $('#Student_FullName').val());
        formData.append('Email', $('#Student_Email').val());
        formData.append('PhoneNumber', $('#Student_PhoneNumber').val());
        formData.append('DateOfBirth', $('#Student_DateOfBirth').val());
        formData.append('IdentityNumber', $('#Student_IdentityNumber').val());
        formData.append('Address', $('#Student_Address').val());
        formData.append('StudentNote', $('#Student_StudentNote').val() || '');
      //  formData.append('AgencyId', $('#Student_AgencyId').val());
        formData.append('CourseId', $('#Student_CourseId').val());
   
        // Add files to formData
        var fileInput = $attachments[0];
        if (fileInput.files.length > 0) {
            for (var i = 0; i < fileInput.files.length; i++) {
                formData.append('files', fileInput.files[i]);
            }
        }

        abp.ui.setBusy($form);

        abp.ajax({
            url: '/api/app/student/update',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false
        }).done(function () {
            abp.notify.info(l('StudentRegisteredSuccessfully'));
            window.location.href = '/Students/Profile';
        }).always(function () {
            abp.ui.clearBusy($form);
        });
    });

    // Handle file upload separately
    $attachments.on('change', function() {
        var files = this.files;
        if (files.length > 0) {
            var studentId = $('#Student_Id').val();
            var formData = new FormData();
            
            // Add studentId to FormData
            formData.append('studentId', studentId);
            
            for (var i = 0; i < files.length; i++) {
                formData.append('file', files[i]);
                formData.append('description', 'test'); // Add empty description
            }

            //$.ajax({
            //    url: abp.appPath + 'api/app/student/upload',
            //    type: 'POST',
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    success: function (result) {
            //        console.log(result);
            //        abp.notify.info(l('SuccessfullyUploaded'));
            //        location.reload();
            //    },
            //    error: function (error) {
            //        console.log(error);
            //        if (error.responseJSON && error.responseJSON.error) {
            //            abp.notify.error(error.responseJSON.error.message);
            //        } else {
            //            abp.notify.error('An error occurred while uploading files');
            //        }
            //    }
            //});
        }
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