
TransportPermitReport = {
    init: function () {
        var self = TransportPermitReport;
        self.bind_events();
        //self.hide_show_transNo();
        //   ReportHelper.init();
    },

    list: function () {
        var self = TransportPermitReport;
        var $list = $('#transport-permit-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Sales/TransportPermit/GetTransportPermitListForDataTable"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "Desc"]],
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
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "ValidFromdate", "className": "ValidFromdate" },
                   { "data": "ValidTodate", "className": "ValidTodate" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Sales/TransportPermit/Details/" + Id);
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
        var self = TransportPermitReport;
        //    $("body").on("ifChanged", ".StockTransferIssue", self.hide_show_transNo);
        //$("body").on("ifChanged", ".SalesInvoice", self.hide_show_transNo);
        // $("body").on("click", "#report-filter-submit", self.get_transportpermit_items);

        $("body").on("click", "#Show", self.get_transportpermit_items);
        $("body").on("click", ".SaveAndPrint", self.print);
        $("body").on("click", ".print", self.print_detail);
        $("body").on('ifChanged', '.chkCheck', self.include_item);
        $('.select-all').on('ifChanged', self.include_item_all);
        $.UIkit.autocomplete($('#stock-issuenoFrom-autocomplete'), { 'source': self.get_stock_transfer_code_from, 'minLength': 1 });
        $('#stock-issuenoFrom-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_transfer_code_from);

        $.UIkit.autocomplete($('#stock-issuenoTo-autocomplete'), { 'source': self.get_stock_transfer_code_to, 'minLength': 1 });
        $('#stock-issuenoTo-autocomplete').on('selectitem.uk.autocomplete', self.set_stock_transfer_code_to);
        $.UIkit.autocomplete($('#sales-invoiceno_from-autocomplete'), { 'source': self.get_sales_invoice_code_from, 'minLength': 1 });
        $('#sales-invoiceno_from-autocomplete').on('selectitem.uk.autocomplete', self.set_sales_invoice_code_from);
        $.UIkit.autocomplete($('#sales-ordernoTo-autocomplete'), { 'source': self.get_sales_invoice_code_to, 'minLength': 1 });
        $('#sales-ordernoTo-autocomplete').on('selectitem.uk.autocomplete', self.set_sales_invoice_code_to);
    },


    

    include_item_all: function () {
        if ($(this).is(":checked")) {
            $(this).closest('table').find('.chkCheck').iCheck('check');
        } else {
            $(this).closest('table').find('.chkCheck').iCheck('uncheck');

        }
    },

    include_item: function () {
        var self = TransportPermitReport;
        if ($(this).is(":checked")) {
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').removeClass('included');
            
        }

        self.count_items();

    },

    count_items: function () {
        var count = $('#TransportPermitList tbody').find('input.chkCheck:checked').length;
        $('#item-count').val(count);
        var rowcount = $('#TransportPermitList tbody tr').length;
        if (count==rowcount)
        {
            $(".select-all").prop("checked", true)
            $(".select-all").closest('div').addClass("checked")
        }
        else
        {
            $(".select-all").prop("checked", false)
            $(".select-all").closest('div').removeClass("checked")
        }
    },
    show_template: function () {
        var self = TransportPermitReport;
        //self.error_count = 0;
        //self.error_count = self.validate_on_upload();
        //if (self.error_count > 0) {
        //    return;
        //}
        var DriverName = $('#DriverName').val();
        var FromDate = $('#FromDate').val();
        var ToDate = $('#ToDate').val();
        var VehicleNumber = $('#VehicleNumber').val();
        var StockTransferIssue;
        var SalesInvoice;
        if ($(".StockTransferIssue").prop("checked") == true) {
            StockTransferIssue == true
        }
        else {
            StockTransferIssue == false
        }
        if ($(".SalesInvoice").prop("checked") == true) {
            SalesInvoice == true
        }
        else {
            SalesInvoice == false
        }

        window.location = "/Reports/Sales/TransportPermit/?DriverName=" + DriverName + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&VehicleNumber=" + VehicleNumber;
    },



    get_stock_transfer_code_from: function (release) {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Stock/StockIssue/GetIssueNoAutoCompleteForReport',
            data: {
                CodeHint: $('#StockTransferNoFrom').val(),
                FromDate: $('#FromDate').val(),
                ToDate: $('#ToDate').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_stock_transfer_code_from: function (event, item) {
        self = TransportPermitReport;
        $("#StockTransferNoFrom").val(item.IssueNo);
        $("#StockTransferNoFromID").val(item.id);
    },

    get_stock_transfer_code_to: function (release) {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Stock/StockIssue/GetIssueNoAutoCompleteForReport',
            data: {
                CodeHint: $('#StockTransferNoTo').val(),
                FromDate: $('#FromDate').val(),
                ToDate: $('#ToDate').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_stock_transfer_code_to: function (event, item) {
        self = TransportPermitReport;
        $("#StockTransferNoTo").val(item.IssueNo);
        $("#StockTransferNoToID").val(item.id);
    },

    Reset: function () {
        var self = TransportPermitReport;
        var currentdate = $("#CurrentDate").val();
        $("#FromDate").val(currentdate);
        $("#ToDate").val(currentdate);
        $("#DriverName").val('');
        $("#VehicleNumber").val('');
        $("#StockTransferNoFrom").val('');
        $("#StockTransferNoTo").val('');
        $("#SalesInvoiceNoFrom").val('');
        $("#SalesInvoiceNoTo").val('');
    },

    get_sales_invoice_code_from: function (release) {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Sales/SalesInvoice/GetSalesInvoiceCodeAutoCompleteForReport',
            data: {
                CodeHint: $('#SalesInvoiceNoFrom').val(),
                FromDate: $('#FromDate').val(),
                ToDate: $('#ToDate').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_sales_invoice_code_from: function (event, item) {
        self = TransportPermitReport;
        $("#SalesInvoiceNoFrom").val(item.IssueNo);
        $("#SalesInvoiceNoFromID").val(item.id);
    },

    get_sales_invoice_code_to: function (release) {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Sales/SalesInvoice/GetSalesInvoiceCodeAutoCompleteForReport',
            data: {
                CodeHint: $('#SalesInvoiceNoTo').val(),
                FromDate: $('#FromDate').val(),
                ToDate: $('#ToDate').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_sales_invoice_code_to: function (event, item) {
        self = TransportPermitReport;
        $("#SalesInvoiceNoTo").val(item.IssueNo);
        $("#SalesInvoiceNoToID").val(item.id);
    },

    get_transportpermit_items: function () {
        var self = TransportPermitReport;
        var ReportType;

        if ($(".StockTransferIssue").prop("checked") == false && $(".SalesInvoice").prop("checked") == false) {
            app.show_error("Please check atleat one check box");
            return;
        }

        self.error_count = 0;
        self.error_count = self.validate_show();
        if (self.error_count > 0) {
            return;
        }

        if ($(".StockTransferIssue").prop("checked") == true && $(".SalesInvoice").prop("checked") == true) {
            ReportType = 'All';
        }
        else if ($(".SalesInvoice").prop("checked") == true) {
            ReportType = 'SalesInvoice';
        }
        else {
            ReportType = 'StockTransferIssue';
        }
        model = {
            StockTransferNoFromID: $('#StockTransferNoFromID').val(),
            StockTransferNoToID: $('#StockTransferNoToID').val(),
            SalesInvoiceNoFromID: $('#SalesInvoiceNoFromID').val(),
            SalesInvoiceNoToID: $('#SalesInvoiceNoToID').val(),
            FromDate: $('#FromDate').val(),
            ToDate: $('#ToDate').val(),
            // SalesInvoiceNoToID: $('#SalesInvoiceNoToID').val(),
            Type: ReportType
        };
        $("#TransportPermitList tbody").html('');
        $.ajax({
            url: '/Sales/TransportPermit/TransportPermit',
            data: {
                model: model
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                $(".select-all").prop("checked", true)
                $(".select-all").closest('div').addClass("checked")
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr class="included">'
                        + '<td class="sl-no uk-text-center">' + slno + '</td>'
                        + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  chkCheck"/>' + '</td>'

                           //--+ "<input type='hidden' class='purchase-order-id' /></td>"
                        + '<td class="TransNo">' + item.TransNo
                         + "<input type='hidden' class='TransID' value=" + item.TransID + " />"
                    + "<input type='hidden' class='CustomerID' value=" + item.CustomerID + " />"
                    + "<input type='hidden' class='LocationID' value=" + item.LocationID + " />"
                    + "<input type='hidden' class='DistrictID' value=" + item.DistrictID + " />"
                    + "<input type='hidden' class='BatchTypeID' value=" + item.BatchTypeID + " />"
                     + "<input type='hidden' class='Quantity' value=" + item.Quantity + " />"
                    + "<input type='hidden' class='ItemID' value=" + item.ItemID + " />"
                        + '</td>'
                        + '<td class="TransDate">' + item.TransDate + '</td>'
                        + '<td class="Type">' + item.Type + '</td>'
                        + '<td class="CustomerName">' + item.CustomerName + '</td>'
                        + '<td class="District">' + item.District + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#TransportPermitList tbody").html($content);
                self.count_items();
            },
        });

    },

    print: function () {
        var self = TransportPermitReport;
        var ReportType;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }

        var model = self.get_data();
        $('.SaveAndPrint').css({ 'display': 'none' });

        $.ajax({
            url: '/Sales/TransportPermit/SaveAndPrint',
            data: {
                model: model
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var ID = response.data;
                    self.show_print(ID);
                }
            }
        });

    },
    show_print: function (ID) {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Reports/Sales/TransportPermit',
            data: {
                ID: ID
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

    },

    print_detail: function () {
        var self = TransportPermitReport;
        $.ajax({
            url: '/Reports/Sales/TransportPermit',
            data: {
                ID: $("#ID").val()
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

    },
    get_data: function () {
        var self = TransportPermitReport;
        var validFromdate = $('#ValidFromDate').val() == "" ? $('#FromDate').val() : $('#ValidFromDate').val();
        var validTodate = $('#ValidToDate').val() == "" ? $('#ToDate').val() : $('#ValidToDate').val();
        var model = {
            DriverName: $('#DriverName').val(),
            VehicleNumber: $('#VehicleNumber').val(),
            ValidFromDate: validFromdate,
            ValidToDate: validTodate,
            FromDate: $('#FromDate').val(),
            ToDate: $('#ToDate').val(),
            TransNo: $('#TransNo').val(),
            Items: self.get_items()
        }
        return model;
    },
    get_items: function () {
        var Items = [];
        $('#TransportPermitList tbody tr.included').each(function (i, record) {
            var row = $(this).closest('tr');
            item = {

                TransNo: $(row).find(".TransNo").text(),
                TransID: clean($(row).find(".TransID").val()),
                TransDate: $(row).find(".TransDate").text(),
                Type: $(row).find(".Type").text(),
                CustomerID: clean($(row).find(".CustomerID").val()),
                LocationID: clean($(row).find(".LocationID").val()),
                DistrictID: clean($(row).find(".DistrictID").val()),
                ItemID: clean($(row).find(".ItemID").val()),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                Quantity: clean($(row).find(".Quantity").val()),
            };
            Items.push(item);
        });
        return Items
    },

    validate_form: function () {
        var self = TransportPermitReport;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_show: function () {
        var self = TransportPermitReport;
        if (self.rules.on_show.length > 0) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },
    rules: {
        on_submit: [
            {
                elements: "#item-count",
                rules: [
                    { type: form.required, message: "Please add atleast one item" },
                    { type: form.non_zero, message: "Please add atleast one item" },
                ]
            },
            {
                elements: "#DriverName",
                rules: [
                    { type: form.required, message: "Please enter driver name" },
                ]
            },
            {
                elements: "#VehicleNumber",
                rules: [
                    { type: form.required, message: "Please enter vehicle number" },
                ]
            },
             {
                 elements: "#ValidToDate",
                 rules: [
                      { type: form.required, message: "valid to date is required" },
             {
                 type: function (element) {
                     var u_date = $(element).val().split('-');
                     var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                     var a = Date.parse(used_date);
                     var po_date = $('#ValidFromDate').val().split('-');
                     var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                     var date = Date.parse(po_datesplit);
                     return date <= a
                 }, message: "Valid to date must be a date after valid from date"
             }

                 ]
             },
             {
                 elements: "#ValidFromDate",
                 rules: [
                      { type: form.required, message: "valid from date is required" }

                 ]
             },
        ],
        on_show: [

           {
               elements: "#ToDate",
               rules: [
                    { type: form.required, message: "valid to date is required" },
           {
               type: function (element) {
                   var u_date = $(element).val().split('-');
                   var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
                   var a = Date.parse(used_date);
                   var po_date = $('#FromDate').val().split('-');
                   var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
                   var date = Date.parse(po_datesplit);
                   return date <= a
               }, message: "To date must be equal to or greater than from date"
           }

               ]
           },
           {
               elements: "#FromDate",
               rules: [
                    { type: form.required, message: "From date is required" }

               ]
           },
        ],

    },
}










