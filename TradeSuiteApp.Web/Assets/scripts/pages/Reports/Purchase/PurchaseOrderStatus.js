
$(function () {
    PurchaseOrderStatus.init();
});

PurchaseOrderStatus = {
    init: function () {
        var self = PurchaseOrderStatus;
        self.bind_events();
        ReportHelper.init();
        //self.get_report_type();
    },
    bind_events: function () {
        var self = PurchaseOrderStatus;
        $('#suppliername-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_name);
        $('#pono-from-autocomplete').on('selectitem.uk.autocomplete', self.set_pono_from);
        $('#pono-to-autocomplete').on('selectitem.uk.autocomplete', self.set_pono_to);
    },
    set_supplier_name: function (event, item) {   // on select auto complete item
        self = PurchaseOrderStatus;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },
    set_pono_from: function (event, item) {
        self = PurchaseOrderStatus;
        $("#PONOFrom").val(item.code);
        $("#PONOFromID").val(item.id);
    },
    set_pono_to: function (event, item) {
        self = PurchaseOrderStatus;
        $("#PONOTo").val(item.code);
        $("#PONOToID").val(item.id);
    },

    get_filters: function () {
        var self = PurchaseOrderStatus;
        var filters = "";
        var data = "";
        if ($("#ItemLocationID").val() != "") {
            filters += "Location: " + $("#ItemLocationID Option:selected").text() + ", ";
        }
        
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return data += filters;
    },

}