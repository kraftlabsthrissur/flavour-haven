$(function () {
    StockLegerReport.init();
});
StockLegerReport = {
    init: function () {
        var self = StockLegerReport;
        ReportHelper.init();
        self.bind_events();
    },

    bind_events: function () {
        var self = StockLegerReport;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('#batchNo-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_name);
        $('#stock-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_name);
        $('.Stock_Ledger_Mode').on('ifChanged', self.show_Mode_type);
        $('.Stock_Ledger_ReportType').on('ifChanged', self.show_report_type);
        $('.Stock_Ledger_ItemType').on('ifChanged', self.refresh);
        $('#Refresh').on('click', self.refresh);
        $("#ItemCategoryID").change(ReportHelper.get_sales_category);
    },

    show_Mode_type: function () {
        self = StockLegerReport;
        var type = $(".Stock_Ledger_Mode:checked").val();
        if (type == "ItemWise") {
            $(".ItemWise").removeClass("uk-hidden");
            $(".BillWise").addClass("uk-hidden");
        }
        else {
            $(".BillWise").removeClass("uk-hidden");
            $(".ItemWise").addClass("uk-hidden");
            $('.Stock_Ledger_ItemType').val = "Summary";
        }
        self.refresh();
    },

    show_report_type: function () {
        self = StockLegerReport;
        var type = $(".Stock_Ledger_ReportType:checked").val();
        if (type == "Detail") {
            $(".batch").addClass("uk-hidden");
            //validation for reporttype
            $("#ItemID").addClass("item-val");
        }
        else {
            $(".batch").removeClass("uk-hidden");
            $("#ItemID").removeClass("item-val");
        }
        //self.refresh();
    },

    set_item_name: function (event, item) {
        var self = StockLegerReport;
        $("#ItemID").val(item.id);
    },

    set_batch_name: function (event, item) {
        var self = StockLegerReport;
        $("#BatchID").val(item.id);
    },

    refresh: function (event, item) {
        var self = StockLegerReport;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        var locationID = $("#LocationID").val();
        $(".Stock_Ledger_Mode:checked").val();
        $(".Stock_Ledger_ReportType:checked").val();
        $(".Stock_Ledger_ItemType:checked").val();
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#LocationID").val(locationID);
        $("#ItemCategoryID").val('');
        $("#SalesCategoryID").val('');
        $("#ValueType").val('MRP');
        $("#BatchNo").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PremiseID").val('');
    },

    get_filters: function () {
        var self = StockLegerReport;
        var filters = "";

        if ($("#LocationID").val() != 0) {
            filters += "Location: " + $("#LocationID option:selected").text() + ", ";
        }

        if ($("#ItemCategoryID").val() != 0) {
            filters += "Item Category: " + $("#ItemCategoryID option:selected").text() + ", ";
        }

        if ($("#SalesCategoryID").val() != 0) {
            filters += "Sales Category: " + $("#SalesCategoryID option:selected").text() + ", ";
        }

        if ($("#ItemName").val() != 0) {
            filters += "Item Name: " + $("#ItemName").val() + ", ";
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

    validate_form: function () {
        var self = StockLegerReport;
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
                 elements: "#ItemID.item-val",
                 rules: [

                     {
                        type: form.required, message: "Please Select Item"
                     },
                 ]


             },
              {
              //    elements: "#ItemName:visible",
              //    rules: [
              //        { type: form.required, message: "Please Select Item" },
                  //    ]
                  elements: "#ItemName:visible",
                  rules: [
                      {
                          type: function (element) {
                              var error = false;
                              var item_type = $(".Stock_Ledger_ItemType:checked").val();
                              var Mode_type = $(".Stock_Ledger_Mode:checked").val();
                              var ItemName = clean($("#ItemName").val());
                              if (Mode_type == "BillWise") {
                                  item_type = "ItemSummary";
                              }
                              if (item_type == "ItemDetail") {
                                  if (ItemName == 0) {
                                      error = true;
                                  }
                              }
                              return !error;
                          }, message: "Please Select Item Name"

                      },
                  ]
              },
        ]

    }
}


