var fh_items;
SalesInvoice = {

    init: function () {
        var self = SalesInvoice;

        Customer.customer_list("sales-invoice");
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#SalesOrderNos",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });

        self.sales_order_list();
        self.proforma_invoice_list();

        $('#sales-order-list').SelectTable({
            selectFunction: self.select_sales_order,
            returnFocus: "#TurnoverDiscount",
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        });

        $('#proforma-invoice-list').SelectTable({
            selectFunction: self.select_proforma_invoice,
            returnFocus: "#PaymentModeID",
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        });


        self.bind_events();
        self.Invoice = {};
        self.on_change_customer_category();
        self.Invoice.InvoiceNo = $("#InvoiceNo").val();
        self.Invoice.InvoiceDate = $("#InvoiceDate").val();
        self.Invoice.AmountDetails = [];
        self.Invoice.PackingDetails = [];
        self.Invoice.SalesTypeID = $("#ItemCategoryID").val();

        if ($("#CustomerID").val() != 0) {
            var ItemIDS = [];
            var UnitIDS = [];
            $("#sales-invoice-items-list tbody tr").each(function () {
                ItemIDS.push($(this).find('.ItemID').val());
                UnitIDS.push($(this).find('.UnitID').val());
            });
            self.get_offer_details_bulk(ItemIDS, UnitIDS);

            self.set_object_values();
        }
        self.freeze_headers();


    },

    list: function () {
        var self = SalesInvoice;
        $('#tabs-salesinvoice').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
        $("body").on('click', ".btnprint", self.SalesInvoiceDetail);
        $("body").on('click', ".btnexport", self.SalesInvoiceExportDetail);


    },

    tabbed_list: function (type) {
        var $list;

        switch (type) {
            case "Draft":
                $list = $('#draft-list');
                break;
            case "SavedInvoices":
                $list = $('#savedInvoices-list');
                break;
            case "PartiallyReceived":
                $list = $('#partiallyReceived-list');
                break;
            case "FullyReceived":
                $list = $('#fullyReceived-list');
                break;
            case "BadAndDoubtfulInvoices":
                $list = $('#badAndDoubtfulInvoices-list');
                break;
            case "Cancelled":
                $list = $('#cancelled-list');
                break;
            default:
                $list = $('#draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Sales/SalesInvoice/GetSalesInvoiceList?type=" + type;

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
                    { "data": "InvoiceDate", "className": "InvoiceDate" },
                    { "data": "SalesType", "className": "SalesType" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "Location", "className": "Location" },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },

                    {
                        "data": "", "searchable": false, "className": "action print", "orderable": false,
                        "render": function (data, type, row, meta) {
                           
                            return  "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnprint' value = '" + row.ID + "'>Print</button>"
                            }
                        }
                    
                    ,

                    {
                        "data": "", "searchable": false, "className": "action export", "orderable": false,
                        "render": function (data, type, row, meta) {
                            
                            return"<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnexport' value = '" + row.ID + "'>Export</button>" 
                            }
                        }
                    

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                    /*$list.find('tbody td:not(.ID)').on('click', function () {*/
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/SalesInvoice/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = SalesInvoice;

        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("#ItemCategoryID").on("change", self.on_item_category_change);
        $("#btnOKCustomer").on("click", self.select_customer);
        $(".btnSave").on("click", self.save_confirm);
        $(".btnSaveASDraft").on("click", self.save_draft);
        $(".btnSaveAndPrint").on("click", self.save_confirm);
        $(".ItemCode").on("click", self.SalesInvoiceWithCode);
        $(".PartNo").on("click", self.SalesInvoicePartNo);
        $(".ExportItemCode").on("click", self.SalesInvoiceExportitemcode);
        $(".ExportPartNo").on("click", self.SalesInvoiceExportpartno);

       



        $("body").on("ifChanged", "#CashDiscountEnabled", self.recalculate);
        $("#btnOkSalesOrder").on('click', self.select_sales_order);
        $("#btnOkProformaInvoice").on('click', self.select_proforma_invoice);
        $('body').on('click', '#sales-invoice-items-list tbody tr.sales-order td:not(.action)', self.show_batch_edit_modal);
        $('body').on('click', '#sales-invoice-items-list tbody tr td.showitemhistory', self.showitemhistory);
        $('body').on('keyup change', '#batch-list .InvoiceQty', self.set_total_invoice_qty);
        $('body').on('keyup change', '#batch-list .InvoiceOfferQty', self.set_total_invoice_offer_qty);
        $('body').on('keyup', '#sales-invoice-items-list tbody .DiscountPercentage, .DiscountAmount', self.set_item_discount);
        $('#DiscountPercentage, #DiscountAmount, #OtherCharges').on('keyup', self.set_total_discount);
        $("body").on("change", "#VATPercentageID", self.set_total_discount);
        $('body').on('click', '#btnOkBatches', self.replace_batches);
        $("body").on('change keyup', "#BatchQty", self.set_offer_qty);
        $("body").on('click', ".cancel", self.cancel_confirm);

        $("body").on('click', ".btnprint", self.SalesInvoiceDetail);
        $("body").on('click', ".btnexport", self.SalesInvoiceExportDetail);


        $("body").on('click', ".cancel", self.cancel_confirm);

        $("body").on("click", ".print-sales-invoice", app.get_print_preferences);
        $("body").on('click', ".sales-order .remove-item", self.remove_item);
        $("body").on("change", "#CustomerCategoryID", self.change_customer_category);
    },
    sales_invoice_history: function () {

        var $list = $('#sales-history-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesInvoice/GetSalesInvoiceHistory";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemID", Value: clean($('#HistoryItemID').val()) },
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
                    { "data": "SalesOrderNos", "className": "SalesOrderNos" },
                    { "data": "InvoiceDate", "className": "InvoiceDate" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "Itemcode", "className": "Itemcode" },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "PartsNumber", "className": "PartsNumber" },
                    {
                        "data": "MRP", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.MRP + "</div>";
                        }
                    },
                    {
                        "data": "Quantity", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Quantity + "</div>";
                        }
                    },
                    { "data": "Unit", "className": "Unit" },
                    {
                        "data": "DiscountPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                        }
                    },
                    {
                        "data": "VATPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.VATPercentage + "</div>";
                        }
                    },
                    {
                        "data": "NetAmount", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("click", '#show-history', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page'); 
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    purchase_order_history: function () {

        var $list = $('#purchase-history-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesInvoice/GetPurchaseHistory";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemID", Value: clean($('#HistoryItemID').val()) },
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
                    { "data": "PurchaseOrderNo", "className": "" },
                    { "data": "PurchaseOrderDate", "className": "" },
                    { "data": "SupplierName", "className": "" },
                    { "data": "Itemcode", "className": "" },
                    { "data": "ItemName", "className": "" },
                    { "data": "PartsNumber", "className": "" },
                    {
                        "data": "LandedCost", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.LandedCost + "</div>";
                        }
                    },
                    {
                        "data": "SecondaryRate", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.SecondaryRate + "</div>";
                        }
                    },
                    {
                        "data": "SecondaryQty", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.SecondaryQty + "</div>";
                        }
                    },
                    { "data": "SecondaryUnit", "className": "" },
                    {
                        "data": "DiscountPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                        }
                    },
                    {
                        "data": "VATPercentage", "className": "",
                         "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.VATPercentage + "</div>";
                        }
                    },
                    {
                        "data": "NetAmount", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("click", '#show-history', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    change_customer_category: function () {
        var self = SalesInvoice;
        var customerid = clean($('#CustomerID').val());
        if (customerid > 0) {
            app.confirm("Selected customer will be removed", function () {
                $('#CustomerID').val(0);
                $('#CustomerName').val('');
                $("#BillingAddressID").val('');
                $("#ShippingAddressID").val('');
                self.clear_items();
                self.on_change_customer_category();
            })
        }
        else {
            self.on_change_customer_category();
        }
    },

    on_change_customer_category: function () {
        var self = SalesInvoice;
        var CustomerCategory = $("#CustomerCategoryID option:selected").text().toLowerCase();
        var id = clean($("#ID").val());
        if (CustomerCategory == 'ecommerce') {
            $(".ecommerce-hide").removeClass("uk-hidden");
            var Freight = clean($("#FreightAmount").val());
            var FreightTax = clean($("#FreightTax").val());
            var TotalFreight = (Freight * 100) / (100 - FreightTax)
            $("#TotalFreightAmt").val(TotalFreight);
            self.set_amount_details();
        }
        else {
            $(".ecommerce-hide").addClass("uk-hidden");
            $("#TotalFreightAmt").val(0);
            $("#FreightTaxAmt").val(0);
            $("#FreightAmount").val(0);
        }

    },


    remove_item: function () {
        var self = SalesInvoice;
        var ItemID = $(this).closest("tr").find(".ItemID").val();
        self.Items = $.grep(self.Items, function (e) { return e.ItemID != ItemID });
        self.process_invoice(self.Items);
    },

    save_confirm: function () {
        var self = SalesInvoice;
        IsPrint = false;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveAndPrint")) {
            IsPrint = true;
        }
        app.confirm_cancel("Do you want to Save", function () {
            IsDraft = false;
            self.save();
        }, function () {
        })
    },

    save_draft: function () {
        var self = SalesInvoice;
        IsPrint = false;
        self.error_count = 0;
        self.error_count = self.validate_form_for_draft();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveASDraft")) {
            IsDraft = true;
            self.save();
        }
    },

    set_object_values: function () {
        var self = SalesInvoice;
        self.Invoice.ID = $("#ID").val();
        self.Invoice.CustomerID = $("#CustomerID").val();
        self.Invoice.SalesOrderNos = $("#SalesOrderNos").val();
        self.Invoice.BillingAddressID = $("#BillingAddressID").val();
        self.Invoice.ShippingAddressID = $("#ShippingAddressID").val();
        self.Invoice.CustomerPONo = $("#CustomerPONo").val();
        self.Invoice.CessAmount = clean($("#CessAmount").val());
        self.Invoice.CashDiscount = clean($("#CashDiscount").val());
        self.Invoice.OtherCharges = clean($("#OtherCharges").val());
        var item = {};
        self.Items = [];
        $("#sales-invoice-items-list tbody tr").each(function (i, record) {
            item = {
                ItemName: $(record).find(".ItemName").text().trim(),
                ItemID: $(record).find(".ItemID").val(),
                UnitName: $(record).find(".UnitName").text().trim(),
                SecondaryUnit: $(record).find(".SecondaryUnit").text().trim(),
                Code: $(record).find(".Code").val(),
                UnitID: $(record).find(".UnitID").val(),
                CGSTPercentage: clean($(record).find(".CGSTPercentage").val()),
                IGSTPercentage: clean($(record).find(".IGSTPercentage").val()),
                SGSTPercentage: clean($(record).find(".SGSTPercentage").val()),
                VATPercentage: clean($(record).find(".VATPercentage").val()),
                IsVat: clean($(record).find(".IsVat").val()),
                IsGST: clean($(record).find(".IsGST").val()),
                PartsNumber: $(record).find(".PartsNumber").val(),
                Remarks: $(record).find(".Remarks").val(),
                Model: $(record).find(".Model").val(),
                DiscountPercentage: clean($(record).find(".DiscountPercentage").val()),
                Qty: clean($(record).find(".OrderQty").val()),
                SecondaryUnitSize: clean($(record).find(".SecondaryUnitSize").val()),
                OfferQty: clean($(record).find(".OfferQty").val()),
                StoreID: clean($("#StoreID").val()),
                SalesOrderItemID: clean($(record).find(".SalesOrderTransID").val()),
                ProformaInvoiceTransID: clean($(record).find(".ProformaInvoiceTransID").val()),
                BatchID: clean($(record).find(".BatchID").val()),
                BatchName: $(record).find(".BatchName").text(),
                CurrencyID: clean($(record).find(".CurrencyID").val()),
                CurrencyName: $(record).find(".CurrencyName").val(),
                BatchTypeID: clean($(record).find(".BatchTypeID").val()),
                BatchTypeName: $(record).find(".BatchType").text(),
                MRP: clean($(record).find(".MRP").val()),
                SecondaryMRP: clean($(record).find(".SecondaryMRP").val()),
                Stock: clean($(record).find(".Stock").val()),
                InvoiceQty: clean($(record).find(".Qty").val()),
                SecondaryInvoiceQty: clean($(record).find(".SecondaryQty").val()),
                InvoiceOfferQty: clean($(record).find(".InvoiceOfferQty").val()),
                SecondaryInvoiceOfferQty: clean($(record).find(".SecondaryInvoiceOfferQty").val()),
                IsIncluded: true,
                InvoiceOfferQtyMet: $(record).hasClass("offer-qty-not-met") ? false : true,
                InvoiceQtyMet: $(record).hasClass("qty-not-met") ? false : true,
                SalesUnitID: clean($(record).find(".SalesUnitID").val()),
                Rate: clean($(record).find(".Rate").val()),
                LooseRate: clean($(record).find(".LooseRate").val()),
                CessAmount: clean($(record).find(".CessAmount").val()),
                CessPercentage: clean($(record).find(".CessPercentage").val()),
                BatchTypeName: $(record).find(".BatchType").text(),
                PackSize: clean($(record).find(".PackSize").val()),
                PrimaryUnit: $(record).find(".PrimaryUnit").val(),
                CategoryID: $(record).find(".CategoryID").val(),
            }
            self.Items.push(item);
        });
        self.Items = self.process_items(self.Items);
        self.set_item_calculations();
        self.calculate_grid_total();
        self.set_amount_details();
        self.set_packing_details();


        //  self.get_discount_details();
        if (clean($("#CashDiscount").val()) == 0) { $("input#CashDiscountEnabled").iCheck('uncheck'); }
    },

    select_sales_order: function () {
        var self = SalesInvoice;
        var row;
        var ID = [];
        var TransNos = "";
        var FregihtAmt = 0;
        var SalesTypeID = 0;
        var checkboxes = $('#sales-order-list tbody input[type="checkbox"]:checked');


        var ShippingAddressID = clean($('#sales-order-list tbody input[type="checkbox"]:checked').eq(0).closest('tr').find('.ShippingAddressID').val());
        var BillingAddressID = clean($('#sales-order-list tbody input[type="checkbox"]:checked').eq(0).closest('tr').find('.BillingAddressID').val());

        var ShippingCount = 0, BillingCount = 0;
        if (checkboxes.length > 0) {
            $("#sales-order-list tbody input[type='checkbox']:checked ").each(function () {
                var row = $(this).closest('tr');
                if ($(row).find('.BillingAddressID').val() != BillingAddressID) {
                    BillingCount = 1;
                }
                if ($(row).find('.ShippingAddressID').val() != ShippingAddressID) {
                    ShippingCount = 1;
                }
            });

            if (BillingCount > 0) {
                app.show_error("Can not bill to different addresses");
                return;
            }
            else if (ShippingCount > 0) {
                app.show_error("Can not ship to different addresses");
                return;
            }
            else if ($("#sales-invoice-items-list tbody tr").length > 0) {
                app.confirm("Items will be removed", function () {
                    $.each(checkboxes, function () {
                        row = $(this).closest("tr");
                        ID.push($(this).val());
                        TransNos += $(row).find(".SONo").text().trim() + ",";
                        SalesTypeID = $(row).find(".SalesTypeID").val();
                        FregihtAmt += clean($(row).find(".FreightAmt").val());
                    });
                    self.clear_items();
                    $("#TotalFreightAmt").val(FregihtAmt)
                    self.calculate_freight_tax();
                    self.get_sales_order_items(ID);
                    TransNos = TransNos.slice(0, -1);
                    $("#SalesOrderNos").val(TransNos);
                    $("#ItemCategoryID").val(SalesTypeID);
                    $("#BillingAddressID").val(BillingAddressID);
                    $("#ShippingAddressID").val(ShippingAddressID);
                    self.Invoice.SalesOrderNos = TransNos;
                }, function () {

                });
            } else {
                $.each(checkboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                    TransNos += $(row).find(".SONo").text().trim() + ",";
                    SalesTypeID = $(row).find(".SalesTypeID").val();
                    FregihtAmt += clean($(row).find(".FreightAmt").val());
                });
                $("#TotalFreightAmt").val(FregihtAmt)
                self.calculate_freight_tax();
                self.get_sales_order_items(ID);
                TransNos = TransNos.slice(0, -1);
                $("#SalesOrderNos").val(TransNos);
                $("#ItemCategoryID").val(SalesTypeID);
                $("#BillingAddressID").val(BillingAddressID);
                $("#ShippingAddressID").val(ShippingAddressID);
                self.Invoice.SalesOrderNos = TransNos;
            }
        } else {
            app.show_error("No orders are selected");
        }
    },
    calculate_freight_tax: function () {
        var TotalFreightAmt = clean($("#TotalFreightAmt").val());
        var FreightTax = clean($("#FreightTax").val());
        var FreightTaxAmt = TotalFreightAmt - (100 * TotalFreightAmt / (100 + FreightTax));
        //var FreightTaxAmt = TotalFreightAmt * FreightTax / 100;
        var FreightAmt = TotalFreightAmt - FreightTaxAmt;
        $("#FreightAmount").val(FreightAmt);
        $("#FreightTaxAmt").val(FreightTaxAmt);
    },


    get_sales_order_items: function (ID) {
        var self = SalesInvoice;
        $.ajax({
            url: '/Sales/SalesOrder/GetSalesOrderItems',
            dataType: "json",
            data: {
                SalesOrderID: ID,
                StoreID: $("#StoreID").val(),
                CustomerID: $("#CustomerID").val(),
                SchemeID: $("#SchemeID").val()
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.discountAndOffer).each(function (i, record) {
                        if (self.search_in_array(self.item_offer_details, record.ItemID) == -1) {
                            var obj = { ItemID: record.ItemID, OfferDetails: record.OfferDetails }
                            self.item_offer_details.push(obj);
                        }
                    });
                    //console.log(JSON.stringify(response.Items));
                    self.process_invoice(response.Items);
                }
            }
        });
    },

    select_proforma_invoice: function () {
        var self = SalesInvoice;
        var row;
        var FregihtAmt = 0;
        var ID = [];
        var TransNos = "";
        var SalesTypeID = 0;
        var checkboxes = $('#proforma-invoice-list tbody input[type="checkbox"]:checked');
        var NoOfBoxes = 0;
        var NoOfCans = 0;
        var NoOfBags = 0;



        var ShippingAddressID = clean($('#proforma-invoice-list tbody input[type="checkbox"]:checked').eq(0).closest('tr').find('.ShippingAddressID').val());
        var BillingAddressID = clean($('#proforma-invoice-list tbody input[type="checkbox"]:checked').eq(0).closest('tr').find('.BillingAddressID').val());

        var ShippingCount = 0, BillingCount = 0;
        if (checkboxes.length > 0) {
            $("#proforma-invoice-list tbody input[type='checkbox']:checked ").each(function () {
                var row = $(this).closest('tr');
                if ($(row).find('.BillingAddressID').val() != BillingAddressID) {
                    BillingCount = 1;
                }
                if ($(row).find('.ShippingAddressID').val() != ShippingAddressID) {
                    ShippingCount = 1;
                }
            });

            if (BillingCount > 0) {
                app.show_error("Can not bill to different addresses");
                return;
            }
            else if (ShippingCount > 0) {
                app.show_error("Can not ship to different addresses");
                return;
            }
            else if ($("#sales-invoice-items-list tbody tr").length > 0) {

                app.confirm("Items will be removed", function () {
                    $.each(checkboxes, function () {
                        row = $(this).closest("tr");
                        ID.push($(this).val());
                        TransNos += $(row).find(".TransNo").text().trim() + ",";
                        NoOfBoxes += clean($(row).find(".NoOfBoxes").val());
                        NoOfCans += clean($(row).find(".NoOfCans").val());
                        NoOfBags += clean($(row).find(".NoOfBags").val());
                        SalesTypeID = $(row).find(".SalesTypeID").val();
                        FregihtAmt += clean($(row).find(".FreightAmt").val());

                    });
                    self.clear_items();
                    $("#FreightAmount").val(FregihtAmt)
                    var FreightTax = clean($("#FreightTax").val());
                    var TotalFreight = (FregihtAmt * 100) / (100 - FreightTax)

                    $("#TotalFreightAmt").val(TotalFreight);
                    self.set_amount_details();
                    self.get_proforma_invoice_items(ID);
                    TransNos = TransNos.slice(0, -1);
                    $("#SalesOrderNos").val(TransNos);
                    $("#ItemCategoryID").val(SalesTypeID);
                    self.Invoice.SalesOrderNos = TransNos;
                    $("#NoOfBoxes").val(NoOfBoxes);
                    $("#NoOfCans").val(NoOfCans);
                    $("#NoOfBags").val(NoOfBags);
                    $("#BillingAddressID").val(BillingAddressID);
                    $("#ShippingAddressID").val(ShippingAddressID);
                }, function () {

                });
            } else {
                $.each(checkboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                    TransNos += $(row).find(".TransNo").text().trim() + ",";
                    NoOfBoxes += clean($(row).find(".NoOfBoxes").val());
                    NoOfCans += clean($(row).find(".NoOfCans").val());
                    NoOfBags += clean($(row).find(".NoOfBags").val());
                    SalesTypeID = $(row).find(".SalesTypeID").val();
                    FregihtAmt += clean($(row).find(".FreightAmt").val());

                });
                $("#FreightAmount").val(FregihtAmt)
                var FreightTax = clean($("#FreightTax").val());
                var TotalFreight = (FregihtAmt * 100) / (100 - FreightTax)

                $("#TotalFreightAmt").val(TotalFreight);
                self.get_proforma_invoice_items(ID);
                TransNos = TransNos.slice(0, -1);
                $("#SalesOrderNos").val(TransNos);
                $("#ItemCategoryID").val(SalesTypeID);
                self.Invoice.SalesOrderNos = TransNos;
                $("#NoOfBoxes").val(NoOfBoxes);
                $("#NoOfCans").val(NoOfCans);
                $("#NoOfBags").val(NoOfBags);
                $("#BillingAddressID").val(BillingAddressID);
                $("#ShippingAddressID").val(ShippingAddressID);
            }
        } else {
            app.show_error("No orders are selected");
        }
    },

    get_proforma_invoice_items: function (ID) {
        var self = SalesInvoice;
        $.ajax({
            url: '/Sales/ProformaInvoice/GetProformaInvoiceItems',
            dataType: "json",
            data: {
                ProformaInvoiceID: ID,
                CustomerID: $("#CustomerID").val(),
                SchemeID: $("#SchemeID").val(),

            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.discountAndOffer).each(function (i, record) {
                        if (self.search_in_array(self.item_offer_details, record.ItemID) == -1) {
                            var obj = { ItemID: record.ItemID, OfferDetails: record.OfferDetails }
                            self.item_offer_details.push(obj);
                        }
                    });
                    self.process_invoice(response.Items);
                }
            }
        });
    },


    process_invoice: function (items) {
        //alert(1);
        var self = SalesInvoice;
        self.Items = self.process_items(items);
        self.set_item_calculations();
        self.calculate_grid_total();
        self.set_amount_details();
        self.add_items_to_grid();
        self.set_form_values();
        self.set_packing_details();
    },

    set_packing_details: function () {
        var self = SalesInvoice;

        self.Invoice.PackingDetails = [];
        $(self.Items).each(function (i, record) {
            self.calculate_group_packing_details(record.PackSize + ' ' + record.PrimaryUnit, record.InvoiceQty, record.UnitName, record.UnitID, record.PackSize);
        });

        self.Invoice.PackingDetails = self.Invoice.PackingDetails.sort(function (a, b) {
            var first = a.Pack;
            var second = b.Pack;
            return second - first
        });

        self.Invoice.PackingDetails = self.Invoice.PackingDetails.sort((a, b) => b - a);

        var tr = "";
        var $tr;
        $(self.Invoice.PackingDetails).each(function (i, record) {
            tr += '<tr  class="uk-text-center">';
            tr += '<td>' + (i + 1);
            tr += '</td>';
            tr += '<td class="PackSize">' + record.PackSize;
            tr += '</td>';
            tr += '<td>' + record.Unit;
            tr += '<input type="hidden" class="UnitID included" value="' + record.UnitID + '"  />'
            tr += '</td>';
            tr += '<td class="uk-text-right Quantity">' + record.Quantity;
            tr += '</td>';
            tr += '</tr>';
        });
        $tr = $(tr);
        app.format($tr);
        $("#packing-detail-list tbody").html($tr);
    },

    calculate_group_packing_details: function (PackSize, Quantity, UnitName, UnitID, Pack) {
        var self = SalesInvoice;
        if (Quantity == 0) {
            return;
        }
        var index = self.search_pack_size(self.Invoice.PackingDetails, PackSize, UnitName, UnitID);
        if (index == -1) {

            self.Invoice.PackingDetails.push({
                PackSize: PackSize,
                Quantity: Quantity,
                Unit: UnitName,
                UnitID: UnitID,
                Pack: Pack
            });

        } else {
            self.Invoice.PackingDetails[index].Quantity += Quantity;
        }
    },

    search_pack_size: function (array, Packsize, UnitName, UnitID) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].PackSize == Packsize && array[i].UnitID == UnitID) {
                return i;
            }
        }
        return -1;
    },

    recalculate: function (event) {
        var self = SalesInvoice;
        self.set_item_calculations();
        self.calculate_grid_total();
        self.set_amount_details();
        self.add_items_to_grid();
        self.set_form_values();
        self.set_packing_details();
    },

    process_items: function (items) {
        var self = SalesInvoice;
        var processed_items = [];
        $.each(items, function (i, item) {
            item = self.get_item_properties(item);
            item.index = i + 1;
            processed_items.push(item);
        });

        return processed_items;
    },

    // calculates and returns item properties
    get_item_properties: function (item) {
        var self = SalesInvoice;
        var IsVATExtra = $("#IsVATExtra").val();
        if (item.StoreID == 0) {
            item.StoreID = $("#StoreID").val();
        }
        item.OfferQtyMet = item.InvoiceOfferQtyMet ? "" : "offer-qty-not-met";
        item.QtyMet = item.InvoiceQtyMet ? "" : "qty-not-met";
        if (item.SalesUnitID == item.UnitID) {
            item.MRP = item.Rate;
        }
        else {
            item.MRP = item.LooseRate;
        }
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        }
        else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        if (CustomerCategory == "Export") {
            item.BasicPrice = item.MRP;
        }
        ////added by anju
        if (IsVATExtra == 1) {
            item.BasicPrice = item.MRP;
        }
        item.BasicPrice = item.BasicPrice;
        item.GrossAmount = (item.BasicPrice * (item.InvoiceQty));
        item.AdditionalDiscount = (item.BasicPrice * item.InvoiceOfferQty);
        item.DiscountAmount = ((item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100);
        item.TurnoverDiscount = 0;
        item.TaxableAmount = 0;
        item.GSTAmount = 0;
        item.CashDiscount = 0;
        item.NetAmount = 0;
        item.IsIncluded = true;
        return item;
    },

    set_item_calculations: function () {
        var self = SalesInvoice;
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var AdditionalDiscount = 0;
        var TurnoverDiscountAvailable = clean($("#TurnoverDiscountAvailable").val());
        var CashDiscountPercentage = 0;
        if ($("#CashDiscountEnabled").is(":checked")) {
            CashDiscountPercentage = clean($("#CashDiscountPercentage").val());
        }

        var IsIGST = self.is_igst();

        $(self.Items).each(function (i, item) {
            GrossAmount += item.GrossAmount;
            DiscountAmount += item.DiscountAmount;
            AdditionalDiscount += item.AdditionalDiscount;
        });

        var NetValue = GrossAmount - DiscountAmount - AdditionalDiscount;

        if (NetValue > TurnoverDiscountAvailable) {
            TurnoverDiscount = TurnoverDiscountAvailable;
        } else {
            TurnoverDiscount = Math.round(NetValue) > 0 ? Math.round(NetValue) - 1 : 0;
        }

        $("#TurnoverDiscount").val(TurnoverDiscount);

        $(self.Items).each(function (i, item) {
            if (item.IsIncluded) {
                item.TurnoverDiscount = (((item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount) / (GrossAmount - DiscountAmount - AdditionalDiscount)) * TurnoverDiscount);
                item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount - item.TurnoverDiscount;
                item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100);

                if (Sales.is_cess_effect()) {
                    item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
                } else {
                    item.CessAmount = 0;
                }
                if (IsIGST) {
                    item.IGST = item.GSTAmount;
                    item.SGST = 0;
                    item.CGST = 0;
                } else {
                    item.IGST = 0;
                    item.SGST = (item.GSTAmount / 2);
                    item.CGST = (item.GSTAmount / 2);
                }
                if (CustomerCategory == "Export") {
                    item.CessAmount = 0;
                    item.CessPercentage = 0;
                    item.IGST = 0;
                    item.CGST = 0;
                    item.SGST = 0;
                    item.GSTAmount = 0;
                    item.IGSTPercentage = 0;
                }

                item.NetAmount = item.TaxableAmount + item.IGST + item.SGST + item.CGST + item.CessAmount;
                item.CashDiscount = (item.NetAmount * CashDiscountPercentage / 100);
                item.NetAmount = item.NetAmount - item.CashDiscount;
            }
        });
    },

    calculate_grid_total: function () {
        var self = SalesInvoice;
        var FreightAmt = clean($("#TotalFreightAmt").val());
        var FreightTaxAmt = clean($("#FreightTaxAmt").val());
        self.Invoice.GSTAmount = 0;
        self.Invoice.TurnoverDiscount = clean($("#TurnoverDiscount").val());
        self.Invoice.GrossAmount = 0;
        self.Invoice.DiscountAmount = 0;
        self.Invoice.DiscountPercentage = 0;
        self.Invoice.AdditionalDiscount = 0;
        self.Invoice.TaxableAmount = 0;
        self.Invoice.NetAmount = 0;
        self.Invoice.RoundOff = 0;
        self.Invoice.CashDiscount = 0;
        self.Invoice.CessAmount = 0;
        var temp = 0;
        $(self.Items).each(function (i, item) {
            if (item.IsIncluded) {
                self.Invoice.GrossAmount += item.GrossAmount;
                self.Invoice.DiscountAmount += item.DiscountAmount;
                self.Invoice.AdditionalDiscount += item.AdditionalDiscount;
                self.Invoice.TaxableAmount += item.TaxableAmount;
                self.Invoice.GSTAmount += item.GSTAmount;
                self.Invoice.NetAmount += item.NetAmount;
                self.Invoice.CashDiscount += item.CashDiscount;
                self.Invoice.CessAmount += item.CessAmount;
            }
        });
        self.Invoice.NetAmount += FreightAmt;
        temp = self.Invoice.NetAmount;
        self.Invoice.NetAmount = parseFloat(self.Invoice.NetAmount);
        self.Invoice.RoundOff = temp - self.Invoice.NetAmount;
        self.Invoice.CheckStock = $("#CheckStock").val();

        if (self.is_igst()) {
            self.Invoice.CGSTAmount = 0;
            self.Invoice.SGSTAmount = 0;
            self.Invoice.IGSTAmount = self.Invoice.GSTAmount + FreightTaxAmt;
        } else {
            self.Invoice.CGSTAmount = ((self.Invoice.GSTAmount / 2) + (FreightTaxAmt / 2));
            self.Invoice.SGSTAmount = ((self.Invoice.GSTAmount / 2) + (FreightTaxAmt / 2));
            self.Invoice.IGSTAmount = 0;
        }
    },

    add_items_to_grid: function () {
        var self = SalesInvoice;
        var index = 0;
        var tr = '';
        var title;
        var row_class = "";
        $(self.Items).each(function (i, item) {
            index++;
            title = (typeof item.SalesOrderNo == "undefined" ? "" : 'Order No : ' + item.SalesOrderNo + '<br/>')
                + 'Order Quantity : ' + item.Qty + '<br/>'
                + 'Offer Quantity : ' + item.OfferQty + '<br/>'
                + 'Stock : ' + item.Stock;
            row_class = item.SalesOrderItemID == 0 ? "proforma-invoice" : "sales-order"
            tr += '<tr class="included ' + row_class + ' ' + item.OfferQtyMet + ' ' + item.QtyMet + ' quantity-' + item.InvoiceQty + ' " data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
                + '<td class="uk-text-center">' + index
                + '<input type="hidden" class="ItemID included" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="BatchID included" value="' + item.BatchID + '"  />'
                + '<input type="hidden" class="IGSTPercentage included" value="' + item.IGSTPercentage + '" />'
                + '<input type="hidden" class="SGSTPercentage included" value="' + item.SGSTPercentage + '" />'
                + '<input type="hidden" class="CGSTPercentage included" value="' + item.CGSTPercentage + '" />'
                + '<input type="hidden" class="ProformaInvoiceTransID included" value="' + item.ProformaInvoiceTransID + '" />'
                + '<input type="hidden" class="SalesOrderItemID included" value="' + item.SalesOrderItemID + '" />'
                + '<input type="hidden" class="StoreID included" value="' + item.StoreID + '" />'
                + '<input type="hidden" class="OrderQty included" value="' + item.Qty + '" />'
                + '<input type="hidden" class="OfferQty included" value="' + item.OfferQty + '" />'
                + '<input type="hidden" class="Stock included" value="' + item.Stock + '" />'
                + '<input type="hidden" class="PackSize" value="' + item.PackSize + '" />'
                + '<input type="hidden" class="PrimaryUnit" value="' + item.PrimaryUnit + '" />'
                + '<input type="hidden" class="CategoryID" value="' + item.CategoryID + '" />'
                + '</td>'
                + '<td class="uk-text-small">' + item.ItemName + '</td>'
                + '<td>' + item.UnitName + '</td>'
                + '<td>' + item.BatchTypeName + '</td>'
                + '<td>' + item.BatchName + '</td>'
                + '<td><input type="text" class="Qty included md-input mask-qty" value="' + item.InvoiceQty + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="InvoiceOfferQty included md-input mask-numeric" value="' + item.InvoiceOfferQty + '" readonly="readonly" /></td>'
                + '<td class="mrp_hidden" ><input type="text" class="MRP included md-input mask-sales-currency" value="' + item.MRP + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="BasicPrice included md-input mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GrossAmount included md-input mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="AdditionalDiscount included md-input mask-sales-currency" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="DiscountPercentage included md-input mask-sales-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="DiscountAmount included md-input mask-sales-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="TurnoverDiscount included md-input mask-sales-currency" value="' + item.TurnoverDiscount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="TaxableAmount included md-input mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GST included md-input mask-currency" value="' + item.IGSTPercentage + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GSTAmount i  ncluded md-input mask-sales-currency" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="CashDiscount included md-input mask-sales-currency" value="' + item.CashDiscount + '" readonly="readonly" /></td>'
                + '<td class="cess-enabled"><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                + '<td class="cess-enabled"><input type="text" class="CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="NetAmount included md-input mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        });

        var $tr = $(tr);
        app.format($tr);
        $("#sales-invoice-items-list tbody").html($tr);
        $("#item-count").val(index);
        setTimeout(function () {
            fh_items.resizeHeader();
        }, 200);
    },

    set_form_values: function () {
        var self = SalesInvoice;
        $("#GrossAmount").val(self.Invoice.GrossAmount);
        $("#DiscountAmount").val(self.Invoice.DiscountAmount);
        $("#AdditionalDiscount").val(self.Invoice.AdditionalDiscount);
        $("#TaxableAmount").val(self.Invoice.TaxableAmount);
        $("#RoundOff").val(self.Invoice.RoundOff);
        $("#NetAmount").val(self.Invoice.NetAmount);
        $("#SGSTAmount").val(self.Invoice.SGSTAmount);
        $("#CGSTAmount").val(self.Invoice.CGSTAmount);
        $("#IGSTAmount").val(self.Invoice.IGSTAmount);
        $("#CashDiscount").val(self.Invoice.CashDiscount);
        $("#CessAmount").val(self.Invoice.CessAmount);
    },

    //gets offer quantity from the SalesOrder.item_offer_details
    get_offer_qty: function (qty, item_id) {
        var self = SalesInvoice;
        var index = -1;
        var offer_qty = 0;
        var j = self.search_in_array(self.item_offer_details, item_id);
        if (j != -1) {
            self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
                if (record.Qty <= qty) {
                    index = i
                }
            });
            if (index != -1) {
                offer_qty = parseInt(qty / self.item_offer_details[j].OfferDetails[index].Qty) * self.item_offer_details[j].OfferDetails[index].OfferQty;
            }
        }

        return offer_qty;
    },

    sales_order_list: function () {

        var $list = $('#sales-order-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesOrder/GetUnProcessedSalesOrderList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "CustomerID", Value: $('#CustomerID').val() },
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
                            return "<input type='checkbox' class='uk-radio SalesOrderID' name='SalesOrderID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='FreightAmt' value='" + row.FreightAmount + "' >"
                                + "<input type='hidden' class='BillingAddressID' value='" + row.BillingAddressID + "' >"
                                + "<input type='hidden' class='ShippingAddressID' value='" + row.ShippingAddressID + "' >"
                                + "<input type='hidden' class='PrintWithItemCode' value='" + row.PrintWithItemCode + "' >"
                                + "<input type='hidden' class='SalesTypeID' value='" + row.SalesTypeID + "' >";
                        }
                    },
                    { "data": "SONo", "className": "SONo" },
                    { "data": "SODate", "className": "SODate" },
                    { "data": "SalesType", "className": "SalesType" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    {
                        "data": "NetAmount", "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#CustomerID', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    proforma_invoice_list: function () {

        var $list = $('#proforma-invoice-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/ProformaInvoice/GetUnProcessedProformaInvoiceList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "CustomerID", Value: $('#CustomerID').val() },
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
                            return "<input type='checkbox' class='uk-radio ProformaInvoiceID' name='ProformaInvoiceID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='SalesTypeID' value='" + row.SalesTypeID + "' >"
                                + "<input type='hidden' class='NoOfBoxes' value='" + row.NoOfBoxes + "' >"
                                + "<input type='hidden' class='NoOfCans' value='" + row.NoOfCans + "' >"
                                + "<input type='hidden' class='NoOfBags' value='" + row.NoOfBags + "' >"
                                + "<input type='hidden' class='BillingAddressID' value='" + row.BillingAddressID + "' >"
                                + "<input type='hidden' class='ShippingAddressID' value='" + row.ShippingAddressID + "' >"
                                + "<input type='hidden' class='FreightAmt' value='" + row.FreightAmount + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "InvoiceDate", "className": "InvoiceDate" },
                    { "data": "SalesType", "className": "SalesType" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "Location", "className": "Location" },
                    {
                        "data": "NetAmount", "className": "NetAmount", "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#CustomerID', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    replace_batches: function () {
        var self = SalesInvoice;

        var InvoiceQty;
        var InvoiceOfferQty;
        var item;

        if (self.validate_stock() > 0) {
            return;
        }
        UIkit.modal("#batch-edit").hide();
        var ItemID = $('#BatchItemID').val();

        var SalesOrderTransIDs = $("#SalesOrderTransIDs").val();
        var ArraySalesOrderTransIDs = SalesOrderTransIDs.split(",");
        var OrderTransID = 0;
        if (ArraySalesOrderTransIDs.length >= 1) {
            OrderTransID = parseInt(ArraySalesOrderTransIDs[0]);
        }

        var OrderQty = clean($("#BatchQty").val());

        //stores the item properties other than batch and qty
        var tempitem = self.search_in_items(parseInt(ItemID), 0);

        //removes items of edited batches from the list 
        self.Items = $.grep(self.Items, function (e) { return e.ItemID != ItemID });

        $('#batch-list tbody tr').each(function (i) {
            InvoiceQty = clean($(this).find('.InvoiceQty').val());
            InvoiceOfferQty = clean($(this).find('.InvoiceOfferQty').val());
            if (InvoiceQty != 0 || InvoiceOfferQty != 0) {

                // assigns the batch and qty to the set temp item
                item = Object.assign({}, tempitem,
                    {
                        InvoiceQty: InvoiceQty + InvoiceOfferQty,
                        InvoiceOfferQty: InvoiceOfferQty,
                        BatchID: $(this).find('.BatchID').val(),
                        BatchName: $(this).find('.BatchName').text(),
                        SalesOrderItemID: clean($(this).find('.OrderTransID').val()) == 0 ? OrderTransID : clean($(this).find('.OrderTransID').val()),
                        Qty: clean($(this).find('.OrderQty').val()) == 0 ? OrderQty : clean($(this).find('.OrderQty').val()),
                        InvoiceOfferQtyMet: true,
                        InvoiceQtyMet: true,
                        OfferQtyMet: "",
                        QtyMet: "",
                        Rate: clean($(this).find('.Rate').val()),
                    });

                self.Items.push(item);

            }
        });

        self.Items.sort(function (a, b) {
            return ((a.SalesOrderItemID < b.SalesOrderItemID) ? -1 : ((a.SalesOrderItemID > b.SalesOrderItemID) ? 1 : 0));
        });

        self.process_invoice(self.Items);
    },
    showitemhistory: function () {
        var ItemID = $(this).closest('tr').find('.ItemID').val();
        $("#HistoryItemID").val(ItemID);
        $('#show-history').trigger('click');
    },

    show_batch_edit_modal: function () {
        var self = SalesInvoice;
        var tr = "";
        var $tr;
        var OrderQty = 0;
        var InvoiceQty;
        var InvoiceOfferQty;
        var TotalInvoiceQty = 0;
        var TotalInvoiceOfferQty = 0;
        var TotalStock = 0;

        var SalesOrderTransID = -1;
        var SalesOrderTransIDs = [];

        var ItemID = $(this).closest('tr').find('.ItemID').val();
        var UnitID = $(this).closest('tr').find('.UnitID').val();
        var CategoryID = $(this).closest('tr').find('.CategoryID').val();
        var item = self.search_in_items(parseInt(ItemID), 0);

        $("#sales-invoice-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr").each(function () {
            row = $(this).closest('tr');
            if (SalesOrderTransID != $(row).find(".SalesOrderItemID").val()) {
                OrderQty += clean($(row).find(".OrderQty").val());
                SalesOrderTransID = $(row).find(".SalesOrderItemID").val();
                if (SalesOrderTransID == undefined || SalesOrderTransID == "undefined") {
                    SalesOrderTransIDs.push(0);
                } else {
                    SalesOrderTransIDs.push($(row).find(".SalesOrderItemID").val());
                }
            }
        });

        $("#BatchItemID").val(item.ItemID);
        $("#BatchItemName").val(item.ItemName);
        $("#BatchItemCode").val(item.Code);
        $("#BatchQty").val(OrderQty);
        $("#BatchOfferQty").val(item.OfferQty);
        $("#BatchMaxQty").val(OrderQty);
        $("#SalesOrderTransIDs").val(SalesOrderTransIDs.toString());

        self.set_offer_qty();

        $.ajax({
            url: '/Masters/Batch/GetAvailableBatches',
            dataType: "json",
            data: {
                ItemID: ItemID,
                UnitID: UnitID,
                OrderQty: OrderQty,
                SalesOrderTransIDs: SalesOrderTransIDs.length == 0 ? [] : SalesOrderTransIDs,
                WarehouseID: $('#StoreID').val(),
                CustomerID: $("#CustomerID").val(),
                SchemeID: $('#SchemeID').val(),
                ProformaInvoiceID: 0
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var BatchID = 0;
                    $(response.Batches).each(function (i, record) {

                        TotalInvoiceQty += record.InvoiceQty - record.InvoiceOfferQty;
                        TotalInvoiceOfferQty += record.InvoiceOfferQty;

                        if (BatchID != record.BatchID) {
                            TotalStock += record.Stock;
                            BatchID = record.BatchID;
                        }

                        tr += '<tr>'
                            + '<td>' + (i + 1)
                            + '<input type="hidden" class="BatchID" value="' + record.BatchID + '" />'
                            + '<input type="hidden" class="OrderTransID" value="' + record.SalesOrderTransID + '" />'
                            + '<input type="hidden" class="OrderQty" value="' + record.Qty + '" />'
                            + '<input type="hidden" class="Rate" value="' + record.Rate + '" />'
                            + '<input type="hidden" class="CategoryID" value="' + CategoryID + '" />'
                            + '</td>'
                            + '<td class="BatchName">' + record.BatchNo + '</td>'
                            + '<td class="Stock mask-qty">' + record.Stock + '</td>'
                            + '<td class="mask-currency">' + record.Rate + '</td>'
                            + '<td>' + record.ExpiryDateStr + '</td>'
                            + '<td><input type="text" class="md-input mask-qty InvoiceQty" value = "' + (record.InvoiceQty - record.InvoiceOfferQty) + '" /></td>'
                            + '<td><input type="text" class="md-input mask-numeric InvoiceOfferQty" value ="' + record.InvoiceOfferQty + '" /></td>'
                            + '<tr>';
                    });
                    tr += '<tr>'
                        + '<td colspan="2"><b>Total</b></td>'
                        + '<td class="mask-qty BatchStock">' + TotalStock + '</td>'
                        + '<td colspan="2"></td>'
                        + '<td><input type="text" class="md-input mask-qty TotalInvoiceQty" disabled value = "' + TotalInvoiceQty + '" /></td>'
                        + '<td><input type="text" class="md-input mask-numeric TotalInvoiceOfferQty" disabled value ="' + TotalInvoiceOfferQty + '" /></td>'
                        + '<tr>';
                    $tr = $(tr);
                    app.format($tr);
                    $('#batch-list tbody').html($tr);
                    $('#show-batch-edit').trigger('click');
                }
            }
        });

    },

    set_total_invoice_qty: function () {
        var TotalInvoiceQty = 0;
        $("#batch-list .InvoiceQty").each(function () {
            TotalInvoiceQty += clean($(this).val());
        });
        $("#batch-list .TotalInvoiceQty").val(TotalInvoiceQty);
    },

    set_total_invoice_offer_qty: function () {
        var TotalInvoiceOfferQty = 0;
        $("#batch-list .InvoiceOfferQty").each(function () {
            TotalInvoiceOfferQty += clean($(this).val());
        });
        $("#batch-list .TotalInvoiceOfferQty").val(TotalInvoiceOfferQty);
    },

    search_in_items: function (ItemID, BatchID) {
        if (BatchID == 0) {
            return SalesInvoice.Items.find(item => { return item.ItemID == ItemID });
        } else {
            return SalesInvoice.Items.find(item => { return item.ItemID == ItemID && item.BatchID == BatchID });
        }
    },

    filter_items: function (ItemID) {
        return SalesInvoice.Items.filter(item => { return item.ItemID == ItemID });
    },

    is_igst: function () {
        if ($("#StateID").val() != $("#LocationStateID").val()) {
            return true;
        }
        return false;
    },

    set_amount_details: function () {
        var self = SalesInvoice;
        var is_igst = self.is_igst();
        var GSTPercent;
        var GSTAmount;
        var TotalFreightAmt = clean($("#FreightAmount").val());
        var FreightTax = clean($("#FreightTax").val());
        // var FreightTaxAmt = TotalFreightAmt * FreightTax / 100;
        //var FreightTaxAmt = clean($("#FreightAmount").val());
        var FreightTaxAmt = clean($("#FreightTaxAmt").val());
        var CustomerType = $("#CustomerCategoryID option:selected").text();
        var tr = "";
        var $tr;
        self.Invoice.AmountDetails = [];

        $(self.Items).each(function (i, item) {

            if (is_igst) {
                self.calculate_group_tax_details(item.IGSTPercentage, item.GSTAmount, "IGST", item.TaxableAmount);
            } else {
                self.calculate_group_tax_details(item.IGSTPercentage / 2, item.GSTAmount / 2, "SGST", item.TaxableAmount);
                self.calculate_group_tax_details(item.IGSTPercentage / 2, item.GSTAmount / 2, "CGST", item.TaxableAmount);
                self.calculate_group_tax_details(item.CessPercentage, item.CessAmount, "Cess", item.TaxableAmount);
            }
        });

        if (CustomerType == "ECOMMERCE") {
            if (is_igst) {
                var index = self.search_tax_group(self.Invoice.AmountDetails, FreightTax, "IGST On FreightAmount");
                if (index == -1) {

                    self.Invoice.AmountDetails.push({
                        Percentage: FreightTax,
                        Amount: FreightTaxAmt,
                        TaxableAmount: TotalFreightAmt,
                        Particulars: "IGST On FreightAmount"
                    });
                }
            }
            else {
                var index = self.search_tax_group(self.Invoice.AmountDetails, (FreightTax / 2), "SGST On FreightAmount");
                if (index == -1) {
                    self.Invoice.AmountDetails.push({
                        Percentage: FreightTax / 2,
                        Amount: FreightTaxAmt / 2,
                        TaxableAmount: TotalFreightAmt,
                        Particulars: "SGST On FreightAmount"
                    });
                }
                var index = self.search_tax_group(self.Invoice.AmountDetails, (FreightTax / 2), "CGST On FreightAmount");
                if (index == -1) {

                    self.Invoice.AmountDetails.push({
                        Percentage: FreightTax / 2,
                        Amount: FreightTaxAmt / 2,
                        TaxableAmount: TotalFreightAmt,
                        Particulars: "CGST On FreightAmount"
                    });
                }
            }
        }

        $.each(self.Invoice.AmountDetails, function (i, record) {
            tr += "<tr  class='uk-text-center'>";
            tr += "<td>" + (i + 1);
            tr += "</td>";
            tr += "<td >" + record.Particulars;
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency' readonly value='" + record.Percentage + "' />";
            tr += "</td>";
            tr += "<td class='mask-currency'>" + record.TaxableAmount;
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency' readonly value='" + record.Amount + "' />";
            tr += "</td>";
            tr += "</tr>";
        });
        $tr = $(tr);
        app.format($tr);
        $("#sales-invoice-amount-details-list tbody").html($tr);
    },

    calculate_group_tax_details: function (GSTPercent, GSTAmount, type, TaxableAmount) {
        var self = SalesInvoice;
        var index = self.search_tax_group(self.Invoice.AmountDetails, GSTPercent, type);
        if (index == -1) {
            self.Invoice.AmountDetails.push({
                Percentage: GSTPercent,
                Amount: GSTAmount,
                Particulars: type,
                TaxableAmount: TaxableAmount
            });
        } else {
            self.Invoice.AmountDetails[index].Amount += GSTAmount;
            self.Invoice.AmountDetails[index].TaxableAmount += TaxableAmount;
        }
    },

    save: function () {
        var self = SalesInvoice;
        var url;
        self.Invoice.PaymentModeID = $("#PaymentModeID").val();
        self.Invoice.IsGST = $("#IsGST").val();
        self.Invoice.IsVat = $("#IsVat").val();
        self.Invoice.CurrencyID = $("#CurrencyID").val();
        self.Invoice.CurrencyExchangeRate = $("#CurrencyExchangeRate").val();
        self.Invoice.Items = self.Items;
        self.Invoice.IsDraft = IsDraft;
        self.Invoice.NoOfBags = $("#NoOfBags").val();
        self.Invoice.NoOfCans = $("#NoOfCans").val();
        self.Invoice.NoOfBoxes = $("#NoOfBoxes").val();
        self.Invoice.FreightAmount = clean($("#FreightAmount").val());
        self.Invoice.SchemeID = $("#SchemeID").val();
        self.Invoice.BillingAddressID = $("#BillingAddressID").val();
        self.Invoice.ShippingAddressID = $("#ShippingAddressID").val();
        self.Invoice.Remarks = $("#Remarks").val();
        self.Invoice.PrintWithItemCode = $("#PrintWithItemCode").is(":checked");
        self.Invoice.OtherCharges = clean($("#OtherCharges").val());
        self.Invoice.CustomerPONo = $("#CustomerPONo").val();
        self.Invoice.PackingDetails = self.Invoice.PackingDetails;
        self.Invoice.CustomerPODate = $("#CustomerPODate").val();
        if (self.Invoice.IsDraft == true) {
            url = '/Sales/SalesInvoice/SaveAsDraft'
        }
        else {
            url = '/Sales/SalesInvoice/Save';
        }
        $(".btnSaveASDraft, .btnSave, .btnSaveAndPrint, .cancel").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: self.Invoice,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var msg;
                    if (self.Invoice.ID == "") {
                        msg = "Sales Invoice Saved Successfully";
                    } else {
                        msg = "Sales Invoice Updated Successfully";
                    }
                    if (self.Invoice.IsDraft) {
                        msg = "Sales Invoice Saved as draft Successfully";
                    }
                    app.show_notice("Sales Invoice Saved Successfully");
                    $("#ID").val(response.ID);
                    if (IsPrint == true) {
                        self.print();
                    }
                    setTimeout(function () {
                        window.location = "/Sales/SalesInvoice/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSave,.btnSaveAndPrint, .cancel").css({ 'display': 'block' });
                }
            }
        });
    },

    validate_form: function () {
        var self = SalesInvoice;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_form_for_draft: function () {
        var self = SalesInvoice;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_stock: function () {
        var self = SalesInvoice;
        if (self.rules.stock.length > 0) {
            return form.validate(self.rules.stock);
        }
        return 0;
    },

    rules: {
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Invalid Invoice Number" },
                ]
            },
            {
                elements: "#InvoiceDate",
                rules: [
                    { type: form.required, message: "Invalid Invoice Date" },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    {
                        type: function (element) {
                            return ($(element).closest("tr").find(".CategoryID").val() == "222") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                        }, message: "Please enter a valid quantity for finished goods"
                    },
                    {
                        type: function (element) {
                            return ($(".offer-qty-not-met").length == 0);
                        },
                        message: "Invalid Qty"
                    }
                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            }
        ],

        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Invalid Invoice Number" },
                ]
            },

            {
                elements: "#InvoiceDate",
                rules: [
                    { type: form.required, message: "Invalid Invoice Date" },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    {
                        type: function (element) {
                            return ($(element).closest("tr").find(".CategoryID").val() == "222") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                        }, message: "Please enter a valid quantity for finished goods"
                    }
                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            }
        ],
        stock: [
            {
                elements: "#BatchQty",
                rules: [
                    { type: form.required, message: "Order quantity required" },
                    { type: form.positive, message: "Invalid Order quantity" },
                    {
                        type: function (element) {
                            return clean($('#BatchMaxQty').val()) >= clean($(element).val());
                        }, message: "Order quantity exceeds sales order quantity"

                    },
                    {
                        type: function (element) {
                            return clean($(".BatchStock").text()) >= clean($('#BatchQty').val()) + clean($('#BatchOfferQty').val());
                        }, message: "Insufficient Stock"
                    }

                ]
            },
            {
                elements: ".InvoiceQty",
                rules: [
                    { type: form.positive, message: "Invalid quantity" },
                    {
                        type: function (element) {
                            return ($(element).closest("tr").find(".CategoryID").val() == "222") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                        }, message: "Please enter a valid quantity for finished goods"
                    }
                ]
            },
            {
                elements: ".InvoiceOfferQty",
                rules: [
                    { type: form.positive, message: "Invalid offer quantity" },
                ]
            },
            {
                elements: ".TotalInvoiceQty",
                rules: [
                    {
                        type: function (element) {
                            return clean($('#BatchQty').val()) == clean($(element).val());
                        }, message: "Invalid invoice quantity", alt_element: '#batch-list .InvoiceQty'

                    }
                ]
            },


            {
                elements: ".TotalInvoiceOfferQty",
                rules: [
                    {
                        type: function (element) {
                            return clean($('#BatchOfferQty').val()) == clean($(element).val());
                        }, message: "Invalid invoice offer quantity", alt_element: '#batch-list .InvoiceOfferQty'
                    }
                ]
            }
        ]
    },

    on_item_category_change: function () {
        var self = SalesInvoice;
    },
    // Gets the items for auto complete
    get_customers: function (release) {
        var self = SalesInvoice;
        $.ajax({
            url: '/Masters/Customer/GetSalesInvoiceCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
                CustomerCategoryID: $('#CustomerCategoryID').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },


    get_discount_details: function () {
        var self = SalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesInvoice/GetDiscountDetails',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#TurnoverDiscountAvailable").val(response.TurnoverDiscountAvailable)
                    if (response.CashDiscountEnabled) {
                        $("input#CashDiscountEnabled").iCheck('enable');
                    } else {
                        $("input#CashDiscountEnabled").iCheck('disable');
                        $("input#CashDiscountEnabled").iCheck('uncheck');
                    }
                }
            }
        });
    },
    // Selects the customer on click modal ok button
    select_customer: function () {
        var self = SalesInvoice;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var CustomerCategory = $(row).find(".CustomerCategory").text().trim();
        var StateID = $(row).find(".StateID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var MinCreditLimit = $(row).find(".MinimumCreditLimit").val();
        var MaxCreditLimit = $(row).find(".MaxCreditLimit").val();
        var CashDiscountPercentage = $(row).find(".CashDiscountPercentage").val();
        var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerCategoryID").val(CustomercategoryID);
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#CustomerID").trigger("change");
        $("#StateID").val(StateID);
        $("#SchemeID").val(SchemeID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
        $("#CustomerCategory").val(CustomerCategory);
        $("#MinCreditLimit").val(MinCreditLimit);
        $("#MaxCreditLimit").val(MaxCreditLimit);
        $("#CashDiscountPercentage").val(CashDiscountPercentage);
        UIkit.modal($('#select-customer')).hide();
        self.Invoice.CustomerID = ID;
        self.customer_on_change();
        self.on_change_customer_category();
    },

    // on select auto complete customer
    set_customer: function (event, item) {
        var self = SalesInvoice;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#CustomerID").trigger("change");
        $("#StateID").val(item.stateId);
        $("#SchemeID").val(item.schemeId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#CustomerCategory").val(item.customerCategory);
        $("#MinCreditLimit").val(item.minCreditLimit);
        $("#MaxCreditLimit").val(item.maxCreditLimit);
        $("#CashDiscountPercentage").val(item.cashDiscountPercentage);
        $("#CustomerCategoryID").val(item.customercategoryid);
        self.Invoice.CustomerID = item.id;
        self.customer_on_change();
        self.on_change_customer_category();
    },

    customer_on_change: function () {
        var self = SalesInvoice;
        $("#SalesOrderNos").focus();
        self.clear_items();
        //self.get_scheme_allocation();
        self.get_discount_details();
        Sales.get_customer_addresses();
        self.get_credit_amount();
    },
    get_credit_amount: function () {
        var self = SalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesInvoice/GetCreditAmountByCustomer',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#CreditAmount").val(response.Data);

                }
            }
        });
    },

    // clear items on changing customer
    clear_items: function () {
        var self = SalesInvoice;
        $("#sales-invoice-items-list tbody").html('');
        $("#sales-invoice-amount-details-list tbody").html('');
        $("#SalesOrderNos").val('');
        $("#GrossAmount").val('');
        $("#AdditionalDiscount").val('');
        $("#DiscountAmount").val('');
        $("#TurnoverDiscount").val('');
        $("#TaxableAmount").val('');
        $("#SGSTAmount").val('');
        $("#CGSTAmount").val('');
        $("#IGSTAmount").val('');
        $("#CashDiscount").val('');
        $("#RoundOff").val('');
        $("#NetAmount").val('');
        $("#CessAmount").val('');
        self.Items = [];
        self.Invoice.GrossAmount = 0;
        self.Invoice.AdditionalDiscount = 0;
        self.Invoice.DiscountAmount = 0;
        self.Invoice.TurnoverDiscount = 0;
        self.Invoice.TaxableAmount = 0;
        self.Invoice.CGSTAmount = 0;
        self.Invoice.SGSTAmount = 0;
        self.Invoice.IGSTAmount = 0;
        self.Invoice.CashDiscount = 0;
        self.Invoice.NetAmount = 0;
        self.Invoice.RoundOff = 0;
        self.Invoice.CessAmount = 0;

    },
    // gets the scheme for the selected customer
    get_scheme_allocation: function () {
        var self = SalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesOrder/GetSchemeAllocation',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#SchemeID").val(response.SchemeID);
                    self.Invoice.SchemeID = response.SchemeID;
                }
            }
        });
    },

    details: function () {
        var self = SalesInvoice;
        self.freeze_headers();
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on("click", ".print-sales-invoice", self.print);
        $("body").on("click", "#btnOkPrintPreference", self.print_preview);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".Exportpdf", self.Exportpdf);
        $("body").on("click", ".ItemCode", self.SalesInvoiceWithCode);
        $("body").on("click", ".PartNo", self.SalesInvoicePartNo);
        $("body").on("click", ".ExportItemCode", self.SalesInvoiceExportitemcode);
        $("body").on("click", ".ExportPartNo", self.SalesInvoiceExportpartno);




    },

    print_preview: function () {
        $.ajax({
            url: '/Reports/Sales/PrintInvoice',
            data: {
                SalesInvoiceID: $("#ID").val(), Preferences: app.print_preferences
            },

            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.print_preview(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },

    print: function () {
        var self = SalesInvoice;
        $.ajax({
            url: '/Sales/SalesInvoice/Print',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    SignalRClient.print.text_file(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },


    SalesInvoicePartNo: function () {
        var self = SalesInvoice;
        $.ajax({
            url: '/Reports/Sales/SalesInvoicePartNo',
            data: {
                id: $("#ID").val(),
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



    SalesInvoiceExportitemcode: function () {
        var self = SalesInvoice;
        $.ajax({
            url: '/Reports/Sales/SalesInvoiceExportitemcode',
            data: {
                id: $("#ID").val(),
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


    SalesInvoiceExportpartno: function () {
        var self = SalesInvoice;
        $.ajax({ 
            url: '/Reports/Sales/SalesInvoiceExportpartno',
            data: {
                id: $("#ID").val(),
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


    Exportpdf: function () {
        var self = SalesInvoice;
        $.ajax({
            url: '/Reports/Sales/SalesInvoiceExportPrintPdf',
            data: {
                id: $("#ID").val(),
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

    SalesInvoiceWithCode: function () {
        var self = SalesInvoice;
        $.ajax({
            url: '/Reports/Sales/SalesInvoiceWithCode',
            data: {
                id: $("#ID").val(),
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

    SalesInvoiceDetail: function () {
        var self = SalesInvoice;
        var id = $(this).parents('tr').find('.ID').val();
        $.ajax({
            url: '/Reports/Sales/SalesInvoiceDetail',
            data: {
                id: id
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


    SalesInvoiceExportDetail: function () {
        var self = SalesInvoice;
        var id = $(this).parents('tr').find('.ID').val();
        $.ajax({
            url: '/Reports/Sales/SalesInvoiceExportDetail',
            data: {
                id: id
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
        var self = SalesInvoice;
        $.ajax({
            url: '/Reports/Sales/SalesInvoicePrintPdf',
            data: {
                id: $("#ID").val(),
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

    //print: function () {
    //    var self = SalesInvoice;
    //    app.get_print_preferences(self.print_preview);
    //},

    cancel_confirm: function () {
        var self = SalesInvoice
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();

        }, function () {
        })
    },

    cancel: function () {
        $.ajax({
            url: '/Sales/SalesInvoice/Cancel',
            data: {
                SalesInvoiceID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    if (data.data == 1) {
                        app.show_notice("Sales Invoice cancelled successfully");
                        setTimeout(function () {
                            window.location = "/Sales/SalesInvoice/Index";
                        }, 1000);
                    }
                    else {
                        app.show_error("Could not Cancel Sales Invoice");
                    }

                } else {
                    app.show_error("Failed to cancel.");
                }
            },
        });
    },

    freeze_headers: function () {
        fh_items = $("#sales-invoice-items-list").FreezeHeader();
        $('#tabs-invoice[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "item-tab") {
                    fh_items.resizeHeader();
                }
            }, 500);
        });
    },
    // gets the discount and offer details of multiple items
    get_offer_details_bulk: function (ItemIDs, UnitIDs) {
        var self = SalesInvoice;

        var CustomerID = $("#CustomerID").val();
        var SchemeID = $("#SchemeID").val();

        $.ajax({
            url: '/Sales/SalesOrder/GetOfferDetails/',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
                SchemeID: SchemeID,
                ItemID: ItemIDs,
                Qty: 0,
                UnitID: UnitIDs
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.discountAndOffer).each(function (i, record) {
                        if (self.search_in_array(self.item_offer_details, record.ItemID) == -1) {
                            var obj = { ItemID: record.ItemID, OfferDetails: record.OfferDetails }
                            self.item_offer_details.push(obj);
                        }
                    });
                }
            }
        });
    },

    item_offer_details: [],
    PackingDetails: [],


    search_in_array: function (array, item_id) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].ItemID == item_id) {
                return i;
            }
        }
        return -1;
    },

    search_tax_group: function (array, value, type) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].Percentage == value && array[i].Particulars == type) {
                return i;
            }
        }
        return -1;
    },

    set_offer_qty: function () {
        var self = SalesInvoice;
        var qty = clean($('#BatchQty').val());
        var item_id = $("#BatchItemID").val();
        var offer_qty = self.get_offer_qty(qty, item_id);
        $("#BatchOfferQty").val(offer_qty);
    },
}