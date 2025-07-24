MaterialPurification = {

    init: function () {
        var self = MaterialPurification;

        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        item_list = Item.material_purification_item_list();
        item_select_table = $('#material-purification-item-list').SelectTable({
            selectFunction: self.select_purification_item,
            modal: "#select-material-purification-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        self.bind_events();
    },

    details: function () {

    },

    list: function () {
        $list = $('#material-purification-list');
        $list.find('tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Masters/MaterialPurification/Details/' + Id;
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

    bind_events: function () {
        var self = MaterialPurification;
        $.UIkit.autocomplete($('#Item-autocomplete'), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
        $('#Item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_detail_autocomplete);
        $.UIkit.autocomplete($('#Purification-Item-autocomplete'), { 'source': self.get_purification_item_AutoComplete, 'minLength': 1 });
        $('#Purification-Item-autocomplete').on('selectitem.uk.autocomplete', self.set_purification_item_detail_autocomplete);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", "#btnOKpurificationItem", self.select_purification_item);
        $(".btnSave").on("click", self.save_confirm);
        $("#DDLItemCategory").on("change", self.clear_item);
        $("#ProcessID").on("change", self.clear_purification_item);

    },

    clear_item: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#UnitID").val('');
        $("#Unit").val('');
    },

    clear_purification_item:function(){
        $("#PurificationItemName").val('');
        $("#PurificationUnit").val('');
        $("#PurificationUnitID").val('');
    },

    //get auto complete
    get_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    //set auto complete
    set_item_detail_autocomplete :function (event, item) {
        var itemName = item.value;
        var itemID = item.itemid;
        var unitID = item.unitid;
        var unit = item.unit;
        var stock = item.stock;
        var processID = item.processid;
        $("#ItemID").val(itemID);
        $('#Unit').val(unit);
        $("#UnitID").val(unitID);
    },

    get_purification_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Purchase/PurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#PurificationItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_purification_item_detail_autocomplete: function (event, item) {
        var itemName = item.value;
        var itemID = item.itemid;
        var unitID = item.unitid;
        var unit = item.unit;
        var stock = item.stock;
        var processID = item.processid;
        $("#PurificationItemID").val(itemID);
        $('#PurificationUnit').val(unit);
        $("#PurificationUnitID").val(unitID);
    },

    //item select in modal
    select_item: function () {
        var self = MaterialPurification;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var UnitID = $(row).find(".PrimaryUnitID").val();

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $('#Unit').val(Unit);
        $('#UnitID').val(UnitID);
        UIkit.modal($('#select-item')).hide();
    },

    select_purification_item: function () {
        var self = MaterialPurification;
        var radio = $('#select-material-purification-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var UnitID = $(row).find(".PrimaryUnitID").val();

        $("#PurificationItemName").val(Name);
        $("#PurificationItemID").val(ID);
        $('#PurificationUnit').val(Unit);
        $('#PurificationUnitID').val(UnitID);
        UIkit.modal($('#select-material-purification-item')).hide();
    },

    save_confirm: function () {
        var self = MaterialPurification
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    Save: function () {
        var self = MaterialPurification;
        var data;
        data = self.get_data();
        var location = "/Masters/MaterialPurification/Index";
        $('.btnSave').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/MaterialPurification/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("MaterialPurification Saved successfully");
                    window.location = location;
                } else {
                    app.show_error("Failed to create data.");
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID : $("#ID").val(),
            ItemID: $("#ItemID").val(),
            UnitID: $("#UnitID").val(),
            ProcessID: $("#ProcessID").val(),
            PurificationItemID: $("#PurificationItemID").val(),
            PurificationUnitID: $("#PurificationUnitID").val(),
        }
        return model;
    },

    validate_submit: function () {
        var self = MaterialPurification;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {

        on_submit: [
         {
             elements: "#ItemID",
             rules: [
                 { type: form.required, message: "Please choose an item" },
                 { type: form.non_zero, message: "Please choose an item" },
             ],
         },
         {
             elements: "#PurificationItemID",
             rules: [
                 { type: form.required, message: "Please choose Purification item" },
                 { type: form.non_zero, message: "Please choose Purification item" },
             ],
         },
         {
             elements: "#ItemName",
             rules: [
                 { type: form.required, message: "Please choose an item" },
                 { type: form.non_zero, message: "Please choose an item" },
             ],
         },
         {
             elements: "#PurificationItemName",
             rules: [
                 { type: form.required, message: "Please choose Purification item" },
                 { type: form.non_zero, message: "Please choose Purification item" },
             ],
         },
         {
             elements: "#ProcessID",
             rules: [
                 { type: form.required, message: "Please select Process" },
                 { type: form.non_zero, message: "Please select Process" },
             ],
         },
        ],
    }
};