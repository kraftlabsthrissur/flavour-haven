// This js used For DSCentre Build Print change
CounterSales.print_countersales_invoice= function () {
    var self = CounterSales;
    $.ajax({
        url: '/Reports/Sales/CounterSalesInvoicePrintPdfV5',
        data: {
            id: $("#ID").val(),
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status = "success") {
                var url = response.URL;
                app.print_preview(url);
            }
        }
    });
};
CounterSales.list= function () {
    var self = CounterSales;

    $('#tabs-counter-sales').on('change.uk.tab', function (event, active_item, previous_item) {
        if (!active_item.data('tab-loaded')) {
            self.tabbed_list(active_item.data('tab'));
            active_item.data('tab-loaded', true);
        }
    });
},
CounterSales.tabbed_list= function (type) {
    var self = CounterSales;

    var $list;

    switch (type) {
        case "draft":
            $list = $('#draft-counter-sales');
            break;
        case "saved-counter-sales":
            $list = $('#saved-counter-sales');
            break;
        case "cancelled":
            $list = $('#cancel-counter-sales');
            break;
        default:
            $list = $('#draft-counter-sales');
    }

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
                "url": "/Sales/CounterSales/GetListForCounterSales?type=" + type,
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
                        + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                    }
                },
                { "data": "TransNo", "className": "TransNo" },
                { "data": "TransDate", "className": "TransDate" },
                { "data": "Type", "className": "Type" },
                { "data": "PartyName", "className": "PartyName", },
                {
                    "data": "NetAmount", "searchable": false, "className": "NetAmount",
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                    }
                },
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.IsDraft);
                $(row).addClass(data.IsCancelled);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.action)').on('click', function () {
                    var Id = $(this).closest("tr").find("td .ID").val();
                    app.load_content("/Sales/CounterSales/DetailsV5/" + Id);
                });
            },
        });

        $list.find('thead.search input').on('textchange', function () {
            var index = $(this).parent().parent().index();
            list_table.api().column(index).search(this.value).draw();
        });

        return list_table;
    }
}
CounterSales.on_save= function () {

    var self = CounterSales;
    var data = self.get_data();
    var location = "/Sales/CounterSales/IndexV5";
    var url = '/Sales/CounterSales/Save';
    IsPrint = true;
    if ($(this).hasClass("btnSaveASDraft")) {
        data.IsDraft = true;
        IsPrint = false;
        url = '/Sales/CounterSales/SaveAsDraft'
        self.error_count = self.validate_draft();
    } else {
        self.error_count = self.validate_form();
        if ($(this).hasClass("btnSaveAndPrint")) {
            IsPrint = true;
        }
        if ($(this).hasClass("btnSaveAndNew")) {
            location = "/Sales/CounterSales/CreateV5";
            $("#save_new").val("new");
        }
    }

    if (self.error_count > 0) {
        return;
    }

    if (!data.IsDraft) {
        app.confirm_cancel("Do you want to save?", function () {
            IsPrint = true;
            self.save(data, url, location);
        }, function () {
        })
    } else {
        IsPrint = false;
        self.save(data, url, location);
    }
}    
CounterSales.print_close = function () {
    var self = CounterSales;
    var SaveFunction = $("#save_new").val();
    if ($("#page_content_inner").hasClass("sales form-view")) {
        var url = '/Sales/CounterSales/IndexV5';
        if (SaveFunction == 'new') {
            var url = '/Sales/CounterSales/CreateV5';
        }
        setTimeout(function () {
            window.location = url
        }, 1000);
    }
};

CounterSales.details = function () {
    var self = CounterSales;
    setTimeout(function () {
        fh_items = $("#counter-sales-items-list").FreezeHeader();
    }, 50);
    $("body").on("click", ".printpdf", self.printpdf);
    $("body").on("click", ".cancel", self.cancel_confirm);
    $(document).on('keydown', null, 'alt+l', self.on_close);
    $(document).on('keydown', null, 'alt+c', self.on_cancel);
    $(document).on('keydown', null, 'alt+r', self.on_print);
    $("body").on("click", ".print", self.print);
    $("body").on("click", ".btnPrint", self.thermal_print);
},

CounterSales.printpdf = function () {
    var self = CounterSales;
    $.ajax({
        url: '/Reports/Sales/CounterSalesInvoicePrintPdfV5',
        data: {
            id: $("#ID").val(),
        },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status = "success") {
                var url = response.URL;
                app.print_preview(url);
            }
        }
    });
};
CounterSales.save = function (data, url, location) {

    var self = CounterSales;
    var IsDotMatrixPrint = $("#IsDotMatrixPrint").val();
    var IsThermalPrint = $("#IsThermalPrint").val();

    $(".btnSaveASDraft, .btnSave,.cancel,.btnSaveAndNew,.btnSaveAndPrint").css({ 'display': 'none' });

    $.ajax({
        url: url,
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Counter Sales Saved Successfully");
                $("#ID").val(response.ID);
                if (IsPrint == true) {
                    if (IsDotMatrixPrint == "True" && IsThermalPrint=="False")
                    {
                        self.print();
                    }
                    else if(IsDotMatrixPrint == "False" && IsThermalPrint=="True")
                    {
                        self.thermal_print_on_create();
                    }
                    else {
                        self.print_countersales_invoice();
                    }
                }
                else
                {
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                }
                   
            } else {
                if (typeof response.data[0].ErrorMessage != "undefined")
                    app.show_error(response.data[0].ErrorMessage);
                $(".btnSaveASDraft, .btnSave,.cancel,.btnSaveAndNew,.btnSaveAndPrint").css({ 'display': 'block' });

            }
        }
    });
}



