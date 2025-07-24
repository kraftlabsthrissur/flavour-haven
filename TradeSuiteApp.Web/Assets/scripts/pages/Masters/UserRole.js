
UserRole = {

    init: function () {
        var self = UserRole;
       
        roles_list = self.role_list();
        employee_list = Employee.employee_list('user-role');
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        item_select_role = $('#role-list').SelectTable({
            selectFunction: self.select_role,
            returnFocus: "#ItemName",
            modal: "#select-role",
            initiatingElement: "#Name"
        });
        self.bind_events();
        self.freeze_headers();
    },
    details: function () {
        var self = UserRole;
        self.freeze_headers();
    },
    list: function () {
        $list = $('#UserRole');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            $('#UserRole tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .UserID").val();
                window.location = '/Masters/UserRoles/Details/' + Id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    select_role: function () {
        var self = UserRole;
        var radio = $('#select-role tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#RoleName").val(Name);
        $("#RoleID").val(ID);
        UIkit.modal($('#select-role')).hide();

    },

    select_employee: function () {
        var self = UserRole;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var UserID = $(row).find(".UserID").val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var place = $(row).find(".Location").text().trim();
        $("#UserName").val(Name);
        $("#UserID").val(UserID);
        $("#Code").val(Code);
        $("#Place").val(place);
        UIkit.modal($('#select-employee')).hide();
        self.on_user_change(UserID);
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#UserName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_item: function (event, item) {
        var self = UserRole;
        console.log(item)
        $("#UserID").val(item.id),
        $("#UserName").val(item.name);
        var userid = item.id;
        self.on_user_change(userid);
    },

    on_user_change: function (ID)
    {
        var self = UserRole;
        self.IsUserhaveRoles(ID)
        $("#user-role-list tbody tr").remove();
    },

    IsUserhaveRoles: function (Id) {
        var self = UserRole;
        $.ajax({
            url: '/Masters/UserRoles/IsUserhaveRoles',
            data: {
                UserID: Id
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                if (result.Data == 1) {
                    app.confirm_cancel("User have already Roles.Do you want to Edit", function () {
                        window.location = '/Masters/UserRoles/Edit/' + Id;
                    }, function () {
                        $("#UserID").val('');
                        $("#UserName").val('');
                    })
                }
              
            }
        });
    },

    get_roles: function (release) {
        $.ajax({
            url: '/Masters/UserRoles/GetRolesForAutoComplete',
            data: {
                Hint: $('#RoleName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_roles: function (event, item) {
        var self = UserRole;
        console.log(item)
        $("#RoleName").val(item.name),
        $("#RoleID").val(item.id);
    },

    freeze_headers: function () {
        fh_items = $("#user-role-list").FreezeHeader();
    },

    bind_events: function () {
        var self = UserRole;
        $(".btnSave").on("click", self.save_confirm);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $.UIkit.autocomplete($('#role-autocomplete'), { 'source': self.get_roles, 'minLength': 1 });
        $('#role-autocomplete').on('selectitem.uk.autocomplete', self.set_roles);
        $("#btnOKEmployee").on("click", self.select_employee);
        $("#btnOKrole").on("click", self.select_role);
        $("body").on("click", "#btnAddItem", self.add_item);
        $("body").on("click", ".remove-item", self.delete_item);
    },

    add_item: function () {
        var self = UserRole;
        if (self.validate_role() > 0) {
            return;
        }
        var UserID = clean($("#UserID").val());
        var UserName = $("#UserName").val();
        var RoleID = clean($("#RoleID").val());
        var RoleName = $("#RoleName").val();
        var Location = $("#LocationID Option:selected").text();
        var LocationID = $("#LocationID Option:selected").val();
        var content = "";
        var $content;
        var sino = "";
        sino = $('#user-role-list tbody tr').length + 1;
        content = '<tr>'
            + '<td class="serial-no">' + sino + '</td>'
            + '<td class="UserName">' + UserName
            + '<input type="hidden" class = "UserID" value="' + UserID + '" />'
            + '<input type="hidden" class = "RoleID" value="' + RoleID + '" />'
            + '<input type="hidden" class = "LocationID" value="' + LocationID + '" />'
            + '</td>'
            + '<td class="RoleName">' + RoleName + '</td>'
            + '<td class="Location">' + Location + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#user-role-list tbody').append($content);
        index = $("#user-role-list tbody tr").length;
        $("#item-count").val(index);
        self.clear_data();
    },

    delete_item: function () {
        var self = UserRole;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#user-role-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#user-role-list tbody tr").length);
    },

    clear_data: function () {
        $("#RoleName").val('');
        $("#RoleID").val('');
        $("#LocationID").val('');
    },

    save_confirm: function () {
        var self = UserRole
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

    get_data: function () {
        var self = UserRole;
        var data = {};
        data.UserRoles = [];
        var item = {};
        $('#user-role-list tbody tr ').each(function () {
            item = {};
            item.UserID = $(this).find(".UserID").val();
            item.RoleID = $(this).find(".RoleID").val();
            item.LocationID = $(this).find(".LocationID").val();
            data.UserRoles.push(item);
        });
        return data;
    },

    Save: function () {
        var self = UserRole;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/UserRoles/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/UserRoles/Index";
                }
                else {
                    app.show_error('Failed to create UserRole');
                }
            }
        });
    },

    validate_role: function () {
        var self = UserRole;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form: function () {
        var self = UserRole;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_add: [
               {
                   elements: "#UserID",
                   rules: [
                       { type: form.non_zero, message: "Please add User", alt_element: "#UserName" },
                       { type: form.required, message: "Please add User", alt_element: "#UserName" },
                       { type: form.positive, message: "Please add User", alt_element: "#UserName" },
                   ]
               },

                {
                    elements: "#RoleID",
                    rules: [
                        { type: form.required, message: "Please choose a RoleName", alt_element: "#RoleName" },
                        { type: form.non_zero, message: "Please choose a RoleName", alt_element: "#RoleName" },
                        { type: form.positive, message: "Please choose a RoleName", alt_element: "#RoleName" },
                        {
                            type: function (element) {
                                var error = false;
                                $("#user-role-list tbody tr").each(function () {
                                    var location_id = $(this).find(".LocationID").val()
                                    var Locationid = $("#LocationID").val()
                                    if ($(this).find(".RoleID").val() == $(element).val() && location_id == Locationid) {
                                        error = true;
                                    }
                                });
                                return !error;
                            }, message: "RoleName already exists", alt_element: "#RoleName"
                        },
                    ]
                },
                {
                    elements: "#LocationID",
                    rules: [
                        { type: form.non_zero, message: "Please add location" },
                        { type: form.required, message: "Please add location" },
                        { type: form.positive, message: "Please add location" },
                    ]
                },
        ],
        on_save: [
           {
               elements: "#item-count",
               rules: [
                   { type: form.required, message: "Please add atleast one role" },
                   { type: form.non_zero, message: "Please add atleast one role" },
                   { type: form.positive, message: "Please add atleast one role" },
               ],
           }
        ]
    },

    role_list: function (type) {
        var self = UserRole;
        var $list = $('#role-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url;

            url = "/Masters/UserRoles/GetRolesList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
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
                            return "<input type='radio' class='uk-radio roleID' name='RoleID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
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
        }
    },
}