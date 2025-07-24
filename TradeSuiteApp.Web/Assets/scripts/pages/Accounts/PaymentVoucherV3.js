voucherCRUD.init = function () {
    var self = voucherCRUD;
    AccountHead.account_head_list();
    $('#account-head-list').SelectTable({
        selectFunction: self.select_aacount_head,
        modal: "#select-account-head",
        initiatingElement: "#ReportingToName"
    });
    freeze_header = $("#payment-voucher-items-list").FreezeHeader();
    self.voucherCreateAndUpdate();
    self.bind_events();
    $('#BankACNo').hide();
    self.count();
};

voucherCRUD.select_aacount_head = function () {
    var self = voucherCRUD;
    var radio = $('#select-account-head tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".AccountName").text().trim();   
    var CurrencyID = $(row).find(".CurrencyID").val();
    var CurrencyName = $(row).find(".CurrencyName").val();
    var CurrencyCode = $(row).find(".CurrencyCode").val();
    var CurrencyConversionRate = $(row).find(".CurrencyConversionRate").val();
    var DecimalPlaces = $(row).find(".DecimalPlaces").val();
    $("#AccountHead").val(Name);
    $("#AccountHeadID").val(ID);
    $('#NetAmt').val(0);
    $("#DecimalPlaces").val(DecimalPlaces);
    $("#CurrencyID").val(CurrencyID);
    $("#CurrencyName").val(CurrencyName);
    $("#CurrencyCode").val(CurrencyCode);
    $("#CurrencyExchangeRate").val(CurrencyConversionRate);
  //  self.on_account_head_change(ID);
    self.on_account_head_change(ID, CurrencyCode);
    UIkit.modal($('#select-account-head')).hide();
   
};

voucherCRUD.on_account_head_change = function (AccountHeadID) {
    if (typeof AccountHeadID === "undefined" || AccountHeadID === null) {
        AccountHeadID = $currObj.SelectedSupplierID;
    }
    else AccountHeadID = AccountHeadID;  
    var callBack = function (response) {
        var $response = $(response);
        app.format($response);
        $("#unSettledPurchaseInvoiceTblContainer").html($response);
        freeze_header = $("#tblUnSettledPurchaseInvoice").FreezeHeader();
        voucherCRUD.count();
        $('.txtPayNow').on('keyup', function (e) {
            var row = $(this).closest("tr");
            if (clean($(this).val()) != 0) {
                $(row).find('.include').prop("checked", true);
                $(row).find('.include').closest('div.icheckbox_md').addClass('checked');
            }
            var sum = 0;

            $('.txtPayNow').each(function () {
                sum += clean($(this).val());
            });

            $('#TotPayNow').html(sum.toFixed(2));
            $('#NetAmt').val(sum.toFixed(2));
            // $("#CurrencyCode").val(CurrencyCode);
            if (CurrencyCode) {
                $("#CurrencyCode").val(CurrencyCode);
            }
        });
    };
    var data = { AccountHeadID: AccountHeadID };
    AjaxRequest('/Accounts/PaymentVoucher/GetUnSettledPurchaseInvoicesV3', data, 'GET', callBack);
};

voucherCRUD.bind_events = function () {
    var self = voucherCRUD;
    $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
    $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
    $("#btnOKSupplier").on('click', self.select_supplier);
    $("#dropPayment").on("change", self.get_bank_name);
    $('body').on('ifChanged', '.include', self.include_item);
    $('.btnSaveAndPost, .btnSaveASDraft').on('click', self.on_save);
    $('.txtPayNow').on('keyup', self.calculate_sum);
    $("#btnAdd").on("click", self.add_item);
    $("#btnProcess").on("click", self.process_item);
    $("#btn-ok-account-head").on('click', self.select_aacount_head);
    $("body").on("ifChanged", "#OtherPayment", self.show_other_payments);
    $.UIkit.autocomplete($('#account-head-autocomplete'), Config.get_account_head_list);
    $('#account-head-autocomplete').on('selectitem.uk.autocomplete', self.set_account_head);
    //For ReconciledDate Updation
    $("body").on("click", ".btnReconciledDate", self.show_reconciled_date_modal);
    $("body").on("click", "#btnSaveReconciledDate", self.save_reconciled_date);

    //For Create AccountHead From VoucherDirectly(also add Div in the FormV3 with id=div-account-head)
    $("#btnAddAccountHead").off('click').on('click', AccountHeadAdd.add_account_head);
    $("#CurrencyID").on('change', self.get_currency_details);

};
voucherCRUD.get_currency_details = function () {
    var self = voucherCRUD;
    var CurrencyID = $("#CurrencyID").val();
    var LocalCurrencyID = $("#LocalCurrencyID").val();
    var NetAmt = $('#NetAmt').val();
    if (CurrencyID == null || CurrencyID == "") {
        CurrencyID = 0;
    }
    $.ajax({
        url: '/Masters/CurrencyConversion/GetDebitCurrencyDetails',
        data: {
            BaseCurrencyID: CurrencyID,
            ConversionCurrencyID: LocalCurrencyID
        },
        dataType: "json",
        type: "GET",
        success: function (response) {
            if (response.data != null) {
                $("#CurrencyExchangeRate").val(response.data.ExchangeRate);
                $('#LocalNetAmt').val(response.data.ExchangeRate * NetAmt);
            }
        }
    });
};
voucherCRUD.show_reconciled_date_modal = function () {
    var self = voucherCRUD;
    var ID = $(this).parents('tr').find('.ID').val();
    var BankReferanceNumber = $(this).parents('tr').find('.BankReferanceNumber').val();
    var Remarks = $(this).parents('tr').find('.Remarks').val();
    var BankName = $(this).parents('tr').find('.BankName').val();
    $('#show-reconciled-date').trigger('click');
    $("#ReceiptVoucherID").val(ID)
    $("#BankName").val(BankName)
    $("#Remarks").val(Remarks)
    $("#ReferenceNumber").val(BankReferanceNumber)
};

voucherCRUD.save_reconciled_date = function () {
    var self = voucherCRUD;
    self.error_count = 0;
    self.error_count = self.validate_reconsiled_date();
    if (self.error_count > 0) {
        return;
    }
    $.ajax({
        url: '/Accounts/PaymentVoucher/SaveReconciledDate',
        data: {
            ID: $("#ReceiptVoucherID").val(),
            ReconciledDate: $("#ReconciledDate").val(),
            BankReferanceNumber: $("#ReferenceNumber").val(),
            Remarks: $("#Remarks").val()
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "Success") {
                app.show_notice("ReconciledDate Saved Successfully");
                setTimeout(function () {
                    window.location = "/Accounts/PaymentVoucher/IndexV3";
                }, 1000);
            } else {
                if (typeof response.data[0].ErrorMessage != "undefined")
                    app.show_error(response.data[0].ErrorMessage);
            }
        }
    });
};

voucherCRUD.set_account_head = function (event, item) {
    var self = voucherCRUD;
    $("#AccountHeadID").val(item.id),
    $("#AccountHead").val(item.value);
    $('#NetAmt').val(0);
    $('#LocalNetAmt').val(0);
    $("#CurrencyCode").val(item.value);

    self.on_account_head_change(item.id);
},
voucherCRUD.on_save = function () {

    var self = voucherCRUD;
    var data = self.GetSaveObj();

    var location = "/Accounts/PaymentVoucher/CreateV3";
    var url = '/Accounts/PaymentVoucher/SaveV3';

    if ($(this).hasClass("btnSaveASDraft")) {
        data.IsDraft = true;
        url = '/Accounts/PaymentVoucher/SaveAsDraftV3'
        self.error_count = self.validateForm();
    } else {
        self.error_count = self.validateForm();

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
};

voucherCRUD.save = function (data, url, location) {

    var self = voucherCRUD;

    $(".btnSaveASDraft, .btnSaveAndPost").css({ 'display': 'none' });

    $.ajax({
        url: url,
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {

                app.show_notice(response.Message);

                setTimeout(function () {
                    window.location = location;
                }, 1000);

            } else {
                if (typeof response.data[0].ErrorMessage != "undefined")
                    app.show_error(response.data[0].ErrorMessage);
                $(".btnSaveASDraft, .btnSaveAndPost").css({ 'display': 'block' });

            }
        }
    });
},

voucherCRUD.GetSaveObj = function () {
    var self = voucherCRUD;
    var obj = {};
    if ($("#dropPayment option:selected").text().toUpperCase() == "CASH") {
        obj.PaymentMode = "Cash";
    }
    else {
        obj.PaymentMode = "Bank";
    }
    obj.ID = $('#ID').val();
    //obj.CurrencyID= $("#CurrencyID option:selected").val(),
    obj.VoucherNo = $('#txtVoucherNo').val();
    obj.VoucherDateStr = $('#txtVoucherDate').val();
    obj.AccountHeadID = $("#AccountHeadID").val();
    obj.AccountNumber = $('#txtAccountNumber').val();
    obj.ReferenceNumber = $('#txtRefernceNumber').val();
    obj.Description = $('#txtRemarks').val();
    obj.ReceiverBankID = $("#ReceiverBankID").val(),
        obj.BankInstrumentNumber = $("#BankInstrumentNumber").val(),
        obj.ChecqueDate = $("#ChecqueDate").val(),
        obj.Bankcharges = $("#Bankcharges").val(),
    //obj.CurrencyCode = $("#CurrencyCode").val(),
        obj.CurrencyCode = $("#CurrencyID option:selected").text(),
        obj.CurrencyID = $("#CurrencyID").val(),
        obj.LocalCurrencyCode = $("#LocalCurrencyCode").val(),
        obj.LocalCurrencyID = $("#CurrencyID").val(),
        obj.CurrencyExchangeRate = clean($("#CurrencyExchangeRate").val()),
        obj.LocalVoucherAmt = clean($("#LocalNetAmt").val()),
    $currObj.SaveType = 'save';
    if ($('#Bank option:selected').text() == "Select") {
        obj.BankName = "";
    }
    else {
        obj.BankName = $('#Bank option:selected').text();
    }
    //if ($("#ReceiverBankName option:selected").text() == "Select") {
    //    obj.ReceiverBankName = "";
    //}
    //else {
    //    obj.ReceiverBankName = $('#ReceiverBankName option:selected').text();;
    //}
    obj.BankID = $("#Bank").val();
    obj.PaymentTypeId = $('#dropPayment').val();
    obj.SaveType = $currObj.SaveType;

    var unProcessedItems = [];
    var paynow;
    $('#unSettledPurchaseInvoiceTblContainer #tblUnSettledPurchaseInvoice tbody tr:not(:last)').each(function () {
        $currRow = $(this);
        var unProcessedItem = {};
        unProcessedItem.PayableID = $currRow.find('.hdnPayableID').val();
        //unProcessedItem.Type = $currRow.find('.hdnType').val();
        unProcessedItem.Type = $currRow.find('.hdnDocType').val();
        unProcessedItem.Number = $currRow.find('td:eq(4)').html();
        unProcessedItem.DateStr = $currRow.find('td:eq(5)').html();
        unProcessedItem.Amount = $currRow.find('td:eq(6)').html();
        //unProcessedItem.Balance = $currRow.find('td:eq(4)').html();
        unProcessedItem.AmountToBePaid = clean($currRow.find('.txtAmtToPaid').val());
        unProcessedItem.Narration = $currRow.find('.Narrations').val(); 
        //narration = $currRow.find('.Narrations').val();
        //unProcessedItem.Narrations = narration;

        paynow = clean($currRow.find('.txtPayNow').val());
        unProcessedItem.PayNow = paynow;
        // unProcessedItem.PayNow =clean( $currRow.find('.txtPayNow').val());
        /// unProcessedItem.OriginalAmount = clean($currRow.find('.txtOriginalAmt').val());

        unProcessedItem.OriginalAmount = clean($currRow.find('.TotPayNow').val());

        unProcessedItem.AdvanceID = $currRow.find('.hdnAdvanceID').val();
        unProcessedItem.DebitNoteID = $currRow.find('.hdnDebitNoteID').val();
        unProcessedItem.CreditNoteID = $currRow.find('.hdnCreditNoteID').val();
        unProcessedItem.IRGID = $currRow.find('.hdnIRGID').val();
        unProcessedItem.PaymentReturnVoucherTransID = $currRow.find('.hdnPaymentReturnVoucherTransID').val();
        unProcessedItem.CreatedDateStr = $currRow.find('.hdnCreatedDate').val();
        unProcessedItem.DueDateStr = $currRow.find('.hdnDueDate').val();
        unProcessedItems.push(unProcessedItem);
    });

    obj.UnProcessedItems = unProcessedItems;


    return obj;
};

voucherCRUD.list = function () {
    var self = voucherCRUD;
    $('#tabs-paymentvoucher').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            self.tabbed_list(active_item.data('tab'));
            active_item.data('tab-loaded', true);
        }
    });
};

voucherCRUD.tabbed_list = function (type) {

    var $list;

    switch (type) {
        case "draft":
            $list = $('#draft-list');
            break;
        case "saved-paymentvoucher":
            $list = $('#savedpayment-list');
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

        var url = "/Accounts/PaymentVoucher/GetPaymentVoucherListV3?type=" + type;
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
                           + "<input type='hidden' class='BankReferanceNumber' value='" + row.BankReferanceNumber + "'>"
                           + "<input type='hidden' class='Remarks' value='" + row.Remarks + "'>"
                       + "<input type='hidden' class='PaymentTypeName' value='" + row.PaymentTypeName + "'>"
                           
                           + "<input type='hidden' class='BankName' value='" + row.BankName + "'>";

                   }
               },

               { "data": "VoucherNumber", "className": "VoucherNumber" },
               { "data": "VoucherDate", "className": "VoucherDate" },
               { "data": "AccountHead", "className": "AccountHead" },
                {
                    "data": "Amount", "searchable": false, "className": "Amount",
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.Amount + "</div>";
                    }
                },
                {
                    "data": "ReconciledDate", "searchable": false, "className": "ReconciledDate",
                    "render": function (data, type, row, meta) {
                        if (row.ReconciledDate == "01-Jan-1900" && row.PaymentTypeName != "CASH") {
                            return "<button class='md-btn md-btn-primary md-btn-block md-btn-wave-light waves-effect waves-button waves-light btnReconciledDate'>Edit</button>"
                        }
                        else if (row.PaymentTypeName == "CASH" || row.IsDraft == 1) {
                            return "<div class='' ></div>";
                        }
                        else {
                            return "<div class='' >" + row.ReconciledDate + "</div>";
                        }

                    }
                },
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.Status);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.ReconciledDate)').on('click', function () {
                    var Id = $(this).closest("tr").find("td .ID").val();
                    app.load_content("/Accounts/PaymentVoucher/DetailsV3/" + Id);

                });
            },
        });

        $list.find('thead.search input').on('keyup change', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });
    }
};

voucherCRUD.validate_reconsiled_date = function () {
    var self = voucherCRUD;
    if (self.rules.on_save_reconsiled_date.length > 0) {
        return form.validate(self.rules.on_save_reconsiled_date);
    }
    return 0;
};