$(function () {
    ShortTransferReport.init();
});
ShortTransferReport = {
    init: function () {
        var self = ShortTransferReport;
        self.bind_events();
        ReportHelper.init();
    },

    bind_events: function () {
        var self = ShortTransferReport;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $("#FromLocationID").on("change", self.get_location_from);
        $("#ToLocationID").on("change", self.get_location_to);
        $('#Refresh').on('click', self.refresh);
    },

    get_location_from: function () {
        var self = ShortTransferReport;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
        var locationID = clean($("#LocationID").val());
        var id = clean($("#FromLocationID").val());
        var locationheadID = $("#FromLocationID option:selected").data("headid");

        if (locationID == locationheadID) {
            $('#FromLocationID').attr('disabled', false);
            $('#ToLocationID').attr('disabled', false);
        }
        else {
            if ($("#FromLocationID").val() != locationID) {
                $('#ToLocationID option:selected').val(locationID);
                $('#ToLocationID').attr('disabled', true);
            }
            else {
                $('#ToLocationID').attr('disabled', false);
            }
        }
    },

    get_location_to: function () {
        var self = ShortTransferReport;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }

        var locationID = clean($("#LocationID").val());
        var id = clean($("#ToLocationID").val());
        var locationheadID = $("#ToLocationID option:selected").data("headid");
        if (locationID == locationheadID) {
            $('#ToLocationID').attr('disabled', false);
            $('#FromLocationID').attr('disabled', false);
        }
        else {
            if ($("#ToLocationID").val() != locationID) {
                $("#FromLocationID option:selected").val(locationID);
                $('#FromLocationID').attr('disabled', true);
            }
            else {
                $('#FromLocationID').attr('disabled', false);
            }
        }
    },

    set_item_name: function (event,item) {
        var self = ShortTransferReport;
        $("#ItemID").val(item.id);
    },

    refresh: function (event,item) {
        var self = ShortTransferReport;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $('#BranchLocationID').val('');
        $('#SalesCategoryID').val('');
        $('#ItemName').val('');
        $('#ItemID').val('');
        $("#FromLocationID").val(locationID);
        $("#ToLocationID").val(locationID);
        $('#FromLocationID').attr('disabled', false);
        $('#ToLocationID').attr('disabled', false);
    },

    get_filters: function () {
        var self = ShortTransferReport;
        var filters = "";
        if (($("#FromLocationID").val() + $("#ToLocationID").val()).trim() != "") {
            if ($("#ToLocationID").val().trim() == 0) {
                filters += "Location " + "From: " + $("#FromLocationID option:selected").text() + ", ";
            }
            else if ($("#FromLocationID").val().trim() == 0) {
                filters += "Location " + "To: " + $("#ToLocationID option:selected").text() + ", ";
            }
            else if (($("#FromLocationID").val() + $("#ToLocationID").val()).trim() != "") {
                filters += "Location: " + $("#FromLocationID option:selected").text() + " - " + $("#ToLocationID option:selected").text() + ", ";
            }
        }

        if ($("#SalesCategoryID").val() != 0) {
            filters += "Sales Category: " + $("#SalesCategoryID option:selected").text() + ", ";
        }

        if ($("#ItemName").val() != 0) {
            filters += "Item Name: " + $("#ItemName").val() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },
}