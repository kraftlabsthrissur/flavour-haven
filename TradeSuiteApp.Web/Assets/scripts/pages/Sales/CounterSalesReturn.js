
var fh_items;
var item_list;
var customer_list;

CounterSalesReturn = {
    init: function () {
        var self = CounterSalesReturn;
        //customer_list = InternationalPatient.Patientlist();
        //$('#patient-list').SelectTable({
        //    selectFunction: self.select_patient,
        //    modal: "#select-patient",
        //    initiatingElement: "#PartyName"
        //});

        //customer_list = Customer.party_list();
        //$('#customer-list').SelectTable({
        //    selectFunction: self.select_party,
        //    returnFocus: "#InvoiceNo",
        //    modal: "#select-party",
        //    initiatingElement: "#PartyName",
        //    selectionType: "radio"
        //});
        invoice_list = self.sales_invoice_list();
        item_select_table = $('#itempopup-list').SelectTable({
            selectFunction: self.remove_item_details,
            modal: "#add-invoice",
            initiatingElement: "#InvoiceNo",
        });
        self.customer_list();
        fh_items = $("#sales-return-items-list").FreezeHeader();
        self.bind_events();
    },
    view_type: "",
    details: function () {
        var self = CounterSalesReturn;
        fh_items = $("#sales-return-items-list").FreezeHeader();
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = CounterSalesReturn;
        $.ajax({
            url: '/Reports/Sales/CounterSalesReturnPrintPdf',
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
    customer_list: function (type) {
        var self = CounterSalesReturn;
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
    },
    sales_invoice_list: function () {

        var $list = $('#itempopup-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = '/Sales/CounterSales/GetCounterSalesForReturn';

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
                            { Key: "PartyID", Value: $('#PartyID').val() },
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
                            return "<input type='radio' class='CheckItem ID' name='ItemID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='PartyName' value='" + row.PartyName + "'>"
                                + "<input type='hidden' class='BillDiscount' value='" + row.BillDiscount + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
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

            $('body').on("change", '#PartyID', function () {
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
        var self = CounterSalesReturn;
        $('#tabs-CounterSalesReturn').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "counter-sales-return":
                $list = $('#counter-sales-return-list');
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

            var url = "/Sales/CounterSalesReturn/GetCounterSalesReturnListForDataTable?type=" + type;
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

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/CounterSalesReturn/Details/" + Id);

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
        var self = CounterSalesReturn;
        $.UIkit.autocomplete($('#party-autocomplete'), { 'source': self.get_party, 'minLength': 1 });
        $('#party-autocomplete').on('selectitem.uk.autocomplete', self.set_party);
        $("#btnOKParty").on("click", self.select_party);
        $("body").on("keyup change", "#sales-return-items-list tbody .ReturnQty.included", self.update_item);
        $("body").on("keyup change", "#sales-return-items-list tbody .SecondaryReturnQty.included", self.change_secondary_item);
        $(".btnSaveASDraft,.btnSave").on("click", self.on_save);
        $("#btnOkInvoiceList").on("click", self.remove_item_details);
        $("body").on('ifChanged', '.include-item', self.include_item);
        $("#PartyID").on('changed', self.refresh_invoice);
        $("body").on("change", "#sales-return-items-list tbody .Unit.included", self.Process_Item);
        $('body').on('change', "#PaymentModeID", self.get_bank_name);
        $("#btnOkAddPatient").on("click", self.select_patient);
        $("#btnOKCustomer").on("click", self.select_customer);
    },
    select_customer: function () {
        var self = CounterSalesReturn;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#CashSales").val(Name);
        $("#PartyName").val(Name);
        $("#PartyID").val(ID);
        $("#ContactID").val(0);
        $("#ContactName").val('');
        $("#PartyID").trigger("change");
        UIkit.modal($('#select-customer')).hide();
    },
    get_bank_name: function () {
        var self = CounterSalesReturn;
        var mode;

        if ($("#PaymentModeID option:selected").text() == "Select") {
            mode = "";

        }
        else if ($("#PaymentModeID option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }

        $.ajax({
            url: '/Masters/Treasury/GetBankForCounterSales',
            data: {

                Mode: mode
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#BankID").html("");
                var html;
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });

    },

    refresh_invoice: function () {
        list_table.fnDraw();
    },

    Process_Item: function (event) {
        var self = CounterSalesReturn;
        var row = $(event.target).closest('tr');
        var SalesUnitID = clean($(row).find(".SalesUnitID").val());
        var LoosePrice = clean($(row).find(".LoosePrice").val());
        var UnitID = clean($(row).find(".Unit option:selected").val());
        var FullPrice = clean($(row).find(".FullPrice").val());
        var CounterSalesTransUnitID = clean($(row).find(".CounterSalesTransUnitID").val());
        var Quantity = clean($(row).find(".Quantity").val());
        var ConvertedQuantity = clean($(row).find(".ConvertedQuantity").val());
        var CessPercentage = clean($(row).find(".CessPercentage").val());

        if (SalesUnitID == UnitID) {
            $(row).find(".Rate").val(FullPrice);
        }
        else {
            $(row).find(".Rate").val(LoosePrice);
        }
        if (CounterSalesTransUnitID == UnitID) {
            $(row).find(".Qty").val(Quantity);
        }
        else {
            $(row).find(".Qty").val(ConvertedQuantity);
        }
        self.update_item(event);
    },
    change_secondary_item: function () {
        var self = CounterSalesReturn;
        var $row = $(this).closest('tr');
        var SecondaryReturnQty = clean($row.find('.SecondaryReturnQty').val());
        var SecondaryUnitSize = clean($row.find('.SecondaryUnitSize').val());
        var ReturnQty = (SecondaryReturnQty * SecondaryUnitSize);
        $row.find('.ReturnQty').val(ReturnQty)
        $row.find('.ReturnQty').trigger('change');
    },
    update_item: function (event) {
        var self = CounterSalesReturn;
        var row = $(event.target).closest('tr');
        var ReturnQty = clean($(row).find('.ReturnQty').val());
        var SaledQty = clean($(row).find('.Qty').val());
        var Rate = clean($(row).find('.Rate').val());
        var GST = clean($(row).find(".GST").val());
        var VATPercentage = clean($(row).find(".VATPercentage ").val());
        var CessPercentage = clean($(row).find(".CessPercentage").val());
        var BasicPrice;
        var GrossAmount;
        var TaxableAmount;
        var GSTAmount;
        var NetAmount;
        var CGST;
        var SGST;
        var NetAmount;
        var VATAmount;
        var CessAmount;
        if (ReturnQty > SaledQty) {
            app.show_error('Invalid return quantity');
            app.add_error_class($(row).find('.ReturnQty'));
            return;
        }
        BasicPrice = (Rate * 100 / (100 + GST + CessPercentage)).roundTo(2);
        //BasicPrice = (Rate * 100 / (100 + VATPercentage + CessPercentage)).roundTo(2);
        GrossAmount = (BasicPrice * ReturnQty).roundTo(2);
        TaxableAmount = GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
        GSTAmount = (TaxableAmount * GST / 100).roundTo(2);
        VATAmount = (TaxableAmount * VATPercentage / 100).roundTo(2);
        CessAmount = (TaxableAmount * CessPercentage / 100).roundTo(2);

        CGST = (GSTAmount / 2).roundTo(2);
        SGST = (GSTAmount / 2).roundTo(2);
        //NetAmount = (TaxableAmount + CGST + SGST + CessAmount).roundTo(2);
        NetAmount = (TaxableAmount + VATAmount + CessAmount).roundTo(2);
        $(row).find(".GrossAmount").val(GrossAmount);
        $(row).find(".CGST").val(CGST);
        $(row).find(".SGST").val(SGST);
        $(row).find(".VATAmount").val(VATAmount);
        $(row).find(".CessAmount").val(CessAmount);
        $(row).find(".NetAmount").val(NetAmount);
        self.calculate_grid_total();

    },

    remove_item_details: function () {
        var self = CounterSalesReturn;
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
    select_sales_invoice: function () {
        var self = CounterSalesReturn;
        var radio = $('#itempopup-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var InvoiceNo = $(row).find(".TransNo").text().trim();
        var PartyName = $(row).find(".PartyName").val();
        var BillDiscount = $(row).find(".BillDiscount").val();
        $("#InvoiceNo").val(InvoiceNo);
        $("#InvoiceID").val(ID);
        $("#PartyName").val(PartyName);
        $("#BillDiscount").val(BillDiscount);
        var sales_invoice_ids = $("#itempopup-list .CheckItem:checked").map(function () {
            return $(this).val();
        }).get();
        var PurchaseRequisitionIDS = [];

        if (sales_invoice_ids.length == 0) {
            app.show_error('Please select  invoice');
            return;
        }
        $.ajax({
            url: '/Sales/CounterSales/GetCounterSalesTrans/',
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
                    //    if (PurchaseRequisitionIDS.indexOf(item.InvoiceNo) == -1) {
                    //        PurchaseRequisitionIDS.push(item.InvoiceNo);
                    //    }
                    index = $("#sales-return-items-list tbody tr").length + 1;
                    var Gst = item.CGSTPercentage + item.SGSTPercentage;
                    Unit = '<select class="md-input label-fixed Unit >' + CounterSalesReturn.Build_Select(item.UnitID, item.Unit, item.SalesUnitName, item.SalesUnitID, item.CounterSalesTransUnitID) + '</select>'

                    tr += '<tr>'
                        + ' <td class="uk-text-center">' + (i + 1) + ' </td>'
                        + "<td class='checked uk-text-center' data-md-icheck> "
                        + "<input type='checkbox' class='include-item'/>"
                        + '</td>'
                        + '<td >' + item.Code
                        + ' <input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                        + ' <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                        + ' <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                        + ' <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                        + ' <input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                        + ' <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                        + ' <input type="hidden" class="Stock" value="' + item.Stock + '" />'
                        + ' <input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                        + ' <input type="hidden" class="TaxableAmount" value="' + item.TaxableAmount + '" />'
                        + ' <input type="hidden" class="CounterSalesTransID" value="' + item.ID + '" />'
                        + ' <input type="hidden" class="SalesUnitID" value="' + item.SalesUnitID + '" />'
                        + ' <input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                        + ' <input type="hidden" class="FullPrice" value="' + item.FullPrice + '" />'
                        + ' <input type="hidden" class="LoosePrice" value="' + item.LoosePrice + '" />'
                        + ' <input type="hidden" class="CounterSalesTransUnitID" value="' + item.CounterSalesTransUnitID + '" />'
                        + ' <input type="hidden" class="ConvertedQuantity" value="' + item.ConvertedQuantity + '" />'
                        + ' <input type="hidden" class="Quantity" value="' + item.Quantity + '" />'
                        + ' <input type="hidden" class="SecondaryUnitSize" value="' + item.SecondaryUnitSize + '" />'
                        + '</td>'
                        + '<td class="ItemName">' + item.Name + '</td>'
                        + '<td class="uk-hidden">' + Unit + '</td>'
                        + '<td class="SecondaryUnit">' + item.SecondaryUnit + '</td>'
                        + '<td>' + item.BatchNo + '</d>'
                        + '<td class="uk-hidden"><input type="text" class="md-input Rate mask-currency" value="' + item.MRP + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input SecondaryRate mask-currency" value="' + item.SecondaryRate + '" readonly="readonly" /></td>'
                        + '<td class="uk-hidden"><input type="text" class="md-input Qty mask-numeric" value="' + item.Quantity + '"  readonly="readonly"  /></td>'
                        + '<td><input type="text" class="md-input SecondaryQty mask-numeric" value="' + item.SecondaryQty + '"  readonly="readonly"  /></td>'
                        + '<td class="action"><input type="text" class="md-input SecondaryReturnQty mask-numeric" value="' + 0 + '" disabled /></td>'
                        + '<td class="action uk-hidden"><input type="text" class="md-input ReturnQty mask-numeric" value="' + 0 + '" disabled /></td>'
                        + '<td><input type="text" class="md-input GrossAmount mask-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input DiscountPercentage mask-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input DiscountAmount mask-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input GST mask-currency" value="' + Gst + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input CGST mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input SGST mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input IGST mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input CessPercentage mask-sales-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                        //+ '<td><input type="text" class="md-input CessAmount mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input VATPercentage mask-currency" value="' + item.VATPercentage + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input VATAmount mask-currency" value="' + item.VATAmount + '" readonly="readonly" /></td>'
                        + '<td><input type="text" class="md-input NetAmount mask-currency" value="' + 0 + '" readonly="readonly" /></td>'

                        + '</tr>';
                });
                // $("#InvoiceNo").val(PurchaseRequisitionIDS.join(','));
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
        var self = CounterSalesReturn;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            self.calculate_grid_total();
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
            self.calculate_grid_total();
        }
        self.count_items();
    },

    set_party: function (event, item) {
        var self = CounterSalesReturn;
        $("#PartyName").val(item.Name);
        $("#PartyID").val(item.id).trigger("change");
    },
    get_party: function (release) {
        var self = CounterSalesReturn;

        $.ajax({
            url: '/Masters/Customer/GetPartyAutoComplete',
            data: {
                Hint: $('#PartyName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    select_party: function () {
        var self = CounterSalesReturn;
        var radio = $('#party-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text();
        var PartyID = $(row).find(".PartyID").val();
        $("#PartyName").val(Name);
        $("#PartyID").val(ID).trigger("change");
        UIkit.modal($('#select-party')).hide();

    },
    select_patient: function () {
        var self = CounterSalesReturn;
        var radio = $('#select-patient tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#PartyName").val(Name);
        $("#PartyID").val(ID);
        UIkit.modal($('#select-patient')).hide();
    },
    calculate_grid_total: function () {
        var self = CounterSalesReturn;
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var CessAmount = 0;
        var VatAmount = 0;
        var BillDiscount = 0;
        var temp = 0;

        $("#sales-return-items-list tbody tr.included").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            NetAmount += clean($(this).find('.NetAmount').val());
            VatAmount += clean($(this).find('.VATAmount').val());

        });
        BillDiscount = clean($("#BillDiscount").val());
        //temp = NetAmount - BillDiscount;
        //NetAmount = Math.round(NetAmount) - DiscountAmount;
        //RoundOff = temp - NetAmount;
        temp = NetAmount - BillDiscount;
        NetAmount = NetAmount - DiscountAmount;
        RoundOff = 0;
        $("#GrossAmount").val(GrossAmount);
        $("#DiscountAmount").val(DiscountAmount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#RoundOff").val(RoundOff.roundTo(2));
        $("#NetAmount").val(NetAmount);
        $("#VATAmount").val(VatAmount);
        self.count_items();

    },

    save: function (data, url, location) {

        var self = CounterSalesReturn;

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
                    if (typeof response.response[0].ErrorMessage != "undefined")
                        app.show_error(response.response[0].ErrorMessage);
                    $(".btnSaveAndPost,.btnSaveASDraft").css({ 'display': 'block' });
                }
            }
        });
    },
    on_save: function () {

        var self = CounterSalesReturn;
        var data = self.get_data();
        var location = "/Sales/CounterSalesReturn/Index";
        var url = '/Sales/CounterSalesReturn/Save';

        if ($(this).hasClass("btnSaveASDraft")) {
            data.IsDraft = true;
            url = '/Sales/CounterSalesReturn/SaveAsDraft'
            self.error_count = self.validate_form();
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
            PartyID: clean($("#PartyID").val()),
            ReturnNo: $("#ReturnNo").val(),
            ReturnDate: $("#ReturnDate").val(),
            DiscountAmount: clean($("#DiscountAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            BankID: clean($("#BankID").val()),
            PaymentModeID: clean($("#PaymentModeID").val()),
            InvoiceID: clean($("#InvoiceID ").val()),
            Reason: $("#Reason").val(),
            BillDiscount: clean($("#BillDiscount").val()),
            VATAmount: clean($("#VATAmount").val()),
        };
        data.Items = [];
        $("#sales-return-items-list tbody tr.included").each(function () {
            item = {
                ItemID: $(this).find(".ItemID").val(),
                ItemName: $(this).find(".ItemName").val(),
                UnitID: $(this).find(".Unit option:selected").val(),
                FullOrLoose: $(this).find(".FullOrLoose").text(),
                MRP: clean($(this).find(".Rate").val()),
                Quantity: clean($(this).find(".Qty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                GSTPercntage: $(this).find(".GST").val(),
                CGSTAmount: clean($(this).find(".CGST").val()),
                SGSTAmount: clean($(this).find(".SGST").val()),
                IGSTAmount: clean($(this).find(".IGST").val()),
                NetAmount: clean($(this).find(".NetAmount").val()),
                BatchTypeID: clean($(this).find(".BatchTypeID").val()),
                BatchID: clean($(this).find(".BatchID").val()),
                WareHouseID: clean($(this).find(".WareHouseID").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercentage").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercentage").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercentage").val()),
                CessPercentage: clean($(this).find(".CessPercentage").val()),
                CessAmount: clean($(this).find(".CessAmount").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").val()),
                ReturnQty: clean($(this).find(".ReturnQty").val()),
                CounterSalesTransID: clean($(this).find(".CounterSalesTransID").val()),
                SecondaryReturnQty: clean($(this).find(".SecondaryReturnQty").val()),
                SecondaryUnitSize: clean($(this).find(".SecondaryUnitSize").val()),
                SecondaryRate: clean($(this).find(".SecondaryRate").val()),
                SecondaryUnit: $(this).find(".SecondaryUnit").text().trim(),
                DiscountPercentage: clean($(this).find(".DiscountPercentage").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount").val()),
                VATPercentage: clean($(this).find(".VATPercentage").val()),
                VATAmount: clean($(this).find(".VATAmount").val()),
            }
            data.Items.push(item);
        });
        return data;
    },


    validate_form: function () {
        var self = CounterSalesReturn;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
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
                            $("#sales-return-items-list tbody tr").each(function () {
                                if ($(this).find(".ItemID").val() == $(element).val()) {
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
                elements: "#BankID",
                rules: [
                    { type: form.non_zero, message: "Bank name required" },
                ]
            },


            {
                elements: ".ReturnQty.included",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var ReturnQty = clean($(row).find('.ReturnQty').val());
                            var SaleQty = clean($(row).find('.Qty').val());
                            return ReturnQty <= (SaleQty)
                        }, message: "Invalid return quantity"
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
            }
        ],

    }
};
Number.prototype.roundToCustom = function () {

    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};

