var InternationalPatient = {

    init: function () {
        var self = InternationalPatient;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });

        $(".international").hide();
        self.bind_events();
        self.change_country();
        self.change_discount();
    },

    bind_events: function () {
        var self = InternationalPatient;
        $("#CountryID").on('change', self.change_country);
        //$("#DiscountTypeID").on('change', self.change_discount);
        $("#DOB").on('change', self.age_calculation);
        $("#StateID").on('change', self.GetDistrict);
        $(".btnSave").on('click', self.save_confirm);
        UIkit.uploadSelect($("#select-photo"), self.selected_photo_settings);
        UIkit.uploadSelect($("#select-passport"), self.selected_passport_settings);
        UIkit.uploadSelect($("#select-visa"), self.selected_visa_settings);
        $('body').on('click', 'a.remove-quotation', self.remove_photo);
        $('body').on('click', 'a.remove-passport', self.remove_passport);
        $('body').on('click', 'a.remove-visa', self.remove_visa);
        $.UIkit.autocomplete($('#doctor-autocomplete'), Config.doctor);
        $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $("#btnOKEmployee").on("click", self.select_employee);
        $("#CountryID").on('change', self.Get_State);

        $("#DiscountTypeID").on('change', InternationalPatient.change_discount);
        $("body").on("ifChanged", ".check-box", InternationalPatient.check);
        $("body").on("change", ".DiscountCategoryID", InternationalPatient.set_discount);
        UIkit.uploadSelect($("#other-quotations"), self.other_quotation_settings);
        UIkit.uploadSelect($("#other-documents"), self.other_document_settings);
    },

    other_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            //percent = Math.ceil(percent);
            // bar.css("width", percent + "%").text(percent + "%");
        },
        complete: function (response, xhr) {
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                var dropdown = $("#other-quotation-list .uk-nav-dropdown");
                width = $('#other-quotation-list').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        dropdown.append("<li class='file-list'>"
                        + "<a class='remove-file'>X</a>"
                        + "<span data-id='" + record.ID + "' style='width:" + width + "px;' class='view-file' data-url='" + record.URL + "' data-path='" + record.Path + "'>"
                        + record.Name
                        + "</span>"
                        + "</li>");
                        success += record.Name + " " + record.Description + "<br/>";
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
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            // app.show_notice("Uploaded");
            //console.log(response);

            var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
            $('#file-count').text(file_count + " File(s)");
        }
    },

    other_document_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            //percent = Math.ceil(percent);
            // bar.css("width", percent + "%").text(percent + "%");
        },
        complete: function (response, xhr) {
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                var dropdown = $("#other-document-list .uk-nav-dropdown");
                width = $('#other-document-list').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        dropdown.append("<li class='file-list'>"
                        + "<a class='remove-file'>X</a>"
                        + "<span data-id='" + record.ID + "' style='width:" + width + "px;' class='view-file' data-url='" + record.URL + "' data-path='" + record.Path + "'>"
                        + record.Name
                        + "</span>"
                        + "</li>");
                        success += record.Name + " " + record.Description + "<br/>";
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
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            // app.show_notice("Uploaded");
            //console.log(response);

            var file_count = $("#other-document-list .uk-nav-dropdown li").length;
            $('#file-count1').text(file_count + " File(s)");
        }
    },

    select_employee: function () {
        var self = InternationalPatient;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    set_doctor: function (event, item) {
        var self = AppointmentSchedule;
        $("#DoctorName").val(item.Name);
        $("#DoctorID").val(item.id);
    },

    selected_photo_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
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
                width = $('#selected-photo').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-photo').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
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

    selected_passport_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
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
                width = $('#selected-passport').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-passport').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-passport'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
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

    selected_visa_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
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
                width = $('#selected-visa').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-visa').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-passport'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
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

    remove_photo: function () {
        $(this).closest('span').remove();
    },

    remove_passport: function () {
        $(this).closest('span').remove();
    },

    remove_visa: function () {
        $(this).closest('span').remove();
    },

    change_country: function () {
        var self = InternationalPatient;

        var Country = $("#CountryID option:selected").text().trim();

        if (Country != "INDIA") {
            $('.international').show();
        }
        else {
            $('.international').hide();
        }
    },

    change_discount: function () {
        var self = InternationalPatient;
        var DiscountType = $("#DiscountTypeID option:selected").text().trim();
        var DiscountTypeID = $("#DiscountTypeID option:selected").val();
        if (DiscountTypeID > 0) {
            self.get_discount_by_discountType(DiscountTypeID)
        }
        if (DiscountType != "Select") {
            $('.Discount-Tab').show();
        }
        else {
            $('.Discount-Tab').hide();
        }
    },

    get_discount_by_discountType: function (DiscountTypeID) {
        var length;
        var self = InternationalPatient;
        $("#tbl-discount-list tbody").empty();
        $.ajax({
            url: '/Masters/Discount/GetDiscountDetailsByDiscountType/',
            dataType: "json",
            data: {
                'DiscountTypeID': DiscountTypeID,
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                var DiscountPercentageList = $("#DiscountPercentageList").html();
                if (length != 0) {
                    var c = $('#tbl-discount-list tbody tr').length;
                    $(response.Data).each(function (i, item) {
                        var slno = (c + (i + 1));
                        content += '<tr>'
                            + '<td class="sl-no">' + slno + '</td>'
                            + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                            + '<td class="itemName">' + item.BusinessCategoryName
                            + '<input type="hidden" class="BusinessCategoryID" value="' + item.BusinessCategoryID + '"/>'
                            + '<input type="hidden" class="DiscountCategory" value="' + item.DiscountCategoryID + '"/>'
                            + '</td>'
                            + '<td class="DiscountPercentageList" ></td>'
                            + '<td>' + '<input type="text" value=" ' + item.DiscountPercentage + '" class="md-input uk-text-right DiscountPercentage mask-qty" disabled /> ' + '</td>'
                            + '</tr>';
                        $content = $(content);
                        $content.find(".DiscountPercentageList").html("<select class='md-input DiscountCategoryID' disabled>" + DiscountPercentageList + "</select>");
                        app.format($content);
                        //self.clear_data();
                    });
                    $("#tbl-discount-list tbody").append($content);
                    $("#tbl-discount-list tbody .DiscountCategoryID").each(function () { $(this).val($(this).closest('tr').find('.DiscountCategory').val()) });
                }
            },
        });
    },

    check: function () {
        var self = InternationalPatient;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
            $(row).find(".DiscountCategoryID").prop("disabled", false);
        } else {
            $(row).find(".DiscountCategoryID").prop("disabled", true);
            $(row).removeClass('included');
        }
        //self.count_items();
    },
    set_discount: function () {
        var self = InternationalPatient;
        var row = $(this).closest('tr');
        var data = $(row).find(".DiscountCategoryID option:selected").data('value');
        $(row).find(".DiscountPercentage").val(data);
    },

    age_calculation: function () {
        var self = InternationalPatient;
        var dobSplit = $("#DOB").val().split("-");
        var dob = `${dobSplit[2]}-${dobSplit[1]}-${dobSplit[0]}`
        var dobdate = new Date(dob)
        var nowDate = new Date();
        var dates = self.time_conversion(nowDate - dobdate)
        console.log(dates)
        //var DOB = dob.split("-");
        //var DOBYear = DOB[2];
        //var nowYear = nowDate.getFullYear();
        //var age = nowYear - DOBYear;
        $("#Age").val(dates.years.toString());
        $("#Month").val(dates.months.toString());
    },

    time_conversion: function (millisec) {
        var self = InternationalPatient;
        var days = (millisec / (1000 * 60 * 60 * 24)).toFixed(1);
        let reminder = days % 365
        let round = days - reminder
        let years = round / 365
        let months = (reminder / 30).toFixed(0)
        return { years, months }

    },

    GetDistrict: function () {
        var self = InternationalPatient;
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

    Get_State: function () {
        var self = InternationalPatient;
        var country = $(this);
        $.ajax({
            url: '/Masters/State/GetStateCountryWise/',
            dataType: "json",
            type: "GET",
            data: {
                CountryID: country.val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                if (country.attr('id') == "Country") {
                    $("#StateID").html("");
                    $("#StateID").append(html);
                } else {
                    $("#StateID").html("");
                    $("#StateID").append(html);
                }
            }
        });
    },

    save_confirm: function () {
        var self = InternationalPatient;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
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
                 elements: "#MobileNumber",
                 rules: [
                     { type: form.required, message: "Please enter MobileNumber" },
                 ]
             },
             //{
             //    elements: "#Age",
             //    rules: [
             //        { type: form.required, message: "Please enter Age" },
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
                  elements: "#DiscountStartDate",
                  rules: [
                      {
                          type: function (element) {
                              var discountType = $("#DiscountTypeID :selected").val()
                              var startDate = $("#DiscountStartDate").val();
                              var error = false;
                              if (discountType > 0 && startDate =="") {
                                  error = true;
                              }
                              return !error;
                          }, message: "Discount start date required"
                      },
                  ]
              },
              {
                  elements: "#DiscountEndDate",
                  rules: [
                      {
                          type: function (element) {
                              var discountType = $("#DiscountTypeID :selected").val()
                              var startDate = $("#DiscountEndDate").val();
                              var error = false;
                              if (discountType > 0 && startDate == "") {
                                  error = true;
                              }
                              return !error;
                          }, message: "Discount end date required"
                      },
                  ]
              },
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
    },

    validate_form: function () {
        var self = InternationalPatient;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    Save: function () {
        var self = InternationalPatient;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/InternationalPatient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/InternationalPatient/Index";
                }
                else {
                    app.show_error("Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    get_data: function () {
        var self = InternationalPatient;
        var OtherQ = [];
        $("#other-quotation-list span.view-file").each(function () {
            OtherQ.push($(this).data('id'));
        });
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
            LandLine: $("#LandLine").val(),
            Place: $("#Place").val(),
            Month: $("#Month").val(),
            DiscountTypeID: $("#DiscountTypeID").val(),
            DiscountStartDate: $("#DiscountStartDate").val(),
            DiscountEndDate: $("#DiscountEndDate").val(),
            MaxDisccountAmount: clean($("#MaxDisccountAmount").val()),
            ReferalName: $("#ReferalName").val(),
            MiddleName: $("#MiddleName").val(),
            LastName: $("#LastName").val(),
            CountryCode: $("#CountryCode").val(),
            OtherQuotationIDS: OtherQ.join(","),
            EmergencyContactNo: $("#EmergencyContactNo").val(),

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

    change_country_detail: function () {
        var self = InternationalPatient;

        var Country = $("#Country").val().trim();

        if (Country != "INDIA") {
            $('.international').show();
        }
        else {
            $('.international').hide();
        }
    },

    history: function () {
        var self = InternationalPatient;
        var ID = $(this).closest('tr').find(".ID").val();
        $("#tblhistorylist thead").empty();
        $.ajax({
            url: '/Masters/InternationalPatient/GetInternationalPatientDetails',
            dataType: "json",
            data: {
                ID: ID,
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Items.length;
                if (length != 0) {
                    var c = $('#tblhistorylist thead').length;
                    $(response.Items).each(function (i, item) {
                        content +=
                            '<tr class="md-color-green-800">'
                               + '<td><b>Patient     : </b></td>'
                               + '<td><b>' + item.Name + '</b></td>'
                            + '</tr>'
                            + '<tr class="md-color-green-800">'
                               + '<td><b>MRDNo     : </b></td>'
                               + '<td><b>' + item.Code + '</b></td>'
                            + '</tr>'
                            + '<tr class="md-color-green-800">'
                               + '<td><b>Age     : </b></td>'
                               + '<td><b>' + item.Age + '</b></td>'
                            + '</tr>'
                            + '<tr class="md-color-green-800">'
                               + '<td><b>Place     : </b></td>'
                               + '<td><b>' + item.Place + '</b></td>'
                            + '</tr>'
                            + '<tr class="md-color-green-800">'
                               + '<td><b>Phone     : </b></td>'
                               + '<td><b>' + item.MobileNumber + '</b></td>'
                            + '</tr>';
                        $content = $(content);
                        app.format($content);
                    });
                    $("#tblhistorylist thead").html($content);
                }
            },
        });
        $.ajax({
            url: '/AHCMS/PatientDiagnosis/History',
            data: {
                OPID: 0,
                PatientID: ID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#history-list").empty();
                $response = $(response);
                app.format($response);
                $("#history-list").append($response);
            },
        });
        $(".history").removeClass("uk-hidden");
        $(".history").addClass("uk-active");
        $(".patients").removeClass("uk-active");
        //$(".tabReview").addClass("uk-hidden");
        //if ($(".fresh").is(":checked") == true) {
        //    $('.fresh').iCheck('uncheck')
        //};
    },

    EditPatient: function (release) {
        $.ajax({
            url: '/Masters/InternationalPatient/Edit',
            data: {
                ID: ID,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    list: function () {
        var self = InternationalPatient;

        $('#tabs-patientlist').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
        $('body').on('click', '.btnHistory', self.history);
        $('body').on('click', '.btnEdit', self.EditPatient);
    },

    tabbed_list: function (type) {
        var self = InternationalPatient;

        var $list;

        switch (type) {
            case "saved":
                $list = $('#patient-list');
                break;
            default:
                $list = $('#patient-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/InternationalPatient/GetInternationalPatientListView"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Masters/InternationalPatient/GetInternationalPatientListView?type=" + type,
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },

                   { "data": "Code", "className": "Code" },
                   { "data": "Name", "className": "Name" },
                   { "data": "Phone", "className": "Phone" },
                   {
                       "data": "History", "className": "action uk-text-center", "searchable": false,
                       "render": function (data, type, row, meta) {
                           return "<button class='md-btn md-btn-primary btnHistory' >History</button>";
                       }
                   }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/InternationalPatient/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;
        }
    },

    Patientlist: function () {
        var $list = $('#patient-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/InternationalPatient/GetInternationalPatientList"

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
                              + "<input type='hidden' class='ReferenceID' value='" + row.PatientReferedByID + "'>"
                              + "<input type='hidden' class='Reference' value='" + row.PatientReferedBy + "'>"
                              + "<input type='hidden' class='ConsultationMode' value='" + row.ConsultationMode + "'>"


                       }
                   },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck unchecked value='" + row.ID + "' >";
                        }
                    },
                   { "data": "Code", "className": "Code" },
                   { "data": "Name", "className": "Name" },
                   { "data": "Place", "className": "Place" },
                   { "data": "District", "className": "District" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "Phone", "className": "Phone" },
                   { "data": "LastVisitDate", "className": "LastVisitDate" },

                     {
                         "data": "Edit", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnEdit' >Edit</button>";
                         }
                     },
                   {
                       "data": "History", "className": "action uk-text-center", "searchable": false,
                       "render": function (data, type, row, meta) {
                           return "<button class='md-btn md-btn-primary btnHistory' >...</button>";
                       }
                   },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('.btnHistory').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        window.open("/Masters/InternationalPatient/PatientHistory/" + Id);
                    }),
                        $list.find('.btnEdit').on('click', function () {
                            var Id = $(this).closest("tr").find("td .ID").val();
                            window.open("/Masters/InternationalPatient/Edit/" + Id);
                        });
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
    InPatientlist: function () {
        var $list = $('#in-patient-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/InternationalPatient/GetInPatientList"

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
                              + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                              + "<input type='hidden' class='RoomID' value='" + row.RoomID + "'>"                             
                       }
                   },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio PatientID' name='PatientID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                   { "data": "Code", "className": "Code" },
                   { "data": "Patient", "className": "Patient" },                   
                   { "data": "IPNO", "className": "IPNO" },
                   { "data": "AdmissionDate", "className": "AdmissionDate" },
                   { "data": "Room", "className": "Room" },
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
    AppoinmentPatientList: function () {
        var $list = $('#appoinment-scheduled-patient-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/InternationalPatient/GetAppointmentScheduledPatientList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[5, 'desc']],
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
                               + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                               + "<input type='hidden' class='Age' value='" + row.Age + "'>"
                               + "<input type='hidden' class='Doctor' value='" + row.Doctor + "'>"
                               + "<input type='hidden' class='Gender' value='" + row.Gender + "'>"
                               + "<input type='hidden' class='Mobile' value='" + row.Phone + "'>"
                               + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                       }
                   },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                   { "data": "Name", "className": "Name" },
                   { "data": "OPNO", "className": "OPNO" },
                   { "data": "OPDate", "className": "OPDate" },                   
                   { "data": "Phone", "className": "Phone" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });           
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
}