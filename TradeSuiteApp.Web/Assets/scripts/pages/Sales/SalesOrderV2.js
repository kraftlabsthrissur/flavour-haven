var gridcurrencyclass = 'mask-sales2-currency';
var DecPlaces = 0;
$(function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
    //alert(gridcurrencyclass);
});
SalesOrder.init = function () {
    var self = SalesOrder;
    self.customer_list('sales-order');
    item_list = self.item_list();
    $('#item-list').SelectTable({
        selectFunction: self.select_item,
        returnFocus: "#Qty",
        modal: "#select-item",
        initiatingElement: "#ItemName",
        startFocusIndex: 3
    });
    $('#customer-list').SelectTable({
        selectFunction: self.select_customer,
        returnFocus: "#DespatchDate",
        modal: "#select-customer",
        initiatingElement: "#CustomerName"
    });
    if ($("#CustomerID").val() != 0) {
        var ItemIDS = [];
        var UnitIDS = [];
        $("#sales-order-items-list tbody tr").each(function () {
            ItemIDS.push($(this).find('.ItemID').val());
            UnitIDS.push($(this).find('.UnitID').val());
        });
        self.get_offer_details_bulk(ItemIDS, UnitIDS);
    }
    self.on_change_customer_category();
    self.bind_events();
    self.freeze_headers();
    self.purchase_order_history();
    self.pending_po_history();
    self.purchase_legacy_history();
    self.sales_order_history('Quotation');
    self.sales_order_history('SalesOrder');
    $("#Offers").hide();
    $('#tabs-sales-order-create').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            active_item.data('tab-loaded', true);
        }
    });
};
SalesOrder.on_qty_change = function () {
    var self = SalesOrder;
    var getclassNames = $(this).attr('class');
    var row = $(this).closest('tr');
    var classname = '';
    if (getclassNames.indexOf('DiscountPercentage') != -1) {
        classname = 'DiscountPercentage';
    } else if (getclassNames.indexOf('DiscountAmount') != -1) {
        classname = 'DiscountAmount';
    } else if (getclassNames.indexOf('MRP') != -1) {
        classname = 'MRP';
    }
    else {
        classname = '';
    }
    self.update_item(row, classname);
};
SalesOrder.on_secondary_unit_change = function () {
    var self = SalesOrder;
    var $row = $(this).closest('tr');
    var SecondaryQty = clean($row.find('.secondaryQty').val());
    var SecondaryUnitSize = clean($row.find('.secondaryUnit').val());
    if ($(this).attr('class').indexOf('secondaryMRP') != -1) {
        var SecondaryMRP = clean($row.find('.secondaryMRP').val());
        var MRP = (SecondaryMRP / SecondaryUnitSize).toFixed(10);
        $row.find('.MRP').val(MRP);
        $row.find('.MRP').trigger('change');

    } else {
        var MRP = clean($row.find('.MRP').val());
        var SecondaryMRP = MRP * SecondaryUnitSize;
        $row.find('.secondary .secondaryMRP').val(SecondaryMRP);
        var Qty = (SecondaryQty * SecondaryUnitSize).toFixed(10);
        $row.find('.Qty').val(Qty);
        $row.find('.Qty').trigger('change');
    }

};
SalesOrder.showitemhistory = function () {
    var ItemID = $(this).closest('tr').find('.ItemID').val();
    $("#HistoryItemID").val(ItemID);
    $('#show-history').trigger('click');
}

SalesOrder.item_list = function () {
    var $list = $('#item-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            if (title !== undefined && title.length > 0) {
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            }
        });
        altair_md.inputs();
        var url = "/Masters/Item/GetSaleableItemsList";
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
                "beforeSend": function () {
                    if (typeof list_table != "undefined" && list_table.api().hasOwnProperty('settings')) {
                        list_table.api().settings()[0].jqXHR.abort();
                    }
                },
                "data": function (data) {
                    data.params = [
                        { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                        { Key: "SalesCategoryID", Value: $('#SalesCategoryID').val() },
                        { Key: "PriceListID", Value: $('#PriceListID').val() },
                        { Key: "StoreID", Value: $('#StoreID').val() },
                        { Key: "CheckStock", Value: $('#CheckStock').val() },
                        { Key: "BatchTypeID", Value: $('#BatchTypeID').val() },
                        { Key: "FullOrLoose", Value: $('#FullOrLoose').length == 0 ? "F" : $('#FullOrLoose').val() },
                        { Key: "BusinessCategoryID", Value: $('#BusinessCategoryID').val() },
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
                            + "<input type='hidden' class='Stock' value='" + row.Stock + "'>"
                            + "<input type='hidden' class='CGSTPercentage' value='" + row.CGSTPercentage + "'>"
                            + "<input type='hidden' class='IGSTPercentage' value='" + row.IGSTPercentage + "'>"
                            + "<input type='hidden' class='SGSTPercentage' value='" + row.SGSTPercentage + "'>"
                            + "<input type='hidden' class='CessPercentage' value='" + row.CessPercentage + "'>"
                            + "<input type='hidden' class='VATPercentage' value='" + row.VATPercentage + "'>"
                            + "<input type='hidden' class='Model' value='" + row.Model+' / '+ row.Make + "'>"
                            + "<input type='hidden' class='TaxType' value='" + row.TaxType + "'>"
                            + "<input type='hidden' class='TaxTypeID' value='" + row.TaxTypeID + "'>"
                            + "<input type='hidden' class='PartsNumber' value='" + row.PartsNumber + "'>"
                            + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                            + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                            + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"
                            + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>"
                            + "<input type='hidden' class='SalesCategoryID' value='" + row.SalesCategoryID + "'>"
                            + "<input type='hidden' class='SalesCategory' value='" + row.SalesCategory + "'>"
                            + "<input type='hidden' class='LooseRate' value='" + row.LooseRate + "'>"
                            + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>"
                            + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>"
                            + "<input type='hidden' class='MaxSalesQtyFull' value='" + row.MaxSalesQtyFull + "'>"
                            + "<input type='hidden' class='MinSalesQtyLoose' value='" + row.MinSalesQtyLoose + "'>"
                            + "<input type='hidden' class='MinSalesQtyFull' value='" + row.MinSalesQtyFull + "'>"
                            + "<input type='hidden' class='MaxSalesQtyLoose' value='" + row.MaxSalesQtyLoose + "'>";
                    }
                },
                { "data": "Code", "className": "Code" },
                { "data": "Name", "className": "Name" },
                { "data": "ItemCategory", "className": "ItemCategory" },
                { "data": "SalesCategory", "className": "SalesCategory" },
                { "data": "PartsNumber", "className": "PartsNumber" },
                { "data": "Model", "className": "Model" },
                { "data": "Make", "className": "Make" },
                { "data": "Remarks", "className": "Remarks" },
                {
                    "data": "Stock", "className": "Stock", "searchable": false, "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.Stock + "</div>";
                    }
                },
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
        $('body').on("change", '#SalesCategoryID', function () {
            list_table.fnDraw();
        });
        $('body').on("change", '#StoreID', function () {
            list_table.fnDraw();
        });
        $('body').on("change", '#BusinessCategoryID', function () {
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

SalesOrder.customer_list = function (type) {
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

SalesOrder.sales_order_history = function (type) {

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
SalesOrder.purchase_legacy_history = function () {

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
SalesOrder.purchase_order_history = function () {

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

SalesOrder.pending_po_history = function () {

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
SalesOrder.on_change_discount_Amount = function () {

    var self = SalesOrder;
    var elementid = $(this).attr('id');
    var GrossAmt = clean($("#GrossAmount").val());
    var discount = clean($("#DiscAmount").val());
    var discountpercent = clean($("#DiscPercentage").val());
    var vatpercent = 0;
    var vatpercentid = 0;
    vatpercentid = clean($("#VATPercentageID").val());
    vatpercent = clean($("#VATPercentageID option:selected").text());
    if (elementid == 'DiscPercentage') {
        discount = 0;
        discountpercent = 0;
        discountpercent = clean($("#DiscPercentage").val());
        discount = (GrossAmt * discountpercent / 100);
    } else if (elementid == 'DiscAmount') {
        discount = 0;
        discountpercent = 0;
        discount = clean($("#DiscAmount").val());
        discountpercent = (discount / GrossAmt * 100);
    }
    else if (elementid == 'VATPercentageID') {
        vatpercentid = clean($("#VATPercentageID").val());
        vatpercent = clean($("#VATPercentageID option:selected").text());
    }
    var discountper = ((discount * 100) / GrossAmt);
    var TotalGrossAmount = 0;
    var TotalTaxableAmount = 0;
    var TotalSGSTAmount = 0;
    var TotalCGSTAmount = 0;
    var TotalIGSTAmount = 0;
    var TotalVATAmount = 0;
    var TotalRoundOff = 0;
    var TotalCessAmount = 0;
    var TotalNetAmount = 0;
    $("#sales-order-items-list tbody tr").each(function () {

        $row = $(this).closest("tr");
        var GrossAmount = clean($row.find(".GrossAmount").val());
        TotalGrossAmount = TotalGrossAmount + GrossAmount;
        var SGSTPercentage = clean($row.find(".SGSTPercentage").val());
        var CGSTPercentage = clean($row.find(".CGSTPercentage").val());
        var CessPercentage = clean($row.find(".CessPercentage").val());
        var IGSTPercentage = clean($row.find(".IGSTPercentage").val());
        //var VATPercentage = clean($row.find(".VATPercentage").val());
        var VATPercentage = vatpercent
        var DiscountAmount = (GrossAmount * (discountper / 100));
        var DiscountPercentage = (DiscountAmount / GrossAmount * 100);
        var TaxableAmount = (GrossAmount - DiscountAmount);
        TotalTaxableAmount = TotalTaxableAmount + TaxableAmount;
        //var TaxableAmount = GrossAmount - TaxableAmt;
        IGSTAmount = TaxableAmount * (IGSTPercentage / 100);
        TotalIGSTAmount = TotalIGSTAmount + IGSTAmount;
        SGSTAmount = TaxableAmount * (SGSTPercentage / 100);
        TotalSGSTAmount = TotalSGSTAmount + SGSTAmount;
        CGSTAmount = TaxableAmount * (CGSTPercentage / 100);
        TotalCGSTAmount = TotalCGSTAmount + CGSTAmount;
        CessAmount = TaxableAmount * (CessPercentage / 100);
        TotalCessAmount = TotalCessAmount + CessAmount;
        VATAmount = TaxableAmount * (VATPercentage / 100);
        TotalVATAmount = TotalVATAmount + VATAmount;
        var NetAmount = TaxableAmount + CessAmount + CGSTAmount + SGSTAmount + VATAmount;
        TotalNetAmount = TotalNetAmount + NetAmount;
        $row.find(".TaxableAmount").val(TaxableAmount.roundToCustom());
        $row.find(".CessAmount").val(CessAmount.roundToCustom());
        $row.find(".CGST").val(CGSTAmount.roundToCustom());
        $row.find(".SGST").val(SGSTAmount.roundToCustom());
        $row.find(".IGST").val(IGSTAmount.roundToCustom());
        $row.find(".VATAmount").val(VATAmount.roundToCustom());
        $row.find(".NetAmount").val(NetAmount.roundToCustom());
        $row.find(".DiscountAmount").val(DiscountAmount.roundToCustom());
        $row.find(".DiscountPercentage").val(DiscountPercentage.roundToCustom());
        $row.find(".VATPercentageID").val(vatpercentid);
        $row.find(".VATPercentage").val(vatpercent);
    });
    if (elementid == 'DiscPercentage') {
        $("#DiscountAmt").val(discount.roundToCustom());
        $("#DiscAmount").val(discount.roundToCustom());
    } else if (elementid == 'DiscAmount') {
        var discountpercent = (discount / GrossAmt * 100);
        $("#DiscPercentage").val(discountpercent.roundToCustom());
    } else {
        $("#DiscountAmt").val(discount);
        $("#DiscAmount").val(discount);
        var discountpercent = (discount / GrossAmt * 100);
        $("#DiscPercentage").val(discountpercent.roundToCustom());
    }
    $("#GrossAmount").val(TotalGrossAmount.roundToCustom());
    $("#TaxableAmount").val(TotalTaxableAmount.roundToCustom());
    $("#SGSTAmount").val(TotalSGSTAmount.roundToCustom());
    $("#CGSTAmount").val(TotalCGSTAmount.roundToCustom());
    $("#IGSTAmount").val(TotalIGSTAmount.roundToCustom());
    $("#VATAmount").val(TotalVATAmount.roundToCustom());
    $("#CessAmount").val(TotalCessAmount.roundToCustom());
    $("#RoundOff").val(TotalRoundOff.roundToCustom());
    $("#NetAmount").val(TotalNetAmount.roundToCustom());
    $("#NetAmountValue").val(TotalNetAmount.roundToCustom());

};
SalesOrder.update_item = function (row, classname) {
    var self = SalesOrder;

    var item = {};

    item.Category = $(row).find(".Category").val();
    var batchtype = $("#BatchType").val();
    if (item.Category == "Finished Goods") {
        item.BatchType = batchtype;
    }
    else {
        item.BatchType = "";
    }
    //item.Qty = clean($(row).find(".Qty").val());
    //item.MRP = clean($(row).find(".MRP").val());
    item.Qty = clean($(row).find(".secondaryQty").val());
    item.MRP = clean($(row).find(".secondaryMRP").val());
    if (classname == 'DiscountPercentage') {
        item.DiscountPercentage = clean($(row).find(".DiscountPercentage").val());
        item.DiscountAmount = 0;
    } else if (classname == 'DiscountAmount') {
        item.DiscountAmount = clean($(row).find(".DiscountAmount").val());
        item.DiscountPercentage = 0;
    } else {
        item.DiscountAmount = clean($(row).find(".DiscountAmount").val());
        item.DiscountPercentage = 0;
    }
    item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
    item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
    item.CGSTPercentage = clean($(row).find(".CGSTPercentage").val());
    item.VATPercentage = clean($(row).find(".VATPercentage").val());
    item.ID = clean($(row).find(".ItemID").val());
    item.UnitID = clean($(row).find(".UnitID").val());
    item.FullRate = clean($("#Rate").val());
    item.LooseRate = clean($("#LooseRate").val());
    item.SaleUnitID = clean($("#SalesUnitID").val());
    item.CessPercentage = clean($("#CessPercentage").val());
    item = self.get_item_properties(item);

    $(row).find(".OfferQty").val(item.OfferQty);
    $(row).find(".GrossAmount").val(item.GrossAmount);
    if (classname == 'DiscountPercentage') {
        $(row).find(".DiscountAmount").val(item.DiscountAmount);
    } else if (classname == 'DiscountAmount') {
        $(row).find(".DiscountPercentage").val(item.DiscountPercentage);
    } else {
        $(row).find(".DiscountAmount").val(item.DiscountAmount);
        $(row).find(".DiscountPercentage").val(item.DiscountPercentage);
    }
    $(row).find(".AdditionalDiscount ").val(item.AdditionalDiscount);
    $(row).find(".TaxableAmount").val(item.TaxableAmount);
    $(row).find(".CGST").val(item.CGST);
    $(row).find(".SGST").val(item.SGST);
    $(row).find(".IGST").val(item.IGST);
    $(row).find(".GSTAmount").val(item.GSTAmount);
    $(row).find(".VATAmount").val(item.VATAmount);
    $(row).find(".CessAmount").val(item.CessAmount);
    $(row).find(".NetAmount").val(item.NetAmount);
    $(row).find(".BatchType").text(item.BatchType);
    $(row).find(".BasicPrice").val(item.BasicPrice);
    self.calculate_grid_total();
};
SalesOrder.select_customer = function () {
    var self = SalesOrder;
    var radio = $('#select-customer tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var CurrencyCode = $(row).find(".CurrencyCode").val();

    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    var StateID = $(row).find(".StateID").val();
    var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
    var PriceListID = $(row).find(".PriceListID").val();
    var SchemeID = $(row).find(".SchemeID").val();
    var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
    var CustomerCategory = $(row).find(".CustomerCategory").text();
    var CurrencyID = $(row).find(".CurrencyID").val();
    var CurrencyName = $(row).find(".CurrencyName").text().trim();
    var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
    var DecimalPlaces = $(row).find(".DecimalPlaces").val();
    gridcurrencyclass = app.change_decimalplaces($("#NetAmount"), DecimalPlaces);
    app.change_decimalplaces($("#GrossAmount"), DecimalPlaces);
    app.change_decimalplaces($("#VATAmount"), DecimalPlaces);
    app.change_decimalplaces($("#RoundOff"), DecimalPlaces);
    app.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
    app.change_decimalplaces($("#DiscAmount"), DecimalPlaces);
    app.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);

    $("#CustomerName").val(Name);
    $("#CustomerCategoryID").val(CustomercategoryID);
    $("#CustomerID").val(ID);
    $("#CurrencyID").val(CurrencyID);
    $("#CurrencyName").val(CurrencyName);
    $("#CurrencyCode").val(CurrencyCode);
    $("#CurrencyExchangeRate").val(CurrencyConversionRate);
    $("#StateID").val(StateID);
    $("#PriceListID").val(PriceListID);
    $("#SchemeID").val(SchemeID);
    $("#CustomerCategory").val(CustomerCategory);
    $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
    $("#DecimalPlaces").val(DecimalPlaces);
    UIkit.modal($('#select-customer')).hide();

    self.get_batch_type(function () {
        var self = SalesOrder;
        var ItemIDS = [];
        var UnitIDS = [];
        $("#sales-order-items-list tbody tr").each(function () {
            ItemIDS.push($(this).find('.ItemID').val());
            UnitIDS.push($(this).find('.UnitID').val());
        });
        if (ItemIDS.length > 0) {
            self.get_offer_details_bulk(ItemIDS, UnitIDS, function () {
                var self = SalesOrder;

                $("#sales-order-items-list tbody tr").each(function () {
                    var row = $(this).closest("tr");
                    self.update_item(row);
                });
            });
        }

    });
    if ($("#IsClone").val() != "True") {
        self.clear_grid_offer();
    }
    Sales.get_customer_addresses();
    self.on_change_customer_category();
};
SalesOrder.get_item_properties = function (item) {
    var self = SalesOrder;
    var currency = self.getCurrencyDetails();
    item.CurrencyID = currency.CurrencyID;
    item.CurrencyName = currency.CurrencyName;
    item.IsGST = currency.IsGST;
    item.IsVat = currency.IsVat;
    item.TaxTypeID = currency.TaxTypeID;
    item.CountryID = currency.CountryID;
    item.BaseCurrencyMRP = item.MRP;
    if (item.IsVat == 1) {
        item.VATPercentage = $("#VATPercentageID option:selected").text();
    }
    var CustomerCategory = $("#CustomerCategory").val();
    var IsVATExtra = $("#IsVATExtra").val();
    item.item_offer_details = self.item_offer_details;
    item.OfferQty = self.get_offer_qty(item.Qty, item.ID, item.UnitID);

    if (item.IsGST == 1) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        } else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        if (CustomerCategory == "Export") {
            item.BasicPrice = item.MRP;
        }
    } else if (item.IsVat == 1) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.VATPercentage + item.CessPercentage);
        } else {
            item.BasicPrice = item.MRP * 100 / (100 + item.VATPercentage);
        }
    }
    ////added by anju
    if (IsVATExtra == 1) {
        item.BasicPrice = item.MRP;
    }
    item.BasicPrice = item.BasicPrice;
    item.GrossAmount = item.BasicPrice * (item.Qty + item.OfferQty);
    if (item.DiscountPercentage > 0) {
        item.DiscountAmount = (item.Qty * item.BasicPrice * item.DiscountPercentage / 100);
    } else if (item.DiscountAmount > 0) {
        item.DiscountPercentage = item.DiscountAmount / (item.Qty * item.BasicPrice) * 100;
    } else {
        item.DiscountAmount = 0;
        item.DiscountPercentage = 0;
    }
    item.AdditionalDiscount = (item.BasicPrice * item.OfferQty);
    item.TaxableAmount = (item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount);
    if (item.IsGST == 1) {
        item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100);
        if (CustomerCategory == "Export") {
            item.CessAmount = 0;
            item.CessPercentage = 0;
            item.IGST = 0;
            item.CGST = 0;
            item.SGST = 0;
            item.GSTAmount = 0;
            item.GSTPercentage = 0;
        }
        else {
            if (Sales.is_cess_effect()) {
                item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
            } else {
                item.CessAmount = 0;
                item.CessPercentage = 0;
            }
            item.GSTPercentage = item.IGSTPercentage;

            if (!Sales.is_igst()) {
                item.CGST = (item.GSTAmount / 2);
                item.SGST = (item.GSTAmount / 2);
                item.IGST = 0;
            } else {
                item.CGST = 0;
                item.SGST = 0;
                item.IGST = item.GSTAmount;
            }
        }
        item.NetAmount = item.TaxableAmount + item.CGST + item.IGST + item.SGST + item.CessAmount;
    } else if (item.IsVat == 1) {
        if (Sales.is_cess_effect()) {
            item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
            item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100);
            item.NetAmount = item.TaxableAmount + item.VATAmount + item.CessAmount;
        } else {
            item.CessAmount = 0;
            item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100);
            item.NetAmount = item.TaxableAmount + item.VATAmount + item.CessAmount;
        }
    }
    return item;
};
SalesOrder.select_item = function () {
    var self = SalesOrder;
    var radio = $('#select-item tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var item = {};
    item.id = radio.val();
    item.name = $(row).find(".Name").text().trim();
    item.code = $(row).find(".Code").text().trim();
    item.partsnumber = $(row).find(".PartsNumber").val();
    item.unit = $(row).find(".Unit").val();
    item.unitId = $(row).find(".UnitID").val();
    item.saleUnit = $(row).find(".SalesUnit").val();
    item.saleUnitId = $(row).find(".SalesUnitID").val();
    item.stock = $(row).find(".Stock").val();
    item.cgstpercentage = $(row).find(".CGSTPercentage").val();
    item.igstpercentage = $(row).find(".IGSTPercentage").val();
    item.sgstpercentage = $(row).find(".SGSTPercentage").val();
    item.vatpercentage = $(row).find(".VATPercentage").val();
    item.secondaryUnits = $(row).find(".SecondaryUnits").val();
    item.model = $(row).find(".Model").val();
    item.taxtypeid = $(row).find(".TaxTypeID").val();
    item.taxtype = $(row).find(".TaxType").val();
    item.cessPercentage = $(row).find(".CessPercentage").val();
    item.rate = $(row).find(".Rate").val();
    item.looserate = $(row).find(".LooseRate").val();
    item.category = $(row).find(".ItemCategory").text().trim();
    item.maxsalesqtyfull = $(row).find(".MaxSalesQtyFull").val();
    item.minsalesqtyloose = $(row).find(".MinSalesQtyLoose").val();
    item.minsalesqtyfull = $(row).find(".MinSalesQtyFull").val();
    item.maxsalesqtyloose = $(row).find(".MaxSalesQtyLoose").val();
    self.on_select_item(item);

    UIkit.modal($('#select-item')).hide();
};
SalesOrder.on_select_item = function (item) {
    var self = SalesOrder;
    $("#ItemName").val(item.name);
    $("#PartsNumber").val(item.partsnumber);
    $("#ItemID").val(item.id);
    $("#PrimaryUnit").val(item.unit);
    $("#Code").val(item.code);
    $("#PrimaryUnitID").val(item.unitId);
    $("#Stock").val(item.stock);
    $("#SalesUnit").val(item.saleUnit);
    $("#SalesUnitID").val(item.saleUnitId);
    $("#CGSTPercentage").val(item.cgstpercentage);
    $("#IGSTPercentage").val(item.igstpercentage);
    $("#SGSTPercentage").val(item.sgstpercentage);
    $("#CessPercentage").val(item.cessPercentage);
    $("#VATPercentage").val(item.vatpercentage);
    $("#SecondaryUnits").val(item.secondaryUnits);
    $("#Model").val(item.model);
    $("#TaxTypeID").val(item.taxtypeid);
    $("#TaxType").val(item.taxtype);
    $("#Rate").val(item.rate);
    $("#LooseRate").val(item.looserate);
    $("#Category").val(item.category);
    $("#MinSalesQtyFull").val(item.minsalesqtyfull);
    $("#MinSalesQtyLoose").val(item.minsalesqtyloose);
    $("#MaxSalesQtyFull").val(item.maxsalesqtyfull);
    $("#MaxSalesQtyLoose").val(item.maxsalesqtyloose);
    $("#Qty").focus();
    self.get_units();
    self.get_discount_and_offer_details();
};
SalesOrder.get_items = function (release) {
    var self = SalesOrder;
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
};
SalesOrder.add_item = function () {
    var self = SalesOrder;
    self.error_count = 0;
    self.error_count = self.validate_customer();
    if (self.error_count > 0) {
        self.clear_item();
        $("#CustomerName").focus();
        return;
    }
    self.error_count = self.validate_item();
    if (self.error_count > 0) {
        // $("#ItemName").focus();
        return;
    }
    var item = {};
    item.Name = $("#ItemName").val();
    item.ID = $("#ItemID").val();
    item.Unit = $("#UnitID option:selected").text();
    item.Code = $("#Code").val();
    item.UnitID = $("#UnitID").val();
    item.Stock = $("#Stock").val();
    item.CGSTPercentage = clean($("#CGSTPercentage").val());
    item.IGSTPercentage = clean($("#IGSTPercentage").val());
    item.SGSTPercentage = clean($("#SGSTPercentage").val());
    item.CessPercentage = clean($("#CessPercentage").val());
    item.SGSTPercentage = clean($("#SGSTPercentage").val());
    item.VATPercentage = clean($("#VATPercentage").val());
    item.SecondaryUnits = $("#SecondaryUnits").val();
    item.DeliveryTerm = '';
    item.Model = $("#Model").val();
    item.TaxTypeID = clean($("#TaxTypeID").val());
    item.TaxType = clean($("#TaxType").val());
    item.PartsNumber = $("#PartsNumber").val();
    item.Qty = clean($("#Qty").val());
    item.FullRate = clean($("#Rate").val());
    item.LooseRate = clean($("#Rate").val());// clean($("#LooseRate").val());
    item.SaleUnitID = clean($("#SalesUnitID").val());
    item.ExchangeRate = clean($("#CurrencyExchangeRate").val());
    if (item.ExchangeRate == 0) {
        item.ExchangeRate = 1;
    }
    item.Category = $("#Category").val();
    if (item.Category == "Finished Goods") {
        item.BatchType = $("#BatchType").val();
        item.BatchTypeID = clean($("#BatchTypeID").val());
    }
    else {
        item.BatchType = "";
        item.BatchTypeID = 0;
    }
    if (item.SaleUnitID == item.UnitID) {
        item.MRP = item.FullRate;
    }
    else {
        item.MRP = item.LooseRate;
    }
    item.MRP = item.MRP;// * item.ExchangeRate;
    if (item.UnitID == $("#SalesUnitID").val()) {
        item.MinSalesQty = clean($("#MinSalesQtyFull").val());
        item.MaxSalesQty = clean($("#MaxSalesQtyFull").val());
    }
    else {
        item.MinSalesQty = clean($("#MinSalesQtyLoose").val());
        item.MaxSalesQty = 5000;
    }
    item.DiscountPercentage = 0;
    item.DiscountAmount = 0;
    self.add_item_to_grid(item);
    fh_items.resizeHeader();
    self.clear_item();
    setTimeout(function () {
        $("#ItemName").focus();
    }, 100);
};
SalesOrder.SelectSecondaryUnits = function (Unit, SecondaryUnits) {
    var optionsSecondaryUnits = SecondaryUnits.split(',');
    var select = '<select class="md-input label-fixed secondaryUnit">';
    select += '<option value="1" selected>' + Unit + '</option>';
    optionsSecondaryUnits.forEach(function (option) {
        var parts = option.split('|');
        if (parts.length > 1) {
            var text = parts[0];
            var value = parts[1];
            select += '<option value="' + value + '">' + text + '</option>';
        }
    });
    select += '</select>';
    return select;
};
SalesOrder.add_item_to_grid = function (item) {
    var self = SalesOrder;
    var index, GSTAmount, tr;
    var title = '';
    var readonly = '';
    var a = '';
    item = self.get_item_properties(item);

    var j = self.search_in_array(self.item_offer_details, item.ID);
    index = $("#sales-order-items-list tbody tr").length + 1;
    self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
        a += '<tr > <td> Scheme ' + record.Qty + ' +' + record.OfferQty + '</td></tr></br>'

    });
    if (clean($("#IsPriceEditable").val()) == 0) {
        readonly = ' readonly="readonly"';
    }
    title += a;
    var tr = '';
    if (item.IsGST == 1) {

        tr = '<tr  data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
            + ' <td class="uk-text-center">' + index + ' </td>'
            + ' <td >' + item.Code
            + '     <input type="hidden" class="ItemID" value="' + item.ID + '"  />'
            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
            + '     <input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
            + '     <input type="hidden" class="CountryID" value="' + item.CountryID + '"  />'
            + '     <input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
            + '     <input type="hidden" class="IsVat" value="' + item.IsVat + '"  />'
            + '     <input type="hidden" class="ExchangeRate" value="' + item.ExchangeRate + '"  />'
            + '     <input type="hidden" class="BaseCurrencyMRP" value="' + item.BaseCurrencyMRP + '"  />'
            + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
            + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
            + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
            + '     <input type="hidden" class="IGST" value="' + item.IGST + '" />'
            + '     <input type="hidden" class="SGST" value="' + item.SGST + '" />'
            + '     <input type="hidden" class="CGST" value="' + item.CGST + '" />'
            + '     <input type="hidden" class="FullRate" value="' + item.FullRate + '" />'
            + '     <input type="hidden" class="LooseRate" value="' + item.LooseRate + '" />'
            + '     <input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
            + '     <input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
            + '     <input type="hidden" class="Category" value="' + item.Category + '" />'
            + '</td>'
            + ' <td><input type="text" class="md-input Name" value="' + item.Name + '"  /></td>'
            + ' <td><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '"  /></td>'
            + ' <td><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '"  /></td>'
            + ' <td><input type="text" class="md-input Model" value="' + item.Model + '"  /></td>'
            + '<td class="uk-hidden">' + item.Unit + '</td>'
            + '<td class="secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
            //+ ' <td class="BatchType">' + item.BatchType + '</td>'
            + ' <td class="CurrencyName">' + item.CurrencyName + '</td>'
            + ' <td class="mrp_hidden uk-hidden"><input type="text" class="md-input MRP ' + gridcurrencyclass + '" value="' + item.MRP + '"' + readonly + '/></td>'
            + ' <td class="secondary"><input type="text" class="md-input secondaryMRP ' + gridcurrencyclass + '" value="' + item.MRP + '"' + readonly + '/></td>'
            //+ ' <td class="mrp_hidden"><input type="text" class="MRP ' + gridcurrencyclass +' " value="' + item.MRP + '" /></td>'
            //+ ' <td><input type="text" class="BasicPrice ' + gridcurrencyclass +' " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
            + ' <td class="uk-hidden"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Qty + '"  /></td>'
            + ' <td class="secondary" ><input type="text" class="md-input secondaryQty mask-sales2-currency" value="' + item.Qty + '"  /></td>'
            + ' <td class="uk-hidden"><input type="text" class="OfferQty mask-sales2-currency" value="' + item.OfferQty + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GrossAmount ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
            + ' <td><input type="text" class="md-input DiscountAmount ' + gridcurrencyclass + '" value="' + item.DiscountAmount + '" /></td>'
            //+ ' <td><input type="text" class="AdditionalDiscount ' + gridcurrencyclass +'" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="TaxableAmount ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GST mask-currency" value="' + item.GSTPercentage + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GSTAmount ' + gridcurrencyclass + '" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
            + ' <td class="cess-enabled"><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
            + ' <td class="cess-enabled"><input type="text" class="CessAmount ' + gridcurrencyclass + '" value="' + item.CessAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="NetAmount ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
            + ' <td class="uk-text-center">'
            + '     <a class="remove-item">'
            + '         <i class="uk-icon-remove"></i>'
            + '     </a>'
            + ' </td>'
            + '</tr>';
    } else if (item.IsVat == 1) {
        tr = '<tr  data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
            + '<td class="uk-text-center showitemhistory action">'
            + '     <a class="view-itemhistory">'
            + '         <i class="uk-icon-eye-slash"></i>'
            + '     </a>'
            + '</td>'
            + ' <td class="uk-text-center">' + index + ' </td>'
            + ' <td >' + item.Code
            + '     <input type="hidden" class="ItemID" value="' + item.ID + '"  />'
            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
            + '     <input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
            + '     <input type="hidden" class="CountryID" value="' + item.CountryID + '"  />'
            + '     <input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
            + '     <input type="hidden" class="IsVat" value="' + item.IsVat + '"  />'
            + '     <input type="hidden" class="ExchangeRate" value="' + item.ExchangeRate + '"  />'
            + '     <input type="hidden" class="BaseCurrencyMRP" value="' + item.BaseCurrencyMRP + '"  />'
            + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '"  />'
            + '     <input type="hidden" class="VATPercentage" value="' + item.VATPercentage + '" />'
            + '     <input type="hidden" class="FullRate" value="' + item.FullRate + '" />'
            + '     <input type="hidden" class="LooseRate" value="' + item.LooseRate + '" />'
            + '     <input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
            + '     <input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
            + '     <input type="hidden" class="Category" value="' + item.Category + '" />'
            + '</td>'
            + '<td><input type="text" class="md-input Name" value="' + item.Name + '"  /></td>'
            + '<td><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '"  /></td>'
            + '<td><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '"  /></td>'
            + '<td><input type="text" class="md-input Model" value="' + item.Model + '"  /></td>'
            + '<td class="uk-hidden">' + item.Unit + '</td>'
            + '<td class="secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
            //+ ' <td class="BatchType">' + item.BatchType + '</td>'
            + ' <td class="CurrencyName">' + item.CurrencyName + '</td>'
            + ' <td class="mrp_hidden uk-hidden"><input type="text" class="md-input MRP ' + gridcurrencyclass + '" value="' + item.MRP + '"' + readonly + '/></td>'
            + ' <td class="secondary"><input type="text" class="md-input secondaryMRP ' + gridcurrencyclass + '" value="' + item.MRP + '"' + readonly + '/></td>'
            //+ ' <td class="mrp_hidden"><input type="text" class="MRP ' + gridcurrencyclass +' " value="' + item.MRP + '" /></td>'
            // + ' <td><input type="text" class="BasicPrice ' + gridcurrencyclass +' " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
            + ' <td class="uk-hidden"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Qty + '"  /></td>'
            + ' <td class="secondary" ><input type="text" class="md-input secondaryQty mask-sales2-currency" value="' + item.Qty + '"  /></td>'
            + ' <td class="uk-hidden"><input type="text" class="OfferQty mask-sales2-currency" value="' + item.OfferQty + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GrossAmount ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
            + ' <td><input type="text" class="md-input DiscountAmount ' + gridcurrencyclass + '" value="' + item.DiscountAmount + '" /></td>'
            // + ' <td><input type="text" class="AdditionalDiscount ' + gridcurrencyclass +'" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="TaxableAmount ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="VATPercentage mask-currency" value="' + item.VATPercentage + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="VATAmount ' + gridcurrencyclass + '" value="' + item.VATAmount + '" readonly="readonly" /></td>'
            //+ ' <td class="cess-enabled"><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
            //+ ' <td class="cess-enabled"><input type="text" class="CessAmount ' + gridcurrencyclass +'" value="' + item.CessAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="NetAmount ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
            + ' <td class="uk-text-center">'
            + '     <a class="remove-item">'
            + '         <i class="uk-icon-remove"></i>'
            + '     </a>'
            + ' </td>'
            + '</tr>';
    }
    $("#item-count").val(index);
    var $tr = $(tr);
    app.format($tr);
    $("#sales-order-items-list tbody").append($tr);
    self.calculate_grid_total();
};

SalesOrder.calculate_grid_total = function () {
    var GrossAmount = 0;
    var DiscountAmount = 0;
    var DiscountPercentage = 0;
    var TaxableAmount = 0;
    var SGSTAmount = 0;
    var CGSTAmount = 0;
    var IGSTAmount = 0;
    var VATAmount = 0;
    var CessAmount = 0;
    var NetAmount = 0;
    var RoundOff = 0;
    //var temp = 0;
    $("#sales-order-items-list tbody tr").each(function () {
        GrossAmount += clean($(this).find('.GrossAmount').val());
        DiscountAmount += clean($(this).find('.DiscountAmount').val());
        TaxableAmount += clean($(this).find('.TaxableAmount').val());
        SGSTAmount += clean($(this).find('.SGST').val());
        CGSTAmount += clean($(this).find('.CGST').val());
        IGSTAmount += clean($(this).find('.IGST').val());
        VATAmount += clean($(this).find('.VATAmount').val());
        CessAmount += clean($(this).find('.CessAmount').val());
        NetAmount += clean($(this).find('.NetAmount').val());
    });
    var FreightAmt = clean($("#FreightAmount").val());
    //NetAmount = NetAmount + FreightAmt;
    //temp = NetAmount;
    //NetAmount = Math.round(NetAmount);
    //RoundOff = NetAmount - temp;
    $("#GrossAmount").val(GrossAmount);
    $("#DiscAmount").val(DiscountAmount.roundToCustom());
    DiscountPercentage = (DiscountAmount / GrossAmount * 100).roundToCustom();
    $("#DiscPercentage").val(DiscountPercentage);
    $("#TaxableAmount").val(TaxableAmount.roundToCustom());
    $("#SGSTAmount").val(SGSTAmount.roundToCustom());
    $("#CGSTAmount").val(CGSTAmount.roundToCustom());
    $("#IGSTAmount").val(IGSTAmount.roundToCustom());
    $("#VATAmount").val(VATAmount.roundToCustom());
    $("#CessAmount").val(CessAmount.roundToCustom());
    $("#RoundOff").val(RoundOff.roundToCustom());
    $("#NetAmount").val(NetAmount.roundToCustom());
    $("#NetAmountValue").val(NetAmount.roundToCustom());
};
Number.prototype.roundToCustom = function () {

    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};