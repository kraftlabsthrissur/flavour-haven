var fh_items;
var gridcurrencyclass = 'mask-sales2-currency';
var DecPlaces = 0;
$(function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
});
SalesInquiry = {
    // Initializes the page
    init: function () {
        var self = SalesInquiry;
        self.item_list();
        self.bind_events();
        self.freeze_headers();
        $("#Offers").hide();
        self.purchase_order_history();
        self.pending_po_history();
        self.purchase_legacy_history();
        self.sales_order_history('Quotation');
        self.sales_invoice_history();
    },
    details: function () {
        var self = SalesInquiry;
        self.freeze_headers();
        $(".btnApprove").on("click", self.open_approval_details);
        $("#btnOkApprove").on('click', self.approve);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);

    },
    item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
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
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
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
                                + "<input type='hidden' class='Model' value='" + row.Model + ' / ' + row.Make + "'>"
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
                    }
                    ,
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
    showitemhistory: function () {
        var ItemID = $(this).closest('tr').find('.ItemID').val();
        $("#HistoryItemID").val(ItemID);
        $('#show-history').trigger('click');
    },
    purchase_order_history: function () {

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
    },
    pending_po_history: function () {

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
    },
    purchase_legacy_history: function () {

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
    },
    sales_order_history: function (type) {

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
    getCurrencyDetails: function () {
        var currency = {
            CurrencyID: $("#CurrencyID").val(),
            CurrencyName: $("#CurrencyName").val(),
            ExchangeRate: parseFloat($("#CurrencyExchangeRate").val()).toFixed(4),
            IsGST: $("#IsGST").val(),
            IsVat: $("#IsVat").val(),
            TaxTypeID: $("#TaxTypeID").val(),
            CountryID: $("#CountryID").val()
        };
        return currency;
    },
    freeze_headers: function () {
        fh_items = $("#sales-inquiry-items-list").FreezeHeader();
    },

    //Bind the events to elements  
    bind_events: function () {
        var self = SalesInquiry

        $.UIkit.autocomplete($('#requested-customer-autocomplete'), { 'source': self.get_inquery_customer, 'minLength': 1 });
        $('#requested-customer-autocomplete').on('selectitem.uk.autocomplete', self.set_inquery_customer);

        $("#btnOKItem").on("click", self.select_item);
        $("body").on("click", "#btnAddItem", self.confirm_add_item);
        $("body").on("click", "#btnAddNewItem", self.add_new_item);

        $(".btnSave").on("click", self.save_confirm);
        $(".btnSaveASDraft").on("click", self.save_draft);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("keyup change", "#sales-inquiry-items-list tbody .Qty, .MRP, .DiscountPercentage, .DiscountAmount", self.on_qty_change);
        //$("body").on("keyup change", "#sales-inquiry-items-list tbody tr .secondary .secondaryQty, .secondaryUnit, .secondaryMRP", self.on_secondary_unit_change);
        $('body').on('click', '#sales-inquiry-items-list tbody tr td.showitemhistory', self.showitemhistory);
    },
    on_qty_change: function () {
        var self = SalesInquiry;
        var row = $(this).closest('tr');
        self.update_item(row);
    },
    save_confirm: function () {
        var self = SalesInquiry;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            var IsDraft = false;
            self.save(IsDraft);
        }, function () {
        })
    },

    save_draft: function () {
        var self = SalesInquiry;
        self.error_count = 0;
        self.error_count = self.validate_draft();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveASDraft")) {
            IsDraft = true;
            self.save(IsDraft);
        }

    },

    open_approval_details: function () {
        var self = SalesInquiry;
        var color;
        //color = $(this).find(".color").val();
        //document.getElementById("#Name").style.color = "Green";
        $('#show-approval').trigger('click');
    },

    // updates item details on changing qty 
    update_item: function (row) {
        var self = SalesInquiry;

        var item = {};

        item.Category = $(row).find(".Category").val();

        var batchtype = $("#BatchType").val();
        if (item.Category == "Finished Goods") {
            item.BatchType = batchtype;
        }
        else {
            item.BatchType = "";
        }
        item.Qty = clean($(row).find(".Qty").val());
        item.MRP = clean($(row).find(".MRP").val());
        item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
        item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.CGSTPercentage = clean($(row).find(".CGSTPercentage").val());
        item.VATPercentage = clean($(row).find(".VAT").val());
        item.ID = clean($(row).find(".ItemID").val());
        item.UnitID = clean($(row).find(".UnitID").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.CessPercentage = clean($("#CessPercentage").val());
        item = self.get_item_properties(item);

        $(row).find(".OfferQty").val(item.OfferQty);
        $(row).find(".GrossAmount").val(item.GrossAmount);
        $(row).find(".DiscountAmount").val(item.DiscountAmount);
        $(row).find(".AdditionalDiscount ").val(item.AdditionalDiscount);
        $(row).find(".TaxableAmount").val(item.TaxableAmount);
        $(row).find(".CGST").val(item.CGST);
        $(row).find(".SGST").val(item.SGST);
        $(row).find(".IGST").val(item.IGST);
        $(row).find(".GSTAmount").val(item.GSTAmount);
        $(row).find(".VATAmount").val(item.VATAmount);
        $(row).find(".CessAmount").val(item.CessAmount);
        $(row).find(".NetAmount").val(item.NetAmount);
        $(row).find(".DiscountPercentage").val(item.DiscountPercentage);
        $(row).find(".BatchType").text(item.BatchType);
        $(row).find(".BasicPrice").val(item.BasicPrice);
        self.calculate_grid_total();
    },
    // removes item from the grid on clicking remove button
    remove_item: function () {
        var self = SalesInquiry;
        $(this).closest("tr").remove();
        $("#sales-inquiry-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(1).html(i + 1);
        });
        $("#item-count").val($("#sales-inquiry-items-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader(false);
    },

    // Selects the item on clicking the modal ok button 
    select_item: function () {
        var self = SalesInquiry;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {};
        item.id = radio.val();
        item.name = $(row).find(".Name").text().trim();
        item.code = $(row).find(".Code").text().trim();
        item.unit = $(row).find(".Unit").val();
        item.unitId = $(row).find(".UnitID").val();
        item.saleUnit = $(row).find(".SalesUnit").val();
        item.saleUnitId = $(row).find(".SalesUnitID").val();
        item.model = $(row).find(".Model").val();
        item.rate = $(row).find(".Rate").val();
        item.vatpercentage = $(row).find(".VATPercentage").val();
        item.partsnumber = $(row).find(".PartsNumber").val();
        self.on_select_item(item);

        UIkit.modal($('#select-item')).hide();
    },
    set_inquery_customer: function (event, item) {
        var self = SalesInquiry;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#PrimaryUnit").val(item.unit);
        $("#Code").val(item.code);
        $("#PartsNumber").val(item.partsnumber);
    },

    on_select_item: function (item) {
        var self = SalesInquiry;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#PrimaryUnit").val(item.unit);
        $("#Code").val(item.code);
        $("#PartsNumber").val(item.partsnumber);
        $("#PrimaryUnitID").val(item.unitId);
        $("#Stock").val(item.stock);
        $("#SalesUnit").val(item.saleUnit);
        $("#SalesUnitID").val(item.saleUnitId);
        $("#CGSTPercentage").val(item.cgstpercentage);
        $("#IGSTPercentage").val(item.igstpercentage);
        $("#SGSTPercentage").val(item.sgstpercentage);
        $("#CessPercentage").val(item.cessPercentage);
        $("#VATPercentage").val(item.vatpercentage);
        $("#Rate").val(item.rate);
        $("#Model").val(item.model);
        $("#Make").val(item.make);
        $("#Qty").focus();
        self.get_units();
    },

    get_units: function () {
        var self = SalesInquiry;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        $("#UnitID").append(html);
    },
    // Gets the items for auto complete
    get_inquery_customer: function (release) {
        var self = SalesInquiry;
        $.ajax({
            url: '/Sales/SalesInquiry/GetInqueryCustomerAutoComplete',
            data: {
                CustomerName: $('#RequestedCustomerName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    // on select auto complete customer
    set_customer: function (event, item) {
        var self = SalesInquiry;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#SchemeID").val(item.schemeId);
        $("#CurrencyID").val(item.CurrencyID);
        $("#CurrencyName").val(item.CurrencyName);
        $("#CurrencyCode").val(item.CurrencyCode);
        $("#CurrencyExchangeRate").val(item.CurrencyConversionRate);
        $("#CustomerCategoryID").val(item.customercategoryid);
        $("#CustomerCategory").val(item.customercategory);
        $("#DespatchDate").focus();

    },
    get_customers: function (release) {
        var self = SalesInquiry;

        $.ajax({
            url: '/Masters/Customer/GetSalesInquiryCustomersAutoComplete',
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
    // Clears the item details after adding it to grid 
    clear_item: function () {
        var self = SalesInquiry;
        $("#ItemID").val('');
        $("#Code").val('');
        $("#PartsNumber").val('');
        $("#PrimaryUnit").val('');
        $("#PrimaryUnitID").val('');
        $("#SalesUnit").val('');
        $("#SalesUnitID").val('');
        $("#Make").val('');
        $("#Model").val('');
        $("#TaxType").val('');
        $("#CessPercentage").val('');
        $("#Unit").val('');
        $("#UnitID").html('');
        $("#Stock").val('');
        $("#CGSTPercentage").val('');
        $("#IGSTPercentage").val('');
        $("#SGSTPercentage").val('');
        $("#DiscountPercentage").val('');
        $("#MaxSalesQtyFull").val('');
        $("#MinSalesQtyLoose").val('');
        $("#MinSalesQtyFull").val('');
        $("#MaxSalesQtyLoose").val('');
        $("#Qty").val('');
        $("#Rate").val(0);
        $("#Category").val('');
        $("#OfferQty").val('');
        $("#DiscPer").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);

    },
    confirm_add_item: function () {
        var self = SalesInquiry;

        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        var IsItemPreent = false;
        $("#sales-inquiry-items-list tbody tr").each(function () {
            if ($(this).find(".ItemID").val() == $("#ItemID").val()) {
                IsItemPreent = true;
            }
        });
        if (IsItemPreent) {
            app.confirm_cancel("Are you sure want to add this item again ?", function () {
                self.add_item();
            }, function () {
            })

        } else {
            self.add_item();
        }

    },
    add_new_item: function () {
        var self = SalesInquiry;
        var item = {};
        item.Name = "";
        item.ID = 0;
        item.Unit = "";
        item.Code = "";
        item.PartsNumber = "";
        item.UnitID = 0;
        item.Stock = 0;
        item.CGSTPercentage = 0;
        item.IGSTPercentage = 0;
        item.SGSTPercentage = 0;
        item.CessPercentage = 0;
        item.VATPercentage = 0;
        item.DiscountPercentage = 0;
        item.Qty = 1;
        item.FullRate = 0;
        item.LooseRate = 0;
        item.SaleUnitID = 0;
        item.Category = "";
        item.BatchType = "";
        item.BatchTypeID = 0;
        item.MRP = item.LooseRate;
        self.add_item_to_grid(item);
        fh_items.resizeHeader();
        //setTimeout(function () {
        //    $("#ItemName").focus();
        //}, 100);
    },
    add_item: function () {
        var self = SalesInquiry;
        self.error_count = 0;
        var item = {};
        item.Name = $("#ItemName").val();
        item.ID = $("#ItemID").val();
        item.Unit = $("#UnitID option:selected").text();
        item.Code = $("#Code").val();
        item.PartsNumber = $("#PartsNumber").val();
        item.UnitID = $("#UnitID").val();
        item.Stock = $("#Stock").val();
        item.CGSTPercentage = 0;
        item.IGSTPercentage = 0;
        item.SGSTPercentage = 0;
        item.CessPercentage = 0;
        item.VATPercentage = 0;
        item.DiscountPercentag = 0;
        item.Qty = clean($("#Qty").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#Rate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.Category = $("#Category").val();
        item.BatchType = "";
        item.BatchTypeID = 0;
        item.MRP = item.LooseRate;
        self.add_item_to_grid(item);
        fh_items.resizeHeader();
        self.clear_item();
        //setTimeout(function () {
        //    $("#ItemName").focus();
        //}, 100);
    },

    validate_form: function () {
        var self = SalesInquiry;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = SalesInquiry;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_add: function () {
        var self = SalesInquiry;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    // calculates and returns item properties
    get_item_properties: function (item) {
        var self = SalesInquiry;
        var CustomerCategory = $("#CustomerCategory").val();
        item.DiscountPercentage = 0;//$("#DiscPer").val()
        item.BasicPrice = item.MRP;
        item.BasicPrice = item.BasicPrice.roundTo(2);
        item.GrossAmount = item.BasicPrice * item.Qty;
        item.DiscountAmount = 0;// (item.Qty * item.BasicPrice * item.DiscountPercentage / 100).roundTo(2);
        item.AdditionalDiscount = 0;// (item.BasicPrice * item.OfferQty).roundTo(2);
        item.TaxableAmount = (item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount).roundTo(2);
        item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);
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
            item.CessAmount = 0;
            item.VATAmount = 0;
            item.CessPercentage = 0;
            item.GSTPercentage = item.IGSTPercentage;

            //if (!Sales.is_igst()) 
            {
                item.CGST = (item.GSTAmount / 2).roundTo(2);
                item.SGST = (item.GSTAmount / 2).roundTo(2);
                item.IGST = 0;
            }
            //else {
            //    item.CGST = 0;
            //    item.SGST = 0;
            //    item.IGST = item.GSTAmount;
            //}
        }
        item.NetAmount = item.TaxableAmount + item.CGST + item.IGST + item.SGST + item.CessAmount + item.VATAmount;

        return item;
    },
    //add validated items to grid
    add_item_to_grid: function (item) {
        var self = SalesInquiry;
        var index, GSTAmount, tr;
        var title = '';
        var readonly = '';
        var a = '';
        item = self.get_item_properties(item);
        index = $("#sales-inquiry-items-list tbody tr").length + 1;
        title += a;
        tr = '<tr  data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
            + (item.ID > 0 ? '<td class="uk-text-center showitemhistory action"><a class="uk-text-center action"><i class="uk-icon-eye-slash"></i></a></td>' : '<td></td>')
            + ' <td class="uk-text-center">' + index + ' </td>'
            + ' <td class="ItemCode">' + item.Code
            + '     <input type="hidden" class="ItemID" value="' + item.ID + '"  />'
            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
            + '</td>'
            + '<td><input type="text" class="md-input Name" value="' + item.Name + '" /></td>'
            + '<td><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '" /></td>'
            + '<td><input type="text" class="md-input Unit" value="' + item.Unit + '" /></td>'
            + '<td><input type="text" class="md-input DeliveryTerm" value="" /></td>'
            + '<td><input type="text" class="md-input Remarks" value="" /></td>'
            + '<td><input type="text" class="md-input MRP ' + gridcurrencyclass + '" value="' + item.MRP + '" /></td>'
            + '<td><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  /></td>'
            + '<td><input type="text" class="GrossAmount ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
            + '<td><input type="text" class="VATPercentage mask-currency" value="' + item.VATPercentage + '" readonly="readonly" /></td>'
            + '<td><input type="text" class="VATAmount ' + gridcurrencyclass + '" value="' + item.VATAmount + '" readonly="readonly" /></td>'
            + '<td><input type="text" class="NetAmount ' + gridcurrencyclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
            + '<td class="uk-text-center">'
            + '    <a class="remove-item">'
            + '        <i class="uk-icon-remove"></i>'
            + '    </a>'
            + '</td>'
            + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#sales-inquiry-items-list tbody").append($tr);
        self.calculate_grid_total();
    },

    calculate_grid_total: function () {
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var CessAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var temp = 0;
        $("#sales-inquiry-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            CessAmount += clean($(this).find('.CessAmount').val());
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        var FreightAmt = clean($("#FreightAmount").val());
        NetAmount = NetAmount + FreightAmt;
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = NetAmount - temp;
        $("#GrossAmount").val(GrossAmount);
        $("#DiscountAmount").val(DiscountAmount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#CessAmount").val(CessAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
    },
    list: function () {
        var self = SalesInquiry;
        $('body').on("click", '#sales-inquiry-items-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Sales/SalesInquiry/Details/' + id;
        });
        $('#tabs-sales-order').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = SalesInquiry;

        var $list;

        switch (type) {
            case "Draft":
                $list = $('#sales-inquiry-draft-list');
                break;
            case "SalesInquiry":
                $list = $('#sales-inquiry-table-list');
                break;
            case "Converted":
                $list = $('#sales-inquiry-converted-list');
                break;
            case "Cancelled":
                $list = $('#sales-inquiry-cancelled-list');
                break;
            default:
                $list = $('#sales-inquiry-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/SalesInquiry/GetSalesInquiryList?type=" + type,
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
                    { "data": "SalesInquiryNo", "className": "SalesInquiryNo" },
                    { "data": "SalesInquiryDate", "className": "SalesInquiryDate" },
                    { "data": "RequestedCustomerName", "className": "RequestedCustomerName" },
                    {
                        "data": null, "className": "PhoneNo",
                        "render": function (data, type, row, meta) {
                            return row.PhoneNo1 + ", " + row.PhoneNo2;
                        }
                    },
                    { "data": "RequestedCustomerAddress", "className": "RequestedCustomerAddress" },
                    { "data": "Remarks", "className": "Remarks" },
                    {
                        "data": null, "searchable": false, "className": "GrossAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.GrossAmount + "</div>";
                        }
                    },
                    {
                        "data": null, "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.NetAmount + "</div>";
                        }
                    }


                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/SalesInquiry/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;
        }
    },
    save: function (IsDraft) {
        var self = SalesInquiry;
        var data;
        var url = "/Sales/SalesInquiry/Save";

        data = self.get_data(IsDraft);
        if (IsDraft) {
            url = "/Sales/SalesInquiry/SaveAsDraft";
        }
        self.error_count = ((IsDraft == true) ? self.validate_draft() : self.validate_form());
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave, .btnSaveASDraft').css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Sales Order Saved Successfully");
                    setTimeout(function () {
                        window.location = "/Sales/SalesInquiry/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $('.btnSave, .btnSaveASDraft').css({ 'display': 'block' });

                }
            }
        });
    },

    get_data: function (IsDraft) {
        var item = {};
        var data = {
            ID: $("#ID").val(),
            SalesInquiryNo: $("#SalesInquiryNo").val(),
            SalesInquiryDate: $("#SalesInquiryDate").val(),
            RequestedDelivaryDate: $("#RequestedDelivaryDate").val(),
            RequestExpiryDate: $("#RequestExpiryDate").val(),
            RequestedCustomerName: $("#RequestedCustomerName").val(),
            RequestedCustomerAddress: $("#RequestedCustomerAddress").val(),
            PhoneNo1: $("#PhoneNo1").val(),
            PhoneNo2: $("#PhoneNo2").val(),
            Make: $("#Make").val(),
            Model: $("#Model").val(),
            Year: $("#Year").val(),
            SIOrVINNumber: $("#SIOrVINNumber").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            DiscountAmount: clean($("#DiscountAmount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            CurrencyExchangeRate: $("#CurrencyExchangeRate").val(),
            PaymentTerms: $("#PaymentTerms").val(),
            QuotationExpiry: $("#QuotationExpiry").val(),
            VATAmount: clean($("#VATAmount").val()),
            CessAmount: clean($("#CessAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            IsDraft: IsDraft
        };
        data.Items = [];
        var itmModel = $("#Make").val() + ' / ' + $("#Model").val();
        $("#sales-inquiry-items-list tbody tr").each(function () {
            item = {
                ItemID: $(this).find(".ItemID").val(),
                ItemCode: $(this).find(".ItemCode").text().trim(),
                ItemName: $(this).find(".Name").val(),
                UnitName: $(this).find(".Unit").val(),
                PartsNumber: $(this).find(".PartsNumber").val(),
                Year: $("#Year").val(),
                SIOrVINNumber: $("#SIOrVINNumber").val(),
                Model: itmModel,
                Remarks: $(this).find(".Remarks").val(),
                DeliveryTerm: $(this).find(".DeliveryTerm").val(),
                UnitID: $(this).find(".UnitID").val(),
                Rate: clean($(this).find(".MRP").val()),
                Qty: clean($(this).find(".Qty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").val()),
                VATPercentage: clean($(this).find(".VATPercentage").val()),
                VATAmount: clean($(this).find(".VATAmount").val()),
                CessAmount: clean($(this).find(".CessAmount").val()),
                NetAmount: clean($(this).find(".NetAmount").val())
            }
            data.Items.push(item);
        });
        return data;
    },
    approve: function () {
        $(".btnApprove").css({ 'display': 'none' });

        $.ajax({
            url: '/Sales/SalesInquiry/Approve',
            data: {
                SalesInquiryID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Successfully Approved");
                    setTimeout(function () {
                        window.location = "/Sales/SalesInquiry/Index";
                    }, 1000);
                } else {
                    app.show_error("Approve Failed");
                    $(".btnApprove").css({ 'display': 'block' });
                }
            },
        });
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
                elements: "#SONo",
                rules: [
                    { type: form.required, message: "Invalid Sales Order Number" },
                ]
            },
            {
                elements: "#SODate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: "#DespatchDate",
                rules: [
                    { type: form.required, message: "Dispatch Date is required" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#SODate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Dispatch Date should be a date on or after sales order date"
                    }
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
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
                elements: "#SONo",
                rules: [
                    { type: form.required, message: "Invalid Sales Order Number" },
                ]
            },
            {
                elements: "#SODate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },

            {
                elements: "#DespatchDate",
                rules: [
                    { type: form.required, message: "Dispatch Date is required" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#SODate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Dispatch Date should be a date on or after sales order date"
                    }
                ]
            },
            {
                elements: "#QuotationExpiry",
                rules: [
                    {
                        type: function (element) {
                            var val = $(element).val();

                            return val.length > 0 ? true : false;
                        }, message: "Quotation Expiry is required"
                    },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                ]
            },
        ],
    },


}