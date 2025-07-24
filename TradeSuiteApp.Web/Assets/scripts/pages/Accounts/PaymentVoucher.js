var $currObj;
var select_table;
var freeze_header;
voucherCRUD = {
    init: function () {
        var self = voucherCRUD;

        supplier.supplier_list('Payment');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#dropPayment",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });

        freeze_header = $("#payment-voucher-items-list").FreezeHeader();
        self.voucherCreateAndUpdate();
        self.bind_events();
        $('#BankACNo').hide();
        self.count();
    },

    details: function () {
        var self = voucherCRUD;
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        freeze_header = $("#payment-voucher-items-list").FreezeHeader();
    },

    print: function () {
        var self = voucherCRUD;
        $.ajax({
            url: '/Accounts/PaymentVoucher/Print',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    SignalRClient.print.text_file(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },

    printpdf: function () {
        var self = voucherCRUD;
        $.ajax({
            url: '/Reports/Accounts/PaymentVoucherPrintPdf',
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

    list: function () {
        var self = voucherCRUD;
        $('#tabs-paymentvoucher').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-paymentvoucher":
                $list = $('#savedpayment-list');
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

            var url = "/Accounts/PaymentVoucher/GetPaymentVoucherList?type=" + type;
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

                    { "data": "VoucherNumber", "className": "VoucherNumber" },
                    { "data": "VoucherDate", "className": "VoucherDate" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    {
                        "data": "Amount", "searchable": false, "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/PaymentVoucher/Details/" + Id);

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
        var self = voucherCRUD;
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#btnOKSupplier").on('click', self.select_supplier);
        $("#dropPayment").on("change", self.get_bank_name);
        $('body').on('ifChanged', '.include', self.include_item);
        $('.btnSaveAndPost, .btnSaveASDraft').on('click', self.on_save);
        $('.txtPayNow').on('keyup', self.calculate_sum);
        $("body").on("ifChanged", "#OtherPayment", self.show_other_payments);
    },

    show_other_payments: function () {
        var self = voucherCRUD;
        if ($("#OtherPayment").prop('checked') == true) {
            $(".hide-show-other-payments").removeClass("uk-hidden");

        } else {
            $(".hide-show-other-payments").addClass("uk-hidden");
        }
    },
    calculate_sum: function () {
        var sum = 0;
        $('.txtPayNow').each(function () {
            sum += clean($(this).val());
        });

        $('#TotPayNow').html(sum.toFixed(2));
        $('#NetAmt').html(sum.toFixed(2));
    },
    save: function (data, url, location) {

        var self = voucherCRUD;

        $(".btnSaveASDraft, .btnSaveAndPost").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {

                    app.show_notice(response.Message);

                    setTimeout(function () {
                        window.location = location;
                    }, 1000);

                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSaveAndPost").css({ 'display': 'block' });

                }
            }
        });
    },
    on_save: function () {

        var self = voucherCRUD;
        var data = self.GetSaveObj();

        var location = "/Accounts/PaymentVoucher/Create";
        var url = '/Accounts/PaymentVoucher/Save';

        if ($(this).hasClass("btnSaveASDraft")) {
            data.IsDraft = true;
            url = '/Accounts/PaymentVoucher/SaveAsDraft'
            self.error_count = self.validateForm();
        } else {
            self.error_count = self.validateForm();

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

    GetSaveObj: function () {

        var self = voucherCRUD;
        var obj = {};
        if ($("#dropPayment option:selected").text().toUpperCase() == "CASH") {
            obj.PaymentMode = "Cash";
        }
        else {
            obj.PaymentMode = "Bank";
        }
        obj.ID = $('#ID').val();
        obj.VoucherNo = $('#txtVoucherNo').val();
        obj.VoucherDateStr = $('#txtVoucherDate').val();
        obj.SupplierID = $("#SupplierID").val();
        obj.AccountNumber = $('#txtAccountNumber').val();
        obj.ReferenceNumber = $('#txtRefernceNumber').val();
        obj.Description = $('#txtRemarks').val();
        obj.ReceiverBankID = $("#ReceiverBankID").val(),
            obj.BankInstrumentNumber = $("#BankInstrumentNumber").val(),
            obj.ChecqueDate = $("#ChecqueDate").val(),
            obj.Bankcharges = $("#Bankcharges").val(),

            $currObj.SaveType = 'save';
        if ($('#Bank option:selected').text() == "Select") {
            obj.BankName = "";
        }
        else {
            obj.BankName = $('#Bank option:selected').text();
        }
        //if ($("#ReceiverBankName option:selected").text() == "Select") {
        //    obj.ReceiverBankName = "";
        //}
        //else {
        //    obj.ReceiverBankName = $('#ReceiverBankName option:selected').text();;
        //}
        obj.PaymentTypeId = $('#dropPayment').val();
        obj.SaveType = $currObj.SaveType;

        var unProcessedItems = [];
        var paynow;
        $('#unSettledPurchaseInvoiceTblContainer #tblUnSettledPurchaseInvoice tbody tr:not(:last)').each(function () {
            $currRow = $(this);
            var unProcessedItem = {};
            unProcessedItem.PayableID = $currRow.find('.hdnPayableID').val();
            //unProcessedItem.Type = $currRow.find('.hdnType').val();
            unProcessedItem.Type = $currRow.find('.hdnDocType').val();
            unProcessedItem.Number = $currRow.find('td:eq(4)').html();
            unProcessedItem.DateStr = $currRow.find('td:eq(5)').html();
            unProcessedItem.Amount = $currRow.find('td:eq(6)').html();
            //unProcessedItem.Balance = $currRow.find('td:eq(4)').html();
            unProcessedItem.AmountToBePaid = clean($currRow.find('.txtAmtToPaid').val());
            unProcessedItem.Narration = $currRow.find('.Narrations').val();

            paynow = clean($currRow.find('.txtPayNow').val());
            unProcessedItem.PayNow = paynow;
            // unProcessedItem.PayNow =clean( $currRow.find('.txtPayNow').val());
            /// unProcessedItem.OriginalAmount = clean($currRow.find('.txtOriginalAmt').val());

            unProcessedItem.OriginalAmount = clean($currRow.find('.TotPayNow').val());

            unProcessedItem.AdvanceID = $currRow.find('.hdnAdvanceID').val();
            unProcessedItem.DebitNoteID = $currRow.find('.hdnDebitNoteID').val();
            unProcessedItem.CreditNoteID = $currRow.find('.hdnCreditNoteID').val();
            unProcessedItem.IRGID = $currRow.find('.hdnIRGID').val();
            unProcessedItem.PaymentReturnVoucherTransID = $currRow.find('.hdnPaymentReturnVoucherTransID').val();
            unProcessedItem.CreatedDateStr = $currRow.find('.hdnCreatedDate').val();
            unProcessedItem.DueDateStr = $currRow.find('.hdnDueDate').val();
            unProcessedItems.push(unProcessedItem);
        });

        obj.UnProcessedItems = unProcessedItems;


        return obj;
    },

    include_item: function () {
        var self = voucherCRUD;
        var row = $(this).closest('tr');
        var sum = 0;
        if ($(this).is(":checked")) {
            $(row).addClass("included");
            $(row).find('.txtPayNow').val($(row).find('.txtAmtToPaid').val());
            $('.txtPayNow').each(function () {
                sum += clean($(this).val());
            });

            $('#TotPayNow').html(sum.toFixed(2));
            $('#NetAmt').val(sum.toFixed(2));
        } else {
            $(row).removeClass("included");
            $(row).find('.txtPayNow').val(0);
            $('.txtPayNow').each(function () {
                sum += clean($(this).val());
            });
            $('#TotPayNow').html((sum).toFixed(2));
            $('#NetAmt').val(sum.toFixed(2));
        }
        self.count();
    },
    get_bank_name: function () {
        var self = voucherCRUD;
        var mode;
        var Module = "Payment";
        if ($("#dropPayment option:selected").text() == "Select") {
            mode = "";
        }
        else if ($("#dropPayment option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }
        $.ajax({
            url: '/Accounts/PaymentVoucher/GetSerialNo',
            data: {
                Mode: mode
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#txtVoucherNo").val(response.data);
                }
            }
        });
        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                ModuleName: Module,
                Mode: mode

            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#Bank").html("");
                var html = ""; //"<option value =''>Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'  data-creditBalance='" + record.CreditBalance + "'>" + record.BankName + "</option>";
                });
                $("#Bank").append(html);
            }
        });
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetPaymentSupplierForAutoComplete',
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
        $("#SupplierName").val(item.name);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        if (item.accountNo != "") {
            $('#BankACNo').show();
            $('#BankACNo').val("BankAccountName: " + item.accountNo);
        }
        else {
            $('#BankACNo').hide();
        }

        var self = voucherCRUD;
        self.SupplierChanged($("#SupplierID").val());
        $("#VoucherAmt").val(0);
        $("#NetAmt").val(0);
        $("#DecimalPlaces").val(item.decimalplaces);
        $("#CurrencyID").val(item.currencyid);
        $("#CurrencyName").val(item.currencyname);
        $("#CurrencyCode").val(item.CurrencyCode);
        $("#CurrencyConversionRate").val(item.currencyconversionrate);

    },
    select_supplier: function () {
        var self = voucherCRUD;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var BankACNo = $(row).find(".BankACNo").val();
        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyName = $(row).find(".CurrencyName").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        var DecimalPlaces = $(row).find(".DecimalPlaces").val();
        var newSupplierID = ID;
        $currRow = row;
        if (BankACNo != "") {
            $('#BankACNo').show();
            $('#BankACNo').val("BankAccountName: " + BankACNo);
        }
        else {
            $('#BankACNo').hide();
        }
        if (newSupplierID != $currObj.SelectedSupplierID) {
            $currObj.SelectedSupplierID = newSupplierID;
            $currObj.SelectedSupplierName = Name;
            $('#SupplierName').val($currObj.SelectedSupplierName);
            $('#SupplierID').val(newSupplierID);
            self.SupplierChanged();
            $("#VoucherAmt").val(0);
            $("#NetAmt").val(0);
            $("#DecimalPlaces").val(DecimalPlaces);
            $("#CurrencyID").val(CurrencyID);
            $("#CurrencyName").val(CurrencyName);
            $("#CurrencyCode").val(CurrencyCode);
            $("#CurrencyExchangeRate").val(CurrencyConversionRate);
        }
        UIkit.modal($('#select-supplier')).hide();
    },

    SupplierChanged: function (supplierID) {
        if (typeof supplierID === "undefined" || supplierID === null) {
            supplierID = $currObj.SelectedSupplierID;
        }
        else supplierID = supplierID;

        var callBack = function (response) {
            var $response = $(response);
            app.format($response);
            $("#unSettledPurchaseInvoiceTblContainer").html($response);
            freeze_header = $("#tblUnSettledPurchaseInvoice").FreezeHeader();
            voucherCRUD.count();
            $('.txtPayNow').on('keyup', function (e) {
                var row = $(this).closest("tr");
                if (clean($(this).val()) != 0) {
                    $(row).find('.include').prop("checked", true);
                    $(row).find('.include').closest('div.icheckbox_md').addClass('checked');
                }
                var sum = 0;

                $('.txtPayNow').each(function () {
                    sum += clean($(this).val());
                });

                $('#TotPayNow').html(sum.toFixed(2));
                $('#NetAmt').val(sum.toFixed(2));
            });
        };
        var data = { supplierID: supplierID };
        AjaxRequest('/Accounts/PaymentVoucher/GetUnSettledPurchaseInvoices', data, 'GET', callBack);
    },
    count: function () {
        var count = $('#tblUnSettledPurchaseInvoice tbody tr').length - 1;
        $('#item-count').val(count);
    },

    process_item: function () {
        var self = voucherCRUD;
        self.error_count = self.validate_process();
        if (self.error_count > 0) {
            return;
        }

        var payment_amount = clean($("#VoucherAmt").val());
        var DirectPayableTotalAmt = 0;
        var total_pay_now = clean($("#VoucherAmt").val());
        var CurrencyExchangeRate = clean($("#CurrencyExchangeRate").val());
        var total_balance_to_be_paid = 0;
        var row;
        $("#NetAmt").val(payment_amount);
        $("#LocalNetAmt").val(payment_amount * CurrencyExchangeRate);
        $("#tblUnSettledPurchaseInvoice tbody tr #TotPayNow").text('');

        $("#tblUnSettledPurchaseInvoice tbody tr:not(:contains(DirectPayable))").each(function () {
            row = $(this).closest("tr");
            var invoiceAmt = clean($(row).find(".invoiceAmt").text());
            $(row).find(".txtPayNow").val(0);
        });

        $("#tblUnSettledPurchaseInvoice tbody tr:contains('DirectPayable')").each(function () {
            row = $(this).closest("tr");
            DirectPayableTotalAmt += clean($(row).find(".invoiceAmt").val());
        });

        payment_amount = payment_amount - DirectPayableTotalAmt;

        $("#tblUnSettledPurchaseInvoice tbody tr:not(:contains(DirectPayable))").each(function () {
            row = $(this).closest("tr");
            var invoiceAmt = clean($(row).find(".txtAmtToPaid").val());
            if (payment_amount == 0) {
                $(row).removeClass("included");
                $(row).find(":checkbox").prop("checked", false);
                $(row).find(":checkbox").closest('div').removeClass("checked");
            }
            else if (payment_amount > invoiceAmt) {
                $(row).addClass("included");
                $(row).find(":checkbox").prop("checked", true);
                $(row).find(":checkbox").closest('div').addClass("checked");
                $(row).find(".txtPayNow").val(invoiceAmt);
                payment_amount = payment_amount - invoiceAmt;
            }
            else {
                $(row).addClass("included");
                $(row).find(":checkbox").prop("checked", true);
                $(row).find(":checkbox").closest('div').addClass("checked");
                $(row).find(".txtPayNow").val(payment_amount);
                payment_amount = 0
            }
        });
        $("#tblUnSettledPurchaseInvoice tbody tr #TotPayNow").text(total_pay_now);

        self.count();
    },


    validateForm: function () {
        var self = voucherCRUD;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = voucherCRUD;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_add_other_payment: function () {
        var self = voucherCRUD;
        if (self.rules.on_add_other_payment.length > 0) {
            return form.validate(self.rules.on_add_other_payment);
        }
        return 0;
    },

    validate_process: function () {
        var self = voucherCRUD;
        if (self.rules.on_process.length > 0) {
            return form.validate(self.rules.on_process);
        }
        return 0;
    },


    rules: {

        on_blur: [],

        on_draft: [
            {
                elements: "#SupplierName",
                rules: [
                    { type: form.required, message: "Please choose a valid Supplier" },
                ]
            },
            {
                elements: "#TotPayNow",
                rules: [
                    { type: form.positive, message: "Invalid total pay now" },

                ]
            },
            {
                elements: (".txtPayNow.mask-positive-currency"),
                rules: [
                    {
                        type: function (element) {
                            var PayNow = clean($(element).val());
                            var Balance = clean($(element).closest('tr').find('.txtAmtToPaid').val());
                            return Balance >= PayNow;
                        }, message: "Amount should not exceed  orginal amount"
                    },
                ]
            },
            {
                elements: (".txtPayNow.mask-negative-currency"),
                rules: [
                    {
                        type: function (element) {
                            var PayNow = -1 * clean($(element).val());
                            var Balance = clean($(element).closest('tr').find('.txtAmtToPaid').val());
                            return Balance >= PayNow;
                        }, message: "Amount should not exceed orginal amount"
                    },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    {
                        type: function (element) {

                            var error = true;

                            if (clean($("#item-count").val()) == 0) {
                                error = false;
                            }
                            $('.txtPayNow.mask-positive-currency').each(function () {
                                if (clean($(this).val()) > 0) {
                                    error = false;
                                }
                            });

                            return !error;
                        }, message: "Amount for atleast one item must be positive  number"
                    },
                ]
            },

        ],

        on_add_other_payment: [
            {
                elements: "#SupplierName",
                rules: [
                    { type: form.required, message: "Please choose a valid Supplier" },
                ]
            },
            {
                elements: "#Narration",
                rules: [
                    { type: form.required, message: "Invalid Narration" },

                ]
            },

            {
                elements: "#DocumentNo",
                rules: [
                    { type: form.required, message: "Invalid DocumentNo" },

                ]
            },
            {
                elements: "#Date",
                rules: [
                    { type: form.required, message: "Invalid Date" },

                ]
            },
            {
                elements: ("#Amount"),
                rules: [
                    { type: form.non_zero, message: "Invalid Amount" },
                    { type: form.required, message: "Invalid Amount" },
                ]
            },
        ],

        on_process: [
            {
                elements: "#VoucherAmt",
                rules: [
                    { type: form.positive, message: "Invalid VoucherAmt" },
                    {
                        type: function (element) {
                            var TotBalance = clean($("#TotBalance").text());
                            var VoucherAmt = clean($("#VoucherAmt").val());
                            return TotBalance >= VoucherAmt;
                        }, message: 'VoucherAmt Greater than the Total Balance Payable'
                    },
                ]
            },
            {
                elements: "#SupplierName",
                rules: [
                    { type: form.required, message: "Please choose a valid Supplier" },
                ]
            },
        ],

        on_save_reconsiled_date: [
            {
                elements: "#ReconciledDate",
                rules: [
                    { type: form.required, message: "Please enter date" }
                ]
            },
        ],

        on_submit: [
            {
                elements: "#SupplierName",
                rules: [
                    { type: form.required, message: "Please choose a valid Supplier" },
                ]
            },
            {
                elements: "#Bank",
                rules: [
                    { type: form.required, message: "Please choose a valid Bank Name" },
                ]
            },
            {
                elements: "#ReceiverBankID",
                rules: [
                    { type: form.required, message: "Please choose a valid  Receiver Bank Name" },
                ]
            },
            {
                elements: "#dropPayment",
                rules: [
                { type: form.required, message: "Please choose a mode of payment" },
                 {
                     type: function (element) {
                         var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                         var NetAmt = clean($("#TotPayNow").text());
                         var PaymentMode = $("#dropPayment Option:Selected ").text().toUpperCase();
                         var error = false;
                         if (CashPaymentLimit < NetAmt && PaymentMode == 'CASH') {
                             error = true;
                         }
                         return !error;
                     }, message: 'Please choose Another Payment Mode'
                    },

                ]

            },
            {
                elements: "#ChecqueDate",
                rules: [
                    {
                        type: function (element) {
                            var PaymentMode = $("#dropPayment option:selected").text().toUpperCase();
                            if (PaymentMode === 'CHEQUE') {
                                return $(element).val().trim() !== "";
                            }
                            return true;
                        },
                        message: "Please select a valid Cheque Date"
                    }
                ]
            },
           



            {
                elements: "#TotPayNow",
                rules: [
                { type: form.positive, message: "Invalid total pay now" },
                //{
                //    type: function (element) {
                //        var CreditLimit = $("#Bank option:selected").data('creditbalance');
                //        CreditLimit = typeof CreditLimit === "undefined" ? 0 : CreditLimit;
                //        var NetAmt = clean($("#TotPayNow").text());

                //        return CreditLimit >= NetAmt;
                //    }, message: 'Total exceeds credit limit'
                //},
                ]
            },
            {
                elements: (".txtPayNow.mask-positive-currency"),
                rules: [
                    {
                        type: function (element) {
                            var PayNow = clean($(element).val());
                            var Balance = clean($(element).closest('tr').find('.txtAmtToPaid').val());
                            return Balance >= PayNow;
                        }, message: "Amount should not exceed  orginal amount"
                    },
                ]
            },
            {
                elements: (".txtPayNow.mask-negative-currency"),
                rules: [
                    {
                        type: function (element) {
                            var PayNow = -1 * clean($(element).val());
                            var Balance = clean($(element).closest('tr').find('.txtAmtToPaid').val());
                            return Balance >= PayNow;
                        }, message: "Amount should not exceed orginal amount"
                    },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                {
                    type: function (element) {

                        var error = true;

                        if (clean($("#item-count").val()) == 0) {
                            error = false;
                        }
                        $('.txtPayNow.mask-positive-currency').each(function () {
                            if (clean($(this).val()) > 0) {
                                error = false;
                            }
                        });

                        return !error;
                    }, message: "Amount for atleast one item must be positive  number"
                },
                ]
            },

            {
                elements: "#VoucherAmt",
                rules: [
                { type: form.positive, message: "Invalid VoucherAmt" },
                {
                    type: function (element) {
                        var TotPayNow = clean($("#TotPayNow").text());
                        var VoucherAmt = clean($("#VoucherAmt").val());
                        return TotPayNow == VoucherAmt;
                    }, message: 'Mismatch with TotalPayNow'
                },
                ]
            },
        ]

    },

    voucherCreateAndUpdate: function () {
        $currObj = this;
        $currObj.SelectedSupplierID = 0;
        $currObj.SelectedSupplierName = '';
        $currObj.Location = '';
        $currObj.StateID = '';
        $currObj.SaveType = '';
    }
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