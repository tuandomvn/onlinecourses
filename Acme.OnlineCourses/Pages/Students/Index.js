$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable = $('#StudentsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[4, "desc"]], // Sort by registration date by default
            searching: false,
            ajax: abp.libs.datatables.createAjax(acme.onlineCourses.students.student.getStudentsWithCourses, function () {
                return {
                    courseStatus: $('#CourseStatusFilter').val(),
                    agencyId: $('#AgencyFilter').val(),
                };
            }),
            columnDefs: [
                {
                    title: l('FullName'),
                    data: "fullName"
                },
                {
                    title: l('Email'),
                    data: "email"
                },
                {
                    title: l('PhoneNumber'),
                    data: "phoneNumber"
                },
                {
                    title: l('RegistrationDate'),
                    data: "registrationDate",
                    render: function (data) {
                        return data ? moment(data).format('L') : '';
                    }
                },
                {
                    title: l('CourseName'),
                    data: "courseName"
                },
                {
                    title: l('CourseStatus'),
                    data: "courseStatus",
                    render: function (data) {
                        return data !== null ? l('Enum:StudentCourseStatus:' + data) : '';
                    }
                },
                {
                    title: l('TestStatus'),
                    data: "testStatus",
                    render: function (data) {
                        return data !== null ? l('Enum:TestStatus:' + data) : '';
                    }
                },
                {
                    title: l('PaymentStatus'),
                    data: "paymentStatus",
                    render: function (data) {
                        return data !== null ? l('Enum:PaymentStatus:' + data) : '';
                    }
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ 
                                            studentId: data.record.id, 
                                            courseId: data.record.courseId 
                                        });
                                    }
                                },
                                //{
                                //    text: l('Delete'),
                                //    confirmMessage: function (data) {
                                //        return l('StudentDeletionConfirmationMessage', data.record.fullName);
                                //    },
                                //    action: function (data) {
                                //        acme.onlineCourses.students.student
                                //            .delete(data.record.id)
                                //            .then(function() {
                                //                abp.notify.info(l('SuccessfullyDeleted'));
                                //                dataTable.ajax.reload();
                                //            });
                                //    }
                                //}
                            ]
                    }
                }
            ]
        })
    );

    var createModal = new abp.ModalManager(abp.appPath + 'Students/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Students/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewStudentButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#CourseStatusFilter').change(function () {
        dataTable.ajax.reload();
    });

    $('#AgencyFilter').change(function () {
        dataTable.ajax.reload();
    });
}); 