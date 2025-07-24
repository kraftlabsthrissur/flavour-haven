$(function () {
   secondaryunit.unit_list();
   secondaryunit.bind_events();
})
secondaryunit = {
    unit_list: function () {
        $unit_list = $('#unit-list');
        if ($unit_list.length) {
            $unit_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#unit-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/SecondaryUnit/Details/' + Id;
            });
            altair_md.inputs();
            var unit_list_table = $unit_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            unit_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    unit_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click',secondaryunit.update);
        $(".btnSave").on('click',secondaryunit.save_confirm);
    },

    save_confirm: function () {
        var self = secondaryunit;
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

    error_count: 0,
    save :function () {
        var self = secondaryunit;
        $('.btnSave').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/SecondaryUnit/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("UNIT created successfully");
                    setTimeout(function () {
                       window.location = "/Masters/SecondaryUnit/Index";
                    }, 1000);
                } else {
                    app.show_error("Already Exists.");
                    $('.update').css({ 'display': 'block' });
                }
            },
        });
    },
    update: function () {
        var self = secondaryunit;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/SecondaryUnit/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("UNIT updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/SecondaryUnit/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.update').css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            UnitID: $("#UnitID").val(),
            UnitGroupID: $("#UnitGroupID").val(),
            PackSize: $("#PackSize").val()
        }
        return model;
    },
    validate_form: function () {
        var self = secondaryunit;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#QOM",
                rules: [
                   { type: form.required, message: "Quantity of Measurement is required" },
                ]
            },
             {
                 elements: "#Name",
                 rules: [
                    { type: form.required, message: "Unit Name is required" },
                 ]
             },
             {
                 elements: "#UOM",
                 rules: [
                     {type: form.required ,message: "Unit of Measurement is Required"},
                 ]
             },
            

        ]
    }
}

