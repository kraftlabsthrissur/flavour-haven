AccountHead = {
    init: function () {
        var self = AccountHead;
        self.bind_events();
    },
    list: function () {
        var self = AccountHead;
        $('#tabs-accounthead').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "accounthead":
                $list = $('#account_head_list');
                break;
            default:
                $list = $('#account_head_list');
        }
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/AccountHead/GetAccountHeadListV3";
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "Type", Value: type },
                        ];
                    }
                },
                "aoColumns": [
                   {
                       "data": null,
                       "className": "uk-text-center",
                       "searchable": false,
                       "orderable": false,
                       "render": function (data, type, row, meta) {
                           return (meta.settings.oAjaxData.start + meta.row + 1)
                               + "<input type='hidden' class='ID action' value='" + row.ID + "'>"
                               + "<input type='hidden' class='CurrencyConversionRate' value='" + row.CurrencyConversionRate + "'>"
                               + "<input type='hidden' class='CurrencyID' value='" + row.CurrencyID + "'>"
                               + "<input type='hidden' class='CurrencyCode' value='" + row.CurrencyCode + "'>"
                               + "<input type='hidden' class='CurrencyName' value='" + row.CurrencyName + "'>"
                               + "<input type='hidden' class='CurrencyPrefix' value='" + row.CurrencyPrefix + "'>"
                               + "<input type='hidden' class='DecimalPlaces' value='" + row.DecimalPlaces + "'>"

                       }
                   },
                   { "data": "AccountID", "className": "AccountID" },
                   { "data": "AccountName", "className": "AccountName" },
                   { "data": "GroupName", "className": "GroupName" },
                   {
                       "data": "", "className": "action uk-text-center", "searchable": false,
                       "render": function (data, type, row, meta) {
                           return "<button class='md-btn md-btn-primary edit' >Edit</button>";
                       }
                   },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/AccountHead/DetailsV3/" + Id);

                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    
    bind_events: function () {
        var self = AccountHead;
        $.UIkit.autocomplete($('#group-name-autocomplete'), Config.get_account_group_list);
        $('#group-name-autocomplete').on('selectitem.uk.autocomplete', self.set_account_group_name);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on('click', '.edit', self.on_edit);
    },
    on_edit: function () {
        var self = AccountHead;
        var ID = $(this).closest('tr').find('.ID').val();
        app.load_content("/Masters/AccountHead/EditV3/" + ID);
    },
    get_account_id: function (AccountName) {
        var self = AccountHead;
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

    get_account_group: function (release) {
        var self = AccountHead;
        $.ajax({
            url: '/Masters/AccountGroup/GetAccountGroupParentAutoComplete',
            data: {
                Hint: $('#AccountGroupName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        })
    },
    set_account_group_name: function (event, item) {
        var self = AccountHead;
        $("#AccountGroupID").val(item.id),
        $("#AccountGroupName").val(item.value);
        if ($('#AccountGroupID').val() > 0)
        {
            self.get_account_id(item.value);
        }
    },
    save_confirm: function () {
        var self = AccountHead
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    Save: function () {
        var self = AccountHead;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/AccountHead/SaveV3',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("AccountHead created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/AccountHead/CreateV3";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var self = AccountHead;
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
        var self = AccountHead;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
               {
                   elements: "#AccountGroupName",
                   rules: [
                       { type: form.required, message: "Please Select AccountGroupName" },

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

    account_head_list: function () {
        var $list = $('#account-head-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/AccountHead/GetAccountHeadListV3",
                    "type": "POST",
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='CurrencyConversionRate' value='" + row.CurrencyConversionRate + "'>"
                                + "<input type='hidden' class='CurrencyID' value='" + row.CurrencyID + "'>"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.CurrencyCode + "'>"
                                + "<input type='hidden' class='CurrencyName' value='" + row.CurrencyName + "'>"
                                + "<input type='hidden' class='CurrencyPrefix' value='" + row.CurrencyPrefix + "'>"
                                + "<input type='hidden' class='DecimalPlaces' value='" + row.DecimalPlaces + "'>";
                        }
                    },
                    { "data": "AccountID", "className": "AccountID" },
                    { "data": "AccountName", "className": "AccountName" },
                    { "data": "GroupName", "className": "GroupName" }
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },

    account_head_list_suppliers: function () {
        var $list = $('#account-head-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/AccountHead/GetAccountHeadListForSupplier",
                    "type": "POST",
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "AccountID", "className": "AccountID" },
                    { "data": "AccountName", "className": "AccountName" },
                    { "data": "GroupName", "className": "GroupName" }
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },

    debit_account_list: function () {
        var $list = $('#debit-account-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/AccountHead/GetDebitAccountList",
                    "type": "POST",
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "AccountID", "className": "AccountID" },
                    { "data": "AccountName", "className": "AccountName" },
                    { "data": "GroupName", "className": "GroupName" }
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },

    credit_account_list: function () {
        var $list = $('#credit-account-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/AccountHead/GetCreditAccountList",
                    "type": "POST",
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "AccountID", "className": "AccountID" },
                    { "data": "AccountName", "className": "AccountName" },
                    { "data": "GroupName", "className": "GroupName" }
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },
}