CreditDays = {
    init: function () {
        var self = CreditDays;
        self.bind_events();
    },

    list: function () {
        var $list = $('#creditDays-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#creditDays-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/CreditDays/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
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
        var self = CreditDays;
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = CreditDays;
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

    save: function () {
        var self = CreditDays;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/CreditDays/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Credit Days created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/CreditDays/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create Credit Days");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            Days: $("#Days").val(),
        }
        return model;
    },

    validate_form: function () {
        var self = CreditDays;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: "Credit Days is Required" },
                 ]
             },
             {
                 elements: "#Days",
                 rules: [
                     { type: form.required, message: "Number of Days is Required" },
                 ]
             },
        ]
    },
}