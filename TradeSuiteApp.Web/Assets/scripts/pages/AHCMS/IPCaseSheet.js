IPCaseSheet = {
    init: function () {
        var self = IPCaseSheet;
        self.get_patient_data();
        self.get_history();
        self.bind_events();
        var IsDischarged = $("#IsDischarged").val();
        if (IsDischarged == "false") {
            self.include_discharge_summary();
        }
        self.lablist();
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
    },

    list: function () {
        var self = IPCaseSheet;

        $('#tabs-iplist').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
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
                        app.load_content("/AHCMS/IPCaseSheet/Create/?ID=" + PatientID + "&IPID=" + IPID + "&OPID=" + OPID + "&IsDischarged=" + IsDischarged + "&IsDischargeAdviced=" + IsDischargeAdviced + "&DischargeSummaryID=" + DischargeSummaryID);
                    });
                }
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;

        }
    },


    bind_events: function () {
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

        $("body").on("click", ".remove-discharge-medicine", self.delete_discharge_medicine);
    },
    show_cancel_treatment: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        var EndDate = $('#TentativeEndDateForCancel').val();
        var TreatmentID = $('#TreatmentIDForCancel').val();
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
        var self = IPCaseSheet;
        var frequency = ($('.Treatmentfrequency:checked').size());
        var numberOfTreatments = $("#NoofTreatment").val();
        if (frequency == 0) {
            frequency = 1;
        }
        let currentNumberOfTreatments = (numberOfTreatments - numberOfTreatments % frequency) / frequency +
            (numberOfTreatments % frequency / frequency > 1 ? 2 : numberOfTreatments % frequency / frequency !== 0 ? 1 : 0)
        $("#Noofdays").val(currentNumberOfTreatments)
        currentNumberOfTreatments = currentNumberOfTreatments - 1;

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
        var self = IPCaseSheet;
        if ($(this).prop("checked") == true) {
            $("#MorningTimeID ").prop("disabled", false);
        }
        else {
            $("#MorningTimeID").val('');
            $("#MorningTimeID ").prop("disabled", true);
        }
    },
    remove_medicine: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        self.count();
    },
    check_noonTime: function () {
        var self = IPCaseSheet;
        if ($(this).prop("checked") == true) {
            $("#NoonTimeID").prop("disabled", false);
        }
        else {
            $("#NoonTimeID").val('');
            $("#NoonTimeID").prop("disabled", true);
        }
    },

    check_eveningTime: function () {
        var self = IPCaseSheet;
        if ($(this).prop("checked") == true) {
            $("#EveningTimeID").prop("disabled", false);
        }
        else {
            $("#EveningTimeID").val('');
            $("#EveningTimeID").prop("disabled", true);
        }
    },

    check_nightTime: function () {
        var self = IPCaseSheet;
        if ($(this).prop("checked") == true) {
            $("#NightTimeID").prop("disabled", false);
        }
        else {
            $("#NightTimeID").val('');
            $("#NightTimeID").prop("disabled", true);
        }
    },

    add_doctor: function () {
        var self = IPCaseSheet;
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
        self.count();
    },

    clear_doctor_data: function () {
        var self = IPCaseSheet;
        $("#DoctorName").val('');
        $("#DoctorNameID").val('');
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
        var self = IPCaseSheet;
        $("#DoctorNameID").val(item.id),
        $("#DoctorName").val(item.name);
        UIkit.modal($('#select-employee')).hide();
    },

    select_employee: function () {
        var self = IPCaseSheet;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorNameID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    get_history: function () {
        var self = IPCaseSheet;
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        $.ajax({
            url: "/AHCMS/PatientDiagnosis/History",
            dataType: "html",
            data: {
                OPID: OPID,
                PatientID: PatientID,
            },
            type: "POST",
            success: function (response) {
                $("#history-list").empty();
                $response = $(response);
                app.format($response);
                $("#history-list").append($response);
                $('.btnViewReview').addClass("uk-hidden");
            },
        });
    },

    get_history_details: function () {
        var self = IPCaseSheet;
        var OPID = $('#AppointmentProcessID').val();
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var ParentID = $(this).closest("tr").find(".ParentID").val();
        var AppointmentType = $(this).closest("tr").find(".AppointmentType").val();
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
                AppointmentType: AppointmentType
            },
            type: "POST",
            success: function (response) {
                $("#history-details").empty();
                $response = $(response);
                app.format($response);
                $("#history-details").append($response);
                $('.btnReview').addClass("uk-hidden");
            },
        });
    },

    show_history: function () {
        var self = IPCaseSheet;
        $(".HistoryDetails").addClass("uk-hidden");
        $(".tabReview").removeClass("uk-hidden");
    },

    set_labtest: function () {
        var self = IPCaseSheet;
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

    clear_lab_items: function () {
        var self = IPCaseSheet;
        $("#LabTest").val('');
        $("#LabTestID").val('');
    },

    get_labtest: function (OPID) {
        var self = IPCaseSheet;
        var count;
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetCheckedTest',
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

    get_xray_test: function (OPID) {
        var self = IPCaseSheet;
        var count;
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetPrescribedXrayTest',
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

    set_xray_test: function () {
        var self = IPCaseSheet;
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

    show_labtestlist: function () {
        var self = IPCaseSheet;
        $('#show-labresult').trigger('click');
    },

    delete_rounds: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#rounds-list tbody tr .rounds-serial-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#rounds-list tbody tr").length);

    },

    delete_physiotherapy: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#add_physiotherapy tbody tr .physiotherapy-serial-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_physiotherapy tbody tr").length);

    },

    delete_xray: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#add_xray tbody tr .xray-serial-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_xray tbody tr").length);

    },

    delete_vital_chart: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#add_vital_chart_list tbody tr .vital-serial-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_vital_chart_list tbody tr").length);

    },

    save_confirm: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        var data = {};
        data.AppointmentScheduleItemID = $("#AppointmentScheduleItemID").val();
        data.AppointmentProcessID = $("#AppointmentProcessID").val();
        data.IPID = $("#IPID").val(),
        data.PatientID = $("#PatientID").val(),
        data.Date = $("#Date option:selected").val();


        data.ExaminationItems = [];
        data.ReportItems = [];
        data.TreatmentItems = [];
        data.Medicines = [];
        data.MedicineItems = [];
        data.TreatmentMedicines = [];
        data.VitalChartItems = [];
        data.LabTestItems = [];
        data.PhysiotherapyItems = [];
        data.XrayItems = [];
        data.DoctorList = [];
        data.RoundsItems = [];
        data.InternalMedicine = [];
        data.InternalMedicineItems = [];
        data.DischargeSummary = [];
        data.BaseLineItems = [];
        data.OtherConditionsItems = [];
        $('.IsDischargeAdvice').prop("checked") == true ? data.IsDischargeAdvice = true : data.IsDischargeAdvice = false;
        var item = {};
        $('#examination-list tbody .examination-results').each(function () {
            item = {};

            item.ID = $(this).parent().prevAll(".ID").val();
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

        $('#report-list tbody tr').each(function () {
            item = {};
            item.ID = $(this).find(".ID").val();
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

        $('#add_physiotherapy tbody tr').each(function () {
            item = {};
            item.PhysiotherapyID = $(this).find(".PhysiotherapyID").val();
            item.StartDate = $(this).find(".FromDate").text();
            item.EndDate = $(this).find(".ToDate").text();
            data.PhysiotherapyItems.push(item);
        });

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

        $('#rounds-list tbody tr').each(function () {
            item = {};
            item.Remarks = $(this).find(".Remarks").text();
            item.RoundsDate = $(this).find(".Date").text();
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
        return data;
    },

    Save: function () {
        var self = IPCaseSheet;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/AHCMS/IPCaseSheet/Index";
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    set_medicine_name: function (event, item) {
        var self = IPCaseSheet;
        $("#Medicines").val(item.value);
        $("#MedicinesID").val(item.id);
        $("#SalesUnitID").val(item.salesunitid);
        $("#SalesUnit").val(item.salesunit);
        $("#PrimaryUnitID").val(item.primaryunitid);
        $("#PrimaryUnit").val(item.primaryunit);
        $("#CategoryID").val(item.categoryid);
        self.get_units();
        self.Get_prescription();
        $("#Qty").focus();
        self.Get_Stock();
    },

    Get_Stock: function () {
        var self = IPCaseSheet;
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

    set_external_medicine_name: function (event, item) {
        var self = IPCaseSheet;
        $("#ExternalMedicines").val(item.Name);
        $("#ExternalMedicinesID").val(item.id);
        $("#SalesUnitID").val(item.salesunitid);
        $("#SalesUnit").val(item.salesunit);
        $("#PrimaryUnitID").val(item.primaryunitid);
        $("#PrimaryUnit").val(item.primaryunit);
        $("#CategoryID").val(item.categoryid);
        self.get_medicine_units();
        self.Get_medicine_prescription();
    },

    Get_prescription: function () {
        var self = IPCaseSheet;
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

    Get_medicine_prescription: function () {
        var self = IPCaseSheet;
        var CategoryID = $('#CategoryID').val();
        $.ajax({
            url: '/Masters/PrescriptionTemplate/GetPrescription',
            dataType: "json",
            type: "POST",
            data: { CategoryID: CategoryID },
            success: function (response) {
                $("#MedicinePrescription").html("");
                var html;
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.MedicineCategory + "</option>";
                });
                $("#MedicinePrescription").append(html);
            }
        });
    },

    get_units: function () {
        var self = IPCaseSheet;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        //html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        $("#UnitID").append(html);
    },

    get_medicine_units: function () {
        var self = IPCaseSheet;
        $("#MedicineUnit").html("");
        var html = "";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        //html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        $("#MedicineUnit").append(html);
    },

    set_medicine: function (event, item) {
        var self = IPCaseSheet;
        $("#Medicine").val(item.Name);
        $("#MedicineID").val(item.id);
        $("#TreatmentMedicineUnitID").val(item.unitId);
        $("#TreatmentMedicineUnit").val(item.unit);
    },

    set_lab_test: function (event, item) {
        var self = IPCaseSheet;
        $("#LabTest").val(item.Name);
        $("#LabTestID").val(item.id);
        $("#Category").val(item.category);
    },

    set_physiotherapy: function (event, item) {
        var self = IPCaseSheet;
        $("#Physiotherapy").val(item.Name);
        $("#PhysiotherapyID").val(item.id);
    },

    set_xray: function (event, item) {
        var self = IPCaseSheet;
        $("#XrayName").val(item.Name);
        $("#XrayID").val(item.id);
    },

    set_treatment: function (event, item) {
        var self = IPCaseSheet;
        $("#TreatmentName").val(item.value);
        $("#TreatmentID ").val(item.id);
        $("#StandardMedicineQty").val(item.qty);
    },

    select_medicine: function () {
        var self = IPCaseSheet;
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
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)',  // File filter
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
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        var date = $("#StartDate").val();
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

    calculate_medicine_days: function () {
        var self = IPCaseSheet;
        var date = $("#StartDate").val();
        days = parseInt($("#NoofDay").val()) - 1;

        var FromDate = app.get_date_time(date);
        var ToDate = new Date();

        ToDate.setTime(FromDate + days * 24 * 60 * 60 * 1000);
        var day = ((ToDate.getDate().toString().length) == 1) ? '0' + (ToDate.getDate()) : (ToDate.getDate());
        var month = (ToDate.getMonth() < 9 ? '0' : '') + (ToDate.getMonth() + 1);
        var year = ToDate.getFullYear();
        FromattedDate = day + '-' + month + '-' + year;

        $("#enddate").val(FromattedDate);
    },

    check_morning: function () {
        var self = IPCaseSheet;
        if ($(".Morning ").prop("checked") == true) {
            $("#MorningID ").prop("disabled", false);
        }
        else {
            $("#MorningID").val('');
            $("#MorningID ").prop("disabled", true);
        }
    },

    check_morning_medicine: function () {
        var self = IPCaseSheet;
        if ($(".Morningbox ").prop("checked") == true) {
            $("#morningid ").prop("disabled", false);
        }
        else {
            $("#MorningID").val('');
            $("#morningid ").prop("disabled", true);
        }
    },

    check_noon_medicine: function () {
        var self = IPCaseSheet;
        if ($(".Noonbox ").prop("checked") == true) {
            $("#noonid ").prop("disabled", false);
        }
        else {
            $("#noonid").val('');
            $("#noonid ").prop("disabled", true);
        }
    },

    check_noon: function () {
        var self = IPCaseSheet;
        if ($(".Noon").prop("checked") == true) {
            $("#NoonID").prop("disabled", false);
        }
        else {
            $("#NoonID").val('');
            $("#NoonID").prop("disabled", true);
        }
    },

    check_evening: function () {
        var self = IPCaseSheet;
        if ($(".Evening").prop("checked") == true) {
            $("#EveningID").prop("disabled", false);
        }
        else {
            $("#EveningID").val('');
            $("#EveningID").prop("disabled", true);
        }
    },

    check_evening_time: function () {
        var self = IPCaseSheet;
        if ($(".Eveningbox").prop("checked") == true) {
            $("#eveningid").prop("disabled", false);
        }
        else {
            $("#eveningid").val('');
            $("#eveningid").prop("disabled", true);
        }
    },

    check_night: function () {
        var self = IPCaseSheet;
        if ($(".Night").prop("checked") == true) {
            $("#NightID").prop("disabled", false);
        }
        else {
            $("#NightID").val('');
            $("#NightID").prop("disabled", true);
        }
    },

    check_night_time: function () {
        var self = IPCaseSheet;
        if ($(".Nightbox").prop("checked") == true) {
            $("#nightid").prop("disabled", false);
        }
        else {
            $("#nightid").val('');
            $("#nightid").prop("disabled", true);
        }
    },

    Items: [],

    MedicinesItems: [],

    Medicine: [],

    InternalMedicinesItems: [],

    InternalMedicine: [],

    count: function () {
        index = $("#medicine_list tbody tr").length;
        $("#item-count").val(index);
    },

    count_treatment_medicine: function () {
        index = $("#medicinelist tbody tr").length;
        $("#treatment-medicine-count").val(index);
    },

    show_add_medicine: function () {
        var self = IPCaseSheet;
        self.clear_medicine_tab();
        $("#btnAddmedicines").removeClass('uk-hidden');
        $("#TimeDescription").removeAttr('disabled');
        $("#Medicines").removeAttr('disabled');
        $("#Qty").removeAttr('disabled');
        $("#Prescription").removeAttr('disabled');
        $(".Noon").attr("disabled", false);
        $(".Morning").attr("disabled", false);
        $(".Evening").attr("disabled", false);
        $(".Night").attr("disabled", false);
        $("#ModeOfAdministrationID").removeAttr('disabled');
        $('.emptystomach').iCheck('uncheck');
        $('.beforefod').iCheck('uncheck');
        $('.afterfood').iCheck('uncheck');
        $('.emptystomach').attr("disabled", false);
        $('.beforefod').attr("disabled", false);
        $('.afterfood').attr("disabled", false);
        $('.middleoffood').iCheck('uncheck');
        $('.withfood').iCheck('uncheck');
        $('#show-add-medicine').trigger('click');
    },

    show_add_internal_medicine: function () {
        var self = IPCaseSheet;
        self.clear_internal_medicine_tab();
        $('#show-internal-medicine').trigger('click');
    },

    show_add_report: function () {
        var self = IPCaseSheet;
        self.clear_report();
        $('#show-add-report').trigger('click');
    },

    show_add_treatment: function () {
        var self = IPCaseSheet;
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

    remove_lab_items: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#add_lab_items tbody tr .sl-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
    },

    delete_item: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#report-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#report-list tbody tr").length);

    },

    delete__treatment_medicine: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var RowIndex = clean($("#RowMedicineIndex").val());
        var MedicineID = clean($(this).closest('tr').find(".MedicinesID").val());
        var index = self.Medicine.findIndex(x => x.MedicinesID === MedicineID && x.GroupID === RowIndex);
        self.Medicine.splice(index, 1);
        var sino = 0;
        $('#internal_medicine_list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#External-item-count").val($("#internal_medicine_list tbody tr").length);

    },

    delete_medicine_on_grid: function () {
        var self = IPCaseSheet;
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

    delete_discharge_medicine: function () {
        var self = IPCaseSheet;
        $(this).closest('tr').remove();
        var GroupID = clean($(this).closest('tr').find(".GroupID").val());
        var tableindex = self.InternalMedicinesItems.findIndex(x => x.GroupID === GroupID);
        self.InternalMedicinesItems.splice(tableindex, 1);
        self.InternalMedicine = $.grep(self.InternalMedicine, function (element, index) { return element.GroupsID != GroupID; });
        var sino = 0;
        $('#add_internal_medicine tbody tr .serial-no-medicines').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#add_internal_medicine tbody tr").length);

    },



    delete_treatment_on_grid: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        self.error_count = 0;
        self.error_count = self.validate_treatment_medicine();
        if (self.error_count > 0) {
            return;
        }

        var Medicine = $("#Medicine").val();
        var MedicineID = $("#MedicineID").val();
        var Qty = $("#StandardMedicineQty").val();
        var UnitID = $("#TreatmentMedicineUnitID").val();
        var Unit = $("#TreatmentMedicineUnit").val();
        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="Medicine">' + Medicine
            + '<input type="hidden" class="TreatmentMedicineUnitID"value="' + UnitID + '"/>'
            + '<input type="hidden" class="MedicineID"value="' + MedicineID + '"/>'
            + '</td>'
            + '<td class="TreatmentMedicineUnit">' + Unit + '</td>'
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
        var self = IPCaseSheet;

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
            self.Items = $.grep(self.Items, function (element, index) { return element.TreatmentID != TreatmentID; });
        }

        //if ($("#RowIndex").val() == 0) {
        $('#medicinelist tbody tr').each(function () {
            self.Items.push({
                Madicine: $(this).find(".Medicine").text(),
                MedicineID: clean($(this).find(".MedicineID").val()),
                StandardMedicineQty: clean($(this).find(".Qty").text()),
                TreatmentID: clean($("#TreatmentID").val()),
                TreatmentMedicineUnitID: $(this).find(".TreatmentMedicineUnitID").val(),
                TreatmentMedicineUnit: $(this).find(".TreatmentMedicineUnit").text()
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
            $(rowindex).find(".Isevening").val(Isevening);
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
                + '<input type="hidden" class="Isevening"value="' + Isevening + '"/>'
                + '<input type="hidden" class="IsNight"value="' + IsNight + '"/>'
                + '<input type="hidden" class="Frequency"value="' + Frequency + '"/>'
                + '<input type="hidden" class="NoofDays"value="' + NoofDays + '"/>'

                + '</td>'
                + '<td class="EndDate">' + EndDate
                + '</td>'
                + '<td class="NoofTreatments">' + NoofTreatments
                + '</td>'
                + '<td>'
                + '</td>'
                + '<td>'
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
        var self = IPCaseSheet;
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

    add_internale_medicineitems: function () {
        var self = IPCaseSheet;
        self.error_count = 0;
        self.error_count = self.validate_external_medicine();
        if (self.error_count > 0) {
            return;
        }

        var Medicine = $("#ExternalMedicines").val();
        var MedicinesID = $("#ExternalMedicinesID").val();
        var UnitID = $("#MedicineUnit option:selected").val();
        var Unit = $("#MedicineUnit option:selected").text();
        var Quantity = $("#ExternalQty").val();

        var Prescription = $("#MedicinePrescription option:selected").text();
        var Description = $("#description").val();

        Prescription.trim() != "" ? Description.trim() != "" ? $("#description").val(Description.trim() + " + " + Prescription.trim()) : $("#description").val(Prescription.trim()) : null
        //var str = Description + " + " + Prescription;
        //var string1 = str.slice(2)
        //$("#description").val(string1);

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
            + '<a class="remove-medicines">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#internal_medicine_list tbody').append($content);
        self.external_medicine_count();
        self.clear_external_medicine_onadd();
    },

    add_medicines_on_grid: function () {
        var self = IPCaseSheet;
        self.error_count = 0;
        //$("#item-count").val($("#medicine_list tbody tr"));
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
        StartDate = $("#StartDate").val();
        EndDate = $("#EndDate").val();
        NoofDays = $("#NoofDays").val();
        Instructions = $("#Prescription option:selected").text();
        InstructionsID = $("#Prescription option:selected").val();
        Description = $("#TimeDescription").val();
        sino = $('#add_medicine-list tbody tr').length + 1;
        modeofadministrationID = $("#ModeOfAdministrationID").val();
        modeofadministration = $("#ModeOfAdministrationID").text();
        PatientMedicineID = clean($("#PatientMedicineID").val());
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

        var IsEdit = 1;
        var Frequency = ($('.frequency:checked').size());

        //Check edit or add

        var RowIndex = $('#RowMedicineIndex').val();
        if (RowIndex <= 0) {
            var Index = $('#add_medicine-list tbody tr').length;
            RowIndex = parseInt(Index) + 1;
            IsEdit = 0;
        }
        //else {


        //if (PatientMedicineID > 0) {
        //    //delete from MedicinesItems list for edit with PatientMedicineID(For already saved data -will not have groupid for first case in list)
        //    var Medicineindex = self.MedicinesItems.findIndex(x => x.PatientMedicineID == PatientMedicineID);
        //    self.MedicinesItems.splice(Medicineindex, 1);
        //    //delete from Medicine which have same PatientMedicinesID
        //    self.Medicine = $.grep(self.Medicine, function (element, index) { return element.PatientMedicinesID != PatientMedicineID; });
        //}
        //else {
        //    //delete from MedicinesItems list for edit with groupid
        //    var Medicineindex = self.MedicinesItems.findIndex(x => x.GroupID == RowIndex);
        //    self.MedicinesItems.splice(Medicineindex, 1);
        //    self.Medicine = $.grep(self.Medicine, function (element, index) { return element.GroupID != RowIndex; });
        //}
        //}
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
                item.GroupID = RowIndex,
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
                PatientMedicineID: PatientMedicineID,
                ModeOfAdministrationID: modeofadministrationID,
                MedicineInstruction: string1,
                QuantityInstruction: quantity1,
            });
            ////edit in the grid
            //var Index = RowIndex - 1;
            //if ($("#RowMedicineIndex").val() > 0) {
            //    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Medicine").text(string1);
            //    $('#add_medicine-list tbody tr:eq(' + Index + ')').find(".Instructions").text(Description);
            //    $('#add-treatment').trigger('click');
            //}
            //Add To the grid

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
            + '<td class="StartDate">' + StartDate + '</td>'
            + '<td class="EndDate">' + EndDate + '</td>'
            //+ '<td class="uk-text-center">' + '<i class="material-icons view_report">' + "remove_red_eye" + '</i>' + '</td>'
            + '<td class="Status">' + '</td>'
            + '<td>' + '' + '</td>'
             + '<td class="uk-text-center">' + '<button class="edit_medicines">' + '<i class="material-icons">' + "create" + '</i>' + '</button>' + '</td>'
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
            //$('#add-medicine').trigger('click');

            //self.clear_add_medicine();
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
                    //IsMultipleTimes: IsMultipleTimes,
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
                    //$('#add-treatment').trigger('click');
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

    stop_medicine: function (PatientMedicinesID, StopIndex) {
        var self = IPCaseSheet;
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
                        if (MedicineID == PatientMedicinesID) {
                            self.MedicinesItems.splice(i, 1);
                            i = i - 1;
                        }
                    }
                }
                for (var i = 0; i < self.Medicine.length; i++) {
                    var MedicineID;
                    MedicineID = self.Medicine[i].GroupID
                    if (MedicineID == PatientMedicinesID) {
                        self.Medicine.splice(i, 1);
                        i = i - 1;
                    }
                }
            }
        });
    },
    add_internal_medicines: function () {
        var self = IPCaseSheet;
        self.error_count = 0;
        self.error_count = self.validate_external_medicineitems();
        if (self.error_count > 0) {
            return;
        }
        MorningTime = $("#morningid option:selected").text();
        MorningTimeID = $("#morningid option:selected").val();
        NoonTime = $("#noonid option:selected").text();
        NoonTimeID = $("#noonid option:selected").val();
        EveningTime = $("#eveningid option:selected").text();
        EveningTimeID = $("#eveningid option:selected").val();
        NightTime = $("#nightid option:selected").text();
        NightTimeID = $("#nightid option:selected").val();
        StartDate = $("#StartDate").val();
        EndDate = $("#enddate").val();
        NoofDays = $("#NoofDay").val();
        Instructions = $("#MedicinePrescription option:selected").text();
        InstructionsID = $("#MedicinePrescription option:selected").val();
        Description = $("#description").val();
        sino = $('#add_internal_medicine tbody tr').length + 1;
        PatientMedicineID = $("#ExternalPatientMedicineID").val();
        DischargeSummaryID = $("#DischargeSummaryID").val();

        var IsMorning = "";
        if ($('.Morningbox').prop("checked")) {
            IsMorning = true;
        }
        else {
            IsMorning = false;
        }

        var Isevening = "";
        if ($('.Eveningbox').prop("checked")) {
            Isevening = true;
        }
        else {
            Isevening = false;
        }

        var IsNoon = "";
        if ($('.Noonbox').prop("checked")) {
            IsNoon = true;
        }
        else {
            IsNoon = false;
        }

        var IsNight = "";
        if ($('.Nightbox').prop("checked")) {
            IsNight = true;
        }
        else {
            IsNight = false;
        }

        var IsEmptyStomach = "";
        if ($('.isemptystomach').prop("checked")) {
            IsEmptyStomach = true;
        }
        else {
            IsEmptyStomach = false;
        }
        var IsBeforeFood = "";
        if ($('.isbeforefod').prop("checked")) {
            IsBeforeFood = true;
        }
        else {
            IsBeforeFood = false;
        }
        var IsAfterFood = "";
        if ($('.isafterfood').prop("checked")) {
            IsAfterFood = true;
        }
        else {
            IsAfterFood = false;
        }

        var IsMiddleOfFood = "";
        if ($('.ismiddleoffood').prop("checked")) {
            IsMiddleOfFood = true;
        }
        else {
            IsMiddleOfFood = false;
        }

        var IsWithFood = "";
        if ($('.iswithfood').prop("checked")) {
            IsWithFood = true;
        }
        else {
            IsWithFood = false;
        }


        var Frequency = ($('.ex-frequency:checked').size());

        //Check edit or add
        var RowIndex = $('#RowMedicineIndex').val();
        if (RowIndex <= 0) {
            var Index = $('#add_internal_medicine tbody tr').length;
            RowIndex = parseInt(Index) + 1;
        }
        else {
            //delete from MedicinesItems list for edit with groupid
            var Medicineindex = self.InternalMedicinesItems.findIndex(x => x.GroupID == RowIndex);
            self.InternalMedicinesItems.splice(Medicineindex, 1);

            var Medicineindex = self.InternalMedicine.findIndex(x => x.GroupID == RowIndex);
            self.InternalMedicine.splice(Medicineindex, 50);
        }

        //push into medicine items 
        self.InternalMedicinesItems.push({
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
            IsEmptyStomach: IsEmptyStomach,
            IsAfterFood: IsAfterFood,
            IsBeforeFood: IsBeforeFood,
            MorningTimeID: MorningTimeID,
            NoonTimeID: NoonTimeID,
            EveningTimeID: EveningTimeID,
            NightTimeID: NightTimeID,
            GroupID: RowIndex,
            Frequency: Frequency,
            PatientMedicineID: PatientMedicineID,
            IsMiddleOfFood: IsMiddleOfFood,
            IsWithFood: IsWithFood,
            DischargeSummaryID: DischargeSummaryID
        });
        var str = "";

        $('#internal_medicine_list tbody tr').each(function () {
            item = {};
            item.Medicines = $(this).find(".Medicines").text(),
            item.MedicinesID = clean($(this).find(".MedicinesID").val()),
            item.Unit = $(this).find(".Unit").text(),
            item.UnitID = $(this).find(".UnitID").val(),
            item.Quantity = clean($(this).find(".Quantity").text()),
            item.GroupID = RowIndex,
            str = str + " + " + item.Medicines;
            self.InternalMedicine.push(item);

        });

        string = str.replace(/,(\s+)?$/, '');
        var string1 = string.slice(2)
        ////edit in the grid
        var Index = $("#Index").val();
        if ($("#RowMedicineIndex").val() > 0) {
            var rowindex = $('#add_internal_medicine tbody').find('tr').eq(Index);
            $(rowindex).find(".Medicines").text(string1);
            $(rowindex).find(".Instructions").text(Description);
            $(rowindex).find(".StartDateMed").text(StartDate);
            $(rowindex).find(".EndDate").text(EndDate);
            $('#add-treatment').trigger('click');
        }
            //Add To the grid
        else {
            var content = "";
            var $content;
            content = '<tr>'
            + '<td class="serial-no-medicines uk-text-center">' + sino + '</td>'
            + '<td class="Medicines">' + string1 + '</td>'
            + '<td class="Instructions">' + Description + '</td>'
            + '<td class="uk-hidden">'
            + '<input type="hidden" class="GroupID"value="' + RowIndex + '"/>'
            + '</td>'
            + '<td class="StartDate">' + StartDate + '</td>'
            + '<td class="EndDate">' + EndDate + '</td>'
            //+ '<td class="uk-text-center">' + '<button class="edit_internal_medicines">' + '<i class="material-icons">' + "create" + '</i>' + '</button>' + '</td>'
            //+ '<td class="uk-text-center">' + '<i class="material-icons view_report">' + "remove_red_eye" + '</i>' + '</td>'
            + '<td>'
            + '<a class="remove-discharge-medicine">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'

            //+ '<a class="remove-medicine_ongrid">'
            //+ '<i class="uk-icon-remove"></i>'
            //+ '</a>'

            + '</td>'
            + '</tr>';
            $content = $(content);
            app.format($content);
            $('#add_internal_medicine tbody').append($content);
        }
        $('#add-intrenal-medicine').trigger('click');

        self.clear_external_medicine();
    },

    edit_report: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
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
        var NoofTreatments = $(row).find(".NoofTreatments").text();

        var IsMorning = $(row).find(".IsMorning").val().trim();
        var MorningTimeID = $(row).find(".MorningTimeID").val();
        var MorningTime = $(row).find(".MorningTime").val().trim();

        var IsNoon = $(row).find(".IsNoon").val().trim();
        var NoonTime = $(row).find(".NoonTime").val().trim();
        var NoonTimeID = $(row).find(".NoonTimeID").val();

        var Isevening = $(row).find(".Isevening").val().trim();
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
        $("#NoofTreatment").val(NoofTreatments);

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
                    + '<input type="hidden" class="MedicineID"value="' + record.MedicineID + '"/>'
                    + '<input type="hidden" class="TreatmentMedicineUnitID"value="' + record.TreatmentMedicineUnitID + '"/>'
                    + '</td>'
                    + '<td class="TreatmentMedicineUnit">' + record.TreatmentMedicineUnit + '</td>'
                    + '<td class="Qty">' + record.StandardMedicineQty + '</td>'
                    + '<td>'
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
        var self = IPCaseSheet;
        self.clear_medicine_tab();
        var row = $(this).closest('tr');
        var GroupID = $(row).find(".GroupID").val();
        var index = row.index();
        //var GroupID = index + 1
        var PatientMedicinesID = $(row).find(".PatientMedicineID").val();

        $("#RowMedicineIndex").val(GroupID);
        $("#PatientMedicineID").val(PatientMedicinesID);
        $("#StopIndex").val(index);
        //var Index = $(row).index();

        //$("#btnAddmedicines").addClass('uk-hidden');
        //$("#TimeDescription").attr('disabled', 'disabled');
        //$("#Medicines").attr('disabled', 'disabled');
        //$("#Qty").attr('disabled', 'disabled');
        //$("#Prescription").attr('disabled', 'disabled');
        //$(".Noon").attr("disabled", true);
        //$(".Morning").attr("disabled", true);
        //$(".Evening").attr("disabled", true);
        //$(".Night").attr("disabled", true);
        //$("#ModeOfAdministrationID").attr('disabled', 'disabled');
        //$(".emptystomach").attr("disabled", true);
        //$(".beforefod").attr("disabled", true);
        //$(".afterfood").attr("disabled", true);
        //$(".middleoffood").attr("disabled", true);
        //$(".withfood").attr("disabled", true);

        //if ($(".Morning").prop('checked') == true) {
        //    $("#MorningID").prop("disabled", true);
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
                     + '</td>'
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
                    $("#StartDate").val(record.StartDate);
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
        self.count();
        $('#show-add-medicine').trigger('click');
    },

    edit_internal_medicines: function () {
        var self = IPCaseSheet;
        self.clear_internal_medicine_tab();
        var row = $(this).closest('tr');
        var GroupID = $(row).find(".GroupID").val();
        var PatientMedicineID = $(row).find(".PatientMedicineID").val();
        $("#PatientMedicineID").val(PatientMedicineID);
        //var Index = $(row).index();
        $("#RowMedicineIndex").val(GroupID);
        //$("#Index").val(Index);
        $('#internal_medicine_list tbody').empty();
        $(self.InternalMedicine).each(function (i, record) {
            var content = "";
            var $content;
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
                 + '<a class="remove-external-medicines">'
                 + '<i class="uk-icon-remove"></i>'
                 + '</a>'
                 + '</td>'
                 + '</tr>';
                $content = $(content);
                app.format($content);
                $('#internal_medicine_list tbody').append($content);
                self.external_medicine_count();
                self.clear_internal_medicine_tab();

            }
        });
        $(self.InternalMedicinesItems).each(function (i, record) {
            if (record.GroupID == GroupID) {
                $("#description").val(record.Description);
                $("#MedicinePrescription").val(record.InstructionsID);
                $("#StartDate").val(record.StartDate);
                $("#enddate").val(record.EndDate);
                $("#NoofDay").val(record.NoofDays);
                $("#MorningID").val(record.MorningTimeID);
                $("#noonid").val(record.NoonTimeID);
                $("#eveningid").val(record.EveningTimeID);
                $("#nightid").val(record.NightTimeID);
                if (record.IsAfterFood == true) {
                    $('.afterfood').prop("checked")
                }
                if (record.IsMorning == true) {
                    $('.Morningbox').iCheck("check")
                }
                if (record.Isevening == true) {
                    $('.Eveningbox').iCheck("check")
                }
                if (record.IsNoon == true) {
                    $('.Noonbox').iCheck("check")
                }
                if (record.IsNight == true) {
                    $('.Nightbox').iCheck("check")
                }
                if (record.IsEmptyStomach == true) {
                    $('.emptystomach').prop("checked")
                }
                if (record.IsBeforeFood == true) {
                    $('.beforefod').prop("checked")
                }
                if (record.IsMiddleOfFood == true) {
                    $('.middleoffood').prop("checked")
                }
                if (record.IsWithFood == true) {
                    $('.withfood').prop("checked")
                }
            }
        });

        $('#show-internal-medicine').trigger('click');
    },

    external_medicine_count: function () {
        var self = IPCaseSheet;
        index = $("#internal_medicine_list tbody tr").length;
        $("#External-item-count").val(index);
    },

    clear_medicine_tab: function () {
        var self = IPCaseSheet;
        $('#medicine_list tbody').text('');
        $("#TimeDescription").val('');
        $("#MorningID").val('');
        $("#NoonID").val('');
        $("#EveningID").val('');
        $("#NightID").val('');
        $("#StartDateMed").val('');
        $("#EndDate").val('');
        $("#NoofDays").val('');
        $("#Prescription").val('');
        $(".Morning").iCheck('uncheck');
        $(".Evening").iCheck('uncheck');
        $(".Noon").iCheck('uncheck');
        $(".Night").iCheck('uncheck');
        $("#Medicines").val('');
        $("#Qty").val('');
        $("#UnitID option:selected").text('Select');
        $("#MedicinesID").val('');
        $("#ModeOfAdministrationID").val('');
        $("#RowMedicineIndex").val('');
        $("#PatientMedicinesID").val('');
    },

    clear_internal_medicine_tab: function () {
        var self = IPCaseSheet;
        $('#internal_medicine_list tbody').text('');
        $("#description").val('');
        $("#morningid").val('');
        $("#noonid").val('');
        $("#eveningid").val('');
        $("#nightid").val('');
        //$("#StartDate").val('');
        $("#enddate").val('');
        $("#NoofDay").val('');
        $("#MedicinePrescription").val('');
        $(".Morningbox").iCheck('uncheck');
        $(".Eveningbox").iCheck('uncheck');
        $(".Noonbox").iCheck('uncheck');
        $(".Nightbox").iCheck('uncheck');
        $('.middleoffood').iCheck('uncheck');
        $('.withfood').iCheck('uncheck');
        $('.afterfood').iCheck('uncheck');
        $('.beforefod').iCheck('uncheck');
        $('.emptystomach').iCheck('uncheck');
        $("#ExternalMedicines").val('');
        $("#ExternalQty").val('');
        $("#MedicineUnit option:selected").text('Select');
        $("#ExternalMedicinesID").val('');
    },

    clear_add_medicine: function () {
        var self = IPCaseSheet;
        $("#Prescription").val('');
        $('#medicine_list tbody').text('');
        $("#TimeDescription").val('');
        $(".Morning").iCheck('uncheck');
        $(".Noon").iCheck('uncheck');
        $(".Evening").iCheck('uncheck');
        $(".Night").iCheck('uncheck');
        $("#MorningID").val('');
        $("#NoonID").val('');
        $("#EveningID").val('');
        $("#NightID ").val('');
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
        $("#item-count").val(0);
    },

    clear_external_medicine: function () {
        var self = IPCaseSheet;
        $("#MedicinePrescription").val('');
        $('#medicine_list tbody').text('');
        $("#description").val('');
        $(".Morningbox").iCheck('uncheck');
        $(".Noonbox").iCheck('uncheck');
        $(".Eveningbox").iCheck('uncheck');
        $(".Nightbox").iCheck('uncheck');
        $("#MorningID").val('');
        $("#noonid").val('');
        $("#eveningid").val('');
        $("#nightid ").val('');
        $("#enddate").val('');
        $("#NoofDay").val('');
        $("#ExternalMedicines").val('');
        $("#ExternalQty").val('');
        $("#MedicineUnit option:selected").text('Select');
        $("#MedicinesID").val('');
        $('#RowMedicineIndex').val('');
        $('#External-item-count').val(0);
    },

    clear_medicine_onadd: function () {
        var self = IPCaseSheet;
        $("#Medicines").val('');
        $("#Qty").val('');
        $("#UnitID option:selected").text('Select');
        $("#MedicinesID").val('');
        $("#Prescription").val('');
        $("#MedicineStock").val('');
        $("#TotalStock").val('');
        $(".StockItems").addClass('uk-hidden');
    },

    clear_external_medicine_onadd: function () {
        var self = IPCaseSheet;
        $("#ExternalMedicines").val('');
        $("#ExternalQty").val('');
        $("#MedicineUnit option:selected").text('Select');
        $("#ExternalMedicinesID").val('');
        $("#MedicinePrescription").val('');
    },

    clear_medicine: function () {
        var self = IPCaseSheet;
        $("#Medicine").val('');
        $("#MedicineID").val('');
        $("#StandardMedicineQty").val('');
        $("#TreatmentMedicineUnitID").val('');
        $("#TreatmentMedicineUnit").val('');
    },

    clear_treatment: function () {
        var self = IPCaseSheet;
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
        var self = IPCaseSheet;
        $("#ReportName").val('');
        $("#selected-quotation").text('');
        $("#Description").val('');
        $("#RowIndex").val('');
        $("#Index").val('');
    },

    clear_vital_chart: function () {
        var self = IPCaseSheet;
        $("#Time").val('');
        $("#BP").val('');
        $("#Pulse").val('');
        $("#Temperature").val('');
        $("#HR").val('');
        $("#RR").val('');
        $("#Weight").val('');
        $("#Height").val('');
        $("#Others").val('');
    },

    get_patient_data: function () {
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
        self.get_doctor_list();
        self.get_discharge_summary();
        self.get_external_medicine();
        self.get_external_medicines_details();
        self.get_external_medicines_Items();
    },

    get_doctor_list: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetDoctorList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                Date: Date,
                IPID: IPID
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

    get_vital_chart: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();

        $.ajax({
            url: "/AHCMS/IPCaseSheet/VitalChart",
            data: {
                PatientID: PatientID,
                IPID: IPID,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#add_vital_chart_list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_vital_chart_list tbody").append($response);
            }
        });

    },

    get_chart: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();

        $.ajax({
            url: "/AHCMS/IPCaseSheet/Chart",
            data: {
                PatientID: PatientID,
                Date: date,
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

    get_treatment: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var IsDischarged = $("#IsDischarged").val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/Treatment",
            data: {
                PatientID: PatientID,
                IPID: IPID,
                IsDischarged: IsDischarged
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

    get_treatment_summary: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/TreatmentSummary",
            data: {
                PatientID: PatientID,
                IPID: IPID,
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

    get_report: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/GetReport",
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
    },

    get_casesheet: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/CaseSheet",
            data: {
                PatientID: PatientID,
                Date: Date,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $(".case-sheet").html(response);
            }
        });
    },

    get_rounds: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/Rounds",
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
    },

    report_view: function () {
        var self = IPCaseSheet;
        var row = $(this).closest("tr");
        var url = $(row).find(".url").val();
        $("#print-preview-title").hide();
        $("#btnOkPrint").hide();
        app.print_preview(url);
    },

    show_xray_result: function () {
        var self = IPCaseSheet;
        var row = $(this).closest("tr");
        var Name = $(row).find(".DocumentName").val();
        var URL = $(row).find(".URL").val();
        // var url = "/" + "Uploads" + "/" + path;
        $("#print-preview-title").hide();
        $("#btnOkPrint").hide();
        app.print_preview(URL);
    },

    get_treatment_item: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetTreatmentMedicineList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                IPID: IPID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        //self.Items = [];
                        self.Items.push({
                            TreatmentID: record.TreatmentID, MedicineID: record.MedicineID, Madicine: record.Medicine,
                            StandardMedicineQty: record.StandardMedicineQty, TreatmentMedicineUnitID: record.TreatmentMedicineUnitID,
                            TreatmentMedicineUnit: record.TreatmentMedicineUnit
                        });
                    });
                }
            },
        });
    },

    get_medicines_details: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetMedicinesList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                IPID: IPID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.Medicine.push({
                            MedicinesID: record.MedicineID, UnitID: record.UnitID, Unit: record.Unit, Medicines: record.Medicine,
                            GroupID: record.TransID, Quantity: record.Quantity, PatientMedicinesID: record.TransID
                        });
                    });
                }
            },
        });
        self.count();
    },

    get_external_medicines_details: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var DischargeSummaryID = $('#DischargeSummaryID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetExternalMedicinesList/',
            dataType: "json",
            data: {
                DischargeSumaryID: DischargeSummaryID,
                PatientID: PatientID,
                IPID: IPID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.InternalMedicine.push({
                            MedicinesID: record.MedicineID, UnitID: record.UnitID, Unit: record.Unit, Medicines: record.Medicine,
                            GroupID: record.TransID, Quantity: record.Quantity, PatientMedicinesID: record.PatientMedicinesID
                        });
                    });
                }
            },
        });
        self.count();
    },

    get_medicines_Items: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetMedicinesItemsList/',
            dataType: "json",
            data: {
                PatientID: PatientID,
                IPID: IPID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.MedicinesItems.push({
                            NoofDays: record.NoofDays, IsMorning: record.IsMorning, IsNoon: record.IsNoon, Isevening: record.Isevening,
                            IsNight: record.IsNight, MorningTime: record.MorningTime, NoonTime: record.NoonTime, EveningTime: record.EveningTime,
                            NightTime: record.NightTime, IsEmptyStomach: record.IsEmptyStomach, IsBeforeFood: record.IsBeforeFood,
                            IsAfterFood: record.IsAfterFood, Description: record.Description, GroupID: record.PatientMedicineID, MorningTimeID: record.MorningTimeID,
                            EveningTimeID: record.EveningTimeID, NoonTimeID: record.NoonTimeID, NightTimeID: record.NightTimeID,
                            StartDate: record.StartDate, EndDate: record.EndDate, PatientMedicineID: record.PatientMedicineID,
                            ModeOfAdministrationID: record.ModeOfAdministrationID, IsWithFood: record.IsWithFood, IsMiddleOfFood: record.IsMiddleOfFood,
                            MedicineInstruction: record.MedicineInstruction, QuantityInstruction: record.QuantityInstruction
                        });
                    });
                }
            },
        });
    },

    get_external_medicines_Items: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var DischargeSummaryID = $('#DischargeSummaryID').val();
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetExternalMedicinesItemsList/',
            dataType: "json",
            data: {
                DischargeSummaryID: DischargeSummaryID,
                PatientID: PatientID,
                IPID: IPID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        self.InternalMedicinesItems.push({
                            NoofDays: record.NoofDays, IsMorning: record.IsMorning, IsNoon: record.IsNoon, Isevening: record.Isevening,
                            IsNight: record.IsNight, MorningTime: record.MorningTime, NoonTime: record.NoonTime, EveningTime: record.EveningTime,
                            NightTime: record.NightTime, IsEmptyStomach: record.IsEmptyStomach, IsBeforeFood: record.IsBeforeFood,
                            IsAfterFood: record.IsAfterFood, Description: record.Description, GroupID: record.PatientMedicineID, MorningTimeID: record.MorningTimeID,
                            EveningTimeID: record.EveningTimeID, NoonTimeID: record.NoonTimeID, NightTimeID: record.NightTimeID,
                            StartDate: record.StartDate, EndDate: record.EndDate, PatientMedicineID: 0, DischargeSummaryID: DischargeSummaryID
                        });
                    });
                }
            },
        });
    },

    get_medicine: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var IsDischarged = $("#IsDischarged").val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/Medicines",
            dataType: "html",
            data: {
                PatientID: PatientID,
                IPID: IPID,
                IsDischarged: IsDischarged
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

    get_external_medicine: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        var DischargeSummaryID = $('#DischargeSummaryID').val();
        var IsDischarged = $("#IsDischarged").val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/ExternalMedicines",
            dataType: "html",
            data: {
                DischargeSummaryID: DischargeSummaryID,
                PatientID: PatientID,
                IPID: IPID,
                IsDischarged: IsDischarged
            },
            type: "POST",
            success: function (response) {
                $("#add_internal_medicine tbody").empty();
                $response = $(response);
                app.format($response);
                $("#add_internal_medicine tbody").append($response);
            },
        });
    },

    validate_medicine: function () {
        var self = IPCaseSheet;
        if (self.rules.on_medicine_add.length > 0) {
            return form.validate(self.rules.on_medicine_add);
        }
        return 0;
    },

    validate_external_medicineitems: function () {
        var self = IPCaseSheet;
        if (self.rules.on_medicine_items.length > 0) {
            return form.validate(self.rules.on_medicine_items);
        }
        return 0;
    },

    validate_external_medicine: function () {
        var self = IPCaseSheet;
        if (self.rules.on_external_medicine_add.length > 0) {
            return form.validate(self.rules.on_external_medicine_add);
        }
        return 0;
    },

    validate_treatment: function () {
        var self = IPCaseSheet;
        if (self.rules.on_treatment_add.length > 0) {
            return form.validate(self.rules.on_treatment_add);
        }
        return 0;
    },

    validate_report: function () {
        var self = IPCaseSheet;
        if (self.rules.on_report_add.length > 0) {
            return form.validate(self.rules.on_report_add);
        }
        return 0;
    },

    validate_sub_medicine: function () {
        var self = IPCaseSheet;
        if (self.rules.on_sub_medicine_add.length > 0) {
            return form.validate(self.rules.on_sub_medicine_add);
        }
        return 0;
    },

    validate_add_vital_chart: function () {
        var self = IPCaseSheet;
        if (self.rules.on_vital_chart_add.length > 0) {
            return form.validate(self.rules.on_vital_chart_add);
        }
        return 0;
    },

    validate_treatment_medicine: function () {
        var self = IPCaseSheet;
        if (self.rules.on_treatment_medicine.length > 0) {
            return form.validate(self.rules.on_treatment_medicine);
        }
        return 0;
    },

    validate_save: function () {
        var self = IPCaseSheet;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_rounds: function () {
        var self = IPCaseSheet;
        if (self.rules.on_add_rounds.length > 0) {
            return form.validate(self.rules.on_add_rounds);
        }
        return 0;
    },

    validate_lab_tests: function () {
        var self = IPCaseSheet;
        if (self.rules.on_add_lab_tests.length > 0) {
            return form.validate(self.rules.on_add_lab_tests);
        }
        return 0;
    },

    validate_physiotherapy: function () {
        var self = IPCaseSheet;
        if (self.rules.on_add_physiotherapy.length > 0) {
            return form.validate(self.rules.on_add_physiotherapy);
        }
        return 0;
    },

    validate_add_xray: function () {
        var self = IPCaseSheet;
        if (self.rules.on_add_xray.length > 0) {
            return form.validate(self.rules.on_add_xray);
        }
        return 0;
    },

    rules: {
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
    //    elements: "#ExternalPatientMedicineID",
    //    rules: [
    //                { type: form.non_zero, message: "Please Add Medicine!" },

    //    ]
    //},
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
        { type: form.required, message: "Please add atleast one medicine" },

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
         elements: "#ExternalMedicinesID",
         rules: [
         { type: form.required, message: "Please select Medicines" },
         { type: form.non_zero, message: "Please select Medicines" },
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
            elements: "#Remark",
            rules: [
            { type: form.required, message: "Please Add Remark" },
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
    },

    add_vita_chart: function () {
        var self = IPCaseSheet;
        if (self.validate_add_vital_chart() > 0) {
            return;
        }
        sino = $('#add_vital_chart_list tbody tr ').length + 1;
        var Date = $("#VitalChartDate").val();
        var BP = $("#BP").val();
        var Pulse = $("#Pulse").val();
        var Temperature = $("#Temperature").val();
        var HR = $("#HR").val();
        var RR = $("#RR").val();
        var Weight = $("#Weight").val();
        var Height = $("#Height").val();
        var Others = $("#Others").val();
        var Time = $("#Time").val();

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="vital-serial-no uk-text-center">' + sino + '</td>'
            + '<td class="Date">' + Date + '</td>'
            + '<td class="Time">' + Time + '</td>'
            + '<td class="BP">' + BP + '</td>'
            + '<td class="Pulse">' + Pulse + '</td>'
            + '<td class="Temperature">' + Temperature + '</td>'
            + '<td class="HR">' + HR + '</td>'
            + '<td class="RR">' + RR + '</td>'
            + '<td class="Weight">' + Weight + '</td>'
            + '<td class="Height">' + Height + '</td>'
            + '<td class="Others">' + Others + '</td>'
            + '<td>'
            + '<a class="vital-chart-remove">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_vital_chart_list tbody').append($content);
        self.clear_vital_chart();

    },

    add_rounds: function () {
        var self = IPCaseSheet;
        if (self.validate_rounds() > 0) {
            return;
        }
        sino = $('#rounds-list tbody tr ').length + 1;
        var Date = $("#RoundsDate").val();
        var Remarks = $("#Remark").val();

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="rounds-serial-no uk-text-center">' + sino + '</td>'
            + '<td class="Date">' + Date + '</td>'
            + '<td class="Remarks">' + Remarks + '</td>'
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

    },

    clear_rounds: function () {
        var self = IPCaseSheet;
        $("#RoundsDate").val('');
        $("#Remark").val('');
    },

    clear_xray: function () {
        var self = IPCaseSheet;
        $("#XrayName").val('');
        $("#XrayID").val('');
    },

    clear_lab_items: function () {
        var self = IPCaseSheet;
        $("#LabTestID").val('');
        $("#LabTest").val('');
    },

    clear_physiotherapy: function () {
        var self = IPCaseSheet;
        $("#Physiotherapy").val('');
        $("#PhysiotherapyID").val('');
        $("#PhysioFromDate").val('');
        $("#PhysioToDate").val('');
    },

    add_lab_item: function () {
        var self = IPCaseSheet;
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
            + '<td class="labitem-serial-no uk-text-center">' + sino
            + '<input type="hidden" class="LabTestID"value="' + LabTestID + '"/>' + '</td>'
            + '<td class="Date">' + Date + '</td>'
            + '<td class="LabItems">' + LabItems + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td>'
            + '<a class="labitems-remove">'
            + '<i class="uk-icon-remove "></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_lab_items tbody').append($content);
        self.clear_lab_items();

    },

    add_physiotherapy: function () {
        var self = IPCaseSheet;
        if (self.validate_physiotherapy() > 0) {
            return;
        }
        sino = $('#add_physiotherapy tbody tr ').length + 1;
        var Physiotherapy = $("#Physiotherapy").val();
        var PhysiotherapyID = $("#PhysiotherapyID").val();
        var FromDate = $("#PhysioFromDate").val();
        var ToDate = $("#PhysioToDate").val();

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="physiotherapy-serial-no uk-text-center">' + sino
            + '<input type="hidden" class="PhysiotherapyID"value="' + PhysiotherapyID + '"/>' + '</td>'
            + '<td class="Physiotherapy">' + Physiotherapy + '</td>'
            + '<td class="FromDate">' + FromDate + '</td>'
            + '<td class="ToDate">' + ToDate + '</td>'
            + '<td class="">' + '' + '</td>'
            + '<td>'
            + '<a class="physiotherapy-remove">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#add_physiotherapy tbody').append($content);
        self.clear_physiotherapy();

    },

    get_LabItems: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/LabTest",
            dataType: "html",
            data: {
                PatientID: PatientID,
                IPID: IPID
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

    include_discharge_summary: function () {
        var self = IPCaseSheet;
        if ($(".IsDischargeAdvice").is(":checked")) {
            $('#CourseInTheHospital').removeAttr('disabled');
            $('#ConditionAtDischarge').removeAttr('disabled');
            $('#DietAdvice').removeAttr('disabled');
            $("#btn_add_internal_medicine").removeClass('uk-hidden');

        } else {
            $('#CourseInTheHospital').attr('disabled', 'disabled');
            $('#ConditionAtDischarge').attr('disabled', 'disabled');
            $('#DietAdvice').attr('disabled', 'disabled');
            $("#btn_add_internal_medicine").addClass('uk-hidden');
        }
    },

    get_discharge_summary: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/DischargeSummary",
            data: {
                PatientID: PatientID,
                IPID: IPID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#CourseInTheHospital").val(response.Data.CourseInTheHospital);
                $("#ConditionAtDischarge").val(response.Data.ConditionAtDischarge);
                $("#DietAdvice").val(response.Data.DietAdvice);
            }
        });
    },
    check: function () {
        var self = IPCaseSheet;
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

    PrescribedTest: [],
    get_data_from_modal: function () {
        var self = IPCaseSheet;
        var data = {};
        //var checkboxes = $('#labtest-list tbody');
        PrescribedTest = [];
        var item = {};
        //$.each(checkboxes, function () {
        //    item = {};
        //    row = $(this).closest("tr");
        //    item.Name = $(this).find(".Name").text();
        //    item.ID = $(this).find(".ItemID").val();
        //    item.Date = $("#TestDate").val();
        //    self.PrescribedTest.push(item);
        //});
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
        var self = IPCaseSheet;
        $(self.PrescribedTest).each(function (i, record) {
            var serialno = $('#add_lab_items tr').length;
            var content = "";
            var $content;
            content = '<tr class="LabItems sl-no">'
                 + '<td class="uk-text-center sl-no">' + serialno
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
                 + '<td>'
                 + '</td>'
                 + '<td>'
                 + '<a class="labitems-remove">'
                 + '<i class="uk-icon-remove labitems-remove"></i>'
                 + '</a>'
                 + '</td>'
                 + '</tr>';
            $content = $(content);
            app.format($content);
            $('#add_lab_items tbody').append($content);
        });
    },
    get_baseline_information: function () {
        var self = IPCaseSheet;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/IPCaseSheet/BaseLineInformation",
            data: {
                PatientID: PatientID,
                Date: date,
                IPID: IPID,
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
}