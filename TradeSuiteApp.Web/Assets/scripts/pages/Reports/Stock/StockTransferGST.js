$(function () {
    StockTransferGST.init();
});

StockTransferGST = {
    init: function () {
        var self = StockTransferGST;
        self.bind_events();
        ReportHelper.init();
        self.gst_report_type();
    },

    bind_events: function () {
        var self = StockTransferGST;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('.report_type').on('ifChanged', self.gst_report_type);
        $('#receipt-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_recipt_no_from);
        $('#receipt-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_recipt_no_to);
        $('#issue-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_issue_no_from);
        $('#issue-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_issue_no_to);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_no);
        $("#FromLocationID").on("change", self.get_location_from);
        $("#ToLocationID").on("change", self.get_location_to);
        $('#Refresh').on('click', self.refresh);
    },

    get_location_from: function () {
        var self = StockTransferGST;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
        var locationID = clean($("#LocationID").val());
        var id = clean($("#FromLocationID").val());
        var locationheadID = $("#FromLocationID option:selected").data("headid");

        if (locationID == locationheadID) {
            $('#FromLocationID').attr('disabled', false);
            $('#ToLocationID').attr('disabled', false);
        }
        else {
            if ($("#FromLocationID").val() != locationID) {
                $('#ToLocationID option:selected').val(locationID);
                $('#ToLocationID').attr('disabled', true);
            }
            else {
                $('#ToLocationID').attr('disabled', false);
            }
        }
    },

    get_location_to: function () {
        var self = StockTransferGST;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }

        var locationID = clean($("#LocationID").val());
        var id = clean($("#ToLocationID").val());
        var locationheadID = $("#ToLocationID option:selected").data("headid");
        if (locationID == locationheadID) {
            $('#ToLocationID').attr('disabled', false);
            $('#FromLocationID').attr('disabled', false);
        }
        else {
            if ($("#ToLocationID").val() != locationID) {
                $("#FromLocationID option:selected").val(locationID);
                $('#FromLocationID').attr('disabled', true);
            }
            else {
                $('#FromLocationID').attr('disabled', false);
            }
        }
    },

    set_stock_recipt_no_from: function (event, item) {
        var self = StockTransferGST;
        $('#ReceiptNoFromID').val(item.id);
    },

    set_stock_recipt_no_to: function (event, item) {
        var self = StockTransferGST;
        $('#ReceiptNoToID').val(item.id);
    },

    set_stock_issue_no_from: function (event, item) {
        var self = StockTransferGST;
        $('#IssueNoFromID').val(item.id);
    },

    set_stock_issue_no_to: function (event, item) {
        var self = StockTransferGST;
        $('#IssueNoToID').val(item.id);
    },

    set_item_name: function (event, item) {
        var self = StockValuationReport;
        $("#ItemID").val(item.id);
    },

    set_batch_no: function (event, item) {
        var self = StockValuationReport;
        $("#BatchID").val(item.id)
    },

    refresh: function (event, item) {
        var self = StockTransferGST;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $('#ReceiptNoFrom').val('');
        $('#ReceiptNoTo').val('');
        $('#IssueNoFrom').val('');
        $('#IssueNoTo').val('');
        $("#FromLocationID").val(locationID);
        $("#ToLocationID").val(locationID);
        $('#FromLocationID').attr('disabled', false);
        $('#ToLocationID').attr('disabled', false);
        $('#IssueNoFromID').val('');
        $('#IssueNoToID').val('');
        $('#ReceiptNoFromID').val('');
        $('#ReceiptNoToID').val('');
    },

    gst_report_type: function () {
        var self = StockTransferGST;
        var report_type = $(".report_type:checked").val();
        $('.filters').addClass('uk-hidden');
        $("." + report_type).removeClass("uk-hidden");
        self.refresh();
    },

    get_filters: function () {
        var self = StockTransferGST;
        var filters = "";
        if (($("#IssueNoFrom").val() + $("#IssueNoTo").val()).trim() != "") {
            if ($("#IssueNoTo").val().trim() == "") {
                filters += "Issue No " + "From: " + $("#IssueNoFrom").val() + ", ";
            }
            else if ($("#IssueNoFrom").val().trim() == "") {
                filters += "Issue No " + "To: " + $("#IssueNoTo").val() + ", ";
            }
            else if (($("#IssueNoFrom").val() + $("#IssueNoTo").val()).trim() != "") {
                filters += "Issue No: " + $("#IssueNoFrom").val() + " - " + $("#IssueNoTo").val() + ", ";
            }
        }

        if (($("#ReceiptNoFrom").val() + $("#ReceiptNoTo").val()).trim() != "") {
            if ($("#ReceiptNoTo").val().trim() == "") {
                filters += "Receipt No " + "From: " + $("#ReceiptNoFrom").val() + ", ";
            }
            else if ($("#ReceiptNoFrom").val().trim() == "") {
                filters += "Receipt No " + "To: " + $("#ReceiptNoTo").val() + ", ";
            }
            else if (($("#ReceiptNoFrom").val() + $("#ReceiptNoTo").val()).trim() != "") {
                filters += "Receipt No: " + $("#ReceiptNoFrom").val() + " - " + $("#ReceiptNoTo").val() + ", ";
            }
        }

        if (($("#FromLocationID").val() + $("#ToLocationID").val()).trim() != "") {
            if ($("#ToLocationID").val().trim() == 0) {
                filters += "Location " + "From: " + $("#FromLocationID option:selected").text() + ", ";
            }
            else if ($("#FromLocationID").val().trim() == 0) {
                filters += "Location " + "To: " + $("#ToLocationID option:selected").text() + ", ";
            }
            else if (($("#FromLocationID").val() + $("#ToLocationID").val()).trim() != "") {
                filters += "Location: " + $("#FromLocationID option:selected").text() + " - " + $("#ToLocationID option:selected").text() + ", ";
            }
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    }
}