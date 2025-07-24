var fh_items;
stockadjustment = {
    init: function () {
        var self = stockadjustment;

        item_list = Item.stock_adjustment_item_list_alopathy();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        if ((clean($("#ItemID").val())==0) && (clean($("#ID").val())==0))
        {
            self.get_filter_item();
        }
        stockadjustment.bind_events();
        self.select_results();

    },



    list: function () {
        var $list = $('#stock-adjustment-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Stock/StockAdjustment/GetStockAdjustmentList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
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
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "Date", "className": "Date" },              
                   { "data": "ItemName", "className": "ItemName" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/StockAdjustment/Details/" + Id);
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
        var self = stockadjustment;
        $.UIkit.autocomplete($('#add-item-autocomplete'), { 'source': self.get_add_items, 'minLength': 1 });
        $('#add-item-autocomplete').on('selectitem.uk.autocomplete', self.set_add_item_details);

        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", "#btnFilter", self.filter_items);
        $("#WarehouseID").on("change", self.clear);
        $(".btnSave, .btnSaveNew, .btnSavedraft").on("click", self.on_save);
        $("body").on("ifChanged", ".check-box", self.check);
        $("#DDLItemCategory").on("change", self.Get_sales_category);
        $("#DDLSalesCategory").on("change", self.clear_item);
        $("#BatchType").on("change", self.get_batches);
        $(".btnRevert").on("click", self.revert);

        $('body').on('click', '#stock-adjustment-items-list tbody tr .btnOpenBatchWithZeroQty', self.open_batch_with_zero_qty);
        $('body').on('click', '#btnOkBatches', self.add_batches);
        $('body').on('click', '#btn_template', self.download_template);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
        $("body").on("keyup change", ".physicalqty", self.calculate_excess_qty);
        $('.select-all').on('ifChanged', self.select_results);
        $("body").on("click", ".btnPrint", self.print_ScheduledStockItems);
        self.Load_All_DropDown();
    },
    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
    },
    Load_All_DropDown: function () {
        $.ajax({
            url: '/Stock/DamageEntry/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    DamageTypeList = response.data;
                }
            }
        });
    },

    save_confirm: function () {
        var self = stockadjustment
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    add_batches: function () {
        var self = stockadjustment;


        var error_count = self.validate_add_batch();
        if (error_count > 0) {
            return;
        }
        UIkit.modal($('#batch-edit')).hide();
        var ItemID = $('#BatchItemID').val();
        var ItemName = $('#BatchItemName').val();
        var Index = $('#Index').val();


        var item = {};
        var items = [];


        item = {
            ItemID: clean($("#BatchItemID").val()),
            BatchTypeID: clean($("#BatchType option:selected").val()),
            BatchType: $("#BatchType option:selected").text(),
            BatchID: clean($("#Batch option:selected").val()),
            Batch: $("#Batch option:selected").text(),
            UnitName: $("#UnitID option:selected").text(),
            ItemName: ItemName,
            UnitID: $("#UnitID option:selected").val(),
            ExpiryDateString: $("#Batch option:selected").data("expirydatestring"),
            PhysicalQty: clean($("#txtRequiredQty").val()),
            CurrentQty: 0.0,
            Rate: $("#UnitID option:selected").data("rate"),
            PrimaryUnitID: $("#BatchPrimaryUnitID").val(),
            PrimaryUnit: $("#BatchPrimaryUnit").val(),
            InventoryUnit: $("#BatchInventoryUnit").val(),
            InventoryUnitID: $("#BatchInventoryUnitID").val(),
            FullRate: $("#FullRate").val(),
            LooseRate: $("#LooseRate").val(),
            DamageType: $("#DamageType option:selected").text(),
            DamageTypeID: clean($("#DamageType option:selected").val()),
            Remark: $("#Remark").val(),
        };
        items.push(item);
        self.add_items_to_grid(items, Index);
        self.clear_batch();
    },
    clear_batch: function () {
        $("#DamageType").val("");
        $("#txtRequiredQty").val(0);
        $("#BatchTypeID").val("");
        $("#BatchType").val("");
        $("#Remark").val("");


    },
    calculate_excess_qty: function () {
        var row = $(this).closest('tr');
        var physicalqty = clean($(row).find(".physicalqty").val());
        var currentqty = clean($(row).find(".currentqty").val());
        var Rate = clean($(row).find(".Rate").text());
        var excessQty = physicalqty - currentqty;
        var excessValue = excessQty * Rate;

        $(row).find(".ExcessQty").val(excessQty);
        $(row).find(".ExcessValue").val(excessValue);

        var DamageType = "Select";
        if (excessQty < 0) {
            DamageType = "Shortage";
        } else if (excessQty > 0) {
            DamageType = "Excess";
        }


        $(row).find('.DamageTypeID option').map(function () {
            if ($(this).text() == DamageType) return this;
        }).attr('selected', 'selected');

    },
    add_items_to_grid: function (items, Index) {
        var self = stockadjustment;
        var row;
        var itemid_count = 0;
        var tr = '';
        var $items_list = $('#stock-adjustment-items-list tbody');
        var index = $items_list.find('tr').length + 1

        $.each(items, function (i, item) {
            $("#stock-adjustment-items-list tbody tr").each(function () {
                if (($(this).find(".BatchID").val() == item.BatchID) && ($(this).find(".BatchTypeID").val() == item.BatchTypeID) && ($(this).find(".UnitID").val() == item.UnitID)) {
                    itemid_count++;
                    row = $(this).closest("tr");
                }
            })
            if (itemid_count > 0) {
                app.confirm("Selected batch already added, Do you want to update physical quantity", function () {
                    var currentphysicalqty = clean($(row).find(".physicalqty").val());
                    var physicalqty = item.PhysicalQty;
                    currentphysicalqty += physicalqty;
                    var excessQty = currentphysicalqty - item.CurrentQty;
                    var excessValue = excessQty * item.Rate;
                    $(row).find(".physicalqty").val(currentphysicalqty);
                    $(row).find(".ExcessQty").val(excessQty);
                    $(row).find(".ExcessValue").val(excessValue);
                })

            }
            else {
                var changeStockButton;
                var WarehouseID = $('#WarehouseID option:selected').val();
                var Warehouse = $('#WarehouseID option:selected').text();
                var DamageType = '<select class="md-input label-fixed DamageTypeID">' + stockadjustment.Build_Select(DamageTypeList, item.DamageType) + '</select>'
                var checkbox;
                var disabled;
                var included;
                if (item.PhysicalQty > 0) {
                    changeStockButton = '<div class="uk-input-group"><button class="btnOpenBatchWithZeroQty" >Change Stock</button></div>';
                    checkbox = '<input type="checkbox" name="items" data-md-icheck checked class="md-input check-box"/>'
                    disabled = "";
                    included = "included";
                }
                else {
                    changeStockButton = "";
                    checkbox = '<input type="checkbox" name="items" data-md-icheck  class="md-input check-box"/>'
                    disabled = "disabled";
                    included = "";
                }

                var excessQty = item.PhysicalQty - item.CurrentQty;
                var excessValue = excessQty * item.Rate;
                tr += ' <tr class= "included add-item-color">'
                            + '<td class="sl-no">' + (i + index) + '</td>'
                            + '<td class="td-check">' + checkbox + '</td>'
                             + '<td class="itemname">' + item.ItemName
                            + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                            + '<input type="hidden" class="UnitID"value="' + item.UnitID + '"/>'
                            + '<input type="hidden" class="BatchID"value="' + item.BatchID + '"/>'
                            + '<input type="hidden" class="BatchTypeID"value="' + item.BatchTypeID + '"/>'
                            + '<input type="hidden" class="WarehouseID"value="' + WarehouseID + '"/>'

                        + '<input type="hidden" class="PrimaryUnit"value="' + item.PrimaryUnit + '"/>'
                        + '<input type="hidden" class="PrimaryUnitID"value="' + item.PrimaryUnitID + '"/>'
                         + '<input type="hidden" class="InventoryUnit"value="' + item.InventoryUnit + '"/>'
                        + '<input type="hidden" class="InventoryUnitID"value="' + item.InventoryUnitID + '"/>'
                          + '<input type="hidden" class="FullRate mask-currency"value="' + item.FullRate + '"/>'
                        + '<input type="hidden" class="LooseRate mask-currency"value="' + item.LooseRate + '"/>'
                            + '</td>'
                            + '<td class="unit">' + item.UnitName + '</td>'
                            + '<td class="batch">' + item.Batch + '</td>'
                            + '<td class="BatchType">' + item.BatchType + '</td>'
                            + '<td class="Rate mask-currency">' + item.Rate + '</td>'
                            + '<td class="Warehouse">' + Warehouse + '</td>'
                             + '<td class="ExpiryDate">' + item.ExpiryDateString + '</td>'
                            + '<td class="currentqty mask-production-qty">' + item.CurrentQty + '</td>'
                            + '<td  >' + '<input type="text" value=" ' + item.PhysicalQty + '" class="md-input uk-text-right physicalqty mask-production-qty" ' + disabled + '  /> ' + '</td>'
                                + '<td class="ExcessQty mask-production-qty">' + excessQty + '</td>'
                        + '<td class="ExcessValue mask-production-qty">' + excessValue + '</td>'
                            + '<td  >' + '<input type="text" class="md-input uk-text Remark "  value=" ' + item.Remark + '" /></td>'
                            + '<td class="">' + DamageType + '</td>'
                              + '<td>' + '<div class="uk-input-group"><button class="btnOpenBatchWithZeroQty" >+</button></div></td>'
                            + '</tr>';
            }
        });

        var $tr = $(tr);
        app.format($tr);
        $items_list.insertAt(Index, $tr);
        self.count_items();


    },
    open_batch_with_zero_qty: function () {

        var self = stockadjustment;
        var row = $(this).closest("tr");
        var ItemID = row.find('.ItemID').val();
        var ItemName = row.find('.itemname').text();
        var batchTypeID = row.find('.BatchTypeID').val();
        var categoryid = $("#DDLItemCategory").val() == null ? 0 : $("#DDLItemCategory").val();
        var salescategoryid = $("#DDLSalesCategory").val() == null ? 0 : $("#DDLSalesCategory").val();
        var Index = $(row).index();


        var UnitID = row.find('.InventoryUnitID').val();
        var Unit = row.find('.InventoryUnit').val();
        var PrimaryUnit = row.find('.PrimaryUnit').val();
        var PrimaryUnitID = row.find('.PrimaryUnitID').val();
        var FullRate = row.find('.FullRate').val();
        var LooseRate = row.find('.LooseRate').val();
        $("#BatchInventoryUnitID").val(UnitID);
        $("#BatchInventoryUnit").val(Unit);


        $("#BatchPrimaryUnitID").val(PrimaryUnitID);
        $("#BatchPrimaryUnit").val(PrimaryUnit);

        $("#BatchItemName").val(ItemName);
        $("#BatchItemID").val(ItemID);



        $("#FullRate").val(FullRate);
        $("#LooseRate").val(LooseRate);

        $("#Index").val(Index);

        $("#BatchType option:selected").val(batchTypeID);
        self.get_batches();
        $("#show-batch-edit").trigger('click');

        $("#DDLItemCategory").val(categoryid);
        $("#DDLSalesCategory").val(salescategoryid);
        self.get_units();


    },

    Get_sales_category: function () {
        var self = stockadjustment;
        var ItemCategoryID = $('#DDLItemCategory').val();
        $.ajax({
            url: '/Masters/Category/GetSalesCategory/',
            dataType: "json",
            type: "POST",
            data: { id: ItemCategoryID },
            success: function (response) {
                $("#DDLSalesCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#DDLSalesCategory").append(html);
                item_list.fnDraw();
            }
        });
        self.clear_item();
    },
    clear: function () {
        if ($('#stock-adjustment-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                $("#ItemID").val('');
                $("#ItemName").val('');
                $('#stock-adjustment-items-list tbody').html('');
            });
        }
        else {
            $("#ItemID").val('');
            $("#ItemName").val('');
        }
    },
    //set auto complete
    set_item_details: function (event, item) {
        if ($('#stock-adjustment-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                $("#stock-adjustment-items-list tbody").html('');

                $("#ItemID").val(item.id);
            });
        }
        else {
            $("#ItemID").val(item.id);
        }

       
    },
    set_add_item_details: function (event, item) {
        var self = stockadjustment;
        $("#BatchItemID").val(item.id);
        $("#BatchItemName").val(item.value);
        $("#BatchPrimaryUnitID").val(item.primaryunitId);
        $("#BatchPrimaryUnit").val(item.primaryunit);
        $("#BatchInventoryUnitID").val(item.inventoryunitId);
        $("#BatchInventoryUnit").val(item.inventoryunit);
        $("#FullRate").val(item.fullRate);
        $("#LooseRate").val(item.looseRate);

        self.get_batches();
        self.get_units();
    },
    get_units: function () {
        var self = stockadjustment;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#BatchInventoryUnitID").val() + "' data-rate='" + $("#FullRate").val() + "'>" + $("#BatchInventoryUnit").val() + "</option>";
        html += "<option value='" + $("#BatchPrimaryUnitID").val() + "' data-rate='" + $("#LooseRate").val() + "'>" + $("#BatchPrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);
    },
    get_batches: function () {
        var ItemID = $("#BatchItemID").val();
        var BatchTypeID = $("#BatchType").val();
        $.ajax({
            url: '/Stock/StockAdjustment/GetBatchesByItemIDForStockAdjustment',
            dataType: "json",
            type: "POST",
            data: {

                ItemID: ItemID,
                WarehouseID: $('#WarehouseID').val(),
                BatchTypeID: BatchTypeID
            },
            success: function (response) {
                $("#Batch").html("");
                var html = "<option value >Select</option>";
                $.each(response.Data, function (i, record) {
                    html += "<option value='" + record.BatchID + "'  data-expiryDateString='" + record.ExpiryDateString + "'  >" + record.Batch + "</option>";
                });
                $("#Batch").append(html);

            }
        });
    },
    //get auto complete
    get_items: function (release) {
        var FromDate = $("#FromDate").val();
        var ToDate = $("#ToDate").val() ;
        $.ajax({
            url: '/Masters/Item/getStockAdjustmentItemForAutoComplete',
            data: {
                
                FromDate: FromDate,
                ToDate: ToDate,
                term: $('#ItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_add_items: function (release) {
        var ItemCategoryID = ($("#DDLItemCategory").val() == "") ? 0 : $("#DDLItemCategory").val();
        var SalesCategoryID = $("#DDLSalesCategory").val() == "" ? 0 : $("#DDLSalesCategory").val();
        $.ajax({
            url: '/Masters/Item/getStockAdjustmentItemForAutoComplete',
            data: {
                term: $('#BatchItemName').val(),
                ItemCategoryID: ItemCategoryID,
                SalesCategoryID: SalesCategoryID,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    select_item: function () {
        var self = stockadjustment;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var SalesCategoryID = $(row).find(".SalesCategoryID").val();

        if ($('#stock-adjustment-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                $("#stock-adjustment-items-list tbody").html('');

                $("#ItemName").val(Name);
                $("#ItemID").val(ID);
                $("#DDLSalesCategory").val(SalesCategoryID)
                UIkit.modal($('#select-item')).hide();
            });
        }
        else {
            $("#ItemName").val(Name);
            $("#ItemID").val(ID);
            $("#DDLSalesCategory").val(SalesCategoryID)
            UIkit.modal($('#select-item')).hide();
        }
       
      
    },

    validate_filter: function () {
        var self = stockadjustment;
        if (self.rules.on_filter.length) {
            return form.validate(self.rules.on_filter);
        }
        return 0;
    },
    validate_add_batch: function () {
        var self = stockadjustment;
        if (self.rules.on_add_batch.length) {
            return form.validate(self.rules.on_add_batch);
        }
        return 0;
    },
    validate_submit: function () {
        var self = stockadjustment;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_revert :function () {
        var self = stockadjustment;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_revert);
        }
        return 0;
    },
    validate_draft: function () {
        var self = stockadjustment;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_filter: [
            {
                elements: "#WarehouseID",
                rules: [
                    { type: form.required, message: "Please choose a store" },
                 { type: form.non_zero, message: "Please choose a store" },

                ],
            },
            //{
            //    elements: "#DDLSalesCategory",
            //    rules: [
            //        { type: form.required, message: "Please choose sales category" },
            //     { type: form.non_zero, message: "Please choose sales category" },

            //    ],
            //},


        ],
        on_add_batch: [
        {
            elements: "#UnitID",
            rules: [
                { type: form.required, message: "Please choose a unit" },
             { type: form.non_zero, message: "Please choose a unit" },

            ],
        },
        {
            elements: "#Batch",
            rules: [
                { type: form.required, message: "Please choose  Batch" },
             { type: form.non_zero, message: "Please choose Batch" },

            ],
        },

        {
            elements: "#DamageType",
            rules: [
                { type: form.required, message: "Please choose  reason" },
             { type: form.non_zero, message: "Please choose reason" },

            ],
        },
        {
            elements: "#txtRequiredQty",
            rules: [

                { type: form.positive, message: "Physical quantity must be positive" },
            ]
        },
        ],
        on_draft: [

         {
             elements: "#item-count",
             rules: [
                 { type: form.non_zero, message: "Please choose atleast one item" },
                 { type: form.required, message: "Please choose atleast one item" },
             ]
         },

        ],
        on_submit: [
            {
                elements: ".included .physicalqty",
                rules: [
                    { type: form.positive, message: "Positive physical qty required" },
                ],
            },
            {
                elements: ".included .DamageTypeID",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var physicalqty = clean($(row).find('.physicalqty').val());
                            var currentqty = clean($(row).find('.currentqty').text());
                            return (physicalqty == currentqty) ? true : form.non_zero(element);

                        }, message: 'Physical quantity and current quantity must be different '
                    },
                ],
            },
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please choose atleast one item" },
                     { type: form.required, message: "Please choose atleast one item" },
                 ]
             },

        ],
        on_revert: [
       
         {
             elements: "#item-count",
             rules: [
                 { type: form.non_zero, message: "Please choose atleast one item" },
                 { type: form.required, message: "Please choose atleast one item" },
             ]
         },

        ],
    },

    filter_items: function () {
        var self = stockadjustment;
        var error_count = self.validate_filter();
        if (error_count > 0) {
            return;
        }
        if ($('#stock-adjustment-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                self.get_filter_item();
            });
        }
        else {
            self.get_filter_item();
        }

        self.count_items();
        //fh_items.resizeHeader();
    },
    get_filter_item: function () {
        var length;
        var self = stockadjustment;
        $.ajax({
            url: '/Stock/StockAdjustment/GetStockAdjustmentItemsForAlopathy/',
            dataType: "json",
            data: {
                FromDate: $("#FromDate").val(),
                ToDate: $("#ToDate").val(),
                ItemID: clean($("#ItemID").val()),
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {

                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="itemname">' + item.ItemName
                        + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                        + '<input type="hidden" class="BatchID"value="' + item.BatchID + '"/>'
                        + '<input type="hidden" class="WarehouseID"value="' + item.WarehouseID + '"/>'
                        + '<input type="hidden" class="CurrentQty"value="' + item.CurrentQty + '"/>'
                         + '<input type="hidden" class="UnitID"value="' + item.UnitID + '"/>'
                         + '<input type="hidden" class="ID" value="' + item.ID + '"/>'
                         + '</td>'
                          + '<td class="unit">' + item.UnitName + '</td>'
                        + '<td class="batch">' + item.Batch + '</td>'
                        + '<td class="Warehouse">' + item.WareHouse + '</td>'
                        + '<td >' + '<input type="text" value=" ' + 0 + '" class="md-input uk-text-right physicalqty mask-production-qty" disabled /> ' + '</td>'
                        + '<td class="Status">' + item.Status + '</td>'
                        + '</tr>';

                });
                $content = $(content);
                app.format($content);
                $("#stock-adjustment-items-list tbody").html($content);
                //if (length == 0) {
                //    if (clean($("#ItemID").val()) == 0) {
                //        app.show_error("Selected store have no stock, please select another item");
                //    }
                //    else {
                //        app.show_error("Selected store have no stock, please select another item");
                //    }
                //}
            },

        });

    },
    Build_Select: function (options, selected_text) {
        var $select = '';
        var $select = $('<select> </select>');
        var $option = '';
        if (typeof selected_text == "undefined") {
            selected_text = "Select";
        }
        $option = '<option value="0">Select</option>';
        //$select.append($option);
        for (var i = 0; i < options.length; i++) {
            $option = '<option ' + ((selected_text == options[i].Name) ? 'selected="selected"' : '') + ' value="' + options[i].ID + '">' + options[i].Name + '</option>';

            $select.append($option);
        }
        return $select.html();

    },
    get_batch_with_zero_stock: function () {
        var self = stockadjustment;
        $.ajax({
            url: '/Stock/StockAdjustment/GetBatchesByItemIDForStockAdjustment',
            dataType: "json",
            data: {
                ItemID: $('#ItemID').val(),
                WarehouseID: $('#WarehouseID').val(),
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.add_items_to_grid(response.Data);
                }
            }
        });
    },
    check: function () {
        var self = stockadjustment;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
            $(row).find(".physicalqty").prop("disabled", false);
        } else {
            $(row).find(".physicalqty").prop("disabled", true);
            $(row).removeClass('included');
        }
        self.count_items();
    },
    count_items: function () {
        var count = $('#stock-adjustment-items-list tbody').find('.check-box:checked').length;
        $('#item-count').val(count);
    },
    get_data: function () {
        var self = stockadjustment;
        var data = {};
        data.TransNo = $("#TransNo").val();
        data.Date = $("#txtDate").val();
        data.ID = $("#ID").val();
        data.Items = [];
        var item = {};
        $("#stock-adjustment-items-list tbody tr.included").each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val()),
            item.WarehouseID = clean($(this).find(".WarehouseID").val()),
            item.UnitID = clean($(this).find(".UnitID").val()),
            item.BatchID = clean($(this).find(".BatchID").val()),
            item.BatchTypeID = clean($(this).find(".BatchTypeID").val()),
            item.CurrentQty = clean($(this).find(".CurrentQty").val()),
            item.PhysicalQty = clean($(this).find(".physicalqty").val()),
            item.ID = clean($(this).find(".ID").val()),
            data.Items.push(item);
        });

        return data;
    },
    get_revert_data: function () {
        var self = stockadjustment;
        var data = {};
       data.Items = [];
        var item = {};
        $("#stock-adjustment-items-list tbody tr.included").each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val()),
            item.WarehouseID = clean($(this).find(".WarehouseID").val()),
            item.BatchID = clean($(this).find(".BatchID").val()),
            item.BatchTypeID = clean($(this).find(".BatchTypeID").val()),
            item.ID = clean($(this).find(".ID").val())
            data.Items.push(item);
        });

        return data;
    },

    on_save: function () {

        var self = stockadjustment;
        var data = self.get_data();
        var location = "/Stock/StockAdjustment/Index";
        var url = '/Stock/StockAdjustment/Save';

        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
            url = '/Stock/StockAdjustment/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_submit();
            if ($(this).hasClass("btnSaveNew")) {
                location = "/Stock/StockAdjustment/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(data, url, location);
            }, function () {
            })
        } else {
            self.save(data, url, location);
        }
    },
    revert: function () {

        var self = stockadjustment;
        var data = self.get_revert_data();
        var location = "/Stock/StockAdjustment/Index";
        var url = '/Stock/StockAdjustment/Revert';      
            self.error_count = self.validate_revert();                  
        if (self.error_count > 0) {
            return;
        }       
            app.confirm_cancel("Do you want to revert?", function () {
                self.revert_data(data, url, location);
            }, function () {
            })
         
    },
    revert_data: function (data, url, location) {
        var self = stockadjustment;
        $(".btnSavedraft, .btnSave, .btnSaveNew,.btnRevert,.btnPrint").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Stock Adjustment Reverted Successfully");
                    window.location = location;
                }
                else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSavedraft, .btnSave, .btnSaveNew,.btnRevert,.btnPrint").css({ 'display': 'block' });
                }
            }
        });


    },
    save: function (data, url, location) {
        var self = stockadjustment;
        $(".btnSavedraft, .btnSave, .btnSaveNew,.btnRevert,.btnPrint").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Stock Adjustment Saved Successfully");
                    window.location = location;
                }
                else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSavedraft, .btnSave, .btnSaveNew,.btnRevert,.btnPrint").css({ 'display': 'block' });
                }
            }
        });


    },
    download_template: function () {
        var self = stockadjustment;

        window.location = "/Reports/Stock/StockAdjustment";
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
            var self = stockadjustment;
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
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
                self.populate_uploaded_stock_adjustment();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    populate_uploaded_stock_adjustment: function () {
        var self = stockadjustment;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Stock/StockAdjustment/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var html = "";
                var $html;
                if (response.Status == "success") {
                    $.each(response.Data, function (i, record) {
                        var slno = (i + 1);
                        var DamageType = '<select class="md-input label-fixed DamageTypeID">' + stockadjustment.Build_Select(DamageTypeList, "Excess") + '</select>'
                        html += '<tr class="included">'
                                + '<td class="uk-text-center">' + slno + '</td>'
                                + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck checked class="md-input check-box"/>' + '</td>'
                                + '<td class="itemname">' + record.ItemName
                                + '<input type="hidden" class="ItemID"value="' + record.ItemID + '"/>'
                                + '<input type="hidden" class="UnitID"value="' + record.UnitID + '"/>'
                                + '<input type="hidden" class="BatchID"value="' + record.BatchID + '"/>'
                                + '<input type="hidden" class="BatchTypeID"value="' + record.BatchTypeID + '"/>'
                                + '<input type="hidden" class="WarehouseID"value="' + record.WarehouseID + '"/>'
                                + '</td>'
                                + '<td class="unit">' + record.UnitName + '</td>'
                                + '<td class="batch">' + record.Batch + '</td>'
                                + '<td class="BatchType">' + record.BatchType + '</td>'
                                + '<td class="BatchType">' + record.Warehouse + '</td>'
                                + '<td class="ExpiryDate">' + record.ExpiryDate + '</td>'
                                + '<td class="currentqty uk-text-right mask-qty">' + record.CurrentQty + '</td>'
                                + '<td>' + '<input type="text" value=" ' + record.PhysicalQty + '" class="md-input uk-text-right physicalqty mask-qty" disabled /> ' + '</td>'
                                + '<td>' + '<input type="text" class="md-input uk-text-right Remark "  /> ' + '</td>'
                                + '<td >' + DamageType + '</td>'

                                + "</tr>";
                    });
                }
                else {
                    app.show_error(response.Message);
                }
                if (html != "") {
                    $html = $(html);
                    app.format($html);
                    $("#stock-adjustment-items-list tbody").html($html);
                }
            }
        });
    },

    select_results: function () {
        var self = stockadjustment;
        if ($(this).prop('checked') == true) {
            $(this).closest('table').find('.check-box').iCheck('check');
        }
        else {
            $(this).closest('table').find('.check-box').iCheck('uncheck');
        }
    },
    print_ScheduledStockItems: function () {
        var self = stockadjustment;
        var startDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();
        $.ajax({
            url:'/Reports/Stock/StockAdjustmentScheduledItemsPrintPdf',
            data: {
                FromDate: startDate,
                ToDate: toDate
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
}