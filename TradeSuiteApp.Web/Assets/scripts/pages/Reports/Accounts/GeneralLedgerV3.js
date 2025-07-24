$(function () {
    GeneralLedger.init();
});

GeneralLedger = {
    init: function () {
        var self = GeneralLedger;
        self.bind_events();
        ReportHelper.init();
        //self.get_report_type();
    },

    bind_events: function () {
        var self = GeneralLedger;
        $('#Refresh').on('click', self.refresh);
        $.UIkit.autocomplete($('#ledgeraccountname-autocomplete'), { 'source': self.get_ledger_account_name, 'minLength': 1 });
        $('#ledgeraccountname-autocomplete').on('selectitem.uk.autocomplete', self.set_ledger_account_name);
        $('.LedgerType').on('ifChanged', self.show_ledger_type);
        $.UIkit.autocomplete($('#account-group-parent-autocomplete'), { 'source': self.get_account_group_parent, 'minLength': 1 });
        $('#account-group-parent-autocomplete').on('selectitem.uk.autocomplete', self.set_account_group_parent);
    },

    get_ledger_account_name: function (release) {
        var Hint = $('#AccountName').val();
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadNameAutoComplete',
            data: {
                Hint: Hint,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_ledger_account_name: function (event, item) {
        var self = GeneralLedger;
        $("#AccountCodeFrom").val(item.accountid);
        $("#AccountCodeFromID").val(item.id);
        $("#AccountName").val(item.accountname);
        $("#AccountNameID").val(item.id);
    },

    show_ledger_type: function () {
        self = GeneralLedger;
        var Type = $(".LedgerType:checked").val();
        if (Type == "AccountGroup") {
            $(".ac-name").addClass("uk-hidden");
            $(".ac-group").removeClass("uk-hidden");
        } else if (Type == "AccountName") {
            $(".ac-group").addClass("uk-hidden");
            $(".ac-name").removeClass("uk-hidden");
        }
        $('#AccountGroup').val('');
        $('#AccountGroupID').val('');
        $('#AccountName').val('');
        $('#AccountNameID').val('');
    },
    get_account_group_parent: function (release) {
        $.ajax({
            url: '/Masters/AccountGroup/GetAccountGroupParentAutoComplete',
            data: {
                Hint: $('#AccountGroup').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },
    set_account_group_parent: function (event, item) {
        var self = GeneralLedger;
        $("#AccountGroupID").val(item.id),
        $("#AccountGroup").val(item.value);
    },

    get_filters: function () {
        var self = GeneralLedger;
        var filters = "";

        //if ($("#TransactionNo").val() != "") {
        //    filters += "TransactionNo: " + $("#TransactionNo").val() + " , ";
        //}

        if ($("#AccountNameID").val() != 0) {
            filters += "Account Name: " + $("#AccountName").val() + ", ";
        }
        if ($("#AccountGroupID").val() != 0) {
            filters += "Account Group: " + $("#AccountGroup").val() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "" + filters;
        }
        return filters;
    },
    refresh: function () {
        var self = GeneralLedger;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $('#FromDateString').val(findate);
        $('#ToDateString').val(currentdate);
        $('#AccountGroup').val('');
        $('#AccountGroupID').val('');
        $('#AccountName').val('');
        $('#AccountNameID').val('');
    },

    validate_form: function () {
        var self = GeneralLedger;
        self.error_count = 0;
        if (self.error_count > 0) {
            return;
        }
        if (self.rules.on_show.length) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },

    rules: {
        on_show: [
             {
                 elements: "#AccountName:visible",
                 rules: [
                     { type: form.required, message: "Please Select AccountName" },
                 ]
             },
        ]
    }
}