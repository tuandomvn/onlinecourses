$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable;
    var modalManager = new abp.ModalManager(abp.appPath + 'Agencies/StudentsModal');

    function initializeDataTable() {
        dataTable = $('#AgencyStudentsTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[0, "asc"]],
                searching: false,
                ajax: abp.libs.datatables.createAjax(function (input) {
                    return acme.onlineCourses.agencies.agency.getStudentsByAgency(modalManager.getArgs().agencyId, input);
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
                        title: l('CourseName'),
                        data: "courseName"
                    },
                    {
                        title: l('RegistrationDate'),
                        data: "registrationDate",
                        render: function (data) {
                            return moment(data).format('L');
                        }
                    },
                    {
                        title: l('TestStatus'),
                        data: "testStatus",
                        render: function (data) {
                            return l('Enum:TestStatus:' + data);
                        }
                    },
                    {
                        title: l('PaymentStatus'),
                        data: "paymentStatus",
                        render: function (data) {
                            return l('Enum:PaymentStatus:' + data);
                        }
                    },
                    {
                        title: l('AccountStatus'),
                        data: "accountStatus",
                        render: function (data) {
                            return l('Enum:AccountStatus:' + data);
                        }
                    }
                ]
            })
        );
    }

    modalManager.onResult(function () {
        dataTable.ajax.reload();
    });

    modalManager.onOpen(function () {
        initializeDataTable();
    });
}); 