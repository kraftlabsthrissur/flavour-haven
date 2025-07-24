$(function () {
    GSTCategory.init();  
});
GSTCategory = {
    init: function () {
        var self = GSTCategory;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.list();
        }
        self.bind_events();
    },
    list: function () {
        var $list = $('#gst-category-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#gst-category-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/GSTCategory/Details/' + id;
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
        var self = GSTCategory;
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = GSTCategory
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = GSTCategory;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/GSTCategory/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("GSTCategory created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/GSTCategory/Index";
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
            Name: $("#Name").val(),
            SGSTPercent: $("#SGSTPercent").val(),
            CGSTPercent: $("#CGSTPercent").val(),
            IGSTPercent: $("#IGSTPercent").val(),

        }
        return model;
    },
    validate_form: function () {
        var self = GSTCategory;
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
                     { type: form.required, message: "Name is Required" },
                 ]
             },
        ]
    },
}