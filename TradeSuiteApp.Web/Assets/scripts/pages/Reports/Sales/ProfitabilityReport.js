$(function () {
    ProfitabilityReport.init();
});

ProfitabilityReport = {
    init: function () {
        var self = ProfitabilityReport;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = ProfitabilityReport;
        $('#suppliername-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_name);
        $.UIkit.autocomplete($('#invoice-invoiceno-autocomplete'), { 'source': self.get_invoiceno, 'minLength': 1 });
        $('#invoice-invoiceno-autocomplete').on('selectitem.uk.autocomplete', self.set_invoiceno);
        $('#Refresh').on('click', self.refresh);
    },
    get_invoiceno: function (release) {
        self = ProfitabilityReport;
        var type = $(".invoice_type:checked").val();

        var Table;
        Table = 'PurchaseInvoice';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#InvoiceNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_invoiceno: function (event, item) {
        self = ProfitabilityReport;
        $("#InvoiceNo").val(item.code);
        $("#InvoiceNoID").val(item.id);
    },
    refresh: function () {
        var self = ProfitabilityReport;
        $('#FromDate').val('');
        $('#ToDate').val('');
    },
    set_supplier_name: function (event, item) {   // on select auto complete item
        self = ProfitabilityReport;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },
    get_filters: function () {
        var self = ProfitabilityReport;
        var filters = "";
        if ($("#FromDateString").val() != " ") {
            filters += "From Date: " + $("#FromDateString").val() + ", ";
        }
        if ($("#ToDateString").val() != 0) {
            filters += "To Date: " + $("#ToDateString").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
}