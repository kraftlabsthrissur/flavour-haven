
var fh_items;
DamageEntry = {
    init: function () {
        var self = DamageEntry;
        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            startFocusIndex: 3,
            initiatingElement: "#ItemName"
        });

        self.bind_events();
        self.freeze_headers();
    },

    freeze_headers: function () {
        fh_items = $("#damage-entry-items-list").FreezeHeader();
    },

    list: function () {
        $list = $('#damage-entry-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Stock/DamageEntry/Details/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = DamageEntry;
        self.freeze_headers();
    },

    bind_events: function () {
        var self = DamageEntry;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", "#btnFilter", self.filter_items);
        $("#WarehouseID").on("change", self.clear);
        $(".btnSavedraft,.btnSaveNew").on("click", self.save);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on("ifChanged", ".check-box", self.check);
        $("#DDLItemCategory").on("change", self.update_item_list);

        self.Load_All_DropDown();
    },

    save_confirm: function () {
        var self = DamageEntry
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    Load_All_DropDown: function () {
        $.ajax({
            url: '/Stock/DamageEntry/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    DamageTypeList = response.data;
                }
            }
        });
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

    //set auto complete
    set_item_details: function (event, item) {
        $("#ItemID").val(item.id);
    },

    //item select in modal
    select_item: function () {
        var self = DamageEntry;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        UIkit.modal($('#select-item')).hide();
    },

    filter_items: function () {
        var self = DamageEntry;
        var error_count = self.validate_filter();
        if (error_count > 0) {
            return;
        }
        if ($('#damage-entry-items-list tbody tr').length > 0) {
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

    check: function () {
        var self = DamageEntry;
        var row = $(this).closest('tr');
        if ($(this).prop("checked") == true) {
            $(row).addClass('included');
            $(row).find(".damageqty").prop("disabled", false);
        } else {
            $(row).find(".damageqty").prop("disabled", true);
            $(row).removeClass('included');
        }
        self.count_items();
    },

    count_items: function () {
        var count = $('#damage-entry-items-list tbody').find('input.check-box:checked').length;
        $('#item-count').val(count);
    },

    validate_filter: function () {
        var self = DamageEntry;
        if (self.rules.on_filter.length) {
            return form.validate(self.rules.on_filter);
        }
        return 0;
    },

    validate_submit: function () {
        var self = DamageEntry;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_filter: [
            {
                elements: "#WarehouseID",
                rules: [
                    { type: form.required, message: "Please choose a store" },
                 { type: form.non_zero, message: "Please choose a store" },

                ],
            },
             {
                 elements: "#ItemID",
                 rules: [
                     { type: form.required, message: "Please choose an item" },
                  { type: form.non_zero, message: "Please choose an item" },

                 ],
             }
        ],
        on_submit: [
         {
             elements: ".included .damageqty",
             rules: [
                { type: form.positive, message: "Positive damage qty required" },
                { type: form.non_zero, message: "Damage quantity should not be zero" },
                {
                    type: function (element) {
                        var row = $(element).closest('tr');
                        var damageqty = clean($(row).find('.damageqty').val());
                        var currentqty = clean($(row).find('.currentqty').val());
                        return damageqty <= currentqty;

                    }, message: 'Damage quantity should not be greater than current qty '
                },
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

    get_filter_item: function () {
        var length;
        var DamageType;
        $.ajax({
            url: '/Stock/DamageEntry/GetDamageEntryItems/',
            dataType: "json",
            data: {
                'ItemCategoryID': $("#DDLItemCategory").val(),
                'ItemID': $("#ItemID").val(),
                'WarehouseID': $("#WarehouseID").val()
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {


                    var slno = (i + 1);
                    DamageType = '<select class="md-input label-fixed>' + DamageEntry.Build_Select(DamageTypeList, item.DamageTypeID) + '</select>'
                    content += '<tr>'
                        + '<td class="sl-no">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box"/>' + '</td>'
                        + '<td class="itemname">' + item.ItemName
                        + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                        + '<input type="hidden" class="UnitID"value="' + item.UnitID + '"/>'
                        + '<input type="hidden" class="BatchID"value="' + item.BatchID + '"/>'
                        + '<input type="hidden" class="BatchTypeID"value="' + item.BatchTypeID + '"/>'
                        + '<input type="hidden" class="WarehouseID"value="' + item.WarehouseID + '"/>'
                        + '</td>'
                        + '<td class="unit">' + item.UnitName + '</td>'
                        + '<td class="batch">' + item.Batch + '</td>'
                        + '<td class="BatchType">' + item.BatchType + '</td>'
                        + '<td class="ExpiryDate">' + item.ExpiryDateString + '</td>'
                        + '<td class="currentqty mask-qty">' + item.CurrentQty + '</td>'
                        + '<td  >' + '<input type="text" value=" ' + item.DamageQty + '" class="md-input uk-text-right damageqty mask-qty" disabled /> ' + '</td>'
                        + '<td class="DamageTypeID">' + DamageType + '</td>'
                        + '<td><input type="text" class="md-input Remarks" value="' + (item.Remarks == null ? "" : item.Remarks) + '" /></td>'
                        + '</tr>';

                });
                $content = $(content);
                app.format($content);
                $("#damage-entry-items-list tbody").html($content);
                if (length == 0) {
                    app.show_error('Selected item dont have batch with stock');

                }
            },

        });

    },

    Build_Select: function (options, selected) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';
        for (var i = 0; i < options.length; i++) {
            if (options[i].ID == selected) {
                $option = '<option selected value="' + options[i].ID + '">' + options[i].Name + '</option>';
            } else {
                $option = '<option value="' + options[i].ID + '">' + options[i].Name + '</option>';
            }


            $select.append($option);
        }

        return $select.html();

    },

    get_data: function () {
        var self = DamageEntry;
        var data = {};
        data.TransNo = $("#TransNo").val();
        data.Date = $("#txtDate").val();
        data.WarehouseID = clean($("#WarehouseID").val())
        data.ID = $("#ID").val();
        data.Items = [];
        var item = {};
        $("#damage-entry-items-list tbody tr.included").each(function () {
            item = {};
            item.ItemID = clean($(this).find(".ItemID").val()),
            item.WarehouseID = clean($(this).find(".WarehouseID").val()),
            item.UnitID = clean($(this).find(".UnitID").val()),
            item.BatchID = clean($(this).find(".BatchID").val()),
            item.BatchTypeID = clean($(this).find(".BatchTypeID").val()),
            item.CurrentQty = clean($(this).find(".currentqty").val()),
            item.DamageQty = clean($(this).find(".damageqty").val()),
            item.ExpiryDate = $(this).find(".ExpiryDate").text()
            item.DamageTypeID = clean($(this).find(".DamageTypeID :selected").val()),
            item.Remarks = $(this).find(".Remarks").val(),
            data.Items.push(item);
        });

        return data;
    },

    save: function () {
        var self = DamageEntry;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        if ($(this).hasClass("btnSavedraft")) {
            data.IsDraft = true;
        }

        var location = "/Stock/DamageEntry/Index";
        if ($(this).hasClass("btnSaveNew")) {
            location = "/Stock/DamageEntry/Create";
        }
        $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'none' });

        $.ajax({
            url: '/Stock/DamageEntry/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Damage Entry Saved Successfully");
                    window.location = location;
                }
                else {
                    app.show_error('Failed to create damage entry');
                    $(".btnSavedraft, .btnSave, .btnSaveNew").css({ 'display': 'block' });
                }
            }
        });

    }
}