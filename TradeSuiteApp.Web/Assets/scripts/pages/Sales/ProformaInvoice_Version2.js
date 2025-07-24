ProformaInvoice.get_item_properties = function (item) {
    var self = ProformaInvoice;
    var IsIGST = self.is_igst();
    item.StoreID = $("#StoreID").val();
    item.OfferQtyMet = item.InvoiceOfferQtyMet ? "" : "offer-qty-not-met";
    item.QtyMet = item.InvoiceQtyMet ? "" : "qty-not-met";
    if (item.SalesUnitID == item.UnitID) {
        item.MRP = item.Rate;
    }
    else {
        item.MRP = item.LooseRate;
    }
    item.BasicPrice = item.MRP;//  * 100 / (100 + item.IGSTPercentage);
    item.GrossAmount = item.BasicPrice * (item.InvoiceQty);
    item.AdditionalDiscount = item.BasicPrice * item.InvoiceOfferQty;
    item.DiscountAmount = (item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100;
    item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount;
    item.GSTAmount = item.TaxableAmount * item.IGSTPercentage / 100;
    if (IsIGST) {
        item.IGST = item.GSTAmount;
        item.SGST = 0;
        item.CGST = 0;
    } else {
        item.IGST = 0;
        item.SGST = item.GSTAmount / 2;
        item.CGST = item.GSTAmount / 2;
    }
    item.NetAmount = item.TaxableAmount + item.GSTAmount;
    return item;
}
ProformaInvoice.get_units = function () {
    var self = ProformaInvoice;
    $("#UnitID").html("");
    var html;
    html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
    html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
    $("#UnitID").append(html);
}