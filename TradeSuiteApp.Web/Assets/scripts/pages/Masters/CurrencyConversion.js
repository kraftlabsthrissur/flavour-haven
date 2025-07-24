$(function () {
    currencyConvertion.currencyConvertion_list();
    basecurrencyList = currencyConvertion.basecurrency_list();
    conversioncurrencyList = currencyConvertion.conversioncurrency_list();
    currencyConvertion.bind_events();
});
var currencyConvertion = {
    currencyConvertion_list: function () {
        $currencyConvertion_list = $('#currencyConvertion-list');
        if ($currencyConvertion_list.length) {
            $currencyConvertion_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#currencyConvertion-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/CurrencyConversion/Details/' + Id;
            });
            altair_md.inputs();
            var currencyConvertion_list_table = $currencyConvertion_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            currencyConvertion_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    currencyConvertion_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click', currencyConvertion.update);
        $(".btnSave").on('click', currencyConvertion.save_confirm);
        $("body").on("click", "#btnOKBaseCurrency", currencyConvertion.select_base_currency);
        $("body").on("click", "#btnOKConversionCurrency", currencyConvertion.select_conversion_currency);
    },
    select_base_currency: function () {
        // var self = currency;
        var radio = $('#Base_Currency_List tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Code = $(row).find(".Code").text().trim();

        $("#BaseCurrencyID").val(ID);
        $("#BaseCurrencyCode").val(Code);
        $("span.BaseCurrency").text(Code);

    },
    select_conversion_currency: function () {
        // var self = currency;
        var radio = $('#Conversion_Currency_List tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Code = $(row).find(".Code").text().trim();

        $("#ConversionCurrencyID").val(ID);
        $("#ConversionCurrencyCode").val(Code);
        $("span.ConversionCurrency").text(Code);
    },
    basecurrency_list: function () {
        var $baselist = $('#Base_Currency_List');//item-list
        if ($baselist.length) {
            $baselist.find('thead.search th').each(function () {

                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Currency/GetBaseCurrenciesList";

            var baseCurrency_list_table = $baselist.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CurrencyID", Value: $('#CurrencyID').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio CurrencyID' name='CurrencyID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.Code + "'>";
                            + "<input type='hidden' class='CurrencyName' value='" + row.Name + "'>"
                                + "<input type='hidden' class='CountryName' value='" + row.CountryName + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "CountryName", "className": "CountryName" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $baselist.trigger("datatable.changed");
                },
            });
            $baselist.on('previous.page', function () {
                baseCurrency_list_table.api().page('previous').draw('page');
            });
            $baselist.on('next.page', function () {
                baseCurrency_list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                //baseCurrency_list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                //baseCurrency_list_table.fnDraw();
            });
            $baselist.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                baseCurrency_list_table.api().column(index).search(this.value).draw();
            });
            return baseCurrency_list_table;
        }
    },
    conversioncurrency_list: function () {
        var $conversionlist = $('#Conversion_Currency_List');//item-list
        if ($conversionlist.length) {
            $conversionlist.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Currency/GetConversionCurrenciesList";

            var converioncurrency_list_table = $conversionlist.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CurrencyID", Value: $('#CurrencyID').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio CurrencyID' name='CurrencyID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='CurrencyCode' value='" + row.Code + "'>";
                            + "<input type='hidden' class='CurrencyName' value='" + row.Name + "'>"
                                + "<input type='hidden' class='CountryName' value='" + row.CountryName + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "CountryName", "className": "CountryName" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $conversionlist.trigger("datatable.changed");
                },
            });
            $conversionlist.on('previous.page', function () {
                converioncurrency_list_table.api().page('previous').draw('page');
            });
            $conversionlist.on('next.page', function () {
                converioncurrency_list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                //converioncurrency_list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                //converioncurrency_list_table.fnDraw();
            });
            $conversionlist.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                converioncurrency_list_table.api().column(index).search(this.value).draw();
            });
            return converioncurrency_list_table;
        }
    },
    save_confirm: function () {
        var self = currencyConvertion;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    error_count: 0,
    save: function () {
        var self = currencyConvertion;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/CurrencyConversion/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Currency created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/CurrencyConversion/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    update: function () {
        var self = currency;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/CurrencyConversion/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Currency updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/urrencyConversion/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'visibility': 'visible' });
                }
            },
        });
    },


    get_data: function () {
        var model = {
            Id: $("#Id").val(),
            BaseCurrencyCode: $("#BaseCurrencyCode").val(),
            BaseCurrencyID: $("#BaseCurrencyID").val(),
            ConversionCurrencyCode: $("#ConversionCurrencyCode").val(),
            ConversionCurrencyID: $("#ConversionCurrencyID").val(),
            FromDate: $("#FromDate").val(),
            Description: $('#Description').val(),
            ExchangeRate: clean($('#ExchangeRate').val()),
            InverseExchangeRate: clean($('#InverseExchangeRate').val())
        }
        return model;
    },
    validate_form: function () {
        var self = currencyConvertion;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#BaseCurrencyID",
                rules: [
                    { type: form.required, message: "Base Currency is required" },
                ]
            },
            {
                elements: "#ConversionCurrencyID",
                rules: [
                    { type: form.required, message: "Conversion Currency is required" },
                ]
            },
            {
                elements: "#ExchangeRate",
                rules: [
                    { type: form.required, message: "Exchange Rate is required" },
                ]
            },
            {
                elements: "#InverseExchangeRate",
                rules: [
                    { type: form.required, message: "Inverse Rate is required" },
                ]
            },
            {
                elements: "#FromDate",
                rules: [
                    { type: form.required, message: "FromDate is required" },
                ]
            }

        ]
    }

};