$(function () {

});
var asset_items_list;

correction = {
    init: function () {
        correction.bind_events();
    },
    list: function () {
        asset_items_list = correction.items_list();
        correction.bind_events();
    },
    items_list: function (type) {
        var url;

        $list = $('#asset-items-list');
        url = "/Asset/AssetCorrection/GetCapitalListForCorrection";
        if ($list.length) {

            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "AssetCodeFrom", Value: $('#AssetCodeFrom').val() },
                            { Key: "AssetCodeTo", Value: $('#AssetCodeTo').val() },
                            { Key: "AssetNameFrom", Value: $('#FromAssetRange').val() },
                            { Key: "AssetNameTo", Value: $('#ToAssetRange').val() },
                            { Key: "AssetName", Value: $('#AssetName').val() },
                        ];
                    }
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo", },
                    { "data": "AssetCode", "className": "AssetCode", },
                    { "data": "AssetName", "className": "AssetName", },
                    { "data": "ItemName", "className": "ItemName", },
                    { "data": "SupplierName", "className": "SupplierName", },
                    {
                        "data": "GrossBlockAssetValue", "className": "GrossBlockAssetValue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.GrossBlockAssetValue + "</div>";
                        }
                    },
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                }
            });
            $('#btnSubmit').on("click", function () {
                $(".filter").addClass("uk-hidden");
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    bind_events: function () {
        var self = correction;
        $(".btnFilter").on("click", self.filter);
        $("#btnReset").on("click", self.reset);
        $.UIkit.autocomplete($('#asset-name-autocomplete'), { 'source': self.get_asset_name, 'minLength': 1 });
        $('#asset-name-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_name);
        $("#FromAssetRange").on("change", self.get_asset_to_range);
        $.UIkit.autocomplete($('#code-from-autocomplete'), { 'source': self.get_asset_code, 'minLength': 1 });
        $('#code-from-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_code);
        $.UIkit.autocomplete($('#code-to-autocomplete'), { 'source': self.get_asset_code_to, 'minLength': 1 });
        $('#code-to-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_code_to);
        $('body').on("click", '#asset-items-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Asset/AssetCorrection/Edit/' + id;
        });
        $(".btnSave").on("click", self.save_confirm);
        $.UIkit.autocomplete($('#creditaccountname-autocomplete'), { 'source': self.get_credit_account, 'minLength': 1 });
        $('#creditaccountname-autocomplete').on('selectitem.uk.autocomplete', self.set_credit_account);

        $.UIkit.autocomplete($('#debitaccountname-autocomplete'), { 'source': self.get_debit_account, 'minLength': 1 });
        $('#debitaccountname-autocomplete').on('selectitem.uk.autocomplete', self.set_debit_account);

        $.UIkit.autocomplete($('#creditaccountnumber-autocomplete'), { 'source': self.get_credit_account, 'minLength': 1 });
        $('#creditaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_credit_account);

        $.UIkit.autocomplete($('#debitaccountnumber-autocomplete'), { 'source': self.get_debit_account, 'minLength': 1 });
        $('#debitaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_debit_account);

        $("#DebitACName,#DebitACCode,#CreditACName,#CreditACCode").on("click", self.focus)

    },
    focus:function()
    {
        this.select();
        
    },

    save_confirm: function () {
        var self = correction;
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_credit_account: function (release) {
           $.ajax({
            url: '/Accounts/Journal/GetCreditAccountAutoComplete',
            data: {
                CreditAccountName: $('#CreditACName').val(),
                CreditAccountCode: $('#CreditACCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_credit_account: function (event, item) {
        $("#CreditACID").val(item.id);
        $("#CreditACCode").val(item.number);
        $("#CreditACName").val(item.name);
    },
    get_debit_account: function (release) {
        $.ajax({
            url: '/Accounts/Journal/GetDebitAccountAutoComplete',
            data: {
                DebitAccountName: $('#DebitACName').val(),
                DebitAccountCode: $('#DebitACCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_debit_account: function (event, item) {
        $("#DebitACID").val(item.id);
        $("#DebitACCode").val(item.number);
        $("#DebitACName").val(item.name);
    },
    save: function () {
        var self = correction;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        else {
            var model = self.get_data();
            $(".btnSave").css({ 'display': 'none' });
            $.ajax({
                url: '/Asset/AssetCorrection/Save',
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Asset corrected successfully.");
                        setTimeout(function () {
                            window.location = "/Asset/AssetCorrection/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to correct asset.");
                        $(".btnSave").css({ 'display': 'block' });
                    }
                },
            });
        }
    },
    get_data: function () {
        self = correction;

        var model = {
            ID: clean($("#ID").val()),
            CorrectionDateStr: $("#CorrectionDate").val(),
            AmountValue: clean($("#AmountValue").val()),
            DebitACID: $("#DebitACID").val(),           
            CreditACID: $("#CreditACID").val(),           
            IsAdditionDuringYear: ($(".IsAdditionDuringYear:checked").val() == 1 ? true : false),
            IsDepreciation: ($(".IsDepreciation:checked").val() == 1 ? true : false),
            Remark: $("#Remark").val(),

        };
        return model;
    },

    get_asset_code_to: function (release) {
        var Table;
        Table = 'AssetCode';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#AssetCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_asset_code_to: function (event, item) {
        self = correction;
        $("#AssetCodeTo").val(item.code);

    },
    get_asset_code: function (release) {
        var Table;
        Table = 'AssetCode';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#AssetCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_asset_code: function (event, item) {
        self = correction;
        $("#AssetCodeFrom").val(item.code);

    },

    get_asset_to_range: function () {
        var self = correction;
        var from_range = $("#FromAssetRange").val();
        $.ajax({
            url: '/Asset/Asset/GetAssetRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToAssetRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToAssetRange").append(html);
            }
        });
    },

    get_asset_name: function (release) {
        var Table;
        Table = 'AssetName';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#AssetName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_asset_name: function (event, item) {
        self = correction;
        $("#AssetName").val(item.code);

    },
    filter: function () {
        $(".filter").removeClass("uk-hidden");
    },
    reset: function () {
        self = correction;
        $("#AssetCodeFrom").val('');
        $("#AssetCodeTo").val('');
        $("#FromAssetRange").val('');
        $("#ToAssetRange").val('');
        $("#AssetName").val('');
    },
    validate_form: function () {
        var self = correction;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [


       {
           elements: "#AmountValue",
           rules: [
                  { type: form.required, message: "Please enter amount value" },
                   {
                       type: function (element) {
                           var error = false;
                           var amount_value = clean($('#AmountValue').val());
                           var asset_value = clean($('#AssetValue').val());
                           if (amount_value > asset_value)
                               error = true;


                           return !error;
                       }, message: 'You dont have enough asset value'
                   },
                  { type: form.positive, message: "Invalid amount value" },

           ]
       },
       {
            elements: "#DebitACID",
            rules: [
                   { type: form.required, message: "Please enter debit account details" },
            ]
        },
       {
                  elements: "#CreditACID",
                  rules: [
                         { type: form.required, message: "Please enter credit account details" },
                  ]
        },

        ],
    },
}