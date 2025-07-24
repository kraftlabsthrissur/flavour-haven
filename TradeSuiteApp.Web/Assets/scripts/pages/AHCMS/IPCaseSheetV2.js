

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
                    app.load_content("/AHCMS/IPCaseSheet/CreateV2/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
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
                    app.load_content("/AHCMS/IPCaseSheet/CreateV2/?ID=" + PatientID + "&IPID=" + IPID + "&OPID=" + OPID + "&IsDischarged=" + IsDischarged + "&IsDischargeAdviced=" + IsDischargeAdviced + "&DischargeSummaryID=" + DischargeSummaryID);
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
                    app.load_content("/AHCMS/IPCaseSheet/CreateV2/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
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
IPCaseSheet.Items = [];

IPCaseSheet.MedicinesItems = [];

//IPCaseSheet.VitalChartItems = [];

IPCaseSheet.LabTestItems = [];

IPCaseSheet.XrayItems = [];

IPCaseSheet.Save = function () {
    var self = IPCaseSheet;
    var data;
    data = self.get_data();
    $.ajax({
        url: '/AHCMS/IPCaseSheet/SaveV2',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Saved Successfully");
                setTimeout(function () {
                    window.location = "/AHCMS/IPCaseSheet/IndexV2";
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
    });
    //casesheet tab
    $('#case-sheet-list tbody tr .casesheetResults').each(function (i, row) {
        item = {};
        var groupname = $(this).attr('name')
        if ($(this).is('input:radio') || ($(this).is('input:text'))||($(this).is('textarea'))) {
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



