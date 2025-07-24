var gridcurrencyclass = '';
var DecPlaces = 0;
var CurrentPurchaseRequisitionTrasID = 0;
var itemPopUpWindow;
$(function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
});
var progressbar = $("#progress-bar"),
    bar = progressbar.find('.uk-progress-bar');
var select_table;
var freeze_header;
purchase_order = {
    init: function () {
        var self = purchase_order;

        item_list = self.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item_confirm,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.purchase_requisition_list();
        supplier.supplier_list('stock');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        $('#purchase-requisition-list').SelectTable({
            selectFunction: self.select_puchase_requisitions,
            returnFocus: "#GST",
            modal: "#select_pr",
            initiatingElement: "#PurchaseRequisitionIDS",
            selectionType: "checkbox"
        });

        purchase_orderCRUD.purchaseOCreateAndUpdate();
        purchase_order.get_purchase_category();
        freeze_header = $("#purchase-order-items-list").FreezeHeader();

        self.bind_events();

        self.purchase_order_history();
        self.pending_po_history();
        self.purchase_legacy_history();
        self.sales_order_history('Quotation');
        self.sales_invoice_history();

        if (clean($("#ID").val()) > 0) {
            self.get_supplier_addresses();
            if (clean($("#IsInterCompany").val()) == 1) {
                $(".supplierLocation").addClass("uk-hidden");
                $(".intercompanySupplierlocation").removeClass("uk-hidden");
                var checkboxes = "";
                var $checkboxes = $("<div></div>");
                $(".checkbox-container").html('');
                checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
                $checkboxes.append(checkboxes);
                app.format($checkboxes);
                $(".checkbox-container").eq(0).html($checkboxes);
            }
            else {
                $(".supplierLocation").removeClass("uk-hidden");
                $(".intercompanySupplierlocation").addClass("uk-hidden");
                var checkboxes = "";
                var $checkboxes = $("<div></div>");
                $(".checkbox-container").html('');
                checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
                $checkboxes.append(checkboxes);
                app.format($checkboxes);
                $(".checkbox-container").eq(0).html($checkboxes);
            }
        }
        $('#tabs-purchase-order-create').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                active_item.data('tab-loaded', true);
            }
        });
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
            $('body').on("click", '#show-purchae-history', function () {
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
            $('body').on("click", '#show-purchae-history', function () {
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
        var self = purchase_order;
        setTimeout(function () {
            freeze_header = $("#purchase-order-items-list").FreezeHeader();
        }, 100);
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".ExportItemCode", self.ExportItemCode);
        $("body").on("click", ".ExportPartNo", self.ExportPartNo);


        $("body").on("click", ".ItemCode", self.ItemCode);
      //  $("body").on("click", ".PartNo", self.PartNo);
        $("body").on("click", ".btnSendMail", self.send_mail);
        if (clean($("#IsInterCompany").val()) == 1) {
            $(".supplierLocation").addClass("uk-hidden");
            $(".intercompanySupplierlocation").removeClass("uk-hidden");

        }
        else {
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }
        $('body').on('click', '.btnitemsuspend', purchase_order.confirm_suspend_item);
    },
    confirm_suspend_item: function () {
        var self = purchase_order;
        var id = $(this).closest('tr').find('.POTransID').val();
        app.confirm("Do you want to suspend? This can't be undone", function () {
            self.suspend_purchaseorder_item(id);
        });
    },
    add_item_confirm: function () {

        var self = purchase_order;
        if (clean($("#purchase-order-items-list tbody tr").find("td").find(".PurchaseRequisitionTrasID").val()) > 0) {
            app.confirm_cancel("Do you want to remove existing items ?", function () {
                self.remove_all_items_from_grid();
                self.add_item();
            }, function () {
            });
        } else {
            self.add_item();
        }
    },
    suspend_purchaseorder_item: function (id) {
        $.ajax({
            url: '/Purchase/PurchaseOrder/SuspendItem',
            dataType: "json",
            type: "GET",
            data: {
                ID: id
            },
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Purchase order item suspended successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnitemsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                    freeze_header.resizeHeader();
                }

                if (response.Data == 0) {
                    app.show_error("Some error occured while suspending item");

                }
            }
        });
    },

    printpdf: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },

    ItemCode: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderItemCodePrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },
    PartNo: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPartNoPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },
    ExportItemCode: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderExportIemCodePrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },
    ExportPartNo: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderExportPartNoPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },

    send_mail: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        var subject = "Automaticaly generated";
        var body = "This is an automaticaly generated mail.";
        //var ToEmailID = "pradeep@kraftlabs.com";
        var ToEmailID = $("#Email").val();

        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.sendmail(url, "PurchaseOrder", id, subject, body, ToEmailID);
                }
            }
        });
    },

    list: function () {
        var self = purchase_order;

        $('#tabs-purchase-order').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

        $("body").on('click', '.btnclone', self.open_clone);
        $("body").on('click', '.btnsuspend', self.confirm_suspend_po);
        $("body").on('click', '.btnpoprint', purchase_order.Purchase_print);

    },

    tabbed_list: function (type) {
        var self = purchase_order;
        var $list;

        switch (type) {
            case "Draft":
                $list = $('#purchase-order-draft-list');
                break;
            case "ToBeApproved":
                $list = $('#purchase-order-to-be-approved-list');
                break;
            case "PartiallyApproved":
                $list = $('#purchase-order-partially-approved-list');
                break;
            case "Approved":
                $list = $('#purchase-order-approved-list');
                break;
            case "PartiallyProcessed":
                $list = $('#purchase-order-partially-processed-list');
                break;
            case "Processed":
                $list = $('#purchase-order-processed-list');
                break;
            case "Suspended":
                $list = $('#purchase-order-suspended-list');
                break;
            case "Cancelled":
                $list = $('#purchase-order-cancelled-list');
                break;
            default:
                $list = $('#purchase-order-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/PurchaseOrder/GetPurchaseOrderList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 10,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "async": true,
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
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "ItemName", "className": "ItemName" },
                    {
                        "data": "Suspend", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<button class='md-btn md-btn-primary btnsuspend' >Suspend</button>";
                        }
                    },


                    {
                        "data": "poprint", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<button class='md-btn md-btn-primary btnpoprint' >Print</button>";
                        }
                    },
                    //{
                    //    "data": "Cancel", "className": "action uk-text-center", "searchable": false,
                    //    "render": function (data, type, row, meta) {
                    //        return "<button class='md-btn md-btn-primary btnclone' >Clone</button>";
                    //    }
                    //}
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseOrder/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
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
                            { Key: "BusinessCategoryID", Value: $('#BusinessCategoryID').val() },
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
                                + "<input type='hidden' class='vatPercentage' value='" + row.VATPercentage + "'>"
                                + "<input type='hidden' class='retailLooseRate' value='" + row.RetailLooseRate + "'>"
                                + "<input type='hidden' class='retailMRP' value='" + row.RetailMRP + "'>"
                                + "<input type='hidden' class='purchaseLooseRate' value='" + row.PurchaseLooseRate + "'>"
                                + "<input type='hidden' class='purchaseMRP' value='" + row.PurchaseMRP + "'>"
                               // + "<input type='hidden' class='partsNumber' value='" + row.PartsNumber + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='finishedGoodsCategoryID' value='" + row.FinishedGoodsCategoryID + "'>"
                                + "<input type='hidden' class='TravelCategoryID' value='" + row.TravelCategoryID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='PurchaseUnit' value='" + row.PurchaseUnit + "'>"
                                + "<input type='hidden' class='PurchaseUnitID' value='" + row.PurchaseUnitID + "'>"
                                + "<input type='hidden' class='PurchaseCategoryID' value='" + row.PurchaseCategoryID + "'>"
                                + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    //{ "data": "PartsNumber", "className": "PartsNumber" },
                    { "data": "Remark", "className": "Remark" },
                    { "data": "Model", "className": "Model" },
                    { "data": "PurchaseUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PurchaseCategory", "className": "PurchaseCategory" },
                    { "data": "PendingOrderQty", "className": "PendingOrderQty" },
                    { "data": "QtyAvailable", "className": "QtyAvailable" },

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
    },
    purchase_requisition_list: function () {
        var $list = $('#purchase-requisition-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Purchase/PurchaseRequisition/GetPurchaseRequisitionListForPurchaseOrder";

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
                            { Key: "Type", Value: $('#type').val() }
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
                            return "<input type='checkbox' class='uk-radio PurchaseRequisitionID' name='PurchaseRequisitionID' data-md-icheck value='" + row.PurchaseRequisitionID + "' >";
                        }
                    },
                    { "data": "RequisitionNo", "className": "RequisitionNo" },
                    { "data": "Date", "className": "Date" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    { "data": "RequisitedPhoneNumber1", "className": "RequisitedPhoneNumber1" },
                    { "data": "RequisitedCustomerAddress", "className": "RequisitedCustomerAddress" },
                    { "data": "Remarks", "className": "Remarks" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

    bind_events: function () {
        //$("body").on('click', ".cancel", purchase_order.cancel_confirm);
        $("body").on('ifChanged', '.chkCheck', purchase_order.include_item);
        $("body").on('click', '.remove-item', purchase_order.remove_item);
        $("#DDLItemCategory").on('change', purchase_order.get_purchase_category);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': purchase_order.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_supplier_details);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': purchase_order.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_item_details);
        UIkit.uploadSelect($("#select-quotation"), purchase_order.selected_quotation_settings);
        UIkit.uploadSelect($("#other-quotations"), purchase_order.other_quotation_settings);
        $('body').on('click', 'a.remove-file', purchase_order.remove_file);
        $('body').on('click', 'a.remove-quotation', purchase_order.remove_quotation);
        $('body').on('click', '.cancel', purchase_order.cancel);
        $('body').on('click', '.print-preview', purchase_order.print_preview);
        $('body').on('click', '.close-preview', purchase_order.close_print_preview);
        $('body').on('click', '#invoice_print', purchase_order.print);
        $('body').on('click', '.print', purchase_order.print);
        $("#BusinessCategoryID").on('change', purchase_order.remove_all_items_from_grid);
        $('body').on('change', '#ShippingToId', purchase_order.set_state_id);
        $("#btnOkPrList").on('click', purchase_order.select_puchase_requisitions);
        $("#btnOKSupplier").on('click', purchase_order.select_supplier);
        $("#btnOkPurchaseRequisition").on('click', purchase_order.select_purchase_requisition_confirm);


        $("body").on('click', '#purchase-order-list tbody tr', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Purchase/PurchaseOrder/Details/' + Id;
        });
        $("#btnAddProduct").on("click", purchase_order.add_item_confirm);
        $("body").on('click', '.btnclone', purchase_order.open_clone);
        $("body").on('click', '.btnsuspend', purchase_order.confirm_suspend_po);

        $("body").on('click', '.btnpoprint', purchase_order.Purchase_print);

        $(".select-item").on('click', purchase_order.check_supplier);
        $("#btnOKItem").on("click", purchase_order.select_item_confirm);
        $("#BatchType").on("change", purchase_order.get_rate);
        $("body").on('change', '#purchase-order-items-list tbody tr .batch_type', purchase_order.get_row);
        $('body').on('click', '#purchase-order-items-list tbody tr td.showitemhistory', purchase_order.showitemhistory);
        $("body").on('ifChanged', ".select-all-item", purchase_order.select_all_items);
        purchase_order.Load_All_DropDown();
        $("body").on("keyup change", "#purchase-order-items-list tbody tr .clRqQty, .Rate, .DiscountPercentage, .DiscountAmount, .VATPercentage, .VATAmount", purchase_order.change_grid_values);
        $("body").on("keyup change", "#DiscountPercentage, #Discount, #VATPercentage, #VATAmount, #SuppDocAmount, #SuppShipAmount, #SuppOtherCharge, #GST", purchase_order.change_grid_total_calculations);
        $("body").on("keyup change", "#purchase-order-items-list tbody tr .secondary .secondaryQty, .secondaryUnit, .secondaryRate", purchase_order.change_grid_package_values);
        $("body").on("click", "#purchase-order-items-list tbody tr .create-item", purchase_order.create_item);
    },
    CalculateValueInGrid: function (parent) {
        e = parent.closest('tr');
        var Rate = clean(e.find(".clRate .txtclRate").val());
        var Qty = clean(e.find(".clQty .clRqQty").val());
        e.find(".clValue .txtclValue").val(Rate * Qty);
        CalculateGSTInsideGrid(e);
    },

    select_purchase_requisition_confirm: function () {
        var self = purchase_order;
        var GetSupplierID = clean($("#SupplierID").val());
        if (GetSupplierID == 0) {
            app.show_error("Please Select Supplier Or ExchangeRate is not set for the Supplier Currency.");
            return;
        }
        // var radio = $('#purchase-requisition-list tbody input[type="radio"]:checked');
        var checkboxes = $('#purchase-requisition-list tbody input[type="checkbox"]:checked');
        //var row = $(radio).closest("tr");
        //var PurchaseRequisitionID = radio.val();
        var PurchaseRequisitionIDS = [];
        $.each(checkboxes, function () {
            row = $(this).closest("tr");
            PurchaseRequisitionIDS.push($(this).val());
            //TransNos += $(row).find(".SONo").text().trim() + ",";
            //SalesTypeID = $(row).find(".SalesTypeID").val();
            //FregihtAmt += clean($(row).find(".FreightAmt").val());
        });
        if ($("#purchase-order-items-list tbody tr").length > 0) {
            app.confirm_cancel("Do you sure want to clear selected items ?", function () {
                self.remove_all_items_from_grid();
                self.select_purchase_requisition(PurchaseRequisitionIDS);
            }, function () {
            });
        }
        else {
            self.select_purchase_requisition(PurchaseRequisitionIDS);
        }

    },
    select_purchase_requisition: function (PurchaseRequisitionIDS) {
        var self = purchase_order;

        $('#item-count').val(0);

        $.ajax({
            url: '/Masters/Item/GetItemsListByPurchaseRequisitionIDS',
            dataType: "json",
            data: {
                PurchaseRequisitionIDS: PurchaseRequisitionIDS,
            },
            type: "POST",
            success: function (response) {

                $.each(response.items, function (key, item) {

                    self.AddProductToPrGrid("", item);
                });

                freeze_header.resizeHeader();
                self.count_items();
            }


        });
    },
    remove_all_items_from_grid: function () {
        var self = purchase_order;
        $("#purchase-order-items-list tbody").html("");
        //CalculateNetAmountValue();
        self.count_items();
    },
    get_item_properties: function (item) {
        var self = purchase_order;
        if (item.IsGST == 1) {
            item.GSTAmount = 0;
            item.BasicPrice = item.MRP;
            item.GrossAmount = (item.BasicPrice * item.Qty);
            item.DiscountPercentage = 0;
            item.DiscountAmount = 0;
            item.TaxableAmount = (item.GrossAmount - item.DiscountAmount);
            item.CGST = (item.TaxableAmount * item.CGSTPercent / 100);
            item.SGST = (item.TaxableAmount * item.SGSTPercent / 100);
            item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
            item.NetAmount = (item.TaxableAmount + item.CGST + item.SGST + item.CessAmount);
            return item;
        } else if (item.IsVat == 1) {
            item.BasicPrice = item.MRP;
            item.GrossAmount = (item.BasicPrice * item.Qty);
            item.DiscountPercentage = 0;
            item.DiscountAmount = 0;
            item.TaxableAmount = (item.GrossAmount - item.DiscountAmount);
            item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100);
            item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100);
            item.NetAmount = (item.TaxableAmount + item.VATAmount + item.CessAmount);
            return item;
        }

    },
    create_item: function () {
        var self = purchase_order;
        var $row = $(this).closest("tr");
        $row.find('.create-item').hide();
        var PurchaseRequisitionTrasID = $row.find(".PurchaseRequisitionTrasID").val();
        CurrentPurchaseRequisitionTrasID = PurchaseRequisitionTrasID;
        var itemCode = $row.find(".clCode").text().trim();
        var itemname = $row.find(".clItem").text().trim();
        var partsnumber = $row.find(".clPartsNumber").text().trim();
        var unit = $row.find(".clUnit").text().trim();
        var MRP = clean($row.find(".secondaryRate").val());
        var relativeUrl = "/Masters/Item/CreateV4?pritemid=" + PurchaseRequisitionTrasID + "&code=" + itemCode + "&name=" + itemname + "&pno=" + partsnumber + "&unit=" + unit;
        var windowFeatures = "width=700,height=700,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes";
        itemPopUpWindow = window.open(relativeUrl, '_blank', windowFeatures);

        itemPopUpWindow.addEventListener("message", self.listenMessage, false);

    },
    listenMessage: function (msg) {
        var $td = $('#purchase-order-items-list tbody tr td input.PurchaseRequisitionTrasID').filter(':input[value="' + CurrentPurchaseRequisitionTrasID + '"]').closest('td');
        var itemid = msg.data.ItemID;
        var unitid = msg.data.UnitID;
        var Code = msg.data.Code;
        var Name = msg.data.Name;
        var PartsNumbers = msg.data.PartsNumbers;
        var SalePrice = msg.data.SalePrice;

        $td.find(".ItemID").val(itemid);
        $td.find(".UnitID").val(unitid);
        var $tr = $td.closest('tr');
        $tr.find("td.clCode").text(Code);
        $tr.find("td.clItem").text(Name);
        $tr.find("td.clPartsNumber").text(PartsNumbers);
        //$tr.find(".MRP").val(SalePrice);
    },
    change_grid_package_values: function () {
        var self = purchase_order;
        var $tr = $(this).closest('tr');
        var SecondaryQty = clean($tr.find('.secondaryQty').val());
        var SecondaryUnitSize = clean($tr.find('.secondaryUnit').val());
        if ($(this).attr('class').indexOf('secondaryRate') != -1) {
            var SecondaryRate = clean($tr.find('.secondaryRate').val());
            var Rate = (SecondaryRate / SecondaryUnitSize).toFixed(10);
            $tr.find('.Rate').val(Rate);
            $tr.find('.Rate').trigger('change');

        } else {
            var Rate = clean($tr.find('.Rate').val());
            var SecondaryRate = (Rate * SecondaryUnitSize).toFixed(10);
            $tr.find('.secondary .secondaryRate').val(SecondaryRate);
            var Qty = SecondaryQty * SecondaryUnitSize;
            $tr.find('.clQty .clRqQty').val(Qty);
            $tr.find('.clQty .clRqQty').trigger('change');
        }

    },
    change_grid_values: function () {
        var self = purchase_order;
        var $tr = $(this).closest('tr');
        if ($(this).attr('class').indexOf('DiscountPercentage') != -1) {
            var DiscountPercentage = clean($tr.find('.DiscountPercentage').val());
            self.update_grid_values($tr, 'DiscountPercentage', 0, 0, DiscountPercentage, 0, 0, 0);
        } else if ($(this).attr('class').indexOf('DiscountAmount') != -1) {
            var DiscountAmount = clean($tr.find('.DiscountAmount').val());
            self.update_grid_values($tr, 'DiscountAmount', 0, DiscountAmount, 0, 0, 0, 0);
        } else if ($(this).attr('class').indexOf('clRqQty') != -1) {
            var Qty = clean($tr.find('.clRqQty').val());
            self.update_grid_values($tr, 'clRqQty', Qty, 0, 0, 0, 0, 0);
        } else if ($(this).attr('class').indexOf('Rate') != -1) {
            var Rate = clean($tr.find('.Rate').val());
            self.update_grid_values($tr, 'Rate', 0, 0, 0, Rate, 0, 0);
        } else if ($(this).attr('class').indexOf('VATPercentage') != -1) {
            var VATPercentage = clean($tr.find('.VATPercentage').val());
            self.update_grid_values($tr, 'VATPercentage', 0, 0, 0, 0, VATPercentage, 0);
        } else if ($(this).attr('class').indexOf('VATAmount') != -1) {
            var VATAmount = clean($tr.find('.VATAmount').val());
            self.update_grid_values($tr, 'VATAmount', 0, 0, 0, 0, 0, VATAmount);
        }
    },
    update_grid_values: function ($tr, change, Qty, DiscountAmount, DiscountPercentage, Rate, VATPercentage, VATAmount) {
        var self = purchase_order;
        var MRP = 0;
        var IGSTPercent = 0;
        var CGSTPercent = 0;
        var SGSTPercent = 0;
        var CessPercentage = 0;
        var GrossAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var CessAmount = 0;
        var NetAmount = 0;
        var IsGST = clean($tr.find('.IsGST').val());
        var IsVat = clean($tr.find('.IsVat').val());
        MRP = Rate > 0 ? Rate : clean($tr.find('.Rate').val());
        if (change == 'clRqQty') {
            Qty = Qty;
        } else {
            Qty = clean($tr.find('.clRqQty').val());
        }
        GrossAmount = MRP * Qty;
        if (change == 'DiscountAmount') {
            DiscountPercentage = DiscountAmount / GrossAmount * 100;
            $tr.find('.DiscountPercentage').val(DiscountPercentage);
        } else if (change == 'DiscountPercentage') {
            DiscountAmount = GrossAmount * DiscountPercentage / 100;
            $tr.find('.DiscountAmount').val(DiscountAmount);
        } else {
            DiscountAmount = $tr.find('.DiscountAmount').val();
            DiscountPercentage = DiscountAmount / GrossAmount * 100;
            $tr.find('.DiscountPercentage').val(DiscountPercentage);
        }
        IGSTPercent = clean($tr.find('.IGSTPercent').val());
        CGSTPercent = clean($tr.find('.CGSTPercent').val());
        SGSTPercent = clean($tr.find('.SGSTPercent').val());
        CessPercentage = clean($tr.find('.CessPercentage').val());
        TaxableAmount = GrossAmount - DiscountAmount;
        IGSTAmount = TaxableAmount * IGSTPercent / 100;
        SGSTAmount = TaxableAmount * SGSTPercent / 100;
        CGSTAmount = TaxableAmount * CGSTPercent / 100;
        CessAmount = TaxableAmount * CessPercentage / 100;
        if (change == 'VATPercentage') {
            VATAmount = VATPercentage * TaxableAmount / 100;
            $tr.find('.VATAmount').val(VATAmount);
        } else if (change == 'VATAmount') {
            VATPercentage = VATAmount / TaxableAmount * 100;
            $tr.find('.VATPercentage').val(VATPercentage);
        } else {
            VATPercentage = clean($tr.find('.VATPercentage').val());
            VATAmount = VATPercentage * TaxableAmount / 100;
            $tr.find('.VATPercentage').val(VATPercentage);
            $tr.find('.VATAmount').val(VATAmount);
        }
        $tr.find('.CessAmount').val(CessAmount);
        if (IsGST == 1) {
            NetAmount = TaxableAmount + SGSTAmount + CGSTAmount + CessAmount;
            $tr.find('.SGSTAmount').val(SGSTAmount);
            $tr.find('.CGSTAmount').val(CGSTAmount);
        } else {
            NetAmount = TaxableAmount + VATAmount + CessAmount;
        }
        $tr.find('.GrossAmount').val(GrossAmount);
        $tr.find('.TaxableAmount').val(TaxableAmount);
        $tr.find('.NetAmount').val(NetAmount);
        self.calculate_grid_total();
    },
    showitemhistory: function () {
        var ItemID = $(this).closest('tr').find('.ItemID').val();
        $("#HistoryItemID").val(ItemID);
        $('#show-purchae-history').trigger('click');
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
        var VATAmount = 0;
        var VATPercentage = 0;
        $("#purchase-order-items-list tbody tr.included").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            VATAmount += clean($(this).find('.VATAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            CessAmount += clean($(this).find('.CessAmount').val());
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        $("#GrossAmount").val(GrossAmount);
        $("#Discount").val(DiscountAmount);
        var DiscountPercentage = DiscountAmount / GrossAmount * 100;
        $("#DiscountPercentage").val(DiscountPercentage);
        $("#TaxableAmount").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#CessAmount").val(CessAmount);
        $("#RoundOff").val(RoundOff);
        $("#VATAmount").val(VATAmount);
        VATPercentage = VATAmount / TaxableAmount * 100;
        $("#VATPercentage").val(VATPercentage);
        var SuppDocAmount = clean($("#SuppDocAmount").val());
        var SuppShipAmount = clean($("#SuppShipAmount").val());
        var SuppOtherCharge = clean($("#SuppOtherCharge").val());
        NetAmount = NetAmount + SuppDocAmount + SuppShipAmount + SuppOtherCharge;
        $("#NetAmount").val(NetAmount);
    },
    change_grid_total_calculations: function () {
        var self = purchase_order;
        var elementid = $(this).attr('id');
        var GrossAmount = 0, DiscountPercentage = 0, DiscountAmount = 0, IGSTAmount = 0, SGSTAmount = 0, CGSTAmount = 0, VATAmount = 0, VATPercentage = 0, TaxableAmount = 0,
            NetAmount = 0, RoundOff = 0, CessAmount = 0, discountperitem = 0, vatperitem = 0
        GrossAmount = clean($("#GrossAmount").val());
        TaxableAmount = clean($("#TaxableAmount").val());
        if (elementid == 'DiscountPercentage') {
            DiscountPercentage = clean($("#DiscountPercentage").val());
            DiscountAmount = (GrossAmount * DiscountPercentage / 100);
            discountperitem = DiscountPercentage;
        } else if (elementid == 'Discount') {
            DiscountAmount = clean($("#Discount").val());
            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
            discountperitem = DiscountPercentage;
        } else if (elementid == 'VATPercentage') {
            VATPercentage = clean($("#VATPercentage").val());
            VATAmount = TaxableAmount * VATPercentage / 100;
            vatperitem = VATPercentage;
        } else if (elementid == 'VATAmount') {
            VATAmount = clean($("#VATAmount").val());
            VATPercentage = VATAmount / TaxableAmount * 100;
            vatperitem = VATPercentage;
        } else {
            discountperitem = 0
            vatperitem = clean($("#VATPercentage").val());;
        }
        GrossAmount = 0;
        TaxableAmount = 0;
        DiscountAmount = 0;
        DiscountPercentage = 0;
        VATAmount = 0;
        VATPercentage = 0;
        $("#purchase-order-items-list tbody tr.included").each(function () {
            var $row = $(this);

            var itemDiscountAmount = 0, itemDiscountPercentage = 0, itemGrossAmount = 0, itemTaxableAmount = 0,
                itemIGSTAmount = 0, itemSGSTAmount = 0, itemCGSTAmount = 0, itemVATAmount = 0, itemVATPercentage = 0, itemCessAmount = 0, Rate = 0, Qty = 0;


            itemGrossAmount = clean($row.find(".GrossAmount").val());
            GrossAmount += itemGrossAmount;
            Rate = clean($row.find(".secondaryRate").val());
            Qty = clean($row.find(".secondaryQty").val());

            if (elementid == 'DiscountPercentage' || elementid == 'Discount') {
                itemDiscountAmount = (itemGrossAmount * (discountperitem / 100));
                DiscountAmount += itemDiscountAmount;
                itemDiscountPercentage = (itemDiscountAmount / itemGrossAmount * 100);
            } else {
                itemDiscountAmount = clean($row.find(".DiscountAmount").val());
                DiscountAmount += itemDiscountAmount;
                itemDiscountPercentage = (itemDiscountAmount / itemGrossAmount * 100);
            }
            itemTaxableAmount = itemGrossAmount - itemDiscountAmount;
            TaxableAmount += itemTaxableAmount;
            itemIGSTAmount = itemTaxableAmount * clean($row.find(".IGSTPercent").val()) / 100;
            IGSTAmount += itemIGSTAmount;
            itemSGSTAmount = itemTaxableAmount * clean($row.find(".SGSTPercent").val()) / 100;
            SGSTAmount += itemSGSTAmount;
            itemCGSTAmount = itemTaxableAmount * clean($row.find(".CGSTPercent").val()) / 100;
            CGSTAmount += itemCGSTAmount;
            itemCessAmount = itemTaxableAmount * clean($row.find(".CessPercent").val()) / 100;
            CessAmount += itemCessAmount;

            if (elementid == 'VATPercentage' || elementid == 'VATAmount') {
                itemVATAmount = itemTaxableAmount * vatperitem / 100;
                itemVATPercentage = vatperitem;
            } else {
                itemVATPercentage = clean($row.find(".VATPercentage").val());
                itemVATAmount = itemTaxableAmount * itemVATPercentage / 100;
            }

            if ($("#IsVATExtra").val() == 1) {
                if ($('#GST').val() == 1) {

                    var vRate = Rate * 100 / (100 + vatperitem);
                    itemVATAmount = (Rate - vRate) * Qty;
                    itemNetAmount = ((vRate * Qty) - itemDiscountAmount) + itemVATAmount;
                } else if ($('#GST').val() == 2) {
                    var vRate = Rate;
                    itemVATAmount = itemTaxableAmount * vatperitem / 100;
                    itemNetAmount = itemTaxableAmount + itemVATAmount;
                } else {
                    var vRate = Rate;
                    itemVATAmount = itemTaxableAmount * vatperitem / 100;
                    itemNetAmount = itemTaxableAmount + itemVATAmount;
                }
            }
            else {
                itemNetAmount = itemTaxableAmount + itemCessAmount + itemCGSTAmount + itemSGSTAmount + itemVATAmount;
            }

            VATAmount += itemVATAmount;
            //itemNetAmount = itemTaxableAmount + itemCessAmount + itemCGSTAmount + itemSGSTAmount + itemVATAmount;
            NetAmount += itemNetAmount;
            $row.find(".TaxableAmount").val(itemTaxableAmount.roundToCustom());
            $row.find(".CessAmount").val(itemCessAmount.roundToCustom());
            $row.find(".CGST").val(itemCGSTAmount.roundToCustom());
            $row.find(".SGST").val(itemSGSTAmount.roundToCustom());
            $row.find(".IGST").val(itemIGSTAmount.roundToCustom());
            $row.find(".VATPercentage").val(itemVATPercentage.roundToCustom());
            $row.find(".VATAmount").val(itemVATAmount.roundToCustom());
            $row.find(".NetAmount").val(itemNetAmount.roundToCustom());
            $row.find(".DiscountAmount").val(itemDiscountAmount.roundToCustom());
            $row.find(".DiscountPercentage").val(itemDiscountPercentage.roundToCustom());

        });


        if (elementid == 'DiscountPercentage') {
            $("#Discount").val(DiscountAmount.roundToCustom());
        } else if (elementid == 'Discount') {
            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
            $("#DiscountPercentage").val(DiscountPercentage.roundToCustom());
        } else if (elementid == 'VATAmount') {
            VATPercentage = (VATAmount / TaxableAmount * 100);
            $("#VATPercentage").val(VATPercentage.roundToCustom());
        } else if (elementid == 'VATPercentage') {
            $("#VATAmount").val(VATAmount.roundToCustom());
        } else {
            $("#Discount").val(DiscountAmount.roundToCustom());
            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
            $("#DiscountPercentage").val(DiscountPercentage.roundToCustom());
        }
        $("#GrossAmount").val(GrossAmount.roundToCustom());
        $("#TaxableAmount").val(TaxableAmount.roundToCustom());
        $("#SGSTAmount").val(SGSTAmount.roundToCustom());
        $("#CGSTAmount").val(CGSTAmount.roundToCustom());
        $("#IGSTAmount").val(IGSTAmount.roundToCustom());
        $("#VATAmount").val(VATAmount.roundToCustom());
        $("#CessAmount").val(CessAmount.roundToCustom());
        $("#RoundOff").val(RoundOff.roundToCustom());
        var SuppDocAmount = clean($("#SuppDocAmount").val());
        var SuppShipAmount = clean($("#SuppShipAmount").val());
        var SuppOtherCharge = clean($("#SuppOtherCharge").val());
        NetAmount = NetAmount + SuppDocAmount + SuppShipAmount + SuppOtherCharge;
        $("#NetAmountPrefix").val(NetAmount.roundToCustom());
        $("#NetAmount").val(NetAmount.roundToCustom());

    },
    select_all_items: function () {

        var checkboxes = $("#purchase-order-items-list").find("input.chkCheck");
        var self = this;
        $(checkboxes).each(function () {
            if ($(this).is(":checked") != $(self).is(":checked")) {
                $(this).iCheck('toggle');
            }
        });
        //$("#purchase-order-items-list tr").each(function (e) {
        //    CalculateGST($(this));
        //});
        //CalculateGSTOutsideTheGrid();
        //CalculateNetAmountValue();
    },
    get_row: function () {
        var self = purchase_order;
        var row = $(this).closest('tr');
        self.get_po_item_details(row);
    },
    get_po_item_details: function (row) {
        var self = purchase_order;
        var ItemID = $(row).find('.ItemID').val();
        var BatchtypeID = $(row).find('.clBatch').val();
        self.get_trans_details(ItemID, BatchtypeID, row);
    },
    get_trans_details: function (ItemID, BatchtypeID, row) {
        $.ajax({
            url: '/Masters/Item/GetTranasactionDetails',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: clean(ItemID),
                BatchTypeID: clean(BatchtypeID)
            },
            success: function (response) {
                if (response.Status == "success") {
                    $(row).find('.clLastPr').val(response.data.LastPR);
                    $(row).find('.clLowestPr').val(response.data.LowestPR);
                    $(row).find('.clPendingOrderQty').val(response.data.PendingPOQty);
                    $(row).find('.clQtyWithQc').val(response.data.QtyUnderQC);
                    $(row).find('.clQtyAvailable').val(response.data.Stock);
                }
            }
        })
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
            $('body').on("click", '#show-purchae-history', function () {
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
            $('body').on("click", '#show-purchae-history', function () {
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
            $('body').on("click", '#show-purchae-history', function () {
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
    get_rate: function () {
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
    check_supplier: function () {
        var self = purchase_order;
        self.error_count = 0;
        self.error_count = self.validate_supplier();
        if (self.error_count > 0) {
            UIkit.modal($('#select-item')).hide();
        }

    },
    update_item_list: function () {
        item_list.fnDraw();
    },

    cancel_confirm: function () {
        var self = purchase_order
        app.confirm_cancel("Do you want to cancel? This can't be undone", function () {
            self.cancel();
        }, function () {
        })
    },

    open_clone: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).closest('tr').find('.ID').val();
        window.location = '/Purchase/PurchaseOrder/Clone/' + id;
    },
    confirm_suspend_po: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).closest('tr').find('.ID').val();
        app.confirm("Do you want to suspend? This can't be undone", function () {
            self.suspend_purchaseorder(id);
        });
    },
    is_item_supplied_by_supplier: function () {
        var self = purchase_order;
        var ItemList = "";
        $("#purchase-order-items-list tbody tr").each(function () {
            ItemList += ($(this).find('.ItemID').val()) + ',';
        });
        if (ItemList.length > 0) {
            $.ajax({
                url: '/Purchase/PurchaseOrder/IsItemSuppliedBySupplier',
                data: {
                    ItemLists: ItemList,
                    SupplierID: $("#SupplierID").val()
                },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, function (key, item) {
                        $("#purchase-order-items-list tbody tr").each(function () {
                            var i = $(this).find(".ItemID").val();
                            if (($("#purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status != "Eligible")) {
                                $(this).closest('tr').addClass('ineligible');
                            }
                            else if (($("#purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status == "Eligible")) {
                                $(this).closest('tr').removeClass('ineligible');
                            }
                        });
                    });
                    //$("#purchase-order-items-list tbody tr .clRqQty").each(function () {
                    //    CalculateValueInGrid($(this));
                    //});
                },
            });
        }
        item_list.fnDraw();
    },
    suspend_purchaseorder: function (id) {
        $.ajax({
            url: '/Purchase/PurchaseOrder/Suspend',
            dataType: "json",
            type: "GET",
            data: {
                ID: id,
                Table: "PurchaseOrder"
            },
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Purchase order suspended successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                }
                if (response.Data == 0) {
                    app.show_error("Purchase order already processed");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('processed');
                }
                if (response.Data == 2) {
                    app.show_error("Please cancel GRN before suspending purchase order");

                }
            }
        });
    },


    Purchase_print: function () {
        var self = purchase_order;
        var id = $(this).closest('tr').find('.ID').val();
        /*var id = 231;*/
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPrint',
            dataType: "json",
            type: "GET",
            data: {
                ID: id,

            },
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },















    Load_All_DropDown: function () {
        $.ajax({
            url: '/Purchase/PurchaseOrder/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    BatchTypeList = response.data;
                }
            }
        });
    },


    select_puchase_requisitions: function () {
        var self = purchase_order;
        self.error_count = self.validate_requsition();
        if (self.error_count != 0)
            return
        var UnProcPrList = [];
        $(".unPrTbody .rowUnPr").each(function () {
            if ($(this).find('.clChk .clChkItem').is(":checked")) {
                UnProcPrList.push($(this).find('.clId .clprIdItem').val())
            }
        });

        if ($(".poTbody .rowPR").length > 0 && UnProcPrList.length > 0) {
            app.confirm("Selected items will be removed", function () {
                purchase_order.get_po_items(UnProcPrList);
            })

        } else {
            purchase_order.get_po_items(UnProcPrList);
        }
    },
    get_po_items: function (UnProcPrList) {
        $("#purchase-order-items-list tbody .rowPR").each(function () {
            $(this).remove();
        });
        $("#PurchaseRequisitionIDS").val('');
        var PurchaseRequisitionIDS = [];
        if (UnProcPrList.length > 0) {
            $.ajax({
                url: '/Purchase/PurchaseOrder/GetPoTransByPrId',
                data: { PurchaseRequisitionIDList: UnProcPrList, SupplierID: $("#SupplierID").val() },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, function (key, item) {
                        item.Value = 0;
                        if (PurchaseRequisitionIDS.indexOf(item.Code) == -1) {
                            PurchaseRequisitionIDS.push(item.Code);
                        }
                        purchase_order.AddProductToPrGrid("rowPR", item);
                    });
                    purchase_order.count_items();
                    $("#PurchaseRequisitionIDS").val(PurchaseRequisitionIDS.join(','));
                    //Calculate net amount
                    //CalculateNetAmountValue();
                    setTimeout(function () { freeze_header.resizeHeader(); }, 100);

                },
            });
        }
    },
    Build_Select: function (options, selected) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';

        // console.log(options);
        for (var i = 0; i < options.length; i++) {
            //alert(options[i].Text);
            if (options[i].ID == selected) {
                //$option.attr('selected', 'selected');
                $option = '<option selected value="' + options[i].ID + '">' + options[i].Name + '</option>';
            } else {
                $option = '<option value="' + options[i].ID + '">' + options[i].Name + '</option>';
            }


            $select.append($option);
        }

        return $select.html();

    },
    SelectSecondaryUnits: function (Unit, SecondaryUnits) {
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
    },
    AddProductToPrGrid: function (PrRowClass, item) {
        var self = purchase_order;
        item = self.get_item_properties(item);
        var existedHtml = '';
        var html = '';
        var BatchType;
        var SerialNo = $(".poTbody .rowPO").length + 1;
        if (typeof item.Rate == "undefined") {
            item.Rate = 0;
        }
        if (typeof item.GSTAmount == "undefined") {
            item.GSTAmount = 0;
        }

        //item.LastPR = item.LastPR * item.ExchangeRate;
        //item.LowestPR = item.LowestPR * item.ExchangeRate;
        BatchType = '<select class="md-input label-fixed clBatch ' + (item.FGCategoryID == 0 ? '' : 'enable') + '"' + (item.FGCategoryID == 0 ? 'disabled' : '') + ' >' + purchase_order.Build_Select(BatchTypeList, item.BatchTypeID) + '</select>'
        html = '<tr class="rowPO included ' + PrRowClass + '">';
        if (item.ID === 0) {
            html += '<td></td>';
        } else {
            html += '<td class="uk-text-center showitemhistory action"><a class="uk-text-center action" ><i class="uk-icon-eye-slash"></i></a></td >';
        }
        html += '<td class="uk-text-center clPr">' + SerialNo
            + '<input type="hidden" class="ItemID" value="' + item.ID + '" />'
            + '<input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
            + '<input type="hidden" class="PurchaseRequisitionID" value="' + item.PurchaseRequisitionID + '" />'
            + '<input type="hidden" class="PurchaseRequisitionTrasID" value="' + item.PurchaseRequisitionTrasID + '" />'
            + '<input type="hidden" class="ExchangeRate" value="' + item.ExchangeRate + '" />'
            + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '" />'
            + '<input type="hidden" class="CountryID" value="' + item.CountryID + '" />'
            + '<input type="hidden" class="IsVat" value="' + item.IsVat + '" />'
            + '<input type="hidden" class="IsGST" value="' + item.IsGST + '" />'
            + '<input type="hidden" class="Remark" value="' + item.Remark + '" />'
            + '<input type="hidden" class="Model" value="' + item.Model + '" />'
            + '</td>'
            + '<td class="uk-text-center checked chkValid" data-md-icheck><input type="checkbox" class="chkCheck"  checked/></td>'
            + '<td class="clCode">' + item.Code + '</td>'
            + '<td class="clItem">' + item.Name + '</td>'
           //+ '<td class="clPartsNumber">' + item.PartsNumber + '</td>'
            + '<td class="clUnit uk-hidden">' + item.Unit + '</td>'
            + '<td class="secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
            + '<td class="clQty uk-hidden" ><input type="text"  class="md-input mask-sales2-currency clRqQty included" value="' + item.Qty + '" /></td>'
            + '<td class="secondary"><input type="text"  class="md-input mask-sales2-currency secondaryQty included" value="' + item.Qty + '" /></td>'
            + '<td class="uk-hidden"><input type="text" class="md-input Rate ' + gridcurrencyclass + ' included" value="' + item.Rate + '" /></td>'
            + '<td class="secondary"><input type="text" class="md-input secondaryRate ' + gridcurrencyclass + ' included" value="' + item.Rate + '" /></td>'
            + '<td><input type="text" class="md-input GrossAmount ' + gridcurrencyclass + '" value="' + item.GrossAmount + '" disabled /></td>'
            + '<td><input type="text" class="md-input DiscountPercentage mask-sales2-currency included" value="' + item.DiscountPercentage + '" /></td>'
            + '<td><input type="text" class="md-input DiscountAmount ' + gridcurrencyclass + '" value="' + item.DiscountAmount + ' included" /></td>'
            + '<td><input type="text"  class="md-input clLastPr ' + gridcurrencyclass + '" value="' + item.LastPR + '" disabled /></td>'
            + '<td><input type="text"  class="md-input clLowestPr ' + gridcurrencyclass + '" value="' + item.LowestPR + '" disabled /></td>'
            + '<td><input type="text"  class="md-input clPendingOrderQty ' + gridcurrencyclass + '" value="' + item.PendingOrderQty + '" disabled /></td>'
            + '<td><input type="text"  class="md-input clQtyAvailable ' + gridcurrencyclass + '" value="' + item.QtyAvailable + '" disabled /></td>'
            + '<td><input type="text" class="md-input VATPercentage mask-sales2-currency included" value="' + item.VATPercentage + '" /></td>'
            + '<td><input type="text" class="md-input VATAmount ' + gridcurrencyclass + ' included" value="' + item.VATAmount + '" /></td>'
            + '<td><input type="text" class="md-input TaxableAmount ' + gridcurrencyclass + '" value="' + item.TaxableAmount + '" disabled /></td>'
            + '<td><input type="text" class="md-input NetAmount ' + gridcurrencyclass + '" value="' + item.NetAmount + '" disabled /></td>';
        if (item.ID === 0) {
            html += '<td><button class="md-btn md-btn-primary create-item" >Create</button></td>'
                + '</tr>';
        } else {
            html += '<td>Item exist</td>'
                + '</tr>';
        }
        var $html = $(html);
        app.format($html);
        $("#purchase-order-items-list tbody").append($html);
        self.calculate_grid_total();
    },
    print_preview: function () {
        $("#screen").addClass("uk-hidden");
        $("#print").removeClass("uk-hidden");
        $('.print-actions').removeClass('uk-hidden');
        $('.hidden-print-preview').addClass('uk-hidden');
    },
    close_print_preview: function () {
        $("#screen").removeClass("uk-hidden");
        $("#print").addClass("uk-hidden");
        $('.print-actions').addClass('uk-hidden');
        $('.hidden-print-preview').removeClass('uk-hidden');
    },
    close_preview: function () {
        $("#screen").removeClass("uk-hidden");
        $("#print").addClass("uk-hidden");
    },

    add_item: function () {
        var self = purchase_order;
        self.error_count = 0;
        var batchType = "";
        var batchTypeID = 0;

        self.error_count = self.validate_item();
        if (self.error_count == 0) {

            batchTypeID = $("#BatchType").val();
            batchType = batchTypeID != '' || batchTypeID != null ? $("#BatchType option:selected").text() : "";


            var item = {
                ID: $("#ItemID").val(),
                Code: $("#Code").val(),
                Name: $("#ItemName").val(),
                PartsNumber: $("#PartsNumber").val(),
                Qty: clean($("#Qty").val()),
                Unit: $("#UnitID option:selected").text(),
                UnitID: $("#UnitID option:selected").val(),
                IGSTPercent: 0,
                CGSTPercent: 0,
                SGSTPercent: 0,
                VATPercentage: 0,
                CessPercentage: 0,
                PurchaseRequisitionID: 0,
                PurchaseRequisitionTrasID: 0,
                Value: clean($("#txtValue").val()),
                LastPR: clean($("#LastPr").val()),
                LowestPR: ($("#LowestPr").val()),
                ExchangeRate: clean($("#CurrencyExchangeRate").val()),
                Rate: clean($("#PurchaseRate").val()),
                MRP: clean($("#PurchaseRate").val()),
                CurrencyID: clean($("#CurrencyID").val()),
                IsVat: clean($("#IsVat").val()),
                IsGST: clean($("#IsGST").val()),
                PendingOrderQty: clean($("#PendingOrderQty").val()),
                QtyUnderQC: clean($("#QtyWithQc").val()),
                QtyAvailable: clean($("#QtyAvailable").val()),
                Remark: $("#Remark").val(),
                Model: $("#Model").val(),
                GSTPercentage: clean($('#GSTPercentage').val()),
                GSTAmount: 0,
                PurchaseRequisitionID: "",
                PRTransID: "",
                BatchType: batchType,
                BatchTypeID: batchTypeID,
                FGCategoryID: $("#CategoryID").val(),
                SecondaryUnits: $("#SecondaryUnits").val()
            };
            self.AddProductToPrGrid("", item);
            freeze_header.resizeHeader();
            //Calculate net amount
            //CalculateNetAmountValue();

            purchase_order.count_items();
            self.clear_item();
            $("#ItemName").focus();
        }


    },
    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#Qty").val('');
        $("#Rate").val('');
        $("#UnitID").val('');
        $("#LastPr").val('');
        $("#LowestPr").val('');
        $("#PendingOrderQty").val('');
        $("#QtyOrdered").val('');
        $("#QtyAvailable").val('');
        $("#QtyWithQc").val('');
        $("#txtValue").val('');
        $("#ItemRemarks").val('');
        $("#BatchType").val('');
        $("#PurchaseRate").val('')
        $("#select_batch_type").addClass("uk-hidden").removeClass('visible');
    },

    print: function () {
        window.print();
    },
    select_supplier: function () {
        var self = purchase_order;
        self.remove_all_items_from_grid();
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
        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyName = $(row).find(".CurrencyName").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        var DecimalPlaces = $(row).find(".DecimalPlaces").val();
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        var GSTID = clean($("#GSTID").val());
        if (IsInterCompany == 1) {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", true);
            $(".intercompanySupplierlocation").removeClass("uk-hidden");
            $(".supplierLocation").addClass("uk-hidden");
            self.get_intercompany_location();
        }
        else {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", false);
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }

        $("#SupplierName").val(Name);
        $("#SupplierLocation").val(Location);
        $("#SupplierID").val(ID);
        $("#StateId").val(StateID);
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
        $("#IsInterCompany").val(IsInterCompany);
        $("#Email").val(Email);
        $("#StateId").val() == $("#ShippingStateId").val()
        {

        }
        var checkboxes = "";
        var $checkboxes = $("<div></div>");
        $(".checkbox-container").html('');
        checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
        $checkboxes.append(checkboxes);
        app.format($checkboxes);
        $(".checkbox-container").eq(0).html($checkboxes);
        $("#ItemName").focus();
        self.is_item_supplied_by_supplier();
        self.get_supplier_description();
        self.get_supplier_addresses();
        gridcurrencyclass = app.change_decimalplaces($("#AdvanceAmount"), DecimalPlaces);
        app.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);
        app.change_decimalplaces($("#GrossAmount"), DecimalPlaces);
        app.change_decimalplaces($("#Discount"), DecimalPlaces);
        app.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
        app.change_decimalplaces($("#VATAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppDocAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppShipAmount"), DecimalPlaces);
        app.change_decimalplaces($("#SuppOtherCharge"), DecimalPlaces);
        app.change_decimalplaces($("#NetAmount"), DecimalPlaces);
        $("#DecimalPlaces").val(DecimalPlaces);
        $("#CurrencyID").val(CurrencyID);
        $("#CurrencyName").val(CurrencyName);
        $("#CurrencyCode").val(CurrencyCode);
        $("#CurrencyExchangeRate").val(CurrencyConversionRate);

    },
    get_supplier_addresses: function () {
        var SupplierID = $("#SupplierID").val();
        $.ajax({
            url: '/Masters/Supplier/GetAddresses/',
            dataType: "json",
            data: {
                SupplierID: SupplierID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var html = "";
                    $.each(response.BillingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultBillingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + ', ' + record.AddressLine2 + "</option>";
                    });
                    $("#ShippingToId").html(html);
                    html = "";
                    $.each(response.ShippingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultShippingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + ', ' + record.AddressLine2 + "</option>";
                    });
                    $("#DDLBillTo").html(html);
                    if (response.Message !== undefined) {
                        app.show_error(response.Message);
                    }

                }
            },
            error: function (xhr, status, error) {
                // Handle the error here
                app.show_error(error);
            }
        });
    },
    get_supplier_description: function () {
        var self = purchase_order;

        var SupplierID;
        $(".Description-div").removeClass("uk-hidden");

        SupplierID = clean($('#SupplierID').val());

        supplier.get_description(SupplierID, "Purchase");


    },
    get_details: function () {
        var self = purchase_order;
        var row = $(this).closest('tr');
        var ItemID;
        $(".Description-div").removeClass("uk-hidden");

        if (row.length == 0) {
            ItemID = clean($('#ItemID').val());
        }
        else {
            ItemID = clean($(row).find('.ItemID').val());
        }

        Item.get_description(ItemID, "Purchase");


    },
    set_state_id: function () {
        var state_id = $(this).find("option:selected").data("state-id");
        $("#ShippingStateId").val(state_id);
        //CalculateGSTOutsideTheGrid();
    },
    cancel: function () {
        app.confirm("Do you want to cancel? This can't be undone", function () {
            var PurchaseOrderID = $("#ID").val();
            $.ajax({
                url: '/Purchase/PurchaseOrder/Cancel',
                data: {
                    PurchaseOrderID: PurchaseOrderID,
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Purchase order cancelled successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/PurchaseOrder/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to cancel purchase order");
                    }
                }
            });
        })
    },
    GetPurchaseOrderForSave: function (IsDraft) {
        var ID;
        if (typeof IsDraft === "undefined" || IsDraft === null) {
            IsDraft = false;
        }
        var inCludGST = false;
        var Extra = false;
        if (clean($('#GST').val()) == 1) {
            inCludGST = true;
        } else {
            Extra = true;
        }

        var OtherQ = [];
        $("#other-quotation-list span.view-file").each(function () {
            OtherQ.push($(this).data('id'));
        });
        if ($("#IsClone").val() == "True") {
            ID = 0;
        }
        else {
            ID = $("#ID").val();
        }
        var Model = {
            ID: ID,
            PurchaseOrderNo: $("#PurchaseOrderNo").val(),
            PurchaseOrderDate: $("#PurchaseOrderDate").val(),
            SupplierID: $("#SupplierID").val(),
            AdvancePercentage: clean($("#AdvanceAmount").val()),
            AdvanceAmount: clean($("#AdvanceAmount").val()),
            PaymentModeID: $("#PaymentModeID option:selected").val(),
            ShippingAddressID: $("#ShippingToId").val(),
            BillingAddressID: $("#DDLBillTo").val(),
            IsGST: $("#IsGST").val(),
            IsVat: $("#IsVat").val(),
            CurrencyExchangeRate: clean($("#CurrencyExchangeRate").val()),
            GrossAmount: clean($("#GrossAmount").val()),
            VATAmount: clean($("#VATAmount").val()),
            SuppDocAmount: clean($("#SuppDocAmount").val()),
            SuppShipAmount: clean($("#SuppShipAmount").val()),
            SuppOtherCharge: clean($("#SuppOtherCharge").val()),
            SuppDocCode: $("#SuppDocCode").val(),
            SuppShipCode: $("#SuppShipCode").val(),
            SuppOtherRemark: $("#SuppOtherRemark").val(),
            OrderType: $("#OrderType").val(),
            SuppQuotNo: $("#SuppQuotNo").val(),
            Shipment: $("#Shipment").val(),
            Remarks: $("#Remarks").val(),
            Discount: clean($("#Discount").val()),
            NetAmt: clean($("#NetAmount").val()),//NetAmt: 0,
            InclusiveGST: inCludGST,
            GstExtra: Extra,
            SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
            OtherQuotationIDS: OtherQ.join(","),
            DeliveryWithin: $("#DeliveryWithin").val(),
            PaymentWithinID: $("#DDLPaymentWithin").val(),
            SGSTAmt: 0,
            CGSTAmt: 0,
            IGSTAmt: 0,
            FreightAmt: clean($("#FreightAmt").val()),
            OtherCharges: clean($("#OtherCharges").val()),
            PackingShippingCharge: clean($("#PackingShippingCharge").val()),
            DaysToSupply: $("#DeliveryWithin").val(),
            SupplierReferenceNo: "",
            TermsOfPrice: $("#TermsOfPrice").val(),
            IsDraft: IsDraft,
            IsGSTRegistred: $("#IsGSTRegistred").val(),
            SalesOrderLocationID: $("#LocationID").val()
        }
        return Model;
    },
    GetProductFromGrid: function () {
        var ProductsArray = [];
        $("#purchase-order-items-list tbody tr.included").each(function () {
            var ItemID = clean($(this).find('.ItemID').val());
            var ItemName = $(this).find('.clItem').text().trim();
            var ItemCode = $(this).find('.clCode').text().trim();
            var PartsNumber = $(this).find('.clPartsNumber').text().trim();
            var Remark = $(this).find('.Remark').val();
            var Model = $(this).find('.Model').val();
            var IsGST = $(this).find('.IsGST').val();
            var IsVat = $(this).find('.IsVat').val();

            var PurchaseReqID = clean($(this).find('.PrId').val());
            var PRTransID = clean($(this).find('.PrTransId').val());
            var Quantity = clean($(this).find('.clRqQty').val());
            var Amount = 0;
            var MRP = clean($(this).find('.Rate').val());
            var Rate = clean($(this).find('.Rate').val());
            var SecondaryUnit = $(this).find('.secondaryUnit option:selected').text().trim();
            var SecondaryUnitSize = clean($(this).find('.secondaryUnit').val());;
            var SecondaryRate = clean($(this).find('.secondaryRate').val());
            var SecondaryQty = clean($(this).find('.secondaryQty').val());
            var GrossAmount = clean($(this).find('.GrossAmount').val());
            var DiscountPercentage = clean($(this).find('.DiscountPercentage').val());
            var DiscountAmount = clean($(this).find('.DiscountAmount').val());
            var VATPercentage = clean($(this).find('.VATPercentage').val());
            var VATAmount = clean($(this).find('.VATAmount').val());

            var TaxableAmount = clean($(this).find('.TaxableAmount').val());
            var NetAmount = clean($(this).find('.NetAmount').val());
            var LastPurchaseRate = clean($(this).find('.clLastPr').val());
            var LowestPR = clean($(this).find('.clLowestPr').val());

            var ExchangeRate = clean($(this).find('.ExchangeRate').val());
            var CurrencyID = clean($(this).find('.CurrencyID').val());

            var QtyInQC = clean($(this).find('.clQtyWithQc').val());
            var QtyOrdered = clean($(this).find('.clPendingOrderQty').val());
            var QtyAvailable = clean($(this).find('.clQtyAvailable').val());
            var UnitID = clean($(this).find('.UnitID').val());
            var Tot_GST_persc = clean($(this).find('.clGstPerscnt  .txtClGSTPer').val());
            var Tot_GST_Amt = 0;
            var QtyMet = 0;
            var ExpDate = $("#PurchaseOrderDate").val();
            var SGSTPercent = Tot_GST_persc / 2;
            var CGSTPercent = Tot_GST_persc / 2;
            var IGSTPercent = Tot_GST_persc;
            var PurchaseRequisitionID = clean($(this).find('.PurchaseRequisitionID').val());
            var PurchaseRequisitionTrasID = clean($(this).find('.PurchaseRequisitionTrasID').val());
            var SGSTAmt = 0;
            var CGSTAmt = 0;
            var IGSTAmt = 0;
            var BatchTypeID = clean($(this).find('.clBatch').val());

            if ($("#StateId").val() == $("#ShippingStateId").val()) {
                SGSTAmt = Tot_GST_Amt / 2;
                CGSTAmt = Tot_GST_Amt / 2;
            } else {
                IGSTAmt = Tot_GST_Amt;
            }

            ProductsArray.push({
                PurchaseRequisitionID: PurchaseRequisitionID,
                PurchaseRequisitionTrasID: PurchaseRequisitionTrasID,
                ItemID: ItemID,
                ItemCode: ItemCode,
                ItemName: ItemName,
                PartsNumber: PartsNumber,
                Remark: Remark,
                Model: Model,
                IsGST: IsGST,
                IsVat: IsVat,
                PurchaseReqID: PurchaseReqID,
                PRTransID: PRTransID,
                Quantity: Quantity,
                Amount: Amount,
                LastPurchaseRate: LastPurchaseRate,
                LowestPR: LowestPR,
                ExchangeRate: ExchangeRate,
                VATPercentage: VATPercentage,
                VATAmount: VATAmount,
                CurrencyID: CurrencyID,
                QtyInQC: QtyInQC,
                QtyAvailable: QtyAvailable,
                QtyOrdered: QtyOrdered,
                BatchTypeID: BatchTypeID,
                UnitID: UnitID,
                ExpDate: ExpDate,
                QtyMet: QtyMet,
                BatchNo: "",
                Rate: Rate,
                MRP: MRP,
                SecondaryUnit: SecondaryUnit,
                SecondaryUnitSize: SecondaryUnitSize,
                SecondaryRate: SecondaryRate,
                SecondaryQty: SecondaryQty,
                GrossAmount: GrossAmount,
                DiscountPercent: DiscountPercentage,
                Discount: DiscountAmount,
                TaxableAmount: TaxableAmount,
                SGSTPercent: SGSTPercent,
                CGSTPercent: CGSTPercent,
                IGSTPercent: IGSTPercent,
                SGSTAmt: SGSTAmt,
                CGSTAmt: CGSTAmt,
                IGSTAmt: IGSTAmt,
                NetAmount: NetAmount
            });

        })
        return ProductsArray;
    },
    set_supplier_details: function (event, item) {   // on select auto complete item

        var self = purchase_order;
        self.remove_all_items_from_grid();
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#IsInterCompany").val(item.isintercompany);
        $("#Email").val(item.email);

        $("#InterCompanyLocationID").val(item.interCompanyLocationid);
        $("#DDLPaymentWithin option:contains(" + item.paymentDays + ")").attr("selected", true);
        $("#SupplierReferenceNo").focus();
        var GSTID = clean($("#GSTID").val());
        if (item.isintercompany == 1) {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", true);
            $(".intercompanySupplierlocation").removeClass("uk-hidden");
            $(".supplierLocation").addClass("uk-hidden");
            self.get_intercompany_location();
        }
        else {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", false);
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }
        //  $("#SupplierID").trigger('change');
        var checkboxes = "";
        var $checkboxes = $("<div></div>");
        $(".checkbox-container").html('');
        checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
        $checkboxes.append(checkboxes);
        app.format($checkboxes);
        $(".checkbox-container").eq(0).html($checkboxes);
        $("#ItemName").focus();
        self.is_item_supplied_by_supplier();
        self.get_supplier_description();

        gridcurrencyclass = app.change_decimalplaces($("#AdvanceAmount"), item.decimalplaces);
        app.change_decimalplaces($("#CurrencyExchangeRate"), item.decimalplaces);
        $("#DecimalPlaces").val(item.decimalplaces);
        $("#CurrencyID").val(item.currencyid);
        $("#CurrencyName").val(item.currencyname);
        $("#CurrencyCode").val(item.CurrencyCode);
        $("#CurrencyConversionRate").val(item.currencyconversionrate);
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
    get_intercompany_location: function (release) {
        $.ajax({
            url: '/Masters/Location/getInterCompanyLocation',
            data: {
                LocationID: clean($('#InterCompanyLocationID').val())
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#LocationID").html("");
                var html = "<option value >Select</option>";
                $.each(data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#LocationID").append(html);
            }
        });
    },
    set_item_details: function (event, item) {   // on select auto complete item
        var self = purchase_order;
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
    },
    select_item_confirm: function () {
        var self = purchase_order;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        if (radio.length > 0) {
            self.select_item(radio, row);
        }

    },
    select_item: function (radio, row) {
        var self = purchase_order;
        var GetSupplierID = clean($("#SupplierID").val());
        if (GetSupplierID == 0) {
            app.show_error("Please Select Supplier Or ExchangeRate is not set for the Supplier Currency.");
            return;
        }

        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var PartsNumber = $(row).find(".PartsNumber").text().trim();
        var Remark = $(row).find(".Remark").text().trim();
        var Model = $(row).find(".Model").text().trim();
        var PrimaryUnit = $(row).find(".PrimaryUnit").val();
        var PrimaryUnitID = $(row).find(".PrimaryUnitID").val();
        var PurchaseUnit = $(row).find(".PurchaseUnit").val();
        var PurchaseUnitID = $(row).find(".PurchaseUnitID").val();
        var lastPr = $(row).find(".lastPr").val();
        var lowestPr = $(row).find(".lowestPr").val();
        var pendingOrderQty = $(row).find(".pendingOrderQty").val();
        var qtyWithQc = $(row).find(".qtyWithQc").val();
        var qtyAvailable = $(row).find(".qtyAvailable").val();
        var gstPercentage = $(row).find(".gstPercentage").val();
        var itemCategoryID = $(row).find(".finishedGoodsCategoryID").val();
        var purchaseMRP = clean($(row).find(".purchaseMRP").val());
        var SecondaryUnits = $(row).find(".SecondaryUnits").val();
        $("#ItemID").val(ID);
        $("#ItemName").val(Name);
        $("#Code").val(Code);
        $("#PartsNumber").val(PartsNumber);
        $("#Remark").val(Remark);
        $("#Model").val(Model);
        //$("#Rate").val('');
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
        $("#SecondaryUnits").val(SecondaryUnits);
        purchaseMRP = purchaseMRP * clean($("#CurrencyExchangeRate").val());
        $("#PurchaseRate").val(purchaseMRP);
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

    },
    get_units: function () {
        var self = purchase_order;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
        //html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);
    },
    get_item_details: function (release) {

        $.ajax({
            url: '/Purchase/PurchaseOrder/getProductList',
            data: {
                Areas: 'PurchaseOrder',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
                SupplierId: $("#SupplierID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_purchase_category: function () {
        var item_category_id = $("#DDLItemCategory").val();
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
    remove_file: function () {
        $(this).parent('li').remove();
        var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
        $('#file-count').text(file_count + " File(s)");
    },
    remove_quotation: function () {
        $(this).closest('span').remove();
    },
    remove_item: function () {
        $(this).closest('tr').remove();
        purchase_order.count_items();
        freeze_header.resizeHeader();
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('disabled');
        purchase_order.count_items();
        purchase_order.change_grid_total_calculations();
        //CalculateGSTInsideGrid($(this).closest('tr'));
        //CalculateGSTOutsideTheGrid();
        //CalculateNetAmountValue();
    },
    error_count: 0,
    validate_item: function () {
        var self = purchase_order;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_supplier: function () {
        var self = purchase_order;
        if (self.rules.on_item.length > 0) {
            return form.validate(self.rules.on_item);
        }
        return 0;
    },
    count_items: function () {
        var count = $('#purchase-order-items-list tbody').find('input.chkCheck:checked').length;
        $('#item-count').val(count);
    },
    validate_form: function () {
        var self = purchase_order;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_requsition: function () {
        var self = purchase_order;
        if (self.rules.on_requsition.length > 0) {
            return form.validate(self.rules.on_requsition);
        }
        return 0;
    },
    validate_draft: function () {
        var self = purchase_order;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    rules: {
        on_item: [

            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                    { type: form.required, message: "Please select supplier" },
                ]
            },

        ],
        on_requsition: [
            {
                elements: "#GST",
                rules: [
                    { type: form.required, message: "Please select GST type" },
                ]
            },
        ],
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please select an item" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = 0;
                            var itemid = clean($(element).val());
                            var isintercompany = clean($('#IsInterCompany').val());

                            if (isintercompany == 1) {
                                $("#purchase-order-items-list tbody tr .ItemID[value='" + itemid + "']").each(function () {
                                    count = 1;

                                });
                            }
                            return count == 0

                        }, message: "Item already exist"
                    },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                    { type: form.required, message: "Please select supplier" },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Please enter valid quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },
            {
                elements: "#Rate",
                rules: [
                    { type: form.required, message: "Please enter rate" },
                    { type: form.positive, message: "Please enter positive rate" },
                    { type: form.non_zero, message: "Please enter rate" },
                ]
            },
            {
                elements: "#GST",
                rules: [
                    { type: form.required, message: "Please select GST type" },
                ]
            },
            {
                elements: ".visible #BatchType:visible",
                rules: [
                    { type: form.required, message: "Please select batch type" },

                ]
            },
        ],
        on_draft: [
            {
                elements: "#GST",
                rules: [
                    { type: form.required, message: "Please select GST type" },
                ]
            },
            {
                elements: "#LocationID",
                rules: [

                    {
                        type: function (element) {
                            var error = false;
                            var isintercompany = clean($('#IsInterCompany').val());
                            var locationid = clean($('#LocationID').val());
                            if (isintercompany == 1 && locationid == 0)
                                error = true;
                            return !error;
                        }, message: 'Please select supplier location'
                    },
                ]
            },
            {
                elements: "#purchase-order-items-list tbody tr .ItemID",
                rules: [
                    {
                        type: function (element) {
                            var ItemID = clean($(element).val());
                            return ItemID > 0 ? true : false;
                        },
                        message: 'Item not created, Create Item First.'

                    }
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = $("#purchase-order-items-list tbody tr.included.ineligible").length;

                            if (count > 0)
                                error = true;
                            return !error;
                        }, message: 'Some of the items in grid are not supplied by supplier please select another supplier'
                    },
                ]
            },
            {
                elements: "#DeliveryWithin",
                rules: [
                    { type: form.required, message: "Please enter delivery within days" },
                    /*{ type: form.non_zero, message: "Please enter delivery within days" },*/
                    //{ type: form.positive, message: "Please enter positive delivery within days" },
                    //{ type: form.numeric, message: "Please enter valid delivery within days" },
                ]
            },
            {
                elements: "#PaymentModeID",
                rules: [
                    { type: form.required, message: "Please select payment mode" },
                    {
                        type: function (element) {
                            var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                            var NetAmt = clean($("#NetAmount").val());
                            var PaymentMode = $("#PaymentModeID option:selected").text();
                            var error = false;
                            if (CashPaymentLimit < NetAmt && PaymentMode.toLowerCase() == "cash") {
                                error = true;
                            }
                            return !error;
                        }, message: 'Please select another payment mode'
                    },
                ]
            },
            {
                elements: ".clBatch.enable:visible",
                rules: [
                    {
                        type: function (element) {
                            var batchid = clean($(element).val());
                            var error = false;
                            if (batchid == 0) {
                                error = true;
                            }
                            return !error;
                        }, message: 'Please select batch type'
                    },
                ]
            },


            {
                elements: "#NetAmount",
                rules: [
                    { type: form.positive, message: "Invalid net amount" },
                ]
            },
            {
                elements: ".clRqQty.included",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },

            {
                elements: "#FreightAmt",
                rules: [
                    { type: form.positive, message: "Please enter positive freight amount" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());

                            if (NetAmt < sum)
                                error = true;


                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges should be less than net amount'
                    },

                ]
            },
            //{
            //    elements: ".txtClGSTAmt.included",
            //    rules: [
            //        { type: form.required, message: "Please enter GST Amount" },
            //        { type: form.positive, message: "Please enter positive value for txtClGSTAmt" },
            //    ]
            //},
            {
                elements: "#OtherCharges",
                rules: [
                    { type: form.positive, message: "Please enter positive other charges" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());

                            if (NetAmt < sum)
                                error = true;


                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
                    },

                ]
            },
            {
                elements: "#AdvanceAmount",
                rules: [
                    { type: form.positive, message: "Please enter positive advance amount" },

                    {
                        type: function (element) {
                            var error = false;
                            var advance = clean($('#AdvanceAmount').val());

                            var NetAmt = clean($('#NetAmount').val());

                            if (advance > NetAmt)
                                error = true;


                            return !error;
                        }, message: 'Advance amount exceeds net amount'
                    },
                ]
            },
            {
                elements: "#PackingShippingCharge",
                rules: [
                    { type: form.positive, message: "Please enter positive shipping charges" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());
                            if (NetAmt < sum)
                                error = true;
                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
                    },
                ]
            },
        ],
        on_submit: [
            {
                elements: ".clBatch.enable:visible",
                rules: [
                    {
                        type: function (element) {
                            var batchid = clean($(element).val());
                            var error = false;
                            if (batchid == 0) {
                                error = true;
                            }
                            return !error;
                        }, message: 'Please select batch type'
                    },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = $("#purchase-order-items-list tbody tr.included.ineligible").length;

                            if (count > 0)
                                error = true;
                            return !error;
                        }, message: 'Some of the items in grid are not supplied by selected supplier, please select another supplier'
                    },
                ]
            },
            {
                elements: "#purchase-order-items-list tbody tr .ItemID",
                rules: [
                    {
                        type: function (element) {
                            var ItemID = clean($(element).val());
                            return ItemID > 0 ? true : false;
                        },
                        message: 'Item not created, Create Item First.'

                    }
                ]
            },
            {
                elements: "#CurrencyExchangeRate",
                rules: [
                    { type: form.required, message: "Invalid Currency Exchange Rate" },
                    { type: form.positive, message: "Invalid Currency Exchange Rate" },
                    { type: form.numeric, message: "Invalid Currency Exchange Rate" },
                    { type: form.non_zero, message: "Currency Exchange Rate is non zero" },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier", alt_element: "#SupplierName" },
                    { type: form.required, message: "Please select supplier", alt_element: "#SupplierName" },
                ]
            },
            {
                elements: "#ShippingToId",
                rules: [
                    { type: form.required, message: "Please select shipping location" },
                ]
            },
            {
                elements: "#DDLBillTo",
                rules: [
                    { type: form.required, message: "Please select billing location" },
                ]
            },
            {
                elements: "#PaymentModeID",
                rules: [
                    { type: form.required, message: "Please select payment mode" },
                    {
                        type: function (element) {
                            var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                            var NetAmt = clean($("#NetAmount").val());
                            var PaymentMode = $("#PaymentModeID option:selected").text();
                            var error = false;
                            if (CashPaymentLimit < NetAmt && PaymentMode.toLowerCase() == "cash") {
                                error = true;
                            }
                            return !error;
                        }, message: 'Please select another payment mode'
                    },
                ]
            },
            //{
            //    elements: "#DDLPaymentWithin",
            //    rules: [
            //        { type: form.required, message: "Please select payment within days" },
            //    ]
            //},
            {
                elements: "#DeliveryWithin",
                rules: [
                    { type: form.required, message: "Please enter delivery within days" },
                    //{ type: form.positive, message: "Please enter positive  delivery within days" },
                    //{ type: form.numeric, message: "Please enter valid delivery within days" },
                    //{ type: form.non_zero, message: "Please enter delivery within days" },

                    //{
                    //    type: function (element) {
                    //        var error = false;
                    //        var deliverywithin = clean($('#DeliveryWithin').val());
                    //        if (deliverywithin >= 365)
                    //            error = true;
                    //        return !error;
                    //    }, message: 'Delivery within exceeds 365 days'
                    //},
                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
            {
                elements: ".clRqQty.included",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },
            {
                elements: ".txtclRate.included",
                rules: [
                    { type: form.required, message: "Please enter rate" },
                    { type: form.positive, message: "Please enter positive rate" },
                    { type: form.non_zero, message: "Please enter rate" },
                ]
            },
            {
                elements: ".txtclValue.included",
                rules: [
                    { type: form.required, message: "Please enter value" },
                    { type: form.positive, message: "Please enter positive value" },
                    { type: form.non_zero, message: "Please enter value" },
                ]
            },
            {
                elements: "#FreightAmt",
                rules: [
                    { type: form.positive, message: "Please enter positive freight mmount" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());
                            if (NetAmt < sum)
                                error = true;
                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
                    },
                ]
            },
            {
                elements: ".txtClGSTAmt.included",
                rules: [
                    { type: form.required, message: "Please enter GST amount" },
                    { type: form.positive, message: "Please enter positive GST amount" },
                ]
            },
            {
                elements: "#OtherCharges",
                rules: [
                    { type: form.positive, message: "Please enter positive other charges" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());
                            if (NetAmt < sum)
                                error = true;
                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
                    },
                ]
            },
            {
                elements: "#AdvanceAmount",
                rules: [
                    { type: form.positive, message: "Please enter positive advance amount" },
                    {
                        type: function (element) {
                            var error = false;
                            var advance = clean($('#AdvanceAmount').val());

                            var NetAmt = clean($('#NetAmount').val());

                            if (advance > NetAmt)
                                error = true;


                            return !error;
                        }, message: 'Advance amount exceeds net amount'
                    },

                ]
            },
            {
                elements: "#PackingShippingCharge",
                rules: [
                    { type: form.positive, message: "Please enter positive shipping charges" },
                    {
                        type: function (element) {
                            var error = false;
                            var FrightAmt = clean($('#FreightAmt').val());
                            var OtherCharges = clean($('#OtherCharges').val());
                            var ShippingCharges = clean($('#PackingShippingCharge').val());
                            var sum = FrightAmt + OtherCharges + ShippingCharges;
                            var NetAmt = clean($('#NetAmount').val());
                            if (NetAmt < sum)
                                error = true;
                            return !error;
                        }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
                    },
                ]
            },

            {
                elements: "#LocationID",
                rules: [

                    {
                        type: function (element) {
                            var error = false;
                            var isintercompany = clean($('#IsInterCompany').val());
                            var locationid = clean($('#LocationID').val());
                            if (isintercompany == 1 && locationid == 0)
                                error = true;
                            return !error;
                        }, message: 'Please select supplier location'
                    },
                ]
            },
            {
                elements: ".clBatch.enable:visible",
                rules: [
                    { type: form.required, message: "Please select batch type" },
                ]
            },
        ]
    },
    selected_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },
    other_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            //percent = Math.ceil(percent);
            // bar.css("width", percent + "%").text(percent + "%");
        },
        complete: function (response, xhr) {
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                var dropdown = $("#other-quotation-list .uk-nav-dropdown");
                width = $('#other-quotation-list').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        dropdown.append("<li class='file-list'>"
                            + "<a class='remove-file'>X</a>"
                            + "<span data-id='" + record.ID + "' style='width:" + width + "px;' class='view-file' data-url='" + record.URL + "' data-path='" + record.Path + "'>"
                            + record.Name
                            + "</span>"
                            + "</li>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            // app.show_notice("Uploaded");
            //console.log(response);

            var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
            $('#file-count').text(file_count + " File(s)");
        }
    }
};

purchase_orderCRUD = {
    purchaseOCreateAndUpdate: function () {

        //Add products to pr grid

        //function clearItemSelectControls() {
        //    var self = purchase_order;
        //    self.clear_item();
        //}
        $(".btnSelectReqstion").click(function () {
            if ($("#SupplierID").val() > 0) {
                UIkit.modal('#select_pr', { center: false }).show();
            } else {
                app.show_error("Please select supplier")
            }
        });

        $(".btnSaveAndMail").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var _Master = self.GetPurchaseOrderForSave();
                var _Trans = self.GetProductFromGrid();
                var IsSendMail = 1
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })

            }
        });
        $(".btnSavePO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var _Master = self.GetPurchaseOrderForSave();
                var _Trans = self.GetProductFromGrid();
                var IsSendMail = 0
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })
            }
        });
        $(".btnSaveASDraftPO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_draft();
            if (self.error_count == 0) {
                var _Master = self.GetPurchaseOrderForSave(true);
                var _Trans = self.GetProductFromGrid();
                var IsSendMail = 0
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })
            }
        });
        function SavePO(_Master, _Trans, IsSendMail) {
            var self = purchase_order;
            var url;
            if (_Master.IsDraft == true) {
                url = '/Purchase/PurchaseOrder/SaveAsDraft';
            }
            else {
                url = '/Purchase/PurchaseOrder/Save';
            }
            $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'none' });
            $.ajax({
                url: url,
                data: { PO: _Master, Details: _Trans },
                dataType: "json",
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.Status == "success") {
                        $("#ID").val(data.Data.ID);
                        if (IsSendMail == 1) {
                            self.send_mail();
                        }
                        app.show_notice("Purchase order saved successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/PurchaseOrder/Index";
                        }, 1000);
                    } else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        //app.show_error(data.Message);
                        $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'block' });
                    }
                },
            });
        }


    }
};
Number.prototype.roundToCustom = function () {

    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};
//Common Calcultions

//calculate GST outside the grid
//function CalculateGSTOutsideTheGrid() {
//    var TotalGSTAmount = 0;
//    $(".poTbody .rowPO.included").each(function () {
//        var obj = clean($(this).find(".clGstAmount .txtClGSTAmt").val());
//        TotalGSTAmount = TotalGSTAmount + obj;
//    });
//    //if ($("#StateId").val() == $("#ShippingStateId").val()) {
//    //    $("#SGSTAmt").val(TotalGSTAmount / 2);
//    //    $("#CGSTAmt").val(TotalGSTAmount / 2);
//    //    $("#IGSTAmt").val(0);
//    //} else {
//    //    $("#SGSTAmt").val(0);
//    //    $("#CGSTAmt").val(0);
//    //    $("#IGSTAmt").val(TotalGSTAmount);
//    //}
//}

//function CalculateGST(e) {
//    var Rate = clean(e.find(".clRate .txtclRate").val());
//    var Qty = clean(e.find(".clQty .clRqQty").val());
//    var percent = clean(e.find(".clGstPerscnt .txtClGSTPer").val());
//    if ($("#IsGSTRegistred").val().toLowerCase() == "true") {
//        if ($('#GST').val() == 1) {

//            var vRate = Rate * 100 / (100 + percent);
//            var GstAmount = (Rate - vRate) * Qty;
//            var Total = (vRate * Qty) + GstAmount;

//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        } else if ($('#GST').val() == 2) {
//            var vRate = Rate;
//            var GstAmount = Rate * Qty * percent / 100;
//            var Total = (Rate * Qty) + GstAmount;
//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        } else {
//            var vRate = Rate;
//            var GstAmount = 0;
//            var Total = (Rate * Qty) + GstAmount;
//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        }
//    } else {
//        e.find(".clGstAmount .txtClGSTAmt").val(0);
//        e.find(".clTotal").val(Rate * Qty);
//    }
//}

//CalculateGST Inside the grid
//function CalculateGSTInsideGrid(e) {
//    var Rate = clean(e.find(".clRate .txtclRate").val());
//    var Qty = clean(e.find(".clQty .clRqQty").val());
//    var percent = clean(e.find(".clGstPerscnt .txtClGSTPer").val());
//    if ($("#IsGSTRegistred").val().toLowerCase() == "true") {
//        if ($('#GST').val() == 1) {

//            var vRate = Rate * 100 / (100 + percent);
//            var GstAmount = (Rate - vRate) * Qty;
//            var Total = (vRate * Qty) + GstAmount;

//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        } else if ($('#GST').val() == 2) {
//            var vRate = Rate;
//            var GstAmount = Rate * Qty * percent / 100;
//            var Total = (Rate * Qty) + GstAmount;
//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        } else {
//            var vRate = Rate;
//            var GstAmount = 0;
//            var Total = (Rate * Qty) + GstAmount;
//            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
//            e.find(".clTotal").val(Total);
//        }
//    } else {
//        e.find(".clGstAmount .txtClGSTAmt").val(0);
//        e.find(".clTotal").val(Rate * Qty);
//    }
//    CalculateGSTOutsideTheGrid();
//    //CalculateNetAmountValue();
//}

//function CalculateNetAmountValue() {
//    var TotalProductAmount = 0;
//    $("#purchase-order-items-list tbody tr.included").each(function () {
//        TotalProductAmount = TotalProductAmount + clean($(this).find(".clTotal").val());
//    });
//    var FreightAmt = clean($("#FreightAmt").val());
//    var OtherCharges = clean($("#OtherCharges").val());
//    var PackingShippingCharge = clean($("#PackingShippingCharge").val());
//    var NetAmt = TotalProductAmount + FreightAmt + OtherCharges + PackingShippingCharge;
//    $("#NetAmt").val(NetAmt);
//}

//function CalculateValueInGrid(parent) {
//    e = parent.closest('tr');
//    var Rate = clean(e.find(".clRate .txtclRate").val());
//    var Qty = clean(e.find(".clQty .clRqQty").val());
//    e.find(".clValue .txtclValue").val(Rate * Qty);
//    CalculateGSTInsideGrid(e);
//}


//purchase_order_CalculateEvents = {
//    calculations: function () {
//        $(document).on('keyup', '.txtClGSTPer', function () {
//            CalculateGSTInsideGrid($(this).closest('tr'));
//        });
//        $(document).on('change', '.txtClGSTPer', function () {
//            CalculateGSTInsideGrid($(this).closest('tr'));
//        });
//        $('#GST').on('change', function () {
//            if ($(this).val() == null) {
//                return;
//            }
//            $("#purchase-order-items-list tbody tr .clRqQty").each(function () {
//                CalculateValueInGrid($(this));
//            });
//        })

//        $(document).on('keyup', '.clRqQty , .txtclRate', function () {
//            CalculateValueInGrid($(this).closest('tr'));
//        });
//        $(document).on('change', '.clRqQty , .txtclRate', function () {
//            CalculateValueInGrid($(this).closest('tr'));
//        });

//        $(document).on('keyup', '#FreightAmt,#OtherCharges,#PackingShippingCharge', function () {
//            //Calculate net amount
//            CalculateNetAmountValue();
//        });
//        $(document).on('change', '#FreightAmt,#OtherCharges,#PackingShippingCharge', function () {
//            //Calculate net amount
//            CalculateNetAmountValue();
//        });

//        $("#Qty, #Rate").keyup(function () {

//            $("#txtValue").val(clean($("#Rate").val()) * clean($("#Qty").val()));
//        });
//    },
//}