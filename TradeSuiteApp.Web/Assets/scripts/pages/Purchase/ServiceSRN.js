var LocationList = [];
var DepartmentList = [];
var EmployeeList = [];
var CompanyList = [];
var ProjectList = [];
var freeze_header;
SRN = {
    init: function () {
        var self = SRN;

        freeze_header = $("#service-srn-items").FreezeHeader();

        supplier.supplier_list('service');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#DeliveryChallanNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });

        $('#purchase-order-list').SelectTable({
            modal: "#select_po",
            selectFunction:self.Load_Trans_By_UnProcessed_POService,
            initiatingElement: "#PurchseOrder",
            startFocusIndex: 3,
            selectionType: "checkbox",
        });

        self.bind_events();

        $currObj = this;
        $currObj.OldSearchedSupplierName = '';          //For searching
        $currObj.SupplierChanged = false;
        $currObj.POChanged = false;
    },

    details: function () {
        var self = SRN;
        freeze_header = $("#service-srn-items").FreezeHeader();
    },

    list: function () {
        var self = SRN;
        $('#tabs-srn').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
        //self.tabbed_list("draft");
        //self.tabbed_list("to-be-invoiced");
        //self.tabbed_list("partially-invoiced");
        //self.tabbed_list("invoiced");
        //self.tabbed_list("cancelled");
    },

    tabbed_list: function (type) {
        var self = SRN;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#srn-draft-list');
                break;
            case "to-be-invoiced":
                $list = $('#srn-to-be-invoiced-list');
                break;
            case "partially-invoiced":
                $list = $('#srn-partially-invoiced-list');
                break;
            case "invoiced":
                $list = $('#srn-invoiced-list');
                break;
            case "cancelled":
                $list = $('#srn-cancelled-list');
                break;
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/ServiceSRN/GetSRNList?type=" + type;

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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    { "data": "DeliveryChallanNo", "className": "DeliveryChallanNo" },
                    { "data": "DeliveryChallanDate", "className": "DeliveryChallanDate" },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/ServiceSRN/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    purchase_order_list: function () {
        $purchase_order_list = $('#purchase-order-list');

        if ($purchase_order_list.length) {
            $purchase_order_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                if (!$(this).find('input').length)
                    $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();

            var purchase_order_list_table = $purchase_order_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "bSort": false
            });
            purchase_order_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    purchase_order_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    supplier_list: function () {
        var $list = $('#supplier-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                supplier_list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': SRN.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', SRN.set_supplier_details);
        $(".btnSaveSRN").on('click', function () { SRN.save_confirm(false); });
        $(".btnSaveAndNewSRN").on('click', function () { SRN.save(false, true); });
        $(".btnSaveDraftSRN").on('click', function () { SRN.save(true); });
        SRN.Load_All_DropDown();
        //$(".btnSelectPOForService").click(function () { SRN.SearchPO_For_Service($currObj); });
        $("#btnOkPoList").click(SRN.Load_Trans_By_UnProcessed_POService);
        $("#btnOkSplierList").click(SRN.select_supplier);
        $("body").on('ifChanged', '.include-item', SRN.include_item);
        $(".cancel").on("click", SRN.cancel_confirm);
        $('#supplier-list').on('datatable.changed', function () {
            select_table.refresh();
        });
        $('#select-supplier').on('show.uk.modal', function () {
            select_table.setFocus();
        });

        $("#btnOKSupplier").on('click', SRN.select_supplier);
        $("#DeliveryChallanNo").on("change", SRN.get_invoice_number_count);
        $("body").on("keyup change", "#service-srn-items tbody .ReceivedQty", SRN.set_accepted_qty);
        $("body").on("keyup change", "#service-srn-items tbody .AcceptedQty", SRN.set_accepted_value);
    },
    set_accepted_value: function () {
        var row = $(this).closest('tr');
        var acceptedqty = clean($(row).find('.AcceptedQty').val());
        var porate = clean($(row).find('.PORate').val());
        var acceptedvalue = acceptedqty * porate;    
        $(row).find('.AcceptedValue').val(acceptedvalue);
    },
    set_accepted_qty: function () {
        var row = $(this).closest('tr');
        var acceptedqty = clean($(row).find('.ReceivedQty').val());
        var porate = clean($(row).find('.PORate').val());
        var acceptedvalue = acceptedqty * porate;
        $(row).find('.AcceptedQty').val(acceptedqty);
        $(row).find('.AcceptedValue').val(acceptedvalue);
    },
    cancel_confirm: function () {
        var self = SRN
        app.confirm_cancel("Do you want to cancel", function () {
            self.cancel();
        }, function () {
        })
    },

    save_confirm: function () {
        var self = SRN
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_invoice_number_count: function (release) {

        $.ajax({
            url: '/Purchase/ServiceSRN/GetInvoiceNumberCount',
            data: {
                Hint: $("#DeliveryChallanNo").val(),
                Table: "ServiceReceiptNote",
                SupplierID: $('#SupplierID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                count = response.data;
                $("#invoice-count").val(count);
            }
        });
    },

    include_item_edit: function () {
        var self = SRN;
        $('.ChkSRN').each(function () {
            $(this).prop('checked', 'checked');
            $(this).closest('.icheckbox_md').addClass('checked');
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        });
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input, select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
            $(this).removeAttr('readonly');
        }
        SRN.count_items();
    },
    count_items: function () {
        var count = $('#service-srn-items tbody').find('input.include-item:checked').length;
        $('#item-count').val(count);
    },
    select_supplier: function () {
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        if (($('#service-srn-items tbody tr').length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Items will be removed", function () {
                $('#service-srn-items tbody').empty();

                $("#SupplierID").val(ID);
                $("#SupplierName").val(Name);
                $("#SupplierLocation").val(Location);
                $("#StateId").val(StateID);
                $("#IsGSTRegistred").val(IsGSTRegistered);
                $('#grn-items-list tbody').html('');
                SRN.populate_purchase_orders();
            });
        } else {
            $("#SupplierID").val(ID);
            $("#SupplierName").val(Name);
            $("#SupplierLocation").val(Location);
            $("#StateId").val(StateID);
            $("#IsGSTRegistred").val(IsGSTRegistered);
            $('#grn-items-list tbody').html('');
            SRN.populate_purchase_orders();
        }
        SRN.get_invoice_number_count();
        UIkit.modal($('#select-supplier')).hide();
    },
    set_supplier_details: function (event, item) {   // on select auto complete item
        if (($('#service-srn-items tbody tr ').length > 0) && (item.id != $("#SupplierID").val())) {
            app.confirm("Items will be removed", function () {
                $('#service-srn-items tbody').empty();
                $("#SupplierName").val(item.name);
                $("#SupplierLocation").val(item.location);
                $("#SupplierID").val(item.id);
                $("#StateId").val(item.stateId);
                $("#IsGSTRegistred").val(item.isGstRegistered);

                SRN.populate_purchase_orders();
                $('#grn-items-list tbody').html('');

            });

        } else {
            $("#SupplierName").val(item.name);
            $("#SupplierLocation").val(item.location);
            $("#SupplierID").val(item.id);
            $("#StateId").val(item.stateId);
            $("#IsGSTRegistred").val(item.isGstRegistered);
            $("#SupplierReferenceNo").focus();
            SRN.populate_purchase_orders();

        }
        SRN.get_invoice_number_count();
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getServiceSupplierForAutoComplete',
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
    populate_purchase_orders: function () {
        $.ajax({
            url: '/Purchase/ServiceSRN/GetUnprocessedPoService/',
            dataType: "json",
            data: { SupplierId: $("#SupplierID").val() },
            type: "POST",
            success: function (response) {
                if ($.fn.DataTable.isDataTable('#purchase-order-list')) {
                    $('#purchase-order-list').DataTable().destroy();
                }
                var $purchase_order_list = $('#purchase-order-list tbody');
                $purchase_order_list.html("");
                if (response.Status == "success") {
                    var tr = '';
                    $.each(response.data, function (i, purchase_order) {
                        tr += "<tr>"
                            + "<td class='uk-text-center'>" + (i + 1)
                            + "<input type=hidden class=Date value='" + purchase_order.PurchaseOrderDate + "'/>"
                            + "</td>"
                            + "<td class='uk-text-center checked' data-md-icheck>"
                            + "<input type='checkbox' class='purchase-order-id' value='" + purchase_order.ID + "'/>"
                            + "</td>"
                            + "<td>" + purchase_order.PurchaseOrderNo + "</td>"
                            + "<td>" + purchase_order.PurchaseOrderDate + "</td>"
                            + "<td>" + purchase_order.SupplierName + "</td>"
                            + "<td>" + purchase_order.RequestedBy + "</td>"
                            + "<td class='mask-currency'>" + purchase_order.NetAmt + "</td>"
                        + "</tr>";

                    });
                    var $tr = $(tr);
                    app.format($tr);
                    $purchase_order_list.append($tr);
                    SRN.purchase_order_list();
                } else {

                }

            },
        });
    },
    Load_All_DropDown: function () {
        $.ajax({
            url: '/Purchase/ServiceSRN/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (data) {
                if (data != null) {
                    DepartmentList = data.DDLDepartment;
                    LocationList = data.DDLLocation;
                    CompanyList = data.DDLInterCompany;
                    ProjectList = data.DDLProject;
                    EmployeeList = data.DDLEmployee;
                }
            }
        });
    },
    Load_Trans_By_UnProcessed_POService: function () {
        var self = SRN;
        if ($("#service-srn-items tbody tr").length > 0 && $("table#purchase-order-list tbody .purchase-order-id:checked").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#service-srn-items tbody').empty();
                self.add_to_grid();
            });
        }
        else {
            self.add_to_grid();
        }
    },
    add_to_grid: function () {
        var UnProcPoList = [];
        $("table#purchase-order-list tbody .purchase-order-id:checked").each(function () {
            UnProcPoList.push($(this).val())
        });
        $('#item-count').val(0);
        if ($(".SRNtbody .rowSRN").length > 0 && UnProcPoList.length > 0) {
            app.confirm("Selected Items will be removed", function () {
                maxDate = $("#purchase-order-list .purchase-order-id:checked").eq(0).closest("tr").find(".Date").val();

                $("#purchase-order-list  tbody tr .purchase-order-id:checked").each(function () {

                    var currRow = $(this).parents('tr');

                    CurrentDate = $(currRow).find('.Date').val();
                    if (Date.parse(CurrentDate) > Date.parse(maxDate)) {
                        maxDate = CurrentDate;
                    }


                })

                $('#ServicePODate').val(maxDate);
                SRN.get_SRN_items(UnProcPoList);

            })

        } else {
            maxDate = $("#purchase-order-list .purchase-order-id:checked").eq(0).closest("tr").find(".Date").val();

            $("#purchase-order-list  tbody tr .purchase-order-id:checked").each(function () {

                var currRow = $(this).parents('tr');

                CurrentDate = $(currRow).find('.Date').val();
                if (Date.parse(CurrentDate) > Date.parse(maxDate)) {
                    maxDate = CurrentDate;
                }


            })

            $('#ServicePODate').val(maxDate);
            SRN.get_SRN_items(UnProcPoList);

        }

    },
    get_SRN_items: function (UnProcPoList) {
        $(".SRNtbody .rowSRN").each(function () {
            $(this).remove();
        });
        if (UnProcPoList.length > 0) {
            $currObj.POChanged = $currObj.SupplierChanged = false;
            $.ajax({
                url: '/ServiceSRN/GetPoTransByPrId',
                data: { PoIdList: UnProcPoList },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                //dataType: "json",

                success: function (response) {
                    var $response = $(response);
                    app.format($response);
                    $('#service-srn-items tbody').html($response);
                    freeze_header.resizeHeader();
                },
            });
        }
    },
    Add_Trans_To_Grid: function (item) {
        var SerialNo = $(".SRNtbody .rowsrn").length + 1;

        var html = '<tr class="rowsrn">' +
           '<td class="uk-text-center">' + SerialNo + '</td>' +
           '<td class="checked uk-text-center" data-md-icheck> <input type="checkbox" class="ChkSRN" /></td>' +
           '<td><input type="hidden"class="POSId" value="' + item.POServiceID + '"> <input type="hidden"class="POSTransID" value="' + item.POServiceTransID + '"><input type="hidden" class="ItemId" value="' + item.ItemID + '"></td>' +
           '<td>' + item.Unit + '</td>' +
           '<td>' + item.PurchaseOrderNo + '</td>' +
           '<td class="mask-qty PurchaseOrderQty"  >' + item.PurchaseOrderQty + '</td>' +
           '<td><input type="text" class="md-input mask-qty ReceivedQty" value="' + item.ReceivedQty + '" /></td>' +
           '<td><input type="text" class="md-input mask-qty AcceptedQty" value="' + item.AcceptedQty + '" /></td>' +
           '<td> <select class="md-input label-fixed clLocation"> ' + SRN.Build_Select(LocationList, item.ServiceLocationID) + '</select></td>' +
           '<td> <select class="md-input label-fixed clDepartment">' + SRN.Build_Select(DepartmentList, item.DepartmentID) + ' </select></td>' +
         '<td> <select class="md-input label-fixed clEmployee">' + SRN.Build_Select(EmployeeList, item.EmployeeID) + ' </select></td>' +
          //  '<td> <select class="md-input label-fixed employee"> ' + SRN.Build_Select(EmployeeList, item.EmployeeID) + '</select></td>' +

           '<td> <select class="md-input label-fixed clCompany"> ' + SRN.Build_Select(CompanyList, item.CompanyID) + '</select></td>' +
           '<td> <select class="md-input label-fixed clProject">' + SRN.Build_Select(ProjectList, item.ProjectID) + ' </select></td>' +
           '<td><input type="text" class="md-input clRemarks"  /></td>' +
           '</tr>';
        var $html = $(html);
        app.format($html);
        $(".SRNtbody").html($html);
    },
    Build_Select: function buildSelect(options, selected) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';

        // console.log(options);
        for (var i = 0; i < options.length; i++) {
            //alert(options[i].Text);
            if (options[i].Value == selected) {
                //$option.attr('selected', 'selected');
                $option = '<option selected value="' + options[i].Value + '">' + options[i].Text + '</option>';
            } else {
                $option = '<option value="' + options[i].Value + '">' + options[i].Text + '</option>';
            }


            $select.append($option);
        }

        return $select.html();

    },
    cancel: function () {
        $(".btnSaveSRN, .btnSaveDraftSRN,.btnSaveAndNewSRN,.cancel ").css({ 'display': 'none' });

        $.ajax({
            url: '/Purchase/ServiceSRN/Cancel',
            data: {
                ID: $("#ID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("SRN cancelled successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/ServiceSRN/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to cancel.");
                    $(".btnSaveSRN, .btnSaveDraftSRN,.btnSaveAndNewSRN,.cancel ").css({ 'display': 'block' });
                }
            },
        });

    },

    save: function (isDraft, isSaveAndNew) {
        var self = SRN;
        self.error_count = 0;
        if (!isDraft) {
            self.error_count = self.validate_form();
        }
        else {
            self.error_count = self.validate_draft();
        }
        if (self.error_count == 0) {
            SRN.Save_SRN(isDraft, isSaveAndNew);
        }
    },
    validate_form: function () {
        var self = SRN;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_draft: function () {
        var self = SRN;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_submit: [
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.required, message: "Please select a Supplier" },
                    { type: form.non_zero, message: "Please select a Supplier" }
                ]
            },
            {
                elements: "#DeliveryChallanNo ",
                rules: [
                    { type: form.required, message: "DC/Invoice Number Required" },
                        {
                            type: function (element) {
                                var error = false;
                                var count = clean($('#invoice-count').val());
                                if (count > 0)
                                    error = true;
                                return !error;
                            }, message: 'Invoice number has been already entered for this supplier'
                        },
                ]
            },
            {
                elements: "#DeliveryChallanDate",
                rules: [
                    { type: form.past_date, message: "DC/Invoice Date Invalid" },
                    { type: form.required, message: "DC/Invoice Date Required" },
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);

                            var po_date = $('#ServicePODate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);

                            return used_date.getTime() >= po_datesplit.getTime();

                        }, message: "Invoice date must be within maximum of selected purchase order date and current date"
                    }
                ]
            },
            {
                elements: "#service-srn-items .ReceivedQty.included",
                rules: [
                    { type: form.required, message: "Please enter received quantity" },
                    { type: form.non_zero, message: "Please enter received quantity" },
                    { type: form.numeric, message: "Please enter numeric value for received quantity" },
                    { type: form.positive, message: "Please enter positive value for received quantity" },
                    {
                        type: function (element) {
                            var received_quantity = clean($(element).val());
                            var quantity = clean($(element).closest('tr').find('.PurchaseOrderQty').text());
                            var qty_tolerance_percent = clean($(element).closest('tr').find('.qty-tolerance-percent.included').val());
                            //var pending_po_quantity = $(element).closest('tr').find('.pending-po-quantity.included').val();
                            var allowed_quantity = parseFloat(quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                            return parseFloat($(element).val()) <= allowed_quantity;

                        }, message: "Received quantity greater than tolerance"
                    },
                ]
            },
            {
                elements: "#service-srn-items .AcceptedQty.included",
                rules: [
                    //{ type: form.required, message: "Please enter accepted quantity" },
                    // { type: form.non_zero, message: "Please enter accepted quantity" },
                    { type: form.numeric, message: "Please enter numeric value for accepted quantity" },
                    { type: form.positive, message: "Please enter positive value for accepted quantity" },
                    {
                        type: function (element) {
                            return clean($(element).val()) <= clean($(element).closest('tr').find('.ReceivedQty.included').val());
                        }, message: "Accepted quantity should not be greater than received quantity "
                    },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please choose atleast one item" },
                    { type: form.required, message: "Please choose atleast one item" },
                ]
            },
        ],

        on_draft: [
       {
           elements: "#SupplierID",
           rules: [
               { type: form.required, message: "Please select a Supplier" },
               { type: form.non_zero, message: "Please select a Supplier" }
           ]
       },
       {
           elements: "#service-srn-items .ReceivedQty.included",
           rules: [
               { type: form.required, message: "Please enter received quantity" },
               { type: form.non_zero, message: "Please enter received quantity" },
               { type: form.numeric, message: "Please enter numeric value for received quantity" },
               { type: form.positive, message: "Please enter positive value for received quantity" },
               {
                   type: function (element) {
                       var received_quantity = clean($(element).val());
                       var quantity = clean($(element).closest('tr').find('.PurchaseOrderQty').text());
                       var qty_tolerance_percent = clean($(element).closest('tr').find('.qty-tolerance-percent.included').val());
                       //var pending_po_quantity = $(element).closest('tr').find('.pending-po-quantity.included').val();
                       var allowed_quantity = parseFloat(quantity) + parseFloat(quantity) * parseFloat(qty_tolerance_percent) / 100;
                       return parseFloat($(element).val()) <= allowed_quantity;

                   }, message: "Received quantity greater than tolerance"
               },
           ]
       },
        {
            elements: "#service-srn-items .AcceptedQty.included",
            rules: [
                //{ type: form.required, message: "Please enter accepted quantity" },
                // { type: form.non_zero, message: "Please enter accepted quantity" },
                { type: form.numeric, message: "Please enter numeric value for accepted quantity" },
                { type: form.positive, message: "Please enter positive value for accepted quantity" },
                {
                    type: function (element) {
                        return clean($(element).val()) <= clean($(element).closest('tr').find('.ReceivedQty.included').val());
                    }, message: "Accepted quantity should not be greater than received quantity "
                },
            ]
        },
       {
           elements: "#item-count",
           rules: [
               { type: form.non_zero, message: "Please choose atleast one item" },
               { type: form.required, message: "Please choose atleast one item" },
           ]
       },
        ]
    },
    Save_SRN: function (isDraft, isSaveAndNew) {
        var master = SRN.get_master_model(isDraft);
        var Trans = SRN.get_Trans_model();
        var url = '/Purchase/ServiceSRN/save';
        if (isDraft == true) {
            url = '/Purchase/ServiceSRN/SaveAsDraft';
        }
        $(".btnSaveSRN, .btnSaveDraftSRN,.btnSaveAndNewSRN ").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { _master: master, _trans: Trans },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.Status == "success") {
                    //clearAllControls();
                    app.show_notice("Successfully saved ");
                    if (isSaveAndNew) {
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                    }
                    else {
                        setTimeout(function () {
                            window.location = "/Purchase/ServiceSRN/Index";
                        }, 1000);
                    }
                }

                else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveSRN, .btnSaveDraftSRN,.btnSaveAndNewSRN ").css({ 'display': 'block' });
                    ;
                }
            },
        });
    },
    get_master_model: function (isDraft) {
        var model = {
            SRNNumber: $("#SRNNumber").val(),
            Date: new $("#Date").val(),
            SupplierID: clean($("#SupplierID").val()),
            DeliveryChallanNo: $("#DeliveryChallanNo").val(),
            DeliveryChallanDate: $("#DeliveryChallanDate").val(),
            IsDraft: isDraft,
            ID: $("#ID").val()
        }
        return model;
    },
    get_Trans_model: function () {
        var ProductsArray = [];
        $("tbody .rowsrn.included").each(function () {
            var e = $(this);
            var POServiceID = clean(e.find(".POSId").val());
            var POServiceTransID = clean(e.find(".POSTransID").val());
            var ItemID = clean(e.find(".ItemId").val());
            var PurchaseOrderQty = clean(e.find(".PurchaseOrderQty").text());
            var ReceivedQty = clean(e.find(".ReceivedQty").val());
            var AcceptedQty = clean(e.find(".AcceptedQty").val());
            var ServiceLocationID = clean(e.find(".clLocation").val());
            var DepartmentID = clean(e.find(".clDepartment").val());
            var EmployeeID = clean(e.find(".clEmployee").val());
            var CompanyID = clean(e.find(".clCompany").val());
            var ProjectID = clean(e.find(".clProject").val());
            var Remarks = e.find(".clRemarks").val();
            var TolaranceQty = clean(e.find(".tolaranceQty").val());


            ProductsArray.push({
                POServiceID: POServiceID,
                POServiceTransID: POServiceTransID,
                ItemID: ItemID,
                PurchaseOrderQty: PurchaseOrderQty,
                ReceivedQty: ReceivedQty,
                AcceptedQty: AcceptedQty,
                ServiceLocationID: ServiceLocationID,
                DepartmentID: DepartmentID,
                EmployeeID: EmployeeID,
                CompanyID: CompanyID,
                ProjectID: ProjectID,
                Remarks: Remarks,
                TolaranceQty: TolaranceQty
            });

        });
        return ProductsArray;
    },

};