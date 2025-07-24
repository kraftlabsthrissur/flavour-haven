Manufacturer = {
    init: function () {
        var self = Manufacturer;
        self.bind_events();
    },
    list: function () {
        var $list = $('#manufacturer_list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#manufacturer_list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/Manufacturer/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        var self = Manufacturer;
        $(".btnSave").on('click', self.save_confirm);
    },
    save_confirm: function () {
        var self = Manufacturer;
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
    validate_form: function () {
        var self = Manufacturer;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    save: function () {
        var self = Manufacturer;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Manufacturer/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Manufacturer created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Manufacturer/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },

    get_data: function () {
        var self = Manufacturer;
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            StateID: $("#StateID").val(),
            Name: $("#Name").val(),
            AddressLine1: $("#AddressLine1").val(),
            AddressLine2: $("#AddressLine2").val(),
            Place: $("#Place").val(),
            Description: $("#Description").val(),
            Phone: $("#Phone").val()
        }
        return model;
    },
    rules: {
        on_submit: [
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: "Manufacturer Name is Required" },
                 ]
             },
             {
                 elements: "#StateID",
                 rules: [
                     { type: form.required, message: "Select State" },
                 ]
             },
              {
                  elements: "#Phone",
                  rules: [
                      { type: form.required, message: "PhoneNumber  is Required" },
                  ]
              },
               {
                   elements: "#AddedDate",
                   rules: [
                       { type: form.required, message: "Date  is Required" },
                   ]
               },
                 {
                     elements: "#Place",
                     rules: [
                         { type: form.required, message: "Place  is Required" },
                     ]
                 },
        ]
    },
}