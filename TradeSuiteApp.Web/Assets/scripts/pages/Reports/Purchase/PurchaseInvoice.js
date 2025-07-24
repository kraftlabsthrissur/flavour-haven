
$(function () {
    PurchaseInvoice.init();
});

PurchaseInvoice = {
    init: function () {
        var self = PurchaseInvoice;
        self.bind_events();
        ReportHelper.init();
        //self.get_report_type();
    },
    bind_events: function () {
        var self = PurchaseInvoice;
        $.UIkit.autocomplete($('#invoice-invoiceno-autocomplete'), { 'source': self.get_invoiceno, 'minLength': 1 });
        $('#invoice-invoiceno-autocomplete').on('selectitem.uk.autocomplete', self.set_invoiceno);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#SupplierName-autocomplete').on('selectitem.uk.autocomplete', self.set_suppliers);
        $("#IsIncludeOverruled").on("ifChanged", self.get_with_overruled);
        $('.invoice_type').on('ifChanged', self.invoice_refresh);////////////////////////////////
        $('.invoice_Summary').on('ifChanged', self.invoice_show_report_item_type);
        $('.invoice_report_type').on('ifChanged', self.invoice_show_report_type);//////////////////
        $('#Refresh').on('click', self.refresh);
    },

    get_invoiceno: function (release) {
        var self = PurchaseInvoice;
        var type = $(".invoice_type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseInvoice';
        }
        else if (type == "Service") {
            Table = 'PurchaseInvoiceForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

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
    set_invoiceno: function (event, item) {
        var self = PurchaseInvoice;
        $("#InvoiceNOFrom").val(item.code);
        $("#InvoiceNOFromID").val(item.id);
        var report_type = $(".invoice_Summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            $("#InvoiceNOTo").val(item.code);
            $("#InvoiceNOToID").val(item.id);
        }
        else {
            $("#InvoiceNOTo").val('');
            $("#InvoiceNOToID").val('');
        }
    },

    get_items: function (release) {
        var self = PurchaseInvoice;
        var area;
        //var type = $("input[name='Type']:checked").val(); 
        area = $("#ItemAutoType").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#ItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item: function (event, item) {
        var self = PurchaseInvoice;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },

    get_suppliers: function (release) {
        var self = PurchaseInvoice;
        var type = $("#ItemAutoType").val();
        var ReportType = $(".mis_report_type:checked").val();
        var url = '/Masters/Supplier/GetAllSupplierAutoComplete';
        if ($(".item-type:visible").val()) {
            if (type == "Stock") {
                url = '/Masters/Supplier/getSupplierForAutoComplete';
            }
            else if (type == "Service") {
                url = '/Masters/Supplier/getServiceSupplierForAutoComplete';

            }
        }
        else {
            if ((ReportType == "supplier-ageing") || (ReportType == "supplier-ledger")) {
                url = '/Masters/Supplier/GetAllSupplierAutoComplete';
            }
        }
        //var url = '/Masters/Supplier/getSupplierForAutoComplete';
        //if ($(".order-type:checked").val() == 'Service') {
        //    url = '/Masters/Supplier/getServiceSupplierForAutoComplete';
        //}
        $.ajax({
            url: url,
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
    set_suppliers: function (event, item) {   // on select auto complete item
        var self = PurchaseInvoice;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },

    get_with_overruled: function () {
        var self = PurchaseInvoice;
        if ($("#IsIncludeOverruled").is(':checked')) {
            $("#IsOverruled").val(true);
        } else {
            $("#IsOverruled").val(false);
        }
    },

    invoice_refresh: function () {
        var self = PurchaseInvoice;
        var item_type = $(".invoice_Summary:checked").val();
        var type = $(".invoice_type:checked").val();
        $("#ItemAutoType").val(type);
        if (type == "Stock") {
            //$("#Mode").removeClass("uk-hidden");
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".without-item-wise").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").hide();
                $(".detail").show();
                $(".service-summary").hide();
                $(".service-srn").addClass("uk-hidden");
                $(".invoice").text('Invoice No');
                $("#Mode").removeClass("uk-hidden");

                var mode = $(".invoice_report_type:checked").val();
                $("." + mode).show();
                if (mode == "without-item-wise") {
                    $(".status").removeClass("uk-hidden");
                    $(".item-wise").hide();
                    $(".without-item-wise").hide();
                    $(".summary").hide();
                    $(".without-item-wise").show();
                }
                else {
                    $(".status").addClass("uk-hidden");
                    $(".without-item-wise").hide();
                    $(".item-wise").removeClass("uk-hidden");
                    $(".summary").hide();
                    $(".item-wise").show();
                }
            }
            else {
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                //$(".detail").hide();
                $("#Mode").addClass("uk-hidden");
                $(".summary").show();
                $(".service-summary").hide();
                $(".service-srn").addClass("uk-hidden");
                $(".invoice").text('Invoice No');
            }
            self.refresh();
        }
        else if (type == "Service") {
            if (item_type == "Detail") {
                $("#Mode").addClass("uk-hidden");
                $(".summary").hide();
                $(".without-item-wise").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").hide();
                $(".detail").show();
                $(".service-summary").hide();
                $(".service-srn").addClass("uk-hidden");
                $(".invoice").text('Invoice No');
            }
            else {
                $("#Mode").removeClass("uk-hidden");
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".detail").hide();
                $(".summary").hide();
                $(".service-summary").hide();
                $(".service-summary").show();
                $(".service-srn").removeClass("uk-hidden");
                $(".invoice").text('Invoice No');
                $("." + mode).show();
                if (mode == "without-item-wise") {
                    $(".status").removeClass("uk-hidden");
                    $(".item-wise").hide();
                    $(".without-item-wise").hide();
                    $(".summary").hide();
                    $(".without-item-wise").show();
                }
                else {
                    $(".status").addClass("uk-hidden");
                    $(".without-item-wise").hide();
                    $(".item-wise").hide();
                    $(".summary").hide();
                    $(".detail").hide();
                    $(".item-wise").show();
                }
            }
            self.refresh();
        }
       // self.get_invoice_item_category(type);
    },

    invoice_show_report_item_type: function () {
        self = PurchaseInvoice;
        var item_type = $(".invoice_Summary:checked").val();
        var type = $(".invoice_type:checked").val();
        $("#ItemAutoType").val(type);
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".without-item-wise").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").hide();
                $(".detail").show();
                $(".service-summary").hide();
                $(".invoice").text('Invoice No');
                $("#Mode").removeClass("uk-hidden");
                var mode = $(".invoice_report_type:checked").val();
                $("." + mode).show();
                if (mode == "without-item-wise") {
                    $(".status").removeClass("uk-hidden");
                    $(".item-wise").hide();
                    $(".without-item-wise").hide();
                    $(".summary").hide();
                    $(".without-item-wise").show();
                }
                else {
                    $(".status").addClass("uk-hidden");
                    $(".without-item-wise").hide();
                    $(".item-wise").removeClass("uk-hidden");
                    $(".summary").hide();
                    //$(".detail").hide();
                    $(".item-wise").show();
                }
            }
            else {
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".detail").hide();
                $("#Mode").addClass("uk-hidden");
                $(".summary").show();
                $(".service-summary").hide();
                $(".invoice").text('Invoice No');
                
            }
            self.refresh();
        }
        else if (type == "Service") {
            if (item_type == "Detail") {
                $("#Mode").addClass("uk-hidden");
                $(".summary").hide();
                $(".without-item-wise").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").hide();
                $(".detail").show();
                $(".service-summary").hide();
                $(".service-srn").addClass("uk-hidden");
                $(".invoice").text('Invoice No');
            }
            else {
                $("#Mode").removeClass("uk-hidden");
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".detail").hide();
                $(".summary").hide();
                $(".service-summary").show();
                $(".invoice").text('Invoice No From');
                $(".service-srn").removeClass("uk-hidden");
                $("." + mode).show();
                if (mode == "without-item-wise") {
                    $(".status").removeClass("uk-hidden");
                    $(".item-wise").hide();
                    $(".without-item-wise").hide();
                    $(".summary").hide();
                    $(".without-item-wise").show();
                }
                else {
                    $(".status").addClass("uk-hidden");
                    $(".without-item-wise").hide();
                    $(".item-wise").hide();
                    $(".summary").hide();
                    $(".detail").hide();
                    $(".item-wise").show();
                }
            }
            self.refresh();
        }
    },

    invoice_show_report_type: function () {
        self = PurchaseInvoice;
        var report_type = $(".invoice_report_type:checked").val();
        var type = $(".invoice_type:checked").val();
        $("#ItemAutoType").val(type);
        if (type == "Stock") {
            if (report_type == "item-wise") {
                $(".without-item-wise").hide();
                $(".item-wise").removeClass("uk-hidden");
                $(".summary").hide();
                //$(".detail").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").show();
            }
            else if (report_type == "without-item-wise") {
                $(".item-wise").hide();
                $(".without-item-wise").hide();
                $(".summary").hide();
                $(".without-item-wise").show();
                $(".status").removeClass("uk-hidden");
            }
            //self.get_invoice_item_category(type);
            self.refresh();
        }
        else {
            if (report_type == "item-wise") {
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".summary").hide();
                $(".detail").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").show();
            }
            else if (report_type == "without-item-wise") {
                $(".item-wise").hide();
                $(".without-item-wise").hide();
                $(".summary").hide();
                $(".without-item-wise").show();
                $(".status").removeClass("uk-hidden");
            }
            //self.get_invoice_item_category(type);
            self.refresh();
        }
    },

    refresh: function (event, item) {
        self = PurchaseInvoice;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        //$("#ToDate").val(currentdate);
        $("#PRDateTo").val(currentdate);
        $("#PODateTo").val(currentdate);
        $("#ToInvoiceDate").val(currentdate);
        $("#ToTransDate").val(currentdate);
        //$("#FromDate").val(findate);
        $("#PODateFrom").val(findate);
        $("#PRDateFrom").val(findate);
        $("#FromInvoiceDate").val(findate);
        $("#FromTransDate").val(findate);
        $("#POPRdateFrom").val('');
        $("#POPRdateTo").val('');
        $("#POPRdateToSupplierInvoiceNO").val('');
        $("#PRNOFrom").val('');
        $("#PRID").val('');
        $("#PRNOFromID").val('');
        $("#PRNOTo").val('');
        $("#PRID").val('');
        $("#PRNOToID").val('');
        $("#PONOFrom").val('');
        $("#PRID").val('');
        $("#PONOFromID").val('');
        $("#PONOTo").val('');
        $("#PRID").val('');
        $("#PONOToID").val('');
        $("#ItemID").val('');
        $("#SupplierID").val('');
        $("#ItemName").val('');
        $("#SupplierName").val('');
        $("#FromItemNameRange").val('');
        $("#ToItemNameRange").val('');
        $("#FromSupplierRange").val('');
        $("#ToSupplierRange").val('');
        $("#InvoiceNOFrom").val('');
        $("#InvoiceNOFromID").val('');
        $("#InvoiceNOTo").val('');
        $("#InvoiceNOToID").val('');
        $("#SupplierInvoiceNO").val('');
        $("#SupplierInvoiceNOTO").val('');
        $("#SupplierInvoiceNOID").val('');
        $("#SRNFrom").val('');
        $("#SRNNOFromID").val('');
        $("#SRNTo").val('');
        $("#SRNNOToID").val('');
        $("#FromItemCategoryRange").val('');
        $("#ToItemCategoryRange").val('');
        $("#FromCategoryRange").val('');
        $("#ToCategoryRange").val('');
        $("#ItemCategory").val('');
        $("#ItemCategoryID").val('');
        $("#Status").val('');
        $("#QCNOFrom").val('');
        $("#QCNoFrom").val('');
        $("#QCNoFromID").val('');
        $("#QCNOFromID").val('');
        $("#QCNOTo").val('');
        $("#QCNOToID").val('');
        $("#QCNoToID").val('');
        $("#QCNoFromID").val('');
        $("#GRNNOFrom").val('');
        $("#GRNNOFromID").val('');
        $("#GRNNOTo").val('');
        $("#GRNNOToID").val('');
        $("#FromUserRange").val('');
        $("#ToUserRange").val('');
        $("#Userslist").val('');
        $("#ItemCategory").val('');
        $("#Department").val('');
        $("#FromDocumentRange").val('');
        $("#ToDocumentRange").val('');
        $("#Status").val('');
        $("#OutstandingDays").val('');
        $("#SupplierID").val('');
        $("#SupplierInvoiceNOID").val('');
        $("#ItemCategory").val('');
        $("#ToDepartmentFromRange").val('');
        $("#ToDepartmentToRange").val('');
        $("#FromCategoryRange").val('');
        $("#ToCategoryRange").val('');
        $("#FromItemRange").val('');
        $("#ToItemRange").val('');
        $("#AccountName").val('');
        $("#AccountCodeFrom").val('');
        $("#AccountCodeTo").val('');
        $("#Users").val('');
        $("#KeyValue").val('');
        $("#TransType").val('');
        $("#ItemAccountCategory").val('');
        $("#AccountNameFromRange").val('');
        $("#AccountNameToRange").val('');
        $("#FromTransTypeRange").val('');
        $("#ToTransTypeRange").val('');
        $("#FromItemAccountsCategory").val('');
        $("#ToItemAccountsCategory").val('');
        $("#UserID").val('');
        $("#PODateFrom").val('');
        $("#PODateTo").val('');
        $("#GRNFromDate").val('');
        $("#GRNToDate").val('');
        $("#AccountNameID").val('');
        $('#AgeingBucket').val('');
        $('#AgeingBucketID').val('');
    },

    get_filters: function () {
        var self = PurchaseInvoice;
        var filters = "";
        var data = "";
        if ($("#ItemLocationID").val() != "") {
            filters += "Location: " + $("#ItemLocationID Option:selected").text() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return data += filters;
    },

}