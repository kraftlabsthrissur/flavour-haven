$(function () {

});
var pending_asset_items_list;
var capital_asset_items_list;
var revenue_asset_items_list;
asset = {
    init: function () {
        asset.bind_events();
    },
    list: function () {
        pending_asset_items_list = asset.items_list("pending");
        capital_asset_items_list = asset.asset_items_list("capital");
        revenue_asset_items_list = asset.asset_items_list("revenue");
        asset.bind_events();
    },
    items_list: function (type) {
        var url;
        var acc_category_range_from;
        var acc_category_range_to;
      
            $list = $('#pending-asset-items-list');
            url = "/Asset/Asset/GetPendingList";
      
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
                            { Key: "TransNoFromID", Value: clean($('#TransNoFromID').val()) },
                            { Key: "TransNoToID", Value: clean($('#TransNoToID').val()) },
                            { Key: "TransDateFrom", Value: $('#TransDateFromStr').val() },
                            { Key: "TransDateTo", Value: $('#TransDateToStr').val() },
                            { Key: "ReceiptNoFromID", Value: clean($('#ReceiptNoFromID').val()) },
                            { Key: "ReceiptNoToID", Value: clean($('#ReceiptNoToID').val()) },
                            { Key: "AssetNameFrom", Value: $('#FromAssetRange').val() },
                            { Key: "AssetNameTo", Value: $('#ToAssetRange').val() },
                            { Key: "AssetName", Value: $('#AssetName').val() },
                            { Key: "AccountCategoryFrom", Value: $('#FromAccountCategoryRange').val() },
                            { Key: "AccountCategoryTo", Value: $('#ToAccountCategoryRange').val() },
                            { Key: "AccountCategoryID", Value: clean($('#AccountCategory').val()) },
                            { Key: "SupplierNameFrom", Value: $('#FromSupplierNameRange').val() },
                            { Key: "SupplierNameTo", Value: $('#ToSupplierNameRange').val() },
                            { Key: "SupplierID", Value: clean($('#SupplierID').val()) },
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
    asset_items_list: function (type) {
        var url;
        var acc_category_range_from;
        var acc_category_range_to;
        if (type == "pending") {
            $list = $('#pending-asset-items-list');
            url = "/Asset/Asset/GetPendingList";
        } else if (type == "revenue") {
            $list = $('#revenue-asset-items-list');
            url = "/Asset/Asset/GetRevenueList";
        } else if (type == "capital") {
            $list = $('#capital-asset-items-list');
            url = "/Asset/Asset/GetCapitalList";
        }
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
                            { Key: "TransNoFromID", Value: clean($('#TransNoFromID').val()) },
                            { Key: "TransNoToID", Value: clean($('#TransNoToID').val()) },
                            { Key: "TransDateFrom", Value: $('#TransDateFromStr').val() },
                            { Key: "TransDateTo", Value: $('#TransDateToStr').val() },
                            { Key: "ReceiptNoFromID", Value: clean($('#ReceiptNoFromID').val()) },
                            { Key: "ReceiptNoToID", Value: clean($('#ReceiptNoToID').val()) },
                            { Key: "AssetNameFrom", Value: $('#FromAssetRange').val() },
                            { Key: "AssetNameTo", Value: $('#ToAssetRange').val() },
                            { Key: "AssetName", Value: $('#AssetName').val() },
                            { Key: "AccountCategoryFrom", Value: $('#FromAccountCategoryRange').val() },
                            { Key: "AccountCategoryTo", Value: $('#ToAccountCategoryRange').val() },
                            { Key: "AccountCategoryID", Value: clean($('#AccountCategory').val()) },
                            { Key: "SupplierNameFrom", Value: $('#FromSupplierNameRange').val() },
                            { Key: "SupplierNameTo", Value: $('#ToSupplierNameRange').val() },
                            { Key: "SupplierID", Value: clean($('#SupplierID').val()) },
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
        var self = asset;
        $("#FromAssetRange").on("change", self.get_asset_to_range);
        $("#FromAccountCategoryRange").on("change", self.get_category_to_range);
        $("#FromSupplierNameRange").on("change", self.get_supplier_to_range);
        $.UIkit.autocomplete($('#receiptno-from-autocomplete'), { 'source': self.get_grnno, 'minLength': 1 });
        $('#receiptno-from-autocomplete').on('selectitem.uk.autocomplete', self.set_grnno);
        $.UIkit.autocomplete($('#receiptno-to-autocomplete'), { 'source': self.get_grnnoto, 'minLength': 1 });
        $('#receiptno-to-autocomplete').on('selectitem.uk.autocomplete', self.set_grnnoo);
        $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#SupplierName-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $.UIkit.autocomplete($('#asset-name-autocomplete'), { 'source': self.get_asset_name, 'minLength': 1 });
        $('#asset-name-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_name);
        $.UIkit.autocomplete($('#transno-from-autocomplete'), { 'source': self.get_asset_transno, 'minLength': 1 });
        $('#transno-from-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_transno);
        $("#Status").on("change", self.get_asset_no);
        //  $("#StatusDetail").on("change", self.get_asset_no_detail);

        $.UIkit.autocomplete($('#transno-to-autocomplete'), { 'source': self.get_asset_transno_to, 'minLength': 1 });
        $('#transno-to-autocomplete').on('selectitem.uk.autocomplete', self.set_asset_transno_to);
        $('body').on("click", '#pending-asset-items-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Asset/Asset/Edit/' + id;
        });
        $('body').on("click", '#capital-asset-items-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Asset/Asset/Details/' + id;
        });
        $('body').on("click", '#revenue-asset-items-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Asset/Asset/Details/' + id;
        });
        $(".btnSave").on("click", self.save);
        $(".btnChangeStatus").on("click", self.change_status);
        $(".btnFilter").on("click", self.filter);
        $("#btnReset").on("click", self.reset);
        $("#AssetUniqueNo").on("change", self.get_asset_unique_number);

    },
    get_asset_unique_number: function (release) {

        $.ajax({
            url: '/Asset/Asset/GetAssetUniqueNumber',
            data: {
                Hint: $("#AssetUniqueNo").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                count = response.data;
                $("#AssetUniqueNoCount").val(count);
            }
        });
    },
    reset: function () {
        self = asset;
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var currentdate = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();
        var findate = '1-4-' + d.getFullYear();
        $("#TransDateToStr").val(currentdate);
        $("#TransDateFromStr").val(findate);
        $("#TransNoFrom").val('');
        $("#TransNoFromID").val('');
        $("#TransNoTo").val('');
        $("#TransNoToID").val('');
        $("#ReceiptNoFrom").val('');
        $("#ReceiptNoFromID").val('');
        $("#ReceiptNoTo").val('');
        $("#ReceiptNoToID").val('');
        $("#FromAssetRange").val('');
        $("#ToAssetRange").val('');
        $("#AssetName").val('');
        $("#FromAccountCategoryRange").val('');
        $("#ToAccountCategoryRange").val('');
        $("#AccountCategory").val('');
        $("#FromSupplierNameRange").val('');
        $("#ToSupplierNameRange").val('');
        $("#SupplierID").val('');
        $("#SupplierName").val('');

    },
    filter: function () {
        $(".filter").removeClass("uk-hidden");
    },
    change_status: function () {
        var self = asset;
        self.error_count = 0;
        self.error_count = self.validate_status();
        if (self.error_count > 0) {
            return;
        }
        else {
            var model = self.get_data(false);
            $(".btnChangeStatus").css({ 'display': 'none' });
            $.ajax({
                url: '/Asset/Asset/ChangeStatus',
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Asset status change successfully.");
                        setTimeout(function () {
                            window.location = "/Asset/Asset/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to change asset status.");
                        $(".btnChangeStatus").css({ 'display': 'block' });
                    }
                },
            });
        }
    },
    save: function () {
        var self = asset;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        else {
            var model = self.get_data(false);
            $(".btnSave").css({ 'display': 'none' });
            $.ajax({
                url: '/Asset/Asset/Save',
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Asset created successfully.");
                        setTimeout(function () {
                            window.location = "/Asset/Asset/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to create asset.");
                        $(".btnSave").css({ 'display': 'block' });
                    }
                },
            });
        }
    },
    get_data: function (IsDraft) {
        self = asset;
        var model = {
            ID: clean($("#ID").val()),
            TransDateStr: $("#TransDate").val(),
            AssetName: $("#AssetName").val(),
            IsRepairable: $(".isrepairable:checked").val(),
            CompanyDepreciationRate: clean($("#CompanyDepreciationRate").val()),
            IncomeTaxDepreciationRate: clean($("#IncomeTaxDepreciationRate").val()),
            LifeInYears: clean($("#LifeInYears").val()),
            ResidualValue: clean($("#ResidualValue").val()),
            Status: $("#Status").val(),
            IsDraft: IsDraft,
            AdditionToAssetNo: $("#AdditionToAssetNo").val(),
            StatusChangeDateStr: $("#StatusChangeDateStr").val(),
            Remark: $("#Remark").val(),
            AssetUniqueNo: $("#AssetUniqueNo").val(),
            IsCapital: $("#IsCapital").val(),
            AssetCode: $("#AssetNumber").val()
        };
        return model;
    },
    get_asset_no_detail: function (release) {
        var assetno;
        if ($("#Status").val() != "Capital") {
            if ($("#StatusDetail").val() == "Capital") {
                $.ajax({
                    url: '/Asset/Asset/GetAssetNumber',
                    data: {
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        assetno = response.data[0];
                        $("#AssetNumber").val(assetno);
                    }
                });
            }
            else {
                $("#AssetNumber").val('');
            }
        }


    },

    get_asset_no: function (release) {
        var assetno;
        if ($("#IsCapital").val() == "False") {
            if ($("#Status").val() == "Capital") {
                //  $(".assetno").removeClass("uk-hidden");
                $.ajax({
                    url: '/Asset/Asset/GetAssetNumber',
                    data: {
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        assetno = response.data[0];
                        $("#AssetNumber").val(assetno);
                    }
                });
            }
            else {

                $("#AssetNumber").val('');
            }
        }
    },
    get_asset_transno_to: function (release) {
        var Table;
        Table = 'AssetTransNo';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#TransNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_asset_transno_to: function (event, item) {
        self = asset;
        $("#TransNoTo").val(item.code);
        $("#TransNoToID").val(item.id);
    },

    get_asset_transno: function (release) {
        var Table;
        Table = 'AssetTransNo';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#TransNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_asset_transno: function (event, item) {
        self = asset;
        $("#TransNoFrom").val(item.code);
        $("#TransNoFromID").val(item.id);
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
        self = asset;
        $("#AssetName").val(item.code);
        $("#AssetNameID").val(item.id);


    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
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

    set_supplier_details: function (event, item) {   // on select auto complete item
        self = asset;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },

    get_grnno: function (release) {
        var Table;
        Table = 'ServiceReceiptNote';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
    set_grnno: function (event, item) {
        self = asset;
        $("#ReceiptNoFrom").val(item.code);
        $("#ReceiptNoFromID").val(item.id);
    },
    get_grnnoto: function (release) {
        var Table;
        Table = 'ServiceReceiptNote';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
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
    set_grnnoTo: function (event, item) {
        self = asset;
        $("#ReceiptNoTo").val(item.code);
        $("#ReceiptNoToID").val(item.id);
    },
    get_asset_to_range: function () {
        var self = asset;
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

    get_category_to_range: function () {
        var self = asset;
        var from_range = $("#FromAccountCategoryRange").val();
        $.ajax({
            url: '/Asset/Asset/GetCategoryRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToAccountCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToAccountCategoryRange").append(html);
            }
        });
    },
    get_supplier_to_range() {
        var self = asset;
        var from_range = $("#FromSupplierNameRange").val();
        $.ajax({
            url: '/Asset/Asset/GetSupplierRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierNameRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierNameRange").append(html);
            }
        })
    },
    validate_form: function () {
        var self = asset;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_status: function () {
        var self = asset;
        if (self.rules.on_status.length) {
            return form.validate(self.rules.on_status);
        }
        return 0;
    },
    rules: {
        on_submit: [
             {
                 elements: "#Status",
                 rules: [
                        { type: form.required, message: "Please select asset status" },
                 ]
             },
             {
                 elements: "#AssetName",
                 rules: [
                        { type: form.required, message: "Please enter asset name" },
                 ]
             },

             {
                 elements: "#AssetUniqueNo",
                 rules: [
                        { type: form.required, message: "Please enter asset unique number" },
                         {
                             type: function (element) {
                                 var error = false;
                                 var count = clean($('#AssetUniqueNoCount').val());
                                 if (count > 0)
                                     error = true;


                                 return !error;
                             }, message: 'Asset unique number must be unique'
                         },
                 ]
             },
             {
                 elements: "#CompanyDepreciationRate",
                 rules: [
                  { type: form.positive, message: "Invalid company depreciation rate" },
                  { type: form.non_zero, message: "Invalid company depreciation rate" },
                     {
                         type: function (element) {
                             var error = false;
                             var rate = clean($('#CompanyDepreciationRate').val());
                             if (rate > 100)
                                 error = true;


                             return !error;
                         }, message: 'Invalid company depreciation rate'
                     },

                 ]
             },
             {
                 elements: "#IncomeTaxDepreciationRate",
                 rules: [
                  { type: form.positive, message: "Invalid income tax repreciation rate" },
                  { type: form.non_zero, message: "Invalid income tax repreciation rate" },
                     {
                         type: function (element) {
                             var error = false;
                             var rate = clean($('#IncomeTaxDepreciationRate').val());
                             if (rate > 100)
                                 error = true;


                             return !error;
                         }, message: 'Invalid income tax depreciation rate'
                     },

                 ]
             },
             {
                 elements: "#LifeInYears",
                 rules: [
                  { type: form.positive, message: "Invalid life in years" },
                  { type: form.non_zero, message: "Invalid life in years" },

                 ]
             },
             {
                 elements: "#ResidualValue",
                 rules: [
                  { type: form.positive, message: "Invalid residual value" },
                  { type: form.non_zero, message: "Invalid residual value" },
                       {
                           type: function (element) {
                               var error = false;
                               var residual_value = clean($('#ResidualValue').val());
                               var asset_value = clean($('#AssetValue').val());
                               if (residual_value > asset_value)
                                   error = true;


                               return !error;
                           }, message: 'Residual value must be less than asset value'
                       },


                 ]
             },
        ],
        on_status: [
           {
               elements: "#Status",
               rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var currentstatus = $('#StatusDetail').val();
                               var status = $('#Status').val();
                               if (status == currentstatus)
                                   error = true;

                               return !error;
                           }, message: 'Can not change to same status '
                       },
               ]
           },

        ],
    },
}