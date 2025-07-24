var fh_items;
var item_list;
var customer_list;

SalesReturn = {
    init: function () {

        var self = SalesReturn;

        customer_list = Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        invoice_list = self.sales_invoice_list();

        item_select_table = $('#invoice-list').SelectTable({
            modal: "#add-invoice",
            initiatingElement: "#InvoiceNo",
            selectFunction: self.get_sales_details,
        });

        item_list = Item.item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName"
        });

        self.bind_events();
        //if ($("#ID").val() > 0) {
        //    if ($('#IsNewInvoice').val() == "True") {
        //        $(".add-new-item").removeClass("uk-hidden");
        //        var id = 0;
        //        $("#CustomerIDDummy").val(id);

        //    } else {
        //        var ID = $("#CustomerID").val();
        //        $("#CustomerIDDummy").val(ID);
        //    }
        //    $("#CustomerIDDummy").trigger("change");

        //    $("#sales-return-items-list tbody tr.included").each(function () {
        //        var row = $(this).closest('tr');
        //        self.Process_Item(row);

        //    });
        //}

    },


    details: function () {
        var self = SalesReturn;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = SalesReturn;
        $.ajax({
            url: '/Reports/Sales/SalesReturnPrintPdf',
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

    sales_return_list: function () {
        var self = SalesReturn;
        $('#tabs-SalesReturn').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "sales-return":
                $list = $('#sales-return-list');
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

            var url = "/Sales/SalesReturn/GetSalesReturnListForDataTable?type=" + type;
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[2, "desc"]],
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>"

                        }
                    },

                    { "data": "ReturnNo", "className": "ReturnNo" },
                    { "data": "ReturnDate", "className": "ReturnDate" },
                    { "data": "CustomerName", "className": "CustomerName" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/SalesReturn/Details/" + Id);

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
        var self = SalesReturn;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $("#CustomerCategoryID").on("change", self.update_customer_list);
        $("#btnOKCustomer").on("click", self.select_customer);
        $("body").on("keyup change", "#sales-return-items-list tbody .ReturnQty", self.on_change_qty);
        $("body").on("keyup change", "#sales-return-items-list tbody .SecondaryReturnQty", self.on_change_secondaryqty);

        $("body").on("keyup change", "#sales-return-items-list tbody .OfferReturnQty", self.validate_return_offer_qty);
        $(".btnSaveASDraft,.btnSave").on("click", self.on_save);
        $("#btnOkInvoiceList").on("click", self.get_sales_details);
        $("body").on('ifChanged', '.include-item', self.include_item);
        $("#CustomerID").on('changed', self.refresh_invoice);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnAddItem").on("click", self.add_item);
        $(".is-new-invoice").on("ifChanged", self.show_add_item);
        $("body").on("change", "#sales-return-items-list tbody .Unit.included", self.get_item_row);
        self.Load_All_DropDown();
    },
    get_item_row: function () {
        var self = SalesReturn;
        var row = $(this).closest('tr');
        self.Process_Item(row);
    },
    Load_All_DropDown: function () {
        $.ajax({
            url: '/Sales/SalesReturn/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    SalesReturnLogicCodeList = response.data;
                }
            }
        });
    },


    show_add_item: function () {
        var self = SalesReturn;
        if ($('#sales-return-items-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                if ($(this).prop('checked') == true) {
                    $(".add-new-item").removeClass("uk-hidden");
                }
                else {
                    $(".add-new-item").addClass("uk-hidden");
                }
                $("#InvoiceNo").val('');
                $('#sales-return-items-list tbody').html('');
                $("#item-count").val(0);
            })
        }
        else {
            if ($(this).prop('checked') == true) {
                $(".add-new-item").removeClass("uk-hidden");
            }
            else {
                $(".add-new-item").addClass("uk-hidden");
            }

        }
        if ($(this).prop('checked') == true) {
            var id = 0;
            $("#CustomerIDDummy").val(id);
        } else {
            var ID = $("#CustomerID").val();
            $("#CustomerIDDummy").val(ID);
        }
        $("#CustomerIDDummy").trigger("change");

    },

    add_item: function () {
        var self = SalesReturn;
        self.error_count = 0;
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
        item.DiscountPercentage = (item.Code == 'FG00723' || item.Code == 'FG00815') ? 20 : clean($("#DiscountPercent").val());
        item.Qty = clean($("#Qty").val());
        item.InvoiceNo = $("#InvoiceNo").val();
        item.Batch = $("#Batch").val();
        item.OfferQty = clean($("#OfferQty").val());
        item.BatchTypeID = $("#BatchTypeID").val();
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SalesUnitID = clean($("#SalesUnitID").val());
        item.MRP = clean($("#MRP").val());
        item.IsNewInvoice = $(".is-new-invoice").prop('checked') == true ? 1 : 0;

        self.add_item_to_grid(item);

        self.clear_item();
        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);

    },

    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#Qty").val('');
        $("#OfferQty").val('');
        $("#Batch").val('');
        $("#MRP").val('');
        $("#BasicPrice").val();
        $("#DiscountPercent").val('');
    },

    get_item_properties: function (item) {
        var self = SalesReturn;
        item.BasicPrice = (item.MRP * 100 / (100 + item.IGSTPercentage)).roundTo(2);
        item.GrossAmount = (item.BasicPrice * (item.Qty)).roundTo(2);
        //    item.GrossAmount += item.BasicPrice * item.ReturnOfferQty;
        item.AdditionalDiscount = 0;// (item.BasicPrice * item.OfferQty).roundTo(2);;
        item.DiscountAmount = ((item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100).roundTo(2);
        item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount;

        item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);

        item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100).roundTo(2);

        item.IGSTAmount = 0;
        item.CGSTAmount = 0;
        item.SGSTAmount = 0;

        if ($("#StateID").val() == $("#LocationStateID").val()) {
            item.CGSTAmount = (item.TaxableAmount * item.CGSTPercentage / 100).roundTo(2);
            item.SGSTAmount = (item.TaxableAmount * item.SGSTPercentage / 100).roundTo(2);
        } else {
            item.IGSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);
        }

        item.NetAmount = item.TaxableAmount + item.IGSTAmount + item.CGSTAmount + item.SGSTAmount + item.VATAmount;
        return item;
    },

    item_offer_details: [],

    add_item_to_grid: function (item) {
        var self = SalesReturn;
        var index, GSTAmount, tr;
        item = self.get_item_properties(item);


        index = $("#sales-return-items-list tbody tr").length + 1;
        var $sales_invoice_items_list = $('#sales-return-items-list tbody');
        var tr = '';
        LogicCode = '<select class="md-input label-fixed>' + SalesReturn.Build_Select_LogicCode(SalesReturnLogicCodeList, item.LogicCodeID) + '</select>'
        GSTAmount = item.CGSTAmount + item.SGSTAmount + item.IGSTAmount;
        tr += " <tr class='included'>"
            + "<td class='uk-text-center'>" + index + "</td>"
            + "<td class='checked uk-text-center' data-md-icheck> "
            + "<input type='checkbox' class='include-item' checked/>"
            + '<input type="hidden" class="UnitID included" value="' + item.UnitID + '"  />'
            + '<input type="hidden" class="invoiceID included" value="' + 0 + '" />'
            + '<input type="hidden" class="InvoiceTransID included" value="' + 0 + '" />'
            + '<input type="hidden" class="InvoiceTransNo included" value="' + item.InvoiceNo + '" />'
            + '<input type="hidden" class="BatchID included" value="' + item.BatchID + '" />'
            + '<input type="hidden" class="Stock included" value="' + item.Stock + '" />'
            + '<input type="hidden" class="Batch included" value="' + item.Batch + '" />'
            + '<input type="hidden" class="BatchTypeID included" value="' + item.BatchTypeID + '" />'
            + '<input type="hidden" class="CGSTPercent mask-currency included" value="' + item.CGSTPercentage + '" />'
            + '<input type="hidden" class="IGSTPercent mask-currency included" value="' + item.IGSTPercentage + '"  />'
            + '<input type="hidden" class="ItemID included" value="' + item.ID + '"  />'
            + '<input type="hidden" class="SGSTPercent mask-currency included" value="' + item.SGSTPercentage + '"  />'
            + '<input type="hidden" class="CGST mask-sales-currency included" value="' + item.CGSTAmount + '" />'
            + '<input type="hidden" class="SGST mask-sales-currency included" value="' + item.SGSTAmount + '" />'
            + '<input type="hidden" class="IGST mask-sales-currency included" value="' + item.IGSTAmount + '" />'
            + '<input type="hidden" class="Unit included" value="' + item.UnitID + '"  />'
            + '<input type="hidden" class="IsNewInvoice included" value="' + item.IsNewInvoice + '"  />'
            + "</td>"
            + '<td>' + item.Code
            + '</td>'
            + ' <td >' + item.Name + '</td>'
            + ' <td >' + item.Batch + '</td>'
            + ' <td>' + item.Unit + '</td>'
            + ' <td ><input type="text" class="md-input MRP mask-sales-currency included" value="' + item.MRP + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input BasicPrice mask-sales-currency included" value="' + item.BasicPrice + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input SaleQty mask-qty included" value="' + item.Qty + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input OfferQty mask-qty included" value="' + item.OfferQty + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input ReturnQty mask-qty included" value="' + item.Qty + '"  /></td>'
            + ' <td ><input type="text" class="md-input OfferReturnQty mask-qty included" value="' + item.OfferQty + '"  /></td>'
            + ' <td ><input type="text" class="md-input GrossAmount mask-sales-currency included" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input DiscountPercentage mask-currency included" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input DiscountAmount mask-sales-currency included" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input GST mask-sales-currency included" value="' + GSTAmount + '" readonly="readonly" /></td>'
            + ' <td ><input type="text" class="md-input NetAmount mask-sales-currency included" value="' + item.NetAmount + '" readonly="readonly" /></td>'
            + ' <td class="LogicCodeID">' + LogicCode + '</td>'
            + '</tr>';
        var $tr = $(tr);
        app.format($tr);
        $sales_invoice_items_list.append($tr);
        self.calculate_grid_total();

    },

    get_items: function (release) {
        var self = SalesReturn;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
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

    set_item: function (event, item) {
        var self = SalesReturn;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#Unit").val(item.unit);
        $("#Code").val(item.code);
        $("#UnitID").val(item.unitId);
        $("#Stock").val(item.stock);
        $("#CGSTPercentage").val(item.cgstpercentage);
        $("#IGSTPercentage").val(item.igstpercentage);
        $("#SGSTPercentage").val(item.sgstpercentage);
        $("#Rate").val(item.rate);
        $("#PrimaryUnit").val(item.unit);
        $("#PrimaryUnitID").val(item.unitId);
        $("#SalesUnit").val(item.saleUnit);
        $("#SalesUnitID").val(item.saleUnitId);
        self.get_units();
        $("#Qty").focus();
    },

    refresh_invoice: function () {
        list_table.fnDraw();
    },

    sales_invoice_list: function () {
        var $list = $('#invoice-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = '/Sales/SalesInvoice/GetInvoiceListForSalesReturn';

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
                            { Key: "CustomerID", Value: $('#CustomerIDDummy').val() },
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
                            return "<input type='radio' class='CheckItem' name='ID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "InvoiceDate", "className": "InvoiceDate" },
                    {
                        "data": "NetAmount",
                        "className": "NetAmount",
                        "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }

                    },
                ],

                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            //$('body').on("change", '#CustomerID', function () {
            //    list_table.fnDraw();
            //});
            $('body').on("change", '#CustomerIDDummy', function () {
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

    validate_return_offer_qty: function () {
        var self = SalesReturn;
        var row = $(this).closest('tr');
        var OfferQty = clean($(row).find('.OfferQty').val());
        var ReturnOfferQty = clean($(row).find('.OfferReturnQty').val());
        var IsNewInvoice = clean($(row).find('.IsNewInvoice').val());

        if ((ReturnOfferQty > OfferQty) && (IsNewInvoice == 0)) {
            app.show_error('Invalid offer return quantity');
            app.add_error_class($(row).find('.OfferReturnQty'));
        }
        else {
            var item = {};
            var row = $(this).closest('tr');
            //item.ReturnOfferQty = clean($(row).find('.OfferReturnQty').val());;
            item.Qty = clean($(row).find('.ReturnQty').val());
            item.BasicPrice = clean($(row).find('.BasicPrice').val());
            item.DiscountPercentage = clean($(row).find('.DiscountPercentage').val());
            item.CGSTPercentage = clean($(row).find('.CGSTPercent').val());
            item.SGSTPercentage = clean($(row).find('.SGSTPercent').val());
            item.IGSTPercentage = clean($(row).find('.IGSTPercent').val());
            item.OfferQty = clean($(row).find('.OfferReturnQty').val());
            item.MRP = clean($(row).find('.MRP').val());
            item = self.get_item_properties(item);
            $(row).find('.DiscountAmount').val(item.DiscountAmount);
            $(row).find('.GrossAmount').val(item.GrossAmount);
            $(row).find('.CGST').val(item.CGSTAmount);
            $(row).find('.IGST').val(item.IGSTAmount);
            $(row).find('.SGST').val(item.SGSTAmount);
            $(row).find('.NetAmount').val(item.NetAmount);
            self.calculate_grid_total();
        }

    },

    Process_Item: function (row) {
        var self = SalesReturn;
        var SalesUnitID = clean($(row).find(".SalesUnitID").val());
        var LoosePrice = clean($(row).find(".LoosePrice").val());
        var UnitID = clean($(row).find(".Unit option:selected").val());
        var FullPrice = clean($(row).find(".FullPrice").val());
        var CounterSalesTransUnitID = clean($(row).find(".CounterSalesTransUnitID").val());
        var Quantity = clean($(row).find(".Quantity").val());
        var ConvertedQuantity = clean($(row).find(".ConvertedQuantity").val());
        var ConvertedOfferQuantity = clean($(row).find(".ConvertedOfferQuantity").val());
        var OfferQuantity = clean($(row).find(".OfferQuantity").val());
        if (SalesUnitID == UnitID) {
            $(row).find(".MRP").val(FullPrice);
        }
        else {
            $(row).find(".MRP").val(LoosePrice);
        }
        if (CounterSalesTransUnitID == UnitID) {
            $(row).find(".SaleQty").val(Quantity);
            $(row).find(".OfferQty").val(OfferQuantity);

        }
        else {
            $(row).find(".SaleQty").val(ConvertedQuantity);
            $(row).find(".OfferQty").val(ConvertedOfferQuantity);
        }
        self.update_item(row);
    },

    on_change_secondaryqty: function () {
        var self = SalesReturn;
        var row = $(this).closest('tr');
        var SecondaryUnitSize = clean($(row).find('.SecondaryUnitSize').val());
        var SecondaryReturnQty = clean($(row).find('.SecondaryReturnQty').val());
        var ReturnQty = SecondaryUnitSize * SecondaryReturnQty;
        $(row).find('.ReturnQty').val(ReturnQty);
        $(row).find('.ReturnQty').trigger('change');
    },
    on_change_qty: function () {
        var self = SalesReturn;
        var row = $(this).closest('tr');
        self.update_item(row)
    },

    update_item: function (row) {
        var self = SalesReturn;
        // var row = $(this).closest('tr');
        var ReturnQty = clean($(row).find('.ReturnQty').val());


        var SaledQty = clean($(row).find('.SaleQty').val());
        var OfferQty = clean($(row).find('.OfferReturnQty').val());
        var IsNewInvoice = clean($(row).find('.IsNewInvoice').val());

        if ((ReturnQty > SaledQty) && (IsNewInvoice == 0)) {
            app.show_error('Invalid return quantity');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else {
            var item = {};

            var GST = 0;
            item.Qty = ReturnQty;
            // item.ReturnOfferQty = clean($(row).find('.ReturnOfferQty').val());;
            item.BasicPrice = clean($(row).find('.BasicPrice').val());
            item.DiscountPercentage = clean($(row).find('.DiscountPercentage').val());
            item.CGSTPercentage = clean($(row).find('.CGSTPercent').val());
            item.SGSTPercentage = clean($(row).find('.SGSTPercent').val());
            item.IGSTPercentage = clean($(row).find('.IGSTPercent').val());
            item.OfferQty = clean($(row).find('.OfferReturnQty').val());
            item.MRP = clean($(row).find('.MRP').val());
            item.VATPercentage = clean($(row).find('.VATPercentage').val());
            item = self.get_item_properties(item);
            GST = item.CGSTAmount + item.SGSTAmount + item.IGSTAmount;
            $(row).find('.DiscountAmount ').val(item.DiscountAmount);
            $(row).find('.GrossAmount ').val(item.GrossAmount);
            $(row).find('.CGST').val(item.CGSTAmount);
            $(row).find('.IGST').val(item.IGSTAmount);
            $(row).find('.SGST').val(item.SGSTAmount);
            $(row).find('.GST').val(GST);
            $(row).find('.VATAmount').val(item.VATAmount);
            $(row).find('.NetAmount').val(item.NetAmount);
            self.calculate_grid_total();
        }
    },

    get_sales_details: function () {
        var self = SalesReturn;
        if ($("#sales-return-items-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#sales-return-items-list tbody').empty();
                self.count_items();
                self.select_sales_invoice();
            })
        }
        else {
            self.select_sales_invoice();
        }
    },

    Build_Select: function (UnitID, Unit, SalesUnit, SalesUnitID, CounterSalesTransUnitID) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';

        if (CounterSalesTransUnitID == SalesUnitID) {
            $option += "<option selected value='" + SalesUnitID + "'>" + SalesUnit + "</option>";
        }
        else {
            $option += "<option  value='" + SalesUnitID + "'>" + SalesUnit + "</option>";

        }
        $select.append($option);
        if (CounterSalesTransUnitID == UnitID) {
            $option += "<option selected value='" + UnitID + "'>" + Unit + "</option>";

        }
        else {
            $option += "<option value='" + UnitID + "'>" + Unit + "</option>";
        }

        $select.append($option);


        return $select.html();

    },

    Build_Select_LogicCode: function (options, selected) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';
        $option = '<option value="' + 0 + '">Select</option>';
        $select.append($option);
        for (var i = 0; i < options.length; i++) {
            if (options[i].ID == selected) {
                $option = '<option selected value="' + options[i].LogicCodeID + '">' + options[i].LogicName + '</option>';
            } else {
                $option = '<option value="' + options[i].LogicCodeID + '">' + options[i].LogicName + '</option>';
            }

            $select.append($option);
        }

        return $select.html();

    },

    select_sales_invoice: function () {
        var self = SalesReturn;
        var radio = $('#invoice-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var InvoiceNo = $(row).find(".TransNo").text().trim();
        $("#SalesInvoiceID").val(ID);
        $("#InvoiceNo").val(InvoiceNo)
        var sales_invoice_ids = $("#invoice-list .CheckItem:checked").map(function () {
            return $(this).val();
        }).get();
        if (sales_invoice_ids == null) {
            sales_invoice_ids = $("#InvoiceID").val();
        }
        var InvoiceNos = [];

        if (sales_invoice_ids.length == 0) {
            app.show_error('Please select invoice');
            return;
        }
        $("#NetAmount").val(0);
        $.ajax({
            url: '/Sales/SalesInvoice/GetSalesInvoices/',
            dataType: "json",
            data: {
                SalesInvoiceIDS: sales_invoice_ids,
                PriceListID: $("#PriceListID").val()
            },
            type: "POST",
            success: function (sales_invoice_items) {

                var $sales_invoice_items_list = $('#sales-return-items-list tbody');
                $sales_invoice_items_list.html('');
                var tr = '';
                $.each(sales_invoice_items, function (i, item) {
                    if (InvoiceNos.indexOf(item.InvoiceNo) == -1) {
                        InvoiceNos.push(item.InvoiceNo);
                    }
                    var quantity = item.Qty - item.OfferQty;
                    Unit = '<select class="md-input label-fixed Unit >' + SalesReturn.Build_Select(item.UnitID, item.UnitName, item.SalesUnitName, item.SalesUnitID, item.CounterSalesTransUnitID) + '</select>'
                    LogicCode = '<select class="md-input label-fixed>' + SalesReturn.Build_Select_LogicCode(SalesReturnLogicCodeList, item.LogicCodeID) + '</select>'
                    tr += " <tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='checked uk-text-center' data-md-icheck> "
                        + "<input type='checkbox' class='include-item'/>"

                        + '     <input type="hidden" class="invoiceID" value="' + item.InvoiceID + '" />'
                        + '     <input type="hidden" class="InvoiceTransID" value="' + item.InvoiceTransID + '" />'
                        + '     <input type="hidden" class="InvoiceTransNo" value="' + item.InvoiceNo + '" />'
                        + '     <input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                        + '     <input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                        + '     <input type="hidden" class="Stock" value="' + item.Stock + '" />'
                        + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                        + '     <input type="hidden" class="Batch" value="' + item.BatchName + '" />'
                        + '     <input type="hidden" class="CGSTPercent" value="' + item.CGSTPercent + '" readonly="readonly" />'
                        + '     <input type="hidden" class="IGSTPercent" value="' + item.IGSTPercent + '" readonly="readonly" />'
                        + '     <input type="hidden" class="SGSTPercent" value="' + item.SGSTPercent + '" readonly="readonly" />'
                        + '     <input type="hidden" class="SalesUnitID " value="' + item.SalesUnitID + '" />'
                        + '     <input type="hidden" class="UnitID " value="' + item.UnitID + '" />'
                        + '     <input type="hidden" class="FullPrice " value="' + item.FullPrice + '" />'
                        + '     <input type="hidden" class="LoosePrice " value="' + item.LoosePrice + '" />'
                        + '     <input type="hidden" class="CounterSalesTransUnitID " value="' + item.CounterSalesTransUnitID + '" />'
                        + '     <input type="hidden" class="ConvertedQuantity " value="' + item.ConvertedQuantity + '" />'
                        + '     <input type="hidden" class="ConvertedOfferQuantity " value="' + item.ConvertedOfferQuantity + '" />'
                        + '     <input type="hidden" class="Quantity " value="' + quantity + '" />'
                        + '     <input type="hidden" class="OfferQuantity " value="' + item.OfferQty + '" />'
                        + '     <input type="hidden" class="CGST mask-sales-currency included" value="' + 0 + '" />'
                        + '     <input type="hidden" class="SGST mask-sales-currency included" value="' + 0 + '" />'
                        + '     <input type="hidden" class="IGST mask-sales-currency included" value="' + 0 + '" />'
                        + '     <input type="hidden" class="IsNewInvoice mask-sales-currency included" value="' + 0 + '" />'
                        + '     <input type="hidden" class="SecondaryUnitSize" value="' + item.SecondaryUnitSize + '" />'
                        + "</td>"
                        + ' <td>' + item.ItemCode
                        + '</td>'
                        + ' <td>' + item.ItemName + '</td>'
                        + ' <td>' + item.BatchName + '</td>'
                        + ' <td  class="uk-hidden">' + Unit + '</td>'
                        + ' <td class="SecondaryUnit">' + item.SecondaryUnit + '</td>'
                        + ' <td class="uk-hidden"><input type="text" class="md-input MRP mask-sales-currency" value="' + item.MRP + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input SecondaryMRP mask-sales-currency" value="' + item.SecondaryMRP + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                        + ' <td class="uk-hidden"><input type="text" class="md-input SaleQty mask-qty" value="' + quantity + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input SecondaryQty mask-qty" value="' + item.SecondaryQty + '" readonly="readonly" /></td>'
                        + ' <td class="uk-hidden"><input type="text" class="md-input OfferQty mask-qty" value="' + item.OfferQty + '" readonly="readonly" /></td>'
                        + ' <td class="uk-hidden"><input type="text" class="md-input ReturnQty mask-qty" value="' + 0 + '" disabled /></td>'
                        + ' <td ><input type="text" class="md-input SecondaryReturnQty mask-qty" value="' + 0 + '" disabled /></td>'
                        + ' <td class="uk-hidden"><input type="text" class="md-input OfferReturnQty mask-qty" value="' + 0 + '" disabled /></td>'
                        + ' <td ><input type="text" class="md-input GrossAmount mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input DiscountPercentage mask-currency" value="' + item.DiscPercentage + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input DiscountAmount mask-sales-currency" value="' + item.CashDiscount + '" readonly="readonly" /></td>'
                        /*+ ' <td ><input type="text" class="md-input GST mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'*/
                        + ' <td ><input type="text" class="md-input VATPercentage mask-currency" value="' + item.VATPercentage + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input VATAmount mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input NetAmount mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + ' <td class="LogicCodeID">' + LogicCode + '</td>'
                        + '</tr>';

                });
                //$("#InvoiceNo").val(InvoiceNos.join(','));
                var $tr = $(tr);
                app.format($tr);
                $sales_invoice_items_list.append($tr);

                self.count_items();
            },
        });
    },

    count_items: function () {
        var count = $('#sales-return-items-list tbody tr.included').length;
        $('#item-count').val(count);
    },

    include_item: function () {
        var self = SalesReturn;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }
        self.calculate_grid_total();
        self.count_items();
    },

    get_invoice: function () {
        var self = SalesReturn;

        self.error_count = 0;
        self.error_count = self.validate_select_invoice();
        if (self.error_count > 0) {
            return;
        }
        var CustomerID = $("#CustomerID").val();

        $.ajax({
            url: '/Sales/SalesInvoice/GetSalesInvoiceList/',
            dataType: "json",
            type: "GET",
            data: {
                CustomerID: CustomerID,
            },
            success: function (response) {
                self.invoice_list_table.fnDestroy();
                var $invoice_list = $('#itempopup-list tbody');
                $invoice_list.html('');
                var tr = '';
                $.each(response, function (i, invoice) {
                    tr += "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center ' data-md-icheck><input type='checkbox'  class='CheckItem' value=" + invoice.ID + " /></td>"
                        + "<td>" + invoice.InvoiceNo + "</td>"
                        + "<td>" + invoice.InvoiceDate + "</td>"
                        + "<td>" + invoice.NetAmount + "</td>"
                        + "</tr>";

                });
                var $tr = $(tr);
                app.format($tr);
                $invoice_list.append($tr);
                self.invoice_list();
            },
        });
    },

    select_item: function () {
        var self = SalesReturn;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            $("#CustomerName").focus();
            return;
        }
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var UnitID = $(row).find(".UnitID").val();
        var SalesUnit = $(row).find(".SalesUnit").val();
        var SalesUnitID = $(row).find(".SalesUnitID").val();
        var Stock = $(row).find(".Stock").val();
        var CGSTPercentage = $(row).find(".CGSTPercentage").val();
        var IGSTPercentage = $(row).find(".IGSTPercentage").val();
        var SGSTPercentage = $(row).find(".SGSTPercentage").val();
        var Rate = $(row).find(".Rate").val();
        var LooseRate = $(row).find(".LooseRate").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#PrimaryUnit").val(Unit);
        $("#PrimaryUnitID").val(UnitID);
        $("#SalesUnit").val(SalesUnit);
        $("#SalesUnitID").val(SalesUnitID);
        $("#Stock").val(Stock);
        $("#CGSTPercentage").val(CGSTPercentage);
        $("#IGSTPercentage").val(IGSTPercentage);
        $("#SGSTPercentage").val(SGSTPercentage);
        $("#LooseRate").val(LooseRate);
        $("#Rate").val(Rate);
        $("#Qty").focus();
        self.get_units();
        UIkit.modal($('#select-item')).hide();

    },

    get_units: function () {
        var self = SalesReturn;


        $("#UnitID").html("");

        var html;//= "<option value >Select</option>";          
        html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);

    },

    set_customer: function (event, item) {
        var self = SalesReturn;
        if ($('#sales-return-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                $('#sales-return-items-list tbody').html('');
                $("#CustomerName").val(item.value);
                $("#CustomerID").val(item.id);
                $("#StateID").val(item.stateId);
                $("#CustomerCategoryID").val(item.customerCategoryId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#InvoiceNo").val('');
                self.get_batch_type();
                $("#CustomerID").val(item.id).trigger("change");
            });
        }
        else {
            $("#CustomerName").val(item.value);
            $("#CustomerID").val(item.id);
            $("#StateID").val(item.stateId);
            $("#IsGSTRegistered").val(item.isGstRegistered);
            $("#CustomerID").val(item.id).trigger("change");
            $("#CustomerCategoryID").val(item.customerCategoryId);
            self.get_batch_type();
        }
        if ($(".is-new-invoice").prop("checked") == "true") {
            var id = 0;
            $("#CustomerIDDummy").val(id);

        } else {
            $("#CustomerIDDummy").val(item.id);
        }
        $("#CustomerIDDummy").trigger("change");
    },

    get_customers: function (release) {
        var self = SalesReturn;

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

    select_customer: function () {
        var self = SalesReturn;
        var radio = $('#customer-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var CustomerCategoryID = $(row).find(".CustomerCategoryID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        $("#CurrencyCode").val(CurrencyCode);
        if ($('#sales-return-items-list tbody tr').length > 0) {

            app.confirm("Items will be removed", function () {
                $('#sales-return-items-list tbody').html('');
                $("#CustomerName").val(Name);
                $("#CustomerID").val(ID);
                $("#StateID").val(StateID);
                $("#PriceListID").val(PriceListID);
                $("#InvoiceNo").val('');
                $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
                $("#CustomerID").val(ID).trigger("change");
                UIkit.modal($('#select-customer')).hide();
                self.get_batch_type();
            });
        }
        else {
            $("#CustomerName").val(Name);
            $("#CustomerID").val(ID);
            $("#StateID").val(StateID);
            $("#PriceListID").val(PriceListID);
            $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
            $("#CustomerID").val(ID).trigger("change");
            $("#CustomerCategoryID").val(CustomerCategoryID);
            UIkit.modal($('#select-customer')).hide();
            self.get_batch_type();
        }
        if ($(".is-new-invoice").prop("checked") == "true") {
            var id = 0;
            $("#CustomerIDDummy").val(id);

        } else {
            $("#CustomerIDDummy").val(ID);
        }
        $("#CustomerIDDummy").trigger("change");
    },

    update_customer_list: function () {
        customer_list.fnDraw();
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
                }
            }
        });
    },

    calculate_grid_total: function () {
        self = SalesReturn;
        var NetAmount = 0;
        var DiscountAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var GrossAmount = 0;

        var RoundOff = 0;
        $("#sales-return-items-list tbody tr.included").each(function () {
            NetAmount += clean($(this).find('.NetAmount.included').val());
            GrossAmount += clean($(this).find('.GrossAmount.included').val());
            DiscountAmount += clean($(this).find('.DiscountAmount.included').val());
            SGSTAmount += clean($(this).find('.SGST.included').val());
            CGSTAmount += clean($(this).find('.CGST.included').val());
            IGSTAmount += clean($(this).find('.IGST.included').val());
        });
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = (NetAmount - temp).roundTo(2);;
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
        $("#GrossAmount").val(GrossAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);

        self.count_items();
    },

    save: function (data, url, location) {

        var self = SalesReturn;

        $(".btnSaveASDraft, .btnSave").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Sales return saved successfully");

                    setTimeout(function () {
                        window.location = location;
                    }, 1000);

                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSave").css({ 'display': 'block' });

                }
            }
        });
    },


    on_save: function () {

        var self = SalesReturn;
        var data = self.get_data();
        var location = "/Sales/SalesReturn/Index";
        var url = '/Sales/SalesReturn/Save';

        if ($(this).hasClass("btnSaveASDraft")) {

            data = self.get_data();
            data.IsDraft = true;
            url = '/Sales/SalesReturn/SaveAsDraft';
            self.error_count = self.validate_draft_form();
        } else {
            self.error_count = self.validate_form();
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(data, url, location);
            }, function () {
            })
        } else {
            self.save(data, url, location);
        }
    },


    get_data: function () {
        var item = {};
        var data = {
            ID: $("#ID").val(),
            SRNo: $("#SRNo").val(),
            SRDate: $("#SRDate").val(),
            CustomerCategoryID: $("#CustomerCategoryID").val(),
            CustomerID: $("#CustomerID").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            DiscountAmount: clean($("#DiscountAmount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            UnitID: clean($("#UnitID").val()),
            SalesInvoiceID: clean($("#SalesInvoiceID").val()),
            InvoiceNo: $("#InvoiceNo").val(),
            IsNewInvoice: $('.is-new-invoice').prop("checked") == true ? true : false
        };
        data.Items = [];
        $("#sales-return-items-list tbody tr.included").each(function () {
            item = {
                LogicCodeID: $(this).find(".LogicCodeID option:selected").val(),
                ItemID: $(this).find(".ItemID.included").val(),
                UnitID: clean($(this).find(".Unit.included").val()),
                MRP: clean($(this).find(".MRP.included").val()),
                BasicPrice: clean($(this).find(".BasicPrice.included").val()),
                Qty: clean($(this).find(".ReturnQty.included").val()),
                SecondaryUnit: $(this).find(".SecondaryUnit").text().trim(),
                SecondaryUnitSize: clean($(this).find(".SecondaryUnitSize").val()),
                SecondaryMRP: clean($(this).find(".SecondaryMRP").val()),
                SecondaryQty: clean($(this).find(".SecondaryReturnQty").val()),
                OfferQty: clean($(this).find(".OfferQty.included").val()),
                GrossAmount: clean($(this).find(".GrossAmount.included").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount.included").val()),
                DiscountPercentage: clean($(this).find(".DiscountPercentage.included").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercent.included").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercent.included").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercent.included").val()),
                IGST: clean($(this).find(".IGST.included").val()),
                SGST: clean($(this).find(".SGST.included").val()),
                CGST: clean($(this).find(".CGST.included").val()),
                NetAmount: clean($(this).find(".NetAmount.included").val()),
                SaleQty: clean($(this).find(".SaleQty").val()),
                ID: $(this).find(".InvoiceTransID.included").val(),
                TransNo: $(this).find(".InvoiceTransNo.included").val(),
                InvoiceID: $(this).find(".invoiceID.included").val(),
                SalesInvoiceTransID: clean($(this).find(".InvoiceTransID.included").val()),
                OfferReturnQty: clean($(this).find(".OfferReturnQty.included").val()),
                BatchID: $(this).find(".BatchID.included").val(),
                BatchTypeID: $(this).find(".BatchTypeID.included").val(),
                Batch: $(this).find(".Batch.included").val(),
                SalesInvoiceID: $(this).find(".invoiceID.included").val(),
                VATPercentage: clean($(this).find(".VATPercentage.included").val()),
                VATAmount: clean($(this).find(".VATAmount.included").val()),
            }
            data.Items.push(item);
        });
        return data;
    },

    get_draft_data: function () {
        var item = {};
        var data = {
            ID: $("#ID").val(),
            SRNo: $("#SRNo").val(),
            SRDate: $("#SRDate").val(),
            CustomerCategoryID: $("#CustomerCategoryID").val(),
            CustomerID: $("#CustomerID").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            DiscountAmount: clean($("#DiscountAmount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
        };
        data.Items = [];
        $("#sales-return-items-list tbody tr").each(function () {
            item = {
                LogicCodeID: $(this).find(".LogicCodeID option:selected").val(),
                ItemID: $(this).find(".ItemID").val(),
                UnitID: $(this).find(".UnitID").val(),
                MRP: clean($(this).find(".MRP").val()),
                BasicPrice: clean($(this).find(".BasicPrice").val()),
                Qty: clean($(this).find(".ReturnQty").val()),
                OfferQty: clean($(this).find(".OfferQty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount").val()),
                DiscountPercentage: clean($(this).find(".DiscountPercentage").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercent").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercent").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercent").val()),
                IGST: clean($(this).find(".IGST").val()),
                SGST: clean($(this).find(".SGST").val()),
                CGST: clean($(this).find(".CGST").val()),
                NetAmount: clean($(this).find(".NetAmount").val()),
                SaleQty: clean($(this).find(".SaleQty").val()),
                ID: $(this).find(".InvoiceTransID").val(),
                TransNo: $(this).find(".InvoiceTransNo").val(),
                InvoiceID: $(this).find(".invoiceID.included").val(),
                Batch: $(this).find(".Batch.included").val(),
                BatchID: $(this).find(".BatchID.included").val(),
                BatchTypeID: $(this).find(".BatchTypeID.included").val(),
                SalesInvoiceID: $(this).find(".invoiceID.included").val()
            }
            data.Items.push(item);
        });
        return data;
    },

    validate_customer: function () {
        var self = SalesReturn;
        if (self.rules.on_customer.length) {
            return form.validate(self.rules.on_customer);
        }
        return 0;
    },

    validate_form: function () {
        var self = SalesReturn;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft_form: function () {
        var self = SalesReturn;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_item: function () {
        var self = SalesReturn;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    rules: {
        on_add_item: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                    {
                        type: function (element) {
                            var error = false;
                            var unitid = $("#UnitID option:selected").val();
                            $("#sales-order-items-list tbody tr").each(function () {
                                if (($(this).find(".ItemID").val() == $(element).val()) && (($(this).find(".UnitID").val() == unitid))) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already exists"
                    },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid sale quantity" },
                    { type: form.non_zero, message: "Please enter a valid sale quantity" },
                    { type: form.positive, message: "Please enter a valid sale quantity" },
                ]

            },
            {
                elements: "#DiscountPercent",
                rules: [
                    { type: form.positive, message: "Please enter a valid discount percentage" },
                    {
                        type: function (element) {
                            var DiscountPercent = clean($('#DiscountPercent').val());
                            return DiscountPercent <= 100
                        }, message: "Invalid offer discount percentage"
                    },
                ]

            },
            {
                elements: "#OfferQty",
                rules: [
                    { type: form.positive, message: "Please enter a valid offer quantity" },
                ]

            },
            {
                elements: "#Batch",
                rules: [

                    { type: form.required, message: "Please enter a valid batch" },
                ]

            },
            {
                elements: "#MRP",
                rules: [
                    { type: form.required, message: "Please enter a valid MRP" },
                    { type: form.non_zero, message: "Please enter a valid MRP" },
                    { type: form.positive, message: "Please enter a valid MRP" },
                ]

            },
            {
                elements: "#BasicPrice",
                rules: [
                    { type: form.required, message: "Please enter a valid basic price" },
                    { type: form.non_zero, message: "Please enter a valid basic price" },
                    { type: form.positive, message: "Please enter a valid basic price" },
                ]

            },
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Please enter a valid invoice number" },
                ]

            },

        ],
        on_customer: [

            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose customer" },
                    {
                        type: function (element) {
                            return ($(element).val() > 0)

                        }, message: 'Please select customer'
                    },
                ]
            },
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
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },

            {
                elements: ".ReturnQty.included",
                rules: [
                    { type: form.required, message: "Invalid return Quantity" },
                    { type: form.non_zero, message: "Invalid return Quantity" },
                    { type: form.positive, message: "Invalid return Quantity" },
                    {
                        type: function (element) {
                            var error = 1;
                            var row = $(element).closest('tr');
                            var ReturnQty = clean($(row).find('.ReturnQty').val());
                            var SaleQty = clean($(row).find('.SaleQty').val());
                            var OfferQty = 0;
                            var IsNewInvoice = clean($(row).find('.IsNewInvoice').val());
                            if (IsNewInvoice == 0) {
                                error = (ReturnQty <= SaleQty) ? 1 : 0;
                            }
                            return error
                        }, message: "Invalid return quantity"
                    },
                ]
            },
            {
                elements: ".OfferReturnQty.included",
                rules: [
                    { type: form.positive, message: "Invalid offer return Quantity" },
                    {
                        type: function (element) {
                            var error = 1;
                            var row = $(element).closest('tr');
                            var OfferReturnQty = clean($(row).find('.OfferReturnQty').val());
                            var OfferQty = clean($(row).find('.OfferQty').val());
                            var IsNewInvoice = clean($(row).find('.IsNewInvoice').val());
                            var IsNewInvoice = clean($(row).find('.IsNewInvoice').val());
                            if (IsNewInvoice == 0) {
                                error = (OfferReturnQty <= OfferQty) ? 1 : 0;
                            }
                            return error
                        }, message: "Invalid offer return quantity"
                    },
                ]
            },
            {
                elements: ".included .LogicCodeID",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var id = $(row).find('.LogicCodeID option:selected').val();
                            return id > 0
                        }, message: "Please select logic code"
                    },
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
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Please enter a valid invoice number" },
                ]

            },
        ],
        on_draft: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
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
                    { type: form.required, message: "Please enter a valid invoice number" },
                ]

            },
        ],
    }
}
