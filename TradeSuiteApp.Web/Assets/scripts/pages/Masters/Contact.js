Contact = {

    init: function () {
        var self = Contact;

        Customer.customer_list("All");
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
           
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });

        //self.sales_order_list();
        //self.proforma_invoice_list();

      

      

        self.contactList();
        self.bind_events();
  
       


    },
    contactList: function () {
        debugger
        var self = Contact;
        $contact_list = $('#contact-list');
        if ($contact_list.length) {
            $contact_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#contact-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Contact/Details/' + Id;
            });
            altair_md.inputs();
            var contact_list_table = $contact_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            contact_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    contact_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        var self = Contact;

        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("#btnOKCustomer").on("click", self.select_customer);
        $(".btnSave").on('click', self.save_confirm);
        $(".btnUpdate").on('click', self.update);
        $('#ContactNo').on('keydown', self.phone_validate);
    },
    get_customers: function (release) {
        var self = Contact;
        $.ajax({
            url: '/Masters/Customer/GetSalesInvoiceCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
                CustomerCategoryID: $('#CustomerCategoryID').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    phone_validate: function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
        var keyCode = e.which || e.keyCode;


        if ($(this).val().length >= 10) {
            e.preventDefault();
        }
    },
     select_customer: function () {
         var self = Contact;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var CustomerCategory = $(row).find(".CustomerCategory").text().trim();
        var StateID = $(row).find(".StateID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var MinCreditLimit = $(row).find(".MinimumCreditLimit").val();
        var MaxCreditLimit = $(row).find(".MaxCreditLimit").val();
        var CashDiscountPercentage = $(row).find(".CashDiscountPercentage").val();
        var CustomercategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerCategoryID").val(CustomercategoryID);
         $("#Company").val(Name);
        $("#CustomerID").val(ID);
        $("#CustomerID").trigger("change");
        $("#StateID").val(StateID);
        $("#SchemeID").val(SchemeID);
        $("#PriceListID").val(PriceListID);
        $("#IsGSTRegistered").val(IsGSTRegistered.toLowerCase());
        $("#CustomerCategory").val(CustomerCategory);
        $("#MinCreditLimit").val(MinCreditLimit);
        $("#MaxCreditLimit").val(MaxCreditLimit);
        $("#CashDiscountPercentage").val(CashDiscountPercentage);
        UIkit.modal($('#select-customer')).hide();
        //self.Invoice.CustomerID = ID;
        //self.customer_on_change();
        //self.on_change_customer_category();
    },
    update: function () {
        var self = Contact;
        //self.error_count = 0;
        //self.error_count = self.validate_form();
        //if (self.error_count > 0) {
        //    return;
        //}
        $('.btnUpdate').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Contact/Edit1',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Contact updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Contact/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    set_customer: function (event, item) {
        var self = Contact;
        $("#Company").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#CustomerID").trigger("change");
        $("#StateID").val(item.stateId);
        $("#SchemeID").val(item.schemeId);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#CustomerCategory").val(item.customerCategory);
        $("#MinCreditLimit").val(item.minCreditLimit);
        $("#MaxCreditLimit").val(item.maxCreditLimit);
        $("#CashDiscountPercentage").val(item.cashDiscountPercentage);
        $("#CustomerCategoryID").val(item.customercategoryid);
        //self.Invoice.CustomerID = item.id;
        //self.customer_on_change();
        //self.on_change_customer_category();
    },
    save_confirm: function () {
        var self = Contact;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save: function () {
        var self = Contact;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Contact/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Contact created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Contact/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: clean($("#ID").val()), 
            IsActive: $('.IsReceipt').is(':checked') ? true : false,
           
           
          
            Firstname: $("#Firstname").val(),
            Lastname: $("#Lastname").val(),
            ContactNo: $("#ContactNo").val(),
           
            Email: $("#Email").val(),
          /*  Contact:$("#Contact").val(),*/
            AlternateNo: $("#AlternateNo").val(),
           
            Address1: $("#Address1").val(),
            Address2: $("#Address2").val(),
            Address3: $("#Address3").val(),
            Designation: $("#Designation").val(),
            CustomerID: $("#CustomerID").val(),
        }
        return model;
    },
    error_count: 0,
    validate_form: function () {
        var self = Contact;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#Firstname",
                rules: [
                    { type: form.required, message: "Firstname is required" },
                ]
            },
            {
                elements: "#Lastname",
                rules: [
                    { type: form.required, message: "Lastname is required" },
                ]
            },
            {
                elements: "#Email",
                rules: [
                    { type: form.required, message: "Email is required" },
                ]
            },
         
            {
                elements: "#ContactNo",
                rules: [
                    { type: form.required, message: "ContactNo is required" },
                    
                ]
            },
            {
                elements: "#Company",
                rules: [
                    { type: form.required, message: "Company is required" },
                ]
            },
            {
                elements: "#Designation",
                rules: [
                    { type: form.required, message: "Designation is required" },
                ]
            },

        ]
    }
}