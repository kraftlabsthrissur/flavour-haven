var MedicineConsumption = {

    init: function () {
        var self = MedicineConsumption;
        self.bind_events();
    },

    list: function () {
        var self = MedicineConsumption;

        $('#tabs-medicineconsumption').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                var ss = active_item.data('tab');
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = MedicineConsumption;
        var $list;

        switch (type) {
            case "Scheduled":
                $list = $('#scheduled-list');
                break;
            case "Completed":
                $list = $('#completed-list');
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

            var url = "/AHCMS/MedicineConsumption/GetMedicineConsumptionListForDataTable?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "ASC"]],
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
                   { "data": "Time", "className": "Time" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "Medicines", "className": "Medicines" },
                   { "data": "Room", "className": "Room" },
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
        var self = MedicineConsumption;
        $("#btnShow").on("click", self.get_filtered_data);
        $("body").on('ifChanged', '.chkCheck', self.include_item);
        $("body").on("click", ".btnSave", self.save_confirm);
        $('body').on('click', '#medicine-consumption-list tbody tr.included.addmedicine td:not(.action)', self.show_medicine_qty);
        $('body').on('click', '#btnOkMedicines', self.push_medicines);
        $("body").on("ifChanged", ".medicines-check-box", self.include_medicines);
        $("body").on("change", ".Status ", self.add_medicine_class);
    },
    add_medicine_class: function () {
        var self = MedicineConsumption;
        var Status = $(this).closest('tr').find('.Status option:selected').text();
        if (Status == "Completed") {
            $(this).closest('tr').addClass("addmedicine");
            var row = $(this).closest('tr');
            var MedicineConsumptionID = $(row).find('.MedicineConsumptionID').val();
            var PatientMedicinesID = $(row).find('.PatientMedicinesID').val();
            self.show_medicine_qty_edit_modal(MedicineConsumptionID, PatientMedicinesID);
        }
        else {
            $(this).closest('tr').removeClass("addmedicine");
        }

    },

    show_medicine_qty: function () {
        var self = MedicineConsumption;
        var row = $(this).closest('tr');
        var MedicineConsumptionID = $(row).find('.MedicineConsumptionID').val();
        var PatientMedicinesID = $(row).find('.PatientMedicinesID').val();
        self.show_medicine_qty_edit_modal(MedicineConsumptionID, PatientMedicinesID);
    },
    Medicines: [],
    ProductionGroup: [],
    get_filtered_data: function () {
        var self = MedicineConsumption;
        var length;
        var Datestring = $("#Date").val();
        var Time = $("#Time").val();
        var RoomID = $("#RoomID").val();
        var StoreID = $("#StoreID").val();
        $.ajax({
            url: '/AHCMS/MedicineConsumption/MedicineConsumptionList',
            dataType: "html",
            data: {
                Datestring: Datestring,
                StoreID: StoreID,
                RoomID: RoomID,
                Time: Time
            },
            type: "POST",
            success: function (response) {
                $("#medicine-consumption-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#medicine-consumption-list tbody").append($response);
                index = $("#medicine-consumption-list tbody tr .included").length;
                $("#item-count").val(index);
            },
        });
    },

    include_item: function () {
        var self = MedicineConsumption
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }

        index = $("#medicine-consumption-list tbody tr.included").length;
        $("#item-count").val(index);
    },

    show_medicine_qty_edit_modal: function (MedicineConsumptionID, PatientMedicinesID) {
        var self = MedicineConsumption;
        var tr = "";
        var $tr;
        var StoreID = $("#StoreID").val();
        //treatment-schedule-id element used for remove from array with same TreatmentScheduleItemID(used when click ok button of medicine modal)
        $("#patient-medicines-id").val(MedicineConsumptionID);
        $.ajax({
            url: '/AHCMS/MedicineConsumption/MedicinesDataList',
            dataType: "html",
            data: {
                PatientMedicinesID: PatientMedicinesID,
                StoreID: StoreID,
                MedicineConsumptionID: MedicineConsumptionID
            },
            type: "POST",
            success: function (response) {
                $("#medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#medicine-list tbody").append($response);
                if (self.Medicines.length > 0) {
                    var ItemID;
                    var PatientMedicinesID;
                    var MedicineConsumptionID;
                    var Qty;
                    var Rowindex;
                    $('#medicine-list tbody tr').each(function () {
                        ItemID = clean($(this).find(".ItemID").val());
                        MedicineConsumptionID = clean($(this).find(".MedicineConsumptionID").val());
                        //check in medicine list contains data with same item and treatment schedule
                        if (self.Medicines.some(a => a.MedicineConsumptionID == MedicineConsumptionID && a.ItemID == ItemID)) {
                            Qty = self.Medicines.find(x => x.MedicineConsumptionID == MedicineConsumptionID && x.ItemID == ItemID).Qty;
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
        var self = MedicineConsumption;
        self.error_count = 0;
        self.error_count = self.validate_add_medicines();
        if (self.error_count > 0) {
            return;
        }
        var MedicineConsumptionID = $("#patient-medicines-id").val();
        //Remove from Array with same PatientMedicinesID
        self.Medicines = $.grep(self.Medicines, function (element, index) { return element.MedicineConsumptionID != MedicineConsumptionID; });

        var item = {};
        var ProductionGroups = {};
        $('#medicine-list tbody tr.included').each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val());
            item.UnitID = clean($(this).find(".UnitID").val());
            item.PatientMedicinesID = clean($(this).find(".PatientMedicinesID").val());
            item.Qty = clean($(this).find(".Qty").val());
            item.ProductionGroupID = clean($(this).find(".ProductionGroupID").val());
            item.MedicineConsumptionID = clean($(this).find(".MedicineConsumptionID").val());
            self.Medicines.push(item);
        });
        //Push ProductionGroup Of Treatment For Validation
        $('#medicine-list tbody tr').each(function () {
            ProductionGroups = {};
            ProductionGroups.ProductionGroupID = clean($(this).find(".ProductionGroupID").val());
            ProductionGroups.MedicineConsumptionID = clean($(this).find(".MedicineConsumptionID").val());
            //Check in ProductionGroup array dupilcate data
            if (self.ProductionGroup.some(a => a.MedicineConsumptionID == ProductionGroups.MedicineConsumptionID && a.ProductionGroupID == ProductionGroups.ProductionGroupID)) {
            }
            else {
                self.ProductionGroup.push(ProductionGroups);
            }
        });
        UIkit.modal($('#medicine-list')).hide();
    },

    save_confirm: function () {
        var self = MedicineConsumption;
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
        var self = MedicineConsumption;
        var modal = self.get_data();
        $.ajax({
            url: '/AHCMS/MedicineConsumption/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("MedicineConsumption Saved successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/MedicineConsumption/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = MedicineConsumption;
        var data = {};
        data.Items = [];
        data.Medicines = [];
        var item = {};
        $("#medicine-consumption-list tbody tr.included").each(function () {
            item = {};
            item.Date = $(this).find(".Date").val();
            item.MedicineConsumptionID = clean($(this).find(".MedicineConsumptionID").val());
            item.ActiualTime = $(this).find(".ActiualTime").val();
            item.Status = $(this).find(".Status").val();
            data.Items.push(item);
        });
        var MedicineItem = {};
        $(self.Medicines).each(function (i, record) {
            MedicineItem = {};
            MedicineItem.ItemID = record.ItemID,
            MedicineItem.MedicineConsumptionID = record.MedicineConsumptionID,
            MedicineItem.PatientMedicinesID = record.PatientMedicinesID,
            MedicineItem.UnitID = record.UnitID,
            MedicineItem.Qty = record.Qty,
            MedicineItem.StoreID = clean($("#StoreID").val());
            data.Medicines.push(MedicineItem);
        });
        return data;
    },

    set_date_time: function () {
        var self = MedicineConsumption;
        var string_date_time;
        var string_actual_date_time;
        $("#medicine-consumption-list tbody tr.to-be-compared").each(function () {
            var row = $(this).closest('tr');
            var r = $(this).find(".Time").val();
            string_date_time = app.string_to_date($(this).find(".Date").val() + " " + $(this).find(".Time").val());
            string_actual_date_time = app.string_to_date($(this).find(".Date").val() + " " + $(this).find(".ActiualTime").val());
            var dt = new Date();
            var time = dt.getTime();
            $(this).find(".Time").data("date-time", string_date_time);
            $(this).find(".ActiualTime").data("date-time", string_actual_date_time);
        });
    },

    include_medicines: function () {
        var self = MedicineConsumption
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.Qty').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('.Qty').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }
    },

    validate_save: function () {
        var self = MedicineConsumption;

        $('#medicine-consumption-list tbody tr.included').addClass('to-be-compared');
        self.set_date_time();
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    validate_add_medicines: function () {
        var self = MedicineConsumption;
        if (self.rules.on_medicine_add.length) {
            return form.validate(self.rules.on_medicine_add);
        }
        return 0;
    },

    rules: {

        on_save: [

              {
                  elements: ".included .ActiualTime",
                  rules: [
                     { type: form.required, message: "Please Select ActiualTime" },
                     { type: form.non_zero, message: "Please Select ActiualTime" },
                  ]
              },
              {
                  elements: "#item-count",
                  rules: [
                     { type: form.required, message: "Please Select Medicine" },
                     { type: form.non_zero, message: "Please Select Medicine" },
                  ]
              },
              {
                  elements: ".included .Time",
                  rules: [
                       {
                           type: function (element) {

                               $(element).closest("tr").removeClass('to-be-compared');
                               var scheduled_date_time = $(element).closest("tr").find(".Time").data("date-time");
                               var dt = new Date();
                               var current_time = dt.getTime();
                               var error = false;
                               if (scheduled_date_time > current_time) {
                                   error = true
                               }
                               return !error;
                           }, message: "Time Is not Due"
                       },

                  ]
              },

              {
                  elements: ".included .Status ",
                  rules: [
                       { type: form.required, message: "Please Select Status" },
                       {
                           type: function (element) {
                               var MedicineConsumptionID = $(element).closest("tr").find(".MedicineConsumptionID").val();
                               var Status = $(element).find('option:selected').text();
                               var error = false;
                               if (Status == "Completed") {
                                   $(MedicineConsumption.ProductionGroup).each(function (i, record) {
                                       if (error == true) { return }
                                       if (MedicineConsumptionID == record.MedicineConsumptionID) {
                                           if (MedicineConsumption.Medicines.some(a => a.MedicineConsumptionID == record.MedicineConsumptionID && a.ProductionGroupID == record.ProductionGroupID)) {
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
                           }, message: "Add Medicines"
                       },
                       {
                           type: function (element) {
                               var MedicineConsumptionID = $(element).closest("tr").find(".MedicineConsumptionID").val();
                               var Status = $(element).find('option:selected').text();
                               var error = false;
                               if (Status == "Completed") {
                                   if (MedicineConsumption.Medicines.some(a => a.MedicineConsumptionID == MedicineConsumptionID)) {
                                       error = false
                                   }
                                   else {
                                       error = true
                                       return
                                   }
                                   //return !error;
                               } return !error;
                           }, message: "Add Medicines For CompletedStatus"
                       },
                  ]
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
    },
}