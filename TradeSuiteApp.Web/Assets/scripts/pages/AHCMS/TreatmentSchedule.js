TreatmentSchedule = {
    init: function () {
        var self = TreatmentSchedule;
        CounterSales.patient_list();
        $('#appointment-process-list').SelectTable({
            selectFunction: self.select_patient,
            returnFocus: "#btnView",
            modal: "#select-appointment-process",
            initiatingElement: "#PatientName"
        });
        self.bind_events();
        self.hide_and_show_elements();
    },

    hide_and_show_elements: function () {
        var self = TreatmentSchedule
        $("#treatment-schedule-list tbody tr").remove();
        $("#FromDate").val("")
        $("#PatientID").val("");
        $("#AppointmentProcessID").val("");
        $("#PatientName").val("");
        $("#PatientName").val("");
        var type = $("input[name='filtertype']:checked").val();
        $(".DateWise").addClass("uk-hidden");
        $(".PatientWise").addClass("uk-hidden");
        if (type == 'DateWise') {
            $("#PatientID").attr("disabled", "disabled").removeClass("enabled");
            $("#FromDate").removeAttr("disabled").addClass("enabled");
        }
        else {
            $("#PatientID").removeAttr("disabled").addClass("enabled");
            $("#FromDate").attr("disabled", "disabled").removeClass("enabled");
        }
        $("." + type).removeClass("uk-hidden");
        index = $("#treatment-schedule-list tbody tr.included").length;
        $("#item-count").val(index);
    },

    select_patient: function () {
        var self = TreatmentSchedule;
        var length = $("#treatment-schedule-list tbody tr").length;
        var radio = $('#appointment-process-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".PatientName").text().trim();
        var DoctorName = $(row).find(".DoctorName").text().trim();
        var PatientID = clean($(row).find(".PatientID").val());
        var DoctorID = clean($(row).find(".DoctorID").val())


        if (length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                $("#treatment-schedule-list tbody tr").remove();
                $("#item-count").val($("#Treatment-schedule-list tbody tr").length);
                $("#PatientName").val(Name);
                $("#PatientID").val(PatientID);
                $("#AppointmentProcessID").val(ID);
                UIkit.modal($('#select-appointment-process')).hide();
            }, function () {
            })
        }
        else {
            $("#PatientName").val(Name);
            $("#PatientID").val(PatientID);
            $("#AppointmentProcessID").val(ID);
            UIkit.modal($('#select-appointment-process')).hide();
        }
        index = $("#treatment-schedule-list tbody tr.included").length;
        $("#item-count").val(index);
    },

    list: function () {
        var self = TreatmentSchedule;

        $('#tabs-treatmentprocess').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                var ss = active_item.data('tab');
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = TreatmentSchedule;
        var $list;

        switch (type) {
            case "Scheduled":
                $list = $('#scheduled-list');
                break;
            case "Prescribed":
                $list = $('#prescribed-list');
                break;
            case "Completed":
                $list = $('#completed-list');
                break;
            case "Started":
                $list = $('#started-list');
                break;
            case "Paused":
                $list = $('#paused-list');
                break;
            case "Cancelled":
                $list = $('#cancelled-list');
                break;
            default:
                $list = $('#prescribed-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/TreatmentSchedule/GetTreatmentScheduleList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "Type", Value: type },
                        ];
                    }
                },
                "aoColumns": [
                   {
                       "data": null,
                       "className": "uk-text-center",
                       "searchable": false,
                       "orderable": false,
                       "render": function (data, type, row, meta) {
                           return (meta.settings.oAjaxData.start + meta.row + 1)
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "Date", "className": "Date" },
                   { "data": "StartTime", "className": "StartTime" },
                   { "data": "EndTime", "className": "EndTime" },
                   { "data": "Treatment", "className": "Treatment" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "Therapist", "className": "Therapist" },
                   { "data": "Medicines", "className": "Medicines" },
                   { "data": "TreatmentRoom", "className": "TreatmentRoom" },
                   { "data": "Status", "className": "Status" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                },
            });
            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    bind_events: function () {
        var self = TreatmentSchedule;
        $("#btnView").on("click", self.get_filter);
        $("body").on("click", ".btnSave", self.save_confirm);


        $('.filtertype').on("ifChecked", self.hide_and_show_elements)
        $("#btn-ok-appointment-process").on("click", self.select_patient);
        $("body").on('ifChanged', '.chkCheck', self.include_item);

    },

    get_filter: function () {
        var self = TreatmentSchedule;
        var Type = $("input[name='filtertype']:checked").val();
        if (self.validate_on_add() > 0) {
            return;
        }
        var length;
        var FromDate = $("#FromDate").val();
        if (FromDate == ""){
            FromDate = null;
        }
        var PatientID = $("#PatientID").val();
        var AppointmentProcessID = $("#AppointmentProcessID").val();
        $.ajax({
            url: '/AHCMS/TreatmentSchedule/ScheduledTreatmentList',
            dataType: "html",
            data: {
                Type: Type,
                FromDate: FromDate,
                PatientID: PatientID,
                AppointmentProcessID: AppointmentProcessID
            },
            type: "POST",
            success: function (response) {
                $("#treatment-schedule-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#treatment-schedule-list tbody").append($response);
                index = $("#treatment-schedule-list tbody tr.included").length;
                $("#item-count").val(index);
                if ($("#treatment-schedule-list tbody tr").length <= 0) {
                    app.show_error("No Treatments Exist");
                }
            },
        })
    },

    include_item: function () {
        var self = TreatmentSchedule;

        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            if ($("input[name='filtertype']:checked").val() == 'DateWise') {

                $(this).closest('tr').find('.StartDate').attr("readonly", 'readonly');
                $(this).closest('tr').find('.StartDate').attr("disabled", true);
            }
        } else {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').attr("disabled", true);
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').find('.StartDate').prop("readonly", true);
        }
        index = $("#treatment-schedule-list tbody tr.included").length;
        $("#item-count").val(index);
        //self.count_items();
    },

    get_data: function () {
        var self = TreatmentSchedule;
        var data = {};
        data.Items = [];
        var item = {};
        $("#treatment-schedule-list tbody tr.included").each(function () {
            item = {};
            item.ScheduleID = clean($(this).find(".ScheduleID").val());
            item.ScheduledDate = $(this).find(".StartDate").val();
            item.TreatmentRoomID = clean($(this).find(".TreatmentRoomID option:selected").val());
            item.StartTime = $(this).find(".StartTime").val();
            item.EndTime = $(this).find(".EndTime").val();
            item.DurationID = clean($(this).find(".DurationID option:selected").val());
            item.TherapistID = clean($(this).find(".TherapistID option:selected").val());
            data.Items.push(item);
        });
        return data;
    },

    save: function () {
        var self = TreatmentSchedule;
        var modal = self.get_data();
        $.ajax({
            url: '/AHCMS/TreatmentSchedule/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("TreatmentSchedule created successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/TreatmentSchedule/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    save_confirm: function () {
        var self = TreatmentSchedule;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    set_date_time: function () {
        var self = TreatmentSchedule;
        var string_start_date_time;
        var string_end_date_time;
        $("#treatment-schedule-list tbody tr.to-be-compared").each(function () {
            var row = $(this).closest('tr');
            var duration = $(row).find(".DurationID option:selected").text();
            var durationnum = duration.split(" ");
            string_start_date_time = app.string_to_date($(this).find(".StartDate").val() + " " + $(this).find(".StartTime").val());
            string_end_date_time = string_start_date_time + durationnum[0] * 60 * 1000;

            $(this).find(".StartTime").data("date-time", string_start_date_time);
            $(this).find(".EndTime").data("date-time", string_end_date_time);
        });
    },


    validate_save: function () {
        var self = TreatmentSchedule;

        $('#treatment-schedule-list tbody tr.included').addClass('to-be-compared');
        $('#treatment-schedule-list tbody tr:contains("Scheduled")').removeClass('to-be-compared');
        $('#treatment-schedule-list tbody tr:contains("Scheduled")').addClass('to-be-compared');

        $("#treatment-schedule-list tbody tr").removeClass('conflict-color')
        $("#treatment-schedule-list tbody tr input").removeClass('conflict-color')
        $("#treatment-schedule-list tbody tr Select").removeClass('conflict-color')
        self.set_date_time();
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_on_add: function () {
        var self = TreatmentSchedule;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
             {
                 elements: "#FromDate.enabled",
                 rules: [
                     { type: form.required, message: "Please Select FromDate" },
                     { type: form.non_zero, message: "Please Select FromDate" },

                 ]
             },
             {
                 elements: "#PatientID.enabled",
                 rules: [
                    { type: form.required, message: "Please choose Patient", alt_element: "#PartyName" },
                    { type: form.positive, message: "Please choose Patient", alt_element: "#PartyName" },
                    { type: form.non_zero, message: "Please choose Patient", alt_element: "#PartyName" }
                 ]
             },
        ],
        on_save: [

              {
                  elements: ".included .DurationID",
                  rules: [
                     { type: form.required, message: "Please Select Duration" },
                     { type: form.non_zero, message: "Please Select Duration" },
                  ]
              },
              {
                  elements: "#item-count",
                  rules: [
                     { type: form.required, message: "Please Schedule Treatment" },
                     { type: form.non_zero, message: "Please Schedule Treatment" },
                  ]
              },
              {
                  elements: ".included .StartTime",
                  rules: [
                       { type: form.required, message: "Please Select Time" },
                       { type: form.non_zero, message: "Please Select Time" },
                       {
                           type: function (element) {

                               $(element).closest("tr").removeClass('to-be-compared');
                               var start_date_time = $(element).closest("tr").find(".StartTime").data("date-time");
                               var end_date_time = $(element).closest("tr").find(".EndTime").data("date-time");
                               var therapistID = $(element).closest("tr").find(".TherapistID").val();
                               var roomID = $(element).closest("tr").find(".TreatmentRoomID").val();
                               var error = false;
                               $('#treatment-schedule-list tbody tr.to-be-compared').each(function () {
                                   var current_start_date_time = $(this).find('.StartTime').data("date-time");
                                   var current_end_date_time = $(this).find('.EndTime').data("date-time");
                                   var current_therapistID = $(this).find('.TherapistID').val();
                                   var current_roomID = $(this).find('.TreatmentRoomID').val();
                                   if (current_roomID == roomID) {
                                       if (!((start_date_time <= current_start_date_time && end_date_time <= current_start_date_time) ||
                                           (start_date_time >= current_end_date_time && end_date_time >= current_end_date_time))) {
                                           $(element).closest("tr").addClass('conflict-color')
                                           $(this).closest("tr").addClass('conflict-color')
                                           $(element).closest("tr").find("input").addClass('conflict-color')
                                           $(this).closest("tr").find("input").addClass('conflict-color')
                                           $(element).closest("tr").find("Select").addClass('conflict-color')
                                           $(this).closest("tr").find("Select").addClass('conflict-color')
                                           error = true;
                                           return;
                                       }
                                   }
                               });
                               return !error;
                           }, message: "Conflict Occured In TreatmentRoom"
                       },
                       {
                           type: function (element) {

                               $(element).closest("tr").removeClass('to-be-compared');
                               var start_date_time = $(element).closest("tr").find(".StartTime").data("date-time");
                               var end_date_time = $(element).closest("tr").find(".EndTime").data("date-time");
                               var therapistID = $(element).closest("tr").find(".TherapistID").val();
                               var roomID = $(element).closest("tr").find(".TreatmentRoomID").val();
                               var error = false;
                               $('#treatment-schedule-list tbody tr.to-be-compared').each(function () {
                                   var current_start_date_time = $(this).find('.StartTime').data("date-time");
                                   var current_end_date_time = $(this).find('.EndTime').data("date-time");
                                   var current_therapistID = $(this).find('.TherapistID').val();
                                   var current_roomID = $(this).find('.TreatmentRoomID').val();
                                   if (current_therapistID == therapistID) {
                                       if (!((start_date_time <= current_start_date_time && end_date_time <= current_start_date_time) ||
                                           (start_date_time >= current_end_date_time && end_date_time >= current_end_date_time))) {
                                           $(element).closest("tr").addClass('conflict-color')
                                           $(this).closest("tr").addClass('conflict-color')
                                           $(element).closest("tr").find("input").addClass('conflict-color')
                                           $(this).closest("tr").find("input").addClass('conflict-color')
                                           $(element).closest("tr").find("Select").addClass('conflict-color')
                                           $(this).closest("tr").find("Select").addClass('conflict-color')
                                           error = true;
                                           return;
                                       }
                                   }
                               });
                               return !error;
                           }, message: "Conflict Occured For Therapist"
                       },
                       {
                           type: function (element) {

                               $(element).closest("tr").removeClass('to-be-compared');
                               var start_date_time = $(element).closest("tr").find(".StartTime").data("date-time");
                               var end_date_time = $(element).closest("tr").find(".EndTime").data("date-time");
                               var therapistID = $(element).closest("tr").find(".TherapistID").val();
                               var patientID = $(element).closest("tr").find(".PatientID").val();
                               var error = false;
                               $('#treatment-schedule-list tbody tr.to-be-compared').each(function () {
                                   var current_start_date_time = $(this).find('.StartTime').data("date-time");
                                   var current_end_date_time = $(this).find('.EndTime').data("date-time");
                                   var current_therapistID = $(this).find('.TherapistID').val();
                                   var current_patientID = $(this).find('.PatientID').val();
                                   if (current_patientID == patientID) {
                                       if (!((start_date_time <= current_start_date_time && end_date_time <= current_start_date_time) ||
                                           (start_date_time >= current_end_date_time && end_date_time >= current_end_date_time))) {
                                           $(element).closest("tr").addClass('conflict-color')
                                           $(this).closest("tr").addClass('conflict-color')
                                           $(element).closest("tr").find("input").addClass('conflict-color')
                                           $(this).closest("tr").find("input").addClass('conflict-color')
                                           $(element).closest("tr").find("Select").addClass('conflict-color')
                                           $(this).closest("tr").find("Select").addClass('conflict-color')
                                           error = true;
                                           return;
                                       }
                                   }
                               });
                               return !error;
                           }, message: "Conflict Occured In Patient"
                       },
                  ]
              },
        ]
    },


}