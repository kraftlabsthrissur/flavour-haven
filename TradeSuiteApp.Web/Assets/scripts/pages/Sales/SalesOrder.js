var fh_items;
var item_list;
SalesOrder = {
    // Initializes the page
    init: function () {
        var self = SalesOrder;
        Customer.customer_list('sales-order');
        item_list = Item.item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        if ($("#CustomerID").val() != 0) {
            var ItemIDS = [];
            var UnitIDS = [];
            $("#sales-order-items-list tbody tr").each(function () {
                ItemIDS.push($(this).find('.ItemID').val());
                UnitIDS.push($(this).find('.UnitID').val());
            });
            self.get_offer_details_bulk(ItemIDS, UnitIDS);
        }
        self.on_change_customer_category();
        self.bind_events();
        self.freeze_headers();
        $("#Offers").hide();
    },

    details: function () {
        var self = SalesOrder;
        self.freeze_headers();
        $("body").on('click', ".cancel", self.cancel_confirm);
        $(".btnApprove").on("click", self.open_approval_details);
        $("#btnOkApprove").on('click', self.approve);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".Exportpdf", self.Exportpdf);


        $("body").on("click", ".ItemCode", self.SalesOrderWithItemCode);
        $("body").on("click", ".PartNo", self.WithPartNo);
        $("body").on("click", ".ExportItemCode", self.SalesOrderExportWithItemCode);
        $("body").on("click", ".ExportPartNo", self.SalesOrderExportPartNo);
   

    },
    printpdf: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderPrintPdf',
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

    SalesOrderdetailpage: function () {
        var self = SalesOrder;
        var id = $(this).parents('tr').find('.ID').val();
        $.ajax({
            url: '/Reports/Sales/SalesOrderdetailpage',
            data: {
                Id: id,
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

    SalesOrderExportDetial: function () {
        var self = SalesOrder;
        var id = $(this).parents('tr').find('.ID').val();
        $.ajax({
            url: '/Reports/Sales/SalesOrderExportDetial',
            data: {
                Id: id,
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




    Exportpdf : function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderExportPrintPdf',
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



    SalesOrderWithItemCode: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderWithItemCode',
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



    WithPartNo: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/WithPartNo',
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

    SalesOrderExportWithItemCode: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderExportWithItemCode',
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

    SalesOrderExportPartNo: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderExportPartNo',
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





    SalesOrderExportPrintPdf: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Reports/Sales/SalesOrderExportPrintPdf',
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
    change_customer_category: function () {
        var self = SalesOrder;
        var customerid = clean($('#CustomerID').val());
        if (customerid > 0) {
            app.confirm("Selected customer will be removed", function () {
                $('#CustomerID').val(0);
                $('#CustomerName').val('');
                self.on_change_customer_category();
                $("#BillingAddressID").val('');
                $("#ShippingAddressID").val('');
                $("#GrossAmount").val('');

                $("#DiscountAmount").val('');
                $("#TaxableAmount").val('');
                $("#SGSTAmount").val('');
                $("#CGSTAmount").val('');
                $("#IGSTAmount").val('');
                $("#CessAmount").val('');
                $("#RoundOff").val('');
                $("#item-count").val($("#sales-order-items-list tbody tr").length);

                if ($("#IsClone").val() != "True") {
                    self.clear_grid_offer();
                }
            })
        }
        else {
            self.on_change_customer_category();
        }
    },
    getCurrencyDetails: function () {
        var currency = {
            CurrencyID: $("#CurrencyID").val(),
            CurrencyName: $("#CurrencyName").val(),
            ExchangeRate: parseFloat($("#CurrencyExchangeRate").val()).toFixed(4),
            IsGST: $("#IsGST").val(),
            IsVat: $("#IsVat").val(),
            TaxTypeID: $("#TaxTypeID").val(),
            CountryID: $("#CountryID").val()
        };
        return currency;
    }
    ,
    on_change_customer_category: function () {
        var self = SalesOrder;
        var CustomerCategory = $("#CustomerCategoryID option:selected").text();
        if (CustomerCategory == 'ECOMMERCE') {
            $(".ecommerce-hide").removeClass("uk-hidden");
        }
        else {
            $(".ecommerce-hide").addClass("uk-hidden");
            $("#FreightAmount").val(0);
        }
    },

    freeze_headers: function () {
        fh_items = $("#sales-order-items-list").FreezeHeader();
    },

    //Bind the events to elements  
    bind_events: function () {
        var self = SalesOrder

        $('body').on("click", '#sales-order-list tbody tr', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Sales/SalesOrder/Details/' + id;
        });

        $('#Qty').keydown(function (e) {
            if (e.which == 13) {
                self.add_item();
            }
        });

        //Bind auto complete event for item 
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);

        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);

        $("#ItemCategoryID").on("change", self.get_sales_category);
        $("body").on("keyup change", "#FreightAmount", self.calculate_grid_total);
        //$(").on("click", self.select_item);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKCustomer").on("click", self.select_customer);
        $("#btnAddItem").on("click", self.confirm_add_item);
        $(".btnSave").on("click", self.save_confirm);
        $(".ItemCode").on("click", self.SalesOrderWithItemCode);
        $(".PartNo").on("click", self.WithPartNo);
        $(".ExportItemCode").on("click", self.SalesOrderExportWithItemCode);
        $(".ExportPartNo").on("click", self.SalesOrderExportPartNo);
       

        $(".btnSaveASDraft").on("click", self.save_draft);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("keyup change", "#sales-order-items-list tbody .Qty, .MRP, .DiscountPercentage, .DiscountAmount", self.on_qty_change);
        $("body").on("keyup change", "#sales-order-items-list tbody tr .secondary .secondaryQty, .secondaryUnit, .secondaryMRP", self.on_secondary_unit_change);
        $("body").on("change", "#CustomerCategoryID", self.change_customer_category);
        $("#DiscPercentage, #DiscAmount").on("keyup", self.on_change_discount_Amount);
        $("body").on("change", "#VATPercentageID", self.on_change_discount_Amount);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
        $("body").on("click", "#btnOkUplodedOrders", self.process_uploaded_orders);
        $("body").on('click', ".cancel", self.cancel_confirm);
        $("body").on('click', ".btnsuspend", self.so_suspend);
        $("body").on('click', ".btnclone", self.so_clone);
        $("body").on('click', ".btnprint", self.SalesOrderdetailpage);
        $("body").on('click', ".btnexport", self.SalesOrderExportDetial);
        $('body').on('click', '#sales-order-items-list tbody tr td.showitemhistory', self.showitemhistory);
    },
    on_qty_change: function () {
        var self = SalesOrder;
        var row = $(this).closest('tr');
        self.update_item(row);
    },
    save_confirm: function () {
        var self = SalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            IsDraft = false;
            self.save();
        }, function () {
        })
    },

    save_draft: function () {
        var self = SalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_draft();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveASDraft")) {
            IsDraft = true;
            self.save();
        }

    },

    so_suspend: function () {
        //TODO
    },

    so_clone: function (e) {
        e.stopPropagation();
        var self = SalesOrder;
        var id = $(this).parents('tr').find('.ID').val();
        window.location = '/Sales/SalesOrder/Clone/' + id;
    },



    //print: function () {
    //    var self = SalesOrder;
    //    var is_preview_needed = false;
    //    if ($(this).hasClass('print-preview-sales-order')) {
    //        is_preview_needed = true;
    //    }
    //    $.ajax({
    //        url: '/Reports/Sales/PickList',
    //        data: {
    //            SalesOrderID: $("#ID").val(),
    //        },
    //        dataType: "json",
    //        type: "POST",
    //        success: function (data) {
    //            if (data.Status == "success") {
    //                app.print(data.URL, is_preview_needed);
    //            } else {
    //                app.show_error("Failed to print");
    //            }
    //        },
    //    });
    //},

    print: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Sales/SalesOrder/Print',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    SignalRClient.print.text_file(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },

    open_approval_details: function () {
        var self = SalesOrder;
        var color;
        //color = $(this).find(".color").val();
        //document.getElementById("#Name").style.color = "Green";
        $('#show-approval').trigger('click');
    },

    cancel_confirm: function () {
        var self = SalesOrder
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        $.ajax({
            url: '/Sales/SalesOrder/Cancel',
            data: {
                SalesOrderID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Sales Order cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/SalesOrder/Index";
                    }, 1000);
                }
                else if (data.Status == "fail") {
                    app.show_error("Please cancel Proforma invoice or sales invoice");
                }
                else {
                    app.show_error("Failed to cancel.");
                }
            },
        });
    },
    //update item on customer change of clone

    update_clone_item: function () {
        var self = SalesOrder;

        $("#sales-order-items-list tbody tr").each(function () {
            var row = $(this).closest("tr");
            self.update_item(row);
        });
    },


    // updates item details on changing qty 
    update_item: function (row) {
        var self = SalesOrder;

        var item = {};

        item.Category = $(row).find(".Category").val();

        var batchtype = $("#BatchType").val();
        if (item.Category == "Finished Goods") {
            item.BatchType = batchtype;
        }
        else {
            item.BatchType = "";
        }
        item.Qty = clean($(row).find(".Qty").val());
        item.MRP = clean($(row).find(".MRP").val());
        item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
        item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.CGSTPercentage = clean($(row).find(".CGSTPercentage").val());
        item.VATPercentage = clean($(row).find(".VATPercentage ").val());
        item.ID = clean($(row).find(".ItemID").val());
        item.UnitID = clean($(row).find(".UnitID").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.CessPercentage = clean($("#CessPercentage").val());
        item = self.get_item_properties(item);

        $(row).find(".OfferQty").val(item.OfferQty);
        $(row).find(".GrossAmount").val(item.GrossAmount);
        $(row).find(".DiscountAmount").val(item.DiscountAmount);
        $(row).find(".AdditionalDiscount ").val(item.AdditionalDiscount);
        $(row).find(".TaxableAmount").val(item.TaxableAmount);
        $(row).find(".CGST").val(item.CGST);
        $(row).find(".SGST").val(item.SGST);
        $(row).find(".IGST").val(item.IGST);
        $(row).find(".GSTAmount").val(item.GSTAmount);
        $(row).find(".VATAmount").val(item.VATAmount);
        $(row).find(".CessAmount").val(item.CessAmount);
        $(row).find(".NetAmount").val(item.NetAmount);
        $(row).find(".DiscountPercentage").val(item.DiscountPercentage);
        $(row).find(".BatchType").text(item.BatchType);
        $(row).find(".BasicPrice").val(item.BasicPrice);
        self.calculate_grid_total();
    },
    // removes item from the grid on clicking remove button
    remove_item: function () {
        var self = SalesOrder;
        $(this).closest("tr").remove();
        $("#sales-order-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(1).html(i + 1);
        });
        $("#item-count").val($("#sales-order-items-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader(false);
    },

    // gets the discount and offer details on selecting an item
    get_discount_and_offer_details: function () {
        var self = SalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        var CustomerID = $("#CustomerID").val();
        var SchemeID = $("#SchemeID").val();
        var ItemID = $("#ItemID").val();
        var UnitID = $("#UnitID option:selected").val();
        $.ajax({
            url: '/Sales/SalesOrder/GetDiscountAndOfferDetails/',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
                SchemeID: SchemeID,
                ItemID: ItemID,
                Qty: 0,
                UnitID: UnitID
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#DiscountPercentage").val(response.discountAndOffer.DiscountPercentage);
                    if (self.search_in_array(self.item_offer_details, ItemID) == -1) {
                        var obj = { ItemID: ItemID, UnitID: response.discountAndOffer.UnitID, OfferDetails: response.discountAndOffer.OfferDetails, DiscountPercentage: response.discountAndOffer.DiscountPercentage }
                        self.item_offer_details.push(obj);
                    }
                    var title = '';
                    var a = '';
                    var j = self.search_in_array(self.item_offer_details, ItemID);
                    self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
                        a += ' ' + record.Qty + ' + ' + record.OfferQty + ','
                    });
                    title = a.replace(/,(\s+)?$/, '');
                    if (title != '') {
                        $("#Offers").show();
                        $("#Offers").val("Scheme: " + title);
                        setTimeout(function () { $("#Offers").hide(); }, 10000);
                    }
                    else {
                        $("#Offers").hide();
                    }


                }
            }
        });

    },

    // gets the discount and offer details of multiple items
    get_offer_details_bulk: function (ItemIDs, UnitIDs, callback) {
        var self = SalesOrder;

        var CustomerID = $("#CustomerID").val();
        var SchemeID = $("#SchemeID").val();
        SalesOrder.item_offer_details = [];
        $.ajax({
            url: '/Sales/SalesOrder/GetOfferDetails/',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
                SchemeID: SchemeID,
                ItemID: ItemIDs,
                Qty: 0,
                UnitID: UnitIDs
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {

                    $(response.discountAndOffer).each(function (i, record) {
                        if (self.search_in_array(self.item_offer_details, record.ItemID) == -1) {
                            var obj = { ItemID: record.ItemID, UnitID: record.UnitID, OfferDetails: record.OfferDetails, DiscountPercentage: record.DiscountPercentage }
                            self.item_offer_details.push(obj);

                        }

                    });
                    if (typeof callback == "function") {
                        callback();
                    }
                }
                var a = '';
                $("#sales-order-items-list tbody tr").each(function () {
                    var row = $(this).closest('tr');
                    itemid = clean($(this).find('.ItemID').val());
                    var j = self.search_in_array(self.item_offer_details, itemid);

                    self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
                        a += ' <div> Scheme ' + record.Qty + ' +' + record.OfferQty + '</div>'

                    });
                    $(row).attr('title', a);
                });


            }


        });
    },

    item_offer_details: [],
    // updates the item list datatable on changing the item/sales categories 
    update_item_list: function () {
        item_list.fnDraw();
    },
    // Gets the sales categories for selected item categories
    get_sales_category: function () {
        var self = SalesOrder;
        var item_category_id = $(this).val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = 0;
        }
        $.ajax({
            url: '/Masters/Category/GetSalesCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#SalesCategoryID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#SalesCategoryID").append(html);
            }
        });
    },
    // Selects the item on clicking the modal ok button 
    select_item: function () {
        var self = SalesOrder;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {};
        item.id = radio.val();
        item.name = $(row).find(".Name").text().trim();
        item.code = $(row).find(".Code").text().trim();
        item.unit = $(row).find(".Unit").val();
        item.unitId = $(row).find(".UnitID").val();
        item.saleUnit = $(row).find(".SalesUnit").val();
        item.saleUnitId = $(row).find(".SalesUnitID").val();
        item.stock = $(row).find(".Stock").val();
        item.cgstpercentage = $(row).find(".CGSTPercentage").val();
        item.igstpercentage = $(row).find(".IGSTPercentage").val();
        item.sgstpercentage = $(row).find(".SGSTPercentage").val();
        item.cessPercentage = $(row).find(".CessPercentage").val();
        item.rate = $(row).find(".Rate").val();
        item.looseRate = $(row).find(".LooseRate").val();
        item.category = $(row).find(".ItemCategory").text().trim();
        item.maxsalesqtyfull = $(row).find(".MaxSalesQtyFull").val();
        item.minsalesqtyloose = $(row).find(".MinSalesQtyLoose").val();
        item.minsalesqtyfull = $(row).find(".MinSalesQtyFull").val();
        item.maxsalesqtyloose = $(row).find(".MaxSalesQtyLoose").val();
        self.on_select_item(item);

        UIkit.modal($('#select-item')).hide();
    },

    // on select auto complete item
    set_item: function (event, item) {
        var self = SalesOrder;
        self.on_select_item(item);
    },

    on_select_item: function (item) {
        var self = SalesOrder;
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#PrimaryUnit").val(item.unit);
        $("#Code").val(item.code);
        $("#PrimaryUnitID").val(item.unitId);
        $("#Stock").val(item.stock);
        $("#SalesUnit").val(item.saleUnit);
        $("#SalesUnitID").val(item.saleUnitId);
        $("#CGSTPercentage").val(item.cgstpercentage);
        $("#IGSTPercentage").val(item.igstpercentage);
        $("#SGSTPercentage").val(item.sgstpercentage);
        $("#CessPercentage").val(item.cessPercentage);
        $("#Rate").val(item.rate);
        $("#LooseRate").val(item.looseRate);
        $("#Category").val(item.category);
        $("#MinSalesQtyFull").val(item.minsalesqtyfull);
        $("#MinSalesQtyLoose").val(item.minsalesqtyloose);
        $("#MaxSalesQtyFull").val(item.maxsalesqtyfull);
        $("#MaxSalesQtyLoose").val(item.maxsalesqtyloose);
        $("#Qty").focus();
        self.get_units();
        self.get_discount_and_offer_details();
    },

    get_units: function () {
        var self = SalesOrder;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        $("#UnitID").append(html);
    },
    // Gets the items for auto complete
    get_items: function (release) {
        var self = SalesOrder;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        $.ajax({
            url: '/Masters/Item/GetSaleableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $('#ItemCategoryID').val(),
                SalesCategoryID: $('#SalesCategoryID').val(),
                PriceListID: $('#PriceListID').val(),
                StoreID: $('#StoreID').val(),
                CheckStock: $('#CheckStock').val(),
                BatchTypeID: $('#BatchTypeID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    // on select auto complete customer
    set_customer: function (event, item) {
        var self = SalesOrder;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#SchemeID").val(item.schemeId);
        $("#CurrencyID").val(item.CurrencyID);
        $("#CurrencyName").val(item.CurrencyName);
        $("#CurrencyCode").val(item.CurrencyCode);
        $("#CurrencyExchangeRate").val(item.CurrencyConversionRate);
        $("#CustomerCategoryID").val(item.customercategoryid);
        $("#CustomerCategory").val(item.customercategory);
        $("#DespatchDate").focus();
        // self.get_scheme_allocation();;//commented by prama

        self.get_batch_type(function () {
            var self = SalesOrder;
            var ItemIDS = [];
            var UnitIDS = [];
            $("#sales-order-items-list tbody tr").each(function () {
                ItemIDS.push($(this).find('.ItemID').val());
                UnitIDS.push($(this).find('.UnitID').val());
            });
            if (ItemIDS.length > 0) {
                self.get_offer_details_bulk(ItemIDS, UnitIDS, function () {
                    var self = SalesOrder;

                    $("#sales-order-items-list tbody tr").each(function () {
                        var row = $(this).closest("tr");
                        self.update_item(row);

                        itemid = clean($(this).find('.ItemID').val());
                        var j = self.search_in_array(self.item_offer_details, itemid);
                        var a = '';
                        self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
                            a += '<tr > <td> Scheme ' + record.Qty + ' +' + record.OfferQty + '</td></tr></br>'


                            $(this).closest('tr').title(a);

                        });



                    });
                });
            }

        });
        Sales.get_customer_addresses();

        self.on_change_customer_category();


    },

    clear_grid_offer: function () {
        $("#sales-order-items-list tbody").html('');
        SalesOrder.item_offer_details = [];
        $("#NetAmount").val('');
    },
    // Gets the items for auto complete
    get_customers: function (release) {
        var self = SalesOrder;

        $.ajax({
            url: '/Masters/Customer/GetSalesOrderCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
                CustomerCategoryID: $('#CustomerCategoryID').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    // Clears the item details after adding it to grid 
    clear_item: function () {
        var self = SalesOrder;
        $("#ItemID").val('');
        $("#Unit").val('');
        $("#UnitID").html('');
        $("#Stock").val('');
        $("#CGSTPercentage").val('');
        $("#IGSTPercentage").val('');
        $("#SGSTPercentage").val('');
        $("#DiscountPercentage").val('');
        $("#MaxSalesQtyFull").val('');
        $("#MinSalesQtyLoose").val('');
        $("#MinSalesQtyFull").val('');
        $("#MaxSalesQtyLoose").val('');
        $("#Qty").val('');
        $("#Rate").val(0);
        $("#Category").val('');
        $("#OfferQty").val('');
        $("#DiscPer").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);

    },
    confirm_add_item: function () {
        var self = SalesOrder;
        var IsItemPreent = false;
        $("#sales-order-items-list tbody tr").each(function () {
            if ($(this).find(".ItemID").val() == $("#ItemID").val()) {
                IsItemPreent = true;
            }
        });
        if (IsItemPreent) {
            app.confirm_cancel("Are you sure want to add this item again ?", function () {
                self.add_item();
            }, function () {
            })

        } else {
            self.add_item();
        }

    },
    add_item: function () {
        var self = SalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_item();
            $("#CustomerName").focus();
            return;
        }
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            // $("#ItemName").focus();
            return;
        }
        var item = {};
        item.Name = $("#ItemName").val();
        item.ID = $("#ItemID").val();
        item.Unit = $("#UnitID option:selected").text();
        item.Code = $("#Code").val();
        item.UnitID = $("#UnitID").val();
        item.Stock = $("#Stock").val();
        item.CGSTPercentage = clean($("#CGSTPercentage").val());
        item.IGSTPercentage = clean($("#IGSTPercentage").val());
        item.SGSTPercentage = clean($("#SGSTPercentage").val());
        item.CessPercentage = clean($("#CessPercentage").val());
        //item.DiscountPercentage = clean($("#DiscountPercentage").val());
        item.DiscountPercentage = clean($("#DiscPer").val());
        item.Qty = clean($("#Qty").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.Category = $("#Category").val();
        if (item.Category == "Finished Goods") {
            item.BatchType = $("#BatchType").val();
            item.BatchTypeID = clean($("#BatchTypeID").val());
        }
        else {
            item.BatchType = "";
            item.BatchTypeID = 0;
        }
        if (item.SaleUnitID == item.UnitID) {
            item.MRP = item.FullRate;
        }
        else {
            item.MRP = item.LooseRate;
        }
        if (item.UnitID == $("#SalesUnitID").val()) {
            item.MinSalesQty = clean($("#MinSalesQtyFull").val());
            item.MaxSalesQty = clean($("#MaxSalesQtyFull").val());
        }
        else {
            item.MinSalesQty = clean($("#MinSalesQtyLoose").val());
            item.MaxSalesQty = 5000;
        }

        self.add_item_to_grid(item);
        fh_items.resizeHeader();
        self.clear_item();
        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);
    },


    // checks whether the customer id is set 
    validate_customer: function () {
        var self = SalesOrder;
        if (self.rules.on_select_item.length > 0) {
            return form.validate(self.rules.on_select_item);
        }
        return 0;
    },

    validate_item: function () {
        var self = SalesOrder;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    validate_form: function () {
        var self = SalesOrder;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = SalesOrder;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    // calculates and returns item properties
    get_item_properties: function (item) {
        var self = SalesOrder;
        var CustomerCategory = $("#CustomerCategory").val();
        var IsVATExtra = $("#IsVATExtra").val();
        item.item_offer_details = self.item_offer_details;
        item.OfferQty = self.get_offer_qty(item.Qty, item.ID, item.UnitID);
        //item.DiscountPercentage = self.get_discount_percent(item.ID, item.UnitID);
        item.DiscountPercentage = $("#DiscPer").val();
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        } else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        ////added by anju
        if (IsVATExtra == 1) {
            item.BasicPrice = item.MRP;
        }
        if (CustomerCategory == "Export") {
            item.BasicPrice = item.MRP;
        }
        item.BasicPrice = item.BasicPrice.roundTo(2);
        item.GrossAmount = item.BasicPrice * (item.Qty + item.OfferQty);
        item.DiscountAmount = (item.Qty * item.BasicPrice * item.DiscountPercentage / 100).roundTo(2);
        item.AdditionalDiscount = (item.BasicPrice * item.OfferQty).roundTo(2);
        item.TaxableAmount = (item.GrossAmount - item.DiscountAmount - item.AdditionalDiscount).roundTo(2);
        item.GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);
        if (CustomerCategory == "Export") {
            item.CessAmount = 0;
            item.CessPercentage = 0;
            item.IGST = 0;
            item.CGST = 0;
            item.SGST = 0;
            item.GSTAmount = 0;
            item.GSTPercentage = 0;
        }
        else {
            if (Sales.is_cess_effect()) {
                item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100).roundTo(2);
            } else {
                item.CessAmount = 0;
                item.CessPercentage = 0;
            }
            item.GSTPercentage = item.IGSTPercentage;

            if (!Sales.is_igst()) {
                item.CGST = (item.GSTAmount / 2).roundTo(2);
                item.SGST = (item.GSTAmount / 2).roundTo(2);
                item.IGST = 0;
            } else {
                item.CGST = 0;
                item.SGST = 0;
                item.IGST = item.GSTAmount;
            }
        }
        item.NetAmount = item.TaxableAmount + item.CGST + item.IGST + item.SGST + item.CessAmount;

        return item;
    },
    //add validated items to grid
    add_item_to_grid: function (item) {
        var self = SalesOrder;
        var index, GSTAmount, tr;
        var title = '';
        var readonly = '';
        var a = '';
        item = self.get_item_properties(item);

        var j = self.search_in_array(self.item_offer_details, item.ID);
        index = $("#sales-order-items-list tbody tr").length + 1;
        self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
            a += '<tr > <td> Scheme ' + record.Qty + ' +' + record.OfferQty + '</td></tr></br>'

        });
        if (clean($("#IsPriceEditable").val()) == 0) {
            readonly = ' readonly="readonly"';
        }
        title += a;
        tr = '<tr  data-uk-tooltip="{cls:long-text}" title = "' + title + '" >'
            + ' <td class="uk-text-center">' + index + ' </td>'
            + ' <td >' + item.Code
            + '     <input type="hidden" class="ItemID" value="' + item.ID + '"  />'
            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
            + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '"  />'
            + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
            + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
            + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
            + '     <input type="hidden" class="IGST" value="' + item.IGST + '" />'
            + '     <input type="hidden" class="SGST" value="' + item.SGST + '" />'
            + '     <input type="hidden" class="CGST" value="' + item.CGST + '" />'
            + '     <input type="hidden" class="FullRate" value="' + item.FullRate + '" />'
            + '     <input type="hidden" class="LooseRate" value="' + item.LooseRate + '" />'
            + '     <input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
            + '     <input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
            + '     <input type="hidden" class="Category" value="' + item.Category + '" />'
            + '</td>'
            + ' <td class="uk-text-small">' + item.Name + '</td>'
            + ' <td>' + item.Unit + '</td>'
            + ' <td class="BatchType">' + item.BatchType + '</td>'
            + ' <td class="mrp_hidden"><input type="text" class="md-input MRP mask-sales-currency " value="' + item.MRP + '"' + readonly + '/></td>'
            //+ ' <td class="mrp_hidden"><input type="text" class="MRP mask-sales-currency " value="' + item.MRP + '" /></td>'
            + ' <td><input type="text" class="BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  /></td>'
            + ' <td><input type="text" class="OfferQty mask-numeric" value="' + item.OfferQty + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="DiscountPercentage mask-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="DiscountAmount mask-sales-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="AdditionalDiscount mask-sales-currency" value="' + item.AdditionalDiscount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="TaxableAmount mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GST mask-currency" value="' + item.GSTPercentage + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="GSTAmount mask-sales-currency" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
            + ' <td class="cess-enabled"><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
            + ' <td class="cess-enabled"><input type="text" class="CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
            + ' <td><input type="text" class="NetAmount mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
            + ' <td class="uk-text-center">'
            + '     <a class="remove-item">'
            + '         <i class="uk-icon-remove"></i>'
            + '     </a>'
            + ' </td>'
            + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#sales-order-items-list tbody").append($tr);
        self.calculate_grid_total();
    },

    calculate_grid_total: function () {
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var CessAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var temp = 0;
        $("#sales-order-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            CessAmount += clean($(this).find('.CessAmount').val());
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        var FreightAmt = clean($("#FreightAmount").val());
        NetAmount = NetAmount + FreightAmt;
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = NetAmount - temp;
        $("#GrossAmount").val(GrossAmount);
        $("#DiscountAmount").val(DiscountAmount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#CessAmount").val(CessAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
    },
    //gets offer quantity from the SalesOrder.item_offer_details
    get_offer_qty: function (qty, item_id, unit_id) {
        var self = SalesOrder;
        var index = -2;
        var offer_qty = 0;
        var j = self.search_in_array(self.item_offer_details, item_id);
        while (j != -1 && index != -1) {
            index = -1;
            self.item_offer_details[j].OfferDetails.forEach(function (record, i) {
                if (record.Qty <= qty) {
                    index = i
                }
            });

            if (index != -1) {
                if (self.item_offer_details[j].UnitID == unit_id) {
                    offer_qty += parseInt(qty / self.item_offer_details[j].OfferDetails[index].Qty) * self.item_offer_details[j].OfferDetails[index].OfferQty;
                }
                qty -= parseInt(qty / self.item_offer_details[j].OfferDetails[index].Qty) * self.item_offer_details[j].OfferDetails[index].Qty;

            }
        }

        return offer_qty;
    },

    //gets offer quantity from the SalesOrder.item_offer_details
    get_discount_percent: function (item_id, unit_id) {
        var self = SalesOrder;

        var item_id, discountpercent = 0;


        var index = -2;

        var j = self.search_in_array(self.item_offer_details, item_id);
        while (j != -1 && index != -1) {
            index = -1;
            discountpercent = self.item_offer_details[j].DiscountPercentage;

        }

        return discountpercent;
    },

    // Selects the customer on click modal ok button
    select_customer: function () {
        var self = SalesOrder;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
        var CustomerCategory = $(row).find(".CustomerCategory").text();
        $("#CustomerName").val(Name);
        $("#CustomerCategoryID").val(CustomercategoryID);
        $("#CustomerID").val(ID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#SchemeID").val(SchemeID);
        $("#CustomerCategory").val(CustomerCategory);
        $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
        UIkit.modal($('#select-customer')).hide();

        self.get_batch_type(function () {
            var self = SalesOrder;
            var ItemIDS = [];
            var UnitIDS = [];
            $("#sales-order-items-list tbody tr").each(function () {
                ItemIDS.push($(this).find('.ItemID').val());
                UnitIDS.push($(this).find('.UnitID').val());
            });
            if (ItemIDS.length > 0) {
                self.get_offer_details_bulk(ItemIDS, UnitIDS, function () {
                    var self = SalesOrder;

                    $("#sales-order-items-list tbody tr").each(function () {
                        var row = $(this).closest("tr");
                        self.update_item(row);
                    });
                });
            }

        });
        Sales.get_customer_addresses();
        //   self.clear_grid_offer();
        self.on_change_customer_category();
    },

    get_batch_type: function (callback) {
        var self = SalesOrder;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Masters/Customer/GetBatchType',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#BatchTypeID").val(response.BatchTypeID);
                    $("#BatchType").val(response.BatchType);
                    self.update_item_list();
                }
                if (typeof callback == "function") {
                    callback();
                }
            }
        });
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
            var self = SalesOrder;
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
                self.populate_uploaded_sales_orders();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    uploaded_sales_orders: [],

    populate_uploaded_sales_orders: function () {
        var self = SalesOrder;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Sales/SalesOrder/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#select-file").prop("value", "");
                var customer_code = "";
                var status = "";
                var html = "";
                var $html;
                var j = -1;
                if (response.Status == "success") {
                    $.each(response.Data, function (i, record) {
                        if (customer_code != record.OldCode) {
                            customer_code = record.OldCode;
                            status = record.CustomerCode == null ? "customer-not-identified" : "";
                            html += "<tr class ='" + status + "' >"
                                + "<th colspan = '3'>Customer Code: " + record.CustomerCode + "(" + record.OldCode + ")</th>"
                                + "<th colspan = '3'>Customer Name: " + record.CustomerName + "</th>"
                                + "</tr>";
                            j++;
                            self.uploaded_sales_orders[j] = { CustomerID: record.CustomerID, Items: [] };
                        }
                        status = record.ItemCode == null ? "item-not-identified" : "";
                        html += "<tr class ='" + status + "' >"
                            + "<td>" + (i + 1) + "<input type='hidden' class='ItemID' value='" + record.ItemID + "' /></td>"
                            + "<td>" + record.PackingCode + "</td>"
                            + "<td>" + record.PackingName + "</td>"
                            + "<td>" + (record.ItemCode == null ? "" : record.ItemCode) + "</td>"
                            + "<td>" + (record.ItemCode == null ? "" : record.ItemName) + "</td>"
                            + "<td class='mask-qty Quantity'>" + record.Qty + "</td>"
                            + "</tr>";

                        self.uploaded_sales_orders[j].Items.push(
                            {
                                ItemName: record.ItemName,
                                ItemCategoryID: record.ItemCategoryID,
                                ItemID: record.ItemID,
                                Qty: record.Qty,
                                SGSTPercentage: record.SGSTPercentage,
                                CGSTPercentage: record.CGSTPercentage,
                                IGSTPercentage: record.IGSTPercentage,
                                UnitID: record.UnitID,
                                CessPercentage: record.CessPercentage
                            });
                    });

                    if (html != "") {
                        $html = $(html);
                        $("#notice").html("");

                        app.format($html);
                        $("#uploded-orders-list tbody").html($html);
                        if ($("#uploded-orders-list tbody").find(".customer-not-identified").length > 0) {
                            $("#notice").append("<p>Customer not identified, Please correct the Customer Code(s) and upload the file again</p>");
                        }
                        if ($("#uploded-orders-list tbody").find(".item-not-identified").length > 0) {
                            $("#notice").append("<p>Item not identified, Please correct the Packing Code(s) and upload the file again</p>");
                        }
                        if ($("#uploded-orders-list tbody").find(".customer-not-identified").length > 0 || $("#uploded-orders-list tbody").find(".item-not-identified").length > 0) {
                            $("#btnOkUplodedOrders").hide();
                        } else {
                            $("#btnOkUplodedOrders").show();
                        }
                        UIkit.modal("#uploded-orders").show({ center: false });
                    }
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },

    process_uploaded_orders: function () {
        var self = SalesOrder;
        $.ajax({
            url: '/Sales/SalesOrder/ProcessUploadedOrders/',
            data: { SalesOrders: self.uploaded_sales_orders },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#sales-order-list").trigger("datatable.change");
                    app.show_notice(response.Message);
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },
    // List view datatable

    list: function () {
        var self = SalesOrder;

        $('#tabs-sales-order').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

        $("body").on('click', ".btnclone", self.so_clone);
        $("body").on('click', ".btnprint", self.SalesOrderdetailpage);
        $("body").on('click', ".btnexport", self.SalesOrderExportDetial);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
        $("body").on("click", "#btnOkUplodedOrders", self.process_uploaded_orders);
    },

    tabbed_list: function (type) {
        var self = SalesOrder;

        var $list;

        switch (type) {
            case "draft":
                $list = $('#sales-order-draft-list');
                break;
            case "to-be-invoiced":
                $list = $('#sales-order-to-be-invoiced-list');
                break;
            case "partially-proforma-invoiced":
                $list = $('#sales-order-partially-proforma-invoiced-list');
                break;
            case "fully-proforma-invoiced":
                $list = $('#sales-order-fully-proforma-invoiced-list');
                break;
            case "partially-invoiced":
                $list = $('#sales-order-partially-invoiced-list');
                break;
            case "fully-invoiced":
                $list = $('#sales-order-fully-invoiced-list');
                break;
            case "cancelled":
                $list = $('#sales-order-cancelled-list');
                break;
            case "suspended":
                $list = $('#sales-order-suspended-list');
                break;
            default:
                $list = $('#sales-order-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/SalesOrder/GetSalesOrderList?type=" + type,
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
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    {
                        "data": "SONo",
                        "className": "",
                    },
                    { "data": "SODate", "className": "SODate", "searchable": false, },
                    { "data": "SalesType", "className": "SalesType" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "DespatchDate", "searchable": false, },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + gridcurrencyclass + "' >" + row.NetAmount + "</div>";
                        }
                    },
                   
                    {
                        "data": "", "searchable": false, "className": "action clone", "orderable": false,
                        "render": function (data, type, row, meta) {
                            
                                return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnclone' value = '" + row.ID + "'>CLONE</button>"
                            }
                        }
                    
                    
                    ,
                    {
                        "data": "", "searchable": false, "className": "action print", "orderable": false,
                        "render": function (data, type, row, meta) {
                           /* if (row.Status === 'draft') {*/
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnprint' value = '" + row.ID + "'>Print</button>"
                            }
                        }
                    
                    ,

                    {
                        "data": "", "searchable": false, "className": "action export", "orderable": false,
                        "render": function (data, type, row, meta) {
                           /* if (row.Status === 'draft') {*/
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnexport' value = '" + row.ID + "'>Export</button>"                            }
                        }
                    




                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/SalesOrder/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;
        }
    },
    // Validation rules
    rules: {
        on_select_item: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose a customer" },
                    { type: form.non_zero, message: "Please choose a customer" },
                ]
            },
        ],
        on_add_item: [
            //{
            //    elements: "#ItemID",
            //    rules: [
            //        { type: form.required, message: "Please choose a valid item" },
            //        { type: form.non_zero, message: "Please choose a valid item" },
            //        {
            //            type: function (element) {
            //                var error = false;

            //                $("#sales-order-items-list tbody tr").each(function () {
            //                    if ($(this).find(".ItemID").val() == $(element).val()) {
            //                        error = true;
            //                    }
            //                });
            //                return !error;
            //            }, message: "Item already exists"
            //        },
            //    ]
            //},
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                    //{
                    //    type: function (element) {
                    //        return ($("#Category").val().toLowerCase() == "finished goods") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                    //    }, message: "Please enter a valid quantity for finished goods"
                    //},
                    {
                        type: function (element) {
                            var error = false;

                            var MinSalesQty = $("#MinSalesQty").val();
                            var MaxSalesQty = $("#MaxSalesQty").val();
                            var Qty = clean($(element).val());
                            if (MinSalesQty > Qty) {
                                error = true;
                            }

                            return !error;
                        }, message: "Item Quantity Should Be Greater than MaxSalesQuantity "
                    },
                    {
                        type: function (element) {
                            var error = false;
                            var MinSalesQty = $("#MinSalesQty").val()
                            var MaxSalesQty = $("#MaxSalesQty").val()
                            var Qty = clean($(element).val());
                            if (MaxSalesQty < Qty) {
                                error = true;
                            }
                            return !error;
                        }, message: "Item Quantity Should Be Less than MaxSalesQuantity "
                    },
                ]
            }
        ],
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#SONo",
                rules: [
                    { type: form.required, message: "Invalid Sales Order Number" },
                ]
            },
            {
                elements: "#SODate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: "#DespatchDate",
                rules: [
                    { type: form.required, message: "Dispatch Date is required" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#SODate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Dispatch Date should be a date on or after sales order date"
                    }
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    //{
                    //    type: function (element) {
                    //        return ($(element).closest("tr").find(".Category").val().toLowerCase() == "finished goods") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                    //    }, message: "Please enter a valid quantity for finished goods"
                    //},
                    {
                        type: function (element) {
                            var error = false;
                            $("#sales-order-items-list tbody tr").each(function () {
                                var MinSalesQty = clean($(this).find(".MinSalesQty").val())
                                var MaxSalesQty = clean($(this).find(".MaxSalesQty").val())
                                var Qty = clean($(element).val());
                                if (MinSalesQty > Qty) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item quantity deceeds minimum sales quantity "
                    },
                    {
                        type: function (element) {
                            var error = false;
                            var CheckStock = $("#CheckStock").val();
                            $("#sales-order-items-list tbody tr").each(function () {
                                var MinSalesQty = clean($(this).find(".MinSalesQty").val())
                                var MaxSalesQty = clean($(this).find(".MaxSalesQty").val())
                                var Qty = clean($(element).val());
                                if (CheckStock == true && MaxSalesQty < Qty) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item quantity exceeds maximum sale quantity"
                    },

                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Total Net Amount" },
                    { type: form.non_zero, message: "Invalid Total Net Amount" },
                    { type: form.positive, message: "Invalid Total 3 Net Amount" },
                    {
                        type: function (element) {
                            var error = false;
                            //alert(element.value);
                            if (element.value < 0) {
                                error = true;
                            }
                            return !error;
                        }, message: "Invalid Total Net Amount"
                    }
                ]
            },
            {
                elements: ".NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount1" },
                   { type: form.non_zero, message: "Invalid Net Amount2" },
                    { type: form.positive, message: "Invalid Net Amount3" },
                ]
            },
            //{

            //    elements: "#FreightAmount",
            //    rules: [
            //        {
            //            type: function (element) {
            //                var Category = $("#CustomerCategoryID option:selected").text();
            //                var FregihtAmt = clean($("#FreightAmount"   ).val());
            //                var error = false;
            //                if (Category == 'ECOMMERCE' && FregihtAmt <= 0) {
            //                    error = true;
            //                }
            //                return !error;
            //            }, message: "Invalid FreightAmount"
            //        }
            //    ]
            //},


        ],

        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#SONo",
                rules: [
                    { type: form.required, message: "Invalid Sales Order Number" },
                ]
            },
            {
                elements: "#SODate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },

            {
                elements: "#DespatchDate",
                rules: [
                    { type: form.required, message: "Dispatch Date is required" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#SODate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "Dispatch Date should be a date on or after sales order date"
                    }
                ]
            },
            {
                elements: "#QuotationExpiry",
                rules: [
                    {
                        type: function (element) {
                            var val = $(element).val();

                            return val.length > 0 ? true : false;
                        }, message: "Quotation Expiry is required"
                    },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    {
                        type: function (element) {
                            return ($(element).closest("tr").find(".Category").val().toLowerCase() == "finished goods") ? ($(element).val() == Math.round($(element).val()) ? true : false) : true;
                        }, message: "Please enter a valid quantity for finished goods"
                    },
                    {
                        type: function (element) {
                            var error = false;
                            $("#sales-order-items-list tbody tr").each(function () {
                                var MinSalesQty = clean($(this).find(".MinSalesQty").val())
                                var MaxSalesQty = clean($(this).find(".MaxSalesQty").val())
                                var Qty = clean($(element).val());
                                if (MinSalesQty > Qty) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item quantity deceeds minimum sales quantity "
                    },
                    //{
                    //    type: function (element) {
                    //        var error = false;
                    //        $("#sales-order-items-list tbody tr").each(function () {
                    //            var MinSalesQty = clean($(this).find(".MinSalesQty").val())
                    //            var MaxSalesQty = clean($(this).find(".MaxSalesQty").val())
                    //            var Qty = clean($(element).val());
                    //            if (MaxSalesQty < Qty) {
                    //                error = true;
                    //            }
                    //        });
                    //        return !error;
                    //    }, message: "Item quantity exceeds maximum sales quantity"
                    //},
                ]
            },
            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                   // { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
            {
                elements: ".NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                  //  { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            }
        ],
    },

    save: function () {
        var self = SalesOrder;
        var data;
        var url = "/Sales/SalesOrder/Save";

        data = self.get_data(IsDraft);
        if (IsDraft) {
            url = "/Sales/SalesOrder/SaveAsDraft";
        }
        self.error_count = ((IsDraft == true) ? self.validate_draft() : self.validate_form());
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave, .btnSaveASDraft').css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Sales Order Saved Successfully");
                    setTimeout(function () {
                        window.location = "/Sales/SalesOrder/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $('.btnSave, .btnSaveASDraft').css({ 'display': 'block' });

                }
            }
        });
    },

    get_data: function (IsDraft) {
        var item = {};
        var IsApproved;
        if (clean($("#IsApproved").val()) == 1) {
            IsApproved = true;
        }
        else {
            IsApproved = false;
        }
        if ($("#IsClone").val() == "True") {
            ID = 0;
        }
        else {
            ID = $("#ID").val();
        }
        var data = {
            ID: ID,
            SONo: $("#SONo").val(),
            SODate: $("#SODate").val(),
            CustomerCategoryID: $("#CustomerCategoryID").val(),
            ItemCategoryID: $("#ItemCategoryID").val(),
            CustomerID: $("#CustomerID").val(),
            BillingAddressID: $("#BillingAddressID").val(),
            ShippingAddressID: $("#ShippingAddressID").val(),
            DespatchDate: $("#DespatchDate").val(),
            SchemeAllocationID: $("#SchemeID").val(),
            FreightAmount: clean($("#FreightAmount").val()),
            GrossAmount: clean($("#GrossAmount").val()),
            DiscountPercentage: clean($("#DiscPercentage").val()),
            DiscountAmount: clean($("#DiscAmount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            CurrencyID: $("#CurrencyID").val(),
            CurrencyExchangeRate: $("#CurrencyExchangeRate").val(),
            PaymentTerms: $("#PaymentTerms").val(),
            QuotationExpiry: $("#QuotationExpiry").val(),
            CustomerEnquiryNumber: $("#CustomerEnquiryNumber").val(),
            EnquiryDate: $("#EnquiryDate").val(),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            VATAmount: clean($("#VATAmount").val()),
            TaxTypeID: clean($("#TaxTypeID").val()),
            IsGST: clean($("#IsGST").val()),
            IsVat: clean($("#IsVat").val()),
            CessAmount: clean($("#CessAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            Remarks: $("#Remarks").val(),
            NetAmount: clean($("#NetAmount").val()),
            IsApproved: IsApproved,
            IsDraft: IsDraft,
            PrintWithItemCode: $("#PrintWithItemCode").is(":checked"),
            VATPercentage: clean($("#VATPercentageID option:selected").text()),
            VATPercentageID : clean($("#VATPercentageID").val())
        };
        data.Items = [];
        $("#sales-order-items-list tbody tr").each(function () {
            item = {
                ItemID: $(this).find(".ItemID").val(),
                ItemName: $(this).find(".Name").val(),
                PartsNumber: $(this).find(".PartsNumber").val(),
                UnitID: $(this).find(".UnitID").val(),
                CurrencyID: clean($(this).find(".CurrencyID").val()),
                IsGST: clean($(this).find(".IsGST").val()),
                IsVat: clean($(this).find(".IsVat").val()),
                ExchangeRate: clean($(this).find(".ExchangeRate").val()),
                BaseCurrencyMRP: $(this).find(".BaseCurrencyMRP").val(),
                BatchTypeID: $(this).find(".BatchTypeID").val(),
                MRP: clean($(this).find(".MRP").val()),
                BasicPrice: clean($(this).find(".BasicPrice").val()),
                Qty: clean($(this).find(".Qty").val()),
                OfferQty: clean($(this).find(".OfferQty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount").val()),
                AdditionalDiscount: clean($(this).find(".AdditionalDiscount").val()),
                DiscountPercentage: clean($(this).find(".DiscountPercentage").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercentage").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercentage").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercentage").val()),
                VATPercentage: clean($(this).find(".VATPercentage").val()),
                VATAmount: clean($(this).find(".VATAmount").val()),
                SecondaryUnit: $(this).find('.secondaryUnit option:selected').text().trim(),
                secondaryMRP: clean($(this).find('.secondaryMRP').val()),
                SecondaryUnitSize: clean($(this).find('.secondaryUnit').val()),
                SecondaryRate: clean($(this).find('.secondaryRate').val()),
                SecondaryQty: clean($(this).find('.secondaryQty').val()),
                Model: $(this).find(".Model").val(),
                DeliveryTerm: $(this).find(".DeliveryTerm").val(),
                IGST: clean($(this).find(".IGST").val()),
                SGST: clean($(this).find(".SGST").val()),
                CGST: clean($(this).find(".CGST").val()),
                CessPercentage: clean($(this).find(".CessPercentage").val()),
                CessAmount: clean($(this).find(".CessAmount").val()),
                NetAmount: clean($(this).find(".NetAmount").val())
            }
            data.Items.push(item);
        });
        return data;
    },

    search_in_array: function (array, item_id) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].ItemID == item_id) {
                return i;
            }
        }
        return -1;
    },

    approve: function () {
        $(".btnApprove").css({ 'display': 'none' });

        $.ajax({
            url: '/Sales/SalesOrder/Approve',
            data: {
                SalesOrderID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Successfully Approved");
                    setTimeout(function () {
                        window.location = "/Sales/SalesOrder/Index";
                    }, 1000);
                } else {
                    app.show_error("Approve Failed");
                    $(".btnApprove").css({ 'display': 'block' });
                }
            },
        });
    }
}