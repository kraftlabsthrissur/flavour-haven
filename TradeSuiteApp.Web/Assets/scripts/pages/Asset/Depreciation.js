$(function () {

});
var capital_items_list;

depreciation = {
    init: function () {
        depreciation.bind_events();
    },
    list: function () {
        capital_items_list = depreciation.items_list();
        depreciation.bind_events();
    },
    items_list: function () {
        var url;
        $list = $('#capital-items-list');
        url = "/Asset/Depreciation/GetCapitalList";

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "FromCompanyDepreciationRate", Value: clean($('#FromCompanyDepreciationRate').val()) },
                            { Key: "ToCompanyDepreciationRate", Value: clean($('#ToCompanyDepreciationRate').val()) },
                            { Key: "FromIncomeTaxDepreciationRate", Value: clean($('#FromIncomeTaxDepreciationRate').val()) },
                            { Key: "ToIncomeTaxDepreciationRate", Value: clean($('#ToIncomeTaxDepreciationRate').val()) },
                            { Key: "TransDateFrom", Value: $('#TransDateFromStr').val() },
                            { Key: "TransDateTo", Value: $('#TransDateToStr').val() },
                        ];
                    }
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return (meta.settings.oAjaxData.start + meta.row + 1)
                                + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "AssetName", "className": "AssetName", },
                    { "data": "ItemName", "className": "ItemName", },
                    { "data": "SupplierName", "className": "SupplierName", },
                    {
                        "data": "GrossBlockAssetValue", "className": "GrossBlockAssetValue ",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.GrossBlockAssetValue + "</div>";
                        }
                    },
                    {
                        "data": "OpeningWDV", "className": "OpeningWDV",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.OpeningWDV + "</div>";
                        }
                    },
                    {
                        "data": "DepreciationRate", "className": "DepreciationRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.DepreciationRate + "</div>";
                        }
                    },
                    {
                        "data": "AccumulatedDepreciationValue", "className": "AccumulatedDepreciationValue",
                             "render": function (data, type, row, meta) {
                                 return "<div class='mask-currency' >" + row.AccumulatedDepreciationValue + "</div>";
                             }
                    },
                         {
                             "data": "DepreciationValue", "className": "DepreciationValue",
                             "render": function (data, type, row, meta) {
                                 return "<div class='mask-currency' >" + row.DepreciationValue + "</div>";
                             }
                         },

                    {
                        "data": "CurrentValue", "className": "CurrentValue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.CurrentValue + "</div>";
                        }
                    },

                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                }
            });
            $('#btnSubmit').on("click", function () {
                $(".filter").addClass("uk-hidden");
                list_table.fnDraw();
            });         
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },
    bind_events: function () {
        var self = depreciation;
        $(".btnFilter").on("click", self.filter);
        $("#btnReset").on("click", self.reset);
        $(".btnProcess").on("click", self.process);
    },
    process: function () {
        $(".btnProcess").css({ 'display': 'none' });

        $.ajax({
            url: '/Asset/Depreciation/CalculateDepreciation',

            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(response.data);
                
                }
                else {
                    app.show_error(response.data);
                    $(".btnProcess").css({ 'display': 'block' });

                }
            }
        });
        window.location = window.location;
    },
    filter: function () {
        $(".filter").removeClass("uk-hidden");
    },
    reset: function () {
        self = depreciation;
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var currentdate = (('' + day).length < 2 ? '0' : '') + day + '-' + (('' + month).length < 2 ? '0' : '') + month + '-' + d.getFullYear();
        var findate = '1-4-' + d.getFullYear();
        $("#TransDateToStr").val(currentdate);
        $("#TransDateFromStr").val(findate);
        $("#FromCompanyDepreciationRate").val('0.00');
        $("#ToCompanyDepreciationRate").val('0.00');
        $("#FromIncomeTaxDepreciationRate").val('0.00');
        $("#ToIncomeTaxDepreciationRate").val('0.00');
    },
}