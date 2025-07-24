ApprovalFlow = {
    init: function () {
        var self = ApprovalFlow;
        self.bind_events();

    },

    list: function () {
        var $list = $('#approval-flow-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/ApprovalFlow/GetApprovalFlowList"

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
                   { "data": "AppQueueName", "className": "AppQueueName" },
                   { "data": "ForDepartmentName", "className": "ForDepartmentName" },
                   {
                        "data": "AmountAbove", "searchable": false, "className": "AmountAbove",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.AmountAbove + "</div>";
                        }
                   },
                    {
                        "data": "AmountBelow", "searchable": false, "className": "AmountBelow",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.AmountBelow + "</div>";
                        }
                    },
                   { "data": "ItemCategoryName", "className": "ItemCategoryName" },
                   { "data": "ItemAccountsCategoryName", "className": "ItemAccountsCategoryName" },
                   { "data": "SuppliercategoryName", "className": "SuppliercategoryName" },
                   { "data": "SupplierAccountscategoryName", "className": "SupplierAccountscategoryName" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/ApprovalFlow/Details/" + Id);
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
        var self = ApprovalFlow;
        $("body").on('click', '.btnSave,.btnSaveNew', self.save);
        $("#btnAdd").on("click", self.get_ApprovalFlow);
    },

    get_data: function () {
        var self = ApprovalFlow;
        var data = {};
        data.ID = $("#ID").val();
        data.UserLocationID = $("#UserLocationID").val();
        data.ForDepartmentID = $("#ForDepartmentID").val();
        data.AmountAbove = clean($("#AmountAbove").val());
        data.AmountBelow = clean($("#AmountBelow").val());
        data.ItemCategoryID = $("#ItemCategoryID").val();
        data.ItemAccountsCategoryID = $("#ItemAccountsCategoryID").val();
        data.ApprovalQueueID = $("#ApprovalQueueID").val();
        data.SupplierCategoryID = $("#SupplierCategoryID").val();
        data.SupplierAccountsCategoryID = $("#SupplierAccountsCategoryID").val();
        data.LocationName = $("#UserLocationID option:selected").text();
        return data;
    },

    save: function () {
        var self = ApprovalFlow;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        var IsNew = false;
        if ($(this).hasClass("btnSaveNew")) {
            IsNew = true;
        }
        data = self.get_data();
        $(".btnSave,.btnSaveNew").css({ 'display': 'none' });
        $.ajax({
            url: '/Masters/ApprovalFlow/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                        app.show_notice("Saved Successfully");
                    if (IsNew == true) {
                        setTimeout(function () {
                            window.location = "/Masters/ApprovalFlow/Create";
                        }, 1000);
                    }
                    else {
                        setTimeout(function () {
                            window.location = "/Masters/ApprovalFlow/Index";
                        }, 1000);
                    }
                }
                else{
                    app.show_error("Amount Already Exist In Range");
                    $(".btnSave,.btnSaveNew").css({ 'display': 'block' });
            }
            }
        });
    },

    validate_save: function () {
        var self = ApprovalFlow;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_add: function () {
        var self = ApprovalFlow;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_save: [
             {
                 elements: "#AmountBelow",
                 rules: [
                         { type: form.possitive, message: "Amount Should be possitive value" },
                         {
                             type: function (element) {
                                 var amountabove = clean($("#AmountAbove").val());
                                 var amountbelow = clean($("#AmountBelow").val());
                                 return amountbelow > amountabove
                             }, message: "Amount Below should not be less than Amount Above"
                         },
                 ]
             },
             {
                 elements: "#ForDepartmentID",
                 rules: [
                     { type: form.required, message: "Please Select Department" },
                     { type: form.non_zero, message: "Please Select Department" },
                 ],
             },
              {
                  elements: "#UserLocationID",
                  rules: [
                      { type: form.required, message: "Please Select Location" },
                      { type: form.non_zero, message: "Please Select Location" },
                  ],
              },
              
                {
                    elements: "#ItemCategoryID",
                    rules: [
                        { type: form.required, message: "Please Select Item Category" },
                        { type: form.non_zero, message: "Please Select Item Category" },
                    ],
                },
                  {
                      elements: "#ItemAccountsCategoryID",
                      rules: [
                          { type: form.required, message: "Please Select Item Accounts Category" },
                          { type: form.non_zero, message: "Please Select Item Accounts Category" },
                      ],
                  },
                  {
                      elements: "#ApprovalQueueID",
                      rules: [
                          { type: form.required, message: "Please Select ApprovalQueue" },
                          { type: form.non_zero, message: "Please Select ApprovalQueue" },
                      ],
                  },
        ],
        on_add: [
            {
                elements: "#ForDepartmentID",
                rules: [
                    { type: form.required, message: "Please Select Department" },
                    { type: form.non_zero, message: "Please Select Department" },
                ],
            },
             {
                 elements: "#UserLocationID",
                 rules: [
                     { type: form.required, message: "Please Select Location" },
                     { type: form.non_zero, message: "Please Select Location" },
                 ],
             },

               {
                   elements: "#ItemCategoryID",
                   rules: [
                       { type: form.required, message: "Please Select Item Category" },
                       { type: form.non_zero, message: "Please Select Item Category" },
                   ],
               },
                 {
                     elements: "#ItemAccountsCategoryID",
                     rules: [
                         { type: form.required, message: "Please Select Item Accounts Category" },
                         { type: form.non_zero, message: "Please Select Item Accounts Category" },
                     ],
                 },
        ]
    },

    get_ApprovalFlow: function () {
        var self = ApprovalFlow;
        var length;
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Masters/ApprovalFlow/GetApprovalFlow/',
            dataType: "json",
            data: {
                'UserLocationID': $("#UserLocationID").val(),
                'ForDepartmentID': $("#ForDepartmentID").val(),
                'ItemCategoryID': $("#ItemCategoryID").val(), 
                'ItemAccountsCategoryID': $("#ItemAccountsCategoryID").val(),
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td class="sl-no uk-text-center">' + slno + '</td>'
                        + '<td class="TestName">' + item.LocationName
                        + '<input type="hidden" class="QCTestID"value="' + item.QCTestID + '"/>'
                        + '<input type="hidden" class = "ID" value="' + item.ID + '" />'
                        + '</td>'
                        + '<td class="ForDepartmentName">' + item.ForDepartmentName + '</td>'
                        + '<td class="ItemCategoryName">' + item.ItemCategoryName + '</td>'
                        + '<td>' + item.ItemAccountsCategoryName + '</td>'
                        + '<td class="uk-text-right mask-qty">' + item.AmountAbove + '</td>'
                        + '<td class="uk-text-right mask-qty">' + item.AmountBelow + '</td>'
                        + '<td>' + item.AppQueueName + '</td>'
                        + '<td>' + item.SuppliercategoryName + '</td>'
                        + '<td>' + item.SupplierAccountscategoryName + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#ApprovalFlowList tbody").html($content);
            },
        });
    },


}
