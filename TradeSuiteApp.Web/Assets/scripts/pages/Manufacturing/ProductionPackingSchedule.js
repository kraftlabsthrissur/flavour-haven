var freeze_header;
ProductionPackingSchedule = {
    init: function () {
        var self = ProductionPackingSchedule;
        freeze_header = $("#production-schedule-item-list").FreezeHeader({ height: 250 });
        Item.production_packing_item_list();
        $('#production-group-list').SelectTable({
            selectFunction: self.select_production_group,
            returnFocus: "#BatchID",
            modal: "#select-production-group",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        self.bind_events();
        self.freeze_headers();
        self.count_items();
    },

    freeze_headers: function () {
        fh_material = $("#production-packing-material-list").FreezeHeader({ height: 200 });

    },

    list: function () {
        var self = ProductionPackingSchedule;
        $('#packing-schedule').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "scheduled":
                $list = $('#scheduled-list');
                break;
            default:
                $list = $('#draft-list');
        }
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Manufacturing/ProductionPackingSchedule/GetPackingScheduleForDataTable?type=" + type;
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>"

                       }
                   },

                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "BatchType", "className": "BatchType" },
                   {
                       "data": "PackedQty", "searchable": false, "className": "PackedQty",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-production-qty' >" + row.PackedQty + "</div>";
                       }
                   },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/ProductionPackingSchedule/Details/" + Id);

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
        var self = ProductionPackingSchedule;
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item);

        $("#btnOKPackingMaterial").on("click", self.select_item);
        

        $('#btnOKProductionGroup').on('click', self.select_production_group);
        $('#btnGetPackingMaterials').on("click", self.get_packing_materials);

        $('.save-as-draft').on('click', self.save);
        $('.save').on('click', self.save_confirm);
    },

    get_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPackingItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item: function (event, item) {
        var self = ProductionPackingSchedule;
        var Name = item.name;
        var ID = item.id;
        var Unit = item.unit;
        var UnitID = item.unitId;
        var Code = item.code;
        var ProductID = item.productId;
        var ProductGroupID = item.productGroupId
        var BatchSize = item.batchSize;

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#UOM").val(Unit);
        $("#ItemCode").val(Code);
        $("#ProductID").val(ProductID);
        $("#ProductGroupID").val(ProductGroupID);
        $("#BatchSize").val(BatchSize);
        $("#UnitID").val(UnitID);
        if ($("#production-packing-material-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#production-packing-material-list tbody').empty();
                $("#btnGetPackingMaterials").css({ 'display': 'block' });

            })
        }
        self.get_batches_with_stock();
    },

    get_batches_with_stock: function () {
        var ItemID = $('#ProductID').val();
        var DefaultPackingStoreID = $('#DefaultPackingStoreID').val();
        $('#BatchID').html('');
        var options = "<option value='0'>Select</option>";
        $.ajax({
            url: '/Masters/Batch/GetBatch/',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: ItemID,
                StoreID: DefaultPackingStoreID
            },
            success: function (response) {
                if (response.Status == 'success') {
                    $.each(response.data, function (i, record) {
                        options += "<option value='" + record.ID + "'>" + record.BatchNo + "</option>";
                    });
                }
                $('#BatchID').html(options);
            }
        });
        $('#production-packing-output-list tbody').empty();
    },

    select_production_group: function () {
        var self = ProductionPackingSchedule;
        var radio = $('#select-production-group tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".Unit").text().trim();
        var UnitID = $(row).find(".UnitID").val();
        var ProductId = $(row).find(".ProductID").val();
        var BatchSize = $(row).find(".BatchSize").val();
        var ProductionGroupID = $(row).find(".ProductionGroupID").val();

        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#UOM").val(Unit);
        $("#UnitID").val(UnitID);
        $("#ProductID").val(ProductId);
        $("#BatchSize").val(BatchSize);
        $("#ProductGroupID").val(ProductionGroupID);
        if ($("#production-packing-material-list tbody tr").length > 0 || $("#production-packing-process-list tbody tr").length > 0) {
            app.confirm("Selected Items will be removed", function () {
                $('#production-packing-material-list tbody').empty();
                $('#production-packing-process-list tbody').empty();
                $("#btnGetPackingMaterials").css({ 'display': 'block' });

            })
        }
        self.get_batches_with_stock();
        UIkit.modal($('#select-production-group')).hide();
    },

    get_packing_materials: function () {
        var self = ProductionPackingSchedule;
        var tr;
        var serial_no;
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var ItemID = $("#ItemID").val();
        var BatchID = $('#BatchID').val();
        var ProductGroupID = $('#ProductGroupID').val();
        var BatchTypeID = $('#BatchTypeID').val();
        var PackedQty = clean($("#PackedQty").val());
        var BatchType = $('#BatchTypeID option:selected').text();
        var StoreID = $('#DefaultPackingStoreID').val();

        self.get_packing_material();
    },

    get_packing_material: function () {
        var self = ProductionPackingSchedule;
        var ItemID = $("#ItemID").val();
        var BatchID = $('#BatchID').val();
        var ProductGroupID = $('#ProductGroupID').val();
        var BatchTypeID = $('#BatchTypeID').val();
        var PackedQty = clean($("#PackedQty").val());
        var BatchType = $('#BatchTypeID option:selected').text();
        var StoreID = $('#DefaultPackingStoreID').val();

        $.ajax({
            url: '/Manufacturing/ProductionPackingSchedule/GetPackingMaterials',
            data: {
                ItemID: ItemID,
                BatchID: BatchID,
                ProductGroupID: ProductGroupID,
                BatchTypeID: BatchTypeID,
                PackedQty: PackedQty,
                BatchType: BatchType,
                StoreID: StoreID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $response = $(response);
                $response.each(function () {
                    $(this).removeClass("saved").addClass("new");
                });
                app.format($response);
                $("#production-packing-material-list tbody").append($response);
                fh_material.resizeHeader();
                serial_no = $('#production-packing-output-list tbody tr').length + 1;
                tr = '<tr class="new ' + BatchType + '">'
                    + '<td class="serial uk-text-center">'
                    + serial_no
                    + '</td>'
                    + '<td class="Date">' + $("#Date").val() + '</td>'
                    + '<td>' + $("#ItemName").val()
                    + '     <input type="hidden" class="ItemID" value="' + $("#ItemID").val() + '" />'
                    + '     <input type="hidden" class="BatchTypeID" value="' + $("#BatchTypeID").val() + '" />'
                    + '     <input type="hidden" class="BatchID" value="' + $("#BatchID").val() + '" />'
                    + '     <input type="hidden" class="UnitID" value="' + $("#UnitID").val() + '" />'
                    + '     <input type="hidden" class="Status" value="' + 1 + '" /></td>'
                    + '<td class="batchNo">' + $('#BatchID option:selected').text() + '</td>'
                    + '<td>'
                    + '    <input type="text" class="md-input PackedQty mask-production-qty" value="' + clean($('#PackedQty').val()) + '"/>'
                    + '</td>'
                    + '<td class="BatchTypeOutPut"> ' + BatchType
                    + '</td>'
                    + '<td data-md-icheck class="uk-hidden"><input type="checkbox" class="IsQCCompleted" />'
                    + '</td>'
                    + '</tr>';
                $tr = $(tr);
                app.format($tr);
                $('#production-packing-output-list tbody').prepend($tr);
                self.count_items();
                $("#btnGetPackingMaterials").css({ 'display': 'none' });
            }
        });
    },

    count_items: function () {
        var count = $('#production-packing-material-list tbody').length;
        $('#item-count').val(count);
    },

    save_confirm: function () {
        var self = ProductionPackingSchedule;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        else
            {
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
        }
    },

    save: function () {
        var self = ProductionPackingSchedule;
        var data = self.get_data();
        var message = "";
        var url;
        if ($(this).hasClass('save-as-draft')) {
            data.IsDraft = true;
            self.error_count = self.validate_draft();
            if (self.error_count > 0) {
                return;
            }
            message = 'PackingSchedule Saved as Draft';
            url = '/Manufacturing/ProductionPackingSchedule/SaveAsDraft/';
        }
        else {
            data.IsDraft = false;
            data.IsCompleted = false;
            message = 'Packing Saved';
            url = '/Manufacturing/ProductionPackingSchedule/Save/';
        }
        $(".save-as-draft, .save,.complete").css({ 'display': 'none' });
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            data: { Packing: data },
            success: function (response) {
                if (response.Status == 'success') {
                    app.show_notice(message);
                    window.location = '/Manufacturing/ProductionPackingSchedule/Index';
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".save-as-draft, .save,.complete").css({ 'display': 'block' });

                }
            }
        });
    },

    get_data: function () {
        var self = ProductionPackingSchedule;
        var model = {
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            ID: $("#ID").val(),
            ItemID: $("#ItemID").val(),
            ProductID: $("#ProductID").val(),
            ProductGroupID: $("#ProductGroupID").val(),
            BatchID: $("#BatchID").val(),
            BatchTypeID: $("#BatchTypeID").val(),
            PackedQty:$("#PackedQty").val(),
            Remarks: $("#Remarks").val(),
            UnitID: $("#UnitID").val(),
            StartDateStr: $("#StartDate").val(),
            StartDate: $("#StartDate").val()
        };
        model.Materials = [];
        var obj;
        var row;

        $("#production-packing-material-list tbody tr").each(function () {
            row = $(this);
            obj = {
                ItemID: clean($(row).find(".ItemID").val()),
                BatchTypeID: clean($(row).find(".BatchTypeID").val()),
                UnitID: clean($(row).find(".UnitID").val()),
                ActualQty: clean($(row).find(".ActualQty").text()),
                StandardQty: clean($(row).find(".StandardQty").text()),
                IssueQty: clean($(row).find(".IssueQty").val()),
                PackingMaterialMasterID: clean($(row).find(".PackingMaterialMasterID").val()),
                Remarks: $(row).find(".Remarks").val()
            };
            model.Materials.push(obj);
        });
        return model;
    },


    validate_item: function () {
        var self = ProductionPackingSchedule;
        if (self.rules.on_add_item.length > 0) {
            return form.validate(self.rules.on_add_item);
        }
        return 0;
    },

    validate_form: function () {
        var self = ProductionPackingSchedule;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    validate_draft: function () {
        var self = ProductionPackingSchedule;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    

    rules: {
        on_add_item: [
           {
               elements: "#ItemID",
               rules: [
                   { type: form.required, message: "Please choose a valid item" },
                   { type: form.non_zero, message: "Please choose a valid item" },

               ]
           },
           {
               elements: "#BatchID",
               rules: [
                   { type: form.required, message: "Please select Batch" },
                   { type: form.non_zero, message: "Please select Batch" },
                   { type: form.positive, message: "Please select Batch" },
               ]
           },
           {
               elements: "#PackedQty",
               rules: [
                   { type: form.required, message: "Please enter Packed Quantity" },
                   { type: form.non_zero, message: "Please enter Packed Quantity" },
                   { type: form.positive, message: "Please enter Packed Quantity" },
                    {
                        type: function (element) {
                            var error = false;
                            var packedqty = clean($('#PackedQty').val());

                            if (packedqty % 1 != 0)
                                error = true;
                            return !error;
                        }, message: 'Invalid packed quantity'
                    },
               ]
           },


           {
               elements: "#BatchTypeID",
               rules: [
                   { type: form.required, message: "Please select Batch Type" },
                   { type: form.non_zero, message: "Please select Batch Type" },
                   { type: form.positive, message: "Please select Batch Type" },

               ]
           }
        ],
        on_submit: [
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please add atleast one item" },
                     { type: form.required, message: "Please add atleast one item" },
                 ]
             },
           {
               elements: "#ItemID",
               rules: [
                   { type: form.required, message: "Please choose a valid item" },
                   { type: form.non_zero, message: "Please choose a valid item" },

               ]
           },
           {
               elements: "#BatchID",
               rules: [
                   { type: form.required, message: "Please choose a Batch" },
                   { type: form.non_zero, message: "Please choose a Batch" },

               ]
           },
           {
               elements: "#BatchTypeID ",
               rules: [
                   { type: form.required, message: "Please choose a BatchType" },
                   { type: form.non_zero, message: "Please choose a BatchType" },

               ]
           },
           {
               elements: "#StartDate",
               rules: [
                   { type: form.required, message: "Please add  StartDate" },
               ]
           },
        ],
        on_draft: [
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.non_zero, message: "Please add atleast one item" },
                     { type: form.required, message: "Please add atleast one item" },
                 ]
             },
               {
                   elements: "#BatchID",
                   rules: [
                       { type: form.required, message: "Please choose a Batch" },
                       { type: form.non_zero, message: "Please choose a Batch" },

                   ]
               },
           {
               elements: "#BatchTypeID ",
               rules: [
                   { type: form.required, message: "Please choose a BatchType" },
                   { type: form.non_zero, message: "Please choose a BatchType" },

               ]
           },
           {
               elements: "#StartDate",
               rules: [
                   { type: form.required, message: "Please add  StartDate" },
               ]
           },
        ],
    },


}