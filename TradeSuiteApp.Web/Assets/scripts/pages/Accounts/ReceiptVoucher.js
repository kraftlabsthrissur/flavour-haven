var customer_list;
var freeze_header;
receipt = {
    init: function () {
        var self = receipt;
        customer_list = Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        self.bind_events();
        self.freeze_headers();
        self.process_item();
        self.settlements = [];
    },

    details: function () {
        var self = receipt;
        self.freeze_headers();
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
    },

    print: function () {
        var self = receipt;
        $.ajax({
            url: '/Accounts/Receipt/Print',
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
        var self = receipt;
        $.ajax({
            url: '/Reports/Accounts/ReceiptVoucherPrintPdf',
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

    freeze_headers: function () {
        freeze_header = $("#invoice-list").FreezeHeader();
    },

    list: function () {
        var self = receipt;
        self.tabbed_list("draft");
        self.tabbed_list("received");
        self.tabbed_list("cancelled");
    },

    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "received":
                $list = $('#received-list');
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
            altair_md.inputs($list);

            var url = "/Accounts/Receipt/GetReceiptVoucherList?type=" + type;

            var list_table = $list.dataTable({
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>"
                           + "<input type='hidden' class='CustomerID' value='" + row.CustomerID + "'>";

                       }
                   },

                   { "data": "ReceiptNo", "className": "ReceiptNo" },
                   { "data": "ReceiptDate", "className": "ReceiptDate" },
                   { "data": "CustomerName", "className": "Customer" },
                   { "data": "PaymentTypeName", "className": "PaymentTypeName" },
                    {
                        "data": "ReceiptAmount", "searchable": false, "className": "ReceiptAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.ReceiptAmount + "</div>";
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/Receipt/Details/" + Id);

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
        var self = receipt;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $('body').on('click', '#btnOKCustomer', self.select_customer);
        $('body').on('change', "#PaymentTypeID", self.get_bank_name);
        $('body').on('click', "#btnAddReceipt", self.process_item);
        $('body').on("click", ".btnSave", self.save_confirm);
        $('body').on("click", ".btnSaveDraft", self.save_draft);
        $('body').on('ifChanged', '.included', self.include_item);
    },

    save_confirm: function () {
        var self = receipt;
        $(".btnSave,.btnSaveDraft").css({ 'display': 'none' });
        app.confirm_cancel("Do you want to Save", function () {
            if (clean($("#ReceiptAmount").val()) > 0) {
                self.error_count = self.validate_form();
            }
            else {
                self.error_count = self.validate_receipt();
            }

            if (self.error_count > 0) {
                $(".btnSave,.btnSaveDraft").css({ 'display': 'block' });
                return;
            }
            else {
                self.save();
            }
        }, function () {
            $(".btnSave,.btnSaveDraft").css({ 'display': 'block' });
        })
    },
    include_item: function () {
        var self = receipt;
        var row = $(this).closest('tr');
        if ($(this).prop('checked') == true) {
            $(row).addClass("included");
        } else {
            $(row).removeClass("included");
        }
        self.process_item();
    },

    process_item: function () {
        var self = receipt;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }

        var receipt_amount = clean($("#ReceiptAmount").val());
        var c_amount = 0;
        var balance = 0;
        var settled_amount = 0;
        var used_amount = 0;
        var balance_amount = 0;

        var bill_amount_total = 0;
        var balance_total = 0;
        var matched_total = 0;
        var remaining_total = 0;
        var incdcr = 1;
        var DebitNoteID;
        var AdvanceID;
        var SalesReturnID;
        var SettlementFrom;

        var row;
        var obj = {};
        self.settlements = [];

        $("#invoice-list tbody .advancereceived").remove();
        $("#invoice-list tbody .summary").remove();

        $("#invoice-list tbody tr").each(function () {
            row = $(this).closest("tr");
            balance = clean($(row).find(".Balance").text());
            $(row).find(".BalanceHidden").val(balance);
            $(row).find(".SettledAmount").val(0);
            $(row).find(".BalanceToBePaid").val(0);
            $(row).find(".RemainingAmount").val(0);
        });

        $("#invoice-list tbody tr.included .BalanceToBePaid.mask-negative-currency").each(function () {
            balance_amount = c_amount = clean($(this).closest("tr").find(".Balance").text());
            used_amount = 0;
            DebitNoteID = $(this).closest("tr").find(".DebitNoteID").val();
            AdvanceID = $(this).closest("tr").find(".AdvanceID").val();
            SalesReturnID = $(this).closest("tr").find(".SalesReturnID").val();
            CustomerReturnVoucherID = $(this).closest("tr").find(".CustomerReturnVoucherID").val();
            SettlementFrom = $(this).closest("tr").find(".DocumentType").text();
            $("#invoice-list tbody tr.included .BalanceToBePaid.mask-currency").each(function () {
                row = $(this).closest("tr");
                balance = clean($(row).find(".BalanceHidden").val());
                obj = {};
                if (c_amount > 0 && balance > 0) {
                    settled_amount = clean($(row).find(".SettledAmount").val());
                    if (c_amount >= balance) {
                        $(row).find(".BalanceHidden").val(0);
                        $(row).find(".SettledAmount").val(settled_amount + balance);
                        c_amount -= balance;
                        used_amount += balance;
                        obj.SettlementAmount = balance;
                    } else {
                        $(row).find(".BalanceHidden").val(balance - c_amount);
                        $(row).find(".SettledAmount").val(settled_amount + c_amount);
                        used_amount += c_amount;
                        obj.SettlementAmount = c_amount;
                        c_amount = 0;
                    }
                    obj.DebitNoteID = DebitNoteID;
                    obj.AdvanceID = AdvanceID;
                    obj.SalesReturnID = SalesReturnID;
                    obj.CustomerReturnVoucherID = CustomerReturnVoucherID;
                    obj.ReceivableID = $(row).find(".InvoiceID").val();
                    obj.CreditNoteID = $(row).find(".CreditNoteID").val();
                    obj.DocumentType = $(row).find(".DocumentType").text();
                    obj.DocumentNo = $(row).find(".DocumentNo").text();
                    obj.Amount = balance_amount;
                    obj.SettlementFrom = SettlementFrom;
                    self.settlements.push(obj);
                }
            });
            $(this).closest("tr").find(".SettledAmount").val(used_amount);
        });

        balance_amount = receipt_amount;

        $("#invoice-list tbody tr.included .BalanceToBePaid.mask-currency").each(function () {
            row = $(this).closest("tr");
            balance = clean($(row).find(".BalanceHidden").val());
            if (receipt_amount > 0 && balance > 0) {
                settled_amount = clean($(row).find(".SettledAmount").val());
                obj = {};
                if (receipt_amount >= balance) {
                    $(row).find(".BalanceHidden").val(0);
                    $(row).find(".SettledAmount").val(settled_amount + balance);
                    receipt_amount -= balance;
                    used_amount += balance;
                    obj.SettlementAmount = balance;
                } else {
                    var hiddenbalanceamount = (balance - receipt_amount).roundTo(2);
                    var hiddensettlemaount = (settled_amount + receipt_amount).roundTo(2);
                    $(row).find(".BalanceHidden").val(hiddenbalanceamount);
                    $(row).find(".SettledAmount").val(hiddensettlemaount);
                    used_amount += receipt_amount;
                    obj.SettlementAmount = receipt_amount;
                    receipt_amount = 0;
                }

                obj.DebitNoteID = 0;
                obj.AdvanceID = 0;
                obj.SalesReturnID = 0;
                obj.CustomerReturnVoucherID = 0;
                obj.ReceivableID = $(row).find(".InvoiceID").val();
                obj.CreditNoteID = $(row).find(".CreditNoteID").val();
                obj.DocumentType = $(row).find(".DocumentType").text();
                obj.DocumentNo = $(row).find(".DocumentNo").text();
                obj.Amount = balance_amount;
                obj.SettlementFrom = "Receipt";
                self.settlements.push(obj);
            }
        });

        if (receipt_amount > 0) {
            var nowDate = new Date();
            receipt_amount = (receipt_amount).roundTo(2);
            var nowDay = ((nowDate.getDate().toString().length) == 1) ? '0' + (nowDate.getDate()) : (nowDate.getDate());
            var nowMonth = nowDate.getMonth() < 9 ? '0' + (nowDate.getMonth() + 1) : (nowDate.getMonth() + 1);
            var nowYear = nowDate.getFullYear();
            var formattedDate = nowDay + "-" + nowMonth + "-" + nowYear;
            var content = "";
            var $content;
            content = '<tr class="included advancereceived">'
                + '<td class="serial-no uk-text-center width-20">' + ($('#invoice-list tbody tr').length + 1) + '</td>'
                + '<td class="width-20"><input type="checkbox" checked data-md-icheck class="included" disabled="disabled" />'
                + ' <input type="hidden" class="BalanceHidden" value="' + receipt_amount + '" />'
                + ' <input type="hidden" class="SettledAmount" value="' + receipt_amount + '" />'
                + '</td>'
                + '<td class="DocumentType">' + 'ADVANCE RECEIVED' + '</td>'
                + '<td>' + '' + '</td>'
                + '<td class="InvoiceDate">' + formattedDate + '</td>'
                + '<td class="PendingDays">' + 0 + '</td>'
                + '<td class="mask-currency">' + '0.00' + '</td>'
                + '<td class="mask-currency">' + '0.00' + '</td>'
                + '<td><input type="text" class="md-input Sum BalanceToBePaid  mask-currency" disabled="disabled" value="' + receipt_amount + '"  /> </td>'
                + '<td><input type="text" class="md-input RemainingAmount mask-currency" disabled="disabled" value="0"  /> </td>'
                + '</tr>';
            $content = $(content);
            app.format($content);
            $('#invoice-list tbody').append($content);
        }

        $("#invoice-list tbody tr.included").each(function () {
            settled_amount = clean($(this).find(".SettledAmount").val());
            balance = clean($(this).find(".Balance").text());
            $(this).find(".BalanceToBePaid").val(settled_amount);
            $(this).find(".RemainingAmount").val(balance - settled_amount);

            incdcr = 1;
            if ($(this).find(".BalanceToBePaid").hasClass("mask-negative-currency")) {
                incdcr = -1;
            }

            bill_amount_total += incdcr * clean($(this).find(".Amount").text());
            balance_total += incdcr * clean($(this).find(".Balance").text());
            matched_total += clean($(this).find(".BalanceToBePaid").val());
            remaining_total += incdcr * clean($(this).find(".RemainingAmount").val());
        });

        $("#invoice-list tbody tr").each(function () {
            var BalanceTopay = clean($(this).find('.BalanceToBePaid').val());
            var Balance = clean($(this).find('.Balance').text());
            var Status = '';
            if (Balance == BalanceTopay) {
                Status = 'Settled';
            } else if (BalanceTopay != 0) {
                Status = 'Partial'
            }

            $(this).find('.Status').val(Status);
        });

        var summary = '<tr class="summary">'
               + '<td colspan="6"></td>'
               + '<td class="uk-text-bold mask-currency"  >' + bill_amount_total + '</td>'
               + '<td class="uk-text-bold mask-currency"  >' + balance_total + '</td>'
               + '<td class="uk-text-bold mask-currency"  >' + matched_total + '</td>'
               + '<td class="uk-text-bold mask-currency"  >' + remaining_total + '</td>'
               + '</tr>';
        $summary = $(summary);
        app.format($summary);
        $('#invoice-list tbody').append($summary);

        self.count_items();
    },

    save_draft: function () {
        var self = receipt;
        self.error_count = 0;
        self.error_count = self.validate_draft();
        if (self.error_count > 0)
            return;
        self.save(true);
    },

    save: function (IsDraft) {
        var self = receipt;
        var data;
        var url;
        $(".btnSave,.btnSaveDraft").css({ 'display': 'none' });
        self.error_count = 0;
        data = self.get_data();
        if (IsDraft) {
            data.IsDraft = true;
            url = '/Accounts/Receipt/SaveAsDraft';
        }
        else {
            url = '/Accounts/Receipt/Save';
        }

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Receipt Saved Successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/Receipt/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSave,.btnSaveDraft").css({ 'display': 'block' });
                }
            }
        });
    },

    get_data: function () {
        var self = receipt;
        var model = {
            ID: $("#ID").val(),
            ReceiptNo: $("#ReceiptNo").val(),
            ReceiptDate: $("#ReceiptDate").val(),
            CustomerID: $("#CustomerID").val(),
            ReceiptAmount: clean($("#ReceiptAmount").val()),
            PaymentTypeID: $("#PaymentTypeID").val(),
            BankID: $('#BankID').val(),
            Date: $('#Date').val(),
            BankReferanceNumber: $("#BankReferanceNumber").val(),
            Remarks: $("#Remarks").val(),
            ReceiverBankID: $("#ReceiverBankID").val(),
            BankInstrumentNumber: $("#BankInstrumentNumber").val(),
            ChecqueDate: $("#ChecqueDate").val(),


        };
        model.Item = self.GetReceivablesList();
        model.Settlements = self.settlements;
        return model;
    },

    GetReceivablesList: function () {
        var Receivables = [];
        $("#invoice-list tbody tr.included").each(function () {
            Receivables.push(
                {
                    ReceivableID: $(this).find('.InvoiceID').val(),
                    CreditNoteID: $(this).find('.CreditNoteID').val(),
                    DebitNoteID: $(this).find('.DebitNoteID').val(),
                    AdvanceID: $(this).find('.AdvanceID').val(),
                    Status: $(this).find('.Status').val(),
                    DocumentType: $(this).find('.DocumentType').text().trim(),
                    DocumentNo: $(this).find('.DocumentNo').text(),
                    ReceivableDate: $(this).find('.InvoiceDate').text(),
                    Amount: clean($(this).find('.Amount').text()),
                    Balance: clean($(this).find('.Balance').text()),
                    AmountToBeMatched: clean($(this).find('.BalanceToBePaid').val()),
                    AdvanceReceivedAmount: clean($(this).find('.Sum').val()),
                    PendingDays: clean($(this).find('.PendingDays').text()),
                    SalesReturnID: clean($(this).find('.SalesReturnID').val()),
                    CustomerReturnVoucherID: clean($(this).find('.CustomerReturnVoucherID').val()),
                }
            );
        })
        return Receivables;
    },

    get_bank_name: function () {
        var self = receipt;
        var mode;
        var Module = "Receipt"
        if ($("#PaymentTypeID option:selected").text() == "Select") {
            mode = "";
            $("#Date").val('');
        }
        else if ($("#PaymentTypeID option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
            var date = new Date();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var current_date = (day < 10 ? '0' : '') + day + '/' +
               (month < 10 ? '0' : '') + month + '/' +
               date.getFullYear();
            $("#Date").val(current_date);
        }
        else {
            mode = "Bank"
        }

        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                ModuleName: Module,
                Mode: mode
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#BankID").html("");
                var html = "";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });

    },

    set_customer: function (event, item) {
        var self = receipt;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#IsBlockedForChequeReceipt").val(item.isblockedForChequeReceipt);
        $("#ReceiptAmount").val('');
        self.get_invoices();

    },

    get_customers: function (release) {
        var self = receipt;

        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
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

    select_customer: function () {
        var self = receipt;
        var radio = $('#customer-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var IsBlockedForChequeReceipt = $(row).find(".IsBlockedForChequeReceipt").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#IsBlockedForChequeReceipt").val(IsBlockedForChequeReceipt);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        UIkit.modal($('#select-customer')).hide();
        $("#ReceiptAmount").val('');
        self.get_invoices();
    },

    get_invoices: function () {
        var self = receipt;
        self.error_count = 0;
        if (self.error_count > 0) {
            return;
        }
        var CustomerID = $("#CustomerID").val();

        $.ajax({
            url: '/Accounts/Receipt/GetInvoiceForReceiptVoucher/',
            dataType: "json",
            type: "POST",
            data: {
                CustomerID: CustomerID,
            },
            success: function (invoice_items) {
                var $invoice_items_list = $('#invoice-list tbody');
                $invoice_items_list.html('');
                var tr = '';
                var DocumentType;
                $.each(invoice_items, function (i, item) {
                    DocumentType = (item.DocumentType).replace(" ", "").toUpperCase();
                    tr += "<tr class='included'>"
                       + " <td class='uk-text-center'>" + (i + 1) + "</td>"
                    tr += ' <td><input type="checkbox" data-md-icheck checked class="included ' + DocumentType + '" />'
                    tr += ' <input type="hidden" class="InvoiceID" value="' + item.ID + '"/>'
                    tr += ' <input type="hidden" class="CreditNoteID" value="' + item.CreditNoteID + '" />'
                    tr += ' <input type="hidden" class="DebitNoteID" value="' + item.DebitNoteID + '" />'
                    tr += ' <input type="hidden" class="AdvanceID" value="' + item.AdvanceID + '" />'
                    tr += ' <input type="hidden" class="SalesReturnID" value="' + item.SalesReturnID + '" />'
                    tr += ' <input type="hidden" class="CustomerReturnVoucherID" value="' + item.CustomerReturnVoucherID + '" />'
                    tr += ' <input type="hidden" class="BalanceHidden" value="' + item.Balance + '" />'
                    tr += ' <input type="hidden" class="SettledAmount" value="' + 0 + '" />'
                    tr += ' <input type="hidden" class="Status" /></td>'
                    tr += ' <td class="DocumentType">' + item.DocumentType + '</td>'
                    tr += ' <td class="DocumentNo">' + item.DocumentNo + '</td>'
                    tr += ' <td class="InvoiceDate" >' + item.TransDateStr + '</td>'
                    tr += ' <td class="PendingDays" >' + item.PendingDays + '</td>'
                    tr += ' <td class="Amount mask-sales-currency" >' + item.ReceivableAmount + '</td>'
                    tr += ' <td class="Balance mask-qty" >' + item.Balance + '</td>';

                    if (DocumentType == "INVOICE" || DocumentType == "DEBITNOTE" || DocumentType == "CUSTOMERCHARGES" || DocumentType == "CUSTOMERRETURN") {
                        tr += ' <td><input type="text" class="md-input BalanceToBePaid mask-currency" disabled value="' + 0.00 + '"  /></td>'
                    }
                    else {
                        tr += ' <td><input type="text" class="md-input BalanceToBePaid mask-negative-currency " disabled value="' + 0.00 + '"  /></td>'
                    }
                    tr += ' <td><input type="text" class="md-input RemainingAmount mask-currency " disabled value="' + 0.00 + '"  /></td>'
                    tr += '</tr>';
                });

                var $tr = $(tr);
                app.format($tr);
                $invoice_items_list.append($tr);
                setTimeout(function () {
                    freeze_header.resizeHeader();
                });

                self.process_item();

            },
        });
    },

    count_items: function () {
        var count = $('#invoice-list tbody tr.included').length;
        $('#item-count').val(count);
    },

    validate_item: function () {
        var self = receipt;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_draft: function () {
        var self = receipt;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_draft);
            self.save(false);
        }
        return 0;
    },

    validate_receipt: function () {
        var self = receipt;
        if (self.rules.on_receipt.length > 0) {
            return form.validate(self.rules.on_receipt);
            self.save(false);
        }
        return 0;
    },

    validate_form: function () {
        var self = receipt;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
            self.save(false);
        }
        return 0;
    },

    rules: {
        on_add: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose customer" },
                    { type: form.positive, message: 'Please select customer' },
                ]
            },
            {
                elements: "#ReceiptAmount",
                rules: [
                    { type: form.positive, message: "Please enter valid receipt amount" },
                ]
            },
        ],
        on_draft: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please select customer" },
                    { type: form.non_zero, message: "Please select customer" },
                ]
            },

            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
        ],
        on_receipt: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please select customer" },
                    { type: form.non_zero, message: "Please select customer" },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: ".included .BalanceToBePaid ",
                rules: [
                    {
                        type: function (element) {
                            var count = $("#invoice-list tbody tr.included").length;
                            var balancecount = $("#invoice-list tbody tr.included .BalanceToBePaid").filter(function () {
                                return $(this).val() == 0.00
                            }).closest("tr").length;
                            return (balancecount < count)
                        }, message: "Invalid settlement amount"
                    },
                    {
                        type: function (element) {
                            var amount = clean($("#ReceiptAmount").val());
                            var balanceamount = 0;
                            var row;
                            $("#invoice-list tbody tr.included .BalanceToBePaid").each(function () {
                                row = $(this).closest('tr');
                                balanceamount += clean($(row).find(".BalanceToBePaid").val());
                                balanceamount = (balanceamount).roundTo(2);
                            });
                            return (amount == balanceamount)
                        }, message: "Invalid receipt amount"
                    },
                ]
            },
        ],
        on_submit: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please select customer" },
                    { type: form.non_zero, message: "Please select customer" },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: ".included .BalanceToBePaid ",
                rules: [
                    {
                        type: function (element) {
                            var count = $("#invoice-list tbody tr.included").length;
                            var balancecount = $("#invoice-list tbody tr.included .BalanceToBePaid").filter(function () {
                                return $(this).val() == 0.00
                            }).closest("tr").length;
                            return (balancecount < count)
                        }, message: "Invalid settlement amount"
                    },
                    {
                        type: function (element) {
                            var amount = clean($("#ReceiptAmount").val());
                            var balanceamount = 0;
                            var row;
                            $("#invoice-list tbody tr.included .BalanceToBePaid").each(function () {
                                row = $(this).closest('tr');
                                balanceamount += clean($(row).find(".BalanceToBePaid").val());
                                balanceamount = (balanceamount).roundTo(2);
                            });
                            return (amount == balanceamount)
                        }, message: "Invalid receipt amount"
                    },
                ]
            },
            {
                elements: "#PaymentTypeID",
                rules: [
                    { type: form.required, message: "Please choose a mode of payment" },
                    {
                        type: function (element) {
                            var error = false;
                            var paymenType = $("#PaymentTypeID option:selected").text().toUpperCase();
                            var isBlockedForChequeReceipt = $("#IsBlockedForChequeReceipt").val();
                            if (paymenType == "CHEQUE" && (isBlockedForChequeReceipt == "true" || isBlockedForChequeReceipt == "True")) {
                                error = true;
                            }
                            return !error

                        }, message: "Customer blocked for cheque receipt"
                    },
                ]
            },
            {
                elements: "#ChecqueDate",
                rules: [
                    {
                        type: function (element) {
                            var PaymentMode = $("#PaymentTypeID option:selected").text().toUpperCase();
                            if (PaymentMode === 'CHEQUE') {
                                return $(element).val().trim() !== "";
                            }
                            return true;
                        },
                        message: "Please select a valid Cheque Date"
                    }
                ]
            },
            {
                elements: "#BankID",
                rules: [
                    { type: form.required, message: "Please choose a bank" },
                ]
            },
            {
                elements: "#ReceiptAmount",
                rules: [
                    {
                        type: function (element) {
                            var sum = 0;
                            $('tr.included .BalanceToBePaid').each(function () {
                                sum += clean($(this).val());
                            });
                            return sum.roundTo(2) == clean($(element).val());
                        }, message: "Please process receipt"
                    },
                ]
            },
            {
                elements: "#Date",
                rules: [
                    { type: form.required, message: "Please enter date" },
                    {
                        type: function (element) {
                            return $("#PaymentTypeID option:selected").text().toUpperCase() == "CASH" ? form.current_date(element) : true;
                        }, message: "Instrument date must be current date"

                    },
                     {
                         type: function (element) {
                             var date = new Date();
                             var error = true;
                             var nextdate = date.setDate(date.getDate() + 1);
                             var instrumentDate = $(element).val().split('-');
                             var Date_datesplit = new Date(instrumentDate[2], instrumentDate[1] - 1, instrumentDate[0]);
                             var instrumentDate = Date.parse(Date_datesplit);
                             if ($("#PaymentTypeID option:selected").text().toUpperCase() == "CHEQUE" && (instrumentDate > nextdate)) {
                                 error = false;
                             }
                             return error
                         }, message: "Instrument date should not exceeds tomorrow date"
                     }
                ]
            },
        ],
    }
}