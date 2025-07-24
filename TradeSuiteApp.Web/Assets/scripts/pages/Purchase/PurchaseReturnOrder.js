var item_list;
var freeze_header;
var gridcurrencyclass = '';
var DecPlaces = 0;
$(function () {
    DecPlaces = clean($("#DecimalPlaces").val());
    gridcurrencyclass = $("#normalclass").val();
});
purchase_return_order = {

    init: function () {
        var self = purchase_return_order;
        supplier.supplier_list('GRNWise');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#InvoiceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });

        $('#invoice-popup-list').SelectTable({
            selectFunction: self.remove_item_details,
            modal: "#add-invoice",
            initiatingElement: "#InvoiceNo",
            startFocusIndex: 3,
            selectionType: "checkbox"
        });

        freeze_header = $("#tblPurchaseReturnItems").FreezeHeader();
        setTimeout(function () {
            freeze_header.resizeHeader();
        }, 100);


        self.invoice_list();
        self.bind_events();
        if ($("#ID").val() > 0) {
            self.get_invoice();
        }
        self.load_gst_dropDown();
    },

    details: function () {
        var self = purchase_return_order
        //freeze_header = $("#tblPurchaseReturnItems").FreezeHeader();
        $("body").on("click", ".printpdf", self.printpdf);
    },
    printpdf: function () {
        var self = purchase_return_order
        $.ajax({
            url: '/Reports/Purchase/PurchaseReturnPrintPdf',
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
    invoice_list: function () {
        var self = purchase_return_order;
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
                "bFilter": true,
                "bSort": false,
                "pageLength": 50,
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    list: function () {
        var self = purchase_return_order;
        $('#tabs-PurchaseReturnOrder').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "purchase-return-order":
                $list = $('#purchase-return-order-list');
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

            var url = "/Purchase/PurchaseReturnOrder/GetPurchaseReturnOderListForDataTable?type=" + type;
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>"

                        }
                    },

                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "SupplierName", "className": "SupplierName" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseReturnOrder/Details/" + Id);

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
        var self = purchase_return_order

        $
        $("#btnOKSupplier").on("click", self.select_supplier);

        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("body").on('ifChanged', '.include-item', self.include_item);
        $("#SupplierName").on("change", self.clearItemSelectControls);
        $("body").on("keyup change", ".clReturnQty", self.check_stock_quantity);
        $("body").on("keyup change", ".SecondaryReturnQty", self.secondary_check_stock_quantity);
        $(".btnSaveAndNew, .btnSaveAsDraft").on("click", self.on_save);
        $("#btnOkInvoiceList").on("click", self.remove_item_details);
        //    $("body").on("change", "#tblPurchaseReturnItems tbody .Unit", self.check_stock_quantity);
        $("body").on("change", "#tblPurchaseReturnItems .GstPercentageID", self.on_change_discount_gst_and_qty);
        $("body").on("keyup", "#tblPurchaseReturnItems .Discount", self.on_change_discount_gst_and_qty);
        $('#Discount, #IGSTAmount , #SGSTAmount , #CGSTAmount').on('keyup', self.calculate_total);
        $('#FreightAmount, #SupplierOtherCharges , #PackingForwarding , #SuppShipAmount,#SuppDocAmount').on('keyup', self.claculate_grid_total);
    },

    calculate_total: function () {
        var self = purchase_return_order;
        var NetAmount = $("#txtNetAmount").val();
        var GrossAmnt = $("#TaxableAmount").val()
        var SGSTAmount = $("#SGSTAmount").val();
        var IGSTAmount = $("#IGSTAmount").val();
        var CGSTAmount = $("#CGSTAmount").val();
        var discount = clean($('#Discount').val());
        GrossAmnt = GrossAmnt - (discount);
        var GSTAmount = clean($('#SGSTAmount').val()) + clean($('#CGSTAmount').val()) + clean($('#IGSTAmount').val())
        NetAmount = GrossAmnt + GSTAmount
        $("#txtNetAmount").val(NetAmount);

    },

    on_change_discount_gst_and_qty: function () {
        var self = purchase_return_order;
        var row = $(this).closest('tr');
        var IsGSTRegistred = $("#IsGSTRegistred").val();
        var StateID = $("#StateID").val();
        var ShippingStateID = $("#ShippingStateID").val();
        var Rate = clean($(row).find('.clRate').val());
        var qty = clean($(row).find('.clReturnQty').val());
        var GST = clean($(row).find('.GstPercentageID option:selected').text());
        var Discount = clean($(row).find('.Discount').val());
        var taxableAmt = Rate * qty;
        var amount = (Rate * qty) - Discount;

        var gstamount = GST * amount / 100;
        var clSgstAmount = 0
        var clIgstAmount = 0
        if (IsGSTRegistred == "false") {
            gstamount = 0
            clSgstAmount = 0
            clIgstAmount = 0;
        } else {
            if (ShippingStateID == StateID) {
                clSgstAmount = gstamount / 2
                clIgstAmount = 0;
            }
            else {
                clSgstAmount = 0;
                clIgstAmount = gstamount;
            }

        }
        var totalamount = gstamount + amount;
        $(row).find('.clTotal').text(totalamount.toFixed(2))
        $(row).find('.gstamt').text(gstamount.toFixed(2))
        $(row).find('.clSgstAmount').text(clSgstAmount.toFixed(2))
        $(row).find('.clCgstAmount').text(clSgstAmount.toFixed(2))
        $(row).find('.clIgstAmount').text(clIgstAmount.toFixed(2))
        $(row).find('.TaxableAmount').text(taxableAmt.toFixed(2))
        self.claculate_grid_total();
    },
    calculate_itemwise_gst: function () {
        var self = purchase_return_order;

        var row = $(this).closest('tr');
        var Rate = clean($(row).find('.clRate').val());
        var qty = clean($(row).find('.clReturnQty').val());
        var GST = clean($(row).find('.GstPercentageID option:selected').text());
        var amount = Rate * qty;
        var gstamount = GST * amount / 100;
        var totalgstamount = gstamount + amount;
        $(row).find('.clTotal').text(totalgstamount.toFixed(2))
        $(row).find('.gstamt').text(gstamount.toFixed(2))

        var NetAmount = 0;
        $("#tblPurchaseReturnItems tbody tr.included").each(function () {

            var amt = parseInt($(this).find('.clTotal').text().replace(",", ""));
            NetAmount = NetAmount + amt;
        });
        $("#txtNetAmount").val(NetAmount);


    },

    include_item: function () {
        var self = purchase_return_order;
        if ($(this).is(":checked")) {

            $(this).closest('tr').addClass('included');
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
        } else {

            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).removeAttr('disabled');
        }
        purchase_return_order.count_items();
        purchase_return_order.claculate_grid_total();
    },

    remove_item_details: function () {
        var self = purchase_return_order;
        if ($("#tblPurchaseReturnItems tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                self.count_items();
                self.select_invoice();
            })
        }
        else {
            self.select_invoice();
        }
    },

    load_gst_dropDown: function () {
        $.ajax({
            url: '/Masters/GSTCategory/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    GSTList = response.data;
                }
            }
        });
    },

    select_invoice: function () {
        var self = purchase_return_order;
        var IsGSTRegistred = $("#IsGSTRegistred").val();
        var StateID = $("#StateID").val();
        var ShippingStateID = $("#ShippingStateID").val();
        //var invoice_ids = [];
        var invoice_ids = $("#invoice-popup-list .CheckItem:checked").map(function () {
            return $(this).val();
        }).get();
        //var checkboxes = $('#invoice-popup-list tbody input[type="checkbox"]:checked');
        //$.each(checkboxes, function () {
        //    row = $(this).closest("tr");
        //    invoice_ids.push($(this).val());
        //});

        if (invoice_ids.length == 0) {
            app.show_error('Please select atleast one invoice');
            return;
        }
        $.ajax({
            url: '/Purchase/PurchaseInvoice/GetPurchaseInvoice/',
            dataType: "json",
            data: {
                InvoiceIDS: invoice_ids,
            },
            type: "POST",
            success: function (invoice) {

              //  $("#SuppDocAmount").val(invoice.SuppDocAmount);
              //  $("#SuppShipAmount").val(invoice.SuppShipAmount);
              //  $("#PackingForwarding").val(invoice.PackingForwarding);
              //  $("#SupplierOtherCharges").val(invoice.SupplierOtherCharges);
              //  $("#FreightAmount").val(invoice.FreightAmount);
                $("#LocalCustomsDuty").val(invoice.LocalCustomsDuty);
                $("#LocalFreight").val(invoice.LocalFreight);
                $("#LocalMiscCharge").val(invoice.LocalMiscCharge);
                $("#LocalOtherCharges").val(invoice.LocalOtherCharges);
                self.add_items_to_grid(invoice.InvoiceTransItems);
                self.claculate_grid_total();

                setTimeout(function () {
                    freeze_header.resizeHeader();
                }, 500);
                self.count_items();
                $("#ItemName").focus();

            },
        });
    },
    claculate_grid_item: function (item) {

        item.GrossAmount = (item.Rate * item.InvoiceQty) + item.Discount;// item.Rate return as net innvoice rate
        item.TaxableAmount = item.GrossAmount - item.Discount;
        item.GSTPercent = item.IGSTPercent + item.CGSTPercent + item.SGSTPercent;
        item.IGSTAmount = 0.0;
        item.CGSTAmount = 0;
        item.SGSTAmount = 0;
        item.VATAmount = (item.TaxableAmount * item.VATPercentage / 100).roundTo(2);
        //if (item.SGSTAmt == 0 && item.CGSTAmt == 0 && item.IGSTAmt>0)
        if (IsGSTRegistred == "false") {
            item.IGSTPercent = 0.00;
            item.CGSTPercent = 0.00;
            item.SGSTPercent = 0.00;
        }
        else {
            if (ShippingStateID != StateID) {
                item.IGSTAmount = item.GSTPercent * item.GrossAmount / 100;
                item.IGSTPercent = item.GSTPercent;
                item.CGSTPercent = 0.00;
                item.SGSTPercent = 0.00;
            }
            //else if (item.SGSTAmt > 0 && item.CGSTAmt > 0 && item.IGSTAmt == 0)
            else {
                item.IGSTPercent = 0.00;
                item.CGSTPercent = item.GSTPercent / 2;
                item.SGSTPercent = item.GSTPercent / 2;
                item.CGSTAmount = item.CGSTPercent * item.GrossAmount / 100;
                item.SGSTAmount = item.SGSTPercent * item.GrossAmount / 100;
            }
        }

        item.NetAmount = item.GrossAmount + item.IGSTAmount + item.SGSTAmount + item.CGSTAmount + item.VATAmount - item.Discount;
        item.GSTAmount = item.IGSTAmount + item.SGSTAmount + item.CGSTAmount;
        return item;
    },
    add_items_to_grid: function (items) {
        var self = purchase_return_order;
        var $tblPurchaseReturnItems = $('#tblPurchaseReturnItems tbody');
        $tblPurchaseReturnItems.html('');
        var PurchaseInvoiceIDS = [];
        var html = '';
        var SerialNo = $(".tbody .rowPr").length;
        $.each(items, function (i, item) {
            if (PurchaseInvoiceIDS.indexOf(item.InvoiceNo) == -1) {
                PurchaseInvoiceIDS.push(item.InvoiceNo);
            }
            SerialNo++;
            item = self.claculate_grid_item(item);
            //   Unit = '<select class="md-input label-fixed Unit >' + purchase_return_order.Build_Select(item.PrimaryUnitID, item.PrimaryUnit, item.PurchaseUnit, item.PurchaseUnitID, item.UnitID) + '</select>'
            //var gstPercent = item.IGSTPercent + item.CGSTPercent + item.SGSTPercent;
            //gstPercent = '<select class="md-input label-fixed GstPercentageID">' + purchase_return_order.Build_Select_Gst(GSTList, setgstpercent) + '</select>'
            //var gstPercent = item.IGSTPercent;
            html += '<tr>'
                + '<td class="uk-text-center">' + SerialNo + '</td>'
                + "<td class='uk-text-center'><input type='checkbox' class='include-item' data-md-icheck/>"
                + '<input type="hidden" class="ItemID" value="' + item.ItemID + '" />'
                + '<input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                + '<input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '<input type="hidden" class="WareHouseID" value="' + item.WarehouseID + '" />'
                + '<input type="hidden" class="BatchTypeID" value="' + 1 + '" />'
                + '<input type="hidden" class="Qty" value="' + item.InvoiceQty + '" />'
                + '<input type="hidden" class="ConvertedQty" value="' + item.ConvertedQty + '" />'
                + '<input type="hidden" class="ConvertedStock" value="' + item.ConvertedStock + '" />'
                + '<input type="hidden" class="InvoiceID" value="' + item.InvoiceID + '" />'
                + '<input type="hidden" class="PrimaryUnitID" value="' + item.PrimaryUnitID + '" />'
                + '<input type="hidden" class="PurchaseUnitID" value="' + item.PurchaseUnitID + '" />'
                + '<input type="hidden" class="clCgstPercentage" value="' + item.CGSTPercent + '" />'
                + '<input type="hidden" class="clSgstPercentage" value="' + item.SGSTPercent + '" />'
                + '<input type="hidden" class="clIgstPercentage" value="' + item.IGSTPercent + '" />'
                + '<input type="hidden" class="InvoiceTransID" value="' + item.InvoiceTransID + '" />'
                + '<input type="hidden" class="SecondaryUnitSize" value="' + item.SecondaryUnitSize + '" />'
                + "</td>"
                + '<td class="clItemCode">' + item.ItemCode + '</td >'
                + '<td class="clProduct">' + item.ItemName + '</td>'
                + '<td class="clPartsNumber">' + item.PartsNumber + '</td>'
                + '<td class="clRemarks">' + item.Remarks + '</td>'
                + '<td class="clModel">' + item.Model + '</td>'
                + '<td class="clUnit uk-hidden">' + item.Unit + '</td>'
                + '<td class="SecondaryUnit">' + item.SecondaryUnit + '</td>'
                + '<td class="clGrn">' + item.PurchaseNo + '</td>'
                + '<td class="clGrn">' + item.InvoiceNo + '</td>'
                + '<td class="clAcptQty mask-qty uk-hidden">' + item.InvoiceQty + '</td>'
                + '<td class="mask-qty">' + item.SecondaryInvoiceQty + '</td>'
                + '<td class="uk-hidden"><input type="text"  class="md-input mask-qty clReturnQty" value="' + item.InvoiceQty + '" /></td>'
                + '<td ><input type="text"  class="md-input mask-qty SecondaryReturnQty" value="' + item.SecondaryInvoiceQty + '" /></td>'
                + '<td class="clRate mask-currency uk-hidden">' + item.Rate + '</td>'
                + '<td class="SecondaryRate mask-currency">' + item.SecondaryRate + '</td>'
                + '<td class="clGrossAmount mask-currency">' + item.GrossAmount + '</td>'
                + '<td class="DiscountPercentage mask-qty">' + item.DiscountPercent + '</td>'
                + '<td class="Discount mask-qty">' + item.Discount + '</td>'
                + '<td class="clTaxableAmount mask-currency">' + item.TaxableAmount + ' </td>'
                + '<td class="VATPercentage mask-currency">' + item.VATPercentage + ' </td>'
                + '<td class="VATAmount mask-currency">' + item.VATAmount + ' </td>'
                + '<td class="clTotal mask-currency">' + item.NetAmount + ' </td>'
                + '</tr>';
        });

        $("#InvoiceNo").val(PurchaseInvoiceIDS.join(','));
        var $html = $(html);
        app.format($html);
        $tblPurchaseReturnItems.append($html);
    },
    Build_Select_Gst: function (options, selected_text) {
        var $select = '';
        var $select = $('<select> </select>');
        var $option = '';
        if (typeof selected_text == "undefined") {
            selected_text = "Select";
        }
        $option = '<option value="0">Select</option>';
        //$select.append($option);
        for (var i = 0; i < options.length; i++) {
            $option = '<option ' + ((selected_text == options[i].IGSTPercent) ? 'selected="selected"' : '') + ' value="' + options[i].ID + '">' + options[i].IGSTPercent + '</option>';

            $select.append($option);
        }
        return $select.html();

    },

    Build_Select: function (PrimaryUnitID, PrimaryUnit, PurchaseUnit, PurchaseUnitID, UnitID) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';

        if (UnitID == PurchaseUnitID) {
            $option += "<option selected value='" + PurchaseUnitID + "'>" + PurchaseUnit + "</option>";
        }
        else {
            $option += "<option  value='" + PurchaseUnitID + "'>" + PurchaseUnit + "</option>";

        }
        $select.append($option);
        if (UnitID == PrimaryUnitID) {
            $option += "<option selected value='" + PrimaryUnitID + "'>" + PrimaryUnit + "</option>";

        }
        else {
            $option += "<option value='" + PrimaryUnitID + "'>" + PrimaryUnit + "</option>";
        }

        $select.append($option);


        return $select.html();

    },

    count_items: function () {
        var count = $('#tblPurchaseReturnItems tbody tr.included').length;
        $('#item-count').val(count);
    },
    secondary_check_stock_quantity: function () {
        var self = purchase_return_order;
        var row = $(this).closest('tr');
        var qty = clean($(row).find('.SecondaryUnitSize').val()) * clean($(row).find('.SecondaryReturnQty').val());
        $(row).find('.clReturnQty').val(qty).trigger("keyup");
    },
    check_stock_quantity: function () {
        var self = purchase_return_order;
        var row = $(this).closest('tr');
        var stock = clean($(row).find('.Stock').val());
        var returnQty = clean($(row).find('.clReturnQty').val());
        var acceptedqty = clean($(row).find('.clAcptQty').val());
        var rate = clean($(row).find('.clRate ').val());
        var igstPercent = clean($(row).find('.clIgstPercentage').val());
        var cgstPercent = clean($(row).find('.clCgstPercentage').val());
        var sgstPercent = clean($(row).find('.clSgstPercentage').val());
        var unitID = clean($(row).find(".Unit option:selected").val());
        var primaryUnitID = clean($(row).find('.PrimaryUnitID').val());
        var purchaseUnitID = clean($(row).find('.PurchaseUnitID').val());
        var convertedStock = clean($(row).find('.ConvertedStock').val());
        var convertedQty = clean($(row).find('.ConvertedQty').val());
        var quantity = clean($(row).find('.Qty').val());
        var transUnitID = clean($(row).find('.UnitID').val());

        if (returnQty > acceptedqty) {
            app.show_error('Return quantity should be less than or equal to maximum return quantity ');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else {
            var IsGSTRegistred = $("#IsGSTRegistred").val();
            var StateID = $("#StateID").val();
            var ShippingStateID = $("#ShippingStateID").val();
            var Rate = clean($(row).find('.clRate').val());
            var qty = clean($(row).find('.clReturnQty').val());
            var GST = clean($(row).find('.GstPercentageID option:selected').text());
            var VATPercentage = clean($(row).find('.VATPercentage').text());
            var Discount = clean($(row).find('.Discount').val());
            var taxableAmt = (Rate * qty);
            var amount = (Rate * qty) - Discount;
            var VATAmount = (taxableAmt * VATPercentage / 100).roundTo(2);
            var gstamount = GST * amount / 100;
            var clSgstAmount = 0
            var clIgstAmount = 0
            if (IsGSTRegistred == false || IsGSTRegistred == "false") {
                gstamount = 0
                clSgstAmount = 0
                clIgstAmount = 0;
            } else {
                if (ShippingStateID == StateID) {
                    clSgstAmount = gstamount / 2
                    clIgstAmount = 0;
                }
                else {
                    clSgstAmount = 0;
                    clIgstAmount = gstamount;
                }

            }
            var totalamount = gstamount + amount;
            $(row).find('.clTotal').text(totalamount.toFixed(2))
            $(row).find('.gstamt').text(gstamount.toFixed(2))
            $(row).find('.clSgstAmount').text(clSgstAmount.toFixed(2))
            $(row).find('.clCgstAmount').text(clSgstAmount.toFixed(2))
            $(row).find('.clIgstAmount').text(clIgstAmount.toFixed(2))
            $(row).find('.TaxableAmount').text(taxableAmt.toFixed(2))
            $(row).find('.clGrossAmount').text(taxableAmt.toFixed(2)) 
            $(row).find('.VATAmount').text(VATAmount.toFixed(2)) 
            self.claculate_grid_total();
        }

    },

    claculate_grid_total: function () {
        var NetAmount = 0, GrossAmount = 0, Discounnt = 0, CGSTAmount = 0, IGSTAmount = 0, SGSTAmount = 0, VATAmount =0;
        $("#tblPurchaseReturnItems tbody tr.included").each(function () {
            var $row = $(this);
            CGSTAmount += clean($row.find('.clCgstAmount').text());
            IGSTAmount += clean($row.find('.clIgstAmount').text());
            SGSTAmount += clean($row.find('.clSgstAmount').text());
            GrossAmount += clean($row.find('.clGrossAmount').text());
            Discounnt += clean($row.find('.Discount').val());
            VATAmount += clean($row.find('.VATAmount').text());
        });
        var SuppDocAmount = clean($("#SuppDocAmount").val());
        var SuppShipAmount = clean($("#SuppShipAmount").val());
        var PackingForwarding = clean($("#PackingForwarding").val());
        var SupplierOtherCharges = clean($("#SupplierOtherCharges").val());
        var FreightAmount = clean($("#FreightAmount").val());
        NetAmount = GrossAmount - Discounnt + CGSTAmount + IGSTAmount + SGSTAmount + VATAmount + SuppDocAmount + SuppShipAmount + PackingForwarding + SupplierOtherCharges + FreightAmount;
        $("#IGSTAmount").val(IGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#GrossAmount").val(GrossAmount);
        $("#Discount").val(Discounnt);
        DiscounntPercentage = Discounnt / GrossAmount * 100;
        $("#DiscountPercentage").val(DiscounntPercentage);
        $("#txtNetAmount").val(NetAmount);
        

    },
  
    on_save: function () {

        var self = purchase_return_order;
        var model = self.get_data();
        var location = "/Purchase/PurchaseReturnOrder/Index";
        var url = '/Purchase/PurchaseReturnOrder/Create';

        if ($(this).hasClass("btnSaveAsDraft")) {
            model.IsDraft = true;
            url = '/Purchase/PurchaseReturnOrder/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            self.error_count = self.validate_form();
        }

        if (self.error_count > 0) {
            return;
        }

        if (!model.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save_and_new(model, url, location);
            }, function () {
            })
        } else {
            self.save_and_new(model, url, location);
        }
    },

    save_and_new: function (model, url, location) {
        var self = purchase_return_order;
        $(".btnSaveAndNew,.btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase return created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveAndNew,btnSaveAsDraft ").css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function (IsDraft) {
        self = purchase_return_order;
        var model = {
            ID: $("#ID").val(),
            GrossAmount: $("#GrossAmount").val(),
            CurrencyExchangeRate: $("#CurrencyExchangeRate").val(),
            CurrencyID: clean($("#CurrencyID").val()),
            ReturnNo: $("#txtReturnNo").val(),
            SupplierID: $("#SupplierID").val(),
            ReturnDate: $("#ReturnDate").val(),
            NetAmount: clean($("#txtNetAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            Discount: clean($("#Discount").val()),
            SGSTPercent: clean($("#SGSTPercentage").val()),
            IGSTPercent: clean($("#IGSTPercentage").val()),
            CGSTPercent: clean($("#CGSTPercentage").val()),
            IsDraft: IsDraft,
            Items: self.GetProductList(),
        };
        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var i = 0;
        $(".tbody tr.included").each(function () {
            i++;
            ProductsArray.push({
                ItemID: $(this).find('.ItemID').val(),
                InvoiceID: $(this).find('.InvoiceID').val(),
                InvoiceTransID: $(this).find('.InvoiceTransID').val(),
                SecondaryUnitSize: clean($(this).find('.SecondaryUnitSize').val()),
                SecondaryUnit: $(this).find('.SecondaryUnit').text().trim(),
                SecondaryReturnQty: clean($(this).find('.SecondaryReturnQty').val()),
                SecondaryRate: clean($(this).find('.SecondaryRate').text().trim()),
                Quantity: clean($(this).find('.clReturnQty').val()),
                OfferQty: clean($(this).find('.OfferReturnQty').val()),
                Rate: clean($(this).find('.clRate').text()),
                SGSTPercent: clean($(this).find('.clSgstPercentage').val()),
                CGSTPercent: clean($(this).find('.clCgstPercentage').val()),
                IGSTPercent: clean($(this).find('.clIgstPercentage').val()),
                SGSTAmt: clean($(this).find('.clSgstAmount').text()),
                CGSTAmt: clean($(this).find('.clCgstAmount').text()),
                IGSTAmt: clean($(this).find('.clIgstAmount').text()),
                Discount: clean($(this).find('.Discount').val()),
                DiscountPercentage: clean($(this).find('.DiscountPercentage').val()),
                Amount: clean($(this).find('.clTotal').text()),
                Remarks: $(this).find('.clRemarks').val(),
                WarehouseID: $(this).find('.WareHouseID').val(),
                BatchTypeID: $(this).find('.BatchTypeID').val(),
                UnitID: clean($(this).find('.UnitID').val()),
                GSTPercent: clean($(this).find('.GstPercentageID option:selected').text()),
                GSTAmount: clean($(this).find('.gstamt ').val()),
                VATPercentage: clean($(this).find('.VATPercentage').text()),
                VATAmount: clean($(this).find('.VATAmount').text()),
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
        $('#tblPurchaseReturnItems tbody').html('')
    },
    calculate_net_amount: function () {
        var packing_charge = clean($("#txtPackingCharges").val());
        var freight = clean($("#txtFreight").val());
        var other_charges = clean($("#txtOtherCharges").val());
        var total = packing_charge + freight + other_charges;
        $(".clTotal").each(function () {
            total += clean($(this).text());
        });
        $("#txtNetAmount").val(total);

    },

    set_supplier_details: function (event, item) {
        var self = purchase_return_order;
        if (($("#tblPurchaseReturnItems tbody tr").length > 0 && (item.id != $("#SupplierID").val()))) {
            app.confirm("Selected items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                $('#itempopup-list tbody').empty();
                $("#SupplierName").val(item.name);
                $("#SupplierLocation").val(item.location);
                $("#SupplierID").val(item.id);
                $("#StateID").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#SupplierReferenceNo").focus();
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(item.name);
            $("#SupplierLocation").val(item.location);
            $("#SupplierID").val(item.id);
            $("#StateID").val(item.stateId);
            $("#IsGSTRegistered").val(item.isGstRegistered);
            $("#SupplierReferenceNo").focus();
            self.get_invoice();
        }


    },
    get_invoice: function () {
        var self = purchase_return_order
        $('#invoice-popup-list tbody').empty();
        var total = 0;
        //   $("#txtNetAmount").val(total);
        self.error_count = 0;
        self.error_count = self.validate_select_invoice();
        if (self.error_count > 0) {
            return;
        }
        var SupplierID = $("#SupplierID").val();
        $("#InvoiceNo").val("");

        $.ajax({
            url: '/Purchase/PurchaseInvoice/GetPurchaseInvoiceList/',
            dataType: "json",
            type: "GET",
            data: {
                SupplierID: SupplierID,
            },
            success: function (response) {
                self.invoice_list_table.fnDestroy();
                var $invoice_list = $('#invoice-popup-list tbody');
                $invoice_list.html('');
                var tr = '';
                $.each(response, function (i, invoice) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center ' data-md-icheck><input type='checkbox'  class='CheckItem' value=" + invoice.Id + " /></td>"
                        + "<td>" + invoice.PurchaseNo + "</td>"
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
            url: '/Masters/Supplier/GetGRNWiseSupplierForAutoComplete',
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
    select_supplier: function (event) {

        var self = purchase_return_order;

        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        if ((($("#tblPurchaseReturnItems tbody tr").length > 0) && (ID != $("#SupplierID").val()))) {
            app.confirm("Selected items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                $("#SupplierName").val(Name);
                $("#SupplierLocation").val(Location);
                $("#SupplierID").val(ID);
                $("#StateID").val(StateID);
                $("#IsGSTRegistred").val(IsGSTRegistered);
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(Name);
            $("#SupplierLocation").val(Location);
            $("#SupplierID").val(ID);
            $("#StateID").val(StateID);
            $("#IsGSTRegistred").val(IsGSTRegistered);
            self.get_invoice();
        }
        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyName = $(row).find(".CurrencyName").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        var DecimalPlaces = clean($(row).find(".DecimalPlaces").val());
        DecPlaces = DecimalPlaces;
        gridcurrencyclass = app.change_decimalplaces($("#CurrencyExchangeRate"), DecimalPlaces);
        app.change_decimalplaces($("#Discount"), DecimalPlaces);
        app.change_decimalplaces($("#GrossAmount"), DecimalPlaces);
        app.change_decimalplaces($("#txtNetAmount"), DecimalPlaces);
        $("#DecimalPlaces").val(DecimalPlaces);
        $("#CurrencyID").val(CurrencyID);
        $("#CurrencyName").val(CurrencyName);
        $("#CurrencyCode").val(CurrencyCode);
        $("#CurrencyExchangeRate").val(CurrencyConversionRate);
        UIkit.modal($('#select-supplier')).hide();

        self.clearControls();
    },

    validate_form: function () {
        var self = purchase_return_order;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_select_invoice: function () {
        var self = purchase_return_order;
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
                            $('.included  .clReturnQty').each(function () {
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
                        }, message: 'Return qty should be less than or equal to maximum return quantity'
                    },
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
                elements: ".OfferReturnQty",
                rules: [
                    {
                        type: function (element) {
                            var error = false;
                            $('.OfferReturnQty').each(function () {
                                var row = $(this).closest('tr');
                                var returnofferqty = clean($(row).find('.OfferReturnQty').val());
                                var offerqty = clean($(row).find('.OfferQty ').val());
                                if (returnofferqty > offerqty)
                                    error = true;

                            });
                            return !error;
                        }, message: 'Offer return qty should be less than or equal to maximum offer quantity'
                    },
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
    }
}

Number.prototype.roundToCustom = function () {

    if (DecPlaces > 0 && DecPlaces <= 20) {
        return Number(this.toFixed(DecPlaces));
    } else {
        return Number(this.toFixed(4));
    }
};






