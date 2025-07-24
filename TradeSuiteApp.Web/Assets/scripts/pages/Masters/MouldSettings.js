var freeze_header;
MouldSettings = {
    init: function () {
        var self = MouldSettings;
        freeze_header = $("#mouldsettings-list").FreezeHeader();
        self. show_hide_btn();
        self.mould_list();
        select_mould_table = $('#mould-list').SelectTable({
            selectFunction: self.select_mould,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-mould",
            initiatingElement: "#MouldName",
            selectionType: "radio"
        });
        self.add_mouldsettings();
        self.bind_events();
    },

    list: function () {
        var $list = $('#mouldsettings-Machine-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }

    },

    bind_events: function () {
        var self = MouldSettings;
        $.UIkit.autocomplete($('#Mould-autocomplete'), { 'source': self.get_mould, 'minLength': 1 });
        $('#Mould-autocomplete').on('selectitem.uk.autocomplete', self.set_mould_details);
        $("body").on('click', '.btnremove', self.save_remove_confirm);
        $("body").on('click', '.btnadd', self.save_add_confirm);
        $("#btnOKMould").on("click", self.select_mould);
        $("body").on('click', '#mouldsettings-Machine-list tbody tr', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Masters/MouldSettings/Create/' + Id;
        });
    },

    save_add_confirm: function () {
        var self = MouldSettings;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.add_mould();
        }, function () {
        })
    },

    save_remove_confirm: function () {
        var self = MouldSettings;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.remove_mould();
        }, function () {
        })
    },

    select_mould: function () {
        self = MouldSettings;
        var radio = $('#mould-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Code = $(row).find(".Code").val();
        var MouldName = $(row).find(".MouldName").text().trim();
        $("#MouldName").val(MouldName);
        $("#MouldID").val(ID);
        UIkit.modal($('#select-mould')).hide();
    },

    mould_list: function () {
        var self = MouldSettings;
        var $list = $('#mould-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Mould/GetMouldList";

            var list_table = $list.dataTable({
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
                            
                        ];
                    }
                },
                "columns": [
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
                            return "<input type='radio' class='uk-radio ID' name='ID' data-md-icheck value='" + row.ID + "' >"
                            
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "MouldName", "className": "MouldName" },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "MachineName", "className": "MachineName" },
                ]
                ,
                createdRow: function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            return list_table;
        }
    },

    get_mould: function (release) {
        $.ajax({
            url: '/Masters/Mould/GetMouldListForAutoComplete',
            data: {
                term: $('#MouldName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_mould_details: function (event, item) {   // on select auto complete mould
        self = MouldSettings;
        $("#MouldName").val(item.value);
        $("#MouldID").val(item.id);
    },

    remove_mould: function () {
        var self = MouldSettings;
        var data = self.get_data();
        data.MouldID = 0;
        self.clear_data();
        $.ajax({
            url: '/Masters/MouldSettings/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    self.show_hide_btn();
                    $('#mouldsettings-list tbody tr').remove();
                    self.add_mouldsettings();
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },

    add_mould: function () {
        var self = MouldSettings;
        var location = "/Masters/MouldSettings/Index";
        var data = self.get_data();
        $.ajax({
            url: '/Masters/MouldSettings/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },

    clear_data:function(){
        var self = MouldSettings;
        $("#MouldID").val('');
        $("#MouldName").val('');
        $("#SettingTime").val('');
        $("#Reason").val('');
    },

    show_hide_btn: function () {
        var self = MouldSettings;
        $(".btnremove").hide();
        $(".btnadd").hide();
        $(".add").hide();
        $(".remove").hide();
        var val=$("#MouldID").val()
        if (val > 0)
        {
            $(".btnremove").show();
            $(".remove").show();
        }
        else {
            $(".btnadd").show();
            $(".add").show();
        }
    },

    get_data: function () {
        var self = MouldSettings;
        var data = {};
        data.ID = $("#ID").val();
        data.MouldID = $("#MouldID").val();
        data.SettingTime = $("#SettingTime").val();
        data.Reason = $("#Reason").val();
        return data;
    },

    validate_form: function () {
        var self = MouldSettings;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
           {
               elements: "#MouldID",
               rules: [
                   { type: form.required, message: "Please choose an MouldName", alt_element: "#MouldName" },
                   { type: form.positive, message: "Please choose an MouldName", alt_element: "#MouldName" },
                   { type: form.non_zero, message: "Please choose an MouldName", alt_element: "#MouldName" },
                  
               ],
           },
            {
                elements: "#Reason",
                rules: [
                    { type: form.required, message: "Please enter Reason" },
                ],
            },
            {
                elements: "#SettingTime",
                rules: [
                    { type: form.required, message: "Please enter SettingTime" },
                ],
            },
        ],
    },

    add_mouldsettings: function () {
        var self = MouldSettings;
        var ID = $('#ID').val();
        $.ajax({
            url: '/Masters/MouldSettings/GetMouldSettingsByMachine',
            dataType: "json",
            data: {
                ID: ID,
            },
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td>' + (i + 1)
                        + '</td>'
                        + '<td>' + item.Date + '</td>'
                        + '<td>' + item.SettingTime + '</td>'
                        + '<td>' + item.Mould + '</td>'
                        + '<td>' + item.AddorRemove + '</td>'
                        + '<td>' + item.Reason + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $('#mouldsettings-list tbody').append($content);
                //if (response.Status == "success") {
                //    $(response.Data).each(function (i, record) {
                //        content = '<tr>'
                //        + '<td>' + (i + 1)
                //        + '</td>'
                //        + '<td>' + record. Date + '</td>'
                //        + '<td>' + record.SettingTime + '</td>'
                //        + '<td>' + record.Mould + '</td>'
                //        + '<td>' + record.AddorRemove + '</td>'
                //        + '<td>' + record.Reason + '</td>'
                //        + '<tr>';
                //    });
                //    $content = $(content);
                //    app.format($content);
                //    $('#mouldsettings-list tbody').append($content);
                //}
            }
        });
    },
}