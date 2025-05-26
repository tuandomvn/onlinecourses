$(function () {
    var l = abp.localization.getResource('OnlineCourses');
    var dataTable = $('#BlogsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            ajax: abp.libs.datatables.createAjax(acme.onlineCourses.blogs.blog.getList),
            columnDefs: [
                {
                    title: l('Title'),
                    data: "title"
                },
                {
                    title: l('PublishedDate'),
                    data: "publishedDate",
                    render: function (data) {
                        return data ? moment(data).format('L') : '';
                    }
                },
                {
                    title: l('IsPublished'),
                    data: "isPublished",
                    render: function (data) {
                        return data ? '<i class="fas fa-check text-success"></i>' : '<i class="fas fa-times text-danger"></i>';
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
                                        return l('BlogDeletionConfirmationMessage', data.record.title);
                                    },
                                    action: function (data) {
                                        acme.onlineCourses.blogs.blog
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

    var createModal = new abp.ModalManager(abp.appPath + 'Blogs/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Blogs/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewBlogButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
}); 