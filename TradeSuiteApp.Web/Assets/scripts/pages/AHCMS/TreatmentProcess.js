TreatmentProcess = {
    init: function () {
        var self = TreatmentProcess;
        self.check();
        self.bind_events();

    },
    Medicines: [],
    ProductionGroup: [],
    list: function () {
        var self = TreatmentProcess;

        $('#tabs-treatmentprocess').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                var ss = active_item.data('tab');
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = TreatmentProcess;
        var $list;

        switch (type) {
            case "Scheduled":
                $list = $('#scheduled-list');
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
                $list = $('#scheduled-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/TreatmentProcess/GetTreatmentProcessList?type=" + type;

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
        var self = TreatmentProcess;
        $("#btnShow").on("click", self.get_filtered_data);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on("ifChanged", ".check-box", self.check);
        $("body").on("change", ".DurationID", self.set_end_time);
        $("body").on("change", ".StartTime", self.set_end_time);
        $('body').on('click', '#Treatment-issue-list tbody tr.included.addmedicine td:not(.action)', self.show_medicine_qty);
        $("body").on("ifChanged", ".medicines-check-box", self.include_medicines);
        $('body').on('click', '#btnOkMedicines', self.push_medicines);
        $("body").on("change", ".TreatmentStatus ", self.add_medicine_class);
        //$("body").on("change", ".BatchID", self.set_medicine_stock);
    },
    add_medicine_class: function () {
        var self = TreatmentProcess;
        var Status = $(this).closest('tr').find('.TreatmentStatus option:selected').text();
        if (Status == "Completed")
        {
            var row = $(this).closest('tr');
            var TreatmentScheduleItemID = $(row).find('.TreatmentScheduleItemID').val();
            var TreatmentProcessID = $(row).find('.TreatmentProcessID').val();
            self.show_medicine_qty_edit_modal(TreatmentScheduleItemID, TreatmentProcessID);
            $(this).closest('tr').addClass("addmedicine");
            
        }
        else {
            $(this).closest('tr').removeClass("addmedicine");
        }

    },

    show_medicine_qty: function () {
        var self = TreatmentProcess;
        var row = $(this).closest('tr');
        var TreatmentScheduleItemID = $(row).find('.TreatmentScheduleItemID').val();
        var TreatmentProcessID = $(row).find('.TreatmentProcessID').val();
        self.show_medicine_qty_edit_modal(TreatmentScheduleItemID, TreatmentProcessID);
    },
    //set_medicine_stock: function () {
    //    var self = TreatmentProcess;
    //    var row = $(this).closest('tr');
    //    var BatchID = $(row).find(".BatchID").val();
    //    var ItemID = $(row).find(".ItemID").val();
    //    var TreatmentScheduleID = clean($(this).find(".TreatmentScheduleID").val());
    //    $.ajax({
    //        url: '/AHCMS/TreatmentProcess/GetItemStockByBatchID',
    //        dataType: "json",
    //        data: {
    //            ItemID: ItemID,
    //            BatchID: BatchID,
    //            TreatmentScheduleID: TreatmentScheduleID
    //        },
    //        type: "POST",
    //        success: function (response) {
                
    //        },
    //    });
    //    //$(row).find(".DiscountPercentage").val(data);
    //},
    show_medicine_qty_edit_modal: function (TreatmentScheduleItemID, TreatmentProcessID) {
        var self = TreatmentProcess;
        var tr = "";
        var $tr;
        var row = $(this).closest('tr');
        //treatment-schedule-id element used for remove from array with same TreatmentScheduleItemID(used when click ok button of medicine modal)
        $("#treatment-schedule-id").val(TreatmentScheduleItemID);
        $.ajax({
            url: '/AHCMS/TreatmentProcess/TreatmentMedicineDataList',
            dataType: "html",
            data: {
                TreatmentScheduleID: TreatmentScheduleItemID,
                TreatmentProcessID: TreatmentProcessID
            },
            type: "POST",
            success: function (response) {
                $("#medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#medicine-list tbody").append($response);
                if (self.Medicines.length > 0) {
                    var ItemID;
                    var TreatmentScheduleID;
                    var Qty;
                    var Rowindex;
                    $('#medicine-list tbody tr').each(function () {
                        ItemID = clean($(this).find(".ItemID").val());
                        TreatmentScheduleID = clean($(this).find(".TreatmentScheduleID").val());
                        //check in medicine list contains data with same item and treatment schedule
                        if (self.Medicines.some(a => a.TreatmentScheduleID == TreatmentScheduleID && a.ItemID == ItemID)) {
                            Qty = self.Medicines.find(x => x.TreatmentScheduleID == TreatmentScheduleID && x.ItemID == ItemID).Qty;
                            $(this).find(".Qty").val(Qty);
                            Qty = clean($(this).find(".Qty").val(Qty));

                            $(this).find('.Qty').addClass('included').removeAttr('disabled');
                            $(this).addClass('included');
                            $(this).find(".medicines-check-box").prop("checked", true)
                            $(this).find(".medicines-check-box").closest('div').addClass("checked")
                        }
                        else {
                            $(this).find('.Qty').removeClass('included').attr('disabled', 'disabled');
                            $(this).removeClass('included');
                        }
                    });
                }
                $('#show-medicine-list').trigger('click');
            }
        });

    },

    push_medicines: function () {
        var self = TreatmentProcess;
        self.error_count = 0;
        self.error_count = self.validate_add_medicines();
        if (self.error_count > 0) {
            return;
        }
        var TreatmentScheduleID = $("#treatment-schedule-id").val();
        //Remove from Array with same TreatmentScheduleID
        self.Medicines = $.grep(self.Medicines, function (element, index) { return element.TreatmentScheduleID != TreatmentScheduleID; });
        
        var item = {};
        var ProductionGroups = {};
        $('#medicine-list tbody tr.included').each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val());
            item.UnitID = clean($(this).find(".UnitID").val());
            item.TreatmentProcessID = clean($(this).find(".TreatmentProcessID").val());
            item.Qty = clean($(this).find(".Qty").val());
            item.TreatmentScheduleID = clean($(this).find(".TreatmentScheduleID").val());
            item.ProductionGroupID = clean($(this).find(".ProductionGroupID").val());
            self.Medicines.push(item);
        });
        //Push ProductionGroup Of Treatment For Validation
        $('#medicine-list tbody tr').each(function () {
            ProductionGroups = {};
            ProductionGroups.ProductionGroupID = clean($(this).find(".ProductionGroupID").val());
            ProductionGroups.TreatmentScheduleID = clean($(this).find(".TreatmentScheduleID").val());
            //Check in ProductionGroup array dupilcate data
            if (self.ProductionGroup.some(a => a.TreatmentScheduleID == ProductionGroups.TreatmentScheduleID && a.ProductionGroupID == ProductionGroups.ProductionGroupID)) {
            }
            else {
                self.ProductionGroup.push(ProductionGroups);
            }
        });
        UIkit.modal($('#medicine-list')).hide();
    },

    Load_All_DropDown: function () {
        var self = TreatmentProcess;
        $.ajax({
            url: '/AHCMS/TreatmentProcess/GetDropDownDetails',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    TreatmentStatusList = response.data;
                }
            }
        });
    },

    Build_Select_TreatmentStatus: function (options, selected) {
        var self = TreatmentProcess;
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';
        $select.append($option);
        for (var i = 0; i < options.length; i++) {
            $option = '<option ' + ((selected == options[i].TreatmentStatus) ? 'selected="selected"' : '') + ' value="' + options[i].TreatmentStatusID + '">' + options[i].TreatmentStatus + '</option>';
            $select.append($option);
        }

        return $select.html();

    },

    set_end_time: function () {
        var self = TreatmentProcess;
        var row = $(this).closest('tr');
        var EndTime;
        var duration = $(row).find("option:selected").text();
        var start_time = $(row).find(".StartTime").val();
        var splitTime = start_time.replace(" ", ":").split(":");
        var durationnum = duration.split(" ");
        var t = new Date();

        t.setHours(splitTime[0]); // convert in to 24 hr format
        t.setMinutes(splitTime[1]);
        var a = new Date(t.getTime() + durationnum[0] * 60000)// 1000 milli seconds into 60 seconds  
        var hour = a.getHours();
        var min = a.getMinutes();
        if (min == 0) {
            min = min + "0";
        }
        if (hour > 12) {
            EndTime = (hour - 12) + ":" + min + " PM";

        }
        else if (hour == 12) {
            EndTime = "12" + ":" + min + " PM";
        }
        else {
            EndTime = (hour) + ":" + min + " AM";
        }

        $(row).find(".EndTime").text(EndTime);

    },

    get_filtered_data: function () {
        var self = TreatmentProcess;
        var length;
        var fromDate = $("#Date").val();
        $.ajax({
            url: '/AHCMS/TreatmentProcess/ScheduleList',
            dataType: "html",
            data: {
                Date: fromDate,
            },
            type: "POST",
            success: function (response) {
                $("#Treatment-issue-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#Treatment-issue-list tbody").append($response);
                index = $("#Treatment-issue-list tbody tr .included").length;
                $("#item-count").val(index);
            },
        });
    },

    save_confirm: function () {
        var self = TreatmentProcess;
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

    save: function () {
        var self = TreatmentProcess;
        var modal = self.get_data();
        $.ajax({
            url: '/AHCMS/TreatmentProcess/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("TreatmentSchedule created successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/TreatmentProcess/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = TreatmentProcess;
        var data = {};
        data.Date = $("#Date").val();
        data.Items = [];
        data.Medicines = [];
        var item = {};
        $('#Treatment-issue-list tbody tr.included').each(function () {
            item = {};
            item.PatientID = clean($(this).find(".PatientID").val());
            item.TreatmentScheduleItemID = clean($(this).find(".TreatmentScheduleItemID").val());
            item.AppointmentProcessID = clean($(this).find(".AppointmentProcessID").val());
            item.TreatmentProcessID = clean($(this).find(".TreatmentProcessID").val());
            item.TreatmentID = clean($(this).find(".TreatmentID").val());
            item.NoOfTreatment = clean($(this).find(".NoOfTreatment").text());
            item.TreatmentRoomID = clean($(this).find(".TreatmentRoomID").val());
            item.StartTime = $(this).find(".time15").val();
            item.EndTime = $(this).find(".EndTime").text().trim();
            item.DurationID = clean($(this).find(".DurationID").val());
            item.TherapistID = clean($(this).find(".TherapistID").val());
            item.Remarks = $(this).find(".remarks").val();
            item.StatusID = clean($(this).find(".TreatmentStatus option:selected").val());
            item.Status = $(this).find(".TreatmentStatus option:selected").text();
            item.Date = $("#Date").val();
            item.TotalTreatmentNo = $(this).find(".TreatmentNo").val();
            data.Items.push(item);
        });
        var MedicineItem = {};
        $(self.Medicines).each(function (i, record) {
            MedicineItem = {};
            MedicineItem.ItemID=record.ItemID,
            MedicineItem.TreatmentProcessID = record.TreatmentProcessID,
            MedicineItem.TreatmentScheduleID = record.TreatmentScheduleID,
            MedicineItem.UnitID = record.UnitID,
            MedicineItem.Qty = record.Qty
            data.Medicines.push(MedicineItem);
        });
        return data;
    },

    check: function () {
        var self = TreatmentProcess
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }


        self.count();
    },

    include_medicines: function () {
        var self = TreatmentProcess
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.Qty').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('.Qty').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }


        self.count();
    },

    set_date_time: function () {
        var self = TreatmentProcess;
        var string_start_date_time;
        var string_end_date_time;
        $("#Treatment-issue-list tbody tr.to-be-compared").each(function () {
            var row = $(this).closest('tr');
            var duration = $(row).find(".DurationID option:selected").text();
            var durationnum = duration.split(" ");
            string_start_date_time = app.string_to_date($("#Date").val() + " " + $(this).find(".StartTime").val());
            string_end_date_time = string_start_date_time + durationnum[0] * 60 * 1000;

            $(this).find(".StartTime").data("date-time", string_start_date_time);
            $(this).find(".EndTime").data("date-time", string_end_date_time);
        });
    },

    validate_save: function () {
        var self = TreatmentProcess;
        $('#Treatment-issue-list tbody tr.included').addClass('to-be-compared');

        $("#Treatment-issue-list tbody tr").removeClass('conflict-color')
        $("#Treatment-issue-list tbody tr input").removeClass('conflict-color')
        $("#Treatment-issue-list tbody tr Select").removeClass('conflict-color')
        self.set_date_time();
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_add_medicines: function () {
        var self = TreatmentProcess;
        if (self.rules.on_medicine_add.length) {
            return form.validate(self.rules.on_medicine_add);
        }
        return 0;
    },

    count: function () {
        var self = TreatmentProcess;
        index = $("#Treatment-issue-list  tbody tr.included").length;
        $("#item-count").val(index);
    },

    rules: {
        on_add: [
             {
                 elements: "#FromDate",
                 rules: [
                     { type: form.required, message: "Please Select FromDate" },
                     { type: form.non_zero, message: "Please Select FromDate" },

                 ]
             },
            {
                elements: "#ToDate",
                rules: [
                    { type: form.required, message: "Please Select ToDate" },
                    { type: form.non_zero, message: "Please Select ToDate" },
                ],
            },
        ],

        on_medicine_add: [
             {
                 elements: ".included .Qty ",
                 rules: [
                     { type: form.required, message: "Please Enter Quantity" },
                     { type: form.non_zero, message: "Please Enter Quantity" },
                     {
                         type: function (element) {
                             var error = false;

                             var Stock = clean($(element).closest("tr.included").find(".Stock").val());
                             var Qty = clean($(element).val());
                             if (Qty > Stock) {
                                 error = true;
                             }
                             return !error;
                         }, message: "Out Of Stock"
                     },
                 ]
             },
             
           
        ],
        on_save: [

                {
                    elements: "#item-count",
                    rules: [
                        { type: form.non_zero, message: "Please add atleast one item" },
                       { type: form.required, message: "Please add atleast one item" },

                    ]
                },

                {
                    elements: ".included .DurationID",
                    rules: [
                       { type: form.required, message: "Please Select Duration" },
                       { type: form.non_zero, message: "Please Select Duration" },
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
                                 var error = false;
                                 $('#Treatment-issue-list tbody tr.to-be-compared').each(function () {
                                     var current_start_date_time = $(this).find('.StartTime').data("date-time");
                                     var current_end_date_time = $(this).find('.EndTime').data("date-time");
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
                                 });
                                 return !error;
                             }, message: "Conflict Occured In StartTime"
                         },
                    ]
                },

                {
                    elements: ".included .TreatmentStatus ",
                    rules: [
                         { type: form.required, message: "Please Select TreatmentStatus" },
                         {
                             type: function (element) {
                                 var TreatmentScheduleID = $(element).closest("tr").find(".TreatmentScheduleItemID").val();
                                 var TreatmentStatus = $(element).find('option:selected').text();
                                 var error = false;
                                 if (TreatmentStatus == "Completed") {
                                     $(TreatmentProcess.ProductionGroup).each(function (i, record) {
                                         if(error==true){return}
                                         if (TreatmentScheduleID == record.TreatmentScheduleID) {
                                             if (TreatmentProcess.Medicines.some(a => a.TreatmentScheduleID == record.TreatmentScheduleID && a.ProductionGroupID == record.ProductionGroupID))
                                                 {
                                                     error = false
                                                 }
                                             else {
                                                 error = true
                                                 return
                                             }
                                         }
                                         
                                     });
                                     //return !error;
                                 } return !error;
                             }, message: "Add Medicines For CompletedTreatment"
                         },

                         {
                             type: function (element) {
                                 var TreatmentScheduleID = $(element).closest("tr").find(".TreatmentScheduleItemID").val();
                                 var TreatmentStatus = $(element).find('option:selected').text();
                                 var error = false;
                                 if (TreatmentStatus == "Completed") {
                                     if (TreatmentProcess.Medicines.some(a => a.TreatmentScheduleID == TreatmentScheduleID)) {
                                         error = false
                                     }
                                     else {
                                         error = true
                                         return
                                     }
                                     //return !error;
                                 } return !error;
                             }, message: "Add Medicines For CompletedTreatment"
                         },
                    ]
                },
        ]
    },

}