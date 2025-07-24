$(function () {
    cheque_status.bind_events();
});
cheque_status = {

    bind_events: function () {
        var self = cheque_status;
        $("#report-filter-form").on("submit", function () { return false; });

        $("#report-filter-submit").on("click", self.get_report_view);

        $('#Refresh').on('click', self.refresh);

        $('.cheque_report_name').on("ifChecked", self.cheque_report_type)

        $.UIkit.autocomplete($('#receipt-no-from-autocomplete'), { 'source': self.get_receiptno_from, 'minLength': 1 });
        $('#receipt-no-from-autocomplete').on('selectitem.uk.autocomplete', self.set_receiptno_from);

        $.UIkit.autocomplete($('#receipt-no-to-autocomplete'), { 'source': self.get_receiptno_to, 'minLength': 1 });
        $('#receipt-no-to-autocomplete').on('selectitem.uk.autocomplete', self.set_receiptno_to);

        $.UIkit.autocomplete($('#sales-customercodefrom-autocomplete'), { 'source': self.get_customercode_from, 'minLength': 1 });
        $('#sales-customercodefrom-autocomplete').on('selectitem.uk.autocomplete', self.set_customercode_from);

        $.UIkit.autocomplete($('#sales-customercodeTo-autocomplete'), { 'source': self.get_customercode_To, 'minLength': 1 });
        $('#sales-customercodeTo-autocomplete').on('selectitem.uk.autocomplete', self.set_customercode_To);

        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customername, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customername);

        $.UIkit.autocomplete($('#cheque-no-autocomplete'), { 'source': self.get_cheque_No, 'minLength': 1 });
        $('#cheque-no-autocomplete').on('selectitem.uk.autocomplete', self.set_cheque_No);
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
        switch (name) {
            case "ChequeStatus":
                if ($("#Customer").val() != "") {
                    filters += "Customer: " + $("#Customer").val() + ", ";
                }
                if ($("#LocationsID").val() != "") {
                    filters += "Location: " + $("#LocationsID Option:selected").text() + ", ";
                }
                if ($("#ChequeNo").val() != "") {
                    filters += "Cheque No: " + $("#ChequeNo").val() + ", ";
                }
                if ($("#ChequeDate").val() != "") {
                    filters += "Cheque Date: " + $("#ChequeDate").val() + ", ";
                }
                if ($("#ChequeStatus").val() != "") {
                    filters += "Cheque Status: " + $("#ChequeStatus").val() + ", ";
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

    get_receiptno_from: function (release) {
        self.cheque_status;
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
        self.cheque_status;
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
        self = cheque_status;
        $("#CustomerCodeFrom").val(item.code);
        $("#CustomerCodeFromID").val(item.id);
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
        self = cheque_status;
        $("#CustomerCodeTo").val(item.code);
        $("#CustomerCodeToID").val(item.id);
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
        self = cheque_status;
        $("#Customer").val(item.name);
        $("#CustomerID").val(item.id);

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

    refresh: function (event, item) {
        self = cheque_status;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#ReceiptDateFrom").val(findate);
        $("#ReceiptDateTo").val(currentdate);
        $("#ReceiptNoFrom").val('');
        $("#ReceiptNoTo").val('');
        $("#CustomerCodeFrom").val('');
        $("#CustomerCodeTo").val('');
        $("#Customer").val('');
        $("#CustomerID").val('');
        $("#ChequeNo").val('');
        $("#LocationFromID").val('');
        $("#LocationToID").val('');
        $("#ChequeStatus").val('');
        $("#ChequeStatus").val('');
        $("#LocationsID").val(locationID);
    },

    cheque_report_type: function () {
        var self = cheque_status;
        var report_type = $(this).val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        self.refresh();
    },

};