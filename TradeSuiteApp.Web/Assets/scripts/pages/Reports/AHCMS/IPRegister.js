$(function () {
    IPRegister.init();
});

IPRegister = {
    init: function () {
        var self = IPRegister;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = IPRegister;
        $('#Refresh').on('click', self.refresh);
    },
    refresh: function () {
        var self = IPRegister;
        $('#StartDate').val('');
        $('#EndDate').val('');
    },
    get_filters: function () {
        var self = IPRegister;
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
        var self = IPRegister;
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