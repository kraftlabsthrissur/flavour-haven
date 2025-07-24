ProductionIssue = {
    init: function () {
        ProductionIssueCRUD.createedit();
        ProductionIssue.freeze_headers();
    },

    list: function () {
        var self = ProductionIssue;
        $('#tabs-productionissue').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = ProductionIssue;
        var $list;

        switch (type) {
            //case "scheduled":
            //    $list = $('#Scheduled-list');
            //    break;
            case "onGoing":
                $list = $('#ongoing-list');
                break;
            case "aborted":
                $list = $('#Aborted-list');
                break;
            case "completed":
                $list = $('#completed-list');
                break;
            default:
                $list = $('#Scheduled-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Manufacturing/ProductionIssue/GetProductionIssueList?type=" + type;

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
                   { "data": "ProductionGroupName", "className": "ProductionGroupName" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "Unit", "className": "Unit" },
                   {
                       "data": "ActualBatchSize", "className": "ActualBatchSize",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.ActualBatchSize + "</div>";
                       }
                   },
                   { "data": "ExpectedDate", "className": "ExpectedDate" },                
                   {
                        "data": "StandardOutput", "className": "StandardOutput",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.StandardOutput + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/ProductionIssue/Details/" + Id);
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
        ProductionIssue.freeze_headers();
        ProductionIssueCRUD.details();
    },

    freeze_headers: function () {
        fh_material = $("#tblMaterialProductionIssue").FreezeHeader({ height: 200 });
        fh_process = $("#tblProcessProductionIssue").FreezeHeader({ height: 200 });

        $('#tabs-material-process[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {

            setTimeout(function () {
                if ($(active_item).attr('id') == "MaterialTab") {
                    fh_material.resizeHeader();
                } else {
                    fh_process.resizeHeader();
                }

            }, 500);
        });
    }
}
var fh_material;
var fh_process;
var sl_production_schedule;

ProductionIssueCRUD = {
    details: function () {
        $currobj = this;
        $currobj.get_sequence_item();
        $("body").on('click', '.btnOpenMaterialQtyMaintenance', $currobj.open_material_qty_maintenance);
        $("body").on('click', '#liNext', $currobj.load_next_production_issue);
        $("body").on('click', '#liPrevious', $currobj.load_prev_production_issue);
        $("#SequenceItemID").on("change", $currobj.change_sequence);
    },
    createedit: function () {
        $currobj = this;

        $currobj.ProductionIssue = { ProductionIssueID: 0, ItemID: 0, UOM: '', ProductionGroupID: 0, ProductionSequence: 0, ItemName: '', BatchSize: 0, ProductionID: 0 };

        $currobj.SelectedAdditionalIssue = { ItemID: 0, Name: '', UnitID: '', Unit: '', Category:'' };
        $currobj.SelectedMaterialReturn = { ItemID: 0, Name: '', QOM: '', Qty: '', UnitID: '', Unit: '', UOM: '', Category:'' };

        $currobj.CurrSelectedMaterialTotalQty = 0;

        $currobj.AddedMaterialQtyItems = [];

        $currobj.Stores;    //to fill Store drop down, when adding Material or Material 

        $currobj.CurrSelectedRow = null;

        this.init = function () {
            //self.rootURL = "/Manufacturing/ProductionIssue/";

            var productionID = $('#hdnProductionID').val();

            if (productionID > 0) {        //Edit page

                $currobj.ProductionIssue.ItemID = $('#hdnItemID').val();
                $currobj.ProductionIssue.UOM = $('#txtUOM').val();
                $currobj.ProductionIssue.ProductionGroupID = $('#hdnProductionGroupID').val();
                $currobj.ProductionIssue.ProductionSequence = $('#hdnProductionSequence').val();
                $currobj.ProductionIssue.ItemName = $('#hdnItemName').val();
                $currobj.ProductionIssue.BatchSize = clean($('#txtStandardBatchSize').val());
                $currobj.ProductionIssue.ProductionID = $('#hdnProductionID').val();
                $currobj.get_sequence_item();
                var stdBatchSize = clean($('#txtStandardBatchSize').val());
                var actualBatchSize = clean($('#txtActualBatchSize').val());

                var noOfTimes = actualBatchSize / stdBatchSize;
                var stdQtyForActualBatch;
                var issueQty = 0;
                $("#tblMaterialProductionIssue").find('tbody tr').each(function () {
                    var IsAdditionalIssue = $(this).find('.IsAdditionalIssue').val();
                    var stdQtyForStdBatch = clean($(this).find('.txtStdQtyStdBatch').val());
                    var IsSubProduct = clean($(this).find('.IsSubProduct').val());
                    if (IsAdditionalIssue == 0) {
                        issueQty = clean($(this).find('.txtIssuedQty').val());
                        stdQtyForActualBatch = stdQtyForStdBatch * noOfTimes;
                        $(this).find('.txtStdQtyActualBatch').val(stdQtyForActualBatch);
                        if (IsSubProduct == 0) {
                            $(this).find('.ActualOutPutForStdBatch').val(stdQtyForActualBatch);
                        }
                        $currobj.calculate_set_variance($(this));
                    }
                });
             
            }

            if ($('#productionIssueItem-autocomplete').length > 0) {    //No need in Detail page
                Item.production_group_item_list();
                $('#production-group-list').SelectTable({
                    selectFunction: $currobj.select_production_group_item,
                    returnFocus: "#ProductionScheduleName",
                    modal: "#select-production-group",
                    initiatingElement: "#txtProductionIssueItem",
                    selectionType: "radio"
                });



                sl_production_schedule = $('#production-schedule-list').SelectTable({
                    selectFunction: $currobj.select_production_schedule,
                    returnFocus: "#SequenceItemID",
                    modal: "#select-production-schedule",
                    initiatingElement: "#ProductionScheduleName",
                    selectionType: "radio"
                });
                $.UIkit.autocomplete($('#productionIssueItem-autocomplete'), { 'source': $currobj.get_item_AutoComplete, 'minLength': 1 });
                $('#productionIssueItem-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_productionissueitem_details);
                $("#btnOKProductionGroup").on("click", $currobj.select_production_group_item);
                $("#btnOKProductionSchedule").on("click", $currobj.select_production_schedule);

            }

            $("body").on('click', '#liNext', $currobj.load_next_production_issue);
            $("body").on('click', '#liPrevious', $currobj.load_prev_production_issue);


            if ($('#additionalIssueItem-autocomplete').length > 0) {//No need in Detail page
                $.UIkit.autocomplete($('#additionalIssueItem-autocomplete'), { 'source': $currobj.get_additionalissueitem_AutoComplete, 'minLength': 1 });
                $('#additionalIssueItem-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_additionalIssueItem_details);
                Item.raw_material_list();
                $('#raw-material-list').SelectTable({
                    selectFunction: $currobj.select_additional_issue_item,
                    returnFocus: "#txtAdditionalIssueQty",
                    modal: "#select-raw-material",
                    initiatingElement: "#txtAddnitionalIssue",
                    startFocusIndex: 3,
                    selectionType: "radio"
                });
                $("#btnOKRawMaterial").on("click", $currobj.select_additional_issue_item);
            }

            if ($('#materialReturnItem-autocomplete').length > 0 && productionID > 0) {

                $.UIkit.autocomplete($('#materialReturnItem-autocomplete'), { 'source': $currobj.get_materialreturnitem_AutoComplete, 'minLength': 1 });
                $('#materialReturnItem-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_materialreturnitem_details);
                Item.material_return_production_issue_list(productionID);
                $('#material-return-list').SelectTable({
                    selectFunction: $currobj.select_material_return_item,
                    returnFocus: "#txtMaterialReturnQty",
                    modal: "#select-material-return-list",
                    initiatingElement: "#txtMaterialReturn",
                    startFocusIndex: 2,
                    selectionType: "radio"
                });
                $("#btnOKReturnMaterial").on("click", $currobj.select_material_return_item);
            }

            $('body').on('click', '#btnAddMaterialProcess', $currobj.load_Material_Process);
            $('body').on('click', '#btnAddAdditionalIssue', $currobj.add_additional_issue);
            $('body').on('click', '#btnAddMaterialReturn', $currobj.add_material_return);
            $('body').on('keyup', '#txtActualBatchSize', $currobj.actual_batch_size_changed);
            $('body').on('keyup', '#tblOutputProductionIssue .txtActualOutput', $currobj.actual_output_changed);
            $("body").on('keyup', '#tblMaterialProductionIssue .txtIssuedQty', $currobj.issue_qtyChanged);

            $("body").on('keydown', '#txtMaterialManageIssueQty', function (event) {
                $currobj.trigger_btn_click_on_enter(event, $('#btnAddMaterialQty'));
            });

            $("body").on('keydown', '#txtAdditionalIssueQty', function (event) {
                $currobj.trigger_btn_click_on_enter(event, $('#btnAddAdditionalIssue'));
            });

            $("body").on('keydown', '#txtMaterialReturnQty', function (event) {
                $currobj.trigger_btn_click_on_enter(event, $('#btnAddMaterialReturn'));
            });

            $("body").on('click', '#btnAddMaterialQty', $currobj.add_material_qty_clicked);
            $("body").on('click', '#btnOkMaterialMaintenance', $currobj.ok_material_maintenance_clicked);
            $("body").on('click', '.btnOpenMaterialQtyMaintenance', $currobj.open_material_qty_maintenance);

            $("body").on('click', '.btnSaveASDraft', $currobj.save_confirm);
            $("body").on('click', '.btnCompleted', $currobj.completed_clicked);
            $("body").on('click', '.abort', $currobj.abort_clicked);
            $("body").on('click', '.btnCancel', $currobj.cancel_clicked);

            $("#SequenceItemID").on("change", $currobj.change_sequence);
            $("body").on('ifChanged', ".fill-all", $currobj.fill_all_issue_qty);
            $("body").on('ifChanged', ".fill", $currobj.fill_issue_qty);
            $("body").on("change", "#tblOutputProductionIssue .OutputStatus", $currobj.set_output_status);



            $currobj.check_output_status();
        }

        this.actual_batch_size_changed = function () {
            $currobj.calculate_actual_batch();
        }

        this.actual_output_changed = function () {
            var actualOutput = clean($(this).val());
            var row = $(this).closest('tr')
            var variance = clean($(row).find('.txtStandardOutput').val()) - parseFloat(actualOutput);
            $(row).find('.txtVariance').val(variance);

        }

        this.issue_qtyChanged = function () {
            var isMaterialReturn = clean($(this).parents('tr').find('.IsMaterialReturn').val());
            var isAdditionalIssue = clean($(this).parents('tr').find('.IsAdditionalIssue').val());
            if (clean($(this).val()) == 0) {
                $(this).parents('tr').find('.IssueDate').val('');
            } else {
                $(this).parents('tr').find('.IssueDate').val($currobj.get_date());
            }

            if (clean($(this).val()) < 0 && isMaterialReturn == 1) {

                $(this).parents('tr').find('.txtStdQtyActualBatch').val(clean($(this).val()));
                $(this).parents('tr').find('.ActualOutPutForStdBatch').val(clean($(this).val()));
            }
            if (clean($(this).val()) > 0 && isAdditionalIssue == 1) {

                $(this).parents('tr').find('.txtStdQtyActualBatch').val(clean($(this).val()));
                $(this).parents('tr').find('.ActualOutPutForStdBatch').val(clean($(this).val()));
            }
            $currobj.calculate_set_variance($(this).parents('tr'));
        }

        this.calculate_set_variance = function (row) {
            var variance = 0;

            var issueQty = clean($(row).find('.txtIssuedQty').val());

            if (issueQty != '' && issueQty > 0) {
                var stdQtyForActualBatch = clean($(row).find('.txtStdQtyActualBatch').val());
                if (stdQtyForActualBatch != '' && stdQtyForActualBatch > 0) {

                    variance = stdQtyForActualBatch - issueQty;
                }
            }

            $(row).find('.txtVariance').val(variance);
        }

        this.calculate_actual_batch = function () {
            var stdBatchSize = clean($('#txtStandardBatchSize').val());
            var actualBatchSize = clean($('#txtActualBatchSize').val());
            var production_sequence;
            var standard_output;
            var standard_output_for_actual_batchsize;

            if (stdBatchSize != 0) {

                var noOfTimes = actualBatchSize / stdBatchSize;
                var stdQtyForActualBatch;
                var issueQty = 0;
                var $tblMaterialObj = $('#tblMaterialProductionIssue');

                $tblMaterialObj.find('tbody tr').each(function () {
                    var IsAdditionalIssue = $(this).find('.IsAdditionalIssue').val();
                    var stdQtyForStdBatch = clean($(this).find('.txtStdQtyStdBatch').val());
                    var IsSubProduct = clean($(this).find('.IsSubProduct').val());
                    if (IsAdditionalIssue == 0) {
                        issueQty = clean($(this).find('.txtIssuedQty').val());
                        stdQtyForActualBatch = stdQtyForStdBatch * noOfTimes;
                        $(this).find('.txtStdQtyActualBatch').val(stdQtyForActualBatch);
                        if (IsSubProduct == 0) {
                            $(this).find('.ActualOutPutForStdBatch').val(stdQtyForActualBatch);
                        }
                        $currobj.calculate_set_variance($(this));
                    }
                });


                $("#tblProcessProductionIssue tbody tr").each(function () {
                    if ($(this).find(".SkilledLaboursStandard").val() != 0) {
                        $(this).find(".txtStandardSkilledLabourHour").val($(this).find(".SkilledLaboursStandard").val() * noOfTimes);
                    }
                    if ($(this).find(".UnSkilledLabourStandard").val() != 0) {
                        $(this).find(".txtStandardUnSkilledLabourHour").val($(this).find(".UnSkilledLabourStandard").val() * noOfTimes);
                    }
                    if ($(this).find(".MachineHoursStandard").val()) {
                        $(this).find(".txtStandardMachineHour").val($(this).find(".MachineHoursStandard").val() * noOfTimes)
                    }
                });

                $("#tblOutputProductionIssue tbody tr").each(function () {
                    production_sequence = $(this).data("production-sequence");
                    standard_output = $("#SequenceItemID").find("option[data-production-sequence=" + production_sequence + "]").data("standard-output");
                    standard_output_for_actual_batchsize = standard_output * noOfTimes;
                    $(this).find(".txtStandardOutput").val(standard_output_for_actual_batchsize);
                    $(this).find(".ActualBatchSize").val(actualBatchSize);
                });
            }
        }

        this.get_item_AutoComplete = function (release) {
            $.ajax({
                url: '/Masters/Item/GetProductionGroupItemsForAutoComplete',
                data: {
                    Hint: $('#txtProductionIssueItem').val()
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    release(data);
                }
            });
        }

        this.get_additionalissueitem_AutoComplete = function (release) {
            $.ajax({
                url: '/Masters/Item/GetRawMaterialForAutoComplete',
                data: {
                    Hint: $('#txtAddnitionalIssue').val(),
                    WarehouseID: $('#WarehouseID').val()
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    release(data);
                }
            });
        }

        this.get_materialreturnitem_AutoComplete = function (callback) {
            var productionID = $currobj.ProductionIssue.ProductionID;
            var productionSequence = $("#SequenceItemID option:selected").data("production-sequence");
            var itemHint = $('#txtMaterialReturn').val();

            //console.log($currobj.ProductionIssue);
            var data = {
                productionID: productionID,
                productionSequence: productionSequence,
                itemHint: itemHint
            }
            //console.log(data);
            $currobj.ajaxRequest("GetMaterialReturnAutoCompleteItemList", data, "GET", callback);
        }

        this.set_productionissueitem_details = function (event, item) {   // on select auto complete item
            $currobj.on_production_group_change($("#ProductionGroupID").val(), item.id);
            $("#txtUOM").val(item.unit);
            $("#ProductionGroupID").val(item.id);
            $("#IsKalkan").val(item.isKalkan);
            $currobj.get_batch_no();
            $currobj.get_production_schedule();
            $currobj.get_sequence_item();
            $("#ProductionScheduleName").focus();
        }

        this.select_additional_issue_item = function () {
            var radio = $('#select-raw-material tbody input[type="radio"]:checked');
            var row = $(radio).closest("tr");
            var ID = radio.val();
            var Name = $(row).find(".Name").text().trim();
            var Unit = $(row).find(".Unit").text().trim();
            var Stock = $(row).find(".Stock").val();
            var UnitID = $(row).find(".UnitID").val();
            var Category = $(row).find(".Category").text().trim();
            var BatchTypeID = $(row).find(".BatchTypeID").val();
            $("#txtAddnitionalIssue").val(Name);
            $("#txtAdditionalIssueUOM").val(Unit);
            $("#UnitID").val(UnitID);
            $("#BatchType").val(BatchTypeID)
            $currobj.SelectedAdditionalIssue = { ItemID: ID, Name: Name, UnitID: UnitID, Unit: Unit, Stock: Stock, Category: Category };
            if(Category=="Finished Goods")
            {
                $("#select_batch_type").removeClass('uk-hidden');
            }
            else
            {
                $("#select_batch_type").addClass('uk-hidden');
                $("#BatchType").val('')
            }
        };

        this.select_material_return_item = function () {
            var radio = $('#select-material-return-list tbody input[type="radio"]:checked');
            var row = $(radio).closest("tr");
            var ID = radio.val();
            var Name = $(row).find(".Name").text().trim();
            var Unit = $(row).find(".Unit").text().trim();
            var Stock = $(row).find(".Stock").val();
            var UnitID = $(row).find(".UnitID").val();
            $("#txtMaterialReturn").val(Name);
            $("#txtMaterialReturnUOM").val(Unit);
            $currobj.SelectedMaterialReturn = { ItemID: ID, Name: Name, Unit: Unit, UnitID: UnitID, Stock: Stock };
            $currobj.get_batch();
        };
        this.get_batch = function () {
            $('#Batch').html('');
            var options = "<option value='0'>Select</option>";
            var callBack = function (response) {              
                if (response.Status == 'success') {
                    $.each(response.data, function (i, record) {
                        options += "<option value='" + record.ID + "' data-IssueQty='" + record.IssueQty + "'>" + record.BatchNo + ' : '+ record.IssueQty + "</option>";
                    });
                }
                $('#Batch').html(options);
            }
            var data = {
                productionID: clean($currobj.ProductionIssue.ProductionID),
                itemID: $currobj.SelectedMaterialReturn.ItemID
            };
            $currobj.ajaxRequest("GetReturnItemBatch", data,
                "GET", callBack);
        }
        this.set_additionalIssueItem_details = function (event, item) {   // on select auto complete item
            $('#txtAdditionalIssueUOM').val(item.unit);
            $("#BatchType").val(item.batchtypeId);
            if (item.category == "Finished Goods") {
                $("#select_batch_type").removeClass('uk-hidden');
            }
            else {
                $("#select_batch_type").addClass('uk-hidden');
                $("#BatchType").val('');
            }
           
            $currobj.SelectedAdditionalIssue = { ItemID: item.id, Name: item.value, UnitID: item.unitId, Unit: item.unit, Stock: item.stock, Category: item.category };
        }

        this.set_materialreturnitem_details = function (event, item) {   // on select auto complete item
            $('#txtMaterialReturnUOM').val(item.uom);
            $currobj.SelectedMaterialReturn = { ItemID: item.id, Name: item.value, QOM: item.qom, Qty: item.qty, UnitID: item.unitId, Unit: item.uom, UOM: item.uom, Stock: item.stock };
            $currobj.get_batch();
        }

        this.load_Material_Process = function () {
            var production_schedule_id, count;
            production_schedule_id = clean($("#ProductionScheduleID").val());
            count = $("#production-schedule-list tbody tr").length;
            var startdate = $("#txtStartDate").val();
            if ($("#hdnProductionID").val() == 0) {
                $("#StartDateStr").val(startdate);
            }

            error_count = $currobj.validate_item()
            if (error_count == 0) {

                if ((production_schedule_id == 0) && (count > 0)) {
                    app.confirm_cancel("Please select production schedule", function () {
                        UIkit.modal($('#select-production-schedule')).show();
                    }, function () {
                        var callBack = function (response) {
                            var MaterialView = $(response.MaterialView);
                            app.format($(MaterialView))
                            var ProcessView = $(response.ProcessView);
                            app.format($(ProcessView))
                            $('#tblMaterialProductionIssue tbody').append(MaterialView);
                            $('#tblProcessProductionIssue tbody').append(ProcessView);
                            fh_material.resizeHeader();
                            fh_process.resizeHeader();
                            $currobj.Stores = response.Stores;
                            $currobj.defaultReceiptStoreID = response.defaultReceiptStoreID;

                            $currobj.add_output();
                            $currobj.actual_batch_size_changed();       //Need to calculate the Std Qty for Standard batch.
                            $("#btnAddMaterialProcess").hide();
                        }
                        var data = {
                            productionGroupID: $("#ProductionGroupID").val(),
                            productionSequence: $("#SequenceItemID option:selected").data("production-sequence"),
                            itemID: $("#SequenceItemID").val(),

                        };
                        console.log($currobj.ProductionIssue);
                        console.log($currobj.ProductionIssue);
                        $currobj.ajaxRequest("GetProductionMaterialProcessView", data,
                            "GET", callBack);

                    });



                } else {
                    var callBack = function (response) {
                        var MaterialView = $(response.MaterialView);
                        app.format($(MaterialView))
                        var ProcessView = $(response.ProcessView);
                        app.format($(ProcessView))
                        $('#tblMaterialProductionIssue tbody').append(MaterialView);
                        $('#tblProcessProductionIssue tbody').append(ProcessView);
                        fh_material.resizeHeader();
                        fh_process.resizeHeader();
                        $currobj.Stores = response.Stores;
                        $currobj.defaultReceiptStoreID = response.defaultReceiptStoreID;

                        $currobj.add_output();
                        $currobj.actual_batch_size_changed();       //Need to calculate the Std Qty for Standard batch.
                        $("#btnAddMaterialProcess").hide();
                    }
                    var data = {
                        productionGroupID: $("#ProductionGroupID").val(),
                        productionSequence: $("#SequenceItemID option:selected").data("production-sequence"),
                        itemID: $("#SequenceItemID").val(),
                        ProductID: $("#hdnProductionID").val()
                    };
                    $currobj.ajaxRequest("GetProductionMaterialProcessView", data,
                      "GET", callBack);

                }
            }

        }

        this.add_material_qty_clicked = function () {

            var itemID = $currobj.CurrSelectedRow.find('.hdnMaterialID').val(); //$('#hdnMaterialMaintainanceItemID').val();
            var itemName = $('#txtMaterialMaintanceItemName').val();
            var uom = $('#txtMaterialMaintainceUOM').val();
            var issueQty = clean($('#txtMaterialManageIssueQty').val());
            var date = $currobj.get_date();

            if (issueQty > 0) {

                $currobj.CurrSelectedMaterialTotalQty += parseInt(issueQty);
                var materialTransDetail = { ItemID: itemID, ItemName: itemName, UOM: uom, IssueQty: issueQty };
                $currobj.AddedMaterialQtyItems.push(materialTransDetail);

                var rowNo = $('#tblMaterialQtyMaintenance tbody tr').length + 1;

                var rowHtml = '<tr class="added">';
                rowHtml += '<td>' + rowNo + '</td>';
                rowHtml += '<td>' + itemName + '</td>';
                rowHtml += '<td>' + uom + '</td>';
                rowHtml += '<td class="mask-production-qty issue-qty">' + issueQty + '</td>';
                rowHtml += '<td></td>';
                rowHtml += '<td>' + date + '</td>';
                rowHtml += '</tr>';
                var $rowHtml = $(rowHtml);
                app.format($rowHtml);
                $('#tblMaterialQtyMaintenance tbody').append($rowHtml);
            }
        }

        this.ok_material_maintenance_clicked = function () {
            var issue_qty = 0;
            var additional_issue_qty = 0;
            var date = $currobj.get_date();
            $('#tblMaterialQtyMaintenance tbody tr').each(function () {
                issue_qty += clean($(this).find('.issue-qty').val());
            });

            $('#tblMaterialQtyMaintenance tbody tr.added').each(function () {
                additional_issue_qty += clean($(this).find('.issue-qty').val());
            });
            $currobj.CurrSelectedRow.find('.txtIssuedQty').val(issue_qty);
            $currobj.CurrSelectedRow.find('.txtIssuedQty').attr('readonly', true);
            $currobj.CurrSelectedRow.find('.AdditionalIssueQty').val(additional_issue_qty);
            $currobj.CurrSelectedRow.find('.txtIssueDate').val(date);
            $currobj.calculate_set_variance($currobj.CurrSelectedRow);
            $currobj.CurrSelectedRow = null;
            $currobj.CurrSelectedMaterialTotalQty = 0;
        }

        this.add_additional_issue = function () {
            var qty = clean($('#txtAdditionalIssueQty').val());
            if ($currobj.SelectedAdditionalIssue.ItemID == 0) {
                app.show_error("Please select an item");
                app.add_error_class($("#txtAddnitionalIssue"));
                return;
            }
            if (qty == 0) {
                app.show_error("Please enter quantity");
                app.add_error_class($("#txtAdditionalIssueQty"));
                return;
            }
            if ($currobj.SelectedAdditionalIssue.Category == "Finished Goods" && $("#BatchType option:selected").val()=='') {
                app.show_error("Please select batch type");
                app.add_error_class($("#BatchType"));
                return;
            }
            var BatchType=$("#BatchType").val()==""?0:$("#BatchType").val();
            var $rowHtml = $currobj.get_material_tab_rowhtml($currobj.SelectedAdditionalIssue, qty, BatchType);
            $('#tblMaterialProductionIssue tbody').append($rowHtml);
            fh_material.resizeHeader();
            $currobj.SelectedAdditionalIssue.ItemID = 0;
            $("#txtAddnitionalIssue").val('');
            $('#txtAdditionalIssueUOM').val('');
            $('#txtAdditionalIssueQty').val(0);
            $("#BatchType").val('');
        }

        this.add_material_return = function () {
            var qty = clean($('#txtMaterialReturnQty').val());
            var issueqty = $("#Batch option:selected").data("issueqty");
            if ($currobj.SelectedMaterialReturn.ItemID == 0) {
                app.show_error("Please select an item");
                app.add_error_class($("#txtMaterialReturn"));
                return;
            }
            if (qty == 0) {
                app.show_error("Please enter quantity");
                app.add_error_class($("#txtMaterialReturnQty"));
                return;
            }
            if (qty > issueqty) {
                app.show_error("Return quantity must be less than issue quantity");
                app.add_error_class($("#txtMaterialReturnQty"));
                return;
            }
            $currobj.SelectedMaterialReturn.BatchID = $("#Batch option:selected").val();
            var $rowHtml = $currobj.get_material_tab_rowhtml($currobj.SelectedMaterialReturn, (-1) * qty,0);
            $('#tblMaterialProductionIssue tbody').append($rowHtml);
            fh_material.resizeHeader();
            $currobj.SelectedMaterialReturn.ItemID = 0;
            $('#txtMaterialReturn').val('');
            $('#txtMaterialReturnUOM').val('');
            $('#txtMaterialReturnQty').val(0);
            $("#Batch").html('');
        }

        this.get_material_tab_rowhtml = function (item, qty, BatchType) {
            var isadditionalisue;
            var ismaterialreturn;
            var IsPositiveIssueQty;
            var masknegative = 'mask-negative-currency';
            if (qty > 0) {
                isadditionalisue = 1;
                ismaterialreturn = 0;
                masknegative = '';
                IsPositiveIssueQty = 1;
            }
            else {
                isadditionalisue = 0;
                ismaterialreturn = 1;
                IsPositiveIssueQty = 0;
            }
            var production_sequence = $("#SequenceItemID option:selected").data("production-sequence");
            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
            if ($(output_row).find(".OutputStatus").val() == "completed") {
                app.show_error("Can not add items to a completed sequence");
                return '';
            }
            var rowNo = $("#tblMaterialProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").length + 1;
            var WarehouseID = $("#WarehouseID").val();
            var rowHtml = '<tr data-production-sequence="' + production_sequence + '">';
            rowHtml += '<td>' + rowNo + '</td>';
            rowHtml += '<td><input type="checkbox" class="fill" data-md-icheck  /></td>';
            rowHtml += '<td>' + item.Name;
            rowHtml += '<input type="hidden" class="hdnMaterialID" value="' + item.ItemID + '" />';
            rowHtml += '<input type="hidden" class="ProductionSequence" value="' + production_sequence + '" />';
            rowHtml += '<input type="hidden" class="stock" value="' + item.Stock + '" />';
            rowHtml += '<input type="hidden" class="StoreID" value="' + WarehouseID + '" />'
            rowHtml += '<input type="hidden" class="IsAdditionalIssue" value="' + isadditionalisue + '" />'
            rowHtml += '<input type="hidden" class="IsMaterialReturn " value="' + ismaterialreturn + '" />'
            rowHtml += '<input type="hidden" class="IsPositiveIssueQty " value="' + IsPositiveIssueQty + '" />'
            rowHtml += '<input type="hidden" class="BatchTypeID " value="' + BatchType + '" />'
            rowHtml += '<input type="hidden" class="UnitID " value="' + item.UnitID + '" />'
            rowHtml += '<input type="hidden" class="BatchID " value="' + item.BatchID + '" />'
            rowHtml += '<input type="hidden" class="ActualOutPutForStdBatch " value="' + qty + '" />'
            rowHtml += '</td>';
            rowHtml += '<td>' + item.Unit + '</td>';
            rowHtml += '<td><input type = "text" class="md-input mask-production-qty txtStdQtyStdBatch " value = "0" readonly="readonly" ></td>';
            rowHtml += '<td><input type = "text" class="md-input mask-production-qty txtStdQtyActualBatch"  value = "' + qty + '" readonly></td>';
            rowHtml += '<td class="mask-production-qty">' + item.Stock + '</td>';
            rowHtml += '<td><input type = "text" class="md-input mask-production-qty  txtIssuedQty " value="' + qty + '"></td>';
            rowHtml += '<td><input type = "text" class="md-input mask-production-qty txtVariance" readonly></td>';
            rowHtml += '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed past-date date txtIssueDate" value="' + $currobj.get_date() + '" name="txtIssueDate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar past-date materialDatePicker"></i></span></div></td>';
            rowHtml += '<td><input type = "text" class="md-input uk-text txtRemarks" ></td>';
            rowHtml += '<td></td>';
            rowHtml += '</tr>';
            var $row = $(rowHtml);
            app.format($row);
            return $row;
        }

        this.trigger_btn_click_on_enter = function (e, obj) {       //Trigger btn on enter click
            var code = e.keyCode || e.which;
            if (code == 13)
                $(obj).trigger('click');
        }

        this.get_date = function () {
            var d = new Date();

            var month = d.getMonth() + 1;
            var day = d.getDate();

            var date = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();
            return date;
        }

        this.setNavStatus();
        //Saving process

        this.save_confirm = function () {
            app.confirm_cancel("Do you want to Save", function () {
                $currobj.save_as_draft_clicked();
            }, function () {
            })
        },

        this.save_as_draft_clicked = function () {
            if ($currobj.validate_form_on_save() == 0) {
                $currobj.save_production_issue(true, false);
            }
        }

        this.completed_clicked = function () {
            if ($currobj.validate_form_on_complete() == 0) {
                $currobj.save_production_issue(false, true);
            }
        }

        this.abort_clicked = function () {
            if ($currobj.validate_form_on_complete() == 0) {
                var obj = $currobj.get_save_obj(0, 0);

                var saveCallback = function (response) {
                    console.log(response);
                    if (response.Status == "Success") {
                        //alert(response.Message);
                        app.show_notice(response.Message);

                        setTimeout(function () {
                            window.location = "/Manufacturing/ProductionIssue/";
                        }, 1000);
                    }
                    else {
                        $(".btnCompleted, .btnSaveASDraft,.abort").css({ 'display': 'block' });
                        app.show_error(response.Message);
                    }
                }

                //console.log(obj);
                $(".btnCompleted, .btnSaveASDraft,.abort").css({ 'display': 'none' });

                $currobj.ajaxRequest('Save', obj, 'POST', saveCallback);
            }
        }

        this.cancel_clicked = function () { }

        this.save_production_issue = function (isDraft, isCompleted) {

            var obj = $currobj.get_save_obj(isDraft, isCompleted);

            var saveCallback = function (response) {

                if (response.Status == "Success") {
                    //alert(response.Message);
                    app.show_notice(response.Message);

                    setTimeout(function () {
                        window.location = "/Manufacturing/ProductionIssue/";
                    }, 1000);
                }
                else {
                    $(".btnCompleted, .btnSaveASDraft,.abort").css({ 'display': 'block' });
                    app.show_error(response.Message);
                }
            }

            //console.log(obj);
            $(".btnCompleted, .btnSaveASDraft,.abort").css({ 'display': 'none' });
            $currobj.ajaxRequest('Save', obj, 'POST', saveCallback);
        }

        $currobj.init();
    },
    set_output_status: function () {
        var row = $(this).closest('tr');
        $(row).removeClass("started").removeClass("completed");
        $(row).addClass($(this).val());
    },
    root_url: function () {
        return "/Manufacturing/ProductionIssue/";
    },
    ajaxRequest: function (url, data, requestType, callBack) {
        var $currobj = ProductionIssueCRUD;
        $.ajax({
            url: $currobj.root_url() + url,
            type: requestType,
            data: data,
            success: function (successResponse) {
                //console.log('successResponse');
                if (callBack != null && callBack != undefined)
                    callBack(successResponse);
            },
            error: function (errResponse) {//Error Occured 
                //console.log(errResponse);
            }
        });
    },
    open_material_qty_maintenance: function () {
        var $currobj = ProductionIssueCRUD;
        var productionIssueMaterialID = $(this).parents('tr').find('.hdnMaterialProductionIssueID').val();
        var materialName = $(this).attr('name');
        var uom = $(this).attr('uom');
        var AdditionalIssueQty = $(this).parents('tr').find('.AdditionalIssueQty').val();
        var itemID = $(this).parents('tr').find('.hdnMaterialID').val();
        $("#hdnMaterialMaintainanceItemID").val(itemID);
        $("#ProductionIssueMaterialID").val(productionIssueMaterialID);
        $("#txtMaterialMaintanceItemName").val(materialName);
        $("#txtMaterialMaintainceUOM").val(uom);
        if ($(this).hasClass("editable")) {
            $("#add-item-wrapper").show();
            $("#btnOkMaterialMaintenance").show();
        } else {
            $("#add-item-wrapper").hide();
            $("#btnOkMaterialMaintenance").hide();
        }

        var data = {
            productionIssueMaterialID: productionIssueMaterialID,
        };
        var callBack = function (response) {
            if (response.Status == "Success") {
                var tr = "";
                var j = 0;
                $.each(response.Data, function (i, record) {
                    j++;
                    tr += '<tr>';
                    tr += '     <td>' + (i + 1) + '</td>';
                    tr += '     <td>' + record.ItemName + '</td>';
                    tr += '     <td>' + record.Unit + '</td>';
                    tr += '     <td class="mask-production-qty issue-qty">' + record.IssueQty + '</td>';
                    tr += '     <td>' + record.BatchNo + '</td>';
                    tr += '     <td>' + record.IssueDateStr + '</td>';
                    tr += '</tr>';
                });
                if (AdditionalIssueQty > 0) {
                    tr += '<tr class="added">';
                    tr += '     <td>' + (j + 1) + '</td>';
                    tr += '     <td>' + materialName + '</td>';
                    tr += '     <td>' + uom + '</td>';
                    tr += '     <td class="mask-production-qty issue-qty">' + AdditionalIssueQty + '</td>';
                    tr += '     <td></td>';
                    tr += '     <td>' + $currobj.get_date() + '</td>';
                    tr += '</tr>';
                }
                var $tr = $(tr);
                app.format($tr);
                $('#tblMaterialQtyMaintenance tbody').html($tr);
                UIkit.modal("#materialQtymaintainance", { center: false }).show();
            }
        }
        $currobj.CurrSelectedRow = $(this).parents('tr');
        $currobj.ajaxRequest("GetMaterialQtyMaintainanceView", data, "GET", callBack);
    },
    load_next_production_issue: function () {
        var self = ProductionIssueCRUD;
        var length = $("#SequenceItemID option").length;
        var selectedIndex = $("#SequenceItemID").prop('selectedIndex');
        if (selectedIndex < length - 1) {
            $("#SequenceItemID").prop('selectedIndex', selectedIndex + 1).trigger("change");
        }

        self.setNavStatus();
    },

    load_prev_production_issue: function () {
        var self = ProductionIssueCRUD;
        var selectedIndex = $("#SequenceItemID").prop('selectedIndex');
        if (selectedIndex >= 1) {
            $("#SequenceItemID").prop('selectedIndex', selectedIndex - 1).trigger("change");
        }

        self.setNavStatus();

    },
    setNavStatus: function () {
        var length = $("#SequenceItemID option").length;
        var selectedIndex = $("#SequenceItemID").prop('selectedIndex');
        if (length == 1) {
            $("#liNext").addClass("disabled").attr("disabled");
            $("#liPrevious").addClass("disabled").attr("disabled")
        } else if (selectedIndex < length - 1 && selectedIndex > 0) {
            $("#liNext").removeClass("disabled").removeAttr("disabled");
            $("#liPrevious").removeClass("disabled").removeAttr("disabled");
        } else if (selectedIndex == 0) {
            $("#liPrevious").addClass("disabled").attr("disabled");
            $("#liNext").removeClass("disabled").removeAttr("disabled");
        } else if (selectedIndex == length - 1) {
            $("#liPrevious").removeClass("disabled").removeAttr("disabled");
            $("#liNext").addClass("disabled").attr("disabled");
        }
    },
    get_save_obj: function (isDraft, isCompleted) {
        var production_issue = {
            TransNo: $('#txtTransNo').val(),
            TransDateStr: '',
            ProductionID: $("#hdnProductionID").val(),
            ProductionIssueID: $currobj.ProductionIssue.ProductionIssueID,     //0- Create, greater than 0 - Edit
            ProductionGroupID: $("#ProductionGroupID").val(),
            ProductionSequence: $currobj.ProductionIssue.ProductionSequence,
            ProductionScheduleID: $("#ProductionScheduleID").val(),
            BatchNo: $("#txtBatchNo").val(),
            StartDateStr: $('#txtStartDate').val(),
            StartTimeStr: $('#txtStartTime').val(),
            ProductionLocationID: $('#ddlProductionLocation').val(),
            IsDraft: isDraft,
            IsCompleted: isCompleted,
            IsKalkan: $("#IsKalkan").val(),
            Output: [],
            Materials: [],
            Processes: [],
        };
        var completed_sequence_count = 0;
        $("#tblOutputProductionIssue tbody tr .OutputStatus").each(function () {
            if ($(this).val() == "completed") {
                completed_sequence_count++;
            }

        });
        if ($("#SequenceItemID").find("option").length == completed_sequence_count) {
            production_issue.IsCompleted = true;
        }
        $('#tblMaterialProductionIssue tbody tr').each(function () {
            var additionalissue = (clean($(this).find('.IsAdditionalIssue').val()) == 0) ? false : true;
            obj = {
                ProductionSequence: $(this).find(".ProductionSequence").val(),
                MaterialProductionIssueID: $(this).find('.hdnMaterialProductionIssueID').val(),
                RawMaterialId: $(this).find('.hdnMaterialID').val(),
                StandardQty: clean($(this).find('.txtStdQtyStdBatch').val()),
                ActualQty: clean($(this).find('.txtStdQtyActualBatch').val()),
                IssueQty: clean($(this).find('.txtIssuedQty').val()),
                AdditionalIssueQty: clean($(this).find('.AdditionalIssueQty').val()),
                Variance: clean($(this).find('.txtVariance').val()),
                BatchNo: $(this).find('.ddlBatch').text(),
                BatchID: $(this).find('.BatchID').val(),
                WareHouseID: $("#WarehouseID").val(),
                IssueDateStr: $(this).find('.txtIssueDate').val(),
                Remarks: $(this).find('.txtRemarks').val(),
                ProductDefinitionTransID: $(this).find('.ProductDefinitionTransID').val(),
                RawMaterialUnitID: $(this).find('.UnitID').val(),
                IsAdditionalIssue: additionalissue,
                BatchTypeID: $(this).find('.BatchTypeID').val()
            };
            production_issue.Materials.push(obj);
        });

        $('#tblProcessProductionIssue tbody tr').each(function () {
            obj = {
                ProcessProductionIssueID: $(this).find('.hdnProcessProductionIssueID').val(),
                Stage: $(this).find('.txtStage').val(),
                ProductionSequence: $(this).find(".ProductionSequence").val(),
                ProcessName: $(this).find('.txtProcessName').val(),
                StartTimeStr: $(this).find('.txtStartTime').val(),
                EndTimeStr: $(this).find('.txtEndTime').val(),
                StartDateStr: $(this).find('.txtStartDate').val(),
                EndDateStr: $(this).find('.txtEndDate').val(),

                SkilledLaboursStandard: clean($(this).find('.txtStandardSkilledLabourHour').val()),
                SkilledLaboursActual: clean($(this).find('.txtActualSkilledLabourHour').val()),
                UnSkilledLabourStandard: clean($(this).find('.txtStandardUnSkilledLabourHour').val()),
                UnSkilledLabourActual: clean($(this).find('.txtActualUnSkilledLabourHour').val()),
                MachineHoursStandard: clean($(this).find('.txtStandardMachineHour').val()),
                MachineHoursActual: clean($(this).find('.txtActualMachineHour').val()),
                Status: $(this).find('.txtStatus').val(),
                DoneBy: $(this).find('.txtDone').val(),
                ProcessDefinitionTransID: $(this).find('.ProcessDefinitionTransID').val(),
            };
            production_issue.Processes.push(obj);
        });

        $('#tblOutputProductionIssue tbody tr').each(function () {
            obj = {
                ProductionID: $currobj.ProductionIssue.ProductionID,
                ProductionSequence: $(this).find(".ProductionSequence").val(),
                ProductionIssueID: $(this).find(".ProductionIssueID").val(),
                ItemID: $(this).find(".ItemID").val(),
                StandardBatchSize: clean($(this).find(".StandardBatchSize").val()),
                ActualBatchSize: clean($(this).find(".ActualBatchSize").val()),
                StandardOutput: clean($(this).find(".txtStandardOutput").val()),
                ActualOutput: clean($(this).find(".txtActualOutput").val()),
                Variance: clean($(this).find(".txtVariance").val()),
                StartDateStr: $(this).find(".txtStartDate").val(),
                StartTimeStr: $(this).find(".txtStartTime").val(),
                EndDateStr: $(this).find(".txtEndDate").val(),
                EndTimeStr: $(this).find(".txtEndTime").val(),
                ProductionLocationID: $('#ddlProductionLocation').val(),
                StoreID: $(this).find(".ReceiptStore").val(),
                ProductionStatus: $(this).find(".OutputStatus").val(),
                IsQCRequired: $(this).find(".ProductionQC").val(),
                IsSubProduct: $(this).find(".SubProduct").val(),
                ProcessStage: $(this).find(".ProcessStage").val()
            };
            production_issue.Output.push(obj);
        });

        return production_issue;
    },
    select_production_group_item: function () {
        var self = ProductionIssueCRUD;
        var radio = $('#select-production-group tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Unit = $(row).find(".Unit").text().trim();
        var IsKalkan = $(row).find(".IsKalkan").val();
        self.on_production_group_change($("#ProductionGroupID").val(), ID);
        $("#ProductionGroupID").val(ID);
        $("#txtProductionIssueItem").val(Name);
        $("#txtUOM").val(Unit);
        $("#IsKalkan").val(IsKalkan);
        UIkit.modal($('#select-production-group')).hide();
        self.get_batch_no();
        self.get_production_schedule();
        self.get_sequence_item();
        self.clear_production_schedule();
    },
    clear_production_schedule:function(){
        $("#ProductionScheduleName").val('');
        $("#ProductionScheduleID").val('');
    },
    get_batch_no: function () {
        $.ajax({
            url: '/Manufacturing/ProductionSchedule/GetProductionBatchNo',
            data: {
                IsKalkan: $('#IsKalkan').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#txtBatchNo").val(response.data);
                }
            }
        });
    },
    get_production_schedule: function () {
        $.ajax({
            url: '/Manufacturing/ProductionIssue/GetProductionSchedules',
            data: {
                ItemID: $('#ProductionGroupID').val()
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                app.format($response);
                $("#production-schedule-list tbody").html($response);
                sl_production_schedule.refresh();
            }
        });
    },
    get_sequence_item: function () {
        var self = ProductionIssueCRUD;
        $.ajax({
            url: '/Manufacturing/ProductionIssue/GetProductionSequences',
            data: {
                ItemID: $('#ProductionGroupID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var options = "";
                var checkboxes = "";
                var $checkboxes = $("<div></div>");
                if (response.Status == "success") {
                    $.each(response.data, function (i, record) {
                        options += "<option data-batch-size = '" + record.BatchSize + "' "
                            + " data-production-sequence = '" + record.ProductionSequence + "' "
                            + " data-production-QC = '" + record.IsQCRequired + "' "
                            + " data-standard-output = '" + record.StandardOutput + "' "
                            + " data-process-stage ='" + record.ProcessStage + "'"
                            + " data-unit ='" + record.Unit + "'"
                            + " value='" + record.ItemID + "'>" + record.Name + "</option>";
                        checkboxes += "<input type='checkbox' data-md-icheck class='fill-all' "
                            + " data-production-sequence = '" + record.ProductionSequence + "' "
                            + " >";
                    });
                    $checkboxes.append(checkboxes);
                    app.format($checkboxes);
                    $(".checkbox-container").eq(0).html($checkboxes);
                    $("#txtStandardBatchSize").val(response.data[0].BatchSize);

                }
                $("#SequenceItemID").html(options);
                self.change_sequence();
            }
        });
    },
    select_production_schedule: function () {
        var self = ProductionIssueCRUD;
        var radio = $('#select-production-schedule tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var BatchNo = $(row).find(".BatchNo").text().trim();
        var BatchID = $(row).find(".BatchID").val();
        var ActualBatchSize = $(row).find('.ActualBatchSize').text().trim();
        $("#ProductionScheduleName").val(Name);
        $("#ProductionScheduleID").val(ID);
        $("#BatchID").val(BatchID);
        $("#txtBatchNo").val(BatchNo);
        $("#txtActualBatchSize").val(ActualBatchSize);
        $("#txtActualBatchSize").prop("readonly", true);

    },
    change_sequence: function () {

        var self = ProductionIssueCRUD;
        var production_sequence = $("#SequenceItemID option:selected").data("production-sequence");

        $("#tblMaterialProductionIssue tbody tr").hide();
        $("#tblProcessProductionIssue tbody tr").hide();
        $("#tblOutputProductionIssue tbody tr").hide();
        $(".checkbox-container").find("div.icheckbox_md").hide();
        $(".checkbox-container").find("input[data-production-sequence=" + production_sequence + "]").closest("div").show();
        if ($("#tblMaterialProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").length) {
            $("#tblMaterialProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").show();
            $("#tblProcessProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").show();
            $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").show();

            $("#btnAddMaterialProcess").hide();

        } else {
            $("#btnAddMaterialProcess").show();
        }
        fh_material.resizeHeader();
        fh_process.resizeHeader();
        self.setNavStatus();
    },
    check_output_status: function () {
        var production_sequence;
        $('#tblOutputProductionIssue tbody tr').each(function () {
            if ($(this).find(".OutputStatus").val() == "completed") {
                production_sequence = $(this).data("production-sequence");
                $("#tblMaterialProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").each(function () {
                    $(this).find("input,select").attr("disabled", "disabled");
                    $(this).find(".btnOpenMaterialQtyMaintenance").removeClass("editable");
                });
                $("#tblProcessProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]").each(function () {
                    $(this).find("input,select").attr("disabled", "disabled");

                });
                $(this).find("input,select").attr("disabled", "disabled");
            }
        });
    },
    on_production_group_change: function (previous_id, current_id) {
        if (previous_id != current_id) {
            $("#tblMaterialProductionIssue tbody").html('');
            $("#tblProcessProductionIssue tbody").html('');
            $("#tblOutputProductionIssue tbody").html('');
            $(".checkbox-container").html('');
            $("#btnAddMaterialProcess").show();
            $("#txtActualBatchSize").removeAttr('readonly');
        }
    },
    add_output: function () {
        var self = ProductionIssueCRUD;
        var serial_no = $("#tblOutputProductionIssue tbody tr").length + 1;
        var item_name = $("#SequenceItemID option:selected").text();
        var item_id = $("#SequenceItemID").val();
        var production_sequence = $("#SequenceItemID option:selected").data("production-sequence");
        var unit = $("#SequenceItemID option:selected").data("unit");
        var standard_output = $("#SequenceItemID option:selected").data("standard-output") * (clean($("#txtActualBatchSize").val()) / clean($("#txtStandardBatchSize").val()));
        var receipt_store = self.generate_ddl_html(self.Stores, true, "ReceiptStore", "", "", self.defaultReceiptStoreID);
        var start_date = $("#txtStartDate").val();
        var start_time = $("#txtStartTime").val();
        var qc = $("#SequenceItemID option:selected").data("production-qc");
        var status = "<select class='md-input OutputStatus' ><option value='started' selected='selected'>Started</option><option value='completed'>Completed</option></select>";
        var process_stage = $("#SequenceItemID option:selected").data("process-stage");
        var tr = '<tr data-production-sequence = "' + production_sequence + '">'
                 + '   <td class="uk-text-center">' + serial_no + '</td>'
                 + '   <td>' + item_name
                 + '        <input type="hidden" class="ItemID" value="' + item_id + '" />'
                 + '        <input type="hidden" class="ProductionSequence" value="' + production_sequence + '" />'
                 + '        <input type="hidden" class="ProductionQC" value="' + qc + '" />'

                 + '        <input type="hidden" class="StandardBatchSize" value="' + $("#txtStandardBatchSize").val() + '" />'
                 + '        <input type="hidden" class="ActualBatchSize" value="' + $("#txtActualBatchSize").val() + '" />'
                 + '        <input type="hidden" class="ProcessStage" value="' + process_stage + '" />'
                 + '   </td>'
                 + '<td>' + unit + '</td>'
                 + '   <td><input type="text" class="md-input mask-production-qty txtStandardOutput" value="' + standard_output + '" readonly /></td>'
                 + '   <td><input type="text" class="md-input mask-production-qty txtActualOutput" value="" /></td>'
                 + '   <td><input type="text" class="md-input mask-production-qty txtVariance" value="" readonly /></td>'
                 + '   <td>' + receipt_store + '</td>'
                 + '   <td>'
                 + '        <input type="text" class="md-input label-fixed past-date date txtStartDate" value="' + start_date + '" readonly="readonly" />'
                 + '   </td>'
                 + '   <td><input type="text" class="md-input time txtStartTime" value="' + start_time + '"  /></td>'
                 + '   <td>'
                 + '       <div class="uk-input-group">'
                 + '           <input type="text" class="md-input label-fixed current-date date txtEndDate" value="" />'
                 + '           <span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar current-date endDate"></i></span>'
                 + '       </div>'
                 + '   </td>'
                 + '   <td><input type="text" class="md-input time txtEndTime" value="" /></td>'
                 + '   <td>' + status + '</td>'
                + '</tr>';
        tr = $(tr);
        app.format(tr);
        $("#tblOutputProductionIssue tbody").append(tr);
    },
    generate_ddl_html: function (options, isIncludeSelectOption, classSelector, idSelector, nameSelector, value) {
        var ddlHtml = '<select class="md-input ' + classSelector + '" id="' + idSelector + '" name="' + nameSelector + '">';

        if (isIncludeSelectOption)
            //ddlHtml += '<option value=""> Select</option>';
            $(options).each(function (icx, obj) {
                ddlHtml += '<option value="' + obj.Value + '" ' + ((typeof value != 'undefined' && value == obj.Value) ? ' selected = "selected"' : '') + '>' + obj.Text + '</option>';
            });
        ddlHtml += '</select>';
        return ddlHtml;
    },
    fill_all_issue_qty: function () {
        var production_sequence = $(this).data("production-sequence");
        var checkboxes = $("#tblMaterialProductionIssue").find("tr[data-production-sequence=" + production_sequence + "]").find("input.fill");
        var self = this;
        $(checkboxes).each(function () {
            if ($(this).is(":checked") != $(self).is(":checked")) {
                $(this).next('.iCheck-helper').trigger('click')
                $(this).trigger('ifChanged');
            }
        });
    },
    fill_issue_qty: function () {
        var row = $(this).closest('tr');
        var $issue_qty = $(row).find('.txtIssuedQty');
        var $standard_qty = $(row).find('.ActualOutPutForStdBatch');
        var $issue_date = $(row).find('.txtIssueDate');
        var $variance = $(row).find('.txtVariance')
        var date = new Date();
        var formatted_date = (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) + "-" + (date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + date.getFullYear();
        if (!$issue_qty.is("[readonly]")) {
            if ($(this).is(":checked")) {
                $issue_qty.val($standard_qty.val());
                $issue_date.val(formatted_date);
                $variance.val(0);
            } else {
                $issue_qty.val('0.0000');
                $issue_date.val('');
                $variance.val(0);
            }

        }
    },
    set_date_time: function () {

        var production_sequence;
        var min_start_time;
        var max_end_time;
        var end_time;
        var start_time;

        $("#tblProcessProductionIssue tbody tr").each(function () {
            $(this).find(".txtStartTime").data("date-time", $(this).find(".txtStartDate").val() + " " + $(this).find(".txtStartTime").val());
            $(this).find(".txtEndTime").data("date-time", $(this).find(".txtEndDate").val() + " " + $(this).find(".txtEndTime").val());
        });

        $("#tblOutputProductionIssue tbody tr").each(function () {
            $(this).find(".txtStartTime").data("date-time", $(this).find(".txtStartDate").val() + " " + $(this).find(".txtStartTime").val());
            $(this).find(".txtEndTime").data("date-time", $(this).find(".txtEndDate").val() + " " + $(this).find(".txtEndTime").val());

            production_sequence = $(this).data("production-sequence");

            min_start_time = $('#tblProcessProductionIssue tbody').find("tr[data-production-sequence=" + production_sequence + "] .txtStartTime").data("date-time");
            max_end_time = $('#tblProcessProductionIssue tbody').find("tr[data-production-sequence=" + production_sequence + "] .txtEndTime").data("date-time");

            $('#tblProcessProductionIssue tbody').find("tr[data-production-sequence=" + production_sequence + "]").each(function () {
                start_time = $(this).find(".txtStartTime").data("date-time");
                end_time = $(this).find(".txtEndTime").data("date-time");
                if (app.compare_date_time(start_time, min_start_time) == "lesser") {
                    min_start_time = start_time;
                }
                if (app.compare_date_time(max_end_time, end_time) == "lesser") {
                    max_end_time = end_time;
                }
            });

            $(this).find(".txtStartTime").data("min-start-time", min_start_time);
            $(this).find(".txtEndTime").data("max-end-time", max_end_time);
        });
    },
    validate_form_on_complete: function () {
        var self = ProductionIssueCRUD;
        self.set_date_time();
        if (self.rules.on_complete.length > 0) {
            return form.validate(self.rules.on_complete);
        }
        return 0;
    },
    validate_additional_output: function () {
        var self = ProductionIssueCRUD;
        if (self.rules.on_additional_output.length) {
            return form.validate(self.rules.on_additional_output);
        }
        return 0;
    },

    validate_form_on_save: function () {
        var self = ProductionIssueCRUD;
        self.set_date_time();
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    validate_item: function () {
        var self = ProductionIssueCRUD;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    ConvertTime: function (time) {
        var hrs = Number(time.match(/^(\d+)/)[1]);
        var mnts = Number(time.match(/:(\d+)/)[1]);
        var format = time.match(/\s(.*)$/)[1];
        if (format == "PM" && hrs < 12) hrs = hrs + 12;
        if (format == "AM" && hrs == 12) hrs = hrs - 12;
        var hours = hrs.toString();
        var minutes = mnts.toString();
        if (hrs < 10) hours = "0" + hours;
        if (mnts < 10) minutes = "0" + minutes;
        time = hours + ":" + minutes;
        return time;
    },
    rules: {
        on_add: [
            {
                elements: "#ProductionGroupID",
                rules: [
                    { type: form.required, message: "Please choose a valid Product Group" },
                    { type: form.non_zero, message: "Please choose a valid Product Group" },
                ]
            },
             {
                 elements: "#SequenceItemID",
                 rules: [
                     { type: form.required, message: "Invalid Item" },
                 ]
             },
            {
                elements: "#txtStandardBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Standard Batch Size" },
                    { type: form.numeric, message: "Invalid Standard Batch Size" },
                    { type: form.positive, message: "Invalid Standard Batch Size" },
                    { type: form.non_zero, message: "Invalid Standard Batch Size" },
                ]
            },
            {
                elements: "#txtActualBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Actual Batch Size" },
                    { type: form.numeric, message: "Invalid Actual Batch Size" },
                    { type: form.positive, message: "Invalid Actual Batch Size" },
                    { type: form.non_zero, message: "Invalid Actual Batch Size" },
                ]
            },
            {
                elements: "#txtStartDate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            }

        ],
        on_save: [
            {
                elements: "#ProductionGroupID",
                rules: [
                    { type: form.required, message: "Please choose a valid Product Group" },
                    { type: form.non_zero, message: "Please choose a valid Product Group" },
                ]
            },
            {
                elements: "#txtStandardBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Standard Batch Size" },
                    { type: form.numeric, message: "Invalid Standard Batch Size" },
                    { type: form.positive, message: "Invalid Standard Batch Size" },
                    { type: form.non_zero, message: "Invalid Standard Batch Size" },
                ]
            },
            {
                elements: "#txtActualBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Actual Batch Size" },
                    { type: form.numeric, message: "Invalid Actual Batch Size" },
                    { type: form.positive, message: "Invalid Actual Batch Size" },
                    { type: form.non_zero, message: "Invalid Actual Batch Size" },
                ]
            },
            {
                elements: "#txtStartDate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },
            {
                elements: ".txtIssuedQty",
                rules: [
                      {
                          type: function (element) {
                              var error = 0;
                              var IsPositive = $(element).closest("tr").find(".IsPositiveIssueQty").val();
                              var IssueQty = clean($(element).val());
                              if ((IsPositive == 1) && (IssueQty < 0)) {
                                  error++;
                              }
                              return error == 0
                          }, message: "Issue quantity other than material return must be positive"
                      },
                     {
                         type: function (element) {
                             var error = 0;
                             var IsPositive = $(element).closest("tr").find(".IsAdditionalIssue").val();
                             var IssueQty = clean($(element).val());
                             if ((IsPositive == 1) && (IssueQty == 0)) {
                                 error++;
                             }
                             return error == 0
                         }, message: "Invalid additional issue quantity"
                     },
                      {
                          type: function (element) {
                              var error = 0;
                              var IsReturn = $(element).closest("tr").find(".IsMaterialReturn").val();
                              var IssueQty = clean($(element).val());
                              if ((IsReturn == 1) && (IssueQty >= 0)) {
                                  error++;
                              }
                              return error == 0
                          }, message: "Invalid return quantity"
                      },
                      {
                          type: function (element) {
                              var ActualOutPutForStdBatch = Math.ceil(clean($(element).closest("tr").find(".ActualOutPutForStdBatch").val()));
                              var issueqty = clean($(element).val());
                              return ActualOutPutForStdBatch >= issueqty;
                          }, message: "Invalid issue quantity"
                      },
                    {
                        type: function (element) {
                            return clean($(element).closest("tr").find(".stock").val()) >= clean($(element).val());
                        }, message: "Insufficient stock"
                    },

                ]
            },
            {
                elements: ".txtActualSkilledLabourHour",
                rules: [
                    { type: form.positive, message: "Invalid actual skilled labour" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_skilled_labour = clean($(element).closest('tr').find('.txtStandardSkilledLabourHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_skilled_labour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual skilled labour"
                    }
                ]
            },
            {
                elements: ".txtActualUnSkilledLabourHour",
                rules: [
                    { type: form.positive, message: "Invalid actual unskilled labour" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_unskilled_labour = clean($(element).closest('tr').find('.txtStandardUnSkilledLabourHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_unskilled_labour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual unskilled labour"
                    }
                ]
            },
            {
                elements: ".txtActualMachineHour",
                rules: [
                    { type: form.positive, message: "Invalid actual machine minutes" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_machine_hour = clean($(element).closest('tr').find('.txtStandardMachineHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_machine_hour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual unskilled labour"
                    }

                ]
            },
            {
                elements: "#tblProcessProductionIssue .txtStartTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtStartDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "Start date required"
                    }
                ]
            },
            {
                elements: "#tblProcessProductionIssue .txtEndTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtEndDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "End date required"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).closest("tr").find(".txtStartTime").data("date-time");
                            var date_time_string2 = $(element).data("date-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "greater" ? false : true;
                        }, message: "End Time must be greater than Start Time"
                    }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtStartTime",
                rules: [
                    { type: form.required, message: "Start time required" },
                    {
                        type: function (element) {
                            return $(element).data("date-time") == " " ? false : true;
                        }, message: "Start date and time required"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).data("date-time");
                            var date_time_string2 = $(element).data("min-start-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "greater" ? false : true;
                        }, message: "Start date succeeds process dates"
                    }
                ],
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtEndTime",
                rules: [
                    { type: form.required, message: "End time required" },
                    {
                        type: function (element) {
                            return $(element).data("date-time") == " " ? false : true;
                        }, message: "End date and time required"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).data("date-time");
                            var date_time_string2 = $(element).data("max-end-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "lesser" ? false : true;
                        }, message: "End date preceeds process dates"
                    }
                ],
            },
            {
                elements: "#tblOutputProductionIssue .txtStartTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtStartDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "Start date required"
                    }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .txtEndTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtEndDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "End date required", alt_element: "#tblOutputProductionIssue .txtEndTime"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).closest("tr").find(".txtStartTime").data("date-time");
                            var date_time_string2 = $(element).data("date-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "greater" ? false : true;
                        }, message: "End Time must be greater than Start Time"
                    }
                ]
            },
            {
                elements: ".OutputStatus",
                rules: [
                    {
                        type: function (element) {
                            if ($(element).val() == "started") {
                                return true;
                            }
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var flag = false;
                            $("#tblMaterialProductionIssue").find("tr[data-production-sequence=" + production_sequence + "] .txtIssuedQty").each(function () {
                                if (clean($(this).val()) != 0) {
                                    flag = true;
                                }
                            });
                            return flag;
                        }, message: "Please issue ateast one item before complete"
                    },
                ]
            },
            {
                elements: ".txtActualOutput",
                rules: [
                   { type: form.positive, message: "Invalid Actual output" },
                   {
                       type: function (element) {
                           return $(element).closest("tr").find(".OutputStatus").val() == "started" ? true : form.required(element);
                       }, message: "Actual output quantity required"
                   },
                   {
                       type: function (element) {
                           return $(element).closest("tr").find(".OutputStatus").val() == "started" ? true : form.non_zero(element);
                       }, message: "Actual output quantity required"
                   }
                ]
            },
            {
                elements: "#tblMaterialProductionIssue .txtIssueDate",
                rules: [
                    { type: form.past_date, message: "Invalid issue date" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var result;
                            var error = 0;
                            $("#tblMaterialProductionIssue").find("tr[data-production-sequence=" + production_sequence + "] .txtIssueDate").each(function () {
                                var date_time_string1 = $("#StartDateStr").val();
                                var date_time_string2 = $(this).closest('tr').find('.txtIssueDate').val();
                                var date = date_time_string1.split('-');
                                var startdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                                var date = date_time_string2.split('-');
                                var issuedate = new Date(date[2], date[1] - 1, date[0]).getTime();
                                if (issuedate != NaN) {
                                    if ((startdate > issuedate))
                                        error++;
                                }
                            });

                            return error == 0
                        }, message: "Invalid issue date"
                    }
                ]
            },
            {
                elements: "#SequenceItemID",
                rules: [
                    {
                        type: function (element) {
                            return $("#tblOutputProductionIssue tbody tr").length>0;
                        }, message: "Please add materail"
                    },
                ]
            },
        ],
        on_complete: [
            {
                elements: "#ProductionGroupID",
                rules: [
                    { type: form.required, message: "Please choose a valid Product Group" },
                    { type: form.non_zero, message: "Please choose a valid Product Group" },
                ]
            },
            {
                elements: "#txtStandardBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Standard Batch Size" },
                    { type: form.numeric, message: "Invalid Standard Batch Size" },
                    { type: form.positive, message: "Invalid Standard Batch Size" },
                    { type: form.non_zero, message: "Invalid Standard Batch Size" },
                ]
            },
            {
                elements: "#txtActualBatchSize",
                rules: [
                    { type: form.required, message: "Invalid Actual Batch Size" },
                    { type: form.numeric, message: "Invalid Actual Batch Size" },
                    { type: form.positive, message: "Invalid Actual Batch Size" },
                    { type: form.non_zero, message: "Invalid Actual Batch Size" },
                ]
            },
            {
                elements: "#txtStartDate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },
             {
                 elements: "#tblMaterialProductionIssue .txtIssueDate",
                 rules: [
                     { type: form.past_date, message: "Invalid issue date" },
                     {
                         type: function (element) {
                             var production_sequence = $(element).closest("tr").data("production-sequence");
                             var result;
                             var error = 0;
                             $("#tblMaterialProductionIssue").find("tr[data-production-sequence=" + production_sequence + "] .txtIssueDate").each(function () {
                                 var date_time_string1 = $("#StartDateStr").val();
                                 var date_time_string2 = $(this).closest('tr').find('.txtIssueDate').val();
                                 var date = date_time_string1.split('-');
                                 var startdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                                 var date = date_time_string2.split('-');
                                 var issuedate = new Date(date[2], date[1] - 1, date[0]).getTime();
                                 if (issuedate != NaN) {
                                     if ((startdate > issuedate))
                                         error++;
                                 }
                             });

                             return error == 0
                         }, message: "Invalid issue date"
                     }
                 ]
             },
            {
                elements: ".txtIssuedQty",
                rules: [

                    {
                        type: function (element) {
                            var error = 0;
                            var IsPositive = $(element).closest("tr").find(".IsPositiveIssueQty").val();
                            var IssueQty = clean($(element).val());
                            if ((IsPositive == 1) && (IssueQty < 0)) {
                                error++;
                            }
                            return error == 0
                        }, message: "Issue quantity other than material return must be positive"
                    },
                    {
                        type: function (element) {
                            var error = 0;
                            var IsPositive = $(element).closest("tr").find(".IsAdditionalIssue").val();
                            var IssueQty = clean($(element).val());
                            if ((IsPositive == 1) && (IssueQty == 0)) {
                                error++;
                            }
                            return error == 0
                        }, message: "Invalid additional issue quantity"
                    },
                    {
                        type: function (element) {
                            var error = 0;
                            var IsReturn = $(element).closest("tr").find(".IsMaterialReturn").val();
                            var IssueQty = clean($(element).val());
                            if ((IsReturn == 1) && (IssueQty >= 0)) {
                                error++;
                            }
                            return error == 0
                        }, message: "Invalid return quantity"
                    },
                    {
                        type: function (element) {
                            return clean($(element).closest("tr").find(".ActualOutPutForStdBatch").val()) >= clean($(element).val());
                        }, message: "Invalid issue quantity"
                    },
                    {
                        type: function (element) {
                            return clean($(element).closest("tr").find(".stock").val()) >= clean($(element).val());
                        }, message: "Insufficient stock"
                    },
                ]
            },
            {
                elements: ".txtActualSkilledLabourHour",
                rules: [
                    { type: form.positive, message: "Invalid actual skilled labour" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_skilled_labour = clean($(element).closest('tr').find('.txtStandardSkilledLabourHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_skilled_labour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual skilled labour"
                    }
                ]
            },
            {
                elements: ".txtActualUnSkilledLabourHour",
                rules: [
                    { type: form.positive, message: "Invalid actual unskilled labour" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_unskilled_labour = clean($(element).closest('tr').find('.txtStandardUnSkilledLabourHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_unskilled_labour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual unskilled labour"
                    }
                ]
            },
            {
                elements: ".txtActualMachineHour",
                rules: [
                    { type: form.positive, message: "Invalid actual machine minutes" },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var output_row = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + production_sequence + "]");
                            var standard_machine_hour = clean($(element).closest('tr').find('.txtStandardMachineHour').val());
                            if ($(output_row).find(".OutputStatus").val() == "completed" && standard_machine_hour != 0) {
                                return form.non_zero(element);
                            }
                            return true;

                        }, message: "Invalid actual unskilled labour"
                    }

                ]
            },
            {
                elements: ".txtActualOutput",
                rules: [
                    { type: form.numeric, message: "Invalid Actual Output" },
                    { type: form.required, message: "Invalid Actual Output" },
                    { type: form.positive, message: "Invalid Actual Output" },
                    { type: form.non_zero, message: "Invalid Actual Output" },
                ]
            },
            {
                elements: ".OutputStatus",
                rules: [
                    {
                        type: function (element) {
                            return $(element).val() == "completed";
                        }, message: "Output status should be completed"
                    },
                    {
                        type: function (element) {
                            var production_sequence = $(element).closest("tr").data("production-sequence");
                            var flag = false;
                            $("#tblMaterialProductionIssue").find("tr[data-production-sequence=" + production_sequence + "] .txtIssuedQty").each(function () {
                                if (clean($(this).val()) != 0) {
                                    flag = true;
                                }
                            });
                            return flag;
                        }, message: "Please issue ateast one item before complete"
                    },
                ]
            },
            {
                elements: "#SequenceItemID",
                rules: [
                    {
                        type: function (element) {
                            return $(element).find("option").length == $("#tblOutputProductionIssue tbody tr").length;
                        }, message: "Incomplete production sequence"
                    },
                ]
            },
            {
                elements: "#tblProcessProductionIssue .txtStartTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtStartDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "Start date required"
                    }
                ]
            },
            {
                elements: "#tblProcessProductionIssue .txtEndTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtEndDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "End date required"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).closest("tr").find(".txtStartTime").data("date-time");
                            var date_time_string2 = $(element).data("date-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "greater" ? false : true;
                        }, message: "End Time must be greater than Start Time"
                    }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtStartDate",
                rules: [
                    { type: form.required, message: "Start date required" }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtEndDate",
                rules: [
                    { type: form.required, message: "End date required" }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtStartTime",
                rules: [
                     { type: form.required, message: "Start time required" },
                     {
                         type: function (element) {
                             return $(element).data("date-time") == " " ? false : true;
                         }, message: "Start date and time required"
                     }
                ],
            },
            {
                elements: "#tblOutputProductionIssue .completed .txtEndTime",
                rules: [
                    {
                        type: form.required, message: "End time required"
                    },
                    {
                        type: function (element) {
                            return $(element).data("date-time") == " " ? false : true;
                        }, message: "End date and time required"
                    },
                ],
            },
            {
                elements: "#tblOutputProductionIssue .txtStartTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtStartDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "Start date required"
                    }
                ]
            },
            {
                elements: "#tblOutputProductionIssue .txtEndTime",
                rules: [
                    {
                        type: function (element) {
                            return $(element).closest("tr").find(".txtEndDate").val() == "" && $(element).val() != "" ? false : true;
                        }, message: "End date required"
                    },
                    {
                        type: function (element) {
                            var date_time_string1 = $(element).closest("tr").find(".txtStartTime").data("date-time");
                            var date_time_string2 = $(element).data("date-time");
                            var result = app.compare_date_time(date_time_string1, date_time_string2);
                            return result == "greater" ? false : true;
                        }, message: "End Time must be greater than Start Time"
                    }
                ]
            },
        ],
        on_additional_output: [
            {
                elements: "#AdditionalOutputItemID",
                rules: [
                   { type: form.required, message: "Please select item" },
                ]
            },
             {
                 elements: "#AdditionalActualOutput",
                 rules: [
               { type: form.required, message: "Please enter actual output" },

                 ]
             },
              {
                  elements: "#AdditionalOutputStore",
                  rules: [
                { type: form.required, message: "Please select store" },

                  ]
              },
            {
                elements: "#AdditionalStartTime",
                rules: [
                     { type: form.required, message: "Please enter start time" },
                ]
            },
            {
                elements: "#additionalEndDate",
                rules: [
                     { type: form.required, message: "Please enter end date" },
                     { type: form.current_date, message: "invalid end date" },
                ]
            },
            {
                elements: "#AdditioanlEndTime",
                rules: [
                     { type: form.required, message: "Please enter end time" },
                     {
                         type: function (element) {
                             var end_time;
                             var start_time;
                             start_time = $("#AdditionalStartDate").val() + " " + $("#AdditionalStartTime").val();
                             end_time = $("#additionalEndDate").val() + " " + $("#AdditioanlEndTime").val();

                             var date_time_string1 = start_time;
                             var date_time_string2 = end_time;
                             var result = app.compare_date_time(date_time_string1, date_time_string2);
                             return result == "greater" ? false : true;
                         }, message: "End Time must be greater than Start Time"
                     }
                ]
            },

        ],
    }
}