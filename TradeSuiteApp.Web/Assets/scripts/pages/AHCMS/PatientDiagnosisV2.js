
PatientDiagnosis.init = function () {
    var self = PatientDiagnosis;
    self.is_patient_history();
    self.bind_events();
    //self.get_vitals();
    self.lablist();
    //$('#labtest-list').SelectTable({
    //    modal: "#select-labtest",
    //    initiatingElement: "#PartyName"
    // });

    employee_list = Employee.employee_list();
    item_select_table = $('#employee-list').SelectTable({
        selectFunction: self.select_employee,
        returnFocus: "#ItemName",
        modal: "#select-employee",
        initiatingElement: "#EmployeeName"
    });

    self.list();

},
PatientDiagnosis.bind_events = function () {
    var self = PatientDiagnosis;
    $("#btnAddMedicine").on("click", self.show_add_medicine);
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
    $.UIkit.autocomplete($('#treatment-autocomplete'), Config.get_treatments);
    $.UIkit.autocomplete($('#medicinename-autocomplete'), Config.get_medicineList_autocomplete);
    $('#medicine-autocomplete').on('selectitem.uk.autocomplete', self.set_medicine);
    $('#treatment-autocomplete').on('selectitem.uk.autocomplete', self.set_treatment);
    $('#medicinename-autocomplete').on('selectitem.uk.autocomplete', self.set_medicine_name);
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
    $("body").on("ifChanged", ".Noon", self.check_noon);
    $("body").on("ifChanged", ".Evening", self.check_evening);
    $("body").on("ifChanged", ".Night", self.check_night);
    $("body").on("ifChanged", ".MultipleTimes", self.check_multipletimes);
    $("body").on("click", ".prescription_printpdf", self.prescription_printpdf);
    $("body").on("click", ".treatment_printpdf", self.treatment_printpdf);
    $("body").on("ifChanged", ".labtest", self.set_labtest);
    $("#btn_view_result").on('click', self.show_labtestlist);
    $("body").on("ifChanged", ".xray", self.set_xray_test);
    $.UIkit.autocomplete($('#labtest-autocomplete'), Config.get_labitems);
    $('#labtest-autocomplete').on('selectitem.uk.autocomplete', self.set_lab_test);
    $("body").on("click", "#btn_add_labtests", self.get_data_from_modal);
    $("body").on("click", ".remove-item", self.delete_item);
    $("body").on("click", "#btn_add_lab_items", self.add_lab_item);
    $("body").on("click", ".labitems-remove", self.remove_lab_items);
    $("body").on("click", ".btn_view_xray_result", self.show_xray_result);
    //$("#Medicines").on("click", self.show_patient_medicine_list);
    $("#btnprevMedicines").on("click", self.show_patient_medicine_list);
    $("body").on("click", "#btnAllMadicine", self.add_previous_medicine);
    $("#TreatmentStartDate").on('change', self.calculate_treatment_days);
    $('.Review').on("ifChecked", self.get_previous_patient_data);
    $('.fresh').on("ifChecked", self.get_patient_data);
    $('.History').on("ifChecked", self.show_history);
    $("body").on("click", "#btnHistory", self.show_history);
    $("#btnAddNext").on('click', self.add_medicines_on_grid);
    //$("body").on("click", "#btnViewHistory", self.get_history_details);
    $("body").on("click", ".historytable thead .btnViewHistory", self.get_history_details);
    $("body").on("click", ".btnBackToHistory", self.show_history);
    $("body").on("click", ".historytable thead .btnViewReview", self.get_previous_patient_data);
    $("body").on("click", ".btnReview", self.show_review);
    $("body").on("ifChanged", ".check-box", self.check);
    //$("#Date").on('change', self.get_patient_data_by_date);
    //$.UIkit.autocomplete($('#name-autocomplete'), Config.get_diagnosis_list);
    //$('#name-autocomplete').on('selectitem.uk.autocomplete', self.set_diagnosis);
    $("body").on("click", ".btneditdepartment", self.edit_department);
    $("body").on("click", "#btnEditDepartment", self.edit_department_confirm);
    $.UIkit.autocomplete($('#name-autocomplete'), Config.get_diagnosis_list);
    $('#name-autocomplete').on('selectitem.uk.autocomplete', self.set_diagnosis);
    $("body").on("click", ".historytable thead .btnView", self.get_previous_patient_data);
    //$('.baseLine_txt').on('input', self.unselect_radion_buttons)
    //$('#Weight').on('input', self.calculate_BMI);
    $(document).on('keyup', '#Weight', self.calculate_BMI);
    $("#btnOKEmployee").on("click", self.select_employee);
    $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
    $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $("body").on("click", "#btn_adddoctor", self.add_doctor);

    $("#btnAdd").on("click", self.add_items_to_grid);
    $("body").on("click", ".remove-item", self.delete_item);
    // $("body").on("ifChanged", ".chk-lab-test", self.get_checked_labItems);
    $("body").on("ifChanged", ".MorningTime", self.check_morningTime);
    $("body").on("ifChanged", ".NoonTime", self.check_noonTime);
    $("body").on("ifChanged", ".EveningTime", self.check_eveningTime);
    $("body").on("ifChanged", ".NightTime", self.check_nightTime);
    $("body").on("ifChanged", ".Treatmentfrequency", self.calculate_treatment_day);
    $("body").on("click", ".remove-medicines-grid", self.remove_medicine);
    $("body").on("click", "#btnViewMedicineStock", self.view_medicine);
    $("body").on("click", "#btn_stock_modal_close", self.close_medicine_stock_modal);
    $("body").on("click", ".btncanceltreatment", self.show_cancel_treatment);
    $("body").on("click", "#btn-cancel-confirm", self.cancel_treatment);
    $("body").on('ifChanged', '.check-box', self.disable_checkbox);
},
PatientDiagnosis.today_list= function ($list, type) {
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs($list);

        var url = "/AHCMS/PatientDiagnosis/GetManagePatientList?type=" + type

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
                       + "<input type='hidden' class='IsReferedIP' value='" + row.IsReferedIP + "'>"
                       + "<input type='hidden' class='IsWalkin' value='" + row.IsWalkin + "'>"
                       + "<input type='hidden' class='ReviewID' value='" + row.ReviewID + "'>";;

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
                    var reviewID = $(this).closest("tr").find("td .ReviewID").val();
                    var IsWalkin = $(this).closest("tr").find("td .IsWalkin").val();
                    app.load_content("/AHCMS/PatientDiagnosis/CreateV2/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP + "&ReviewID=" + reviewID + "&IsWalkin=" + IsWalkin);
                });
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};

PatientDiagnosis.previous_List = function ($list, type) {
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
                "url": "/AHCMS/PatientDiagnosis/GetManagePatientList?type=" + type,
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
                        + "<input type='hidden' class='IsReferedIP' value='" + row.IsReferedIP + "'>"
                        + "<input type='hidden' class='IsWalkin' value='" + row.IsWalkin + "'>"
                        + "<input type='hidden' class='ReviewID' value='" + row.ReviewID + "'>";
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
                    var reviewID = $(this).closest("tr").find("td .ReviewID").val();
                    var IsWalkin = $(this).closest("tr").find("td .IsWalkin").val();
                    app.load_content("/AHCMS/PatientDiagnosis/CreateV2/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP + "&ReviewID=" + reviewID + "&IsWalkin=" + IsWalkin);
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

PatientDiagnosis.Save = function () {
    var self = PatientDiagnosis;
    var data;
    data = self.get_data();
    $.ajax({
        url: '/AHCMS/PatientDiagnosis/SaveV2',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Saved Successfully");
                setTimeout(function () {
                    window.location = "/AHCMS/PatientDiagnosis/IndexV2";
                }, 1000);
            }
            else {
                app.show_error("Failed to Create");
                $(" .btnSave").css({ 'visibility': 'visible' });

            }
        }
    });
};
PatientDiagnosis.get_data = function () {

    var self = PatientDiagnosis;
    var data = {};
    data.AppointmentScheduleItemID = $("#AppointmentScheduleItemID").val();
    data.AppointmentType = $(".op:checked").val();
    data.VisitType = $(".fresh:checked").val();
    data.PatientID = $("#PatientID").val(),
    data.ParentID = $("#ParentID").val(),
    data.ReviewID = $("#ReviewID").val(),
    data.Date = $("#Date option:selected").val();
    data.AppointmentProcessID = $("#AppointmentProcessID").val(),

    self.VitalChartItems.push({
        BP: $("#BP").val(),
        Pulse: $("#Pulse").val(),
        Temperature: $("#Temperature").val(),
        Unit: $("#Unit").val(),
        HR: $("#HR").val(),
        RR: $("#RR").val(),
        Height: $("#Height").val(),
        Weight: $("#Weight").val(),
        BMI: $("#BMI").val(),
        RespiratoryRate: $("#RespiratoryRate").val(),
        Others: $("#Others").val()
    });
    data.NextVisitDate = $("#NextVisitDate").val();
    data.Remark = $("#Remark").val();
    data.ExaminationItems = [];
    data.ReportItems = [];
    data.DoctorList = [];
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
    data.QuestionnaireItems = [];
    $('.IsIP').prop("checked") == true ? data.IsReferedIP = true : data.IsReferedIP = false;
    $('.IsCompleted').prop("checked") == true ? data.IsCompleted = true : data.IsCompleted = false;
    $('.Iswalkin').prop("checked") == true ? data.IswalkIn = true : data.IswalkIn = false;
    var item = {};
    $('#examination-list tbody .examination-results').each(function () {
        item = {};

        item.ID = $(this).parent().prevAll(".ID").val();
        //item.ID = $(this).parent().find(".ID").val();
        item.Value = $(this).parent().find(".value").val();
        item.Description = $(this).parent().find(".description").val();
        data.ExaminationItems.push(item);
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
                var row = $(this).closest('tr');
                var txt = $(row).find(".baseLine_txt").val();
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
        item.Name = $(this).find(".ReportName").text();
        item.Description = $(this).find(".Description").text();
        item.DocumentID = $(this).find(".ReportID").val();
        item.Date = $(this).find(".Date").text();
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
            if ($(this).is('input:text')) {
                var row = $(this).closest('tr');
                var txt = $(row).find(".Roga_Pareeksha_txt").val();
                item.Name = Groupname;
                //var txt = $(".case_sheet_txt").val();
                //var txt = $(this).find('.case_sheet_txt').text();
                //if (txt != "") {
                item.Description = txt;
                data.RogaPareekshaItems.push(item);
                //}

            }
        }
    });
    //casesheet tab
    $('#case-sheet-list tbody tr .casesheetResults').each(function (i, row) {
        item = {};
        var groupname = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text')) || $(this).is('textarea')) {
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


    $('#questionnaire-list tbody tr .questionnaire-Results').each(function (i, row) {
        item = {};
        var question = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text'))) {
            if ($(this).is(':checked')) {
                item.Question = question;
                item.Answer = $(this).val();
                data.QuestionnaireItems.push(item);
            }
            if ($(this).is('input:text')) {
                item.Question = question;
                var txt = $(this).val();
                if (txt != "") {
                    item.Answer = txt;
                    data.QuestionnaireItems.push(item);
                }

            }
        }
    });
    data.TreatmentMedicines = self.Items;

    data.Medicines = self.Medicine;

    data.MedicineItems = self.MedicinesItems;

    data.VitalChartItems = self.VitalChartItems;

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

    $('#add_lab_items tbody tr.X-ray').each(function () {
        item = {};
        item.XrayID = $(this).find(".LabTestID").val();
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
PatientDiagnosis.get_questionnaire = function () {
    var self = PatientDiagnosis;
    var PatientID = $('#PatientID').val();
    var AppointmentProcessID = $('#AppointmentProcessID').val();
    $("#questionnaire-list tbody").empty();
    $.ajax({
        url: '/AHCMS/Screening/Questionnaire/',
        dataType: "json",
        data: {
            PatientID: PatientID,
            AppointmentProcessID: AppointmentProcessID
        },
        type: "POST",
        success: function (response) {
            var content = "";
            var $content;
            length = response.Data.length;
            $(response.Data).each(function (i, item) {

                var slno = (i + 1);
                content += '<tr>'
                    + '<td class="sl-no uk-text-center">' + slno + '</td>'
                    + '<td class="question">' + item.Question
                    + '<input type="hidden" class="questionID "value="' + item.QuestionID + '"/>' + '</td>'
                   + '<td class="value">' + item.Answer + '</td>'
                + '<td>'
                + '<a class="remove-item">'
                + '<i class="uk-icon-remove added"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';
            });
            $content = $(content);
            app.format($content);
            $("#questionnaire-list tbody").html($content);
        },
    });
};
PatientDiagnosis.validate_add = function () {
    var self = PatientDiagnosis;
    if (self.rules.on_question_add.length > 0) {
        return form.validate(self.rules.on_question_add);
    }
    return 0;
};
PatientDiagnosis.add_items_to_grid = function () {
    var self = PatientDiagnosis;
    sino = $('#questionnaire-list tbody tr').length + 1;
    self.error_count = 0;
    self.error_count = self.validate_add();
    if (self.error_count > 0) {
        return;
    }
    var QuestionID = $('#QuestionID').val();
    var Question = $("#QuestionID option:selected").text();
    var Answer = $("#Answer").val();
    content = '<tr>'
                + '<td class="sl-no uk-text-center">' + sino + '</td>'
                + '<td class="question">' + Question
                + '<input type="hidden" class="questionID "value="' + QuestionID + '"/>' + '</td>'
                + '<td class="value">' + Answer + '</td>'
                + '<td>'
                + '<a class="remove-item">'
                + '<i class="uk-icon-remove added"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';
    $content = $(content);
    app.format($content);
    $('#questionnaire-list tbody').append($content);
    $("#QuestionID").val('');
    $("#Answer").val('');

};
PatientDiagnosis.delete_item = function () {
    var self = PatientDiagnosis;
    $(this).closest('tr').remove();
    var sino = 0;
    $('#questionnaire-list tbody tr .sl-no').each(function () {
        sino = sino + 1;
        $(this).text(sino);
    });
};

PatientDiagnosis.rules = {

    on_medicine_add: [
         {
             elements: "#item-medicine-count",
             rules: [
                 { type: form.non_zero, message: "Please add atleast one medicine" },
                { type: form.required, message: "Please add atleast one medicine" },

             ]
         },
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

    ],

    on_add_lab_tests: [
   {
       elements: "#LabTest",
       rules: [
       { type: form.required, message: "Please Select Lab Test" },
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
        //        { type: form.required, message: "Please select Therapist" },
        //    ]
        //},
         {
             elements: "#TreatmentRoomID",
             rules: [
                 { type: form.required, message: "Please select Treatment Room" },
             ]
         },
         {
             elements: "#TentativeStartDate",
             rules: [
                 { type: form.required, message: "Please select StartDate" },
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
                 { type: form.required, message: "Please select No of Treatment" },
             ]
         },
          {
              elements: "#TreatmentID",
              rules: [
                  { type: form.required, message: "Treatment Name is Invalid" },
                  { type: form.non_zero, message: "Treatment Name is Invalid" },
              ]
          },
          //{
          //    elements: "#treatment-medicine-count",
          //    rules: [
          //        { type: form.non_zero, message: "Please add atleast one medicine" },
          //       { type: form.required, message: "Please add atleast one medicine" },

          //    ]
          //},
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

        {
            elements: "#Qty",
            rules: [
                { type: form.required, message: "Please enter Quantity" },
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
          {
              elements: ".Ayurvedic",
              rules: [
                  {
                      type: function (element) {
                          var error = false;
                          if (($(".IsIP").prop('checked') == true) || ($(".IsCompleted").prop('checked') == true) || ($(".Iswalkin").prop('checked') == true)) {
                              if ($(".Ayurvedic").val() != '') {
                                  error = false;
                              }
                              else {
                                  error = true;
                              }
                          }
                          return !error;
                  },
                   message: "Please Add Ayurvedic Diagnosis" },
              ]
          },
    ],
    on_question_add: [
         {
             elements: ".questionID",
             rules: [
                 {
                     type: function (element) {
                         var error = false;
                         $("#questionnaire-list tbody tr").each(function () {
                             var a = ($(this).find(".questionID").val().trim());
                             var QuestionID = $("#QuestionID").val();
                             if ($(this).find(".questionID").val().trim() == QuestionID) {
                                 error = true;
                             }
                         });
                         return !error;
                     }, message: "Already answered to this Question"
                 },
             ]
         },

           {
               elements: "#Answer",
               rules: [
                   { type: form.required, message: "Please Enter the Answer" },
               ]
           },
    ],
};
PatientDiagnosis.get_previous_patient_data= function () {
    var self = PatientDiagnosis;
    var ParentID = $(this).closest("tr").find(".ParentID").val();
    var ReviewID = $(this).closest("tr").find(".ReviewID").val();

    $("#ParentID").val(ParentID);
    $("#ReviewID").val(ReviewID);
    $("#ReviewStatus").val("Review");
    var date = $('#Date option:selected').val();
    var nowDate = new Date();
    var nowDay = ((nowDate.getDate().toString().length) == 1) ? '0' + (nowDate.getDate()) : (nowDate.getDate());
    var nowMonth = nowDate.getMonth() < 9 ? '0' + (nowDate.getMonth() + 1) : (nowDate.getMonth() + 1);
    var nowYear = nowDate.getFullYear();
    var formattedDate = nowDay + "-" + nowMonth + "-" + nowYear;
    if (date != formattedDate) {
        $("#btnAddMedicine").hide();
        $("#btnaddtreatment").hide();
        $("#btnaddreport").hide();
        //$(".btnSave").hide();
        $("#btn_add_lab_items").hide();

    }
    else {
        $("#btnAddMedicine").show();
        $("#btnaddtreatment").show();
        $("#btnaddreport").show();
        $(".btnSave").show();
    }
    //var tabHistoryContents = document.getElementsByClassName("tabhistory");
    //for (var i = 0; i < tabHistoryContents.length; i++) {
    //    tabHistoryContents[i].style.display = 'none';
    //}
    //var tabContents = document.getElementsByClassName("tabContent");
    //for (var i = 0; i < tabContents.length; i++) {
    //    tabContents[i].style.display = 'block';
    //}
    app.confirm("Data you Entered Will be Cleared", function () {
        self.get_previous_examination(ReviewID);
        //self.get_examination();
        self.get_vital_chart(ReviewID);
        self.get_chart(ReviewID);
        self.get_vitals(ReviewID);
        //self.get_medicine();
        self.MedicineReview(ReviewID);
        self.get_medicines_details(ReviewID);
        self.get_medicines_Items(ReviewID);
        self.get_treatment_item();
        self.get_treatment();
        self.get_casesheet();
        if($("#IsCompleted").val() == "true"){
             self.GetCaseSheetList(ReviewID);
        }       
        self.get_LabItems();
        self.get_report();
        self.get_doctor_list();
    });
    $(".tabHistory").addClass("uk-hidden");
    $(".HistoryDetails").addClass("uk-hidden");
    $(".tabReview").removeClass("uk-hidden");
};
PatientDiagnosis.GetCaseSheetList = function (ReviewID) {
    var self = PatientDiagnosis;
    var PatientID = $('#PatientID').val();
    var date = $('#Date option:selected').val();
    var AppointmentProcessID = ReviewID;
    $.ajax({
        url: "/AHCMS/PatientDiagnosis/CaseSheetV2",
        data: {
            PatientID: PatientID,
            Date:date,
            AppointmentProcessID: AppointmentProcessID,
            ReviewID:AppointmentProcessID
        },
        dataType: "html",
        type: "POST",
        success: function (response) {
            $("#case-sheet-list tbody").empty();
            $response = $(response);
            app.format($response);
            $("#case-sheet-list tbody").append($response);

        }
    });
};

//PatientDiagnosis.get_data_from_modal= function () {
//    var self = PatientDiagnosis;
//    var data = {};
//    PrescribedTest = [];
//    var LabTestCategoryIDs = [];
//    for (var i = 0; i < self.LabItemList2.length; i++) {
//        var item = {};       
//        item.ID = self.LabItemList2[i].id;
//        item.Name = self.LabItemList2[i].name;
//        item.Type = self.LabItemList2[i].type;
//        item.Date = $("#TestDate").val();       
//        if (item.Type == "Lab Item Group") {
//           LabTestCategoryIDs.push(item.ID);
//        } else {
//            self.PrescribedTest.push(item);
//        }
//    }
//    self.add_data_to_grid();
//    if (LabTestCategoryIDs.length != 0) {
//        self.getCategorywiseLabItems(LabTestCategoryIDs);
//    }  
//    $("#labtest-list tbody tr.included").each(function () {
//        $(this).find('.ItemID').parent('div').removeClass("checked");
//        $(this).removeClass('included');
//    });
//    self.PrescribedTest = [];
//    self.LabItemList = [];
//    self.LabItemList2 = []; 
//    LabTestCategoryIDs = [];
//    return data;
//}
//PatientDiagnosis.check= function () {
//    var self = PatientDiagnosis
//    var row = $(this).closest('tr');
//    if ($(this).is(":checked")) {
//        //$(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
//        $(this).closest('tr').addClass('included');
//        self.LabItemList.push(clean($(row).find('.chk-lab-test').val()));
//        self.LabItemList2.push({ 'id': clean($(row).find('.chk-lab-test').val()), 'name': $(row).find('.Name').text(), 'type': $(row).find('.Type').text() });
//    } else {
//        //$(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
//        $(this).closest('tr').removeClass('included');
//        $(this).removeAttr('disabled');
//        var value = $(row).find('.chk-lab-test').val();
//        var i = 0;
//        while (i < self.LabItemList.length) {
//            if (self.LabItemList[i] == value) {
//                self.LabItemList.splice(i, 1);
//                self.LabItemList2.splice(i, 1);
//            } else {
//                ++i;
//            }
//        }
//    }
//};
//PatientDiagnosis.getCategorywiseLabItems = function (categoryID) {
//    var self = PatientDiagnosis;  
//    $.ajax({
//        url: "/AHCMS/PatientDiagnosis/GetCategoryWiseLabItems",
//        dataType: "json",
//        data: {          
//            LabTestCategoryID: categoryID
//        },  
//        type: "POST",
//        success: function (response) {
//            if (response.Status == "success") {
//                var item;
//                $(response.Data).each(function (i, record) {
//                    item=[];
//                    item.ID = record.ID;
//                    item.Name = record.ItemName;
//                    item.Type = record.Type;
//                    item.Date = $("#TestDate").val();

//                    self.PrescribedTest.push(item);
//                });
//                self.add_data_to_grid();
//                self.PrescribedTest = [];
//            }
//        }
//    });
//};