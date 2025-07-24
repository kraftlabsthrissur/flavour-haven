var gridcurrencyclass = 'mask-sales2-currency';
var DecPlaces = 0;
$(function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
    //alert(gridcurrencyclass);
});
SalesInvoice.init = function () {
    var self = SalesInvoice;

    self.customer_list("sales-invoice");
    $('#customer-list').SelectTable({
        selectFunction: self.select_customer,
        returnFocus: "#SalesOrderNos",
        modal: "#select-customer",
        initiatingElement: "#CustomerName"
    });

    self.sales_order_list();
    self.proforma_invoice_list();

    self.purchase_order_history();
    self.pending_po_history();
    self.purchase_legacy_history();
    self.sales_order_history('Quotation');
    self.sales_invoice_history();

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
    $('#tabs-sales-invoice-create').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            active_item.data('tab-loaded', true);
        }
    });
};
SalesInvoice.select_customer = function () {
    var self = SalesInvoice;
    var radio = $('#select-customer tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var CurrencyCode = $(row).find(".CurrencyCode").val();
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
    var CurrencyID = $(row).find(".CurrencyID").val();
    var CurrencyName = $(row).find(".CurrencyName").text().trim();
    var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
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
    $("#CurrencyID").val(CurrencyID);
    $("#CurrencyName").val(CurrencyName);
    $("#CurrencyCode").val(CurrencyCode);
    $("#CurrencyExchangeRate").val(CurrencyConversionRate);
    UIkit.modal($('#select-customer')).hide();
    self.Invoice.CustomerID = ID;
    self.customer_on_change();
    self.on_change_customer_category();
};
SalesInvoice.clear_items = function () {
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
    $("#VATAmount").val('');
    $("#CashDiscount").val('');
    $("#RoundOff").val('');
    $("#NetAmount").val('');
    $("#NetAmountPrefix").val('');
    $("#CessAmount").val('');
    self.Items = [];
    self.Invoice.GrossAmount = 0;
    self.Invoice.AdditionalDiscount = 0;
    self.Invoice.DiscountAmount = 0;
    self.Invoice.DiscountPercentage = 0;
    self.Invoice.TurnoverDiscount = 0;
    self.Invoice.TaxableAmount = 0;
    self.Invoice.CGSTAmount = 0;
    self.Invoice.SGSTAmount = 0;
    self.Invoice.IGSTAmount = 0;
    self.Invoice.VATAmount = 0;
    self.Invoice.CashDiscount = 0;
    self.Invoice.NetAmount = 0;
    self.Invoice.RoundOff = 0;
    self.Invoice.CessAmount = 0;

};
SalesInvoice.get_item_properties = function (item) {
    var self = SalesInvoice;
    var IsVATExtra = $("#IsVATExtra").val();
    item.VATPercentage = $("#VATPercentageID option:selected").text();
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
    item.CurrencyID = $("#CurrencyID").val();
    if (item.IsGST == 1) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        }
        else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        if (CustomerCategory == "Export") {
            item.BasicPrice = item.MRP;
        }
    }
    else if (item.IsVat == 1) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.VATPercentage + item.CessPercentage);
        }
        else {
            item.BasicPrice = item.MRP * 100 / (100 + item.VATPercentage);
        }
    }
    ////added by anju
    if (IsVATExtra == 1) {
        item.BasicPrice = item.MRP;
    }
    //alert(item.InvoiceQty);
    item.BasicPrice = item.BasicPrice;
    item.GrossAmount = (item.BasicPrice * (item.InvoiceQty));
    item.AdditionalDiscount = (item.BasicPrice * item.InvoiceOfferQty);
    //item.DiscountAmount = ((item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100);
    item.DiscountAmount = item.DiscountAmount;
    item.TurnoverDiscount = 0;
    item.TaxableAmount = 0;
    item.GSTAmount = 0;
    item.VATAmount = 0;
    item.CashDiscount = 0;
    item.NetAmount = 0;
    item.IsIncluded = true;
    return item;
};
SalesInvoice.purchase_order_history = function () {

    var $list = $('#purchase-history-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            if (title !== undefined && title.length > 0) {
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            }
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
};
SalesInvoice.pending_po_history = function () {

    var $list = $('#pending-po-history-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            if (title !== undefined && title.length > 0) {
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            }
        });
        altair_md.inputs();
        var url = "/Sales/SalesInvoice/GetPendingPOHistory";

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
                {
                    "data": "QtyMet", "className": "",
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.QtyMet + "</div>";
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
};
SalesInvoice.purchase_legacy_history = function () {

    var $list = $('#purchase-legacy-history-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            if (title !== undefined && title.length > 0) {
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            }
        });
        altair_md.inputs();
        var url = "/Sales/SalesInvoice/GetLegacyPurchaseHistory";

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
                { "data": "ReferenceNo", "className": "" },
                { "data": "ItemCode", "className": "" },
                { "data": "ItemName", "className": "" },
                { "data": "SupplierName", "className": "" },
                { "data": "PartsNumber", "className": "" },
                { "data": "OrderDate", "className": "" },
                {
                    "data": "Quantity", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + gridcurrencyclass + "' >" + row.Quantity + "</div>";
                    }
                },
                {
                    "data": "Rate", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.Rate + "</div>";
                    }
                },
                {
                    "data": "GrossAmount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.GrossAmount + "</div>";
                    }
                },
                {
                    "data": "Discount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.Discount + "</div>";
                    }
                },
                {
                    "data": "TaxAmount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + gridcurrencyclass + "' >" + row.TaxAmount + "</div>";
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
};
SalesInvoice.sales_order_history = function (type) {

    var $list = $('#sales-order-history-list');
    if (type == 'Quotation') {
        $list = $('#sales-quotation-history-list');
    }
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            if (title !== undefined && title.length > 0) {
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            }
        });
        altair_md.inputs();
        var url = "/Sales/SalesOrder/GetSalesOrderHistory";

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
                        return meta.settings.oAjaxData.start + meta.row + 1;
                    }
                },
                { "data": "SalesOrderNo", "className": "" },
                { "data": "OrderDate", "className": "" },
                { "data": "CustomerName", "className": "" },
                { "data": "Itemcode", "className": "", "searchable": false, "orderable": false, },
                { "data": "ItemName", "className": "" },
                { "data": "PartsNumber", "className": "" },
                {
                    "data": "SecondaryMRP", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + gridcurrencyclass + "' >" + row.SecondaryMRP + "</div>";
                    }
                },
                {
                    "data": "SecondaryQty", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.SecondaryQty + "</div>";
                    }
                },
                { "data": "SecondaryUnit", "className": "" },
                {
                    "data": "DiscountPercentage", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                    }
                },
                {
                    "data": "VATPercentage", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.VATPercentage + "</div>";
                    }
                },
                {
                    "data": "TaxableAmount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + gridcurrencyclass + "' >" + row.TaxableAmount + "</div>";
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
};
SalesInvoice.set_item_discount = function () {

    var self = SalesInvoice;
    var elementclass = $(this).attr('class');
    var $row = $(this).closest("tr");
    var className = '';
    if (elementclass.indexOf('DiscountAmount') != -1) {
        className = 'DiscountAmount';
        var GrossAmount = clean($row.find(".GrossAmount").val());
        var DiscountAmount = clean($row.find(".DiscountAmount").val());
        // var DiscountPercentage = DiscountPercentage / GrossAmount * 100;
       
        var DiscountPercentage = DiscountAmount / GrossAmount * 100;
        //  $row.find(".DiscountPercentage").val(DiscountPercentage.roundToCustom());
        $row.find(".DiscountPercentage").val(DiscountPercentage.roundToCustom());

    } else if (elementclass.indexOf('DiscountPercentage') != -1) {
        className = 'DiscountPercentage';
        var GrossAmount = clean($row.find(".GrossAmount").val());
        var DiscountPercentage = clean($row.find(".DiscountPercentage").val());
        var DiscountAmount = GrossAmount * DiscountPercentage / 100;
        $row.find(".DiscountAmount").val(DiscountAmount.roundToCustom());
    }
    self.set_grid_total_calculations('', className);
};
SalesInvoice.set_total_discount = function () {
    var self = SalesInvoice;
    var elementid = $(this).attr('id');
    self.set_grid_total_calculations(elementid, '');
};
SalesInvoice.get_grid_item_values = function ($record) {
    return item = {
        ItemName: $record.find(".ItemName").val(),
        SGSTPercentage: clean($record.find(".SGSTPercentage").val()),
        CGSTPercentage: clean($record.find(".CGSTPercentage").val()),
        CessPercentage: clean($record.find(".CessPercentage").val()),
        IGSTPercentage: clean($record.find(".IGSTPercentage").val()),
        VATPercentage: clean($record.find(".VATPercentage").val()),
        ItemID: $record.find(".ItemID").val(),
        UnitName: $record.find(".UnitName").text().trim(),
        Code: $record.find(".Code").val(),
        UnitID: $record.find(".UnitID").val(),
        CurrencyID: $record.find(".CurrencyID").val(),
        IsVat: clean($record.find(".IsVat").val()),
        IsGST: clean($record.find(".IsGST").val()),
        PartsNumber: $record.find(".PartsNumber").val(),
        DeliveryTerm: $record.find(".DeliveryTerm").val(),
        Model: $record.find(".Model").val(),
        Qty: clean($record.find(".OrderQty").val()),
        OfferQty: clean($record.find(".OfferQty").val()),
        StoreID: clean($("#StoreID").val()),
        SalesOrderItemID: clean($record.find(".SalesOrderTransID").val()),
        ProformaInvoiceTransID: clean($record.find(".ProformaInvoiceTransID").val()),
        BatchID: clean($record.find(".BatchID").val()),
        BatchName: $record.find(".BatchName").text(),
        BatchTypeID: clean($record.find(".BatchTypeID").val()),
        BatchTypeName: $record.find(".BatchType").text(),
        MRP: clean($record.find(".MRP").val()),
        Stock: clean($record.find(".Stock").val()),
        InvoiceQty: clean($record.find(".Qty").val()),
        InvoiceOfferQty: clean($record.find(".InvoiceOfferQty").val()),
        IsIncluded: true,
        InvoiceOfferQtyMet: $record.hasClass("offer-qty-not-met") ? false : true,
        InvoiceQtyMet: $record.hasClass("qty-not-met") ? false : true,
        SalesUnitID: clean($record.find(".SalesUnitID").val()),
        Rate: clean($record.find(".Rate").val()),
        LooseRate: clean($record.find(".LooseRate").val()),
        CessAmount: clean($record.find(".CessAmount").val()),
        CessPercentage: clean($record.find(".CessPercentage").val()),
        BatchTypeName: $record.find(".BatchType").text(),
        PackSize: clean($record.find(".PackSize").val()),
        PrimaryUnit: $record.find(".PrimaryUnit").val(),
        CategoryID: $record.find(".CategoryID").val(),
    }
}
SalesInvoice.set_grid_total_calculations = function (elementid, elementclass) {
    var self = SalesInvoice;
    self.Invoice.GrossAmount = clean($("#GrossAmount").val());
    self.Invoice.OtherCharges = clean($("#OtherCharges").val());
    self.Invoice.VATPercentage = clean($("#VATPercentageID option:selected").text());
    self.Invoice.VATPercentageID = clean($("#VATPercentageID").val());
    self.Invoice.OtherChargesVATAmount = self.Invoice.OtherCharges * self.Invoice.VATPercentage / 100;
    var discountperitem = 0;
    if (elementid == 'DiscountPercentage') {
        self.Invoice.DiscountPercentage = clean($("#DiscountPercentage").val());
        self.Invoice.DiscountAmount = (self.Invoice.GrossAmount * self.Invoice.DiscountPercentage / 100);
        discountperitem = ((self.Invoice.DiscountAmount * 100) / self.Invoice.GrossAmount);
    } else if (elementid == 'DiscountAmount') {
        self.Invoice.DiscountAmount = clean($("#DiscountAmount").val());
        self.Invoice.DiscountPercentage = (self.Invoice.DiscountAmount / self.Invoice.GrossAmount * 100);
        discountperitem = ((self.Invoice.DiscountAmount * 100) / self.Invoice.GrossAmount);
    } else {
        self.Invoice.DiscountPercentage = clean($("#DiscountPercentage").val());
        self.Invoice.DiscountAmount = clean($("#DiscountAmount").val());
        discountperitem = ((self.Invoice.DiscountAmount * 100) / self.Invoice.GrossAmount);
    }
    self.Invoice.GrossAmount = 0;
    self.Invoice.DiscountAmount = 0;
    self.Invoice.DiscountPercentage = 0;
    self.Invoice.GSTAmount = 0;
    self.Invoice.VATAmount = 0;
    //self.Invoice.TurnoverDiscount = clean($("#TurnoverDiscount").val());
    self.Invoice.AdditionalDiscount = 0;
    self.Invoice.TaxableAmount = 0;
    self.Invoice.NetAmount = 0;
    self.Invoice.RoundOff = 0;
    self.Invoice.CashDiscount = 0;
    self.Invoice.CessAmount = 0;
    var temp = 0;
    $.each(self.Items, function (index, item) {
        var $row;
        $("#sales-invoice-items-list tbody tr").each(function () {
            var idx = clean($(this).find("td.index").text().trim());
            if (idx == item.index) {
                $row = $(this);
            }
        });
        item.VATPercentage = clean($("#VATPercentageID option:selected").text());
        item.DiscountAmount = 0;
        item.DiscountPercentage = 0;
        item.GrossAmount = clean($row.find(".GrossAmount").val());
        self.Invoice.GrossAmount += item.GrossAmount;

        if (discountperitem >= 0) {
            item.DiscountAmount = (item.GrossAmount * (discountperitem / 100));
            self.Invoice.DiscountAmount += item.DiscountAmount;
            item.DiscountPercentage = (item.DiscountAmount / item.GrossAmount * 100);
        } else {
            if (elementclass == 'DiscountPercentage') {
                item.DiscountPercentage = clean($row.find(".DiscountPercentage").val());
                item.DiscountAmount = item.GrossAmount * item.DiscountPercentage / 100;
            } else if (elementclass == 'DiscountAmount') {
                item.DiscountAmount = clean($row.find(".DiscountAmount").val());
                item.DiscountPercentage = item.DiscountAmount / item.GrossAmount * 100;
            }
            else {
                item.DiscountAmount = clean($row.find(".DiscountAmount").val());
                item.DiscountPercentage = item.DiscountAmount / item.GrossAmount * 100;
            }
            self.Invoice.DiscountAmount += item.DiscountAmount;
        }
        item.TaxableAmount = item.GrossAmount - item.DiscountAmount;
        self.Invoice.TaxableAmount += item.TaxableAmount;
        item.IGSTAmount = item.TaxableAmount * item.IGSTPercentage / 100;
        self.Invoice.IGSTAmount += item.IGSTAmount;
        item.SGSTAmount = item.TaxableAmount * item.SGSTPercentage / 100;
        self.Invoice.SGSTAmount += item.SGSTAmount;
        item.CGSTAmount = item.TaxableAmount * item.CGSTPercentage / 100;
        self.Invoice.CGSTAmount += item.CGSTAmount;
        item.VATAmount = item.TaxableAmount * item.VATPercentage / 100;
        self.Invoice.VATAmount += item.VATAmount;
        item.CessAmount = item.TaxableAmount * item.CessPercentage / 100;
        self.Invoice.CessAmount += item.CessAmount;
        item.NetAmount = item.TaxableAmount + item.CessAmount + item.CGSTAmount + item.SGSTAmount + item.VATAmount;
        self.Invoice.NetAmount += item.NetAmount;
        $row.find(".TaxableAmount").val(item.TaxableAmount.roundToCustom());
        $row.find(".CessAmount").val(item.CessAmount.roundToCustom());
        $row.find(".CGST").val(item.CGSTAmount.roundToCustom());
        $row.find(".SGST").val(item.SGSTAmount.roundToCustom());
        $row.find(".IGST").val(item.IGSTAmount.roundToCustom());
        $row.find(".VATAmount").val(item.VATAmount.roundToCustom());
        $row.find(".NetAmount").val(item.NetAmount.roundToCustom());
        if (elementclass == 'DiscountPercentage') {
            $row.find(".DiscountAmount").val(item.DiscountAmount.roundToCustom());
        } else if (elementclass == 'DiscountAmount') {
            $row.find(".DiscountPercentage").val(item.DiscountPercentage.roundToCustom());
        } else {
            $row.find(".DiscountAmount").val(item.DiscountAmount.roundToCustom());
            $row.find(".DiscountPercentage").val(item.DiscountPercentage.roundToCustom());
        }
        self.Items[index] = item;
    });
    temp = self.Invoice.NetAmount;
    self.Invoice.NetAmount = parseFloat(self.Invoice.NetAmount);
    self.Invoice.RoundOff = temp - self.Invoice.NetAmount;
    if (elementid == 'DiscountPercentage') {
        $("#DiscountAmount").val(self.Invoice.DiscountAmount.roundToCustom());
    } else if (elementid == 'DiscountAmount') {
        self.Invoice.DiscountPercentage = (self.Invoice.DiscountAmount / self.Invoice.GrossAmount * 100);
        $("#DiscountPercentage").val(self.Invoice.DiscountPercentage.roundToCustom());
    } else {
        $("#DiscountAmount").val(self.Invoice.DiscountAmount.roundToCustom());
        self.Invoice.DiscountPercentage = (self.Invoice.DiscountAmount / self.Invoice.GrossAmount * 100);
        $("#DiscountPercentage").val(self.Invoice.DiscountPercentage.roundToCustom());
    }
    $("#GrossAmount").val(self.Invoice.GrossAmount.roundToCustom());
    $("#TaxableAmount").val(self.Invoice.TaxableAmount.roundToCustom());
    $("#SGSTAmount").val(self.Invoice.SGSTAmount.roundToCustom());
    $("#CGSTAmount").val(self.Invoice.CGSTAmount.roundToCustom());
    $("#IGSTAmount").val(self.Invoice.IGSTAmount.roundToCustom());
    $("#VATAmount").val(self.Invoice.VATAmount.roundToCustom());
    $("#CessAmount").val(self.Invoice.CessAmount.roundToCustom());
    $("#RoundOff").val(self.Invoice.RoundOff.roundToCustom());
    $("#OtherChargesVATAmount").val(self.Invoice.OtherChargesVATAmount.roundToCustom());
    self.Invoice.NetAmount += (self.Invoice.OtherCharges + self.Invoice.OtherChargesVATAmount);
    $("#NetAmountPrefix").val(self.Invoice.NetAmount.roundToCustom());
    $("#NetAmount").val(self.Invoice.NetAmount.roundToCustom());

};
SalesInvoice.add_items_to_grid = function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
    var self = SalesInvoice;
    var index = 0;
    var tr = '';
    var title;
    var row_class = "";
    $(self.Items).each(function (i, item) {
        //console.log(JSON.stringify(item));
        index = item.index;
        if (item.IsGST == 1) {
            title = (typeof item.SalesOrderNo == "undefined" ? "" : 'Order No : ' + item.SalesOrderNo + '<br/>')
                + 'Order Quantity : ' + item.Qty + '<br/>'
                + 'Offer Quantity : ' + item.OfferQty + '<br/>'
                + 'Stock : ' + item.Stock;
            row_class = item.SalesOrderItemID == 0 ? "proforma-invoice" : "sales-order"
            tr += '<tr class="included ' + row_class + ' ' + item.OfferQtyMet + ' ' + item.QtyMet + ' quantity-' + item.InvoiceQty + ' " data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
                + '<td class="uk-text-center showitemhistory action">'
                + '     <a class="view-itemhistory">'
                + '         <i class="uk-icon-eye-slash"></i>'
                + '     </a>'
                + '</td>'
                + '<td class="uk-text-center index">' + item.index
                + '<input type="hidden" class="ItemID included" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '<input type="hidden" class="Code" value="' + item.Code + '"  />'
                + '<input type="hidden" class="ItemName" value="' + item.ItemName + '"  />'
                + '<input type="hidden" class="PartsNumber" value="' + item.PartsNumber + '"  />'
                + '<input type="hidden" class="DeliveryTerm" value="' + item.DeliveryTerm + '"  />'
                + '<input type="hidden" class="Model" value="' + item.Model + '"  />'
                + '<input type="hidden" class="IsVAT" value="' + item.IsVat + '"  />'
                + '<input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
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
                + '<input type="hidden" class="PrintWithItemCode" value="' + item.PrintWithItemCode + '" />'
                + '<input type="hidden" class="SecondaryUnitSize" value="' + item.SecondaryUnitSize + '" />'
                + '</td>'
                + '<td class="uk-text-small">' + item.Code + '</td>'
                + '<td class="uk-text-small">' + item.ItemName + '</td>'
                + '<td class="uk-text-small">' + item.PartsNumber + '</td>'
                + '<td class="uk-text-small">' + item.DeliveryTerm + '</td>'
                + '<td class="uk-text-small">' + item.Model + '</td>'
                + '<td class="uk-hidden">' + item.UnitName + '</td>'
                + '<td>' + item.SecondaryUnit + '</td>'
                //+'<td>' + item.BatchTypeName + '</td>'
                //+' <td>' + item.BatchName + '</td>'
                + '<td class="uk-hidden"><input type="text" class="Qty included md-input mask-sale2-currency" value="' + item.InvoiceQty + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="SecondaryQty included md-input mask-sale2-currency" value="' + item.SecondaryInvoiceQty + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden"><input type="text" class="InvoiceOfferQty included md-input mask-sale2-currency" value="' + item.InvoiceOfferQty + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden"><input type="text" class="SecondaryInvoiceOfferQty included md-input mask-sale2-currency" value="' + item.SecondaryInvoiceOfferQty + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="CurrencyName" value="' + item.CurrencyName + '" readonly="readonly" /></td>'
                + '<td class="mrp_hidden uk-hidden" ><input type="text" class="MRP included md-input ' + gridcurrencyclass + '" value="' + item.MRP + '" readonly="readonly" /></td>'
                + '<td class="mrp_hidden" ><input type="text" class="SecondaryMRP included md-input ' + gridcurrencyclass + '" value="' + item.SecondaryMRP + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="BasicPrice included md-input ' + gridcurrencyclass + '" value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GrossAmount included md-input ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                //+' <td><input type="text" class="AdditionalDiscount included md-input ' + gridcurrencyclass +'" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
                + '<td class="action"><input type="text" class="DiscountPercentage included md-input mask-sale2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + '<td class="action"><input type="text" class="DiscountAmount included md-input ' + gridcurrencyclass + '" value="' + item.DiscountAmount + '" /></td>'
                //+' <td><input type="text" class="TurnoverDiscount included md-input ' + gridcurrencyclass +'" value="' + item.TurnoverDiscount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="TaxableAmount included md-input ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GST included md-input mask-currency" value="' + item.IGSTPercentage + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GSTAmount i  ncluded md-input ' + gridcurrencyclass + '" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="CashDiscount included md-input ' + gridcurrencyclass + '" value="' + item.CashDiscount + '" readonly="readonly" /></td>'
                + '<td class="cess-enabled"><input type="text" class="CessPercentage ' + gridcurrencyclass + '" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                + '<td class="cess-enabled"><input type="text" class="CessAmount ' + gridcurrencyclass + '" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="NetAmount included md-input ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        } else if (item.IsVat == 1) {
            title = (typeof item.SalesOrderNo == "undefined" ? "" : 'Order No : ' + item.SalesOrderNo + '<br/>')
                + 'Order Quantity : ' + item.Qty + '<br/>'
                + 'Offer Quantity : ' + item.OfferQty + '<br/>'
                + 'Stock : ' + item.Stock;
            row_class = item.SalesOrderItemID == 0 ? "proforma-invoice" : "sales-order"
            tr += '<tr class="included ' + row_class + ' ' + item.OfferQtyMet + ' ' + item.QtyMet + ' quantity-' + item.InvoiceQty + ' " data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
                + ' <td class="uk-text-center showitemhistory action">'
                + '     <a class="view-itemhistory">'
                + '         <i class="uk-icon-eye-slash"></i>'
                + '     </a>'
                + '</td>'
                + '<td class="uk-text-center index">' + item.index
                + '<input type="hidden" class="ItemID included" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '<input type="hidden" class="Code" value="' + item.Code + '"  />'
                + '<input type="hidden" class="ItemName" value="' + item.ItemName + '"  />'
                + '<input type="hidden" class="PartsNumber" value="' + item.PartsNumber + '"  />'
                + '<input type="hidden" class="DeliveryTerm" value="' + item.DeliveryTerm + '"  />'
                + '<input type="hidden" class="Model" value="' + item.Model + '"  />'
                + '<input type="hidden" class="IsVAT" value="' + item.IsVat + '"  />'
                + '<input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
                + '<input type="hidden" class="BatchID included" value="' + item.BatchID + '"  />'
                + '<input type="hidden" class="VATPercentage" value="' + item.VATPercentage + '" />'
                + '<input type="hidden" class="ProformaInvoiceTransID included" value="' + item.ProformaInvoiceTransID + '" />'
                + '<input type="hidden" class="SalesOrderItemID included" value="' + item.SalesOrderItemID + '" />'
                + '<input type="hidden" class="StoreID included" value="' + item.StoreID + '" />'
                + '<input type="hidden" class="OrderQty included" value="' + item.Qty + '" />'
                + '<input type="hidden" class="OfferQty included" value="' + item.OfferQty + '" />'
                + '<input type="hidden" class="Stock included" value="' + item.Stock + '" />'
                + '<input type="hidden" class="PackSize" value="' + item.PackSize + '" />'
                + '<input type="hidden" class="PrimaryUnit" value="' + item.PrimaryUnit + '" />'
                + '<input type="hidden" class="CategoryID" value="' + item.CategoryID + '" />'
                + '<input type="hidden" class="PrintWithItemCode" value="' + item.PrintWithItemCode + '" />'
                + '<input type="hidden" class="SecondaryUnitSize" value="' + item.SecondaryUnitSize + '" />'
                + '</td>'
                + '<td class="uk-text-small">' + item.Code + '</td>'
                + '<td class="uk-text-small">' + item.ItemName + '</td>'
                + '<td class="uk-text-small">' + item.PartsNumber + '</td>'
                + '<td class="uk-text-small">' + item.DeliveryTerm + '</td>'
                + '<td class="uk-text-small">' + item.Model + '</td>'
                + '<td class="uk-hidden">' + item.UnitName + '</td>'
                + '<td>' + item.SecondaryUnit + '</td>'
                //+'<td>' + item.BatchTypeName + '</td>'
                //+' <td>' + item.BatchName + '</td>'
                + '<td class="uk-hidden"><input type="text" class="Qty included md-input mask-sales2-currency" value="' + item.InvoiceQty + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="SecondaryQty included md-input mask-sale2-currency" value="' + item.SecondaryInvoiceQty + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden"><input type="text" class="InvoiceOfferQty included md-input mask-sales2-currency" value="' + item.InvoiceOfferQty + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden"><input type="text" class="SecondaryInvoiceOfferQty included md-input mask-sale2-currency" value="' + item.SecondaryInvoiceOfferQty + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="CurrencyName" value="' + item.CurrencyName + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden mrp_hidden" ><input type="text" class="MRP included md-input ' + gridcurrencyclass + '" value="' + item.MRP + '" readonly="readonly" /></td>'
                + '<td class="mrp_hidden" ><input type="text" class="SecondaryMRP included md-input ' + gridcurrencyclass + '" value="' + item.SecondaryMRP + '" readonly="readonly" /></td>'
                //+' <td><input type="text" class="BasicPrice included md-input ' + gridcurrencyclass +'" value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="GrossAmount included md-input ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                //+' <td><input type="text" class="AdditionalDiscount included md-input ' + gridcurrencyclass +'" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
                + '<td class="action"><input type="text" class="DiscountPercentage included md-input mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + '<td class="action"><input type="text" class="DiscountAmount included md-input ' + gridcurrencyclass + '" value="' + item.DiscountAmount + '" /></td>'
                //+' <td><input type="text" class="TurnoverDiscount included md-input ' + gridcurrencyclass +'" value="' + item.TurnoverDiscount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="TaxableAmount included md-input ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="VAT included md-input ' + gridcurrencyclass + '" value="' + item.VATPercentage + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="VATAmount i  ncluded md-input ' + gridcurrencyclass + '" value="' + item.VATAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="CashDiscount included md-input ' + gridcurrencyclass + '" value="' + item.CashDiscount + '" readonly="readonly" /></td>'
                //+' <td class="cess-enabled"><input type="text" class="CessPercentage ' + gridcurrencyclass +'" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                //+' <td class="cess-enabled"><input type="text" class="CessAmount ' + gridcurrencyclass +'" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="NetAmount included md-input ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + '</td>'
                + '</tr>';
        }

    });

    var $tr = $(tr);
    app.format($tr);
    $("#sales-invoice-items-list tbody").html($tr);
    $("#item-count").val(index);
    setTimeout(function () {
        fh_items.resizeHeader();
    }, 200);
};
SalesInvoice.set_amount_details = function () {
    var self = SalesInvoice;
    var is_igst = self.is_igst();
    var GSTPercent;
    var GSTAmount;
    var VATPercent;
    var VATAmount;
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

        if (item.IsGST == 1) {
            if (is_igst) {
                self.calculate_group_tax_details(item.IGSTPercentage, item.GSTAmount, "IGST", item.TaxableAmount);
            } else {
                self.calculate_group_tax_details(item.IGSTPercentage / 2, item.GSTAmount / 2, "SGST", item.TaxableAmount);
                self.calculate_group_tax_details(item.IGSTPercentage / 2, item.GSTAmount / 2, "CGST", item.TaxableAmount);
                self.calculate_group_tax_details(item.CessPercentage, item.CessAmount, "Cess", item.TaxableAmount);
            }
        }
        else if (item.IsVat == 1) {
            self.calculate_group_tax_details(item.VATPercentage, item.VATAmount, "VAT", item.TaxableAmount);
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
};
SalesInvoice.set_form_values = function () {
    var self = SalesInvoice;
    $("#GrossAmount").val(self.Invoice.GrossAmount);
    $("#DiscountAmount").val(self.Invoice.DiscountAmount.roundToCustom());
    self.Invoice.DiscountPercentage = self.Invoice.DiscountAmount / self.Invoice.GrossAmount * 100;
    $("#DiscountPercentage").val(self.Invoice.DiscountPercentage.roundToCustom());
    $("#AdditionalDiscount").val(self.Invoice.AdditionalDiscount.roundToCustom());
    $("#TaxableAmount").val(self.Invoice.TaxableAmount.roundToCustom());
    $("#RoundOff").val(self.Invoice.RoundOff.roundToCustom());
    $("#SGSTAmount").val(self.Invoice.SGSTAmount.roundToCustom());
    $("#CGSTAmount").val(self.Invoice.CGSTAmount.roundToCustom());
    $("#IGSTAmount").val(self.Invoice.IGSTAmount.roundToCustom());
    $("#VATAmount").val(self.Invoice.VATAmount.roundToCustom());
    $("#CashDiscount").val(self.Invoice.CashDiscount.roundToCustom());
    $("#CessAmount").val(self.Invoice.CessAmount.roundToCustom());
    $("#NetAmountPrefix").val(self.Invoice.NetAmount);
    $("#NetAmount").val(self.Invoice.NetAmount.roundToCustom());
};
SalesInvoice.set_item_calculations = function () {
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
            if (item.IsGST == 1) {
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
            } else if (item.IsVat == 1) {
                item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100);
                if (Sales.is_cess_effect()) {
                    item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
                } else {
                    item.CessAmount = 0;
                }
                item.NetAmount = item.TaxableAmount + item.VATAmount + item.CessAmount;
            }
            item.CashDiscount = (item.NetAmount * CashDiscountPercentage / 100);
            item.NetAmount = item.NetAmount - item.CashDiscount;
        }
    });
};
SalesInvoice.find_currencyClass = function ($input) {
    var currencyClassName = "";
    var classNamesArray = $($input).attr('class').split(' ');
    for (var i = 0; i < classNamesArray.length; i++) {
        if (classNamesArray[i].indexOf('mask-') != -1) {
            currencyClassName = classNamesArray[i];
        }
    }
    return currencyClassName;
};
SalesInvoice.change_decimalplaces = function ($input, DecimalPlaces) {
    var self = SalesInvoice;
    var existingCurrencyClass = self.find_currencyClass($input);
    var newnormalclass = self.get_new_currencyClass(DecimalPlaces);
    $($input).removeClass(existingCurrencyClass);
    $($input).addClass(newnormalclass);
    self.applay_new_currencyClass('', $input, DecimalPlaces);
    gridcurrencyclass = newnormalclass;
};
SalesInvoice.get_new_currencyClass = function (DecimalPlaces) {
    var currencyClassName = "";
    if (clean(DecimalPlaces) > 0) {
        currencyClassName = 'mask-sales' + DecimalPlaces + '-currency';
    }
    else {
        currencyClassName = 'mask-sales2-currency';
    }
    return currencyClassName;
};
SalesInvoice.applay_new_currencyClass = function (prefix, $currency, DecimalPlaces) {
    if (prefix.length > 0) {
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': " + DecimalPlaces + ", 'digitsOptional': false, 'prefix': '" + prefix + "', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
    } else {
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': " + DecimalPlaces + ", 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
    }

};
SalesInvoice.select_customer = function () {
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
    var CurrencyCode = $(row).find(".CurrencyCode").val();
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
    $("#CurrencyCode").val(CurrencyCode);

    $("#CashDiscountPercentage").val(CashDiscountPercentage);
    var DecimalPlaces = $(row).find(".DecimalPlaces").val();
    self.change_decimalplaces($("#NetAmountPrefix"), DecimalPlaces);
    self.change_decimalplaces($("#GrossAmount"), DecimalPlaces);
    self.change_decimalplaces($("#AdditionalDiscount"), DecimalPlaces);
    self.change_decimalplaces($("#TurnoverDiscount"), DecimalPlaces);
    self.change_decimalplaces($("#VATAmount"), DecimalPlaces);
    self.change_decimalplaces($("#RoundOff"), DecimalPlaces);
    self.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
    self.change_decimalplaces($("#DiscountAmount"), DecimalPlaces);
    self.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);
    self.change_decimalplaces($("#CessAmount"), DecimalPlaces);
    self.change_decimalplaces($("#CashDiscount"), DecimalPlaces);

    UIkit.modal($('#select-customer')).hide();
    self.Invoice.CustomerID = ID;
    self.customer_on_change();
    self.on_change_customer_category();
};
SalesInvoice.calculate_grid_total = function () {
    var self = SalesInvoice;
    var FreightAmt = clean($("#TotalFreightAmt").val());
    var FreightTaxAmt = clean($("#FreightTaxAmt").val());
    self.Invoice.GSTAmount = 0;
    self.Invoice.VATAmount = 0;
    self.Invoice.TurnoverDiscount = clean($("#TurnoverDiscount").val());
    self.Invoice.GrossAmount = 0;
    self.Invoice.DiscountAmount = 0;
    self.Invoice.AdditionalDiscount = 0;
    self.Invoice.TaxableAmount = 0;
    self.Invoice.NetAmount = 0;
    self.Invoice.RoundOff = 0;
    self.Invoice.CashDiscount = 0;
    self.Invoice.CessAmount = 0;
    self.Invoice.IsGST = 0;
    self.Invoice.IsVat = 0;
    var temp = 0;
    $(self.Items).each(function (i, item) {
        if (item.IsIncluded) {
            self.Invoice.GrossAmount += item.GrossAmount;
            self.Invoice.DiscountAmount += item.DiscountAmount;
            self.Invoice.AdditionalDiscount += item.AdditionalDiscount;
            self.Invoice.TaxableAmount += item.TaxableAmount;
            self.Invoice.GSTAmount += item.GSTAmount;
            self.Invoice.VATAmount += item.VATAmount;
            self.Invoice.NetAmount += item.NetAmount;
            self.Invoice.CashDiscount += item.CashDiscount;
            self.Invoice.CessAmount += item.CessAmount;
            self.Invoice.IsGST = item.IsGST;
            self.Invoice.IsVat = item.IsVat;
        }
    });
    self.Invoice.NetAmount += FreightAmt;
    temp = self.Invoice.NetAmount;
    self.Invoice.NetAmount = parseFloat(self.Invoice.NetAmount);
    self.Invoice.RoundOff = temp - self.Invoice.NetAmount;
    self.Invoice.CheckStock = $("#CheckStock").val();
    if (self.Invoice.IsGST == 1) {
        if (self.is_igst()) {
            self.Invoice.CGSTAmount = 0;
            self.Invoice.SGSTAmount = 0;
            self.Invoice.IGSTAmount = self.Invoice.GSTAmount + FreightTaxAmt;
        } else {
            self.Invoice.CGSTAmount = ((self.Invoice.GSTAmount / 2) + (FreightTaxAmt / 2));
            self.Invoice.SGSTAmount = ((self.Invoice.GSTAmount / 2) + (FreightTaxAmt / 2));
            self.Invoice.IGSTAmount = 0;
        }
    } else if (self.Invoice.IsVat == 1) {
        self.Invoice.VATAmount = self.Invoice.VATAmount + FreightTaxAmt;
    }
};
SalesInvoice.customer_list = function (type) {
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
                        return row.LandLine1 + ', ' + row.LandLine2;
                    }
                },
                { "data": "MobileNo", "className": "MobileNo" },
                {
                    "data": null,
                    "className": "uk-text-center uk-hidden",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        return row.IsGSTRegistered == 1 ? "Yes" : "No";
                    }
                },
                { "data": "CurrencyName", "className": "CurrencyName uk-hidden" },
                { "data": "CurrencyConversionRate", "className": "CurrencyConversionRate uk-hidden" },
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
};

Number.prototype.roundToCustom = function () {

    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};

