$(function () {
    Treatment.init();
});

Treatment = {
    init: function () {
        var self = Treatment;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = Treatment;
        $.UIkit.autocomplete($('#treatment-autocomplete'), Config.get_treatments);
        $.UIkit.autocomplete($('#treatment-room-autocomplete'), Config.get_treatment_rooms);
        $.UIkit.autocomplete($('#therapist-autocomplete'), Config.get_therapist);
        $.UIkit.autocomplete($('#patient-autocomplete'), Config.patient);
        $('#treatment-autocomplete').on('selectitem.uk.autocomplete', self.set_treatment);
        $('#treatment-room-autocomplete').on('selectitem.uk.autocomplete', self.set_treatment_room);
        $('#therapist-autocomplete').on('selectitem.uk.autocomplete', self.set_therapist);
        $('#patient-autocomplete').on('selectitem.uk.autocomplete', self.set_patient);
        $('#Refresh').on('click', self.refresh);
    },
    set_treatment: function (event, item) {
        var self = Treatment;
        $("#TreatmentID").val(item.id);
        $("#Treatment").val(item.name);
    },
    set_treatment_room: function (event, item) {
        var self = Treatment;
        $("#TreatmentRoomID").val(item.id);
        $("#TreatmentRoom").val(item.name);
    },
    set_therapist: function (event, item) {
        var self = Treatment;
        $("#TherapistID").val(item.id);
        $("#Therapist").val(item.name);
    },
    set_patient: function (event, item) {
        var self = Treatment;
        $("#PatientID").val(item.id);
        $("#PatientName").val(item.name);
    },
    refresh: function () {
        var self = Treatment;
        $('#Treatment').val('');
        $('#TreatmentID').val(''); 
        $('#PatientName').val('');
        $('#PatientID').val('');
        $('#TreatmentRoomID').val('');
        $('#TreatmentRoom').val('');
        $('#Therapist').val('');
        $('#TherapistID').val('');
    },
    get_filters: function () {
        var self = Treatment;
        var filters = "";
        if ($("#StartDate").val() != " ") {
            filters += "From Date: " + $("#StartDate").val() + ", ";
        }
        if ($("#EndDate").val() != "") {
            filters += "To Date: " + $("#EndDate").val() + ", ";
        }
        if ($("#TreatmentID").val() !=0) {
            filters += "Treatment: " + $("#Treatment").val() + ", ";
        }
        if ($("#PatientID").val() != 0) {
            filters += "Patient: " + $("#PatientName").val() + ", ";
        }
        if ($("#TreatmentRoomID").val() !=0) {
            filters += "Treatment Room: " + $("#TreatmentRoom").val() + ", ";
        }
        if ($("#TherapistID").val() !=0) {
            filters += "Therapist: " + $("#Therapist").val() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },
    //validate_form: function () {
    //    var self = OPRegister;
    //    self.error_count = 0;
    //    if (self.error_count > 0) {
    //        return;
    //    }
    //    if (self.rules.on_show.length) {
    //        return form.validate(self.rules.on_show);
    //    }
    //    return 0;
    //},

    //rules: {
    //    on_show: [
    //         {
    //             elements: "#FromDateString:visible",
    //             rules: [
    //                 { type: form.required, message: "FromDate Is Required" },
    //             ]
    //         },

    //          {
    //              elements: "#ToDateString:visible",
    //              rules: [
    //                  { type: form.required, message: "ToDate Is Required" },
    //              ]
    //          },
    //    ]
    //}
}