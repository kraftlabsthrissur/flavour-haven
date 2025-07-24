GSTSubCategory = {
    init: function () {
        var self = GSTSubCategory;
        self.bind_events();
    },
    list: function () {
        gst_sub_categories_list = $('#gst-sub-categories-list');
        if (gst_sub_categories_list.length) {
            gst_sub_categories_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#gst-sub-categories-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                var CategoryType = $(this).find("td:eq(0) .CategoryType").val();
                window.location = '/Masters/GSTSubCategory/Details/' + Id ;
            });
            altair_md.inputs();
            var gst_sub_categories_list_table = gst_sub_categories_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
            });
            gst_sub_categories_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    gst_sub_categories_list_table.api().column(index).search(this.value).draw();
                });
            });
        }

    },
    bind_events: function () {
        var self = GSTSubCategory;
        $('.btnSave').on('click',self.Save)

    },
    Save: function () {
        var self = GSTSubCategory;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/GSTSubCategory/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("GST Sub Category created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/GSTSubCategory/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var self = GSTSubCategory;
        var modal = {
            ID: $("#ID").val(),
            Name: $('#Name').val(),
            Description: $('#Description').val(),
            Percentage: $('#Percentage').val()
        }
        return modal;

    },
    validate_form: function () {
        var self = GSTSubCategory;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
    },
    rules: {
        on_submit: [
            {
                elements: "#Name",
                rules: [
                   { type: form.required, message: "Category Name  is Required" },
                ]
            },
            {
                elements: "#Description",
                rules: [
                   { type: form.required, message: "Description  is Required" },
                ]
            },
            {
                elements: "#Percentage",
                rules: [
                   { type: form.required, message: "Percentage  is Required" },
                ]
            },
        ]
    },
}