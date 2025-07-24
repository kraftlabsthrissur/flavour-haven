$(function () {
    Journal.init();
});
var fh_items;
Journal = {
    init: function () {
        var self = Journal;
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
            fh_items = $("#journal-item-list").FreezeHeader();
        }
        self.bind_events();
        self.get_debit_currency_details();
        self.get_credit_currency_details();
    },

    list: function () {
        var self = Journal;
        $('#tabs-journal').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-journal":
                $list = $('#saved-journal-list');
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

            var url = "/Accounts/Journal/GetJournalList?type=" + type;

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

                   { "data": "VoucherNo", "className": "VoucherNo" },
                   { "data": "Date", "className": "Date" },
                   { "data": "DebitAccountName", "className": "DebitAccountName" },
                    {
                        "data": "TotalDebitAmount", "searchable": false, "className": "TotalDebitAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.TotalDebitAmount + "</div>";
                        }
                    },
                   { "data": "CreditAccountName", "className": "CreditAccountName" },
                    {
                        "data": "TotalCreditAmount", "searchable": false, "className": "TotalCreditAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.TotalCreditAmount + "</div>";
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
                        app.load_content("/Accounts/Journal/Details/" + Id);

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
        var self = Journal;
        $.UIkit.autocomplete($('#creditaccountnumber-autocomplete'), { 'source': self.get_CreditAccount, 'minLength': 1 });
        $('#creditaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_CreditAccount);

        $.UIkit.autocomplete($('#debitaccountnumber-autocomplete'), { 'source': self.get_DebitAccount, 'minLength': 1 });
        $('#debitaccountnumber-autocomplete').on('selectitem.uk.autocomplete', self.set_DebitAccount);

        $.UIkit.autocomplete($('#creditaccountname-autocomplete'), { 'source': self.get_CreditAccount, 'minLength': 1 });
        $('#creditaccountname-autocomplete').on('selectitem.uk.autocomplete', self.set_CreditAccount);

        $.UIkit.autocomplete($('#debitaccountname-autocomplete'), { 'source': self.get_DebitAccount, 'minLength': 1 });
        $('#debitaccountname-autocomplete').on('selectitem.uk.autocomplete', self.set_DebitAccount);

        $("body").on("keydown", "#DebitAccountName", self.clear_debit_account_code);
        $("body").on("keydown", "#DebitAccountCode", self.clear_debit_account_name);

        $("body").on("keydown", "#CreditAccountName", self.clear_credit_account_code);
        $("body").on("keydown", "#CreditAccountCode", self.clear_credit_account_name);

        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);


        $("body").on('click', '.btnSave,.btnSaveAsDraft', self.save_jounral);
        //$(".btnSaveAndNew").on("click", self.save_and_new);
        $("#DebitCurrencyID").on('change', self.get_debit_currency_details);
        $("#CreditCurrencyID").on('change', self.get_credit_currency_details);
    },
    get_debit_currency_details: function () {
        var self = Journal;
        var DebitCurrencyID = $("#DebitCurrencyID").val();
        var LocalCurrencyID = $("#CurrencyID").val();
        if (DebitCurrencyID == null || DebitCurrencyID == "") {
            DebitCurrencyID = 0;
        }
        $.ajax({
            url: '/Masters/CurrencyConversion/GetDebitCurrencyDetails',
            data: {
                BaseCurrencyID: LocalCurrencyID,
                ConversionCurrencyID: DebitCurrencyID
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.data != null) {
                    $("#DebitExchangeRate").val(response.data.ExchangeRate);
                }
            }
        });
    },
    get_credit_currency_details: function () {
        var self = Journal;
        var CreditCurrencyID = $("#CreditCurrencyID").val();
        var LocalCurrencyID = $("#CurrencyID").val();
        if (CreditCurrencyID == null || CreditCurrencyID == "") {
            CreditCurrencyID = 0;
        }
        $.ajax({
            url: '/Masters/CurrencyConversion/GetCurrencyDetails',
            data: {
                BaseCurrencyID: LocalCurrencyID,
                ConversionCurrencyID: CreditCurrencyID
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.data != null) {
                    $("#CreditExchangeRate").val(response.data.ExchangeRate);
                }
            }
        });
    },
    is_month_closed: function () {
        var self = Journal;
        var classs = this.className;
        $.ajax({
            url: '/Masters/PeriodClosing/IsMonthClosed',
            data: {
                Type: 'JournalStatus',
                Date: $("#Date").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.data != "Open") {
                        app.show_error("This Month Is Closed");
                    }
                    else {
                        if (classs == "md-btn btnSaveAsDraft") {
                            self.save_as_draft();
                        }
                        else {
                            self.save_confirm();
                        }
                    }
                }
                else {
                    return;
                }
            },
        });
    },
    save_jounral: function () {
        var self = Journal;
        var classs = this.className;
        if (classs == "md-btn btnSaveAsDraft") {
            self.save_as_draft();
        }
        else {
            self.save_confirm();
        }
    },
    save_confirm: function () {
        var self = Journal
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save(false);
        }, function () {
        })
    },
    save_as_draft: function () {
        var self = Journal;
        self.error_count = 0;
        self.error_count = 0;
        self.error_count = self.validate_draft();
        if (self.error_count > 0) {
            return;
        }
        else {
            self.save(true);
        }



    },
    save: function (IsDraft) {
        var self = Journal;
        var url = '/Accounts/Journal/Save';
        var model = self.get_data();
        model.IsDraft = IsDraft;
        if (IsDraft) {
            url = "/Accounts/Journal/SaveAsDraft";
        }

        $(".btnSaveAndNew, .btnSave, .btnSaveAsDraft ").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Journal created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/Journal/Index";
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $("btnSaveAndNew, .btnSave, .btnSaveAsDraft ").css({ 'display': 'block' });
                }
            },
        });
    },

    clear_debit_account_code: function (e) {
        if (e.keyCode != 13) {
            $("#DebitAccountHeadID").val("");
            $("#DebitAccountCode").val("");
            $("#DebitAmount").val("");
        }
    },
    clear_debit_account_name: function (e) {
        if (e.keyCode != 13) {
            $("#DebitAccountHeadID").val("");
            $("#DebitAccountName").val("");
            $("#DebitAmount").val("");
        }
    },
    clear_credit_account_code: function (e) {
        if (e.keyCode != 13) {
            $("#CreditAccountHeadID").val("");
            $("#CreditAccountCode").val("");
            $("#CreditAmount").val("");
        }
    },
    clear_credit_account_name: function (e) {
        if (e.keyCode != 13) {
            $("#CreditAccountHeadID").val("");
            $("#CreditAccountName").val("");
            $("#CreditAmount").val("");

        }
    },
    get_CreditAccount: function (release) {
        $.ajax({
            url: '/Accounts/Journal/GetCreditAccountAutoComplete',
            data: {
                CreditAccountName: $('#CreditAccountName').val(),
                CreditAccountCode: $('#CreditAccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_CreditAccount: function (event, item) {
        $("#CreditAccountHeadID").val(item.id);
        $("#CreditAccountCode").val(item.number);
        $("#CreditAccountName").val(item.name);
        $("#CreditAccountCode, #CreditAccountName").removeClass('md-input-danger');
        $("#CreditAccountCode, #CreditAccountName").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#CreditAmount').focus();
    },
    get_DebitAccount: function (release) {
        $.ajax({
            url: '/Accounts/Journal/GetDebitAccountAutoComplete',
            data: {
                DebitAccountName: $('#DebitAccountName').val(),
                DebitAccountCode: $('#DebitAccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_DebitAccount: function (event, item) {
        $("#DebitAccountHeadID").val(item.id);
        $("#DebitAccountCode").val(item.number);
        $("#DebitAccountName").val(item.name);
        $("#DebitAccountCode, #DebitAccountName").removeClass('md-input-danger');
        $("#DebitAccountCode, #DebitAccountName").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $('#DebitAmount').focus();
    },
    add_item: function () {
        var self = Journal;
        var item = {};
        var CreditHeadID = ($("#CreditAccountHeadID").val());
        var DebitHeadID = ($("#DebitAccountHeadID").val());
        if (CreditHeadID == '' || CreditHeadID == 0) {
            ($('#CreditAccountName').val(''));
            ($("#CreditAmount").val(''));
        }
        if (DebitHeadID == '' || DebitHeadID == 0) {
            ($('#DebitAccountName').val(''));
            ($("#DebitAmount").val(''));
        }
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }

        item.CreditAccountHeadID = ($("#CreditAccountHeadID").val());
        item.CreditAccountCode = ($("#CreditAccountCode").val());
        item.CreditAccountName = ($('#CreditAccountName').val());
        item.CreditAmount = clean($("#CreditAmount").val());
        item.DebitAccountHeadID = ($("#DebitAccountHeadID").val());
        item.DebitAccountCode = ($("#DebitAccountCode").val());
        item.DebitAccountName = ($('#DebitAccountName').val())
        item.DebitAmount = clean($("#DebitAmount").val());
        item.LocationID = ($("#LocationID").val());
        item.Location = ($("#LocationID :selected").text());
        item.DepartmentID = ($("#DepartmentID").val());
        item.Department = ($("#DepartmentID :selected").text());
        item.EmployeeID = ($("#EmployeeID").val());
        item.Employee = $("#EmployeeID").val() == "" ? "" : $("#EmployeeID  option:selected").text();
        item.InterCompanyID = ($("#InterCompanyID").val());
        item.InterCompany = $("#InterCompanyID").val() == "" ? "" : $("#InterCompanyID  option:selected").text();
        item.ProjectID = ($("#ProjectID").val());
        item.Project = $("#ProjectID").val() == "" ? "" : $("#ProjectID  option:selected").text();
        item.Remarks = ($("#Remarks").val());
        item.DebitCurrencyID = $("#DebitCurrencyID").val();
        item.DebitCurrency = $("#DebitCurrencyID option:selected").text();
        item.CreditCurrencyID = $("#CreditCurrencyID").val();
        item.CreditCurrency = $("#CreditCurrencyID option:selected").text();
        item.DebitExchangeRate = clean($("#DebitExchangeRate").val());
        item.CreditExchangeRate = clean($("#CreditExchangeRate").val());
        item.LocalDebitAmount = item.DebitAmount * item.DebitExchangeRate
        item.LocalCreditAmount = item.CreditAmount * item.CreditExchangeRate

        self.add_item_to_grid(item);
        self.clear_item();
        self.calculate_grid_total();

        $("#CreditAccountCode,#CreditAmount").removeClass('md-input-danger');
        $("#CreditAccountCode,#CreditAmount").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
        $("#DebitAccountCode,#DebitAmount").removeClass('md-input-danger');
        $("#DebitAccountCode,#DebitAmount").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
    },
    validate_item: function () {
        var self = Journal;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },
    validate_form: function () {
        var self = Journal;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = Journal;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_add_item: [

                {
                    elements: "#CreditAccountCode",
                    rules: [
                        {
                            type: function (element) {
                                var error = false;
                                var CreditAccountCode = $('#CreditAccountCode').val();
                                var DebitAccountCode = $('#DebitAccountCode').val();
                                return CreditAccountCode != "" || DebitAccountCode != ""

                            }, message: "Please Select Debit Account Number Or Credit Account Number"
                        }

                    ]
                },
                {
                    elements: "#DebitAccountCode",
                    rules: [
                        //{ type: form.required, message: "Please Select Credit Account Number" },
                        {
                            type: function (element) {
                                var error = false;
                                var CreditAccountCode = $('#CreditAccountCode').val();
                                var DebitAccountCode = $('#DebitAccountCode').val();
                                if (CreditAccountCode != "" || DebitAccountCode != "") {
                                    return true;
                                }
                            }, message: "Please Select Debit Account Number Or Credit Account Number"
                        }

                    ]
                },
                {
                    elements: "#CreditAccountHeadID",
                    rule: [
                        {
                            type: function (element) {
                                return clean($("#DebitAccountHeadID").val()) == 0 ? form.non_zero(element) && form.required(element) : true;
                            }, message: "Debit Account or Credit Account is required"
                        },
                        {
                            type: function (element) {
                                return clean($("#DebitAccountHeadID").val()) == clean($(element).val()) ? false : true;
                            }, message: "Debit Account and credit acoount should not be equal"
                        }
                    ]
                },
                {
                    elements: "#DebitAccountHeadID",
                    rules: [

                        {
                            type: function (element) {
                                return clean($("#CreditAccountHeadID").val()) == 0 ? form.non_zero(element) && form.required(element) : true;
                            }, message: "Debit Account or Credit Account is required"
                        },
                         {
                             type: function (element) {
                                 return clean($("#CreditAccountHeadID").val()) == clean($(element).val()) ? false : true;
                             }, message: "Debit and credit accounts are equal"
                         },
                        {
                            type: function (element) {
                                var row, index, error = false;
                                var CreditAccountHeadID = ($('#CreditAccountHeadID').val());
                                var DebitAccountHeadID = ($('#DebitAccountHeadID').val());


                                index = $("#journal-item-list tbody tr").length;
                                if (index > 0) {
                                    $("#journal-item-list tbody tr").each(function () {
                                        row = $(this);
                                        var CreditID = clean($(row).find('.CreditAccountHeadID').val());
                                        var DebitID = clean(($(row).find('.DebitAccountHeadID').val()));
                                        if (DebitID != "" || DebitID != 0) {
                                            if (CreditAccountHeadID == DebitID) {
                                                error = true;
                                            }
                                        }
                                        if (CreditID != "" || CreditID != 0) {
                                            if (DebitAccountHeadID == CreditID) {
                                                error = true;
                                            }
                                        }

                                    })
                                } return !error;
                            }, message: 'Please select a different Account Code/Account Name'
                        }

                    ]
                },
                {
                    elements: "#CreditAmount",
                    rules: [
                        {
                            type: function (element) {
                                var error = false;
                                var CreditAccountCode = $('#CreditAccountCode').val();
                                var DebitAccountCode = $('#DebitAccountCode').val();
                                var CreditAmount = clean($("#CreditAmount").val());
                                var DebitAmount = clean($("#DebitAmount").val());
                                if (CreditAccountCode != "") {
                                    if (CreditAmount > 0.0) {
                                        return true;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                                if (DebitAccountCode != "") {
                                    if (DebitAmount > 0.0) {
                                        return true;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                            }, message: "Please Enter Valid Credit/Debit Amount"
                        },
                    ]
                },
                {
                    elements: "#DebitAmount",
                    rules: [
                        {
                            type: function (element) {
                                var error = false;
                                var CreditAccountCode = $('#CreditAccountCode').val();
                                var DebitAccountCode = $('#DebitAccountCode').val();
                                var CreditAmount = clean($("#CreditAmount").val());
                                var DebitAmount = clean($("#DebitAmount").val());
                                if (CreditAccountCode != "") {
                                    if (CreditAmount > 0.0) {
                                        return true;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                                if (DebitAccountCode != "") {
                                    if (DebitAmount > 0.0) {
                                        return true;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                            }, message: "Please Enter Valid Credit/Debit Amount"
                        },
                    ]
                },
                {
                    elements: "#LocationID",
                    rules: [
                        { type: form.required, message: "Please Select Location" },
                    ]
                },
                {
                    elements: "#DepartmentID",
                    rules: [
                        { type: form.required, message: "Please Select Department" },
                    ]
                },
        ],
        on_submit: [
        {
            elements: "#item-count",
            rules: [
            { type: form.required, message: "Please add atleast one item" },
            {type: form.non_zero, message: "Please add atleast one item"},
            ]
        },

         {
             elements: ".DebitAccountHeads",
             rules: [
               {
                   type: function (element) {
                       var debit_count = $("#journal-item-list tbody tr.DebitAccountHeads").length;
                       var credit_count = $("#journal-item-list tbody tr.CreditAccountHeads").length;
                       var error = false;
                       if (debit_count > 1 && credit_count > 1)
                       {
                           error = true;
                       }
                       return !error;
                   }, message: "Invalid Journal Entry"
               },
             ]
         },

            {
                elements: "#TotalCreditAmount",
                rules: [
                    { type: form.required, message: "Invalid Credit Amount" },
                    { type: form.non_zero, message: "Invalid Credit Amount" },
                    { type: form.positive, message: "Invalid Credit Amount" },
                          {
                              type: function (element) {
                                  var error = false;
                                  var TotalCreditAmount = $('#TotalCreditAmount').val();
                                  var TotalDebitAmount = $('#TotalDebitAmount').val();
                                  if (TotalCreditAmount != TotalDebitAmount)
                                      error = true;
                                  return !error;
                              }, message: 'Credit & Debit Amount Must Be Equal'
                          },
                ]
            },
            {
                elements: "#TotalDebitAmount",
                rules: [
                    { type: form.required, message: "Invalid Debit Amount" },
                    { type: form.non_zero, message: "Invalid Debit Amount" },
                    { type: form.positive, message: "Invalid Debit Amount" },
                     {
                         type: function (element) {
                             var error = false;
                             var TotalCreditAmount = $('#TotalCreditAmount').val();
                             var TotalDebitAmount = $('#TotalDebitAmount').val();
                             if (TotalCreditAmount != TotalDebitAmount)
                                 error = true;
                             return !error;
                         },
                     },
                ]
            },
        ],
        on_draft: [
       {
           elements: "#item-count",
           rules: [
           { type: form.required, message: "Please add atleast one item" },
           {type: form.non_zero, message: "Please add atleast one item"},
           ]
       },
        ],
    },
    add_item_to_grid: function (item) {
        var self = Journal;
        var row_class_name = "";
        if ($('#DebitAccountHeadID').val() > 0 && $('#CreditAccountHeadID').val() > 0)
        {
            row_class_name = "DebitAccountHeads CreditAccountHeads";
        }
        else if ($('#DebitAccountHeadID').val() > 0)
        {
            row_class_name = "DebitAccountHeads";
        }
        else if ($('#CreditAccountHeadID').val() > 0) {
            row_class_name = "CreditAccountHeads";
        }
        else {
            row_class_name = "";
        }

        item.DebitCurrencyID = $("#DebitCurrencyID").val();
        item.DebitCurrency = $("#DebitCurrencyID option:selected").text();
        item.CreditCurrencyID = $("#CreditCurrencyID").val();
        item.CreditCurrency = $("#CreditCurrencyID option:selected").text();
        item.DebitExchangeRate = clean($("#DebitExchangeRate").val());
        item.CreditExchangeRate = clean($("#CreditExchangeRate").val());
        item.LocalDebitAmount = item.DebitAmount * item.DebitExchangeRate
        item.LocalCreditAmount = item.CreditAmount * item.DebitExchangeRate

        var index, tr;
        index = $("#journal-item-list tbody tr").length + 1;
        tr = '<tr class="'+row_class_name+'">'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td class="DebitAccountCode">' + item.DebitAccountCode
                + ' <input type="hidden" class="DebitAccountHeadID" value="' + $('#DebitAccountHeadID').val() + '" /></td>'
                + ' <td class="DebitAccountName">' + item.DebitAccountName
                + ' <td class="CreditAccountCode">' + item.CreditAccountCode
                + ' <input type="hidden" class="CreditAccountHeadID" value="' + $('#CreditAccountHeadID').val() + '" /></td>'
                + ' <td class="CreditAccountName">' + item.CreditAccountName
                + ' <td class="DebitCurrency">' + item.DebitCurrency
                + ' <input type="hidden" class="DebitCurrencyID" value="' + item.DebitCurrencyID + '" /></td>'
                + ' <td class="CreditCurrency">' + item.CreditCurrency
                + ' <input type="hidden" class="CreditCurrencyID" value="' + item.CreditCurrencyID + '" />'
                + ' <input type="hidden" class="DebitExchangeRate" value="' + item.DebitExchangeRate + '" />'
                + ' <input type="hidden" class="CreditExchangeRate" value="' + item.CreditExchangeRate + '" /></td > '
                + ' <td class="DebitAmount mask-currency">' + item.DebitAmount + '</td>'
                + ' <td class="LocalDebitAmount mask-currency">' + item.LocalDebitAmount + '</td>'
                + ' <td class="CreditAmount mask-currency">' + item.CreditAmount + '</td>'
                + ' <td class="LocalCreditAmount mask-currency">' + item.LocalCreditAmount + '</td>'
                + ' <td class="Location">' + item.Location
                + ' <input type="hidden" class="LocationID" value="' + $('#LocationID').val() + '" /></td>'
                + ' <td class="Department">' + item.Department
                + ' <input type="hidden" class="DepartmentID" value="' + $('#DepartmentID').val() + '" /></td>'
                + ' <td class="InterCompany">' + item.InterCompany
                + ' <input type="hidden" class="InterCompanyID" value="' + $('#InterCompanyID').val() + '" /></td>'
                + ' <td class="Employee">' + item.Employee
                + ' <input type="hidden" class="EmployeeID" value="' + $('#EmployeeID').val() + '" /></td>'
                + ' <td class="Project">' + item.Project
                + ' <input type="hidden" class="ProjectID" value="' + $('#ProjectID').val() + '" /></td>'
                + ' <td class="Remarks">' + item.Remarks + '</td>'
                + ' <td class="uk-text-center">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';

        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#journal-item-list tbody").append($tr);
        self.clear_item();
        fh_items.resizeHeader();
    },
    clear_item: function () {
        var self = Journal;
        $("#DebitAccountHeadID").val('');
        $("#CreditAccountHeadID").val('');
        $("#CreditAccountCode").val('');
        $("#CreditAccountName").val('');
        $("#CreditAmount").val('');
        $("#DebitAccountCode").val('');
        $("#DebitAccountName").val('');
        $("#DebitAmount").val('');
        //$("#LocationID").val('');
        //$("#DepartmentID").val('');
        //$("#EmployeeID").val('');
        //$("#InterCompanyID").val('');
        //$("#ProjectID").val('');
        $("#Remarks").val('');
    },
    calculate_grid_total: function () {
        var NetCreditAmount = 0;
        var NetDebitAmount = 0;

        $("#journal-item-list tbody tr").each(function () {

            //NetCreditAmount += clean($(this).find('.CreditAmount').val());
            //NetDebitAmount += clean($(this).find('.DebitAmount').val());
            NetCreditAmount += clean($(this).find('.LocalCreditAmount').val());
            NetDebitAmount += clean($(this).find('.LocalDebitAmount').val());
        });
        $("#TotalCreditAmount").val(NetCreditAmount);
        $("#TotalDebitAmount").val(NetDebitAmount);
    },
    remove_item: function () {
        var self = Journal;
        $(this).closest("tr").remove();
        $("#journal-item-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#journal-item-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader();
    },

   
    get_data: function () {
        var self = Journal;

        var model = {
            ID: $("#ID").val(),
            VoucherNo: $("#VoucherNo").val(),
            Date: $("#Date").val(),
            TotalCreditAmount: clean($("#TotalCreditAmount").val()),
            TotalDebitAmount: clean($("#TotalDebitAmount").val()),
            CurrencyID: $("#CurrencyID option:selected").val(),
            Currency: $("#CurrencyID option:selected").text(),
            //IsDraft: IsDraft
        };
        model.Items = self.GetProductList();

        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var row;
        $("#journal-item-list tbody tr").each(function () {
            row = $(this);
            var CreditAccountHeadID = $(row).find('.CreditAccountHeadID').val();
            var CreditAccountCode = clean($(row).find('.CreditAccountCode').text());
            var CreditAmount = clean($(row).find('.CreditAmount').val());
            var DebitAccountHeadID = ($(row).find('.DebitAccountHeadID').val());
            var DebitAccountCode = $(row).find('.DebitAccountCode').text();
            var DebitAmount = clean($(row).find('.DebitAmount').val());
            var JournalLocationID = $(row).find('.LocationID').val();
            var DepartmentID = $(row).find('.DepartmentID').val();
            var EmployeeID = $(row).find('.EmployeeID').val();
            var InterCompanyID = $(row).find('.InterCompanyID').val();
            var ProjectID = $(row).find('.ProjectID').val();
            var Remarks = $(row).find('.Remarks').text();
            var DebitCurrencyID = $(row).find('.DebitCurrencyID').val();
            var DebitCurrency = $(row).find('.DebitCurrency').text();
            var CreditCurrencyID = $(row).find('.CreditCurrencyID').val();
            var CreditCurrency = $(row).find('.CreditCurrency').text();
            var DebitExchangeRate = clean($(row).find('.DebitExchangeRate').val());
            var CreditExchangeRate = clean($(row).find('.CreditExchangeRate').val());
            var LocalDebitAmount = clean($(row).find('.LocalDebitAmount').val());
            var LocalCreditAmount = clean($(row).find('.LocalCreditAmount').val());

            ProductsArray.push({
                CreditAccountHeadID: CreditAccountHeadID,
                CreditAccountCode: CreditAccountCode,
                CreditAmount: CreditAmount,
                DebitAccountHeadID: DebitAccountHeadID,
                DebitAccountCode: DebitAccountCode,
                DebitAmount: DebitAmount,
                JournalLocationID: JournalLocationID,
                DepartmentID: DepartmentID,
                EmployeeID: EmployeeID,
                InterCompanyID: InterCompanyID,
                ProjectID: ProjectID,
                Remarks: Remarks,
                DebitCurrencyID: DebitCurrencyID,
                DebitCurrency: DebitCurrency,
                CreditCurrencyID: CreditCurrencyID,
                CreditCurrency: CreditCurrency,
                DebitExchangeRate: DebitExchangeRate,
                CreditExchangeRate: CreditExchangeRate,
                LocalDebitAmount: LocalDebitAmount,
                LocalCreditAmount: LocalCreditAmount
            });
        })
        return ProductsArray;
    },
}

