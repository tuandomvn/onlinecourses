$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable;
    var studentsDataTable;
    var modalManager = new abp.ModalManager({
        viewUrl: abp.appPath + 'Agencies/StudentsModal',
        modalClass: 'StudentsModal'
    });

    // Add new modal manager for "Cấp tài khoản"
    var createAccountModal = new abp.ModalManager(abp.appPath + 'Agencies/CreateAgencyAccountModal');

    // Add event handler for modal opened
    modalManager.onOpen(function() {
        console.log('Modal opened, initializing students table');
        initializeStudentsTable();

        // Bind close button event
        $('#StudentsModal .btn-secondary').off('click').on('click', function(e) {
            console.log('Close button clicked');
            e.preventDefault();
            e.stopPropagation();
            modalManager.close();
            return false;
        });

        // Bind modal background click
        $('#StudentsModal').off('click').on('click', function(e) {
            if ($(e.target).is('#StudentsModal')) {
                console.log('Modal background clicked');
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    });

    // Add event handler for modal hidden
    $(document).on('hidden.bs.modal', '#StudentsModal', function () {
        console.log('Modal hidden, cleaning up');
        if (studentsDataTable) {
            studentsDataTable.destroy();
            studentsDataTable = null;
        }
        $('#AgencyStudentsTable').empty();
    });

    function initializeStudentsTable() {
        console.log('Initializing Students DataTable');
        if (studentsDataTable) {
            console.log('Destroying existing Students DataTable');
            studentsDataTable.destroy();
            $('#AgencyStudentsTable').empty();
        }

        studentsDataTable = $('#AgencyStudentsTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                processing: true,
                ajax: abp.libs.datatables.createAjax(acme.onlineCourses.agencies.agency.getStudentsList, function (input) {
                    var data = {
                        agencyId: $('#AgencyId').val()
                    };
                    console.log('Calling API with data:', data);
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
                    //{
                    //    title: l('CityCode'),
                    //    data: "cityCode"
                    //},
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

    function initializeDataTable() {
        dataTable = $('#AgenciesTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                ajax: abp.libs.datatables.createAjax(acme.onlineCourses.agencies.agency.getListAllAgency),
                columnDefs: [
                    {
                        title: l('Code'),
                        data: "code"
                    },
                    {
                        title: l('Name'),
                        data: "name"
                    },
                    {
                        title: l('OrgName'),
                        data: "orgName"
                    },
                    {
                        title: l('ContactEmail'),
                        data: "contactEmail"
                    },
                    {
                        title: l('ContactPhone'),
                        data: "contactPhone"
                    },
                    {
                        title: l('CityCode'),
                        data: "cityCode"
                    },
                    {
                        title: l('CommissionPercent'),
                        data: "commissionPercent",
                        render: function (data) {
                            return data + '%';
                        }
                    },
                    {
                        title: l('Status'),
                        data: "status",
                        render: function (data) {
                            return l('Enum:AgencyStatus:' + data);
                        }
                    },
                    {
                        title: l('Actions'),
                        rowAction: {
                            items:
                                [
                                    //{
                                    //    text: l('ViewStudents'),
                                    //    action: function (data) {
                                    //        console.log('Opening modal for agency:', data.record.id);
                                    //        modalManager.open({
                                    //            agencyId: data.record.id
                                    //        });
                                    //    }
                                    //},
                                    {
                                        text: 'Cấp tài khoản',
                                        action: function (data) {
                                            createAccountModal.open({ id: data.record.id });
                                        }
                                    },
                                    {
                                        text: l('Edit'),
                                        action: function (data) {
                                            editModal.open({ id: data.record.id });
                                        }
                                    },
                                    {
                                        text: l('Delete'),
                                        confirmMessage: function (data) {
                                            return l('AgencyDeletionConfirmationMessage', data.record.name);
                                        },
                                        action: function (data) {
                                            acme.onlineCourses.agencies.agency
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.notify.info(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        }
                                    }
                                ]
                        }
                    }
                ]
            })
        );
    }

    initializeDataTable();

    var createModal = new abp.ModalManager(abp.appPath + 'Agencies/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Agencies/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    createAccountModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAgencyButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
}); 