ReportHelper = {
    init: function () {
        var self = ReportHelper;
        self.bind_events();
    },

    bind_events: function () {
        var self = ReportHelper;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $("#report-filter-submit").on("click", self.get_report_view);

        $.UIkit.autocomplete($('#item-autocomplete'), Config.item_code);
        $.UIkit.autocomplete($('#item-name-autocomplete'), Config.item_name);
        $.UIkit.autocomplete($('#item-code-from-autocomplete'), Config.item_code_from);
        $.UIkit.autocomplete($('#item-code-to-autocomplete'), Config.item_code_to);
        $.UIkit.autocomplete($('#stock-item-autocomplete'), Config.stock_items);

        $.UIkit.autocomplete($('#batchNo-autocomplete'), Config.batch_no);
        $.UIkit.autocomplete($('#receipt-no-from-autocomplete'), Config.stock_receipt_no_from);
        $.UIkit.autocomplete($('#receipt-no-to-autocomplete'), Config.stock_receipt_no_to);
        $.UIkit.autocomplete($('#issue-no-from-autocomplete'), Config.stock_issue_no_from);
        $.UIkit.autocomplete($('#issue-no-to-autocomplete'), Config.stock_issue_no_to);

        $.UIkit.autocomplete($('#customer-autocomplete'), Config.customer_name);
        $.UIkit.autocomplete($('#customer-code-autocomplete'), Config.customer_code);
        $.UIkit.autocomplete($('#customer-code-from-autocomplete'), Config.customer_code_from);
        $.UIkit.autocomplete($('#customer-code-to-autocomplete'), Config.customer_code_to);

        $.UIkit.autocomplete($('#invoice-no-from-autocomplete'), Config.invoice_no_from);
        $.UIkit.autocomplete($('#invoice-no-to-autocomplete'), Config.invoice_no_to);

        $.UIkit.autocomplete($('#employee-autocomplete'), Config.employee_name);

        $.UIkit.autocomplete($('#sales-order-no-from-autocomplete'), Config.sales_order_no_from);
        $.UIkit.autocomplete($('#sales-order-no-to-autocomplete'), Config.sales_order_no_to);

        $.UIkit.autocomplete($('#tds-transaction-no-autocomplete'), Config.tds_transaction_no);

        $.UIkit.autocomplete($('#suppliername-autocomplete'), Config.supplier_name);

        $.UIkit.autocomplete($('#tdscode-autocomplete'), Config.tds_code);

        $.UIkit.autocomplete($('#panno-autocomplete'), Config.pan_no);

        $.UIkit.autocomplete($('#preprocess-issue-item-name-autocomplete'), Config.preprocess_issue_item);
        $.UIkit.autocomplete($('#preprocess-receipt-item-name-autocomplete'), Config.preprocess_receipt_item);

        $.UIkit.autocomplete($('#production-groupname-autocomplete'), Config.production_group_name);

        $.UIkit.autocomplete($('#production-batchno-autocomplete'), Config.production_batch_no);

        $.UIkit.autocomplete($('#preprocess-issueno-autocomplete'), Config.preprocess_issue_no);
        $.UIkit.autocomplete($('#preprocess-receiptno-autocomplete'), Config.preprocess_receipt_no);
        $.UIkit.autocomplete($('#production-group-autocomplete'), Config.production_group);
        $.UIkit.autocomplete($('#fsoname-autocomplete'), Config.fso_name);
        $.UIkit.autocomplete($('#pono-from-autocomplete'), Config.pono_from);
        $.UIkit.autocomplete($('#pono-to-autocomplete'), Config.pono_to);
    },

    get_report_view: function () {
        var self = ReportHelper;
        var report = $("#report-filter-form").data("name");
        var filtersAppliedText = window[report].get_filters();
        if ($("#FiltersApplied").length) {
            $("#FiltersApplied").val(filtersAppliedText);
        } else {
            $("#report-filter-form").append("<input type='hidden' name='Filters' value='" + filtersAppliedText + "' id='FiltersApplied'>");
        }
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        if (typeof window[report].validate_form == "function") {
            if (window[report].validate_form() != 0) {
                return false;
            }
        }
       
        if ($("#ExportType").length == 0 || $("#ExportType").val() == "Html") {
            self.hide_controls();
            var url = $("#report-filter-form").attr("action");
            $.ajax({
                url: url,
                data: data,
                dataType: "html",
                type: "POST",
                success: function (response) {
                    $("#report-viewer").html(response);
                    ReportHelper.inject_js();
                }
            });        
        } else {
            $("#report-filter-form").submit();
        }
        return false;

    },
    export_report: function (e) {
        var self = ReportHelper;
        e.preventDefault();
        var report = $("#report-filter-form").data("name");
        var export_type = $("#ExportType").val();
        $("#report-filter-form").append("<input type='hidden' name='ExportType' value='" + export_type + "' id='ExportType'>");
        $("#report-filter-form").append("<input type='hidden' name='Filters' value='" + window[report].get_filters() + "' id='FiltersApplied'>");
        $("#report-filter-form").submit();
        //$("#ExportType").remove();
        $("#FiltersApplied").remove();
        return false;
    },

    on_report_load_complete: function () {
        var self = ReportHelper;
        $(document).trigger('load-completed');
        self.show_controls();
    },

    hide_controls: function () {
        $("#report-filter-submit").hide();
        $("#Refresh").hide();
    },

    show_controls: function () {
        $("#report-filter-submit").show();
        $("#Refresh").show();
    },

    inject_js: function () {
        $("#report-viewer iframe").on('load', function () {
            var js_string = "<script type='text/javascript'>"
                + " $(document).ready(function(){ "
                    + "window.parent.ReportHelper.on_report_load_complete();"
                    + "setTimeout(function(){ "
                        + "var width = $('#report-viewer').find('iframe').contents().find('#ReportViewer1_fixedTable').width();"
                        + "var height = $('#report-viewer').find('iframe').contents().find('#ReportViewer1_fixedTable').height()+50;"
                        + "$('#report-viewer').find('iframe').css({'min-height':height, 'min-width':width}); "
                        + "$('#report-viewer').find('iframe').contents().find('#ReportViewer1').css('overflow','hidden'); "
                    + "}, 1000);"
                + " })"
                + "</script>";
            $("#report-viewer").find('iframe').contents().find("body").append($(js_string));
        });
    },

    get_item_filters: function () {
        var data = "";
        if ($('#ItemName').val() != "") {
            data += ' of ' + $('#ItemName').val();
        } else {

            if ($('#ItemCodeFrom').val() != "" && $('#ItemCodeTo').val() != "") {
                data += ' of item codes between ' + $('#ItemCodeFrom').val() + ' and ' + $('#ItemCodeTo').val()
            } else {
                data += ' of all items ';
            }
        }
        return data;
    },

    get_sales_category: function () {
        var self = ReportHelper;
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

}

