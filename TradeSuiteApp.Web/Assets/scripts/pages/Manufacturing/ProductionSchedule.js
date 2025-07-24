var freeze_header;

production_schedule = {
    init: function () {
        var self = production_schedule;
        freeze_header = $("#production-schedule-item-list").FreezeHeader({ height: 250 });
        Item.production_group_item_list();
        $('#production-group-list').SelectTable({
            selectFunction: ProductionScheduleCRUD.select_production_group,
            returnFocus: "#txtBatchSize",
            modal: "#select-production-group",
            initiatingElement: "#txtProductGroupName",
            startFocusIndex: 3,
            selectionType: "radio"
        });

        Item.stockable_item_list();
        $('#item-list').SelectTable({
            selectFunction: ProductionScheduleCRUD.select_item,
            returnFocus: "#txtAdditionalIssueQty",
            modal: "#select-item",
            initiatingElement: "#txtAddnitionalIssue",
            startFocusIndex: 3,
            selectionType: "radio"
        });
        ProductionScheduleCRUD.create();
        self.bind_events();
    },   
    details: function () {
        var self = production_schedule;
        freeze_header = $("#production-schedule-item-list").FreezeHeader();
        $("body").on("click", ".printpdf", self.printpdf);
        self.bind_events();
    },

    printpdf: function () {
        var self = production_schedule;
        $.ajax({
            url: '/Reports/Manufacturing/ProductionSchedulePrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },

    bind_events: function () {
        $("body").on('click', ".cancel", ProductionScheduleCRUD.cancel_confirm);
        $("#btnOKProductionGroup").on("click", ProductionScheduleCRUD.select_production_group);
        $("#btnOKItem").on("click", ProductionScheduleCRUD.select_item);
       
    },
    list: function () {
        var self = production_schedule;
        $('#tabs-module').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = production_schedule;
        // types
        //  1. Scheduled
        //  2. Cancelled.
        //  3. Draft.

        var $list;

        switch (type) {
            case "draft":
                $list = $('#draft-list');
                break;
            case "scheduled":
                $list = $('#scheduled-list');
                break;
            case "issue-started":
                $list = $('#issue-started-list');
                break;
            case "issue-completed":
                $list = $('#issue-completed-list');
                break;
            case "cancelled":
                $list = $('#cancelled-list');
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

            var url = "/Manufacturing/ProductionSchedule/GetProductionScheduleList?type=" + type;

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
                               + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "TransDate", "className": "TransDate" },
                   { "data": "ProductionGroupName", "className": "ProductionGroup" },
                   { "data": "ProductionStartDate", "className": "StartDate" },
                   { "data": "ActualBatchSize", "className": "Batchsize" },
                   { "data": "BatchNo", "className": "BatchNo" },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Manufacturing/ProductionSchedule/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

}
 

ProductionScheduleCRUD = {

    cancel_confirm: function () {
        $currobj = ProductionScheduleCRUD;
        app.confirm_cancel("Do you want to cancel", function () {
            $currobj.cancel();
        }, function () {
        })
    },

    create: function () {

        $currobj = ProductionScheduleCRUD;
        $currobj.ProductGroup = { ID: 0, Name: '' };

        this.init = function () {
            $.UIkit.autocomplete($('#productionschedule-autocomplete'), { 'source': $currobj.get_item_AutoComplete, 'minLength': 1 });
            $('#productionschedule-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_item_details);


            $.UIkit.autocomplete($('#additionalIssueItem-autocomplete'), { 'source': $currobj.get_additionalissueitem_AutoComplete, 'minLength': 1 });
            $('#additionalIssueItem-autocomplete').on('selectitem.uk.autocomplete', $currobj.set_additionalIssueItem_details);


            $('body').on('click', '#btnAddItems', $currobj.add_item_clicked);

            //$('body').on('keydown', '.txtActualBatchSize', $currobj.actual_batch_size_changing);
            //$('body').on('keyup', '.txtActualBatchSize', $currobj.actual_batch_size_changed);

            $('body').on('click', '.btnSaveASDraft', function () { $currobj.save(true); });
            $('body').on('click', '.btnCompleted', function () { $currobj.save_confirm(false); });
            $('body').on('keyup', '#txtBatchSize', $currobj.calculate_update_requiredqty);
            $('body').on('click', '#btnAddAdditionalIssue', $currobj.add_additional_issue);

            if ($('#hdnProductionScheduleID').val() > 0) {       //Edit
                var productionGroupID = $('#hdnProductionGroupID').val();
                var productionGroupName = $('#hdnProductionGroupName').val();
                $currobj.ProductGroup = { ID: productionGroupID, Name: productionGroupName };
            }

        }

        this.get_item_AutoComplete = function (release) {

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

        this.get_additionalissueitem_AutoComplete = function (callback) {
            var itemHint = $('#txtAddnitionalIssue').val();
            $currobj.ajaxRequest("GetAdditionalIssueAutoCompleteItemList", { itemHint: itemHint }, "GET", callback);
        }

        this.set_item_details = function (event, item) {
            var id = item.id;
            var name = item.name;
            $("#txtProductGroupName").val(name);
            $("#ProductionGroupID").val(id);
            $("#StandardBatchSize").val(item.stdBatchSize);
            $("#IsKalkan").val(item.isKalkan);
            ProductionScheduleCRUD.get_batch_no();
            $("#txtBatchSize").focus();
        };

        this.set_additionalIssueItem_details = function (event, item) {   // on select auto complete item
            $('#txtAdditionalIssueUOM').val(item.unit);
            $("#txtAddnitionalIssue").val(item.name);
            $("#ItemID").val(item.id);
            $("#Code").val(item.code);
            $("#UnitID").val(item.unitid);
            $("#txtAdditionalIssueQty").focus();
            $currobj.SelectedAdditionalIssue = { ItemID: item.id, Name: item.value, UnitID: item.unitid, Unit: item.unit };

        }

        this.add_additional_issue = function () {
            self = ProductionScheduleCRUD;
            self.error_count = 0;
            self.error_count = self.validate_add_additional();
            if (self.error_count == 0) {
                var rowNo = $currobj.get_next_row_sno($('#production-schedule-item-list'));

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
                $currobj.SelectedAdditionalIssue = null;

                $('#txtAdditionalIssueQty').val('');
                $('#txtReqDate').val('');
                $('#ItemID').val('');
                $('#txtAddnitionalIssue').val('');
                $('#Unit').val('');
                $("#txtAdditionalIssueUOM").val('');
            }
        }

        this.add_item_clicked = function () {
            self = ProductionScheduleCRUD;
            self.error_count = 0;
            var groupId = clean($("#ProductionGroupID").val());
            self.error_count = self.validate_add();
            if (self.error_count == 0) {
                if (groupId > 0) {
                    var startDate = $('#txtStartDate').val();
                    var callback = function (response) {
                        var $response = $(response);
                        app.format($response);
                        $('#production-schedule-item-list tbody').html($response);
                        freeze_header.resizeHeader();

                        $('#production-schedule-item-list tbody tr').each(function () {
                            if (startDate) {
                                $(this).find('.txtRequiredDate').val(startDate);
                            }
                        });
                        $currobj.calculate_update_requiredqty();
                    }
                    $currobj.ajaxRequest("GetProductionIssueItemView", {
                        productionGroupID: groupId
                    }, "GET", callback);
                }
            }
        }

        this.get_next_row_sno = function (tblObj) {
            var rowNo = $(tblObj).find('tbody tr').length + 1;
            return rowNo;
        }

        this.get_material_tab_rowhtml = function (rowNo, itemID, name, uom, unitID, qty, date, parentCallBack) {

            var callBack = function (response) {
                parentCallBack($row);
            }


        }

        this.actual_batch_size_changing = function (event) {
            var code = event.which;
            if ((code > 95 && code < 106) || code == 110 || code == 8 || code == 37 || code == 38) {
                return true;
            }
            else return false;
        }

        this.calculate_update_requiredqty = function (currRow) {
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

        this.initialize_date_picker = function (obj) {
            var date = UIkit.datepicker(UIkit.$(obj), { format: 'DD-MM-YYYY' });
            var self = $(obj);
            $(obj).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();
            });
        }

        this.save_confirm= function () {
            app.confirm_cancel("Do you want to Save", function () {
                $currobj.save();
            }, function () {
            })
        },

        this.save = function (isDraft) {
            var error_count = 0;
            var url;
            if (isDraft) {
                error_count = $currobj.validate_draft();
                url = 'SaveAsDraft'
            } else {
                error_count = $currobj.validate_form();
                url = 'Save';
            }

            if (error_count > 0) {
                return;
            }
            var saveObj = $currobj.get_save_obj(isDraft);


            var callback = function (response) {
                if (response.Result) {
                    app.show_notice(response.Message);
                    setTimeout(function () {
                        window.location = "/Manufacturing/ProductionSchedule/";
                    }, 1000);
                }
                else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".btnSaveASDraft, .btnCompleted").css({ 'display': 'block' });
                }
            }
            $(".btnSaveASDraft, .btnCompleted").css({ 'display': 'none' });
            $currobj.ajaxRequest(url, saveObj, 'POST', callback);
        }

        this.get_save_obj = function (isDraft) {
            var saveObj = {};

            saveObj.ID = $('#hdnProductionScheduleID').val();
            saveObj.TransNo = $('#txtTransNo').val();
            saveObj.TransDateStr = $('#txtTransDate').val();
            saveObj.ProductionGroupID = $('#ProductionGroupID').val();
            saveObj.ProductID = 0; //$('').val();
            saveObj.ProductionStartDateStr = $('#txtStartDate').val();
            saveObj.StandardBatchSize = $('#StandardBatchSize').val();
            saveObj.ActualBatchSize = clean($('#txtBatchSize').val());
            saveObj.ProductionLocationID = $('#ddlProductionLocation').val();
            saveObj.StoreID = $('#ddlProductionStore').val();
            saveObj.IsDraft = isDraft;
            saveObj.IsCompleted = 0;
            saveObj.BatchNo = $('#txtBatchNo').val();
            saveObj.IsKalkan = $("#IsKalkan").val();
            saveObj.Remarks = $("#Remarks").val();

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
                item.YogamQty = $(currRow).find('.tdYogamQty').val();
                item.StandardOutputQty = $(currRow).find('.StandardOutputQty').val();
                items.push(item);
            });

            saveObj.Items = items;

            return saveObj;
        }

        this.ajaxRequest = function (url, data, requestType, callBack) {
            $.ajax({
                url: $currobj.root_url() + url,
                cache: false,
                type: requestType,

                data: data,
                success: function (successResponse) {
                    if (callBack != null && callBack != undefined)
                        callBack(successResponse);
                },
                error: function (errResponse) {//Error Occured 
                }
            });
        }

        this.root_url = function () {
            return "/Manufacturing/ProductionSchedule/";
        }

        $currobj.init();
    },
    select_item: function () {
        //var self = create;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        var UnitID = $(row).find(".PrimaryUnitID").val();
        $("#txtAddnitionalIssue").val(Name);
        $("#txtAdditionalIssueUOM").val(Unit);
        $("#ItemID").val(ID);
        $("#Code").val(Code);
        $("#Unit").val(Unit);
        $("#UnitID").val(UnitID);
        UIkit.modal($('#select-item')).hide();
    },
    select_production_group: function () {
        //var self = create;
        var radio = $('#select-production-group tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var StandardBatchSize = $(row).find(".StdBatchSize").text().trim();
        var IsKalkan = $(row).find(".IsKalkan").val();
        if ($('#production-schedule-item-list tbody tr').length > 0) {
            app.confirm("Items will be removed", function () {
                $('#production-schedule-item-list tbody').html('');
                $("#txtProductGroupName").val(Name);
                $("#ProductionGroupID").val(ID);
                $("#StandardBatchSize").val(StandardBatchSize);
                $("#IsKalkan").val(IsKalkan);
                UIkit.modal($('#select-production-group')).hide();
            })
        }
        else {
            $('#production-schedule-item-list tbody').val('');
            $("#txtProductGroupName").val(Name);
            $("#ProductionGroupID").val(ID);
            $("#StandardBatchSize").val(StandardBatchSize);
            $("#IsKalkan").val(IsKalkan);
            UIkit.modal($('#select-production-group')).hide();
        }
        ProductionScheduleCRUD.get_batch_no();
    },

    get_batch_no: function () {
        $.ajax({
            url: '/Manufacturing/ProductionSchedule/GetProductionBatchNo',
            data: {
                IsKalkan: $('#IsKalkan').val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#txtBatchNo").val(response.data);
                }
            }
        });
    },

    cancel: function () {
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

    },

    validate_add: function () {
        var self = ProductionScheduleCRUD;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_add_additional: function () {
        var self = ProductionScheduleCRUD;
        if (self.rules.on_add_additional.length) {
            return form.validate(self.rules.on_add_additional);
        }
        return 0;
    },
    validate_draft: function () {
        var self = ProductionScheduleCRUD;
        if (self.rules.on_draft.length) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },
    validate_form: function () {
        var self = ProductionScheduleCRUD;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_add_additional: [
        {
            elements: "#txtAdditionalIssueQty",
            rules: [
              { type: form.required, message: "Please enter quantity size" },
                  { type: form.numeric, message: "Numeric value required" },
                  { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter Batch size" },
            ]
        },
        {
            elements: "#txtReqDate",
            rules: [
                 { type: form.required, message: "Invalid  date" },
                { type: form.future_date, message: "Invalid  date" },

            ]
        },
         {
             elements: "#txtAddnitionalIssue",
             rules: [
                 { type: form.required, message: "Please select item" },
             ]
         },

        ],
        on_add: [
            {
                elements: "#txtProductGroupName",
                rules: [
                    { type: form.required, message: "Please select Production Group Name" },
                ]
            },
            {
                elements: "#txtBatchSize",
                rules: [
                    { type: form.required, message: "Please enter Batch size" },
                    { type: form.numeric, message: "Numeric value required" },
                    { type: form.positive, message: "Positive number required" },
                    { type: form.non_zero, message: "Please enter Batch size" },

                ]
            },
            {
                elements: "#txtStartDate",
                rules: [
                     { type: form.required, message: "Invalid  date" },
                    { type: form.future_date, message: "Invalid  date" },
                ]
            },
        ],
        on_submit: [
             {
                 elements: "#ProductionGroupID",
                 rules: [
                     { type: form.required, message: "Please select Production Group", alt_element: "#txtProductGroupName" },
                 ]
             },
             {
                 elements: "#txtStartDate",
                 rules: [
                     { type: form.required, message: "Please enter start date" },
                     { type: form.future_date, message: "Invalid start date" },
                 ]
             },
             {
                 elements: "#txtBatchSize",
                 rules: [
                     { type: form.required, message: "Please enter Batch size" },
                     { type: form.numeric, message: "Numeric value required" },
                     { type: form.positive, message: "Positive number required" },
                     { type: form.non_zero, message: "Please enter Batch size" },
                 ]
             },
             {
                 elements: "#production-schedule-item-list tbody",
                 rules: [
                     {
                         type: function (element) {

                             return $(element).find("tr").length > 0;
                         }, message: "No items added"
                     },
                 ]
             },
             {
                 elements: ".txtRequiredDate",
                 rules: [
                     { type: form.required, message: "Please enter required date" },
                     { type: form.future_date, message: "Invalid required date" },
                 ]
             },
        ],
        on_draft: [
             {
                 elements: "#ProductionGroupID",
                 rules: [
                     { type: form.required, message: "Please select Production Group", alt_element: "#txtProductGroupName" },
                 ]
             },
             {
                 elements: "#txtStartDate",
                 rules: [
                     { type: form.required, message: "Please enter start date" },
                     { type: form.future_date, message: "Invalid start date" },
                 ]
             },
             {
                 elements: "#txtBatchSize",
                 rules: [
                     { type: form.required, message: "Please enter Batch size" },
                     { type: form.numeric, message: "Numeric value required" },
                     { type: form.positive, message: "Positive number required" },
                     { type: form.non_zero, message: "Please enter Batch size" },
                 ]
             },
             {
                 elements: "#production-schedule-item-list tbody",
                 rules: [
                     {
                         type: function (element) {
                             return $(element).find("tr").length > 0;
                         }, message: "No items added"
                     },
                 ]
             }
        ]
    },
}