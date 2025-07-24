BillType = {
    init: function () {
        var self = BillType;
        self.bind_events();
        $('#InPatient').hide();
        self.get_op_item();
        self.get_ip_item();
    },
    bind_events: function () {
        var self = BillType;
        $('.radioOp').on("ifChecked", self.show_op_services);
        $('.radioIp').on("ifChecked", self.show_ip_services);
        $(".btnSave").on('click', self.save_confirm);
        $("body").on("ifChanged", ".op", self.set_op_item);
        $("body").on("ifChanged", ".ip", self.set_ip_item);
    },
    set_op_item: function () {
        var self = BillType;
        if ($(this).is(":checked") == true) {
            var BillTypeID = clean($(this).val());
            self.OPItems.push({ BillTypeID: BillTypeID,Type:'OP'});
        }
        else {
            var BillTypeID = clean($(this).val());
            var index = self.OPItems.indexOf(BillTypeID)
            self.OPItems.splice(index, 1);
        }
    },
    get_op_item: function () {
        var self = BillType;
        $.ajax({
            url: '/AHCMS/BillType/GetBillTypeItems',
            dataType: "json",
            data: {
                Type:'OP',
            },
            type: "POST",
            success: function (response) {
                $(response.BillTypeID).each(function (i, record) {
                    var BillTypeID = record.BillTypeID;
                    self.OPItems.push({ BillTypeID: BillTypeID,Type:'OP' });
                });
            }
        });

    },
    get_ip_item: function () {
        var self = BillType;
        $.ajax({
            url: '/AHCMS/BillType/GetBillTypeItems',
            dataType: "json",
            data: {
                Type: 'IP',
            },
            type: "POST",
            success: function (response) {
                $(response.BillTypeID).each(function (i, record) {
                    var BillTypeID = record.BillTypeID;
                    self.IPItems.push({ BillTypeID: BillTypeID, Type: 'IP' });
                });
            }
        });

    },
    set_ip_item: function () {
        var self = BillType;
        if ($(this).is(":checked") == true) {
            var BillTypeID = clean($(this).val());
            self.IPItems.push({ BillTypeID: BillTypeID, Type: 'IP' });
        }
        else {
            var BillTypeID = clean($(this).val());
            var index = self.IPItems.indexOf(BillTypeID)
            self.IPItems.splice(index, 1);
        }
    },
    show_op_services: function () {
        var self = BillType;
        $('#InPatient').hide();
        $('#OutPatient').show();
    },
    show_ip_services: function () {
        var self = BillType;
        $('#OutPatient').hide();
        $('#InPatient').show();
    },
    save_confirm: function () {
        var self = BillType;
        self.error_count = 0;

        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },
    Save: function () {
        var self = BillType;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/BillType/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                }
                else {
                    app.show_error("Failed to Create");
                    $(" .btnSave").css({ 'visibility': 'visible' });

                }
            }
        });
    },
    get_data: function () {
        var self = BillType;
        var data = {};
        data.OPItems = [];
        data.IPItems = [];
        data.OPItems = self.OPItems;
        data.IPItems = self.IPItems;
        return data;
    },
    OPItems: [],
    IPItems: [],
}