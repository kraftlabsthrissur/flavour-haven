Designation = {
    init: function () {
        var self = Designation;
        self.bind_events();
    },
    list: function () {
        var self = Designation;
        var $list = $('#designation-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#designation-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/Designation/Details/' + id;
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
        var self = Designation;
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = Designation
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = Designation;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Designation/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Designation created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Designation/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            DepartmentID: $("#DepartmentID").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            IsActive: $("#IsActive").is(":checked") ? true : false,

        }
        return model;
    },
    validate_form: function () {
        var self = Designation;
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
                    { type: form.required, message: "Code is Required" },
                 ]
             },
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: " Name is Required" },
                 ]
             },

             {
                 elements: "#DepartmentID",
                 rules: [
                     { type: form.required, message: " Department is Required" },
                 ]
             },
             {
                 elements: "#EndDate",
                 rules: [

             {
                 type: function (element) {
                     var u_date = $(element).val().split('-');
                     var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                     var a = Date.parse(used_date);
                     var po_date = $('#StartDate').val().split('-');
                     var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                     var date = Date.parse(po_datesplit);
                     return date <= a
                 }, message: "End date should be a date on or after start date"
             }

                 ]
             },
        ]
    },
}