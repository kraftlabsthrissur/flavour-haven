var gridcurrencyclass = '';
var DecimalPlaces = 0;
DirectPurchaseInvoice = {
    init: function () {
        var self = DirectPurchaseInvoice;
        item_list = self.purchase_item_list();//item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        index = $("#purchase-order-items-list tbody tr").length;
        $("#item-count").val(index);
        supplier.supplier_list('stock');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        self.bind_events();
        self.set_gst_enable();
        freeze_header = $("#purchase-order-items-list").FreezeHeader();
    },
    purchase_item_list: function () {

        var $list = $('#item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetItemsListForPurchase";

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
                            { Key: "ItemCategoryID", Value: $('#DDLItemCategory').val() },
                            { Key: "PurchaseCategoryID", Value: $('#DDLPurchaseCategory').val() },
                            { Key: "BusinessCategoryID", Value: $('#BusinessCategoryID').val() },
                            { Key: "SupplierID", Value: $('#SupplierID').val() },
                            { Key: "Type", Value: $('#type').val() },

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
                                + "<input type='hidden' class='QtyUnderQC' value='" + row.QtyUnderQC + "'>"
                                + "<input type='hidden' class='QtyOrdered' value='" + row.QtyOrdered + "'>"
                                + "<input type='hidden' class='lastPr' value='" + row.LastPr + "'>"
                                + "<input type='hidden' class='lowestPr' value='" + row.LowestPr + "'>"
                                + "<input type='hidden' class='pendingOrderQty' value='" + row.PendingOrderQty + "'>"
                                + "<input type='hidden' class='qtyWithQc' value='" + row.QtyWithQc + "'>"
                                + "<input type='hidden' class='qtyAvailable' value='" + row.QtyAvailable + "'>"
                                + "<input type='hidden' class='gstPercentage' value='" + row.GstPercentage + "'>"
                                + "<input type='hidden' class='vatPercentage' value='" + row.VATPercentage + "'>"
                                + "<input type='hidden' class='retailLooseRate' value='" + row.RetailLooseRate + "'>"
                                + "<input type='hidden' class='retailMRP' value='" + row.RetailMRP + "'>"
                                + "<input type='hidden' class='partsnumber' value='" + row.PartsNumber + "'>"
                                + "<input type='hidden' class='model' value='" + row.Model + "'>"
                                + "<input type='hidden' class='remark' value='" + row.Remark + "'>"
                                + "<input type='hidden' class='SecondaryUnits' value='" + row.SecondaryUnits + "'>"
                                + "<input type='hidden' class='BatchNo' value='" + row.BatchNo + "'>"
                                + "<input type='hidden' class='itemCategory' value='" + row.ItemCategory + "'>"
                                + "<input type='hidden' class='itemCategoryID' value='" + row.ItemCategoryID + "'>"
                                + "<input type='hidden' class='finishedGoodsCategoryID' value='" + row.FinishedGoodsCategoryID + "'>"
                                + "<input type='hidden' class='TravelCategoryID' value='" + row.TravelCategoryID + "'>"
                                + "<input type='hidden' class='PrimaryUnit' value='" + row.PrimaryUnit + "'>"
                                + "<input type='hidden' class='PrimaryUnitID' value='" + row.PrimaryUnitID + "'>"
                                + "<input type='hidden' class='PurchaseUnit' value='" + row.PurchaseUnit + "'>"
                                + "<input type='hidden' class='PurchaseUnitID' value='" + row.PurchaseUnitID + "'>"
                                + "<input type='hidden' class='PurchaseCategoryID' value='" + row.PurchaseCategoryID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "PartsNumber", "className": "PartsNumber" },
                    { "data": "Remark", "className": "Remark" },
                    { "data": "Model", "className": "Model" },
                    { "data": "PurchaseUnit", "className": "Unit" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "PurchaseCategory", "className": "PurchaseCategory" },
                    { "data": "PendingOrderQty", "className": "PendingOrderQty" },
                    { "data": "QtyAvailable", "className": "QtyAvailable" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });

            $('body').on("change", '#DDLItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#DDLPurchaseCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#SupplierID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#BusinessCategoryID', function () {
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
    set_gst_enable: function () {
        var self = DirectPurchaseInvoice
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var StateID = $("#SupplierStateID").val();
        if ((IsGSTRegistered == "true" || IsGSTRegistered == "True") && StateID != 32) {
            $("#SGSTAmount").prop("disabled", true);
            $("#CGSTAmount").prop("disabled", true);
            $("#IGSTAmount").prop("disabled", false);
        }
        else if ((IsGSTRegistered == "true" || IsGSTRegistered == "True") && StateID == 32) {
            $("#SGSTAmount").prop("disabled", false);
            $("#CGSTAmount").prop("disabled", false);
            $("#IGSTAmount").prop("disabled", true);
        }
    },
    list: function () {
        var self = DirectPurchaseInvoice;

        $('#tabs-directpurchase').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = DirectPurchaseInvoice;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#direct-purchase-draft-list');
                break;
            case "Purchased":
                $list = $('#direct-purchased');
                break;
            case "cancelled":
                $list = $('#cancelled-purchase-list');
                break;
            default:
                $list = $('#purchase-order-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Purchase/DirectPurchaseInvoice/GetDirectPurchaseInvoiceList?type=" + type;

            list_table = $list.dataTable({
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
                    { "data": "PODate", "className": "PODate" },
                    { "data": "Supplier", "className": "Supplier" },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/DirectPurchaseInvoice/Details/" + Id);
                    });
                },
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    select_supplier: function () {
        var self = DirectPurchaseInvoice
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        var IsInterCompany = $(row).find(".IsInterCompany").val();
        var InterCompanyLocationID = clean($(row).find(".InterCompanyLocationID").val());
        var CurrencyID = $(row).find(".CurrencyID").val();
        var CurrencyCode = $(row).find(".CurrencyCode").val();
        var CurrencyName = $(row).find(".CurrencyName").val();
        var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
        DecimalPlaces = clean($(row).find(".DecimalPlaces").val());
        gridcurrencyclass = app.change_decimalplaces($("#NetAmount"), DecimalPlaces);
        $("#DecimalPlaces").val(DecimalPlaces);
        $("#GridCurrencyClass").val(gridcurrencyclass);
        $("#CurrencyConversionRate").val(CurrencyConversionRate);
        app.change_decimalplaces($("#TaxableAmount"), DecimalPlaces);
        app.change_decimalplaces($("#VATAmount"), DecimalPlaces);
        app.change_decimalplaces($("#VATAmount"), DecimalPlaces);
        app.change_decimalplaces($("#Discount"), DecimalPlaces);
        app.change_decimalplaces($("#MRP"), DecimalPlaces);
        app.change_decimalplaces($("#RetailMRP"), DecimalPlaces);
        app.change_decimalplaces($("#Rate"), DecimalPlaces);
        app.change_decimalplaces($("#RetailRate"), DecimalPlaces);
        if ($("#purchase-order-items-list tbody tr").length > 0) {
            app.confirm("Items will be removed", function () {
                $("#InterCompanyLocationID").val(InterCompanyLocationID);
                $("#SupplierName").val(Name);
                $("#SupplierLocation").val(Location);
                $("#SupplierID").val(ID);
                $("#SupplierStateID").val(StateID);
                $("#InterCompanyLocationID").val(InterCompanyLocationID);
                $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
                $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
                $("#IsInterCompany").val(IsInterCompany);
                $("#CurrencyID").val(CurrencyID);
                $("#CurrencyCode").val(CurrencyCode);
                $("#CurrencyName").val(CurrencyName);
            }, function () {
            });
        }
        else {
            $("#InterCompanyLocationID").val(InterCompanyLocationID);
            $("#SupplierName").val(Name);
            $("#SupplierLocation").val(Location);
            $("#SupplierID").val(ID);
            $("#SupplierStateID").val(StateID);
            $("#InterCompanyLocationID").val(InterCompanyLocationID);
            $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
            $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
            $("#IsInterCompany").val(IsInterCompany);
            $("#CurrencyID").val(CurrencyID);
            $("#CurrencyCode").val(CurrencyCode);
            $("#CurrencyName").val(CurrencyName);
        }
        self.set_gst_enable();
    },

    bind_events: function () {
        var self = DirectPurchaseInvoice
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKSupplier").on("click", self.select_supplier);

        $.UIkit.autocomplete($('#batch-autocomplete'), { 'source': self.get_batches, 'minLength': 1 });
        $('#batch-autocomplete').on('selectitem.uk.autocomplete', self.set_batch_details);
        //$("body").on("keyup", "#IGSTAmount , #SGSTAmount , #CGSTAmount", self.calculate_grid_total);
        // $('#IGSTAmount , #SGSTAmount , #CGSTAmount').on('keyup', self.calculate_gst_total);

        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);

        $("#DDLPurchaseCategory").on('change', self.clear_item_on_select_purchasecategory);
        $('body').on('click', '#btnAddProduct', self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("keyup change", "#purchase-order-items-list tbody .Qty, .Rate, .MRP", self.update_amount);
        $("body").on("keyup change", "#purchase-order-items-list tbody .DiscountPercent", self.update_discount_amount);
        $("body").on("keyup change", "#purchase-order-items-list tbody .Discount,#Discount", self.update_discount_percent);
        $(".btnSave, .btnSaveDraft").on("click", self.on_save);
        //$('#Discount, #OtherDeductions, #IGSTAmount , #SGSTAmount , #CGSTAmount').on('keyup', self.calculate_gst_total);
        $('#Discount, #OtherDeductions, #VATAmount').on('keyup', self.calculate_vat_total);
        $("body").on("keyup change", "#RetailMRP", self.calculate_loose_price);
        $("body").on("keyup change", "#MRP", self.calculate_loose_price);
        $("#UnitID").on("change", self.calculate_loose_price);
        $("body").on("keyup change", "#purchase-order-items-list tbody tr .secondary .secondaryQty, .secondaryUnit, .secondaryRate", self.change_grid_package_values);
    },
    change_grid_package_values: function () {
        var self = DirectPurchaseInvoice
        var $tr = $(this).closest('tr');
        var SecondaryQty = clean($tr.find('.secondaryQty').val());
        var SecondaryUnitSize = clean($tr.find('.secondaryUnit').val());
        if ($(this).attr('class').indexOf('secondaryRate') != -1) {
            var SecondaryRate = clean($tr.find('.secondaryRate').val());
            var MRP = (SecondaryRate / SecondaryUnitSize).toFixed(10);
            $tr.find('.MRP').val(MRP);
            $tr.find('.MRP').trigger('change');

        } else {
            var MRP = clean($tr.find('.MRP').val());
            var SecondaryRate = (MRP * SecondaryUnitSize).toFixed(10);
            $tr.find('.secondary .secondaryRate').val(SecondaryRate);
            var Qty = SecondaryQty * SecondaryUnitSize;
            $tr.find('.Qty').val(Qty);
            $tr.find('.Qty').trigger('change');
        }

    },
    calculate_loose_price: function () {
        var retailMRP = clean($("#RetailMRP").val());
        var Category = $('#Category').val();
        var ConversionFactorPtoI = $("#ConversionFactorPtoI").val();
        var purchaseMRP = clean($("#MRP").val());
        var packSize = $('#UnitID option:selected').data("packsize");
        if (Category == 'Arishtams' || Category == 'Asavams' || Category == 'Kashayams' || Category == 'Kuzhambu' || Category == 'Thailam (Enna)' || Category == 'Thailam (Keram)' || Category == 'Dravakam') {
            retailLoose = retailMRP * ConversionFactorPtoI;
            purchaseLoose = purchaseMRP * ConversionFactorPtoI;
        }
        else {
            var retailLoose = retailMRP / packSize;
            var purchaseLoose = purchaseMRP / packSize;
        }
        $("#RetailRate").val(retailLoose);
        $("#Rate").val(purchaseLoose);
    },
    cancel_confirm: function () {
        var self = DirectPurchaseInvoice;
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        var self = DirectPurchaseInvoice;
        $(".btnSave, .btnSaveDraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Purchase/DirectPurchaseInvoice/Cancel',
            data: {
                PurchaseOrderID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Purchase invoice cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/DirectPurchaseInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },

    get_batches: function (release) {
        $.ajax({
            url: '/Masters/Batch/GetBatchesForAutoComplete',
            data: {
                Hint: $('#BatchNo').val(),
                ItemID: $("#ItemID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_batch_details: function (event, item) {
        var self = DirectPurchaseInvoice;
        $("#Value").val('');
        $("#Value").removeAttr("disabled").addClass("enabled");
        $("#Batch").val(item.value);
        $("#BatchID").val(item.id);
        self.get_rate_on_batch_selection(item.value);
    },

    select_item: function () {

        var self = DirectPurchaseInvoice
        $("#BatchNo").val('');
        /* $("#ExpDate").val('');*/
        $("#Rate").val('');
        $("#MRP").val('');
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var PrimaryUnit = $(row).find(".PrimaryUnit").val();
        var PrimaryUnitID = $(row).find(".PrimaryUnitID").val();
        var PurchaseUnit = $(row).find(".PurchaseUnit").val();
        var PurchaseUnitID = $(row).find(".PurchaseUnitID").val();
        var Stock = $(row).find(".Stock").val();
        var Category = $(row).find(".ItemCategory").val();
        var QtyUnderQC = $(row).find(".QtyUnderQC").val();
        var QtyOrdered = $(row).find(".QtyOrdered").val();
        var gstPercentage = clean($(row).find(".gstPercentage").val());
        var vatPercentage = clean($(row).find(".vatPercentage").val());
        var retailMRP = clean($(row).find(".retailMRP").val());
        var retailLooseRate = clean($(row).find(".retailLooseRate").val());
        var PartsNumber = $(row).find(".partsnumber").val();
        var Model = $(row).find(".model").val();
        var Remark = $(row).find(".remark").val();
        var SecondaryUnits = $(row).find(".SecondaryUnits").val();
        var BatchNo = $(row).find(".BatchNo").val();
        $("#ItemID").val(ID);
        $("#ItemCode").val(Code);
        $("#Stock").val(Stock);
        $("#PrimaryUnit").val(PrimaryUnit);
        $("#PrimaryUnitID").val(PrimaryUnitID);
        $("#PurchaseUnit").val(PurchaseUnit);
        $("#PurchaseUnitID").val(PurchaseUnitID);
        $("#ItemName").val(Name);
        $("#GSTPercentage").val(gstPercentage);
        $("#VATPercentage").val(vatPercentage);
        $("#PartsNumber").val(PartsNumber);
        $("#Model").val(Model);
        $("#Remark").val(Remark);
        $("#RetailMRP").val(retailMRP);
        $("#RetailRate").val(retailLooseRate);
        $("#SecondaryUnits").val(SecondaryUnits);
        $("#BatchNo").val(BatchNo);
        UIkit.modal($('#select-item')).hide();
        self.get_item_unit_category();
        self.get_units();
        self.get_MRP();
    },

    get_MRP: function () {
        var self = DirectPurchaseInvoice;
        var ItemID = $("#ItemID").val();
        $("#MRP").val('');
        $.ajax({
            url: '/Purchase/DirectPurchaseInvoice/GetMRPForPurchaseInvoice',
            data: {
                ItemID: ItemID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $.each(response.Data, function (i, record) {
                    $("#MRP").val(record.MRP);
                    $("#Rate").val(record.Rate);
                });
            }

        });
    },

    get_rate_on_batch_selection: function (Batch) {
        var self = DirectPurchaseInvoice;
        var ItemID = $("#ItemID").val();
        //var Batch = $("#BatchNo").val();
        $.ajax({
            url: '/Purchase/DirectPurchaseInvoice/GetMRPForPurchaseInvoiceByBatchID',
            data: {
                ItemID: ItemID,
                Batch: Batch
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $.each(response.Data, function (i, record) {
                    $("#MRP").val(record.MRP);
                    $("#ExpDate").val(record.ExpDate);
                });
            }

        });
    },

    set_item_details: function (event, item) {   // on select auto complete item
        var self = DirectPurchaseInvoice

        //alert(JSON.stringify(item));
        $("#BatchNo").val('');
        /*  $("#ExpDate").val('');*/
        $("#Rate").val('');
        $("#MRP").val('');
        $("#ItemID").val(item.id);
        $("#ItemTypeID").val(item.itemTypeId);
        $("#Stock").val(item.stock);
        $("#QtyUnderQC").val(item.qtyUnderQc);
        $("#QtyOrdered").val(item.qtyOrdered);
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#PurchaseUnit").val(item.purchaseUnit);
        $("#PurchaseUnitID").val(item.purchaseUnitId);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#VATPercentage").val(item.vatpercentage);
        $("#ItemCode").val(item.code);
        $("#ItemName").val(item.name);
        $("#PartsNumber").val(item.partsnumber);
        $("#Remark").val(item.remark);
        $("#Model").val(item.model);
        $("#RetailMRP").val(item.retailmrp);
        $("#RetailRate").val(item.retaillooserate);
        self.get_item_unit_category();
        self.get_units();
        $('#UnitID').focus();
    },
    get_units: function () {
        var self = DirectPurchaseInvoice;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
        $("#UnitID").append(html);
    },

    get_item_unit_category: function () {
        var self = DirectPurchaseInvoice;
        var ItemID = $("#ItemID").val();
        $.ajax({
            url: '/Purchase/DirectPurchaseInvoice/GetUnitsAndCategoryByItemID',
            data: {
                ItemID: ItemID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $.each(response.Data, function (i, record) {
                    $("#PurchaseUnitID").val(record.PurchaseUnitID);
                    $("#PurchaseUnit").val(record.PurchaseUnit);
                    $("#Category").val(record.Category);
                    $("#ConversionFactorPtoI").val(record.ConversionFactorPtoI);
                });
            }

        });
    },

    get_items: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
            data: {
                term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_supplier_details: function (event, item) {   // on select auto complete item
        var self = DirectPurchaseInvoice
        if ($("#purchase-order-items-list tbody tr").length > 0) {

            app.confirm("Items will be removed", function () {
                $("#SupplierLocation").val(item.location);
                $("#SupplierName").val(item.value);
                $("#SupplierID").val(item.id);
                $("#SupplierStateID").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#IsInterCompany").val(item.isintercompany);
                $("#InterCompanyLocationID").val(item.interCompanyLocationid);
            }, function () {
            });
        }
        else {
            $("#SupplierLocation").val(item.location);
            $("#SupplierName").val(item.value);
            $("#SupplierID").val(item.id);
            $("#SupplierStateID").val(item.stateId);
            $("#IsGSTRegistered").val(item.isGstRegistered);
            $("#IsInterCompany").val(item.isintercompany);
            $("#InterCompanyLocationID").val(item.interCompanyLocationid);
        }
        self.set_gst_enable();
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
    add_item: function () {

        var self = DirectPurchaseInvoice
        var FormType = "Add";
        if (self.validate_form(FormType) > 0) {
            return;
        }
        var PurchaseCategoryID = $("#DDLPurchaseCategory").val();
        var SupplierStateID = clean($("#SupplierStateID").val());
        var ItemID = $("#ItemID").val();
        var ItemName = $("#ItemName").val();
        var UnitID = $("#UnitID").val();
        var Unit = $("#UnitID Option:selected").text();
        var Qty = clean($("#Qty").val());
        var GSTPercentage = clean($("#GSTPercentage").val());
        var MRP = clean($("#MRP").val());
        var Rate = clean($("#Rate").val());
        var RetailMRP = MRP;// clean($("#RetailMRP").val());
        var RetailRate = Rate;// clean($("#RetailRate").val());
        var DiscountPercent = $("#DiscountPercent").val();
        var Discount = (DiscountPercent * (Qty * MRP)) / 100;
        var value = (Qty * MRP) - Discount;
        var BatchNo = $("#BatchNo").val();
        /* var ExpDate = $("#ExpDate").val();*/
        var Remarks = $("#ItemRemarks").val();
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var GSTAmnt = 0;
        var CGSTPercent = 0;
        var SGSTPercent = 0;
        var IGSTPercent = 0;
        var CGSTAmt = 0;
        var SGSTAmt = 0;
        var IGSTAmt = 0;
        if (IsGSTRegistered == "true") {
            if (SupplierStateID == 32) {
                CGSTPercent = GSTPercentage / 2;
                SGSTPercent = GSTPercentage / 2;
                IGSTPercent = 0
            }
            else {
                CGSTPercent = 0;
                SGSTPercent = 0;
                IGSTPercent = GSTPercentage
            }
        }
        else {
            CGSTPercent = 0
            IGSTPercent = 0
            SGSTPercent = 0
            GSTPercentage = 0
        }
        CGSTAmt = value * CGSTPercent / 100;
        SGSTAmt = value * SGSTPercent / 100;
        IGSTAmt = value * IGSTPercent / 100;
        GSTAmnt = CGSTAmt + SGSTAmt + IGSTAmt;
        var TotalAmnt = value + GSTAmnt;
        var content = "";
        var $content;
        var sino = "";
        sino = $('#purchase-order-items-list tbody tr').length + 1;
        content = '<tr>'
            + '<td class="uk-text-center serial-no">' + sino + '</td>'
            + '<td class="item-name">' + ItemName
            + '<input type="hidden" class = "ItemID" value="' + ItemID + '" />'
            + '<input type="hidden" class = "UnitID" value="' + UnitID + '" />'
            + '<input type="hidden" class = "PurchaseCategoryID" value="' + PurchaseCategoryID + '" />'
            + '<input type="hidden" class = "CGSTPercent" value="' + CGSTPercent + '" />'
            + '<input type="hidden" class = "SGSTPercent" value="' + SGSTPercent + '" />'
            + '<input type="hidden" class = "IGSTPercent" value="' + IGSTPercent + '" />'
            + '<input type="hidden" class = "SGSTAmt" value="' + SGSTAmt + '" />'
            + '<input type="hidden" class = "IGSTAmt" value="' + IGSTAmt + '" />'
            + '<input type="hidden" class = "CGSTAmt" value="' + CGSTAmt + '" />'
            + '<input type="hidden" class = "BatchTypeID" value="' + 1 + '" />'
            + '<input type="hidden" class = "Rate" value="' + Rate + ' " /></td>'
            + '<input type="hidden" class = "RetailMRP"  value="' + RetailMRP + ' " />'
            + '<input type="hidden" class = "RetailRate" value="' + RetailRate + ' " />'
            + '<td class="Unit">' + Unit + '</td>'
            + '<td><input type="text" class = "md-input BatchNo uk-hidden" value="' + BatchNo + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty Qty" value="' + Qty + '" /></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency MRP " disabled value="' + MRP + ' " /></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency DiscountPercent" value="' + DiscountPercent + '"/></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency Discount" value="' + Discount + '"/></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency TaxableAmount" value="' + value + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency GSTPercentage" value="' + GSTPercentage + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency GSTAmnt" value="' + GSTAmnt + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-positive-currency TotalAmnt" value="' + TotalAmnt + '" disabled /></td>'
            /*    + '<td><input type="text" class = "md-input ExpDate" value="' + ExpDate + '" disabled /></td>'*/
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#purchase-order-items-list tbody').append($content);
        index = $("#purchase-order-items-list tbody tr").length;
        $("#item-count").val(index);
        self.calculate_grid_total();
        self.clear();
        $("#ItemName").focus();
        freeze_header.resizeHeader();
    },

    calculate_grid_total: function () {

        var NetAmount = 0;
        var GrossAmnt = 0;
        var SGSTAmount = 0;
        var IGSTAmount = 0;
        var CGSTAmount = 0;
        var VATAmount = 0;
        var DiscountAmount = 0;
        var IsGST = clean($("#IsGST").val());
        var IsVat = clean($("#IsVat").val());
        $('#purchase-order-items-list tbody tr').each(function () {
            if (IsGST == 1) {
                var total = clean($(this).find(".TotalAmnt").val());
                var SGSTAmt = clean($(this).find(".SGSTAmt").val());
                var CGSTAmt = clean($(this).find(".CGSTAmt").val());
                var IGSTAmt = clean($(this).find(".IGSTAmt").val());

                var GrossAmount = clean($(this).find(".TaxableAmount").val());
                var discount = clean($(this).find(".Discount").val());
                GrossAmnt = GrossAmnt + GrossAmount;
                DiscountAmount = DiscountAmount + discount;
                SGSTAmount = SGSTAmount + SGSTAmt;
                IGSTAmount = IGSTAmount + IGSTAmt;
                CGSTAmount = CGSTAmount + CGSTAmt;
                NetAmount = NetAmount + total;
            }
            if (IsVat == 1) {
                var total = clean($(this).find(".TotalAmnt").val());
                var VATAmt = clean($(this).find(".VATAmount").val());
                var GrossAmount = clean($(this).find(".TaxableAmount").val());
                var discount = clean($(this).find(".Discount").val());

                GrossAmnt = GrossAmnt + GrossAmount;
                DiscountAmount = DiscountAmount + discount;
                VATAmount = VATAmount + VATAmt;
                NetAmount = NetAmount + total;


            }
        });

        var discount = clean($('#Discount').val());
        var deductions = clean($('#OtherDeductions').val());

        GrossAmnt = GrossAmnt - (discount + deductions);
        if (IsGST == 1) {
            $("#SGSTAmount").val(SGSTAmount);
            $("#CGSTAmount").val(CGSTAmount);
            $("#IGSTAmount").val(IGSTAmount);
            $("#TaxableAmount").val(GrossAmnt);
            var GSTAmount = clean($('#SGSTAmount').val()) + clean($('#CGSTAmount').val()) + clean($('#IGSTAmount').val())
            NetAmount = GrossAmnt + GSTAmount
            $("#NetAmount").val(NetAmount);
        }
        if (IsVat == 1) {
            NetAmount = GrossAmnt + VATAmount;
            $("#NetAmount").val(NetAmount);
        }
    },

    calculate_gst_total: function () {
        var NetAmount = clean($("#NetAmount").val());
        var GrossAmnt = clean($("#TaxableAmount").val());
        var SGSTAmount = clean($("#SGSTAmount").val());
        var IGSTAmount = clean($("#IGSTAmount").val());
        var CGSTAmount = clean($("#CGSTAmount").val());

        var discount = clean($('#Discount').val());
        var deductions = clean($('#OtherDeductions').val());
        GrossAmnt = GrossAmnt - (discount + deductions);
        var GSTAmount = clean($('#SGSTAmount').val()) + clean($('#CGSTAmount').val()) + clean($('#IGSTAmount').val())
        NetAmount = GrossAmnt + GSTAmount
        $("#NetAmount").val(NetAmount);

    },

    calculate_vat_total: function () {
        var NetAmount = clean($("#NetAmount").val());
        var GrossAmnt = clean($("#TaxableAmount").val());
        var VATAmount = clean($("#VATAmount").val());
        var discount = clean($('#Discount').val());
        var deductions = clean($('#OtherDeductions').val());
        GrossAmnt = GrossAmnt - (discount + deductions);

        NetAmount = GrossAmnt + VATAmount
        $("#NetAmount").val(NetAmount);

    },

    on_save: function () {

        var self = DirectPurchaseInvoice
        var model = self.get_data();
        var location = "/Purchase/DirectPurchaseInvoice/Index";
        var url = '/Purchase/DirectPurchaseInvoice/Save';
        var FormType = "Form";

        if ($(this).hasClass("btnSaveDraft")) {
            model.IsDraft = true;
            url = '/Purchase/DirectPurchaseInvoice/SaveAsDraft'
            self.error_count = self.validate_form(FormType);
        } else {
            self.error_count = self.validate_form(FormType);
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
        var self = DirectPurchaseInvoice
        $(".btnSave,.btnSaveDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: model,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    app.show_error(response.message);
                    $(".btnSave,.btnSaveDraft").css({ 'display': 'block' });
                }
            }
        });
    },

    clear: function () {
        var self = DirectPurchaseInvoice;
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PrimaryUnit").val('');
        $("#PrimaryUnitID").val('');
        $("#PurchaseUnit").val('');
        $("#PurchaseUnitID").val('');
        $("#DDLPurchaseCategory").val('');
        $("#UnitID").val('');
        $("#Rate").val('');
        $("#MRP").val('');
        $("#RetailRate").val('');
        $("#RetailMRP").val('');
        $("#GSTPercentage").val('');
        $("#Qty").val('');
        $("#ItemRemarks").val('');
        /*  $("#ExpDate").val('');*/
        $("#BatchNo").val('');
        $("#ItemRemarks").val('');
        $("#DiscountPercent").val('');
        self.get_item_unit_category();
        self.get_units();
    },

    clear_item_on_select_purchasecategory: function () {
        var self = DirectPurchaseInvoice;
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PrimaryUnit").val('');
        $("#PrimaryUnitID").val('');
        $("#PurchaseUnit").val('');
        $("#PurchaseUnitID").val('');
        self.get_item_unit_category();
        self.get_units();
    },

    remove_item: function () {
        var self = DirectPurchaseInvoice;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#purchase-order-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#purchase-order-items-list tbody tr").length);
        self.calculate_grid_total();
    },

    get_data: function () {

        var self = DirectPurchaseInvoice
        var data = {};
        data.ID = $("#ID").val();
        data.SupplierID = $("#SupplierID").val();
        data.PurchaseOrderDate = $("#PurchaseOrderDate").val();
        data.NetAmount = clean($("#NetAmount").val());
        data.TaxableAmount = clean($("#TaxableAmount").val());
        data.SGSTAmount = clean($("#SGSTAmount").val());
        data.CGSTAmount = clean($("#CGSTAmount").val());
        data.IGSTAmount = clean($("#IGSTAmount").val());
        data.VATAmount = clean($("#VATAmount").val());
        data.IsGST = clean($("#IsGST").val());
        data.IsVat = clean($("#IsVat").val());
        data.Discount = clean($("#Discount").val());
        data.OtherDeductions = clean($("#OtherDeductions").val());
        data.StoreID = $("#StoreID").val();
        data.InvoiceNo = $("#InvoiceNo").val();
        data.InvoiceDate = $("#InvoiceDate").val();
        data.CurrencyID = $("#CurrencyID").val();
        data.Items = [];
        var item = {};
        $('#purchase-order-items-list tbody tr ').each(function () {
            item = {};
            item.ItemID = $(this).find(".ItemID").val();
            item.UnitID = $(this).find(".UnitID").val();
            item.PurchaseCategoryID = $(this).find(".PurchaseCategoryID").val();
            item.Qty = clean($(this).find(".Qty").val());
            item.Rate = clean($(this).find(".Rate").val());
            item.MRP = clean($(this).find(".MRP").val());
            item.RetailRate = clean($(this).find(".RetailRate").val());
            item.RetailMRP = clean($(this).find(".RetailMRP").val());
            item.TaxableAmount = clean($(this).find(".TaxableAmount").val());
            item.SGSTPercent = clean($(this).find(".SGSTPercent").val());
            item.CGSTPercent = clean($(this).find(".CGSTPercent").val());
            item.IGSTPercent = clean($(this).find(".IGSTPercent").val());
            item.GSTPercentage = clean($(this).find(".GSTPercentage").val());
            item.VATPercentage = clean($(this).find(".VATPercentage").val());
            item.ItemName = $(this).find(".ItemName").val();
            item.ItemCode = $(this).find(".ItemCode").val();
            item.PartsNumber = $(this).find(".PartsNumber").val();
            item.Remark = $(this).find(".Remark").val();
            item.Model = $(this).find(".Model").val();
            item.SecondaryUnit = $(this).find('.secondaryUnit option:selected').text().trim();
            item.SecondaryUnitSize = clean($(this).find('.secondaryUnit').val());;
            item.SecondaryRate = clean($(this).find('.secondaryRate').val());
            item.SecondaryQty = clean($(this).find('.secondaryQty').val());
            item.ExchangeRate = clean($(this).find('.ExchangeRate').val());
            item.IGSTAmount = clean($(this).find(".IGSTAmt").val());
            item.SGSTAmount = clean($(this).find(".SGSTAmt").val());
            item.CGSTAmount = clean($(this).find(".CGSTAmt").val());
            item.TotalAmount = clean($(this).find(".TotalAmnt").val());
            item.BatchTypeID = clean($(this).find(".BatchTypeID").val());
            item.VATAmount = clean($(this).find(".VATAmount").val());
            /*  item.ExpDate = $(this).find(".ExpDate").val()*/
            item.BatchNo = $(this).find(".BatchNo").val();
            item.Remarks = $(this).find(".Remarks").text();
            item.DiscountPercent = clean($(this).find(".DiscountPercent").val());
            item.Discount = clean($(this).find(".Discount").val());
            data.Items.push(item);
        });
        return data;
    },
    update_discount_amount: function () {
        var self = DirectPurchaseInvoice;
        var row = $(this).closest("tr");
        if (event.keyCode !== 110 && event.keyCode !== 190) {
            $(row).find(".Discount").val(0);
            $("#purchase-order-items-list tbody .Qty").trigger("change");
        }
    },
    update_discount_percent: function () {
        var self = DirectPurchaseInvoice;
        var row = $(this).closest("tr");
        if (event.keyCode !== 110 && event.keyCode !== 190) {
            $(row).find(".DiscountPercent").val(0);
            $("#purchase-order-items-list tbody .Qty").trigger("change");
        }
    },
    update_amount: function () {

        var self = DirectPurchaseInvoice;
        var CGSTAmt = 0;
        var SGSTAmt = 0;
        var IGSTAmt = 0;
        var GSTAmnt = 0;
        var VATAmount = 0;
        var total = 0
        var DiscountExtra = clean($("#Discount").val());
        var row = $(this).closest('tr');
        var qty = clean($(row).find(".Qty").val());
        var MRP = clean($(row).find(".MRP").val());
        var DiscountPercent = clean($("#DiscountPercent").val());
        var IsGST = clean($("#IsGST").val());
        var IsVat = clean($("#IsVat").val());
        var VATPercentage = clean($(row).find(".VATPercentage").val());
        var Discount = (DiscountPercent * (qty * MRP)) / 100;
        var value = (qty * MRP)
        if (IsGST == 1) {
            var CGSTPercent = clean($(row).find(".CGSTPercent").val());
            var SGSTPercent = clean($(row).find(".SGSTPercent").val());
            var IGSTPercent = clean($(row).find(".IGSTPercent").val());
            var Discount = clean($(row).find(".Discount").val());

            if (DiscountPercent > 0) {
                Discount = (DiscountPercent * (qty * MRP)) / 100;
            } else {
                DiscountPercent = (Discount * 100) / (qty * MRP);
            }
            //var Value = qty * MRP;
            var Value = qty * MRP - Discount - DiscountExtra;;
            CGSTAmt = Value * CGSTPercent / 100;
            SGSTAmt = Value * SGSTPercent / 100;
            IGSTAmt = Value * IGSTPercent / 100;
            GSTAmnt = CGSTAmt + SGSTAmt + IGSTAmt;
            total = Value + GSTAmnt;
        }
        if (IsVat == 1) {

            VATAmount = value * VATPercentage / 100;
            TotalAmnt = value + VATAmount;

        }
        $(row).find(".TaxableAmount").val(value);
        $(row).find(".CGSTAmt").val(CGSTAmt);
        $(row).find(".SGSTAmt").val(CGSTAmt);
        $(row).find(".IGSTAmt").val(IGSTAmt);
        $(row).find(".GSTAmnt").val(GSTAmnt)
        $(row).find(".TotalAmnt").val(TotalAmnt);

        $(row).find(".DiscountPercent").val(DiscountPercent);
        $(row).find(".Discount").val(Discount);
        self.calculate_grid_total();
    },
    validate_form: function (FormType) {
        var self = DirectPurchaseInvoice
        if (FormType == "Form") {
            if (self.rules.on_submit.length) {
                return form.validate(self.rules.on_submit);
            }
        } else if (FormType == "Add") {
            if (self.rules.on_add.length) {
                return form.validate(self.rules.on_add);
            }
        }
        return 0;
    },

    rules: {

        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose an item", alt_element: "#ItemName" },
                    { type: form.positive, message: "Please choose an item", alt_element: "#ItemName" },
                    { type: form.non_zero, message: "Please choose an item", alt_element: "#ItemName" },
                    {
                        type: function (element) {
                            var ItemID = clean($("#ItemID").val());
                            var error = false;
                            $('#purchase-order-items-list tbody tr').each(function () {
                                if ($(this).find('.ItemID').val() == ItemID) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "already added in the grid, try editing Qty"
                    },
                ],
            },
            {
                elements: "#UnitID",
                rules: [
                    { type: form.required, message: "Please choose an unit" },
                    { type: form.positive, message: "Please choose an unit" },
                    { type: form.non_zero, message: "Please choose an unit" }
                ],
            },

            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please Fill Quantity" },
                    { type: form.positive, message: "Please Fill Quantity" },
                    { type: form.non_zero, message: "Please Fill Quantity" }
                ],
            },
            {
                elements: "#Rate",
                rules: [
                    { type: form.required, message: "please fill rate" },
                    { type: form.positive, message: "please fill rate" },
                    { type: form.non_zero, message: "please fill rate" }
                ],
            },
            {
                elements: "#MRP",
                rules: [
                    { type: form.required, message: "please fill MRP" },
                    { type: form.positive, message: "please fill MRP" },
                    { type: form.non_zero, message: "please fill MRP" }
                ],
            },
            //{
            //    elements: "#BatchNo",
            //    rules: [
            //        { type: form.required, message: "please fill BatchNo" }
            //    ],
            //},
            //{
            //    elements: "#ExpDate",
            //    rules: [
            //        { type: form.required, message: "please fill ExpDate" }
            //    ],
            //},
        ],
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ],
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Please Fill InvoiceNo" },
                ]
            },
            {
                elements: "#InvoiceDate",
                rules: [
                    { type: form.required, message: "Please Fill InvoiceDate" },
                    { type: form.non_zero, message: "Please Fill InvoiceDate" },
                    { type: form.positive, message: "Please Fill InvoiceDate" },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Please Fill Quantity" },
                    { type: form.positive, message: "Invalid Quantity" },
                    { type: form.non_zero, message: "Please Fill Quantity" }
                ],
            },
            {
                elements: ".Rate",
                rules: [
                    { type: form.required, message: "Please Fill Rate" },
                    { type: form.positive, message: "Invalid Rate" },
                    { type: form.non_zero, message: "Please Fill Rate" }
                ],
            },

            {
                elements: "#SupplierID",
                rules: [
                    { type: form.required, message: "Please choose an Supplier", alt_element: "#SupplierName" },
                    { type: form.positive, message: "Please choose an Supplier", alt_element: "#SupplierName" },
                    { type: form.non_zero, message: "Please choose an Supplier", alt_element: "#SupplierName" }
                ],
            },
            {
                elements: "#StoreID",
                rules: [
                    { type: form.required, message: "Please choose an Store" },
                    { type: form.positive, message: "Please choose an Store" },
                    { type: form.non_zero, message: "Please choose an Store" }
                ],
            },
        ],

    },

}