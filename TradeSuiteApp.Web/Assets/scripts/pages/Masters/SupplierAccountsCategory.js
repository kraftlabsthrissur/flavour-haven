$(function () {
    supplieraccountscategory.list();
    supplieraccountscategory.bind_events();
});

supplieraccountscategory = {

    list: function () {
        $list = $('#Supplier-accountCategory-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Supplier-accountCategory-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/SupplierAccountsCategory/Details/' + Id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        $(".btnSave").on("click", supplieraccountscategory.save_confirm);
        // $(".btnUpdate").on("click", suppliercategory.update);
    },

    save_confirm: function () {
        var self = supplieraccountscategory
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = supplieraccountscategory;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Masters/SupplierAccountsCategory/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Supplier Accounts Category Saved Successfully");
                    window.location = "/Masters/SupplierAccountsCategory/Index";;
                }
                else {
                    app.show_notice("Already Exists");
                }

            }
        });
    },

    get_data: function () {
        var self = supplieraccountscategory;
        var data = {};
        data.ID = $("#ID").val(),
        data.Name = $("#Name").val()
        return data;

    },

    validate_form: function () {
        var self = supplieraccountscategory;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter a Name" }
                ],
            },

        ],
    },

}