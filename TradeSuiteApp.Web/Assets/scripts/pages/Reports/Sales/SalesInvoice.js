$(function () {
    SalesInvoice.init();
});

SalesInvoice = {
    init: function () {
        var self = SalesInvoice;
        self.bind_events();
        ReportHelper.init();
        self.get_report_type();
    },

    bind_events: function () {
        var self = SalesInvoice;
        $('.report_type').on('ifChanged', self.get_report_type);

        $('.ItemType').on('ifChanged', self.get_item_type);
        $('#Refresh').on('click', self.refresh);
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer_name);
        $('#customer-code-autocomplete').on('selectitem.uk.autocomplete', self.set_customer_code);
        $('#invoice-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_invoice_no_from);
        $('#invoice-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_invoice_no_to);
        $('#item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
    },

    get_report_type: function () {
        var self = SalesInvoice;
        var report_type = $(".report_type:checked").val();
        $('.filters').addClass('uk-hidden');
        $("." + report_type).removeClass('uk-hidden');
        var item_type= $(".ItemType:checked").val();     
        self.get_category(item_type);
        self.refresh();

    },

    get_item_type: function () {
        var self = SalesInvoice;
        var item_type = $(".ItemType:checked").val();
        self.get_category(item_type);
        self.refresh();

    },
    get_category: function (type) {
        var self = SalesInvoice;
        $.ajax({
            url: '/Masters/Category/GetItemCategories/',
            dataType: "json",
            type: "GET",
            data: {
                Type: type,
            },
            success: function (response) {
                $("#ItemCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#ItemCategoryID").append(html);
            }
        });
    },
    set_customer_name: function(event,item){
        var self = SalesInvoice;
        $('#CustomerID').val(item.id);
    },

    set_customer_code: function(event,item){
        var self = SalesInvoice;
        $('#CustomerID').val(item.id);
    },

    set_invoice_no_from: function(event,item){
        var self = SalesInvoice;
        $('#InvoiceNoFromID').val(item.id);
    },

    set_invoice_no_to: function(event,item){
        var self = SalesInvoice;
        $('#InvoiceNoToID').val(item.id);
    },

    get_filters: function () {
        var self = SalesInvoice;
        var filters = "";
        var data ="";
        if ($("#ItemLocationID").val() != "") {
            filters += "Location: " + $("#ItemLocationID Option:selected").text() + ", ";
        }
        if ($("#CustomerName").val() != "") {
            filters += "Customer: " + $("#CustomerName").val() + ", ";
        }
        if ($("#CustomerCodeFrom").val() != "") {
            filters += "Customer Code: " + $("#CustomerCodeFrom").val() + ", ";
        }
        if ($("#InvoiceNoFrom").val() + $("#InvoiceNoTo").val().trim() != "") {
            if ($("#InvoiceNoFrom").val().trim() == " " || $("#InvoiceNoTo").val().trim() == " ") {
                filters += "Invoice No: " + $("#InvoiceNoFrom").val() + $("#InvoiceNoTo").val() + ", ";
            }
            else {
                filters += "Invoice No: " + $("#InvoiceNoFrom").val() + " - " + $("#InvoiceNoTo").val() + ", ";
            }
        }

        if ($("#BatchTypeID").val() != "") {
            filters += "Batch Type: " + $("#BatchTypeID Option:selected").text() + ", ";
        }
        if ($("#StateID").val() != "") {
            filters += "State: " + $("#StateID Option:selected").text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return data +=  filters;
    },

    refresh: function (event, item) {
        var self = SalesInvoice;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $('#LocationID').val('');
        $('#CustomerName').val('');
        $('#CustomerID').val('');
        $('#CustomerCode').val('');
        $('#InvoiceNoFrom').val('');
        $('#InvoiceNoFromID').val('');
        $('#InvoiceNoTo').val('');
        $('#InvoiceNoToID').val('');
        $('#BatchTypeID').val('');
        $('#StateID').val('');
        $('#CustomerCodeFrom').val('');
       // $('#ItemCategoryID').val('');
    },

}
