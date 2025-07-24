openingstock = {
    init: function () {
        var self = openingstock;

        item_list = Item.stockable_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRequiredQty",
            modal: "#select-item",
            initiatingElement: "#ItemName"
        });

        self.bind_events();
    },

    details: function () {
        var self = openingstock;
    },

    list: function () {
        var self = openingstock;
        $('#tabs-OpeningStock').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "opening-stock":
                $list = $('#stock-list');
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

            var url = "/Stock/OpeningStock/GetOpeningStockListForDataTable?type=" + type;
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
                   { "data": "Date", "className": "Date" },
                   { "data": "Store", "className": "Store" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/OpeningStock/Details/" + Id);

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
        var self = openingstock;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $.UIkit.autocomplete($('#batch-autocomplete'), { 'source': self.get_batches, 'minLength': 1 });
        $('#batch-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_details);
        //$("body").on("blur", "#Batch", self.get_MRP);
        $("body").on("click", "#btnAddItem", self.add_item);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", ".remove-item", self.delete_item);
        $(".btnSave, .btnSavedraft, .btnSaveNew").on("click", self.on_save);
        $("body").on("change", "#add-batch #UnitID", self.unit_change);
        //$('body').on('click', '#btnCreateBatch', GRN.create_batch);
    },

    get_batches: function (release) {
        $.ajax({
            url: '/Masters/Batch/GetBatchesForAutoComplete',
            data: {
                Hint: $('#Batch').val(),
                ItemID: $("#ItemID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },

    set_batch_details: function (event, items) {
        var self = openingstock;
        var ItemID = $("#ItemID").val();
        var ItemName = $("#ItemName").val();
        if (items.value == "Create new Batch") {
            $('#show-add-batch').trigger('click');
            $("#add-batch #ItemName").val(ItemName);
            $("#add-batch #ItemID").val(ItemID);
            //$("#add-batch #POTransID").val(POTransID);
            //$("#add-batch #Unit").val(unit);
            //$("#add-batch #UnitID").val(unitID);
            //$("#add-batch #PackSize").val(packsize);
            //$("#add-batch #manufacture-date").val(date);
            //$("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
           // self.get_latest_batch_details(itemid, packsize);
        }
        //OLD CODE In OpeningStock
        //$("#Value").val('');
        //$("#Value").removeAttr("disabled").addClass("enabled");
        //$("#Batch").val(items.value);
        //$("#BatchID").val(items.id);
        //self.get_MRP();
    },


    unit_change: function () {
        var self = openingstock;
        var ItemID = $("#ItemID").val();
        var packsize = clean($("#add-batch #UnitID option:selected").data("packsize"));
        var unit = $("#add-batch #UnitID option:selected").data("unit");
        $("#add-batch #PackSize").val(packsize);
        $("#add-batch #Unit").val(unit);
        $("#add-batch #unitSelected").text('packsize you have selected ' + packsize)
        self.get_latest_batch_details(ItemID, packsize);
        self.calculate_profit();
    },

    get_latest_batch_details: function (itemid, packsize) {
        $.ajax({
            url: '/Masters/Batch/GetLatestBatchDetails',
            data: {
                ItemID: itemid,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    $("#LatestRetailMRP").val(data.data.RetailMRP);
                    $("#LatestRetailLooseRate").val(data.data.RetailLooseRate);
                    $("#LatestPurchaseMRP").val(data.data.BatchRate);
                    $("#LatestPurchaseLooseRate").val(data.data.PurchaseLooseRate);
                    $("#LatestProfitPrice").val(data.data.ProfitPrice);
                    $("#RetailMRP").val(data.data.RetailMRP);
                    $("#RetailLooseRate").val(data.data.RetailMRP / packsize);


                }
            },
        });
    },

    calculate_profit: function () {
        var retailMRP = clean($("#RetailMRP").val());
        var purchaseMRP = clean($("#PurchaseMRP").val());
        var packSize = clean($("#add-batch #PackSize").val());
        var retailLoose = retailMRP / packSize;
        var purchaseLoose = purchaseMRP / packSize;
        var profit = ((retailMRP - purchaseMRP) / (retailMRP)) * 100;
        $("#RetailLooseRate").val(retailLoose);
        $("#PurchaseLooseRate").val(purchaseLoose);
        $("#ProfitPrice").val(profit);

    },

    create_batch: function () {
        var self = openingstock;
        self.error_count = self.validate_create_batch();
        if (self.error_count > 0) {
            return;
        }
        var batchname = $("#add-batch #BatchName").val();
        var expiryDate = $("#add-batch #expiry-date").val();
        var rate = clean($("#add-batch #Rate").val());
        var itemid = clean($("#add-batch #ItemID").val());
        var POTransID = clean($("#add-batch #POTransID").val());
        var RetailMRP = clean($("#add-batch #RetailMRP").val());
        var RetailLooseRate = clean($("#add-batch #RetailLooseRate").val());
        var PurchaseMRP = clean($("#add-batch #PurchaseMRP").val());
        var PurchaseLooseRate = clean($("#add-batch #PurchaseLooseRate").val());

        var UnitID = clean($("#add-batch #UnitID").val());
        var Unit = $("#add-batch #UnitID option:selected").text();
        var PackSize = clean($("#add-batch #PackSize").val());
        if (batchname == "Create new Batch") {
            app.show_error("Invalid batch name");
            return;
        }
        var modal;
        var row = $("#Row").val();
        modal = self.get_batch_data();
        $.ajax({
            url: '/Masters/Batch/CreateBatch',
            data: modal,
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Batch created");
                    $("#BatchID").val(data.data)
                    $("#UnitID").val(UnitID)
                    UIkit.modal($('#add-batch')).hide();
                }
                else {
                    app.show_error(data.message);
                }
            },
        });
    },

    get_batch_data: function () {
        var model = {
            ID: 0,
            ExpDate: $("#add-batch #expiry-date").val(),
            ManufacturingDate: $("#add-batch #manufacture-date").val(),
            ItemID: clean($("#add-batch #ItemID").val()),
            BatchNo: $("#add-batch #BatchName").val(),
            ISKPrice: clean($("#add-batch #RetailMRP").val()),
            OSKPrice: 1,
            ExportPrice: 1,
            RetailMRP: clean($("#add-batch #RetailMRP").val()),
            RetailLooseRate: clean($("#add-batch #RetailLooseRate").val()),
            BatchRate: clean($("#add-batch #PurchaseMRP").val()),
            PurchaseLooseRate: clean($("#add-batch #PurchaseLooseRate").val()),
            UnitID: clean($("#add-batch #UnitID").val()),
            ProfitPrice: clean($("#add-batch #ProfitPrice").val())
        };
        return model;
    },

    select_item: function () {
        var self = openingstock;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var UnitID = $(row).find(".PrimaryUnitID").val();
        var Stock = $(row).find(".Stock").val();
        var Category = $(row).find(".ItemCategory").val();
        var rate = $(row).find(".Rate").val();

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#Unit").val(Unit);
        $("#UnitID").val(UnitID);
        $("#Category").val(Category);
        $("#Stock").val(Stock);
        $("#Rate").val(rate);
        $("#txtRequiredQty").focus();
        self.is_finished_good(Category);
        self.clear_item_select();
    },

    is_finished_good: function (Category) {
        if (Category == "Finished Goods") {
            $("#BatchTypeID").removeAttr("disabled").addClass("enabled");
        }
        else {
            $("#BatchTypeID").attr("disabled", "disabled").removeClass("enabled");
        }
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAllItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item_details: function (event, items) {   // on select auto complete item
        var self = openingstock;
        $("#ItemID").val(items.id);
        $("#Unit").val(items.unit);
        $("#UnitID").val(items.UnitID);
        $("#ItemName").val(items.value);
        $("#Category").val(items.category);
        $("#Stock").val(items.stock);
        $("#Rate").val(items.rate);
        self.is_finished_good(items.category);
        self.clear_item_select();
    },

    count: function () {
        index = $("#OpeningStock tbody").length;
        $("#item-count").val(index);
    },

    add_item: function () {
        var self = openingstock;
        if (self.validate_item() > 0) {
            return;
        }
        var category = $("#Category").val();
        var categoryID = $("#ItemCategoryID Option:selected").val();
        var storeID = $("#StoreID Option:selected").val();
        var name = $("#ItemName").val();
        var nameID = $("#ItemID").val();
        var store = $("#StoreID Option:selected").text();
        var batchtype = $("#BatchTypeID Option:selected").text();
        var batchtypeID = $("#BatchTypeID Option:selected").val();
        var ExpDate = $("#ExpDate").val();
        var unit = $("#Unit").val();
        var unitID = $("#UnitID").val();
        var qty = $("#RequiredQty").val();
        var batch = $("#Batch").val();
        var batchID = $("#BatchID").val();
        var value = $("#Value").val();
        
        var content = "";
        var $content;
        index = $("#OpeningStock tbody").length;
        var sino = "";
        sino = $('#OpeningStock tbody tr').length + 1;
        content = '<tr>'
            + '<td class="uk-text-center serial-no">' + sino + '</td>'
            + '<td class="item-name">' + name
            + '<input type="hidden" class = "category-ID" value="' + categoryID + '" />'
            + '<input type="hidden" class = "batchtype-ID" value="' + 1 + '" />'
            + '<input type="hidden" class = "store-ID" value="' + storeID + '" />'
            + '<input type="hidden" class = "item-ID" value="' + nameID + '" />'
            + '<input type="hidden" class = "unit-ID" value="' + unitID + '" />'
            + '<input type="hidden" class = "batch-ID" value="' + batchID + '" />'
            + '</td>'
            + '<td class="batch">' + batch + '</td>'
            + '<td class="ExpDate">' + ExpDate + '</td>'
            + '<td class="unit">' + unit + '</td>'
            + '<td class="RequiredQty mask-production-qty">' + qty + '</td>'
            + '<td class="Value mask-currency">' + value + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#OpeningStock tbody').append($content);

        self.count();
        self.clear_data();
    },

    validate_item: function () {
        var self = openingstock;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    delete_item: function () {
        var self = openingstock;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#OpeningStock tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#OpeningStock tbody tr").length);

    },

    validate_form: function () {
        var self = openingstock;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    on_save: function () {

        var self = openingstock;
        var data = self.get_data();
        var location = "/Stock/OpeningStock/Index";
        var url = '/Stock/OpeningStock/Save';
        index = $("#OpeningStock tbody").length;
        $("#item-count").val(index);

        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
            url = '/Stock/OpeningStock/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            if ($(this).hasClass("btnSaveNew")) {
                location = "/Stock/OpeningStock/Create";
            }
            self.error_count = self.validate_form();
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
        var self = openingstock;

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                }
            }
        });
    },

    get_data: function () {
        var self = openingstock;
        var data = {};
        data.TransNo = $("#TransNo").val();
        data.ID = $("#ID").val();
        data.Date = $("#txtDate").val();
        data.Items = [];
        var item = {};
        $('#OpeningStock tbody tr ').each(function () {
            item = {};
            item.WarehouseID = $(this).find(".store-ID").val();
            item.ItemID = $(this).find(".item-ID").val();
            item.BatchTypeID = $(this).find(".batchtype-ID").val();
            item.BatchID = $(this).find(".batch-ID").val();
            item.Batch = $(this).find(".batch").text();
            item.UnitID = $(this).find(".unit-ID").val();
            item.Qty = clean($(this).find(".RequiredQty").val());
            item.Value = clean($(this).find(".Value").val());
            item.ExpDate = $(this).find(".ExpDate").text();
            data.Items.push(item);
        });
        return data;
    },

    get_MRP: function (release) {
        var self = openingstock;
        var ItemID = $("#ItemID").val();
        var BatchTypeID = 1;
        var Batch = $("#Batch").val();
        $("#Value").val('');
        $.ajax({
            url: '/Stock/OpeningStock/GetMRPForOpeningStock',
            data: {
                ItemID: ItemID,
                BatchTypeID: BatchTypeID,
                Batch: Batch
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $.each(response.Data, function (i, record) {
                    $("#Value").val(record.MRP);
                    if (record.MRP > 0) {
                        $("#Value").attr("disabled", "disabled").removeClass("enabled");
                    }
                });
            }

        });
    },

    clear_item_select: function () {
        var self = openingstock;
        $("#BatchTypeID").val(1);
        $("#RequiredQty").val('');
        $("#BatchID").val('');
        $("#Batch").val('');
        $("#Value").val('');
        
        $("#ExpDate").val('');
        $("#Value").removeAttr("disabled").addClass("enabled");
    },

    clear_data: function () {
        var self = openingstock;
        $("#Category").val('');
        $("#ItemCategoryID").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#Unit").val('');
        $("#UnitID").val('');
        $("#RequiredQty").val('');
        $("#BatchID").val('');
        $("#Batch").val('');
        $("#Value").val('');
        $("#ExpDate").val('');
        $("#Value").removeAttr("disabled").addClass("enabled");
    },

    rules: {
        on_add: [
         {
             elements: "#ItemID",
             rules: [
                 { type: form.required, message: "Please choose an item", alt_element: "#ItemName" },
                 { type: form.positive, message: "Please choose an item", alt_element: "#ItemName" },
                 { type: form.non_zero, message: "Please choose an item", alt_element: "#ItemName" }
             ],
         },
         {
             elements: "#StoreID",
             rules: [
                 { type: form.required, message: "Please choose Store" },
             ]
         },
         {
             elements: "#RequiredQty",
             rules: [
                 { type: form.required, message: "Please Fill Quantity" },
                 { type: form.positive, message: "Please Fill Quantity" },
                 { type: form.non_zero, message: "Please Fill Quantity" }
             ],
         },
         {
             elements: "#Batch",
             rules: [
                 { type: form.required, message: "Please Select Batch" },
             ],
         },
         {
             elements: "#ExpDate",
             rules: [
                 { type: form.required, message: "please fill ExpDate" }
             ],
         },
        ],

        on_save: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ],
            }
        ]
    },

}
