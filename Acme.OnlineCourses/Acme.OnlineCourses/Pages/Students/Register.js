$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var registerForm = $('#RegisterForm');
    var termsModal = $('#TermsModal');
    var termsContent = $('#TermsContent');
    var agreeToTermsCheckbox = $('#AgreeToTermsCheckbox');
    var agreeToTermsHidden = $('#AgreeToTerms');

    // Load terms content when modal opens
    $('#TermsLink').click(function (e) {
        e.preventDefault();
        loadTermsContent();
        termsModal.modal('show');
    });

    function loadTermsContent() {
        abp.ajax({
            url: abp.appPath + 'api/app/blog/by-code/BLG003',
            type: 'GET'
        }).done(function (result) {
            if (result) {
                termsContent.html(result.content);
            } else {
                termsContent.html('<div class="alert alert-warning">@L["BlogNotFound"]</div>');
            }
        }).fail(function () {
            termsContent.html('<div class="alert alert-danger">@L["ErrorLoadingTerms"]</div>');
        });
    }

    // Handle accept terms button
    $('#AcceptTermsButton').click(function () {
        if (agreeToTermsCheckbox.is(':checked')) {
            agreeToTermsHidden.val('true');
            termsModal.modal('hide');
        } else {
            abp.notify.warn(l('PleaseAgreeToTerms'));
        }
    });

    // Reset checkbox when modal is closed
    termsModal.on('hidden.bs.modal', function () {
        agreeToTermsCheckbox.prop('checked', false);
    });

    $('#SaveButton').click(function (e) {
        e.preventDefault();

        if (!registerForm.valid()) {
            return;
        }

        if (agreeToTermsHidden.val() !== 'true') {
            abp.notify.warn(l('PleaseAgreeToTerms'));
            return;
        }

        var student = registerForm.serializeFormToObject();

        abp.ajax({
            url: abp.appPath + 'api/app/student/register',
            type: 'POST',
            data: JSON.stringify(student)
        }).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/Students';
        });
    });
}); 