var item_list;
var select_table;
var fh_items;
SupplierDebitNote = {
    init: function () {
        var self = SupplierDebitNote;

        supplier.supplier_list('Payment');
        select_table = $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#ReferenceInvoiceNumber",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        })
        item_list = Item.debit_and_credit_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.freeze_headers();
        self.bind_events();
        $("#item-count").val($("#supplierdebitnote-item-list tbody tr").length);
    },
    freeze_headers: function () {
        fh_items = $("#supplierdebitnote-item-list").FreezeHeader();
    },
    details: function () {
        var self = SupplierDebitNote;
        $("body").on("click", ".print", self.print);
        $("body").on("click", ".printpdf", self.printpdf);
        self.freeze_headers();
    },

    print: function () {
        var self = SupplierDebitNote;
        $.ajax({
            url: '/Accounts/SupplierDebitNote/Print',
            data: {
                ID: $("#ID").val()
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
        var self = SupplierDebitNote;
        $.ajax({
            url: '/Reports/Accounts/SupplierDebitNotePrintPdf',
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
        var self = SupplierDebitNote;
        $('#tabs-supplier-debitnote').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "supplier-debit-note":
                $list = $('#supplierdebitnote-list');
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

            var url = "/Accounts/SupplierDebitNote/GetSupplierDebitNoteList?type=" + type;
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

                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "Supplier", "className": "Supplier" },
                   { "data": "ReferenceInvoiceNumber", "className": "ReferenceInvoiceNumber" },
                   { "data": "ReferenceDocumentDate", "className": "ReferenceDocumentDate" },
                  
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
                        app.load_content("/Accounts/SupplierDebitNote/Details/" + Id);

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

        var self = SupplierDebitNote;
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#btnOKSupplier").on('click', self.select_supplier);

        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        //$.UIkit.autocomplete($('#PurchaseReturnNo-autocomplete'), { 'source': self.get_purchase_return_no, 'minLength': 1 });
        //$('#PurchaseReturnNo-autocomplete').on('selectitem.uk.autocomplete', self.set_purchase_return_no);

        $("#Qty").on("keyup change", self.update_amount);
        $("#Rate").on("keyup change", self.update_amount);

        $("body").on("keyup change", "#supplierdebitnote-item-list tbody tr .SGSTAmount", self.update_item);
        $("body").on("keyup change", "#supplierdebitnote-item-list tbody tr .IGSTAmount", self.update_item);
        $("body").on("keyup change", "#supplierdebitnote-item-list tbody tr .CGSTAmount", self.update_item);

        $("#btnAddItem").on("click", self.add_item);
        $("body").on("click", ".remove-item", self.remove_item);

        $("body").on("change", "#DepartmentID", self.get_employee);
        $("body").on('click', '.btnSaveDn,.btnSaveASDraft,.btnSaveDnNew', self.is_month_closed);
        $("#btnOKItem").on("click", self.select_item);
        $("#RoundOff").on("keyup change", self.update_netamount);
    },
    get_employee: function () {

        $.ajax({
            url: '/Masters/Employee/GetEmployeeByDepartment/',
            data: { DepartmentID: $("#DepartmentID option:selected").val() },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#EmployeeID").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#EmployeeID").append(html);
            }
        });

    },


    is_month_closed: function () {
        var self = SupplierDebitNote;
        var classs = this.className;
        $.ajax({
            url: '/Masters/PeriodClosing/IsMonthClosed',
            data: {
                Type: 'SDNStatus',
                Date: $("#Date").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    if (response.data != "Open") {
                        app.show_error("This Month Is Closed");
                    }
                    else {
                        if (classs == "md-btn btnSaveASDraft") {
                            self.save_as_draft();
                        }
                        else if (classs == "md-btn btnSaveDnNew") {
                            self.save_new();
                        }
                        else {
                            self.save_confirm();
                        }
                    }
                }
                else {
                    return;
                }
            },
        });
    },

    save_confirm: function () {
        var self = SupplierDebitNote
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save(false,false);
        }, function () {
        })
    },

    //get_purchase_return_no: function (release) {
    //    if ($("#SupplierID").val() == 0) {
    //        app.show_error("Please select Supplier");

    //    } else if ($("#ItemID").val() == 0) {
    //        app.show_error("Please select Item");

    //    }
    //    else {
    //        $.ajax({
    //            url: '/Purchase/PurchaseReturn/GetPurchaseReturnAutocompleteByID',
    //            data: {
    //                Term: $('#PurchaseReturnNo').val(),
    //                ItemID: $('#ItemID').val(),
    //                SupplierID: $('#SupplierID').val()
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            success: function (data) {
    //                release(data);
    //            }
    //        });
    //    }
    //},
    //set_purchase_return_no: function (event, item) {
    //    self = SupplierDebitNote;
    //    $("#PurchaseReturnNo").val(item.ReturnNo);
    //    $("#PurchaseReturnID").val(item.id);
    //},

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
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
    set_supplier_details: function (event, item) {
        var self = SupplierDebitNote;
        if (($("#supplierdebitnote-item-list tbody tr").length > 0) && (item.id != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#supplierdebitnote-item-list tbody').empty();
                $("#SupplierName").val(item.name);
                $("#SupplierLocation").val(item.location);
                $("#SupplierID").val(item.id);
                $("#StateID").val(item.stateID);
                $("#IsGSTRegistred").val(item.isGstRegistered);
                $("#ReferenceInvoiceNumber").val('');
                $("#ReferenceDocumentDate").val('');
                self.calculate_grid_total();
            })

        }
        else {
            $("#SupplierName").val(item.name);
            $("#SupplierLocation").val(item.location);
            $("#SupplierID").val(item.id);
            $("#StateID").val(item.stateID);
            $("#IsGSTRegistred").val(item.isGstRegistered);

        }
    },
    select_supplier: function () {
        var self = SupplierDebitNote;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        if (($("#supplierdebitnote-item-list tbody tr").length > 0) && (ID != $("#SupplierID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#supplierdebitnote-item-list tbody').empty();
                $("#SupplierName").val(Name);
                $("#SupplierLocation").val(Location);
                $("#SupplierID").val(ID);
                $("#StateID").val(StateID);
                $("#ReferenceInvoiceNumber").val('');
                $("#ReferenceDocumentDate").val('');
                $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
                self.calculate_grid_total();
                UIkit.modal($('#select-supplier')).hide();
            })

        }
        else {
            $("#SupplierName").val(Name);
            $("#SupplierLocation").val(Location);
            $("#SupplierID").val(ID);
            $("#StateID").val(StateID);
            $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
            UIkit.modal($('#select-supplier')).hide();

        }
    },
    get_item_details: function (release) {
        var self = SupplierDebitNote;
        self.error_count = self.validate_supplier();
        if (self.error_count > 0) {
            $("#SupplierName").focus();
            return;
        }
        $.ajax({
            url: '/Masters/Item/GetDebitAndCrediItemsAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                Type: $('#ItemType').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {

                release(data);
            }
        });
    },
    set_item_details: function (event, item) {   // on select auto complete item
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#Rate").val('');
        $("#Unit").val(item.unit);
        $("#Qty").val('');
        $("#Amount").val('');
        $("#Type").val(item.itemtype);
        $("#CGSTPercentage").val(item.cgstpercentage);
        $("#IGSTPercentage").val(item.igstpercentage);
        $("#SGSTPercentage").val(item.sgstpercentage);
        $("#Qty").focus();
    },
    select_item: function () {
        var self = SupplierDebitNote;
        self.error_count = self.validate_supplier();
        if (self.error_count > 0) {
            $("#SupplierName").focus();
            return;
        }
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").val();
        var Type = $(row).find(".Type").val();
        var CGSTPercentage = clean($(row).find(".CGSTPercentage").val());
        var SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        var IGSTPercentage = clean($(row).find(".IGSTPercentage").val());
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Rate").val('');
        $("#Unit").val(Unit);
        $("#Qty").val('');
        $("#Amount").val('');
        $("#Type").val(Type);
        $("#CGSTPercentage").val(CGSTPercentage);
        $("#SGSTPercentage").val(SGSTPercentage);
        $("#IGSTPercentage").val(IGSTPercentage);
        $("#Qty").focus();
        UIkit.modal($('#select-item')).hide();

    },
    add_item: function () {
        var self = SupplierDebitNote;
        self.error_count = 0;

        self.error_count = self.validate_item();
        if (self.error_count > 0) {

            return;
        }
        var item = {};
        var IsGSTRegistred = $("#IsGSTRegistred").val();
        var StateID = $("#StateID").val();
        var LocationStateID = $("#LocationStateID").val();
        if (IsGSTRegistred == "true" || IsGSTRegistred == "True") {
            item.CGSTPercentage = clean($("#CGSTPercentage").val());
            item.SGSTPercentage = clean($("#SGSTPercentage").val());
            item.IGSTPercentage = clean($("#IGSTPercentage").val());
        }
        else {
            item.CGSTPercentage = 0;
            item.SGSTPercentage = 0;
            item.IGSTPercentage = 0;
        }
        item.Name = $("#ItemName").val();
        item.ID = $("#ItemID").val();
        item.ReferenceInvoiceNumber = $("#ReferenceInvoiceNumber").val();
        item.ReferenceDocumentDate = $("#ReferenceDocumentDate").val();
        item.Qty = clean($("#Qty").val());
        item.Rate = clean($("#Rate").val());
        item.Amount = clean($("#Amount").val());
        item.LocationID = ($("#LocationID").val());
        item.Location = ($("#LocationID :selected").text());
        item.DepartmentID = ($("#DepartmentID").val());
        item.Department = ($("#DepartmentID :selected").text());
        item.EmployeeID = ($("#EmployeeID").val());
        item.Employee = $("#EmployeeID").val() == "" ? "" : $("#EmployeeID  option:selected").text();
        item.InterCompanyID = ($("#InterCompanyID").val());
        item.InterCompany = $("#InterCompanyID").val() == "" ? "" : $("#InterCompanyID  option:selected").text();
        item.ProjectID = ($("#ProjectID").val());
        item.Project = $("#ProjectID").val() == "" ? "" : $("#ProjectID  option:selected").text();
        item.Remarks = ($("#Remarks").val());
        item.PurchaseReturnID = ($("#PurchaseReturnID").val());
        item.PurchaseReturnNo = $("#PurchaseReturnNo").val();       
        if (StateID == LocationStateID && (IsGSTRegistred == "true"|| IsGSTRegistred == "True")) {
            item.CGSTAmount = item.Amount * item.CGSTPercentage / 100;
            item.SGSTAmount = item.Amount * item.SGSTPercentage / 100;
            item.IGSTAmount = 0;
            item.IGSTreadonly = "readonly";
        } else if (StateID != LocationStateID && (IsGSTRegistred == "true" || IsGSTRegistred == "True")) {
            item.CGSTAmount = 0;
            item.SGSTAmount = 0;
            item.GSTreadonly = "readonly";
            item.IGSTAmount = item.Amount * item.IGSTPercentage / 100;
        }
        else {
            item.SGSTAmount = 0;
            item.CGSTAmount = 0;
            item.IGSTAmount = 0;
            item.IGSTreadonly = "readonly";
            item.GSTreadonly = "readonly";
        }
        item.NetAmount = item.CGSTAmount + item.SGSTAmount + item.IGSTAmount + item.Amount
        self.add_item_to_grid(item);
        self.clear_item();
        setTimeout(function () {
            $("#ItemName").focus();
        }, 100);
        self.calculate_grid_total();
    },

    validate_item: function () {
        var self = SupplierDebitNote;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    rules: {
        on_add_item: [
             {
                 elements: "#SupplierID",
                 rules: [
                     { type: form.required, message: "Please choose a Supplier" },
                     { type: form.non_zero, message: "Please choose a Supplier" },
                 ]
             },
                {
                    elements: "#ItemName",
                    rules: [
                        { type: form.required, message: "Please choose a valid item" },
                        { type: form.non_zero, message: "Please choose a valid item" },
                        {
                            type: function (element) {
                                var error = false;
                                $("#Supplierdebitnote-item-list tbody tr").each(function () {
                                    if ($(this).find(".ItemID").val() == $(element).val()) {
                                        error = true;
                                    }
                                });
                                return !error;
                            }, message: "Item already exists"
                        },
                    ]
                },
                    //{
                    //    elements: "#PurchaseReturnID",
                    //    rules: [
                    //        {

                    //            type: function (element) {
                    //                var error = false;
                    //                var Type = $("#Type").val();
                    //                var ReturnID = clean($("#PurchaseReturnID").val());
                    //                if (Type == "Stock" && ReturnID > 0)
                    //                    return true;
                    //                else if (Type == "Service")
                    //                    return true;
                    //                else
                    //                    return false;
                    //            }, message: ' Enter purchase return number'
                    //        }, ]
                    //},
                {
                    elements: "#Rate",
                    rules: [
                        { type: form.required, message: "Invalid Item Rate" },
                        { type: form.non_zero, message: "Invalid Item Rate" },
                        { type: form.positive, message: "Invalid Item Rate" },
                    ]
                },
                {
                    elements: "#Qty",
                    rules: [
                        { type: form.required, message: "Invalid Item Quantity" },
                        { type: form.non_zero, message: "Invalid Item Quantity" },
                        { type: form.positive, message: "Invalid Item Quantity" },
                    ]
                },
                {
                    elements: "#LocationID",
                    rules: [
                        { type: form.required, message: "Please Select Location" },

                    ]
                },
                {
                    elements: "#ReferenceInvoiceNumber",
                    rules: [
                        { type: form.required, message: "Please Select ReferenceInvoiceNumber" },

                    ]
                },
                 {
                     elements: "#DepartmentID",
                     rules: [
                         { type: form.required, message: "Please Select Department" },

                     ]
                 },
                 {
                     elements: "#ReferenceDocumentDate",
                     rules: [
                         { type: form.required, message: "Invalid Date" },
                         { type: form.past_date, message: "Invalid Date" },
                     ]
                 },
                 {
                     elements: "#ItemID",
                     rules: [
                         { type: form.required, message: "Please choose a Item" },
                         { type: form.non_zero, message: "Please choose a Item" },
                     ]
                 },
                {
                    elements: "#SupplierID",
                    rules: [
                        { type: form.required, message: "Please choose a Supplier" },
                        { type: form.non_zero, message: "Please choose a Supplier" },
                    ]
                },

        ],
        on_select_item: [
      {
          elements: "#SupplierID",
          rules: [
              { type: form.required, message: "Please choose a Supplier" },
              { type: form.non_zero, message: "Please choose a Supplier" },
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
              {
                  elements: "#SupplierID",
                  rules: [
                      { type: form.required, message: "Please choose a Supplier" },
                      { type: form.non_zero, message: "Please choose a Supplier" },
                  ]
              },
              {
                  elements: "#supplierdebitnote-item-list tbody tr .CGSTAmount",
                  rules: [
                          { type: form.positive, message: "Invalid and CGST Amount" },
                          {
                              type: function (element) {
                                  var cgst_amount = clean($(element).val());
                                  var sgst_amount = clean($(element).closest('tr').find('.SGSTAmount').val());
                                  return cgst_amount == sgst_amount
                              }, message: "SGST and CGST Amount must be same  "
                          },
                  ]
              },
         {
             elements: "#supplierdebitnote-item-list tbody tr .SGSTAmount",
             rules: [
                     { type: form.positive, message: "Invalid and SGST Amount" },
             ]
         },
         {
             elements: "#supplierdebitnote-item-list tbody tr .IGSTAmount",
             rules: [
                     { type: form.positive, message: "Invalid and IGST Amount" },
             ]
         },
        ],
    },

    add_item_to_grid: function (item) {
        var self = SupplierDebitNote;
        var index, tr;
        index = $("#supplierdebitnote-item-list tbody tr").length + 1;
        tr = '<tr>'
                + ' <td class="uk-text-center">' + index + ' </td>'
                + ' <td class="RNumber">' + item.ReferenceInvoiceNumber + '</td>'
                + ' <td class="RDate">' + item.ReferenceDocumentDate + '</td>'
                + ' <td class="Product">' + item.Name
                + ' <input type="hidden" class="ItemID" value="' + item.ID + '" /></td>'
                + ' <td class="Qty mask-qty" >' + item.Qty + '></td>'
                + ' <td class="Rate mask-currency">' + item.Rate + '</td>'
                + ' <td><input type="text" class="md-input Amount mask-currency" value="' + item.Amount + '" readonly="readonly"   /></td>'
                + ' <td><input type="text" class="md-input GSTPercentage mask-currency" value="' + item.IGSTPercentage + '" readonly="readonly"   /></td>'
                + ' <td><input type="text" class="md-input mask-currency SGSTAmount" value="' + item.SGSTAmount + '" ' + item.GSTreadonly + '="' + item.GSTreadonly + '" /></td>'
                + ' <td><input type="text" class="md-input mask-currency CGSTAmount" value="' + item.CGSTAmount + '" ' + item.GSTreadonly + '="' + item.GSTreadonly + '"  /></td>'
                + ' <td><input type="text" class="md-input mask-currency IGSTAmount" value="' + item.IGSTAmount + '" ' + item.IGSTreadonly + '="' + item.IGSTreadonly + '"  /></td>'
                + ' <td class="Location">' + item.Location
                + ' <input type="hidden" class="LocationID" value="' + $('#LocationID').val() + '" /></td>'
                + ' <td class="Department">' + item.Department
                + ' <input type="hidden" class="DepartmentID" value="' + $('#DepartmentID').val() + '" /></td>'
                + ' <td class="Employee">' + item.Employee
                + ' <input type="hidden" class="EmployeeID" value="' + $('#EmployeeID').val() + '" /></td>'
                + ' <td class="InterCompany">' + item.InterCompany
                + ' <input type="hidden" class="InterCompanyID" value="' + $('#InterCompanyID').val() + '" /></td>'
                + ' <td class="Project">' + item.Project
                + ' <input type="hidden" class="ProjectID" value="' + $('#ProjectID').val() + '" /></td>'
                + ' <td class="Remarks">' + item.Remarks + '</td>'
                + ' <td class="NetAmount mask-currency">' + item.NetAmount + '</td>'
                //+ ' <td class="PurchaseReturnNo">' + item.PurchaseReturnNo
                //+ ' <input type="hidden" class="PurchaseReturnNoID" value="' + $('#PurchaseReturnID').val() + '" />'
                //+ '</td>'
                + ' <td class="uk-text-center">'
                + '     <a class="remove-item">'
                + '         <i class="uk-icon-remove"></i>'
                + '     </a>'
                + ' </td>'
                + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#supplierdebitnote-item-list tbody").append($tr);

        fh_items.resizeHeader();
    },

    clear_item: function () {
        var self = SupplierDebitNote;
        $("#ItemID").val('');
        $("#Qty").val('');
        $("#Rate").val('');
        $("#Amount").val('');
        $("#LocationID").val('');
        $("#EmployeeID").val('');
        $("#DepartmentID").val('');
        $("#InterCompanyID").val('');
        $("#ProjectID").val('');
        $("#Remarks").val('');
        $("#PurchaseReturnID").val('');
        $("#PurchaseReturnNo").val('');
        setTimeout(function () {
            $("#ItemName").val('');
        }, 100);
    },

    update_amount: function () {

        var Quantity = clean($('#Qty').val());
        var Rate = clean($('#Rate').val());

        var Amount = Quantity * Rate;

        $("#Amount").val(Amount);
    },

    update_item: function () {
        var self = SupplierDebitNote;
        var row = $(this).closest('tr');
        var Quantity = clean($(row).find(".Qty").val());
        var Rate = clean($(row).find(".Rate").val());
        var CGSTAmount = clean($(row).find(".CGSTAmount").val());
        var SGSTAmount = clean($(row).find(".SGSTAmount").val());
        var IGSTAmount = clean($(row).find(".IGSTAmount").val());
        var Amount = Quantity * Rate;
        var NetAmount = Amount + CGSTAmount + SGSTAmount + IGSTAmount;
        $(row).find(".Amount").val(Amount);
        $(row).find(".NetAmount").val(NetAmount);
        self.calculate_grid_total();
    },

    calculate_grid_total: function () {
        var NetAmount = 0;
        var CGSTAmount = 0;
        var SGSTAmount = 0;
        var IGSTAmount = 0;
        var TaxableAmount = 0;
        $("#supplierdebitnote-item-list tbody tr").each(function () {
            CGSTAmount += clean($(this).find('.CGSTAmount').val());
            SGSTAmount += clean($(this).find('.SGSTAmount').val());
            IGSTAmount += clean($(this).find('.IGSTAmount').val());
            NetAmount += clean($(this).find('.NetAmount').val());
            TaxableAmount += clean($(this).find('.Amount').val());
        });
        var RoundOff = clean($("#RoundOff").val());
        NetAmount = NetAmount + RoundOff;

        $("#TotalAmount").val(NetAmount);
        $("#RoundOff").val(RoundOff);
        $("#TaxableAmount").val(TaxableAmount);
        $("#CGSTAmt").val(CGSTAmount);
        $("#SGSTAmt").val(SGSTAmount);
        $("#IGSTAmt").val(IGSTAmount);
    },
    update_netamount: function () {
        var RoundOff = clean($("#RoundOff").val());
        var NetAmount = 0;
        $("#supplierdebitnote-item-list tbody tr").each(function () {
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        NetAmount = NetAmount + RoundOff;
        $("#TotalAmount").val(NetAmount);
    },
    save: function (IsDraft, Isnew) {
        self = SupplierDebitNote;
        var location = "/Accounts/SupplierDebitNote/Index";
        var model = self.get_data(IsDraft);
        if (Isnew == true) {
            location = "/Accounts/SupplierDebitNote/Create";
        }
        var url = '/Accounts/SupplierDebitNote/Save';

        if (IsDraft == 1) {
            url = '/Accounts/SupplierDebitNote/SaveAsDraft'
        }
        $(".btnSaveDnNew,.btnSaveASDraft, .btnSaveDn").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Successfully saved ");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveCnNew,.btnSaveASDraft, .btnSaveCn").css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function (IsDraft) {
        var self = SupplierDebitNote;
        var model = {
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            SupplierID: $("#SupplierID").val(),
            SupplierName: $("#SupplierName").val(),
            ReferenceInvoiceNumber: $("#ReferenceInvoiceNumber").val(),
            ReferenceDocumentDate: $("#ReferenceDocumentDate").val(),
            TaxableAmount: clean($("#TaxableAmount").val()),
            TotalAmount: clean($("#TotalAmount").val()),
            CGSTAmt: clean($("#CGSTAmt").val()),
            SGSTAmt: clean($("#SGSTAmt").val()),
            IGSTAmt: clean($("#IGSTAmt").val()),
            RoundOff: clean($("#RoundOff").val()),
            IsDraft: IsDraft,
            ID:clean($("#ID").val())
        };
        model.Items = self.GetProductList()
        return model;
    },

    GetProductList: function () {
        var ProductsArray = [];
        var row;
        $("#supplierdebitnote-item-list tbody tr").each(function () {
            row = $(this);
            var ReferenceInvoiceNumber = $(row).find('.RNumber').text();
            var ReferenceDocumentDate = $(row).find('.RDate').text();
            var ItemID = $(row).find('.ItemID').val();
            var Qty = clean($(row).find('.Qty').val());
            var Rate = clean($(row).find('.Rate').val());
            var Amount = clean($(row).find('.Amount').val());
            var NetAmount = clean($(row).find('.NetAmount').val());
            var CGSTAmt = clean($(row).find('.CGSTAmount').val());
            var IGSTAmt = clean($(row).find('.IGSTAmount').val());
            var SGSTAmt = clean($(row).find('.SGSTAmount').val());
            var GSTPercentage = clean($(row).find('.GSTPercentage').val());
            var Location = $(row).find('.LocationID').val();
            var Department = $(row).find('.DepartmentID').val();
            var Employee = $(row).find('.EmployeeID').val();
            var InterCompany = $(row).find('.InterCompanyID').val();
            var Project = $(row).find('.ProjectID').val();
            var Remarks = $(row).find('.Remarks').text();
            var PurchaseReturnID = $(row).find('.PurchaseReturnNoID').val();

            ProductsArray.push({
                ReferenceInvoiceNumber: ReferenceInvoiceNumber,
                ReferenceDocumentDate: ReferenceDocumentDate,
                ItemID: ItemID,
                Qty: Qty,
                Rate: Rate,
                Amount: Amount,
                LocationID: Location,
                DepartmentID: Department,
                EmployeeID: Employee,
                InterCompanyID: InterCompany,
                ProjectID: Project,
                Remarks: Remarks,
                PurchaseReturnID: PurchaseReturnID,
                CGSTAmt: CGSTAmt,
                SGSTAmt: SGSTAmt,
                IGSTAmt: IGSTAmt,
                GSTPercentage: GSTPercentage,
                NetAmount: NetAmount

            });
        })
        return ProductsArray;
    },

    validate_form: function () {
        var self = SupplierDebitNote;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_supplier: function () {
        var self = SupplierDebitNote;
        if (self.rules.on_select_item.length > 0) {
            return form.validate(self.rules.on_select_item);
        }
        return 0;
    },

    save_new: function () {
        var self = SupplierDebitNote;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save(false, true);
        }, function () {
        })
    },

    save_as_draft: function () {
        var self = SupplierDebitNote;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count == 0) {
            self.save(true, false);
        }
    },

    remove_item: function () {
        var self = SupplierDebitNote;
        $(this).closest("tr").remove();
        $("#supplierdebitnote-item-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#supplierdebitnote-item-list tbody tr").length);
        self.calculate_grid_total();
        fh_items.resizeHeader();
    },
}

