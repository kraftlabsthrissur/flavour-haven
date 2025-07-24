$(function () {
    country.country_list();
    country.bind_events();
});
country = {
    country_list: function () {
        $country_list = $('#country-list');
        if ($country_list.length) {
            $country_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#country-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Country/Details/' + Id;
            });
            altair_md.inputs();
            var country_list_table = $country_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            country_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    country_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click', country.update);
        $(".btnSave").on('click', country.save_confirm);
        $("body").on('mouseover', "#country-list tbody tr", country.country_tooltip);
    },
    country_tooltip: function () {
        $(this).attr('title', 'Country List');
    },
    save_confirm: function () {
        var self = country;
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
        var self = country;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Country/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Country created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Country/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    update: function () {
        var self = country;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Country/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Country updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Country/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'visibility': 'visible' });
                }
            },
        });
    },


    get_data: function () {
        var model = {
            Id: $("#Id").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            IsActive: $("#IsActive").is(":checked"),
            IsIntraCountry: $("#IsIntraCountry").is(":checked"),
        }
        return model;
    },
    validate_form: function () {
        var self = country;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Code is required" },
                ]
            },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Name is required" },
                ]
            },
        ]
    }

};