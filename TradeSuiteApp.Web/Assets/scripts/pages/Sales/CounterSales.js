var POL_LOCATIONID = 2;
var fh_items;
var normalclass = '';
$(function () {
    var self = CounterSales;
    normalclass = $("#normalclass").val();
    DecPlaces = clean($("#DecimalPlaces").val());
    self.calculate_balance();

});
var CounterSales = {

    init: function () {
        var self = CounterSales;
        Employee.employee_list('FreeMedicineEmployeeList');
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });

        supplier.Doctor_list();
        $('#doctor-list').SelectTable({
            selectFunction: self.select_doctor,
            returnFocus: "#ItemName",
            modal: "#select-doctor",
            initiatingElement: "#DoctorName"
        });
        Patient.patient_list();
        $('#patient-list').SelectTable({
            selectFunction: self.select_patient,
            returnFocus: "#PartyName",
            modal: "#select-patient",
            initiatingElement: "#PartyName"
        });



        item_list = Item.item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        if ($("#ID").val() > 0) {
            var LocationID = $("#current-location select").val();
            if ($("#TypeID option:selected").text().toLowerCase() == "employee" && LocationID == POL_LOCATIONID) {
                $("#BatchTypeID").val(1);
            }
        }

        self.hide_show_sales_type();
        self.bind_events();
        self.hide_show_elements();
        self.calculate_balance();
        setTimeout(function () {
            fh_items = $("#counter-sales-items-list").FreezeHeader();
        }, 50);


    },
    bind_events: function () {
        var self = CounterSales;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $.UIkit.autocomplete($('#doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $("body").on("keyup change", "#counter-sales-items-list .Qty, .Rate,.SecondaryUnit", self.check_stock);
        $.UIkit.autocomplete($('#add-doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#add-doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);

        $('body').on('click', '#counter-sales-items-list tbody td:not(.action)', self.show_batch_with_stock);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKPatient").on("click", self.select_patient);
        $("#btnOKDoctor").on("click", self.select_doctor);
        $("#btnAddPatient").on("click", self.show_add_patient);
        $("#btnAddProduct").on("click", self.add_item);
        $(".btnSave, .btnSaveAndNew, .btnSaveASDraft, .btnSaveAndPrint").on("click", self.on_save);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("click", ".cancel", self.cancel_confirm);
        $("body").on("keyup change", "#batch-list .BatchQty", self.set_total_quantity);
        $('body').on('click', '#btnOkBatches', self.replace_batches);
        $("#btnOkAddPatient").on("click", self.save_patient);
        $("#PackingPrice").on("keyup change", self.calculate_packing_charge);
        $("#PaymentModeID").on("change", self.hide_show_elements);
        $("body").on("keyup change", "#TotalAmountReceived", self.calculate_balance);
        $("body").on("keydown", "#Qty", self.trigger_add_item);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $("#TypeID").on("change", self.hide_show_sales_type);
        $("#btnOKEmployee").on("click", self.select_employee);
        $('body').on('change', "#PaymentModeID", self.get_bank_name);
        $("#TypeID").on("change", self.set_batchtype);
        $("#DiscountPercentageList").on('change', self.calculate_discount_Amount);
        $("body").on("keydown", "#btnOkClose", self.print_close);
        $("body").on("click", "#btnClosePrint", self.print_close);
    },

    on_save_print: function () {
        var self = CounterSales;
        $(".btnSave").trigger("click");
    },
    on_save_new: function () {
        var self = CounterSales;
        $(".btnSaveAndNew").trigger("click");
    },
    on_save_draft: function () {
        var self = CounterSales;
        $(".btnSaveASDraft").trigger("click");
    },

    on_close: function () {
        var self = CounterSales;
        var location = "/Sales/CounterSales/Index";
        window.location = location;
    },
    on_cancel: function () {
        var self = CounterSales;
        $(".cancel").trigger("click");
    },

    on_print: function () {
        var self = CounterSales;
        $(".printpdf").trigger("click");
    },

    set_batchtype: function () {
        var self = CounterSales;
        var LocationID = $("#current-location select").val();

        if ($("#TypeID option:selected").text().toLowerCase() == "employee" && LocationID == POL_LOCATIONID) {
            if ($("#BatchTypeID").val() != 1) {
                $("#BatchTypeID").val(1);
                self.clear_form();
            }
        } else {
            if ($("#BatchTypeID").val() != $("#BatchTypeIDTemp").val()) {
                $("#BatchTypeID").val($("#BatchTypeIDTemp").val());
                self.clear_form();
            }
        }
    },
    showitemhistory: function () {
        var ItemID = $(this).closest('tr').find('.ItemID').val();
        $("#HistoryItemID").val(ItemID);
        $('#show-history').trigger('click');
    },
    clear_form: function () {
        var self = CounterSales;
        $("#counter-sales-items-list tbody").html('');
        $("#sales-invoice-amount-details-list tbody").html('');
        self.count_items();

        $("#NetAmount").val(0);
        $("#PackingPrice").val(0);
        $("#SGSTAmount").val(0);
        $("#CGSTAmount").val(0);
        $("#RoundOff").val(0);
        $("#TotalAmountReceived").val(0);
        $("#BalanceToBePaid").val(0);
        $("#CessAmount").val(0);
        $("#TaxableAmt").val(0);
        $("#GrossAmount").val(0);
    },

    get_bank_name: function () {
        var self = CounterSales;
        var mode;

        if ($("#PaymentModeID option:selected").text() == "Select") {
            mode = "";

        }
        else if ($("#PaymentModeID option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank";
        }

        $.ajax({
            url: '/Masters/Treasury/GetBankForCounterSales',
            data: {

                Mode: mode
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#BankID").html("");
                var html;
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });

    },

    select_employee: function () {
        var self = CounterSales;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var BalAmount = clean($(row).find(".BalAmount").val());
        $("#EmployeeName").val(Name);
        $("#BalAmount").val(BalAmount);
        $("#EmployeeID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetFreeMedicineEmployeeListForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                Type: $('#EmployeeType').val(),
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
        var self = CounterSales;
        $("#EmployeeID").val(item.id),
            $("#EmployeeName").val(item.name);
        $("#BalAmount").val(item.balamount);
    },

    trigger_add_item: function (e) {
        var self = CounterSales;
        if (e.keyCode == 13) {
            self.add_item();
        }
    },

    calculate_balance: function () {
        var self = CounterSales;
        var NetAmnt = clean($("#NetAmount").val());
        var TotalAmnt = clean($("#TotalAmountReceived").val());
        $("#BalanceToBePaid").val('');
        if (TotalAmnt > 0) {
            var Balance = TotalAmnt - NetAmnt;
            $("#BalanceToBePaid").val(Balance);
        }
    },

    hide_show_elements: function () {
        var self = CounterSales;
        var LocationID = $("#current-location select").val();
        var paymentid = $("#PaymentModeID").val();
        if (paymentid == 1) {
            $(".hidden").show();
            self.calculate_balance();
        }
        else {
            $("#BalanceToBePaid").val(0);
            $(".hidden").hide();

        }
    },

    on_save: function () {

        var self = CounterSales;
        var data = self.get_data();
        var location = "/Sales/CounterSales/Index";
        var url = '/Sales/CounterSales/Save';
        IsPrint = true;
        if ($(this).hasClass("btnSaveASDraft")) {
            data.IsDraft = true;
            IsPrint = false;
            url = '/Sales/CounterSales/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveAndPrint")) {
                IsPrint = true;
            }
            if ($(this).hasClass("btnSaveAndNew")) {
                location = "/Sales/CounterSales/Create";
                $("#save_new").val("new");
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                IsPrint = true;
                self.save(data, url, location);
            }, function () {
            })
        } else {
            IsPrint = false;
            self.save(data, url, location);
        }
    },

    details: function () {
       
        var self = CounterSales;
        setTimeout(function () {
            fh_items = $("#counter-sales-items-list").FreezeHeader();
        }, 50);
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".cancel", self.cancel_confirm);
        self.calculate_balance();
    },
    calculate_balancee: function () {
        var self = CounterSales;
        var NetAmount = $("#NetAmount").val();
        var AmountRecieveds = $("#").val();
        var balance = NetAmount - AmountRecieveds;
        $("#BalanceToBePaid").val(balance);


    },
    print: function () {
        var self = CounterSales;
        $.ajax({
            url: '/Sales/CounterSales/Print',
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
    //created by Priyanka
    print_countersales_invoice: function () {
        var self = CounterSales;
        $.ajax({
            url: '/Reports/Sales/CounterSalesInvoicePrintPdf',
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

    print_close: function () {
        var self = CounterSales;
        var SaveFunction = $("#save_new").val();
        if ($("#page_content_inner").hasClass("sales form-view")) {
            var url = '/Sales/CounterSales/Index';
            if (SaveFunction == 'new') {
                var url = '/Sales/CounterSales/Create';
            }
            setTimeout(function () {
                window.location = url
            }, 1000);
        }
    },
    calculate_packing_charge: function () {
        var net_amount = 0.00;
        var round_Off = 0.00;
        var temp = 0.00;
        var packing_charge = clean($("#PackingPrice").val());
        $("#counter-sales-items-list tbody tr").each(function () {
            net_amount += clean($(this).find('.NetAmount').val());
        });
        net_amount = net_amount + packing_charge;
        temp = net_amount;
        net_amount = Math.round(net_amount);
        round_Off = temp - net_amount;
        $("#RoundOff").val(round_Off);
        $("#NetAmount").val(net_amount);

    },

    show_add_patient: function () {
        var self = CounterSales;
        self.patient_clear();
        $('#show-add-patient').trigger('click');
    },

    patient_clear: function () {
        var date = $("#CurrentDate").val();
        $("#add-patient #PartyName").val('');
        $("#add-patient #MobileNo").val('');
        $("#Age").val('');
        $("#DOB").val(date);
        $("#Sex").val('F');
        $("#add-patient #Email").val('');
        $("#add-patient #Address1").val('');
        $("#add-patient #Address2").val('');
        $("#Place").val('');
        $("#PinCode").val('');
        $("#DoctorID").val('');
        $("#DoctorName").val('');
        $("#PatientID").val('');
    },

    check_stock: function () {
        var self = CounterSales;
        var row = $(this).closest('tr');
        var IssueQty = clean($(row).find('.Qty').val());
        var AvailableStock = clean($(row).find('.Stock').val());
        var Rate = clean($(row).find('.Rate').val());

        var DiscountAmount;
        var GST = clean($(row).find(".GST").val());
        var Quantity = clean($(row).find(".Qty").val());
        var BasicPrice = clean($(row).find('.BasicPrice').val());
        var GrossAmount;
        var CessPercentage = clean($(row).find('.CessPercentage').val());
        var discount = $("#DiscountPercentageList option:selected").data('value');
        var TaxableAmount;
        var GSTAmount;
        var NetAmount;
        var CGST;
        var SGST;
        var NetAmount;
        var CessAmount;
        if (IssueQty > AvailableStock) {
            app.show_error('Selected item dont have enough stock ');
            app.add_error_class($(row).find('.Qty'));
            return;
        }

        if (Sales.is_cess_effect()) {
            BasicPrice = (Rate * 100 / (100 + GST + CessPercentage)).roundTo(2);
            CessAmount = (BasicPrice * (Quantity) * CessPercentage / 100).roundTo(2);
        } else {
            BasicPrice = (Rate * 100 / (100 + GST)).roundTo(2);
            CessAmount = 0;
        }
        GrossAmount = (BasicPrice * Quantity).roundTo(2);

        DiscountAmount = GrossAmount * (discount / 100);
        TaxableAmount = GrossAmount - DiscountAmount;
        GSTAmount = (TaxableAmount * GST / 100).roundTo(2);
        CGST = (GSTAmount / 2).roundTo(2);
        SGST = (GSTAmount / 2).roundTo(2);
        NetAmount = (TaxableAmount + CGST + SGST + CessAmount).roundTo(2);

        $(row).find(".GrossAmount").val(GrossAmount);
        $(row).find(".TaxableAmount").val(TaxableAmount);
        $(row).find(".CGST").val(CGST);
        $(row).find(".SGST").val(SGST);
        $(row).find(".NetAmount").val(NetAmount);
        $(row).find(".CessAmount ").val(CessAmount);
        $(row).find(".DiscountAmount ").val(DiscountAmount);
        $(row).find(".BasicPrice ").val(BasicPrice);
        self.calculate_grid_total();

    },
    process_item: function (items) {

        var self = CounterSales;
        //items.LocationID = $("#LocationID").val()
        $.each(items, function (i, item) {
            if (item.UnitID == $("#SalesUnitID").val()) {
                item.MinSalesQty = clean($("#MinSalesQtyFull").val());
                item.MaxSalesQty = clean($("#MaxSalesQtyFull").val());
            }
            else {
                item.MinSalesQty = clean($("#MinSalesQtyLoose").val());
                item.MaxSalesQty = 5000;
            }
        });
        self.add_items_to_grid(items);

    },
    add_items_to_grid: function (items) {
        var self = CounterSales;
        var index, GSTAmount, tr = '';
        var readonly = '';

        $.each(items, function (i, item) {

            if (item.SalesUnitID == item.UnitID) {
                item.MRP = item.FullPrice;
                item.BasicPrice = item.BasicPrice;
            }
            else {
                item.MRP = item.LoosePrice;
            }
            item = self.get_item_properties(item);
            if (clean($("#IsPriceEditable").val()) == 0) {
                readonly = ' readonly="readonly"';
            }
            index = $("#counter-sales-items-list tbody tr").length + 1;
            var Gst = item.CGSTPercentage + item.SGSTPercentage;
            tr += '<tr>'
                + ' <td class="uk-text-center index">' + index + ' </td>'
                + ' <td >' + item.Code
                + '     <input type="hidden" class="ItemID" value="' + item.ItemID + '"  />'
                + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '"  />'
                + '     <input type="hidden" class="IGSTPercentage" value="' + item.IGSTPercentage + '" />'
                + '     <input type="hidden" class="SGSTPercentage" value="' + item.SGSTPercentage + '" />'
                + '     <input type="hidden" class="CGSTPercentage" value="' + item.CGSTPercentage + '" />'
                + '     <input type="hidden" class="BatchID" value="' + item.BatchID + '" />'
                + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                + '     <input type="hidden" class="Stock" value="' + item.Stock + '" />'
                + '     <input type="hidden" class="WareHouseID" value="' + item.WareHouseID + '" />'
                + '     <input type="hidden" class="TaxableAmount" value="' + item.TaxableAmount + '" />'
                + '     <input type="hidden" class="MinSalesQty" value="' + item.MinSalesQty + '" />'
                + '     <input type="hidden" class="MaxSalesQty" value="' + item.MaxSalesQty + '" />'
                + '</td>'
                + ' <td class="ItemName">' + item.Name + '</td>'
                + ' <td >' + item.BatchNo + '</d>'
                + ' <td class="Unit">' + item.Unit + '</td>'
                + ' <td class="ExpiryDate">' + item.ExpiryDateString + '</td>'
                + ' <td class="action"><input type="text" class="md-input Rate mask-sales-currency" value="' + item.MRP + '"' + readonly + '/></td>'
                + ' <td ><input type="text" class="BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + ' <td class="action"><input type="text" class="md-input Qty mask-numeric" value="' + item.Quantity + '"  /></td>'
                + ' <td ><input type="text" class="md-input GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input TaxableAmount mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'

                + ' <td ><input type="text" class="md-input GST mask-currency" value="' + Gst + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input CGST mask-sales-currency" value="' + item.CGST + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input SGST mask-sales-currency" value="' + item.SGST + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input IGST mask-sales-currency" value="' + item.IGST + '" readonly="readonly" /></td>'
                + ' <td class="cess-enabled"><input type="text" class="md-input CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                + ' <td class="cess-enabled"><input type="text" class="md-input CessAmount mask-sales-currency" value="' + item.CessAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input NetAmount mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + ' <td class="uk-text-center action">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        });
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#counter-sales-items-list tbody").append($tr);
        fh_items.resizeHeader();
        self.calculate_grid_total();
        self.clear_item();
        self.count_items();
        self.count();

    },
    count: function () {
        var count = $("#counter-sales-items-list tbody tr").length;

        $("#counter-sales-items-list tbody tr .index").each(function (i) {
            $(this).text(i + 1);

        });

    },
    replace_batches: function () {
        var self = CounterSales;
        if (self.validate_batch() > 0) {
            return;
        }
        UIkit.modal($('#batch-edit')).hide();
        var ItemID = $('#BatchItemID').val();
        var ItemName = $('#BatchItemName').val();
        var UnitID = $('#BatchUnitID').val();
        var MinSalesQty = clean($("#MinimumSalesQty").val());
        var MaxSalesQty = clean($("#MaximumSalesQty").val());
        var item = {};
        var items = [];

        $("#batch-list tbody tr").each(function () {
            if (clean($(this).find('.BatchQty').val()) > 0) {
                item = {
                    ItemID: ItemID,
                    Code: $(this).find('.ItemCode').val(),
                    Name: ItemName,
                    BatchNo: $(this).find('.Batch').text(),
                    UnitID: $(this).find('.UnitID').val(),
                    Unit: $(this).find('.Unit').val(),
                    Rate: clean($(this).find('.Rate').text()),
                    Quantity: clean($(this).find('.BatchQty').val()),
                    Stock: clean($(this).find('.Stock').text()),
                    GSTPercntage: clean($(this).find('.IGSTPercent').val()),
                    CGSTPercentage: clean($(this).find('.CGSTPercentage').val()),
                    IGSTPercentage: clean($(this).find('.IGSTPercentage').val()),
                    SGSTPercentage: clean($(this).find('.SGSTPercentage').val()),
                    CessPercentage: clean($(this).find('.CessPercentage').val()),
                    BatchID: clean($(this).find('.BatchID').val()),
                    BatchTypeID: clean($(this).find('.BatchTypeID').val()),
                    ExpiryDateString: $(this).find('.ExpiryDate').text(),
                    WareHouseID: clean($(this).find('.WareHouseID').val()),
                    SalesUnitID: clean($(this).find('.SalesUnitID').val()),
                    LoosePrice: clean($(this).find('.LoosePrice').val()),
                    FullPrice: clean($(this).find('.FullPrice').val()),
                    UnitID: UnitID,
                    MinSalesQty: MinSalesQty,
                    MaxSalesQty: MaxSalesQty,
                    //IsGSTRegisteredLocation: clean($(this).find('.IsGSTRegisteredLocation').val())
                    IsGSTRegisteredLocation: $(this).find('.IsGSTRegisteredLocation').val()

                };
                items.push(item);
            }
        });

        var row = $("#counter-sales-items-list tbody tr").find(".ItemID[value='" + ItemID + "']").closest("tr");
        $(row).find((".UnitID[value='" + UnitID + "']")).closest("tr").remove();

        self.add_items_to_grid(items);

    },

    set_total_quantity: function () {
        var self = CounterSales;
        var TotalIssueQty = 0;
        $("#batch-list .BatchQty").each(function () {
            TotalIssueQty += clean($(this).val());
        });
        $("#batch-edit .TotalQty").val(TotalIssueQty);
    },

    show_batch_with_stock: function () {
        var tr = "";
        var $tr;
        var TotalStock = 0.0;
        var TotalIssueQty = 0;
        var TempQty = 0;
        var row = $(this).closest("tr");
        var ItemID = row.find('.ItemID').val();
        var ItemName = row.find('.ItemName').text();
        var UnitID = row.find('.UnitID').val();
        var Unit = row.find('.Unit').text();
        var Qty = 0;
        $("#counter-sales-items-list tbody tr .ItemID[value='" + ItemID + "']").each(function () {
            Qty += clean($(this).closest('tr').find(".Qty").val());
        });
        var WareHouseID = row.find('.WareHouseID').val();
        var MaxsaleQty = clean(row.find('.MaxSalesQty').val());
        var MinSalesQty = clean(row.find('.MinSalesQty').val());
        $("#BatchItemName").val(ItemName);
        $("#BatchItemID").val(ItemID);
        $("#BatchUnitID").val(UnitID);
        $("#BatchUnit").val(Unit);
        $("#BatchQty").val(Qty);
        $("#BatchUnit").val(Unit);
        $("#BatchQty").val(Qty);
        $("#MinimumSalesQty").val(MinSalesQty);
        $("#MaximumSalesQty").val(MaxsaleQty);

        $.ajax({
            url: '/Masters/Batch/GetBatchesByItemIDForCounterSales',
            dataType: "json",
            data: {
                ItemID: ItemID,
                WarehouseID: WareHouseID,
                BatchTypeID: $("#BatchTypeID").val(),
                UnitID: UnitID,
                Qty: Qty,
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data).each(function (i, record) {
                        TotalStock += record.Stock;
                        TempQty = 0;
                        if (record.Stock >= Qty) {
                            TempQty = Qty;
                            Qty = 0;
                        } else {
                            TempQty = parseFloat(record.Stock);
                            Qty -= record.Stock;
                        }
                        TotalIssueQty += parseFloat(TempQty);
                        if (record.SalesUnitID == record.UnitID) {
                            record.Rate = record.FullRate;
                        }
                        else {
                            record.Rate = record.LooseRate;
                        }
                        tr += '<tr>'
                            + '<td>' + (i + 1)
                            + '     <input type="hidden" class="BatchID" value="' + record.BatchID + '" />'
                            + '     <input type="hidden" class="BatchTypeID" value="' + record.BatchTypeID + '" />'
                            + '     <input type="hidden" class="BatchType" value="' + record.BatchType + '" />'
                            + '     <input type="hidden" class="Unit" value="' + record.Unit + '" />'
                            + '     <input type="hidden" class="ItemID" value="' + record.ItemID + '" />'
                            + '     <input type="hidden" class="ItemName" value="' + record.ItemName + '" />'
                            + '     <input type="hidden" class="UnitID" value="' + record.UnitID + '" />'
                            + '     <input type="hidden" class="ItemCode" value="' + record.Code + '" />'
                            + '     <input type="hidden" class="CGSTPercentage" value="' + record.CGSTPercentage + '" />'
                            + '     <input type="hidden" class="IGSTPercentage" value="' + record.IGSTPercentage + '" />'
                            + '     <input type="hidden" class="SGSTPercentage" value="' + record.SGSTPercentage + '" />'
                            + '     <input type="hidden" class="WareHouseID" value="' + WareHouseID + '" />'
                            + '     <input type="hidden" class="SalesUnitID" value="' + record.SalesUnitID + '" />'
                            + '     <input type="hidden" class="FullPrice" value="' + record.FullRate + '" />'
                            + '     <input type="hidden" class="LoosePrice" value="' + record.LooseRate + '" />'
                            + '     <input type="hidden" class="CessPercentage" value="' + record.CessPercentage + '" />'
                            + '     <input type="hidden" class="IsGSTRegisteredLocation" value="' + record.IsGSTRegisteredLocation + '" />'
                            + '</td>'
                            + '<td class="Batch">' + record.BatchNo + ' </td>'
                            + '<td class="Stock mask-qty">' + record.Stock + '</td>'
                            + '<td class="mask-currency Rate">' + record.Rate + '</td>'
                            + '<td class="ExpiryDate" >' + record.ExpiryDateStr + '</td>'
                            + '<td><input type="text" class="md-input mask-numeric BatchQty" value = "' + TempQty + '" /></td>'
                            + '<tr>';
                    });
                    tr += '<tr>'
                        + '<td colspan="2"><b>Total</b></td>'
                        + '<td class="mask-qty">' + TotalStock + '</td>'
                        + '<td colspan="2"></td>'
                        + '<td><input type="text" class="md-input mask-numeric TotalQty" disabled value = "' + TotalIssueQty + '" /></td>'
                        + '<tr>';
                    $tr = $(tr);
                    app.format($tr);
                    $('#batch-list tbody').html($tr);
                    //$('#show-batch-edit').trigger('click');
                }
            }
        });
        //  $("#show-batch-edit").trigger('click');

    },
    get_doctor: function (release) {

        $.ajax({
            url: '/Masters/Supplier/GetDoctorAutoComplete',
            data: {
                term: $('#DoctorName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_doctor: function (event, item) {
        var self = CounterSales;
        $("#addDoctorName").val(item.Name);
        $("#DoctorName").val(item.Name);
        $("#DoctorID").val(item.id);
    },
    select_doctor: function () {
        var self = CounterSales;
        var radio = $('#select-doctor tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-doctor')).hide();
    },
    select_patient: function () {
        var self = CounterSales;
        var radio = $('#select-patient tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var DoctorName = $(row).find(".DoctorName").text().trim();
        $("#PartyName").val(Name);
        $("#PatientID").val(ID);
        $("#DoctorName").val(DoctorName);
        UIkit.modal($('#select-patient')).hide();
    },

    cancel: function () {
        var self = CounterSales;
        $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Sales/CounterSales/Cancel',
            data: {
                CounterSalesID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Counter sales cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/CounterSales/IndexV4";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".btnSave, .btnSaveASDraft,.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },

    cancel_confirm: function () {
        var self = CounterSales;
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },
    get_items: function (release) {
        var itemData = {
            Hint: $('#ItemName').val(),
            ItemCategoryID: $('#ItemCategoryID').val(),
            SalesCategoryID: $('#SalesCategoryID').val(),
            PriceListID: $('#PriceListID').val(),
            StoreID: $('#StoreID').val(),
            CheckStock: $('#CheckStock').val(),
            BatchTypeID: $('#BatchTypeID').val(),
            FullOrLooseID: $('#FullOrLooseID').val(),
            BusinessCategoryID: $('#BusinessCategoryID').val(),
        };
        $.ajax({
            url: '/Masters/Item/GetSaleableItemsForAutoComplete',
            data: itemData,
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, items) {   // on select auto complete item
        var self = CounterSales;
        $("#ItemID").val(items.id);
        $("#ItemName").val(items.value);
        $("#Code").val(items.code);
        $("#Stock").val(items.stock);
        $("#Rate").val(items.rate);
        $("#CGSTPercentage").val(items.cgstpercentage);
        $("#IGSTPercentage").val(items.igstpercentage);
        $("#SGSTPercentage").val(items.sgstpercentage);
        $("#VATPercentage").val(items.vatpercentage);
        $("#PartsNumber").val(items.partsnumber);
        $("#Remark").val(items.remark);
        $("#Model").val(items.model);
        $("#SalesUnit").val(items.saleUnit);
        $("#SalesUnitID").val(items.saleUnitId);
        $("#PrimaryUnit").val(items.unit);
        $("#PrimaryUnitID").val(items.unitId);
        $("#CessPercentage").val(items.cessPercentage);
        $("#UnitID").val(items.unit);
        $("#MinSalesQtyFull").val(items.minsalesqtyfull);
        $("#MinSalesQtyLoose").val(items.minsalesqtyloose);
        $("#MaxSalesQtyFull").val(items.maxsalesqtyfull);
        $("#MaxSalesQtyLoose").val(items.maxsalesqtyloose);
        $("#Qty").focus();


        self.get_units();
    },

    remove_item: function () {
        var self = CounterSales;
        $(this).closest("tr").remove();
        $("#counter-sales-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#counter-sales-items-list tbody tr").length);
        self.calculate_grid_total();
        self.count_items();
    },

    list: function () {
        var self = CounterSales;

        $('#tabs-counter-sales').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = CounterSales;

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-counter-sales');
                break;
            case "saved-counter-sales":
                $list = $('#saved-counter-sales');
                break;
            case "cancelled":
                $list = $('#cancel-counter-sales');
                break;
            default:
                $list = $('#draft-counter-sales');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Sales/CounterSales/GetListForCounterSales?type=" + type,
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
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "Type", "className": "Type" },
                    { "data": "PartyName", "className": "PartyName", },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.IsDraft);
                    $(row).addClass(data.IsCancelled);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/CounterSales/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });

            return list_table;
        }
    },
    counter_sales_history: function () {

        var $list = $('#counter-sales-history-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                if (title !== undefined && title.length > 0) {
                    $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
                }
            });
            altair_md.inputs();
            var url = "/Sales/SalesInvoice/GetCounterSalesHistory";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemID", Value: clean($('#HistoryItemID').val()) },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "Itemcode", "className": "Itemcode" },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "PartsNumber", "className": "PartsNumber" },
                    {
                        "data": "MRP", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + normalclass + "' >" + row.MRP + "</div>";
                        }
                    },
                    {
                        "data": "Quantity", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Quantity + "</div>";
                        }
                    },
                    { "data": "Unit", "className": "Unit" },
                    {
                        "data": "DiscountPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                        }
                    },
                    {
                        "data": "VATPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.VATPercentage + "</div>";
                        }
                    },
                    {
                        "data": "NetAmount", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("click", '#show-history', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    purchase_order_history: function () {

        var $list = $('#purchase-history-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                if (title !== undefined && title.length > 0) {
                    $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
                }
            });
            altair_md.inputs();
            var url = "/Sales/SalesInvoice/GetPurchaseHistory";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ItemID", Value: clean($('#HistoryItemID').val()) },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    { "data": "PurchaseOrderNo", "className": "" },
                    { "data": "PurchaseOrderDate", "className": "" },
                    { "data": "SupplierName", "className": "" },
                    { "data": "Itemcode", "className": "" },
                    { "data": "ItemName", "className": "" },
                    { "data": "PartsNumber", "className": "" },
                    {
                        "data": "LandedCost", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + normalclass + "' >" + row.LandedCost + "</div>";
                        }
                    },
                    {
                        "data": "SecondaryRate", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + normalclass + "' >" + row.SecondaryRate + "</div>";
                        }
                    },
                    {
                        "data": "SecondaryQty", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.SecondaryQty + "</div>";
                        }
                    },
                    { "data": "SecondaryUnit", "className": "" },
                    {
                        "data": "DiscountPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                        }
                    },
                    {
                        "data": "VATPercentage", "className": "",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.VATPercentage + "</div>";
                        }
                    },
                    {
                        "data": "NetAmount", "className": "", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='" + normalclass + "' >" + row.NetAmount + "</div>";
                        }
                    }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("click", '#show-history', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    // Selects the item on clicking the modal ok button 
    select_item: function () {
        var self = CounterSales;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var UnitID = $(row).find(".UnitID").val();
        var Stock = $(row).find(".Stock").val();
        var CGSTPercentage = $(row).find(".CGSTPercentage").val();
        var IGSTPercentage = $(row).find(".IGSTPercentage").val();
        var SGSTPercentage = $(row).find(".SGSTPercentage").val();
        var VATPercentage = $(row).find(".VATPercentage").val();
        var PartsNumber = $(row).find(".PartsNumber").val();
        var Remark = $(row).find(".Remark").val();
        var Model = $(row).find(".Model").val();
        var Rate = $(row).find(".Rate").val();
        var SalesUnit = $(row).find(".SalesUnit").val();
        var SalesUnitID = $(row).find(".SalesUnitID").val();
        var CessPercentage = $(row).find(".CessPercentage").val();
        var Maxsalesqtyfull = $(row).find(".MaxSalesQtyFull").val();
        var Minsalesqtyloose = $(row).find(".MinSalesQtyLoose").val();
        var Minsalesqtyfull = $(row).find(".MinSalesQtyFull").val();
        var Maxsalesqtyloose = $(row).find(".MaxSalesQtyFull").val();
        var SecondaryUnits = $(row).find(".SecondaryUnits").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#PartsNumber").val(PartsNumber);
        $("#Remark").val(Remark);
        $("#Model").val(Model);
        $("#Unit").val(Unit);
        $("#UnitID").val(UnitID);
        $("#Stock").val(Stock);
        $("#CGSTPercentage").val(CGSTPercentage);
        $("#IGSTPercentage").val(IGSTPercentage);
        $("#SGSTPercentage").val(SGSTPercentage);
        $("#VATPercentage").val(VATPercentage);
        $("#Rate").val(Rate);
        $("#PrimaryUnit").val(Unit);
        $("#PrimaryUnitID").val(UnitID);
        $("#SalesUnit").val(SalesUnit);
        $("#SalesUnitID").val(SalesUnitID);
        $("#CessPercentage").val(CessPercentage);
        $("#MinSalesQtyFull").val(Minsalesqtyfull);
        $("#MinSalesQtyLoose").val(Minsalesqtyloose);
        $("#MaxSalesQtyFull").val(Maxsalesqtyfull);
        $("#MaxSalesQtyLoose").val(Maxsalesqtyloose);
        $("#SecondaryUnits").val(SecondaryUnits);
        $("#Qty").focus();
        self.get_units();
        UIkit.modal($('#select-item')).hide();

    },
    get_units: function () {
        var self = CounterSales;
        $("#UnitID").html("");
        var html = "";
        html += "<option value='" + $("#SalesUnitID").val() + "'>" + $("#SalesUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";

        $("#UnitID").append(html);

    },
    add_item: function () {
        var self = CounterSales;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Sales/CounterSales/GetBatchwiseItemForCounterSales',
            dataType: "json",
            data: {
                ItemID: $('#ItemID').val(),
                WarehouseID: $('#StoreID').val(),
                BatchTypeID: $("#BatchTypeID").val(),
                Qty: clean($('#Qty').val()),
                UnitID: $("#UnitID").val(),
                Unit: $("#UnitID option:selected").text(),
                CustomerType: $("#TypeID option:selected").text().toLowerCase()
            },
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.process_item(response.Data);
                    $("#ItemName").focus();
                } else {
                    app.show_error(response.Message)
                }
            },
        });


        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);
    },

    count_items: function () {
        var count = $('#counter-sales-items-list tbody tr').length;
        $('#item-count').val(count);
    },
    //Clear item details after adding to grid
    clear_item: function () {
        var self = CounterSales;
        $("#ItemID").val('');
        $("#Unit").val('');
        $("#UnitID").val('');
        $("#Batch").val('');
        $("#FullOrLooseID").val(1);
        $("#CGSTPercentage").val('');
        $("#IGSTPercentage").val('');
        $("#SGSTPercentage").val('');
        $("#DiscountPercentage").val('');
        $("#Qty").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);

    },
    // calculates and returns item properties
    get_item_properties: function (item) {

        var self = CounterSales;
        if (item.IsGSTRegisteredLocation) {
            if (Sales.is_cess_effect()) {
                item.BasicPrice = (item.MRP * 100 / (100 + item.IGSTPercentage + item.CessPercentage)).roundTo(2);
                item.CessAmount = (item.BasicPrice * (item.Quantity) * item.CessPercentage / 100).roundTo(2);
            } else {
                item.BasicPrice = (item.MRP * 100 / (100 + item.IGSTPercentage)).roundTo(2);
                item.CessAmount = 0;
            }
            var discount = $("#DiscountPercentageList option:selected").data('value');

            item.GrossAmount = (item.BasicPrice * item.Quantity).roundTo(2);
            //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
            TaxableAmt = item.GrossAmount * (discount / 100);
            item.DiscountAmount = item.GrossAmount * (discount / 100);
            item.TaxableAmount = item.GrossAmount - TaxableAmt;
            GSTAmount = (item.TaxableAmount * item.IGSTPercentage / 100).roundTo(2);

            item.CGST = (GSTAmount / 2).roundTo(2);
            item.SGST = (GSTAmount / 2).roundTo(2);
            item.NetAmount = item.TaxableAmount + item.CGST + item.SGST + item.CessAmount;

            return item;
        } else {
            item.BasicPrice = item.MRP.roundTo(2);

            var discount = $("#DiscountPercentageList option:selected").data('value');

            item.GrossAmount = (item.BasicPrice * item.Quantity).roundTo(2);
            //item.TaxableAmount = item.GrossAmount;   // - item.DiscountAmount - item.AdditionalDiscount;
            TaxableAmt = item.GrossAmount * (discount / 100);
            item.DiscountAmount = item.GrossAmount * (discount / 100);
            item.TaxableAmount = item.GrossAmount - TaxableAmt;
            GSTAmount = 0;

            item.CGST = 0;
            item.SGST = 0;
            item.NetAmount = item.TaxableAmount + item.CGST + item.SGST + item.CessAmount;

            return item;
        }

    },
    save: function (data, url, location) {

        var self = CounterSales;
        var IsDotMatrixPrint = $("#IsDotMatrixPrint").val();
        var IsThermalPrint = $("#IsThermalPrint").val();
        $(".btnSaveASDraft, .btnSave,.cancel,.btnSaveAndNew,.btnSaveAndPrint").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Counter Sales Saved Successfully");
                    $("#ID").val(response.ID);
                    if (IsPrint == true) {
                        self.print_countersales_invoice();
                        //if (IsDotMatrixPrint == "True" && IsThermalPrint == "False") {
                        //    self.print();
                        //}
                        //else if (IsDotMatrixPrint == "False" && IsThermalPrint == "True") {
                        //    //console.log("haaaaa");
                        //    //console.log(JSON.stringify(response));
                        //    self.thermal_print_on_create();
                        //}
                        //else {
                        //    self.print_countersales_invoice();
                        //}
                    }
                    else {
                        setTimeout(function () {
                            window.location = location;
                        }, 1000);
                    }

                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnSave,.cancel,.btnSaveAndNew,.btnSaveAndPrint").css({ 'display': 'block' });

                }
            }
        });
    },


    save_patient: function () {
        var self = CounterSales;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_patient();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_patient_data();
        $.ajax({
            url: '/Masters/Patient/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.Data > 0) {
                        $("#PatientID").val(response.Data);

                        var doctorName = $("#addDoctorName").val();
                        var name = $("#add-patient #PartyName").val();

                        $("#DoctorName").val(doctorName);
                        $("#PartyName").val(name);
                        UIkit.modal($('#add-patient')).hide();

                        $.ajax({
                            url: '/Sales/CounterSales/GetPatientSerialNo',
                            dataType: "json",
                            type: "POST",
                            success: function (response) {
                                if (response.Status == "success") {

                                    $("#PatientCode").val(response.data);
                                }
                            }
                        })

                    }
                    else {
                        app.show_error("Patient already exist");
                    }
                } else {
                    app.show_error("Patient creation failed");
                }
            }
        });
    },

    get_data: function () {
        var item = {};
        var AmountDetails = {};
        var data = {};
        var SalesTypeName = $("#TypeID Option:Selected").text();

        data = {
            ID: $("#ID").val(),
            TransNo: $("#TransNo").val(),
            TransDate: $("#TransDate").val(),
            PatientID: $("#PatientID").val(),
            WarehouseID: $("#StoreID").val(),
            DoctorID: $("#DoctorID").val(),
            NetAmount: clean($("#NetAmount").val()),
            PackingPrice: clean($("#PackingPrice").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            PaymentModeID: clean($("#PaymentModeID").val()),
            TotalAmountReceived: clean($("#TotalAmountReceived").val()),
            BalanceToBePaid: clean($("#BalanceToBePaid").val()),
            CessAmount: clean($("#CessAmount").val()),
            EmployeeID: clean($("#EmployeeID").val()),
            TypeID: clean($("#TypeID").val()),
            TaxableAmt: clean($("#TaxableAmt").val()),
            GrossAmount: clean($("#GrossAmount").val()),
            BankID: $("#BankID").val(),
            DoctorName: $("#DoctorName").val(),
            DiscountCategoryID: clean($("#DiscountPercentageList").val()),
            DiscountAmt: clean($("#DiscountAmt").val()),
            AmountReceived: clean($("#AmountReceived").val()),

        };
        if (SalesTypeName == "Patient") {
            data.PartyName = $("#PartyName").val();
        }
        else if (SalesTypeName == "Cash Sales") {
            data.PartyName = $("#CashSales").val();
        }
        else if (SalesTypeName == "Employee") {
            data.PartyName = $("#EmployeeName").val();
        }

        data.Items = [];
        $("#counter-sales-items-list tbody tr").each(function () {
            item = {
                ItemID: $(this).find(".ItemID").val(),
                ItemName: $(this).find(".ItemName").val(),
                UnitID: $(this).find(".UnitID").val(),
                FullOrLoose: $(this).find(".FullOrLoose").text(),
                MRP: clean($(this).find(".Rate").val()),
                BasicPrice: clean($(this).find(".BasicPrice ").val()),
                Quantity: clean($(this).find(".Qty").val()),
                GrossAmount: clean($(this).find(".GrossAmount").val()),
                GSTPercntage: $(this).find(".GST").val(),
                CGSTAmount: clean($(this).find(".CGST").val()),
                SGSTAmount: clean($(this).find(".SGST").val()),
                IGSTAmount: clean($(this).find(".IGST").val()),
                NetAmount: clean($(this).find(".NetAmount").val()),
                BatchTypeID: clean($(this).find(".BatchTypeID").val()),
                BatchID: clean($(this).find(".BatchID").val()),
                WareHouseID: clean($(this).find(".WareHouseID").val()),
                SGSTPercentage: clean($(this).find(".SGSTPercentage").val()),
                CGSTPercentage: clean($(this).find(".CGSTPercentage").val()),
                IGSTPercentage: clean($(this).find(".IGSTPercentage").val()),
                TaxableAmount: clean($(this).find(".TaxableAmount").val()),
                CessAmount: clean($(this).find(".CessAmount").val()),
                CessPercentage: clean($(this).find(".CessPercentage").val()),
                DiscountAmount: clean($(this).find(".DiscountAmount").val()),
            }
            data.Items.push(item);
        });
        data.AmountDetails = [];
        $("#sales-invoice-amount-details-list tbody tr").each(function () {
            AmountDetails = {
                Amount: $(this).find(".Amount").val(),
                Particulars: $(this).find(".Particulars").text().trim(),
                Percentage: $(this).find(".Percentage").val(),
            }
            data.AmountDetails.push(AmountDetails);
        });

        return data;
    },
    get_patient_data: function () {
        var data = {
            Code: $("#PatientCode").val(),
            Name: $("#add-patient #PartyName").val(),
            Email: $("#Email").val(),
            Mobile: $("#add-patient #MobileNo").val(),
            Age: $("#Age").val(),
            DOB: $("#DOB").val(),
            Sex: $("#Sex").val(),
            Email: $("#Email").val(),
            Address1: $("#add-patient #Address1").val(),
            Address2: $("#add-patient #Address2").val(),
            Place: $("#Place").val(),
            PinCode: $("#PinCode").val(),
            DoctorID: $("#DoctorID").val(),
            DoctorName: $("#add-patient #addDoctorName").val(),
        };
        return data;
    },
    calculate_grid_total: function () {
        var self = CounterSales;
        var GrossAmount = 0;
        var DiscountAmount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var temp = 0;
        var CessAmount = 0;
        var packingcharge = clean($("#PackingPrice").val());
        $("#counter-sales-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            DiscountAmount += clean($(this).find('.DiscountAmount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGST').val());
            CGSTAmount += clean($(this).find('.CGST').val());
            IGSTAmount += clean($(this).find('.IGST').val());
            NetAmount += clean($(this).find('.NetAmount').val());
            CessAmount += clean($(this).find('.CessAmount').val());
        });

        NetAmount += packingcharge;
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = temp - NetAmount;
        $("#GrossAmount").val(GrossAmount);
        $("#DiscountAmt").val(DiscountAmount);
        $("#TaxableAmt").val(TaxableAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
        $("#CessAmount").val(CessAmount);
        self.add_amount_details();
        self.calculate_balance();
    },

    calculate_discount_Amount: function () {
        var self = CounterSales;
        var discountamt = 0
        var discount = $("#DiscountPercentageList option:selected").data('value');
        $("#counter-sales-items-list tbody tr").each(function () {
            $row = $(this).closest("tr");
            GrossAmount = clean($row.find(".GrossAmount").val());
            SGSTPercentage = clean($row.find(".SGSTPercentage").val());
            CGSTPercentage = clean($row.find(".CGSTPercentage").val());
            CessPercentage = clean($row.find(".CessPercentage").val());
            TaxableAmt = GrossAmount * (discount / 100);
            discountamt = GrossAmount * (discount / 100);
            TaxableAmt = GrossAmount - TaxableAmt;
            SGSTAmt = TaxableAmt * (SGSTPercentage / 100);
            CGSTAmt = TaxableAmt * (CGSTPercentage / 100);
            CessAmount = TaxableAmt * (CessPercentage / 100);
            var NetAmount = TaxableAmt + CessAmount + CGSTAmt + SGSTAmt
            $row.find(".TaxableAmount").val(TaxableAmt);
            $row.find(".CessAmount").val(CessAmount);
            $row.find(".CGST").val(CGSTAmt);
            $row.find(".SGST").val(SGSTAmt);
            $row.find(".NetAmount").val(NetAmount);
            $row.find(".DiscountAmount").val(discountamt);

        });
        self.calculate_grid_total();
    },

    add_amount_details: function () {

        var self = CounterSales;
        //var amt_details = [];
        var index;
        var j = 1;
        var GST = 0;
        var SGST = 0;
        var CGST = 0;
        var IGST = 0;
        var CessPercentage, CessAmount;
        var IsInclusiveGST = false;
        var tr = "";
        var j = 1;
        var tax_percentages = $('.tax-template').html();
        var inclusive_gst = "";
        self.amt_details = [];

        $("#counter-sales-items-list tbody tr").each(function () {
            $row = $(this).closest("tr");
            GST = clean($row.find(".GST").val());
            VAT = clean($row.find(".VAT").val());
            IsVAT = clean($row.find(".IsVAT").val());
            IsGST = clean($row.find(".IsGST").val());
            SGST = parseFloat($row.find(".SGSTPercentage").val());
            CGST = parseFloat($row.find(".CGSTPercentage").val());
            SGSTamount = parseFloat($row.find(".SGST").val());
            CGSTamount = parseFloat($row.find(".CGST").val());
            VATAmount = parseFloat($row.find(".VATAmount").val());
            CessAmount = clean($row.find(".CessAmount").val());
            CessPercentage = clean($row.find(".CessPercentage").val());
            //  IGST = parseFloat($row.find(".IGST").val());
            if (IsGST == 1) {
                self.calculate_group_amount_details(SGST, SGSTamount, "SGST");
                self.calculate_group_amount_details(CGST, CGSTamount, "CGST");
                self.calculate_group_amount_details(CessPercentage, CessAmount, "Cess");
            }
            if (IsVAT == 1) {
                self.calculate_group_amount_details(VAT, VATAmount, "VAT");
                self.calculate_group_amount_details(CessPercentage, CessAmount, "Cess");
            }
            // self.calculate_group_amount_details(GST, IGST, "IGST");
        });
        $.each(self.amt_details, function (i, record) {
            tr += "<tr  class='uk-text-center'>";
            tr += "<td>" + (i + 1);
            tr += "</td>";
            tr += "<td class='Particulars'>" + record.particular;
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency Percentage' readonly value='" + record.tax_percentage + "' />";
            tr += "</td>";
            tr += "<td><input type='text' class='md-input mask-sales-currency Amount' readonly value='" + record.amount + "' />";
            tr += "</td>";
            tr += "</tr>";
        });
        $tr = $(tr);
        app.format($tr);
        $("#sales-invoice-amount-details-list tbody").html($tr);
    },
    //Amount details grouping
    calculate_group_amount_details: function (GST, value, type) {
        var self = CounterSales;
        if (value == 0) {
            return;
        }
        index = self.search_amount_group(self.amt_details, GST, value, type);
        if (index == -1) {
            self.amt_details.push(
                {
                    particular: type,
                    amount: value,
                    tax_percentage: GST
                })
        }
        else {
            self.amt_details[index].amount += value;
        }
    },
    search_amount_group: function (amt_details, GST, value, type) {
        for (var i = 0; i < amt_details.length; i++) {
            if (amt_details[i].tax_percentage == GST && amt_details[i].particular == type) {
                return i;
            }
        }
        return -1;
    },
    validate_form: function () {
        var self = CounterSales;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_item: function () {
        var self = CounterSales;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },
    validate_patient: function () {
        var self = CounterSales;
        if (self.rules.on_save_patient.length > 0) {
            return form.validate(self.rules.on_save_patient);
        }
        return 0;
    },
    validate_draft: function () {
        var self = CounterSales;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_batch: function () {
        var self = CounterSales;
        if (self.rules.batch.length > 0) {
            return form.validate(self.rules.batch);
        }
        return 0;
    },
    // Validation rules
    rules: {
        on_select_item: [
            {
                elements: "#CustomerID",
                rules: [
                    { type: form.required, message: "Please choose a customer" },
                    { type: form.non_zero, message: "Please choose a customer" },
                ]
            },
        ],
        on_add_item: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                    {
                        type: function (element) {
                            var error = false;
                            var unitid = $("#UnitID option:selected").val();
                            $("#counter-sales-items-list tbody tr").each(function () {
                                if (($(this).find(".ItemID").val() == $(element).val()) && (($(this).find(".UnitID").val() == unitid))) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already exists"
                    },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                    {
                        type: function (element) {
                            var error = false;

                            var MinSalesQty = clean($("#MinSalesQtyFull").val());
                            var MaxSalesQty = clean($("#MaxSalesQtyFull").val());
                            var Qty = clean($(element).val());
                            if (MinSalesQty > Qty) {
                                error = true;
                            }

                            return MinSalesQty <= Qty;
                        }, message: "Item quantity deceeds minimum sales quantity "
                    },
                    {
                        type: function (element) {
                            var error = false;
                            var MinSalesQty = clean($("#MinSalesQtyFull").val());
                            var MaxSalesQty = clean($("#MaxSalesQtyFull").val());
                            var Qty = clean($(element).val());
                            return (MaxSalesQty >= Qty)
                        }, message: "Item quantity exceeds maximum sale quantity "
                    },

                ]
            },

            {
                elements: "#FullOrLoose",
                rules: [
                    { type: form.required, message: "Please select full or loose" },
                ]
            }
        ],
        on_save_patient: [
            {
                elements: "#add-patient #PartyName",
                rules: [
                    { type: form.required, message: "Please enter name" },
                ]
            },
            {
                elements: "#add-patient #PatientCode",
                rules: [
                    { type: form.required, message: "Please enter code" },
                ]
            },
            {
                elements: "#add-patient #DoctorName",
                rules: [
                    {
                        type: function (element) {
                            var error = 0;
                            var doctor = $(element).val();
                            var doctorid = clean($("#DoctorID").val());
                            if (doctor != '' && doctorid <= 0)
                                error++;
                            return error == 0;
                        }, message: "Please select doctor"
                    },
                ]
            },
        ],
        on_draft: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },

            {
                elements: "#BalanceToBePaid",
                rules: [
                    { type: form.positive, message: "Invalid Balance Amount" },
                ]
            },

            {
                elements: "#TotalAmountReceived",
                rules: [

                    { type: form.positive, message: "Invalid amount received" },
                ]
            },
            {
                elements: ".NetAmount",
                rules: [
                    { type: form.non_zero, message: "Invalid net amount in grid" },
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
                elements: "#PatientID.enabled",
                rules: [
                    { type: form.required, message: "Please choose Patient", alt_element: "#PartyName" },
                    { type: form.positive, message: "Please choose Patient", alt_element: "#PartyName" },
                    { type: form.non_zero, message: "Please choose Patient", alt_element: "#PartyName" }
                ]
            },

            {
                elements: "#TotalAmountReceived",
                rules: [

                    { type: form.positive, message: "Invalid amount received" },
                ]
            },

            //{
            //    elements: "#CashSales.enabled",
            //    rules: [
            //        { type: form.required, message: "Please enter CustomerName" },
            //    ]
            //},

            {
                elements: "#EmployeeID.enabled",
                rules: [
                    { type: form.required, message: "Please choose Employee", alt_element: "#EmployeeName" },
                    { type: form.positive, message: "Please choose Employee", alt_element: "#EmployeeName" },
                    { type: form.non_zero, message: "Please choose Employee", alt_element: "#EmployeeName" }
                ]
            },


            {
                elements: "#SONo",
                rules: [
                    { type: form.required, message: "Invalid Sales Order Number" },
                ]
            },
            {
                elements: "#BankID",
                rules: [
                    { type: form.non_zero, message: "Bank name required" },
                ]
            },
            {
                elements: "#SODate",
                rules: [
                    { type: form.required, message: "Invalid Date" },
                ]
            },
            {
                elements: "#CustomerName",
                rules: [
                    { type: form.required, message: "Invalid Customer" },
                ]
            },
            {
                elements: "#DespatchDate",
                rules: [
                    { type: form.required, message: "Invalid Dispatch Date" },
                ]
            },
            {
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },
                    {
                        type: function (element) {
                            var error = false;
                            var sum = 0;
                            var itemid = clean($(element).closest('tr').find('.ItemID').val())
                            $("#counter-sales-items-list tbody tr .ItemID[value='" + itemid + "']").each(function () {
                                sum += clean($(this).closest('tr').find(".Qty").val());
                            });
                            var MinSalesQty = clean($(element).closest('tr').find(".MinSalesQty").val());
                            return MinSalesQty <= sum;

                        }, message: "Item quantity deceeds minimum sales quantity "
                    },
                    {
                        type: function (element) {
                            var error = false;
                            var sum = 0;
                            var itemid = clean($(element).closest('tr').find('.ItemID').val())
                            $("#counter-sales-items-list tbody tr .ItemID[value='" + itemid + "']").each(function () {
                                sum += clean($(this).closest('tr').find(".Qty").val());
                            });

                            var MaxSalesQty = clean($(element).closest('tr').find(".MaxSalesQty").val());
                            return MaxSalesQty >= sum;
                        }, message: "Item quantity exceeds maximum sale quantity"
                    },

                ]
            },

            {
                elements: ".NetAmount",
                rules: [
                    { type: form.non_zero, message: "Invalid net amount in grid" },
                ]
            },

            {
                elements: "#BalanceToBePaid",
                rules: [
                    { type: form.positive, message: "Invalid Balance Amount" },
                ]
            },

            //{
            //    elements: "#TotalAmountReceived",
            //    rules: [
            //         {
            //             type: function (element) {
            //                 return ($("#PaymentModeID").val() == 1) ? form.non_zero(element) : true;
            //             }, message: "Please enter a Total Amount"
            //         }
            //    ],
            //},

            {
                elements: "#NetAmount",
                rules: [
                    { type: form.required, message: "Invalid Net Amount" },
                    { type: form.non_zero, message: "Invalid Net Amount" },
                    { type: form.positive, message: "Invalid Net Amount" },
                    {
                        type: function (element) {
                            var error = false;
                            var Type = clean($("#TypeID").val());
                            if (Type == 2) {
                                var netamount = clean($("#NetAmount").val());
                                var balamount = clean($("#BalAmount").val());
                                if (netamount > balamount) {
                                    error = true;
                                }
                            }
                            return !error
                        }, message: "Net amount greater than balance amount"
                    },
                ]
            },
            //{
            //    elements: "#DoctorName",
            //    rules: [
            //         {
            //             type: function (element) {
            //                 var error = 0;
            //                 var doctor = $(element).val();
            //                 var doctorid = $("#DoctorID").val();
            //                 if ((doctor != "" && doctor != " ") && doctorid <= 0)
            //                     error++;
            //                 return error == 0;
            //             }, message: "Please select doctor"
            //         },
            //    ]
            //},
        ],
        batch: [
            {
                elements: "#batch-edit .BatchQty",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            return clean($(element).val()) <= clean($(row).find('.Stock').text());
                        }, message: "Insufficient Stock"
                    },
                    { type: form.positive, message: "Invalid batch quantity" },
                ]
            },
        ]
    },

    hide_show_sales_type: function () {
        var self = CounterSales;
        var Type = $("#TypeID option:selected").text();
        if (Type == "Employee") {
            $(".Employee").show();
            $(".Patient").hide();
            $(".CashSales").hide();
            $("#PatientID").attr("disabled", "disabled").removeClass("enabled");
            $("#CashSales").attr("disabled", "disabled").removeClass("enabled");
            $("#EmployeeID").removeAttr("disabled").addClass("enabled");
            $("#PatientID").val(0);
            $("#CashSales").val('');
            $("#PartyName").val('');
        }
        if (Type == "Patient") {
            $(".Patient").show();
            $(".CashSales").hide();
            $(".Employee").hide();
            $("#PatientID").removeAttr("disabled").addClass("enabled");
            $("#CashSales").attr("disabled", "disabled").removeClass("enabled");
            $("#EmployeeID").attr("disabled", "disabled").removeClass("enabled");
            $("#EmployeeID").val(0);
            $("#CashSales").val('');
            $("#EmployeeName").val('');

        }
        if (Type == "Cash Sales") {
            $(".Patient").hide();
            $(".CashSales").show();
            $(".Employee").hide();
            $("#PatientID").attr("disabled", "disabled").removeClass("enabled");
            $("#CashSales").removeAttr("disabled").addClass("enabled");
            $("#EmployeeID").attr("disabled", "disabled").removeClass("enabled");
            $("#EmployeeID").val(0);
            $("#EmployeeName").val('');
            $("#PartyName").val('');
            $("#PatientID").val(0);
        }
    },
}

