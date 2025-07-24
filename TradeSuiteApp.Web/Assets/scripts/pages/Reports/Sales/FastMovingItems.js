$(function () {
    FastMovingItemsReport.init();
});

FastMovingItemsReport = {
    init: function () {
        var self = FastMovingItemsReport;
        self.bind_events();
        ReportHelper.init();
        $('#Quantity').hide();
    },

    bind_events: function () {
        var self = FastMovingItemsReport;
        $('#Refresh').on('click', self.refresh);
        $('.reportTypeQuantity').on("ifChecked", self.show_summary);
        $('.reportType').on("ifChecked", self.hide_summary);
        $('.Detail').on("ifChecked", self.hide_date);
        $('.summary').on("ifChecked", self.show_date);
    },
    show_summary: function () {
        var self = FastMovingItemsReport;
        $('#Quantity').show();
        if ($('.Detail').is(':checked')) {
            $('#Date').hide();
        }
    },
    hide_date: function () {
        var self = FastMovingItemsReport;
        $('#Date').hide();
    },
    show_date: function () {
        var self = FastMovingItemsReport;
        $('#Date').show();
    },
    hide_summary: function () {
        var self = FastMovingItemsReport;
        $('#Quantity').hide();
        $('#Date').show();
        $('#Mode').val("");
        $('#Mode').attr('checked', false);
    },
    refresh: function () {
        var self = FastMovingItemsReport;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FirstDayOfMonth").val();
        $('#FromDateString').val(findate);
        $('#ToDateString').val(currentdate);
    },
    get_filters: function () {
        var self = FastMovingItemsReport;
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