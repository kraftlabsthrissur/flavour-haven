$(function () {
    stock.bind_events();

});
var is_first_run = true;
stock = {

    bind_events: function () {
        $("#report-filter-submit").on("click", stock.get_report_view);
        $("#ItemNameFromRange").on("change", stock.get_to_itemrange);
        $("#ItemCategoryFromRange").on("change", stock.get_to_itemcategoryrange);
        $.UIkit.autocomplete($('#stock-requestno-autocomplete'), { 'source': stock.get_requestno, 'minLength': 1 });
        $('#stock-requestno-autocomplete').on('selectitem.uk.autocomplete', stock.set_requestno);
        $.UIkit.autocomplete($('#stock-requestnoTo-autocomplete'), { 'source': stock.get_requestnoTo, 'minLength': 1 });
        $('#stock-requestnoTo-autocomplete').on('selectitem.uk.autocomplete', stock.set_requestnoTo);
        $.UIkit.autocomplete($('#stock-issueno-autocomplete'), { 'source': stock.get_issueno, 'minLength': 1 });
        $('#stock-issueno-autocomplete').on('selectitem.uk.autocomplete', stock.set_issueno);
        $.UIkit.autocomplete($('#stock-issuenoTo-autocomplete'), { 'source': stock.get_issuenoTo, 'minLength': 1 });
        $('#stock-issuenoTo-autocomplete').on('selectitem.uk.autocomplete', stock.set_issuenoTo);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': stock.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', stock.set_item);
        //$('.stocktransfer_report_type').on('ifChanged', stock.show_stocktransfer_report_type);
        $.UIkit.autocomplete($('#itemcode-from-autocomplete'), { 'source': stock.get_itemcode_from, 'minLength': 1 });
        $('#itemcode-from-autocomplete').on('selectitem.uk.autocomplete', stock.set_itemcode_from);
        $.UIkit.autocomplete($('#itemcode-to-autocomplete'), { 'source': stock.get_itemcode_to, 'minLength': 1 });
        $('#itemcode-to-autocomplete').on('selectitem.uk.autocomplete', stock.set_itemcode_to);
        $.UIkit.autocomplete($('#itemname-autocomplete'), { 'source': stock.get_itemname, 'minLength': 1 });
        $('#itemname-autocomplete').on('selectitem.uk.autocomplete', stock.set_itemname);
        $('.reportType').on('ifChanged', stock.show_Transaction_type);
        $('#Refresh').on('click', stock.refresh);
        $('.stocktransfer_report_type').on('ifChanged', stock.show_stocktransfer_report_type);
        $('.stocktransfer-summary').on('ifChanged', stock.show_stock_transfer_summary);
        //$('.stock_ledger_type').on('ifChanged', stock.show_stock_ledger_summary)
        $('.itemwise-purchase').on('ifChanged', stock.show_Itemwise_purchase);
        $("#LocationFromID").on("change", stock.get_premises_from);
        $("#LocationToID").on("change", stock.get_premises_to);
        $('.stockageing-summary').on('ifChanged', stock.show_ageing_summary);
        $.UIkit.autocomplete($('#purchase-order-no-autocomplete'), { 'source': stock.get_purchase_order, 'minLength': 1 });
        $('#purchase-order-no-autocomplete').on('selectitem.uk.autocomplete', stock.set_purchase_order);
        $.UIkit.autocomplete($('#purchase-order-no-to-autocomplete'), { 'source': stock.get_purchase_order_to, 'minLength': 1 });
        $('#purchase-order-no-to-autocomplete').on('selectitem.uk.autocomplete', stock.set_purchase_order_to);
        $.UIkit.autocomplete($('#GRNNOFrom-autocomplete'), { 'source': stock.get_GRNNOFrom, 'minLength': 1 });
        $('#GRNNOFrom-autocomplete').on('selectitem.uk.autocomplete', stock.set_GRNNOFrom);
        $.UIkit.autocomplete($('#GRNNoTo-autocomplete'), { 'source': stock.get_GRNNOTo, 'minLength': 1 });
        $('#GRNNoTo-autocomplete').on('selectitem.uk.autocomplete', stock.set_GRNNOTo);

    },

    get_report_view: function (e) {
        e.preventDefault();
        self = stock;
        var data = $("#report-filter-form").serialize();
        var url = $("#report-filter-form").attr("action");
        console.log(data);
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#report-viewer").html(response);
                ReportHelper.inject_js();
            }
        })
        return false;
    },
    get_to_itemrange: function () {
        var self = stock;
        var from_range = $("#ItemNameFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ItemNameToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ItemNameToRange").append(html);
            }
        });
    },
    get_to_itemcategoryrange() {
        var self = stock;
        var from_range = $("#ItemCategoryFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ItemCategoryToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ItemCategoryToRange").append(html);
            }
        })
    },
    get_requestno: function (release) {
        var Table = "StockRequestNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#RequestNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_requestno: function (event, item) {
        self = stock;
        $("#RequestNoFrom").val(item.code);
        $("#RequestNoFromID").val(item.id);
        //var stock_type = $(".stocktransfer_report_type:checked").val();
        //var report_type = $(".stocktransfer-summary:checked").val();
        //if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
        //    if (stock_type != "StockTransferForm") {
        //        $("#RequestNoTo").val('');
        //        $("#RequestNoToID").val('');
        //    }
        //    else {
        //        $("#RequestNoTo").val(item.code);
        //        $("#RequestNoToID").val(item.id);
        //    }
        //    $("#RequestNoTo").val(item.code);
        //    $("#RequestNoToID").val(item.id);
        //}
        //else {
        //    $("#RequestNoTo").val('');
        //    $("#RequestNoToID").val('');
        //}
    },
    get_requestnoTo: function (release) {
        var Table = "StockRequestNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#RequestNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_requestnoTo: function (event, item) {
        self = stock;
        $("#RequestNoTo").val(item.code);
        $("#RequestNoToID").val(item.id);

        var stock_type = $(".stocktransfer_report_type:checked").val();
        var report_type = $(".stocktransfer-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            if (stock_type != "StockTransferForm") {
                $("#RequestNoFrom").val('');
                $("#RequestNoFromID").val('');
            }
            else {
                $("#RequestNoFrom").val(item.code);
                $("#RequestNoFromID").val(item.id);
            }
            $("#RequestNoFrom").val(item.code);
            $("#RequestNoFromID").val(item.id);
        }
        //else {
        //    $("#RequestNoFrom").val('');
        //    $("#RequestNoFromID").val('');
        //}
    },

    get_issueno: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#IssueNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_issueno: function (event, item) {
        self = stock;
        $("#IssueNoFrom").val(item.code);
        $("#IssueNoFromID").val(item.id);
    },

    get_issuenoTo: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#IssueNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_issuenoTo: function (event, item) {
        self = stock;
        $("#IssueNoTo").val(item.code);
        $("#IssueNoToID").val(item.id);

        var stock_type = $(".stocktransfer_report_type:checked").val();
        var report_type = $(".stocktransfer-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            if (stock_type != "StockTransferForm") {
                $("#IssueNoFrom").val('');
                $("#IssueNoFromID").val('');
            }
            else {
                $("#IssueNoFrom").val(item.code);
                $("#IssueNoFromID").val(item.id);
            }
            $("#IssueNoFrom").val(item.code);
            $("#IssueNoFromID").val(item.id);
        }
    },
    get_to_itemnamerange: function () {
        var self = stock;
        var from_range = $("#ItemNameFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ItemNameToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ItemNameToRange").append(html);
            }
        });
    },
    get_to_itemcategoryrange() {
        var self = stock;
        var from_range = $("#ItemCategoryFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ItemCategoryToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ItemCategoryToRange").append(html);
            }
        })
    },
    get_items: function (release) {
        var area = "Stock";
        if ($("#Service").is(":checked")) {
            area = "Service";
        }
        $.ajax({
            url: '/Reports/Stock/GetItemsForAutoComplete',
            data: {
                Hint: $("#ItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item: function (event, item) {
        var self = stock;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },
    get_itemcode_from: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#ItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_itemcode_from: function (event, item) {
        self = stock;
        $("#ItemCodeFrom").val(item.code);
        $("#ItemCodeFromID").val(item.id);
        //$("#ItemName").val(item.code);
        //$("#ItemNameID").val(item.id);
    },
    get_itemcode_to: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#ItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
     set_itemcode_to: function (event, item) {
        self = stock;
        $("#ItemCodeTo").val(item.code);
        $("#ItemCodeToID").val(item.id);
        //$("#ItemName").val(item.code);
        //$("#ItemNameID").val(item.id);
        var stock_type = $(".stocktransfer_report_type:checked").val();
        var report_type = $(".stocktransfer-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            if (stock_type != "StockTransferForm") {
                $("#ItemCodeFrom").val('');
                $("#ItemCodeFromID").val('');
            }
            else {
                $("#ItemCodeFrom").val(item.code);
                $("#ItemCodeFromID").val(item.id);
            }
            $("#ItemCodeFrom").val(item.code);
            $("#ItemCodeFromID").val(item.id);
        }
        else {
            if (stock_type = "stocktransfer_report_type") {

            } else {
                $("#ItemCodeFrom").val('');
                $("#ItemCodeFromID").val('');
            }

        }
    },
    get_itemname: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#ItemName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_itemname: function (event, item) {
        self = stock;
        $("#ItemName").val(item.code);
        $("#ItemNameID").val(item.id);
        $("#ItemCodeFromID").val(item.id);
        $("#ItemCodeToID").val(item.id);
    },

    refresh: function (event, item) {
        self = stock;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#GRNDateFrom").val(findate);
        $("#GRNDateTo").val(currentdate);
        $("#RequestDateFrom").val(findate);
        $("#RequestDateTo").val(currentdate);
        $("#IssueDateFrom").val(findate);
        $("#IssueDateTo").val(currentdate);
        $("#FromDate").val(findate);
        $("#ToDate").val(currentdate);
        $("#DeliveredDateFrom").val(findate);
        $("#DeliveredDateTo").val(currentdate);
        $("#IssueNoFromID").val('');
        $("#IssueNoToID").val('');
        $("#IssueNoFrom").val('');
        $("#IssueNoTo").val('');
        $("#RequestNoFrom").val(''); 
        $("#RequestNoFromID").val(''); 
        $("#RequestNoTo").val('');
        $("#RequestNoToID").val('');
        $("#LocationFromID").val(locationID);
        $("#LocationToID").val(locationID);
        //$("#LocationFromID").prop('selectedIndex', 1);
        //$("#LocationToID").prop('selectedIndex', 1);
        $("#PremisesFromID").val('');
        $("#PremisesToID").val('');
        $("#ItemCategoryFromRange").val('');
        $("#ItemCategoryToRange").val('');
        $("#ItemCategoryID").val('');
        $("#ItemCodeFromID").val('');
        $("#ItemCodeToID").val('');
        $("#ItemCodeFrom").val('');
        $("#ItemCodeTo").val('');
        $("#ItemNameFromRange").val('');
        $("#ItemNameToRange").val('');
        $("#ItemID").val('');
        $("#ItemNameID").val('');
        $("#FromItemNameRange").val('');
        $("#ToItemNameRange").val('');
        $("#ItemName").val('');
        $("#BatchTypeID").val('');
        $("#ItemMovementTransactionType").val('All');
        $("#StatusFrom").val('All');
        $("#StatusTo").val('');
        $("#ValueType").val('MRP'); 
        $("#GRNNoFrom").val();
        $("#GRNNoTo").val();
        //$("#Type").val('');
        if ($(".reportType:checked").val() == "Movement")
        {
            $("#RequestDateFrom").val('');
            $("#RequestDateTo").val('');
            $("#DeliveredDateFrom").val('');
            $("#DeliveredDateTo").val('');
        }


    },
   
    getPurchaseRequisitionNoFrom: function (release) {
        self = stock;
        var Table = 'PurchaseRequisition';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#PRNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    setPurchaseRequisitionNoFrom: function (event, item) {
        self = stock;
        $("#RequestNoFrom").val(item.code);
        $("#RequestNoFromID").val(item.id);
    },
    getPurchaseRequisitionNoTo: function (release) {
        self = stock;
        var Table = 'PurchaseRequisition';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#PRNOTo').val(),   
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    setPurchaseRequisitionNoTo: function (event, item) {
        self = stock;
        $("#RequestNoTo").val(item.code);
        $("#RequestNoToID").val(item.id);
    },
    getItemName: function (release) {
        self = stock;
        var area = "Stock"
        $.ajax({
            url: '/Reports/Stock/GetItemsForAutoComplete',
            data: {
                Hint: $("#ItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    setItemName: function (event, item) {
        self = stock;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },
    get_premises_from: function () {
        var self = stock;
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
                $("#PremisesFromID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#PremisesFromID").append(html);
            }
        });
    },
    get_premises_to: function () {
        var self = stock;
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
                $("#PremisesToID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#PremisesToID").append(html);
            }
        });
    },

    show_stocktransfer_report_type: function () {
        self = stock;
        var report_type = $(this).val();
        var item_type = $(".stocktransfer-summary:checked").val();

        if (report_type == "StockTransferForm") {
            $(".stocktransfer_byitem").addClass('uk-hidden');
            $(".stocktransfer_bylocation").addClass('uk-hidden');
            if (item_type == "Summary") {
                $(".summary").removeClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").addClass("uk-hidden");

                $(".req-noto").text('Request No To.');
                $(".issue-noto").text('Issue No To');
                $(".req-dateto").text("Request Date To");
                $(".deli-datefrom").text("Issue Date From");
                $(".deli-dateto").text("Issue Date To");
                //$(".premisesfrom").text("Premises To");
                $(".itemcode_to").text("Item Code To ");

            }
            else {
                $(".summary").addClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").addClass("uk-hidden");
                $(".location-summary").addClass("uk-hidden");
                $(".location-detail").addClass("uk-hidden");

                $(".req-dateto").text("Request Date");
                $(".req-noto").text('Request No.');
                $(".issue-noto").text('Issue No');
                //$(".locationto").text('Location')
                //$(".premisesfrom").text("Premises")
                $(".itemcode_to").text("Item Code");
                $(".deli-dateto").text("Issue Date");

            }
        }
        else {
            if (item_type == "Summary") {
                $(".summary").removeClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").removeClass("uk-hidden");
                $(".location-summary").removeClass("uk-hidden");
                $(".location-detail").removeClass("uk-hidden");

                $(".req-noto").text('Request No To.');
                //$(".locationto").text('Location To')
                $(".req-dateto").text("Request Date To");
                $(".deli-datefrom").text("Delivery Date From");
                $(".deli-dateto").text("Delivery Date To");
                //$(".premisesfrom").text("Premises To");
                $(".itemcode_to").text("Item Code To ");

            }
            else {
                $(".summary").addClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").removeClass("uk-hidden");
                $(".location-summary").addClass("uk-hidden");
                $(".location-detail").removeClass("uk-hidden");

                $(".req-dateto").text("Request Date");
                $(".req-noto").text('Request No.');
                //$(".locationto").text('Location')
                //$(".premisesfrom").text("Premises")
                $(".itemcode_to").text("Item Code");
                $(".deli-dateto").text("Delivery Date ");
            }
        }
         self.refresh();
    },
    show_stock_transfer_summary: function () {
        self = stock;
        var type = $(".stocktransfer_report_type:checked").val();
        var item_type = $(".stocktransfer-summary:checked").val();
        if (type == "StockTransferForm") {
            if (item_type == "Summary") {
                $(".summary").removeClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").addClass("uk-hidden");
                $(".location-summary").removeClass("uk-hidden");
                $(".location-detail").removeClass("uk-hidden");

                $(".req-noto").text('Request No To.');
                $(".issue-noto").text('Issue No To');
                //$(".locationto").text('Location To')
                $(".req-dateto").text("Request Date To");
                //$(".deli-datefrom").text("Issue Date From");
                $(".deli-dateto").text("Issue Date To");
                //$(".premisesfrom").text("Premises To");
                $(".itemcode_to").text("Item Code To ");
            }
            else {
                $(".summary").addClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").addClass("uk-hidden");
                //$(".location-summary").addClass("uk-hidden");
                $(".location-detail").addClass("uk-hidden");

                $(".req-dateto").text("Request Date");
                $(".req-noto").text('Request No.');
                $(".issue-noto").text('Issue No');
                //$(".locationto").text('Location')
                //$(".premisesfrom").text("Premises")
                $(".itemcode_to").text("Item Code");
                $(".deli-dateto").text("Issue Date ");

            }
        }
        else {
            if (item_type == "Summary") {
                $(".summary").removeClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").removeClass("uk-hidden");
                $(".location-summary").removeClass("uk-hidden");
                $(".location-detail").removeClass("uk-hidden");

                $(".req-noto").text('Request No To.');
                //$(".locationto").text('Location To')
                $(".req-dateto").text("Request Date To");
                $(".deli-datefrom").text("Delivery Date From");
                $(".deli-dateto").text("Delivery Date To");
                //$(".premisesfrom").text("Premises To");
                $(".itemcode_to").text("Item Code To ");

            }
            else {
                $(".summary").addClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
                $(".location").removeClass("uk-hidden");
                $(".location-summary").addClass("uk-hidden");
                $(".location-detail").removeClass("uk-hidden");

                $(".req-dateto").text("Request Date");
                $(".req-noto").text('Request No.');
                //$(".locationto").text('Location')
                //$(".premisesfrom").text("Premises")
                $(".itemcode_to").text("Item Code");
                $(".deli-dateto").text("Delivery Date ");
            }
        }
         self.refresh();
    },

    show_stock_ledger_summary: function () {
        self = stock;
        var reportType = $(".stock_ledger_type:checked").val();
        if (reportType == "Detail") {
            $(".mode").removeClass('uk-hidden');
            $(".summary").addClass('uk-hidden');
            $(".detail").removeClass('uk-hidden');
            $(".itemcode_to").text("Item Code");
            $(".Itemrange").addClass('uk-hidden');
            //$(".itemrange").hide();
        }
        else {
            $(".mode").addClass('uk-hidden');
            $(".summary").removeClass('uk-hidden');
            $(".itemcode_to").text("Item Code To");
            $(".Itemrange").removeClass('uk-hidden');
            //$(".itemrange").show();
            //$(".detail").addClass('uk-hidden');

        }
    },

    show_Transaction_type: function () {
        self = stock;
        var reportType = $(".reportType:checked").val();
        var trans = $("#ItemMovement:checked").val();
        if (reportType == "Movement") {
            $(".summary").show();
            $(".location").show();
            $(".status").addClass('uk-hidden');
            $(".itemwise-purchase").addClass('uk-hidden');
            $(".trans").show();
            $(".request-no").addClass('uk-hidden');
            $(".LocationFrom").text("Location From");
            $(".premises").show();
            $(".ValueType").show();
            $(".ratevalue").removeClass("uk-hidden");
            $("#RequestDateFrom").val();
            $("#RequestDateTo").val();
        }
        else if (reportType == "ItemwisePurchase") {
            $(".summary").hide();
            $(".trans").hide();
            $(".status").addClass('uk-hidden');
            $(".itemwise-purchase").removeClass('uk-hidden');
            $(".trans").hide();
            $(".request-no").addClass('uk-hidden');
            $(".location").hide();
            $(".LocationFrom").text("Location");
            $(".BatchType").hide();
            $(".premises").hide();
            $(".ValueType").hide();
            $(".ratevalue").addClass("uk-hidden");
        }
        else if (reportType == "Status") {
            var currentdate = $("#CurrentDate").val();
            var findate = $("#FinStartDate").val();
            $("#RequestDateFrom").val(findate);
            $("#RequestDateTo").val(currentdate);
            $(".summary").show();
            $(".location").show();
            $(".itemwise-purchase").addClass('uk-hidden');
            $(".request-no").addClass('uk-hidden');
            $(".status").removeClass('uk-hidden');
            $(".trans").hide();
            $(".LocationFrom").text("Location From");
            $(".premises").show();
            $(".ValueType").show();
            $(".ratevalue").removeClass("uk-hidden");
        }
        else {
            $(".summary").show();
            $(".location").show();
            $(".itemwise-purchase").addClass('uk-hidden');
            $(".request-no").removeClass('uk-hidden');
            $(".trans").hide();
            $(".status").addClass('uk-hidden');
            $(".LocationFrom").text("Location From");
            $(".premises").show();
            $(".ValueType").hide();
            $(".ratevalue").addClass("uk-hidden");
        }
          self.refresh();
    },
    show_ageing_summary: function () {
        self = stock;
        var reportType = $(".stockageing-summary:checked").val();
        if (reportType == "Detail") {
            $(".Batch").removeClass('uk-hidden');
        }
        else {
            $(".Batch").addClass('uk-hidden');
        }
    },
    get_purchase_order: function (release) {
        self = stock
        var Table;
        Table = 'PurchaseOrder';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#purchaseOrderNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_purchase_order: function (event, item) {
        self = stock;
        $("#purchaseOrderNoFrom").val(item.code);
        $("#purchaseOrderNOFromID").val(item.id);

    },
    get_purchase_order_to: function (release) {
        self = stock;
        var Table;
        Table = 'PurchaseOrder';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#PurchaseOrderNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_purchase_order_to: function (event, item) {
        self = stock;
        $("#PurchaseOrderNoTo").val(item.code);
        $("#PurchaseOrderNoToID").val(item.id);
    },
    get_GRNNOFrom: function (release) {
        self = stock;
        var Table;
        Table = 'GRN';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#GRNNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_GRNNOFrom: function (event, item) {
        $("#GRNNoFrom").val(item.code);
        $("#GRNNoFromID").val(item.id);
    },
    get_GRNNOTo: function (release) {
        self = stock;
        var Table;
        Table = 'GRN';
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#GRNNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_GRNNOTo: function (event, item) {
        $("#GRNNoTo").val(item.code);
        $("#GRNNoToID").val(item.id);
    },
}