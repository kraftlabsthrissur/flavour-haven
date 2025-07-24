var select_table;
var freeze_header;
var gridcurrencyclass = '';
GRN = {
    init: function () {

        var self = GRN;
        self.batchlist();
        self.po_list();
        supplier.supplier_list('not-intercompany');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#txt-purchase-order",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtBatch",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        freeze_header = $("#grn-items-list").FreezeHeader();
        self.bind_events();
        self.populate_purchase_orders();
        $("#btnPrintQRCode").hide();
        self.list();
        $('#tabs-purchase-GRM-create').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                active_item.data('tab-loaded', true);
            }
        });
    },

    details: function () {
        var self = GRN;
        $("body").on("click", ".print", self.printpdf);
        freeze_header = $("#GRN-list").FreezeHeader();
        $(".cancel").on("click", GRN.cancel_confirm);
        $("body").on("click", ".btnPrint", GRN.printpdfgrn);
        $("body").on("click", ".btnPrint2", GRN.printpdfgrn2);

    },

    list: function () {
        var self = GRN;
        $('#tabs-grn').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    printpdfgrn: function () {
        var self = GRN;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/GRNPrintPDF',
            data: {
                id: $("#Id").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },
    printpdfgrn2: function () {
        var self = GRN;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/GRNPrintExportPDF',
            data: {
                id: $("#Id").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },
    printpdf: function () {
        var self = GRN;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/GoodsReceiptNotePrint',
            data: {
                id: $("#Id").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },

    batchlist: function () {
        var $batch_list = $('#batch-list');
        if ($batch_list.length) {
            $batch_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($batch_list);


            var url = "/Masters/Batch/GetBatchListForGrn";
            var list_table = $batch_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 10,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemID", Value: clean($('#ItemIDHidden').val()) },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='retailMRP' value='" + row.RetailMRP + "'>"
                                + "<input type='hidden' class='retailLooseRate' value='" + row.RetailLooseRate + "'>"
                                + "<input type='hidden' class='purchaseMRP' value='" + row.PurchaseMRP + "'>"
                                + "<input type='hidden' class='PackSize' value='" + row.PackSize + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                + "<input type='hidden' class='purchaseLooseRate' value='" + row.PurchaseLooseRate + "'>"
                                + "<input type='hidden' class='ExpiryDate' value='" + row.ExpiryDateStr + "'>";
                        }
                    },
                    {
                        "data": "BatchNo", "className": "BatchName", "searchable": false, "focus": true, "render": function (data, type, row, meta) {
                            return "<div class='' >" + row.BatchNo + "</div>";
                        }
                    },

                    { "data": "ExpiryDate", "className": "Expiry" },
                    {
                        "data": "PurchaseMRP", "className": "Price", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.PurchaseMRP + "</div>";
                        }
                    },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $batch_list.trigger("datatable.changed");
                },
            });
            $('#ItemIDHidden').on("change", function () {

                $batch_list.fnDraw();


            });
            $batch_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $batch_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            //$scheme_list.find('thead.search input').on('textchange', function (e) {
            //    e.preventDefault();
            //    var index = $(this).parent().parent().index();
            //    list_table.api().column(index).search(this.value).draw();
            //});

            $batch_list.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    tabbed_list: function (type) {
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
                        app.load_content("/Purchase/GRN/GenerateDetails/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    po_list: function () {
        $po_list = $('#purchase-order-list');

        if ($po_list.length) {
            $po_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var po_list_table = $po_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            po_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    po_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': GRN.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', GRN.set_supplier_details);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': GRN.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', GRN.set_item_details);
        $('#btnSelectPurchaseOrders').on('click', GRN.select_purchase_orders);
        $('#btnOkPurchaseOrderList').on('click', GRN.select_purchase_orders);
        $('#btnOKPurchaseOrders').on('click', GRN.fill_grid);
        $(".save-grn").on("click", GRN.save_confirm);
        $(".save-grn-new").on("click", GRN.save);
        $("body").on('ifChanged', '.include-item', GRN.include_item);
        $("#btnOKSupplier").on('click', GRN.select_supplier);
        $("#btnOKItem").on('click', GRN.select_item);
        $(".save-draft-grn").on("click", GRN.save);
        $("#invoice-number").on("change", GRN.get_invoice_number_count);
        $("body").on('click', ".cancel", GRN.cancel_confirm);
        $("body").on("keyup", "#txtBatch", GRN.show_batch_stock);
        $.UIkit.autocomplete($('#txtbatch-autocomplete'), { 'source': GRN.get_batches, 'minLength': 1 });
        $('#txtbatch-autocomplete').on('selectitem.uk.autocomplete', GRN.set_batch);
        $("body").on("keyup change", "#grn-items-list tbody .received-qty", GRN.set_accepted_qty);
        $('body').on("keyup", "#grn-items-list tbody .batch", GRN.show_batch_with_stock);
        // $('body').on("click", "#batch-list tbody .BatchName", GRN.show_create_batch);
        $('#btnOkBatches').on('click', GRN.show_create_batch);
        //$('body').on('click', '#grn-items-list tbody td:not(.action)', GRN.show_batch_with_stock);
        $('body').on('click', '#btnCreateBatch', GRN.create_batch);
        $("body").on("keyup change", "#grn-items-list tbody .receivedqty", GRN.calculate_rate);
        $("body").on("keyup change", "#grn-items-list tbody .offerQty ", GRN.calculate_rate);

        $(".save-generate-grn").on("click", GRN.save_grn_confirm);
        $(".save-generate-grn-new").on("click", GRN.save);
        $(".save-draft-generate-grn").on("click", GRN.save);
        $("body").on("keyup change", "#PurchaseMRP", GRN.calculate_profit);
        $("body").on("keyup change", "#RetailMRP", GRN.calculate_profit);
        $("body").on("change", "#add-batch #UnitID", GRN.unit_change);

        $("body").on("change", "#grn-items-list tbody #DiscountID , #gstPercentage", GRN.calculate_rate);
        $("body").on("change", "#DiscountPercent", GRN.set_discount);
        //$("body").on("change", ".DiscPer", GRN.set_discountforDiscPer);
        //$("body").on("change", ".discountAmt", GRN.set_discountPerForAmt);
        //$("body").on("change", "#DiscountAmt", GRN.calculate_total);

        $("#SGSTAmt, #CGSTAmt, #IGSTAmt").on("keyup change", GRN.calculate_gst_total);
        $("body").on("keyup change", "#grn-items-list tbody .discountAmt", GRN.calculate_total);
        $('body').on("keyup", "#txt-purchase-order", GRN.show_po);
        $('#grn-items-list  tbody tr').each(function (i) {
            $.UIkit.autocomplete($('#batch-autocomplete' + (i + 1)), { 'source': GRN.get_batches, 'minLength': 1 });
            $('#batch-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', GRN.set_batch_details);
            GRN.count_items();
        });
        $("body").on("keydown", "#Qty", GRN.trigger_add_item);
        $("#btnQRCodeSave").on("click", GRN.save_qr_code_confirm);
        $("body").on("click", "#btnPrintQRCode", GRN.print_qr_code);//For Test
        $("body").on("click", "#btnPrintBarCode", GRN.print_bar_code);//For Test
        $("body").on("click", ".qrcodeClose ,barcodeClose", GRN.print_close);
        $("body").on('click', ".btnqrcode", GRN.get_item_details);
        $("body").on('click', "#btn_print_code", GRN.get_qrcode);
        $("#BusinessCategoryID").on('change', GRN.remove_all_items_from_grid);
        $("#btnAddProduct").on("click", GRN.add_item);
        $("body").on('ifChanged', '.include-print-item', GRN.include_print_item);
        $("#btnFilter").on("click", GRN.on_filter);
        $(".btnPrint").on("click", GRN.on_print_qr_code);
        $("body").on("click", ".printqrcodeClose", GRN.print_qr_code_close);
    },

    on_print_qr_code: function () {
        var self = GRN;
        self.qrcodeitems.splice(0, self.qrcodeitems.length)
        $('#batch-list-qr-code-print-list tbody tr.included').each(function () {
            var BatchID = clean($(this).find(".BatchID").val());
            var Batch = $(this).find(".Batch").val();
            var ItemID = clean($(this).find(".ItemID").val());
            var ItemName = $(this).find(".ItemName").val();
            var ItemCode = $(this).find(".ItemCode").val();
            var RetailMRP = clean($(this).find(".RetailMRP").val());
            var PrintingQty = clean($(this).find(".PrintingQty").val());
            var j;
            for (j = 0; j < PrintingQty; j++) {
                self.qrcodeitems.push({
                    BatchID: BatchID, Batch: Batch, ItemName: ItemName, ItemCode: ItemCode,
                    ItemID: ItemID, RetailMRP: RetailMRP, PrintingQty: PrintingQty
                });
            }
        });
        self.generate_qrcode();
        $(".qrcodeClose").hide();
        $(".printqrcodeClose").removeClass('uk-hidden');
        $("#btnPrintQRCode").show();
        $("#btnQRCodeSave").hide();
    },

    on_filter: function () {
        var self = GRN;
        self.error_count = 0;
        self.error_count = self.validate_on_filter();
        if (self.error_count > 0) {
            return;
        }
        $("#batch-list-qr-code-print-list tbody").empty();
        self.get_records();
        self.count_items();
    },

    get_records: function () {
        var self = GRN;
        var model = self.get_filter_data();
        $.ajax({
            url: "/Purchase/GRN/GetBatchListForQRCodePrint",
            data: model,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#batch-list-qr-code-print-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#batch-list-qr-code-print-list tbody").append($response);
                //freeze_header.resizeHeader();
                //self.count_items();
            }
        });
    },

    get_filter_data: function () {
        var model = {};
        model.ItemID = $("#ItemID").val()
        return model;
    },

    include_print_item: function () {
        var self = GRN;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }
        self.count_qr_code_print_items();
    },

    count_qr_code_print_items: function () {
        var count = $('#batch-list-qr-code-print-list tbody tr.included').length;
        $('#qr-code-print-count').val(count);
    },

    set_batch: function (event, item) {
        var self = GRN;
        var Name = $("#ItemName").text().trim();
        var batchname = $("#txt").text()
        var ItemName = $("#ItemName").val();
        var itemid = $("#ItemID").val();
        var POTransID = $("#POTransID").val();
        var Unit = $("#UnitID option:selected").text();
        var UnitID = $("#UnitID option:selected").val();
        var Batch = $("#txtBatch").val();
        var BatchID = $("#txtBatchID").val();
        var Rate = $("#Rate").val();
        var MRP = $("#MRP").val();
        var packsize = $("#PackSize").val();
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
            var receivedqty = 0
            var offerqty = 0

            var looseqty = 0
            var rate = 0
            var looserate = 0

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
    },

    add_item: function () {
        var self = GRN;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }

        var ItemName = $("#ItemName").val();
        var ItemID = $("#ItemID").val();
        var Qty = clean($("#Qty").val());
        var Unit = $("#UnitID option:selected").text();
        var UnitID = $("#UnitID option:selected").val();
        var Batch = $("#txtBatch").val();
        var BatchID = $("#txtBatchID").val();
        var Rate = $("#Rate").val();
        var MRP = $("#MRP").val();

        $.ajax({
            url: "/Purchase/GRN/ItemDetails",
            data: {
                ItemID: ItemID,
                ItemName: ItemName,
                Qty: Qty,
                UnitName: Unit,
                UnitID: UnitID,
                Batch: Batch,
                BatchID: BatchID,
                MRP: MRP,
                Rate: Rate
            },
            dataType: "html",
            type: "GET",
            success: function (response) {
                var $response = $(response);
                app.format($response);
                $('#grn-items-list tbody').append($response);
                self.count_items();
                GRN.calculate_total();
                self.clear_item();
            }

        });

        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);

    },

    trigger_add_item: function (e) {
        var self = GRN;
        if (e.keyCode == 13) {
            self.add_item();
        }
    },

    clear_item: function () {
        var self = GRN;
        $("#ItemID").val('');
        $("#Unit").val('');
        $("#UnitID").val('');
        $("#Qty").val('');
        $("#txtBatch").val('');
        $("#txtBatchID").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);

    },



    remove_all_items_from_grid: function () {
        var self = GRN;
        $('#grn-items-list tbody tr').empty();
        GRN.calculate_gst_total();
        GRN.populate_purchase_orders();
        GRN.count_items();
    },

    calculate_gst_total: function () {
        var self = GRN;
        var discountamount = 0, grossamount = 0, rate = 0, receivedqty = 0, igstamt = 0, cgstamt = 0, sgstamt = 0, netamount = 0, roundoff = 0;
        $("#grn-items-list tbody tr.included").each(function (i, record) {
            var row = $(this).closest('tr');
            receivedqty = clean($(this).find('.receivedqty').val());
            rate = clean($(this).find('.purchaserate').val());
            discountamount += clean($(this).find('.discountAmt').val());
            grossamount += (receivedqty * rate);
        });
        sgstamt = clean($("#SGSTAmt").val());
        cgstamt = clean($("#CGSTAmt").val());
        igstamt = clean($("#IGSTAmt").val());
        netamount = igstamt + cgstamt + sgstamt + grossamount - discountamount;
        roundoff = 0;
        $("#DiscountAmt").val(discountamount);
        $("#GrossAmt").val(grossamount);
        $("#NetAmt").val(netamount);
        $("#RoundOff").val(roundoff);
        freeze_header.resizeHeader(false);
    },



    get_qrcode: function () {
        var self = GRN;
        //var data = {};
        //var row = $(this).closest('tr');
        //var grnID = clean($(row).find(".ID").val());
        //var IsQRCodeSaved = clean($(row).find(".IsQRCodeSaved").val());
        var grnID = $("#GRNID").val();
        self.grnItems = [];
        $('#GRN-list tbody tr').each(function () {
            item = {};
            item.Batch = $(this).find(".batch_type").text();
            item.BatchID = $(this).find(".batchID").val();
            item.PrintQty = clean($(this).find(".print-qty").val());
            self.grnItems.push(item);
        });
        self.get_item_for_qrcode_generator(grnID);
        $("#btnPrintQRCode").show();
        $("#btnQRCodeSave").hide();
    },
    grnItems: [],
    get_item_details: function () {
        var self = GRN;
        var row = $(this).closest('tr');
        var grnID = clean($(row).find(".ID").val());
        $("#GRNID").val("");
        $("#GRNID").val(grnID);
        var IsQRCodeSaved = clean($(row).find(".IsQRCodeSaved").val());
        self.get_grn_items(grnID);
    },
    get_grn_items: function (grnID) {
        var self = GRN;
        $("#GRNID").val("");
        $("#GRNID").val(grnID);
        $.ajax({
            url: "/Purchase/GRN/GetGrnItems",
            data: {
                ID: grnID,
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#GRN-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#GRN-list").append($response);
            }
        });
        $('#item-details').show();
        $('#show-item-details').trigger('click');
    },
    set_discount: function () {
        var self = GRN;
        var discountId = clean($("#DiscountPercent").val());
        $("#grn-items-list tbody tr").each(function () {
            var row = $(this).closest('tr');
            $(row).find('#DiscountID option:selected').attr("selected", null);

            $(row).find('#DiscountID option[value=' + discountId + ']').attr("selected", "selected");
            self.calculate();
        });

    },
    set_discountforDiscPer: function () {
        var self = GRN;
        $(this).data('discAmt', true);
        self.calculate();
    },
    set_discountPerForAmt: function () {
        var self = GRN;
        $(this).data('discPer', true);
        self.calculate();
    },
    calculate: function () {
        var self = GRN;
        $("#grn-items-list tbody tr").each(function () {
            var row = $(this).closest('tr');
            var IsGSTRegistered = $("#IsGSTRegistered").val();
            var StateID = $("#StateID").val();
            var ShippingStateID = $("#ShippingStateID").val();
            var packsize = clean($(row).find(".PackSize").val());
            var discountpercent = clean($(row).find('.DiscountPercentage').val());
            var offerqty = clean($(row).find('.offerQty').val());
            var receivedqty = clean($(row).find('.receivedqty').val());
            var looseqty = (receivedqty + offerqty) * packsize;
            var rate = clean($(row).find('.purchaserate').val());
            var looserate = (rate / looseqty);
            var discountamount = (rate * receivedqty * discountpercent) / 100; discountAmt
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
                self.calculate_total();
            }
        });
    },

    show_po: function (e) {
        if (e.which == 13) {
            UIkit.modal($('#select_po')).show();
        }

    },

    set_accepted_qty: function () {
        var row = $(this).closest('tr');
        var acceptedqty = clean($(row).find('.received-qty').val());
        $(row).find('.accepted-qty').val(acceptedqty);
    },
    save_confirm: function () {
        var self = GRN
        app.confirm_cancel("Do you want to Save?", function () {
            self.save();
        }, function () {
        })
    },
    save_grn_confirm: function () {
        var self = GRN
        app.confirm_cancel("Do you want to Save?", function () {
            self.grn_save();
        }, function () {
        })
    },
    get_invoice_number_count: function (release) {

        $.ajax({
            url: '/Purchase/GRN/GetInvoiceNumberCount',
            data: {
                Hint: $("#invoice-number").val(),
                Table: "GoodsReceiptNote",
                SupplierID: $('#SupplierID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                count = response.data;
                $("#invoice-count").val(count);
            }
        });
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }
        GRN.count_items();
        GRN.calculate_total();

    },

    cancel_confirm: function () {
        app.confirm_cancel("Do you want to cancel? This can't be undone", function () {
            GRN.cancel();
        }, function () {
        })
    },

    set_supplier_details: function (event, item) {   // on select auto complete item
        if ($('#grn-items-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                $("#SupplierName").val(item.name);
                $("#SupplierLocation").val(item.location);
                $("#SupplierID").val(item.id);
                $("#SupplierIDPrint").val(item.id);
                $("#SupplierCodePrint").val(item.code);
                $("#StateID").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#Code").val(item.Code);
                $("#SupplierReferenceNo").focus();
                GRN.populate_purchase_orders();
                $('#grn-items-list tbody').html('');
            });

        } else {
            $("#SupplierName").val(item.name);
            $("#SupplierLocation").val(item.location);
            $("#SupplierID").val(item.id);
            $("#IsGSTRegistered").val(item.isGstRegistered);
            $("#Code").val(item.code);
            $("#SupplierIDPrint").val(item.id);
            $("#SupplierCodePrint").val(item.code);
            $("#StateID").val(item.stateId);
            $("#SupplierReferenceNo").focus();
            GRN.populate_purchase_orders();

        }
        GRN.get_invoice_number_count();
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierNonInterCompanyForAutoComplete',
            data: {
                term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    select_supplier: function () {
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        if ($('#grn-items-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                $('#SupplierName').val(Name);
                $('#SupplierID').val(ID);
                $('#Code').val(Code);
                $("#SupplierIDPrint").val(ID);
                $("#SupplierCodePrint").val(Code);
                $('#grn-items-list tbody').html('');
                $("#StateID").val(StateID);
                $("#IsGSTRegistered").val(IsGSTRegistered);
                GRN.populate_purchase_orders();
            })
        } else {
            $('#SupplierName').val(Name);
            $('#SupplierID').val(ID);
            $('#Code').val(Code);
            $("#SupplierIDPrint").val(ID);
            $("#SupplierCodePrint").val(Code);
            $("#StateID").val(StateID);
            $("#IsGSTRegistered").val(IsGSTRegistered);
            GRN.populate_purchase_orders();
            GRN.get_invoice_number_count();
        }

        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyName = $(row).find(".CurrencyName").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        var DecimalPlaces = $(row).find(".DecimalPlaces").val();

        gridcurrencyclass = app.change_decimalplaces($("#NetAmt"), DecimalPlaces);
        app.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);
        app.change_decimalplaces($("#GrossAmt"), DecimalPlaces);
        app.change_decimalplaces($("#RoundOff"), DecimalPlaces);
        app.change_decimalplaces($("#DiscountPercent"), DecimalPlaces);
        app.change_decimalplaces($("#DiscountAmt"), DecimalPlaces);
        app.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
        app.change_decimalplaces($("#VATPercentage"), DecimalPlaces);
        app.change_decimalplaces($("#VATAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppDocAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppShipAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppOtherCharges"), DecimalPlaces);
        app.change_decimalplaces($("#PackingForwarding"), DecimalPlaces);
        app.change_decimalplaces($("#SuppFreight"), DecimalPlaces);
        app.change_decimalplaces($("#LocalCustomsDuty"), DecimalPlaces);
        app.change_decimalplaces($("#LocalFreight"), DecimalPlaces);
        app.change_decimalplaces($("#LocalMiscCharge"), DecimalPlaces);
        app.change_decimalplaces($("#LocalOtherCharges"), DecimalPlaces);
        app.change_decimalplaces($("#LocalLandinngCost"), DecimalPlaces);
        $("#DecimalPlaces").val(DecimalPlaces);
        $("#CurrencyID").val(CurrencyID);
        $("#CurrencyName").val(CurrencyName);
        $("#CurrencyCode").val(CurrencyCode);
        $("#CurrencyExchangeRate").val(CurrencyConversionRate);
        UIkit.modal($('#select-supplier')).hide();
    },
    count_items: function () {
        var count = $('#grn-items-list tbody').find('input.include-item:checked').length;
        $('#item-count').val(count);
        var sino = 0;
        $('#grn-items-list tbody tr .serial-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
    },
    populate_purchase_orders: function () {
        var supplier_id = clean($('#SupplierID').val());
        $.ajax({
            url: '/Purchase/GRN/GetPurchaseOrders/' + supplier_id,
            dataType: "json",
            data: {
                Areas: 'Purchase'
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
    },
    select_purchase_orders: function () {
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


        })

        $('#PurchaseOrderDate').val(maxDate);
        $.ajax({
            url: '/GRN/GetPurchaseOrderItems/',
            dataType: "json",
            data: {
                Areas: 'Purchase',
                PurchaseOrderIDS: purchase_order_ids,
            },
            type: "POST",
            success: function (purchase_order_items) {
                var $purchase_order_items_list = $('#grn-items-list tbody');
                $purchase_order_items_list.html('');
                var tr = '';
                var localcode = 0;
                if ($('#Code').val() == "LOCALSUP") {
                    localcode = 1;
                }
                $.each(purchase_order_items, function (i, purchase_order_item) {
                    var batchType;
                    if (purchase_order_item.BatchType == null) {
                        batchType = '';
                    }
                    else {
                        batchType = purchase_order_item.BatchType;
                    }
                    var allowed_quantity = purchase_order_item.Quantity * purchase_order_item.QtyTolerancePercent / 100;

                    tr += " <tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='checked uk-text-center' data-md-icheck> "
                        + "<input type='checkbox' class='include-item'/>"
                        + "<input type='hidden' class='purchase-order-id' value='" + purchase_order_item.PurchaseOrderID + "'/>"
                        + "<input type='hidden' class='purchase-order-trans-id' value='" + purchase_order_item.ID + "'/>"
                        + "<input type='hidden' class='item-id' value='" + purchase_order_item.ItemID + "'/>"
                        + "<input type='hidden' class='qty-tolerance-percent' value='" + purchase_order_item.QtyTolerancePercent + "'/>"
                        + "<input type='hidden' class='quantity ' value='" + purchase_order_item.Quantity + "'/>"
                        + "<input type='hidden' class='is-qc-required' value='" + purchase_order_item.IsQCRequired + "'/>"
                        + "<input type='hidden' class='pending-po-quantity' value='" + purchase_order_item.PendingPOQty + "'/>"
                        + "<input type='hidden' class='toleranceqty' value='" + allowed_quantity + "'/>"
                        + "<input type='hidden' class='UnitID' value='" + purchase_order_item.UnitID + "'/>"

                        + "<input type='hidden' class='item-category' value='" + purchase_order_item.Category + "'/>"
                        + "</td>"
                        + "<td>" + purchase_order_item.Name + "</td>"
                        + "<td class=''>" + purchase_order_item.Unit + "</td>"
                        + "<td class='batch_type'>" + batchType + "</td>"
                        + "<td>" + purchase_order_item.PurchaseOrderNo + "</td>"
                        + "<td class='uk-text-right mask-qty'>" + purchase_order_item.PendingPOQty + "</td>"
                        + "<td ><input type='text'  class='md-input received-qty mask-qty' " + (localcode == 1 ? " readonly='readonly' " : "") + " disabled value= " + (localcode == 1 ? purchase_order_item.PendingPOQty : 0.0) + "  /> </td>"
                        + "<td ><input type='text'  class='md-input accepted-qty mask-qty' " + (localcode == 1 ? " readonly='readonly' " : "") + " disabled value=" + (localcode == 1 ? purchase_order_item.PendingPOQty : 0.0) + "  /></td>"

                        + "<td class='batch-hidden'><input type='text' class='md-input batch' disabled /></td>"
                        + "<td class='batch-hidden'><div class='uk-input-group'>"
                        + "<input type='text' class='md-input expiry-date future-date-only date' disabled  />"
                        + "<span class='uk-input-group-addon'>"
                        + "<i class='uk-input-group-icon uk-icon-calendar future-date-only'></i>"
                        + "</span></div>"
                        + "</td>"
                        + "<td><input type='text' class='md-input remarks' disabled /></td>"

                        + "</tr>";


                });
                var $tr = $(tr);
                app.format($tr);
                $purchase_order_items_list.append($tr);
                freeze_header.resizeHeader(false);
            }


        });
    },
    set_batch_details: function (event, item) {
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
            $("#Batch" + id).val(item.value);
            $(row).find('.purchaserate').val(item.purchaseRate);
            $(row).find('.expirydate').text(item.expiryDate);
            $(row).find('.BatchID').val(item.id);
            $(row).find('.mrp').val(item.mrp);
            $(row).find('.Unit').val(item.unit);
            $(row).find('.UnitID').val(item.unitid);
        }
        var packsize = item.packsize;
        var receivedqty = clean($(row).find('.receivedqty').val());
        var offerqty = clean($(row).find('.offerQty').val());

        var looseqty = (offerqty + receivedqty) * packsize;
        var rate = clean($(row).find('.purchaserate').val());
        var looserate = (rate / looseqty);
        if (receivedqty != 0) {
            $(row).find('.looseqty').val(looseqty);
            $(row).find('.looserate').val(looserate);
        }
        self.calculate_total();

        // self.get_rate_on_batch_selection(item.value);
    },
    get_batches: function (release) {
        var hint = $(this.input).val();
        var BatchHint = $(this.input).parents('tr').find('.batch').val();
        var ItemHint = clean($(this.input).parents('tr').find('.item-id').val());

        $.ajax({
            url: '/Masters/Batch/GetBatchForGrnAutocomplete',
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
    },
    get_rate_on_batch_selection: function (Batch) {
        var self = GRN;
        var ItemID = $("#ItemID").val();
        //var Batch = $("#BatchNo").val();
        $.ajax({
            url: '/Purchase/DirectPurchaseInvoice/GetMRPForPurchaseInvoiceByBatchID',
            data: {
                ItemID: ItemID,
                Batch: Batch
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $.each(response.Data, function (i, record) {
                    // $("#MRP").val(record.MRP);
                    // $("#ExpDate").val(record.ExpDate);
                });
            }
        });
    },

    fill_grid: function () {
        var self = GRN;

        if ($('#grn-items-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                $('#grn-items-list tbody ').html('');
                self.grn_grid_fill();
            });
        }
        else {
            self.grn_grid_fill();

        }
    },
    grn_grid_fill: function () {
        var self = GRN;

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


        })

        $('#PurchaseOrderDate').val(maxDate);
        $.ajax({
            url: '/GRN/GetPurchaseOrderTrans/',
            //dataType: "json",
            data: {
                Areas: 'Purchase',
                PurchaseOrderIDS: purchase_order_ids,
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
                        var IsGSTRegistered = $("#IsGSTRegistered").val();
                        var StateID = $("#StateID").val();
                        var ShippingStateID = $("#ShippingStateID").val();
                        var igst = clean($(row).find('.IGSTPercent').val());
                        var Rate = clean($(row).find('.txtInvoiceRate').val());
                        var qty = clean($(row).find('.txtItemInvoiceQty').val());
                        var Discount = clean($(row).find('.DiscountAmount').val());
                        var DiscountPer = clean($(row).find('.DiscPer').val());
                        var QtyTolerencePer = clean($(row).find('.Toleranceper').val());
                        var POQty = clean($(row).find('.poqty').val());
                        var allowed_quantity = POQty * QtyTolerencePer / 100;
                        var total = (Rate * qty) - Discount;
                        var gstamount = igst * total / 100;
                        var totalamount = gstamount + total;
                        if (IsGSTRegistered == "false") {
                            IGSTAmt = 0.00;
                            SGSTAmt = 0.00;
                            CGSTAmt = 0.00;
                            IGSTPercent = 0.00;
                            SGSTPercent = 0.00;
                            CGSTPercent = 0.00;
                        }
                        else {
                            if (ShippingStateID != StateID) {
                                IGSTAmt = igst * total / 100;
                                SGSTAmt = 0.00;
                                CGSTAmt = 0.00;
                                IGSTPercent = igst;
                                SGSTPercent = 0.00;
                                CGSTPercent = 0.00;
                            }
                            //else if (item.SGSTAmt > 0 && item.CGSTAmt > 0 && item.IGSTAmt == 0)
                            else {
                                IGSTPercent = 0.00;
                                SGSTPercent = igst / 2;
                                CGSTPercent = igst / 2;
                                IGSTAmt = 0.00;
                                SGSTAmt = SGSTPercent * total / 100;
                                CGSTAmt = CGSTPercent * total / 100;
                            }
                        }
                        //GSTPercent = '<select class="md-input label-fixed GstPercentageID">' + purchase_invoice.Build_Select_Gst(GSTList, igst) + '</select>'
                        $(row).find('.SGSTPercent').val(SGSTPercent);
                        $(row).find('.CGSTPercent').val(CGSTPercent);
                        $(row).find('.IGSTPercent').val(IGSTPercent);
                        $(row).find('.SGSTAmt').val(SGSTAmt);
                        $(row).find('.CGSTAmt').val(CGSTAmt);
                        $(row).find('.IGSTAmt').val(IGSTAmt);
                        $(row).find('.NetAmount').val(totalamount)
                        //$(row).find('.gstPercentage option:selected').text(igst);
                        //$(row).find('.gstPercentage option:selected').val(igst);
                    });
                });
            }


        });
    },
    show_batch_with_stock: function (e) {

        var self = GRN;
        var row = $(this).closest("tr");
        var ItemName = $(row).find('.ItemName').text().trim();
        var ItemID = clean($(row).find('.item-id').val());
        var batch = $(row).find('.batch').val();
        var TransID = clean($(row).find('.POTransID').val());
        var UnitID = clean($(row).find('.UnitID').val());
        var unit = $(row).find('.Unit').val();
        var packsize = clean($(row).find(".PackSize").val());
        $("#ItemIDHidden").val(ItemID);

        if ((batch == "" || batch == " ") && e.which == 13) {
            self.batch_with_stock(ItemName, ItemID, batch, TransID, UnitID, unit, packsize);
        }
    },

    show_batch_stock: function (e) {

        var self = GRN;
        var ItemName = $("#ItemName").val();
        var ItemID = $("#ItemID").val();
        var BatchList = $("#txtBatch").val();
        var TransID = $("#TransID").val();
        var unit = $("#UnitID option:selected").text();
        var UnitID = $("#UnitID option:selected").val();
        var packsize = $("#packsize").val();
        $("#ItemIDHidden").val(ItemID);
        if ((BatchList == "" || BatchList == " ") && e.which == 13) {
            self.batch_stock(ItemName, ItemID, BatchList, TransID, UnitID, unit, packsize);
        }
    },


    batch_with_stock: function (ItemName, ItemID, batch, TransID, UnitID, unit, packsize) {

        $("#ItemName").val(ItemName);
        $("#ItemID").val(ItemID);
        $("#POTransID").val(TransID);
        $("#UnitID").val(UnitID);
        $("#Unit").val(unit);
        $("#PackSize").val(packsize);
        $("#ItemIDHidden").trigger('change');
        $('#show-batch-edit').trigger('click');
    },

    batch_stock: function (ItemName, ItemID, Batchlist, TransID, UnitID, unit, packsize) {

        $("#ItemName").val(ItemName);
        $("#ItemID").val(ItemID);
        $("#POTransID").val(TransID);
        $("#UnitID").val(UnitID);
        $("#Unit").val(unit);
        $("#PackSize").val(packsize);
        $("#ItemIDHidden").trigger('change');
        $('#show-batch-edit').trigger('click');
    },

    show_create_batch: function () {
        var self = GRN;
        var radio = $('#batch-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $("#ItemName").text().trim();
        var batchname = $(row).find(".BatchName").text();
        var ItemName = $("#ItemName").val();
        var itemid = $("#ItemID").val();
        var POTransID = $("#POTransID").val();
        var rate = $(row).find('.Price').text();
        var expiryDate = $(row).find('.ExpiryDate').val();
        var unitID = clean($(row).find(".UnitID").val());
        var unit = $(row).find(".Unit").val()
        var PackSize = clean($(row).find(".PackSize").val());
        var mrp = clean($(row).find(".retailMRP").val());
        //var GSTPercentage = clean($(row).find("#gstPercentage option:selected").text.val());


        var BatchID = ID;
        if (batchname != "Create new Batch") {
            self.Update_grid(itemid, POTransID, batchname, rate, expiryDate, BatchID, mrp, unit, unitID, PackSize);
            self.set_create_batch(batchname, rate, expiryDate, BatchID, mrp, Unit, UnitID, PackSize);
            UIkit.modal($('#batch-edit')).hide();

        }
        else {
            self.clear_batch();
            $('#show-add-batch').trigger('click');
            var date = $("#Date").val();
            PackSize = clean($('#PackSize').val());
            unitID = clean($('#UnitID').val());
            unit = $('#Unit').val();
            $("#add-batch #ItemName").val(ItemName);
            $("#add-batch #ItemID").val(itemid);
            $("#add-batch #POTransID").val(POTransID);
            $("#add-batch #Unit").val(unit);
            $("#add-batch #UnitID").val(unitID);
            $("#add-batch #PackSize").val(PackSize);
            //$("#add-batch #GSTPercentage").val(GSTPercentage);
            $("#add-batch #manufacture-date").val(date);
            $("#add-batch #unitSelected").text('packsize you have selected ' + PackSize)
            $("#BatchUnitID").val(unitID);
            $("#add-batch #BatchName").focus();

            self.get_latest_batch_details(itemid, PackSize, unitID);
        }
    },
    //show_create_batch: function () {
    //    var self = GRN;
    //    var row = $(this).closest('tr');
    //    var batchname = $(row).find(".BatchName").text();
    //    var ItemName = $("#ItemName").val();
    //    var itemid = clean($("#ItemID").val());
    //    var POTransID = $("#POTransID").val();
    //    var rate = clean($(row).find('.Price').text());
    //    var expiryDate = $(row).find('.ExpiryDate').text();
    //    var unitID = clean($('#UnitID').val());
    //    var unit = $('#Unit').val();
    //    var PackSize = $("#PackSize").val();
    //    if (batchname != "Create new Batch") {

    //        self.Update_grid(itemid, POTransID, batchname, rate, expiryDate);
    //        UIkit.modal($('#batch-edit')).hide();

    //    }
    //    else {
    //        self.clear_batch();
    //        $('#show-add-batch').trigger('click');
    //        $("#add-batch #ItemName").val(ItemName);
    //        $("#add-batch #ItemID").val(itemid);
    //        $("#add-batch #POTransID").val(POTransID);
    //        $("#add-batch #Unit").val(unit);
    //        $("#add-batch #UnitID").val(unitID);
    //        $("#add-batch #PackSize").val(PackSize);
    //        self.get_latest_batch_details(itemid);
    //    }
    //},
    get_latest_batch_details: function (itemid, packsize, unitID) {
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
                    $("#UnitID").val(data.data.UnitID);

                }
            },
        });
    },
    validate_form: function () {
        var self = GRN;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_item: function () {
        var self = GRN;
        if (self.rules.on_add_item.length) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },
    validate_grn_form: function () {
        var self = GRN;
        if (self.rules.on_grn_submit.length) {
            return form.validate(self.rules.on_grn_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = GRN;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_create_batch: function () {
        var self = GRN;
        if (self.rules.on_create_batch.length) {
            return form.validate(self.rules.on_create_batch);
        }
        return 0;
    },
    validate_grn_draft: function () {
        var self = GRN;
        if (self.rules.on_grn_draft.length) {
            return form.validate(self.rules.on_grn_draft);
        }
        return 0;
    },
    validate_on_filter: function () {
        var self = GRN;
        if (self.rules.on_filter.length > 0) {
            return form.validate(self.rules.on_filter);
        }
        return 0;
    },
    error_count: 0,
    get_data: function (IsDraft) {
        var model = {
            ID: $("#Id").val(),
            GRNNo: $("#GRNNo").val(),
            GRNDate: $("#txtDate").val(),
            SupplierID: clean($("#SupplierID").val()),
            WarehouseID: clean($("#StoreID").val()),
            InvoiceNo: $("#invoice-number").val(),
            InvoiceDate: $("#invoice-date").val(),
            IsDraft: IsDraft,
            grnItems: []
        };
        var object;
        $("#grn-items-list tbody tr.included").each(function (i, record) {
            object = {
                PurchaseOrderID: clean($(this).find('.purchase-order-id').val()),
                POTransID: clean($(this).find('.purchase-order-trans-id').val()),
                ItemID: clean($(this).find('.item-id').val()),
                Batch: $(this).find('.batch').val(),
                ExpiryDate: $(this).find('.expiry-date').val(),
                ReceivedQty: clean($(this).find('.received-qty').val()),
                AcceptedQty: clean($(this).find('.accepted-qty').val()),
                Remarks: $(this).find('.remarks').val(),
                IsQCRequired: $(this).find('.is-qc-required').val(),
                QtyTolerance: clean($(this).find('.toleranceqty').val()),
                UnitID: clean($(this).find('.UnitID').val()),

            };
            model.grnItems.push(object);
        });
        return model;
    },
    get_grn_data: function (IsDraft) {

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
            VATAmount: clean($("#VATAmount").val()),
            SuppDocAmount: clean($("#SuppDocAmount").val()),
            SuppShipAmount: clean($("#SuppShipAmount").val()),
            PackingForwarding: clean($("#PackingForwarding").val()),
            SuppOtherCharges: clean($("#SuppOtherCharges").val()),
            SuppFreight: clean($("#SuppFreight").val()),
            LocalCustomsDuty: clean($("#LocalCustomsDuty").val()),
            LocalFreight: clean($("#LocalFreight").val()),
            LocalMiscCharge: clean($("#LocalMiscCharge").val()),
            LocalOtherCharges: clean($("#LocalOtherCharges").val()),
            LocalLandinngCost: clean($("#LocalLandinngCost").val()),
            CurrencyExchangeRate: clean($("#CurrencyExchangeRate").val()),
            Remarks: $("#Remarks").val(),
            NetAmount: clean($("#NetAmt").val()),
            grnItems: []
        };
        var object;
        $("#grn-items-list tbody tr.included").each(function (i, record) {
            object = {
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
    },
    get_batch_data: function () {
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
            UnitID: clean($("#add-batch #UnitID").val()),
            ProfitPrice: clean($("#add-batch #ProfitPrice").val())
        };
        return model;
    },

    cancel: function () {
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
                        window.location = "/Purchase/GRN/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel GRN");
                    $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'block' });
                }
            },
        });

    },
    unit_change: function () {
        var self = GRN;
        var packsize = clean($("#add-batch #UnitID option:selected").data("packsize"));
        var unit = $("#add-batch #UnitID option:selected").data("unit");
        $("#add-batch #PackSize").val(packsize);
        $("#add-batch #Unit").val(unit);
        $("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
        self.calculate_profit();

        //var row = $(this).closest('tr');
        //var batchID = clean($(row).find('.BatchID').val());
        //var unitId = clean($(row).find(".UnitID").val());
        //if (batchID > 0) {
        //    app.confirm_cancel("Do you want to change unit?", function () {
        //        var id = clean($(row).find('.hdnItemID').val());
        //        var unitId = $(row).find("#UnitId option:selected").val();
        //        $(row).find('.purchaserate').val(0.0);
        //        $(row).find('.mrp').val(0.0);
        //        $(row).find('.BatchID').val(0);
        //        $(row).find('.discountAmt').val(0);
        //        $(row).find('.expirydate').text('');
        //        $(row).find('.expirydate').text('');
        //        $(row).find('#Batch').text(' ');
        //        $(row).find('#Batch').val(' ');
        //        $(row).find('.looseqty').val(0);
        //        $(row).find('.receivedqty').val(0);
        //        $(row).find('.offerQty').val(0);
        //        $(row).find("#UnitId option:selected").val(unitId);
        //    }, function () {
        //        // $(row).find("#UnitId option:selected").val(unitId);
        //        self.calculate_rate();
        //    })

        //}
        //else {
        //    // $(row).find("#UnitId option:selected").val(unitId);
        //    $(row).find('.looseqty').val(0);
        //    $(row).find('.receivedqty').val(0);
        //    $(row).find('.offerQty').val(0);
        //    self.calculate_rate();

        //}
    },

    calculate_rate: function () {
        var self = GRN;
        var row = $(this).closest('tr');
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var StateID = $("#StateID").val();
        var ShippingStateID = $("#ShippingStateID").val();
        var packsize = clean($(row).find(".PackSize").val());
        var discountpercent = clean($(row).find("#DiscountID option:selected").data("percent"));
        var offerqty = clean($(row).find('.offerQty').val());
        var receivedqty = clean($(row).find('.receivedqty').val());
        var looseqty = (receivedqty + offerqty) * packsize;
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
        $(row).find('.SGSTPercent').val(SGSTPercent);
        $(row).find('.CGSTPercent').val(CGSTPercent);
        $(row).find('.IGSTPercent').val(IGSTPercent);
        $(row).find('.SGSTAmt').val(SGSTAmt);
        $(row).find('.CGSTAmt').val(CGSTAmt);
        $(row).find('.IGSTAmt').val(IGSTAmt);
        $(row).find('.looseqty').val(looseqty);
        if (receivedqty != 0 && rate != 0) {

            $(row).find('.discountAmt').val(discountamount);
            self.calculate_total();
        }

    },
    calculate_total: function () {
        var discountamount = 0, discountpercent = 0, grossamount = 0, rate = 0, receivedqty = 0, igstamt = 0, cgstamt = 0, sgstamt = 0, netamount = 0, roundoff = 0;
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
        netamount = igstamt + cgstamt + sgstamt + grossamount - discountamount;
        roundoff = 0;
        discountpercent = discountamount / grossamount * 100;
        $("#DiscountAmt").val(discountamount);
        $("#DiscountPercent").val(discountpercent);
        $("#SGSTAmt").val(sgstamt);
        $("#CGSTAmt").val(cgstamt);
        $("#IGSTAmt").val(igstamt);
        $("#GrossAmt").val(grossamount);
        $("#NetAmt").val(netamount);
        $("#RoundOff").val(roundoff);
        freeze_header.resizeHeader(false);
    },


    calculate_profit: function () {
        var self = GRN;
        var retailMRP = clean($("#RetailMRP").val());
        var purchaseMRP = clean($("#PurchaseMRP").val());
        //var packSize = clean($("#add-batch #PackSize").val());
        var packSize = clean($("#add-batch #UnitID option:selected").data("packsize"));
        var retailLoose = retailMRP / packSize;
        var purchaseLoose = purchaseMRP / packSize;
        var profit = ((retailMRP - purchaseMRP) / (retailMRP)) * 100;
        $("#RetailLooseRate").val(retailLoose);
        $("#PurchaseLooseRate").val(purchaseLoose);
        $("#ProfitPrice").val(profit);

    },
    create_batch: function () {
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
                    UIkit.modal($('#add-batch')).hide();
                    self.set_create_batch(batchname, PurchaseMRP, expiryDate, data.data, RetailMRP, Unit, UnitID, PackSize);

                }
                else {

                    app.show_error(data.message);
                }
            },
        });
    },

    set_create_batch: function (batchname, PurchaseMRP, expiryDate, batchID, RetailMRP, Unit, UnitID, PackSize) {
        var self = GRN;
        $("#txtBatch").val(batchname);
        $("#txtBatchID").val(batchID);
        $("#MRP").val(RetailMRP);
        $("#Rate").val(PurchaseMRP);
        UIkit.modal($('#batch-edit')).hide();
    },

    clear_batch: function () {
        $("#BatchName").val('');
        $("#manufacture-date").val('');
        $("#expiry-date").val('');
        $("#RetailMRP").val('');
        $("#RetailLooseRate").val('');
        $("#PurchaseMRP").val('');
        $("#PurchaseLooseRate").val('');
        $("#ProfitPrice").val('');

    },
    Update_grid: function (itemid, POTransID, batchname, rate, expiryDate, batchID, mrp, unit, unitID, PackSize) {
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
                $(row).find('.SGSTPercent').val(SGSTPercent);
                $(row).find('.CGSTPercent').val(CGSTPercent);
                $(row).find('.IGSTPercent').val(IGSTPercent);
                $(row).find('.SGSTAmt').val(SGSTAmt);
                $(row).find('.CGSTAmt').val(CGSTAmt);
                $(row).find('.IGSTAmt').val(IGSTAmt);
                $(row).find('.looseqty').val(looseqty);
                if (receivedqty != 0 && rate != 0) {

                    $(row).find('.discountAmt').val(discountamount);
                    self.calculate_total();
                }
                self.calculate_rate();
            }
        });

    },
    grn_save: function () {
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
        var modal
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
                        window.location = "/Purchase/GRN/Index";
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
                    ////self.get_item_for_qrcode_generator(grnID);//(For Test Bar Code)
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
    },

    get_item_for_qrcode_generator: function (grnID) {
        var self = GRN;
        var IsBarCodeGenerator;

        IsBarCodeGenerator = $("#IsBarCodeGenerator").val();

        $.ajax({
            url: "/Purchase/GRN/GetItemForQRCodeGenerator",
            data: {
                GRNID: grnID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.qrcodeitems.splice(0, self.qrcodeitems.length)
                    $(response.Data).each(function (i, record) {
                        self.qrcodeitems.push({
                            BatchID: record.BatchID, Batch: record.Batch, ItemName: record.ItemName, ItemCode: record.ItemCode,
                            ItemID: record.ItemID, RetailMRP: record.RetailMRP
                        });
                    });
                }
                if (IsBarCodeGenerator == "False") {
                    $('#showw-qrcode').trigger('click');//(For Test Bar Code)
                    self.generate_qrcode();//(For Test Bar Code)
                }
                else {
                    $('#show-barcode').trigger('click');
                    self.generate_barcode();
                }
            }
        });
    },

    qrcodeitems: [],

    baseImages: [],

    qrElements: [],

    printQty: [],


    generate_qrcode: function () {
        var self = GRN;
        $("#qrcode").empty();
        var mainDiv = $("#qrcode");
        self.qrElements = self.qrcodeitems.map((item) => {
            var qrdiv = document.createElement("DIV");
            qrdiv.className = "qrcodegeneration";
            let qrElement = new QRCode(qrdiv);

            qrElement.makeCode(item.Batch + ":" + item.ItemCode);
            console.log(qrElement)
            qrElement._el.data_mydata = JSON.stringify(item);
            return qrElement._el;
        });


        self.qrElements.forEach((item) => {
            let data = JSON.parse(item.data_mydata)
            let p1 = document.createElement('DIV')
            let p2 = document.createElement('DIV')
            //p1.style.cssText = 'position:absolute;top:300px;left:300px;width:200px;height:200px;'
            //p1.style.cssText = 'width:150px;height:20px;font-size:8px;margin-top:1px;float:left;'
            //p2.style.cssText = 'width:150px;height:20px;font-size:8px;margin-top:1px;float:left;'
            p1.style.cssText = 'width:300px;font-size:11px;float:left;'
            p2.style.cssText = 'width:300px;font-size:11px;float:left;'
            //let text = document.createTextNode(`Batch : ${data.Batch}, Item : ${data.ItemName}`)
            let text1 = document.createTextNode(`Batch : ${data.Batch}`)
            let text2 = document.createTextNode(`Item : ${data.ItemName}`)
            p1.append(text1)
            p2.append(text2)
            item.append(p1)
            item.append(p2)
            mainDiv.append(item)
            self.baseImages.push({ BatchID: data.BatchID, ItemID: data.ItemID, QRCode: item.title });
        });
        $('#show-qrcode').trigger('click');
    },


    generate_barcode: function () {
        var self = GRN;
        var mainDiv = $("#barcode");
        $("#barcode").empty();
        var i = 0
        var content = "";
        var classname;
        var $content;
        var barcodedata;
        var textdata;
        self.qrcodeitems.forEach((item) => {
            i = i + 1;
            classname = 'generatebarcode' + i;
            barcodedata = self.leftpad(item.BatchID, 5);  //+ "-" + item.ItemCode
            textdata = self.rightpad(item.ItemName.substring(0, 20), 20) + " Rs" + self.leftpad(item.RetailMRP, 7);
            var PrintQty = 0;
            barcodedata = self.leftpad(item.Batch, 5);  //+ "-" + item.ItemCode
            textdata = self.rightpad(item.ItemName.substring(0, 20), 20) + " Rs" + self.leftpad(item.RetailMRP, 7);
            var PrintQty = 0;
            self.grnItems.forEach(function (e) {
                if (item.BatchID == e.BatchID) {
                    PrintQty = e.PrintQty
                    return;
                }
            });
            //self.printQty = $.grep(self.PrintQtyItems, function (element, index) { return element.BatchID == item.BatchID; });
            //barcodedata = "*" + item.Batch + "*" //+ "-" + item.ItemCode

            var j;
            for (j = 0; j < PrintQty; j++) {
                classname = 'generatebarcode' + i + j;
                content = '<span style="padding:10px;"><svg width="158px" height="72px" class="barcode_print ' + classname + '">' + '</svg></span>'
                $content = $(content);
                $('#barcode').append($content);
                $('.' + classname).JsBarcode(barcodedata, {
                    width: 1,
                    height: 40,
                    textmargin: 0,
                    fontoptions: "bold",
                    fontSize: 6,
                    //margin: 20,
                    text: textdata.substring(0, 30),
                    textAlign: "right",
                    marginLeft: 0,
                    marginTop: 40,
                    marginBottom: 0,
                    marginRight: 0,

                    //format: "Code39",
                    mod43: true
                });
            }


        });
        self.print_bar_code();
        //$('#show-barcode').trigger('click');
    },

    leftpad: function (str, max) {
        str = str.toString();
        return str.length < max ? GRN.leftpad(" " + str, max) : str;
    },

    rightpad: function (str, max) {
        str = str.toString();
        return str.length < max ? GRN.rightpad(str + " ", max) : str;
    },

    save: function () {
        var self = GRN;
        var IsNew = false, IsDraft = false;
        self.error_count = 0;
        var url = '/Purchase/GRN/Save';
        if ($(this).hasClass("save-draft-grn")) {
            IsDraft = true;
            url = '/Purchase/GRN/SaveAsDraft'
        }
        if ($(this).hasClass("save-grn-new")) {
            IsNew = true;
        }
        self.error_count = ((IsDraft == true) ? self.validate_draft() : self.validate_form());
        if (self.error_count > 0) {
            return;
        }
        $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'none' });
        var modal;
        modal = self.get_data(IsDraft);
        $.ajax({
            url: url,
            data: modal,
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("GRN created successfully");
                    if (IsNew == true) {
                        setTimeout(function () {
                            window.location = "/Purchase/GRN/Create";
                        }, 1000);
                    }
                    else {
                        setTimeout(function () {
                            window.location = "/Purchase/GRN/Index";
                        }, 1000);
                    }
                } else if (data.Status == "entered") {
                    app.show_error("Some of the items in the purchase order are already received");
                    $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'block' });
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    // app.show_error("Failed to create GRN");
                    $('.save-grn, .save-grn-new, .save-draft-grn,.cancel').css({ 'display': 'block' });
                }
            },
        });
    },

    rules: {
        on_add_item: [
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                    { type: form.required, message: "Please select supplier" },
                ]
            },
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                ]
            },
            {
                elements: "#txtBatch",
                rules: [
                    { type: form.required, message: "Please enter a valid Batch" },
                    { type: form.non_zero, message: "Please enter a valid Batch" },
                    //{ type: form.positive, message: "Please enter a valid Batch" },
                ]
            },

        ],
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#txtDate",
                rules: [
                    { type: form.required, message: "Please enter GRN date" },
                    { type: form.past_date, message: "Invalid GRN date" },
                    {
                        type: function (element) {
                            var date = new Date();
                            var relaxation = $(element).data('relaxation');
                            var relaxation_date = new Date();
                            relaxation_date.setDate(date.getDate() + relaxation);
                            var relaxation_day = new Date(relaxation_date.getFullYear(), relaxation_date.getMonth(), relaxation_date.getDate());
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            if (used_date == "Invalid GRN date") {
                                return false;
                            }
                            return used_date.getTime() >= relaxation_day.getTime();
                        }, message: "Invalid GRN date"
                    },
                    {
                        type: function (element) {
                            var dateTypeVar = $('#PurchaseOrderDate').val();
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#PurchaseOrderDate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "GRN date must be within purchase order date and current date"
                    }
                ]
            },
            {
                elements: ".expiry-date:visible",
                rules: [
                    { type: form.future_date_only, message: "Invalid expiry date" },
                ]
            },
            {
                elements: "#invoice-date",
                rules: [
                    { type: form.required, message: "Invalid invoice date" },
                    { type: form.past_date, message: "Invalid invoice date" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);

                            var po_date = $('#PurchaseOrderDate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);

                            return used_date.getTime() >= po_datesplit.getTime();
                        }, message: "Invoice date must be within maximum of selected purchase order date and current date"
                    }
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                ]
            },
            {
                elements: "#StoreID",
                rules: [
                    { type: form.non_zero, message: "Please select store" },
                ]
            },
            {
                elements: "#invoice-number",
                rules: [
                    { type: form.required, message: "Please enter invoice number" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = clean($('#invoice-count').val());
                            if (count > 0)
                                error = true;
                            return !error;
                        }, message: 'Invoice number already entered for this supplier'
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .received-qty",
                rules: [
                    { type: form.required, message: "Please enter received quantity" },
                    { type: form.non_zero, message: "Please enter received quantity" },
                    { type: form.numeric, message: "Please enter valid received quantity" },
                    { type: form.positive, message: "Please enter positive received quantity" },
                    {
                        type: function (element) {
                            var received_quantity = clean($(element).val());
                            var quantity = $(element).closest('tr').find('.quantity').val();
                            var qty_tolerance_percent = $(element).closest('tr').find('.qty-tolerance-percent').val();
                            var pending_po_quantity = $(element).closest('tr').find('.pending-po-quantity').val();
                            var allowed_quantity = parseFloat(pending_po_quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                            return received_quantity <= allowed_quantity;

                        }, message: "Received quantity exceeds tolerance"
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .accepted-qty",
                rules: [
                    { type: form.required, message: "Please enter accepted quantity" },
                    { type: form.numeric, message: "Please enter valid accepted quantity" },
                    { type: form.positive, message: "Please enter positive accepted quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.received-qty').val());
                        }, message: "Accepted quantity exceeds received quantity "
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .batch:visible",
                rules: [
                    {
                        type: function (element) {
                            var item_category = $(element).closest('tr').find('.item-category').val();
                            if (item_category.indexOf('Finished Good') != -1) {
                                if ($(element).val() == "") {
                                    return false;
                                }
                            }
                            return true;
                        }, message: "Batch is required for FG items"
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .expiry-date:visible",
                rules: [
                    {
                        type: function (element) {
                            var item_category = $(element).closest('tr').find('.item-category').val();
                            if (item_category.indexOf('Finished Good') != -1) {
                                if ($(element).val() == "") {
                                    return false;
                                }
                            }
                            return true;
                        }, message: "Please enter expiry date for FG items"
                    },
                    { type: form.future_date, message: "Invalid expiry date" }
                ]
            },
        ],
        on_grn_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#txtDate",
                rules: [
                    { type: form.required, message: "Please enter GRN date" },

                ]
            },

            {
                elements: "#invoice-date",
                rules: [
                    { type: form.required, message: "Invalid invoice date" },
                    { type: form.past_date, message: "Invalid invoice date" },

                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                ]
            },
            {
                elements: "#StoreID",
                rules: [
                    { type: form.non_zero, message: "Please select store" },
                ]
            },
            {
                elements: "#invoice-number",
                rules: [
                    { type: form.required, message: "Please enter invoice number" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = clean($('#invoice-count').val());
                            var id = clean($('#Id').val())
                            if ((count > 0) && (id == 0))
                                error = true;
                            return !error;
                        }, message: 'Invoice number already entered for this supplier'
                    },
                ]
            },
            //{
            //    elements: "#grn-items-list .included .receivedqty",
            //    rules: [
            //        { type: form.required, message: "Please enter received quantity" },
            //        { type: form.non_zero, message: "Please enter received quantity" },
            //        { type: form.numeric, message: "Please enter valid received quantity" },
            //        { type: form.positive, message: "Please enter positive received quantity" },
            //         {
            //             type: function (element) {
            //                 var error = false;
            //                 var poqty = clean($(element).closest('tr').find('.poqty').val());
            //                 var receivedqty = clean($(element).val());


            //                 if ((receivedqty > poqty))
            //                     error = true;
            //                 return !error;
            //             }, message: 'Received qty should not exceed pending po quantity'
            //         },

            //    ]
            //},
            {
                elements: "#grn-items-list .included .receivedqty",
                rules: [
                    { type: form.required, message: "Please enter received quantity" },
                    { type: form.non_zero, message: "Please enter received quantity" },
                    { type: form.numeric, message: "Please enter valid received quantity" },
                    { type: form.positive, message: "Please enter positive received quantity" },
                    {
                        type: function (element) {
                            var received_quantity = clean($(element).val());
                            var quantity = clean($(element).closest('tr').find('.poqty').val());
                            var qty_tolerance_percent = clean($(element).closest('tr').find('.Toleranceper').val());
                            var pending_po_quantity = clean($(element).closest('tr').find('.poqty').val());
                            //var allowed_quantity = parseFloat(pending_po_quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                            var allowed_quantity = parseFloat(pending_po_quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                            return received_quantity <= allowed_quantity;

                        }, message: "Received quantity exceeds tolerance"
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .discountAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid discount amount" },
                    { type: form.positive, message: "Please enter positive discount amount" },

                ]
            },
            {
                elements: "#grn-items-list .included .purchaserate",
                rules: [

                    { type: form.non_zero, message: "Please enter valid purchase rate" },
                ]
            },
            {
                elements: "#IGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid IGST amount" },
                    { type: form.positive, message: "Please enter positive IGST amount" },

                ]
            },

            {
                elements: "#SGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid SGST amount" },
                    { type: form.positive, message: "Please enter positive SGST amount" },

                ]
            },

            {
                elements: "#CGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid CGST amount" },
                    { type: form.positive, message: "Please enter positive CGST amount" },

                ]
            },

            {
                elements: "#grn-items-list .included .offerQty",
                rules: [
                    { type: form.numeric, message: "Please enter valid offer quantity" },
                    { type: form.positive, message: "Please enter positive offer quantity" },

                ]
            },
        ],
        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#txtDate",
                rules: [
                    { type: form.required, message: "Please enter GRN date" },
                    { type: form.past_date, message: "Invalid GRN date" },
                    {
                        type: function (element) {
                            var date = new Date();
                            var relaxation = $(element).data('relaxation');
                            var relaxation_date = new Date();
                            relaxation_date.setDate(date.getDate() + relaxation);
                            var relaxation_day = new Date(relaxation_date.getFullYear(), relaxation_date.getMonth(), relaxation_date.getDate());
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            if (used_date == "Invalid GRN date") {
                                return false;
                            }
                            return used_date.getTime() >= relaxation_day.getTime();
                        }, message: "Invalid GRN date"
                    },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                ]
            },
            {
                elements: "#StoreID",
                rules: [
                    { type: form.non_zero, message: "Please select store" },
                ]
            },
            {
                elements: "#grn-items-list .included .batch:visible",
                rules: [
                    {
                        type: function (element) {
                            var item_category = $(element).closest('tr').find('.item-category').val();
                            if (item_category.indexOf('Finished Good') != -1) {
                                if ($(element).val() == "") {
                                    return false;
                                }
                            }
                            return true;
                        }, message: "Please enter batch for FG items"
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .expiry-date:visible",
                rules: [
                    {
                        type: function (element) {
                            var item_category = $(element).closest('tr').find('.item-category').val();
                            if (item_category.indexOf('Finished Good') != -1) {
                                if ($(element).val() == "") {
                                    return false;
                                }
                            }
                            return true;
                        }, message: "Please enter expiry date for FG items"
                    },
                    { type: form.future_date, message: "Please enter expiry date" }
                ]
            },
            {
                elements: "#grn-items-list .included .received-qty",
                rules: [
                    { type: form.required, message: "Please enter received quantity" },
                    { type: form.non_zero, message: "Please enter received quantity" },
                    { type: form.numeric, message: "Please enter valid received quantity" },
                    { type: form.positive, message: "Please enter positive received quantity" },
                    {
                        type: function (element) {
                            var received_quantity = clean($(element).val());
                            var quantity = $(element).closest('tr').find('.quantity').val();
                            var qty_tolerance_percent = $(element).closest('tr').find('.qty-tolerance-percent').val();
                            var pending_po_quantity = $(element).closest('tr').find('.pending-po-quantity').val();
                            var allowed_quantity = parseFloat(pending_po_quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                            return received_quantity <= allowed_quantity;

                        }, message: "Received quantity exceeds tolerance"
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .accepted-qty",
                rules: [
                    { type: form.required, message: "Please enter accepted quantity" },
                    { type: form.numeric, message: "Please enter valid accepted quantity" },
                    { type: form.positive, message: "Please enter positive accepted quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.received-qty').val());
                        }, message: "Accepted quantity exceeds received quantity "
                    },
                ]
            },
        ],

        on_grn_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#txtDate",
                rules: [
                    { type: form.required, message: "Please enter GRN date" },

                ]
            },

            {
                elements: "#invoice-date",
                rules: [
                    { type: form.required, message: "Invalid invoice date" },
                    { type: form.past_date, message: "Invalid invoice date" },

                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                ]
            },
            {
                elements: "#StoreID",
                rules: [
                    { type: form.non_zero, message: "Please select store" },
                ]
            },
            {
                elements: "#invoice-number",
                rules: [
                    { type: form.required, message: "Please enter invoice number" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = clean($('#invoice-count').val());
                            if (count > 0)
                                error = true;
                            return !error;
                        }, message: 'Invoice number already entered for this supplier'
                    },
                ]
            },
            {
                elements: "#grn-items-list .included .receivedqty",
                rules: [
                    { type: form.required, message: "Please enter received quantity" },
                    { type: form.non_zero, message: "Please enter received quantity" },
                    { type: form.numeric, message: "Please enter valid received quantity" },
                    { type: form.positive, message: "Please enter positive received quantity" },

                ]
            },
            {
                elements: "#grn-items-list .included .discountAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid discount amount" },
                    { type: form.positive, message: "Please enter positive discount amount" },

                ]
            },
            {
                elements: "#IGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid IGST amount" },
                    { type: form.positive, message: "Please enter positive IGST amount" },

                ]
            },

            {
                elements: "#SGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid SGST amount" },
                    { type: form.positive, message: "Please enter positive SGST amount" },

                ]
            },

            {
                elements: "#CGSTAmt",
                rules: [

                    { type: form.numeric, message: "Please enter valid CGST amount" },
                    { type: form.positive, message: "Please enter positive CGST amount" },

                ]
            },

            {
                elements: "#grn-items-list .included .offerQty",
                rules: [
                    { type: form.numeric, message: "Please enter valid offer quantity" },
                    { type: form.positive, message: "Please enter positive offer quantity" },

                ]
            },

        ],
        on_create_batch: [
            {
                elements: "#BatchName",
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
                elements: "#expiry-date",
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
            {
                elements: "#add-batch #UnitID",
                rules: [
                    { type: form.non_zero, message: "Please select unit" },
                ]
            },
        ],
        on_filter: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.non_zero, message: "Please select item" },
                    { type: form.required, message: "Please select item" },
                ]
            },

        ],
    },

    save_qr_code_confirm: function () {
        var self = GRN;
        app.confirm_cancel("Do you want to Save", function () {
            self.save_qr_code();
        }, function () {
        })
    },

    save_qr_code: function () {
        var self = GRN;
        var modal = self.get_data_for_qr_code();
        $.ajax({
            url: '/Purchase/GRN/SaveQRCode',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("QRCode Saved successfully");
                    $("#btnQRCodeSave").hide();
                    $("#btnPrintQRCode").show();
                } else {

                }
            },
        })
    },

    get_data_for_qr_code: function () {
        var self = GRN;
        var data = {};
        data.QRCodeItems = [];
        data.QRCodeItems = self.baseImages;
        return data;
    },

    print_qr_code: function () {
        var self = GRN;
        var height = $(document).height() - $("#print-content").offset().top - $("#print-preview-common .uk-modal-footer").height() - 100;
        $("#print-content").html("<iframe id='IFramePDF'></iframe>");
        var iframeBody = $("#IFramePDF").contents().find("body");
        var qrdiv = document.createElement("DIV");
        self.qrElements.forEach((item) => {
            console.log(item)
            item.children[1].style.height = "80px";
            item.children[1].style.width = "80px";
            item.children[1].style.float = "bottom";
            item.children[1].style.paddingTop = "2px";

            item.style.height = "80px";
            item.style.width = "80px";
            item.style.float = "bottom";
            item.style.margin = "6px";
            item.style.paddingBottom = "20px"
            //item.style.marginBottom = "6px";

            let data = JSON.parse(item.data_mydata)
            qrdiv.append(item)
        });



        var styleTag = iframeBody.append(qrdiv);
        $("#print-content").height(height);
        var $IFrame = $("#IFramePDF");
        $IFrame.css({ 'width': '100%', 'height': '100%' });
        UIkit.modal("#print-preview-common").show();
    },
    //Pending
    print_bar_code: function () {
        var self = GRN;
        var height = $(document).height() - $("#print-content").offset().top - $("#print-preview-common .uk-modal-footer").height() - 100;
        $("#print-content").html("<iframe id='IFramePDF'></iframe>");
        var iframeBody = $("#IFramePDF").contents().find("body");
        iframeBody.append($("#barcode").html());
        $("#print-content").height(height);
        var $IFrame = $("#IFramePDF");
        $IFrame.css({ 'width': '100%', 'height': '100%' });
        UIkit.modal("#print-preview-common").show();
    },

    print_close: function () {
        var url = '/Purchase/GRN/Index';
        setTimeout(function () {
            window.location = url
        }, 1000);
    },

    print_qr_code_close: function () {
        var url = '/Purchase/GRN/PrintQRCode';
        setTimeout(function () {
            //window.location = url
            window.close
        }, 1000);
    },
}