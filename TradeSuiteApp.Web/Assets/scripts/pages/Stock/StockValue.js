
stockvalue = {
    init: function () {
        var self = stockvalue;
        self.bind_events();
    },
    list: function () {

        var self = stockvalue;
        self.bind_events();

        var $list = $('#stock-value-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": "/Stock/StockValue/GetStockValueList",
                    "type": "POST"
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
                    {
                        "data": "ItemName",
                        "className": "",
                    },
                    {
                        "data": "OpeningStock", "searchable": false, "orderable": true, "className": "OpeningStock",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.OpeningStock + "</div>";
                        }
                    },
                    {
                        "data": "OpeningRate", "searchable": false, "orderable": true, "className": "OpeningRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.OpeningRate + "</div>";
                        }
                    },
                    {
                        "data": "OpeningStockValue", "searchable": false, "orderable": true, "className": "OpeningStockValue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.OpeningStockValue + "</div>";
                        }
                    },
                    {
                        "data": "StockIn", "searchable": false, "orderable": true, "className": "StockIn",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.StockIn + "</div>";
                        }
                    },
                    {
                        "data": "StockInRate", "searchable": false, "orderable": true, "className": "StockInRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.StockInRate + "</div>";
                        }
                    },
                    {
                        "data": "NetStockValueIn", "searchable": false, "orderable": true, "className": "NetStockValueIn",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.NetStockValueIn + "</div>";
                        }
                    },
                    {
                        "data": "IssueStock", "searchable": false, "orderable": true, "className": "IssueStock",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.IssueStock + "</div>";
                        }
                    },
                    {
                        "data": "IssueRate", "searchable": false, "orderable": true, "className": "IssueRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.IssueRate + "</div>";
                        }
                    },
                    {
                        "data": "IssueValue", "searchable": false, "orderable": true, "className": "IssueValue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.IssueValue + "</div>";
                        }
                    },
                    {
                        "data": "ClosingStock", "searchable": false, "orderable": true, "className": "ClosingStock",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.ClosingStock + "</div>";
                        }
                    },
                    {
                        "data": "ClosingRate", "searchable": false, "orderable": true, "className": "ClosingRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.ClosingRate + "</div>";
                        }
                    },
                    {
                        "data": "ClosingValue", "searchable": false, "orderable": true, "className": "ClosingValue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.ClosingValue + "</div>";
                        }
                    },
                    {
                        "data": "LastUpdatedRate", "searchable": false, "orderable": true, "className": "LastUpdatedRate",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-production-qty' >" + row.LastUpdatedRate + "</div>";
                        }
                    },
                    { "data": "LastUpdatedDate", "className": "LastUpdatedDate", "searchable": true, },
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                }
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = stockvalue;
        $("#process").on('click', self.process);
    },


    process: function () {
        var self = stockvalue;
        $.ajax({
            url: '/Stock/StockValue/Execute',
            data: {

            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                window.location = window.location;
            }
        });
    },

}