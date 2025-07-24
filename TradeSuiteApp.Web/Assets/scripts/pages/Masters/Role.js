Role = {
    init: function () {
        Role.bind_events();
        Role.Role_list();

        var RoleID = $("#ID").val();
        if (RoleID > 0) {
            Role.get_actionid(RoleID);
        }


    },
    ActionsID: [],
    TabsID: [],
    get_actionid: function (RoleID) {
        var self = Role;
        $.ajax({
            url: '/Masters/Role/GetRoleActions?RoleID=' + RoleID,
            dataType: "json",
            type: "POST",
            success: function (response) {
                $(response.ActionID).each(function (i, record) {
                    self.ActionsID.push(record.ActionID);
                    self.TabsID.push(record.TabID);
                });
                
            }
        });

    },

    Role_list: function () {
        var $list = $('#role-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/Role/GetRoleList"

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
                   { "data": "Code", "className": "Code" },
                   { "data": "RoleName", "className": "RoleName" },
                   { "data": "Remarks", "className": "Remarks" },
                     {
                         "data": "Actions", "searchable": false, "className": "Actions",
                         "render": function (data, type, row, meta) {
                             return "<div>" + row.Controller + "-" + row.Actions + "</div>";
                         }
                     },
                   { "data": "Tabs", "className": "Tabs" },


                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/Role/Details/" + Id);
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
        var self = Role;
        $("body").on('click', '.btnSave,.btnSaveNew', self.save_confirm);
        $("body").on("ifChanged", ".action-head", self.check);
        $("body").on("ifChanged", ".tab-head", self.check_tab);
        $("body").on('click', '.areas.not-opened', self.tab_loaded);
        $("body").on("ifChanged", ".actions", self.check_actions);
        $("body").on("ifChanged", ".tab", self.check_tabs);
    },

    save_confirm: function () {
        var self = Role;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    check: function () {
        var self = Role;
        var group = $(this).closest(".action-group");
        var actions = $(group).find('.actions');
        var element = this;
        $.each(actions, function () {
            if ($(this).is(":checked") != $(element).is(":checked")) {
                $(this).next('.iCheck-helper').trigger("click");
               
            }
        });
    },

    check_tab: function () {
        var self = Role;
        var group = $(this).closest(".tab-group");
        var actions = $(group).find('.tab');
        var element = this;
        $.each(actions, function () {
            if ($(this).is(":checked") != $(element).is(":checked")) {
                $(this).next('.iCheck-helper').trigger("click");
            }
        });
    },

    save: function () {
        var self = Role;
        var location = "/Masters/Role/Index";
        if ($(this).hasClass('btnSaveNew')) {
            location = "/Masters/Role/Create";
        }
        var data = self.get_data();
        $.ajax({
            url: '/Masters/Role/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                }
            }
        });

    },

    get_data: function () {
        var self = Role;
        var data = {};
        data.ID = $("#ID").val();
        data.Code = $("#Code").val();
        data.Remarks = $("#Remarks").val();
        data.RoleName = $("#RoleName").val();
        data.ActionID = [];
        data.ActionID = self.ActionsID;

        data.TabID = [];
        data.TabID = self.TabsID;

        return data;
    },

    validate_form: function () {
        var self = Role;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please enter code" },
                ],
            },
            {
                elements: "#RoleName",
                rules: [
                    { type: form.required, message: "Please enter RoleName" },
                ],
            },
            {
                elements: "#Remarks",
                rules: [
                    { type: form.required, message: "Please enter Remarks" },
                ],
            },

        ]
    },

    tab_loaded: function () {
        var self = Role;
        var element = $(this);
        var Area = $(this).text();
        var RoleID = $("#ID").val();
        if (RoleID == undefined)
        {
            RoleID = 0;
        }
        $.ajax({
            url: '/Masters/Role/Actions?Area=' + Area + "&RoleID=" + RoleID,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $(element).removeClass("not-opened");
                app.format($("#" + Area).html(response));
            }
        });
    },

    check_actions: function () {
        var self = Role;
        var group = $(this).closest(".action-group");
        var actions = $(this).closest('.actions');
            if ($(actions).is(":checked") == true) {
                var ID = clean($(this).val());
                self.ActionsID.push(ID);
            }
            else {
                var ID = clean($(this).val());
                var index = self.ActionsID.indexOf(ID)
                self.ActionsID.splice(index,1);
            }
    },

    check_tabs: function () {
        var self = Role;

        var group = $(this).closest(".tab-group");
        var actions = $(this).closest('.tab');
        if ($(actions).is(":checked") == true) {
            var ID = clean($(this).val());
            self.TabsID.push(ID);
        }
        else {
            var ID = clean($(this).val());
            var index = self.TabsID.indexOf(ID)
            self.TabsID.splice(index, 1);
        }
    }



        
        
}
