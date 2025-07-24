$(function () {

});
var a;
ServiceItem = {
    init: function () {
        var self = ServiceItem;
        self.bind_events();
        if ($('#ID').val() != 0) {
            self.GetItemLocationList();
        }
    },  
    list: function () {
        var $list = $('#tbl-item-list');
        $list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/ServiceItem/Details/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/ServiceItem/GetServiceItemList";
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemCategoryID", Value: $('#ItemCategoryID').val() },
                            { Key: "SalesCategoryID", Value: $('#SalesCategoryID').val() },
                            { Key: "PriceListID", Value: $('#PriceListID').val() },
                            { Key: "StoreID", Value: $('#StoreID').val() },
                            { Key: "CheckStock", Value: $('#CheckStock').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    {
                        "data": "Rate", "className": "", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Rate + "</div>";
                        }
                    },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    { "data": "AccountsCategory", "className": "AccountsCategory" },
                    {
                        "data": "stock", "className": "Stock", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Stock + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#SalesCategoryID', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#StoreID', function () {
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
    bind_events: function () {
        var self =ServiceItem;
        $('#ItemTypeID').on('change', self.GetItemCode)
        $('#UnitID').on('change', self.GetPackSize)
        $(".BtnSave").on("click", self.save_confirm);
        $(".BtnSaveAsDraft").on("click", self.save);
        $("#CategoryID").on("change", self.set_unit);
        if ($('#ID').val() > 0) {
            self.ItemDetails();
        }

        $("body").on("ifChanged", ".IsDisContinued", self.check_isdisContinued);
        $("body").on("ifChanged", ".Isactive", self.check_isactive);
        $(".ItemLocation").on('ifChanged', self.get_Location_data);
    },



    save_confirm: function () {
        var self =ServiceItem;
        self.error_count = 0;
        var FormType = "Form";
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Masters/Item/IsItemExist',
            data: {
                Name: $("#Name").val(),
                HSNCode: $("#HSNCode").val(),
                ID: clean($('#ID').val())
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.confirm_cancel(response.data, function () {
                        self.save();
                    }, function () {
                        $('.BtnSave').css({ 'visibility': 'visible' });
                    })
                }
            },
        });
    },

    set_unit: function () {
        var id = clean($("#ID").val());
        if (id == 0) {
            $("#Code").val($("#CategoryID option:selected").data("code"));
        }
        if ($("#CategoryID option:selected").data("type") == "Service") {
            $("#SecondaryUnitID").val($("#SecondaryUnitID option:contains(Nos)").val());
            $("#InventoryUnitID").val($("#InventoryUnitID option:contains(Nos)").val());
            $("#UnitID").val($("#UnitID option:contains(Nos)").val());
            $("#SalesUnitID").val($("#SalesUnitID option:contains(Nos)").val());
            $("#PurchaseUnitID").val($("#PurchaseUnitID option:contains(Nos)").val());
            $("#ConversionFactorPtoI").val(1);
            $("#ConversionFactorPurchaseToInventory").val(1);
            $("#ConversionFactorPtoSecondary").val(1);
            $("#ConversionFactorPtoS").val(1);
            $("#ConversionFactorSalesToInventory").val(1);
        }

        else {
            $("#SecondaryUnitID").val('');
            $("#InventoryUnitID").val('');
            $("#UnitID").val('');
            $("#SalesUnitID").val('');
            $("#PurchaseUnitID").val('');
            $("#ConversionFactorPtoI").val(0.00);
            $("#ConversionFactorPurchaseToInventory").val(0.00);
            $("#ConversionFactorPtoSecondary").val(0.00);
            $("#ConversionFactorPtoS").val(0.00);
            $("#ConversionFactorSalesToInventory").val(0.00);
        }

    },
    ItemDetails: function () {
        $('#IsStockItem').val() == "True" ? $('.IsStockItem').iCheck('check') : $('.IsStockItem').iCheck('uncheck');
        $('#Isactive ').val() == "True" ? $('.Isactive ').iCheck('check') : $('.Isactive ').iCheck('uncheck');
        $('#IsStockValue').val() == "True" ? $('.IsStockValue').iCheck('check') : $('.IsStockValue').iCheck('uncheck');
        $('#IsDemandPlanRequired').val() == "True" ? $('.IsDemandPlanRequired').iCheck('check') : $('.IsDemandPlanRequired').iCheck('uncheck');
        $('#IsMaterialPlanRequired').val() == "True" ? $('.IsMaterialPlanRequired').iCheck('check') : $('.IsMaterialPlanRequired').iCheck('uncheck');
        $('#IsPhantomItem').val() == "True" ? $('.IsPhantomItem').iCheck('check') : $('.IsPhantomItem').iCheck('uncheck');
        $('#IsSaleable').val() == "True" ? $('.IsSaleable').iCheck('check') : $('.IsSaleable').iCheck('uncheck');
        $('#N2GActivity').val() == "True" ? $('.N2GActivity').iCheck('check') : $('.N2GActivity').iCheck('uncheck');
        $('#IsMrp').val() == "True" ? $('.IsMrp').iCheck('check') : $('.IsMrp').iCheck('uncheck');
        $('#IsPriceListReference').val() == "True" ? $('.IsPriceListReference').iCheck('check') : $('.IsPriceListReference').iCheck('uncheck');
        $('#IsQCRequired').val() == "True" ? $('.IsQCRequired').iCheck('check') : $('.IsQCRequired').iCheck('uncheck');
        $('#IsQCRequiredForProduction').val() == "True" ? $('.IsQCRequiredForProduction').iCheck('check') : $('.IsQCRequiredForProduction').iCheck('uncheck');
        $('#IsProprietary').val() == "True" ? $('.IsProprietary').iCheck('check') : $('.IsProprietary').iCheck('uncheck');
        $('#IsPurchaseItem').val() == "True" ? $('.IsPurchaseItem').iCheck('check') : $('.IsPurchaseItem').iCheck('uncheck');
        $('#IsSeasonalPurchase').val() == "True" ? $('.IsSeasonalPurchase').iCheck('check') : $('.IsSeasonalPurchase').iCheck('uncheck');
        $('#IsPORequired').val() == "True" ? $('.IsPORequired').iCheck('check') : $('.IsPORequired').iCheck('uncheck');
        $('#IsAsset').val() == "True" ? $('.IsAsset').iCheck('check') : $('.IsAsset').iCheck('uncheck');
        $('#IsProject').val() == "True" ? $('.IsProject').iCheck('check') : $('.IsProject').iCheck('uncheck');
        $('#IsEmployee').val() == "True" ? $('.IsEmployee').iCheck('check') : $('.IsEmployee').iCheck('uncheck');
        $('#IsDepartment').val() == "True" ? $('.IsDepartment').iCheck('check') : $('.IsDepartment').iCheck('uncheck');
        $('#IsInterCompany').val() == "True" ? $('.IsInterCompany').iCheck('check') : $('.IsInterCompany').iCheck('uncheck');
        $('#IsLocation').val() == "True" ? $('.IsLocation').iCheck('check') : $('.IsLocation').iCheck('uncheck');
        $('#IsMasterFormula').val() == "True" ? $('.IsMasterFormula').iCheck('check') : $('.IsMasterFormula').iCheck('uncheck');
        $('#IsReProcessAllowed').val() == "True" ? $('.IsReProcessAllowed').iCheck('check') : $('.IsReProcessAllowed').iCheck('uncheck');
        $('#IsBatch').val() == "True" ? $('.IsBatch').iCheck('check') : $('.IsBatch').iCheck('uncheck');
        $('#IsDisContinued').val() == "True" ? $('.IsDisContinued').iCheck('check') : $('.IsDisContinued').iCheck('uncheck');
        //  $('#UnitID').val($('#UnitOMID').val());
        $("#ItemTypeID").attr("disabled", "disabled");
    },
    GetPackSize: function () {
        $('#PackSize').val($("#UnitID option:selected").data("uom"));
    },
    save: function (event) {
        var self = ServiceItem;
        var IsDraft, model = self.get_data()
        this.id == "BtnSaveAsDraft" ? IsDraft = true : IsDraft = false;
        model.IsDraft = IsDraft;

        var SalesModel = self.get_sales_data();
        var QCModel = self.get_QC_data();
        var CostModel = self.get_cost_data();
        var PurchaseModel = self.get_purchase_data();
        var AccountsModel = self.get_accounts_data();
        var ProductionModel = self.get_production_data();
        $.extend(model, SalesModel, QCModel, CostModel, PurchaseModel, AccountsModel, ProductionModel);
        model.ItemLocationList = self.LocationList
        $(".BtnSave").css({ 'visibility': 'hidden' });
        $(".BtnSaveAsDraft").css({ 'visibility': 'hidden' });
        $.ajax({
            url: '/Masters/ServiceItem/Save',
            data: { model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Item created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/ServiceItem/Index"
                    }, 1000);
                } else {
                    app.show_error("Failed to create Item");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".BtnSaveAsDraft").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: clean($('#ID').val()),
            Name: $('#Name').val(),
            MalayalamName: $('#MalayalamName').val(),
            HindiName: $('#HindiName').val(),
            SanskritName: $('#SanskritName').val(),
            StorageCategoryID: clean($('#StorageCategoryID').val()),
            ItemTypeID: clean($('#ItemTypeID').val()),
            Code: $('#Code').val(),
            OldItemCode: $('#OldItemCode').val(),
            OldItemCode2: $('#OldItemCode2').val(),
            HSNCode: $('#HSNCode').val(),
            BarCode: $('#BarCode').val(),
            QRCode: $('#QRCode').val(),
            Description: $('#Description').val(),
            CategoryID: clean($('#CategoryID').val()),
            BusinessCategoryID: clean($('#BusinessCategoryID').val()),
            SecondaryUnitID: clean($('#SecondaryUnitID').val()),
            InventoryUnitID: clean($('#InventoryUnitID').val()),
            UnitID: clean($('#UnitID').val().split('#')[0]),
            PackSize: clean($('#PackSize').val()),
            ConversionFactorPtoI: clean($('#ConversionFactorPtoI').val()),
            MinStockQTY: clean($('#MinStockQTY').val()),
            MaxStockQTY: clean($('#MaxStockQTY').val()),
            BirthDate: $('#BirthDate').val(),
            DisContinuedDate: $('#DisContinuedDate').val(),
            ItemTypeName: $("#ItemTypeID option:selected").text(),
            OldName: $("#OldName").val(),
            CategoryName: $('#CategoryID option:selected').text()

        };
        $('.IsStockItem').prop("checked") == true ? model.IsStockItem = true : model.IsStockItem = false;
        $('.IsStockValue').prop("checked") == true ? model.IsStockValue = true : model.IsStockValue = false;
        $('.IsDemandPlanRequired').prop("checked") == true ? model.IsDemandPlanRequired = true : model.IsDemandPlanRequired = false;
        $('.IsMaterialPlanRequired').prop("checked") == true ? model.IsMaterialPlanRequired = true : model.IsMaterialPlanRequired = false;
        $('.IsPhantomItem').prop("checked") == true ? model.IsPhantomItem = true : model.IsPhantomItem = false;
        if (clean($('#ID').val()) > 0) {
            $('.IsDisContinued').prop("checked") == true ? model.IsDisContinued = true : model.IsDisContinued = false;
        } else {
            model.IsDisContinued = false;
        }
        $('.Isactive ').prop("checked") == true ? model.Isactive = true : model.Isactive = false;
        return model;
    },
    get_sales_data: function () {
        var model = {
            SalesIncentiveCategoryID: clean($('#SalesIncentiveCategoryID').val()),
            SalesCategoryID: clean($('#SalesCategoryID').val()),
            DrugScheduleID: clean($('#DrugScheduleID').val()),
            SalesUnitID: clean($('#SalesUnitID').val()),
            ConversionFactorPtoS: clean($('#ConversionFactorPtoS').val()),
            MinSalesQtyFull: clean($('#MinSalesQtyFull').val()),
            MinSalesQtyLoose: clean($('#MinSalesQtyLoose').val()),
            MaxSalesQty: clean($('#MaxSalesQty').val()),
            DiseaseCategoryID: clean($('#DiseaseCategoryID').val()),
            SeasonStarts: $('#SeasonStarts').val(),
            SeasonEnds: $('#SeasonEnds').val(),
            ConversionFactorSalesToInventory: clean($('#ConversionFactorSalesToInventory').val()),
            ConversionFactorPurchaseToInventory: ($('#ConversionFactorPurchaseToInventory').val()),
        };
        $('.IsSaleable').prop("checked") == true ? model.IsSaleable = true : model.IsSaleable = false;
        $('.N2GActivity').prop("checked") == true ? model.N2GActivity = true : model.N2GActivity = false;
        $('.IsMrp').prop("checked") == true ? model.IsMrp = true : model.IsMrp = false;
        $('.IsPriceListReference').prop("checked") == true ? model.IsPriceListReference = true : model.IsPriceListReference = false;
        return model;
    },
    get_QC_data: function () {
        var model = {
            BotanicalName: $('#BotanicalName').val(),
            QCCategoryID: clean($('#QCCategoryID').val()),
            PatentNo: $('#PatentNo').val(),
            ConversionFactorPtoSecondary: clean($('#ConversionFactorPtoSecondary').val()),
        };
        $('.IsQCRequired').prop("checked") == true ? model.IsQCRequired = true : model.IsQCRequired = false;
        $('.IsQCRequiredForProduction').prop("checked") == true ? model.IsQCRequiredForProduction = true : model.IsQCRequiredForProduction = false;
        $('.IsProprietary').prop("checked") == true ? model.IsProprietary = true : model.IsProprietary = false;

        return model;
    },
    get_cost_data: function () {
        var model = {
            CostingCategoryID: clean($('#CostingCategoryID').val())
        };
        return model;
    },
    get_purchase_data: function () {
        var model = {
            PurchaseCategoryID: clean($('#PurchaseCategoryID').val()),
            PurchaseUnitID: clean($('#PurchaseUnitID').val()),
            ReOrderLevel: clean($('#ReOrderLevel').val()),
            MinPurchaseQTY: clean($('#MinPurchaseQTY').val()),
            MaxPurchaseQTY: clean($('#MaxPurchaseQTY').val()),
            QtyTolerancePercent: clean($('#QtyTolerancePercent').val()),
            ReOrderQty: clean($('#ReOrderQty').val()),
            SeasonPurchaseStarts: $('#SeasonPurchaseStarts').val(),
            SeasonPurchaseEnds: $('#SeasonPurchaseEnds').val(),
            PurchaseLeadTime: clean($('#PurchaseLeadTime').val()),
            ConversionFactorPurchaseToInventory: clean($('#ConversionFactorPurchaseToInventory').val()),
        };
        $('.IsPurchaseItem').prop("checked") == true ? model.IsPurchaseItem = true : model.IsPurchaseItem = false;
        $('.IsSeasonalPurchase').prop("checked") == true ? model.IsSeasonalPurchase = true : model.IsSeasonalPurchase = false;
        $('.IsPORequired').prop("checked") == true ? model.IsPORequired = true : model.IsPORequired = false;
        return model;
    },
    get_accounts_data: function () {
        var model = {
            AssetCategoryID: clean($('#AssetCategoryID').val()),
            GSTCategoryID: clean($('#GSTCategoryID').val()),
            AccountsCategoryID: clean($('#AccountsCategoryID').val()),
            GSTSubCategoryID: clean($('#GSTSubCategoryID').val())
        };
        $('.IsLocation').prop("checked") == true ? model.IsLocation = true : model.IsLocation = false;
        $('.IsInterCompany').prop("checked") == true ? model.IsInterCompany = true : model.IsInterCompany = false;
        $('.IsDepartment').prop("checked") == true ? model.IsDepartment = true : model.IsDepartment = false;
        $('.IsEmployee').prop("checked") == true ? model.IsEmployee = true : model.IsEmployee = false;
        $('.IsProject').prop("checked") == true ? model.IsProject = true : model.IsProject = false;
        $('.IsAsset').prop("checked") == true ? model.IsAsset = true : model.IsAsset = false;

        return model;
    },
    get_production_data: function () {
        var model = {
            ProductionCategoryID: clean($('#ProductionCategoryID').val()),
            MasterFormulaRefNo: clean($('#MasterFormulaRefNo').val()),
            NormalLossQty: clean($('#NormalLossQty').val()),
            NormalLossPercent: clean($('#NormalLossPercent').val()),
            ProductLeadDays: clean($('#ProductLeadDays').val()),
            BatchSizeQTY: clean($('#BatchSizeQTY').val()),
            ShelfLifeMonths: clean($('#ShelfLifeMonths').val()),
            ProductionGroup: $('#ProductionGroup').val(),
        };
        $('.IsMasterFormula').prop("checked") == true ? model.IsMasterFormula = true : model.IsMasterFormula = false;
        $('.IsReProcessAllowed').prop("checked") == true ? model.IsReProcessAllowed = true : model.IsReProcessAllowed = false;
        $('.IsBatch').prop("checked") == true ? model.IsBatch = true : model.IsBatch = false;
        return model;
    },

    LocationList: [],

    get_Location_data: function () {
        var self = ServiceItem;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Item-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Item-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },

    GetItemCode: function () {
        if ($(this).val() != 0) {
            $.ajax({
                url: '/Masters/Item/GetItemCodeByItemType',
                dataType: "json",
                type: "GET",
                data: {
                    Form: $("#ItemTypeID option:selected").text()
                },
                success: function (response) {
                    $('#Code').val(response);
                }
            });
        }
    },

    check_isdisContinued: function () {
        var self = ServiceItem;
        if ($(".IsDisContinued").prop('checked') == true) {
            $(".Isactive").prop("checked", false)
            $(".Isactive").closest('div').removeClass("checked")
        }
    },

    check_isactive: function () {
        var self = ServiceItem;
        if ($(".Isactive").prop('checked') == true) {
            $(".IsDisContinued").prop("checked", false)
            $(".IsDisContinued").closest('div').removeClass("checked")
        }
    },

    GetItemLocationList: function () {
        var self = ServiceItem;
        var ItemID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Item/GetItemLocationMapping',
            data: { ItemID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $ItemLocationDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].LocationName + "'> </div>"
                    app.format($ItemLocationDetail);
                    $('#Location-Detail-Container').append($ItemLocationDetail);
                    $('.ItemLocation').each(function () {
                        if ($(this).val() == data[i].LocationID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });
    },

    validate_form: function () {
        var self = ServiceItem;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#Name",
                rules: [
                   { type: form.required, message: "Item Name is required" },
                ]
            },
           {
               elements: "#GSTCategoryID",
               rules: [
                  { type: form.required, message: "Select a GST Category" },
               ]
           }
        ]

    }

};
