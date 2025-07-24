var freeze_header;
PaymentReturnVoucher = {
    init: function () {
        var self = PaymentReturnVoucher;
        self.debitnote_list();
        supplier.supplier_list('PaymentReturnVoucher');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#debitnote_for_return",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        $("#item-count").val($("#payment-return-list tbody tr").length);
        self.bind_events();
        self.freeze_headers();
        //self.populate_debitnote_list();
    },

    freeze_headers: function () {
        freeze_header = $("#payment-return-list").FreezeHeader();
    },

    details: function () {
        var self = PaymentReturnVoucher;
        $("body").on("click", ".printpdf", self.printpdf);
        freeze_header = $("#payment-return-list").FreezeHeader();
    },

    printpdf: function () {
        var self = PaymentReturnVoucher;
        $.ajax({
            url: '/Reports/Accounts/PaymentReturnVoucherPrintPdf',
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

    bind_events: function () {
        var self = PaymentReturnVoucher;
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#btnOKSupplier").on('click', self.select_supplier);
     //   $('body').on('change', "#PaymentTypeID", self.get_bank_name);
        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);
        $('body').on("click", ".btnSave", self.on_save);
        $('body').on("click", ".btnSaveAsDraft", self.on_save);
        $('#btnOKDebitNote').on('click', self.fill_debit_note);
        $('body').on("keyup", "#debitnote_for_return", self.show_po);
    },
    remove_item: function () {
        var self = PaymentReturnVoucher;
        $(this).closest("tr").remove();
        $("#payment-return-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#payment-return-list tbody tr").length);
    },
    count_items: function () {
        $("#payment-return-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        var count = $('#payment-return-list tbody tr').length;
        $('#item-count').val(count);
    },

    //get_bank_name: function () {
    //    var self = PaymentReturnVoucher;
    //    var mode;
    //    var Module = "Payment"//Payment(doubt)
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

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetPaymentSupplierForAutoComplete',
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

    set_supplier_details: function (event, item) {   // on select auto complete item

        if (($("#payment-return-list tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#payment-return-list tbody').empty();
                $("#SupplierName").val(item.name);
                $("#SupplierID").val(item.id);
                PaymentReturnVoucher.populate_debitnote_list();
            })
        }
        else {
            $("#SupplierName").val(item.name);
            $("#SupplierID").val(item.id);
            PaymentReturnVoucher.populate_debitnote_list();
        }
    },

    select_supplier: function () {
        var self = PaymentReturnVoucher;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var BankACNo = $(row).find(".BankACNo").val();
        if (($("#payment-return-list tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#payment-return-list tbody').empty();
                $("#SupplierName").val(Name);
                $("#SupplierID").val(ID);
                self.populate_debitnote_list();
                UIkit.modal($('#select-supplier')).hide();
            })
        }
        else {
            $("#SupplierName").val(Name);
            $("#SupplierID").val(ID);
            self.populate_debitnote_list();
            UIkit.modal($('#select-customer')).hide();
        }
    },

    populate_debitnote_list: function () {
        var self = PaymentReturnVoucher;
        var supplier_id = $('#SupplierID').val();
        $.ajax({
            url: '/Accounts/PaymentReturnVoucher/GetDebitNoteListForPaymentReturn/' + supplier_id,
            dataType: "json",
            //data: {
            //    Areas: 'Accounts'
            //},
            //type: "POST",
            type: "GET",
            data: {
                Term: '',
            },
            success: function (debitnotes) {
                var $debitnote_list = $('#debitnote-list tbody');
                $debitnote_list.html('');
                var tr = '';
                console.log(debitnotes);
                $.each(debitnotes, function (i, PaymentReturnVoucher) {

                    tr += "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1)
                        + "<input type=hidden class='DebitAccountCode' value='" + PaymentReturnVoucher.DebitAccountCode + "'/>"
                        + "</td>"
                        + "<td class='uk-text-center '><input type='radio'  class='DebitNoteID' name='DebitNoteID' data-md-icheck value=" + PaymentReturnVoucher.ID + " />"
                        + "<td class='TransNo'>" + PaymentReturnVoucher.TransNo + "</td>"
                        + "<td class='TransDate'>" + PaymentReturnVoucher.TransDate + "</td>"
                        + "<td class='DebitAccount'>" + PaymentReturnVoucher.DebitAccount + "</td>"
                        + "<td class='mask-currency DebitNoteAmount'>" + PaymentReturnVoucher.Amount + "</td>"
                    + "</tr>";

                });
                var $tr = $(tr);
                app.format($tr);
                $debitnote_list.append($tr);
            },
        });
    },

    //select_debitnote: function () {
    //    var self = PaymentReturnVoucher;
    //    var radio = $('#select-debitnote tbody input[type="radio"]:checked');
    //    var row = $(radio).closest("tr");
    //    var ID = radio.val();
    //    var Name = $(row).find(".Name").text().trim();
    //    var BankACNo = $(row).find(".BankACNo").val();
    //    if (($("#payment-return-list tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
    //        app.confirm("Selected Items will be removed", function () {
    //            $('#payment-return-list tbody').empty();
    //            $("#SupplierName").val(Name);
    //            $("#SupplierID").val(ID);
    //            UIkit.modal($('#select-supplier')).hide();
    //        })
    //    }
    //    else {
    //        $("#SupplierName").val(Name);
    //        $("#SupplierID").val(ID);
    //        UIkit.modal($('#select-customer')).hide();
    //    }
    //},

    show_po: function (e) {
        if (e.which == 13) {
            UIkit.modal($('#select_debitnote')).show();
        }

    },

    fill_debit_note: function () {
        var self = PaymentReturnVoucher;
        var radio = $('#select_debitnote tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var TransNo = $(row).find(".TransNo").text().trim();
        var DebitNoteAmount = $(row).find(".DebitNoteAmount").text().trim();
        var DebitAccount = $(row).find(".DebitAccount").text().trim();
        var DebitAccountCode = $(row).find(".DebitAccountCode").val();
        if (($("#payment-return-list tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#payment-return-list tbody').empty();
                $("#debitnote_for_return").val(TransNo);
                $("#DebitNoteID").val(ID);
                $("#Amount").val(DebitNoteAmount);
                $("#DebitAccount").val(DebitAccount);
                $("#DebitAccountCode").val(DebitAccountCode);
                UIkit.modal($('#select_debitnote')).hide();
            })
        }
        else {
            $("#debitnote_for_return").val(TransNo);
            $("#DebitNoteID").val(ID);
            $("#Amount").val(DebitNoteAmount);
            $("#DebitAccount").val(DebitAccount);
            $("#DebitAccountCode").val(DebitAccountCode);
            UIkit.modal($('#select_debitnote')).hide();
        }
    },

    debitnote_fill: function () {
        var self = PaymentReturnVoucher;

        var debitnote_ids = $("#debitnote-list debitnote-id:checked").map(function () {
            return $(this).val();
        }).get();
    },

    debitnote_list: function () {
        $debitnote_list = $('#debitnote-list');

        if ($debitnote_list.length) {
            $debitnote_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var debitnote_list_table = $debitnote_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            debitnote_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    debitnote_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    add_item: function () {
        var self = PaymentReturnVoucher;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.SupplierName = $("#SupplierName").val();
        item.SupplierID = $("#SupplierID").val();
        item.Amount = clean($("#Amount").val());
        item.Remarks = $("#Remarks").val();
        self.add_item_to_grid(item);
        self.clear_item();
    },

    add_item_to_grid: function (item) {
        var self = PaymentReturnVoucher;
        var index, tr;
        index = $("#payment-return-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + '<td class="supplier-name">' + item.SupplierName
                + '<input type="hidden" class = "supplier-id" value="' + item.SupplierID + '" />'
                + '</td>'
                + ' <td><input type="text" class = "md-input mask-currency amount" value="' + item.Amount + '" /></td>'
                + ' <td class="remarks">' + item.Remarks + '</td>'
                + ' <td class="uk-text-center">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#payment-return-list tbody").append($tr);
    },

    clear_item: function () {
        var self = PaymentReturnVoucher;
        $("#Amount").val('');
        $("#Remarks").val('');
    },


    on_save: function () {
        var self = PaymentReturnVoucher;
        var data = self.get_data();
        var location = "/Accounts/PaymentReturnVoucher/Index";
        var url = '/Accounts/PaymentReturnVoucher/Save';
        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Accounts/PaymentReturnVoucher/SaveAsDraft'
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
                    app.show_notice("Payment Return Voucher Created Successfully");
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

    get_data: function () {
        var self = PaymentReturnVoucher;
        var model = {
            ID: $("#ID").val(),
            VoucherNo: $("#VoucherNo").val(),
            VoucherDate: $("#VoucherDate").val(),
            SupplierID: clean($('#SupplierID').val()),
            SupplierName: $('#SupplierName').val(),
            PaymentTypeID: clean($('#PaymentTypeID').val()),
            BankID: clean($('#BankID').val()),
            BankReferenceNumber: $('#BankReferenceNumber').val(),
            DebitNoteID: $("#DebitNoteID").val(),
            TransNo: $("#debitnote_for_return").val(),
            DebitAccount: $("#DebitAccount").val(),
            DebitAccountCode: $("#DebitAccountCode").val(),

        };
        model.Items = [];
        var item = {};
        $("#payment-return-list tbody tr").each(function () {
            item = {};
            item.Amount = clean($(this).find('.amount').val());
            item.Remarks = $(this).find('.remarks').text();
            model.Items.push(item);
        });
        return model;
    },

    list: function () {
        var self = PaymentReturnVoucher;
        $('#tabs-payment-return-voucher').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "processed":
                $list = $('#processed-list');
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

            var url = "/Accounts/PaymentReturnVoucher/GetPaymentReturnVoucherList?type=" + type;

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
                   { "data": "SupplierName", "className": "SupplierName" },
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
                        app.load_content("/Accounts/PaymentReturnVoucher/Details/" + Id);

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
        var self = PaymentReturnVoucher;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_item: function () {
        var self = PaymentReturnVoucher;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
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
                     elements: "#SupplierID",
                     rules: [
                         { type: form.required, message: "Please select supplier", alt_element: "#SupplierID" },
                         { type: form.non_zero, message: "Please select supplier", alt_element: "#SupplierID" },
                         { type: form.positive, message: "Please select supplier", alt_element: "#SupplierID" },
                     ],
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
               elements: "#PaymentTypeID",
               rules: [

                   { type: form.required, message: "Please choose a mode of payment" },
                   { type: form.non_zero, message: "Please choose a mode of payment" },
               ]
           },

            {
                elements: "#BankID",
                rules: [
                    { type: form.required, message: "Please choose a bank" },
                ]
            },
            {
                elements: "#payment-return-list tbody tr .Amount",
                rules: [
                         { type: form.required, message: "Invalid Amount" },
                         { type: form.non_zero, message: "Invalid Amount" },
                         { type: form.positive, message: "Invalid Amount" },
                ]
            },
        ],
    },
}