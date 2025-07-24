CreditNote = {
    init: function () {
        var self = CreditNote;
        AccountHead.debit_account_list();
        AccountHead.credit_account_list();
        $('#debit-account-list').SelectTable({
            selectFunction: self.select_debit_acount,
            modal: "#select-debit-account",
            initiatingElement: "#DebitAccount"
        });

        $('#credit-account-list').SelectTable({
            selectFunction: self.select_credit_acount,
            modal: "#select-credit-account",
            initiatingElement: "#CreditAccount"
        });
        self.bind_events();
    },

    bind_events: function () {
        var self = CreditNote;
        $.UIkit.autocomplete($('#debit-account-autocomplete'), Config.get_credit_account_list);
        $('#debit-account-autocomplete').on('selectitem.uk.autocomplete', self.set_debit_account);
        $("#btn-ok-debit-account").on('click', self.select_debit_acount);

        $.UIkit.autocomplete($('#credit-account-autocomplete'), Config.get_credit_account_list);
        $('#credit-account-autocomplete').on('selectitem.uk.autocomplete', self.set_credit_account);
        $("#btn-ok-credit-account").on('click', self.select_credit_acount);
        $(".btnSave, .btnSavedraft, .btnSaveNew").on("click", self.on_save);
        $("#Amount").on("keyup change", self.update_amount);
        $('body').on('change', "#GSTID", self.update_amount);
        $('body').on('change', "#GSTID", self.set_gst_inclusive);
        $('body').on('change', "#GSTCategoryID", self.update_amount);
    },

    select_debit_acount: function () {
        var self = CreditNote;
        var radio = $('#select-debit-account tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".AccountName").text().trim();
        $("#DebitAccount").val(Name);
        $("#DebitAccountID").val(ID);
        UIkit.modal($('#select-debit-account')).hide();
    },

    select_credit_acount: function () {
        var self = CreditNote;
        var radio = $('#select-credit-account tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".AccountName").text().trim();
        $("#CreditAccount").val(Name);
        $("#CreditAccountID").val(ID);
        UIkit.modal($('#select-credit-account')).hide();
    },

    set_credit_account: function (event, item) {
        var self = CreditNote;
        $("#CreditAccount").val(item.value);
        $("#CreditAccountID").val(item.id);
    },

    set_debit_account: function (event, item) {
        var self = CreditNote;
        $("#DebitAccount").val(item.value);
        $("#DebitAccountID").val(item.id);
    },

    on_save: function () {
        var self = CreditNote;
        var data = self.get_data();
        var location = "/Accounts/CreditNote/Index";
        var url = '/Accounts/CreditNote/Save';

        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
            url = '/Accounts/CreditNote/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            if ($(this).hasClass("btnSaveNew")) {
                location = "/Accounts/CreditNote/Create";
            }
            self.error_count = self.validate_form();
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.Save(data, url, location);
            }, function () {
            })
        } else {
            self.Save(data, url, location);
        }
    },

    save_confirm: function () {
        var self = CreditNote
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    Save: function (data, url, location) {
        var self = CreditNote;
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("CreditNote created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = CreditNote;
        var data = {};
        data.ID = $("#ID").val();
        data.CreditAccountID = $("#CreditAccountID").val();
        data.CreditAccount = $("#CreditAccount").val();
        data.DebitAccount = $("#DebitAccount").val();
        data.DebitAccountID = $("#DebitAccountID").val();
        data.Amount = clean($("#Amount").val());
        data.Remarks = $("#Remarks").val();
        data.TransNo = $("#TransNo").val();
        data.TransDate = $("#TransDate").val();
        data.IsInclusive = $("#IsInclusive").val();
        data.GSTCategoryID = $("#GSTCategoryID  option:selected").val();
        data.TaxableAmount = clean($("#TaxableAmount").val());
        data.GSTAmount = clean($("#GSTAmount").val());
        data.TotalAmount = clean($("#TotalAmount").val());
        return data;
    },

    list: function () {
        var self = CreditNote;
        $('#tabs-CreditNote').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "creditnote":
                $list = $('#credit-note-list');
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

            var url = "/Accounts/CreditNote/GetCreditNoteList?type=" + type;
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
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "DebitAccount", "className": "DebitAccount" },
                   { "data": "CreditAccount", "className": "CreditAccount" },
                   { "data": "Amount", "className": "Amount" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/CreditNote/Detail/" + Id);

                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    validate_form: function () {
        var self = CreditNote;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
               {
                   elements: "#DebitAccountID",
                   rules: [
                       { type: form.required, message: "Please choose an DebitAccount", alt_element: "#DebitAccount" },
                       { type: form.positive, message: "Please choose an DebitAccount", alt_element: "#DebitAccount" },
                       { type: form.non_zero, message: "Please choose an DebitAccount", alt_element: "#DebitAccount" }

                   ],
               },
               {
                   elements: "#CreditAccountID",
                   rules: [
                       { type: form.required, message: "Please choose an CreditAccount", alt_element: "#CreditAccount" },
                       { type: form.positive, message: "Please choose an CreditAccount", alt_element: "#CreditAccount" },
                       { type: form.non_zero, message: "Please choose an CreditAccount", alt_element: "#CreditAccount" }

                   ],
               },
               {
                   elements: "#Amount",
                   rules: [
                       { type: form.required, message: "Please enter Amount" },
                       { type: form.positive, message: "Please enter Amount" },
                       { type: form.non_zero, message: "Please enter Amount" }

                   ],
               },
               {
                   elements: "#GSTID",
                   rules: [
                       { type: form.required, message: "Please select GST" },

                   ],
               },
               {
                   elements: "#GSTCategoryID",
                   rules: [
                       { type: form.required, message: "Please select GST Percentage" },

                   ],
               },
        ]
    },
    update_amount: function () {
        var self = CreditNote;
        var taxable_amount = 0;
        var gst_amount = 0;
        var net_amount = 0;
        var gst = $("#GSTID  option:selected").val();
        var percent = clean($("#GSTCategoryID  option:selected").text());
        var amount = clean($("#Amount").val());
        if (gst == 1) {
            taxable_amount = (amount) * (100 / (100 + percent));
            gst_amount = amount - taxable_amount;
            net_amount = amount

            $("#TaxableAmount").val(taxable_amount);
            $("#GSTAmount").val(gst_amount);
            $("#TotalAmount").val(net_amount);
        }
        else {

            gst_amount = (amount) * (percent / (100));
            net_amount = amount + gst_amount;

            $("#TaxableAmount").val(amount);
            $("#GSTAmount").val(gst_amount);
            $("#TotalAmount").val(net_amount);
        }
    },
    set_gst_inclusive: function () {
        var self = CreditNote;
        var gst = $("#GSTID  option:selected").val()
        if (gst == 1) {
            $("#IsInclusive").val(true);
        }
        else {
            $("#IsInclusive").val(false);
        }
    },
    edit: function () {
        var inclusive = $("#IsInclusive").val();
        if (inclusive == "True") {
            $("#GSTID").val(1);
        } else {
            $("#GSTID").val(2);
        }
    }

}