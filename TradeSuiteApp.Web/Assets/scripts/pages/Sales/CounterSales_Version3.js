CounterSales.init = function () {
    var self = CounterSales;

    //self.patient_list();
    //Employee.employee_list('FreeMedicineEmployeeList');
    //item_select_table = $('#employee-list').SelectTable({
    //    selectFunction: self.select_employee,
    //    modal: "#select-all-employee",
    //    initiatingElement: "#EmployeeName"
    //});
    employee_list = Employee.employee_list();
    item_select_table = $('#employee-list').SelectTable({
        selectFunction: self.select_doctor,
        returnFocus: "#ItemName",
        modal: "#select-employee",
        initiatingElement: "#DoctorName"
    });
    $('#appointment-process-list').SelectTable({
        selectFunction: self.select_patient,
        returnFocus: "#PartyName",
        modal: "#select-appointment-process",
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
    //Employee.employee_free_medicine_lists();
    item_select_table = $('#employees-list').SelectTable({
        selectFunction: self.select_employees,
        returnFocus: "#ItemName",
        modal: "#select-all-employee",
        initiatingElement: "#DoctorName"
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
    setTimeout(function () {
        fh_items = $("#counter-sales-items-list").FreezeHeader();
    }, 50);
};

CounterSales.patient_list = function () {
    var $list = $('#appointment-process-list');
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
            "aaSorting": [[3, "asc"]],
            "ajax": {
                "url": "/Sales/CounterSales/GetAppointmentProcessList",
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
                        return meta.settings.oAjaxData.start + meta.row + 1;
                    }
                },
                {
                    "data": null,
                    "className": "uk-text-center",
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        return "<input type='radio' class='uk-radio ID' name='PatientID' data-md-icheck value='" + row.ID + "' >"
                            + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                            + "<input type='hidden' class='DoctorID' value='" + row.DoctorID + "'>"
                            + "<input type='hidden' class='DiscountCategoryID' value='" + row.DiscountCategoryID + "'>";
                    }
                },
                { "data": "TransNo", "className": "TransNo" },
                { "data": "TransDate", "className": "TransDate" },
                { "data": "PatientName", "className": "PatientName" },
                { "data": "DoctorName", "className": "DoctorName" },
                { "data": "Phone", "className": "Phone" },

            ],
            "drawCallback": function () {
                altair_md.checkbox_radio();
                $list.trigger("datatable.changed");
            },
        });
        $('body').on("change", '#LocationID', function () {
            list_table.fnDraw();
        });

        $list.on('previous.page', function () {
            list_table.api().page('previous').draw('page');
        });
        $list.on('next.page', function () {
            list_table.api().page('next').draw('page');
        });
        list_table.api().columns().each(function (g, h) {
            $('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        });
        return list_table;
    }
};

CounterSales.select_patient = function () {
    var self = CounterSales;
    var length = $("#counter-sales-items-list tbody tr").length;
    var radio = $('#appointment-process-list tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".PatientName").text().trim();
    var DoctorName = $(row).find(".DoctorName").text().trim();
    var PatientID = clean($(row).find(".PatientID").val());
    var DoctorID = clean($(row).find(".DoctorID").val())
    var DiscountCategoryID = clean($(row).find(".DiscountCategoryID").val())
    var DiscountPercentageList = clean($(row).find(".DiscountCategoryID").val())
    if (length > 0) {
        //app.confirm("Selected Items will be removed", function () {
        app.confirm_cancel("Selected Items will be removed", function () {
            $("#GrossAmount").val('');
            $("NetAmount").val('');
            $("#DiscountAmount").val('');
            $("#TaxableAmount").val('');
            $("#SGSTAmount").val('');
            $("#CGSTAmount").val('');
            $("#IGSTAmount").val('');
            $("#CessAmount").val('');
            $("#RoundOff").val('');
            $("#PartyName").val('');
            $("#PatientID").val('');
            $("#DoctorName").val('');
            $("#DoctorID").val('');
            $("#AppointmentProcessID").val('');
            $("#counter-sales-items-list tbody tr").remove();
            $("#item-count").val($("#counter-sales-items-list tbody tr").length);
            $("#PartyName").val(Name);
            $("#PatientID").val(PatientID);
            $("#DoctorName").val(DoctorName);
            $("#DoctorID").val(DoctorID);
            $("#DoctorID").val(DoctorID);
            $("#DiscountPercentageList").val(DiscountCategoryID);
            UIkit.modal($('#select-appointment-process')).hide();
            self.get_batchwise_prescription_items(ID, PatientID);
        }, function () {
        })
    }
    else {
        $("#PartyName").val(Name);
        $("#PatientID").val(PatientID);
        $("#DoctorName").val(DoctorName);
        $("#DoctorID").val(DoctorID);
        $("#AppointmentProcessID").val(ID);
        $("#DiscountPercentageList").val(DiscountCategoryID);
        UIkit.modal($('#select-appointment-process')).hide();
        self.get_batchwise_prescription_items(ID, PatientID);
    }


};

CounterSales.bind_events = function () {
    var self = CounterSales;
    $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
    $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
    $.UIkit.autocomplete($('#doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $('#doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
    $("body").on("keyup change", "#counter-sales-items-list .Qty, .Rate", self.check_stock);
    $.UIkit.autocomplete($('#add-doctor-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
    $('#add-doctor-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);

    $('body').on('click', '#counter-sales-items-list tbody td:not(.action)', self.show_batch_with_stock);
    $("#btnOKItem").on("click", self.select_item);
    $("#btn-ok-appointment-process").on("click", self.select_patient);
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
    $("#btnOKEmployee").on("click", self.select_doctor);
    $('body').on('change', "#PaymentModeID", self.get_bank_name);
    $("#TypeID").on("change", self.set_batchtype);
    $("#DiscountPercentageList").on('change', self.calculate_discount_Amount);
    $("#BusinessCategoryID").on('change', self.remove_all_items_from_grid);
    $("#btnOKEmployees").on("click", self.select_employees);
    $("body").on("click", "#btnClosePrint", self.print_close);
    $(document).on('keydown', null, 'alt+p', self.on_save_print);
    $(document).on('keydown', null, 'alt+n', self.on_save_new);
    $(document).on('keydown', null, 'alt+d', self.on_save_draft);
    $(document).on('keydown', null, 'alt+l', self.on_close);
    $(document).on('keydown', null, 'alt+c', self.on_cancel);


};

CounterSales.remove_all_items_from_grid = function () {
    var self = CounterSales;
    $("#counter-sales-items-list tbody tr").empty();
    $("#item-count").val($("#counter-sales-items-list tbody tr").length);
    self.calculate_grid_total();
    self.count_items();
};

CounterSales.get_batchwise_prescription_items = function (AppointmentProcessID, PatientID) {
    var self = CounterSales;


    var index, GSTAmount, tr = '';
    $.ajax({
        url: '/Sales/CounterSales/GetBatchwisePrescriptionItems',
        dataType: "json",
        data: {
            AppointmentProcessID: AppointmentProcessID,
            PatientID: PatientID
        },
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                $(response.Items).each(function (i, item) {
                    index = $("#counter-sales-items-list tbody tr").length + 1;
                    var discountamt = 0;
                    var Gst = item.SGSTPercentage + item.CGSTPercentage;
                    var CGST = item.InvoiceQty * (item.CGSTPercentage / 100);
                    var SGST = item.InvoiceQty * (item.SGSTPercentage / 100);
                    var CessAmount = item.InvoiceQty * (item.CessPercentage / 100);
                    var GrossAmount = item.InvoiceQty * item.Rate;
                    var discount = $("#DiscountPercentageList option:selected").data('value');
                    TaxableAmt = GrossAmount * (discount / 100);
                    discountamt = GrossAmount - (GrossAmount * (discount / 100));
                    TaxableAmount = GrossAmount - TaxableAmt;
                    var NetAmount = TaxableAmount + CessAmount + CGST + SGST;
                    item.WareHouseID = 1;
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
                        + '     <input type="hidden" class="Taxableamount" value="' + item.TaxableAmount + '" />'
                        + '</td>'
                        + ' <td class="ItemName">' + item.Name + '</td>'
                        + ' <td>' + item.BatchName + '</d>'
                        + ' <td class="Unit">' + item.Unit + '</td>'
                        //+ ' <td class="ExpiryDate">' + item.ExpiryDateString + '</td>'
                        + ' <td ><input type="text" class="md-input Rate mask-sales-currency" value="' + item.Rate + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="BasicPrice mask-sales-currency " value="' + item.Rate + '" readonly="readonly" /></td>'
                        + ' <td class="action"><input type="text" class="md-input Qty mask-numeric" value="' + item.InvoiceQty + '"  /></td>'
                        + ' <td ><input type="text" class="md-input GrossAmount mask-sales-currency" value="' + GrossAmount + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input TaxableAmount mask-sales-currency" value="' + TaxableAmount + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input GST mask-currency" value="' + Gst + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input CGST mask-sales-currency" value="' + CGST + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input SGST mask-sales-currency" value="' + SGST + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input IGST mask-sales-currency" value="' + 0 + '" readonly="readonly" /></td>'
                        + ' <td class="cess-enabled"><input type="text" class="md-input CessPercentage mask-currency" value="' + item.CessPercentage + '" readonly="readonly" /></td>'
                        + ' <td class="cess-enabled"><input type="text" class="md-input CessAmount mask-sales-currency" value="' + CessAmount + '" readonly="readonly" /></td>'
                        + ' <td ><input type="text" class="md-input NetAmount mask-sales-currency" value="' + NetAmount + '" readonly="readonly" /></td>'
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
            }
        }
    });

};

CounterSales.add_items_to_grid = function (items) {
    var self = CounterSales;
    var index, GSTAmount, tr = '';
    var readonly = '';
    var discount = $("#DiscountPercentageList option:selected").data('value');

    $.each(items, function (i, item) {
        if (item.SalesUnitID == item.UnitID) {
            item.MRP = item.FullPrice;
            item.BasicPrice = item.BasicPrice;
        }
        else {
            item.MRP = item.LoosePrice;
        }
        item = self.get_item_properties(item);
        index = $("#counter-sales-items-list tbody tr").length + 1;
        if (item.IsGSTRegisteredLocation) {
            var Gst = item.CGSTPercentage + item.SGSTPercentage;
        } else {
            GST = 0;
        }
        if (clean($("#IsPriceEditable").val()) == 0) {
            readonly = ' readonly="readonly"';
        }
        if (item.IsGSTRegisteredLocation) {
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
                + '     <input type="hidden" class="DiscountAmount" value="' + item.DiscountAmount + '" />'
                + '     <input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + ' <td class="ItemName">' + item.Name + '</td>'
                + ' <td >' + item.BatchNo + '</d>'
                + ' <td class="Unit">' + item.Unit + '</td>'
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
        } else {
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
                + '     <input type="hidden" class="DiscountAmount" value="' + item.DiscountAmount + '" />'
                + '     <input type="hidden" class="IsGSTRegisteredLocation" value="' + item.IsGSTRegisteredLocation + '" />'
                + '</td>'
                + ' <td class="ItemName">' + item.Name + '</td>'
                + ' <td >' + item.BatchNo + '</d>'
                + ' <td class="Unit">' + item.Unit + '</td>'
                + ' <td class="action"><input type="text" class="md-input Rate mask-sales-currency" value="' + item.MRP + '"' + readonly + '/></td>'
                + ' <td ><input type="text" class="BasicPrice mask-sales-currency " value="' + item.BasicPrice + '" readonly="readonly" /></td>'
                + ' <td class="action"><input type="text" class="md-input Qty mask-numeric" value="' + item.Quantity + '"  /></td>'
                + ' <td ><input type="text" class="md-input GrossAmount mask-sales-currency" value="' + item.GrossAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input TaxableAmount mask-sales-currency" value="' + item.TaxableAmount + '" readonly="readonly" /></td>'
                + ' <td ><input type="text" class="md-input NetAmount mask-sales-currency" value="' + item.NetAmount + '" readonly="readonly" /></td>'
                + ' <td class="uk-text-center action">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        }

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

};

CounterSales.printpdf = function () {
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
};



CounterSales.btnpart = function () {
    var self = CounterSales;
    $.ajax({
        url: '/Reports/Sales/CounterSalesPartNo',
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
};



CounterSales.details = function () {
    var self = CounterSales;
    setTimeout(function () {
        fh_items = $("#counter-sales-items-list").FreezeHeader();
    }, 50);
    $("body").on("click", ".btnpart", self.btnpart);
    $("body").on("click", ".printpdf", self.printpdf);
    $("body").on("click", ".cancel", self.cancel_confirm);
    $(document).on('keydown', null, 'alt+l', self.on_close);
    $(document).on('keydown', null, 'alt+c', self.on_cancel);
    $(document).on('keydown', null, 'alt+r', self.on_print);
    $("body").on("click", ".print", self.print);
    $("body").on("click", ".btnPrint", self.thermal_print);
},

    CounterSales.select_doctor = function () {
        var self = CounterSales;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    CounterSales.select_employees = function () {
        var self = CounterSales;
        var radio = $('#select-all-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#EmployeeName").val(Name);
        $("#EmployeeID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    CounterSales.rules = {
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

                ]
            },

            {
                elements: "#FullOrLoose",
                rules: [
                    { type: form.required, message: "Please select full or loose" },
                ]
            }
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
                elements: ".Qty",
                rules: [
                    { type: form.required, message: "Invalid Item Quantity" },
                    { type: form.non_zero, message: "Invalid Item Quantity" },
                    { type: form.positive, message: "Invalid Item Quantity" },

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
                    {
                        type: function (element) {
                            var NetAmt = $(element).val().replace(/,/g, '');
                            //alert(NetAmt);
                            //$(element).closest("tr").addClass('validateclass');
                            if (NetAmt > 0)
                                return true;
                            else
                                return false;

                        }, message: "Invalid net amount in grid"
                    },
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

                ]
            },

            {
                elements: ".NetAmount",
                rules: [
                    {
                        type: function (element) {
                            var NetAmt = $(element).val().replace(/,/g, '');
                            //alert(NetAmt);
                            //$(element).closest("tr").addClass('validateclass');
                            if (NetAmt > 0)
                                return true;
                            else
                                return false;

                        }, message: "Invalid net amount in grid"
                    },
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
    };

CounterSales.thermal_print = function () {
    var self = CounterSales;

    var dvContainer = $("#dvContainer")
    console.log(dvContainer)
    console.log([dvContainer])
    var divContents = $("#dvContainer").html();
    var printWindow = window.open('', '', 'height=400,width=800');
    printWindow.document.write('<html><head><title>DIV Contents</title>');
    printWindow.document.write('</head><body >');
    printWindow.document.write(divContents);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
};


CounterSales.thermal_print_on_create = function () {
    var self = CounterSales;
    $.ajax({
        url: '/Sales/CounterSales/GetCounterSalesDetails/',
        data: {
            'ID': $("#ID").val()
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            var content = "";
            var $content;
            //$("#tbl-thermal-print tbody").empty();
            var PrintWithItemName = response.Data.PrintWithItemName;
            $(response.Data.Items).each(function (i, item) {
                var slno = (i + 1);
                console.log("hai");
                content += '<tr>'
                    + '<td class="sl-no">' + slno + '</td>'
                    + (PrintWithItemName == true ? '<td class="ItemName">' + item.Name + '</td>' : '<td class="PartsNumber">' + item.PartsNumber + '</td>')
                    + '<td class="mask-sales-currency">' + item.MRP + '</td>'
                    + '<td class="mask-sales-currency">' + item.Qty + '</td>'
                    + '<td class="mask-sales-currency">' + item.NetAmount + '</td>'
                    + '</tr>';
            });
            content += '<tr>'
                + '<td colspan="5">' + "----------------------------------------------" + '</td>'
                + '</tr>';
            content += '<tr>'
                + '<td></td>'
                + '<td colspan="3">' + "Gross Amount" + '</td>'
                + '<td class="mask-sales-currency">' + response.Data.GrossAmount + '</td>'
                + '</tr>';
            if (response.Data.IsGST == 1) {
                content += '<tr>'
                    + '<td></td>'
                    + '<td colspan="3">' + "SGST" + '</td>'
                    + '<td class="mask-sales-currency">' + response.Data.SGSTAmount + '</td>'
                    + '</tr>';
                content += '<tr>'
                    + '<td></td>'
                    + '<td colspan="3">' + "CGST" + '</td>'
                    + '<td class="mask-sales-currency">' + response.Data.CGSTAmount + '</td>'
                    + '</tr>';
            }
            else if (response.Data.IsVat == 1) {
                content += '<tr>'
                    + '<td></td>'
                    + '<td colspan="3">' + "VAT Amount" + '</td>'
                    + '<td class="mask-sales-currency">' + response.Data.TotalVATAmount + '</td>'
                    + '</tr>';
            }
            content += '<tr>'
                + '<td></td>'
                + '<td colspan="3">' + "Cess Amount" + '</td>'
                + '<td class="mask-sales-currency">' + response.Data.CessAmount + '</td>'
                + '</tr>';
            content += '<tr>'
                + '<td></td>'
                + '<td colspan="3">' + "Discount" + '</td>'
                + '<td class="mask-sales-currency">' + response.Data.BillDiscount + '</td>'
                + '</tr>';
            content += '<tr>'
                + '<td></td>'
                + '<td colspan="3">' + "Round Off" + '</td>'
                + '<td class="mask-sales-currency">' + response.Data.RoundOff + '</td>'
                + '</tr>';
            content += '<tr>'
                + '<td colspan="5">' + "----------------------------------------------" + '</td>'
                + '</tr>';
            content += '<tr>'
                + '<td></td>'
                + '<td colspan="3" style="font-weight: bold;"> Net Amount </td>'
                + '<td class="mask-sales-currency" style="font-weight: bold;">' + response.Data.NetAmount + '</td>'
                + '</tr>';
            $content = $(content);
            app.format($content);
            $("#trans-no").text('BillNo:' + response.Data.TransNo);
            //$("#tbl-thermal-print tbody").empty();
            console.log('setting tbody')
            console.log($("#tbl-thermal-print tbody"))
            $(".thermal-tbody").append($content);
            self.thermal_print();
        }

    });

};

