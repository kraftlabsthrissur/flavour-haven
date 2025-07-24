$(function () {
    TransferQuantity.init();
});

TransferQuantity = {
    init: function () {
        var self = TransferQuantity;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = TransferQuantity;
        $.UIkit.autocomplete($('#warehouse-from-autocomplete'), Config.get_nursing_station_list);
        $('#warehouse-from-autocomplete').on('selectitem.uk.autocomplete', self.set_warehouse_from);
        $.UIkit.autocomplete($('#warehouse-to-autocomplete'), Config.get_warehouse_list);
        $('#warehouse-to-autocomplete').on('selectitem.uk.autocomplete', self.set_warehouse_to);

        $('#Refresh').on('click', self.refresh);
    },
    set_warehouse_from: function (event, item) {
        var self = TransferQuantity;
        $("#NursingStationID").val(item.id);
        $("#NursingStation").val(item.name);
    },
    set_warehouse_to: function (event, item) {
        var self = TransferQuantity;
        $("#WareHouseToID").val(item.id);
        $("#WareHouseTo").val(item.name);
    },
    refresh: function () {
        var self = TransferQuantity;
        $('#NursingStationID').val('');
        $('#NursingStation').val('');
        $('#WareHouseToID').val('');
        $('#WareHouseTo').val('');

    },
    get_filters: function () {
        var self = TransferQuantity;
        var filters = "";
        if ($("#StartDate").val() != " ") {
            filters += "From Date: " + $("#StartDate").val() + ", ";
        }
        if ($("#EndDate").val() != "") {
            filters += "To Date: " + $("#EndDate").val() + ", ";
        }
        if ($("#NursingStationID").val() != 0) {
            filters += "WareHouse From: " + $("#NursingStation").val() + ", ";
        }
        if ($("#WareHouseToID").val() != 0) {
            filters += "WareHouse To: " + $("#WareHouseTo").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
}