$(function () {
    taxtype.taxtype_list();
    taxtype.bind_events();
    taxtype_list = taxtype.location_list();
    //item_select_table = $('#taxtype-list').SelectTable({
    //    selectFunction: self.select_item,
    //    returnFocus: "#txtRequiredQty",
    //    modal: "#select-item",
    //    initiatingElement: "#ItemName"
    //});
});
var taxtype = {
    taxtype_list: function () {
        $taxtype_list = $('#taxtype-list');
        if ($taxtype_list.length) {
            $taxtype_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#taxtype-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Taxtype/Edit/' + Id;
            });
            altair_md.inputs();
            var Taxtype_list_table = $taxtype_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            Taxtype_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    Taxtype_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        $(".btnUpdate").on('click', taxtype.update);
        $(".btnSave").on('click', taxtype.save_confirm);
        $("body").on("click", "#btnOKItem", taxtype.select_item);
    },
    select_item: function () {
       // var self = currency;
        var radio = $('#location_list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
      
        $("#LocationName").val(Name);
        $("#LocationID").val(ID);
        $("#LocationCode").val(Code);
        $("#Description").focus();
       // self.is_finished_good(Category);
        //self.clear_item_select();
    },
    location_list: function () {
        var $list = $('#location_list');//item-list
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Location/GetLocationList";

            var location_list_table = $list.dataTable({
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
                            { Key: "LocationID", Value: $('#LocationID').val() },
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
                            return "<input type='radio' class='uk-radio LocationID' name='LocationID' data-md-icheck value='" + row.ID + "' >"
                                + "<input type='hidden' class='LocationName' value='" + row.Name + "'>"
                              ;
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
                location_list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                location_list_table.api().page('next').draw('page');
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
                location_list_table.api().column(index).search(this.value).draw();
            });
            return location_list_table;
        }
    },
    save_confirm: function () {
        var self = taxtype;
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
        var self = taxtype;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Taxtype/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Taxtype created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Taxtype/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    update: function () {
        var self = taxtype;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Taxtype/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Currency updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Taxtype/Index";
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
            Name: $("#Name").val(),
            Description: $("#Description").val(),
            LocationName: $("#LocationName").val(),
            LocationID: $("#LocationID").val(),
           
        }
        return model;
    },
    validate_form: function () {
        var self = taxtype;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
           
            {
                elements: "#Name",
                rules: [
                    { type: form.required, message: "Name is required" },
                ]
            },
            {
                elements: "#LocationID",
                rules: [
                    { type: form.required, message: "Location is required" },
                ]
            },
            
        ]
    }

};