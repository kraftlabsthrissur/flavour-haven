stockconsumption = {
    init: function () {
        var self = stockconsumption;

        item_list = Item.stock_adjustment_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });

        stockconsumption.bind_events();
        self.select_results();

    },

    select_item: function () {
        var self = stockconsumption;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var SalesCategoryID = $(row).find(".SalesCategoryID").val();

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#DDLSalesCategory").val(SalesCategoryID)
        UIkit.modal($('#select-item')).hide();
    },

    Get_sales_category: function () {
        var self = stockconsumption;
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
    set_item_details: function (event, item) {
        $("#ItemID").val(item.id);
        $("#DDLSalesCategory").val(item.salescategoryid);

    },

    list: function () {
        var self = stockconsumption;
        $('#tabs-StockConsumption').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "stock-consumption":
                $list = $('#stock-consumption-list');
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

            var url = "/Stock/StockConsumption/GetStockConsumptionList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
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
                   { "data": "Date", "className": "Date" },
                   { "data": "Warehouse", "className": "Warehouse" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "SalesCategory", "className": "SalesCategory" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/StockConsumption/Details/" + Id);
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
        var self = stockconsumption;
        $("#DDLItemCategory").on("change", self.Get_sales_category);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $("body").on("click", "#btnFilter", self.filter_items);
        $("body").on("ifChanged", ".check-box", self.check);
        $('.select-all').on('ifChanged', self.select_results);
        $(".btnSave, .btnSaveNew, .btnSavedraft").on("click", self.on_save);
        self.Load_All_DropDown();
    },

    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
    },

    get_items: function (release) {
        var ItemCategoryID = ($("#DDLItemCategory").val() == "") ? 0 : $("#DDLItemCategory").val();
        var SalesCategoryID = $("#DDLSalesCategory").val() == "" ? 0 : $("#DDLSalesCategory").val();
        $.ajax({
            url: '/Masters/Item/getStockAdjustmentItemForAutoComplete',
            data: {
                term: $('#ItemName').val(),
                ItemCategoryID: ItemCategoryID,
                SalesCategoryID: SalesCategoryID
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    filter_items: function () {
        var self = stockconsumption;
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

    count_items: function () {
        var count = $('#stock-adjustment-items-list tbody').find('.check-box:checked').length;
        $('#item-count').val(count);
    },

    validate_filter: function () {
        var self = stockconsumption;
        if (self.rules.on_filter.length) {
            return form.validate(self.rules.on_filter);
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
            {
                elements: "#DDLSalesCategory",
                rules: [
                    { type: form.required, message: "Please choose sales category" },
                 { type: form.non_zero, message: "Please choose sales category" },

                ],
            },
            {
                elements: "#ItemName",
                rules: [
                    { type: form.required, message: "Please choose an item" },
                 { type: form.non_zero, message: "Please choose an item" },

                ],
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

         {
             elements: ".included .physicalqty",
             rules: [
                 {
                     type: function (element) {
                         var row = $(element).closest('tr');
                         var physicalqty = clean($(row).find('.physicalqty').val());
                         var currentqty = clean($(row).find('.currentqty').val());
                         if (currentqty >= physicalqty)
                             return true;

                     }, message: 'ConsumptionQty  must be lesser than AvailableQty'
                 },
             ],
         },

        ],
        on_submit: [
            {
                elements: ".included .physicalqty",
                rules: [
                    { type: form.positive, message: "Positive ConsumptionQty required" },
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
                elements: ".included .physicalqty",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var physicalqty = clean($(row).find('.physicalqty').val());
                            var currentqty = clean($(row).find('.currentqty').val());
                            if (currentqty >= physicalqty)
                                return true;

                        }, message: 'ConsumptionQty  must be lesser than AvailableQty'
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
    },

    Build_Select: function (options, selected_text) {
        var $select = '';
        var $select = $('<select> </select>');
        var $option = '';
        if (typeof selected_text == "undefined") {
            selected_text = "Select";
        }
        $option = '<option value="0">Select</option>';
        for (var i = 0; i < options.length; i++) {
            $option = '<option ' + ((selected_text == options[i].Name) ? 'selected="selected"' : '') + ' value="' + options[i].ID + '">' + options[i].Name + '</option>';

            $select.append($option);
        }
        return $select.html();

    },

    get_filter_item: function () {
        var length;
        var self = stockconsumption;
        $.ajax({
            url: '/Stock/StockConsumption/GetStockConsumptionItems/',
            dataType: "json",
            data: {
                'ItemCategoryID': $("#DDLItemCategory").val(),
                'ItemID': $("#ItemID").val(),
                'WarehouseID': $("#WarehouseID").val(),
                'SalesCategoryID': $("#DDLSalesCategory").val()
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {

                    var slno = (i + 1);
                    var Warehouse = $("#WarehouseID option:selected").text()
                    var DamageType = '<select class="md-input label-fixed DamageTypeID">' + stockconsumption.Build_Select(DamageTypeList) + '</select>'
                    var excessQty = item.PhysicalQty - item.AvailableQty;
                    var excessValue = excessQty * item.Rate;
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="itemname">' + item.ItemName
                        + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                        + '<input type="hidden" class="UnitID"value="' + item.InventoryUnitID + '"/>'
                        + '<input type="hidden" class="BatchID"value="' + item.BatchID + '"/>'
                        + '<input type="hidden" class="BatchTypeID"value="' + item.BatchTypeID + '"/>'
                        + '<input type="hidden" class="WarehouseID"value="' + item.WarehouseID + '"/>'
                        + '<input type="hidden" class="PrimaryUnit"value="' + item.UnitName + '"/>'
                        + '<input type="hidden" class="PrimaryUnitID"value="' + item.UnitID + '"/>'
                         + '<input type="hidden" class="InventoryUnit"value="' + item.InventoryUnit + '"/>'
                        + '<input type="hidden" class="InventoryUnitID"value="' + item.InventoryUnitID + '"/>'
                        + '<input type="hidden" class="FullRate mask-currency"value="' + item.Rate + '"/>'
                        + '<input type="hidden" class="LooseRate mask-currency"value="' + item.LooseRate + '"/>'
                        + '</td>'
                        + '<td class="unit">' + item.InventoryUnit + '</td>'
                        + '<td class="batch">' + item.Batch + '</td>'
                        + '<td class="BatchType">' + item.BatchType + '</td>'
                        + '<td class="Rate mask-currency">' + item.Rate + '</td>'
                        + '<td class="BatchType">' + Warehouse + '</td>'
                        + '<td class="ExpiryDate">' + item.ExpiryDateString + '</td>'
                        + '<td class="mask-production-qty currentqty ">' + item.AvailableQty + '</td>'
                        + '<td  >' + '<input type="text" value=" ' + item.PhysicalQty + '" class="md-input uk-text-right physicalqty mask-production-qty" disabled /> ' + '</td>'
                        + '</tr>';

                });
                $content = $(content);
                app.format($content);
                $("#stock-adjustment-items-list tbody").html($content);
                if (length == 0) {
                    if (clean($("#ItemID").val()) == 0) {
                        app.show_error("Selected store have no stock, please select another item");
                    }
                    else {
                        app.show_error("Selected store have no stock, please select another item");
                    }
                }
            },

        });

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

    get_data: function () {
        var self = stockconsumption;
        var data = {};
        data.TransNo = $("#TransNo").val();
        data.Date = $("#txtDate").val();
        data.WarehouseID = clean($("#WarehouseID").val())
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
            item.CurrentQty = clean($(this).find(".currentqty").val()),
            item.PhysicalQty = clean($(this).find(".physicalqty").val()),
            item.ExpiryDate = $(this).find(".ExpiryDate").text(),
            item.Remark = $(this).find(".Remark").val(),
            item.DamageTypeID = $(this).find(".DamageTypeID").val(),
            item.Rate = $(this).find(".Rate").text(),
            item.ExcessQty = clean($(this).find(".ExcessQty").text()),
            item.ExcessValue = clean($(this).find(".ExcessValue").text()),
            data.Items.push(item);
        });

        return data;
    },

    check: function () {
        var self = stockconsumption;
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

    select_results: function () {
        var self = stockconsumption;
        if ($(this).prop('checked') == true) {
            $(this).closest('table').find('.check-box').iCheck('check');
        }
        else {
            $(this).closest('table').find('.check-box').iCheck('uncheck');
        }
    },

    on_save: function () {

        var self = stockconsumption;
        var data = self.get_data();
        var location = "/Stock/StockConsumption/Index";
        var url = '/Stock/StockConsumption/Save';

        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
            url = '/Stock/StockConsumption/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_submit();
            if ($(this).hasClass("btnSaveNew")) {
                location = "/Stock/StockConsumption/Create";
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

    save: function (data, url, location) {
        var self = stockconsumption;
        $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Stock Consumption Saved Successfully");
                    window.location = location;
                }
                else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'block' });
                }
            }
        });
    },

    validate_draft: function () {
        var self = stockconsumption;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_submit: function () {
        var self = stockconsumption;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
}