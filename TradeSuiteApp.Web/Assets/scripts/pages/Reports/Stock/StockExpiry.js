$(function () {
    StockExpiryReport.init();
});

StockExpiryReport = {
    init: function () {
        var self = StockExpiryReport;        
        ReportHelper.init();
        self.bind_events();
    },

    bind_events: function () {
        var self = StockExpiryReport;
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
        $('#item-code-to-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_to);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $("#LocationID").on("change", self.get_premises);
        $('#Refresh').on('click', self.refresh);
    },

    set_item_code_from: function (event, item) {
        var self = StockExpiryReport;
        $("#ItemCodeFromID").val(item.id);
    },

    set_item_code_to: function (event, item) {
        var self = StockExpiryReport;
        $("#ItemCodeToID").val(item.id);
    },

    set_item_name: function (event, item) {
        var self = StockExpiryReport;
        $("#ItemID").val(item.id);
    },

    get_premises: function () {
        var self = StockExpiryReport;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
        var locationID = clean($("#LocationID").val());

        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                $("#PremiseID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#PremiseID").append(html);
            }
        });
    },
   
    get_filters: function () {
        var data = 'Stock Expiry Report';

        var filters = "";
        if ($("#LocationID").val() != 0) {
            filters += "Location: " + $("#LocationID option:selected").text() + ", ";
        }

        if ($("#PremiseID").val() != 0) {
            filters += "Premise: " + $("#PremiseID option:selected").text() + ", ";
        }
        if ($("#ItemCodeFrom").val() != 0) {
            if ($("#ItemID").val() == "") {
                filters += "Item Code: " + $("#ItemCodeFrom").val() + ", ";
            }
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

    refresh : function(event, item)
    {
        var self = StockExpiryReport;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#LocationID").val(locationID);
        $("#Location").val('');
        $("#PremiseID").val('');
        $("#ItemCodeFrom").val('');
        $("#ItemCodeFromID").val('');
        $("#ItemCodeTo").val('');
        $("#ItemCodeToID").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#BatchTypeID").val('');
    },

    get_report_view: function (e) {
        e.preventDefault();
        self = StockExpiryReport;
        var data = $("#report-filter-form").serialize();
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        switch (name) {
            case "StockExpiry":

                if ($("#LocationID").val() != 0) {
                    filters += "Location: " + $("#LocationID option:selected").text() + ", ";
                }

                if ($("#PremiseID").val() != 0) {
                    filters += "Premise: " + $("#PremiseID option:selected").text() + ", ";
                }
                if ($("#ItemCodeFrom").val() != 0) {
                    if ($("#ItemID").val() == "") {
                        filters += "Item Code: " + $("#ItemCodeFrom").val() + ", ";
                    }
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
                data += "&Filters=" + filters;

        }
        console.log(data);
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#report-viewer").html(response)
                ReportHelper.inject_js();
            }
        })
        return false;
    },

}