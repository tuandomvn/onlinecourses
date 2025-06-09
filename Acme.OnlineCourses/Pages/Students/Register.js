$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var _$form = $('#RegisterForm');
    var _$termsModal = $('#TermsModal');
    var _$termsContent = $('#TermsContent');
    var _$agreeToTermsCheckbox = $('#AgreeToTermsCheckbox');
    var _$agreeToTerms = $('#AgreeToTerms');
    var fileList = $('#fileList');
    var attachments = [];

    // Disable save button initially
    var _$saveButton = $('#SaveButton');
    _$saveButton.prop('disabled', true);

    // Enable save button when terms are agreed
    _$agreeToTermsCheckbox.change(function () {
        _$saveButton.prop('disabled', !$(this).is(':checked'));
        _$agreeToTerms.val($(this).is(':checked'));

        // Close modal if checkbox is checked
        if ($(this).is(':checked')) {
            _$termsModal.modal('hide');
        }
    });

    // Add click handler for Save button
    _$saveButton.click(function() {
        _$form.submit();
    });

    _$form.on('submit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var formData = new FormData();
        
        // Add form fields to FormData
        formData.append('FirstName', $('#Student_FirstName').val());
        formData.append('LastName', $('#Student_LastName').val());
        formData.append('Email', $('#Student_Email').val());
        formData.append('PhoneNumber', $('#Student_PhoneNumber').val());
        formData.append('DateOfBirth', $('#Student_DateOfBirth').val());
        formData.append('IdentityNumber', $('#Student_IdentityNumber').val());
        formData.append('Address', $('#Student_Address').val());
        formData.append('StudentNote', $('#Student_StudentNote').val() || '');
        formData.append('AgencyId', $('#Student_AgencyId').val());
        formData.append('CourseId', $('#Student_CourseId').val());
        formData.append('AgreeToTerms', _$agreeToTerms.val());
        
        // Add files to formData
        var fileInput = $('#attachments')[0];
        if (fileInput.files.length > 0) {
            for (var i = 0; i < fileInput.files.length; i++) {
                formData.append('files', fileInput.files[i]);
            }
        }

        abp.ui.setBusy(_$form);

        abp.ajax({
            url: '/api/app/student/register-student',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false
        }).done(function () {
            abp.notify.info(l('StudentRegisteredSuccessfully'));
            window.location.href = '/Students/Profile';
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    });

    $('#attachments').on('change', function(e) {
        var files = e.target.files;
        fileList.empty();
        attachments = [];

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            attachments.push(file);
            
            var fileItem = $('<div class="file-item mb-2">' +
                '<span class="file-name">' + file.name + '</span> ' +
                '<span class="file-size">(' + formatFileSize(file.size) + ')</span>' +
                '</div>');
            fileList.append(fileItem);
        }
    });

    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        var k = 1024;
        var sizes = ['Bytes', 'KB', 'MB', 'GB'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    $('#TermsLink').click(function () {
        loadTermsContent();
        $('#TermsModal').modal('show');
    });

    function loadTermsContent() {
        console.log('loadTermsContent...')
        var currentLanguage = abp.localization.currentCulture.name === 'en' ? 0 : 1; // 0: en, 1: vi
        abp.ajax({
            url: abp.appPath + 'api/app/blog/by-code/BLG003',
            type: 'GET',
            data: {
                language: currentLanguage
            }
        }).done(function (result) {
            if (result) {
                _$termsContent.html(result.content);
            } else {
                _$termsContent.html('<div class="alert alert-warning">@L["BlogNotFound"]</div>');
            }
        }).fail(function () {
            _$termsContent.html('<div class="alert alert-danger">@L["ErrorLoadingTerms"]</div>');
        });
    }
}); 