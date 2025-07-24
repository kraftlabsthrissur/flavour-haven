Process = {
    init: function () {
        Process.bind_events();
        Process.list();
    },

    list: function () {
        var self = Process;
        $role_list = $('#Process-list');
        if ($role_list.length) {
            $role_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Process-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Process/Details/' + Id;
            });
            altair_md.inputs();
            var role_list_table = $role_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            role_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    role_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        var self = Process;
        $(".btnSave").on("click", self.save_confirm);
    },

    save_confirm: function () {
        var self = Process;
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_data: function () {
        var self = Process;
        var data = {};
        data.ID = $("#ID").val(),
        data.Code = $("#Code").val(),
        data.Process = $("#Process").val()
        return data;
    },

    save: function () {
        var self = Process;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Masters/Process/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/Process/Index";
                }
                else {
                    app.show_error("Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            }
        });
    },

    validate_form: function () {
        var self = Process;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please enter a Code" }
                ],
            },

            {
                elements: "#Process",
                rules: [
                    { type: form.required, message: "Please enter a Process" }
                ],
            },

        ],
    }
}
