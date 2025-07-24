var fh_items;
ServiceItemReceipt = {
    init: function () {
        var self = ServiceItemReceipt;
        self.bind_events();
        self.freeze_headers();
        self.stock_issue_list();
        self.get_serial_no();
        self.get_issue();
        issue_select_table = $('#itempopup-list').SelectTable({
            selectFunction: self.remove_item_details,
            returnFocus: "#txtRequiredQty",
            modal: "#add-request",
            initiatingElement: "#IssueNo",
            startFocusIndex: 3,
            selectionType: "radio"
        });
    },
    list: function () {
        var self = ServiceItemReceipt;

        var $list = $('#stock-receipt-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Stock/ServiceItemReceipt/GetServiceItemReceiptList";

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
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "IssueLocation", "className": "IssueLocation" },
                    { "data": "IssuePremise", "className": "IssuePremise" },
                    { "data": "ReceiptLocation", "className": "ReceiptLocation" },
                    { "data": "ReceiptPremise", "className": "ReceiptPremise" },
                    { "data": "Amount", "className": "Amount" },
                ],
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/ServiceItemReceipt/Details/" + Id);
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
        var self = ServiceItemReceipt;
        self.freeze_headers();
    },

    freeze_headers: function () {
        fh_items = $("#stock-receipt-items-list").FreezeHeader();
    },

    get_issue: function () {
        var self = ServiceItemReceipt;
        self.error_count = 0;
        self.error_count = self.validate_issue();
        if (self.error_count > 0) {
            return;
        }
        var IssueLocationID = $("#IssueLocationID").val();
        var ReceiptLocationID = $("#ReceiptLocationID").val();
        var IssuePremiseID = $("#IssuePremiseID").val();
        var ReceiptPremiseID = $("#ReceiptPremiseID").val();
        $.ajax({
            url: '/Stock/ServiceItemIssue/GetUnProcessedServiceItemIssueList/',
            dataType: "json",
            type: "POST",
            data: {
                IssueLocationID: IssueLocationID,
                ReceiptLocationID: ReceiptLocationID,
                IssuePremiseID: IssuePremiseID,
                ReceiptPremiseID: ReceiptPremiseID,
            },
            success: function (response) {
                self.stock_issue_list_table.fnDestroy();

                var $stock_requisition_list = $('#itempopup-list tbody');
                $stock_requisition_list.html('');
                var tr = '';
                $.each(response, function (i, record) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1)
                        + "</td>"
                        + " <td class='uk-text-center' data-md-icheck>"
                        + "   <input type='radio' name='inputrequestno' id='requestno-id' class='spclCheckItem' value=" + record.ID + " />"
                        + "   <input type='hidden' class='IssueLocationID' value=" + record.IssueLocationID + " />"
                        + "   <input type='hidden' class='IssuePremiseID' value=" + record.IssuePremiseID + " />"
                        + "   <input type='hidden' class='ReceiptLocationID' value=" + record.ReceiptLocationID + " />"
                        + "   <input type='hidden' class='ReceiptPremiseID' value=" + record.ReceiptPremiseID + " />"
                        + "   <input type='hidden' class='NetAmount' value=" + record.NetAmount + " />"
                        + "</td>"
                        + "<td class='IssueNo'>" + record.IssueNo + "</td>"
                        + "<td>" + record.Date + "</td>"
                        + "<td>" + record.IssueLocationName + "</td>"
                        + "<td>" + record.IssuePremiseName + "</td>"
                        + "<td>" + record.ReceiptLocationName + "</td>"
                        + "<td>" + record.ReceiptPremiseName + "</td>"
                        + "<td>" + record.ProductionGroup + "</td>"
                    + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $stock_requisition_list.append($tr);
                });
                self.stock_issue_list();
            },

        });
    },

    get_serial_no: function () {

        var self = ServiceItemReceipt;
        var IssueStateID = $("#IssueLocationID option:selected").data("state-id");
        var ReceiptStateID = $("#LocationStateID").val();

        var IssueLocationID = $("#IssueLocationID").val();
        var ReceiptLocationID = $("#ReceiptLocationID").val();

        if (IssueLocationID == ReceiptLocationID) {
            self.ReceiptType = "IntraLocation";
        } else if (IssueStateID == ReceiptStateID) {
            self.ReceiptType = "IntraState";
        } else {
            self.ReceiptType = "InterState";
        }

        $.ajax({
            url: '/Stock/ServiceItemReceipt/GetSerialNo/',
            dataType: "json",
            type: "POST",
            data: { ReceiptType: self.ReceiptType },
            success: function (response) {
                if (response.Status == "success") {
                    $("#ReceiptNo").val(response.ReceiptNo);
                }
            }
        });
    },

    stock_issue_list: function () {
        var self = ServiceItemReceipt;
        var $list = $('#itempopup-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            self.stock_issue_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    select_stock_issue: function () {
        var self = ServiceItemReceipt;
        var stock_issue_ids = $("#itempopup-list .spclCheckItem:checked").map(function () {
            return $(this).val();
        }).get();

        var row = $("#itempopup-list .spclCheckItem:checked").closest("tr");
        $("#IssuePremiseID").val($(row).find(".IssuePremiseID").val());
        $("#ReceiptPremiseID").val($(row).find(".ReceiptPremiseID").val());
        $("#NetAmount").val($(row).find(".NetAmount").val());
        $("#IssueNo").val($(row).find(".IssueNo").text());
        if (stock_issue_ids.length == 0) {
            app.show_error('Please select atleast one issue');
            return;
        }
        $.ajax({
            url: '/Stock/StockIssue/GetStockIssueForReceipt/',
            dataType: "json",
            data: {
                Areas: 'Stock',
                StockIssueIDS: stock_issue_ids,
            },
            type: "POST",
            success: function (stock_issue_items) {
                var $stock_issue_items_list = $('#stock-issue-items-list tbody');
                $stock_issue_items_list.html('');
                var tr = '';
                var gstpercent, gstamount;
                $.each(stock_issue_items, function (i, stock_issue_item) {

                    gstamount = stock_issue_item.IGSTAmount + stock_issue_item.CGSTAmount + stock_issue_item.SGSTAmount;
                    gstpercent = stock_issue_item.IGSTPercentage + stock_issue_item.CGSTPercentage + stock_issue_item.SGSTPercentage;
                    tr += " <tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "     <input type='hidden' class='stock-issue-trans-id' value='" + stock_issue_item.StockIssueTransID + "'/>"
                        + "     <input type='hidden' class='stock-issue-id' value='" + stock_issue_item.StockIssueID + "'/>"
                        + "     <input type='hidden' class='item-id' value='" + stock_issue_item.ID + "'/>"
                        + "     <input type='hidden' class='Rate' value='" + stock_issue_item.Rate + "'/>"
                        + "     <input type='hidden' class='NetAmount' value='" + stock_issue_item.NetAmount + "'/>"
                        + "     <input type='hidden' class='IGSTPercentage' value='" + stock_issue_item.IGSTPercentage + "'/>"
                        + "     <input type='hidden' class='CGSTPercentage' value='" + stock_issue_item.CGSTPercentage + "'/>"
                        + "     <input type='hidden' class='SGSTPercentage' value='" + stock_issue_item.SGSTPercentage + "'/>"
                        + "     <input type='hidden' class='IGSTAmount' value='" + stock_issue_item.IGSTAmount + "'/>"
                        + "     <input type='hidden' class='CGSTAmount' value='" + stock_issue_item.CGSTAmount + "'/>"
                        + "     <input type='hidden' class='SGSTAmount' value='" + stock_issue_item.SGSTAmount + "'/>"
                        + "     <input type='hidden' class='UnitID' value='" + stock_issue_item.UnitID + "'/>"
                        + "</td>"
                        + "<td>" + stock_issue_item.Name + "</td>"
                        + "<td>" + stock_issue_item.Unit + "</td>"
                        + "<td class='mask-production-qty issueqty'>" + stock_issue_item.IssueQty + "</td>"
                        + '<td class="mask-production-qty">' + stock_issue_item.IssueQty + '</td>'
                        + '<td class="mask-production-qty BasicPrice">' + stock_issue_item.BasicPrice + '</td>'
                        + '<td class="mask-production-qty GrossAmount">' + stock_issue_item.GrossAmount + '</td>'
                        + '<td class="mask-production-qty TradeDiscountPercent">' + stock_issue_item.TradeDiscountPercentage + '</td>'
                        + '<td class="mask-production-qty TradeDiscount">' + stock_issue_item.TradeDiscount + '</td>'
                        + '<td class="mask-production-qty TaxableAmount">' + stock_issue_item.TaxableAmount + '</td>'
                        + '<td class="mask-production-qty GSTPercent">' + gstpercent + '</td>'
                        + '<td class="mask-production-qty GSTAmount">' + gstamount + '</td>'
                        + '<td class="mask-production-qty ">' + stock_issue_item.NetAmount + '</td>'
                        + "</tr>";
                });
                var $tr = $(tr);
                app.format($tr);
                $stock_issue_items_list.append($tr);
                self.count_items();
            },
        });
    },

    count_items: function () {
        var count = $('#stock-issue-items-list  tbody tr').length;
        $('#item-count').val(count);
    },

    bind_events: function () {
        var self = ServiceItemReceipt;
        $(".btnSaveAndNew").on("click", self.save_and_new);
        $(".btnSave").on("click", self.save_confirm);
        $('body').on('change', "#IssueLocationID, #ReceiptPremiseID, #IssuePremiseID", self.update_stock_issue_list);
        $("#btnOkStockRequisitionList").on("click", self.remove_item_details);
        $("body").on("change", "#IssueLocationID", self.get_issue_premises);
    },
    get_issue_premises: function () {
        var self = ServiceItemReceipt;
        var location_id = $("#IssueLocationID").val();
        if (location_id == null || location_id == "") {
            return;
        }
        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                $("#IssuePremiseID").html("");
                if (response.Status == "success") {
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    });
                    $("#IssuePremiseID").append(html);
                }
            }
        });
    },

    remove_item_details: function () {
        var self = ServiceItemReceipt;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#stock-receipt-items-list tbody').empty();
                self.select_stock_issue();
            })
        }
        else {
            self.select_stock_issue();
        }
    },

    update_stock_issue_list: function () {
        var self = ServiceItemReceipt;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#stock-issue-items-list tbody').empty();
                self.count_items();
            })
        }
        self.get_issue();
    },

    save_confirm: function () {
        var self = ServiceItemReceipt
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save(false);
        }, function () {
        })
    },
    save_and_new: function () {
        var self = ServiceItemReceipt
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save(true);
        }, function () {
        })
    },

    save: function (IsSaveandNew) {
        var self = ServiceItemReceipt;
        var model = self.get_data();
        var location = "/Stock/ServiceItemReceipt/Index";
        if (IsSaveandNew==true) {
            location = "/Stock/ServiceItemReceipt/Create";
        }

        $(".btnSaveAndNew, .btnSave").css({ 'display': 'none' });
        $.ajax({
            url: '/ServiceItemReceipt/Save',
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Stock receipt created successfully");
                    setTimeout(function () {
                        window.location = location
                    }, 1000);
                } else {
                    app.show_error("Failed to create Stock receipt.");
                    $(".btnSaveAndNew, .btnSave").css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function () {
        var self = ServiceItemReceipt;
        var model = {
            ReceiptNo: $("#ReceiptNo").val(),
            Date: $("#Date").val(),
            ReceiptType: self.ReceiptType,
            NetAmount: clean($("#NetAmount").val())
        };
        model.Item = self.GetProductList();
        return model;
    },

    GetProductList: function () {
        var ProductsArray = [];
        $("#stock-issue-items-list tbody tr").each(function () {
            ProductsArray.push(
                {
                    StockIssueTransID: $(this).find('.stock-issue-trans-id').val(),
                    StockIssueID: $(this).find('.stock-issue-id').val(),
                    ItemID: $(this).find('.item-id').val(),
                    IssueQty: clean($(this).find('.issueqty').val()),
                    ReceiptQty: clean($(this).find('.issueqty').val()),
                    IssueLocationID: $("#IssueLocationID").val(),
                    IssuePremiseID: $("#IssuePremiseID").val(),
                    ReceiptLocationID: $("#ReceiptLocationID").val(),
                    ReceiptPremiseID: $("#ReceiptPremiseID").val(),
                    Rate: clean($(this).find('.Rate').val()),
                    NetAmount: clean($(this).find('.NetAmount').val()),
                    GrossAmount: clean($(this).find('.GrossAmount').val()),
                    TradeDiscountPercent: clean($(this).find('.TradeDiscountPercent').val()),
                    TradeDiscount: clean($(this).find('.TradeDiscount').val()),
                    TaxableAmount: clean($(this).find('.TaxableAmount').val()),
                    IGSTPercentage: clean($(this).find('.IGSTPercentage').val()),
                    CGSTPercentage: clean($(this).find('.CGSTPercentage').val()),
                    SGSTPercentage: clean($(this).find('.SGSTPercentage').val()),
                    IGSTAmount: clean($(this).find('.IGSTAmount').val()),
                    CGSTAmount: clean($(this).find('.CGSTAmount').val()),
                    SGSTAmount: clean($(this).find('.SGSTAmount').val()),
                    BasicPrice: clean($(this).find('.BasicPrice').val()),
                    UnitID: clean($(this).find('.UnitID').val()),
                }
            );
        })
        return ProductsArray;
    },

    validate_issue: function () {
        var self = ServiceItemReceipt;
        if (self.rules.on_issue.length) {
            return form.validate(self.rules.on_issue);
        }
        return 0;
    },

    validate_form: function () {
        var self = ServiceItemReceipt;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_issue: [
            {
                elements: "#IssueLocationID",
                rules: [
                    { type: form.required, message: "Please select Issue Location" },
                    { type: form.numeric, message: "Please select Issue Location" },
                ]
            },

        ],
        on_submit: [
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