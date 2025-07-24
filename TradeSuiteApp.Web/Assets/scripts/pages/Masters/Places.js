//file created by prama on 6-6-18
$(function () {
    places.places_list();
    places.bind_events();
});
places = {
    places_list: function () {
        $places_list = $('#places-list');
        if ($places_list.length) {
            $places_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#places-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Places/Details/' + Id;
            });
            altair_md.inputs();
            var places_list_table = $places_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
            });

            $places_list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                places_list_table.api().column(index).search(this.value).draw();
            });

        }
    },
    bind_events: function () {
        $("#StateID").on('change', places.GetDisitrict);
        $(".btnSave").on("click", places.save_confirm);
        $(".btnUpdate").on("click", places.update_confirm);
    },

    save_confirm: function () {
        var self = places;
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
    update_confirm: function () {
        var self = places;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Update", function () {
            self.update();
        }, function () {
        })
    },

    save: function () {
        var self = places;
      
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        var location = "/Masters/Places/Index";
        $.ajax({
            url: '/Masters/Places/Save/',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "Success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);

                }
            }
        });
    },
    update: function () {
        var self = places;
        $('.btnUpdate').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Places/Update',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Places updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Places/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function () {
        var self = places;
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Address: $("#Address").val(),
            DistrictID: $("#DistrictID").val(),
            District: $('#DistrictID :selected').text(),
            StateID: $("#StateID").val(),
            State: $('#StateID :selected').text(),
            CountryID: $("#CountryID").val(),
            Country: $('#CountryID :selected').text(),
        };
        return model;
    },
    GetDisitrict: function () {
        var state = $(this);
        $.ajax({
            url: '/Masters/District/GetDistrict/',
            dataType: "json",
            type: "GET",
            data: {
                StateID: state.val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                });
            }
        });
    },
    validate_form: function () {
        var self = places;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
    },
    rules: {
        on_submit: [
             {
                 elements: "#Code",
                 rules: [
                     { type: form.required, message: "Please enter code" }
                 ]
             },
             {
                 elements: "#Name",
                 rules: [
                      { type: form.required, message: "Please enter Name" }
                 ]
             },
             {
                 elements: "#StateID",
                 rules: [
                      { type: form.required, message: "Please select State" }
                 ]
             },
             {
                 elements: "#DistrictID",
                 rules: [
                      { type: form.required, message: "Please select District" }
                 ]
             },
             {
                 elements: "#CountryID",
                 rules: [
                      { type: form.required, message: "Please select Country" }
                 ]
             },
        ]
    }
}