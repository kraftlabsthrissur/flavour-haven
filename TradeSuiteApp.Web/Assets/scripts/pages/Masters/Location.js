
Location = {
    init: function () {
        var self = Location;
        self.bind_events();
        supplier.InterCompany_supplier_list_for_location();
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#ItemName",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        Customer.customer_list('sales-order');
        self.supplier_list();
        if ($('#ID').val() != 0) {
            self.GetAddressList();
        }
        self.currency_list();

    },
    list: function () {
        var $list = $('#location-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#location-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/Location/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        var self = Location;
        $(".btnSave").on('click', self.save_confirm);
        $("#State").on('change', self.GetDistrict);
        $("body").on("ifChanged", "#tbl-address .IsBillingDefault", self.set_default_billing_address);
        $("body").on("ifChanged", "#tbl-address .IsShippingDefault", self.set_default_shipping_address);
        $("#tbl-address").on('click', 'tbody tr', self.EditAddress);
        $("body").on('click', 'button#btnAddAddress', self.AddAddress);
        $("#btnOKSupplier").on("click", self.select_supplier);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("body").on("click", "#btnOKCurrency", self.select_currency);
        $.UIkit.autocomplete($('#currency-autocomplete'), { 'source': self.get_currencies, 'minLength': 1 });
        $('#currency-autocomplete').on('selectitem.uk.autocomplete', self.set_currency);
    },
    currency_list: function () {

        var url = "/Masters/Currency/GetCurrenciesList";
        var $list = $('#currency_list');
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
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CurrencyID", Value: $('#CurrencyID').val() },

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
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio CurrencyID' name='CurrencyID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='Code' value='" + row.Code + "'>"
                                + "<input type='hidden' class='Name' value='" + row.Name + "'>"
                                + "<input type='hidden' class='CountryName' value='" + row.CountryName + "'>"
                                + "<input type='hidden' class='CountryID' value='" + row.CountryID + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "CountryName", "className": "CountryName" },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            //$('body').on("change", '#CustomerCategoryID,#CustomerStateID', function () {
            //    list_table.fnDraw();
            //});
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
    },
    get_customers: function (release) {
        var self = Location;
        $.ajax({
            url: '/Masters/Customer/GetSalesOrderCustomersAutoComplete',
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
    get_currencies: function (release) {
        var self = Location;
        $.ajax({
            url: '/Masters/Currency/GetCurrenciesAutoCompleteList',
            data: {
                CurrencyName: $('#CurrencyName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customer: function (event, item) {
        var self = Location;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
    },
    select_currency: function () {

        var radio = $('#currency_list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").val().trim();
        var CountryID = $(row).find(".CountryID").val().trim();
        var CountryName = $(row).find(".CountryName").val().trim();
        $("#CurrencyID").val(ID);
        $("#CurrencyName").val(Name);
        $("#CountryID").val(CountryID);
        $("#CountryName").val(CountryName);

    },
    set_currency: function (event, item) {
        $("#CurrencyID").val(item.id);
        $("#CurrencyName").val(item.name);
        $("#CountryID").val(item.countryid);
        $("#CountryName").val(item.countryname);
    },
    select_customer: function () {
        var self = Location;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
    },
    select_supplier: function (event) {
        var self = Location;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = clean(radio.val());
        var Name = $(row).find(".Name").text().trim();
        var CustomerID = $(row).find(".CustomerID").val();
        var LocationID = $(row).find(".LocationID").val();
        $("#SupplierName").val(Name);
        $("#SupplierID").val(ID);
        UIkit.modal($('#select-supplier')).hide();
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/InterCompanySupplier',
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
        self = Location;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },
    supplier_list: function () {
        var self = Location;
        $list = $('#supplier-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();


            $list.find('thead.search input').off('keyup change').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    save_confirm: function () {
        var self = Location;
        self.error_count = 0;
        var FormType = "Form";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    AddressList: [],
    get_form_data: function () {
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Place: $("#Place").val(),
            LocationStateID: $("#LocationStateID").val(),
            LocationTypeID: $("#LocationTypeID").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            CompanyName: $("#CompanyName").val(),
            OwnerName: $("#OwnerName").val(),
            GSTNo: $("#GSTNo").val(),
            Jurisdiction: $("#Jurisdiction").val(),
            AuthorizedSignature: $("#AuthorizedSignature").val(),
            URL: $("#URL").val(),
            LocationHeadID: $("#LocationHeadID").val(),
            SupplierID: $("#SupplierID").val(),
            CustomerID: $("#CustomerID").val(),
            CurrencyID: $("#CurrencyID").val(),
            CountryID: $("#CountryID").val(),
            VatRegNo: $("#VatRegNo").val(),
        }
        return model;
    },
    validate_form: function () {
        var self = Location;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [

            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: " Code is Required" },
                ]
            },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: " Name is Required" },
                ]
            },
            {
                elements: "#LocationTypeID",
                rules: [
                    { type: form.required, message: " LocationGroup is Required" },
                ]
            },
            {
                elements: "#LocationStateID",
                rules: [
                    { type: form.required, message: " State is Required" },
                ]
            },
            {
                elements: "#LocationHeadID",
                rules: [
                    { type: form.required, message: " LocationHead is Required" },
                ]
            },
            {
                elements: "#Place",
                rules: [
                    { type: form.required, message: " place is Required" },
                ]
            },
            {
                elements: "#address-count",
                rules: [
                    {
                        type: function (element) {
                            var defaultBillingCount = $('#tbl-address tbody').find('input.IsBillingDefault:checked').length;
                            var defaultShippingCount = $('#tbl-address tbody').find('input.IsShippingDefault:checked').length;
                            if (defaultBillingCount == 0 || defaultShippingCount == 0) {
                                return false;
                            }
                            return true;
                        }, message: "Billing And Shipping Address Required"
                    }
                ]
            },
            {
                elements: "#EndDate",
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
            {
                elements: "#CurrencyID",
                rules: [
                    {
                        type: function (element) {
                            var getCurrencyID = $(element).val();
                            if (getCurrencyID > 0)
                                return true;
                            else
                                return false;
                        }, message: "Currency is Required"
                    },
                ]
            },
            {
                elements: "#CountryID",
                rules: [
                    {
                        type: function (element) {
                            var getCountryID = $(element).val();
                            if (getCountryID > 0)
                                return true;
                            else
                                return false;
                        }, message: " Country is Required"
                    },
                ]
            }
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
                elements: "#AddressPlace",
                rules: [
                    { type: form.required, message: "Place is required" },
                ]
            },
            {
                elements: "#PIN",
                rules: [
                    { type: form.required, message: "PIN is required" },
                ]
            },
            {
                elements: "#State",
                rules: [
                    { type: form.required, message: "Please Select State" },
                ]
            },
            {
                elements: "#DistrictID",
                rules: [
                    { type: form.required, message: "Please Select District" },
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
    AddAddress: function () {
        var self = Location;
        var Address;
        self.error_count = 0;
        var FormType = "Address";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        Address = Location.GetAddress();
        Address.Index = self.AddressList.length;
        if ($('#FormIndex').val() != "") {
            Address.Index = $('#FormIndex').val();
        }
        for (var i = 0; i < self.AddressList.length; i++) {
            if ($('#FormIndex').val() == self.AddressList[i].Index && $('#FormIndex').val() != "") {
                self.AddressList.splice(i, 1)
            }
        }
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
        self.AddressList.push(Address);
        self.PopulateAddress(Address);
        self.clearAddressData();
    },
    GetAddress: function () {
        var ListData = [];
        var Data = {
            AddressID: clean($('#AddressID').val()),
            AddressLine1: $('#Address1').val(),
            AddressLine2: $('#Address2').val(),
            AddressLine3: $('#Address3').val(),
            AddressPlace: $('#AddressPlace').val(),
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
            Index: Location.AddressList.length
        }

        return Data
    },
    set_default_billing_address: function () {
        var self = Location;
        var td = $(this).closest("td");
        var index = $(td).find("input.index").val();
        $.each(self.AddressList, function (i, address) {
            address.IsDefault = false;
        });
        self.AddressList[index].IsDefault = true;

        var defaultBillingCount = $('#tbl-address tbody').find('input.IsBillingDefault:checked').length;
        $('#address-count').val(defaultBillingCount);
    },
    set_default_shipping_address: function () {
        var self = Location;
        var td = $(this).closest("td");
        var index = $(td).find("input.index").val();
        $.each(self.AddressList, function (i, address) {
            address.IsDefaultShipping = false;
        });
        self.AddressList[index].IsDefaultShipping = true;
        var defaultShippingCount = $('#tbl-address tbody').find('input.IsShippingDefault:checked').length;
        $('#address-count').val(defaultShippingCount);
    },
    PopulateAddress: function (Address) {
        var self = Location;
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
            + "<td>" + Address.AddressLine1 + "<br>" + Address.AddressLine2 + "<br>" + Address.AddressLine3 + "<br>" + Address.AddressPlace + "<br>" + Address.District + "," + Address.State + "-" + Address.PIN + "</td>"
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
    EditAddress: function () {
        var self = Location;
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
        $('#AddressPlace').val(AddressItem.AddressPlace);
        $('#FormIndex').val(AddressItem.Index);
        $("#State").trigger("change");
        setTimeout(function () {
            $("#DistrictID option[value='" + AddressItem.DistrictID + "']").attr("selected", "selected");
        }, 100);
        if (AddressItem.IsBilling) {
            $('#IsBillingAddress').iCheck('check');
        }
        else {
            $('#IsBillingAddress').iCheck('uncheck');
        }
        if (AddressItem.IsShipping) {
            $('#IsShippingAddress').iCheck('check');
        }
        else {
            $('#IsShippingAddress').iCheck('uncheck');
        }
        $('#IsDefault').val(AddressItem.IsDefault);
        $('#IsDefaultShipping').val(AddressItem.IsDefaultShipping);
    },
    clearAddressData: function () {
        $('#AddressID').val(0);
        $('#Address1').val('');
        $('#Address2').val('');
        $('#Address3').val('');
        $('#AddressPlace').val('');
        $('#DistrictID').val('');
        $('#LandLine1').val('');
        $('#LandLine2').val('');
        $('#MobileNo').val('');
        $('#State').val('');
        $('#Fax').val('');
        $('#Email').val('');
        $('#PIN').val('');
        $('#ContactPerson').val('');
        $('#IsBillingAddress').iCheck('uncheck');
        $('#IsShippingAddress').iCheck('uncheck');
        $('#FormIndex').val('');
        $('#IsDefault').val('');
        $('#IsDefaultShipping').val('');
    },
    save: function (event) {
        var self = Location;
        var IsBillingDefaultIndex = clean($('input[name=IsBillingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var IsShippingDefaultIndex = clean($('input[name=IsShippingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var AddressListLength = self.AddressList.length;
        for (var j = 0; j < AddressListLength; j++) {
            if (self.AddressList[j].SameAsBilling == true) {
                if (self.AddressList[j].Index == IsBillingDefaultIndex && self.AddressList[j].Index == IsShippingDefaultIndex) {
                    self.AddressList[j].IsDefault = true;
                    self.AddressList[j].IsDefaultShipping = true;
                } else {
                    self.AddressList[j].Index == IsBillingDefaultIndex ? self.AddressList[j].IsDefault = true : self.AddressList[j].IsDefault = false;
                    self.AddressList[j].Index == IsShippingDefaultIndex ? self.AddressList[j].IsDefaultShipping = true : self.AddressList[j].IsDefaultShipping = false;
                }
            } else {
                if (self.AddressList[j].Index == IsBillingDefaultIndex && self.AddressList[j].IsBilling == true) {
                    self.AddressList[j].IsDefault = true;
                } if (self.AddressList[j].Index == IsShippingDefaultIndex && self.AddressList[j].IsShipping == true) {
                    self.AddressList[j].IsDefaultShipping = true;
                }
            }
        }
        var model = self.get_form_data();
        model.AddressList = self.AddressList;
        $(".BtnSave").css({ 'visibility': 'hidden' });
        console.log(model);
        $.ajax({
            url: '/Masters/Location/Save',
            data: { model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Location created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Location/Index"
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.BtnSave').css({ 'visibility': 'visible' });
                }
            },
        });

    },
    validate_form: function (FormType) {
        var self = Location;
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
    GetAddressList: function () {
        var self = Location;
        var locationID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Location/GetAddressList',
            data: { locationID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i].Index = i;
                    if (data[i].IsBilling == data[i].IsShipping) {
                        data[i].SameAsBilling = true;
                    }
                    self.AddressList.push(data[i]);
                    self.PopulateAddress(data[i]);
                }
                console.log(self.AddressList);
            },
        });
    },
    error_count: 0,
}