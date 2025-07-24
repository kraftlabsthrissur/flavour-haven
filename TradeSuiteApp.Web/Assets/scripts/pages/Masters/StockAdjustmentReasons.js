StockAdjustmentReasons = {
    init: function () {
        var self = StockAdjustmentReasons;

        self.bind_events();
    },

    list: function () {
        var self = StockAdjustmentReasons;
        var $list = $('#Stock-Adjustment-Reasons-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/StockAdjustmentReasons/GetStockAdjustmentReasonsList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
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
                   { "data": "Code", "className": "Code" },
                   { "data": "Name", "className": "Name" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/StockAdjustmentReasons/Details/" + Id);
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
        var self = StockAdjustmentReasons;
        $(".btnSave").on("click", self.save_confirm);
    },

    save_confirm: function () {
        var self = StockAdjustmentReasons;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    get_data: function () {
        var data = {
            ID: $("#ID").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Remarks: $("#Remarks").val(),
        };
        return data;
    },

    save: function () {
        var self = StockAdjustmentReasons;
        var data;
       
        data = self.get_data();
        $.ajax({
            url: '/Masters/StockAdjustmentReasons/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    setTimeout(function () {
                        window.location = "/Masters/StockAdjustmentReasons/Index";
                    }, 1000);
                } else {
                    app.show_error("Already Exists.");
                }
            }
        });
    },

    rules: {
        on_save: [
      {
          elements: "#Code",
          rules: [
              { type: form.required, message: "Please enter Code" },
          ]
      },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Please enter Name" },
                ]
            },
        ],
    },

    validate_save: function () {
        var self = StockAdjustmentReasons;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },


}