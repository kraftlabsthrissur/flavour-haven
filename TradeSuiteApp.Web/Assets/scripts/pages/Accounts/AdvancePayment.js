var temp;
var select_table;
var employee_select_table;
var freeze_header;
var is_first_run = true;
advance_payment = {
    init: function () {
        var self = advance_payment;

        freeze_header = $('#Advance-Details').FreezeHeader();

        supplier.supplier_list('Payment');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            modal: '#select-supplier',
            returnFocus: "#dropPayment",
            initiatingElement: "#SupplierName"
        });
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        if ($("#ID").val() > 0) {
            var supplierID = $("#SupplierID").val();
            var employeeID = $("#EmployeeID").val();
            if ($("#Category").val() == "Supplier") {
                self.get_purchase_orders_for_edit(supplierID);
            }
            else {
                self.get_Advannce_request_edit();
            }
        }
        self.bind_events();
    },

    details: function () {
        var self = advance_payment;
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        freeze_header = $('#Advance-Details').FreezeHeader();
    },

    print: function () {
        var self = advance_payment;
        $.ajax({
            url: '/Accounts/AdvancePayment/Print',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    SignalRClient.print.text_file(data.URL);
                } else {
                    app.show_error("Failed to print");
                }
            },
        });
    },

    printpdf: function () {
        var self = advance_payment;
        $.ajax({
            url: '/Reports/Accounts/AdvancePaymentPrintPdf',
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
        var self = advance_payment;
        $('#tabs-AdvancePayment').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "advance-payment":
                $list = $('#advance-payment-list');
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

            var url = "/Accounts/AdvancePayment/GetAdvancePaymentListForDataTable?type=" + type;
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

                    { "data": "AdvancePaymentNo", "className": "AdvancePaymentNo" },
                    { "data": "AdvancePaymentDate", "className": "AdvancePaymentDate" },
                    { "data": "Category", "className": "Category" },
                    { "data": "Name", "className": "Name" },
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
                        app.load_content("/Accounts/AdvancePayment/Details/" + Id);

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
        var self = advance_payment;

        $("#ddlCategory").on('change', self.select_purpose);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employees, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $("#btnOKEmployee").on("click", self.select_employee);
        $('.btnSaveAndPost,.btnSaveASDraft').on('click', self.on_save);
        $("#dropPayment").on("change", self.get_bank_name);
        $("#btnOKSupplier").on("click", self.select_supplier);
        $(".radPurpose").on("ifChecked", self.get_employee_advance);
        $('body').on('keyup change', '#Advance-Details .txtAmount', self.calculateTDSAmount);
        $('body').on('click', "#btnAddPayment", self.process_item);
    },

    process_item: function () {
        var self = advance_payment;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }

        var advance_amount = clean($("#AdvanceAmount").val());
        var row;
        var obj = {};
        self.settlements = [];

        $("#Advance-Details tbody .advancereceived").remove();

        if (advance_amount > 0) {
            var nowDate = new Date();
            advance_amount = (advance_amount).roundTo(2);
            var nowDay = ((nowDate.getDate().toString().length) == 1) ? '0' + (nowDate.getDate()) : (nowDate.getDate());
            var nowMonth = nowDate.getMonth() < 9 ? '0' + (nowDate.getMonth() + 1) : (nowDate.getMonth() + 1);
            var nowYear = nowDate.getFullYear();
            var formattedDate = nowDay + "-" + nowMonth + "-" + nowYear;
            var content = "";
            var $content;
            content = '<tr class="included advancereceived">'
                + '<td class="serial-no uk-text-center width-20">' + ($('#Advance-Details tbody tr').length + 1) + '</td>'
                + '<td class="transNo">' + 'ADVANCE PAYMENT'
                + '<input type="hidden" class="hdnPOID" value="0"/><input type="hidden" id="0" class="hdnItemID"/>'
                + '</td>'
                + '<td class="podate">' + formattedDate + '</td>'
                + '<td class="poTerms"></td>'
                + '<td class="mask-currency poAdvance"></td>'
                + '<td class="poItem_value_ItemName"></td>'
                + '<td class="mask-currency txtAmount">' + advance_amount + '</td>'
                + '<td class="ddlTDSCode uk-hidden"></td>'
                + '<td class="mask-currency uk-hidden">' + '0.00' + '</td>'
                + '<td><input type="text" class="md-input txtNetAmount  mask-qty" disabled="disabled" value="' + advance_amount + '"  /> </td>'
                + '<td><input type="text" class="md-input txtRemarks" /></td>'
                + '</tr>';
            $content = $(content);
            app.format($content);
            $('#Advance-Details tbody').append($content);
        }
        self.calculateTotals();
        $("#AdvanceAmount").val(0);
    },
    on_save: function () {

        var self = advance_payment;
        var IsDraft = false;

        var location = "/Accounts/AdvancePayment/Index";
        var url = '/Accounts/AdvancePayment/Save';

        if ($(this).hasClass("btnSaveASDraft")) {
            IsDraft = true;
            url = '/Accounts/AdvancePayment/SaveAsDraft'
            self.error_count = self.validateForm();
        } else {
            self.error_count = self.validateForm();

        }

        if (self.error_count > 0) {
            return;
        }

        if (!IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(IsDraft, url, location);
            }, function () {
            })
        } else {
            self.save(IsDraft, url, location);
        }
    },

    get_employee_advance: function () {
        //if (is_first_run) {
        //    is_first_run = false;
        //    setTimeout(function () { is_first_run = true }, 500);
        //} else {
        //    return;
        //}
        var self = advance_payment;

        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm_cancel("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);
                self.get_Advannce_request();
            },
                function () {
                    var purpose = $('input.radPurpose:not(:checked)').val();
                    var current_purpose = $('input.radPurpose:checked').val();
                    $('#' + purpose + '').prop("checked", true)
                    $('#' + purpose + '').closest('div').addClass("checked")
                    $('#' + current_purpose + '').prop("checked", false)
                    $('#' + current_purpose + '').closest('div').removeClass("checked")
                })
        }
        else {

            self.get_Advannce_request();
        }

    },
    calculateTDSAmount: function () {
        var self = advance_payment;

        if ($("#ddlCategory").val() == "Supplier") {
            var parenttr = $(this).parents('tr');
            var Code = parenttr.find('.ddlTDSCode').val();
            var percentage = Code.split("#");
            var tdsAmt = 0;

            var amt = clean(parenttr.find('.txtAmount').val());
            var netamt = 0;

            if ($(this).val() != '') {
                var thisAmount = parseFloat($(this).val());
                tdsAmt = clean(self.getCalculatedTDSAmt(clean($(this).val()), clean(percentage[1])));
                if (tdsAmt != 0) {
                    // var amount = tdsAmt;

                    tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) != 0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
                }
                netamt = amt - tdsAmt;
            }
            else
                tdsAmt = 0;

            parenttr.find('.txtTDSAmount').val(tdsAmt);
            parenttr.find('.txtNetAmount').val(netamt);
        }
        else {
            var parenttr = $(this).parents('tr');
            var amt = clean(parenttr.find('.txtAmount').val());
            parenttr.find('.txtNetAmount').val(amt);


        }
        self.calculateTotals();

    },
    get_employees: function (release) {
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
    set_employee: function (event, item) {
        var self = advance_payment;
        console.log(item)
        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);

                var employeeId = item.id;
                $("#EmployeeID").val(item.id),
                    $("#Name").val(item.value);
                $("#Code").val(item.code);
                $("#Place").val(item.place);
                $("#hdnName").val(item.value);
                $("#hdnNameID").val(item.id);

                self.get_Advannce_request();
            })
        }
        else {
            $("#EmployeeID").val(item.id),
                $("#Name").val(item.value);
            $("#Code").val(item.code);
            $("#Place").val(item.place);
            $("#hdnName").val(item.value);
            $("#hdnNameID").val(item.id);
            self.get_Advannce_request();
        }

    },
    get_Advannce_request_edit: function () {
        var self = advance_payment;
        var ID = clean($('#ID').val());

        var callBack = function (response) {
            $('#Advance-Details tfoot .totalAmount').val(0);
            $('#totalTDSAmount').val(0);
            var $response = $(response);
            app.format($response);
            $('#Advance-Details tbody').html($response);
            freeze_header.resizeHeader();
            self.calculate_total();
        };
        $.ajax({
            url: '/Accounts/AdvancePayment/GetAdvanceRequestForEdit',
            data: {
                ID: ID

            },
            method: "GET",
            success: function (successResponse) {
                if (callBack != null && callBack != undefined)
                    callBack(successResponse);
            },
        });

    },
    get_Advannce_request: function () {
        var self = advance_payment;
        var EmployeeID = clean($('#EmployeeID').val());
        var IsOfficial = $('.radPurpose:checked').val() == "Official" ? 1 : 0;
        var callBack = function (response) {
            $('#Advance-Details tfoot .totalAmount').val(0);
            $('#totalTDSAmount').val(0);
            var $response = $(response);
            app.format($response);
            $('#Advance-Details tbody').html($response);
            freeze_header.resizeHeader();
            self.calculate_total();
        };
        $.ajax({
            url: '/Accounts/AdvancePayment/GetAdvanceRequest',
            data: {
                EmployeeID: EmployeeID,
                IsOfficial: IsOfficial
            },
            method: "GET",
            success: function (successResponse) {
                if (callBack != null && callBack != undefined)
                    callBack(successResponse);
            },
        });

    },
    calculate_total: function () {
        var total = 0;
        var tdstotal = 0;
        $('#Advance-Details tbody .txtAmount').each(function () {
            if (clean($(this).val()) > 0) {
                total += clean($(this).val());
            }
        });
        $('#Advance-Details tbody .txtTDSAmount').each(function () {
            if (clean($(this).val()) > 0) {

                tdstotal += clean($(this).val());
            }
            else {
                tdstotal += 0;
            }
        });
        $('#Advance-Details tfoot .totalAmount').val(total);
        // $('#Advance-Details tfoot .amount-hidden').val(total);
        $('#Advance-Details tfoot .totalNetAmount').val(total);
        $('#NetAmount').val(total);
        $('#totalTDSAmount').val(tdstotal);
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetPaymentSupplierForAutoComplete',
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

        self = advance_payment;
        $("#SupplierName").val(item.name);
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#hdnName").val(item.name);
        $("#hdnNameID").val(item.id);
        $("#SupplierID").val(item.id);
        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);

                self.get_purchase_orders(item.id);
            })
        }
        else {
            self.get_purchase_orders(item.id);
        }

    },
    get_item_details: function (release) {
        var hint = $(this.input).val();
        var PurchaseID = $(this.input).parents('tr').find('.hdnPOID').val();
        var TransNo = $(this.input).parents('tr').find('.transNo').text().trim();
        $.ajax({
            url: '/AdvancePayment/GetItemNamesByPurchaseOrder',
            data: {
                search: hint,
                TransNo: TransNo,
                PurchaseID: PurchaseID,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, item) {   // on select auto complete item


        var code = event.currentTarget.id;
        split = code.split("autocomplete");

        id = clean(split[1]);
        $("#txtItemName").val(item.label);
        $("#hdnItemID" + id).val(item.id);
    },

    get_bank_name: function () {
        var self = advance_payment;
        var mode;
        var Module = "Payment"
        if ($("#dropPayment option:selected").text() == "Select") {
            mode = "";
        }
        else if ($("#dropPayment option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }
        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                ModuleName: Module,
                Mode: mode

            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#Bank").html("");
                var html = "";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'  data-creditBalance='" + record.CreditBalance + "'>" + record.BankName + "</option>";
                });
                $("#Bank").append(html);
            }
        });

    },
    get_purchase_orders_for_edit: function () {
        var self = advance_payment;
        var callBack = function (response) {
            $('#Advance-Details tfoot .totalAmount').val(0);
            var $response = $(response);
            app.format($response);
            $('#Advance-Details tbody').html($response);
            self.calculateTotals();
            freeze_header.resizeHeader();
            $('#Advance-Details tbody tr').each(function (i) {
                $.UIkit.autocomplete($('#item-autocomplete' + (i + 1)), { 'source': self.get_item_details, 'minLength': 1 });
                $('#item-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', self.set_item_details);
            });
            var code;
            $('#Advance-Details tbody .ddlTDSCode').on('change', function () {
                code = $(this).val();
                var percentage = code.split("#");
                var parenttr = $(this).parents('tr');
                var amount = clean(parenttr.find('.txtAmount').val());
                var netamount;
                var tdsAmt = clean(self.getCalculatedTDSAmt(amount, clean(percentage[1])));
                parenttr.find('.txtTDSAmount').val(tdsAmt);
                if (tdsAmt != 0) {

                    tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) != 0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
                }
                netamt = amount - tdsAmt;
                parenttr.find('.txtNetAmount').val(netamt.toFixed(2));
                parenttr.find('.txtTDSAmount').val(tdsAmt);

                self.calculateTotals();
            });
            var currentPOID = 0;
        };
        var ID = $('#ID').val();
        this.ajaxRequest('/Accounts/AdvancePayment/GetPurchaseOrdersAdvancePaymentForEdit',
            {
                ID: ID

            }, "GET", callBack);
    },

    get_purchase_orders: function (SupplierID) {
        var self = advance_payment;
        var callBack = function (response) {
            $('#Advance-Details tfoot .totalAmount').val(0);
            var $response = $(response);
            app.format($response);
            $('#Advance-Details tbody').html($response);
            self.calculateTotals();
            freeze_header.resizeHeader();
            $('#Advance-Details tbody tr').each(function (i) {
                $.UIkit.autocomplete($('#item-autocomplete' + (i + 1)), { 'source': self.get_item_details, 'minLength': 1 });
                $('#item-autocomplete' + (i + 1)).on('selectitem.uk.autocomplete', self.set_item_details);
            });
            var code;
            $('#Advance-Details tbody .ddlTDSCode').on('change', function () {
                code = $(this).val();
                var percentage = code.split("#");
                var parenttr = $(this).parents('tr');
                var amount = clean(parenttr.find('.txtAmount').val());
                var netamount;
                var tdsAmt = clean(self.getCalculatedTDSAmt(amount, clean(percentage[1])));
                parenttr.find('.txtTDSAmount').val(tdsAmt);
                if (tdsAmt != 0) {

                    tdsAmt = (((tdsAmt % 1) < 1) && ((tdsAmt % 1) != 0)) ? ((tdsAmt - (tdsAmt % 1)) + 1) : (tdsAmt);
                }
                netamt = amount - tdsAmt;
                parenttr.find('.txtNetAmount').val(netamt.toFixed(2));
                parenttr.find('.txtTDSAmount').val(tdsAmt);

                self.calculateTotals();
            });
            var currentPOID = 0;
        };
        var Category = $('#ddlCategory').val();
        this.ajaxRequest('/Accounts/AdvancePayment/GetPurchaseOrders',
            {
                Category: Category,
                SupplierID: SupplierID
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
    getCalculatedTDSAmt: function (amt, percentage) {

        console.log(amt + "   " + percentage);
        if (amt == null || amt == undefined)
            amt = '0';
        if (amt == '' || amt.length <= 0) { amt = '0' }
        if (percentage != '') {
            return ((parseFloat(amt) / parseFloat(100)) * parseFloat(percentage)).toFixed(2);
        }
        else
            $('#txtTDSAmount').val(amt);

    },
    select_purpose: function () {
        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);

            })
        }
        var select_category = $(this).val();
        $("#EmployeeName").val('');
        $("#SupplierName").val('');
        $("#select_purpose").toggleClass('uk-hidden');
        $("#EmployeeWrapper").toggleClass('uk-hidden').toggleClass('selected');
        $("#SupplierWrapper").toggleClass('uk-hidden').toggleClass('selected');


    },
    select_supplier: function (event, SupplierList) {

        var self = advance_payment;

        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        $("#SupplierName").val(Name);
        $("#hdnNameID").val(ID);
        $("#SupplierID").val(ID);
        $("#hdnName").val(Name);
        UIkit.modal($('#select-supplier')).hide();
        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);

                self.get_purchase_orders(ID);
            })
        }
        else {
            self.get_purchase_orders(ID);
        }

    },
    select_employee: function (event, EmployeeList) {

        var self = advance_payment;

        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var Department = $(row).find(".Department").val();


        UIkit.modal($('#select-employee')).hide();
        if (($("#Advance-Details tbody tr").length > 0)) {
            app.confirm("Selected Items will be removed", function () {
                $('#Advance-Details tbody').empty();
                $('#Advance-Details tfoot .totalAmount').val(0.0);
                $('#Advance-Details tfoot .totalNetAmount').val(0.0);
                $('#NetAmount').val(0.0);

                $("#EmployeeID").val(ID),
                    $("#Name").val(Name);
                $("#Place").val(Location);
                $("#hdnName").val(Name);
                $("#hdnNameID").val(ID);
                $("#EmployeeName").val(Name);
                self.get_Advannce_request();
            })
        }
        else {

            $("#EmployeeID").val(ID),
                $("#Name").val(Name);
            $("#Place").val(Location);
            $("#hdnName").val(Name);
            $("#hdnNameID").val(ID);
            $("#EmployeeName").val(Name);
            self.get_Advannce_request();
        }
    },

    save: function (IsDraft, url, location) {

        self = advance_payment;
        var purpose = $('input.radPurpose:checked').val();
        if (purpose == undefined)
            purpose = "";
        var pos = [];
        var obj = {
            ModeOfPayment: '',
            BankID: 0,
            //BankAccNo: '',
            AccountNo: '',
            ReferenceNo: '',
            SelectedID: 0,
            SelectedName: '',
            SaveType: '',
            Purpose: '',
            PurchaseOrders: {},
            Category: '',
        };
        obj.AdvancePaymentNo = $('#txtAdvancePaymentNo').val();
        obj.ModeOfPaymentID = $('#dropPayment').val();
        if ($('#Bank option:selected').text() == "Select") {
            obj.BankName = "";
        }
        else {
            obj.BankID = $('#Bank option:selected').val();
            obj.BankDetail = $('#Bank option:selected').text();
        }
        obj.ID = $("#ID").val();
        obj.SelectedID = $('#hdnNameID').val();     //SupplierID or  EmployeeID
        obj.SelectedName = $('#hdnName').val();     //SupplierName or  EmployeeName
        obj.AccountNo = $('#txtAccountNumber').val();
        obj.ReferenceNo = $('#txtReferenceNumber').val();
        obj.AdvancePaymentDateStr = $('#AdvancePaymentDate').val();
        obj.Purpose = purpose;
        obj.Category = $('#ddlCategory').val();
        obj.SupplierID = $('#SupplierID').val();
        obj.EmployeeID = $('#EmployeeID').val();
        obj.SaveType = '';
        obj.Amt = clean($('#Advance-Details tfoot .totalAmount').val());
        obj.NetAmount = clean($("#NetAmount").val());
        obj.Draft = IsDraft;
        var code, split, id;
        $('#unProcessedItemsTblContainer table tbody tr').each(function () {
            var currRow = $(this);
            var po = {};
            po.PurchaseOrderID = currRow.find('.hdnPOID').val();
            po.TransNo = currRow.find('.transNo').text().trim();
            po.ItemID = currRow.find('.hdnItemID').val();
            po.ItemName = currRow.find('.txtItemName').text();
            po.Amount = clean(currRow.find('.txtAmount').val());
            code = currRow.find('.ddlTDSCode').val();
            po.Advance = clean(currRow.find('.poAdvance').val());
            id = 0;
            if ($('#ddlCategory option:selected').text() == "Supplier") {
                split = code.split("#");
                id = clean(split[0]);
                po.TDSID = id;
            }
            else {
                po.TDSID = 0;
            }
            po.TDSAmount = clean(currRow.find('.txtTDSAmount').val());
            po.Remarks = currRow.find('.txtRemarks').val();
            po.PurchaseOrderDateStr = currRow.find('.podate').text();
            po.PurchaseOrderTerms = currRow.find('.poTerms').text();
            pos.push(po);
        });

        $(".btnSaveASDraft, .btnSaveAndPost").css({ 'display': 'none' });
        var count = 0;
        $('#unProcessedItemsTblContainer table tbody tr').each(function () {
            var currRow = $(this);
            var tdsamount = clean(currRow.find('.txtTDSAmount').val());
            var amount = clean(currRow.find('.txtAmount').val());
            if ((amount > 0) && ((tdsamount == undefined) || (tdsamount == 0)))
                count++;
        });
        if (count > 0 && IsDraft == 0) {
            /*app.confirm("Do you want to create voucher without TDS?", function () {*/
                $(".btnSaveAndPost,.btnSaveASDraft").css({ 'display': 'none' });
                $.ajax({
                    url: url,
                    data: {
                        model: obj,
                        trans: pos
                    },
                    method: "POST",
                    success: function (data) {
                        if (data.Status == "success") {
                            app.show_notice(data.Message);
                            window.location = location;
                        }
                        else {
                            if (typeof data.data[0].ErrorMessage != "undefined")
                                app.show_error(data.data[0].ErrorMessage);
                            $(".btnSaveAndPost,.btnSaveASDraft").css({ 'display': 'block' });
                        }
                    }
                });
           /* })*/
        }

        else {
            $(".btnSaveAndPost,.btnSaveASDraft").css({ 'display': 'none' });
            $.ajax({
                url: url,
                data: {
                    model: obj,
                    trans: pos
                },
                method: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice(data.Message);
                        window.location = location;
                    }
                    else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        $(".btnSaveAndPost,.btnSaveASDraft").css({ 'display': 'block' });
                    }
                }
            });
        }
    },
    validate_item: function () {
        var self = advance_payment;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validateForm: function () {
        var self = advance_payment;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_blur: [],
        on_add: [
            {
                elements: "#AdvanceAmount",
                rules: [
                    { type: form.positive, message: "Please enter valid advance amount" },
                ]
            },
        ],
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
                elements: ".txtAmount",
                rules: [
                    //{
                    //    type: function (element) {
                    //        var error = false;
                    //        if ($("#ddlCategory").val() == "Supplier") {
                    //            var row = $(element).closest("tr");
                    //            return clean($(row).find(".balance-amount").val()) >= clean($(element).val());
                    //        }
                    //        return !error;


                    //    }, message: "Advance amount does not exceed PurchaseOrder amount "
                    //},
                    {
                        type: function (element) {
                            var error = false;
                            if ($("#ddlCategory").val() == "Employee") {
                                var row = $(element).closest("tr");
                                return clean($(row).find(".request-amount").val()) >= clean($(element).val());
                            }
                            return !error;


                        }, message: "Advance amount does not exceed Requested amount "
                    },

                ]
            },


            {
                elements: "#dropPayment",
                rules: [
                    { type: form.required, message: "Please choose a mode of payment" },
                    {
                        type: function (element) {
                            var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                            var NetAmt = clean($("#NetAmount").val());
                            var PaymentMode = $("#dropPayment Option:Selected ").text().toUpperCase();
                            var error = false;
                            if (CashPaymentLimit < NetAmt && PaymentMode == 'CASH') {
                                error = true;
                            }
                            return !error;
                        }, message: 'Please choose Another Payment Mode'
                    },
                ]
            },

            //{
            //    elements: ".totalNetAmount",
            //    rules: [

            //    {
            //        type: function (element) {
            //            var CreditLimit = $("#Bank option:selected").data('creditbalance');
            //            CreditLimit = typeof CreditLimit === "undefined" ? 0 : CreditLimit;
            //            var NetAmt = clean($(".totalNetAmount").val());

            //            return CreditLimit >= NetAmt;
            //        }, message: 'Total exceeds credit limit'
            //    },
            //    ]
            //},
            {

                elements: ".selected #SupplierName",
                rules: [
                    { type: form.required, message: "Please choose supplier name" },
                ]
            },
            {
                elements: ".selected #EmployeeName",
                rules: [
                    { type: form.required, message: "Please choose employee name" },
                ]
            },
            {
                elements: "#Bank",
                rules: [
                    { type: form.non_zero, message: "Please choose Bank Name" },
                ]
            }
        ]

    },
    ajaxRequest: function (url, data, requestType, callBack) {
        $.ajax({
            url: url,
            cache: false,
            type: requestType,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            //traditional: true,
            data: data,
            success: function (successResponse) {
                //console.log('successResponse');
                if (callBack != null && callBack != undefined)
                    callBack(successResponse);
            },
            error: function (errResponse) {//Error Occured 
                //console.log(errResponse);
            }
        });
    }
}