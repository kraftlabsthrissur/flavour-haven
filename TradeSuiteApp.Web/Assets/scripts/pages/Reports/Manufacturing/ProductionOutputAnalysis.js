$(function () {
    ProductionOutputAnalysis.init();
});

ProductionOutputAnalysis = {
    init: function () {
        var self = ProductionOutputAnalysis;
        self.bind_events();
        ReportHelper.init();
        
    },

    bind_events: function () {
        var self = ProductionOutputAnalysis;
        $('#production-groupname-autocomplete').on('selectitem.uk.autocomplete', self.set_production_group_name);
        $('#production-batchno-autocomplete').on('selectitem.uk.autocomplete', self.set_production_batch_no);
        $('#Refresh').on('click', self.refresh);
    },

    set_production_group_name: function (event, item) {   // on select auto complete item
        self = ProductionOutputAnalysis;
        $("#ProductionGroupName").val(item.name);
        $("#ProductionGroupID").val(item.id);
    },

    set_production_batch_no: function (event, item) {   // on select auto complete item
        self = ProductionOutputAnalysis;
        $("#BatchNo").val(item.name);
        $("#BatchNoID").val(item.id);
    },

    get_filters: function () {
        var self = ProductionOutputAnalysis;
        var filters = " ";
        if (($("#PackingFromDateString").val() + $("#PackingToDateString").val()).trim() != "") {
            if ($("#PackingFromDateString").val().trim() == "" || $("#PackingToDateString").val().trim() == "") {
                filters += "Packing Date: " + $("#PackingFromDateString").val() + $("#PackingToDateString").val() + ", ";
            } else {
                filters += "Packing Date: " + $("#PackingFromDateString").val() + " - " + $("#PackingToDateString").val() + ", ";
            }
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;
    },
}