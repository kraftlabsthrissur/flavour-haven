var freeze_header;
JobWorkReceipt = {
    init: function () {
        var self = JobWorkReceipt;
        var index = $("#JobWork-receipt-items-list tbody tr").length;
        $("#ReceiptTableLength").val(index);
        var issueindex = $("#JobWork-Issue-item-list tbody tr").length;
        $("#IssueTableLength").val(issueindex);
        JobWorkIssue.issue_list();
        select_Issue_table = $('#issue-list').SelectTable({
            selectFunction: self.select_issue,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-issue",
            initiatingElement: "#IssueNo",
            selectionType: "radio"
        });
        item_list = Item.stockable_items_list();
        item_select_table = $('#stockable-items-list').SelectTable({
            selectFunction: self.select_receipt_item,
            returnFocus: "#txtRequiredQty",
            modal: "#select-stockable-items",
            initiatingElement: "#ItemName",
            startFocusIndex: 2
        });
        supplier.supplier_list('service');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        self.bind_events();
        freeze_header = $("#JobWork-Issue-item-list").FreezeHeader();
        freeze_header = $("#JobWork-receipt-items-list").FreezeHeader();

    },

    list: function () {
        $list = $('#receipt-list-table');
        $list.find('tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Manufacturing/JobWorkReceipt/Details/' + id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    select_item: function () {
        var self = JobWorkReceipt;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        $("#ReceiptItemID").val(ID);
        $("#ReceiptCode").val(Code);
        $("#ReceiptUnit").val(Unit);
        $("#ReceiptItemName").val(Name);
        UIkit.modal($('#select-item')).hide();
    },

    select_supplier: function () {
        self = JobWorkReceipt;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        $("#Supplier").val(Name);
        $("#SupplierLocation").val(Location);
        $("#SupplierID").val(ID).trigger('change');
        $("#StateId").val(StateID);
        $('#IssueNo').val('');
        UIkit.modal($('#select-supplier')).hide();
    },

    select_issue: function () {
        self = JobWorkReceipt;
        var radio = $('#select-issue tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var SupplierID = $(row).find(".supplier-ID").val();
        var IssueNO = $(row).find(".IssueNo").text().trim();
        var SupplierName = $(row).find(".SupplierName").text().trim();
        var IssueDate = $(row).find(".IssueDate").text().trim();
        $("#IssueNo").val(IssueNO);
        $("#Supplier").val(SupplierName);
        $("#SupplierID").val(SupplierID);
        $("#IssueID").val(ID);
        self.get_issue_items(ID, IssueDate);
        UIkit.modal($('#select-issue')).hide();
    },

    get_issue_items: function (ID, IssueDate) {
        var self = JobWorkReceipt;
        var tr = "";
        var $tr;
        $.ajax({
            url: '/Manufacturing/JobWorkIssue/GetIssueItems',
            dataType: "json",
            data: {
                IssueID: ID,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        var PendingQty = record.IssueQty - record.QtyMet
                        var checked = '';
                        if (PendingQty == 0) {
                            checked = 'checked';
                        }
                        tr += '<tr>'
                        + '<td>' + (i + 1)
                        + '<input type="hidden" class="issue-ItemID" value="' + record.IssueItemID + '" />'
                        + '<input type="hidden" class="issue-TransID" value="' + record.IssueTransID + '" />'
                        + '</td>'
                        + '<td class ="issue-item">' + record.IssueItemName + '</td>'
                        + '<td class ="issue-unit">' + record.IssueUnit + '</td>'
                        + '<td><input type="text" class = "md-input mask-production-qty issue-qty" value="' + record.IssueQty + '" readonly /></td>'
                        + '<td class ="issue-date">' + IssueDate + '</td>'
                        + '<td><input type="text" class = "md-input mask-production-qty pending-qty" value="' + PendingQty + '"/></td>'
                        + '<td class="uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck ' + checked + ' class="md-input check-box"/>' + '</td>'
                        + '</tr>';
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $('#JobWork-Issue-item-list tbody').html($tr);
                    var issueindex = $("#JobWork-Issue-item-list tbody tr").length;
                    $("#IssueTableLength").val(issueindex);

                }
            }
        });
    },

    bind_events: function () {
        var self = JobWorkReceipt;
        $("body").on("click", "#btnOKSupplier", self.select_supplier);
        $("body").on("click", "#btnOKIssue", self.select_issue);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $.UIkit.autocomplete($('#Issue-autocomplete'), { 'source': self.get_issue, 'minLength': 1 });
        $('#Issue-autocomplete').on('selectitem.uk.autocomplete', self.set_issue_details);
        $.UIkit.autocomplete($('#ReceiptItem-autocomplete'), { 'source': self.get_receipt_items, 'minLength': 1 });
        $('#ReceiptItem-autocomplete').on('selectitem.uk.autocomplete', self.set_receipt_item_details);
        $("body").on("click", "#btnAddReceipt-Item", self.add_receipt_items);
        $("body").on("click", ".remove-item", self.delete_receipt);
        $("body").on("click", "#btnOKStockableItem", self.select_receipt_item);
        $("body").on('click', '.btnSave,.btnSaveASDraft,.btnSaveNew', self.save);
    },

    save: function () {
        var self = JobWorkReceipt;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var data = self.get_data();
        if ($(this).hasClass('btnSaveASDraft')) {
            data.IsDraft = true;
        }
        var location = "/Manufacturing/JobWorkReceipt/Index";
        if ($(this).hasClass('btnSaveNew')) {
            location = "/Manufacturing/JobWorkReceipt/Create";
        }

        $.ajax({
            url: '/Manufacturing/JobWorkReceipt/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);

                }
            }
        });
    },

    get_data: function () {
        var self = JobWorkReceipt;
        var data = {};
        data.ID = $("#ID").val();
        data.TransNo = $("#TransNo").val();
        data.TransDate = $("#TransDate").val();
        data.SupplierID = $("#SupplierID").val();
        data.IssueID = $("#IssueID").val();
        data.IssuedItems = [];
        var Issueitem = {};
        $('#JobWork-Issue-item-list tbody tr ').each(function () {
            Issueitem = {};
            Issueitem.IssueTransID = $(this).find(".issue-TransID").val();
            Issueitem.PendingQuantity = $(this).find(".pending-qty").val();
            if (($(".check-box").prop('checked') == true) || Issueitem.PendingQuantity==0) {
                Issueitem.IsCompleted = true;
            }
            else {
                Issueitem.IsCompleted = false;
            }
            data.IssuedItems.push(Issueitem);
        });
        data.ReceiptItems = [];
        var Receiptitem = {};
        $('#JobWork-receipt-items-list tbody tr ').each(function () {
            Receiptitem = {};
            Receiptitem.ReceiptItemID = $(this).find(".receipt-item-ID").val();
            Receiptitem.ReceiptUnit = $(this).find(".receipt-unit").text();
            Receiptitem.ReceiptQty = $(this).find(".receipt-qty").val();
            Receiptitem.ReceiptDate = $(this).find(".receipt-Date").text();
            Receiptitem.WarehouseID = $(this).find(".warehouse-ID").val();
            data.ReceiptItems.push(Receiptitem);
        });
        return data;
    },

    select_receipt_item: function () {
        var self = JobWorkIssue;
        var radio = $('#stockable-items-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var ItemCategory = $(row).find(".ItemCategory").val();
        $("#ReceiptItemID").val(ID);
        $("#ReceiptCode").val(Code);
        $("#ReceiptUnit").val(Unit);
        $("#ReceiptItemName").val(Name);

    },

    validate_form: function () {
        var self = JobWorkReceipt;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    delete_receipt: function () {
        var self = JobWorkReceipt;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#JobWork-receipt-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#ReceiptTableLength").val($("#JobWork-receipt-items-list tbody tr").length);

    },

    add_receipt_items: function () {
        var self = JobWorkReceipt;
        if (self.validate_jobWorkReceipt() > 0) {
            return;
        }
        var ReceiptItemName = $("#ReceiptItemName").val();
        var ReceiptUnit = $("#ReceiptUnit").val();
        var ReceiptQty = $("#ReceiptQty").val();
        var ReceiptItemID = $("#ReceiptItemID").val();
        var ReceiptDate = $("#ReceiptDate").val();
        var WarehouseID = $("#WarehouseID").val();
        var content = "";
        var $content;
        var sino = "";
        sino = $('#JobWork-receipt-items-list tbody tr').length + 1;
        content = '<tr>'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class="receipt-item-name">' + ReceiptItemName
            + '<input type="hidden" class = "receipt-item-ID" value="' + ReceiptItemID + '" />'
            + '<input type="hidden" class = "warehouse-ID" value="' + WarehouseID + '" />'
            + '</td>'
            + '<td class="receipt-unit">' + ReceiptUnit + '</td>'
            + '<td><input type="text" class = "md-input mask-production-qty receipt-qty" value="' + ReceiptQty + '" /></td>'
            + '<td class="receipt-Date">' + ReceiptDate + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#JobWork-receipt-items-list tbody').append($content);
        freeze_header.resizeHeader();
        index = $("#JobWork-receipt-items-list tbody tr").length;
        $("#ReceiptTableLength").val(index);
        self.clear_data();
    },

    clear_data: function () {
        var self = JobWorkReceipt;
        $("#ReceiptItemName").val('');
        $("#ReceiptUnit").val('');
        $("#ReceiptItemID").val('');
        $("#ReceiptQty").val('');
        $("#ReceiptDate").val('');
    },

    validate_jobWorkReceipt: function () {
        var self = JobWorkReceipt;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
            {
                elements: "#ReceiptItemID",
                rules: [
                    { type: form.required, message: "Please choose an ReceiptItem", alt_element: "#ReceiptItemName" },
                    { type: form.positive, message: "Please choose an ReceiptItem", alt_element: "#ReceiptItemName" },
                    { type: form.non_zero, message: "Please choose an ReceiptItem", alt_element: "#ReceiptItemName" },
                    {
                      type: function (element) {
                                var item_id = $(element).val();
                                var error = false;
                                $('#JobWork-receipt-items-list tbody tr').each(function () {
                                    if ($(this).find('.receipt-item-ID').val() == item_id) {
                                        error = true;
                                         }
                                         });
                                return !error;
                      },
                      message: "Item already added in the grid, try editing receipt quantity"
                    }

                ],
            },
            {
                elements: "#ReceiptQty",
                rules: [
                    { type: form.required, message: "Please Fill Quantity" },
                    { type: form.positive, message: "Please Fill Quantity" },
                    { type: form.non_zero, message: "Please Fill Quantity" }
                ],
            },
            {
                elements: "#ReceiptDate",
                rules: [
                    { type: form.required, message: "Please Fill ReceiptDate" },
                    { type: form.positive, message: "Please Fill ReceiptDate" },
                    { type: form.non_zero, message: "Please Fill ReceiptDate" }
                ],
            },
            {
                elements: "#WarehouseID",
                rules: [
                     { type: form.required, message: "Please choose an Store"},
                    { type: form.positive, message: "Please choose an Store" },
                    { type: form.non_zero, message: "Please choose an Store" }
                ],
            }

        ],
        on_save: [
            {
                elements: "#ReceiptTableLength",
                rules: [
                    { type: form.required, message: "Please add atleast one Receiptitem" },
                    { type: form.non_zero, message: "Please add atleast one Receiptitem" },
                ],
            },
            {
                elements: "#IssueTableLength",
                rules: [
                    { type: form.required, message: "Please add atleast one Issueitem" },
                    { type: form.non_zero, message: "Please add atleast one Issueitem" },
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
                elements: "#IssueID",
                rules: [
                    { type: form.required, message: "Please choose an Issue" },
                    { type: form.positive, message: "Please choose an Issue" },
                    { type: form.non_zero, message: "Please choose an Issue" }
                ],
            },
             {
                 elements: "#JobWork-receipt-items-list",
                 rules: [
                     {
                         type: function (element) {
                             var error = false;
                             $('#JobWork-receipt-items-list tbody tr').each(function () {
                                 if ($(this).find('.receipt-qty').val() == 0) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "ReceiptQuantity Should not be Zero"
                     }

                 ],
             },
             {
                 elements: "#JobWork-Issue-item-list",
                 rules: [
                     {
                         type: function (element) {
                             var error = false;
                             $('#JobWork-Issue-item-list tbody tr').each(function () {
                                 if ($(this).find('.pending-qty').val() > $(this).find('.issue-qty').val()) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "IssueQuantity Should not be Greater than PendingQuantity"
                     }

                 ],
             }

        ]
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

    get_issue: function (release) {
        $.ajax({
            url: '/Manufacturing/JobWorkIssue/GetIssueListForAutoComplete',
            data: {
                term: $('#IssueNo').val(),
                SupplierID: $('#SupplierID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_issue_details: function (event, item) {   // on select auto complete issue
        self = JobWorkReceipt;
        $("#IssueNo").val(item.value);
        $("#Supplier").val(item.supplier);
        $("#SupplierID").val(item.supplierId);
        $("#IssueID").val(item.id);
        $("#Issue-Date").val(item.issueDate);
        var ID = $("#IssueID").val();
        var IssueDate = $("#Issue-Date").val();
        self.get_issue_items(ID, IssueDate);
    },

    set_supplier_details: function (event, item) {   // on select auto complete supplier
        self = JobWorkReceipt;
        $("#SupplierName").val(item.value);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#SupplierID").trigger('change');
    },

    get_receipt_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAvailableStockItemsForAutoComplete',
            data: {
                Hint: $('#ReceiptItemName').val(),
                WarehouseID:$('#WarehouseID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_receipt_item_details: function (event, items) {   // on select auto complete item
        var self = JobWorkReceipt;
        $("#ReceiptItemID").val(items.id);
        $("#ReceiptUnit").val(items.unit);
        $("#ReceiptItemName").val(items.value);
    },
}