$(function () {
    StockValuationReport.init();
});

StockValuationReport = {
    init: function () {
        var self = StockValuationReport;
        ReportHelper.init();
        self.on_change_report_type();
        self.bind_events();
        $("input:radio[name=Itemtype]:first").prop("checked", true)
        $("input:radio[name=Itemtype]:first").closest('div').addClass("checked")
        self.get_category();
    },
    get_category: function () {
        var self = StockValuationReport;
        var type = $("input:radio[name=Itemtype]:checked").val();
        var html = "";
        $.ajax({
            url: '/Masters/Category/GetStockValuationItemCategory',
            dataType: "json",
            data: {
                CategoryName: type,
            },
            type: "POST",
            success: function (response) {
                $("#ItemCategoryID").html("");
                if (response.data.length > 1)
                {
                    var html = "<option value >Select</option>"
                }
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#ItemCategoryID").append(html);
            }
        });
    },
    bind_events: function () {
        var self = StockValuationReport;
        $('#item-code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_item_code_from);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_no);
        $("#LocationID").on("change", self.get_premises_by_location);
        $('#Refresh').on('click', self.refresh);
        $(".Stock_Valuation_ReportType").on('ifChanged', self.on_change_report_type);
        $(".Itemtype").on('ifChanged', self.get_category);
        $("#ItemCategoryID").change(ReportHelper.get_sales_category);
        $("#IsIncludeZero").on("ifChanged", self.get_zero_qty);

    },

    on_change_report_type: function () {
        var type = $(".Stock_Valuation_ReportType:checked").val();
        if (type == 'Summary') {
            $(".BatchNo").hide();
        }
        else {
            $(".BatchNo").show();
        }

    },

    set_item_code_from: function (event, item) {
        var self = StockValuationReport;
        $("#ItemCodeFromID").val(item.id);
    },

    set_item_name: function (event, item) {
        var self = StockValuationReport;
        $("#ItemID").val(item.id);
    },

    set_batch_no: function (event, item) {
        var self = StockValuationReport;
        $("#BatchID").val(item.id)
    },

    get_premises_by_location: function () {
        var self = StockValuationReport;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
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

    get_zero_qty: function () {
        var self = StockValuationReport;
        if ($("#IsIncludeZero").is(':checked')) {
            $("#IsQtyZero").val(true);
        } else {
            $("#IsQtyZero").val(false);
        }
    },

    get_filters: function () {

        var reportype = $(".Stock_Valuation_ReportType:checked").val();
        var filters = "";

        if ($("#Location").val() != 0) {
            filters += "Location: " + $("#Location option:selected").text() + ", ";
        }

        if ($("#PremiseID").val() != 0) {
            filters += "Premise: " + $("#PremiseID option:selected").text() + ", ";
        }

        if ($("#ItemCategoryID").val() != 0) {
            filters += "Item Category: " + $("#ItemCategoryID option:selected").text() + ", ";
        }

        if ($("#SalesCategoryID").val() != 0) {
            filters += "Sales Category: " + $("#SalesCategoryID option:selected").text() + ", ";
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

        if ($("#ValueType").val() != 0) {
            filters += "Value On: " + $("#ValueType option:selected").text() + ", ";
        }

        if ($("#BatchNo").val() != 0) {
            filters += "Batch No: " + $("#BatchNo").val() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;
    },

    refresh: function (event, item) {
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $("#ToDateString").val(currentdate);
        $("#LocationID").val(locationID);
        $("#PremiseID").val('');
        $("#ItemCodeFrom").val('');
        $("#ItemCodeFromID").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#BatchNo").val('');
        $("#BatchID").val('');
        $("#BatchTypeID").val('');
        $("#ValueType").val('MRP');
        $("#Location").val(locationID);

    }
}