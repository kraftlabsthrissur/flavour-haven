$(function () {
    currency.currency_list();
    currency.bind_events();
    country_list = currency.country_list();
    //item_select_table = $('#currency-list').SelectTable({
    //    selectFunction: self.select_item,
    //    returnFocus: "#txtRequiredQty",
    //    modal: "#select-item",
    //    initiatingElement: "#ItemName"
    //});
});
var currency = {
    currency_list: function () {
        $currency_list = $('#currency-list');
        if ($currency_list.length) {
            $currency_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#currency-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Currency/Details/' + Id;
            });
            altair_md.inputs();
            var currency_list_table = $currency_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            currency_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    currency_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click', currency.update);
        $(".btnSave").on('click', currency.save_confirm);
        $("body").on("click", "#btnOKItem", currency.select_item);
    },
    select_item: function () {
        // var self = currency;
        var radio = $('#country_list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();

        $("#CountryName").val(Name);
        $("#CountryID").val(ID);
        $("#CountryCode").val(Code);
        $("#Description").focus();
        // self.is_finished_good(Category);
        //self.clear_item_select();
    },
    country_list: function () {
        var $list = $('#country_list');//item-list
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Country/GetCountriesList";

            var country_list_table = $list.dataTable({
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
                            { Key: "CountryID", Value: $('#CountryID').val() },
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
                            return "<input type='radio' class='uk-radio CountryID' name='CountryID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='CountryName' value='" + row.Name + "'>"
                                + "<input type='hidden' class='CountryCode' value='" + row.Code + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" }
                ],
                createdRow: function (row, data, index) {
                    app.format(row);
                },

                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                country_list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                country_list_table.api().page('next').draw('page');
            });
            $('body').on("change", '#ItemCategory', function () {
                //country_list_table.fnDraw();
            });
            $('body').on("change", '#ItemCategoryID', function () {
                //country_list_table.fnDraw();
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                country_list_table.api().column(index).search(this.value).draw();
            });
            return country_list_table;
        }
    },
    save_confirm: function () {
        var self = currency;
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
        var self = currency;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Currency/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Currency created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Currency/Index";
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
            url: '/Masters/Currency/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Currency updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Currency/Index";
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
            Id: clean($("#Id").val()),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Description: $("#Description").val(),
            CountryName: $("#CountryName").val(),
            CountryID: clean($("#CountryID").val()),
            DecimalPlaces: clean($("#DecimalPlaces").val()),
            MinimumCurrency: $("#MinimumCurrency").val(),
            MinimumCurrencyCode: $("#MinimumCurrencyCode").val()
        }
        return model;
    },
    validate_form: function () {
        var self = currency;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Code is required" },
                ]
            },
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Name is required" },
                ]
            },
            {
                elements: "#CountryID",
                rules: [
                    { type: form.required, message: "Country is required" },
                ]
            },
            {
                elements: "#DecimalPlaces",
                rules: [
                    { type: form.required, message: "Country is required" },
                    { type: form.non_zero, message: "Invalid Number" },
                    { type: form.positive, message: "Number should be positive" },
                ]
            },

        ]
    }

};