UserLocations = {
    init: function () {
    },

    list: function () {
        var $list = $('#UserLocation-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/UserLocations/GetUserLocationsList"

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
                       }
                   },
                   { "data": "EmployeeCode", "className": "EmployeeCode" },
                   { "data": "EmployeeName", "className": "EmployeeName" },
                   { "data": "UserName", "className": "UserName" },
                   { "data": "defaultLocation", "className": "defaultLocation" },
                   { "data": "CurrentLocation", "className": "CurrentLocation" },
                   { "data": "OtherLocation", "className": "OtherLocation" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).find("td:eq(0) .ItemID").val();
                        //app.load_content("/Masters/UserLocations/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
}