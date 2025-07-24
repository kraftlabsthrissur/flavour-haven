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
                    window.location = "/AHCMS/AppointmentSchedule/IndexV3";
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
                        window.location = "/AHCMS/AppointmentSchedule/IndexV3";
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
                window.location = "/AHCMS/AppointmentSchedule/Indexv3";
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
    var PhId;
    var Image;
    self.qrcodeitems.forEach((item) => {
        classname = 'generatebarcode' //+ i;
        barcodedata = item.PatientCode;  //+ "-" + item.ItemCode
        textdata = item.PatientCode;
        Image = item.PhotoName;
        PhId = item.PhotoID;
        if (PhId == 0) { Image = "noimage.png"; }

        classname = 'generatebarcode' //+ i;


        content = '<table style="width:336px;height:192px;">'
                + '<tr><td colspan="2" style="height:40px;"></td></tr>'
                + '<tr><td colspan="2" style="height:0px;font-size:18px;font-weight:bold;padding:0px 0px 0px 0px;">Reg No.: ' + item.PatientCode + '</td></tr>'
                + '<tr><td colspan="2" style="height:0px;padding:4px 0px 0px 0px;font-size:15px;font-weight:bold;">Name: ' + item.PatientName + '</td>'
                + '<td rowspan="4"><img style="width:100px;height:100px" src="/Uploads/' + Image + '"></td></tr>'
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
},

AppointmentSchedule.get_item_for_barcode_generator= function (PatientID) {
    var self = AppointmentSchedule;

    $.ajax({
        url: "/AHCMS/AppointmentSchedule/GetPatientForBarCodeGeneratorWithImage",
        data: {
            ID: PatientID,
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                self.qrcodeitems.splice(0, self.qrcodeitems.length)
                $(response.Data).each(function (i, record) {
                    self.qrcodeitems.push({
                        //BatchID: record.BatchID, Batch: record.Batch, ItemName: record.ItemName, ItemCode: record.ItemCode,
                        //ItemID: record.ItemID, RetailMRP: record.RetailMRP
                        PatientID: record.PatientID, PatientName: record.Patient, PatientCode: record.PatientCode,
                        Age: record.Age, Gender: record.Gender, PhotoID: record.PhotoID, PhotoName: record.PhotoName,
                        PhotoPath: record.PhotoPath
                    });
                });
            }
            $('#show-barcode').trigger('click');
            self.generate_barcode();
            //}
        }
    });
}