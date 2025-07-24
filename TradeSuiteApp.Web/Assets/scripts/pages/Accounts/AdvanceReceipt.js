advance_receipt = {

    list: function () {
        $list = $('#advance-Receipt-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#advance-Receipt-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Accounts/AdvanceReceipt/Details/' + Id;
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

    init: function () {
        var self = advance_receipt;

        Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"

        });
        self.bind_events();
    },

    details: function () {

    },

    bind_events: function () {
        var self = advance_receipt;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("#btnOKCustomer").on("click", self.select_customer);
        $("#dropPayment").on("change", self.get_bank_name);
        $('body').on('keyup change', '#Advance-Receipt .txtAmount', self.calculateTDSAmount);
        $(".btnSaveAndPost").on("click", self.save);
    },

    get_bank_name: function () {
        var self = advance_receipt;
        var mode;
        var Module = "Receipt";
        if ($("#dropPayment option:selected").text() == "Select") {
            mode = "";
        }
        else if ($("#dropPayment option:selected").text() == "Cash") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }
        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                Module:Module,
                Mode: mode

            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#Bank").html("");
                var html = "<option value =''>Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#Bank").append(html);
            }
        });

    },

    get_customers: function (release) {
        var self = advance_receipt;

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

    set_customer: function (event, item) {
        var self = advance_receipt;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#StateID").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#DespatchDate").focus();
        self.get_sales_orders();

    },

    select_customer: function () {
        var self = advance_receipt;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        self.get_sales_orders();
        UIkit.modal($('#select-customer')).hide();
    },

    get_sales_orders: function (CustomerID) {
        var self = advance_receipt;
        var callBack = function (response) {
            $('#Advance-Receipt tfoot .totalAmount').val(0);
            var $response = $(response);
            app.format($response);
            $('#Advance-Receipt tbody').html($response);
            self.calculateTotals();
            $('#Advance-Receipt tbody tr').each(function (i) {
                $.UIkit.autocomplete($('#item-autocomplete' + (i + 1)), { 'source': self.get_item_details, 'minLength': 1 });
                $('#item-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', self.set_item_details);
            });
            var code;
            $('#Advance-Receipt tbody .ddlTDSCode').on('change', function () {
                code = $(this).val();
                var percentage = code.split("#");
                var parenttr = $(this).parents('tr');
                var amount = clean(parenttr.find('.txtAmount').val());
                var netamount;
                var tdsAmt = clean(self.getCalculatedTDSAmt(amount, clean(percentage[1])));
                parenttr.find('.txtTDSAmount').val(tdsAmt);
                netamt = amount - tdsAmt;
                parenttr.find('.txtNetAmount').val(netamt.toFixed(2));
                self.calculateTotals();

            });
            var currentSOID = 0;
        };
        this.ajaxRequest('/Accounts/AdvanceReceipt/GetSalesOrders',
            {
                CustomerID: $("#CustomerID").val(),
            }, "GET", callBack);
    },

    calculateTotals: function () {
        var total = 0;
        var totalTDS = 0;
        var totalnetamt = 0;

        $('#unProcessedItemsTblContainer table tbody tr').each(function () {
            var currtr = $(this);
            var amt = clean(currtr.find('.txtAmount').val());
            var tdsAmt = clean(currtr.find('.txtTDSAmount').val());
            var netAmt = clean(currtr.find('.txtNetAmount ').val());
            total += amt;
            totalTDS += tdsAmt;
            totalnetamt += netAmt;
        });
        $('#totalTDSAmount').val(totalTDS.toFixed(2));
        $('.totalNetAmount').val(totalnetamt.toFixed(2));
        $('.totalAmount').val(total.toFixed(2));
        $("#NetAmount").val(totalnetamt.toFixed(2));
    },

    get_item_details: function (release) {
        var hint = $(this.input).val();
        var SalesID = $(this.input).parents('tr').find('.hdnSOID').val();
        var TransNo = $(this.input).parents('tr').find('.transNo').text().trim();
        $.ajax({
            url: '/AdvanceReceipt/GetItemNamesForSalesOrder',
            data: {
                search: hint,
                TransNo: TransNo,
                SalesID: SalesID,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    ajaxRequest: function (url, data, requestType, callBack) {
        $.ajax({
            url: url,
            cache: false,
            type: requestType,
            data: data,
            success: function (successResponse) {
                if (callBack != null && callBack != undefined)
                    callBack(successResponse);
            },
            error: function (errResponse) {
            }
        });
    },

    getCalculatedTDSAmt: function (amt, percentage) {

        console.log(amt + "   " + percentage);
        if (amt == null || amt == undefined)
            amt = '0';
        if (amt == '' || amt.length <= 0)
        { amt = '0' }
        if (percentage != '') {
            return ((parseFloat(amt) / parseFloat(100)) * parseFloat(percentage)).toFixed(2);
        }
        else
            $('#txtTDSAmount').val(amt);

    },

    calculateTDSAmount: function () {
        var self = advance_receipt;


        var parenttr = $(this).parents('tr');
        var tdsAmt = 0;

        var amt = clean(parenttr.find('.txtAmount').val());
        var netamt = 0;

        if ($(this).val() != '') {
            var thisAmount = parseFloat($(this).val());
            tdsAmt = clean(self.getCalculatedTDSAmt(clean($(this).val()), parenttr.find('.ddlTDSCode').val()));
            netamt = amt - tdsAmt;
        }
        else
            tdsAmt = 0;

        parenttr.find('.txtTDSAmount').val(tdsAmt);
        parenttr.find('.txtNetAmount').val(netamt);


        self.calculateTotals();

    },

    get_data: function () {
        var item = {};
        var data = {
            AdvanceReceiptNo: $("#txtAdvancePaymentNo").val(),
            AdvanceReceiptDate: $("#AdvanceReceiptDate").val(),
            CategoryID: $("#CustomerCategory").val(),
            CustomerID: $("#CustomerID").val(),
            PaymentTypeID: $("#dropPayment").val(),
            BankName: $("#Bank option:selected").text(),
            ReferenceNo: $("#ReferenceNo").val(),
            Amount: clean($('#Advance-Receipt tfoot .totalAmount').val()),
            TDSAmount: clean($('#Advance-Receipt tfoot #totalTDSAmount').val()),
            NetAmount: clean($('#Advance-Receipt tfoot .totalNetAmount').val()),
        };
        data.Items = [];
        $('#Advance-Receipt tbody tr').each(function () {
            // var currRow = $(this);
            item = {};
            item.TransNo = $(this).find('.transNo').text().trim();
            item.SalesOrderDate = $(this).find('.sodate').text();
            item.ItemID = $(this).find('#hdnItemID').val();
            item.ItemName = $(this).find('.txtItemName').text();
            item.ItemAmount = clean($(this).find('.txtAmount').val());
            var splitvar = $(this).find('#ddlTDSCode  option:selected').val();
            var items = splitvar.split('#');
            item.TDSID = items[0];
            item.Remarks = $(this).find('.txtRemarks').text();
            data.Items.push(item);
        });

        return data;
    },

    save: function () {
        var self = advance_receipt;
        var data;

        self.error_count = 0;
        self.error_count = self.validateForm();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Accounts/AdvanceReceipt/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Accounts/AdvanceReceipt/Index";
                }
                else {
                    app.show_error('Failed to create Advance Receipt');
                }
            }
        });
    },

    set_item_details: function (event, item) {   // on select auto complete item


        var code = event.currentTarget.id;
        split = code.split("autocomplete");

        id = clean(split[1]);
        $("#txtItemName1").val(item.label);
        $(".hdnItemID" + id).val(item.id);
    },

    validateForm: function () {
        var self = advance_receipt;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_blur: [],
        on_submit: [

            {
                elements: ".totalAmount",
                rules: [
                    {
                        type: function (element) {
                            var total = clean($(element).val());
                            return total > 0;
                        }, message: "Total Amount must be greater than zero "
                    },
                ]
            },

             {
                 elements: "#dropPayment",
                 rules: [
                 { type: form.required, message: "Please choose a mode of payment" },
                 ]
             },
             {

                 elements: " #CustomerName",
                 rules: [
                 { type: form.required, message: "Please choose an Customer" },
                    { type: form.non_zero, message: "Please choose an Customer" },
                 ],
             },

                 {
                     elements: "#ReferenceNo",
                     rules: [
                     { type: form.required, message: "Please enter reference number" },
                     ]
                 },
                  {
                      elements: "#Bank",
                      rules: [
                      { type: form.required, message: "Please choose Bank Name" },
                      ]
                  }
        ]

    },


}