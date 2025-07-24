$(function () {
    DailyTotalCollection.init();
});

DailyTotalCollection = {
    init: function () {
        var self = DailyTotalCollection;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = DailyTotalCollection;
        $('#Refresh').on('click', self.refresh);
    },
    refresh: function () {
        var self = DailyTotalCollection;
        $('#FromDateString').val('');
        $('#ToDateString').val('');
    },
    get_filters: function () {
        var self = DailyTotalCollection;
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
    validate_form: function () {
        var self = DailyTotalCollection;
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