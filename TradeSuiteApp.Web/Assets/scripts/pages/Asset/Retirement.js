$(function () {

});
var asset_items_list;

retirement = {
    init: function () {
        retirement.bind_events();
    },
    list: function () {
        asset_items_list = retirement.items_list();
        retirement.bind_events();
    },
    items_list: function (type) {
        var url;

        $list = $('#asset-items-list');
        url = "/Asset/Retirement/GetAssetForRetirementList";
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
                            { Key: "CapitalisationDateFrom", Value: $('#CapitalisationDateFromStr').val() },
                            { Key: "CapitalisationDateTo", Value: $('#CapitalisationDateToStr').val() },
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
        var self = retirement;
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
            window.location = '/Asset/Retirement/Edit/' + id;
        });
        $(".save").on("click", self.save);
    },
    save: function () {
        var self = retirement;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        else {
            var model = self.get_data();
            $(".save").css({ 'display': 'none' });
            $.ajax({
                url: '/Asset/Retirement/Save',
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Asset created successfully.");
                        setTimeout(function () {
                            window.location = "/Asset/Retirement/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to create asset.");
                        $(".save").css({ 'display': 'block' });
                    }
                },
            });
        }
    },
    get_data: function () {
        self = retirement;
        var model = {
            ID: clean($("#ID").val()),
            DateStr: $("#TransDate").val(),
            EndDateStr: $("#EndDate").val(),
            ClosingGrossBlockValue: clean($("#ClosingGrossBlockValue").val()),
            ClosingAccumulatedDepreciation: clean($("#ClosingAccumulatedDepreciation").val()),
            CompanyDepreciationRate: clean($("#CompanyDepreciationRate").val()),
            ClosingWDV: clean($("#ClosingWDV").val()),
            SaleQty: clean($("#SaleQty").val()),
            SaleValue: clean($("#SaleValue").val()),
            Status: "Retired"
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
        self = retirement;
        $("#AssetCodeTo").val(item.code);
        //  $("#TransNoToID").val(item.id);
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
        self = retirement;
        $("#AssetCodeFrom").val(item.code);
        //  $("#TransNoFromID").val(item.id);
    },

    get_asset_to_range: function () {
        var self = retirement;
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
        self = retirement;
        $("#AssetName").val(item.code);

    },
    filter: function () {
        $(".filter").removeClass("uk-hidden");
    },
    reset: function () {
        self = retirement;

        $("#AssetCodeFrom").val('');
        $("#AssetCodeTo").val('');
        $("#FromAssetRange").val('');
        $("#ToAssetRange").val('');
        $("#AssetName").val('');
    },
    validate_form: function () {
        var self = retirement;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [
             {
                 elements: "#EndDate",
                 rules: [
                        { type: form.required, message: "Please enter end date" },
                 ]
             },
             {
                 elements: "#SaleQty",
                 rules: [
                  { type: form.positive, message: "Invalid Sale Quantity" },
                  { type: form.non_zero, message: "Invalid Sale Quantity" },
                  {
                      type: function (element) {
                          var error = false;
                          var sale_qty = clean($('#SaleQty').val());
                          var asset_qty = clean($('#AssetQty').val());
                          if (sale_qty > asset_qty)
                              error = true;
                          return !error;
                      }, message: 'You dont have enough asset to sell'
                  },
                 ]
             },
             {
                 elements: "#SaleValue",
                 rules: [
                  { type: form.positive, message: "Invalid Sale Value" },
                  { type: form.non_zero, message: "Invalid Sale Value" },

                 ]
             },
        ],

    },
}