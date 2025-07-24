XrayTest = {
    init: function () {
        var self = XrayTest;
        self.bind_events();
    },
    list: function () {
        var $list = $('#xray_test_list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#xray_test_list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/XrayTest/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
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
        var self = XrayTest;
        $(".btnSave").on('click', self.save_confirm);
    },
    save_confirm: function () {
        var self = XrayTest;
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
        var self = XrayTest;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/XrayTest/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Xray created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/XrayTest/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },
    get_data: function () {
        var self = XrayTest;
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            AddedDate: $("#AddedDate").val(),
            Description: $("#Description").val(),
            PurchaseCategoryID: $("#PurchaseCategoryID").val(),
            QCCategoryID: $("#QCCategoryID").val(),
            GSTSubCategoryID: $("#GSTSubCategoryID").val(),
            SalesCategoryID: $("#SalesCategoryID").val(),
            SalesIncentiveCategoryID: $("#SalesIncentiveCategoryID").val(),
            StorageCategoryID: $("#StorageCategoryID").val(),
            ItemTypeID: $("#ItemTypeID").val(),
            AccountsCategoryID: $("#AccountsCategoryID").val(),
            BusinessCategoryID: $("#BusinessCategoryID").val(),
            ItemUnitID: $("#ItemUnitID").val(),
            CategoryID: $("#CategoryID").val()
        }
        return model;
    },
    validate_form: function () {
        var self = XrayTest;
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
             }
        ]
    },
}