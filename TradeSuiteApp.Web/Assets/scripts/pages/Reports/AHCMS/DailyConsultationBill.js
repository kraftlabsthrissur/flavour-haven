$(function () {
    DailyConsultationBill.init();
});

DailyConsultationBill = {
    init: function () {
        var self = DailyConsultationBill;
        self.bind_events();
        ReportHelper.init();
        self.get_report_type();
    },

    bind_events: function () {
        var self = DailyConsultationBill;
        $('#Refresh').on('click', self.refresh);
        $('.cf_report_type').on('ifChanged', self.get_report_type);
    },
    get_report_type: function () {
        var self = DailyConsultationBill;   
        var report_type = $(".cf_report_type:checked").val();
        if(report_type == "Consultation Fee")
        {
            $('.ConsultationFee').addClass('uk-hidden');
        }else
        {
            $('.ConsultationFee').removeClass('uk-hidden');
        }

    },
    refresh: function () {
        var self = DailyConsultationBill;
        $('#DoctorID').val('');
    },
    get_filters: function () {
        var self = DailyConsultationBill;
        var filters = "";
        if ($("#FromDateString").val() != " ") {
            filters += "From Date: " + $("#FromDateString").val() + ", ";
        }
        if ($("#ToDateString").val() != 0) {
            filters += "To Date: " + $("#ToDateString").val() + ", ";
        }
        if ($("#DoctorID").val() != 0) {
            filters += "Doctor Name: " + $("#DoctorID").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
    validate_form: function () {
        var self = DailyConsultationBill;
        self.error_count = 0;
        if (self.error_count > 0) {
            return;
        }
        if (self.rules.on_show.length) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },

    rules: {
        on_show: [
             {
                 elements: "#FromDateString:visible",
                 rules: [
                     { type: form.required, message: "FromDate Is Required" },
                 ]
             },

              {
                  elements: "#ToDateString:visible",
                  rules: [
                      { type: form.required, message: "ToDate Is Required" },
                  ]
              },
        ]
    }
}