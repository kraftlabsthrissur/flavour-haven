

///This GRN.js For AyushmanBhava-Created By neethu

GRN.bind_events = function () {
    $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': GRN.get_suppliers, 'minLength': 1 });
    $('#supplier-autocomplete').on('selectitem.uk.autocomplete', GRN.set_supplier_details);
    $('#btnSelectPurchaseOrders').on('click', GRN.select_purchase_orders);
    $('#btnOkPurchaseOrderList').on('click', GRN.select_purchase_orders);
    $('#btnOKPurchaseOrders').on('click', GRN.fill_grid);
    $(".save-grn").on("click", GRN.save_confirm);
    $(".save-grn-new").on("click", GRN.save);
    $("body").on('ifChanged', '.include-item', GRN.include_item);
    $("#btnOKSupplier").on('click', GRN.select_supplier);
    $(".save-draft-grn").on("click", GRN.save);
    $("#invoice-number").on("change", GRN.get_invoice_number_count);
    $("body").on('click', ".cancel", GRN.cancel_confirm);
   // $('body').on('click', ".btnPrint", GRN.printpdfgrn);



    $("body").on("keyup change", "#grn-items-list tbody .received-qty", GRN.set_accepted_qty);
    $('body').on("keyup", "#grn-items-list tbody .batch", GRN.show_batch_with_stock);
    // $('body').on("click", "#batch-list tbody .BatchName", GRN.show_create_batch);
    $('#btnOkBatches').on('click', GRN.show_create_batch);
    //$('body').on('click', '#grn-items-list tbody td:not(.action)', GRN.show_batch_with_stock);
    $('body').on('click', '#btnCreateBatch', GRN.create_batch);
    $("body").on("keyup change", "#grn-items-list tbody .receivedqty, .offerQty", GRN.calculate_rate);
    $("body").on("keyup change", "#grn-items-list tbody .receivedsecondaryqty, .offerSecondaryQty", GRN.calculate_secondary_rate);

    $("body").on("keyup change", "#grn-items-list tbody  .DiscountPercentage, .discountAmt, .VATPercentage, .VATAmount", GRN.change_grid_values);
    $("body").on("keyup change", "#DiscountPercent, #DiscountAmt, #VATPercentage, #VATAmount, #SuppDocAmount,#SuppFreight,#LocalCustomsDuty,#LocalFreight,#LocalMiscCharge,#LocalOtherCharges,#PackingForwarding, #SuppShipAmount, #SuppOtherCharges", GRN.change_calculate_total);
    //$(".btnPrint").on("click", GRN.printpdfgrn);
   


    $(".save-generate-grn").on("click", GRN.save_grn_confirm);
    $(".save-generate-grn-new").on("click", GRN.save);
    $(".save-draft-generate-grn").on("click", GRN.save);
    $("body").on("keyup change", "#PurchaseMRP", GRN.calculate_exclusive_purchase_mrp);
    $("body").on("keyup change", "#InclusivePurchaseMRP", GRN.calculate_inclusive_purchase_mrp);
    $("body").on("keyup change", "#RetailMRP", GRN.calculate_profit);
    $("body").on("change", "#add-batch #BatchUnitID", GRN.unit_change);
    //$("body").on("change", "#grn-items-list tbody #DiscountID , #gstPercentage", GRN.calculate_rate);
    //$("body").on("change", "#DiscountPercent", GRN.set_discount);

    $("#SGSTAmt, #CGSTAmt, #IGSTAmt").on("keyup change", GRN.calculate_gst_total);
    //$("body").on("keyup change", "#grn-items-list tbody .discountAmt", GRN.calculate_total);

    $('body').on("keyup", "#txt-purchase-order", GRN.show_po);
    $('#grn-items-list  tbody tr').each(function (i) {
        $.UIkit.autocomplete($('#batch-autocomplete' + (i + 1)), { 'source': GRN.get_batches, 'minLength': 1 });
        $('#batch-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', GRN.set_batch_details);
        GRN.count_items();
    });
    $("#btnQRCodeSave").on("click", GRN.save_qr_code_confirm);
    $("body").on("click", "#btnPrintQRCode", GRN.print_qr_code);//For Test
    $("body").on("click", "#btnPrintBarCode", GRN.print_bar_code);//For Test
    $("body").on("click", ".qrcodeClose ,barcodeClose", GRN.print_close);

    $("body").on('click', ".btnqrcode", GRN.get_item_details);
    $("body").on('click', "#btn_print_code", GRN.get_qrcode);
    $("#BusinessCategoryID").on('change', GRN.remove_all_items_from_grid);
};


GRN.create_batch = function () {
    var self = GRN;
    self.error_count = self.validate_create_batch();
    if (self.error_count > 0) {
        return;
    }
    var batchname = $("#add-batch #BatchName").val();
    var expiryDate = $("#add-batch #expiry-date").val();
    var rate = clean($("#add-batch #Rate").val());
    var itemid = clean($("#add-batch #ItemID").val());
    var POTransID = clean($("#add-batch #POTransID").val());
    var RetailMRP = clean($("#add-batch #RetailMRP").val());
    var RetailLooseRate = clean($("#add-batch #RetailLooseRate").val());
    var PurchaseMRP = clean($("#add-batch #PurchaseMRP").val());
    var PurchaseLooseRate = clean($("#add-batch #PurchaseLooseRate").val());

    var UnitID = clean($("#add-batch #BatchUnitID").val());
    var Unit = $("#add-batch #BatchUnitID option:selected").text();
    var PackSize = clean($("#add-batch #PackSize").val());
    if (batchname == "Create new Batch") {
        app.show_error("Invalid batch name");
        return;
    }
    var modal;
    var row = $("#Row").val();
    modal = self.get_batch_data();
    $.ajax({
        url: '/Masters/Batch/CreateBatch',
        data: modal,
        dataType: "json",
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Status == "Success") {
                app.show_notice("Batch created");
                self.Update_grid(itemid, POTransID, batchname, PurchaseMRP, expiryDate, data.data, RetailMRP, Unit, UnitID, PackSize);
                UIkit.modal($('#add-batch')).hide();
            }
            else {

                app.show_error(data.message);
            }
        },
    });
};

GRN.get_batch_data = function () {
    var model = {
        ID: 0,
        ExpDate: $("#add-batch #expiry-date").val(),
        ManufacturingDate: $("#add-batch #manufacture-date").val(),
        ItemID: clean($("#add-batch #ItemID").val()),
        BatchNo: $("#add-batch #BatchName").val(),
        CustomBatchNo: $("#add-batch #BatchName").val(),
        ISKPrice: clean($("#add-batch #RetailMRP").val()),
        OSKPrice: 1,
        ExportPrice: 1,
        RetailMRP: clean($("#add-batch #RetailMRP").val()),
        RetailLooseRate: clean($("#add-batch #RetailLooseRate").val()),
        BatchRate: clean($("#add-batch #PurchaseMRP").val()),
        PurchaseLooseRate: clean($("#add-batch #PurchaseLooseRate").val()),
        UnitID: clean($("#add-batch #BatchUnitID").val()),
        ProfitPrice: clean($("#add-batch #ProfitPrice").val())
    };
    return model;
},

    GRN.set_batch_details = function (event, item) {
        var self = GRN;

        var code = event.currentTarget.id;
        split = code.split("autocomplete");

        id = clean(split[1]);
        var row = $(this).closest('tr').find('.batch' + id).closest('tr');

        var batchname = $(row).find(".BatchName").text();
        var ItemName = $(row).find(".ItemName").text().trim();
        var itemid = clean($(row).find(".item-id").val());
        var POTransID = clean($(row).find(".POTransID").val());
        var unitID = clean($(row).find(".UnitID").val());
        var unit = $(row).find(".Unit").val();
        var GSTPercentage = $(row).find(".gstPercentage").val();

        var packsize = clean($(row).find(".PackSize").val());
        var date = $("#Date").val();
        if (item.value == "Create new Batch") {
            self.clear_batch();
            $('#show-add-batch').trigger('click');
            $("#add-batch #ItemName").val(ItemName);
            $("#add-batch #ItemID").val(itemid);
            $("#add-batch #POTransID").val(POTransID);
            $("#add-batch #Unit").val(unit);
            $("#add-batch #UnitID").val(unitID);
            $("#add-batch #PackSize").val(packsize);
            $("#add-batch #GSTPercentage").val(GSTPercentage);
            $("#add-batch #manufacture-date").val(date);
            $("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
            self.get_latest_batch_details(itemid, packsize, unitID);
        }
        else {
            var receivedqty = 0
            var offerqty = 0

            var looseqty = 0
            var rate = 0
            var looserate = 0
            self.Update_grid(itemid, POTransID, item.value, item.purchaseRate, item.expiryDate, item.id, item.mrp, item.unit, item.unitid, item.packsize);
            //UIkit.modal($('#batch-edit')).hide();

            $.ajax({
                url: '/Masters/Batch/GetLatestBatchDetailsByCustomBatchNo',
                data: {
                    ItemID: itemid,
                    CustomBatchNo: item.value,
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        $("#Batch" + id).val(item.value);
                        $(row).find('.purchaserate').val(data.data.PurchaseMRP);
                        $(row).find('.expirydate').text(data.data.ExpDate);
                        $(row).find('.BatchID').val(data.data.ID);
                        $(row).find('.mrp').val(data.data.RetailMRP);
                        $(row).find('.Unit').val(data.data.Unit);
                        $(row).find('.UnitID').val(data.data.UnitID);
                        var packsize = data.data.PackSize;
                        receivedqty = clean($(row).find('.receivedqty').val());
                        offerqty = clean($(row).find('.offerQty').val());

                        looseqty = (offerqty + receivedqty) * packsize;
                        rate = clean($(row).find('.purchaserate').val());
                        looserate = (rate / looseqty);
                        if (receivedqty != 0) {
                            $(row).find('.looseqty').val(looseqty);
                            $(row).find('.looserate').val(looserate);
                        }
                        self.calculate_total();
                    }
                },
            });
        }

        receivedqty = clean($(row).find('.receivedqty').val());
        offerqty = clean($(row).find('.offerQty').val());

        looseqty = (offerqty + receivedqty) * packsize;
        rate = clean($(row).find('.purchaserate').val());
        looserate = (rate / looseqty);
        if (receivedqty != 0) {
            $(row).find('.looseqty').val(looseqty);
            $(row).find('.looserate').val(looserate);
        }
        self.calculate_total();

        // self.get_rate_on_batch_selection(item.value);
    };

GRN.get_latest_batch_details = function (itemid, packsize, unitID) {
    var self = GRN;
    $.ajax({
        url: '/Masters/Batch/GetLatestBatchDetailsV3',
        data: {
            ItemID: itemid,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data.Status == "success") {
                var LooseRatePercent = data.data.LooseRatePercent;
                $("#LooseRatePercent").val(data.data.LooseRatePercent);
                $("#LatestRetailMRP").val(data.data.RetailMRP);
                $("#LatestRetailLooseRate").val(data.data.RetailLooseRate);
                $("#LatestPurchaseMRP").val(data.data.BatchRate);
                $("#LatestPurchaseLooseRate").val(data.data.PurchaseLooseRate);
                $("#LatestProfitPrice").val(data.data.ProfitPrice);
                $("#RetailMRP").val(data.data.RetailMRP);
                $("#RetailLooseRate").val(data.data.RetailMRP / packsize);
                $("#PrimaryUnit").val(data.data.PrimaryUnit);
                $("#PrimaryUnitID").val(data.data.PrimaryUnitID);
                $("#InventoryUnit").val(data.data.InventoryUnit);
                $("#InventoryUnitID").val(data.data.InventoryUnitID);
                $("#ConversionFactorPtoI").val(data.data.ConversionFactorPtoI);
                $("#Category").val(data.data.Category);
                $("#GSTPercentage").val(data.data.GSTPercentage);
                $("#add-batch #PurchaseMRP").val(data.data.BatchRate);
                $("#add-batch #PurchaseLooseRate").val(data.data.PurchaseLooseRate);

                var GSTPercentage = clean($("#GSTPercentage").val());
                var purchaseMRP = clean($("#PurchaseMRP").val());
                var IsGSTRegistered = $("#IsGSTRegistered").val();
                var InclusivePurchaseMRP = 0;
                if (IsGSTRegistered == "false") {
                    InclusivePurchaseMRP = purchaseMRP;
                } else {
                    InclusivePurchaseMRP = (purchaseMRP * (GSTPercentage / 100)) + purchaseMRP;
                }
                $("#InclusivePurchaseMRP").val(InclusivePurchaseMRP)

                var Category = data.data.Category;
                if (Category == 'Arishtams' || Category == 'Asavams' || Category == 'Kashayams' || Category == 'Kuzhambu' || Category == 'Thailam (Enna)' || Category == 'Thailam (Keram)' || Category == 'Dravakam') {
                    self.get_units();
                }
                else {
                    var HiddenBatchUnitList = $("#HiddenBatchUnitList").html();
                    $("#BatchUnitID").html("<select class='md-input BatchUnitID' disabled>" + HiddenBatchUnitList + "</select>");
                    $("#BatchUnitID").val(unitID);
                }


            }
        },
    });
};

GRN.unit_change = function () {
    var self = GRN;
    var packsize = clean($("#add-batch #BatchUnitID option:selected").data("packsize"));
    var unit = $("#add-batch #BatchUnitID option:selected").val();
    $("#add-batch #PackSize").val(packsize);
    $("#add-batch #BatchUnitID").val(unit);
    $("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
    self.calculate_profit();

};

GRN.calculate_profit = function () {
    var Category = $("#Category").val();
    var ConversionFactorPtoI = clean($("#ConversionFactorPtoI").val());
    var LooseRatePercent = clean($("#LooseRatePercent").val());
    var retailMRP = clean($("#RetailMRP").val());
    var purchaseMRP = clean($("#PurchaseMRP").val());
    var packSize = clean($("#add-batch #PackSize").val());
    var retailLoose = 0;
    var purchaseLoose = 0;
    if (Category == 'Arishtams' || Category == 'Asavams' || Category == 'Kashayams' || Category == 'Kuzhambu' || Category == 'Thailam (Enna)' || Category == 'Thailam (Keram)' || Category == 'Dravakam') {
        retailLoose = retailMRP * ConversionFactorPtoI;
        retailLoose = retailLoose + (retailLoose * LooseRatePercent / 100)
        purchaseLoose = purchaseMRP * ConversionFactorPtoI;
    } else {
        retailLoose = retailMRP / packSize;
        purchaseLoose = purchaseMRP / packSize;
    }

    var profit = ((retailMRP - purchaseMRP) / (retailMRP)) * 100;
    $("#RetailLooseRate").val(retailLoose);
    $("#PurchaseLooseRate").val(purchaseLoose);
    $("#ProfitPrice").val(profit);
};

GRN.grn_save = function () {
    var self = GRN;
    var IsNew = false, IsDraft = false;
    self.error_count = 0;
    var url = '/Purchase/GRN/CreateGRN';
    if ($(this).hasClass("save-draft-generate-grn")) {
        IsDraft = true;
        url = '/Purchase/GRN/DraftGRN'
    }
    if ($(this).hasClass("save-generate-grn-new")) {
        IsNew = true;
    }
    self.error_count = ((IsDraft == true) ? self.validate_grn_draft() : self.validate_grn_form());
    if (self.error_count > 0) {
        return;
    }
    $('.save-generate-grn, .save-generate-grn-new, .save-draft-generate-grn,.cancel').css({ 'display': 'none' });
    var modal;
    modal = self.get_grn_data(IsDraft);
    $.ajax({
        url: url,
        data: modal,
        dataType: "json",
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Status == "success") {
                app.show_notice("GRN created successfully");
                setTimeout(function () {
                    window.location = "/Purchase/GRN/IndexV3";
                }, 1000);
                //if (IsNew == true) {
                //    setTimeout(function () {
                //        window.location = "/Purchase/GRN/Generate";
                //    }, 1000);
                //}
                //else {
                //    setTimeout(function () {
                //        window.location = "/Purchase/GRN/Index";
                //    }, 1000);
                //}
                //var grnID = data.GRNID;
                //self.get_grn_items(grnID);
            } else if (data.Status == "entered") {
                app.show_error("Some of the items in the purchase order are already received");
                $('.save-generate-grn, .save-generate-grn-new, .save-draft-generate-grn,.cancel').css({ 'display': 'block' });
            } else {
                if (typeof data.data[0].ErrorMessage != "undefined")
                    app.show_error(data.data[0].ErrorMessage);
                // app.show_error("Failed to create GRN");
                $('.save-generate-grn, .save-generate-grn-new, .save-draft-generate-grn,.cancel').css({ 'display': 'block' });
            }
        },
    });
};

GRN.cancel = function () {
    $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'none' });
    $.ajax({
        url: '/Purchase/GRN/Cancel',
        data: {
            ID: $("#Id").val(),
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data.Status == "success") {
                app.show_notice("GRN cancelled successfully");
                setTimeout(function () {
                    window.location = "/Purchase/GRN/IndexV3";
                }, 1000);
            } else {
                app.show_error("Failed to cancel GRN");
                $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'block' });
            }
        },
    });

};

GRN.print_close = function () {
    var url = '/Purchase/GRN/IndexV3';
    setTimeout(function () {
        window.location = url
    }, 1000);
};

GRN.Update_grid = function (itemid, POTransID, batchname, rate, expiryDate, batchID, mrp, unit, unitID, PackSize) {
    var self = GRN;
    var rates = rate;
    var IsGSTRegistered = $("#IsGSTRegistered").val();
    var StateID = $("#StateID").val();
    var ShippingStateID = $("#ShippingStateID").val();
    $("#grn-items-list tbody tr .item-id[value='" + itemid + "']").each(function () {
        var row = $(this).closest('tr').find(".POTransID[value='" + POTransID + "']").closest('tr');
        if (row != undefined) {
            $(row).find(".expirydate").text(expiryDate);
            $(row).find(".purchaserate").val(rates);
            $(row).find(".mrp").val(mrp);
            $(row).find(".batch").val(batchname);
            $(row).find(".BatchID").val(batchID);
            $(row).find(".Unit").val(unit);
            $(row).find(".UnitID").val(unitID);

            var discountpercent = clean($(row).find("#DiscountID option:selected").data("percent"));
            var offerqty = clean($(row).find('.offerQty').val());
            var receivedqty = clean($(row).find('.receivedqty').val());
            var looseqty = (receivedqty + offerqty) * PackSize;
            var rate = clean($(row).find('.purchaserate').val());
            var looserate = (rate / looseqty);
            var discountamount = (rate * receivedqty * discountpercent) / 100;
            var GST = clean($(row).find('#gstPercentage option:selected').text());
            var amount = (rate * receivedqty) - discountamount;
            var gstamount = GST * amount / 100;
            var clSgstAmount = 0
            var clIgstAmount = 0
            if (IsGSTRegistered == "false") {
                SGSTAmt = 0;
                CGSTAmt = 0;
                IGSTAmt = 0;
                SGSTPercent = 0;
                CGSTPercent = 0;
                IGSTPercent = 0;
            } else {
                if (ShippingStateID == StateID) {
                    SGSTAmt = gstamount / 2;
                    CGSTAmt = gstamount / 2;
                    SGSTPercent = GST / 2;
                    CGSTPercent = GST / 2;
                    IGSTPercent = 0;
                    IGSTAmt = 0;
                }
                else {
                    SGSTAmt = 0;
                    CGSTAmt = 0;
                    IGSTAmt = gstamount;
                    SGSTPercent = 0;
                    CGSTPercent = 0;
                    IGSTPercent = GST;
                }
            }
            var net_amount = IGSTAmt + SGSTAmt + CGSTAmt + amount
            $(row).find('.SGSTPercent').val(SGSTPercent);
            $(row).find('.CGSTPercent').val(CGSTPercent);
            $(row).find('.IGSTPercent').val(IGSTPercent);
            $(row).find('.SGSTAmt').val(SGSTAmt);
            $(row).find('.CGSTAmt').val(CGSTAmt);
            $(row).find('.IGSTAmt').val(IGSTAmt);
            $(row).find('.looseqty').val(looseqty);
            $(row).find('.netamount').val(net_amount);
            if (receivedqty != 0 && rate != 0) {

                $(row).find('.discountAmt').val(discountamount);
                self.calculate_total();
            }
            self.calculate_rate();
        }
    });

};

GRN.get_units = function () {
    var self = GRN;
    $("#BatchUnitID").html("");
    var html;
    html += "<option value='" + $("#InventoryUnitID").val() + "'>" + $("#InventoryUnit").val() + "</option>";
    //html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
    $("#BatchUnitID").append(html);
};

GRN.tabbed_list = function (type) {
    var self = GRN;
    var $list;

    switch (type) {
        case "draft":
            $list = $('#grn-draft-list');
            break;
        case "to-be-invoiced":
            $list = $('#grn-to-be-invoiced-list');
            break;
        case "partially-invoiced":
            $list = $('#grn-partially-invoiced-list');
            break;
        case "invoiced":
            $list = $('#grn-invoiced-list');
            break;
        case "cancelled":
            $list = $('#grn-cancelled-list');
            break;
    }

    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });

        altair_md.inputs($list);

        var url = "/Purchase/GRN/GetGRNList?type=" + type;

        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": false,
            "aaSorting": [[1, "desc"]],
            "ajax": {
                "url": url,
                "type": "POST",
                "data": function (data) {
                    data.params = [
                        { Key: "Type", Value: type },
                    ];
                }
            },
            "aoColumns": [
                {
                    "data": null,
                    "className": "uk-text-center",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        return (meta.settings.oAjaxData.start + meta.row + 1)
                            + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                    }
                },
                { "data": "TransNo", "className": "TransNo" },
                { "data": "TransDate", "className": "TransDate" },
                { "data": "SupplierName", "className": "SupplierName" },
                { "data": "DeliveryChallanNo", "className": "DeliveryChallanNo" },
                { "data": "DeliveryChallanDate", "className": "DeliveryChallanDate" },
                { "data": "WarehouseName", "className": "WarehouseName" },
                {
                    "data": "", "searchable": false, "className": "action qrcode", "orderable": false,
                    "render": function (data, type, row, meta) {
                        return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnqrcode' value = '" + row.ID + "'>QR Code</button>"
                    }
                }
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.Status.toLowerCase());
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var Id = $(this).closest("tr").find("td .ID").val();
                    app.load_content("/Purchase/GRN/GenerateDetailsV3/" + Id);
                });
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};
GRN.reset_values = function () {
    $("#Remarks").val('');
    $("#DiscountPercent").val('');
    $("#DiscountAmt").val('');
    $("#VATPercentage").val('');
    $("#VATAmount").val('');
    $("#SuppDocAmount").val('');
    $("#SuppShipAmount").val('');
    $("#PackingForwarding").val('');
    $("#SuppFreight").val('');
    $("#LocalCustomsDuty").val('');
    $("#LocalFreight").val('');
    $("#LocalMiscCharge").val('');
    $("#LocalOtherCharges").val('');
    $("#LocalLandinngCost").val('');
};
GRN.grn_grid_fill = function () {
    var self = GRN;
    self.reset_values();
    var purchase_order_ids = $("#purchase-order-list .purchase-order-id:checked").map(function () {
        return $(this).val();
    }).get();

    if (purchase_order_ids.length == 0) {
        app.show_error('Please select purchase order');
        return;
    }
    $('#item-count').val(0);
    maxDate = $("#purchase-order-list .purchase-order-id:checked").eq(0).closest("tr").find(".Date").val();

    $("#purchase-order-list  tbody tr .purchase-order-id:checked").each(function () {

        var currRow = $(this).parents('tr');

        CurrentDate = $(currRow).find('.Date').val();
        if (Date.parse(CurrentDate) > Date.parse(maxDate)) {
            maxDate = CurrentDate;
        }


    });
    $('#PurchaseOrderDate').val(maxDate);
    $.ajax({
        url: '/GRN/GetPurchaseOrderTransV3/',
        //dataType: "json",
        data: {
            Areas: 'Purchase',
            PurchaseOrderIDS: purchase_order_ids,
            "normalclass": gridcurrencyclass
        },
        type: "post",
        success: function (purchase_order_items) {
            var $response = $(purchase_order_items);
            app.format($response);
            $('#grn-items-list tbody').append($response);
            $('#grn-items-list  tbody tr').each(function (i) {
                $.UIkit.autocomplete($('#batch-autocomplete' + (i + 1)), { 'source': self.get_batches, 'minLength': 1 });
                $('#batch-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', self.set_batch_details);
                self.count_items();
                $('#grn-items-list tbody tr').each(function () {
                    var row = $(this).closest('tr');
                    self.calculate_grid(row);
                });
                $("#SuppDocAmount").val(clean($(".hiddenSuppDocAmount").val()));
                $("#SuppShipAmount").val(clean($(".hiddenSuppShipAmount").val()));
                $("#SuppOtherCharges").val(clean($(".hiddenSuppOtherCharge").val()));
                self.calculate_total();
            });
        }


    });
};

GRN.calculate_grid = function (row, name = '', DisPercent = 0, DisAmount = 0, DisItemPer = 0, VPercentage = 0, VAmount = 0, VATItemPer = 0) {
    var self = GRN;
    var IsGSTRegistered = $("#IsGSTRegistered").val();
    var StateID = $("#StateID").val();
    var ShippingStateID = $("#ShippingStateID").val();
    var packsize = clean($(row).find(".PackSize").val());
    var offerqty = clean($(row).find('.offerQty').val());
    var receivedqty = clean($(row).find('.receivedqty').val());
    var looseqty = (receivedqty + offerqty) * packsize;
    var rate = clean($(row).find('.purchaserate').val());
    var looserate = (rate / looseqty);
    var grosamount = (rate * receivedqty);
    var discountpercent = 0;
    var discountamount = 0;
    var VATPercentage = 0;
    var VATAmount = 0;
    if (name == 'DiscountPercentage') {
        discountpercent = DisPercent;
        discountamount = grosamount * discountpercent / 100;
    } else if (name == 'DisItemPer') {
        discountpercent = DisItemPer;
        discountamount = grosamount * discountpercent / 100;
    } else if (name == 'discountAmt') {
        discountamount = DisAmount;
        discountpercent = discountamount / grosamount * 100;
    } else {
        discountamount = clean($(row).find(".discountAmt").val());
        discountpercent = discountamount / grosamount * 100;
    }
    var TaxableAmount = grosamount - discountamount;
    if (name == 'VATPercentage') {
        VATPercentage = VPercentage;
        VATAmount = TaxableAmount * VATPercentage / 100;
    } else if (name == 'VATItemPer') {
        VATPercentage = VATItemPer;
        VATAmount = TaxableAmount * VATPercentage / 100;
    } else if (name == 'VATAmount') {
        VATAmount = VAmount;
        VATPercentage = VATAmount / TaxableAmount * 100;
    } else {
        VATAmount = clean($(row).find(".VATAmount").val());
        VATPercentage = VATAmount / TaxableAmount * 100;
    }
    var GST = clean($(row).find('#gstPercentage option:selected').text());

    var gstamount = GST * TaxableAmount / 100;
    var clSgstAmount = 0
    var clIgstAmount = 0
    if (IsGSTRegistered == "false") {
        SGSTAmt = 0;
        CGSTAmt = 0;
        IGSTAmt = 0;
        SGSTPercent = 0;
        CGSTPercent = 0;
        IGSTPercent = 0;
    } else {
        if (ShippingStateID == StateID) {
            SGSTAmt = gstamount / 2;
            CGSTAmt = gstamount / 2;
            SGSTPercent = GST / 2;
            CGSTPercent = GST / 2;
            IGSTPercent = 0;
            IGSTAmt = 0;
        }
        else {
            SGSTAmt = 0;
            CGSTAmt = 0;
            IGSTAmt = gstamount;
            SGSTPercent = 0;
            CGSTPercent = 0;
            IGSTPercent = GST;
        }

    }
    var net_amount = IGSTAmt + SGSTAmt + CGSTAmt + VATAmount + TaxableAmount
    $(row).find('.SGSTPercent').val(SGSTPercent);
    $(row).find('.CGSTPercent').val(CGSTPercent);
    $(row).find('.IGSTPercent').val(IGSTPercent);
    $(row).find('.SGSTAmt').val(SGSTAmt);
    $(row).find('.CGSTAmt').val(CGSTAmt);
    $(row).find('.IGSTAmt').val(IGSTAmt);
    $(row).find('.looseqty').val(looseqty);
    $(row).find('.GrossAmount ').val(grosamount);
    $(row).find('.TaxableAmount ').val(TaxableAmount);
    if (name == 'discountAmt') {
        $(row).find('.DiscountPercentage').val(discountpercent);
    }
    else if (name == 'DiscountPercentage') {
        $(row).find('.discountAmt').val(discountamount);
    }
    else {
        $(row).find('.DiscountPercentage').val(discountpercent);
        $(row).find('.discountAmt').val(discountamount);
    }
    if (name == 'VATAmount') {
        $(row).find('.VATPercentage').val(VATPercentage);
    }
    else if (name == 'VATPercentage') {
        $(row).find('.VATAmount').val(VATAmount);
    }
    else {
        $(row).find('.VATPercentage').val(VATPercentage);
        $(row).find('.VATAmount').val(VATAmount);
    }
    $(row).find('.netamount').val(net_amount);
};
GRN.change_grid_values = function () {
    var self = GRN;
    var row = $(this).closest('tr');
    if ($(this).attr('class').indexOf('DiscountPercentage') != -1) {
        var DiscountPercentage = clean(row.find('.DiscountPercentage').val());
        self.calculate_grid(row, 'DiscountPercentage', DiscountPercentage, 0, 0, 0, 0, 0);
    } else if ($(this).attr('class').indexOf('discountAmt') != -1) {
        var DiscountAmount = clean(row.find('.discountAmt').val());
        self.calculate_grid(row, 'discountAmt', 0, DiscountAmount, 0, 0, 0, 0);
    } else if ($(this).attr('class').indexOf('VATPercentage') != -1) {
        var VATPercentage = clean(row.find('.VATPercentage').val());
        self.calculate_grid(row, 'VATPercentage', 0, 0, 0, VATPercentage, 0, 0);
    } else if ($(this).attr('class').indexOf('VATAmount') != -1) {
        var VATAmount = clean(row.find('.VATAmount').val());
        self.calculate_grid(row, 'VATAmount', 0, 0, 0, 0, VATAmount, 0);
    }
    self.calculate_total();
};
GRN.change_calculate_total = function () {
    var self = GRN;
    if ($(this).attr('id') == 'DiscountPercent') {
        var DiscountPercentage = clean($(this).val());
        self.calculate_total('DiscountPercent', DiscountPercentage, 0);
    } else if ($(this).attr('id') == 'DiscountAmt') {
        var DiscountAmount = clean($(this).val());
        self.calculate_total('DiscountAmt', 0, DiscountAmount);
    } else if ($(this).attr('id') == 'VATPercentage') {
        var VATPercentage = clean($(this).val());
        self.calculate_total('VATPercentage', 0, 0, VATPercentage, 0);
    } else if ($(this).attr('id') == 'VATAmount') {
        var VATAmount = clean($(this).val());
        self.calculate_total('VATAmount', 0, 0, 0, VATAmount);
    } else {
        self.calculate_total();
    }
};
GRN.calculate_total = function (name = '', TDisPercent = 0, TDisAmount = 0, VPercentage = 0, VAmount = 0) {
    var self = GRN;
    var discountamount = 0, discountpercent = 0, grossamount = 0, taxableamount = 0, igstamt = 0, cgstamt = 0, sgstamt = 0, VATAmount = 0, VATPercentage = 0,
        netamount = 0, LocalLandinngCost = 0, SuppDocAmount = 0, SuppShipAmount = 0, PackingForwarding = 0, SuppFreight = 0, SuppOtherCharges = 0, roundoff = 0, discountpercentitemper = 0, VATItemPer = 0,
        LocalCustomsDuty = 0, LocalFreight = 0, LocalMiscCharge = 0, LocalOtherCharges = 0;
    if (name == 'DiscountPercent') {
        discountpercentitemper = TDisPercent;
    }
    else if (name == 'DiscountAmt') {
        var Gross = clean($("#GrossAmt").val());
        discountpercentitemper = TDisAmount / Gross * 100;
    } else if (name == 'VATPercentage') {
        VATItemPer = VPercentage;
    }
    else if (name == 'VATAmount') {
        var Taxable = clean($("#TaxableAmount").val());
        VATItemPer = VAmount / Taxable * 100;
    }
    $("#grn-items-list tbody tr.included").each(function (i, record) {
        var row = $(this).closest('tr');
        if (name == 'DiscountPercent' || name == 'DiscountAmt') {
            self.calculate_grid(row, 'DisItemPer', 0, 0, discountpercentitemper, 0, 0, 0);
        } else if (name == 'VATPercentage' || name == 'VATAmount') {
            self.calculate_grid(row, 'VATItemPer', 0, 0, 0, 0, 0, VATItemPer);
        }
        var receivedqty = clean($(this).find('.receivedqty').val());
        var rate = clean($(this).find('.purchaserate').val());
        var discount = clean($(this).find('.discountAmt').val());
        discountamount += discount;
        sgstamt += clean($(this).find('.SGSTAmt').val());
        cgstamt += clean($(this).find('.CGSTAmt').val());
        igstamt += clean($(this).find('.IGSTAmt').val());
        VATAmount += clean($(this).find('.VATAmount').val());
        grossamount += (receivedqty * rate);
        taxableamount += (receivedqty * rate) - discount;
    });
    SuppDocAmount = clean($("#SuppDocAmount").val());
    SuppShipAmount = clean($("#SuppShipAmount").val());
    PackingForwarding = clean($("#PackingForwarding").val());
    SuppOtherCharges = clean($("#SuppOtherCharges").val());
    SuppFreight = clean($("#SuppFreight").val());
    netamount = igstamt + cgstamt + sgstamt + grossamount + VATAmount + SuppDocAmount + SuppShipAmount + SuppFreight + PackingForwarding + SuppOtherCharges - discountamount;
    LocalCustomsDuty = clean($("#LocalCustomsDuty").val());
    LocalFreight = clean($("#LocalFreight").val());
    LocalMiscCharge = clean($("#LocalMiscCharge").val());
    LocalOtherCharges = clean($("#LocalOtherCharges").val());
    LocalLandinngCost = LocalCustomsDuty + LocalFreight + LocalMiscCharge + LocalOtherCharges;
    roundoff = 0;
    discountpercent = discountamount / grossamount * 100;
    VATPercentage = VATAmount / taxableamount * 100;
    if (name == 'DiscountPercent') {
        $("#DiscountAmt").val(discountamount);
    }
    if (name == 'DiscountAmt') {
        $("#DiscountPercent").val(discountpercent);
    } else {
        $("#DiscountAmt").val(discountamount);
        $("#DiscountPercent").val(discountpercent);
    }
    if (name == 'VATPercentage') {
        $("#VATAmount").val(VATAmount);
    }
    if (name == 'VATAmount') {
        $("#VATPercentage").val(VATPercentage);
    } else {
        $("#VATAmount").val(VATAmount);
        $("#VATPercentage").val(VATPercentage);
    }
    $("#SGSTAmt").val(sgstamt);
    $("#CGSTAmt").val(cgstamt);
    $("#IGSTAmt").val(igstamt);
    $("#GrossAmt").val(grossamount);
    $("#TaxableAmount").val(taxableamount);
    $("#NetAmt").val(netamount);
    $("#LocalLandinngCost").val(LocalLandinngCost);
    $("#RoundOff").val(roundoff);
    freeze_header.resizeHeader(false);
};
GRN.calculate_secondary_rate = function () {
    var self = GRN;
    var row = $(this).closest('tr');
    var secondaryqty = clean($(row).find(".receivedsecondaryqty").val());
    var secondaryunitsize = clean($(row).find(".secondaryunitsize").val());
    var qty = secondaryqty * secondaryunitsize;
    var offerSecondaryQty = clean($(row).find(".offerSecondaryQty").val());
    var offerQty = offerSecondaryQty * secondaryunitsize;
    $(row).find(".receivedqty").val(qty);
    $(row).find(".offerQty").val(offerQty);
    $(row).find(".receivedqty").trigger('change');
};
GRN.calculate_rate = function () {
    var self = GRN;
    var row = $(this).closest('tr');
    self.calculate_grid(row);
    self.calculate_total();
};

GRN.calculate_inclusive_purchase_mrp = function () {
    var GSTPercentage = clean($("#GSTPercentage").val());
    //var purchaseMRP = clean($("#PurchaseMRP").val());
    var InclusivePurchaseMRP = clean($("#InclusivePurchaseMRP").val());
    var IsGSTRegistered = $("#IsGSTRegistered").val();
    var purchaseMRP = 0;
    if (IsGSTRegistered == "false") {
        purchaseMRP = InclusivePurchaseMRP
    } else {
        purchaseMRP = InclusivePurchaseMRP * ((100) / (100 + GSTPercentage))
    }
    $("#PurchaseMRP").val(purchaseMRP)
    GRN.calculate_profit();
};

GRN.calculate_exclusive_purchase_mrp = function () {
    var GSTPercentage = clean($("#GSTPercentage").val());
    var purchaseMRP = clean($("#PurchaseMRP").val());
    var IsGSTRegistered = $("#IsGSTRegistered").val();
    var InclusivePurchaseMRP = 0;
    if (IsGSTRegistered == "false") {
        InclusivePurchaseMRP = purchaseMRP;
    } else {
        InclusivePurchaseMRP = (purchaseMRP * (GSTPercentage / 100)) + purchaseMRP;
    }
    $("#InclusivePurchaseMRP").val(InclusivePurchaseMRP)
    GRN.calculate_profit();
};