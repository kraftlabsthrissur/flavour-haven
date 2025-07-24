var fh
//to do.Item search
var StockAdjustmentSchedule = {
    init: function () {
        var self = StockAdjustmentSchedule;
        self.bind_events();
        fh = $("#tbl-date-details").FreezeHeader();
    },
    details:function()
    {
        fh = $("#tbl-date-details").FreezeHeader();

    },
    list: function () {
        $batch_list = $('#stock-adjustment-schedule-list');
        $batch_list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/StockAdjustmentSchedule/Details/' + Id;
        });
        if ($batch_list.length) {
            $batch_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/StockAdjustmentSchedule/StockAdjustmentScheduleList";
            var list_table = $batch_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Frequency", "className": "Frequency" },
                    { "data": "NoOfDaysToComplete", "className": "NoOfDaysToComplete" }
                    
                   
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $batch_list.trigger("datatable.changed");
                },
            });
            $batch_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $batch_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $batch_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = StockAdjustmentSchedule;
        $(".btnSave").on('click', self.save);        
        $("body").on("keyup change", "#TimeLimit", self.calculate_limit);
        $("#btnAddDate").on('click', self.add_date);
        $("body").on("click", ".remove-item", self.remove_item);

       
    },
    calculate_limit: function () {
        var TimeLimit = clean($("#TimeLimit").val());
        var ItemCount = clean($("#ItemCount").val());
        var frequency = ItemCount / TimeLimit;
        var temp = frequency;
        frequency = Math.round(frequency);
       var roundoff = temp - frequency;
       if (roundoff > 0)
       {
           frequency = frequency + 1;
       }
        $("#FrequencyOfItem").val(frequency);

    },
    add_date:function()
    {
        var self = StockAdjustmentSchedule;
        self.error_count = self.validate_add();

        if (self.error_count > 0) {
            return;
        }
        var tr;
        var excludeddate = $("#ExcludedDates").val();
        var SerialNo = $("#tbl-date-details tbody tr").length + 1;
        tr= '<tr>' +
               '<td class="uk-text-center ">' + SerialNo + '</td>' +
               '<td class="excludeddate "> ' + excludeddate + '</td>' +
                  ' <td class="uk-text-center">' +
                        '   <a  class="remove-item" >' +
                            '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                            ' </a>' +
                        ' </td>' +
            '</tr>';
        var $tr = $(tr);
        app.format($tr);
        $("#tbl-date-details tbody").append($tr);
        fh.resizeHeader();
        $("#ExcludedDates").val('');
    },
    remove_item: function () {
        var self = StockAdjustmentSchedule;
        $(this).closest("tr").remove();
        $("#tbl-date-details tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
      
        fh.resizeHeader();
    },


    save: function () {
        var self = StockAdjustmentSchedule;
        $("#MorningStartTimeStr").data("date-time", $("#Date").val() + " " + $("#MorningStartTimeStr").val());
        $("#EveningEndTimeStr").data("date-time", $("#Date").val() + " " + $("#EveningEndTimeStr").val());
        $("#MorningEndTimeStr").data("date-time", $("#Date").val() + " " + $("#MorningEndTimeStr").val());
        $("#EveningStartTimeStr").data("date-time", $("#Date").val() + " " + $("#EveningStartTimeStr").val());

        self.error_count = self.validate_schedule();

        if (self.error_count > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/StockAdjustmentSchedule/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Stock Adjustment Scheduled successfully");
                    setTimeout(function () {
                        window.location = "/Masters/StockAdjustmentSchedule/Index";
                    }, 1000);
                } else {
                    app.show_error("failed to save");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = StockAdjustmentSchedule;
        var model = {
            ID: $("#ID").val(),
            ItemCount: clean($("#ItemCount").val()),
            TimeLimit: clean($("#TimeLimit").val()),
            FrequencyOfItem: clean($("#FrequencyOfItem").val()),           
            MorningStartTimeStr: $("#MorningStartTimeStr").val(),
            MorningEndTimeStr: $("#MorningEndTimeStr").val(),
           // EveningStartTimeStr: $("#EveningStartTimeStr").val(),
            //EveningEndTimeStr: $("#EveningEndTimeStr").val(),
            EveningStartTimeStr: $("#MorningStartTimeStr").val(),
            EveningEndTimeStr: $("#MorningEndTimeStr").val(),
            ExcludedDateList:self.Get_date_List()
        }
        return model;
    },
    Get_date_List: function () {
       dates = [];
        var date = {};
        $("#tbl-date-details tbody tr").each(function () {
            date = {};
            date.Date = $(this).find(".excludeddate").text().trim(),
            date.ID =0
            dates.push(date);
        });
        return dates;
    },
        
    validate_schedule: function () {
        var self = StockAdjustmentSchedule;
        if (self.rules.on_schedule.length) {
            return form.validate(self.rules.on_schedule);
        }
        return 0;
    },
    validate_add: function () {
        var self = StockAdjustmentSchedule;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    rules: {
        on_add: [
       {
           elements: "#ExcludedDates",
           rules: [
                { type: form.required, message: "Please select date" },
           ]
       },
        ],
        on_schedule: [
        {
            elements: "#MorningStartTimeStr",
            rules: [
                 { type: form.required, message: "Please enter morning start time" },
            ]
        },
        {
            elements: "#TimeLimit",
            rules: [
                { type: form.non_zero, message: "Please enter time to complete one circle" },
            ]
        },
        {
            elements: "#MorningEndTimeStr",
            rules: [
                { type: form.required, message: "Please enter morning end time" },
                {
                    type: function (element) {
                        var date_time_string1 = $("#MorningStartTimeStr").data("date-time");
                        var date_time_string2 = $(element).data("date-time");
                        var result = app.compare_date_time(date_time_string1, date_time_string2);
                        return result == "greater" ? false : true;
                    }, message: "Morning end time must be greater than start time"
                }
            ]
        },

         {
             elements: "#EveningStartTimeStr",
             rules: [
                 { type: form.required, message: "Please enter evening start time" },
                 {
                     type: function (element) {
                         var date_time_string1 = $("#MorningStartTimeStr").data("date-time");
                         var date_time_string2 = $(element).data("date-time");
                         var result = app.compare_date_time(date_time_string1, date_time_string2);
                         return result == "greater" ? false : true;
                     }, message: "evening start time must be greater than morning start time"
                 }
             ]
         },
        {
            elements: "#EveningEndTimeStr",
            rules: [
                 { type: form.required, message: "Please enter evening end time" },
                   {
                       type: function (element) {
                           var date_time_string1 = $("#EveningStartTimeStr").data("date-time");
                           var date_time_string2 = $(element).data("date-time");
                           var result = app.compare_date_time(date_time_string1, date_time_string2);
                           return result == "greater" ? false : true;
                       }, message: "evening end time must be greater than start time"
                   }
            ]
        },           
        ],
    }
}