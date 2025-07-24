$(function () {
    IncentiveReport.init();
});


IncentiveReport = {
    init: function () {
        var self = IncentiveReport;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = IncentiveReport;
        $('#Refresh').on('click', self.refresh);
        $("#DurationID").on('change', self.GetTimeList);
        $('#fsoname-autocomplete').on('selectitem.uk.autocomplete', self.set_fso_name);

    },

    GetTimeList: function () {
        $.ajax({
            url: '/Masters/IncentiveCalculation/GetTimePeriodList/',
            dataType: "json",
            type: "GET",
            data: {
                DurationID: $('#DurationID').val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.Id + "'>" + record.Name + "</option>";
                });
                $("#TimePeriodID").html("");
                $("#TimePeriodID").append(html);
            }
        });
    },

    set_fso_name: function (event, item) {
        var self = IncentiveReport;
        $("#FSOName").val(item.name);
        $("#FSONID").val(item.id);
    },

    refresh: function () {
        var self = IncentiveReport;
        $('#PartyType').val('');
        $('#DurationID').val(0);
        $('#TimePeriodID').val(0);
    },

    get_filters: function () {
        var self = IncentiveReport;
        var filters = "";

        if ($("#PartyType").val() != "") {
            filters += "PartyType: " + $("#PartyType").val() + ", ";
        }
        if ($("#DurationID").val() != 0) {
            filters += "Duration: " + $("#DurationID option:selected").text() + ", ";
        }
        if ($("#TimePeriodID").val() != 0) {
            filters += "TimePeriod: " + $("#TimePeriodID option:selected").text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },

    validate_form: function () {
        var self = IncentiveReport;
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
                 elements: "#PartyType",
                 rules: [
                     { type: form.required, message: "Please Select PartyType" },
                 ]
             },
              {
                  elements: "#DurationID",
                  rules: [
                      { type: form.required, message: "Please Select Duration" },
                  ]
              },
               {
                   elements: "#TimePeriodID",
                   rules: [
                       { type: form.required, message: "Please Select TimePeriod" },
                   ]
               },
        ]
    }
}

