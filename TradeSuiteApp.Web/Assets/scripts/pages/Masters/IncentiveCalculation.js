IncentiveCalculation = {

    init: function () {
        var self = IncentiveCalculation;
       
        self.bind_events();
       

    },
    bind_events: function () {
        var self = IncentiveCalculation;
        $("#DurationID").on('change', self.GetTimeList);
        $("body").on("click", "#btn-calculate-incentive", self.show_incentives);
    },


    show_incentives: function () {
        var self = IncentiveCalculation;
        var error_count = self.validate_show_incentives();
        if (error_count > 0) {
            return;
        }
        if ($('#calculatied-incentive-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                self.get_incentives();
            });
        }
        else {
            self.get_incentives();
        }
    },
    get_incentives: function () {
        var length;
        var self = IncentiveCalculation;
        $.ajax({
            url: '/Masters/IncentiveCalculation/GetCalculatedIncentives/',
            dataType: "json",
            data: {
                'DurationID': $("#DurationID").val(),
                'TimePeriodID': $("#TimePeriodID").val(),
                'PartyType': $("#Party").val()
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="PartyName">' + item.PartyName
                        + '<input type="hidden" class="PartyID"value="' + item.PartyID + '"/>'
                        + '</td>'
                        + '<td class="TotalClassicalTarget  mask-currency">' + item.TotalClassicalTarget + '</td>'
                        + '<td class="TotalPatentTarget  mask-currency">' + item.TotalPatentTarget + '</td>'
                        + '<td class="TotalTarget  mask-currency">' + item.TotalTarget + '</td>'
                        + '<td class="TotalAchievedClassicalPercent mask-currency">' + item.TotalAchievedClassicalPercent + '</td>'
                        + '<td class="TotalAchievedPatentPercent mask-currency">' + item.TotalAchievedPatentPercent + '</td>'
                        + '<td class="TotalAchievedPercent mask-currency">' + item.TotalAchievedPercent + '</td>'
                        + '<td class=" mask-currency ClassicalIncentiveAmount ">' + item.ClassicalIncentiveAmount + '</td>'
                        + '<td class="PatentIncentiveAmount  mask-currency">' + item.PatentIncentiveAmount + '</td>'
                        + '<td class="TotalIncentiveAmount  mask-currency">' + item.TotalIncentiveAmount + '</td>'
                        + '<td class="TotalIncentiveAmount  mask-currency">' + item.IncentiveAboveLimit + '</td>'
                        + '<td class="TotalIncentiveAmount  mask-currency">' + item.TotalEligableAmount + '</td>'
                        + '<td class="Extramount  mask-currency">' + item.TotalEligableAmount + '</td>'
                        + '</tr>';

                });
                $content = $(content);
                app.format($content);
                $("#calculatied-incentive-list tbody").html($content);
            },

        });

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




    validate_show_incentives: function () {
        var self = IncentiveCalculation;
        if (self.rules.on_show.length) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },

    rules: {
        on_show: [
               {
                   elements: "#DurationID",
                   rules: [
                       { type: form.required, message: "Please select Duration" },
                       { type: form.non_zero, message: "Please select Duration" },
                       { type: form.positive, message: "Please select Duration" },

                   ],
               },
                 {
                     elements: "#TimePeriodID",
                     rules: [
                         { type: form.required, message: "Please select TimePeriod" },
                         { type: form.non_zero, message: "Please select TimePeriod" },
                         { type: form.positive, message: "Please select TimePeriod" },
                     ],
                 },
                 {
                     elements: "#Party",
                     rules: [
                         { type: form.required, message: "Please select PartyType" },
                     ],
                 },

        ]
    },
}