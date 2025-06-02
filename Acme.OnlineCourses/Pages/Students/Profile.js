$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var _studentService = acme.onlineCourses.students.student;
    var _$form = $('#ProfileForm');
    var _$saveButton = $('#SaveButton');

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
}); 