RoomReservation = {
    init:function()
    {
        InternationalPatient.change_country();
        var self = RoomReservation;
        self.bind_events();

        InternationalPatient.Patientlist();
        $('#patient-list').SelectTable({
            selectFunction: self.select_patient,
            modal: "#select-patient",
            initiatingElement: "#PartyName"
        });
       
    },

    list: function () {
        var $list = $('#room_reservation_list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/RoomReservation/GetRoomReservationList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
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
                   { "data": "FromDate", "className": "FromDate" },
                   { "data": "ToDate", "className": "ToDate" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Room", "className": "Room" }

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/AHCMS/RoomReservation/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = RoomReservation;
        $("#CountryID").on('change', InternationalPatient.change_country);
        $("#CountryID").on('change', InternationalPatient.Get_State);
        $("#DOB").on('change', InternationalPatient.age_calculation);
        UIkit.uploadSelect($("#select-photo"), InternationalPatient.selected_photo_settings);
        UIkit.uploadSelect($("#select-passport"), InternationalPatient.selected_passport_settings);
        UIkit.uploadSelect($("#select-visa"), InternationalPatient.selected_visa_settings);
        $('body').on('click', 'a.remove-quotation', InternationalPatient.remove_photo);
        $('body').on('click', 'a.remove-passport', InternationalPatient.remove_passport);
        $('body').on('click', 'a.remove-visa', InternationalPatient.remove_visa);
        $("#StateID").on('change', self.GetDistrict);
        $("#btnAddPatient").on("click", self.show_add_patient);
        $("#btnOkAddPatient").on("click", self.select_patient);
        $("#btnOkSavePatient").on("click", self.save_patient);
        $("#RoomTypeID").on('change', self.GetRoom);
        $("#RoomID").on('change', self.GetRate);
        $(".btnSave").on('click', self.save_confirm);
        $('#FromDate').on('change', self.get_room_type);
        $('#ToDate').on('change', self.get_room_type);
    },
    GetRoom: function () {
        var self = RoomReservation;
        var RoomTypeID = $(this);
        var fromDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();
        var patientID = $("#PatientID").val();
        $.ajax({
            url: '/AHCMS/RoomReservation/GetAvailableRoom/',
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
        var self = RoomReservation;
        var RoomID = $(this);
        $.ajax({
            url: '/AHCMS/RoomReservation/GetRoomDetailsByID/',
            dataType: "json",
            type: "GET",
            data: {
                ID: RoomID.val(),
            },
            success: function (response) {
                $("#Rate").val('');
                var html;
                $.each(response.data, function (i, record) {
                    $("#Rate").val(record.Rate)

                });
            }
        });
    },

    show_add_patient: function () {
        var self = RoomReservation;
        self.patient_clear();
        $('#show-add-patient').trigger('click');
    },
    patient_clear: function () {
        var self = RoomReservation;
        $("#Name").val('');
        $("#AddressLine1").val('');
        $("#AddressLine2").val('');
        $("#PinCode").val('');
        $("#Email").val('');
        $("#MobileNumber").val('');
        $("#DOB").val('');
        $("#StateID").val('');
        $("#DistrictID").val('');
        $("#OccupationID").val('');
        $("#GuardianName").val('');
        $("#Gender").val('');
        $("#MartialStatus").val('');
        $("#CountryID").val(1);
        $("#PinCode").val('');
        $("#BloodGroupID").val('');
        $("#Age").val('');
        $("#PatientReferedByID").val('');
        $("#ReferalContactNo").val('');
        $("#OccupationID").val('');
        $("#MobileNumber").val('');
        $("#Email").val('');
        $("#select-photo").val('');
        $("#PurposeOfVisit").val('');
        $("#PassportNo").val('');
        $("#PlaceOfIssue").val('');
        $("#DateOfIssuePassport").val('');
        $("#DateOfExpiry").val('');
        $("#EmployedIn").val('');
        $("#VisaNo").val('');
        $("#DateOfIssueVisa").val('');
        $("#DateOfExpiryVisa").val('');
        $("#ArrivedFrom").val('');
        $("#ProceedingTo").val('');
        $("#DurationOfStay").val('');
        $("#DateOfArrival").val('');
        $("#select-passport").val('');
        $("#select-visa").val('');

    },
    select_patient: function () {
        var self = RoomReservation;
        var radio = $('#select-patient tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#PatientName").val(Name);
        $("#PatientID").val(ID);
        UIkit.modal($('#select-patient')).hide();
    },
    GetDistrict: function () {
        var self = RoomReservation;
        var state = $(this);
        $.ajax({
            url: '/Masters/District/GetDistrict/',
            dataType: "json",
            type: "GET",
            data: {
                StateID: state.val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                if (state.attr('id') == "State") {
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                } else {
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                }
            }
        });
    },

    save_patient: function () {
        var self = RoomReservation;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_patient_data();

        var name = (data.Name);
        var id = (data.ID);
        $.ajax({
            url: '/Masters/InternationalPatient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    $("#PatientName").val(name);
                    $("#PatientID").val(response.data)
                    $('#add-patient').trigger('click');
                }
                else {
                    app.show_error("Already Exists.");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });

    },
    get_patient_data: function () {
        var self = RoomReservation;
        var data = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            GuardianName: $("#GuardianName").val(),
            AddressLine1: $("#AddressLine1").val(),
            AddressLine2: $("#AddressLine2").val(),
            StateID: $("#StateID option:selected").val(),
            DistrictID: $("#DistrictID option:selected").val(),
            CountryID: $("#CountryID option:selected").val(),
            MobileNumber: $("#MobileNumber").val(),
            DOB: $("#DOB").val(),
            Age: $("#Age").val(),
            Email: $("#Email").val(),
            PinCode: $("#PinCode").val(),
            Gender: $("#Gender option:selected").text(),
            MartialStatus: $("#MartialStatus option:selected").text(),
            BloodGroup: $("#BloodGroupID option:selected").text(),
            OccupationID: $("#OccupationID option:selected").val(),
            PatientReferedByID: $("#PatientReferedByID option:selected").val(),
            ReferalContactNo: $("#ReferalContactNo").val(),
            DateOfArrival: $("#DateOfArrival").val(),
            PurposeOfVisit: $("#PurposeOfVisit").val(),
            PassportNo: $("#PassportNo").val(),
            PlaceOfIssue: $("#PlaceOfIssue").val(),
            DateOfIssuePassport: $("#DateOfIssuePassport").val(),
            DateOfExpiry: $("#DateOfExpiry").val(),
            VisaNo: $("#VisaNo").val(),
            DateOfIssueVisa: $("#DateOfIssueVisa").val(),
            DateOfExpiryVisa: $("#DateOfExpiryVisa").val(),
            ArrivedFrom: $("#ArrivedFrom").val(),
            ProceedingTo: $("#ProceedingTo").val(),
            DurationOfStay: $("#DurationOfStay").val(),
            PhotoID: $('#selected-photo .view-file').data('id'),
            PassportID: $('#selected-passport .view-file').data('id'),
            VisaID: $('#selected-visa .view-file').data('id'),
            EmployedIn: $("#EmployedIn option:selected").text(),
            DoctorID: $("#DoctorID").val(),
        };
        return data;
    },
    save_confirm: function () {
        var self = RoomReservation;
        self.error_count = 0;
        self.error_count = self.validate_view();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save: function () {
        var self = RoomReservation;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/AHCMS/RoomReservation/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Room Reserved successfully");
                    setTimeout(function () {
                        window.location = "/AHCMS/RoomReservation/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },
    get_data: function () {
        var self = RoomReservation;
        var model = {
            ID:$("#ID").val(),
            Date: $("#Date").val(),
            PatientID: $("#PatientID").val(),
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            RoomTypeID: $("#RoomTypeID").val(),
            RoomID: $("#RoomID").val(),
            Rate: $("#Rate").val()
        }
        return model;
    },
    validate_view: function () {
        var self = RoomReservation;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_form: function () {
        var self = RoomReservation;
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

            {
                elements: "#Gender",
                rules: [
                    { type: form.required, message: "Please choose Gender" },
                ]
            },
             {
                 elements: "#MobileNumber",
                 rules: [
                     { type: form.required, message: "Please enter MobileNumber" },
                 ]
             },
             //{
             //    elements: "#DOB",
             //    rules: [
             //        { type: form.required, message: "Please enter DOB" },
             //    ]
             //},
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
                 elements: "#FromDate",
                 rules: [
                     { type: form.required, message: "FromDate  is Required" },
                 ]
             },
              {
                  elements: "#ToDate",
                  rules: [
                      { type: form.required, message: "ToDate  is Required" },
                  ]
              },
        ]
    },

    get_room_type: function () {
        var self = RoomReservation;
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