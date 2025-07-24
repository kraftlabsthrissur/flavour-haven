var fh;
var fh_item;
var fh_batch;
//to do.Item search
var Batch = {
    init: function () {
        var self = Batch;
        self.bind_events();
        fh = $("#batch-list").FreezeHeader();
        //item_list = Item.item_list();
        item_list = Item.item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });


        if (self.view_type == "form") {
            item_list = Item.item_list();
            $('#item-list').SelectTable({
                selectFunction: self.select_item,
                modal: "#select-item",
                initiatingElement: "#ItemName"
            });
        }
    },
    Details: function () {
        var self = Batch;
        fh_item = $("#tbl-batch-details").FreezeHeader();
        fh_item.resizeHeader();
        self.bind_events();
    },
    list: function () {
        $batch_list = $('#batch-list');
        $batch_list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/Batch/Details/' + Id;
        });
        if ($batch_list.length) {
            $batch_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Batch/GetBatchList";
            var list_table = $batch_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "BatchNo", "className": "BatchNo" },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "ItemCategory", "className": "SalesCategory" },
                    {
                        "data": "ISKPrice", "searchable": false, "className": "RetailMRP",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.RetailMRP + "</div>";
                        }
                    },
                    { "data": "StartDate", "className": "StartDate" },
                    { "data": "ExpiryDate", "className": "ExpiryDate" },
                    { "data": "NetProfitRatio", "className": "NetProfitRatio" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $batch_list.trigger("datatable.changed");
                },
            });
            $batch_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $batch_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $batch_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = Batch;
        $(".btnSave").on('click', self.save_confirm);
        $(".btnBatchEdit").on('click', self.get_batch_invoice_details);
        $("body").on("click", "#btnBatch", Batch.save_batch_confirm);

  
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $("#btnOKItem").on("click", self.select_item);
        $("body").on("keyup change", "#PurchaseMRP", self.calculate_profit);
        $("body").on("keyup change", "#RetailMRP", self.calculate_profit);
        $("body").on('ifChanged', '.include-item', self.include_item);
        $("body").on("keyup change", ".Quantity,.OfferQty,.InvoiceRate", self.get_invoice_details);
        $("body").on("change", "#UnitID", self.set_PackSize);
        $("body").on("change", "#TaxPercentage", self.get_batch_details);
        $("body").on("change", "#tbl-batch-details tbody tr.included .DiscountID", self.get_invoice_details);
    },
    get_batch_invoice_details: function () {
        var self = Batch;
        var data = {};
        data.Trans = [];     
        var item = {};
        $('.Descriptiondetails').html('');

        $("#tbl-batch-details tbody tr.included").each(function (i, record) {
            item = {};
            row = $(this).closest('tr');
            item.RetailMRP = clean($(this).closest('tr').find('.RetailMRP').val());
            item.DiscountPercent = clean($(this).find(".DiscountID option:selected").data("percent"));
            item.Quantity = clean($(this).closest('tr').find('.Quantity').val());
            item.OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
            item.InvoiceRate = clean($(this).closest('tr').find('.InvoiceRate').val());
            item.PurchasePrice = clean($(this).closest('tr').find('.PurchasePrice').val());
            item.PurchaseLooseRate = clean($(row).closest('tr').find('.PurchaseLooseRate').val());
            item.Salesrate = clean($(row).closest('tr').find('.SalesRate').val());
            item.LooseSalesRate = clean($(row).closest('tr').find('.LooseSalesRate').val());
            item.Discount = clean($(row).closest('tr').find('.Discount').val());
            item.ProfitRatio = clean($(row).closest('tr').find('.ProfitRatio').val());
            item.PreviousQuantity = clean($(this).closest('tr').find('.PreviousQuantity').val());
            item.PreviousOfferQty = clean($(this).closest('tr').find('.PreviousOfferQty').val());
            item.PreviousInvoiceRate = clean($(this).closest('tr').find('.PreviousInvoiceRate').val());
            item.PreviousPurchasePrice = clean($(row).closest('tr').find('.PreviousPurchasePrice').val());
            item.PreviousPurchaseLooseRate = clean($(row).closest('tr').find('.PreviousPurchaseLooseRate').val());
            item.PreviousLooseSalesRate = clean($(row).closest('tr').find('.PreviousLooseSalesRate').val());
            item.PreviousGSTAmount = clean($(row).closest('tr').find('.PreviousGSTAmount').val());
            item.PreviousDiscount = clean($(row).closest('tr').find('.PreviousDiscount').val());
            item.PreviousSalesRate = clean($(row).closest('tr').find('.PreviousSalesRate').val());
            item.PreviousProfitRatio = clean($(row).closest('tr').find('.PreviousProfitRatio').val());
            item.PreviousLooseQty = clean($(row).closest('tr').find('.PreviousLooseQty').val());            
            item.InvoiceNo =$(row).closest('tr').find('.InvoiceNo').text();
            item.InvoiceDate = $(row).closest('tr').find('.InvoiceDate').text();
            item.PreviousCGSTAmt = clean($(row).closest('tr').find('.PreviousCGSTAmt').val());
            item.PreviousSGSTAmt = clean($(row).closest('tr').find('.PreviousSGSTAmt').val());
            item.SGSTAmt = clean($(row).closest('tr').find('.SGSTAmt').val());
            item.CGSTAmt = clean($(row).closest('tr').find('.CGSTAmt').val());
            item.PreviousUnit = $(row).closest('tr').find('.PreviousUnit').text();
            item.Unit = $("#UnitID option:selected").data("unit")
            data.Trans.push(item);
        });
  
        $.ajax({
            url: '/Masters/Batch/GetBatchInvoiceDetails',
            data: {
                model: data
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $p = $(response);
                app.format($p);
                $('.Descriptiondetails').append($p);
                $(".batch-invoice-description-div").removeClass("uk-hidden");
                $("#style_switcher_batch").animate("slow");
                var $switcher = $('#style_switcher_batch');
                $switcher.addClass('switcher_active');
               
            }
        });
    },

    include_item: function () {
        var self = Batch;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');

        }
        self.get_batch_details();
    },
    get_invoice_details: function () {
        var self = Batch;
        var row, RetailMRP, DiscountPercent, ReceivedQty, OfferQty, GST, CessPercent, InvoiceRate;

        row = $(this).closest('tr');
        RetailMRP = clean($("#RetailMRP").val());
        DiscountPercent = clean($(this).closest('tr').find(".DiscountID option:selected").data("percent"));
        ReceivedQty = clean($(this).closest('tr').find('.Quantity').val());
        OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
        GST = clean($("#TaxPercentage option:selected").val());
        CessPercent = clean($(this).closest('tr').find('.CessPercentage').val());
        InvoiceRate = clean($(this).closest('tr').find('.InvoiceRate').val());
        self.Calculate_Value(row, RetailMRP, DiscountPercent, ReceivedQty, OfferQty, GST, CessPercent, InvoiceRate);
    },
    get_batch_details: function () {
        var self = Batch;
        var row, RetailMRP, DiscountPercent, ReceivedQty, OfferQty, GST, CessPercent, InvoiceRate;
        $("#tbl-batch-details tbody tr.included").each(function (i, record) {
            row = $(this).closest('tr');
            RetailMRP = $("#RetailMRP").val();
            DiscountPercent = clean($(this).find(".DiscountID option:selected").data("percent"));
            ReceivedQty = clean($(this).closest('tr').find('.Quantity').val());
            OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
            CessPercent = clean($(this).closest('tr').find('.CessPercentage').val());
            InvoiceRate = clean($(this).closest('tr').find('.InvoiceRate').val());
            GST = clean($("#TaxPercentage option:selected").val());

            self.Calculate_Value(row, RetailMRP, DiscountPercent, ReceivedQty, OfferQty, GST, CessPercent, InvoiceRate);
        });


    },
    set_PackSize: function () {
        var self = Batch;
        var packsize = clean($("#UnitID option:selected").data("packsize"));
        $("#PackSize").val(packsize);
        self.get_batch_details();
    },
    Calculate_Value: function (row, RetailMRP, DiscountPercent, ReceivedQty, OfferQty, GST, CessPercent, InvoiceRate) {
        var GrossAmount = ReceivedQty * InvoiceRate;
        var DiscountAmt = (GrossAmount * DiscountPercent) / 100;
        var PackSize = clean($("#PackSize").val());
        var purchaseprice = GrossAmount - DiscountAmt;
        var LoosePurchasePrice = ((purchaseprice) / (ReceivedQty + OfferQty)).roundTo(2);
        var Salesrate = (RetailMRP * (100 / (100 + GST + CessPercent))).roundTo(2);
        var loosesalesRate = ((Salesrate) / PackSize).roundTo(2);
        var CurrentBatchNetProfit = (((Salesrate - LoosePurchasePrice) / Salesrate) * 100).roundTo(2);
        var LooseQty = (ReceivedQty + OfferQty) * PackSize;
        var GSTAmt = (GrossAmount * (GST / 100)).roundTo(2);
        var SGST = (GSTAmt / 2).roundTo(2);
        var CGST = (GSTAmt / 2).roundTo(2);
        $(row).closest('tr').find('.PurchasePrice').val(purchaseprice);
        $(row).closest('tr').find('.PurchaseLooseRate').val(LoosePurchasePrice);
        $(row).closest('tr').find('.SalesRate').val(Salesrate);
        $(row).closest('tr').find('.LooseSalesRate').val(loosesalesRate);
        $(row).closest('tr').find('.Discount').val(DiscountAmt);
        $(row).closest('tr').find('.ProfitRatio').val(CurrentBatchNetProfit);
        $(row).closest('tr').find('.LooseQty').val(LooseQty);
        $(row).closest('tr').find('.SGSTAmt').val(SGST);
        $(row).closest('tr').find('.CGSTAmt').val(CGST);
    },

    calculate_profit: function () {
        var self = Batch;
        var retailMRP = clean($("#RetailMRP").val());
        var purchaseMRP = clean($("#PurchaseMRP").val());
        var packSize = clean($("#PackSize").val());
        var retailLoose = retailMRP / packSize;
        var purchaseLoose = purchaseMRP / packSize;
        var profit = ((retailMRP - purchaseMRP) / (retailMRP)) * 100;
        $("#RetailLooseRate").val(retailLoose);
        $("#PurchaseLooseRate").val(purchaseLoose);
        $("#ProfitPrice").val(profit);
        self.get_batch_details();
    },
    get_latest_batch_details: function (itemid) {
        $.ajax({
            url: '/Masters/Batch/GetLatestBatchDetails',
            data: {
                ItemID: itemid,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    $("#LatestRetailMRP").val(data.data.RetailMRP);
                    $("#LatestRetailLooseRate").val(data.data.RetailLooseRate);
                    $("#LatestPurchaseMRP").val(data.data.BatchRate);
                    $("#LatestPurchaseLooseRate").val(data.data.PurchaseLooseRate);
                    $("#LatestProfitPrice").val(data.data.ProfitPrice);
                    $("#RetailMRP").val(data.data.RetailMRP);
                    $("#RetailLooseRate").val(data.data.RetailMRP / data.data.PackSize);
                    $("#UnitID").val(data.data.UnitID);
                    $("#PackSize").val(data.data.PackSize);
                    $("#unitSelected").text('Previous Unit selected ' + data.data.Unit)

                }
            },
        });
    },

    get_items: function (release) {
        var self = Batch;
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item: function (event, item) {
        var self = Batch;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#Itemtype").val("Finished Goods");
        self.get_latest_batch_details(item.id)
    },

    select_item: function () {
        var self = Batch;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {};
        item.id = radio.val();
        item.name = $(row).find(".Name").text().trim();
        item.code = $(row).find(".Code").text().trim();
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#Itemtype").val("Finished Goods");
        self.get_latest_batch_details(item.id);

        UIkit.modal($('#select-item')).hide();
    },

    save_confirm: function () {
        var self = Batch
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save_batch_confirm: function () {
        var self = Batch
        app.confirm_cancel("This change will affect GRN, Purchaseinvoice ,Are sure to Save", function () {
            self.save_batch();
        }, function () {
        })
    },
    save_batch: function () {
        var self = Batch;
      
        $('.btnBatch').css({ 'visibility': 'hidden' });
        var modal = self.get_batch_data();
        $.ajax({
            url: '/Masters/Batch/EditBatchInvoice',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Batch Updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Batch/Index";
                    }, 1000);
                } else {
                    app.show_error(data.message);
                    $(" .btnBatch").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    save: function () {
        var self = Batch;
        self.error_count = self.validate_create_batch();

        if (self.error_count > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Batch/CreateBatch',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Batch Updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Batch/Index";
                    }, 1000);
                } else {
                    app.show_error(data.message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            ExpDate: $("#ExpDate").val(),
            CustomBatchNo: $("#CustomBatchNo").val(),
            ItemID: $("#ItemID").val(),
            BatchNo: $("#BatchNo").val(),
            // ManufacturingDate: $("#ManufacturingDate").val(),
            //ExpDate: $("#expiry-date").val(),
            ManufacturingDate: $("#manufacture-date").val(),
            ISKPrice: clean($("#RetailMRP").val()),
            OSKPrice: 1,
            ExportPrice: 1,
            RetailMRP: clean($("#RetailMRP").val()),
            RetailLooseRate: clean($("#RetailLooseRate").val()),
            BatchRate: clean($("#PurchaseMRP").val()),
            PurchaseLooseRate: clean($("#PurchaseLooseRate").val()),
            UnitID: clean($("#UnitID").val()),
            ProfitPrice: clean($("#ProfitPrice").val())

        }
        return model;
    },
    get_batch_data: function () {
        var model = {
            ID: $("#ID").val(),           
            RetailMRP: clean($("#RetailMRP").val()),
            RetailLooseRate: clean($("#RetailLooseRate").val()),
            UnitID: clean($("#UnitID").val()),
          
        }
        model.Trans = [];
        var item = {};
        
        $("#tbl-batch-details tbody tr.included").each(function (i, record) {
            item = {};
            row = $(this).closest('tr');
            item.DiscountPercent = clean($(this).find(".DiscountID option:selected").data("percent"));
            item.Quantity = clean($(this).closest('tr').find('.Quantity').val());
            item.OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
            item.InvoiceRate = clean($(this).closest('tr').find('.InvoiceRate').val());
            item.PurchasePrice = clean($(this).closest('tr').find('.PurchasePrice').val());
            item.PurchaseLooseRate = clean($(row).closest('tr').find('.PurchaseLooseRate').val());
            item.Salesrate = clean($(row).closest('tr').find('.SalesRate').val());
            item.LooseSalesRate = clean($(row).closest('tr').find('.LooseSalesRate').val());
            item.Discount = clean($(row).closest('tr').find('.Discount').val());
            item.ProfitRatio = clean($(row).closest('tr').find('.ProfitRatio').val());
            item.SGSTAmt = clean($(row).closest('tr').find('.SGSTAmt').val());
            item.CGSTAmt = clean($(row).closest('tr').find('.CGSTAmt').val());
            item.DiscountID = clean($(this).find(".DiscountID option:selected").val());
            item.InvoiceID = clean($(this).closest('tr').find('.hdnId').val());
            item.GrossAmount = (item.Quantity + item.OfferQty) * item.InvoiceRate;
            item.NetAmount = item.GrossAmount - item.Discount;
            model.Trans.push(item);
        });
        return model;
    },

    validate_create_batch: function () {
        var self = Batch;
        if (self.rules.on_create_batch.length) {
            return form.validate(self.rules.on_create_batch);
        }
        return 0;
    },
    rules: {
        on_create_batch: [
        {
            elements: "#BatchNo",
            rules: [
                 { type: form.required, message: "Please enter batch" },
            ]
        },
        {
            elements: "#manufacture-date",
            rules: [
                { type: form.required, message: "Please enter manufacturing date" },
                { type: form.past_date, message: "Invalid manufacturing date" },

            ]
        },

         {
             elements: "#ExpDate",
             rules: [
                 { type: form.required, message: "Please enter expiry date" },
                 { type: form.future_date, message: "Invalid expiry date" },

             ]
         },
        {
            elements: "#RetailMRP",
            rules: [
                { type: form.non_zero, message: "Please enter retail mrp" },
               { type: form.numeric, message: "Please enter valid retail mrp" },
               { type: form.positive, message: "Please enter positive retail mrp" },
            ]
        },

            {
                elements: "#PurchaseMRP",
                rules: [
                    { type: form.non_zero, message: "Please enter purchase mrp" },
                   { type: form.numeric, message: "Please enter valid purchase mrp" },
                   { type: form.positive, message: "Please enter positive purchase mrp" },
                ]
            },

        ],
    }
}