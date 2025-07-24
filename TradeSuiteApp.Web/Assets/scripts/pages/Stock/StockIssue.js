var fh_items;
stockissue = {
    init: function () {
        var self = stockissue;
        self.stock_request_list();
        item_list = self.available_stock_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRequiredQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        self.ReceiptLocationID = $("#ReceiptLocationID").val();
        self.ReceiptPremiseID = $("#ReceiptPremiseID").val();
        self.IssueType = "";
        self.update_form();
        self.get_requisitions();
        self.freeze_headers();
        self.RequisitionIDs = [];
        self.RequisitionNos = [];
        //if ($("#ID").val() != 0) {
        self.get_serial_no();
        self.PackingDetails = [];
        // }
    },

    update_form: function () {
        var self = stockissue;
        var ReceiptStateID = $("#ReceiptLocationID option:selected").data("state-id");

        self.default_batch_type_id = 1;
        if (ReceiptStateID != 32) {
            self.default_batch_type_id = 2;
        }
        $("#BatchTypeID").val(self.default_batch_type_id);

        if (self.is_gst_effect()) {
            $(".inter-branch").removeClass("uk-hidden");
        } else {
            $(".inter-branch").addClass("uk-hidden");
        }

    },
    available_stock_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAvailableStockItemsList";

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
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "WarehouseID", Value: $('#WarehouseID').val() },
                            { Key: "BatchTypeID", Value: $('#BatchTypeID').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='Stock' value='" + row.Stock + "'>"
                                + "<input type='hidden' class='BatchTypeID' value='" + row.BatchTypeID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='Rate' value='" + row.Rate + "'>"
                                + "<input type='hidden' class='CGSTPercentage' value='" + row.CGSTPercentage + "'>"
                                + "<input type='hidden' class='IGSTPercentage' value='" + row.IGSTPercentage + "'>"
                                + "<input type='hidden' class='SGSTPercentage' value='" + row.SGSTPercentage + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "PartsNo", "className": "PartsNo" },
                    { "data": "Make", "className": "Make" },
                    { "data": "Model", "className": "Model" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Rate + "</div>";
                        }
                    },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Stock + "</div>";
                        }
                    },
                ]
                ,
                createdRow: function (row, data, index) {
                    $(row).addClass((data.BatchTypeName).toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on('change', '#ItemCategoryID, #WarehouseID, #BatchTypeID', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    get_serial_no: function () {

        var self = stockissue;
        var ReceiptStateID = $("#ReceiptLocationID option:selected").data("state-id");
        var IssueStateID = $("#LocationStateID").val();
        var IssueLocationID = $("#IssueLocationID").val();
        var ReceiptLocationID = $("#ReceiptLocationID").val();

        if (IssueLocationID == ReceiptLocationID) {
            self.IssueType = "IntraLocation";
        } else if (IssueStateID == ReceiptStateID) {
            self.IssueType = "IntraState";
        } else {
            self.IssueType = "InterState";
        }

        $.ajax({
            url: '/Stock/StockIssue/GetSerialNo/',
            dataType: "json",
            type: "POST",
            data: { IssueType: self.IssueType },
            success: function (response) {
                if (response.Status == "success") {
                    $("#IssueNo").val(response.IssueNo);
                }
            }
        });
    },

    is_gst_effect: function () {
        return true; // $("#LocationStateID").val() != $("#ReceiptLocationID option:selected").data("state-id");
    },

    is_igst: function () {
        return $("#LocationStateID").val() != $("#ReceiptLocationID option:selected").data("state-id");
    },

    details: function () {
        var self = stockissue;
        self.update_form();
        self.freeze_headers();
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
    },

    print: function () {
        var self = stockissue;
        $.ajax({
            url: '/Stock/StockIssue/Print',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    SignalRClient.print.text_file(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },

    printpdf: function () {
        var self = stockissue;
        $.ajax({
            url: '/Reports/Stock/StockIssuePrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },

    freeze_headers: function () {
        fh_items = $("#stock-issue-items-list").FreezeHeader();

        $('#tabs-issue[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                fh_items.resizeHeader();
            }, 500);
        });
    },

    stock_request_list: function () {
        var self = stockissue;
        $list = $('#requisition-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            self.stock_request_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                self.stock_request_list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    list: function () {
        var self = stockissue;
        $('#tabs-stock-issue').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = stockissue;
        // types
        //1. Draft.
        //2. To be Received.
        //3. Fully Received.
        //4. Cancelled.

        var $list;

        switch (type) {
            case "draft":
                $list = $('#stock-issue-draft-list');
                break;
            case "to-be-received":
                $list = $('#stock-issue-to-be-received-list');
                break;
            case "fully-received":
                $list = $('#stock-issue-fully-received-list');
                break;
            case "cancelled":
                $list = $('#stock-issue-cancelled-list');
                break;
            default:
                $list = $('#stock-issue-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Stock/StockIssue/GetIssueList?type=" + type;

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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "IssueLocation", "className": "IssueLocation" },
                    { "data": "IssuePremise", "className": "IssuePremise" },
                    { "data": "ReceiptLocation", "className": "ReceiptLocation" },
                    { "data": "ReceiptPremise", "className": "ReceiptPremise" },
                    {
                        "data": "Amount", "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        },
                    },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/StockIssue/Details/" + Id);
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
        var self = stockissue;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': stockissue.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', stockissue.set_item_details);
        $("#btnOKItem").on("click", self.select_item);

        $("#btnOkStockRequisitionList").on("click", self.select_stock_requisition);

        $("body").on("change", "#IssuePremiseID, #ReceiptLocationID, #ReceiptPremiseID", self.get_requisitions);
        $("body").on("change", "#IssuePremiseID", self.on_change_issue_premise);
        $("body").on("change", "#ReceiptLocationID", self.on_change_receipt_location);
        $("body").on("change", "#ReceiptPremiseID", self.on_change_receipt_premise);

        $("body").on("keyup", "#RequestNo", self.show_stock_requisitions);
        $("body").on("click", ".show-stock-requisitions", self.show_stock_requisitions);

        $("body").on('click', "#btnAddProduct", self.add_item);
        $("body").on("click", ".btnSaveAsDraft", self.save_draft);
        $("body").on("click", ".btnSave,.btnSaveAndNew", self.save_confirm);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on('click', ".cancel", self.cancel_confirm);

        $('body').on('click', '#btnOkBatches', self.replace_batches);
        $('body').on('click', '#stock-issue-items-list tbody td:not(.action)', self.show_batch_edit_modal);
        $("body").on("keyup change", "#batch-list .IssueQty", self.set_total_quantity);

        $("body").on("ifChanged", "#requisition-list .RequisitionID", self.choose_requisition);
        $("body").on("click", "#btn-download-template", self.download_template);
        $("body").on("change", "#BatchType", self.change_batch_type);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
    },

    change_batch_type: function () {
        $("#BatchTypeID").val($(this).val()).trigger("change");
    },

    //check_grid_length:function()
    //{
    //    var self = stockissue;
    //    self.upload_file();
    //    if ($("#stock-issue-items-list tr").length > 0) {
    //        app.confirm_cancel("Selected Items will be removed", function () {
    //            self.upload_file();
    //        }, function () {
    //        })
    //    }
    //    else
    //    {
    //        self.upload_file();
    //    }
    //},
    download_template: function () {
        var self = stockissue;
        self.error_count = 0;
        self.error_count = self.validate_on_download_template();
        if (self.error_count > 0) {
            return;
        }
        var Category = $("#ItemCategoryID option:selected").text();
        var WarehouseID = $("#IssuePremiseID").val();
        window.location = "/Reports/Stock/GetIssueTemplate/?Category=" + Category + "&WarehouseID=" + WarehouseID;
    },
    upload_file: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(xls|xlsx)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File Extension Is InValid - Only Upload EXCEL File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            var self = stockissue;

            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 20;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-file').html("<a class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</a>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
                self.populate_uploaded_template();
            }
            if (failure != "") {
                $("#preloader").hide();
                altair_helpers.content_preloader_hide();
                app.show_error(failure);
            }
            $("#select-file").val("");
        }
    },

    populate_uploaded_template: function () {
        var self = stockissue;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Stock/StockIssue/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var items = [];

                if (response.Status == "success") {
                    $("#stock-issue-items-list tbody").html("");
                    $.each(response.Data, function (i, item) {
                        if (item.Remarks == null) {
                            item.Remarks = "";
                        }

                        items.push(item);
                    });
                    self.get_items_to_grid(items);
                }

                $("#preloader").hide();
                altair_helpers.content_preloader_hide();

            }
        });
    },
    get_items_to_grid: function (items) {
        var self = stockissue;
        $.ajax({
            url: '/Stock/StockIssue/GetItemsToGrid/',
            data: {
                model: items,
                IssuePremiseID: $("#IssuePremiseID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.add_items_to_grid(response.Data, 0);
                } else {
                    app.show_error(response.Message)
                }
            },
        });
    },
    save_confirm: function () {
        var self = stockissue;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            IsDraft = false;
            self.save();
        }, function () {
        })
    },

    save_draft: function () {
        var self = stockissue;
        self.error_count = 0;
        self.error_count = self.validate_form_for_draft();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveAsDraft")) {
            IsDraft = true;
            self.save();
        }
    },

    cancel_confirm: function () {
        var self = stockissue
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    choose_requisition: function () {
        var self = stockissue;
        var RequisitionID = $(this).val();
        var RequisitionNo = $(this).closest("tr").find(".RequestNo").text().trim();
        self.RequisitionIDs = [];
        self.RequisitionNos = [];
        self.RequisitionIDs.push(RequisitionID);
        self.RequisitionNos.push(RequisitionNo);

        $("#IssuePremiseID").val($(this).closest("tr").find(".IssuePremiseID").val());
        $("#ReceiptPremiseID").val($(this).closest("tr").find(".ReceiptPremiseID").val());
    },

    replace_batches: function () {
        var self = stockissue;
        if (self.validate_batch() > 0) {
            return;
        }
        UIkit.modal($('#batch-edit')).hide();
        var ItemID = $('#BatchItemID').val();
        var item = {};
        var items = [];

        $("#batch-list tbody tr").each(function () {
            if (clean($(this).find('.IssueQty').val()) > 0) {
                item = {
                    ItemID: ItemID,
                    Name: $('#BatchItemName').val().trim(),
                    BatchTypeID: clean($(this).find('.BatchTypeID').val()),
                    BatchType: $(this).find('.BatchType').val(),
                    BatchID: $(this).find('.BatchID').val(),
                    BatchName: $(this).find('.BatchName').text().trim(),
                    Unit: $(this).find('.Unit').val(),
                    IssueQty: clean($(this).find('.IssueQty').val()),
                    Stock: clean($(this).find('.Stock').text()),
                    RequestTransID: clean($(this).find('.StockRequisitionTransID').val()),
                    RequestID: clean($(this).find('.StockRequisitionID').val()),
                    StockRequisitionNo: $(this).find('.StockRequisitionNo').val(),
                    RequestedQty: clean($(this).find('.RequestedQty').val()),
                    Rate: clean($(this).find('.Rate').text()),
                    SGSTPercentage: clean($(this).find('.SGSTPercentage').val()),
                    CGSTPercentage: clean($(this).find('.CGSTPercentage').val()),
                    IGSTPercentage: clean($(this).find('.IGSTPercentage').val()),
                    TradeDiscountPercentage: clean($(this).find('.TradeDiscountPercentage').val()),
                    UnitID: clean($(this).find('.UnitID').val()),
                    PackSize: $(this).find('.PackSize').val(),
                    PrimaryUnit: $(this).find('.PrimaryUnit').val(),
                    PrimaryUnitID: $(this).find('.PrimaryUnitID').val(),
                };
                items.push(item);
            }
        });
        var index = $("#stock-issue-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr").eq(0).index();
        $("#stock-issue-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr").remove();
        self.add_items_to_grid(items, index);
    },

    set_total_quantity: function () {
        var self = stockissue;
        var TotalIssueQty = 0;
        $("#batch-list .IssueQty").each(function () {
            TotalIssueQty += clean($(this).val());
        });
        $("#batch-edit .TotalIssueQty").val(TotalIssueQty);
    },

    show_batch_edit_modal: function () {
        var self = stockissue;
        var tr = "";
        var $tr;
        var RequestedQty = 0;
        var TotalRequestedQty = 0;
        var TotalStock = 0;
        var TotalIssueQty = 0;
        var RequestTransID = -1;
        var RequestTransIDs = [];
        var row = $(this).closest('tr');

        var ItemID = $(row).find('.ItemID').val();

        $("#stock-issue-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr").each(function () {
            row = $(this).closest('tr');
            if (RequestTransID != $(row).find(".RequestTransID").val()) {
                TotalRequestedQty += clean($(row).find(".RequestedQty").text());
                RequestTransID = $(row).find(".RequestTransID").val();
                RequestTransIDs.push($(row).find(".RequestTransID").val());
            }
        });
        var item = {
            ItemID: $(row).find('.ItemID').val(),
            Name: $(row).find('.ItemName').text().trim(),
            Unit: $(row).find('.Unit').text(),
            BatchName: $(row).find('.BatchName').text().trim(),
            BatchType: $(row).find('.BatchType').text(),
            BatchTypeID: clean($(row).find('.BatchTypeID').val()),
            IssueQty: clean($(row).find('.IssueQty').val()),
            StockRequestTransID: clean($(row).find('.RequestTransID').val()),
            StockRequestID: clean($(row).find('.RequestID').val()),
            RequestedQty: clean($(row).find('.RequestedQty').val()),
            Rate: clean($(row).find('.Rate').text()),
            SGSTPercentage: clean($(row).find('.SGSTPercentage').val()),
            CGSTPercentage: clean($(row).find('.CGSTPercentage').val()),
            IGSTPercentage: clean($(row).find('.IGSTPercentage').val()),
            TradeDiscountPercentage: clean($(row).find('.TradeDiscountPercentage').val()),
            UnitID: clean($(row).find('.UnitID').val()),
            PackSize: $(row).find('.PackSize').val(),
            PrimaryUnit: $(row).find('.PrimaryUnit').val(),
            PrimaryUnitID: $(row).find('.PrimaryUnitID').val(),
        }

        $("#BatchItemID").val(item.ItemID);
        $("#BatchItemName").val(item.Name);
        $("#BatchQty").val(TotalRequestedQty);

        $.ajax({
            url: '/Masters/Batch/GetAvailableBatchesForStockIssue',
            dataType: "json",
            data: {
                ItemID: ItemID,
                StockIssueID: $("#ID").val(),
                WarehouseID: $('#IssuePremiseID').val(),
                BatchTypeID: item.BatchTypeID,
                RequiredQty: TotalRequestedQty,
                RequestTransIDs: RequestTransIDs,
                UnitID: item.UnitID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Batches).each(function (i, record) {
                        TotalStock += record.Stock;
                        TotalIssueQty += record.IssueQty;
                        tr += '<tr>'
                            + '<td>' + (i + 1)
                            + '     <input type="hidden" class="BatchID" value="' + record.BatchID + '" />'
                            + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                            + '     <input type="hidden" class="BatchType" value="' + item.BatchType + '" />'
                            + '     <input type="hidden" class="Unit" value="' + item.Unit + '" />'
                            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                            + '     <input type="hidden" class="StockRequisitionNo" value="' + record.StockRequisitionNo + '" />'
                            + '     <input type="hidden" class="StockRequisitionID" value="' + record.StockRequisitionID + '" />'
                            + '     <input type="hidden" class="StockRequisitionTransID" value="' + record.StockRequisitionTransID + '" />'
                            + "     <input type='hidden' class='CGSTPercentage' value='" + item.CGSTPercentage + "' />"
                            + "     <input type='hidden' class='SGSTPercentage' value='" + item.SGSTPercentage + "' />"
                            + "     <input type='hidden' class='IGSTPercentage' value='" + item.IGSTPercentage + "' />"
                            + "     <input type='hidden' class='RequestedQty' value='" + item.RequestedQty + "' />"
                            + "     <input type='hidden' class='PackSize' value='" + item.PackSize + "' />"
                            + "     <input type='hidden' class='PrimaryUnit' value='" + item.PrimaryUnit + "' />"
                            + "     <input type='hidden' class='PrimaryUnitID' value='" + item.PrimaryUnitID + "' />"
                            + "     <input type='hidden' class='TradeDiscountPercentage' value='" + item.TradeDiscountPercentage + "' />"
                            + '</td>'
                            + '<td class="BatchName">' + record.BatchNo + '</td>'
                            + '<td class="BatchTypeName batch_type">' + record.BatchTypeName + '</td>'
                            + '<td class="Stock mask-qty">' + record.Stock + '</td>'
                            + '<td class="mask-currency Rate">' + record.Rate + '</td>'
                            + '<td>' + record.ExpiryDateStr + '</td>'
                            + '<td><input type="text" class="md-input mask-qty IssueQty" value = "' + record.IssueQty + '" /></td>'
                            + '<tr>';
                    });
                    var colspan = 3;
                    if ($(".batch_type:visible").length == 0) {
                        colspan = 2;
                    }
                    tr += '<tr>'
                        + '<td colspan="' + colspan + '"><b>Total</b></td>'
                        + '<td class="mask-qty">' + TotalStock + '</td>'
                        + '<td colspan="2"></td>'
                        + '<td><input type="text" class="md-input mask-qty TotalIssueQty" disabled value = "' + TotalIssueQty + '" /></td>'
                        + '<tr>';
                    $tr = $(tr);
                    app.format($tr);
                    $('#batch-list tbody').html($tr);
                    $('#show-batch-edit').trigger('click');
                }
            }
        });

    },

    search_in_items: function (ItemID, BatchID, RequisitionID) {
        var row = $("#stock-issue-items-list tbody").find(".ItemID[value='" + ItemID + "']").closest("tr");
    },

    cancel: function () {
        $(".btnSavePrNew, .btnSavePr,.btnSavedraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Stock/StockIssue/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "StockIssue"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Stock Issue cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Stock/StockIssue/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSavePrNew, .btnSavePr,.btnSavedraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },

    clear_grid: function () {
        var self = stockissue;
        $('#stock-issue-items-list tbody').empty();
        self.count_items();
        $("#GrossAmount").val(0);
        $("#TradeDiscount").val(0);
        $("#TaxableAmount").val(0);
        $("#SGSTAmount").val(0);
        $("#CGSTAmount").val(0);
        $("#IGSTAmount").val(0);
        $("#RoundOff").val(0);
        $("#NetAmount").val(0);
    },

    on_change_issue_premise: function () {
        var self = stockissue;
        var warehouse_id;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                warehouse_id = $("#IssuePremiseID").val();
                $("#WarehouseID").val(warehouse_id).trigger('change');
                self.clear_grid();
            }, function () {
                $("#IssuePremiseID").val($("#WarehouseID").val());
            });
        }
        else {
            warehouse_id = $("#IssuePremiseID").val();
            $("#WarehouseID").val(warehouse_id).trigger('change');
        }
    },

    on_change_receipt_premise: function () {
        var self = stockissue;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                self.clear_grid();
                self.ReceiptPremiseID = $("#ReceiptPremiseID").val();
            }, function () {
                $("#ReceiptPremiseID").val(self.ReceiptPremiseID);
            });
        }
        else {
            self.ReceiptPremiseID = $("#ReceiptPremiseID").val();
        }
    },

    on_change_receipt_location: function () {
        var self = stockissue;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                self.clear_grid();
                self.ReceiptLocationID = $("#ReceiptLocationID").val();
                self.get_receipt_premises();
                self.update_form();
                self.get_serial_no();
                setTimeout(function () { fh_items.resizeHeader(); }, 200);
            }, function () {
                $("#ReceiptLocationID").val(self.ReceiptLocationID);
            });
        }
        else {
            self.ReceiptLocationID = $("#ReceiptLocationID").val();
            self.get_receipt_premises();
            self.update_form();
            self.get_serial_no();
            setTimeout(function () { fh_items.resizeHeader(); }, 200);
        }
    },

    get_receipt_premises: function () {
        var self = stockissue;
        var location_id = $("#ReceiptLocationID").val();
        if (location_id == null || location_id == "") {
            return;
        }
        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                $("#ReceiptPremiseID").html("");
                if (response.Status == "success") {
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    });
                    $("#ReceiptPremiseID").append(html);
                }
            }
        });
    },

    show_stock_requisitions: function () {
        var self = stockissue;
        self.error_count = 0;
        self.error_count = self.validate_requisition();
        if (self.error_count > 0) {
            return;
        }
        $("#stock-requisitions").trigger("click");
    },

    get_requisitions: function () {
        var self = stockissue;

        self.RequisitionIDs = [];

        var IssueLocationID = $("#IssueLocationID").val();
        var ReceiptLocationID = $("#ReceiptLocationID").val();
        var IssuePremiseID = $("#IssuePremiseID").val();
        var ReceiptPremiseID = $("#ReceiptPremiseID").val();

        $.ajax({
            url: '/Stock/StockRequest/GetStockRequisitionList/',
            dataType: "json",
            type: "POST",
            data: {
                IssueLocationID: IssueLocationID,
                ReceiptLocationID: ReceiptLocationID,
                IssuePremiseID: IssuePremiseID,
                ReceiptPremiseID: ReceiptPremiseID,
            },
            success: function (response) {
                if (response.Status == "success") {
                    self.stock_request_list_table.fnDestroy();
                    var $stock_requisition_list = $('#requisition-list tbody');
                    $stock_requisition_list.html('');
                    var tr = '';
                    $.each(response.Data, function (i, stock_requisition) {
                        tr += "<tr>"
                            + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                            + "<td class='uk-text-center' data-md-icheck>"
                            + "  <input type='radio' name='RequisitionID' class='RequisitionID' value=" + stock_requisition.ID + " />"
                            + "  <input type='hidden' class='IssueLocationID' value=" + stock_requisition.IssueLocationID + " />"
                            + "  <input type='hidden' class='IssuePremiseID' value=" + stock_requisition.IssuePremiseID + " />"
                            + "  <input type='hidden' class='ReceiptLocationID' value=" + stock_requisition.ReceiptLocationID + " />"
                            + "  <input type='hidden' class='ReceiptPremiseID' value=" + stock_requisition.ReceiptPremiseID + " />"
                            + "</td>"
                            + "<td class='RequestNo'>" + stock_requisition.RequestNo + "</td>"
                            + "<td>" + stock_requisition.Date + "</td>"
                            + "<td>" + stock_requisition.IssueLocationName + "</td>"
                            + "<td>" + stock_requisition.IssuePremiseName + "</td>"
                            + "<td>" + stock_requisition.ReceiptLocationName + "</td>"
                            + "<td>" + stock_requisition.ReceiptPremiseName + "</td>"
                            + "<td>" + stock_requisition.ProductionGroup + "</td>"
                            + "<td>" + stock_requisition.Batch + "</td>"
                            + "</tr>";
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $stock_requisition_list.append($tr);
                    self.stock_request_list();
                }
            },
        });
    },

    remove_item: function () {
        var self = stockissue;
        $(this).closest("tr").remove();
        $("#stock-issue-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#stock-issue-items-list tbody tr").length);
        self.update_form_summary();
        $('packing-detail-list').html('');
        self.set_packing_details();
        fh_items.resizeHeader(false);
    },

    select_item: function () {
        var self = stockissue;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Category = $(row).find(".ItemCategory").val();
        var primaryUnit = $(row).find(".PrimaryUnit").val();
        var primaryUnitID = $(row).find(".PrimaryUnitID").val();
        var inventoryUnit = $(row).find(".InventoryUnit").val();
        var inventoryUnitID = $(row).find(".InventoryUnitID").val();
        var SecondaryUnits = $(row).find(".SecondaryUnits").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Category").val(Category);

        $("#PrimaryUnit").val(primaryUnit);
        $("#PrimaryUnitID").val(primaryUnitID);
        $("#InventoryUnit").val(inventoryUnit);
        $("#InventoryUnitID").val(inventoryUnitID);
        $("#txtRequiredQty").focus();
        if (($("#Category").val() == "Finished Goods")) {
            $("#BatchTypeID").val(self.default_batch_type_id);
        }
        UIkit.modal($('#select-item')).hide();
        self.get_units();
        self.SetSecondaryUnits(primaryUnit, SecondaryUnits);
    },
    SetSecondaryUnits: function (Unit, SecondaryUnits) {
        var optionsSecondaryUnits = SecondaryUnits.split(',');
        var options = '<option value="1" selected>' + Unit + '</option>';
        optionsSecondaryUnits.forEach(function (option) {
            var parts = option.split('|');
            if (parts.length > 1) {
                var text = parts[0];
                var value = parts[1];
                options += '<option value="' + value + '">' + text + '</option>';
            }
        });
        $("#UnitID").empty().append(options);
    },
    get_units: function () {
        var self = stockissue;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#InventoryUnitID").val() + "'>" + $("#InventoryUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);

    },


    count_items: function () {
        var count = $('#stock-issue-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    add_item: function () {
        var self = stockissue;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var SecondaryUnitSize = clean($("#UnitID option:selected").val());
        var qty = SecondaryUnitSize * clean($("#txtRequiredQty").val());
        var params = {
            ItemID: $("#ItemID").val(),
            BatchTypeID: 1,
            WarehouseID: $("#IssuePremiseID").val(),
            Qty: qty,
            SecondaryQty: clean($("#txtRequiredQty").val()),
            SecondaryUnit: $("#UnitID option:selected").text().trim(),
            SecondaryUnitSize: SecondaryUnitSize,
            UnitID: $("#PrimaryUnitID").val()
        };
        //BatchTypeID = $("#BatchType").val() == "" ? "0" : $("#BatchType").val();
        //if ($("#BatchType:visible").length == 0 || ($("#Category").val() != "Finished Goods")) {
        //    BatchTypeID = 0;
        //}
        $.ajax({
            url: '/Stock/StockIssue/GetBatchwiseItemWithSUnit',//GetBatchwiseItem
            dataType: "json",
            data: params,
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.add_items_to_grid(response.Data, 1000);
                    self.clear_item_details();
                } else {
                    app.show_error(response.Message)
                }
            },
        });
    },

    clear_item_details: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#UnitID").val('');
        $("#Unit").val('');
        $("#Batch").val('');
        $("#txtRequiredQty").val('');
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAvailableStockItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
                WarehouseID: $("#WarehouseID").val(),
                BatchTypeID: $("#BatchTypeID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item_details: function (event, items) {   // on select auto complete item
        var self = stockissue;
        $("#ItemID").val(items.id);
        $("#ItemName").val(items.value);
        $("#Category").val(items.category);
        $("#PrimaryUnit").val(items.primaryUnit);
        $("#PrimaryUnitID").val(items.primaryUnitId);
        $("#InventoryUnit").val(items.inventoryUnit);
        $("#InventoryUnitID").val(items.inventoryUnitId);
        if (($("#Category").val() == "Finished Goods")) {
            $("#BatchTypeID").val(self.default_batch_type_id);
        }
        self.get_units();
    },

    select_stock_requisition: function () {
        var self = stockissue;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $("#stock-issue-items-list tbody").empty();
                self.get_stock_request_items();
            })
        } else {
            self.get_stock_request_items();
        }
    },

    get_stock_request_items: function () {

        var self = stockissue;

        if (self.RequisitionIDs.length == 0) {
            app.show_error('Please select atleast one requisition');
            return;
        }
        $.ajax({
            url: '/Stock/StockRequest/GetStockRequestItems/',
            dataType: "json",
            data: {
                StockRequisitionIDS: self.RequisitionIDs,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.add_items_to_grid(response.Data, 0);
                    $("#RequestNo").val(self.RequisitionNos);
                } else {
                    app.show_error(response.Message)
                }
            },
        });
    },

    add_items_to_grid: function (items, position) {
        var self = stockissue;
        var date = $("#Date").val();
        var UnitID, Unit;
        var tr = '';

        var $items_list = $('#stock-issue-items-list tbody');
        var index = position + 1
        items = self.item_calculations(items);
        $.each(items, function (i, item) {
            if (item.UnitID == 0) {
                item.UnitID = $("#PrimaryUnitID").val();
                item.Unit = $("#PrimaryUnit").val();
            }
            tr += "<tr>"
                + "<td class='uk-text-center serial-no'>" + (i + index) + "</td>"
                + "<td class='ItemCode'>" + item.Code + "</td>"
                + "<td class='ItemName' >" + item.Name
                + "<input type='hidden' class='ItemID' value='" + item.ItemID + "'/>"
                + "<input type='hidden' class='BatchID' value='" + item.BatchID + "' />"
                + "<input type='hidden' class='BatchTypeID' value='" + item.BatchTypeID + "' />"
                + "<input type='hidden' class='Stock' value='" + item.Stock + "' />"
                + "<input type='hidden' class='RequestTransID' value='" + item.RequestTransID + "' />"
                + "<input type='hidden' class='RequestID' value='" + item.RequestID + "' />"
                + "<input type='hidden' class='Rate' value='" + item.Rate + "' />"
                + "<input type='hidden' class='CGSTPercentage' value='" + item.CGSTPercentage + "' />"
                + "<input type='hidden' class='SGSTPercentage' value='" + item.SGSTPercentage + "' />"
                + "<input type='hidden' class='IGSTPercentage' value='" + item.IGSTPercentage + "' />"
                + "<input type='hidden' class='CGSTAmount' value='" + item.CGSTAmount + "' />"
                + "<input type='hidden' class='SGSTAmount' value='" + item.SGSTAmount + "' />"
                + "<input type='hidden' class='IGSTAmount' value='" + item.IGSTAmount + "' />"
                + "<input type='hidden' class='StockRequisitionNo' value='" + item.StockRequisitionNo + "' />"
                + "<input type='hidden' class='PackSize' value='" + item.PackSize + "' />"
                + "<input type='hidden' class='PrimaryUnit' value='" + item.PrimaryUnit + "' />"
                + "<input type='hidden' class='PrimaryUnitID' value='" + item.PrimaryUnitID + "' />"
                + "<input type='hidden' class='SecondaryUnitSize' value='" + item.SecondaryUnitSize + "' />"
                + "<input type='hidden' class='UnitID' value='" + item.UnitID + "' />"
                + "</td>"
                + "<td class='PartsNo'>" + item.PartsNo + "</td>"
                + "<td class='Model'>" + item.Make + '/' + item.Model + "</td>"
                + "<td class='Unit uk-hidden'>" + item.Unit + "</td>"
                + "<td class='SecondaryUnit'>" + item.SecondaryUnit + "</td>"
                + "<td class='BatchName'>" + item.BatchName + "</td>"
                + "<td class='mask-production-qty RequestedQty uk-hidden'>" + item.RequestedQty + "</td>"
                + "<td class='mask-production-qty SecondaryQty'>" + item.SecondaryQty + "</td>"
                + "<td class='action uk-hidden'>"
                + "<input type='text' min='0' class='md-input mask-production-qty IssueQty' readonly='readonly' value='" + item.IssueQty + "' />"
                + "</td>"
                + "<td class='mask-production-qty SecondaryIssueQty'>" + item.SecondaryIssueQty + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency BasicPrice' disabled value='" + item.BasicPrice + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency GrossAmount' disabled value='" + item.GrossAmount + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency TradeDiscountPercentage' disabled value='" + item.TradeDiscountPercentage + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency TradeDiscount' disabled value='" + item.TradeDiscount + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency TaxableAmount' disabled value='" + item.TaxableAmount + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency GSTPercentage' disabled value='" + item.IGSTPercentage + "' />"
                + "</td>"
                + "<td class='inter-branch'>"
                + "      <input type='text' class='md-input mask-currency GSTAmount' disabled value='" + item.GSTAmount + "' />"
                + "</td>"
                + "<td class='inter-branch '>"
                + "      <input type='text' class='md-input mask-currency NetAmount' disabled value='" + item.NetAmount + "' />"
                + "</td>"
                + "<td class='action'>"
                + "      <input type='text' class='md-input Remarks' />"
                + "</td>"
                + " <td class='uk-text-center action'>"
                + "     <a class='remove-item'>"
                + "         <i class='uk-icon-remove'></i>"
                + "     </a>"
                + " </td>"
                + "</tr>";

        });
        var $tr = $(tr);
        app.format($tr);
        $items_list.insertAt(position, $tr);
        self.update_form();
        self.update_form_summary();
        self.count_items();

        $items_list.find("tr td.uk-text-center.serial-no").each(function (i, record) {
            $(this).text(i + 1);
        });
        setTimeout(function () { fh_items.resizeHeader(); }, 200);
        self.set_packing_details();
    },
    set_packing_details: function () {
        var self = stockissue;
        $('#packing-detail-list tbody').html('');
        self.PackingDetails = [];
        var PackSize, PrimaryUnit, IssueQty, row, PackUnitID;
        $('#stock-issue-items-list tbody tr').each(function (i, record) {
            row = $(this).closest('tr');
            PackSize = $(row).find('.PackSize').val();
            PrimaryUnit = $(row).find('.PrimaryUnit').val();
            IssueQty = clean($(row).find('.IssueQty').val());
            PackUnitID = clean($(row).find('.UnitID').val());
            PackUnit = $(row).find('.Unit').text();
            if (IssueQty != 0) {
                self.calculate_group_packing_details(parseInt(PackSize) + ' ' + PrimaryUnit, IssueQty, PackUnit, PackUnitID, PackSize);
            }
        });

        self.PackingDetails = self.PackingDetails.sort(function (a, b) {
            var first = a.Pack;
            var second = b.Pack;
            return second - first
        });

        self.PackingDetails = self.PackingDetails.sort((a, b) => b - a);

        var tr = "";
        var $tr;
        $(self.PackingDetails).each(function (i, record) {
            tr += '<tr  class="uk-text-center">';
            tr += '<td>' + (i + 1);
            tr += '</td>';
            tr += '<td class="PackSize">' + record.PackSize;
            tr += '     <input type="hidden" class="PackUnitID" value="' + record.PackUnitID + '" />'
            '</td>';
            tr += '<td class="PackUnit">' + record.PackUnit;
            tr += '</td>';
            tr += '<td class="uk-text-right Quantity">' + record.Quantity;
            tr += '</td>';
            tr += '</tr>';
        });
        $tr = $(tr);
        app.format($tr);
        $("#packing-detail-list tbody").html($tr);


    },
    calculate_group_packing_details: function (PackSize, Quantity, PackUnit, PackUnitID, Pack) {
        var self = stockissue;
        if (Quantity == 0) {
            return;
        }
        var index = self.search_pack_size(self.PackingDetails, PackSize, PackUnit, PackUnitID);
        if (index == -1) {
            self.PackingDetails.push({
                PackSize: PackSize,
                Quantity: Quantity,
                PackUnit: PackUnit,
                PackUnitID: PackUnitID,
                Pack: Pack

            });
        } else {
            var qty = self.PackingDetails[index].Quantity;

            self.PackingDetails[index].Quantity += Quantity;

        }
    },
    search_pack_size: function (array, Packsize, PackUnit, PackUnitID) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].PackSize == Packsize && array[i].PackUnitID == PackUnitID) {
                return i;
            }
        }
        return -1;
    },
    item_calculations: function (items) {
        var self = stockissue;
        if (!self.is_gst_effect()) {
            return items;
        }
        $(items).each(function (i, item) {
            if (self.IssueType != "InterState") {
                // item.Rate = 0;
                item.TradeDiscountPercentage = 0;
            }
            if ($("#ReceiptLocationID").val() == $("#IssueLocationID").val()) {
                item.Rate = 0;
                item.TradeDiscountPercentage = 0;
            }
            item.BasicPrice = (item.Rate * 100 / (100 + item.IGSTPercentage)).roundTo(2);
            item.GrossAmount = (item.BasicPrice * item.IssueQty).roundTo(2);
            item.TradeDiscount = (item.GrossAmount * item.TradeDiscountPercentage / 100).roundTo(2);
            item.TaxableAmount = (item.GrossAmount - item.TradeDiscount).roundTo(2);

            if (self.is_igst()) {
                item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);
            } else {
                item.GSTAmount = 0;
            }

            if (self.is_igst()) {
                item.IGSTAmount = (item.GSTAmount).roundTo(2);
                item.CGSTAmount = 0;
                item.SGSTAmount = 0;
            } else {
                item.IGSTAmount = 0;
                item.CGSTAmount = (item.GSTAmount / 2).roundTo(2);
                item.SGSTAmount = (item.GSTAmount / 2).roundTo(2);
            }
            item.NetAmount = (item.TaxableAmount + item.GSTAmount).roundTo(2);
        });
        return items;
    },

    update_form_summary: function () {
        var self = stockissue;

        var GrossAmount = 0;
        var TradeDiscount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var RoundOff = 0;
        var NetAmount = 0;

        $("#stock-issue-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find(".GrossAmount").val());
            TradeDiscount += clean($(this).find(".TradeDiscount").val());
            TaxableAmount += clean($(this).find(".TaxableAmount").val());
            SGSTAmount += clean($(this).find(".SGSTAmount").val());
            CGSTAmount += clean($(this).find(".CGSTAmount").val());
            IGSTAmount += clean($(this).find(".IGSTAmount").val());
            NetAmount += clean($(this).find(".NetAmount").val());
        });

        RoundOff = Math.round(NetAmount) - NetAmount;

        $("#GrossAmount").val(GrossAmount);
        $("#TradeDiscount").val(TradeDiscount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(Math.round(NetAmount));
    },

    save: function () {
        var self = stockissue;
        var location = "/Stock/StockIssue/Index";
        if ($(this).hasClass("btnSaveAndNew")) {
            location = "/Stock/StockIssue/Create";
        }
        var model = self.get_data(IsDraft);
        if (IsDraft == true) {
            var url = '/StockIssue/SaveAsDraft';
        }
        else {
            var url = '/StockIssue/Save';
        }
        $(".btnSaveAndNew, .btnSave,.btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Stock issue created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    app.show_error("Failed to save Stock Issue");
                    $(".btnSaveAndNew, .btnSave, .btnSaveAsDraft").css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function (IsDraft) {
        var self = stockissue;
        var model = {
            IssueNo: $("#IssueNo").val(),
            Date: $("#Date").val(),
            RequestNo: $("#RequestNo").val(),
            IssueLocationID: $("#IssueLocationID").val(),
            IssuePremiseID: $("#IssuePremiseID").val(),
            ReceiptLocationID: $("#ReceiptLocationID").val(),
            ReceiptPremiseID: $("#ReceiptPremiseID").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            TradeDiscount: clean($("#TradeDiscount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            ID: $("#ID").val(),
            IssueType: self.IssueType,
            IsDraft: IsDraft,
            IsService: 0,
            Remark: $("#Remark").val()
        };
        model.Items = self.get_item_list();
        model.PackingDetails = self.get_packing_list();
        return model;
    },
    get_item_list: function () {
        var Items = [];
        $("#stock-issue-items-list tbody tr").each(function (i) {
            Items.push(
                {
                    ItemID: $(this).find('.ItemID').val(),
                    Name: $(this).find('.ItemName').text().trim(),
                    BatchName: $(this).find('.BatchName').text(),
                    BatchID: clean($(this).find('.BatchID').val()),
                    BatchTypeID: clean($(this).find('.BatchTypeID').val()),
                    IssueQty: clean($(this).find('.IssueQty').val()),
                    IssueDate: $("#Date").val(),
                    SecondaryUnit: $(this).find('.SecondaryUnit').text().trim(),
                    SecondaryIssueQty: clean($(this).find('.SecondaryIssueQty').val()),
                    SecondaryQty: clean($(this).find('.SecondaryQty').val()),
                    SecondaryUnitSize: clean($(this).find('.SecondaryUnitSize').val()),
                    StockRequestTransID: clean($(this).find('.RequestTransID').val()),
                    StockRequestID: clean($(this).find('.RequestID').val()),
                    RequestedQty: clean($(this).find('.RequestedQty').text()),
                    Rate: clean($(this).find('.Rate').val()),
                    BasicPrice: clean($(this).find('.BasicPrice').val()),
                    GrossAmount: clean($(this).find('.GrossAmount').val()),
                    TradeDiscountPercentage: clean($(this).find('.TradeDiscountPercentage').val()),
                    TradeDiscount: clean($(this).find('.TradeDiscount').val()),
                    TaxableAmount: clean($(this).find('.TaxableAmount').val()),
                    SGSTPercentage: clean($(this).find('.SGSTPercentage').val()),
                    CGSTPercentage: clean($(this).find('.CGSTPercentage').val()),
                    IGSTPercentage: clean($(this).find('.IGSTPercentage').val()),
                    SGSTAmount: clean($(this).find('.SGSTAmount').val()),
                    CGSTAmount: clean($(this).find('.CGSTAmount').val()),
                    IGSTAmount: clean($(this).find('.IGSTAmount').val()),
                    NetAmount: clean($(this).find('.NetAmount').val()),
                    UnitID: clean($(this).find('.UnitID').val()),
                    Remark: $(this).find('.Remarks').val(),
                });
        });

        return Items;
    },

    get_packing_list: function () {
        var Pack_details = [];
        $("#packing-detail-list tbody tr").each(function (i) {
            Pack_details.push(
                {
                    PackSize: $(this).find('.PackSize').text().trim(),
                    PackUnit: $(this).find('.PackUnit').text().trim(),
                    Quantity: $(this).find('.Quantity').text(),
                    PackUnitID: clean($(this).find('.PackUnitID').val()),
                });
        });

        return Pack_details;
    },

    validate_item: function () {
        var self = stockissue;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form_for_draft: function () {
        var self = stockissue;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_form: function () {
        var self = stockissue;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_requisition: function () {
        var self = stockissue;
        if (self.rules.on_requisition.length) {
            return form.validate(self.rules.on_requisition);
        }
        return 0;
    },
    validate_on_download_template: function () {
        var self = stockissue;
        if (self.rules.on_download_template.length) {
            return form.validate(self.rules.on_download_template);
        }
        return 0;
    },
    validate_batch: function () {
        var self = stockissue;
        if (self.rules.batch.length > 0) {
            return form.validate(self.rules.batch);
        }
        return 0;
    },

    rules: {
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#IssuePremiseID",
                rules: [
                    { type: form.required, message: "Please select Issue Premise" },
                    {
                        type: function (element) {
                            var FromPremises = $("#IssuePremiseID").val();
                            var ToPremises = $("#ReceiptPremiseID").val();
                            return FromPremises != ToPremises;
                        },
                        message: "Stock can not be transfered between same premise",
                        alt_element: "#ReceiptPremiseID"
                    },
                ]
            },
            {
                elements: "#ReceiptPremiseID",
                rules: [
                    { type: form.required, message: "Please select Receipt Premise" },
                ]
            },
            {
                elements: "#ReceiptLocationID",
                rules: [
                    { type: form.required, message: "Please select Receipt Location" },
                ]
            },
            {
                elements: "#IssueLocationID",
                rules: [
                    { type: form.required, message: "Please select Issue Location" },
                ]
            },
            {
                elements: "#stock-issue-items-list .SecondaryIssueQty",
                rules: [
                    {
                        type: function (element) {
                            return clean($(element).val()) >= 1;
                        }, message: "Item out of stock"
                    }
                ]
            },
            {
                elements: '#stock-issue-items-list .IssueQty',
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.Stock').val());
                        }, message: "Item out of stock"
                    },
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var IssueQty = clean($(element).val());
                            var RequestedQty = clean($(row).find('.RequestedQty').text());
                            return IssueQty <= RequestedQty;
                        }, message: 'IssueQty cannot greater than RequestedQty'
                    },
                ]
            }
        ],

        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            //{
            //    elements: "#IssuePremiseID",
            //    rules: [
            //        { type: form.required, message: "Please select Issue Premise" },
            //        {
            //            type: function (element) {
            //                var FromPremises = $("#IssuePremiseID").val();
            //                var ToPremises = $("#ReceiptPremiseID").val();
            //                return FromPremises != ToPremises;
            //            },
            //            message: "Stock can not be transfered between same premise",
            //            alt_element: "#ReceiptPremiseID"
            //        },
            //    ]
            //},
            //{
            //    elements: "#ReceiptPremiseID",
            //    rules: [
            //        { type: form.required, message: "Please select Receipt Premise" },
            //    ]
            //},
            {
                elements: "#ReceiptLocationID",
                rules: [
                    { type: form.required, message: "Please select Receipt Location" },
                ]
            },
            {
                elements: "#IssueLocationID",
                rules: [
                    { type: form.required, message: "Please select Issue Location" },
                ]
            },
            {
                elements: '#stock-issue-items-list .IssueQty',
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.Stock').val());
                        }, message: "Item out of stock"
                    },
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var IssueQty = clean($(element).val());
                            var RequestedQty = clean($(row).find('.RequestedQty').text());
                            return IssueQty <= RequestedQty;
                        }, message: 'IssueQty cannot greater than RequestedQty'
                    },
                ]
            }
        ],
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    {
                        type: function (element) {
                            var item_id = $(element).val();
                            var batch_type = $("#BatchType").val();
                            var error = false;
                            $('#stock-issue-items-list tbody tr').each(function () {
                                if ($(this).find('.ItemID').val() == item_id && $(this).find('.BatchTypeID').val() == batch_type) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already added in the grid, try editing issue quantity"
                    }
                ]
            },
            //{
            //    elements: "#Category",
            //    rules: [
            //        {
            //            type: function (element) {
            //                var count = 0;
            //                if ($(element).val() == "Finished Goods") {
            //                    if ($("#BatchType").val() == "")
            //                        count = 1;
            //                }
            //                return count == 0;
            //            }, message: "Please choose BatchType"
            //        },
            //    ]
            //},
            {
                elements: "#txtRequiredQty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                ]
            },
        ],
        on_download_template: [
            {
                elements: "#ReceiptPremiseID",
                rules: [
                    { type: form.required, message: "Please select Receipt Premises" },
                ]
            },
            {
                elements: "#ItemCategoryID",
                rules: [
                    { type: form.required, message: "Please select Item Category" },
                ]
            },
        ],
        on_requisition: [
            {
                elements: "#IssuePremiseID",
                rules: [
                    {
                        type: function (element) {
                            var IssuePremiseID = clean($("#IssuePremiseID").val());
                            var ReceiptPremiseID = clean($("#ReceiptPremiseID").val());
                            if (IssuePremiseID != 0 && ReceiptPremiseID != 0) {
                                return ReceiptPremiseID != IssuePremiseID;
                            } else {
                                return true;
                            }

                        }, message: "Stock can not be transfered between same premise", alt_element: "#ReceiptPremiseID"
                    },
                ]
            },
            {
                elements: "#ReceiptLocationID",
                rules: [
                    { type: form.required, message: "Please select Receipt Location" },
                    { type: form.numeric, message: "Please select Receipt Location" },
                ]
            },
        ],
        batch: [
            {
                elements: "#batch-edit .IssueQty",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            return clean($(element).val()) <= clean($(row).find('.Stock').text());
                        }, message: "Insufficient Stock"
                    },
                ]
            },
            {
                elements: "#batch-edit .TotalIssueQty",
                rules: [
                    {
                        type: function (element) {
                            var BatchQty = clean($("#BatchQty").val());
                            var TotalIssueQty = clean($(".TotalIssueQty").val());
                            return BatchQty >= TotalIssueQty;
                        }, message: "Total quantity exceeds requested quantity"
                    },
                ]
            },
        ]

    }
}