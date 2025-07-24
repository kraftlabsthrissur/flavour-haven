Actions = {
    init: function () {
        var self = Actions;
        self.list();
        self.bind_events();
        self.hide_show_btn();
    },

    list: function () {
        var self = Actions;
        self.tabbed_list("enabled");
        self.tabbed_list("not-enabled");
        self.tabbed_list("all");
    },

    tabbed_list: function (type) {
        // types
        //  1. enabled
        //  2. not-enabled.
        //  3. all.

        var $list;

        switch (type) {
            case "enabled":
                $list = $('#enable-list');
                break;
            case "not-enabled":
                $list = $('#not-enable-list');
                break;
            case "all":
                $list = $('#all-list');
                break;
            default:
                $list = $('#enable-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Admin/App/GetActionList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "asc"]],
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='checkbox' data-md-icheck class='md-input check-box'/>"
                        }
                    },

                   { "data": "Name", "className": "Name" },
                   { "data": "Area", "className": "Area" },
                   { "data": "Controller", "className": "Controller" },
                   { "data": "Action", "className": "Action" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = Actions;
        $(".Enabled ").on("click", self.hide_show_btn);
        $(".NotEnabled ").on("click", self.hide_show_not_enable_btn);
        $(".All ").on("click", self.hide_show_all_btn);
        $("body").on("ifChanged", ".check-box", self.check);
        $("body").on("click", ".btnDisable,.btnEnable", self.enable_item);
    },

    enable_item: function () {
        var self = Actions;
        data = self.get_data();
        $.ajax({
            url: '/Admin/App/EnableItem',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Successfully enabled.");
                    window.location = "/Admin/App/Index";
                } else {
                    app.show_error("Failed to enable.");
                }
            },
        });
    },

    get_data: function () {
        var self = Actions;
        var data = {};
        data.enable = [];
        var items = {}; 
        if ($(event.target).hasClass("btnDisable"))
        {
            $("#enable-list tbody .included").each(function () {
                items = {};
                items.Action = $(this).find(".Action").text();
                items.Controller = $(this).find(".Controller").text();
                items.ID = $(this).find(".ID").val();
                data.enable.push(items);
            });
        }
        else
        {
            $("#not-enable-list tbody .included").each(function () {
                items = {};
                items.Action = $(this).find(".Action").text();
                items.Controller = $(this).find(".Controller").text();
                items.ID = $(this).find(".ID").val();
                data.enable.push(items);
            });

        }
        return data;
    },

    hide_show_btn: function () {
        var self = Actions;
        $(".btnEnable").hide();
        $(".btnDisable").show();
    },

    hide_show_not_enable_btn: function () {
        var self = Actions;
        $(".btnEnable").show();
        $(".btnDisable").hide();
    },

    hide_show_all_btn: function () {
        var self = Actions;
        $(".btnEnable").hide();
        $(".btnDisable").hide();
    },

    check: function () {
        var self = Actions;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
        } else {
            $(row).removeClass('included');
        }
    },


}