
var gcurrencyclass = '';

DirectPurchaseInvoice.add_item = function () {
    var self = DirectPurchaseInvoice
    var FormType = "Add";
    if (self.validate_form(FormType) > 0) {
        return;
    }
    var PurchaseCategoryID = $("#DDLPurchaseCategory").val();
    var SupplierStateID = clean($("#SupplierStateID").val());
    var ItemID = $("#ItemID").val();
    var ItemName = $("#ItemName").val();
    var ItemCode = $("#ItemCode").val();
    var UnitID = $("#UnitID").val();
    var Unit = $("#UnitID Option:selected").text();
    var Qty = clean($("#Qty").val());
    var GSTPercentage = clean($("#GSTPercentage").val());
    var MRP = clean($("#MRP").val());
    var Rate = clean($("#Rate").val());
    var RetailMRP = clean($("#RetailMRP").val());
    var RetailRate = clean($("#RetailRate").val());
    var DiscountPercent = $("#DiscountPercent").val();
    var Discount = (DiscountPercent * (Qty * MRP)) / 100;
    var value = (Qty * MRP) - Discount;
    var BatchNo = $("#BatchNo").val();
    var IsGST = $("#IsGST").val();
    var IsVat = $("#IsVat").val();

    var PartsNumber = $("#PartsNumber").val();
    var Remark = $("#Remark").val();
    var Model = $("#Model").val();
    var CurrencyID = $("#CurrencyID").val();
    var CurrencyName = $("#CurrencyName").val();
    var IsGSTRegistered = $("#IsGSTRegistered").val();
    var VATPercentage = $("#VATPercentage").val();
    var SecondaryUnits = $("#SecondaryUnits").val();
    var ExchangeRate = clean($("#CurrencyConversionRate").val());
    var GSTAmnt = 0;
    var CGSTPercent = 0;
    var SGSTPercent = 0;
    var IGSTPercent = 0;
    var VATPercent = 0;
    var CGSTAmt = 0;
    var SGSTAmt = 0;
    var IGSTAmt = 0;
    var VATAmt = 0;

    var VATAmount = 0;
    var TotalAmnt = 0;
    if (IsGST == 1) {
        if (IsGSTRegistered == "true") {
            if (SupplierStateID == 32) {
                CGSTPercent = GSTPercentage / 2;
                SGSTPercent = GSTPercentage / 2;
                IGSTPercent = 0
            }
            else {
                CGSTPercent = 0;
                SGSTPercent = 0;
                IGSTPercent = GSTPercentage
            }
        }
        else {
            CGSTPercent = 0
            IGSTPercent = 0
            SGSTPercent = 0
            GSTPercentage = 0
        }
        CGSTAmt = value * CGSTPercent / 100;
        SGSTAmt = value * SGSTPercent / 100;
        IGSTAmt = value * IGSTPercent / 100;
        GSTAmnt = CGSTAmt + SGSTAmt + IGSTAmt;
        TotalAmnt = value + GSTAmnt;

    }
    if (IsVat == 1) {

        VATAmount = value * VATPercentage / 100;
        TotalAmnt = value + VATAmount;

    }
    var content = "";
    var $content;
    var sino = "";
    gcurrencyclass = $("#GridCurrencyClass").val();
    sino = $('#purchase-order-items-list tbody tr').length + 1;
    if (IsGST == 1) {

        content = '<tr>'
            + '<td class="uk-text-center serial-no">' + sino + '</td>'
            + '<td>' + ItemCode + '</td>'
            + '<td>' + ItemName
            + '<input type="hidden" class = "ItemID" value="' + ItemID + '" />'
            + '<input type="hidden" class = "ItemName" value="' + ItemName + '" />'
            + '<input type="hidden" class = "ItemCode" value="' + ItemCode + '" />'
            + '<input type="hidden" class = "PartsNumber" value="' + PartsNumber + '"  />'
            + '<input type="hidden" class = "Remark" value="' + Remark + '"  />'
            + '<input type="hidden" class = "Model" value="' + Model + '"  />'
            + '<input type="hidden" class = "UnitID" value="' + UnitID + '" />'
            + '<input type="hidden" class = "CurrencyID" value="' + CurrencyID + '"  />'
            + '<input type="hidden" class = "PurchaseCategoryID" value="' + PurchaseCategoryID + '" />'
            + '<input type="hidden" class = "CGSTPercent" value="' + CGSTPercent + '" />'
            + '<input type="hidden" class = "SGSTPercent" value="' + SGSTPercent + '" />'
            + '<input type="hidden" class = "IGSTPercent" value="' + IGSTPercent + '" />'
            + '<input type="hidden" class = "SGSTAmt" value="' + SGSTAmt + '" />'
            + '<input type="hidden" class = "IGSTAmt" value="' + IGSTAmt + '" />'
            + '<input type="hidden" class = "CGSTAmt" value="' + CGSTAmt + '" />'
            + '<input type="hidden" class = "BatchTypeID" value="' + 1 + '" />'
            + '<input type="hidden" class = "Rate" value="' + Rate + ' " /></td>'
            + '<input type="hidden" class = "RetailMRP"  value="' + RetailMRP + ' " />'
            + '<input type="hidden" class = "RetailRate" value="' + RetailRate + ' " />'
            + '</td>'
          //  + '<td type="hidden">' + PartsNumber + '</td>'
            + '<td>' + Remark + '</td>'
            + '<td>' + Model + '</td>'
            + '<td>' + Unit + '</td>'
            + '<td>' + CurrencyName + '</td>'
            //+ '<td><input type="text" class = "md-input BatchNo" value="' + BatchNo + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-currency Qty" value="' + Qty + '" /></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' MRP " disabled value="' + MRP + ' " /></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' DiscountPercent" value="' + DiscountPercent + '"/></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' Discount" value="' + Discount + '"/></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' TaxableAmount" value="' + value + '" disabled /></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' GSTPercentage" value="' + GSTPercentage + '" disabled /></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' GSTAmnt" value="' + GSTAmnt + '" disabled /></td>'
            + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' TotalAmnt" value="' + TotalAmnt + '" disabled /></td>'
            /* + '<td><input type="text" class = "md-input ExpDate" value="' + ExpDate + '" disabled /></td>'*/
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';

    }

    else if (IsVat == 1) {
        {
            content = '<tr>'
                + '<td class="uk-text-center serial-no">' + sino + '</td>'
                + '<td>' + ItemCode + '</td>'
                + '<td>' + ItemName
                + '<input type="hidden" class = "ItemID" value="' + ItemID + '" />'
                + '<input type="hidden" class = "ItemName" value="' + ItemName + '" />'
                + '<input type="hidden" class = "ItemCode" value="' + ItemCode + '" />'
                + '<input type="hidden" class = "PartsNumber" value="' + PartsNumber + '"  />'
                + '<input type="hidden" class = "Remark" value="' + Remark + '"  />'
                + '<input type="hidden" class = "Model" value="' + Model + '"  />'
                + '<input type="hidden" class = "UnitID" value="' + UnitID + '" />'
                + '<input type="hidden" class = "CurrencyID" value="' + CurrencyID + '"  />'
                + '<input type="hidden" class = "PurchaseCategoryID" value="' + PurchaseCategoryID + '" />'
                + '<input type="hidden" class = "VATPercent" value="' + VATPercent + '" />'
                + '<input type="hidden" class = "VATAmt" value="' + VATAmt + '" />'
                + '<input type="hidden" class = "BatchTypeID" value="' + 1 + '" />'
                + '<input type="hidden" class = "Rate" value="' + Rate + ' " />/*</td>*/'
                + '<input type="hidden" class = "RetailMRP"  value="' + RetailMRP + ' " />'
                + '<input type="hidden" class = "RetailRate" value="' + RetailRate + ' " />'
                + '<input type="hidden" class = "ExchangeRate" value="' + ExchangeRate + ' " />'
                + '<input type="hidden" class = "BatchNo" value="' + BatchNo + ' " />'
                + '</td>'
              //  + '<td type="hidden">' + PartsNumber + '</td>'
                + '<td>' + Remark + '</td>'
                + '<td>' + Model + '</td>'
                + '<td class="uk-hidden Unit">' + Unit + '</td>'
                + '<td class="secondary">' + self.SelectSecondaryUnits(Unit, SecondaryUnits) + '</td>'
                + '<td>' + CurrencyName + '</td>'
                + '<td class="uk-hidden"><input type="text" class ="md-input mask-production-qty Qty" value="' + Qty + '" /></td>'
                + '<td class="secondary"><input type="text" class="md-input mask-production-qty secondaryQty" value="' + Qty + '" /></td>'
                + '<td class="uk-hidden"><input type="text" class ="md-input ' + gcurrencyclass + ' MRP" value="' + MRP + ' " /></td>'
                + '<td class="secondary"><input type="text" class="md-input secondaryRate ' + gcurrencyclass + '" value="' + MRP + '" /></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' DiscountPercent" value="' + DiscountPercent + '"/></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' Discount" value="' + Discount + '"/></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' TaxableAmount" value="' + value + '" disabled /></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' VATPercentage" value="' + VATPercentage + '" disabled /></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' VATAmount" value="' + VATAmount + '" disabled /></td>'
                + '<td><input type="text" class = "md-input ' + gcurrencyclass + ' TotalAmnt" value="' + TotalAmnt + '" disabled /></td>'
                /*   + '<td><input type="text" class = "md-input ExpDate" value="' + ExpDate + '" disabled /></td>'*/
                + '<td>'
                + '<a class="remove-item">'
                + '<i class="uk-icon-remove"></i>'
                + '</a>'
                + '</td>'
                + '</tr>';

        }
    }
    $content = $(content);
    app.format($content);
    $('#purchase-order-items-list tbody').append($content);
    index = $("#purchase-order-items-list tbody tr").length;
    $("#item-count").val(index);
    self.calculate_grid_total();
    self.clear();
    $("#ItemName").focus();
    freeze_header.resizeHeader();
};