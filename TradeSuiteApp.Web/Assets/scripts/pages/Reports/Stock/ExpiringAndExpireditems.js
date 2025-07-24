$(function () {
    ExpiringAndExpiredItems.init();
});

ExpiringAndExpiredItems = {
    init: function () {
        var self = ExpiringAndExpiredItems;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = ExpiringAndExpiredItems;
        $('#Refresh').on('click', self.refresh);
        $('.Expired').on("ifChecked", self.hide_date);
        $('.Expiring').on("ifChecked", self.show_date);
    },
    hide_date: function () {
        var self = ExpiringAndExpiredItems;
        $('#ExpiryDate').hide();
    },
    show_date: function () {
        var self = ExpiringAndExpiredItems;
        $('#ExpiryDate').show();
    },
    refresh: function () {
        var self = ExpiringAndExpiredItems;
        $('#FromDateString').val('');
        $('#ToDateString').val('');
    },
    get_filters: function () {
        var self = ExpiringAndExpiredItems;
        var filters = "";
        if ($("#FromDateString").val() != "") {
            filters += "From Date: " + $("#FromDateString").val() + ", ";
        }
        if ($("#ToDateString").val() != "") {
            filters += "To Date: " + $("#ToDateString").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
}