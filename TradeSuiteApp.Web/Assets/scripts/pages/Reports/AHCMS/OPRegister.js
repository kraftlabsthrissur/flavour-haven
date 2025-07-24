$(function () {
    OPRegister.init();
});

OPRegister = {
    init: function () {
        var self = OPRegister;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = OPRegister;
        $('#Refresh').on('click', self.refresh);
    },
    refresh: function () {
        var self = OPRegister;
        $('#StartDate').val('');
        $('#EndDate').val('');
    },
    get_filters: function () {
        var self = OPRegister;
        var filters = "";
        if ($("#StartDate").val() != " ") {
            filters += "From Date: " + $("#StartDate").val() + ", ";
        }
        if ($("#EndDate").val() != 0) {
            filters += "To Date: " + $("#EndDate").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
    validate_form: function () {
        var self = OPRegister;
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