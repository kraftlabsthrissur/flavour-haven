IPCaseSheet.XrayTest = [];
IPCaseSheet.XrayItemList = [];
IPCaseSheet.XrayItemList2 = [];
IPCaseSheet.init = function () {
    var self = IPCaseSheet;
    self.get_patient_data();
    self.get_history();
    self.bind_events();
    var IsDischarged = $("#IsDischarged").val();
    if (IsDischarged == "false") {
        self.include_discharge_summary();
    }
    self.lablist();
    self.xraylist();
    //$('#labtest-list').SelectTable({
    //    modal: "#select-labtest",
    //    initiatingElement: "#PartyName"
    //});

    employee_list = Employee.employee_list();
    item_select_table = $('#employee-list').SelectTable({
        selectFunction: self.select_employee,
        returnFocus: "#ItemName",
        modal: "#select-employee",
        initiatingElement: "#EmployeeName"
    });
};
IPCaseSheet.bind_events= function () {
    var self = IPCaseSheet;
    $("#btnAddMedicine").on("click", self.show_add_medicine);
    $("body").on("click", "#btn_add_internal_medicine", self.show_add_internal_medicine);
    $("#btnmedicines").on("click", self.select_medicine);
    $("#btnaddreport").on("click", self.show_add_report);
    UIkit.uploadSelect($("#select-quotation"), self.selected_report_settings);
    $('body').on('click', 'a.remove-quotation', self.remove_report);
    $(".btnSave").on('click', self.save_confirm);
    $("#btnSaveReport").on('click', self.add_report);
    $("body").on("click", ".edit_report", self.edit_report);
    $("body").on("click", ".remove-item", self.delete_item);
    $("#btnaddtreatment").on("click", self.show_add_treatment);
    $.UIkit.autocomplete($('#medicine-autocomplete'), Config.get_treatment_Item);
    $.UIkit.autocomplete($('#labtest-autocomplete'), Config.get_labitems);
    $.UIkit.autocomplete($('#treatment-autocomplete'), Config.get_treatments);
    $.UIkit.autocomplete($('#medicinename-autocomplete'), Config.get_treatment_medicineList);
    $.UIkit.autocomplete($('#internal_medicine_autocomplete'), Config.get_internal_medicine);
    $.UIkit.autocomplete($('#physiotherapy-autocomplete'), Config.physiotherapy_item);
    $.UIkit.autocomplete($('#xray-autocomplete'), Config.xray_item);
    $('#medicine-autocomplete').on('selectitem.uk.autocomplete', self.set_medicine);
    $('#internal_medicine_autocomplete').on('selectitem.uk.autocomplete', self.set_external_medicine_name);
    $('#labtest-autocomplete').on('selectitem.uk.autocomplete', self.set_lab_test);
    $('#treatment-autocomplete').on('selectitem.uk.autocomplete', self.set_treatment);
    $('#medicinename-autocomplete').on('selectitem.uk.autocomplete', self.set_medicine_name);
    $('#physiotherapy-autocomplete').on('selectitem.uk.autocomplete', self.set_physiotherapy);
    $('#xray-autocomplete').on('selectitem.uk.autocomplete', self.set_xray);
    $("#btnAddTreatmentMedicines").on("click", self.add_treatment_medicine);
    $("body").on("click", ".remove-medicine", self.delete__treatment_medicine);
    $("#btnAddTreatment").on("click", self.add_treatments);
    $("#NoofTreatment").on('keyup', self.calculate_treatment_days);
    $("body").on("click", ".edit_treatment", self.edit_treatment);
    $("#btnAddmedicines").on('click', self.add_medicine);
    $("#btnSaveMadicine").on('click', self.add_medicines_on_grid);
    $("body").on("click", "#btnfilter", self.get_patient_data);
    $("body").on("click", ".remove-medicines", self.delete_medicine);
    $("body").on("click", ".remove-medicine_ongrid", self.delete_medicine_on_grid);
    $("body").on("click", ".view_report", self.report_view);
    $("body").on("click", ".edit_medicines", self.edit_medicines);
    $("body").on("click", ".remove-treatment", self.delete_treatment_on_grid);
    $("body").on("keyup change", "#NoofDays", self.calculate_days);
    $("body").on("ifChanged", ".Morning", self.check_morning);
    $("body").on("ifChanged", ".Morningbox", self.check_morning_medicine);
    $("body").on("ifChanged", ".Noonbox", self.check_noon_medicine);
    $("body").on("ifChanged", ".Noon", self.check_noon);
    $("body").on("ifChanged", ".Eveningbox", self.check_evening_time);
    $("body").on("ifChanged", ".Evening", self.check_evening);
    $("body").on("ifChanged", ".Night", self.check_night);
    $("body").on("ifChanged", ".Nightbox", self.check_night_time);
    $("body").on("click", "#btn_add_vital", self.add_vita_chart);
    $("body").on("click", ".vital-chart-remove", self.delete_vital_chart);
    $("body").on("click", ".rounds-remove", self.delete_rounds);
    $("body").on("click", "#btn_add_rounds", self.add_rounds);
    $("body").on("click", "#btn_add_physiotherapy", self.add_physiotherapy);
    $("body").on("click", ".physiotherapy-remove", self.delete_physiotherapy);
    $("body").on("click", ".xray-remove", self.delete_xray);
    $("body").on("ifChanged", ".labtest", self.set_labtest);
    $("#btn_view_result").on('click', self.show_labtestlist);
    $("body").on("ifChanged", ".xray", self.set_xray_test);
    $("body").on("click", ".btn_view_xray_result", self.show_xray_result);
    $("body").on("click", "#btn_add_lab_items", self.add_lab_item);
    $("body").on("ifChanged", ".check-box", self.check);
    $("#btn_internal_medicine").on('click', self.add_internal_medicines);
    $("#btnAddinternalmedicines").on('click', self.add_internale_medicineitems);
    $("body").on("click", ".edit_internal_medicines", self.edit_internal_medicines);
    $("body").on('ifChanged', '.IsDischargeAdvice', self.include_discharge_summary);
    $("body").on("keyup change", "#NoofDay", self.calculate_medicine_days);
    $("body").on("click", ".labitems-remove", self.remove_lab_items);
    $("#TreatmentStartDate").on('change', self.calculate_treatment_days);
    $("body").on("click", ".historytable thead .btnViewHistory", self.get_history_details);
    $("body").on("click", ".btnBackToHistory", self.show_history);
    $("body").on("click", "#btn_add_labtests", self.get_data_from_modal);
    $("#btnOKEmployee").on("click", self.select_employee);
    $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
    $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $("body").on("click", "#btn_adddoctor", self.add_doctor);

    $("body").on("ifChanged", ".MorningTime", self.check_morningTime);
    $("body").on("ifChanged", ".NoonTime", self.check_noonTime);
    $("body").on("ifChanged", ".EveningTime", self.check_eveningTime);
    $("body").on("ifChanged", ".NightTime", self.check_nightTime);
    $("body").on("ifChanged", ".Treatmentfrequency", self.calculate_treatment_day);
    $("body").on("click", ".remove-medicines-grid", self.remove_medicine);
    $("#btnAddNext").on('click', self.add_medicines_on_grid);
    $("body").on("click", ".btncanceltreatment", self.show_cancel_treatment);
    $("body").on("click", "#btn-cancel-confirm", self.cancel_treatment);
    $("body").on("ifChanged", ".xray-check-box", self.xray_check);
    $("body").on("click", ".remove-xray-item", self.xray_delete_item);
    $("body").on("click", "#btn_add_xray", self.get_xray_details_modal);

    $("body").on("click", ".remove-discharge-medicine", self.delete_discharge_medicine);
},
IPCaseSheet.today_list = function ($list, type) {
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs($list);

        var url = "/AHCMS/IPCaseSheet/GetManagePatientList?type=" + type

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": true,
            "aaSorting": [[5, "desc"]],
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
                       + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                       + "<input type='hidden' class='TransID' value='" + row.TransID + "'>"
                       + "<input type='hidden' class='OPID' value='" + row.AppointmentProcessID + "'>"
                       + "<input type='hidden' class='IsCompleted' value='" + row.IsCompleted + "'>"
                       + "<input type='hidden' class='IsReferedIP' value='" + row.IsReferedIP + "'>";;

                   }
               },
               { "data": "Code", "className": "Code" },
               { "data": "Name", "className": "Name" },
               { "data": "Time", "className": "Time" },
               { "data": "TokenNo", "className": "TokenNo" },
               { "data": "Date", "className": "Date" },
               {
                   "data": "", "searchable": false, "className": "action editdepartment", "orderable": false,
                   "render": function (data, type, row, meta) {
                       return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btneditdepartment'>Edit Department</button>"
                   }
               }
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.IsCompleted);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var PatientID = $(this).closest("tr").find("td .PatientID").val();
                    var ScheduleItemID = $(this).closest("tr").find("td .TransID").val();
                    var OPID = $(this).closest("tr").find("td .OPID").val();
                    var IsCompleted = $(this).closest("tr").find("td .IsCompleted").val();
                    var IsReferedIP = $(this).closest("tr").find("td .IsReferedIP").val();
                    app.load_content("/AHCMS/IPCaseSheet/CreateV5/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
                });
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};
IPCaseSheet.list = function () {
    var self = IPCaseSheet;

    $('#tabs-iplist').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            self.tabbed_list(active_item.data('tab'));
            active_item.data('tab-loaded', true);
        }
    });
};
IPCaseSheet.tabbed_list= function (type) {
    var self = IPCaseSheet;

    var $list;

    switch (type) {
        case "IPPatient":
            $list = $('#ip-list');
            break;
        case "DischargeAdvicedPatients":
            $list = $('#dischargeadviced-list');
            break;
        case "DischargedPatients":
            $list = $('#discharge-list');
            break;

        default:
            $list = $('#ip-list');
    }
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });

        altair_md.inputs($list);

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": true,
            "aaSorting": [[1, "desc"]],
            "ajax": {
                "url": "/AHCMS/IPCaseSheet/GetIPPatientList?type=" + type,
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
                        + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                        + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                        + "<input type='hidden' class='OPID' value='" + row.AppointmentProcessID + "'>"
                        + "<input type='hidden' class='IsDischarged' value='" + row.IsDischarged + "'>"
                        + "<input type='hidden' class='IsDischargeAdviced' value='" + row.IsDischargeAdviced + "'>"
                        + "<input type='hidden' class='DischargeSummaryID' value='" + row.DischargeSummaryID + "'>";
                   }
               },
               { "data": "Date", "className": "TransDate" },
               { "data": "Code", "className": "Code" },
               { "data": "Name", "className": "Name" },


            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.IsDraft);
                $(row).addClass(data.IsCancelled);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var PatientID = $(this).closest("tr").find("td .PatientID").val();
                    var IPID = $(this).closest("tr").find("td .IPID").val();
                    var OPID = $(this).closest("tr").find("td .OPID").val();
                    var IsDischarged = $(this).closest("tr").find("td .IsDischarged").val();
                    var IsDischargeAdviced = $(this).closest("tr").find("td .IsDischargeAdviced").val();
                    var DischargeSummaryID = $(this).closest("tr").find("td .DischargeSummaryID").val();
                    app.load_content("/AHCMS/IPCaseSheet/CreateV5/?ID=" + PatientID + "&IPID=" + IPID + "&OPID=" + OPID + "&IsDischarged=" + IsDischarged + "&IsDischargeAdviced=" + IsDischargeAdviced + "&DischargeSummaryID=" + DischargeSummaryID);
                });
            }
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });

        return list_table;
      
    }
};
IPCaseSheet.previous_List = function ($list, type) {
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });

        altair_md.inputs($list);

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": true,
            "aaSorting": [[5, "desc"]],
            "ajax": {
                "url": "/AHCMS/IPCaseSheet/GetManagePatientList?type=" + type,
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
                    "className": "",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        return (meta.settings.oAjaxData.start + meta.row + 1)
                        + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                        + "<input type='hidden' class='TransID' value='" + row.TransID + "'>"
                        + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                        + "<input type='hidden' class='IsCompleted' value='" + row.IsCompleted + "'>"
                        + "<input type='hidden' class='IsReferedIP' value='" + row.IsReferedIP + "'>";
                    }
                },
               { "data": "Code", "className": "Code" },
               { "data": "Name", "className": "Name" },
               { "data": "Time", "className": "Time" },
               { "data": "TokenNo", "className": "TokenNo" },
               { "data": "Date", "className": "Date" },

               {
                   "data": "", "searchable": false, "className": "action editdepartment", "orderable": false,
                   "render": function (data, type, row, meta) {
                       return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btneditdepartment'>Edit Department</button>"
                   }
               }
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.IsCompleted);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var PatientID = $(this).closest("tr").find("td .PatientID").val();
                    var ScheduleItemID = $(this).closest("tr").find("td .TransID").val();
                    var OPID = $(this).closest("tr").find("td .AppointmentProcessID").val();
                    var IsCompleted = $(this).closest("tr").find("td .IsCompleted").val();
                    var IsReferedIP = $(this).closest("tr").find("td .IsReferedIP").val();
                    app.load_content("/AHCMS/IPCaseSheet/CreateV5/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
                });
            },
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });

        return list_table;
    }
};
IPCaseSheet.Save = function () {
    var self = IPCaseSheet;
    var data;
    data = self.get_data();
    $.ajax({
        url: '/AHCMS/IPCaseSheet/SaveV5',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Saved Successfully");
                setTimeout(function () {
                    window.location = "/AHCMS/IPCaseSheet/IndexV5";
                }, 1000);
            }
            else {
                app.show_error("Failed to Create");
                $(" .btnSave").css({ 'visibility': 'visible' });
            }
        }
    });
};
IPCaseSheet.rules = {
    on_medicine_add: [
    {
        elements: "#StartDate",
        rules: [
        { type: form.required, message: "Please select StartDate" },
        ]
    },

    {
        elements: "#TimeDescription",
        rules: [
        { type: form.required, message: "Please Add Description" },
        ]
    },


    {
        elements: "#NoofDays",
        rules: [
        { type: form.required, message: "Please enter No of Days" },
        ]
    },
    {
        elements: "#EndDate",
        rules: [
        { type: form.required, message: "Please enter EndDate" },
        ]
    },
    {
        elements: "#item-count",
        rules: [
        { type: form.non_zero, message: "Please add atleast one medicine" },
        {
            type: form.required, message: "Please add atleast one medicine"
        },

        ]
    },
    ],

    on_medicine_items: [
{
    elements: "#StartDate",
    rules: [
    { type: form.required, message: "Please select StartDate" },
    ]
},

{
    elements: "#description",
    rules: [
    { type: form.required, message: "Please Add Description" },
    ]
},
//{
//    elements: "#MedicinesID",
//    rules: [
//        { type: form.required, message: "Medicine Name is Invalid" },
//        { type: form.non_zero, message: "Medicine Name is Invalid" },
//    ]
//},

{
    elements: "#NoofDay",
    rules: [
    { type: form.required, message: "Please enter No of Days" },
    ]
},
{
    elements: "#enddate",
    rules: [
    { type: form.required, message: "Please enter EndDate" },
    ]
},
{
    elements: "#External-item-count",
    rules: [
    { type: form.non_zero, message: "Please add atleast one medicine" },
    {
        type: form.required, message: "Please add atleast one medicine"
    },

    ]
},
    ],

    on_vital_chart_add: [
    {
        elements: "#VitalChartDate",
        rules: [
        { type: form.required, message: "Please select Date" },
        ]
    },
    ],

    on_treatment_add: [
    {
        elements: "#TreatmentName",
        rules: [
        { type: form.required, message: "Please select Treatment" },
        ]
    },

    //{
    //    elements: "#TherapistID",
    //    rules: [
    //    { type: form.required, message: "Please select Therapist" },
    //    ]
    //},
    //{
    //    elements: "#TreatmentRoomID",
    //    rules: [
    //    { type: form.required, message: "Please select Treatment Room" },
    //    ]
    //},
    {
        elements: "#TentativeStartDate",
        rules: [
        { type: form.required, message: "Please select StartDate" },
        ]
    },
    {
        elements: "#TreatmentID",
        rules: [
            { type: form.required, message: "Treatment Name is Invalid" },
            { type: form.non_zero, message: "Treatment Name is Invalid" },
        ]
    },
    {
        elements: "#TentativeEndDate",
        rules: [
        { type: form.required, message: "Please select EndDate" },
        ]
    },
    {
        elements: "#NoofTreatment",
        rules: [
         { type: form.required, message: "Please enter No of Treatment" },
         { type: form.non_zero, message: "Please enter valid Treatment No" },
        ]
    },

    ],

    on_report_add: [
    {
        elements: "#ReportName",
        rules: [
        { type: form.required, message: "Please add Report Name" },
        ]
    },

    {
        elements: "#select-quotation",
        rules: [
        { type: form.required, message: "Please Upload Report" },
        ]
    },
    ],

    on_sub_medicine_add: [
    {
        elements: "#MedicinesID",
        rules: [
        { type: form.required, message: "Please select Medicines" },
        ]
    },

    //{
    //    elements: "#Qty",
    //    rules: [
    //    { type: form.required, message: "Please enter Quantity" },
    //    {
    //        type: function (element) {
    //            var error = false;
    //            var TotalStock = $("#TotalStock").val();
    //            var Qty = clean($(element).val());
    //            if (Qty > TotalStock) {
    //                error = true;
    //            }
    //            return !error;
    //        }, message: "Insufficient Stock"
    //    },
    //    ]
    //},

    ],

    on_external_medicine_add: [
 {
     elements: "#ExternalMedicines",
     rules: [
     { type: form.required, message: "Please select Medicines" },
     ]
 },

 {
     elements: "#ExternalQty",
     rules: [
     { type: form.required, message: "Please enter quantity" },
     ]
 },

    ],

    on_treatment_medicine: [
    {
        elements: "#Medicine",
        rules: [
        { type: form.required, message: "Please select Medicines" },
        ]
    },
    ],

    on_save: [
    {
        elements: ".PresentingComplaints",
        rules: [
            { type: form.required, message: "Please Add Presenting Complaints" },
        ]
    },
    ],

    on_add_rounds: [
    {
        elements: "#RoundsDate",
        rules: [
        { type: form.required, message: "Please Add Date" },
        ]
    },
    {
        elements: "#RoundTime",
        rules: [
        { type: form.required, message: "Please Add Time" },
        ]
    },
    {
        elements: "#ClinicalNote",
        rules: [
        { type: form.required, message: "Please Add Clinical Note" },
        ]
    },
    ],

    on_add_lab_tests: [
    {
        elements: "#LabTest",
        rules: [
        { type: form.required, message: "Please Select Lab Test" },
        ]
    },
    ],

    on_add_xray: [
   {
       elements: "#XrayName",
       rules: [
       { type: form.required, message: "Please Select X-Ray" },
       ]
   },
    ],

    on_add_physiotherapy: [
    {
        elements: "#Physiotherapy",
        rules: [
        { type: form.required, message: "Please Select Physiotherapy" },
        ]
    },

    {
        elements: "#PhysioFromDate",
        rules: [
        { type: form.required, message: "Please Select StartDate" },
        ]
    },

    {
        elements: "#PhysioToDate",
        rules: [
        { type: form.required, message: "Please Select EndDate" },
        ]
    },
    ],
};
IPCaseSheet.get_report = function () {
    var self = IPCaseSheet;
    var PatientID = $('#PatientID').val();
    var IPID = $('#IPID').val();
    $.ajax({
        url: "/AHCMS/IPCaseSheet/GetReportV5",
        data: {
            PatientID: PatientID,
            IPID: IPID,
        },
        dataType: "html",
        type: "POST",
        success: function (response) {
            $("#report-list tbody").empty();
            $response = $(response);
            app.format($response);
            $("#report-list tbody").append($response);
        }
    });
};
IPCaseSheet.add_report= function () {
    var self = IPCaseSheet;
    self.error_count = 0;
    self.error_count = self.validate_report();
    if (self.error_count > 0) {
        return;
    }
    $(".url").val('');
    sino = $('#report-list tbody tr').length + 1;
    var checked = "";
    var IsBeforeAdmission = false;
    var ReportName = $("#ReportName").val();
    var Date = $("#ReportDate").val();
    var Report = $('#selected-quotation').text();
    var Description = $("#Description").val();
    var ReportID = $('#ReportID').val();
    var Index = $("#Index").val();
    var url = $('#URL').val();
    if ($("#chk_IsBeforeAdmission").prop("checked") == true) {
        IsBeforeAdmission = true;
        checked = "checked";
    }
    if ($("#RowIndex").val() > 0) {
        var rowindex = $('#report-list tbody').find('tr').eq(Index);
        $(rowindex).find(".ReportName").text(ReportName);
        $(rowindex).find(".Description").text(Description);
        $(rowindex).find(".ReportID").val(ReportID);
        $(rowindex).find(".Report").val(Report);
        $(rowindex).find(".Date").text(Date);
        $(rowindex).find(".url").val(url);
        $('#add-report').trigger('click');
    }
    else {

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class="ReportName">' + ReportName
            + '</td>'
            + '<td class="Description">' + Description + '</td>'
            + '<td class = "uk-hidden">'
            + '<input type="hidden" class="ReportID" value="' + ReportID + '"/>'
            + '<input type="hidden" class="Report" value="' + Report + '"/>'
            + '<input type="hidden" class="url" value="' + url + '"/>'
            + '</td>'
            + '<td class = "Date">' + Date + '</td>'
            + '<td class="uk-text-center">' + '<button class="view_report">' + '<i class="material-icons">' + "remove_red_eye" + '</i>' + '</button>' + '</td>'
            + '<td class="uk-text-center">' + '<button class="edit_report">' + '<i class="material-icons">' + "create" + '</i>' + '</button>' + '</td>'            
            + '<td>'
            + '<input type="checkbox" name="IsBeforeAdmission" class="IsBeforeAdmission" data-md-icheck disabled value="' + IsBeforeAdmission + '" ' + checked + '/>'
            + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#report-list tbody').append($content);
        $('#add-report').trigger('click');
    }
};
IPCaseSheet.Save = function () {
    var self = IPCaseSheet;
    var data;
    data = self.get_data();
    $.ajax({
        url: '/AHCMS/IPCaseSheet/SaveV5',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Saved Successfully");
                setTimeout(function () {
                    window.location = "/AHCMS/IPCaseSheet/IndexV5";
                }, 1000);
            }
            else {
                app.show_error("Failed to Create");
                $(" .btnSave").css({ 'visibility': 'visible' });
            }
        }
    });
};
IPCaseSheet.get_data = function () {

    var self = IPCaseSheet;
    var data = {};
    data.AppointmentScheduleItemID = $("#AppointmentScheduleItemID").val();
    data.AppointmentType = $(".op:checked").val();
    data.VisitType = $(".fresh:checked").val();
    data.PatientID = $("#PatientID").val(),
    data.ParentID = $("#ParentID").val(),
    data.ReviewID = $("#ReviewID").val(),
    data.Date = $("#Date option:selected").val();
    data.AppointmentProcessID = $("#AppointmentProcessID").val(),
    data.IPID = $("#IPID").val(),
    //self.VitalChartItems.push({
    //    BP: $("#BP").val(),
    //    Pulse: $("#Pulse").val(),
    //    Temperature: $("#Temperature").val(),
    //    Unit: $("#Unit").val(),
    //    HR: $("#HR").val(),
    //    RR: $("#RR").val(),
    //    Height: $("#Height").val(),
    //    Weight: $("#Weight").val(),
    //    BMI: $("#BMI").val(),
    //    RespiratoryRate: $("#RespiratoryRate").val(),
    //    Others: $("#Others").val()
    //});
    data.NextVisitDate = $("#NextVisitDate").val();
    data.Remark = $("#Remark").val();
    data.ExaminationItems = [];
    data.ReportItems = [];
    data.DoctorList = [];
    data.RoundsItems = [];
    data.TreatmentItems = [];
    data.Medicines = [];
    data.MedicineItems = [];
    data.TreatmentMedicines = [];
    data.VitalChartItems = [];
    data.LabTestItems = [];
    data.XrayItems = [];
    data.BaseLineItems = [];
    data.OtherConditionsItems = [];
    data.RogaPareekshaItems = [];
    data.CaseSheetItems = [];
    data.AssociatedConditionsItems = [];
    data.RogaNirnayamItems = [];
    data.InternalMedicine = [];
    data.InternalMedicineItems = [];
    data.DischargeSummary = [];
    data.ExaminationNewItems = [];

    $('.IsDischargeAdvice').prop("checked") == true ? data.IsDischargeAdvice = true : data.IsDischargeAdvice = false;
    var item = {};
    $('#examination-list tbody .examination-results').each(function () {
        item = {};

        item.ID = $(this).parent().prevAll(".ID").val();
        //item.ID = $(this).parent().find(".ID").val();
        item.Value = $(this).parent().find(".value").val();
        item.Description = $(this).parent().find(".description").val();
        data.ExaminationItems.push(item);
    });
    $('#examination-list tbody .dahavidyaResults').each(function () {
        item = {};
        var Area = $(this).closest('td').attr('class');
        var Groupname = $(this).attr('name');
        if ($(this).is('input:radio')) {
            if ($(this).is(':checked')) {
                item.Name = Groupname;
                item.Description = $('input[name=' + Groupname + ']:checked').val();
                item.Area = Area;
                data.ExaminationNewItems.push(item);
            }
        }
        if (($(this).is('input:checkbox'))) {
            if ($(this).is(':checked')) {
                item.Name = Groupname;
                item.Description = $('input[name=' + Groupname + ']:checked').val();
                item.Area = Area;
                data.ExaminationNewItems.push(item);
            }
        }
        if (($(this).is('input:text'))) {
            item.Name = Groupname;
            item.Description = $('input[name=' + Groupname + ']').val();
            item.Area = Area;
            data.ExaminationNewItems.push(item);
        }

    });
    $('#base-line-information-list tbody tr .baselineResults').each(function (i, row) {
        item = {};
        var baseline = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text'))) {
            if ($(this).is(':checked')) {
                item.Name = baseline;
                item.Description = $('input[name=' + baseline + ']:checked').val();
                data.BaseLineItems.push(item);
            }
            if ($(this).is('input:text')) {
                item.Name = baseline;
                var txt = $(".baseLine_txt").val();
                if (txt != "") {
                    item.Description = txt;
                    data.BaseLineItems.push(item);
                }

            }
        }
    });
    $('#base-line-information-list').find('input[type="checkbox"]:checked').each(function () {
        item = {};
        data.BaseLineItems.OtherConditions = [];
        var baseline = $(this).attr('name')
        var intRegex = /^\d+$/;
        var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;
        var str = $(this).val();
        if (intRegex.test(str) || floatRegex.test(str)) {
            var num = str;
        }
        else {
            item.Name = baseline;
            item.Description = $(this).val();
            data.BaseLineItems.OtherConditions.push(item);
            data.OtherConditionsItems.push(item);
        }

    })
    $('#add_doctor tbody tr').each(function () {
        item = {};
        item.DoctorName = $(this).find(".DoctorName").text();
        item.DoctorNameID = $(this).find(".DoctorNameID").val();
        data.DoctorList.push(item);
    });

    $('#report-list tbody tr').each(function () {
        item = {};
        item.ID = $(this).find(".ID").val();
        item.Name = $(this).find(".ReportName").text();
        item.Description = $(this).find(".Description").text();
        item.DocumentID = $(this).find(".ReportID").val();
        item.Date = $(this).find(".Date").text();
        item.IsBeforeAdmission = $(this).find(".IsBeforeAdmission").val();
        data.ReportItems.push(item);
    });

    $('#treatment-list tbody tr').each(function () {
        item = {};
        item.TreatmentID = $(this).find(".TreatmentID").val();
        item.TherapistID = $(this).find(".TherapistID").val();
        item.TreatmentRoomID = $(this).find(".TreatmentRoomID").val();
        item.Instructions = $(this).find(".Instructions").text();
        item.StartDate = $(this).find(".StartDate").text();
        item.EndDate = $(this).find(".EndDate").text();
        item.TreatmentNo = $(this).find(".NoofTreatments").text();
        item.PatientTreatmentID = $(this).find(".PatientTreatmentID").val();

        item.MorningTime = $(this).find(".MorningTime").val();
        item.NoonTime = $(this).find(".NoonTime").val();
        item.EveningTime = $(this).find(".EveningTime").val();
        item.NightTime = $(this).find(".NightTime").val();

        item.IsMorning = $(this).find(".IsMorning").val();
        item.IsNoon = $(this).find(".IsNoon").val();
        item.Isevening = $(this).find(".Isevening").val();
        item.IsNight = $(this).find(".IsNight").val();
        item.NoofDays = $(this).find(".NoofDays").val();
        data.TreatmentItems.push(item);
    });

    //roga-rogi pareeksha tab 
    $('#roga-pareeksha-list tbody tr .rogapareekshaResults').each(function (i, row) {
        item = {};
        var Groupname = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text'))) {
            if ($(this).is(':checked')) {
                item.Name = Groupname;
                item.Description = $('input[name=' + Groupname + ']:checked').val();
                data.RogaPareekshaItems.push(item);
            }
        }
    });
    //casesheet tab
    $('#case-sheet-list tbody tr .casesheetResults').each(function (i, row) {
        item = {};
        var groupname = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text')) || ($(this).is('textarea'))) {
            if ($(this).is(':checked')) {
                item.Name = groupname;
                groupname = groupname.split(' ').join('_');
                item.Description = $('input[name=' + groupname + ']:checked').val();
                data.CaseSheetItems.push(item);
            }
            if ($(this).is('input:text')) {
                var row = $(this).closest('tr');
                var txt = $(row).find(".case_sheet_txt").val();
                item.Name = groupname;
                //var txt = $(".case_sheet_txt").val();
                //var txt = $(this).find('.case_sheet_txt').text();
                //if (txt != "") {
                item.Description = txt;
                data.CaseSheetItems.push(item);
                //}

            }
            if ($(this).is('textarea')) {

                var row = $(this).closest('tr');
                item.Name = groupname;
                var txt = $(row).find(".case_sheet_txt").val();
                item.Description = txt;
                data.CaseSheetItems.push(item);
            }
        }
    });
    $('#case-sheet-list').find('input[type="checkbox"]:checked').each(function () {
        item = {};
        data.CaseSheetItems.AssociatedConditions = [];
        var groupname = $(this).attr('name')
        var intRegex = /^\d+$/;
        var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;
        var str = $(this).val();
        if (intRegex.test(str) || floatRegex.test(str)) {
            var num = str;
        }
        else {
            item.Name = groupname;
            item.Description = $(this).val();
            data.CaseSheetItems.AssociatedConditions.push(item);
            data.AssociatedConditionsItems.push(item);
        }

    })

    $('#roga-nirnayam-list tbody .examination-results').each(function () {
        item = {};

        item.ID = $(this).parent().prevAll(".ID").val();
        //item.ID = $(this).parent().find(".ID").val();
        item.Value = $(this).parent().find(".value").val();
        item.Description = $(this).parent().find(".description").val();
        data.RogaNirnayamItems.push(item);
    });
    $('#rounds-list tbody tr').each(function () {
        item = {};
        item.Remarks = $(this).find(".Remarks").text(); RoundTime
        item.RoundsDate = $(this).find(".Date").text();
        item.RoundsTime = $(this).find(".RoundTime").text();
        item.ClinicalNote = $(this).find(".ClinicalNote").text();
        data.RoundsItems.push(item);

    });

    data.TreatmentMedicines = self.Items;

    data.Medicines = self.Medicine;

    data.MedicineItems = self.MedicinesItems;

    data.InternalMedicine = self.InternalMedicine;

    data.InternalMedicineItems = self.InternalMedicinesItems;

    data.DischargeSummary.push({
        CourseInTheHospital: $("#CourseInTheHospital").val(),
        ConditionAtDischarge: $("#ConditionAtDischarge").val(),
        DietAdvice: $("#DietAdvice").val(),
    })

    //data.VitalChartItems = self.VitalChartItems;
    $('#add_vital_chart_list tbody tr').each(function () {
        item = {};
        item.Date = $(this).find(".Date").text();
        item.Time = $(this).find(".Time").text();
        item.BP = $(this).find(".BP").text();
        item.Pulse = $(this).find(".Pulse").text();
        item.Temperature = $(this).find(".Temperature").text();
        item.HR = $(this).find(".HR").text();
        item.RR = $(this).find(".RR").text();
        item.Weight = $(this).find(".Weight").text();
        item.Height = $(this).find(".Height").text();
        item.Others = $(this).find(".Others").text();
        data.VitalChartItems.push(item);
    });

    //$("#add_medicine-list tbody tr").each(function () {
    //    item = {};
    //    item.MedicinesID = $(this).find(".MedicinesID").val();
    //    item.UnitID = $(this).find(".UnitID").val();
    //    item.Unit = $(this).find(".Unit").val();
    //    item.GroupID = $(this).find(".GroupID").val();
    //    item.Quantity = $(this).find(".Quantity").val();
    //    item.Medicines = $(this).find(".Medicines").val();
    //    item.PatientMedicinesID = $(this).find(".PatientMedicinesID").val();
    //    data.Medicines.push(item);
    //});

    $('#add_xray_items tbody tr').each(function () {
        item = {};
        item.XrayID = $(this).find(".XrayID").val();
        item.XrayDate = $(this).find(".Date").text();
        item.ID = $(this).find(".ID").val();
        data.XrayItems.push(item);
    });

    $('#add_lab_items tbody tr.LabItems').each(function () {
        item = {};
        item.LabTestID = $(this).find(".LabTestID").val();
        item.TestDate = $(this).find(".Date").text();
        item.ID = $(this).find(".ID").val();
        data.LabTestItems.push(item);
    });

    return data;
};
IPCaseSheet.lablist= function () {
    var $list = $('#labtest-list');

    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs($list);

        var url = "/AHCMS/PatientDiagnosis/GetLaborXrayListV5"

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": true,
            "aaSorting": [[1, "desc"]],
            "ajax": {
                "url": url,
                "type": "POST",
                "data": function (data) {
                    data.params = [
                       { Key: "Type", Value: "Lab" },
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
                {
                    "data": null,
                    "className": "uk-text-center",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        var checked = "";
                        var self = IPCaseSheet;
                        if (self.LabItemList.indexOf(row.ID) != -1) {
                            checked = "checked";
                        }
                        return "<input type='checkbox' class='uk-radio check-box ItemID chk-lab-test' " + checked + "  name='ItemID' data-md-icheck value='" + row.ID + "' >";
                    }
                },
               { "data": "Code", "className": "Code" },
               { "data": "Type", "className": "Type" },
               { "data": "GroupName", "className": "GroupName" },
               { "data": "Name", "className": "Name" },
            ],
            "createdRow": function (row, data, index) {
                app.format(row);
            },
            "drawCallback": function () {
                altair_md.checkbox_radio();
                $list.trigger("datatable.changed");
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};
IPCaseSheet.xraylist = function () {
    var $list = $('#xray-list');

    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs($list);

        var url = "/AHCMS/PatientDiagnosis/GetLaborXrayListV5"

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": true,
            "aaSorting": [[1, "desc"]],
            "ajax": {
                "url": url,
                "type": "POST",
                "data": function (data) {
                    data.params = [
                           { Key: "Type", Value: "x-ray" },
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
                {
                    "data": null,
                    "className": "uk-text-center",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        var checked = "";
                        var self = IPCaseSheet;
                        if (self.XrayItemList.indexOf(row.ID) != -1) {
                            checked = "checked";
                        }
                        return "<input type='checkbox' class='uk-radio xray-check-box ItemID chk-xray-test' " + checked + "  name='ItemID' data-md-icheck value='" + row.ID + "' >";
                    }
                },
               { "data": "Code", "className": "Code" },
               { "data": "Type", "className": "Type" },
               { "data": "GroupName", "className": "GroupName" },
               { "data": "Name", "className": "Name" },
            ],
            "createdRow": function (row, data, index) {
                app.format(row);
            },
            "drawCallback": function () {
                altair_md.checkbox_radio();
                $list.trigger("datatable.changed");
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};
IPCaseSheet.get_xray_details_modal = function () {
    var self = IPCaseSheet;
    var data = {};
    for (var i = 0; i < self.XrayItemList2.length; i++) {
        var item = {};
        item.ID = self.XrayItemList2[i].id;
        item.Name = self.XrayItemList2[i].name;
        item.Date = $("#XrayDate").val();
        self.XrayTest.push(item);
    }
    self.add_xray_data_to_grid();
    $("#xray-list tbody tr.included").each(function () {
        $(this).find('.ItemID').parent('div').removeClass("checked");
        $(this).removeClass('included');
    });
    self.XrayTest = [];
    self.XrayItemList = [];
    self.XrayItemList2 = [];

    return data;
};
IPCaseSheet.xray_check = function () {
    var self = IPCaseSheet
    var row = $(this).closest('tr');
    if ($(this).is(":checked")) {
        //$(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
        $(this).closest('tr').addClass('included');
        self.XrayItemList.push(clean($(row).find('.chk-xray-test').val()));
        self.XrayItemList2.push({ 'id': clean($(row).find('.chk-xray-test').val()), 'name': $(row).find('.Name').text() });
    } else {
        //$(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
        $(this).closest('tr').removeClass('included');
        $(this).removeAttr('disabled');
        var value = $(row).find('.chk-xray-test').val();
        var i = 0;
        while (i < self.XrayItemList.length) {
            if (self.XrayItemList[i] == value) {
                self.XrayItemList.splice(i, 1);
                self.XrayItemList2.splice(i, 1);
            } else {
                ++i;
            }
        }
    }
};
IPCaseSheet.add_xray_data_to_grid = function () {
    var self = IPCaseSheet;
    $(self.XrayTest).each(function (i, record) {
        var serialno = $('#add_xray_items tbody tr').length + 1;
        var content = "";
        var $content;
        content = '<tr class="XrayItems">'
             + '<td class="uk-text-center slno">' + serialno
             + '</td>'
             + '<td class="Date">' + record.Date
             + '<input type="hidden" class="XrayID" value="' + record.ID + '"/>'
             + '<input type="hidden" class="ID" value="0" />'
             + '</td>'
             + '<td class="XrayName">' + record.Name
             + '</td>'
             + '<td>'
             + '</td>'
             + '<td>'
             + '<a class="">'
             + '<i class="uk-icon-remove remove-xray-item"></i>'
             + '</a>'
             + '</td>'
             + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_xray_items tbody').append($content);
    });
};
IPCaseSheet.xray_delete_item = function () {
    var self = IPCaseSheet;
    $(this).closest('tr').remove();
    index = $("#add_xray_items tbody tr").length;
    var sino = 0;
    $('#add_xray_items tbody tr .slno').each(function () {
        sino = sino + 1;
        $(this).text(sino);
    });
};
IPCaseSheet.get_XrayItems = function () {
    var self = IPCaseSheet;
    var PatientID = $('#PatientID').val();
    var IPID = $('#IPID').val();
    $.ajax({
        url: "/AHCMS/IPCaseSheet/XrayTest",
        dataType: "html",
        data: {
            PatientID: PatientID,
            IPID: IPID
        },
        type: "POST",
        success: function (response) {
            $("#add_xray_items tbody").empty();
            $response = $(response);
            app.format($response);
            $("#add_xray_items tbody").append($response);
        },
    });    
};
IPCaseSheet.get_patient_data = function () {
    var self = IPCaseSheet;
    self.get_vital_chart();
    self.get_medicine();
    self.get_medicines_details();
    self.get_medicines_Items();
    self.get_treatment();
    self.get_treatment_item();
    self.get_report();
    self.get_rounds();
    self.get_LabItems();
    self.get_XrayItems();
    self.get_doctor_list();
    self.get_discharge_summary();
    self.get_external_medicine();
    self.get_external_medicines_details();
    self.get_external_medicines_Items();
    self.get_DashaVidhaPareekhsa();
};
IPCaseSheet.get_DashaVidhaPareekhsa = function () {
    var self = IPCaseSheet;
    var PatientID = $('#PatientID').val();
    var IPID = $('#IPID').val();
    $.ajax({
        url: "/AHCMS/IPCaseSheet/GetDashaVidhaPareekhsa",
        data: {
            PatientID: PatientID,
            IPID: IPID,
        },
        dataType: "Json",
        type: "POST",
        success: function (response) {
            $(response.Data).each(function (i, record) {
                if (record.GroupName == 'DOSHA' || record.GroupName == 'DHATU' || record.GroupName == 'MALA') {
                    $("." + record.GroupName + "").val(record.Description);
                }
                else {
                    $('input[name="' + record.GroupName + '"][value="' + record.Description + '"]').iCheck('check');
                }


            });
        }
    });
};
IPCaseSheet.add_rounds = function () {
    var self = IPCaseSheet;
    if (self.validate_rounds() > 0) {
        return;
    }
    sino = $('#rounds-list tbody tr ').length + 1;
    var Date = $("#RoundsDate").val();
    var Remarks = $("#Remark").val();
    var ClinicalNote = $("#ClinicalNote").val();
    var Doctor = $("#Doctor").val();
    var UserID = $("#UserID").val();
    var Time = $("#RoundTime").val();

    var content = "";
    var $content;
    content = '<tr>'
        + '<td class="rounds-serial-no uk-text-center">' + sino + '</td>'
        + '<td class="Date">' + Date + '</td>'
        + '<td class="RoundTime">' + Time + '</td>'
        + '<td class="ClinicalNote">' + ClinicalNote         
        +'</td>'
        + '<td class="Remarks">' + Remarks + '</td>'
        + '<td class="Doctor">' + Doctor
        + '<input type="hidden" class="UserID" value="' + UserID + '"/>'
        + '</td>'
        + '<td>'
        + '<a class="rounds-remove">'
        + '<i class="uk-icon-remove"></i>'
        + '</a>'
        + '</td>'
        + '</tr>';
    $content = $(content);
    app.format($content);
    $('#rounds-list tbody').append($content);
    self.clear_rounds();

};
IPCaseSheet.clear_rounds= function () {
    var self = IPCaseSheet;    
    $("#ClinicalNote").val('');
    $("#Remark").val('');
};
IPCaseSheet.get_rounds = function () {
    var self = IPCaseSheet;
    var PatientID = $('#PatientID').val();
    var IPID = $('#IPID').val();
    $.ajax({
        url: "/AHCMS/IPCaseSheet/RoundsV5",
        data: {
            PatientID: PatientID,
            IPID: IPID,
        },
        dataType: "html",
        type: "POST",
        success: function (response) {
            $response = $(response);
            app.format($response);
            $("#rounds-list tbody").append($response);
        }
    });
};