var item_list;
var fh_items;
InterCompanyPurchaseInvoice = {

    init: function () {
        var self = InterCompanyPurchaseInvoice;
        supplier.InterCompany_supplier_list();
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#InvoiceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });

        $('#Purchase-invoice-items-list').SelectTable({
            selectFunction: self.remove_item_details,
            modal: "#get-Invoice",
            initiatingElement: "#InvoiceNo",
            selectionType: "radio",
        });

        self.freeze_headers();
        self.invoice_list();
        self.supplier_list();
        self.bind_events();
    },

    details: function () {
        var self = InterCompanyPurchaseInvoice;
        freeze_header = $("#purchase-invoice-items-list").FreezeHeader();
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = InterCompanyPurchaseInvoice;
        $.ajax({
            url: '/Reports/Purchase/InterCompanyPurchaseInvoicePrintPdf',
            data: {
                id: $("#Id").val(),
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

    freeze_headers: function () {
        fh_items = $("#Purchase-invoice-items-list").FreezeHeader();
        $('#tabs-invoice[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "item-tab") {
                    fh_items.resizeHeader();
                }
            }, 500);
        });
    },
    invoice_list: function () {
        var self = InterCompanyPurchaseInvoice;
        $list = $('#invoice-popup-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            self.invoice_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    supplier_list: function () {
        var self = InterCompanyPurchaseInvoice;
        $list = $('#supplier-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();


            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    list: function () {
        var $list = $('#purchase-invoice-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Purchase/InterCompanyPurchaseInvoice/GetInterCompanyList"

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
                   { "data": "PurchaseNo", "className": "PurchaseNo" },
                   { "data": "PurchaseDate", "className": "PurchaseDate" },
                   { "data": "InvoiceNo", "className": "InvoiceNo" },
                   { "data": "InvoiceDate", "className": "InvoiceDate" },
                   { "data": "SupplierName", "className": "SupplierName" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/InterCompanyPurchaseInvoice/Details/" + Id);
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
        var self = InterCompanyPurchaseInvoice;
        $("#btnOKSupplier").on("click", self.select_supplier);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $(".btnSave").on("click", self.save_confirm);

        $("#btnOkInvoiceList").on("click", self.remove_item_details);
    },
    remove_item_details: function () {
        var self = InterCompanyPurchaseInvoice;
        if ($("#Purchase-invoice-items-list tbody tr").length > 0) {
            app.confirm("Selected items will be removed", function () {
                $('#Purchase-invoice-items-list tbody').empty();
                self.count_items();
                self.select_invoice();
            })
        }
        else {
            self.select_invoice();
        }
    },
    select_invoice: function () {
        var self = InterCompanyPurchaseInvoice;
        var invoiceid = $("#invoice-popup-list .CheckItem:checked").val();

        var PurchaseInvoiceIDS = [];
        var igst, cgst, sgst, total, rate, acceptedqty, sumigst = 0, sumsgst = 0, sumcgst = 0, netamount = 0;
        if (invoiceid.length == 0) {
            app.show_error('Please select invoice');
            return;
        }
        $.ajax({
            url: '/Sales/SalesInvoice/GetIntercompanySalesInvoice/',
            dataType: "json",
            data: {
                invoiceid: invoiceid,
                LocationID: clean($("#LocationID").val())
            },
            type: "POST",
            success: function (sales_invoice_items) {

                var $sales_invoice_items_list = $('#Purchase-invoice-items-list tbody');
                $sales_invoice_items_list.html('');
                var tr = '';
                var html = '';
                var ApprovedValue;
                var invoicevalue;
                var difference;
                var SerialNo = $(".tbody .rowPr").length;
                var Amount_Serial_No = '';
                var totaldifference = 0;
                var gsrtPercentage = 0; var Selectedclass = "";
                $.each(sales_invoice_items.Items, function (i, item) {
            
                    if (item.IGST > 0) {
                        item.SGSTPercentage = 0;
                        item.CGSTPercentage = 0;
                    }
                    else {
                        item.IGSTPercentage = 0;
                    }
                    SerialNo++;
                    ApprovedValue = item.POQuantity * item.PORate;
                    invoicevalue = (item.OfferQty * item.InvoiceQty) * item.MRP;
                    difference = ApprovedValue - invoicevalue;
                    totaldifference += difference;
                    gstPercentage = item.SGSTPercentage + item.CGSTPercentage + item.IGSTPercentage;
                    html += '  <tr class="rowPr">'
                                 + ' <td class="uk-text-center">' + SerialNo + '</td>'
                                 + '     <input type="hidden" class="ItemID " value="' + item.ItemID + '"  />'
                                 + '     <input type="hidden" class="UnitID " value="' + item.UnitID + '"  />'
                                 + '     <input type="hidden" class="BatchID " value="' + item.BatchID + '"  />'
                                 + '     <input type="hidden" class="IGSTPercentage " value="' + item.IGSTPercentage + '" />'
                                 + '     <input type="hidden" class="SGSTPercentage " value="' + item.SGSTPercentage + '" />'
                                 + '     <input type="hidden" class="CGSTPercentage " value="' + item.CGSTPercentage + '" />'
                                 + '     <input type="hidden" class="IGSTAmount " value="' + item.IGST + '" />'
                                 + '     <input type="hidden" class="SGSTAmount " value="' + item.SGST + '" />'
                                 + '     <input type="hidden" class="CGSTAmount " value="' + item.CGST + '" />'
                                 + '     <input type="hidden" class="SalesInvoiceTransID " value="' + item.SalesInvoiceTransID + '" />'
                                 + '     <input type="hidden" class="POID " value="' + item.POID + '" />'
                                 + '     <input type="hidden" class="POTransID " value="' + item.POTransID + '" />'
                                 + '     <input type="hidden" class="Qty " value="' + (item.InvoiceQty + item.OfferQty) + '" />'
                                 + '     <input type="hidden" class="OfferQty " value="' + item.OfferQty + '" />'
                                 + '     <input type="hidden" class="PORate " value="' + item.PORate + '" />'
                                 + '     <input type="hidden" class="BatchTypeID " value="' + item.BatchTypeID + '" />'
                                 + '     <input type="hidden" class="SalesInvoiceID " value="' + item.SalesInvoiceID + '" />'
                                 + '     <input type="hidden" class="Batch " value="' + item.Batch + '" />'

                                 + '     <input type="hidden" class="ApprovedValue " value="' + ApprovedValue + '" />'
                                 + '     <input type="hidden" class="InvoiceValue " value="' + invoicevalue + '" />'
                                 + '     <input type="hidden" class="Difference " value="' + difference + '" />'
                                 + ' </td>'
                                 + ' <td >' + item.ItemName + '</td>'
                                 + ' <td >' + item.UnitName + '</td>'
                                 + ' <td >' + item.BatchType + '</td>'
                                 + ' <td >' + item.BatchName + '</td>'
                                 + ' <td ><input type="text" class="  md-input mask-qty" value="' + item.InvoiceQty + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="OfferQty  md-input mask-qty" value="' + item.OfferQty + '" readonly="readonly" /></td>'
                                 + ' <td class="mrp_hidden" ><input type="text" class="MRP  md-input mask-currency" value="' + item.MRP + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="BasicPrice  md-input mask-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="GrossAmount  md-input mask-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="AdditionalDiscount  md-input mask-currency" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="DiscountPercentage  md-input mask-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="DiscountAmount  md-input mask-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="TurnoverDiscount  md-input mask-currency" value="' + item.TurnoverDiscount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="TaxableAmount  md-input mask-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="GST  md-input mask-currency" value="' + gstPercentage + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="GSTAmount  md-input mask-currency" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="CashDiscount  md-input mask-currency" value="' + item.CashDiscount + '" readonly="readonly" /></td>'
                                 + ' <td ><input type="text" class="NetAmount  md-input mask-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                                 + '</tr>';
                });

                var $html = $(html);
                app.format($html);

                $sales_invoice_items_list.append($html);

                //   $("#InvoiceNo").val(PurchaseInvoiceIDS.join(','));
                $.each(sales_invoice_items.AmountDetails, function (i, record) {
                    tr += "<tr  class='uk-text-center'>";
                    tr += "<td>" + (i + 1);
                    tr += "</td>";
                    tr += "<td class='Particulars'>" + record.Particulars;
                    tr += "</td>";
                    tr += "<td><input type='text' class='md-input mask-purchase-currency Percentage' readonly value='" + record.Percentage + "' />";
                    tr += "</td>";
                    tr += "<td><input type='text' class='md-input mask-purchase-currency Amount' readonly value='" + record.Amount + "' />";
                    tr += "</td>";
                    tr += "</tr>";
                });
                $tr = $(tr);
                app.format($tr);
                $("#Purchase-invoice-amount-details-list tbody").html($tr);
                var cashdiscount = sales_invoice_items.CashDiscountEnabled == true ? 'check' : 'uncheck';
                $("#SalesInvoiceNo").val(sales_invoice_items.InvoiceNo);
                $("#SalesInvoiceDate").val(sales_invoice_items.InvoiceDate);
                $("#GrossAmount").val(sales_invoice_items.GrossAmount);
                $("#AdditionalDiscount").val(sales_invoice_items.AdditionalDiscount);
                $("#DiscountAmount").val(sales_invoice_items.DiscountAmount);
                $("#TurnoverDiscount").val(sales_invoice_items.TurnoverDiscount);
                $("#TurnoverDiscountAvailable").val(sales_invoice_items.TurnoverDiscountAvailable);
                $("#TaxableAmount").val(sales_invoice_items.TaxableAmount);
                $("#SGSTAmount").val(sales_invoice_items.SGSTAmount);
                $("#CGSTAmount").val(sales_invoice_items.CGSTAmount);
                $("#IGSTAmount").val(sales_invoice_items.IGSTAmount);
                $("#CashDiscount").val(sales_invoice_items.CashDiscount);
                $("#RoundOff").val(sales_invoice_items.RoundOff);
                $("#PaymentMode").val(sales_invoice_items.PaymentMode);
                $("#PaymentModeID").val(sales_invoice_items.PaymentModeID);
                $("#BillingAddressID").val(sales_invoice_items.BillingAddressID);
                $("#ShippingAddressID").val(sales_invoice_items.ShippingAddressID);
                $("#CashDiscountEnabled").iCheck(cashdiscount);
                $("#NetAmount").val(sales_invoice_items.NetAmount);
                $("#TotalDifference").val(totaldifference);
                $("#SalesInvoiceID").val(sales_invoice_items.ID);
                setTimeout(function () {
                    fh_items.resizeHeader();
                }, 200);
                self.count_items();

            },
        });

    },
    save_confirm: function () {
        var self = InterCompanyPurchaseInvoice
        app.confirm_cancel("Do you want to save ?", function () {
            self.save_and_new();
        }, function () {
        })
    },

    count_items: function () {
        var count = $('#Purchase-invoice-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    save_and_new: function () {
        var self = InterCompanyPurchaseInvoice;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        else {
            var model = self.get_data(false);
            $(".btnSave").css({ 'display': 'none' });
            $.ajax({
                url: '/Purchase/InterCompanyPurchaseInvoice/Save',
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Inter company purchase invoice created successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/InterCompanyPurchaseInvoice/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to create Inter company purchase invoice");
                        $(".btnSave ").css({ 'display': 'block' });
                    }
                },
            });
        }
    },

    get_data: function (IsDraft) {
        self = InterCompanyPurchaseInvoice;
        var model = {
            InvoiceNo: $("#InvoiceNo").val(),
            SupplierID: $("#SupplierID").val(),
            PurchaseDate: $("#PurchaseDate").val(),
            SalesInvoiceNo: $("#SalesInvoiceNo").val(),
            SalesInvoiceDate: $("#SalesInvoiceDate").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            AdditionalDiscount: clean($("#AdditionalDiscount").val()),
            DiscountAmount: clean($("#DiscountAmount").val()),
            TurnoverDiscount: clean($("#TurnoverDiscount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            NetAmount: clean($("#NetAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            CashDiscount: clean($("#CashDiscount").val()),
            RoundOff: clean($("#RoundOff").val()),
            PaymentMode: clean($("#PaymentMode").val()),
            PaymentModeID: clean($("#PaymentModeID").val()),
            ShippingAddressID: clean($("#ShippingAddressID").val()),
            BillingAddressID: clean($("#BillingAddressID").val()),
            TradeDiscount: clean($("#DiscountAmount").val()),
            NetAmount: clean($("#NetAmount").val()),
            TotalDifference: clean($("#TotalDifference").val()),
            SalesInvoiceID: clean($("#SalesInvoiceID").val()),
            CashDiscountEnabled: $("#CashDiscountEnabled").prop("checked") == true ? true : false,
            Trans: self.GetProductList(),
            TaxDetails: self.AmountDetails(),
            // TaxDetails: self.TaxDetails()
        };
        return model;
    },
    AmountDetails: function () {
        var AmountDetails = [];
        var i = 0;
        $("#Purchase-invoice-amount-details-list tbody tr").each(function () {
            i++;

            AmountDetails.push({

                Particulars: $(this).find('.Particulars').text(),
                Percentage: clean($(this).find('.Percentage').val()),
                Amount: clean($(this).find('.Amount').val()),

            });
        })
        return AmountDetails;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var i = 0;
        $("tbody .rowPr").each(function () {
            i++;

            ProductsArray.push({
                ItemID: $(this).find('.ItemID').val(),
                UnitID: $(this).find('.UnitID').val(),
                BatchID: clean($(this).find('.BatchID').val()),
                IGSTPercent: clean($(this).find('.IGSTPercentage').val()),
                SGSTPercent: clean($(this).find('.SGSTPercentage').val()),
                CGSTPercent: clean($(this).find('.CGSTPercentage').val()),
                SalesInvoiceTransID: clean($(this).find('.SalesInvoiceTransID ').val()),
                PurchaseOrderID: clean($(this).find('.POID ').val()),
                POTransID: clean($(this).find('.POTransID').val()),
                InvoiceQty: clean($(this).find('.Qty').val()),
                PORate: clean($(this).find('.PORate').val()),
                BatchTypeID: clean($(this).find('.BatchTypeID').val()),
                SalesInvoiceID: clean($(this).find('.SalesInvoiceID').val()),
                InvoiceRate: clean($(this).find('.MRP').val()),
                BasicPrice: clean($(this).find('.BasicPrice').val()),
                GrossAmount: clean($(this).find('.GrossAmount').val()),
                AdditionalDiscount: clean($(this).find('.AdditionalDiscount').val()),
                DiscountPercentage: clean($(this).find('.DiscountPercentage').val()),
                DiscountAmount: clean($(this).find('.DiscountAmount').val()),
                TurnoverDiscount: clean($(this).find('.TurnoverDiscount').val()),
                TaxableAmount: clean($(this).find('.TaxableAmount').val()),
                GST: clean($(this).find('.GST').val()),
                GSTAmount: clean($(this).find('.GSTAmount').val()),
                CashDiscount: clean($(this).find('.CashDiscount').val()),
                NetAmount: clean($(this).find('.NetAmount').val()),
                Batch: $(this).find('.Batch').val(),
                ApprovedValue: clean($(this).find('.ApprovedValue').val()),
                InvoiceValue: clean($(this).find('.InvoiceValue').val()),
                Difference: clean($(this).find('.Difference').val()),
                TradeDiscAmount: clean($(this).find('.TradeDiscAmount').val()),
                IGSTAmt: clean($(this).find('.IGSTAmount').val()),
                SGSTAmt: clean($(this).find('.SGSTAmount').val()),
                CGSTAmt: clean($(this).find('.CGSTAmount').val()),
            });
        })
        return ProductsArray;
    },
    clearControls: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#UnitID").val('');
        $("#UnitName").val(' ');
        $("#Stock").val(' ');
        $("#Premises").val('');
        $("#GRNno").val('');
        $("#AcceptedQty").val(' ');
        $("#Rate").val(' ');
        $("#SGSTPercentage").val(' ');
        $("#CGSTPercentage").val(' ');
        $("#IGSTPercentage").val(' ');
        $("#Remark").val(' ');
        $("#ReturnQty").val(' ');
        $("#item-count").val(0);
        $("#SalesInvoiceNo").val('');
        $("#SalesInvoiceDate").val('');
        $("#GrossAmount").val(0);
        $("#NetAmount").val(0);
        $("#AdditionalDiscount").val(0);
        $("#DiscountAmount").val(0);
        $("#TurnoverDiscount").val(0);
        $("#TaxableAmount").val(0);
        $("#SGSTAmount").val(0);

        $("#CGSTAmount").val(0);
        $("#IGSTAmount").val(0);
        $("#CashDiscount").val(0);
        $("#RoundOff").val(0);



        $('#tblPurchaseReturnItems tbody').html('')

    },
    set_supplier_details: function (event, item) {
        self = InterCompanyPurchaseInvoice;
        if (($("#Purchase-invoice-items-list tbody tr").length > 0 && (item.id != $("#SupplierID").val()))) {
            app.confirm("Selected items will be removed", function () {
                $('#Purchase-invoice-items-list tbody').empty();
                $('#itempopup-list tbody').empty();
                $("#SupplierName").val(item.name);
                $("#CustomerID").val(item.customerId);
                $("#SupplierID").val(item.id);
                $("#LocationID").val(item.locationId);
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(item.name);
            $("#CustomerID").val(item.customerId);
            $("#SupplierID").val(item.id);
            $("#LocationID").val(item.locationId);
            self.get_invoice();
        }

        self.clearControls();
    },
    get_invoice: function () {
        var self = InterCompanyPurchaseInvoice
        $('#invoice-popup-list tbody').empty();
        var total = 0;
        $("#txtNetAmount").val(total);
        self.error_count = 0;
        self.error_count = self.validate_select_invoice();
        if (self.error_count > 0) {
            return;
        }
        var SupplierID = $("#CustomerID").val();
        var LocationID = $("#LocationID").val();
        $("#InvoiceNo").val("");

        $.ajax({
            url: '/Sales/SalesInvoice/GetIntercompanySalesInvoiceList/',
            dataType: "json",
            type: "GET",
            data: {
                SupplierID: SupplierID,
                LocationID: LocationID
            },
            success: function (response) {
                self.invoice_list_table.fnDestroy();
                var $invoice_list = $('#invoice-popup-list tbody');
                $invoice_list.html('');
                var tr = '';
                $.each(response, function (i, invoice) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center ' data-md-icheck><input type='radio'  name='CheckItem'  class='CheckItem' value=" + invoice.ID + " /></td>"
                        + "<td>" + invoice.InvoiceNo + "</td>"
                        + "<td>" + invoice.InvoiceDate + "</td>"
                        + "<td><div class='mask-currency'>" + invoice.NetAmount + "</div></td>"
                    + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $invoice_list.append($tr);
                });
                self.invoice_list();
            },
        });
    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/InterCompanySupplier',
            data: {
                Term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    select_supplier: function (event) {
        var self = InterCompanyPurchaseInvoice;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = clean(radio.val());
        var Name = $(row).find(".Name").text().trim();
        var CustomerID = $(row).find(".CustomerID").val();
        var LocationID = $(row).find(".LocationID").val();
        if ((($("#Purchase-invoice-items-list tbody tr").length > 0) && (ID != clean($("#SupplierID").val())))) {
            app.confirm("Selected items will be removed", function () {
                $('#Purchase-invoice-items-list tbody').empty();
                $("#SupplierName").val(Name);
                $("#SupplierID").val(ID);
                $("#CustomerID").val(CustomerID);
                $("#LocationID").val(LocationID);
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(Name);
            $("#SupplierID").val(ID);
            $("#CustomerID").val(CustomerID);
            $("#LocationID").val(LocationID);
            self.get_invoice();
        }
        UIkit.modal($('#select-supplier')).hide();
        self.clearControls();
    },

    validate_form: function () {
        var self = InterCompanyPurchaseInvoice;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_select_invoice: function () {
        var self = InterCompanyPurchaseInvoice;
        if (self.rules.on_select_invoices.length) {
            return form.validate(self.rules.on_select_invoices);
        }
        return 0;
    },
    rules: {

        on_blur: [],
        on_submit: [
                 {
                     elements: ".clReturnQty",
                     rules: [
            {
                type: function (element) {
                    var error = false;
                    $('.clReturnQty').each(function () {
                        var row = $(this).closest('tr');
                        var returnqty = clean($(row).find('.clReturnQty').val());
                        var AvailableStock = clean($(row).find('.Stock').val());
                        var accepted = clean($(row).find('.clAcptQty ').val());
                        if (returnqty > AvailableStock)
                            error = true;

                    });
                    return !error;
                }, message: 'Selected item dont have enough stock'
            },
            {
                type: function (element) {
                    var error = false;
                    $('.clReturnQty').each(function () {
                        var row = $(this).closest('tr');
                        var returnqty = clean($(row).find('.clReturnQty').val());
                        if (returnqty <= 0) {
                            error = true;
                        }

                    });
                    return !error;
                }, message: 'Return quantity must be greater than zero'
            },
            {
                type: function (element) {
                    var error = false;
                    $('.clReturnQty').each(function () {
                        var row = $(this).closest('tr');
                        var returnqty = clean($(row).find('.clReturnQty').val());
                        var AvailableStock = clean($(row).find('.Stock').val());
                        var accepted = clean($(row).find('.clAcptQty ').val());
                        if (returnqty > accepted)
                            error = true;

                    });
                    return !error;
                }, message: 'Return qty exceeds accepted qty'
            },
                     ]
                 },
           {
               elements: "#txtFreight",
               rules: [
                      { type: form.positive, message: "Invalid freight" },
               ]
           },
                    {
                        elements: "#txtOtherCharges",
                        rules: [
                               { type: form.positive, message: "Invalid other charges" },
                        ]
                    },
                    {
                        elements: "#txtPackingCharges",
                        rules: [
                               { type: form.positive, message: "Invalid packing charges" },
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
                        elements: "#item-count",
                        rules: [
                            { type: form.required, message: "Please add atleast one item" },
                            { type: form.non_zero, message: "Please add atleast one item" },
                        ]
                    },
        ],
        on_select_invoices: [

{
    elements: "#SupplierID",
    rules: [
        { type: form.required, message: "Please choose supplier" },
        {
            type: function (element) {
                return ($(element).val() > 0)

            }, message: 'Please select supplier'
        },
    ]
},
        ],
    },
}








