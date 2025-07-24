var fh_material;
var fh_process;
ProductionDefinition = {
    init: function () {
        var self = ProductionDefinition;
        item_list = Item.production_definition_item_list();
        item_select_table = $('#production-definition-item-list').SelectTable({
            selectFunction: self.select_production_definition,
            modal: "#select-production-definition-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        item_list = Item.stockable_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        item_list = Item.production_definition_material_list();
        item_select_table = $('#production-definition-material-list').SelectTable({
            selectFunction: self.select_material,
            modal: "#select-production-definition-material",
            initiatingElement: "#ItemName",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        self.bind_events();
        self.current_sequence = 0;
        self.materials = [];
        self.process = [];
        self.deletedSequences = [];
        self.deletedMaterials = [];
        self.deletedProcesses = [];
        if ($("#ProductionGroupID").val() > 0) {
            self.set_iskalkan();
            self.count_sequences();
            self.GetProductionLocationList();
            self.change_location();
            $(".material-count").val($(".material-list tbody tr").length);
        }
        $("#tabs-sequence-content .BatchTypeID:visible").trigger("change");

    },
    list: function () {

        var $list = $('#definition-list');
        $list.find('tbody tr').on('click', function () {
            var $list = $('#definition-list');
            var Id = ($(this).find("td:eq(0) .ProductionGroupID").val());
            window.location = '/Masters/ProductionDefinition/Details/' + Id;
        });
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/ProductionDefinition/GetProductionDefinitionList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "asc"]],
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
                                + "<input type='hidden' class='ProductionGroupID' value='" + row.ProductionGroupID + "'>";
                           + "<input type='hidden' class='ID' value='" + row.ID + "'>";


                       }
                   },
                   { "data": "ProductionGroupName", "className": "ProductionGroupName" },
                   { "data": "ProductName", "className": "ProductName" },
                   { "data": "BatchSize", "className": "BatchSize uk-text-right" },
                   { "data": "StandardOutputQty", "className": "StandardOutputQty uk-text-right" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ProductionGroupID").val();
                        app.load_content("/Masters/ProductionDefinition/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    details: function () {
        var self = ProductionDefinition;
        $(".BatchTypeID").on("change", self.change_materials);
        $("#tabs-sequence-content .BatchTypeID:visible").trigger("change");
        self.set_iskalkan();
        self.GetProductionLocationList();
    },

    freeze_headers: function () {
        var self = ProductionDefinition;
        fh_material = $(".material-list").FreezeHeader();
        fh_process = $(".process-list").FreezeHeader();
        //   $("body").on('change.uk.tab', "#tabs-sequence", self.set_current_seqence);
    },

    bind_events: function () {
        var self = ProductionDefinition;
        $.UIkit.autocomplete($('#production-definition-item-autocomplete'), { 'source': self.get_production_definition_item_AutoComplete, 'minLength': 1 });
        $('#production-definition-item-autocomplete').on('selectitem.uk.autocomplete', self.set_production_definition_item_details);
        $("body").on("click", "#btnOKProductionDefinition", self.select_production_definition);
        $("body").on("click", "#btnOKItem", self.select_item);
        $("body").on("click", "#btnOKProductionDefinitionMaterial", self.select_material);
        $("body").on("click", "#btnAddSequence", { AddSequence: 'ProductionSequence' }, self.add_sequence);
        $("body").on("click", "#btnAddPackingSequence", { AddSequence: 'PackingSequence' }, self.add_sequence);
        $("body").on("click", ".btnAddMaterial", self.add_material);
        $("body").on("click", ".btnAddProcess", self.add_process);
        $("body").on('change.uk.tab', "#tabs-sequence", self.set_current_seqence);
        $("body").on("click", ".btnSave", self.save);
        $("body").on("click", ".remove-material", self.remove_material);
        $("body").on("click", ".remove-process", self.remove_process);
        $(".ProductionLocation").on('ifChanged', self.AddLocationList)
        $(".LocationID").on("change", self.change_location);     
    },

    change_location: function () {
        var self = ProductionDefinition;
        var productionlocationid = $('.ProductionLocation').val();
        $('.LocationID option:selected').text() == "VOS TKY" ? ($('#1').iCheck('check'), $('#1').attr('disabled', true)) : ($('#1').iCheck('uncheck'), $('#1').attr('disabled', false));
        $('.LocationID option:selected').text() == "VOS POL" ? ($('#2').iCheck('check'), $('#2').attr('disabled', true)) : ($('#2').iCheck('uncheck'), $('#2').attr('disabled', false));
        $('.LocationID option:selected').text() == "VOS CHU" ? ($('#3').iCheck('check'), $('#3').attr('disabled', true)) : ($('#3').iCheck('uncheck'), $('#3').attr('disabled', false));
        //$('.ProductionLocation').each(function () {
        //    if ($(this).val() == $(".LocationID").val) {
        //        $(this).iCheck('check');
        //    }
        //})
    },

    LocationList: [],

    AddLocationList: function () {
        var self = ProductionDefinition;
        var LocationID = clean($(this).val());
        if ($(this).prop("checked") == true) {
            var item = {
                LocationID: LocationID
            };
            self.LocationList.push(item);
            $('#Production-Location-Mapping').val(self.LocationList.length);
        } else {
            for (var i = 0; i < self.LocationList.length; i++) {
                if (self.LocationList[i].LocationID == LocationID) {
                    self.LocationList.splice(i, 1);
                    $('#Production-Location-Mapping').val(self.LocationList.length);
                }
            }
        }
    },

    GetProductionLocationList: function () {
        var self = ProductionDefinition;
        var ProductionGroupID = $('#ID').val();
        var result;
        $.ajax({
            url: '/Masters/ProductionDefinition/GetProductionLocationMapping',
            data: { ProductionGroupID },
            dataType: "json",
            type: "POST",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $ProductionLocationDetail = " <div class='uk-width-medium-2-8'> <input class='md-input label-fixed' disabled='disabled' type='text' value='" + data[i].Location + "'> </div>"
                    app.format($ProductionLocationDetail);
                    $('#Location-Detail-Container').append($ProductionLocationDetail);
                    $('.ProductionLocation').each(function () {
                        if ($(this).val() == data[i].ProductionLocationID) {
                            $(this).iCheck('check');
                        }
                    })
                }

            }
        });
    },

    set_iskalkan: function () {
        var self = ProductionDefinition;
        $('#IsKalkan').val() == "True" ? $('.IsKalkan').iCheck('check') : $('.IsKalkan').iCheck('uncheck');
    },

    change_materials: function () {

        var self = ProductionDefinition;
        var $sequence = $(this).closest('.sequence');
        var batch_type_id = $(this).val();
        if (batch_type_id == "") {
            $sequence.find(".material-list tbody tr").show();
        } else {
            $sequence.find(".material-list tbody tr").hide();
            $(".material-list tbody").find("tr[data-batch-type-id=" + batch_type_id + "]").show();
        }
        $sequence.find(".material-list tbody tr:visible").each(function (i) {
            $(this).find(".serial-no").text(i + 1);
        });
    },

    set_current_seqence: function (event, active_item, previous_item) {
        var self = ProductionDefinition;
        self.current_sequence = active_item.index() - 1;
        var SequenceType = $("#tabs-sequence-content li").eq(self.current_sequence).find(".DefinitionSequence").val();
        var category_id;
        if (SequenceType == "ProductionSequence") {
            category_id = $("#SemifinishedGoodsItemCategoryID").val();
            $("#ItemCategoryID").val(category_id).trigger("change");
        } else if (SequenceType == "PackingSequence") {
            category_id = $("#FinishedGoodsItemCategoryID").val();
            $("#ItemCategoryID").val(category_id).trigger("change");
        }
        $(".Type").val(SequenceType).trigger("change");
        $.UIkit.autocomplete($('.Material-autocomplete').eq(self.current_sequence), { 'source': self.get_material_AutoComplete, 'minLength': 1 });
        $('.Material-autocomplete').eq(self.current_sequence).on('selectitem.uk.autocomplete', self.set_material_detail_autocomplete);
        $.UIkit.autocomplete($('.Item-autocomplete').eq(self.current_sequence), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
        $('.Item-autocomplete').eq(self.current_sequence).on('selectitem.uk.autocomplete', self.set_item_detail_autocomplete);
        $(".BatchTypeID").on("change", self.change_materials);

    },

    remove_material: function () {
        var self = ProductionDefinition;
        var $sequence = $(this).closest('.sequence');
        var row = $(this).closest('tr');
        if ($sequence.find(".DefinitionSequence").val() == "PackingSequence") {
            PackingSequence = true;
        }
        else {
            PackingSequence = false;
        }
        var deletedmaterials = {
            ID: clean(row.find(".ID").val()),
            ProductionSequence: self.current_sequence + 1,
            PackingSequence: PackingSequence
        };
        self.deletedMaterials.push(deletedmaterials);
        $(this).closest("tr").remove();
        $sequence.find(".material-list tbody tr").each(function (i, record) {
            $(this).find('.serial-no').text(i + 1);
        });
        $(".material-count").val($(".material-list tbody tr").length);

        fh_items.resizeHeader();
    },

    remove_process: function () {
        var self = ProductionDefinition;
        var $sequence = $(this).closest('.sequence');
        var row = $(this).closest('tr');
        if ($sequence.find(".DefinitionSequence").val() == "PackingSequence") {
            PackingSequence = true;
        }
        else {
            PackingSequence = false;
        }
        var deletedprocesses = {
            ProcessID: clean(row.find(".ID").val()),
            ProductionSequence: self.current_sequence + 1,
            PackingSequence: PackingSequence
        };
        self.deletedProcesses.push(deletedprocesses);
        $(this).closest("tr").remove();
        $sequence.find(".process-list tbody tr").each(function (i, record) {
            $(this).find('.index').text(i + 1);
        });
        $(".process-count").val($(".process-list tbody tr").length);

        fh_items.resizeHeader();
    },

    add_material: function () {
        var self = ProductionDefinition;
        self.error_count = 0;
        self.error_count = self.validate_materials_submit();
        if (self.error_count > 0) {
            return;
        }
        var $sequence = $(this).closest('.sequence');
        var items = [];
        var item = {
            ID: 0,
            ProductionDefinitionID: 0,
            BatchTypeID: $sequence.find('.BatchTypeID').val(),
            BatchType: $sequence.find('.BatchTypeID option:selected').text(),
            MaterialID: $sequence.find('.MaterialID').val(),
            MaterialName: $sequence.find('.MaterialName').val(),
            UnitID: $sequence.find('.MaterialUnit').val(),
            Unit: $sequence.find('.MaterialUnit option:selected').text(),
            Qty: $sequence.find('.Qty').val(),
            UsageMode: $sequence.find('.UsageMode').val(),
            PrimaryToPackUnitConFact: $sequence.find('.UnitConvertionFactor').val(),
            ProductionSequence: self.current_sequence + 1,
            StartDate: $sequence.find('.startdate').val(),
            EndDate: $sequence.find('.enddate').val(),
        };
        items.push(item);
        self.materials.push(item);
        self.add_materials_to_grid(items, $sequence);
    },

    add_materials_to_grid: function (items, $sequence) {
        var self = ProductionDefinition;
        var html = "";
        var length = $sequence.find(".material-list tbody tr").length;
        $(items).each(function (i, item) {
            html += ' <tr data-batch-type-id='+item.BatchTypeID +'>'
                    + ' <td class="uk-text-center serial-no">' + (length + i + 1) + '</td>'
                    + ' <td class="BatchType">' + item.BatchType + '</td>'
                    + ' <td class="Name">' + item.MaterialName
                    + '     <input type="hidden" class="ID" value="' + item.ID + '" />'
                    + '     <input type="hidden" class="ProductionDefinitionID" value="' + item.ProductionDefinitionID + '" />'
                    + '     <input type="hidden" class="ItemID" value="' + item.MaterialID + '" />'
                    + '     <input type="hidden" class="UnitID" value="' + item.UnitID + '" />'
                    + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                    + '     <input type="hidden" class="ProductionSequence" value="' + item.ProductionSequence + '" />'
                    + ' </td>'
                    + ' <td class="Unit">' + item.Unit + '</td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty MaterialQty" value="' + item.Qty + '" /> </td>'
                    + ' <td> <input type="text" class="md-input Usage" value="' + item.UsageMode + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty PrimaryToPackUnitConFact" value="' + item.PrimaryToPackUnitConFact + '" /> </td>'
                    + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date startdate" value="' + item.StartDate + '" name="startdate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date materialDatePicker"></i></span></div></td>'
                   + '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed all-date date enddate" value="' + item.EndDate + '" name="enddate"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar all-date materialDatePicker"></i></span></div></td>'
                    + ' <td class="uk-text-center remove-material">'
                    + '   <a data-uk-tooltip="{pos:"bottom"}" >'
                    + '   <i class="md-btn-icon-small uk-icon-remove"></i>'
                    + ' </a>'
                    + ' </td>'
                + '</tr>';
        });

        $html = $(html);
        app.format($html);
        $sequence.find(".material-list tbody").append($html);
        self.clear_material($sequence);
        self.count_materials($sequence);
        //setTimeout(function () {
        //    fh_items.resizeHeader();
        //}, 200);
    },

    clear_material: function ($sequence) {
        $sequence.find(".MaterialName").val('');
        $sequence.find(".MaterialID").val('');
        $sequence.find(".MaterialPrimaryUnitID").val('');
        $sequence.find(".MaterialPrimaryUnit").val('');
        $sequence.find(".MaterialInventoryUnitID").val('');
        $sequence.find(".MaterialInventoryUnit").val('');
        $sequence.find(".Qty").val('');
        $sequence.find(".UsageMode").val('');
        $sequence.find(".UnitConvertionFactor").val('');
        //$sequence.find(".Startdate").val('');
        //$sequence.find(".Enddate").val('');
        $sequence.find(".MaterialName").focus();
    },

    count_materials: function ($sequence) {
        var count = $sequence.find('.material-list tbody tr').length;
        $sequence.find('.material-count').val(count);
    },

    add_process: function () {
        var self = ProductionDefinition;
        self.error_count = 0;
        self.error_count = self.validate_processes_submit();
        if (self.error_count > 0) {
            return;
        }
        var $sequence = $(this).closest('.sequence');
        var items = [];
        var item = {
            ProcessID: 0,
            ProductionDefinitionID: 0,
            BatchTypeID: $sequence.find('.BatchTypeID').val(),
            ProcessName: $sequence.find('.ProcessName').val(),
            Steps: $sequence.find(".Steps").val(),
            SkilledLabourMinutes: $sequence.find('.SkilledLabourMinutes').val(),
            SkilledLabourCost: $sequence.find('.SkilledLabourCost').val(),
            UnSkilledLabourMinutes: $sequence.find('.UnSkilledLabourMinutes').val(),
            UnSkilledLabourCost: $sequence.find('.UnSkilledLabourCost').val(),
            MachineMinutes: $sequence.find('.MachineMinutes').val(),
            MachineCost: $sequence.find('.MachineCost').val(),
            Process: $sequence.find('.Process').val(),
            ProductionSequence: self.current_sequence + 1
        };
        items.push(item);
        self.process.push(item);
        self.add_process_to_grid(items, $sequence);
    },

    add_process_to_grid: function (items, $sequence) {
        var self = ProductionDefinition;
        var html = "";
        var length = $sequence.find(".process-list tbody tr").length;

        $(items).each(function (i, item) {
            html += '  <tr>'
                    + ' <td class="uk-text-center index">' + (length + i + 1) + '</td>'
                    + ' <td><input type="text" class="md-input ProcessesName" value="' + item.ProcessName + '"/>'
                    + '     <input type="hidden" class="ID" value="' + item.ProcessID + '" />'
                    + '     <input type="hidden" class="ProductionDefinitionID" value="' + item.ProductionDefinitionID + '" />'
                    + '     <input type="hidden" class="BatchTypeID" value="' + item.BatchTypeID + '" />'
                    + '     <input type="hidden" class="ProductionSequence" value="' + item.ProductionSequence + '" />'
                    + '</td>'
                    + ' <td> <input type="text" class="md-input Step" value="' + item.Steps + '"/></td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty SkilledMinutes" value="' + item.SkilledLabourMinutes + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty SkilledCost" value="' + item.SkilledLabourCost + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty UnSkilledMinutes" value="' + item.UnSkilledLabourMinutes + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty UnSkilledCost" value="' + item.UnSkilledLabourCost + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty MacMinutes" value="' + item.MachineMinutes + '" /> </td>'
                    + ' <td> <input type="text" class="md-input uk-text-right mask-production-qty MacCost" value="' + item.MachineCost + '" /> </td>'
                    + ' <td> <input type="text" class="md-input Processes" value="' + item.Process + '" /></td>'
                    + ' <td class="uk-text-center remove-process">'
                    + '   <a data-uk-tooltip="{pos:"bottom"}" >'
                    + '   <i class="md-btn-icon-small uk-icon-remove"></i>'
                    + ' </a>'
                    + ' </td>'
                + '</tr>';
        });

        $html = $(html);
        app.format($html);
        $sequence.find(".process-list tbody").append($html);
        self.clear_process($sequence);
        self.count_processes($sequence);
        //setTimeout(function () {
        //    fh_items.resizeHeader();
        //}, 200);
    },

    clear_process: function ($sequence) {
        $sequence.find("#ProcessName").val('');
        $sequence.find("#Steps").val('');
        $sequence.find("#SkilledLabourMinutes").val('');
        $sequence.find("#SkilledLabourCost").val('');
        $sequence.find("#UnSkilledLabourMinutes").val('');
        $sequence.find("#UnSkilledLabourCost").val('');
        $sequence.find("#MachineMinutes").val('');
        $sequence.find("#MachineCost").val('');
        $sequence.find("#Process").val('');
        $sequence.find("#ProcessName").focus();
    },

    count_processes: function ($sequence) {
        var count = $sequence.find('.process-list tbody tr').length;
        $('.process-count').val(count);
    },

    add_sequence: function (event) {
        var self = ProductionDefinition;
        var error_count = self.validate_sequence_submit();
        if (error_count > 0) {
            return;
        }
        $.ajax({
            url: '/Masters/ProductionDefinition/GetSequenceTemplate',
            type: "POST",
            success: function (response) {
                var Sequence = $(response);
                var serial_no = $('#tabs-sequence-content li').length + 1;
                app.format(Sequence);
                var SequenceName = $(".SequenceName").val();
                var category_id;
                $('#tabs-sequence-content').append(Sequence);
                //$("#tabs-sequence").append("<li  aria-expanded='true'><a>" + SequneceName + serial_no + "</a></li>");
                $("#tabs-sequence").append("<li  aria-expanded='true'><a>" + SequenceName + "</a></li>");
                $("#tabs-sequence").children(":last").click();
                $(".SequenceName").val('');
                $(".ProcessStage").eq(self.current_sequence).val(SequenceName);
                if (event.data.AddSequence == "PackingSequence") {
                    $('.StdOutputQty').eq(self.current_sequence).addClass('uk-hidden');
                    $('.BatchTypeList').eq(self.current_sequence).removeClass('uk-hidden');
                    $('.PackProcess').eq(self.current_sequence).addClass('uk-hidden');
                    category_id = $("#FinishedGoodsItemCategoryID").val();
                    $("#ItemCategoryID").val(category_id).trigger("change");
                    $(".DefinitionSequence").eq(self.current_sequence).val(event.data.AddSequence);
                    
                    //$(".material-list tbody tr").closest("th").find(".Mode").eq(self.current_sequence).addClass('uk-hidden');
                    //$(".material-list tbody tr").closest("td").find(".UsageMode").eq(self.current_sequence).addClass('uk-hidden');
                }
                else {
                    $('.StdOutputQty').eq(self.current_sequence).removeClass('uk-hidden');
                    $('.BatchTypeList').eq(self.current_sequence).addClass('uk-hidden');
                    $(".ConvertionFactor").eq(self.current_sequence).addClass('uk-hidden');
                    category_id = $("#SemifinishedGoodsItemCategoryID").val();
                    $("#ItemCategoryID").val(category_id).trigger("change");
                    $(".DefinitionSequence").eq(self.current_sequence).val(event.data.AddSequence);
                }
                $(".Type").val(event.data.AddSequence).trigger("change");
                clean($(".SequenceNo").eq(self.current_sequence).val(serial_no));
                var id = $(".SequenceNo").val();
                $.UIkit.autocomplete($('.Item-autocomplete').eq(self.current_sequence), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
                $('.Item-autocomplete').eq(self.current_sequence).on('selectitem.uk.autocomplete', self.set_item_detail_autocomplete);
                $.UIkit.autocomplete($('.Material-autocomplete').eq(self.current_sequence), { 'source': self.get_material_AutoComplete, 'minLength': 1 });
                $('.Material-autocomplete').eq(self.current_sequence).on('selectitem.uk.autocomplete', self.set_material_detail_autocomplete);
                $(".BatchTypeID").on("change", self.change_materials);
            }
        });
        self.count_sequences();
    },

    count_sequences: function () {
        var self = ProductionDefinition;
        var count = $(".sequence-count").val($("#tabs-sequence").length);
        $('.sequence-count').val(count);

    },

    select_production_definition: function () {
        var self = ProductionDefinition;
        var radio = $('#select-production-definition-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var ItemID = $(row).find(".ProductionItemID").val();
        $("#ItemName").val(Name);
        //$("#ProductionGroupID").val(ID);
        $("#ProductionGroupItemID").val(ID);
        UIkit.modal($('#select-production-definition-item')).hide();
    },

    select_item: function () {
        var self = ProductionDefinition;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var id = radio.val();
        var name = $(row).find(".Name").text().trim();
        var primaryUnit = $(row).find(".PrimaryUnit").val();
        var primaryUnitId = $(row).find(".PrimaryUnitID").val();
        var inventoryUnit = $(row).find(".InventoryUnit").val();
        var inventoryUnitId = $(row).find(".InventoryUnitID").val();
        var category = $(row).find(".ItemCategory").val();

        $(".ProductName").eq(self.current_sequence).val(name);
        $(".ProductID").eq(self.current_sequence).val(id);
        $(".ProductUnit").eq(self.current_sequence).val(inventoryUnit);
        $(".ProductUnitID").eq(self.current_sequence).val(inventoryUnitId);
        UIkit.modal($('#select-item')).hide();
    },

    select_material: function () {
        var self = ProductionDefinition;
        var radio = $('#select-production-definition-material tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var id = radio.val();
        var name = $(row).find(".Name").text().trim();
        var primaryUnit = $(row).find(".PrimaryUnit").val();
        var primaryUnitId = $(row).find(".PrimaryUnitID").val();
        var inventoryUnit = $(row).find(".InventoryUnit").val();
        var inventoryUnitId = $(row).find(".InventoryUnitID").val();
        var category = $(row).find(".ItemCategory").val();

        $(".MaterialName").eq(self.current_sequence).val(name);
        $(".MaterialID").eq(self.current_sequence).val(id);
        $(".MaterialPrimaryUnit").eq(self.current_sequence).val(primaryUnit);
        $(".MaterialPrimaryUnitID").eq(self.current_sequence).val(primaryUnitId);
        $(".MaterialInventoryUnit").eq(self.current_sequence).val(inventoryUnit);
        $(".MaterialInventoryUnitID").eq(self.current_sequence).val(inventoryUnitId);
        self.get_units();
        UIkit.modal($('#select-production-definition-material')).hide();
    },

    get_units: function () {
        var self = ProductionDefinition;
        $(".MaterialUnit").eq(self.current_sequence).html("");
        var html = "";
        html += "<option value='" + $(".MaterialInventoryUnitID").eq(self.current_sequence).val() + "'>" + $(".MaterialInventoryUnit").eq(self.current_sequence).val() + "</option>";
        html += "<option value='" + $(".MaterialPrimaryUnitID").eq(self.current_sequence).val() + "'>" + $(".MaterialPrimaryUnit").eq(self.current_sequence).val() + "</option>";
        $(".MaterialUnit").eq(self.current_sequence).append(html);

    },

    get_production_definition_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_production_definition_item_details: function (event, item) {
        var id = item.id;
        var name = item.name;
        $("#ItemName").val(name);
        $("#ProductionGroupItemID").val(id);
    },

    //get auto complete
    get_item_AutoComplete: function (release) {
        var self = ProductionDefinition;
        var ProductName = $(".ProductName").eq(self.current_sequence).val();
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: ProductName,
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    //set auto complete
    set_item_detail_autocomplete: function (event, item) {
        var self = ProductionDefinition;
        $(".ProductName").eq(self.current_sequence).val(item.name);
        $(".ProductID").eq(self.current_sequence).val(item.id);
        $(".ProductUnit").eq(self.current_sequence).val(item.inventoryUnit);
        $(".ProductUnitID").eq(self.current_sequence).val(item.inventoryUnitId);
    },

    //get auto complete
    get_material_AutoComplete: function (release) {
        var self = ProductionDefinition;
        var MaterialName = $(".MaterialName").eq(self.current_sequence).val();

        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: MaterialName,
                //ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    //set auto complete
    set_material_detail_autocomplete: function (event, item) {
        var self = ProductionDefinition;
        $(".MaterialName").eq(self.current_sequence).val(item.name);
        $(".MaterialID").eq(self.current_sequence).val(item.id);
        $(".MaterialPrimaryUnit").eq(self.current_sequence).val(item.primaryUnit);
        $(".MaterialPrimaryUnitID").eq(self.current_sequence).val(item.primaryUnitId);
        $(".MaterialInventoryUnit").eq(self.current_sequence).val(item.inventoryUnit);
        $(".MaterialInventoryUnitID").eq(self.current_sequence).val(item.inventoryUnitId);
        self.get_units();
    },

    save: function () {
        var self = ProductionDefinition;
        var data;
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        var location = "/Masters/ProductionDefinition/Index";
        $('.btnSave').css({ 'display': 'none' });
        var model = self.get_data();
        model.DeleteSequences = self.deletedSequences;
        model.DeleteMaterials = self.deletedMaterials;
        model.DeleteProcesses = self.deletedProcesses;
        model.LocationList = self.LocationList;
        $.ajax({
            url: '/Masters/ProductionDefinition/Save',
            data: model,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("ProductionDefinition Saved successfully");
                    window.location = location;
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'display': 'block' });
                }
            },
        });
    },
    get_data: function () {
        var self = ProductionDefinition;
        var model = {};
        model.ID = $("#ID").val();
        model.ProductionGroupID = $("#ProductionGroupID").val();
        model.ProductionGroupItemID = $("#ProductionGroupItemID").val();
        model.ProductionGroupName = $("#ProductionGroupName").val();
        model.BatchSize = clean($(".BatchSize").val());
        $(".IsKalkan").prop("checked") ? model.IsKalkan = true : model.IsKalkan = false;
        model.ProductionLocationID = clean($(".LocationID").val());
        model.Sequences = [];
        var item = {};
        $(".sequence").each(function (i) {
            item = {};
            item.ProductionID = clean($(this).find(".ProductionID").val());
            item.ProductID = clean($(this).find(".ProductID").val());
            item.UnitID = clean($(this).find(".ProductUnitID").val());
            //item.BatchSize = clean($(this).find(".BatchSize").val());
            item.ProcessStage = $(this).find(".ProcessStage").val();
            item.StandardOutputQty = clean($(this).find(".StandardOutputQty").val());
            item.ProductionSequence = i + 1;
            if ($(this).find(".DefinitionSequence").val() == "PackingSequence") {
                item.PackingSequence = true;
            }
            else {
                item.PackingSequence = false;
            }
            //item.PackingSequence = clean($(this).find(".DefinitionSequence").val());
            model.Sequences.push(item);
        });
        model.Materials = [];
        var material = {};
        $(".material-list tbody tr").each(function () {
            material = {};
            material.ID = clean($(this).find(".ID").val()),
            material.ProductionDefinitionID = clean($(this).find(".ProductionDefinitionID").val()),
            material.BatchTypeID = clean($(this).find(".BatchTypeID").val()),
            material.MaterialID = clean($(this).find(".ItemID").val()),
            material.MaterialName = $(this).find(".Name").val(),
            material.UnitID = clean($(this).find(".UnitID").val()),
            material.Unit = $(this).find(".Unit").val(),
            material.Qty = clean($(this).find(".MaterialQty").val()),
            material.UsageMode = $(this).find(".Usage").val(),
            material.StartDate = $(this).find(".startdate").val(),
            material.EndDate = $(this).find(".enddate").val(),
            material.PrimaryToPackUnitConFact = clean($(this).find(".PrimaryToPackUnitConFact").val()),
            material.ProductionSequence = clean($(this).find(".ProductionSequence").val()),
            model.Materials.push(material);
        });
        model.Processes = [];
        var process = {};
        $(".process-list tbody tr").each(function () {
            process = {};
            process.ProcessID = clean($(this).find(".ID").val()),
            process.ProductionDefinitionID = clean($(this).find(".ProductionDefinitionID").val()),
            process.BatchTypeID = clean($(this).find(".BatchTypeID").val()),
            process.ProcessName = $(this).find(".ProcessesName").val(),
            process.Steps = $(this).find(".Step").val(),
            process.SkilledLabourMinutes = clean($(this).find(".SkilledMinutes").val()),
            process.SkilledLabourCost = clean($(this).find(".SkilledCost").val()),
            process.UnSkilledLabourMinutes = clean($(this).find(".UnSkilledMinutes").val()),
            process.UnSkilledLabourCost = clean($(this).find(".UnSkilledCost").val()),
            process.MachineMinutes = clean($(this).find(".MacMinutes").val()),
            process.MachineCost = clean($(this).find(".MacCost").val()),
            process.Process = $(this).find(".Processes").val(),
            process.ProductionSequence = clean($(this).find(".ProductionSequence").val()),
            model.Processes.push(process);
        });
        return model;
    },

    validate_sequence_submit: function () {
        var self = ProductionDefinition;
        if (self.rules.on_sequence_submit.length) {
            return form.validate(self.rules.on_sequence_submit);
        }
        return 0;
    },

    validate_submit: function () {
        var self = ProductionDefinition;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    validate_materials_submit: function () {
        var self = ProductionDefinition;
        if (self.rules.on_materials_submit.length) {
            return form.validate(self.rules.on_materials_submit);
        }
        return 0;
    },

    validate_processes_submit: function () {
        var self = ProductionDefinition;
        if (self.rules.on_processes_submit.length) {
            return form.validate(self.rules.on_processes_submit);
        }
        return 0;
    },

    rules: {
        on_sequence_submit: [
             {
                 elements: "#ProductionGroupName",
                 rules: [
                     { type: form.required, message: "Production Group Name is required" },
                 ],
             },
             {
                 elements: "#ProductionGroupItemID",
                 rules: [
                     { type: form.required, message: "Please choose an item" },
                     { type: form.non_zero, message: "Please choose an item" },
                 ],
             },
             {
                 elements: "#BatchSize",
                 rules: [
                     { type: form.required, message: "Batch Size is required" },
                     { type: form.non_zero, message: "Batch Size is required" },
                 ],
             },
             {
                 elements: "#SequenceName",
                 rules: [
                     { type: form.required, message: "Sequence Name is required" },
                     { type: form.non_zero, message: "Sequence Name is required" },
                 ],
             },
        ],

        on_materials_submit: [
             {
                 elements: ".MaterialID",
                 rules: [
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                         },
                         message: "Please choose material"
                     },
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.non_zero(element) : true;
                         }, message: "Please choose material"
                     },
                 ],
             },
             {
                 elements: ".Qty",
                 rules: [
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                         },
                         message: "Material qty is required"
                     },
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.non_zero(element) : true;
                         }, message: "Material qty is required"
                     },
                 ],
             },
             {
                 elements: ".BatchTypeID",
                 rules: [
                        {
                            type: function (element) {
                                var self = ProductionDefinition;
                                var jj = $(element).closest("li").find(".DefinitionSequence").val();
                                return $(element).closest("li").find(".DefinitionSequence").val() == "PackingSequence" ? $(element).closest("li").index() == self.current_sequence ? form.required(element) : true : true;
                            }, message: 'Please select batch type'
                        },
                 ]
             },
             {
                 elements: ".enddate",
                 rules: [
                      //{ type: form.required, message: "Please enter end date" },
                      {
                          type: function (element) {
                              var self = ProductionDefinition;
                              var jj = $(element).closest("li").find(".DefinitionSequence").val();
                              return $(element).closest("li").find(".enddate").val() == "" ? $(element).closest("li").index() == self.current_sequence ? form.required(element) : true : true;
                          }, message: 'Please select end date'
                      },
                      {
                          type: function (element) {
                              var end_time;
                              var self = ProductionDefinition;
                              var start_time, start_date_datesplit, start_date = 0, end_date_datesplit, end_date = 0;
                              if ($(element).closest("li").index() == self.current_sequence) {
                                  start_date = $(element).closest("li").find(".startdate").val().split('-');
                                  end_date = $(element).closest("li").find(".enddate").val().split('-');
                                  var start_date_datesplit = new Date(start_date[2], start_date[1] - 1, start_date[0]);
                                  var start_date = Date.parse(start_date_datesplit);
                                  var end_date_datesplit = new Date(end_date[2], end_date[1] - 1, end_date[0]);
                                  var end_date = Date.parse(end_date_datesplit);
                              }
                              return start_date <= end_date;
                          }, message: "Start date exceeds end date"
                      }
                 ]
             },
               {
                   elements: ".startdate",
                   rules: [
                        //{ type: form.required, message: "Please enter start date" },
                         {
                             type: function (element) {
                                 var self = ProductionDefinition;
                                 var jj = $(element).closest("li").find(".DefinitionSequence").val();
                                 return $(element).closest("li").find(".startdate").val() == "" ? $(element).closest("li").index() == self.current_sequence ? form.required(element) : true : true;
                             }, message: 'Please select start date'
                         },
                   ]
               },
        ],

        on_processes_submit: [
             {
                 elements: ".ProcessName",
                 rules: [
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                         },
                         message: "Process name is required"
                     },
                 ],
             },
             {
                 elements: ".Steps",
                 rules: [
                     {
                         type: function (element) {
                             var self = ProductionDefinition;
                             return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                         },
                         message: "Steps is required"
                     },
                 ],
             },
             {
                 elements: ".SkilledLabourMinutes",
                 rules: [
                    {
                        type: function (element) {
                            var self = ProductionDefinition;
                            var SkilledLabourMinutes = $(".SkilledLabourMinutes").eq(self.current_sequence).val();
                            var SkilledLabourCost = $(".SkilledLabourCost").eq(self.current_sequence).val();
                            var UnSkilledLabourMinutes = $(".UnSkilledLabourMinutes").eq(self.current_sequence).val();
                            var UnSkilledLabourCost = $(".UnSkilledLabourCost").eq(self.current_sequence).val();
                            var MachineMinutes = $(".MachineMinutes").eq(self.current_sequence).val();
                            var MachineCost = $(".MachineCost").eq(self.current_sequence).val();
                            if (SkilledLabourMinutes == 0 && SkilledLabourCost == 0 && UnSkilledLabourMinutes == 0
                                && UnSkilledLabourCost == 0 && MachineMinutes == 0 && MachineCost == 0) {
                                return false;
                            }
                            return true;
                        },
                        message: "Atleast one minutes or cost is required"
                    },
                 ],
             },
        ],

        on_submit: [
            {
                elements: "#ProductionGroupName",
                rules: [
                    { type: form.required, message: "Production Group Name is required" },
                ],
            },
             {
                 elements: "#ProductionGroupItemID",
                 rules: [
                     { type: form.required, message: "Please choose an item" },
                     { type: form.non_zero, message: "Please choose an item" },
                 ],
             },
             {
                 elements: "#BatchSize",
                 rules: [
                     { type: form.required, message: "Batch Size is required" },
                     { type: form.non_zero, message: "Batch Size is required" },
                 ],
             },
            {
                elements: ".Sequence",
                rules: [
                    { type: form.required, message: "Please add atleast one sequence" },
                ],
            },
            {
                elements: ".ProductID",
                rules: [
                    {
                        type: function (element) {
                            var self = ProductionDefinition;
                            return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                        },
                        message: "Please choose product"
                    },
                    {
                        type: function (element) {
                            var self = ProductionDefinition;
                            return $(element).closest("li").index() == self.current_sequence ? form.non_zero(element) : true;
                        }, message: "Please choose product"
                    },
                ],
            },
            {
                elements: ".ProcessStage",
                rules: [
                    { type: form.required, message: "Please enter process stage" },
                ],
            },
            //{
            //    elements: ".StandardOutputQty",
            //    rules: [
            //        { type: form.required, message: "Material quantity is required" },
            //        { type: form.non_zero, message: "Material quantity is required" },
            //    ],
            //},
            {
                elements: ".material-count",
                rules: [
                       { type: form.required, message: "Please choose atleast one material" },
                       { type: form.non_zero, message: "Please choose atleast one material" },
                ]
            },
             {
                 elements: ".material-list .enddate",
                 rules: [
                        { type: form.required, message: "Please enter end date" },
                     {
                         type: function (element) {

                             var result;
                             var error = 0;
                             var production_sequence = clean($(element).closest("tr").find(".ProductionSequence").val());
                             $(".material-list tbody tr .ProductionSequence[value='" + production_sequence + "']").each(function () {
                                 var date_time_string1 = $(this).closest('tr').find('.startdate').val();
                                 var date_time_string2 = $(this).closest('tr').find('.enddate').val();
                                 var date = date_time_string1.split('-');
                                 var startdate = new Date(date[2], date[1] - 1, date[0]);
                                 start_date = Date.parse(startdate);
                                 var date = date_time_string2.split('-');
                                 var enddate = new Date(date[2], date[1] - 1, date[0]);
                                 enddate = Date.parse(enddate);

                                 if ((startdate > enddate))
                                     error++;

                             });

                             return error == 0
                         }, message: "Start date exceeds end date"
                     }
                 ]
             },
           {
               elements: ".material-list .startdate",
               rules: [
                      { type: form.required, message: "Please enter start date" },

               ]
           },
            {
                elements: ".Process-count",
                rules: [
                    { type: form.non_zero, message: "Process is required" },
                    {
                        type: function (element) {
                            var self = ProductionDefinition;
                            return $(element).closest("li").index() == self.current_sequence ? form.required(element) : true;
                        },
                        message: "Process is required"
                    },
                ]
            },
            {
                elements: ".sequence-count",
                rules: [
                       { type: form.required, message: "Please choose atleast one sequence" },
                       { type: form.non_zero, message: "Please choose atleast one sequence" },
                ]
            },

            {
                elements: ".LocationID",
                rules: [
                       { type: form.required, message: "Please select Location" },
                       { type: form.non_zero, message: "Please select Location" },
                ]
            },
        ],
    },

};