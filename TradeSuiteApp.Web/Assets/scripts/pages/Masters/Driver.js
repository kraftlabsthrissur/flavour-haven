
Driver = {
    init: function () {
        var self = Driver;
        self.bind_events();
    },
    list: function () {
        driver_list = $('#driver-list');
        if (driver_list.length) {
            driver_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#driver-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Driver/Details/' + Id;
            });
            altair_md.inputs();
            var driver_list_table = driver_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            driver_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    driver_list_table.api().column(index).search(this.value).draw();
                });
            });
        }

    },
    bind_events: function () {
        $(".btnSave").on('click', Driver.save)
    },
    save: function () {
        var self = Driver;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Driver/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Driver created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Driver/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var modal = {
            Id: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Address: $("#Address").val(),
            LicenseNo: $("#LicenseNo").val(),
            PhoneNo: $("#PhoneNo").val(),
            IsActive: $("#IsActive").val()
        }
        return modal;
    },
    validate_form: function () {
        var self = Driver;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [
             {
                 elements: "#Code",
                 rules: [
                    { type: form.required, message: "Code  is Required" },
                 ]
             },
             {
                 elements: "#Name",
                 rules: [
                    { type: form.required, message: "Name is Required" },
                 ]
             },
             {
                 elements: "#Address",
                 rules: [
                    { type: form.required, message: "Address is Required" },
                 ]
             },
             {
                 elements: "#PhoneNo",
                 rules: [
                    { type: form.required, message: "Phone is Required" },
                 ]
             },
             {
                 elements: "#LicenseNo",
                 rules: [
                    { type: form.required, message: "License No is Required" },
                 ]
             },
        ]
    }
}