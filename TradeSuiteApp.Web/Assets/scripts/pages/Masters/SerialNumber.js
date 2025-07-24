var fh;
serialNumber = {
    init: function () {
        var self = serialNumber;
        self.bind_events();
        fh = $("#serial-number-list").FreezeHeader();
    },
    list: function () {
        $serial_number_list = $('#serial-number-list');
        if ($serial_number_list.length) {
            $serial_number_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/SerialNumber/GetSerialNumberList";
            var list_table = $serial_number_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST"
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Form", "className": "Form" },
                    { "data": "Prefix", "className": "Prefix" },
                    { "data": "LocationPrefix", "className": "LocationPrefix" },
                    { "data": "FinYearPrefix", "className": "FinYearPrefix" },
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $serial_number_list.trigger("datatable.changed");
                },
            });
            $serial_number_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $serial_number_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $serial_number_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        $('.btnUpdate').on('click', serialNumber.update_finyear_prefix);

    },

    update_finyear_prefix: function () {
        var self = serialNumber;
           var modal = self.get_update_data();
        $.ajax({
            url: '/Masters/SerialNumber/UpdateFinYearAndFinPrefix',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Serial number  Updated successfully.");
                    setTimeout(function () {
                        window.location = "/Masters/SerialNumber/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update Financial Year and Prefix .");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });

    },

    get_update_data: function () {
        var self = serialNumber;
        var modal = {
            NewFinYear: $("#NewFinYear").val(),
            NewFinPrefix: $("#NewFinPrefix").val(),
            Trans: self.get_list(),
        };
        return modal;
    },

    get_list: function () {
        var item = [];
        $('#serial-number-list tbody tr').each(function (i, record) {
            var ismaster = clean($(this).find(".IsMaster").val()) == 1 ? true : false
            item.push({
                ID: clean($(this).find(".ID").val()),
                FormName: $(this).find(".FormName").text().trim(),
                Field: $(this).find(".Field").text(),
                LocationID: $(this).find(".LocationID").val(),
                Prefix: $(this).find(".Prefix").val(),
                SpecialPrefix: $(this).find(".SpecialPrefix").val(),
                FinYearPrefix: $(this).find(".FinYearPrefix").val(),
                Value: $(this).find(".Value").text(),
                IsLeadingZero: $(this).find(".IsLeadingZero").text(),
                NumberOfDigits: $(this).find(".NumberOfDigits").text(),
                FinYear: $(this).find(".NewFinYear").text(),
                Suffix: $(this).find(".Suffix").text(),
                IsMaster: ismaster
            });
        })

        return item;
    },

    details: function () {
        var self = serialNumber;
        self.details_trans_list();
    },

    details_trans_list: function () {
        $scheme_list = $('#serial-number-list');

        if ($scheme_list.length) {
            $scheme_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($scheme_list);
            var url = "/Masters/SerialNumber/GetFinYearDetails";
            var list_table = $scheme_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [

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
                            return meta.settings.oAjaxData.start + meta.row + 1 + "<input type='hidden' class='ID' value='" + row.ID + "' >";
                        }
                    },
                    { "data": "Form", "className": "FormName" },
                    { "data": "Field", "className": "Field" },
                    { "data": "LocationPrefix", "className": "LocationPrefix" },
                     {
                         "data": "Prefix",
                         "className": "",
                         "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<div ><td><input type='text'  class='md-input Prefix' value='" + row.Prefix + "' /></td></div>";
                         }

                     },

                    {
                        "data": "SpecialPrefix",
                        "className": "",
                        "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div ><td><input type='text'  class='md-input SpecialPrefix' value='" + row.SpecialPrefix + "' /></td></div>";
                        }

                    },
                    {
                        "data": "FinYearPrefix",
                        "className": "",
                        "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div ><td><input type='text'  class='md-input FinYearPrefix' value='" + row.FinYearPrefix + "' /></td></div>";
                        }

                    },
                    { "data": "Value", "className": "Value" },
                    { "data": "IsLeadingZero", "className": "IsLeadingZero" },
                    { "data": "NoOfDigits", "className": "NoOfDigits" },
                    { "data": "FinYear", "className": "FinYear" },
                    { "data": "Location", "className": "Location" },
                    { "data": "Suffix", "className": "Suffix" },

                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $scheme_list.trigger("datatable.changed");
                },
            });
            $scheme_list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $scheme_list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $scheme_list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
}
