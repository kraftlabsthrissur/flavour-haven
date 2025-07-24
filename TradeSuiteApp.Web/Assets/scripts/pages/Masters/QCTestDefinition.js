QCTestDefinition = {
    init: function () {
        var self = QCTestDefinition;
        item_list = Item.stockable_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRequiredQty",
            modal: "#select-item",
            initiatingElement: "#ItemName"
        });
        self.list();

        self.bind_events();
        self.deleted = [];
    },


    list: function () {
        var $list = $('#QCTestDefinition-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/QCTestDefinition/GetQcTestDefinitionList"

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
                               + "<input type='hidden' class='ItemID' value='" + row.ItemID + "'>";
                           + "<input type='hidden' class='QCTest' value='" + row.QCTest + "'>";

                       }
                   },
                   { "data": "Code", "className": "ItemCode" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "TestName", "className": "TestName" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ItemID").val();
                        app.load_content("/Masters/QCTestDefinition/Details/" + Id);
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
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': QCTestDefinition.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', QCTestDefinition.set_item_details);
        $("#btnOKItem").on("click", QCTestDefinition.select_item);
        $(".btnSave").on("click", QCTestDefinition.save_confirm);
        $("body").on("click", "#btnAdd", QCTestDefinition.add_item);
      //  $("body").on("click", ".remove-item", QCTestDefinition.isdeletable_item);
        $("body").on("ifChanged", ".check-box", QCTestDefinition.check);
    },

    save_confirm: function () {
        var self = QCTestDefinition;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    select_item: function () {
        var self = QCTestDefinition;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#Unit").val(Unit);
        self.get_item();
        UIkit.modal($('#select-item')).hide();
    },

    set_item_details: function (event, item) {
        var self = QCTestDefinition;// on select auto complete item
        $("#ItemID").val(item.id);
        $("#ItemTypeID").val(item.itemTypeId);
        $("#Stock").val(item.stock);
        $("#QtyUnderQC").val(item.qtyUnderQc);
        $("#Unit").val(item.unit);
        $("#UnitID").val(item.unitId);
        $("#QtyOrdered").val(item.qtyOrdered);
        $('#txtRqQty').focus();
        self.get_item();
    },

    get_items: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    save: function () {
        var self = QCTestDefinition;
        var data;
        index = $("#QCTestDefinitionlist tbody .included").length;
        var deleted_array_length = self.deleted.length;
        index = index + deleted_array_length;
        $("#item-count").val(index);
        data = self.get_data();
        $.ajax({
            url: '/Masters/QCTestDefinition/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/QCTestDefinition/Index";
                }
                else {
                    app.show_error('Failed to create Qc Test Definition');
                }
            }
        });
    },

    get_data: function () {
        var self = QCTestDefinition;
        var data = {};
        data.ItemID = $("#ItemID").val(),
        data.Items = [];
        var item = {};
        $('#QCTestDefinitionlist tbody tr.included').each(function () {
            item = {};
            item.ID = $(this).find(".ID").val();
            item.QCTestID = $(this).find(".QCTestID").val();
            item.RangeFrom = clean($(this).find(".RangeFrom").val());
            item.RangeTo = clean($(this).find(".RangeTo").val());
            item.Result = $(this).find(".Result").val();
            item.IsMandatory = $(this).find(".IsMandatory").text();
            item.StartDate = $(this).find(".startdate").val();
            item.EndDate = $(this).find(".enddate").val();
            data.Items.push(item);
        });
        data.ID = [];
        data.ID = self.deleted;
        return data;
    },

    add_item: function () {
        var self = QCTestDefinition;
        if (self.validate_on_add() > 0) {
            return;
        }
        sino = $('#QCTestDefinitionlist tbody tr ').length + 1;
        var TestName = $("#QCTestID option:selected").text();
        var QCTestID = $("#QCTestID").val();
        var RangeFrom = $("#RangeFrom").val();
        var RangeTo = $("#RangeTo").val();
        var Result = $("#Result").val();
        var IsMandatory = "";
        var StartDate = $("#StartDate").val();
        var EndDate = $("#EndDate").val();
        if ($('.IsMandatory').prop("checked")) {
            IsMandatory = "True";
        }
        else {
            IsMandatory = "False";
        }
        var content = "";
        var $content;
        content = '<tr class="included">'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class=" td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box"/>' + '</td>'
            + '<td class="TestName">' + TestName
            + '<input type="hidden" class = "QCTestID" value="' + QCTestID + '" />'
            + '<input type="hidden" class = "ID" value="' + 0 + '" />'
            + '</td>'
            + '<td  >' + '<input type="text" value=" ' + RangeFrom + '" class="md-input uk-text-right RangeFrom mask-production-qty"  /> ' + '</td>'
            + '<td  >' + '<input type="text" value=" ' + RangeTo + '" class="md-input uk-text-right RangeTo mask-production-qty"  /> ' + '</td>'
            + '<td  >' + '<input type="text" value=" ' + Result + '" class="md-input Result "  /> ' + '</td>'
            + '<td class="IsMandatory uk-text-center" >' + IsMandatory + '</td>'
            + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date startdate" value="' + StartDate + '" name="startdate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date QCDatePicker"></i></span></div></td>'
            + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date enddate" value="' + EndDate + '" name="enddate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date QCDatePicker"></i></span></div></td>'
           
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#QCTestDefinitionlist tbody').append($content);

        var IsMandatory = "";
        self.count();
        self.clear_data();
    },

    clear_data: function () {
        var self = QCTestDefinition;
        $("#TestName").val('');
        $("#RangeFrom").val('');
        $("#RangeTo").val('');
        $("#Result").val('');
        $(".IsMandatory").iCheck('uncheck');
        $("#QCTestID option:selected").text('Select');

    },

    validate_on_add: function () {
        var self = QCTestDefinition;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
             {
                 elements: "#QCTestID",
                 rules: [
                     { type: form.required, message: "Please choose a TestName" },
                     { type: form.non_zero, message: "Please choose a TestName" },
                     {
                         type: function (element) {
                             var error = false;
                             $("#QCTestDefinitionlist tbody tr").each(function () {
                                 if ($(this).find(".QCTestID").val() == $(element).val()) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "Test already exists"
                     },
                 ]
             },
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose an Item" },
                    { type: form.non_zero, message: "Please choose an Item" },
                ],
            },
             {
                 elements: "#Result",
                 rules: [
                      {
                          type: function (element) {
                              return (clean($("#RangeFrom").val()) == 0 && clean($("#RangeTo").val()) == 0) ? form.required(element) : true;
                          }, message: "Please enter a result"
                      }
                 ],
             },
              {
                  elements: "#RangeFrom",
                  rules: [
                       {
                           type: function (element) {
                               return (clean($("#Result").val()) == "") ? form.required(element) : true;
                           }, message: "Please enter a From Limit"
                       }
                  ],
              },
               {
                   elements: "#RangeTo",
                   rules: [
                        {
                            type: function (element) {
                                return (clean($("#RangeFrom").val()) != 0) ? form.required(element) : true;
                            }, message: "Please enter a To Limit"
                        }
                   ],
               },

                {
                    elements: "#EndDate",
                    rules: [
                        {
                            type: function (element) {
                                return $(element).closest("tr").find(".enddate").val() == "" && $(element).val() != "" ? false : true;
                            }, message: "End date required"
                        },
                         {
                             type: function (element) {
                                 var startdate = $("#StartDate").val().split('-');
                                 var startdate1 = new Date(startdate[2], startdate[1] - 1, startdate[0]);
                                 var enddate = $(element).val().split('-');
                                 var enddate2 = new Date(enddate[2], enddate[1] - 1, enddate[0]);
                                 return enddate2 > startdate1
                             }, message: "End date must be greater than start date"
                         }
                    ]
                },
            {
                elements: "#StartDate",
                rules: [
                     { type: form.required, message: "Please enter start date" },
                ]
            },

        ],
        on_save: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                   { type: form.required, message: "Please add atleast one item" },

                ]
            },
            {
                elements: "#QCTestDefinitionlist .enddate",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".enddate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "End date required"
                    },
                    {
                        type: function (element) {
                            var startdate = $(element).closest("tr").find(".startdate").val().split('-');
                            var startdate1 = new Date(startdate[2], startdate[1] - 1, startdate[0]);
                            var enddate = $(element).val().split('-');
                            var enddate2 = new Date(enddate[2], enddate[1] - 1, enddate[0]);
                            // var date_time2 = new Date(date_time_string2[2], date_time_string2[1] - 1, date_time_string1[0]);
                            return enddate2 > startdate1
                        }, message: "End date must be greater than start date"
                    }
                ]
            },
            {
                elements: ".startdate",
                rules: [
                     { type: form.required, message: "Please enter start date" },
                ]
            },

        ]
    },

    validate_save: function () {
        var self = QCTestDefinition;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    get_item: function () {
        var length;
        $.ajax({
            url: '/Masters/QCTestDefinition/GetTestForItemList/',
            dataType: "json",
            data: {
                'ItemID': $("#ItemID").val(),
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
                        + '<td class="td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="TestName">' + item.TestName
                        + '<input type="hidden" class="QCTestID"value="' + item.QCTestID + '"/>'
                        + '<input type="hidden" class = "ID" value="' + item.ID + '" />'
                        + '</td>'
                        + '<td>' + '<input type="text" value=" ' + item.RangeFrom + '" class="md-input uk-text-right RangeFrom mask-production-qty" disabled /> ' + '</td>'
                        + '<td>' + '<input type="text" value=" ' + item.RangeTo + '" class="md-input uk-text-right RangeTo mask-production-qty" disabled /> ' + '</td>'
                        + '<td>' + '<input type="text" value=" ' + item.Result + '" class="md-input Result " disabled /> ' + '</td>'
                        + '<td class="IsMandatory uk-text-center">' + item.IsMandatory + '</td>'
                        + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date startdate" value="' + item.StartDate + '" name="startdate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date QCDatePicker"></i></span></div></td>'
                        + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date enddate" value="' + item.EndDate + '" name="enddate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date QCDatePicker"></i></span></div></td>'
                       
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#QCTestDefinitionlist tbody").html($content);
            },
        });
    },

    check: function () {
        var self = QCTestDefinition;
        $("#QCTestDefinitionlist tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
                $(row).find(".RangeFrom").prop("disabled", false);
                $(row).find(".RangeTo").prop("disabled", false);
                $(row).find(".Result").prop("disabled", false);
                $(row).find(".IsMandatory").prop("disabled", false);

            } else {
                $(row).find(".RangeFrom").prop("disabled", true);
                $(row).find(".RangeTo").prop("disabled", true);
                $(row).find(".Result").prop("disabled", true);
                $(row).find(".IsMandatory").prop("disabled", true);
                $(row).removeClass('included');
            }
        });
        self.count();
    },
    count: function () {
        index = $("#QCTestDefinitionlist tbody .included").length;
        $("#item-count").val(index);
    },
    //isdeletable_item: function () {
    //    var self = QCTestDefinition;
    //    var ID = $(this).closest('tr').find(".ID").val();
    //    var QCTestID = $(this).closest('tr').find(".QCTestID").val();
    //    var ItemID = $("#ItemID").val();
    //    var row = $(this).closest('tr');
    //    $.ajax({
    //        url: '/Masters/QCTestDefinition/IsDeletable',
    //        data: {
    //            ID: ID,
    //            QCTestID: QCTestID,
    //            ItemID: ItemID
    //        },
    //        dataType: "json",
    //        type: "POST",
    //        success: function (data) {
    //            if (data.Status == "success") {
    //                $(row).remove();
    //                self.deleted.push(ID);
    //            } else {
    //                app.show_error("Failed to delete.");
    //            }
    //        },
    //    });
    //    self.count();
    //},
}