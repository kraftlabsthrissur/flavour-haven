SLA = {
    list: function () {
        var self = SLA;
        self.values_list("Values");
        self.to_be_posted_list("ToBePosted");
        self.posted_list("Posted");
        self.errors_list("Errors");
        self.bind_events();
    },

    values_list: function (type) {
        var self = SLA;
        var $list = $('#sla-values-items-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/SLA/GetSLAValuesList?type=" + type;

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
                        data.params = [
                            { Key: "Type", Value: type },
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                        }
                    },
                    { "data": "Date", "className": "Date" },
                    { "data": "TransationType", "className": "TransationType" },
                    { "data": "KeyValue", "className": "KeyValue" },
                    {
                        "data": "Amount", "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        }
                    },
                    { "data": "Event", "className": "Event" },
                    { "data": "DocumentTable", "className": "DocumentTable" },
                    { "data": "DocumentNo", "className": "DocumentNumber" },
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    to_be_posted_list: function (type) {
        var self = SLA;
        var $list = $('#sla-to-be-posted-items-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/SLA/GetSLAToBePostedList?type=" + type;

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
                        data.params = [
                            { Key: "Type", Value: type },
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                        }
                    },
                    { "data": "Date", "className": "Date" },
                    { "data": "AccountDebitID", "className": "DebitAccount" },
                    { "data": "DebitAccount", "className": "DebitAccountName" },
                    { "data": "AccountCreditID", "className": "CreditAccount" },
                    { "data": "CreditAccount", "className": "CreditAccountName" },
                    {
                        "data": "Amount", "className": "Amount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.Amount + "</div>";
                        }
                    },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    { "data": "DocumentTable", "className": "DocumentTable" },
                    { "data": "DocumentNo", "className": "DocumentNumber" },
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    posted_list: function (type) {
        var self = SLA;
        var $list = $('#sla-posted-items-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/SLA/GetSLAPostedList?type=" + type;

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
                        data.params = [
                            { Key: "Type", Value: type },
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "Date", "className": "Date" },
                   { "data": "AccountDebitID", "className": "DebitAccount" },
                   { "data": "DebitAccount", "className": "DebitAccountName" },
                   { "data": "AccountCreditID", "className": "CreditAccount" },
                   { "data": "CreditAccount", "className": "CreditAccountName" },
                   {
                       "data": "Amount", "className": "Amount",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.Amount + "</div>";
                       }
                   },
                   { "data": "DocumentTable", "className": "DocumentTable" },
                   { "data": "DocumentNo", "className": "DocumentNumber" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    errors_list: function (type) {
        var self = SLA;
        var $list = $('#sla-error-items-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/SLA/GetSLAErrorList?type=" + type;

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
                        data.params = [
                            { Key: "Type", Value: type },
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "CreatedDate", "className": "Date" },
                   { "data": "TrnType", "className": "TransationType" },
                   { "data": "KeyValue", "className": "KeyValue", },
                   { "data": "TableName", "className": "Event" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "SupplierName", "className": "SupplierName" },
                   { "data": "Description", "className": "Description" },
                   { "data": "Remarks", "className": "Remarks" },
                   { "data": "DocumentTable", "className": "DocumentTable" },
                   { "data": "DocumentNo", "className": "DocumentNumber" },
                    {
                        "data": null,
                        "className": "",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "";
                        }
                    },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
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
        $('.sla-process').on('click', SLA.process);
        $('.sla-post').on('click', SLA.post);
    },

    process: function () {
        $.ajax({
            url: '/Accounts/SLA/Process/',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Processed successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/SLA/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                }
            }
        });
    },

    post: function () {
        $.ajax({
            url: '/Accounts/SLA/PostAccounts/',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Posted successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/SLA/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                }
            }
        });
    }
}