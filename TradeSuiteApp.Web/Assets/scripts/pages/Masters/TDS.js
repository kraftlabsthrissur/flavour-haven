        TDS = {
    init: function () {
        var self = TDS;
        self.bind_events();
    },
    list: function () {
        var $list = $('#tds-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#tds-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/TDS/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
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
        var self = TDS;

    },
    bind_events: function () {
        var self = TDS;
        $(".btnSave").on('click', self.save_confirm);
    },

    save_confirm: function () {
        var self = TDS;
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

    save: function () {
        var self = TDS;
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/TDS/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("TDS created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/TDS/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            ItemAccountCategory: $("#ItemAccountCategory").val(),
            TDSRate: $("#TDSRate").val(),
            CompanyType: $("#CompanyType").val(),
            ExpenseType: $("#ExpenseType").val(),
            ITSection: $("#ITSection").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            Remarks: $("#Remarks").val()

        }
        return model;
    },

    validate_form: function () {
        var self = TDS;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [

             {
                 elements: "#Code",
                 rules: [
                    { type: form.required, message: "Code is Required" },
                 ]
             },
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: "Name is Required" },
                 ]
             },

             {
                 elements: "#ItemAccountCategory",
                 rules: [
                     { type: form.required, message: "ItemAccountCategory is Required" },
                 ]
             },
                 {
                     elements: "#TDSRate",
                     rules: [
                         { type: form.required, message: "TDSRate is Required" },
                     ]
                 },
                 {
                     elements: "#CompanyType",
                     rules: [
                         { type: form.required, message: "CompanyType is Required" },
                     ]
                 },
                 {
                     elements: "#ExpenseType",
                     rules: [
                         { type: form.required, message: "ExpenseType is Required" },
                     ]
                 },
                  {
                      elements: "#ITSection",
                      rules: [
                          { type: form.required, message: "ITSection is Required" },
                      ]
                  },
                  {
                      elements: "#EndDate",
                      rules: [
                  {
                      type: function (element) {
                          var u_date = $(element).val().split('-');
                          var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                          var a = Date.parse(used_date);
                          var po_date = $('#StartDate').val().split('-');
                          var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                          var date = Date.parse(po_datesplit);
                          return date <= a
                      }, message: "End date should be a date on or after start date"
                  }

                      ]
                  },
        ]
    },
}