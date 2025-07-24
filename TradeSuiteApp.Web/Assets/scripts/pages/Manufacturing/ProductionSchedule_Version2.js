production_schedule.init = function () {
    var self = production_schedule;
    freeze_header = $("#production-schedule-item-list").FreezeHeader({ height: 250 });
    Item.raw_material_list();
    $('#raw-material-list').SelectTable({
        selectFunction: self.select_additional_issue_item,
        returnFocus: "#txtAdditionalIssueQty",
        modal: "#select-raw-material",
        initiatingElement: "#txtAddnitionalIssue",
        startFocusIndex: 2,
        selectionType: "radio"
    });

    Item.production_group_item_list();
    $('#production-group-list').SelectTable({
        selectFunction: self.select_production_group,
        returnFocus: "#txtBatchSize",
        modal: "#select-production-group",
        initiatingElement: "#txtProductGroupName",
        selectionType: "radio"
    });
    machine.all_machine_list();
    $('#machine-list').SelectTable({
        selectFunction: self.select_machine,
        returnFocus: "#txtAdditionalIssueQty",
        modal: "#select-machine",
        initiatingElement: "#MachineList",
        selectionType: "radio"
    });

    MouldSettings.mould_list();
    select_mould_table = $('#mould-list').SelectTable({
        selectFunction: self.select_mould,
        returnFocus: "#SupplierReferenceNo",
        modal: "#select-mould",
        initiatingElement: "#MouldName",
        selectionType: "radio"
    });
    self.hide_grid();
    self.bind_events();
    var selectedVal = $('#ddlProductionProcess option:selected').text();
    if (selectedVal == "Mixing") {
        $(".stdbatchsize").show();
        $(".batchsize").show();
        $("#btnAddItems").show();
        $(".Productiontables").show();
    }

}


production_schedule.bind_events = function () {
    var self = production_schedule;

    $.UIkit.autocomplete($('#productionschedule-autocomplete'), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
    $('#productionschedule-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);

    $.UIkit.autocomplete($('#additionalIssueItem-autocomplete'), { 'source': self.get_additionalissueitem_AutoComplete, 'minLength': 1 });
    $('#additionalIssueItem-autocomplete').on('selectitem.uk.autocomplete', self.set_additionalIssueItem_details);

    $.UIkit.autocomplete($('#machine-autocomplete'), { 'source': self.get_machine_AutoComplete, 'minLength': 1 });

    $.UIkit.autocomplete($('#Mould-autocomplete'), { 'source': self.get_mould, 'minLength': 1 });
    $('#Mould-autocomplete').on('selectitem.uk.autocomplete', self.set_mould_details);

    $("#btnOKItem").on("click", self.select_additional_issue_item);
    $("#btnOKMachine").on("click", self.select_machine);
    $("#ddlProductionProcess").on("change", self.show_grid);
    $('#btnOKProductionGroup').on('click', self.select_production_group);
    $('#btnAddItems').on('click', self.add_item_clicked);
    $('#btnAddAdditionalIssue').on('click', self.add_additional_issue);
    $('.btnCompleted').on('click', self.save);
    $('.btnSaveASDraft').on('click', self.save);
    $("#btnOKMould").on("click", self.select_mould);
    $('.cancel').on('click', self.cancel_confirm);
}
production_schedule.cancel_confirm= function () {
    self = production_schedule;
    app.confirm_cancel("Do you want to cancel", function () {
        self.cancel();
    }, function () {
    })
}

production_schedule.hide_grid = function () {
    var self = production_schedule;
    $(".stdbatchsize").hide();
    $(".batchsize").hide();
    $("#btnAddItems").hide();
    $(".Productiontables").hide();
}

production_schedule.show_grid = function () {

    var self = production_schedule;
    var selectedVal = $('#ddlProductionProcess option:selected').text();
    if (selectedVal == "Mixing") {
        $(".stdbatchsize").show();
        $(".batchsize").show();
        $("#btnAddItems").show();
        $(".Productiontables").show();
    }
    else {
        self.hide_grid();
    }

}

production_schedule.select_machine = function () {
    var radio = $('#select-machine tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    var Code = $(row).find(".Code").text().trim();
    var LoadedMould = $(row).find(".LoadedMould").text();
    $("#Machine").val(Name);
    $("#MachineID").val(ID);
    UIkit.modal($('#select-machine')).hide();
}

production_schedule.select_additional_issue_item = function () {
    var self = production_schedule;
    var radio = $('#select-raw-material tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    var Unit = $(row).find(".Unit").text().trim();
    var Stock = $(row).find(".Stock").val();
    var UnitID = $(row).find(".UnitID").val();
    $("#txtAddnitionalIssue").val(Name);
    $("#txtAdditionalIssueUOM").val(Unit);
    $("#UnitID").val(UnitID);
    $("#ItemID").val(ID);
}

production_schedule.select_production_group = function () {
    var self = production_schedule;
    var radio = $('#select-production-group tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Name = $(row).find(".Name").text().trim();
    var Code = $(row).find(".Code").text().trim();
    var Unit = $(row).find(".Unit").text().trim();
    var UnitID = $(row).find(".UnitID").val();
    var ProductId = $(row).find(".ProductID").text();
    var BatchSize = $(row).find(".StdBatchSize").text();
    var ProductionGroupID = $(row).find(".ItemID").val();
    $("#txtProductGroupName").val(Name);
    $("#UOM").val(Unit);
    $("#UnitID").val(UnitID);
    $("#ProductID").val(ProductId);
    $(".stdbatchsize").val(BatchSize);
    $("#ProductionGroupID").val(ProductionGroupID);
    UIkit.modal($('#select-production-group')).hide();
}

production_schedule.get_additionalissueitem_AutoComplete = function (release) {
    $.ajax({
        url: '/Masters/Item/GetRawMaterialForAutoComplete',
        data: {
            Hint: $('#txtAddnitionalIssue').val(),
            WarehouseID: $('#WarehouseID').val()
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            release(data);
        }
    });
}

production_schedule.set_additionalIssueItem_details = function (event, item) {   // on select auto complete item
    $('#txtAdditionalIssueUOM').val(item.unit);
}

production_schedule.get_machine_AutoComplete = function (release) {
    $.ajax({
        url: '/Masters/Machine/GetMachineListForAutoComplete',
        data: {
            Hint: $('#MachineList').val()
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            release(data);
        }
    });
}

production_schedule. select_mould= function () {
    self = production_schedule;
    var radio = $('#mould-list tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var ID = radio.val();
    var Code = $(row).find(".Code").val();
    var MouldName = $(row).find(".MouldName").text().trim();
    $("#MouldName").val(MouldName);
    $("#MouldID").val(ID);
    UIkit.modal($('#select-mould')).hide();
}

production_schedule.get_mould= function (release) {
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
}

production_schedule.set_mould_details= function (event, item) {   // on select auto complete mould
    self = production_schedule;
    $("#MouldName").val(item.value);
    $("#MouldID").val(item.id);
}

production_schedule.get_item_AutoComplete = function (release) {

    $.ajax({
        url: '/Masters/Item/GetProductionGroupItemAutoComplete',
        data: {
            NameHint: $('#txtProductGroupName').val()
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            release(data.data);
        }
    });
}

production_schedule.set_item_details = function (event, item) {
    var id = item.id;
    var name = item.name;
    $("#txtProductGroupName").val(name);
    $("#ProductionGroupID").val(id);
    $("#StandardBatchSize").val(item.stdBatchSize);
    $("#txtBatchSize").focus();
}

production_schedule.add_item_clicked = function () {
    var self = production_schedule;
    self.error_count = 0;
    self.error_count = self.validate_add();
    var groupId = $("#ProductionGroupID").val();
    $.ajax({
        url: '/Manufacturing/Productionschedule/GetProductionIssueItemView',
        dataType: "html",
        data: {
            'productionGroupID': groupId
        },
        type: "POST",
        success: function (response) {
            $('#production-schedule-item-list tbody').html(response);
            self.calculate_update_requiredqty();
            freeze_header.resizeHeader();
            var startDate = $('#txtStartDate').val();
            $('#production-schedule-item-list tbody tr').each(function () {
                if (startDate) {
                    $(this).find('.txtRequiredDate').val(startDate);

                }
            });
        }
    });
}

production_schedule.add_additional_issue = function () {
    var self = production_schedule;
    self.error_count = 0;
    if (self.error_count == 0) {
        var rowNo = self.get_next_row_sno($('#production-schedule-item-list'));

        var qty = clean($('#txtAdditionalIssueQty').val());
        var date = $('#txtReqDate').val();
        var itemID = $('#ItemID').val();
        var name = $('#txtAddnitionalIssue').val();
        var uom = $('#txtAdditionalIssueUOM').val();
        var unitID = $('#UnitID').val();
        var rowHtml = '<tr>';
        rowHtml += '<td><input type="hidden" class="hdnItemID" value="' + itemID + '">'
            + '<input type="hidden" class="hdnUnitID" value="' + unitID + '">' + rowNo + '</td>';
        rowHtml += '<td class="txtName">' + name + '</td>';
        rowHtml += '<td class="txtUOM">' + uom + '</td>';
        rowHtml += '<td class="tdYogamQty mask-qty"> ' + 0 + ' </td>';
        rowHtml += '<td><input type="text" class="md-input label-fixed txtRequiredQty mask-qty" value="' + qty + '" /></td>';
        rowHtml += '<td><div class="uk-input-group"><input type="text" class="md-input label-fixed future-date date txtRequiredDate" name="txtRequiredDate" value = "' + date + '"><span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar future-date"></i></span></div></td>';
        rowHtml += '<td><input type = "text" class="md-input uk-text txtRemarks" ></td>';
        rowHtml += ' <td class="uk-text-center" onclick="$(this).parent().remove()">' +
                '   <a data-uk-tooltip="{pos:"bottom"}" >' +
                    '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                    ' </a>' +
                ' </td>';
        rowHtml += '</tr>';
        var $row = $(rowHtml);
        app.format($row);
        $('#production-schedule-item-list tbody').append($row);
        freeze_header.resizeHeader();
        self.SelectedAdditionalIssue = null;

        $('#txtAdditionalIssueQty').val('');
        $('#txtReqDate').val('');
        $('#ItemID').val('');
        $('#txtAddnitionalIssue').val('');
        $('#Unit').val('');
        $("#txtAdditionalIssueUOM").val('');
    }
}

production_schedule.get_next_row_sno = function (tblObj) {
    var rowNo = $(tblObj).find('tbody tr').length + 1;
    return rowNo;
}

production_schedule.calculate_update_requiredqty = function (currRow) {
    var startDate = $('#txtStartDate').val();

    $('#production-schedule-item-list tbody tr').each(function () {

        var currRow = $(this);
        $(currRow).find('.txtRequiredDate').val(startDate);

        var yogamQty = clean($(currRow).find('.hdnYogamQty').val());
        var stdBatchSize = clean($(currRow).find('.hdnStandardBatchSize').val());

        var actualBatchSize = clean($('#txtBatchSize').val());

        if (stdBatchSize > 0) {
            var result = ((actualBatchSize / stdBatchSize) * yogamQty);
            $(currRow).find('.txtRequiredQty').val(result);
        }
    });

}

production_schedule.get_save_items = function () {
    var saveObj = {};

    saveObj.ID = $('#hdnProductionScheduleID').val();
    saveObj.TransNo = $('#txtTransNo').val();
    saveObj.TransDateStr = $('#txtTransDate').val();
    saveObj.ProductionGroupID = $('#ProductionGroupID').val();
    saveObj.ProductID = 0; //$('').val();
    saveObj.ProductionStartDateStr = $('#txtStartDate').val();
    saveObj.StartTime = $('#StartTime').val();
    saveObj.StandardBatchSize = $('#StandardBatchSize').val();
    saveObj.ActualBatchSize = clean($('#txtBatchSize').val());
    saveObj.ProductionLocationID = $('#ddlProductionLocation').val();
    saveObj.StoreID = $('#ddlProductionStore').val();
    saveObj.IsCompleted = 0;
    saveObj.BatchNo = $('#txtBatchNo').val();
    saveObj.EndDate = $('#txtEndDate').val();
    saveObj.EndTime = $('#EndTime').val();
    saveObj.MouldID = $('#MouldID').val();
    saveObj.MachineID = $('#MachineID').val();
    saveObj.ProcessID = $('#ddlProductionProcess').val();

    var items = [];

    $('#production-schedule-item-list tbody tr').each(function () {
        var item = {};
        var currRow = $(this);
        item.ItemID = $(currRow).find('.hdnItemID').val();
        item.RequiredQty = clean($(currRow).find('.txtRequiredQty').val());
        item.RequiredDateStr = $(currRow).find('.txtRequiredDate').val();
        item.Remarks = $(currRow).find('.txtRemarks').val();
        item.ProductDefinitionTransID = $(currRow).find('.ProductDefinitionTransID').val();
        item.UnitID = $(currRow).find('.hdnUnitID').val();
        items.push(item);
    });

    saveObj.Items = items;

    return saveObj;
}

production_schedule.save= function () {
    var self = production_schedule;
    var data;
    index = $("#production-schedule-item-list tbody").length;
    $("#item-count").val(index);
    self.error_count = 0;
    self.error_count = self.validate_draft();
    if (self.error_count > 0) {
            return;
        }
    
    data = self.get_save_items();
    if ($(this).hasClass("btnSaveASDraft")) {
        data.IsDraft = true;
    }
    $.ajax({
        url: '/Manufacturing/ProductionSchedule/Save',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Result) {
                app.show_notice(response.Message);
                setTimeout(function () {
                    window.location = "/Manufacturing/ProductionSchedule/";
                }, 1000);
            }
            else {
                $(".btnSaveASDraft, .btnCompleted").css({ 'display': 'block' });
                app.show_error(response.Message);
            }
        }
    });
}

production_schedule.validate_draft= function () {
    var self = production_schedule;
    if (self.rules.on_draft.length) {
        return form.validate(self.rules.on_draft);
    }
    return 0;
}

production_schedule.validate_add = function () {
    var self = production_schedule;
    if (self.rules.on_add.length) {
        return form.validate(self.rules.on_add);
    }
    return 0;
}

production_schedule.rules= {
    on_draft:[
               {
                   elements: "#txtProductGroupName",
                   rules: [
                       { type: form.required, message: "Please select Production Group", alt_element: "#txtProductGroupName" },
                   ]
               },
               {
                   elements: "#Machine",
                   rules: [
                       { type: form.required, message: "Please enter Machine" },
                   ]
               },
               {
                   elements: "#MouldName",
                   rules: [
                       { type: form.required, message: "Please enter Mould" },
                   ]
               },
                {
                    elements: "#ddlProductionProcess",
                    rules: [
                        { type: form.required, message: "Please enter Mould" },
                    ]
                },
                 {
                     elements: "#StartTime",
                     rules: [
                         { type: form.required, message: "Please enter Start Time" },
                     ]
                 },
                  {
                      elements: "#txtEndDate",
                      rules: [
                          { type: form.required, message: "Please enter End Date" },
                      ]
                  },
                   {
                       elements: "#EndTime",
                       rules: [
                           { type: form.required, message: "Please enter End Time" },
                       ]
                   },
    ],
    on_add: [
              {
                  elements: "#txtProductGroupName",
                  rules: [
                      { type: form.required, message: "Please select Production Group", alt_element: "#txtProductGroupName" },
                  ]
              },
              {
                  elements: "#Machine",
                  rules: [
                      { type: form.required, message: "Please enter Machine" },
                  ]
              },
              {
                  elements: "#MouldName",
                  rules: [
                      { type: form.required, message: "Please enter Mould" },
                  ]
              },
               {
                   elements: "#ddlProductionProcess",
                   rules: [
                       { type: form.required, message: "Please enter Mould" },
                   ]
               },
                {
                    elements: ".batchsize",
                    rules: [
                        { type: form.required, message: "Please enter Batch size" },
                    ]
                },
    ]
}

production_schedule.cancel= function () {
    $(".btnSaveASDraft, .btnCompleted,.cancel,.edit ").css({ 'display': 'none' });
    $.ajax({
        url: '/Manufacturing/Productionschedule/Cancel',
        data: {
            ID: $("#ID").val(),
            Table: "Production.ProductionSchedule"
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data.Status == "success") {
                app.show_notice("Production schedule cancelled successfully");
                setTimeout(function () {
                    window.location = "/Manufacturing/Productionschedule/Index";
                }, 1000);
            } else {
                app.show_error("Failed to cancel.");

                $(".btnSaveASDraft, .btnCompleted,.cancel,.edit ").css({ 'display': 'block' });
            }
        },
    });
}
  