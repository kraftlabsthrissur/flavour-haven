
var FatList = [];
var SupplierList = [];
var freeze_header;
milk_purchase = {

    init: function () {
        var self = milk_purchase;
        self.purchase_requisition_list();
        $('#purchase-requisition-list').SelectTable({
            selectFunction: self.select_puchase_requisitions,
            returnFocus: "#GST",
            modal: "#select-milk-pr",
            initiatingElement: "#PurchaseRequisitionIDS",
            selectionType: "checkbox"
        });

        supplier.supplier_list('milk');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#slip_number",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });

        self.freeze_header();
        milk_purchase.bind_events();
    },

    list: function () {
        var self = milk_purchase;
        $('#tabs-module').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = milk_purchase;
        var $list;

        switch (type) {
            case "Today":
                $list = $('#Today-list');
                break;
            case "Invoiced":
                $list = $('#Invoiced-list');
                break;
            case "ToBeInvoiced":
                $list = $('#ToBeInvoiced-list');
                break;
            case "PartiallyInvoiced":
                $list = $('#PartiallyInvoiced-list');
                break;
            default:
                $list = $('#Today-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Purchase/MilkPurchase/GetMilkPurchaseList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "asc"]],
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
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "SupplierName", "className": "SupplierName" },
                    {
                        "data": "TotalQty",
                        "className": "Quantity",
                        "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.TotalQty + "</div>";
                        }
                    },
                    {
                        "data": "TotalAmount", "searchable": false, "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.TotalAmount + "</div>";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/MilkPurchase/Details/" + Id);
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
        var self = milk_purchase;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = milk_purchase;
        $.ajax({
            url: '/Reports/Purchase/MilkPurchasePrintPdf',
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

    freeze_header: function () {
        freeze_header = $("#milk-purchase-item-list").FreezeHeader();
    },

    purchase_requisition_list: function () {
        var $list = $('#purchase-requisition-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = milk_purchase;
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier);
        $("#btnOKSupplier").on("click", self.select_supplier);

        $("#btnAddMilk").on('click', milk_purchase.add_item);

        $("#FatRange").on('change', milk_purchase.get_milk_rate);
        $("body").on('ifChanged', '.chkCheck', milk_purchase.include_item);

        $("#btnOkPrList").on('click', milk_purchase.select_puchase_requisitions);
        $("#txtQty").on("keyup", self.milk_calculate_amount_by_fatRate);
        $("body").on("keyup", ".txtQty", self.calculate_item_amount_by_row);

        $(".btnSaveMilk, .btnSaveMilkAndNew,.btnSaveAndDraft").on("click", milk_purchase.on_save);

    },


    select_puchase_requisitions: function () {
        var UnProcPrList = "";
        var pr_numbers = "";
        var row;
        var total_quantity = 0;
        $("#purchase-requisition-list tr .clChkItem:checked").each(function () {
            row = $(this).closest("tr");
            total_quantity += clean($(row).find(".quantity").text());
            UnProcPrList += ($(row).find(".clprIdItem").val() + ",");
            pr_numbers += $(row).find(".prnumber").text() + ", ";
        });
        $("#PurchaseRequisitionIDS").val(pr_numbers);
        $("#RequiredQty").val(total_quantity);

        $("#RequisitionIDs").val(UnProcPrList);
    },

    get_suppliers: function (release) {
        var obj;
        $.ajax({
            url: '/Masters/Supplier/GetMilkSupplierAutoComplete',
            data: {
                NameHint: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                release(response.Data);
            }
        });
    },

    set_supplier: function (event, item) {
        if ($('#milk-purchase-item-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                $("#SupplierName").val(item.name);
                $("#SupplierID").val(item.id);
                $('#Code').val(item.code);
                $("#milk-purchase-item-list tbody").html("");
            })
        } else {
            $("#SupplierName").val(item.name);
            $("#SupplierID").val(item.id);
            $('#Code').val(item.code);
        }
    },

    select_supplier: function () {
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();

        if ($('#milk-purchase-item-list tbody tr').length > 0) {
            app.confirm("Selected items will be removed", function () {
                $('#SupplierName').val(Name);
                $('#SupplierID').val(ID);
                $('#Code').val(Code);
                $("#milk-purchase-item-list tbody").html("");
            })
        } else {
            $('#SupplierName').val(Name);
            $('#SupplierID').val(ID);
            $('#Code').val(Code);
        }
        UIkit.modal($('#select-supplier')).hide();
    },

    get_milk_rate: function () {
        self = milk_purchase;
        var hour = new Date().getHours();
        if (hour > 11) {
            $("#Remarks").val("PM")
        }
        else {
            $("#Remarks").val("AM")
        }
        var price = $("#FatRange option:selected").data("price");
        $("#rate").val(price)
        //  $("#txtQty").focus();
        self.milk_calculate_amount_by_fatRate();
    },

    milk_calculate_amount_by_fatRate: function () {
        var FatRate = clean($("#rate").val());
        var QTY = clean($("#txtQty").val());
        $("#amount").val(FatRate * QTY);
    },

    add_item: function (PrRowClass) {
        var self = milk_purchase;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count == 0) {
            var SerialNo = $("#milk-purchase-item-list tbody tr").length + 1;
            var html = '<tr class="included">' +
                        ' <td class="uk-text-center width-10">' + SerialNo + '</td>' +
                        '<td class="uk-text-center checked chkValid" data-md-icheck><input type="checkbox" class="chkCheck"  checked/></td>' +
                        ' <td class="slip_number">' + $("#slip_number").val() + '</td>' +
                        ' <td class="">' + $("#FatRange option:selected").text() +
                        '   <input type="hidden" class="fatId" value="' + $("#FatRange").val() + '" />' +
                        ' </td>' +
                        ' <td class="uk-text-right width-40"><input type="text" class="md-input mask-qty txtQty included" value="' + $("#txtQty").val() + '" /></td>' +
                        ' <td class=" width-40"> <input type="text" class="md-input rate mask-currency" value ="' + $("#rate").val() + '" readonly="readonly" /></td>' +
                        ' <td class="width-40"> <input type="text" class="md-input amount mask-currency" value="' + $("#amount").val() + '" readonly="readonly" /></td>' +
                         //' <td class="uk-text-center width-10 Remarks">' + $("#Remarks").val() + '</td>' +
                          ' <td class="width-40"> <input type="text" class="md-input Remarks" value="' + $("#Remarks").val() + '"  /></td>' +
                        '</tr>';
            var $html = $(html);
            app.format($html);
            $("#milk-purchase-item-list tbody").append($html);
            self.calculate_total_amount_and_quantity();
            self.clear_item_select_controls();
            $("#slip_number").focus();
            freeze_header.resizeHeader();
            self.count_items();
        }
    },

    validate_item: function () {
        var self = milk_purchase;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('disabled');
        milk_purchase.count_items();
    },

    count_items: function () {
        var count = $('#milk-purchase-item-list tbody').find('.chkCheck:checked').length;
        $('#item-count').val(count);
        milk_purchase.calculate_total_amount_and_quantity();
    },

    validate_form: function () {
        var self = milk_purchase;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
    },

    rules: {
        on_add: [
            {
                elements: ("#SupplierName"),
                rules: [
                    { type: form.required, message: "Please select supplier" }
                ]
            },
            {
                elements: ("#SupplierID"),
                rules: [
                    { type: form.required, message: "Please select supplier" },
                    { type: form.non_zero, message: "Please select supplier" }
                ]
            },
            {
                elements: ("#txtQty"),
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Please enter valid quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },
            {
                elements: ("#rate"),
                rules: [
                    { type: form.required, message: "Please enter rate" }
                ]
            },
            {
                elements: ("#FatRange"),
                rules: [
                    { type: form.required, message: "Please select fat range" },
                    { type: form.non_zero, message: "Please select fat range" }
                ]
            },
           {
               elements: "#slip_number",
               rules: [
                   { type: form.required, message: "Please enter slip number" },

               ]
           }
        ],
        on_submit: [
            {
                elements: ".txtQty.included",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Please enter valid quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },
            {
                elements: ".rate.included",
                rules: [
                    { type: form.required, message: "Please enter rate" }
                ]
            },
            {
                elements: "#totalAmount",
                rules: [
                    { type: form.non_zero, message: "Invalid total amount" },
                    { type: form.numeric, message: "Invalid total amount" },
                    { type: form.positive, message: "Invalid total amount" },
                ]
            },
            {
                elements: "#totalQty",
                rules: [
                    { type: form.numeric, message: "Invalid total quantity" },
                    { type: form.positive, message: "Please enter positive total quantity" },
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
    },

    calculate_item_amount_by_row: function () {
        e = $(this).closest('tr');
        var Rate = clean(e.find(".rate").val());
        var Qty = clean(e.find(".txtQty").val());
        e.find(".amount").val(Rate * Qty);
        milk_purchase.calculate_total_amount_and_quantity();
    },

    calculate_total_amount_and_quantity: function () {
        var total_amount = 0;
        var total_quantity = 0;
        $("#milk-purchase-item-list tr.included .amount").each(function () {
            total_amount += clean($(this).val());
        });
        $("#milk-purchase-item-list tr.included .txtQty").each(function () {
            total_quantity += clean($(this).val());
        });
        $("#totalAmount").val(total_amount);
        $("#totalQty").val(total_quantity);

    },

    clear_item_select_controls: function () {
        $("#slip_number").val('');
        $("#txtQty").val('');
        $("#rate").val('');
        $("#amount").val('');
        $("#FatRange").val(0);     
    },

    get_milk_master_model: function () {
        var model = {
            TransNo: $("#TransNo").val(),
            DateString: $("#DateString").val(),
            TotalAmount: clean($("#totalAmount").val()),
            TotalQty: clean($("#totalQty").val()),
            ID: $("#ID").val(),
            RequisitionIDs: $("#RequisitionIDs").val(),
            DirectInvoice: $("#DirectInvoice").is(":checked") ? true : false
        }
        return model;
    },

    get_milk_trans_model: function () {
        var ProductsArray = [];
        $("#milk-purchase-item-list tbody tr.included").each(function () {
            ProductsArray.push({
                MilkSupplierID: $("#SupplierID").val(),
                SlipNo: $(this).find(".slip_number").text().trim(),
                Qty: clean($(this).find(".txtQty").val()),
                FatRangeID: $(this).find(".fatId").val(),
                Rate: clean($(this).find(".rate").val()),
                Amount: clean($(this).find(".amount").val()),
                Remarks: $(this).find(".Remarks").val().trim(),
            });

        })
        return ProductsArray;
    },
    on_save: function () {

        var self = milk_purchase;
        var master = self.get_milk_master_model();
        var Trans = self.get_milk_trans_model();
        var location = "/Purchase/MilkPurchase/Index";
        var url = '/Purchase/MilkPurchase/Save';

        if ($(this).hasClass("btnSaveAndDraft")) {
            master.IsDraft = true;
            url = '/Purchase/MilkPurchase/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveMilkAndNew")) {
                location = "/Purchase/MilkPurchase/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!master.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(master,Trans, url, location);
            }, function () {
            })
        } else {
            self.save(master, Trans, url, location);
        }
    },

    save: function (master, Trans, url, location) {
        var self = milk_purchase;
        $.ajax({
            url: url,
            data: { _master: master, _trans: Trans },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Milk purchase created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveMilk, .btnSaveAndDraft,.btnSaveMilkAndNew").css({ 'display': 'block' });
                }
            },
        });
    },
};