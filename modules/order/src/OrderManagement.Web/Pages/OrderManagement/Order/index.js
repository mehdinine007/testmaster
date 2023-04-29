$(function () {

    var l = abp.localization.getResource('OrderManagement');
    var _createModal = new abp.ModalManager(abp.appPath + 'OrderManagement/Orders/Create');
    var _editModal = new abp.ModalManager(abp.appPath + 'OrderManagement/Orders/Edit');

    var _dataTable = $('#OrdersTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "desc"]],
        ajax: abp.libs.datatables.createAjax(OrderManagement.Orders.getListPaged),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('OrderManagement.Order.Update'),
                                action: function (data) {
                                    _editModal.open({
                                        OrderId: data.record.id
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('OrderManagement.Order.Delete'),
                                confirmMessage: function (data) { return l('OrderDeletionWarningMessage'); },
                                action: function (data) {
                                    OrderManagement.Orders
                                        .delete(data.record.id)
                                        .then(function () {
                                            _dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            {
                target: 1,
                data: "code"
            },
            {
                target: 2,
                data: "name"
            },
            {
                target: 3,
                data: "price"
            },
            {
                target: 4,
                data: "stockCount"
            }
        ]
    }));


    $("#CreateNewOrderButtonId").click(function () {
        _createModal.open();
    });

    _createModal.onClose(function () {
        _dataTable.ajax.reload();
    });

    _editModal.onResult(function () {
        _dataTable.ajax.reload();
    });

});