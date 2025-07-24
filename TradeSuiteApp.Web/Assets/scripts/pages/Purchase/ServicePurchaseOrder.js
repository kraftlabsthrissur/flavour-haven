
var progressbar = $("#progress-bar");
var bar = progressbar.find('.uk-progress-bar');
var select_table;
var freeze_header;
purchase_order = {
    init: function () {
        var self = purchase_order;

        self.purchase_requisition_list();
        supplier.supplier_list('service');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: purchase_order.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        $('#purchase-requisition-list').SelectTable({
            modal: "#select_pr",
            initiatingElement: "#purchase_requisition",
            startFocusIndex: 3,
            selectionType: "checkbox",
        });

        purchase_orderCRUD.purchaseOCreateAndUpdate();
        purchase_order_CalculateEvents.calculations();
        self.disable_address_list();
        freeze_header = $("#service-purchase-order-items-list").FreezeHeader();
        self.bind_events();
        self.get_employee();
    },

    details: function () {
        var self = purchase_order;
        $("body").on("click", ".printpdf", self.printpdf);
        freeze_header = $("#service-purchase-order-items-list").FreezeHeader();
    },

    printpdf: function () {
        var self = purchase_order;
        $.ajax({
            url: '/Reports/Purchase/ServicePurchaseOrderPrintPdf',
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

    list: function () {
        var self = purchase_order;
        $('#tabs-po').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

        $("body").on('click', '.btnclone', self.open_clone);
        $("body").on('click', '.btnsuspend', self.suspend_po);
    },

    tabbed_list: function (type) {
        var self = purchase_order;
        var $list;

        switch (type) {
            case "Draft":
                $list = $('#purchase-order-draft-list');
                break;
            case "ToBeApproved":
                $list = $('#purchase-order-to-be-approved-list');
                break;
            case "PartiallyApproved":
                $list = $('#purchase-order-partially-approved-list');
                break;
            case "Approved":
                $list = $('#purchase-order-approved-list');
                break;
            case "PartiallyProcessed":
                $list = $('#purchase-order-partially-processed-list');
                break;
            case "Processed":
                $list = $('#purchase-order-processed-list');
                break;
            case "Suspended":
                $list = $('#purchase-order-suspended-list');
                break;
            case "Cancelled":
                $list = $('#purchase-order-cancelled-list');
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

            var url = "/Purchase/ServicePurchaseOrder/GetPurchaseOrderList?type=" + type;

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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "ItemName", "className": "ItemName" },
                    {
                        "data": "Suspend", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsSuspendable ? "<button class='md-btn md-btn-primary btnsuspend' >Suspend</button>" : "";
                        }
                    },
                    {
                        "data": "Cancel", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<button class='md-btn md-btn-primary btnclone' >Clone</button>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/ServicePurchaseOrder/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    purchase_requisition_list: function () {
        $purchase_requisition_list = $('#purchase-requisition-list');

        if ($purchase_requisition_list.length) {
            $purchase_requisition_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var purchase_requisition_list_table = $purchase_requisition_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            purchase_requisition_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    purchase_requisition_list_table.api().column(index).search(this.value).draw();
                });
            });

        }
    },

    bind_events: function () {
        $("body").on('ifChanged', '.chkCheck', purchase_order.include_item);
        $("body").on('ifChanged', '#DirectInvoice', purchase_order.showsrnFields);
        $("body").on('click', '.remove-item', purchase_order.remove_item);
        $("#DDLItemCategory").on('change', purchase_order.get_purchase_category);

        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': purchase_order.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_supplier_details);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': purchase_order.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_item_details);

        UIkit.uploadSelect($("#select-quotation"), purchase_order.selected_quotation_settings)
        UIkit.uploadSelect($("#other-quotations"), purchase_order.other_quotation_settings)

        $('body').on('click', 'a.remove-quotation', purchase_order.remove_quotation);
        $('body').on('click', 'a.remove-file', purchase_order.remove_file);
        $('body').on('change', '#ShippingToId', purchase_order.set_state_id);
        $('body').on('click', '.cancel', purchase_order.cancel_confirm);

        $("#btnOKSupplier").on('click', purchase_order.select_supplier);
        $("#SupplierName").on('change', purchase_order.clearItem);

        $("body").on('click', '#service-purchase-order-list tbody tr', function (e) {
            var Id = $(this).find("td:eq(0) .ID").val();
            app.load_content('/Purchase/ServicePurchaseOrder/Details/' + Id);
        });
        $("body").on('click', '.btnsuspend', purchase_order.suspend_po);
        $("body").on('click', '.btnclone', purchase_order.open_clone);
        $(".select-item").on('click', purchase_order.check_supplier);
        $("#btnOKItem").on("click", purchase_order.select_item);
        $("#InvoiceNo").on("change", purchase_order.get_invoice_number_count);
        $('#Discount, #OtherDeductions').on('keyup', purchase_order.calculate_invoice_value);
        $("#DirectInvoice").on('ifChanged', purchase_order.disable_address_list);
        $("body").on("change", "#Department", purchase_order.get_employee);
    },
    get_employee: function () {

        $.ajax({
            url: '/Masters/Employee/GetEmployeeByDepartment/',
            data: { DepartmentID: $("#Department option:selected").val() },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#Employee").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#Employee").append(html);
            }
        });

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
    calculate_invoice_value: function () {                               //Calculate Total Invoice. When any changes in TDSOnFreight, LessTDS, Discount, Deductions, TotalInvoice
        var self = purchase_order;

        var deductions = clean($('#OtherDeductions').val());

        var discount = clean($('#Discount').val());
        if (discount < 0) {
            app.show_error("Discount amount must be positive");
        }
        else {
            CalculateNetAmountValue();
        }

    },

    cancel_confirm: function () {
        var self = purchase_order
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    get_invoice_number_count: function (release) {

        $.ajax({
            url: '/Purchase/ServicePurchaseInvoice/GetInvoiceNumberCount',
            data: {
                Hint: $("#InvoiceNo").val(),
                Table: "PurchaseInvoiceForservice",
                SupplierID: $('#SupplierID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                count = response.data;
                $("#invoice-count").val(count);
            }
        });
    },
    check_supplier: function () {
        var self = purchase_order;
        self.error_count = 0;
        self.error_count = self.validate_supplier();
        if (self.error_count > 0) {
            UIkit.modal($('#select-item')).hide();
        }

    },
    open_clone: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).parents('tr').find('.ID').val();
        window.location = '/Purchase/ServicePurchaseOrder/Clone/' + id;
    },
    suspend_po: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).parents('tr').find('.ID').val();

        app.confirm("Do you really want to Suspend? This can not be undone.", function () {
            self.suspend_purchaseorder(id);
        });
    },
    suspend_purchaseorder: function (id) {
        $.ajax({
            url: '/Purchase/ServicePurchaseOrder/Suspend',
            content: "application/json;charset=utf-8",
            dataType: "json",
            type: "GET",
            data: {
                ID: id,
                Table: "PurchaseOrderForService"
            },
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Purchase Order Suspended Successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                }
                if (response.Data == 0) {
                    app.show_error("Purchase Order Already Processed");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('processed');
                }
                if (response.Data == 2) {
                    app.show_error("Please cancel SRN before suspending");

                }
            }
        });
    },

    select_supplier: function () {
        self = purchase_order;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        $("#SupplierName").val(Name);
        $("#SupplierLocation").val(Location);
        $("#SupplierID").val(ID);
        $("#StateId").val(StateID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
        self.clearItem();
        UIkit.modal($('#select-supplier')).hide();
        $("#SupplierID").trigger('change');
        self.is_item_supplied_by_supplier();
        self.get_invoice_number_count();
        self.get_supplier_addresses();
    },
    set_state_id: function () {
        var state_id = $(this).find("option:selected").data("state-id");
        $("#ShippingStateId").val(state_id);
        $("#service-purchase-order-items-list tbody tr .clRqQty.included").each(function () {
            CalculateValueInGrid($(this));
        });
    },
    set_supplier_details: function (event, item) {   // on select auto complete item
        self = purchase_order;
        $("#SupplierName").val(item.value);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#DDLPaymentWithin option:contains(" + item.paymentDays + ")").attr("selected", true);
        $("#SupplierLocation").focus();
        $("#SupplierID").trigger('change');
        self.is_item_supplied_by_supplier();
        self.get_invoice_number_count();
        self.get_supplier_addresses();
    },
    get_supplier_addresses: function () {
        var SupplierID = $("#SupplierID").val();
        $.ajax({
            url: '/Masters/Supplier/GetAddresses/',
            dataType: "json",
            data: {
                SupplierID: SupplierID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var html = "";
                    $.each(response.BillingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultBillingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + ', ' + record.AddressLine2 + "</option>";
                    });
                    $("#ShippingToId").html(html);
                    html = "";
                    $.each(response.ShippingAddressList, function (i, record) {
                        html += "<option " + (response.DefaultShippingAddressID == record.AddressID ? " selected = 'selected' " : "") + " value='" + record.AddressID + "'>" + record.AddressLine1 + ', ' + record.AddressLine2 + "</option>";
                    });
                    $("#DDLBillTo").html(html);
                    if (response.Message !== undefined) {
                        app.show_error(response.Message);
                    }

                }
            },
            error: function (xhr, status, error) {
                // Handle the error here
                app.show_error(error);
            }
        });
    },
    is_item_supplied_by_supplier: function () {
        var self = purchase_order;
        var ItemList = "";
        $("#service-purchase-order-items-list tbody tr").each(function () {
            if (clean($(this).find('.CategoryID').val() )> 0) {
                ItemList += ($(this).find('.ItemID').val()) + ',';
            }
        });
        if (ItemList.length > 0) {
            $.ajax({
                url: '/Purchase/PurchaseOrder/IsItemSuppliedBySupplier',
                data: {
                    ItemLists: ItemList,
                    SupplierID: $("#SupplierID").val()
                },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, function (key, item) {
                        $("#service-purchase-order-items-list tbody tr").each(function () {
                            var i = $(this).find(".ItemID").val();
                            if (($("#service-purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status != "Eligible")) {
                                $(this).closest('tr').addClass('ineligible');

                            }

                            else if (($("#service-purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status == "Eligible")) {
                                $(this).closest('tr').removeClass('ineligible');
                            }
                        });
                    });

                    $("#service-purchase-order-items-list tbody tr .clRqQty").each(function () {
                        CalculateValueInGrid($(this));
                    });

                },
            });
        }
    },

    get_suppliers: function (release) {


        $.ajax({
            url: '/Masters/Supplier/getServiceSupplierForAutoComplete',
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

    cancel: function () {
        app.confirm("Are you sure cancel the service purchase order?", function () {
            var ServicePurchaseOrderID = $("#ID").val();
            $.ajax({
                url: '/Purchase/ServicePurchaseOrder/Cancel',
                data: {
                    ServicePurchaseOrderID: ServicePurchaseOrderID,
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Purchase Order Service Cancelled Successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/ServicePurchaseOrder/Index";
                        }, 1000);
                    } else {
                        app.show_error("Could not Cancel the Purchase Order Service");
                    }
                }
            });
        })
    },
    clearItem: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
    },//
    set_item_details: function (event, item) {   // on select auto complete item
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#Rate").val('');
        $("#Unit").val(item.unit);
        $("#LastPr").val(item.lastPr);
        $("#LowestPr").val(item.lowestPr);
        $("#PendingOrderQty").val(item.pendingOrderQty);
        $("#QtyWithQc").val(item.qtyWithQc);
        $("#QtyAvailable").val(item.qtyAvailable);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CategoryID").val(item.itemCategory);
        $("#TravelCategoryID").val(item.travelCategory);
        if ($("#TravelCategoryID").val() > 0) {
            $("#select_travel").removeClass("uk-hidden").addClass("visible");
        }
        else {
            $("#select_travel").addClass("uk-hidden").removeClass("visible");
        }
        $("#Qty").focus();
    },

    select_item: function () {
        var self = purchase_order;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var lastPr = $(row).find(".lastPr").val();
        var lowestPr = $(row).find(".lowestPr").val();
        var Category = $(row).find(".ItemCategory").val();
        var pendingOrderQty = $(row).find(".pendingOrderQty").val();
        var qtyWithQc = $(row).find(".qtyWithQc").val();
        var qtyAvailable = $(row).find(".qtyAvailable").val();
        var gstPercentage = $(row).find(".gstPercentage").val();
        var itemCategoryID = $(row).find(".itemCategoryID").val();
        var travelCategoryID = clean($(row).find(".TravelCategoryID").val());
        $("#TravelCategoryID").val(travelCategoryID);
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Rate").val('');
        $("#Unit").val(Unit);
        $("#LastPr").val(lastPr);
        $("#LowestPr").val(lowestPr);
        $("#PendingOrderQty").val(pendingOrderQty);
        $("#QtyWithQc").val(qtyWithQc);
        $("#QtyAvailable").val(qtyAvailable);
        $("#GSTPercentage").val(gstPercentage);
        $("#CategoryID").val(itemCategoryID);
        if ($("#TravelCategoryID").val() > 0) {
            $("#select_travel").removeClass("uk-hidden").addClass("visible");
        }
        else {
            $("#select_travel").addClass("uk-hidden").removeClass("visible");
        }
        $("#txtRqQty").focus();
        UIkit.modal($('#select-item')).hide();

    },

    get_item_details: function (release) {

        if ($("#SupplierID").val() == "0") {
            app.add_error_class($("#SupplierName"));
            app.show_error("Please select Supplier");
            return;
        }
        if ($("#GST").val() == "") {
            app.add_error_class($("#GST"));
            app.show_error("Please select GST Type");
            return;
        }
        $.ajax({
            url: '/Masters/Item/ItemByServiceCategoryAndSupplierID',
            //    url: '/Purchase/ServicePurchaseRequisition/getItemForAutoComplete',

            data: {
                Area: "Service",
                NameHint: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
                SupplierID: $("#SupplierID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },
    get_purchase_category: function () {
        var item_category_id = $(this).val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = -1;
        }
        $.ajax({
            url: '/Purchase/PurchaseRequisition/GetPurchaseCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#DDLPurchaseCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#DDLPurchaseCategory").append(html);
            }
        });

    },
    remove_quotation: function () {
        $(this).closest('span').remove();
    },
    remove_file: function () {
        $(this).parent('li').remove();
        var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
        $('#file-count').text(file_count + " File(s)");
    },
    remove_item: function () {
        $(this).closest('tr').remove();
        purchase_order.count_items();
        CalculateGSTOutsideTheGrid();
        CalculateNetAmountValue();
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('readonly');
        purchase_order.count_items();
        CalculateGSTOutsideTheGrid();
        CalculateNetAmountValue();

    },
    showsrnFields: function () {
        if ($(this).is(":checked")) {
            $('.srnfields').addClass('direct').removeClass('uk-hidden');
            //$('.btnSaveASDraftPO').addClass('uk-hidden');
        } else {
            $('.srnfields').removeClass('direct').addClass('uk-hidden');
            //$('.btnSaveASDraftPO').removeClass('uk-hidden');
        }
    },
    error_count: 0,
    validate_item: function () {
        var self = purchase_order;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    count_items: function () {
        var count = $('#service-purchase-order-items-list tbody').find('input.chkCheck:checked').length;
        $('#item-count').val(count);
    },
    validate_form: function () {
        var self = purchase_order;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_form_for_draft: function () {
        var self = purchase_order;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_supplier: function () {
        var self = purchase_order;
        if (self.rules.on_item.length > 0) {
            return form.validate(self.rules.on_item);
        }
        return 0;
    },
    rules: {
        on_add: [

            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                ]
            },
            {
                elements: "#ItemName",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please choose a Supplier" },
                    { type: form.required, message: "Please choose a Supplier" },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                ]
            },
            {
                elements: "#Rate",
                rules: [
                    { type: form.required, message: "Please enter Rate" },
                    { type: form.positive, message: "Please enter positive value for rate" },
                     { type: form.non_zero, message: "Please enter rate" },
                ]
            },
            {
                elements: "#GST",
                rules: [
                   { type: form.required, message: "Please select GST Type" },
                ]
            },
            //{
            //    elements: "#Location",
            //    rules: [
            //       { type: form.required, message: "Please choose Location" },
            //    ]
            //},
            //{
            //    elements: "#Department",
            //    rules: [
            //        { type: form.required, message: "Please choose Department" }
            //    ]
            //},
               {
                   elements: "#select_travel.visible #TravelToID",
                   rules: [
                        { type: form.required, message: "Please choose travel to ID" },
                        //{
                        //    type: function (element) {
                        //        return $(element).val() == "" || $(element).val() != $("#TravelFromID").val();
                        //    }, message: "Travel from and travel locations to are same"
                        //},
                        {
                            type: function (element) {
                                return !($(element).val() == "" && $("#TravelFromID").val() != "");
                            }, message: "invalid Travel To location"
                        }
                   ]
               },
            {
                elements: "#select_travel.visible #TravelFromID",
                rules: [
                        {
                            type: function (element) {
                                return !($(element).val() == "" && $("#TravelToID").val() != "");
                            }, message: "invalid Travel From location"
                        },
                        { type: form.required, message: "Please choose travel from ID" },

                ]
            },
             {
                 elements: "#select_travel.visible #travelDate",
                 rules: [
                       { type: form.required, message: "Please choose travel date" },

                 ]
             },
             {
                 elements: "#select_travel.visible #TransportModeID",
                 rules: [
                    { type: form.required, message: "Please choose travel mode" },

                 ]
             }

        ],
        on_item: [

    {
        elements: "#SupplierID",
        rules: [
            { type: form.non_zero, message: "Please choose a Supplier" },
            { type: form.required, message: "Please choose a Supplier" },
        ]
    },

        ],
        on_submit: [
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please add atleast one item" },
                     { type: form.required, message: "Please add atleast one item" },
                     //{
                     //    type: function (element) {
                     //        var error = false;
                     //        var count = $("#service-purchase-order-items-list tbody tr.included.ineligible").length;

                     //        if (count > 0)
                     //            error = true;
                     //        return !error;
                     //    }, message: 'Some of the items in grid are not supplied by supplier please select another supplier'
                     //},
                 ]
             },
             {
                 elements: "#GST",
                 rules: [
                    { type: form.required, message: "Please select GST Type" },
                 ]
             },

            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please choose a Supplier" },
                    { type: form.required, message: "Please choose a Supplier" },
                ]
            },
           //{
           //    elements: "#ShippingToId",
           //    rules: [
           //        { type: form.required, message: "Please choose a Shipping Location" },
           //    ]
           //},
           // {
           //     elements: "#DDLBillTo",
           //     rules: [
           //         { type: form.required, message: "Please choose a Billing Location" },
           //     ]
           // },

           //  {
           //      elements: ".clRqQty.included",
           //      rules: [
           //          { type: form.required, message: "Please enter quantity" },
           //          { type: form.positive, message: "Please enter positive value quantity" },
           //      ]
           //  },
           //  {
           //      elements: ".txtclRate.included",
           //      rules: [
           //          { type: form.required, message: "Please enter rate" },
           //          { type: form.positive, message: "Please enter positive value for rate" },
           //          { type: form.non_zero, message: "Please enter rate" },
           //      ]
           //  },
           //   {
           //       elements: ".txtclValue.included",
           //       rules: [
           //           { type: form.required, message: "Please enter value" },
           //           { type: form.positive, message: "Please enter positive value for value" },
           //           { type: form.non_zero, message: "Please enter rate" },
           //       ]
           //   },
              //{
              //    elements: ".txtClGSTAmt.included",
              //    rules: [
              //        { type: form.required, message: "Please enter GST Amount" },
              //        { type: form.positive, message: "Please enter positive value for txtClGSTAmt" },
              //    ]
              //},
               //{
               //    elements: "#NetAmt",
               //    rules: [
               //         { type: form.positive, message: "Please check Net Amount" },
               //         { type: form.non_zero, message: "Invaild NetAmount" },
               //    ]
               //},
               //{
               //    elements: "#PaymentModeID",
               //    rules: [
               //        { type: form.required, message: "Please choose Payment Mode" },
               //        {
               //            type: function (element) {
               //                var CashPaymentLimit = clean($("#CashPaymentLimit").val());
               //                var NetAmt = clean($("#NetAmt").val());
               //                var PaymentMode = $("#PaymentModeID Option:Selected ").text();
               //                var error = false;
               //                if (CashPaymentLimit < NetAmt && PaymentMode == 'Cash') {
               //                    error = true;
               //                }
               //                return !error;
               //            }, message: 'Please choose Another Payment Mode'
               //        },
               //    ]
               //},
               //{
               //    elements: ".direct #InvoiceNo ",
               //    rules: [
               //         { type: form.required, message: "Please Enter Invoice Number." },
               //                 {
               //                     type: function (element) {
               //                         var error = false;
               //                         var count = clean($('#invoice-count').val());
               //                         if (count > 0)
               //                             error = true;
               //                         return !error;
               //                     }, message: 'Invoice number has been already entered for this supplier'
               //                 },
               //    ]
               //},
               // {
               //     elements: ".direct #InvoiceDate ",
               //     rules: [
               //      { type: form.required, message: "Please Enter Invoice Date." },
               //      { type: form.current_date, message: "Invalid invoice date" },
               //     ]
               // },

               //  {
               //      elements: "#AdvanceAmount",
               //      rules: [
               //           { type: form.positive, message: "Please enter positive value for advance Amount" },
               //          {
               //              type: function (element) {
               //                  var error = false;
               //                  var advance = clean($('#AdvanceAmount').val());

               //                  var NetAmt = clean($('#NetAmt').val());

               //                  if (advance > NetAmt)
               //                      error = true;


               //                  return !error;
               //              }, message: 'Advance amount must be less than net amount'
               //          },

               //      ]
               //  },
        ],
        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                    //{
                    //    type: function (element) {
                    //        var error = false;
                    //        var count = $("#service-purchase-order-items-list tbody tr.included.ineligible").length;

                    //        if (count > 0)
                    //            error = true;
                    //        return !error;
                    //    }, message: 'Some of the items in grid are not supplied by supplier please select another supplier'
                    //},
                ]
            },


           {
               elements: "#SupplierID",
               rules: [
                   { type: form.non_zero, message: "Please choose a Supplier" },
                   { type: form.required, message: "Please choose a Supplier" },
               ]
           },
          {
              elements: "#ShippingToId",
              rules: [
                  { type: form.required, message: "Please choose a Shipping Location" },
              ]
          },
           {
               elements: "#DDLBillTo",
               rules: [
                   { type: form.required, message: "Please choose a Billing Location" },
               ]
           },
            {
                elements: ".clRqQty.included",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.positive, message: "Please enter positive value quantity" },
                ]
            },

             {
                 elements: ".txtclValue.included",
                 rules: [
                     { type: form.required, message: "Please enter value" },
                     { type: form.positive, message: "Please enter positive value for value" },
                     { type: form.non_zero, message: "Please enter rate" },
                 ]
             },
             {
                 elements: ".txtClGSTAmt.included",
                 rules: [
                     { type: form.required, message: "Please enter GST Amount" },
                     { type: form.positive, message: "Please enter positive value for txtClGSTAmt" },
                 ]
             },
              {
                  elements: "#NetAmt",
                  rules: [
                       { type: form.positive, message: "Please check Net Amount" },
                  ]
              },
              {
                  elements: "#PaymentModeID",
                  rules: [
                      { type: form.required, message: "Please choose Payment Mode" },
                      {
                          type: function (element) {
                              var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                              var NetAmt = clean($("#NetAmt").val());
                              var PaymentMode = $("#PaymentModeID Option:Selected ").text();
                              var error = false;
                              if (CashPaymentLimit < NetAmt && PaymentMode == 'Cash') {
                                  error = true;
                              }
                              return !error;
                          }, message: 'Please choose Another Payment Mode'
                      },
                  ]
              },
              {
                  elements: ".direct #InvoiceNo ",
                  rules: [
                       { type: form.required, message: "Please Enter Invoice Number." },
                               {
                                   type: function (element) {
                                       var error = false;
                                       var count = clean($('#invoice-count').val());
                                       if (count > 0)
                                           error = true;
                                       return !error;
                                   }, message: 'Invoice number has been already entered for this supplier'
                               },
                  ]
              },
               {
                   elements: ".direct #InvoiceDate ",
                   rules: [
                    { type: form.required, message: "Please Enter Invoice Date." },
                    { type: form.current_date, message: "Invalid invoice date" },
                   ]
               },

                {
                    elements: "#AdvanceAmount",
                    rules: [
                         { type: form.positive, message: "Please enter positive value for advance Amount" },
                        {
                            type: function (element) {
                                var error = false;
                                var advance = clean($('#AdvanceAmount').val());

                                var NetAmt = clean($('#NetAmt').val());

                                if (advance > NetAmt)
                                    error = true;


                                return !error;
                            }, message: 'Advance amount must be less than net amount'
                        },

                    ]
                },
        ]
    },
    selected_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        progress: function (percent) {
            //percent = Math.ceil(percent);
            //bar.css("width", percent + "%").text(percent + "%");
        },
        notallowed: function () {
            app.show_error("File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/Image File");
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response);
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
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
            }
            if (failure != "") {
                app.show_error(failure);
            }

        }
    },
    other_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            // bar.css("width", percent + "%").text(percent + "%");
        },
        notallowed: function () {
            app.show_error("File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/Image File");
        },
        complete: function (response, xhr) {
            var data = $.parseJSON(response);
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                var dropdown = $("#other-quotation-list .uk-nav-dropdown");
                width = $('#other-quotation-list').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        dropdown.append("<li class='file-list'>"
                        + "<a class='remove-file'>X</a>"
                        + "<span data-id='" + record.ID + "' style='width:" + width + "px;' class='view-file' data-url='" + record.URL + "' data-path='" + record.Path + "'>"
                        + record.Name
                        + "</span>"
                        + "</li>");
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
            }
            if (failure != "") {
                app.show_error(failure);
            }
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();

            var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
            $('#file-count').text(file_count + " File(s)");
        }
    }
};

purchase_orderCRUD = {

    purchaseOCreateAndUpdate: function () {

        //Add products to pr grid
        $("#btnAddProduct").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_item();
            if (self.error_count == 0) {
                var item = {
                    Code: "",
                    Name: $("#ItemName").val(),
                    ID: $("#ItemID").val(),
                    Qty: clean($("#Qty").val()),
                    Unit: $("#Unit").val(),
                    Rate: clean($("#Rate").val()),
                    Value: $("#txtValue").val(),
                    LastPR: $("#LastPr").val(),
                    LowestPR: $("#LowestPr").val(),
                    PendingOrderQty: $("#PendingOrderQty").val(),
                    QtyUnderQC: $("#QtyWithQc").val(),
                    QtyAvailable: $("#QtyAvailable").val(),
                    Remarks: $("#ItemRemarks").val(),
                    GSTPercentage: clean($('#GSTPercentage').val()),
                    GSTAmount: 0,

                    ServiceLocationID: $("#Location").val(),
                    DepartmentID: $("#Department").val(),
                    EmployeeID: $("#Employee").val(),
                    CompanyID: $("#InterCompany").val(),
                    ProjectID: $("#Project").val(),
                    TravelFromID: $("#TravelFromID").val(),
                    TravelToID: $("#TravelToID").val(),
                    ModeOfTransportID: $("#TransportModeID").val(),
                    TravelRemark: $("#TravelingRemarks").val() == undefined ? '' : $("#TravelingRemarks").val(),
                    TravelDate: $("#travelDate").val(),
                    ServiceLocation: $("#Location").val() == "" ? "" : $("#Location  option:selected").text(),
                    Department: $("#Department").val() == "" ? "" : $("#Department  option:selected").text(),
                    Employee: $("#Employee").val() == "" ? "" : $("#Employee  option:selected").text(),
                    Company: $("#InterCompany").val() == "" ? "" : $("#InterCompany  option:selected").text(),
                    Project: $("#Project").val() == "" ? "" : $("#Project  option:selected").text(),
                    TravelFrom: $("#TravelFromID").val() == "" ? "" : $("#TravelFromID  option:selected").text(),
                    TravelTo: $("#TravelToID").val() == "" ? "" : $("#TravelToID  option:selected").text(),
                    ModeOfTransport: $("#TransportModeID").val() == "" ? "" : $("#TransportModeID  option:selected").text(),
                    ItemCategoryID: $("#CategoryID").val(),
                    TravelCategoryID: $("#TravelCategoryID").val(),
                    PurchaseRequisitionID: "",
                    PRTransID: ""
                };
                AddProductToPrGrid("", item);
                purchase_order.count_items();
                clearItemSelectControls();
                $("#ItemName").focus();
            }

        });
        function clearItemSelectControls() {
            $("#ItemName").val('');
            $("#ItemID").val('');
            $("#Qty").val('');
            $("#Rate").val('');
            $("#Unit").val('');
            $("#LastPr").val('');
            $("#LowestPr").val('');
            $("#PendingOrderQty").val('');
            $("#QtyOrdered").val('');
            $("#QtyAvailable").val('');
            $("#QtyWithQc").val('');
            $("#txtValue").val('');
            $("#ItemRemarks").val('');
            $("#TravelFromID").val("");
            $("#TravelToID").val("");
            $("#TravelModeID").val("");
            $("#TransportModeID").val("");
            $("#TravelingRemarks").val('');
            $("#Employee").val("");
            $("#InterCompany").val("");
            $("#Project").val("");
            $("#TransportModeID").val("");
            if ($("#IsBranchLocation").val() == "False") {
                $("#Location").val("");
                $("#Department").val("");
            }

        }
        $(".btnSelectReqstion").click(function () {
            if ($("#SupplierID").val() > 0) {
                UIkit.modal('#select_pr').show();
            } else {
                app.show_error("Please select supplier details to proceed.")
            }
        });
        function AddProductToPrGrid(PrRowClass, item) {
            var existedHtml = '';
            var html = '';
            var $html;
            var SerialNo = $(".poTbody .rowPO").length + 1;
            var tr = '';
            if (item.TravelCategoryID > 0) {
                tr = '<tr>'
           + '<td>'
       + '<input type="hidden" class="travelfromId" value="' + item.TravelFromID + '"/>'
           + '<input type="hidden" class="traveltoId" value="' + item.TravelToID + '"/>' //TODO
           + '<input type="hidden" class="modeoftravelId" value="' + item.ModeOfTransportID + '"/>'

          + '</td>'
           + '<td colspan="16">'
           + '<div class="uk-grid">'
           + '<div class="uk-width-medium-2-10"><label>Travel From</label>'
           + '<div> ' + item.TravelFrom + '</div></div>'
           + '<div class="uk-width-medium-2-10"><label>Travel To</label>'
           + '<div> ' + item.TravelTo + '</div></div>'

           + '<div class="uk-width-medium-2-10"><label>Mode of Travel</label>'
            + '<div> ' + item.ModeOfTransport + '</div></div>'

           + '<div class="uk-width-medium-2-10  "><label>Travel Date</label>'
              + '<div class="md-input all-date date traveldate"> ' + item.TravelDate + '</div></div>'

           + '<div class="uk-width-medium-2-10 "><label>Travel Remarks</label>'
             + '<div class="travelremark"> ' + item.TravelRemark + '</div></div>'

           + '</div></td>';

            }

            html = '<tr class="rowPO included ' + PrRowClass + '">' +
                    '<td class="uk-text-center clPr">' + SerialNo + '</td>' +
                    '<td class="uk-text-center checked chkValid" data-md-icheck><input type="checkbox" class="chkCheck"  checked/></td>' +
                    '<td class="clItem">' + item.Name +
                        '<input type="hidden" class="ItemID" value="' + item.ID + '" />' +
                        '<input type="hidden" class="PrId" value="' + item.PurchaseRequisitionID + '" />' +
                     '<input type="hidden" class="PrTransId" value="' + item.PRTransID + '" />' +
                      '<input type="hidden" class="CategoryID" value="' + item.ItemCategoryID + '" />' +
                    '</td>' +
                    '<td class="clUnit">' + item.Unit + '</td>' +
                    '<td><input type="text" class="md-input mask-qty clRqQty included" value="' + item.Qty + '" /></td>' +
                    '<td><input type="text"   class="md-input mask-currency txtclRate included" value="' + item.Rate + '"  /></td>' +
                    '<td><input  type="text"  class="md-input mask-currency txtclValue included" readonly value="' + item.Value + '" disabled/></td>' +
                    '<td><input type="text" class="md-input mask-currency txtClGSTPer included" readonly  value="' + item.GSTPercentage + '" disabled /></td>' +
                    '<td><input type="text"  class="md-input mask-currency txtClGSTAmt included" readonly value="' + item.GSTAmount + '"  disabled/></td>' +
                    '<td><input type="text"  class="md-input mask-currency clTotal included" readonly value="' + item.Value + '"  disabled/></td>' +
                    '<td class="">' + item.ServiceLocation + '<input type="hidden" class="included location-id" value="' + item.ServiceLocationID + '"></td>' +
                    '<td class="">' + item.Department + '<input type="hidden" class="included department-id" value="' + item.DepartmentID + '"></td>' +
                    '<td class="">' + item.Employee + '<input type="hidden" class="included employee-id" value="' + item.EmployeeID + '"></td>' +
                    '<td class="">' + item.Company + '<input type="hidden" class="included company-id" value="' + item.CompanyID + '"></td>' +
                    '<td class="">' + item.Project + '<input type="hidden" class="included project-id" value="' + item.ProjectID + '"></td>' +
                    '<td class="clRemarks"><input type="text" class="md-input txtClRemarks included" value="' + (item.Remarks == null ? "" : item.Remarks) + '" /></td>' +
                   '</tr>' + tr;
            $html = $(html);
            app.format($html);
            $(".poTbody").append($html);
            if (item.TravelCategoryID > 0) {
                CalculateGSTInsideGrid($(".poTbody tr").last().prev());
            }
            else {
                CalculateGSTInsideGrid($(".poTbody tr").last());
            }

            //Calculate net amount
            calculateNetamountvalueForGrid();
            freeze_header.resizeHeader();
        }
        function SetSerialNumberReorder() {
            var SerialNo = 1;
            $(".poTbody .rowPO").each(function () {
                $(this).find(".clPr").text(SerialNo);
                SerialNo++;
            });
        }
        function get_po_items(UnProcPrList) {
            $(".poTbody .rowPR").each(function () {
                $(this).remove();
                $(this).next("tr").remove();
            });
            $(".poTbody tr").each(function () {
                $(this).remove();
                $(this).next("tr").remove();
            });
            if (UnProcPrList.length > 0) {
                $.ajax({
                    url: '/Purchase/ServicePurchaseOrder/GetPurchaseRequisitionItems',
                    data: { PurchaseRequisitionID: UnProcPrList, SupplierID: $("#SupplierID").val() },
                    type: "GET",
                    cache: false,
                    traditional: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $.each(data, function (key, item) {
                            item.Value = 0;
                            item.Qty = item.Quantity;
                            item.ServiceLocation = item.ServiceLocation == null ? "" : item.ServiceLocation;
                            item.Department = item.Department == null ? "" : item.Department;
                            item.Employee = item.Employee == null ? "" : item.Employee;
                            item.Project = item.Project == null ? "" : item.Project;
                            item.Company = item.Company == null ? "" : item.Company;
                            item.PurchaseRequisitionID = item.PurchaseReqID == null ? 0 : item.PurchaseReqID;
                            item.TravelDate = item.TravelDate == null ? "" : item.TravelDateString;
                            item.TravelFrom = item.TravelFrom == null ? "" : item.TravelFrom;
                            item.TravelFromID = item.TravelFromID == null ? "" : item.TravelFromID;
                            item.TravelRemark = item.TravelingRemarks == null ? "" : item.TravelingRemarks;
                            item.TravelTo = item.TravelTo == null ? "" : item.TravelTo;
                            item.TravelToID = item.TravelToID == null ? "" : item.TravelToID;
                            item.ModeOfTransport = item.TransportMode == null ? "" : item.TransportMode;
                            item.ModeOfTransportID = item.TransportModeID == null ? "" : item.TransportModeID;
                            item.TravelCategoryID = item.TravelCategoryID;
                            AddProductToPrGrid("rowPR", item);

                            purchase_order.count_items();
                        });

                        //Reorder serial no
                        SetSerialNumberReorder();
                        //Calculate net amount
                        calculateNetamountvalueForGrid();
                        freeze_header.resizeHeader();
                    },
                });
            }
        }
        $("#btnOkPrList").click(function () {
            var UnProcPrList = [];
            $(".unPrTbody .rowUnPr").each(function () {
                if ($(this).find('.clChk .clChkItem').is(":checked")) {
                    UnProcPrList.push($(this).find('.clId .clprIdItem').val())
                }
            });

            if ($(".poTbody .rowPR").length > 0 && UnProcPrList.length > 0) {
                app.confirm("Selected Items will be removed", function () {
                    get_po_items(UnProcPrList);
                })

            } else {
                get_po_items(UnProcPrList);
            }

        });
        $(".btnSavePO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var _Master = GetPurchaseOrderForSave();
                console.log(_Master);
                var _Trans = GetProductFromGrid();
                if (_Master.ID != "0") {
                    UpdatePO(_Master, _Trans);
                } else {
                    SavePO(_Master, _Trans);
                }
            }
        });
        $(".btnSaveASDraftPO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form_for_draft();
            if (self.error_count == 0) {
                var _Master = GetPurchaseOrderForSave(true);
                var _Trans = GetProductFromGrid();
                if (_Master.ID != "0") {
                    UpdatePO(_Master, _Trans);
                } else {
                    SavePO(_Master, _Trans);
                }
            }
        });
        function SavePO(_Master, _Trans) {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            var url;
            if (_Master.IsDraft == true) {
                url = '/Purchase/ServicePurchaseOrder/SaveAsDraft'
            }
            else {
                url = '/Purchase/ServicePurchaseOrder/Save';
            }
            if (self.error_count == 0) {
                $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'none' });
                $.ajax({
                    url: url,
                    data: { PO: _Master, Details: _Trans },
                    dataType: "json",
                    type: "POST",
                    //contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.Status == "success") {
                            //clearAllControls();
                            app.show_notice("Successfully saved " + _Master.PurchaseOrderNo + ".");
                            setTimeout(function () {
                                window.location = "/Purchase/ServicePurchaseOrder/Index";
                            }, 1000);
                        } else {
                            if (typeof data.data[0].ErrorMessage != "undefined")
                                app.show_error(data.data[0].ErrorMessage);
                            $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'block' });
                        }
                    },
                });
            }
        }
        function UpdatePO(_Master, _Trans) {
            $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'none' });
            $.ajax({
                url: '/Purchase/ServicePurchaseOrder/Update',
                data: { PO: _Master, Details: _Trans },
                dataType: "json",
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.Status == "success") {
                        //clearAllControls();
                        app.show_notice("Successfully updated " + _Master.PurchaseOrderNo + ".");
                        setTimeout(function () {
                            window.location = "/Purchase/ServicePurchaseOrder/Index";
                        }, 1000);
                    } else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'block' });
                    }
                },
            });
        }
        function calculateNetamountvalueForGrid() {
            var Totalproductamount = 0;
            $(".poTbody .rowPO.included").each(function () {
                var obj = clean($(this).find(".clTotal").val());
                Totalproductamount = Totalproductamount + obj;
            });
            var discount = clean($('#Discount').val());
            var deductions = clean($('#OtherDeductions').val());
            Totalproductamount = Totalproductamount - discount + deductions;
            $("#NetAmt").val(Totalproductamount);

        }
        function GetPurchaseOrderForSave(IsDraft) {
            if (typeof IsDraft === "undefined" || IsDraft === null) {
                IsDraft = false;
            }
            var ID;
            var inCludGST = false;
            var Extra = false;
            if ($('#GST').val() == 1) {
                inCludGST = true;
            } else {
                Extra = true;
            }

            var OtherQ = [];
            $("#other-quotation-list span.view-file").each(function () {
                OtherQ.push($(this).data('id'));
            });
            if ($("#IsClone").val() == "True") {
                ID = 0;
            }
            else {
                ID = $("#ID").val();
            }
            var Model = {
                ID: ID,
                PurchaseOrderNo: $("#PurchaseOrderNo").val(),
                PurchaseOrderDate: $("#PurchaseOrderDate").val(),
                SupplierID: $("#SupplierID").val(),
                AdvancePercentage: clean($("#AdvanceAmount").val()) || 0,
                AdvanceAmount: clean($("#AdvanceAmount").val()) || 0,
                PaymentModeID: $("#PaymentModeID").val(),
                ShippingAddressID: $("#ShippingToId").val(),
                BillingAddressID: $("#DDLBillTo").val(),
                InclusiveGST: inCludGST,
                GstExtra: Extra,
                SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
                OtherQuotationIDS: OtherQ.join(","),
                DeliveryWithin: $("#DeliveryWithin").val(),
                PaymentWithinID: $("#DDLPaymentWithin").val(),
                SGSTAmt: clean($("#SGSTAmt").val()),
                CGSTAmt: clean($("#CGSTAmt").val()),
                IGSTAmt: clean($("#IGSTAmt").val()),
                FreightAmt: clean($("#FreightAmt").val()),
                OtherCharges: clean($("#OtherCharges").val()),
                PackingShippingCharge: clean($("#PackingShippingCharge").val()),
                NetAmt: clean($("#NetAmt").val()),
                DaysToSupply: $("#DeliveryWithin").val(),
                Remarks: $("#Remarks").val(),
                SupplierReferenceNo: $("#SupplierReferenceNo").val(),
                TermsOfPrice: $("#TermsOfPrice").val(),
                PurchaseOrderDate: $("#txtDate").val(),
                DirectInvoice: $("#DirectInvoice").is(":checked") ? true : false,
                IsDraft: IsDraft,
                InvoiceNo: $("#InvoiceNo").val(),
                InvoiceDateStr: $("#InvoiceDate").val(),
                IsGSTRegistred: $("#IsGSTRegistred").val(),
                Discount: clean($("#Discount").val()),
                OtherDeductions: clean($("#OtherDeductions").val())
            }
            return Model;
        }
        function GetProductFromGrid() {
            var ProductsArray = [];
            $(".poTbody .rowPO").each(function () {
                if ($(this).find('.chkValid .chkCheck').is(':checked')) {
                    var ItemID = $(this).find('.clItem  .ItemID').val();
                    var PurchaseReqID = $(this).find('.PrId').val();
                    var PRTransID = $(this).find('.PrTransId').val();
                    var Quantity = clean($(this).find('.clRqQty').val());
                    var Rate = clean($(this).find('.txtclRate').val());
                    var Amount = clean($(this).find('.txtclValue').val());
                    var NetAmount = clean($(this).find('.clTotal').val());
                    var Remarks = $(this).find('.clRemarks  .txtClRemarks').val();

                    var LastPurchaseRate = clean($(this).find('.clLastPr').text());
                    var LowestPR = clean($(this).find('.clLowestPr').text());
                    var QtyInQC = clean($(this).find('.clQtyWithQc').text());
                    var QtyOrdered = clean($(this).find('.clPendingOrderQty').text());
                    var QtyAvailable = clean($(this).find('.clQtyAvailable').text());

                    var Tot_GST_persc = clean($(this).find('.txtClGSTPer').val());
                    var Tot_GST_Amt = clean($(this).find('.txtClGSTAmt').val());
                    var SGSTPercent = 0;
                    var CGSTPercent = 0;
                    var IGSTPercent = 0;

                    var SGSTAmt = 0;
                    var CGSTAmt = 0;
                    var IGSTAmt = 0;
                    var next_row = $(this).next("tr");

                    var TravelFromID = $(next_row).find(".travelfromId").val() || 0;
                    var TravelToID = $(next_row).find(".traveltoId").val() || 0;
                    var TransportModeID = $(next_row).find(".modeoftravelId").val() || 0;
                    var TravelingRemarks = $(next_row).find(".travelremark").text() || '';
                    var TravelDateString = $(next_row).find(".traveldate").text() || '';

                    if ($("#StateId").val() == $("#ShippingStateId").val()) {
                        SGSTPercent = Tot_GST_persc / 2;
                        CGSTPercent = SGSTPercent;

                        SGSTAmt = Tot_GST_Amt / 2;
                        CGSTAmt = SGSTAmt;
                    } else {
                        IGSTPercent = Tot_GST_persc;
                        IGSTAmt = Tot_GST_Amt;
                    }

                    ProductsArray.push({
                        ItemID: ItemID,
                        PurchaseReqID: PurchaseReqID,
                        PRTransID: PRTransID,
                        Quantity: Quantity,
                        Rate: Rate,
                        Amount: Amount,
                        NetAmount: NetAmount,
                        SGSTPercent: SGSTPercent,
                        CGSTPercent: CGSTPercent,
                        IGSTPercent: IGSTPercent,
                        SGSTAmt: SGSTAmt,
                        CGSTAmt: CGSTAmt,
                        IGSTAmt: IGSTAmt,
                        Remarks: Remarks,
                        LastPurchaseRate: LastPurchaseRate,
                        LowestPR: LowestPR,
                        QtyInQC: QtyInQC,
                        QtyAvailable: QtyAvailable,
                        QtyOrdered: QtyOrdered,
                        ServiceLocationID: $(this).find('.location-id').val(),
                        EmployeeID: $(this).find('.employee-id').val(),
                        DepartmentID: $(this).find('.department-id').val(),
                        CompanyID: $(this).find('.company-id').val(),
                        ProjectID: $(this).find('.project-id').val(),
                        TravelFromID: TravelFromID,
                        TravelToID: TravelToID,
                        TransportModeID: TransportModeID,
                        TravelingRemarks: TravelingRemarks,
                        TravelDateString: TravelDateString
                    });
                }

            })
            return ProductsArray;
        }
    }
};
//Common Calcultions

//calculate GST outside the grid
function CalculateGSTOutsideTheGrid() {
    var TotalGSTAmount = 0;
    $("#service-purchase-order-items-list tr.included").each(function () {
        var obj = clean($(this).find(".txtClGSTAmt").val());
        TotalGSTAmount = TotalGSTAmount + obj;
    });
    if ($("#StateId").val() == $("#ShippingStateId").val()) {
        $("#SGSTAmt").val((TotalGSTAmount / 2).toFixed(2));
        $("#CGSTAmt").val((TotalGSTAmount / 2).toFixed(2));
        $("#IGSTAmt").val(0);
    } else {
        $("#SGSTAmt").val(0);
        $("#CGSTAmt").val(0);
        $("#IGSTAmt").val((TotalGSTAmount).toFixed(2));
    }
}

//CalculateGST Inside the grid
function CalculateGSTInsideGrid(e) {
    var Rate = clean(e.find(".txtclRate").val());
    var Qty = clean(e.find(".clRqQty").val());
    var percent = clean(e.find(".txtClGSTPer").val());
    if ($("#IsGSTRegistred").val().toLowerCase() == "true") {
        if ($('#GST').val() == 1) {

            var vRate = Rate * 100 / (100 + percent);
            var GstAmount = (Rate - vRate) * Qty;
            var Total = (vRate * Qty) + GstAmount;

            e.find(".txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else if ($('#GST').val() == 2) {
            var vRate = Rate;
            var GstAmount = Rate * Qty * percent / 100;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else {
            var vRate = Rate;
            var GstAmount = 0;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".txtClGSTAmt").val(GstAmount.toFixed(2));
            e.find(".clTotal").val(Total);
        }
    } else {
        e.find(".txtClGSTAmt").val(0);
        e.find(".clTotal").val(Rate * Qty);
    }
    CalculateGSTOutsideTheGrid();
    CalculateNetAmountValue();
}

function CalculateNetAmountValue() {
    var TotalProductAmount = 0;

    var discount = clean($('#Discount').val());
    var deductions = clean($('#OtherDeductions').val());

    $(".poTbody .rowPO.included").each(function () {
        TotalProductAmount = TotalProductAmount + clean($(this).find(".clTotal").val());
    });
    TotalProductAmount = TotalProductAmount - discount + deductions;
    $("#NetAmt").val(TotalProductAmount);

}

purchase_order_CalculateEvents = {
    calculations: function () {

        $(document).on('change', '.txtClGSTPer', function () {
            CalculateGSTInsideGrid($(this).closest('tr'));
        });
        $('#GST').on('change', function () {
            if ($(this).val() == null) {
                return;
            }
            $("#service-purchase-order-items-list tbody tr .clRqQty").each(function () {
                CalculateValueInGrid($(this));
            });
        })


        $(document).on('change keyup', '.clRqQty , .txtclRate', function () {
            CalculateValueInGrid($(this).closest('tr'));
        });


        $(document).on('change', '#FreightAmt,#OtherCharges,#PackingShippingCharge', function () {
            //Calculate net amount
            CalculateNetAmountValue();

        });
        // written by lini on 07/05/2018
        $("#Qty, #Rate").keyup(function () {

            $("#txtValue").val(clean($("#Rate").val()) * clean($("#Qty").val()));
        });

        //$("#Qty, #Rate").on("change", function () {
        //    $("#txtValue").val(($("#Rate").val() * $("#Qty").val()).toFixed(2));
        //});
    }
}

function CalculateValueInGrid(parent) {
    e = parent.closest('tr');
    var Rate = clean(e.find(".txtclRate").val());
    var Qty = clean(e.find(".clRqQty").val());
    e.find(".txtclValue").val((Rate * Qty).toFixed(2));
    CalculateGSTInsideGrid(e);
}