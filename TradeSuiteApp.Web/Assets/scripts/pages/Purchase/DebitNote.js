$(function () {
    altair_datatables.debitnote_list();
});
altair_datatables = {
    debitnote_list: function () {
        $debitnote_list = $('#debitnote-list');
       
        if ($debitnote_list.length) {

            $('#debitnote-list tbody tr').on('click', function () {
                window.location = 'Edit/1';
            });

            $debitnote_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var debitnote_list_table = $debitnote_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            debitnote_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    debitnote_list_table.api().column(index).search(this.value).draw();
                });
            });

        }
    }
};