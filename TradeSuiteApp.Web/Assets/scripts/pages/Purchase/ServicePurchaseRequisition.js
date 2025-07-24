var ItemList = [];
var freeze_header;
service_purchase_requisition = {
    init: function () {
        var self = service_purchase_requisition;

        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.bind_events();
        self.freeze_headers();
        self.get_employee();

    },

    details: function () {
        var self = service_purchase_requisition;
        self.freeze_headers();
    },

    freeze_headers: function () {
        freeze_header = $("#service-purchase-requisition-items-list").FreezeHeader();
    },

    list: function () {
        var self = service_purchase_requisition;
        $('#tabs-pr').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var self = service_purchase_requisition;
        var $list;

        switch (type) {
            case "draft":
                $list = $('#pr-draft-list');
                break;
            case "to-be-ordered":
                $list = $('#pr-to-be-ordered-list');
                break;
            case "partially-ordered":
                $list = $('#pr-partially-ordered-list');
                break;
            case "fully-ordered":
                $list = $('#pr-fully-ordered-list');
                break;
            case "cancelled":
                $list = $('#pr-cancelled-list');
                break;
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/ServicePurchaseRequisition/GetPurchaseRequisitionList?type=" + type;

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
                    { "data": "PurchaseOrderNumber", "className": "PurchaseOrderNumber" },
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "ItemName", "className": "ItemName" },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/ServicePurchaseRequisition/Details/" + Id);
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
        $("#btnAddProduct").on('click', service_purchase_requisition.add_item);
        $("#DDLItemCategory").on('change', service_purchase_requisition.get_purchase_category);
        $(".btnUpdateSPr").on("click", service_purchase_requisition.update);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': service_purchase_requisition.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', service_purchase_requisition.set_item_details);
        $("body").on("click", ".remove-item", service_purchase_requisition.remove_item);
        //$(".btnSaveSPrDraft").on("click", service_purchase_requisition.save);
        $("#btnOKItem").on("click", service_purchase_requisition.select_item);
        $("body").on("change", "#sprDepartment", service_purchase_requisition.get_employee);
        $(".btnSaveSPr, .btnSaveSPrNew,.btnSaveSPrDraft").on("click", service_purchase_requisition.on_save);
    },
    get_employee: function () {

        $.ajax({
            url: '/Masters/Employee/GetEmployeeByDepartment/',
            data: { DepartmentID: $("#sprDepartment option:selected").val() },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#sprEmployee").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#sprEmployee").append(html);
            }
        });
    },
    remove_item: function () {
        var self = service_purchase_requisition;
        var row = $(this).closest("tr");
        var next_row = $(row).next("tr");
        var category = $(this).closest("tr").find(".CategoryID").val();
        row.remove();
        if (category > 0) {
            next_row.remove();
        }

        $("#service-purchase-requisition-items-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#item-count").val($("#service-purchase-requisition-items-list tbody tr").length);

    },

    get_purchase_category: function () {
        var item_category_id = $(this).val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = -1;
        }
        $.ajax({
            url: '/Purchase/PurchaseRequisition/GetPurchaseCategory/' + item_category_id,
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#DDLPurchaseCategory").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#DDLPurchaseCategory").append(html);
            }
        });
    },

    ServicePR_item_Source: function (request, response) {
        $.ajax({
            url: '/ServicePurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'Purchase',
                term: $('#ItemID').val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                ItemList = [];
                ItemList = data;
                response($.map(data, function (item) {
                    return {
                        label: item.Name,
                        value: item.ID,
                    }
                }))
            },
        });
    },

    ServicePR_item_select: function (e, i) {
        e.preventDefault();
        $("#ItemID").val(i.item.label);
        $("#Item").val(i.item.value);
        $.each(ItemList, function (key, item) {
            if (item.ID == i.item.value) {
                $("#Unit").val(item.Unit);
            }
        });

    },

    set_item_details: function (event, item) {   // on select auto complete item
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#Rate").val('');
        $("#Unit").val(item.unit);
        //$("#LastPr").val(item.lastPr);
        //   $("#LowestPr").val(item.lowestPr);
        //  $("#PendingOrderQty").val(item.pendingOrderQty);
        //  $("#QtyWithQc").val(item.qtyWithQc);
        //  $("#QtyAvailable").val(item.qtyAvailable);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CategoryID").val(item.itemCategory);
        $("#TravelCategoryID").val(item.travelCategory);
        if ($("#TravelCategoryID").val() > 0) {
            $(".travel-services").removeClass("uk-hidden").addClass("visible");
        }
        else {
            $(".travel-services").addClass("uk-hidden").removeClass("visible");
        }
        $("#txtRqQty").focus();
    },

    select_item: function () {
        var self = service_purchase_requisition;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Unit = $(row).find(".PrimaryUnit").val();
        // var lastPr = $(row).find(".lastPr").val();
        //  var lowestPr = $(row).find(".lowestPr").val();
        //var Category = $(row).find(".ItemCategory").val();
        //    var pendingOrderQty = $(row).find(".pendingOrderQty").val();
        //    var qtyWithQc = $(row).find(".qtyWithQc").val();
        //     var qtyAvailable = $(row).find(".qtyAvailable").val();
        var gstPercentage = $(row).find(".gstPercentage").val();
        var itemCategoryID = $(row).find(".itemCategoryID").val();
        var travelCategoryID = clean($(row).find(".TravelCategoryID").val());
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Rate").val('');
        $("#Unit").val(Unit);
        // $("#LastPr").val(lastPr);
        //  $("#LowestPr").val(lowestPr);
        //  $("#PendingOrderQty").val(pendingOrderQty);
        //  $("#QtyWithQc").val(qtyWithQc);
        //  $("#QtyAvailable").val(qtyAvailable);
        $("#GSTPercentage").val(gstPercentage);
        $("#CategoryID").val(itemCategoryID);
        $("#TravelCategoryID").val(travelCategoryID);
        if ($("#TravelCategoryID").val() > 0) {
            $(".travel-services").removeClass("uk-hidden").addClass("visible");
        }
        else {
            $(".travel-services").addClass("uk-hidden").removeClass("visible");
        }
        $("#txtRqQty").focus();
        UIkit.modal($('#select-item')).hide();

    },

    get_item_details: function (release) {

        $.ajax({
            url: '/Purchase/ServicePurchaseRequisition/getItemForAutoComplete',
            data: {
                Areas: 'PurchaseOrder',
                term: $('#ItemName').val(),
                ItemCategoryID: $("#DDLItemCategory").val(),
                PurchaseCategoryID: $("#DDLPurchaseCategory").val(),
                SupplierId: $("#SupplierID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    add_item: function () {
        var self = service_purchase_requisition;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count == 0) {
            var SerialNo = $(".prTbody .rowPr").length + 1;
            var ServiceLocation = $("#sprLocation").val() == "" ? "" : $("#sprLocation  option:selected").text();
            var Department = $("#sprDepartment").val() == "" ? "" : $("#sprDepartment  option:selected").text();
            var Employee = $("#sprEmployee").val() == "" ? "" : $("#sprEmployee  option:selected").text();
            var Company = $("#sprInterCompany").val() == "" ? "" : $("#sprInterCompany  option:selected").text();
            var Project = $("#sprProject").val() == "" ? "" : $("#sprProject  option:selected").text();


            var tr = '';
            if ($("#TravelCategoryID").val() > 0) {
                var TravelFrom = $("#TravelFromID").val() == "" ? "" : $("#TravelFromID  option:selected").text();
                var TravelTo = $("#TravelFromID").val() == "" ? "" : $("#TravelToID  option:selected").text();
                var TransportMode = $("#TransportModeID").val() == "" ? "" : $("#TransportModeID  option:selected").text();
                var TravelRemark = $("#TravelingRemarks").val() == undefined ? '' : $("#TravelingRemarks").val();
                tr = '<tr>'
                        + '<td>'
                            + '<input type="hidden" class="travelfromId" value="' + $("#TravelFromID option:selected").val() + '"/>'
                            + '<input type="hidden" class="traveltoId" value="' + $("#TravelToID option:selected").val() + '"/>' //TODO
                            + '<input type="hidden" class="modeoftravelId" value="' + $("#TransportModeID option:selected").val() + '"/>'
                            + '<input type="hidden" class="traveldate" value="' + $("#travelDate").val() + '"/>' //TODO
                            + '<input type="hidden" class="travelremark" value="' + TravelRemark + '"/>'

                        + '</td>'
                        + '<td colspan="10">'
                        + '<div class="uk-grid">'
                        + '<div class="uk-width-medium-2-10"><label>Travel From</label>'
                        + '<div> ' + TravelFrom + '</div></div>'
                        + '<div class="uk-width-medium-2-10"><label>Travel To</label>'
                        + '<div> ' + TravelTo + '</div></div>'

                        + '<div class="uk-width-medium-2-10"><label>Mode of Travel</label>'
                        + '<div> ' + TransportMode + '</div></div>'

                  + '<div class="uk-width-medium-2-10  "><label>Travel Date</label>'
                        + '<div class="md-input future-date date "> ' + $("#travelDate").val() + '</div></div>'

                        + '<div class="uk-width-medium-2-10 "><label>Travel Remarks</label>'
                        + '<div class="travelremark"> ' + $("#TravelingRemarks").val() + '</div></div>'


                        + '</div></td>'
                        + '<td></td>';
            }

            html = '<tr class="rowPr">' +
                    ' <td class="uk-text-center">' + SerialNo + '</td>' +

                    ' <td>' + $("#ItemName").val() +
                    '<input type="hidden" class="ItemId" value="' + $("#ItemID").val() + '"/>' +
                    '<input type="hidden" class="LocationId" value="' + $("#sprLocation").val() + '"/>' +//TODO
                    '<input type="hidden" class="DepartmentId" value="' + $("#sprDepartment").val() + '"/>' +
                    '<input type="hidden" class="EmployeeId" value="' + $("#sprEmployee").val() + '"/>' +
                    '<input type="hidden" class="InterCompanyId" value="' + $("#sprInterCompany").val() + '"/>' +
                    '<input type="hidden" class="ProjectId" value="' + $("#sprProject").val() + '"/>' +
                    '<input type="hidden" class="CategoryID" value="' + $("#CategoryID").val() + '"/>' +
                    '</td>' +
                    ' <td >' + $("#Unit").val() + '</td>' +
                    ' <td><input type="text" class="md-input txtRqQty mask-qty" min="0" value="' + $("#txtRqQty").val() + '" /></td>' +
                    ' <td><input type="text" class="md-input future-date date txtDate" value="' + $("#txtExpDate").val() + '" /></td>' +
                    ' <td class="sprLocation">' + ServiceLocation + '</td>' +
                    ' <td class="sprDepartment">' + Department + '</td>' +
                    ' <td class="sprEmployee">' + Employee + '</td>' +
                    ' <td class="sprInterCompany">' + Company + '</td>' +
                    ' <td class="sprProject">' + Project + '</td>' +
                    ' <td><input type="text" class="md-input sprRemarks" value="' + $("#sprRemarks").val() + '" /></td>' +
                    ' <td class="uk-text-center">' +
                         '     <a class="remove-item">' +
                         '         <i class="uk-icon-remove"></i>' +
                         '     </a>' +
                         ' </td>' +
            '</tr>' + tr;
            var $html = $(html);
            app.format($html);
            $(".prTbody").append($html);
            service_purchase_requisition.clear_item_select_controls();
            $("#ItemID").focus();
            freeze_header.resizeHeader();
            self.count_items();
        }
      
    },

    count_items: function () {
        var count = $('#service-purchase-requisition-items-list tbody tr').length;
        $('#item-count').val(count);
    },

    validate_item: function () {
        var self = service_purchase_requisition;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_form: function () {
        var self = service_purchase_requisition;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
    },
    validate_draft: function () {
        var self = service_purchase_requisition;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
    },

    get_data: function () {

    },

    clearItemSelectControls: function () {

    },

    GetProductList: function () {

    },

    //submit: function () {
    //    var self = service_purchase_requisition;
    //    self.error_count = 0;
    //    self.error_count = self.validate_form();
    //    if (self.error_count == 0) {
    //        service_purchase_requisition.save();
    //    }
    //},

    on_save: function () {

        var self = service_purchase_requisition;
        var master = self.ServicePR_Master();
        var Trans = self.ServicePR_Trans();
        var location = "/Purchase/ServicePurchaseRequisition/Index";
        var url = '/Purchase/ServicePurchaseRequisition/Create';

        if ($(this).hasClass("btnSaveSPrDraft")) {
            master.IsDraft = true;
            url = '/Purchase/ServicePurchaseRequisition/SaveAsDraft'
            self.error_count = self.validate_draft();
        } else {
            self.error_count = self.validate_form();
            if ($(this).hasClass("btnSaveSPrNew")) {
                location = "/Purchase/ServicePurchaseRequisition/Create";
            }
        }

        if (self.error_count > 0) {
            return;
        }

        if (!master.IsDraft) {
            app.confirm_cancel("Do you want to save?", function () {
                self.save(master, Trans, url, location);
            }, function () {
            })
        } else {
            self.save(master, Trans, url, location);
        }
    },


    save: function (master, Trans, url, location) {
        var self = service_purchase_requisition;
        $(".btnSaveSPrDraft, .btnSaveSPr,.btnSaveSPrNew ").css({ 'display': 'none' });
        $.ajax({
            url: url,
            data: { _master: master, _trans: Trans },
            dataType: "json",
            type: "POST",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "success") {
                    //clearAllControls();
                    app.show_notice("Successfully saved");
                    setTimeout(function () {
                        window.location = location;
                    }, 1000);
                } else {
                    if (typeof data.data[0].ErrorMessage != "undefined")
                        app.show_error(data.data[0].ErrorMessage);
                    $(".btnSaveSPrDraft, .btnSaveSPr,.btnSaveSPrNew ").css({ 'display': 'block' });

                }
            },
        });
    },

    ServicePR_Master: function (IsDraft) {
        var model = {
            PurchaseRequisitionNumber: $("#PurchaseRequisitionNumber").val(),
            PrDate:$("#PrDate").val(),
            IsDraft: IsDraft,
            ID: $("#ID").val(),
            FromDeptID: $("#FromDeptID").val(),
            PrTrans: service_purchase_requisition.ServicePR_Trans()
        }
        return model;
    },

    ServicePR_Trans: function () {
        var ProductsArray = [];
        $(".prTbody .rowPr").each(function () {
            var e = $(this);
            var ID = e.find(".ItemId").val();
            var ReqQuantity = e.find(".txtRqQty").val().replace(/\,/g, '');
            var ExpDate = e.find(".txtDate").val();
            var DepartmentID = e.find(".DepartmentId").val();
            var LocationID = e.find(".LocationId").val();
            var EmployeeID = e.find(".EmployeeId").val();
            var InterCompanyID = e.find(".InterCompanyId").val();
            var ProjectID = e.find(".ProjectId").val();
            var Remark = e.find(".sprRemarks").val();

            var next_row = $(e).next("tr");

            var TravelFromID = $(next_row).find(".travelfromId").val();
            var TravelToID = $(next_row).find(".traveltoId").val();
            var TransportModeID = $(next_row).find(".modeoftravelId").val();
            var TravelingRemarks = $(next_row).find(".travelremark").val();
            var TravelDate = $(next_row).find(".traveldate").val();

            ProductsArray.push({
                ID: ID,
                ReqQuantity: ReqQuantity,
                ExpDate: ExpDate,
                DepartmentID: DepartmentID,
                LocationID: LocationID,
                EmployeeID: EmployeeID,
                InterCompanyID: InterCompanyID,
                ProjectID: ProjectID,
                Remark: Remark,
                TravelFromID: TravelFromID || 0,
                TravelToID: TravelToID || 0,
                TransportModeID: TransportModeID || 0,
                TravelingRemarks: TravelingRemarks || '',
                TravelDate: TravelDate || ''
            });
        });
        return ProductsArray;
    },

    //save_and_new: function () {
    //    var self = service_purchase_requisition;
    //    self.error_count = 0;
    //    self.error_count = self.validate_form();
    //    if (self.error_count == 0) {
    //        service_purchase_requisition.save_new();
    //    }
    //},

    update: function () {

    },

    rules: {
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                   { type: form.required, message: "Please choose a valid item" },
                ]
            },
             {
                 elements: "#txtRqQty",
                 rules: [
                     { type: form.required, message: "Please enter quantity" },
                     { type: form.numeric, message: "Numeric value required" },
                     { type: form.positive, message: "Positive number required" },
                     { type: form.non_zero, message: "Positive number required" },
                 ]
             },
              {
                  elements: "#txtExpDate",
                  rules: [
                      { type: form.future_date, message: "Invalid Date" },
                  ]
              }, {
                  elements: "#sprLocation",
                  rules: [
                      { type: form.required, message: "Please select Location" },
                  ]
              },
                  {
                      elements: "#sprDepartment",
                      rules: [
                          { type: form.required, message: "Please select Department" },
                      ]
                  },
            {
                elements: ".travel-services.visible #TravelToID",
                rules: [
                     { type: form.required, message: "Please choose travel to ID" },
                     //{
                     //    type: function (element) {
                     //        return $(element).val() == "" || $(element).val() != $("#TravelFromID").val();
                     //    }, message: "Travel from and travel locations to are same"
                     //},
                     {
                         type: function (element) {
                             return !($(element).val() == "" && $("#TravelFromID").val() != "");
                         }, message: "invalid Travel To location"
                     }
                ]
            },
            {
                elements: ".travel-services.visible #TravelFromID",
                rules: [
                        {
                            type: function (element) {
                                return !($(element).val() == "" && $("#TravelToID").val() != "");
                            }, message: "invalid Travel From location"
                        },
                        { type: form.required, message: "Please choose travel from ID" },

                ]
            },
             {
                 elements: ".travel-services.visible #travelDate",
                 rules: [
                       { type: form.required, message: "Please choose travel date" },

                 ]
             },
             {
                 elements: ".travel-services.visible #TransportModeID",
                 rules: [
                    { type: form.required, message: "Please choose travel mode" },

                 ]
             },          


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
                elements: "#FromDeptID",
                rules: [
                    { type: form.non_zero, message: "Please select from department" },
                ]
            },
              {
                  elements: ".txtRqQty",

                  rules: [
                      {
                          type: form.non_zero, message: 'Please change Item Quantity'
                      }
                  ]
              },
               {
                   elements: ".txtDate",
                   rules: [
                       { type: form.future_date, message: "Invalid Date" },
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
                elements: "#FromDeptID",
                rules: [
                    { type: form.non_zero, message: "Please select from department" },
                ]
            }
             

        ]

    },


    clear_item_select_controls: function () {
        $("#ItemID").val('');
        $("#ItemName").val('');
        $("#txtRqQty").val('');
        $("#txtexpDate").val('');
        $("#sprEmployee").val('');
        $("#sprInterCompany").val('');
        $("#sprProject").val('');
        $("#sprRemarks").val('');
        $("#TravelFromID").val("");
        $("#TravelToID").val("");
        $("#TravelModeID").val("");
        $("#TransportModeID").val("");
        $("#TravelingRemarks").val('');
        $("#TravelCategoryID").val('');
        $("#CategoryID").val('');
    }
};