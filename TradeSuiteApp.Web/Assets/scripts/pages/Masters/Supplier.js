supplier = {
    init: function () {

        var self = supplier;
        self.bind_events();
        $("#GSTRegistered").prop("checked", false);
        $("#GSTNotRegistered").prop("checked", true);
        $(".IsGSTRegistered").trigger("ifChanged");
        if ($('#ID').val() != 0) {
            self.GetAddressList();
            self.GetSupplierItemCategoryList();
            self.GetSupplierLocationList();
            self.GetCheckedData();
        }

        Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });

        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });

        self.supplier_list('all');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        //Default value Set  For Allopathy
        $("#SupplierAccountsCategoryID").val(1);
        $("#SupplierTaxCategoryID").val(1);
        $("#SupplierTaxSubCategoryID").val(1);
        $("#PaymentMethodID").val(1);
        $(".SupplierLocation").trigger("ifChanged");
        $(".SuppllierItemCategory").trigger("ifChanged")
    },
    address_reset_text_onclick: function () {
        var defaultValue = $(this).val();
        if (defaultValue === "Default") {
            $(this).val('');
        }
    },
    address_reset_text_focusout: function () {
        var defaultValue = $(this).val();
        if (defaultValue === "") {
            $(this).val('Default');
        }
    },

    // Selects the customer on click modal ok button
    select_customer: function () {
        var self = supplier;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#CustomerID").trigger("change");
        UIkit.modal($('#select-customer')).hide();
    },

    select_employee: function () {
        var self = supplier;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#EmployeeName").val(Name);
        $("#EmployeeID").val(ID);
        UIkit.modal($('#select-employee')).hide();
    },

    get_employee: function (release) {
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
        var self = supplier;
        console.log(item)
        $("#EmployeeID").val(item.id),
            $("#EmployeeName").val(item.name);

    },

    // Gets the items for auto complete
    get_customers: function (release) {
        var self = supplier;
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    // on select auto complete customer
    set_customer: function (event, item) {
        var self = supplier;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        //$("#CustomerID").trigger("change");
    },

    bind_events: function () {
        var self = supplier;
        var SupplierID = $('#ID').val();
        $('#IsBlockForPurcahse').val() == "True" ? $('.IsBlockForPurcahse').iCheck('check') : $('.IsBlockForPurcahse').iCheck('uncheck');
        $('#IsBlockForPayment').val() == "True" ? $('.IsBlockForPayment').iCheck('check') : $('.IsBlockForPayment').iCheck('uncheck');
        $('#IsBlockForReceipt').val() == "True" ? $('.IsBlockForReceipt').iCheck('check') : $('.IsBlockForReceipt').iCheck('uncheck');
        $('#IsDeactivated').val() == "True" ? $('.IsDeactivated').iCheck('check') : $('.IsDeactivated').iCheck('uncheck');
        $('#GstNo').addClass('visible');
        $("#State").on('change', self.GetDistrict);
        $('.IsGSTRegistered').on('ifChanged', self.IsGSTChanged);
        $("#tbl-address").on('click', 'tbody tr', self.EditAddress);
        $(".SuppllierItemCategory").on('ifChanged', self.AddItemCategoryList)
        $(".BtnSave").on("click", self.on_save);
        $(".btnSaveAsDraft").on("click", self.save_draft);
        $("body").on("ifChecked", "#tbl-address .IsBillingDefault", self.set_default_billing_address);
        $("body").on("ifChecked", "#tbl-address .IsShippingDefault", self.set_default_shipping_address);
        $("body").on('click', 'button#btnAddAddress', self.AddAddress);
        $(".SupplierLocation").on('ifChanged', self.AddLocationList)
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $('#IsCustomerName').on('ifChanged', self.IsCustomerChanged);
        $('#IsEmployeeName').on('ifChanged', self.IsEmployeeChanged);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $("#btnOKSupplier").on('click', self.select_supplier);
        $("body").on("click", "#btnAdd", self.add_item);
        $("body").on("click", ".remove-item", self.delete_item);
        $(".btnDelete").on("click", self.Delete_Conform);
        $("#Address1,#Address2,#Place,#MobileNo,#PIN").on("click", self.address_reset_text_onclick);
        $("body").on("focusout", "#Address1,#Address2,#Place,#MobileNo,#PIN", self.address_reset_text_focusout);
        // $("#IsBillingAddress").on('ifChanged',)
    },
    save_draft: function () {
        var self = supplier;
        IsActiveSupplier = false;
        self.check_already_exist(IsActiveSupplier);
    },
    on_save: function () {
        var self = supplier;
        IsActiveSupplier = true;
        self.check_already_exist(IsActiveSupplier);
    },

    Delete_Conform: function () {
        var self = supplier;
        app.confirm_cancel("Do you want to Delete", function () {
            self.Delete();
        }, function () {
        })
    },

    Delete: function () {
        var self = supplier;
        var ID = $("#ID").val();
        $.ajax({
            url: '/Masters/Supplier/DeleteSupplier',
            data: { ID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Supplier canceled successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Supplier/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to delete data.");
                }
            },
        });
    },

    delete_item: function () {
        var self = supplier;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#related-supplier-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });

    },

    add_item: function () {
        var self = supplier;
        sino = $('#related-supplier-list tbody tr ').length + 1;
        var Supplier = $("#SupplierName").val();
        var Location = $("#SupplierLocation").val();
        var RelatedSupplierID = $("#RelatedSupplierID").val();
        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="serial-no uk-text-center width-10">' + sino + '</td>'
            + '<td class="RelatedSupplier width-100">' + Supplier
            + '<input type="hidden" class = "RelatedSupplierID" value="' + RelatedSupplierID + '" />'
            + '</td>'
            + '<td class="RelatedSupplierLocation width-50">' + Location + '</td>'
            + '<td class="width-10 uk-text-center">'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#related-supplier-list tbody').append($content);
        self.clear_data();

    },

    clear_data: function () {
        var self = supplier;
        $("#SupplierName").val('');
        $("#SupplierLocation").val('');


    },

    select_supplier: function () {
        var self = supplier;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        var IsInterCompany = $(row).find(".IsInterCompany").val();
        if (IsInterCompany == 1) {
            $("#GST").val(1);
            $("#GST").prop("disabled", true);
        }
        else {
            $("#GST").val("");
            $("#GST").prop("disabled", false);
        }
        $("#SupplierName").val(Name);
        $("#SupplierLocation").val(Location);
        $("#RelatedSupplierID").val(ID);
        $("#StateId").val(StateID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
        $("#IsInterCompany").val(IsInterCompany);
        $("#StateId").val() == $("#ShippingStateId").val()
        {

        }
    },

    GetCheckedData: function () {
        var self = supplier;
        $('#IsGSTRegistered').val() == "True" ? $('#GSTRegistered').iCheck('check') : $('#GSTNotRegistered').iCheck('check');
        $('#IsCustomer').val() == "True" ? $('.IsCustomer').iCheck('check') : $('.IsCustomer').iCheck('uncheck');
        $('#IsEmployee').val() == "True" ? $('.IsEmployee').iCheck('check') : $('.IsEmployee').iCheck('uncheck');
    },

    AddAddress: function () {
        var self = supplier;
        var Address;
        self.error_count = 0;
        var FormType = "Address";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        Address = supplier.GetAddress();
        Address.Index = self.AddressList.length;
        if ($('#FormIndex').val() != "") {
            Address.Index = clean($('#FormIndex').val());
        }
        for (var i = 0; i < self.AddressList.length; i++) {
            if (clean($('#FormIndex').val()) == self.AddressList[i].Index && $('#FormIndex').val() != "") {
                self.AddressList.splice(i, 1)
            }
        }

        var is_set_default_shipping = false;
        var is_set_default_billing = false;

        $.each(self.AddressList, function () {
            if (this.IsDefault == true) {
                is_set_default_billing = true;
            }
            if (this.IsDefaultShipping == true) {
                is_set_default_shipping = true;
            }
        });

        if (Address.IsBilling) {
            if (!Address.IsDefault) {
                Address.IsDefault = $.grep(self.AddressList, function (element, index) {
                    return element.IsBilling == 1;
                }).length == 0 ? true : false;
            }
        }
        if (Address.IsShipping) {
            if (!Address.IsDefaultShipping) {
                Address.IsDefaultShipping = $.grep(self.AddressList, function (element, index) {
                    return element.IsShipping == 1;
                }).length == 0 ? true : false;
            }
        }
        var index = clean($('#FormIndex').val());
        self.AddressList.push(Address);
        self.PopulateAddress(Address);
        self.clearAddressData();

        $("#tbl-address .IsBillingDefault, #tbl-address .IsShippingDefault").closest('div.iradio_md').show();
        $('#IsBillingAddress').removeAttr("disabled");
        $('#IsShippingAddress').removeAttr("disabled");
    },

    GetAddress: function () {
        if ($("#IsBillingAddress").prop('checked') == false) {
            $('#IsDefault').val('');
        }
        if ($("#IsShippingAddress").prop('checked') == false) {
            $('#IsDefaultShipping').val('');
        }
        var ListData = [];
        var Data = {
            AddressID: clean($('#AddressID').val()),
            AddressLine1: $('#Address1').val(),
            AddressLine2: $('#Address2').val(),
            AddressLine3: $('#Address3').val(),

            Place: $('#Place').val(),
            ContactPerson: $('#ContactPerson').val(),
            LandLine1: $('#LandLine1').val(),
            LandLine2: $('#LandLine2').val(),
            MobileNo: $('#MobileNo').val(),
            StateID: clean($('#State').val()),
            State: $('#State option:selected').text(),
            PIN: $('#PIN').val(),
            Fax: $('#Fax').val(),
            Email: $('#Email').val(),
            District: $("#DistrictID option:selected").text(),
            DistrictID: clean($('#DistrictID').val()),
            IsBilling: $('#IsBillingAddress').prop('checked'),
            IsShipping: $('#IsShippingAddress').prop('checked'),
            IsDefault: $('#IsDefault').val() == "" ? false : ($('#IsDefault').val() == 'true'),
            IsDefaultShipping: $('#IsDefaultShipping').val() == "" ? false : ($('#IsDefaultShipping').val() == 'true'),
            Index: supplier.AddressList.length
        }

        return Data
    },
    get_description: function (SupplierID, Type) {
        var self = supplier;
        $.ajax({
            url: '/Masters/Supplier/GetDescription',
            data: {
                SupplierID: SupplierID,
                Type: Type
            },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {


                if (Type == "Purchase") {

                    $(".Descriptiondetails").html('');

                    var p = "";


                    $.each(data.data, function (key, item) {
                        p += ' <li>' +
                            '<div class="md-list-content">' +
                            '<span class="LastPurchase description">' + item.Key + ' :</span>' +
                            '<span class="LastPurchaseVal description-val">' + item.Value + '</span>' +
                            '</div>' +
                            '</li>'
                    });
                    $('.Descriptiondetails').append(
                        ' <div class="md-card" >' +
                        '<div class="md-card-toolbar DataDiv supplier-div">' +
                        '<span class="DescName description-name">' + data.Name + '</span>' +
                        '</div> ' +
                        '<div class="md-card-content">' +
                        '<ul class="md-list">' + p +

                        '</ul>' +
                        '</div>' +
                        '</div>'

                    );
                    $("#style_switcher").animate("slow");
                    var $switcher = $('#style_switcher');
                    //$switcher.addClass('switcher_active');

                }
            },
        });

    },


    clearAddressData: function () {
        $('#AddressID').val(0);
        $('#Address1').val('Default');
        $('#Address2').val('Default');
        $('#Address3').val('');
        $('#Place').val('Default');
        $('#DistrictID').val('');
        $('#LandLine1').val('');
        $('#LandLine2').val('');
        $('#MobileNo').val('Default');
        $('#State').val('');
        $('#Fax').val('');
        $('#Email').val('');
        $('#PIN').val('Default');
        $('#ContactPerson').val('');
        $('#IsBillingAddress').iCheck('uncheck');
        $('#IsShippingAddress').iCheck('uncheck');
        $('#FormIndex').val('');
        $('#IsDefault').val('');
        $('#IsDefaultShipping').val('');
    },

    set_default_billing_address: function () {
        var self = supplier;
        var td = $(this).closest("td");
        var index = clean($(td).find("input.index").val());
        var IsBillingDefaultIndex = clean($('input[name=IsBillingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var billingindex = self.AddressList[IsBillingDefaultIndex].Index;

        $.each(self.AddressList, function (i, address) {
            address.IsDefault = false;
        });

        self.AddressList[billingindex].IsDefault = true;

        var defaultBillingCount = $('#tbl-address tbody').find('input.IsBillingDefault:checked').length;
        $('#address-count').val(defaultBillingCount);
    },

    set_default_shipping_address: function () {
        var self = supplier;
        var td = $(this).closest("td");
        var index = clean($(td).find("input.index").val());
        var IsShippingDefaultIndex = clean($('input[name=IsShippingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var shippingindex = self.AddressList[IsShippingDefaultIndex].Index;

        $.each(self.AddressList, function (i, address) {
            address.IsDefaultShipping = false;
        });

        self.AddressList[shippingindex].IsDefaultShipping = true;
        var defaultShippingCount = $('#tbl-address tbody').find('input.IsShippingDefault:checked').length;
        $('#address-count').val(defaultShippingCount);
    },

    ItemCategoryList: [],
    AddressList: [],
    LocationList: [],

    AddItemCategoryList: function () {
        var self = supplier;
        var CategoryID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                CategoryID: CategoryID
            };
            self.ItemCategoryList.push(item);
            $('#Supplier-ItemCategory-Mapping').val(self.ItemCategoryList.length);
        } else {
            for (var i = 0; i < self.ItemCategoryList.length; i++) {
                if (self.ItemCategoryList[i].CategoryID == CategoryID) {
                    self.ItemCategoryList.splice(i, 1);
                    $('#Supplier-ItemCategory-Mapping').val(self.ItemCategoryList.length);
                }
            }
        }
    },

    AddLocationList: function () {
        var self = supplier;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Supplier-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Supplier-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },

    EditAddress: function () {
        var self = supplier;
        var Index = clean($(this).find("input[type=hidden]").val());
        for (var i = 0; i < self.AddressList.length; i++) {
            if (self.AddressList[i].Index == Index) {
                var AddressItem = self.AddressList[i];
            }
        }
        $('#AddressID').val(AddressItem.AddressID);
        $('#ContactPerson').val(AddressItem.ContactPerson);
        $('#LandLine1').val(AddressItem.LandLine1);
        $('#LandLine2').val(AddressItem.LandLine2);
        $('#MobileNo').val(AddressItem.MobileNo);
        $('#State').val(AddressItem.StateID);
        $('#PIN').val(AddressItem.PIN);
        $('#Fax').val(AddressItem.Fax);
        $('#Email').val(AddressItem.Email);
        $('#Address1').val(AddressItem.AddressLine1);
        $('#Address2').val(AddressItem.AddressLine2);
        $('#Address3').val(AddressItem.AddressLine3);
        $('#Place').val(AddressItem.Place);
        $('#FormIndex').val(AddressItem.Index);
        $("#State").trigger("change");
        setTimeout(function () {
            $("#DistrictID option[value='" + AddressItem.DistrictID + "']").attr("selected", "selected");
        }, 200);

        $('#IsBillingAddress').iCheck('uncheck');
        $('#IsShippingAddress').iCheck('uncheck');

        if (AddressItem.IsBilling) {
            $('#IsBillingAddress').iCheck('check');
        }
        else {
            $('#IsBillingAddress').iCheck('uncheck');
            AddressItem.IsDefault = false;
        }
        if (AddressItem.IsShipping) {
            $('#IsShippingAddress').iCheck('check');
        }
        else {
            $('#IsShippingAddress').iCheck('uncheck');
            AddressItem.IsDefaultShipping = false;
        }

        $('#IsBillingAddress').removeAttr("disabled");
        $('#IsShippingAddress').removeAttr("disabled");
        if (AddressItem.IsDefault) {
            $('#IsBillingAddress').attr("disabled", "disabled");
        }
        if (AddressItem.IsDefaultShipping) {
            $('#IsShippingAddress').attr("disabled", "disabled");
        }

        $('#IsDefault').val(AddressItem.IsDefault);
        $('#IsDefaultShipping').val(AddressItem.IsDefaultShipping);

        $("#tbl-address .IsBillingDefault, #tbl-address .IsShippingDefault").closest('div.iradio_md').hide();


    },

    GetAddressList: function () {
        var self = supplier;
        var SupplierID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Supplier/GetAddressList',
            data: { SupplierID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i].Index = i;
                    if (data[i].IsBilling == data[i].IsShipping) {
                        data[i].SameAsBilling = true;
                    }
                    self.AddressList.push(data[i]);
                    self.AddressEditPage(data[i]);
                }
                console.log(self.AddressList);
            },
        });
    },

    GetSupplierItemCategoryList: function () {
        var self = supplier;
        var SupplierID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Supplier/GetSupplierItemCategory',
            data: { SupplierID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $ItemCategoryDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].CategoryName + "'> </div>"
                    app.format($ItemCategoryDetail);
                    $('#Item-Category-Detail-Container').append($ItemCategoryDetail);
                    $('.SuppllierItemCategory').each(function () {
                        if ($(this).val() == data[i].CategoryID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });

    },

    GetSupplierLocationList: function () {
        var self = supplier;
        var SupplierID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Supplier/GetSupplierLocationMapping',
            data: { SupplierID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $SupplierLocationDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].LocationName + "'> </div>"
                    app.format($SupplierLocationDetail);
                    $('#Location-Detail-Container').append($SupplierLocationDetail);
                    $('.SupplierLocation').each(function () {
                        if ($(this).val() == data[i].SupplierLocationID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });
    },

    supplier_list: function (type) {
        if (typeof type == "undefined") {
            type = "all";
        }
        var $list = $('#supplier-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url;
            if (type == "stock") {
                url = "/Masters/Supplier/GetStockSupplierList";
            } else if (type == "stock-and-milk") {
                url = "/Masters/Supplier/GetStockAndMilkSupplierList";
            } else if (type == "service") {
                url = "/Masters/Supplier/GetServiceSupplierList";
            } else if (type == "GRNWise") {
                url = "/Masters/Supplier/GetGRNWiseSupplierList";
            } else if (type == "Creditors") {
                url = "/Masters/Supplier/GetCreditorSupplierList";
            } else if (type == "milk") {
                url = "/Masters/Supplier/GetMilkSupplierList";
            } else if (type == "Payment") {
                url = "/Masters/Supplier/GetPaymentSupplierList";
            } else if (type == "PaymentReturnVoucher") {
                url = "/Masters/Supplier/GetSupplierListForPaymentReturn";
            } else if (type == "not-intercompany") {
                url = "/Masters/Supplier/GetNotInterCompanySupplierList";
            } else if (type == "not-intercompany-and-milk") {
                url = "/Masters/Supplier/GetNotInterCompanyAndMilkSupplierList";
            } else {
                url = "/Masters/Supplier/GetAllSupplierList";
            }
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
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
                            return "<input type='radio' class='uk-radio SupplierID' name='SupplierID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='StateID' value='" + row.StateID + "'>"
                                + "<input type='hidden' class='GstNo' value='" + row.GstNo + "'>"
                                + "<input type='hidden' class='PaymentDays' value='" + row.PaymentDays + "'>"
                                + "<input type='hidden' class='SupplierCategoryID' value='" + row.SupplierCategoryID + "'>"
                                + "<input type='hidden' class='CurrencyConversionRate' value='" + row.CurrencyConversionRate + "'>"
                                + "<input type='hidden' class='CurrencyID' value='" + row.CurrencyID + "'>"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.CurrencyCode + "'>"
                                + "<input type='hidden' class='CurrencyName' value='" + row.CurrencyName + "'>"
                                + "<input type='hidden' class='CurrencyPrefix' value='" + row.CurrencyPrefix + "'>"
                                + "<input type='hidden' class='DecimalPlaces' value='" + row.DecimalPlaces + "'>"
                                + "<input type='hidden' class='IsInterCompany' value='" + row.IsInterCompany + "'>"
                                + "<input type='hidden' class='InterCompanyLocationID' value='" + row.InterCompanyLocationID + "'>"
                                + "<input type='hidden' class='IsGSTRegistered' value='" + row.IsGSTRegistered + "'>"
                                + "<input type='hidden' class='BankACNo' value='" + row.BankACNo + "'>"
                                + "<input type='hidden' class='Email' value='" + row.Email + "'>"
                                + "<input type='hidden' class='BankName' value='" + row.BankName + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Location", "className": "Location" },
                    { "data": "SupplierCategory", "className": "SupplierCategory" },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.LandLine1 + ', ' + row.LandLine2;
                        }
                    },
                    { "data": "MobileNo", "className": "MobileNo" },
                    {
                        "data": "ItemCategory",
                        "className": "ItemCategory",
                        "render": function (data, type, row) {
                            var text = row.ItemCategory;
                            if (text.length > 75) {
                                text = text.substring(0, 75) + "..";
                            }
                            return "<div class='part' >" + text + "</div>" + "<div class='full' >" + row.ItemCategory + "</div>";
                        }
                    },
                    {
                        "data": "GSTRegistered",
                        "className": "GSTRegistered uk-hidden",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

        }
    },

    InterCompany_supplier_list: function () {
        var self = supplier
        $('#supplier-list tbody').empty();

        $.ajax({
            url: '/Masters/Supplier/InterCompanySupplier/',
            dataType: "json",
            type: "GET",
            data: {
                Term: '',
            },
            success: function (response) {
                var $list = $('#supplier-list tbody');
                $list.html('');
                var tr = '';
                $.each(response, function (i, supplier) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center '><input type='radio'  class='SupplierID' name='SupplierID' data-md-icheck value=" + supplier.ID + " />"
                        + "<input type='hidden' class='LocationID' value='" + supplier.LocationID + "'>"
                        + "<input type='hidden' class='CustomerID' value='" + supplier.CustomerID + "'></td>"
                        + "<td>" + supplier.Code + "</td>"
                        + "<td class='Name'>" + supplier.Name + "</td>"
                        + "<td>" + supplier.Location + "</td>"
                        + "<td>" + supplier.SupplierCategoryName + "</td>"
                        + "<td>" + supplier.ItemCategory + "</td>"
                        + "<td>" + supplier.IsGSTRegistered + "</td>"
                        + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $list.append($tr);
                });
            },
        });
    },
    InterCompany_supplier_list_for_location: function () {
        var self = supplier
        $('#supplier-list tbody').empty();

        $.ajax({
            url: '/Masters/Supplier/InterCompanySupplierForLocation/',
            dataType: "json",
            type: "GET",
            data: {
                Term: '',
            },
            success: function (response) {
                var $list = $('#supplier-list tbody');
                $list.html('');
                var tr = '';
                $.each(response, function (i, supplier) {
                    tr = "<tr>"
                        + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                        + "<td class='uk-text-center '><input type='radio'  class='SupplierID' name='SupplierID' data-md-icheck value=" + supplier.ID + " />"
                        + "<input type='hidden' class='LocationID' value='" + supplier.LocationID + "'>"
                        + "<input type='hidden' class='CustomerID' value='" + supplier.CustomerID + "'></td>"
                        + "<td>" + supplier.Code + "</td>"
                        + "<td class='Name'>" + supplier.Name + "</td>"
                        + "<td>" + supplier.Location + "</td>"
                        + "<td>" + supplier.SupplierCategoryName + "</td>"
                        + "<td>" + supplier.ItemCategory + "</td>"
                        + "<td>" + supplier.IsGSTRegistered + "</td>"
                        + "</tr>";
                    $tr = $(tr);
                    app.format($tr);
                    $list.append($tr);
                });
            },
        });
    },
    Doctor_list: function (type) {

        var $list = $('#doctor-list');
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
                    "url": "/Masters/Supplier/GetDoctorList",
                    "type": "POST"
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
                            return "<input type='radio' class='uk-radio SupplierID' name='SupplierID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='StateID' value='" + row.StateID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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

        }
    },
    list: function () {
        var $list = $('#supplier-list');
        $list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/Supplier/Details/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Supplier/GetAllSupplierListForMainList";
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    {
                        "data": "Code", "className": "Code"
                    },
                    {
                        "data": "Name", "className": "Name"
                    },
                    {
                        "data": "Location", "className": "Location"
                    },
                    { "data": "SupplierCategory", "className": "SupplierCategory" },
                    {
                        "data": "ItemCategory",
                        "className": "ItemCategory",
                        "render": function (data, type, row) {
                            var text = row.ItemCategory;
                            if (text.length > 100) {
                                text = text.substring(0, 100) + "..";
                            }
                            return "<div class='part' >" + text + "</div>" + "<div class='full' >" + row.ItemCategory + "</div>";
                        }
                    },
                    { "data": "OldCode", "className": "OldCode" },
                    {
                        "data": "GSTRegistered",
                        "className": "GSTRegistered",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
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
        }
    },

    AddressEditPage: function (Address) {
        var self = supplier;
        var icheckBilling = "disabled", icheckShipping = "disabled", billing = "", shipping = "";
        var defaultBilling = "", defaultShipping = "";
        var html = "";

        if (Address.IsBilling) {
            icheckBilling = "";
            billing = "Billing Address";
            if (Address.IsDefault) {
                defaultBilling = "checked";
            }
        }
        if (Address.IsShipping) {
            icheckShipping = "";
            shipping = "Shipping Address";
            if (Address.IsDefaultShipping) {
                defaultShipping = "checked";
            }
        }
        html = "<tr><td>" + billing + "<br>" + shipping + "</td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsBillingDefault default' " + icheckBilling + " data-md-icheck " + defaultBilling + "/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsShippingDefault default' " + icheckShipping + " data-md-icheck " + defaultShipping + "/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td>" + Address.ContactPerson + "</td>"
            + "<td>" + Address.AddressLine1 + "<br>" + Address.AddressLine2 + "<br>" + Address.AddressLine3 + "<br>" + Address.Place + "<br>" + Address.District + "," + Address.State + "</td>"
            + "<td>" + Address.LandLine1 + "<br>" + Address.LandLine2 + "<br>Fax :" + Address.Fax + "<br>Email :" + Address.Email + "</td>"
            + "<td>" + Address.MobileNo + "</td></tr>";

        var $htmlBilling = $(html);
        app.format($htmlBilling);
        $htmlBilling.find('input[type = radio]').attr("name", "IsBillingDefault");
        $htmlBilling.find('input[type = radio].IsShippingDefault').attr("name", "IsShippingDefault");
        if ($('#FormIndex').val() == "") {
            $("#tbl-address  tbody").append($htmlBilling);
        } else {
            $('#tbl-address tbody tr').each(function () {
                if ($('#FormIndex').val() == $(this).find('input[type=hidden]').val()) {
                    $(this).replaceWith($htmlBilling);
                }
            });
        }

    },

    PopulateAddress: function (Address) {
        var self = supplier;
        var icheckBilling = "disabled", icheckShipping = "disabled", billing = "", shipping = "";
        var html = "";

        if (Address.IsBilling) {
            icheckBilling = "";
            billing = "Billing Address";
            if (Address.IsDefault) {
                icheckBilling = "checked";
            }
        }
        if (Address.IsShipping) {
            icheckShipping = "";
            shipping = "Shipping Address";
            if (Address.IsDefaultShipping) {
                icheckShipping = "checked";
            }
        }
        html = "<tr ><td class='BillingShipping'>" + billing + "<br>" + shipping + "</td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsBillingDefault default' " + icheckBilling + " data-md-icheck/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsShippingDefault default' " + icheckShipping + " data-md-icheck/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td>" + Address.ContactPerson + "</td>"
            + "<td>" + Address.AddressLine1 + "<br>" + Address.AddressLine2 + "<br>" + Address.AddressLine3 + "<br>" + Address.Place + "<br>" + Address.District + "," + Address.State + "</td>"
            + "<td>" + Address.LandLine1 + "<br>" + Address.LandLine2 + "<br>Fax :" + Address.Fax + "<br>Email :" + Address.Email + "</td>"
            + "<td>" + Address.MobileNo + "</td></tr>";
        var $htmlBilling = $(html);
        app.format($htmlBilling);
        $htmlBilling.find('input[type = radio]').attr("name", "IsBillingDefault");
        $htmlBilling.find('input[type = radio].IsShippingDefault').attr("name", "IsShippingDefault");
        if ($('#FormIndex').val() == "") {
            $("#tbl-address  tbody").append($htmlBilling);
        } else {
            $('#tbl-address tbody tr').each(function () {
                if ($('#FormIndex').val() == $(this).find('input[type=hidden]').val()) {
                    $(this).replaceWith($htmlBilling);
                }
            });
        }

    },

    IsGSTChanged: function () {
        if ($(".IsGSTRegistered").prop('checked') == true) {
            $('#GstNo').addClass('visible');
            $(".Gst-number").show();
            //Default value for Allopathy
            $("#SupplierTaxSubCategoryID").val(1);
        } else {
            $(".Gst-number").hide();
            $('#GstNo').removeClass('visible');
            //Default value for Allopathy
            $("#SupplierTaxSubCategoryID").val(2);
        }
    },

    IsCustomerChanged: function () {
        if ($("#IsCustomerName").prop('checked') == true) {
            $('.Customer-name').removeClass('uk-hidden');
        } else {
            $('.Customer-name').addClass('uk-hidden');
        }
    },

    IsEmployeeChanged: function () {
        if ($("#IsEmployeeName").prop('checked') == true) {
            $('.Employee-name').removeClass('uk-hidden');
            $(".Employee-name").show();
        } else {
            $(".Employee-name").hide();
            $('.Employee-name').addClass('uk-hidden');
        }
    },
    check_already_exist: function (IsActiveSupplier) {
        var self = supplier;
        self.error_count = 0;
        var FormType = "Form";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        var GstNo, mobile, landline, landline2, index;
        $(".BtnSave").css({ 'visibility': 'hidden' });
        $(".btnSaveAsDraft").css({ 'visibility': 'hidden' });
        if ($(".IsGSTRegistered").prop('checked') == true) {

            GstNo = $("#GstNo").val();
        }
        if ($("#tbl-address .IsBillingDefault").prop('checked') == true) {

            var row = $('#tbl-address tbody').find('input.IsBillingDefault:checked').closest('tr');
            index = $(row).find('.index').val();
            landline = self.AddressList[index].LandLine1;
            landline2 = self.AddressList[index].LandLine2;
            mobile = self.AddressList[index].MobileNo;
        }
        $.ajax({
            url: '/Masters/Supplier/CheckSupplierAlradyExist',
            data: {
                ID: clean($("#ID").val()),
                Name: $("#Name").val(),
                GstNo: GstNo,
                PanCardNo: $("#PanCardNo").val(),
                AdhaarCardNo: $("#AdhaarCardNo").val(),
                Mobile: mobile,
                LandLine1: landline,
                landline2: landline2,
                AcNo: $("#AcNo").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    if (data.IsDuplicate == true) {
                        app.show_error("Duplicate Suppliers Exists.");
                        $('.BtnSave').css({ 'visibility': 'visible' });
                        $(".btnSaveAsDraft").css({ 'visibility': 'visible' });
                    } else {
                        app.confirm_cancel(data.Message, function () {
                            self.save(IsActiveSupplier);
                        }, function () {
                            $('.BtnSave').css({ 'visibility': 'visible' });
                            $(".btnSaveAsDraft").css({ 'visibility': 'visible' });
                        })
                    }
                }
            },
            error: function () {
                $('.BtnSave').css({ 'visibility': 'visible' });
                $(".btnSaveAsDraft").css({ 'visibility': 'visible' });
            }
        });

    },

    save: function (IsActiveSupplier) {
        var self = supplier;
        var url = '/Masters/Supplier/Save'
        if (!IsActiveSupplier) {
            url = '/Masters/Supplier/SaveAsDraft'
        }
        var model = self.get_form_data();
        model.IsActiveSupplier = IsActiveSupplier
        $.ajax({
            url: url,
            data: {
                model
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Supplier created successfully");
                    setTimeout(function () {
                        app.load_content("/Masters/Supplier/Index");
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $('.BtnSave').css({ 'visibility': 'visible' });
                    $(".btnSaveAsDraft").css({ 'visibility': 'visible' });
                }
            },
            error: function () {
                $('.BtnSave').css({ 'visibility': 'visible' });
                $(".btnSaveAsDraft").css({ 'visibility': 'visible' });
            }
        });
        //}, function () {
        //})
    },

    get_form_data: function () {
        debugger
        var self = supplier;
        var model = {
            ID: clean($("#ID").val()),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            AdhaarCardNo: $("#AdhaarCardNo").val(),
            PanCardNo: $("#PanCardNo").val(),
            PaymentDays: $("#PaymentDays option:selected").data("days"),
            CurrencyID: $("#CurrencyID option:selected").val(),
            SupplierCategoryID: clean($("#SupplierCategoryID").val()),
            StartDate: $("#StartDate").val(),
            DeactivatedDate: $("#DeactivatedDate").val(),
            SupplierAccountsCategoryID: clean($("#SupplierAccountsCategoryID").val()),
            SupplierTaxCategoryID: clean($("#SupplierTaxCategoryID").val()),
            SupplierTaxSubCategoryID: clean($("#SupplierTaxSubCategoryID").val()),
            PaymentMethodID: clean($("#PaymentMethodID").val()),
            PaymentGroupID: clean($("#PaymentGroupID").val()),
            Currency: $("#CurrencyID option:selected").text(),
            OldCode: $("#OldCode").val(),
            UanNo: $("#UanNo").val(),
            BankName: $("#BankName").val(),
            BranchName: $("#BranchName").val(),
            AcNo: $("#AcNo").val(),
            IfscNo: $("#IfscNo").val(),
            TradeLegalName: $("#TradeLegalName").val(),
        }
        var item = {};
        model.RelatedSupplierList = [];
        $('#related-supplier-list tbody tr').each(function () {
            item = {};
            item.RelatedSupplierID = $(this).find(".RelatedSupplierID").val();
            item.RelatedSupplierLocation = $(this).find(".RelatedSupplierLocation").text();
            model.RelatedSupplierList.push(item);
        });
        if ($("#IsCustomerName").prop('checked') == true) {
            model.IsCustomer = true;
            model.CustomerID = $("#CustomerID").val();
        } else {
            model.IsCustomer = false;
        }
        if ($("#IsEmployeeName").prop('checked') == true) {
            model.IsEmployee = true;
            model.EmployeeID = $("#EmployeeID").val();
        } else {
            model.IsEmployee = false;
        }
        if ($(".IsGSTRegistered").prop('checked') == true) {
            model.IsGSTRegistered = true;
            model.GstNo = $("#GstNo").val();
        } else {
            model.IsGSTRegistered = false;
        }

        $('.IsDeactivated').prop("checked") == true ? model.IsDeactivated = true : model.IsDeactivated = false;
        $('.IsBlockForPurcahse').prop("checked") == true ? model.IsBlockForPurcahse = true : model.IsBlockForPurcahse = false;
        $('.IsBlockForPayment').prop("checked") == true ? model.IsBlockForPayment = true : model.IsBlockForPayment = false;
        $('.IsBlockForReceipt').prop("checked") == true ? model.IsBlockForReceipt = true : model.IsBlockForReceipt = false;

        model.AddressList = self.AddressList;
        model.SupplierItemCategoryList = self.ItemCategoryList;
        model.SupplierLocationList = self.LocationList

        return model;
    },

    GetDistrict: function () {
        var state = $(this);
        $.ajax({
            url: '/Masters/District/GetDistrict/',
            dataType: "json",
            type: "GET",
            data: {
                StateID: state.val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                });
            }
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Name: $("#Name").val(),
            GstState: $("#GstState").val(),
        }
        return model;
    },

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

    set_supplier_details: function (event, item) {   // on select auto complete item

        var self = supplier;
        $("#SupplierLocation").val(item.location);
        $("#RelatedSupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#IsInterCompany").val(item.isintercompany);
        $("#DDLPaymentWithin option:contains(" + item.paymentDays + ")").attr("selected", true);
        $("#SupplierReferenceNo").focus();
    },
    validate_form: function (FormType) {
        var self = supplier;
        if (FormType == "Form") {
            if (self.rules.on_submit.length) {
                return form.validate(self.rules.on_submit);
            }
        } else if (FormType == "Address") {
            if (self.rules.on_address_submit.length) {
                return form.validate(self.rules.on_address_submit);
            }
        }
        return 0;
    },


    index_list: function () {
        var self = supplier;
        $('#tabs-supplier').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "saved-supplier":
                $list = $('#saved-supplier-list');
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
            var url = "/Masters/Supplier/GetSuppliersForListView?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "asc"]],
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    {
                        "data": "Code", "className": "Code"
                    },
                    {
                        "data": "Name", "className": "Name"
                    },
                    {
                        "data": "Location", "className": "Location"
                    },
                    { "data": "SupplierCategory", "className": "SupplierCategory" },
                    { "data": "ItemCategory", "className": "ItemCategory" },
                    { "data": "OldCode", "className": "OldCode" },
                    {
                        "data": "GSTRegistered",
                        "className": "GSTRegistered",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
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
                        app.load_content("/Masters/Supplier/Details/" + Id);

                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },


    error_count: 0,

    rules: {
        on_submit: [
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Supplier Name is required" },
                ]
            },
            {
                elements: "#SupplierCategoryID",
                rules: [
                    { type: form.required, message: "Select a Supplier Category" },
                ]
            },
            {
                elements: "#CurrencyID",
                rules: [
                    { type: form.required, message: "Select a Currency" },
                ]
            },

            {
                elements: "#CreditDays",
                rules: [
                    { type: form.required, message: "Select CreditDays" },
                ]
            },
            {
                elements: "#GstNo.visible",
                rules: [
                    { type: form.required, message: "GST Number is Required" },
                ]
            },
            {
                elements: ".Customer-name",
                rules: [
                    {
                        type: function (element) {
                            var count = 0
                            var isCustomer = $('#IsCustomerName').prop('checked');
                            if (isCustomer == true) {
                                if ($("#CustomerName").val() == "") {
                                    count++;
                                }
                            }
                            return count == 0;
                        }, message: "Customer Name Is Required "
                    },
                ]
            },
            {
                elements: ".Employee-name",
                rules: [
                    {
                        type: function (element) {
                            var count = 0
                            var isEmployee = $('#IsEmployeeName').prop('checked');

                            if (isEmployee == true) {
                                if ($("#EmployeeName").val() == "") {
                                    count++;
                                }
                            }
                            return count == 0;
                        }, message: "Employee Name Is Required "
                    },
                ]
            },
            {
                elements: "#DeactivatedDate",
                rules: [
                    {
                        type: function (element) {
                            var u_date = $(element).val().split('-');
                            var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                            var a = Date.parse(used_date);
                            var po_date = $('#StartDate').val().split('-');
                            var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                            var date = Date.parse(po_datesplit);
                            return date <= a
                        }, message: "End date should be a date on or after start date"
                    }
                ]
            },

        ],
        on_address_submit: [
            {
                elements: "#Address1",
                rules: [
                    { type: form.required, message: "Address Line 1 is required" },
                ]
            },
            {
                elements: "#Address2",
                rules: [
                    { type: form.required, message: "Address Line 2 is required" },
                ]
            },
            {
                elements: "#Place",
                rules: [
                    { type: form.required, message: "Bill Place is required" },
                ]
            },
            {
                elements: "#PIN",
                rules: [
                    { type: form.required, message: "PIN is required" },
                ]
            },
            {
                elements: "#MobileNo",
                rules: [
                    { type: form.required, message: "Mobile Number is required" },
                ]
            },
            {
                elements: "#IsBillingAddress",
                rules: [
                    {
                        type: function (element) {
                            var isBilling = $('#IsBillingAddress').prop('checked');
                            var isShipping = $('#IsShippingAddress').prop('checked');
                            if (isShipping == false && isBilling == false) {
                                return false;
                            }
                            return true;
                        }, message: "Please Choose Billing or Shipping Address "
                    },
                ]
            }
        ],
    }
};
