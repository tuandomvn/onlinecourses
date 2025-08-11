$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    
    // Test if Bootstrap modal is available
    if (typeof $.fn.modal === 'undefined') {
        console.error('Bootstrap modal is not loaded!');
        return;
    }
    
    var _$form = $('#RegisterForm');
    var _$termsModal = $('#TermsModal');
    var _$termsContent = $('#TermsContent');
    var _$agreeToTermsCheckbox = $('#AgreeToTermsCheckbox');
    var _$modalAgreeToTermsCheckbox = $('#ModalAgreeToTermsCheckbox');
    var _$agreeToTerms = $('#AgreeToTerms');
    var fileList = $('#fileList');
    var attachments = [];

    // Disable save button initially
    var _$saveButton = $('#SaveButton');
    //_$saveButton.prop('disabled', true);

    // Enable save button when terms are agreed (main form checkbox)
    _$agreeToTermsCheckbox.change(function () {
        _$saveButton.prop('disabled', !$(this).is(':checked'));
        _$agreeToTerms.val($(this).is(':checked'));
        
        // Sync with modal checkbox
        _$modalAgreeToTermsCheckbox.prop('checked', $(this).is(':checked'));
    });

    // Sync modal checkbox with main form checkbox
    _$modalAgreeToTermsCheckbox.change(function () {
        _$agreeToTermsCheckbox.prop('checked', $(this).is(':checked'));
        _$saveButton.prop('disabled', !$(this).is(':checked'));
        _$agreeToTerms.val($(this).is(':checked'));
    });

    // Add click handler for Save button
    //_$saveButton.click(function() {
    //    _$form.submit();
    //});

    _$form.on('submit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        // Kiểm tra đồng ý điều khoản
        if (!_$agreeToTermsCheckbox.is(':checked')) {
            //toastr.error(l('PleaseAgreeToTerms'), l('Notification'));
            return;
        }

        var formData = new FormData();
        
        // Add form fields to FormData
        formData.append('Fullname', $('#Student_Fullname').val());
        formData.append('Email', $('#Student_Email').val());
        formData.append('PhoneNumber', $('#Student_PhoneNumber').val());
        formData.append('DateOfBirth', $('#Student_DateOfBirth').val());
        formData.append('ExpectedStudyDate', $('#Student_ExpectedStudyDate').val() || '');
        formData.append('Address', $('#Student_Address').val());
        formData.append('StudentNote', $('#Student_StudentNote').val() || '');
        formData.append('AgencyId', $('#Student_AgencyId').val());
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
            //toastr.success(l('StudentRegisteredSuccessfully'));

            // Show success message
            Swal.fire({
                text: l('StudentRegisteredSuccessfully'),
                icon: 'success',
                confirmButtonText: l("Close")
            });



            // Kiểm tra xem user đã đăng nhập chưa
            var currentUser = abp.currentUser;
            if (currentUser && currentUser.isAuthenticated) {
                // User đã đăng nhập - redirect đến Profile sau 2 giây
                setTimeout(function() {
                    window.location.href = '/Students/Profile';
                }, 2000);
            } else {
                // User chưa đăng nhập - giữ nguyên ở trang Register
                // Reset form để user có thể đăng ký thêm
                setTimeout(function() {
                    _$form[0].reset();
                    _$agreeToTermsCheckbox.prop('checked', false);
                    fileList.empty();
                    attachments = [];
                }, 2000);
            }
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



    $('#TermsLink').click(function (e) {
        e.preventDefault();
        console.log('TermsLink clicked');
        
        // Load content first
        //loadTermsContent();
        
        // Then open modal
        $('#TermsModal').modal('show');
    });

    // Handle modal close button click
    $('#ModalCloseBtn').click(function() {
        console.log('Modal close button clicked');
        $('#TermsModal').modal('hide');
    });

    // Handle modal footer close button click
    $('#ModalFooterCloseBtn').click(function() {
        console.log('Modal footer close button clicked');
        $('#TermsModal').modal('hide');
    });

    // Handle close buttons using event delegation (in case buttons are added dynamically)
    $(document).on('click', '[data-dismiss="modal"], .close', function() {
        console.log('Close button clicked via delegation');
        $('#TermsModal').modal('hide');
    });

    // Handle backdrop click to close modal
    $('#TermsModal').on('click', function(e) {
        if (e.target === this) {
            console.log('Backdrop clicked');
            $('#TermsModal').modal('hide');
        }
    });

}); 