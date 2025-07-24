$(function () {
    sales.bind_events();
});

sales = {
    bind_events: function () {
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('.MIS_type').on("ifChanged", sales.mis_report_type)
        $('.Report_Type').on("ifChanged", sales.show_mis_summary_detail)
        $('.Sales_Order_ReportType').on("ifChanged", sales.show_sales_order_type)
        $('.cheque_report_name').on("ifChanged", sales.cheque_report_type)
        $('.sales_report_type').on("ifChanged", sales.sales_report_type)
        $('.cs_report_type').on("ifChanged", sales.counter_sales_type)
        $('.ReportType').on("ifChanged", sales.sales_report_summary_type)
        $("#report-filter-form").on("submit", function () { return false; });
        $("#report-filter-submit").on("click", sales.get_report_view);
        $.UIkit.autocomplete($('#fsoname-autocomplete'), { 'source': sales.get_fsoname, 'minLength': 1 });
        $('#fsoname-autocomplete').on('selectitem.uk.autocomplete', sales.set_fsoname);

        $.UIkit.autocomplete($('#sales-invoiceno-autocomplete'), { 'source': sales.get_invoiceno_from, 'minLength': 1 });
        $('#sales-invoiceno-autocomplete').on('selectitem.uk.autocomplete', sales.set_invoiceno_from);
        $.UIkit.autocomplete($('#sales-invoicenoTo-autocomplete'), { 'source': sales.get_invoiceno_To, 'minLength': 1 });
        $('#sales-invoicenoTo-autocomplete').on('selectitem.uk.autocomplete', sales.set_invoiceno_To);
        $.UIkit.autocomplete($('#sales-customercodefrom-autocomplete'), { 'source': sales.get_customercode_from, 'minLength': 1 });
        $('#sales-customercodefrom-autocomplete').on('selectitem.uk.autocomplete', sales.set_customercode_from);
        $.UIkit.autocomplete($('#sales-customercodeTo-autocomplete'), { 'source': sales.get_customercode_To, 'minLength': 1 });
        $('#sales-customercodeTo-autocomplete').on('selectitem.uk.autocomplete', sales.set_customercode_To);
        $("#FromCustomerRange").on("change", sales.get_to_customerrange);
        $("#FromCategoryRange").on("change", sales.get_to_categoryrange);
        $("#ItemFromRange").on("change", sales.get_to_itemrange);
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': sales.get_customername, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', sales.set_customername);
        $.UIkit.autocomplete($('#sales-itemcodefrom-autocomplete'), { 'source': sales.get_itemcode_from, 'minLength': 1 });
        $('#sales-itemcodefrom-autocomplete').on('selectitem.uk.autocomplete', sales.set_itemcode_from);
        $.UIkit.autocomplete($('#sales-itemcodeTo-autocomplete'), { 'source': sales.get_itemcode_To, 'minLength': 1 });
        $('#sales-itemcodeTo-autocomplete').on('selectitem.uk.autocomplete', sales.set_itemcode_To);
        $.UIkit.autocomplete($('#itemname-autocomplete'), { 'source': sales.get_itemname, 'minLength': 1 });
        $('#itemname-autocomplete').on('selectitem.uk.autocomplete', sales.set_itemname);
        $.UIkit.autocomplete($('#sales-orderno-autocomplete'), { 'source': sales.get_salesorderno_from, 'minLength': 1 });
        $('#sales-orderno-autocomplete').on('selectitem.uk.autocomplete', sales.set_salesorderno_from);
        $.UIkit.autocomplete($('#sales-ordernoTo-autocomplete'), { 'source': sales.get_salesorderno_To, 'minLength': 1 });
        $('#sales-ordernoTo-autocomplete').on('selectitem.uk.autocomplete', sales.set_salesorderno_To);
        $('#Refresh').on('click', sales.refresh);
        $.UIkit.autocomplete($('#itemcode-from-autocomplete'), { 'source': sales.get_item_code_from, 'minLength': 1 });
        $('#itemcode-from-autocomplete').on('selectitem.uk.autocomplete', sales.set_item_code_from);
        $.UIkit.autocomplete($('#itemcode-to-autocomplete'), { 'source': sales.get_item_code_to, 'minLength': 1 });
        $('#itemcode-to-autocomplete').on('selectitem.uk.autocomplete', sales.set_item_code_to);
        $.UIkit.autocomplete($('#receipt-no-from-autocomplete'), { 'source': sales.get_receiptno_from, 'minLength': 1 });
        $('#receipt-no-from-autocomplete').on('selectitem.uk.autocomplete', sales.set_receiptno_from);
        $.UIkit.autocomplete($('#receipt-no-to-autocomplete'), { 'source': sales.get_receiptno_to, 'minLength': 1 });
        $('#receipt-no-to-autocomplete').on('selectitem.uk.autocomplete', sales.set_receiptno_to);
        $.UIkit.autocomplete($('#sales-customercode-autocomplete'), { 'source': sales.get_customercode, 'minLength': 1 });
        $('#sales-customercode-autocomplete').on('selectitem.uk.autocomplete', sales.set_customercode);
        $.UIkit.autocomplete($('#cheque-no-autocomplete'), { 'source': sales.get_cheque_No, 'minLength': 1 });
        $('#cheque-no-autocomplete').on('selectitem.uk.autocomplete', sales.set_cheque_No);

        $.UIkit.autocomplete($('#account-no-autocomplete'), { 'source': sales.get_account_No, 'minLength': 1 });
        $('#account-no-autocomplete').on('selectitem.uk.autocomplete', sales.set_account_No);


        $.UIkit.autocomplete($('#patient-autocomplete'), { 'source': sales.get_patient_No, 'minLength': 1 });
        $('#patient-autocomplete').on('selectitem.uk.autocomplete', sales.set_patient_No);

        $("#ItemCategoryID").change(sales.get_sales_category);

        $.UIkit.autocomplete($('#ageing-bucket-autocomplete'), { 'source': sales.get_ageing_bucket, 'minLength': 1 });
        $('#ageing-bucket-autocomplete').on('selectitem.uk.autocomplete', sales.set_ageing_bucket);
        

        
        function init() {
            sales.load_time_hide();
        }


        //$("#LocationFromID").on("click", sales.get_locations);
    },
    get_locations: function () {
        //var self = sales;
        //$('#LocationFromID').selectize({
        //    persist: false,
        //    maxItems: null,
        //});
    },

    load_time_hide: function () {
        var self = sales;
        $('.uk-hidden').hide();
    },

    mis_report_type: function () {
        var self = sales;
        var report_type = $(this).val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        self.show_mis_summary_detail();
        self.refresh();
    },

    show_mis_summary_detail: function () {
        var self = sales;
        var report_type = $('.MIS_type:Checked').val();
        var summar_detail = $('.ReportType:Checked').val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        $("." + summar_detail).removeClass("uk-hidden");
    },

    show_sales_order_type: function () {
        var self = sales;
        var report_type = "";aa
        var report_type = $('.Sales_Order_ReportType:checked').val();
        if (report_type == "Detail") {
            $(".detail").removeClass('uk-hidden');
        }
        else {
            $(".detail").addClass('uk-hidden');
        }
    },

    cheque_report_type: function () {
        var self = sales;
        var report_type = $(this).val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        self.refresh();
    },

    counter_sales_type: function () {
        var self = sales;
        var report_type = $(".cs_report_type:Checked").val();
        if (report_type == "Detail") {
            $(".itemname").removeClass("uk-hidden");
            $(".doctorname").removeClass("uk-hidden");
            $(".paymentterms").removeClass("uk-hidden");
        }
        else if (report_type == "ScheduleH")
        {
            $(".itemname").addClass("uk-hidden");
            $(".doctorname").addClass("uk-hidden");
            $(".paymentterms").addClass("uk-hidden");
            
        }
        else
        {
            $(".itemname").addClass("uk-hidden");
            $(".doctorname").removeClass("uk-hidden");
            $(".paymentterms").removeClass("uk-hidden");
        }
    },

    sales_report_type: function () {
        var self = sales;
        var report_type = $("sales_report_type:Checked").val();
        var summary_deatil = $(".ReportType:Checked").val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        self.sales_report_summary_type();
    },

    sales_report_summary_type: function () {
        var self = sales;
        var report_type = $(".sales_report_type:Checked").val();
        var summary_deatil = $(".ReportType:Checked").val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        if (report_type == "SalesByBranch") {
            if (summary_deatil == "Summary") {
                $(".itemname").addClass("uk-hidden");
                $(".salescategory").addClass("uk-hidden");
                $(".location").show();
                $(".itemcategory").addClass("uk-hidden");
                //$(".batch_type").addClass("uk-hidden");
            }
            else {
                $(".itemname").removeClass("uk-hidden");
                $(".salescategory").removeClass("uk-hidden");
                $(".location").show();
                $(".itemcategory").removeClass("uk-hidden");
                //$(".batch_type").addClass("uk-hidden");
            }
        }
        else if (report_type == "SalesByItem") {
            if (summary_deatil == "Summary") {
                $(".location").show();
                $(".itemcategory").removeClass("uk-hidden");
                //$(".batch_type").addClass("uk-hidden");
            }
            else {
                $(".location").show();
                $(".itemcategory").removeClass("uk-hidden");
                //$(".batch_type").addClass("uk-hidden");
            }
        }
        else if (report_type == "SalesByCustomer") {
            if (summary_deatil == "Summary") {
                $(".invoiceno").addClass("uk-hidden");
                $(".customercode").addClass("uk-hidden");
                $(".customerrange").addClass("uk-hidden");
                $(".customername").addClass("uk-hidden");
                $(".itemcode").addClass("uk-hidden");
                $(".itemrange").addClass("uk-hidden");
                $(".itemname").addClass("uk-hidden");
                $(".location").show();
                $(".itemcategory").removeClass("uk-hidden");
                //$(".batch_type").removeClass("uk-hidden");
            }
            else {
                $(".invoiceno").removeClass("uk-hidden");
                $(".customercode").removeClass("uk-hidden");
                $(".customerrange").removeClass("uk-hidden");
                $(".customername").removeClass("uk-hidden");
                $(".itemcode").removeClass("uk-hidden");
                $(".itemrange").removeClass("uk-hidden");
                $(".itemname").removeClass("uk-hidden");
                $(".location").show();
                $(".itemcategory").removeClass("uk-hidden");
                //$(".batch_type").removeClass("uk-hidden");
            }
        }
        else if (report_type == "SalesByFSO") {
            if (summary_deatil == "Summary") {
                $(".invoiceno").addClass("uk-hidden");
                $(".customercode").addClass("uk-hidden");
                $(".customerrange").addClass("uk-hidden");
                $(".customername").addClass("uk-hidden");
                $(".location").show();
                //$(".batch_type").removeClass("uk-hidden");
                $(".salesincentive").removeClass("uk-hidden")
                $(".fsoname").removeClass("uk-hidden")
              
            }
            else {
                $(".invoiceno").removeClass("uk-hidden");
                $(".customercode").removeClass("uk-hidden");
                $(".customerrange").removeClass("uk-hidden");
                $(".customername").removeClass("uk-hidden");
                $(".location").show();
                //$(".batch_type").removeClass("uk-hidden");
                $(".salesincentive").removeClass("uk-hidden")
                $(".fsoname").removeClass("uk-hidden")
                
            }
        }
        else if (report_type == "invoice-status") {
            if (summary_deatil == "Summary") {
                $(".itemcategory").addClass("uk-hidden");
                $(".itemcode").addClass("uk-hidden");
                $(".itemrange").addClass("uk-hidden");
                $(".itemname").addClass("uk-hidden");
                $(".batch_type").addClass("uk-hidden");
                $(".location").show();
            }
            else {
                $(".itemcategory").addClass("uk-hidden");
                $(".itemcode").addClass("uk-hidden");
                $(".itemrange").addClass("uk-hidden");
                $(".itemname").addClass("uk-hidden");
                $(".batch_type").addClass("uk-hidden");
                $(".location").show();
            }
        }
            //else if (report_type == "receipt-voucher") {
            //    if (summary_deatil == "Summary") {
            //    }
            //}
        else {
            if (summary_deatil == "Summary") {

                //$(".customercode").removeClass("uk-hidden");
                //$(".customerrange").removeClass("uk-hidden");
                $(".customername").removeClass("uk-hidden");
                $(".itemcode").addClass("uk-hidden");
                $(".itemrange").addClass("uk-hidden");
                $(".itemname").addClass("uk-hidden");
                $(".customercode").addClass("uk-hidden");
                $(".customerrange").addClass("uk-hidden");
                $(".location").hide();
                //$(".batch_type").addClass("uk-hidden");

            }
            else {
                //$(".customercode").removeClass("uk-hidden");
                //$(".customerrange").removeClass("uk-hidden");
                $(".customername").removeClass("uk-hidden");
                $(".itemcode").addClass("uk-hidden");
                $(".itemrange").addClass("uk-hidden");
                $(".itemname").addClass("uk-hidden");
                $(".customercode").addClass("uk-hidden");
                $(".customerrange").addClass("uk-hidden");
                $(".location").hide();
                //$(".batch_type").addClass("uk-hidden");
            }
        }
        $("#InvoiceNOFromID").val('');
        $("#InvoiceNOFrom").val('');
        $("#InvoiceNOTo").val('');
        $("#InvoiceNOToID").val('');
        $("#ReceiptNoFrom").val(''); 
        $("#ReceiptNoFromID").val('');
        $("#ReceiptNoTo").val('');
        $("#ReceiptNoToID").val('');
        $("#CustomerCodeFrom").val('');
        $("#CustomerCodeFromID").val('');
        $("#FromCustomerRange").val('');
        $("#ToCustomerRange").val('');
        $("#Customer").val('');
        $("#CustomerID").val('');

    },

    get_report_view: function () {
        var self = sales;
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        ReportHelper.hide_controls();
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";

        switch (name) {
            case "SalesOrder":
                break;
            case "SalesReports":
                filters += self.get_invoice_no();
                filters += self.get_customer_details();
                filters += self.get_item_details();
                filters += self.get_receipt_no();
                if (($("#ReceiptDateFrom:visible").length) + ($("#ReceiptDateTo:visible").length)) {
                    if ($("#ReceiptDateFrom").val().trim() != "" && $("#ReceiptDateTo").val().trim() != "") {
                        filters += "Receipt Date: " + $("#ReceiptDateFrom").val() + " - " + $("#ReceiptDateTo").val() + ", ";
                    }
                    else {
                        if ($("#ReceiptDateFrom").val().trim() != "") {
                            filters += "Receipt Date: " + $("#ReceiptDateFrom").val() + ", ";
                        }
                        else {
                            filters += "Receipt Date: " + $("#ReceiptDateTo").val() + ", ";
                        }
                    }
                }
                if ($("#ItemCategoryID:visible").length) {
                    if (clean($('#ItemCategoryID').val()) != 0) {
                        filters += "Item Category: " + $("#ItemCategoryID option:selected").text() + ', ';
                    }
                }
                if ($("#SalesCategoryID:visible").length) {
                    if (clean($('#SalesCategoryID').val()) != 0) {
                        filters += "Sales Category: " + $("#SalesCategoryID option:selected").text() + ', ';
                    }
                }
                if ($("#CustomerCategoryID:visible").length) {
                    if (clean($('#CustomerCategoryID').val()) != 0) {
                        filters += "Customer Category: " + $("#CustomerCategoryID option:selected").text() + ', ';
                    }
                }
                if ($("#BatchTypeID:visible").length) {
                    if (clean($('#BatchTypeID').val()) != 0) {
                        filters += "Batch Type: " + $("#BatchTypeID option:selected").text() + ', ';
                    }
                }
                if ($("#ItemLocationID:visible").length) {
                    if (clean($('#ItemLocationID').val()) != 0) {
                        filters += "Location: " + $("#ItemLocationID option:selected").text() + ', ';
                    }
                }
                if ($("#FSOName").val() != "") {
                    filters += "FSO: " + $("#FSOName").val() + ", ";
                }
                if ($("#SalesIncentiveCategoryID:visible").length) {
                    if (clean($('#SalesIncentiveCategoryID').val()) != 0) {
                        filters += "Sales Incentive Category: " + $("#SalesIncentiveCategoryID option:selected").text() + ', ';
                    }
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "CashBankLedger":
                data += "&Filters=";
                data += "Bank Or Cash Ledger ";
                data += " From " + $("#FromDate").val();
                data += " to " + $("#ToDate").val();
                break;
            case "ShortDelivery":
                if ($("#Locations").val() != "") {
                    filters += "Location: " + $("#Locations Option:selected").text() + ", ";
                }
                if ($("#SalesCategoryID").val() != "") {
                    filters += "Sales Category: " + $("#SalesCategoryID Option:selected").text() + ", ";
                }
                if ($("#ItemID").val() != "") {
                    filters += "Item: " + $("#ItemName").val();
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "CounterSales":
                if ($("#DoctorID").val() != "") {
                    filters += "Doctor: " + $("#DoctorID Option:selected").text();
                }
                if ($("#Patient").val() != "") {
                    filters += "Patient: " + $("#Patient").val();
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;
            case "CustomerSubLedger":
                if ($("#Customer").val() != "") {
                    filters += "Customer: " + $("#Customer").val()+", ";
                }
                if ($("#ItemLocationID").val() != "") {
                    filters += "Location: " + $("#ItemLocationID Option:selected").text();
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "MISReports":
                filters += self.get_customer_details();
                if ($("#ItemLocationID:visible").length) {
                    if ($("#ItemLocationID").val() != 0) {
                        filters += "Location: " + $("#ItemLocationID Option:Selected").text() + ", ";
                    }
                }
                if ($("#AgeingBucket:visible").length) {
                    if ($("#AgeingBucket:visible").val() != 0) {
                        filters += "Ageing Bucket: " + $("#AgeingBucket").val();
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

    get_customer_details: function () {
        var self = sales;
        var filters = "";
        if ($("#CustomerID").val() == "") {
            if ($("#FromCustomerRange").val() + $("#ToCustomerRange").val().trim() != "") {
                if ($("#FromCustomerRange").val().trim() != "" && $("#ToCustomerRange").val().trim() != "") {
                    filters += "Customers from: " + $("#FromCustomerRange").val() + " - " + $("#ToCustomerRange").val() + ", ";
                } else {
                    if ($("#FromCustomerRange").val().trim() != "") {
                        filters += "Customer Range From: " + $("#FromCustomerRange").val() + ", ";
                    }
                    else {
                        filters += "Customer Range To: " + $("#ToCustomerRange").val() + ", ";
                    }
                }
            }
            else if ($("#CustomerCodeFrom").val().trim() + $("#CustomerCodeTo").val() != "") {
                if ($("#CustomerCodeFrom").val().trim() != "" && + $("#CustomerCodeTo").val().trim() !=""){
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
    get_invoice_no: function () {
        var self = sales;
        var filters = "";
        if (($("#InvoiceNOFrom").val() + $("#InvoiceNOTo").val()).trim() != "") {
            if ($("#InvoiceNOFrom").val().trim() != "" && $("#InvoiceNOTo").val().trim() != "") {
                filters += "Invoice No: " + $("#InvoiceNOFrom").val() + " - " + $("#InvoiceNOTo").val() + ", ";
            }
            else {
                if ($("#InvoiceNOFrom").val().trim() != ""){
                    filters += "Invoice No From: " + $("#InvoiceNOFrom").val() + ", ";
                }
                else{
                    filters += "Invoice No To: " + $("#InvoiceNOTo").val() + ", ";
                }
            }
        }
        return filters;
    },

    get_receipt_no: function () {
        var self = sales;
        var filters = "";
        if (($("#ReceiptNoFrom").val() + $("#ReceiptNoTo").val()).trim() != "") {
            if ($("#ReceiptNoFrom").val().trim() != "" && $("#ReceiptNoTo").val().trim() != "") {
                filters += "Receipt No: " + $("#ReceiptNoFrom").val() + " - " + $("#ReceiptNoTo").val() + ", ";
            }
            else {
                if ($("#ReceiptNoFrom").val().trim() != "") {
                    filters += "Receipt No From: " + $("#ReceiptNoFrom").val() + ", ";
                }
                else{
                    filters += "Receipt No To: " + $("#ReceiptNoTo").val() + ", ";
                }
            }
        }
        return filters;
    },
    
    get_item_details: function () {
        var self = sales;
        var filters = "";
        if ($("#ItemID").val() == "")
        {
            if ($("#ItemFromRange").val() + $("#ItemToRange").val().trim() != "") {
                if ($("#ItemFromRange").val().trim() != "" && $("#ItemToRange").val().trim() != "") {
                    filters += "Item From: " + $("#ItemFromRange").val() + " - " + $("#ItemToRange").val() + ", ";
                } else {
                    if ($("#ItemFromRange").val().trim() != "") {
                        filters += "Item Range From: " + $("#ItemFromRange").val() + ", ";
                    }
                    else {
                        filters += "Item Range To: " + $("#ItemToRange").val() + ", ";
                    }
                }
            }
            else if ($("#ItemCodeFrom").val().trim() + $("#ItemCodeTo").val() != "") {
                if ($("#ItemCodeFrom").val().trim() != "" && + $("#ItemCodeTo").val().trim() !=""){
                    filters += "Item Code:" + $("#ItemCodeFrom").val() + " - " + $("#ItemCodeTo").val() + ", ";
                } else {
                    if ($("#ItemCodeFrom").val().trim() != "") {
                        filters += "Item Code From: " + $("#ItemCodeFrom").val() + ", ";
                    }
                    else {
                        filters += "Item Code To: " + $("#ItemCodeTo").val() + ", ";
                    }
                }
            }
        }
        else {
            filters += "Item: " + $("#ItemName").val() +", ";
        }
        return filters;
    },

    get_fsoname: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#FSOName').val(),
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

    set_fsoname: function (event, item) {
        var self = sales;
        console.log(item)
        $("#FSOID").val(item.id);
        $("#FSOName").val(item.name);
        //$("#FSOCode").val(item.Code);
    },

    get_patient_No: function (release) {
        var self = sales;
        $.ajax({
            url: '/Masters/InternationalPatient/GetPatientAutoComplete',
            data: {
                Hint: $('#Patient').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },

    set_patient_No: function (event, item) {
        var self = sales;
        $("#PatientID").val(item.id);
        $("#Patient").val(item.name);
    },

    get_invoiceno_from: function (release) {
        Table = 'SalesInvoiceNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#InvoiceNOFrom').val(),
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
        $("#InvoiceNOFrom").val(item.code);
        $("#InvoiceNOFromID").val(item.id);
    },
    get_invoiceno_To: function (release) {

        Table = 'SalesInvoiceNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#InvoiceNOTo').val(),
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
        $("#InvoiceNOTo").val(item.code);
        $("#InvoiceNOToID").val(item.id);
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
        self = sales;
        $("#CustomerCodeFrom").val(item.code);
        $("#CustomerCodeFromID").val(item.id);
    },
    get_customercode: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCode').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customercode: function (event, item) {
        self = sales;
        $("#CustomerCode").val(item.code);
        $("#CustomerCodeID").val(item.id);
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
        self = sales;
        $("#CustomerCodeTo").val(item.code);
        $("#CustomerCodeToID").val(item.id);
    },
    get_cheque_No: function (release) {
        var Table = "ChequeInstrumentNo";

        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ChequeNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_cheque_No: function (event, item) {
        $("#ChequeNo").val(item.code);
        $("#ID").val(item.id);

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

    get_to_categoryrange() {
        var self = sales;
        var from_range = $("#FromCategoryRange").val();
        $.ajax({
            url: '/Reports/Sales/GetItemCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToCategoryRange").append(html);
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

    get_itemcode_from: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_itemcode_from: function (event, item) {
        self = sales;
        $("#ItemCodeFrom").val(item.code);
        $("#ItemCodeFromID").val(item.id);
        $("#ItemName").val(item.code);
        $("#ItemNameID").val(item.id);
    },

    get_itemcode_To: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_itemcode_To: function (event, item) {
        self = sales;
        $("#ItemCodeTo").val(item.code);
        $("#ItemCodeToID").val(item.id);
        //$("#ItemName").val(item.code);
        //$("#ItemNameID").val(item.id);
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

    refresh: function (event, item) {
        self = sales;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#FromDate").val(findate);
        $("#ToDate").val(currentdate);
        $("#ReceiptDateFrom").val(findate);
        $("#ReceiptDateTo").val(currentdate);
        $("#InvoiceNOFrom").val('');
        $("#InvoiceNOFromID").val('');
        $("#InvoiceNOTo").val('');
        $("#InvoiceNOToID").val('');
        $("#CustomerCodeFrom").val('');
        $("#CustomerCodeFromID").val('');
        $("#CustomerCodeTo").val('');
        $("#CustomerCodeToID").val('');
        $("#FromCustomerRange").val('');
        $("#ToCustomerRange").val('');
        $("#Customer").val('');
        $("#CustomerID").val('');
        $("#LocationFromID").val('');
        $("#LocationFrom").val('');
        $("#LocationToID").val('');
        $("#LocationTo").val('');
        $("#FromCategoryRange").val('');
        $("#ToCategoryRange").val('');
        $("#ItemCategoryID").val('');
        $("#ItemCodeFromID").val('');
        $("#ItemCodeToID").val('');
        $("#ItemCodeFrom").val('');
        $("#ItemCodeTo").val('');
        $("#ItemFromRange").val('');
        $("#ItemToRange").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#SalesOrderNoFrom").val('');
        $("#SalesOrderNoFromID").val('');
        $("#SalesOrderNoTo").val('');
        $("#SalesOrderNoToID").val('');
        $("#CustomerCode").val('');
        $("#ReceiptNoFrom").val('');
        $("#ReceiptNoFromID").val('');
        $("#ReceiptNoTo").val('');
        $("#ReceiptNoToID").val('');
        $("#ChequeNo").val('');
        $("#BankAccountNo").val('');
        $("#DoctorID").val('');
        $("#SalesCategoryID").val('');
        $("#CustomerCategoryID").val('');
        $("#LocationID").val(locationID)
        $("#Locations").val(locationID);
        $("#ItemLocationID").val(locationID);
        $("#BatchTypeID").val('');
        $("#SalesIncentiveCategoryID").val('');
        $("#FSOName").val('');
        $("#FSOID").val('');
        $('#Patient').val('');
        $('#PatientID').val('');
        $('#AgeingBucket').val('');
        $('#AgeingBucketID').val('');
    },

    get_salesorderno_from: function (release) {

        Table = 'SalesOrderNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#SalesOrderNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_salesorderno_from: function (event, item) {
        self = sales;
        $("#SalesOrderNoFrom").val(item.code);
        $("#SalesOrderNoFromID").val(item.id);
    },
    get_salesorderno_To: function (release) {

        Table = 'SalesOrderNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#SalesOrderNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_salesorderno_To: function (event, item) {
        self = sales;
        $("#SalesOrderNoTo").val(item.code);
        $("#SalesOrderNoToID").val(item.id);
    },
    get_item_code_from: function (release) {
        self = sales;
        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_code_from: function (event, item) {
        self = sales;
        $("#ItemCodeFrom").val(item.code);
        $("#ItemCodeFromID").val(item.id);

    },
    get_item_code_to: function (release) {
        self = sales;
        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_code_to: function (event, item) {
        self = sales;
        $("#ItemCodeTo").val(item.code);
        $("#ItemCodeToID").val(item.id);

    },
    get_receiptno_from: function (release) {
        self.sales;
        Table = 'ReceiptVoucher';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_receiptno_from: function (event, item) {
        $("#ReceiptNoFrom").val(item.code);
        $("#ReceiptNoFromID").val(item.id);
    },
    get_receiptno_to: function (release) {
        self.sales;
        Table = 'ReceiptVoucher';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_receiptno_to: function (event, item) {
        $("#ReceiptNoTo").val(item.code);
        $("#ReceiptNoToID").val(item.id);
    },
    get_account_No: function (release) {
        self.sales;
        $.ajax({
            url: '/Master/Treasury/GetTreasuryDetailsForAutoComplete',
            data: {
                Term: $('#BankAccountNo').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_account_No: function (event, item) {
        self.sales;
        $("#BankAccountNo").val(item.codeAccountId);
        $("#AccountName").val(item.BankName);
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

    get_ageing_bucket: function (release) {
        self.sales;
        Table = 'AgeingBucket';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#AgeingBucket').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_ageing_bucket: function (event, item) {
        var self = sales;
        $("#AgeingBucket").val(item.code);
        $("#AgeingBucketID").val(item.id);
    },

    validate_submit: function()
    {
        var self = sales;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    
    rules: {
        on_submit: [
        {
            elements: "#AgeingBucket:visible",
          rules: [
                  { type: form.required, message: "Please enter Ageing Bucket" },
                ]
        }
        ]
      }
};