RoomAllocation = {
    init: function () {
        var self = RoomAllocation;
        self.bind_events();
    },
    list: function () {
        var $list = $('#refered-to-ip-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/RoomAllocation/GetReferedToIPList"

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
                               + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                               + "<input type='hidden' class='ReservationID' value='" + row.ReservationID + "'>"
                               + "<input type='hidden' class='RoomStatusID' value='" + row.RoomStatusID + "'>"
                       }
                   },
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "ReferedDate", "className": "ReferedDate" },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnAdmit' >Admit</button>";
                         }
                     },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btncancel' >Cancel</button>";
                         }
                     },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    inpatient_list: function () {
        var $list = $('#inpatient-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/RoomAllocation/GetInPatientList"

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
                               + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                               + "<input type='hidden' class='ReservationID' value='" + row.ReservationID + "'>"
                               + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                               + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                               + "<input type='hidden' class='RoomStatusID' value='" + row.RoomStatusID + "'>"
                               
                       }
                   },
                  
                   { "data": "Patient", "className": "Patient" },
                   { "data": "RoomName", "className": "RoomName" },
                   { "data": "AdmissionDate", "className": "AdmissionDate" },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnChange' >Change</button>";
                         }
                     },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnBilling' >Billing</button>";
                         }
                     },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    bind_events: function () {
        var self = RoomAllocation;
        $("#RoomTypeID").on('change', self.GetRoom);
        $("#RoomID").on('change', self.GetRate);
        $(".btnSave").on('click', self.save_confirm);
        $("body").on('click', '.btnAdmit', self.allocate_room);
        $("body").on('click', '.btnChange', self.change_room);
        $("body").on('click', '.btnBilling', self.on_billing);
        $("#RoomID").on('change', self.GetRate);
        $('#AdmissionDate').on('change', self.get_room_type);
        $('#RoomChangeDate').on('change', self.get_room_type);
        $('#AdmissionDateTill').on('change', self.get_room_type);
    },
    GetRoom: function () {
        var self = RoomAllocation;
        var RoomTypeID = $(this);
        var IsRoomChange = $("#IsRoomChange").val();
        if (IsRoomChange == 'true')
        {
            var fromDate = $("#RoomChangeDate").val();
            var toDate = $("#AdmissionDateTill").val();
        }
        else
        {
            var fromDate = $("#AdmissionDate").val();
            var toDate = $("#AdmissionDateTill").val();
        }    
        var patientID = $("#PatientID").val();
        $.ajax({
            url: '/AHCMS/RoomAllocation/GetAvailableRoom/',
            dataType: "json",
            type: "GET",
            data: {
                ID: RoomTypeID.val(),
                FromDate: fromDate,
                ToDate: toDate,
                PatientID: patientID
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.RoomID + "'>" + record.RoomName + "</option>";
                });
                if (RoomTypeID.attr('id') == "RoomTypeID") {
                    $("#RoomID").html("");
                    $("#RoomID").append(html);
                } else {
                    $("#RoomID").html("");
                    $("#RoomID").append(html);
                }
            }
        });
    },

    GetRate: function () {
        var self = RoomAllocation;
        var RoomID = $(this);
        $.ajax({
            url: '/AHCMS/RoomAllocation/GetRoomDetailsByID/',
            dataType: "json",
            type: "GET",
            data: {
                ID: RoomID.val(),
            },
            success: function (response) {
                $("#Rate").val('');
                var html;
                $.each(response.data, function (i, record) {
                    clean($("#Rate").val(record.Rate))

                });
            }
        });
    },

    allocate_room: function () {
        //e.stopPropagation();
        var self = RoomAllocation;
        var AppointmentProcessID = $(this).closest('tr').find('.AppointmentProcessID').val();
        var ReservationID = $(this).closest('tr').find('.ReservationID').val();
        var RoomStatusID = $(this).closest('tr').find('.RoomStatusID').val();
        app.load_content("/AHCMS/RoomAllocation/Create/?AppointmentProcessID=" + AppointmentProcessID + "&ReservationID=" + ReservationID + "&RoomStatusID=" + RoomStatusID);
    },

    //change_room: function () {
    //    //e.stopPropagation();
    //    var self = RoomAllocation;
    //    var AppointmentProcessID = $(this).closest('tr').find('.AppointmentProcessID').val();
    //    var ReservationID = $(this).closest('tr').find('.ReservationID').val();
    //    var RoomStatusID = $(this).closest('tr').find('.RoomStatusID').val();
    //    var IsRoomChange = true;
    //    app.load_content("/AHCMS/RoomAllocation/Create/?AppointmentProcessID=" + AppointmentProcessID + "&ReservationID=" + ReservationID + "&RoomStatusID=" + RoomStatusID + "&IsRoomChange=" + IsRoomChange);
    //},
    change_room: function () {
        //e.stopPropagation();
        var self = RoomAllocation;
        var RoomStatusID = $(this).closest('tr').find('.RoomStatusID').val();
        app.load_content("/AHCMS/RoomChange/Create/?RoomStatusID=" + RoomStatusID);
    },

    on_billing: function () {
        //e.stopPropagation();
        var self = RoomAllocation;
        var IPID = $(this).closest('tr').find('.IPID').val();
        var PatientID = $(this).closest('tr').find('.PatientID').val();
        
        app.load_content("/Sales/ServiceSalesOrder/Create/?IPID=" + IPID);
    },

    save_confirm: function () {
        var self = RoomAllocation;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = RoomAllocation;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/AHCMS/RoomAllocation/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Room Allocated successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/RoomAllocation/ReferedToIPList";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },
    get_data: function () {
        var self = RoomAllocation;
        var model = {
            AppointmentProcessID: $("#AppointmentProcessID").val(),
            PatientID: $("#PatientID").val(),
            DoctorID: $("#DoctorID").val(),
            PatientName: $("#PatientName").val(),
            RoomTypeID: $("#RoomTypeID").val(),
            RoomID: $("#RoomID").val(),
            Rate: clean($("#Rate").val()), 
            AdmissionDate: $("#AdmissionDate").val(),
            AdmissionDateTill: $("#AdmissionDateTill").val(),
            RoomChangeDate: $("#RoomChangeDate").val(),
            IsRoomChange:$("#IsRoomChange").val(),
            ReservationID: $("#ReservationID").val(),
            Bystander: $("#ByStander").val(),
            MobileNumber: $("#MobileNumber").val(),
            RoomStatusID: $("#RoomStatusID").val(),

        }
        return model;
    },

    validate_form: function () {
        var self = RoomAllocation;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [
             {
                 elements: "#PatientName",
                 rules: [
                     { type: form.required, message: "Patient Name is Required" },
                 ]
             },

             {
                 elements: "#RoomTypeID",
                 rules: [
                     { type: form.required, message: "Select Room Type" },
                 ]
             },
             {
                 elements: "#RoomID",
                 rules: [
                     { type: form.required, message: "Select Room" },
                 ]
             },
              {
                  elements: "#AdmissionDate",
                  rules: [
                      { type: form.required, message: "Admission Date  is Required" },
                  ]
              },
               {
                   elements: "#AdmissionDateTill",
                   rules: [
                       { type: form.required, message: "Admission Date Till  is Required" },
                   ]
               },
        ]
    },

    get_room_type: function () {
        var self = RoomAllocation;
        $.ajax({
            url: '/AHCMS/RoomAllocation/GetRoomType/',
            dataType: "json",
            type: "GET",
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.RoomType + "</option>";
                });
                $("#RoomTypeID").html("");
                $("#RoomTypeID").append(html);
            }
        });
    },

}