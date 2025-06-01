$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var _studentService = acme.onlineCourses.students.student;
    var _$form = $('#RegisterForm');
    var _$saveButton = $('#SaveButton');
    var _$agreeToTermsCheckbox = $('#AgreeToTermsCheckbox');
    var _$termsLink = $('#TermsLink');
    var _$termsModal = $('#TermsModal');
    var _$termsContent = $('#TermsContent');

    // Disable save button initially
    _$saveButton.prop('disabled', true);

    // Handle terms checkbox change
    _$agreeToTermsCheckbox.on('change', function() {
        _$saveButton.prop('disabled', !$(this).is(':checked'));
        $('#AgreeToTerms').val($(this).is(':checked'));
        
        // Close modal if checkbox is checked
        if ($(this).is(':checked')) {
            _$termsModal.modal('hide');
        }
    });

    _$termsLink.on('click', function (e) {
        e.preventDefault();
        loadTermsContent();
        _$termsModal.modal('show');
    });

    // Handle close button click
    $('.close, .btn-secondary').on('click', function() {
        _$termsModal.modal('hide');
    });

    function loadTermsContent() {
        console.log('loadTermsContent...')
        abp.ajax({
            url: abp.appPath + 'api/app/blog/by-code/BLG003',
            type: 'GET'
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

    _$form.on('submit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        if (!_$agreeToTermsCheckbox.is(':checked')) {
            abp.notify.error(l('PleaseAgreeToTerms'));
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

        // Add AgreeToTerms
        formData.agreeToTerms = _$agreeToTermsCheckbox.is(':checked');

        abp.ui.setBusy(_$form);
        _studentService.registerStudent(formData)
            .done(function () {
                abp.notify.info(l('SuccessfullyRegistered'));
                //window.location.href = '/Students';
            })
            .always(function () {
                abp.ui.clearBusy(_$form);
            });
    });

    _$saveButton.on('click', function (e) {
        e.preventDefault();
        _$form.submit();
    });
}); 