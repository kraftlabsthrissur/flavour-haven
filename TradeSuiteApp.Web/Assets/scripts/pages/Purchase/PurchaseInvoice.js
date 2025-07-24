var fh_item;
var fh_other;
var fh_tax;
purchase_invoice = {
    init: function () {
        var self = purchase_invoice;
        self.get_alopathy_grn();
        self.freeze_headers();
        self.bind_events();
        self.count_items();
        if ($("#SupplierID").val() == 0 || $("#SupplierID").val() == "") {
            supplier.supplier_list('not-intercompany-and-milk');
            $('#supplier-list').SelectTable({
                selectFunction: self.select_supplier,
                returnFocus: "#txtInvoiceNo",
                modal: "#select-supplier",
                initiatingElement: "#SupplierName",
                selectionType: "radio"
            });
        } else {
            //  $("#SGSTAmt").trigger('keyup');

            self.get_grn();
            // self.Calculate_invoice_Totals();

        }
        // self.Calculate_invoice_Totals();
        // $("#SGSTAmt").trigger('keyup');
        self.load_gst_dropDown();

    },
    details: function () {
        var self = purchase_invoice;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = purchase_invoice;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseInvoicePrintPdf',
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
    ExportItemCode: function () {
        var self = purchase_invoice;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseInvoiceExportIemCodePrintPdf',
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
    ItemCode: function () {
        var self = purchase_invoice;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseInvoiceIemCodePrintPdf',
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
    PartNo: function () {
        var self = purchase_invoice;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseInvoicePartNoPrintPdf',
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
    ExportPartNo: function () {
        var self = purchase_invoice;
        var id = $("#Id").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseInvoiceExportPartNoPrintPdf',
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

    bind_events: function () {
        var self = purchase_invoice;

        $.UIkit.autocomplete($('#grn-autocomplete'), { 'source': self.get_grn_autocomplete, 'minLength': 1 });
        $('#grn-autocomplete').on('selectitem.uk.autocomplete', self.set_grn_autocomplete);
        $("#btnOKSupplier").on('click', self.select_supplier);
        $("#btnOKGRN").on('click', self.select_grn);
        $("#btnOKMilk").on('click', self.select_grn_milk);
        $("#SupplierID").on('change', function () {
            $('#tbl-item-details tbody').html("");
        });

        //$('body').on('ifChanged', ".chkItem", self.include_item);
        $('.btnSaveAndNew').on('click', self.save_confirm);
        $('.btnSaveInvoice').on('click', self.save_confirm_invoice);
        $('.btnSaveAsDraft').on('click', self.SaveInvoice);
        $('.btnSaveAsDraftInvoice').on('click', self.SaveInvoiceData);
        $('.btnApprove').on('click', self.approve);
        $('.approve').on('click', self.confirm_approve);
        $(".cancel").on("click", self.cancel_confirm);
        $(".ExportItemCode").on("click", self.ExportItemCode);
        $(".ExportPartNo").on("click", self.ExportPartNo);
        $(".ItemCode").on("click", self.ItemCode);
        $(".PartNo").on("click", self.PartNo);



        // $('#txtDiscount, #txtDeductions').on('keyup', self.calculate_invoice_value);
        $('body').on('keyup', '.txtItemInvoiceRate, .txtItemInvoiceQty', self.calculate_item_diff_and_invoice_value);
        //$('body').on('keyup', '.txtDeductionInvoiceValue', self.calculate_other_charge_diff);
        // $('body').on('keyup', '.tax-po-value, .tax-invoice-value', self.calculate_tax_diff);
        // $('body').on('change', '.gstPercentage', self.on_gst_changed);
        // $("#TDSCode").on('change', self.calculate_tds);
        $("#txtInvoiceNo").on("change", self.get_invoice_number_count);
        // $('#tbl-item-details tbody tr .Batch').on('mouseover', self.get_batch_details);
        UIkit.uploadSelect($("#select-quotation"), self.selected_quotation_settings);
        $("body").on("mouseover", "#tbl-item-details tbody tr .Batch", self.get_batch_details);
        $('body').on('keyup', '.txtInvoiceRate, .DiscountAmount,#SGSTAmt,#CGSTAmt,#IGSTAmt', self.calculate_invoice_total);
        $("body").on("change", "#tbl-item-details tbody tr .gstPercentage", self.on_change_gst);
        $('#Discount, #SGSTAmt , #CGSTAmt , #IGSTAmt').on('keyup', self.calculate_total);

        $("#BusinessCategoryID").on('change', self.remove_all_items_from_grid);
    },

    remove_all_items_from_grid: function () {
        var self = purchase_invoice;
        $('#tbl-item-details tbody tr').empty();
        self.claculate_grid_total();
        self.calculate_total();
        self.get_alopathy_grn();
        self.count_items();
    },

    get_grn_autocomplete: function (release) {
        $.ajax({
            url: '/Purchase/GRN/GetUnProcessedGRNAutoComplete',
            data: {
                Hint: $('#GRNNo').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },

    set_grn_autocomplete: function (event, item) {  
        var self = purchase_invoice;
        event.preventDefault();
        if ($("#tbl-item-details tbody tr").length > 0) {
            app.confirm("Selected items will be removed", function () {
                self.clear_items();
                self.get_grn_items_for_allopathy_autocomplete(item);
            })
        }
        else {
            self.get_grn_items_for_allopathy_autocomplete(item);
        }
        return false;
    },

    get_grn_items_for_allopathy_autocomplete: function (item) {
        var self = purchase_invoice;
        var maxDate;
        var maxDateString;
        var CurrentDate;
        var grn_ids = [];
        var grnid = 0
        var gstTotal;
        //maxDate = self.get_date_time($("#Date").val());
        //maxDateString = $("#Date").val();
        //CurrentDate = self.get_date_time($("#Date").val());
        //    if (CurrentDate > maxDate) {
        //        maxDate = CurrentDate;
        //        maxDateString = $("#Date").val();
        //    }
        grn_ids = item.id;
        grnid = item.id;
        //$('#Date').val(maxDateString);
        $('#GRNNo').val(item.code);
        $('#SupplierName').val(item.supplierName);
        $("#SupplierID").val(item.supplierId);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        var GRN = $("#GRNNo").val();
        var SupplerName = $("#SupplierName").val();
        var SupplierID = clean($("#SupplierID").val());
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var StateID = clean($("#StateID").val());
        var ShippingStateID = $("#ShippingStateID").val();

        $.ajax({
            url: '/Purchase/PurchaseInvoice/GRNItemsForPurchaseInvoice',
            data: {
                grnIDS: grn_ids
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var container = $("<tbody>" + response + "</tbody>");
                //$("#tbl-item-details tbody").find(".hdnGRNTransID").each(function () {
                //    var GRNTransID = $(this).val();
                //    $(container).find(".hdnGRNTransID[value=" + GRNTransID + "]").closest('tr').remove();
                //})
                $('#tbl-item-details tbody').html('');
                $('#tbl-item-details tbody').append(container.html());
                $("#item-count").val(1);
                app.format($('#tbl-item-details'));
                setTimeout(function () { fh_item.resizeHeader(); }, 100);
                $('#tbl-item-details tbody tr').each(function () {
                    var row = $(this).closest('tr');
                    var igst = clean($(row).find('.IGSTPercent').val());
                    var Rate = clean($(row).find('.txtInvoiceRate').val());
                    var qty = clean($(row).find('.txtItemInvoiceQty').val());
                    var Discount = clean($(row).find('.DiscountAmount').val());
                    var total = (Rate * qty) - Discount;
                    var gstamount = igst * total / 100;
                    var totalamount = gstamount + total;
                    if (IsGSTRegistered == "false" || IsGSTRegistered == false) {
                        IGSTAmt = 0.00;
                        SGSTAmt = 0.00;
                        CGSTAmt = 0.00;
                        IGSTPercent = 0.00;
                        SGSTPercent = 0.00;
                        CGSTPercent = 0.00;
                    }
                    else {
                        if (ShippingStateID != StateID) {
                            IGSTAmt = igst * total / 100;
                            SGSTAmt = 0.00;
                            CGSTAmt = 0.00;
                            IGSTPercent = igst;
                            SGSTPercent = 0.00;
                            CGSTPercent = 0.00;
                        }
                            //else if (item.SGSTAmt > 0 && item.CGSTAmt > 0 && item.IGSTAmt == 0)
                        else {
                            IGSTPercent = 0.00;
                            SGSTPercent = igst / 2;
                            CGSTPercent = igst / 2;
                            IGSTAmt = 0.00;
                            SGSTAmt = SGSTPercent * total / 100;
                            CGSTAmt = CGSTPercent * total / 100;
                        }
                    }
                    //GSTPercent = '<select class="md-input label-fixed GstPercentageID">' + purchase_invoice.Build_Select_Gst(GSTList, igst) + '</select>'
                    $(row).find('.gstPercentage option:selected').text(igst);
                    $(row).find('.gstPercentage option:selected').val(igst);
                    $(row).find('.SGSTPercent').val(SGSTPercent);
                    $(row).find('.CGSTPercent').val(CGSTPercent);
                    $(row).find('.IGSTPercent').val(IGSTPercent);
                    $(row).find('.SGSTAmt').val(SGSTAmt);
                    $(row).find('.CGSTAmt').val(CGSTAmt);
                    $(row).find('.IGSTAmt').val(IGSTAmt);
                    $(row).find('.NetAmount').val(totalamount.toFixed(2))
                });

            }

        });
        self.get_grn_master(grnid);
    },

    calculate_total: function () {
        var self = purchase_invoice;
        var NetAmount = $("#txtAmountPayable").val();
        var GrossAmnt = $("#GrossAmount").val()
        var SGSTAmount = $("#SGSTAmt").val();
        var IGSTAmount = $("#IGSTAmt").val();
        var CGSTAmount = $("#CGSTAmt").val();
        var discount = clean($('#Discount').val());
        GrossAmnt = GrossAmnt - (discount);
        var GSTAmount = clean($('#SGSTAmt').val()) + clean($('#CGSTAmt').val()) + clean($('#IGSTAmt').val());
        NetAmount = GrossAmnt + GSTAmount;
        var temp = netamount;
        netamount = Math.round(netamount);
        roundoff = temp - netamount;
        $("#txtAmountPayable").val(netamount);
        $("#txtDeductions").val(roundoff);
    },

    on_change_gst: function () {
        var self = purchase_invoice;
        var row = $(this).closest('tr');
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var StateID = $("#StateID").val();
        var ShippingStateID = $("#ShippingStateID").val();
        var Rate = clean($(row).find('.txtInvoiceRate').val());
        var qty = clean($(row).find('.txtItemInvoiceQty').val());
        var GST = clean($(row).find('.gstPercentage option:selected').text());
        var Discount = clean($(row).find('.DiscountAmount').val());
        var amount = (Rate * qty) - Discount;

        var gstamount = GST * amount / 100;
        var clSgstAmount = 0
        var clIgstAmount = 0
        if (IsGSTRegistered == "false") {
            SGSTAmt = 0;
            CGSTAmt = 0;
            IGSTAmt = 0;
        } else {
            if (ShippingStateID == StateID) {
                SGSTAmt = gstamount / 2;
                CGSTAmt = gstamount / 2;
                IGSTAmt = 0;
            }
            else {
                SGSTAmt = 0;
                CGSTAmt = 0;
                IGSTAmt = gstamount;
            }

        }
        var totalamount = gstamount + amount;
        $(row).find('.NetAmount').val(totalamount.toFixed(2))
        $(row).find('.SGSTPercent').val(SGSTPercent);
        $(row).find('.CGSTPercent').val(CGSTPercent);
        $(row).find('.IGSTPercent').val(IGSTPercent);
        $(row).find('.SGSTAmt').val(SGSTAmt);
        $(row).find('.CGSTAmt').val(CGSTAmt);
        $(row).find('.IGSTAmt').val(IGSTAmt);
        self.claculate_grid_total();
        var PrevoiusBatchNetProfitRatio = $(row).find('.PrevoiusBatchNetProfitRatio').val();

        var BatchID = 0, ItemID = 0, currentprofittolerance = 0;
        var profit = 0;
        var p = "";
        var PurchaseMRP = clean($(this).closest('tr').find('.PurchaseMRP').val());
        var RetailMRP = clean($(this).closest('tr').find('.RetailMRP').val());
        var GrossAmount = clean($(this).closest('tr').find('.txtItemInvoiceValue').val());
        var ProfitRatio = 0;
        var DiscountAmt = clean($(this).closest('tr').find('.DiscountAmount').val());
        var ReceivedQty = clean($(this).closest('tr').find('.txtItemInvoiceQty').val());
        var OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
        var GST = clean($(this).closest('tr').find('.gstPercentage').val());
        var CessPercent = clean($(this).closest('tr').find('.CessPercentage').val());
        var invoicepriceprice = clean($(this).closest('tr').find('.txtInvoiceRate ').val());
        var purchaseprice = GrossAmount - DiscountAmt;
        //  var purchasevalue = (purchaseprice * ReceivedQty).roundTo(2);
        var NetPurchasePrice = ((purchaseprice) / (ReceivedQty + OfferQty)).roundTo(2);
        var Salesrate = (RetailMRP * (100 / (100 + GST + CessPercent))).roundTo(2);
        var CurrentProfitTolerance = (((Salesrate - NetPurchasePrice) / Salesrate) * 100).roundTo(2);

        if (CurrentProfitTolerance>=PrevoiusBatchNetProfitRatio)
        {
            
            $(this).closest('tr').removeClass("insufficient-stock")
        }
        else
        {
            $(this).closest('tr').addClass("insufficient-stock")
        }

    },

    claculate_grid_total: function () {
        var self = purchase_invoice;
        var netamount = 0, gst, igst, cgst, sgst, total, discount, grossamount;
        var netdiscount = 0;
        var netSgstAmount = 0;
        var netCgstAmount = 0;
        var netIgstAmount = 0;
        var netGrossAmount = 0;
        $("#tbl-item-details tbody tr").each(function () {

            total = clean(($(this)).find('.NetAmount').val());
            sgst = clean($(this).find('.SGSTAmt').val());
            cgst = clean($(this).find('.CGSTAmt').val());
            igst = clean($(this).find('.IGSTAmt').val());
            discount = clean($(this).find('.DiscountAmount').val());
            grossamount = clean($(this).find('.txtItemInvoiceValue').val());
            netamount += total;
            netSgstAmount += sgst;
            netCgstAmount += cgst;
            netIgstAmount += igst;
            netdiscount += discount;
            netGrossAmount += grossamount;
        })
        var temp = netamount;
        netamount = Math.round(netamount);
        roundoff = temp - netamount;
        $("#txtAmountPayable").val(netamount);
        $("#txtDeductions").val(roundoff);
        $("#SGSTAmt").val((netSgstAmount));
        $("#CGSTAmt").val((netCgstAmount));
        $("#IGSTAmt").val(netIgstAmount);
        $("#Discount").val(netdiscount);
        $("#GrossAmount").val(netGrossAmount);
    },

    get_batch_details: function () {
        var self = purchase_invoice;
        var row = $(this).closest('tr');
        var BatchID = 0, ItemID = 0, currentprofittolerance = 0;
        var profit = 0;
        var p = "";
        BatchID = clean($(this).closest('tr').find('.BatchID').val());
        var PurchaseMRP = clean($(this).closest('tr').find('.PurchaseMRP').val());
        var RetailMRP = clean($(this).closest('tr').find('.RetailMRP').val());
        var GrossAmount = clean($(this).closest('tr').find('.txtItemInvoiceValue').val());
        var ProfitRatio = 0;
        var DiscountAmt = clean($(this).closest('tr').find('.DiscountAmount').val());
        var ReceivedQty = clean($(this).closest('tr').find('.txtItemInvoiceQty').val());
        var OfferQty = clean($(this).closest('tr').find('.OfferQty').val());
        var GST = clean($(this).closest('tr').find('.gstPercentage').val());
        var CessPercent = clean($(this).closest('tr').find('.CessPercentage').val());
        var invoicepriceprice = clean($(this).closest('tr').find('.txtInvoiceRate ').val());
        var UnitID = clean($(this).closest('tr').find('.UnitID').val());
        var Unit = $(this).closest('tr').find('.Unit').text();
        // var NetPurchasePrice = (GrossAmount - DiscountAmt) / (ReceivedQty + OfferQty);
        //var CurrentBatchNetProfit = ((RetailMRP - NetPurchasePrice) / RetailMRP) * 100;
        var purchaseprice = GrossAmount - DiscountAmt;
        //  var purchasevalue = (purchaseprice * ReceivedQty).roundTo(2);
        var NetPurchasePrice = ((purchaseprice) / (ReceivedQty + OfferQty)).roundTo(2);
        var Salesrate = (RetailMRP * (100 / (100 + GST + CessPercent))).roundTo(2);
        var CurrentBatchNetProfit = (((Salesrate - NetPurchasePrice) / Salesrate) * 100).roundTo(2);
        var SupplierName = $("#SupplierName").val();
        var ItemName = $(this).closest('tr').find('.ItemName').text();

        $(".Descriptiondetails").html('');
        $.ajax({
            url: '/Masters/Batch/GetPreviousBatchDetails',
            data: {
                BatchID: BatchID,
                CurrentBatchNetProfit: CurrentBatchNetProfit,
                RetailMRP: RetailMRP,
                PurchasePrice: NetPurchasePrice,
                DiscountAmt: DiscountAmt,
                OfferQty: OfferQty,
                ReceivedQty: ReceivedQty,
                SupplierName: SupplierName,
                ItemName: ItemName,
                Unit: Unit,
                UnitID: UnitID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $p = $(response);
                app.format($p);
                $('.Descriptiondetails').append($p);
                ProfitRatio = clean($(".profit").text());
                if (ProfitRatio <= 0) {
                    $(".profit").addClass('description-loose');
                    $(".batch-description-div").removeClass("uk-hidden");
                    $("#style_switcher_horizontal").animate("slow");
                    var $switcher = $('#style_switcher_horizontal');
                    $switcher.addClass('switcher_active');

                }
                else {
                    $(".batch-description-div").addClass("uk-hidden");
                    $(".Descriptiondetails").html('');
                }


                //    currentprofittolerance = (CurrentBatchNetProfit * response.data.ProfitTolerance) / 100;
                //    profit = ((CurrentBatchNetProfit + currentprofittolerance) - response.data.ProfitRatio).roundTo(2);
                //    var classname = profit >= 0 ? 'description-profit' : 'description-loose';                   

                //    var date = response.data.PO == "-" ? "-" : response.data.PODate;
                //    p += '<div class="uk-form-row">'
                //             + '<div class="uk-grid" data-uk-grid-margin="">'
                //                 + '<div class="uk-width-medium-4-8 description-name">'
                //                     + '<label >Previous data </label>'
                //                     + '  </div></div></div>'
                //                 +- '<div class="uk-form-row">'
                //                     + '<div class="uk-grid" data-uk-grid-margin="">'
                //                 + '<div class="uk-width-medium-1-8">'
                //                    + ' <label>Invoice No </label>'
                //                    + '<input type="text" class="md-input label-fixed  "  value=' + response.data.PO + ' disabled />'
                //                    + '       </div>'
                //                 + ' <div class="uk-width-medium-1-8">'
                //                    + ' <label>Invoice Date</label>'
                //                    + '<input type="text" class="md-input label-fixed  "  value=' + date + ' disabled />'
                //                    + '       </div>'

                //                + ' <div class="uk-width-medium-2-8">'
                //                     + '<label>Supplier Name</label>'
                //                     + '<input type="text" class="md-input "  value=' + response.data.SupplierName + ' disabled />'
                //                     + '       </div>'
                //                + ' <div class="uk-width-medium-1-8">'
                //                     + '<label>Quantity</label>'
                //                     + '<input type="text" class="md-input   mask-currency"  value=' + response.data.Quantity + ' disabled />'
                //                     + '       </div>'
                //                 + '<div class="uk-width-medium-1-8">'
                //                     + '<label>Offer Quantity </label>'
                //                     + '<input type="text" class="md-input   mask-currency"  value=' + response.data.OfferQty + ' disabled />'
                //                     + '       </div>'
                //                 + '<div class="uk-width-medium-1-8">'
                //                     + '<label>Purchase Price</label>'
                //                     + '<input type="text" class="md-input   mask-currency"  value=' + response.data.BatchRate + ' disabled />'
                //                     + '       </div>'
                //                       + ' </div></div>'
                //                        + '<div class="uk-form-row">'
                //                             + '<div class="uk-grid" data-uk-grid-margin="">'
                //                 + '<div class="uk-width-medium-1-1 description-price">'
                //                   +'<br/>'
                //                      + '<label class="mask-currency bold ">Previous batch profit ratio ' + response.data.ProfitRatio + ' </label>'
                //                       + '       </div>'
                //                 + ' </div></div>'
                //                            + '<div class="uk-form-row">'
                //                             + '<div class="uk-grid" data-uk-grid-margin="">'
                //                            + '<div class="uk-width-medium-4-8 description-name">'
                //                                +  '<label >Current data </label>'
                //                                + '  </div></div></div>'
                //                            + '<div class="uk-form-row">'
                //                                + '<div class="uk-grid" data-uk-grid-margin="">'
                //                            + '<div class="uk-width-medium-2-8">'
                //                               + ' <label>Current Purchase Price</label>'
                //                               + '<input type="text" class="md-input label-fixed  mask-currency"  value=' + NetPurchasePrice + ' disabled />'
                //                               + '       </div>'
                //                            + ' <div class="uk-width-medium-2-8">'
                //                               + ' <label>Current Batch MRP</label>'
                //                               + '<input type="text" class="md-input label-fixed  mask-currency"  value=' + RetailMRP + ' disabled />'
                //                               + '       </div>'
                //                               + ' </div></div>'

                //                                + '<div class="uk-form-row">'
                //                                + '<div class="uk-grid  " data-uk-grid-margin="">'
                //                                + ' <div class="uk-width-medium-1-1 description-price">'
                //                                + '<br/>'
                //                                 + '<label class="mask-currency bold">Current batch profit ratio ' + CurrentBatchNetProfit.roundTo(2) + ' </label>'
                //                                + '       </div>'
                //                                + '  </div></div>'

                //                          +'<div class="uk-form-row">'
                //                          + '<div class="uk-grid  " data-uk-grid-margin="">'
                //                          + '<div class="uk-width-medium-4-8 ' + classname + '">'
                //                          + '<label class="mask-currency">' + profit.roundTo(2) + ' </label>'
                //                          + '  </div></div></div>';




                //           $("#style_switcher_horizontal").animate("slow");
                //           var $switcher = $('#style_switcher_horizontal');
                //           $switcher.addClass('switcher_active');
                //           $p = $(p);
                //           app.format($p);
                //           if (profit <= 0) {
                //               $(".Descriptiondetails").html('');
                //               $('.Descriptiondetails').append($p);
                // $(".batch-description-div").removeClass("uk-hidden");

                //           }
                //           else {
                //               $(".Descriptiondetails").html('');
                //              $(".batch-description-div").addClass("uk-hidden");
                //           }
            }

        });



    },
    cancel_confirm: function () {
        var self = purchase_invoice
        app.confirm_cancel("Do you want to cancel? This can't be undone", function () {
            self.cancel();
        }, function () {
        })
    },

    save_confirm: function () {
        var self = purchase_invoice
        app.confirm_cancel("Do you want to save ?", function () {
            self.SaveInvoice();
        }, function () {
        })
    },
    save_confirm_invoice: function () {
        var self = purchase_invoice
        app.confirm_cancel("Do you want to save ?", function () {
            self.SaveInvoiceData();
        }, function () {
        })
    },
    confirm_approve: function () {
        var self = purchase_invoice
        app.confirm_cancel("Do you want to approve ? once approved items with less profit also gets saved", function () {
            self.SaveApproveData();
        }, function () {
        })
    },
    get_invoice_number_count: function (release) {

        $.ajax({
            url: '/Purchase/PurchaseInvoice/GetInvoiceNumberCount',
            data: {
                Hint: $("#txtInvoiceNo").val(),
                Table: "PurchaseInvoice",
                SupplierID: $('#SupplierID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                count = response.data;
                $("#invoice-count").val(count);
            }
        });
    },

    calculate_tds: function () {
        var self = purchase_invoice;
        code = $("#TDSCode").val();
        var percentage = code.split("#");
        var amount = clean($('#txtTotalFreightCalculated').val());
        var tdsAmt = clean(self.getCalculatedTDSAmt(amount, clean(percentage[1])));
        var tdsID = clean(percentage[0]);
        $('#TDSID').val(tdsID);
        if (tdsAmt != 0) {

            tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) != 0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
        }
        $('#txtTDSOnFreight').val(tdsAmt);
    },
    getCalculatedTDSAmt: function (amt, percentage) {

        if (amt == null || amt == undefined)
            amt = '0';
        if (amt == '' || amt.length <= 0)
        { amt = '0' }
        if (percentage != '') {
            return ((parseFloat(amt) / parseFloat(100)) * parseFloat(percentage)).toFixed(2);
        }

    },
    freeze_headers: function () {
        fh_item = $("#tbl-item-details").FreezeHeader();
        fh_other = $("#tbl-other-charges").FreezeHeader();
        fh_tax = $("#tbl-tax-details").FreezeHeader();
        $('[data-uk-tab]').on('show.uk.switcher', function (event, area) {
            fh_item.resizeHeader();

        });
    },
    CalculateNetValue: function () {
        Calculate_Net_Value_With_Discount_And_All();
    },

    calculate_invoice_value: function () {                               //Calculate Total Invoice. When any changes in TDSOnFreight, LessTDS, Discount, Deductions, TotalInvoice
        var self = purchase_invoice;
        var totalInvoice = clean($('#DummyInvoiceTotal').val());
        var deductions = clean($('#txtDeductions').val());

        var discount = clean($('#txtDiscount').val());
        if (discount < 0) {
            app.show_error("Please enter positive discount");
        }
        else {
            CalculateInvoiceTotal();
            self.calculate_total();
        }

    },


    calculate_total: function () {
        var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
        var tdsOnFreight = clean($('#txtTDSOnFreight').val());
        var lessTDS = clean($('#txtLessTDS').val());
        var discount = clean($('#txtDiscount').val());
        var deductions = clean($('#txtDeductions').val());
        //var amtPayable = totalInvoice - (tdsOnFreight + lessTDS + deductions);
        var amtPayable = totalInvoice;
        $('#txtAmountPayable').val(amtPayable);
    },
    approve: function () {
        $(".btnApprove").css({ 'display': 'none' });

        $.ajax({
            url: '/Purchase/PurchaseInvoice/Approve',
            data: {
                ID: $("#hdnPurchaseInvoiceId").val(),
                Status: "Approved",
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Purchase invoice approved successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to approve purchase invoice");
                    $(".btnApprove").css({ 'display': 'block' });

                }
            },
        });
    },
    calculate_tax_diff: function () {
        $currtr = $(this).parents('tr');
        var poValue = clean($currtr.find('.tax-po-value').val());
        var currVal = clean($currtr.find('.tax-invoice-value').val());
        $currtr.find('.tax-diff-value').val(currVal - poValue);
        CalculateTotals();
    },
    calculate_other_charge_diff: function () {
        $currtr = $(this).parents('tr');
        var poValue = parseFloat($currtr.find('.deductionPOValue').text().replace(/\,/g, ''));
        var currVal = parseFloat($(this).val().replace(/\,/g, ''));
        $currtr.find('.deductionDiffValue').val(currVal - poValue);
        CalculateTotals();
    },
    calculate_item_diff_and_invoice_value: function () {

        $currtr = $(this).closest('tr');
        var invQty = parseFloat($currtr.find('.txtItemInvoiceQty').val().replace(/\,/g, ''));
        var invRate = parseFloat($currtr.find('.txtItemInvoiceRate').val().replace(/\,/g, ''));
        var poRate = parseFloat($currtr.find('.itemPORate').text().replace(/\,/g, ''));
        var approvedvalue = parseFloat($currtr.find('.itemApprovedValue ').text().replace(/\,/g, ''));
        var invValue = invQty * invRate;
        $currtr.find('.txtItemInvoiceValue').val(invValue);
        $currtr.find('.itemDiffValue').val(invValue - approvedvalue);
        CalculateTotals();
    },
    calculate_invoice_total: function () {
        var self = purchase_invoice;
        $currtr = $(this).closest('tr');
        var invQty = clean($currtr.find('.txtItemInvoiceQty').val());
        var invRate = clean($currtr.find('.txtInvoiceRate').val());
        var poRate = clean($currtr.find('.itemPORate').text());
        var discount = clean($currtr.find('.DiscountAmount').val());
        var approvedvalue = clean($currtr.find('.itemApprovedValue ').text());
        var invValue = invQty * invRate;
        var netamount = invValue - discount;
        $currtr.find('.txtItemInvoiceValue').val(invValue);

        $currtr.find('.NetAmount').val(netamount);
        $currtr.find('.itemDiffValue').val(invValue - approvedvalue);
        self.Calculate_invoice_Totals();
        fh_item.resizeHeader();
    },
    Calculate_invoice_Totals: function () {
        var currVal = 0, totalInvoice = 0, discount = 0, grossamount = 0, row, netamount = 0, roundoff = 0;
        var Igst = clean($("#IGSTAmt").val());
        var Sgst = clean($("#SGSTAmt").val());
        var Cgst = clean($("#CGSTAmt").val());
        $('#item-details-tblContainer table tr td .chkItem:checked').parents('tr').find('.NetAmount').each(function () {
            currVal = clean($(this).val());
            totalInvoice += currVal;
            row = $(this).closest('tr');
            discount += clean($(row).find('.DiscountAmount').val());
            grossamount += clean($(row).find('.txtItemInvoiceValue').val());
        });
        $("#txtDiscount").val(discount);
        $("#GrossAmt").val(grossamount);
        netamount = grossamount - discount + Igst + Sgst + Cgst;
        var temp = netamount;
        netamount = Math.round(netamount);
        roundoff = temp - netamount;
        $("#txtAmountPayable").val(netamount);
        $("#txtDeductions").val(roundoff);
    },

    include_item: function () {
        var self = purchase_invoice;
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('readonly');
        self.count_items();
        self.set_other_charges();
        self.set_tax_details();
        CalculateTotals();
    },
    on_gst_changed: function () {
        var self = purchase_invoice;
        self.set_tax_details();
        CalculateTotals();
    },
    count_items: function () {
        var count = $('#tbl-item-details tbody').find('input.chkItem:checked').length;
        $('#item-count').val(count);
    },
    set_tax_details: function () {
        var self = purchase_invoice;
        var tax_details = [];
        var index;
        var SGST = 0;
        var CGST = 0;
        var IGST = 0;
        var GST = 0;
        var IsInclusiveGST = 0;
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var tr = "";
        var $tr;
        var j = 1;
        var tax_percentages = $('.tax-template').html();
        var inclusive_gst = "";
        var GSTChanges = [];
        var isIGST = true;
        if ($("#StateID").val() == $("#ShippingStateID").val()) {
            isIGST = false;
        }

        $("#tbl-item-details input.chkItem:checked").each(function () {
            $row = $(this).closest("tr");
            SGST = clean($row.find(".SGSTPercent").val());
            CGST = clean($row.find(".CGSTPercent").val());
            IGST = clean($row.find(".IGSTPercent").val());
            GST = clean($row.find(".gstPercentage").val());
            if (IGST + CGST + SGST != GST && self.is_gst_in_array(GSTChanges, GST) == -1) {
                GSTChanges.push(GST);
            }
            accepted_value = IsGSTRegistered.toLowerCase() == "true" ? clean($row.find(".itemApprovedValue").val()) : 0;
            if (isIGST) {
                tax_details = self.calculate_group_tax_details(tax_details, IGST, "IGST", accepted_value, IsInclusiveGST);
            } else {
                tax_details = self.calculate_group_tax_details(tax_details, SGST, "SGST", accepted_value, IsInclusiveGST);
                tax_details = self.calculate_group_tax_details(tax_details, CGST, "CGST", accepted_value, IsInclusiveGST);
            }
        });

        for (var i = 0; i < GSTChanges.length; i++) {
            if (isIGST) {
                tax_details = self.calculate_group_tax_details(tax_details, GSTChanges[i], "IGST", 0, IsInclusiveGST);
            } else {
                tax_details = self.calculate_group_tax_details(tax_details, GSTChanges[i] / 2, "SGST", 0, IsInclusiveGST);
                tax_details = self.calculate_group_tax_details(tax_details, GSTChanges[i] / 2, "CGST", 0, IsInclusiveGST);
            }
        }

        $("#tbl-tax-details tbody").find(".gst").remove();
        $.each(tax_details.sort((a, b) => (a.tax_percentage > b.tax_percentage) ? 1 : 0), function (i, record) {

            inclusive_gst = "gst-extra";

            tr += "<tr class='gst " + record.type + "'>"
                + "<td class='serial-no uk-text-center'>" + (j++) + "</td>"
                + "<td>" + record.particular + "<input type='hidden' class='tax-particular' value='" + record.type + "'></td>"
                + "<td><input type='text' readonly = 'readonly' class='md-input tax-percentage mask-currency' value='" + record.tax_percentage + "' /></td>"
                + "<td><input type='text' readonly = 'readonly' class='md-input tax-po-value mask-currency' value='" + record.tax_amount + "' /></td>"
                + "<td><input type='text' class='md-input tax-invoice-value mask-currency " + inclusive_gst + "' value='0' /></td>"
                + "<td><input type='text' disabled='disabled' class='md-input tax-diff-value mask-currency' value='" + (-record.tax_amount) + "' /></td>"
                + "<td><input type='text' class='md-input tax-remarks' value=''></td>"
                + "</tr>";
        });
        if ($("#txtTaxOnFreightInvoiceValue").length == 0) {
            tr += "<tr>"
               + "<td  class='serial-no uk-text-center'>" + (j++) + "</td>"
               + "<td>Tax On Freight<input type='hidden' class='tax-particular' value='Tax On Freight'></td>"
               + "<td><select class='md-input tax-percentage' >" + tax_percentages + "</select></td>"
               + "<td><input type='text' class='md-input tax-po-value mask-currency' value='0' /></td>"
               + "<td><input type='text' class='md-input tax-invoice-value mask-currency gst-extra'  id='txtTaxOnFreightInvoiceValue' value='0' /></td>"
               + "<td><input type='text' disabled='disabled' class='md-input tax-diff-value mask-currency' value='0'  /></td>"
               + "<td><input type='text' class='md-input tax-remarks ' value=''></td>"
               + "</tr>";
        }
        if ($("#txtTaxOnOtherChargeInvoiceValue").length == 0) {
            tr += "<tr>"
                + "<td  class='serial-no uk-text-center'>" + (j++) + "</td>"
                + "<td>Tax On Other Charges<input type='hidden' class='tax-particular' value='Tax On Other Charges'></td>"
                + "<td><select class='md-input tax-percentage' >" + tax_percentages + "</select></td>"
                + "<td><input type='text' class='md-input tax-po-value mask-currency' value='0' /></td>"
                + "<td><input type='text' class='md-input tax-invoice-value mask-currency gst-extra'  id='txtTaxOnOtherChargeInvoiceValue' value='0' /></td>"
                + "<td><input type='text' disabled='disabled' class='md-input tax-diff-value mask-currency' value='0' /></td>"
                + "<td><input type='text' class='md-input tax-remarks ' value=''></td>"
                + "</tr>";
        }
        if ($("#txtTaxOnPackingChargeInvoiceValue").length == 0) {
            tr += "<tr>"
                   + "<td  class='serial-no uk-text-center'>" + (j++) + "</td>"
                   + "<td>Tax On Packing Charges<input type='hidden' class='tax-particular' value='Tax On Packing Charges'></td>"
                   + "<td><select class='md-input tax-percentage' >" + tax_percentages + "</select></td>"
                   + "<td><input type='text' class='md-input tax-po-value mask-currency' value='0' /></td>"
                   + "<td><input type='text' class='md-input tax-invoice-value mask-currency gst-extra'  id='txtTaxOnPackingChargeInvoiceValue' value='0' /></td>"
                   + "<td><input type='text' disabled='disabled' class='md-input tax-diff-value mask-currency' value='0'  /></td>"
                   + "<td><input type='text' class='md-input tax-remarks ' value=''></td>"
                   + "</tr>";
        }
        $tr = $(tr);
        app.format($tr);
        $("#tbl-tax-details tbody").prepend($tr);
        $('#tbl-tax-details .serial-no').each(function (i) { $(this).text(i + 1) });
        fh_tax.resizeHeader();
    },

    calculate_group_tax_details: function (tax_details, GST, type, accepted_value, IsInclusiveGST) {
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        if (!IsGSTRegistered) {
            return;
        }
        var tax_amount;
        var self = purchase_invoice;
        //if (GST != 0) {
        index = self.search_tax_group(tax_details, type, GST);
        if (type != "IGST") {
            GST = GST * 2;
        }
        if (index !== -1) {
            tax_details[index].accepted_value += accepted_value;
            if (IsInclusiveGST == 1) {
                tax_amount = tax_details[index].accepted_value - (tax_details[index].accepted_value * 100 / (GST + 100));
            } else {
                tax_amount = tax_details[index].accepted_value * GST / 100;
            }
            if (type == "IGST") {
                tax_details[index].tax_amount = tax_amount;
            } else {
                tax_details[index].tax_amount = tax_amount / 2;
            }

        } else {
            if (IsInclusiveGST == 1) {
                tax_amount = accepted_value - (accepted_value * 100 / (GST + 100));
            } else {
                tax_amount = accepted_value * GST / 100;
            }
            if (type != "IGST") {
                tax_amount = tax_amount / 2;
                GST = GST / 2;
            }
            tax_details.push(
                        {
                            particular: type + " @ " + GST + "%",
                            accepted_value: accepted_value,
                            tax_amount: tax_amount,
                            type: type,
                            tax_percentage: GST
                        }
            );
        }
        //}
        return tax_details;
    },

    set_other_charges: function () {
        var self = purchase_invoice;
        var PurchaseOrderNo;
        var PurchaseOrderID;
        var FreightAmt;
        var OtherCharges;
        var PackingShippingCharge;
        var row;
        var other_charge_details = [];
        var obj = {};
        var tr;
        var $tr;
        var j = 1;
        $('#tbl-item-details tbody').find("input.chkItem:checked").each(function () {
            var row = $(this).closest("tr");
            PurchaseOrderNo = $(row).find('.PurchaseOrderNo').val();
            if (self.search_in_array(other_charge_details, PurchaseOrderNo) == -1) {
                PurchaseOrderID = $(row).find('.hdnPOID').val();
                FreightAmt = $(row).find('.FreightAmt').val();
                OtherCharges = $(row).find('.OtherCharges').val();
                PackingShippingCharge = $(row).find('.PackingShippingCharge').val();
                obj = {
                    PurchaseOrderNo: PurchaseOrderNo,
                    PurchaseOrderID: PurchaseOrderID,
                    FreightAmt: FreightAmt,
                    OtherCharges: OtherCharges,
                    PackingShippingCharge: PackingShippingCharge,
                }
                other_charge_details.push(obj);
            }
        });
        $("#tbl-other-charges tbody").html("");

        $.each(other_charge_details, function (i, record) {
            tr = '<tr>'
                    + '<td class="uk-text-center">' + (j++)
                    + '<input type="hidden" class="hdnPurchaseOrderID" value="' + record.PurchaseOrderID + '" />'
                    + '</td>'
                    + '<td class="deductionName">Freight Amount</td>'
                    + '<td>' + record.PurchaseOrderNo + '</td>'
                    + '<td class="uk-text-right freightPOValue deductionPOValue mask-currency">' + record.FreightAmt + '</td>'
                    + '<td><input type="text" class="md-input label-fixed txtFreightInvoiceValue txtDeductionInvoiceValue mask-currency" value="0" /></td>'
                    + '<td><input type="text" disabled="disabled" class="md-input freightDiff deductionDiffValue mask-currency" value="' + (-record.FreightAmt) + '" /></td>'
                    + '<td><input type="text" class="md-input label-fixed label-fixed txtFreightRemarks txtDeductionRemarks" /></td>'
                + '</tr>';
            tr += '<tr>'
                  + '<td class="uk-text-center">' + (j++)
                  + '<input type="hidden" class="hdnPurchaseOrderID" value="' + record.PurchaseOrderID + '" />'
                  + '</td>'
                  + '<td class="deductionName">Other Charges</td>'
                  + '<td>' + record.PurchaseOrderNo + '</td>'
                  + '<td class="uk-text-right otherChargePOValue deductionPOValue mask-currency">' + record.OtherCharges + '</td>'
                  + '<td><input type="text" class="md-input label-fixed txtOtherChargeInvoiceValue txtDeductionInvoiceValue mask-currency" value="0" /></td>'
                  + '<td><input type="text" disabled="disabled" class="md-input otherChargeDiff deductionDiffValue mask-currency" value="' + (-record.OtherCharges) + '" /></td>'
                  + '<td><input type="text" class="md-input label-fixed txtOtherDeductionRemarks txtDeductionRemarks" /></td>'
              + '</tr>';
            tr += '<tr>'
                 + '<td class="uk-text-center">' + (j++)
                 + '<input type="hidden" class="hdnPurchaseOrderID" value="' + record.PurchaseOrderID + '" />'
                 + '</td>'
                 + '<td class="deductionName">Packing/Shipping Charges</td>'
                 + '<td>' + record.PurchaseOrderNo + '</td>'
                 + '<td class="uk-text-right packingChargePOValue deductionPOValue mask-currency">' + record.PackingShippingCharge + '</td>'
                 + '<td><input type="text" class="md-input label-fixed txtPackingChargeInvoiceValue txtDeductionInvoiceValue mask-currency" value="0" /></td>'
                 + '<td><input type="text" disabled="disabled" class="md-input packingChargeDiff deductionDiffValue mask-currency" value="' + (-record.PackingShippingCharge) + '" /></td>'
                 + '<td><input type="text" class="md-input label-fixed label-fixed txtPackingChargeRemarks txtDeductionRemarks" /></td>'
             + '</tr>'

            $tr = $(tr);
            app.format($tr);
            $("#tbl-other-charges tbody").append($tr);
        });
        fh_other.resizeHeader();
    },

    search_in_array: function (array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].PurchaseOrderNo == value) {
                return i;
            }
        }
        return -1;
    },

    cancel: function () {
        $(".btnSaveAndNew,.btnSaveAsDraft,.cancel").css({ 'display': 'none' });

        $.ajax({
            url: '/Purchase/PurchaseInvoice/Cancel',
            data: {
                ID: $("#Id").val(),
                Table: "PurchaseInvoice"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Purchase invoice cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel purchase invoice");
                    $(".btnSaveAndNew,.btnSaveAsDraft,.cancel").css({ 'display': 'block' });
                }
            },
        });

    },

    get_suppliers: function (release) {

        $.ajax({
            url: '/Masters/Supplier/GetNotInterCompanyAndMilkSupplierAutoComplete',
            data: {
                NameHint: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.Data);
            }
        });

    },

    clear_items: function () {
        var self = purchase_invoice;
        $('#tbl-item-details tbody').empty();
        $('#item-count').val(0);
        $("#tbl-other-charges tbody").html('');
        $("#tbl-tax-details tbody").html('');
        $("#txtTotalInvoiceValueCalculated").val(0);
        $("#txtTotalDifferenceCalculated").val(0);
        $('#txtAmountPayable').val(0);
        $('#txtInvoiceNo').val('');
        $('#txtInvoiceDate').val('');
        $('#txtDiscount').val(0);
        $('#RoundOff').val(0);
        $('#SGSTAmt').val(0);
        $('#CGSTAmt').val(0);
        $('#IGSTAmt').val(0);
        $('#GrossAmt').val(0);
    },

    set_supplier_details: function (event, item) {   // on select auto complete item
        var self = purchase_invoice;
        event.preventDefault();
        if (($("#tbl-item-details tbody tr").length > 0) && (item.id != $("#SupplierID").val())) {
            app.confirm("Selected items will be removed", function () {
                self.clear_items();
                self.on_change_supplier(item);
            });
        }
        else {
            self.on_change_supplier(item);
        }
        self.get_invoice_number_count();
        return false;
    },

    select_supplier: function () {
        var self = purchase_invoice;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {};
        item.id = radio.val();
        item.value = $(row).find(".Name").text().trim();
        item.location = $(row).find(".Location").text().trim();
        item.code = $(row).find(".Code").text().trim();
        item.stateId = $(row).find(".StateID").val();
        item.isGstRegistered = $(row).find(".IsGSTRegistered").val();
        item.gstno = $(row).find(".GstNo").val();
        item.supplierCategory = $(row).find(".SupplierCategory").text().trim();

        if ($("#SupplierID").val() != item.id) {
            if (($("#tbl-item-details tbody tr").length > 0) && (item.id != $("#SupplierID").val())) {
                app.confirm("Selected items will be removed", function () {
                    self.clear_items();
                    self.on_change_supplier(item);
                })
            }
            else {
                self.on_change_supplier(item);
            }
        }
        self.get_invoice_number_count();
        UIkit.modal($('#select-supplier')).hide();
    },

    on_change_supplier: function (item) {

        $("#SupplierName").val(item.value);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#SupplierCode").val(item.code);
        $("#SupplierCategory").val(item.supplierCategory);
        $("#GSTNo").val(item.gstno);

        if (item.code == "LOCALSUP") {
            $("#txtLocalSupplier").removeAttr("disabled");
        }
        if (item.supplierCategory == "Local Milk Suppliers") {
            $(".milk").removeClass("uk-hidden");
            $(".grn").addClass("uk-hidden");
        }
        else {
            $(".milk").addClass("uk-hidden");
            $(".grn").removeClass("uk-hidden");
        }

        $("#SupplierReferenceNo").focus();
        purchase_invoice.grn_ids = [];
        purchase_invoice.get_grn();
    },
    get_alopathy_grn: function () {
        var self = purchase_invoice;
        $.ajax({
            url: '/Purchase/GRN/GetUnProcessedGRN',
            data: {

            },
            dataType: "html",
            type: "GET",
            success: function (response) {
                var $response = $(response);
                app.format($response);
                $('#grn-list tbody').html($response);
                grn_select_table = $('#grn-list').SelectTable({
                    selectFunction: self.select_grn,
                    modal: "#add_grn",
                    returnFocus: "#SupplierReferenceNo",
                    initiatingElement: "#GRNNo",
                    selectionType: "radio"
                });

            }
        });
    },

    get_grn: function () {
        if ($("#SupplierCategory").val() != "Local Milk Suppliers") {
            $.ajax({
                url: '/Purchase/GRN/GetUnProcessedGRNBySupplier',
                data: {
                    SupplierID: $('#SupplierID').val()
                },
                dataType: "html",
                type: "GET",
                success: function (response) {
                    var $response = $(response);
                    app.format($response);
                    $('#grn-list tbody').html($response);
                    grn_select_table = $('#grn-list').SelectTable({
                        returnFocus: "#SupplierReferenceNo"
                    });

                }
            });
        }
        else {
            $.ajax({
                url: '/Purchase/GRN/GetUnProcessedMilkBySupplier',
                data: {
                    SupplierID: $('#SupplierID').val()
                },
                dataType: "html",
                type: "GET",
                success: function (response) {
                    var $response = $(response);
                    app.format($response);
                    $('#milk-list tbody').html($response);
                    grn_select_table = $('#milk-list').SelectTable({
                        returnFocus: "#SupplierReferenceNo"
                    });
                }
            });
        }
    },

    select_grn_milk: function () {
        var self = purchase_invoice;
        if ($("#tbl-item-details tbody tr").length > 0) {
            app.confirm("Selected items will be removed", function () {
                self.clear_items();
                self.get_grn_items_milk();
            })
        }
        else {
            self.get_grn_items_milk();
        }
    },

    select_grn: function () {
        var self = purchase_invoice;
        if ($("#tbl-item-details tbody tr").length > 0) {
            app.confirm("Selected items will be removed", function () {
                self.clear_items();
                //self.get_grn_items();
                self.get_grn_items_for_allopathy();
            })
        }
        else {
            self.get_grn_items_for_allopathy();
        }
    },

    get_grn_items: function () {
        var self = purchase_invoice;
        var maxDate;
        var maxDateString;
        var CurrentDate;
        var grn_ids = [];

        var gstTotal;
        if ($("#grn-list tbody tr .chkGRNBO:checked").length == 0) {
            app.show_error("Please select GRN");
            return;
        }
        maxDate = self.get_date_time($("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val());
        maxDateString = $("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val();

        $("#grn-list tbody tr .chkGRNBO:checked").each(function () {
            grn_ids.push($(this).val());
            var currRow = $(this).closest('tr');
            CurrentDate = self.get_date_time($(currRow).find('.Date').val());
            if (CurrentDate > maxDate) {
                maxDate = CurrentDate;
                maxDateString = $(currRow).find('.Date').val();
            }
        })
        $('#Date').val(maxDateString);
        $.ajax({
            url: '/Purchase/PurchaseInvoice/GetUnProcessedGRNItems',
            data: {
                grnIDS: grn_ids
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var container = $("<tbody>" + response + "</tbody>");
                $("#tbl-item-details tbody").find(".hdnGRNTransID").each(function () {
                    var GRNTransID = $(this).val();
                    $(container).find(".hdnGRNTransID[value=" + GRNTransID + "]").closest('tr').remove();
                })

                $('#tbl-item-details tbody').append(container.html());
                app.format($('#tbl-item-details'));
                setTimeout(function () { fh_item.resizeHeader(); }, 100);
                $('#tbl-item-details tbody tr').each(function () {
                    var row = $(this).closest('tr');
                    var igst = clean($(row).find('.IGSTPercent').val());
                    $(row).find('.gstPercentage option:selected').text(igst);
                    $(row).find('.gstPercentage option:selected').val(igst);
                });
            }
        });
    },

    //For Allopathy (Ayurware-get_grn_items)

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

    get_grn_items_for_allopathy: function () {
        var self = purchase_invoice;
        var maxDate;
        var maxDateString;
        var CurrentDate;
        var grn_ids = [];
        var grnid = 0
        var gstTotal;
        if ($("#grn-list tbody tr .chkGRNBO:checked").length == 0) {
            app.show_error("Please select GRN");
            return;
        }
        maxDate = self.get_date_time($("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val());
        maxDateString = $("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val();
        var GRN = $("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".GrnCode").text();
        var SupplerName = $("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".SupplierName").text();
        var SupplierID = clean($("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".SupplierID").val());
        var IsGSTRegistered = clean($("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".IsGSTRegistered").val());
        var StateID = clean($("#grn-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".StateID").val());
        $("#StateID").val(StateID);
        var ShippingStateID = $("#ShippingStateID").val();
        $("#grn-list tbody tr .chkGRNBO:checked").each(function () {
            grn_ids.push($(this).val());
            grnid = $(this).val();
            var currRow = $(this).closest('tr');
            CurrentDate = self.get_date_time($(currRow).find('.Date').val());
            if (CurrentDate > maxDate) {
                maxDate = CurrentDate;
                maxDateString = $(currRow).find('.Date').val();
            }
        })
        $('#Date').val(maxDateString);
        $('#GRNNo').val(GRN);
        $('#SupplierName').val(SupplerName);

        $("#SupplierID").val(SupplierID);
        $.ajax({
            url: '/Purchase/PurchaseInvoice/GRNItemsForPurchaseInvoice',
            data: {
                grnIDS: grn_ids
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var container = $("<tbody>" + response + "</tbody>");
                //$("#tbl-item-details tbody").find(".hdnGRNTransID").each(function () {
                //    var GRNTransID = $(this).val();
                //    $(container).find(".hdnGRNTransID[value=" + GRNTransID + "]").closest('tr').remove();
                //})
                $('#tbl-item-details tbody').html('');
                $('#tbl-item-details tbody').append(container.html());
                $("#item-count").val(1);
                app.format($('#tbl-item-details'));
                setTimeout(function () { fh_item.resizeHeader(); }, 100);
                $('#tbl-item-details tbody tr').each(function () {
                    var row = $(this).closest('tr');
                    var igst = clean($(row).find('.IGSTPercent').val());
                    var Rate = clean($(row).find('.txtInvoiceRate').val());
                    var qty = clean($(row).find('.txtItemInvoiceQty').val());
                    var Discount = clean($(row).find('.DiscountAmount').val());
                    var total = (Rate * qty) - Discount;
                    var gstamount = igst * total / 100;
                    var totalamount = gstamount + total;
                    if (IsGSTRegistered == "false") {
                        IGSTAmt = 0.00;
                        SGSTAmt = 0.00;
                        CGSTAmt = 0.00;
                        IGSTPercent = 0.00;
                        SGSTPercent = 0.00;
                        CGSTPercent = 0.00;
                    }
                    else {
                        if (ShippingStateID != StateID) {
                            IGSTAmt = igst * total / 100;
                            SGSTAmt = 0.00;
                            CGSTAmt = 0.00;
                            IGSTPercent = igst;
                            SGSTPercent = 0.00;
                            CGSTPercent = 0.00;
                        }
                            //else if (item.SGSTAmt > 0 && item.CGSTAmt > 0 && item.IGSTAmt == 0)
                        else {
                            IGSTPercent = 0.00;
                            SGSTPercent = igst / 2;
                            CGSTPercent = igst / 2;
                            IGSTAmt = 0.00;
                            SGSTAmt = SGSTPercent * total / 100;
                            CGSTAmt = CGSTPercent * total / 100;
                        }
                    }
                    //GSTPercent = '<select class="md-input label-fixed GstPercentageID">' + purchase_invoice.Build_Select_Gst(GSTList, igst) + '</select>'
                    $(row).find('.gstPercentage option:selected').text(igst);
                    $(row).find('.gstPercentage option:selected').val(igst);
                    $(row).find('.SGSTPercent').val(SGSTPercent);
                    $(row).find('.CGSTPercent').val(CGSTPercent);
                    $(row).find('.IGSTPercent').val(IGSTPercent);
                    $(row).find('.SGSTAmt').val(SGSTAmt.toFixed(2));
                    $(row).find('.CGSTAmt').val(CGSTAmt.toFixed(2));
                    $(row).find('.IGSTAmt').val(IGSTAmt.toFixed(2));
                    $(row).find('.NetAmount').val(totalamount.toFixed(2))
                });

            }

        });

        self.get_grn_master(grnid);
    },

    get_grn_master: function (grnid) {
        var self = purchase_invoice;
        $.ajax({
            url: '/Purchase/PurchaseInvoice/GRNIMasterForPurchaseInvoice',
            data: {
                grnIDS: grnid
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#txtGrossAmount").val(response.data.GrossAmount);
                $("#txtDiscount").val(response.data.DiscountAmt);
                $("#CGSTAmt").val(response.data.CGSTAmt);
                $("#IGSTAmt").val(response.data.IGSTAmt);
                $("#SGSTAmt").val(response.data.SGSTAmt);
                $("#GrossAmt").val(response.data.GrossAmt);
                $("#txtDeductions").val(response.data.RoundOff);
                $("#txtAmountPayable").val(response.data.NetAmount);
                $("#txtInvoiceDate").val(response.data.InvoiceDate);
                $("#txtInvoiceNo").val(response.data.InvoiceNo);
                //self.claculate_grid_total();
            }
        });
        self.count_items();

    },

    get_grn_items_milk: function () {
        var self = purchase_invoice;
        var maxDate;
        var maxDateString;
        var CurrentDate;
        var grn_ids = [];

        if ($("#milk-list tbody tr .chkGRNBO:checked").length == 0) {
            app.show_error("Please select GRN");
            return;
        }

        maxDate = self.get_date_time($("#milk-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val());
        maxDateString = $("#milk-list tbody tr .chkGRNBO:checked").eq(0).closest("tr").find(".Date").val();

        $("#milk-list tbody tr .chkGRNBO:checked").each(function () {
            grn_ids.push($(this).val());
            var currRow = $(this).closest('tr');
            CurrentDate = self.get_date_time($(currRow).find('.Date').val());
            if (CurrentDate > maxDate) {
                maxDate = CurrentDate;
                maxDateString = $(currRow).find('.Date').val();
            }
        })
        $('#Date').val(maxDateString);

        $.ajax({
            url: '/Purchase/PurchaseInvoice/GetUnProcessedMilkItems',
            data: {
                grnIDS: grn_ids
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var container = $("<tbody>" + response + "</tbody>");

                $('#tbl-item-details tbody').append(container.html());
                app.format($('#tbl-item-details'));

                fh_item.resizeHeader();
            }
        });
    },

    get_date_time: function (date_string) {
        var date = date_string.split('-');
        var used_date = new Date(date[2], date[1] - 1, date[0]);
        return used_date.getTime();
    },

    calculate_gst: function () {
        var self = purchase_invoice;
        //  $('#tbl-item-details tbody tr').each(function () {
        var $currRow = $(this).parents('tr');
        var igst;
        var cgst;
        var sgst;
        var totalgst;
        if ($("#StateID").val() == $("#ShippingStateID").val()) {
            igst = 0;
            totalgst = clean($currRow.find(".gstPercentage").val());
            cgst = totalgst / 2;
            sgst = totalgst / 2;
            $currRow.find(".IGSTPercent").val(igst);
            $currRow.find(".CGSTPercent").val(cgst);
            $currRow.find(".SGSTPercent").val(sgst);
        }
        else {
            igst = clean($currRow.find(".gstPercentage").val());
            cgst = 0;
            sgst = 0;
            $currRow.find(".IGSTPercent").val(igst);
            $currRow.find(".CGSTPercent").val(cgst);
            $currRow.find(".SGSTPercent").val(sgst);
        }
        self.set_tax_details();
    },

    list: function () {
        var self = purchase_invoice;
        $('#tabs-purchase-invoice').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = purchase_invoice;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#purchase-invoice-draft-list');
                break;
            case "to-be-approved":
                $list = $('#purchase-invoice-to-be-approved-list');
                break;
            case "booked":
                $list = $('#purchase-invoice-booked-list');
                break;
            case "partially-paid":
                $list = $('#purchase-invoice-partially-paid-list');
                break;
            case "fully-paid":
                $list = $('#purchase-invoice-fully-paid-list');
                break;
            case "cancelled":
                $list = $('#purchase-invoice-cancelled-list');
                break;
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/PurchaseInvoice/GetPurchaseInvoiceList?type=" + type;

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
                        { "data": "InvoiceNo", "className": "InvoiceNo" },
                        { "data": "InvoiceDate", "className": "InvoiceDate" },
                        { "data": "SupplierName", "className": "SupplierName" },
                        {
                            "data": "NetAmount",
                            "className": "NetAmount",
                            "searchable": false,
                            "render": function (data, type, row, meta) {
                                return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                            }

                    },

                    //{
                    //    "data": "poprint", "className": "action uk-text-center", "searchable": false,
                    //    "render": function (data, type, row, meta) {
                    //        return "<button class='md-btn md-btn-primary btnpoprint' >Print</button>" ;
                    //    }
                    //},


                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseInvoice/GenerateDetails/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    //selected_file_settings: {
    //    action: '/File/UploadFiles', // Target url for the upload
    //    allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
    //    loadstart: function () {
    //        $("#preloader").show();
    //        altair_helpers.content_preloader_show('md', 'success');
    //    },
    //    notallowed: function () {
    //        app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
    //    },
    //    progress: function (percent) {
    //        // percent = Math.ceil(percent);
    //        //  bar.css("width", percent + "%").text(percent + "%");
    //    },
    //    error: function (error) {
    //        console.log(error);
    //    },
    //    allcomplete: function (response, xhr) {
    //        $("#preloader").hide();
    //        altair_helpers.content_preloader_hide();
    //        var data = $.parseJSON(response)
    //        var width;
    //        var success = "";
    //        var failure = "";
    //        if (typeof data.Status != "undefined" && data.Status == "Success") {
    //            width = $('#selected-file').width() - 30;
    //            $(data.Data).each(function (i, record) {
    //                if (record.URL != "") {
    //                    $('#selected-file').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name );
    //                    success += record.Name + " " + record.Description + "<br/>";
    //                } else {
    //                    failure += record.Name + " " + record.Description + "<br/>";
    //                }
    //            });
    //        } else {
    //            failure = data.Message;
    //        }
    //        if (success != "") {
    //            app.show_notice(success);
    //        }
    //        if (failure != "") {
    //            app.show_error(failure);
    //        }
    //    }
    //},


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
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name);
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


    SaveInvoice: function () {
        var self = purchase_invoice;
        var form_data;
        self.error_count = 0;
        var url = '/Purchase/PurchaseInvoice/Save';
        if ($(this).hasClass('btnSaveAsDraft')) {

            self.error_count = self.validate_form_for_draft();
            if (self.error_count > 0) {
                return;
            }
            form_data = GetPurchaseInvoiceSaveObj();
            form_data.IsDraft = true;
            form_data.status = "Draft";
            url = '/Purchase/PurchaseInvoice/SaveAsDraft';
        } else {
            self.error_count = self.validate_form();
            if (self.error_count > 0) {
                return;
            }
            form_data = GetPurchaseInvoiceSaveObj();
            form_data.IsDraft = false;
            form_data.status = "Booked";
        }

        $(".btnSaveAndNew,.btnSaveAsDraft ").css({ 'display': 'none' });

        if (form_data.IsValid) {
            var callback = function (response) {
                if (response.Status == 'success') {
                    app.show_notice(response.Message);
                    window.location = "/Purchase/PurchaseInvoice/"
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveAndNew,.btnSaveAsDraft ").css({ 'display': 'block' });

                }
            }
            AjaxRequest(url, form_data, 'POST', callback);  //UnComment after testing
        }
        else {
            app.show_error(form_data.InValidReason);
        }
        return;

    },
    SaveApproveData: function () {

        var self = purchase_invoice;
        var form_data;
        self.error_count = 0;
        var url = '/Purchase/PurchaseInvoice/ApproveSave';

        self.error_count = self.validate_approve_invoice();
        if (self.error_count > 0) {
            return;
        }
        form_data = GetPurchaseInvoiceData();
        form_data.IsDraft = false;
        form_data.status = "Booked";


        $(".btnSaveInvoice,.btnSaveAsDraftInvoice,.approve").css({ 'display': 'none' });

        if (form_data.IsValid) {
            var callback = function (response) {
                if (response.Status == 'success') {
                    app.show_notice(response.Message);
                    window.location = "/Purchase/PurchaseInvoice/"
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveInvoice,.btnSaveAsDraftInvoice,.approve").css({ 'display': 'block' });

                }
            }
            AjaxRequest(url, form_data, 'POST', callback);  //UnComment after testing
        }
        else {
            app.show_error(form_data.InValidReason);
        }
        return;

    },
    SaveInvoiceData: function () {

        var self = purchase_invoice;
        var form_data;
        self.error_count = 0;
        var url = '/Purchase/PurchaseInvoice/GenereateSave';
        if ($(this).hasClass('btnSaveAsDraftInvoice')) {

            self.error_count = self.validate_form_for_draft();
            if (self.error_count > 0) {
                return;
            }
            form_data = GetPurchaseInvoiceData();
            form_data.IsDraft = true;
            form_data.status = "Draft";
            url = '/Purchase/PurchaseInvoice/GenerateSaveAsDraft';
        } else {
            self.error_count = self.validate_form_invoice();
            if (self.error_count > 0) {
                return;
            }
            form_data = GetPurchaseInvoiceData();
            form_data.IsDraft = false;
            form_data.status = "Booked";
        }

        $(".btnSaveInvoice,.btnSaveAsDraftInvoice,.approve").css({ 'display': 'none' });

        if (form_data.IsValid) {
            var callback = function (response) {
                if (response.Status == 'success') {
                    app.show_notice(response.Message);
                    window.location = "/Purchase/PurchaseInvoice/"
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveInvoice,.btnSaveAsDraftInvoice,.approve").css({ 'display': 'block' });

                }
            }
            AjaxRequest(url, form_data, 'POST', callback);  //UnComment after testing
        }
        else {
            app.show_error(form_data.InValidReason);
        }
        return;

    },
    search_tax_group: function (array, type, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].tax_percentage == value && array[i].type == type) {
                return i;
            }
        }
        return -1;
    },

    is_gst_in_array: function (array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == value) {
                return i;
            }
        }
        return -1;
    },

    validate_form: function () {
        var self = purchase_invoice;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_form_invoice: function () {
        var self = purchase_invoice;
        if (self.rules.on_submit_invoice.length) {
            return form.validate(self.rules.on_submit_invoice);
        }
        return 0;
    },


    validate_approve_invoice: function () {
        var self = purchase_invoice;
        if (self.rules.on_approve_invoice.length) {
            return form.validate(self.rules.on_approve_invoice);
        }
        return 0;
    },
    validate_form_for_draft: function () {
        var self = purchase_invoice;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    rules: {
        on_draft: [
    {
        elements: "#SupplierID",
        rules: [
    { type: form.required, message: "Please select supplier" },
    { type: form.non_zero, message: "Please select supplier" },
        ]
    },
    {
        elements: "#txtInvoiceDate",
        rules: [
    { type: form.required, message: "Invalid invoice date" },
    { type: form.past_date, message: "Invalid invoice date" },

        ]
    },
    //{
    //    elements: "#txtInvoiceNo",
    //    rules: [
    //{ type: form.required, message: "Invalid invoice number" },
    //    ]
    //},


    {
        elements: "#item-count",
        rules: [
    { type: form.non_zero, message: "Please add atleast one item" },
        ]
    },
    {
        elements: "#txtAmountPayable",
        rules: [
    { type: form.positive, message: "Invalid total difference" },
        ]
    },

        ],

        on_submit: [
    {
        elements: "#SupplierID",
        rules: [
    { type: form.required, message: "Please select supplier" },
    { type: form.non_zero, message: "Please select supplier" },
        ]
    },
         {
             elements: ".POLooseQty",
             rules: [
         {
             type: function (element) {
                 var error = false;
                 var LooseQty = clean($(element).closest('tr').find('.LooseQty').val());

                 var POLooseQty = clean($(element).val());
                 if ((POLooseQty != LooseQty))
                     error = true;
                 return !error;
             }, message: 'PO loose quantity and GRN loose quantity are different, please contact administrator'
         },
             ]
         },
    {
        elements: "#txtInvoiceDate",
        rules: [
    { type: form.required, message: "Invalid invoice date" },
    { type: form.past_date, message: "Invalid invoice date" },

        ]
    },
    {
        elements: "#txtInvoiceNo",
        rules: [
    { type: form.required, message: "Please enter invoice number" },
    {
        type: function (element) {
            var error = false;
            var count = clean($('#invoice-count').val());
            if (count > 0)
                error = true;
            return !error;
        }, message: 'Invoice number already entered for this supplier'
    },
        ]
    },
    {
        elements: "#txtInvoiceTotal",
        rules: [
    { type: form.non_zero, message: "Invalid invoice total" },
    { type: form.required, message: "Invalid invoice total" },
    { type: form.positive, message: "Invalid invoice total" },
    {
        type: function (element) {
            return parseFloat($(element).val().replace(/\,/g, '')) == parseFloat($("#txtTotalInvoiceValueCalculated").val().replace(/\,/g, ''));
        }, message: "Invoice total mismatch"
    },
        ]
    },
    {
        elements: "#item-count",
        rules: [
    { type: form.non_zero, message: "Please add atleast one item" },
        ]
    },
    {
        elements: ".txtItemInvoiceQty.included",
        rules: [
    { type: form.non_zero, message: "Please enter invoice quantity" },
    { type: form.required, message: "Please enter invoice quantity" },
    { type: form.positive, message: "Please enter positive invoice quantity" },
        ]
    },

    {
        elements: ".txtItemInvoiceRate.included",
        rules: [
    { type: form.non_zero, message: "Please enter invoice rate" },
    { type: form.required, message: "Please enter invoice rate" },
    { type: form.positive, message: "Please enter positive invoice rate" },
        ]
    },

    {
        elements: "#txtTDSOnFreight",
        rules: [
    { type: form.positive, message: "Invalid TDS on freight" },
        ]
    },
    {
        elements: "#txtLessTDS",
        rules: [
    { type: form.positive, message: "Please enter positive  less TDS" },
        ]
    },
    {
        elements: "#txtDiscount",
        rules: [
    { type: form.positive, message: "Please enter positive discount" },
        ]
    },

    {
        elements: ".txtItemInvoiceQty.included",
        rules: [
    { type: form.non_zero, message: "Please enter invoice quantity" },
    { type: form.required, message: "Please enter invoice quantity" },
    { type: form.positive, message: "Please enter positive invoice quantity" },
        ]
    },
    {
        elements: ".txtDeductionInvoiceValue",
        rules: [
    { type: form.positive, message: "Please enter positive freight" },
        ]
    },
    {
        elements: ".txtDeductionInvoiceValue ",
        rules: [
    { type: form.positive, message: "Please enter positive other chargres" },
        ]
    },
    {
        elements: ".txtDeductionInvoiceValue ",
        rules: [
    { type: form.positive, message: "Please enter positive packing charges" },
        ]
    },
    {
        elements: ".tax-po-value",
        rules: [
    { type: form.positive, message: "Please enter positive tax approved value" },
        ]
    },

    {
        elements: ".gst.SGST .tax-invoice-value",
        rules: [
    { type: form.positive, message: "Please enter positive tax approved value" },
    {
        type: function (element) {
            var sgst_amount = clean($(element).val());
            var cgst_amount = clean($(element).closest('tr').next('.CGST').find('.tax-invoice-value').val());

            return cgst_amount == sgst_amount
        }, message: "SGST and CGST amount must be same"
    },
        ]
    },

    {
        elements: ".gst.CGST .tax-invoice-value",
        rules: [
    { type: form.positive, message: "Invalid tax approved value" },
    {
        type: function (element) {
            var sgst_amount = clean($(element).closest('tr').prev('.SGST').find('.tax-invoice-value').val());
            var cgst_amount = clean($(element).val());

            return cgst_amount == sgst_amount
        }, message: "SGST and CGST amount must be same"
    },
        ]
    },

    {
        elements: "#txtAmountPayable",
        rules: [
    { type: form.positive, message: "Invalid total difference" },
        ]
    },
    {
        elements: "#txtDeductions",
        rules: [
    {
        type: function (element) {
            var deduction = clean($("#txtDeductions").val());
            return deduction >= -1 && deduction <= 0.99
        }, message: "Deductions must be within -1 and 0.99"
    }
        ]
    },

        ],
        
        on_approve_invoice: [
    {
        elements: "#SupplierID",
        rules: [
    { type: form.required, message: "Please select supplier" },
    { type: form.non_zero, message: "Please select supplier" },
        ]
    },
    {
        elements: "#txtInvoiceDate",
        rules: [
    { type: form.required, message: "Invalid invoice date" },
    { type: form.past_date, message: "Invalid invoice date" },

        ]
    },


    {
        elements: "#txtInvoiceNo",
        rules: [
    { type: form.required, message: "Please enter invoice number" },
    {
        type: function (element) {
            var error = false;
            var count = clean($('#invoice-count').val());
            if (count > 0)
                error = true;
            return !error;
        }, message: 'Invoice number already entered for this supplier'
    },
        ]
    },

    {
        elements: "#SGSTAmt",
        rules: [
    {
        type: function (element) {
            var error = false;
            var cgst = clean($('#CGSTAmt').val());
            var sgst = clean($('#SGSTAmt').val());

            if (cgst > sgst)
                error = true;
            return !error;
        }, message: 'SGST & CGST must be equal'
    },
        ]
    },
    {
        elements: "#txtInvoiceTotal",
        rules: [
    { type: form.non_zero, message: "Invalid invoice total" },
    { type: form.required, message: "Invalid invoice total" },
    { type: form.positive, message: "Invalid invoice total" },
    {
        type: function (element) {
            return parseFloat($(element).val().replace(/\,/g, '')) == parseFloat($("#txtAmountPayable").val().replace(/\,/g, ''));
        }, message: "Invoice total mismatch"
    },
        ]
    },
        {
            elements: ".txtInvoiceRate",
            rules: [
        { type: form.non_zero, message: "Please enter invoice rate" },
        { type: form.positive, message: "Please enter positive invoice rate" },
            ]
        },
        {
            elements: "#txtAmountPayable",
            rules: [
        { type: form.non_zero, message: "Invalid total invoice value" },
        { type: form.positive, message: "Invalid total invoice value" },
            ]
        },
    {
        elements: "#txtDeductions",
        rules: [
    {
        type: function (element) {
            var deduction = clean($("#txtDeductions").val());
            return deduction >= -1 && deduction <= 0.99
        }, message: "Deductions must be within -1 and 0.99"
    }
        ]
    },

        ],
        on_submit_invoice: [
    {
        elements: "#SupplierID",
        rules: [
    { type: form.required, message: "Please select supplier" },
    { type: form.non_zero, message: "Please select supplier" },
        ]
    },
    {
        elements: "#txtInvoiceDate",
        rules: [
    { type: form.required, message: "Invalid invoice date" },
    { type: form.past_date, message: "Invalid invoice date" },

        ]
    },
     {
         elements: ".POLooseQty",
         rules: [
     {
         type: function (element) {
             var error = false;
             var LooseQty = clean($(element).closest('tr').find('.LooseQty').val());

             var POLooseQty = clean($(element).val());
             if ((POLooseQty != LooseQty))
                 error = true;
             return !error;
         }, message: 'PO loose quantity and GRN loose quantity are different, please contact administrator'
     },
         ]
     },

    {
        elements: "#txtInvoiceNo",
        rules: [
    { type: form.required, message: "Please enter invoice number" },
    {
        type: function (element) {
            var error = false;
            var count = clean($('#invoice-count').val());
            if (count > 0)
                error = true;
            return !error;
        }, message: 'Invoice number already entered for this supplier'
    },
        ]
    },

    {
        elements: "#SGSTAmt",
        rules: [
    {
        type: function (element) {
            var error = false;
            var cgst = clean($('#CGSTAmt').val());
            var sgst = clean($('#SGSTAmt').val());

            if (cgst > sgst)
                error = true;
            return !error;
        }, message: 'SGST & CGST must be equal'
    },
        ]
    },
    {
        elements: "#txtInvoiceTotal",
        rules: [
    { type: form.non_zero, message: "Invalid invoice total" },
    { type: form.required, message: "Invalid invoice total" },
    { type: form.positive, message: "Invalid invoice total" },
    {
        type: function (element) {
            return parseFloat($(element).val().replace(/\,/g, '')) == parseFloat($("#txtAmountPayable").val().replace(/\,/g, ''));
        }, message: "Invoice total mismatch"
    },
        ]
    },
        {
            elements: ".txtInvoiceRate",
            rules: [
        { type: form.non_zero, message: "Please enter invoice rate" },
        { type: form.positive, message: "Please enter positive invoice rate" },
            ]
        },
        {
            elements: "#txtAmountPayable",
            rules: [
        { type: form.non_zero, message: "Invalid total invoice value" },
        { type: form.positive, message: "Invalid total invoice value" },
            ]
        },
    {
        elements: "#txtDeductions",
        rules: [
    {
        type: function (element) {
            var deduction = clean($("#txtDeductions").val());
            return deduction >= -1 && deduction <= 0.99
        }, message: "Deductions must be within -1 and 0.99"
    }
        ]
    },

        ]

    },

    error_count: 0,
};

function CalculateTotals() {

    CalculateTotalDifference();
    CalculateTotalFreight();
    CalculateAmountPayable();
    Calculate_Net_Value_With_Discount_And_All();
    CalculateInvoiceTotal();

}

function AjaxRequest(url, data, requestType, callBack) {
    $.ajax({
        url: url,
        cache: false,
        type: requestType,
        //traditional: true,
        data: data,
        success: function (successResponse) {
            //console.log('successResponse');
            if (callBack != null && callBack != undefined)
                callBack(successResponse);
        },
        error: function (errResponse) {//Error Occured 
            //console.log(errResponse);
        }
    });
}

function GetPurchaseInvoiceSaveObj() {
    var root = {};
    if ($('#txtTaxOnFreightInvoiceValue').length == 0) {
        root.IsValid = false;
        root.InValidReason = 'Please verify other charges and tax details';
        return root;
    }

    var purchaseInvoiceID = $('#hdnPurchaseInvoiceId').val();

    root = {
        ID: purchaseInvoiceID,
        IsValid: true,
        InValidReason: '',
        PurchaseNo: $('#txtTransNo').val(),
        PurchaseDateStr: $('#txtTransDate').val(),
        SupplierID: $("#SupplierID").val(),
        LocalSupplierName: $('#txtLocalSupplier').val(),
        InvoiceNo: $('#txtInvoiceNo').val(),
        InvoiceDateStr: $('#txtInvoiceDate').val(),
        GrossAmount: clean($('#txtGrossAmount').val()),
        SGSTAmount: 0,      //Will be calculated in controller from TaxDetails tab
        CGSTAmount: 0,      //Will be calculated in controller from TaxDetails tab
        IGSTAmount: 0,      //Will be calculated in controller from TaxDetails tab
        Discount: clean($('#txtDiscount').val()),
        FreightAmount: clean($('#txtTotalFreightCalculated').val()),   //Wil be calculated in controller from OtherCharges tab
        PackingCharges: 0,  //Wil be calculated in controller from OtherCharges tab
        OtherCharges: 0,
        TaxOnFreight: clean($('#txtTaxOnFreightInvoiceValue').val()),
        TaxOnPackingCharges: clean($('#txtTaxOnPackingChargeInvoiceValue').val()),
        TaxOnOtherCharge: clean($('#txtTaxOnOtherChargeInvoiceValue').val()),
        TDSOnFreightPercentage: clean($('#txtTDSOnFreight').val()),
        LessTDS: clean($('#txtLessTDS').val()),
        AmountPayable: clean($('#txtAmountPayable').val()),
        InvoiceTotal: clean($('#txtInvoiceTotal').val()),
        TotalDifference: clean($('#txtTotalDifferenceCalculated').val()),
        OtherDeductions: clean($('#txtDeductions').val()),
        TDSID: clean($('#TDSID').val()),
        SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
        Remarks: $('#Remarks').val(),

        //NetAmount: 0,           //
        //LocationID: 0
    };

    var otherChargeDetailsArr = [];
    //[{ PurchaseInvoiceID: "",PurchaseOrderID: "",Particular: "",POValue: "",InvoiceValue: "",DifferenceValue: "",Remarks: ""}]

    $('#other-charge-tblContainer table#tbl-other-charges tbody tr').each(function () {
        otherChargeDetailsArr.push(
                        {
                            //PurchaseInvoiceID: "",
                            ID: $(this).find('.hdnID').val(),
                            PurchaseOrderID: $(this).find('.hdnPurchaseOrderID').val(),
                            Particular: $(this).find('.deductionName').html(),
                            POValue: clean($(this).find('.deductionPOValue').val()),
                            InvoiceValue: clean($(this).find('.txtDeductionInvoiceValue').val()),
                            //DifferenceValue: "",
                            Remarks: $(this).find('.txtDeductionRemarks').val()
                        });
    });

    var taxDetailsArr = [];
    //[{PurchaseInvoiceID: "",Particular: "",TaxPercentage: "",POVAlue: "",InvoiceValue: "",DifferenceValue: "",Remarks: ""}];

    $('#tax-details-tblContainer table#tbl-tax-details tbody tr').each(function () {
        taxDetailsArr.push(
                        {
                            //PurchaseInvoiceID: "",
                            ID: $(this).find('.hdnID').val(),
                            Particular: $(this).find('.tax-particular').val(),
                            TaxPercentage: $(this).find('.tax-percentage').val(),
                            POValue: clean($(this).find('.tax-po-value').val()),
                            InvoiceValue: clean($(this).find('.tax-invoice-value').val()),
                            //DifferenceValue: "",
                            Remarks: $(this).find('.tax-remarks').val()
                        });
    });


    var invoiceTransArr = [];
    //[{PurchaseInvoiceID: "",GRNID: "",ItemID: "",InvoiceQty: "",InvoiceRate: "",InvoiceValue: "",AcceptedQty: "",ApprovedQty: "",ApprovedValue:"",PORate: "",Difference: "",Remarks: ""}];


    $('#item-details-tblContainer table#tbl-item-details tbody tr .chkItem:checked').each(function () {
        var $currRow = $(this).parents('tr');

        var invQty = $currRow.find('.txtItemInvoiceQty').val().replace(/\,/g, '');
        var invRate = $currRow.find('.txtItemInvoiceRate').val().replace(/\,/g, '');
        var invValue = $currRow.find('.txtItemInvoiceValue').val().replace(/\,/g, '');

        if (invQty == '' || invRate == '' || invValue == '') {
            root.IsValid = false;
            root.InValidReason = 'Invoice details not entered for selected items';
            return root;
        }

        invoiceTransArr.push(
                        {
                            //PurchaseInvoiceID: "",
                            ID: $(this).find('.hdnID').val(),
                            GRNID: $currRow.find('.hdnGRNID').val(),
                            GRNTransID: $currRow.find('.hdnGRNTransID').val(),
                            ItemID: $(this).val(),
                            InvoiceQty: invQty,
                            InvoiceRate: invRate,
                            InvoiceValue: invValue,
                            AcceptedQty: $currRow.find('.itemAcceptedQty').text().replace(/\,/g, ''),
                            ApprovedQty: $currRow.find('.itemAprovedQty').text().replace(/\,/g, ''),
                            ApprovedValue: $currRow.find('.itemApprovedValue').text().replace(/\,/g, ''),
                            PORate: $currRow.find('.itemPORate').text().replace(/\,/g, ''),
                            Difference: $currRow.find('.itemDiffValue').val().replace(/\,/g, ''),
                            Remarks: $currRow.find('.txtItemRemarks').val(),
                            CGSTPercent: clean($currRow.find('.CGSTPercent').val()),
                            SGSTPercent: clean($currRow.find('.SGSTPercent').val()),
                            IGSTPercent: clean($currRow.find('.IGSTPercent').val()),
                            InvoiceGSTPercent: clean($currRow.find('.gstPercentage').val()),
                            MilkPurchaseID: clean($currRow.find('.MilkPurchaseID').val()),
                            UnMatchedQty: clean($currRow.find('.unmatchedQty').text()),
                            UnitID: clean($currRow.find('.UnitID').val()),
                        });
    });

    root.OtherChargeDetails = otherChargeDetailsArr;
    root.TaxDetails = taxDetailsArr;
    root.InvoiceTransItems = invoiceTransArr;
    return root;
}
function GetPurchaseInvoiceData() {
    var root = {};


    var purchaseInvoiceID = $('#hdnPurchaseInvoiceId').val();

    root = {
        ID: purchaseInvoiceID,
        IsValid: true,
        InValidReason: '',
        PurchaseNo: $('#txtTransNo').val(),
        PurchaseDateStr: $('#txtTransDate').val(),
        SupplierID: $("#SupplierID").val(),
        LocalSupplierName: $('#txtLocalSupplier').val(),
        InvoiceNo: $('#txtInvoiceNo').val(),
        InvoiceDateStr: $('#txtInvoiceDate').val(),
        GrossAmount: clean($('#GrossAmt').val()),
        IGST: clean($('#IGSTAmt').val()),
        SGST: clean($('#SGSTAmt').val()),
        CGST: clean($('#CGSTAmt').val()),
        Discount: clean($('#txtDiscount').val()),
        FreightAmount: clean($('#txtTotalFreightCalculated').val()),   //Wil be calculated in controller from OtherCharges tab
        PackingCharges: 0,  //Wil be calculated in controller from OtherCharges tab
        OtherCharges: 0,
        TaxOnFreight: clean($('#txtTaxOnFreightInvoiceValue').val()),
        TaxOnPackingCharges: clean($('#txtTaxOnPackingChargeInvoiceValue').val()),
        TaxOnOtherCharge: clean($('#txtTaxOnOtherChargeInvoiceValue').val()),
        TDSOnFreightPercentage: clean($('#txtTDSOnFreight').val()),
        LessTDS: clean($('#txtLessTDS').val()),
        AmountPayable: clean($('#txtAmountPayable').val()),
        InvoiceTotal: clean($('#txtInvoiceTotal').val()),
        TotalDifference: clean($('#txtTotalDifferenceCalculated').val()),
        OtherDeductions: clean($('#txtDeductions').val()),
        TDSID: clean($('#TDSID').val()),
        SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
        Remarks: $('#txtRemarks').val(),
        NetAmount: clean($('#txtAmountPayable').val()),
        GrnNo: $('#GRNNo').val(),
        Freight: $('#Freight').val(),
        WayBillNo: $('#WayBillNo').val(),
        InvoiceType: $('#InvoiceType').val()
        //NetAmount: 0,           //
        //LocationID: 0
    };


    var invoiceTransArr = [];
    //[{PurchaseInvoiceID: "",GRNID: "",ItemID: "",InvoiceQty: "",InvoiceRate: "",InvoiceValue: "",AcceptedQty: "",ApprovedQty: "",ApprovedValue:"",PORate: "",Difference: "",Remarks: ""}];


    $('#item-details-tblContainer table#tbl-item-details tbody tr .chkItem:checked').each(function () {
        var $currRow = $(this).parents('tr');

        var invQty = $currRow.find('.txtItemInvoiceQty').val().replace(/\,/g, '');
        var invRate = $currRow.find('.txtInvoiceRate').val().replace(/\,/g, '');
        var invValue = $currRow.find('.txtItemInvoiceValue').val().replace(/\,/g, '');

        if (invQty == '' || invRate == '' || invValue == '') {
            root.IsValid = false;
            root.InValidReason = 'Invoice details not entered for selected items';
            return root;
        }

        invoiceTransArr.push(
                        {
                            //PurchaseInvoiceID: "",
                            ID: $(this).find('.hdnID').val(),
                            GRNID: $currRow.find('.hdnGRNID').val(),
                            GRNTransID: $currRow.find('.hdnGRNTransID').val(),
                            ItemID: $(this).val(),
                            InvoiceQty: invQty,
                            InvoiceRate: invRate,
                            InvoiceValue: invValue,
                            AcceptedQty: $currRow.find('.itemAcceptedQty').text().replace(/\,/g, ''),
                            ApprovedQty: $currRow.find('.itemAprovedQty').text().replace(/\,/g, ''),
                            ApprovedValue: $currRow.find('.itemApprovedValue').text().replace(/\,/g, ''),
                            PORate: $currRow.find('.txtInvoiceRate').val().replace(/\,/g, ''),
                            //Difference: $currRow.find('.itemDiffValue').val().replace(/\,/g, ''),
                            Remarks: $currRow.find('.txtItemRemarks').val(),
                            CGSTPercent: clean($currRow.find('.CGSTPercent').val()),
                            SGSTPercent: clean($currRow.find('.SGSTPercent').val()),
                            IGSTPercent: clean($currRow.find('.IGSTPercent').val()),
                            InvoiceGSTPercent: clean($currRow.find('.gstPercentage').val()),
                            MilkPurchaseID: clean($currRow.find('.MilkPurchaseID').val()),
                            UnMatchedQty: clean($currRow.find('.unmatchedQty').text()),
                            UnitID: clean($currRow.find('.UnitID').val()),
                            OfferQty: clean($currRow.find('.OfferQty').val()),
                            BatchID: clean($currRow.find('.BatchID').val()),
                            DiscountAmount: clean($currRow.find('.DiscountAmount').val()),
                            NetAmount: clean($currRow.find('.NetAmount').val()),
                            ItemName: $currRow.find('.ItemName').text(),
                            SGSTAmt: clean($currRow.find('.SGSTAmt').val()),
                            CGSTAmt: clean($currRow.find('.CGSTAmt').val()),
                            IGSTAmt: clean($currRow.find('.IGSTAmt').val()),
                        });
    });

    root.InvoiceTransItems = invoiceTransArr;
    return root;

}
function CheckInvoiceNoValid() {
    var invoiceNo = $('#txtInvoiceNo').val();
    if (invoiceNo != '') {
        var callBack = function (response) {
            //console.log(response.IsValid);
            //if (!response.IsValid || response.IsValid.toLowerCase() == 'false')
            if (!response.IsValid || response.IsValid == 'false' || response.IsValid == 'False') {
                alert(response.Message);
                $currObj.IsValidInvoiceNumber = false;
            }
            else {
                $currObj.IsValidInvoiceNumber = true;
            }
            console.log("Validated");
        };
        var supplierID = $currObj.SelectedSupplierID;
        var data = { supplierID: supplierID, invoiceNo: invoiceNo };
        AjaxRequest('/Purchase/PurchaseInvoice/CheckInvoiceNoValid', data, 'GET', callBack)
    }
}

//Changed to common functions. These methods are shared b/w Create|Edit and Detail
function CalculateInvoiceTotal() {
    var totInvoiceVal = 0;
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());

    var currVal;
    $('#item-details-tblContainer table tr td .chkItem:checked').parents('tr').find('.txtItemInvoiceValue').each(function () {
        currVal = clean($(this).val());
        totInvoiceVal += currVal;
    });

    $('#other-charge-tblContainer table tr td .txtDeductionInvoiceValue').each(function () {
        currVal = clean($(this).val());
        totInvoiceVal += currVal;
    });
    $('#tax-details-tblContainer table tr td .tax-invoice-value.gst-extra').each(function () {
        currVal = clean($(this).val());
        totInvoiceVal += currVal;
    });

    totInvoiceVal = totInvoiceVal - discount + deductions;


    $('#txtTotalInvoiceValueCalculated').val(totInvoiceVal);
}
function Calculate_Net_Value_With_Discount_And_All() {
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());
    // Invoice value - (TDS + TDS on advance)
    // var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
    var totalInvoice = 0;
    var currVal;
    $('#item-details-tblContainer table tr td .chkItem:checked').parents('tr').find('.txtItemInvoiceValue').each(function () {
        currVal = clean($(this).val());
        totalInvoice += currVal;
    });

    $('#other-charge-tblContainer table tr td .txtDeductionInvoiceValue').each(function () {
        currVal = clean($(this).val());
        totalInvoice += currVal;
    });
    $('#tax-details-tblContainer table tr td .tax-invoice-value.gst-extra').each(function () {
        currVal = clean($(this).val());
        totalInvoice += currVal;
    });
    var tds = clean($('#txtTDS').val());
    var advanceTDS = clean($('#txtTDSOnAdvance').val());
    var oldadvanceTDS = clean($('#TDSOnAdvance').val());
    var total;
    if (tds > oldadvanceTDS) {
        $('#txtTDSOnAdvance').val(oldadvanceTDS)
    }
    if (advanceTDS > tds && tds > 0) {
        $('#txtTDSOnAdvance').val(tds)
        total = tds - tds;
    }
    else {
        total = advanceTDS - tds;
    }
    if (total < 0) {
        total = total * -1;
    }
    $("#txtNetTDS").val(total);
    var netPayable = isNaN(totalInvoice) ? 0 : totalInvoice
    var netPayables = netPayable //+ deductions - discount;
    $('#DummyInvoiceTotal').val(netPayable);
    $('#txtAmountPayable').val(netPayables);
}
//function Calculate_Net_Value_With_Discount_And_All() {
//    var discount = clean($('#txtDiscount').val());
//    var deductions = clean($('#txtDeductions').val());
//    //  Invoice value - (TDS + TDS on advance)
//    var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
//    var tds = clean($('#txtTDS').val());
//    var advanceTDS = clean($('#txtTDSOnAdvance').val());
//    var oldadvanceTDS = clean($('#TDSOnAdvance').val());
//    var total;
//    if (tds > oldadvanceTDS) {
//        $('#txtTDSOnAdvance').val(oldadvanceTDS)
//    }
//    if (advanceTDS > tds && tds > 0) {
//        $('#txtTDSOnAdvance').val(tds)
//        total = tds - tds;
//    }
//    else {
//        total = advanceTDS - tds;
//    }
//    if (total < 0) {
//        total = total * -1;
//    }
//    $("#txtNetTDS").val(total);
//    var netPayable = isNaN(totalInvoice) ? 0 : totalInvoice
//    netPayable = totalInvoice - deductions + discount;
//    $('#DummyInvoiceTotal').val(netPayable);
//}
function CalculateTotalDifference() {
    var totDiffVal = 0;
    var currVal;
    $('#item-details-tblContainer table tr td .chkItem:checked').parents('tr').find('.itemDiffValue').each(function () {
        currVal = clean($(this).val());
        totDiffVal += currVal;
    });
    $('#other-charge-tblContainer table tr .deductionDiffValue').each(function () {
        currVal = clean($(this).val());
        totDiffVal += currVal;
    });
    $('#tax-details-tblContainer table tr .tax-diff-value').each(function () {
        currVal = clean($(this).val());
        totDiffVal += currVal;
    });

    $('#txtTotalDifferenceCalculated').val(totDiffVal);
}

function CalculateTotalFreight() {
    var self = purchase_invoice;
    var totFreightVal = 0;
    var currVal;
    var count = $('#tbl-item-details tbody').find('input.chkItem:checked').length;

    if (count != 0) {
        $('table#tbl-other-charges tbody tr td .txtFreightInvoiceValue').each(function () {
            currVal = clean($(this).val());
            totFreightVal += currVal;
        });
    }
    $('#txtTotalFreightCalculated').val(totFreightVal);
    // self.calculate_tds();
}


function CalculateAmountPayable() {                               //Calculate Total Invoice. When any changes in TDSOnFreight, LessTDS, Discount, Deductions, TotalInvoice
    var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());
    var amtPayable = totalInvoice - deductions;
    $('#txtAmountPayable').val(amtPayable);
}
