SalesInvoice.get_item_properties = function (item) {
    var self = SalesInvoice;

    // item.OfferQty = self.get_offer_qty(item.Qty, item.ItemID);
    if (item.StoreID == 0) {
        item.StoreID = $("#StoreID").val();
    }

    if (item.SaleUnitID == item.UnitID) {
        item.MRP = item.Rate;
    }
    else {
        item.MRP = item.LooseRate;
    }
   
    item.OfferQtyMet = item.InvoiceOfferQtyMet ? "" : "offer-qty-not-met";
    item.QtyMet = item.InvoiceQtyMet ? "" : "qty-not-met";
    item.BasicPrice = item.MRP; // * 100 / (100 + item.IGSTPercentage);
    item.GrossAmount = item.BasicPrice * (item.InvoiceQty);
    item.AdditionalDiscount = item.BasicPrice * item.InvoiceOfferQty;
    item.DiscountAmount = (item.GrossAmount - item.AdditionalDiscount) * item.DiscountPercentage / 100;

    var CashDiscountPercentage = 0;
    if ($("#CashDiscountEnabled").is(":checked")) {
        CashDiscountPercentage = clean($("#CashDiscountPercentage").val());
    }

    item.CashDiscount = (item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount) * CashDiscountPercentage / 100;

    item.TurnoverDiscount = 0;
    item.TaxableAmount = 0;
    item.GSTAmount = 0;    
    item.NetAmount = 0;
    item.IsIncluded = true;
    return item;
}

SalesInvoice.set_item_calculations = function () {
    var self = SalesInvoice;
    var GrossAmount = 0;
    var DiscountAmount = 0;
    var AdditionalDiscount = 0;
    var CashDiscount = 0;
    var TurnoverDiscountAvailable = clean($("#TurnoverDiscountAvailable").val());    

    var IsIGST = self.is_igst();

    $(self.Items).each(function (i, item) {
        GrossAmount += item.GrossAmount;
        DiscountAmount += item.DiscountAmount;
        AdditionalDiscount += item.AdditionalDiscount;
        CashDiscount += item.CashDiscount;
    });

    var NetValue = GrossAmount - DiscountAmount - AdditionalDiscount - CashDiscount;

    if (NetValue > TurnoverDiscountAvailable) {
        TurnoverDiscount = TurnoverDiscountAvailable;
    } else {
        TurnoverDiscount = Math.round(NetValue) > 0 ? Math.round(NetValue) - 1 : 0;
    }

    $("#TurnoverDiscount").val(TurnoverDiscount);

    $(self.Items).each(function (i, item) {
        if (item.IsIncluded) {
            item.TurnoverDiscount = ((item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount) / (GrossAmount - DiscountAmount - AdditionalDiscount)) * TurnoverDiscount;
            item.TaxableAmount = item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount - item.TurnoverDiscount - item.CashDiscount;
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
            
        }
    });
}

SalesInvoice.get_discount_details = function () {
    var self = SalesInvoice;
    var CustomerID = $("#CustomerID").val();
    $.ajax({
        url: '/Sales/SalesInvoice/GetDiscountDetailsCustom',
        data: {
            CustomerID: CustomerID,
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                $("#CashDiscountPercentage").val(response.CashDiscountPercentage);
                $("#TurnoverDiscountAvailable").val(response.TurnoverDiscountAvailable);
            }
        }
    });
}