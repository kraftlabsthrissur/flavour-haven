PatientDiagnosis = {
    init: function () {
        var self = PatientDiagnosis;        
        self.is_patient_history();        
        //self.get_report();
        //self.get_medicine();
        //self.get_LabItems();
        //self.get_chart();
        //self.get_casesheet();
        //self.get_vital_chart();
        self.bind_events();       
        //self.get_vitals();       
        //self.get_medicines_details();
        //self.get_medicines_Items();
        //self.get_treatment_item();
        //self.get_treatment();
        self.lablist();
        //self.disable_checkbox();
        //$('#labtest-list').SelectTable({
        //    modal: "#select-labtest",
        //    selectFunction: self.select_employee,
        //    initiatingElement: "#PartyName"
        //});

        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"

        });
        self.list();       
    },

    list: function () {
        var self = PatientDiagnosis;

        $('#tabs-case-sheet').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = PatientDiagnosis;
        var $list;

        switch (type) {
            case "Previous":
                $list = $('#previous-list');
                break;
            case "Today":
                $list = $('#today-list');
                break;
            case "Walkin":
                $list = $('#walkin-list');
                break;
            case "ReferedToIP":
                $list = $('#IP-list');
                break;
            case "Completed":
                $list = $('#completed-list');
                break;
            default:
                $list = $('#today-list');
        }

        if (type == "Previous") {
            self.previous_List($list, type);
        }

        else {
            self.today_list($list, type);
        }
    },

    today_list: function ($list, type) {
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
                        var OPID = $(this).closest("tr").find("td .OPID").val();
                        var IsCompleted = $(this).closest("tr").find("td .IsCompleted").val();
                        var IsReferedIP = $(this).closest("tr").find("td .IsReferedIP").val();
                        var reviewID = $(this).closest("tr").find("td .ReviewID").val();
                        var IsWalkin = $(this).closest("tr").find("td .IsWalkin").val();
                        app.load_content("/AHCMS/PatientDiagnosis/Create/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP + "&ReviewID=" + reviewID + "&IsWalkin=" + IsWalkin);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    previous_List: function ($list, type) {
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
                        app.load_content("/AHCMS/PatientDiagnosis/Create/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP + "&ReviewID=" + reviewID + "&IsWalkin=" + IsWalkin);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;
        }
    },

    bind_events: function () {
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
        $("#btnshowSchedule").on("click", self.show_treatment_schedule);
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
       // $("#Date").on('change', self.get_patient_data_by_date);
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
        $(document).on('keyup', '#Height', self.calculate_BMI);
        $("#btnOKEmployee").on("click", self.select_employee);
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $("body").on("click", "#btn_adddoctor", self.add_doctor);

        $("body").on("ifChanged", ".MorningTime", self.check_morningTime);
        $("body").on("ifChanged", ".NoonTime", self.check_noonTime);
        $("body").on("ifChanged", ".EveningTime", self.check_eveningTime);
        $("body").on("ifChanged", ".NightTime", self.check_nightTime);

        $("body").on("ifChanged", ".Treatmentfrequency", self.calculate_treatment_day);
        $("body").on("click", "#btnViewMedicineStock", self.view_medicine);
        $("body").on("click", "#btn_stock_modal_close", self.close_medicine_stock_modal);
        $("body").on("click", ".remove-medicines-grid", self.remove_medicine);
        $("body").on("click", ".btncanceltreatment", self.show_cancel_treatment);
        $("body").on("click", "#btn-cancel-confirm", self.cancel_treatment);
        $("body").on('ifChanged', '.check-box', self.disable_checkbox);
    },

    disable_checkbox:function()
    {
        var self = PatientDiagnosis;
        if ($(".IsIP").prop('checked') == true)
        {
            $(".IsCompleted").prop("disabled", true);
            $(".Iswalkin").prop("disabled", true);
        }
        else if ($(".IsCompleted").prop('checked') == true) {
            $(".IsIP").prop("disabled", true);
            $(".Iswalkin").prop("disabled", true);
        }
        else if ($(".Iswalkin").prop('checked') == true)
        {
            $(".IsCompleted").prop("disabled", true);
            $(".IsIP").prop("disabled", true);
        }
        else {
            $(".IsCompleted").prop("disabled", false);
            $(".IsIP").prop("disabled", false);
            $(".Iswalkin").prop("disabled", false);
        }
    },

    show_cancel_treatment: function () {
        var self = PatientDiagnosis;
        row = $(this).closest("tr"); Treatment
        var PatientTreatmentID = $(row).find('.PatientTreatmentID').val();
        var Treatment = $(row).find('.Treatment').text();
        var TreatmentID = $(row).find('.TreatmentID').val();
        var TherapistID = $(row).find('.TherapistID').val();
        var StartDate = $(row).find('.StartDate').text();
        var EndDate = $(row).find('.EndDate').text();

        $('#TentativeEndDateForCancel').val(EndDate);
        $('#TreatmentIDForCancel').val(TreatmentID);
        $('#PatientTreatmentID').val(PatientTreatmentID);
        $('#TreatmentNameForCancel').val(Treatment);
       
        $('#show-cancel-treatment').trigger('click');      
    },
    cancel_treatment: function () {
        var self = PatientDiagnosis;
        var EndDate = $('#TentativeEndDateForCancel').val();
        var TreatmentID=$('#TreatmentIDForCancel').val();
        var PatientTreatmentID = $('#PatientTreatmentID').val();
        $('#TreatmentNameForCancel').val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/CancelTreatment',
            dataType: "json",
            type: "POST",
            data: {
                PatientTreatmentID: PatientTreatmentID,
                TreatmentID: TreatmentID, 
                EndDate: EndDate
            },
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Cancelled Successfully");
                    UIkit.modal($('#cancel-treatment')).hide();
                }
            }
        });
    },
    calculate_treatment_day: function () {
        var self = PatientDiagnosis;        
        var frequency = ($('.Treatmentfrequency:checked').size());
        var numberOfTreatments = $("#NoofTreatment").val();
        if (frequency ==0){
            frequency = 1;
        }
        let currentNumberOfTreatments = (numberOfTreatments - numberOfTreatments % frequency) / frequency +
            (numberOfTreatments % frequency / frequency > 1 ? 2 : numberOfTreatments % frequency / frequency !== 0 ? 1 : 0)
        $("#Noofdays").val(currentNumberOfTreatments)
        currentNumberOfTreatments=currentNumberOfTreatments - 1;
        var date = $("#TreatmentStartDate").val();

        var FromDate = app.get_date_time(date);
        var ToDate = new Date();

        ToDate.setTime(FromDate + currentNumberOfTreatments * 24 * 60 * 60 * 1000);
        var day = ((ToDate.getDate().toString().length) == 1) ? '0' + (ToDate.getDate()) : (ToDate.getDate());
        var month = (ToDate.getMonth() < 9 ? '0' : '') + (ToDate.getMonth() + 1);
        var year = ToDate.getFullYear();
        FromattedDate = day + '-' + month + '-' + year;

        $("#TentativeEndDate").val(FromattedDate);
    },

    check_morningTime: function () {
        var self = PatientDiagnosis;
        if ($(this).prop("checked") == true) {
            $("#MorningTimeID ").prop("disabled", false);
        }
        else {
            $("#MorningTimeID").val('');
            $("#MorningTimeID ").prop("disabled", true);
        }
    },

    check_noonTime: function () {
        var self = PatientDiagnosis;
        if ($(this).prop("checked") == true) {
            $("#NoonTimeID").prop("disabled", false);
        }
        else {
            $("#NoonTimeID").val('');
            $("#NoonTimeID").prop("disabled", true);
        }
    },

    check_eveningTime: function () {
        var self = PatientDiagnosis;
        if ($(this).prop("checked") == true) {
            $("#EveningTimeID").prop("disabled", false);
        }
        else {
            $("#EveningTimeID").val('');
            $("#EveningTimeID").prop("disabled", true);
        }
    },

    check_nightTime: function () {
        var self = PatientDiagnosis;
        if ($(this).prop("checked") == true) {
            $("#NightTimeID").prop("disabled", false);
        }
        else {
            $("#NightTimeID").val('');
            $("#NightTimeID").prop("disabled", true);
        }
    },


    LabItemList: [],
    LabItemList2: [],
    lablist: function () {
        var $list = $('#labtest-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/PatientDiagnosis/GetLabTestList"

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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>"
                                + "<input type='hidden' class='Price' value='" + row.Price + "'>"
                                 + "<input type='hidden' class='Category' value='" + row.Category + "'>";

                       }
                   },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            var checked = "";
                            var self = PatientDiagnosis;
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
    },
    get_doctor: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#DoctorName').val(),
                EmployeeCategoryID: $('#EmployeeCategoryID').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_doctor: function (event, item) {
        var self = PatientDiagnosis;
        $("#DoctorNameID").val(item.id),
        $("#DoctorName").val(item.name);
        UIkit.modal($('#select-employee')).hide();
    },

    select_employee: function () {
        var self = PatientDiagnosis;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorNameID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    set_diagnosis: function (event, item) {
        var self = PatientDiagnosis;
        $("#Diagnosis").val(item.Name);
        $("#DiagnosisID").val(item.id);
    },

    unselect_radion_buttons: function () {
        var self = PatientDiagnosis;
        var baseline = $(this).attr('name');
        $('input[name=' + baseline + ']:checked').each(function (i) {
            this.checked = false;
        });
        $('input[name=' + baseline + ']').attr('checked', false);
        //$(input[name = baseline]).removeAttr('checked');
        //$("input[name=thename]").prop("checked", false);
        //$('input:radio[name=' + baseline + ']').prop('checked', false);;
        //$("input[name='+baseline+']").prop("checked", false);
    },

    get_data_for_department: function () {
        var self = PatientDiagnosis;
        var model = {
            PatientID: $("#PatientID").val(),
            AppointmentScheduleItemID: $("#AppointmentScheduleItemID").val(),
            DepartmentID: $("#DepartmentID").val(),
        }
        return model;
    },

    department_edit: function () {
        var self = PatientDiagnosis;
        var data;
        data = self.get_data_for_department();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/EditDepartment',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "Success") {
                    app.show_notice("Saved Successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/PatientDiagnosis/Index";
                    }, 1000);
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    edit_department_confirm: function () {
        var self = PatientDiagnosis;
        app.confirm_cancel("Do you want to Edit Department", function () {
            self.department_edit();
        }, function () {
        })
    },

    edit_department: function () {
        var self = PatientDiagnosis;
        var AppointmentScheduleItemID = $(this).parents('tr').find('.TransID').val();
        self.get_department_items(AppointmentScheduleItemID);
    },

    get_department_items: function (AppointmentScheduleItemID) {
        var self = PatientDiagnosis;
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetDepartmentItems',
            dataType: "json",
            type: "POST",
            data: {
                AppointmentScheduleItemID: AppointmentScheduleItemID
            },
            success: function (response) {
                if (response.Status == "success") {
                    $("#Date").val(response.Data.Date)
                    $("#PatientName").val(response.Data.PatientName)
                    $("#PatientID").val(response.Data.PatientID)
                    $("#DepartmentID").val(response.Data.DepartmentID)
                    $("#AppointmentScheduleItemID").val(AppointmentScheduleItemID)
                }
                $('#show-edit-department').trigger('click');
            }
        });
    },

    get_patient_data_by_date: function () {
        var self = PatientDiagnosis;
        var date = $('#Date option:selected').val();
        var nowDate = new Date();
        var nowDay = ((nowDate.getDate().toString().length) == 1) ? '0' + (nowDate.getDate()) : (nowDate.getDate());
        var nowMonth = nowDate.getMonth() < 9 ? '0' + (nowDate.getMonth() + 1) : (nowDate.getMonth() + 1);
        var nowYear = nowDate.getFullYear();
        var formattedDate = nowDay + "-" + nowMonth + "-" + nowYear;
        $('#ReviewID').val(0);
        if (date != formattedDate) {
            $("#btnAddMedicine").hide();
            $("#btnaddtreatment").hide();
            $("#btnaddreport").hide();
            $(".btnSave").hide();
            $("#btn_add_lab_items").hide();

        }
        else {
            $("#btnAddMedicine").show();
            $("#btnaddtreatment").show();
            $("#btnaddreport").show();
            $(".btnSave").show();
        }

        app.confirm("Data you Entered Will be Cleared", function () {
            self.get_report();
            self.get_medicine();
            self.get_LabItems();
            self.get_chart();
            self.get_casesheet();
            self.get_vital_chart();
            self.get_medicines_details();
            self.get_medicines_Items();
            self.get_treatment_item();
            self.get_treatment();
            self.get_history();
            self.get_examination();
            self.get_baseline_information();
        });
        $(".tabHistory").addClass("uk-hidden");
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabReview").removeClass("uk-hidden");
    },

    show_review: function () {
        var self = PatientDiagnosis;
        //var ParentID = $(this).closest("tr").find(".ParentID").val();
        //var ReviewID = $(this).closest("tr").find(".ReviewID").val();

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
        app.confirm("Data you Entered Will be Cleared", function () {
            self.get_examination();
            //self.get_previous_examination(ReviewID);
            self.get_vital_chart();
            self.get_chart();
            self.get_medicine();
            //self.MedicineReview(ReviewID);
            self.get_medicines_details();
            self.get_medicines_Items();
            self.get_treatment_item();
            self.get_treatment();
            self.get_casesheet();
            self.get_LabItems();
            self.get_report();
        });
        $(".tabHistory").addClass("uk-hidden");
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabReview").removeClass("uk-hidden");
    },

    get_history_details: function () {
        var self = PatientDiagnosis;
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        var IPID = $(this).closest("tr").find(".IPID").val();
        var ParentID = $(this).closest("tr").find(".ParentID").val();
        var AppointmentType = $(this).closest("tr").find(".AppointmentType").val();
        var ReviewID = $(this).closest("tr").find(".ReviewID").val();
        var IsCompleted = $("#IsCompleted").val();
        $("#ReviewID").val(ReviewID);
        $("#ParentID").val(ParentID);
        $(".HistoryDetails").removeClass("uk-hidden");
        $(".tabHistory").addClass("uk-hidden");
        $(".tabReview").addClass("uk-hidden");
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/HistoryDetails",
            dataType: "html",
            data: {
                ParentID: ParentID,
                OPID: OPID,
                IPID: IPID,
                PatientID: PatientID,
                AppointmentType: AppointmentType,
                IsCompleted: IsCompleted
            },
            type: "POST",
            success: function (response) {
                $("#history-details").empty();
                $response = $(response);
                app.format($response);
                $("#history-details").append($response);
            },
        });
    },

    show_history: function () {
        var self = PatientDiagnosis;
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabHistory").removeClass("uk-hidden");
        $(".tabReview").addClass("uk-hidden");
        if ($(".fresh").is(":checked") == true) {
            $('.fresh').iCheck('uncheck')
        };
        //var tabHistoryContents = document.getElementsByClassName("tabhistory");
        //for (var i = 0; i < tabHistoryContents.length; i++) {
        //    tabHistoryContents[i].style.display = 'block';
        //}
        //var tabContents = document.getElementsByClassName("tabContent");
        //for (var i = 0; i < tabContents.length; i++) {
        //    tabContents[i].style.display = 'none';
        //}
        //document.getElementsByClassName("history").style.display = "block";
        //document.getElementById("Examination").style.display = "none";  
    },

    get_all_medicines: function () {
        var self = PatientDiagnosis;
        var PatientID = $("#PatientID").val()
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetPatientMedicines',
            dataType: "json",
            data: {
                PatientID: PatientID
            },
            type: "POST",
            success: function (response) {
                $('#add_all_medicine_list tbody tr ').empty();
                $(response.Medicine).each(function (i, record) {
                    //$('#add_all_medicine_list tbody tr ').empty();
                    var sino = $('#add_all_medicine_list tbody tr ').length + 1;
                    var content = "";
                    var $content;
                    content = '<tr>'
                        + '<td class="labitem-serial-no uk-text-center slno">' + sino
                        + '<input type="hidden" class="MedicineID"value="' + record.MedicineID + '"/>'
                        + '<td class="">' + "<input type='checkbox' class='uk-radio' data-md-icheck value = '" + record.PatientMedicinesID + "'/>" + '</td>'
                        + '<td class="Medicine">' + record.Medicine + '</td>'
                        + '<td class="Unit">' + record.Unit + '</td>'
                        + '<td class="Qty">' + record.Qty + '</td>'
                        + '<td class="Instruction">' + record.Instructions + '</td>'
                        + '<td class="NoofDays">' + record.NoofDays + '</td>'
                        + '<td class="">' + record.StartDate + '</td>'
                        + '<td class="">' + record.EndDate + '</td>'
                        + '</tr>';
                    $content = $(content);
                    app.format($content);
                    $('#add_all_medicine_list tbody').append($content);
                });
            }
        });

    },

    get_previous_medicine: function (PatientMedicinesID) {
        var self = PatientDiagnosis;
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/PreviousMedicines",
            dataType: "html",
            data: {
                PatientMedicinesID: PatientMedicinesID,
            },
            type: "POST",
            success: function (response) {
                $("#add_medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_medicine-list tbody").append($response);
            },
        });
    },

    add_doctor: function () {
        var self = PatientDiagnosis;
        self.error_count = self.validate_doctor();
        if (self.error_count > 0) {
            return;
        }
        sino = $('#add_doctor tbody tr').length + 1;
        var DoctorName = $("#DoctorName").val();
        var DoctorID = $("#DoctorNameID").val();
        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="sl-no uk-text-center">' + sino + '</td>'
            + '<td class="DoctorName">' + DoctorName
            + '<input type="hidden" class="DoctorNameID"value="' + DoctorID + '"/>'
            + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove added"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_doctor tbody').append($content);
        self.clear_doctor_data();
    },

    clear_doctor_data: function () {
        var self = PatientDiagnosis;
        $("#DoctorName").val('');
        $("#DoctorNameID").val('');
    },

    get_previous_medicines_details: function (PatientMedicinesID) {
        var self = PatientDiagnosis;
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetPreviousMedicinesList/',
            dataType: "json",
            data: {
                PatientMedicinesID: PatientMedicinesID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.Medicine.push({
                            MedicinesID: record.MedicineID, UnitID: record.UnitID, Unit: record.Unit, Medicines: record.Medicine,
                            GroupID: 0, Quantity: record.Quantity, PatientMedicinesID: 0
                        });
                    });
                }
            },
        });
    },

    get_previous_medicines_Items: function (PatientMedicinesID) {
        var self = PatientDiagnosis;
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetPreviousMedicinesItemsList/',
            dataType: "json",
            data: {
                PatientMedicinesID: PatientMedicinesID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.MedicinesItems.push({
                            NoofDays: record.NoofDays, IsMorning: record.IsMorning, IsNoon: record.IsNoon, Isevening: record.Isevening,
                            IsNight: record.IsNight, MorningTime: record.MorningTime, NoonTime: record.NoonTime, EveningTime: record.EveningTime,
                            NightTime: record.NightTime, IsEmptyStomach: record.IsEmptyStomach, IsBeforeFood: record.IsBeforeFood,
                            IsAfterFood: record.IsAfterFood, Description: record.Description, GroupID: 0, MorningTimeID: record.MorningTimeID,
                            EveningTimeID: record.EveningTimeID, NoonTimeID: record.NoonTimeID, NightTimeID: record.NightTimeID,
                            StartDate: record.StartDate, EndDate: record.EndDate, ModeOfAdministrationID: record.ModeOfAdministrationID, Frequency: record.Frequency,
                            PatientMedicineID: 0
                        });
                    });
                }
            },
        });
    },

    add_previous_medicine: function () {
        var self = PatientDiagnosis;
        var PatientMedicinesID = [];
        $('#show-all-medicine').trigger('click');
        var checkboxes = $('#add_all_medicine_list tbody input[type="checkbox"]:checked');
        $("#add_all_medicine_list tbody input[type='checkbox']:checked ").each(function () {
            var row = $(this).closest('tr');

        });
        if ($("#add_all_medicine_list tbody tr").length > 0) {
            $.each(checkboxes, function () {
                row = $(this).closest("tr");
                PatientMedicinesID.push($(this).val());
            });
            self.get_previous_medicine(PatientMedicinesID);
            self.get_previous_medicines_details(PatientMedicinesID);
            self.get_previous_medicines_Items(PatientMedicinesID);
        }
    },

    show_patient_medicine_list: function () {
        var self = PatientDiagnosis;
        $('#show-all-medicine').trigger('click');
        self.get_all_medicines();
    },

    remove_lab_items: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#add_lab_items tbody tr .slno').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
    },

    set_lab_test: function (event, item) {
        var self = PatientDiagnosis;
        $("#LabTest").val(item.Name);
        $("#LabTestID").val(item.id);
        $("#Category").val(item.category);
    },

    set_labtest: function () {
        var self = PatientDiagnosis;

        if ($(this).is(":checked") == true) {
            var LabTestID = clean($(this).val());
            self.LabTestItems.push({ LabTestID: LabTestID });
        }
        else {
            var LabTestID = clean($(this).val());
            var index = self.LabTestItems.indexOf(LabTestID)
            self.LabTestItems.splice(index, 1);
        }
    },

    get_labtest: function (OPID) {
        var self = PatientDiagnosis;
        var count;
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetCheckedTest',
            dataType: "json",
            data: {
                OPID: OPID
            },
            type: "POST",
            success: function (response) {
                $(response.LabTestID).each(function (i, record) {
                    var LabTestID = record.LabTestID;
                    var State = record.State;
                    if (State == "Completed") {
                        count = 1;
                    }
                    self.LabTestItems.push({ LabTestID: LabTestID });
                });
                if (count == undefined) {
                    $('#btn_view_result').css({ 'visibility': 'hidden' });
                }
            }
        });

    },

    show_labtestlist: function () {
        var self = PatientDiagnosis;
        $('#show-labresult').trigger('click');
    },

    set_xray_test: function () {
        var self = PatientDiagnosis;
        if ($(this).is(":checked") == true) {
            var XrayID = clean($(this).val());
            self.XrayItems.push({ XrayID: XrayID });
        }
        else {
            var XrayID = clean($(this).val());
            var index = self.XrayItems.indexOf(XrayID)
            self.XrayItems.splice(index, 1);
        }
    },

    get_xray_test: function (OPID) {
        var self = PatientDiagnosis;
        var count;
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetPrescribedXrayTest',
            dataType: "json",
            data: {
                OPID: OPID
            },
            type: "POST",
            success: function (response) {
                $(response.XrayID).each(function (i, record) {
                    var XrayID = record.XrayID;
                    var State = record.State;
                    if (State == "Completed") {
                        count = 1;
                    }
                    self.XrayItems.push({ XrayID: XrayID });
                });
                if (count == undefined) {
                    $('#btn_view_xray_result').css({ 'visibility': 'hidden' });
                }
            }
        });

    },

    show_xray_result: function () {
        var self = PatientDiagnosis;
        var row = $(this).closest("tr");
        var Name = $(row).find(".DocumentName").val();
        var URL = $(row).find(".URL").val();
        // var url = "/" + "Uploads" + "/" + path;
        $("#print-preview-title").hide();
        $("#btnOkPrint").hide();
        app.print_preview(URL);
    },

    save_confirm: function () {
        var self = PatientDiagnosis;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    get_data: function () {
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
        $('.IsIP').prop("checked") == true ? data.IsReferedIP = true : data.IsReferedIP = false;
        $('.IsCompleted').prop("checked") == true ? data.IsCompleted = true : data.IsCompleted = false;
        $('.Iswalkin').prop("checked") == true ? data.IswalkIn = true : data.IswalkIn = false;
        var item = {};
        $('#examination-list tbody .examination-results').each(function () {
            item = {};

            item.ID = $(this).closest('td').find(".ID").val();
            //item.ID = $(this).parent().find(".ID").val();
            item.Value = $(this).closest('td').find(".value").val();
            item.Description = $(this).closest('td').find(".description").val();
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
                    item.Name = baseline;
                    var txt = $(".baseLine_txt").val();
                    if (txt != "") {
                        item.Description = txt;
                        data.BaseLineItems.push(item);
                    }

                }
                //data.BaseLineItems.push(item);
            }
            //if ($(this).is('input:checkbox'))
            //{
            //    var $row = $(row);
            //    $checkedBoxes = $row.find('input:checked');
            //    data.BaseLineItems.OtherConditions = [];
            //    //$("td:eq(3) input[type=checkbox]:checked").each(function () {
            //        $('input[name=' + baseline + ']:checked').each(function () {
            //    //$(this).find('.checkboxresults').eq(3).find('input').each(function () {
            //    //$checkedBoxes.each(function (i, checkbox) {
            //    //$(this).find('td input:checked').each(function () {
            //        if ($(this).is(':checked')) {
            //            item = {};

            //            var intRegex = /^\d+$/;
            //            var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;
            //            var str = $(this).val();
            //            if(intRegex.test(str) || floatRegex.test(str)) {
            //                var num = str;
            //            }
            //            else
            //            {
            //                item.Name = baseline;
            //                item.Description = $(this).val();
            //                data.BaseLineItems.OtherConditions.push(item);
            //                //data.OtherConditionsItems.push(item);
            //            }


            //        }

            //    });

            //}





        });

        //$('#base-line-information-list tbody tr td:eq(3)').find('input[type="checkbox"]:checked').each(function () {
        //$('#base-line-information-list tbody tr').closest('td').parent().find('input[type=checkbox]').each(function () {
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
            item.Isevening = $(this).find(".IsEvening").val();
            item.IsNight = $(this).find(".IsNight").val();
            item.NoofDays = $(this).find(".NoofDays").val();
            data.TreatmentItems.push(item);
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
    },

    Save: function () {
        var self = PatientDiagnosis;
        var data;
        $(".btnSave").css({ 'display': 'none' });
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/PatientDiagnosis/Index";
                    }, 1000);

                    $(".btnSave").css({ 'display': 'block' });
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    set_medicine_name: function (event, item) {
        var self = PatientDiagnosis;
        $("#Medicines").val(item.value);
        $("#MedicinesID").val(item.id);
        $("#SalesUnitID").val(item.salesunitid);
        $("#SalesUnit").val(item.salesunit);
        $("#PrimaryUnitID").val(item.primaryunitid);
        $("#PrimaryUnit").val(item.primaryunit);
        $("#CategoryID").val(item.categoryid);
        self.get_units();
        self.Get_prescription();
        self.Get_Available_Medicine_Stock(item.id);
        $("#Qty").focus();
        self.Get_Stock();
    },
    Get_Stock: function () {
        var self = PatientDiagnosis;
        var ProductionGroupID = $('#MedicinesID').val();
        var ProductionGroup = $('#Medicines').val();
        $.ajax({
            url: '/Masters/Item/GetStockByProductionGroupID',
            data: {
                ProductionGroupID: ProductionGroupID,
                ProductionGroup: ProductionGroup
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $(response.data).each(function (i, record) {
                    $(".StockItems").removeClass('uk-hidden');
                    var x = document.getElementById("MedicineStock");
                    x.innerHTML = record.MedicineStock;                
                    //$("#MedicineStock").val(record.MedicineStock);
                    $("#TotalStock").val(record.TotalStock);
                });
                //setTimeout(function () { $(".StockItems").hide(); }, 10000);
            }
        });
    },
    Get_Available_Medicine_Stock: function (productiongroupID) {
        var self = PatientDiagnosis;
        $('#medicine-stock-list tbody').empty();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetAllMedicinesbyProductionGroup/',
            dataType: "json",
            data: {
                ProductionGroupID: productiongroupID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        var content = "";
                        var $content;
                        content = '<tr>'
                            + '<td>' + record.ItemName                           
                            + '</td>'
                            + '<td>' + record.ProductionGroup
                            + '</td>'
                            + '<td>' + record.Unit
                            + '</td>'
                            + '<td>' + record.Stock
                            + '</td>'
                            + '</tr>';
                        $content = $(content);
                        app.format($content);
                        $('#medicine-stock-list tbody').append($content);
                    });
                }
            }
        });
    },
    Get_prescription: function () {
        var self = PatientDiagnosis;
        var CategoryID = $('#CategoryID').val();
        $.ajax({
            url: '/Masters/PrescriptionTemplate/GetPrescription',
            dataType: "json",
            type: "POST",
            data: { CategoryID: CategoryID },
            success: function (response) {
                $("#Prescription").html("");
                var html;
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.MedicineCategory + "</option>";
                });
                $("#Prescription").append(html);
            }
        });
    },

    get_units: function () {
        var self = PatientDiagnosis;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        //html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        $("#UnitID").append(html);
    },

    set_medicine: function (event, item) {
        var self = PatientDiagnosis; 
        $("#Medicine").val(item.Name);
        $("#MedicineID").val(item.id);
        $("#MedicineUnitID").val(item.unitId);
        $("#MedicineUnit").val(item.unit);
    },    
    set_treatment: function (event, item) {
        var self = PatientDiagnosis;
        $("#TreatmentName").val(item.value);
        $("#TreatmentID ").val(item.id);
        $("#StandardMedicineQty").val(item.qty);
    },

    select_medicine: function () {
        var self = PatientDiagnosis;
        var radio = $('#select-doctor tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#Medicine").val(Name);
        $("#MedicineID").val(ID);
        UIkit.modal($('#select-medicine')).hide();
    },

    selected_report_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload PDF/Image File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
                        $("#ReportID").val(record.ID)
                        $("#URL").val(record.URL)
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    calculate_treatment_days: function () {
        var self = PatientDiagnosis;
        $("#id-morning").iCheck('uncheck')
        $("#id-noon").iCheck('uncheck')
        $("#id-evening").iCheck('uncheck')
        $("#id-night").iCheck('uncheck')
        var date = $("#TreatmentStartDate").val();
        days = parseInt($("#NoofTreatment").val()) - 1;

        var FromDate = app.get_date_time(date);
        var ToDate = new Date();

        ToDate.setTime(FromDate + days * 24 * 60 * 60 * 1000);
        var day = ((ToDate.getDate().toString().length) == 1) ? '0' + (ToDate.getDate()) : (ToDate.getDate());
        var month = (ToDate.getMonth() < 9 ? '0' : '') + (ToDate.getMonth() + 1);
        var year = ToDate.getFullYear();
        FromattedDate = day + '-' + month + '-' + year;
       
       
        $("#TentativeEndDate").val(FromattedDate);
    },

    calculate_days: function () {
        var self = PatientDiagnosis;
        var date = $("#StartDateMed").val();
        days = parseInt($("#NoofDays").val()) - 1;

        var FromDate = app.get_date_time(date);
        var ToDate = new Date();

        ToDate.setTime(FromDate + days * 24 * 60 * 60 * 1000);
        var day = ((ToDate.getDate().toString().length) == 1) ? '0' + (ToDate.getDate()) : (ToDate.getDate());
        var month = (ToDate.getMonth() < 9 ? '0' : '') + (ToDate.getMonth() + 1);
        var year = ToDate.getFullYear();
        FromattedDate = day + '-' + month + '-' + year;

        $("#EndDate").val(FromattedDate);
    },

    check_morning: function () {
        var self = PatientDiagnosis;
        if ($(".Morning ").prop("checked") == true) {
            $("#MorningID ").prop("disabled", false);
        }
        else {
            $("#MorningID").val('');
            $("#MorningID ").prop("disabled", true);
        }
    },

    check_noon: function () {
        var self = PatientDiagnosis;
        if ($(".Noon").prop("checked") == true) {
            $("#NoonID").prop("disabled", false);
        }
        else {
            $("#NoonID").val('');
            $("#NoonID").prop("disabled", true);
        }
    },

    check_evening: function () {
        var self = PatientDiagnosis;
        if ($(".Evening").prop("checked") == true) {
            $("#EveningID").prop("disabled", false);
        }
        else {
            $("#EveningID").val('');
            $("#EveningID").prop("disabled", true);
        }
    },

    check_night: function () {
        var self = PatientDiagnosis;
        if ($(".Night").prop("checked") == true) {
            $("#NightID").prop("disabled", false);
        }
        else {
            $("#NightID").val('');
            $("#NightID").prop("disabled", true);
        }
    },



    check_multipletimes: function () {
        var self = PatientDiagnosis;
        if ($(".MultipleTimes").prop("checked") == true) {
        }
        //else {
        //    $("#NightID").val('');
        //    $("#NightID").prop("disabled", true);
        //}
    },

    Items: [],

    MedicinesItems: [],

    Medicine: [],

    VitalChartItems: [],

    LabTestItems: [],

    XrayItems: [],

    count: function () {
        index = $("#medicine_list tbody tr").length;
        $("#item-medicine-count").val(index);
    },

    count_treatment_medicine: function () {
        index = $("#medicinelist tbody tr").length;
        $("#treatment-medicine-count").val(index);
    },

    show_add_medicine: function () {
        var self = PatientDiagnosis;
        self.clear_medicine_tab();
        var PatientMedicinesID = 0;
        $("#btnAddmedicines").removeClass('uk-hidden');
        $("#TimeDescription").removeAttr('disabled');
        $("#Medicines").removeAttr('disabled');
        $("#Qty").removeAttr('disabled');
        $("#ModeOfAdministrationID").removeAttr('disabled');
        $("#Prescription").removeAttr('disabled');
        $(".Noon").attr("disabled", false);
        $(".Morning").attr("disabled", false);
        $(".Evening").attr("disabled", false);
        $(".Night").attr("disabled", false);

        $('.emptystomach').iCheck('uncheck');
        $('.beforefod').iCheck('uncheck');
        $('.afterfood').iCheck('uncheck');
        $('.middleoffood').iCheck('uncheck');
        $('.withfood').iCheck('uncheck');
        $('#show-add-medicine').trigger('click');
    },

    show_add_report: function () {
        var self = PatientDiagnosis;
        self.clear_report();
        $('#show-add-report').trigger('click');
    },

    show_treatment_schedule: function () {
        var self = PatientDiagnosis;
        var FromDate = new Date().toJSON().slice(0, 10).split('-').reverse().join('-');
        var PatientID = $("#PatientID").val();
        window.open("/AHCMS/TreatmentSchedule/Create/?Form=PatientDiagnosis" + "&FromDate=" + FromDate + "&PatientID=" + PatientID);
    },
    show_add_treatment: function () {
        var self = PatientDiagnosis;
        self.clear_treatment();
        $("#TreatmentName").removeAttr('disabled');
        $("#TherapistID").removeAttr('disabled');
        $("#TreatmentRoomID").removeAttr('disabled');
        $(".edit_medicine").removeClass('uk-hidden');
        $('#show-add-treatment').trigger('click');
        $("#NoofTreatment").val(1);
        $('#NoofTreatment').trigger('keyup');
    },

    remove_report: function () {
        $(this).closest('span').remove();
    },

    delete_item: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#report-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#report-list tbody tr").length);

    },

    delete__treatment_medicine: function () {
        var self = PatientDiagnosis;
        var MedicineID = clean($(this).closest('tr').find(".MedicineID").val());
        var TreatmentID = clean($("#TreatmentName option:selected").val());
        var index = self.Items.findIndex(x => x.MedicineID === MedicineID && x.TreatmentID === TreatmentID);
        self.Items.splice(index, 1);
        $(this).closest('tr').remove();
        var sino = 0;
        $('#medicinelist tbody tr .sl-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#medicinelist tbody tr").length);
    },

    delete_medicine: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        var RowIndex = clean($("#RowMedicineIndex").val());
        var MedicineID = clean($(this).closest('tr').find(".MedicinesID").val());
        var index = self.Medicine.findIndex(x => x.MedicinesID === MedicineID && x.GroupID === RowIndex);
        self.Medicine.splice(index, 1);
        var sino = 0;
        $('#medicine_list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-medicine-count").val($("#medicine_list tbody tr").length);

    },

    delete_medicine_on_grid: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        var GroupID = clean($(this).closest('tr').find(".GroupID").val());
        var tableindex = self.MedicinesItems.findIndex(x => x.GroupID === GroupID);
        self.MedicinesItems.splice(tableindex, 1);
        self.Medicine = $.grep(self.Medicine, function (element, index) { return element.GroupID != GroupID; });
        var sino = 0;
        $('#add_medicine-list tbody tr .serial-no-medicines').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_medicine-list tbody tr").length);

    },
    remove_medicine:function(){
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        self.count();
    },
    delete_treatment_on_grid: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        var TreatmentID = clean($(this).closest('tr').find(".TreatmentID").val());
        //Delete from Items which have selected treatmentID
        self.Items = $.grep(self.Items, function (element, index) { return element.TreatmentID != TreatmentID; });

        var sino = 0;
        $('#treatment-list tbody tr .sl-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#treatment-list tbody tr").length);

    },

    add_report: function () {
        var self = PatientDiagnosis;
        self.error_count = 0;
        self.error_count = self.validate_report();
        if (self.error_count > 0) {
            return;
        }
        $(".url").val('');
        sino = $('#report-list tbody tr').length + 1;
        var ReportName = $("#ReportName").val();
        var Date = $("#ReportDate").val();
        var Report = $('#selected-quotation').text();
        var Description = $("#Description").val();
        var ReportID = $('#ReportID').val();
        var Index = $("#Index").val();
        var url = $('#URL').val();
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
    },

    add_treatment_medicine: function () {
        var self = PatientDiagnosis;
        self.error_count = 0;
        self.error_count = self.validate_treatment_medicine();
        if (self.error_count > 0) {
            return;
        }

        var Medicine = $("#Medicine").val();
        var MedicineID = $("#MedicineID").val();
        var Qty = $("#StandardMedicineQty").val();
        var UnitID = $("#MedicineUnitID").val();
        var Unit = $("#MedicineUnit").val();

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="Medicine">' + Medicine
            + '<input type="hidden" class="MedicineUnitID"value="' + UnitID + '"/>'
            + '<input type="hidden" class="MedicineID"value="' + MedicineID + '"/>'
            + '</td>'
            + '<td class="MedicineUnit">' + Unit + '</td>'
            + '<td class="Qty">' + Qty + '</td>'             
            + '<td>'
            + '<a class="remove-medicine">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#medicinelist tbody').append($content);
        self.count_treatment_medicine();
        self.clear_medicine();
    },

    add_treatments: function () {
        var self = PatientDiagnosis;

        self.error_count = 0;
        self.error_count = self.validate_treatment();
        if (self.error_count > 0) {
            return;
        }
        var Treatment = $("#TreatmentName").val();
        var TreatmentID = $("#TreatmentID").val();
        var Therapist = $("#TherapistID option:selected").text();
        var TherapistID = $("#TherapistID option:selected").val();
        var TreatmentRoom = $("#TreatmentRoomID option:selected").text();
        var TreatmentRoomID = $("#TreatmentRoomID option:selected").val();
        var Instructions = $("#Instructions").val();
        var StartDate = $("#TreatmentStartDate").val();
        var EndDate = $("#TentativeEndDate").val();
        var NoofTreatments = $("#NoofTreatment").val();

        var MorningTime = $("#MorningTimeID option:selected").text();
        var MorningTimeID = $("#MorningTimeID option:selected").val();
        var NoonTime = $("#NoonTimeID option:selected").text();
        var NoonTimeID = $("#NoonTimeID option:selected").val();
        var EveningTime = $("#EveningTimeID option:selected").text();
        var EveningTimeID = $("#EveningTimeID option:selected").val();
        var NightTime = $("#NightTimeID option:selected").text();
        var NightTimeID = $("#NightTimeID option:selected").val();
        var Frequency = ($('.Treatmentfrequency:checked').size());
        var NoofDays = $("#Noofdays").val();

        var IsMorning = "";
        if ($('#id-morning').prop("checked") == true) {
            IsMorning = true;
        }
        else {
            IsMorning = false;
        }

        var Isevening = "";
        if ($('#id-evening').prop("checked") == true) {
            Isevening = true;
        }
        else {
            Isevening = false;
        }

        var IsNoon = "";
        if ($('#id-noon').prop("checked") == true) {
            IsNoon = true;
        }
        else {
            IsNoon = false;
        }

        var IsNight = "";
        if ($('#id-night').prop("checked") == true) {
            IsNight = true;
        }
        else {
            IsNight = false;
        }


        sino = $('#treatment-list tbody tr').length + 1;
        var Index = $("#Index").val();
        var RowIndex = clean($("#RowIndex").val());

        if (RowIndex <= 0) {
            var Index = $('#treatment-list tbody tr').length;
            RowIndex = parseInt(Index) + 1;
        }
        else {
            //var Treatmentindex = self.Items.findIndex(x => x.TreatmentID == TreatmentID);
            //self.Items.splice(Treatmentindex, 50);
            self.Items = $.grep(self.Items, function (element, index) { return element.TreatmentID != TreatmentID; });
        }

        //if ($("#RowIndex").val() == 0) {
        $('#medicinelist tbody tr').each(function () {
            self.Items.push({
                Madicine: $(this).find(".Medicine").text(),
                MedicineID: clean($(this).find(".MedicineID").val()),
                StandardMedicineQty: clean($(this).find(".Qty").text()),
                TreatmentID: clean($("#TreatmentID").val()),
                MedicineUnitID: $(this).find(".MedicineUnitID").val(),
                MedicineUnit: $(this).find(".MedicineUnit").text()
            });
        });
        //}
        if ($("#RowIndex").val() > 0) {
            var rowindex = $('#treatment-list tbody').find('tr').eq(Index);
            $(rowindex).find(".Treatment").text(Treatment);
            $(rowindex).find(".TreatmentID").val(TreatmentID);
            $(rowindex).find(".Therapist").text(Therapist);
            $(rowindex).find(".TherapistID").val(TherapistID);
            $(rowindex).find(".TreatmentRoom").text(TreatmentRoom);
            $(rowindex).find(".TreatmentRoomID").val(TreatmentRoomID);
            $(rowindex).find(".Instructions").text(Instructions);
            $(rowindex).find(".StartDate").text(StartDate);
            $(rowindex).find(".EndDate").text(EndDate);
            $(rowindex).find(".NoofTreatments").text(NoofTreatments);            

            $(rowindex).find(".MorningTime").val(MorningTime);
            $(rowindex).find(".NoonTime").val(NoonTime);
            $(rowindex).find(".EveningTime").val(EveningTime);
            $(rowindex).find(".NightTime").val(NightTime);


            $(rowindex).find(".IsMorning").val(IsMorning);
            $(rowindex).find(".IsNoon").val(IsNoon);
            $(rowindex).find(".IsEvening").val(Isevening);
            $(rowindex).find(".IsNight").val(IsNight);

            $(rowindex).find(".MorningTimeID").val(MorningTimeID);
            $(rowindex).find(".NoonTimeID").val(NoonTimeID);
            $(rowindex).find(".EveningTimeID").val(EveningTimeID);
            $(rowindex).find(".NightTimeID").val(NightTimeID);


            $('#add-treatment').trigger('click');
        }

        else {
            var content = "";
            var $content;
            content = '<tr>'
                + '<td class="sl-no uk-text-center">' + sino + '</td>'
                + '<td class="Treatment">' + Treatment + '</td>'
                + '<td class="Therapist">' + Therapist + '</td>'
                + '<td class="TreatmentRoom">' + TreatmentRoom + '</td>'
                + '<td class="Instructions">' + Instructions
                + '</td>'
                + '<td class="StartDate">' + StartDate + '</td>'
                + '<td class="uk-hidden">'              
                + '<input type="hidden" class="TreatmentID"value="' + TreatmentID + '"/>'
                + '<input type="hidden" class="TherapistID"value="' + TherapistID + '"/>'
                + '<input type="hidden" class="TreatmentRoomID"value="' + TreatmentRoomID + '"/>'

                + '<input type="hidden" class="MorningTime"value="' + MorningTime + '"/>'
                + '<input type="hidden" class="MorningTimeID"value="' + MorningTimeID + '"/>'
                + '<input type="hidden" class="NoonTime"value="' + NoonTime + '"/>'
                + '<input type="hidden" class="NoonTimeID"value="' + NoonTimeID + '"/>'
                + '<input type="hidden" class="EveningTime"value="' + EveningTime + '"/>'
                + '<input type="hidden" class="EveningTimeID"value="' + EveningTimeID + '"/>'
                + '<input type="hidden" class="NightTime"value="' + NightTime + '"/>'
                + '<input type="hidden" class="NightTimeID"value="' + NightTimeID + '"/>'

                + '<input type="hidden" class="IsMorning"value="' + IsMorning + '"/>'
                + '<input type="hidden" class="IsNoon"value="' + IsNoon + '"/>'
                + '<input type="hidden" class="IsEvening"value="' + Isevening + '"/>'
                + '<input type="hidden" class="IsNight"value="' + IsNight + '"/>'
                + '<input type="hidden" class="Frequency"value="' + Frequency + '"/>'
                + '<input type="hidden" class="NoofDays"value="' + NoofDays + '"/>'

                + '</td>'
                + '<td class="EndDate">' + EndDate
                + '</td>'
                + '<td class="NoofTreatments">' + NoofTreatments
                + '</td>'
                + '<td>' + ''
                + '</td>'
                + '<td>' + ''
                + '</td>'
                + '<td class="uk-text-center">' + '<button class="edit_treatment">' + '<i class="material-icons">' + "create" + '</i>' + '</button>' + '</td>'
                //+ '<td class="uk-text-center">' + '<button class="view_treatment">' + '<i class="material-icons ">' + "remove_red_eye" + '</i>' + '</button>' + '</td>'
                + '<td>'
                + '</td>'
                + '<td>'
                + '<a class="remove-treatment">'
                + '<i class="uk-icon-remove"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';
            $content = $(content);
            app.format($content);
            $('#treatment-list tbody').append($content);
            $('#add-treatment').trigger('click');
            self.clear_treatment();
        }
    },

    add_medicine: function () {
        var self = PatientDiagnosis;
        self.error_count = 0;
        self.error_count = self.validate_sub_medicine();
        if (self.error_count > 0) {
            return;
        }

        var Medicine = $("#Medicines").val();
        var MedicinesID = $("#MedicinesID").val();
        var UnitID = $("#UnitID option:selected").val();
        var Unit = $("#UnitID option:selected").text();
        var Quantity = $("#Qty").val();

        var Prescription = $("#Prescription option:selected").text();
        var Description = $("#TimeDescription").val();

        Prescription.trim() != "" ? Description.trim() != "" ? $("#TimeDescription").val(Description.trim() + " + " + Prescription.trim()) : $("#TimeDescription").val(Prescription.trim()) : null

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="Medicines">' + Medicine
            + '<input type="hidden" class="MedicinesID"value="' + MedicinesID + '"/>'
            + '<input type="hidden" class="UnitID"value="' + UnitID + '"/>'
            + '</td>'
            + '<td class="Unit">' + Unit
            + '</td>'
            + '<td class="Quantity">' + Quantity
            + '</td>'
            + '<td>'
            + '<a class="remove-medicines-grid">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#medicine_list tbody').append($content);
        self.count();
        self.clear_medicine_onadd();
        $("#Medicines").focus();
    },

    add_medicines_on_grid: function () {
        var self = PatientDiagnosis;
        self.error_count = 0;
        //$("#item-medicine-count").val($("#medicine_list tbody tr"));
        self.error_count = self.validate_medicine();
        if (self.error_count > 0) {
            return;
        }
        MorningTime = $("#MorningID option:selected").text();
        MorningTimeID = $("#MorningID option:selected").val();
        NoonTime = $("#NoonID option:selected").text();
        NoonTimeID = $("#NoonID option:selected").val();
        EveningTime = $("#EveningID option:selected").text();
        EveningTimeID = $("#EveningID option:selected").val();
        NightTime = $("#NightID option:selected").text();
        NightTimeID = $("#NightID option:selected").val();
        StartDate = $("#StartDateMed").val();
        EndDate = $("#EndDate").val();
        NoofDays = $("#NoofDays").val();
        Instructions = $("#Prescription option:selected").text();
        InstructionsID = $("#Prescription option:selected").val();
        Description = $("#TimeDescription").val();
        sino = $('#add_medicine-list tbody tr').length + 1;
        modeofadministrationID = $("#ModeOfAdministrationID").val();
        modeofadministration = $("#ModeOfAdministrationID").text();
        PatientMedicineID = clean($("#PatientMedicinesID").val());
        StopIndex = clean($("#StopIndex").val());
        var IsMorning = "";
        if ($('.Morning').prop("checked")) {
            IsMorning = true;
        }
        else {
            IsMorning = false;
        }

        var Isevening = "";
        if ($('.Evening').prop("checked")) {
            Isevening = true;
        }
        else {
            Isevening = false;
        }

        var IsNoon = "";
        if ($('.Noon').prop("checked")) {
            IsNoon = true;
        }
        else {
            IsNoon = false;
        }

        var IsNight = "";
        if ($('.Night').prop("checked")) {
            IsNight = true;
        }
        else {
            IsNight = false;
        }

        var IsMultipleTimes = "";
        if ($('.MultipleTimes').prop("checked")) {
            IsMultipleTimes = true;
        }
        else {
            IsMultipleTimes = false;
        }

        var IsEmptyStomach = "";
        if ($('.emptystomach').prop("checked")) {
            IsEmptyStomach = true;
        }
        else {
            IsEmptyStomach = false;
        }
        var IsBeforeFood = "";
        if ($('.beforefod').prop("checked")) {
            IsBeforeFood = true;
        }
        else {
            IsBeforeFood = false;
        }
        var IsAfterFood = "";
        if ($('.afterfood').prop("checked")) {
            IsAfterFood = true;
        }
        else {
            IsAfterFood = false;
        }

        var IsMiddleOfFood = "";
        if ($('.middleoffood').prop("checked")) {
            IsMiddleOfFood = true;
        }
        else {
            IsMiddleOfFood = false;
        }

        var IsWithFood = "";
        if ($('.withfood').prop("checked")) {
            IsWithFood = true;
        }
        else {
            IsWithFood = false;
        }
        //Check Edit Or Add
        var IsEdit = 1;
        var Frequency = ($('.frequency:checked').size());

        //Check edit or add
        var RowIndex = $('#RowMedicineIndex').val();
        if (RowIndex <= 0) {
            var Index = $('#add_medicine-list tbody tr').length;
            RowIndex = parseInt(Index) + 1;
            IsEdit = 0;
        }
        if (IsEdit == 0 || (IsEdit == 1 && PatientMedicineID > 0)) {
            //push into medicine items 
           
            var str = "";
            var qty = "";
            $('#medicine_list tbody tr').each(function () {
                item = {};
                item.Medicines = $(this).find(".Medicines").text(),
                item.MedicinesID = clean($(this).find(".MedicinesID").val()),
                item.Unit = $(this).find(".Unit").text(),
                item.UnitID = $(this).find(".UnitID").val(),
                item.Quantity = clean($(this).find(".Quantity").text()),

               // item.Quantity = $(this).find(".Qty").text(),

                item.GroupID = RowIndex,
                //item.PatientMedicinesID = PatientMedicineID,
                str = str + " + " + item.Medicines;
                qty = qty + " + " + item.Quantity;
                self.Medicine.push(item);

            });

            quantity = qty.replace(/,(\s+)?$/, '');
            var quantity1 = quantity.slice(2)

            string = str.replace(/,(\s+)?$/, '');
            var string1 = string.slice(2)


            self.MedicinesItems.push({
                MorningTime: MorningTime,
                NoonTime: NoonTime,
                EveningTime: EveningTime,
                NightTime: NightTime,
                StartDate: StartDate,
                EndDate: EndDate,
                NoofDays: NoofDays,
                Instructions: Instructions,
                InstructionsID: InstructionsID,
                Description: Description,
                IsMorning: IsMorning,
                Isevening: Isevening,
                IsNoon: IsNoon,
                IsNight: IsNight,
                IsMultipleTimes: IsMultipleTimes,
                IsEmptyStomach: IsEmptyStomach,
                IsAfterFood: IsAfterFood,
                IsBeforeFood: IsBeforeFood,
                IsMiddleOfFood: IsMiddleOfFood,
                IsWithFood: IsWithFood,
                MorningTimeID: MorningTimeID,
                NoonTimeID: NoonTimeID,
                EveningTimeID: EveningTimeID,
                NightTimeID: NightTimeID,
                GroupID: RowIndex,
                Frequency: Frequency,
                ModeOfAdministrationID: modeofadministrationID,
                MedicineInstruction: string1,
                QuantityInstruction: quantity1,
                //PatientMedicineID: PatientMedicineID
            });

            //////edit in the grid
            //var Index = RowIndex - 1;
            //if ($("#RowMedicineIndex").val() > 0) {
            //    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Medicine").text(string1);
            //    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Instructions").text(Description);
            //    $('#add-treatment').trigger('click');
            //}
            ////    //Add To the grid
            //else {
            var content = "";
            var $content;
            content = '<tr>'
            + '<td class="serial-no-medicines uk-text-center">' + sino + '</td>'
            + '<td class="Medicine">' + string1 + '</td>'
            + '<td class="Quantity">' + quantity1 + '</td>'
            + '<td class="Instructions">' + Description + '</td>'
            + '<td class="uk-hidden">'
            + '<input type="hidden" class="GroupID"value="' + RowIndex + '"/>'
            + '</td>'
            + '<td class="uk-text-center">' + '<button class="edit_medicines">' + '<i class="material-icons">' + "create" + '</i>' + '</button>' + '</td>'
            //+ '<td class="uk-text-center">' + '<i class="material-icons view_report">' + "remove_red_eye" + '</i>' + '</td>'
            + '<td class="Status">' + '</td>'
            + '<td>' + '' + '</td>'
            + '<td>'
            + '<a class="remove-medicine_ongrid">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
            $content = $(content);
            app.format($content);
            $('#add_medicine-list tbody').append($content);

            if (PatientMedicineID > 0) {
                self.stop_medicine(PatientMedicineID, StopIndex);

            }
        }

        else {
            if (IsEdit == 1 && PatientMedicineID <= 0) {
                //delete from MedicinesItems list for edit with groupid
                var Medicineindex = self.MedicinesItems.findIndex(x => x.GroupID == RowIndex);
                self.MedicinesItems.splice(Medicineindex, 1);
                self.Medicine = $.grep(self.Medicine, function (element, index) { return element.GroupID != RowIndex; });

                //push into medicine items 

                var str = "";
                var qty = "";
                $('#medicine_list tbody tr').each(function () {
                    item = {};
                    item.Medicines = $(this).find(".Medicines").text(),
                    item.MedicinesID = clean($(this).find(".MedicinesID").val()),
                    item.Unit = $(this).find(".Unit").text(),
                    item.UnitID = $(this).find(".UnitID").val(),
                    item.Quantity = clean($(this).find(".Quantity").text()),

                   // item.Quantity = $(this).find(".Qty").text(),

                    item.GroupID = RowIndex,
                    //item.PatientMedicinesID = PatientMedicineID,
                    str = str + " + " + item.Medicines;
                    qty = qty + " + " + item.Quantity;
                    self.Medicine.push(item);

                });
                string = str.replace(/,(\s+)?$/, '');
                var string1 = string.slice(2)
                quantity = qty.replace(/,(\s+)?$/, '');
                var quantity1 = quantity.slice(2)

                self.MedicinesItems.push({
                    MorningTime: MorningTime,
                    NoonTime: NoonTime,
                    EveningTime: EveningTime,
                    NightTime: NightTime,
                    StartDate: StartDate,
                    EndDate: EndDate,
                    NoofDays: NoofDays,
                    Instructions: Instructions,
                    InstructionsID: InstructionsID,
                    Description: Description,
                    IsMorning: IsMorning,
                    Isevening: Isevening,
                    IsNoon: IsNoon,
                    IsNight: IsNight,
                    IsMultipleTimes: IsMultipleTimes,
                    IsEmptyStomach: IsEmptyStomach,
                    IsAfterFood: IsAfterFood,
                    IsBeforeFood: IsBeforeFood,
                    IsMiddleOfFood: IsMiddleOfFood,
                    IsWithFood: IsWithFood,
                    MorningTimeID: MorningTimeID,
                    NoonTimeID: NoonTimeID,
                    EveningTimeID: EveningTimeID,
                    NightTimeID: NightTimeID,
                    GroupID: RowIndex,
                    Frequency: Frequency,
                    ModeOfAdministrationID: modeofadministrationID,
                    MedicineInstruction: string1,
                    QuantityInstruction: quantity1,
                    //PatientMedicineID: PatientMedicineID
                });


                ////edit in the grid
                var Index = RowIndex - 1;
                if ($("#RowMedicineIndex").val() > 0) {
                    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Medicine").text(string1);
                    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Instructions").text(Description);
                    $('#add-treatment').trigger('click');
                }
            }
        }

        var idClicked = this.id;
        if (idClicked == 'btnSaveMadicine') {
            $('#add-medicine').trigger('click');
        }
        self.clear_add_medicine();
        $("#Medicines").focus();

    },

    edit_report: function () {
        var self = PatientDiagnosis;
        self.clear_report();
        var row = $(this).closest("tr");
        var Index = $(row).index();
        var ReportName = $(row).find(".ReportName").text();
        var Description = $(row).find(".Description").text();
        var Report = $(row).find(".Report").val();
        var ReportID = $(row).find(".ReportID").val();
        var Date = $(row).find(".Date").text();



        var rowindex = $('#report-list tbody').find('tr').eq(Index);

        $("#RowIndex").val(Index + 1);
        $("#ReportName").val(ReportName);
        $("#ReportDate").val(Date);
        $("#selected-quotation").text(Report);
        $("#Description").val(Description);
        $("#Index").val(Index);
        $("#ReportID").val(ReportID);
        $('#show-add-report').trigger('click');
    },

    edit_treatment: function () {
        var self = PatientDiagnosis;
        self.clear_treatment();
        var row = $(this).closest("tr");
        var Index = $(row).index();
        var TreatmentID = clean($(row).find(".TreatmentID").val());
        var Treatment = $(row).find(".Treatment").text().trim();
        var Therapist = clean($(row).find(".TherapistID").val());
        var TreatmentRoom = clean($(row).find(".TreatmentRoomID").val());
        var Instructions = $(row).find(".Instructions").text();
        var StartDate = $(row).find(".StartDate").text();
        var EndDate = $(row).find(".EndDate").text();
        var NofoTreatments = $(row).find(".NoofTreatments").text();

        var IsMorning = $(row).find(".IsMorning").val().trim();
        var MorningTimeID = $(row).find(".MorningTimeID").val();
        var MorningTime = $(row).find(".MorningTime").val().trim();

        var IsNoon = $(row).find(".IsNoon").val().trim();
        var NoonTime = $(row).find(".NoonTime").val().trim();
        var NoonTimeID = $(row).find(".NoonTimeID").val();

        var Isevening = $(row).find(".IsEvening").val().trim();
        var EveningTime = $(row).find(".EveningTime").val().trim();
        var EveningTimeID = $(row).find(".EveningTimeID").val();

        var IsNight = $(row).find(".IsNight").val().trim();
        var NightTime = $(row).find(".NightTime").val().trim();
        var NightTimeID = $(row).find(".NightTimeID").val();

        $("#TreatmentName").attr('disabled', 'disabled');
        $("#TherapistID").attr('disabled', 'disabled');
        $("#TreatmentRoomID").attr('disabled', 'disabled');
        //$(".edit_medicine").addClass('uk-hidden');
     

        $("#RowIndex").val(Index + 1);
        $("#Index").val(Index);
        $("#TreatmentName").val(Treatment);
        $("#TreatmentID").val(TreatmentID);
        $("#TherapistID").val(Therapist);
        $("#TreatmentRoomID").val(TreatmentRoom);
        $("#Instructions").val(Instructions);
        $("#TentativeStartDate").val(StartDate);
        $("#TentativeEndDate").val(EndDate);
        $("#NoofTreatment").val(NofoTreatments);
       

        if (IsMorning == "true") {
            //$('.MorningTime ').prop("checked")
            $('.MorningTime ').iCheck("check")
            $("#MorningTimeID").val(MorningTimeID)
        }
        if (IsNoon == "true") {
            $('.NoonTime').iCheck("check")
            $("#NoonTimeID").val(NoonTimeID)
        }
        if (Isevening == "true") {
            $('.EveningTime').iCheck("check")
            $("#EveningTimeID").val(EveningTimeID)
        }
        if (IsNight == "true") {
            $('.NightTime').iCheck("check")
            $("#NightTimeID").val(NightTimeID)
        }


        $(self.Items).each(function (i, record) {
            if (record.TreatmentID == TreatmentID) {
                var content = "";
                var $content;
                content = '<tr>'
                    + '<td class="Medicine">' + record.Madicine
                    + '</td>'
                    + '<td class="uk-hidden">'
                    + '<input type="hidden" class="MedicineUnitID"value="' + record.MedicineUnitID + '"/>'
                    + '<input type="hidden" class="MedicineID"value="' + record.MedicineID + '"/>'
                    + '</td>'
                     + '<td class="MedicineUnit">' + record.MedicineUnit + '</td>'
                     + '<td class="Qty">' + record.StandardMedicineQty + '</td>'
                    + '<td>' + ''
                    + '<a class="remove-medicine">'
                    + '<i class="uk-icon-remove"></i>'
                    + '</a>'
                    + '</td>'
                    + '</tr>';
                $content = $(content);
                app.format($content);
                $('#medicinelist tbody').append($content);
            }
        });
        self.count_treatment_medicine();
        $('#show-add-treatment').trigger('click');
    },

    edit_medicines: function () {
        var self = PatientDiagnosis;
        self.clear_medicine_tab();
        var row = $(this).closest('tr');
        var index = row.index();


        //var GroupID = index + 1

        var GroupID = $(row).find(".GroupID").val();
        var PatientMedicinesID = $(row).find(".PatientMedicinesID").val();

        //var Index = $(row).index();
        $("#RowMedicineIndex").val(GroupID);
        $("#PatientMedicinesID").val(PatientMedicinesID);
        $("#StopIndex").val(index);
        //if (PatientMedicinesID > 0) {
        //    $("#btnAddmedicines").addClass('uk-hidden');
        //    $("#TimeDescription").attr('disabled', 'disabled');
        //    $("#Medicines").attr('disabled', 'disabled');
        //    $("#Qty").attr('disabled', 'disabled');
        //    $("#ModeOfAdministrationID").attr('disabled', 'disabled');
        //    $("#Prescription").attr('disabled', 'disabled');
        //    $(".Noon").attr("disabled", true);
        //    $(".Morning").attr("disabled", true);
        //    $(".Evening").attr("disabled", true);
        //    $(".Night").attr("disabled", true);
        //    $(".MultipleTimes").attr("disabled", true);


        //    if ($(".Morning").prop('checked') == true) {
        //        $("#MorningID").prop("disabled", true);
        //    }
        //}
        //$("#Index").val(Index);
        $('#medicine_list tbody').empty();
        $(self.Medicine).each(function (i, record) {
            var content = "";
            var $content;
            if (PatientMedicinesID > 0) {
                if (record.PatientMedicinesID == PatientMedicinesID) {
                    content = '<tr>'
                     + '<td class="Medicines">' + record.Medicines
                     + '<input type="hidden" class="MedicinesID"value="' + record.MedicinesID + '"/>'
                     + '<input type="hidden" class="UnitID"value="' + record.UnitID + '"/>'
                     + '</td>'
                     + '<td class="Unit">' + record.Unit
                     + '</td>'
                     + '<td class="Quantity">' + record.Quantity
                     + '</td>'
                     + '<td>'
                      + '<a class="remove-medicines-grid">'
                     + '<i class="uk-icon-remove"></i>'
                     + '</a>'
                     +'</td>'
                     + '</tr>';
                    $content = $(content);
                    app.format($content);
                    $('#medicine_list tbody').append($content);
                    self.count();
                }
            }
            else {
                if (record.GroupID == GroupID) {
                    content = '<tr>'
                     + '<td class="Medicines">' + record.Medicines
                     + '<input type="hidden" class="MedicinesID"value="' + record.MedicinesID + '"/>'
                     + '<input type="hidden" class="UnitID"value="' + record.UnitID + '"/>'
                     + '</td>'
                     + '<td class="Unit">' + record.Unit
                     + '</td>'
                     + '<td class="Quantity">' + record.Quantity
                     + '</td>'
                     + '<td>'
                     + '<a class="remove-medicines-grid">'
                     + '<i class="uk-icon-remove"></i>'
                     + '</a>'
                     + '</td>'
                     + '</tr>';
                    $content = $(content);
                    app.format($content);
                    $('#medicine_list tbody').append($content);
                   self.count();
                }
            }
        });
        if (PatientMedicinesID > 0) {
            $(self.MedicinesItems).each(function (i, record) {
                if (record.PatientMedicineID == PatientMedicinesID) {
                    $("#TimeDescription").val(record.Description);
                    $("#Prescription").val(record.InstructionsID);
                    $("#StartDateMed").val(record.StartDate);
                    $("#EndDate").val(record.EndDate);
                    $("#NoofDays").val(record.NoofDays);
                    $("#MorningID").val(record.MorningTimeID);
                    $("#NoonID").val(record.NoonTimeID);
                    $("#EveningID").val(record.EveningTimeID);
                    $("#NightID").val(record.NightTimeID);
                    $("#ModeOfAdministrationID").val(record.ModeOfAdministrationID);
                    if (record.IsAfterFood == true) {
                        $('.afterfood').prop("checked")
                        $('.afterfood').iCheck("check")
                    }
                    if (record.IsMorning == true) {
                        $('.Morning').iCheck("check")
                    }
                    if (record.Isevening == true) {
                        $('.Evening').iCheck("check")
                    }
                    if (record.IsNoon == true) {
                        $('.Noon').iCheck("check")
                    }
                    if (record.IsNight == true) {
                        $('.Night').iCheck("check")
                    }
                    if (record.IsMultipleTimes == true) {
                        $('.MultipleTimes').iCheck("check")
                    }
                    if (record.IsEmptyStomach == true) {
                        $('.emptystomach').prop("checked")
                        $('.emptystomach').iCheck("check")
                    }
                    if (record.IsBeforeFood == true) {
                        $('.beforefod').prop("checked")
                        $('.beforefod').iCheck("check")
                    }
                    if (record.IsMiddleOfFood == true) {
                        $('.middleoffood').prop("checked")
                        $('.middleoffood').iCheck("check")
                    }
                    if (record.IsWithFood == true) {
                        $('.withfood').prop("checked")
                        $('.withfood').iCheck("check")
                    }
                }
            });
        }
        else {
            $(self.MedicinesItems).each(function (i, record) {
                if (record.GroupID == GroupID) {
                    $("#TimeDescription").val(record.Description);
                    $("#Prescription").val(record.InstructionsID);
                    $("#StartDateMed").val(record.StartDate);
                    $("#EndDate").val(record.EndDate);
                    $("#NoofDays").val(record.NoofDays);
                    $("#MorningID").val(record.MorningTimeID);
                    $("#NoonID").val(record.NoonTimeID);
                    $("#EveningID").val(record.EveningTimeID);
                    $("#NightID").val(record.NightTimeID);
                    $("#ModeOfAdministrationID").val(record.ModeOfAdministrationID);
                    if (record.IsAfterFood == true) {
                        $('.afterfood').prop("checked")
                        $('.afterfood').iCheck("check")
                    }
                    if (record.IsMorning == true) {
                        $('.Morning').iCheck("check")
                    }
                    if (record.Isevening == true) {
                        $('.Evening').iCheck("check")
                    }
                    if (record.IsNoon == true) {
                        $('.Noon').iCheck("check")
                    }
                    if (record.IsNight == true) {
                        $('.Night').iCheck("check")
                    }
                    if (record.IsMultipleTimes == true) {
                        $('.MultipleTimes').iCheck("check")
                    }
                    if (record.IsEmptyStomach == true) {
                        $('.emptystomach').prop("checked")
                        $('.emptystomach').iCheck("check")
                    }
                    if (record.IsBeforeFood == true) {
                        $('.beforefod').prop("checked")
                        $('.beforefod').iCheck("check")
                    }
                    if (record.IsMiddleOfFood == true) {
                        $('.middleoffood').prop("checked")
                        $('.middleoffood').iCheck("check")
                    }
                    if (record.IsWithFood == true) {
                        $('.withfood').prop("checked")
                        $('.withfood').iCheck("check")
                    }
                }
            });
        }

        $('#show-add-medicine').trigger('click');

    },

    stop_medicine: function (PatientMedicinesID, StopIndex) {
        var self = PatientDiagnosis;
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/StopMedicine",
            data: {
                PatientMedicinesID: PatientMedicinesID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $('#add_medicine-list tbody tr:eq(' + StopIndex + ')').find(".edit_medicines").addClass('uk-hidden');
                    $('#add_medicine-list tbody tr:eq(' + StopIndex + ')').find(".Status").text("Stopped");
                    for (var i = 0; i < self.MedicinesItems.length; i++) {
                        var MedicineID;
                        MedicineID = self.MedicinesItems[i].GroupID
                        if(MedicineID==PatientMedicinesID)
                        {
                            self.MedicinesItems.splice(i, 1);
                        }
                    }
                }
                for (var i = 0; i < self.Medicine.length; i++) {
                    var MedicineID;
                    MedicineID = self.Medicine[i].GroupID
                    if (MedicineID == PatientMedicinesID) {
                        self.Medicine.splice(i, 1);
                    }
                }
            }
        });
    },

    clear_medicine_tab: function () {
        var self = PatientDiagnosis;
        $('#medicine_list tbody').text('');
        $("#TimeDescription").val('');
        $("#MorningID").val('');
        $("#NoonID").val('');
        $("#EveningID").val('');
        $("#NightID").val('');
        $("#StartDate").val('');
        $("#EndDate").val('');
        $("#NoofDays").val('');
        $("#Prescription").val('');
        $(".Morning").iCheck('uncheck');
        $(".Evening").iCheck('uncheck');
        $(".Noon").iCheck('uncheck');
        $(".Night").iCheck('uncheck');
        $("#MedicineName").val('');
        $("#Qty").val('');
        $("#UnitID option:selected").text('Select');
        $("#MedicinesID").val('');
        $("#ModeOfAdministrationID").val('');
        $("#RowMedicineIndex").val('');
        $("#PatientMedicinesID").val('');
    },

    clear_add_medicine: function () {
        var self = PatientDiagnosis;
        $("#Prescription").val('');
        $('#medicine_list tbody').text('');
        $("#TimeDescription").val('');
        $(".Morning").iCheck('uncheck');
        $(".Noon").iCheck('uncheck');
        $(".Evening").iCheck('uncheck');
        $(".Night").iCheck('uncheck');
        $(".MultipleTimes").iCheck('uncheck');
        $("#MorningID").val('');
        $("#NoonID").val('');
        $("#EveningID").val('');
        $("#NightID ").val('');
        $("#StartDate").val('');
        $("#EndDate").val('');
        $("#NoofDays").val('');
        $("#Medicines").val('');
        $("#Qty").val('');
        $("#UnitID option:selected").text('Select');
        $("#MedicinesID").val('');
        $('#RowMedicineIndex').val('');
        $('#ModeOfAdministrationID').val('');

        $(".emptystomach").iCheck('uncheck');
        $(".beforefod").iCheck('uncheck');
        $(".afterfood").iCheck('uncheck');
        $(".middleoffood").iCheck('uncheck');
        $(".withfood").iCheck('uncheck');
        $("#item-medicine-count").val('');
    },

    clear_medicine_onadd: function () {
        var self = PatientDiagnosis;
        $("#Medicines").val('');
        $("#Qty").val('');
        $("#UnitID option:selected").text('Select');
        $("#MedicinesID").val('');
        $("#Prescription").val('');
    },

    clear_medicine: function () {
        var self = PatientDiagnosis;
        $("#Medicine").val('');
        $("#MedicineID").val('');
        $("#StandardMedicineQty").val('');
        $("#MedicineUnitID").val('');
        $("#MedicineUnit").val('');
    },

    clear_treatment: function () {
        var self = PatientDiagnosis;
        $("#TreatmentName").val('');
        $("#TherapistID").val('');
        $("#TreatmentRoomID").val('');
        $("#Instructions").val('');
        $("#TentativeStartDate").val('');
        $("#TentativeEndDate").val('');
        $("#NoofTreatment").val('');
        $('#medicinelist tbody').text('');
        $("#RowIndex").val('');
        $("#Index").val('');
        $("#StandardMedicineQty").val('');
        $(".MorningTime").iCheck('uncheck');
        $(".NoonTime").iCheck('uncheck');
        $(".EveningTime").iCheck('uncheck');
        $(".NightTime").iCheck('uncheck');
    },

    clear_report: function () {
        var self = PatientDiagnosis;
        $("#ReportName").val('');
        $("#selected-quotation").text('');
        $("#Description").val('');
        $("#RowIndex").val('');
        $("#Index").val('');
    },

    get_patient_data: function () {
        var self = PatientDiagnosis;
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        var reviewID = $('#ReviewID').val();
        var nowDate = new Date();
        var nowDay = ((nowDate.getDate().toString().length) == 1) ? '0' + (nowDate.getDate()) : (nowDate.getDate());
        var nowMonth = nowDate.getMonth() < 9 ? '0' + (nowDate.getMonth() + 1) : (nowDate.getMonth() + 1);
        var nowYear = nowDate.getFullYear();
        var formattedDate = nowDay + "-" + nowMonth + "-" + nowYear;
        $('#Date option:selected').val(formattedDate);
        $('#Date').val(formattedDate);
        var date = $('#Date option:selected').val();
        $('#ReviewID').val(0);
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
        //app.confirm("Data you Entered Will be Cleared", function () {
            self.get_report();
            self.get_medicine();
            self.get_LabItems();
            self.get_chart(AppointmentProcessID);
            self.get_casesheet();
            self.get_vitals(AppointmentProcessID);
            self.get_vital_chart(AppointmentProcessID);
            self.get_medicines_details(AppointmentProcessID);
            self.get_medicines_Items(AppointmentProcessID);
            self.get_treatment_item();
            self.get_treatment();
        //});
        //self.get_history();
        // self.get_examination();
        $(".tabHistory").addClass("uk-hidden");
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabReview").removeClass("uk-hidden");
    },

    get_vital_chart: function (ProcessID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = ProcessID;
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/VitalChart",
            data: {
                PatientID: PatientID,
                Date: date,
                IsCompleted: IsCompleted,
                AppointmentProcessID: AppointmentProcessID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#vital-chart-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#vital-chart-list tbody").append($response);
            }
        });

    },

    get_chart: function (ProcessID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = ProcessID;
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Chart",
            data: {
                PatientID: PatientID,
                Date: date,
                IsCompleted: IsCompleted,
                AppointmentProcessID: AppointmentProcessID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#chart-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#chart-list tbody").append($response);

            }
        });
    },

    get_vitals: function (ProcessID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = ProcessID;
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Vitals",
            data: {
                PatientID: PatientID,
                Date: date,
                IsCompleted: IsCompleted,
                AppointmentProcessID: AppointmentProcessID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#vital-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#vital-list tbody").append($response);

            }
        });
    },

    get_treatment: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Treatment",
            data: {
                PatientID: PatientID,
                Date: Date,
                IsCompleted: IsCompleted,
                AppointmentProcessID: AppointmentProcessID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#treatment-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#treatment-list tbody").append($response);

            }
        });
    },

    get_history: function () {
        var self = PatientDiagnosis;
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        var IsCompleted = $("#IsCompleted").val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/History",
            dataType: "html",
            data: {
                OPID: OPID,
                PatientID: PatientID,
            },
            type: "POST",
            success: function (response) {

                if ($('.fresh').is(':checked')) {
                    $(".tabHistory").addClass("uk-hidden");
                }
                else {
                    $(".tabHistory").removeClass("uk-hidden");
                }
               
                $("#history-list").empty();
                $response = $(response);
                app.format($response);
                $("#history-list").append($response);
                if (IsCompleted == "true") {
                    $(".btnViewReview").addClass("uk-hidden")
                    $(".btnView").removeClass("uk-hidden")
                }

            },
        });
    },

    get_report: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/GetReport",
            data: {
                PatientID: PatientID,
                Date: Date,
                IsCompleted: IsCompleted,
                AppointmentProcessID: AppointmentProcessID
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
    },

    get_casesheet: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/CaseSheet",
            data: {
                PatientID: PatientID,
                Date: Date,
                AppointmentProcessID: AppointmentProcessID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                //if(response.Data.NextVisitDate)
                $("#NextVisitDate").val(response.Data.NextVisitDate);
                $("#Remark").val(response.Data.Remark);
            }
        });
    },

    get_examination: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Examination",
            data: {
                PatientID: PatientID,
                Date: date,
                AppointmentProcessID: AppointmentProcessID,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#examination-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#examination-list tbody").append($response);

            }
        });
    },

    report_view: function () {
        var self = PatientDiagnosis;
        var row = $(this).closest("tr");
        var url = $(row).find(".url").val();
        $("#print-preview-title").hide();
        $("#btnOkPrint").hide();
        app.print_preview(url);
    },

    clear_lab_items: function () {
        var self = PatientDiagnosis;
        $("#LabTest").val('');
        $("#LabTestID").val('');
    },

    get_treatment_item: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetTreatmentMedicineList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                Date: Date,
                AppointmentProcessID: AppointmentProcessID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        //self.Items = [];
                        self.Items.push({
                            TreatmentID: record.TreatmentID, MedicineID: record.MedicineID, Madicine: record.Medicine,
                            StandardMedicineQty: record.StandardMedicineQty, MedicineUnitID: record.MedicineUnitID,
                            MedicineUnit: record.MedicineUnit
                        });
                    });
                }
                self.count_treatment_medicine();
            },
        });
    },

    get_doctor_list: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetDoctorList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                Date: Date,
                AppointmentProcessID: AppointmentProcessID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {                 
                    $(response.Data).each(function (i, record) {
                        sino = $('#add_doctor tbody tr').length + 1;
                        var content = "";
                        var $content;
                        content = '<tr>'
                            + '<td class="sl-no uk-text-center">' + sino + '</td>'
                            + '<td class="DoctorName">' + record.DoctorName
                            + '<input type="hidden" class="DoctorNameID"value="' + record.DoctorNameID + '"/>'
                            + '</td>'
                            + '<td>'
                            + '</td>'
                            + '</tr>';
                        $content = $(content);
                        app.format($content);
                        $('#add_doctor tbody').append($content);
                    });
                }
                self.count_treatment_medicine();
            },
        });
    },

    get_medicine: function () {
        var self = PatientDiagnosis;
        //var OPID = $('#AppointmentProcessID').val();
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Medicines",
            dataType: "html",
            data: {
                OPID: OPID,
                PatientID: PatientID,
                Date: Date,
                IsCompleted: IsCompleted
            },
            type: "POST",
            success: function (response) {
                $("#add_medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_medicine-list tbody").append($response);
            },
        });
    },

    get_medicines_details: function (ID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();

        //var AppointmentProcessID = $('#AppointmentProcessID').val();
        var ReviewStatus = $('#ReviewStatus').val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetMedicinesList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                Date: Date,
                AppointmentProcessID: ID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.Medicine=[];
                    $(response.Data).each(function (i, record) {
                        self.Medicine.push({
                            MedicinesID: record.MedicineID, UnitID: record.UnitID, Unit: record.Unit, Medicines: record.Medicine,
                            GroupID: record.PatientMedicinesID, Quantity: record.Quantity, PatientMedicinesID: record.PatientMedicinesID
                        });
                    });
                }
            },
        });
        //self.count();
    },

    get_medicines_Items: function (ID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        //var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/GetMedicinesItemsList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                Date: Date,
                AppointmentProcessID: ID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.MedicinesItems =[];
                    $(response.Data).each(function (i, record) {
                        self.MedicinesItems.push({
                            NoofDays: record.NoofDays, IsMorning: record.IsMorning, IsNoon: record.IsNoon, Isevening: record.Isevening,
                            IsNight: record.IsNight, IsMultipleTimes: record.IsMultipleTimes, MorningTime: record.MorningTime, NoonTime: record.NoonTime, EveningTime: record.EveningTime,
                            NightTime: record.NightTime, IsEmptyStomach: record.IsEmptyStomach, IsBeforeFood: record.IsBeforeFood,
                            IsAfterFood: record.IsAfterFood, Description: record.Description, GroupID: record.PatientMedicineID, MorningTimeID: record.MorningTimeID,
                            EveningTimeID: record.EveningTimeID, NoonTimeID: record.NoonTimeID, NightTimeID: record.NightTimeID,
                            StartDate: record.StartDate, EndDate: record.EndDate, ModeOfAdministrationID: record.ModeOfAdministrationID, Frequency: record.Frequency,
                            PatientMedicineID: record.PatientMedicineID, IsWithFood: record.IsWithFood, IsMiddleOfFood: record.IsMiddleOfFood, 
                            MedicineInstruction: record.MedicineInstruction, QuantityInstruction: record.QuantityInstruction
                        });
                    });
                }
            },
        });
    },   
    validate_medicine: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_medicine_add.length > 0) {
            return form.validate(self.rules.on_medicine_add);
        }
        return 0;
    },
    validate_doctor: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_add_doctor.length > 0) {
            return form.validate(self.rules.on_add_doctor);
        }
        return 0;
    },

    validate_treatment: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_treatment_add.length > 0) {
            return form.validate(self.rules.on_treatment_add);
        }
        return 0;
    },

    validate_report: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_report_add.length > 0) {
            return form.validate(self.rules.on_report_add);
        }
        return 0;
    },

    validate_sub_medicine: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_sub_medicine_add.length > 0) {
            return form.validate(self.rules.on_sub_medicine_add);
        }
        return 0;
    },

    validate_treatment_medicine: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_treatment_medicine.length > 0) {
            return form.validate(self.rules.on_treatment_medicine);
        }
        return 0;
    },

    validate_save: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {

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

            {
                elements: "#TherapistID",
                rules: [
                    { type: form.required, message: "Please select Therapist" },
                ]
            },
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
                     { type: form.required, message: "Please enter No of Treatment" },
                     { type: form.non_zero, message: "Please enter valid Treatment No" },
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
                    { type: form.non_zero, message: "Please enter Quantity" },
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
        ],
        on_add_doctor: [
     {
         elements: "#DoctorName",
         rules: [
         { type: form.required, message: "Please Select Doctor" },
         ]
     },
        ],
    },

    list_viewforPrint: function () {
        var $list = $('#Treatment-Details-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/PatientDiagnosis/GetTreatmentDetailsListForPrint"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
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
                               + "<input type='hidden' class='ID' value='" + row.AppointmentProcessID + "'>"
                           + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>";

                       }
                   },
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "Date", "className": "Date" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "Time", "className": "Time" },
                   { "data": "TokenNo", "className": "TokenNo" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        var PatientID = $(this).closest("tr").find("td .PatientID").val();
                        app.load_content("/AHCMS/PatientDiagnosis/Details/?ID=" + Id + "&PatientID=" + PatientID);
                        //app.load_content("/AHCMS/PatientDiagnosis/Create/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    prescription_printpdf: function () {
        var self = PatientDiagnosis;

        $.ajax({
            url: '/Reports/AHCMS/MedicinePrescriptionPrintPdf',
            data: {
                id: $("#AppointmentProcessID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },

    treatment_printpdf: function () {
        var self = PatientDiagnosis;

        $.ajax({
            url: '/Reports/AHCMS/TreatmentDetailsPrintPdf',
            data: {
                id: $("#AppointmentProcessID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },

    add_lab_item: function () {
        var self = PatientDiagnosis;
        if (self.validate_lab_tests() > 0) {
            return;
        }
        sino = $('#add_lab_items tbody tr ').length + 1;
        var LabItems = $("#LabTest").val();
        var LabTestID = $("#LabTestID").val();
        var Date = $("#TestDate").val();
        var Category = $("#Category").val();

        var content = "";
        var $content;
        content = '<tr class = ' + Category + '>'
            + '<td class="labitem-serial-no uk-text-center slno">' + sino
            + '<input type="hidden" class="LabTestID"value="' + LabTestID + '"/>' + '</td>'
            + '<td class="Date">' + Date + '</td>'
            + '<td class="LabItems">' + LabItems + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td>'
            + '<a class="labitems-remove">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_lab_items tbody').append($content);
        self.clear_lab_items();

    },

    validate_lab_tests: function () {
        var self = PatientDiagnosis;
        if (self.rules.on_add_lab_tests.length > 0) {
            return form.validate(self.rules.on_add_lab_tests);
        }
        return 0;
    },

    get_LabItems: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        //var OPID = $('#AppointmentProcessID').val();
        var OPID = $('#AppointmentProcessID').val();
        var Date = $('#Date option:selected').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/LabTest",
            dataType: "html",
            data: {
                PatientID: PatientID,
                OPID: OPID,
                Date: Date
            },
            type: "POST",
            success: function (response) {
                $("#add_lab_items tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_lab_items tbody").append($response);
            },
        });
    },

    get_previous_examination: function (ReviewID) {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/PreviousExamination",
            data: {
                PatientID: PatientID,
                ReviewID: ReviewID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#examination-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#examination-list tbody").append($response);

            }
        });
    },
    MedicineReview: function (ReviewID) {
        var self = PatientDiagnosis;
        //var OPID = $('#AppointmentProcessID').val();
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/Medicines",
            dataType: "html",
            data: {
                OPID: ReviewID,
                PatientID: PatientID,
                Date: Date,
                IsCompleted: IsCompleted
            },
            type: "POST",
            success: function (response) {
                $("#add_medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_medicine-list tbody").append($response);
            },
        });
    },

    get_previous_patient_data: function () {
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
            self.get_LabItems();
            self.get_report();
            self.get_doctor_list();
        });
        $(".tabHistory").addClass("uk-hidden");
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabReview").removeClass("uk-hidden");
    },
    PrescribedTest: [],
    get_data_from_modal: function () {
        var self = PatientDiagnosis;
        var data = {};
        PrescribedTest = [];
        for (var i = 0; i < self.LabItemList2.length; i++) {
            var item = {};
            item.ID = self.LabItemList2[i].id;
            item.Name = self.LabItemList2[i].name;
            item.Date = $("#TestDate").val();
            self.PrescribedTest.push(item);
        }
        self.add_data_to_grid();
        $("#labtest-list tbody tr.included").each(function () {
            $(this).find('.ItemID').parent('div').removeClass("checked");
            $(this).removeClass('included');
        });
        self.PrescribedTest = [];
        self.LabItemList = [];
        self.LabItemList2 = []; 
    
        return data;
    },
    add_data_to_grid: function () {
        var self = PatientDiagnosis;
        $(self.PrescribedTest).each(function (i, record) {
            var serialno = $('#add_lab_items tr').length;
            var content = "";
            var $content;
            content = '<tr class="LabItems">'
                 + '<td class="uk-text-center slno">' + serialno
                 + '</td>'
                 + '<td class="Date">' + record.Date
                 + '<input type="hidden" class="LabTestID" value="' + record.ID + '"/>'
                 + '<input type="hidden" class="ID" value="0" />'
                 + '</td>'
                 + '<td class="TestName">' + record.Name
                 + '</td>'
                 + '<td class="ObservedValue">'
                 + '</td>'
                 + '<td class="unit">'
                 + '</td>'
                 + '<td class="Bioref">'
                 + '</td>'
                 + '<td class="low">'
                 + '</td>'
                 + '<td class="high">'
                 + '</td>'
                 + '<td>'
                 + '</td>'
                 + '<td>'
                 + '<a class="">'
                 + '<i class="uk-icon-remove labitems-remove"></i>'
                 + '</a>'
                 + '</td>'
                 + '</tr>';
            $content = $(content);
            app.format($content);
            $('#add_lab_items tbody').append($content);
        });
    },
    delete_item: function () {
        var self = PatientDiagnosis;
        $(this).closest('tr').remove();
        index = $("#add_lab_items tbody tr").length;
        var sino = 0;
        $('#add_lab_items tbody tr .sl-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_lab_items tbody tr").length);

    },
    check: function () {
        var self = PatientDiagnosis
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            //$(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            self.LabItemList.push(clean($(row).find('.chk-lab-test').val()));
            self.LabItemList2.push({ 'id': clean($(row).find('.chk-lab-test').val()), 'name': $(row).find('.Name').text() });
        } else {
            //$(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
            var value = $(row).find('.chk-lab-test').val();
            var i = 0;
            while (i < self.LabItemList.length) {
                if (self.LabItemList[i] == value) {
                    self.LabItemList.splice(i, 1);
                    self.LabItemList2.splice(i, 1);
                } else {
                    ++i;
                }
            }
        }


        //self.count();
    },

    is_patient_history: function () {
        var self = PatientDiagnosis;
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        var IsCompleted = $("#IsCompleted").val();
        var PatientID = $("#PatientID").val();
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/IsPatientHistory',
            data: {
                AppointmentProcessID: AppointmentProcessID,
                PatientID: PatientID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {

                    if (response.IsPatientHistory == false) {
                        $(".tabReview").removeClass("uk-hidden");
                        $('.fresh').iCheck("check")
                        //self.get_examination();
                        self.get_report();
                        self.get_medicine();
                        self.get_LabItems();
                        self.get_chart(AppointmentProcessID);
                        self.get_casesheet();
                        self.get_vital_chart(AppointmentProcessID);
                        self.get_vitals(AppointmentProcessID);
                        self.get_medicines_details(AppointmentProcessID);
                        self.get_medicines_Items(AppointmentProcessID);
                        self.get_treatment_item();
                        self.get_treatment();
                        self.get_doctor_list();
                        self.get_history();
                        
                        $(".tabHistory").addClass("uk-hidden");
                    }

                    else {
                        self.get_history();
                        $(".btnViewReview").removeClass("uk-hidden")
                        $(".btnView").addClass("uk-hidden")
                    }
                }
            }
        });
    },

    get_baseline_information: function () {
        var self = PatientDiagnosis;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var AppointmentProcessID = $('#ReviewID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/BaseLineInformation",
            data: {
                PatientID: PatientID,
                Date: date,
                AppointmentProcessID: AppointmentProcessID,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#base-line-information-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#base-line-information-list tbody").append($response);

            }
        });
    },
    calculate_BMI: function () {
        var self = PatientDiagnosis;
        var height = $("#Height").val();
        var HeightInMtr = (height / 100);
        var weight = $("#Weight").val();
        var BMI = (weight / (HeightInMtr * HeightInMtr)).toFixed(2);;
        //var Value = (BMI * 10000).toFixed(2);
        $("#BMI").val(BMI);
    },
    view_medicine: function () {
        $('#medicinestock').trigger('click');
    },
    close_medicine_stock_modal: function () {       
        $('#medicinestock').trigger('click');
        UIkit.modal($('#add-medicine')).show();
    }
}