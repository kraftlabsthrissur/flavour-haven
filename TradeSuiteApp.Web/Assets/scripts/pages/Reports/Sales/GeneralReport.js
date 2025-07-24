$(function () {
    GeneralReports.init();
});

GeneralReports = {
    init: function () {
        var self = GeneralReports;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = GeneralReports;
        $('#Refresh').on('click', self.refresh);
        ////$('.report_type').on("ifChecked", self.show_incomeexpense);
        $('.report_type').on('ifChanged', self.show_incomeexpense);
    },
    refresh: function () {
        var self = GeneralReports;
        $('#FromDate').val('');
        $('#ToDate').val('');
    },

    show_incomeexpense: function () {
        var self = GeneralReports;
        var report_type = $(".report_type:checked").val();
        if (report_type == "Income&Expenses") {
            $('.IncomeExpenses').removeClass('uk-hidden');
        }
        else {
            $('.IncomeExpenses').addClass('uk-hidden');
        }
        
    },

    get_filters: function () {
        var self = GeneralReports;
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