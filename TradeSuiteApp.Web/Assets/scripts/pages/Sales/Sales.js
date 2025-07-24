Sales = {
    get_customer_addresses: function () {
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Masters/Customer/GetAddresses/',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var html = "";
                    $.each(response.BillingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultBillingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + "</option>";
                    });
                    $("#BillingAddressID").html(html);
                    html = "";
                    $.each(response.ShippingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultShippingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + "</option>";
                    });
                    $("#ShippingAddressID").html(html);
                    if (response.Message !== undefined) {
                        app.show_error(response.Message);
                    }
                    
                } 
            },
            error: function (xhr, status, error) {
                // Handle the error here
                app.show_error(error);
            }
        });
    },

    is_igst: function () {
        return $("#StateID").val() != $("#LocationStateID").val();
    },

    is_gst_registered_customer: function () {
        return $("#IsGSTRegistered").val().toLowerCase() == "true" ? true : false;
    },

    is_cess_effect: function () {
        var self = Sales;
        if (self.is_igst()) {
            return false;
        }
        if (self.is_gst_registered_customer()) {
            return false;
        }
        return $("#LocationStateID").val() == 32;
    },

    sales_tickers() {
        $.ajax({
            url: '/User/Dashboard/SalesTickers',
            dataType: "json",
            type: "POST",
            success: function (response) {
                SignalRClient.notification.tickers("sales", response.Html, response.UserIDs);
            }
        });
    },
}