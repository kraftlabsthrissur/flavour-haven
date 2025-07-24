var receipt_item_select_table;
var Repacking = {
    init: function () {
        var self = Repacking;

        self.bind_events();
        Item.packing_material_list();
        $('#packing-material-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtAdditionalIssueQty",
            modal: "#select-item",
            initiatingElement: "#txtAdditionalIssue",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        item_list = Item.production_packing_item_list();
        item_select_table = $('#production-group-list').SelectTable({
            selectFunction: self.select_production_group,
            returnFocus: "#IssueBatchTypeID",
            modal: "#select-production-group",
            initiatingElement: "#txtItemIssue",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        item_list = Item.material_item_list();
        item_select_item_table = $('#material-item-list').SelectTable({
            selectFunction: self.select_material,
            returnFocus: "#txtReturnQuantity",
            modal: "#select-material-item",
            initiatingElement: "#txtReturnQuantity",
            startFocusIndex: 3,
            selectionType: "radio"
        });

        $('#production-issue-item-list').SelectTable({
            modal: "#select-production-issue-item",
            initiatingElement: "#txtItemReceipt",
            startFocusIndex: 3,
            selectionType: "radio"
        });
    },

    details: function () {
        var self = Repacking;
        fh_items = $("#sales-order-items-list").FreezeHeader();
        $(".cancel").on("click", self.cancel_confirm);
    },

    production_issue_item_list: function () {
        var self = Repacking;
        $list = $('#production-issue-item-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            self.production_issue_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                self.production_issue_list_table.api().column(index).search(this.value).draw();
            });
            receipt_item_select_table = $('#production-issue-item-list').SelectTable({
                selectFunction: self.select_production_group,
                returnFocus: "#BatchName",
                modal: "#select-production-issue-item",
                initiatingElement: "#txtProductGroupName",
                selectionType: "radio"
            });
        }
    },

    bind_events: function () {
        var self = Repacking;
        //Bind auto complete event for item 
        $.UIkit.autocomplete($('#issue-item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#issue-item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $.UIkit.autocomplete($('#receipt-item-autocomplete'), { 'source': self.get_receiptitems, 'minLength': 1 });
        $('#receipt-item-autocomplete').on('selectitem.uk.autocomplete', self.set_receiptitem);
        $("#btnOKPackingMaterial").on("click", self.select_item);
        $('#btnGetPackingMaterials').on("click", self.get_packing_materials);
        $.UIkit.autocomplete($('#additional-issue-item-autocomplete'), { 'source': self.get_additionalissueitem_AutoComplete, 'minLength': 1 });
        $('#additional-issue-item-autocomplete').on('selectitem.uk.autocomplete', self.set_additionalIssueItem_details);
        $.UIkit.autocomplete($('#materials-return-autocomplete'), { 'source': self.get_materialsReturn_AutoComplete, 'minLength': 1 });
        $('#materials-return-autocomplete').on('selectitem.uk.autocomplete', self.set_materialsReturn_details);
        $('#btnAddAdditionalItem').on('click', self.add_additional_issue);
        $('.btnSaveASDraft').on('click', self.save);
        $('.btnSave').on('click', self.check_qtyout);
        $('#BatchName').on('change', self.set_receiptbatch);
        $('#IssueBatchTypeID').on('change', self.get_batches_with_stock);
        $(".selectissue").on("click", self.get_issue);
        $("#btnOKIssueItem").on("click", self.select_issue);
        $("#btnAddReturnItem").on("click", self.add_material_return);
        $("body").on('click', ".cancel", self.cancel_confirm);
        $('body').on('keyup', '.PackedQty', self.calculate_actual_qty);

        $('#QuantityIn').on('keyup', self.calculate_standard_Receipt_qty);
        $('body').on('keyup', '.IssueQty', self.calculate_variance);
        $("body").on("click", ".remove-item", self.remove_item);
        $("#btnOKProductionGroup").on("click", self.select_production_group);
    },
    remove_item: function () {
        var self = Repacking;
        $(this).closest("tr").remove();
    },

    calculate_variance: function () {
        var self = Repacking;
        var issueQty = clean($(this).closest('tr').find('.IssueQty').val());
        var stdQtyForActualBatch = clean($(this).closest('tr').find('.ActualQty').val());
        var itemID = clean($(this).closest('tr').find(".ItemID").val());
        var issueitemid = $("#IssueItemID").val();
        var variance = 0;
        if (issueQty != '' && issueQty > 0) {
            if (stdQtyForActualBatch != '' && stdQtyForActualBatch > 0) {
                variance = stdQtyForActualBatch - issueQty;
            }
        }
        $(this).closest('tr').find('.Variance').val(variance);
        if (issueitemid == itemID) {
            $("#QuantityIn").val(issueQty);
            self.calculate_standard_Receipt_qty();
        }
    },

    save_confirm: function () {
        var self = Repacking
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            app.confirm_cancel("Do you want to Save", function () {
                self.save_repacking(false);
            }, function () {
            })
        }
    },

    cancel_confirm: function () {
        var self = Repacking
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    calculate_actual_qty: function (index) {
        var self = Repacking;
        var row = $(this).closest('tr');
        var packedQty = clean($(row).find('.PackedQty').val());
        var actualQty = 0;
        var material_row, itemID;;
        $("#QuantityOut").val(packedQty);
        var issueitemid = $("#IssueItemID").val();
        var qtyin = $("#QuantityIn").val();
        var IsAdditionalIssue, IsMaterialReturn;
        $("#repacking-materials-list tbody tr").each(function () {
            material_row = $(this).closest('tr');
            IsAdditionalIssue = clean($(material_row).find(".IsAdditionalIssue").val());
            IsMaterialReturn = clean($(material_row).find(".IsMaterialReturn").val());
            itemID = clean($(material_row).find(".ItemID").val());
            if (IsAdditionalIssue == 1)
                return;
            if (IsMaterialReturn == 1)
                return;
            actualQty = clean($(material_row).find(".StandardQty").text()) * packedQty;
            console.log(actualQty);
            $(material_row).find(".ActualQty").val(actualQty);
            $(material_row).find(".IssueQty").val(actualQty);
            $(material_row).find('.Variance').val(actualQty - actualQty);
            if (issueitemid == itemID) {
                $("#QuantityIn").val(actualQty);
                self.calculate_standard_Receipt_qty();
            }
        });
    },

    cancel: function () {
        $(".btnSave,.btnSaveASDraft,.cancel,.edit").css({ 'display': 'none' });
        $.ajax({
            url: '/Manufacturing/Repacking/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "RepackingIssue"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Repacking cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Manufacturing/Repacking/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");
                    $(".btnSave,.btnSaveASDraft,.cancel,.edit").css({ 'display': 'block' });
                }
            },
        });

    },

    add_material_return: function () {
        var self = Repacking;
        self.error_count = self.validate_material_return();
        if (self.error_count == 0) {
            var rowNo = self.get_next_row_sno($('#repacking-materials-list'));
            var qty = -1 * $('#txtReturnQuantity').val();
            var itemID = $('#ReturnItemID').val();
            var uom = $('#txtReturnMatrialQUM').val();
            var unitID = $('#ReturnUnitID').val();


            var storeID = $('#DefaultPackingStoreID').val();
            var code = $('#ReturnCode').val();
            var stock = $('#ReturnStock').val();
            var ItemName = $('#txtmaterialsreturn').val();
            var batchtype = $('#IssueBatchTypeID option:selected').text() == "Select" ? "" : $('#IssueBatchTypeID option:selected').text();
            var batchtypeID = $('#receiptBatchTypeID option:selected').val();
            var rowHtml = '<tr>';
            rowHtml += '<td>' + rowNo + ' <input type="hidden" class="ItemID" value="' + itemID + '" >'
                    + ' <input type="hidden" class="AvailableStock" value="' + stock + '" >'
                    + ' <input type="hidden" class="BatchTypeID" value="' + batchtypeID + '" >'
                    + '<input type="hidden" class="UnitID" value="' + unitID + '" ></td>'
                    + '<input type="hidden" class="IsAdditionalIssue" value="' + 0 + '" >'
                    + '<input type="hidden" class="IsMaterialReturn" value="' + 1 + '" >'
                    + '<input type="hidden" class="StoreID" value="' + storeID + '" ></td>';
            rowHtml += '<td>' + code + '</td>';
            rowHtml += '<td >' + ItemName + '</td>';
            rowHtml += '<td >' + uom + '</td>';
            rowHtml += '<td class="BatchType"> ' + batchtype + ' </td>';
            rowHtml += '<td class="mask-production-qty Stock">' + stock + '</td>';
            rowHtml += '<td  class="mask-production-qty StandardQty">' + 0 + '</td>';
            rowHtml += '<td  class="mask-production-qty ActualQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty IssueQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty Variance">' + 0 + '</td>';
            rowHtml += ' <td class="uk-text-center"><a  class="remove-item" > <i class="md-btn-icon-small uk-icon-remove"></i> </a> </td>';
            rowHtml += '</tr>';
            var $row = $(rowHtml);
            $('#repacking-materials-list tbody').append($row);
            app.masked_inputs();
            self.clear_Material_Return();
        }
    },

    clear_Material_Return: function () {
        var self = Repacking;
        $('#txtReturnQuantity').val('');
        $('#ReturnItemID').val('');
        $("#txtReturnMatrialQUM").val('')
        $('#ReturnUnitID').val('');
        $("#ReturnCode").val('')
        $('#txtmaterialsreturn').val('');
        $("#ReturnStock").val('')

    },

    select_issue: function () {
        var self = Repacking;
        var radio = $('#production-issue-item-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".name").text().trim();
        var Code = $(row).find(".code").text().trim();
        var BatchSize = $(row).find(".batchSize").text();
        var ConversionFactorp2s = $(row).find(".ConversionFactorPtoS").val();
        $("#txtItemReceipt").val(Name);
        $("#BatchSize").val(BatchSize);
        $("#ReceiptItemID").val(ID);
        $("#ReceiptItemID").val(ID);
        $("#ReceiptConversionFactorP2S").val(ConversionFactorp2s);
        $('#ProductGroupID').trigger("change");
        $('#ReceiptItemID').trigger("change");
        $('#IssueItemID').trigger("change");

        self.clear_receipt_item();
    },
    calculate_standard_Receipt_qty: function () {
        var self = Repacking;
        var IssueConversionFactor = clean($("#IssueConversionFactorP2S").val());
        var ReceiptConversionFactor = clean($("#ReceiptConversionFactorP2S").val());
        var IssueQuantity = clean($("#QuantityIn").val());
        var standard_qty = (IssueQuantity * (1 / IssueConversionFactor)) * ReceiptConversionFactor;
        var receiptitemId = clean($("#ReceiptItemID").val());
        if (receiptitemId > 0) {
            $("#StandardQty").val(standard_qty);
        }
        else {
            $("#StandardQty").val(0.0);
        }
    },
    get_issue: function () {
        var self = Repacking;

        var ProductionGroupID = $("#ProductGroupID").val();
        $.ajax({
            url: '/Masters/Item/GetReciptItemsItemList/',
            dataType: "json",
            type: "GET",
            data: {
                ProductionGroupID: ProductionGroupID,
            },
            success: function (response) {
                self.production_issue_list_table.fnDestroy();
                var $production_list = $('#production-issue-item-list tbody');
                $production_list.html('');
                var tr = '';
                $.each(response, function (i, production_issue) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1)
                        + '     <input type="hidden" class="ConversionFactorPtoS" value="' + production_issue.ConversionFactorPtoS + '" />'
                        + "</td>"
                        + " <td class='uk-text-center spclCheck' data-md-icheck><input type='radio' name='inputrequestno' id='requestno-id' class='itemID' value=" + production_issue.ItemID + " /></td>"
                        + "<td class=code>" + production_issue.Code + "</td>"
                        + "<td class=name>" + production_issue.ItemName + "</td>"
                        + "<td class=batchSize>" + production_issue.BatchSize + "</td>"
                    + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $production_list.append($tr);
                });
                self.production_issue_item_list();
            },
        });
    },

    set_receiptbatch: function () {
        var self = Repacking;
        $('#receiptBatchName').val($('#BatchName').val());

    },

    get_items: function (release) {
        console.log($('#txtItemIssue').val());
        $.ajax({
            url: '/Masters/Item/GetPackingItemsForAutoComplete',
            data: {
                Hint: $('#txtItemIssue').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item: function (event, item) {
        var self = Repacking;
        var Name = item.value;
        var ID = item.id;
        var ProductID = item.productid;
        var ProductionGroupId = item.productiongroupid;

        $("#txtItemIssue").val(Name);
        $("#IssueItemID").val(ID);
        $("#ProductID").val(ProductID);
        $("#ItemID").val(ID);
        $("#IssueConversionFactorP2S").val(item.conversionfactorptos);
        $("#txtItemReceipt").val('');
        $("#ReceiptItemID").val('');
        $('#ProductGroupID').trigger("change");
        $('#ReceiptItemID').trigger("change");
        $('#IssueItemID').trigger("change");

        $("#ProductGroupID").val(ProductionGroupId);
        $('#ProductGroupID').trigger("change");
        $('#ReceiptItemID').trigger("change");
        $('#IssueItemID').trigger("change");

        self.get_batches_with_stock();
    },
    clear_issue_item: function () {
        var self = Repacking;
        var count = $("#repacking-materials-list tbody tr").length;
        if (count > 0) {
            app.confirm("Are you sure to change issue item", function () {
                $("#repacking-materials-list tbody").html('');
                $("#repacking-process-list tbody").html('');
                $("#repacking-output-list tbody").html('');
                //$("#IssueBatchTypeID").val('');
                $("#QuantityIn").val('');
                $("#ReceiptItemID").val('');
                $("#receiptBatchTypeID").val('');
                $("#StandardQty").val('');
                $("#QuantityOut").val('');

                $("#btnGetPackingMaterials").css({ 'display': 'block' });
            })
        }
        else {
            $("#repacking-materials-list tbody").html('');
            $("#repacking-process-list tbody").html('');
            $("#repacking-output-list tbody").html('');
            // $("#IssueBatchTypeID").val('');
            $("#QuantityIn").val('');
            $("#ReceiptItemID").val('');
            $("#receiptBatchTypeID").val('');
            $("#StandardQty").val('');
            $("#QuantityOut").val('');
        }

        $("#item-count").val('');
    },
    clear_receipt_item: function () {
        var self = Repacking;
        var count = $("#repacking-materials-list tbody tr").length;
        if (count > 0) {
            app.confirm("Are you sure to change receipt item", function () {
                $("#repacking-materials-list tbody").html('');
                $("#repacking-process-list tbody").html('');
                $("#repacking-output-list tbody").html('');
                $("#receiptBatchTypeID").val('');
                $("#QuantityOut").val('');
                $("#QuantityOut").removeAttr("disabled");
                $("#QuantityIn").removeAttr("disabled");
                $("#btnGetPackingMaterials").css({ 'display': 'block' });
            })
        }
        else {
            $("#repacking-materials-list tbody").html('');
            $("#repacking-process-list tbody").html('');
            $("#repacking-output-list tbody").html('');
            $("#receiptBatchTypeID").val('');
            $("#QuantityOut").val('');
            $("#QuantityOut").removeAttr("disabled");
        }

        $("#item-count").val('');
        self.calculate_standard_Receipt_qty();
    },
    set_receiptitem: function (event, item) {
        var self = Repacking;
        console.log(item);
        var self = Repacking;
        var Name = item.value;
        var ID = item.id;
        var BatchSize = item.BatchSize;
        var ReceiptItemID = item.receiptitemid;

        $("#txtItemReceipt").val(Name);
        //("#ItemID").val(ID);
        $("#BatchSize").val(BatchSize);
        $("#ReceiptConversionFactorP2S").val(item.conversionfactorptos);
        $("#ReceiptItemID").val(ReceiptItemID);
        $('#ProductGroupID').trigger("change");
        $('#ReceiptItemID').trigger("change");
        $('#IssueItemID').trigger("change");
        self.calculate_standard_Receipt_qty();
        self.clear_receipt_item();
    },

    get_receiptitems: function (release) {
        $.ajax({
            url: '/Masters/Item/GetReciptItemsForAutoComplete',
            data: {
                Hint: $('#txtItemReceipt').val(),
                productGroupId: $("#ProductGroupID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    select_item: function () {
        var self = Repacking;
        var radio = $('#select-packing-material tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").text().trim();
        var Stock = $(row).find(".Stock").val();
        var UnitID = $(row).find(".UnitID").val();
        var category = $(row).find(".ItemCategory").text();
        $('#AdditionalItemUOM').val(Unit);
        $("#txtAdditionalIssue").val(Name);
        $("#AdditionalItemID").val(ID);
        $("#Code").val(Code);
        $("#AdditionalUnit").val(Unit);
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
        //self.select_receipt_Item();

    },

    select_material: function () {
        var self = Repacking;
        var radio = $('#select-material-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var UnitID = $(row).find(".UnitID").val();
        var Stock = $(row).find(".Stock").val();
        $('#txtReturnMatrialQUM').val(Unit);
        $("#txtmaterialsreturn").val(Name);
        $("#ReturnItemID").val(ID);
        $("#ReturnCode").val(Code);
        $("#ReturnStock").val(Stock);
        $("#ReturnUnitID").val(UnitID);
        UIkit.modal($('#select-item')).hide();
        //self.select_receipt_Item();

    },

    get_packing_materials: function () {
        var self = Repacking;
        var tr;
        var serial_no;
        var Datetime;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        self.Datetime = self.get_date();
        $("#QuantityOut").attr('disabled', 'disabled');
        $("#QuantityIn").attr('disabled', 'disabled');
        var ItemID = $("#ReceiptItemID").val();
        var ItemName = $("#txtItemIssue").val();
        var IssueItemID = $("#ItemID").val();
        var Unit = $("#UOM").val();
        var ProductGroupID = $('#ProductGroupID').val();
        var QuantityIn = clean($('#QuantityIn').val());

        //var BatchTypeID = $('#receiptBatchTypeID').val();
        var PackedQty = clean($('#QuantityOut').val());
        var BatchType = $('#IssueBatchTypeID option:selected').text();
        var BatchTypeOut = $('#receiptBatchTypeID option:selected').text();
        var StoreID = $('#DefaultPackingStoreID').val();
        var ItemName = $('#txtItemReceipt').val();
        var BatchNo = $('#receiptBatchName option:selected').text();
        var BatchID = $('#receiptBatchName option:selected').val();
        //var BatchID = $('#receiptBatchName option:selected').val();
        var BatchTypeID = $('#receiptBatchTypeID option:selected').val();
        var IssueBatchTypeID = $('#IssueBatchTypeID option:selected').val();
        //var Quantity = $('#QuantityOut').val();
        var Datetime = $('#Datetime').val();
        //var date = new Date();
        var DateTime = self.get_datetime();
        $("#ReceiptBatchNo").val(BatchNo);
        $("#ReceiptBatchType").val(BatchType);
        $("#PackedQty").val(PackedQty);
        $("#ReceiptItemName").val(ItemName);
        //$("#Datetime").val(Datetime);
        $("#ReceiptBatchTypeID").val(BatchTypeID);

        self.get_packing_process();
        $.ajax({
            url: '/Manufacturing/Repacking/GetPackingMaterials',
            data: {
                ItemID: ItemID,
                ProductGroupID: ProductGroupID,
                BatchTypeID: BatchTypeID,
                PackedQty: PackedQty,
                BatchType: BatchType,
                StoreID: StoreID,
                ItemName: ItemName,
                Unit: Unit,
                QuantityIn: QuantityIn,
                IssueItemID: IssueItemID,
                IssueBatchTypeID: IssueBatchTypeID,
                BatchID: BatchID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                $response.each(function () {
                    $(this).removeClass("saved").addClass("new");
                });
                app.format($response);
                $("#repacking-materials-list tbody").append($response);
                //fh_material.resizeHeader();
                serial_no = $('#repacking-output-list tbody tr').length + 1;
                tr = '<tr>'
                    + '<td class="serial uk-text-center">'
                    + serial_no
                    + '</td>'
                    + '<td class="Date">' + DateTime + '</td>'
                    + '<td>' + ItemName
                    + '     <input type="hidden" class="ItemID" value="' + $("#ItemID").val() + '" />'
                    + '     <input type="hidden" class="BatchTypeID" value="' + $("#ReceiptBatchTypeID").val() + '" />'
                    + '     <input type="hidden" class="BatchID" value="' + $("#receiptBatchName").val() + '" />'
                    + '     <input type="hidden" class="UnitID" value="' + $("#UnitID").val() + '" />'
                    + '<input type="hidden" class="IsAdditionalIssue" value="' + 0 + '" >'
                    + '<input type="hidden" class="IsMaterialReturn" value="' + 0 + '" ></td>'
                    + '<td class="batchNo">' + BatchNo + '</td>'
                    + '<td>'
                    + '    <input type="text" class="md-input PackedQty mask-production-qty" value="' + PackedQty + '"/>'
                    + '</td>'
                    + '<td class="BatchTypeOutPut"> ' + BatchTypeOut
                    + '</td>'
                    + '<td data-md-icheck class="uk-hidden"><input type="checkbox" class="IsQCCompleted" />'
                    + '</td>'
                    + '</tr>';
                $tr = $(tr);
                app.format($tr);
                $('#repacking-output-list tbody').append($tr);
                $("#btnGetPackingMaterials").css({ 'display': 'none' });
                self.count();
            }


        });


    },

    count: function () {
        var self = Repacking;
        $("#item-count").val($("#repacking-output-list tbody tr").length);
    },

    validate_item: function () {
        var self = Repacking;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form: function () {
        var self = Repacking;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = Repacking;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_batch: function () {
        var self = Repacking;
        if (self.rules.on_batch.length) {
            return form.validate(self.rules.on_batch);
        }
        return 0;
    },
    validate_additional_issue: function () {
        var self = Repacking;
        if (self.rules.on_additional_issue.length) {
            return form.validate(self.rules.on_additional_issue);
        }
        return 0;
    },

    validate_material_return: function () {
        var self = Repacking;
        if (self.rules.on_material_return.length) {
            return form.validate(self.rules.on_material_return);
        }
        return 0;
    },

    get_packing_process: function () {
        var self = Repacking;
        var tr;
        var serial_no;
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Manufacturing/Repacking/GetPackingProcess',
            data: {
                ItemID: $("#ReceiptItemID").val(),
                ProductGroupID: $('#ProductGroupID').val(),
                BatchTypeID: $('#receiptBatchTypeID option:selected').val(),
                PackedQty: clean($("#PackedQty").val()),
                BatchType: $('#receiptBatchTypeID option:selected').text()
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                app.format($response);
                $("#repacking-process-list tbody").append($response);
                //fh_process.resizeHeader();
            }
        });
    },

    select_production_group: function () {
        var self = Repacking;
        var radio = $('#select-production-group tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").text().trim();
        var ProductId = $(row).find(".ProductID").val();
        var BatchSize = $(row).find(".BatchSize").val();
        var ConversionFactorP2s = $(row).find(".ConversionFactorPtoS").val();
        var ProductionGroupID = $(row).find(".ProductionGroupID").val();
        $("#txtItemIssue").val(Name);
        $("#ItemID").val(ID);
        $("#IssueItemID").val(ID);
        $("#UOM").val(Unit);
        $("#ProductID").val(ProductId);
        $("#BatchSize").val(BatchSize);
        $("#IssueConversionFactorP2S").val(ConversionFactorP2s);
        $("#ProductGroupID").val(ProductionGroupID);
        if ($("#production-packing-material-list tbody tr").length > 0 || $("#production-packing-process-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#production-packing-material-list tbody').empty();
                $('#production-packing-process-list tbody').empty();
            })
        }
        self.get_batches_with_stock();
        UIkit.modal($('#select-production-group')).hide();
        $("#txtItemReceipt").val('');
        $("#ReceiptItemID").val('');
        $('#ProductGroupID').trigger("change");
        $('#ReceiptItemID').trigger("change");
        $('#IssueItemID').trigger("change");
        self.clear_issue_item();
    },

    get_batches_with_stock: function () {
        var self = Repacking;
        self.error_count = 0;
        self.error_count = self.validate_batch();
        if (self.error_count > 0)
            return;
        var ItemID = $('#ItemID').val();
        var DefaultPackingStoreID = $('#DefaultPackingStoreID').val();
        var BatchTypeID = $('#IssueBatchTypeID option:selected').val();
        $('#BatchID').html('');
        var options = "<option value='0'>Select</option>";
        $.ajax({
            url: '/Masters/Batch/GetBatchBatchTypeWise/',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: ItemID,
                StoreID: DefaultPackingStoreID,
                BatchTypeID: BatchTypeID
            },
            success: function (response) {
                if (response.Status == 'success') {
                    $.each(response.data, function (i, record) {
                        options += "<option value='" + record.ID + "'>" + record.BatchNo + "</option>";
                    });
                }
                $('#BatchName').html(options);
                $('#receiptBatchName').html(options);
            }
        });
        $('#repacking-output-list tbody').empty();
        self.clear_issue_item();
    },

    get_additionalissueitem_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPackingMaterialForAutoComplete',
            data: {
                itemHint: $('#txtAdditionalIssue').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_materialsReturn_AutoComplete: function (release) {
        $.ajax({
            //  url: '/Masters/Item/GetPackingReturnForAutoComplete',
            url: '/Masters/Item/GetStockableItemsForAutoComplete',

            data: {
                itemHint: $('#txtmaterialsreturn').val(),
                ProductGroupID: $('#ProductGroupID').val(),
                ReceiptItemID: $('#ReceiptItemID').val(),
                IssueItemID: $('#IssueItemID').val()

            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_additionalIssueItem_details: function (event, item) {   // on select auto complete item
        var self = Repacking;
        $('#AdditionalItemUOM').val(item.unit);
        $("#txtAdditionalIssue").val(item.name);
        $("#AdditionalItemID").val(item.id);
        $("#Code").val(item.code);
        $("#AdditionalUnit").val(item.unit);
        $("#AdditionalUnitID").val(item.unitid);
        $("#Stock").val(item.stock);
        $("#Category").val(item.category);
        if (item.category == "Finished Goods") {
            $("#AdditionalBatchTypeID").prop("disabled", false);
        }
        else {
            $("#AdditionalBatchTypeID").prop("disabled", true);
        }
        returnFocus: "#txtQuantity";
    },

    set_materialsReturn_details: function (event, item) {   // on select auto complete item
        var self = Repacking;
        $('#txtReturnMatrialQUM').val(item.unit);
        $("#txtmaterialsreturn").val(item.name);
        $("#ReturnItemID").val(item.id);
        $("#ReturnCode").val(item.code);
        $("#ReturnStock").val(item.stock);
        $("#ReturnUnitID").val(item.unitid);
    },

    add_additional_issue: function () {
        var self = Repacking;

        self.error_count = self.validate_additional_issue()
        if (self.error_count == 0) {
            var rowNo = self.get_next_row_sno($('#repacking-materials-list'));
            var qty = clean($('#txtQuantity').val());
            var itemID = $('#AdditionalItemID').val();
            var uom = $('#AdditionalItemUOM').val();
            var unitID = $('#AdditionalUnitID').val();
            var storeID = $('#DefaultPackingStoreID').val();
            var code = $('#Code').val();
            var stock = $('#Stock').val();
            var ItemName = $('#txtAdditionalIssue').val();
            var batchtype = $('#AdditionalBatchTypeID option:selected').text() == "Select" ? "" : $('#AdditionalBatchTypeID option:selected').text();
            var batchtypeID = $('#AdditionalBatchTypeID option:selected').text() == "Select" ? 0 : $('#AdditionalBatchTypeID option:selected').val();

            var rowHtml = '<tr>';
            rowHtml += '<td>' + rowNo + ' <input type="hidden" class="ItemID" value="' + itemID + '" >'
                    + ' <input type="hidden" class="AvailableStock" value="' + stock + '" >'
                    + ' <input type="hidden" class="BatchTypeID" value="' + batchtypeID + '" >'
                    + '<input type="hidden" class="StoreID" value="' + storeID + '" >'
                    + '<input type="hidden" class="IsAdditionalIssue" value="' + 1 + '" >'
                    + '<input type="hidden" class="IsMaterialReturn" value="' + 0 + '" >'
                    + '<input type="hidden" class="UnitID" value="' + unitID + '" ></td>';

            rowHtml += '<td>' + code + '</td>';
            rowHtml += '<td >' + ItemName + '</td>';
            rowHtml += '<td >' + uom + '</td>';
            rowHtml += '<td class="BatchType"> ' + batchtype + ' </td>';
            rowHtml += '<td class="mask-production-qty Stock">' + stock + '</td>';
            rowHtml += '<td  class="mask-production-qty StandardQty">' + 0 + '</td>';
            rowHtml += '<td  class="mask-production-qty ActualQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty IssueQty">' + qty + '</td>';
            rowHtml += '<td class="md-input mask-production-qty Variance">' + 0 + '</td>';
            rowHtml += ' <td class="uk-text-center"><a  class="remove-item" > <i class="md-btn-icon-small uk-icon-remove"></i> </a> </td>';
            rowHtml += '</tr>';
            var $row = $(rowHtml);
            // app.format($row);
            $('#repacking-materials-list tbody').append($row);
            app.masked_inputs();
            self.clear_Additional_Issue();
        }
    },

    get_next_row_sno: function (tblObj) {
        var rowNo = $(tblObj).find('tbody tr').length + 1;
        return rowNo;
    },

    clear_Additional_Issue: function () {
        var self = Repacking;
        $('#txtAdditionalIssue').val('');
        $('#AdditionalBatchID').val('');
        $("#AdditionalItemQty").val('')
        $('#AdditionalItemID').val('');
        $("#txtAddnitionalIssue").val('')
        $('#AdditionalItemUOM').val('');
        $("#UnitID").val('')
        $('#AdditionalBatchTypeID').val('');
        $('#txtQuantity').val('');
        $('#Category').val('');
    },

    get_date: function () {
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var date = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();
        return date;
    },
    check_qtyout: function () {
        var self = Repacking;
        var Standard_qty = clean($("#StandardQty").val());
        var Qty_out = clean($("#repacking-output-list tbody tr .PackedQty").val());

        if (Qty_out > Standard_qty) {
            app.confirm("Quantity out is greater than standard quantity, are sure to continue", function () {
                self.save_confirm();
            })
        }
        else {
            self.save_confirm();
        }
    },

    save: function () {
        var self = Repacking;
        self.error_count = 0;
        self.error_count = self.validate_draft();
        if (self.error_count == 0) {
            self.save_repacking(true);
        }
    },

    save_repacking: function (IsDraft) {
        var self = Repacking;
        var data = self.get_data(IsDraft);
        var url;
        if (IsDraft == true) {
            url = '/Manufacturing/Repacking/SaveAsDraft/';
        }
        else {
            url = '/Manufacturing/Repacking/Save/';
        }
        data.IsCompleted = false;
        console.log(data);
        $(".btnSave,.btnSaveASDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            data: { repacking: data },
            success: function (response) {
                if (response.Status == 'success') {
                    app.show_notice('Repacking Saved');
                    window.location = '/Manufacturing/Repacking/Index';
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSave.btnSaveASDraft").css({ 'display': 'block' });
                }
            }
        });
    },

    get_data: function (IsDraft) {
        var self = Repacking;
        var model = {
            ID: $("#ID").val(),
            RepackingNo: $("#repackingNo").val(),
            RepackingDate: $("#repackingDate").val(),
            ItemIssue: $("#txtItemIssue").val(),
            IssueItemID: clean($("#ItemID").val()),
            IssueItemBatchID: $("#BatchName").val(),
            IssueItemBatchTypeID: clean($("#IssueBatchTypeID").val()),
            QuantityIn: clean($("#QuantityIn").val()),
            ItemReceipt: $("#txtItemReceipt").val(),
            ReceiptItemID: $('#ReceiptItemID').val(),
            ReceiptItemBatchID: $('#receiptBatchName').val(),
            ReceiptItemBatchTypeID: clean($('#receiptBatchTypeID').val()),
            QuantityOut: clean($('#QuantityOut').val()),
            ProductID: clean($("#ProductID").val()),
            ProductGroupID: clean($("#ProductGroupID").val()),
            BatchID: clean($("#BatchID").val()),
            BatchSize: clean($("#BatchSize").val()),
            BatchNo: $("#BatchNo").val(),
            IsDraft: IsDraft,
            StandardQty: clean($("#StandardQty").val()),
            Remark: $("#Remark").val()
        };
        model.Output = [];
        model.Materials = [];
        model.Process = [];
        var obj;
        var row;
        $("#repacking-output-list tbody tr").each(function () {
            row = $(this);
            obj = {
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                ItemID: clean($("#ReceiptItemID").val()),
                BatchID: clean($(".BatchID").val()),
                BatchTypeID: clean($("#receiptBatchTypeID").val()),
                UnitID: clean($(row).find(".UnitID").val()),
                Date: $("#repackingDate").val(),
                ProductionSequence: 1,
                QuantityOut: clean($('.PackedQty').val()),
                StoreID: clean($("#DefaultPackingStoreID").val()),
                IsQCCompleted: $(row).find(".IsQCCompleted").is(":checked"),
            }
            model.Output.push(obj);
        });
        $("#repacking-materials-list tbody tr").each(function () {
            row = $(this);
            obj = {
                ItemID: clean($(row).find(".ItemID").val()),
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                UnitID: clean($(row).find(".UnitID").val()),
                AvailableStock: clean($(row).find(".AvailableStock").val()),
                IssueQty: clean($(row).find(".IssueQty").val()),
                StandardQty: clean($(row).find(".ActualQty").text()),
                StoreID: clean($(row).find(".StoreID").val()),
                StandardQtyForStdBatch: clean($(row).find(".StandardQty").val()),
                Variance: clean($(row).find(".Variance").val()),
                IsAdditionalIssue: clean($(row).find(".IsAdditionalIssue").val()) == 0 ? false : true,
                IsMaterialReturn: clean($(row).find(".IsMaterialReturn").val()) == 0 ? false : true
            };
            model.Materials.push(obj);
        });
        $("#repacking-process-list tbody tr").each(function () {
            row = $(this);
            obj = {
                PackingIssueID: clean($(row).find(".PackingIssueID").val()),
                Stage: $(row).find(".Stage").text().trim(),
                ProcessName: $(row).find(".ProcessName").text().trim(),
                StartTime: $(row).find(".txtStartTime").val(),
                EndTime: $(row).find(".txtEndTime").val(),
                SkilledLaboursStandard: clean($(row).find(".txtStandardSkilledLabourHour").val()),
                SkilledLaboursActual: clean($(row).find(".txtActualSkilledLabourHour").val()),
                UnSkilledLabourStandard: clean($(row).find(".txtStandardUnSkilledLabourHour").val()),
                UnSkilledLabourActual: clean($(row).find(".txtActualUnSkilledLabourHour").val()),
                MachineHoursStandard: clean($(row).find(".txtStandardMachineHour").val()),
                MachineHoursActual: clean($(row).find(".txtActualMachineHour").val()),
                DoneBy: $(row).find(".txtDoneBy").val(),
                Status: $(row).find(".txtStatus").val(),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
            };
            model.Process.push(obj);
        });
        return model;

    },

    get_datetime: function () {
        var date = new Date();
        date = (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) + "-" + (date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + date.getFullYear();
        return date;
    },

    list: function () {
        var self = Repacking;
        $('#tabs-repacking').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-repacking":
                $list = $('#saved-repacking-list');
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

            var url = "/Manufacturing/Repacking/GetRepackingList?type=" + type;

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

                   { "data": "RepackingNo", "className": "RepackingNo" },
                   { "data": "repackingDate", "className": "repackingDate" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "BatchType", "className": "BatchType" },
                    {
                        "data": "Quantity", "searchable": false, "className": "Quantity",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Quantity + "</div>";
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/Repacking/Details/" + Id);

                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    rules: {
        on_batch: [
        {
            elements: "#ItemID",
            rules: [
                { type: form.non_zero, message: "Please choose a valid item" },
            ]
        },
        //{
        //    elements: "#IssueBatchTypeID",
        //    rules: [
        //      {
        //          type: form.required, message: "Please select issue batch type"
        //      },
        //    ]
        //},

        ],
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.non_zero, message: "Please choose a valid item" },
                ]
            },
            {
                elements: "#BatchName",
                rules: [
                       {
                           type: form.required, message: "Please select issue batch "
                       },
                        { type: form.non_zero, message: "Please select issue batch" },
                ]
            },
            {
                elements: "#receiptBatchTypeID",
                rules: [
                        {
                            type: form.required, message: "Please select receipt batch "
                        },
                ]
            },
            {
                elements: "#IssueBatchTypeID",
                rules: [
                  {
                      type: form.required, message: "Please select issue batch type"
                  },
                ]
            },
            {
                elements: "#receiptBatchTypeID",
                rules: [
                   {
                       type: form.required, message: "Please select receipt batch type"
                   },
                ]
            },
            {
                elements: "#BatchName",
                rules: [
                   {
                       type: form.required, message: "Please select issue batch "
                   },
                ]
            },
            {
                elements: "#QuantityIn",
                rules: [
                   { type: form.required, message: "Please enter quantity in" },
                    { type: form.positive, message: "Please enter positive value for quantiy in" },
                    { type: form.numeric, message: "Please enter numeric value for quantity in" },
                       { type: form.non_zero, message: "Please enter non zero value for quantity in" },
                ]
            },
            {
                elements: "#QuantityOut",
                rules: [
                   {
                       type: form.required, message: "Please enter quantity out"
                   },
                   { type: form.positive, message: "Please enter positive value for quantiy out" },
                   { type: form.numeric, message: "Please enter numeric value for quantity out" },
                   { type: form.non_zero, message: "Please enter non zero value for quantity out" },
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
              {
                  elements: "#QuantityIn",
                  rules: [
                     { type: form.required, message: "Please enter quantity in" },
                     { type: form.positive, message: "Please enter positive value for quantiy in" },
                     { type: form.numeric, message: "Please enter numeric value for quantity in" },
                     { type: form.non_zero, message: "Please enter non zero value for quantity in" },
                  ]
              },
              {
                  elements: "#QuantityOut",
                  rules: [
                     {
                         type: form.required, message: "Please enter quantity out"
                     },
                     { type: form.positive, message: "Please enter positive value for quantiy out" },
                     { type: form.numeric, message: "Please enter numeric value for quantity out" },
                     { type: form.non_zero, message: "Please enter non zero value for quantity out" },
                  ]
              },
        {
            elements: ".txtActualSkilledLabourHour",
            rules: [
                {
                    type: function (element) {
                        var error = false;
                        var skilledlabour = clean($(element).closest("tr").find(".txtStandardSkilledLabourHour").val());
                        var actual = clean($(element).val());
                        if (skilledlabour > 0) {
                            if (actual > 0) {
                                error = false;
                            }
                            else {
                                error = true;
                            }
                        }

                        return !error;

                    }, message: "Invalid skilled labour hour"
                },
            ]
        },
        {
            elements: ".txtActualUnSkilledLabourHour",
            rules: [
                {
                    type: function (element) {
                        var error = false;
                        var unskilledlabour = clean($(element).closest("tr").find(".txtStandardUnSkilledLabourHour").val());
                        var actual = clean($(element).val());
                        if (unskilledlabour > 0) {
                            if (actual > 0) {
                                error = false;
                            }
                            else {
                                error = true;
                            }
                        }
                        return !error;
                    }, message: "Invalid unskilled labour hour"
                },
            ]
        },
        {
            elements: ".txtActualMachineHour",
            rules: [
                {
                    type: function (element) {
                        var error = false;
                        var machinehour = clean($(element).closest("tr").find(".txtStandardMachineHour").val());
                        var actual = clean($(element).val());
                        if (machinehour > 0) {
                            if (actual > 0) {
                                error = false;
                            }
                            else {
                                error = true;
                            }
                        }
                        return !error;
                    }, message: "Invalid machine hour"
                },
            ]
        },
        {
            elements: ".IssueQty",
            rules: [
                {
                    type: function (element) {
                        var error;
                        var stock = clean($(element).closest('tr').find('.Stock').val());
                        var qty = clean($(element).val());
                        if (qty < 0)
                            error = false;
                        else if (qty <= stock)
                            error = false;
                        else
                            error = true;
                        return !error
                    }, message: "Insuficient stock"
                },

                {
                    type: function (element) {
                        var error = true;
                        $('#repacking-materials-list tbody tr').each(function () {
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
                        $('#repacking-materials-list tbody tr').each(function () {
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
                     elements: "#QuantityIn",
                     rules: [
                        { type: form.required, message: "Please enter quantity in" },
                        { type: form.positive, message: "Please enter positive value for quantiy in" },
                        { type: form.numeric, message: "Please enter numeric value for quantity in" },
                        { type: form.non_zero, message: "Please enter non zero value for quantity in" },
                     ]
                 },
              {
                  elements: "#QuantityOut",
                  rules: [
                     {
                         type: form.required, message: "Please enter quantity out"
                     },
                     { type: form.positive, message: "Please enter positive value for quantiy out" },
                     { type: form.numeric, message: "Please enter numeric value for quantity out" },
                     { type: form.non_zero, message: "Please enter non zero value for quantity out" },
                  ]
              },
        ],
        on_additional_issue: [
        {
            elements: "#txtAdditionalIssue",
            rules: [

                { type: form.required, message: "Please select one item" },

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
            elements: "#txtQuantity",
            rules: [
                   { type: form.required, message: "Please enter quantity" },
                    { type: form.non_zero, message: "Please enter quantity" },
            ]
        },
        ],
        on_material_return: [
{
    elements: "#txtmaterialsreturn",
    rules: [

        { type: form.required, message: "Please select one item" },

    ]
},
{
    elements: "#txtReturnQuantity",
    rules: [
           { type: form.required, message: "Please enter quantity" },
            { type: form.non_zero, message: "Please enter quantity" },
    ]
},
        ],
    },
}
