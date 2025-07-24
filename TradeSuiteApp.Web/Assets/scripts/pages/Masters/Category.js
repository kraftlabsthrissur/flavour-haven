$(function () {
    category.category_list();
    category.bind_events();
})
category = {
    category_list: function () {
        $category_list = $('#category-list');
        if ($category_list.length) {
            $category_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#category-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Category/Details/' + Id;
            });
            altair_md.inputs();
            var category_list_table = $category_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
            });
            $category_list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                category_list_table.api().column(index).search(this.value).draw();
            });

        }
    },
    bind_events: function () {
        var self = category;
        $(".btnUpdate").on('click', self.update_confirm);
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = category;
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
        var self = category;
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

    error_count: 0,
    save: function () {
        var self = category;
        $('.btnSave').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Category/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("CATEGORY created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Category/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'display': 'block' });
                }
            },
        });
    },

    update: function () {
        var self = category;
        $('.btnUpdate').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Category/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("CATEGORY updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Category/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            CategoryGroupID: $("#CategoryGroupID").val(),
        }
        return model;
    },

    validate_form: function () {
        var self = category;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    error_count: 0,
    rules: {
        on_submit: [

             {
                 elements: "#Name",
                 rules: [
                    { type: form.required, message: "Category Name is Required" },
                 ]
             },
             {
                 elements: "#CategoryGroupID",
                 rules: [
                     { type: form.required, message: "Category Group is Required" },
                 ]
             },

        ]
    }
}