var SalesInquiryItemID = 0;
var PurchaseRequisitionTrasID = 0;
$(function () {
    const queryString = window.location.search;
    if (queryString !== "" && queryString !== undefined) {

        const searchParams = new URLSearchParams(queryString);
        SalesInquiryItemID = clean(searchParams.get('siitemid'));
        PurchaseRequisitionTrasID = clean(searchParams.get('pritemid'));
        $("#SalesInquiryItemID").val(SalesInquiryItemID);
        $("#PurchaseRequisitionTrasID").val(PurchaseRequisitionTrasID);
        $("#Code").val(searchParams.get('code'));
        $("#Name").val(searchParams.get('name'));
        Item.addItemPartsNumberOnload(searchParams.get('pno'));
        var Unit = searchParams.get('unit');
        Item.selectItemUnitFromTextOnload('UnitGroupID', Unit);
        Item.selectItemUnitFromTextOnload('InventoryUnitID', Unit);
        Item.selectItemUnitFromTextOnload('PurchaseUnitID', Unit);
        Item.selectItemUnitFromTextOnload('SalesUnitID', Unit);
        Item.selectItemUnitFromTextOnload('LooseUnitID', Unit);
        Item.selectItemUnitFromTextOnload('CostingMethodID', 'normal');
        $("#ConversionFactorPurchaseToInventory").val(1);
        $("#ConversionFactorPurchaseToLoose").val(1);
        $("#ConversionFactorSalesToInventory").val(1);
        $("#ConversionFactorLooseToSales").val(1);
    }

});
var Item = {
    init: function () {
        var self = Item;
        self.alternative_item_list();
        self.bind_events();
        self.get_currentlocation_data();

        // dropify
        var clearImage = '';

        // Dropify
        var dropifyElement = $('.dropify').dropify({
            messages: {
                default: 'Drag and drop a file here or click',
                replace: 'Drag and drop or click to replace',
                remove: 'Remove',
                error: 'Oops, something went wrong.'
            }
        });
        var dropifyObject = dropifyElement.data('dropify');

        dropifyElement.on('change', function (event) {
            var input = event.target;
            var files = input.files;
            if (files.length > 0) {
                var file = files[0];
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#ItemListImage').find("img").each(function (index, image) {
                        var altvalue = $(image).attr('alt');
                        var imageclass = $(image).attr('class');
                        if (altvalue == '') {
                            clearImage = imageclass;
                            $(image).attr('src', e.target.result).attr('alt', 'image');
                            return false; // Break the loop
                        }
                    });
                };
                reader.readAsDataURL(file);
            }
        });

        $('#btnDeleteImage').on('click', function (event) {
            app.confirm_cancel("Do you want to Remove Selected Image", function () {
                $('#ItemListImage').find("img." + clearImage).attr('src', '').attr('alt', '');
                clearImage = '';
            }, function () { });
        });

        $('#ItemListImage').on('click', 'img', function () {
            var imageSrc = $(this).attr('src');
            clearImage = $(this).attr('class');
            if (imageSrc) {
                var input = $('.dropify');
                input.val('');
                input.attr('data-default-file', imageSrc);
                input.dropify().trigger('dropify.clearElement');

                input.dropify({
                    defaultFile: imageSrc
                });

                $('.dropify-render img').attr('src', imageSrc);
                $('.dropify-preview').css('display', 'block');
            }
        });
        //dropify

    },
    bind_events: function () {
        var self = Item;
        $('#ItemTypeID').on('change', self.GetItemCode);
        $(".BtnSave").on("click", self.save_confirm);
        $(".BtnSaveAsDraft").on("click", self.save);
        $("#CategoryID").on("change", self.set_unit);
        if ($('#ID').val() > 0) {
            self.pushItemTaxArray();
            self.pushItemWareHouseArray();
            self.pushAlternativeItemArray();
            self.pushItemSalesPriceArray();
            self.pushItemPartsNumberArray();
            self.pushItemSecondaryUnitsArray();
        }
        $("#btnAddTax").on("click", self.addTax);
        $("#btnAddPartsNumber").on("click", self.addItemPartsNumber);
        $("#btnAddSecondaryUnit").on("click", self.addItemSecondaryUnit);
        $("body").on("click", ".btnDeleteTax", self.deleteTax);
        $("body").on("click", ".btnDeleteItemPartsNumber", self.deleteItemPartsNumber);
        $("body").on("click", ".btnDeleteItemSecondaryUnit", self.deleteItemSecondaryUnit);
        $('#LocationID').on('change', self.getTaxType_ddl_data);
        $('#TaxTypeID').on('change', self.getTax_ddl_data);
        $('#WareHouseID').on('change', self.getBin_ddl_data);
        $('#BinID').on('change', self.getLot_ddl_data);
        $('#InventoryUnitID').on('change', self.get_secondaryt_unit_ddl_data);
        $("#btnAddWareHouse").on("click", self.addWareHouse);
        $("body").on("click", ".btnDeleteWareHoue", self.deleteWareHouse);
        $("body").on("click", "#btnAddAlternativeItem", self.add_alternative_item_list);
        $("body").on("click", ".btnDeleteAlternativeItem", self.deleteAlternativeItem);
        $("#btnAddSalesPrice").on("click", self.addItemSalesPrice);
        $("body").on("click", ".btnDeleteItemSalesPrice", self.deleteItemSalesPrice);
        $("body").on("click", ".btnEditItemSalesPrice", self.editItemSalesPrice);

    },
    get_secondaryt_unit_ddl_data: function () {
        var self = Item;
        var UnitID = $("#InventoryUnitID").val();

        $.ajax({
            url: '/Masters/Item/GetSecondarytUnitList',
            dataType: "json",
            type: "GET",
            data: {
                UnitID: UnitID,
            },
            success: function (response) {
                $("#ItemSecondaryUnitID").html("");
                var html = '<option value="">Select</option>';
                $.each(response.Data, function (i, item) {
                    html += '<option value="' + item.ID + '">' + item.Name + '</option>';
                });
                $("#ItemSecondaryUnitID").append(html);
            },
            error: function () {
                app.show_error("Error fetching secondary unit list.");
            }
        });
    },
    detail: function () {
        var self = Item;

        // Dropify
        var dropifyElement = $('.dropify').dropify({
            messages: {
                default: '',
                replace: '',
                remove: '',
                error: ''
            }
        });
        var dropifyObject = dropifyElement.data('dropify');
        dropifyElement.on('click', function (event) {
            event.preventDefault(); // Prevent the default file dialog from opening
        });
        $('#ItemListImage').on('click', 'img', function () {
            var imageSrc = $(this).attr('src');
            clearImage = $(this).attr('class');
            if (imageSrc) {
                var input = $('.dropify');
                input.val('');
                input.attr('data-default-file', imageSrc);
                input.dropify().trigger('dropify.clearElement');

                input.dropify({
                    defaultFile: imageSrc
                });

                $('.dropify-render img').attr('src', imageSrc);
                $('.dropify-preview').css('display', 'block');
            }
        });
        //dropify

    },
    save_confirm: function () {
        var self = Item;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        });
    },
    save: function (event) {
        var self = Item;
        var IsDraft;
        var model = self.get_data()
        this.id == "BtnSaveAsDraft" ? IsDraft = true : IsDraft = false;
        model.IsDraft = IsDraft;
        var CostModel = self.get_cost_data();
        var QualityModel = self.get_quality_data();
        var UnitModel = self.get_unit_data();
        var MISEModel = self.get_mise_data();
        $.extend(model, CostModel, QualityModel, UnitModel, MISEModel);
        model.SalesInquiryItemID = clean($("#SalesInquiryItemID").val());
        model.PurchaseRequisitionTrasID = clean($("#PurchaseRequisitionTrasID").val());
        model.ItemLocationList = self.LocationList;
        model.ItemTaxList = self.ItemTaxList;
        model.ItemWareHouseList = self.ItemWareHouseList;
        model.AlternativeItemList = self.AlternativeItemList;
        model.ItemSalesPriceList = self.ItemSalesPriceList;
        model.ItemPartsNumberList = self.get_parts_number_data();
        model.ItemSecondaryUnitList = self.get_secodary_unit_data();
        $(".BtnSave").css({ 'visibility': 'hidden' });
        $(".BtnSaveAsDraft").css({ 'visibility': 'hidden' });
        $.ajax({
            url: '/Masters/Item/Save',
            data: { model },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Item created successfully");
                    setTimeout(function () {
                        if (SalesInquiryItemID == 0 && PurchaseRequisitionTrasID == 0) {
                            window.location = "/Masters/Item/IndexV4"
                        } else {
                            var ItemID = response.data.ItemID;
                            var UnitID = response.data.PurchaseUnitID;
                            var PartsNumbers = response.data.PartsNumbers;
                            var Code = response.data.Code;
                            var Name = response.data.Name;
                            var SalePrice = response.data.SalePrice;
                            var obj = { ItemID: ItemID, Code: Code, Name: Name, UnitID: UnitID, PartsNumbers: PartsNumbers, SalePrice: SalePrice };
                            window.parent.postMessage(obj, "*");
                            window.close();
                        }

                    }, 1000);
                } else {
                    app.show_error("Failed to create Item");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".BtnSaveAsDraft").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    AlternativeItemList: [],
    pushAlternativeItemArray: function () {
        var self = Item;
        $('#alternativeitem-list tbody tr').each(function () {
            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var AlternativeItemID = $(this).find("td").find("input.AlternativeItemID").val();
            var Code = $(this).find("td:eq(1)").text().trim();
            var Name = $(this).find("td:eq(2)").text().trim();
            var Category = $(this).find("td:eq(3)").text().trim();
            var alternativeItem = { "ID": ID, "ItemID": ItemID, "AlternativeItemID": AlternativeItemID, "Code": Code, "Name": Name, "Category": Category }
            self.AlternativeItemList.push(alternativeItem);
        });
    },
    ValidateAlternativeItemList: function (ItemID) {
        var self = Item;
        var arrayList = self.AlternativeItemList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].ItemID == ItemID) {
                app.show_error("Duplicate Alternative Item Exists");
                return false;
            }
        }
        return true;
    },
    deleteAlternativeItem: function () {
        var self = Item;
        var arrayList = self.AlternativeItemList;
        var ItemID = $(this).closest("tr").find("input.ItemID").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].ItemID == ItemID) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateAlternativetable();
                }
                else {
                    app.confirm_cancel("Do you want to Delete", function () {
                        $.ajax({
                            url: '/Masters/Item/deleteAlternativeItem',
                            data: { "ID": ID },
                            dataType: "json",
                            type: "POST",
                            success: function (data) {
                                arrayList.splice(i, 1);
                                self.generateAlternativetable();
                            },
                        });
                    }, function () {
                    });
                }
                break;
            }
        }
    },
    add_alternative_item_list: function () {
        var self = Item;
        var radio = $('#alternative-item-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var ItemID = clean($("#ItemID").val());
        var Code = $(row).find(".Code").val().trim();
        var Name = $(row).find(".Name").val().trim();
        var Category = $(row).find(".Category").val().trim();
        if (self.ValidateAlternativeItemList(ID)) {
            var alternativeItem = { "ID": 0, "ItemID": ItemID, "AlternativeItemID": ID, "Code": Code, "Name": Name, "Category": Category }
            self.AlternativeItemList.push(alternativeItem);
            self.generateAlternativetable();
        }
    },
    generateAlternativetable: function () {
        var self = Item;
        var arrayList = self.AlternativeItemList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="ItemID"  value="' + array.ItemID + '" />' + array.Code + '</td>'
                + '<td class="uk-text-center">' + array.Name + '</td>'
                + '<td class="uk-text-center">' + array.Category + '</td>'
                + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteAlternativeItem">Delete</button></td></tr>';
        };
        $("#alternativeitem-list tbody").html(rows);

    },
    pushItemTaxArray: function () {
        var self = Item;
        $('#itemtax-list tbody tr').each(function () {

            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var LocationID = $(this).find("td").find("input.LocationID").val();
            var Location = $(this).find("td:eq(1)").text().trim();
            var TaxTypeID = $(this).find("td").find("input.TaxTypeID").val();
            var TaxType = $(this).find("td:eq(2)").text().trim();
            var GSTCategoryID = $(this).find("td").find("input.GSTCategoryID").val();
            var GSTCategory = $(this).find("td:eq(3)").text().trim();
            var gettaxItem = { "ID": ID, "ItemID": ItemID, "Location": Location, "LocationID": LocationID, "TaxType": TaxType, "TaxTypeID": TaxTypeID, "GSTCategory": GSTCategory, "GSTCategoryID": GSTCategoryID }
            self.ItemTaxList.push(gettaxItem);
        });
    },
    ItemTaxList: [],
    getTaxType_ddl_data: function () {
        var locationID = $(this).val();
        $.ajax({
            url: '/Masters/Item/getTaxTypeData',
            data: { "LocationID": locationID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#TaxTypeID").html("<option value=''>Select</option>");
                $.each(data, function (key, value) {
                    $("#TaxTypeID").append("<option value='" + value.ID + "'>" + value.Name + "</option>");
                });
            },
        });
    },
    getTax_ddl_data: function () {
        var taxTypeID = $(this).val();
        $.ajax({
            url: '/Masters/Item/getTaxData',
            data: { "TaxTypeID": taxTypeID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#GSTCategoryID").html("<option value=''>Select</option>");
                $.each(data, function (key, value) {
                    $("#GSTCategoryID").append("<option value='" + value.ID + "'>" + value.Name + "</option>");
                });
            },
        });
    },
    deleteTax: function () {
        var self = Item;
        var arrayList = self.ItemTaxList;
        var LocationID = $(this).closest("tr").find("input.LocationID").val();
        var TaxTypeID = $(this).closest("tr").find("input.TaxTypeID").val();
        var GSTCategoryID = $(this).closest("tr").find("input.GSTCategoryID").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].LocationID == LocationID && arrayList[i].TaxTypeID == TaxTypeID && arrayList[i].GSTCategoryID == GSTCategoryID) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateTaxtable();
                }
                else {
                    app.confirm_cancel("Do you want to Delete", function () {
                        $.ajax({
                            url: '/Masters/Item/deleteItemTax',
                            data: { "ID": ID },
                            dataType: "json",
                            type: "POST",
                            success: function (data) {
                                arrayList.splice(i, 1);
                                self.generateTaxtable();
                            },
                        });
                    }, function () { });
                }
                break;
            }
        }
    },
    ValidateItemTaxList: function (LocationID, TaxTypeID) {
        var self = Item;
        var arrayList = self.ItemTaxList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].LocationID == LocationID && arrayList[i].TaxTypeID == TaxTypeID) {
                app.show_error("Duplicate Tax Exists");
                return false;
            }
        }
        return true;
    },
    addTax: function () {
        var self = Item;
        var Location = $("#LocationID option:selected").text();
        var LocationID = $("#LocationID").val();
        var ItemID = clean($("#ItemID").val());
        var TaxType = $("#TaxTypeID option:selected").text();
        var TaxTypeID = $("#TaxTypeID").val();
        var GSTCategory = $("#GSTCategoryID option:selected").text();
        var GSTCategoryID = $("#GSTCategoryID").val();
        if (!(LocationID > 0 && TaxTypeID > 0)) {
            app.show_error("All tax fields need to select");
        }
        else if (self.ValidateItemTaxList(LocationID, TaxTypeID)) {
            var taxItem = { "ID": 0, "ItemID": ItemID, "Location": Location, "LocationID": LocationID, "TaxType": TaxType, "TaxTypeID": TaxTypeID, "GSTCategory": GSTCategory, "GSTCategoryID": GSTCategoryID }
            self.ItemTaxList.push(taxItem);
            self.generateTaxtable();
        }
    },
    generateTaxtable: function () {
        var self = Item;
        var arrayList = self.ItemTaxList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="LocationID"  value="' + array.LocationID + '" />' + array.Location + '</td>'
                + '<td class="uk-text-center"><input type="hidden" class="TaxTypeID" value="' + array.TaxTypeID + '" />' + array.TaxType + '</td>'
                + '<td class="uk-text-center"><input type="hidden" class="GSTCategoryID" value="' + array.GSTCategoryID + '" />' + array.GSTCategory + '</td>'
                + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteTax">Delete</button></td></tr>';
        };
        $("#itemtax-list tbody").html(rows);

    },
    pushItemWareHouseArray: function () {
        var self = Item;
        $('#itemwarehouse-list tbody tr').each(function () {
            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var WareHouseID = $(this).find("td").find("input.WareHouseID").val();
            var WareHouse = $(this).find("td:eq(1)").text().trim();
            var BinID = $(this).find("td").find("input.BinID").val();
            var Bin = $(this).find("td:eq(2)").text().trim();
            var LotID = $(this).find("td").find("input.LotID").val();
            var Lot = $(this).find("td:eq(3)").text().trim();
            var IsDefault = $(this).find("td:eq(4)").text().trim() == "Yes" ? true : false;
            var warehousseitem = { "ID": ID, "ItemID": ItemID, "WareHouse": WareHouse, "WareHouseID": WareHouseID, "Bin": Bin, "BinID": BinID, "Lot": Lot, "LotID": LotID, "IsDefault": IsDefault }
            self.ItemWareHouseList.push(warehousseitem);
        });
    },
    ItemWareHouseList: [],
    getBin_ddl_data: function () {
        var WareHouseID = $(this).val();
        $.ajax({
            url: '/Masters/Item/getBinData',
            data: { "WareHouseID": WareHouseID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#BinID").html("<option value=''>Select</option>");
                $.each(data, function (key, value) {
                    $("#BinID").append("<option value='" + value.ID + "'>" + value.BinCode + "</option>");
                });
            },
        });
    },
    getLot_ddl_data: function () {
        var BinID = $(this).val();
        $.ajax({
            url: '/Masters/Item/getLotData',
            data: { "BinID": BinID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#LotID").html("<option value=''>Select</option>");
                $.each(data, function (key, value) {
                    $("#LotID").append("<option value='" + value.ID + "'>" + value.LotNumber + "</option>");
                });
            },
        });
    },
    deleteWareHouse: function () {
        var self = Item;
        var arrayList = self.ItemWareHouseList;
        var WareHouseID = $(this).closest("tr").find("input.WareHouseID").val();
        var BinID = $(this).closest("tr").find("input.BinID").val();
        var LotID = $(this).closest("tr").find("input.LotID").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].WareHouseID == WareHouseID && arrayList[i].BinID == BinID && arrayList[i].LotID == LotID) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateWareHousetable();
                }
                else {
                    app.confirm_cancel("Do you want to Delete", function () {
                        $.ajax({
                            url: '/Masters/Item/deleteItemWareHouse',
                            data: { "ID": ID },
                            dataType: "json",
                            type: "POST",
                            success: function (data) {
                                arrayList.splice(i, 1);
                                self.generateWareHousetable();
                            },
                        });
                    }, function () { });
                }
                break;
            }
        }
    },
    ValidateWareHouseList: function (WareHouseID, BinID, LotID) {
        var self = Item;
        var arrayList = self.ItemWareHouseList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].WareHouseID == WareHouseID && arrayList[i].BinID == BinID && arrayList[i].LotID == LotID) {
                app.show_error("Duplicate WareHouse Exists");
                return false;
            }
        }
        return true;
    },
    addWareHouse: function () {
        var self = Item;
        var WareHouse = $("#WareHouseID option:selected").text();
        var WareHouseID = $("#WareHouseID").val();
        var ItemID = clean($("#ItemID").val());
        var Bin = $("#BinID option:selected").text();
        var BinID = $("#BinID").val();
        var Lot = $("#LotID option:selected").text();
        var LotID = $("#LotID").val();
        var IsDefault = $("#IsDefault").is(":checked");
        if (!(WareHouseID > 0 && BinID > 0 && LotID > 0)) {
            app.show_error("All Ware House fields need to select");
        }
        else if (self.ValidateWareHouseList(WareHouseID, BinID, LotID)) {
            var warehousseitem = { "ID": 0, "ItemID": ItemID, "WareHouse": WareHouse, "WareHouseID": WareHouseID, "Bin": Bin, "BinID": BinID, "Lot": Lot, "LotID": LotID, "IsDefault": IsDefault }
            self.ItemWareHouseList.push(warehousseitem);
            self.generateWareHousetable();
        }
    },
    generateWareHousetable: function () {
        var self = Item;
        var arrayList = self.ItemWareHouseList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            var YesOrNo = array.IsDefault ? "Yes" : "";
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="WareHouseID"  value="' + array.WareHouseID + '" />' + array.WareHouse + '</td>'
                + '<td class="uk-text-center"><input type="hidden" class="BinID" value="' + array.BinID + '" />' + array.Bin + '</td>'
                + '<td class="uk-text-center"><input type="hidden" class="LotID" value="' + array.LotID + '" />' + array.Lot + '</td>'
                + '<td class="uk-text-center"><input type="hidden" class="IsDefault" value="' + array.IsDefault + '" />' + YesOrNo + '</td>'
                + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteWareHoue">Delete</button></td></tr>';
        };
        $("#itemwarehouse-list tbody").html(rows);

    },
    ItemSalesPriceList: [],
    ValidateItemSalesPriceList: function (LocationID) {
        var self = Item;
        var arrayList = self.ItemSalesPriceList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].LocationID == LocationID) {
                app.show_error("Duplicate WareHouse Exists");
                return false;
            }
        }
        return true;
    },
    resetItemSalesPrice: function () {
        $("#SalesPrice").val(0);
        $("#LoosePrice").val(0);
    },
    addItemSalesPrice: function () {
        var self = Item;
        var ItemID = clean($("#ItemID").val());
        var Location = $("#PriceLocationID option:selected").text().trim();
        var LocationID = $("#PriceLocationID").val();
        var SalesPrice = $("#SalesPrice").val();
        var LoosePrice = $("#LoosePrice").val();
        if (!LocationID > 0) {
            app.show_error("Select Sales Price Details");
        }
        else if (self.ValidateItemSalesPriceList(LocationID)) {
            var itemsalesprice = { "ID": 0, "ItemID": ItemID, "Location": Location, "LocationID": LocationID, "SalesPrice": SalesPrice, "LoosePrice": LoosePrice, "Edited": 0 };
            self.ItemSalesPriceList.push(itemsalesprice);
            self.resetItemSalesPrice();
            self.generateItemSalesPricetable();
        }
    },
    generateItemSalesPricetable: function () {
        var self = Item;
        var arrayList = self.ItemSalesPriceList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="LocationID"  value="' + array.LocationID + '" />' + array.Location + '</td>'
                + '<td class="uk-text-center"><input type="text" class="SalesPrice md-input" value="' + array.SalesPrice + '" disabled = "disabled"/></td>'
                + '<td class="uk-text-center"><input type="text" class="LoosePrice md-input" value="' + array.LoosePrice + '" disabled = "disabled"/></td>';
            if (array.ID > 0)
                rows = rows + '<td class="uk-text-center"><button class="md-btn md-btn-primary btnEditItemSalesPrice">Edit</button></td></tr>';
            else
                rows = rows + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteItemSalesPrice">Delete</button></td></tr>';
        };
        $("#saleitemsprice-list tbody").html(rows);

    },
    editItemSalesPrice: function () {
        var self = Item;
        var $btn = $(this);
        if ($btn.text().trim() == "Edit") {
            $btn.text("Update");
            $btn.closest("tr").find(".SalesPrice").removeAttr("disabled");
            $btn.closest("tr").find(".LoosePrice").removeAttr("disabled");
        }
        else {
            var arrayList = self.ItemSalesPriceList;
            var ID = clean($(this).closest("tr").find(".ID").val());
            var ItemID = clean($("#ItemID").val());
            var Location = $(this).closest("tr").find("td:eq(1)").text().trim();
            var LocationID = $(this).closest("tr").find(".LocationID").val();
            var SalesPrice = $(this).closest("tr").find(".SalesPrice").val();
            var LoosePrice = $(this).closest("tr").find(".LoosePrice").val();

            for (let i = 0; i < arrayList.length; i++) {
                if (ID > 0 && arrayList[i].ID == ID) {
                    var itemsalesprice = { "ID": ID, "ItemID": ItemID, "Location": Location, "LocationID": LocationID, "SalesPrice": SalesPrice, "LoosePrice": LoosePrice, "Edited": 1 };
                    arrayList.splice(i, 1, itemsalesprice);
                }
            }
            self.generateItemSalesPricetable();
        }

    }
    ,
    deleteItemSalesPrice: function () {
        var self = Item;
        var arrayList = self.ItemSalesPriceList;
        var LocationID = $(this).closest("tr").find("input.LocationID").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].LocationID == LocationID) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateItemSalesPricetable();
                }
                break;
            }
        }
    },
    pushItemSalesPriceArray: function () {
        var self = Item;
        $('#saleitemsprice-list tbody tr').each(function () {
            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var LocationID = $(this).find("td").find("input.LocationID").val();
            var Location = $(this).find("td:eq(1)").text().trim();
            var SalesPrice = $(this).find("td").find("input.SalesPrice").val();
            var LoosePrice = $(this).find("td").find("input.LoosePrice").val();
            var itemsalesprice = { "ID": ID, "ItemID": ItemID, "Location": Location, "LocationID": LocationID, "SalesPrice": SalesPrice, "LoosePrice": LoosePrice, "Edited": 0 };
            self.ItemSalesPriceList.push(itemsalesprice);
        });
    },
    // parts no
    ItemPartsNumberList: [],
    ValidateItemPartsNumberList: function (PartsNumber) {
        var self = Item;
        var arrayList = self.ItemPartsNumberList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].PartsNumber == PartsNumber) {
                app.show_error("Duplicate PartsNumber Exists");
                return false;
            }
        }
        return true;
    },

    ItemSecondaryUnitList: [],
    ValidateItemSecondaryUnitList: function (SecondaryUnitID) {
        var self = Item;
        var arrayList = self.ItemSecondaryUnitList;
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].SecondaryUnitID == SecondaryUnitID) {
                app.show_error("Duplicate Secondary Unit Exists");
                return false;
            }
        }
        return true;
    },
    selectItemUnitFromTextOnload: function (SelectID, Unit) {
        var desiredText = Unit.toLowerCase();
        var $option = $('#' + SelectID + ' option').filter(function () {
            return $(this).text().toLowerCase() === desiredText;
        });
        $option.prop("selected", true);
    },
    addItemSecondaryUnit: function () {
        var self = Item;
        var ItemID = 0;
        var SecondaryUnitID = clean($("#ItemSecondaryUnitID").val());
        var SecondaryUnit = $("#ItemSecondaryUnitID option:selected").text().trim();
        if (SecondaryUnitID == 0) {
            app.show_error("Enter Secondary Unit");
        }
        else if (self.ValidateItemSecondaryUnitList(SecondaryUnitID)) {
            $("#ItemSecondaryUnitID").val('');
            var itemSecondaryUnit = { "ID": 0, "ItemID": ItemID, "SecondaryUnitID": SecondaryUnitID, "SecondaryUnit": SecondaryUnit }
            self.ItemSecondaryUnitList.push(itemSecondaryUnit);
            self.generateSecondaryUnittable();
        }
    },
    addItemPartsNumberOnload: function (PartsNumber) {
        var self = Item;
        var ItemID = 0;
        if (PartsNumber.length > 0) {
            var itempartsnumber = { "ID": 0, "ItemID": ItemID, "PartsNumber": PartsNumber }
            self.ItemPartsNumberList.push(itempartsnumber);
            self.generateItemPartsNumbertable();
        }
    },
    addItemPartsNumber: function () {
        var self = Item;
        var ItemID = clean($("#ItemID").val());
        var PartsNumber = $("#PartsNumbers").val();
        if (PartsNumber.length == 0) {
            app.show_error("Enter PartsNumber");
        }
        else if (self.ValidateItemPartsNumberList(PartsNumber, IsDefault)) {
            $("#PartsNumbers").val('');
            $("#IsDefault").prop('checked', false);
            var itempartsnumber = { "ID": 0, "ItemID": ItemID, "PartsNumber": PartsNumber, "IsDefault": false }
            self.ItemPartsNumberList.push(itempartsnumber);
            self.generateItemPartsNumbertable();
        }
    },
    generateSecondaryUnittable: function () {
        var self = Item;
        var arrayList = self.ItemSecondaryUnitList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="SecondaryUnitID"  value="' + array.SecondaryUnitID + '" />' + array.SecondaryUnit + '</td>'
                + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteItemSecondaryUnit">Delete</button></td></tr>';
        };
        $("#itemsecondaryunit-list tbody").html(rows);

    },
    generateItemPartsNumbertable: function () {
        var self = Item;
        var arrayList = self.ItemPartsNumberList;
        var rows = '';
        for (let i = 0; i < arrayList.length; i++) {
            var array = arrayList[i];
            var slNo = Number(i) + 1;
            rows = rows + '<tr><td class="uk-text-center"><input type="hidden" class="ID"  value="' + array.ID + '" />' + slNo + '<input type="hidden" class="ItemID"  value="' + array.ItemID + '" /></td>'
                + '<td class="uk-text-center"><input type="hidden" class="PartsNumber"  value="' + array.PartsNumber + '" />' + array.PartsNumber + '</td>'
                + '<td class="uk-text-center"><input type="radio" class="IsDefault" name="IsDefault" /></td>'
                + '<td class="uk-text-center"><button class="md-btn md-btn-danger btnDeleteItemPartsNumber">Delete</button></td></tr>';
        };
        $("#itempartsno-list tbody").html(rows);

    },
    deleteItemPartsNumber: function () {
        var self = Item;
        var arrayList = self.ItemPartsNumberList;
        var PartsNumber = $(this).closest("tr").find("input.PartsNumber").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        var ItemID = $(this).closest("tr").find("input.ItemID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].PartsNumber == PartsNumber) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateItemPartsNumbertable();
                } else {
                    app.confirm_cancel("Do you want to Delete", function () {
                        $.ajax({
                            url: '/Masters/Item/deleteItemParts',
                            data: { "ID": ID, "ItemID": ItemID },
                            dataType: "json",
                            type: "POST",
                            success: function (data) {
                                arrayList.splice(i, 1);
                                self.generateItemPartsNumbertable();
                            },
                        });
                    }, function () { });
                }
                break;
            }
        }
    },
    deleteItemSecondaryUnit: function () {
        var self = Item;
        var arrayList = self.ItemSecondaryUnitList;
        var SecondryUnitID = $(this).closest("tr").find("input.SecondryUnitID").val();
        var ID = $(this).closest("tr").find("input.ID").val();
        for (let i = 0; i < arrayList.length; i++) {
            if (arrayList[i].SecondryUnitID == SecondryUnitID) {
                if (ID == 0) {
                    arrayList.splice(i, 1);
                    self.generateSecondaryUnittable();
                } else {
                    app.confirm_cancel("Do you want to Delete", function () {
                        $.ajax({
                            url: '/Masters/Item/deleteSecondaryUnitItem',
                            data: { "ID": ID },
                            dataType: "json",
                            type: "POST",
                            success: function (data) {
                                arrayList.splice(i, 1);
                                self.generateSecondaryUnittable();
                            },
                        });
                    }, function () { });
                }
                break;
            }
        }
    },
    pushItemSecondaryUnitsArray: function () {
        var self = Item;
        $('#itemsecondaryunit-list tbody tr').each(function () {
            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var SecondaryUnitID = $(this).find("td").find("input.SecondaryUnitID").val();
            var SecondaryUnit = $(this).find("td").find("input.SecondaryUnit").val();
            var itemSecondaryUnit = { "ID": ID, "ItemID": ItemID, "SecondaryUnitID": SecondaryUnitID, "SecondaryUnit": SecondaryUnit }
            self.ItemSecondaryUnitList.push(itemSecondaryUnit);
        });
    },
    pushItemPartsNumberArray: function () {
        var self = Item;
        $('#itempartsno-list tbody tr').each(function () {
            var ID = $(this).find("td").find("input.ID").val();
            var ItemID = $(this).find("td").find("input.ItemID").val();
            var PartsNumber = $(this).find("td").find("input.PartsNumber").val();
            var itempartsnumber = { "ID": ID, "ItemID": ItemID, "PartsNumber": PartsNumber }
            self.ItemPartsNumberList.push(itempartsnumber);
        });
    },
    // parts no
    get_data: function () {

        var ItemImages = [];
        $('#ItemListImage').find('img').each(function () {
            ItemImages.push($(this).attr('src'));
        });
        var model = {
            ID: clean($('#ID').val()),
            Code: $('#Code').val(),
            Name: $('#Name').val(),
            CategoryID: $("#CategoryID option:selected").val(),
            CategoryName: $("#CategoryID option:selected").text(),
            SanskritName: $('#SanskritName').val(),//Remarks 
            Description: $('#Description').val(),//PartsNo
            PurchaseCategoryID: clean($('#PurchaseCategoryID').val()),//PartsClass- 
            BusinessCategoryID: clean($('#BusinessCategoryID').val()),//PartsGroup
            UnitGroupID: clean($('#UnitGroupID').val()),
            SalesUnitID: clean($('#SalesUnitID').val()),
            PurchaseUnitID: clean($('#PurchaseUnitID').val()),
            InventoryUnitID: clean($('#InventoryUnitID').val()),
            LooseUnitID: clean($('#LooseUnitID').val()),
            ConversionFactorPurchaseToInventory: clean($('#ConversionFactorPurchaseToInventory').val()),
            ConversionFactorPurchaseToLoose: clean($('#ConversionFactorPurchaseToLoose').val()),
            ConversionFactorSalesToInventory: clean($('#ConversionFactorSalesToInventory').val()),
            ConversionFactorLooseToSales: clean($('#ConversionFactorLooseToSales').val()),
            IsStockItem: $('#IsStockValue').is(":checked"),
            IsStockValue: $('#IsStockValue').is(":checked"),
            IsStockValue: $('#IsStockValue').is(":checked"),
            MalayalamName: $('#MalayalamName').val(),//Regional language
            Isactive: $('#Isactive').is(":checked"),
            ItemImages: ItemImages
        };
        return model;
    },
    get_secodary_unit_data: function () {
        var unitmodel = [];
        $('#itemsecondaryunit-list tbody tr').each(function () {
            var item = {
                SecondaryUnit: $(this).find('td.SecondaryUnit').text().trim(),
                ID: $(this).find('.ID').val(),
                ItemID: $(this).find('.ItemID').val(),
                SecondaryUnitID: $(this).find('.SecondaryUnitID').val(),
            };

            unitmodel.push(item);
        });
        return unitmodel;
    },
    get_parts_number_data: function () {
        var partsmodel = [];
        $('#itempartsno-list tbody tr').each(function () {
            var item = {
                ID: $(this).find('.ID').val(),
                ItemID: $(this).find('.ItemID').val(),
                IsDefault: $(this).find('.IsDefault').is(':checked'),
                PartsNumber: $(this).find('.PartsNumber').val(),
            };

            partsmodel.push(item);
        });
        return partsmodel;
    },
    get_cost_data: function () {
        var model = {
            CostPrice: clean($('#CostPrice').val()),
            PurchasePrice: clean($('#PurchasePrice').val()),
            LandedCost: clean($('#LandedCost').val()),
            CostingMethodID: clean($('#CostingMethodID').val()),
            DiscountPercentage: clean($('#DiscountPercentage').val()),
            DisplayPercentage: clean($('#DisplayPercentage').val()),
            BrandID: clean($('#BrandID').val()),
            cross_reference: $('#cross_reference').val(),
        };
        return model;
    },
    get_quality_data: function () {
        var model = {
            ReOrderLevelName: $('#ReOrderLevelName').val(),
            ReOrderLevel: clean($('#ReOrderLevel').val()),
            ReOrderQty: clean($('#ReOrderQty').val())
        };
        return model;
    },
    get_unit_data: function () {
        var model = {
            ItemLength: clean($('#ItemLength').val()),
            LengthUOMID: clean($('#LengthUOMID').val()),
            ItemWidth: clean($('#ItemWidth').val()),
            WidthUOMID: clean($('#WidthUOMID').val()),
            ItemHight: clean($('#ItemHight').val()),
            HightUOMID: clean($('#HightUOMID').val()),
            NetWeight: clean($('#NetWeight').val()),
            NetWeightUOMID: clean($('#NetWeightUOMID').val()),
            InnerDiameter: clean($('#InnerDiameter').val()),
            OuterDiameter: clean($('#OuterDiameter').val())
        };
        return model;
    },
    get_mise_data: function () {
        var model = {
            BuyerID: clean($('#BuyerID').val()),
            SupplierPartCode: $('#SupplierPartCode').val(),
            OEMCode: $('#OEMCode').val(),
            OEMCountryID: clean($('#OEMCountryID').val()),
            ABCCodeID: clean($('#ABCCodeID').val()),
            ABCCode: $("#ABCCodeID option:selected").text(),
            HSNCode: $('#HSNCode').val(),
            EANCode: $('#EANCode').val(),
            BarCode: $('#BarCode').val(),
            SupplierID: clean($('#SupplierID').val()),
            BudgetQunatity: clean($('#BudgetQunatity').val()),
            Make: $('#Make').val(),
            Model: $('#Model').val()
        };
        return model;
    },
    alternative_item_list: function () {
        var $list = $('#alternative-item-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAlternativeItemList";
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
                            { Key: "ID", Value: $('#ID').val() },
                            { Key: "Code", Value: $('#Code').val() },
                            { Key: "Name", Value: $('#Name').val() },
                            { Key: "Category", Value: $('#Category').val() },
                            { Key: "Unit", Value: $('#Unit').val() }
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
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID'  value='" + row.ID + "' >" +
                                "<input type='hidden' class='Code' value='" + row.Code + "'>" +
                                "<input type='hidden' class='Name' value='" + row.Name + "'>" +
                                "<input type='hidden' class='Category' value='" + row.Category + "'>" +
                                "<input type='hidden' class='Unit' value='" + row.Unit + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Category", "className": "Category" },
                    { "data": "Unit", "className": "Unit" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
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
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    list: function () {
        var $list = $('#tbl-item-list');
        $list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/Item/DetailsV4/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Item/GetAllItemList";
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
                            { Key: "Code", Value: $('#Code').val() },
                            { Key: "CategoryName", Value: $('#CategoryName').val() },
                            { Key: "Description", Value: $('#Description').val() },
                            { Key: "PartsNo", Value: $('#PartsNo').val() },
                            { Key: "PartsClass", Value: $('#PartsClass').val() },
                            { Key: "PartsGroup", Value: $('#PartsGroup').val() },
                            { Key: "Remark", Value: $('#Remark').val() },
                            { Key: "Model", Value: $('#Model').val() },
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
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "Description", "className": "Description" },
                    { "data": "PartsNo", "className": "PartsNo", "visible": false },
                    { "data": "PartsClass", "className": "PartsClass", "visible": false },
                    { "data": "PartsGroup", "className": "PartsGroup", "visible": false },
                    { "data": "Remark", "className": "Remark" },
                    { "data": "Model", "className": "Model", "visible": false },
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
    LocationList: [],
    get_currentlocation_data: function () {
        var self = Item;
        CurrentLocationID = $("#current-location select").val();
        var location = {
            LocationID: CurrentLocationID
        };
        self.LocationList.push(location);
    },
    validate_form: function () {
        var self = Item;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#ItemCategoryID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: "Item Category is required"
                    },
                ]
            },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Description is required" },
                ]
            },
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Code is required" },
                ]
            },

            {
                elements: "#PurchaseCategoryID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: 'Parts Class is required'
                    }
                ]
            },
            {
                elements: "#BusinessCategoryID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: 'Parts Group is required'
                    }
                ]
            },
            {
                elements: "#UnitGroupID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: 'Unit Group is required'
                    }
                ]
            },
            {
                elements: "#InventoryUnitID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: 'Inventory Unit is required'
                    }
                ]
            },
            //{
            //    elements: "#itemtax-list",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var count = $(element).find("tbody tr").length;
            //                    if (count > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'item tax is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#PurchaseUnitID",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = $(element).val();
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Purchase Unit is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#SalesUnitID",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = $(element).val();
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Sales Unit is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#LooseUnitID",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = $(element).val();
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Loose Unit is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#ConversionFactorPurchaseToInventory",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = parseFloat($(element).val());
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Conversion Purchase To Inventory is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#ConversionFactorPurchaseToLoose",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = parseFloat($(element).val());
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Conversion Purchase To Loose is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#ConversionFactorSalesToInventory",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = parseFloat($(element).val());
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Conversion Sales To Inventory is required'
            //        }
            //    ]
            //},
            //{
            //    elements: "#ConversionFactorLooseToSales",
            //    rules: [
            //        {
            //            type: function (element) {
            //                {
            //                    var value = parseFloat($(element).val());
            //                    if (value > 0)
            //                        return true;
            //                    else
            //                        return false;
            //                }
            //            }, message: 'Conversion Loose To Sales is required'
            //        }
            //    ]
            //},
            {
                elements: "#saleitemsprice-list",
                rules: [
                    {
                        type: function (element) {
                            {
                                if ($(element).find("tbody tr").length == 0)
                                    return false;
                                else
                                    return true;
                            }
                        }, message: 'Sales Price is required'
                    }
                ]
            },
            {
                elements: "#CostingMethodID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                if (value > 0)
                                    return true;
                                else
                                    return false;
                            }
                        }, message: 'Costing Method is required'
                    }
                ]
            },
            {
                elements: "#LengthUOMID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                var value2 = $('#ItemLength').val()
                                if (value < 0 && value2 > 0)
                                    return false;
                                else
                                    return true;
                            }
                        }, message: 'If Item Length is entered, its unit is required'
                    }
                ]
            },
            {
                elements: "#WidthUOMID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                var value2 = $('#ItemWidth').val()
                                if (value < 0 && value2 > 0)
                                    return false;
                                else
                                    return true;
                            }
                        }, message: 'If Item Width is entered, its unit is required'
                    }
                ]
            },
            {
                elements: "#HightUOMID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                var value2 = $('#ItemHight').val()
                                if (value < 0 && value2 > 0)
                                    return false;
                                else
                                    return true;
                            }
                        }, message: 'If Item Hight is entered, its unit is required'
                    }
                ]
            },
            {
                elements: "#NetWeightUOMID",
                rules: [
                    {
                        type: function (element) {
                            {
                                var value = $(element).val();
                                var value2 = $('#NetWeight').val()
                                if (value < 0 && value2 > 0)
                                    return false;
                                else
                                    return true;
                            }
                        }, message: 'If Net Weight is entered, its unit is required'
                    }
                ]
            },
        ]
    }
}
