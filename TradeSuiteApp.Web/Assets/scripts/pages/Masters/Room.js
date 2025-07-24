Room = {
    init: function () {
        var self = Room;
        self.bind_events();
    },
    list: function () {
        var $list = $('#room_list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#room_list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/Room/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
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
        var self = Room;
        $(".btnSave").on('click', self.save_confirm);
    },
    save_confirm: function () {
        var self = Room;
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
    validate_form: function () {
        var self = Room;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    save: function () {
        var self = Room;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Room/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Room created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Room/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },

    get_data: function () {
        var self = Room;
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            RoomTypeID: $("#RoomTypeID").val(),
            RoomName: $("#RoomName").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            Rate:clean($("#Rate").val()),
            Description: $("#Description").val(),
            StoreID:$("#StoreID").val()
        }
        return model;
    },
    rules: {
        on_submit: [
             {
                 elements: "#RoomName",
                 rules: [
                     { type: form.required, message: "Room Name is Required" },
                 ]
             },

             {
                 elements: "#RoomTypeID",
                 rules: [
                     { type: form.required, message: "Select Room Type" },
                 ]
             },
             {
                 elements: "#StartDate",
                 rules: [
                     { type: form.required, message: "StartDate  is Required" },
                 ]
             },
              {
                  elements: "#EndDate",
                  rules: [
                      { type: form.required, message: "EndDate  is Required" },
                  ]
              },
               {
                   elements: "#Rate",
                   rules: [
                       { type: form.required, message: "Rate  is Required" },
                   ]
               },
                 {
                     elements: "#StoreID",
                     rules: [
                         { type: form.required, message: "Select Store" },
                     ]
                 },
        ]
    },
}