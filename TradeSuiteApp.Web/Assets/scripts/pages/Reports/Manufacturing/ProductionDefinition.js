$(function () {
    ProductionDefinition.init();
});

ProductionDefinition = {
    init: function () {
        var self = ProductionDefinition;
        self.bind_events();
        ReportHelper.init();
        self.get_report_type();
    },

    bind_events: function () {
        var self = ProductionDefinition;
        $('.ProductionDefinition').on('ifChanged', self.get_report_type);
        $('#Refresh').on('click', self.refresh);
        $('#production-group-autocomplete').on('selectitem.uk.autocomplete', self.set_production_group);

    },

    get_report_type: function () {
        var self = ProductionDefinition;
        var report_type = $(".ProductionDefinition:checked").val();
        $('.filters').addClass('uk-hidden');
        $("." + report_type).removeClass('uk-hidden');
        self.refresh();

    },

    set_production_group: function (event, item) {
        var self = ProductionDefinition;
        $("#ProductGroupName").val(item.name);
        $("#ProductionGroupID").val(item.id);
        $("#StandardBatchSize").val(item.stdBatchSize);
        self.get_packsize_items();
        $("#ActualBatchSize").focus();
    },

    get_packsize_items: function () {
        var self = ProductionDefinition;
        var ProductionGroupID = $('#ProductionGroupID').val();
        $("#PackingItemID").html("");
        var html = "";
        $.ajax({
            url: '/Masters/Category/GetItemsWithPackSize/',
            dataType: "json",
            data: { ProductGroupID: ProductionGroupID },
            type: "GET",
            success: function (response) {
                $("#PackingItemID").html("");
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#PackingItemID").append(html);
            }
        });
    },

    refresh: function () {
        var self = ProductionDefinition;
        $('#ProductionGroupName').val('');
        $('#StandardBatchSize').val('');
        $('#ActualBatchSize').val('');
        $('#txtPackSize').val('');
        $('#txtQty').val('');
        $('#PackingItemID').val('');
    },

    get_filters: function () {
        var self = ProductionDefinition;
        var filters = "";

        if ($("#ProductionGroupName").val() != 0) {
            filters += "Production Group Name: " + $("#ProductionGroupName").val() + ", ";
        }
        if ($("#StandardBatchSize").val() != 0) {
            filters += "Std Batch Size: " + $("#StandardBatchSize").val() + ", ";
        }
        if ($("#ActualBatchSize").val() != 0) {
            filters += "Batch Size: " + $("#ActualBatchSize").val() + ", ";
        }
        if ($("#txtQty").val() != 0) {
            filters += "Qty: " + $("#txtQty").val() + ", ";
        }
        if ($("#BatchTypeID").val() != 0 && $("input[name='ReportType']:checked").val() == "Packing") {
            filters += "Batch Type: " + $("#BatchTypeID Option:selected").text() + ", ";
        }
        if ($("#PackingItemID").val() != 0 && $("input[name='ReportType']:checked").val() == "Packing") {
            filters += "Item Name: " + $("#PackingItemID Option:selected").text() + ", ";
        }
        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }

        return filters;

    },

    validate_form: function () {
        var self = ProductionDefinition;
        self.error_count = 0;
        if (self.error_count > 0) {
            return;
        }
        if (self.rules.on_show.length) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },

    rules: {
        on_show: [
             {
                 elements: "#ProductionGroupName:visible",
                 rules: [
                     { type: form.required, message: "Please Select ProductionGroupName" },
                 ]
             },
        ]
    }
}