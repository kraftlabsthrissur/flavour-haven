var customer_list;
var freeze_header;
CustomerReturnVoucher = {
    init: function () {
        var self = CustomerReturnVoucher;
        customer_list = Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        self.bind_events();
        self.freeze_headers();
    },

    details: function () {
        var self = CustomerReturnVoucher;
        $("body").on("click", ".printpdf", self.printpdf);
        self.freeze_headers();
    },

    printpdf: function () {
        var self = CustomerReturnVoucher;
        $.ajax({
            url: '/Reports/Accounts/CustomerReturnVoucherPrintPdf',
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
        freeze_header = $("#customer-return-list").FreezeHeader();
    },

    list: function () {
        var self = CustomerReturnVoucher;
        $('#tabs-customer-return-voucher').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-return-voucher":
                $list = $('#saved-voucher-list');
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

            var url = "/Accounts/CustomerReturnVoucher/GetCustomerReturnVoucherList?type=" + type;

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

                   { "data": "VoucherNo", "className": "VoucherNo" },
                   { "data": "VoucherDate", "className": "VoucherDate" },
                   { "data": "CustomerName", "className": "CustomerName" },
                   { "data": "PaymentTypeName", "className": "PaymentTypeName" },
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
                        app.load_content("/Accounts/CustomerReturnVoucher/Details/" + Id);

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
        var self = CustomerReturnVoucher;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $('body').on('click', '#btnOKCustomer', self.select_customer);
     //   $('body').on('change', "#PaymentTypeID", self.get_bank_name);
        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);
        $('body').on("click", ".btnSave", self.on_save);
        $('body').on("click", ".btnSaveAsDraft", self.on_save);
    },

    get_customers: function (release) {
        var self = CustomerReturnVoucher;
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
        var self = CustomerReturnVoucher;
        if (($("#customer-return-list tbody tr").length > 0) && (item.id != $("#CustomerID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#customer-return-list tbody').empty();
                $("#CustomerName").val(item.Name);
                $("#CustomerID").val(item.id);
            })
        }
        else {
            $("#CustomerName").val(item.Name);
            $("#CustomerID").val(item.id);
        }
    },

    select_customer: function () {
        var self = CustomerReturnVoucher;
        var radio = $('#customer-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        if (($("#customer-return-list tbody tr").length > 0) && (ID != $("#CustomerID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#customer-return-list tbody').empty();
                $("#CustomerName").val(Name);
                $("#CustomerID").val(ID);
                UIkit.modal($('#select-customer')).hide();
            })
        }
        else {
            $("#CustomerName").val(Name);
            $("#CustomerID").val(ID);
            UIkit.modal($('#select-customer')).hide();
        }
    },

    //get_bank_name: function () {
    //    var self = CustomerReturnVoucher;
    //    var mode;
    //    var Module = "Payment"
    //    if ($("#PaymentTypeID option:selected").text() == "Select") {
    //        mode = "";
    //        $("#Date").val('');
    //    }
      
    //    $.ajax({
    //        url: '/Masters/Treasury/GetBankList',
    //        data: {
               
    //        },
    //        dataType: "json",
    //        type: "GET",
    //        success: function (response) {
    //            $("#BankID").html("");
    //            var html = "";
    //            $.each(response.data, function (i, record) {
    //                html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
    //            });
    //            $("#BankID").append(html);
    //        }
    //    });

    //},

    count_items: function () {
        $("#customer-return-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        var count = $('#customer-return-list tbody tr').length;
        $('#item-count').val(count);
    },

    add_item: function () {
        var self = CustomerReturnVoucher;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.CustomerName = $("#CustomerName").val();
        item.Amount = clean($("#Amount").val());
        item.Remarks = $("#Remarks").val();
        self.add_item_to_grid(item);
        self.clear_item();
    },

    validate_item: function () {
        var self = CustomerReturnVoucher;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    add_item_to_grid: function (item) {
        var self = CustomerReturnVoucher;
        var index, tr;
        index = $("#customer-return-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td class="CustomerName uk-text-center">' + item.CustomerName + '</td>'
                + ' <td class="Amount mask-currency">' + item.Amount + '</td>'
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
        $("#customer-return-list tbody").append($tr);
    },

    clear_item: function () {
        var self = CustomerReturnVoucher;
        $("#Amount").val('');
        $("#Remarks").val('');
    },

    remove_item: function () {
        var self = CustomerReturnVoucher;
        $(this).closest("tr").remove();
        $("#customer-return-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#customer-return-list tbody tr").length);
    },

    submit: function () {
        var self = CustomerReturnVoucher;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(false);
        }
    },

    validate_form: function () {
        var self = CustomerReturnVoucher;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    on_save: function () {
        var self = CustomerReturnVoucher;
        var data = self.get_data();
        var location = "/Accounts/CustomerReturnVoucher/Index";
        var url = '/Accounts/CustomerReturnVoucher/Save';
        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Accounts/CustomerReturnVoucher/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            self.error_count = self.validate_form();
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
        $(".btnSave,.btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Customer Return Voucher Created Successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSave,.btnSaveAsDraft").css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function (IsDraft) {
        var self = CustomerReturnVoucher;
        var model = {
            ID: $("#ID").val(),
            VoucherNo: $("#VoucherNo").val(),
            VoucherDate: $("#VoucherDate").val(),
            CustomerID: clean($('#CustomerID').val()),
            CustomerName: $('#CustomerName').val(),
            PaymentTypeID: clean($('#PaymentTypeID').val()),
            BankID: clean($('#BankID').val()),
            BankReferenceNumber: $('#BankReferenceNumber').val(),
            IsDraft: IsDraft
        };
        model.Items = [];
        var item = {};
        $("#customer-return-list tbody tr").each(function () {
            item = {};
            item.CustomerName = $(this).find(".CustomerName").text(),
            item.Amount = clean($(this).find('.Amount').val());
            item.Remarks = $(this).find('.Remarks').text();
            model.Items.push(item);
        });
        return model;
    },

    rules: {
        on_add_item: [
                 {
                     elements: "#Amount",
                     rules: [
                         { type: form.required, message: "Invalid Amount" },
                         { type: form.non_zero, message: "Invalid Amount" },
                         { type: form.positive, message: "Invalid Amount" },
                     ]
                 },
                 {
                     elements: "#CustomerID",
                     rules: [
                         { type: form.required, message: "Please select customer" },
                         { type: form.non_zero, message: "Please select customer" },
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
               elements: "#CustomerID",
               rules: [
                   { type: form.required, message: "Please select customer" },
                   { type: form.non_zero, message: "Please select customer" },
               ]
           },

           {
               elements: "#PaymentTypeID",
               rules: [
                   { type: form.required, message: "Please choose a mode of payment" },
               ]
           },

            {
                elements: "#BankID",
                rules: [
                    { type: form.required, message: "Please choose a bank" },
                ]
            },
            {
                elements: "#customer-return-list tbody tr .Amount",
                rules: [
                         { type: form.required, message: "Invalid Amount" },
                         { type: form.non_zero, message: "Invalid Amount" },
                         { type: form.positive, message: "Invalid Amount" },
                ]
            },
        ],
    },

}
