$(function () {
    ProductionTargetVSActual.init();
});

ProductionTargetVSActual = {
    init: function () {
        var self = ProductionTargetVSActual;
        self.bind_events();
        ReportHelper.init();

    },

    bind_events: function () {
        var self = ProductionTargetVSActual;
        //$('#Refresh').on('click', self.refresh);
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
        $('#item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        //$('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batchNo);
    },

    set_item_code_from: function (event, item) {
        var self = ProductionTargetVSActual;
        $('#ItemCodeID').val(item.id);
    },

    set_item: function (event, item) {
        var self = ProductionTargetVSActual;
        $('#ItemID').val(item.id);
    },

    get_filters: function () {
        var self = ProductionTargetVSActual;
        //var reportype = $(".Stock_Valuation_ReportType:checked").val();
        var filters = "";

        if ($("#ItemCodeFrom").val() != 0) {
                filters += "Item Code: " + $("#ItemCodeFrom").val() + ", ";
        }

        if ($("#ItemID").val() != 0) {
            filters += "Item Name: " + $("#ItemName").val() + ", ";
        }

        if ($("#BatchTypeID").val() != 0) {
            filters += "Batch Type: " + $("#BatchTypeID option:selected").text() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;
    },
}