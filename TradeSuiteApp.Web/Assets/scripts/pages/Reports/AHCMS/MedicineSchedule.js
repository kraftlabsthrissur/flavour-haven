$(function () {
    MedicineSchedule.init();
});

MedicineSchedule = {
    init: function () {
        var self = MedicineSchedule;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = MedicineSchedule;
        $.UIkit.autocomplete($('#nursing-station-autocomplete'), Config.get_nursing_station_list);
        $('#nursing-station-autocomplete').on('selectitem.uk.autocomplete', self.set_nursing_station);
        $('#Refresh').on('click', self.refresh);
    },
    set_nursing_station: function (event, item) {
        var self = MedicineSchedule;
        $("#NursingStationID").val(item.id);
        $("#NursingStation").val(item.name);
    },
    refresh: function () {
        var self = MedicineSchedule;
        $('#NursingStationID').val('');
        $('#NursingStation').val('');
    },
    get_filters: function () {
        var self = MedicineSchedule;
        var filters = "";
        if ($("#StartDate").val() != " ") {
            filters += "From Date: " + $("#StartDate").val() + ", ";
        }
        if ($("#EndDate").val() != "") {
            filters += "To Date: " + $("#EndDate").val() + ", ";
        }
        if ($("#NursingStationID").val() != 0) {
            filters += "NursingStation: " + $("#NursingStation").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
}