
var freeze_header;
JobWorkIssue = {
    init: function () {
        var self = JobWorkIssue;
        var index = $("#Job-Work-issue-items-list tbody tr").length;
        $("#item-count").val(index);
        var is_issueitem = "";
        item_list = Item.available_stock_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_issue_item,
            returnFocus: "#txtRequiredQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
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
        JobWorkIssue.bind_events();
        freeze_header = $("#Job-Work-issue-items-list").FreezeHeader();
        
    },
    select_issue_item: function () {
        var self = JobWorkIssue;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var ItemCategory = $(row).find(".ItemCategory").val();
        var Rate = $(row).find(".Rate").val();
        var StockValue = $(row).find(".Stock").val();
            $("#IssueItemID").val(ID);
            $("#IssueCode").val(Code);
            $("#IssueUnit").val(Unit);
            $("#IssueItemName").val(Name);
            $("#StockValue").val(StockValue);
            $("#Rate").val(Rate);

    },

    list: function () {
        $list = $('#JobWork-issue-list');
        $list.find('tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Manufacturing/JobWorkIssue/Details/' + id;
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

    select_supplier: function () {
        self = JobWorkIssue;
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
        UIkit.modal($('#select-supplier')).hide();
    },

    bind_events: function () {
        var self = JobWorkIssue;
        $("body").on("click", "#btnOKItem", self.select_issue_item);
        $("body").on("click", "#btnOKStockableItem", self.select_receipt_item);
        $("body").on("click", "#btnOKSupplier", self.select_supplier);
        //$("body").on("click", "#receiptsearch", self.open_receipt_item_modal);
        //$("body").on("click", "#issuesearch", self.open_issue_item_modal);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $.UIkit.autocomplete($('#IssueItem-autocomplete'), { 'source': self.get_issue_items, 'minLength': 1 });
        $('#IssueItem-autocomplete').on('selectitem.uk.autocomplete', self.set_issue_item_details);
        $.UIkit.autocomplete($('#ReceiptItem-autocomplete'), { 'source': self.get_receipt_items, 'minLength': 1 });
        $('#ReceiptItem-autocomplete').on('selectitem.uk.autocomplete', self.set_receipt_item_details);
        $("body").on("click", "#btnAddJob", self.add_issue);
        $("body").on("click", ".remove-item", self.delete_issue);
        $("body").on('click', '.btnSave,.btnSaveASDraft,.btnSaveNew', self.save);
        
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

    set_supplier_details: function (event, item) {   // on select auto complete supplier
        self = JobWorkIssue;
        $("#SupplierName").val(item.value);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#DDLPaymentWithin option:contains(" + item.paymentDays + ")").attr("selected", true);
        $("#SupplierLocation").focus();
        $("#SupplierID").trigger('change');
    },

    get_issue_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAvailableStockItemsForAutoComplete',
            data: {
                Hint: $('#IssueItemName').val(),
                WarehouseID: $("#WarehouseID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
       

    },

    set_issue_item_details: function (event, items) {   // on select auto complete item
        var self = JobWorkIssue;
        $("#IssueItemID").val(items.id);
        $("#IssueUnit").val(items.unit);
        $("#IssueItemName").val(items.value);
       
    },

    add_issue: function () {
        var self = JobWorkIssue;
        if (self.validate_jobWorkIssue() > 0) {
            return;
        }
        var IssueItemName = $("#IssueItemName").val();
        var IssueUnit = $("#IssueUnit").val();
        var IssueQty = $("#IssueQty").val();
        var IssueItemID = $("#IssueItemID").val();
        var txtIssueNo = $("#txtIssueNo").val();
        var txtIssueDate = $("#txtIssueDate").val();
        var SupplierName = $("#SupplierName").val();
        var WarehouseID = $("#WarehouseID").val();
        var Stock = $("#StockValue").val();
        var content = "";
        var $content;
        var sino = "";
        sino = $('#Job-Work-issue-items-list tbody tr').length + 1;
        content = '<tr>'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class="issue-item-name">' + IssueItemName
            + '<input type="hidden" class = "issueItem-ID" value="' + IssueItemID + '" />'
            + '<input type="hidden" class = "warehouse-ID" value="' + WarehouseID + '" />'
            + '</td>'
            + '<td class="issue-unit">' + IssueUnit + '</td>'
            + '<td><input type="text" class = "md-input mask-production-qty issue-qty" value="' + IssueQty + '" />'
            + '<input type="hidden" class = "stock" value="' + Stock + '" />'
            + '</td>'
            +'<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#Job-Work-issue-items-list tbody').append($content);
        freeze_header.resizeHeader();
        index = $("#Job-Work-issue-items-list tbody tr").length;
        $("#item-count").val(index);
        self.clear_data();
    },

    clear_data: function () {
        var self = JobWorkIssue;
        $("#IssueItemName").val('');
        $("#IssueUnit").val('');
        $("#IssueQty").val('');
        $("#IssueItemID").val('');
    },

    delete_issue: function () {
        var self = JobWorkIssue;
        $(this).closest('tr').remove();
        var sino = 0;
        $('Job-Work-issue-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#Job-Work-issue-items-list tbody tr").length);

    },
    save: function () {
        var self = JobWorkIssue;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var data = self.get_data();
        if ($(this).hasClass('btnSaveASDraft')) {
            data.IsDraft = true;
        }
        var location = "/Manufacturing/JobWorkIssue/Index";

        if ($(this).hasClass('btnSaveNew')) {
            location = "/Manufacturing/JobWorkIssue/Create";
        }

        $.ajax({
            url: '/Manufacturing/JobWorkIssue/Save',
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
        var self = JobWorkIssue;
        var data = {};
        data.ID = $("#ID").val();
        data.IssueNo = $("#txtIssueNo").val();
        data.IssueDate = $("#txtIssueDate").val();
        data.SupplierID = $("#SupplierID").val();
        data.Items = [];
        var item = {};
        $('#Job-Work-issue-items-list tbody tr ').each(function () {
            item = {};
            item.IssueItemName = $(this).find(".issue-item-name").text();
            item.IssueItemID = $(this).find(".issueItem-ID").val();
            item.IssueUnit = $(this).find(".issue-unit").text();
            item.IssueQty = $(this).find(".issue-qty").val();
            item.WarehouseID = $(this).find(".warehouse-ID").val();
            data.Items.push(item);
        });
        return data;
    },

    validate_form: function () {
        var self = JobWorkIssue;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_jobWorkIssue: function () {
        var self = JobWorkIssue;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.required, message: "Please choose an Supplier", alt_element: "#SupplierName" },
                    { type: form.positive, message: "Please choose an Supplier", alt_element: "#SupplierName" },
                    { type: form.non_zero, message: "Please choose an Supplier", alt_element: "#SupplierName" }
                ],
            },
            {
                elements: "#IssueItemID",
                rules: [
                    { type: form.required, message: "Please choose an IssueItem", alt_element: "#IssueItemName" },
                    { type: form.positive, message: "Please choose an IssueItem", alt_element: "#IssueItemName" },
                    { type: form.non_zero, message: "Please choose an IssueItem", alt_element: "#IssueItemName" },
                     {
                         type: function (element) {
                             var item_id = $(element).val();
                             var error = false;
                             $('#Job-Work-issue-items-list tbody tr').each(function () {
                                 if ($(this).find('.issueItem-ID').val() == item_id) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "Item already added in the grid, try editing issue quantity"
                     }
                ]
            },
            {
                elements: "#IssueQty",
                rules: [
                    { type: form.required, message: "Please Fill Quantity" },
                    { type: form.positive, message: "Please Fill Quantity" },
                    { type: form.non_zero, message: "Please Fill Quantity" },

                     {
                         type: function (element) {
                             return clean($(element).val()) <= clean($('#StockValue').val());
                         }, message: "Item out of stock"
                     }

                ],
            },
        ],

        on_save: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ],
            },
            {
                elements: '#Job-Work-issue-items-list .issue-qty',
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.stock').val());
                        }, message: "Item out of stock"
                    }
                ]
            }
        ]
    },

    issue_list: function () {
        var $list = $('#issue-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Manufacturing/JobWorkIssue/GetIssueList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "SupplierID", Value: $('#SupplierID').val() },
                            
                        ];
                    }
                },
                "columns": [
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
                            return "<input type='radio' class='uk-radio ItemID' name='ItemID' data-md-icheck value='" + row.ID + "' >"
                             + "<input type='hidden' class='issue-ID' value='" + row.ID + "'>"
                             + "<input type='hidden' class='supplier-ID' value='" + row.SupplierID + "'>"
                        }
                    },
                    { "data": "IssueNo", "className": "IssueNo" },
                    { "data": "Supplier", "className": "SupplierName" },
                    { "data": "IssueDate", "className": "IssueDate" },
                ]
                ,
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on('change', '#SupplierID', function () {
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


}