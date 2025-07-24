OutPatient = {

    init: function () {
        var self = OutPatient;
        self.bind_events();
    },

    list: function () {
        var self = OutPatient;
        $list = $('#Out-Patient-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Out-Patient-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/OutPatient/Details/' + Id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    patient_list: function () {
        var $list = $('#patient-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": "/Masters/OutPatient/GetOutPatientListForPopup",
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
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
                    { "data": "Name", "className": "Name" },
                    { "data": "Place", "className": "Place" },
                    { "data": "Mobile", "className": "Mobile" },

                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#LocationID', function () {
                list_table.fnDraw();
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
            return list_table;
        }
    },

    bind_events: function () {
        var self = OutPatient;
        $("#StateID").on('change', self.GetDistrict);
        $(".btnSave").on('click', self.save_confirm);
        $("#Category").on("change", self.Code_change);
        $('.IsGSTRegistered').on('ifChanged', self.IsGSTChanged);

    },

    GetDistrict: function () {
        var self = OutPatient;
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

    get_data: function () {
        var self = OutPatient;
        var data = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            AddressLine1: $("#AddressLine1").val(),
            AddressLine2: $("#AddressLine2").val(),
            StateID: $("#StateID option:selected").val(),
            DistrictID: $("#DistrictID option:selected").val(),
            CountryID: $("#CountryID option:selected").val(),
            MobileNumber: $("#MobileNumber").val(),
            DOB: $("#DOB").val(),
            Email: $("#Email").val(),
            GSTNo: $("#GSTNo").val(),
            PinCode: $("#PinCode").val(),
            Category: $("#Category").val(),
        };
        return data;
    },

    save_confirm: function () {
        var self = OutPatient;
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

    IsGSTChanged: function () {
        if ($(".IsGSTRegistered").prop('checked') == true) {
            $('#GSTNo').addClass('visible');
            $(".Gst-number").show();
        } else {
            $(".Gst-number").hide();
            $('#GSTNo').removeClass('visible');
        }
    },

    Save: function () {
        var self = OutPatient;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/OutPatient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/OutPatient/Index";
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    validate_form: function () {
        var self = OutPatient;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
             {
                 elements: "#Category",
                 rules: [
                     { type: form.required, message: "Please choose category" },
                 ]
             },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter Name" },
                ]
            },

            {
                elements: "#AddressLine1",
                rules: [
                    { type: form.required, message: "Please enter Address" },
                ]
            },

            {
                elements: "#StateID",
                rules: [
                    { type: form.required, message: "Please choose State" },
                ]
            },
             {
                 elements: "#MobileNumber",
                 rules: [
                     { type: form.required, message: "Please enter MobileNumber" },
                 ]
             },
             {
                 elements: "#DOB",
                 rules: [
                     { type: form.required, message: "Please enter DOB" },
                 ]
             },
               {
                   elements: "#GSTNo",
                   rules: [
                       {
                           type: function (element) {
                               var gstno = $("#GSTNo").val().length;
                               if (gstno > 0 && gstno < 15) {
                                   return false;
                               }
                               else {
                                   return true;
                               }
                           }, message: "GSTNo Contain Minimum 15 Characters"
                       },
                   ]
               },
        ],
    },

    Code_change: function () {
        var self = OutPatient;
        var Category = $("#Category").val();
        $.ajax({
            url: '/Masters/OutPatient/GetSerialNo',
            data: { Category: Category },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#Code").val(response.Data);
            }
        });
    },


}