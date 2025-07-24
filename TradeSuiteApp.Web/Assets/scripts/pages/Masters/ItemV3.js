$(function () {

});
var a;
Item = {
    init: function () {
        var self = Item;
        self.bind_events();
        if ($('#ID').val() != 0) {
            self.GetItemLocationList();
        }
        //self.set_unit();
        //self.set_conversion_factor();
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
                                + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"
                                + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>"
                                + "<input type='hidden' class='SalesCategoryID' value='" + row.SalesCategoryID + "'>"
                                + "<input type='hidden' class='SalesCategory' value='" + row.SalesCategory + "'>"
                                + "<input type='hidden' class='LooseRate' value='" + row.LooseRate + "'>"
                                + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>"
                                + "<input type='hidden' class='MaxSalesQtyFull' value='" + row.MaxSalesQtyFull + "'>"
                                + "<input type='hidden' class='MinSalesQtyLoose' value='" + row.MinSalesQtyLoose + "'>"
                                + "<input type='hidden' class='MinSalesQtyFull' value='" + row.MinSalesQtyFull + "'>";
                            + "<input type='hidden' class='MaxSalesQtyLoose' value='" + row.MaxSalesQtyLoose + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Rate + "</div>";
                        }
                    },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    {
                        "data": "Stock", "className": "Stock", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Stock + "</div>";
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
            $('body').on("change", '#SalesCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#StoreID', function () {
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
    item_offer_list: function () {
        var $list = $('#offer-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetSaleableItemsList?type=OfferItem";
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
                                + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"
                                + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>"
                                + "<input type='hidden' class='LooseRate' value='" + row.LooseRate + "'>"
                                + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Rate + "</div>";
                        }
                    },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    {
                        "data": "Stock", "className": "Stock", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Stock + "</div>";
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
            $('body').on("change", '#SalesCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#StoreID', function () {
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
    list: function () {
        var $list = $('#tbl-item-list');
        $list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/Item/DetailsV3/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAllItemListV3";
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "asc"]],
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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Rate + "</div>";
                        }
                    },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    { "data": "AccountsCategory", "className": "AccountsCategory" },
                    {
                        "data": "stock", "className": "Stock", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Stock + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
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
    stockable_items_list: function () {

        var $list = $('#stockable-items-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetStockableItemsList";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },  // new, need to implement
    stockable_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetStockableItemsList?type=all";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='SalesCategory' value='" + row.SalesCategory + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    Purchase_report_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForReport";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategory').val() },
                             { Key: "Type", Value: $("#ItemType").val() },
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
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                    + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });

            $('body').on("change", '#ItemType', function () {
                list_table.fnDraw();
            });

            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    purchase_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForPurchase";

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
                            { Key: "ItemCategoryID", Value: $('#DDLItemCategory').val() },
                            { Key: "PurchaseCategoryID", Value: $('#DDLPurchaseCategory').val() },
                            { Key: "SupplierID", Value: $('#SupplierID').val() },
                            { Key: "Type", Value: $('#type').val() },

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
                                + "<input type='hidden' class='QtyUnderQC' value='" + row.QtyUnderQC + "'>"
                                + "<input type='hidden' class='QtyOrdered' value='" + row.QtyOrdered + "'>"
                                + "<input type='hidden' class='lastPr' value='" + row.LastPr + "'>"
                                + "<input type='hidden' class='lowestPr' value='" + row.LowestPr + "'>"
                                + "<input type='hidden' class='pendingOrderQty' value='" + row.PendingOrderQty + "'>"
                                + "<input type='hidden' class='qtyWithQc' value='" + row.QtyWithQc + "'>"
                                + "<input type='hidden' class='qtyAvailable' value='" + row.QtyAvailable + "'>"
                                + "<input type='hidden' class='gstPercentage' value='" + row.GstPercentage + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='finishedGoodsCategoryID' value='" + row.FinishedGoodsCategoryID + "'>"
                                + "<input type='hidden' class='TravelCategoryID' value='" + row.TravelCategoryID + "'>"
                                 + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='PurchaseUnit' value='" + row.PurchaseUnit + "'>"
                                + "<input type='hidden' class='PurchaseUnitID' value='" + row.PurchaseUnitID + "'>"
                                + "<input type='hidden' class='PurchaseCategoryID' value='" + row.PurchaseCategoryID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "PurchaseUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PurchaseCategory", "className": "PurchaseCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            $('body').on("change", '#DDLItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#DDLPurchaseCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#SupplierID', function () {
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
    stock_adjustment_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForStockAdjustment";

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
                            { Key: "ItemCategoryID", Value: $('#DDLItemCategory').val() },
                            { Key: "SalesCategoryID", Value: $('#DDLSalesCategory').val() }

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
                                + "<input type='hidden' class='SalesCategoryID' value='" + row.SalesCategoryID + "'>"
                                + "<input type='hidden' class='QtyUnderQC' value='" + row.QtyUnderQC + "'>"
                                + "<input type='hidden' class='QtyOrdered' value='" + row.QtyOrdered + "'>"
                                + "<input type='hidden' class='lastPr' value='" + row.LastPr + "'>"
                                + "<input type='hidden' class='lowestPr' value='" + row.LowestPr + "'>"
                                + "<input type='hidden' class='pendingOrderQty' value='" + row.PendingOrderQty + "'>"
                                + "<input type='hidden' class='qtyWithQc' value='" + row.QtyWithQc + "'>"
                                + "<input type='hidden' class='qtyAvailable' value='" + row.QtyAvailable + "'>"
                                + "<input type='hidden' class='gstPercentage' value='" + row.GstPercentage + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='finishedGoodsCategoryID' value='" + row.FinishedGoodsCategoryID + "'>"
                                + "<input type='hidden' class='TravelCategoryID' value='" + row.TravelCategoryID + "'>"
                                 + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='PurchaseUnit' value='" + row.PurchaseUnit + "'>"
                                + "<input type='hidden' class='PurchaseUnitID' value='" + row.PurchaseUnitID + "'>"
                                + "<input type='hidden' class='PurchaseCategoryID' value='" + row.PurchaseCategoryID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "PurchaseUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            //$('body').on("change", '#DDLItemCategory', function () {
            //    list_table.fnDraw();
            //});
            $('body').on("change", '#DDLSalesCategory', function () {
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
    saleable_service_items_list: function () {
        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForSaleableServiceItem";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
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
                                + "<input type='hidden' class='gstPercentage' value='" + row.GstPercentage + "'>"
                                + "<input type='hidden' class='CessPercentage' value='" + row.CessPercentage + "'>"
                                + "<input type='hidden' class='MRP' value='" + row.MRP + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>"
                                + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "SalesUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ItemCategoryID', function () {
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

    offical_advance_items_list: function () {
        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsForOfficialAdvance";

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
                            { Key: "Type", Value: $('#AdvanceCategory').val() },
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
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#AdvanceCategory', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    debit_and_credit_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetDebitAndCreditItemList";

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
                              { Key: "Type", Value: $('#ItemType').val() },
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
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ItemID + "' >"
                                + "<input type='hidden' class='Type' value='" + row.Type + "'>"
                                + "<input type='hidden' class='CGSTPercentage' value='" + row.CGSTPercentage + "'>"
                                + "<input type='hidden' class='SGSTPercentage' value='" + row.SGSTPercentage + "'>"
                                + "<input type='hidden' class='IGSTPercentage' value='" + row.IGSTPercentage + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Unit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PurchaseCategory", "className": "PurchaseCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            $('body').on("change", '#DDLItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#DDLPurchaseCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#SupplierID', function () {
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

    packing_rerurn_item_list: function () {

        var $list = $('#material-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetStockableItemsList";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                    + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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
    material_item_list: function () {

        var $list = $('#material-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs();
            //    var url = "/Masters/Item/GetReturnItemList";
            var url = "/Masters/Item/GetStockableItemsList?type=return-items";

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
                            { Key: "ProductGroupID", Value: $('#ProductGroupID').val() },
                            { Key: "IssueItemID", Value: $("#IssueItemID").val() },
                            { Key: "ReceiptItemID", Value: $("#ReceiptItemID").val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='Unit' value='" + row.PrimaryUnit + "'>"
                                    + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.PrimaryUnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            $('body').on("change", '#ProductGroupID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#IssueItemID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ReceiptItemID', function () {
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
    available_stock_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAvailableStockItemsList";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "WarehouseID", Value: $('#WarehouseID').val() },
                            { Key: "BatchTypeID", Value: $('#BatchTypeID').val() },
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
                                + "<input type='hidden' class='BatchTypeID' value='" + row.BatchTypeID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                                + "<input type='hidden' class='CGSTPercentage' value='" + row.CGSTPercentage + "'>"
                                + "<input type='hidden' class='IGSTPercentage' value='" + row.IGSTPercentage + "'>"
                                + "<input type='hidden' class='SGSTPercentage' value='" + row.SGSTPercentage + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                         {
                             "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                                 return "<div class='mask-currency' >" + row.Rate + "</div>";
                             }
                         },
                      {
                          "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                              return "<div class='mask-currency' >" + row.Stock + "</div>";
                          }
                      },
                ]
                ,
                createdRow: function (row, data, index) {
                    $(row).addClass((data.BatchTypeName).toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on('change', '#ItemCategoryID, #WarehouseID, #BatchTypeID', function () {
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
    production_packing_item_list: function () {

        var $list = $('#production-group-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetPackingItemList";

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
                             + "<input type='hidden' class='ProductID' value='" + row.ProductID + "'>"
                             + "<input type='hidden' class='ProductionGroupID' value='" + row.ProductionGroupID + "'>"
                             + "<input type='hidden' class='BatchSize' value='" + row.BatchSize + "'>"
                             + "<input type='hidden' class='ConversionFactorPtoS' value='" + row.ConversionFactorPtoS + "'>"
                             + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "BatchSize", "className": "" },

                    { "data": "Unit", "className": "Unit" },
                    { "data": "Category", "className": "Category" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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
    production_group_item_list: function () {

        var $list = $('#production-group-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetProductionGroupItemList";

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
                                  { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
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
                            + "<input type='hidden' class='ProductionItemID' value='" + row.ItemID + "'>"
                            + "<input type='hidden' class='IsKalkan' value='" + row.IsKalkan + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "StdBatchSize", "className": "StdBatchSize" },

                    { "data": "Unit", "className": "Unit", "searchable": false, },
                    { "data": "Category", "className": "Category" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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
    raw_material_list: function () {
        var $list = $('#raw-material-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetRawMaterialList";

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
                            { Key: "WarehouseID", Value: $('#WarehouseID').val() },
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
                            + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"
                            + "<input type='hidden' class='BatchTypeID' value='" + row.BatchTypeID + "'>"
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Unit", "className": "Unit", "searchable": false, },
                    { "data": "Category", "className": "Category" }
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass((data.BatchTypeName).toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on("change", "#WarehouseID", function () {
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
    packing_material_list: function () {
        var $list = $('#packing-material-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetPackingMaterialList";

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
                            + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Unit", "className": "Unit", "searchable": false, },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on("change", "#WarehouseID", function () {
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
    material_return_production_issue_list: function (productionID) {
        var $list = $('#material-return-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetProductionIssueMaterialReturnList";

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
                            { Key: "productionID", Value: productionID },
                            { Key: "productionSequence", Value: ($("#SequenceItemID option:selected").data("production-sequence") == undefined) ? 1 : $("#SequenceItemID option:selected").data("production-sequence") },
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
                            + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>"

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Unit", "className": "Unit", "searchable": false, },
                    { "data": "Category", "className": "Category" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on("change", "#SequenceItemID", function () {
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

    grn_wise_item_list: function () {
        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetGRNWiseItemsList";
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
                             { Key: "SupplierID", Value: $('#SupplierID').val() },
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
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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
    preprocess_issue_item_list: function () {

        var $list = $('#preprocess-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetPreProcessIssueItemsList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [

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
                                + "<input type='hidden' class='ProcessID' value='" + row.ProcessID + "'>"
                                + "<input type='hidden' class='Stock' value='" + row.Stock + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Unit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "Activity", "className": "Activity" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

    treatment_item_list: function () {

        var $list = $('#treatment-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemListForTreatment";

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
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >"
                             + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>"
                             + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>"
                             + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                             + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                             + "<input type='hidden' class='CategoryID' value='" + row.CategoryID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

    material_purification_item_list: function () {

        var $list = $('#material-purification-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForMaterialPurification";
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
                            { Key: "ItemCategoryID", Value: $('#DDLItemCategory').val() },
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
                                + "<input type='hidden' class='QtyUnderQC' value='" + row.QtyUnderQC + "'>"
                                + "<input type='hidden' class='QtyOrdered' value='" + row.QtyOrdered + "'>"
                                + "<input type='hidden' class='lastPr' value='" + row.LastPr + "'>"
                                + "<input type='hidden' class='lowestPr' value='" + row.LowestPr + "'>"
                                + "<input type='hidden' class='pendingOrderQty' value='" + row.PendingOrderQty + "'>"
                                + "<input type='hidden' class='qtyWithQc' value='" + row.QtyWithQc + "'>"
                                + "<input type='hidden' class='qtyAvailable' value='" + row.QtyAvailable + "'>"
                                + "<input type='hidden' class='gstPercentage' value='" + row.GstPercentage + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='finishedGoodsCategoryID' value='" + row.FinishedGoodsCategoryID + "'>"
                                + "<input type='hidden' class='TravelCategoryID' value='" + row.TravelCategoryID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='PurchaseUnit' value='" + row.PurchaseUnit + "'>"
                                + "<input type='hidden' class='PurchaseUnitID' value='" + row.PurchaseUnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "PurchaseUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PurchaseCategory", "className": "PurchaseCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            $('body').on("change", '#DDLItemCategory', function () {
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

    production_definition_item_list: function () {

        var $list = $('#production-definition-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetStockableItemsList?type=materials";

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
                            { Key: "ItemCategoryID", Value: $('#CategoryID').val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

    production_definition_material_list: function () {

        var $list = $('#production-definition-material-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetProductionDefinitionMaterialList";

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
                            { Key: "Type", Value: $('.Type').val() },
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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },


    SaleableServiceAndStock_items_list: function () {
        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForSaleableServiceAndStockItem";

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
                            { Key: "SalesCategoryID", Value: $('#SalesCategoryID').val() },
                            { Key: "SalesIncentiveCategoryID", Value: $('#SalesIncentiveCategoryID').val() },
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
                                + "<input type='hidden' class='salesCategoryID' value='" + row.SalesCategoryID + "'>"
                                + "<input type='hidden' class='salesIncentiveCategoryID' value='" + row.SalesIncentiveCategoryID + "'>"
                                + "<input type='hidden' class='businessCategoryID' value='" + row.BusinessCategoryID + "'>"
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    { "data": "SalesIncentiveCategory", "className": "SalesIncentiveCategory" },
                    { "data": "BusinessCategory", "className": "BusinessCategory" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#SalesCategoryID,#SalesIncentiveCategoryID,#BusinessCategoryID', function () {
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

    //---------------
    all_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAllItemList";

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
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "WarehouseID", Value: $('#WarehouseID').val() },
                        ];
                    }
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center ",
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
                                + "<input type='hidden' class='Unit' value='" + row.Unit + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                                + "<input type='hidden' class='UnitID' value='" + row.UnitID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                //    { "data": "PurchaseCategory", "className": "PurchaseCategory" }
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on('change', '#ItemCategoryID, #WarehouseID', function () {
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
    //---------------
    bind_events: function () {
        var self = Item;
        $('#ItemTypeID').on('change', self.GetItemCode)
        $('#PurchaseUnitID , #InventoryUnitID').on('change', self.GetPackSize)
        
        $(".BtnSave").on("click", self.save_confirm);
        $(".BtnSaveAsDraft").on("click", self.save);
        $("#CategoryID").on("change", self.set_unit);
        if ($('#ID').val() > 0) {
            self.ItemDetails();
        }

        $("body").on("ifChanged", ".IsDisContinued", self.check_isdisContinued);
        $("body").on("ifChanged", ".Isactive", self.check_isactive);
        $(".ItemLocation").on('ifChanged', self.get_Location_data);
        $(document).on('keyup', '#PackSize', self.set_conversion_factor)
    },

    set_conversion_factor: function (release) {
        var self = Item;
        var intRegex = /^\d+$/;
        var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;

        var packsize = $("#PackSize").val();
        if (intRegex.test(packsize) || floatRegex.test(packsize)) {
            var packsize = $("#PackSize").val();
            var Cfactor = (parseInt(1) / parseInt(packsize)).toFixed(3);
            $("#ConversionFactorPtoI").val(Cfactor);
        }
        else {
            $("#ConversionFactorPtoI").val("");
        }
    },
    save_confirm: function () {
        var self = Item;
        self.error_count = 0;
        var FormType = "Form";
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Masters/Item/IsItemExist',
            data: {
                Name: $("#Name").val(),
                HSNCode: $("#HSNCode").val(),
                ID: clean($('#ID').val())
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.confirm_cancel(response.data, function () {
                        self.save();
                    }, function () {
                        $('.BtnSave').css({ 'visibility': 'visible' });
                    })
                }
            },
        });
    },

    set_unit: function () {
        var id = clean($("#ID").val());
        if (id == 0) {
            $("#Code").val($("#CategoryID option:selected").data("code"));
        }
        if ($("#CategoryID option:selected").data("type") == "Service") {
            $("#SecondaryUnitID").val($("#SecondaryUnitID option:contains(Nos)").val());
            $("#InventoryUnitID").val($("#InventoryUnitID option:contains(Nos)").val());
            $("#UnitID").val($("#UnitID option:contains(Nos)").val());
            $("#SalesUnitID").val($("#SalesUnitID option:contains(Nos)").val());
            $("#PurchaseUnitID").val($("#PurchaseUnitID option:contains(Nos)").val());
            //$("#ConversionFactorPtoI").val(1);
            $("#ConversionFactorPurchaseToInventory").val(1);
            $("#ConversionFactorPtoSecondary").val(1);
            $("#ConversionFactorPtoS").val(1);
            $("#ConversionFactorSalesToInventory").val(1);
        }

        else {
            $("#SecondaryUnitID").val('');
            $("#InventoryUnitID").val('');
            $("#UnitID").val('');
            $("#SalesUnitID").val('');
            $("#PurchaseUnitID").val('');
            //$("#ConversionFactorPtoI").val(0.00);
            $("#ConversionFactorPurchaseToInventory").val(0.00);
            $("#ConversionFactorPtoSecondary").val(0.00);
            $("#ConversionFactorPtoS").val(0.00);
            $("#ConversionFactorSalesToInventory").val(0.00);
        }

    },
    ItemDetails: function () {
        $('#IsStockItem').val() == "True" ? $('.IsStockItem').iCheck('check') : $('.IsStockItem').iCheck('uncheck');
        $('#Isactive ').val() == "True" ? $('.Isactive ').iCheck('check') : $('.Isactive ').iCheck('uncheck');
        $('#IsStockValue').val() == "True" ? $('.IsStockValue').iCheck('check') : $('.IsStockValue').iCheck('uncheck');
        $('#IsDemandPlanRequired').val() == "True" ? $('.IsDemandPlanRequired').iCheck('check') : $('.IsDemandPlanRequired').iCheck('uncheck');
        $('#IsMaterialPlanRequired').val() == "True" ? $('.IsMaterialPlanRequired').iCheck('check') : $('.IsMaterialPlanRequired').iCheck('uncheck');
        $('#IsPhantomItem').val() == "True" ? $('.IsPhantomItem').iCheck('check') : $('.IsPhantomItem').iCheck('uncheck');
        $('#IsSaleable').val() == "True" ? $('.IsSaleable').iCheck('check') : $('.IsSaleable').iCheck('uncheck');
        $('#N2GActivity').val() == "True" ? $('.N2GActivity').iCheck('check') : $('.N2GActivity').iCheck('uncheck');
        $('#IsMrp').val() == "True" ? $('.IsMrp').iCheck('check') : $('.IsMrp').iCheck('uncheck');
        $('#IsPriceListReference').val() == "True" ? $('.IsPriceListReference').iCheck('check') : $('.IsPriceListReference').iCheck('uncheck');
        $('#IsQCRequired').val() == "True" ? $('.IsQCRequired').iCheck('check') : $('.IsQCRequired').iCheck('uncheck');
        $('#IsQCRequiredForProduction').val() == "True" ? $('.IsQCRequiredForProduction').iCheck('check') : $('.IsQCRequiredForProduction').iCheck('uncheck');
        $('#IsProprietary').val() == "True" ? $('.IsProprietary').iCheck('check') : $('.IsProprietary').iCheck('uncheck');
        $('#IsPurchaseItem').val() == "True" ? $('.IsPurchaseItem').iCheck('check') : $('.IsPurchaseItem').iCheck('uncheck');
        $('#IsSeasonalPurchase').val() == "True" ? $('.IsSeasonalPurchase').iCheck('check') : $('.IsSeasonalPurchase').iCheck('uncheck');
        $('#IsPORequired').val() == "True" ? $('.IsPORequired').iCheck('check') : $('.IsPORequired').iCheck('uncheck');
        $('#IsAsset').val() == "True" ? $('.IsAsset').iCheck('check') : $('.IsAsset').iCheck('uncheck');
        $('#IsProject').val() == "True" ? $('.IsProject').iCheck('check') : $('.IsProject').iCheck('uncheck');
        $('#IsEmployee').val() == "True" ? $('.IsEmployee').iCheck('check') : $('.IsEmployee').iCheck('uncheck');
        $('#IsDepartment').val() == "True" ? $('.IsDepartment').iCheck('check') : $('.IsDepartment').iCheck('uncheck');
        $('#IsInterCompany').val() == "True" ? $('.IsInterCompany').iCheck('check') : $('.IsInterCompany').iCheck('uncheck');
        $('#IsLocation').val() == "True" ? $('.IsLocation').iCheck('check') : $('.IsLocation').iCheck('uncheck');
        $('#IsMasterFormula').val() == "True" ? $('.IsMasterFormula').iCheck('check') : $('.IsMasterFormula').iCheck('uncheck');
        $('#IsReProcessAllowed').val() == "True" ? $('.IsReProcessAllowed').iCheck('check') : $('.IsReProcessAllowed').iCheck('uncheck');
        $('#IsBatch').val() == "True" ? $('.IsBatch').iCheck('check') : $('.IsBatch').iCheck('uncheck');
        $('#IsDisContinued').val() == "True" ? $('.IsDisContinued').iCheck('check') : $('.IsDisContinued').iCheck('uncheck');
        //  $('#UnitID').val($('#UnitOMID').val());
        $("#ItemTypeID").attr("disabled", "disabled");
    },
    GetPackSize: function () {
        var self = Item;
        var looseunit = $("#InventoryUnitID option:selected").text();
        var fullunit = $("#PurchaseUnitID option:selected").text();
        var loosepacksize = $("#InventoryUnitID option:selected").data("uom")
        var fullpacksize = $("#PurchaseUnitID option:selected").data("uom")
        var ConversionFactorPurchaseToInventory = fullpacksize
        if (looseunit == fullunit) {
            $("#PackSize").val(1);
            $("#ConversionFactorPurchaseToInventory").val(1);
        }
        else {
            $("#PackSize").val(fullpacksize);
            $("#ConversionFactorPurchaseToInventory").val(ConversionFactorPurchaseToInventory);
        }
        //$('#PackSize').val($("#UnitID option:selected").data("uom"));
    },
    save: function (event) {
        var self = Item;
        var IsDraft, model = self.get_data()
        this.id == "BtnSaveAsDraft" ? IsDraft = true : IsDraft = false;
        model.IsDraft = IsDraft;

        var SalesModel = self.get_sales_data();
        var QCModel = self.get_QC_data();
        var CostModel = self.get_cost_data();
        var PurchaseModel = self.get_purchase_data();
        var AccountsModel = self.get_accounts_data();
        var ProductionModel = self.get_production_data();
        $.extend(model, SalesModel, QCModel, CostModel, PurchaseModel, AccountsModel, ProductionModel);
        model.ItemLocationList = self.LocationList
        $(".BtnSave").css({ 'visibility': 'hidden' });
        $(".BtnSaveAsDraft").css({ 'visibility': 'hidden' });
        $.ajax({
            url: '/Masters/Item/SaveV3',
            data: { model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Item created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Item/IndexV3"
                    }, 1000);
                } else {
                    app.show_error("Failed to create Item");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".BtnSaveAsDraft").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: clean($('#ID').val()),
            Name: $('#Name').val(),
            MalayalamName: $('#MalayalamName').val(),
            HindiName: $('#HindiName').val(),
            SanskritName: $('#SanskritName').val(),
            StorageCategoryID: clean($('#StorageCategoryID').val()),
            ItemTypeID: clean($('#ItemTypeID').val()),
            Code: $('#Code').val(),
            OldItemCode: $('#OldItemCode').val(),
            OldItemCode2: $('#OldItemCode2').val(),
            HSNCode: $('#HSNCode').val(),
            BarCode: $('#BarCode').val(),
            QRCode: $('#QRCode').val(),
            Description: $('#Description').val(),
            CategoryID: clean($('#CategoryID').val()),
            BusinessCategoryID: clean($('#BusinessCategoryID').val()),
            SecondaryUnitID: clean($('#InventoryUnitID').val()),
            InventoryUnitID: clean($('#InventoryUnitID').val()),
            //PurchaseUnitID: clean($('#PurchaseUnitID').val()),
            //UnitID: clean($('#UnitID').val().split('#')[0]),
            PackSize: clean($('#PackSize').val()),
            ConversionFactorPtoI: clean($('#ConversionFactorPtoI').val()),
            MinStockQTY: clean($('#MinStockQTY').val()),
            MaxStockQTY: clean($('#MaxStockQTY').val()),
            BirthDate: $('#BirthDate').val(),
            DisContinuedDate: $('#DisContinuedDate').val(),
            ItemTypeName: $("#ItemTypeID option:selected").text(),
            OldName: $("#OldName").val(),
            CategoryName: $('#CategoryID option:selected').text()

        };
        $('.IsStockItem').prop("checked") == true ? model.IsStockItem = true : model.IsStockItem = false;
        $('.IsStockValue').prop("checked") == true ? model.IsStockValue = true : model.IsStockValue = false;
        $('.IsDemandPlanRequired').prop("checked") == true ? model.IsDemandPlanRequired = true : model.IsDemandPlanRequired = false;
        $('.IsMaterialPlanRequired').prop("checked") == true ? model.IsMaterialPlanRequired = true : model.IsMaterialPlanRequired = false;
        $('.IsPhantomItem').prop("checked") == true ? model.IsPhantomItem = true : model.IsPhantomItem = false;
        if (clean($('#ID').val()) > 0) {
            $('.IsDisContinued').prop("checked") == true ? model.IsDisContinued = true : model.IsDisContinued = false;
        } else {
            model.IsDisContinued = false;
        }
        $('.Isactive ').prop("checked") == true ? model.Isactive = true : model.Isactive = false;
        return model;
    },
    get_sales_data: function () {
        var model = {
            SalesIncentiveCategoryID: clean($('#SalesIncentiveCategoryID').val()),
            SalesCategoryID: clean($('#SalesCategoryID').val()),
            DrugScheduleID: clean($('#DrugScheduleID').val()),
            SalesUnitID: clean($('#SalesUnitID').val()),
            ConversionFactorPtoS: clean($('#ConversionFactorPtoS').val()),
            MinSalesQtyFull: clean($('#MinSalesQtyFull').val()),
            MinSalesQtyLoose: clean($('#MinSalesQtyLoose').val()),
            MaxSalesQty: clean($('#MaxSalesQty').val()),
            DiseaseCategoryID: clean($('#DiseaseCategoryID').val()),
            SeasonStarts: $('#SeasonStarts').val(),
            SeasonEnds: $('#SeasonEnds').val(),
            ConversionFactorSalesToInventory: clean($('#ConversionFactorSalesToInventory').val()),
            ConversionFactorPurchaseToInventory: ($('#ConversionFactorPurchaseToInventory').val()),
        };
        $('.IsSaleable').prop("checked") == true ? model.IsSaleable = true : model.IsSaleable = false;
        $('.N2GActivity').prop("checked") == true ? model.N2GActivity = true : model.N2GActivity = false;
        $('.IsMrp').prop("checked") == true ? model.IsMrp = true : model.IsMrp = false;
        $('.IsPriceListReference').prop("checked") == true ? model.IsPriceListReference = true : model.IsPriceListReference = false;
        return model;
    },
    get_QC_data: function () {
        var model = {
            BotanicalName: $('#BotanicalName').val(),
            QCCategoryID: clean($('#QCCategoryID').val()),
            PatentNo: $('#PatentNo').val(),
            ConversionFactorPtoSecondary: clean($('#ConversionFactorPtoSecondary').val()),
        };
        $('.IsQCRequired').prop("checked") == true ? model.IsQCRequired = true : model.IsQCRequired = false;
        $('.IsQCRequiredForProduction').prop("checked") == true ? model.IsQCRequiredForProduction = true : model.IsQCRequiredForProduction = false;
        $('.IsProprietary').prop("checked") == true ? model.IsProprietary = true : model.IsProprietary = false;

        return model;
    },
    get_cost_data: function () {
        var model = {
            CostingCategoryID: clean($('#CostingCategoryID').val())
        };
        return model;
    },
    get_purchase_data: function () {
        var model = {
            PurchaseCategoryID: clean($('#PurchaseCategoryID').val()),
            PurchaseUnitID: clean($('#PurchaseUnitID').val()),
            ReOrderLevel: clean($('#ReOrderLevel').val()),
            MinPurchaseQTY: clean($('#MinPurchaseQTY').val()),
            MaxPurchaseQTY: clean($('#MaxPurchaseQTY').val()),
            QtyTolerancePercent: clean($('#QtyTolerancePercent').val()),
            ReOrderQty: clean($('#ReOrderQty').val()),
            SeasonPurchaseStarts: $('#SeasonPurchaseStarts').val(),
            SeasonPurchaseEnds: $('#SeasonPurchaseEnds').val(),
            PurchaseLeadTime: clean($('#PurchaseLeadTime').val()),
            ConversionFactorPurchaseToInventory: clean($('#ConversionFactorPurchaseToInventory').val()),
            ManufacturerID: clean($('#ManufacturerID').val())
        };
        $('.IsPurchaseItem').prop("checked") == true ? model.IsPurchaseItem = true : model.IsPurchaseItem = false;
        $('.IsSeasonalPurchase').prop("checked") == true ? model.IsSeasonalPurchase = true : model.IsSeasonalPurchase = false;
        $('.IsPORequired').prop("checked") == true ? model.IsPORequired = true : model.IsPORequired = false;
        return model;
    },
    get_accounts_data: function () {
        var model = {
            AssetCategoryID: clean($('#AssetCategoryID').val()),
            GSTCategoryID: clean($('#GSTCategoryID').val()),
            AccountsCategoryID: clean($('#AccountsCategoryID').val()),
            GSTSubCategoryID: clean($('#GSTSubCategoryID').val())
        };
        $('.IsLocation').prop("checked") == true ? model.IsLocation = true : model.IsLocation = false;
        $('.IsInterCompany').prop("checked") == true ? model.IsInterCompany = true : model.IsInterCompany = false;
        $('.IsDepartment').prop("checked") == true ? model.IsDepartment = true : model.IsDepartment = false;
        $('.IsEmployee').prop("checked") == true ? model.IsEmployee = true : model.IsEmployee = false;
        $('.IsProject').prop("checked") == true ? model.IsProject = true : model.IsProject = false;
        $('.IsAsset').prop("checked") == true ? model.IsAsset = true : model.IsAsset = false;

        return model;
    },
    get_production_data: function () {
        var model = {
            ProductionCategoryID: clean($('#ProductionCategoryID').val()),
            MasterFormulaRefNo: clean($('#MasterFormulaRefNo').val()),
            NormalLossQty: clean($('#NormalLossQty').val()),
            NormalLossPercent: clean($('#NormalLossPercent').val()),
            ProductLeadDays: clean($('#ProductLeadDays').val()),
            BatchSizeQTY: clean($('#BatchSizeQTY').val()),
            ShelfLifeMonths: clean($('#ShelfLifeMonths').val()),
            ProductionGroup: $('#Name').val(),
        };
        $('.IsMasterFormula').prop("checked") == true ? model.IsMasterFormula = true : model.IsMasterFormula = false;
        $('.IsReProcessAllowed').prop("checked") == true ? model.IsReProcessAllowed = true : model.IsReProcessAllowed = false;
        $('.IsBatch').prop("checked") == true ? model.IsBatch = true : model.IsBatch = false;
        return model;
    },

    LocationList: [],

    get_Location_data: function () {
        var self = Item;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Item-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Item-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },

    GetItemCode: function () {
        if ($(this).val() != 0) {
            $.ajax({
                url: '/Masters/Item/GetItemCodeByItemType',
                dataType: "json",
                type: "GET",
                data: {
                    Form: $("#ItemTypeID option:selected").text()
                },
                success: function (response) {
                    $('#Code').val(response);
                }
            });
        }
    },

    check_isdisContinued: function () {
        var self = Item;
        if ($(".IsDisContinued").prop('checked') == true) {
            $(".Isactive").prop("checked", false)
            $(".Isactive").closest('div').removeClass("checked")
        }
    },

    check_isactive: function () {
        var self = Item;
        if ($(".Isactive").prop('checked') == true) {
            $(".IsDisContinued").prop("checked", false)
            $(".IsDisContinued").closest('div').removeClass("checked")
        }
    },

    GetItemLocationList: function () {
        var self = Item;
        var ItemID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Item/GetItemLocationMapping',
            data: { ItemID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $ItemLocationDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].LocationName + "'> </div>"
                    app.format($ItemLocationDetail);
                    $('#Location-Detail-Container').append($ItemLocationDetail);
                    $('.ItemLocation').each(function () {
                        if ($(this).val() == data[i].LocationID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });
    },

    validate_form: function () {
        var self = Item;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#Name",
                rules: [
                   { type: form.required, message: "Item Name is required" },
                ]
            },
                 {
                     elements: "#CategoryID",
                     rules: [
                        { type: form.non_zero, message: "Item category is required" },
                     ]
                 },
                 {
                     elements: "#PackSize",
                     rules: [
                        { type: form.non_zero, message: "PackSize is required" },
                     ]
                 },
            //{
            //    elements: "#MalayalamName",
            //    rules: [
            //       {
            //           type: function (element) {
            //               var count = 0;
            //               var category = $("#CategoryID option:selected").text();
            //               if ($(element).val() == "") {
            //                   if (category == "Cleaned Raw Materials" || category == "Finished Goods" || category == "Raw Materials" || category == "Semifinished Goods" && $(element).val() == "")
            //                       count++;
            //               }
            //               return count == 0;

            //           }, message: 'Malayalam Name is required'
            //       },
            //    ]
            //},
           //{
           //    elements: "#StorageCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Cleaned Raw Materials" || category == "Finished Goods" || category == "Raw Materials" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Storage category is required'
           //       },
           //    ]
           //},
           {
               elements: "#PackSize",
               rules: [
                  {
                      type: function (element) {
                          var count = 0;
                          var category = $("#CategoryID option:selected").text();
                          if ($(element).val() == "0") {
                              if (category == "Finished Goods")
                                  count++;
                          }
                          return count == 0;

                      }, message: 'PackSize is required'
                  },
               ]
           },

           //{
           //    elements: "#ConversionFactorPtoS",
           //    rules: [
           //       { type: form.required, message: "Conversion factor production to sales is required" },
           //       { type: form.non_zero, message: "Please enter a valid Conversion factor production to sales" },
           //       { type: form.positive, message: "Please enter a valid Conversion factor production to sales" },
           //    ]
           //},
           //{
           //    elements: "#ConversionFactorPurchaseToInventory",
           //    rules: [
           //       { type: form.required, message: "Conversion factor purchase to inventory is required" },
           //       { type: form.non_zero, message: "Please enter a valid Conversion factor purchase to inventory" },
           //       { type: form.positive, message: "Please enter a valid Conversion factor purchase to inventory" },
           //    ]
           //},
           //{
           //    elements: "#ConversionFactorSalesToInventory",
           //    rules: [
           //       { type: form.required, message: "Conversion factor sales to inventory is required" },
           //       { type: form.non_zero, message: "Please enter a valid Conversion factor sales to inventory" },
           //       { type: form.positive, message: "Please enter a valid Conversion factor sales to inventory" },
           //    ]
           //},
           {
               elements: "#HSNCode",
               rules: [
                  {
                      type: function (element) {
                          var count = 0;
                          var category = $("#CategoryID option:selected").text();
                          if ($(element).val() == "") {
                              if (category == "Sales & Marketing Services" || category == "Supply Chain Services" || category == "Travel Services" || category == "Treatment Services" || category == "Assets_Admin" || category == "Accounts Services" || category == "Assets_Finance" || category == "Assets_IT" || category == "Assets_Production" || category == "Common Services" || category == "Corporate Services" || category == "Factory Services" || category == "Finance Services" || category == "Finished Goods" || category == "HR & Admin Services" || category == "Information System Services" || category == "QC Services" || category == "R&M Services" || category == "Raw Materials" || category == "Factory Services" || category == "RCM Services" || category == "Sales & Marketing Services")
                                  count++;
                          }
                          return count == 0;

                      }, message: 'HSNCode is required'
                  },
               ]
           },
           //{
           //    elements: "#CategoryID",
           //    rules: [
           //       { type: form.non_zero, message: "Select a Item Category" },
           //    ]
           //},
           //{
           //    elements: "#BusinessCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'BusinessCategory is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#SecondaryUnitID",
           //    rules: [
           //       { type: form.non_zero, message: "Secondary UOM is required" },
           //    ]
           //},
           {
               elements: "#InventoryUnitID",
               rules: [
                  { type: form.non_zero, message: "Inventory UOM is required" },
               ]
           },
           {
               elements: "#PurchaseUnitID",
               rules: [
                  { type: form.non_zero, message: "PurchaseUnit is required" },
               ]
           },
           //{
           //    elements: "#ConversionFactorPtoI",
           //    rules: [
           //       { type: form.non_zero, message: "Conversion Factor Production to Inventory is required" },
           //       { type: form.non_zero, message: "Please enter a valid Conversion factor Production to Inventory" },
           //       { type: form.positive, message: "Please enter a valid Conversion factor Production to Inventory" },

           //    ]
           //},

           //{
           //    elements: "#ShelfLifeMonths",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid Use in days" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var ShelfLifeMonths = clean($(element).val());
           //               var category = $("#CategoryID option:selected").text();
           //               if (ShelfLifeMonths <= 0) {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Shelf Life In Days required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#BirthDate",
           //    rules: [

           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category != "Customer Credit Note" && category != "Customer Debit Note" && category != "Supplier Credit Note" && category != "Supplier Debit Note" && category != "Refreshment" && category != "Other Income")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'birth date is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#DisContinuedDate",
           //    rules: [

           //       { type: form.future_date, message: "Invalid Discontinued Date" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category != "Customer Credit Note" && category != "Customer Debit Note" && category != "Supplier Credit Note" && category != "Supplier Debit Note" && category != "Refreshment" && category != "Other Income")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'discontinued date is required'
           //       },
           //       {
           //           type: function (element) {
           //               var u_date = $(element).val().split('-');
           //               var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
           //               var a = Date.parse(used_date);
           //               var po_date = $('#BirthDate').val().split('-');
           //               var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
           //               var date = Date.parse(po_datesplit);
           //               return date <= a
           //           }, message: "Discontinued date should be a date on or after birth date"
           //       }
           //    ]
           //},
           //{
           //    elements: "#SalesIncentiveCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Sales Incentive Category is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#SalesCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods" || category == "Raw Materials" || category == "Cleaned Raw Materials" || category == "Packing Material")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Sales Category is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#SalesUnitID",
           //    rules: [
           //       { type: form.required, message: "Select a Sales UOM" },
           //    ]
           //},

           //{
           //    elements: "#DrugScheduleID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Drug Schedule Type is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#MinSalesQtyFull",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid min sale quantity" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Min Sale quantity is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#MinSalesQtyLoose",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid min sale quantity loose" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Max Sale quantity loose is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#MaxSalesQty",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid max sale  quantity" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Max Sale quantity  is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#BotanicalName",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Raw Materials")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Botanical name  is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#ReOrderLevel",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Raw Materials")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Reorder level is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#ReOrderQty",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Raw Materials")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Reorder quantity level is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#QCCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if ($(".IsQCRequired").prop("checked") == true || $(".IsQCRequiredForProduction").prop("checked") == true || $(".IsProprietary").prop("checked") == true) {
           //                       if (category == "Packing Material" || category == "Raw Materials" || category == "Lab Items")

           //                           count++;
           //                   }
           //               }
           //               return count == 0;

           //           }, message: 'Select QC Category'
           //       },
           //    ]
           //},
           //{
           //    elements: "#ConversionFactorPtoSecondary",
           //    rules: [
           //       { type: form.non_zero, message: "Conversion Factor Primary to Secondary is required" },
           //       { type: form.non_zero, message: "Please enter a valid Conversion factor Primary to Secondary" },
           //       { type: form.positive, message: "Please enter a valid Conversion factor Primary to Secondary" },

           //    ]
           //},
           //{
           //    elements: "#CostingCategoryID",
           //    rules: [
           //          {
           //              type: function (element) {
           //                  var count = 0;
           //                  var category = $("#CategoryID option:selected").text();
           //                  if ($(element).val() == "") {
           //                      if (category == "Cleaned Raw Materials" || category == "Raw Materials" || category == "Packing Material" || category == "Semifinished Goods" || category == "Deposits" || category == "Fuel Item" || category == "Other Income")
           //                          count++;
           //                  }
           //                  return count == 0;

           //              }, message: 'Costing category is required'
           //          },
           //    ]
           //},
           //{
           //    elements: "#AssetCategoryID",
           //    rules: [
           //          {
           //              type: function (element) {
           //                  var count = 0;
           //                  var category = $("#CategoryID option:selected").text();
           //                  if ($(element).val() == "") {
           //                      if (category == "Assets_Admin" || category == "Assets_Finance" || category == "Assets_IT" || category == "Assets_Production")
           //                          count++;
           //                  }
           //                  return count == 0;

           //              }, message: 'Asset category is required'
           //          },
           //    ]
           //},
           //{
           //    elements: "#PurchaseCategoryID",
           //    rules: [

           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Cleaned Raw Materials" || category == "Raw Materials" || category == "Packing Material" || category == "Semifinished Goods" || category == "Finished Goods" || category == "Production Consumables" || category == "Fuel Item")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Purchase category is required'
           //       },
           //    ]
           //},

           //{
           //    elements: "#PurchaseUnitID",
           //    rules: [
           //       { type: form.required, message: "Select a Purchase Unit" },
           //    ]
           //},

           //{
           //    elements: "#QtyTolerancePercent",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid quantity tolerance percentage" },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Cleaned Raw Materials" || category == "Raw Materials" || category == "Packing Material")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'quantity tolerance percentage is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#PurchaseLeadTime",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0") {
           //                   if (category == "Customer Credit Note" || category == "Customer Debit Note" || category == "Supplier Credit Note" || category == "Semifinished Goods" || category == "Supplier Debit Note")

           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Purchase Lead Time is required'
           //       },
           //    ]
           //},
           {
               elements: "#GSTCategoryID",
               rules: [
                  { type: form.required, message: "Select a GST Category" },
               ]
           },
           //{
           //    elements: "#AccountsCategoryID",
           //    rules: [
           //       { type: form.required, message: "Select a Accounts Category" },
           //    ]
           //},
           //{
           //    elements: "#ProductionCategoryID",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Production Category is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#MasterFormulaRefNo",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Master formula reference is required'
           //       },
           //    ]
           //},
           //{
           //    elements: "#ProductionGroup",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "") {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'ProductionGroup is required'
           //       },
           //    ]
           //},

           //{
           //    elements: "#ProductLeadDays",
           //    rules: [
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0") {
           //                   if (category == "Finished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Production lead time is required'
           //       },

           //    ]
           //},

           //{
           //    elements: "#BatchSizeQTY",
           //    rules: [
           //       { type: form.positive, message: "Please enter a valid batch size " },
           //       {
           //           type: function (element) {
           //               var count = 0;
           //               var category = $("#CategoryID option:selected").text();
           //               if ($(element).val() == "0.00") {
           //                   if (category == "Finished Goods" || category == "Semifinished Goods")
           //                       count++;
           //               }
           //               return count == 0;

           //           }, message: 'Batch Size is required'
           //       },
           //    ]
           //},

            //{
            //    elements: "#GSTSubCategoryID",
            //    rules: [
            //       { type: form.required, message: "Select a GST Sub Category" },
            //    ]
            //},
            //{
            //    elements: "#SeasonPurchaseEnds",
            //    rules: [
            //{
            //    type: function (element) {
            //        var u_date = $(element).val().split('-');
            //        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
            //        var a = Date.parse(used_date);
            //        var po_date = $('#SeasonPurchaseStarts').val().split('-');
            //        var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
            //        var date = Date.parse(po_datesplit);
            //        return ((date <= a) || (u_date == "" && po_date == ""))
            //    }, message: "Season purchase end date should be a date on or after season purchase start date"
            //}

            //    ]
            //},
            // {
            //     elements: "#SeasonEnds",
            //     rules: [
            // {
            //     type: function (element) {
            //         var u_date = $(element).val().split('-');
            //         var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
            //         var a = Date.parse(used_date);
            //         var po_date = $('#SeasonStarts').val().split('-');
            //         var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
            //         var date = Date.parse(po_datesplit);
            //         return ((date <= a) || (u_date == "" && po_date == ""))
            //     }, message: "Sales season end date should be a date on or after sales season start date"
            // }

            //     ]
            // },
        ]

    }

};
