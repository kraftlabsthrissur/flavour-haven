SalesRepresentative = {
    init: function () {
        var self = SalesRepresentative;
        self.tree();
        self.bind_events();

        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
    },

    list: function () {
        $list = $('#sales-representative-list');

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
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Masters/SalesRepresentative/GetSalesRepresentativeList",
                    "type": "POST"
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ParentName", "className": "ParentName" },
                    { "data": "Area", "className": "Area" },
                    { "data": "SalesIncentiveCategory", "className": "SalesIncentiveCategory" },
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                }
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            $('body').on("datatable.change", '#sales-order-list', function () {
                list_table.fnDraw();
            });
            return list_table;
        }
    },

    select_employee: function () {
        var self = SalesRepresentative;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var DesignationID =clean($(row).find(".DesignationID").val());
        var Designation = $(row).find(".Designation").val();
        $("#FSOName").val(Name);
        $("#EmployeeID").val(ID),
        $("#DesignationID").val(DesignationID),
        $("#Designation").val(Designation),
        UIkit.modal($('#select-employee')).hide();
        UIkit.modal("#add-sales-representative").show();
    },

    bind_events: function () {
        var self = SalesRepresentative;
        $("body").on("click", ".btnAdd", self.show_modal);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $("#btnOKEmployee").on("click", self.select_employee);
        $("#btnCloseEmployee").on("click", self.on_close_employee_modal);

        $("#btnAdd").on("click", self.save);
        $("body").on("click", ".btnEdit", self.edit_modal);
        $("body").on("click", ".btnCancel", self.delete_confirm);
    },

    on_close_employee_modal:function(){
        UIkit.modal("#add-sales-representative").show();
    },

    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#FSOName').val(),
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

    set_employee: function (event, item) {
        var self = SalesRepresentative;
        $("#EmployeeID").val(item.id),
        $("#FSOName").val(item.name);
        $("#DesignationID").val(item.designationid);
        $("#Designation").val(item.designation);
    },

    show_modal: function () {
        var self = SalesRepresentative;
        self.clear_items();
        $(".area-dropdown").removeClass("uk-hidden");
        $(".area-textbox").addClass("uk-hidden");
        var ParentID = $(this).data("parent-id");
        var AreaID= $(this).data("area-id");
        self.get_areas_by_parent_areaid(AreaID)
        $("#ParentID").val(ParentID);
        $("#AreaID").val(AreaID);
        $(".IsSubLevel-checkbox").removeClass("uk-hidden");
        UIkit.modal("#add-sales-representative").show();
    },

    get_areas_by_parent_areaid: function (AreaID) {
        var self = SalesRepresentative;
        $("#AreaID").html("");
        var html = "";
        $.ajax({
            url: '/Masters/SalesRepresentative/GetAreasByParentArea',
            data: { ParentAreaID: AreaID },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    html += "<option value='" + 0 + "'>Select</option>";
                    $(response.Data).each(function (i, record) {
                        html += "<option value='" + record.ID + "'>" +record.Name + "</option>";
                    });
                    $("#AreaID").append(html);
                }
                else {
                   
                }
            }
        });
    },

    get_areas: function (AreaID) {
        var self = SalesRepresentative;
        $("#AreaID").html("");
        var html = "";
        $.ajax({
            url: '/Masters/SalesRepresentative/GetAreas',
            data: { AreaID: AreaID },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    html += "<option value='" + 0 + "'>Select</option>";
                    $(response.Data).each(function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    });
                    $("#AreaID").append(html);
                    $("#AreaID").val(AreaID);
                }
            }
        });
    },

    tree: function () {
        $("#tA").fancytree({
            checkbox: 0,
            selectMode: 0,
            //imagePath: "/assets/icons/others/",
            autoScroll: 0,
        })
    },

    get_data: function () {
        var self = SalesRepresentative;
        var data = {};
        data.ID = $("#ID").val(),
        data.EmployeeID = $("#EmployeeID").val(),
        data.FSOName = $("#FSOName").val();
        data.DesignationID = $("#DesignationID").val();
        data.SalesIncentiveCategoryID = $("#SalesIncentiveCategoryID").val();
        data.AreaID = $("#AreaID").val();
        data.ParentID = $("#ParentID").val();
        if ($(".IsSubLevel").prop('checked') == true) {
            data.IsSubLevel = true;
        } else {
            data.IsSubLevel = false;
        }
        return data;
    },

    save: function () {
        var self = SalesRepresentative;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/SalesRepresentative/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/SalesRepresentative/Index";
                }
                else {
                    app.show_error("FSO already assigned");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            }
        });
    },

    edit_modal: function () {
        var self = SalesRepresentative;
        var ID = $(this).data("id");
        var FSOName = $(this).data("fso");
        var IsSubLevel = $(this).data("is-sublevel");
        var EmployeeID = $(this).data("employeeid");
        var DesignationID = $(this).data("designationid");
        var Designation = $(this).data("designation");
        var SalesIncentiveCategoryID = $(this).data("sales-categoryid");
        var AreaID = $(this).data("area-id");
        var Area = $(this).data("area");
        var ParentID = $(this).data("parent-id");
        if (IsSubLevel == "True") {
            $(".IsSubLevel").closest('div').addClass("checked")
            $(".IsSubLevel").prop('checked', true)
        }
        else {
            $(".IsSubLevel").prop('checked', false)
            $(".IsSubLevel").closest('div').removeClass("checked")
        }
        $(".area-dropdown").addClass("uk-hidden");
        $(".IsSubLevel-checkbox").addClass("uk-hidden");
        $(".area-textbox").removeClass("uk-hidden");
        self.get_areas(AreaID);
        $("#Area").val(Area);
        $("#ID").val(ID);
        $("#EmployeeID").val(EmployeeID);
        $("#FSOName").val(FSOName);
        $("#DesignationID").val(DesignationID);
        $("#Designation").val(Designation);
        if (SalesIncentiveCategoryID > 0) {
        $("#SalesIncentiveCategoryID").val(SalesIncentiveCategoryID);
        }
        else
        {
            $("#SalesIncentiveCategoryID").val("");
        }
        $("#ParentID").val(ParentID);
        UIkit.modal("#add-sales-representative").show();
    },

    clear_items: function () {
        var self = SalesRepresentative;
        $("#EmployeeID").val("");
        $("#FSOName").val("");
        $("#SalesIncentiveCategoryID").val("");
        $("#DesignationID").val("");
        $("#Designation").val("");
        $("#AreaID").val("");
    },

    delete_confirm: function () {
        var self = SalesRepresentative;
        var ID = $(this).data("id");
        app.confirm_cancel("Do you want to remove the item", function () {
            $.ajax({
                url: '/Masters/SalesRepresentative/RemoveFSO',
                data: {
                    ID: ID
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Deleted Successfully");
                        window.location = "/Masters/SalesRepresentative/Create";
                    } else {
                        app.show_error("Child Belongs To This Node So Can't Be Deleted");
                    }
                },
            });
        }, function () {
        })
    },

    validate_form: function () {
        var self = SalesRepresentative;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
                {
                    elements: "#DesignationID",
                    rules: [
                        { type: form.required, message: "Please choose a Designation" },
                        { type: form.positive, message: "Please choose a Designation" },
                        { type: form.non_zero, message: "Please choose a Designation" }
                    ],
                },
                {
                    elements: "#EmployeeID",
                    rules: [
                        { type: form.required, message: "Please choose a FSO", alt_element: "#FSOName" },
                        { type: form.positive, message: "Please choose a FSO", alt_element: "#FSOName" },
                        { type: form.non_zero, message: "Please choose a FSO", alt_element: "#FSOName" }
                    ],
                },
                 
                  {
                      elements: "#AreaID",
                      rules: [
                          { type: form.required, message: "Please choose a Area" },
                          { type: form.positive, message: "Please choose a Area" },
                          { type: form.non_zero, message: "Please choose a Area" }
                      ],
                  },
        ],
    }
}


