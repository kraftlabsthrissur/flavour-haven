$(function () {
    WareHouse.init();
});
var fh_items;
WareHouse = {
    init: function () {
        var self = WareHouse;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.list();
        } else {
            fh_items = $("#warehouse-item-list").FreezeHeader();
        }
        self.bind_events();
    },

    list: function () {
        var $list = $('#warehouse-list');
        if ($list.length) {
            $('#warehouse-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/WareHouse/Details/' + Id;
            });

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
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        self = WareHouse;
        $("body").on('click', '.btnupdate', WareHouse.update);
        $("body").on('click', '.btnsave', WareHouse.save_confirm);
    },

    save_confirm: function () {
        var self = WareHouse;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    error_count: 0,
    save: function () {
        var self = WareHouse;
        $('.save').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/WareHouse/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice(data.message);
                    setTimeout(function () {
                        window.location = "/Masters/WareHouse/Index"
                    }, 1000);
                } else {
                    app.show_error(data.message);
                    $('.save').css({ 'display': 'block' });
                }
            },
        });
    },
    update: function () {
        var self = WareHouse;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.update').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/WareHouse/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("WareHouse updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/WareHouse/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.update').css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Place: $("#Place").val(),
            Remarks: $("#Remarks").val(),
            ItemTypeID: $("#ItemTypeID").val(),
            LocationID: $("#LocationID").val(),
        }
        return model;
    },
    validate_form: function () {
        var self = WareHouse;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    error_count: 0,
    rules: {
        on_submit: [

             {
                 elements: "#Code",
                 rules: [
                    { type: form.required, message: "WareHouse Code is Required" },
                 ]
             }, {
                 elements: "#Name",
                 rules: [
                    { type: form.required, message: "WareHouse Name is Required" },
                 ]
             },
             {
                 elements: "#Place",
                 rules: [
                     { type: form.required, message: "WareHouse Place is Required" },
                 ]
             },
             {
                 elements: "#LocationID",
                 rules: [
                     { type: form.required, message: "WareHouse Place is Required" },
                 ]
             },
        ]
    }

}