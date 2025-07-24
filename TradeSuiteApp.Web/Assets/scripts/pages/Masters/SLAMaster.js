
SLA = {
    init: function () {
        var self = SLA;
        self.bind_events();
    },
    list: function () {
        var self = SLA;
        var $list = $('#tbl-sla-list');
        $list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/SLA/Details/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/SLA/GetAllSLAList";
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": true,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "Cycle", Value: $('#Cycle').val() },
                            { Key: "TransactionType", Value: $('#TransactionType').val() },
                            { Key: "KeyValue", Value: $('#KeyValue').val() },
                            { Key: "Item", Value: $('#Item').val() }
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
                    { "data": "Cycle", "className": "Cycle" },
                    { "data": "TransactionType", "className": "TransactionType" },
                    { "data": "KeyValue", "className": "KeyValue" },
                    { "data": "Item", "className": "Item" },
                    { "data": "Supplier", "className": "Supplier" },
                    { "data": "Customer", "className": "Customer" },
                    { "data": "ItemAccountsCategory", "className": "ItemAccountsCategory" },
                    { "data": "ItemTaxCategory", "className": "ItemTaxCategory" },
                    { "data": "BatchPrefix", "className": "BatchPrefix" },
                    { "data": "SupplierAccountsCategory", "className": "SupplierAccountsCategory" },
                    { "data": "SupplierTaxCategory", "className": "SupplierTaxCategory" },
                    { "data": "SupplierTaxSubCategory", "className": "SupplierTaxSubCategory" },
                    { "data": "CustomerTaxCategory", "className": "CustomerTaxCategory" },
                    { "data": "CustomerAccountsCategory", "className": "CustomerAccountsCategory" },
                    { "data": "CostComponent", "className": "CostComponent" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    { "data": "Capitilization", "className": "Capitilization" },
                    { "data": "DepartmentCategory", "className": "DepartmentCategory" },
                    { "data": "DebitAccount", "className": "DebitAccount" },
                    { "data": "DebitAccountDescription", "className": "DebitAccountDescription" },
                    { "data": "CreditAccount", "className": "CreditAccount" },
                    { "data": "CreditAccountDescription", "className": "CreditAccountDescription" }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                }
            });

            $('body').on("change", '#Cycle', function () {
                list_table.api().column(1).search(this.value).draw();
                list_table.api().column(2).search('').draw();
                list_table.api().column(3).search('').draw();
                setTimeout(function () {
                    if ($('#Cycle').val() != "") {
                        self.animate_list('Cycle');
                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 1000);
                    }
                }, 1000);
            });
            $('body').on("change", '#TransactionType', function () {
                list_table.api().column(2).search(this.value).draw();
                list_table.api().column(3).search('').draw();
                setTimeout(function () {
                    if ($('#TransactionType').val() != "") {
                        self.animate_list('TransactionType');

                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 1000);
                    }
                }, 1000);
            });
            $('body').on("change", '#KeyValue', function () {
                list_table.api().column(3).search(this.value).draw();
                setTimeout(function () {
                    if ($('#KeyValue').val() !== '') {
                        self.animate_list('KeyValue');
                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 100);
                    }
                }, 1000)
            });
            $('body').on("change", '#SLAItem', function () {
                list_table.api().column(4).search(this.value).draw();
                setTimeout(function () {
                    if ($('#SLAItem').val() != "") {
                        self.animate_list('Item');
                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 100);
                    }
                }, 1000)
            });
            $('body').on("change", '#SLASupplier', function () {
                list_table.api().column(5).search(this.value).draw();
                setTimeout(function () {
                    if ($('#SLASupplier').val() != "") {
                        self.animate_list('Supplier');
                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 100);
                    }
                }, 1000)
            });
            $('body').on("change", '#SLACustomer', function () {
                console.log(this.value);
                list_table.api().column(6).search(this.value).draw();
                setTimeout(function () {
                    if ($('#SLACustomer').val() != "") {
                        self.animate_list('Customer');
                    } else {
                        $("#wrapper").animate({ scrollLeft: 0 }, 100);
                    }
                }, 1000)
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
    animate_list: function (item) {
        $("#wrapper").animate({ scrollLeft: 0 }, 0);
        $("#wrapper").animate({ scrollLeft: $('th.' + item).offset().left + $('th.' + item).width() - 26 }, 700);
    },
    bind_events: function () {
        var self = SLA;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $.UIkit.autocomplete($('#creditaccountnumber-autocomplete'), { 'source': self.get_CreditAccount, 'minLength': 1 });
        $('#creditaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_CreditAccount);
        $.UIkit.autocomplete($('#debitaccountnumber-autocomplete'), { 'source': self.get_DebitAccount, 'minLength': 1 });
        $('#debitaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_DebitAccount);
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $(".BtnSaveAsDraft").on("click", self.save);
        $(".BtnSave").on("click", self.save_confirm);
        $('#Cycle').on('change', self.get_transactiontype);
        $('#TransactionType').on('change', self.get_SLAKeyValue);
        //self.common_load();
    },

    save_confirm: function () {
        var self = SLA
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
            data: {
                term: $('#Supplier').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    set_supplier_details: function (event, item) {
        $('#SupplierID').val(item.id);
    },
    get_customers: function (release) {
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#Customer').val(),
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
        $("#CustomerID").val(item.id);
    },
    get_CreditAccount: function (release) {
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadsForSLAAutoComplete',
            data: {
                Hint: $('#CreditAccount').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_CreditAccount: function (event, item) {
        $("#CreditAccountID").val(item.id);
        $("#CreditAccount").val(item.number);
        $("#CreditAccountDescription").val(item.name);
    },
    get_DebitAccount: function (release) {
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadsForSLAAutoComplete',
            data: {
                Hint: $('#DebitAccount').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_DebitAccount: function (event, item) {
        $("#DebitAccountID").val(item.id);
        $("#DebitAccount").val(item.number);
        $("#DebitAccountDescription").val(item.name);
    },
    get_transactiontype: function () {
        var self = SLA;
        var ProcessCycle = $('#Cycle').val();
        if (ProcessCycle != undefined) {
            $.ajax({
                url: '/Masters/SLA/GetTransactionTypeByProcessCycle',
                dataType: "json",
                type: "GET",
                data: {
                    ProcessCycle: ProcessCycle
                },
                success: function (response) {
                    var html, select;
                    html = select = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.TransactionType + "'>" + record.TransactionType + "</option>";
                    });
                    $("#TransactionType").html("");
                    $("#TransactionType").append(html);
                    $("#KeyValue").html("");
                    $("#KeyValue").append(select);
                    $("#KeyValue").val('');
                    $("#TransactionType").val('');
                }
            });
        }
    },
    get_SLAKeyValue: function () {
        var self = SLA;
        var TransactionType = $('#TransactionType').val();
        $('#TransactionType').val(TransactionType);
        if (TransactionType != undefined) {
            $.ajax({
                url: '/Masters/SLA/GetSLAKeyValueByTransactionType',
                dataType: "json",
                type: "GET",
                data: {
                    TransactionType: TransactionType
                },
                success: function (response) {
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.KeyValue + "'>" + record.KeyValue + "</option>";
                    });
                    $("#KeyValue").html("");
                    $("#KeyValue").append(html);
                    $("#KeyValue").val('');
                }
            });
        } else {
            var html = "<option value >Select</option>";
            $("#KeyValue").html("");
            $("#KeyValue").append(html);
        }
    },
    common_load: function () {
        self = SLA
        var SelectAllNoOption = [{
            text: "ALL",
            value: 0
        }, {
            text: "No",
            value: -1
        }];
        $("#ItemAccountsCategory").prepend(self.common_dropdown(SelectAllNoOption));
        $("#CustomerAccountsCategory").prepend(self.common_dropdown(SelectAllNoOption));
        $("#SupplierAccountsCategory").prepend(self.common_dropdown(SelectAllNoOption));
        $("#CustomerCategory").prepend(self.common_dropdown(SelectAllNoOption));
        $("#SupplierTaxSubCategory").prepend(self.common_dropdown(SelectAllNoOption));
        $("#ItemTaxCategory").prepend(self.common_dropdown(SelectAllNoOption));
    },
    common_dropdown: function (content) {
        var result = "";
        var selected;
        for (var i = 0; i < content.length; i++) {
            i == 0 && clean($('#ID').val()) == 0 ? selected = "selected='selected'" : selected = "";
            result = result + "<option value='" + content[i].value + "'" + selected + " >" + content[i].text + "</option>";
        }
        return result;
    },
    save: function (event) {
        var self = SLA;
        self.error_count = 0;
        var FormType = "Form";
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var model = self.get_data();
        $(".BtnSave").css({ 'visibility': 'hidden' });
        $.ajax({
            url: '/Masters/SLA/Save',
            data: { model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("SLA created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/SLA/Index"
                    }, 1000);
                } else if (data.Status == 409) {
                    app.show_error("SLA Already created ");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".BtnSaveAsDraft").css({ 'visibility': 'visible' });
                } else {
                    app.show_error("Failed to create SLA");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".BtnSaveAsDraft").css({ 'visibility': 'visible' });
                }
            },
        });
    },
    validate_form: function () {
        var self = SLA;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAllItemsForAutoComplete',
            data: {
                Hint: $('#Item').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, items) {
        event.preventDefault();
        $("#Item").val(items.value);
        $("#ItemID").val(items.id);

    },
    get_data: function () {

        var model = {
            ID: clean($('#ID').val()),
            Cycle: $('#Cycle').val(),
            Remarks: $('#Remarks').val(),
            TransactionType: $('#TransactionType').val(),
            KeyValue: $('#KeyValue').val(),
            Guidance: $('#Guidance').val(),
            Item: $('#ItemID').val(),
            ItemAccountsCategory: $('#ItemAccountsCategory').val(),
            ItemTaxCategory: $('#ItemTaxCategory').val(),
            BatchPrefix: $('#BatchPrefix').val(),
            Supplier: $('#SupplierID').val(),
            SupplierAccountsCategory: $('#SupplierAccountsCategory').val(),
            SupplierTaxCategory: $('#SupplierTaxCategory').val(),
            SupplierTaxSubCategory: $('#SupplierTaxSubCategory').val(),
            Customer: $('#CustomerID').val(),
            CustomerAccountsCategory: $('#CustomerAccountsCategory').val(),
            CustomerCategory: $('#CustomerCategory').val(),
            CustomerTaxCategory: $('#CustomerTaxCategory').val(),
            CostComponent: $('#CostComponent').val(),
            DepartmentCategory: $('#DepartmentCategory').val(),
            Capitilization: $('#Capitilization').val(),
            Location: $('#Location').val(),
            Condition1: $('#Condition1').val(),
            Condition2: $('#Condition2').val(),
            DebitAccount: $('#DebitAccount').val(),
            DebitAccountDescription: $('#DebitAccountDescription').val(),
            CreditAccount: $('#CreditAccount').val(),
            CreditAccountDescription: $('#CreditAccountDescription').val(),
            EntryInLocation: $('#EntryInLocation').val(),
            EntryInDepartment: $('#EntryInDepartment').val(),
            EntryInEmployee: $('#EntryInEmployee').val(),
            EntryInInterCompanyField: $('#EntryInInterCompanyField').val(),
            EntryInProjectField: $('#EntryInProjectField').val(),
            ItemSubLedger: $('#ItemSubLedger').val(),
            SupplierSubLedger: $('#SupplierSubLedger').val(),
            CustomerSubLedger: $('#CustomerSubLedger').val(),
            EmployeeSubLedger: $('#EmployeeSubLedger').val(),
            AssetsSubLedger: $('#AssetsSubLedger').val(),
            PatientsSubLedger: $('#PatientsSubLedger').val(),
            BankCashSubLedger: $('#BankCashSubLedger').val(),
            StartDateStr: $('#StartDateStr').val(),
            EndDateStr: $('#EndDateStr').val(),
        };
        return model;
    },
    rules: {
        on_submit: [
            {
                elements: "#Cycle",
                rules: [
                   { type: form.required, message: "Cycle is required" },
                ]
            },
            {
                elements: "#TransactionType",
                rules: [
                    {
                        type: function () {
                            var error = false;
                            var TransactionType = $('#TransactionType').val();
                            var Cycle = $('#Cycle').val();
                            if (Cycle != "" && TransactionType == "")
                                error = true;
                            return !error;
                        }, message: 'Transaction Type is required'
                    }
                ]
            },
            {
                elements: "#KeyValue",
                rules: [
                    {
                        type: function () {
                            var error = false;
                            var TransactionType = $('#TransactionType').val();
                            var KeyValue = $('#KeyValue').val();
                            if (TransactionType != "" && KeyValue == "")
                                error = true;
                            return !error;
                        }, message: 'Key Value is required'
                    }
                ]
            },
            {
                elements: "#Item",
                rules: [
                   { type: form.required, message: "Item is required" },
                ]
            },
            {
                elements: "#Item",
                rules: [
                    {
                        type: function () {
                            var error = false;
                            var ItemID = $('#ItemID').val();
                            var Item = $('#Item').val();
                            if (Item != "" && ItemID == "") {
                                error = true;
                            }
                            return !error;
                        }, message: 'Choose Item Correctly'
                    }
                ]
            },
            {
                elements: "#Supplier",
                rules: [
                   { type: form.required, message: "Supplier is required" },
                ]
            },
            {
                elements: "#Supplier",
                rules: [
                   {
                       type: function () {
                           var error = false;
                           var SupplierID = $('#SupplierID').val();
                           var Supplier = $('#Supplier').val();
                           if (Supplier != "" && SupplierID == "") {
                               error = true;
                           }
                           return !error;
                       }, message: 'Choose Supplier Correctly'
                   }
                ]
            },
            {
                elements: "#Customer",
                rules: [
                   { type: form.required, message: "Customer is required" },
                ]
            },
            {
                elements: "#Customer",
                rules: [
                   {
                       type: function () {
                           var error = false;
                           var CustomerID = $('#CustomerID').val();
                           var Customer = $('#Customer').val();
                           if (Customer != "" && CustomerID == "") {
                               error = true;
                           }
                           return !error;
                       }, message: 'Choose Customer Correctly'
                   }
                ]
            },

            {
                elements: "#EndDateStr",
                rules: [

            {
                type: function (element) {
                    var u_date = $(element).val().split('-');
                    var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                    var a = Date.parse(used_date);
                    var po_date = $('#StartDateStr').val().split('-');
                    var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                    var date = Date.parse(po_datesplit);
                    return date <= a
                }, message: "End date should be a date on or after start date"
            }

                ]
            },

                        {
                            elements: "#ItemID",
                            rules: [
                                {
                                    type: function (element) {
                                        var Item = $('#Item').val();
                                        var ItemID = $('#ItemID').val();
                                        if (Item == "ALL" || Item == "No") {
                                            return true;
                                        }
                                        else if (ItemID > 0) {
                                            return true;
                                        }
                                        return false
                                    }, message: "Please Select a Valid Item"
                                },
                            ]
                        },
            {
                elements: "#SupplierID",
                rules: [
                    {
                        type: function (element) {
                            var Supplier = $('#Supplier').val();
                            var SupplierID = $('#SupplierID').val();
                            if (Supplier == "ALL" || Supplier == "No") {
                                return true;
                            }
                            else if (SupplierID > 0) {
                                return true;
                            }
                            return false
                        }, message: "Please Select a Valid Supplier"
                    },
                ]
            },
            {
                elements: "#CustomerID",
                rules: [
                    {
                        type: function (element) {
                            var Customer = $('#Customer').val();
                            var CustomerID = $('#CustomerID').val();
                            if (Customer == "ALL" || Customer == "No") {
                                return true;
                            }
                            else if (CustomerID > 0) {
                                return true;
                            }
                            return false
                        }, message: "Please Select a Valid Customer"
                    },
                ]
            },
        ]
    }
};
