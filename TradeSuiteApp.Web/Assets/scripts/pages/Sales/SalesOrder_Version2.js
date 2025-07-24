SalesOrder.get_item_properties = function (item) {
    var self = SalesOrder;
    item.OfferQty = self.get_offer_qty(item.Qty, item.ID, item.UnitID);
    
    item.BasicPrice = item.MRP; // * 100 / (100 + item.IGSTPercentage);
    item.GrossAmount = item.BasicPrice * (item.Qty + item.OfferQty);
    item.DiscountAmount = item.Qty * item.BasicPrice * item.DiscountPercentage / 100;
    item.AdditionalDiscount = item.BasicPrice * item.OfferQty;
    item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount;
    item.GSTAmount = item.TaxableAmount * item.IGSTPercentage / 100;
    item.NetAmount = item.TaxableAmount + item.GSTAmount;
    var isapproved = 0;
    $("#IsApproved").val(isapproved);
    if ($("#StateID").val() == $("#LocationStateID").val()) {
        item.GSTPercentage = item.IGSTPercentage;
        item.CGST = item.GSTAmount / 2;
        item.SGST = item.GSTAmount / 2;
        item.IGST = 0;
    } else {
        item.GSTPercentage = item.IGSTPercentage;
        item.CGST = 0;
        item.SGST = 0;
        item.IGST = item.GSTAmount;
    }
    return item;
}
SalesOrder.get_units = function () {
    var self = SalesOrder;
    $("#UnitID").html("");
    var html;
    html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
    html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
    $("#UnitID").append(html);
}
