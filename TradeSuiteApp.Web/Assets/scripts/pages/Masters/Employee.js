$(function () {

});
Employee = {
    LocationList: [],
    init: function () {
        var self = Employee;
        self.bind_events();
        $('.dropify').dropify();
        //self.GetSalaryComponents();
        if ($('#ID').val() != 0) {
            self.GetAddressList();
            self.GetFreeMedicineLocationList();
        }
        self.show_esi_items();
        self.show_erpuser_items();
        self.show_pf_items();
        self.show_probation_items();

        employee_list = self.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        
    },

    select_employee: function () {
        var self = Employee;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#ReportingName").val(Name);
        $("#ReportingID").val(ID);
        $("#ReportingCode").val(Code);
        UIkit.modal($('#select-employee')).hide();
    },

    employee_list: function (type) {
        if (typeof type == "undefined") {
            type = "all";
            url = "/Masters/Employee/GetAllEmployeeList";
        }
        if (type == "all") {
            url = "/Masters/Employee/GetAllEmployeeList";
        } else if (type == "user-role") {
            url = "/Masters/Employee/GetEmployeeListForUserRole";
        }
        else if (type == "FreeMedicineEmployeeList") {
            url = "/Masters/Employee/GetFreeMedicineEmployeeList";
        }
        var $list = $('#employee-list');
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
                            { Key: "EmployeeCategoryID", Value: $('#EmployeeCategoryID').val() },
                            { Key: "DefaultLocationID", Value: $('#LocationID').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio EmployeeID' name='EmployeeID' data-md-icheck value='" + row.ID + "' >"
                            + "<input type='hidden' class='UserID' value='" + row.UserID + "'>"
                            + "<input type='hidden' class='BalAmount' value='" + row.BalAmount + "'>"
                            + "<input type='hidden' class='DesignationID' value='" + row.DesignationID + "'>"
                            + "<input type='hidden' class='Designation' value='" + row.Designation + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Department", "className": "Department" },
                    { "data": "Location", "className": "Location" },


                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#EmployeeCategoryID,#LocationID', function () {
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
        }
    },

    employee_free_medicine_list: function (type) {
        var $list = $('#employee-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Employee/GetAllEmployeeList";

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
                            { Key: "EmployeeCategoryID", Value:0 },
                            { Key: "DefaultLocationID", Value:0 },
                            { Key: "Type", Value: 'FreeMedicineEmployeeList' },

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
                             return "<input type='radio' class='uk-radio EmployeeID' name='EmployeeID' data-md-icheck value='" + row.ID + "' >"
                             + "<input type='hidden' class='BalAmount' value='" + row.BalAmount + "'>";
                         }
                     },
                     { "data": "Code", "className": "Code" },
                     { "data": "Name", "className": "Name" },
                     { "data": "Department", "className": "Department" },
                     { "data": "Location", "className": "Location" },


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
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    employee_free_medicine_lists: function (type) {
        var $list = $('#employees-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Employee/GetAllEmployeeList";

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
                            { Key: "EmployeeCategoryID", Value: 0 },
                            { Key: "DefaultLocationID", Value: 0 },
                            { Key: "Type", Value: 'FreeMedicineEmployeeList' },

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
                             return "<input type='radio' class='uk-radio EmployeeID' name='EmployeeID' data-md-icheck value='" + row.ID + "' >"
                             + "<input type='hidden' class='BalAmount' value='" + row.BalAmount + "'>";
                         }
                     },
                     { "data": "Code", "className": "Code" },
                     { "data": "Name", "className": "Name" },
                     { "data": "Department", "className": "Department" },
                     { "data": "Location", "className": "Location" },


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
            $('body').on("change", '#ItemCategory', function () {
                list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    list: function () {
        $list = $('#employee-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#employee-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Employee/Details/' + Id;
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
        var self = Employee
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $("#btnOKEmployee").on("click", self.select_employee);
        $('#btnAddExEmpAddress').on("click", self.AddExEmpDetails)
        $("#State").on('change', self.GetDistrict);
        $("#ExEmpStateID").on('change', self.GetDistrict);

        $("body").on("ifChecked", "#tbl-address .IsBillingDefault", self.set_default_billing_address);
        $("body").on("ifChecked", "#tbl-address .IsShippingDefault", self.set_default_shipping_address);
        $("body").on('click', 'button#btnAddAddress', self.AddAddress);
        $("#tbl-address").on('click', 'tbody tr', self.EditAddress);

        $("body").on('ifChanged', '.pfstatus', self.show_pf_items);
        $("body").on('ifChanged', '.esistatus', self.show_esi_items);
        $("body").on('ifChanged', '.erpuser', self.show_erpuser_items);
        $("body").on('ifChanged', '.probation', self.show_probation_items);
        $("body").on('ifChanged', '.salary-components', self.include_item);
        $("body").on("keyup change", ".salarymonthly", self.update_annual_salary);
        $('.select-all').on('ifChanged', self.select_results);
        $(".btnSave").on("click", self.save_confirm);
        $("#btnAddItem").on("click", self.Save_default_store);

        $("body").on("click", ".remove-item", self.delete_item);
        $("body").on("change", "#UserLocationID", self.get_default_store);

        $(".CustomerLocation").on('ifChanged', self.AddLocationMappingList)
    },

    AddLocationMappingList: function () {
        var self = Employee;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Employee-As-Customer-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Employee-As-Customer-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },


    save_confirm: function () {
        var self = Employee
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

    set_default_billing_address: function () {
        var self = Employee;
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
        var self = Employee;
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

    AddAddress: function () {
        var self = Employee;
        var Address;
        self.error_count = 0;
        self.count();
        var FormType = "Address";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        Address = Employee.GetAddress();
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
      
        
        //if ($('#DistrictID option:selected') == false) {
        //    $('#DistrictID').val('');
        //}
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
            State: $('#State').val() == 0 ? "" : $('#State option:selected').text(),
            PIN: $('#PIN').val(),
            Fax: $('#Fax').val(),
            Email: $('#Email').val(),
            DistrictID: clean($('#DistrictID').val()),
            District: $('#DistrictID').val() == "Select" ? "" : $('#DistrictID option:selected').text(),
            IsBilling: $('#IsBillingAddress').prop('checked'),
            IsShipping: $('#IsShippingAddress').prop('checked'),
            IsDefault: $('#IsDefault').val() == "" ? false : ($('#IsDefault').val() == 'true'),
            IsDefaultShipping: $('#IsDefaultShipping').val() == "" ? false : ($('#IsDefaultShipping').val() == 'true'),
            Index: Employee.AddressList.length,

        }
        return Data
    },

    clearAddressData: function () {
        $('#AddressID').val(0);
        $('#Address1').val('');
        $('#Address2').val('');
        $('#Address3').val('');
        $('#Place').val('');
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

    EditAddress: function () {
        var self = Employee;
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
        var self = Employee;
        var EmployeeID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Employee/GetAddressList',
            data: { EmployeeID },
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

    AddressEditPage: function (Address) {
        var self = Employee;
        var icheckBilling = "disabled", icheckShipping = "disabled", billing = "", shipping = "";
        var defaultBilling = "", defaultShipping = "";
        var html = "";

        if (Address.IsBilling) {
            icheckBilling = "";
            billing = "Present Address";
            if (Address.IsDefault) {
                defaultBilling = "checked";
            }
        }
        if (Address.IsShipping) {
            icheckShipping = "";
            shipping = "Permanent Address";
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
        var self = Employee;
        var icheckBilling = "disabled", icheckShipping = "disabled", billing = "", shipping = "";
        var html = "";

        if (Address.IsBilling) {
            icheckBilling = "";
            billing = "Present Address";
            if (Address.IsDefault) {
                icheckBilling = "checked";
            }
        }
        if (Address.IsShipping) {
            icheckShipping = "";
            shipping = "Permanent Address";
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


    delete_item: function () {
        var self = Employee;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#tbl-ExEmp-address tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });


    },

    AddressList: [],

    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#ReportingName').val(),
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
        var self = Employee;
        console.log(item)
        $("#EmployeeID").val(item.id),
        $("#ReportingName").val(item.name);
        $("#ReportingCode").val(item.Code);
    },

    validate_form: function (FormType) {
        var self = Employee;
        if (FormType == "Form") {
            if (self.rules.on_submit.length) {
                return form.validate(self.rules.on_submit);
            }
        } else if (FormType == "Present") {
            if (self.rules.on_Billing_submit.length) {
                return form.validate(self.rules.on_Billing_submit);
            }
        } else if (FormType == "Address") {
            if (self.rules.on_address_submit.length) {
                return form.validate(self.rules.on_address_submit);
            }
        } else if (FormType == "store") {
            if (self.rules.on_store_submit.length) {
                return form.validate(self.rules.on_store_submit);
            }
        }
        return 0;
    },


    Add_ExEmp_Address: function () {
        var self = Employee;
        self.error_count = 0;
        var FormType = "exemployee";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }

    },
    //form_item: function () {
    //    var self = Employee;
    //    self.error_count = 0;
    //    var FormType = "Form";
    //    self.error_count = self.validate_form(FormType);
    //    if (self.error_count > 0) {
    //        return;
    //    }
    //},
    show_pf_items: function () {
        if ($("#PFStatus").prop('checked') == true) {
            $('.pfvoluntarycontribution').removeClass('uk-hidden');
            $('.pfaccountno').removeClass('uk-hidden');
            $('.pfuan').removeClass('uk-hidden');


        } else {
            $('.probationduration').addClass('uk-hidden');
            $('.pfvoluntarycontribution').addClass('uk-hidden');
            $('.pfaccountno').addClass('uk-hidden');
            $('.pfuan').addClass('uk-hidden');
        }
    },
    show_esi_items: function () {
        if ($("#ESIStatus").prop('checked') == true) {
            $('.esino').removeClass('uk-hidden');

        } else {
            $('.esino').addClass('uk-hidden');
        }
    },
    show_erpuser_items: function () {
        if ($("#IsERPUser").prop('checked') == true) {
            var uesrname = $('#UserName').val();
            $('.erpusername').removeClass('uk-hidden');
            $('#UserName').val(uesrname);
            $('.erppassword').removeClass('uk-hidden');
            $('.erpchangepassword').removeClass('uk-hidden');
        } else {
            $('.erpusername').addClass('uk-hidden');
            $('.erppassword').addClass('uk-hidden');
            $('.erpchangepassword').addClass('uk-hidden');
        }
    },
    show_probation_items: function () {
        if ($("#Probation").prop('checked') == true) {
            $('.probationduration').removeClass('uk-hidden');;
        } else {
            $('.probationduration').addClass('uk-hidden');
        }
    },
    include_item: function () {

        if ($(this).is(":checked")) {
            $(this).closest('tr').addClass('included');
            $(this).closest('tr').find('.salarymonthly').prop("disabled", false);
        } else {
            $(this).closest('tr').removeClass('included');
            $(this).closest('tr').find('.salarymonthly').prop("disabled", true);
        }
    },
    update_annual_salary: function () {
        var mSalary = clean($(this).closest('tr').find('.salarymonthly').val());
        $(this).closest('tr').find('.salaryannual').val(mSalary * 12);
    },
    select_results: function () {
        if ($(this).prop('checked') == true) {
            $(this).closest('table').find('.salary-components').iCheck('check');
        } else {
            $(this).closest('table').find('.salary-components').iCheck('uncheck');

        }
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
                });
                if (state.attr('id') == "ExEmpStateID") {
                    $("#ExEmpDistrictID").html("");
                    $("#ExEmpDistrictID").append(html);
                }
                else {
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                }
            }
        });
    },

    validate_EXemployeeForm: function () {
        var self = Employee;
        if (self.rules.on_ExEmployee_submit.length) {
            return form.validate(self.rules.on_ExEmployee_submit);
        }
        return 0;
    },

    AddExEmpDetails: function () {
        var self = Employee;
        self.error_count = 0;
        self.error_count = self.validate_EXemployeeForm();
        if (self.error_count > 0) {
            return;
        }

        var EmployerName = $("#EmployerName").val();
        var CountryID = $("#ExEmpCountry Option:selected").val();
        var Country = $("#ExEmpCountry Option:selected").text();
        var StateID = $("#ExEmpStateID Option:selected").val();
        var State = $("#ExEmpStateID Option:selected").text();
        var Designation = $("#ExDesignation").val();
        var DateOfJoining = $("#ExDateOfJoining").val();
        var District = $("#ExEmpDistrictID Option:selected").text();
        var DistrictID = $("#ExEmpDistrictID Option:selected").val();
        var DateOfSeverance = $("#ExDateOfSeverance").val();
        var Address1 = $("#ExEmpAddress1").val();
        var Address2 = $("#ExEmpAddress2").val();
        var Address3 = $("#ExEmpAddress3").val();
        var Place = $("#ExEmpPlace").val();
        var Pin = $("#ExEmpPIN").val();
        var ContactPerson = $("#ExEmpContactPerson").val();
        var Email = $("#ExEmpEmail").val();
        var ContactNumber = $("#ExEmpContactNumber").val();
        var content = "";
        var $content;
        var sino = "";
        sino = $('#tbl-ExEmp-address tbody tr').length + 1;
        content = '<tr>'
            + '<td class="uk-text-center serial-no">' + sino + '</td>'
            + '<td class="Employer-name">' + EmployerName
            + '<input type="hidden" class = "state-ID" value="' + StateID + '" />'
            + '<input type="hidden" class = "district-ID" value="' + DistrictID + '" />'
            + '<input type="hidden" class = "country-ID" value="' + CountryID + '" />'
            + '<td class="designation">' + Designation + '</td>'
            + '<td class="address">' + Address1 + '<br/>' + Address2 + '<br>' + Address3 + '<br/>' + Place + '<br/>' + Pin
            + '<input type="hidden" class = "address1" value="' + Address1 + '" />'
            + '<input type="hidden" class = "address2" value="' + Address2 + '" />'
            + '<input type="hidden" class = "address3" value="' + Address3 + '" />'
            + '<input type="hidden" class = "place" value="' + Place + '" />'
            + '<input type="hidden" class = "pin" value="' + Pin + '" />'
            + '</td>'
            + '<td class="date-of-joinning">' + DateOfJoining + '</td>'
            + '<td class="date-of-severance">' + DateOfSeverance + '</td>'
            + '<td class="contact-person">' + ContactPerson + '</td>'
            + '<td class="contact-number">' + ContactNumber + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#tbl-ExEmp-address tbody').append($content);
        index = $("#tbl-ExEmp-address tbody tr").length;
        $("#ExEmp-address-count").val(index);
        self.clear_exempoyerdata();

    },

    clear_exempoyerdata: function () {
        var self = Employee;
        $("#EmployerName").val('');
        $("#ExEmpCountry Option:selected").val('');
        $("#ExEmpCountry Option:selected").text('Select');
        $("#ExEmpStateID Option:selected").val('');
        $("#ExEmpStateID Option:selected").text('Select');
        $("#ExDesignation").val('');
        $("#ExDateOfJoining").val('');
        $("#ExEmpDistrictID Option:selected").text('Select');
        $("#ExEmpDistrictID Option:selected").val('');
        $("#ExDateOfSeverance").val('');
        $("#ExEmpAddress1").val('');
        $("#ExEmpAddress2").val('');
        $("#ExEmpAddress3").val('');
        $("#ExEmpPlace").val('');
        $("#ExEmpPIN").val('');
        $("#ExEmpContactPerson").val('');
        $("#ExEmpEmail").val('');
        $("#ExEmpContactNumber").val('');
    },

    save: function () {
        var self = Employee;
        var data = self.get_data();
        var location = "/Masters/Employee/Index";
        $.ajax({
            url: '/Masters/Employee/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },
    get_data: function () {
        var self = Employee;
        var data = {};
        data.ASPNetUserID = clean($("#ASPNetUserID").val());
        data.ID = $("#ID").val();
        data.Code = $("#Code").val();
        data.Title = $("#Title Option:selected").text() != "Select" ? $("#Title Option:selected").val() : '';
        data.Name = $("#Name").val();
        data.DesignationID = $("#DesignationID Option:selected").val();
        data.Gender = $("#Gender Option:selected").text();
        data.MartialStatus = $("#MartialStatus Option:selected").text() != "Select" ? $("#MartialStatus Option:selected").text() : "";
        data.Qualification1 = $("#Qualification1").val();
        data.DOB = $("#DOB").val();
        data.Qualification2 = $("#Qualification2").val();
        data.Qualification3 = $("#Qualification3").val();
        data.BloodGroup = $("#BloodGroupID Option:selected").val() > 0 ? $("#BloodGroupID Option:selected").text() : '';
        data.NoOfDependent = $("#NoOfDependent").val();
        data.NameOfSpouse = $("#NameOfSpouse").val();
        data.NameOfGuardian = $("#NameOfGuardian").val();
        if ($("#IsExcludeFromPayroll").prop('checked') == true) {
            data.IsExcludeFromPayroll = true;
        } else {
            data.IsExcludeFromPayroll = false;
        }
        data.DateOfJoining = $("#DateOfJoining").val();
        data.DateOfConfirmation = $("#DateOfConfirmation").val();
        data.PayGrade = $("#PayGrade").val();
        data.EmpCategoryID = $("#EmpCategoryID Option:selected").val();
        data.PayrollCategoryID = $("#PayrollCategoryID Option:selected").val();
        data.CompanyEmail = $("#CompanyEmail").val();
        data.ReportingCode = $("#ReportingCode").val();
        data.ReportingName = $("#ReportingName").val();
        data.DepartmentID = $("#DepartmentID Option:selected").val();
        data.LocationID = $("#LocationID Option:selected").val();
        data.InterCompanyID = $("#InterCompanyID Option:selected").val();
        data.DateOfSeverance = $("#DateOfSeverance").val();
        data.DateOfReJoining = $("#DateOfReJoining").val();
        data.ProbationDuration = $("#ProbationDuration").val();
        data.EmploymentJobTypeID = $("#EmploymentJobTypeID Option:selected").val();
        data.PrintPayroll = $("#PrintPayroll Option:selected").text() != "Select" ? $("#PrintPayroll Option:selected").text() : '';
        data.PFVoluntaryContribution = $("#PFVoluntaryContribution").val();
        data.PFAccountNo = $("#PFAccountNo").val();
        data.PFUAN = $("#PFUAN").val();
        data.ESINo = $("#ESINo").val();
        data.IsAlreadyERPUser = $("#IsAlreadyERPUser").val() == "True" ? true : false;
        data.UserName = $("#UserName").val();
        if ($("#PFStatus").prop('checked') == true) {
            data.PFStatus = true;
        } else {
            data.PFStatus = false;
        }
        if ($("#ESIStatus").prop('checked') == true) {
            data.ESIStatus = true;
        } else {
            data.ESIStatus = false;
        }
        if ($("#NPS").prop('checked') == true) {
            data.NPSStatus = true;
        } else {
            data.NPSStatus = false;
        }
        if ($("#MedicalInsurance").prop('checked') == true) {
            data.MedicalInsuranceStatus = true;
        } else {
            data.MedicalInsuranceStatus = false;
        }
        if ($("#AttandancePunching").prop('checked') == true) {
            data.AttandancePunchingStatus = true;
        } else {
            data.AttandancePunchingStatus = false;
        }
        if ($("#MultiLocationPunching").prop('checked') == true) {
            data.MultiLocationPunchingStatus = true;
        } else {
            data.MultiLocationPunchingStatus = false;
        }
        if ($("#SpecialLeave").prop('checked') == true) {
            data.SpecialLeaveStatus = true;
        } else {
            data.SpecialLeaveStatus = false;
        }
        if ($("#Probation").prop('checked') == true) {
            data.ProbationStatus = true;
        } else {
            data.ProbationStatus = false;
        }
        if ($("#ProductionIncentive").prop('checked') == true) {
            data.ProductionIncentiveStatus = true;
        } else {
            data.ProductionIncentiveStatus = false;
        }
        if ($("#SalesIncentive").prop('checked') == true) {
            data.SalesIncentiveStatus = true;
        } else {
            data.SalesIncentiveStatus = false;
        }
        if ($("#FixedIncentive").prop('checked') == true) {
            data.FixedIncentiveStatus = true;
        } else {
            data.FixedIncentiveStatus = false;
        }
        if ($("#IsERPUser").prop('checked') == true) {
            data.IsERPUser = true;
            data.Password = $('#Password').val();
        } else {
            data.IsERPUser = false;
        }
        if ($("#MedicalAid").prop('checked') == true) {
            data.MedicalAidStatus = true;
        } else {
            data.MedicalAidStatus = false;
        }
        if ($("#minimumwages").prop('checked') == true) {
            data.MinimumWagesStatus = true;
        } else {
            data.MinimumWagesStatus = false;
        }
        if ($("#Bonus").prop('checked') == true) {
            data.BonusStatus = true;
        } else {
            data.BonusStatus = false;
        }
        if ($("#professionalTax").prop('checked') == true) {
            data.ProfessionalTaxStatus = true;
        } else {
            data.ProfessionalTaxStatus = false;
        }
        if ($("#WelfareDeduction").prop('checked') == true) {
            data.WelfareDeductionStatus = true;
        } else {
            data.WelfareDeductionStatus = false;
        }
        data.PanNo = $("#PanNo").val();
        data.AadhaarNo = $("#AadhaarNo").val();
        data.AccountNumber = $("#AccountNumber").val();
        data.BankName = $("#BankName").val();
        data.BankBranchName = $("#BankBranchName").val();
        data.IFSC = $("#IFSC").val();
        if ($("#English").prop('checked') == true) {
            data.IsEnglish = true;
        } else {
            data.IsEnglish = false;
        }
        if ($("#Hindi").prop('checked') == true) {
            data.IsHindi = true;
        } else {
            data.IsHindi = false;
        }
        if ($("#Malayalam").prop('checked') == true) {
            data.IsMalayalam = true;
        } else {
            data.IsMalayalam = false;
        }
        if ($("#Tamil").prop('checked') == true) {
            data.IsTamil = true;
        } else {
            data.IsTamil = false;
        }
        if ($("#Telugu").prop('checked') == true) {
            data.IsTelugu = true;
        } else {
            data.IsTelugu = false;
        }
        if ($("#Kannada").prop('checked') == true) {
            data.IsKannada = true;
        } else {
            data.IsKannada = false;
        }

        if ($("#ChangePassword").prop('checked') == true) {
            data.ChangePassword = true;
        } else {
            data.ChangePassword = false;
        }
        data.ExEmployDetails = [];
        var ExEmployDetail = {};
        $('#tbl-ExEmp-address tbody tr ').each(function () {
            ExEmployDetail = {};
            ExEmployDetail.EmployerName = $(this).find(".Employer-name").text();
            ExEmployDetail.StateID = $(this).find(".state-ID").val();
            ExEmployDetail.DistrictID = $(this).find(".district-ID").val();
            ExEmployDetail.CountryID = $(this).find(".country-ID").val();
            ExEmployDetail.Designation = $(this).find(".designation").text();
            ExEmployDetail.ExEmployAddress1 = $(this).find(".address1").val();
            ExEmployDetail.ExEmployAddress2 = $(this).find(".address2").val();
            ExEmployDetail.ExEmployAddress3 = $(this).find(".address3").val();
            ExEmployDetail.ExEmployPlace = $(this).find(".place").val();
            ExEmployDetail.ExEmployPin = $(this).find(".pin").val();
            ExEmployDetail.DateOfJoinning = $(this).find(".date-of-joinning").text();
            ExEmployDetail.DateOfSeverance = $(this).find(".date-of-severance").text();
            ExEmployDetail.ContactPerson = $(this).find(".contact-person").text();
            ExEmployDetail.ContactNumber = $(this).find(".contact-number").text();
            data.ExEmployDetails.push(ExEmployDetail);
        });
        data.AddressList = self.AddressList;
        data.SalaryDetails = [];
        var SalaryDetail = {};
        $('#SalaryComponentList tbody tr ').each(function () {
            SalaryDetail = {};
            if ($(this).find(".salary-components").prop('checked') == true) {
                SalaryDetail.PayType = $(this).find(".paytype").text().trim();
                SalaryDetail.SalaryMonthly = clean($(this).find(".salarymonthly").val());
                SalaryDetail.SalaryAnnual = clean($(this).find(".salaryannual").val());
                if ($(this).find(".finance").prop('checked') == true) {
                    SalaryDetail.IsFinancePayRoll = true;
                }
                else {
                    SalaryDetail.IsFinancePayRoll = false;
                }
                if ($(this).find(".production").prop('checked') == true) {
                    SalaryDetail.IsProductionIncentivePayRoll = true;
                }
                else {
                    SalaryDetail.IsProductionIncentivePayRoll = false;
                }
                if ($(this).find(".sales").prop('checked') == true) {
                    SalaryDetail.IsSalesIncentivePayRoll = true;
                }
                else {
                    SalaryDetail.IsSalesIncentivePayRoll = false;
                }
                data.SalaryDetails.push(SalaryDetail);
            }

        });

        data.FreeMedicineLocationList = self.LocationList

        return data;
    },

    GetFreeMedicineLocationList: function () {
        var self = Employee;
        var EmployeeID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Employee/GetFreeMedicineLocationsByEmployeeID',
            data: { EmployeeID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $CustomerLocationDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].LocationName + "'> </div>"
                    app.format($CustomerLocationDetail);
                    $('#Location-Detail-Container').append($CustomerLocationDetail);
                    $('.CustomerLocation').each(function () {
                        if ($(this).val() == data[i].CustomerLocationID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });
    },

    get_defaultstore_data: function () {
        var self = Employee;
        var data = {
            StoreID: $("#StoreID").val(),
            StoreName: $("#StoreID option:selected").text(),
            UserID: $("#ASPNetUserID").val(),
            LocationID: $("#UserLocationID").val(),
        }
        return data;
    },

    Save_default_store: function () {
        var self = Employee
        var FormType = "store";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        var data = self.get_defaultstore_data();
        $.ajax({
            url: '/Masters/Configuration/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Default Store Saved successfully");
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },

    get_default_store: function () {
        var self = Employee;
        var location_id = clean($("#UserLocationID").val());
        var UserID = $("#ID").val();
        $.ajax({
            url: '/Masters/Configuration/GetDefaultStoreList/',
            dataType: "json",
            type: "POST",
            data: {
                LocationID: location_id,
                UserID: UserID
            },
            success: function (response) {
                $("#StoreID").html("");
                if (response.Status == "success") {
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option " + ((response.SelectedStoreID == record.StoreID) ? "selected='selected'" : "") + " value='" + record.StoreID + "'>" + record.StoreName + "</option>";
                    });
                    $("#StoreID").append(html);
                }
            }
        });
    },

    count: function () {
        index = $("#tbl-address tbody tr").length;
        $("#address-count").val(index);
    },

    rules: {
        on_submit: [

                        {
                            elements: "#Name",
                            rules: [
                        { type: form.required, message: "Name is required" }
                            ]
                        },
                         {
                             elements: "#Password",
                             rules: [
                         {
                             type: function (element) {
                                 var error = false;
                                 if ((($("#IsERPUser").prop('checked') == true)) && ($("#IsAlreadyERPUser").val() == "False") && (clean($("#ID").val()) == 0) || ($("#ChangePassword").prop('checked') == true)) {
                                     if ($('#Password').val() != '') { error = false; }
                                     else
                                     { error = true; }
                                 }
                                 return !error;
                             }, message: "password is required"
                         },
                             ]
                         },
                         {
                             elements: "#UserName",
                             rules: [
                         {
                             type: function (element) {
                                 var error = false;
                                 if ((($("#IsERPUser").prop('checked') == true)) && ($("#IsAlreadyERPUser").val() == "False") && (clean($("#ID").val()) == 0) || ($("#ChangePassword").prop('checked') == true)) {
                                     if ($('#UserName').val() != '') { error = false; }
                                     else
                                     { error = true; }
                                 }
                                 return !error;
                             }, message: "Userame is required"
                         },
                             ]
                         },
                        {
                            elements: "#Designation",
                            rules: [
                        { type: form.required, message: "Designation is required" }
                            ]
                        },
                         {
                             elements: "#Gender",
                             rules: [
                         { type: form.required, message: "Gender is required" }
                             ]
                         },
                           {
                               elements: "#EmpCategoryID",
                               rules: [
                           { type: form.required, message: "EmpCategory is required" }
                               ]
                           },
       ],
        on_address_submit: [
       {
           elements: "#MobileNo",
           rules: [
       { type: form.required, message: "Mobile Number is required" },
           ]
       },
      
       ],
        on_store_submit: [
                           //{
                           //    elements: "#UserLocationID",
                           //    rules: [
                           //{ type: form.required, message: "Please select Location" },
                           //    ]
                           //},
                           //{
                           //    elements: "#StoreID",
                           //    rules: [
                           //{ type: form.required, message: "Please select store" },
                           //    ]
                           //},

        ],
        on_ExEmployee_submit: [
                            {
                                elements: "#ExEmpAddress1",
                                rules: [
                            { type: form.required, message: "Address Line 1 is required" },
                                ]
                            },
                            {
                                elements: "#EmployerName",
                                rules: [
                            { type: form.required, message: "EmployerName is required" },
                                ]
                            },
                            {
                                elements: "#ExDesignation",
                                rules: [
                            { type: form.required, message: "Designation is required" },
                                ]
                            },



                            {
                                elements: "#ExDateOfJoining",
                                rules: [
                            { type: form.required, message: "DateOfJoining is required" },
                                ]
                            },

                               {
                                   elements: "#ExEmpPlace",
                                   rules: [
                               { type: form.required, message: "Place is required" },
                                   ]
                               },
                               {
                                   elements: "#ExEmpStateID",
                                   rules: [
                               { type: form.required, message: "Select   State" },
                                   ]
                               },
                               {
                                   elements: "#ExEmpDistrictID",
                                   rules: [
                               { type: form.required, message: "Select  District" },
                                   ]
                               },
                               {
                                   elements: "#ExEmpCountry",
                                   rules: [
                               { type: form.required, message: "Select  country" },
                                   ]
                               },
        ]

    },



};