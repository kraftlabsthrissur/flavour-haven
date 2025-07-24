var fh;
var categoryid = [];
SchemeAllocation = {
    init: function () {
        var self = SchemeAllocation;
        Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        item_list = Item.item_list();
        $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#Qty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        //Offer Item is not enabled
        //item_list = Item.item_offer_list();
        //$('#offer-item-list').SelectTable({
        //    selectFunction: self.select_offer_item,
        //    returnFocus: "#Qty",
        //    modal: "#select-offer-item",
        //    initiatingElement: "#OfferItemName",
        //    startFocusIndex: 2
        //});
        // fh = $("#tbl_scheme").FreezeHeader();
        self.bind_events();

    },

    list: function () {
        $scheme_list = $('#Scheme-allocation-list');
        $scheme_list.on('click', 'tbody td', function () {
            var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
            window.location = '/Masters/SchemeAllocation/Details/' + Id;
        });
        if ($scheme_list.length) {
            $scheme_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/SchemeAllocation/GetSchemeAllocationList";
            var list_table = $scheme_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[2, "asc"]],
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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    { "data": "CustomerState", "className": "CustomerState" },
                    { "data": "CustomerDistrict", "className": "CustomerDistrict" },
                    { "data": "StartDate", "className": "StartDate" },
                    { "data": "EndDate", "className": "EndDate" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $scheme_list.trigger("datatable.changed");
                },
            });
            $scheme_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $scheme_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $scheme_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = SchemeAllocation;
        self.details_trans_list();
    },

    details_trans_list: function () {
        $scheme_list = $('#tbl_scheme');

        if ($scheme_list.length) {
            $scheme_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($scheme_list);
            var url = "/Masters/SchemeAllocation/GetSchemeAllocationTransList";
            var list_table = $scheme_list.dataTable({
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
                            { Key: "ID", Value: $('#ID').val() }
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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' >";
                        }
                    },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    { "data": "InvoiceQty", "className": "InvoiceQty" },
                    { "data": "OfferQty", "className": "OfferQty" },
                    { "data": "StartDate", "className": "StartDate" },
                    { "data": "EndDate", "className": "EndDate" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $scheme_list.trigger("datatable.changed");
                },
            });
            $scheme_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $scheme_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $scheme_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    select_customer: function () {
        var self = SchemeAllocation;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var CountryID = $(row).find(".CountryID").val();
        var DistrictID = $(row).find(".DistrictID").val();
        var CustomerCategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);

        UIkit.modal($('#select-customer')).hide();
    },

    select_item: function () {
        var self = SchemeAllocation;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var SalesCategoryID = $(row).find(".SalesCategoryID").val();
        var SalesCategory = $(row).find(".SalesCategory").val();
        $("#ItemID").val(ID);
        $("#ItemName").val(Name);
        $("#SalesCategory").val(SalesCategory);
        $("#ItemSalesCategoryID").val(SalesCategoryID);
        $('#txtRqQty').focus();
        UIkit.modal($('#select-item')).hide();

    },

    select_offer_item: function () {
        var self = SchemeAllocation;
        var radio = $('#offer-item-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#OfferItemID").val(ID);

        $("#OfferItemName").val(Name);

        UIkit.modal($('#select-offer-item')).hide();
    },

    bind_events: function () {
        var self = SchemeAllocation;
        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        //Bind auto complete event for Item
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);

        $.UIkit.autocomplete($('#offer-item-autocomplete'), { 'source': self.get_offer_items, 'minLength': 1 });
        $('#offer-item-autocomplete').on('selectitem.uk.autocomplete', self.set_offer_item_details);
        $("body").on("click", "#btnadd", self.add_item);
        $("body").on("click", "#addCustomer", self.add_customer);
        $("body").on("click", ".btnSave", self.save);
        // $("body").on("change", ".InvoiceQty,.OfferQty", self.check_quantity);
        $("body").on("change", "#CountryID", self.get_State);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("click", ".remove-customer", self.remove_customer);
        $('body').on('ifChanged', '.stateall', self.check_state_all);
        $('body').on('ifChanged', '.districtall', self.check_district_all);
        $('body').on('ifChanged', '.state', self.check_state);
        $('body').on('ifChanged', '.district', self.check_district);
        $('body').on('ifChanged', '.customercategoryall', self.check_category_all);
        $('body').on('ifChanged', '.customercategory', self.check_category);
        $("body").on("change", "#EndDate,#StartDate", self.change_date);
        $("body").on("click", "#item-details", self.show_in_item_tab);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKCustomer").on("click", self.select_customer);

    },

    show_in_item_tab: function () {
        var self = SchemeAllocation;
        $(".liked_txt").remove();
        var categoryname = [];
        categoryname = self.get_customer_category_name();
        var categorynames = categoryname.join();
        $('.CustomerCategory-list').append('<span class="liked_txt">' + categorynames + '</span>');
        var Country = $('#CountryID :selected').text();
        $('.country-name').append('<span class="liked_txt">' + Country + '</span>');
        var statename = [];
        statename = self.get_state_name_list();
        var statenames = statename.join();
        $('.state-list').append('<span class="liked_txt">' + statenames + '</span>');

        if (!$(".district-container").hasClass("uk-hidden")) {
            var districtname = [];
            districtname = self.get_district_name_list();
            var districtnames = districtname.join();
            $('.district-list').append('<span class="liked_txt">' + districtnames + '</span>');
        }
        var customers = [];
        customer = self.get_customers_name_list();
        var customers = customer.join();
        $('.customer-list').append('<span class="liked_txt">' + customers + '</span>');
    },

    get_state_name_list: function () {
        var state = [];
        if ($('.stateall').prop('checked') == true) {
            state.push("ALL");
        }
        else if (($('.stateall').prop('checked') == false) && ($('.state').filter(':checked').length == 0)) {
            state.push("ALL");
        }
        else if ($('.state:checked').length > 0) {
            $.each($('.state:checked'), function () {
                var name = $(this).closest('div').next("label").text();
                state.push(name);
            });
        }
        else {
            state.push("ALL");
        }
        return state;
    },

    get_district_name_list: function () {
        var district_names = [];
        if ($('.districtall').prop('checked') == true) {
            district_names.push("ALL");
        }
        else if (($('.districtall').prop('checked') == false) && ($('.district').filter(':checked').length == 0)) {
            district_names.push("ALL");
        }
        else if ($('.district').filter(':checked').length > 0) {
            $.each($('.district').filter(':checked'), function () {
                district_names.push($(this).closest('div').next("label").text());
            });
        }
        else {
            district_names.push("ALL")
        }
        return district_names;
    },

    get_customer_category_name: function () {
        var customercategory_names = [];
        if ($('.customercategoryall').prop('checked') == true) {
            customercategory_names.push("ALL");
        }
        else if (($('.customercategoryall').prop('checked') == false) && ($('.customercategory').filter(':checked').length == 0)) {
            customercategory_names.push("ALL");
        }
        else {
            $.each($('.customercategory').filter(':checked'), function () {
                customercategory_names.push($(this).closest('div').next("label").text());
            });
        }

        return customercategory_names;
    },

    get_customers_name_list: function () {
        var customer = [];
        $('#tbl_Customer tbody tr').each(function (i, record) {
            var name = $(this).find(".CustomerName").text();
            customer.push(name);
        })
        if (customer.length == 0) {
            customer.push("ALL");
        }
        return customer;
    },

    change_date: function () {
        var startdate = $("#StartDate").val();
        var enddate = $("#EndDate").val();
        var row;
        var isactive;
        $("#tbl_scheme tbody tr").each(function () {
            row = $(this).closest('tr');
            $(row).find('.startDate').val(startdate);
            $(row).find('.endDate').val(enddate);
        });

        $("#ItemStartDate").val(startdate);
        $("#ItemEndDate").val(enddate);
    },

    check_state_all: function (e) {
        $(".districtdiv").html("");
        if ($(this).prop('checked') == true) {
            $(".state").prop("checked", true);
            $(".state").closest('div').addClass("checked");
        }
        else {
            $(".state").prop("checked", false)
            $(".state").closest('div').removeClass("checked")
        }

    },

    check_district_all: function () {
        if ($(this).prop('checked') == true) {
            //$('.district').iCheck('check');
            $(".district").prop("checked", true)
            $(".district").closest('div').addClass("checked")
        }
        else {
            // $('.district').iCheck('uncheck');
            $(".district").prop("checked", false)
            $(".district").closest('div').removeClass("checked")
        }
    },

    check_category_all: function () {
        if ($(this).prop('checked') == true) {
            $(".customercategory").prop("checked", true)
            $(".customercategory").closest('div').addClass("checked")

        }
        else {
            $(".customercategory").prop("checked", false)
            $(".customercategory").closest('div').removeClass("checked")

        }
    },

    check_category: function () {
        var self = SchemeAllocation;
        if ($(this).prop('checked') == false) {
            $(".customercategoryall").prop("checked", false)
            $(".customercategoryall").closest('div').removeClass("checked")
        }
    },

    check_state: function () {
        var self = SchemeAllocation;
        var stateId;
        if ($(this).prop('checked') == false) {
            $(".stateall").prop("checked", false)
            $(".stateall").closest('div').removeClass("checked")
        }

        var statecount = $('.state').filter(':checked').length;
        if (statecount == 1) {
            stateId = $('.state').filter(':checked').val();
            self.get_district(stateId);
        }
        else {
            $(".districtdiv").html("");
        }
    },

    check_district: function () {
        var self = SchemeAllocation;
        if ($(this).prop('checked') == false) {
            $(".districtall").prop("checked", false)
            $(".districtall").closest('div').removeClass("checked")
        }
    },

    clear_customer: function () {
        var self = SchemeAllocation;
        $("#CustomerID").val('');
        $("#CustomerName").val('');

    },

    remove_customer: function () {
        var self = SchemeAllocation;
        $(this).closest("tr").remove();
        self.count_customer();
    },

    count_customer: function () {
        var count = clean($('#tbl_Customer tbody tr').length);
        $('#customer-count').val(count);
    },

    add_customer: function () {
        var self = SchemeAllocation;
        self.error_count = 0;
        self.error_count = self.validate_customer();
        var count = 0;
        if (self.error_count == 0) {
            var SerialNo = $("#tbl_Customer tbody tr").length + 1;
            var html = '  <tr class="">' +
                        ' <td class="uk-text-center">' + SerialNo + '</td>' +
                        ' <td class="CustomerName">' + $("#CustomerName").val()
                        + '<input type="hidden" class="CustomerID" value="' + $("#CustomerID").val() + '" /></td>'
                        + ' <td class="uk-text-center remove-customer" >' +
                        '   <a data-uk-tooltip="{pos:"bottom"}" >' +
                            '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                            ' </a>' +
                        ' </td>' +
                        '</tr>';
            var $html = $(html);
            app.format($html);
            $("#tbl_Customer tbody").append($html);
            self.clear_customer();
            self.count_customer();
            //$(".customer-category-container").addClass('uk-hidden');
            //$(".countryName").addClass('uk-hidden');
            //$(".state-container").addClass('uk-hidden');
        }

    },

    remove_item: function () {
        var self = SchemeAllocation;
        $(this).closest("tr").remove();
        self.count_items();

    },

    get_State: function () {
        var CountryID = clean($("#CountryID").val());
        var length;

        $.ajax({
            url: '/Masters/State/GetStateCountryWise',
            data: {
                CountryID: CountryID

            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $(".statediv").html("");
                $(".districtdiv").html("");
                length = response.data.length;
                if (length > 0) {
                    div = " <div class='uk-width-medium-2-10'>"
                             + "    <input type='checkbox' class='icheckbox stateall' value='" + 0 + "' data-md-icheck />"
                             + "     <label>All</label>"
                             + "</div>";

                    $.each(response.data, function (i, record) {
                        div += " <div class='uk-width-medium-2-10'>"
                            + "    <input type='checkbox' class='icheckbox state' data-country-id='" + CountryID + "' value='" + record.ID + "' data-md-icheck />"
                            + "     <label>" + record.Name + "</label>"
                            + "</div>";

                    });
                    var $div = $(div);
                    app.format($div);
                    $(".statediv").html($div);
                }
            }
        });

    },

    get_district: function (StateID) {
        var length;
        var self = SchemeAllocation;
        $.ajax({
            url: '/Masters/District/GetDistrict',
            data: {
                StateID: StateID
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                length = response.data.length;
                $(".districtdiv").html("");
                if (length > 0) {
                    div = " <div class='uk-width-medium-2-10'>"
                             + "    <br/>   <input type='checkbox' class='icheckbox districtall' value='" + 0 + "' data-md-icheck />"
                             + "     <label>All</label>"
                             + "</div>";

                    $.each(response.data, function (i, record) {
                        div += " <div class='uk-width-medium-2-10'>"
                            + "   <br/>  <input type='checkbox' class='icheckbox district' data-state-id='" + record.StateID + "' data-country-id ='" + record.CountryID + "' value='" + record.ID + "' data-md-icheck />"
                            + "     <label>" + record.Name + "</label>"
                            + "</div>";

                    });
                    var $div = $(div);
                    app.format($div);
                    $(".districtdiv").html($div);
                }
            }
        });
    },

    check_quantity: function () {
        var self = SchemeAllocation;
        var ItemIDGrid, OfferItemIDGrid, BusinessCategoryIDGrid, SalesCategoryIDGrid, InvoiceQtyGrid, OfferQtyGrid;
        var ItemID, OfferItemID, BusinessCategoryID, SalesCategoryID, InvoiceQty, OfferQty;
        var count = 0, cnn = 0, cnt = 0, ct = 0, c = 0;
        var row = (this).closest('tr');
        ItemID = clean($(row).find('.ItemID').val());
        OfferItemID = clean($(row).find('.ItemID').val());
        BusinessCategoryID = clean($(row).find('.BusinessCategoryID').val());
        SalesCategoryID = clean($(row).find('.SalesCategoryID').val());
        InvoiceQty = clean($(row).find('.InvoiceQty').val());
        OfferQty = clean($(row).find('.OfferQty').val());
       
        $("#HiddenOfferQty").val(OfferQty);
        $("#HiddenInvoiceQty").val(InvoiceQty);
        $("#HiddenOfferItemID").val(OfferItemID);
        $("#HiddenItemID").val(ItemID);
        
        self.error_count = self.validate_check_quantity();
        if (self.error_count > 0) {
            return;
        }



    },

    add_items_to_grid: function (items) {
        var self = SchemeAllocation;
        var SerialNo = 0;
        var html = "";
        var OfferQty = clean($("#OfferQty").val());
        var Qty = clean($("#InvoiceQty").val());
        var StartDate = $("#ItemStartDate").val();
        var EndDate = $("#ItemEndDate").val();
        $.each(items, function (i, item) {
            SerialNo = $("#tbl_scheme tbody tr").length + 1 + i;
            html += " <tr>"
                        + "<td class='uk-text-center index'>" + SerialNo + "</td>"
                        + "<td>" + item.ItemName
                        + '<input type="hidden" class="ItemID" value="' + item.ItemID + '" />'
                        + '<input type="hidden" class="ID" value="' + 0 + '" />'
                        + '<input type="hidden" class="OfferItemID" value="' + item.ItemID + '" />'
                        + '<input type="hidden" class="BusinessCategoryID" value="' + 0 + '" />'
                        + '<input type="hidden" class="SalesCategoryID" value="' + item.CategoryID + '" />'
                        + "</td>"
                        + ' <td >' + item.Category + '</td>'
                        + ' <td class="uk-text-right " ><input type="text" class="md-input uk-text-right InvoiceQty mask-qty" value="' + Qty + '" /></td>'
                        + ' <td class="uk-text-right " ><input type="text" class="md-input uk-text-right OfferQty mask-qty" value="' + OfferQty + '" /></td>'
                        + "<td >"
                        + "<input type='text' class='md-input startDate'  value=' " + StartDate + "' />"
                        + "</td>"
                        + "<td >"
                        + "<input type='text' class='md-input endDate'  value=' " + EndDate + "' />"
                        + "</td>"
                        + ' <td class="uk-text-center remove-item" >' +
                          '   <a data-uk-tooltip="{pos:"bottom"}" >' +
                              '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                              ' </a>' +
                          ' </td>' +
                        + "</tr>";
        });
        var $html = $(html);
        app.format($html);
        $("#tbl_scheme tbody").append($html);
        self.clear_data();
        $("#ItemName").focus();
        self.count_items();
    },

    add_item: function () {
        var self = SchemeAllocation;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var count = 0;

        var salescategoryID = $("#SalesCategoryID").val();

        var ItemID = clean($("#ItemID").val());

        if (ItemID == 0 && salescategoryID == 0) {
            app.show_error("Please select item or category");
            return;
        }
        if (ItemID == 0) {
            $.each($(categoryid), function (i, record) {
                if ((record.ID == salescategoryID) && (record.Offer == offer) && (record.Qty == Qty))
                    count++;
            });
            if (count > 0) {
                app.show_error("Sales category with same invoice & offer quantity already exist for same time span");
                return;
            }
            categoryid.push({
                ID: salescategoryID,
                Qty: $("#InvoiceQty").val(),
                Offer: $("#OfferQty").val()
            });

            $.ajax({
                url: '/Masters/Item/GetItemsForSchemeItem',
                data: {
                    CategoryID: salescategoryID
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    self.add_items_to_grid(data);
                }
            });
        }
        else {
            var items = [];
            var item = {
                ItemName: $("#ItemName").val(),
                ItemID: $("#ItemID").val(),
                CategoryID: $("#ItemSalesCategoryID").val(),
                Category: $("#SalesCategory").val(),

            }
            items.push(item);
            self.add_items_to_grid(items);
        }
    },
    count_items: function () {
        $("#tbl_scheme tbody tr").each(function (i, record) {
            $(this).find('.index').text(i + 1);
        });

        var count = $('#tbl_scheme tbody tr').length;
        $('#item-count').val(count);
    },
    get_customers: function (release) {
        var self = SchemeAllocation;
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
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
    set_customer: function (event, item) {
        var self = SchemeAllocation;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);

    },
    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetSaleableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
                SalesCategoryID: $("#SalesCategoryID").val(),
                PriceListID:0,
                StoreID: 0,
                CheckStock:false,
                BatchTypeID: 0
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_offer_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetAllItemsForAutoComplete',
            data: {
                Hint: $('#OfferItemName').val(),
                ItemCategoryID: $("#ItemCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, items) {   // on select auto complete item
        var self = SchemeAllocation;
        $("#ItemID").val(items.id);
        $("#ItemName").val(items.value);
        $("#SalesCategory").val(items.salesCategory);
        $("#ItemSalesCategoryID").val(items.salesCategoryid);
    },
    set_offer_item_details: function (event, items) {   // on select auto complete item
        var self = SchemeAllocation;
        $("#OfferItemID").val(items.id);
        $("#OfferItemName").val(items.value);
    },
    clear_data: function () {
        var self = SchemeAllocation;
        $("#ItemID").val('');
        $("#SalesCategoryID").val('');
        $("#ItemName").val('');
        $("#OfferItemID").val('');
        $("#BusinessCategoryID").val('');
        $("#OfferItemName").val('');
        $("#InvoiceQty").val('');
        $("#OfferQty").val('');
           },

    save: function () {
        var self = SchemeAllocation;
        self.error_count = 0;
        $("#preloader").show();
        altair_helpers.content_preloader_show('md', 'success');
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            return;
        }
        var location = "/Masters/SchemeAllocation/Index";
        var Model = self.get_data();
        $.ajax({
            url: '/Masters/SchemeAllocation/Save/',
            data: { Model: Model },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(response.Message);
                    window.location = location;
                } else {
                    app.show_error(response.Message);

                }
            }
        });
    },
    get_data: function () {
        var self = SchemeAllocation;
        var Model = {
            ID: $("#ID").val(),
            Scheme: $("#Scheme").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            Items: self.get_list(),
            Customers: self.get_customers_list(),
            CustomerCategory: self.get_customer_category(),
            Districts: self.get_district_list(),
            States: self.get_state_list()
        };

        return Model;
    },
    get_state_list: function () {
        var state = [];

        $.each($('.state:checked'), function () {
            state.push({
                StateID: clean($(this).val()),
                CountryID: $(this).data('country-id'),
            });
        });

        return state;
    },

    get_district_list: function () {
        var district = [];
        $.each($('.district:checked'), function () {
            district.push({
                DistrictID: clean($(this).val()),
                StateID: $(this).data('state-id'),
                CountryID: $(this).data('country-id'),
            });
        });

        return district;
    },

    get_customer_category: function () {
        var customercategory = [];

        $.each($('.customercategory:checked'), function () {
            customercategory.push({
                CustomerCategoryID: clean($(this).val()),
            });
        });

        return customercategory;
    },
    get_customers_list: function () {
        var customer = [];
        $('#tbl_Customer tbody tr').each(function (i, record) {
            customer.push({
                CustomerID: clean($(this).find(".CustomerID").val()),
            });
        })
        return customer;
    },
    get_list: function () {
        var item = [];
        $('#tbl_scheme tbody tr').each(function (i, record) {
            var buinessCategoryID = clean($(this).find(".BusinessCategoryID").val()) == 0 ? null : clean($(this).find(".BusinessCategoryID").val());
            item.push({
                ID: clean($(this).find(".ID").val()),
                ItemID: clean($(this).find(".ItemID").val()),
                OfferItemID: clean($(this).find(".OfferItemID").val()),
                BusinessCategoryID: buinessCategoryID,
                SalesCategoryID: clean($(this).find(".SalesCategoryID").val()),
                InvoiceQty: clean($(this).find(".InvoiceQty").val()),
                OfferQty: clean($(this).find(".OfferQty").val()),
                StartDate: $(this).find(".startDate").val(),
                EndDate: $(this).find(".endDate").val()
            });
        });
        return item;
    },
    validate_form: function () {
        var self = SchemeAllocation;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    validate_date: function () {
        var self = SchemeAllocation;
        if (self.rules.on_date.length > 0) {
            return form.validate(self.rules.on_date);
        }
        return 0;
    },
    validate_item: function () {
        var self = SchemeAllocation;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_customer: function () {
        var self = SchemeAllocation;
        if (self.rules.on_customer.length > 0) {
            return form.validate(self.rules.on_customer);
        }
        return 0;
    },
    validate_check_quantity: function () {
        var self = SchemeAllocation;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_check_quantity);
        }
        return 0;
    },
    rules: {
        on_check_quantity: [

    {
        elements: ".InvoiceQty",
        rules: [
               { type: form.non_zero, message: "Enter invoice quantity" },
               { type: form.positive, message: "Enter invoice quantity" },
            {
                type: function (element) {
                    var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                    var OfferQty = clean($("#HiddenOfferQty").val());
                    var ItemID = clean($("#HiddenItemID").val());
                    var OfferItemID = clean($("#HiddenItemID").val());

                    var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                    $('#tbl_scheme tbody tr').each(function () {
                        var row = $(this).closest('tr');
                        InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                        OfferQtyGrid = clean($(row).find('.OfferQty').val());
                        ItemIDGrid = clean($(row).find('.ItemID').val());
                        OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                        if ($('#tbl_scheme tbody tr').length > 0) {
                            if ((InvoiceQty == InvoiceQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                count++;
                            }
                        }
                    });
                    return count <= 1
                }, message: "Invoice quantity already exist"
            },

        ]
    },
    {
        elements: ".OfferQty",
        rules: [
               { type: form.non_zero, message: "Enter offer quantity" },
               { type: form.positive, message: "Enter offer quantity" },
            {
                type: function (element) {
                    var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                    var OfferQty = clean($("#HiddenOfferQty").val());
                    var ItemID = clean($("#HiddenItemID").val());
                    var OfferItemID = clean($("#HiddenItemID").val());

                    var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                    $('#tbl_scheme tbody tr').each(function () {
                        var row = $(this).closest('tr');
                        InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                        OfferQtyGrid = clean($(row).find('.OfferQty').val());
                        ItemIDGrid = clean($(row).find('.ItemID').val());
                        OfferItemIDGrid = clean($(row).find('.OfferItemID').val());

                        if ($('#tbl_scheme tbody tr').length > 0) {
                            if ((OfferQty == OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                count++;
                            }
                        }
                    });
                    return count <= 1
                }, message: "Offer quantity already exist"
            },
            {
                type: function (element) {
                    var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                    var OfferQty = clean($("#HiddenOfferQty").val());
                    var ItemID = clean($("#HiddenItemID").val());
                    var OfferItemID = clean($("#HiddenItemID").val());

                    var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                    $('#tbl_scheme tbody tr').each(function () {
                        var row = $(this).closest('tr');
                        InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                        OfferQtyGrid = clean($(row).find('.OfferQty').val());
                        ItemIDGrid = clean($(row).find('.ItemID').val());
                        OfferItemIDGrid = clean($(row).find('.OfferItemID').val());

                        if ((InvoiceQty <= InvoiceQtyGrid) && (OfferQty >= OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                            count++;
                        }

                    });
                    return count <= 1
                }, message: "Please decrease offer quantity"
            },
            {
                type: function (element) {
                    var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                    var OfferQty = clean($("#HiddenOfferQty").val());
                    var ItemID = clean($("#HiddenItemID").val());
                    var OfferItemID = clean($("#HiddenItemID").val());

                    var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                    $('#tbl_scheme tbody tr').each(function () {
                        var row = $(this).closest('tr');
                        InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                        OfferQtyGrid = clean($(row).find('.OfferQty').val());
                        ItemIDGrid = clean($(row).find('.ItemID').val());
                        OfferItemIDGrid = clean($(row).find('.OfferItemID').val());

                        if ((InvoiceQty >= InvoiceQtyGrid) && (OfferQty <= OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                            count++;
                        }

                    });
                    return count <= 1
                }, message: "Please increase offer quantity"
            },
        ]
    },

        ],
        on_add: [
        {
            elements: "#InvoiceQty",
            rules: [
                { type: form.required, message: "Enter item quantity" },
                { type: form.non_zero, message: "Enter item quantity" },
                { type: form.positive, message: "Enter item quantity" },
                {
                    type: function (element) {
                        var ItemID = clean($("#ItemID").val());
                        var SalesCategoryID = clean($("#SalesCategoryID").val());
                        var result = true;
                        if (ItemID != 0) {
                            $('#tbl_scheme tbody tr .ItemID[value="' + ItemID + '"]').each(function () {
                                if (clean($(this).closest("tr").find(".InvoiceQty").val()) == clean($(element).val())) {
                                    result = false;
                                    return;
                                }
                            });
                        } else if (SalesCategoryID != 0) {
                            $('#tbl_scheme tbody tr .SalesCategoryID[value="' + SalesCategoryID + '"]').each(function () {
                                if (clean($(this).closest("tr").find(".InvoiceQty").val()) == clean($(element).val())) {
                                    result = false;
                                    return;
                                }
                            });
                        }
                        return result;
                    }, message: "Item quantity already exists"
                }

            ]
        },
        {
            elements: "#OfferQty",
            rules: [
                { type: form.required, message: "Enter offer quantity" },
                { type: form.non_zero, message: "Enter offer quantity" },
                { type: form.positive, message: "Enter offer quantity" },
                {
                    type: function (element) {
                        var InvoiceQty = clean($("#InvoiceQty").val());
                        var OfferQty = clean($("#OfferQty").val());
                        var ItemID = clean($("#ItemID").val());
                        var OfferItemID = clean($("#ItemID").val());
                        var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                        $('#tbl_scheme tbody tr').each(function () {
                            var row = $(this).closest('tr');
                            InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                            OfferQtyGrid = clean($(row).find('.OfferQty').val());
                            ItemIDGrid = clean($(row).find('.ItemID').val());
                            OfferItemIDGrid = clean($(row).find('.OfferItemID').val());

                            if ($('#tbl_scheme tbody tr').length > 0 && ItemID != 0) {
                                if ((OfferQty == OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                    count = 1;
                                }
                            }
                            0
                        });
                        return count == 0
                    }, message: "Offer quantity already exist"
                },
                {
                    type: function (element) {
                        var InvoiceQty = clean($("#InvoiceQty").val());
                        var OfferQty = clean($("#OfferQty").val());
                        var ItemID = clean($("#ItemID").val());
                        var OfferItemID = clean($("#ItemID").val());
                        var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                        $('#tbl_scheme tbody tr').each(function () {
                            var row = $(this).closest('tr');
                            InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                            OfferQtyGrid = clean($(row).find('.OfferQty').val());
                            ItemIDGrid = clean($(row).find('.ItemID').val());
                            OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                            if ((InvoiceQty < InvoiceQtyGrid) && (OfferQty > OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid) ) {
                                count = 1;
                            }
                        });
                        return count == 0
                    }, message: "Please decrease offer quantity"
                },
                {
                    type: function (element) {
                        var InvoiceQty = clean($("#InvoiceQty").val());
                        var OfferQty = clean($("#OfferQty").val());
                        var ItemID = clean($("#ItemID").val());
                        var OfferItemID = clean($("#ItemID").val());
                        var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                        $('#tbl_scheme tbody tr').each(function () {
                            var row = $(this).closest('tr');
                            InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                            OfferQtyGrid = clean($(row).find('.OfferQty').val());
                            ItemIDGrid = clean($(row).find('.ItemID').val());
                            OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                            if ((InvoiceQty >= InvoiceQtyGrid) && (OfferQty <= OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                count = 1;
                            }
                        });
                        return count == 0
                    }, message: "Please increase offer quantity"
                },
            ]
        },

        ],
        on_customer: [
          {
              elements: "#CustomerID",
              rules: [
                  { type: form.required, message: "Please choose a valid customer" },
                  { type: form.non_zero, message: "Please choose a valid customer" },
              ]
          },

        ],
        on_date: [

           {
               elements: "#Scheme",
               rules: [
                   { type: form.required, message: "Please eter scheme" },

               ],
           },

          {
              elements: "#StartDate",
              rules: [

                { type: form.required, message: "Invalid start date" },
                   {
                       type: function (element) {
                           var startdate = $("#StartDate").val();
                           var enddate = $("#EndDate").val();

                           var date = startdate.split('-');
                           startdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                           var date = enddate.split('-');
                           enddate = new Date(date[2], date[1] - 1, date[0]).getTime();
                           var count = 0;

                           if ((startdate > enddate)) {
                               count = 1;
                           }
                           return count == 0
                       }, message: "Start Date must be less than end date"
                   },
              ]
          },
          {
              elements: "#EndDate",
              rules: [

                { type: form.required, message: "Invalid end date" },
              ]
          },
        ],
        on_save: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ],
            },
            {
                elements: "#Scheme",
                rules: [
                    { type: form.required, message: "Please enter scheme" },
                ],
            },
            {
                elements: "#CountryID",
                rules: [
                    { type: form.required, message: "Please select country" },
                ],
            },
            {
                elements: "#StartDate",
                rules: [

                { type: form.required, message: "Invalid start date" },
                    {
                        type: function (element) {
                            var startdate = $("#StartDate").val();
                            var enddate = $("#EndDate").val();

                            var date = startdate.split('-');
                            startdate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            var date = enddate.split('-');
                            enddate = new Date(date[2], date[1] - 1, date[0]).getTime();
                            var count = 0;

                            if ((startdate > enddate)) {
                                count = 1;
                            }
                            return count == 0
                        }, message: "Start Date must be less than end date"
                    },
                ]
            },
            {
                elements: "#EndDate",
                rules: [
                    { type: form.required, message: "Invalid end date" },
                ]
            },
            {
                elements: ".InvoiceQty",
                rules: [
                    { type: form.non_zero, message: "Enter invoice quantity" },
                    { type: form.positive, message: "Enter invoice quantity" },
                    {
                        type: function (element) {
                            var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                            var OfferQty = clean($("#HiddenOfferQty").val());
                            var ItemID = clean($("#HiddenItemID").val());
                            var OfferItemID = clean($("#HiddenItemID").val());

                            var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                            $('#tbl_scheme tbody tr').each(function () {
                                var row = $(this).closest('tr');
                                InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                                OfferQtyGrid = clean($(row).find('.OfferQty').val());
                                ItemIDGrid = clean($(row).find('.ItemID').val());
                                OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                                if ((InvoiceQty == InvoiceQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                    count++;
                                }
                            });
                            return count <= 1
                        }, message: "Invoice quantity already exist"
                    },

                ]
            },
            {
                elements: ".OfferQty",
                rules: [
                       { type: form.non_zero, message: "Enter offer quantity" },
                       { type: form.positive, message: "Enter offer quantity" },
                    {
                        type: function (element) {
                            var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                            var OfferQty = clean($("#HiddenOfferQty").val());
                            var ItemID = clean($("#HiddenItemID").val());
                            var OfferItemID = clean($("#HiddenItemID").val());

                            var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                            $('#tbl_scheme tbody tr').each(function () {
                                var row = $(this).closest('tr');
                                InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                                OfferQtyGrid = clean($(row).find('.OfferQty').val());
                                ItemIDGrid = clean($(row).find('.ItemID').val());
                                OfferItemIDGrid = clean($(row).find('.OfferItemID').val());

                                if ((OfferQty == OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                    count++;
                                }

                            });
                            return count <= 1
                        }, message: "Offer quantity already exist"
                    },
                    {
                        type: function (element) {
                            var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                            var OfferQty = clean($("#HiddenOfferQty").val());
                            var ItemID = clean($("#HiddenItemID").val());
                            var OfferItemID = clean($("#HiddenItemID").val());

                            var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                            $('#tbl_scheme tbody tr').each(function () {
                                var row = $(this).closest('tr');
                                InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                                OfferQtyGrid = clean($(row).find('.OfferQty').val());
                                ItemIDGrid = clean($(row).find('.ItemID').val());
                                OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                                if ((InvoiceQty <= InvoiceQtyGrid) && (OfferQty >= OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                    count++;
                                }
                            });
                            return count <= 1
                        }, message: "Please decrease offer quantity"
                    },
                    {
                        type: function (element) {
                            var InvoiceQty = clean($("#HiddenInvoiceQty").val());
                            var OfferQty = clean($("#HiddenOfferQty").val());
                            var ItemID = clean($("#HiddenItemID").val());
                            var OfferItemID = clean($("#HiddenItemID").val());

                            var InvoiceQtyGrid, OfferQtyGrid, OfferItemIDGrid, ItemIDGrid, count = 0;
                            $('#tbl_scheme tbody tr').each(function () {
                                var row = $(this).closest('tr');
                                InvoiceQtyGrid = clean($(row).find('.InvoiceQty').val());
                                OfferQtyGrid = clean($(row).find('.OfferQty').val());
                                ItemIDGrid = clean($(row).find('.ItemID').val());
                                OfferItemIDGrid = clean($(row).find('.OfferItemID').val());
                                if ((InvoiceQty >= InvoiceQtyGrid) && (OfferQty <= OfferQtyGrid) && (ItemID == ItemIDGrid) && (OfferItemID == OfferItemIDGrid)) {
                                    count++;
                                }
                            });
                            return count <= 1
                        }, message: "Please increase offer quantity"
                    },
                ]
            },
        ]
    }
}