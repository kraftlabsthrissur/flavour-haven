AppointmentSchedule.date_List = function ($list, type) {
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
                "url": "/AHCMS/AppointmentSchedule/GetAppointmentScheduleList?type=" + type,
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
                        + "<input type='hidden' class='ID' value='" + row.ID + "' >"
                        + "<input type='hidden' class='DoctorID' value='" + row.DoctorID + "' >"
                        + "<input type='hidden' class='PatientID' value='" + row.PatientID + "' >"
                        + "<input type='hidden' class='AppointmentItemID' value='" + row.AppointmentItemID + "' >"
                        + "<input type='hidden' class='Date' value='" + row.FromDate + "' >"
                        + "<input type='hidden' class='BillablesID' value='" + row.BillablesID + "' >"
                        + "<input type='hidden' class='IsConfirmed' value='" + row.IsConfirmed + "' >"
                        + "<input type='hidden' class='IsProcessed' value='" + row.IsProcessed + "' >"
                        + "<input type='hidden' class='DepartmentID' value='" + row.DepartmentID + "' >"
                        + "<input type='hidden' class='ConsultationStatus' value='" + row.ConsultationStatus + "' >";
                    }
                },
                { "data": "Doctor", "className": "Doctor", },
                { "data": "PatientCode", "className": "PatientCode" },
                { "data": "Patient", "className": "Patient" },
                { "data": "Time", "className": "Time" },
                { "data": "TokenNo", "className": "TokenNo uk-text-right" },
                //{
                //    "data": "", "searchable": false, "className": "edit", "orderable": false,
                //    "render": function (data, type, row, meta) {
                //        if (row.IsProcessed == false) {
                //            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnedit'>Edit</button>"
                //        }
                //        else {
                //            return "";
                //        }
                //    }
                //},
                   {
                       "data": "", "searchable": false, "className": "billing", "orderable": false,
                       "render": function (data, type, row, meta) {
                           if (row.IsConfirmed == false) {
                               return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnbilling'>Confirm</button>"
                           }
                           else {
                               return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light print'>Print</button>"
                           }
                       }
                   },

                    {
                        "data": "", "searchable": false, "className": "cancel", "orderable": false,
                        "render": function (data, type, row, meta) {
                            if (row.IsProcessed == false) {
                                return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light cancel_appointment'>Cancel</button>"
                            }else {
                                return "";
                            }

                        }
                    },
                    {
                        "data": "", "searchable": false, "className": "billing", "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light patientcard'>PatientCard</button>"
                        }
                    },


            ],
            "createdRow": function (row, data, index) {
                $(row).addClass();
                app.format(row);
            },
            "drawCallback": function () {
            },
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });

        return list_table;
    }
},

AppointmentSchedule.future_past_List= function ($list, type) {
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
            "aaSorting": [[7, "desc"]],
            "ajax": {
                "url": "/AHCMS/AppointmentSchedule/GetAppointmentScheduleList?type=" + type,
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
                        + "<input type='hidden' class='ID' value='" + row.ID + "' >"
                        + "<input type='hidden' class='DoctorID' value='" + row.DoctorID + "' >"
                        + "<input type='hidden' class='PatientID' value='" + row.PatientID + "' >"
                        + "<input type='hidden' class='AppointmentItemID' value='" + row.AppointmentItemID + "' >"
                        + "<input type='hidden' class='Date' value='" + row.FromDate + "' >"
                        + "<input type='hidden' class='BillablesID' value='" + row.BillablesID + "' >"
                        + "<input type='hidden' class='IsConfirmed' value='" + row.IsConfirmed + "' >"
                        + "<input type='hidden' class='DepartmentID' value='" + row.DepartmentID + "' >"
                        + "<input type='hidden' class='IsProcessed' value='" + row.IsProcessed + "' >"
                        + "<input type='hidden' class='ConsultationStatus' value='" + row.ConsultationStatus + "' >";

                    }
                },
                { "data": "Doctor", "className": "Doctor", },
                { "data": "PatientCode", "className": "PatientCode" },
                { "data": "Patient", "className": "Patient" },
                { "data": "Time", "className": "Time" },
                { "data": "TokenNo", "className": "TokenNo uk-text-right" },
                { "data": "Date", "className": "Date", },
                //{
                //    "data": "", "searchable": false, "className": "edit", "orderable": false,
                //    "render": function (data, type, row, meta) {
                //        if (row.IsProcessed == 0) {
                //            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnedit'>Edit</button>"
                //        }
                //        else {
                //            return "";
                //        }
                //    }
                //},
                {
                    "data": "", "searchable": false, "className": "billing", "orderable": false,
                    "render": function (data, type, row, meta) {
                        if (row.IsConfirmed == 0) {
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnbilling'>Confirm</button>"
                        }
                        else {
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light print'>Print</button>"

                        }
                    }
                },

                    {
                        "data": "", "searchable": false, "className": "cancel", "orderable": false,
                        "render": function (data, type, row, meta) {
                            if (row.ConsultationStatus == 'Not Consulted' && row.IsConfirmed == true) {
                                //return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light cancel_appointment'>Cancel</button>"
                                return "";
                            }
                            if (row.ConsultationStatus == 'Consulted' || row.IsConfirmed == false) {
                                return "";
                            }
                        }
                    },
                    {
                        "data": "", "searchable": false, "className": "billing", "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light patientcard'>PatientCard</button>"
                        }
                    },
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass();
                app.format(row);
            },
            "drawCallback": function () {
            },
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });

        return list_table;
    }
},

AppointmentSchedule.print_Appointment = function () {
    var self = AppointmentSchedule;
    $.ajax({
        url: '/Reports/AHCMS/AppointmentBookingPrintPdfV2',
        data: {
            id: $("#AppointmentScheduleItemID").val(),
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
AppointmentSchedule.print_Appointment_booking= function () {
    var self = AppointmentSchedule;
    var AppointmentItemID = $(this).parents('tr').find('.AppointmentItemID').val();
    $.ajax({
        url: '/Reports/AHCMS/AppointmentBookingPrintPdfV2',
        data: {
            id: AppointmentItemID
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
AppointmentSchedule.save_appointment= function () {
    var self = AppointmentSchedule;
    var modal = self.get_data_for_appointment();
    $.ajax({
        url: '/AHCMS/AppointmentSchedule/SaveAppointment',
        data: modal,
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data.Status == "Success") {
                app.show_notice("Appointment scheduled successfully");
                //UIkit.modal($('#show-appointment-fee')).hide();
                //self.print_Appointment();
                setTimeout(function () {
                    window.location = "/AHCMS/AppointmentSchedule/IndexV2";
                }, 1000);
            } else {
                app.show_error(data.Message);
                $(" .btnSave").css({ 'visibility': 'visible' });
            }
        },
    })
},
AppointmentSchedule.cancellation=function (PatientID, AppointmentItemID, Date, BillablesID) {
    var self = AppointmentSchedule;
    var a = Date.split(" ", 3);
    var b = a[0].split('/');
    var day, month, year;

    if (b[0] <= 9) {
        month = "0" + b[0];
    }
    else {
        month = b[0];
    }

    if (b[1] <= 9) {
        day = "0" + b[1];
    }

    else {
        day = b[1];
    }

    var FromDate = day + "-" + month + "-" + b[2];

    $.ajax({
        url: '/AHCMS/AppointmentSchedule/CreateAppointmentCancellation',
        dataType: "json",
        type: "POST",
        data: {
            PatientID: PatientID,
            Date: FromDate,
            AppointmentScheduleItemID: AppointmentItemID
        },
        success: function (response) {
            if (response.Status == "success") {
                if (response.Iscancel == true) {
                    app.show_notice("Appointment Cancelled successfully");

                    setTimeout(function () {
                        window.location = "/AHCMS/AppointmentSchedule/IndexV2";
                    }, 1000);
                }
                else {
                    app.show_notice("Appointment Cancellation Failed");
                }
            }
        }
    });
},
AppointmentSchedule.save= function () {
    var self = AppointmentSchedule;
    var data;
    index = $("#appointment-schedule-list tbody").length;
    $("#item-count").val(index);
    data = self.get_data();
    $.ajax({
        url: '/AHCMS/AppointmentSchedule/Save',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice(" Saved Successfully");
                window.location = "/AHCMS/AppointmentSchedule/Indexv2";
            }
            else {
                app.show_error('Failed to create Appointment Schedule');
            }
        }
    });
},

AppointmentSchedule.generate_barcode= function () {
    var self = AppointmentSchedule;
    var mainDiv = $("#barcode");
    var textDiv = ("#textdata");
    $("#barcode").empty();
    $("#textdata").empty();
    //var i = 0
    var content = "";
    var content1 = "";
    var classname;
    var classname1;
    var $content;
    var $content1;
    var barcodedata;
    var textdata;
    self.qrcodeitems.forEach((item) => {
        classname = 'generatebarcode' //+ i;
        barcodedata = item.PatientCode;  //+ "-" + item.ItemCode
        textdata = item.PatientCode;

        classname = 'generatebarcode' //+ i;


        content = '<table style="width:336px;height:192px;">'
                + '<tr><td colspan="2" style="height:40px;"></td></tr>'
                + '<tr><td colspan="2" style="height:0px;font-size:18px;font-weight:bold;padding:0px 0px 0px 0px;">TNH No.: ' + item.PatientCode + '</td></tr>'
                + '<tr><td colspan="2" style="height:0px;padding:4px 0px 0px 0px;font-size:15px;font-weight:bold;">Name: ' + item.PatientName + '</td></tr>'
                + '<tr><td  style="height:0px;font-size:12px;padding:10px 0px 0px 0px">Age: ' + item.Age + '</td><td  style="height:0px;font-size:12px;padding:10px 20px 0px 0px">Gender: ' + item.Gender + '</td></tr>'
                + '<tr><td colspan="2" style="height:0px;padding:10px 0px 0px 0px"><svg width="158px" height="72px" class="barcode_print ' + classname + '">' + '</svg></td></tr>'
                + '</table>';
        //  $('#barcode').append($content);
        $('#textdata').append(content);
        $('.' + classname).JsBarcode(barcodedata, {
            width: 1,
            height: 30,
            textmargin: 0,
            fontoptions: "bold",
            fontSize: 0,
            margin: 0,
            text: textdata.substring(0, 30),
            textAlign: "right",
            marginLeft: 0,
            marginTop: 0,
            marginBottom: 0,
            marginRight: 0,

            //format: "Code39",
            mod43: true
        });
        //}


    });
    self.print_bar_code();
    //$('#show-barcode').trigger('click');
}