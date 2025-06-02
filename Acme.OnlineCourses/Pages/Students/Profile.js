$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var _studentService = acme.onlineCourses.students.student;
    var _$form = $('#ProfileForm');
    var _$saveButton = $('#SaveButton');
    var _$uploadForm = $('#UploadForm');
    var _$uploadButton = $('#UploadButton');

    _$form.on('submit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var formData = {};
        var formArray = _$form.serializeArray();
        formArray.forEach(function(item) {
            if (item.name.startsWith('Student.')) {
                var key = item.name.replace('Student.', '');
                formData[key] = item.value;
            }
        });

        abp.ui.setBusy(_$form);
        _studentService.update(formData.id, formData)
            .done(function () {
                abp.notify.info(l('SuccessfullyUpdated'));
            })
            .always(function () {
                abp.ui.clearBusy(_$form);
            });
    });

    _$saveButton.on('click', function (e) {
        e.preventDefault();
        _$form.submit();
    });

    _$uploadButton.on('click', function (e) {
        e.preventDefault();
        uploadFiles();
    });

    function uploadFiles() {
        var formData = new FormData(_$uploadForm[0]);
        
        abp.ui.setBusy(_$uploadForm);
        $.ajax({
            url: abp.appPath + 'api/app/student/upload-attachments',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                abp.notify.info(l('SuccessfullyUploaded'));
                location.reload();
            },
            error: function (error) {
                abp.notify.error(error.message);
            },
            complete: function () {
                abp.ui.clearBusy(_$uploadForm);
            }
        });
    }

    $('.delete-attachment').on('click', function () {
        var attachmentId = $(this).data('id');
        var $attachmentItem = $(this).closest('.attachment-item');

        abp.message.confirm(l('AreYouSureToDelete'), l('DeleteConfirmationMessage'), function (isConfirmed) {
            if (isConfirmed) {
                abp.ui.setBusy($attachmentItem);
                _studentService.deleteAttachment(attachmentId)
                    .done(function () {
                        $attachmentItem.remove();
                        abp.notify.info(l('SuccessfullyDeleted'));
                    })
                    .always(function () {
                        abp.ui.clearBusy($attachmentItem);
                    });
            }
        });
    });
}); 