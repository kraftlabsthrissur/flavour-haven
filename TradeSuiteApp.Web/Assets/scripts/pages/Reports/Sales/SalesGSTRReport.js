$(function () {
    SalesGSTR.init();
});

SalesGSTR = {
    init: function () {
        var self = SalesGSTR;
        self.bind_events();
        ReportHelper.init();
        self.get_report_type();
    },

    bind_events: function () {
        var self = SalesGSTR;
        $('.report_type').on('ifChanged', self.get_report_type);
        $('#Refresh').on('click', self.refresh);
    },

    get_report_type: function () {
        var self = SalesGSTR;
        var report_type = $(".report_type:checked").val();
        if (report_type == "OutputGSTTaxPercentagewise") {
            $('.OutputGSTTaxPercentagewise').removeClass('uk-hidden');
        }
        else {
            $('.OutputGSTTaxPercentagewise').addClass('uk-hidden');
        }
    },

    get_filters: function () {
        var self = SalesGSTR;
        var filters = "";
        if ($("#GSTPercentage").val() != "") {
            filters += "GST %: " + $("#GSTPercentage Option:selected").text() + ", ";
        }
        if ($("#StateID").val() != "") {
            filters += "State: " + $("#StateID Option:selected").text() + ", ";
        }
        if ($("#FilterLocationID").val() != "") {
            filters += "Location: " + $("#FilterLocationID Option:selected").text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },

    refresh: function () {
        var self = SalesGSTR;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $('#FromDateString').val(findate)
        $('#ToDateString').val(currentdate)
        $('#StateID').val('');
        $('#State').val('');
        $('#GSTPercentage').val('');
        $("#FilterLocationID").val(locationID);
    },
}