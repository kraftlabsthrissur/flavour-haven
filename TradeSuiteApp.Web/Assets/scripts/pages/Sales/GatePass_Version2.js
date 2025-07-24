
GatePass = {
    init: function () {
        var self = GatePass;

    },

    list: function () {
        $gatePass_list = $('#gate-pass-list');
        if ($gatePass_list.length) {
            $gatePass_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#gate-pass-list tbody td').on('click', function () {
                var Id = $(this).closest('tr').find("td:eq(0) .ID").val();
                window.location = '/Sales/GatePass/Details/' + Id;
            });
            altair_md.inputs();
            var gatePass_list_table = $gatePass_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            gatePass_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    gatePass_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_evnts: function () {
        $("#btnAddInvoiceDetails").on('click', GatePass.get_sales_Invoices);
        $('.btnUpdate').on('click', GatePass.update_delivery_date);
        $('.save-draft-gatepass,.btnSave,.save-gatepass-new').on('click', GatePass.save_gatepass);
        $("body").on('ifChanged', '.include-item', GatePass.include_item);
        $(".cancel").on("click", GatePass.cancel);
        $("body").on('keyup', '.box-no,.can-no,.bag-no', GatePass.sum_box_bag_can_no);
        $("body").on("ifChanged", ".include-item", GatePass.check);
    },
    check: function () {
        var self = GatePass;
        $("#gatepass-invoice-list tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".include-item").prop("checked") == true) {
                $(row).addClass('included');
                $(row).find(".box-no").prop("disabled", false);
                $(row).find(".can-no").prop("disabled", false);
                $(row).find(".bag-no").prop("disabled", false);
            }
            else {
                $(row).find(".box-no").prop("disabled", true);
                $(row).find(".can-no").prop("disabled", true);
                $(row).find(".bag-no").prop("disabled", true);
                $(row).removeClass('included');
            }
        });
        GatePass.sum_box_bag_can_no();
    },
    sum_box_bag_can_no: function () {
        var self = GatePass;
        var boxsum = 0.0;
        var cansum = 0.0;
        var bagsum = 0.0;
        $("#gatepass-invoice-list tbody tr.included").each(function () {
            var row = $(this).closest('tr');
            boxsum += clean($(row).find(".box-no").val());
            cansum += clean($(row).find(".can-no").val());
            bagsum += clean($(row).find(".bag-no").val());
        });
        $("#BagCount").val(bagsum)
        $("#CanCount").val(cansum)
        $("#BoxCount").val(boxsum)
    },


    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('disabled');
        }
        GatePass.count_items();
    },
    count_items: function () {
        var count = $('#gatepass-invoice-list tbody').find('input.include-item:checked').length;
        $('#item-count').val(count);
    },
    cancel: function () {
        $(".save-draft-gatepass,.btnSave,.save-gatepass-new,.cancel,.edit ").css({ 'display': 'none' });
        $.ajax({
            url: '/Sales/GatePass/Cancel',
            data: {
                ID: $("#ID").val(),
                Table: "sales.gatepass"
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Gate pass cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Sales/GatePass/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");

                    $(".save-draft-gatepass,.btnSave,.save-gatepass-new.cancel,.edit ").css({ 'display': 'block' });
                }
            },
        });

    },
    get_sales_Invoices: function () {
        var self = GatePass;
        self.error_count = 0;
        self.error_count = self.validate_search();
        if (self.error_count > 0) {
            return;
        }
        var FromDate = $("#FromDate").val();
        var ToDate = $("#ToDate").val();
        var Type = $("#Type").val();
        var url = '/Sales/GatePass/GetGatePassDocumentBetweenDates';
        $.ajax({
            url: url,
            data: {
                FromDate: FromDate,
                ToDate: ToDate,
                Type: Type
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var $gatepass_invoice_list = $('#gatepass-invoice-list tbody');
                $gatepass_invoice_list.html('');
                var tr = '';
                $.each(response, function (i, sales_invoice) {
                    tr += "<tr>"
                                + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                                + "<td class='checked uk-text-center' data-md-icheck> "
                                + "<input type='hidden' class='trans-id' value='" + sales_invoice.ID + "'/>"
                                + "<input type='hidden' class='trans-type' value='" + sales_invoice.Type + "'/>"
                                + "<input type='checkbox' class='include-item'/>"
                                + "</td>"
                                + '<td>' + sales_invoice.TransNo + '</td>'
                                + '<td class= "trans-date">' + sales_invoice.TransDate + '</td>'
                                + '<td class= "name">' + sales_invoice.Name + '</td>'
                                + '<td class= "net-amount">' + sales_invoice.Amount + '</td>'
                                + '<td class= "area">' + sales_invoice.Area + '</td>'
                                + '<td>' + "<input type='text' class = 'md-input  mask-postive  box-no' value='" + sales_invoice.NoOfboxes + "' disabled/>" + "</td>"
                                + '<td>' + "<input type='text' class = 'md-input  mask-postive  can-no' value='" + sales_invoice.NoOfCans + "' disabled/>" + "</td>"
                                + '<td>' + "<input type='text' class = 'md-input  mask-postive  bag-no' value='" + sales_invoice.NoOfBags + "' disabled/>" + "</td>"
                        + "</tr>";
                });
                var $tr = $(tr);
                app.format($tr);
                $('#gatepass-invoice-list tbody').append($tr);
            }
        });
    },
    save_gatepass: function () {
        var self = GatePass;
        var IsNew = false, IsDraft = false;
        self.error_count = 0;
        if ($(this).hasClass("save-draft-gatepass")) {
            IsDraft = true;
        }
        if ($(this).hasClass("save-gatepass-new")) {
            IsNew = true;
        }
        self.error_count = ((IsDraft == true) ? self.validate_draft() : self.validate_form());
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave, .save-draft-gatepass, .save-gatepass-new,.cancel').css({ 'display': 'none' });
        modal = self.get_data(IsDraft);
        $.ajax({
            url: '/Sales/GatePass/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("GatePass created successfully");
                    if (IsNew == true) {
                        setTimeout(function () {
                            window.location = "/Sales/GatePass/Create";
                        }, 1000);
                    }
                    else {
                        setTimeout(function () {
                            window.location = "/Sales/GatePass/Index";
                        }, 1000);
                    }
                }
            }
        });
    },
    validate_draft: function () {
        var self = GatePass;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_search: function () {
        var self = GatePass;
        if (self.rules.on_search.length) {
            return form.validate(self.rules.on_search);
        }
        return 0;
    },
    validate_deliverydate_update: function () {
        var self = GatePass;
        if (self.rules.on_deliverydate_update.length) {
            return form.validate(self.rules.on_deliverydate_update);
        }
        return 0;

    },
    validate_form: function () {
        var self = GatePass;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;

    },
    rules: {
        on_deliverydate_update: [
        {
            elements: "#gatepass-invoice-list tr.included .delivery-date",
            rules: [
                { type: form.past_date, message: "Invalid delivery date" },
            ]
        },
        ],
        on_submit: [
            {
                elements: "#Salesman",
                rules: [
                    { type: form.required, message: "Please enter salesman" },
                ]
            },
            {
                elements: "#FromDate",
                rules: [
                    { type: form.required, message: "Invoice from date required" },
                ]
            },
            {
                elements: "#ToDate",
                rules: [
                    { type: form.required, message: "Invoice from date required" },
                ]
            },
            {
                elements: "#VehicleNoID",
                rules: [
                    { type: form.required, message: "Plase select vehicle number" },
                ]
            },
            {
                elements: "#DespatchDateTime",
                rules: [
                    { type: form.required, message: "Plase enter a valid despatch date" },
                ]
            },
            {
                elements: "#Area",
                rules: [
                    { type: form.required, message: "Plase enter transporting agency" },
                ]
            },
            {
                elements: "#IssuedBy",
                rules: [
                    { type: form.required, message: "Plase enter transporting agency" },
                ]
            },
            {
                elements: "#gatepass-invoice-list .included .trans-id",
                rules: [
                    { type: form.required, message: "Please enter atleast one sales invoice" },
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
                 elements: "#DriverID",
                 rules: [
                     { type: form.required, message: "Plase select driver" },
                 ]
             },
        ],
        on_draft: [
            {
                elements: "#Salesman",
                rules: [
                    { type: form.required, message: "Please enter salesman" },
                ]
            },
            {
                elements: "#FromDate",
                rules: [
                    { type: form.required, message: "Invoice from date required" },
                ]
            },
            {
                elements: "#ToDate",
                rules: [
                    { type: form.required, message: "Invoice from date required" },
                ]
            },
            {
                elements: "#VehicleNoID",
                rules: [
                    { type: form.required, message: "Plase enter vehicle number" },
                ]
            },
            {
                elements: "#DespatchDateTime",
                rules: [
                    { type: form.required, message: "Plase enter a valid despatch date" },
                ]
            },
            {
                elements: "#Area",
                rules: [
                    { type: form.required, message: "Plase enter transporting agency" },
                ]
            },
            {
                elements: "#IssuedBy",
                rules: [
                    { type: form.required, message: "Plase enter transporting agency" },
                ]
            },
            {
                elements: "#gatepass-invoice-list .included .trans-id",
                rules: [
                    { type: form.required, message: "Please enter atleast one sales invoice" },
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
                elements: "#DriverID",
                rules: [
                    { type: form.required, message: "Plase select driver" },
                ]
            },
        ],
        on_search: [
           {
               elements: "#Type",
               rules: [
                   { type: form.required, message: "Please Select Document Type" },
               ]
           },
        ]
    },
    update_delivery_date: function () {
        var self = GatePass;
        self.error_count = 0;
        self.error_count = self.validate_deliverydate_update();
        if (self.error_count > 0) {
            return;
        }
        modal = self.get_data_fordeliverydate();
        $.ajax({
            url: '/Sales/GatePass/UpdateDeliveryDate',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("GatePass delivery date successfully");
                    setTimeout(function () {
                        window.location = "/Sales/GatePass/Index";
                    }, 1000);
                }
                else {
                    app.show_error("Updation failed");

                }
            }
        });
    },
    get_data_fordeliverydate: function () {
        var self = GatePass;
        var model = {
            GatepassItems: []
        };
        $('#gatepass-invoice-list tbody tr.included').each(function () {
            object = {
                GatePassTransID: $(this).find('.gatepass-trans-id').val(),
                DeliveryDate: $(this).find('.delivery-date').val(),
            };
            model.GatepassItems.push(object);
        });
        return model;
    },
    get_data: function (IsDraft) {
        var model = {
            ID: $("#ID").val(),
            TransNo: $("#TransNo").val(),
            TransDate: $("#TransDate").val(),
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Type: $("#Type").val(),
            Salesman: $("#Salesman").val(),
            VehicleNoID: $("#VehicleNoID").val(),
            DespatchDateTime: $("#DespatchDateTime").val(),
            DriverID: $("#DriverID").val(),
            DrivingLicense: $("#DrivingLicense").val(),
            VehicleOwner: $("#VehicleOwner").val(),
            TransportingAgency: $("#TransportingAgency").val(),
            HelperName: $("#HelperName").val(),
            Area: $("#Area").val(),
            StartingKilometer: $("#StartingKilometer").val(),
            IssuedBy: $("#IssuedBy").val(),
            Time: $("#Time").val(),
            BagCount: clean($("#BagCount").val()),
            CanCount: clean($("#CanCount").val()),
            BoxCount: clean($("#BoxCount").val()),
            IsDraft: IsDraft,
            GatepassItems: []
        };
        var object = {};
        $('#gatepass-invoice-list tbody tr.included').each(function () {
            object = {
                ID: $(this).find('.trans-id').val(),
                Type: $(this).find('.trans-type').val(),
                Area: $(this).find('.area').text(),
                NoOfboxes: clean($(this).find('.box-no').val()),
                NoOfCans: clean($(this).find('.can-no').val()),
                NoOfBags: clean($(this).find('.bag-no').val()),
                //PPSNO: clean($(this).find('.ppsno').text())
            };

            model.GatepassItems.push(object);
        });
        return model;
    },
}