PeriodClosing = {
    init: function () {
        var self = PeriodClosing;
        self.bind_events();
    },

    //list: function () {
    //    var $list = $('#periodClosing-list');
    //    if ($list.length) {
    //        $list.find('thead.search th').each(function () {
    //            var title = $(this).text().trim();
    //            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
    //        });
    //        $('#periodClosing-list tbody tr').on('click', function () {
    //            var id = $(this).find('.ID').val();
    //            window.location = '/Masters/PaymentGroup/Details/' + id;
    //        });
    //        altair_md.inputs();
    //        var list_table = $list.dataTable({
    //            "bLengthChange": false,
    //            "bFilter": true
    //        });
    //        list_table.api().columns().each(function (g, h) {
    //            $('thead.search input').on('keyup change', function () {
    //                var index = $(this).parent().parent().index();
    //                list_table.api().column(index).search(this.value).draw();
    //            });
    //        });
    //    }
    //},
    bind_events: function () {
        var self = PeriodClosing;
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = PeriodClosing
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    save: function () {
        var self = PeriodClosing;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/PeriodClosing/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Period Closing created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/PeriodClosing/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create Period Closing");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var modal = {};
        modal.Items = [];
        var item = {};
        if ($(".JournalStatus").val == "Open") {
            I
        }
        $("#periodclosing-list tbody tr").each(function () {
            item = {};
            item.ID = clean($(this).find(".ID").val()),
            item.Month = $(this).find(".Month").val()
            item.JournalStatus = $(this).find(".JournalStatus").val(),
            item.SDNStatus = $(this).find(".SDNStatus").val(),
            item.SCNStatus = $(this).find(".SCNStatus").val(),
            item.CDNStatus = $(this).find(".CDNStatus").val(),
            item.CCNStatus = $(this).find(".CCNStatus").val(),
            modal.Items.push(item);
        });
        return modal;
    },

    validate_form: function () {
        var self = PeriodClosing;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [
             {
                 elements: ".JournalStatus",
                 rules: [
                     {
                         type: function (element) {
                             if ($(element).val() == "FinallyClosed" || $(element).val() == "Closed") {
                                 return $(element).closest("tr").prevAll("tr").find('.JournalStatus option:selected[value="Open"]').length > 0 ? false : true;
                             }
                             return true;
                         }, message: "Previous Month must be Closed"
                     },
                 ]
             },
             {
                 elements: ".SDNStatus",
                 rules: [
                     {
                         type: function (element) {
                             if ($(element).val() == "FinallyClosed" || $(element).val() == "Closed") {
                                 return $(element).closest("tr").prevAll("tr").find('.SDNStatus option:selected[value="Open"]').length > 0 ? false : true;
                             }
                             return true;
                         }, message: "Previous Month must be Closed"
                     }
                 ]
             },
             {
                 elements: ".SCNStatus",
                 rules: [
                     {
                         type: function (element) {
                             if ($(element).val() == "FinallyClosed" || $(element).val() == "Closed") {
                                 return $(element).closest("tr").prevAll("tr").find('.SCNStatus option:selected[value="Open"]').length > 0 ? false : true;
                             }
                             return true;
                         }, message: "Previous Month must be Closed"
                     },
                 ]
             },
             {
                 elements: ".CDNStatus",
                 rules: [
                     {
                         type: function (element) {
                             if ($(element).val() == "FinallyClosed" || $(element).val() == "Closed") {
                                 return $(element).closest("tr").prevAll("tr").find('.CDNStatus option:selected[value="Open"]').length > 0 ? false : true;
                             }
                             return true;
                         }, message: "Previous Month must be Closed"
                     },
                 ]
             },
             {
                 elements: ".CCNStatus",
                 rules: [
                     {
                         type: function (element) {
                             if ($(element).val() == "FinallyClosed" || $(element).val() == "Closed") {
                                 return $(element).closest("tr").prevAll("tr").find('.CCNStatus option:selected[value="Open"]').length > 0 ? false : true;
                             }
                             return true;
                         }, message: "Previous Month must be Closed"
                     },
                 ]
             },
        ]
    },
}