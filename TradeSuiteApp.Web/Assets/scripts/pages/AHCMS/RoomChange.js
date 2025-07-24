RoomChange = {
    init: function () {
        var self = RoomChange;
        self.bind_events();
        self.get_allocated_room_details();
    },
    bind_events: function () {
        var self = RoomChange;
        $("#RoomTypeID").on('change', self.GetRoom);
        $("#RoomID").on('change', self.GetRate);
        $("#RoomID").on('change', self.GetRate);
        $('#AdmissionDate').on('change', self.get_room_type);
        $('#RoomChangeDate').on('change', self.get_room_type);
        $('#AdmissionDateTill').on('change', self.get_room_type);
        $("body").on("click", "#btnAddItems", self.add_item);
        $(".btnSave").on('click', self.save_confirm);
    },
    get_allocated_room_details: function () {
        var self = RoomChange;
        var RoomStatusID = $('#RoomStatusID').val();
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/RoomChange/RoomAllocationDetails",
            data: {
                IPID: IPID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#room-allocation-details-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#room-allocation-details-list").append($response);

            }
        });
    },
    GetRoom: function () {
        var self = RoomChange;
        $("#Rate").val("");
        var RoomTypeID = $(this);
        var IsRoomChange = $("#IsRoomChange").val();
        if (IsRoomChange == 'true') {
            var fromDate = $("#RoomChangeDate").val();
            var toDate = $("#AdmissionDateTill").val();
        }
        else {
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
        var self = RoomChange;
        var RoomID = $("#RoomID").val();
        $.ajax({
            url: '/AHCMS/RoomAllocation/GetRoomDetailsByID/',
            dataType: "json",
            type: "GET",
            data: {
                ID: RoomID
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
    get_room_type: function () {
        var self = RoomChange;
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
    add_item: function () {
        var self = RoomChange;
        $('#room-allocation-details-list').addClass("Added");
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        sino = $('#room-allocation-details-list tbody tr').length + 1;
        var StartDate = $("#RoomChangeDate").val();
        var ToDate = $("#AdmissionDateTill").val();
        var RoomType = $("#RoomTypeID option:selected").text();
        var RoomTypeID = $("#RoomTypeID").val();
        var Room = $("#RoomID option:selected").text();
        var RoomID = $("#RoomID").val();
        var Rate = $("#Rate").val();
        var DoctorID = $("#DoctorID").val();
        var PatientID = $("#PatientID").val();
        var RoomStatusID = $("#RoomStatusID").val();

        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="sl-no uk-text-center">' + sino
            + '<input type="hidden" class="DoctorID "value="' + DoctorID + '"/>'
            + '<input type="hidden" class="PatientID "value="' + PatientID + '"/>'
            + '<input type="hidden" class="RoomStatusID "value="' + RoomStatusID + '"/>'
            + '</td>'
            + '<td class="StartDate" name = "Items[' + (sino - 1) + '][StartDate]">' + StartDate
            + '</td>'
            + '<td class="ToDate">' + ToDate
            + '</td>'
            + '<td class="RoomType">' + RoomType
            + '<input type="hidden" class="RoomTypeID "value="' + RoomTypeID + '"/>'
            + '</td>'
            + '<td class="Room">' + Room
            + '<input type="hidden" class="RoomID "value="' + RoomID + '"/>'
            + '</td>'
            + '<td class="Rate mask-positive-currency">' + Rate
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#room-allocation-details-list tbody').append($content);
        //self.clear_data();
        //self.count();
    },

    save: function () {
        var self = RoomChange;
        var data;
        //index = $("#room-allocation-details-list tbody tr:last td ").length;
        //$("#item-count").val(index);
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/RoomChange/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/RoomAllocation/InPatientList";
                    }, 1000);
                }
                else {
                    app.show_error('Room Change Attempt is Failed');
                }
            }
        });
    },

    get_data: function () {
        var self = RoomChange;
        var data = {};
        data.RoomItems = [];
        var item = {};
        $('#room-allocation-details-list tr:last').each(function () {
            item = {};
            item.FromDate = $(this).find(".StartDate").text();
            item.ToDate = $(this).find(".ToDate").text();
            item.RoomID = $(this).find(".RoomID").val();
            item.RoomTypeID = $(this).find(".RoomTypeID").val();
            item.Rate = $(this).find(".Rate").val();
            item.PatientID = $(this).find(".PatientID").val();
            item.DoctorID = $(this).find(".DoctorID").val();
            item.RoomStatusID = $(this).find(".RoomStatusID").val();
            data.RoomItems.push(item);
        });
        data.ID = [];
        data.ID = self.deleted;
        // data = $("#TransactionForm").serialize();

        return data;
    },
    validate_add: function () {
        var self = RoomChange;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    save_confirm: function () {
        var self = RoomChange;
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
    validate_save: function () {
        var self = RoomChange;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    rules: {
        on_save: [
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter Name" },
                ]
            },

            //{
            //    elements: "#AddressLine1",
            //    rules: [
            //        { type: form.required, message: "Please enter Address" },
            //    ]
            //},

            //{
            //    elements: "#StateID",
            //    rules: [
            //        { type: form.required, message: "Please choose State" },
            //    ]
            //},
             {
                 elements: "#MobileNumber",
                 rules: [
                     { type: form.required, message: "Please enter MobileNumber" },
                 ]
             },
             {
                 elements: "#Gender",
                 rules: [
                     { type: form.required, message: "Please enter Gender" },
                 ]
             },
               //{
               //    elements: "#GSTNo",
               //    rules: [
               //        {
               //            type: function (element) {
               //                var gstno = $("#GSTNo").val().length;
               //                if (gstno > 0 && gstno < 15) {
               //                    return false;
               //                }
               //                else {
               //                    return true;
               //                }
               //            }, message: "GSTNo Contain Minimum 15 Characters"
               //        },
               //    ]
               //},
        ],

        on_add: [
              {
                  elements: "#RoomChangeDate",
                  rules: [
                      { type: form.required, message: "RoomChangeDate Is Required" },
                  ]
              },
              {
                  elements: "#AdmissionDateTill",
                  rules: [
                      { type: form.required, message: "AdmissionDateTill Is Required" },
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
                    { type: form.required, message: "Please choose Room" },
                    { type: form.non_zero, message: "Please choose  Room" },
                    {
                        type: function (element) {
                            var error = false;
                            $("#room-allocation-details-list tbody tr").each(function () {
                                if ($(this).find(".RoomID").val().trim() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Can't change to same Room"
                    },
                ]
            },
        ],
        on_save: [
         {
             elements: "#room-allocation-details-list",
             rules: [
                // { type: form.non_zero, message: "Please add atleast one item" },
                //{ type: form.required, message: "Please add atleast one item" },
                {
                    type: function (element) {
                        var error = false;
                        $("#room-allocation-details-list").each(function () {
                            if ($(this).hasClass("Added")) {
                                error = true;
                            }
                        });
                        return error;
                    }, message: "Please add atleast one item"
                },
             ]
         },
         ]
    },

}