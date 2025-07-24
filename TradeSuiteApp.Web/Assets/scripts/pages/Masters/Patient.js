Patient = {
    init: function () {
        var self = Patient;

        supplier.Doctor_list();
        $('#doctor-list').SelectTable({
            selectFunction: self.select_doctor,
            returnFocus: "#ItemName",
            modal: "#select-doctor",
            initiatingElement: "#DoctorName"
        });
        self.bind_events();
    },

    list: function () {
        var self = Patient;
        var $list = $('#Patient-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/Patient/GetPatientList"

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
                        data.params = [
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
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/Patient/Details/" + Id);
                    });
                },
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
                    "url": "/Masters/Patient/GetPatientListForPopup",
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
                    { "data": "DoctorName", "className": "DoctorName" },

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
        var self = Patient;
        $(".btnSave").on("click", self.save_patient);
        $.UIkit.autocomplete($('#add-doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#add-doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $("#btnOKDoctor").on("click", self.select_doctor);
    },

    save_patient: function () {
        var self = Patient;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_patient();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_patient_data();
        $.ajax({
            url: '/Masters/Patient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.Status == "success") {
                        app.show_notice(" Saved Successfully");
                        window.location = "/Masters/Patient/Index";
                    }
                    else {
                        app.show_error('Failed to create Patient');
                    }
                }
            }
        });
    },

    get_patient_data: function () {
        var data = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Email: $("#Email").val(),
            Mobile: $("#Mobile").val(),
            Age: $("#Age").val(),
            DOB: $("#DOB").val(),
            Sex: $("#Sex").val(),
            Email: $("#Email").val(),
            Address1: $("#Address1").val(),
            Address2: $("#Address2").val(),
            Place: $("#Place").val(),
            PinCode: $("#PinCode").val(),
            DoctorID: $("#DoctorID").val(),
        };
        return data;
    },

    validate_patient: function () {
        var self = Patient;
        if (self.rules.on_save_patient.length > 0) {
            return form.validate(self.rules.on_save_patient);
        }
        return 0;
    },

    rules: {
        on_save_patient: [
      {
          elements: "#Name",
          rules: [
              { type: form.required, message: "Please enter name" },
          ]
      },
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please enter code" },
                ]
            },

            {
                elements: "#Age",
                rules: [
                    { type: form.required, message: "Please enter Age" },
                ]
            },

            //{
            //    elements: "#DOB",
            //    rules: [
            //        { type: form.required, message: "Please enter Age" },
            //    ]
            //},
             {
                 elements: "#Sex",
                 rules: [
                     { type: form.required, message: "Please enter Age" },
                 ]
             },
             {
                 elements: "#DoctorName",
                 rules: [
                     { type: form.required, message: "Please choose an Item" },
                     { type: form.non_zero, message: "Please choose an Item" },
                 ],
             },
        ],
    },

    get_doctor: function (release) {

        $.ajax({
            url: '/Masters/Doctor/GetDoctorAutoComplete',
            data: {
                Hint: $('#DoctorName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_doctor: function (event, item) {
        var self = Patient;
        var id = item.id;
        $("#DoctorName").val(item.Name);
        $("#DoctorID").val(item.id);
    },

    select_doctor: function () {
        var self = Patient;
        var radio = $('#select-doctor tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-doctor')).hide();
    },

}
