advance_return = {
    init: function () {
        var self = advance_return;
        Employee.employee_list('all');
        employee_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            modal: '#select-employee',
            returnFocus: "#dropPayment",
            initiatingElement: "#EmployeeName"
        });
        supplier.supplier_list('Creditors');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            modal: '#select-supplier',
            returnFocus: "#dropPayment",
            initiatingElement: "#SupplierName"
        });
        advance_return.bind_events();
        advance_return.advance_payment_list();
        advance_return.get_advance_payment();
    },

    advance_return_list: function () {
        var self = advance_return;
        $('#tabs-advance-return').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-advance-return":
                $list = $('#advance-return-list');
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

            var url = "/Accounts/AdvanceReturn/GetAdvanceReturnList?type=" + type;

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

                   { "data": "AdvanceReturnNo", "className": "AdvanceReturnNo" },
                   { "data": "AdvanceReturnDate", "className": "AdvanceReturnDate" },
                   { "data": "Name", "className": "Name" },
                   { "data": "AdvanceReturnCategory", "className": "AdvanceReturnCategory" },
                    {
                        "data": "Amount", "searchable": false, "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
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
                        app.load_content("/Accounts/AdvanceReturn/Details/" + Id);

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
        var self = advance_return;
        $('#supplier-list').on('datatable.changed', function () {
            select_table.refresh();
        });

        $('#select-supplier').on('show.uk.modal', function () {
            select_table.setFocus();
        });
        $('#SupplierName').keypress(function (e) {
            if (e.which == 13) {
                UIkit.modal('#select-supplier', { center: false }).show();
            }
        });

        $('#employee-list').on('datatable.changed', function () {
            employee_select_table.refresh();
        });

        $('#select-employee').on('show.uk.modal', function () {
            employee_select_table.setFocus();
        });
        $('#EmployeeName').keypress(function (e) {
            if (e.which == 13) {
                UIkit.modal('#select-employee', { center: false }).show();
            }
        });
        $("#ddlCategory").on('change', self.select_purpose);

        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);

        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $('body').on('click', '.btnSaveAndPost', self.save_confirm);
        $('body').on('click', '.btnSaveDraft', self.save_draft);
        $("#btnOkPaymentList").on("click", self.remove_item_details);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("keyup change", ".returnamount", self.check_paid_amount);

        $("#btnOKSupplier").on("click", self.select_supplier);
        $("#btnOKEmployee").on("click", self.select_employee);
        $('body').on('keyup change', '#Advance-Details .txtAmount', self.calculateTDSAmount);
        $("#dropPayment").on("change", self.get_bank_name);
    },
    advance_payment_list: function () {
        var self = advance_return;
        $list = $('#payment-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            self.advance_return_list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });

            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

        }
    },
    check_paid_amount: function () {
        var row = $(this).closest('tr');
        var PaidAmount = clean($(row).find('.paidamount').val());
        var ReturnAmount = clean($(row).find('.returnamount').val());
        var NetAmount = 0;
        if (PaidAmount < ReturnAmount) {
            app.show_error('Return amount should be lessthan or equal to paid amount  ');
            app.add_error_class($(row).find('.returnamount'));
        }
        else {
            $("#Advance-Details tbody tr").each(function () {
                var returnamount = clean($(this).find('.returnamount').val());
                NetAmount += returnamount;
                $("#NetAmount").val(NetAmount);
            })
        }

    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetAllSupplierAutoComplete',
            data: {
                Term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_supplier_details: function (event, item) {

        self = advance_return;
        var suppliername = item.value;
        var supplierid = item.id;
        var Employeeid = 0;
        if ($("#Advance-Details tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $("#Advance-Details tbody tr").html('');
                $("#payment-list tbody tr").html('');
                $("#item-count").val($("#Advance-Details tbody tr").length);              
                $("#SupplierName").val(suppliername);
                $("#SupplierID").val(supplierid);
                $("#EmployeeID").val(Employeeid);
                self.get_advance_payment();
            })
        }
        else {            
            $("#SupplierName").val(suppliername);
            $("#SupplierID").val(supplierid);
            $("#EmployeeID").val(Employeeid);
            self.get_advance_payment();
        }
    },

    select_employee: function (event, EmployeeList) {

        var self = advance_return;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var Department = $(row).find(".Department").val();
        if ($("#Advance-Details tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $("#Advance-Details tbody tr").html('');
                $("#payment-list tbody tr").html('');
                $("#item-count").val($("#Advance-Details tbody tr").length);
                $("#EmployeeName").val(Name);
                $("#hdnNameID").val(ID);
                $("#EmployeeID").val(ID);
                $("#hdnName").val(Name);
                UIkit.modal($('#select-employee')).hide();
                self.get_advance_payment();
            })
        }
        else {
            $("#EmployeeName").val(Name);
            $("#hdnNameID").val(ID);
            $("#EmployeeID").val(ID);
            $("#hdnName").val(Name);
            UIkit.modal($('#select-employee')).hide();
            self.get_advance_payment();
        }

    },

    set_employee: function (event, item) {
        var self = advance_return;
        var EmployeeID = item.id;
        var SupplierID = 0;
        if ($("#Advance-Details tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $("#Advance-Details tbody tr").html('');
                $("#payment-list tbody tr").html('');
                $("#item-count").val($("#Advance-Details tbody tr").length);
                $("#EmployeeID").val(EmployeeID)
                $("#SupplierID").val(SupplierID);
                self.get_advance_payment();
            })
        }
        else {
            $("#EmployeeID").val(EmployeeID);
            $("#SupplierID").val(SupplierID);
            self.get_advance_payment();
        }
    },

    select_supplier: function (event, SupplierList) {

        var self = advance_return;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var Employeeid = 0;

        if ($("#Advance-Details tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $("#Advance-Details tbody tr").html('');
                $("#payment-list tbody tr").html('');
                $("#item-count").val($("#Advance-Details tbody tr").length);
                $("#SupplierName").val(Name);
                $("#SupplierID").val(ID);
                $("#EmployeeID").val(Employeeid);
                UIkit.modal($('#select-supplier')).hide();
                self.get_advance_payment();
            })
        }
        else {

            $("#SupplierName").val(Name);
            $("#SupplierID").val(ID);
            $("#EmployeeID").val(Employeeid);
            UIkit.modal($('#select-supplier')).hide();
            self.get_advance_payment();
        }
    },

    select_purpose: function () {
        var select_category = $(this).val();

        $("#EmployeeWrapper").toggleClass('uk-hidden').toggleClass('selected');
        $("#SupplierWrapper").toggleClass('uk-hidden').toggleClass('selected');
        if (select_category == "Employee") {
            if ($("#Advance-Details tbody tr").length > 0) {
                app.confirm("Selected Items will be removed", function () {
                    $('#Advance-Details tbody').empty();
                    $("#payment-list tbody").html('');
                    $("#EmployeeName").val('');
                    $("#EmployeeID").val('');
                    $("#SupplierID").val('');
                    $("#SupplierName").val('');
                    $("#NetAmount").val('');
                    $("#item-count").val($("#Advance-Details tbody tr").length);
                })
            }
        }
        else {
            if ($("#Advance-Details tbody tr").length > 0) {
                app.confirm("Selected Items will be removed", function () {
                    $('#Advance-Details tbody').empty();
                    $("#payment-list tbody").html('');
                    $("#SupplierName").val('');
                    $("#SupplierID").val('');
                    $("#EmployeeName").val('');
                    $("#EmployeeID").val('');
                    $("#NetAmount").val('');
                    $("#item-count").val($("#Advance-Details tbody tr").length);
                },
                (function () {
                    alert(0);
                }));

            }
        }

    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetAllSupplierAutoComplete',
            data: {
                Term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },


    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    get_advance_payment: function () {
        var self = advance_return;

        var SupplierID = $("#SupplierID").val();
        var EmployeeID = $("#EmployeeID").val();

        $.ajax({
            url: '/Accounts/AdvancePayment/GetAdvancePaymentList/',
            dataType: "json",
            type: "GET",
            data: {
                SupplierID: SupplierID,
                EmployeeID: EmployeeID,
            },
            success: function (response) {
                self.advance_return_list_table.fnDestroy();

                var $advance_payment_list = $('#payment-list tbody');
                $advance_payment_list.html('');
                var tr = '';
                $.each(response, function (i, advance_payment) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + " <td class='uk-text-center spclCheck' data-md-icheck><input type='checkbox' name='inputrequestno' id='paymentno-id' class='spclCheckItem' value=" + advance_payment.ID + " /></td>"
                        + "<td>" + advance_payment.AdvancePaymentNo + "</td>"
                        + "<td>" + advance_payment.AdvancePaymentDate + "</td>"
                        + "<td>" + advance_payment.SupplierName + "</td>"
                        + "<td>" + advance_payment.Category + "</td>"
                        + "<td>" + advance_payment.Purpose + "</td>"
                        + "<td>" + advance_payment.Amount + "</td>"
                        + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $advance_payment_list.append($tr);
                });
                self.advance_payment_list();
            },

        });
    },
    remove_item_details: function () {
        var self = advance_return;
        if ($("#Advance-Details tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                self.select_payment();
            })
        }
        else {
            self.select_payment();
        }
    },
    remove_item: function () {
        var self = advance_return;
        $(this).closest("tr").remove();
        $("#Advance-Details tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#Advance-Details tbody tr").length);
        var NetAmount = 0;
        $("#Advance-Details tbody tr").each(function () {
            var returnamount = clean($(this).find('.returnamount').val());
            NetAmount += returnamount;
            $("#NetAmount").val(NetAmount);
        })
        // fh_items.resizeHeader();
    },
    select_payment: function () {
        var self = advance_return;
        var payment_ids = $("#payment-list .spclCheckItem:checked").map(function () {
            return $(this).val();
        }).get();
        if (payment_ids.length == 0) {
            app.show_error('Please select atleast one Advance Payment');
            return;
        }
        $.ajax({
            url: '/Accounts/AdvancePayment/GetUnprocessedAdvancePaymentTrans/',
            dataType: "json",
            data: {
                PaymentIDs: payment_ids,
            },
            type: "POST",
            success: function (payment_items) {
                var $payment_list = $('#Advance-Details tbody');
                $payment_list.html('');
                var tr = '';
                var BatchName;
                var netamount = 0;
                var returnamount;
                $("#NetAmount").val(netamount);

                $.each(payment_items, function (i, payment_item) {
                    netamount = clean($("#NetAmount").val());
                    returnamount = payment_item.Amount;
                    netamount += returnamount;
                    $("#NetAmount").val(netamount);
                    tr += " <tr>"
                                + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                                + "<input type='hidden' class='payment-trans-id' value='" + payment_item.ID + "'/>"
                                + "<input type='hidden' class='itemid' value='" + payment_item.ItemID + "'/>"
                                + "</td>"
                                + "<td>" + payment_item.AdvancePaymentNo + "</td>"
                                + "<td class='uk-text'>" + payment_item.PurchaseOrderDate + "</td>"
                                + "<td class='uk-text '>" + payment_item.ItemName + "</td>"
                                + "<td class='uk-text-right mask-qty paidamount'>" + payment_item.Amount + "</td>"
                                + ' <td><input type="text" class="md-input uk-text-right mask-qty returnamount" value=" ' + payment_item.Amount + ' "/></td>'
                                + ' <td class="uk-text-center">'
                                + '     <a class="remove-item">'
                                + '         <i class="uk-icon-remove"></i>'
                                + '     </a>'
                                + ' </td>'
                                + "</tr>";
                });
                var $tr = $(tr);
                app.format($tr);
                $payment_list.append($tr);
                self.count_items();
            },
        });
    },
    count_items: function () {
        var count = $('#Advance-Details  tbody tr').length;
        $('#item-count').val(count);
    },
    check_stock_quantity: function () {
        var row = $(this).closest('tr');
        var ReturnAmount = clean($(row).find('.returnamount').val());
        var PaidAmount = clean($(row).find('.paidamount').val());
        var NetAmount = 0;
        if (ReturnAmount > PaidAmount) {
            app.show_error('Return amount shold be less than or equal to paod amount');
            app.add_error_class($(row).find('.returnamount'));
        }
        else {
            $("#Advance-Details tbody tr").each(function () {

                ReturnAmount = clean($(row).find('.returnamount').val());
                NetAmount += ReturnAmount;
            })
        }
    },

    save_confirm: function () {
        var self = advance_return;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            IsDraft = false;
            self.save();
        }, function () {
        })
    },

    save_draft: function () {
        var self = advance_return;
        self.error_count = 0;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        if ($(this).hasClass("btnSaveDraft")) {
            IsDraft = true;
            self.save();
        }

    },

    save: function () {
        var self = advance_return;
        var model = self.get_data(IsDraft);
      
        $(".btnSaveAndPost .btnSaveDraft").css({ 'display': 'none' });
        $.ajax({
            url: '/Accounts/AdvanceReturn/Save',
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Advance Return created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/AdvanceReturn/Index";
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveAndPost").css({ 'display': 'block' });
                }
            },
        });


    },
    get_data: function (IsDraft) {
        var self = advance_return;
        var model = {
            ID: $("#ID").val(),
            Date: $("#Date").val(),
            Category: $("#ddlCategory").val(),
            EmployeeID: $("#EmployeeID").val(),
            SupplierID: $("#SupplierID").val(),
            NetAmount: clean($("#NetAmount").val()),
            PaymentTypeID: $("#dropPayment option:selected").val(),
            BankID: $("#BankID option:selected").val(),
            ReferenceNumber: $("#ReferenceNumber").val(),
            Remarks: $("#Remarks").val(),
            IsDraft : IsDraft
        };
        model.Items = self.GetProductList();
        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];
        $("#Advance-Details tbody tr").each(function () {
            ProductsArray.push(
                {
                    ItemID: $(this).find('.itemid').val(),
                    AdvanceID: clean($(this).find('.payment-trans-id').val()),
                    // BatchID: 0,
                    ReturnAmount: clean($(this).find('.returnamount').val()),
                    AdvancePaidAmount: clean($(this).find('.paidamount').text()),
                }
            );
        })
        return ProductsArray;
    },

    get_bank_name: function () {
        var self = advance_return;
        var mode;
        var Module = "Payment";
        if ($("#dropPayment option:selected").text() == "Select") {
            mode = "";
        }
        else if ($("#dropPayment option:selected").text() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }
        $.ajax({
            url: '/Accounts/PaymentVoucher/GetSerialNo',
            data: {
                Mode: mode
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#txtVoucherNo").val(response.data);
                }
            }
        });
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
                var html ;
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'  data-creditBalance='" + record.CreditBalance + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });
    },
    validate_form: function () {
        var self = advance_return;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [

            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Please check net amount" },
                    { type: form.non_zero, message: "Please check net amount" },
                    { type: form.positive, message: "Please check net amount" },

                ]
            },

             {
                 elements: ".returnamount",
                 rules: [
                    
                     { type: form.non_zero, message: "Return amount must be grater than zero" },
                     { type: form.positive, message: "Return amount must be grater than zero" },
                     {
                         type: function (element) {
                             var error = false;
                             $('.returnamount').each(function () {
                                 var row = $(this).closest('tr');
                                 var ReturnAmount = clean($(row).find('.returnamount').val());
                                 var PaidAmount = clean($(row).find('.paidamount').val());
                                 if (ReturnAmount > PaidAmount)
                                     error = true;

                             });
                             return !error;
                         }, message: 'Returned Amount should be less than or equal to paid amount '
                     },
                 ]
             },

             {
                 elements: "#dropPayment",
                 rules: [
                    { type: form.required, message: "Mode Of Payment is Required" },
                 ]
             },

             {
                 elements: "#BankID",
                 rules: [
                    { type: form.required, message: "Bank Name is Required" },
                 ]
             },

        ],
    }
}



