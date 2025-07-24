ServiceSalesOrder = {

    init: function () {
        var self = ServiceSalesOrder;
        supplier.Doctor_list();
        $('#doctor-list').SelectTable({
            selectFunction: self.select_doctor,
            returnFocus: "#ItemName",
            modal: "#select-doctor",
            initiatingElement: "#DoctorName"
        });
        Customer.service_customer_list('sales-order');
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        item_list = Item.saleable_service_items_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.AmountDetails = [];

        self.hide_show_doctor();
        self.bind_events();
        self.freeze_headers();
        if (clean($("#ID").val()) > 0) {
            self.calculate_grid_total();
        }
        self.calculate_grid_total();
    },

    details: function () {
        var self = ServiceSalesOrder;
        self.freeze_headers();

    },

    freeze_headers: function () {
        var self = ServiceSalesOrder;
        fh_items = $("#sales-order-items-list").FreezeHeader();
        $("body").on('click', ".cancel", self.cancel_confirm);
        $('#tabs-order[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                fh_items.resizeHeader();
            }, 500);
        });
    },

    bind_events: function () {
        var self = ServiceSalesOrder;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKCustomer").on("click", self.select_customer);
        $("#CustomerCategoryID").on("change", self.clear_customer);
        $("#ItemCategoryID").on("change", self.clear_items);
        $("#btnAddItem").on("click", self.add_item);
        $("body").on("keyup change", "#sales-order-items-list tbody .Qty", self.update_item);
        $("body").on("click", ".remove-item", self.remove_item);
        $("#btnOKDoctor").on("click", self.select_doctor);
        $.UIkit.autocomplete($('#doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $("#DirectInvoice").on('ifChanged', self.disable_address_list);
        $(".btnSave, .btnSaveAndNew, .btnSaveASDraft").on("click", self.on_save);
    },
    disable_address_list: function () {
        if ($("#DirectInvoice").prop('checked') == true) {
            $("#ShippingToId").prop("disabled", true);
            $("#DDLBillTo").prop("disabled", true);
            var IsBranch = $('#IsBranchLocation').val();
            var BillingAddressID = $('#BillingAddressID').val();
            var ShippingAddressID = $('#ShippingAddressID').val();
            $('#ShippingToId').val(BillingAddressID);
            $('#DDLBillTo').val(ShippingAddressID);
        }
        else {
            $("#ShippingToId").prop("disabled", false);
            $("#DDLBillTo").prop("disabled", false);
        }
    },
    cancel_confirm: function () {
        var self = ServiceSalesOrder
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    cancel: function () {
        $.ajax({
            url: '/Sales/ServiceSalesOrder/Cancel',
            data: {
                SalesOrderID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Service Sales Order cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/ServiceSalesOrder/Index";
                    }, 1000);
                }
                else if (data.Status == "fail") {
                    app.show_error("Please cancel service sales invoice");
                }
                else {
                    app.show_error("Failed to cancel.");
                }
            },
        });
    },

    on_save: function () {

        var self = ServiceSalesOrder;
        var data = self.get_data();
        var location = "/Sales/ServiceSalesOrder/Index";
        var url = '/Sales/ServiceSalesOrder/Save';

        if ($(this).hasClass("btnSaveASDraft")) {
            data.IsDraft = true;
            url = '/Sales/ServiceSalesOrder/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_draft();
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
        var self = ServiceSalesOrder;
        $('.btnSave, .btnSaveASDraft').css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("ServiceSales Order Saved Successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $('.btnSave, .btnSaveASDraft').css({ 'display': 'block' });

                }
            }
        });
    },

    get_data: function () {
        var self = ServiceSalesOrder;
        var item = {};
        var data = {
            ID: $("#ID").val(),
            SONo: $("#SONo").val(),
            SODate: $("#SODate").val(),
            CustomerCategoryID: $("#CustomerCategoryID").val(),
            ItemCategoryID: $("#ItemCategoryID").val(),
            CustomerID: $("#CustomerID").val(),
            BillingAddressID: $("#BillingAddressID").val(),
            ShippingAddressID: $("#ShippingAddressID").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            DiscountAmount: clean($("#DiscountAmount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            CessAmount: clean($("#CessAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            DirectInvoice: $("#DirectInvoice").is(":checked") ? true : false,
            PaymentModeID: $("#PaymentModeID").val()
        };
        data.Items = [];
        $("#sales-order-items-list tbody tr").each(function () {
            item = {
                ItemID: $(this).find(".ItemID").val(),
                UnitID: $(this).find(".UnitID").val(),
                MRP: clean($(this).find(".MRP").val()),
                BasicPrice: clean($(this).find(".BasicPrice").val()),
                Qty: clean($(this).find(".Qty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount").val()),
                DiscountPercentage: clean($(this).find(".DiscountPercentage").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").val()),
                GSTPercentage: clean($(this).find(".GSTPercentage").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercentage").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercentage").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercentage").val()),
                IGST: clean($(this).find(".IGST").val()),
                SGST: clean($(this).find(".SGST").val()),
                CGST: clean($(this).find(".CGST").val()),
                CessPercentage: clean($(this).find(".CessPercentage").val()),
                CessAmount: clean($(this).find(".CessAmount").val()),
                NetAmount: clean($(this).find(".NetAmount").val()),
                DoctorID: clean($(this).find(".DoctorID").val()),
                Remarks: $(this).find(".Remarks").val(),
                BillableID: $(this).find(".BillableID").val()
            }
            data.Items.push(item);
        });
        data.AmountDetails = [];
        $("#sales-invoice-amount-details-list tbody tr").each(function () {
            item = {
                Particulars: $(this).find(".Particulars").text(),
                Percentage: $(this).find(".Amnt-Percentage").val(),
                Amount: clean($(this).find(".Amount").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").text()),

            }
            data.AmountDetails.push(item);
        });
        return data;
    },

    validate_form: function () {
        var self = ServiceSalesOrder;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = ServiceSalesOrder;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },


    remove_item: function () {
        var self = ServiceSalesOrder;
        $(this).closest("tr").remove();
        $("#sales-order-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#sales-order-items-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader();
    },

    add_item: function () {
        var self = ServiceSalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.Name = $("#ItemName").val();
        item.ID = $("#ItemID").val();
        item.Unit = $("#Unit").val();
        item.Code = $("#Code").val();
        item.UnitID = $("#UnitID").val();
        item.GSTPercentage = clean($("#GSTPercentage").val());
        item.CessPercentage = clean($("#CessPercentage").val());
        item.DiscountPercentage = clean($("#DiscountPercentage").val());
        item.Qty = clean($("#Qty").val());
        item.Rate = clean($("#Rate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.MRP = item.Rate;
        item.Doctor = $("#DoctorName").val();
        item.DoctorID = $("#DoctorID").val();
        item.Remarks = $("#Remarks").val(),
        self.add_item_to_grid(item);
        fh_items.resizeHeader();
        self.clear_items();
        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);
    },

    get_item_properties: function (item) {
        var self = ServiceSalesOrder;
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.GSTPercentage + item.CessPercentage);
        } else {
            item.BasicPrice = item.MRP * 100 / (100 + item.GSTPercentage);
        }
        item.BasicPrice = (item.BasicPrice).roundTo(2);
        item.GrossAmount = (item.BasicPrice * item.Qty).roundTo(2);
        item.DiscountAmount = (item.Qty * item.BasicPrice * item.DiscountPercentage / 100).roundTo(2);
        item.TaxableAmount = item.GrossAmount - item.DiscountAmount;
        item.GSTAmount = (item.TaxableAmount * item.GSTPercentage / 100).roundTo(2);
        if (Sales.is_cess_effect()) {
            item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100).roundTo(2);
        } else {
            item.CessAmount = 0;
        }

        if (!Sales.is_igst()) {
            item.CGST = (item.GSTAmount / 2).roundTo(2);
            item.SGST = (item.GSTAmount / 2).roundTo(2);
            item.SGSTPercentage = (item.GSTPercentage / 2).roundTo(2);
            item.CGSTPercentage = (item.GSTPercentage / 2).roundTo(2);
            item.IGSTPercentage = 0;
            item.IGST = 0;
        } else {
            item.IGSTPercentage = item.GSTPercentage;
            item.SGSTPercentage = 0;
            item.CGSTPercentage = 0;
            item.CGST = 0;
            item.SGST = 0;
            item.IGST = item.GSTAmount;
        }
        item.NetAmount = item.TaxableAmount + item.CGST + item.SGST + item.IGST + item.CessAmount;
        return item;
    },

    add_item_to_grid: function (item) {
        var self = ServiceSalesOrder;
        var index, GSTAmount, tr;
        item = self.get_item_properties(item);
        index = $("#sales-order-items-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td >' + item.Code
                + '     <input type="hidden" class="ItemID" value="' + item.ID + '"  />'
                + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '     <input type="hidden" class="" value="' + item.GSTPercentage + '" />'
                + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                + '     <input type="hidden" class="IGST" value="' + item.IGST + '" />'
                + '     <input type="hidden" class="SGST" value="' + item.SGST + '" />'
                + '     <input type="hidden" class="CGST" value="' + item.CGST + '" />'
                + '     <input type="hidden" class="Rate" value="' + item.Rate + '" />'
                + '     <input type="hidden" class="DoctorID" value="' + item.DoctorID + '"  />'
                + '</td>'
                + ' <td >' + item.Name + '</td>'
                + ' <td >' + item.Unit + '</td>'
                + ' <td class="Doctor">' + item.Doctor + '</td>'
                + ' <td ><input type="text" class="MRP mask-sales-currency " value="' + item.MRP + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  /></td>'
                + ' <td ><input type="text" class="GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="DiscountPercentage mask-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="DiscountAmount mask-sales-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="TaxableAmount mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="GST mask-currency GSTPercentage" value="' + item.GSTPercentage + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="GSTAmount mask-sales-currency" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="NetAmount mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + ' <td><input type="text" class="Remarks md-input label-fixed" value="' + item.Remarks + '" /></td>'
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
        var self = ServiceSalesOrder;
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
        var is_igst = self.is_igst();
        if (is_igst) {
            $("#IGSTAmount").val(IGSTAmount);
        } else {
            $("#SGSTAmount").val(SGSTAmount);
            $("#CGSTAmount").val(CGSTAmount);
        }
        self.set_amount_details();
    },
    is_igst: function () {
        var self = ServiceSalesOrder;
        if ($("#StateID").val() != $("#LocationStateID").val()) {
            return true;
        }
        return false;
    },
    set_amount_details: function () {
        var self = ServiceSalesOrder;
        var is_igst = self.is_igst();
        var GSTPercent;
        var GSTAmount;
        var tr = "";
        var $tr;
        var TaxableAmount;
        self.AmountDetails = [];
        $("#sales-order-items-list tbody tr").each(function () {
            var IGSTPercentage = clean($(this).find('.GSTPercentage').val());
            var GSTAmount = clean($(this).find('.IGST').val());
            var CGSTAmount = clean($(this).find('.CGST').val());
            var CessAmount = clean($(this).find('.CessAmount').val());
            var CessPercentage = clean($(this).find('.CessPercentage').val());
            var TaxableAmount = clean($(this).find('.TaxableAmount').val());


            if (is_igst) {
                self.calculate_group_tax_details(IGSTPercentage, GSTAmount, "IGST", TaxableAmount);
            } else {
                self.calculate_group_tax_details(IGSTPercentage / 2, CGSTAmount, "SGST", TaxableAmount);
                self.calculate_group_tax_details(IGSTPercentage / 2, CGSTAmount, "CGST", TaxableAmount);
                self.calculate_group_tax_details(CessPercentage, CessAmount, "Cess", TaxableAmount);

            }
        });
        $.each(self.AmountDetails, function (i, record) {
            tr += "<tr  class='uk-text-center'>";
            tr += "<td>" + (i + 1);
            tr += "</td>";
            tr += "<td class='Particulars'>" + record.Particulars;
            tr += "</td>";
            tr += "<td class='mask-currency TaxableAmount'>" + record.TaxableAmount;
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency Amnt-Percentage' readonly value='" + record.Percentage + "' />";
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency Amount' readonly value='" + record.Amount + "' />";
            tr += "</td>";
            tr += "</tr>";
        });
        $tr = $(tr);
        app.format($tr);
        $("#sales-invoice-amount-details-list tbody").html($tr);
    },

    calculate_group_tax_details: function (GSTPercent, GSTAmount, type, TaxableAmount) {
        var self = ServiceSalesOrder;
        var index = self.search_tax_group(self.AmountDetails, GSTPercent, type);
        if (index == -1) {
            self.AmountDetails.push({
                Percentage: GSTPercent,
                Amount: GSTAmount,
                Particulars: type,
                TaxableAmount: TaxableAmount
            });
        } else {
            self.AmountDetails[index].Amount += GSTAmount;
            self.AmountDetails[index].TaxableAmount += TaxableAmount;
        }
    },

    search_tax_group: function (array, value, type) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].Percentage == value && array[i].Particulars == type) {
                return i;
            }
        }
        return -1;
    },

    update_item: function () {
        var self = ServiceSalesOrder;
        var row = $(this).closest("tr");
        var item = {};
        item.Qty = clean($(this).val());
        item.MRP = clean($(row).find(".MRP").val());
        item.DiscountPercentage = clean($(row).find(".DiscountPercentage").val());
        item.GSTPercentage = clean($(row).find(".GSTPercentage").val());
        item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
        item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.ID = clean($(row).find(".ItemID").val());
        item.UnitID = clean($(row).find(".UnitID").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
        item.CessPercentage = clean($("#CessPercentage").val());
        item = self.get_item_properties(item);

        $(row).find(".GrossAmount").val(item.GrossAmount);
        $(row).find(".DiscountAmount").val(item.DiscountAmount);
        $(row).find(".TaxableAmount").val(item.TaxableAmount);
        $(row).find(".CGST").val(item.CGST);
        $(row).find(".SGST").val(item.SGST);
        $(row).find(".IGST").val(item.IGST);
        $(row).find(".GSTAmount").val(item.GSTAmount);
        $(row).find(".NetAmount").val(item.NetAmount);

        self.calculate_grid_total();
    },

    get_discount: function () {
        var self = ServiceSalesOrder;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        if (self.error_count > 0) {
            self.clear_items();
            $("#CustomerName").focus();
            return;
        }
        var CustomerID = $("#CustomerID").val();
        var ItemID = $("#ItemID").val();
        $.ajax({
            url: '/Sales/ServiceSalesOrder/GetDiscountPercentage',
            dataType: "json",
            data: {
                CustomerID: CustomerID,
                ItemID: ItemID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#DiscountPercentage").val(response.discountAndOffer);
                }
            }
        });

    },

    clear_items: function () {
        var self = ServiceSalesOrder;
        $("#Rate").prop('disabled', false);
        $("#ItemName").val("");
        $("#ItemID").val("");
        $("#Unit").val("");
        $("#GSTPercentage").val("");
        $("#Rate").val("");
        $("#Qty").val("");
        $("#DoctorName").val("");
        $("#Remarks").val("");

    },

    clear_customer: function () {
        var self = ServiceSalesOrder;
        $("#CustomerID").val("");
        $("#CustomerName").val("");
        $("#StateID").val("");
        $("#PriceListID").val("");
        $("#IsGSTRegistered").val("");
        $("#SchemeID").val("");
        $("#BatchTypeID").val("");
        $("#BillingAddressID").val("");
        $("#ShippingAddressID").val("");
    },

    get_customers: function (release) {
        var self = ServiceSalesOrder;
        $.ajax({
            url: '/Masters/Customer/GetServiceSalesOrderCustomersAutoComplete',
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

    set_customer: function (event, item) {
        var self = ServiceSalesOrder;
        $("#CustomerName").val(item.Name);
        $("#CustomerCategoryID").val(item.customercategoryid);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#DespatchDate").focus();
        Sales.get_customer_addresses();
        self.get_billable_items();
    },

    select_customer: function () {
        var self = ServiceSalesOrder;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var CustomerCategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerCategoryID ").val(CustomerCategoryID);
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
        UIkit.modal($('#select-customer')).hide();
        Sales.get_customer_addresses();
        self.get_billable_items();
    },

    select_item: function () {
        var self = ServiceSalesOrder;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");

        var item = {};
        item.label = $(row).find(".Name").text().trim();
        item.id = radio.val();
        item.code = $(row).find(".Code").text().trim();
        item.unit = $(row).find(".SalesUnit").val();
        item.unitid = $(row).find(".SalesUnitID").val();
        item.gstPercentage = $(row).find(".gstPercentage").val();
        item.mrp = clean($(row).find(".MRP").val());
        item.cessPercentage = $(row).find(".CessPercentage").val();
        $("#Code").val(item.code);
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#Unit").val(item.unit);
        $("#UnitID").val(item.unitid);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CessPercentage").val(item.cessPercentage);
        self.on_select_item(item);
        self.hide_show_doctor(item.label);
        UIkit.modal($('#select-item')).hide();
    },

    set_item_details: function (event, item) {   // on select auto complete item
        var self = ServiceSalesOrder;
        self.on_select_item(item);
        self.hide_show_doctor(item.value);


    },

    on_select_item: function (item) {
        var self = ServiceSalesOrder;
        $("#Qty").val('');
        $("#Rate").val('');
        $("#Code").val(item.code);
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#Unit").val(item.unit);
        $("#UnitID").val(item.unitid);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CessPercentage").val(item.cessPercentage);
        if (item.mrp == 0) {
            $("#Rate").prop('disabled', false);
        } else {
            $("#Rate").prop('disabled', true);
            $("#Rate").val(item.mrp);
        }
        $("#Qty").focus();
        self.get_discount();


    },

    get_item_details: function (release) {

        $.ajax({
            url: '/Masters/Item/GetItemsListForSaleableServiceItemForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    validate_customer: function () {
        var self = ServiceSalesOrder;
        if (self.rules.on_select_item.length > 0) {
            return form.validate(self.rules.on_select_item);
        }
        return 0;
    },

    validate_add: function () {
        var self = ServiceSalesOrder;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    rules: {

        on_select_item: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose a customer", alt_element: "#CustomerName" },
                    { type: form.non_zero, message: "Please choose a customer", alt_element: "#CustomerName" },
                ]
            },
        ],

        on_add_item: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose a customer", alt_element: "#CustomerName" },
                    { type: form.non_zero, message: "Please choose a customer", alt_element: "#CustomerName" },
                ]
            },
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item", alt_element: "#ItemName" },
                    { type: form.non_zero, message: "Please choose a valid item", alt_element: "#ItemName" },
                    {
                        type: function (element) {
                            var error = false;

                            $("#sales-order-items-list tbody tr").each(function () {
                                if ($(this).find(".ItemID").val() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already exists"
                    },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                ]
            },

        {
            elements: "#Rate",
            rules: [
                { type: form.required, message: "Please enter a valid rate" },
                { type: form.non_zero, message: "Please enter a valid rate" },
                { type: form.positive, message: "Please enter a valid rate" },
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
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                ]
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
                elements: ".NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
        {
            elements: "#PaymentModeID",
            rules: [
                {
                    type: function (element) {
                        var error = true;
                        var asr = $("#PaymentModeID option:selected").val();
                        if (($("#DirectInvoice").prop('checked') == true)) {
                            if ($(element).val() == "")
                                error = false;
                        }
                        return error;
                    }, message: "Please select payment mode"
                }
            ]
        }
        ],

        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one Item" },
                    { type: form.required, message: "Please add atleast one Item" },

                ]
            },
        ],

    },

    list: function () {
        var self = ServiceSalesOrder;

        $('#tabs-service-sales-order').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "to-be-invoiced":
                $list = $('#to-be-invoiced-list');
                break;
            case "fully-invoiced":
                $list = $('#fully-invoiced-list');
                break;
            case "partially-invoiced":
                $list = $('#partially-invoiced-list');
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
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": "/Sales/ServiceSalesOrder/GetServiceSalesOrderList?type=" + type,
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

                    { "data": "CustomerName", "className": "CustomerName" },

                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },

                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/ServiceSalesOrder/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            $('body').on("datatable.change", '#sales-order-list', function () {
                list_table.fnDraw();
            });
            return list_table;
        }
    },

    select_doctor: function () {
        var self = ServiceSalesOrder;
        var radio = $('#select-doctor tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-doctor')).hide();
    },

    hide_show_doctor: function (name) {
        var self = ServiceSalesOrder;
        if (name == 'CONSULTATION FEE TO DOCTORS') {
            $(".doctor").show();
        }
        else {
            $(".doctor").hide();
        }
    },

    get_doctor: function (release) {

        $.ajax({
            url: '/Masters/Supplier/GetDoctorAutoComplete',
            data: {
                term: $('#DoctorName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_doctor: function (event, item) {
        var self = ServiceSalesOrder;
        $("#addDoctorName").val(item.Name);
        $("#DoctorName").val(item.Name);
        $("#DoctorID").val(item.id);
    },

    //Get IpPatient Bill Items On Customer Selection
    get_billable_items: function () {
        var length;
        var tr = "";
        var $tr;
        var self = ServiceSalesOrder;
        $.ajax({
            url: '/Sales/ServiceSalesOrder/GetBillingDetails/',
            dataType: "json",
            data: {
                'CustomerID': $("#CustomerID").val(),
            },
            type: "POST",
            success: function (response) {
                length = response.Data.length;
                if (length != 0) {
                    $(response.Data).each(function (i, item) {
                        var slno = (i + 1);
                        tr += '<tr>'
                            + ' <td class="uk-text-center">' + slno + ' </td>'
                            + ' <td >' + item.Code
                            + '     <input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                            + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                            + '     <input type="hidden" class="" value="' + item.GSTPercentage + '" />'
                            + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                            + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                            + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                            + '     <input type="hidden" class="IGST" value="' + item.IGST + '" />'
                            + '     <input type="hidden" class="SGST" value="' + item.SGST + '" />'
                            + '     <input type="hidden" class="CGST" value="' + item.CGST + '" />'
                            + '     <input type="hidden" class="Rate" value="' + item.MRP + '" />'
                            + '     <input type="hidden" class="BillableID" value="' + item.BillableID + '" />'
                            + '     <input type="hidden" class="DoctorID" value="' + item.DoctorID + '"  />'
                            + '</td>'
                            + ' <td >' + item.Name + '</td>'
                            + ' <td >' + item.Unit + '</td>'
                            + ' <td class="Doctor">' + item.DoctorName + '</td>'
                            + ' <td ><input type="text" class="MRP mask-sales-currency " value="' + item.MRP + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="BasicPrice mask-sales-currency " value="' + item.MRP + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  /></td>'
                            + ' <td ><input type="text" class="GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="DiscountPercentage mask-currency" value="' + item.DiscountPercentage + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="DiscountAmount mask-sales-currency" value="' + item.DiscountAmount + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="TaxableAmount mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="GST mask-currency GSTPercentage" value="' + item.GSTPercentage + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="GSTAmount mask-sales-currency" value="' + item.GSTAmount + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                            + ' <td ><input type="text" class="NetAmount mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                            + ' <td><input type="text" class="Remarks md-input label-fixed" /></td>'
                            + ' <td class="uk-text-center">'
                            + '     <a class="remove-item">'
                            + '         <i class="uk-icon-remove"></i>'
                            + '     </a>'
                            + ' </td>'
                            + '</tr>';
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $('#sales-order-items-list tbody').html($tr);
                    self.calculate_grid_total();
                    $("#item-count").val($("#sales-order-items-list tbody tr").length + 1);
                }
            },
        });
    },






}