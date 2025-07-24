$(function () {
    reports.bind_events();//USED TO CALL BINDING EVENT
    //reports.init();
    //report.Account_list();
});
var item_select_table;

reports = {
    bind_events: function () {
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': reports.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', reports.set_item);

        $('#AccountName').on("change", reports.clear_item);
        $("#report-filter-submit").on("click", reports.get_report_view);

        $("#report-filter-form").on("submit", function () { return false; });

        $('.report_type').on('ifChanged', reports.show_report_type);
        $('.summary').on('ifCahnge', reports.show_advancereturn_type);

        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': reports.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', reports.set_supplier_details);
        $.UIkit.autocomplete($('#paymentvoucher-voucher-autocomplete'), { 'source': reports.get_voucher, 'minLength': 1 });
        $('#paymentvoucher-voucher-autocomplete').on('selectitem.uk.autocomplete', reports.set_voucher);

        $.UIkit.autocomplete($('#paymentvoucher-voucherTo-autocomplete'), { 'source': reports.get_voucherTo, 'minLength': 1 });
        $('#paymentvoucher-voucherTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_voucherTo);

        $.UIkit.autocomplete($('#paymentvoucher-document-autocomplete'), { 'source': reports.get_document, 'minLength': 1 });
        $('#paymentvoucher-document-autocomplete').on('selectitem.uk.autocomplete', reports.set_document);
        $.UIkit.autocomplete($('#paymentvoucher-bank-autocomplete'), { 'source': reports.get_bank_details, 'minLength': 1 });
        $('#paymentvoucher-bank-autocomplete').on('selectitem.uk.autocomplete', reports.set_bank_details);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': reports.get_employees, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', reports.set_employees);
        $.UIkit.autocomplete($('#allitem-autocomplete'), { 'source': reports.get_allitems, 'minLength': 1 });
        $("#FromSupplierRange").on("change", reports.get_to_range);
        $('#allitem-autocomplete').on('selectitem.uk.autocomplete', reports.set_allitem);
        $.UIkit.autocomplete($('#generalledger-transtype-autocomplete'), { 'source': reports.get_transtype, 'minLength': 1 });
        $('#generalLedger-transtype-autocomplete').on('selectitem.uk.autocomplete', reports.set_transtype);
        $('#Refresh').on('click', reports.refresh);
        ///////////////////////
        $.UIkit.autocomplete($('#accountnumber-autocomplete'), { 'source': reports.get_Account, 'minLength': 1 });
        $('#accountnumber-autocomplete').on('selectitem.uk.autocomplete', reports.set_Account);
        $.UIkit.autocomplete($('#accountnumberto-autocomplete'), { 'source': reports.get_AccountTo, 'minLength': 1 });
        $('#accountnumberto-autocomplete').on('selectitem.uk.autocomplete', reports.set_AccountTo);
        $.UIkit.autocomplete($('#accountname-autocomplete'), { 'source': reports.get_Account, 'minLength': 1 });
        $('#accountname-autocomplete').on('selectitem.uk.autocomplete', reports.set_Account);
        $.UIkit.autocomplete($('#accountnameto-autocomplete'), { 'source': reports.get_AccountTo, 'minLength': 1 });
        $('#accountnameto-autocomplete').on('selectitem.uk.autocomplete', reports.set_AccountTo);
        ////////////////////////
        $('.voucher-summary').on('ifChanged', reports.show_voucher_report_item_type);
        $("#FromSupplierRange").on("change", reports.get_to_range);
        $("#DocTypeFromRange").on("change", reports.get_doc_to_range);
        $("#TransTypeFromRange").on("change", reports.get_transtype_to_range);
        $("#AccountCodeFromRange").on("change", reports.get_acc_name_to_range);
        $("#EmployeeFromRange").on("change", reports.get_employee_name_to_range);
        $("#CompanyFromRange").on("change", reports.get_company_to_range);
        $("#FromSupplierCatRange").on("change", reports.get_to_suppliercatrange);

        $.UIkit.autocomplete($('#debitnoteno-autocomplete'), { 'source': reports.get_supplierDebitNoteNo, 'minLength': 1 });
        $('#debitnoteno-autocomplete').on('selectitem.uk.autocomplete', reports.set_supplierDebitNoteNo);
        $.UIkit.autocomplete($('#debitnotenoTo-autocomplete'), { 'source': reports.get_supplierDebitNoteNoTo, 'minLength': 1 });
        $('#debitnotenoTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_supplierDebitNoteNoTo);
        $.UIkit.autocomplete($('#advancepymtno-autocomplete'), { 'source': reports.get_advancepaymentno, 'minLength': 1 });
        $('#advancepymtno-autocomplete').on('selectitem.uk.autocomplete', reports.set_advancepaymentno);

        $.UIkit.autocomplete($('#advance-return-voucherno-autocomplete'), { 'source': reports.get_advance_return_voucher_no, 'minLength': 1 });
        $('#advance-return-voucherno-autocomplete').on('selectitem.uk.autocomplete', reports.set_advance_return_voucher_no);
        $.UIkit.autocomplete($('#advance-return-vouchernoTo-autocomplete'), { 'source': reports.get_advance_return_voucher_noTo, 'minLength': 1 });
        $('#advance-return-vouchernoTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_advance_return_voucher_noTo);

        $.UIkit.autocomplete($('#advancepymtnoTo-autocomplete'), { 'source': reports.get_advancepaymentnoTo, 'minLength': 1 });
        $('#advancepymtnoTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_advancepaymentnoTo);
        $('.Advance_Payment_Summary').on('ifChanged', reports.advance_payment_show_report_item_type);
        $('.advance_payment_report_type').on('ifChanged', reports.advance_payment_show_report_type);

        $('.party_type').on('ifChanged', reports.show_party_type);
        $('.supplier_debit_note_type').on('ifChanged', reports.supplier_debit_note_show_report_type);
        $('.supplier_debit_note_summary').on('ifChanged', reports.supplier_debit_note_show_report_item_type);

        //  $.UIkit.autocomplete($('#advancereturnno-autocomplete'), { 'source': reports.get_AdvanceReturnNo, 'minLength': 1 });
        // $('#advancereturnno-autocomplete').on('selectitem.uk.autocomplete', reports.set_AdvanceReturnNo);

        $.UIkit.autocomplete($('#advancereturn-voucherno-autocomplete'), { 'source': reports.get_AdvanceReturnNo, 'minLength': 1 });
        $('#advancereturn-voucherno-autocomplete').on('selectitem.uk.autocomplete', reports.set_AdvanceReturnNo);
        $.UIkit.autocomplete($('#advancereturn-vouchernoTo-autocomplete'), { 'source': reports.get_AdvanceReturnNoTo, 'minLength': 1 });
        $('#advancereturn-vouchernoTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_AdvanceReturnNoTo);

        $('.adv_return_report_type').on('ifChanged', reports.adv_return_show_report_type);
        $('.adv_return_summary').on('ifChanged', reports.adv_return_show_report_item_type);
        $.UIkit.autocomplete($('#FundTransNoFromAutocomplete'), { 'source': reports.get_FundTransNo, 'minLength': 1 });
        $('#FundTransNoFromAutocomplete').on('selectitem.uk.autocomplete', reports.set_FundTransNo);
        $.UIkit.autocomplete($('#FundTransNoToAutocomplete'), { 'source': reports.get_FundTransNoTo, 'minLength': 1 });
        $('#FundTransNoToAutocomplete').on('selectitem.uk.autocomplete', reports.set_FundTransNoTo);

        $.UIkit.autocomplete($('#Cheque-Status-TransNoFrom-Autocomplete'), { 'source': reports.get_ChequeTransNo, 'minLength': 1 });
        $('#Cheque-Status-TransNoFrom-Autocomplete').on('selectitem.uk.autocomplete', reports.set_ChequeTransNo);
        $.UIkit.autocomplete($('#Cheque-Status-TransNoTo-Autocomplete'), { 'source': reports.get_ChequeTransNoTo, 'minLength': 1 });
        $('#Cheque-Status-TransNoTo-Autocomplete').on('selectitem.uk.autocomplete', reports.set_ChequeTransNoTo);

        $.UIkit.autocomplete($('#Cheque-InstrumentNo-Autocomplete'), { 'source': reports.get_ChequeInstrumentNo, 'minLength': 1 });
        $('#Cheque-InstrumentNo-Autocomplete').on('selectitem.uk.autocomplete', reports.set_ChequeInstrumentNo);

        $.UIkit.autocomplete($('#ledger-accountname-autocomplete'), { 'source': reports.get_accountname, 'minLength': 1 });
        $('#ledger-accountname-autocomplete').on('selectitem.uk.autocomplete', reports.set_accountname);

        $.UIkit.autocomplete($('#accountcodefrom-autocomplete'), { 'source': reports.get_accountcodefrom, 'minLength': 1 });
        $('#accountcodefrom-autocomplete').on('selectitem.uk.autocomplete', reports.set_accountcodefrom);
        $.UIkit.autocomplete($('#accountcodeto-autocomplete'), { 'source': reports.get_accountcodeto, 'minLength': 1 });
        $('#accountcodeto-autocomplete').on('selectitem.uk.autocomplete', reports.set_accountcodeto);

        $.UIkit.autocomplete($('#ledgeraccountcodefrom-autocomplete'), { 'source': reports.get_ledger_accountcodefrom, 'minLength': 1 });
        $('#ledgeraccountcodefrom-autocomplete').on('selectitem.uk.autocomplete', reports.set_ledger_accountcodefrom);
        $.UIkit.autocomplete($('#ledgeraccountcodeto-autocomplete'), { 'source': reports.get_ledger_accountcodeto, 'minLength': 1 });
        $('#ledgeraccountcodeto-autocomplete').on('selectitem.uk.autocomplete', reports.set_ledger_accountcodeto);

        $.UIkit.autocomplete($('#refinvoicedocno-autocomplete'), { 'source': reports.get_refinvoicedocno, 'minLength': 1 });
        $('#refinvoicedocno-autocomplete').on('selectitem.uk.autocomplete', reports.set_refinvoicedocno);

        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': reports.get_customername, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', reports.set_customername);
        $.UIkit.autocomplete($('#sales-customercodefrom-autocomplete'), { 'source': reports.get_customercode_from, 'minLength': 1 });
        $('#sales-customercodefrom-autocomplete').on('selectitem.uk.autocomplete', reports.set_customercode_from);
        $.UIkit.autocomplete($('#sales-customercodeTo-autocomplete'), { 'source': reports.get_customercode_To, 'minLength': 1 });
        $('#sales-customercodeTo-autocomplete').on('selectitem.uk.autocomplete', reports.set_customercode_To);

        $.UIkit.autocomplete($('#generalledger-doctype-autocomplete'), { 'source': reports.get_doctype, 'minLength': 1 });
        $('#generalledger-doctype-autocomplete').on('selectitem.uk.autocomplete', reports.set_doctype);

        $.UIkit.autocomplete($('#doc-no-from-autocomplete'), { 'source': reports.get_docno, 'minLength': 1 });
        $('#doc-no-from-autocomplete').on('selectitem.uk.autocomplete', reports.set_docno);


        $("#LocationID").on("change", reports.get_premises);

        $("#FromBankNameID").on("change", reports.get_bank_name_from);
        $("#ToBankNameID").on("change", reports.get_bank_name_to);

        $.UIkit.autocomplete($('#documentnofrom-autocomplete'), { 'source': reports.get_documentnofrom, 'minLength': 1 });
        $('#documentnofrom-autocomplete').on('selectitem.uk.autocomplete', reports.set_documentnofrom);
        $.UIkit.autocomplete($('#documentnoto-autocomplete'), { 'source': reports.get_documentnoto, 'minLength': 1 });
        $('#documentnoto-autocomplete').on('selectitem.uk.autocomplete', reports.set_documentnoto);

        $.UIkit.autocomplete($('#documentnofrom-autocomplete'), { 'source': reports.get_account_head, 'minLength': 1 });
        $('#documentnofrom-autocomplete').on('selectitem.uk.autocomplete', reports.set_account_head);

        $.UIkit.autocomplete($('#ledgeraccountcode-autocomplete'), { 'source': reports.get_ledger_account_code, 'minLength': 1 });
        $('#ledgeraccountcode-autocomplete').on('selectitem.uk.autocomplete', reports.set_ledger_account_code);

        $.UIkit.autocomplete($('#ledgeraccountname-autocomplete'), { 'source': reports.get_ledger_account_name, 'minLength': 1 });
        $('#ledgeraccountname-autocomplete').on('selectitem.uk.autocomplete', reports.set_ledger_account_name);

        $.UIkit.autocomplete($('#generalledger-documentno-autocomplete'), { 'source': reports.get_documentno, 'minLength': 1 });
        $('#generalledger-documentno-autocomplete').on('selectitem.uk.autocomplete', reports.set_documentno);
    },

    get_account_head: function (release) {
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadsForAutoComplete',
            data: {
                DebitAccountName: $('#AccountName').val(),
                DebitAccountCode: $('#AccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_account_head: function (event, item) {
        $("#DebitAccountHeadID").val(item.id);
        $("#DebitAccountCode").val(item.number);
        $("#DebitAccountName").val(item.name);
        $("#DebitAccountCode, #DebitAccountName").removeClass('md-input-danger');
        $("#DebitAccountCode, #DebitAccountName").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#DebitAmount').focus();
    },

    get_documentnofrom: function (release) {
        var self = reports;
        var Table = "DocumentNo";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#DocumentNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_documentnofrom: function (event, item) {
        var self = reports;
        $('#DocumentNoFrom').val(item.code)
        $("#DocumentNoFromID").val(item.id);
    },

    get_documentnoto: function (release) {
        var self = reports;
        var Table = "DocumentNo";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#DocumentNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_documentnoto: function (event, item) {
        var self = reports;
        $('#DocumentNoTo').val(item.code)
        $("#DocumentNoToID").val(item.id);
    },

    get_bank_name_from: function () {
        var self = reports;
        if ($('#ToBankNameID').is('[disabled!=disabled]')) {
            $('#FromBankNameID').attr('disabled', true);
            var location_id = 0;
            $.ajax({
                url: '/Masters/Treasury/GetBankDetails/',
                dataType: "json",
                type: "POST",
                data: { LocationID: location_id },
                success: function (response) {
                    $("#ToBankNameID").html("");
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                    });
                    $("#ToBankNameID").append(html);
                }
            });
        }
    },

    get_bank_name_to: function () {
        var self = reports;
        if ($('#FromBankNameID').is('[disabled!=disabled]')) {
            $('#ToBankNameID').attr('disabled', true);
            var location_id = 0;
            $.ajax({
                url: '/Masters/Treasury/GetBankDetails/',
                dataType: "json",
                type: "POST",
                data: { LocationID: location_id },
                success: function (response) {
                    $("#FromBankNameID").html("");
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                    });
                    $("#FromBankNameID").append(html);
                }
            });
        }
    },

    get_accountname: function (release) {
        var self = reports;
        var Table = "AccountHeadName";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $("#AccountName").val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                console.log(data);
                release(data);
            }
        });

    },
    set_accountname: function (event, item) {
        var self = reports;
        console.log(item);
        $("#AccountNameID").val(item.id);
        $("#AccountName").val(item.Value)

    },

    get_ledger_accountcodefrom: function (release) {
        var self = reports;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AccountCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_ledger_accountcodefrom: function (event, item) {
        var self = reports;
        $('#AccountCodeFrom').val(item.code)
        $("#AccountCodeFromID").val(item.id);

    },
    get_ledger_accountcodeto: function (release) {
        var self = reports;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AccountCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_ledger_accountcodeto: function (event, item) {
        var self = reports;
        $('#AccountCodeTo').val(item.code)
        $("#AccountCodeToID").val(item.id);

    },
    get_advance_return_voucher_no: function (release) {
        var Table;
        Table = 'AdvanceVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvancePaymentNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_advance_return_voucher_no: function (event, item) {
        self = reports;
        $("#AdvancePaymentNoFrom").val(item.code);
        $("#AdvancePaymentNoFromID").val(item.id);
        var report_type = $(".adv_return_summary:checked").val();
        if (report_type == "Detail") {
            $("#AdvancePaymentNoTo").val(item.code);
            $("#AdvancePaymentNoToID").val(item.id);
        }
        else {
            $("#AdvancePaymentNoTo").val('');
            $("#AdvancePaymentNoToID").val('');
        }
    },

    get_advance_return_voucher_noTo: function (release) {
        var Table;
        Table = 'AdvanceVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvancePaymentNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_advance_return_voucher_noTo: function (event, item) {
        self = reports;
        $("#AdvancePaymentNoTo").val(item.code);
        $("#AdvancePaymentNoToID").val(item.id);
    },
    get_AdvanceReturnNo: function (release) {
        var Table = 'AdvanceReturnVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvReturnVchNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_AdvanceReturnNo: function (event, item) {
        self = reports;
        $("#AdvReturnVchNoFrom").val(item.voucherNo);
        $("#AdvReturnVchNoFromID").val(item.id);
    },

    get_AdvanceReturnNoTo: function (release) {
        var Table = 'AdvanceReturnVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvReturnVchNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_AdvanceReturnNoTo: function (event, item) {
        self = reports;
        $("#AdvReturnVchNoTo").val(item.voucherNo);
        $("#AdvReturnVchNoToID").val(item.id);
    },
    adv_return_show_report_type: function () {
        self = reports;
        var item_type = $(".adv_return_summary:checked").val();
        var type = $(this).val();
        if (item_type == "Detail") {
            $(".summary").hide();
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");
            $(".detail").removeClass("uk-hidden");
            //$(".adv-from").text('Adv Vch No');
        }
        else {
            $(".summary").hide();
            $(".summary").show();
            $(".detail").addClass("uk-hidden");
            //$(".adv-from").text('Adv Vch No From');
        }

        if (type == "Supplier") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").removeClass("uk-hidden");

        }
        if (type == "Employee") {
            $(".employee-wise").removeClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");

        }

        self.refresh();
    },

    adv_return_show_report_item_type: function () {
        self = reports;
        var item_type = $(this).val();
        var type = $(".adv_return_report_type:checked").val();

        if (item_type == "Detail") {
            $(".summary").hide();
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");
            $(".detail").removeClass("uk-hidden");
            // $(".adv-from").text('Adv Vch No');
        }
        else {
            $(".summary").hide();
            $(".summary").show();
            $(".detail").addClass("uk-hidden");
            //$(".adv-from").text('Adv Vch No From');
        }
        if (type == "Supplier") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").removeClass("uk-hidden");

        }
        if (type == "Employee") {
            $(".employee-wise").removeClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");
        }



    },
    supplier_debit_note_show_report_item_type: function () {
        self = reports;
        self.refresh();
    },
    show_party_type: function () {
        self = reports;
        var party_type = $(this).val();
        var type = $(".party_type:checked").val();
        if (type == "Supplier") {
            $(".customer").hide();
            $(".supplier").show();
            $(".supplier_report_type").show();
            $(".customer").addClass("uk-hidden");
            $(".supplier-wise").show();

        }
        else {
            $(".supplier_report_type").show();
            $(".customer").show();
            $(".customer").removeClass("uk-hidden");
            $(".supplier-wise").hide();

        }
        self.refresh();

    },

    supplier_debit_note_show_report_type: function () {
        self = reports;

        var type = $(".supplier_debit_note_type:checked").val();
        if (type == "DebitNote") {


            $(".fromdate").text('Debit Note From Date');
            $(".todate").text('Debit Note To Date');
            $(".from").text('Debit Note No From');
            $(".to").text('Debit Note No To');
        }
        else {
            $(".fromdate").text('Credit Note From Date');
            $(".todate").text('Credit Note To Date');
            $(".from").text('Credit Note No From');
            $(".to").text('Credit Note No To');
        }

        self.refresh();
    },


    advance_payment_show_report_type: function () {
        self = reports;
        var item_type = $(".Advance_Payment_Summary:checked").val();
        var type = $(this).val();
        if (item_type == "Detail") {

            $(".summary").hide();
            $(".detail").hide();
            $(".detail").show();
            //$(".advanvepaymentno").text('Advance No');
        }
        else {
            $(".detail").hide();
            $(".summary").hide();
            $(".summary").show();
            $(".advanvepaymentno").text('Adv Vch No From');

        }
        if (type == "Supplier") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").removeClass("uk-hidden");

        }
        if (type == "Employee") {
            $(".employee-wise").removeClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");

        }
        if (type == "All") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");
        }
        self.refresh();
    },

    advance_payment_show_report_item_type: function () {
        self = reports;
        var item_type = $(this).val();
        var type = $(".advance_payment_report_type:checked").val();
        if (item_type == "Detail") {

            $(".summary").hide();
            $(".detail").hide();
            $(".detail").show();
            //$(".advanvepaymentno").text('Advance No');
        }
        else {
            $(".detail").hide();
            $(".summary").hide();
            $(".summary").show();
            $(".advanvepaymentno").text('Adv Vch No From');

        }
        if (type == "Supplier") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").removeClass("uk-hidden");

        }
        if (type == "Employee") {
            $(".employee-wise").removeClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");

        }
        if (type == "All") {
            $(".employee-wise").addClass("uk-hidden");
            $(".supplier-wise").addClass("uk-hidden");
        }
        //self.refresh();
    },

    get_to_range: function () {
        var self = reports;
        var from_range = $("#FromSupplierRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierRange").append(html);
            }
        });
    },
    get_to_suppliercatrange: function () {
        var self = reports;
        var from_range = $("#FromSupplierCatRange").val();
        $.ajax({
            url: '/Reports/Accounts/GetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierCatRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierCatRange").append(html);
            }
        });
    },
    get_doc_to_range: function () {
        var self = reports;
        var from_range = $("#DocTypeFromRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#DocTypeToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#DocTypeToRange").append(html);
            }
        });
    },
    get_transtype_to_range: function () {
        var self = reports;
        var from_range = $("#TransTypeFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#TransTypeToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#TransTypeToRange").append(html);
            }
        });
    },
    get_acc_name_to_range: function () {
        var self = reports;
        var from_range = $("#AccountCodeFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#AccountCodeToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#AccountCodeToRange").append(html);
            }
        });
    },
    get_employee_name_to_range: function () {
        var self = reports;
        var from_range = $("#EmployeeFromRange").val();
        $.ajax({
            url: '/Reports/Stock/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#EmployeeToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#EmployeeToRange").append(html);
            }
        });
    },
    get_company_to_range: function () {
        var self = reports;
        var from_range = $("#CompanyFromRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#CompanyToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#CompanyToRange").append(html);
            }
        });
    },

    show_voucher_report_item_type: function () {
        self = reports;
        var type = $(".voucher-summary:checked").val();
        if (type == "Summary") {
            $(".summary").hide();
            $(".detail").hide();
            $(".summary").show();
            //$(".fromdate").text('From Date');

        }
        else {
            $(".summary").hide();
            $(".detail").hide();
            //$(".fromdate").text('Payment Date');
            $(".detail").show();
            $(".detail").removeClass("uk-hidden");
            $(".detail").removeClass("uk-hidden");
        }
        $("#VoucherNoFrom").val('');
        $("#VoucherNoTo").val('');
        $("#VoucherFromID").val('');
        $("#VoucherToID").val('');
        $("#DocumentNo").val('');
        $("#DocumentID").val('');
        $("#SupplierID").val('');
        $("#SupplierName").val('');
        $("#BankDetails").val('');
        $("#BankID").val('');
        $("#FromSupplierRange").val('');
        $("#ToSupplierRange").val('');
    },

    refresh: function (event, item) {
        self = reports;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#ToDate").val(currentdate);
        $("#FromDate").val(findate);
        $("#FromDateString").val(findate);
        $("#ToDateString").val(currentdate);
        $("#VoucherNo").val('');
        $("#DocumentNo").val('');
        $("#BankDetails").val('');
        $("#PRNOFrom").val('');
        $("#FromSupplierRange").val('');
        $("#ToSupplierRange").val('');
        $("#SupplierID").val('');
        $("#SupplierName").val('');
        $("#VoucherID").val('');
        $("#VoucherNoFrom").val('');
        $("#VoucherNoTo").val('');
        $("#VoucherFromID").val('');
        $("#VoucherToID").val('');
        $("#DocumentID").val('');
        $("#BankID").val('');

        $("#DocTypeFromRange").val('');
        $("#DocTypeToRange").val('');
        $("#DocType").val('');

        $("#TransType").val('');
        $("#CreditAccountCode").val('');
        $("#CreditAccountCodeTo").val('');
        $("#AccountCodeFromRange").val('');
        $("#AccountCodeToRange").val('');
        $("#CreditAccountName").val('');
        $("#DepartmentID").val('');
        $("#EmployeeFromRange").val('');

        $("#EmployeeToRange").val('');
        $("#EmployeeID").val('');
        $("#CompanyFromRange").val('');
        $("#CompanyToRange").val('');

        $("#ProjectID").val('');
        $("#CompanyID").val('');
        $("#TransTypeFromRange").val('');
        $("#TransTypeToRange").val('');

        $("#FromSupplierCatRange").val('');
        $("#ToSupplierCatRange").val('');
        $("#dropPayment").val('');
        $("#AdvancePaymentNoFrom").val('');

        $("#AdvancePaymentNoFromID").val('');
        $("#AdvancePaymentNoTo").val('');
        $("#AdvancePaymentNoToID").val('');

        $("#EmployeeName").val('');
        $("#EmployeeID").val('');

        $("#DebitNoteNoFrom").val('');
        $("#DebitNoteNoFromID").val('');
        $("#DebitNoteNoTo").val('');
        $("#DebitNoteNoToID").val('');

        $("#AdvReturnVchNoFrom").val('');
        $("#AdvReturnVchNoFromID").val('');
        $("#AdvReturnVchNoTo").val('');
        $("#AdvReturnVchNoToID").val('');
        $("#AccountName").val('');
        $("#AccountNameID").val('');
        $("#CustomerCodeFrom").val('');
        $("#CustomerCodeTo").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PremisesID").val('');
        $("#PremisesID").val('');
        $("#Customer").val('');
        $("#CustomerID").val('');
        $("#FromTransNoID").val('');
        $("#FromTransNo").val('');
        $("#ToTransNo").val('');
        $("#ToTransNoID").val('');
        $("#FromBankNameID").val('');
        $("#ToBankNameID").val('');
        $("#FromBankAccountNo").val('');
        $("#FromBankAccountNoID").val('');
        $("#ToBankAccountNo").val('');
        $("#ToBankAccountNoID").val('');
        $('#FromBankNameID').attr('disabled', false);
        $('#ToBankNameID').attr('disabled', false);
        $("#Location").val(locationID);
        $("#RefInvoiceNo").val('');
        $("#RefDocDate").val('');
        $("#LocationID").val(locationID);
        $("#Department").val('');
        $("#AccountCodeFrom").val('');
        $("#AccountCodeTo").val('');
        $("#AccountCodeFromID").val('');
        $("#AccountCodeToID").val('');
        $("#DocumentNoFrom").val('');
        $("#DocumentNoTo").val('');
        $("#DocumentNoFromID").val('');
        $("#DocumentNoToID").val('');
        $("#ItemLocationID").val(locationID);
        reports.get_bank();

    },

    get_bank: function () {
        self = reports;
        var locationID = clean($("#LocationID").val());
        $.ajax({
            url: '/Masters/Treasury/GetBankDetails/',
            dataType: "json",
            type: "POST",
            data: { LocationID: locationID },
            success: function (response) {
                $("#FromBankNameID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#FromBankNameID").append(html);
            }
        });

        $("#FromInstrumentNo").val('');
        $("#InstrumentNoID").val('');
        $("#Status").val('');
        $("#BankAccountNoID").val('')
        $.ajax({
            url: '/Masters/Treasury/GetBankDetails/' + locationID,
            dataType: "json",
            type: "POST",
            data: { LocationID: locationID },
            success: function (response) {
                $("#ToBankNameID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#ToBankNameID").append(html);
            }
        });
    },

    get_to_range: function () {
        var self = reports;
        var from_range = $("#FromSupplierRange").val();

        $.ajax({
            url: '/Reports/Accounts/GetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierRange").append(html);
            }
        });
    },

    get_report_view: function (e) {
        e.preventDefault();
        self = reports;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        ReportHelper.hide_controls();
        switch (name) {
            case "AdvanceRequest":
                if ($("#EmployeeID").val() != "") {
                    filters += "Employee: " + $("#EmployeeName").val() + ", ";
                }
                if ($("#Location").val() != "") {
                    filters += "Location: " + $("#Location Option:selected").text();
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "PaymentVoucher":
                if ($("#Location").val() != "") {
                    filters += "Location: " + $("#Location Option:selected").text() + ", ";
                }
                if ($("#BankDetails").val() != "") {
                    filters += "Bank : " + $("#BankDetails").val() + ", ";
                }
                filters += self.get_supplier_details();
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "AdvancePayment":
                if ($("#EmployeeID").val() != 0) {
                    filters += "Employee: " + $("#EmployeeName").val() + ", ";
                }
                if ($("#dropPayment").val() != 0) {
                    filters += "Payment Mode: " + $("#dropPayment Option:Selected").text() + ", ";
                }
                filters += self.get_advance_payment_no();
                filters += self.get_supplier_details();
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "AdvancePaymentReturn":
                filters += self.get_advance_payment_no();
                if ($("#AdvReturnVchNoFrom").val() != "") {
                    filters += "Advance Return Voucher No: " + $("#AdvReturnVchNoFrom").val() + ", ";
                }
                filters += self.get_supplier_details();
                filters += self.get_employee_details();
                if ($("#Status").val() != "") {
                    filters += "Status: " + $("#Status Option:Selected").text() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "FundTransfer":

                if (($("#FromTransNo").val() + $("#ToTransNo").val()).trim() != "") {
                    if ($("#FromTransNo").val().trim() != "" && $("#ToTransNo").val().trim() != "") {
                        filters += "Trans No: " + $("#FromTransNo").val() + " - " + $("#ToTransNo").val() + ", ";
                    }
                    else {
                        if ($("#FromTransNo").val().trim() != "") {
                            filters += "Trans No From: " + $("#FromTransNo").val() + ", ";
                        }
                        else {
                            filters += "Trans No To: " + $("#ToTransNo").val() + ", ";
                        }
                    }
                }
                if (($("#FromBankNameID").val() + $("#ToBankNameID").val()).trim() != "") {
                    if ($("#FromBankNameID").val().trim() != "" && $("#ToBankNameID").val().trim() != "") {
                        filters += "Bank Name: " + $("#FromBankNameID Option:Selected").text() + " - " + $("#ToBankNameID Option:Selected").text() + ", ";
                    }
                    else {
                        if ($("#FromBankNameID").val().trim() != "") {
                            filters += "Bank Name From: " + $("#FromBankNameID Option:Selected").text() + ", ";
                        }
                        else {
                            filters += "Bank Name To: " + $("#ToBankNameID Option:Selected").text() + ", ";
                        }
                    }
                }
                if (($("#FromBankAccountNo").val() + $("#ToBankAccountNo").val()).trim() != "") {
                    if ($("#FromBankAccountNo").val().trim() != "" && $("#ToBankAccountNo").val().trim() != "") {
                        filters += "Bank Account Code: " + $("#FromBankAccountNo").val() + " - " + $("#ToBankAccountNo").val() + ", ";
                    }
                    else {
                        if ($("#FromBankAccountNo").val().trim() != "") {
                            filters += "Bank Account Code From: " + $("#FromBankAccountNo").val() + ", ";
                        }
                        else {
                            filters += "Bank Account Code To: " + $("#ToBankAccountNo").val() + ", ";
                        }
                    }
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "ChequeDeposit":
                if ($("#FromInstrumentNo").val() != "") {
                    filters += "Instrument No: " + $("#FromInstrumentNo").val() + ", ";
                }

                if ($("#BankID").val() != 0) {
                    filters += "Bank: " + $("#BankID Option:Selected").text() + ", ";;
                }
                if ($("#FromBankAccountNo").val() != "") {
                    filters += "Account No: " + $("#FromBankAccountNo").val() + ", ";;
                }
                if ($("#Status").val() != "") {
                    filters += "Status: " + $("#Status Option:Selected").text();
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "DebitAndCreditNote":

                filters = self.get_customer_details();
               // filters += self.get_employee_details();
                if (($("#DebitNoteNoFrom").val() + $("#DebitNoteNoTo").val()).trim() != "") {
                    if ($("#DebitNoteNoFrom").val().trim() != "" && $("#DebitNoteNoTo").val().trim() != "") {
                        filters += " Note No : " + $("#DebitNoteNoFrom").val() + " - " + $("#DebitNoteNoTo").val() + ", ";
                    }
                    else {
                        if ($("#DebitNoteNoFrom").val().trim() != "") {
                            filters += " No From: " + $("#DebitNoteNoFrom").val() + ", ";
                        }
                        else {
                            filters += "No To: " + $("#DebitNoteNoTo").val() + ", ";
                        }
                    }
                }
                if ($("#SupplierID").val() != "") {
                    filters += "Supplier: " + $("#SupplierName").val() + ", ";
                }
                if ($("#RefInvoiceNo").val() != "") {
                    filters += "Ref. Invoice No: " + $("#RefInvoiceNo").val() + ", ";
                }
                if ($("#RefDocDate").val() != "") {
                    filters += "Ref.Document Date: " + $("#RefDocDate").val() + ", ";
                }
                if ($("#ItemLocationID:visible").length) {
                    if ($("#ItemLocationID:visible").val() != 0) {
                        filters += "Location: " + $("#ItemLocationID Option:Selected").text() + ", ";
                    }
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                    data += "&Filters=" + filters;
                }
                break;
            case "JournalVoucher":
                if (($("#DocumentNoFrom").val() + $("#DocumentNoTo").val()).trim() != "") {
                    if ($("#DocumentNoTo").val().trim() == "") {
                        filters += "Document No " + "From: " + $("#DocumentNoFrom").val() + ", ";
                    }
                    else if ($("#DocumentNoFrom").val().trim() == "") {
                        filters += "Document No " + "To: " + $("#DocumentNoTo").val() + ", ";
                    }
                    else if (($("#DocumentNoFrom").val() + $("#DocumentNoTo").val()).trim() != "") {
                        filters += "Document No: " + $("#DocumentNoFrom").val() + " - " + $("#DocumentNoTo").val() + ", ";
                    }
                }
                if ($("#AccountNameID ").val() == "") {
                    if (($("#AccountCodeFrom").val() + $("#AccountCodeTo").val()).trim() != "") {
                        if ($("#AccountCodeTo").val().trim() == "") {
                            filters += "Account Code " + "From: " + $("#AccountCodeFrom").val() + ", ";
                        }
                        else if ($("#AccountCodeFrom").val().trim() == "") {
                            filters += "Account Code " + "To: " + $("#AccountCodeTo").val() + ", ";
                        }
                        else if (($("#AccountCodeFrom").val() + $("#AccountCodeTo").val()).trim() != "") {
                            filters += "Account Code: " + $("#AccountCodeFrom").val() + " - " + $("#AccountCodeTo").val() + ", ";
                        }
                    }
                }
                if ($("#AccountNameID ").val() == "") {
                    if (($("#AccountCodeFromRange").val() + $("#AccountCodeToRange").val()).trim() != "") {
                        if ($("#AccountCodeToRange").val().trim() == "") {
                            filters += "Account Name Range " + "From: " + $("#AccountCodeFromRange").val() + ", ";
                        }
                        else if ($("#AccountCodeFromRange").val().trim() == "") {
                            filters += "Account Name Range " + "To: " + $("#AccountCodeToRange").val() + ", ";
                        }
                        else if (($("#AccountCodeFromRange").val() + $("#AccountCodeToRange").val()).trim() != "") {
                            filters += "Account Name Range: " + $("#AccountCodeFromRange").val() + " - " + $("#AccountCodeToRange").val() + ", ";
                        }
                    }
                }
                if ($("#AccountNameID ").val() != 0) {
                    filters += "Account Name: " + $("#AccountName").val() + ", ";
                }
                if ($("#LocationID").val() != "") {
                    filters += "Location: " + $("#LocationID option:selected").text() + ", ";
                }
                if ($("#DepartmentID").val() != 0) {
                    filters += "Department: " + $("#DepartmentID option:selected").text() + ", ";
                }
                if ($("#EmployeeName ").val() != 0) {
                    filters += "Employee Name: " + $("#EmployeeName").val() + ", ";
                }
                if ($("#CompanyID").val() != 0) {
                    filters += "Inter Company: " + $("#CompanyID option:selected").text() + ", ";
                }
                if ($("#ProjectID").val() != 0) {
                    filters += "Project: " + $("#ProjectID option:selected").text() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "TrialBalance":
             
                if ($("#AccountCodeFromID ").val() != 0) {
                    filters += "Account Code: " + $("#AccountCodeFrom").val() + ", ";
                }
                if ($("#AccountNameID ").val() != 0) {
                    filters += "Account Name: " + $("#AccountName").val() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "GeneralLedger":

                if ($("#DocType ").val() != 0) {
                    filters += "Document Type: " + $("#DocType").val() + ", ";
                }

                if ($("#TransType ").val() != 0) {
                    filters += "Transaction Type: " + $("#TransType").val() + ", ";
                }

                if ($("#AccountCodeFrom").val() != 0) {
                    filters += "Account Code: " + $("#AccountCodeFrom").val() + ", ";
                }


                //if ($("#AccountNameID ").val() != 0) {
                //    if (($("#AccountCodeFromRange").val() + $("#AccountCodeToRange").val()).trim() != "") {
                //        if ($("#AccountCodeToRange").val().trim() == "") {
                //            filters += "Account Name Range " + "From: " + $("#AccountCodeFromRange").val() + ", ";
                //        }
                //        else if ($("#AccountCodeFromRange").val().trim() == "") {
                //            filters += "Account Name Range " + "To: " + $("#AccountCodeToRange").val() + ", ";
                //        }
                //        else if (($("#AccountCodeFromRange").val() + $("#AccountCodeToRange").val()).trim() != "") {
                //            filters += "Account Name Range: " + $("#AccountCodeFromRange").val() + " - " + $("#AccountCodeToRange").val() + ", ";
                //        }
                //    }
                //}

                if ($("#AccountNameID ").val() != 0) {
                    filters += "Account Name: " + $("#AccountName").val() + ", ";
                }

                if ($("#EmployeeName ").val() != 0) {
                    filters += "Employee Name: " + $("#EmployeeName").val() + ", ";
                }

                if ($("#ItemLocationID").val() != 0) {
                    filters += "Location: " + $("#ItemLocationID option:selected").text() + ", ";
                }

                if ($("#DepartmentID").val() != 0) {
                    filters += "Department: " + $("#DepartmentID option:selected").text() + ", ";
                }

                if ($("#CompanyID").val() != 0) {
                    filters += "Inter Company: " + $("#CompanyID option:selected").text() + ", ";
                }

                if ($("#ProjectID").val() != 0) {
                    filters += "Project: " + $("#ProjectID option:selected").text() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;

                break;
            case "ItemSubLedger":

                if ($("#ItemName").val() != 0) {
                    filters += "Item Name: " + $("#ItemName").val() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;

                break;
            case "CashBankLedger":

                if ($("#BankDetails").val() != 0) {
                    filters += "Bank Details: " + $("#BankDetails").val() + ", ";
                }

                if ($("#DocumentNo").val() != 0) {
                    filters += "Document No: " + $("#DocumentNo").val() + ", ";
                }

                if ($("#AccountCodeFrom").val() != 0) {
                    filters += "Contra D/C No: " + $("#AccountCodeFrom").val() + ", ";
                }

                if ($("#AccountName").val() != 0) {
                    filters += "Contra D/C Name: " + $("#AccountName").val() + ", ";
                }

                if ($("#Location").val() != 0) {
                    filters += "Location: " + $("#Location option:selected").text() + ", ";
                }

                if ($("#PaymentModeID").val() != 0) {
                    filters += "Payment Mode: " + $("#PaymentModeID option:selected").text() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;

                break;

            case "ReceiptVoucher":

                if ($("#Customer").val() != 0) {
                    filters += "Customer Name: " + $("#Customer").val() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;

                break;

        }

        console.log(data);
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#report-viewer").html(response)
                ReportHelper.inject_js();
            }
        })
        return false;
    },

    get_supplier_details: function () {
        self = reports;
        var filters = "";
        if ($("#SupplierID").val() == "") {
            if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
                if ($("#FromSupplierRange").val().trim() != "" && $("#ToSupplierRange").val().trim() != "") {
                    filters += "Supplier Range: " + $("#FromSupplierRange").val() + " - " + $("#ToSupplierRange").val() + ", ";
                } else {
                    if ($("#FromSupplierRange").val().trim() != "") {
                        filters += "Supplier Range From: " + $("#FromSupplierRange").val() + ", ";
                    } else {
                        filters += "Supplier Range To: " + $("#ToSupplierRange").val() + ", ";
                    }
                }
            }
        }
        else {
            filters += "Supplier: " + $("#SupplierName").val() + ", ";
        }
        return filters;
    },

    get_advance_payment_no: function () {
        var self = reports;
        var filters = "";
        if (($("#AdvancePaymentNoFrom").val() + $("#AdvancePaymentNoTo").val()).trim() != "") {
            if ($("#AdvancePaymentNoFrom").val().trim() != "" && $("#AdvancePaymentNoTo").val().trim() != "") {
                filters += "Advance Payment No: " + $("#AdvancePaymentNoFrom").val() + " - " + $("#AdvancePaymentNoTo").val() + ", ";
            }
            else {
                if ($("#AdvancePaymentNoFrom").val().trim() != "") {
                    filters += "Advance Payment No From: " + $("#AdvancePaymentNoFrom").val() + ", ";
                }
                else {
                    filters += "Advance Payment No To: " + $("#AdvancePaymentNoTo").val() + ", ";
                }
            }
        }
        return filters;
    },

    get_employee_details: function () {
        self = reports;
        var filters = "";
        if ($("#EmployeeID").val() == "") {
            if (($("#EmployeeFromRange").val() + $("#EmployeeToRange").val()).trim() != "") {
                if ($("#EmployeeFromRange").val().trim() != "" && $("#EmployeeToRange").val().trim() != "") {
                    filters += "Employee Range: " + $("#EmployeeFromRange").val() + " - " + $("#EmployeeToRange").val() + ", ";
                } else {
                    if ($("#EmployeeFromRange").val().trim() != "") {
                        filters += "Employee Range From: " + $("#EmployeeFromRange").val() + ", ";
                    } else {
                        filters += "Employee Range To: " + $("#EmployeeToRange").val() + ", ";
                    }
                }
            }
        }
        else {
            filters += "Employee: " + $("#EmployeeName").val() + ", ";
        }
        return filters;
    },

    get_customer_details: function () {
        var self = reports;
        var filters = "";
        if ($("#CustomerID").val() == "") {
            if ($("#CustomerCodeFrom").val().trim() + $("#CustomerCodeTo").val() != "") {
                if ($("#CustomerCodeFrom").val().trim() != "" && +$("#CustomerCodeTo").val().trim() != "") {
                    filters += "Customer Code:" + $("#CustomerCodeFrom").val() + " - " + $("#CustomerCodeTo").val() + ", ";
                } else {
                    if ($("#CustomerCodeFrom").val().trim() != "") {
                        filters += "Customer Code From: " + $("#CustomerCodeFrom").val() + ", ";
                    }
                    else {
                        filters += "Customer Code To: " + $("#CustomerCodeTo").val() + ", ";
                    }
                }
            }
        }
        else {
            filters += "Customer: " + $("#Customer").val() + ", ";
        }
        return filters;
    },

    clear_item: function () {
        if ($(this).val() == "") {
            $("#AccountID").val('');
        }
    },
    //get_report_view: function () {
    //    var data = $("#report-filter-form").serialize();
    //    var url = $("#report-filter-form").attr("action");
    //    console.log(data);
    //    $.ajax({
    //        url: url,
    //        data: data,
    //        dataType: "html",
    //        type: "POST",
    //        success: function (response) {
    //            $("#report-viewer").html(response)
    //        }
    //    })
    //    return false;
    //},

    //19/4/2018 lini
    get_items: function (release) {
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadsForAutoComplete',
            data: {
                Hint: $('#AccountName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },
    set_item: function (event, item) {
        var self = reports;
        console.log(item)
        $("#AccountName").val(item.value + "(" + item.accountId + ")");
        $("#AccountID").val(item.accountId),
        $("#GroupClassification").val(item.groupclassification);
        $("#OpeningAmt").val(item.openingamt)

    },
    show_advancereturn_type: function () {
        self = reports;
        var summary = $(this).val();
        if (summary == "Summary") {
            $(".detail").hide();
        } else if (summary == "Detail") {
            $(".summary").hide();
        }
    },
    show_report_type: function () {
        self = reports;
        var report_type = $(this).val();
        $("#select-type").removeClass("uk-hidden");
        $(".employee-wise").hide();
        $(".supplier-wise").hide();
        if (report_type == "All") {
            $(".employee-wise").hide();
            $(".supplier-wise").hide();
        } else if (report_type == "Employee") {
            $(".employee-wise").show();
        }
        else {
            $(".supplier-wise").show();
        }
        self.clear_item();
    },
    clear_item: function () {
        $("#SupplierID").val('');
        $("#SupplierName").val('');
        $("#EmployeeID").val('');
        $("#EmployeeName").val('');

    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetAllSupplierAutoComplete',
            data: {
                Term: $('#SupplierName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_supplier_details: function (event, item) {
        self = reports;
        $("#SupplierName").val(item.name);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#hdnName").val(item.name);
        $("#hdnNameID").val(item.id);
        //self.get_Items(item.id);

    },
    get_voucher: function (release) {
        var Table = 'PaymentVoucher';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#VoucherNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_voucher: function (event, item) {
        self = reports;
        $("#VoucherNoFrom").val(item.voucherNo);
        $("#VoucherFromID").val(item.id);
    },

    get_voucherTo: function (release) {
        var Table = 'PaymentVoucher';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#VoucherNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_voucherTo: function (event, item) {
        self = reports;
        $("#VoucherNoTo").val(item.voucherNo);
        $("#VoucherToID").val(item.id);
    },

    get_document: function (release) {
        var Table = 'PaymentDet';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#DocumentNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_document: function (event, item) {
        self = reports;
        $("#DocumentNo").val(item.voucherNo);
        $("#DocumentID").val(item.id);
    },
    get_bank_details: function (release) {
        var Table = 'Treasury';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#BankDetails').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_bank_details: function (event, item) {
        self = reports;
        $("#BankDetails").val(item.voucherNo);
        $("#BankID").val(item.id);
    },
    get_employees: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },
    set_employees: function (event, item) {
        self = reports;
        console.log(item)
        var employeeId = item.id;
        $("#EmployeeID").val(item.id),
        $("#Name").val(item.value);
        $("#Code").val(item.code);
        $("#Place").val(item.place);
        $("#hdnName").val(item.value);
        $("#hdnNameID").val(item.id);
    },
    get_allitems: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAllItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_allitem: function (event, item) {
        var self = reports;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },
    get_transtype: function (release) {
        //var type = $(".type:checked").val();
        self = reports;
        var Table;
        Table = 'TransactionType';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#TransType').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_transtype: function (event, item) {
        self = reports;
        $("#TransType").val(item.code);
        $("#TransTypeID").val(item.id);

    },

    ///////////////////////////////////////////////////
    get_Account: function (release) {
        $.ajax({
            url: '/Accounts/Journal/GetCreditAccountAutoComplete',
            data: {
                CreditAccountName: $('#CreditAccountName').val(),
                CreditAccountCode: $('#CreditAccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_Account: function (event, item) {
        $("#CreditAccountHeadID").val(item.id);
        $("#CreditAccountCode").val(item.number);
        $("#CreditAccountName").val(item.name);
        $("#CreditAccountCode, #CreditAccountName").removeClass('md-input-danger');
        $("#CreditAccountCode, #CreditAccountName").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#CreditAmount').focus();
    },

    get_AccountTo: function (release) {
        $.ajax({
            url: '/Accounts/Journal/GetCreditAccountAutoComplete',
            data: {
                CreditAccountName: $('#CreditAccountNameTo').val(),
                CreditAccountCode: $('#CreditAccountCodeTo').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_AccountTo: function (event, item) {
        $("#CreditAccountHeadIDTo").val(item.id);
        $("#CreditAccountCodeTo").val(item.number);
        $("#CreditAccountNameTo").val(item.name);
        $("#CreditAccountCodeTo, #CreditAccountNameTo").removeClass('md-input-danger');
        $("#CreditAccountCodeTo, #CreditAccountNameTo").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#CreditAmount').focus();
    },

    //////////////////////////////////////////
    get_supplierDebitNoteNo: function (release) {
        var parttype = $(".party_type:checked").val();
        var type = $(".supplier_debit_note_type:checked").val();
        var Table;
        if (parttype == "Supplier") {


            if (type == "DebitNote") {
                Table = 'SupplierDebitNoteNo';
            }
            else {
                Table = 'SupplierCreditNoteNo';
            }
        }
        else if (parttype == "Customer") {
            if (type == "DebitNote") {
                Table = 'CustomerDebitNoteNo';
            }
            else {
                Table = 'CustomerCreditNoteNo';
            }
        }
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#DebitNoteNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_supplierDebitNoteNo: function (event, item) {
        self = reports;
        $("#DebitNoteNoFrom").val(item.code);
        $("#ID").val(item.id);
        $("#DebitNoteNoFromID").val(item.id);
    },
    get_supplierDebitNoteNoTo: function (release) {
        var parttype = $(".party_type:checked").val();
        var type = $(".supplier_debit_note_type:checked").val();
        var Table;
        if (parttype == "Supplier") {

            if (type == "DebitNote") {
                Table = 'SupplierDebitNoteNo';
            }
            else if (type == "CreditNote") {
                Table = 'SupplierCreditNoteNo';
            }
        }
        else if (parttype == "Customer") {
            if (type == "DebitNote") {
                Table = 'CustomerDebitNoteNo';
            }
            else if (type == "CreditNote") {
                Table = 'CustomerCreditNoteNo';
            }
        }
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#DebitNoteNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_supplierDebitNoteNoTo: function (event, item) {
        self = reports;
        $("#DebitNoteNoTo").val(item.code);
        $("#ID").val(item.id);
        $("#DebitNoteNoToID").val(item.id);

    },

    get_advancepaymentno: function (release) {
        var Table;
        Table = 'AdvanceVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvancePaymentNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_advancepaymentno: function (event, item) {
        self = reports;
        $("#AdvancePaymentNoFrom").val(item.code);
        $("#AdvancePaymentNoFromID").val(item.id);
        //var report_type = $(".Advance_Payment_Summary:checked").val();
        //if (report_type == "Detail") {
        //    $("#AdvancePaymentNoTo").val(item.code);
        //    $("#AdvancePaymentNoToID").val(item.id);
        //}
        //else {
        //    $("#AdvancePaymentNoTo").val('');
        //    $("#AdvancePaymentNoToID").val('');
        //}
    },

    get_advancepaymentnoTo: function (release) {
        var Table;
        Table = 'AdvanceVoucherNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#AdvancePaymentNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_advancepaymentnoTo: function (event, item) {
        self = reports;
        $("#AdvancePaymentNoTo").val(item.code);
        $("#AdvancePaymentNoToID").val(item.id);
    },

    get_FundTransNo: function (release) {
        var Table = 'FundTransNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#FromTransNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_FundTransNo: function (event, item) {
        self = reports;
        $("#FromTransNo").val(item.voucherNo);
        $("#FromTransNoID").val(item.id);
    },

    get_FundTransNoTo: function (release) {
        var Table = 'FundTransNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#ToTransNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_FundTransNoTo: function (event, item) {
        self = reports;
        $("#ToTransNo").val(item.voucherNo);
        $("#ToTransNoID").val(item.id);
    },

    get_ChequeTransNo: function (release) {
        var Table = 'FundTransNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#FromTransNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_ChequeTransNo: function (event, item) {
        self = reports;
        $("#FromTransNo").val(item.voucherNo);
        $("#FromTransNoID").val(item.id);
    },

    get_ChequeInstrumentNo: function (release) {
        var Table = 'ChequeInstrumentNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#FromInstrumentNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_ChequeInstrumentNo: function (event, item) {
        self = reports;
        $("#FromInstrumentNo").val(item.value);
        $("#InstrumentNoID").val(item.id);
    },

    get_ChequeTransNoTo: function (release) {
        var Table = 'FundTransNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#ToTransNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_ChequeTransNoTo: function (event, item) {
        self = reports;
        $("#ToTransNo").val(item.voucherNo);
        $("#ToTransNoID").val(item.id);
    },
    get_accountcodefrom: function (release) {
        var self = reports;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#FromBankAccountNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_accountcodefrom: function (event, item) {
        var self = reports;
        $("#FromBankAccountNo").val(item.code);
        $("#BankAccountNoID").val(item.id);

    },
    get_accountcodeto: function (release) {
        var self = reports;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#ToBankAccountNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_accountcodeto: function (event, item) {

        var self = reports;
        $("#ToBankAccountNo").val(item.code);
        $("#ToBankAccountNoID").val(item.id);

    },

    get_refinvoicedocno: function (release) {
        var Table;
        var type = $(".supplier_debit_note_type:checked").val();
        if (type == "DebitNote") {
            Table = 'CustomerDebitNoteRefInvoiceNo';
        }
        else {
            Table = 'CustomerCreditNoteRefInvoiceNo';
        }

        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#RefInvoiceNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_refinvoicedocno: function (event, item) {
        self = reports;
        $("#RefInvoiceNo").val(item.code);
        $("#RefInvoiceNoID").val(item.id);
        //var report_type = $(".Advance_Payment_Summary:checked").val();
        //if (report_type == "Detail") {
        //    $("#AdvancePaymentNoTo").val(item.code);
        //    $("#AdvancePaymentNoToID").val(item.id);
        //}
        //else {
        //    $("#AdvancePaymentNoTo").val('');
        //    $("#AdvancePaymentNoToID").val('');
        //}
    },


    get_customername: function (release) {
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#Customer').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_customername: function (event, item) {
        self = reports;
        $("#Customer").val(item.name);
        $("#CustomerID").val(item.id);

    },

    get_customercode_from: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customercode_from: function (event, item) {
        self = reports;
        $("#CustomerCodeFrom").val(item.code);
        $("#CustomerCodeFromID").val(item.id);
    },

    get_customercode_To: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customercode_To: function (event, item) {
        self = reports;
        $("#CustomerCodeTo").val(item.code);
        $("#CustomerCodeToID").val(item.id);
    },

    get_doctype: function (release) {
        Table = 'DocType';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#DocType').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_doctype: function (event, item) {
        self = reports;
        $("#DocType").val(item.code);
    },

    get_documentno: function (release) {
        Table = 'GLDocumentNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#DocumentNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_documentno: function (event, item) {
        self = reports;
        $("#DocumentNo").val(item.code);
    },

    get_docno: function (release) {
        Table = 'CashOrBankDocNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#DocumentNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_docno: function (event, item) {
        self = reports;
        $("#DocumentNo").val(item.code);
    },


    get_premises: function () {
        var self = reports;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                $("#PremisesID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#PremisesID").append(html);
            }
        });
    },

    get_ledger_account_code: function (release) {
        var Hint = $('#AccountCodeFrom').val();
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadsForSLAAutoComplete',
            data: {
                Hint: Hint,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_ledger_account_code: function (event, item) {
        var self = reports;
        $("#AccountCodeFrom").val(item.accountid);
        $("#AccountCodeFromID").val(item.id);
        $("#AccountName").val(item.accountname);
        $("#AccountNameID").val(item.id);
    },

    get_ledger_account_name: function (release) {
        var Hint = $('#AccountName').val();
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadNameAutoComplete',
            data: {
                Hint: Hint,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_ledger_account_name: function (event, item) {
        var self = reports;
        $("#AccountCodeFrom").val(item.accountid);
        $("#AccountCodeFromID").val(item.id);
        $("#AccountName").val(item.accountname);
        $("#AccountNameID").val(item.id);
    },

    validate_form: function () {
        var self = reports;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [
            {
                elements: "#AdvancePaymentNoTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#AdvancePaymentNoToID').val());
                               var from_id = clean($('#AdvancePaymentNoFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected Advance No  To must be greater than or equal to Advance No From'
                       },
                ]
            },

            {
                elements: "#DebitNoteNoTo",
                rules: [
                       {

                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#DebitNoteNoToID').val());
                               var from_id = clean($('#DebitNoteNoFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           },
                           message: message = 'Selected Debit/Credit Note No  To must be greater than or equal to Debit/Credit Note No From'

                       }
                ]
            },

              {
                  elements: "#AdvReturnVchNoTo",
                  rules: [
                         {

                             type: function (element) {
                                 var error = false;
                                 var to_id = clean($('#AdvReturnVchNoToID').val());
                                 var from_id = clean($('#AdvReturnVchNoFromID').val());
                                 if (to_id != 0) {
                                     if (to_id < from_id) {
                                         error = true;
                                     }
                                 }
                                 return !error;
                             },
                             message: message = 'Selected Adv Return No  To must be greater than or equal to Adv Return No From'

                         }
                  ]
              },

              {
                  elements: "#VoucherNoTo",
                  rules: [
                         {

                             type: function (element) {
                                 var error = false;
                                 var to_id = clean($('#VoucherToID').val());
                                 var from_id = clean($('#VoucherFromID').val());
                                 if (to_id != 0) {
                                     if (to_id < from_id) {
                                         error = true;
                                     }
                                 }
                                 return !error;
                             },
                             message: message = 'Selected Voucher No To must be greater than or equal to Voucher No From'

                         }
                  ]
              },

              {
                  elements: "#AccountCodeFrom",
                  rules: [
                      {
                          type: function (element) {
                              var error = false;
                              var ReportType = $('.GeneralLedger:checked').val();
                              var AccountCodeFrom = clean($('#AccountCodeFrom').val());
                              if (ReportType == "AccountCodewise") {
                                  if (AccountCodeFrom == 0) {
                                      error = true;
                                  }
                              }
                              return !error;
                          },
                          message: message = 'Please select account code or account name'
                      }
                  ]
              },

              {
                  elements: "#AccountCodeFromID:visible",
                  rules: [
                      {
                          type: function (element) {
                              var error = false;
                              var ReportType = $('.GeneralLedger:checked').val();
                              var AccountCodeFromID = clean($('#AccountCodeFromID').val());
                              if (ReportType == "AccountCodewise") {
                                  if (AccountCodeFromID == 0) {
                                      error = true;
                                  }
                              }
                              return !error;
                          },
                          message: message = 'Please select account code or account name'
                      }
                  ]
              },

             {
                 elements: "#AccountName:visible",
                 rules: [
                     {
                         type: function (element) {
                             var error = false;
                             var ReportType = $('.GeneralLedger:checked').val();
                             var AccountName = clean($('#AccountName').val());
                             if (ReportType == "AccountCodewise") {
                                 if (AccountName == 0) {
                                     error = true;
                                 }
                             }
                             return !error;
                         },
                         message: message = 'Please select account code or account name'
                     }
                 ]
             },

            {
                elements: "#AccountNameID:visible",
                rules: [
                    {
                        type: function (element) {
                            var error = false;
                            var ReportType = $('.GeneralLedger:checked').val();
                            var AccountNameID = clean($('#AccountNameID').val());
                            if (ReportType == "AccountCodewise") {
                                if (AccountNameID == 0) {
                                    error = true;
                                }
                            }
                            return !error;
                        },
                        message: message = 'Please select account code or account name'
                    }
                ]
            },
        ],
    },
};