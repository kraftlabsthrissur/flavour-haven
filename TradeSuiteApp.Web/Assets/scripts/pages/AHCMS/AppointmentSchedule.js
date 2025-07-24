var AppointmentSchedule = {

    init: function () {
        var self = AppointmentSchedule;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });

        InternationalPatient.Patientlist();
        $('#patient-list').SelectTable({
            selectFunction: self.select_patient,
            returnFocus: "#PatientName",
            startFocusIndex: 2,
            modal: "#select-patient",
            initiatingElement: "#PatientName",

        });
        InternationalPatient.change_country();
        InternationalPatient.change_discount();
        self.bind_events();
        self.list();
        self.load_department_dropDown();
    },
    bind_events: function () {
        var self = AppointmentSchedule;
        $("#CountryID").on('change', InternationalPatient.change_country);
        $("#TransactionForm").on("submit", function () { return false; });
        $(".btnHistory").on("click", InternationalPatient.patient_history);
        $("#btnOKEmployee").on("click", self.select_employee);
        $("#btnOkAddPatient").on("click", self.select_patient);
        $("body").on("click", "#btnAddItems", self.is_appointmentProcessed);
        $("#btnAddPatient").on("click", self.show_add_patient);
        $("#StateID").on('change', self.GetDistrict);
        $("#btnOkSavePatient").on("click", self.save_patient);
        $("#btn-save-edit").on("click", self.update_appointment);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on('click', '#liPrevious', self.get_previous_dates);
        $("#CountryID").on('change', InternationalPatient.Get_State);
        $("#DOB").on('change', InternationalPatient.age_calculation);
        UIkit.uploadSelect($("#select-photo"), InternationalPatient.selected_photo_settings);
        UIkit.uploadSelect($("#select-passport"), InternationalPatient.selected_passport_settings);
        UIkit.uploadSelect($("#select-visa"), InternationalPatient.selected_visa_settings);
        $("body").on("click", ".remove-item", self.delete_item);
        $('body').on('click', 'a.remove-quotation', InternationalPatient.remove_photo);
        $('body').on('click', 'a.remove-passport', InternationalPatient.remove_passport);
        $('body').on('click', 'a.remove-visa', InternationalPatient.remove_visa);
        $("body").on("change", "#FromDateStringEdit", self.get_appointment_item);
        $("body").on('keyup', "#FromDateStringEdit", self.get_appointment_item);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $.UIkit.autocomplete($('#doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor_in_edit);
        $.UIkit.autocomplete($('#patient-autocomplete'), Config.patient);
        $('#patient-autocomplete').on('selectitem.uk.autocomplete', self.set_patient);
        $("body").on("click", ".btnbilling", self.appointment_confirm);
        $("body").on("click", ".btnedit", self.appointment_edit);
        $("body").on("click", "#ConsultationMode", self.set_netamount);
        $('body').on('change', "#PaymentModeID", self.get_bank_name);
        $(document).on('keyup', '.rate', self.set_entered_rate)
        $("body").on("click", "#btnSaveAppointment", self.save_appointment_confirm);
        $("body").on("click", ".print", self.print_Appointment_booking);
        $("body").on("click", "#btnprint", self.print_Appointment);

        // $("#btnSaveAppointment").on('click', self.save_confirm);
        $("body").on("click", ".cancel_appointment", self.appointment_cancel);
        $("body").on("click", ".patientcard", self.get_patient_details);
        $("body").on('click', "#btn_print_code", self.get_barcode);
        $("#saveAndconfirm").on("click", self.is_appointmentProcessed_already);


        //$("#btn_close").on("click", self.appointment_cancel);
        //$("body").on("click", "#btn_close", self.delete_appointment);
        self.load_department_dropDown();


        //DiscountCategory
        $("#DiscountTypeID").on('change', InternationalPatient.change_discount);
        $("body").on("ifChanged", ".check-box", InternationalPatient.check);
        $("body").on("change", ".DiscountCategoryID", InternationalPatient.set_discount);
        $('body').on('change', "#SlotName", self.get_schedule_time);
        $('body').on('change', "#ConsultationTime", self.get_consultation_time);
    },
    set_netamount: function () {
        var self = AppointmentSchedule;
        var TotalAmount = 0;
        $('#consultation-list tbody tr').each(function () {
            var Rate = clean($(this).find('.rate').val());
            var orgRate = clean($(this).find('.rate').data('org-rate'));
            if ($('#ConsultationMode :selected').text() == "Free") {
                clean($(this).find(".rate").val(0.00));
                clean($(this).find(".netamount").val(0.00));
                $("#NetAmount").val(0.00);
            }
            else {
                TotalAmount = TotalAmount + orgRate;
                $(this).find(".rate").val(orgRate);
                $(this).find(".netamount").val(orgRate);
                $("#NetAmount").val(TotalAmount);
            }
            //hiding PaymentMode and Bank details while ConsultationMode is credit..
            if ($('#ConsultationMode :selected').text() == "Credit") {
                $("#PaymentMode").hide();
                $("#Bank").hide();
            }
            else {
                $("#PaymentMode").show();
                $("#Bank").show();
            }

        });
    },
    set_entered_rate: function (release) {
        var self = AppointmentSchedule;
        var sum = 0;
        $('#consultation-list tbody tr').each(function () {
            sum += clean($(this).find(".rate").val());
            var Rate = $(this).find(".rate").val();
            $(this).find(".netamount").val(Rate);
            $(this).find(".rate").data('org-rate', Rate);
            $("#NetAmount").val(sum)
        });

    },
    get_bank_name: function () {
        var self = AppointmentSchedule;
        var mode;
        var Module = "Receipt"
        if ($("#PaymentModeID option:selected").text() == "Select") {
            mode = "";
            $("#Date").val('');
        }
        else if ($("#PaymentModeID option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
            var date = new Date();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var current_date = (day < 10 ? '0' : '') + day + '/' +
               (month < 10 ? '0' : '') + month + '/' +
               date.getFullYear();
            $("#Date").val(current_date);
        }
        else {
            mode = "Bank"
        }

        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                ModuleName: Module,
                Mode: mode
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#BankID").html("");
                var html = "";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });

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
    set_patient: function (event, item) {
        var self = AppointmentSchedule;
        $("#PatientID").val(item.id),
        $("#PatientName").val(item.value);
        $("#btnAddItems").focus();
    },
    set_doctor: function (event, item) {
        var self = AppointmentSchedule;
        $("#DoctorID").val(item.id),
        $("#DoctorName").val(item.name);
        UIkit.modal($('#select-employee')).hide();
        self.get_appointment();
        self.get_doctor_consultation_Time();
        $("#PatientName").focus();
    },
    set_doctor_in_edit: function (event, item) {
        var self = AppointmentSchedule;
        $("#DoctorID").val(item.id),
        $("#DoctorName").val(item.name);        
    },
    count: function () {
        index = $("#appointment-schedule-list tbody").length;
        $("#item-count").val(index);
    },
    GetDistrict: function () {
        var self = AppointmentSchedule;
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
    get_patient_data: function () {
        var self = AppointmentSchedule;
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
            Place: $("#Place").val(),
            Month: $("#Month").val(),
            PatientReferedBy: $("#PatientReferedByID option:selected").text(),
            DiscountTypeID: $("#DiscountTypeID").val(),
            DiscountStartDate: $("#DiscountStartDate").val(),
            DiscountEndDate: $("#DiscountEndDate").val(),
            MaxDisccountAmount: clean($("#MaxDisccountAmount").val())
        };
        data.DiscountDetails = [];
        var item = {};
        if (data.DiscountTypeID > 0) {
            $('#tbl-discount-list tbody .included').each(function () {
                item = {};
                item.BusinessCategoryID = $(this).find(".BusinessCategoryID").val();
                item.DiscountCategoryID = $(this).find(".DiscountCategoryID").val();
                item.DiscountPercentage = $(this).find(".DiscountPercentage").val();
                data.DiscountDetails.push(item);
            });
        }
        return data;
    },
    save_patient: function () {
        var self = AppointmentSchedule;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_patient_data();

        var name = (data.Name);
        var ReferenceThroughID = (data.PatientReferedByID);
        var PatientReferedBy = (data.PatientReferedBy);//
        $.ajax({
            url: '/Masters/InternationalPatient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#PatientID").val(response.data)
                    $("#PatientName").val(name);
                    $("#ReferenceThroughID").val(ReferenceThroughID);
                    $("#PatientReferencedBy").val(PatientReferedBy);//storing patient refered data of a currently saved patient
                    app.show_notice("Saved Successfully");
                    UIkit.modal($('#add-patient')).hide();
                }
                else {
                    app.show_error("Already Exists.");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                    UIkit.modal($('#add-patient')).hide();
                }
            }
        });

    },
    update_appointment:function (){
        var self = AppointmentSchedule;
        var model;
        model = self.get_updated_data();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/Update',
            data: model,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "Success") {
                    app.show_notice(response.Message);
                    setTimeout(function () {
                        window.location = "/AHCMS/AppointmentSchedule/Index";
                    }, 1000);
                } else {
                    app.show_error(response.Message);
                   
                }
            },
        });
    },
    get_updated_data:function(){
        var self = AppointmentSchedule;
        var data = {};
        data.DoctorID = $("#DoctorID").val(),
        data.PatientID = $("#PatientID").val();
        data.AppointmentScheduleItemID = $("#AppointmentScheduleItemID").val();
        data.DepartmentID = $("#DepartmentID").val();
        data.AppointmentDate = $("#AppointmentDate").val();
        return data
    },
    select_employee: function () {
        var self = AppointmentSchedule;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-employee')).hide();
        self.get_appointment();
        self.get_doctor_consultation_Time();
        $("#PatientName").focus();
    },

    select_patient: function () {
        var self = AppointmentSchedule;
        var radio = $('#select-patient tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var PatientReferedByID = $(row).find(".ReferenceID").val();
        var PatientReferedBy = $(row).find(".Reference").val();
        var ConsultingMode = $(row).find(".ConsultationMode").val();
        $("#PatientName").val(Name);
        $("#PatientID").val(ID);
        $("#ReferenceThroughID").val(PatientReferedByID);
        $("#ReferenceThrough").val(PatientReferedBy);//storing patientrefered of a already saved patient
        $("#ConsultingMode").val(ConsultingMode);
        UIkit.modal($('#select-patient')).hide();
        $("#btnAddItems").focus();
    },

    //set_doctor: function (event, item) {
    //    var self = AppointmentSchedule;
    //    $("#DoctorName").val(item.Name);
    //    $("#DoctorID").val(item.id);
    //},

    is_appointmentProcessed: function () {
        var self = AppointmentSchedule;
        $("#saveAndconfirm").hide();
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        var PatientReferedBy = $("#ReferenceThrough").val();
        var ConsultationMode = $("#ConsultingMode").val();
        var PatientRefered = $("#PatientReferencedBy").val();
        //if (PatientReferedBy != 'General' || PatientRefered != 'General') {
        //    app.confirm("Patient is Referenced By " + PatientReferedBy+PatientRefered+".. Do you Want To Add", function () {
        //        self.add_appointment();
        //    })
        //}
        //else {
        //    self.add_appointment();
        //}
        self.add_appointment();
    },
    add_appointment: function () {
        var self = AppointmentSchedule;
        var DoctorID = $("#DoctorID").val();
        var PatientID = $("#PatientID").val();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/IsAppointmentProcessed',
            data: {
                DoctorID: DoctorID,
                PatientID: PatientID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.IsAppointmentProcessed == true) {
                        self.add_item();
                    }
                    else {
                        app.show_error('patient already in consultation');
                    }
                }
            }
        });
    },
    add_item: function () {
        var self = AppointmentSchedule;
        sino = $('#appointment-schedule-list tbody tr').length + 1;
        var PatientName = $("#PatientName").val();
        var PatientID = $("#PatientID").val();
        var Time = $("#Time").val();
        var TokenNo = $("#TokenNo").val();
        var DepartmentName = $("#DepartmentID option:selected").text();
        var DepartmentID = $("#DepartmentID option:selected").val();
        var IsAllowConsultationSchedule = $("#IsAllowConsultationSchedule").val();
        var Department = '<select class="md-input label-fixed DepartmentID">' + AppointmentSchedule.Build_Select(DepartmentList, DepartmentName) + '</select>'
        var content = "";
        var $content;
        if (IsAllowConsultationSchedule == 0) {
            content = '<tr>'
                + '<td class="sl-no uk-text-center">' + sino + '</td>'
                + '<td class="PatientName" name = "Items[' + (sino - 1) + '][PatientName]">' + PatientName
                + '<input type="hidden" class = "PatientID" name = "Items[' + (sino - 1) + '][PatientID]" value="' + PatientID + '" />'
                + '</td>'
                + '<td>' + '<input type="text" value=" ' + Time + '" class="md-input Time label-fixed time15" /> ' + '</td>'
                + '<td>' + '<input type="text" value=" ' + TokenNo + '" class="md-input TokenNo"  /> ' + '</td>'
                + '<td>' + Department + '</td>'
                + '<td>'
                + '<a class="remove-item">'
                + '<i class="uk-icon-remove added"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';
        }
        else {
            content = '<tr>'
                + '<td class="sl-no uk-text-center">' + sino + '</td>'
                + '<td class="PatientName" name = "Items[' + (sino - 1) + '][PatientName]">' + PatientName
                + '<input type="hidden" class = "PatientID" name = "Items[' + (sino - 1) + '][PatientID]" value="' + PatientID + '" />'
                + '</td>'
                + '<td>' + '<input type="text" value=" ' + Time + '" class="md-input Time label-fixed"  disabled/> ' + '</td>'
                + '<td>' + '<input type="text" value=" ' + TokenNo + '" class="md-input TokenNo"  /> ' + '</td>'
                + '<td>' + Department + '</td>'
                + '<td>'
                + '<a class="remove-item">'
                + '<i class="uk-icon-remove added"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';
        }
        $content = $(content);
        app.format($content);
        $('#appointment-schedule-list tbody').append($content);
        self.clear_data();
        self.count();
    },



    load_department_dropDown: function () {
        $.ajax({
            url: '/Masters/Department/GetPatientDepartment',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    DepartmentList = response.data;
                }
            }
        });
    },

    Build_Select: function (options, selected_text) {
        var $select = '';
        var $select = $('<select> </select>');
        var $option = '';
        if (typeof selected_text == "undefined") {
            selected_text = "Select";
        }
        $option = '<option value="0">Select</option>';
        //$select.append($option);
        for (var i = 0; i < options.length; i++) {
            $option = '<option ' + ((selected_text == options[i].Name) ? 'selected="selected"' : '') + ' value="' + options[i].ID + '">' + options[i].Name + '</option>';

            $select.append($option);
        }
        return $select.html();

    },


    show_add_patient: function () {
        var self = AppointmentSchedule;
        self.patient_clear();
        $('#show-add-patient').trigger('click');
    },



    patient_clear: function () {
        var self = AppointmentSchedule;
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
        $("#Place").val('');
        $("#Month").val('');

    },

    clear_data: function () {
        var self = AppointmentSchedule;
        $("#PatientName").val('');
        $("#Time").val('');
        $("#TokenNo").val('');
        $("#ReferenceThroughID").val('');
        $("#PatientReferencedBy").val('');
        $("#ReferenceThrough").val('');
        $("#DepartmentID").val('');
    },

    save_confirm: function () {
        var self = AppointmentSchedule;
        var count = $('.added').length;
        $("#count").val(count);
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

    save: function () {
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
                    window.location = "/AHCMS/AppointmentSchedule/Index";
                }
                else {
                    app.show_error('Failed to create Appointment Schedule');
                }
            }
        });
    },

    get_data: function () {
        var self = AppointmentSchedule;
        var data = {};
        data.DoctorID = $("#DoctorID").val(),
        data.FromDateString = $("#FromDateStringEdit").val();
        data.Items = [];
        var item = {};
        $('#appointment-schedule-list tbody tr').each(function () {
            item = {};
            item.PatientID = $(this).find(".PatientID").val();
            item.Time = $(this).find(".Time").val();
            item.TokenNo = $(this).find(".TokenNo").val();
            item.AppointmentScheduleItemID = $(this).find(".AppointmentScheduleItemID").val();
            item.AppointmentProcessID = $(this).find(".AppointmentProcessID").val();
            item.DepartmentID = $(this).find(".DepartmentID option:selected").val();
            data.Items.push(item);
        });
        data.ID = [];
        data.ID = self.deleted;
        // data = $("#TransactionForm").serialize();

        return data;
    },

    validate_form: function () {
        var self = AppointmentSchedule;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_add: function () {
        var self = AppointmentSchedule;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_view: function () {
        var self = AppointmentSchedule;
        if (self.rules.on_view.length > 0) {
            return form.validate(self.rules.on_view);
        }
        return 0;
    },

    validate_save: function () {
        var self = AppointmentSchedule;
        if (self.rules.on_save_appointment.length > 0) {
            return form.validate(self.rules.on_save_appointment);
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

             {
                 elements: "#MobileNumber",
                 rules: [
                     { type: form.required, message: "Please enter MobileNumber" },
                 ]
             },
              //{
              //    elements: "#PatientReferedByID",
              //    rules: [
              //        { type: form.required, message: "Select Patient ReferenceThrough" },
              //    ]
              //},
             {
                 elements: "#Gender",
                 rules: [
                     { type: form.required, message: "Please enter Gender" },
                 ]
             },
              //{
              //    elements: "#Place",
              //    rules: [
              //        { type: form.required, message: "Please enter Place" },
              //    ]
              //},
               {
                   elements: "#Age",
                   rules: [
                        {
                            type: function (element) {
                                var error = false;
                                if ($("#Month").val() <= 0 && $("#Age").val() == 0) {
                                    error = true;
                                }
                                return !error;
                            }, message: "Age is Required"
                        },
                        //{
                        //    type: function (element) {
                        //        var error = false;
                        //        if ($("#Age").val() > 0) {
                        //            return false;
                        //        }

                        //    }
                        //},
                       //{ type: form.required, message: "Please enter a Month" },
                       //{ type: form.non_zero, message: "Please enter a Month" },
                       {
                           type: function (element) {
                               var error = false;
                               if ($("#Month").val() > 12) {
                                   error = true;
                               }
                               return !error;
                           }, message: "Month is Invalid"
                       },
                       {
                           type: function (element) {
                               var error = false;
                               if ($("#Month").val() < 0) {
                                   error = true;
                               }
                               return !error;
                           }, message: "Month is Invalid"
                       },
                   ]
               },
        ],

        on_add: [

             {
                 elements: "#DepartmentID",
                 rules: [
                     { type: form.required, message: "Please choose a Department" },
                     { type: form.non_zero, message: "Please choose a Department" },
                 ]
             },

            {
                elements: "#PatientID",
                rules: [
                    { type: form.required, message: "Please choose a Patient" },
                    { type: form.non_zero, message: "Please choose a Patient" },
                    {
                        type: function (element) {
                            var error = false;
                            $("#appointment-schedule-list tbody tr").each(function () {
                                if ($(this).find(".PatientID").val().trim() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Patient already exists"
                    },
                ]
            },

              {
                  elements: "#Time",
                  rules: [
                      { type: form.required, message: "Please enter a Time" },
                      { type: form.non_zero, message: "Please enter a Time" },
                      {
                          type: function (element) {
                              var error = false;
                              $("#appointment-schedule-list tbody tr").each(function () {
                                  if ($(this).find(".Time").val().trim() == $(element).val()) {
                                      error = true;
                                  }
                              });
                              return !error;
                          }, message: "Time already exists"
                      },
                  ]
              },

            {
                elements: "#TokenNo",
                rules: [
                    { type: form.required, message: "Please enter a Token No" },
                    { type: form.non_zero, message: "Please enter a Token No" },
                    {
                        type: function (element) {
                            var error = false;
                            $("#appointment-schedule-list tbody tr").each(function () {
                                if ($(this).find(".TokenNo").val().trim() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Token No already exists"
                    },
                ]
            },
        ],

        on_view: [
           {
               elements: "#DoctorID",
               rules: [
                   { type: form.non_zero, message: "Please enter Doctor Name" },
                   { type: form.required, message: "Please enter Doctor Name" },
               ]
           },
        ],

        on_save_appointment: [
           {
               elements: "#item-count",
               rules: [
                   { type: form.non_zero, message: "Please add atleast one item" },
                  { type: form.required, message: "Please add atleast one item" },

               ]
           },
           {
               elements: "#count",
               rules: [
                   { type: form.non_zero, message: "Please add atleast one item" },
                  { type: form.required, message: "Please add atleast one item" },

               ]
           },
           {
               elements: "#DoctorID",
               rules: [
                   { type: form.non_zero, message: "Please enter Doctor Name" },
                   { type: form.required, message: "Please enter Doctor Name" },
                   { type: form.positive, message: "Please enter Doctor Name" },
               ]
           },
        ]
    },

    delete_confirm: function () {
        var self = AppointmentSchedule;
        var PatientID = $(this).closest('tr').find(".PatientID").val();
        var row = $(this).closest('tr');
        app.confirm_cancel("Do you want to Delete", function () {
            self.isdeletable_item(PatientID, row);
        }, function () {
        })
    },

    isdeletable_item: function (PatientID, row) {
        var self = AppointmentSchedule;
        var DoctorID = $("#DoctorID").val();
        var FromDate = $("#FromDateStringEdit").val();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/IsDeletable',
            data: {
                DoctorID: DoctorID,
                PatientID: PatientID,
                FromDate: FromDate
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    $(row).remove();
                    app.show_notice("Deleted Successfully");
                } else {
                    app.show_error("Failed to delete.");
                }
                self.count();
            },
        });

    },

    get_appointment_item: function () {
        var self = AppointmentSchedule;
        var length;
        if (!app.is_valid_date($("#FromDateStringEdit").val())) {
            return;
        }
        self.error_count = 0;
        self.error_count = self.validate_view();
        if (self.error_count > 0) {
            return;
        }
        self.get_appointment();
        self.get_doctor_consultation_Time();
        self.count();
    },

    get_appointment: function () {
        var self = AppointmentSchedule;
        $("#appointment-schedule-list tbody").empty();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/GetAppointmentItems/',
            dataType: "json",
            data: {
                'DoctorID': $("#DoctorID").val(),
                'Date': $("#FromDateStringEdit").val(),
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
                        + '<td class="PatientName">' + item.PatientName
                        + '<input type="hidden" class="PatientID"value="' + item.PatientID + '"/>'
                        + '<input type="hidden" class="AppointmentScheduleItemID"value="' + item.AppointmentScheduleItemID + '"/>'
                        + '<input type="hidden" class="AppointmentProcessID"value="' + item.AppointmentProcessID + '"/>'
                        + '</td>'
                    if (item.AppointmentProcessID == 0) {
                        content += ' <td><input type="text" value="' + item.Time + '" class="md-input Time label-fixed time15" /> </td>'
                    }
                    else {
                        content += '<td><input type="text" value="' + item.Time + '" class="md-input Time label-fixed time15" disabled /> </td>'
                    }

                    if (item.AppointmentProcessID == 0) {

                        content += '<td><input type="text" value="' + item.TokenNo + '" class="md-input TokenNo"  /></td>'
                    }
                    else {
                        content += '<td><input type="text" value="' + item.TokenNo + '" class="md-input TokenNo" disabled /></td>'
                    }
                    content += '<td>' + item.DepartmentName + '</td>'

                    + '<td>'
                    + ''
                    + '</td>'
                    + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#appointment-schedule-list tbody").html($content);
            },
        });
        self.count();
    },

    get_previous_dates: function () {
        var self = AppointmentSchedule;
        var PreviousDate = $("#day1").text();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/GetPreviousDates',
            data: {
                PreviousDate: PreviousDate,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {

            },
        });
    },

    list: function () {
        var self = AppointmentSchedule;

        $('#tabs-schedule').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = AppointmentSchedule;

        var $list;

        switch (type) {
            case "past":
                $list = $('#past-list');
                break;
            case "day1":
                $list = $('#day1-list');
                break;
            case "day2":
                $list = $('#day2-list');
                break;
            case "day3":
                $list = $('#day3-list');
                break;
            case "day4":
                $list = $('#day4-list');
                break;
            case "day5":
                $list = $('#day5-list');
                break;
            case "day6":
                $list = $('#day6-list');
                break;
            case "day7":
                $list = $('#day7-list');
                break;
            case "Past":
                $list = $('#past-list');
                break;
            case "Future":
                $list = $('#future-list');
                break;

            default:
                $list = $('#day1-list');
        }
        if (type == "Past" || type == "Future") {
            self.future_past_List($list, type);
        }
        else {
            self.date_List($list, type);
        }
    },

    date_List: function ($list, type) {
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
                    { "data": "Department", "className": "Department" },
                    { "data": "Time", "className": "Time" },
                    { "data": "TokenNo", "className": "TokenNo uk-text-right" },
                    {
                        "data": "", "searchable": false, "className": "edit", "orderable": false,
                        "render": function (data, type, row, meta) {
                            if (row.IsProcessed == false) {
                                return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnedit'>Edit</button>"
                            }
                            else {
                                return "";
                            }
                        }
                    },
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

    future_past_List: function ($list, type) {
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
                    { "data": "Department", "className": "Department" },
                    { "data": "Time", "className": "Time" },
                    { "data": "TokenNo", "className": "TokenNo uk-text-right" },
                    { "data": "Date", "className": "Date", },
                    {
                        "data": "", "searchable": false, "className": "edit", "orderable": false,
                        "render": function (data, type, row, meta) {
                            if (row.IsProcessed == 0) {
                                return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnedit'>Edit</button>"
                            }
                            else {
                                return "";
                            }
                        }
                    },
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

    delete_item: function () {
        var self = AppointmentSchedule;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#appointment-schedule-list tbody tr .sl-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#appointment-schedule-list tbody tr").length);

    },

    appointment_confirm: function () {
        var self = AppointmentSchedule;
        var PatientID = $(this).parents('tr').find('.PatientID').val();
        var DoctorID = $(this).parents('tr').find('.DoctorID').val();
        var AppointmentItemID = $(this).parents('tr').find('.AppointmentItemID').val();
        var Date = $(this).parents('tr').find('.Date').val();
        var BillablesID = $(this).parents('tr').find('.BillablesID').val();
        self.appointment(PatientID, DoctorID, AppointmentItemID, Date, BillablesID);
        //app.confirm("Do you want to confirm", function () {
        //    self.appointment(PatientID, DoctorID, AppointmentItemID, Date, BillablesID);
        //});
        self.set_netamount();
    },
    appointment_edit: function () {
        var self = AppointmentSchedule;
        var PatientID = $(this).parents('tr').find('.PatientID').val();
        var Patient = $(this).parents('tr').find('.Patient').text();
        var DoctorID = $(this).parents('tr').find('.DoctorID').val();
        var Doctor = $(this).parents('tr').find('.Doctor').text();
        var AppointmentItemID = $(this).parents('tr').find('.AppointmentItemID').val();
        var Date = $(this).parents('tr').find('.Date').val();
        var Department = $(this).parents('tr').find('.Department').text();
        var DepartmentID = $(this).parents('tr').find('.DepartmentID').val();
        var BillablesID = $(this).parents('tr').find('.BillablesID').val();
        
        
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/Edit',
            dataType: "html",
            type: "POST",
            data: {
                Date: Date,
                PatientID: PatientID,
                Patient:Patient,
                DoctorID: DoctorID,
                Doctor:Doctor,
                DepartmentID: DepartmentID,
                Department: Department,               
                AppointmentItemID: AppointmentItemID,
                BillablesID: BillablesID
            },
            success: function (response) {
                $("#get-edit").empty();
                $response = $(response);
                app.format($response);
                $("#get-edit").append($response);
                $('#show-edit').trigger('click');

            }
        });
    },
    appointment: function (PatientID, DoctorID, AppointmentItemID, Date, BillablesID) {
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
            url: '/AHCMS/AppointmentSchedule/GetAppointmentConfirmation',
            dataType: "json",
            type: "POST",
            data: {
                DoctorID: DoctorID,
                PatientID: PatientID,
                Date: FromDate,
                AppointmentScheduleItemID: AppointmentItemID,
                BillablesID: BillablesID
            },
            success: function (response) {
                if (response.Status == "success") {
                    if (response.IsAppointment == true) {
                        self.get_appointment_fee(AppointmentItemID);
                    }
                    else {

                    }
                }
            }
        });
    },



    get_appointment_fee: function (AppointmentItemID) {
        var self = AppointmentSchedule;
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/GetAppointment',
            //dataType: "json",
            type: "POST",
            data: {
                AppointmentScheduleItemID: AppointmentItemID,
            },
            success: function (response) {
                $("#get-appointment").empty();
                $response = $(response);
                app.format($response);
                $("#get-appointment").append($response);
                self.get_consultationItems(AppointmentItemID);
                self.get_bank_name();
                $('#show-appointment-fee').trigger('click');

            }
        });
    },

    set_total_amount: function () {
        var self = AppointmentSchedule;
        $('#consultation-list tbody tr').each(function () {
            var Rate = clean($(this).find(".rate").val());
            $(this).find(".netamount").val(Rate);
            var NetAmount = clean($("#NetAmount").val());
            var TotalNetAmount = parseFloat(NetAmount) + parseFloat(Rate);
            $("#NetAmount").val(TotalNetAmount);

        });
    },

    get_consultationItems: function (AppointmentItemID) {
        var self = AppointmentSchedule;
        $.ajax({
            url: "/AHCMS/AppointmentSchedule/ConsultationDetails",
            data: {
                AppointmentScheduleItemID: AppointmentItemID,
            },
            //dataType: "html",
            type: "POST",
            success: function (response) {
                $("#consultation-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#consultation-list").append($response);
                self.set_total_amount();
                self.set_netamount();
            }
        });
    },
    save_appointment_confirm: function () {
        var self = AppointmentSchedule;
        app.confirm_cancel("Do you want to Save", function () {
            self.save_appointment();
        },
        function () {
        })
    },
    save_appointment: function () {
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
                        window.location = "/AHCMS/AppointmentSchedule/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },
    get_data_for_appointment: function () {
        var self = AppointmentSchedule;
        var model = {};
        model.TransNo = $("#TransNo").val(),
        model.NetAmount = clean($("#NetAmount").val()),
        model.Remarks = $(".remarks").val(),
        model.PatientID = $("#PatientID").val(),
        model.BillableID = $("#BillableID").val(),
        model.AppointmentScheduleItemID = $("#AppointmentScheduleItemID").val(),
        model.PaymentModeID = $("#PaymentModeID").val(),
        model.BankID = $('#BankID').val(),
        model.ConsultationMode = $("#ConsultationMode").val(),

        model.ConsultationItems = [];
        var item = {};
        $('#consultation-list tbody tr').each(function () {
            item = {};
            item.ItemID = $(this).find(".ItemID").val();
            item.Rate = clean($(this).find(".rate").val());
            model.ConsultationItems.push(item);
        });

        return model;
    },
    print_Appointment: function () {
        var self = AppointmentSchedule;
        $.ajax({
            url: '/Reports/AHCMS/AppointmentBookingPrintPdf',
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
    print_Appointment_booking: function () {
        var self = AppointmentSchedule;
        var AppointmentItemID = $(this).parents('tr').find('.AppointmentItemID').val();
        $.ajax({
            url: '/Reports/AHCMS/AppointmentBookingPrintPdf',
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

    appointment_cancel: function () {
        var self = AppointmentSchedule;
        var PatientID = $(this).parents('tr').find('.PatientID').val();
        var DoctorID = $(this).parents('tr').find('.DoctorID').val();
        var AppointmentItemID = $(this).parents('tr').find('.AppointmentItemID').val();
        var Date = $(this).parents('tr').find('.Date').val();
        var BillablesID = $(this).parents('tr').find('.BillablesID').val();
        //self.appointment(PatientID, DoctorID, AppointmentItemID, Date, BillablesID);
        app.confirm("Do you want to Cancel Appointment", function () {
            self.cancellation(PatientID, AppointmentItemID, Date);
        })
    },
    cancellation: function (PatientID, AppointmentItemID, Date, BillablesID) {
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
                            window.location = "/AHCMS/AppointmentSchedule/Index";
                        }, 1000);
                    }
                    else {
                        app.show_notice("Appointment Cancellation Failed");
                    }
                }
            }
        });
    },

    save_and_confirm_appointment: function () {
        var self = AppointmentSchedule;
        var data;
        //index = $("#appointment-schedule-list tbody").length;
        //$("#item-count").val(index);
        data = self.get_appointmentSchedule_data();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/SaveAndConfirmAppointment',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var AppointmentScheduleItemID = response.AppointmentScheduleItemID
                    $("#AppointmentScheduleItemID").val(AppointmentScheduleItemID);
                    self.get_appointment_fee(AppointmentScheduleItemID);
                }
                else {
                    app.show_error('Failed to create Appointment Schedule');
                }
            }
        });
    },
    get_appointmentSchedule_data: function () {
        var self = AppointmentSchedule;
        var data = {
            PatientName: $("#PatientName").val(),
            PatientID: $("#PatientID").val(),
            Time: $("#Time").val(),
            TokenNo: $("#TokenNo").val(),
            FromDateString: $("#FromDateStringEdit").val(),
            DoctorID: $("#DoctorID").val(),
            DepartmentID: $("#DepartmentID option:selected").val()
        };
        return data;
    },
    is_appointmentProcessed_already: function () {
        var self = AppointmentSchedule;
        $("#btnAddItems").hide();
        $("#is-save_confirm").val(1);
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        var PatientReferedBy = $("#ReferenceThrough").val();
        var ConsultationMode = $("#ConsultingMode").val();
        var PatientRefered = $("#PatientReferencedBy").val();
        //if (PatientReferedBy != 'General' || PatientRefered != 'General') {
        //    app.confirm("Patient is Referenced By " + PatientReferedBy+PatientRefered+".. Do you Want To Add", function () {
        //        self.add_appointment();
        //    })
        //}
        //else {
        //    self.add_appointment();
        //}
        var DoctorID = $("#DoctorID").val();
        var PatientID = $("#PatientID").val();
        $.ajax({
            url: '/AHCMS/AppointmentSchedule/IsAppointmentProcessed',
            data: {
                DoctorID: DoctorID,
                PatientID: PatientID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.IsAppointmentProcessed == true) {
                        self.save_and_confirm_appointment();
                    }
                    else {
                        app.show_error('patient already in consultation');
                    }
                }
            }
        });
    },

    delete_appointment: function () {
        var self = AppointmentSchedule;
        if ($("#is-save_confirm").val() == 1) {
            var AppointmentItemID = $("#AppointmentScheduleItemID").val();
            $.ajax({
                url: '/AHCMS/AppointmentSchedule/DeleteAppointmentScheduleItems',
                dataType: "json",
                type: "POST",
                data: {
                    AppointmentScheduleItemID: AppointmentItemID
                },
                success: function (response) {
                    if (response.Status == "success") {

                        setTimeout(function () {
                            window.location = "/AHCMS/AppointmentSchedule/Create";
                        }, 1000);
                    }
                }
            });
        }
        else {

        }

    },
    get_doctor_consultation_schedule: function (starTime, endTime) {
        var self = AppointmentSchedule;
        $.ajax({
            url: "/Masters/ConsultationSchedule/GetDoctorConsultationSchedule",
            dataType: "json",
            data: {
                DoctorID: $("#DoctorID").val(),
                Date: $("#FromDateStringEdit").val(),
                StartTime: starTime,
                EndTime: endTime
            },
            //dataType: "html",
            type: "GET",
            success: function (response) {
                $("#SlotName").html("");
                var html = ""; //"<option value =''>Select</option>";
                $.each(response, function (i, record) {
                    html += "<option value='" + record.SlotName + "'data-time='" + record.Time + "'>" + record.Time + "</option>";
                });
                $("#SlotName").append(html);
                var time = $("#SlotName").find(':selected').data('time');
                var slot = $("#SlotName option:selected").val();
                $("#Time").val(time);
                if (slot == undefined || slot == "") {
                    $("#TokenNo").val(0);
                } else {
                    $("#TokenNo").val($("#SlotName option:selected").val().slice(1));
                }
            }
        });
    },
    get_doctor_consultation_Time: function () {
        var self = AppointmentSchedule;
        var Stime = "";
        var Etime = "";
        $("#Time").val("");
        $("#TokenNo").val("");
        $.ajax({
            url: "/Masters/ConsultationSchedule/GetDoctorConsultationTime",
            dataType: "json",
            data: {
                DoctorID: $("#DoctorID").val(),
                Date: $("#FromDateStringEdit").val()
            },
            //dataType: "html",
            type: "GET",
            success: function (response) {
                $("#ConsultationTime").html("");
                var html = ""; //"<option value =''>Select</option>";
                $.each(response, function (i, record) {
                    html += "<option value='" + record.StartTime + "'data-endtime='" + record.EndTime + "'>" + record.StartTime + " - " + record.EndTime + "</option>";
                });
                $("#ConsultationTime").append(html);

                Stime = $("#ConsultationTime").val();
                Etime = $(ConsultationTime).find(':selected').data('endtime');
                self.get_doctor_consultation_schedule(Stime, Etime);
            }
        });
    },
    get_schedule_time: function () {
        var self = AppointmentSchedule;
        $("#Time").val("");
        $("#TokenNo").val("");
        var time = $(this).find(':selected').data('time')
        $("#Time").val(time);
        $("#TokenNo").val($("#SlotName option:selected").val().slice(1));
    },

    get_consultation_time: function () {
        var self = AppointmentSchedule;
        var Stime = $("#ConsultationTime").val();
        var Etime = $(this).find(':selected').data('endtime');
        self.get_doctor_consultation_schedule(Stime, Etime);
        self.get_schedule_time();
    },

    PatientCode: [],
    qrcodeitems: [],

    generate_barcode: function () {
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
                    + '<tr><td colspan="3" style="height:70px;"></td></tr>'
                    + '<tr><td colspan="3" style="height:55px;"></td></tr>'
                    + '<tr><td colspan="3" style="height:9px;"><p >' + item.PatientName + '</p></td></tr>'
                    + '<tr><td style="width:10px;height:6px;padding:3px 10px 0px 50px;font-family:DZ-ILAJ;font-size:14px;">' + item.Age + '</td><td  style="width:30px;font-family:DZ-ILAJ;font-size:14px;padding:3px 0px 0px 65px">' + item.Gender + '</td><td  style="width:50px;font-family:DZ-ILAJ;font-size:14px;padding:3px 0px 0px 60px">' + item.PatientCode + '</td></tr>'
                    + '<tr><td colspan="3" style="padding:10px 0px 0px 70px"><svg width="158px" height="72px" class="barcode_print ' + classname + '">' + '</svg></td></tr>'
                    + '</table>';
            //  $('#barcode').append($content);
            $('#textdata').append(content);
            $('.' + classname).JsBarcode(barcodedata, {
                width: 1,
                height: 25,
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
    },

    print_bar_code: function () {
        var self = AppointmentSchedule;
        var height = $(document).height() - $("#print-content").offset().top - $("#print-preview-common .uk-modal-footer").height() - 100;
        $("#print-content").html("<iframe id='IFramePDF'></iframe>");
        var iframeBody = $("#IFramePDF").contents().find("body");
        var iframehead = $("#IFramePDF").contents().find("head");
        iframehead.append('<style>@font-face {font-family: "DZ-ILAJ";src: url("/Assets/css/fonts/DZ-ILAJ.TTF"); }p{font-family:"DZ-ILAJ";font-size:16px;font-weight:bold;padding:0px 0px 0px 50px;text-transform:uppercase;letter-spacing:1px;}</style>');
        iframeBody.append($("#textdata").html());
        iframeBody.append($("#barcode").html());

        $("#print-content").height(height);
        var $IFrame = $("#IFramePDF");
        $IFrame.css({ 'width': '100%', 'height': '100%' });
        UIkit.modal("#print-preview-common").show();
    },

    get_patient_details: function () {
        var self = AppointmentSchedule;
        var row = $(this).closest('tr');
        PatientID = $(this).parents('tr').find('.PatientID').val();
        $("#PATIENTID").val("");
        $("#PATIENTID").val(PatientID);
        self.get_patient(PatientID);
    },

    get_patient: function (PatientID) {
        var self = AppointmentSchedule;
        $("#PATIENTID").val("");
        $("#PATIENTID").val(PatientID);
        $.ajax({
            url: "/AHCMS/AppointmentSchedule/GetPatientDetails",
            data: {
                ID: PatientID,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#Patient-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#Patient-list").append($response);
            }
        });
        $('#item-details').show();
        $('#show-item-details').trigger('click');
    },

    get_barcode: function () {
        var self = AppointmentSchedule;
        var PatientID = $("#PATIENTID").val();
        self.patientDetails = [];
        $('#Patient-list tbody tr').each(function () {
            item = {};
            item.PatientName = $(this).find(".patient_name").text();
            item.PatientCode = $(this).find(".patient_code").val();
            item.PatientAge = $(this).find(".patient_age").val();
            item.PatientGender = $(this).find(".patient_gender").val();
            self.patientDetails.push(item);
        });
        self.get_item_for_barcode_generator(PatientID);
        $("#btnPrintQRCode").show();
        $("#btnQRCodeSave").hide();
    },

    get_item_for_barcode_generator: function (PatientID) {
        var self = AppointmentSchedule;

        $.ajax({
            url: "/AHCMS/AppointmentSchedule/GetPatientForBarCodeGenerator",
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
                            Age: record.Age, Gender: record.Gender
                        });
                    });
                }
                //if (IsBarCodeGenerator == "False") {
                //    $('#showw-qrcode').trigger('click');//(For Test Bar Code)
                //    self.generate_qrcode();//(For Test Bar Code)
                //}
                //else {
                $('#show-barcode').trigger('click');
                self.generate_barcode();
                //}
            }
        });
    },
}