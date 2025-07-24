
Discount = {
    init: function () {
        var self = Discount;
        Customer.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        item_list = Item.SaleableServiceAndStock_items_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            selectionType: "radio"
        });
        Discount.bind_events();
    },

    list: function () {
        var $list = $('#discount-list');
        //$list.on('click', 'tbody tr', function () {
        //    var Id = $(this).find("td:eq(0) .ID").val();
        //    window.location = '/Masters/Discount/Details/' + Id;
        //});
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Discount/GetDiscountList";
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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "CustomerName", "className": "CustomerName" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    { "data": "State", "className": "State" },
                    { "data": "BusinessCategory", "className": "BusinessCategory" },
                    { "data": "SalesIncentiveCategory", "className": "SalesIncentiveCategory" },
                    { "data": "SalesCategory", "className": "SalesCategory" },
                    { "data": "DiscountCategory", "className": "DiscountCategory" },
                    {
                        "data": "DiscountPercentage", "className": "DiscountPercentage", "searchable": false, "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DiscountPercentage + "</div>";
                        }
                    }
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

    select_customer: function () {
        var self = Discount;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var StateID = $(row).find(".StateID").val();
        var CustomerCategoryID = $(row).find(".CustomerCategoryID").val();
        $("#CustomerName").val(Name);
        $("#CustomerID").val(ID);
        $("#CustomerStateID").val(StateID);
        $("#CustomerCategoryID").val(CustomerCategoryID);
        UIkit.modal($('#select-customer')).hide();
    },
    select_item: function () {
        var self = Discount;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var salesCategoryID = $(row).find(".salesCategoryID").val();
        var salesIncentiveCategoryID = $(row).find(".salesIncentiveCategoryID").val();
        var businessCategoryID = $(row).find(".businessCategoryID").val();
        $("#ItemID").val(ID);
        $("#SalesCategoryID").val(salesCategoryID);
        $("#SalesIncentiveCategoryID").val(salesIncentiveCategoryID);
        $("#BusinessCategoryID").val(businessCategoryID);
        $("#ItemName").val(Name);
        $('#txtRqQty').focus();
        UIkit.modal($('#select-item')).hide();
    },
    bind_events: function () {
        var self = Discount;
        //Bind auto complete event for customer 
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        //Bind auto complete event for Item
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("#btnOKCustomer").on("click", self.select_customer);

        $("body").on("click", "#btnfilter", self.filter_items);
        $("body").on("click", ".btnSave", self.save);
        $("body").on("ifChanged", ".check-box", self.check);
        $("body").on("change", ".DiscountCategoryID", self.set_discount);
        $("#BusinessCategoryID,#SalesCategoryID,#SalesIncentiveCategoryID").on("change", self.clear_item);
        $("#CustomerCategoryID,#CustomerStateID").on("change", self.clear_customer);
    },

    clear_item: function () {
        var self = Discount;
        $("#ItemID").val('');
        $("#ItemName").val('');
    },
    clear_customer: function () {
        var self = Discount;
        $("#CustomerID").val('');
        $("#CustomerName").val('');
    },
    check: function () {
        var self = Discount;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
            $(row).find(".DiscountCategoryID").prop("disabled", false);
        } else {
            $(row).find(".DiscountCategoryID").prop("disabled", true);
            $(row).removeClass('included');
        }
        self.count_items();
    },
    set_discount: function () {
        var self = Discount;
        var row = $(this).closest('tr');
        var data = $(row).find(".DiscountCategoryID option:selected").data('value');
        $(row).find(".DiscountPercentage").val(data);
    },
    count_items: function () {
        var count = $('#tbl_Discount tbody tr').find('input.check-box:checked').length;
        $('#item-count').val(count);
    },
    get_customers: function (release) {
        var self = Discount;
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
                CustomerCategoryID: $('#CustomerCategoryID').val(),
                StateID: $('#CustomerStateID').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customer: function (event, item) {
        var self = Discount;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#CustomerStateID").val(item.stateId);
        $("#CustomerCategoryID").val(item.customercategoryId);
    },
    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetItemsListForSaleableServiceAndStockItemForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                SalesCategoryID: $("#SalesCategoryID").val(),
                SalesIncentiveCategoryID: $("#SalesIncentiveCategoryID").val(),
                BusinessCategoryID: $("#BusinessCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, items) {   // on select auto complete item
        var self = Discount;
        $("#ItemID").val(items.id);
        $("#ItemName").val(items.Name);
        var salesCategoryid = items.salescategoryid;
        var businessCategoryid = items.businesscategoryid;
        var salesIncentiveCategoryid = items.salesincentivecategoryid;
        $("#SalesCategoryID").val(salesCategoryid);
        $("#SalesIncentiveCategoryID").val(salesIncentiveCategoryid);
        $("#BusinessCategoryID").val(businessCategoryid);
    },
    clear_data: function () {
        var self = Discount;
        $("#ItemID").val('');
        $("#CustomerID").val('');
        $("#CustomerCategoryID").val('');
        $("#CustomerStateID").val('');
        $("#BusinessCategoryID").val('');
        $("#SalesIncentiveCategoryID").val('');
        $("#SalesCategoryID").val('');
        $("#ItemName").val('');
        $("#CustomerName").val('');
    },

    filter_items: function () {
        var self = Discount;
        self.error_count = 0;
        self.error_count = self.validate_filter();
        if (self.error_count > 0) {
            self.clear_data();
            return;
        }
        self.get_filter_item();
    },

    get_filter_item: function () {
        var length;
        var self = Discount;
        $.ajax({
            url: '/Masters/Discount/GetDiscountDetails/',
            dataType: "json",
            data: {
                'ItemID': $("#ItemID").val(),
                'CustomerID': $("#CustomerID").val(),
                'CustomerCategoryID': $("#CustomerCategoryID").val(),
                'CustomerStateID': $("#CustomerStateID").val(),
                'BusinessCategoryID': $("#BusinessCategoryID").val(),
                'SalesIncentiveCategoryID': $("#SalesIncentiveCategoryID").val(),
                'SalesCategoryID': $("#SalesCategoryID Option:selected").val(),
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                var DiscountPercentageList = $("#DiscountPercentageList").html();
                if (length != 0) {

                    var c = $('#tbl_Discount tbody tr').length;
                    $(response.Data).each(function (i, item) {
                        var slno = (c + (i + 1));
                        content += '<tr>'
                            + '<td class="sl-no">' + slno + '</td>'
                            + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                            + '<td class="itemName">' + item.ItemName
                            + '<input type="hidden" class="ItemID" value="' + item.ItemID + '"/>'
                            + '<input type="hidden" class="CustomerID" value="' + item.CustomerID + '"/>'
                            + '<input type="hidden" class="CustomerCategoryID" value="' + item.CustomerCategoryID + '"/>'
                            + '<input type="hidden" class="CustomerStateID" value="' + item.CustomerStateID + '"/>'
                            + '<input type="hidden" class="SalesCategoryID" value="' + item.SalesCategoryID + '"/>'
                            + '<input type="hidden" class="SalesIncentiveCategoryID" value="' + item.SalesIncentiveCategoryID + '"/>'
                            + '<input type="hidden" class="BusinessCategoryID" value="' + item.BusinessCategoryID + '"/>'
                            + '<input type="hidden" class="DiscountCategory" value="' + item.DiscountCategoryID + '"/>'
                            + '<input type="hidden" class="ID" value="' + item.ID + '"/>'
                            + '</td>'
                            + '<td>' + item.CustomerName + '</td>'
                            + '<td>' + item.CustomerCategoryName + '</td>'
                            + '<td>' + item.CustomerStateName + '</td>'
                            + '<td>' + item.BusinessCategoryName + '</td>'
                            + '<td>' + item.SalesIncentiveCategoryName + '</td>'
                            + '<td>' + item.SalesCategoryName + '</td>'
                            + '<td class="DiscountPercentageList" ></td>'
                            + '<td>' + '<input type="text" value=" ' + item.DiscountPercentage + '" class="md-input uk-text-right DiscountPercentage mask-qty" disabled /> ' + '</td>'
                            + '</tr>';
                        $content = $(content);
                        $content.find(".DiscountPercentageList").html("<select class='md-input DiscountCategoryID' disabled>" + DiscountPercentageList + "</select>");
                        app.format($content);
                        //self.clear_data();
                    });
                    $("#tbl_Discount tbody").append($content);
                    $("#tbl_Discount tbody .DiscountCategoryID").each(function () { $(this).val($(this).closest('tr').find('.DiscountCategory').val()) });
                }
                else {
                    // app.show_error('Selected datas dont have discount');
                    var i = $('#tbl_Discount tbody tr').length;
                    var ItemID = $("#ItemID").val();
                    var CustomerID = $("#CustomerID").val();
                    var CustomerCategoryID = $("#CustomerCategoryID").val();
                    var CustomerStateID = $("#CustomerStateID").val();
                    var BusinessCategoryID = $("#BusinessCategoryID").val();
                    var SalesIncentiveCategoryID = $("#SalesIncentiveCategoryID").val();
                    var SalesCategoryID = $("#SalesCategoryID Option:selected").val();
                    var ItemName = $("#ItemName").val();
                    var CustomerName = $("#CustomerName").val();
                    var CustomerCategory = $("#CustomerCategoryID :selected ").text();
                    if (CustomerCategory == "Select") {
                        CustomerCategory = "";
                    } else {
                        CustomerCategory = $("#CustomerCategoryID :selected ").text();
                    }
                    var CustomerState = $("#CustomerStateID :selected ").text();
                    if (CustomerState == "Select") {
                        CustomerState = "";
                    } else {
                        CustomerState = $("#CustomerStateID :selected ").text();
                    }
                    var BusinessCategory = $("#BusinessCategoryID :selected ").text();
                    if (BusinessCategory == "Select") {
                        BusinessCategory = "";
                    } else {
                        BusinessCategory = $("#BusinessCategoryID :selected ").text();
                    }
                    var SalesIncentiveCategory = $("#SalesIncentiveCategoryID :selected ").text();
                    if (SalesIncentiveCategory == "Select") {
                        SalesIncentiveCategory = "";
                    } else {
                        SalesIncentiveCategory = $("#SalesIncentiveCategoryID :selected ").text();
                    }
                    var SalesCategory = $("#SalesCategoryID :selected ").text();
                    if (SalesCategory == "Select") {
                        SalesCategory = "";
                    } else {
                        SalesCategory = $("#SalesCategoryID :selected ").text();
                    }
                    var slno = (i + 1);
                    var ID = 0;
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="itemname">' + ItemName
                        + '<input type="hidden" class="ItemID" value="' + ItemID + '"/>'
                        + '<input type="hidden" class="CustomerID" value="' + CustomerID + '"/>'
                        + '<input type="hidden" class="CustomerCategoryID" value="' + CustomerCategoryID + '"/>'
                        + '<input type="hidden" class="CustomerStateID" value="' + CustomerStateID + '"/>'
                        + '<input type="hidden" class="SalesCategoryID" value="' + SalesCategoryID + '"/>'
                        + '<input type="hidden" class="SalesIncentiveCategoryID" value="' + SalesIncentiveCategoryID + '"/>'
                        + '<input type="hidden" class="BusinessCategoryID" value="' + BusinessCategoryID + '"/>'
                        + '<input type="hidden" class="ID" value="' + ID + '"/>'
                        + '</td>'
                        + '<td>' + CustomerName + '</td>'
                        + '<td>' + CustomerCategory + '</td>'
                        + '<td>' + CustomerState + '</td>'
                        + '<td>' + BusinessCategory + '</td>'
                        + '<td>' + SalesIncentiveCategory + '</td>'
                        + '<td>' + SalesCategory + '</td>'
                        + '<td class="DiscountPercentageList" ></td>'
                        + '<td>' + '<input type="text"  class="md-input uk-text-right DiscountPercentage mask-qty" disabled /> ' + '</td>'
                        + '</tr>';
                    $content = $(content);
                    $content.find(".DiscountPercentageList").html("<select class='md-input DiscountCategoryID' disabled>" + DiscountPercentageList + "</select>");
                    app.format($content);
                    $("#tbl_Discount tbody").append($content);
                }
                self.clear_data();
            },
        });
    },

    save: function () {
        var self = Discount;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var location = "/Masters/Discount/Index";
        var data = self.get_data();
        $(".btnSave").css({ 'display': 'none' });
        $.ajax({
            url: '/Masters/Discount/Save/',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                    $(".btnSave").css({ 'display': 'block' });
                }
            }
        });
    },
    get_data: function () {
        var self = Discount;
        var data = {};
        data.DiscountDetails = [];
        var item = {};
        $('#tbl_Discount tbody .included').each(function () {
            item = {};
            item.ItemID = $(this).find(".ItemID").val();
            item.CustomerID = $(this).find(".CustomerID").val();
            item.CustomerCategoryID = $(this).find(".CustomerCategoryID").val();
            item.CustomerStateID = $(this).find(".CustomerStateID").val();
            item.SalesCategoryID = $(this).find(".SalesCategoryID").val();
            item.SalesIncentiveCategoryID = $(this).find(".SalesIncentiveCategoryID").val();
            item.BusinessCategoryID = $(this).find(".BusinessCategoryID").val();
            item.ID = $(this).find(".ID").val();
            item.DiscountCategoryID = $(this).find(".DiscountCategoryID").val();
            item.DiscountPercentage = $(this).find(".DiscountPercentage").val();
            data.DiscountDetails.push(item);
        });
        return data;
    },

    validate_form: function () {
        var self = Discount;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_filter: function () {
        var self = Discount;
        if (self.rules.on_filter.length > 0) {
            return form.validate(self.rules.on_filter);
        }
        return 0;
    },
    rules: {
        on_filter: [
             {
                 elements: "#ItemID",
                 rules: [
                     {
                         type: function (element) {
                             var ItemID = clean($("#ItemID").val());
                             var CustomerID = clean($("#CustomerID").val());
                             var CustomerCategoryID = clean($("#CustomerCategoryID").val());
                             var CustomerStateID = clean($("#CustomerStateID").val());
                             var BusinessCategoryID = clean($("#BusinessCategoryID").val());
                             var SalesIncentiveCategoryID = clean($("#SalesIncentiveCategoryID").val());
                             var SalesCategoryID = clean($("#SalesCategoryID Option:selected").val());
                             var error = false;

                             if (ItemID <= 0 && CustomerID <= 0 && CustomerCategoryID <= 0 && CustomerStateID <= 0 && BusinessCategoryID <= 0 && SalesIncentiveCategoryID <= 0 && SalesCategoryID <= 0) {
                                 error = true;

                             }

                             return !error;
                         }, message: "select any category"
                     },
                     {
                         type: function (element) {
                             var ItemID = clean($("#ItemID").val());
                             var CustomerID = clean($("#CustomerID").val());
                             var CustomerCategoryID = clean($("#CustomerCategoryID").val());
                             var CustomerStateID = clean($("#CustomerStateID").val());
                             var BusinessCategoryID = clean($("#BusinessCategoryID").val());
                             var SalesIncentiveCategoryID = clean($("#SalesIncentiveCategoryID").val());
                             var SalesCategoryID = clean($("#SalesCategoryID Option:selected").val());
                             var error = false;
                             $('#tbl_Discount tbody tr').each(function () {
                                 if ($(this).find('.ItemID').val() == ItemID && $(this).find('.CustomerID').val() == CustomerID && $(this).find('.CustomerCategoryID').val() == CustomerCategoryID && $(this).find('.CustomerStateID').val() == CustomerStateID && $(this).find('.BusinessCategoryID').val() == BusinessCategoryID && $(this).find('.SalesIncentiveCategoryID').val() == SalesIncentiveCategoryID && $(this).find('.SalesCategoryID').val() == SalesCategoryID) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "already added in the grid, try editing discount category"
                     },
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
           }
        ]
    }
}