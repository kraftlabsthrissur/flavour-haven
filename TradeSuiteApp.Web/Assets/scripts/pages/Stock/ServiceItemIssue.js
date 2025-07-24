var fh_items;
ServiceItemIssue = {
    init: function () {
        var self = ServiceItemIssue;
        item_list = Item.saleable_service_items_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        self.IssueType = "";
        self.get_serial_no();
        self.freeze_headers();
        $("#item-count").val($("#stock-issue-items-list  tbody tr").length);
    },

    list: function () {
        var self = ServiceItemIssue;
        $('#tabs-stock-issue').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = ServiceItemIssue;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#stock-issue-draft-list');
                break;
            case "to-be-received":
                $list = $('#stock-issue-to-be-received-list');
                break;
            case "fully-received":
                $list = $('#stock-issue-fully-received-list');
                break;
            case "cancelled":
                $list = $('#stock-issue-cancelled-list');
                break;
            default:
                $list = $('#stock-issue-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Stock/ServiceItemIssue/GetServiceItemIssueList?type=" + type;

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
                    { "data": "IssueLocation", "className": "IssueLocation" },
                    { "data": "IssuePremise", "className": "IssuePremise" },
                    { "data": "ReceiptLocation", "className": "ReceiptLocation" },
                    { "data": "ReceiptPremise", "className": "ReceiptPremise" },
                    {
                        "data": "Amount", "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        },
                    },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/ServiceItemIssue/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = ServiceItemIssue;
        self.freeze_headers();
        $("body").on("click", ".printpdf", self.printpdf);
    },

    printpdf: function () {
        var self = ServiceItemIssue;
        $.ajax({
            url: '/Reports/Stock/ServiceIssuePrintPdf',
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

    freeze_headers: function () {
        fh_items = $("#stock-issue-items-list").FreezeHeader();
    },

    bind_events: function () {
        var self = ServiceItemIssue;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("body").on('click', "#btnAddProduct", self.add_item);
        $("body").on("keyup change", "#stock-issue-items-list tbody tr .IssueQty", self.update_item);
        $(".btnSave, .btnSaveAndNew, .btnSaveAsDraft").on("click", self.on_save);
        $("body").on("click", ".remove-item", self.delete_item);
        $("body").on("change", "#ReceiptLocationID", self.on_change_receipt_location);
    },

    select_item: function () {
        var self = ServiceItemIssue;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");

        var item = {};
        item.value = $(row).find(".Name").text().trim();
        item.id = radio.val();
        item.code = $(row).find(".Code").text().trim();
        item.unit = $(row).find(".SalesUnit").val();
        item.unitid = $(row).find(".SalesUnitID").val();
        item.gstPercentage = $(row).find(".gstPercentage").val();
        item.mrp = clean($(row).find(".MRP").val());
        item.cessPercentage = $(row).find(".CessPercentage").val();
        item.primaryUnit = $(row).find(".PrimaryUnit").val();
        item.primaryUnitId = $(row).find(".PrimaryUnitID").val();
        item.inventoryUnit = $(row).find(".InventoryUnit").val();
        item.inventoryUnitId = $(row).find(".InventoryUnitID").val();
        self.on_select_item(item);
        UIkit.modal($('#select-item')).hide();
    },

    update_item: function () {
        var self = ServiceItemIssue;
        var row = $(this).closest("tr");
        var item = {};
        item.GSTAmount = 0;
        item.IssueQty = clean($(this).val());
        item.Rate = clean($(row).find(".Rate").val());
        item.TradeDiscountPercentage = clean($(row).find(".TradeDiscountPercentage").val());
        item.SGSTPercentage = clean($(row).find(".SGSTPercentage").val());
        item.CGSTPercentage = clean($(row).find(".CGSTPercentage").val());
        item.BasicPrice = clean($(row).find(".BasicPrice").val());
        item.IGSTPercentage = clean($(row).find(".IGSTPercentage").val());

        var IssueStateID = $("#LocationStateID").val()
        var ReceiptStateID = $("#ReceiptLocationID option:selected").data("state-id");
        if (IssueLocationID == ReceiptLocationID) {
            item.Rate = 0;
        }
        
        item.GrossAmount = item.BasicPrice * item.IssueQty;
        item.TradeDiscount = item.GrossAmount * item.TradeDiscountPercentage / 100;
        item.TaxableAmount = item.GrossAmount -item.TradeDiscount;
        
        if (self.is_igst()) {
            item.IGSTAmount = item.TaxableAmount * item.IGSTPercentage / 100;
            item.GSTAmount = item.IGSTAmount;
        }
        item.GSTPercentage = item.CGSTPercentage + item.SGSTPercentage + IGSTAmount;
        item.NetAmount = item.TaxableAmount + item.GSTAmount;

        $(row).find(".GrossAmount").val(item.GrossAmount);
        $(row).find(".TradeDiscount").val(item.TradeDiscount);
        $(row).find(".TaxableAmount").val(item.TaxableAmount);
        $(row).find(".CGSTAmount").val(item.CGSTAmount);
        $(row).find(".SGSTAmount").val(item.SGSTAmount);
        $(row).find(".IGSTAmount").val(item.IGSTAmount);
        $(row).find(".GSTAmount").val(item.GSTAmount);
        $(row).find(".NetAmount").val(item.NetAmount);

        self.calculate_grid_total();

    },

    on_select_item: function (item) {
        var self = ServiceItemIssue;
        $("#Code").val(item.code);
        $("#ItemName").val(item.value);
        $("#ItemID").val(item.id);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CessPercentage").val(item.cessPercentage);
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#InventoryUnit").val(item.inventoryUnit);
        $("#InventoryUnitID").val(item.inventoryUnitId);
        self.get_units();
    },

    delete_item: function () {
        var self = ServiceItemIssue;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#stock-issue-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#stock-issue-items-list  tbody tr").length);
        self.calculate_grid_total();
    },

    get_units: function () {
        var self = ServiceItemIssue;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#InventoryUnitID").val() + "'>" + $("#InventoryUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);

    },

    get_item_details: function (release) {

        $.ajax({
            url: '/Masters/Item/GetItemsListForSaleableServiceItemForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item_details: function (event, item) {   // on select auto complete item
        var self = ServiceItemIssue;
        self.on_select_item(item);
    },

    get_serial_no: function () {

        var self = ServiceItemIssue;
        var ReceiptStateID = $("#ReceiptLocationID option:selected").data("state-id");
        var IssueStateID = $("#LocationStateID").val();
        var IssueLocationID = $("#IssueLocationID").val();
        var ReceiptLocationID = $("#ReceiptLocationID").val();

        if (IssueLocationID == ReceiptLocationID) {
            self.IssueType = "IntraLocation";
        } else if (IssueStateID == ReceiptStateID) {
            self.IssueType = "IntraState";
        } else {
            self.IssueType = "InterState";
        }

        $.ajax({
            url: '/Stock/ServiceItemIssue/GetSerialNo/',
            dataType: "json",
            type: "POST",
            data: { IssueType: self.IssueType },
            success: function (response) {
                if (response.Status == "success") {
                    $("#IssueNo").val(response.IssueNo);
                }
            }
        });
    },


    add_item: function () {
        var self = ServiceItemIssue;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var ItemName = $("#ItemName").val();
        var ItemID = $("#ItemID").val();
        var UnitID = $("#UnitID").val();
        var Unit = $("#UnitID option:selected").text();
        var IssueQty = clean($("#txtRequiredQty").val());
        var Rate = clean($("#Rate").val());
        var IGSTPercentage = 0;
        var TradeDiscountPercentage = 0;
        var IGSTPercentage = 0;
        var Stock = 0;
        var IssueStateID = $("#LocationStateID").val()
        var ReceiptStateID = $("#ReceiptLocationID option:selected").data("state-id");
        if (IssueLocationID == ReceiptLocationID) {
             TradeDiscountPercentage = 0;
             Rate = 0;
             IGSTPercentage = 0;
        } else if (IssueStateID == ReceiptStateID) {
            TradeDiscountPercentage = 0;
            IGSTPercentage = 0;
        } else {
            TradeDiscountPercent = $("#TradeDiscountPercent").val();
            IGSTPercentage = clean($("#GSTPercentage").val());
        }
        var BasicPrice = Rate * 100 / (100 + IGSTPercentage);
        var GrossAmount = BasicPrice * IssueQty;
        var TradeDiscount = GrossAmount * TradeDiscountPercentage / 100;
        var TaxableAmount = GrossAmount - TradeDiscount;
        var GSTAmount = 0;
        var IGSTAmount = 0;
        var CGSTAmount = 0;
        var SGSTAmount = 0;
        var CGSTPercentage = clean($("#GSTPercentage").val())/2;
        var SGSTPercentage = clean($("#GSTPercentage").val())/2;
        if (self.is_igst()) {
            GSTAmount = TaxableAmount * IGSTPercentage / 100;
            IGSTAmount = GSTAmount;
            IGSTPercentage = IGSTPercentage;
            SGSTPercentage = 0;
            CGSTPercentage = 0;
        }
        
        var GSTPercentage = CGSTPercentage + SGSTPercentage + IGSTPercentage;
        var NetAmount = TaxableAmount + GSTAmount;
        var content = "";
        var $content;
        var sino = "";
        sino = $('#stock-issue-items-list tbody tr').length + 1;
        content = '<tr>'
            + "<td class='uk-text-center serial-no'>" + sino + "</td>"
                       + "<td class='ItemName' >" + ItemName
                       + "     <input type='hidden' class='ItemID' value='" +ItemID + "'/>"
                       + "     <input type='hidden' class='Stock' value='" + Stock + "' />"
                       + "     <input type='hidden' class='Rate' value='" + Rate + "' />"
                       + "     <input type='hidden' class='CGSTPercentage' value='" + CGSTPercentage + "' />"
                       + "     <input type='hidden' class='SGSTPercentage' value='" + SGSTPercentage + "' />"
                       + "     <input type='hidden' class='IGSTPercentage' value='" + IGSTPercentage + "' />"
                       + "     <input type='hidden' class='CGSTAmount' value='" + CGSTAmount + "' />"
                       + "     <input type='hidden' class='SGSTAmount' value='" + SGSTAmount + "' />"
                       + "     <input type='hidden' class='IGSTAmount' value='" + IGSTAmount + "' />"
                       + "     <input type='hidden' class='UnitID' value='" + UnitID + "' />"
                       + "</td>"
                       + "<td class='Unit'>" +Unit + "</td>"
                       + "<td class='action'>"
                       + "      <input type='text' min='0' class='md-input mask-production-qty IssueQty' value='" + IssueQty + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency BasicPrice' disabled value='" + BasicPrice + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency GrossAmount' disabled value='" + GrossAmount + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency TradeDiscountPercentage' disabled value='" + TradeDiscountPercentage + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency TradeDiscount' disabled value='" + TradeDiscount + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency TaxableAmount' disabled value='" + TaxableAmount + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency GSTPercentage' disabled value='" + GSTPercentage + "' />"
                       + "</td>"
                       + "<td class='inter-branch'>"
                       + "      <input type='text' class='md-input mask-currency GSTAmount' disabled value='" + GSTAmount + "' />"
                       + "</td>"
                       + "<td class='inter-branch '>"
                       + "      <input type='text' class='md-input mask-currency NetAmount' disabled value='" + NetAmount + "' />"
                       + "</td>"
                       + " <td class='uk-text-center action'>"
                       + "     <a class='remove-item'>"
                       + "         <i class='uk-icon-remove'></i>"
                       + "     </a>"
                       + " </td>"
                   + "</tr>";
        $content = $(content);
        app.format($content);
        $('#stock-issue-items-list tbody').append($content);
        index = $("#stock-issue-items-list tbody tr").length;
        $("#item-count").val(index);
        self.calculate_grid_total();
        self.clear();
    },

    clear: function () {
        var self = ServiceItemIssue;
        $("#ItemID").val(0);
        $("#ItemName").val('');
        $("#UnitID").val(0);
        $("#GSTPercentage").val(0);
        $("#PrimaryUnitID").val(0);
        $("#InventoryUnitID").val(0);
        $("#Rate").val('');
        $("#txtRequiredQty").val('');
    },


    calculate_grid_total: function () {
        var self = ServiceItemIssue;
        var GrossAmount = 0;
        var TradeDiscount = 0;
        var TaxableAmount = 0;
        var SGSTAmount = 0;
        var CGSTAmount = 0;
        var IGSTAmount = 0;
        var NetAmount = 0;
        var RoundOff = 0;
        var temp = 0;
        $("#stock-issue-items-list tbody tr").each(function () {
            GrossAmount += clean($(this).find('.GrossAmount').val());
            TradeDiscount += clean($(this).find('.TradeDiscount').val());
            TaxableAmount += clean($(this).find('.TaxableAmount').val());
            SGSTAmount += clean($(this).find('.SGSTAmount').val());
            CGSTAmount += clean($(this).find('.CGSTAmount').val());
            IGSTAmount += clean($(this).find('.IGSTAmount').val());
            NetAmount += clean($(this).find('.NetAmount').val());
        });
        temp = NetAmount;
        NetAmount = Math.round(NetAmount);
        RoundOff = NetAmount - temp;
        $("#GrossAmount").val(GrossAmount);
        $("#TradeDiscount").val(TradeDiscount);
        $("#TaxableAmount").val(TaxableAmount);
        $("#RoundOff").val(RoundOff);
        $("#NetAmount").val(NetAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#IGSTAmount").val(IGSTAmount);
        $("#SGSTAmount").val(SGSTAmount);
        $("#CGSTAmount").val(CGSTAmount);
    },

    is_igst: function () {
        return $("#LocationStateID").val() != $("#ReceiptLocationID option:selected").data("state-id");
    },

    get_data: function (IsDraft) {
        var self = ServiceItemIssue;
        var model = {
            IssueNo: $("#IssueNo").val(),
            Date: $("#Date").val(),
            IssueLocationID: $("#IssueLocationID").val(),
            IssuePremiseID: $("#IssuePremiseID").val(),
            ReceiptLocationID: $("#ReceiptLocationID").val(),
            ReceiptPremiseID: $("#ReceiptPremiseID").val(),
            GrossAmount: clean($("#GrossAmount").val()),
            TradeDiscount: clean($("#TradeDiscount").val()),
            TaxableAmount: clean($("#TaxableAmount").val()),
            SGSTAmount: clean($("#SGSTAmount").val()),
            CGSTAmount: clean($("#CGSTAmount").val()),
            IGSTAmount: clean($("#IGSTAmount").val()),
            RoundOff: clean($("#RoundOff").val()),
            NetAmount: clean($("#NetAmount").val()),
            ID: $("#ID").val(),
            IssueType: self.IssueType,
            IsDraft: IsDraft,
            IsService: 1,
            RequestNo:'',
        };
        model.Items = self.get_item_list();
        return model;
    },

    get_item_list: function () {
        var Items = [];
        $("#stock-issue-items-list tbody tr").each(function (i) {
            Items.push(
                {
                    ItemID: $(this).find('.ItemID').val(),
                    Name: $(this).find('.ItemName').text(),
                    IssueQty: clean($(this).find('.IssueQty').val()),
                    IssueDate: $("#Date").val(),
                    Rate: clean($(this).find('.Rate').val()),
                    BasicPrice: clean($(this).find('.BasicPrice').val()),
                    GrossAmount: clean($(this).find('.GrossAmount').val()),
                    TradeDiscountPercentage: clean($(this).find('.TradeDiscountPercentage').val()),
                    TradeDiscount: clean($(this).find('.TradeDiscount').val()),
                    TaxableAmount: clean($(this).find('.TaxableAmount').val()),
                    SGSTPercentage: clean($(this).find('.SGSTPercentage').val()),
                    CGSTPercentage: clean($(this).find('.CGSTPercentage').val()),
                    IGSTPercentage: clean($(this).find('.IGSTPercentage').val()),
                    SGSTAmount: clean($(this).find('.SGSTAmount').val()),
                    CGSTAmount: clean($(this).find('.CGSTAmount').val()),
                    IGSTAmount: clean($(this).find('.IGSTAmount').val()),
                    NetAmount: clean($(this).find('.NetAmount').val()),
                    UnitID: clean($(this).find('.UnitID').val()),
                });
        });

        return Items;
    },

    on_save: function () {

        var self = ServiceItemIssue;
        var data = self.get_data();
        var location = "/Stock/ServiceItemIssue/Index";
        var url = '/Stock/ServiceItemIssue/Save';

        if ($(this).hasClass("btnSaveAsDraft")) {
            data.IsDraft = true;
            url = '/Stock/ServiceItemIssue/SaveAsDraft'
            self.error_count = self.validate_form_for_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveAndNew")) {
                location = "/Stock/ServiceItemIssue/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!data.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(data, url, location);
            }, function () {
            })
        } else {
            self.save(data, url, location);
        }
    },

    save: function (data, url, location) {
        var self = ServiceItemIssue;
      
        $.ajax({
            url: url,
            data: { model: data },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("ServiceItem issue created successfully");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveAndNew, .btnSave, .btnSaveAsDraft").css({ 'display': 'block' });
                }
            },
        });
    },

    get_receipt_premises: function () {
        var self = ServiceItemIssue;
        var location_id = $("#ReceiptLocationID").val();
        if (location_id == null || location_id == "") {
            return;
        }
        $.ajax({
            url: '/Masters/Warehouse/GetWareHousesByLocation/',
            dataType: "json",
            type: "POST",
            data: { LocationID: location_id },
            success: function (response) {
                $("#ReceiptPremiseID").html("");
                if (response.Status == "success") {
                    var html = "<option value >Select</option>";
                    $.each(response.data, function (i, record) {
                        html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                    });
                    $("#ReceiptPremiseID").append(html);
                }
            }
        });
    },

    on_change_receipt_location: function () {
        var self = ServiceItemIssue;
        if ($("#stock-issue-items-list tbody tr").length > 0) {
            app.confirm_cancel("Selected Items will be removed", function () {
                self.clear_grid();
                self.ReceiptLocationID = $("#ReceiptLocationID").val();
                self.get_receipt_premises();
                self.get_serial_no();
                setTimeout(function () { fh_items.resizeHeader(); }, 200);
            }, function () {
                $("#ReceiptLocationID").val(self.ReceiptLocationID);
            });
        }
        else {
            self.ReceiptLocationID = $("#ReceiptLocationID").val();
            self.get_receipt_premises();
            self.get_serial_no();
            setTimeout(function () { fh_items.resizeHeader(); }, 200);
        }
    },

    clear_grid: function () {
        var self = ServiceItemIssue;
        $('#stock-issue-items-list tbody').empty();
        self.count_items();
        $("#GrossAmount").val(0);
        $("#TradeDiscount").val(0);
        $("#TaxableAmount").val(0);
        $("#SGSTAmount").val(0);
        $("#CGSTAmount").val(0);
        $("#IGSTAmount").val(0);
        $("#RoundOff").val(0);
        $("#NetAmount").val(0);
    },

    count_items: function () {
        var count = $('#stock-issue-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    validate_form: function () {
        var self = ServiceItemIssue;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_form_for_draft: function () {
        var self = ServiceItemIssue;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_item: function () {
        var self = ServiceItemIssue;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

   
    rules: {

        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please choose a valid item" },
                    { type: form.non_zero, message: "Please choose a valid item" },
                    {
                        type: function (element) {
                            var item_id = $(element).val();
                            var error = false;
                            $('#stock-issue-items-list tbody tr').each(function () {
                                if ($(this).find('.ItemID').val() == item_id) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Item already added in the grid, try editing issue quantity"
                    }
                ]
            },
            {
                elements: "#txtRequiredQty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                ]
            },
            {
                elements: "#Rate",
                rules: [
                    { type: form.required, message: "Please enter Rate" },
                    { type: form.numeric, message: "Numeric Rate required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter a valid rate" },
                ]
            },
        ],

        on_submit: [
            {
                elements: ".IssueQty",
                rules: [
                    { type: form.required, message: "Please enter valid IssueQty" },
                    { type: form.numeric, message: "Please enter valid IssueQty" },
                    { type: form.positive, message: "Please enter valid IssueQty" },
                    { type: form.non_zero, message: "Please enter valid IssueQty" },
                ]
            },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#IssuePremiseID",
                rules: [
                   {
                       type: function (element) {
                           var FromPremises = $("#IssuePremiseID").val();
                           var ToPremises = $("#ReceiptPremiseID").val();
                           var ReceiptLocationID = $("#ReceiptLocationID").val();
                           var IssueLocationID = $("#IssueLocationID").val();
                           var error = false;
                           if (FromPremises == ToPremises && ReceiptLocationID == IssueLocationID && (FromPremises != '' || ToPremises != ''))
                           {
                               error = true;
                           }
                           return !error;
                       },
                       message: "Stock can not be transfered between same premise",
                       alt_element: "#ReceiptPremiseID"
                   },
                   {
                       type: function (element) {
                           var FromPremises = $("#IssuePremiseID").val();
                           var ToPremises = $("#ReceiptPremiseID").val();
                           var error = false;
                           if (FromPremises != ToPremises  && FromPremises == '' ) {
                               error = true;
                           }
                           return !error;
                       },
                       message: "Please Select IssuePremise",
                       alt_element: "#IssuePremiseID"
                   },

                   {
                       type: function (element) {
                           var FromPremises = $("#IssuePremiseID").val();
                           var ToPremises = $("#ReceiptPremiseID").val();
                           var error = false;
                           if (FromPremises != ToPremises && ToPremises == '') {
                               error = true;
                           }
                           return !error;
                       },
                       message: "Please Select ReceiptPremise",
                       alt_element: "#ReceiptPremiseID"
                   },
                  
                ]
            },
            {
                elements: "#ReceiptLocationID",
                rules: [
                    { type: form.required, message: "Please select Receipt Location" },
                    {
                        type: function (element) {
                            var FromPremises = $("#IssuePremiseID").val();
                            var ToPremises = $("#ReceiptPremiseID").val();
                            var ReceiptLocationID = $("#ReceiptLocationID").val();
                            var IssueLocationID = $("#IssueLocationID").val();
                            var error = false;
                            if (FromPremises == ToPremises && ReceiptLocationID == IssueLocationID) {
                                error = true;
                            }
                            return !error;
                        },
                        message: "Stock can not be transfered between same location",
                        alt_element: "#ReceiptLocationID"
                    },
                ]
            },
            {
                elements: "#IssueLocationID",
                rules: [
                    { type: form.required, message: "Please select Issue Location" },
                ]
            },
            {
                elements: '#stock-issue-items-list .IssueQty',
                rules: [
                    { type: form.required, message: "Please enter a valid quantity" },
                    { type: form.non_zero, message: "Please enter a valid quantity" },
                    { type: form.positive, message: "Please enter a valid quantity" },
                ]
            }
        ],

        on_draft: [
         
          {
              elements: "#item-count",
              rules: [
                  { type: form.non_zero, message: "Please add atleast one item" },
                  { type: form.required, message: "Please add atleast one item" },
              ]
          },
        
          {
              elements: "#ReceiptLocationID",
              rules: [
                  { type: form.required, message: "Please select Receipt Location" },
              ]
          },
          {
              elements: "#IssueLocationID",
              rules: [
                  { type: form.required, message: "Please select Issue Location" },
              ]
          },
          {
              elements: '#stock-issue-items-list .IssueQty',
              rules: [
                  { type: form.required, message: "Please enter a valid quantity" },
                  { type: form.non_zero, message: "Please enter a valid quantity" },
                  { type: form.positive, message: "Please enter a valid quantity" },
              ]
          }
        ],

    }

   
}