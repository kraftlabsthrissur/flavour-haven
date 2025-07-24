var fh_items;
var list_table;
var gridcurrencyclass = '';
var DecPlaces = 0;
var SOID = [];
$(function () {
    gridcurrencyclass = $("#normalclass").val();
    DecPlaces = clean($("#DecimalPlaces").val());
    //alert(normalclass);
});
GoodsReceipt = {
    // Initializes the page
    init: function () {
        var self = GoodsReceipt;

        self.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#SalesOrderNos",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        self.sales_order_list();
        self.sales_invoice_list();
        self.counter_sales_list();
        $('#sales-order-list').SelectTable({
            selectFunction: self.select_sales_order,
            returnFocus: "#TurnoverDiscount",
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        });

        $('#sales-invoice-list').SelectTable({
            selectFunction: self.select_sales_invoice,
            returnFocus: "#PaymentModeID",
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        });

        $('#counter-sales-list').SelectTable({
            selectFunction: self.select_counter_sales,
            returnFocus: "#PaymentModeID",
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        });

        self.bind_events();
        self.Receipt = {};
        self.on_change_customer_category();
        self.Receipt.InvoiceNo = $("#InvoiceNo").val();
        self.Receipt.InvoiceDate = $("#InvoiceDate").val();
        self.Receipt.AmountDetails = [];
        self.Receipt.SalesTypeID = $("#ItemCategoryID").val();
        self.Items = [];
        self.SalesOrderIDs = [];
        self.OrderNos = [];
        self.Receipt.PackingDetails = [];
        if ($("#ID").val() != 0) {
            //self.get_offer_details_bulk();
            self.set_object_values();
        }
        self.freeze_headers();
    },
    //Bind the events to elements  
    bind_events: function () {
        var self = GoodsReceipt;
        $('#Qty').keydown(function (e) {
            if (e.which == 13) {
                self.add_item();
            }
        });

        //Bind auto complete event for item 
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);

        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);

        $("#ItemCategoryID").on("change", self.get_sales_category);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKCustomer").on("click", self.select_customer);
        $("#btnAddItem").on("click", self.add_item);
        $(".btnSave,.btnSaveASDraft").on("click", self.on_save);
        $(".ItemCode").on("click", self.GoodsReceiptNoteitemcode);
        $(".PartNo").on("click", self.GoodsReceiptNotePartNo);



        $("body").on('ifChanged', '.include-item', self.include_item);
        $("body").on('ifChanged', '#sales-order-list tbody input[type="checkbox"]', self.SalesOrderUnCheck);

        $("#btnOkSalesOrder").on('click', self.select_sales_order);
        $("#btnOkSalesInvoice").on('click', self.select_sales_invoice);
        $("#btnOkCounterSales").on('click', self.select_counter_sales);
        $("body").on("ifChanged", "#sales-order-list .SalesOrderID", self.choose_sales_order);
        $('body').on('click', '#sales-invoice-items-list tbody td:not(.action)', self.show_batch_edit_modal);
        $('body').on('keyup change', '#batch-list .InvoiceQty', self.set_total_invoice_qty);
        $('body').on('keyup change', '#batch-list .InvoiceOfferQty', self.set_total_invoice_offer_qty);
        $('body').on('click', '#btnOkBatches', self.replace_batches);
        $("body").on('change keyup', "#BatchQty", self.set_offer_qty);
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on('click', ".remove-item", self.remove_item);
        $("body").on("change", "#CustomerCategoryID", self.change_customer_category);
    },
    customer_list: function (type) {
        var url;
        if (type == "sales-order") {
            url = "/Masters/Customer/GetSalesOrderCustomerList";
        } else if (type == "sales-invoice") {
            url = "/Masters/Customer/GetSalesInvoiceCustomerList";
        } else {
            url = "/Masters/Customer/GetCustomerList"
        }
        var $list = $('#customer-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CustomerCategoryID", Value: $('#CustomerCategoryID').val() },
                            { Key: "StateID", Value: $('#CustomerStateID').val() },
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
                            return "<input type='radio' class='uk-radio CustomerID' name='CustomerID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='StateID' value='" + row.StateID + "'>"
                                + "<input type='hidden' class='PriceListID' value='" + row.PriceListID + "'>"
                                + "<input type='hidden' class='CountryID' value='" + row.CountryID + "'>"
                                + "<input type='hidden' class='DistrictID' value='" + row.DistrictID + "'>"
                                + "<input type='hidden' class='CustomerCategoryID' value='" + row.CustomerCategoryID + "'>"
                                + "<input type='hidden' class='SchemeID' value='" + row.SchemeID + "'>"
                                + "<input type='hidden' class='MinimumCreditLimit' value='" + row.MinimumCreditLimit + "'>"
                                + "<input type='hidden' class='MaxCreditLimit' value='" + row.MaxCreditLimit + "'>"
                                + "<input type='hidden' class='CashDiscountPercentage' value='" + row.CashDiscountPercentage + "'>"
                                + "<input type='hidden' class='IsBlockedForChequeReceipt' value='" + row.IsBlockedForChequeReceipt + "'>"
                                + "<input type='hidden' class='OutStandingAmount' value='" + row.OutStandingAmount + "'>"
                                + "<input type='hidden' class='CurrencyID' value='" + row.CurrencyID + "'>"
                                + "<input type='hidden' class='CurrencyPrefix' value='" + row.CurrencyPrefix + "'>"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.CurrencyCode + "'>"
                                + "<input type='hidden' class='CurrencyConversionRate' value='" + row.CurrencyConversionRate + "'>"
                                + "<input type='hidden' class='DecimalPlaces' value='" + row.DecimalPlaces + "'>"
                                + "<input type='hidden' class='IsGSTRegistered' value='" + row.IsGSTRegistered + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Address", "className": "Address" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                    { "data": "CurrencyName", "className": "CurrencyName" },
                    { "data": "CurrencyConversionRate", "className": "CurrencyConversionRate" },


                ],



                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#CustomerCategoryID,#CustomerStateID', function () {
                list_table.fnDraw();
            });

            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },
    change_customer_category: function () {
        var self = GoodsReceipt;
        var customerid = clean($('#CustomerID').val());
        if (customerid > 0) {
            app.confirm("Selected customer will be removed", function () {
                $('#CustomerID').val(0);
                $('#CustomerName').val('');
                $("#BillingAddressID").val('');
                $("#ShippingAddressID").val('');
                $("#GrossAmount").val('');
                self.clear_form();
                self.on_change_customer_category();
            })
        }
        else {
            self.on_change_customer_category();
        }
    },
    on_change_customer_category: function () {
        var self = GoodsReceipt;
        var CustomerCategory = $("#CustomerCategoryID option:selected").text();
        var id = clean($("#ID").val());
        if (CustomerCategory == 'ECOMMERCE') {
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
        var self = GoodsReceipt;
        var sum = 0;
        var Quantity;
        var SalesInvoiceTransID = clean($(this).closest("tr").find(".SalesInvoiceTransID").val());
        var SalesOrderItemTransID = clean($(this).closest("tr").find(".SalesOrderItemTransID").val());
        var CounterSalesItemTransID = clean($(this).closest("tr").find(".CounterSalesItemTransID").val());
        app.show_loading();
        if (SalesInvoiceTransID > 0) {
            self.Items = $.grep(self.Items, function (e) { return e.SalesInvoiceTransID != SalesInvoiceTransID });
        } else if (SalesOrderItemTransID > 0) {
            self.Items = $.grep(self.Items, function (e) { return e.SalesOrderItemTransID != SalesOrderItemTransID });
        } else if (CounterSalesItemTransID > 0) {
            self.Items = $.grep(self.Items, function (e) { return e.CounterSalesItemTransID != CounterSalesItemTransID });
        }
        self.process_invoice(self.Items, false);
        app.hide_loading();

    },


    set_object_values: function () {
        var self = GoodsReceipt;
        self.Receipt.ID = $("#ID").val();
        self.Receipt.CustomerID = $("#CustomerID").val();
        self.Receipt.SchemeID = $("#SchemeID").val();
        self.Receipt.SalesOrderNos = $("#SalesOrderNos").val();
        self.Receipt.BillingAddressID = $("#BillingAddressID").val();
        self.Receipt.ShippingAddressID = $("#ShippingAddressID").val();
        self.Receipt.CessAmount = $("#CessAmount").val();
        var item = {};
        $("#goods-receipt-items-list tbody tr").each(function (i, record) {
            item = {
                ItemID: $(record).find(".ItemID").val(),
                Code: $(record).find(".Code").val(),
                ItemName: $(record).find(".ItemName").val(),
                PartsNumber: $(record).find(".PartsNumber").val(),
                Remarks: $(record).find(".Remarks").val(),
                Model: $(record).find(".Model").val(),
                TransNo: $(record).find(".TransNo").val(),
                UnitName: $(record).find(".UnitName").text().trim(),
                SecondaryUnit: $(record).find(".SecondaryUnit").text().trim(),
                SalesInvoiceID: clean($(record).find(".SalesInvoiceID").val()),
                SalesInvoiceTransID: clean($(record).find(".SalesInvoiceTransID").val()),
                SalesOrderID: clean($(record).find(".SalesOrderID").val()),
                SalesOrderItemTransID: clean($(record).find(".SalesOrderItemTransID").val()),
                CounterSalesID: clean($(record).find(".CounterSalesID").val()),
                CounterSalesItemTransID: clean($(record).find(".CounterSalesItemTransID").val()),
                IsGST: clean($(record).find(".IsGST").val()),
                IsVat: clean($(record).find(".IsVat").val()),
                UnitID: clean($(record).find(".UnitID").val()),
                PrintWithItemName: $(record).find(".PrintWithItemName").val(),
                CurrencyID: clean($(record).find(".CurrencyID").val()),
                CGSTPercentage: clean($(record).find(".CGSTPercentage").val()),
                IGSTPercentage: clean($(record).find(".IGSTPercentage").val()),
                SGSTPercentage: clean($(record).find(".SGSTPercentage").val()),
                DiscountPercentage: clean($(record).find(".DiscountPercentage").val()),
                Qty: clean($(record).find(".OrderQty").val()),
                OfferQty: clean($(record).find(".OfferQty").val()),
                StoreID: clean($("#StoreID").val()),
                SalesOrderItemID: clean($(record).find(".OrderTransID").val()),
                BatchID: clean($(record).find(".BatchID").val()),
                BatchName: $(record).find(".BatchName").text(),
                BatchTypeName: $(record).find(".BatchType").text(),
                BatchTypeID: clean($(record).find(".BatchTypeID").val()),
                MRP: clean($(record).find(".MRP").val()),
                SecondaryMRP: clean($(record).find(".SecondaryMRP").val()),
                Stock: clean($(record).find(".Stock").val()),
                InvoiceQty: clean($(record).find(".InvoiceQty").val()),
                SecondaryQty: clean($(record).find(".SecondaryQty").val()),
                InvoiceOfferQty: clean($(record).find(".InvoiceOfferQty").val()),
                IsIncluded: true,
                InvoiceOfferQtyMet: $(record).hasClass("offer-qty-not-met") ? false : true,
                InvoiceQtyMet: $(record).hasClass("qty-not-met") ? false : true,
                SalesUnitID: clean($(record).find(".SalesUnitID").val()),
                Rate: clean($(record).find(".Rate").val()),
                LooseRate: clean($(record).find(".LooseRate").val()),
                CessAmount: clean($(record).find(".CessAmount").val()),
                CessPercentage: clean($(record).find(".CessPercentage").val()),
                PackSize: clean($(record).find(".PackSize").val()),
                PrimaryUnit: $(record).find(".PrimaryUnit").val(),

            }
            self.Items.push(item);
        });
        self.Items = self.process_items(self.Items);
        self.calculate_grid_total();
        self.set_packing_details();
    },

    choose_sales_order: function () {
        var self = GoodsReceipt;
        var ID = $(this).val();
        var OrderNo = $(this).closest("tr").find(".SONo").text().trim();
        if ($(this).is(":checked")) {
            self.SalesOrderIDs.push(ID);
            self.OrderNos.push(OrderNo)
            $("#ItemCategoryID").val($(this).closest("tr").find(".SalesTypeID").val());
        } else {
            var index = self.SalesOrderIDs.indexOf(ID);
            if (index !== -1) self.SalesOrderIDs.splice(index, 1);
            index = self.OrderNos.indexOf(OrderNo);
            if (index !== -1) self.OrderNos.splice(index, 1);
        }
    },

    select_sales_order: function () {
        var self = GoodsReceipt;

        var TransNos = $("#SalesOrderNos").val();
        var checkboxes = $('#sales-order-list tbody input[type="checkbox"]:checked');
        var row;

        if ($("#goods-receipt-items-list tbody tr").length > 0) {

            app.confirm("Items will be removed", function () {
                self.clear_form();
                $.each(checkboxes, function () {
                    if (!SOID.includes($(this).val())) {
                        row = $(this).closest("tr");
                        SOID.push($(this).val());
                        TransNos += $(row).find(".SalesOrderNo").text().trim() + ",";
                        var TransNosTR = TransNos;
                        TransNosTR = TransNosTR.slice(0, -1);
                        $("#SalesOrderNos").val(TransNosTR);
                        TransNos = TransNosTR;
                    }
                });
                self.Receipt.SalesOrderNos = TransNos;
                self.calculate_freight_tax();
                self.get_sales_order_items(SOID);
                self.ClearSalesOrder();
            }, function () {

            });

        } else {
            $.each(checkboxes, function () {
                if (!SOID.includes($(this).val())) {
                    row = $(this).closest("tr");
                    SOID.push($(this).val());
                    TransNos += $(row).find(".SalesOrderNo").text().trim() + ",";
                    var TransNosTR = TransNos;
                    TransNosTR = TransNosTR.slice(0, -1);
                    $("#SalesOrderNos").val(TransNosTR);
                    TransNos = TransNosTR;
                }
            });

            self.Receipt.SalesOrderNos = TransNos;
            self.calculate_freight_tax();
            self.get_sales_order_items(SOID);
            self.ClearSalesOrder();
        }
    },
    ClearSalesOrder: function () {
        SOID = [];
        var checkedboxes = $('#sales-order-list tbody input[type="checkbox"]:checked)');
        if (checkedboxes.length > 0) {
            $.each(checkedboxes, function () {
                $(this).iCheck('uncheck');
            });
        }
    },
    SalesOrderNewPageLoaded: function () {
        var uncheckedboxes = $('#sales-order-list tbody input[type="checkbox"]:not(:checked)');
        if (uncheckedboxes.length > 0) {
            $.each(uncheckedboxes, function () {
                if (SOID.includes($(this).val())) {
                    $(this).iCheck('check');
                }
            });
        }
    },
    SalesOrderPageChange: function () {
        var checkedboxes = $('#sales-order-list tbody input[type="checkbox"]:checked');
        var TransNos = $("#SalesOrderNos").val();
        if (checkedboxes.length > 0) {
            $.each(checkedboxes, function () {
                if (!SOID.includes($(this).val())) {
                    row = $(this).closest("tr");
                    SOID.push($(this).val());
                    TransNos += $(row).find(".SalesOrderNo").text().trim() + ",";
                    var TransNosTR = TransNos;
                    TransNosTR = TransNosTR.slice(0, -1);
                    $("#SalesOrderNos").val(TransNosTR);
                }
            });
        }
    },
    SalesOrderUnCheck: function () {
        var isChecked = $(this).is(":checked");
        var TransNos = $("#SalesOrderNos").val();
        if (!isChecked) {
            var remove = $(this).val();
            var row = $(this).closest("tr");
            var SalesOrderNo = $(row).find(".SalesOrderNo").text().trim();
            if (SOID.includes(remove)) {
                var index = SOID.indexOf(remove);
                SOID.splice(index, 1);
                TransNos = TransNos.replace(SalesOrderNo, "");
                TransNos = TransNos.replace(",,", ",");
                $("#SalesOrderNos").val(TransNos);
            }
        }
    },
    select_sales_invoice: function () {
        var self = GoodsReceipt;
        var ID = [];
        var TransNos = "";
        var checkboxes = $('#sales-invoice-list tbody input[type="checkbox"]:checked');
        var row;
        if ($("#goods-receipt-items-list tbody tr").length > 0) {

            app.confirm("Items will be removed", function () {
                self.clear_form();
                $.each(checkboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                    TransNos += $(row).find(".TransNo").text().trim() + ",";
                    var TransNosTR = TransNos;
                    TransNosTR = TransNosTR.slice(0, -1);
                    $("#SalesOrderNos").val(TransNosTR);
                    self.Receipt.SalesOrderNos = TransNosTR;
                });
                self.calculate_freight_tax();
                self.get_sales_invoice_items(ID);
            }, function () {

            });
        } else {
            $.each(checkboxes, function () {
                row = $(this).closest("tr");
                ID.push($(this).val());
                TransNos += $(row).find(".TransNo").text().trim() + ",";
                var TransNosTR = TransNos;
                TransNosTR = TransNosTR.slice(0, -1);
                $("#SalesOrderNos").val(TransNosTR);
                self.Receipt.SalesOrderNos = TransNosTR;
            });
            self.calculate_freight_tax();
            self.get_sales_invoice_items(ID);
        }

    },
    select_counter_sales: function () {
        var self = GoodsReceipt;
        var ID = [];
        var TransNos = "";
        var checkboxes = $('#counter-sales-list tbody input[type="checkbox"]:checked');
        var row;

        if ($("#goods-receipt-items-list tbody tr").length > 0) {

            app.confirm("Items will be removed", function () {
                self.clear_form();
                $.each(checkboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                    TransNos += $(row).find(".TransNo").text().trim() + ",";
                    var TransNosTR = TransNos;
                    TransNosTR = TransNosTR.slice(0, -1);
                    $("#SalesOrderNos").val(TransNosTR);
                    self.Receipt.SalesOrderNos = TransNosTR;
                });
                self.calculate_freight_tax();
                self.get_counter_sales_items(ID);
            }, function () {

            });
        } else {
            $.each(checkboxes, function () {
                row = $(this).closest("tr");
                ID.push($(this).val());
                TransNos += $(row).find(".TransNo").text().trim() + ",";
                var TransNosTR = TransNos;
                TransNosTR = TransNosTR.slice(0, -1);
                $("#SalesOrderNos").val(TransNosTR);
                self.Receipt.SalesOrderNos = TransNosTR;
            });
            self.calculate_freight_tax();
            self.get_counter_sales_items(ID);
        }

    },
    calculate_freight_tax: function () {
        var TotalFreightAmt = clean($("#TotalFreightAmt").val());
        var FreightTax = clean($("#FreightTax").val());
        var FreightTaxAmt = TotalFreightAmt * FreightTax / 100;
        var FreightAmt = TotalFreightAmt - FreightTaxAmt;
        $("#FreightAmount").val(FreightAmt);
        $("#FreightTaxAmt").val(FreightTaxAmt);
    },
    get_sales_invoice_items: function (ID) {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Sales/SalesInvoice/GetGoodsReceiptSalesInvoiceItems',
            dataType: "json",
            data: {
                SalesInvoiceID: ID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.discountAndOffer).each(function (i, record) {
                        if (self.search_in_array(self.item_offer_details, record.ItemID) == -1) {
                            var obj = { ItemID: record.ItemID, UnitID: record.UnitID, OfferDetails: record.OfferDetails }
                            self.item_offer_details.push(obj);
                        }
                    });
                    self.process_invoice(response.Items, true);
                }
            }
        });
    },

    get_sales_order_items: function (ID) {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Sales/SalesOrder/GetGoodsReceiptSalesOrderItems',
            dataType: "json",
            data: {
                SalesOrderID: ID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.process_invoice(response.Items, true);
                }
            }
        });
    },
    get_counter_sales_items: function (ID) {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Sales/CounterSales/GetGoodsReceiptsItemForCounterSales',
            dataType: "json",
            data: {
                CounterSalesID: ID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.process_invoice(response.Items, true);
                }
            }
        });
    },

    sales_order_list: function () {
        var self = GoodsReceipt;

        var $list = $('#sales-order-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesOrder/GetCustomerSalesOrderList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
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
                            return "<input type='checkbox' class='uk-radio SalesOrderID' name='SalesOrderID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "SalesOrderNo", "className": "SalesOrderNo" },
                    { "data": "OrderDate", "className": "OrderDate" },
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
            $list.on('page.dt', function () {
                self.SalesOrderPageChange();
            });
            list_table.on('draw.dt', function () {
                self.SalesOrderNewPageLoaded();
            });
            return list_table;
        }
    },

    sales_invoice_list: function () {

        var $list = $('#sales-invoice-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesInvoice/GetCustomerSalesInvoiceList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
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
                            return "<input type='checkbox' class='uk-radio SalesInvoiceID' name='SalesInvoiceID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "InvoiceDate", "className": "InvoiceDate" },
                    { "data": "CustomerName", "className": "CustomerName" },
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

    counter_sales_list: function () {

        var $list = $('#counter-sales-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/CounterSales/GetCustomerCounterSalesList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
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
                            return "<input type='checkbox' class='uk-radio ProformaInvoiceID' name='ProformaInvoiceID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "PartyName", "className": "PartyName" },
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
    details: function () {
        var self = GoodsReceipt;
        self.freeze_headers();
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".ExportPrintPdf", self.ExportPrintPdf);
    },

    print: function () {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Sales/GoodsReceiptNote/Print',
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



    ExportPrintPdf: function () {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Reports/Sales/GoodsReceiptNoteExportPrintPdf',
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
    printpdf: function () {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Reports/Sales/GoodsReceiptNotePrintPdf',
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


    GoodsReceiptNotePartNo: function () {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Reports/Sales/GoodsReceiptNotePartNo',
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



    GoodsReceiptNoteitemcode: function () {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Reports/Sales/GoodsReceiptNoteitemcode',
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





    cancel_confirm: function () {
        var self = GoodsReceipt
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        $.ajax({
            url: '/Sales/GoodsReceiptNote/Cancel',
            data: {
                GoodsReceiptID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Proforma Invoice cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/GoodsReceiptNote/Index";
                    }, 1000);
                }
                else if (data.Status == "fail") {
                    app.show_error("Please cancel sales invoice");
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
    // Gets the items for auto complete
    get_customers: function (release) {
        var self = GoodsReceipt;
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
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
    // Selects the customer on click modal ok button
    select_customer: function () {
        var self = GoodsReceipt;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var CustomerCategory = $(row).find(".CustomerCategory").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
        var OutStandingAmount = $(row).find(".OutStandingAmount").val();
        var MaxCreditLimit = $(row).find(".MaxCreditLimit").val();
        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyName = $(row).find(".CurrencyName").text().trim();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        var DecimalPlaces = $(row).find(".DecimalPlaces").val();
        $("#CustomerCategoryID").val(CustomercategoryID);
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#SchemeID").val(SchemeID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
        $("#CustomerCategory").val(CustomerCategory);
        $("#OutStandingAmount").val(OutStandingAmount);
        $("#MaxCreditLimit").val(MaxCreditLimit);
        $("#CurrencyID").val(CurrencyID);
        $("#CurrencyName").val(CurrencyName);
        $("#CurrencyCode").val(CurrencyCode);
        $("#CurrencyExchangeRate").val(CurrencyConversionRate);
        //gridcurrencyclass = app.change_decimalplaces($("#NetAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#GrossAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#AdditionalDiscount"), DecimalPlaces);
        //app.change_decimalplaces($("#OutStandingAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#MaxCreditLimit"), DecimalPlaces);
        //app.change_decimalplaces($("#CessAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#RoundOff"), DecimalPlaces);
        //app.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#DiscountAmount"), DecimalPlaces);
        //app.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);
        UIkit.modal($('#select-customer')).hide();
        self.Receipt.CustomerID = ID;
        self.customer_on_change();
        self.on_change_customer_category();
    },
    // on select auto complete customer
    set_customer: function (event, item) {
        var self = GoodsReceipt;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#CustomerCategory").val(item.customerCategory);
        $("#SchemeID").val(item.schemeId);
        $("#SalesOrderNos").focus();
        $("#CustomerCategoryID").val(item.customercategoryid);
        $("#OutStandingAmount").val(item.outstandingAmount);
        $("#MaxCreditLimit").val(item.maxCreditLimit);
        self.Receipt.CustomerID = item.id;
        self.customer_on_change();
        self.on_change_customer_category();
    },

    customer_on_change: function () {
        var self = GoodsReceipt;
        self.get_batch_type();
        // self.get_scheme_allocation();
        self.clear_form();
        self.SalesOrderIDs = [];
        self.OrderNos = [];
        // $("#CustomerID").trigger("change");
        Sales.get_customer_addresses();
        //$("#SalesOrderNos").focus();
    },

    get_batch_type: function () {
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Masters/Customer/GetBatchType',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#BatchTypeID").val(response.BatchTypeID);
                    $("#CustomerID").trigger("change");
                }
            }
        });
    },

    clear_form: function () {
        var self = GoodsReceipt;
        $("#sales-invoice-items-list tbody").html('');
        $("#sales-invoice-amount-details-list tbody").html('');
        $("#SalesOrderNos").val('');
        $("#GrossAmount").val('');
        $("#DiscountAmount").val('');
        $("#AdditionalDiscount").val('');
        $("#TaxableAmount").val('');
        $("#SGSTAmount").val('');
        $("#CGSTAmount").val('');
        $("#IGSTAmount").val('');
        $("#RoundOff").val('');
        $("#NetAmount").val('');

        self.Receipt.GrossAmount = 0;
        self.Receipt.DiscountAmount = 0;
        self.Receipt.AdditionalDiscount = 0;
        self.Receipt.TaxableAmount = 0;
        self.Receipt.NetAmount = 0;
        self.Receipt.RoundOff = 0;
        self.Receipt.CGSTAmount = 0;
        self.Receipt.SGSTAmount = 0;
        self.Receipt.IGSTAmount = 0;
        self.Items = [];
    },
    // gets the scheme for the selected customer
    get_scheme_allocation: function () {
        var self = GoodsReceipt;
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
                    self.Receipt.SchemeID = response.SchemeID;
                }
            }
        });
    },

    // Gets the items for auto complete
    get_items: function (release) {
        var self = GoodsReceipt;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        $.ajax({
            url: '/Masters/Item/GetSaleableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $('#ItemCategoryID').val(),
                SalesCategoryID: $('#SalesCategoryID').val(),
                PriceListID: $('#PriceListID').val(),
                StoreID: $('#StoreID').val(),
                CheckStock: $('#CheckStock').val(),
                BatchTypeID: $('#BatchTypeID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    // on select auto complete item
    set_item: function (event, item) {
        var self = GoodsReceipt;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#PrimaryUnit").val(item.unit);
        $("#Code").val(item.code);
        $("#PrimaryUnitID").val(item.unitId);
        $("#Stock").val(item.stock);
        $("#CGSTPercentage").val(item.cgstpercentage);
        $("#IGSTPercentage").val(item.igstpercentage);
        $("#SGSTPercentage").val(item.sgstpercentage);
        $("#SalesUnit").val(item.saleUnit);
        $("#SalesUnitID").val(item.saleUnitId);
        $("#Rate").val(item.rate);
        $("#FromStoreID").val($("#StoreID").val());
        $("#LooseRate").val(item.looseRate);
        $("#CessPercentage").val(item.cessPercentage);
        $("#Qty").focus();
        self.get_units();
        self.get_discount_and_offer_details();
    },
    // Selects the item on clicking the modal ok button 
    select_item: function () {
        var self = GoodsReceipt;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var UnitID = $(row).find(".UnitID").val();
        var Stock = $(row).find(".Stock").val();
        var CGSTPercentage = $(row).find(".CGSTPercentage").val();
        var IGSTPercentage = $(row).find(".IGSTPercentage").val();
        var SGSTPercentage = $(row).find(".SGSTPercentage").val();
        var Rate = $(row).find(".Rate").val();
        var SalesUnit = $(row).find(".SalesUnit").val();
        var SalesUnitID = $(row).find(".SalesUnitID").val();
        var Rate = $(row).find(".Rate").val();
        var LooseRate = $(row).find(".LooseRate").val();
        var CessPercentage = $(row).find(".CessPercentage").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#Unit").val(Unit);
        $("#UnitID").val(UnitID);
        $("#Stock").val(Stock);
        $("#CGSTPercentage").val(CGSTPercentage);
        $("#IGSTPercentage").val(IGSTPercentage);
        $("#SGSTPercentage").val(SGSTPercentage);
        $("#PrimaryUnit").val(Unit);
        $("#PrimaryUnitID").val(UnitID);
        $("#SalesUnit").val(SalesUnit);
        $("#SalesUnitID").val(SalesUnitID);
        $("#Rate").val(Rate);
        $("#FromStoreID").val($("#StoreID").val());
        $("#LooseRate").val(LooseRate);
        $("#CessPercentage").val(CessPercentage);
        $("#Qty").focus();
        self.get_units();
        UIkit.modal($('#select-item')).hide();
        self.get_discount_and_offer_details();
    },
    get_units: function () {
        var self = GoodsReceipt;


        $("#UnitID").html("");

        var html;//= "<option value >Select</option>";          
        html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);

    },
    item_offer_details: [],
    PackingDetails: [],
    // gets the discount and offer details of multiple items on edit
    get_offer_details_bulk: function () {
        var self = GoodsReceipt;

        var CustomerID = $("#CustomerID").val();
        var SchemeID = $("#SchemeID").val();

        var ItemIDs = [];
        var UnitIDs = [];
        $('#sales-invoice-items-list tbody .ItemID').each(function () {
            var row = $(this).closest('tr');
            ItemIDs.push($(this).val());
            UnitIDs.push($(row).find('.UnitID').val());
        });

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
                            var obj = { ItemID: record.ItemID, UnitID: response.discountAndOffer.UnitID, OfferDetails: record.OfferDetails }
                            self.item_offer_details.push(obj);
                        }
                    });
                }
            }
        });
    },
    // gets the discount and offer details on selecting an item
    get_discount_and_offer_details: function () {
        var self = GoodsReceipt;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        var CustomerID = $("#CustomerID").val();
        var SchemeID = $("#SchemeID").val();
        var ItemID = $("#ItemID").val();
        var UnitID = $("#UnitID option:selected").val();
        $.ajax({
            url: '/Sales/SalesOrder/GetDiscountAndOfferDetails/',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
                SchemeID: SchemeID,
                ItemID: ItemID,
                Qty: 0,
                UnitID: UnitID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#DiscountPercentage").val(response.discountAndOffer.DiscountPercentage);
                    if (self.search_in_array(self.item_offer_details, ItemID) == -1) {
                        var obj = {
                            ItemID: ItemID,
                            UnitID: response.discountAndOffer.UnitID,
                            OfferDetails: response.discountAndOffer.OfferDetails,
                            DiscountPercentage: response.discountAndOffer.DiscountPercentage
                        }
                        self.item_offer_details.push(obj);
                    }
                }
            }
        });
    },
    // Gets the sales categories for selected item categories
    get_sales_category: function () {
        var self = GoodsReceipt;
        var item_category_id = $(this).val();
        self.Receipt.SalesTypeID = item_category_id;
        if (item_category_id == null || item_category_id == "") {
            item_category_id = 0;
        }
        $.ajax({
            url: '/Masters/Category/GetSalesCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#SalesCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#SalesCategoryID").append(html);
            }
        });
    },

    add_item: function () {
        var self = GoodsReceipt;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            $("#ItemName").focus();
            return;
        }
        var ItemID = $("#ItemID").val();
        var Qty = clean($("#Qty").val());
        var UnitID = $("#UnitID option:selected").val();
        var OfferQty = self.get_offer_qty(Qty, ItemID, UnitID);
        var StoreID = $("#FromStoreID").val();
        var CustomerID = $("#CustomerID").val();
        var SalesUnitID = $("#SalesUnitID").val();
        var item = {};
        $.ajax({
            url: '/Sales/GoodsReceiptNote/GetItemBatchwise/',
            dataType: "json",
            data: {
                ItemID: ItemID,
                Qty: Qty,
                OfferQty: OfferQty,
                StoreID: StoreID,
                CustomerID: CustomerID,
                UnitID: UnitID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.BatchwiseItems).each(function (i, record) {
                        item = {
                            ItemName: $("#ItemName").val(),
                            ItemID: ItemID,
                            UnitName: $("#UnitID option:selected").text(),
                            Code: $("#Code").val(),
                            UnitID: $("#UnitID option:selected").val(),
                            SalesUnitID: SalesUnitID,
                            Rate: $("#Rate").val(),
                            LooseRate: $("#LooseRate").val(),
                            CGSTPercentage: clean($("#CGSTPercentage").val()),
                            IGSTPercentage: clean($("#IGSTPercentage").val()),
                            SGSTPercentage: clean($("#SGSTPercentage").val()),
                            DiscountPercentage: clean($("#DiscountPercentage").val()),
                            Qty: Qty,
                            OfferQty: OfferQty,
                            StoreID: clean($("#FromStoreID").val()),
                            InvoiceTransID: 0,
                            OrderTransID: 0,
                            BatchID: record.BatchID,
                            BatchName: record.BatchNo,
                            BatchTypeID: record.BatchTypeID,
                            BatchTypeName: record.BatchTypeName,
                            Stock: record.Stock,
                            InvoiceQty: record.InvoiceQty,
                            InvoiceOfferQty: record.InvoiceOfferQty,
                            InvoiceOfferQtyMet: record.InvoiceOfferQtyMet,
                            InvoiceQtyMet: record.InvoiceQtyMet,
                            IsIncluded: true,
                            OfferQtyMet: record.InvoiceOfferQtyMet ? "" : "offer-qty-not-met",
                            QtyMet: record.InvoiceQtyMet ? "" : "qty-not-met",
                            CessPercentage: clean($("#CessPercentage").val())
                        }
                        self.Items.push(item);
                    });
                    self.process_invoice(self.Items, true);
                    self.clear_item();
                    setTimeout(function () {
                        $("#ItemName").focus();
                    }, 100);
                }
            }
        });
    },
    is_gst_registered_customer: function () {
        return $("#IsGSTRegistered").val().toLowerCase() == "true" ? true : false;
    },
    process_invoice: function (items, scrolldown) {
        var self = GoodsReceipt;
        self.Items = self.process_items(items);
        self.calculate_grid_total();
        self.set_amount_details();
        self.set_packing_details();
        self.add_items_to_grid(scrolldown);
        self.set_form_values();
    },
    is_cess_effect: function () {
        var self = GoodsReceipt;
        if (self.is_igst()) {
            return false;
        }
        if (self.is_gst_registered_customer()) {
            return false;
        }
        return $("#LocationStateID").val() == 32;
    },
    process_items: function (items) {
        var self = GoodsReceipt;
        var processed_items = [];
        $.each(items, function (i, item) {
            item = self.get_item_properties(item);
            processed_items.push(item);
        });
        return processed_items;
    },

    // calculates and returns item properties
    get_item_properties: function (item) {
        var self = GoodsReceipt;
        var IsIGST = self.is_igst();
        var CustomerCategory = $("#CustomerCategory").val();
        item.StoreID = $("#StoreID").val();
        item.OfferQtyMet = item.InvoiceOfferQtyMet ? "" : "offer-qty-not-met";
        item.QtyMet = item.InvoiceQtyMet ? "" : "qty-not-met";
        if (item.SalesUnitID == item.UnitID) {
            item.MRP = item.Rate;
        }
        else {
            item.MRP = item.LooseRate;
        }
        if (self.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        } else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        if (CustomerCategory == "Export") {
            item.BasicPrice = item.MRP;
        }
        item.BasicPrice = item.BasicPrice.roundToCustom();
        item.GrossAmount = item.BasicPrice * (item.InvoiceQty);
        item.AdditionalDiscount = item.BasicPrice * item.InvoiceOfferQty;
        item.DiscountAmount = ((item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100).roundToCustom();
        item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount;
        if (CustomerCategory == "Export") {
            item.CessAmount = 0;
            item.CessPercentage = 0;
            item.IGST = 0;
            item.CGST = 0;
            item.SGST = 0;
            item.GSTAmount = 0;
            item.IGSTPercentage = 0;
        }
        else {
            if (self.is_cess_effect()) {
                item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100).roundToCustom();
            } else {
                item.CessAmount = 0;
            }

            item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundToCustom();
            if (IsIGST) {
                item.IGST = item.GSTAmount;
                item.SGST = 0;
                item.CGST = 0;
            } else {
                item.IGST = 0;
                item.SGST = (item.GSTAmount / 2).roundToCustom();
                item.CGST = (item.GSTAmount / 2).roundToCustom();
            }
        }
        item.NetAmount = item.TaxableAmount + item.IGST + item.SGST + item.CGST + item.CessAmount;
        return item;
    },

    calculate_grid_total: function () {
        var self = GoodsReceipt;
        var FreightAmt = clean($("#TotalFreightAmt").val());
        GSTAmount = 0;
        self.Receipt.GrossAmount = 0;
        self.Receipt.DiscountAmount = 0;
        self.Receipt.AdditionalDiscount = 0;
        self.Receipt.TaxableAmount = 0;
        self.Receipt.NetAmount = 0;
        self.Receipt.RoundOff = 0;
        self.Receipt.GSTAmount = 0;
        self.Receipt.IGSTAmount = 0;
        self.Receipt.CGSTAmount = 0;
        self.Receipt.SGSTAmount = 0;
        self.Receipt.CessAmount = 0;
        $(self.Items).each(function (i, item) {
            self.Receipt.GrossAmount += item.GrossAmount;
            self.Receipt.DiscountAmount += item.DiscountAmount;
            self.Receipt.AdditionalDiscount += item.AdditionalDiscount;
            self.Receipt.TaxableAmount += item.TaxableAmount;
            self.Receipt.GSTAmount += item.GSTAmount;
            self.Receipt.IGSTAmount += item.IGST;
            self.Receipt.CGSTAmount += item.CGST;
            self.Receipt.SGSTAmount += item.SGST;
            self.Receipt.NetAmount += item.NetAmount;
            self.Receipt.CessAmount += item.CessAmount;
        });
        self.Receipt.NetAmount += FreightAmt;
        self.Receipt.RoundOff = 0;// Math.round(self.Receipt.NetAmount) - self.Receipt.NetAmount;
        self.Receipt.NetAmount += self.Receipt.RoundOff;

        self.Receipt.CheckStock = $("#CheckStock").val();


        self.set_amount_details();
    },

    set_packing_details: function () {
        var self = GoodsReceipt;

        self.Receipt.PackingDetails = [];
        $(self.Items).each(function (i, record) {
            self.calculate_group_packing_details(record.PackSize + ' ' + record.PrimaryUnit, record.InvoiceQty, record.UnitName, record.UnitID, record.PackSize);
        });

        self.Receipt.PackingDetails = self.Receipt.PackingDetails.sort(function (a, b) {
            var first = a.Pack;
            var second = b.Pack;
            return second - first
        });

        self.Receipt.PackingDetails = self.Receipt.PackingDetails.sort((a, b) => b - a);

        var tr = "";
        var $tr;
        $(self.Receipt.PackingDetails).each(function (i, record) {
            tr += '<tr  class="uk-text-center">';
            tr += '<td>' + (i + 1);
            tr += '</td>';
            tr += '<td class="PackSize">' + record.PackSize;
            tr += '</td>';
            tr += '<td class="Unit">' + record.Unit;
            tr += '<input type="hidden" class="UnitID included" value="' + record.UnitID + '"  />'
            tr += '</td>';
            tr += '<td class="mask-sales2-currency Quantity">' + record.Quantity;
            tr += '</td>';
            tr += '</tr>';
        });
        $tr = $(tr);
        app.format($tr);
        $("#packing-detail-list tbody").html($tr);
    },

    set_amount_details: function () {
        var self = GoodsReceipt;
        var is_igst = self.is_igst();
        var GSTPercent;
        var GSTAmount;
        var FreightTaxAmt = clean($("#FreightAmount").val());
        var FreightTax = clean($("#FreightTax").val());
        var CustomerType = $("#CustomerCategoryID option:selected").text();
        var tr = "";
        var $tr;
        self.Receipt.AmountDetails = [];

        $(self.Items).each(function (i, record) {
            GSTPercent = record.IGSTPercentage
            GSTAmount = record.GSTAmount;
            CessPercent = record.CessPercentage
            CessAmount = record.CessAmount
            if (is_igst) {
                self.calculate_group_tax_details(GSTPercent, GSTAmount, "IGST");
            } else {
                self.calculate_group_tax_details(GSTPercent / 2, GSTAmount / 2, "SGST");
                self.calculate_group_tax_details(GSTPercent / 2, GSTAmount / 2, "CGST");
                self.calculate_group_tax_details(CessPercent, CessAmount, "Cess");
            }
            if (CustomerType == "ECOMMERCE") {
                if (is_igst) {
                    var index = self.search_tax_group(self.Receipt.AmountDetails, FreightTax, "IGST On FreightAmount");
                    if (index == -1) {

                        self.Receipt.AmountDetails.push({
                            Percentage: FreightTax,
                            Amount: FreightTaxAmt,
                            Particulars: "IGST On FreightAmount"
                        });
                    }
                }
                else {
                    var index = self.search_tax_group(self.Receipt.AmountDetails, (FreightTax / 2), "SGST On FreightAmount");
                    if (index == -1) {
                        self.Receipt.AmountDetails.push({
                            Percentage: FreightTax / 2,
                            Amount: FreightTaxAmt / 2,
                            Particulars: "SGST On FreightAmount"
                        });
                    }
                    var index = self.search_tax_group(self.Receipt.AmountDetails, (FreightTax / 2), "CGST On FreightAmount");
                    if (index == -1) {

                        self.Receipt.AmountDetails.push({
                            Percentage: FreightTax / 2,
                            Amount: FreightTaxAmt / 2,
                            Particulars: "CGST On FreightAmount"
                        });
                    }
                }
            }
        });
        //$.each(self.Receipt.AmountDetails, function (i, record) {
        //    tr += "<tr  class='uk-text-center'>";
        //    tr += "<td>" + (i + 1);
        //    tr += "</td>";
        //    tr += "<td>" + record.Particulars;
        //    tr += "</td>";
        //    tr += "<td><input type='text' class='md-input " + gridcurrencyclass + "' readonly value='" + record.Percentage + "' />";
        //    tr += "</td>";
        //    tr += "<td><input type='text' class='md-input" + gridcurrencyclass + "' readonly value='" + record.Amount + "' />";
        //    tr += "</td>";
        //    tr += "</tr>";
        //});
        //$tr = $(tr);
        //app.format($tr);
        //$("#sales-invoice-amount-details-list tbody").html($tr);
    },

    //add validated items to grid
    add_items_to_grid: function (scrolldown) {
        var self = GoodsReceipt;
        var tr = "";
        var title = "";
        var InSufficientStock = "";
        $(self.Items).each(function (i, item) {
            title = (typeof item.SalesOrderNo == "undefined" ? "" : 'Order No : ' + item.SalesOrderNo + '<br/>')
                + 'Order Quantity : ' + item.Qty + ' ' + item.UnitName + '<br/>'
                + 'Offer Quantity : ' + item.OfferQty + ' ' + item.UnitName + '<br/>'
                + 'Stock : ' + item.Stock + ' ' + item.UnitName;
            //InSufficientStock = item.InvoiceQty > item.Stock ? "insufficient-stock" : "";
            tr += '<tr class="included ' + InSufficientStock + ' ' + item.OfferQtyMet + ' ' + item.QtyMet + ' quantity-' + item.InvoiceQty + ' " data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
                + ' <td class="uk-text-center">' + (i + 1)
                + '     <input type="hidden" class="ItemID included" value="' + item.ItemID + '"  />'
                + '     <input type="hidden" class="Code included" value="' + item.Code + '"  />'
                + '     <input type="hidden" class="ItemName included" value="' + item.ItemName + '"  />'
                + '     <input type="hidden" class="PartsNumber included" value="' + item.PartsNumber + '"  />'
                + '     <input type="hidden" class="Remarks included" value="' + item.Remarks + '"  />'
                + '     <input type="hidden" class="Model included" value="' + item.Model + '"  />'
                + '     <input type="hidden" class="TransNo included" value="' + item.TransNo + '"  />'
                + '     <input type="hidden" class="SalesInvoiceID included" value="' + item.SalesInvoiceID + '"  />'
                + '     <input type="hidden" class="SalesInvoiceTransID included" value="' + item.SalesInvoiceTransID + '"  />'
                + '     <input type="hidden" class="SalesOrderID included" value="' + item.SalesOrderID + '"  />'
                + '     <input type="hidden" class="SalesOrderItemTransID included" value="' + item.SalesOrderItemTransID + '"  />'
                + '     <input type="hidden" class="CounterSalesID included" value="' + item.CounterSalesID + '"  />'
                + '     <input type="hidden" class="CounterSalesItemTransID included" value="' + item.CounterSalesItemTransID + '"  />'
                + '     <input type="hidden" class="IsGST included" value="' + item.IsGST + '"  />'
                + '     <input type="hidden" class="IsVat included" value="' + item.IsVat + '"  />'
                + '     <input type="hidden" class="CurrencyID included" value="' + item.CurrencyID + '"  />'
                + '     <input type="hidden" class="VATPercentage included" value="' + item.VATPercentage + '"  />'
                + '     <input type="hidden" class="BatchID included" value="' + item.BatchID + '"  />'
                + '     <input type="hidden" class="BatchTypeID included" value="' + item.BatchTypeID + '"  />'
                + '     <input type="hidden" class="BatchType included" value="' + item.BatchTypeName + '"  />'
                + '     <input type="hidden" class="BatchName included" value="' + item.BatchName + '"  />'
                + '     <input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
                + '     <input type="hidden" class="IGSTPercentage included" value="' + item.IGSTPercentage + '" />'
                + '     <input type="hidden" class="SGSTPercentage included" value="' + item.SGSTPercentage + '" />'
                + '     <input type="hidden" class="CGSTPercentage included" value="' + item.CGSTPercentage + '" />'
                + '     <input type="hidden" class="StoreID included" value="' + item.StoreID + '" />'
                + '     <input type="hidden" class="OfferQty included" value="' + item.OfferQty + '" />'
                + '     <input type="hidden" class="Stock included" value="' + item.Stock + '" />'
                + '     <input type="hidden" class="PackSize" value="' + item.PackSize + '" />'
                + '     <input type="hidden" class="PrimaryUnit" value="' + item.PrimaryUnit + '" />'
                + '     <input type="hidden" class="CategoryID" value="' + item.CategoryID + '" />'
                + ' </td>'
                + ' <td class="uk-text-small">' + item.Code + '</td>'
                + ' <td class="uk-text-small">' + item.ItemName + '</td>'
                + ' <td class="uk-text-small">' + item.PartsNumber + '</td>'
                + ' <td class="uk-text-small">' + item.Remarks + '</td>'
                + ' <td class="uk-text-small">' + item.Model + '</td>'
                + ' <td class="uk-hidden UnitName">' + item.UnitName + '</td>'
                + ' <td class="SecondaryUnit">' + item.SecondaryUnit + '</td>'
                //+ ' <td class="BatchType">' + item.BatchTypeName + '</td>'
                //+ ' <td>' + item.BatchName + '</td>'
                + ' <td class="uk-hidden"><input type="text" class="Qty included md-input mask-sales2-currency" value="' + item.InvoiceQty + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="SecondaryQty included md-input mask-sales2-currency" value="' + item.SecondaryQty + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="OfferQty included md-input mask-sales2-currency" value="' + item.InvoiceOfferQty + '" readonly="readonly" /></td>'
                + ' <td class="uk-hidden mrp_hidden" ><input type="text" class="MRP included md-input ' + gridcurrencyclass + '" value="' + item.MRP + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="SecondaryMRP included md-input ' + gridcurrencyclass + '" value="' + item.SecondaryMRP + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="BasicPrice included md-input ' + gridcurrencyclass + ' " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="GrossAmount included md-input ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="AdditionalDiscount included md-input ' + gridcurrencyclass + '" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="DiscountPercentage included md-input ' + gridcurrencyclass + '" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="DiscountAmount included md-input ' + gridcurrencyclass + '" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="TaxableAmount included md-input ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                //+ ' <td ><input type="text" class="GST included md-input ' + gridcurrencyclass + '" value="' + item.IGSTPercentage + '" readonly="readonly" /></td>'
                //+ ' <td ><input type="text" class="GSTAmount included md-input '+gridcurrencyclass+'" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                //+ ' <td class="cess-enabled"><input type="text" class="CessPercentage ' + gridcurrencyclass + '" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                //+ ' <td class="cess-enabled"><input type="text" class="CessAmount '+gridcurrencyclass+'" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="NetAmount included md-input ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + ' <td class="uk-text-center action">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        });
        var $tr = $(tr);
        app.format($tr);
        $("#goods-receipt-items-list tbody").html($tr);
        $("#item-count").val($("#goods-receipt-items-list tbody tr").length);
        //fh_items.resizeHeader(scrolldown);
    },

    set_form_values: function () {
        var self = GoodsReceipt;
        $("#GrossAmount").val(self.Receipt.GrossAmount);
        $("#DiscountAmount").val(self.Receipt.DiscountAmount);
        $("#AdditionalDiscount").val(self.Receipt.AdditionalDiscount);
        $("#TaxableAmount").val(self.Receipt.TaxableAmount);
        $("#SGSTAmount").val(self.Receipt.SGSTAmount);
        $("#CGSTAmount").val(self.Receipt.CGSTAmount);
        $("#IGSTAmount").val(self.Receipt.IGSTAmount);
        $("#RoundOff").val(self.Receipt.RoundOff);
        $("#NetAmount").val(self.Receipt.NetAmount);
    },

    on_save: function () {

        var self = GoodsReceipt;
        var location = "/Sales/GoodsReceiptNote/Index";
        var url = '/Sales/GoodsReceiptNote/Save';

        if ($(this).hasClass("btnSaveASDraft")) {
            self.Receipt.IsDraft = true;
            url = '/Sales/GoodsReceiptNote/SaveAsDraft'
            self.error_count = self.validate_form_on_draft();
        }
        else {
            self.error_count = self.validate_form();
        }

        if (self.error_count > 0) {
            return;
        }

        if (!self.Receipt.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(url, location);
            }, function () {
            })
        } else {
            self.save(url, location);
        }
    },

    save: function (url, location) {
        var self = GoodsReceipt;
        self.error_count = 0;
        self.Receipt.BillingAddressID = $("#BillingAddressID").val();
        self.Receipt.ShippingAddressID = $("#ShippingAddressID").val();
        self.Receipt.TransDate = $("#TransDate").val();
        self.Receipt.Remarks = $("#Remarks").val();
        self.Receipt.CustomerID = $("#CustomerID").val();
        self.Receipt.Items = self.Items;
        self.Receipt.PackingDetails = self.Receipt.PackingDetails;

        $(".btnSaveASDraft, .btnSave, .cancel").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: self.Receipt,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var msg;
                    if (self.Receipt.ID == "") {
                        msg = "Proforma Invoice Saved Successfully";
                    } else {
                        msg = "Proforma Invoice Updated Successfully";
                    }
                    if (self.Receipt.IsDraft) {
                        msg = "Proforma Invoice Saved as draft Successfully";
                    }

                    app.show_notice("Proforma Invoice Saved Successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSave, .cancel").css({ 'display': 'block' });
                }
            }
        });
    },
    calculate_group_packing_details: function (PackSize, Quantity, UnitName, UnitID, Pack) {
        var self = GoodsReceipt;
        if (Quantity == 0) {
            return;
        }
        var index = self.search_pack_size(self.Receipt.PackingDetails, PackSize, UnitName, UnitID);
        if (index == -1) {
            self.Receipt.PackingDetails.push({
                PackSize: PackSize,
                Quantity: Quantity,
                Unit: UnitName,
                UnitID: UnitID,
                Pack: Pack

            });
        } else {
            self.Receipt.PackingDetails[index].Quantity += Quantity;
        }
    },

    calculate_group_tax_details: function (GSTPercent, GSTAmount, type) {
        var self = GoodsReceipt;
        if (GSTAmount == 0) {
            return;
        }
        var index = self.search_tax_group(self.Receipt.AmountDetails, GSTPercent, type);
        if (index == -1) {
            self.Receipt.AmountDetails.push({
                Percentage: GSTPercent,
                Amount: GSTAmount,
                Particulars: type
            });
        } else {
            self.Receipt.AmountDetails[index].Amount += GSTAmount;
        }
    },
    //gets offer quantity from the SalesOrder.item_offer_details
    get_offer_qty: function (qty, item_id) {
        var self = GoodsReceipt;
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

    is_igst: function () {
        //if ($("#StateID").val() != $("#LocationStateID").val()) {
        //    return true;
        //}
        return false;
    },

    include_item: function () {
        var self = GoodsReceipt;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('disabled');
    },
    // Clears the item details after adding it to grid 
    clear_item: function () {
        var self = GoodsReceipt;
        $("#ItemID").val('');
        $("#Unit").val('');
        $("#UnitID").val('');
        $("#Stock").val('');
        $("#CGSTPercentage").val('');
        $("#IGSTPercentage").val('');
        $("#SGSTPercentage").val('');
        $("#DiscountPercentage").val('');
        $("#Qty").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);

    },
    // checks whether the customer id is set 
    validate_customer: function () {
        var self = GoodsReceipt;
        if (self.rules.on_select_item.length > 0) {
            return form.validate(self.rules.on_select_item);
        }
        return 0;
    },

    validate_item: function () {
        var self = GoodsReceipt;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    validate_form: function () {
        var self = GoodsReceipt;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_form_on_draft: function () {
        var self = GoodsReceipt;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_stock: function () {
        var self = GoodsReceipt;
        if (self.rules.stock.length > 0) {
            return form.validate(self.rules.stock);
        }
        return 0;
    },
    // Validation rules
    rules: {
        on_select_item: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose a customer" },
                    { type: form.non_zero, message: "Please choose a customer" },
                ]
            },
        ],
        on_add_item: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                    {
                        type: function (element) {
                            var error = false;

                            $("#sales-invoice-items-list tbody tr").each(function () {
                                if ($(this).find(".ItemID").val() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already exists, try editing quantity"
                    },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                ]
            }
        ],
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
                    //{
                    //    type: function (element) {
                    //        return ($(".offer-qty-not-met").length == 0);
                    //    },
                    //    message: "Invalid Qty"
                    //}
                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
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
                    },
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

    list: function () {
        var self = GoodsReceipt;
        $('#tabs-goods-receipt').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "Draft":
                $list = $('#goods-receipt-draft-list');
                break;
            case "GoodsReceipt":
                $list = $('#goods-receipt-list');
                break;
            case "Cancelled":
                $list = $('#goods-receipt-cancelled');
                break;
            default:
                $list = $('#goods-receipt-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            if (list_table)
                list_table.fnDestroy();
            list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/GoodsReceiptNote/GetGoodReceiptNoteList?type=" + type,
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
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "ReceiptDate", "className": "ReceiptDate" },
                    { "data": "CustomerName", "className": "CustomerName" },
                   /* { "data": "Status", "searchable": false, },*/
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/GoodsReceiptNote/Details/" + Id);
                    });
                },
            });
            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

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
    search_pack_size: function (array, Packsize, UnitName, UnitID) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].PackSize == Packsize && array[i].UnitID == UnitID) {
                return i;
            }
        }
        return -1;
    },
    show_batch_edit_modal: function () {
        var self = GoodsReceipt;
        var tr = "";
        var $tr;
        var OrderQty = 0;
        var TotalInvoiceQty = 0;
        var TotalInvoiceOfferQty = 0;
        var TotalStock = 0;
        var row;

        var SalesOrderTransID = -1;
        var SalesOrderTransIDs = [];

        var ItemID = $(this).closest('tr').find('.ItemID').val();
        var CategoryID = $(this).closest('tr').find('.CategoryID').val();
        var item = self.search_in_items(parseInt(ItemID), 0);
        var UnitID = $(this).closest('tr').find('.UnitID').val();
        var BatchType = $(this).closest('tr').find('.BatchType').text();
        $("#sales-invoice-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr").each(function () {
            row = $(this).closest('tr');
            if (SalesOrderTransID != $(row).find(".OrderTransID").val()) {
                OrderQty += clean($(row).find(".OrderQty").val());
                SalesOrderTransID = $(row).find(".OrderTransID").val();
                if (SalesOrderTransID == undefined || SalesOrderTransID == "undefined") {
                    SalesOrderTransIDs.push(0);
                } else {
                    SalesOrderTransIDs.push($(row).find(".OrderTransID").val());
                }
            }
        });

        $("#BatchItemID").val(item.ItemID);
        $("#BatchItemName").val(item.ItemName);
        $("#BatchItemCode").val(item.Code);
        $("#BatchQty").val(OrderQty);
        $("#BatchMaxQty").val(OrderQty);
        $("#BatchType").val(BatchType);
        $("#BatchOfferQty").val(self.get_offer_qty(OrderQty, item.ItemID, item.UnitID));
        $("#SalesOrderTransIDs").val(SalesOrderTransIDs.toString());

        $.ajax({
            url: '/Masters/Batch/GetAvailableBatches',
            dataType: "json",
            data: {
                ItemID: ItemID,
                OrderQty: OrderQty,
                SalesOrderTransIDs: SalesOrderTransIDs.length == 0 ? [] : SalesOrderTransIDs,
                WarehouseID: $('#StoreID').val(),
                CustomerID: $("#CustomerID").val(),
                SchemeID: $('#SchemeID').val(),
                UnitID: UnitID,
                ProformaInvoiceID: $("#ID").val()

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
                            + '<td class="Stock mask-sales2-currency">' + record.Stock + '</td>'
                            + '<td class="' + gridcurrencyclass + '">' + record.Rate + '</td>'
                            + '<td>' + record.ExpiryDateStr + '</td>'
                            + '<td><input type="text" class="md-input mask-sales2-currency InvoiceQty" value = "' + (record.InvoiceQty - record.InvoiceOfferQty) + '" /></td>'
                            + '<td><input type="text" class="md-input mask-sales2-currency InvoiceOfferQty" value ="' + record.InvoiceOfferQty + '" /></td>'
                            + '<tr>';
                    });
                    tr += '<tr>'
                        + '<td colspan="2"><b>Total</b></td>'
                        + '<td class="mask-sales2-currency BatchStock">' + TotalStock + '</td>'
                        + '<td colspan="2"></td>'
                        + '<td><input type="text" class="md-input mask-sales2-currency TotalInvoiceQty" disabled value = "' + TotalInvoiceQty + '" /></td>'
                        + '<td><input type="text" class="md-input mask-sales2-currency TotalInvoiceOfferQty" disabled value ="' + TotalInvoiceOfferQty + '" /></td>'
                        + '<tr>';
                    $tr = $(tr);
                    app.format($tr);
                    $('#batch-list tbody').html($tr);
                    $('#show-batch-edit').trigger('click');
                }
            }
        });

    },

    replace_batches: function () {
        var self = GoodsReceipt;

        var InvoiceQty;
        var InvoiceOfferQty;
        var item;

        if (self.validate_stock() > 0) {
            return;
        }
        UIkit.modal("#batch-edit").hide();
        var ItemID = $('#BatchItemID').val();
        var BatchTypeName = $('#BatchType').val();
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
                        Stock: clean($(this).find('.Stock').text().trim()),
                        SalesOrderItemID: clean($(this).find('.OrderTransID').val()) == 0 ? OrderTransID : clean($(this).find('.OrderTransID').val()),
                        Qty: clean($(this).find('.OrderQty').val()) == 0 ? OrderQty : clean($(this).find('.OrderQty').val()),
                        InvoiceOfferQtyMet: true,
                        InvoiceQtyMet: true,
                        OfferQtyMet: "",
                        QtyMet: "",
                        Rate: clean($(this).find('.Rate').val()),
                        BatchTypeName: BatchTypeName,
                    });

                self.Items.push(item);

            }
        });

        self.Items.sort(function (a, b) {
            return ((a.SalesOrderItemID < b.SalesOrderItemID) ? -1 : ((a.SalesOrderItemID > b.SalesOrderItemID) ? 1 : 0));
        });

        self.process_invoice(self.Items, false);
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
        var self = GoodsReceipt;
        if (BatchID == 0) {
            return self.Items.find(item => { return item.ItemID == ItemID });
        } else {
            return self.Items.find(item => { return item.ItemID == ItemID && item.BatchID == BatchID });
        }
    },

    set_offer_qty: function () {
        var self = GoodsReceipt;
        var qty = clean($('#BatchQty').val());
        var item_id = $("#BatchItemID").val();
        var offer_qty = self.get_offer_qty(qty, item_id);
        $("#BatchOfferQty").val(offer_qty);
    }
}
Number.prototype.roundToCustom = function () {
    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};