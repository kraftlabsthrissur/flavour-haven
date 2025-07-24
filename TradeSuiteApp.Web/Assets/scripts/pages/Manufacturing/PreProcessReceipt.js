var fh_item;
PreProcessReceipt = {
    init: function () {
        var self = PreProcessReceipt;
        PreProcessReceiptCRUD.createedit();
        self.materials_list();
        $('#tbl-unprocessed-materialpurification').SelectTable({
            selectFunction: PreProcessReceiptCRUD.set_item_detail_modal,
            returnFocus: "#txtReceiptQty",
            modal: "#select-issueditem",
            initiatingElement: "#txtItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        fh_item = $("#preprocess-receipt-items-list").FreezeHeader();
    },

    list: function () {
        var self = PreProcessReceipt;
        $('#tabs-preprocess-receipt').on('change.uk.tab', function (event, active_item, previous_item) {
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
                $list = $('#draft-list');
                break;
            case "saved-preprocess-receipt":
                $list = $('#saved-list');
                break;
            case "cancelled":
                $list = $('#cancelled-list');
                break;
            default:
                $list = $('#draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Manufacturing/PreProcessReceipt/GetPreProcessReceiptList?type=" + type;

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
                   { "data": "IssuedItem", "className": "IssuedItem" },
                   { "data": "IssuesUnit", "className": "IssuesUnit" },
                   { "data": "IssuedQty", "className": "IssuedQty" },
                   { "data": "ReceiptItem", "className": "ReceiptItem" },
                   { "data": "ReceiptItemUnit", "className": "ReceiptItemUnit" },
                   { "data": "ReceiptQuantity", "className": "ReceiptQuantity" },
                   { "data": "Process", "className": "Process" },
                   { "data": "QtyLoss", "className": "QtyLoss" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/PreProcessReceipt/Details/" + Id);

                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = PreProcessReceipt;
        fh_item = $("#preprocess-receipt-items-list").FreezeHeader();
        $(".cancel").on("click", self.cancel_confirm);
    },

    materials_list: function () {
        var $list = $('#tbl-unprocessed-materialpurification');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
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
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });
            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    bind_events: function () {
        var $currobj = PreProcessReceiptCRUD;
        var self = PreProcessReceipt;
        $.UIkit.autocomplete($('#preprocess-receipt-autocomplete'), { 'source': $currobj.get_item_AutoComplete, 'minLength': 1 });
        $('#preprocess-receipt-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_item_detail_autocomplete);

        $('#btnOKUnProcessedMaterialPurification').on('click', $currobj.set_item_detail_modal);

        $('body').on('click', '#btnAddItem', $currobj.add_item_clicked)
        $('body').on('click', '.btnSave', function () {
            $currobj.save_confirm(false);
        });
        $('body').on('click', '.btnSaveASDraft', function () {
            $currobj.save_preprocess_receipt(true);
        });
        $("body").on('click', ".cancel", self.cancel_confirm);

        $('body').on('click', '.removeItem', $currobj.remove_item_clicked); //dynamic generated html
    },

    cancel_confirm: function () {
        var self = PreProcessReceipt
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Manufacturing/PreProcessReceipt/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "MaterialPurificationReceipt"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Purification receipt cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Manufacturing/PreProcessReceipt/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },
    count_items: function () {
        var count = $('#preprocess-receipt-items-list tbody tr').length;
        $('#item-count').val(count);
    },
    validate_item: function () {
        var self = PreProcessReceipt;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_form: function () {
        var self = PreProcessReceipt;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
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
                elements: "#txtReceiptQty",
                rules: [
                    { type: form.required, message: "Invalid receipt quantity" },
                    { type: form.non_zero, message: "Invalid receipt quantity" },
                    { type: form.positive, message: "Invalid receipt quantity" },
                ]
            },
            {
                elements: "#txtReceiptDate",
                rules: [
                    { type: form.required, message: "Invalid date" },
                    { type: form.past_date, message: "Invalid date" },
                    {
                        type: function (element) {
                            var receiptdate = $("#txtReceiptDate").val();
                            var issuedate = $("#txtIssuedDate").val();

                            var date = receiptdate.split('-');
                            receiptdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            date = issuedate.split('-');
                            issuedate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            var count = 0;

                            return receiptdate >= issuedate

                        }, message: "Invalid receipt date"
                    },
                ]
            },
        ],
        on_submit: [
            {
                elements: "#txtTransDate",
                rules: [
                    { type: form.required, message: "Invalid date" },
                ]
            },
            {
                elements: "#preprocess-receipt-items-list tbody tr",
                rules: [
                    {
                        type: function (element) {
                            return $(element).length > 0;
                        }, message: "Please add atleast one item"
                    },
                ]
            },
            {
                elements: ".txtReceiptQuantity",
                rules: [
                    { type: form.required, message: "Invalid quantity" },
                    { type: form.non_zero, message: "Invalid quantity" },
                    { type: form.positive, message: "Invalid quantity" },
                ]
            },
            {
                elements: ".txtReceiptDate",
                rules: [
                    { type: form.required, message: "Invalid date" },
                    { type: form.past_date, message: "Invalid date" },
                    {
                        type: function (element) {
                            var receiptdate = $(element).val();
                            var row = $(element).closest('tr');
                            var issuedate = $(row).find(".txtIssuedDate").text();

                            var date = receiptdate.split('-');
                            receiptdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            date = issuedate.split('-');
                            issuedate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            var count = 0;

                            return receiptdate >= issuedate

                        }, message: "Invalid receipt date"
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
            {
                elements: ".txtReceiptQuantity",
                rules: [
                    {
                        type: function (element) {
                            var qtymet = clean($(element).closest('tr').find('.hdnBalanceQty').val());
                            var receipt = clean($(element).val());
                            return clean($(element).closest('tr').find('.hdnBalanceQty').val()) >= clean($(element).val());
                        }, message: "Receipt quantity should not be greater than pending quantity"
                    }
                ]
            },
        ],
    }
}

PreProcessReceiptCRUD = {
    createedit: function () {
        $currobj = this;

        $currobj.date = $('#hdnCurrDate').val();

        $currobj.SelectedItem = { ID: 0, PurificationIssueID: 0, ItemID: 0, ItemName: '', Unit: '', UnitID: 0, Quantity: 0, QtyMet: 0, BalanceQty: 0, ProcessID: 0, TransDate: '', TransTime: '', ProcessName: '' };

        this.remove_item_clicked = function () {
            $(this).parents('tr').remove();
            fh_item.resizeHeader();
            PreProcessReceipt.count_items();
        }

        this.txtqty_clicked = function (event) {
            if (event.which == 13) {    //Enter key pressed
                $currobj.add_item_clicked();
            }

        }

        this.save_confirm = function () {
            app.confirm_cancel("Do you want to Save", function () {
                $('.js-modal-confirm').hide();
                $currobj.save_preprocess_receipt();
            }, function () {
            })
        },


        this.save_preprocess_receipt = function (isDraft) {
            var error_count = PreProcessReceipt.validate_form();
            if (error_count > 0) {
                return;
            }
            var obj = $currobj.get_proprocess_receipt_save_obj(isDraft);
            var url;
            if (isDraft == true)
                url = 'SaveAsDraft';
            else
                url = 'Save';
            var saveCallback = function (response) {
                if (response.Result) {
                    app.show_notice(response.Message);
                    setTimeout(function () {
                        window.location = "/Manufacturing/PreProcessReceipt/";
                    }, 1000);
                }
                else {
                    $(".btnSave, .btnSaveASDraft,.cancel").css({ 'display': 'block' });
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                }
            }
            $(".btnSave, .btnSaveASDraft,.cancel").css({ 'display': 'none' });
            $currobj.ajaxRequest(url, obj, 'POST', saveCallback);

        }

        this.get_proprocess_receipt_save_obj = function (isDraft) {
            var obj = {};
            obj.TransNo = $('#txtTransNo').val();
            obj.TransDateStr = $('#txtTransDate').val();
            obj.ID = $('#hdnPreprocessReceiptID').val();
            obj.IsDraft = isDraft;
            obj.Error = '';
            var items = [];
            var isCompleted;

            $('#preprocess-receipt-items-list tbody tr').each(function () {
                if ($(this).closest('tr').find('.IsCompleted').prop("checked") == true) {
                    isCompleted = 'true';
                }
                else {
                    isCompleted = 'false';
                }
                var item = {
                    ID: $(this).find('.hdnID').val(),
                    MaterialPurificationIssueTransID: $(this).find('.hdnMaterialPurificationIssueTransID').val(),
                    IssuedItemID: $(this).find('.hdnIssuedItemID').val(),
                    IssuedQuantity: clean($(this).find('.txtIssuedQuantity').text()),
                    IssuedDateStr: $(this).find('.txtIssuedDate').text().trim(),
                    ReceiptItemID: $(this).find('.hdnReceiptItemID').val(),
                    ReceiptQuantity: clean($(this).find('.txtReceiptQuantity').val()),
                    ReceiptDateStr: $(this).find('.txtReceiptDate').val(),
                    ReceiptItemUnitID: $(this).find('.UnitID').val(),
                    IsCompleted: isCompleted,
                    BalanceQty: clean($(this).find('.hdnBalanceQty').val()),
                };
                items.push(item);
            });
            obj.PreProcessReceiptPurificationItemModelList = items;
            return obj;
        }

        this.get_item_AutoComplete = function (callback) {
            var search = $('#txtItemName').val();
            $currobj.ajaxRequest("GetUnProcessedMaterialPurificationIssueItemListAutoComplete", { search: search }, "GET", callback);
        }

        this.set_item_detail_modal = function () {
            var radio = $('#tbl-unprocessed-materialpurification tbody input[type="radio"]:checked');

            var currRow = radio.parents('tr');
            var id = $(currRow).find('.PurificationIssueTransID').val(); //item.id;
            var itemName = $(currRow).find('.hdnItemName').val(); //item.value;
            var itemID = $(currRow).find('.hdnItemID').val(); //item.itemid;
            var purificationIssueID = $(currRow).find('.hdnPurificationIssueID').val(); //item.purificationissueid;
            var unit = $(currRow).find('.hdnUnit').val(); //item.unit;
            var unitid = $(currRow).find('.hdnUnitID').val(); //item.unitid;
            var quantity = $(currRow).find('.hdnQuantity').val(); //item.quantity;
            var qtyMet = $(currRow).find('.hdnQtyMet').val(); //item.qtymet;
            var balanceQty = $(currRow).find('.hdnBalanceQty').val(); //item.balanceqty;
            var processID = $(currRow).find('.hdnProcessID').val(); //item.processid;
            var processName = $(currRow).find('.hdnProcessName').val(); //item.processname;
            var transDate = $(currRow).find('.hdnTransDate').val(); //item.transdate;
            var transtime = $(currRow).find('.hdnTransTime').val(); //item.transTime;
            var purified_item_id = $(currRow).find('.PurifiedItemID').val();
            var purified_item_name = $(currRow).find('.PurifiedItemName').val();
            var purified_item_unit = $(currRow).find('.PurifiedItemUnit').val();
            var remainingqty = (quantity - qtyMet) < 0 ? 0 : quantity - qtyMet

            UIkit.modal($('#unprocessed-materialpurification')).hide();
            $('#txtItemName').val(itemName);
            $('#txtUOM').val(unit);
            $('#txtActivity').val(processName);
            $('#txtIssuedQty').val(quantity);
            $('#txtIssuedDate').val(transDate);
            $('#txtReceiptQty').val(quantity);
            $('#txtReceiptDate').val($currobj.date);

            $('#ItemID').val(itemID);
            $('#PurifiedItemID').val(purified_item_id);
            $('#PurifiedItemName').val(purified_item_name);
            $('#PurifiedItemUnit').val(purified_item_unit);
            $('#PurificationIssueTransID').val(id);
            $('#QtyMet').val(qtyMet);
            $('#BalanceQty').val(balanceQty);

            $currobj.SelectedItem = {
                ID: id, PurificationIssueID: purificationIssueID, ItemID: itemID, ItemName: itemName, Unit: unit, UnitID: unitid, Quantity: quantity, PurifiedItemUnit: purified_item_unit,
                QtyMet: qtyMet, BalanceQty: balanceQty, ProcessID: processID, TransDate: transDate, TransTime: transtime, ProcessName: processName, RemainingQty: remainingqty
            };

            $('#txtIssuedQty').focus();
        }

        this.set_item_detail_autocomplete = function (event, item) {

            var id = item.id;
            var itemName = item.value;
            var itemID = item.itemid;
            var purificationIssueID = item.purificationissueid;
            var unit = item.unit;
            var unitid = item.unitid;
            var quantity = item.quantity;
            var qtyMet = item.qtymet;
            var balanceQty = item.balanceqty;
            var processID = item.processid;
            var processName = item.processname;
            var transDate = item.transdate;
            var transtime = item.transTime;
            var remainingqty = (quantity - qtyMet) < 0 ? 0 : quantity - qtyMet

            $('#txtUOM').val(unit);
            $('#txtActivity').val(processName);
            $('#RemainingQty').val(remainingqty)
            $('#issue-qty').val(remainingqty)
            $currobj.SelectedItem = {
                ID: id, PurificationIssueID: purificationIssueID, ItemID: itemID, ItemName: itemName, Unit: unit, UnitID: unitid, Quantity: quantity,
                QtyMet: qtyMet, BalanceQty: balanceQty, ProcessID: processID, TransDate: transDate, TransTime: transtime, ProcessName: processName, RemainingQty: remainingqty
            };
            $('#txtIssuedQty').focus();

            $('#txtItemName').val(itemName);
            $('#txtUOM').val(unit);
            $('#txtActivity').val(processName);
            $('#txtIssuedQty').val(quantity);
            $('#txtIssuedDate').val(transDate);
            $('#txtReceiptQty').val(quantity);
            $('#txtReceiptDate').val($currobj.date);

            $('#ItemID').val(itemID);
            $('#PurifiedItemID').val(item.purifiedItemId);
            $('#PurifiedItemName').val(item.purifiedItemName);
            $('#PurifiedItemUnit').val(item.purifiedItemUnit);
            $('#PurificationIssueTransID').val(item.id);
            $('#QtyMet').val(qtyMet);
            $('#BalanceQty').val(balanceQty);
        }

        this.add_item_clicked = function () {
            var error_count = PreProcessReceipt.validate_item();
            if (error_count > 0) {
                return;
            }
            var issueQty = clean($('#txtIssuedQty').val());
            var issueDate = $('#txtIssuedDate').val();
            var receiptQty = clean($('#txtReceiptQty').val());
            var receiptDate = $('#txtReceiptDate').val();
            var PurifiedItemID = clean($('#PurifiedItemID').val());
            var PurifiedItemName = $('#PurifiedItemName').val();
            var PurifiedItemUnit = $('#PurifiedItemUnit').val();
            var PurificationIssueTransID = clean($('#PurificationIssueTransID').val());
            var RemainingQty = $('#RemainingQty').val();
            var tblObj = $('#preprocess-receipt-items-list');
            var nxtSno = $currobj.get_next_row_sno(tblObj);
            var itemid_count = 0;
            var row;
            $("#preprocess-receipt-items-list tbody tr").each(function () {
                if ((clean($(this).find(".hdnIssuedItemID").val()) == $currobj.SelectedItem.ItemID) && (clean($(this).find(".hdnMaterialPurificationIssueTransID").val()) == PurificationIssueTransID)) {
                    itemid_count++;
                    row = $(this).closest("tr");
                }
            })
            if (itemid_count > 0) {
                app.confirm("Selected item already added ,Do you want to update issue quantity", function () {
                    var itemqty = clean($(row).find(".txtReceiptQuantity").val());
                    var pendingqty = clean($(row).find(".hdnBalanceQty").val());
                    receiptQty += itemqty;
                    if (receiptQty > pendingqty) {
                        app.show_error("Receipt quantity should not be greater than pending quantity");
                    }
                    else {
                        $(row).find(".txtReceiptQuantity").val(receiptQty);
                    }
                })
            }
            else {
                var tr = "<tr>"
                            + "<td>"
                            + nxtSno
                            + "</td>"
                            + "<td>"
                            + "<input type='hidden' class='hdnID' >"
                            + "<input type='hidden' class='hdnIssuedItemID' value='" + $currobj.SelectedItem.ItemID + "' >"
                            + "<input type='hidden' class='hdnReceiptItemID' value='" + PurifiedItemID + "' >"
                            + "<input type='hidden' class='hdnMaterialPurificationIssueTransID' value='" + PurificationIssueTransID + "' >"
                            + "<input type='hidden' class='hdnQtyMet' value='" + $currobj.SelectedItem.QtyMet + "' >"
                             + "<input type='hidden' class='UnitID' value='" + $currobj.SelectedItem.UnitID + "' >"
                            + "<input type='hidden' class='hdnBalanceQty' value='" + $currobj.SelectedItem.BalanceQty + "' >"
                            + $currobj.SelectedItem.ItemName
                            + "</td>"
                            + "<td>" + $currobj.SelectedItem.Unit + "</td>"
                            + "<td class='mask-production-qty txtIssuedQuantity'>" + $currobj.SelectedItem.Quantity + "</td>"
                            + "<td class='mask-production-qty'>" + $currobj.SelectedItem.BalanceQty + "</td>"
                            + "<td class='txtIssuedDate'>" + $currobj.SelectedItem.TransDate + "</td>"
                            + "<td>" + $currobj.SelectedItem.ProcessName + "</td>"
                            + "<td>" + PurifiedItemName + "</td>"
                            + "<td>" + PurifiedItemUnit + "</td>"
                            + "<td>" + "<input type='text' class='md-input mask-production-qty txtReceiptQuantity' value='" + receiptQty + "'></td>"
                            + "<td>"
                            + "<div class='uk-input-group'>"
                            + "<input type='text' class='md-input label-fixed past-date date txtReceiptDate' value='" + receiptDate + "'>"
                            + "<span class='uk-input-group-addon'><i class='uk-input-group-icon uk-icon-calendar past-date'></i></span>"
                            + "</div>"
                            + "</td>"
                            + "<td data-md-icheck><input type='checkbox' class='IsCompleted' />"
                            + "</td>"
                            + "<td><i class='uk-close uk-float-right removeItem'></i></td>"
                        + "</tr>"
                var $tr = $(tr);
                app.format($tr);
                $(tblObj).find('tbody').append($tr);
            }
            $currobj.SelectedItem = {};
            $('#txtItemName').val('');
            $('#txtUOM').val('');
            $('#txtIssuedQty').val('');
            $('#txtIssuedDate').val('');
            $('#txtActivity').val('');
            $('#txtReceiptQty').val('');
            $('#txtReceiptDate').val('');
            $('#PurifiedItemID').val('');
            $('#ItemID').val('');
            $('#PurifiedItemName').val('');
            $('#PurifiedUnit').val('');
            $('#PurificationIssueTransID').val('');
            fh_item.resizeHeader();
            PreProcessReceipt.count_items();
        }

        this.get_next_row_sno = function (tblObj) {
            return ($(tblObj).find('tbody tr').length + 1);
        }

        this.ajaxRequest = function (url, data, requestType, callBack) {
            $.ajax({
                url: $currobj.root_url() + url,
                cache: false,
                type: requestType,
                async: false,
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
            return "/Manufacturing/PreProcessReceipt/";
        }
    }

}