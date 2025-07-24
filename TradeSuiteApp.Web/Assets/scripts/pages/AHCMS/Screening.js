Screening = {
    init: function () {
        var self = Screening;
       
        self.bind_events();
        self.get_vitals();
        self.get_chart();
        self.get_vital_chart();
        self.get_report();


        self.list();
    },

    list: function () {
        var self = Screening;

        $('#tabs-case-sheet').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = Screening;
        var $list;

        switch (type) {
            case "Previous":
                $list = $('#previous-list');
                break;
            case "Today":
                $list = $('#today-list');
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

            var url = "/AHCMS/Screening/GetOpPatientList?type=" + type

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
                        app.load_content("/AHCMS/Screening/Create/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
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
                    "url": "/AHCMS/Screening/GetOpPatientList?type=" + type,
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
                        app.load_content("/AHCMS/Screening/Create/?ID=" + PatientID + "&AppointmentScheduleItemID=" + ScheduleItemID + "&OPID=" + OPID + "&IsCompleted=" + IsCompleted + "&IsReferedIP=" + IsReferedIP);
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
        var self = Screening;
        $(".btnSave").on('click', self.save_confirm);
        $(document).on('keyup', '#Weight', self.calculate_BMI);
        $(document).on('keyup', '#Height', self.calculate_BMI);
        $("#btnaddreport").on("click", self.show_add_report);
        UIkit.uploadSelect($("#select-quotation"), self.selected_report_settings);
        $('body').on('click', 'a.remove-quotation', self.remove_report);
        $("#btnSaveReport").on('click', self.add_report);
        $("body").on("click", ".edit_report", self.edit_report);
        $("body").on("click", ".view_report", self.report_view);
    },
    report_view: function () {
        var self = Screening;
        var row = $(this).closest("tr");
        var url = $(row).find(".url").val();
        $("#print-preview-title").hide();
        $("#btnOkPrint").hide();
        app.print_preview(url);
    },

    add_report: function () {
        var self = Screening;
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

    edit_report: function () {
        var self = Screening;
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

    remove_report: function () {
        $(this).closest('span').remove();
    },

    get_report: function () {
        var self = Screening;
        var PatientID = $('#PatientID').val();
        var Date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/Screening/GetReport",
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

    show_add_report: function () {
        var self = Screening;
        self.clear_report();
        $('#show-add-report').trigger('click');
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

    VitalChartItems: [],

    save_confirm: function () {
        var self = Screening;
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

    Save: function () {
        var self = Screening;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/Screening/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/Screening/Index";
                    }, 1000);
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    get_data: function () {
        var self = Screening;
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
        data.ExaminationItems = [];
        data.ReportItems = [];
        data.VitalChartItems = [];
        data.BaseLineItems = [];
        data.OtherConditionsItems = [];
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

        $('#report-list tbody tr').each(function () {
            item = {};
            item.Name = $(this).find(".ReportName").text();
            item.Description = $(this).find(".Description").text();
            item.DocumentID = $(this).find(".ReportID").val();
            item.Date = $(this).find(".Date").text();
            data.ReportItems.push(item);
        });

        data.VitalChartItems = self.VitalChartItems;

        return data;
    },

    get_vitals: function () {
        var self = Screening;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/Screening/Vitals",
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

    get_vital_chart: function () {
        var self = Screening;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/Screening/VitalChart",
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

    clear_report: function () {
        var self = Screening;
        $("#ReportName").val('');
        $("#selected-quotation").text('');
        $("#Description").val('');
        $("#RowIndex").val('');
        $("#Index").val('');
    },

    get_chart: function () {
        var self = Screening;
        var PatientID = $('#PatientID').val();
        var date = $('#Date option:selected').val();
        var IsCompleted = $("#IsCompleted").val();
        var AppointmentProcessID = $('#AppointmentProcessID').val();
        $.ajax({
            url: "/AHCMS/Screening/Chart",
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

    calculate_BMI: function () {
        var self = Screening;
        var height = $("#Height").val();
        var HeightInMtr = (height / 100);
        var weight = $("#Weight").val();
        var BMI = (weight / (HeightInMtr * HeightInMtr)).toFixed(2);;
        //var Value = (BMI * 10000).toFixed(2);
        $("#BMI").val(BMI);
    },

    validate_save: function () {
        var self = Screening;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_report: function () {
        var self = Screening;
        if (self.rules.on_report_add.length > 0) {
            return form.validate(self.rules.on_report_add);
        }
        return 0;
    },

    rules: {

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

        on_save: [
            {
                elements: ".PresentingComplaints",
                rules: [
                    { type: form.required, message: "Please Add Presenting Complaints" },
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
    },
}