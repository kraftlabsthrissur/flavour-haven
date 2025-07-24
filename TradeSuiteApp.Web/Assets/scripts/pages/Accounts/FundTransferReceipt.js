FundTransferReceipt = {

    init: function () {
        var self = FundTransferReceipt;
        self.fund_transfer_receipt_list();
        $('#fund-transfer-issue-list').SelectTable({
            selectFunction: self.select_transfer_issue_order,
            modal: "#select-source",
            initiatingElement: "#IssueNo",
            selectionType: "checkbox"
        })

        self.bind_events();

    },

    list: function () {
        var self = FundTransferReceipt;
        var $list = $('#Receipt-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/FundTransferReceipt/GetFundTransferReceipt"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "Desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                       }
                   },
                   { "data": "FundTransferNo", "className": "FundTransferNo" },
                   { "data": "FundTransferDate", "className": "FundTransferDate" },
                   { "data": "FromLocation", "className": "FromLocation" },
                   { "data": "ToLocation", "className": "ToLocation" },
                   { "data": "ModeOfPayment", "className": "ModeOfPayment" },
                   {
                       "data": "TotalAmount", "searchable": false, "className": "TotalAmount",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.TotalAmount + "</div>";
                       }
                   },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/FundTransferReceipt/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = FundTransferReceipt;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = FundTransferReceipt;
        $.ajax({
            url: '/Reports/Accounts/FundTransferReceiptPrintPdf',
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
        var self = FundTransferReceipt;
        $("#btnOk").on('click', self.select_transfer_issue_order);
        $(".btnSave").on("click", self.save_confirm);
        $("#FromLocationID").on('change', self.get_LocationWise_ToBank);
        $("#ToLocationID").on('change', self.get_LocationWise_FromBank);

    },

    get_LocationWise_ToBank: function () {
        var self = FundTransferReceipt;
        var locationid = $(this).val();
        if (locationid == null || locationid == "") {
            locationid = 0;
        }
        $.ajax({
            url: '/Accounts/FundTransfer/GetLocationWiseBank',
            data: { locationid: locationid },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#FromBankID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#FromBankID").append(html);
            }
        });
    },

    get_LocationWise_FromBank: function () {

        var locationid = $(this).val();
        if (locationid == null || locationid == "") {
            locationid = 0;
        }
        $.ajax({
            url: '/Accounts/FundTransfer/GetLocationWiseBank',
            data: { locationid: locationid },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#ToBankID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#ToBankID").append(html);
            }
        });
    },

    fund_transfer_receipt_list: function () {
        $list = $('#fund-transfer-issue-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Accounts/FundTransferReceipt/GetFundTransferIssueList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "IssueLocationID", Value: $('#FromLocationID').val() },
                        ];
                    }
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                            + "<input type='hidden' class='IssueTransID' value='" + row.ID + "' >";
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio IssueID' name='IssueID' data-md-icheck value='" + row.ID + "' >"

                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "FromLocationName", "className": "FromLocationName", "searchable": false, },
                    { "data": "FromBankName", "className": "FromBankName" },
                    { "data": "ToLocationName", "className": "ToLocationName" },
                    { "data": "ToBankName", "className": "ToBankName" },
                    { "data": "Payment", "className": "Payment" },
                    {
                        "data": "Amount", "searchable": false, "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        }
                    },

                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                }
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            $('body').on("change", '#FromLocationID', function () {
                list_table.fnDraw();
            });
            return list_table;
        }
    },

    select_transfer_issue_order: function () {
        var self = FundTransferReceipt;
        var radio = $('#fund-transfer-issue-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var TransNo = $(row).find(".TransNo").text().trim();
        var IssueLocation = $(row).find(".FromLocationName").text().trim();
        var IssueBankDetails = $(row).find(".FromBankName").text().trim();
        var ReceiptLocation = $(row).find(".ToLocationName").text().trim();
        var ReceiptBankDetails = $(row).find(".ToBankName").text().trim();
        var Amount = $(row).find(".Amount").text().trim();
        var PaymentMode = $(row).find(".Payment").text().trim();
        $("#IssueNo").val(TransNo);
        UIkit.modal($('#select-source')).hide();
        self.get_issue_items(ID);
    },

    get_issue_items: function (ID) {
        var self = FundTransferReceipt;
        $('#fund-transfer-receipt-item-list tbody tr').remove();
        $.ajax({
            url: '/Accounts/FundTransferReceipt/GetTransferIssuedItems',
            dataType: "json",
            data: {
                IssueID: ID
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td>' + (i + 1)
                        + ' <td class="TransNo">' + item.TransNo
                        + '     <input type="hidden" class="FromLocationID" value="' + item.FromLocationID + '" />'
                        + '     <input type="hidden" class="ToLocationID" value="' + item.ToLocationID + '" />'
                        + '     <input type="hidden" class="FromBankID" value="' + item.FromBankID + '" />'
                        + '     <input type="hidden" class="ToBankID" value="' + item.ToBankID + '" />'
                        + '     <input type="hidden" class="ModeOfPayment" value="' + item.ModeOfPayment + '" />'
                        + '     <input type="hidden" class="IssueTransID" value="' + item.ID + '" />'
                        + '</td>'
                        + ' <td>' + item.FromLocationName + '</td>'
                        + ' <td>' + item.ToLocationName + '</td>'
                        + ' <td>' + item.FromBankName + '</td>'
                        + ' <td>' + item.ToBankName + '</td>'
                        + ' <td class="NetAmount mask-sales-currency">' + item.Amount + '</td>'
                        + ' <td>' + item.Payment + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $('#fund-transfer-receipt-item-list tbody').append($content);
                var count = $('#fund-transfer-receipt-item-list tbody tr').length;
                $("#item-count").val(count);
                self.calculate_grid_total();
                self.count();
            }
        });
    },

    calculate_grid_total: function () {
        var self = FundTransferReceipt;
        var NetAmount = 0;
        $("#fund-transfer-receipt-item-list tbody tr").each(function () {
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        temp = NetAmount;
        //NetAmount = Math.round(NetAmount);
        $("#Amount").val(NetAmount);
    },

    get_data: function () {
        var self = FundTransferReceipt;
        var data = {};
        data.ID = $("#ID").val(),
        data.TransNo = $("#TransNo").val(),
        data.TransDate = $("#Date").val(),
        data.Items = [];
        var item = {};
        $('#fund-transfer-receipt-item-list tbody tr').each(function () {
            item = {};
            item.IssueTransID = clean($(this).find(".IssueTransID").val());
            item.FromLocationID = clean($(this).find(".FromLocationID").val());
            item.FromBankID = clean($(this).find(".FromBankID").val());
            item.ToLocationID = clean($(this).find(".ToLocationID").val());
            item.ToBankID = clean($(this).find(".ToBankID").val());
            item.ModeOfPayment = clean($(this).find(".ModeOfPayment").val());
            item.Amount = clean($(this).find(".NetAmount ").val());
            data.Items.push(item);
        });
        return data;
    },

    save_confirm: function () {
        var self = FundTransferReceipt;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = FundTransferReceipt;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Accounts/FundTransferReceipt/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Accounts/FundTransferReceipt/Index";
                }
                else {
                    app.show_error('Failed to create Fund Transfer Receipt');
                }
            }
        });
    },

    validate_save: function () {
        var self = FundTransferReceipt;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    count: function () {
        index = $("#fund-transfer-receipt-item-list tbody").length;
        $("#item-count").val(index);
    },


    rules: {
        on_save: [
            {
                elements: "#IssueNo",
                rules: [
                    { type: form.required, message: "Please choose an Issue" },
                    { type: form.non_zero, message: "Please choose an Issue" },
                ],
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                   { type: form.required, message: "Please add atleast one item" },

                ]
            },

        ],
    },



}