$(function () {
    QCTest.list();
    QCTest.bind_events();
});


QCTest = {

    list: function () {
        $list = $('#QCTest-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#QCTest-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/QCTest/Details/' + Id;
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
        var self = QCTest;
        $(".btnSave").on("click", QCTest.save_confirm);
    },

    save_confirm: function () {
        var self = QCTest;
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
        var self = QCTest;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/QCTest/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/QCTest/Index";
                }
                else {
                    app.show_error("Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },

    get_data: function () {
        var self = QCTest;
        var data = {};
        data.ID = $("#ID").val(),
        data.TestName = $("#TestName").val(),
        data.Type = $("#Type").val()
        return data;
    },

    validate_form: function () {
        var self = QCTest;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
            {
                elements: "#TestName",
                rules: [
                    { type: form.required, message: "Please enter a Name" }
                ],
            },

            {
                elements: "#Type",
                rules: [
                    { type: form.required, message: "Please enter a Type" }
                ],
            },

        ],
    }
}