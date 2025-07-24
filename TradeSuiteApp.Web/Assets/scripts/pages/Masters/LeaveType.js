LeaveType = {
    init: function () {
        self = LeaveType;
        self.bind_events();
    },

    details: function () { },

    list: function () {
        $leavetype_list = $('#leavetype-list');
        $('#leavetype-list tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Masters/LeaveType/Details/' + id;
        });
        if ($leavetype_list.length) {
            $leavetype_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var leavetype_list_table = $leavetype_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            leavetype_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    leavetype_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        var self = LeaveType;
    },
}