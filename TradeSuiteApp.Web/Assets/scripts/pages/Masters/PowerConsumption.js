PowerConsumption = {
    init: function () {
        PowerConsumption.bind_events();
        PowerConsumption.list();
    },

    list: function () {
        var self = PowerConsumption;
        $role_list = $('#Electricity-list');
        if ($role_list.length) {
            $role_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Electricity-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .Location").val();
                window.location = '/Masters/PowerConsumption/Details/' + Id;
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
        var self = PowerConsumption;
        $(".btnSave").on("click", self.save_confirm);
    },

    save: function () {
        var self = PowerConsumption;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Masters/PowerConsumption/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/PowerConsumption/Index";
                }
                else {
                    app.show_error("Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            }
        });
    },

    get_data: function () {
        var self = PowerConsumption;
        var data = {};
        data.ID = $("#ID").val(),
        data.Location = $("#Location").val(),
        data.Items = [];
        var item = {};
        $('#Electricitylist tbody tr').each(function () {
            item = {};
            item.Time = $(this).find(".Time").text().trim();
            item.Amount = clean($(this).find(".Amount").val());
            data.Items.push(item);
        });
        return data;
    },

    validate_form: function () {
        var self = PowerConsumption;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
               {
                   elements: "#Location",
                   rules: [
                       { type: form.non_zero, message: "Please add Location" },
                      { type: form.required, message: "Please add Location" },

                   ]
               },

               {
                   elements: ".Amount",
                   rules: [
                       { type: form.non_zero, message: "Please add Amount" },
                      { type: form.required, message: "Please add Amount" },
                   ]
               },
        ]
    },

    save_confirm: function () {
        var self = PowerConsumption;
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
}
