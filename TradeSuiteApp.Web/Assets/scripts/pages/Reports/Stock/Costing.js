$(function () {
    CostingReport.init();
});
CostingReport = {
    init: function () {
        var self = CostingReport;
        ReportHelper.init();
        self.bind_events();
    },

    bind_events: function () {
        var self = CostingReport;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_name);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $('.Costing_ReportType').on('ifChanged', self.show_report_type);
        $('#Refresh').on('click', self.refresh);
        $("#ItemCategoryID").change(ReportHelper.get_sales_category);
    },

    show_report_type: function () {
        self = StockLegerReport;
        var type = $(".Costing_ReportType:checked").val();
        if (type == "Summary") {
            $(".ItemCategoryID").addClass("uk-hidden");
            $(".SalesCategoryID").addClass("uk-hidden");
            $(".batch").addClass("uk-hidden");
        }
        else {
            $(".ItemCategoryID").removeClass("uk-hidden");
            $(".SalesCategoryID").removeClass("uk-hidden");
            $(".batch").removeClass("uk-hidden");
        }
        self.refresh();
    },

    set_item_name: function (event, item) {
        var self = CostingReport;
        $("#ItemID").val(item.id);
    },

    set_batch_name: function (event, item) {
        var self = CostingReport;
        $("#BatchID").val(item.id);
    },

    refresh: function (event, item) {
        var self = CostingReport;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#LocationID").val(locationID);
        $("#ItemCategoryID").val('');
        $("#SalesCategoryID").val('');
        $("#BatchNo").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
    },

    get_filters: function () {
        var self = CostingReport;
        var filters = "";

        if ($("#LocationID").val() != 0) {
            filters += "Location: " + $("#LocationID option:selected").text() + ", ";
        }

        if ($("#ItemCategoryID").val() != 0) {
            filters += "Item Category: " + $("#ItemCategoryID option:selected").text() + ", ";
        }

        if ($("#SalesCategoryID").val() != 0) {
            filters += "Sales Category: " + $("#SalesCategoryID option:selected").text() + ", ";
        }

        if ($("#ItemName").val() != 0) {
            filters += "Item Name: " + $("#ItemName").val() + ", ";
        }

        if ($("#BatchNo").val() != 0) {
            filters += "Batch No: " + $("#BatchNo").val() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },
}


