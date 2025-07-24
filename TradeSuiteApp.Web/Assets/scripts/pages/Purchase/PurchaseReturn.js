var item_list;
var freeze_header;
purchase_return = {

    init: function () {
        var self = purchase_return;
        supplier.supplier_list('GRNWise');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#InvoiceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        item_list = Item.grn_wise_item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Premises",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        $('#pro-popup-list').SelectTable({
            selectFunction: self.remove_item_details,
            modal: "#get-PRO",
            initiatingElement: "#InvoiceNo",
            selectionType: "checkbox",
            startFocusIndex: 3,
        });

       // freeze_header = $("#tblPurchaseReturnItems").FreezeHeader();
        self.invoice_list();
        self.bind_events();
        //self.get_invoice();   
    },

    details: function () {
       // freeze_header = $("#tblPurchaseReturnItems").FreezeHeader();

    },

    invoice_list: function () {
        var self = purchase_return;
        $list = $('#pro-popup-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            self.invoice_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "bSort": false

            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    list: function () {
        var self = purchase_return;
        $('#tabs-PurchaseReturn').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "purchase-return":
                $list = $('#purchase-return-list');
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

            var url = "/Purchase/PurchaseReturn/GetPurchaseReturn?type=" + type;
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[2, "desc"]],
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
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "SupplierName", "className": "SupplierName" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseReturn/Details/" + Id);

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
        var self = purchase_return

        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKSupplier").on("click", self.select_supplier);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#SupplierName").on("change", self.clearItemSelectControls);
        $("body").on("keyup change", ".clReturnQty", self.check_stock_quantity);
        $("body").on("click", ".remove-item", self.remove_item);
        $("#btnOkInvoiceList").on("click", self.remove_item_details);
        $("body").on("change", "#tblPurchaseReturnItems tbody .Unit", self.check_stock_quantity);
        $(".btnSaveAndNew, .btnSaveASDraft").on("click", self.on_save);


    },

    remove_item_details: function () {
        var self = purchase_return;
        if ($("#tblPurchaseReturnItems tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                self.count_items();
                self.select_order();
            })
        }
        else {
            self.select_order();
        }
    },
    remove_item: function () {
        var self = purchase_return;
        $(this).closest("tr").remove();
        freeze_header.resizeHeader();
        var igst, cgst, sgst, total, rate, acceptedqty, sumigst = 0, sumsgst = 0, sumcgst = 0, netamount = 0;
        var Stock, ReturnQty, igstPercent, cgstPercent, sgstPercent;
        var IGST, CGST, SGST;

        $("#tblPurchaseReturnItems tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#tblPurchaseReturnItems tbody tr").length);
        $("#tblPurchaseReturnItems tbody tr").each(function () {
            Stock = clean($(this).find('.Stock').val());
            ReturnQty = clean($(this).find('.clReturnQty').val());
            acceptedqty = clean($(this).find('.clAcptQty').val());
            rate = clean($(this).find('.clRate ').val());
            igstPercent = clean($(this).find('.clIgstPercentage').val());
            cgstPercent = clean($(this).find('.clCgstPercentage').val());
            sgstPercent = clean($(this).find('.clSgstPercentage').val());
            total = rate * ReturnQty;
            igst = igstPercent * total / 100;
            cgst = cgstPercent * total / 100;
            sgst = sgstPercent * total / 100;
            netamount += total + igst + cgst + sgst;
            sumcgst += cgst;
            sumigst += igst;
            sumsgst += sgst;
            $("#IGSTAmount").val(sumigst);
            $("#CGSTAmount").val(sumcgst);
            $("#SGSTAmount").val(sumsgst);
            $("#txtNetAmount").val(netamount);
        })
        $("#txtNetAmount").val(netamount);
    },

    select_order: function () {
        var self = purchase_return;
        var order_ids = $("#pro-popup-list .CheckItem:checked").map(function () {
            return $(this).val();
        }).get();
        var PurchaseInvoiceIDS = [];
        var igst, cgst, sgst, total, rate, acceptedqty, sumigst = 0, sumsgst = 0, sumcgst = 0, netamount = 0;
        if (order_ids.length == 0) {
            app.show_error('Please select atleast one invoice');
            return;
        }
        $.ajax({
            url: '/Purchase/PurchaseReturnOrder/GetPurchaseReturnOrder/',
            dataType: "json",
            data: {
                OrderIdS: order_ids,
            },
            type: "POST",
            success: function (return_items) {
                var return_items_list = $('#tblPurchaseReturnItems tbody');
                return_items_list.html('');
                var tr = '';
                var html = '';
                var SerialNo = $(".tbody .rowPr").length;
                $.each(return_items, function (i, item) {
                    if (PurchaseInvoiceIDS.indexOf(item.InvoiceNo) == -1) {
                        PurchaseInvoiceIDS.push(item.InvoiceNo);
                    }
                    rate = item.Rate;
                    acceptedqty = item.AcceptedQty;
                    total = rate * acceptedqty;
                    igst = item.IGSTPercent * total / 100;
                    cgst = item.CGSTPercent * total / 100;
                    sgst = item.SGSTPercent * total / 100;
                    sumcgst += cgst;
                    sumigst += igst;
                    sumsgst += sgst;
                    $("#IGSTAmount").val(sumigst);
                    $("#CGSTAmount").val(sumcgst);
                    $("#SGSTAmount").val(sumsgst);
                    netamount += total + igst + sgst + cgst;
                    total = total + igst + sgst + cgst;
                    $("#txtNetAmount").val(netamount);
                    SerialNo++;
                    var gstPercent = item.IGSTPercent + item.CGSTPercent + item.SGSTPercent;

                    html += '  <tr class="rowPr">'
                              + ' <td class="uk-text-center">' + SerialNo + '</td>'
                              + ' <td class="clProduct">' + item.ItemName
                              + '<input type="hidden" class="ItemID" value="' + item.ItemID + '" />'
                              + '<input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                              + '<input type="hidden" class="Stock" value="' + item.Stock + '" />'
                              + '<input type="hidden" class="WareHouseID" value="' + item.WarehouseID + '" />'
                              + '<input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                              + '<input type="hidden" class="Qty" value="' + item.AcceptedQty + '" />'
                              + '<input type="hidden" class="PurchaseReturnOrderID" value="' + item.PurchaseReturnOrderID + '" />'

                                + '<input type="hidden" class="clSgstPercentage" value="' + item.SGSTPercent + '" />'
                              + '<input type="hidden" class="clCgstPercentage" value="' + item.CGSTPercent + '" />'
                              + '<input type="hidden" class="clIgstPercentage" value="' + item.IGSTPercent + '" />'


                              + '<input type="hidden" class="PurchaseReturnOrderTransID" value="' + item.PurchaseReturnOrderTransID + '" /></td>'
                              + ' <td class="clGrn">' + item.ReturnNo + '</td>'
                              + '<input type="hidden" class="InvoiceID" value="' + item.InvoiceID + '" /></td>'
                              + ' <td class="clUnit">' + item.Unit + '</td>'
                              + ' <td class="clAcptQty mask-qty">' + item.AcceptedQty + '</td>'
                              + ' <td class="clGrnQty mask-qty">' + item.GRNQty + '</td>'
                              //+ ' <td ><input type="text"  class="md-input mask-qty clReturnQty" value="' + item.AcceptedQty + '" /></td>'
                              + ' <td  class="clReturnQty  mask-qty">' + item.AcceptedQty + '</td>'
                              + ' <td class="clRate mask-currency">' + item.Rate + '</td>'
                              + ' <td class=" mask-qty">' +gstPercent + '</td>'   
                              + ' <td class="clSgstAmount mask-currency">' + sgst + ' </td>'                         
                              + ' <td class="clCgstAmount mask-currency"> ' + cgst + ' </td>'
                              + ' <td class="clIgstAmount mask-currency">' + igst + ' </td>'
                              + ' <td class="clTotal mask-currency">' + total + ' </td>'
                              + ' <td ><input type="text"  class="md-input clRemarks"  /></td>'
                              + ' <td class="uk-text-center" >'
                              + ' <a class="remove-item" >'
                              + '<i class="md-btn-icon-small uk-icon-remove"></i>'
                              + ' </a>'
                              + ' </td>'
                    '</tr>';
                });

                var $html = $(html);
                app.format($html);
                return_items_list.append($html);
                //setTimeout(function () {
                //    freeze_header.resizeHeader();
                //}, 500);
                $("#InvoiceNo").val(PurchaseInvoiceIDS.join(','));
                self.count_items();
                $("#ItemName").focus();
            },
        });
    },

    count_items: function () {
        var count = $('#tblPurchaseReturnItems tbody tr').length;
        $('#item-count').val(count);
    },


    check_stock_quantity: function () {
        var self = purchase_return;
        var row = $(this).closest('tr');
        var stock = clean($(row).find('.Stock').val());
        var returnQty = clean($(row).find('.clReturnQty').val());
        var grnQty = clean($(row).find('.clGrnQty').val());
        var rate = clean($(row).find('.clRate ').val());
        var igstPercent = clean($(row).find('.clIgstPercentage').val());
        var cgstPercent = clean($(row).find('.clCgstPercentage').val());
        var sgstPercent = clean($(row).find('.clSgstPercentage').val());
        var unitID = clean($(row).find(".Unit option:selected").val());
        var primaryUnitID = clean($(row).find('.PrimaryUnitID').val());
        var purchaseUnitID = clean($(row).find('.PurchaseUnitID').val());
        var convertedStock = clean($(row).find('.ConvertedStock').val());
        var convertedQty = clean($(row).find('.ConvertedQty').val());
        var quantity = clean($(row).find('.Qty').val());
        var transUnitID = clean($(row).find('.UnitID').val());
        var IGST, CGST, SGST, total, netamount = 0, igst, cgst, sgst;
        total = rate * returnQty;
        igst = igstPercent * total / 100;
        cgst = cgstPercent * total / 100;
        sgst = sgstPercent * total / 100;
        netamount += total + igst + cgst + sgst;

        if (returnQty > stock) {
            app.show_error('Selected item dont have enough stock ');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else if (returnQty > grnQty) {
            app.show_error('Return quantity should be less than or equal to grn quantity ');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else {
            $(row).find('.clSgstAmount').val(sgst);
            $(row).find('.clCgstAmount ').val(cgst);
            $(row).find('.clIgstAmount ').val(igst);
            $(row).find('.clTotal ').val(total);
        }
        self.claculate_total();
    },
    claculate_total: function () {
        var netamount = 0, igst, cgst, sgst, total;
        $("#tblPurchaseReturnItems tbody tr").each(function () {

            total = clean(($(this)).find('.clTotal').val());
            igst = clean($(this).find('.clIgstAmount').val());
            cgst = clean($(this).find('.clCgstAmount').val());
            sgst = clean($(this).find('.clSgstAmount').val());
            netamount += total + igst + cgst + sgst;
        })
        $("#txtNetAmount").val(netamount);
    },

    on_save: function () {

        var self = purchase_return;
        var model = self.get_data();
        var location = "/Purchase/PurchaseReturn/Index";
        var url = '/Purchase/PurchaseReturn/Create';

        if ($(this).hasClass("btnSaveASDraft")) {
            model.IsDraft = true;
            url = '/Purchase/PurchaseReturn/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            self.error_count = self.validate_form();
        }

        if (self.error_count > 0) {
            return;
        }

        if (!model.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save_and_new(model, url, location);
            }, function () {
            })
        } else {
            self.save_and_new(model, url, location);
        }
    },

    save_and_new: function (model, url, location) {
        var self = purchase_return;

            $(".btnSaveAndNew,.btnSaveASDraft").css({ 'display': 'none' });
            $.ajax({
                url: url,
                data: { model: model },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data == "success") {
                        app.show_notice("Purchase return created successfully");
                        setTimeout(function () {
                            window.location = location;
                        }, 1000);
                    } else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        $(".btnSaveAndNew,.btnSaveASDraft").css({ 'display': 'block' });
                    }
                },
            });
    },
 
    get_data: function (IsDraft) {
        self = purchase_return;
        var model = {
            ID: $("#ID").val(),
            ReturnNo: $("#txtReturnNo").val(),
            SupplierID: $("#SupplierID").val(),
            ReturnDate: $("#ReturnDate").val(),
            NetAmount: clean($("#txtNetAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            SGSTPercent: clean($("#SGSTPercentage").val()),
            IGSTPercent: clean($("#IGSTPercentage").val()),
            CGSTPercent: clean($("#CGSTPercentage").val()),
            IsDraft: IsDraft,
            ReturnItems: self.GetProductList(),
        };
        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var i = 0;
        $(".tbody .rowPr").each(function () {
            i++;
            ProductsArray.push({
                ItemID: $(this).find('.ItemID').val(),
                InvoiceID: $(this).find('.InvoiceID').val(),
                Quantity: clean($(this).find('.clReturnQty').val()),
                Rate: clean($(this).find('.clRate').text()),
                SGSTPercent: clean($(this).find('.clSgstPercentage').val()),
                CGSTPercent: clean($(this).find('.clCgstPercentage').val()),
                IGSTPercent: clean($(this).find('.clIgstPercentage').val()),
                SGSTAmount: clean($(this).find('.clSgstAmount').text()),
                CGSTAmount: clean($(this).find('.clCgstAmount').text()),
                IGSTAmount: clean($(this).find('.clIgstAmount').text()),
                Amount: clean($(this).find('.clTotal').text()),
                Remarks: $(this).find('.clRemarks').val(),
                WarehouseID: $(this).find('.WareHouseID').val(),
                BatchTypeID: $(this).find('.BatchTypeID').val(),
                UnitID: clean($(this).find('.UnitID').val()),
                InvoiceID: clean($(this).find('.InvoiceID').val()),
                PurchaseReturnOrderID: clean($(this).find('.PurchaseReturnOrderID').val()),
                PurchaseReturnOrderTransID: clean($(this).find('.PurchaseReturnOrderTransID').val()),
            });
        })
        return ProductsArray;
    },
    clearControls: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#UnitID").val('');
        $("#UnitName").val(' ');
        $("#Stock").val(' ');
        $("#Premises").val('');
        $("#GRNno").val('');
        $("#AcceptedQty").val(' ');
        $("#Rate").val(' ');
        $("#SGSTPercentage").val(' ');
        $("#CGSTPercentage").val(' ');
        $("#IGSTPercentage").val(' ');
        $("#Remark").val(' ');
        $("#ReturnQty").val(' ');
        $('#tblPurchaseReturnItems tbody').html('')
    },
    calculate_net_amount: function () {
        var packing_charge = clean($("#txtPackingCharges").val());
        var freight = clean($("#txtFreight").val());
        var other_charges = clean($("#txtOtherCharges").val());
        var total = packing_charge + freight + other_charges;
        $(".clTotal").each(function () {
            total += clean($(this).text());
        });
        $("#txtNetAmount").val(total);

    },

    set_supplier_details: function (event, item) {
        self = purchase_return;
        if (($("#tblPurchaseReturnItems tbody tr").length > 0 && (item.id != $("#SupplierID").val()))) {
            app.confirm("Selected items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                $('#itempopup-list tbody').empty();
                $("#SupplierName").val(item.name);
                $("#SupplierLocation").val(item.location);
                $("#SupplierID").val(item.id);
                $("#StateId").val(item.stateId);
                $("#IsGSTRegistered").val(item.isGstRegistered);
                $("#SupplierReferenceNo").focus();
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(item.name);
            $("#SupplierLocation").val(item.location);
            $("#SupplierID").val(item.id);
            $("#StateId").val(item.stateId);
            $("#IsGSTRegistered").val(item.isGstRegistered);
            $("#SupplierReferenceNo").focus();
            self.get_invoice();
        }


    },
    get_invoice: function () {
        var self = purchase_return
        $('#pro-popup-list tbody').empty();
        var total = 0;
        self.error_count = 0;
        self.error_count = self.validate_select_invoice();
        if (self.error_count > 0) {
            return;
        }
        var SupplierID = $("#SupplierID").val();
        $("#InvoiceNo").val("");

        $.ajax({
            url: '/Purchase/PurchaseReturnOrder/GetPurchaseReturnOrderList/',
            dataType: "json",
            type: "GET",
            data: {
                SupplierID: SupplierID,
            },
            success: function (response) {
                self.invoice_list_table.fnDestroy();
                var $invoice_list = $('#pro-popup-list tbody');
                $invoice_list.html('');
                var tr = '';
                $.each(response, function (i, invoice) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center ' data-md-icheck><input type='checkbox'  class='CheckItem' value=" + invoice.ID + " /></td>"
                        + "<td>" + invoice.ReturnNo + "</td>"
                        + "<td>" + invoice.ReturnDate + "</td>"
                        + "<td><div class='mask-currency'>" + invoice.NetAmount + "</div></td>"
                    + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $invoice_list.append($tr);
                });
                self.invoice_list();
            },
        });
    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetGRNWiseSupplierForAutoComplete',
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
    select_supplier: function (event) {

        var self = purchase_return;

        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        if ((($("#tblPurchaseReturnItems tbody tr").length > 0) && (ID != $("#SupplierID").val()))) {
            app.confirm("Selected items will be removed", function () {
                $('#tblPurchaseReturnItems tbody').empty();
                $("#SupplierName").val(Name);
                $("#SupplierLocation").val(Location);
                $("#SupplierID").val(ID);
                $("#StateId").val(StateID);
                $("#IsGSTRegistred").val(IsGSTRegistered);
                self.get_invoice();
            })
        }
        else {
            $("#SupplierName").val(Name);
            $("#SupplierLocation").val(Location);
            $("#SupplierID").val(ID);
            $("#StateId").val(StateID);
            $("#IsGSTRegistred").val(IsGSTRegistered);
            self.get_invoice();
        }

        UIkit.modal($('#select-supplier')).hide();

        self.clearControls();
    },

    validate_form: function () {
        var self = purchase_return;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },


    validate_select_invoice: function () {
        var self = purchase_return;
        if (self.rules.on_select_invoices.length) {
            return form.validate(self.rules.on_select_invoices);
        }
        return 0;
    },
    rules: {

        on_blur: [],
        on_submit: [
           {
               elements: ".clReturnQty",
               rules: [
                       {
                           type: function (element) {
                               var error = false;
                               $('.clReturnQty').each(function () {
                                   var row = $(this).closest('tr');
                                   var returnqty = clean($(row).find('.clReturnQty').val());
                                   var AvailableStock = clean($(row).find('.Stock').val());
                                   var accepted = clean($(row).find('.clAcptQty ').val());
                                   if (returnqty > AvailableStock)
                                       error = true;

                               });
                               return !error;
                           }, message: 'Selected item dont have enough stock'
                       },
                       {
                           type: function (element) {
                               var error = false;
                               $('.clReturnQty').each(function () {
                                   var row = $(this).closest('tr');
                                   var returnqty = clean($(row).find('.clReturnQty').val());
                                   if (returnqty <= 0) {
                                       error = true;
                                   }

                               });
                               return !error;
                           }, message: 'Return quantity must be greater than zero'
                       },
                       {
                           type: function (element) {
                               var error = false;
                               $('.clReturnQty').each(function () {
                                   var row = $(this).closest('tr');
                                   var returnqty = clean($(row).find('.clReturnQty').val());
                                   var AvailableStock = clean($(row).find('.Stock').val());
                                   var accepted = clean($(row).find('.clGrnQty').val());
                                   if (returnqty > accepted)
                                       error = true;

                               });
                               return !error;
                           }, message: 'Return quantity should be less than or equal to grn quantity'
                       },
               ]
           },
           {
               elements: "#txtFreight",
               rules: [
                      { type: form.positive, message: "Invalid freight" },
               ]
           },
           {
               elements: "#txtOtherCharges",
               rules: [
                      { type: form.positive, message: "Invalid other charges" },
               ]
           },
           {
               elements: "#txtPackingCharges",
               rules: [
                      { type: form.positive, message: "Invalid packing charges" },
               ]
           },
           {
               elements: "#item-count",
               rules: [
                   { type: form.required, message: "Please add atleast one item" },
                   { type: form.non_zero, message: "Please add atleast one item" },
               ]
           },
           {
               elements: "#item-count",
               rules: [
                   { type: form.required, message: "Please add atleast one item" },
                   { type: form.non_zero, message: "Please add atleast one item" },
               ]
           },
        ],
        on_select_invoices: [

        {
            elements: "#SupplierID",
            rules: [
                  { type: form.required, message: "Please choose supplier" },
                  {
                      type: function (element) {
                          return ($(element).val() > 0)
                      }, message: 'Please select supplier'
                  },
            ]
        },
        ],

    },


}








