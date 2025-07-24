$(function () {
    EmployeeDailyReport.init();
});

EmployeeDailyReport = {
    init: function () {
        var self = EmployeeDailyReport;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = EmployeeDailyReport;
        $('#Refresh').on('click', self.refresh);
    },
    refresh: function () {
        var self = EmployeeDailyReport;
        $('#FromDate').val('');
        $('#ToDate').val('');
    },
    get_filters: function () {
        var self = EmployeeDailyReport;
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