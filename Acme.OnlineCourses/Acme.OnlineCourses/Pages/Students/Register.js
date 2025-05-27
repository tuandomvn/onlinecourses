$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var registerForm = $('#RegisterForm');

    $('#SaveButton').click(function (e) {
        e.preventDefault();

        if (!registerForm.valid()) {
            return;
        }

        var student = registerForm.serializeFormToObject();

        abp.ajax({
            url: '/Students/Register',
            type: 'POST',
            data: JSON.stringify(student)
        }).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/Students';
        });
    });
}); 