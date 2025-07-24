AccountGroup = {
    init: function () {
        var self = AccountGroup;
        self.bind_events();
    },

    list: function () {
        var self = AccountGroup;
        $('#tabs-accountgroup').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "accountgroup":
                $list = $('#account-group-list');
                break;
            default:
                $list = $('#account-group-list');
        }
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/AccountGroup/GetAccountGroupListV3";
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

                       }
                   },

                   { "data": "AccountName", "className": "AccountName" },
                   { "data": "ParentAccount", "className": "ParentAccount" },
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
                        app.load_content("/Masters/AccountGroup/DetailsV3/" + Id);

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
        var self = AccountGroup;
        $(".btnSave").on("click", self.save_confirm);
        $.UIkit.autocomplete($('#account-group-parent-autocomplete'), { 'source': self.get_account_group_parent, 'minLength': 1 });
        $('#account-group-parent-autocomplete').on('selectitem.uk.autocomplete', self.set_account_group_parent);
        $("body").on('click', '.edit', self.on_edit);
        $(document).on('keydown', null, 'alt+l', self.on_close);
        $(document).on('keydown', null, 'alt+s', self.on_save);
    },
    on_close: function () {
        var self = AccountGroup;
        var location = "/Masters/AccountGroup/IndexV3";
        window.location = location;
    },

    on_save: function () {
        var self = AccountGroup;
        $(".btnSave").trigger("click");
    },

    on_edit: function () {
        var self = AccountGroup;
        var ID = $(this).closest('tr').find('.ID').val();
        app.load_content("/Masters/AccountGroup/EditV3/" + ID);
    },

    save_confirm: function () {
        var self = AccountGroup
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
        var self = AccountGroup;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/AccountGroup/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/AccountGroup/CreateV3";
                }
                else {
                    app.show_error('Failed to create');
                }
            }
        });
    },

    get_data: function () {
        var self = AccountGroup;
        var data = {};
        data.ID = $("#ID").val();
        data.AccountGroupName = $("#AccountGroupName").val();
        data.Code = $("#Code").val();
        data.ParentGroupID = $("#ParentGroupID").val();
        data.ParentGroup = $("#ParentGroup").val();
        data.IsAllowAccountsUnder = true;
        data.AccountHeadCodePrefix = $("#AccountHeadCodePrefix").val();
        return data;
    },


    get_account_group_parent: function (release) {
        $.ajax({
            url: '/Masters/AccountGroup/GetAccountGroupParentAutoComplete',
            data: {
                Hint: $('#ParentGroup').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },

    set_account_group_parent: function (event, item) {
        var self = AccountGroup;
        $("#ParentGroupID").val(item.id),
        $("#ParentGroup").val(item.value);
    },


    validate_form: function () {
        var self = AccountGroup;
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
                       { type: form.required, message: "Please enter AccountGroupName" },
                   ],
               },

               {
                   elements: "#AccountHeadCodePrefix",
                   rules: [
                       { type: form.required, message: "Please enter AccountHeadCodePrefix" },

                   ],
               },
        ]
    },
}