Customer = {
    AddressList: [],
    LocationList: [],
    _customer_list: function (type) {
        var url;
        var url;
        if (type == "sales-order") {
            url = "/Masters/Customer/GetSalesOrderCustomerList";
        } else if (type == "sales-invoice") {
            url = "/Masters/Customer/GetSalesInvoiceCustomerList";
        } else {
            url = "/Masters/Customer/GetCustomerList"
        }

        var $list = $('#customer-list');
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
                            { Key: "CustomerCategoryID", Value: $('#CustomerCategoryID').val() },
                            { Key: "StateID", Value: $('#CustomerStateID').val() },

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
                            return "<input type='radio' class='uk-radio CustomerID' name='CustomerID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='StateID' value='" + row.StateID + "'>"
                                + "<input type='hidden' class='PriceListID' value='" + row.PriceListID + "'>"
                                + "<input type='hidden' class='CountryID' value='" + row.CountryID + "'>"
                                + "<input type='hidden' class='DistrictID' value='" + row.DistrictID + "'>"
                                + "<input type='hidden' class='CustomerCategoryID' value='" + row.CustomerCategoryID + "'>"
                                + "<input type='hidden' class='SchemeID' value='" + row.SchemeID + "'>"
                                + "<input type='hidden' class='MinimumCreditLimit' value='" + row.MinimumCreditLimit + "'>"
                                + "<input type='hidden' class='MaxCreditLimit' value='" + row.MaxCreditLimit + "'>"
                                + "<input type='hidden' class='CashDiscountPercentage' value='" + row.CashDiscountPercentage + "'>"
                                + "<input type='hidden' class='IsBlockedForChequeReceipt' value='" + row.IsBlockedForChequeReceipt + "'>"
                                + "<input type='hidden' class='OutStandingAmount' value='" + row.OutStandingAmount + "'>"
                                + "<input type='hidden' class='CurrencyID' value='" + row.CurrencyID + "'>"
                                + "<input type='hidden' class='CurrencyPrefix' value='" + row.CurrencyPrefix + "'>"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.CurrencyCode + "'>"
                                + "<input type='hidden' class='CurrencyConversionRate' value='" + row.CurrencyConversionRate + "'>"
                                + "<input type='hidden' class='DecimalPlaces' value='" + row.DecimalPlaces + "'>"
                                + "<input type='hidden' class='IsGSTRegistered' value='" + row.IsGSTRegistered + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Address", "className": "Address" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
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
                        "data": null,
                        "className": "uk-text-center uk-hidden",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                    { "data": "CurrencyName", "className": "CurrencyName uk-hidden" },
                    { "data": "CurrencyConversionRate", "className": "CurrencyConversionRate uk-hidden" },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#CustomerCategoryID,#CustomerStateID', function () {
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
    },
    get customer_list() {
        return this._customer_list;
    },
    set customer_list(value) {
        this._customer_list = value;
    },
    party_list: function () {
        var $list = $('#party-list');
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
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Masters/Customer/GetPartyList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [

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
                            return "<input type='radio' class='uk-radio PartyID' name='PartyID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Name", "className": "Name" },
                    { "data": "Doctor" },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
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
    },
    doctor_list: function () {
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
                    "url": "/Masters/Customer/GetDoctorList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
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
                            return "<input type='radio' class='uk-radio DoctorID' name='DoctorID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },

                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
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
    },

    details: function () {
    },

    customer_details_list: function () {
        var $list = $('#customer-details-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            var url = "/Masters/Customer/GetCustomerDetails"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[2, "asc"]],
                "ajax": {
                    "url": url,
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
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='CustomerID' value='" + row.ID + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "CategoryName", "className": "Category" },
                    { "data": "Location", "className": "Location" },
                    { "data": "Scheme", "className": "Scheme" },
                    { "data": "DiscountPercentage", "className": "DiscountPercentage uk-text-right" },
                    { "data": "PriceList", "className": "PriceList" },
                    {
                        "data": "MinimumCreditLimit", "searchable": false, "className": "MinCreditLimit",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.MinimumCreditLimit + "</div>";
                        }
                    },
                    {
                        "data": "MaxCreditLimit", "searchable": false, "className": "MaxCreditLimit",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.MaxCreditLimit + "</div>";
                        }
                    },
                    {
                        "data": "OutStandingAmount", "searchable": false, "className": "OutStandingAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.OutStandingAmount + "</div>";
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var CustomerID = $(this).closest("tr").find("td .CustomerID").val();
                        $("#CustomerID").val(CustomerID).trigger("change");
                        UIkit.modal("#select-customer-items").show();
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    customer_item_details_list: function () {
        var $list = $('#customer-item-details-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/Customer/GetCustomerItemDetails"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[2, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CustomerID", Value: $('#CustomerID').val() },
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
                                + "<input type='hidden' class='ItemID' value='" + row.ItemID + "'>";

                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "ItemName", "className": "ItemName" },
                    {
                        "data": "MRP", "searchable": false, "className": "MRP",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.MRP + "</div>";
                        }
                    },
                    { "data": "DiscountPercentage", "className": "DiscountPercentage uk-text-right" },
                    { "data": "InvoiceQty", "className": "Quantity uk-text-right" },
                    { "data": "OfferQty", "className": "OfferQuantity uk-text-right" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $('body').on("change", '#CustomerID', function () {
                list_table.fnDraw();
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    List: function () {
        var self = Customer;

        $('#tabs-customer').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = Customer;

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "saved-customer":
                $list = $('#saved-customer-list');
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

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Masters/Customer/GetCustomerForMenuList?type=" + type,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "Type", Value: type },
                            { Key: "CustomerCategoryID", Value: $('#CustomerCategoryID').val() }
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
                            return meta.settings.oAjaxData.start + meta.row + 1
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >"
                                + "<input type='hidden' class='ID' value='" + row.CustomerCategoryID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Location" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    { "data": "PropratorName", "className": "PropratorName" },
                    { "data": "OldCode", "className": "OldCode" },
                    {
                        "data": null,
                        "className": "uk-text-center",
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
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        window.location = '/Masters/Customer/Details/' + Id;
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

    init: function () {
        var self = Customer;
        self.bind_events();
        $("#GSTRegistered").prop("checked", false);
        $("#GSTNotRegistered").prop("checked", true);
        $(".IsGSTRegistered").trigger("ifChanged");

        if ($('#ID').val() != 0) {
            self.GetAddressList();
            self.GetCustomerLocationMappingList();
            self.GetCheckedData();
        }
        supplier.supplier_list('ALL');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        self.fso_list();
        $('#fso-list').SelectTable({
            selectFunction: self.select_fso,
            modal: "#select-fso",
            initiatingElement: "#FsoName"
        });
    },
    loadDefaultFSO: function () {
        var radio = $('#select-fso tbody input[type="radio"]:first');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".FSOName").text().trim();
        $("#FSOName").val(Name);
        $("#FSOID").val(ID);
    },
    fso_list: function () {
        var self = Customer;
        var $list = $('#fso-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/FSO/GetFSOList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ID", Value: $('#ID').val() },
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
                            return "<input type='radio' class='uk-radio FSOID' name='FSOID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "FSOCode", "className": "FSOCode" },
                    { "data": "FSOName", "className": "FSOName" },
                    { "data": "AreaManager", "className": "AreaManager" },
                    { "data": "ZonalManager", "className": "ZonalManager" },
                    { "data": "SalesManager", "className": "SalesManager" },
                    { "data": "RegionalSalesManager", "className": "RegionalSalesManager" },
                    { "data": "RouteName", "className": "RouteName" },
                ],
                "initComplete": function (settings, json) {
                    self.loadDefaultFSO();
                },
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ID', function () {
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
    },

    select_fso: function () {
        var self = Customer;
        var radio = $('#select-fso tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".FSOName").text().trim();
        $("#FSOName").val(Name);
        $("#FSOID").val(ID);
        $("#FSOID").trigger("change");
        UIkit.modal($('#select-fso')).hide();
    },

    get_fso: function (release) {
        $.ajax({
            url: '/Masters/FSO/GetFSOAutoComplete',
            data: {
                Hint: $('#FSOName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },

    set_fso: function (event, item) {
        var self = Customer;
        console.log(item)
        $("#FSOID").val(item.id),
            $("#FSOName").val(item.FSOName);
    },

    select_supplier: function () {
        var self = Customer;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        var IsInterCompany = $(row).find(".IsInterCompany").val();
        $("#SupplierName").val(Name);
        $("#SupplierID").val(ID);
    },

    bind_events: function () {
        var self = Customer;
        var CustomerID = $('#ID').val();
        $.UIkit.autocomplete($('#fso-autocomplete'), { 'source': self.get_fso, 'minLength': 1 });
        $('#fso-autocomplete').on('selectitem.uk.autocomplete', self.set_fso);
        $("#btn-ok-fso").on("click", self.select_fso);
        $('#IsInterCompany').val() == "True" ? $('.IsInterCompany').iCheck('check') : $('.IsInterCompany').iCheck('uncheck');
        $('#IsMappedtoExpsEntries').val() == "True" ? $('.IsMappedtoExpsEntries').iCheck('check') : $('.IsMappedtoExpsEntries').iCheck('uncheck');
        $('#IsBlockedForSalesOrders').val() == "True" ? $('.IsBlockedForSalesOrders').iCheck('check') : $('.IsBlockedForSalesOrders').iCheck('uncheck');
        $('#IsBlockedForSalesInvoices').val() == "True" ? $('.IsBlockedForSalesInvoices').iCheck('check') : $('.IsBlockedForSalesInvoices').iCheck('uncheck');
        $('#IsAlsoASupplier').val() == "True" ? $('.IsAlsoASupplier').iCheck('check') : $('.IsAlsoASupplier').iCheck('uncheck');
        $('#IsMappedToServiceSales').val() == "True" ? $('.IsMappedToServiceSales').iCheck('check') : $('.IsMappedToServiceSales').iCheck('uncheck');
        $('#GstNo').addClass('visible');
        $("#State").on('change', self.GetDistrict);
        $('.IsGSTRegistered').on('ifChanged', self.IsGSTChanged);
        $(".BtnSave, .btnSaveAsDraft").on("click", self.on_save);
        $("body").on('click', 'button#btnAddAddress', self.AddAddress);
        $(".CustomerLocation").on('ifChanged', self.AddLocationMappingList)
        $("#tbl-address").on('click', 'tbody tr', self.EditAddress);
        $("body").on("ifChanged", "#tbl-address .IsBillingDefault", self.set_default_billing_address);
        $("body").on("ifChanged", "#tbl-address .IsShippingDefault", self.set_default_shipping_address);
        $("#btnOKSupplier").on('click', self.select_supplier);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': self.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_details);
        $('.IsAlsoASupplier').on('ifChanged', self.IsSupplierChanged);
        $("body").on('click', '.btnDelete', self.delete_confirm);
        $("#Address1,#Address2,#Place,#MobileNo,#PIN").on("click", self.address_reset_text_onclick);
        $("body").on("focusout", "#Address1,#Address2,#Place,#MobileNo,#PIN", self.address_reset_text_focusout);
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
    set_supplier_details: function (event, item) {   // on select auto complete item
        var self = Customer;
        $("#SupplierID").val(item.id);
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
    IsSupplierChanged: function () {
        if ($(".IsAlsoASupplier").prop('checked') == true) {
            $('.Supplier').removeClass('uk-hidden');
        } else {
            $('.Supplier').addClass('uk-hidden');
        }
    },
    GetCheckedData: function () {
        var self = Customer;
        $('#IsGSTRegistered').val() == "True" ? $('#GSTRegistered').iCheck('check') : $('#GSTNotRegistered').iCheck('check');
    },

    on_save: function () {

        var self = Customer;
        var data = self.get_form_data();
        var location = "/Masters/Customer/Index";
        var url = "/Masters/Customer/Save";
        var FormType = "Form";

        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = "/Masters/Customer/SaveAsDraft";
            self.error_count = self.validate_form(FormType);
        } else {
            self.error_count = self.validate_form(FormType);
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            var GstNo, mobile, landline, landline2, index;
            $(".BtnSave").css({ 'visibility': 'hidden' });
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
                url: '/Masters/Customer/CheckCustomerAlradyExist',
                data: {
                    ID: clean($("#ID").val()),
                    Name: $("#Name").val(),
                    GstNo: GstNo,
                    PanCardNo: $("#PanNo").val(),
                    AdhaarCardNo: $("#AadhaarNo").val(),
                    Mobile: mobile,
                    LandLine1: landline,
                    landline2: landline2,
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        if (data.IsDuplicate == true) {
                            app.show_error("Duplicate Customer Exists.");
                            $('.BtnSave').css({ 'visibility': 'visible' });
                        } else {
                            app.confirm_cancel(data.Message, function () {
                                data = self.get_form_data();
                                self.save(data, url, location);

                            }, function () {
                                $('.BtnSave').css({ 'visibility': 'visible' });
                            })
                        }
                    }
                },
                error: function () {
                    $('.BtnSave').css({ 'visibility': 'visible' });
                }
            });

        } else {
            self.save(data, url, location);
        }
    },

    set_default_billing_address: function () {
        var self = Customer;
        var td = $(this).closest("td");
        var IsBillingDefaultIndex = clean($('input[name=IsBillingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var billingindex = self.AddressList[IsBillingDefaultIndex].Index;

        $.each(self.AddressList, function (i, address) {
            address.IsDefault = false;
        });

        self.AddressList[billingindex].IsDefault = true;

        var defaultBillingCount = $('#tbl-address tbody').find('input.IsBillingDefault:checked').length;
        $('#billing-address-count').val(defaultBillingCount);
    },

    set_default_shipping_address: function () {
        var self = Customer;
        var td = $(this).closest("td");
        var IsShippingDefaultIndex = clean($('input[name=IsShippingDefault]:checked').closest('td').find('input[type=hidden]').val());
        var shippingindex = self.AddressList[IsShippingDefaultIndex].Index;

        $.each(self.AddressList, function (i, address) {
            address.IsDefaultShipping = false;
        });

        self.AddressList[shippingindex].IsDefaultShipping = true;
        var defaultShippingCount = $('#tbl-address tbody').find('input.IsShippingDefault:checked').length;
        $('#shipping-address-count').val(defaultShippingCount);
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
                if (state.attr('id') == "State") {
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                } else {
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
                }
            }
        });
    },

    IsGSTChanged: function () {
        if ($(".IsGSTRegistered").prop('checked') == true) {
            $('#GstNo').addClass('visible');
            $(".Gst-number").show();
        } else {
            $(".Gst-number").hide();
            $('#GstNo').removeClass('visible');
        }
    },

    AddAddress: function () {
        var self = Customer;
        var Address;
        self.error_count = 0;
        var FormType = "Address";
        self.error_count = self.validate_form(FormType);
        if (self.error_count > 0) {
            return;
        }
        Address = Customer.GetAddress();
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
            Index: Customer.AddressList.length
        }

        return Data
    },

    PopulateAddress: function (Address) {
        var self = Customer;
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
        html = "<tr><td>" + billing + "<br>" + shipping + "</td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsBillingDefault default' " + icheckBilling + " data-md-icheck/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td class='uk-text-center'><input type='radio' class='uk-radio IsShippingDefault default' " + icheckShipping + " data-md-icheck/>"
            + "<input type='hidden' class='index' value='" + Address.Index + "'></td>"
            + "<td>" + Address.ContactPerson + "</td>"
            + "<td>" + Address.AddressLine1 + "<br>" + Address.AddressLine2 + "<br>" + Address.AddressLine3 + "<br>" + Address.Place + "<br>" + Address.District + "," + Address.State + "</td>"
            + "<td>" + Address.LandLine1 + "<br>" + Address.LandLine2 + "<br>FileNo :" + Address.Fax + "<br>Email :" + Address.Email + "</td>"
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
        $('#FormIndex').val('')
        $('#IsDefault').val('');
        $('#IsDefaultShipping').val('');
    },

    EditAddress: function () {
        var self = Customer;
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

    AddressEditPage: function (Address) {
        var self = Customer;
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
            + "<td>" + Address.LandLine1 + "<br>" + Address.LandLine2 + "<br>FileNo :" + Address.Fax + "<br>Email :" + Address.Email + "</td>"
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

    GetAddressList: function () {
        var self = Customer;
        var CustomerID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Customer/GetAddressList',
            data: { CustomerID },
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

    GetCustomerLocationMappingList: function () {
        var self = Customer;
        var CustomerID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/Customer/GetCustomerLocationMapping',
            data: { CustomerID },
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

    AddLocationMappingList: function () {
        var self = Customer;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Customer-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Customer-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },

    save: function (data, url, location) {
        var self = Customer;
        $.ajax({
            url: url,
            data: { model: data },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Customer created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.BtnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    delete_confirm: function () {
        var self = Customer;
        app.confirm_cancel("Do you want to Delete", function () {
            self.Delete();
        }, function () {
        })
    },

    Delete: function () {
        var self = Customer;
        var ID = $("#ID").val();
        $.ajax({
            url: '/Masters/Customer/DeleteCustomer',
            data: { ID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Customer Delete successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Customer/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to delete data.");
                }
            },
        });
    },

    validate_form: function (FormType) {
        var self = Customer;
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

    get_form_data: function () {
        var self = Customer;
        var model = {
            ID: clean($("#ID").val()),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Name2: $("#Name2").val(),
            CurrencyID: $("#CurrencyID option:selected").val(),
            AadhaarNo: $("#AadhaarNo").val(),
            SupplierID: 0,
            PanNo: $("#PanNo").val(),
            EmailID: $("#EmailID").val(),
            FaxNo: $("#FaxNo").val(),
            ContactPersonName: $("#ContactPersonName").val(),
            StartDate: $("#StartDate").val(),
            ExpiryDate: $("#ExpiryDate").val(),
            CategoryID: clean($("#CategoryID").val()),
            CustomerAccountsCategoryID: clean($("#CustomerAccountsCategoryID").val()),
            PaymentTypeID: clean($("#PaymentTypeID").val()),
            CustomerTaxCategoryID: clean($("#CustomerTaxCategoryID").val()),
            PriceListID: clean($("#PriceListID").val()),
            DiscountID: clean($("#DiscountID").val()),
            CashDiscountID: clean($("#CashDiscountID").val()),
            CreditDays: clean($("#CreditDays").val()),
            MinCreditLimit: clean($("#MinCreditLimit").val()),
            MaxCreditLimit: clean($("#MaxCreditLimit").val()),
            Currency: $("#CurrencyID option:selected").text(),
            OldCode: $("#OldCode").val(),
            FSOID: clean($("#FSOID").val()),
            CustomerMonthlyTarget: clean($("#CustomerMonthlyTarget").val()),
            TradeLegalName: $("#TradeLegalName").val()
        }
        if ($("#IsCustomer").prop('checked') == true) {
            model.isCustomer = 1;
        } else {
            model.isCustomer = 0;
        }
        if ($(".IsGSTRegistered").prop('checked') == true) {
            model.IsGSTRegistered = true;
            model.GstNo = $("#GstNo").val();
        } else {
            model.IsGSTRegistered = false;
        }
        if ($(".IsAlsoASupplier").prop('checked') == true) {
            model.IsAlsoASupplier = true;
            model.SupplierID = $("#SupplierID").val();
        } else {
            model.IsAlsoASupplier = false;
            model.SupplierID = 0
        }
        $('.IsInterCompany').prop("checked") == true ? model.IsInterCompany = true : model.IsInterCompany = false;
        $('.IsMappedtoExpsEntries').prop("checked") == true ? model.IsMappedtoExpsEntries = true : model.IsMappedtoExpsEntries = false;
        $('.IsBlockedForSalesOrders').prop("checked") == true ? model.IsBlockedForSalesOrders = true : model.IsBlockedForSalesOrders = false;
        $('.IsBlockedForSalesInvoices').prop("checked") == true ? model.IsBlockedForSalesInvoices = true : model.IsBlockedForSalesInvoices = false;
        $('.IsMappedToServiceSales').prop("checked") == true ? model.IsMappedToServiceSales = true : model.IsMappedToServiceSales = false;
        model.AddressList = self.AddressList;
        model.CustomerLocationList = self.LocationList
        return model;
    },

    error_count: 0,
    rules: {

        on_submit: [
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Customer Name is Required" },
                ]
            },
            {
                elements: "#CategoryID",
                rules: [
                    { type: form.required, message: "Category is Required" },
                ]
            },
            {
                elements: "#CustomerAccountsCategoryID",
                rules: [
                    { type: form.required, message: "Select Accounts Category" },
                ]
            },
            {
                elements: "#PaymentTypeID",
                rules: [
                    { type: form.required, message: "Select Payment Type" },
                ]
            },
            {
                elements: "#PriceListID",
                rules: [
                    { type: form.required, message: "Select PriceList" },
                ]
            },
            {
                elements: "#DiscountID",
                rules: [
                    { type: form.required, message: "Select Discount Percentage" },
                ]
            },
            {
                elements: "#CreditDaysID",
                rules: [
                    { type: form.required, message: "Select Credit Days" },
                ]
            },
            {
                elements: "#GstNo.visible",
                rules: [
                    { type: form.required, message: "GST Number is Required" },
                ]
            },
            {
                elements: "#MinCreditLimit",
                rules: [
                    { type: form.required, message: "Minimum Credit Limit is Required" },
                    { type: form.non_zero, message: "Minimum Credit Limit is Required" },
                ]
            },
            {
                elements: "#CurrencyID",
                rules: [
                    { type: form.required, message: "Select a Currency" },
                ]
            },
            {
                elements: "#MaxCreditLimit",
                rules: [
                    { type: form.required, message: "Maximum Credit Limit is Required" },
                    { type: form.non_zero, message: "Maximum Credit Limit is Required" },
                ]
            },
            {
                elements: "#Customer-Location-Mapping",
                rules: [
                    {
                        type: function (element) {
                            var CustomerLocation = clean($(element).val());
                            return CustomerLocation > 0 ? true : false;
                        }, message: "Location is Required"
                    },
                ]
            },
            {
                elements: "#FSOID",
                rules: [
                    { type: form.required, message: "Please choose an FSO", alt_element: "#FSOName" },
                    { type: form.positive, message: "Please choose an FSO", alt_element: "#FSOName" },
                    { type: form.non_zero, message: "Please choose an FSO", alt_element: "#FSOName" }
                ],
            },
            {
                elements: "#SupplierID",
                rules: [
                    {
                        type: function (element) {
                            var count = 0
                            var isCustomer = $('.IsAlsoASupplier').prop('checked');
                            if (isCustomer == true) {
                                if (clean($("#SupplierID").val()) <= 0) {
                                    count++;
                                }
                            }
                            return count == 0;
                        }, message: "Supplier Name Is Required "
                    },
                ]
            },
            {
                elements: "#ExpiryDate",
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
    },

    service_customer_list: function (type) {
        if (type == "sales-order") {
            url = "/Masters/Customer/GetServiceSalesOrderCustomerList";
        } else if (type == "sales-invoice") {
            url = "/Masters/Customer/GetServiceSalesInvoiceCustomerList";
        } else {
            url = "/Masters/Customer/GetServiceCustomerList"
        }
        var $list = $('#customer-list');
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
                            { Key: "CustomerCategoryID", Value: $('#CustomerCategoryID').val() },
                            { Key: "StateID", Value: $('#CustomerStateID').val() },

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
                            return "<input type='radio' class='uk-radio CustomerID' name='CustomerID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='StateID' value='" + row.StateID + "'>"
                                + "<input type='hidden' class='PriceListID' value='" + row.PriceListID + "'>"
                                + "<input type='hidden' class='CountryID' value='" + row.CountryID + "'>"
                                + "<input type='hidden' class='DistrictID' value='" + row.DistrictID + "'>"
                                + "<input type='hidden' class='CustomerCategoryID' value='" + row.CustomerCategoryID + "'>"
                                + "<input type='hidden' class='SchemeID' value='" + row.SchemeID + "'>"
                                + "<input type='hidden' class='MinimumCreditLimit' value='" + row.MinimumCreditLimit + "'>"
                                + "<input type='hidden' class='MaxCreditLimit' value='" + row.MaxCreditLimit + "'>"
                                + "<input type='hidden' class='CashDiscountPercentage' value='" + row.CashDiscountPercentage + "'>"
                                + "<input type='hidden' class='IsGSTRegistered' value='" + row.IsGSTRegistered + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Address", "className": "Address" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#CustomerCategoryID,#CustomerStateID', function () {
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
    },
}
