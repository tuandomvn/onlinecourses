$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable = $('#AgenciesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            ajax: abp.libs.datatables.createAjax(acme.onlineCourses.agencies.agency.getList),
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
                    title: l('ContactEmail'),
                    data: "contactEmail"
                },
                {
                    title: l('ContactPhone'),
                    data: "contactPhone"
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
                                            .then(function() {
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

    var createModal = new abp.ModalManager(abp.appPath + 'Agencies/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Agencies/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAgencyButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
}); 