var fh_items;
var previous_status;
ChequeStatus = {
    init: function () {
        var self = ChequeStatus
        self.bind_events();
        fh_items = $("#cheque-status-item-list").FreezeHeader();
    },

    list: function () {
        var self = ChequeStatus;
        $('#tabs-cheque-status').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-cheque-status":
                $list = $('#saved-cheque-status-list');
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

            var url = "/Accounts/ChequeStatus/GetChequeStatusList?type=" + type;

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
                   { "data": "InstrumentStatus", "className": "InstrumentStatus" },
                   { "data": "FromReceiptDate", "className": "FromReceiptDate" },
                   { "data": "ToReceiptDate", "className": "ToReceiptDate" },
                   { "data": "CustomerName", "className": "CustomerName" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/ChequeStatus/Details/" + Id);

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
        var self = ChequeStatus;
        $("body").on('ifChanged', '.chkCheck', self.include_item);
        $("#btnShowStatus").on("click", self.show);
        $("#InstrumentStatusID").on("change", self.remove_data);
        $("#InstrumentStatusID").on("focus", self.set_previous_status);
        $("body").on("change", '.ChequeStatus', self.change_check_status);
        $("body").on("keyup", ".ChargesToCustomer", self.update_customer_charge);
        $(".btnSave, .btnSaveAsDraft").on("click", self.on_save);

    },
    details: function () {
        var self = ChequeStatus;
        fh_items = $("#cheque-status-item-list").FreezeHeader();
    },

    change_check_status: function () {
        var self = ChequeStatus;
        var element = this;
        $(this).removeAttr('disabled');
        $(element).find("option").each(function () {
            $(element).removeClass($(this).val());
        });
        $(element).addClass($(element).val());

        if ($(element).val() == 'Bounced') {
            $(element).closest('tr').find('.ChargesToCustomer').prop("disabled", false);
        } else {
            $(element).closest('tr').find('.ChargesToCustomer').prop("disabled", true);
            $(element).closest('tr').find('.ChargesToCustomer').val('');
            $(element).closest('tr').find('.CGST').val('');
            $(element).closest('tr').find('.SGST').val('');
            $(element).closest('tr').find('.IGST').val('');
        }

    },

    //save_confirm: function () {
    //    var self = ChequeStatus
    //    app.confirm_cancel("Do you want to Save", function () {
    //        self.submit();
    //    }, function () {
    //    })
    //},

    include_item: function () {
        var self = ChequeStatus;

        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            $(this).closest('tr').find('.ChequeStatus').prop("disabled", false);
            $(this).closest('tr').find('.BankCharges').prop("disabled", false);

            $(this).closest('tr').find('.Remarks').prop("disabled", false);
            $(this).closest('tr').find('.InstrumentDate').prop("readonly", true);
        } else {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').attr("disabled", true);
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').find('.ChequeStatus').prop("disabled", true);
            $(this).closest('tr').find('.BankCharges').prop("disabled", true);

            $(this).closest('tr').find('.Remarks').prop("disabled", true);
        }
        self.count_items();
    },
    show: function () {
        var self = ChequeStatus;
        self.error_count = 0;
        self.error_count = self.validate_Item();
        if (self.error_count > 0) {
            return;
        }
        if ($("#cheque-status-item-list tbody tr").length > 0) {
            app.confirm_cancel("Please save selected Items.Do you want to remove it?", function () {
                $('#cheque-status-item-list tbody').empty();
                self.get_Cheque_Status();
            },
            function () {

            });

        }
        else {
            self.get_Cheque_Status();
        }
    },
    validate_Item: function () {
        var self = ChequeStatus;
        if (self.rules.on_show.length > 0) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },
    rules: {
        on_show: [

                {
                    elements: "#InstrumentStatusID",
                    rules: [
                         { type: form.required, message: "Please Select Instrument Status" },

                    ]
                },
                 {
                     elements: "#ReceiptDateFrom",
                     rules: [
                          { type: form.required, message: "Please Select From ReceiptDate" },

                     ]
                 },
                  {
                      elements: "#ReceiptDateTo",
                      rules: [
                           { type: form.required, message: "Please Select To ReceiptDate" },

                      ]
                  },
        ],
        on_submit: [
            {
                elements: "#InstrumentStatusID",
                rules: [
                    { type: form.required, message: "Please Select InstrumentStatus" },

                ]
            },
            {
                elements: "#ReceiptDateFrom",
                rules: [
                    { type: form.required, message: "Please Select From ReceiptDate" },

                ]
            },
            {
                elements: "#ReceiptDateTo",
                rules: [
                    { type: form.required, message: "Please Select To ReceiptDate" },

                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: ".ChequeStatus.included",
                rules: [
                {
                    type: function (element) {
                        var error = false;
                        $('.ChequeStatus.included').each(function () {
                            var row = $(this).closest('tr');
                            var chequestatus = $(row).find('.ChequeStatus').val();
                            var instrumentstatus = $('#InstrumentStatusID').val();
                            if (chequestatus == instrumentstatus)
                                error = true;

                        });
                        return !error;
                    }, message: 'Please change Cheque status'
                }
                ]
            },
            {
                elements: ".Deposited.included",
                rules: [
                    {
                        type: function (element) {
                            return form.past_date($(element).closest("tr").find(".InstrumentDate"));
                        }, message: 'Please Check Instrument Date '
                    }
                ]
            },


             {
                 elements: ".StatusChangeDate.included",
                 rules: [
             { type: form.required, message: "StatusChangeDate is required" },
             {
                 type: function (element) {
                     var StatusChangeDate = $(element).val().split('-');
                     var used_date = new Date(StatusChangeDate[2], StatusChangeDate[1] - 1, StatusChangeDate[0]);
                     var a = Date.parse(used_date);
                     var row = $(element).closest('tr');
                     var ChequeReceivedDate = $(row).find('.ChequeReceivedDate').val().split('-');
                     var ChequeReceived_datesplit = new Date(ChequeReceivedDate[2], ChequeReceivedDate[1] - 1, ChequeReceivedDate[0]);
                     var date = Date.parse(ChequeReceived_datesplit);
                     return date <= a
                 }, message: "StatusChangeDate should be a date on or after Date of InstrumentDate"
             }

                 ]
             },

            {
                elements: ".ChargesToCustomer.included",
                rules: [

                        { type: form.positive, message: "Please enter positive value" },

                ]
            }
        ],
        on_draft: [
            {
                elements: "#InstrumentStatusID",
                rules: [
                    { type: form.required, message: "Please Select InstrumentStatus" },

                ]
            },


            {
                elements: ".StatusChangeDate.included",
                rules: [
            { type: form.required, message: "StatusChangeDate is required" },
            {
                type: function (element) {
                    var StatusChangeDate = $(element).val().split('-');
                    var used_date = new Date(StatusChangeDate[2], StatusChangeDate[1] - 1, StatusChangeDate[0]);
                    var a = Date.parse(used_date);
                    var row = $(element).closest('tr');
                    var ChequeReceivedDate = $(row).find('.ChequeReceivedDate').val().split('-');
                    var ChequeReceived_datesplit = new Date(ChequeReceivedDate[2], ChequeReceivedDate[1] - 1, ChequeReceivedDate[0]);
                    var date = Date.parse(ChequeReceived_datesplit);
                    return date <= a
                }, message: "StatusChangeDate should be a date on or after Date of InstrumentDate"
            }

                ]
            },


            {
                elements: "#ReceiptDateFrom",
                rules: [
                    { type: form.required, message: "Please Select From ReceiptDate" },

                ]
            },
            {
                elements: "#ReceiptDateTo",
                rules: [
                    { type: form.required, message: "Please Select To ReceiptDate" },

                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: ".ChequeStatus.included",
                rules: [
                {
                    type: function (element) {
                        var error = false;
                        $('.ChequeStatus.included').each(function () {
                            var row = $(this).closest('tr');
                            var chequestatus = $(row).find('.ChequeStatus').val();
                            var instrumentstatus = $('#InstrumentStatusID').val();
                            if (chequestatus == instrumentstatus)
                                error = true;

                        });
                        return !error;
                    }, message: 'Please change Cheque status'
                }
                ]
            },
        ]
    },
    get_Cheque_Status: function () {
        var self = ChequeStatus;
        var InstrumentStatusID = $("#InstrumentStatusID").val();
        $.ajax({
            url: '/Accounts/ChequeStatus/getChequeStatus',
            data: {
                InstrumentStatus: $("#InstrumentStatusID").val(),
                FromDate: $("#ReceiptDateFrom").val(),
                TODate: $("#ReceiptDateTo").val(),

            },
            dataType: "json",
            type: "POST",
            success: function (bookentries) {
                var Status = $('#StatusTemplate' + InstrumentStatusID).html();
                var $status_list = $('#cheque-status-item-list tbody');
                $status_list.html('');
                var tr = '';
                var $tr;
                $.each(bookentries, function (i, bookentry) {
                    var SerialNo = $("#cheque-status-item-list tbody tr").length + 1;
                    var datarelaxation = -(parseInt(app.get_date_diff(bookentry.ChequeReceivedDate)));
                    tr = "<tr >"
                                + '<td class="uk-text-center">' + SerialNo + '</td>'
                                + '<td class="uk-text-center checked width-20" data-md-icheck><input type="checkbox" class="chkCheck" />'
                                + ' <input type="hidden" class="VoucherNo" value="' + bookentry.VoucherNo + '" />'
                                + ' <input type="hidden" class="VoucherID" value="' + bookentry.VoucherID + '" /></td>'
                                + ' <input type="hidden" class="ChequeReceivedDate" value="' + bookentry.ChequeReceivedDate + '" /></td>'
                                + '<td class="InstrumentNumber width-80">' + bookentry.InstrumentNumber + '</td>'
                                + '<td><input type="text" class="md-input InstrumentDate width-80" readonly value="' + bookentry.InstrumentDate + '"/></td>'
                                + "<td class><select  class='md-input width-100 ChequeStatus' disabled>" + Status + "</select></td>"
                                + '<td><input type="text" class="md-input uk-icon-calendar past-date StatusChangeDate" data-relaxation="' + datarelaxation + '" disabled="disabled" value="' + $("#Date").val() + '"  /></td>'
                                + '<td class="Customer ">' + bookentry.CustomerName + '</td>'
                                + ' <input type="hidden" class="CustomerID" value="' + bookentry.CustomerID + '" />'
                                + ' <input type="hidden" class="StateID" value="' + bookentry.StateID + '" /></td>'
                                + '<td class="InstrumentAmount width-80 mask-currency">' + bookentry.InstrumentAmount + '></td>'
                                + '<td ><input type="text"  class="md-input width-80 ChargesToCustomer mask-positive-currency included" disabled="disabled" value="' + bookentry.ChargesToCustomer + '"/></td>'
                                + '<td class="width-60 CGST mask-currency included">' + bookentry.CGST + '</td>'
                                + '<td class="width-60 SGST mask-currency">' + bookentry.SGST + '</td>'
                                + '<td class="width-60 IGST mask-currency">' + bookentry.IGST + '</td>'
                                + '<td ><input type="text"  class="md-input width-120 Remarks included" value="' + bookentry.Remarks + '" disabled/></td>'
                                + "</tr>";
                    $tr = $(tr);
                    $tr.find(".ChequeStatus").val(InstrumentStatusID);
                    app.format($tr);
                    $status_list.append($tr);
                    self.count_items();
                });
                $("#ReceiptDateFrom,#ReceiptDateTo").removeClass('md-input-danger');
                $("#ReceiptDateFrom,#ReceiptDateTo").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
            }
        });
    },

    on_save: function () {

        var self = ChequeStatus;
        var data = self.get_data();
        var location = "/Accounts/ChequeStatus/Index";
        var url = '/Accounts/ChequeStatus/Save';

        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Accounts/ChequeStatus/SaveAsDraft'
           // self.error_count = self.validate_form();
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
        var self = ChequeStatus;
        if ($("#cheque-status-item-list tbody tr").length > 0) {
          //  var model = self.get_data(IsDraft);
            $(" .btnSaveAsDraft, .btnSave").css({ 'display': 'none' });
            $.ajax({
                url: url,
                data: { model: data },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data == "success") {
                        app.show_notice("Cheque Status saved successfully");
                        setTimeout(function () {
                            window.location = location;
                        }, 1000);
                    } else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        $(".btnSaveAsDraft, .btnSave ").css({ 'display': 'block' });
                    }
                },
            });
        } else {
            app.show_error("No Data Found");
        }
    },
    submit: function () {
        var self = ChequeStatus;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(false);
        }
    },
    save_as_draft: function () {
        var self = ChequeStatus;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(true);
        }
    },
    validate_form: function () {
        var self = ChequeStatus;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    get_data: function (IsDraft) {
        var self = ChequeStatus;
        var model = {
            ID: $("#ID").val(),
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            ReceiptDateFrom: $("#ReceiptDateFrom").val(),
            ReceiptDateTo: $("#ReceiptDateTo").val(),
            InstrumentStatus: $("#InstrumentStatusID").val(),
            IsDraft: IsDraft
        };
        model.Items = self.GetProductList();

        return model;
    },
    GetProductList: function () {
        var ProductsArray = [];
        var row;
        var ProductsArray = [];
        $("#cheque-status-item-list tbody tr.included").each(function () {
            row = $(this);
            var InstrumentNumber = $(row).find('.InstrumentNumber').text();
            var InstrumentDate = $(row).find('.InstrumentDate').val();
            var CustomerID = $(row).find('.CustomerID').val();
            var ChequeStatus = $(row).find('.ChequeStatus').val();
            var StatusChangeDate = $(row).find('.StatusChangeDate').val();
            var InstrumentAmount = clean($(row).find('.InstrumentAmount').val());
            var BankCharges = clean($(row).find('.BankCharges').val());
            var TotalAmount = clean($(row).find('.TotalAmount').val());
            var ChargesToCustomer = clean($(row).find('.ChargesToCustomer').val());
            var Remarks = $(row).find('.Remarks').val();
            var IsActive = true;
            var VoucherNo = $(row).find('.VoucherNo').val();
            var VoucherID = $(row).find('.VoucherID').val();
            var CGST = $(row).find('.CGST').val();
            var SGST = $(row).find('.SGST').val();
            var IGST = $(row).find('.IGST').val();
            ProductsArray.push({
                InstrumentNumber: InstrumentNumber,
                InstrumentDate: InstrumentDate,
                CustomerID: CustomerID,
                ChequeStatus: ChequeStatus,
                StatusChangeDate: StatusChangeDate,
                InstrumentAmount: InstrumentAmount,
                BankCharges: BankCharges,
                TotalAmount: TotalAmount,
                ChargesToCustomer: ChargesToCustomer,
                Remarks: Remarks,
                IsActive: IsActive,
                VoucherNo: VoucherNo,
                VoucherID: VoucherID,
                CGST: CGST,
                SGST: SGST,
                IGST: IGST
            });
        })
        return ProductsArray;
    },
    set_previous_status: function () {
        previous_status = $("#InstrumentStatusID").val();
    },
    remove_data: function () {
        var self = ChequeStatus;
        var status = $("#InstrumentStatusID").val();
        if ($("#cheque-status-item-list tbody tr").length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                $('#cheque-status-item-list tbody').empty();
            }, function () {
                $("#InstrumentStatusID").val(previous_status);
            });
        }


    },
    //change_check_status: function () {
    //    var self = ChequeStatus;
    //    var SearchedStatus = $("#InstrumentStatusID").val();

    //    $("#cheque-status-item-list tbody tr.included").each(function () {
    //        if (SearchedStatus == $(this).find('.ChequeStatus').val()) {
    //            $(this).removeClass("changed");
    //        } else {
    //            $(this).addClass("changed");
    //        }
    //    });
    //    self.count_items();
    //},
    count_items: function () {
        $("#item-count").val($('#cheque-status-item-list tr.included').length)
    },
    update_customer_charge: function () {
        var row = $(this).closest('tr');
        var CustomerStateID = $(row).find('.StateID').val();
        var UserStateID = $("#UserStateID").val();
        var CGSTpercent = 0.00, SGSTpercent = 0.00, IGSTpercent = 0.00, amount = 0.00;
        var ChargesToCustomer = clean($(row).find('.ChargesToCustomer').val());

        IGSTpercent = clean($('#IGST').val());
        CGSTpercent = clean($('#CGST').val());
        SGSTpercent = clean($('#SGST').val());

        if (CustomerStateID == UserStateID) {
            amount =ChargesToCustomer * (CGSTpercent / (100 + CGSTpercent + SGSTpercent));
            $(row).find('.CGST').val(amount);
            $(row).find('.SGST').val(amount);

        } else {
            amount = ChargesToCustomer * (IGSTpercent / (100 + IGSTpercent));
            $(row).find('.IGST').val(amount);
        }
    }
}


