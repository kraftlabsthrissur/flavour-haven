LaboratoryTest = {
    init: function () {
        var self = LaboratoryTest;
        self.bind_events();
        self.enable_Item();
        self.lablist();
        item_select_table = $('#labtest-list').SelectTable({
            selectFunction: self.select_test,
            returnFocus: "#LabTest",
            modal: "#select-labtest",
            initiatingElement: "#LabTest",
            startFocusIndex: 5
        });
        index = $("#lab-items-list tbody tr").length;
        $("#item-count").val(index);
    },
    select_test: function () {
        var self = LaboratoryTest;
        var radio = $('#select-labtest tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#LabTest").val(Name);
        $("#LabTestID").val(ID);
        UIkit.modal($('#select-labtest')).hide();

    },
    list: function () {
        var $list = $('#laboratory_test_list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#laboratory_test_list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Masters/LaboratoryTest/Details/' + id;
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
    lablist: function () {
        var $list = $('#labtest-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/LaboratoryTest/GetLabTestList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[1, "desc"]],
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
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio check-box ItemID chk-lab-test'name='ItemID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                   { "data": "Code", "className": "Code" },
                   { "data": "Type", "className": "Type" },
                   { "data": "GroupName", "className": "GroupName" },
                   { "data": "Name", "className": "Name" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = LaboratoryTest;
        $(".btnSave").on('click', self.save_confirm);
        $('body').on('click', '#btnAddTest', self.add_item);
        $("body").on("ifChanged", "#IsAlsoGroup", self.enable_Item);
        $("body").on("click", ".remove-item", self.remove_item);
        $.UIkit.autocomplete($('#Labtest-autocomplete'), { 'source': self.get_Tests, 'minLength': 1 });
        $('#Labtest-autocomplete').on('selectitem.uk.autocomplete', self.set_Tests);
        $('body').on('click', '#btn_add_labtests', self.select_test);
        
    },
    get_Tests: function (release) {
        $.ajax({
            url: '/Masters/LaboratoryTest/GetLabTestAutoComplete',
            data: {
                Hint: $('#LabTest').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_Tests: function (event, item) {
        var self = LaboratoryTest;
        $("#Labtest").val(item.value);
        $("#LabTestID").val(item.id);
        
    },
    enable_Item: function () {
        var self = LaboratoryTest;
        if($("#IsAlsoGroup").is(":checked"))
        {
            $("#IsAlsoGroup").val(true);
            $(".lab-group").show();
            $("#Rate").removeAttr("disabled");

        } else {
            $("#IsAlsoGroup").val(false);
            $(".lab-group").hide();
            $("#Rate").val(0);
            $("#Rate").attr("disabled", "disabled");
            $('#lab-items-list tbody').empty();
            $('#item-count').val(0);
        }

    },
    add_item: function () {
        var self = LaboratoryTest;
        var ItemID = $("#LabTestID").val();
        var ItemName = $("#LabTest").val();
        if (self.validate_Add() > 0) {
            return;
        }
        var content = "";
        var tr = "";
        var $tr;
        var sino = "";
        sino = $('#lab-items-list tbody tr').length + 1;
        tr += "<tr>"
        content = '<tr>'
        tr += '<td class="uk-text-center serial-no">' + sino
        tr += '<input type="hidden" class = "LabTestID" value="' + ItemID + '" />'
           + '</td>'
        tr += '<td class="LabTestID">' + ItemName
        tr += '<td>'
        tr += '<a class="remove-item">'
        tr += '<i class="uk-icon-remove"></i>'
        tr += '</a>'
        tr += '</td>'
        tr += '</tr>';
        $tr = $(tr);
        app.format($tr);
        $('#lab-items-list tbody').append($tr);
        index = $("#lab-items-list tbody tr").length;
        $("#item-count").val(index);
        self.clear();
        $("#LabTest").focus();
    },
    clear: function () {
        $("#LabTestID").val('');
        $("#LabTest").val('');
    },
    save_confirm: function () {
        var self = LaboratoryTest;
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
    save: function () {
        var self = LaboratoryTest;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/LaboratoryTest/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Laboratory Test created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/LaboratoryTest/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            },
        })
    },
    remove_item: function () {
        var self = LaboratoryTest;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#lab-items-list tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#lab-items-list tbody tr").length);       
    },

    get_data: function () {
        var self = LaboratoryTest;
        var item = [];
        var data = {};
      
        data.ID= $("#ID").val();
        data.Code= $("#Code").val();
        data.Name= $("#Name").val(),
        data.BiologicalReference= $("#BiologicalReference").val();
        data.Unit= $("#Unit").val();
        data.AddedDate= $("#AddedDate").val();
        data.Description= $("#Description").val();
        data.PurchaseCategoryID= $("#PurchaseCategoryID").val();
        data.QCCategoryID= $("#QCCategoryID").val();
        data.GSTCategoryID= $("#GSTCategoryID").val();
        data.SalesCategoryID= $("#SalesCategoryID").val();
        data.SalesIncentiveCategoryID= $("#SalesIncentiveCategoryID").val();
        data.StorageCategoryID= $("#StorageCategoryID").val();
        data.ItemTypeID= $("#ItemTypeID").val();
        data.AccountsCategoryID= $("#AccountsCategoryID").val();
        data.BusinessCategoryID= $("#BusinessCategoryID").val();
        data.ItemUnitID= $("#ItemUnitID").val();
        data.CategoryID= $("#CategoryID").val();
        data.Method= $("#Method").val();
        data.SpecimenID= $("#SpecimenID").val();
        data.Rate = clean($("#Rate").val());
        data.IsAlsoGroup = $("#IsAlsoGroup").val();
        data.Items = [];
        var item = {};
        $('#lab-items-list tbody tr').each(function () {
            item = {};
            item.LabtestID = $(this).find(".LabTestID").val();
            data.Items.push(item);
        });
        return data;
    },
    validate_form: function () {
        var self = LaboratoryTest;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_Add: function () {
        var self = LaboratoryTest;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    rules: {
        on_submit: [
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: "Name is Required" },
                 ]
             },

             {
                 elements: "#BiologicalReference",
                 rules: [
                      { type: form.required, message: "BiologicalReference is Required" },
                 ]
             },
             {
                 elements: "#Unit",
                 rules: [
                     { type: form.required, message: "Unit  is Required" },
                 ]
             },
             {
                 elements: "#item-count",
                 rules: [
                     {
                         type: function (element) {
                             if ($("#IsAlsoGroup").is(":checked")) {
                                 var error = false;
                                 if ($('#item-count').val() == 0) {
                                     error = true;
                                 }
                             }     
                             return !error;
                         }, message: "Please add Lab test"
                     },
                 ],
             },
        ],
        on_add: [
             {
                 elements: "#LabTestID",
                 rules: [
                     { type: form.required, message: "Lab Test is Required" },
                     { type: form.non_zero, message: "Lab Test is Required", alt_element: "#ItemName" },
                     {
                         type: function (element) {
                             var ItemID = clean($("#LabTestID").val());
                             var error = false;
                             $('#lab-items-list tbody tr').each(function () {
                                 if ($(this).find('.LabTestID').val() == ItemID) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "Lab test already added"
                     },
                 ]
             },
        ]
    },
}