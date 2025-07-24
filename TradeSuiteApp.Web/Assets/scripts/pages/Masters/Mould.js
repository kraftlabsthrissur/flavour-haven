Mould = {
    init: function () {
        var self = Mould;
        item_list = Item.stockable_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 2
        });
        self.bind_events();
    },
    list: function () {
        var $list = $('#mould-list');
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

    bind_events: function () {
        var self = Mould;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnOKItem").on("click", self.select_item);
        $("body").on("click", ".remove-item", self.delete_item);
        $("body").on('click', '.btnSave,.btnSaveAndNew', self.save_confirm);
        $("body").on("click", ".btnAddItem", self.add_item);
        $("body").on('click', '#mould-list tbody tr', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Masters/Mould/Details/' + Id;
        });
    },

    save_confirm: function () {
        var self = Mould;
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
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

    select_item: function () {
        var self = Mould;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var item = {
            id: radio.val(),
            name: $(row).find(".Name").text().trim(),
            code: $(row).find(".Code").text().trim(),
            stock: $(row).find(".Stock").val(),
            primaryUnit: $(row).find(".PrimaryUnit").val(),
            primaryUnitId: $(row).find(".PrimaryUnitID").val(),
            inventoryUnit: $(row).find(".InventoryUnit").val(),
            inventoryUnitId: $(row).find(".InventoryUnitID").val(),
            category: $(row).find(".ItemCategory").val()
        };
        $("#ItemID").val(item.id);
        $("#ItemName").val(item.name)
        UIkit.modal($('#select-item')).hide();
    },

    set_item_details: function (event, item) {
        $("#ItemID").val(item.id);
    },

    add_item: function () {
        var self = Mould;
        if (self.validate_item() > 0) {
            return;
        }
        var item = $("#ItemName").val();
        var itemId = $("#ItemID").val();
        var NoOfCavity = $("#NoOfCavity").val();
        var StdTime = $("#StdTime").val();
        var StdWeight = $("#StdWeight").val();
        var StdRunningWaste = $("#StdRunningWaste").val();
        var StdShootingWaste = $("#StdShootingWaste").val();
        var StdGrindingWaste = $("#StdGrindingWaste").val();
        var content = "";
        var $content;
        var sino = "";
        sino = $('#tbl-Item tbody tr').length + 1;
        content = '<tr>'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class="ItemName">' + item
            + '<input type="hidden" class = "ItemID" value="' + itemId + '" />'
            + '</td>'
            + '<td><input type="text" class = "md-input mask-postive NoOfCavity" value="' + NoOfCavity + '" /></td>'
            + '<td><input type="text" class = "md-input mask-postive StdTime" value="' + StdTime + '" /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty StdWeight" value="' + StdWeight + '" /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty StdRunningWaste" value="' + StdRunningWaste + '" /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty StdShootingWaste" value="' + StdShootingWaste + '" /></td>'
            + '<td><input type="text" class = "md-input mask-production-qty StdGrindingWaste" value="' + StdGrindingWaste + '" /></td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#tbl-Item tbody').append($content);
        index = $("#tbl-Item tbody tr").length;
        $("#item-count").val(index);
        self.clear_data();
    },

    clear_data: function () {
        var self = Mould;
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#NoOfCavity").val('');
        $("#StdTime").val('');
        $("#StdWeight").val('');
        $("#StdRunningWaste").val('');
        $("#StdShootingWaste").val('');
        $("#StdGrindingWaste").val('');
    },

    delete_item: function () {
        var self = Mould;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#tbl-Item tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#tbl-Item tbody tr").length);
    },

    validate_item: function () {
        var self = Mould;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    save: function () {
        var self = Mould;
        self.error_count = 0;
        var machinecount = $('.machines:checked').length;
        $("#machine-count").val(machinecount);
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var location = "/Masters/Mould/Index";
        if ($(this).hasClass('btnSaveAndNew')) {
            location = "/Masters/Mould/Create";
        }
        var data = self.get_data();
        $.ajax({
            url: '/Masters/Mould/Save',
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
        var self = Mould;
        var data = {};
        data.ID = $("#ID").val();
        data.Code = $("#Code").val();
        data.InceptionDate = $("#InceptionDate").val();
        data.MouldName = $("#MouldName").val();
        data.ExpairyDate = $("#ExpairyDate").val();
        data.MandatoryMaintenanceTime = $("#MandatoryMaintenanceTime").val();
        data.ManufacturedBy = $("#ManufacturedBy").val();
        data.CurrentLocationID = $("#CurrentLocationID").val();
        data.Items = [];
        var Item = {};
        $('#tbl-Item tbody tr ').each(function () {
            Item = {};
            Item.ItemID = $(this).find(".ItemID").val();
            Item.NoOfCavity = $(this).find(".NoOfCavity").val();
            Item.StdTime = $(this).find(".StdTime").val();
            Item.StdWeight = $(this).find(".StdWeight").val();
            Item.StdRunningWaste = $(this).find(".StdRunningWaste").val();
            Item.StdShootingWaste = $(this).find(".StdRunningWaste").val();
            Item.StdGrindingWaste = $(this).find(".StdRunningWaste").val();
            data.Items.push(Item);
        });
        data.Machines = [];
        var Machine = {};
        $.each($(".machines:checked"), function () {
            Machine = {};
            Machine.ID = $(this).val();
            data.Machines.push(Machine);
        });
        return data;
    },

    validate_form: function () {
        var self = Mould;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_add: [
           {
               elements: "#ItemID",
               rules: [
                   { type: form.required, message: "Please choose an item", alt_element: "#ItemName" },
                   { type: form.positive, message: "Please choose an item", alt_element: "#ItemName" },
                   { type: form.non_zero, message: "Please choose an item", alt_element: "#ItemName" },
                  {
                      type: function (element) {
                          var item_id = $(element).val();
                          var error = false;
                          $('#tbl-Item tbody tr').each(function () {
                              if ($(this).find('.ItemID').val() == item_id) {
                                  error = true;
                              }
                          });
                          return !error;
                      }, message: "Item already added in the grid, try editing"
                  }
               ],
           },
           {
               elements: "#NoOfCavity",
               rules: [
                   { type: form.required, message: "Please enter NoOfCavities"},
                   { type: form.positive, message: "Please enter NoOfCavities"},
                   { type: form.non_zero, message: "Please enter NoOfCavities"}
               ]
           },
           {
               elements: "#StdTime",
               rules: [
                   { type: form.required, message: "Please enter StdTime For Single Shot" },
                   { type: form.positive, message: "Please enter StdTime For Single Shot" },
                   { type: form.non_zero, message: "Please enter StdTime For Single Shot" }
               ]
           },
           {
               elements: "#StdWeight",
               rules: [
                   { type: form.required, message: "Please enter StdWeight For Single Shot" },
                   { type: form.positive, message: "Please enter StdWeight For Single Shot" },
                   { type: form.non_zero, message: "Please enter StdWeight For Single Shot" }
               ],
           },
        ],
        on_save: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please enter Code" },
                ],
            },
             {
                 elements: "#InceptionDate",
                 rules: [
                     { type: form.required, message: "Please enter InceptionDate" },
                 ],
             },
             {
                 elements: "#MandatoryMaintenanceTime",
                 rules: [
                     { type: form.required, message: "Please select MandatoryMaintenanceTime" },
                     { type: form.positive, message: "Please select MandatoryMaintenanceTime" },
                     { type: form.non_zero, message: "Please select MandatoryMaintenanceTime" }
                 ],
             },
            {
                elements: "#MouldName",
                rules: [
                    { type: form.required, message: "Please enter MouldName" },
                ],
            },
            {
                elements: "#ExpairyDate",
                rules: [
                    { type: form.required, message: "Please enter ExpairyDate" },
                ],
            },
            {
                elements: "#DurationBetweenMaintenance",
                rules: [
                    { type: form.required, message: "Please enter DurationBetweenMaintenance" },
                ],
            },
            {
                 elements: "#machine-count",
                 rules: [
                     { type: form.required, message: "Please select Machines" },
                     { type: form.non_zero, message: "Please select Machines" }
                 ],
            },
            {
                elements: "#CurrentLocationID",
                rules: [
                    { type: form.required, message: "Please select CurrentLocation" },
                    { type: form.positive, message: "Please select CurrentLocation" },
                    { type: form.non_zero, message: "Please select CurrentLocation" }
                ],
            },
            {
                elements: '#tbl-Item tbody .NoOfCavity',
                rules: [
                    { type: form.required, message: "Please enter a valid Cavity" },
                    { type: form.non_zero, message: "Please enter a valid Cavity" },
                    { type: form.positive, message: "Please enter a valid Cavity" },
                ]
            },
            {
                elements: '#tbl-Item tbody .StdTime',
                rules: [
                    { type: form.required, message: "Please enter a valid StdTime" },
                    { type: form.non_zero, message: "Please enter a valid StdTime" },
                    { type: form.positive, message: "Please enter a valid StdTime" },
                ]
            },
             {
                 elements: '#tbl-Item tbody .StdWeight',
                 rules: [
                     { type: form.required, message: "Please enter a valid StdWeight" },
                     { type: form.non_zero, message: "Please enter a valid StdWeight" },
                     { type: form.positive, message: "Please enter a valid StdWeight" },
                 ]
             }
              
        ]
    },

}