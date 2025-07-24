$(function () {
    ProductionWorkInProgress.init();
});

ProductionWorkInProgress = {
    init: function () {
        var self = ProductionWorkInProgress;
        self.bind_events();
        ReportHelper.init();
    },
    bind_events: function () {
        var self = ProductionWorkInProgress;
        $('#Refresh').on('click', self.refresh);
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_no);
        $('#production-groupname-autocomplete').on('selectitem.uk.autocomplete', self.set_production_groupname);
    },

    refresh: function () {
        var self = ProductionWorkInProgress;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $('#FromDateString').val(findate)
        $('#ToDateString').val(currentdate)
        $('#ProductionGroupName').val('')
        $('#ProductionGroupID').val('');
        $('#BatchNo').val('');
        $('#BatchID').val('');
        $('#ProductionCategoryID').val('');
        $('#SalesCategoryID').val('');
    },

    set_batch_no: function (event, item) {
        var self = ProductionWorkInProgress;
        $("#BatchID").val(item.id)
    },

    set_production_groupname: function (event, item) {
        var self = ProductionWorkInProgress;
        $("#ProductionGroupID").val(item.id)
    },

    get_filters : function(){
        var self = ProductionWorkInProgress;
        var filters = " ";
        if ($('#ProductionGroupName').val() != "") {
            filters += 'Production Group Name: ' + $('#ProductionGroupName').val() + ", ";
        }
        if ($('#BatchNo').val() != "") {
            filters += 'Batch : ' + $('#BatchNo').val() + ", ";
        }
        if ($('#ProductionCategoryID').val() != 0) {
            filters += 'Production Category: ' + $('#ProductionCategoryID Option:selected').text() + ", ";
        }
        if ($('#SalesCategoryID').val() != 0) {
            filters += 'Sales Category : ' + $('#SalesCategoryID Option:selected').text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    }


}