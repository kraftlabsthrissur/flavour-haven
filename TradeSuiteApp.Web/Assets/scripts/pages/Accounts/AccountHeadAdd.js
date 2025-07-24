AccountHeadAdd = {
    init: function () {
        var self = AccountHeadAdd;
        self.bind_events();
    },


    bind_events: function () {
        var self=AccountHeadAdd
        $("body").on('click', '.account-head-add-modal-close', self.close_account_head_add_modal);
        $.UIkit.autocomplete($('#group-name-autocomplete'), Config.get_account_group_list);
        $('#group-name-autocomplete').on('selectitem.uk.autocomplete', self.set_account_group_name);
        $("body").on('click', '#btnSaveAccountHead', self.Save);
    },

    add_account_head: function () {
        var self = AccountHeadAdd
        $("#div-account-head").removeClass("uk-hidden");
        $.ajax({
            url: '/Masters/AccountHead/AddAccountHead',
            dataType: "html",
            data: {
            },
            type: "POST",
            success: function (response) {
                $("#div-account-head").empty();
                $response = $(response);
                app.format($response);
                $("#div-account-head").append($response);
                $('#show-add-account-head').trigger('click');
                AccountHeadAdd.init();
                $.UIkit.autocomplete($('#group-name-autocomplete'), Config.get_account_group_list);
                //$("#div-account-head").removeClass("uk-hidden")
            },
        })
    },

    close_account_head_add_modal: function () {
        var self = AccountHeadAdd
        $("#div-account-head").addClass("uk-hidden");
        $("#div-account-head").empty();
    },

    get_account_id: function (AccountName) {
        var self=AccountHeadAdd
        $.ajax({
            url: '/Masters/AccountHead/GetAccountID',
            data: {
                AccountName: AccountName
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var AccountID = response.AccountID;
                $('#AccountID').val(AccountID)
            }
        })
    },


    set_account_group_name: function (event, item) {
        var self=AccountHeadAdd
        $("#AccountGroupID").val(item.id),
        $("#AccountGroupName").val(item.value);
        if ($('#AccountGroupID').val() > 0) {
            self.get_account_id(item.value);
        }
    },

    Save: function () {
        var self=AccountHeadAdd
        var data;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Masters/AccountHead/SaveV3',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("AccountHead created successfully");
                    $("#AccountHeadID").val(data.ID);
                    $("#AccountHead").val(data.data.AccountName);
                    $("#div-account-head").addClass("uk-hidden");
                    $("#div-account-head").empty();
                    setTimeout(function () {
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $("#div-account-head").addClass("uk-hidden");
                    $("#div-account-head").empty();
                }
            },
        });
    },
    get_data: function () {
        var self=AccountHeadAdd
        var data = {};
        data.ID = $("#ID").val();
        data.AccountGroupName = $("#AccountGroupName").val();
        data.AccountGroupID = $("#AccountGroupID").val();
        data.AccountID = $("#AccountID").val();
        data.AccountName = $("#AccountName").val();
        data.OpeningAmount = clean($("#OpeningAmount").val());
        data.OpeningAmountType = $("#OpeningAmountType").val();
        return data;
    },
    validate_form: function () {
        var self=AccountHeadAdd
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
               {
                   elements: "#AccountGroupID",
                   rules: [
                       { type: form.required, message: "Please choose an AccountGroupName", alt_element: "#AccountGroupName" },
                       { type: form.positive, message: "Please choose an AccountGroupName", alt_element: "#AccountGroupName" },
                       { type: form.non_zero, message: "Please choose an AccountGroupName", alt_element: "#AccountGroupName" }
                   ],
               },
               {
                   elements: "#AccountName",
                   rules: [
                       { type: form.required, message: "Please Enter AccountName" },

                   ],
               },

               {
                   elements: "#AccountID",
                   rules: [
                       { type: form.required, message: "Please Enter AccountID" },

                   ],
               },
        ]
    },

}