$(function () {
    district.district_list();
    district.bind_events();
})

district = {
    district_list: function () {
        $district_list = $('#district-list');
        if ($district_list.length) {
            $district_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#district-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/District/Details/' + Id;
            });
            altair_md.inputs();
            var district_list_table = $district_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            district_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    district_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        $(".btnUpdate").on('click', district.update);
        $(".btnSave").on('click', district.save_confirm);
    },

    save_confirm: function () {
        var self = district;
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
        var self = district;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/District/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("District created successfully");
                    window.location = "/Masters/District/Index";
                    setTimeout(function () {
                       
                    }, 1000);
                } else {
                    app.show_error("District Already Exist");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    update: function () {
        var self = district;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/District/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("District updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/District/Index";
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
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            StateID: $("#StateID").val(),
            OfficeName: $("#OfficeName").val(),
            PIN: $("#PIN").val(),
            Taluk:$("#Taluk").val(),
        }
        return model;
    },

    validate_form: function () {
        var self = district;
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
                    { type: form.required, message: "District Name is Required" },
                 ]
             },
             {
                 elements: "#StateID",
                 rules: [
                     { type: form.required, message: "State Name is Required" },
                 ]
             },

             {
                 elements: "#OfficeName",
                 rules: [
                     { type: form.required, message: "Office Name is Required" },
                 ]
             },

             {
                 elements: "#PIN",
                 rules: [
                     { type: form.required, message: "PIN is Required" },
                 ]
             },

             {
                 elements: "#Taluk",
                 rules: [
                     { type: form.required, message: "Taluk is Required" },
                 ]
             },
        ]
    }
}