
var fh_tax;
var fh_item;
var h = 0;
service_purchase_invoice = {
    init: function () {
        var self = service_purchase_invoice;
        $currObj = this;
        $currObj.SelectedSupplierID = 0;
        $currObj.SelectedSRNArray = [];

        var isTaxesLoaded = false;

        self.Freeze_header();

        if ($("#SupplierID").val() == 0 || $("#SupplierID").val() == "") {
            supplier.supplier_list('service');
            select_table = $('#supplier-list').SelectTable({
                selectFunction: self.select_supplier,
                returnFocus: "#txtInvoiceNo",
                modal: "#select-supplier",
                initiatingElement: "#SupplierName",
                selectionType: "radio"
            });
        } else {
            self.populate_srn();
        }

        self.bind_events();
    },

    details: function () {
        var self = service_purchase_invoice;
        $(".cancel").on("click", self.cancel_confirm);
        $('.btnApprove').on('click', self.approve);
        h = 0;
        self.Freeze_header();

    },

    Freeze_header: function () {

        if (h == 0) {
            fh_tax = $('#tax-details').FreezeHeader();
            fh_item = $('#item-details').FreezeHeader();
            h++;
        }
        else {
            fh_item.resizeHeader();
            fh_tax.resizeHeader();
        }
        $('[data-uk-tab]').on('show.uk.switcher', function (event, area) {
            fh_item.resizeHeader();
            fh_tax.resizeHeader();
        });
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getServiceSupplierForAutoComplete',
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
    bind_events: function () {
        var self = service_purchase_invoice;

        $("body").on('click', '#btnOKSupplier', self.select_supplier);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $('#txtDiscount, #txtDeductions').on('keyup', self.calculate_invoice_value);

        $("body").on('click', "#btnSRNSelectedOk", self.get_srn_items);
        $('.btnSaveAsDraft').on('click', self.save_as_draft);
        $('.btnSaveNew').on('click', self.save_confirm);
        $('body').on('change', '.txtItemInvoiceQty, .txtItemInvoiceRate', self.calculate_item_invoice_value);
        $('body').on('keyup', '.txtItemInvoiceQty, .txtItemInvoiceRate', self.calculate_item_invoice_value);
        $('body').on('keyup', '.tax-invoice-value', self.calculate_tax_diff);
        $('body').on('change', '.tax-invoice-value', self.calculate_tax_diff);
        $('body').on('change', '.gstPercentage', self.on_gst_changed);
        $('#txtTDS, #txtTDSOnAdvance').on('keyup', function () {
            CalculateNetPayable();
        });
        $('#txtTDS, #txtTDSOnAdvance').on('change', function () {
            CalculateNetPayable();
        });
        $(document).on('ifChanged', ".chkItem", self.include_item);

        //$('[data-uk-tab]').on('show.uk.switcher', function (event, area) {
        //    try {
        //        fh_item.resizeHeader();
        //        fh_tax.resizeHeader();
        //    } catch (e) {

        //    }

        //});
        $("#txtInvoiceNo").on("change", self.get_invoice_number_count);

        $("#btnOKSupplier").on('click', self.select_supplier);
        $("#TDSCode").on('change', self.calculate_tds);


    },

    cancel_confirm: function () {
        var self = service_purchase_invoice
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    save_confirm: function () {
        var self = service_purchase_invoice
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_invoice_number_count: function (release) {

        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/GetInvoiceNumberCount',
            data: {
                Hint: $("#txtInvoiceNo").val(),
                Table: "PurchaseInvoiceForservice",
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

    calculate_invoice_value: function () {                               //Calculate Total Invoice. When any changes in TDSOnFreight, LessTDS, Discount, Deductions, TotalInvoice
        var self = service_purchase_invoice;
        var totalInvoice = clean($('#DummyInvoiceTotal').val());
        var deductions = clean($('#txtDeductions').val());

        var discount = clean($('#txtDiscount').val());
        if (discount < 0) {
            app.show_error("Discount amount must be positive");
        }
        else {
            CalculateInvoiceTotal();

            self.calculate_total();
        }

    },
    CalculateNetValue: function () {
        Calculate_Net_Value_With_Discount_And_All();
    },
    calculate_total: function () {
        var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
        var discount = clean($('#txtDiscount').val());
        var deductions = clean($('#txtDeductions').val());
        //var amtPayable = totalInvoice - (tdsOnFreight + lessTDS + deductions);
        var amtPayable = totalInvoice;
        $('#txtNetPayable').val(amtPayable);
    },
    calculate_tds: function () {
        var self = service_purchase_invoice;
        var invValue = 0.0;
        var gst;
        $("#item-details tbody tr input.chkItem:checked").each(function () {

            $currtr = $(this).parents('tr');
            var isinclusivegst = 0;
            var invQty = clean($currtr.find('.txtItemInvoiceQty').val());
            var invRate = clean($currtr.find('.txtItemInvoiceRate').val());

            if (isinclusivegst == 1) {
                gst = clean($currtr.find('.gstPercentage ').val());
                invValue += (invQty * invRate) * 100 / (gst + 100);
            } else {
                invValue += (invQty * invRate);
            }
        })
        $("#TaxableValue").val(invValue);
        code = $("#TDSCode").val();
        var oldadvanceTDS = clean($('#TDSOnAdvance').val());
        var tdsonadvance = clean($('#txtTDSOnAdvance').val());
        var percentage = code.split("#");
        var amount = clean($('#TaxableValue').val());
        var tdsAmt = clean(self.getCalculatedTDSAmt(amount, clean(percentage[1])));
        var tdsID = clean(percentage[0]);
        var nettds = tdsAmt + tdsonadvance;
        $('#TDSID').val(tdsID);
        $('#txtTDS').val(tdsAmt);
        if (tdsAmt == 0) {
            $('#txtTDSOnAdvance').val(oldadvanceTDS);
            //$('#txtNetTDS').val(oldadvanceTDS);
        }
        else if (tdsAmt < oldadvanceTDS) {
            tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) !=0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
            $('#txtTDS').val(tdsAmt);
            $('#txtTDSOnAdvance').val(tdsAmt);
            nettds = 0;//tdsAmt - tdsAmt;
            $('#txtNetTDS').val(nettds);
        }
        else {
            $('#txtTDSOnAdvance').val(oldadvanceTDS);
            tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) != 0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
            nettds = tdsonadvance - tdsAmt;
            $('#txtTDS').val(tdsAmt);
            $('#txtNetTDS').val(nettds);
        }
        CalculateTotals();
    },
    getCalculatedTDSAmt: function (amt, percentage) {

        console.log(amt + "   " + percentage);
        if (amt == null || amt == undefined)
            amt = '0';
        if (amt == '' || amt.length <= 0)
        { amt = '0' }
        if (percentage != '') {
            return ((parseFloat(amt) / parseFloat(100)) * parseFloat(percentage)).toFixed(2);
        }

    },
    on_gst_changed: function () {
        var self = service_purchase_invoice;
        self.set_tax_details();
        CalculateTotals();
    },
    calculate_gst: function () {
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
        service_purchase_invoice.set_tax_details();
        CalculateTotals();

    },

    approve: function () {
        $(".btnApprove").css({ 'display': 'none' });

        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/Approve',
            data: {
                ID: $("#hdnServicePurchaseInvoiceID").val(),
                Status: "Approved",
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Successfully Approved");
                    setTimeout(function () {
                        window.location = "/Purchase/ServicePurchaseInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Approve Failed");
                    $(".btnApprove").css({ 'display': 'block' });

                }
            },
        });
    },
    calculate_tax_diff: function () {
        var tr = $(this).closest('tr');
        var invoice_value = clean($(this).val());
        var approved_value = clean($(tr).find('.po-value').text());
        $(tr).find('.tax-diff-value').val(invoice_value - approved_value);
        CalculateTotals();
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
            $(this).closest('tr').next('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').next('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
        }
        $(this).removeAttr('readonly');
        service_purchase_invoice.count_items();
        service_purchase_invoice.Get_tds_amount_For_UnProcessedSRNItems();
        service_purchase_invoice.set_tax_details();
        CalculateTotals();
    },
    set_tax_details: function () {
        //if ($("#IsGSTRegistered").val().toLowerCase() != "true") {
        //    return;
        //}
        var self = service_purchase_invoice;
        var tax_details = [];
        var index;
        var SGST = 0;
        var CGST = 0;
        var IGST = 0;
        var IsInclusiveGST = false;
        var tr;
        var inclusive_gst = "";
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var GSTChanges = [];
        var isIGST = true;
        if ($("#StateID").val() == $("#ShippingStateID").val()) {
            isIGST = false;
        }
        $("#item-details input.chkItem:checked").each(function () {
            $row = $(this).closest("tr");
            SGST = parseFloat($row.find(".SGSTPercent").val());
            CGST = parseFloat($row.find(".CGSTPercent").val());
            IGST = parseFloat($row.find(".IGSTPercent").val());
            GST = clean($row.find(".gstPercentage").val());
            if (IGST + CGST + SGST != GST && self.is_gst_in_array(GSTChanges, GST) == -1) {
                GSTChanges.push(GST);
            }

            accepted_value = IsGSTRegistered.toLowerCase() == "true" ? clean($row.find(".itemAcceptedValue").val()) : 0;
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

        $("#tax-details tbody").html("");
        $.each(tax_details, function (i, record) {

            inclusive_gst = "gst-extra";

            tr = "<tr class='" + record.type + "'>"
                + "<td>" + (i + 1) + "</td>"
                + "<td>" + record.particular + "<input type='hidden' class='tax-particular' value='" + record.type + "'></td>"
                + "<td class='mask-currency tax-percentage'>" + record.tax_percentage + "</td>"
                + "<td class='po-value mask-currency'>" + record.tax_amount + "</td>"
                + "<td><input type='text' class='md-input tax-invoice-value mask-currency " + inclusive_gst + "' value='0' /></td>"
                + "<td><input type='text' class='md-input tax-diff-value mask-currency' disabled='disabled' /></td>"
                + "<td><input type='text' class='md-input tax-remarks ' value=''></td>"
                + "</tr>";
            $("#tax-details tbody").append(tr);
        });
        //    self.Freeze_header();
        // fh_tax.resizeHeader();
        $currency = $('#tax-details tbody .mask-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
    },
    calculate_group_tax_details: function (tax_details, GST, type, accepted_value, IsInclusiveGST) {

        var IsGSTRegistered = $("#IsGSTRegistered").val();
        if (!IsGSTRegistered) {
            return;
        }
        var tax_amount = 0;

        var index = search_in_array(tax_details, type, GST);
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


        return tax_details;
    },
    count_items: function () {
        var self = service_purchase_invoice;
        var count = $('#item-details tbody').find('input.chkItem:checked').length;
        $('#item-count').val(count);
    },
    Get_tds_amount_For_UnProcessedSRNItems: function () {
        var self = service_purchase_invoice;
        var ids = '';
        $("#item-details tbody tr input.chkItem:checked").each(function () {

            var currRow = $(this).parents('tr');

            srnid = $(currRow).find('.hdnSRNID').val();
            ids += srnid + ',';
        })
        ids = ids.replace(/,\s*$/, "");
        var ID = clean($("#ServicePurchaseInvoiceID").val());
        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/GetTDsAmountForUnProcessedSRNItems',
            data: {

                IDs: ids,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (ID > 0) {
                    $("#TDSOnAdvance").val(data.data)
                }
                else {
                    $("#txtTDSOnAdvance").val(data.data)
                    $("#TDSOnAdvance").val(data.data)
                    if (clean($('#txtTDS').val()) == 0) {
                        $("#txtNetTDS").val(0);
                    }
                    else {
                        $("#txtNetTDS").val(data.data);
                    }


                }
            }
        });
        self.calculate_tds();
    },
    get_date_time: function (date_string) {
        var date = date_string.split('-');
        var used_date = new Date(date[2], date[1] - 1, date[0]);
        return used_date.getTime();
    },
    get_srn_items: function () {
        var self = service_purchase_invoice;
        var maxDate;
        var maxDateString;
        $currObj.SelectedSRNArray = [];
        $tblBody = $('#service-purchase-requisition-list-tblContainer table tbody');

        $tblBody.find('.chkSRNBO:checked').each(function () {
            $currObj.SelectedSRNArray.push($(this).val());
        });

        if ($currObj.SelectedSRNArray.length > 0) {                       //GRN Selected
            $('#item-count').val(0);
            maxDate = self.get_date_time($("#service-purchase-requisition-list .chkSRNBO:checked").eq(0).closest("tr").find(".Date").val());
            maxDateString = $("#service-purchase-requisition-list .chkSRNBO:checked").eq(0).closest("tr").find(".Date").val();

            $("#service-purchase-requisition-list tbody tr .chkSRNBO:checked").each(function () {

                var currRow = $(this).parents('tr');

                CurrentDate = self.get_date_time($(currRow).find('.Date').val());
                if (CurrentDate > maxDate) {
                    maxDate = CurrentDate;
                    maxDateString = $(currRow).find('.Date').val();
                }
            })
            var tdsadvance = clean($("#txtTDSOnAdvance").val());
            $('#SrDate').val(maxDateString);
            $.ajax({
                url: '/Purchase/ServicePurchaseInvoice/GetUnProcessedSRNItemsView',
                cache: false,
                type: "POST",
                dataType: "html",
                data: { srnArr: $currObj.SelectedSRNArray },
                success: function (response) {
                    var $response = $(response);
                    app.format($response);
                    $('#item-details tbody').html($response);
                    $('#item-details tbody tr').each(function () {
                        igst = clean($(this).find(".IGSTPercent").val());
                        sgst = clean($(this).find(".SGSTPercent").val());
                        cgst = clean($(this).find(".CGSTPercent").val());
                        tdsamount = clean($(this).find(".tdsamount").val());
                        gstTotal = igst + cgst + sgst;
                        $(this).find(".gstPercentage").val(gstTotal);
                        $("#txtTDSOnAdvance").val(tdsamount);
                    });
                    //  self.Freeze_header();
                    fh_item.resizeHeader();
                    isTaxesLoaded = false;
                    CalculateTotals();
                },
                error: function (errResponse) {
                    //console.log(errResponse);
                }
            });

        }

    },
    clear_items: function () {
        $('#item-details tbody').empty();
        $('#item-count').val(0);
        $('#tax-details tbody').html('');
        $('#txtNetPayable').val(0);
        $('#txtTotalInvoiceValueCalculated').val(0);
        $('#txtTotalDifferenceCalculated').val(0);
    },
    select_supplier: function () {
        var self = service_purchase_invoice;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var GstNumber = $(row).find(".GstNo").val();
        if (($("#item-details tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                self.clear_items();
                $("#SupplierName").val(Name);
                $("#SupplierID").val(ID);
                $("#SupplierLocation").val(Location);
                $("#StateID").val(StateID);
                $("#GSTNo").val(GstNumber);

                $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());

                service_purchase_invoice.populate_srn();
            })



        } else {
            $("#SupplierName").val(Name);
            $("#SupplierID").val(ID);
            $("#SupplierLocation").val(Location);
            $("#StateID").val(StateID);
            $("#GSTNo").val(GstNumber);
            $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());

            service_purchase_invoice.populate_srn();
        }
        service_purchase_invoice.get_invoice_number_count();
        UIkit.modal($('#select-supplier')).hide();
    },
    set_supplier_details: function (event, item) {   // on select auto complete item
        var self = service_purchase_invoice;
        event.preventDefault();
        if (($("#item-details tbody tr").length > 0) && (item.id != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                self.clear_items();
                $("#SupplierName").val(item.name);
                $("#SupplierID").val(item.id);
                $("#SupplierLocation").val(item.location);
                $("#StateID").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#GSTNo").val(item.gstno);
                $("#SupplierReferenceNo").focus();
                if (item.code == "LOCALSUP") {
                    $("#txtLocalSupplier").removeAttr("disabled");
                }

                service_purchase_invoice.populate_srn();
            })
        }
        else {
            if ($("#SupplierID").val() != item.id) {
                $("#SupplierName").val(item.name);
                $("#SupplierID").val(item.id);
                $("#SupplierLocation").val(item.location);
                $("#StateID").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#GSTNo").val(item.gstno);
                $("#SupplierReferenceNo").focus();

                if (item.code == "LOCALSUP") {
                    $("#txtLocalSupplier").removeAttr("disabled");
                }
                service_purchase_invoice.populate_srn();
            }
        }
        self.get_invoice_number_count();
        return false;
    },
    populate_srn: function () {


        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/GetUnProcessedSRNBySupplier',
            cache: false,
            type: "GET",
            dataType: "html",
            data: { supplierID: $("#SupplierID").val() },
            success: function (response) {
                var $response = $(response);
                app.format($response);
                $('#service-purchase-requisition-list-tblContainer').html($response);
            }
        });

    },
    calculate_item_invoice_value: function () {
        var self = service_purchase_invoice;
        $currtr = $(this).parents('tr');
        var invQty = clean($currtr.find('.txtItemInvoiceQty').val());
        var invRate = clean($currtr.find('.txtItemInvoiceRate').val());
        var itemPORate = clean($currtr.find('.itemPORate').text());
        var approvedvalue = clean($currtr.find('.itemAcceptedValue ').text());
        if (!isNaN(invQty) && !isNaN(invRate)) {
            var invValue = invQty * invRate;
            $currtr.find('.txtItemInvoiceValue').val(invValue);
            $currtr.find('.itemDiffValue').val(invValue - approvedvalue);
        }
        else {
            $currtr.find('.txtItemInvoiceValue').val('');
            $currtr.find('.itemDiffValue').val(-approvedValue);
        }
        self.calculate_tds();
        // CalculateTotals();
    },
    list: function () {
        var self = service_purchase_invoice;
        $('#tabs-purchase-invoice').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
        //self.tabbed_list("draft");
        //self.tabbed_list("to-be-approved");
        //self.tabbed_list("booked");
        //self.tabbed_list("partially-paid");
        //self.tabbed_list("fully-paid");
        //self.tabbed_list("cancelled");
    },

    tabbed_list: function (type) {
        var self = service_purchase_invoice;
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

            var url = "/Purchase/ServicePurchaseInvoice/GetPurchaseInvoiceList?type=" + type;

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
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/ServicePurchaseInvoice/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    save_as_draft: function () {
        var self = service_purchase_invoice;
        self.error_count = 0;
        self.error_count = self.validate_form_for_draft();
        if (self.error_count > 0) {
            return;
        }
        var form_data = GetSaveObj();
        form_data.IsDraft = true;


        form_data.Status = "Draft";
        $(".btnSaveAsDraft , .btnSaveNew").css({ 'display': 'none' });
        $.ajax({
            url: "/Purchase/ServicePurchaseInvoice/SaveAsDraft",
            cache: false,
            type: "POST",
            dataType: "json",
            data: { servicePurchaseInvoice: form_data },
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Purchase invoice saved as draft");
                    window.location = "/Purchase/ServicePurchaseInvoice/"
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                }
            },
            error: function () {//Error Occured 
                app.show_error("Something went wrong");
                $(".btnSaveAsDraft , .btnSaveNew").css({ 'display': 'block' });
            }
        });
    },
    cancel: function () {
        $(".btnSaveAsDraft , .btnSaveNew,.cancel").css({ 'display': 'none' });
        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/Cancel',
            data: {
                ID: $("#ServicePurchaseInvoiceID").val(),
                Table: "PurchaseInvoiceForService"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Invoice cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/ServicePurchaseInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSaveAsDraft , .btnSaveNew,.cancel").css({ 'display': 'block' });
                }
            },
        });

    },

    save: function () {
        var self = service_purchase_invoice;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var form_data = GetSaveObj();
        form_data.IsDraft = false;
        //code below by prama on 5-6-18
        form_data.Status = "Booked";
        $(".btnSaveAsDraft , .btnSaveNew").css({ 'display': 'none' });
        $.ajax({
            url: "/Purchase/ServicePurchaseInvoice/Save",
            cache: false,
            type: "POST",
            dataType: "json",
            data: { servicePurchaseInvoice: form_data },
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Purchase invoice saved successfully");
                    setTimeout(function () { window.location = "/Purchase/ServicePurchaseInvoice/" }, 500);

                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveAsDraft , .btnSaveNew").css({ 'display': 'block' });
                }
            },
            error: function () {//Error Occured 
                app.show_error("Something went wrong");
                $(".btnSaveAsDraft , .btnSaveNew").css({ 'display': 'block' });
            }
        });
    },

    validate_form: function () {
        var self = service_purchase_invoice;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_form_for_draft: function () {
        var self = service_purchase_invoice;
        if (self.rules.draft.length) {
            return form.validate(self.rules.draft);
        }
        return 0;
    },
    rules: {
        draft: [
           //{
           //    elements: "#SupplierID",
           //    rules: [
           //        { type: form.required, message: "Please select Supplier" },
           //        { type: form.non_zero, message: "Please select Supplier" },
           //    ]
           //},
            {
                elements: "#txtTDS",
                rules: [

                    { type: form.positive, message: "Please enter positive value for TDS" },
                ]
            },
            {
                elements: "#txtTDSOnAdvance",
                rules: [

                    { type: form.positive, message: "Please enter positive value for TDS on advance" },
                ]
            },
                 {
                     elements: "#txtDiscount",
                     rules: [
                            { type: form.positive, message: "Invalid less discount" },
                     ]
                 },

            {
                elements: ".tax-invoice-value",
                rules: [
                    { type: form.required, message: "Please enter tax invoice value" },
                    { type: form.positive, message: "Please enter positive value for tax invoice value" },
                ]
            },
             {
                 elements: ".SGST .tax-invoice-value",
                 rules: [
                        { type: form.positive, message: "Invalid tax invoice value" },
                         {
                             type: function (element) {
                                 var sgst_amount = clean($(element).val());
                                 var cgst_amount = clean($(element).closest('tr').next('.CGST').find('.tax-invoice-value').val());

                                 return cgst_amount == sgst_amount
                             }, message: "SGST and CGST Amount must be same  "
                         },
                 ]
             },
                {
                    elements: ".CGST .tax-invoice-value",
                    rules: [
                           { type: form.positive, message: "Invalid tax invoice value" },
                            {
                                type: function (element) {
                                    var sgst_amount = clean($(element).closest('tr').prev('.SGST').find('.tax-invoice-value').val());
                                    var cgst_amount = clean($(element).val());

                                    return cgst_amount == sgst_amount
                                }, message: "SGST and CGST Amount must be same  "
                            },
                    ]
                },
            {
                elements: "#txtInvoiceDate",
                rules: [
                    { type: form.required, message: "Invalid invoice date" },
                    { type: form.past_date, message: "Invalid invoice date" },
                        //{
                        //    type: function (element) {
                        //        var u_date = $(element).val().split('-');
                        //        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);

                        //        var u_date1 = $('#SrDate').val().split('-');
                        //        var used_date1 = new Date(u_date1[2], u_date1[1] - 1, u_date1[0]);

                        //        return used_date.getTime() >= used_date1.getTime();
                        //    }, message: "Invoice date shall be within PO date & Current Date"
                        //}
                ]
            },
            {
                elements: "#txtInvoiceNo",
                rules: [
                    { type: form.required, message: "Invalid invoice number" },
                ]
            },
            //{
            //    elements: "#txtInvoiceTotal",
            //    rules: [
            //        { type: form.non_zero, message: "Invalid invoice total" },
            //         { type: form.required, message: "Invalid invoice total" },
            //         { type: form.positive, message: "Invalid invoice total" },
            //    ]
            //},
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please select atleast one item" },
                ]
            },
            //{
            //    elements: ".txtItemInvoiceQty.included",
            //    rules: [
            //         { type: form.non_zero, message: "Invalid invoice quantity" },
            //         { type: form.required, message: "Invalid invoice quantity" },
            //         { type: form.positive, message: "Invalid invoice quantity" },
            //    ]
            //},
            //{
            //    elements: ".txtItemInvoiceRate.included",
            //    rules: [
            //         { type: form.non_zero, message: "Invalid invoice rate" },
            //         { type: form.required, message: "Invalid invoice rate" },
            //         { type: form.positive, message: "Invalid invoice rate" },
            //    ]
            //},
              {
                  elements: ".txtTaxInvoiceValue",
                  rules: [
                      { type: form.non_zero, message: "Invalid invoice tax value" },
                       { type: form.required, message: "Invalid invoice tax value" },
                       { type: form.positive, message: "Invalid invoice tax value" },
                  ]

              },

              {
                  elements: "#txtNetPayable",
                  rules: [
                      { type: form.positive, message: " Net payable value must be positive" },
                  ]
              },

        ],
        on_submit: [

            {
                elements: "#SupplierID",
                rules: [
                    { type: form.required, message: "Please select Supplier" },
                    { type: form.non_zero, message: "Please select Supplier" },
                ]
            },
            //{
            //    elements: "#txtInvoiceDate",
            //    rules: [
            //         { type: form.required, message: "Invalid invoice date" },
            //        { type: form.past_date, message: "Invalid invoice date" },
            //            {
            //                type: function (element) {
            //                    var u_date = $(element).val().split('-');
            //                    var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);

            //                    var u_date1 = $('#SrDate').val().split('-');
            //                    var used_date1 = new Date(u_date1[2], u_date1[1] - 1, u_date1[0]);

            //                    return used_date.getTime() >= used_date1.getTime();
            //                }, message: "Invoice date shall be within PO date & Current Date"
            //            }

            //    ]
            //},
                 {
                     elements: "#txtDiscount",
                     rules: [
                            { type: form.positive, message: "Invalid less discount" },
                     ]
                 },
            {
                elements: "#txtInvoiceNo",
                rules: [
                    { type: form.required, message: "Invalid invoice number" },
                         {
                             type: function (element) {
                                 var error = false;
                                 var count = clean($('#invoice-count').val());
                                 if (count > 0)
                                     error = true;
                                 return !error;
                             }, message: 'Invoice number has been already entered for this supplier'
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
                            return clean($(element).val()) == clean($("#txtTotalInvoiceValueCalculated").val());
                        }, message: "Invoice total mismatch"
                    },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please select atleast one item" },
                ]
            },
            {
                elements: ".txtItemInvoiceQty.included",
                rules: [
                     { type: form.non_zero, message: "Invalid invoice quantity" },
                     { type: form.required, message: "Invalid invoice quantity" },
                     { type: form.positive, message: "Invalid invoice quantity" },
                ]
            },
            {
                elements: ".txtItemInvoiceRate.included",
                rules: [
                     { type: form.non_zero, message: "Invalid invoice rate" },
                     { type: form.required, message: "Invalid invoice rate" },
                     { type: form.positive, message: "Invalid invoice rate" },
                ]
            },
            {
                elements: "#txtTDS",
                rules: [

                    { type: form.positive, message: "Please enter positive value for TDS" },

                ]
            },
            {
                elements: "#txtTDSOnAdvance",
                rules: [

                    { type: form.positive, message: "Please enter positive value for TDS on advance" },

                ]
            },
            {
                elements: ".tax-invoice-value",
                rules: [
                    { type: form.required, message: "Please enter tax invoice value" },
                    { type: form.positive, message: "Please enter positive value for tax invoice value" },
                ]
            },

             {
                 elements: ".SGST .tax-invoice-value",
                 rules: [
                        { type: form.positive, message: "Invalid tax invoice value" },
                         {
                             type: function (element) {
                                 var sgst_amount = clean($(element).val());
                                 var cgst_amount = clean($(element).closest('tr').next('.CGST').find('.tax-invoice-value').val());

                                 return cgst_amount == sgst_amount
                             }, message: "SGST and CGST Amount must be same  "
                         },
                 ]
             },
                {
                    elements: ".CGST .tax-invoice-value",
                    rules: [
                           { type: form.positive, message: "Invalid tax invoice value" },
                            {
                                type: function (element) {
                                    var sgst_amount = clean($(element).closest('tr').prev('.SGST').find('.tax-invoice-value').val());
                                    var cgst_amount = clean($(element).val());

                                    return cgst_amount == sgst_amount
                                }, message: "SGST and CGST Amount must be same  "
                            },
                    ]
                },

            {
                elements: "#txtNetPayable",
                rules: [
                    { type: form.positive, message: " Net payable value must be positive" },
                ]
            },
        ]
    },
    error_count: 0,
    is_gst_in_array: function (array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == value) {
                return i;
            }
        }
        return -1;
    },
};

function search_in_array(array, type, value) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].tax_percentage == value && array[i].type == type) {
            return i;
        }
    }
    return -1;
}

function GetSaveObj() {
    var obj = {};
    obj.ServicePurchaseInvoiceID = $('#hdnServicePurchaseInvoiceID').val();
    obj.PurchaseNo = $('#txtTransNo').val();
    obj.PurchaseDate = $('#txtTransDate').val();
    obj.SupplierID = $('#SupplierID').val();
    obj.LocalSupplierName = '';
    obj.InvoiceNo = $('#txtInvoiceNo').val();
    obj.InvoiceDate = $('#txtInvoiceDate').val();
    obj.InvoiceAmount = clean($('#txtTotalInvoiceValueCalculated').val());
    obj.DifferenceAmount = clean($('#txtTotalDifferenceCalculated').val());
    obj.AcceptedAmount = $('#AcceptedAmount').val();
    obj.SGSTAmount = $('#SGSTAmount').val();
    obj.CGSTAmount = $('#CGSTAmount').val();
    obj.IGSTAmount = $('#IGSTAmount').val();
    obj.TDS = clean($('#txtTDS').val());
    obj.TDSOnAdvance = clean($('#txtTDSOnAdvance').val());
    obj.NetTDS = clean($('#txtNetTDS').val());
    obj.NetAmountPayable = clean($('#txtNetPayable').val());
    obj.IsValidItemInvoiceValues = true;
    obj.TDSID = clean($('#TDSID').val());
    obj.Discount = clean($('#txtDiscount').val());
    obj.OtherDeductions = clean($('#txtDeductions').val());

    var transItemArr = [];
    $('#item-details-tblContainer table tbody tr .chkItem:checked').each(function () {
        var transItem = {};
        var currRow = $(this).parents('tr');
        var inclusive = false;
        if (clean(currRow.find('.IsInclusiveGST').val()) == 1)
            inclusive = true;
        transItem.SRNID = currRow.find('.hdnSRNID').val();
        transItem.SRNTransID = currRow.find('.hdnSRNTransID').val();
        transItem.ItemID = currRow.find('.chkItem').val();
        transItem.AcceptedQty = clean(currRow.find('.itemAcceptedQty').text());
        transItem.UnMatchedQty = clean(currRow.find('.itemUnMatchedQty').text());
        transItem.PORate = clean(currRow.find('.itemPORate').text());
        transItem.InvoiceQty = clean(currRow.find('.txtItemInvoiceQty').val());
        transItem.InvoiceRate = clean(currRow.find('.txtItemInvoiceRate').val());
        transItem.InvoiceAmount = clean(currRow.find('.txtItemInvoiceValue').val());
        transItem.Difference = clean(currRow.find('.itemDiffValue').val());
        transItem.SGSTPercent = clean(currRow.find('.SGSTPercent').val());
        transItem.IGSTPercent = clean(currRow.find('.IGSTPercent').val());
        transItem.CGSTPercent = clean(currRow.find('.CGSTPercent').val());
        transItem.InvoiceGSTPercent = clean(currRow.find('.gstPercentage').val());
        var next_row = $(currRow).next("tr");
        transItem.ServiceLocationID = $(next_row).find('.location-id').val();
        transItem.DepartmentID = $(next_row).find('.department-id').val();
        transItem.EmployeeID = $(next_row).find('.employee-id').val();
        transItem.CompanyID = $(next_row).find('.company-id').val();
        transItem.ProjectID = $(next_row).find('.project-id').val();
        transItem.InclusiveGST = inclusive;

        transItem.Remarks = currRow.find('.txtItemRemarks').val();

        transItemArr.push(transItem);
    });

    var taxDetailsArr = [];
    $('#tax-details tbody tr').each(function () {
        var taxDetail = {};
        var currRow = $(this);

        taxDetail.Particular = currRow.find('.tax-particular').val();
        taxDetail.TaxPercentage = clean(currRow.find('.tax-percentage').text());
        taxDetail.POValue = clean(currRow.find('.po-value').text());
        taxDetail.InvoiceValue = clean(currRow.find('.tax-invoice-value').val());
        taxDetail.DifferenceValue = clean(currRow.find('.tax-diff-value').val());
        taxDetail.Remarks = currRow.find('.tax-remarks').val();

        taxDetailsArr.push(taxDetail);
    });

    obj.InvoiceTransItems = transItemArr;
    obj.TaxDetails = taxDetailsArr;
    return obj;
}
function GetDiffVal(from, to, isDecimalPlaces, fixedTo) {

    if (isNaN(from))
        from = 0;
    if (isNaN(to))
        to = 0;

    var diff = from - to;

    if (isDecimalPlaces) {
        if (fixedTo != undefined && fixedTo != null)
            diff = diff.toFixed(fixedTo);
    }
    return diff;
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
function CalculateTotals() {

    CalculateInvoiceTotal();
    CalculateTotalDifference();
    CalculateNetPayable();
    Calculategst();
    Calculate_Net_Value_With_Discount_And_All();
}
function Calculategst() {
    $('#tbl-item-details tbody tr').each(function () {

        igst = clean($(this).find(".IGSTPercent").val());
        sgst = clean($(this).find(".SGSTPercent").val());
        cgst = clean($(this).find(".CGSTPercent").val());
        gstTotal = igst + cgst + sgst;
        $(this).find(".gstPercentage").val(gstTotal);

    });
}

function CalculateInvoiceTotal() {
    var totInvoiceVal = 0;
    var total_accepted_value = 0;
    var is_inclusive_gst = 0;
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());

    $('#item-details .chkItem:checked').closest('tr').each(function () {
        var currVal = clean($(this).find('.txtItemInvoiceValue').val());
        if (!isNaN(currVal) && currVal != '')
            totInvoiceVal += currVal;
        total_accepted_value += clean($(this).find('.itemAcceptedValue').val());

    });
    $("#AcceptedAmount").val(total_accepted_value);
    var SGSTAmount = 0;
    var IGSTAmount = 0;
    var CGSTAmount = 0;
    var type;
    $('#tax-details tbody tr').each(function () {
        var currVal = clean($(this).find(".tax-invoice-value.gst-extra").val());
        if (!isNaN(currVal) && currVal != '' && is_inclusive_gst == 0) {
            totInvoiceVal += parseFloat(currVal);
        }
        type = $(this).find(".tax-particular").val();
        if (type == "SGST") {
            SGSTAmount += currVal;
        } else if (type == "CGST") {
            CGSTAmount += currVal;
        } else if (type == "IGST") {
            IGSTAmount += currVal;
        }
    });
    totInvoiceVal = totInvoiceVal - discount + deductions;

    $("#SGSTAmount").val(SGSTAmount);
    $("#CGSTAmount").val(CGSTAmount);
    $("#IGSTAmount").val(IGSTAmount);
    $('#txtTotalInvoiceValueCalculated').val(totInvoiceVal);
}

function CalculateTotalDifference() {
    var totDiffVal = 0;
    $('#item-details .chkItem:checked').parents('tr').find('.itemDiffValue').each(function () {
        var currVal = clean($(this).val());
        if (!isNaN(currVal) && currVal != '' && currVal != '0')
            totDiffVal += parseFloat(currVal);
    });

    $('#tax-details .tax-diff-value').each(function () {
        var currVal = clean($(this).val());
        if (!isNaN(currVal) && currVal != '' && currVal != '0')
            totDiffVal += parseFloat(currVal);
    });

    $('#txtTotalDifferenceCalculated').val(totDiffVal);
}

function CalculateNetPayable() {
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());
    //Invoice value - (TDS + TDS on advance)
    var totalInvoice = clean($('#txtTotalInvoiceValueCalculated').val());
    var tds = clean($('#txtTDS').val());
    var advanceTDS = clean($('#txtTDSOnAdvance').val());
    var oldadvanceTDS = clean($('#TDSOnAdvance').val());
    var total;
    if (tds > oldadvanceTDS) {
        $('#txtTDSOnAdvance').val(oldadvanceTDS)
    }
    if (advanceTDS > tds && tds > 0) {
        $('#txtTDSOnAdvance').val(tds)
        total = 0;
    }
    else {
        total = advanceTDS - tds;
    }
    if (total < 0) {
        total = total * -1;
    }
    if (clean($('#txtTDS').val()) == 0) {
        $("#txtNetTDS").val(0);
    } else {
        $("#txtNetTDS").val(total);
    }
    var netPayable = isNaN(totalInvoice) ? 0 : totalInvoice
    netPayable = totalInvoice + deductions;
    $('#txtNetPayable').val(netPayable);
}

function Calculate_Net_Value_With_Discount_And_All() {
    var discount = clean($('#txtDiscount').val());
    var deductions = clean($('#txtDeductions').val());
    //Invoice value - (TDS + TDS on advance)
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
    $('.tablescroll #tax-details tr td .tax-invoice-value.gst-extra').each(function () {
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
    else if (tds > 0) {
        total = advanceTDS - tds;
    }
    else {
        total = 0;
    }
    if (total < 0) {
        total = total * -1;
    }
    if (clean($('#txtTDS').val()) == 0) {
        total = 0;
    }
    $("#txtNetTDS").val(total);
    var netPayable = isNaN(totalInvoice) ? 0 : totalInvoice
    var netPayables = netPayable + deductions - discount;
    $('#DummyInvoiceTotal').val(netPayable);
    $('#txtNetPayable').val(netPayables);
}















