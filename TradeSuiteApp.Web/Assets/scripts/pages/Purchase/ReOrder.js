ReOrder = {
    init: function () {
        var self = ReOrder;
        self.bind_events();
        self.get_filter();
    },

    bind_events: function () {
        var self = ReOrder;
        $("#btnView").on("click", self.get_filter);
        $("body").on('ifChanged', '.chkCheck', self.include_item);
        $("body").on('click', '.btnSave', self.save_confirm);
    },
    get_filter: function () {
        var self = ReOrder;
        //var Type = $("input[name='filtertype']:checked").val();
        //if (self.validate_on_add() > 0) {
        //    return;
        //}
        //var length;
        
        var ReOrderDays = clean($("#ReOrderDays").val());
        var OrderDays = clean($("#OrderDays").val());
        var ItemType = $("#ItemType").val()
        $.ajax({
            url: '/Purchase/ReOrder/ReOrderList',
            dataType: "html",
            data: {
                ReOrderDays: ReOrderDays,
                OrderDays: OrderDays,
                ItemType: ItemType
            },
            type: "POST",
            success: function (response) {
                $("#re-order-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#re-order-list tbody").append($response);
                index = $("#re-order-list tbody tr.included").length;
                $("#item-count").val(index);
                if ($("#re-order-list tbody tr").length <= 0) {
                    app.show_error("No ReOrders Exist");
                }
            },
        })
    },

    include_item: function () {
        var self = ReOrder;
        if ($(this).is(":checked")) {
            $(this).closest('tr').addClass('included');
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').attr("disabled", false);
            $(this).closest('tr').find(".IsOrdered").val(true);
        } else {
            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').attr("disabled", false);
            $(this).closest('tr').find(".IsOrdered").val(false);
        }
        index = $("#re-order-list tbody tr.included").length;
        $("#item-count").val(index);
    },

    get_data: function () {
        var self = ReOrder;
        var data = {};
        data.ReOrderDays = $("#ReOrderDays").val();
        data.OrderDays = $("#OrderDays").val();
        data.ItemType = $("#ItemType").val();
        data.Items = [];
        var item = {};
        $("#re-order-list tbody tr").each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val());
            item.UnitID = clean($(this).find(".UnitID").val());
            item.SupplierID = $(this).find(".SupplierID").val();
            item.Qty = clean($(this).find(".Qty").val());
            item.ReOrderQty = clean($(this).find(".ReOrderQty").val());
            item.IsOrdered = $(this).find(".IsOrdered").val();
            item.LastPurchasedDate = $(this).find(".LastPurchasedDate").val();
            item.Stock = clean($(this).find(".Stock").val());
            item.Rate = clean($(this).find(".Rate").val());
            item.LastPurchaseQty = clean($(this).find(".LastPurchaseQty").val());
            item.LastPurchaseOfferQty = clean($(this).find(".LastPurchaseOfferQty").val());
            data.Items.push(item);
        });
        return data;
    },

    save: function () {
        var self = ReOrder
        var modal = self.get_data();
        $.ajax({
            url: '/Purchase/ReOrder/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("ReOrder created successfully");                   
                        setTimeout(function () {
                            window.location = "/Purchase/PurchaseOrder/Index";
                        }, 1000);
                    } else {
                        app.show_error(data.Message);
                        $('.btnSave').css({ 'visibility': 'visible' });
                    }
                }
           
        });
    },

    save_confirm: function () {
        var self = ReOrder;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    validate_save: function () {
        var self = ReOrder;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {

        on_add: [
             {
                 elements: "#ItemID",
                 rules: [
                     { type: form.required, message: "Please choose an item", alt_element: "#ItemName" },
                     { type: form.positive, message: "Please choose an item", alt_element: "#ItemName" },
                     { type: form.non_zero, message: "Please choose an item", alt_element: "#ItemName" },
                     {
                         type: function (element) {
                             var ItemID = clean($("#ItemID").val());
                             var error = false;
                             $('#purchase-order-items-list tbody tr').each(function () {
                                 if ($(this).find('.ItemID').val() == ItemID) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "already added in the grid, try editing Qty"
                     },
                 ],
             },
             {
                 elements: "#UnitID",
                 rules: [
                     { type: form.required, message: "Please choose an unit" },
                     { type: form.positive, message: "Please choose an unit" },
                     { type: form.non_zero, message: "Please choose an unit" }
                 ],
             },

             {
                 elements: "#Qty",
                 rules: [
                     { type: form.required, message: "Please Fill Quantity" },
                     { type: form.positive, message: "Please Fill Quantity" },
                     { type: form.non_zero, message: "Please Fill Quantity" }
                 ],
             },
              {
                  elements: "#Rate",
                  rules: [
                      { type: form.required, message: "please fill rate" },
                      { type: form.positive, message: "please fill rate" },
                      { type: form.non_zero, message: "please fill rate" }
                  ],
              },
        ],
        on_save: [
              {
                  elements: "#item-count",
                  rules: [
                     { type: form.required, message: "Please Choose an Item" },
                     { type: form.non_zero, message: "Please Choose an Item" },
                  ]
              },
        ]
    },

}