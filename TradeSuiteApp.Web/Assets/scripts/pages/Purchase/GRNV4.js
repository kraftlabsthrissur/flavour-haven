
///This GRN.js For Janaushadhi-Created By neethu

//GRN.bind_events = function () {
//    $("body").on('click', ".btnqrcode", GRN.get_item_details);
//};

GRN.create_batch= function () {
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

    var UnitID = clean($("#add-batch #UnitID").val());
    var Unit = $("#add-batch #UnitID option:selected").text();
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
                self.set_create_batch(batchname, PurchaseMRP, expiryDate, data.data, RetailMRP, Unit, UnitID, PackSize);
                UIkit.modal($('#add-batch')).hide();
            }
            else {

                app.show_error(data.message);
            }
        },
    });
};

GRN.get_batch_data = function () {
    var bacth_no = $("#add-batch #BatchName").val();
    var grn_no = $("#GRNNo").val();
    var item_id = $("#add-batch #ItemID").val();
    var model = {
        ID: 0,
        ExpDate: $("#add-batch #expiry-date").val(),
        ManufacturingDate: $("#add-batch #manufacture-date").val(),
        ItemID: clean($("#add-batch #ItemID").val()),
        CustomBatchNo: $("#add-batch #BatchName").val(),
        BatchNo: grn_no + item_id+bacth_no,
        //BatchNo: bacth_no,
        ISKPrice: clean($("#add-batch #RetailMRP").val()),
        OSKPrice: 1,
        ExportPrice: 1,
        RetailMRP: clean($("#add-batch #RetailMRP").val()),
        RetailLooseRate: clean($("#add-batch #RetailLooseRate").val()),
        BatchRate: clean($("#add-batch #PurchaseMRP").val()),
        PurchaseLooseRate: clean($("#add-batch #PurchaseLooseRate").val()),
        UnitID: clean($("#add-batch #UnitID").val()),
        ProfitPrice: clean($("#add-batch #ProfitPrice").val())
    };
    return model;
};

GRN.get_batches = function (release) {
    var hint = $(this.input).val();
    var BatchHint = $(this.input).parents('tr').find('.batch').val();
    var ItemHint = clean($(this.input).parents('tr').find('.item-id').val());

    $.ajax({
        url: '/Masters/Batch/GetCustomBatchForGrnAutocomplete',
        data: {
            BatchHint: BatchHint,
            ItemHint: ItemHint,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            release(data.data);
        }
    });
};


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
        $("#add-batch #manufacture-date").val(date);
        $("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
        self.get_latest_batch_details(itemid, packsize, unitID);
    }
    else {
        var receivedqty =0
        var offerqty=0

        var looseqty=0
        var rate =0
        var looserate =0

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
                     looserate = Math.round((rate / looseqty));
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
     looserate = Math.round((rate / looseqty));
    if (receivedqty != 0) {
        $(row).find('.looseqty').val(looseqty);
        $(row).find('.looserate').val(looserate);
    }
    self.calculate_total();

    // self.get_rate_on_batch_selection(item.value);
};

GRN.get_latest_batch_details = function (itemid, packsize, unitID) {
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
                $("#RetailLooseRate").val(data.data.RetailMRP / packsize);
                var retailMRP = clean($("#RetailMRP").val());
                var purchaseMRP = clean($("#PurchaseMRP").val());
                //var packSize = clean($("#add-batch #PackSize").val());
                var packsize = clean($("#add-batch #UnitID option:selected").data("packsize"));
                var retailLoose = retailMRP / packsize;
                var purchaseLoose = purchaseMRP / packsize;
                var profit = ((retailMRP - purchaseMRP) / (retailMRP)) * 100;
                $("#RetailLooseRate").val(retailLoose);
                $("#PurchaseLooseRate").val(purchaseLoose);
                $("#ProfitPrice").val(profit);

            }
        },
    });
};
GRN.grn_save= function () {
    var self = GRN;
    var IsNew = false, IsDraft = false;
    self.error_count = 0;
    var url = '/Purchase/GRN/CreateGRNV4';
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
                // app.show_notice("GRN created successfully");
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
                var grnID = data.GRNID;
                self.get_item_for_qrcode_generator(grnID);
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

GRN.populate_purchase_orders= function () {
    var SupplierID = $('#SupplierID').val();
    var BusinessCategoryID = $('#BusinessCategoryID').val();
    $.ajax({
        url: '/Purchase/GRN/GetPurchaseOrdersV4',
        dataType: "json",
        data: {
            SupplierID: SupplierID,
            BusinessCategoryID: BusinessCategoryID,
        },
        type: "POST",
        success: function (purchase_orders) {
            var $purchase_order_list = $('#purchase-order-list tbody');
            $purchase_order_list.html('');
            var tr = '';
            $.each(purchase_orders, function (i, purchase_order) {
                tr += "<tr>"
                    + "<td class='uk-text-center'>" + (i + 1)
                    + "<input type=hidden class=Date value='" + purchase_order.PurchaseOrderDate + "'/>"
                    + "</td>"
                    + "<td class='uk-text-center checked' data-md-icheck>"
                    + "<input type='checkbox' class='purchase-order-id' value='" + purchase_order.ID + "'/>"
                    + "</td>"
                    + "<td>" + purchase_order.PurchaseOrderNo + "</td>"
                    + "<td>" + purchase_order.PurchaseOrderDate + "</td>"
                    + "<td>" + purchase_order.SupplierName + "</td>"
                    + "<td>" + purchase_order.RequestedBy + "</td>"
                    + "<td class='mask-currency'>" + purchase_order.NetAmt + "</td>"
                + "</tr>";

            });
            var $tr = $(tr);
            app.format($tr);
            $purchase_order_list.append($tr);
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
                    window.location = "/Purchase/GRN/IndexV4";
                }, 1000);
            } else {
                app.show_error("Failed to cancel GRN");
                $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'block' });
            }
        },
    });

};

GRN.print_close = function () {
    var url = '/Purchase/GRN/IndexV4';
    setTimeout(function () {
        window.location = url
    }, 1000);
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
                    app.load_content("/Purchase/GRN/GenerateDetailsV4/" + Id);
                });
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};

GRN.Populate_item = function () {
    var self = GRN;
    var Item;
    item_list = Item.purchase_item_list();
    item_select_table = $('#item-list').SelectTable({
        selectFunction: self.select_item,
        returnFocus: "#txtRqQty",
        modal: "#select-item",
        initiatingElement: "#ItemName",
        startFocusIndex: 3
    });
};


//GRN.get_item_details= function (release) {

//    $.ajax({
//        url: '/Purchase/PurchaseOrder/getProductList',
//        data: {
//            Areas: 'PurchaseOrder',
//            term: $('#ItemName').val(),
//            ItemCategoryID: $("#DDLItemCategory").val(),
//            PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
//            SupplierId: $("#SupplierID").val(),
//        },
//        dataType: "json",
//        type: "POST",
//        success: function (data) {
//            release(data);
//        }
//    });
//};

GRN.set_item_details = function (event, item) {   // on select auto complete item
    var self = GRN;
    $("#ItemName").val(item.label);
    $("#ItemID").val(item.id);
    $("#Rate").val('');
    $("#Unit").val(item.unit);
    $("#LastPr").val(item.lastPr);
    $("#LowestPr").val(item.lowestPr);
    $("#PendingOrderQty").val(item.pendingOrderQty);
    $("#QtyWithQc").val(item.qtyWithQc);
    $("#QtyAvailable").val(item.qtyAvailable);
    $("#GSTPercentage").val(item.gstPercentage);
    $("#CategoryID").val(item.itemCategory);
    $("#PrimaryUnit").val(item.primaryUnit);
    $("#PrimaryUnitID").val(item.primaryUnitId);
    $("#PurchaseUnit").val(item.purchaseUnit);
    $("#PurchaseUnitID").val(item.purchaseUnitId);
    if ($("#CategoryID").val() > 0) {
        $("#select_batch_type").removeClass("uk-hidden").addClass('visible');
    }
    else {
        $("#select_batch_type").addClass("uk-hidden").removeClass('visible');
    }
    self.get_units();
    self.get_rate();
    self.get_details();
    $("#Qty").focus();
};

GRN.get_units = function () {
    var self = GRN;
    $("#UnitID").html("");
    var html;
    html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
    html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
    $("#UnitID").append(html);
},

GRN.get_rate = function () {
    var ItemID = clean($("#ItemID").val());
    var BatchType = $("#BatchType option:selected").text();
    //var IsInterCompany = clean($("#IsInterCompany").val());
    //if (IsInterCompany == 0)
    //    return;
    $.ajax({
        url: '/Purchase/PurchaseOrder/GetRateForInterCompany',
        dataType: "json",
        type: "GET",
        data: {
            ItemID: ItemID,
            BatchType: BatchType
        },
        success: function (response) {
            if (response.Status == "success") {
                if (response.data >= 0) {
                    $("#Rate").val(response.data);
                    $("#txtValue").val(clean($("#Rate").val()) * clean($("#Qty").val()));
                }
                else {
                    $("#Rate").val(0);
                }
            }
        }
    });
},

GRN.get_details = function () {
    var self = GRN;
    var row = $(this).closest('tr');
    var ItemID;
    $(".Description-div").removeClass("uk-hidden");

    if (row.length == 0) {
        ItemID = clean($('#ItemID').val());
    }
    else {
        ItemID = clean($(row).find('.ItemID').val());
    }
};

GRN.calculate= function () {
    var self = GRN;
    $("#grn-items-list tbody tr").each(function () {
        var row = $(this).closest('tr');
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var StateID = $("#StateID").val();
        var ShippingStateID = $("#ShippingStateID").val();
        var packsize = clean($(row).find(".PackSize").val());
        var discountpercent = clean($(row).find(".DiscPer").val());
        var discountamount = clean($(row).find(".discountAmt").val());
        var offerqty = clean($(row).find('.offerQty').val());
        var receivedqty = clean($(row).find('.receivedqty').val());
        var looseqty = (receivedqty + offerqty) * packsize;
        var rate = clean($(row).find('.purchaserate').val());
        var looserate = (rate / looseqty);
        //if (discountpercent !== 0) {
        //    var discountamount = (rate * receivedqty * discountpercent) / 100;
        //}
        //if (discountamount !== 0) {
        //    discountpercent = (discountamount / (rate * receivedqty)) * 100
        //}
        //if ($('.DiscPer').on("change")) {
        //    var discountamount = (rate * receivedqty * discountpercent) / 100;
        //}
        //else if ($('.discountAmt').on("change")) {
        //    discountpercent = (discountamount / (rate * receivedqty)) * 100;
        //}
        if ($('.DiscPer').data('discAmt')) {
            var discountamount = (rate * receivedqty * discountpercent) / 100;
        }
        if ($('.discountAmt').data('discPer')) {
            discountpercent = (discountamount / (rate * receivedqty)) * 100;
        }
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
        $(row).find('.SGSTPercent').val(SGSTPercent);
        $(row).find('.CGSTPercent').val(CGSTPercent);
        $(row).find('.IGSTPercent').val(IGSTPercent);
        $(row).find('.SGSTAmt').val(SGSTAmt);
        $(row).find('.CGSTAmt').val(CGSTAmt);
        $(row).find('.IGSTAmt').val(IGSTAmt);
        $(row).find('.looseqty').val(looseqty);
        if (receivedqty != 0 && rate != 0) {

            $(row).find('.discountAmt').val(discountamount);
            $(row).find('.DiscPer').val(discountpercent);
            self.calculate_total();
        }
    });
};

GRN.get_grn_data= function (IsDraft) {
        
    var model = {
        ID: $("#Id").val(),
        GRNNo: $("#GRNNo").val(),
        GRNDate: $("#txtDate").val(),
        SupplierID: clean($("#SupplierID").val()),
        WarehouseID: clean($("#StoreID").val()),
        InvoiceNo: $("#invoice-number").val(),
        InvoiceDate: $("#invoice-date").val(),
        IsDraft: IsDraft,
        SGSTAmt: clean($("#SGSTAmt").val()),
        CGSTAmt: clean($("#CGSTAmt").val()),
        IGSTAmt: clean($("#IGSTAmt").val()),
        DiscountAmt: clean($("#DiscountAmt").val()),
        GrossAmt: clean($("#GrossAmt").val()),
        RoundOff: clean($("#RoundOff").val()),
        NetAmount: clean($("#NetAmt").val()),
        grnItems: []
    };
    var object;
    $("#grn-items-list tbody tr.included").each(function (i, record) {
        object = {
            //PurchaseOrderID: clean($(this).find('.hdnPOID').val()),
            //POTransID: clean($(this).find('.purchase-order-trans-id').val()),
            //ItemID: clean($(this).find('.item-id').val()),
            //Batch: $(this).find('.batch').val().trim(),
            //ExpiryDate: $(this).find('.expirydate').text(),
            //ReceivedQty: clean($(this).find('.receivedqty').val()),
            //LooseQty: clean($(this).find('.looseqty').val()),
            //Remarks: $(this).find('.remarks').val().trim(),
            //LooseRate: clean($(this).find('.looserate').val()),
            //PurchaseOrderQty: clean($(this).find('.poqty').val()),
            //UnitID: clean($(this).find('.UnitID').val()),
            //PurchaseRate: clean($(this).find('.purchaserate').val()),
            //OfferQty: clean($(this).find('.offerQty').val()),
            //DiscountID: clean($(this).find('#DiscountID').val()),
            //DiscountPercent: clean($(this).find(".DiscPer").val()),
            //DiscountAmount: clean($(this).find('.discountAmt').val()),
            //BatchID: clean($(this).find('.BatchID').val()),
            //CGSTPercent: clean($(this).find('.CGSTPercent').val()),
            //SGSTPercent: clean($(this).find('.SGSTPercent').val()),
            //IGSTPercent: clean($(this).find('.IGSTPercent').val()),
            //SGSTAmt: clean($(this).find('.SGSTAmt').val()),
            //CGSTAmt: clean($(this).find('.CGSTAmt').val()),
            //IGSTAmt: clean($(this).find('.IGSTAmt').val()),
            PurchaseOrderID: clean($(this).find('.hdnPOID').val()),
            POTransID: clean($(this).find('.purchase-order-trans-id').val()),
            ItemID: clean($(this).find('.item-id').val()),
            ItemCode: $(this).find('.ItemCode').val(),
            ItemName: $(this).find('.ItemName').text().trim(),
            PartsNumber: $(this).find('.PartsNumber').val(),
            Model: $(this).find('.Model').val(),
            Remark: $(this).find('.Remark').val(),
            IsGST: clean($(this).find('.IsGST').val()),
            IsVat: clean($(this).find('.IsVat').val()),
            CurrencyID: clean($(this).find('.CurrencyID').val()),
            Batch: $(this).find('.batch').val(),
            ExpiryDate: $(this).find('.expirydate').text().trim(),
            ReceivedQty: clean($(this).find('.receivedqty').val()),
            LooseQty: clean($(this).find('.looseqty').val()),
            LooseRate: clean($(this).find('.looserate').val()),
            PurchaseOrderQty: clean($(this).find('.poqty').val()),
            UnitID: clean($(this).find('.UnitID').val()),
            PurchaseRate: clean($(this).find('.purchaserate').val()),
            SecondaryUnit: $(this).find('.SecondaryUnit').val(),
            SecondaryRate: clean($(this).find('.secondarypurchaserate').val()),
            SecondaryUnitSize: clean($(this).find('.secondaryunitsize').val()),
            OfferQty: clean($(this).find('.offerQty').val()),
            DiscountID: clean($(this).find('#DiscountID').val()),
            GrossAmount: clean($(this).find('.GrossAmount').val()),
            DiscountPercent: clean($(this).find('.DiscountPercentage').val()),
            DiscountAmount: clean($(this).find('.discountAmt').val()),
            VATPercentage: clean($(this).find('.VATPercentage').val()),
            VATAmount: clean($(this).find('.VATAmount').val()),
            TaxableAmount: clean($(this).find('.TaxableAmount').val()),
            NetAmount: clean($(this).find('.netamount').val()),
            BatchID: $(this).find('.BatchID').val(),
            CGSTPercent: clean($(this).find('.CGSTPercent').val()),
            SGSTPercent: clean($(this).find('.SGSTPercent').val()),
            IGSTPercent: clean($(this).find('.IGSTPercent').val()),
            SGSTAmt: clean($(this).find('.SGSTAmt').val()),
            CGSTAmt: clean($(this).find('.CGSTAmt').val()),
            IGSTAmt: clean($(this).find('.IGSTAmt').val()),
            BinCode: $(this).find('.BinCode').text().trim(),
            PurchaseOrderNo: $(this).find('.PurchaseOrderNo').text().trim(),
        };
        model.grnItems.push(object);
    });
    if ($("#IsCheckedDirectInvoice").prop('checked') == true) {
        model.IsCheckedDirectInvoice = true;
    } else {
        model.IsCheckedDirectInvoice = false;
    }
    return model;
};

GRN.calculate_total= function () {
    var discountamount = 0, grossamount = 0, rate = 0, receivedqty = 0, igstamt = 0, cgstamt = 0, sgstamt = 0, netamount = 0, roundoff = 0;
    $("#grn-items-list tbody tr.included").each(function (i, record) {
        var row = $(this).closest('tr');
        receivedqty = clean($(this).find('.receivedqty').val());
        rate = clean($(this).find('.purchaserate').val());
        discountamount += clean($(this).find('.discountAmt').val());
        sgstamt += clean($(this).find('.SGSTAmt').val());
        cgstamt += clean($(this).find('.CGSTAmt').val());
        igstamt += clean($(this).find('.IGSTAmt').val());
        grossamount += (receivedqty * rate);
    });
    if (discountamount == 0 && $("#DiscountAmt").val() !== 0) {
        discountamount = $("#DiscountAmt").val();
    }
    netamount = igstamt + cgstamt + sgstamt + grossamount - discountamount;
    var temp = netamount;
    netamount = Math.round(netamount);
    roundoff = temp - netamount;
    $("#DiscountAmt").val(discountamount);
    $("#SGSTAmt").val(sgstamt);
    $("#CGSTAmt").val(cgstamt);
    $("#IGSTAmt").val(igstamt);
    $("#GrossAmt").val(grossamount);
    $("#NetAmt").val(netamount);
    $("#RoundOff").val(roundoff);
    freeze_header.resizeHeader(false);
};

GRN.select_item = function () {
    var self = GRN;
    var radio = $('#select-item tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    var Code = $(row).find(".Code").text().trim();
    var PrimaryUnit = $(row).find(".PrimaryUnit").val();
    var PrimaryUnitID = $(row).find(".PrimaryUnitID").val();
    var PurchaseUnit = $(row).find(".PurchaseUnit").val();
    var PurchaseUnitID = $(row).find(".PurchaseUnitID").val();
    var lastPr = $(row).find(".lastPr").val();
    var lowestPr = $(row).find(".lowestPr").val();
    var Category = $(row).find(".ItemCategory").val();
    var pendingOrderQty = $(row).find(".pendingOrderQty").val();
    var qtyWithQc = $(row).find(".qtyWithQc").val();
    var qtyAvailable = $(row).find(".qtyAvailable").val();
    var gstPercentage = $(row).find(".gstPercentage").val();
    var itemCategoryID = $(row).find(".finishedGoodsCategoryID").val();
    $("#ItemName").val(Name);
    $("#ItemID").val(ID);
    $("#Rate").val('');
    $("#PrimaryUnit").val(PrimaryUnit);
    $("#PrimaryUnitID").val(PrimaryUnitID);
    $("#PurchaseUnit").val(PurchaseUnit);
    $("#PurchaseUnitID").val(PurchaseUnitID);
    $("#LastPr").val(lastPr);
    $("#LowestPr").val(lowestPr);
    $("#PendingOrderQty").val(pendingOrderQty);
    $("#QtyWithQc").val(qtyWithQc);
    $("#QtyAvailable").val(qtyAvailable);
    $("#GSTPercentage").val(gstPercentage);
    $("#CategoryID").val(itemCategoryID);
    if ($("#CategoryID").val() > 0) {
        $("#select_batch_type").removeClass("uk-hidden").addClass('visible');
    }
    else {
        $("#select_batch_type").addClass("uk-hidden").removeClass('visible');
    }
    $("#Qty").focus();
    self.get_units();
    self.get_rate();
    self.get_details();
    UIkit.modal($('#select-item')).hide();

};