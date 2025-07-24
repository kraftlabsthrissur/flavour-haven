$(function () {
    Project.list();
    Project.bind_events();
});

Project = {

    list: function () {
        $list = $('#Project-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Project-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Project/Details/' + Id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });


        }
    },

    bind_events: function () {
        var self = Project;
        $(".btnSave").on("click", Project.save_confirm);
    },

    save_confirm: function () {
        var self = Project;
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
        var self = Project;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/Project/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Project Saved Successfully");
                    window.location = "/Masters/Project/Index";;
                }
                else {
                    app.show_error("Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    get_data: function () {
        var self = Project;
        var data = {};
        data.ID = $("#ID").val(),
        data.Code = $("#Code").val(),
        data.Name = $("#Name").val(),
        data.Description = $("#Description").val(),
        data.StartDate = $("#StartDate").val(),
        data.EndDate = $("#EndDate").val()
        return data;
    },

    validate_form: function () {
        var self = Project;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please enter a Code" }
                ],
            },

            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter a Name" }
                ],
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

        ],
    },
}