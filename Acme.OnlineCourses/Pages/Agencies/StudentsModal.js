$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable;

    function initializeDataTable() {
        dataTable = $('#AgencyStudentsTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                processing: true,
                ajax: abp.libs.datatables.createAjax(acme.onlineCourses.agencies.agency.getStudentsList, function (input) {
                    var sorting = '';
                    //if (input.order && input.order.length > 0) {
                    //    sorting = input.order[0].column + ' ' + input.order[0].dir;
                    //}

                    var data = {
                        agencyId: $('#AgencyId').val()
                    };
                    console.log('Calling API with data1:', data);
                    return data;
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
                            return moment(data).format('DD/MM/YYYY');
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

    initializeDataTable();
}); 