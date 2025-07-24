$(function () {
    state.state_list();
    state.bind_events();
});
state = {
    state_list: function () {
        $state_list = $('#state-list');
        if ($state_list.length) {
            $state_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#state-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/State/Details/' + Id;
            });
            altair_md.inputs();
            var state_list_table = $state_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            state_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    state_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click', state.update);
        $(".btnSave").on('click', state.save_confirm);
    },

    save_confirm: function () {
        var self = state;
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
    save: function () {
        var self = state;
      
        $('.btnSave').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/State/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("STATE created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/State/Index";
                    }, 1000);
                } else {
                    app.show_error("Already Exists.");
                    $('.update').css({ 'display': 'block' });
                }
            
            },
        });
    },

    update: function () {
        var self = state;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/State/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("STATE updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/State/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to Update data.");
                    $('.update').css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            GstState: $("#GstState").val(),
            CountryID: $("#CountryID").val()
        }
        return model;
    },
    validate_form: function () {
        var self = state;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#GstState",
                rules: [
                   { type: form.required, message: "GST State is required" },
                ]
            },
             {
                 elements: "#Name",
                 rules: [
                    { type: form.required, message: "State Name is required" },
                 ]
             },
              {
                  elements: "#CountryID",
                  rules: [
                     { type: form.required, message: "Country is required" },
                  ]
              },
        ]
    }

};