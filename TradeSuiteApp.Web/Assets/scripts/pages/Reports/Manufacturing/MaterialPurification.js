$(function () {
    MaterialPurification.init();
});

MaterialPurification = {
    init: function () {
        var self = MaterialPurification;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = MaterialPurification;

        $('#Refresh').on('click', self.refresh);

        $('#suppliername-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_name);

        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_issue_item_code);
        $('#preprocess-issue-item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_issue_item);

        $('#item-code-to-autocomplete').on('selectitem.uk.autocomplete', self.set_receipt_item_code);
        $('#preprocess-receipt-item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_receipt_item);

        $('#preprocess-issueno-autocomplete').on('selectitem.uk.autocomplete', self.set_issueno);
        $('#preprocess-receiptno-autocomplete').on('selectitem.uk.autocomplete', self.set_receipt);
    },

    set_issueno: function (event, item) {
        var self = MaterialPurification;
        $('#IssueNoID').val(item.id);
    },

    set_receipt: function (event, item) {
        var self = MaterialPurification;
        $('#ReceiptNoID').val(item.id);
    },

    set_supplier_name: function (event, item) {
        var self = MaterialPurification;
        $('#SupplierID').val(item.id);
    },

    set_issue_item_code: function (event, item) {
        var self = MaterialPurification;
        $('#IssueItemCodeID').val(item.id);
    },

    set_issue_item: function (event, item) {
        var self = MaterialPurification;
        $('#IssueItemID').val(item.itemid);
    },

    set_receipt_item_code: function (event, item) {
        var self = MaterialPurification;
        $('#ReceiptItemCodeID').val(item.id);
    },

    set_receipt_item: function (event, item) {
        var self = MaterialPurification;
        $('#ReceiptItemID').val(item.itemid);
    },

    refresh: function () {
        var self = MaterialPurification;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#ReceiptDateFrom").val(findate);
        $("#ReceiptDateTo").val(currentdate);
        $("#SupplierName").val('');
        $("#SupplierID").val('');
        $("#ItemCodeFrom").val('');
        $("#IssueItemCodeID").val('');
        $("#RceiptCodeFrom").val('');
        $("#ReceiptItemCodeID").val('');
        $("#IssueItemName").val('');
        $("#IssueItemID").val('');
        $("#ReceiptItemName").val('');
        $("#ReceiptItemID").val('');
        $("#ProcessID").val('');
        $("#IssueNo").val('');
        $("#IssueNoID").val('');
        $("#ReceiptNo").val('');
        $("#ReceiptNoID").val('');
    },

    get_filters: function () {
        var self = MaterialPurification;
        var filters = "";
    }
}