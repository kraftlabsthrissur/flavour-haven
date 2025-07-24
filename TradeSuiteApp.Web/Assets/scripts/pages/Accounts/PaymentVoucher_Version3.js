voucherCRUD.init= function () {
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
};

voucherCRUD.bind_events = function () {
    var self = voucherCRUD;
    $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
    $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
    $("#btnOKSupplier").on('click', self.select_supplier);
    $("#dropPayment").on("change", self.get_bank_name);
    $('body').on('ifChanged', '.include', self.include_item);
    $('.btnSaveAndPost, .btnSaveASDraft').on('click', self.on_save);
    $('.txtPayNow').on('keyup', self.calculate_sum);
    $("#btnAdd").on("click", self.add_item);
    $("#btnProcess").on("click", self.process_item);
    
    $("body").on("ifChanged", "#OtherPayment", self.show_other_payments);


};
voucherCRUD.add_item = function (item) {
    var self = voucherCRUD;
    self.error_count = self.validate_add_other_payment();
    if (self.error_count > 0) {
        return;
    }
    var TotalInvoiceAmt = clean($('#tblUnSettledPurchaseInvoice tbody tr#total-row').find("#TotalInvoiceAmt").val());
    var TotBalance = clean($('#tblUnSettledPurchaseInvoice tbody tr#total-row').find("#TotBalance").val());
    var TotPayNow = clean($('#tblUnSettledPurchaseInvoice tbody tr#total-row').find("#TotPayNow").val());
    $('#tblUnSettledPurchaseInvoice tbody tr#total-row').remove();
    var SupplierName = $("#SupplierName").val();
    var DocumentNo = $("#DocumentNo").val();
    var Narration = $(".Narrations").val();
    var Date = $("#Date").val();
    var DocumentAmount = clean($("#Amount").val());
    var CurrencyExchangeRate = clean($("#CurrencyExchangeRate").val());
    TotalInvoiceAmt = (TotalInvoiceAmt + DocumentAmount);
    TotBalance = (TotBalance + DocumentAmount);
    TotPayNow = (TotPayNow + DocumentAmount)
    var index, tr;
    index = $("#tblUnSettledPurchaseInvoice tbody tr").length + 1;
    tr = '<tr>'
            + ' <td class="uk-text-center">' + index 
            + ' <input type="hidden" class="hdnDocType" value="' + 'DirectPayable' + '" />'
            + ' <input type="hidden" class="hdnDocNo" value="' + 0 + '" />'
            + ' <input type="hidden" class="hdnAdvanceID" value="' + 0 + '" />'
            + ' <input type="hidden" class="hdnDebitNoteID" value="' + 0 + '" />'
            + ' <input type="hidden" class="hdnCreditNoteID" value="' + 0 + '" />'
            + ' <input type="hidden" class="hdnIRGID" value="' + 0 + '" />'
            + ' <input type="hidden" class="hdnCreatedDate" value="' + Date + '" />'
            + ' <input type="hidden" class="hdnDueDate" value="' + Date + '" />'
            + ' <input type="hidden" class="hdnPaymentReturnVoucherTransID" value="' + 0 + '" />'
            + ' </td>'
            + '<td><input type="checkbox" data-md-icheck checked class="include" /></td>'
            + ' <td class="documenttype">' + "DirectPayable" + '</td>'
            + '<td>' + '<input type="text" class="md-input Narrations" value="' + Narration + '"  />' + '</td>'
            + '<td>' + DocumentNo + '</td>'
            +'<td class="date">'+Date+'</td>'
            + '<td class="mask-currency invoiceAmt">' + DocumentAmount + '</td>'
            +'<td class="balance">'
            + '<input type="text" class="md-input label-fixed mask-currency txtAmtToPaid decimalnum" value="' + DocumentAmount + '"  readonly />'
            +'</td>'
            +'<td class="uk-text-right balance">'
            + '<input type="text" class="md-input txtPayNow mask-positive-currency " value="' + DocumentAmount + '" readonly />'
            + '</td>'

            + '<tr id="total-row">'
            +'<td colspan="6" class="bold">'+'Total' +'</td>'
            + '<td class="mask-currency bold" id="TotalInvoiceAmt">' + TotalInvoiceAmt + '</td>'
            + '<td class="mask-currency bold" id="TotBalance">' + TotBalance + '</td>'
            + '<td class="mask-currency bold" id="TotPayNow">' + TotPayNow + '</td>'
            +'</tr>'

            + '</tr>';

    $("#item-count").val(index);
    var $tr = $(tr);
    app.format($tr);
    $("#tblUnSettledPurchaseInvoice tbody").append($tr);
    $("#NetAmt").val(TotPayNow);
    $("#LocalNetAmt").val(TotPayNow * CurrencyExchangeRate);
    self.clear_item();
};
voucherCRUD.clear_item = function () {
    $("#DocumentNo").val('');
    $("#Date").val('');
    $("#Amount").val('');
    $("#Narration").val('');

};