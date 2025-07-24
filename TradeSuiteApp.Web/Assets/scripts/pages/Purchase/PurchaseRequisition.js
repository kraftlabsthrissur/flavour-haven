var freeze_header;
var gridcurrencyclass = 'mask-sales2-currency';
var DecPlaces = 0;
var itemPopUpWindow;
var CurrentSalesInquiryItemID = 0;
$(function () {
    //DecPlaces = clean($("#DecimalPlaces").val());
    //gridcurrencyclass = $("#normalclass").val();
});

purchase_requisition = {
    init: function () {
        var self = purchase_requisition;
        self.item_list();
        self.sales_inquiry_list();
        select_sales_inquiry_table = $('#sales-inquiry-list').SelectTable({
            selectFunction: self.select_sales_inquiry,
            returnFocus: "#txtRqQty",
            modal: "#select-sales-inquiry",
            initiatingElement: "#SalesInquiry",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        supplier.supplier_list('stock');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        self.bind_events();

        freeze_header = $("#purchase-requisition-items-list").FreezeHeader();
        self.purchase_order_history();
        self.pending_po_history();
        self.purchase_legacy_history();
        self.sales_order_history('Quotation');
        self.sales_invoice_history();

    },

    details: function () {
        freeze_header = $("#purchase-requisition-items-list").FreezeHeader();
    },

    list: function () {
        var self = purchase_requisition;
        $('#tabs-pr').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = purchase_requisition;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#pr-draft-list');
                break;
            case "to-be-ordered":
                $list = $('#pr-to-be-ordered-list');
                break;
            case "partially-ordered":
                $list = $('#pr-partially-ordered-list');
                break;
            case "fully-ordered":
                $list = $('#pr-fully-ordered-list');
                break;
            case "cancelled":
                $list = $('#pr-cancelled-list');
                break;
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/PurchaseRequisition/GetPurchaseRequisitionList?type=" + type;

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
                    { "data": "FromDepartment", "className": "FromDepartment" },
                    { "data": "ToDepartment", "className": "ToDepartment" },
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "ItemName", "className": "ItemName" },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseRequisition/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    sales_inquiry_list: function () {

        var $list = $('#sales-inquiry-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Sales/SalesInquiry/GetSalesInquiryList";

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
                            { Key: "Type", Value: "SalesInquiry" },
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
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >";
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
                    { "data": "Remarks", "className": "Remarks" },
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
    clear_items: function () {
        var self = purchase_requisition;
        $("#purchase-requisition-items-list tbody").html('');
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
        self.GrossAmount = 0;
        self.AdditionalDiscount = 0;
        self.DiscountAmount = 0;
        self.TurnoverDiscount = 0;
        self.TaxableAmount = 0;
        self.CGSTAmount = 0;
        self.SGSTAmount = 0;
        self.IGSTAmount = 0;
        self.CashDiscount = 0;
        self.NetAmount = 0;
        self.RoundOff = 0;
        self.CessAmount = 0;

    },
    select_sales_inquiry: function () {
        var self = purchase_requisition;
        var row;
        var ID = [];
        var radioboxes = $('#sales-inquiry-list tbody input[type="radio"]:checked');
        if (radioboxes.length > 0) {

            if ($("#purchase-requisition-items-list tbody tr").length > 0) {

                app.confirm("Items will be removed", function () {
                    self.clear_items();
                    $.each(radioboxes, function () {
                        row = $(this).closest("tr");
                        ID.push($(this).val());
                    });
                    self.get_sales_inquiry_items(ID);
                }, function () {

                });
            } else {
                $.each(radioboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                });
                self.get_sales_inquiry_items(ID);

            }
        } else {
            app.show_error("No orders are selected");
        }
    },
    get_row_item: function ($row) {
        var item = {};
        item.ItemID = clean($row.find(".ItemID").val());
        item.UnitID = clean($row.find(".SalesUnitID").val());
        item.SalesInquiryItemID = 0;
        item.ItemCode = $row.find(".Code").text().trim();
        item.ItemName = $row.find(".Name").text().trim();
        item.PartsNumber = $row.find(".PartsNumber").val();
        item.UnitName = $row.find(".SalesUnit").val();
        item.Remarks = "";
        item.MRP = clean($row.find(".Rate").val());
        item.BasicPrice = $row.find(".Rate").val();
        item.Qty = 1;
        item.GrossAmount = item.BasicPrice * item.Qty;
        item.AdditionalDiscount = 0;
        item.DiscountAmount = 0;
        item.TurnoverDiscount = 0;
        item.TaxableAmount = 0;
        item.GSTAmount = 0;
        item.CashDiscount = 0;
        item.NetAmount = item.GrossAmount - item.DiscountAmount;
        item.IsIncluded = true;
        return item;
    },
    itemExists: function (items, item) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].ItemID == item.ItemID) {
                return true;
            }
        }
        return false;
    },
    select_item: function () {
        var self = purchase_requisition;
        var items = [];
        var $radio = $('#item-list tbody input[type="radio"]:checked');
        if ($radio.length > 0) {

            if (clean($("#purchase-requisition-items-list tbody tr").find("td").find(".SalesInquiryItemID").val()) > 0) {
                app.confirm("Items will be removed", function () {
                    self.clear_items();
                    var $row = $radio.closest("tr");
                    var item = self.get_row_item($row);
                    items.push(item);
                    self.process_requisition(items);
                }, function () {

                });
            } else {
                items = self.GetProductList();
                var $row = $radio.closest("tr");
                var item = self.get_row_item($row);
                if (!self.itemExists(items, item)) {
                    items.push(item);
                    self.process_requisition(items);
                }
            }

        } else {
            app.show_error("No orders are selected");
        }
    },
    get_sales_inquiry_items: function (ID) {
        var self = purchase_requisition;
        $("#SalesInquiryID").val(ID);
        $.ajax({
            url: '/Sales/SalesInquiry/GetSalesInquiryItemsPurchaseRequisition',
            dataType: "json",
            data: {
                SalesInquiryID: ID

            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.set_salesinauiry(response.SalesInauiry);
                    self.process_requisition(response.Items);
                }
            }
        });
    },
    set_salesinauiry: function (SalesInauiry) {
        $("#PurchaseRequisitedCustomer").val(SalesInauiry.RequestedCustomerName);
        $("#RequisitedCustomerAddress").val(SalesInauiry.RequestedCustomerAddress);
        $("#RequisitedPhoneNumber1").val(SalesInauiry.PhoneNo1);
        $("#RequisitedPhoneNumber2").val(SalesInauiry.PhoneNo2);
        $("#txtRemarks").val(SalesInauiry.Remarks);
    },
    process_requisition: function (items) {
        var self = purchase_requisition;
        self.Items = self.process_items(items);
        self.add_items_to_grid();
        self.calculate_grid_total();
    },
    process_items: function (items) {
        var self = purchase_requisition;
        var processed_items = [];
        $.each(items, function (i, item) {
            item = self.get_item_properties(item);
            item.index = i + 1;
            processed_items.push(item);
        });

        return processed_items;
    },
    get_item_properties: function (item) {
        var self = purchase_requisition;
        item.MRP = item.Rate;
        item.BasicPrice = item.MRP;
        item.GrossAmount = (item.BasicPrice * (item.Qty));
        item.AdditionalDiscount = 0;
        item.DiscountAmount = 0;
        item.TurnoverDiscount = 0;
        item.TaxableAmount = 0;
        item.GSTAmount = 0;
        item.CashDiscount = 0;
        item.NetAmount = item.GrossAmount - item.DiscountAmount;
        item.IsIncluded = true;
        return item;
    },
    calculate_grid_total: function () {
        var self = purchase_requisition;
        self.GSTAmount = 0;
        self.TurnoverDiscount = 0;
        self.GrossAmount = 0;
        self.DiscountAmount = 0;
        self.DiscountPercentage = 0;
        self.AdditionalDiscount = 0;
        self.TaxableAmount = 0;
        self.NetAmount = 0;
        self.RoundOff = 0;
        self.CashDiscount = 0;
        self.CessAmount = 0;
        var temp = 0;
        $(self.Items).each(function (i, item) {
            if (item.IsIncluded) {
                self.GrossAmount += item.GrossAmount;
                self.DiscountAmount += item.DiscountAmount;
                self.AdditionalDiscount += item.AdditionalDiscount;
                self.TaxableAmount += item.TaxableAmount;
                self.GSTAmount += item.GSTAmount;
                self.NetAmount += item.NetAmount;
                self.CashDiscount += item.CashDiscount;
                self.CessAmount += item.CessAmount;
            }
        });
        //temp = self.NetAmount;
        //self.NetAmount = Math.round(self.NetAmount);
        //self.RoundOff = temp - self.NetAmount;

        self.CGSTAmount = ((self.GSTAmount / 2));
        self.SGSTAmount = ((self.GSTAmount / 2));
        self.IGSTAmount = 0;

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
                    },
                    { "data": "CurrencyCode", "className": "CurrencyCode" },
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
                    },
                    { "data": "CurrencyCode", "className": "CurrencyCode" },
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
                    },
                    { "data": "CurrencyCode", "className": "CurrencyCode" },
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
                    },
                    { "data": "CurrencyCode", "className": "CurrencyCode" },
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
                    },
                    { "data": "CurrencyCode", "className": "CurrencyCode" },
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
    showitemhistory: function () {
        var ItemID = $(this).closest('tr').find('.ItemID').val();
        $("#HistoryItemID").val(ItemID);
        $('#show-history').trigger('click');
    },
    add_items_to_grid: function () {
        var self = purchase_requisition;
        var index = 0;
        var tr = '';
        $(self.Items).each(function (i, item) {
            index++;
            tr += '<tr class="included"  >'
                + (item.ItemID > 0 ? '<td class="uk-text-center showitemhistory action"><a class="uk-text-center action"><i class="uk-icon-eye-slash"></i></a></td>' : '<td></td>')
                + '<td class="uk-text-center">' + index
                + '<input type="hidden" class="ItemID included" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="SalesInquiryItemID included" value="' + item.SalesInquiryItemID + '"  />'
                + '</td>'
                + '<td class="ItemCode">' + item.ItemCode + '</td>'
                + '<td class="ItemName">' + item.ItemName + '</td>'
                + '<td class="PartsNumber">' + item.PartsNumber + '</td>'
                + '<td class="UnitName">' + item.UnitName + '</td>'
                + '<td><input type="text" class="md-input Remarks" value="' + item.Remarks + '" /></td>'
                + '<td><input type="text" class="md-input MRP ' + gridcurrencyclass + '" value="' + item.MRP + '" readonly="readonly" /></td>'
                + '<td><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  /></td>'
            if (item.ItemID === 0) {
                tr += '<td><button class="md-btn md-btn-primary create-item" >Create</button></td>';
            } else {
                tr += '<td>Item exist</td>';
            }
            tr += '<td class="uk-text-center">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        });

        var $tr = $(tr);
        app.format($tr);
        $("#purchase-requisition-items-list tbody").html($tr);
        $("#item-count").val(index);
    },

    bind_events: function () {
        var self = purchase_requisition;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnAddProduct").on('click', self.add_item);
        $(".btnUpdateSPrDraft,.Update").on("click", self.update);
        $(".UpdateNew").on("click", self.update_new);
        $("#DDLItemCategory").on('change', self.get_purchase_category);
        $("body").on("click", ".remove-item", self.remove_item);
        $("#btnOksalesinquiry").on("click", self.select_sales_inquiry);
        $("#btnOKItem").on("click", self.select_item);

        $(".btnSavePr, .btnSaveSPrDraft,.btnSavePrNew").on("click", self.on_save);
        $("body").on("click", ".create-item", self.create_item);
        $('body').on('click', '#purchase-requisition-items-list tbody tr td.showitemhistory', self.showitemhistory);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#btnOKSupplier").on('click', self.select_supplier);
        SetMinDateValue();
    },
    select_supplier: function () {
        var self = purchase_requisition;
        //self.remove_all_items_from_grid();
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var Email = $(row).find(".Email").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        var IsInterCompany = $(row).find(".IsInterCompany").val();
        var InterCompanyLocationID = clean($(row).find(".InterCompanyLocationID").val());
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        $("#SupplierName").val(Name);
        $("#SupplierLocation").val(Location);
        $("#SupplierID").val(ID);
        $("#StateId").val(StateID);
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
        $("#IsInterCompany").val(IsInterCompany);
        $("#Email").val(Email);

    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
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
    set_supplier_details: function (event, item) {   // on select auto complete item
        var self = purchase_requisition;
        self.remove_all_items_from_grid();
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#IsInterCompany").val(item.isintercompany);
        $("#Email").val(item.email);
        $("#InterCompanyLocationID").val(item.interCompanyLocationid);
    },
    listenMessage: function (msg) {
        var $td = $('#purchase-requisition-items-list tbody tr td input.SalesInquiryItemID').filter(':input[value="' + CurrentSalesInquiryItemID + '"]').closest('td');
        var itemid = msg.data.ItemID;
        var unitid = msg.data.UnitID;
        var Code = msg.data.Code;
        var Name = msg.data.Name;
        var PartsNumbers = msg.data.PartsNumbers;
        var SalePrice = msg.data.SalePrice;
        $td.find(".ItemID").val(itemid);
        $td.find(".UnitID").val(unitid);
        var $tr = $td.closest('tr');
        $tr.find("td.ItemCode").text(Code);
        $tr.find("td.ItemName").text(Name);
        $tr.find("td.PartsNumber").text(PartsNumbers);
        $tr.find(".MRP").val(SalePrice);
    },
    create_item: function () {
        var self = purchase_requisition;
        var $row = $(this).closest("tr");
        $row.find('.create-item').hide();
        var SalesInquiryItemID = $row.find(".SalesInquiryItemID").val();
        CurrentSalesInquiryItemID = SalesInquiryItemID;
        var itemCode = $row.find(".ItemCode").text().trim();
        var itemname = $row.find(".ItemName").text().trim();
        var partsnumber = $row.find(".PartsNumber").text().trim();
        var unit = $row.find(".UnitName").text().trim();
        var MRP = $row.find(".MRP").val();
        var relativeUrl = "/Masters/Item/CreateV4?siitemid=" + SalesInquiryItemID + "&code=" + itemCode + "&name=" + itemname + "&pno=" + partsnumber + "&unit=" + unit;
        var windowFeatures = "width=700,height=700,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes";
        itemPopUpWindow = window.open(relativeUrl, '_blank', windowFeatures);

        itemPopUpWindow.addEventListener("message", self.listenMessage, false);

    },

    update_item_list: function () {
        item_list.fnDraw();
    },

    remove_item: function () {
        var self = purchase_requisition;
        $(this).closest("tr").remove();
        $("#purchase-requisition-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#purchase-requisition-items-list tbody tr").length);

        freeze_header.resizeHeader();
    },

    get_purchase_category: function () {
        var item_category_id = $(this).val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = 0;
        }
        $.ajax({
            url: '/Purchase/PurchaseRequisition/GetPurchaseCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#DDLPurchaseCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#DDLPurchaseCategory").append(html);
            }
        });

    },

    set_item_details: function (event, item) {   // on select auto complete item
        var self = purchase_requisition;
        $("#ItemID").val(item.id);
        $("#ItemTypeID").val(item.itemTypeId);
        $("#Stock").val(item.stock);
        $("#QtyUnderQC").val(item.qtyUnderQc);
        $("#QtyOrdered").val(item.qtyOrdered);
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#PurchaseUnit").val(item.purchaseUnit);
        $("#PurchaseUnitID").val(item.purchaseUnitId);
        self.get_units();
        $('#txtRqQty').focus();
    },

    get_units: function () {
        var self = purchase_requisition;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);
    },

    get_items: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    error_count: 0,

    add_item: function () {
        var self = purchase_requisition;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count == 0) {
            var SerialNo = $(".prTbody .rowPr").length + 1;
            var html = '  <tr class="rowPr">' +
                ' <td class="uk-text-center">' + SerialNo + '</td>' +
                ' <td class="clProduct">' + $("#ItemName").val()
                + '<input type="hidden" class="ItemID" value="' + $("#ItemID").val() + '" />'
                + '<input type="hidden" class="UnitID" value="' + $("#UnitID option:selected").val() + '" />'
                + '<input type="hidden" class="ItemTypeID" value="' + $("#ItemTypeID").val() + '" /></td>' +
                ' <td class=" clUnit">' + $("#UnitID option:selected").text() + '</td>' +
                ' <td class="uk-text-right clQty" ><input type="text" class="md-input uk-text-right clRqQty mask-qty" value="' + clean($("#txtRqQty").val()) + '" /></td>' +
                ' <td class="clDate"><input type="text" class="md-input future-date date cltxtDate" value="' + $("#txtExpDate").val() + '" /></td>' +
                ' <td><input type="text" class="md-input txtRemarks"  value="' + $("#txtRemarks").val() + '" /></td>' +
                ' <td class="uk-text-right clStock mask-qty" >' + $("#Stock").val() + '</td>' +
                ' <td class="uk-text-right clQtyUnderQC mask-qty">' + $("#QtyUnderQC").val() + '</td>' +
                ' <td class="uk-text-right clQtyOrdered mask-qty">' + $("#QtyOrdered").val() + '</td>' +
                ' <td class="uk-text-center">' +
                '   <a  class="remove-item" >' +
                '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                ' </a>' +
                ' </td>' +
                '</tr>';
            var $html = $(html);
            app.format($html);
            $(".prTbody").append($html);
            self.clearItemSelectControls();
            $("#ItemName").focus();
            SetMinDateValue();
            self.count_items();
            freeze_header.resizeHeader();
        }
    },

    count_items: function () {
        var count = $('#purchase-requisition-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    validate_item: function () {
        var self = purchase_requisition;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form: function () {
        var self = purchase_requisition;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = purchase_requisition;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    get_data: function (IsDraft) {
        var self = purchase_requisition;
        var modal = {
            Id: $("#Id").val(),
            SalesInquiryID: $("#SalesInquiryID").val(),
            PurchaseRequisitionNumber: $("#PurchaseRequisitionNumber").val(),
            PurchaseRequisitedCustomer: $("#PurchaseRequisitedCustomer").val(),
            RequisitedCustomerAddress: $("#RequisitedCustomerAddress").val(),
            RequisitedPhoneNumber1: $("#RequisitedPhoneNumber1").val(),
            RequisitedPhoneNumber2: $("#RequisitedPhoneNumber2").val(),
            Remarks: $("#txtRemarks").val(),
            PrDate: $("#Date").val(),
            DepartmentFrom: $("#DepartmentFrom").val(),
            DepartmentTo: $("#DepartmentTo").val(),
            IsDraft: IsDraft,
            SupplierID: $("#SupplierID").val(),
            SupplierName: $("#SupplierName").val(),
            Item: self.GetProductList(),
        };
        return modal;
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
                        "data": "PendingOrderQty", "className": "PendingOrderQty", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.PendingOrderQty + "</div>";
                        }
                    },
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
    clearItemSelectControls: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#ItemTypeID").val('');
        $("#Stock").val('');
        $("#QtyUnderQC").val('');
        $("#Unit").val('');
        $("#UnitID").val('');
        $("#QtyOrdered").val('');
        $("#txtRqQty").val('');
        $("#txtExpDate").val('');
        $("#txtRemarks").val('');
    },
    GetNotCreatedItmesCount: function () {
        var count = 0;
        $("#purchase-requisition-items-list tbody tr.included").each(function () {
            if (clean($(this).find('.ItemID').val()) == 0) {
                count++;
            }
        })
        return count;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var i = 0;

        $("#purchase-requisition-items-list tbody tr.included").each(function () {
            i++;
            var ItemID = $(this).find('.ItemID').val();
            var ItemCode = $(this).find('.ItemCode').text().trim();
            var ItemName = $(this).find('.ItemName').text().trim();
            var PartsNumber = $(this).find('.PartsNumber').text().trim();
            var UnitName = $(this).find('.UnitName').text().trim();
            var SalesInquiryItemID = $(this).find('.SalesInquiryItemID ').val();
            var Qty = clean($(this).find('.Qty').val());
            var MRP = clean($(this).find('.MRP').val());
            var Remarks = $(this).find('.Remarks').val();
            var GrossAmount = clean($(this).find('.GrossAmount').val());
            var NetAmount = clean($(this).find('.NetAmount').val());
            var UnitID = clean($(this).find('.UnitID').val());
            ProductsArray.push({ ItemID: ItemID, ItemCode: ItemCode, ItemName: ItemName, PartsNumber: PartsNumber, Unit: UnitName, UnitName: UnitName, UnitID: UnitID, SalesInquiryItemID: SalesInquiryItemID, Qty: Qty, MRP: MRP, Remarks: Remarks, GrossAmount: GrossAmount, NetAmount: NetAmount });
        })
        return ProductsArray;
    },

    on_save: function () {

        var self = purchase_requisition;
        var data = self.get_data();
        var location = "/Purchase/PurchaseRequisition/Index";
        var url = '/Purchase/PurchaseRequisition/Create';

        if ($(this).hasClass("btnSaveSPrDraft")) {
            data.IsDraft = true;
            url = '/Purchase/PurchaseRequisition/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSavePrNew")) {
                location = "/Purchase/PurchaseRequisition/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {

            var itemvalidate = self.GetNotCreatedItmesCount();
            var conMsg = itemvalidate > 0 ? "Do you want to continue with creating item?" : 'Do you want to save?';
            app.confirm_cancel(conMsg, function () {
                self.save(data, url, location);
            }, function () {
            })
        } else {
            self.save(data, url, location);
        }
    },

    save: function (data, url, location) {
        var self = purchase_requisition;
        $(".btnSavePrNew, .btnSavePr,.btnSaveSPrDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { modal: data },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase requisition created successfully");

                    setTimeout(function () {
                        window.location = location;
                    }, 1000);

                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    //app.show_error("Failed to create purchase requisition");
                    $(".btnSavePrNew, .btnSavePr,.btnSaveSPrDraft").css({ 'display': 'block' });
                }
            },
        });
    },

    save_new: function () {
        var self = purchase_requisition;
        var modal;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSavePr")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("btnSavePrNew")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("btnSaveSPrDraft")) {
            modal = self.get_data(true);
        }

        $(".btnSavePrNew, .btnSavePr,.btnSaveSPrDraft").css({ 'display': 'none' });
        $.ajax({
            url: '/Purchase/PurchaseRequisition/Create',
            data: { modal: modal },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase requisition created successfully");

                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseRequisition/Create";
                    }, 1000);

                } else {
                    app.show_error("Failed to create purchase requisition");
                    $(".btnSavePrNew, .btnSavePr,.btnSaveSPrDraft").css({ 'display': 'block' });
                }
            },
        });
    },
    update: function () {
        var self = purchase_requisition;
        self.error_count = 0;
        var modal;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }

        if ($(this).hasClass("Update")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("UpdateNew")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("btnUpdateSPrDraft")) {
            modal = self.get_data(true);
        }
        $(".UpdateNew,.btnUpdateSPrDraft,.Update").css({ 'display': 'none' });
        $.ajax({
            url: '/PurchaseRequisition/Edit',
            data: { modal: modal },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase requisition updated successfully");

                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseRequisition/Index";
                    }, 1000);


                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    //app.show_error("Failed to update purchase requisition");
                    $(".UpdateNew,.btnUpdateSPrDraft,.Update").css({ 'display': 'block' });
                }
            },
        });
    },

    update_new: function () {
        var self = purchase_requisition;
        self.error_count = 0;
        var modal;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }

        if ($(this).hasClass("Update")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("UpdateNew")) {
            modal = self.get_data(false);
        }
        else if ($(this).hasClass("btnUpdateSPrDraft")) {
            modal = self.get_data(true);
        }
        $(".UpdateNew,.btnUpdateSPrDraft,.Update").css({ 'display': 'none' });
        $.ajax({
            url: '/PurchaseRequisition/Edit',
            data: { modal: modal },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase requisition updated successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseRequisition/Create";
                    }, 1000);
                } else {
                    app.show_error("Failed to update purchase requisition");
                    $(".UpdateNew,.btnUpdateSPrDraft,.Update").css({ 'display': 'block' });
                }
            },
        });
    },

    rules: {
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please select an item" },
                ]
            },
            {
                elements: "#txtRqQty",
                rules: [
                    { type: form.non_zero, message: "Please enter required quantity" },
                    { type: form.required, message: "Please enter required quantity" },
                    { type: form.numeric, message: "Please enter valid required quantity" },
                    { type: form.positive, message: "Please enter positive required quantity" },
                ]
            },
            {
                elements: "#txtExpDate",
                rules: [
                    { type: form.future_date, message: "Invalid expected date" },
                    {
                        type: function (element) {
                            var item_name = $("#ItemName").val();
                            return item_name != "MILK" ? true : form.required(element);
                        }, message: "Please enter expected date"
                    },
                ]
            }
        ],
        on_blur: [],
        on_draft: [
            {
                elements: "#DepartmentTo",
                rules: [
                    { type: form.required, message: "Please select to department" },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },

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
                elements: ".clRqQty",

                rules: [
                    {
                        type: form.non_zero, message: 'Please enter required quantity'
                    }
                ]

            }
            ,
            {
                elements: ".cltxtDate",
                rules: [
                    { type: form.future_date, message: "Invalid date" },
                ]
            }
            //{
            //    elements: ".ItemID",
            //    rules: [
            //        {
            //            type: function (element) {
            //                var ItemID = clean($(element).val());
            //                if (ItemID > 0) {
            //                    return true;
            //                } else {
            //                    return false;
            //                }
            //            },
            //            message: "Item Not present. Create item First"
            //        }
            //    ]
            //}
            //,
            //{
            //    elements: "#DepartmentFrom",
            //    rules: [
            //        { type: form.required, message: "Please select from department" },
            //    ]
            //},
            //{
            //    elements: "#DepartmentTo",
            //    rules: [
            //        { type: form.required, message: "Please select to department " },
            //    ]
            //}
        ]
    }
};



function SetMinDateValue() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;

    $(".cltxtDate").attr("min", today);
}
