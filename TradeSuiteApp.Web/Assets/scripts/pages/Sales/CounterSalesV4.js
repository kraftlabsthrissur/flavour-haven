
var normalclass = '';
var DecPlaces = 0;
$(function () {
    normalclass = $("#normalclass").val();
    DecPlaces = clean($("#DecimalPlaces").val());
    //alert(normalclass);
    $('#CashSales, #PartyName').on('change', CounterSales.reset_customerdetails);
    CounterSales.calculate_balancee();

});
CounterSales.init = function () {
    var self = CounterSales;
    //Employee.employee_list('FreeMedicineEmployeeList');
    //item_select_table = $('#employee-list').SelectTable({
    //    selectFunction: self.select_employee,
    //    modal: "#select-employee",
    //    initiatingElement: "#EmployeeName"
    //});

    //supplier.Doctor_list();
    //$('#doctor-list').SelectTable({
    //    selectFunction: self.select_doctor,
    //    returnFocus: "#ItemName",
    //    modal: "#select-doctor",
    //    initiatingElement: "#DoctorName"
    //});
    //Patient.patient_list();
    //$('#patient-list').SelectTable({
    //    selectFunction: self.select_patient,
    //    returnFocus: "#PartyName",
    //    modal: "#select-patient",
    //    initiatingElement: "#PartyName"
    //});


    self.customer_list();

    self.purchase_order_history();
    self.pending_po_history();
    self.purchase_legacy_history();
    self.on_change_tab_history();

    self.item_list();
    self.contact_list();
    $('#item-list').SelectTable({
        selectFunction: self.select_item,
        returnFocus: "#Qty",
        modal: "#select-item",
        initiatingElement: "#ItemName",
        startFocusIndex: 3
    });
    if ($("#ID").val() > 0) {
        var LocationID = $("#current-location select").val();
        if ($("#TypeID option:selected").text().toLowerCase() == "employee" && LocationID == POL_LOCATIONID) {
            $("#BatchTypeID").val(1);
        }
    }

    self.hide_show_sales_type();
    self.bind_events();
    self.hide_show_elements();
    setTimeout(function () {
        fh_items = $("#counter-sales-items-list").FreezeHeader();
    }, 50);
    $("#CashSales").focus();
    $('#tabs-counter-sales-create').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            active_item.data('tab-loaded', true);
        }
    });
    self.calculate_balance();


};
CounterSales.on_change_tab_history = function () {
    var self = CounterSales;
    $('#tabs-counter-sales-history').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            var type = active_item.data('tab');
            if (type == "Quotation") {
                self.sales_order_history(type);
            } else if (type == "Sales") {
                self.counter_sales_history();
            }
            active_item.data('tab-loaded', true);
        }
    });
};
CounterSales.customer_list = function (type) {
    var url = "/Masters/Customer/GetCustomerList";

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
CounterSales.contact_list = function (type) {
    var url = "/Masters/Contact/GetContactList";

    var $list = $('#contact-list');
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
                        return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >"
                            + "<input type='hidden' class='Firstname' value='" + row.Firstname + "'>"
                            + "<input type='hidden' class='Lastname' value='" + row.Lastname + "'>";
                    }
                },
                { "data": "Firstname", "className": "Firstname" },
                { "data": "Lastname", "className": "Lastname" },
                { "data": "ContactNo", "className": "ContactNo" },
                { "data": "AlternateNo", "className": "AlternateNo" },
                { "data": "Designation", "className": "Designation" },
                { "data": "Email", "className": "Email" },
            ],
            "drawCallback": function () {
                altair_md.checkbox_radio();
                $list.trigger("datatable.changed");
            },
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
CounterSales.item_list = function () {
    var $list = $('#item-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs();
        var url = "/Masters/Item/GetPurchaseItemsList";
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
                            + "<input type='hidden' class='Model' value='" + row.Model + "'>"
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
                            + "<input type='hidden' class='MaxSalesQtyFull' value='" + row.MaxSalesQtyFull + "'>"
                            + "<input type='hidden' class='MinSalesQtyLoose' value='" + row.MinSalesQtyLoose + "'>"
                            + "<input type='hidden' class='MinSalesQtyFull' value='" + row.MinSalesQtyFull + "'>"
                            + "<input type='hidden' class='MaxSalesQtyLoose' value='" + row.MaxSalesQtyLoose + "'>"
                            + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>";
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
                        return "<div class='" + normalclass + "' >" + row.Stock + "</div>";
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

CounterSales.purchase_order_history = function () {

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
                        return "<div class='" + normalclass + "' >" + row.LandedCost + "</div>";
                    }
                },
                {
                    "data": "SecondaryRate", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + normalclass + "' >" + row.SecondaryRate + "</div>";
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
                        return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
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
CounterSales.pending_po_history = function () {

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
                        return "<div class='" + normalclass + "' >" + row.LandedCost + "</div>";
                    }
                },
                {
                    "data": "SecondaryRate", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + normalclass + "' >" + row.SecondaryRate + "</div>";
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
                        return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
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
CounterSales.purchase_legacy_history = function () {

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
                        return "<div class='" + normalclass + "' >" + row.Quantity + "</div>";
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
                        return "<div class='" + normalclass + "' >" + row.TaxAmount + "</div>";
                    }
                },
                {
                    "data": "NetAmount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
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
CounterSales.sales_order_history = function (type) {

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
                        return "<div class='" + normalclass + "' >" + row.SecondaryMRP + "</div>";
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
                        return "<div class='" + normalclass + "' >" + row.TaxableAmount + "</div>";
                    }
                },
                {
                    "data": "NetAmount", "className": "", "searchable": false,
                    "render": function (data, type, row, meta) {
                        return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
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
CounterSales.bind_events = function () {
    var self = CounterSales;
    $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
    $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
    $.UIkit.autocomplete($('#doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
    $("body").on("keyup change", "#counter-sales-items-list .Qty, .Rate, .DiscountAmount, .DiscountPercentage", self.change_price);
    $("body").on("keyup change", "#counter-sales-items-list tbody tr .Secondary .SecondaryQty, .SecondaryUnit, .SecondaryRate", self.on_secondary_unit_change);
    $("body").on("keyup", "#counter-sales-items-list .Name, .PartsNumber", self.change_print_details);
    $("body").on("change", "#PrintWithItemName", self.change_print_details);
    $.UIkit.autocomplete($('#add-doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $('#add-doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
    $("body").on("keyup change", "#counter-sales-items-list .Qty, .Rate,.SecondaryRate", self.check_stock);


    $('body').on('click', '#counter-sales-items-list tbody td:not(.action)', self.show_batch_with_stock);
    $('body').on('click', '#counter-sales-items-list tbody tr td.showitemhistory', self.showitemhistory);
    $("#btnOKItem").on("click", self.select_item);
    $("#btn-ok-appointment-process").on("click", self.select_patient);
    $("#btnOKDoctor").on("click", self.select_doctor);
    $("#btnAddPatient").on("click", self.show_add_patient);
    $("#btnAddProduct").on("click", self.add_item);
    $(".btnSave, .btnSaveAndNew, .btnSaveASDraft, .btnSaveAndPrint").on("click", self.on_save);
    $("body").on("click", ".remove-item", self.remove_item);
    $("body").on("click", ".cancel", self.cancel_confirm);
    $("#btnOKCustomer").on("click", self.select_customer);
    $("#btnOKContact").on("click", self.select_contact);
    $("body").on("keyup change", "#batch-list .BatchQty", self.set_total_quantity);
    $('body').on('click', '#btnOkBatches', self.replace_batches);
    $("#btnOkAddPatient").on("click", self.save_patient);
    $("#PackingPrice").on("keyup change", self.calculate_packing_charge);
    $("#PaymentModeID").on("change", self.hide_show_elements);
    $("body").on("keyup change", "#TotalAmountReceived", self.calculate_balance);
    $("body").on("keydown", "#Qty", self.trigger_add_item);
    $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
    $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
    $("#TypeID").on("change", self.hide_show_sales_type);
    $("#btnOKEmployee").on("click", self.select_doctor);
    $('body').on('change', "#PaymentModeID", self.get_bank_name);
    $("#TypeID").on("change", self.set_batchtype);
    $("#DiscountPercentageList").on('change', self.calculate_discount_Amount);
    $("#BusinessCategoryID").on('change', self.remove_all_items_from_grid);
    $("#btnOKEmployees").on("click", self.select_employees);
    $("body").on("click", "#btnClosePrint", self.print_close);
    $(document).on('keydown', null, 'alt+p', self.on_save_print);
    $(document).on('keydown', null, 'alt+n', self.on_save_new);
    $(document).on('keydown', null, 'alt+d', self.on_save_draft);
    $(document).on('keydown', null, 'alt+l', self.on_close);
    $(document).on('keydown', null, 'alt+c', self.on_cancel);
    $("body").on("keydown", "#BarCode", self.on_change_bar_code);
    $("#btnReset").on("click", self.on_rest);
    $("#BillDiscount, #BillDiscountPercent").on("keyup", self.on_change_discount_Amount);
    $("#VATPercentageID").on("change", self.on_change_discount_Amount);
};
CounterSales.reset_customerdetails = function () {
    $("#CustomerID").val(0);
    $("#ContactID").val(0);
    $("#ContactName").val('');
    $("#CustomerID").trigger("change");
};

CounterSales.change_print_details = function () {
    var self = CounterSales;
    self.add_amount_details();
};
CounterSales.change_price = function () {

    var self = CounterSales;
    var className = $(this).prop('class');
    var row = $(this).closest('tr');
    // self.check_stock(row, className);
    self.check_stock();
};
CounterSales.on_secondary_unit_change = function () {
    var self = CounterSales;
    var $row = $(this).closest('tr');
    var SecondaryQty = clean($row.find('.SecondaryQty').val());
    var SecondaryUnitSize = clean($row.find('.SecondaryUnit').val());
    if ($(this).attr('class').indexOf('SecondaryRate') != -1) {
        var SecondaryRate = clean($row.find('.SecondaryRate').val());
        var Rate = (SecondaryRate / SecondaryUnitSize).toFixed(10);
        $row.find('.Rate').val(Rate);
        $row.find('.Rate').trigger('change');

    } else {
        var Rate = clean($row.find('.Rate').val());
        var SecondaryRate = Rate * SecondaryUnitSize;
        $row.find('.Secondary .SecondaryRate').val(SecondaryRate);
        var Qty = (SecondaryQty * SecondaryUnitSize).toFixed(10);
        $row.find('.Qty').val(Qty);
        $row.find('.Qty').trigger('change');
    }

};
//CounterSales.check_stock = function (row, className) {

//    var self = CounterSales;

//    var IssueQty = clean($(row).find('.Qty').val());
//    var AvailableStock = clean($(row).find('.Stock').val());
//    var Rate = clean($(row).find('.Rate').val());
//    var Quantity = clean($(row).find(".Qty").val());
//    var BasicPrice = clean($(row).find('.BasicPrice').val());
//    var DiscountPercentage = clean($(row).find('.DiscountPercentage').val());
//    var DiscountAmount = clean($(row).find('.DiscountAmount').val());
//    var GST = clean($(row).find(".GST").val());
//    var GrossAmount;
//    var CessPercentage = clean($(row).find('.CessPercentage').val());
//    var discount = 0;// $("#DiscountPercentageList option:selected").data('value');
//    var IsVAT = clean($(row).find('.IsVAT').val());
//    var IsGST = clean($(row).find('.IsGST').val());
//    var VAT = clean($(row).find('.VAT').val());
//    var TaxableAmount = 0;
//    var NetAmount = 0;
//    var GSTAmount = 0;
//    var CGST = 0;
//    var SGST = 0;
//    var IGST = 0;
//    var CessAmount = 0;
//    if (IssueQty > AvailableStock) {
//        app.show_error('Selected item dont have enough stock ');
//        app.add_error_class($(row).find('.Qty'));
//        return;
//    }
//    if (IsGST == 1) {
//        if (Sales.is_cess_effect()) {
//            BasicPrice = (Rate * 100 / (100 + GST + CessPercentage));
//            CessAmount = (BasicPrice * (Quantity) * CessPercentage / 100);
//        } else {
//            BasicPrice = (Rate * 100 / (100 + GST));
//            CessAmount = 0;
//        }
//        GrossAmount = (BasicPrice * Quantity);
//        if (typeof className !== 'undefined' && className.indexOf('DiscountAmount') != -1) {
//            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
//            $(row).find('.DiscountPercentage').val(DiscountPercentage.roundToCustom());
//        } else if (typeof className !== 'undefined' && className.indexOf('DiscountPercentage') != -1) {
//            DiscountAmount = (GrossAmount * DiscountPercentage / 100);
//            $(row).find('.DiscountAmount').val(DiscountAmount.roundToCustom());
//        } else {
//            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
//            $(row).find('.DiscountPercentage').val(DiscountPercentage.roundToCustom());
//        }
//        TaxableAmount = (GrossAmount - DiscountAmount);
//        GSTAmount = (TaxableAmount * GST / 100);
//        CGST = (GSTAmount / 2);
//        SGST = (GSTAmount / 2);
//        NetAmount = (TaxableAmount + CGST + SGST + CessAmount);
//    }
//    else if (IsVAT == 1) {
//        if (Sales.is_cess_effect()) {
//            BasicPrice = (Rate * 100 / (100 + VAT + CessPercentage));
//            CessAmount = (BasicPrice * (Quantity) * CessPercentage / 100);
//        } else {
//            BasicPrice = (Rate * 100 / (100 + VAT));
//            CessAmount = 0;
//        }
//        GrossAmount = (BasicPrice * Quantity);
//        if (typeof className !== 'undefined' && className.indexOf('DiscountAmount') != -1) {
//            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
//            $(row).find('.DiscountPercentage').val(DiscountPercentage.roundToCustom());
//        } else if (typeof className !== 'undefined' && className.indexOf('DiscountPercentage') != -1) {
//            DiscountAmount = (GrossAmount * DiscountPercentage / 100);
//            $(row).find('.DiscountAmount').val(DiscountAmount.roundToCustom());
//        } else {
//            DiscountPercentage = (DiscountAmount / GrossAmount * 100);
//            $(row).find('.DiscountPercentage').val(DiscountPercentage.roundToCustom());
//        }
//        TaxableAmount = (GrossAmount - DiscountAmount);
//        VATAmount = (TaxableAmount * VAT / 100);
//        NetAmount = (TaxableAmount + VATAmount + CessAmount);
//    }

//    $(row).find(".GrossAmount").val(GrossAmount);
//    $(row).find(".TaxableAmount").val(TaxableAmount.roundToCustom());
//    $(row).find(".CGST").val(CGST.roundToCustom());
//    $(row).find(".SGST").val(SGST.roundToCustom());
//    $(row).find(".IGST").val(IGST.roundToCustom());
//    $(row).find(".VATAmount").val(VATAmount.roundToCustom());
//    $(row).find(".NetAmount").val(NetAmount.roundToCustom());
//    $(row).find(".CessAmount").val(CessAmount.roundToCustom());
//    $(row).find(".BasicPrice").val(BasicPrice);
//    self.calculate_grid_total();

//};
//CounterSales: check_stock = function () {
//    var self = CounterSales;
//    var row = $(this).closest('tr');
//    var IssueQty = clean($(row).find('.Qty').val());
//    var AvailableStock = clean($(row).find('.Stock').val());
//    var Rate = clean($(row).find('.Rate').val());

//    var DiscountAmount;
//    var GST = clean($(row).find(".GST").val());
//    var Quantity = clean($(row).find(".Qty").val());
//    var BasicPrice = clean($(row).find('.BasicPrice').val());
//    var GrossAmount;
//    var CessPercentage = clean($(row).find('.CessPercentage').val());
//    var discount = $("#DiscountPercentageList option:selected").data('value');
//    var TaxableAmount;
//    var GSTAmount;
//    var NetAmount;
//    var CGST;
//    var SGST;
//    var NetAmount;
//    var CessAmount;
//    if (IssueQty > AvailableStock) {
//        app.show_error('Selected item dont have enough stock ');
//        app.add_error_class($(row).find('.Qty'));
//        return;
//    }

//    if (Sales.is_cess_effect()) {
//        BasicPrice = (Rate * 100 / (100 + GST + CessPercentage)).roundTo(2);
//        CessAmount = (BasicPrice * (Quantity) * CessPercentage / 100).roundTo(2);
//    } else {
//        BasicPrice = (Rate * 100 / (100 + GST)).roundTo(2);
//        CessAmount = 0;
//    }
//    GrossAmount = (BasicPrice * Quantity).roundTo(2);

//    DiscountAmount = GrossAmount * (discount / 100);
//    TaxableAmount = GrossAmount - DiscountAmount;
//    GSTAmount = (TaxableAmount * GST / 100).roundTo(2);
//    CGST = (GSTAmount / 2).roundTo(2);
//    SGST = (GSTAmount / 2).roundTo(2);
//    NetAmount = (TaxableAmount + CGST + SGST + CessAmount).roundTo(2);

//    $(row).find(".GrossAmount").val(GrossAmount);
//    $(row).find(".TaxableAmount").val(TaxableAmount);
//    $(row).find(".CGST").val(CGST);
//    $(row).find(".SGST").val(SGST);
//    $(row).find(".NetAmount").val(NetAmount);
//    $(row).find(".CessAmount ").val(CessAmount);
//    $(row).find(".DiscountAmount ").val(DiscountAmount);
//    $(row).find(".BasicPrice ").val(BasicPrice);
//    self.calculate_grid_total();

//};
CounterSales.select_contact = function () {
    var self = CounterSales;
    var radio = $('#select-Contact tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var FName = $(row).find(".Firstname").val();
    var LName = $(row).find(".Lastname").val();
    var Name = FName + " " + LName;
    $("#ContactID").val(ID);
    $("#ContactName").val(Name);
    UIkit.modal($('#select-Contact')).hide();
};
CounterSales.select_customer = function () {
    var self = CounterSales;
    var radio = $('#select-customer tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    $("#CashSales").val(Name);
    $("#PartyName").val(Name);
    $("#CustomerID").val(ID);
    $("#ContactID").val(0);
    $("#ContactName").val('');
    $("#CustomerID").trigger("change");
    UIkit.modal($('#select-customer')).hide();
};
CounterSales.clear_item = function () {
    var self = CounterSales;
    $("#ItemID").val('');
    $("#Unit").val('');
    $("#UnitID").val('');
    $("#Batch").val('');
    $("#FullOrLooseID").val(1);
    $("#CGSTPercentage").val('');
    $("#IGSTPercentage").val('');
    $("#SGSTPercentage").val('');
    $("#DiscountPercentage").val('');
    $("#Qty").val('');
    $("#Rate").val('');
    setTimeout(function () {
        $("#ItemName").val('');
    }, 100);
    $("#BarCode").val("");
    $("#CashSales").focus();

};
CounterSales.on_change_bar_code = function () {
    var self = CounterSales;
    var BarCode = $("#BarCode").val();
    //BarCode = BarCode.substr(0, BarCode.indexOf('-'));
    $.ajax({
        url: '/Masters/Batch/GetBatchDetailsByBatch',
        data: {
            BatchID: BarCode,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data.Status == "Success") {
                var CGSTPercentage = (data.Data.GSTPercentage / 2);
                $("#ItemName").val(data.Data.ItemName);
                $("#ItemID").val(data.Data.ItemID);
                //$("#Code").val(Code);
                $("#Unit").val(data.Data.Unit);
                $("#UnitID").val(data.Data.UnitID);
                $("#Stock").val(data.Data.Stock);
                $("#CGSTPercentage").val(data.Data.CGSTPercentage);
                $("#IGSTPercentage").val(data.Data.GSTPercentage);
                $("#SGSTPercentage").val(data.Data, CGSTPercentage);
                $("#Rate").val(data.Data.RetailLooseRate);
                $("#PrimaryUnit").val(data.Data.Unit);
                $("#PrimaryUnitID").val(data.Data.UnitID);
                $("#SalesUnit").val(data.Data.Unit);
                $("#SalesUnitID").val(data.Data.UnitID);
                $("#CessPercentage").val(data.Data.CessPercentage);
                $("#Qty").focus();
                self.get_units();
                $("#ItemName").prop("disabled", true);
                $("#ItemID").prop("disabled", true)
                $("#BarCode").val(BarCode);

            }
        },
    });
};

CounterSales.on_rest = function () {
    var self = CounterSales;
    $("#ItemName").prop("disabled", false);
    $("#ItemID").prop("disabled", false)
    $("#ItemName").val("");
    $("#ItemID").val("");
    $("#Unit").val("");
    $("#UnitID").val("");
    $("#Stock").val("");
    $("#CGSTPercentage").val("");
    $("#IGSTPercentage").val("");
    $("#SGSTPercentage").val("");
    $("#SGSTPercentage").val("");
    $("#TotalVATAmount").val("");
    $("#Rate").val("");
    $("#PrimaryUnit").val("");
    $("#PrimaryUnitID").val("");
    $("#SalesUnit").val("");
    $("#SalesUnitID").val("");
    $("#CessPercentage").val("");
    $("#BarCode").val("");
    $("#CashSales").focus();
    self.get_units();
};
//CounterSales.on_change_discount_Percentage = function () {
//    var self = CounterSales;
//    var discountpercent = clean($("#BillDiscountPercent").val());
//    var GrossAmt = clean($("#GrossAmount").val());

//    $("#BillDiscount").val(discount);
//    self.on_change_discount_Amount();
//};

CounterSales.on_change_discount_Amount = function () {

    var self = CounterSales;
    var elementid = $(this).attr('id');
    var GrossAmt = clean($("#GrossAmount").val());
    var discount = 0;
    var VATPercentage = clean($("#VATPercentageID option:selected").text());
    var discountpercent = 0;
    if (elementid == 'BillDiscountPercent') {
        discountpercent = clean($("#BillDiscountPercent").val());
        discount = (GrossAmt * discountpercent / 100);
    } else if (elementid == 'BillDiscount') {
        discount = clean($("#BillDiscount").val());
        discountpercent = (discount / GrossAmt * 100);
    }
    var discountper = ((discount * 100) / GrossAmt);
    var TotalVATAmount = 0;
    $("#counter-sales-items-list tbody tr").each(function () {

        $row = $(this).closest("tr");
        var GrossAmount = clean($row.find(".GrossAmount").val());
        var SGSTPercentage = clean($row.find(".SGSTPercentage").val());
        var CGSTPercentage = clean($row.find(".CGSTPercentage").val());
        var CessPercentage = clean($row.find(".CessPercentage").val());
        var IGSTPercentage = clean($row.find(".IGSTPercentage").val());
        //var VATPercentage = clean($row.find(".VAT").val());

        var DiscountAmount = (GrossAmount * (discountper / 100));
        var DiscountPercentage = (DiscountAmount / GrossAmount * 100);
        var TaxableAmount = (GrossAmount - DiscountAmount);
        //var TaxableAmount = GrossAmount - TaxableAmt;
        IGSTAmount = TaxableAmount * (IGSTPercentage / 100);
        SGSTAmount = TaxableAmount * (SGSTPercentage / 100);
        CGSTAmount = TaxableAmount * (CGSTPercentage / 100);
        CessAmount = TaxableAmount * (CessPercentage / 100);
        VATAmount = TaxableAmount * (VATPercentage / 100);
        var NetAmount = TaxableAmount + CessAmount + CGSTAmount + SGSTAmount + VATAmount;
        $row.find(".TaxableAmount").val(TaxableAmount.roundToCustom());
        $row.find(".CessAmount").val(CessAmount.roundToCustom());
        $row.find(".CGST").val(CGSTAmount.roundToCustom());
        $row.find(".SGST").val(SGSTAmount.roundToCustom());
        $row.find(".IGST").val(IGSTAmount.roundToCustom());
        $row.find(".VAT").val(VATPercentage.roundToCustom());
        $row.find(".VATPercentage").val(VATPercentage.roundToCustom());
        $row.find(".VATAmount").val(VATAmount.roundToCustom());
        $row.find(".NetAmount").val(NetAmount.roundToCustom());
        $row.find(".DiscountAmount").val(DiscountAmount.roundToCustom());
        $row.find(".DiscountPercentage").val(DiscountPercentage.roundToCustom());
        TotalVATAmount += $row.find(".VATAmount").val(VATAmount.roundToCustom());
    });
    //self.calculate_grid_total();
    if (elementid == 'BillDiscountPercent') {
        $("#DiscountAmt").val(discount.roundToCustom());
        $("#BillDiscount").val(discount.roundToCustom());
    } else if (elementid == 'BillDiscount') {
        var discountpercent = (discount / GrossAmt * 100);
        $("#BillDiscountPercent").val(discountpercent.roundToCustom());
    } else {
        $("#DiscountAmt").val(discount);
        $("#BillDiscount").val(discount);
        var discountpercent = (discount / GrossAmt * 100);
        $("#BillDiscountPercent").val(discountpercent.roundToCustom());
    }
    self.calculate_grid_total();
};
CounterSales.get_item_properties = function (item) {
    var self = CounterSales;
    var IsVATExtra = $("#IsVATExtra").val();
    /*if (item.IsVAT == 1) {*/
        item.VATPercentage = $("#VATPercentageID option:selected").text();
    /*}*/
    if (item.IsGST == 1 && item.IsGSTRegisteredLocation) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = (item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage));
            item.CessAmount = (item.BasicPrice * (item.Quantity) * item.CessPercentage / 100);
        } else {
            item.BasicPrice = (item.MRP * 100 / (100 + item.IGSTPercentage));
            item.CessAmount = 0;
        }
        var discount = $("#DiscountPercentageList option:selected").data('value');

        item.GrossAmount = (item.BasicPrice * item.Quantity);
        //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
        TaxableAmt = (item.GrossAmount * (discount / 100));
        item.DiscountAmount = (item.GrossAmount * (discount / 100));
        item.TaxableAmount = (item.GrossAmount - TaxableAmt);
        GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100);

        item.CGST = (GSTAmount / 2);
        item.SGST = (GSTAmount / 2);
        item.NetAmount = (item.TaxableAmount + item.CGST + item.SGST + item.CessAmount);

        return item;
    }
    else if (item.IsGST == 1) {
        item.BasicPrice = item.MRP;

        var discount = $("#DiscountPercentageList option:selected").data('value');

        item.GrossAmount = (item.BasicPrice * item.Quantity);
        //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
        TaxableAmt = (item.GrossAmount * (discount / 100));
        item.DiscountAmount = (item.GrossAmount * (discount / 100));
        item.TaxableAmount = (item.GrossAmount - TaxableAmt);
        GSTAmount = 0;

        item.CGST = 0;
        item.SGST = 0;
        item.NetAmount = (item.TaxableAmount + item.CGST + item.SGST + item.CessAmount);

        return item;
    } else if (item.IsVAT == 1) {
        if (Sales.is_cess_effect()) {
            item.BasicPrice = (item.MRP * 100 / (100 + item.VATPercentage + item.CessPercentage));
            item.CessAmount = (item.BasicPrice * (item.Quantity) * item.CessPercentage / 100);
        } else {
            item.BasicPrice = (item.MRP * 100 / (100 + item.VATPercentage));
            item.CessAmount = 0;
        }
        ///added by anju
        if (IsVATExtra == 1) {
            item.BasicPrice = item.MRP;
        }
        var discount = $("#DiscountPercentageList option:selected").data('value');

        item.GrossAmount = (item.BasicPrice * item.Quantity);
        //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
        TaxableAmt = (item.GrossAmount * (discount / 100));
        item.DiscountAmount = (item.GrossAmount * (discount / 100));
        item.TaxableAmount = (item.GrossAmount - TaxableAmt);
        VATAmount = (item.TaxableAmount * item.VATPercentage / 100);

        item.VATAmount = (VATAmount);
        item.NetAmount = (item.TaxableAmount + item.VATAmount + item.CessAmount);

        return item;
    }
    else {

        item.BasicPrice = (item.MRP * 100 / (100));
        item.CessAmount = 0;
        var discount = $("#DiscountPercentageList option:selected").data('value');

        item.GrossAmount = (item.BasicPrice * item.Quantity);
        //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
        TaxableAmt = (item.GrossAmount * (discount / 100));
        item.DiscountAmount = (item.GrossAmount * (discount / 100));
        item.TaxableAmount = (item.GrossAmount - TaxableAmt);
        VATAmount = (item.TaxableAmount * item.VATPercentage / 100);

        item.VATAmount = (VATAmount);
        item.NetAmount = (item.TaxableAmount);

        return item;
    }

};
CounterSales.add_item = function () {

    var self = CounterSales;
    self.error_count = 0;
    self.error_count = self.validate_item();
    if (self.error_count > 0) {
        return;
    }
    $.ajax({
        url: '/Sales/CounterSales/GetBatchwiseItemForCounterSales',
        dataType: "json",
        data: {
            ItemID: $('#ItemID').val(),
            WarehouseID: $('#StoreID').val(),
            BatchTypeID: $("#BatchTypeID").val(),
            Qty: clean($('#Qty').val()),
            UnitID: $("#UnitID").val(),
            TaxTypeID: $("#TaxTypeID").val(),
            Unit: $("#UnitID option:selected").text(),
            CustomerType: $("#TypeID option:selected").text().toLowerCase(),
            PartsNumber: $("#PartsNumber").val(),
        },
        type: "POST",
        success: function (response) {

            if (response.Status == "success") {
                self.process_item(response.Data);
                $("#ItemName").focus();
                //console.log(JSON.stringify(response.Data));
            } else {
                app.show_error(response.Message)
            }
        },
    });


    setTimeout(function () {
        $("#ItemName").focus();
    }, 100);
};
CounterSales.SelectSecondaryUnits = function (Unit, SecondaryUnits) {
    var select = '<select class="md-input label-fixed SecondaryUnit">';
    select += '<option value="1" selected>' + Unit + '</option>';
    if (SecondaryUnits != null) {
        var optionsSecondaryUnits = SecondaryUnits.split(',');
        optionsSecondaryUnits.forEach(function (option) {
            var parts = option.split('|');
            if (parts.length > 1) {
                var text = parts[0];
                var value = parts[1];
                select += '<option value="' + value + '">' + text + '</option>';
            }
        });
        select += '</select>';
    }
    return select;
};
CounterSales.add_items_to_grid = function (items) {

    var self = CounterSales;
    var index, GSTAmount, tr = '';
    var readonly = '';
    var discount = $("#DiscountPercentageList option:selected").data('value');
    $.each(items, function (i, item) {
        //console.log(JSON.stringify(item));
        //if (item.SalesUnitID == item.UnitID) {
        //    item.MRP = item.FullPrice;
        //    item.BasicPrice = item.BasicPrice;
        //}
        //else {
        //    item.MRP = item.LoosePrice;
        //}
        item.MRP = clean($("#Rate").val());
        item = self.get_item_properties(item);
        index = $("#counter-sales-items-list tbody tr").length + 1;
        if (item.IsGST == 1 && item.IsGSTRegisteredLocation) {
            var Gst = item.CGSTPercentage + item.SGSTPercentage;
        } else {
            GST = 0;
        } if (item.IsVAT == 1) {
            VAT = item.VATPercentage;
        }
        VAT = item.VATPercentage;
        if (clean($("#IsPriceEditable").val()) == 0) {
            readonly = ' readonly="readonly"';
        }
        if (item.IsGST == 1 && item.IsGSTRegisteredLocation) {
            tr += '<tr>'
                + ' <td class="uk-text-center showitemhistory action">'
                + '     <a class="view-itemhistory">'
                + '         <i class="uk-icon-eye-slash"></i>'
                + '     </a>'
                + ' </td>'
                + ' <td class="uk-text-center index">' + index + ' </td>'
                + ' <td >' + item.Code
                + '     <input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                + '     <input type="hidden" class="ItemCode" value="' + item.Code + '"  />'
                + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '     <input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '     <input type="hidden" class="IsVAT" value="' + item.IsVAT + '"  />'
                + '     <input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
                + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                + '     <input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '     <input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '     <input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                + '     <input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
                + '     <input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
                + '     <input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + '<td class="action"> <input type="text" class="md-input Name" value="' + item.Name + '" /></td >'
                + '<td class="action"><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '" /></td>'
                + '<td class="action"><input type="text" class="md-input Model" value="' + item.Model + '" /></td>'
                + '<td class="uk-hidden">' + item.Unit + '</td>'
                + '<td class="secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
                + ' <td>' + item.CurrencyName + '</td>'
                + ' <td class="action"><input type="text" class="md-input Rate "+ normalclass+"" value="' + item.MRP + '" /></td>'
                + ' <td ><input type="text" class="BasicPrice ' + normalclass + '" value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + ' <td class="action"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + ' <td ><input type="text" class="md-input GrossAmount ' + normalclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input TaxableAmount ' + normalclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + ' <td class="action"><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + ' <td class="action"><input type="text" class="md-input DiscountAmount ' + normalclass + '" value="' + item.DiscountAmount + '" /></td>'
                + ' <td ><input type="text" class="md-input GST ' + normalclass + '" value="' + Gst + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input CGST ' + normalclass + '" value="' + item.CGST + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input SGST ' + normalclass + '" value="' + item.SGST + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input IGST ' + normalclass + '" value="' + item.IGST + '" readonly="readonly" /></td>'
                + ' <td class="cess-enabled"><input type="text" class="md-input CessPercentage ' + normalclass + '" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                + ' <td class="cess-enabled"><input type="text" class="md-input CessAmount ' + normalclass + '" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input NetAmount ' + normalclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + ' <td class="uk-text-center action">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        } else if (item.IsGST == 1) {
            tr += '<tr>'
                + ' <td class="uk-text-center showitemhistory action">'
                + '     <a class="view-itemhistory">'
                + '         <i class="uk-icon-eye-slash"></i>'
                + '     </a>'
                + '</td>'
                + '<td class="uk-text-center index">' + index + ' </td>'
                + '<td >' + item.Code
                + '<input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="ItemCode" value="' + item.Code + '"  />'
                + '<input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '<input type="hidden" class="IsVAT" value="' + item.IsVAT + '"  />'
                + '<input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
                + '<input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                + '<input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                + '<input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                + '<input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                + '<input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '<input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '<input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                + '<input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
                + '<input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
                + '<input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + '< td > <input type="text" class="md-input Name" value="' + item.Name + '" /></td >'
                + '<td><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '" /></td>'
                + '<td class="action"><input type="text" class="md-input Model" value="' + item.Model + '" /></td>'
                + '<td class="uk-hidden">' + item.Unit + '</td>'
                + '<td class="Secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
                + '<td>' + item.CurrencyName + '</td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Rate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryRate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td ><input type="text" class="BasicPrice ' + normalclass + '" value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryQty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td ><input type="text" class="md-input GrossAmount ' + normalclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input TaxableAmount ' + normalclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountAmount ' + normalclass + '" value="' + item.DiscountAmount + '" /></td>'
                + '<td ><input type="text" class="md-input NetAmount ' + normalclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        } else if (item.IsVAT == 1) {
            tr += '<tr>'
                + '<td class="uk-text-center showitemhistory action">'
                + '    <a class="view-itemhistory">'
                + '        <i class="uk-icon-eye-slash"></i>'
                + '    </a>'
                + '</td>'
                + '<td class="uk-text-center index">' + index + ' </td>'
                + '<td >' + item.Code
                + '<input type="hidden" class="ItemCode" value="' + item.Code + '"  />'
                + '<input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '<input type="hidden" class="IsVAT" value="' + item.IsVAT + '"  />'
                + '<input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
                + '<input type="hidden" class="VATPercentage" value="' + item.VATPercentage + '" />'
                + '<input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                + '<input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '<input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '<input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                + '<input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
                + '<input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
                + '<input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + '<td class="action"><input type="text" class="md-input Name" value="' + item.Name + '" /></td >'
                + '<td class="action"><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '" /></td>'
                + '<td class="action"><input type="text" class="md-input Model" value="' + item.Model + '" /></td>'
                + '<td class="uk-hidden">' + item.Unit + '</td>'
                + '<td class="Secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
                + '<td>' + item.CurrencyName + '</td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Rate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryRate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td ><input type="text" class="BasicPrice ' + normalclass + ' " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryQty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td ><input type="text" class="md-input GrossAmount ' + normalclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input TaxableAmount ' + normalclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountAmount ' + normalclass + '" value="' + item.DiscountAmount + '" /></td>'
                + '<td ><input type="text" class="md-input VAT ' + normalclass + '" value="' + VAT + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input VATAmount ' + normalclass + '" value="' + item.VATAmount + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input NetAmount ' + normalclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        }
        else {
            tr += '<tr>'
                + '<td class="uk-text-center showitemhistory action">'
                + '    <a class="view-itemhistory">'
                + '        <i class="uk-icon-eye-slash"></i>'
                + '    </a>'
                + '</td>'
                + '<td class="uk-text-center index">' + index + ' </td>'
                + '<td >' + item.Code
                + '<input type="hidden" class="ItemCode" value="' + item.Code + '"  />'
                + '<input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                + '<input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '<input type="hidden" class="CurrencyID" value="' + item.CurrencyID + '"  />'
                + '<input type="hidden" class="IsVAT" value="' + item.IsVAT + '"  />'
                + '<input type="hidden" class="IsGST" value="' + item.IsGST + '"  />'
                + '<input type="hidden" class="VATPercentage" value="' + item.VATPercentage + '" />'
                + '<input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                + '<input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '<input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '<input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                + '<input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
                + '<input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
                + '<input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + '<td class="action"><input type="text" class="md-input Name" value="' + item.Name + '" /></td >'
                + '<td class="action"><input type="text" class="md-input PartsNumber" value="' + item.PartsNumber + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DeliveryTerm" value="' + item.DeliveryTerm + '" /></td>'
                + '<td class="action"><input type="text" class="md-input Model" value="' + item.Model + '" /></td>'
                + '<td class="uk-hidden">' + item.Unit + '</td>'
                + '<td class="Secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
                + '<td>' + item.CurrencyName + '</td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Rate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryRate ' + normalclass + '" value="' + item.MRP + '" /></td>'
                + '<td ><input type="text" class="BasicPrice ' + normalclass + ' " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + '<td class="uk-hidden action"><input type="text" class="md-input Qty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td class="Secondary action"><input type="text" class="md-input SecondaryQty mask-sales2-currency" value="' + item.Quantity + '"  /></td>'
                + '<td ><input type="text" class="md-input GrossAmount ' + normalclass + '" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input TaxableAmount ' + normalclass + '" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountPercentage mask-sales2-currency" value="' + item.DiscountPercentage + '" /></td>'
                + '<td class="action"><input type="text" class="md-input DiscountAmount ' + normalclass + '" value="' + item.DiscountAmount + '" /></td>'
                + '<td ><input type="text" class="md-input VAT ' + normalclass + '" value="' + VAT + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input VATAmount ' + normalclass + '" value="' + item.VATAmount + '" readonly="readonly" /></td>'
                + '<td ><input type="text" class="md-input NetAmount ' + normalclass + '" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + '<td class="uk-text-center action">'
                + '    <a class="remove-item">'
                + '        <i class="uk-icon-remove"></i>'
                + '    </a>'
                + '</td>'
                + '</tr>';
        }

    });
    $("#item-count").val(index);
    var $tr = $(tr);
    app.format($tr);
    $("#counter-sales-items-list tbody").append($tr);
    fh_items.resizeHeader();
    self.calculate_grid_total();
    self.clear_item();
    self.count_items();
    self.count();

};
CounterSales.calculate_grid_total = function () {

    var self = CounterSales;
    var GrossAmount = 0;
    var DiscountAmount = 0;
    var DiscountPercentage = 0;
    var TaxableAmount = 0;
    var SGSTAmount = 0;
    var CGSTAmount = 0;
    var IGSTAmount = 0;
    var VATAmount = 0;
    var IsGST = 0;
    var IsVAT = 0;
    var NetAmount = 0;
    var RoundOff = 0;
    var temp = 0;
    var CessAmount = 0;
    var packingcharge = clean($("#PackingPrice").val());
    $("#counter-sales-items-list tbody tr").each(function () {
        GrossAmount += clean($(this).find('.GrossAmount').val());
        DiscountAmount += clean($(this).find('.DiscountAmount').val());
        TaxableAmount += clean($(this).find('.TaxableAmount').val());
        SGSTAmount += clean($(this).find('.SGST').val());
        CGSTAmount += clean($(this).find('.CGST').val());
        IGSTAmount += clean($(this).find('.IGST').val());
        IsGST = clean($(this).find('.IsGST').val());
        IsVAT = clean($(this).find('.IsVAT').val());
        VATAmount += clean($(this).find('.VATAmount').val());
        NetAmount += clean($(this).find('.NetAmount').val());
        CessAmount += clean($(this).find('.CessAmount').val());
       // AmountRecieveds += clean($(this).find('.NetAmount').val());
    });

    NetAmount += packingcharge;
    //temp = NetAmount;
    //NetAmount = Math.round(NetAmount);
    RoundOff = 0;//temp - NetAmount;
    $("#GrossAmount").val(GrossAmount.roundToCustom());
    $("#DiscountAmt").val(DiscountAmount.roundToCustom());
    $("#BillDiscount").val(DiscountAmount.roundToCustom());
    DiscountPercentage = (DiscountAmount / GrossAmount * 100).roundToCustom();
    $("#BillDiscountPercent").val(DiscountPercentage);
    $("#TaxableAmt").val(TaxableAmount.roundToCustom());
    $("#SGSTAmount").val(SGSTAmount.roundToCustom());
    $("#CGSTAmount").val(CGSTAmount.roundToCustom());
    $("#IGSTAmount").val(IGSTAmount.roundToCustom());
    $("#TotalVATAmount").val(VATAmount.roundToCustom());
    $("#RoundOff").val(RoundOff.roundToCustom());
    $("#NetAmount").val(NetAmount.roundToCustom());
    $("#AmountRecieveds").val(NetAmount.roundToCustom());
    $("#CessAmount").val(CessAmount.roundToCustom());
    self.add_amount_details();
   self.calculate_balance();
};
CounterSales.calculate_balancee = function () {
    var self = CounterSales;
    var NetAmount = $("#NetAmount").val();
    var AmountRecieveds = $("#AmountRecieveds").val();
    var balance = NetAmount - AmountRecieveds;
    $("#BalanceToBePaid").val(balance);


},
   CounterSales. calculate_balance=function () {
        var self = CounterSales;
        var NetAmnt = clean($("#NetAmount").val());
        var TotalAmnt = clean($("#TotalAmountReceived").val());
        $("#BalanceToBePaid").val('');
        if (TotalAmnt > 0) {
            var Balance = TotalAmnt - NetAmnt;
            $("#BalanceToBePaid").val(Balance);
        }
    },


CounterSales.add_amount_details = function () {

    var self = CounterSales;
    //var amt_details = [];
    var index;
    var j = 1;
    var GST = 0;
    var SGST = 0;
    var CGST = 0;
    var IGST = 0;
    var CessPercentage, CessAmount;
    var IsInclusiveGST = false;
    var tr = "";
    var j = 1;
    var tax_percentages = $('.tax-template').html();
    var inclusive_gst = "";
    self.amt_details = [];
    $("#counter-sales-items-list tbody tr").each(function () {
        $row = $(this).closest("tr");
        GST = clean($row.find(".GST").val());
        VAT = clean($row.find(".VAT").val());
        IsVAT = clean($row.find(".IsVAT").val());
        IsGST = clean($row.find(".IsGST").val());
        SGST = parseFloat($row.find(".SGSTPercentage").val());
        CGST = parseFloat($row.find(".CGSTPercentage").val());
        SGSTamount = parseFloat($row.find(".SGST").val());
        CGSTamount = parseFloat($row.find(".CGST").val());
        VATAmount = parseFloat($row.find(".VATAmount").val());
        CessAmount = clean($row.find(".CessAmount").val());
        CessPercentage = clean($row.find(".CessPercentage").val());
        DiscountAmount = clean($row.find(".DiscountAmount").val());
        DiscountPercentage = clean($row.find(".DiscountPercentage ").val());
        Name = $row.find(".Name").val();
        PartsNumber = $row.find(".PartsNumber").val();
        NetAmount = clean($row.find(".NetAmount ").val());
      //0  AmountRecieveds = clean($row.find(".NetAmount ").val());
        //  IGST = parseFloat($row.find(".IGST").val());
        //if (IsGST == 1) {
        //    self.calculate_group_amount_details(SGST, SGSTamount, "SGST");
        //    self.calculate_group_amount_details(CGST, CGSTamount, "CGST");
        //    self.calculate_group_amount_details(CessPercentage, CessAmount, "Cess");
        //}
        //if (IsVAT == 1) {
        //    self.calculate_group_amount_details(VAT, VATAmount, "VAT");
        //    self.calculate_group_amount_details(CessPercentage, CessAmount, "Cess");
        //}
        var isPrintWithName = $("#PrintWithItemName").is(":checked");
        if (isPrintWithName)
            self.calculate_group_amount_details(0, NetAmount, Name);
        else
            self.calculate_group_amount_details(0, NetAmount, PartsNumber);
        // self.calculate_group_amount_details(GST, IGST, "IGST");
    });


    $.each(self.amt_details, function (i, record) {

        tr += "<tr  class='uk-text-center'>";
        tr += "<td>" + (i + 1);
        tr += "</td>";
        tr += "<td class='Particulars'>" + record.particular;
        tr += "</td>";
        tr += "<td><input type='text' class='md-input " + normalclass + " Percentage' readonly value='" + record.tax_percentage.roundToCustom() + "' />";
        tr += "</td>";
        tr += "<td><input type='text' class='md-input " + normalclass + " Amount' readonly value='" + record.amount.roundToCustom() + "' />";
        tr += "</td>";
        tr += "</tr>";
    });
    $tr = $(tr);
    app.format($tr);
    $("#sales-invoice-amount-details-list tbody").html($tr);
};
//Amount details grouping
CounterSales.calculate_group_amount_details = function (GST, value, type) {

    var self = CounterSales;
    if (value == 0) {
        return;
    }
    index = self.search_amount_group(self.amt_details, GST, value, type);
    if (index == -1) {
        self.amt_details.push(
            {
                particular: type,
                amount: value,
                tax_percentage: GST
            })
    }
    else {
        self.amt_details[index].amount += value;
    }
};
CounterSales.on_close = function () {
    var self = CounterSales;
    var location = "/Sales/CounterSales/Index";
    window.location = location;
};
CounterSales.get_data = function () {
    var item = {};
    var AmountDetails = {};
    var data = {};
    var SalesTypeName = $("#TypeID Option:Selected").text();

    data = {
        ID: $("#ID").val(),
        TransNo: $("#TransNo").val(),
        TransDate: $("#TransDate").val(),
        PatientID: $("#PatientID").val(),
        WarehouseID: $("#StoreID").val(),
        DoctorID: $("#DoctorID").val(),
        NetAmount: clean($("#NetAmount").val().replace(/,/g, '')),
        PackingPrice: clean($("#PackingPrice").val()),
        SGSTAmount: clean($("#SGSTAmount").val()),
        CGSTAmount: clean($("#CGSTAmount").val()),
        TotalVATAmount: clean($("#TotalVATAmount").val()),
        IsGST: clean($("#IsGST").val()),
        IsVat: clean($("#IsVat").val()),
        CurrencyID: clean($("#CurrencyID").val()),
        RoundOff: clean($("#RoundOff").val()),
        PaymentModeID: clean($("#PaymentModeID").val()),
        TotalAmountReceived: clean($("#TotalAmountReceived").val()),
        BalanceToBePaid: clean($("#BalanceToBePaid").val()),
        CessAmount: clean($("#CessAmount").val()),
        EmployeeID: clean($("#EmployeeID").val()),
        TypeID: clean($("#TypeID").val()),
        TaxableAmt: clean($("#TaxableAmt").val()),
        GrossAmount: clean($("#GrossAmount").val()),
        BankID: $("#BankID").val(),
        DoctorName: $("#DoctorName").val(),
        CivilID: $("#CivilID").val(),
        CustomerID: clean($("#CustomerID").val()),
        ContactID: clean($("#ContactID").val()),
        MobileNumber: $("#MobileNumber").val(),
        DiscountCategoryID: clean($("#DiscountPercentageList").val()),
        BillDiscount: clean($("#BillDiscount").val()),
        BillDiscountPercent: clean($("#BillDiscountPercent").val()),
        DiscountAmt: clean($("#DiscountAmt").val()),
        PrintWithItemCode: $("#PrintWithItemCode").is(":checked"),
        Remarks: $("#Remarks").val(),
        AmountRecieveds: clean($("#AmountRecieveds").val()),
        ReferenceNo: $("#ReferenceNoo").val(),
        VATPercentageID: clean($("#VATPercentageID").val()),
        VATPercentage: clean($("#VATPercentageID option:selected").text()),
    };
    if (SalesTypeName == "Patient") {
        data.PartyName = $("#PartyName").val();
    }
    else if (SalesTypeName == "Cash Sales") {
        data.PartyName = $("#CashSales").val();
    }
    else if (SalesTypeName == "Employee") {
        data.PartyName = $("#EmployeeName").val();
    }

    data.Items = [];
    $("#counter-sales-items-list tbody tr").each(function () {
        item = {
            ItemID: $(this).find(".ItemID").val(),
            ItemName: $(this).find(".Name").val(),
            PartsNumber: $(this).find(".PartsNumber").val(),
            DeliveryTerm: $(this).find(".DeliveryTerm").val(),
            Model: $(this).find(".Model").val(),
            UnitID: $(this).find(".UnitID").val(),
            FullOrLoose: $(this).find(".FullOrLoose").text(),
            MRP: clean($(this).find(".Rate").val()),
            BasicPrice: clean($(this).find(".BasicPrice").val()),
            Quantity: clean($(this).find(".Qty").val()),
            SecondaryUnit: $(this).find('.SecondaryUnit option:selected').text().trim(),
            SecondaryUnitSize: clean($(this).find('.SecondaryUnit').val()),
            SecondaryRate: clean($(this).find('.SecondaryRate').val()),
            SecondaryQty: clean($(this).find('.SecondaryQty').val()),
            GrossAmount: clean($(this).find(".GrossAmount").val()),
            GSTPercntage: $(this).find(".GST").val(),
            CGSTAmount: clean($(this).find(".CGST").val()),
            SGSTAmount: clean($(this).find(".SGST").val()),
            IGSTAmount: clean($(this).find(".IGST").val()),
            IsVAT: clean($(this).find(".IsVAT").val()),
            IsGST: clean($(this).find(".IsGST").val()),
            CurrencyID: clean($(this).find(".CurrencyID").val()),
            VATPercentage: clean($(this).find(".VAT").val()),
            VATAmount: clean($(this).find(".VATAmount").val()),
            NetAmount: clean($(this).find(".NetAmount").val()),
            BatchTypeID: clean($(this).find(".BatchTypeID").val()),
            BatchID: clean($(this).find(".BatchID").val()),
            WareHouseID: clean($(this).find(".WareHouseID").val()),
            SGSTPercentage: clean($(this).find(".SGSTPercentage").val()),
            CGSTPercentage: clean($(this).find(".CGSTPercentage").val()),
            IGSTPercentage: clean($(this).find(".IGSTPercentage").val()),
            TaxableAmount: clean($(this).find(".TaxableAmount").val()),
            CessAmount: clean($(this).find(".CessAmount").val()),
            CessPercentage: clean($(this).find(".CessPercentage").val()),
            DiscountAmount: clean($(this).find(".DiscountAmount").val()),
            DiscountPercentage: clean($(this).find(".DiscountPercentage").val()),
        }
        data.Items.push(item);
    });
    data.AmountDetails = [];
    $("#sales-invoice-amount-details-list tbody tr").each(function () {
        AmountDetails = {
            Amount: $(this).find(".Amount").val(),
            Particulars: $(this).find(".Particulars").text().trim(),
            Percentage: $(this).find(".Percentage").val(),
        }
        data.AmountDetails.push(AmountDetails);
    });

    return data;
};
CounterSales.on_save = function () {

    var self = CounterSales;
    self.Checkcustomer();
    if (self.alertShown) {
        return;
    }
    var data = self.get_data();
    var location = "/Sales/CounterSales/IndexV4";
    var url = '/Sales/CounterSales/Save';
    IsPrint = true;
    if ($(this).hasClass("btnSaveASDraft")) {
        data.IsDraft = true;
        IsPrint = false;
        url = '/Sales/CounterSales/SaveAsDraft'
        self.error_count = self.validate_draft();
    } else {
       
        if (NetAmount = AmountRecieveds) {
            self.error_count = self.validate_form();
        }
        else {
            app.show_error("Please select Customer Name");
            return;
            if ($("#CashSales").length > 0) {
                self.error_count = self.validate_form();
            }

            
        }
        
        self.error_count = self.validate_form();
        if ($(this).hasClass("btnSaveAndPrint")) {
            IsPrint = true;
        }
        if ($(this).hasClass("btnSaveAndNew")) {
            location = "/Sales/CounterSales/CreateV4";
            $("#save_new").val("new");
        }
    }

    if (self.error_count > 0) {
        return;
    }

    if (!data.IsDraft) {
        app.confirm_cancel("Do you want to save?", function () {
            IsPrint = true;
            self.save(data, url, location);
        }, function () {
        })
    } else {
        IsPrint = false;
        self.save(data, url, location);
    }
};
CounterSales.Checkcustomer = function () {
    var self = CounterSales;
    var NetAmount = $("#NetAmount").val();
    var AmountRecieveds = $("#AmountRecieveds").val();
    if (NetAmount != AmountRecieveds) {
        var cid = $("#CustomerID").val();
        if (cid == 0) {
            app.show_error("Please select Customer Name");
            self.alertShown = true

            setTimeout(function () {
                self.alertShown = false;


            }, 1000);
        }

        else {
            self.alertShown = false;

        }


    }
};
CounterSales.print_close = function () {
    var self = CounterSales;
    var SaveFunction = $("#save_new").val();
    if ($("#page_content_inner").hasClass("sales form-view")) {
        var url = '/Sales/CounterSales/IndexV4';
        if (SaveFunction == 'new') {
            var url = '/Sales/CounterSales/CreateV4';
        }
        setTimeout(function () {
            window.location = url
        }, 1000);
    }
};

CounterSales.tabbed_list = function (type) {
    var self = CounterSales;
    var $list;

    switch (type) {
        case "draft":
            $list = $('#draft-counter-sales');
            break;
        case "saved-counter-sales":
            $list = $('#saved-counter-sales');
            break;
        case "cancelled":
            $list = $('#cancel-counter-sales');
            break;
        default:
            $list = $('#draft-counter-sales');
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
                "url": "/Sales/CounterSales/GetListForCounterSales?type=" + type,
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
                { "data": "TransDate", "className": "TransDate" },
                { "data": "Type", "className": "Type" },
                { "data": "TaxType", "className": "TaxType", },
                { "data": "CurrencyName", "className": "CurrencyName", },
                { "data": "PartyName", "className": "PartyName", },
                {
                    "data": "NetAmount", "searchable": false, "className": "NetAmount",
                    "render": function (data, type, row, meta) {
                        return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
                    }
                },
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.IsDraft);
                $(row).addClass(data.IsCancelled);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var Id = $(this).closest("tr").find("td .ID").val();
                    app.load_content("/Sales/CounterSales/DetailsV4/" + Id);
                });
            },
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
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
