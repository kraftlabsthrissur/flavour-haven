$(function () {
    BankExpenses.init();
});
var fh_items;
BankExpenses = {
    init: function () {
        var self = BankExpenses;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.list();
        } else {
            fh_items = $("#bank-expenses-item-list").FreezeHeader();
        }
        self.bind_events();
    },

    list: function () {
        var self = BankExpenses;
        $('#tabs-financial-expenses').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-financial-expenses":
                $list = $('#saved-bank-expences-list');
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

            var url = "/Accounts/BankExpenses/GetFinancialExpensesList?type=" + type;

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

                       }
                   },

                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "Date", "className": "Date" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "ModeOfPayment", "className": "ModeOfPayment" },
                   {
                       "data": "TotalAmount", "searchable": false, "className": "TotalAmount",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.TotalAmount + "</div>";
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
                        app.load_content("/Accounts/BankExpenses/Details/" + Id);

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
        self = BankExpenses;
        $.UIkit.autocomplete($('#accountnumber-autocomplete'), { 'source': self.get_Account, 'minLength': 1 });
        $('#accountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_Account);
        $.UIkit.autocomplete($('#accountname-autocomplete'), { 'source': self.get_Account, 'minLength': 1 });
        $('#accountname-autocomplete').on('selectitem.uk.autocomplete', self.set_Account);
        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);
        //$('body').on('change','#BankID', self.clear_grid);
        $(".btnSave, .btnSaveAndNew, .btnSaveAsDraft").on("click", self.on_save);
    },

    details: function () {
        var self = BankExpenses;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = BankExpenses;
        $.ajax({
            url: '/Reports/Accounts/BankExpensesPrintPdf',
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

    add_item: function () {
        var self = BankExpenses;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }

        var item = {};
        item.BankID = $("#BankID").val();
        item.Bank = ($("#BankID :selected").text());
        item.TransactionNumber = $("#TransactionNumber").val();
        item.TransactionDate = $("#TransactionDate").val();
        item.ItemID = ($("#ItemID").val());
        item.ItemName = ($("#ItemID :selected").text());
        item.ModeOfPaymentID = ($("#ModeOfPaymentID").val());
        item.ModeOfPayment = ($("#ModeOfPaymentID :selected").text());
        item.Amount = clean($("#Amount").val());
        item.Remarks = ($("#Remarks").val());
        item.ReferenceNo = $("#ReferenceNo").val();
        self.add_item_to_grid(item);
        self.clear_item();
        self.calculate_grid_total();


    },
    clear_item: function () {
        var self = BankExpenses;
        var currentdate = $("#TransactionDate").val();
        $("#TransactionNumber").val('');
        $("#TransactionDate").val(currentdate);
        $("#AccountCode").val('');
        $("#AccountName").val('');
        $("#ModeOfPaymentID").val('');
        $("#Amount").val('');
        ($("#ItemID").val(''));
        ($("#ItemName").val(''));
        $("#InstrumentDate").val('');
        $("#Remarks").val('');
    },
    validate_item: function () {
        var self = BankExpenses;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    validate_draft: function () {
        var self = BankExpenses;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_add_item: [

                {
                    elements: "#BankID",
                    rules: [
                        { type: form.required, message: "Please Select Bank " },
                    ]
                },

                 {
                     elements: "#Amount",
                     rules: [
                         { type: form.required, message: "Invalid Amount " },
                         { type: form.non_zero, message: "Invalid Amount " },
                         { type: form.positive, message: "Invalid Amount " },
                     ]
                 },
                 {
                     elements: "#ModeOfPaymentID",
                     rules: [
                         { type: form.required, message: "Please Select Payment Mode" },
                     ]
                 },
                 {
                     elements: "#ItemID",
                     rules: [
                         { type: form.required, message: "Please select an Item" },
                     ]
                 },
                  {
                      elements: "#AccountHeadID",
                      rules: [
                           { type: form.non_zero, message: "Please Select a  valid Item" },
                          { type: form.required, message: "Please Select a  valid Item" },
                      ]
                  },
                   {
                       elements: "#AccountCode",
                       rules: [
                           { type: form.required, message: "Please Select AccountCode" },
                       ]
                   },
                  {
                      elements: "#AccountName",
                      rules: [
                          { type: form.required, message: "Please Select AccountName" },
                      ]
                  },
                

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
                elements: "#TotalAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
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
                elements: "#TotalAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                ]
            },
        ],
        on_draft: [
           {
               elements: "#item-count",
               rules: [
                   { type: form.required, message: "Please add atleast one item" },
                   { type: form.non_zero, message: "Please add atleast one item" },
               ]
           },
           {
               elements: "#TotalAmount",
               rules: [
                   { type: form.required, message: "Invalid Net Amount" },
                   { type: form.non_zero, message: "Invalid Net Amount" },
                   { type: form.positive, message: "Invalid Net Amount" },
               ]
           },
        ],
    },
    add_item_to_grid: function (item) {
        var self = BankExpenses;
        var index, tr;
        index = $("#bank-expenses-item-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td class="TransactionNumber">' + item.TransactionNumber + '</td>'
                + ' <td class="TransactionDate">' + item.TransactionDate + '</td>'
                + ' <td class="ItemName">' + item.ItemName
                + ' <input type="hidden" class="ItemID" value="' + $('#ItemID').val() + '" /></td>'
                + ' <td class="ModeOfPayment">' + item.ModeOfPayment
                + ' <input type="hidden" class="ModeOfPaymentID" value="' + $('#ModeOfPaymentID').val() + '" /></td>'
                + ' <td class="Amount mask-currency">' + item.Amount + '</td>'
                + ' <td class="Remarks">' + item.Remarks + '</td>'
                + ' <td class="ReferenceNo">' + item.ReferenceNo + '</td>'
                + ' <td class="uk-text-center">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#bank-expenses-item-list tbody").append($tr);
        fh_items.resizeHeader();

        $("#TransactionDate").removeClass('md-input-danger');
        $("#TransactionDate").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
    },
    calculate_grid_total: function () {
        var NetAmount = 0;

        $("#bank-expenses-item-list  tbody tr").each(function () {

            NetAmount += clean($(this).find('.Amount').val());
        });
        $("#TotalAmount").val(NetAmount);
    },
    remove_item: function () {
        var self = BankExpenses;
        $(this).closest("tr").remove();
        $("#bank-expenses-item-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#bank-expenses-item-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader();
    },

    on_save: function () {

        var self = BankExpenses;
        var data = self.get_data();
        var location = "/Accounts/BankExpenses/Index";
        var url = '/Accounts/BankExpenses/Save';

        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Accounts/BankExpenses/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveAndNew")) {
                location = "/Accounts/BankExpenses/Create";
            }
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

        var self = BankExpenses;

        $(".btnSaveAndNew, .btnSaveAsDraft, .btnSave ").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: data },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Bank Expenses created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveAndNew, .btnSaveAsDraft, .btnSave  ").css({ 'display': 'block' });
                }
            },
        });
    },
    validate_form: function () {
        var self = BankExpenses;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    save_as_draft: function () {
        var self = BankExpenses;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(true);
        }
    },
    submit: function () {
        var self = BankExpenses;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(false);
        }
    },
    get_data: function (IsDraft) {
        var self = BankExpenses;
        var model = {
            ID: $("#ID").val(),
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            BankID: $('#BankID').val(),
            TotalAmount: clean($("#TotalAmount").val()),
            IsDraft: IsDraft,

        };
        model.Items = self.get_productlist();
        return model;
    },
    get_productlist: function () {
        var ProductsArray = [];
        var row;
        $("#bank-expenses-item-list tbody tr").each(function () {
            row = $(this);
            var TransactionNumber = $(row).find('.TransactionNumber').text();
            var TransactionDate = $(row).find('.TransactionDate').text();
            var AccountHeadID = $(row).find('.AccountHeadID').val();
            var ModeOfPaymentID = $(row).find('.ModeOfPaymentID').val();
            var Amount = clean($(row).find('.Amount').val());
            var ItemID = $(row).find('.ItemID').val();
            var Remarks = $(row).find('.Remarks').text();
            var ReferenceNo = $(row).find('.ReferenceNo').text();
            ProductsArray.push({
                TransactionNumber: TransactionNumber,
                TransactionDate: TransactionDate,
                AccountHeadID: AccountHeadID,
                ModeOfPaymentID: ModeOfPaymentID,
                Amount: Amount,
                ItemID: ItemID,
                Remarks: Remarks,
                ReferenceNo: ReferenceNo
            });
        })
        return ProductsArray;
    },
    get_Account: function (release) {
        $.ajax({
            url: '/Accounts/BankExpenses/GetAccountAutoComplete',
            data: {
                AccountName: $('#AccountName').val(),
                AccountCode: $('#AccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_Account: function (event, item) {
        $("#AccountHeadID").val(item.id);
        $("#AccountCode").val(item.number);
        $("#AccountName").val(item.name);
        $("#AccountCode, #AccountName").removeClass('md-input-danger');
        $("#AccountCode, #AccountName").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#ModeOfPayment').focus();
    },

    save_and_new: function () {
        var self = BankExpenses;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var model = self.get_data();
        $(".btnSaveAndNew, .btnSave , .btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: '/Accounts/BankExpenses/Save',
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Bank Expense created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/BankExpenses/Create";
                    }, 1000);
                } else {
                    app.show_error("Failed to create Bank Expense");
                    $(".btnSaveAndNew,.btnSaveAsDraft , .btnSave").css({ 'display': 'block' });
                }
            },
        });
    },
    clear_grid: function () {
        var self = BankExpenses;
        var ID = $("#BankID").val();
        var Name = $('#BankID option:selected').text();
        if (($("#bank-expenses-item-list tbody tr").length > 0) && (ID > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#bank-expenses-item-list tbody').empty();
                $("#Bank").val(Name);
                $("#BankID").val(ID);
                UIkit.modal($('#select-customer')).hide();
                self.calculate_grid_total();
            })
        }
        else {
            $("#BankID").val(ID);
            $("#Bank").val(Name);
            UIkit.modal($('#select-customer')).hide();
            self.calculate_grid_total();
        }

    }
}


