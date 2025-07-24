$(function () {
    StockAdjustment.init();
});

StockAdjustment = {
    init: function () {
        var self = StockAdjustment;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = StockAdjustment;
        $('#Refresh').on('click', self.refresh);
        $("#ItemCategoryID").change(ReportHelper.get_sales_category);
    },

    refresh: function () {
        var self = StockAdjustment;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#ItemLocationID").val(locationID);
        $("#ItemCategoryID").val('');
        $("#SalesCategoryID").val('');
    },

    get_filters: function () {
        var self = StockAdjustment;
        var filters = "";

        if ($("#ItemLocationID").val() != 0) {
            filters += "Location: " + $("#ItemLocationID option:selected").text() + ", ";
        }
        

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    }

}