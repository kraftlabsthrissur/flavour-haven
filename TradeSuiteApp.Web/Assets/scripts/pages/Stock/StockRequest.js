var fh_items;
stockrequest = {
    init: function () {
        var self = stockrequest;

        item_list = self.stockable_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        self.freeze_headers();

        // for cross checking issue location and warehouse 
        $.each($("#IssuePremiseID option"), function () {
            $(this).data("location-id", $("#IssueLocationID").val());
        })
    },

    details: function () {
        var self = stockrequest;
        $("body").on("click", ".printpdf", self.printpdf);
        self.freeze_headers();
        self.bind_events();
    },
    SelectSecondaryUnits: function (Unit, SecondaryUnits) {
        var optionsSecondaryUnits = SecondaryUnits.split(',');
        var select = '<select class="md-input label-fixed secondaryUnit">';
        select += '<option value="1" selected>' + Unit + '</option>';
        optionsSecondaryUnits.forEach(function (option) {
            var parts = option.split('|');
            if (parts.length > 1) {
                var text = parts[0];
                var value = parts[1];
                select += '<option value="' + value + '">' + text + '</option>';
            }
        });
        select += '</select>';
        return select;
    },
    stockable_item_list: function () {
        var $list = $('#item-list');//item-list
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetStockableItemsList?type=all";

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
                                + "<input type='hidden' class='Stock' value='" + row.stock + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='ItemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='InventoryUnitID' value='" + row.InventoryUnitID + "'>"
                                + "<input type='hidden' class='SalesCategory' value='" + row.SalesCategory + "'>"
                                + "<input type='hidden' class='SalesUnitID' value='" + row.SalesUnitID + "'>"
                                + "<input type='hidden' class='SalesUnit' value='" + row.SalesUnit + "'>"
                                + "<input type='hidden' class='PartsNumber' value='" + row.PartsNumber + "'>"
                                + "<input type='hidden' class='Make' value='" + row.Make + "'>"
                                + "<input type='hidden' class='Model' value='" + row.Model + "'>"
                                + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>"
                                + "<input type='hidden' class='InventoryUnit' value='" + row.InventoryUnit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PartsNumber", "className": "PartsNumber" },
                    { "data": "Make", "className": "Make" },
                    { "data": "Model", "className": "Model" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    printpdf: function () {
        var self = stockrequest;
        $.ajax({
            url: '/Reports/Stock/StockRequestPrintPdf',
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

    list: function () {
        var self = stockrequest;
        $('#tabs-stock-request').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

        $("body").on('click', '.btnsuspend', self.confirm_suspend);
        $("body").on('click', '.btnclone', self.open_clone);

    },

    tabbed_list: function (type) {
        // types
        //  1. Draft
        //  2. To be Issued.
        //  3. Partially Issued.
        //  4. Fully Issued.
        //  5. Cancelled.
        //  6. Suspended.

        var $list;

        switch (type) {
            case "draft":
                $list = $('#stock-request-draft-list');
                break;
            case "to-be-issued":
                $list = $('#stock-request-to-be-issued-list');
                break;
            case "partially-issued":
                $list = $('#stock-request-partially-issued-list');
                break;
            case "fully-issued":
                $list = $('#stock-request-fully-issued-list');
                break;
            case "cancelled":
                $list = $('#stock-request-cancelled-list');
                break;
            case "suspended":
                $list = $('#stock-request-suspended-list');
                break;
            default:
                $list = $('#stock-request-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Stock/StockRequest/GetRequisitionList?type=" + type;

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
                        "data": "Suspend", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsSuspendable ? "<button class='md-btn md-btn-primary btnsuspend' >Suspend</button>" : "";
                        }
                    },
                    {
                        "data": "Clone", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsClonable ? "<button class='md-btn md-btn-primary btnclone' >Clone</button>" : "";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/StockRequest/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    freeze_headers: function () {
        fh_items = $("#stock-request-items-list").FreezeHeader();
    },

    bind_events: function () {
        var self = stockrequest;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnAddProduct").on('click', self.add_item);

        $(".btnSave, .btnSaveAndNew, .btnSaveAsDraft").on("click", self.on_save);

        $("#IssueLocationID").on("change", self.get_issue_premises);

        $("body").on("click", ".remove-item", self.remove_item);
        $("#ToPremise").on("change", self.set_warehouse_id);

        $("body").on('click', '.btnsuspend', self.confirm_suspend);
        $("body").on('click', '.btnclone', self.open_clone);
        $("body").on('click', ".cancel", self.cancel_confirm);

        $('body').on('change', "#txtExpDate, #txtRequiredTime", self.set_date_time);
        $("body").on("click", "#btn-download-template", self.download_template);
        $("body").on("keyup change", "#stock-request-items-list tbody tr .secondary .secondaryQty, .secondaryUnit", self.on_secondary_unit_change);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
    },
    on_secondary_unit_change: function () {
        var self = stockrequest;
        var $row = $(this).closest('tr');
        var SecondaryQty = clean($row.find('.secondaryQty').val());
        var SecondaryUnitSize = clean($row.find('.secondaryUnit').val());
        var Qty = (SecondaryQty * SecondaryUnitSize).toFixed(10);
        $row.find('.clQty').val(Qty);
        $row.find('.clQty').trigger('change');
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
            var self = stockrequest;

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
        var self = stockrequest;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Stock/StockRequest/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var items = [];

                if (response.Status == "success") {
                    $("#stock-request-items-list tbody").html("");
                    $.each(response.Data, function (i, item) {
                        if (item.Remarks == null) {
                            item.Remarks = "";
                        }
                        item.RequiredDate = $("#txtExpDate").val();
                        items.push(item);
                    });
                    self.add_items_to_grid(items);
                }

                $("#preloader").hide();
                altair_helpers.content_preloader_hide();

            }
        });
    },

    download_template: function () {
        var self = stockrequest;
        self.error_count = 0;
        self.error_count = self.validate_on_download_template();
        if (self.error_count > 0) {
            return;
        }
        var Category = $("#ItemCategoryID option:selected").text();
        var WarehouseID = $("#ReceiptPremiseID").val();
        window.location = "/Reports/Stock/GetRequisitionTemplate/?Category=" + Category + "&WarehouseID=" + WarehouseID;
    },

    open_clone: function (e) {
        e.stopPropagation();
        var self = stockrequest;
        var id = $(this).parents('tr').find('.ID').val();
        window.location = '/Stock/StockRequest/Clone/' + id;
    },

    set_date_time: function () {
        $("#DateTime").val($("#txtExpDate").val() + " " + $("#txtRequiredTime").val());
    },

    cancel_confirm: function () {
        var self = stockrequest
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        $(".btnSavePrNew, .btnSavePr,.btnSavedraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Stock/StockRequest/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "StockRequisition"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Stock request cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Stock/StockRequest/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSavePrNew, .btnSavePr,.btnSavedraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },

    confirm_suspend: function (e) {
        e.stopPropagation();
        var self = stockrequest;
        var id = $(this).parents('tr').find('.ID').val();
        app.confirm("Do you really want to Suspend? This can not be undone.", function () {
            self.suspend(id);
        });
    },

    suspend: function (id) {

        $.ajax({
            url: '/Stock/StockRequest/Suspend',
            data: {
                ID: id,
                Table: "StockRequisition"
            },
            type: "GET",
            cache: false,
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Stock Request Suspended Successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                }
                if (response.Data == 0) {
                    app.show_error("Stock Request Already Processed");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('processed');
                }
            },
        });
    },

    set_warehouse_id: function () {
        var warehouseid = $("#ToPremise").val();
        $("#WarehouseID").val(warehouseid).trigger('change');
    },

    remove_item: function () {
        var self = stockrequest;
        $(this).closest("tr").remove();
        $("#stock-request-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#stock-request-items-list tbody tr").length);
        fh_items.resizeHeader();
    },

    select_item: function () {
        var self = stockrequest;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {
            id: radio.val(),
            name: $(row).find(".Name").text().trim(),
            code: $(row).find(".Code").text().trim(),
            stock: $(row).find(".Stock").val(),
            primaryUnit: $(row).find(".PrimaryUnit").val(),
            primaryUnitId: $(row).find(".PrimaryUnitID").val(),
            partsNumber: $(row).find(".PartsNumber").val(),
            make: $(row).find(".Make").val(),
            model: $(row).find(".Model").val(),
            secondaryUnits: $(row).find(".SecondaryUnits").val(),
            inventoryUnit: $(row).find(".InventoryUnit").val(),
            inventoryUnitId: $(row).find(".InventoryUnitID").val(),
            category: $(row).find(".ItemCategory").val(),
            salescategory: $(row).find(".SalesCategory").val(),
        };
        self.on_select_item(item);
        UIkit.modal($('#select-item')).hide();
    },

    set_item_details: function (event, item) {
        var self = stockrequest;
        self.on_select_item(item);
    },

    on_select_item: function (item) {
        var self = stockrequest;
        $("#ItemID").val(item.id);
        $("#ItemName").val(item.name);
        $("#Code").val(item.code);
        $("#PartsNumber").val(item.partsNumber);
        $("#Model").val(item.model);
        $("#Make").val(item.make);
        $("#Stock").val(item.stock);
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#SecondaryUnits").val(item.secondaryUnits);
        $("#InventoryUnit").val(item.inventoryUnit);
        $("#InventoryUnitID").val(item.inventoryUnitId);

        $("#Category").val(item.category);
        $("#SalesCategory").val(item.salescategory);
        $('#txtRqQty').focus();

        if (($("#Category").val() == "Finished Goods")) {
            $("#BatchType").removeAttr("disabled");
            $("#BatchType").val($("#DefaultBatchTypeID").val());
        }
        else {
            $("#BatchType").attr("disabled", "disabled");
            $("#BatchType").val('');
        }

        $.ajax({
            url: '/Masters/Item/GetAverageSalesAndStock',
            data: {
                ItemID: $("#ItemID").val(),
                BatchTypeID: $("#BatchType").val(),
                LocationID: $("#ReceiptLocationID").val(),
                WarehouseID: $("#ReceiptPremiseID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#AverageSales").val(response.AverageSales);
                    $("#Stock").val(response.Stock);
                }
            }
        });
        self.get_units();
    },

    get_units: function () {
        var self = stockrequest;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#InventoryUnitID").val() + "'>" + $("#InventoryUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);

    },

    get_issue_premises: function () {
        var self = stockrequest;
        var location_id = $(this).val();
        if (location_id == null || location_id == "") {
            location_id = 0;
        }
        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                var selected = "";
                $("#IssuePremiseID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    selected = "";
                    if (response.data.length == 1) {
                        selected = "selected";
                    } else if (location_id != $("#ReceiptLocationID").val() && i == 0) {
                        selected = "selected";
                    }
                    html += "<option data-location-id = '" + location_id + "' " + selected + " value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#IssuePremiseID").append(html);

            }
        });
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    add_item: function () {
        var self = stockrequest;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var items = [];
        var item = {
            ItemID: $("#ItemID").val(),
            Code: $("#Code").val(),
            Name: $("#ItemName").val(),
            PartsNumber: $("#PartsNumber").val(),
            Model: $("#Model").val(),
            Make: $("#Make").val(),
            BatchType: "ISK",
            BatchTypeID: 1,
            UnitID: $("#UnitID option:selected").val(),
            Unit: $("#UnitID option:selected").text(),
            SecondaryUnits: $("#SecondaryUnits").val(),
            Remarks: $("#txtRemarks").val(),
            Stock: $("#Stock").val(),
            AverageSales: $("#AverageSales").val(),
            RequiredDate: $("#txtExpDate").val(),
            RequiredQty: $("#txtRqQty").val(),
            SuggestedQty: $("#BatchType").val() == 2 ? clean($("#AverageSales").val()) * 1.5 : clean($("#AverageSales").val()) * 1,
            SalesCategory: $("#SalesCategory").val(),
        };

        items.push(item);

        self.add_items_to_grid(items);

    },

    add_items_to_grid: function (items) {
        var self = stockrequest;
        var html = "";
        var length = $("#stock-request-items-list tbody tr").length;

        $(items).each(function (i, item) {
            html += '<tr>'
                + '<td class="uk-text-center">' + (length + i + 1) + '</td>'
                + '<td>' + item.Code + '</td>'
                + '<td class="clProduct">' + item.Name
                + '   <input type="hidden" class="ItemID" value="' + item.ItemID + '" />'
                + '   <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '   <input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                + '</td>'
                + '<td>' + item.PartsNumber + '</td>'
                + '<td>' + item.Make + ' / ' + item.Model + '</td>'
                + '<td class="uk-hidden">' + item.Unit + '</td>'
                + '<td class="secondary">' + self.SelectSecondaryUnits(item.Unit, item.SecondaryUnits) + '</td>'
                + '<td class="uk-hidden"><input type="text" class="md-input uk-text-right mask-production-qty clQty" value="' + item.RequiredQty + '" /></td>'
                + '<td class="secondary" ><input type="text" class="md-input secondaryQty mask-sales2-currency" value="' + item.RequiredQty + '"  /></td>'
                + '<td class="cltxtDate">' + item.RequiredDate + ' </td>'
                + '<td><input type="text" class="md-input txtRemarks" value="' + item.Remarks + '"/></td>'
                + '<td><input type="text" class="md-input mask-qty StockInPremises" value="' + item.Stock + '" readonly = "readonly" /></td>'
                + '<td><input type="text" class="md-input mask-qty SuggestedQty" value="' + item.SuggestedQty + '" readonly = "readonly" /></td>'
                + '<td><input type="text" class="md-input mask-qty AverageSales" value="' + item.AverageSales + '"  readonly = "readonly"/></td>'
                + '<td class="uk-text-center">'
                + '  <a class="remove-item">'
                + '      <i class="uk-icon-remove"></i>'
                + '  </a>'
                + '</td>'
                + '</tr>';
        });

        $html = $(html);
        app.format($html);
        $("#stock-request-items-list tbody").append($html);

        self.clear_item();
        self.count_items();
        setTimeout(function () {
            fh_items.resizeHeader();
        }, 200);
    },

    count_items: function () {
        var count = $('#stock-request-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemCode").val('');
        $("#PartsNumber").val('');
        $("#Make").val('');
        $("#Model").val('');
        $("#ItemID").val('');
        $("#ItemTypeID").val('');
        $("#UnitID").val('');
        $("#txtRqQty").val('');
        $("#txtRemarks").val('');
        $("#BatchType").val('');
        $("#Stock").val('');
        $("#AverageSales").val('');
        $("#ItemName").focus();
    },

    validate_item: function () {
        var self = stockrequest;
        self.set_date_time();
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form: function () {
        var self = stockrequest;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_on_download_template: function () {
        var self = stockrequest;
        if (self.rules.on_download_template.length) {
            return form.validate(self.rules.on_download_template);
        }
        return 0;
    },

    on_save: function () {

        var self = stockrequest;
        var model = self.get_data();
        var location = "/Stock/StockRequest/Index";
        var url = '/Stock/StockRequest/Save';

        if ($(this).hasClass("btnSaveAsDraft")) {
            model.IsDraft = true;
            url = '/Stock/StockRequest/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveAndNew")) {
                location = "/Stock/StockRequest/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!model.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(model, url, location);
            }, function () {
            })
        } else {
            self.save(model, url, location);
        }
    },

    save: function (model, url, location) {
        var self = stockrequest;
        $(".btnSaveAndNew, .btnSave,.btnSaveAsDraft ").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Stock requisition created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);

                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveAndNew, .btnSave,.btnSaveAsDraft ").css({ 'display': 'block' });
                }
            },
            error: function () {
                $(".btnSaveAndNew, .btnSave,.btnSaveAsDraft ").css({ 'display': 'block' });
            }
        });

    },

    get_data: function () {
        var self = stockrequest;
        if ($("#IsClone").val() == "True") {
            ID = 0;
        }
        else {
            ID = $("#ID").val();
        }
        var model = {
            ID: ID,
            RequestNo: $("#StockRequestNumber").val(),
            Date: $("#Date").val(),
            IssuePremiseID: $("#IssuePremiseID").val(),
            ReceiptPremiseID: $("#ReceiptPremiseID").val(),
            IssueLocationID: $("#IssueLocationID").val(),
            ReceiptLocationID: $("#ReceiptLocationID").val(),
            Items: self.get_items_list(),
        };
        return model;
    },

    get_items_list: function () {
        var items = [];
        var item = {};
        $("#stock-request-items-list tbody tr").each(function () {
            item = {
                ItemID: $(this).find('.ItemID').val(),
                SecondaryQty: clean($(this).find('.secondaryQty').val()),
                SecondaryUnit: $(this).find('.secondaryUnit option:selected').text().trim(),
                SecondaryUnitSize: clean($(this).find('.secondaryUnit').val()),
                RequiredQty: (clean($(this).find('.secondaryUnit').val()) * clean($(this).find('.secondaryQty').val())),//clean($(this).find('.clQty').val()),
                Remarks: $(this).find('.txtRemarks').val(),
                RequiredDate: $(this).find('.cltxtDate').text().trim(),
                BatchTypeID: $(this).find('.BatchTypeID').val(),
                Stock: clean($(this).find('.StockInPremises').val()),
                AverageSales: clean($(this).find('.AverageSales').val()),
                UnitID: clean($(this).find('.UnitID').val()),
                SuggestedQty: clean($(this).find('.SuggestedQty').val()),
            };
            items.push(item);
        })
        return items;
    },

    validate_draft: function () {
        var self = stockrequest;
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
                    { type: form.required, message: "Please choose a valid item" },

                    {
                        type: function (element) {
                            var error = false;
                            var batchtypeID = clean($("#BatchType option:selected").val());
                            $("#stock-request-items-list tbody tr").each(function () {
                                if (($(this).find(".ItemID").val() == $(element).val()) && ($(this).find(".BatchTypeID").val() == batchtypeID)) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already exists, try editing quantity"
                    },
                ]
            },
            {
                elements: "#Category",
                rules: [
                    {
                        type: function (element) {
                            var count = 0;
                            if ($(element).val() == "Finished Goods") {
                                if ($("#BatchType").val() == "")
                                    count = 1;
                            }
                            return count != 1;
                        }, message: "Please choose BatchType"
                    },
                ]
            },
            {
                elements: "#txtExpDate",
                rules: [
                    { type: form.futute_date, message: "Please choose a valid Date" },
                    {
                        type: function (element) {
                            var dateTypeVar = $('#txtExpDate').val();
                            var dateTypeVar = $('#txtExpDate').val();
                            if (dateTypeVar == "")
                                return true;
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#CurrentDate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Invalid required date"
                    }
                ]
            },
            {
                elements: "#txtRqQty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                ]
            },
            {
                elements: "#ReceiptPremiseID",
                rules: [
                    { type: form.required, message: "Please choose Receipt Premise" },
                ]
            },
            {
                elements: "#DateTime",
                rules: [
                    //{ type: form.future_time, message: "Please choose a valid Date and time", alt_element: "#txtRequiredTime" },
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
                    {
                        type: function (element) {
                            return clean($("#IssueLocationID").val()) == $(element).find("option:selected").data("location-id");
                        },
                        message: "Please deselect and select issue location",
                        alt_element: "#IssueLocationID"
                    },
                ]
            },
            {
                elements: "#ReceiptPremiseID",
                rules: [
                    { type: form.required, message: "Please select Receipt Premises" },
                ]
            },
        ],
        on_blur: [],
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
                    {
                        type: function (element) {
                            return clean($("#IssueLocationID").val()) == $(element).find("option:selected").data("location-id");
                        },
                        message: "Please deselect and select issue location",
                        alt_element: "#IssueLocationID"
                    },
                ]
            },
            {
                elements: ".clQty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                ]
            },
            {
                elements: "#ReceiptPremiseID",
                rules: [
                    { type: form.required, message: "Please select Receipt Premises" },
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
            {
                elements: "#txtExpDate",
                rules: [
                    { type: form.futute_date, message: "Please choose a valid Date" },
                    {
                        type: function (element) {
                            var dateTypeVar = $('#txtExpDate').val();
                            if (dateTypeVar == "")
                                return true;
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#CurrentDate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Invalid required date"
                    }
                ]
            },
        ]
    }
}
