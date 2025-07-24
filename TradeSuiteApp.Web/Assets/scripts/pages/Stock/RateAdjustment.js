var fh_items;
rateadjustment = {
    init: function () {
        var self = rateadjustment;

        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        self.freeze_headers();
    },

    details: function () {
        var self = rateadjustment;
        self.freeze_headers();
    },

    freeze_headers: function () {
        fh_items = $("#rate-adjustment-items-list").FreezeHeader();
    },


    list: function () {
        var self = rateadjustment;
        $('#tabs-RateAdjustment').on('change.uk.tab', function (event, active_item, previous_item) {
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
            case "rate-adjustment":
                $list = $('#rate-adjustment-list');
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

            var url = "/Stock/RateAdjustment/GetRateAdjustmentListForDataTable?type=" + type;
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

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Stock/RateAdjustment/Details/" + Id);

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
        var self = rateadjustment;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", "#btnFilter", self.filter_items);
        $(".btnSave, .btnSaveNew, .btnSavedraft").on("click", self.on_save);
        $("body").on("ifChanged", ".check-box", self.check);
        $("#ItemCategoryID").on("change", self.update_item_list);
        $('body').on('keyup', '.ActualAvgCost', self.calculate_value);
        $("#DDLItemCategory").on("change", self.update_item_list);

    },

    calculate_value: function () {

        $currtr = $(this).closest('tr');
        var SystemStockQty = clean($currtr.find('.SystemStockQty').val());
        var SystemAvgCost = clean($currtr.find('.SystemAvgCost').val());
        var ActualAvgCost = clean($currtr.find('.ActualAvgCost').val());
        var differencecost = SystemAvgCost - ActualAvgCost;
        var systemstockvalue = SystemStockQty * SystemAvgCost;
        var actualstockvalue = SystemStockQty * ActualAvgCost;
        var differencestockvalue = systemstockvalue - actualstockvalue;

        $currtr.find('.DifferenceInAvgCost').val(differencecost);
        $currtr.find('.SystemStockValue').val(systemstockvalue);
        $currtr.find('.ActualStockValue ').val(actualstockvalue);
        $currtr.find('.DifferenceInStockValue').val(differencestockvalue);

    },
    update_item_list: function () {
        item_list.fnDraw();
    },

    //set auto complete
    set_item_details: function (event, item) {
        $("#ItemID").val(item.id);
    },
    //get auto complete
    get_items: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },

            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    select_item: function () {
        var self = rateadjustment;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        UIkit.modal($('#select-item')).hide();
    },

    validate_filter: function () {
        var self = rateadjustment;
        if (self.rules.on_filter.length) {
            return form.validate(self.rules.on_filter);
        }
        return 0;
    },
    validate_submit: function () {
        var self = rateadjustment;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_draft: function () {
        var self = rateadjustment;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    rules: {
        on_filter: [
                    {
                        elements: "#ItemID",
                        rules: [
                            { type: form.required, message: "Please choose an item" },
                         { type: form.non_zero, message: "Please choose an item" },

                        ],
                    }
        ],

        on_draft: [
         {
             elements: "#item-count",
             rules: [
                 { type: form.non_zero, message: "Please choose atleast one item" },
                 { type: form.required, message: "Please choose atleast one item" },
             ]
         },

        ],
        on_submit: [
         {
             elements: ".included .effect-date",
             rules: [
               { type: form.required, message: "Please enter effect Date" },
               { type: form.past_date, message: "Invalid effect date" },
             ],
         },
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please choose atleast one item" },
                     { type: form.required, message: "Please choose atleast one item" },
                 ]
             },

        ],
    },

    filter_items: function () {
        var self = rateadjustment;
        var error_count = self.validate_filter();
        if (error_count > 0) {
            return;
        }
        if ($('#rate-adjustment-items-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                self.get_filter_item();
            });
        }
        else {
            self.get_filter_item();
        }
        self.count_items();
        fh_items.resizeHeader();
    },
    get_filter_item: function () {
        var length = 0;
        $.ajax({
            url: '/Stock/rateadjustment/GetRateAdjustmentItems/',
            dataType: "json",
            data: {
                'ItemCategoryID': $("#ItemCategoryID").val(),
                'ItemID': $("#ItemID").val(),

            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;

                $(response.Data).each(function (i, item) {

                    var slno = (i + 1);
                    var SystemStockValue = item.SystemStockQty * item.SystemAvgStock;
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="itemname">' + item.ItemName
                        + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                        + '</td>'
                        + '<td class="Category">' + item.Category + '</td>'
                        //+ '<td>'
                        //+ '<div class="uk-input-group">'
                        //+ '<input type="text" class="md-input effect-date past-date date"  disabled/>'
                        //+ '<span class="uk-input-group-addon">'
                        //+ '<i class="uk-input-group-icon uk-icon-calendar past-date"></i>'
                        //+ '</span></div>'
                        //+ '</td>'
                        + '<td class="SystemStockQty mask-production-qty">' + item.SystemStockQty + '</td>'
                        + '<td class="SystemAvgCost mask-production-qty">' + item.SystemAvgCost + '</td>'
                         + '<td  >' + '<input type="text" value="0.00" class="md-input uk-text-right ActualAvgCost mask-production-qty" disabled /> ' + '</td>'
                         + '<td class="DifferenceInAvgCost mask-production-qty">' + item.DifferenceInAvgStock + '</td>'
                        + '<td class="SystemStockValue mask-production-qty">' + SystemStockValue + '</td>'
                        + '<td class="ActualStockValue mask-production-qty">0.00</td>'
                        + '<td class="DifferenceInStockValue mask-production-qty">0.00</td>'
                        + '<td  >' + '<input type="text" class="md-input uk-text-right Remark "  /> ' + '</td>'

                          + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#rate-adjustment-items-list tbody").html($content);
                if (length == 0) {
                    app.show_error('Selected item dont have batch ');

                }
            },
        });

    },
    check: function () {
        var self = rateadjustment;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
            $(row).find(".ActualAvgCost").prop("disabled", false);
            $(row).find(".effect-date").prop("disabled", false);

        } else {
            $(row).find(".ActualAvgCost").prop("disabled", true);
            $(row).find(".effect-date").prop("disabled", true);

            $(row).removeClass('included');
        }
        self.count_items();
    },
    count_items: function () {
        var count = $('#rate-adjustment-items-list tbody').find('input.check-box:checked').length;
        $('#item-count').val(count);
    },
    get_data: function () {
        var self = rateadjustment;
        var data = {};
        data.TransNo = $("#TransNo").val();
        data.Date = $("#txtDate").val();

        data.ID = $("#ID").val();
        data.Items = [];
        var item = {};
        $("#rate-adjustment-items-list tbody tr.included").each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val()),
            item.SystemStockQty = clean($(this).find(".SystemStockQty").val()),
            item.SystemAvgCost = clean($(this).find(".SystemAvgCost").val()),
            item.ActualAvgCost = clean($(this).find(".ActualAvgCost").val()),
            item.DifferenceInAvgCost = clean($(this).find(".DifferenceInAvgCost").val()),
            item.SystemStockValue = clean($(this).find(".SystemStockValue").val()),
            item.ActualStockValue = clean($(this).find(".ActualStockValue").val()),
            item.DifferenceInStockValue = clean($(this).find(".DifferenceInStockValue").val()),
            item.EffectDate = $(this).find(".effect-date").val(),
            item.Remark = $(this).find(".Remark").val()
            data.Items.push(item);
        });

        return data;
    },

    on_save: function () {

        var self = rateadjustment;
        var data = self.get_data();
        var location = "/Stock/RateAdjustment/Index";
        var url = '/Stock/RateAdjustment/Save';

        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
            url = '/Stock/RateAdjustment/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_submit();
            if ($(this).hasClass("btnSaveNew")) {
                location = "/Stock/RateAdjustment/Create";
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
        var self = rateadjustment;
        $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'none' });

        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Rate Adjustment Saved Successfully");
                    window.location = location;
                }
                else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'block' });
                }
            }
        });


    }
}