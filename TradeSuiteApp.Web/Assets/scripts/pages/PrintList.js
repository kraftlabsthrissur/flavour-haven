PrintList = {
    init: function () {
        var self = PrintList;
        self.bind_events();
        self.list();
    },

    bind_events: function () {
        var self = PrintList;
        $("body").on("click", ".generate-file", self.get_generate_file);

    },


    list: function () {
        var $list = $('#Print-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Home/GetPrintList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "DateFrom", Value: $("#DateFrom").val() },
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
                            + '     <input type="hidden" class="ID" value="' + row.ID + '"  />';

                       }
                   },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "Form", "className": "Form" },
                    { "data": "CreatedDate", "className": "CreatedDate" },
                    {
                        "data": null,
                        render: function (data, type, row, meta) {
                            return "<a href='/Outputs/" + row.Form + "/" + row.TransNo + ".txt' />Download</a>";
                        }
                    },
                    {
                        "data": null,
                        render: function (data, type, row, meta) {
                            return "<a class='generate-file' data-url='/" + row.Area + "/" + row.Form + "/" + "Print" + "' data-id='" + row.ID + "'/>Generate File</a>";
                        }
                    }

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                    });
                },
            });

            $('body').on("change", '#DateFrom', function () {
                list_table.fnDraw();
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });


        }
    },

    get_generate_file: function () {
        var self = PrintList;
        var url = $(this).data("url");
        var ID = $(this).data("id");
        $.ajax({
            url: url,
            data: { ID : ID},
            dataType: "json",
            type: "POST",
            success: function () {
            },
        });
    },
}