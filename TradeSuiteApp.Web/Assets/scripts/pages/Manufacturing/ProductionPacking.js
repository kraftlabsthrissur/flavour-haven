var item_select_table;
var item_list;
var fh_material;
var fh_process;
ProductionPacking = {
    init: function () {
        var self = ProductionPacking;


        Item.packing_material_list();
        $('#packing-material-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtAdditionalIssueQty",
            modal: "#select-item",
            initiatingElement: "#txtAddnitionalIssue",
            startFocusIndex: 3,
            selectionType: "radio"
        });

        Item.production_packing_item_list();
        $('#production-group-list').SelectTable({
            selectFunction: self.select_production_group,
            returnFocus: "#BatchID",
            modal: "#select-production-group",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });

        Item.packing_rerurn_item_list();
        $('#material-item-list').SelectTable({
            selectFunction: self.select_material,
            returnFocus: "#ReturnItemQty",
            modal: "#select-material-item",
            initiatingElement: "#ReturnItemQty",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        self.bind_events();
        self.freeze_headers();
        self.list();
    },

    freeze_headers: function () {
        fh_material = $("#production-packing-material-list").FreezeHeader({ height: 200 });
        fh_process = $("#production-packing-process-list").FreezeHeader({ height: 200 });
        $('#tabs-packing[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "material-tab") {
                    fh_material.resizeHeader();
                } else {
                    fh_process.resizeHeader();
                }
            }, 500);
        });
    },

    details: function () {
        var self = ProductionPacking;
        self.freeze_headers();
    },

    list: function () {
        var self = ProductionPacking;
        $('#tabs-module').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = ProductionPacking;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "cancelled":
                $list = $('#cancelled-list');
                break;
            case "ongoing":
                $list = $('#ongoing-list');
                break;
            case "packed":
                $list = $('#packed-list');
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

            var url = "/Manufacturing/ProductionPacking/GetPackingList?type=" + type;

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
                   { "data": "ProductionGroupName", "className": "ProductionGroup" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "PackingQty", "className": "PackingQty uk-text-right" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/ProductionPacking/Details/" + Id);
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
        var self = ProductionPacking;
        //Bind auto complete event for item 
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);

        $.UIkit.autocomplete($('#additionalIssueItem-autocomplete'), { 'source': self.get_additionalissueitem_AutoComplete, 'minLength': 1 });
        $('#additionalIssueItem-autocomplete').on('selectitem.uk.autocomplete', self.set_additionalIssueItem_details);

        $.UIkit.autocomplete($('#material-return-autocomplete'), { 'source': self.get_material_return_AutoComplete, 'minLength': 1 });
        $('#material-return-autocomplete').on('selectitem.uk.autocomplete', self.set_material_return_details);

        $("#btnOKPackingMaterial").on("click", self.select_item);
        $('#btnGetPackingMaterials').on("click", self.get_packing_materials);
        //$('#btnAddAdditionalItem').on('click', self.add_additional_material);

        $('#btnAddReturnItem').on('click', self.add_return_material);
        $('body').on('keyup', '.PackedQty', self.calculate_actual_qty);

        $('#BatchID').on('change', self.get_stock);
        $('body').on('change', '.IssueQty', self.check_stock);

        $('.save-as-draft').on('click', self.save);
        $('.save').on('click', self.save_confirm);
        $('.complete').on('click', self.complete);
        //  $('.completed').on('click', self.completed);
        $('.cancel').on('click', self.cancel_confirm);
        // $("#BatchID").on("change", self.get_production_packing);
        $('#btnAddAdditionalItem').on('click', self.add_additional_issue);
        $('#AdditionalBatchID').on('change', self.get_additional_stock);
        $('#btnOKProductionGroup').on('click', self.select_production_group);
        $("body").on("click", ".remove-item", self.remove_item);
        $('body').on('keyup', '.IssueQty', self.calculate_variance);
    },
    remove_item: function () {
        var self = ProductionPacking;
        $(this).closest("tr").remove();
    },
    cancel_confirm: function () {
        var self = ProductionPacking
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },
    calculate_variance: function () {
        var issueQty = clean($(this).closest('tr').find('.IssueQty').val());
        var stdQtyForActualBatch = clean($(this).closest('tr').find('.ActualQty').val());
        var variance = 0;
        if (issueQty != '' && issueQty > 0) {
            if (stdQtyForActualBatch != '' && stdQtyForActualBatch > 0) {
                variance = stdQtyForActualBatch - issueQty;
            }
        }
        $(this).closest('tr').find('.Variance').val(variance);
    },
    save_confirm: function () {
        var self = ProductionPacking;
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },


    select_material: function () {
        var self = ProductionPacking;
        var radio = $('#select-material-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var UnitID = $(row).find(".PrimaryUnitID").val();
        var Stock = $(row).find(".Stock").val();
        $('#ReturnItemUOM').val(Unit);
        $("#ReturnItemName").val(Name);
        $("#ReturnItemID").val(ID);
        $("#ReturnCode").val(Code);
        $("#ReturnStock").val(Stock);
        $("#ReturnUnitID").val(UnitID);
        UIkit.modal($('#select-item')).hide();
        //self.select_receipt_Item();

    },

    select_production_group: function () {
        var self = ProductionPacking;
        var radio = $('#select-production-group tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").text().trim();
        var UnitID = $(row).find(".UnitID").val();
        var ProductId = $(row).find(".ProductID").val();
        var BatchSize = $(row).find(".BatchSize").val();
        var ProductionGroupID = $(row).find(".ProductionGroupID").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#UOM").val(Unit);
        $("#UnitID").val(UnitID);
        $("#ProductID").val(ProductId);
        $("#BatchSize").val(BatchSize);
        $("#ProductGroupID").val(ProductionGroupID);
        if ($("#production-packing-material-list tbody tr").length > 0 || $("#production-packing-process-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#production-packing-material-list tbody').empty();
                $('#production-packing-process-list tbody').empty();
                $("#btnGetPackingMaterials").css({ 'display': 'block' });

            })
        }
        self.get_batches_with_stock();
        UIkit.modal($('#select-production-group')).hide();
    },

    cancel: function () {
        app.confirm("Are you sure cancel the packing?", function () {
            var packingID = $("#ID").val();
            $.ajax({
                url: '/Manufacturing/ProductionPacking/Cancel',
                data: {
                    PackingID: packingID,
                    Table: "packing"
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Packing Cancelled Successfully");
                        setTimeout(function () {
                            window.location = "/Manufacturing/ProductionPacking/Index";
                        }, 1000);
                    } else {
                        app.show_error("Could not Cancel the Packing");
                    }
                }
            });
        })
    },

    get_additionalissueitem_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPackingMaterialForAutoComplete',
            data: {

                Hint: $('#txtAddnitionalIssue').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_material_return_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                itemHint: $('#ReturnItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    select_item: function () {
        var self = ProductionPacking;
        var radio = $('#select-packing-material tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").text();
        var UnitID = $(row).find(".UnitID").val();
        var category = $(row).find(".ItemCategory").text();

        var Stock = $(row).find(".Stock").val();
        $('#AdditionalItemUOM').val(Unit);
        $("#txtAddnitionalIssue").val(Name);
        $("#AdditionalItemID").val(ID);
        $("#AdditionalCode").val(Code);
        $("#AdditionalUnitID").val(UnitID);
        $("#Stock").val(Stock);
        $("#Category").val(category);
        if (category == "Finished Goods") {
            $("#AdditionalBatchTypeID").prop("disabled", false);
        }
        else {
            $("#AdditionalBatchTypeID").prop("disabled", true);
        }
        UIkit.modal($('#select-packing-material')).hide();
    },

    select_receipt_Item: function () {

    },

    set_additionalIssueItem_details: function (event, item) {   // on select auto complete item
        var self = ProductionPacking;
        $('#AdditionalItemUOM').val(item.unit);
        $("#txtAddnitionalIssue").val(item.name);
        $("#AdditionalItemID").val(item.id);
        $("#AdditionalCode").val(item.code);
        $("#AdditionalUnitID").val(item.unitid);
        $("#Stock").val(item.stock);
        $("#Category").val(item.category);
        if (item.category == "Finished Goods") {
            $("#AdditionalBatchTypeID").prop("disabled", false);
        }
        else {
            $("#AdditionalBatchTypeID").prop("disabled", true);
        }
    },
    set_material_return_details: function (event, item) {   // on select auto complete item
        var self = ProductionPacking;

        $('#ReturnItemUOM').val(item.unit);
        $("#ReturnItemName").val(item.name);
        $("#ReturnItemID").val(item.id);
        $("#ReturnCode").val(item.code);
        $("#ReturnStock").val(item.stock);
        $("#ReturnUnitID").val(item.unitid);
    },

    get_production_packing: function () {
        var self = ProductionPacking;
        //self.update_item_list();
        var BatchID = $("#BatchID option:selected").val();
        ItemID = $("#ItemID").val();

        $.ajax({
            url: '/Manufacturing/ProductionPacking/GetProductionPackingOutput/',
            data: {

                ItemID: ItemID,
                BatchID: BatchID
            },
            dataType: "json",
            type: "GET",
            success: function (response) {

                $.each(response, function (i, out) {
                    serial_no = $('#production-packing-output-list tbody tr').length + 1;
                    tr = '<tr>'
                        + '<td class="serial">'
                        + serial_no
                        + '</td>'
                        + '<td class="Date">' + out.Date + '</td>'
                        + '<td>' + out.ItemName
                        + '     <input type="hidden" class="ItemID" value="' + $("#ItemID").val() + '" />'
                        + '     <input type="hidden" class="BatchTypeID" value="' + $("#BatchTypeID").val() + '" />'
                        + '     <input type="hidden" class="BatchID" value="' + $("#BatchID").val() + '" />'
                        + '     <input type="hidden" class="UnitID" value="' + $("#UnitID").val() + '" />'
                        + '     <input type="hidden" class="Status" value="' + 0 + '" /></td>'
                        + '<td class="batchNo">' + out.BatchNo + '</td>'
                        + '<td class="PackedQty mask-production-qty uk-text-right"> ' + out.PackedQty + '</td>'
                        + '<td class="batch_type">' + out.BatchType
                        + '</td>'
                        + '</tr>';

                    $('#production-packing-output-list tbody').append(tr);
                });
                $.each($('#production-packing-output-list tbody tr'), function (i, out) {
                    var row = $(this).closest('tr');
                    $(row).find(".serial").text(i + 1);
                    i++;
                });
            }
        });
    },

    calculate_actual_qty: function (index) {
        var self = ProductionPacking;
        var row = $(this).closest('tr');
        //if (typeof index != "undefined") {
        //    row = $("#production-packing-output-list tbody tr").eq(index);
        //    $(row).find('.PackedQty').val(clean($("#PackedQty").val()));
        //}
        var packedQty = clean($(row).find('.PackedQty').val());
        var batchType = $(row).find('.BatchTypeOutPut').text().trim();
        var actualQty = 0;
        var material_row, standard_skilled_labour_hour, standard_unskilled_labour_hour;
        var standard_machine_hour, batchsize, row;
        $("#production-packing-material-list tbody tr.new").each(function () {
            material_row = $(this);
            if ($(material_row).find(".BatchType").text().trim() == batchType) {
                actualQty = clean($(material_row).find(".StandardQty").text()) * packedQty;
                console.log(actualQty);
                $(material_row).find(".ActualQty").val(actualQty);
                $(material_row).find(".IssueQty").val(actualQty);
                $(material_row).find('.Variance').val(actualQty - clean($(material_row).find(".IssueQty").val()));
            }
        });

        $("#production-packing-process-list tbody tr").each(function () {
            row = $(this);
            var SkilledLaboursStandard = clean($(row).find(".SkilledLaboursStandard").val());
            var UnSkilledLabourStandard = clean($(row).find(".UnSkilledLabourStandard").val());
            var MachineHoursStandard = clean($(row).find(".MachineHoursStandard").val());
            batchsize = clean($(row).find(".BatchSize").val());
            var value = packedQty / batchsize;
            standard_skilled_labour_hour = (value * SkilledLaboursStandard);
            standard_unskilled_labour_hour = (value * UnSkilledLabourStandard);
            standard_machine_hour = (value * MachineHoursStandard);
            $(row).find(".txtStandardSkilledLabourHour").val(standard_skilled_labour_hour);
            $(row).find(".txtStandardUnSkilledLabourHour").val(standard_unskilled_labour_hour);
            $(row).find(".txtStandardMachineHour").val(standard_machine_hour);
        });
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPackingItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item: function (event, item) {
        var self = ProductionPacking;
        var Name = item.name;
        var ID = item.id;
        var Unit = item.unit;
        var UnitID = item.unitId;
        var Code = item.code;
        var ProductID = item.productId;
        var ProductGroupID = item.productGroupId
        var BatchSize = item.batchSize;

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#UOM").val(Unit);
        $("#ItemCode").val(Code);
        $("#ProductID").val(ProductID);
        $("#ProductGroupID").val(ProductGroupID);
        $("#BatchSize").val(BatchSize);
        $("#UnitID").val(UnitID);
        if ($("#production-packing-material-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#production-packing-material-list tbody').empty();
                $("#btnGetPackingMaterials").css({ 'display': 'block' });

            })
        }
        self.get_batches_with_stock();
    },

    count_items: function () {
        var count = $('#production-packing-material-list tbody').length;
        $('#item-count').val(count);
        //  self.get_packing_process();
    },

    get_batches_with_stock: function () {
        var ItemID = $('#ProductID').val();
        var DefaultPackingStoreID = $('#DefaultPackingStoreID').val();
        $('#BatchID').html('');
        var options = "<option value='0'>Select</option>";
        $.ajax({
            url: '/Masters/Batch/GetBatch/',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: ItemID,
                StoreID: DefaultPackingStoreID
            },
            success: function (response) {
                if (response.Status == 'success') {
                    $.each(response.data, function (i, record) {
                        options += "<option value='" + record.ID + "'>" + record.BatchNo + "</option>";
                    });
                }
                $('#BatchID').html(options);
            }
        });
        $('#production-packing-output-list tbody').empty();
    },

    add_additional_issue: function () {
        var self = ProductionPacking;
        self.error_count = 0;
        self.error_count = self.validate_additional();
        if (self.error_count == 0) {
            var rowNo = self.get_next_row_sno($('#production-packing-material-list'));
            var qty = clean($('#AdditionalItemQty').val());
            var itemID = $('#AdditionalItemID').val();
            var name = $('#txtAddnitionalIssue').val();
            var uom = $('#AdditionalItemUOM').val();
            var unitID = $('#AdditionalUnitID').val();
            var batchtype = $('#AdditionalBatchTypeID option:selected').text() == "Select" ? "" : $('#AdditionalBatchTypeID option:selected').text();
            var batchtypeID = $('#AdditionalBatchTypeID option:selected').text() == "Select" ? 0 : $('#AdditionalBatchTypeID').val();
            var storeID = $('#DefaultPackingStoreID').val();
            var code = $('#AdditionalCode').val();
            var stock = $('#Stock').val();
            var rowHtml = '<tr>';
            rowHtml += '<td>' + rowNo + ' <input type="hidden" class="ItemID" value="' + itemID + '" >'
                    + ' <input type="hidden" class="AvailableStock" value="' + stock + '" >'
                    + ' <input type="hidden" class="BatchTypeID" value="' + batchtypeID + '" >'
                    + '<input type="hidden" class="IsAdditionalIssue" value="' + 1 + '" >'
                    + '<input type="hidden" class="IsMaterialReturn" value="' + 0 + '" >'
                    + '<input type="hidden" class="StoreID" value="' + storeID + '" >'
                    + '<input type="hidden" class="UnitID" value="' + unitID + '" ></td>';

            rowHtml += '<td>' + code + '</td>';
            rowHtml += '<td >' + name + '</td>';
            rowHtml += '<td >' + uom + '</td>';
            rowHtml += '<td class="BatchType"> ' + batchtype + ' </td>';
            rowHtml += '<td class="mask-production-qty Stock">' + stock + '</td>';
            rowHtml += '<td  class="mask-production-qty StandardQty">' + 0 + '</td>';
            rowHtml += '<td  class="mask-production-qty ActualQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty IssueQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty Variance">' + 0 + '</td>';
            rowHtml += '<td><input type="text" class="md-input  Remarks"   /></td>';
            rowHtml += ' <td class="uk-text-center"><a  class="remove-item" > <i class="md-btn-icon-small uk-icon-remove"></i> </a> </td>';
            rowHtml += '</tr>';
            var $row = $(rowHtml);
            $('#production-packing-material-list tbody').append($row);
            app.masked_inputs();
            self.clear_Additional_Issue();
        }

    },

    clear_Additional_Issue: function () {
        var self = ProductionPacking;
        $('#AdditionalBatchID').val('');
        $("#AdditionalItemQty").val('')
        $('#AdditionalItemID').val('');
        $("#txtAddnitionalIssue").val('')
        $('#AdditionalItemUOM').val('');
        $("#UnitID").val('')
        $('#AdditionalBatchTypeID').val('');
        $('#Category').val('');
        self.arrange_serial();
    },

    clear_material_return: function () {
        var self = ProductionPacking;
        $('#ReturnItemID').val('');
        $("#ReturnStock").val('')
        $('#ReturnCode').val('');
        $("#ReturnItemUOM").val('')
        $('#ReturnItemQty').val('');
        $("#ReturnItemName").val('')


    },

    get_next_row_sno: function (tblObj) {
        var rowNo = $(tblObj).find('tbody tr').length + 1;
        return rowNo;
    },

    get_packing_materials: function () {
        var self = ProductionPacking;
        var tr;
        var serial_no;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var ItemID = $("#ItemID").val();
        var BatchID = $('#BatchID').val();
        var ProductGroupID = $('#ProductGroupID').val();
        var BatchTypeID = $('#BatchTypeID').val();
        var PackedQty = clean($("#PackedQty").val());
        var BatchType = $('#BatchTypeID option:selected').text();
        var StoreID = $('#DefaultPackingStoreID').val();
        //if ($('#production-packing-output-list tbody').find('tr.new.' + BatchType).length == 1) {
        //    var index = $('#production-packing-output-list tbody tr').index($('#production-packing-output-list tbody').find('tr.new.' + BatchType));
        //    self.calculate_actual_qty(index);
        //    return;
        //}
        self.get_packing_process();
        self.get_packing_material();
        self.get_packing_output_by_batchID();
        self.arrange_serial();

    },

    get_packing_material: function () {
        var self = ProductionPacking;
        var ItemID = $("#ItemID").val();
        var BatchID = $('#BatchID').val();
        var ProductGroupID = $('#ProductGroupID').val();
        var BatchTypeID = $('#BatchTypeID').val();
        var PackedQty = clean($("#PackedQty").val());
        var BatchType = $('#BatchTypeID option:selected').text();
        var StoreID = $('#DefaultPackingStoreID').val();

        $.ajax({
            url: '/Manufacturing/ProductionPacking/GetPackingMaterials',
            data: {
                ItemID: ItemID,
                BatchID: BatchID,
                ProductGroupID: ProductGroupID,
                BatchTypeID: BatchTypeID,
                PackedQty: PackedQty,
                BatchType: BatchType,
                StoreID: StoreID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                $response.each(function () {
                    $(this).removeClass("saved").addClass("new");
                });
                app.format($response);
                $("#production-packing-material-list tbody").append($response);
                fh_material.resizeHeader();
                serial_no = $('#production-packing-output-list tbody tr').length + 1;
                tr = '<tr class="new ' + BatchType + '">'
                    + '<td class="serial uk-text-center">'
                    + serial_no
                    + '</td>'
                    + '<td class="Date">' + $("#Date").val() + '</td>'
                    + '<td>' + $("#ItemName").val()
                    + '     <input type="hidden" class="ItemID" value="' + $("#ItemID").val() + '" />'
                    + '     <input type="hidden" class="BatchTypeID" value="' + $("#BatchTypeID").val() + '" />'
                    + '     <input type="hidden" class="BatchID" value="' + $("#BatchID").val() + '" />'
                    + '     <input type="hidden" class="UnitID" value="' + $("#UnitID").val() + '" />'
                    + '     <input type="hidden" class="Status" value="' + 1 + '" /></td>'
                    + '<td class="batchNo">' + $('#BatchID option:selected').text() + '</td>'
                    + '<td>'
                    + '    <input type="text" class="md-input PackedQty mask-production-qty" value="' + clean($('#PackedQty').val()) + '"/>'
                    + '</td>'
                    + '<td class="BatchTypeOutPut"> ' + BatchType
                    + '</td>'
                    + '<td data-md-icheck class="uk-hidden"><input type="checkbox" class="IsQCCompleted" />'
                    + '</td>'
                    + '</tr>';
                $tr = $(tr);
                app.format($tr);
                $('#production-packing-output-list tbody').prepend($tr);
                //self.clear_add_fields();
                self.count_items();
                $("#btnGetPackingMaterials").css({ 'display': 'none' });
            }
        });
    },

    get_packing_output_by_batchID: function () {
        var self = ProductionPacking;
        var tr = '';
        var serial_no;

        // self.error_count = self.validate_item();
        var BatchID = $('#BatchID').val();
        var qc;
        $.ajax({
            url: '/Manufacturing/ProductionPacking/GetPackingOutputByBatchID',
            data: {
                BatchID: BatchID
            },
            dataType: "json",
            type: "GET",
            success: function (output_items) {
                var $output_list = $('#production-packing-output-list tbody');
                // $output_list.html('');

                $.each(output_items, function (i, output_item) {
                    serial_no = $('#production-packing-output-list tbody tr').length + 1;
                    tr = '';
                    qc = (output_item.IsQCCompleted == true) ? "Yes" : "No";

                    tr += '<tr>'
                        + '<td class="serial uk-text-center">'
                        + serial_no
                        + '</td>'
                        + '<td >' + output_item.Date + '</td>'
                        + '<td>' + output_item.ItemName
                        + '     <input type="hidden" class="Status" value="' + 1 + '" /></td>'
                        + '<td >' + output_item.BatchNo + '</td>'
                        + '<td class="mask-production-qty">' + output_item.PackedQty + '</td>'
                        + '<td > ' + output_item.BatchType
                        + '</td>'
                        + '<td class="uk-hidden">' + qc
                        + '</td>'
                        + '</tr>';
                    var $tr = $(tr);
                    app.format($tr);
                    $output_list.append($tr);
                });


            }
        });

    },

    get_packing_process: function () {
        var self = ProductionPacking;
        var tr;
        var serial_no;
        // self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Manufacturing/ProductionPacking/GetPackingProcess',
            data: {
                ItemID: $("#ItemID").val(),
                ProductGroupID: $('#ProductGroupID').val(),
                BatchTypeID: $('#BatchTypeID option:selected').val(),
                PackedQty: clean($("#PackedQty").val()),
                BatchType: $('#BatchTypeID option:selected').text()
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                app.format($response);
                $("#production-packing-process-list tbody").append($response);
                fh_process.resizeHeader();
            }
        });
    },

    arrange_serial: function () {

        $.each($('#production-packing-output-list tbody tr'), function (i, out) {
            var row = $(this).closest('tr');
            $(row).find(".serial").text(i + 1);
            i++;
        });

    },

    get_packing_processes: function () { },

    add_additional_material: function () { },

    add_return_material: function () {
        var self = ProductionPacking;
        self.error_count = 0;
        self.error_count = self.validate_return();
        if (self.error_count == 0) {
            var rowNo = self.get_next_row_sno($('#production-packing-material-list'));
            var qty = -1 * $('#ReturnItemQty').val();
            var itemID = $('#ReturnItemID').val();
            var name = $('#ReturnItemName').val();
            var uom = $('#ReturnItemUOM').val();
            var unitID = $('#ReturnUnitID').val();
            var batchtype = $('#BatchTypeID option:selected').text() == "Select" ? "" : $('#BatchTypeID option:selected').text();
            var batchtypeID = $('#BatchTypeID').val();
            var storeID = $('#DefaultPackingStoreID').val();
            var code = $('#ReturnCode').val();
            var stock = $('#ReturnStock').val();
            var rowHtml = '<tr>';
            rowHtml += '<td>' + rowNo + ' <input type="hidden" class="ItemID" value="' + itemID + '" >'
                    + ' <input type="hidden" class="AvailableStock" value="' + stock + '" >'
                    + ' <input type="hidden" class="BatchTypeID" value="' + batchtypeID + '" >'
                    + '<input type="hidden" class="IsAdditionalIssue" value="' + 0 + '" >'
                    + '<input type="hidden" class="IsMaterialReturn" value="' + 1 + '" >'
                      + '<input type="hidden" class="StoreID" value="' + storeID + '" >'
                    + '<input type="hidden" class="UnitID" value="' + unitID + '" ></td>';

            rowHtml += '<td>' + code + '</td>';
            rowHtml += '<td >' + name + '</td>';
            rowHtml += '<td >' + uom + '</td>';
            rowHtml += '<td class="BatchType"> ' + batchtype + ' </td>';
            rowHtml += '<td class="mask-production-qty Stock">' + stock + '</td>';
            rowHtml += '<td  class="mask-production-qty StandardQty">' + 0 + '</td>';
            rowHtml += '<td  class="mask-production-qty ActualQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty IssueQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty Variance">' + 0 + '</td>';
            rowHtml += '<td><input type="text" class="md-input  Remarks"  /></td>';
            rowHtml += ' <td class="uk-text-center"><a  class="remove-item" > <i class="md-btn-icon-small uk-icon-remove"></i> </a> </td>';
            rowHtml += '</tr>';
            var $row = $(rowHtml);
            $('#production-packing-material-list tbody').append($row);
            app.masked_inputs();
            self.clear_material_return();
        }

    },

    clear_add_fields: function () {
        var self = ProductionPacking;
        $('#BatchTypeID').val('');
        $("#PackedQty").val('')
        self.arrange_serial();
    },

    get_stock: function () {
        var self = ProductionPacking;
        var row = $(this).closest('tr');
        var BatchID = $('#BatchID').val();
        var StoreID = $('#DefaultPackingStoreID').val();
        var ProductionGroupID = $('#ProductGroupID').val();
        if (StoreID == 0 || StoreID == '' || BatchID == 0 || BatchID == '') {
            return;
        }
        $.ajax({
            url: '/Masters/Batch/GetBatchWiseStockForPackingSemiFinishedGood/',
            dataType: "json",
            type: "POST",
            data: {
                BatchID: BatchID,
                StoreID: StoreID,
                ProductionGroupID: ProductionGroupID
            },
            success: function (response) {
                if (response.Status == 'success') {
                    $('#AvailableStock').val(response.data);
                }
            }
        });
    },

    get_additional_stock: function () {
        var self = ProductionPacking;

        var BatchID = $('#AdditionalBatchID option:selected').val();
        var StoreID = $('#StoreID option:selected').val();
        if (StoreID == 0 || StoreID == '' || BatchID == 0 || BatchID == '') {
            return;
        }
        $.ajax({
            url: '/Masters/Batch/GetBatchWiseStock/',
            dataType: "json",
            type: "POST",
            data: {
                BatchID: BatchID,
                StoreID: StoreID
            },
            success: function (response) {
                if (response.Status == 'success') {
                    $('#Stock').val(response.data[0].Stock);
                }
            }
        });
    },

    check_stock: function () {
        var self = ProductionPacking;
        var row = $(this).closest('tr');
        var AvailableStock = clean($(row).find('.AvailableStock').val());
        var IssueQty = clean($(row).find('.IssueQty').val());
        console.log(AvailableStock);
        console.log(IssueQty);
        if (IssueQty > AvailableStock) {
            app.show_error("Insufficient Stock");
            app.add_error_class($(row).find('.IssueQty'));
        }
    },

    get_data: function () {
        var self = ProductionPacking;
        var model = {
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            ID: $("#ID").val(),
            ItemID: $("#ItemID").val(),
            ProductID: $("#ProductID").val(),
            ProductGroupID: $("#ProductGroupID").val(),
            BatchID: $("#BatchID").val(),
            BatchSize: $("#BatchSize").val(),
            Remarks : $("#Remarks").val()
        };
        model.Output = [];
        model.Materials = [];
        model.Process = [];
        var obj;
        var row;
        $("#production-packing-output-list tbody tr .ItemID").each(function () {
            row = $(this).closest('tr');
            obj = {
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                ItemID: clean($(row).find(".ItemID").val()),
                BatchID: clean($(row).find(".BatchID").val()),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                UnitID: clean($(row).find(".UnitID").val()),
                Date: $(row).find(".Date").text(),
                ProductionSequence: 1,
                PackedQty: clean($(row).find(".PackedQty").val()),
                StoreID: clean($("#DefaultPackingStoreID").val()),
                IsQCCompleted: $(row).find(".IsQCCompleted").is(":checked"),
            }
            model.Output.push(obj);
        });
        $("#production-packing-material-list tbody tr").each(function () {
            row = $(this);
            obj = {
                ItemID: clean($(row).find(".ItemID").val()),
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                UnitID: clean($(row).find(".UnitID").val()),
                AvailableStock: clean($(row).find(".AvailableStock").val()),
                IssueQty: clean($(row).find(".IssueQty").val()),
                StandardQty: clean($(row).find(".StandardQty").text()),
                StoreID: clean($(row).find(".StoreID").val()),
                BatchID: clean($("#BatchID").val()),
                PackingMaterialMasterID: clean($(row).find(".PackingMaterialMasterID").val()),
                Remarks: $(row).find(".Remarks").val(),
                IsAdditionalIssue: clean($(row).find(".IsAdditionalIssue").val()) == 0 ? false : true,
                IsMaterialReturn: clean($(row).find(".IsMaterialReturn").val()) == 0 ? false : true,
                variance: clean($(row).find(".Variance").val()),

            };
            model.Materials.push(obj);
        });
        $("#production-packing-process-list tbody tr").each(function () {
            row = $(this);
            obj = {
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                Stage: $(row).find(".Stage").text().trim(),
                ProcessName: $(row).find(".ProcessName").text().trim(),
                StartTimeStr: $(row).find(".txtStartTime").val(),
                EndTimeStr: $(row).find(".txtEndTime").val(),
                SkilledLaboursStandard: clean($(row).find(".txtStandardSkilledLabourHour").val()),
                SkilledLaboursActual: clean($(row).find(".txtActualSkilledLabourHour").val()),
                UnSkilledLabourStandard: clean($(row).find(".txtStandardUnSkilledLabourHour").val()),
                UnSkilledLabourActual: clean($(row).find(".txtActualUnSkilledLabourHour").val()),
                MachineHoursStandard: clean($(row).find(".txtStandardMachineHour").val()),
                MachineHoursActual: clean($(row).find(".txtActualMachineHour").val()),
                DoneBy: $(row).find(".txtDoneBy").val(),
                Status: $(row).find(".txtStatus").val(),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                PackingProcessDefinitionTransID: clean($(row).find('.PackingProcessDefinitionTransID').val()),
                BatchSize: clean($(row).find('.BatchSize').val()),
            };
            model.Process.push(obj);
        });
        return model;

    },

    save: function () {
        var self = ProductionPacking;
        var data = self.get_data();
        var message = "";
        var url;
        if ($(this).hasClass('save-as-draft')) {
            data.IsDraft = true;
            data.IsCompleted = false;
            self.error_count = self.validate_draft();
            message = 'Packing Saved as Draft';
            url = '/Manufacturing/ProductionPacking/SaveAsDraft/';
        } else if ($(this).hasClass('complete')) {
            data.IsDraft = false;
            data.IsCompleted = true;
            self.error_count = self.validate_form();
            message = 'Packing Completed';
            var url = '/Manufacturing/ProductionPacking/Save/';
        }
        else {
            data.IsDraft = false;
            data.IsCompleted = false;
            self.error_count = self.validate_form();
            message = 'Packing Saved';
            url = '/Manufacturing/ProductionPacking/Save/';
        }

        if (self.error_count > 0) {
            return;
        }

        $(".save-as-draft, .save,.complete").css({ 'display': 'none' });
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            data: { Packing: data },
            success: function (response) {
                if (response.Status == 'success') {
                    app.show_notice(message);
                    window.location = '/Manufacturing/ProductionPacking/Index';
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".save-as-draft, .save,.complete").css({ 'display': 'block' });

                }
            }
        });
    },

    complete: function () {
        var self = ProductionPacking;
        var data = self.get_data();
        var message = "";
        if ($('#production-packing-material-list tbody tr').length > 0) {
            app.confirm("Once you complete, batch can't be used for further packing", function () {

                data.IsDraft = false;
                data.IsCompleted = true;
                self.error_count = self.validate_form();
                message = 'Packing Completed';


                if (self.error_count > 0) {
                    return;
                }

                $(".save-as-draft, .save,.complete").css({ 'display': 'none' });
                $.ajax({
                    url: '/Manufacturing/ProductionPacking/Save/',
                    dataType: "json",
                    type: "POST",
                    data: { Packing: data },
                    success: function (response) {
                        if (response.Status == 'success') {
                            app.show_notice(message);
                            window.location = '/Manufacturing/ProductionPacking/Index';
                        } else {
                            app.show_error("Failed to save");
                            $(".save-as-draft, .save,.complete").css({ 'display': 'block' });

                        }
                    }
                });
            })
        }
    },

    validate_item: function () {
        var self = ProductionPacking;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    validate_form: function () {
        var self = ProductionPacking;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = ProductionPacking;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_additional: function () {
        var self = ProductionPacking;
        if (self.rules.on_additional_issue.length > 0) {
            return form.validate(self.rules.on_additional_issue);
        }
        return 0;
    },

    validate_return: function () {
        var self = ProductionPacking;
        if (self.rules.on_return.length > 0) {
            return form.validate(self.rules.on_return);
        }
        return 0;
    },

    rules: {
        on_add_item: [
           {
               elements: "#ItemID",
               rules: [
                   { type: form.required, message: "Please choose a valid item" },
                   { type: form.non_zero, message: "Please choose a valid item" },

               ]
           },
           {
               elements: "#BatchID",
               rules: [
                   { type: form.required, message: "Please select Batch" },
                   { type: form.non_zero, message: "Please select Batch" },
                   { type: form.positive, message: "Please select Batch" },
               ]
           },
           {
               elements: "#PackedQty",
               rules: [
                   { type: form.required, message: "Please enter Packed Quantity" },
                   { type: form.non_zero, message: "Please enter Packed Quantity" },
                   { type: form.positive, message: "Please enter Packed Quantity" },
                    {
                        type: function (element) {
                            var error = false;
                            var packedqty = clean($('#PackedQty').val());

                            if (packedqty % 1 != 0)
                                error = true;
                            return !error;
                        }, message: 'Invalid packed quantity'
                    },
               ]
           },


           {
               elements: "#BatchTypeID",
               rules: [
                   { type: form.required, message: "Please select Batch Type" },
                   { type: form.non_zero, message: "Please select Batch Type" },
                   { type: form.positive, message: "Please select Batch Type" },

               ]
           }
        ],
        on_submit: [
              {
                  elements: ".txtActualSkilledLabourHour",
                  rules: [
                      { type: form.positive, message: "Invalid actual skilled labour" },
                    {
                        type: function (element) {
                            var row = $(element).closest("tr");
                            return clean($(row).find(".txtStandardSkilledLabourHour").val()) == 0 ? true : form.non_zero(element);
                        }, message: "Invalid actual skilled labour"
                    },

                  ]
              },
            {
                elements: ".txtActualUnSkilledLabourHour",
                rules: [
                    { type: form.positive, message: "Invalid unskilled labour hour" },
                    {
                        type: function (element) {
                            var row = $(element).closest("tr");
                            return clean($(row).find(".txtStandardUnSkilledLabourHour").val()) == 0 ? true : form.non_zero(element);
                        }, message: "Invalid unskilled labour"
                    },

                ]
            },
            {
                elements: ".txtActualMachineHour",
                rules: [
                    { type: form.positive, message: "Invalid machine hour" },
                    {
                        type: function (element) {
                            var row = $(element).closest("tr");
                            return clean($(row).find(".txtStandardMachineHour").val()) == 0 ? true : form.non_zero(element);
                        }, message: "Invalid machine minutes"
                    }
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
               elements: "#ItemID",
               rules: [
                   { type: form.required, message: "Please choose a valid item" },
                   { type: form.non_zero, message: "Please choose a valid item" },

               ]
           },
           {
               elements: "#production-packing-material-list .IssueQty",
               rules: [
                   {
                       type: function (element) {
                           return clean($(element).closest('tr').find('.AvailableStock').val()) >= clean($(element).val());
                       }, message: "Insufficient stock"
                   },

                    {
                        type: function (element) {
                            var error = true;
                            $('#production-packing-material-list tbody tr').each(function () {
                                var issueqty = clean($(this).find('.IssueQty').val());
                                var actualqty = clean($(this).find('.ActualQty').val());
                                if (issueqty > 0 && issueqty <= actualqty) {
                                    return error = false;
                                }
                            });
                            return !error;
                        }, message: "Invalid issue quantity"
                    },

                {
                    type: function (element) {
                        var error = false;
                        $('#production-packing-material-list tbody tr').each(function () {
                            var issueqty = clean($(this).find('.IssueQty').val());
                            var actualqty = clean($(this).find('.ActualQty').val());
                            if (issueqty < 0 && issueqty > actualqty) {
                                error = true;
                            }
                        });
                        return !error;
                    }, message: "Invalid issue quantity"
                },


               ]
           },
           {
               elements: "#production-packing-material-list .StoreID",
               rules: [
                   { type: form.required, message: "Please select store" },
               ]
           },
           {
               elements: "#production-packing-material-list .BatchID",
               rules: [
                   { type: form.required, message: "Please select batch" },
               ]
           },
           {
               elements: "#IsBatchSuspended",
               rules: [
                   {
                       type: function (element) {
                           var error = false;
                           return clean($(element).val()) == 0;
                       }, message: "Batch Completed"
                   },
               ]
           },
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
                  elements: "#IsBatchSuspended",
                  rules: [
                      {
                          type: function (element) {
                              var error = false;
                              return clean($(element).val()) == 0;
                          }, message: "Batch Completed"
                      },
                  ]
              },
        ],
        on_return: [
        {
            elements: "#ReturnItemName",
            rules: [

                { type: form.required, message: "Please select one item" },

            ]
        },
        {
            elements: "#ReturnItemQty",
            rules: [
                   { type: form.required, message: "Please enter quantity" },
                    { type: form.non_zero, message: "Please enter quantity" },
            ]
        },

        ],
        on_additional_issue: [
            {
                elements: "#AdditionalItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },

                ]
            },
            {
                elements: "#AdditionalBatchTypeID",
                rules: [
                    {
                        type: function (element) {
                            return $(Category).val() == "Finished Goods" ? form.required(element) : true;
                        }, message: "Please select Batchtype"
                    }
                ]
            },
            {
                elements: "#AdditionalItemQty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter quantity" },
                    {
                        type: function (element) {
                            var stock = clean($("#Stock").val());
                            return stock >= clean($(element).val());
                        }, message: "Insufficient stock"
                    }
                ]
            },
        ]
    },
}



