LocalPurchaseInvoice = {
    init: function () {
        var self = LocalPurchaseInvoice;
        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 2
        });
        index = $("#purchase-order-items-list tbody tr").length;
        $("#item-count").val(index);
        self.bind_events();
    },

    list: function () {
        var self = LocalPurchaseInvoice;

        $('#tabs-localpurchase').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = LocalPurchaseInvoice;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#local-purchase-draft-list');
                break;
            case "Purchased":
                $list = $('#local-purchased');
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

            var url = "/Purchase/LocalPurchaseInvoice/GetLocalPurchases?type=" + type;

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
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/LocalPurchaseInvoice/Details/" + Id);
                    });
                },
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = LocalPurchaseInvoice;
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
    },

    print: function () {
        var self = LocalPurchaseInvoice;
        $.ajax({
            url: '/Purchase/LocalPurchaseInvoice/Print',
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

    printpdf: function () {
        var self = LocalPurchaseInvoice;
        $.ajax({
            url: '/Reports/Purchase/LocalPurchaseInvoicePrintPdf',
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
       var self = LocalPurchaseInvoice;
       $("#btnOKItem").on("click", self.select_item);
       $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_item_details, 'minLength': 1 });
       $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
       $("#DDLPurchaseCategory").on('change', self.clear_item_on_select_purchasecategory);
       $('body').on('click', '#btnAddProduct', self.add_item);
       $("body").on("click", ".remove-item", self.remove_item);
       $("body").on("keyup change", "#purchase-order-items-list tbody .Qty, .Rate", self.update_amount);
       $(".btnSave, .btnSaveDraft").on("click", self.on_save);
    },
    update_amount: function () {
        var self = LocalPurchaseInvoice;
        $('#purchase-order-items-list tbody tr').each(function () {
            var qty = clean($(this).find(".Qty").val());
            var rate = clean($(this).find(".Rate").val());
            var GSTPercentage = clean($(this).find(".GSTPercentage").val());
            var Value = qty * rate;
            var GSTAmnt = 0;
            var CGST = 0;
            var total = Value + GSTAmnt;
            $(this).find(".value").val(Value);
            $(this).find(".CGST").val(CGST);
            $(this).find(".GSTAmnt").val(GSTAmnt);
            $(this).find(".TotalAmnt").val(total);
        });
        self.calculate_grid_total();
    },

    remove_item: function () {
        var self = LocalPurchaseInvoice;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#purchase-order-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#purchase-order-items-list tbody tr").length);
        self.calculate_grid_total();
    },

    get_item_details: function (release) {
        $.ajax({
            url: '/Masters/Item/ItemByServiceCategoryAndSupplierID',
            data: {
                Area: "Stock",
                NameHint: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },

    set_item_details: function (event, item) {
        var self = LocalPurchaseInvoice;
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#UnitID").val(item.purchaseUnitId);
        $("#LastPr").val(item.lastPr);
        $("#LowestPr").val(item.lowestPr);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CategoryID").val(item.itemCategory);
        $("#Qty").focus();
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#PurchaseUnit").val(item.purchaseUnit);
        $("#PurchaseUnitID").val(item.purchaseUnitId);
        $("#DDLPurchaseCategory").val(item.purchaseCategoryId)
        self.get_units();
    },

    clear_item_on_select_purchasecategory: function () {
        var self = LocalPurchaseInvoice;
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PrimaryUnit").val('');
        $("#PrimaryUnitID").val('');
        $("#PurchaseUnit").val('');
        $("#PurchaseUnitID").val('');
        self.get_units();
    },

    select_item: function () {
        var self = LocalPurchaseInvoice;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var PrimaryUnit = $(row).find(".PrimaryUnit").val();
        var PrimaryUnitID = $(row).find(".PrimaryUnitID").val();
        var PurchaseUnit = $(row).find(".PurchaseUnit").val();
        var PurchaseUnitID = $(row).find(".PurchaseUnitID").val();
        var lastPr = $(row).find(".lastPr").val();
        var lowestPr = $(row).find(".lowestPr").val();
        var Category = $(row).find(".ItemCategory").val();
        var pendingOrderQty = $(row).find(".pendingOrderQty").val();
        var qtyWithQc = $(row).find(".qtyWithQc").val();
        var qtyAvailable = $(row).find(".qtyAvailable").val();
        var gstPercentage = $(row).find(".gstPercentage").val();
        var PurchaseCategoryID = $(row).find(".PurchaseCategoryID").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Rate").val('');
        $("#PrimaryUnit").val(PrimaryUnit);
        $("#PrimaryUnitID").val(PrimaryUnitID);
        $("#PurchaseUnit").val(PurchaseUnit);
        $("#PurchaseUnitID").val(PurchaseUnitID);
        $("#LastPr").val(lastPr);
        $("#LowestPr").val(lowestPr);
        $("#QtyWithQc").val(qtyWithQc);
        $("#QtyAvailable").val(qtyAvailable);
        $("#GSTPercentage").val(gstPercentage);
        $("#DDLPurchaseCategory").val(PurchaseCategoryID);
        $("#Qty").focus();
        self.get_units();
        UIkit.modal($('#select-item')).hide();

    },

    get_units: function () {
        var self = LocalPurchaseInvoice;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);
    },

    add_item: function () {
        var self = LocalPurchaseInvoice;
        var FormType = "Add";
        if (self.validate_form(FormType) > 0) {
            return;
        }
        var PurchaseCategoryID = $("#DDLPurchaseCategory").val();
        var ItemID = $("#ItemID").val();
        var ItemName = $("#ItemName").val();
        var UnitID = $("#UnitID").val();
        var Unit = $("#UnitID Option:selected").text();
        var Qty = clean($("#Qty").val());
        var GSTPercentage = clean($("#GSTPercentage").val());
        var Rate = clean($("#Rate").val());
        var value = (Qty * Rate);
        var Remarks = $("#ItemRemarks").val();
        var IsGSTRegistered = $("#IsGSTRegistered").val();
        var GSTAmnt = 0;
        var CGST = 0;
        
        var CGSTPercent = (GSTPercentage / 2);
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
            + '<input type="hidden" class = "CGST" value="' + CGST + '" />'
            + '<input type="hidden" class = "CGSTPercent" value="' + CGSTPercent + '" />'
            + '</td>'
            + '<td class="Unit">' + Unit + '</td>'
            + '<td><input type="text" class = "md-input mask-production-qty Qty" value="' + Qty + '" /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty Rate" value="' + Rate + ' " /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty value" value="' + value + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty GSTPercentage" value="' + GSTPercentage + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty GSTAmnt" value="' + GSTAmnt + '" disabled /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty TotalAmnt" value="' + TotalAmnt + '" disabled /></td>'
            + '<td class="Remarks">' + Remarks + '</td>'
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
    },

    calculate_grid_total: function () {
        var NetAmount = 0;
        var GrossAmnt = 0;
        var GSTAmount = 0;
        $('#purchase-order-items-list tbody tr').each(function () {
            var total =clean($(this).find(".TotalAmnt").val());
            var GST = 0;
            var GrossAmount = clean($(this).find(".value").val());
            GrossAmnt = GrossAmnt + GrossAmount;
            GSTAmount = GSTAmount + GST;
            NetAmount = NetAmount + total;
        });
        var CGST = GSTAmount / 2
        $("#GSTAmount").val(CGST);
        $("#GrossAmnt").val(GrossAmnt);
        $("#NetAmount").val(NetAmount);
       
    },

    clear: function () {
        var self = LocalPurchaseInvoice;
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#PrimaryUnit").val('');
        $("#PrimaryUnitID").val('');
        $("#PurchaseUnit").val('');
        $("#PurchaseUnitID").val('');
        $("#DDLPurchaseCategory").val('');
        $("#UnitID").val('');
        $("#Rate").val('');
        $("#GSTPercentage").val('');
        $("#Qty").val('');
        $("#ItemRemarks").val('');
        self.get_units();
    },



    validate_form: function (FormType) {
        var self = LocalPurchaseInvoice;
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

    on_save: function () {

        var self = LocalPurchaseInvoice;
        var model = self.get_data();
        var location = "/Purchase/LocalPurchaseInvoice/Index";
        var url = '/Purchase/LocalPurchaseInvoice/Save';
        var FormType = "Form";

        if ($(this).hasClass("btnSaveDraft")) {
            model.IsDraft = true;
            url = '/Purchase/LocalPurchaseInvoice/SaveAsDraft'
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
        var self = LocalPurchaseInvoice;
        $(".btnSave,.btnSaveDraft").css({ 'display': 'none' });
        $.ajax({
            url: '/Purchase/LocalPurchaseInvoice/Save',
            data: model,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSave,.btnSaveDraft").css({ 'display': 'block' });
                }
            }
        });
    },

    get_data: function () {
        var self = LocalPurchaseInvoice;
        var data = {};
        data.PurchaseOrderNo = $("#PurchaseOrderNo").val();
        data.ID = $("#ID").val();
        data.SupplierID = $("#SupplierID").val();
        data.PurchaseOrderDate = $("#PurchaseOrderDate").val();
        data.SupplierReference = $("#SupplierReference").val();
        data.NetAmount = clean($("#NetAmount").val());
        data.GSTAmount = clean($("#GSTAmount").val());
        data.GrossAmnt = clean($("#GrossAmnt").val());
        data.Items = [];
        var item = {};
        $('#purchase-order-items-list tbody tr ').each(function () {
            item = {};
            item.ItemID = $(this).find(".ItemID").val();
            item.UnitID = $(this).find(".UnitID").val();
            item.PurchaseCategoryID = $(this).find(".PurchaseCategoryID").val();
            item.Qty = clean($(this).find(".Qty").val());
            item.Rate = clean($(this).find(".Rate").val());
            item.Value = clean($(this).find(".value").val());
            item.SGSTPercent = clean($(this).find(".CGSTPercent").val());
            item.CGSTPercent = clean($(this).find(".CGSTPercent").val());
            item.GSTPercentage = clean($(this).find(".GSTPercentage").val());
            item.GSTAmount = 0;
            item.CGSTAmount = 0;
            item.SGSTAmount = 0;
            item.TotalAmount = clean($(this).find(".TotalAmnt").val());
            item.Remarks = $(this).find(".Remarks").text();
            data.Items.push(item);
        });
        return data;
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
                elements: "#SupplierReference",
                rules: [
                    { type: form.required, message: "Please enter supplierReference" },
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
        ],

    },
   

    

   

}