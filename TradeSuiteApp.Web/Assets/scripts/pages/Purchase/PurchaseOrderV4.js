var progressbar = $("#progress-bar"),
           bar = progressbar.find('.uk-progress-bar');
var select_table;
var freeze_header;
purchase_order = {
    init: function () {
        var self = purchase_order;

        item_list = Item.purchase_item_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });
        self.purchase_requisition_list();
        supplier.supplier_list('stock');
        $('#supplier-list').SelectTable({
            selectFunction: self.select_supplier,
            returnFocus: "#SupplierReferenceNo",
            modal: "#select-supplier",
            initiatingElement: "#SupplierName",
            selectionType: "radio"
        });
        $('#purchase-requisition-list').SelectTable({
            selectFunction: self.select_puchase_requisitions,
            returnFocus: "#GST",
            modal: "#select_pr",
            initiatingElement: "#PurchaseRequisitionIDS",
            selectionType: "checkbox"
        });

        purchase_orderCRUD.purchaseOCreateAndUpdate();
        purchase_order_CalculateEvents.calculations();
        purchase_order.get_purchase_category();
        freeze_header = $("#purchase-order-items-list").FreezeHeader();

        self.bind_events();


        if (clean($("#ID").val()) > 0) {
            if (clean($("#IsInterCompany").val()) == 1) {
                $(".supplierLocation").addClass("uk-hidden");
                $(".intercompanySupplierlocation").removeClass("uk-hidden");
                var checkboxes = "";
                var $checkboxes = $("<div></div>");
                $(".checkbox-container").html('');
                checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
                $checkboxes.append(checkboxes);
                app.format($checkboxes);
                $(".checkbox-container").eq(0).html($checkboxes);
            }
            else {
                $(".supplierLocation").removeClass("uk-hidden");
                $(".intercompanySupplierlocation").addClass("uk-hidden");
                var checkboxes = "";
                var $checkboxes = $("<div></div>");
                $(".checkbox-container").html('');
                checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
                $checkboxes.append(checkboxes);
                app.format($checkboxes);
                $(".checkbox-container").eq(0).html($checkboxes);
            }
        }
    },

    details: function () {
        var self = purchase_order;
        setTimeout(function () {
            freeze_header = $("#purchase-order-items-list").FreezeHeader();
        }, 100);
        $("body").on("click", ".printpdf", self.printpdf);
        $("body").on("click", ".btnSendMail", self.send_mail);
        if (clean($("#IsInterCompany").val()) == 1) {
            $(".supplierLocation").addClass("uk-hidden");
            $(".intercompanySupplierlocation").removeClass("uk-hidden");

        }
        else {
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }
        $('body').on('click', '.btnitemsuspend', purchase_order.confirm_suspend_item);
    },
    confirm_suspend_item: function () {
        var self = purchase_order;
        var id = $(this).closest('tr').find('.POTransID').val();
        app.confirm("Do you want to suspend? This can't be undone", function () {
            self.suspend_purchaseorder_item(id);
        });
    },

    suspend_purchaseorder_item: function (id) {
        $.ajax({
            url: '/Purchase/PurchaseOrder/SuspendItem',
            dataType: "json",
            type: "GET",
            data: {
                ID: id
            },
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Purchase order item suspended successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnitemsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                    freeze_header.resizeHeader();
                }

                if (response.Data == 0) {
                    app.show_error("Some error occured while suspending item");

                }
            }
        });
    },

    printpdf: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                    //app.sendmail(url, "PurchaseOrder", id,subject,body);
                }
            }
        });
    },

    send_mail: function () {
        var self = purchase_order;
        var id = $("#ID").val();
        var subject = "Automaticaly generated";
        var body = "This is an automaticaly generated mail.";
        //var ToEmailID = "neethu@kraftlabs.com";
        var ToEmailID = $("#Email").val();

        $.ajax({
            url: '/Reports/Purchase/PurchaseOrderPrintPdf',
            data: {
                id: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.sendmail(url, "PurchaseOrder", id, subject, body,ToEmailID);
                }
            }
        });
    },

    list: function () {
        var self = purchase_order;

        $('#tabs-purchase-order').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

        $("body").on('click', '.btnclone', self.open_clone);
        $("body").on('click', '.btnsuspend', self.confirm_suspend_po);
    },

    tabbed_list: function (type) {
        var self = purchase_order;
        var $list;

        switch (type) {
            case "Draft":
                $list = $('#purchase-order-draft-list');
                break;
            case "ToBeApproved":
                $list = $('#purchase-order-to-be-approved-list');
                break;
            case "PartiallyApproved":
                $list = $('#purchase-order-partially-approved-list');
                break;
            case "Approved":
                $list = $('#purchase-order-approved-list');
                break;
            case "PartiallyProcessed":
                $list = $('#purchase-order-partially-processed-list');
                break;
            case "Processed":
                $list = $('#purchase-order-processed-list');
                break;
            case "Suspended":
                $list = $('#purchase-order-suspended-list');
                break;
            case "Cancelled":
                $list = $('#purchase-order-cancelled-list');
                break;
            default:
                $list = $('#purchase-order-draft-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/PurchaseOrder/GetPurchaseOrderList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 10,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "async": true,
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
                    { "data": "SupplierName", "className": "SupplierName" },
                    {
                        "data": "NetAmount", "searchable": false, "className": "NetAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                    { "data": "CategoryName", "className": "CategoryName" },
                    { "data": "ItemName", "className": "ItemName" },
                    {
                        "data": "Suspend", "className": "action uk-text-center", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsSuspendable ? "<button class='md-btn md-btn-primary btnsuspend' >Suspend</button>" : "";
                        }
                    },
                    //{
                    //    "data": "Cancel", "className": "action uk-text-center", "searchable": false,
                    //    "render": function (data, type, row, meta) {
                    //        return "<button class='md-btn md-btn-primary btnclone' >Clone</button>";
                    //    }
                    //}
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Purchase/PurchaseOrder/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    purchase_requisition_list: function () {
        var $list = $('#purchase-requisition-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        //$("body").on('click', ".cancel", purchase_order.cancel_confirm);
        $("body").on('ifChanged', '.chkCheck', purchase_order.include_item);
        $("body").on('click', '.remove-item', purchase_order.remove_item);
        $("#DDLItemCategory").on('change', purchase_order.get_purchase_category);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': purchase_order.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_supplier_details);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': purchase_order.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', purchase_order.set_item_details);
        UIkit.uploadSelect($("#select-quotation"), purchase_order.selected_quotation_settings);
        UIkit.uploadSelect($("#other-quotations"), purchase_order.other_quotation_settings);
        $('body').on('click', 'a.remove-file', purchase_order.remove_file);
        $('body').on('click', 'a.remove-quotation', purchase_order.remove_quotation);
        $('body').on('click', '.cancel', purchase_order.cancel);
        $('body').on('click', '.print-preview', purchase_order.print_preview);
        $('body').on('click', '.close-preview', purchase_order.close_print_preview);
        $('body').on('click', '#invoice_print', purchase_order.print);
        $('body').on('click', '.print', purchase_order.print);
        $("#BusinessCategoryID").on('change', purchase_order.remove_all_items_from_grid);
        $('body').on('change', '#ShippingToId', purchase_order.set_state_id);
        $("#btnOkPrList").on('click', purchase_order.select_puchase_requisitions);
        $("#btnOKSupplier").on('click', purchase_order.select_supplier);
        $("body").on('click', '#purchase-order-list tbody tr', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Purchase/PurchaseOrder/Details/' + Id;
        });

        $("body").on('click', '.btnclone', purchase_order.open_clone);
        $("body").on('click', '.btnsuspend', purchase_order.confirm_suspend_po);
        $(".select-item").on('click', purchase_order.check_supplier);
        $("#btnOKItem").on("click", purchase_order.select_item);
        $("#BatchType").on("change", purchase_order.get_rate);
        $("body").on('change', '#purchase-order-items-list tbody tr .batch_type', purchase_order.get_row);
        $("body").on('ifChanged', ".select-all-item", purchase_order.select_all_items);
        purchase_order.Load_All_DropDown();
        //$("body").on('click', '.btnSaveAndMail', purchase_order.SaveAndEmail);

    },
    remove_all_items_from_grid:function() {
        var self = purchase_order;
        $("#purchase-order-items-list tbody tr").empty();
        CalculateNetAmountValue();
        self.count_items();
    },
    select_all_items: function () {

        var checkboxes = $("#purchase-order-items-list").find("input.chkCheck");
        var self = this;
        $(checkboxes).each(function () {
            if ($(this).is(":checked") != $(self).is(":checked")) {
                $(this).iCheck('toggle');
            }
        });
        $("#purchase-order-items-list tr").each(function (e) {
            CalculateGST($(this));
        });
        CalculateGSTOutsideTheGrid();
        CalculateNetAmountValue();
    },
    get_row: function () {
        var self = purchase_order;
        var row = $(this).closest('tr');
        self.get_po_item_details(row);
    },
    get_po_item_details: function (row) {
        var self = purchase_order;
        var ItemID = $(row).find('.ItemID').val();
        var BatchtypeID = $(row).find('.clBatch').val();
        self.get_trans_details(ItemID, BatchtypeID, row);
    },
    get_trans_details: function (ItemID, BatchtypeID, row) {
        $.ajax({
            url: '/Masters/Item/GetTranasactionDetails',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: ItemID,
                BatchTypeID: BatchtypeID
            },
            success: function (response) {
                if (response.Status == "success") {
                    $(row).find('.clLastPr').val(response.data.LastPR);
                    $(row).find('.clLowestPr').val(response.data.LowestPR);
                    $(row).find('.clPendingOrderQty').val(response.data.PendingPOQty);
                    $(row).find('.clQtyWithQc').val(response.data.QtyUnderQC);
                    $(row).find('.clQtyAvailable').val(response.data.Stock);
                }
            }
        })
    },
    get_rate: function () {
        var ItemID = clean($("#ItemID").val());
        var BatchType = $("#BatchType option:selected").text();
        //var IsInterCompany = clean($("#IsInterCompany").val());
        //if (IsInterCompany == 0)
        //    return;
        $.ajax({
            url: '/Purchase/PurchaseOrder/GetRateForInterCompany',
            dataType: "json",
            type: "GET",
            data: {
                ItemID: ItemID,
                BatchType: BatchType
            },
            success: function (response) {
                if (response.Status == "success") {
                    if (response.data >= 0) {
                        $("#Rate").val(response.data);
                        $("#txtValue").val(clean($("#Rate").val()) * clean($("#Qty").val()));
                    }
                    else {
                        $("#Rate").val(0);
                    }
                }
            }
        });
    },
    check_supplier: function () {
        var self = purchase_order;
        self.error_count = 0;
        self.error_count = self.validate_supplier();
        if (self.error_count > 0) {
            UIkit.modal($('#select-item')).hide();
        }

    },
    update_item_list: function () {
        item_list.fnDraw();
    },

    cancel_confirm: function () {
        var self = purchase_order
        app.confirm_cancel("Do you want to cancel? This can't be undone", function () {
            self.cancel();
        }, function () {
        })
    },

    open_clone: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).closest('tr').find('.ID').val();
        window.location = '/Purchase/PurchaseOrder/Clone/' + id;
    },
    confirm_suspend_po: function (e) {
        e.stopPropagation();
        var self = purchase_order;
        var id = $(this).closest('tr').find('.ID').val();
        app.confirm("Do you want to suspend? This can't be undone", function () {
            self.suspend_purchaseorder(id);
        });
    },
    is_item_supplied_by_supplier: function () {
        var self = purchase_order;
        var ItemList = "";
        $("#purchase-order-items-list tbody tr").each(function () {
            ItemList += ($(this).find('.ItemID').val()) + ',';
        });
        if (ItemList.length > 0) {
            $.ajax({
                url: '/Purchase/PurchaseOrder/IsItemSuppliedBySupplier',
                data: {
                    ItemLists: ItemList,
                    SupplierID: $("#SupplierID").val()
                },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, function (key, item) {
                        $("#purchase-order-items-list tbody tr").each(function () {
                            var i = $(this).find(".ItemID").val();
                            if (($("#purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status != "Eligible")) {
                                $(this).closest('tr').addClass('ineligible');
                            }
                            else if (($("#purchase-order-items-list tbody tr").find(".ItemID").val() == item.ItemID) && (item.Status == "Eligible")) {
                                $(this).closest('tr').removeClass('ineligible');
                            }
                        });
                    });
                    $("#purchase-order-items-list tbody tr .clRqQty").each(function () {
                        CalculateValueInGrid($(this));
                    });
                },
            });
        }
        item_list.fnDraw();
    },
    suspend_purchaseorder: function (id) {
        $.ajax({
            url: '/Purchase/PurchaseOrder/Suspend',
            dataType: "json",
            type: "GET",
            data: {
                ID: id,
                Table: "PurchaseOrder"
            },
            success: function (response) {
                if (response.Data == 1) {
                    app.show_notice("Purchase order suspended successfully");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('suspended');
                }
                if (response.Data == 0) {
                    app.show_error("Purchase order already processed");
                    $('input[value=' + id + ']').closest('tr').find('.btnsuspend').addClass('uk-hidden');
                    $('input[value=' + id + ']').closest('tr').addClass('processed');
                }
                if (response.Data == 2) {
                    app.show_error("Please cancel GRN before suspending purchase order");

                }
            }
        });
    },
    Load_All_DropDown: function () {
        $.ajax({
            url: '/Purchase/PurchaseOrder/GetDropdownVal',
            dataType: "json",
            type: "GET",
            success: function (response) {
                if (response.Status == "success") {
                    BatchTypeList = response.data;
                }
            }
        });
    },


    select_puchase_requisitions: function () {
        var self = purchase_order;
        self.error_count = self.validate_requsition();
        if (self.error_count != 0)
            return
        var UnProcPrList = [];
        $(".unPrTbody .rowUnPr").each(function () {
            if ($(this).find('.clChk .clChkItem').is(":checked")) {
                UnProcPrList.push($(this).find('.clId .clprIdItem').val())
            }
        });

        if ($(".poTbody .rowPR").length > 0 && UnProcPrList.length > 0) {
            app.confirm("Selected items will be removed", function () {
                purchase_order.get_po_items(UnProcPrList);
            })

        } else {
            purchase_order.get_po_items(UnProcPrList);
        }
    },
    get_po_items: function (UnProcPrList) {
        $("#purchase-order-items-list tbody .rowPR").each(function () {
            $(this).remove();
        });
        $("#PurchaseRequisitionIDS").val('');
        var PurchaseRequisitionIDS = [];
        if (UnProcPrList.length > 0) {
            $.ajax({
                url: '/Purchase/PurchaseOrder/GetPoTransByPrId',
                data: { PurchaseRequisitionIDList: UnProcPrList, SupplierID: $("#SupplierID").val() },
                type: "GET",
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, function (key, item) {
                        item.Value = 0;
                        if (PurchaseRequisitionIDS.indexOf(item.Code) == -1) {
                            PurchaseRequisitionIDS.push(item.Code);
                        }
                        purchase_order.AddProductToPrGrid("rowPR", item);
                    });
                    purchase_order.count_items();
                    $("#PurchaseRequisitionIDS").val(PurchaseRequisitionIDS.join(','));
                    //Calculate net amount
                    CalculateNetAmountValue();
                    setTimeout(function () { freeze_header.resizeHeader(); }, 100);

                },
            });
        }
    },
    Build_Select: function (options, selected) {
        var $select = '';
        var $select = $('<select></select>');
        var $option = '';

        // console.log(options);
        for (var i = 0; i < options.length; i++) {
            //alert(options[i].Text);
            if (options[i].ID == selected) {
                //$option.attr('selected', 'selected');
                $option = '<option selected value="' + options[i].ID + '">' + options[i].Name + '</option>';
            } else {
                $option = '<option value="' + options[i].ID + '">' + options[i].Name + '</option>';
            }


            $select.append($option);
        }

        return $select.html();

    },
    AddProductToPrGrid: function (PrRowClass, item) {
        var self = purchase_order;
        var existedHtml = '';
        var html = '';
        var BatchType;
        var SerialNo = $(".poTbody .rowPO").length + 1;
        if (typeof item.Rate == "undefined") {
            item.Rate = 0;
        }
        if (typeof item.GSTAmount == "undefined") {
            item.GSTAmount = 0;
        }
        BatchType = '<select class="md-input label-fixed clBatch ' + (item.FGCategoryID == 0 ? '' : 'enable') + '"' + (item.FGCategoryID == 0 ? 'disabled' : '') + ' >' + purchase_order.Build_Select(BatchTypeList, item.BatchTypeID) + '</select>'
        html ='<tr class="rowPO included ' + PrRowClass + '">' +
              '<td class="uk-text-center clPr">' + SerialNo + '</td>' +
              '<td class="uk-text-center checked chkValid" data-md-icheck><input type="checkbox" class="chkCheck"  checked/></td>' +
              '<td class="clItem">' + item.Name +
              '<input type="hidden" class="ItemID" value="' + item.ID + '" />' +
              '<input type="hidden" class="UnitID" value="' + item.UnitID + '" />' +
              '<input type="hidden" class="PrId" value="' + item.PurchaseRequisitionID + '" />' +
              '<input type="hidden" class="PrTransId" value="' + item.PRTransID + '" />' +
              '<td class="clUnit">' + item.Unit + '</td>' +
              '<td class="clQty" ><input type="text"  class="md-input mask-qty clRqQty included " value="' + item.Qty + '" /></td>' +
              '<td class="clGstPerscnt"><input type="text" class="md-input mask-qty txtClGSTPer included"  value="' + item.GSTPercentage + '" disabled /></td>' +
              '<td ><input type="text"  class="md-input mask-currency clLastPr" value="' + item.LastPR + '" disabled /></td>' +
              '<td><input type="text"  class="md-input mask-currency clLowestPr" value="' + item.LowestPR + '" disabled /></td>' +
              '<td ><input type="text"  class="md-input mask-currency clPendingOrderQty" value="' + item.PendingOrderQty + '" disabled /></td>' +
              '<td><input type="text"  class="md-input mask-currency clQtyAvailable" value="' + item.QtyAvailable + '" disabled /></td>' +
              '</tr>';
        var $html = $(html);
        app.format($html);
        $("#purchase-order-items-list tbody").append($html);
        CalculateGSTInsideGrid($("#purchase-order-items-list tbody tr").last());
        self.get_po_item_details($("#purchase-order-items-list tbody tr").last());
    },
    print_preview: function () {
        $("#screen").addClass("uk-hidden");
        $("#print").removeClass("uk-hidden");
        $('.print-actions').removeClass('uk-hidden');
        $('.hidden-print-preview').addClass('uk-hidden');
    },
    close_print_preview: function () {
        $("#screen").removeClass("uk-hidden");
        $("#print").addClass("uk-hidden");
        $('.print-actions').addClass('uk-hidden');
        $('.hidden-print-preview').removeClass('uk-hidden');
    },
    close_preview: function () {
        $("#screen").removeClass("uk-hidden");
        $("#print").addClass("uk-hidden");
    },



    print: function () {
        window.print();
    },
    select_supplier: function () {
        var self = purchase_order;
        var radio = $('#select-supplier tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Location = $(row).find(".Location").text().trim();
        var StateID = $(row).find(".StateID").val();
        var Email = $(row).find(".Email").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PaymentDays = $(row).find(".PaymentDays").val();
        var IsInterCompany = $(row).find(".IsInterCompany").val();
        var InterCompanyLocationID = clean($(row).find(".InterCompanyLocationID").val());
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        var GSTID = clean($("#GSTID").val());
        if (IsInterCompany == 1) {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", true);
            $(".intercompanySupplierlocation").removeClass("uk-hidden");
            $(".supplierLocation").addClass("uk-hidden");
            self.get_intercompany_location();
        }
        else {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", false);
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }
        $("#SupplierName").val(Name);
        $("#SupplierLocation").val(Location);
        $("#SupplierID").val(ID);
        $("#StateId").val(StateID);
        $("#InterCompanyLocationID").val(InterCompanyLocationID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        $("#DDLPaymentWithin option:contains(" + PaymentDays + ")").attr("selected", true);
        $("#IsInterCompany").val(IsInterCompany);
        $("#Email").val(Email);
        $("#StateId").val() == $("#ShippingStateId").val()
        {

        }
        var checkboxes = "";
        var $checkboxes = $("<div></div>");
        $(".checkbox-container").html('');
        checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
        $checkboxes.append(checkboxes);
        app.format($checkboxes);
        $(".checkbox-container").eq(0).html($checkboxes);
        $("#ItemName").focus();
        self.is_item_supplied_by_supplier();
        self.get_supplier_description();
    },
    get_supplier_description: function () {
        var self = purchase_order;

        var SupplierID;
        $(".Description-div").removeClass("uk-hidden");

        SupplierID = clean($('#SupplierID').val());

        supplier.get_description(SupplierID, "Purchase");


    },
    get_details: function () {
        var self = purchase_order;
        var row = $(this).closest('tr');
        var ItemID;
        $(".Description-div").removeClass("uk-hidden");

        if (row.length == 0) {
            ItemID = clean($('#ItemID').val());
        }
        else {
            ItemID = clean($(row).find('.ItemID').val());
        }

        Item.get_description(ItemID, "Purchase");


    },
    set_state_id: function () {
        var state_id = $(this).find("option:selected").data("state-id");
        $("#ShippingStateId").val(state_id);
        CalculateGSTOutsideTheGrid();
    },
    cancel: function () {
        app.confirm("Do you want to cancel? This can't be undone", function () {
            var PurchaseOrderID = $("#ID").val();
            $.ajax({
                url: '/Purchase/PurchaseOrder/Cancel',
                data: {
                    PurchaseOrderID: PurchaseOrderID,
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Purchase order cancelled successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/PurchaseOrder/IndexV4";
                        }, 1000);
                    } else {
                        app.show_error("Failed to cancel purchase order");
                    }
                }
            });
        })
    },
    set_supplier_details: function (event, item) {   // on select auto complete item

        var self = purchase_order;
        $("#SupplierLocation").val(item.location);
        $("#SupplierID").val(item.id);
        $("#StateId").val(item.stateId);
        $("#IsGSTRegistred").val(item.isGstRegistered);
        $("#IsInterCompany").val(item.isintercompany);
        $("#Email").val(item.email);

        $("#InterCompanyLocationID").val(item.interCompanyLocationid);
        $("#DDLPaymentWithin option:contains(" + item.paymentDays + ")").attr("selected", true);
        $("#SupplierReferenceNo").focus();
        var GSTID = clean($("#GSTID").val());
        if (item.isintercompany == 1) {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", true);
            $(".intercompanySupplierlocation").removeClass("uk-hidden");
            $(".supplierLocation").addClass("uk-hidden");
            self.get_intercompany_location();
        }
        else {
            $("#GST").val(GSTID);
            $("#GST").prop("disabled", false);
            $(".supplierLocation").removeClass("uk-hidden");
            $(".intercompanySupplierlocation").addClass("uk-hidden");
        }
        //  $("#SupplierID").trigger('change');
        var checkboxes = "";
        var $checkboxes = $("<div></div>");
        $(".checkbox-container").html('');
        checkboxes = "<input type='checkbox' data-md-icheck class='select-all-item' checked>";
        $checkboxes.append(checkboxes);
        app.format($checkboxes);
        $(".checkbox-container").eq(0).html($checkboxes);
        $("#ItemName").focus();
        self.is_item_supplied_by_supplier();
        self.get_supplier_description();
    },
    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/getSupplierForAutoComplete',
            data: {
                term: $('#SupplierName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_intercompany_location: function (release) {
        $.ajax({
            url: '/Masters/Location/getInterCompanyLocation',
            data: {
                LocationID: clean($('#InterCompanyLocationID').val())
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $("#LocationID").html("");
                var html = "<option value >Select</option>";
                $.each(data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                $("#LocationID").append(html);
            }
        });
    },
    set_item_details: function (event, item) {   // on select auto complete item
        var self = purchase_order;
        $("#ItemName").val(item.label);
        $("#ItemID").val(item.id);
        $("#Rate").val('');
        $("#Unit").val(item.unit);
        $("#LastPr").val(item.lastPr);
        $("#LowestPr").val(item.lowestPr);
        $("#PendingOrderQty").val(item.pendingOrderQty);
        $("#QtyWithQc").val(item.qtyWithQc);
        $("#QtyAvailable").val(item.qtyAvailable);
        $("#GSTPercentage").val(item.gstPercentage);
        $("#CategoryID").val(item.itemCategory);
        $("#PrimaryUnit").val(item.primaryUnit);
        $("#PrimaryUnitID").val(item.primaryUnitId);
        $("#PurchaseUnit").val(item.purchaseUnit);
        $("#PurchaseUnitID").val(item.purchaseUnitId);
        if ($("#CategoryID").val() > 0) {
            $("#select_batch_type").removeClass("uk-hidden").addClass('visible');
        }
        else {
            $("#select_batch_type").addClass("uk-hidden").removeClass('visible');
        }
        self.get_units();
        self.get_rate();
        self.get_details();
        $("#Qty").focus();
    },
    select_item: function () {
        var self = purchase_order;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var PrimaryUnit = $(row).find(".PrimaryUnit").val();
        var PrimaryUnitID = $(row).find(".PrimaryUnitID").val();
        var PurchaseUnit = $(row).find(".PurchaseUnit").val();
        var PurchaseUnitID = $(row).find(".PurchaseUnitID").val();
        var lastPr = $(row).find(".lastPr").val();
        var lowestPr = $(row).find(".lowestPr").val();
        var Category = $(row).find(".ItemCategory").val();
        var pendingOrderQty = $(row).find(".pendingOrderQty").val();
        var qtyWithQc = $(row).find(".qtyWithQc").val();
        var qtyAvailable = $(row).find(".qtyAvailable").val();
        var gstPercentage = $(row).find(".gstPercentage").val();
        var itemCategoryID = $(row).find(".finishedGoodsCategoryID").val();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        $("#Rate").val('');
        $("#PrimaryUnit").val(PrimaryUnit);
        $("#PrimaryUnitID").val(PrimaryUnitID);
        $("#PurchaseUnit").val(PurchaseUnit);
        $("#PurchaseUnitID").val(PurchaseUnitID);
        $("#LastPr").val(lastPr);
        $("#LowestPr").val(lowestPr);
        $("#PendingOrderQty").val(pendingOrderQty);
        $("#QtyWithQc").val(qtyWithQc);
        $("#QtyAvailable").val(qtyAvailable);
        $("#GSTPercentage").val(gstPercentage);
        $("#CategoryID").val(itemCategoryID);
        if ($("#CategoryID").val() > 0) {
            $("#select_batch_type").removeClass("uk-hidden").addClass('visible');
        }
        else {
            $("#select_batch_type").addClass("uk-hidden").removeClass('visible');
        }
        $("#Qty").focus();
        self.get_units();
        self.get_rate();
        self.get_details();
        UIkit.modal($('#select-item')).hide();

    },
    get_units: function () {
        var self = purchase_order;
        $("#UnitID").html("");
        var html;
        html += "<option value='" + $("#PurchaseUnitID").val() + "'>" + $("#PurchaseUnit").val() + "</option>";
        html += "<option value='" + $("#PrimaryUnitID").val() + "'>" + $("#PrimaryUnit").val() + "</option>";
        $("#UnitID").append(html);
    },
    get_item_details: function (release) {

        $.ajax({
            url: '/Purchase/PurchaseOrder/getProductList',
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
    get_purchase_category: function () {
        var item_category_id = $("#DDLItemCategory").val();
        if (item_category_id == null || item_category_id == "") {
            item_category_id = 0;
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
    remove_file: function () {
        $(this).parent('li').remove();
        var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
        $('#file-count').text(file_count + " File(s)");
    },
    remove_quotation: function () {
        $(this).closest('span').remove();
    },
    remove_item: function () {
        $(this).closest('tr').remove();
        purchase_order.count_items();
        freeze_header.resizeHeader();
    },
    include_item: function () {
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('disabled');
        purchase_order.count_items();
        CalculateGSTInsideGrid($(this).closest('tr'));
        CalculateGSTOutsideTheGrid();
        CalculateNetAmountValue();
    },
    error_count: 0,
    validate_item: function () {
        var self = purchase_order;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_supplier: function () {
        var self = purchase_order;
        if (self.rules.on_item.length > 0) {
            return form.validate(self.rules.on_item);
        }
        return 0;
    },
    count_items: function () {
        var count = $('#purchase-order-items-list tbody').find('input.chkCheck:checked').length;
        $('#item-count').val(count);
    },
    validate_form: function () {
        var self = purchase_order;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    validate_requsition: function () {
        var self = purchase_order;
        if (self.rules.on_requsition.length > 0) {
            return form.validate(self.rules.on_requsition);
        }
        return 0;
    },
    validate_draft: function () {
        var self = purchase_order;
        if (self.rules.on_draft.length > 0) {
            return form.validate(self.rules.on_draft);
        }
        return 0;
    },

    rules: {
        on_item: [

           {
               elements: "#SupplierID",
               rules: [
                   { type: form.non_zero, message: "Please select supplier" },
                   { type: form.required, message: "Please select supplier" },
               ]
           },

        ],
        on_requsition: [
       {
           elements: "#GST",
           rules: [
              { type: form.required, message: "Please select GST type" },
           ]
       },
        ],
        on_add: [
            {
                elements: "#ItemID",
                rules: [
                    { type: form.required, message: "Please select an item" },
                     {
                         type: function (element) {
                             var error = false;
                             var count = 0;
                             var itemid = clean($(element).val());
                             var isintercompany = clean($('#IsInterCompany').val());

                             if (isintercompany == 1) {
                                 $("#purchase-order-items-list tbody tr .ItemID[value='" + itemid + "']").each(function () {
                                     count = 1;

                                 });
                             }
                             return count == 0

                         }, message: "Item already exist"
                     },
                ]
            },
            {
                elements: "#SupplierID",
                rules: [
                    { type: form.non_zero, message: "Please select supplier" },
                    { type: form.required, message: "Please select supplier" },
                ]
            },
            {
                elements: "#Qty",
                rules: [
                    { type: form.required, message: "Please enter quantity" },
                    { type: form.numeric, message: "Please enter valid quantity" },
                    { type: form.positive, message: "Please enter positive quantity" },
                ]
            },
            {
                elements: "#Rate",
                rules: [
                    { type: form.required, message: "Please enter rate" },
                    { type: form.positive, message: "Please enter positive rate" },
                     { type: form.non_zero, message: "Please enter rate" },
                ]
            },
            {
                elements: "#GST",
                rules: [
                   { type: form.required, message: "Please select GST type" },
                ]
            },
            {
                elements: ".visible #BatchType:visible",
                rules: [
                    { type: form.required, message: "Please select batch type" },

                ]
            },
        ],
        on_draft: [
             {
                 elements: "#GST",
                 rules: [
                    { type: form.required, message: "Please select GST type" },
                 ]
             },
                      {
                          elements: "#LocationID",
                          rules: [

                     {
                         type: function (element) {
                             var error = false;
                             var isintercompany = clean($('#IsInterCompany').val());
                             var locationid = clean($('#LocationID').val());
                             if (isintercompany == 1 && locationid == 0)
                                 error = true;
                             return !error;
                         }, message: 'Please select supplier location'
                     },
                          ]
                      },
            {
                elements: "#item-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one item" },
                    { type: form.required, message: "Please add atleast one item" },
                    {
                        type: function (element) {
                            var error = false;
                            var count = $("#purchase-order-items-list tbody tr.included.ineligible").length;

                            if (count > 0)
                                error = true;
                            return !error;
                        }, message: 'Some of the items in grid are not supplied by supplier please select another supplier'
                    },
                ]
            },
                    {
                        elements: "#DeliveryWithin",
                        rules: [
                            { type: form.required, message: "Please enter delivery within days" }
                        //{ type: form.non_zero, message: "Please enter delivery within days" },
                        //{ type: form.positive, message: "Please enter positive delivery within days" },
                        //{ type: form.numeric, message: "Please enter valid delivery within days" },
                        ]
                    },
            {
                elements: "#PaymentModeID",
                rules: [
                    { type: form.required, message: "Please select payment mode" },
                     {
                         type: function (element) {
                             var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                             var NetAmt = clean($("#NetAmt").val());
                             var PaymentMode = $("#PaymentModeID option:selected").text();
                             var error = false;
                             if (CashPaymentLimit < NetAmt && PaymentMode.toLowerCase() == "cash") {
                                 error = true;
                             }
                             return !error;
                         }, message: 'Please select another payment mode'
                     },
                ]
            },
                {
                    elements: ".clBatch.enable:visible",
                    rules: [
                        {
                            type: function (element) {
                                var batchid = clean($(element).val());
                                var error = false;
                                if (batchid == 0) {
                                    error = true;
                                }
                                return !error;
                            }, message: 'Please select batch type'
                        },
                    ]
                },


                {
                    elements: "#NetAmt",
                    rules: [
                         { type: form.positive, message: "Invalid net amount" },
                    ]
                },
                {
                    elements: ".clRqQty.included",
                    rules: [
                        { type: form.required, message: "Please enter quantity" },
                        { type: form.positive, message: "Please enter positive quantity" },
                    ]
                },

                {
                    elements: "#FreightAmt",
                    rules: [
                         { type: form.positive, message: "Please enter positive freight amount" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());

                       if (NetAmt < sum)
                           error = true;


                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges should be less than net amount'
               },

                    ]
                },
                //{
                //    elements: ".txtClGSTAmt.included",
                //    rules: [
                //        { type: form.required, message: "Please enter GST Amount" },
                //        { type: form.positive, message: "Please enter positive value for txtClGSTAmt" },
                //    ]
                //},
                {
                    elements: "#OtherCharges",
                    rules: [
                         { type: form.positive, message: "Please enter positive other charges" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());

                       if (NetAmt < sum)
                           error = true;


                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
               },

                    ]
                },
                {
                    elements: "#AdvanceAmount",
                    rules: [
                         { type: form.positive, message: "Please enter positive advance amount" },

                            {
                                type: function (element) {
                                    var error = false;
                                    var advance = clean($('#AdvanceAmount').val());

                                    var NetAmt = clean($('#NetAmt').val());

                                    if (advance > NetAmt)
                                        error = true;


                                    return !error;
                                }, message: 'Advance amount exceeds net amount'
                            },
                    ]
                },
                {
                    elements: "#PackingShippingCharge",
                    rules: [
                         { type: form.positive, message: "Please enter positive shipping charges" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());
                       if (NetAmt < sum)
                           error = true;
                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
               },
                    ]
                },
        ],
        on_submit: [
                   {
                       elements: ".clBatch.enable:visible",
                       rules: [
                               {
                                   type: function (element) {
                                       var batchid = clean($(element).val());
                                       var error = false;
                                       if (batchid == 0) {
                                           error = true;
                                       }
                                       return !error;
                                   }, message: 'Please select batch type'
                               },
                       ]
                   },
                {
                    elements: "#item-count",
                    rules: [
                        { type: form.non_zero, message: "Please add atleast one item" },
                        { type: form.required, message: "Please add atleast one item" },
                        {
                            type: function (element) {
                                var error = false;
                                var count = $("#purchase-order-items-list tbody tr.included.ineligible").length;

                                if (count > 0)
                                    error = true;
                                return !error;
                            }, message: 'Some of the items in grid are not supplied by selected supplier, please select another supplier'
                        },
                    ]
                },

                {
                    elements: "#GST",
                    rules: [
                       { type: form.required, message: "Please select GST type" },
                    ]
                },
                {
                    elements: "#SupplierID",
                    rules: [
                        { type: form.non_zero, message: "Please select supplier", alt_element: "#SupplierName" },
                        { type: form.required, message: "Please select supplier", alt_element: "#SupplierName" },
                    ]
                },
                {
                    elements: "#ShippingToId",
                    rules: [
                        { type: form.required, message: "Please select shipping location" },
                    ]
                },
                {
                    elements: "#DDLBillTo",
                    rules: [
                        { type: form.required, message: "Please select billing location" },
                    ]
                },
                {
                    elements: "#PaymentModeID",
                    rules: [
                        { type: form.required, message: "Please select payment mode" },
                        {
                            type: function (element) {
                                var CashPaymentLimit = clean($("#CashPaymentLimit").val());
                                var NetAmt = clean($("#NetAmt").val());
                                var PaymentMode = $("#PaymentModeID option:selected").text();
                                var error = false;
                                if (CashPaymentLimit < NetAmt && PaymentMode.toLowerCase() == "cash") {
                                    error = true;
                                }
                                return !error;
                            }, message: 'Please select another payment mode'
                        },
                    ]
                },
                {
                    elements: "#DDLPaymentWithin",
                    rules: [
                        { type: form.required, message: "Please select payment within days" },
                    ]
                },
                {
                    elements: "#DeliveryWithin",
                    rules: [
                        { type: form.required, message: "Please enter delivery within days" },
                        //{ type: form.positive, message: "Please enter positive  delivery within days" },
                        //{ type: form.numeric, message: "Please enter valid delivery within days" },
                        //{ type: form.non_zero, message: "Please enter delivery within days" },

                        //{
                        //    type: function (element) {
                        //        var error = false;
                        //        var deliverywithin = clean($('#DeliveryWithin').val());
                        //        if (deliverywithin >= 365)
                        //            error = true;
                        //        return !error;
                        //    }, message: 'Delivery within exceeds 365 days'
                        //},
                    ]
                },
                {
                    elements: "#NetAmt",
                    rules: [
                         { type: form.positive, message: "Invalid net amount" },
                    ]
                },
                {
                    elements: ".clRqQty.included",
                    rules: [
                        { type: form.required, message: "Please enter quantity" },
                        { type: form.positive, message: "Please enter positive quantity" },
                    ]
                },
                {
                    elements: ".txtclRate.included",
                    rules: [
                        { type: form.required, message: "Please enter rate" },
                        { type: form.positive, message: "Please enter positive rate" },
                        { type: form.non_zero, message: "Please enter rate" },
                    ]
                },
                {
                    elements: ".txtclValue.included",
                    rules: [
                        { type: form.required, message: "Please enter value" },
                        { type: form.positive, message: "Please enter positive value" },
                        { type: form.non_zero, message: "Please enter value" },
                    ]
                },
                {
                    elements: "#FreightAmt",
                    rules: [
                         { type: form.positive, message: "Please enter positive freight mmount" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());
                       if (NetAmt < sum)
                           error = true;
                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
               },
                    ]
                },
                {
                    elements: ".txtClGSTAmt.included",
                    rules: [
                        { type: form.required, message: "Please enter GST amount" },
                        { type: form.positive, message: "Please enter positive GST amount" },
                    ]
                },
                {
                    elements: "#OtherCharges",
                    rules: [
                         { type: form.positive, message: "Please enter positive other charges" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());
                       if (NetAmt < sum)
                           error = true;
                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
               },
                    ]
                },
                {
                    elements: "#AdvanceAmount",
                    rules: [
                         { type: form.positive, message: "Please enter positive advance amount" },
                        {
                            type: function (element) {
                                var error = false;
                                var advance = clean($('#AdvanceAmount').val());

                                var NetAmt = clean($('#NetAmt').val());

                                if (advance > NetAmt)
                                    error = true;


                                return !error;
                            }, message: 'Advance amount exceeds net amount'
                        },

                    ]
                },
                {
                    elements: "#PackingShippingCharge",
                    rules: [
                         { type: form.positive, message: "Please enter positive shipping charges" },
               {
                   type: function (element) {
                       var error = false;
                       var FrightAmt = clean($('#FreightAmt').val());
                       var OtherCharges = clean($('#OtherCharges').val());
                       var ShippingCharges = clean($('#PackingShippingCharge').val());
                       var sum = FrightAmt + OtherCharges + ShippingCharges;
                       var NetAmt = clean($('#NetAmt').val());
                       if (NetAmt < sum)
                           error = true;
                       return !error;
                   }, message: 'Sum of freight,other charges and packing charges exceeds net amount'
               },
                    ]
                },

                  {
                      elements: "#LocationID",
                      rules: [

                 {
                     type: function (element) {
                         var error = false;
                         var isintercompany = clean($('#IsInterCompany').val());
                         var locationid = clean($('#LocationID').val());
                         if (isintercompany == 1 && locationid == 0)
                             error = true;
                         return !error;
                     }, message: 'Please select supplier location'
                 },
                      ]
                  },
                {
                    elements: ".clBatch.enable:visible",
                    rules: [
                            { type: form.required, message: "Please select batch type" },
                    ]
                },
        ]
    },
    selected_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },
    other_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            //percent = Math.ceil(percent);
            // bar.css("width", percent + "%").text(percent + "%");
        },
        complete: function (response, xhr) {
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                var dropdown = $("#other-quotation-list .uk-nav-dropdown");
                width = $('#other-quotation-list').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        dropdown.append("<li class='file-list'>"
                        + "<a class='remove-file'>X</a>"
                        + "<span data-id='" + record.ID + "' style='width:" + width + "px;' class='view-file' data-url='" + record.URL + "' data-path='" + record.Path + "'>"
                        + record.Name
                        + "</span>"
                        + "</li>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            // app.show_notice("Uploaded");
            //console.log(response);

            var file_count = $("#other-quotation-list .uk-nav-dropdown li").length;
            $('#file-count').text(file_count + " File(s)");
        }
    }
};

purchase_orderCRUD = {

    purchaseOCreateAndUpdate: function () {

        //Add products to pr grid
        $("#btnAddProduct").click(function () {

            var self = purchase_order;
            self.error_count = 0;
            var batchType = "";
            var batchTypeID = 0;

            self.error_count = self.validate_item();
            if (self.error_count == 0) {

                batchTypeID = $("#BatchType").val();
                batchType = batchTypeID != '' || batchTypeID != null ? $("#BatchType option:selected").text() : "";


                var item = {
                    Code: "",
                    Name: $("#ItemName").val(),
                    ID: $("#ItemID").val(),
                    Qty: clean($("#Qty").val()),
                    Unit: $("#UnitID option:selected").text(),
                    UnitID: $("#UnitID option:selected").val(),
                    Rate: 0,
                    Value: clean($("#txtValue").val()),
                    LastPR: clean($("#LastPr").val()),
                    LowestPR: ($("#LowestPr").val()),
                    PendingOrderQty: clean($("#PendingOrderQty").val()),
                    QtyUnderQC: clean($("#QtyWithQc").val()),
                    QtyAvailable: clean($("#QtyAvailable").val()),
                    Remarks:"",
                    GSTPercentage: clean($('#GSTPercentage').val()),
                    GSTAmount: 0,
                    PurchaseRequisitionID: "",
                    PRTransID: "",
                    BatchType: batchType,
                    BatchTypeID: batchTypeID,
                    FGCategoryID: $("#CategoryID").val()
                };
                self.AddProductToPrGrid("", item);
                freeze_header.resizeHeader();
                //Calculate net amount
                CalculateNetAmountValue();

                purchase_order.count_items();
                clearItemSelectControls();
                $("#ItemName").focus();
            }
        });
        function clearItemSelectControls() {
            $("#ItemName").val('');
            $("#ItemID").val('');
            $("#Qty").val('');
            $("#Rate").val('');
            $("#UnitID").val('');
            $("#LastPr").val('');
            $("#LowestPr").val('');
            $("#PendingOrderQty").val('');
            $("#QtyOrdered").val('');
            $("#QtyAvailable").val('');
            $("#QtyWithQc").val('');
            $("#txtValue").val('');
            $("#ItemRemarks").val('');
            $("#BatchType").val('');
            $("#select_batch_type").addClass("uk-hidden").removeClass('visible');

        }
        $(".btnSelectReqstion").click(function () {
            if ($("#SupplierID").val() > 0) {
                UIkit.modal('#select_pr', { center: false }).show();
            } else {
                app.show_error("Please select supplier")
            }
        });

        $(".btnSaveAndMail").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var _Master = GetPurchaseOrderForSave();
                var _Trans = GetProductFromGrid();
                var IsSendMail = 1
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })
                
            }
        }); 
        $(".btnSavePO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var _Master = GetPurchaseOrderForSave();
                var _Trans = GetProductFromGrid();
                var IsSendMail = 0
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })
            }
        });
        $(".btnSaveASDraftPO").click(function () {
            var self = purchase_order;
            self.error_count = 0;
            self.error_count = self.validate_draft();
            if (self.error_count == 0) {
                var _Master = GetPurchaseOrderForSave(true);
                var _Trans = GetProductFromGrid();
                var IsSendMail = 0
                app.confirm_cancel("Do you want to save?", function () {
                    SavePO(_Master, _Trans, IsSendMail);
                }, function () {
                })
            }
        });
        function SavePO(_Master, _Trans, IsSendMail) {
            var self = purchase_order;
            var url;
            if (_Master.IsDraft == true) {
                url = '/Purchase/PurchaseOrder/SaveAsDraft';
            }
            else {
                url = '/Purchase/PurchaseOrder/Save';
            }
            $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'none' });
            $.ajax({
                url: url,
                data: { PO: _Master, Details: _Trans },
                dataType: "json",
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.Status == "success") {
                        $("#ID").val(data.Data.ID);
                        if (IsSendMail == 1)
                        {
                            self.send_mail();
                        }
                        app.show_notice("Purchase order saved successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/PurchaseOrder/IndexV4";
                        }, 1000);
                    } else {
                        if (typeof data.data[0].ErrorMessage != "undefined")
                            app.show_error(data.data[0].ErrorMessage);
                        //app.show_error(data.Message);
                        $(".btnSavePO, .btnSaveASDraftPO ").css({ 'display': 'block' });
                    }
                },
            });
        }

        function GetPurchaseOrderForSave(IsDraft) {
            var ID;
            if (typeof IsDraft === "undefined" || IsDraft === null) {
                IsDraft = false;
            }
            var inCludGST = false;
            var Extra = false;
            if (clean($('#GST').val()) == 1) {
                inCludGST = true;
            } else {
                Extra = true;
            }

            var OtherQ = [];
            $("#other-quotation-list span.view-file").each(function () {
                OtherQ.push($(this).data('id'));
            });
            if ($("#IsClone").val() == "True") {
                ID = 0;
            }
            else {
                ID = $("#ID").val();
            }
            var Model = {
                ID: ID,
                PurchaseOrderNo: $("#PurchaseOrderNo").val(),
                PurchaseOrderDate: $("#PurchaseOrderDate").val(),
                SupplierID: $("#SupplierID").val(),
                AdvancePercentage: clean($("#AdvanceAmount").val()),
                AdvanceAmount: clean($("#AdvanceAmount").val()),
                PaymentModeID: $("#PaymentModeID option:selected").val(),
                ShippingAddressID: $("#ShippingToId").val(),
                BillingAddressID: $("#DDLBillTo").val(),
                InclusiveGST: inCludGST,
                GstExtra: Extra,
                SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
                OtherQuotationIDS: OtherQ.join(","),
                DeliveryWithin: $("#DeliveryWithin").val(),
                PaymentWithinID: $("#DDLPaymentWithin").val(),
                SGSTAmt: 0,
                CGSTAmt: 0,
                IGSTAmt: 0,
                FreightAmt: clean($("#FreightAmt").val()),
                OtherCharges: clean($("#OtherCharges").val()),
                PackingShippingCharge: clean($("#PackingShippingCharge").val()),
                NetAmt: 0,
                DaysToSupply: $("#DeliveryWithin").val(),
                Remarks: $("#Remarks").val(),
                SupplierReferenceNo: "",
                TermsOfPrice: $("#TermsOfPrice").val(),
                IsDraft: IsDraft,
                IsGSTRegistred: $("#IsGSTRegistred").val(),
                SalesOrderLocationID: $("#LocationID").val()
            }
            return Model;
        }
        function GetProductFromGrid() {
            var ProductsArray = [];
            $("#purchase-order-items-list tbody tr.included").each(function () {
                var ItemID = clean($(this).find('.clItem  .ItemID').val());
                var PurchaseReqID = clean($(this).find('.PrId').val());
                var PRTransID = clean($(this).find('.PrTransId').val());
                var Quantity = clean($(this).find('.clQty  .clRqQty').val());
                var Rate = 0;
                var Amount = 0;
                var NetAmount =0;
                var Remarks ="";

                var LastPurchaseRate = clean($(this).find('.clLastPr').val());
                var LowestPR = clean($(this).find('.clLowestPr').val());
                var QtyInQC = clean($(this).find('.clQtyWithQc').val());
                var QtyOrdered = clean($(this).find('.clPendingOrderQty').val());
                var QtyAvailable = clean($(this).find('.clQtyAvailable').val());
                var UnitID = clean($(this).find('.UnitID').val());
                var Tot_GST_persc = clean($(this).find('.clGstPerscnt  .txtClGSTPer').val());
                var Tot_GST_Amt = 0;
                var QtyMet = 0;
                var ExpDate = $("#PurchaseOrderDate").val();
                var SGSTPercent = Tot_GST_persc / 2;
                var CGSTPercent = Tot_GST_persc / 2;
                var IGSTPercent = Tot_GST_persc;

                var SGSTAmt = 0;
                var CGSTAmt = 0;
                var IGSTAmt = 0;
                var BatchTypeID = clean($(this).find('.clBatch').val());

                if ($("#StateId").val() == $("#ShippingStateId").val()) {
                    SGSTAmt = Tot_GST_Amt / 2;
                    CGSTAmt = Tot_GST_Amt / 2;
                } else {
                    IGSTAmt = Tot_GST_Amt;
                }

                ProductsArray.push({
                    ItemID: ItemID,
                    PurchaseReqID: PurchaseReqID,
                    PRTransID: PRTransID,
                    Quantity: Quantity,
                    Rate: Rate,
                    Amount: Amount,
                    NetAmount: NetAmount,
                    SGSTPercent: SGSTPercent,
                    CGSTPercent: CGSTPercent,
                    IGSTPercent: IGSTPercent,
                    SGSTAmt: SGSTAmt,
                    CGSTAmt: CGSTAmt,
                    IGSTAmt: IGSTAmt,
                    Remarks: Remarks,
                    LastPurchaseRate: LastPurchaseRate,
                    LowestPR: LowestPR,
                    QtyInQC: QtyInQC,
                    QtyAvailable: QtyAvailable,
                    QtyOrdered: QtyOrdered,
                    BatchTypeID: BatchTypeID,
                    UnitID: UnitID,
                    ExpDate: ExpDate,
                    QtyMet: QtyMet,
                    BatchNo: "",
                    GSTPercentage: 0,
                    MRP: 0

                });

            })
            return ProductsArray;
        }
    }
};
//Common Calcultions

//calculate GST outside the grid
function CalculateGSTOutsideTheGrid() {
    var TotalGSTAmount = 0;
    $(".poTbody .rowPO.included").each(function () {
        var obj = clean($(this).find(".clGstAmount .txtClGSTAmt").val());
        TotalGSTAmount = TotalGSTAmount + obj;
    });
    //if ($("#StateId").val() == $("#ShippingStateId").val()) {
    //    $("#SGSTAmt").val(TotalGSTAmount / 2);
    //    $("#CGSTAmt").val(TotalGSTAmount / 2);
    //    $("#IGSTAmt").val(0);
    //} else {
    //    $("#SGSTAmt").val(0);
    //    $("#CGSTAmt").val(0);
    //    $("#IGSTAmt").val(TotalGSTAmount);
    //}
}

function CalculateGST(e) {
    var Rate = clean(e.find(".clRate .txtclRate").val());
    var Qty = clean(e.find(".clQty .clRqQty").val());
    var percent = clean(e.find(".clGstPerscnt .txtClGSTPer").val());
    if ($("#IsGSTRegistred").val().toLowerCase() == "true") {
        if ($('#GST').val() == 1) {

            var vRate = Rate * 100 / (100 + percent);
            var GstAmount = (Rate - vRate) * Qty;
            var Total = (vRate * Qty) + GstAmount;

            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else if ($('#GST').val() == 2) {
            var vRate = Rate;
            var GstAmount = Rate * Qty * percent / 100;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else {
            var vRate = Rate;
            var GstAmount = 0;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        }
    } else {
        e.find(".clGstAmount .txtClGSTAmt").val(0);
        e.find(".clTotal").val(Rate * Qty);
    }
}

//CalculateGST Inside the grid
function CalculateGSTInsideGrid(e) {
    var Rate = clean(e.find(".clRate .txtclRate").val());
    var Qty = clean(e.find(".clQty .clRqQty").val());
    var percent = clean(e.find(".clGstPerscnt .txtClGSTPer").val());
    if ($("#IsGSTRegistred").val().toLowerCase() == "true") {
        if ($('#GST').val() == 1) {

            var vRate = Rate * 100 / (100 + percent);
            var GstAmount = (Rate - vRate) * Qty;
            var Total = (vRate * Qty) + GstAmount;

            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else if ($('#GST').val() == 2) {
            var vRate = Rate;
            var GstAmount = Rate * Qty * percent / 100;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        } else {
            var vRate = Rate;
            var GstAmount = 0;
            var Total = (Rate * Qty) + GstAmount;
            e.find(".clGstAmount .txtClGSTAmt").val(GstAmount);
            e.find(".clTotal").val(Total);
        }
    } else {
        e.find(".clGstAmount .txtClGSTAmt").val(0);
        e.find(".clTotal").val(Rate * Qty);
    }
    CalculateGSTOutsideTheGrid();
    CalculateNetAmountValue();
}

function CalculateNetAmountValue() {
    var TotalProductAmount = 0;
    $("#purchase-order-items-list tbody tr.included").each(function () {
        TotalProductAmount = TotalProductAmount + clean($(this).find(".clTotal").val());
    });
    var FreightAmt = clean($("#FreightAmt").val());
    var OtherCharges = clean($("#OtherCharges").val());
    var PackingShippingCharge = clean($("#PackingShippingCharge").val());
    var NetAmt = TotalProductAmount + FreightAmt + OtherCharges + PackingShippingCharge;
    $("#NetAmt").val(NetAmt);
}

function CalculateValueInGrid(parent) {
    e = parent.closest('tr');
    var Rate = clean(e.find(".clRate .txtclRate").val());
    var Qty = clean(e.find(".clQty .clRqQty").val());
    e.find(".clValue .txtclValue").val(Rate * Qty);
    CalculateGSTInsideGrid(e);
}


purchase_order_CalculateEvents = {
    calculations: function () {
        $(document).on('keyup', '.txtClGSTPer', function () {
            CalculateGSTInsideGrid($(this).closest('tr'));
        });
        $(document).on('change', '.txtClGSTPer', function () {
            CalculateGSTInsideGrid($(this).closest('tr'));
        });
        $('#GST').on('change', function () {
            if ($(this).val() == null) {
                return;
            }
            $("#purchase-order-items-list tbody tr .clRqQty").each(function () {
                CalculateValueInGrid($(this));
            });
        })

        $(document).on('keyup', '.clRqQty , .txtclRate', function () {
            CalculateValueInGrid($(this).closest('tr'));
        });
        $(document).on('change', '.clRqQty , .txtclRate', function () {
            CalculateValueInGrid($(this).closest('tr'));
        });

        $(document).on('keyup', '#FreightAmt,#OtherCharges,#PackingShippingCharge', function () {
            //Calculate net amount
            CalculateNetAmountValue();
        });
        $(document).on('change', '#FreightAmt,#OtherCharges,#PackingShippingCharge', function () {
            //Calculate net amount
            CalculateNetAmountValue();
        });

        $("#Qty, #Rate").keyup(function () {

            $("#txtValue").val(clean($("#Rate").val()) * clean($("#Qty").val()));
        });
    },
}