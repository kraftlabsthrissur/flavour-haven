DiscountCategory = {
    init: function () {
        var self = DiscountCategory;
        self.bind_events();
        self.is_cash_discount();
    },

    list: function () {
        var $list = $('#Discount-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
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
        var self = DiscountCategory;
        $("#DiscountType").on("change", self.is_cash_discount);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on('click', '#Discount-list tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/DiscountCategory/Details/' + Id;
        });
    },
    is_cash_discount: function () {
        var DiscountType = $("#DiscountType").val();
        if (DiscountType == "CashDiscount") {
            $("#Days").removeAttr("disabled").addClass("enabled");
        }
        else {
            $("#Days").attr("disabled", "disabled").removeClass("enabled");
        }
    },

    save_confirm: function () {
        var self = DiscountCategory;
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

    get_data: function () {
        var self = DiscountCategory;
        var data = {};
        data.ID = $("#ID").val(),
        data.DiscountPercentage = clean($("#DiscountPercentage").val());
        data.DiscountType = $("#DiscountType").val();
        data.Days = clean($("#Days").val());
        return data;
    },

    save: function () {
        var self = DiscountCategory;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/DiscountCategory/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("DiscountCategory Saved Successfully");
                    window.location = "/Masters/DiscountCategory/Index";
                }
                else {
                    app.show_error("DiscountCategory Already Exist");
                }
            }
        });
    },

    validate_form: function () {
        var self = DiscountCategory;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
            {
                elements: "#DiscountPercentage",
                rules: [
                    { type: form.required, message: "Please enter discountPercentage", },
                    { type: form.positive, message: "Please enter discountPercentage", },
                    { type: form.non_zero, message: "Please enter discountPercentage", }
                ],
            },
            {
                elements: "#DiscountType",
                rules: [
                    { type: form.required, message: "Please enter a DiscountType" }
                ],
            },
           {
               elements: "#Days.enabled",
               rules: [
                   { type: form.required, message: "Please enter Days", },
                   { type: form.positive, message: "Please enter Days", },
                   { type: form.non_zero, message: "Please enter Days", }
               ]
           }

        ],
    }
}
