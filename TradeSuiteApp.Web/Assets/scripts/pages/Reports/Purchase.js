$(function () {
    var self = purchase;
    purchase.bind_events();
    // Item.Purchase_report_item_list();
    $('#item-list').SelectTable({
        selectFunction: self.select_item,
        //  returnFocus: "#txtAdditionalIssueQty",
        modal: "#select-item",
        initiatingElement: "#ItemName",
        selectionType: "radio"
    });
    $("input:radio[name=Type]:first").prop("checked", true)
    $("input:radio[name=Type]:first").closest('div').addClass("checked")
});


var is_first_run = true;
purchase = {
    Requisitioninit: function () {
        var self = purchase
        self.show_purchase_requsition_report_type();
    },
    Invoiceinit: function () {
        var self = purchase
        self.invoice_refresh();
    },
    Orderinit: function () {
        var self = purchase
        self.show_purchase_order_type();
    },
    GRNinit: function () {
        var self = purchase
        self.show_grn_report_type();
    },

    bind_events: function () {
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': purchase.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', purchase.set_item);        $
        $("#ItemCategoryID").on("change", purchase.change_category);
        $("#report-filter-submit").on("click", purchase.get_report_view);
        $('.invoice_type').on('ifChanged', purchase.invoice_refresh);////////////////////////////////
        $("#report-filter-form").on("submit", function () { return false; });
        $('.order-type').on('ifChanged', purchase.show_purchase_order_type);
        $('.report_type').on('ifChanged', purchase.show_purchase_order_report_type);
        $('.requisition-type').on('ifChanged', purchase.show_purchase_requsition_report_type);
        $('.invoice_report_type').on('ifChanged', purchase.invoice_show_report_type);//////////////////
        $('.invoice_Summary').on('ifChanged', purchase.invoice_show_report_item_type);/////////////////
        $('.requisition-summary').on('ifChanged', purchase.requisition_show_report_item_type);
        $('.summary').on('ifChanged', purchase.show_report_item_type);
        $('.grn_type').on('ifChanged', purchase.show_grn_report_type);
        $('.grn-summary').on('ifChanged', purchase.show_grn_report_item_type);
        $('.qc-summary').on('ifChanged', purchase.show_qc_report_item_type);
        $('.qc-type').on('ifChanged', purchase.show_qc_itemType);
        $('.recipttype').on('ifChanged', purchase.change_type);
        $('.mis_report_type').on('ifChanged', purchase.show_mis_report_type);
        $('.item-type').on('ifChanged', purchase.change_mis_item_type);
        //$('.item-type').on("change", purchase.clear_invoice_transno);
        $("#btnOKItem").on("click", purchase.select_item);

        $('.pur-return-type').off('ifChanged').on('ifChanged', purchase.show_pur_report_type);
        $.UIkit.autocomplete($('#PurchaseReturnNOFrom-autocomplete'), { 'source': purchase.get_purchase_return_no, 'minLength': 1 });
        $('#PurchaseReturnNOFrom-autocomplete').on('selectitem.uk.autocomplete', purchase.get_purchase_return_no);
        //$.UIkit.autocomplete($('#PurchaseReturnNOFrom-autocomplete'), { 'source': purchase.get_purchase_return_no, 'minLength': 1 });
        //$('#PurchaseReturnNOFrom-autocomplete').on('selectitem.uk.autocomplete', purchase.set_purchase_return_no);

        $.UIkit.autocomplete($('#PurchaseReturnNoTo-autocomplete'), { 'source': purchase.get_purchase_return_no_to, 'minLength': 1 });
        $('#PurchaseReturnNoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.get_purchase_return_no_to);
        $.UIkit.autocomplete($('#PurchaseReturnNoTo-autocomplete'), { 'source': purchase.get_purchase_return_no_to, 'minLength': 1 });
        $('#PurchaseReturnNoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_purchase_return_no_to);
        $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': purchase.get_suppliers, 'minLength': 1 });
        $('#SupplierName-autocomplete').on('selectitem.uk.autocomplete', purchase.set_suppliers);

        //$.UIkit.autocomplete($('#itemName-autocomplete'), { 'source': purchase.get_itemName, 'minLength': 1 });
        //$('#itemName-autocomplete-autocomplete').on('selectitem.uk.autocomplete', purchase.set_itemName);

        $.UIkit.autocomplete($('#PurcahseReturnGrnNoFrom-autocomplete'), { 'source': purchase.get_grnno, 'minLength': 1 });
        $('#PurcahseReturnGrnNoFrom-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grnno);
        $.UIkit.autocomplete($('#PurcahseReturnGrnNoTo-autocomplete'), { 'source': purchase.get_grnnoTo, 'minLength': 1 });
        $('#PurcahseReturnGrnNoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grnnoTo);


        $.UIkit.autocomplete($('#prno-autocomplete'), { 'source': purchase.get_prno, 'minLength': 1 });
        $('#prno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_prno);
        $('#Refresh').on('click', purchase.refresh);
        $.UIkit.autocomplete($('#prnoTo-autocomplete'), { 'source': purchase.get_prnoTo, 'minLength': 1 });
        $('#prnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_prnoTo);
        $.UIkit.autocomplete($('#requisition-prno-autocomplete'), { 'source': purchase.get_requisition_prno, 'minLength': 1 });
        $('#requisition-prno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_requisition_prno);
        $.UIkit.autocomplete($('#requisition-prnoTo-autocomplete'), { 'source': purchase.get_requisition_prnoTo, 'minLength': 1 });
        $('#requisition-prnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_requisition_prnoTo);
        $.UIkit.autocomplete($('#pono-autocomplete'), { 'source': purchase.get_pono, 'minLength': 1 });
        $('#pono-autocomplete').on('selectitem.uk.autocomplete', purchase.set_pono);
        $.UIkit.autocomplete($('#ponoTo-autocomplete'), { 'source': purchase.get_ponoTo, 'minLength': 1 });
        $('#ponoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_ponoTo);
        $.UIkit.autocomplete($('#grn-pono-autocomplete'), { 'source': purchase.get_grn_pono, 'minLength': 1 });
        $('#grn-pono-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grn_pono);
        $.UIkit.autocomplete($('#grn-ponoTo-autocomplete'), { 'source': purchase.get_grn_ponoTo, 'minLength': 1 });
        $('#grn-ponoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grn_ponoTo);
        $.UIkit.autocomplete($('#invoice-pono-autocomplete'), { 'source': purchase.get_invoice_pono, 'minLength': 1 });
        $('#invoice-pono-autocomplete').on('selectitem.uk.autocomplete', purchase.set_invoice_pono);
        $.UIkit.autocomplete($('#invoice-ponoTo-autocomplete'), { 'source': purchase.get_invoice_ponoTo, 'minLength': 1 });
        $('#invoice-ponoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_invoice_ponoTo);
        $.UIkit.autocomplete($('#qc-pono-autocomplete'), { 'source': purchase.get_qc_pono, 'minLength': 1 });
        $('#qc-pono-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_pono);
        $.UIkit.autocomplete($('#qc-ponoTo-autocomplete'), { 'source': purchase.get_qc_ponoTo, 'minLength': 1 });
        $('#qc-ponoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_ponoTo);
        $.UIkit.autocomplete($('#invoice-qcno-autocomplete'), { 'source': purchase.get_qcno, 'minLength': 1 });
        $('#invoice-qcno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qcno);
        $.UIkit.autocomplete($('#invoice-qcnoTo-autocomplete'), { 'source': purchase.get_qcnoTo, 'minLength': 1 });
        $('#invoice-qcnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qcnoTo);
        $.UIkit.autocomplete($('#qc-qcno-autocomplete'), { 'source': purchase.get_qc_qcno, 'minLength': 1 });
        $('#qc-qcno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_qcno);
        $.UIkit.autocomplete($('#qc-qcnoTo-autocomplete'), { 'source': purchase.get_qc_qcnoTo, 'minLength': 1 });
        $('#qc-qcnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_qcnoTo);
        $.UIkit.autocomplete($('#invoice-grnno-autocomplete'), { 'source': purchase.get_grnno, 'minLength': 1 });
        $('#invoice-grnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grnno);
        $.UIkit.autocomplete($('#invoice-grnnoTo-autocomplete'), { 'source': purchase.get_grnnoTo, 'minLength': 1 });
        $('#invoice-grnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grnnoTo);
        $.UIkit.autocomplete($('#qc-grnno-autocomplete'), { 'source': purchase.get_qc_grnno, 'minLength': 1 });
        $('#qc-grnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_grnno);
        $.UIkit.autocomplete($('#qc-grnnoTo-autocomplete'), { 'source': purchase.get_qc_grnnoTo, 'minLength': 1 });
        $('#qc-grnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_qc_grnnoTo);
        $('#invoice-grnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grnnoTo);
        $.UIkit.autocomplete($('#grn-grnno-autocomplete'), { 'source': purchase.get_grn_grnno, 'minLength': 1 });
        $('#grn-grnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grn_grnno);
        $.UIkit.autocomplete($('#grn-grnnoTo-autocomplete'), { 'source': purchase.get_grn_grnnoTo, 'minLength': 1 });
        $('#grn-grnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_grn_grnnoTo);
        $.UIkit.autocomplete($('#grn-srnno-autocomplete'), { 'source': purchase.get_srnno_grn, 'minLength': 1 });
        $('#grn-srnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_srnno_grn);
        $.UIkit.autocomplete($('#grn-srnnoTo-autocomplete'), { 'source': purchase.get_srnnoTo_grn, 'minLength': 1 });
        $('#grn-srnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_srnnoTo_grn);
        $.UIkit.autocomplete($('#invoice-invoiceno-autocomplete'), { 'source': purchase.get_invoiceno, 'minLength': 1 });
        $('#invoice-invoiceno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_invoiceno);
        $.UIkit.autocomplete($('#invoice-invoicenoTo-autocomplete'), { 'source': purchase.get_invoicenoTo, 'minLength': 1 });
        $('#invoice-invoicenoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_invoicenoTo);
        $.UIkit.autocomplete($('#invoice-srnno-autocomplete'), { 'source': purchase.get_srnno_invoice, 'minLength': 1 });
        $('#invoice-srnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_srnno_invoice);
        $.UIkit.autocomplete($('#invoice-srnnoTo-autocomplete'), { 'source': purchase.get_srnnoTo_invoice, 'minLength': 1 });
        $('#invoice-srnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_srnnoTo_invoice);
        $.UIkit.autocomplete($('#returnno-autocomplete'), { 'source': purchase.get_returnno, 'minLength': 1 });
        $('#returnno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_returnno);
        $.UIkit.autocomplete($('#returnnoTo-autocomplete'), { 'source': purchase.get_returnnoTo, 'minLength': 1 });
        $('#returnnoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_returnnoTo);
        $.UIkit.autocomplete($('#supplierinvoiceno-autocomplete'), { 'source': purchase.get_supplierinvoiceno, 'minLength': 1 });
        $('#supplierinvoiceno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_supplierinvoiceno);

        $.UIkit.autocomplete($('#mis-supplierinvoicenoto-autocomplete'), { 'source': purchase.get_mis_supplierinvoicenoto, 'minLength': 1 });
        $('#mis-supplierinvoicenoto-autocomplete').on('selectitem.uk.autocomplete', purchase.set_mis_supplierinvoicenoto);

        $.UIkit.autocomplete($('#mis-supplierinvoiceno-autocomplete'), { 'source': purchase.get_mis_supplierinvoiceno, 'minLength': 1 });
        $('#mis-supplierinvoiceno-autocomplete').on('selectitem.uk.autocomplete', purchase.set_mis_supplierinvoiceno);
        $.UIkit.autocomplete($('#mis-invoice-autocomplete'), { 'source': purchase.get_mis_invoiceno, 'minLength': 1 });
        $('#mis-invoice-autocomplete').on('selectitem.uk.autocomplete', purchase.set_mis_invoiceno);
        $.UIkit.autocomplete($('#mis-invoicenoTo-autocomplete'), { 'source': purchase.get_mis_invoicenoTo, 'minLength': 1 });
        $('#mis-invoicenoTo-autocomplete').on('selectitem.uk.autocomplete', purchase.set_mis_invoicenoTo);

        $.UIkit.autocomplete($('#milk-supplier-autocomplete'), { 'source': purchase.get_milk_supplier, 'minLength': 1 });
        $('#milk-supplier-autocomplete').on('selectitem.uk.autocomplete', purchase.set_milk_supplier);

        $("#FromSupplierRange").on("change", purchase.get_to_range);
        $("#FromItemNameRange").on("change", purchase.get_to_itemrange);
        $("#FromItemNameRangeMis").on("change", purchase.get_to_misitemrange);

        $("#FromTransTypeRange").on("change", purchase.get_to_transtyperange);
        $("#FromItemAccountsCategory").on("change", purchase.get_to_item_account_category_range);
        $("#AccountNameFromRange").on("change", purchase.get_to_account_name_range);
        $("#FromItemCategoryRange").on("change", purchase.get_to_itemcategoryrange);
        $("#FromUserRange").on("change", purchase.get_to_userrange);
        $("#FromDocumentRange").on("change", purchase.get_to_documentrange);
        $("#ToDepartmentFromRange").on("change", purchase.get_to_ToDepartmentrange);
        $("#FromItemCategoryRange").on("change", purchase.get_to_categoryrange);
        $(".balance-payable").on("ifChanged", purchase.set_status);
        $.UIkit.autocomplete($('#item-account-category-autocomplete'), { 'source': purchase.get_item_accounts_category, 'minLength': 1 });
        $('#item-account-category-autocomplete').on('selectitem.uk.autocomplete', purchase.set__item_accounts_category);
        $.UIkit.autocomplete($('#generalledger-transtype-autocomplete'), { 'source': purchase.get_transtype, 'minLength': 1 });
        $('#generalLedger-transtype-autocomplete').on('selectitem.uk.autocomplete', purchase.set_transtype);
        $.UIkit.autocomplete($('#keyvalue-autocomplete'), { 'source': purchase.get_keyvalue, 'minLength': 1 });
        $('#keyvalue-autocomplete').on('selectitem.uk.autocomplete', purchase.set_keyvalue);
        $.UIkit.autocomplete($('#accountname-autocomplete'), { 'source': purchase.get_accountname, 'minLength': 1 });
        $('#accountname-autocomplete').on('selectitem.uk.autocomplete', purchase.set_accountname);

        $.UIkit.autocomplete($('#accountcodefrom-autocomplete'), { 'source': purchase.get_accountcodefrom, 'minLength': 1 });
        $('#accountcodefrom-autocomplete').on('selectitem.uk.autocomplete', purchase.set_accountcodefrom);
        $.UIkit.autocomplete($('#accountcodeto-autocomplete'), { 'source': purchase.get_accountcodeto, 'minLength': 1 });
        $('#accountcodeto-autocomplete').on('selectitem.uk.autocomplete', purchase.set_accountcodeto);

        $.UIkit.autocomplete($('#ageing-bucket-autocomplete'), { 'source': purchase.get_ageing_bucket, 'minLength': 1 });
        $('#ageing-bucket-autocomplete').on('selectitem.uk.autocomplete', purchase.set_ageing_bucket);

        $("#IsIncludeOverruled").on("ifChanged", self.get_with_overruled);

         function init() {
            purchase.load_time_hide();
        }
    },
    load_time_hide: function () {
        var self = purchase;
        $(".uk-hidden").hide();
    },
    set_status: function () {
        var self = purchase;
        var balance_payable = $(this).val();
        if (balance_payable == "Yes") {
            $("#Status").val("Partial");
        }
        else {
            $("#Status").val("");
        }
    },

    show_mis_report_type: function () {
        var self = purchase;
        var report_type = $(this).val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        //self.refresh();
    },

    change_mis_item_type: function () {
        var self = purchase;
        var type = $(".item-type:checked").val();
        $("#ItemAutoType").val(type);
        purchase.get_category();
        self.refresh();
    },
    get_item_accounts_category: function (release) {

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#ItemAccountCategory').val(),
                Table: 'ItemAccountsCategory'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set__item_accounts_category: function (event, item) {
        self = purchase;
        $("#ItemAccountCategory").val(item.code);
        //$("#ID").val(item.id);
        $("#ItemAccountCategoryID").val(item.id);
    },

    get_transtype: function (release) {
        var Table;
        Table = 'TransactionType';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
        self = purchase;
        $("#TransType").val(item.code);
        $("#TransTypeID").val(item.id);
    },


    get_keyvalue: function (release) {
        var Table;
        Table = 'MisKeyValue';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#KeyValue').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_keyvalue: function (event, item) {
        self = purchase;
        $("#KeyValue").val(item.code);
        $("#KeyValueID").val(item.id);
    },

    invoice_show_report_type: function () {
        self = purchase;
        var report_type = $(".invoice_report_type:checked").val();
        var type = $(".invoice_type:checked").val();
        if (type == "Stock") {
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
            self.get_invoice_item_category(type);
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
            self.get_invoice_item_category(type);
            self.refresh();
        }
    },

    get_invoice_item_category: function (type) {
        var self = purchase;
        $.ajax({
            url: '/Reports/Purchase/GetCategory/',
            dataType: "json",
            type: "GET",
            data: {
                Type: type,
            },
            success: function (response) {
                $("#ItemCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.Value + "'>" + record.Text + "</option>";
                });
                $("#ItemCategoryID").append(html);
            }
        });
    },
    get_report_view: function (e) {
        e.preventDefault();
        self = purchase;
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

            case "PurchaseRequisition":
                if (($("#PRNOFrom").val() + $("#PRNOTo").val()).trim() != "") {
                    if ($("#PRNOFrom").val().trim() == "" || $("#PRNOTo").val().trim() == "") {
                        filters += "Request No: " + $("#PRNOFrom").val() + $("#PRNOTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#PRNOFrom").val() + " - " + $("#PRNOTo").val() + ", ";
                    }
                }

                filters += self.get_item_title();

                if ($("#Department").val() != "") {
                    filters += "To Department: " + $("#Department Option:selected").text() + ", ";
                }
                if ($("#ItemCategory").val() != 0) {
                    filters += "Item Category: " + $("#ItemCategory Option:selected").text() + ", ";
                }
               
                if ($("#Userslist").val() != 0) {
                    filters += "Employee: " + $("#Userslist Option:selected").text() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;

            case "PurchaseOrder":

                if (($("#PONOFrom").val() + $("#PONOTo").val()).trim() != "") {
                    if ($("#PONOFrom").val().trim() == "" || $("#PONOTo").val().trim() == "") {
                        filters += "Order No: " + $("#PONOFrom").val() + $("#PONOTo").val() + ", ";
                    } else {
                        filters += "Order No: " + $("#PONOFrom").val() + " - " + $("#PONOTo").val() + ", ";
                    }
                }

                if (($("#POPRdateFrom").val() + $("#POPRdateTo").val()).trim() != "") {
                    if ($("#POPRdateFrom").val().trim() == "" || $("#POPRdateTo").val().trim() == "") {
                        filters += "Request Date: " + $("#POPRdateFrom").val() + $("#POPRdateTo").val() + ", ";
                    } else {
                        filters += "Request Date: " + $("#POPRdateFrom").val() + " - " + $("#POPRdateTo").val() + ", ";
                    }
                }

                if (($("#PRNOFrom").val() + $("#PRNOTo").val()).trim() != "") {
                    if ($("#PRNOFrom").val().trim() == "" || $("#PRNOTo").val().trim() == "") {
                        filters += "Request No: " + $("#PRNOFrom").val() + $("#PRNOTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#PRNOFrom").val() + " - " + $("#PRNOTo").val() + ", ";
                    }
                }
                filters += self.get_supplier_title();

                filters += self.get_item_title();

                if ($("#UserID").val() != 0) {
                    filters += "Employee: " + $("#UserID Option:selected").text() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;

            case "GRN":

                if (($("#PODateFrom").val() + $("#PODateTo").val()).trim() != "") {
                    if ($("#PODateFrom").val().trim() == "" || $("#PODateTo").val().trim() == "") {
                        filters += "Order Date: " + $("#PODateFrom").val() + $("#PODateTo").val() + ", ";
                    } else {
                        filters += "Order Date: " + $("#PODateFrom").val() + " - " + $("#PODateTo").val() + ", ";
                    }
                }

                if (($("#GRNNOFrom").val() + $("#GRNNOTo").val()).trim() != "") {
                    if ($("#GRNNOFrom").val().trim() == "" || $("#GRNNOTo").val().trim() == "") {
                        filters += "Request No: " + $("#GRNNOFrom").val() + $("#GRNNOTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#GRNNOFrom").val() + " - " + $("#GRNNOTo").val() + ", ";
                    }
                }

                if (($("#PONOFrom").val() + $("#PONOTo").val()).trim() != "") {
                    if ($("#PONOFrom").val().trim() == "" || $("#PONOTo").val().trim() == "") {
                        filters += "Order No: " + $("#PONOFrom").val() + $("#PONOTo").val() + ", ";
                    } else {
                        filters += "Order No: " + $("#PONOFrom").val() + " - " + $("#PONOTo").val() + ", ";
                    }
                }

                filters += self.get_item_title();

                if ($("#SupplierID").val() != 0) {
                    filters += "Supplier: " + $("#SupplierName").val() + ", ";
                }

                if ($("#SupplierInvoiceNO").val() != "") {
                    filters += "Supplier Invoice No: " + $("#SupplierInvoiceNO").val() + ", ";
                }

                if ($("#Userslist").val() != 0) {
                    filters += "Employee: " + $("#Userslist Option:selected").text() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;

            case "QualityCheck":

                if (($("#PODateFrom").val() + $("#PODateTo").val()).trim() != "") {
                    if ($("#PODateFrom").val().trim() == "" || $("#PODateTo").val().trim() == "") {
                        filters += "Request Order Date: " + $("#PODateFrom").val() + $("#PODateTo").val() + ", ";
                    } else {
                        filters += "Request Order Date: " + $("#PODateFrom").val() + " - " + $("#PODateTo").val() + ", ";
                    }
                }

                if (($("#GRNFromDate").val() + $("#GRNToDate").val()).trim() != "") {
                    if ($("#GRNFromDate").val().trim() == "" || $("#GRNToDate").val().trim() == "") {
                        filters += "Request GRN Date: " + $("#GRNFromDate").val() + $("#GRNToDate").val() + ", ";
                    } else {
                        filters += "Request GRN Date: " + $("#GRNFromDate").val() + " - " + $("#GRNToDate").val() + ", ";
                    }
                }

                if (($("#PONOFrom").val() + $("#PONOTo").val()).trim() != "") {
                    if ($("#PONOFrom").val().trim() == "" || $("#PONOTo").val().trim() == "") {
                        filters += "Request Order No: " + $("#PONOFrom").val() + $("#PONOTo").val() + ", ";
                    } else {
                        filters += "Request Order No: " + $("#PONOFrom").val() + " - " + $("#PONOTo").val() + ", ";
                    }
                }

                if (($("#GRNNOFrom").val() + $("#GRNNOTo").val()).trim() != "") {
                    if ($("#GRNNOFrom").val().trim() == "" || $("#GRNNOTo").val().trim() == "") {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + $("#GRNNOTo").val() + ", ";
                    } else {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + " - " + $("#GRNNOTo").val() + ", ";
                    }
                }

                if (($("#QCNOFrom").val() + $("#QCNOTo").val()).trim() != "") {
                    if ($("#QCNOFrom").val().trim() == "" || $("#QCNOTo").val().trim() == "") {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + $("#QCNOTo").val() + ", ";
                    } else {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + " - " + $("#QCNOTo").val() + ", ";
                    }
                }

                filters += self.get_item_title();


                if ($("#Status").val() != 0) {
                    filters += "Status: " + $("#Status").val() + ", ";
                }

                if ($("#ItemType:visible").length) {
                    if ($("#ItemType").val() != 0) {
                        filters += "Item Type: " + $("#ItemType").val() + ", ";
                    }
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;
            case "PurchaseInvoice":

                if (($("#PONOFrom").val() + $("#PONOTo").val()).trim() != "") {
                    if ($("#PONOFrom").val().trim() == "" || $("#PONOTo").val().trim() == "") {
                        filters += "Request Order No: " + $("#PONOFrom").val() + $("#PONOTo").val() + ", ";
                    } else {
                        filters += "Request Order No: " + $("#PONOFrom").val() + " - " + $("#PONOTo").val() + ", ";
                    }
                }

                if (($("#QCNOFrom").val() + $("#QCNOTo").val()).trim() != "") {
                    if ($("#QCNOFrom").val().trim() == "" || $("#QCNOTo").val().trim() == "") {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + $("#QCNOTo").val() + ", ";
                    } else {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + " - " + $("#QCNOTo").val() + ", ";
                    }
                }

                if (($("#GRNNOFrom").val() + $("#GRNNOTo").val()).trim() != "") {
                    if ($("#GRNNOFrom").val().trim() == "" || $("#GRNNOTo").val().trim() == "") {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + $("#GRNNOTo").val() + ", ";
                    } else {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + " - " + $("#GRNNOTo").val() + ", ";
                    }
                }

                if (($("#InvoiceNOFrom").val() + $("#InvoiceNOTo").val()).trim() != "") {
                    if ($("#InvoiceNOFrom").val().trim() == "" || $("#InvoiceNOTo").val().trim() == "") {
                        filters += "Request Invoice No: " + $("#InvoiceNOFrom").val() + $("#InvoiceNOTo").val() + ", ";
                    } else {
                        filters += "Request Invoice No: " + $("#InvoiceNOFrom").val() + " - " + $("#InvoiceNOTo").val() + ", ";
                    }
                }

                if ($("#ItemCategoryID").val() != 0) {
                    filters += "Item Category: " + $("#ItemCategoryID Option:selected").text() + ", ";
                }

                filters += self.get_supplier_title();

                if ($("#ItemID").val() == "") {
                    if (($("#FromItemRange").val() + $("#ToItemRange").val()).trim() != "") {
                        if ($("#ToItemRange").val().trim() == "") {
                            filters += "Item Name Range " + "From: " + $("#FromItemRange").val();
                        }
                        else if ($("#FromItemRange").val().trim() == "") {
                            filters += "Item Name Range " + "To: " + $("#ToItemRange").val() + ", ";
                        }
                        else if (($("#FromItemRange").val() + $("#ToItemRange").val()).trim() != "") {
                            filters += "Item Name Range: " + $("#FromItemRange").val() + " - " + $("#ToItemRange").val() + ", ";
                        }
                    }
                }

                if ($("#ItemID").val() != 0) {
                    filters += "Item: " + $("#ItemName").val() + ", ";
                }

                if ($("#SupplierInvoiceNO").val() != "") {
                    filters += "Supplier Invoice No: " + $("#SupplierInvoiceNO").val() + ", ";
                }

                if ($("#Status").val() != 0) {
                    filters += "Status: " + $("#Status").val() + ", ";
                }

                if (($("#SRNFrom").val() + $("#SRNTo").val()).trim() != "") {
                    if ($("#SRNFrom").val().trim() == "" || $("#SRNTo").val().trim() == "") {
                        filters += "Request No: " + $("#SRNFrom").val() + $("#SRNTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#SRNFrom").val() + " - " + $("#SRNTo").val() + ", ";
                    }
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;

            case "MilkPurchase":

                if ($("#SupplierID").val() != 0) {
                    filters += "Supplier: " + $("#SupplierName").val() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }
                data += "&Filters=" + filters;
                break;

         

            case "MISReport":

                filters += self.get_supplier_title();
               
                if (($("#InvoiceNOFrom").val() + $("#InvoiceNOTo").val()).trim() != "") {
                    if ($("#InvoiceNOFrom").val().trim() == "" || $("#InvoiceNOTo").val().trim() == "") {
                        filters += "Request No: " + $("#InvoiceNOFrom").val() + $("#InvoiceNOTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#InvoiceNOFrom").val() + " - " + $("#InvoiceNOTo").val() + ", ";
                    }
                }

                if ($("#SupplierInvoiceNO").val() != "") {
                    filters += "Supplier Invoice No: " + $("#SupplierInvoiceNO").val() + ", ";
                }

                if ($("#OutstandingDays").val() != "") {
                    filters += "Outstanding Days: " + $("#OutstandingDays").val() + ", ";
                }

                if ($(".payable:visible").length){
                    filters += "Balance Payable: " + $(".payable:checked").closest("div").next("label").text() + ", ";
                }

                if ($("#Status").val() != 0) {
                    filters += "Status: " + $("#Status").val() + ", ";
                } 

                if ($("#TransTypeID").val() == "") {
                    if (($("#FromTransTypeRange").val() + $("#ToTransTypeRange").val()).trim() != "") {
                        if ($("#ToTransTypeRange").val().trim() == "") {
                            filters += "Trans Type " + "From: " + $("#FromTransTypeRange").val() + ", ";
                        }
                        else if ($("#FromTransTypeRange").val().trim() == "") {
                            filters += "Trans Type " + "To: " + $("#ToTransTypeRange").val() + ", ";
                        }
                        else if (($("#FromTransTypeRange").val() + $("#ToTransTypeRange").val()).trim() != "") {
                            filters += "Trans Type: " + $("#FromTransTypeRange").val() + " - " + $("#ToTransTypeRange").val() + ", ";
                        }
                    }
                }

                if ($("#TransTypeID").val() != 0) {
                    filters += "Trans Type: " + $("#TransType").val() + ", ";
                } 

                if ($("#KeyValue").val() != 0) {
                    filters += "Key Value: " + $("#KeyValue").val() + ", ";
                }

                if ($("#ItemAccountCategoryID").val() == "") {
                    if (($("#FromItemAccountsCategory").val() + $("#ToItemAccountsCategory").val()).trim() != "") {
                        if ($("#ToItemAccountsCategory").val().trim() == "") {
                            filters += "Item Account Category " + "From: " + $("#FromItemAccountsCategory").val() + ", ";
                        }
                        else if ($("#FromItemAccountsCategory").val().trim() == "") {
                            filters += "Item Account Category " + "To: " + $("#ToItemAccountsCategory").val() + ", ";
                        }
                        else if (($("#FromItemAccountsCategory").val() + $("#ToItemAccountsCategory").val()).trim() != "") {
                            filters += "Item Account Category: " + $("#FromItemAccountsCategory").val() + " - " + $("#ToItemAccountsCategory").val() + ", ";
                        }
                    }
                }


                if ($("#ItemAccountCategoryID").val() != 0) {
                    filters += "Item Account Category: " + $("#ItemAccountCategory").val() + ", ";
                } 
                
                if ($("#AgeingBucket:visible").length) {
                    if ($("#AgeingBucket").val() != 0) {
                        filters += "Ageing Bucket: " + $("#AgeingBucket").val() + ", ";
                    }
                } 

                if ($("#ItemCategory").val() != 0) {
                    filters += "Item Category: " + $("#ItemCategory option:selected").text() + ", ";
                }

                if ($("#ItemID").val() == "") {
                    if (($("#FromItemNameRangeMis").val() + $("#ToItemNameRangeMis").val()).trim() != "") {
                        if ($("#ToItemNameRangeMis").val().trim() == "") {
                            filters += "Item Name Range " + "From: " + $("#FromItemNameRangeMis").val() + ", ";
                        }
                        else if ($("#FromItemNameRangeMis").val().trim() == "") {
                            filters += "Item Name Range " + "To: " + $("#ToItemNameRangeMis").val() + ", ";
                        }
                        else if (($("#FromItemNameRangeMis").val() + $("#ToItemNameRangeMis").val()).trim() != "") {
                            filters += "Item Name Range: " + $("#FromItemNameRangeMis").val() + " - " + $("#ToItemNameRangeMis").val() + ", ";
                        }
                    }
                }

                if ($("#ItemID").val() != 0) {
                    filters += "Item: " + $("#ItemName").val() + ", ";
                }

                if ($("#AccountNameID").val() == "") {
                    if (($("#AccountCodeFrom").val() + $("#AccountCodeTo").val()).trim() != "") {
                        if ($("#AccountCodeTo").val().trim() == "") {
                            filters += "Account Code Range " + "From: " + $("#AccountCodeFrom").val() + ", ";
                        }
                        else if ($("#AccountCodeFrom").val().trim() == "") {
                            filters += "Account Code Range " + "To: " + $("#AccountCodeTo").val() + ", ";
                        }
                        else if (($("#AccountCodeFrom").val() + $("#AccountCodeTo").val()).trim() != "") {
                            filters += "Account Code Range: " + $("#AccountCodeFrom").val() + " - " + $("#AccountCodeTo").val() + ", ";
                        }
                    }
                }

                if ($("#AccountNameID").val() == "") {
                    if (($("#AccountNameFromRange").val() + $("#AccountNameToRange").val()).trim() != "") {
                        if ($("#AccountNameToRange").val().trim() == "") {
                            filters += "Account Name Range " + "From: " + $("#AccountNameFromRange").val() + ", ";
                        }
                        else if ($("#AccountNameFromRange").val().trim() == "") {
                            filters += "Account Name Range " + "To: " + $("#AccountNameToRange").val() + ", ";
                        }
                        else if (($("#AccountNameFromRange").val() + $("#AccountNameToRange").val()).trim() != "") {
                            filters += "Account Name Range: " + $("#AccountNameFromRange").val() + " - " + $("#AccountNameToRange").val() + ", ";
                        }
                    }
                }

                if ($("#AccountNameID").val() != 0) {
                    filters += "Account Name: " + $("#AccountName").val() + ", ";
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

    get_item_title: function () {
        self = purchase;
        var filters = "";
        if ($("#ItemID").val() == "") {
            if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
                if ($("#ToItemNameRange").val().trim() == "") {
                    filters += "Item Name Range " + "From: " + $("#FromItemNameRange").val();
                }
                else if ($("#FromItemNameRange").val().trim() == "") {
                    filters += "Item Name Range " + "To: " + $("#ToItemNameRange").val() + ", ";
                }
                else if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
                    filters += "Item Name Range: " + $("#FromItemNameRange").val() + " - " + $("#ToItemNameRange").val() + ", ";
                }
            }
        }

        if ($("#ItemID").val() != 0) {
            filters += "Item: " + $("#ItemName").val() + ", ";
        }
        return filters;
    },

    get_supplier_title: function () {
        self = purchase;
        var filters = "";
        if ($("#SupplierID").val() == "") {
            if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
                if ($("#ToSupplierRange").val().trim() == "") {
                    filters += "Supplier Name Range " + "From: " + $("#FromSupplierRange").val() + ", ";
                }
                else if ($("#FromSupplierRange").val().trim() == "") {
                    filters += "Supplier Name Range " + "To: " + $("#ToSupplierRange").val() + ", ";
                }
                else if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
                    filters += "Supplier Name Range: " + $("#FromSupplierRange").val() + " - " + $("#ToSupplierRange").val() + ", ";
                }
            }
        }

        if ($("#SupplierID").val() != 0) {
            filters += "Supplier: " + $("#SupplierName").val() + ", ";
        }
        return filters;
    },

    show_purchase_order_type: function () {
        var self = purchase;
        //var type = $(this).val();
        //$("#ItemAutoType").val(type);
        var type = $(".order-type:checked").val();
        $("#ItemAutoType").val(type);
        var detail = $(".summary:checked").val();
        var supplier = $(".report_type:checked").val();
        if (type == "Stock") {
            $(".stock").show();
            $(".podate").text('PO Date From');
            $(".podateto").text('PO Date To');
            $(".prdate").text('PR Date From');
            $(".prdateto").text('PR Date To');
            $(".pono").text('PO No From');
            $(".ponoto").text('PO No To');
            $(".prno").text('PR No From');
            $(".prnoto").text('PR No To');
            //if (detail == "Detail") {
            //$(".podate").text('PO Date');
            //$(".podateto").text('PO Date');
            //$(".pono").text('PO No');
            //$(".ponoto").text('PO No');
            //}
            $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': purchase.get_suppliers, 'minLength': 1 });
        }
        else {
            $(".stock").hide();
            $(".podate").text('POS Date From');
            $(".podateto").text('POS Date To');
            $(".prdate").text('PRS Date From');
            $(".prdateto").text('PRS Date To');
            $(".pono").text('POS No From');
            $(".ponoto").text('POS No To');
            $(".prno").text('PRS No From');
            $(".prnoto").text('PRS No To');
            //if (detail == "Detail") {
            //$(".podate").text('POS Date');
            //$(".podateto").text('POS Date');
            //$(".pono").text('POS No');
            //$(".ponoto").text('POS No');

            //}
            $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': purchase.get_servicesuppliers, 'minLength': 1 });
            //$(".stock-supplier-wise detail").addClass("uk-hidden");
            self.refresh();
        }
        $("#ItemName").val('');
        item_category_type = $(".order-type:checked").val();
        if (item_category_type == null || item_category_type == " ") {
            item_category_type = 0;
        }
        $.ajax({
            url: '/Reports/Purchase/GetItemCategory',
            dataType: "json",
            type: "POST",
            data: { "Type": item_category_type },
            success: function (response) {
                $("#ItemCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#ItemCategoryID").append(html);
            }
        });
        self.refresh();
    },

    change_category: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
    },
    get_items: function (release) {
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
        var self = purchase;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },

    select_item: function () {
        var self = purchase;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        UIkit.modal($('#select-item')).hide();
    },
    set_stock_item: function (event, item) {
        var self = purchase;
        $("#StockItem").val(item.Name);
        $("#ItemID").val(item.id);
    },
    show_purchase_order_report_type: function () {
        self = purchase;
        var report_type = $(this).val();
        var detail = $(".summary:checked").val();
        var supplier = $(".report_type:checked").val();

        if (report_type == "Item Wise") {
            $(".supplier-wise").hide();
            $(".stock").hide();
            $(".item-wise").show();
            var type = $(".order-type:checked").val();
            $("#ItemAutoType").val(type);
            if (type == "Stock") {
                $(".stock").show();
                //$(".podate").text('PO Date From');
                //$(".podateto").text('PO Date To');
                //$(".prdate").text('PR Date From');
                //$(".prdateto").text('PR Date To');
                //$(".pono").text('PO No');
                //$(".ponoto").text('PO No To');
                //$(".prno").text('PR No From');
                //$(".prnoto").text('PR No To');
                //if (detail == "Detail" && supplier == "supplier-wise") {
                //    $(".stock-supplier-wise detail").removeClass("uk-hidden");
                //}                                                                
            }
            else {
                $(".stock").hide();
                //$(".podate").text('POS Date');
                //$(".podateto").text('POS Date');
                $(".prdate").text('PRS Date From');
                $(".prdateto").text('PRS Date To');
                //$(".pono").text('POS No');
                //$(".ponoto").text('POS No');
                $(".prno").text('PRS No From');
                $(".prnoto").text('PRS No To');
                //$(".stock-supplier-wise detail").addClass("uk-hidden");
            }
        }
        else if (report_type == "Supplier Wise") {
            $(".item-wise").hide();
            $(".supplier-wise").show();
            $(".stock").hide();
            var type = $(".order-type:checked").val();
            if (type == "Stock") {
                $(".stock").show();
                $(".podate").text('PO Date From');
                $(".podateto").text('PO Date To');
                //$(".prdate").text('PR Date From');
                //$(".prdateto").text('PR Date To');
                //$(".pono").text('PO No');
                //$(".ponoto").text('PO No To');
                //$(".prno").text('PR No From');
                //$(".prnoto").text('PR No To');
                //$(".stock-supplier-wise detail").addClass("uk-hidden"); 
            }
        }
        else {
            $(".stock").hide();
            //$(".podate").text('POS Date');
            //$(".podateto").text('POS Date');
            $(".prdate").text('PRS Date From');
            $(".prdateto").text('PRS Date To');
            //$(".pono").text('POS No');
            //$(".ponoto").text('POS No');
            $(".prno").text('PRS No From');
            $(".prnoto").text('PRS No To');
        }
        self.refresh();
    },
    show_pur_report_type: function () {
        self = purchase;
        if (is_first_run) {
            is_first_run = false;
            setTimeout(function () { is_first_run = true }, 100);
        } else {
            return;
        }
        var item_type = $(this).val();
        console.log(item_type);
        if (item_type == "Summary") {
            $(".summary").addClass("uk-hidden");
            $(".detail").removeClass("uk-hidden");
        }
        else {
            $(".summary").removeClass("uk-hidden");
            $(".detail").addClass("uk-hidden");
        }
        return false;
    },
    show_purchase_requsition_report_type: function () {
        self = purchase;
        var type = $(".requisition-type:checked").val();
        $("#ItemAutoType").val(type);
        var item_type = $(".requisition-summary:checked").val();
        $("#ItemType").val(type).trigger('change');
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".stock").show();
                //$(".prno").text('PR No');
                //$(".stock-detail").removeClass("uk-hidden");
            }
            else {
                $(".summary").hide();
                $(".detail").hide();
                $(".summary").show();
                $(".stock").hide();
                $(".prno").text('PR No From');
                $(".prsdatefrom").text('PR Date From');
                $(".prsdateto").text('PR Date To');
                $(".prno").text('PR No From');
                $(".prnoto").text('PR No To');
                //$(".stock-detail").addClass("uk-hidden");
            }

            self.refresh();
        }
        else if (type == "Service") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".stock").show();
                $(".stock-summary").hide();
                //$(".prno").text('PRS No');
                //$(".stock-detail").addClass("uk-hidden");

            }
            else {
                $(".summary").hide();
                $(".detail").hide();
                $(".summary").show();
                $(".stock").hide();
                $(".stock-summary").hide();
                $(".prsdatefrom").text('PRS Date From');
                $(".prsdateto").text('PRS Date To');
                $(".prno").text('PRS No From');
                $(".prnoto").text('PRS No To');
                //$(".stock-detail").addClass("uk-hidden");

            }

            self.refresh();
            self.get_category(type);
        }
    },
    get_category: function (type) {
        var self = purchase;
        $.ajax({
            url: '/Reports/Purchase/GetCategory/',
            dataType: "json",
            type: "GET",
            data: {
                Type: type,
            },
            success: function (response) {
                $("#ItemCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.Value + "'>" + record.Text + "</option>";
                });
                $("#ItemCategory").append(html);
            }
        });
    },
    show_grn_report_type: function () {
        self = purchase;
        var type = $(".grn_type:checked").val();
        $("#ItemAutoType").val(type);
        var item_type = $(".grn-summary:checked").val();
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                //$(".supplier").removeClass("uk-hidden");
                $(".GRN").removeClass("uk-hidden");
                $(".SRN").addClass("uk-hidden");
                $(".invoice").removeClass("uk-hidden");
                //$(".grnfrom").text("GRN No");
                //$(".pofrom").text("PO No");
                // $(".login-date").addClass("uk-hidden");
            }
            else {
                $(".detail").hide();
                $(".summary").hide();
                $(".fromgrndate").text("GRN From Date");
                $(".togrndate").text("GRN To Date");
                $(".GRN").removeClass("uk-hidden");
                $(".SRN").addClass("uk-hidden");
                $(".invoice").addClass("uk-hidden");
                //$(".supplier").addClass("uk-hidden");
                $(".grnfrom").text("GRN No From");
                $(".pofrom").text("PO No From");
                $(".summary").show();
            }
            self.refresh();
        }
        else {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".SRN").removeClass("uk-hidden");
                $(".GRN").addClass("uk-hidden");
                $(".invoice").removeClass("uk-hidden");
                //$(".supplier").removeClass("uk-hidden");
                //$(".srnfrom").text("SRN No");
                //$(".pofrom").text("PO No");
                //  $(".login-date").addClass("uk-hidden");
            }
            else {
                $(".detail").hide();
                $(".summary").hide();
                $(".SRN").removeClass("uk-hidden");
                $(".GRN").addClass("uk-hidden");
                $(".invoice").addClass("uk-hidden");
                //$(".supplier").addClass("uk-hidden");
                $(".srnfrom").text("SRN No From");
                $(".fromgrndate").text("SRN From Date");
                $(".togrndate").text("SRN To Date");
                $(".pofrom").text("PO No From");
                $(".summary").show();

            }
            self.refresh();
        }
    },
    show_report_type: function () {
        self = purchase;
        var report_type = $(this).val();
        $("#select-type").removeClass("uk-hidden");
        $(".item-wise").hide();
        if (report_type == "All") {
            $(".item-wise").hide();
        } else if (report_type == "item-wise") {
            $(".supplier-wise").hide();
            $(".item-wise").show();
        }
        else if (report_type == "ItemCategoryWise") {
            $(".category-wise").show();
        }
        else if (report_type == "supplier-wise") {
            $(".item-wise").hide();
            $(".supplier-wise").show();
        }
        if ($('.category-wise').is(':visible')) {
            $("#select-type").removeClass("uk-hidden");
        } else {
            $("#select-type").addClass("uk-hidden");
        }
        self.refresh();
        self.clear_item();
    },
    show_report_item_type: function () {
        self = purchase;
        var item_type = $(this).val();
        var detail = $(".summary:checked").val();
        var supplier = $(".report_type:checked").val();
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var date = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();
        if (item_type == "Detail") {
            $(".summary").hide();
            $(".supplier-wise").hide();
            $(".item-wise").hide();
            $("#Mode").removeClass("uk-hidden");
            var mode = $(".report_type:checked").val();
            $("." + mode).show();
            $(".invoice").text('InvoiceNo');
            $(".prno").text("PR No From");
            //$(".pono").text("PO No");
            $(".prdate").text("PR Date From");
            //$(".podate").text("PO Date");
            var type = $(".order-type:checked").val();
            if (type == "Stock") {
                $(".stock").show();
                //$(".podate").text('PO Date');
                $(".podateto").text('PO Date To');
                $(".prdate").text('PR Date From');
                $(".prdateto").text('PR Date To');
                //$(".pono").text('PO No ');
                $(".ponoto").text('PO No To');
                $(".prno").text('PR No From');
                $(".prnoto").text('PR No To');
            }
            else {
                $(".stock").hide();
                $(".podate").text('POS Date From');
                $(".podateto").text('POS Date To');
                //$(".prdate").text('PRS Date From');
                //$(".prdateto").text('PRS Date To');
                //$(".pono").text('POS No');
                //$(".ponoto").text('POS No');
                $(".prno").text('PRS No From');
                $(".prnoto").text('PRS No To');
                //$(".stock-supplier-wise detail").addClass("uk-hidden");
            }
            self.refresh();
        }
        else {
            $(".supplier-wise").hide();
            $(".item-wise").hide();
            $("#Mode").addClass("uk-hidden");
            $(".summary").show();
            $(".invoice").text('Invoice No From');
            $(".prno").text("PR No From");
            $(".pono").text("PO No From");
            $(".prdate").text("PR Date From");
            $(".podate").text("PO Date From");
            $(".podateto").text("PO Date To");
            var type = $(".order-type:checked").val();
            if (type == "Stock") {
                $(".stock").show();
                $(".service").show();
                $(".podate").text('PO Date From');
                $(".podateto").text('PO Date To');
                $(".prdate").text('PR Date From');
                $(".pono").text('PO No From');
                $(".ponoto").text('PO No To');
                $(".prno").text('PR No From');
                $(".prnoto").text('PR No To');
                //$(".stock-supplier-wise detail").addClass("uk-hidden");

            }
            else {
                $(".stock").hide();
                $(".supplier-wise").show();
                $(".service").show();
                $(".podate").text('POS Date From');
                $(".podateto").text('POS Date To');
                $(".prdate").text('PRS Date From');
                $(".pono").text('POS No From');
                $(".ponoto").text('POS No To');
                $(".prno").text('PRS No From');
                $(".prnoto").text('PRS No To');

            }
        }
        self.refresh();
    },
    invoice_show_report_item_type: function () {
        self = purchase;
        var item_type = $(".invoice_Summary:checked").val();
        var type = $(".invoice_type:checked").val();
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".without-item-wise").hide();
                $(".status").addClass("uk-hidden");
                $(".item-wise").hide();
                $(".detail").show();
                $(".service-summary").hide();
                $(".invoice").text('Invoice No');
                $("#Mode").addClass("uk-hidden");
            }
            else {
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".detail").hide();
                $("#Mode").removeClass("uk-hidden");
                $(".summary").hide();
                $(".service-summary").hide();
                $(".invoice").text('Invoice No From');
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
                    $(".item-wise").hide();
                    $(".summary").hide();
                    $(".detail").hide();
                    $(".item-wise").show();
                }
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

    requisition_show_report_item_type: function () {
        self = purchase;
        var item_type = $(this).val();
        var type = $(".requisition-type:checked").val();
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".stock").show();
                //$(".prno").text('PR No');
                //$(".stock-detail").removeClass("uk-hidden");

            }
            else {
                $(".summary").hide();
                $(".detail").hide();
                $(".summary").show();
                $(".stock").hide();
                $(".prno").text('PR No From');
                $(".prsdatefrom").text('PR Date From');
                $(".prsdateto").text('PR Date To');
                $(".prno").text('PR No From');
                $(".prnoto").text('PR No To');
                //$(".stock-detail").addClass("uk-hidden");

            }
            self.refresh();
        }
        else if (type == "Service") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".stock").show();
                $(".stock-summary").hide();
                //$(".prno").text('PRS No');
                //$(".stock-detail").addClass("uk-hidden");

            }
            else {
                $(".summary").hide();
                $(".detail").hide();
                $(".summary").show();
                $(".stock").hide();
                $(".stock-summary").hide();
                $(".prsdatefrom").text('PRS Date From');
                $(".prsdateto").text('PRS Date To');
                $(".prno").text('PRS No From');
                $(".prnoto").text('PRS No To');
                //$(".stock-detail").addClass("uk-hidden");

            }
            self.refresh();
        }
    },

    show_grn_report_item_type: function () {
        self = purchase;
        var type = $(".grn_type:checked").val();
        var item_type = $(this).val();
        if (type == "Stock") {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                //$(".supplier").removeClass("uk-hidden");
                $(".GRN").removeClass("uk-hidden");
                $(".SRN").addClass("uk-hidden");
                $(".invoice").removeClass("uk-hidden");
                //$(".grnfrom").text("GRN No");
                //$(".pofrom").text("PO No");
                //$(".srn-detailed").removeClass("uk-hidden");
                $(".login-date").hide();
            }
            else {
                $(".detail").hide();
                $(".summary").hide();
                $(".GRN").removeClass("uk-hidden");
                $(".SRN").addClass("uk-hidden");
                //$(".supplier").addClass("uk-hidden");
                $(".invoice").addClass("uk-hidden");
                $(".grnfrom").text("GRN No From");
                $(".pofrom").text("PO No From");
                $(".summary").show();
            }
        }
        else {
            if (item_type == "Detail") {
                $(".summary").hide();
                $(".detail").hide();
                $(".detail").show();
                $(".SRN").removeClass("uk-hidden");
                $(".GRN").addClass("uk-hidden");
                $(".invoice").removeClass("uk-hidden");
                //$(".supplier").removeClass("uk-hidden");
                //$(".srnfrom").text("SRN No");
                //$(".pofrom").text("PO No");
                $(".login-date").removeClass("uk-hidden");
            }
            else {
                $(".detail").hide();
                $(".summary").hide();
                $(".SRN").removeClass("uk-hidden");
                $(".GRN").addClass("uk-hidden");
                $(".invoice").addClass("uk-hidden");
                //$(".supplier").addClass("uk-hidden");
                $(".srnfrom").text("SRN No From");
                $(".pofrom").text("PO No From");
                $(".summary").show();
                $(".login-date").addClass("uk-hidden");
                $(".LoginDateNo").hide();

            }
        }
        self.refresh();
    },

    show_qc_report_item_type: function () {
        self = purchase;
        var type = $(".qc-summary:checked").val();
        var qctype = $(".qc-type:checked").val();
        if (type == "Detail") {
            $(".qc-type").removeClass("uk-hidden");
            $(".summary").hide();
            $(".detail").hide();
            //$(".qcfrom").text('QC NO');
            //$(".grnnofrom").text('GRN NO');
            $(".detail").show();
            if (qctype == "QCAssurance") {
                $(".item-type").addClass("uk-hidden");
                $(".detail").removeClass("uk-hidden");
            }
            else {
                $(".item-type").removeClass("uk-hidden")
            }

        }
        else {
            $(".qc-type").addClass("uk-hidden");
            $(".item-type").addClass("uk-hidden")
            $(".summary").hide();
            $(".detail").hide();
            $(".summary").show();
            $(".qcfrom").text('QC NO From');
            $(".grnnofrom").text('GRN NO From');
        }
        self.refresh();
    },

    show_qc_itemType: function () {
        self = purchase;
        var type = $(".qc-summary:checked").val();
        var qctype = $(".qc-type:checked").val();
        if (type == "Detail") {
            $(".qc-type").removeClass("uk-hidden");
            $(".summary").hide();
            $(".detail").hide();
            //$(".qcfrom").text('QC NO');
            //$(".grnnofrom").text('GRN NO');
            $(".detail").show();
            if (qctype == "QCAssurance") {
                $(".item-type").addClass("uk-hidden")
            }
            else {
                $(".item-type").removeClass("uk-hidden")
            }
        }
        else {
            $(".qc-type").addClass("uk-hidden");
            $(".item-type").addClass("uk-hidden")
            $(".summary").hide();
            $(".detail").hide();
            $(".summary").show();
            $(".qcfrom").text('QC NO From');
            $(".grnnofrom").text('GRN NO From');
        }

    },

    clear_item: function () {
        $("#ItemCategoryID").val('');
        $("#ItemID").val('');
        $("#ItemName").val('');
    },
    get_to_range: function () {
        var self = purchase;
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

    get_to_itemrange: function () {
        var self = purchase;
        var from_range = $("#FromItemNameRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemNameRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemNameRange").append(html);
            }
        });
    },
    get_to_misitemrange: function () {
        var self = purchase;
        var from_range = $("#FromItemNameRangeMis").val();
        $.ajax({
            url: '/Reports/Purchase/MISGetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemNameRangeMis").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemNameRangeMis").append(html);
            }
        });
    },

    get_to_transtyperange: function () {
        var self = purchase;
        var from_range = $("#FromTransTypeRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetTranTypeRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToTransTypeRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToTransTypeRange").append(html);
            }
        });
    },

    get_to_item_account_category_range: function () {
        var self = purchase;
        var from_range = $("#FromItemAccountsCategory").val();
        $.ajax({
            url: '/Reports/Purchase/MISGetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemAccountsCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemAccountsCategory").append(html);
            }
        });
    },

    get_to_account_name_range: function () {
        var self = purchase;
        var from_range = $("#AccountNameFromRange").val();
        $.ajax({
            url: '/Reports/Purchase/MISGetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#AccountNameToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#AccountNameToRange").append(html);
            }
        });
    },

    get_to_itemcategoryrange() {
        var self = purchase;
        var from_range = $("#FromItemCategoryRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemCategoryRange").append(html);
            }
        })
    },

    get_to_ToDepartmentrange: function () {
        var self = purchase;
        var from_range = $("#ToDepartmentFromRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetToDepartmentRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToDepartmentToRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToDepartmentToRange").append(html);
            }
        });
    },
    get_to_userrange: function () {
        var self = purchase;
        var from_range = $("#FromUserRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetUserRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToUserRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToUserRange").append(html);
            }
        });
    },
    get_to_documentrange: function () {
        var self = purchase;
        var from_range = $("#FromDocumentRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetDocumentRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToDocumentRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToDocumentRange").append(html);
            }
        });
    },
    get_to_categoryrange: function () {
        var self = purchase;
        var from_range = $("#FromItemCategoryRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemCategoryRange").append(html);
            }
        });
    },

    get_prno: function (release) {
        var type = $(".order-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseRequisition';
        }
        else if (type == "Service") {
            Table = 'PurchaseRequisitionForservice';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PRNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },


    get_purchase_return_no: function (release) {

        var Table;
        Table = 'PurchaseReturn';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PurchaseRturnNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },


    set_prno: function (event, item) {
        self = purchase;
        $("#PRNOFrom").val(item.code);
        $("#PRID").val(item.id);
        $("#PRNOFromID").val(item.id);

    },
    get_requisition_prno: function (release) {
        var type = $(".requisition-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseRequisition';
        }
        else if (type == "Service") {
            Table = 'PurchaseRequisitionForservice';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PRNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_requisition_prno: function (event, item) {
        self = purchase;
        $("#PRNOFrom").val(item.code);
        $("#PRID").val(item.id);
        $("#PRNOFromID").val(item.id);
        var report_type = $(".requisition-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            $("#PRNOTo").val(item.code);
            $("#PRNOToID").val(item.id);
        }
        else {
            $("#PRNOTo").val('');
            $("#PRNOToID").val('');
        }
    },
    invoice_refresh: function () {
        var self = purchase;
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
                $("#Mode").addClass("uk-hidden");
            }
            else {
                $(".without-item-wise").hide();
                $(".item-wise").hide();
                $(".detail").hide();
                $("#Mode").removeClass("uk-hidden");
                $(".summary").hide();
                $(".service-summary").hide();
                $(".service-srn").addClass("uk-hidden");
                $(".invoice").text('Invoice No From');
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
                    $(".item-wise").hide();
                    $(".summary").hide();
                    $(".detail").hide();
                    $(".item-wise").show();
                }
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
                $(".invoice").text('Invoice No From');
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
        self.get_invoice_item_category(type);
    },

    refresh: function (event, item) {
        self = purchase;
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

    get_prnoTo: function (release) {
        var type = $(".order-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseRequisition';
        }
        else if (type == "Service") {
            Table = 'PurchaseRequisitionForservice';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PRNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_prnoTo: function (event, item) {
        self = purchase;
        $("#PRNOTo").val(item.code);
        $("#PRID").val(item.id);
        $("#PRNOToID").val(item.id);
    },

    get_requisition_prnoTo: function (release) {
        var type = $(".requisition-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseRequisition';
        }
        else if (type == "Service") {
            Table = 'PurchaseRequisitionForservice';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PRNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_requisition_prnoTo: function (event, item) {
        self = purchase;
        $("#PRNOTo").val(item.code);
        $("#PRID").val(item.id);
        $("#PRNOToID").val(item.id);
    },

    get_pono: function (release) {
        var type = $(".order-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_pono: function (event, item) {
        self = purchase;
        $("#PONOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#PONOFromID").val(item.id);
        var report_type = $(".summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            $("#PONOTo").val(item.code);
            $("#PONOToID").val(item.id);
        }
        else {
            $("#PONOTo").val('');
            $("#PONOToID").val('');
        }
    },

    get_ponoTo: function (release) {
        var type = $(".order-type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_ponoTo: function (event, item) {
        self = purchase;
        $("#PONOTo").val(item.code);
        $("#ID").val(item.id);
        $("#PONOToID").val(item.id);
    },
    get_grn_pono: function (release) {
        var type = $(".grn_type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_grn_pono: function (event, item) {
        self = purchase;
        $("#PONOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#PONOFromID").val(item.id);
        var report_type = $(".grn-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            $("#PONOTo").val(item.code);
            $("#PONOToID").val(item.id);
        }
        else {
            $("#PONOTo").val('');
            $("#PONOToID").val('');
        }
    },

    get_grn_ponoTo: function (release) {
        var type = $(".grn_type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_grn_ponoTo: function (event, item) {
        self = purchase;
        $("#PONOTo").val(item.code);
        $("#ID").val(item.id);
        $("#PONOToID").val(item.id);
    },

    get_invoice_pono: function (release) {
        var type = $(".invoice_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_invoice_pono: function (event, item) {
        self = purchase;
        $("#PONOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#PONOFromID").val(item.id);

    },

    get_invoice_ponoTo: function (release) {
        var type = $(".invoice_type:checked").val();
        var Table;
        if (type == "Stock") {
            Table = 'PurchaseOrder';
        }
        else if (type == "Service") {
            Table = 'PurchaseOrderForService';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_invoice_ponoTo: function (event, item) {
        self = purchase;
        $("#PONOTo").val(item.code);
        $("#ID").val(item.id);
        $("#PONOToID").val(item.id);
    },

    get_qc_pono: function (release) {
        var Table = 'PurchaseOrder';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_qc_pono: function (event, item) {
        self = purchase;
        $("#PONOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#PONOFromID").val(item.id);
    },

    get_qc_ponoTo: function (release) {
        var Table = 'PurchaseOrder';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qc_ponoTo: function (event, item) {
        self = purchase;
        $("#PONOTo").val(item.code);
        $("#ID").val(item.id);
        $("#PONOToID").val(item.id);
    },

    get_qcno: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOFrom').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qcno: function (event, item) {
        self = purchase;
        $("#QCNOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#QCNOFromID").val(item.id);
    },

    get_qcnoTo: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOTo').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qcnoTo: function (event, item) {
        self = purchase;
        $("#QCNOTo").val(item.code);
        $("#ID").val(item.id);
        $("#QCNOToID").val(item.id);
    },
    get_qc_qcno: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOFrom').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qc_qcno: function (event, item) {
        self = purchase;
        var type = $(".qc-summary:checked").val();

        $("#QCNOFrom").val(item.code);

        $("#QCNOFromID").val(item.id);
        if (type == "Detail") {
            $("#QCNOTo").val(item.code);
            $("#QCNOToID").val(item.id);

        }
        else {
            $("#QCNOTo").val('');
            $("#QCNOToID").val('');
        }
    },

    get_qc_qcnoTo: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOTo').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qc_qcnoTo: function (event, item) {
        self = purchase;
        $("#QCNOTo").val(item.code);
        $("#ID").val(item.id);
        $("#QCNOToID").val(item.id);
    },

    get_grnno: function (release) {
        var type = $(".invoice_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grnno: function (event, item) {
        self = purchase;
        $("#GRNNOFrom").val(item.code);
        $("#GRNNOFromID").val(item.id);
    },

    get_grnnoTo: function (release) {
        var type = $(".invoice_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grnnoTo: function (event, item) {
        self = purchase;
        $("#GRNNOTo").val(item.code);
        $("#GRNNOToID").val(item.id);
    },
    get_qc_grnno: function (release) {

        Table = 'GRN';

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qc_grnno: function (event, item) {
        self = purchase;
        $("#GRNNOFrom").val(item.code);
        $("#GRNNOFromID").val(item.id);
        var type = $(".qc-summary:checked").val();
        if (type == 'Detail') {
            $("#GRNNOTo").val(item.code);
            $("#GRNNOToID").val(item.id);

        }
        else {
            $("#GRNNOTo").val('');
            $("#GRNNOToID").val('');

        }

    },
    get_qc_grnnoTo: function (release) {

        Table = 'GRN';

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qc_grnnoTo: function (event, item) {
        self = purchase;

        $("#GRNNOTo").val(item.code);
        $("#GRNNOToID").val(item.id);
    },
    get_grn_grnno: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grn_grnno: function (event, item) {
        self = purchase;
        $("#GRNNOFrom").val(item.code);
        $("#GRNNOFromID").val(item.id);
        var report_type = $(".grn-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same

            $("#GRNNOTo").val(item.code);
            $("#GRNNOToID").val(item.id);
        }
        else {
            $("#GRNNOTo").val('');
            $("#GRNNOToID").val('');
        }
    },
    get_grn_grnnoTo: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grn_grnnoTo: function (event, item) {
        self = purchase;
        $("#GRNNOTo").val(item.code);
        $("#GRNNOToID").val(item.id);
    },
    get_invoiceno: function (release) {
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
        self = purchase;
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

    get_mis_invoiceno: function (release) {
        var type = $(".item-type:checked").val();
        var ReportType = $(".mis_report_type:checked").val();
        var Table
        if (type == "Stock") {
            Table = 'MISPurchaseInvoice';
        }
        else if (type == "Services") {
            Table = 'MISPurchaseInvoiceService';
        }

        if (ReportType == "supplier-ageing") {
            Table = "MISInvoiceNoStockService"
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
    set_mis_invoiceno: function (event, item) {
        self = purchase;
        var prefix = "PIG";
        if (item.voucherNo.indexOf("PIS") != -1) {
            prefix = "PIS";
        }


        if ($("#InvoiceNOTo").val() != "") {
            if ($("#InvoiceNOTo").val().indexOf(prefix) == -1) {
                $("#InvoiceNOFrom").val("");
                return;
            }
        }
        $("#InvoiceNOFrom").val(item.voucherNo);
        $("#InvoiceNOFromID").val(item.id);

    },

    get_invoicenoTo: function (release) {
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
    set_invoicenoTo: function (event, item) {
        self = purchase;
        $("#InvoiceNOTo").val(item.code);
        $("#ID").val(item.id);
        $("#InvoiceNOToID").val(item.id);
    },

    get_mis_invoicenoTo: function (release) {
        var type = $(".item-type:checked").val();
        var ReportType = $(".mis_report_type:checked").val();
        var Table
        if (type == "Stock") {
            Table = 'MISPurchaseInvoice';
        }
        else if (type == "Services") {
            Table = 'MISPurchaseInvoiceService';
        }
        if (ReportType == "supplier-ageing") {
            Table = "MISInvoiceNoStockService"
        }

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

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
    set_mis_invoicenoTo: function (event, item) {
        self = purchase;
        var prefix = "PIG";
        if (item.voucherNo.indexOf("PIS") != -1) {
            prefix = "PIS";
        }


        if ($("#InvoiceNOFrom").val() != "") {
            if ($("#InvoiceNOFrom").val().indexOf(prefix) == -1) {

                $("#InvoiceNOTo").val("");
                app.show_error("Select Same Prefix");
                return;
            }
        }

        $("#InvoiceNOTo").val(item.voucherNo);
        $("#InvoiceNOToID").val(item.id);
    },

    get_srnno_invoice: function (release) {
        var type = $(".invoice_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SRNFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_srnno_invoice: function (event, item) {
        self = purchase;
        $("#SRNFrom").val(item.code);
        $("#ID").val(item.id);
        $("#SRNNOFromID").val(item.id);
    },
    get_srnnoTo_invoice: function (release) {
        var type = $(".invoice_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SRNTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_srnnoTo_invoice: function (event, item) {
        self = purchase;
        $("#SRNTo").val(item.code);
        $("#ID").val(item.id);
        $("#SRNNOToID").val(item.id);
    },
    get_srnno_grn: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SRNFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_srnno_grn: function (event, item) {
        self = purchase;
        $("#SRNFrom").val(item.code);
        $("#ID").val(item.id);
        $("#SRNNOFromID").val(item.id);

        var report_type = $(".grn-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same

            $("#SRNNOTo").val(item.code);
            $("#SRNNOToID").val(item.id);
        }
        else {
            $("#SRNNOTo").val('');
            $("#SRNNOToID").val('');
        }
    },
    get_srnnoTo_grn: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SRNTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_srnnoTo_grn: function (event, item) {
        self = purchase;
        $("#SRNTo").val(item.code);
        $("#ID").val(item.id);
        $("#SRNNOToID").val(item.id);

    },

    get_returnno: function (release) {
        var Table = "PurchaseReturn";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#ReturnNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_returnno: function (event, item) {
        self = purchase;
        $("#ReturnNOFrom").val(item.code);
        $("#ID").val(item.id);
        $("#ReturnNOFromID").val(item.id);
    },

    get_returnnoTo: function (release) {
        var Table = "PurchaseReturn";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#ReturnNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_returnnoTo: function (event, item) {
        self = purchase;
        $("#ReturnNOTo").val(item.code);
        $("#ID").val(item.id);
        $("#ReturnNOToID").val(item.id);
    },

    get_supplierinvoiceno: function (release) {
        var type = $("input[name='Type']:checked").val();
        var Table = '';
        if (type == 'Stock') {
            Table = "SupplierInvoiceNo";
        } else {
            Table = "ServiceSupplierInvoiceNo";
        }


        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SupplierInvoiceNO').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_supplierinvoiceno: function (event, item) {
        self = purchase;
        $("#SupplierInvoiceNO").val(item.code);
        $("#SupplierInvoiceNOID").val(item.id);
    },
    get_mis_supplierinvoicenoto: function (release) {
        var Table = "MISSupplierInvoiceNo";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#SupplierInvoiceNO').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_mis_supplierinvoicenoto: function (event, item) {
        self = purchase;
        $("#SupplierInvoiceNOTO").val(item.code);
        $("#SupplierInvoiceNOTOID").val(item.id);
    },
    get_mis_supplierinvoiceno: function (release) {

        var type = $(".item-type:checked").val();
        var ReportType = $(".mis_report_type:checked").val();
        var invoicetype = $(".invoice_type:checked").val();
        var grntype = $(".grn_type:checked").val();
        var Table
        if (type == "Stock") {
            Table = 'DCInvoiceNoStock';
        }
        else if (type == "Services") {
            Table = 'DCInvoiceNoService';
        }

        if (invoicetype == "Stock") {
            Table = 'DCInvoiceNoStock';
        }
        else if (invoicetype == "Services") {
            Table = 'DCInvoiceNoService';
        }


        if (ReportType == "supplier-ageing") {
            Table = 'MISSupplierInvoiceNo';
        }
        if (grntype == "Stock") {
            Table = 'SupplierInvoiceNo';
        }
        if (grntype == "Service") {
            Table = 'ServiceSupplierInvoiceNo';
        }

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#SupplierInvoiceNO').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_mis_supplierinvoiceno: function (event, item) {
        self = purchase;
        $("#SupplierInvoiceNO").val(item.code);
        $("#SupplierInvoiceNOID").val(item.id);
    },

    get_suppliers: function (release) {
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
        self = purchase;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },
    validate_form: function () {
        var self = purchase;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [
            {
                elements: "#PRNOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#PRNOToID').val());
                               var from_id = clean($('#PRNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected PR No To must be greater than or equal to PR No From'
                       },
                ]
            },
            {
                elements: "#PONOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#PONOToID').val());
                               var from_id = clean($('#PONOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected PO No To must be greater than or equal to PO No From'

                       },
                ]
            },

            {
                elements: "#QCNOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#QCNOToID').val());
                               var from_id = clean($('#QCNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected QC No To must be greater than or equal to QC No From'

                       },
                ]
            },
            {
                elements: "#GRNNOTo",
                rules: [
                       {

                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#GRNNOToID').val());
                               var from_id = clean($('#GRNNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected GRN No To must be greater than or equal to GRN No From'

                       },
                ]
            },
            {
                elements: "#InvoiceNOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#InvoiceNOToID').val());
                               var from_id = clean($('#InvoiceNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected Invoice No To must be greater than or equal to Invoice No From'

                       },
                ]
            },
               {
                   elements: "#SRNNOTo",
                   rules: [
                          {
                              type: function (element) {
                                  var error = false;
                                  var to_id = clean($('#SRNNOToID').val());
                                  var from_id = clean($('#SRNNOFromID').val());
                                  if (to_id != 0) {
                                      if (to_id < from_id) {
                                          error = true;
                                      }
                                  }
                                  return !error;
                              }, message: 'Selected SRN No To must be greater than or equal to SRN No From'

                          },
                   ]
               },

               {
                   elements: "#InvoiceNOTo",
                   rules: [
                          {
                              type: function (element) {
                                  var error = false;
                                  var to_id = clean($('#InvoiceNOToID').val());
                                  var from_id = clean($('#InvoiceNOFromID').val());
                                  if (to_id != 0) {
                                      if (to_id < from_id) {
                                          error = true;
                                      }
                                  }
                                  return !error;
                              }, message: 'Selected Invoice No To must be greater than or equal to Invoice No From'

                          },
                   ]
               },
               {
                   elements: "#AgeingBucket:visible",
                   rules: [
                           { type: form.required, message: "Please enter Ageing Bucket" },
                   ]
               }

        ],
    },

    change_type: function () {
        self = purchase;
        self.clear_grn_item();
        var report_type = $(this).val();
        if (report_type == "Service") {
            $(".GRN").hide();
            $(".SRN").removeClass("uk-hidden");
            $(".SRN").show();
        } else {
            $(".GRN").show();
            $(".SRN").hide();
        }
    },
    get_srnno: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Hint: $("#SRNFrom").val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            },
            error: function () {
            }
        });
    },
    set_srnno: function (event, item) {
        var self = purchase;
        $("#SRNFrom").val(item.code);
        $("#SRNFromID").val(item.id);
    },
    get_srnnoTo: function (release) {
        var type = $(".grn_type:checked").val();

        var Table;
        if (type == "Stock") {
            Table = 'GRN';
        }
        else if (type == "Service") {
            Table = 'ServiceReceiptNote';
        }
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Hint: $("#SRNTo").val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            },
            error: function () {
            }
        });
    },
    set_srnnoTo: function (event, item) {
        var self = purchase;
        $("#SRNTo").val(item.code);
        $("#SRNToID").val(item.id);
    },
    clear_grn_item: function () {
        $("#GRNNOFromID").val('');
        $("#GRNFrom").val('');
        $("#GRNNOToID").val('');
        $("#GRNTo").val('');
        $("#SRNFromID").val('');
        $("#SRNFrom").val('');
        $("#SRNToID").val('');
        $("#SRNTo").val('');
        $("#FromItemNameRange").val('');
        $("#ToItemNameRange").val('');
        $("#ItemID").val('');
        $("#ItemName").val('');
        $("#PONOFromID").val('');
        $("#PONOFrom").val('');
        $("#PONOToID").val('');
        $("#PONOTo").val('');
        $("#InvoiceNOFromID").val('');
        $("#User").val('');
    },
    get_accountname: function (release) {
        var self = purchase;
        var Table = "AccountHeadName";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
        var self = purchase;
        console.log(item);
        $("#AccountNameID").val(item.id);
        $("#AccountName").val(item.Value)

    },

    get_accountcodefrom: function (release) {
        var self = purchase;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
    set_accountcodefrom: function (event, item) {
        var self = purchase;
        $("#AccountCodeFromID").val(item.id);

    },
    get_accountcodeto: function (release) {
        var self = purchase;
        //var Table = "AccountEntryMaster";
        var Table = "AccountHeadCode";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
    set_accountcodeto: function (event, item) {

        var self = purchase;
        $("#AccountCodeToID").val(item.id);

    },

    get_milk_supplier: function (release) {
        var self = purchase;
        var Table = "MilkSupplier";
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#SupplierName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_milk_supplier: function (event, item) {
        self = purchase;
        $("#SupplierName").val(item.code);
        $("#SupplierID").val(item.id);
    },

    get_ageing_bucket: function (release) {
        self.purchase;
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
        var self = purchase;
        $("#AgeingBucket").val(item.code);
        $("#AgeingBucketID").val(item.id);
    },

    get_with_overruled: function () {
        var self = purchase;
        if ($("#IsIncludeOverruled").is(':checked')) {
            $("#IsOverruled").val(true);
        } else {
            $("#IsOverruled").val(false);
        }
    },
}