Diagnosis = {
    init: function () {
        var self = Diagnosis;
        self.bind_events();
    },

    bind_events: function(){
        var self = Diagnosis;
        $(".btnSave").on('click', self.save_confirm);
    },

    validate_form: function () {
        var self = Diagnosis;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    save_confirm: function () {
        var self = Diagnosis;
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

    get_data: function () {
        var self = Diagnosis;
        var data = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            Description: $("#Description").val(),
        }
        return data;
    },

    save: function () {
        var self = Diagnosis;
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Diagnosis/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Diagnosis created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Diagnosis/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                }
            },
        });
    },

    rules: {
        on_save: [
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter Diagnosis Name" },
                ]
            },
        ],
    },

    list: function () {
        var $list = $('#diagnosis-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/Diagnosis/GetDiagnosisList"

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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "Name", "className": "Name" },
                   { "data": "Description", "className": "Description" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/Diagnosis/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
}