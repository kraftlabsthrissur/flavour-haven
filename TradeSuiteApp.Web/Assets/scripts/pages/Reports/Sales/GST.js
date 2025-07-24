$(function () {
    sales.bind_events();
    sales.gst_report_data_type();
});
sales = {
    bind_events: function () {
        $("#report-filter-form").on("submit", function () { return false; });
        $("#report-filter-submit").on("click", sales.get_report_view);
        $("#FromCustomerRange").on("change", sales.get_to_customerrange);
        $("#ItemFromRange").on("change", sales.get_to_itemrange);
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': sales.get_customername, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', sales.set_customername);
        $.UIkit.autocomplete($('#itemname-autocomplete'), { 'source': sales.get_itemname, 'minLength': 1 });
        $('#itemname-autocomplete').on('selectitem.uk.autocomplete', sales.set_itemname);
        $.UIkit.autocomplete($('#sales-invoiceno-autocomplete'), { 'source': sales.get_invoiceno_from, 'minLength': 1 });
        $('#sales-invoiceno-autocomplete').on('selectitem.uk.autocomplete', sales.set_invoiceno_from);
        $.UIkit.autocomplete($('#sales-invoicenoTo-autocomplete'), { 'source': sales.get_invoiceno_To, 'minLength': 1 });
        $('#sales-invoicenoTo-autocomplete').on('selectitem.uk.autocomplete', sales.set_invoiceno_To);
        $.UIkit.autocomplete($('#customer-gstno-autocomplete'), { 'source': sales.get_customer_gstno, 'minLength': 1 });
        $('#customer-gstno-autocomplete').on('selectitem.uk.autocomplete', sales.set_customer_gstno);
        $("#FromGSTRateRange").on("change", sales.get_gst_to);
        $("#Refresh").on("click", sales.refresh);
        $("#ItemCategoryID").change(sales.get_sales_category);
        $('.report_type').on("ifChanged", sales.gst_report_type)
        $('.report_data_type').on("ifChanged", sales.gst_report_data_type)
        
    },

    get_report_view: function () {
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        ReportHelper.hide_controls();
        switch (name){
            case "GSTReport":
                if (($("#InvoiceNoFrom").val() + $("#InvoiceNoTo").val()).trim() != ""){
                    if ($("#InvoiceNoFrom").val().trim() == "" || $("#InvoiceNoTo").val().trim() == ""){
                        filters += "Invoice No: " + $("#InvoiceNoFrom").val() + $("#InvoiceNoTo").val() + ", ";
                    }
                    else {
                        filters += "Invoice No: " + $("#InvoiceNoFrom").val() + " - " + $("#InvoiceNoTo").val() + ", ";
                    }
                }
                if ($("#CustomerTaxCategoryID").val() != ""){
                    filters += "Customer Tax Category: " +$("#CustomerTaxCategoryID Option:selected").text() + ", ";
                }

                if ($("#Customer").val() == "") {
                    if ($("#FromCustomerRange").val() != "")
                        filters += "Customer name from: " + $("#FromCustomerRange").val() + ", ";

                    if ($("#ToCustomerRange").val() != "") {
                        filters += " - " + $("#ToCustomerRange").val() + ", ";
                    }
                }
                else filters += "customer: " + $("#Customer").val() + ", ";
              
                if ($("#ItemCategoryID").val() != "") {
                    filters += "Item Category: " + $("#ItemCategoryID Option:selected").text() + ", ";
                }
                if ($("#SalesCategoryID").val() != "") {
                    filters += "Sales Category: " + $("#SalesCategoryID Option:selected").text() + ", ";
                }

                if ($("#ItemName").val() == "") {
                    if ($("#ItemFromRange").val() != "")
                        filters += "Item Name from: " + $("#ItemFromRange").val();

                    if ($("#ItemToRange").val() != "") {
                        filters += " - " + $("#ItemToRange").val();
                    }
                }
                else filters += "Item: " + $("#ItemName").val() + ", ";

                if ($("#CustomerGSTNo").val() != "") {
                    filters += "GST No: " + $("#CustomerGSTNo").val() + ", ";
                }

                if (($("#FromGSTRateRange").val() + $("#ToGSTRateRange").val()).trim() != "") {
                    if ($("#FromGSTRateRange").val().trim() == "" || $("#ToGSTRateRange").val().trim() == "") {
                        filters += "GST Rate: " + $("#FromGSTRateRange").val() + $("#ToGSTRateRange").val() + ", ";
                    }
                    else {
                        filters += " GST Rate: " + $("#FromGSTRateRange").val() + " - " + $("#ToGSTRateRange").val() + ", ";
                    }
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
                $("#report-viewer").html(response);
                ReportHelper.inject_js();
            }
        })
        return false;
    },

    get_to_customerrange() {
        var self = sales;
        var from_range = $("#FromCustomerRange").val();
        $.ajax({
            url: '/Reports/Sales/GetCustomerRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToCustomerRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToCustomerRange").append(html);
            }
        })
    },

    get_to_itemrange() {
        var self = sales;
        var from_range = $("#ItemFromRange").val();
        $.ajax({
            url: '/Reports/Sales/GetItemNameRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ItemToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ItemToRange").append(html);
            }
        })
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
        self = sales;
        $("#Customer").val(item.name);
        $("#CustomerID").val(item.id);

    },

    get_itemname: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    set_itemname: function (event, item) {
        self = sales;
        $("#ItemName").val(item.code);
        $("#ItemID").val(item.id);
    },

    get_invoiceno_from: function (release) {
        Table = 'SalesInvoiceNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#InvoiceNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_invoiceno_from: function (event, item) {
        self = sales;
        $("#InvoiceNoFrom").val(item.code);
        $("#InvoiceNoFromID").val(item.id);
    },

    get_invoiceno_To: function (release) {

        Table = 'SalesInvoiceNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#InvoiceNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_invoiceno_To: function (event, item) {
        self = sales;
        $("#InvoiceNoTo").val(item.code);
        $("#InvoiceNoToID").val(item.id);
    },

    get_customer_gstno: function (release) {

        Table = 'CustomerGSTNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerGSTNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_customer_gstno: function (event, item) {
        self = sales;
        $("#CustomerGSTNo").val(item.code);
        $("#CustomerGSTNoID").val(item.id);
    },

    get_gst_to: function (e) {
        console.log(e);
        var self = sales;
       var FromGst = clean($("#FromGSTRateRange").val());
        if (FromGst == null || FromGst == "Select") {
            FromGst = 0.0;
        }
        $.ajax({
            url: '/Reports/GST/GetGstToRate/' + FromGst,
            data: { FromGst: FromGst },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#ToGSTRateRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.IGSTPercentage + "'>" + record.IGSTPercentage.toFixed(2) + "</option>";
                });
                $("#ToGSTRateRange").append(html);
            }
        });
    },

    get_sales_category: function () {
        var self = sales;
        var item_category_id = $(this).val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = 0;
        }
        $.ajax({
            url: '/Masters/Category/GetSalesCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#SalesCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#SalesCategoryID").append(html);
            }
        });
    },

    refresh: function () {
        var self = sales;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#InvoiceDateFrom").val(findate);
        $("#InvoiceDateTo").val(currentdate);
        $('#LocationID').val('');
        $('#InvoiceNoFrom').val('');
        $('#InvoiceNoFromID').val('');
        $('#InvoiceNoTo').val('');
        $('#InvoiceNoToID').val('');
        $('#FromCustomerRange').val('');
        $('#ToCustomerRange').val('');
        $('#Customer').val('');
        $('#ItemCategoryID').val('');
        $('#SalesCategoryID').val('');
        $('#ItemFromRange').val('');
        $('#ItemToRange').val('');
        $('#ItemName').val('');
        $('#ItemID').val('');
        $('#CustomerGSTNo').val('');
        $('#FromGSTRateRange').val(''); 
        $('#ToGSTRateRange').val('');
        $('#CustomerTaxCategoryID').val('');
        $("#CustomerCodeFromID").val('');
        $("#CustomerID").val('');
    },

    gst_report_type: function () {
        var self = sales;
        var report_type = "";
        report_type = $('.report_type:checked').val();
        if (report_type == "Sales Output GST") {
            $(".gst_item_type").addClass('uk-hidden');
        }
        else {
            $(".gst_item_type").addClass('uk-hidden');
        }
    },

    gst_report_data_type: function () {
        var self = sales;
        var report_data_type = $('input[name=ReportDataType]:checked').val();
        $('.filters').addClass('uk-hidden');
        $("." + report_data_type).removeClass('uk-hidden');
        self.refresh();
    },
};