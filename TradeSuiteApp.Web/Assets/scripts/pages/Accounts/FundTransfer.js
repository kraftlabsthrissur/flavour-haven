$(function () {
    FundTransfer.init();
});
var fh_items;
FundTransfer = {
    init: function () {
        var self = FundTransfer;
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
            fh_items = $("#fund-transfer-item-list").FreezeHeader();
        }
        self.bind_events();
    },
    list: function () {
        var $list = $('#fund-transfer-list');
        if ($list.length) {
            $('#fund-transfer-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Accounts/FundTransfer/Details/' + Id;
            });

            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    details: function () {
        var self = FundTransfer;
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = FundTransfer;
        $.ajax({
            url: '/Reports/Accounts/FundTransferPrintPdf',
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



    list: function () {
        var self = FundTransfer;
        $('#tabs-savedFundTranasfer').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-fund-transfer":
                $list = $('#fund-transfer-list');
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

            var url = "/Accounts/FundTransfer/GetFundTransferList?type=" + type;
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
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/FundTransfer/Details/" + Id);

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
        self = FundTransfer;
        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);
        $(".btnSaveFt").on("click", self.on_save);
        $(".btnSaveAsDraft").on("click", self.on_save);
        $(".btnSaveDnNew").on("click", self.on_save);
        $("#FromLocation").on('change', self.get_LocationWise_FromBank);
        $("#ToLocation").on('change', self.get_LocationWise_ToBank);
    },

    save_confirm: function () {
        var self = FundTransfer
        app.confirm_cancel("Do you want to Save", function () {
            self.submit();
        }, function () {
        })
    },

    rules: {
        on_add_item: [

                {
                    elements: "#FromLocation",
                    rules: [
                        { type: form.required, message: "Please Select From Location" },
                    ]
                },
                 {
                     elements: "#InstrumentNumber",
                     rules: [
                         { type: form.required, message: "Instrument Number required" },
                     ]
                 },
                 {
                     elements: "#InstrumentDate",
                     rules: [
                         { type: form.required, message: "Instrument Date required" },
                     ]
                 },
                {
                    elements: "#ToLocation",
                    rules: [
                        { type: form.required, message: "Please Select To Location" },

                    ]
                },
                {
                    elements: "#FromBank",
                    rules: [
                        { type: form.required, message: "Please Select FromBankDetail" },

                    ]
                },
                {
                    elements: "#ToBank",
                    rules: [
                        { type: form.required, message: "Please Select ToBankDetail" },
                        {
                            type: function (element) {
                                var error = false;
                                var FromBank = ($('#FromBank').val());
                                var ToBank = ($('#ToBank').val());

                                if (FromBank == ToBank) {
                                    error = true;
                                } return !error;
                            }, message: 'Please select  different Banks'
                        }

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

             //{
             //    elements: ".Amount",
             //    rules: [
             //            {
             //                type: function (element) {
             //                    var error = false;
             //                    var sum = 0;
             //                    var bankid = clean($(element).closest('tr').find('.FromBankID').val())
             //                    $("#fund-transfer-item-list tbody tr .FromBankID[value='" + bankid + "']").each(function () {
             //                        sum += clean($(this).closest('tr').find(".Amount").text());
             //                    });
             //                    var CreditBalance = clean($(element).closest('tr').find(".CreditBalance").val());
             //                    return CreditBalance >= sum;

             //                }, message: 'Total exceeds credit limit'
             //            },

             //    ]
             //},
        ],
    },
    add_item: function () {
        var self = FundTransfer;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.FromLocationID = $("#FromLocation").val();
        item.FromLocation = ($("#FromLocation :selected").text());
        item.ToLocationID = $("#ToLocation").val();
        item.ToLocation = ($("#ToLocation :selected").text());
        item.FromBankID = $("#FromBank").val();
        item.FromBank = ($("#FromBank :selected").text());
        item.ToBankID = $("#ToBank").val();
        item.ToBank = ($("#ToBank :selected").text());
        item.Amount = clean($("#Amount").val());
        item.ModeOfPaymentID = ($("#ModeOfPaymentID").val());
        item.ModeOfPayment = ($("#ModeOfPaymentID :selected").text());
        item.InstrumentNumber = ($("#InstrumentNumber").val());
        item.InstrumentDate = ($("#InstrumentDate").val());
        item.Remarks = ($("#Remarks").val());
        item.CreditBalance = $("#FromBank :selected").data('creditbalance');
        self.add_item_to_grid(item);
        self.clear_item();
        self.calculate_grid_total();


    },
    validate_item: function () {
        var self = FundTransfer;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },
    add_item_to_grid: function (item) {
        var self = FundTransfer;
        var index, tr;
        index = $("#fund-transfer-item-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td class="FromLocation">' + item.FromLocation
                + ' <input type="hidden" class="FromLocationID" value="' + $('#FromLocation').val() + '" /></td>'
                + ' <td class="ToLocation">' + item.ToLocation
                + ' <input type="hidden" class="ToLocationID" value="' + $('#ToLocation').val() + '" /></td>'
                + ' <td class="FromBank">' + item.FromBank
                + ' <input type="hidden" class="FromBankID" value="' + $('#FromBank').val() + '" /></td>'
                + ' <input type="hidden" class="CreditBalance" value="' + item.CreditBalance + '" /></td>'
                + ' <td class="ToBank">' + item.ToBank
                + ' <input type="hidden" class="ToBankID" value="' + $('#ToBank').val() + '" /></td>'
                + ' <td class="Amount mask-currency">' + item.Amount + '</td>'
                + ' <td class="ModeOfPayment">' + item.ModeOfPayment
                + ' <input type="hidden" class="ModeOfPaymentID" value="' + $('#ModeOfPaymentID').val() + '" /></td>'
                + ' <td class="InstrumentNumber">' + item.InstrumentNumber + '</td>'
                + ' <td class="InstrumentDate">' + item.InstrumentDate + '</td>'
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
        $("#fund-transfer-item-list tbody").append($tr);
        fh_items.resizeHeader();
    },
    remove_item: function () {
        var self = FundTransfer;
        $(this).closest("tr").remove();
        $("#fund-transfer-item-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#fund-transfer-item-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader();
    },
    calculate_grid_total: function () {
        var NetAmount = 0;

        $("#fund-transfer-item-list tbody tr").each(function () {

            NetAmount += clean($(this).find('.Amount').val());
        });
        $("#TotalAmount").val(NetAmount);
    },
    clear_item: function () {
        var self = FundTransfer;
        $("#FromBank").val('');
        $("#ToBank").val('');
        $("#Amount").val('');
        $("#InstrumentNumber").val('');
        $("#InstrumentDate").val('');
        $("#Remarks").val('');
    },

    on_save: function () {

        var self = FundTransfer;
        var data = self.get_data();
        var location = "/Accounts/FundTransfer/Index";
        var url = '/Accounts/FundTransfer/Save';

        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Accounts/FundTransfer/SaveAsDraft'
            self.error_count = self.validate_form();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveDnNew")) {
                location = "/Accounts/FundTransfer/Create";
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

       
        $(".btnSaveDnNew,.btnSaveFt,.btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data:data,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Fund Transfer created successfully");
                    setTimeout(function () {
                        window.location =location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveFtNew,.btnSaveFt,.btnSaveAsDraft").css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function (IsDraft) {
        var self = FundTransfer;

        var model = {
            ID: $("#ID").val(),
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            TotalAmount: clean($('#TotalAmount').val()),
            IsDraft: IsDraft
        };
        model.Items = self.GetProductList();

        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];

        var row;
        $("#fund-transfer-item-list tbody tr").each(function () {
            row = $(this);
            var FromLocationID = $(row).find('.FromLocationID').val();
            var ToLocationID = $(row).find('.ToLocationID').val();
            var FromBankID = $(row).find('.FromBankID').val();
            var ToBankID = $(row).find('.ToBankID').val();
            var Amount = clean($(row).find('.Amount').val());
            var ModeOfPaymentID = $(row).find('.ModeOfPaymentID').val();
            var InstrumentNumber = $(row).find('.InstrumentNumber').text();
            var InstrumentDate = $(row).find('.InstrumentDate').text();
            var Remarks = $(row).find('.Remarks').text();

            ProductsArray.push({
                FromLocationID: FromLocationID,
                ToLocationID: ToLocationID,
                FromBankID: FromBankID,
                ToBankID: ToBankID,
                Amount: Amount,
                ModeOfPaymentID: ModeOfPaymentID,
                InstrumentNumber: InstrumentNumber,
                InstrumentDate: InstrumentDate,
                Remarks: Remarks
            });
        })
        return ProductsArray;
    },
    save_new: function () {
        var self = FundTransfer;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var model = self.get_data();
        $(".btnSaveDnNew, .btnSaveFt,.btnSaveAsDraft").css({ 'display': 'none' });
        $.ajax({
            url: '/Accounts/FundTransfer/Save',
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Fund Transfer created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/FundTransfer/Create";
                    }, 1000);
                } else {
                    app.show_error("Failed to Fund Transfer");
                    $(".btnSaveFtNew, .btnSaveFt,.btnSaveAsDraft ").css({ 'display': 'block' });
                }
            },
        });
    },
    validate_form: function () {
        var self = FundTransfer;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    save_as_draft: function () {
        var self = FundTransfer;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(true);
        }
    },
    submit: function () {
        var self = FundTransfer;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(false);
        }
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
                $("#FromBank").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "' data-creditBalance='" + record.CreditBalance + "'>" + record.BankName + "</option>";
                });
                $("#FromBank").append(html);
            }
        });
    },
    get_LocationWise_ToBank: function () {

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
                $("#ToBank").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#ToBank").append(html);
            }
        });
    },
   
}


