ServiceSalesInvoice = {

    init: function () {
        var self = ServiceSalesInvoice;

        Customer.service_customer_list("sales-invoice");
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#SalesOrderNos",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });

        self.Invoice = {};
        self.Invoice.InvoiceNo = $("#InvoiceNo").val();
        self.Invoice.InvoiceDate = $("#InvoiceDate").val();
        self.Invoice.AmountDetails = [];

        self.service_sales_order_list();

        $('#sales-order-list').SelectTable({
            selectFunction: self.select_service_sales_order,
            modal: "#select-source",
            initiatingElement: "#SalesOrderNos",
            selectionType: "checkbox"
        })
        self.bind_events();
        self.set_object_values();
    },

    details: function () {
        var self = ServiceSalesInvoice;
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".cancel", self.cancel_confirm);
    },

    printpdf: function () {
        var self = ServiceSalesInvoice;
        $.ajax({
            url: '/Reports/Sales/ServiceSalesInvoicePrintPdf',
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

    bind_events: function () {
        var self = ServiceSalesInvoice;
        $('.btnSave').on('click', self.save_confirm);
        $(".btnSaveASDraft").on("click", self.save);

        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);

        $("#btnOKCustomer").on("click", self.select_customer);
        $("#btnOkSalesOrder").on('click', self.select_service_sales_order);
        $("body").on("keyup change", "#sales-invoice-items-list tbody .Qty", self.update_item);
        $("#DiscountPercentageList").on('change', self.calculate_discount_Amount);
        $("body").on("click", ".cancel", self.cancel_confirm);
    },

    cancel: function () {
        var self = ServiceSalesInvoice;
        $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Sales/ServiceSalesInvoice/Cancel',
            data: {
                SalesInvoiceID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Counter sales cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/ServiceSalesInvoice/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },

    cancel_confirm: function () {
        var self = ServiceSalesInvoice;
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    save_confirm: function () {
        var self = ServiceSalesInvoice;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    update_item: function () {
        var self = ServiceSalesInvoice;
        var row = $(this).closest("tr");
        var item = {};
        item.Qty = clean($(this).val());
        item.MRP = clean($(row).find(".MRP").val());
        item.DiscountPercentage = clean($(row).find(".DiscountPercentage").val());
        item.GSTPercentage = clean($(row).find(".GSTPercentage").val());
        item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
        item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.CGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.ID = clean($(row).find(".ItemID").val());
        item.UnitID = clean($(row).find(".UnitID").val());
        item.FullRate = clean($("#Rate").val());
        item.LooseRate = clean($("#LooseRate").val());
        item.SaleUnitID = clean($("#SalesUnitID").val());
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

    get_item_properties: function (item) {
        var self = ServiceSalesInvoice;
        if (Sales.is_cess_effect()) {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage);
        }
        else {
            item.BasicPrice = item.MRP * 100 / (100 + item.IGSTPercentage);
        }
        item.BasicPrice = (item.BasicPrice).roundTo(2);
        item.GrossAmount = (item.BasicPrice * item.Qty).roundTo(2);
        item.DiscountAmount = (item.Qty * item.BasicPrice * item.DiscountPercentage / 100).roundTo(2);
        item.TaxableAmount = item.GrossAmount - item.DiscountAmount;
        if (Sales.is_cess_effect()) {
            item.CessAmount = (item.TaxableAmount * item.CessPercentage / 100).roundTo(2);
        } else {
            item.CessAmount = 0;
        }
        item.GSTAmount = (item.TaxableAmount * item.GSTPercentage / 100).roundTo(2);

        if ($("#StateID").val() == $("#LocationStateID").val()) {
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
        item.NetAmount = (item.TaxableAmount + item.IGST + item.CGST + item.SGST + item.CessAmount).roundTo(2);
        return item;
    },

    save: function () {
        var self = ServiceSalesInvoice;
        var url;
        self.set_object_values();
        self.Invoice.PaymentModeID = $("#PaymentModeID").val();
        self.Invoice.SalesOrderID = $("#SalesOrderID").val();
        self.Invoice.Items = self.Items;
        self.Invoice.amountDetails = self.amountDetails;
        if ($(this).hasClass("btnSaveASDraft")) {
            self.error_count = 0;
            self.error_count = self.validate_draft();
            if (self.error_count > 0) {
                return;
            }
            self.Invoice.IsDraft = true;
            url = '/Sales/ServiceSalesInvoice/SaveAsDraft';
        }
        else {
            url = '/Sales/ServiceSalesInvoice/Save';
        }

        self.Invoice.BillingAddressID = $("#BillingAddressID").val();
        self.Invoice.ShippingAddressID = $("#ShippingAddressID").val();

        $(".btnSaveASDraft, .btnSave, .cancel").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: self.Invoice,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Sales Invoice Saved Successfully");
                    setTimeout(function () {
                        window.location = "/Sales/ServiceSalesInvoice/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSave, .cancel").css({ 'display': 'block' });
                }
            }
        });
    },

    set_object_values: function () {
        var self = ServiceSalesInvoice;
        self.Invoice.ID = $("#ID").val();
        self.Invoice.CustomerID = $("#CustomerID").val();
        self.Invoice.SalesOrderNos = $("#SalesOrderNos").val();
        self.Invoice.SalesOrderID = $("#SalesOrderID").val();
        self.Invoice.BillingAddressID = $("#BillingAddressID").val();
        self.Invoice.ShippingAddressID = $("#ShippingAddressID").val();
        self.Invoice.GrossAmount = clean($("#GrossAmount").val());
        self.Invoice.DiscountAmount = clean($("#DiscountAmount").val());
        self.Invoice.TaxableAmount = clean($("#TaxableAmount").val());
        self.Invoice.SGSTAmount = clean($("#SGSTAmount").val());
        self.Invoice.CGSTAmount = clean($("#CGSTAmount").val());
        self.Invoice.NetAmount = clean($("#NetAmount").val());
        self.Invoice.IGSTAmount = clean($("#IGSTAmount").val());
        self.Invoice.RoundOff = clean($("#RoundOff").val());
        self.Invoice.CessAmount = $("#CessAmount").val();
        self.Invoice.DiscountCategoryID = clean($("#DiscountPercentageList").val());
        var item = {};
        self.Items = [];

        $("#sales-invoice-items-list tbody tr").each(function (i, record) {
            item = {
                ItemName: $(record).find(".ItemName").text().trim(),
                ItemID: $(record).find(".ItemID").val(),
                UnitName: $(record).find(".UnitName").text().trim(),
                Code: $(record).find(".UnitName").text().trim(),
                UnitID: $(record).find(".UnitID").val(),
                CGSTPercentage: clean($(record).find(".CGSTPercentage").val()),
                IGSTPercentage: clean($(record).find(".IGSTPercentage").val()),
                SGSTPercentage: clean($(record).find(".SGSTPercentage").val()),
                DiscountPercentage: clean($(record).find(".DiscountPercentage").val()),
                Qty: clean($(record).find(".Qty").val()),
                SalesOrderItemID: clean($(record).find(".SalesOrderTransID").val()),
                MRP: clean($(record).find(".MRP").val()),
                InvoiceQty: clean($(record).find(".Qty").val()),
                IsIncluded: true,
                Rate: clean($(record).find(".Rate").val()),
                BasicPrice: clean($(record).find(".BasicPrice").val()),
                GrossAmount: clean($(record).find(".GrossAmount").val()),
                DiscountAmount: clean($(record).find(".DiscountAmount").val()),
                TaxableAmount: clean($(record).find(".TaxableAmount").val()),
                GSTAmount: clean($(record).find(".GSTAmount").val()),
                NetAmount: clean($(record).find(".NetAmount").val()),
                IGST: clean($(record).find(".IGST").val()),
                SGST: clean($(record).find(".CGST").val()),
                CGST: clean($(record).find(".CGST").val()),
                CessAmount: clean($(record).find(".CessAmount").val()),
                CessPercentage: clean($(record).find(".CessPercentage").val()),
                Remarks: $(record).find(".Remarks").val()
            }
            self.Items.push(item);
        });

        var amountDetail = {};

        self.amountDetails = [];
        $("#sales-invoice-amount-details-list tbody tr").each(function (i, record) {
            amountDetail = {
                Particulars: $(record).find(".Particulars").text().trim(),
                Percentage: $(record).find(".Amnt-Percentage").val(),
                Amount: $(record).find(".Amount").val(),
            }
            self.amountDetails.push(amountDetail);
        });
    },

    validate_form: function () {
        var self = ServiceSalesInvoice;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = ServiceSalesInvoice;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    select_service_sales_order: function () {
        var self = ServiceSalesInvoice;
        //var radio = $('#sales-order-list tbody input[type="checkbox"]:checked');
        var checkboxes = $('#sales-order-list tbody input[type="checkbox"]:checked');
        var row;
        var ID = [];
        var TransNos = "";
        var NetAmount = 0;

        if (checkboxes.length > 0) {
            if ($("#sales-invoice-items-list tbody tr").length > 0) {
                app.confirm("Items will be removed", function () {
                    $.each(checkboxes, function () {
                        row = $(this).closest("tr");
                        ID.push($(this).val());
                        TransNos += $(row).find(".SONo").text().trim() + ",";
                        NetAmount += clean($(row).find(".NetAmount").val());
                    });
                    self.clear_items();
                    self.get_sales_order_items(ID);
                    TransNos = TransNos.slice(0, -1);
                    $("#SalesOrderNos").val(TransNos);
                    self.Invoice.SalesOrderNos = TransNos;
                }, function () {

                });
            } else {
                $.each(checkboxes, function () {
                    row = $(this).closest("tr");
                    ID.push($(this).val());
                    TransNos += $(row).find(".SONo").text().trim() + ",";
                    NetAmount += clean($(row).find(".NetAmount").val());
                });
                self.get_sales_order_items(ID);
                TransNos = TransNos.slice(0, -1);
                $("#SalesOrderNos").val(TransNos);
                self.Invoice.SalesOrderNos = TransNos;
            }
        }
        else {
            app.show_error("No orders are selected");
        }


        //var row = $(radio).closest("tr");
        //var ID = radio.val();
        //var CustomerName = $(row).find(".CustomerName").text().trim();
        //var NetAmount = $(row).find(".NetAmount").text().trim();
        //var SONo = $(row).find(".SONo").text().trim();
        //var SODate = $(row).find(".SODate").text().trim();
        //$("#SalesOrderNos").val(SONo);
        //$("#SalesOrderID").val(ID);
        //UIkit.modal($('#select-source')).hide();
        //self.Invoice.CustomerID = ID;
        //self.get_sales_order_items(ID);
    },

    get_sales_order_items: function (ID) {
        var self = ServiceSalesInvoice;
        $('#sales-invoice-items-list tbody tr').remove();
        $.ajax({
            url: '/Sales/ServiceSalesOrder/GetServiceSalesOrderItems',
            dataType: "json",
            data: {
                SalesOrderID: ID
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    var GST = item.SGST + item.CGST + item.IGST;
                    var discount = $("#DiscountPercentageList option:selected").data('value');
                    var DiscountPercentage = $("#DiscountPercentageList option:selected").text();
                    TaxableAmt = item.GrossAmount * (discount / 100);
                    TaxableAmount = item.GrossAmount - TaxableAmt;
                    var NetAmount = TaxableAmount + GST;
                    content += '<tr>'
                        + '<td>' + (i + 1)
                        + '     <input type="hidden" class="SalesOrderTransID" value="' + item.SalesOrderTransID + '" />'
                        + '     <input type="hidden" class="ItemID" value="' + item.ItemID + '" />'
                        + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                        + ' <td >' + item.ItemName
                        + '     <input type="hidden" class="ItemID" value="' + item.UnitName + '"  />'
                        + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                        + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                        + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                        + '     <input type="hidden" class="IGST" value="' + item.IGST + '" />'
                        + '     <input type="hidden" class="SGST" value="' + item.SGST + '" />'
                        + '     <input type="hidden" class="CGST" value="' + item.CGST + '" />'
                        + '     <input type="hidden" class="Rate" value="' + item.Rate + '" />'
                        + '</td>'
                        + ' <td>' + item.Unit + '</td>'
                        + ' <td><input type="text" class="md-input Qty mask-qty" value="' + item.Qty + '"  readonly="readonly" /></td>'
                        + ' <td><input type="text" class="MRP mask-sales-currency " value="' + item.MRP + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="DiscountPercentage mask-currency" value="' + DiscountPercentage + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="DiscountAmount mask-sales-currency" value="' + discount + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="TaxableAmount mask-sales-currency" value="' + TaxableAmount + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="GST mask-currency GSTPercentage" value="' + item.GSTPercentage + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="GSTAmount mask-sales-currency" value="' + GST + '" readonly="readonly" /></td>'
                        + ' <td class="cess-enabled"><input type="text" class="CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                        + ' <td class="cess-enabled"><input type="text" class="CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="NetAmount mask-sales-currency" value="' + NetAmount + '" readonly="readonly" /></td>'
                        + ' <td><input type="text" class="Remarks md-input label-fixed" value=""/></td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $('#sales-invoice-items-list tbody').append($content);
                var count = $('#sales-invoice-items-list tbody tr').length;
                $("#item-count").val(count);
                self.calculate_grid_total();
            }
        });
    },

    calculate_discount_Amount: function () {
        var self = ServiceSalesInvoice;
        var discount = $("#DiscountPercentageList option:selected").data('value');
        var DiscountPercentage = $("#DiscountPercentageList option:selected").text();
        $("#sales-invoice-items-list tbody tr").each(function () {
            $row = $(this).closest("tr");
            GrossAmount = clean($row.find(".GrossAmount").val());
            SGSTAmt = clean($row.find(".SGST").val());
            CGSTAmt = clean($row.find(".CGST").val());
            CessAmount = clean($row.find(".CessAmount").val());
            TaxableAmt = GrossAmount * (discount / 100);
            TaxableAmt = GrossAmount - TaxableAmt;
            var NetAmount = TaxableAmt + CessAmount + CGSTAmt + SGSTAmt
            $row.find(".TaxableAmount").val(TaxableAmt);
            $row.find(".NetAmount").val(NetAmount);
            $row.find(".DiscountPercentage").val(DiscountPercentage);
            $row.find(".DiscountAmount").val(discount);
        });
        self.calculate_grid_total();
    },

    calculate_grid_total: function () {
        var self = ServiceSalesInvoice;
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var temp = 0;
        var CessAmount = 0;
        $("#sales-invoice-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            NetAmount += clean($(this).find('.NetAmount').val());
            CessAmount += clean($(this).find('.CessAmount').val());
        });
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = NetAmount - temp;
        $("#GrossAmount").val(GrossAmount);
        $("#DiscountAmount").val(DiscountAmount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
        $("#CessAmount").val(CessAmount);
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
        var self = ServiceSalesInvoice;
        if ($("#StateID").val() != $("#LocationStateID").val()) {
            return true;
        }
        return false;
    },

    set_amount_details: function () {
        var self = ServiceSalesInvoice;
        var is_igst = self.is_igst();
        var GSTPercent;
        var GSTAmount;
        var tr = "";
        var $tr;
        var TaxableAmount;
        self.Invoice.AmountDetails = [];
        $("#sales-invoice-items-list tbody tr").each(function () {
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
        $.each(self.Invoice.AmountDetails, function (i, record) {
            tr += "<tr  class='uk-text-center'>";
            tr += "<td>" + (i + 1);
            tr += "</td>";
            tr += "<td class='Particulars'>" + record.Particulars;
            tr += "</td>";
            tr += "<td class='mask-currency'>" + record.TaxableAmount;
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
        var self = ServiceSalesInvoice;
        var index = self.search_tax_group(self.Invoice.AmountDetails, GSTPercent, type);
        if (index == -1) {
            self.Invoice.AmountDetails.push({
                Percentage: GSTPercent,
                Amount: GSTAmount,
                Particulars: type,
                TaxableAmount: TaxableAmount
            });
        } else {
            self.Invoice.AmountDetails[index].Amount += GSTAmount;
            self.Invoice.AmountDetails[index].TaxableAmount += TaxableAmount;
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

    service_sales_order_list: function () {
        $list = $('#sales-order-list');
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
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/ServiceSalesOrder/GetServiceSalesUnprocessOrderList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CustomerID", Value: $('#CustomerID').val() },
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
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='checkbox' class='uk-radio ItemID' name='ItemID' data-md-icheck checked value='" + row.ID + "' >"

                        }
                    },


                    {
                        "data": "SONo",
                        "className": "SONo",
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
                }
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            $('body').on("change", '#CustomerID', function () {
                list_table.fnDraw();
            });
            return list_table;
        }
    },

    get_customers: function (release) {
        var self = SalesInvoice;
        $.ajax({
            url: '/Masters/Customer/GetServiceSalesInvoiceCustomersAutoComplete',
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
        var self = ServiceSalesInvoice;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#CustomerID").trigger("change");
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#CustomerCategoryID").val(item.customercategoryid);
        $("#MinCreditLimit").val(item.minCreditLimit);
        $("#MaxCreditLimit").val(item.maxCreditLimit);
        self.Invoice.CustomerID = item.id;
        self.customer_on_change();
    },

    select_customer: function () {
        var self = ServiceSalesInvoice;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var CustomerCategory = $(row).find(".CustomerCategoryID").val();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();

        var MinCreditLimit = $(row).find(".MinimumCreditLimit").val();
        var MaxCreditLimit = $(row).find(".MaxCreditLimit").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#CustomerID").trigger("change");
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#CustomerCategoryID").val(CustomerCategory);
        $("#MinCreditLimit").val(MinCreditLimit);
        $("#MaxCreditLimit").val(MaxCreditLimit);
        UIkit.modal($('#select-customer')).hide();
        self.Invoice.CustomerID = ID;
        self.customer_on_change();
    },

    customer_on_change: function () {
        var self = ServiceSalesInvoice;
        //$("#SalesOrderNos").focus();
        self.clear_items();
        self.get_scheme_allocation();
        self.get_discount_details();
        Sales.get_customer_addresses();
        self.get_credit_amount();
    },
    get_credit_amount: function () {
        var self = ServiceSalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesInvoice/GetCreditAmountByCustomer',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#CreditAmount").val(response.Data);

                }
            }
        });
    },
    clear_items: function () {
        var self = ServiceSalesInvoice;
        $("#sales-invoice-items-list tbody").html('');

        $("#SalesOrderNos").val('');
        $("#GrossAmount").val('');
        $("#AdditionalDiscount").val('');
        $("#DiscountAmount").val('');
        $("#TurnoverDiscount").val('');
        $("#TaxableAmount").val('');
        $("#SGSTAmount").val('');
        $("#CGSTAmount").val('');
        $("#IGSTAmount").val('');
        $("#CashDiscount").val('');
        $("#RoundOff").val('');
        $("#NetAmount").val('');
        $("#CessAmount").val('');
        self.Items = [];
        self.Invoice.GrossAmount = 0;
        self.Invoice.AdditionalDiscount = 0;
        self.Invoice.DiscountAmount = 0;
        self.Invoice.TurnoverDiscount = 0;
        self.Invoice.TaxableAmount = 0;
        self.Invoice.CGSTAmount = 0;
        self.Invoice.SGSTAmount = 0;
        self.Invoice.IGSTAmount = 0;
        self.Invoice.CashDiscount = 0;
        self.Invoice.NetAmount = 0;
        self.Invoice.RoundOff = 0;
        self.Invoice.CessAmount = 0;

    },

    // gets the scheme for the selected customer
    get_scheme_allocation: function () {
        var self = ServiceSalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesOrder/GetSchemeAllocation',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#SchemeID").val(response.SchemeID);
                    self.Invoice.SchemeID = response.SchemeID;
                }
            }
        });
    },

    get_discount_details: function () {
        var self = ServiceSalesInvoice;
        var CustomerID = $("#CustomerID").val();
        $.ajax({
            url: '/Sales/SalesInvoice/GetDiscountDetails',
            data: {
                CustomerID: CustomerID,
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.CashDiscountEnabled) {
                        $("input#CashDiscountEnabled").iCheck('enable');
                    } else {
                        $("input#CashDiscountEnabled").iCheck('disable');
                        $("input#CashDiscountEnabled").iCheck('uncheck');
                    }
                    $("#TurnoverDiscountAvailable").val(response.TurnoverDiscountAvailable);
                }
            }
        });
    },

    rules: {
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },

            {
                elements: "#PaymentModeID",
                rules: [
                     { type: form.required, message: "Please choose PaymentMode" },
                     { type: form.positive, message: "Please choose PaymentMode" },
                     { type: form.non_zero, message: "Please choose PaymentMode" }
                ]
            },
            {
                elements: "#InvoiceNo",
                rules: [
                    { type: form.required, message: "Invalid Invoice Number" },
                ]
            },
            {
                elements: "#InvoiceDate",
                rules: [
                    { type: form.required, message: "Invalid Invoice Date" },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose Customer", alt_element: "#CustomerName" },
                    { type: form.positive, message: "Please choose Customer", alt_element: "#CustomerName" },
                    { type: form.non_zero, message: "Please choose Customer", alt_element: "#CustomerName" }
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
                    {
                        type: function (element) {
                            var netamount = clean($("#NetAmount").val());
                            var min_credit_limit = clean($("#MinCreditLimit").val());
                            return netamount >= min_credit_limit
                        }, message: "Netamount deceeds minimum credit limit"
                    },
                    {
                        type: function (element) {
                            var netamount = clean($("#NetAmount").val());
                            var max_credit_limit = clean($("#MaxCreditLimit").val());
                            return netamount <= max_credit_limit
                        }, message: "Netamount exceeds maximum  credit limit"
                    },
                     {
                         type: function (element) {
                             var max_credit_limit = clean($("#MaxCreditLimit").val());
                             var netamount = clean($("#NetAmount").val());
                             var creditamount = clean($("#CreditAmount").val());
                             return (max_credit_limit >= (creditamount + netamount))
                         }, message: "Credit balance of customer  exceeds maximum  credit limit"
                     },
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
        var self = ServiceSalesInvoice;

        $('#tabs-servicesalesinvoice').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "SavedInvoices":
                $list = $('#saved-invoices-list');
                break;
            case "PartiallyReceived":
                $list = $('#partially-received-list');
                break;
            case "FullyReceived":
                $list = $('#fully-received-list');
                break;
            case "BadAndDoubtfulInvoices":
                $list = $('#bad-and-doubtful-invoices-list');
                break;
            case "cancelled":
                $list = $('#cancel-list');
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
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/ServiceSalesInvoice/GetServiceSalesInvoiceList?type=" + type,
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
                        "data": "TransNo",
                        "className": "",
                    },
                                { "data": "InvoiceDate", "className": "InvoiceDate" },
                                { "data": "CustomerName", "className": "CustomerName" },
                                { "data": "Location", "searchable": false, },
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
                        app.load_content("/Sales/ServiceSalesInvoice/Details/" + Id);
                    });
                },
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
}
