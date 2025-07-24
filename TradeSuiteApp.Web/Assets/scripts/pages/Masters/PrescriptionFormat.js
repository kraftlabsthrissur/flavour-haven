PrescriptionFormat = {
    init: function ()
    {
        var self = PrescriptionFormat;
        //self.check();
        self.bind_events();
    },
    list: function () {
        var $list = $('#PrescriptionFormat-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#PrescriptionFormat-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/PrescriptionTemplate/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
   
    bind_events: function ()
    {
        $("body").on("click", "#btnAdd", PrescriptionFormat.add_item);
        $("body").on("click", ".remove-item", PrescriptionFormat.delete_item);
        $(".btnSave").on("click", PrescriptionFormat.save_confirm);
        $("#btnView").on("click", PrescriptionFormat.filter_items);
        $("body").on("ifChanged", ".check-box", PrescriptionFormat.check);
        $('#MedicineCategory').on('change', PrescriptionFormat.get_item);
    },
    add_item:function()
    {
        var self = PrescriptionFormat;
        if (self.validate_on_add() > 0) {
            return;
        }
        sino = $('#PrescriptionFormatlist tbody tr').length + 1;
        var Prescription = $("#Prescription").val();
        var MedicineCategory = $("#MedicineCategory  option:selected").text();
        var MedicineCategoryID = $("#MedicineCategory  option:selected").val();
        var content = "";
        var $content;
        content += '<tr class="included">'
                          + '<td class="serial-no uk-text-center">' + sino + '</td>'
                          + '<td class="td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck checked class="md-input check-box"/>' + '</td>'
                          + '<td class="MedicineCategory">' + MedicineCategory
                          + '<input type="hidden" class="MedicineCategoryID" value="' + MedicineCategoryID + '"/>'
                          + '</td>'
                          + '<td>' + '<input type="text" value=" ' + Prescription + '" class="md-input Prescription" /> ' + '</td>'
                          + '<td>'
                          + '<a class="remove-item">'
                          + '<i class="uk-icon-remove"></i>'
                          + '</a>'
                          + '</td>'
                          + '</tr>';
        $content = $(content);
        app.format($content);
        $('#PrescriptionFormatlist tbody').append($content);
        self.count();
        self.clear_data();
    },
    save_confirm: function () {
        var self = PrescriptionFormat;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save: function () {
        var self = PrescriptionFormat;
        var data;
        //index = $("#PrescriptionFormatlist tbody  .included").length;
        //$("#item-count").val(index);
        data = self.get_data();
        $.ajax({
            url: '/Masters/PrescriptionTemplate/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/PrescriptionTemplate/Index";
                }
                else {
                    app.show_error('Failed to create Prescription Format Definition');
                }
            }
        });
    },
    get_data: function () {
        var self = PrescriptionFormat;
        var data = {};
        //data.Prescription = $("#Prescription").val(),
        data.MedicineCategory = $("#MedicineCategory").val()
        data.Items = [];
        var item = {};
        $('#PrescriptionFormatlist tbody .included').each(function () {
            item = {};
            item.MedicineCategoryID = $(this).find(".MedicineCategoryID").val();
            item.Prescription = $(this).find(".Prescription").val();
            data.Items.push(item);
        });
        return data;
    },

    get_item: function () {
        var self = PrescriptionFormat;
        var length;
        if ($('#PrescriptionFormatlist tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                self.filter_items();
            });
        }
        else
        {
            self.filter_items();
        }
    },

    filter_items:function() {
        var self = PrescriptionFormat;
        var length;
        var ID = $("#MedicineCategory").val()
        $.ajax({
            url: '/Masters/PrescriptionTemplate/GetPrescriptionFormatItemList',
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
                    content += '<tr class="included">'
                        + '<td class="serial-no uk-text-center">' + slno + '</td>'
                        + '<td class="td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck class="md-input check-box" checked/>' + '</td>'
                        + '<td class="MedicineCategory">' + item.MedicineCategory
                        + '<input type="hidden" class="MedicineCategoryID"value="' + item.MedicineCategoryID + '"/>'
                        + '</td>'
                        + '<td>' + '<input type="text" value=" ' + item.Prescription + '" class="md-input Prescription" /> ' + '</td>'
                        + '<td>'
                        + '<a class="remove-item">'
                        + '<i class="uk-icon-remove"></i>'
                        + '</a>'
                        + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#PrescriptionFormatlist tbody ").html($content);
                index = $("#PrescriptionFormatlist tbody tr.included").length;
                $("#item-count").val(index);
            },
            
        });
    },

    check: function () {
        var self = PrescriptionFormat;
        $("#PrescriptionFormatlist tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
                $(row).find(".Prescription").prop("disabled", false);

            } else {
                $(row).find(".Prescription").prop("disabled", true);
                $(row).removeClass('included');
            }
        });
        self.count();
    },

    count: function () {
        var self = PrescriptionFormat;
        index = $("#PrescriptionFormatlist tbody tr.included").length;
        $("#item-count").val(index);
    },
    clear_data: function () {
        var self = PrescriptionFormat;
        $("#Prescription").val('');
    },
    validate_on_add: function () {
        var self = PrescriptionFormat;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_save: function () {
        var self = PrescriptionFormat;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    delete_item: function () {
        var self = PrescriptionFormat;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#PrescriptionFormatlist tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#PrescriptionFormatlist tbody tr").length);

    },
    rules: {
        on_add: [
            {
                elements: "#Prescription",
                rules: [
                    { type: form.required, message: "Please Add Prescription" },
                    { type: form.non_zero, message: "Please Add Prescription" },
                    {
                        type: function (element) {
                            var error = false;
                            $("#PrescriptionFormatlist tbody tr").each(function () {
                                if ($(this).find(".Prescription").val() == $(element).val()) {
                                    error = true;
                                }
                            });
                            return !error;
                        }, message: "Prescription already exists"
                    },
                ]
            },
        ],
        on_save: [
           {
               elements: "#item-count",
               rules: [
                   { type: form.non_zero, message: "Please add atleast one item" },
                  { type: form.required, message: "Please add atleast one item" },

               ]
           },
        ]
    }
}