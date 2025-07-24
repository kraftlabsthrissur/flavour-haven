PreProcessIssue = {
    init: function () {
        var self = PreProcessIssue;
        PreProcessIssueCRUD.createedit();
        Item.preprocess_issue_item_list();
        $('#preprocess-item-list').SelectTable({
            selectFunction: PreProcessIssueCRUD.set_item_detail_modal,
            returnFocus: "#txtQty",
            modal: "#select-preprocess-issue-item",
            initiatingElement: "#txtItemName",
            startFocusIndex: 3
        });
    
        self.bind_events();
    },
   
    list: function () {
        var self = PreProcessIssue;
        $('#tabs-preprocess-issue').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

    },

    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-preprocess-issue-list');
                break;
            case "saved-preprocess-issue":
                $list = $('#saved-preprocess-issue-list');
                break;
            case "cancelled":
                $list = $('#cancelled-list');
                break;
            default:
                $list = $('#draft-preprocess-issue-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Manufacturing/PreProcessIssue/GetPreProcessIssueList?type=" + type;

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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>"

                       }
                   },

                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "TransDate", "className": "Date" },
                   { "data": "CreatedUser", "className": "CreatedUser" },
                   { "data": "ItemName", "className": "ItemName" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/PreProcessIssue/Details/" + Id);

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
        var self = PreProcessIssue;
        $.UIkit.autocomplete($('#preprocessIssueItem-autocomplete'), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
        $('#preprocessIssueItem-autocomplete').on('selectitem.uk.autocomplete', self.set_item_detail_autocomplete);
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on("keyup change", "#preprocess-issue-items-list .txtQuantity ", self.check_stock_quantity);


    },
    check_stock_quantity: function () {
        var row = $(this).closest('tr');
        var IssueQty = clean($(row).find('.txtQuantity').val());
        var AvailableStock = clean($(row).find('.Stock').val());
        if (IssueQty > AvailableStock) {
            app.show_error('Selected item dont have enough stock ');
            app.add_error_class($(row).find('.txtQuantity'));
        }

    },

    cancel_confirm: function () {
        var self = PreProcessIssue
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    get_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPreProcessIssueItemsForAutoComplete',
            data: {
                Hint: $('#txtItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_detail_autocomplete: function (event, item) {
        //item.productiongroupid,
        var itemName = item.value;
        //var purificationItemID = item.purificationitemid;
        var itemID = item.itemid;
        var unitID = item.unitid;
        var unit = item.unit;
        var stock = item.stock;
        var processID = item.processid;
        var processName = item.processname;
        $('#txtPreprocess').val(processName);
        $("#ItemID").val(itemID);
        $('#txtUnit').val(unit);
        $('#Stock').val(stock);
        $("#ProcessID").val(processID);
        $("#UnitID").val(unitID);
        $('#txtQty').focus();
    },
    cancel: function () {
        $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Manufacturing/PreProcessIssue/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "MaterialPurificationIssue"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Purification isssue cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Manufacturing/PreProcessIssue/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },
    details: function () {
        PreProcessIssue.freeze_header();
    },
    freeze_header: function () {
        fh_item = $("#preprocess-issue-items-list").FreezeHeader();
    },
    count_items: function () {
        var count = $('#preprocess-issue-items-list tbody tr').length;
        $('#item-count').val(count);
    },
    get_issueitem_batchwise: function () {
        var ProcessID = $("#ProcessID").val();
        var processName = $('#txtPreprocess').val();
        var rowHtml = "";
        $.ajax({
            url: '/Masters/Batch/GetPreProcessItemBatchwise',
            data: {
                ItemID: clean($("#ItemID").val()),
                UnitID: $('#UnitID').val(),
                Quantity: clean($('#txtQty').val()),

            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        var nxtSno = $('#preprocess-issue-items-list tbodt tr').length + 1;
                        rowHtml += '<tr>'
                     + '<td class="uk-text-center index">' + nxtSno + '</td>'
                     + '<td>'
                     + '<input type = "hidden" class="hdnID" value="0" /> '
                     + '<input type = "hidden" class="hdnItemID" value="' + record.ItemID + '" /> '
                     + '<input type = "hidden" class="UnitID" value="' + record.UnitID + '" /> '
                     + '<input type = "hidden" class="hdnProcessID" value="' + ProcessID + '"/> '
                     + '<input type = "hidden" class="Stock" value="' + record.Stock + '"/> '
                     + '<input type = "hidden" class="BatchID" value="' + record.ID + '"/> '
                     + record.ItemName
                     + '</td>'
                     + '<td>' + record.Unit + '</td>'
                     + '<td>' + record.BatchNo + '</td>'
                     + '<td class=mask-production-qty uk-text-right>' + record.Stock + '</td>'
                     + '<td><input type="text" class="md-input mask-production-qty txtQuantity positive" value="' + record.IssueQty + '" /></td>'
                     + '<td>' + processName + '</td>'
                     + '<td><i class="uk-close uk-float-right removeItem" ></i></td>'
                     + '</tr>'

                    });
                    $tr = $(rowHtml);
                    app.format($tr);
                    $('#preprocess-issue-items-list').find('tbody').append($tr);

                }
                $("#preprocess-issue-items-list tbody tr").each(function (i) {
                    $(this).closest('tr').find(".index").text(i + 1);
                });
                PreProcessIssue.count_items();
            },

        });


    },
    validate_item: function () {
        var self = PreProcessIssue;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_form: function () {
        var self = PreProcessIssue;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = PreProcessIssue;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item", alt_element: "#txtItemName" },
                ]
            },
            {
                elements: "#txtQty",
                rules: [
                    { type: form.required, message: "Invalid quantity" },
                    { type: form.non_zero, message: "Invalid quantity" },
                    { type: form.positive, message: "Invalid quantity" },                    
                ]
            },
        ],
        on_submit: [
            {
                elements: "#txtIssueDate",
                rules: [
                    { type: form.required, message: "Invalid date" },
                ]
            },
            {
                elements: "#preprocess-issue-items-list tbody tr",
                rules: [
                    {
                        type: function (element) {
                            return $(element).length > 0;
                        }, message: "Please add atleast one item"
                    },
                ]
            },
            {
                elements: ".hdnItemID",
                rules: [
                    { type: form.required, message: "Invalid item" },
                ]
            },
            {
                elements: ".txtQuantity",
                rules: [
                    { type: form.required, message: "Invalid quantity" },
                    { type: form.non_zero, message: "Invalid quantity" },
                    { type: form.positive, message: "Invalid quantity" },
                     {
                         type: function (element) {
                             return clean($(element).val()) <= clean($(element).closest('tr').find(".Stock").val());
                         }, message: "Insufficient stock"
                     },
                ]
            },
               {
                   elements: "#item-count",
                   rules: [
                       { type: form.non_zero, message: "Please add atleast one item" },
                       { type: form.required, message: "Please add atleast one item" },

                   ]
               },
        ],
        on_draft: [
            {
                elements: "#txtIssueDate",
                rules: [
                    { type: form.required, message: "Invalid date" },
                ]
            },
            {
                elements: "#preprocess-issue-items-list tbody tr",
                rules: [
                    {
                        type: function (element) {
                            return $(element).length > 0;
                        }, message: "Please add atleast one item"
                    },
                ]
            },
            {
                elements: ".hdnItemID",
                rules: [
                    { type: form.required, message: "Invalid item" },
                ]
            },
            {
                elements: ".txtQuantity",
                rules: [
                    { type: form.required, message: "Invalid quantity" },
                    { type: form.non_zero, message: "Invalid quantity" },
                    { type: form.positive, message: "Invalid quantity" },

                ]
            },
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please add atleast one item" },
                     { type: form.required, message: "Please add atleast one item" },

                 ]
             },
        ]
    }
}
var fh_item;
PreProcessIssueCRUD = {
    createedit: function () {
        $currobj = this;

        $currobj.SelectedItem = { ItemName: '', PurificationItemID: 0, ItemID: 0, UnitID: 0, Unit: '', ProcessID: 1, ProcessName: '' };




        this.init = function () {


            //$.UIkit.autocomplete($('#preprocessIssueItem-autocomplete'), { 'source': $currobj.get_item_AutoComplete, 'minLength': 1 });
            //$('#preprocessIssueItem-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_item_detail_autocomplete);

            $('#btnOKPreProcessIssueItem').on('click', $currobj.set_item_detail_modal);

            $('body').on('click', '#btnAddProduct', $currobj.add_product_clicked)
            $('body').on('click', '.btnSave', function () {
                $currobj.save_confirm(false);
            });
            $('body').on('click', '.btnSaveASDraft', function () {
                $currobj.save_preprocess_issue(true);
            });

            $('body').on('keydown', '#txtQty', $currobj.txtqty_clicked);

            $('body').on('click', '.removeItem', $currobj.remove_item_clicked); //dynamic generated html
            //$('#txtQty')

        }
        this.save_confirm = function () {
            app.confirm_cancel("Do you want to Save", function () {
                $currobj.save_preprocess_issue();
            }, function () {
            })
        },

        this.remove_item_clicked = function () {
            $(this).parents('tr').remove();          
            PreProcessIssue.count_items();
        }

        this.txtqty_clicked = function (event) {
            if (event.which == 13) {    //Enter key pressed
                $currobj.add_product_clicked();
            }

        }

        this.save_preprocess_issue = function (isDraft) {
            var error_count;
            var url;
            if (isDraft) {
                error_count = PreProcessIssue.validate_draft();
                url = '/Manufacturing/PreProcessIssue/SaveAsDraft';
            } else {
                error_count = PreProcessIssue.validate_form();
                url = '/Manufacturing/PreProcessIssue/Save';
            }
            if (error_count > 0) {
                return;
            }
            var obj = $currobj.get_proprocess_issue_save_obj(isDraft);

            $(".btnSave, .btnSaveASDraft,.cancel").css({ 'display': 'none' });

            $.ajax({
                url: url,
                data: obj,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice(response.Message);
                        setTimeout(function () {
                            window.location = "/Manufacturing/PreProcessIssue/";
                        }, 1000);
                    }
                    else {
                        if (typeof response.data[0].ErrorMessage != "undefined")
                            app.show_error(response.data[0].ErrorMessage);
                        $(".btnSave, .btnSaveASDraft,.cancel").css({ 'display': 'block' });
                    }
                }
            });
        }

        this.custom_success_alert_notification = function (msg) {
            app.show_notice(msg);
        }

        this.custom_error_alert_notification = function (msg) {
            app.show_error(msg);
        }

        this.get_proprocess_issue_save_obj = function (isDraft) {
            var obj = {};
            obj.IssueNo = $('#txtIssueNo').val();
            obj.IssueDateStr = $('#txtIssueDate').val();
            obj.ID = $('#hdnPreprocessIssueID').val();
            obj.IsDraft = isDraft;

            var items = [];

            $('#preprocess-issue-items-list tbody tr').each(function () {
                var item = {
                    ID: $(this).find('.hdnID').val(),
                    ItemID: $(this).find('.hdnItemID').val(),
                    ProcessID: $(this).find('.hdnProcessID').val(),
                    Quantity: clean($(this).find('.txtQuantity').val()),
                    UnitID: clean($(this).find('.UnitID').val()),
                    BatchID: clean($(this).find('.BatchID').val()),
                };
                items.push(item);
            });
            obj.Items = items;
            return obj;
        }

        this.set_item_detail_modal = function () {
            var radio = $('#preprocess-item-list tbody input[type="radio"]:checked');
            var currRow = $(radio).parents('tr');

            var itemName = $(currRow).find('.Name').text();
            var itemID = $(currRow).find('.ItemID').val();
            var unit = $(currRow).find('.Unit').text();
            var stock = $(currRow).find('.Stock').val();
            var processID = $(currRow).find('.ProcessID').val();
            var processName = $(currRow).find('.Activity').text();
            var unitID = $(currRow).find('.UnitID').val();
            $('#txtPreprocess').val(processName);
            $('#txtItemName').val(itemName);
            $('#txtUnit').val(unit);
            $('#Stock').val(stock);
            $("#ItemID").val(itemID);
            $("#ProcessID").val(processID);
            $("#UnitID").val(unitID);
            $('#txtQty').focus();
        }

        this.add_product_clicked = function () {

            var error_count = PreProcessIssue.validate_item();

            if (error_count > 0) {
                return;
            }

            var qty = clean($('#txtQty').val());
            var processName = $('#txtPreprocess').val();
            var itemName = $('#txtItemName').val();
            var itemID = clean($("#ItemID").val());
            var ProcessID = $("#ProcessID").val();
            var unit = $('#txtUnit').val();
            var stock = clean($('#Stock').val());
            var unitID = $('#UnitID').val();
            var tblObj = $('#preprocess-issue-items-list');
            var nxtSno = $currobj.get_next_row_sno(tblObj);
            var itemid_count = 0;
            var row;
            $("#preprocess-issue-items-list tbody tr").each(function () {
                if ($(this).find(".hdnItemID").val() == itemID) {
                    row = $(this).closest("tr").remove();;
                }
            })
            PreProcessIssue.get_issueitem_batchwise();
         
            $('#txtQty').val('0');
            $('#txtItemName').val('');
            $('#txtUnit').val('');
            $('#ItemID').val('');
            $('#txtPreprocess').val('');
            setTimeout(function () {
                $('#txtItemName').focus();
            }, 100);
            PreProcessIssue.count_items();
        }

        this.get_next_row_sno = function (tblObj) {
            var rowNo = $(tblObj).find('tr:last td:first').html();
            if (rowNo != undefined && rowNo != null)
                rowNo = parseInt(rowNo) + 1;
            else
                rowNo = 1;
            return rowNo;
        }

        this.ajaxRequest = function (url, data, requestType, callBack) {
            $.ajax({
                url: $currobj.root_url() + url,
                type: requestType,
                data: data,
                success: function (successResponse) {
                    if (callBack != null && callBack != undefined)
                        callBack(successResponse);
                },
                error: function (errResponse) {//Error Occured 
                }
            });
        }

        this.root_url = function () {
            return "/Manufacturing/PreProcessIssue/";
        }

        $currobj.init();
    }

}