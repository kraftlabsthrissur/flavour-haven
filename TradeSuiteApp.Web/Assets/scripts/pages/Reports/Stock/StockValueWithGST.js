$(function () {
    StockValueWithGST.init();
});
StockValueWithGST = {
    init: function () {
        var self = StockValueWithGST;
        ReportHelper.init();
        self.bind_events();
    },

    bind_events: function () {
        var self = StockValueWithGST;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $('#Refresh').on('click', self.refresh);
        //$("#ItemCategoryID").change(ReportHelper.get_sales_category);
    },

    set_item_name: function (event, item) {
        var self = StockValueWithGST;
        $("#ItemID").val(item.id);
    },

    refresh: function (event, item) {
        var self = StockValueWithGST;
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#LocationID").val(locationID);
        $("#ItemName").val('');
        $("#ItemID").val('');
    },

    get_filters: function () {
        var self = StockValueWithGST;
        var filters = "";

        if ($("#LocationID").val() != 0) {
            filters += "Location: " + $("#LocationID option:selected").text() + ", ";
        }

        if ($("#ItemName").val() != 0) {
            filters += "Item Name: " + $("#ItemName").val() + ", ";
        }


        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },
}


