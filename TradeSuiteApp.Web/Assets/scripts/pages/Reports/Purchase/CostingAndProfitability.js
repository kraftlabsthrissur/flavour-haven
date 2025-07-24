$(function () {
    CostingAndProfitability.init();
});

CostingAndProfitability = {
    init: function () {
        var self = CostingAndProfitability;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = CostingAndProfitability;
        $('#Refresh').on('click', self.refresh);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
    },
    refresh: function () {
        var self = CostingAndProfitability;
        $('#FromDate').val('');
        $('#ToDate').val('');
    },
    set_item_name: function (event, item) {
        var self = CostingAndProfitability;
        $("#ItemID").val(item.id);
    },
    get_filters: function () {
        var self = CostingAndProfitability;
        var filters = "";
        if ($("#FromDateString").val() != " ") {
            filters += "From Date: " + $("#FromDateString").val() + ", ";
        }
        if ($("#ToDateString").val() != 0) {
            filters += "To Date: " + $("#ToDateString").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
}