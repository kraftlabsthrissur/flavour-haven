$(function () {
    SalesOrder.init();
});

SalesOrder = {
    init: function () {
        var self = SalesOrder;
        self.bind_events();
        ReportHelper.init();
        self.get_report_type();
    },

    bind_events: function () {
        var self = SalesOrder;
        $('.report-type').on('ifChanged', self.get_report_type);
        $('.item-type').on('ifChanged', self.on_item_type_change);
        $('#Refresh').on('click', self.refresh);

        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer_name);
        $('#customer-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_customer_code_from);
        $('#customer-code-to-autocomplete').on('selectitem.uk.autocomplete', self.set_customer_code_to);

        $('#item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
        $('#item-code-to-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_to);

        $('#invoice-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_invoice_no_from);
        $('#invoice-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_invoice_no_to);

        $('#sales-order-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_sales_order_no_from);
        $('#sales-order-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_sales_order_no_to);
    },

    get_report_type: function () {
        var self = SalesOrder;
        var report_type = $(".report-type:checked").val();
        $('.filters').addClass('uk-hidden');
        $("." + report_type).removeClass('uk-hidden');
        self.refresh();
    },

    on_item_type_change: function () {
        var self = SalesOrder;
        var type = $(this).val();
        if (type == "stock") {
            $('#search-item-type').val("saleablestockitem");
        } else {
            $('#search-item-type').val("saleableserviceitem");
        }
        
    },

    get_item_category: function () {
        var self = SalesOrder;
    },

    set_customer_name: function (event, item) {
        var self = SalesOrder;
        $('#CustomerID').val(item.id);
    },

    set_customer_code_from: function (event, item) {
        var self = SalesOrder;
        $('#CustomerFromID').val(item.id);
    },

    set_customer_code_to: function (event, item) {
        var self = SalesOrder;
        $('#CustomerToID').val(item.id);
    },

    set_item_code_from: function (event, item) {
        var self = SalesOrder;
        $('#ItemFromID').val(item.id);
    },

    set_item_code_to: function (event, item) {
        var self = SalesOrder;
        $('#ItemToID').val(item.id);
    },

    set_item: function (event, item) {
        var self = SalesOrder;
        $('#ItemID').val(item.id);
    },

    set_sales_order_no_from: function (event, item) {
        self = SalesOrder;
        $("#SalesOrderFromID").val(item.id);
    },

    set_sales_order_no_to: function (event, item) {
        self = SalesOrder;
        $("#SalesOrderToID").val(item.id);
    },

    get_filters: function () {
        var self = SalesOrder;
        var filters = "";
        if (($("#SalesOrderNoFrom").val() + $("#SalesOrderNoTo").val()).trim() != "") {
            if ($("#SalesOrderNoFrom").val().trim() == "" || $("#SalesOrderNoTo").val().trim() == "") {
                filters += "Order No: " + $("#SalesOrderNoFrom").val() + $("#SalesOrderNoTo").val() + ",";
            }
            else {
                filters += "Order No: " + $("#SalesOrderNoFrom").val() + " - " + $("#SalesOrderNoTo").val() + ",";
            }
        }
        if ($("#CustomerCodeFrom").val() + $("#CustomerCodeTo").val().trim() != "") {
            if ($("#CustomerCodeFrom").val().trim() == "" || $("#CustomerCodeTo").val().trim() == "") {
                filters += "Cust.Code: " + $("#CustomerCodeFrom").val() + $("#CustomerCodeTo").val() + " , ";
            } else {
                filters += "Cust.Code: " + $("#CustomerCodeFrom").val() + " - " + $("#CustomerCodeTo").val() + " , ";
            }
        }
        if ($("#CustomerID").val() == "") {
            if ($("#FromCustomerRange").val() != "")
                filters += "Customer name from " + $("#FromCustomerRange").val();

            if ($("#ToCustomerRange").val() != "") {
                filters += " and " + $("#ToCustomerRange").val();
            }
        }
        else {
            filters += "customer: " + $("#CustomerName").val() + ", ";
        }
        if ($("#LocationID").val() != "") {
            filters += "Location: " + $("#LocationID Option:selected").text() + ", ";
        }
        if ($("#ItemCategoryID").val() != 0) {
            filters += "Item category: " + $("#ItemCategoryID Option:selected").text() + ", ";
        }
        if ($("#ItemCodeFrom").val() + $("#ItemCodeTo").val().trim() != "")
            if ($("#ItemCodeFrom").val().trim() != "" || $("#ItemCodeTo").val().trim() != "") {
                filters += "Item code: " + $("#ItemCodeFrom").val() + $("#ItemCodeTo").val() + ", ";
            } else {
                filters += "Item code: " + $("#ItemCodeFrom").val() + " - " + $("#ItemCodeTo").val() + ", ";
            }
        if ($("#ItemID").val() != "") {
            filters += "Item: " + ($("#ItemName").val()) + ", ";
        }
        if ($("#Status").val() != "") {
            filters += "Status: " + $("#Status Option:selected").text();
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },

    refresh: function () {
        var self = SalesOrder;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $('#FromDateString').val(findate)
        $('#ToDateString').val(currentdate)
        $('#CustomerName').val('');
        $('#CustomerID').val('');
        $('#CustomerCodeFrom').val('');
        $('#CustomerCodeTo').val('');
        $('#CustomerFromID').val('');
        $('#CustomerToID').val('');
        $('#FromCustomerRange').val('');
        $('#ToCustomerRange').val('');
        $('#SalesOrderNoFrom').val('');
        $('#SalesOrderNoTo').val('');
        $('#SalesOrderFromID').val('');
        $('#SalesOrderToID').val('');
        $('#ItemCategoryID').val('');
        $('#ItemID').val('');
        $('#ItemName').val('');
        $('#ItemFromID').val('');
        $('#ItemToID').val('');
        $('#ItemCodeFrom').val('');
        $('#ItemCodeTo').val('');
    },

}
