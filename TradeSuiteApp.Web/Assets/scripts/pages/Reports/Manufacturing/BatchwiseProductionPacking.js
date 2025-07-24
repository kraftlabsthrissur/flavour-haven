$(function () {
    BatchwiseProductionPacking.init();
});

BatchwiseProductionPacking = {
    init: function(){
        var self = BatchwiseProductionPacking;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = BatchwiseProductionPacking;
        $('#Refresh').on('click', self.refresh);
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
        $('#item-name-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batchNo);
    },

    set_item_code_from: function (event,item) {
        var self = BatchwiseProductionPacking;
        $('#ItemID').val(item.id);
    },

    set_item: function (event, item) {
        var self = BatchwiseProductionPacking;
        $('#ItemID').val(item.id);
    },

    set_batchNo: function (event, item) {
        var self = BatchwiseProductionPacking;
        $('#BatchID').val(item.id);
    },

    get_filters: function () {
        var self = BatchwiseProductionPacking;
        var filters = "";
        if ($("#ItemID").val() != 0)
        {
            if ($("#ItemCode").val() != "") {
                filters += "Item code: " + $("#ItemCode").val() + ", ";
            }
            else {
                filters += "Item Name: " + $("#ItemName").val() + ", ";
            }
        }
        if ($("#SalesCategoryID").val() != "") {
            filters += "Sales Category: " + $("#SalesCategoryID Option:selected").text() + ", ";
        }
        if ($("#BatchNo").val() != "") {
            filters += "Batch No: " + $("#BatchNo").val() + ", ";
        }
        if ($("#BatchTypeID").val() != "") {
            filters += "Batch Type: " + $("#BatchTypeID Option:selected").text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },

    refresh: function () {
        var self = BatchwiseProductionPacking;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $('#FromDateString').val(findate);
        $('#ToDateString').val(currentdate);
        $('#ItemID').val('');
        $('#ItemCode').val('');
        $('#ItemName').val('');
        $('#SalesCategoryID').val('');
        $('#BatchNo').val('');
        $('#BatchID').val('');
        $('#BatchTypeID').val('');
    }
}