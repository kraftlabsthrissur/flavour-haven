Categories = {
    init: function () {
        var self = Categories;
        self.bind_events();
    },
    list: function () {
        categories_list = $('#categories-list');
        if (categories_list.length) {
            categories_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#categories-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                var CategoryType = $(this).find("td:eq(0) .CategoryType").val();
                window.location = '/Masters/Categories/Details/' + Id + '?CategoryType=' + CategoryType;
            });
            altair_md.inputs();
            var categories_list_table = categories_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
            });
            categories_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    categories_list_table.api().column(index).search(this.value).draw();
                });
            });
        }

    },
    bind_events: function () {
        var self = Categories
        $(".btnSave").on('click', self.save);
        $(".btnEdit").on('click', self.Edit);
    },
        save: function () {
        var self = Categories;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Categories/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Category created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Categories/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var self = Categories;
        var modal = {
            ID: $('#ID').val(),
            CategoryType: $('#CategoryType').val(),
            CategoryName: $('#CategoryName').val(),
        }
        return(modal);

    },
    validate_form: function () {
        var self = Categories;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
    },
    rules: {
        on_submit: [
            {
                elements: "#CategoryName",
                rules: [
                   { type: form.required, message: "CategoryName  is Required" },
                ]
            },
        ]
    },
    Edit: function () {
        var self = Categories;
        var Id = $('#ID').val();
        var CategoryType = $('#CategoryType').val();
        window.location = '/Masters/Categories/Edit/' + Id + '?CategoryType=' + CategoryType;
    }
}
