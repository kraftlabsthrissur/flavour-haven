$(function () {
    Manufacturing.bind_events();
});
Manufacturing = {

    bind_events: function () {
        var self = Manufacturing;
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $.UIkit.autocomplete($('#productionGroup-autocomplete'), { 'source': self.get_item_AutoComplete, 'minLength': 1 });
        $('#productionGroup-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $.UIkit.autocomplete($('#output-itemcode-autocomplete'), { 'source': Manufacturing.get_output_itemcode, 'minLength': 1 });
        $('#output-itemcode-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_output_itemcode);
        $.UIkit.autocomplete($('#output-itemname-autocomplete'), { 'source': Manufacturing.get_output_itemname, 'minLength': 1 });
        $('#output-itemname-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_output_itemname);
        $.UIkit.autocomplete($('#input-itemcode-autocomplete'), { 'source': Manufacturing.get_input_itemcode, 'minLength': 1 });
        $('#input-itemcode-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_input_itemcode);
        $.UIkit.autocomplete($('#input-itemname-autocomplete'), { 'source': Manufacturing.get_input_itemname, 'minLength': 1 });
        $('#input-itemname-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_input_itemname);
        $("#FromOutputItemCodeRange").on("change", Manufacturing.get_to_itemcoderange);
        $("#FromOutputItemNameRange").on("change", Manufacturing.get_to_itemnamerange);
        $("#FromInputItemCodeRange").on("change", Manufacturing.get_to_input_itemcoderange);
        $("#FromInputItemNameRange").on("change", Manufacturing.get_to_input_itemnamerange);
        $.UIkit.autocomplete($('#productschedule-transNoFrom-autocomplete'), { 'source': Manufacturing.get_productSchedule_transNoFrom, 'minLength': 1 });
        $('#productschedule-transNoFrom-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_productSchedule_transNoFrom);
        $.UIkit.autocomplete($('#productschedule-transNoTo-autocomplete'), { 'source': Manufacturing.get_productSchedule_transNoTo, 'minLength': 1 });
        $('#productschedule-transNoTo-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_productSchedule_transNoTo);
        $.UIkit.autocomplete($('#productschedule-batchNoFrom-autocomplete'), { 'source': Manufacturing.get_productSchedule_batchNoFrom, 'minLength': 1 });
        $('#productschedule-batchNoFrom-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_productSchedule_batchNoFrom);
        $.UIkit.autocomplete($('#productschedule-batchNoTo-autocomplete'), { 'source': Manufacturing.get_productSchedule_batchNoTo, 'minLength': 1 });
        $('#productschedule-batchNoTo-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_productSchedule_batchNoTo);
        $.UIkit.autocomplete($('#production-groupname-autocomplete'), { 'source': Manufacturing.get_production_groupname, 'minLength': 1 });
        $('#production-groupname-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_production_groupname);
        $.UIkit.autocomplete($('#categoryfrom-autocomplete'), { 'source': Manufacturing.get_categoryfrom, 'minLength': 1 });
        $('#categoryfrom-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_categoryfrom);
        $.UIkit.autocomplete($('#categoryto-autocomplete'), { 'source': Manufacturing.get_categoryto, 'minLength': 1 });
        $('#categoryto-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_categoryto);
        $('.ProductionType').on('ifChanged', Manufacturing.show_production_type);
        $('.ProductionTypeByBatch').on('ifChanged', Manufacturing.show_production_type_bybatch);
        $('.StdCost').on('ifChanged', Manufacturing.show_std_cost);
        $.UIkit.autocomplete($('#itemcode-from-autocomplete'), { 'source': Manufacturing.get_itemcode_from, 'minLength': 1 });
        $('#itemcode-from-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_itemcode_from);
        $.UIkit.autocomplete($('#itemcode-to-autocomplete'), { 'source': Manufacturing.get_itemcode_to, 'minLength': 1 });
        $('#itemcode-to-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_itemcode_to);
        $.UIkit.autocomplete($('#input-itemcode-from-autocomplete'), { 'source': Manufacturing.get_input_itemcode_from, 'minLength': 1 });
        $('#input-itemcode-from-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_input_itemcode_from);
        $.UIkit.autocomplete($('#input-itemcode-to-autocomplete'), { 'source': Manufacturing.get_input_itemcode_to, 'minLength': 1 });
        $('#input-itemcode-to-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_input_itemcode_to);
        $.UIkit.autocomplete($('#itemname-autocomplete'), { 'source': Manufacturing.get_itemname, 'minLength': 1 });
        $('#input-itemname-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_itemname);

        $.UIkit.autocomplete($('#stock-issueno-autocomplete'), { 'source': Manufacturing.get_issueno, 'minLength': 1 });
        $('#stock-issueno-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_issueno);
        $.UIkit.autocomplete($('#stock-issuenoTo-autocomplete'), { 'source': Manufacturing.get_issuenoTo, 'minLength': 1 });
        $('#stock-issuenoTo-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_issuenoTo);

        $.UIkit.autocomplete($('#stock-receiptno-autocomplete'), { 'source': Manufacturing.get_receiptno, 'minLength': 1 });
        $('#stock-receiptno-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_receiptno);
        $.UIkit.autocomplete($('#stock-receiptnoTo-autocomplete'), { 'source': Manufacturing.get_receiptnoTo, 'minLength': 1 });
        $('#stock-receiptnoTo-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_receiptnoTo);

        $.UIkit.autocomplete($('#production-issueno-autocomplete'), { 'source': Manufacturing.get_production_issueno, 'minLength': 1 });
        $('#production-issueno-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_production_issueno);
        $.UIkit.autocomplete($('#production-issuenoTo-autocomplete'), { 'source': Manufacturing.get_production_issuenoTo, 'minLength': 1 });
        $('#production-issuenoTo-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_production_issuenoTo);


        $('.SummaryByCategory').on('ifChanged', Manufacturing.show_summary_byCategory);
        $('#Refresh').on('click', Manufacturing.refresh);
        $("#report-filter-submit").on("click", Manufacturing.get_report_view);

        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': Manufacturing.get_items, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_item);

        $.UIkit.autocomplete($('#output-items-autocomplete'), { 'source': Manufacturing.get_output_items, 'minLength': 1 });
        $('#output-items-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_output_items);

        $('.day-month-report-type').on('ifChanged', Manufacturing.show_production_daymonth_summary)

        $.UIkit.autocomplete($('#fg-items-autocomplete'), { 'source': Manufacturing.get_fishedgoods_items, 'minLength': 1 });
        $('#fg-items-autocomplete').on('selectitem.uk.autocomplete', Manufacturing.set_fishedgoods_items);
    },

    get_report_view: function (e) {
        e.preventDefault();
        self = Manufacturing;
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        ReportHelper.hide_controls();
        switch (name) {

            case "ProductionSchedule":

                if (($("#PSTransNoFrom").val() + $("#PSTransNoTo").val()).trim() != "") {
                    if ($("#PSTransNoFrom").val().trim() == "" || $("#PSTransNoTo").val().trim() == "") {
                        filters += "Request No: " + $("#PSTransNoFrom").val() + $("#PSTransNoTo").val() + ", ";
                    } else {
                        filters += "Request No: " + $("#PSTransNoFrom").val() + " - " + $("#PSTransNoTo").val() + ", ";
                    }
                }

                if (($("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val()).trim() != "") {
                    if ($("#PSBatchDateFrom").val().trim() == "" || $("#PSBatchDateTo").val().trim() == "") {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val() + ", ";
                    } else {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + " - " + $("#PSBatchDateTo").val() + ", ";
                    }
                }

                if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                    if ($("#PSBatchNoTo").val().trim() == "") {
                        filters += "Batch No " + "From: " + $("#PSBatchNoFrom").val() + ", ";
                    }
                    else if ($("#PSBatchNoFrom").val().trim() == "") {
                        filters += "Batch No " + "To: " + $("#PSBatchNoTo").val() + ", ";
                    }
                    else if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                        filters += "Batch No: " + $("#PSBatchNoFrom").val() + " - " + $("#PSBatchNoTo").val() + ", ";
                    }
                }

                filters += self.get_output_item_title();

                if ($("#ProductionGroupName").val() != 0) {
                    filters += "Production Group Name: " + $("#ProductionGroupName").val() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionMaterialWhereUsed":
                filters += self.get_output_item_title();
                if ($("#InputItemName").val() != 0) {
                    filters += "Input Item Name: " + $("#InputItemName").val() + ", ";
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionTimeUtilisation":

                if (($("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val()).trim() != "") {
                    if ($("#PSBatchDateFrom").val().trim() == "" || $("#PSBatchDateTo").val().trim() == "") {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val() + ", ";
                    } else {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + " - " + $("#PSBatchDateTo").val() + ", ";
                    }
                }

                if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                    if ($("#PSBatchNoTo").val().trim() == "") {
                        filters += "Batch No " + "From: " + $("#PSBatchNoFrom").val() + ", ";
                    }
                    else if ($("#PSBatchNoFrom").val().trim() == "") {
                        filters += "Batch No " + "To: " + $("#PSBatchNoTo").val() + ", ";
                    }
                    else if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                        filters += "Batch No: " + $("#PSBatchNoFrom").val() + " - " + $("#PSBatchNoTo").val() + ", ";
                    }
                }

                filters += self.get_output_item_title();

                if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                    if ($("#CategoryTo").val().trim() == "") {
                        filters += "Category " + "From: " + $("#CategoryFrom").val() + ", ";
                    }
                    else if ($("#CategoryFrom").val().trim() == "") {
                        filters += "Category " + "To: " + $("#CategoryTo").val() + ", ";
                    }
                    else if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                        filters += "Category: " + $("#CategoryFrom").val() + " - " + $("#CategoryTo").val() + ", ";
                    }
                }
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionByBatch":

                if (($("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val()).trim() != "") {
                    if ($("#PSBatchDateFrom").val().trim() == "" || $("#PSBatchDateTo").val().trim() == "") {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val() + ", ";
                    } else {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + " - " + $("#PSBatchDateTo").val() + ", ";
                    }
                }

                if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                    if ($("#PSBatchNoTo").val().trim() == "") {
                        filters += "Batch No " + "From: " + $("#PSBatchNoFrom").val() + ", ";
                    }
                    else if ($("#PSBatchNoFrom").val().trim() == "") {
                        filters += "Batch No " + "To: " + $("#PSBatchNoTo").val() + ", ";
                    }
                    else if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                        filters += "Batch No: " + $("#PSBatchNoFrom").val() + " - " + $("#PSBatchNoTo").val() + ", ";
                    }
                }

                filters += self.get_output_item_title();
                filters += self.get_input_item_title();

                if ($("#StatusFrom").val() != 0) {
                    filters += "Batch Status From: " + $("#StatusFrom Option:selected").text() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionByItem":
                if (($("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val()).trim() != "") {
                    if ($("#PSBatchDateFrom").val().trim() == "" || $("#PSBatchDateTo").val().trim() == "") {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val() + ", ";
                    } else {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + " - " + $("#PSBatchDateTo").val() + ", ";
                    }
                }

                filters += self.get_output_item_title();
                filters += self.get_input_item_title();
                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionByCategory":

                if (($("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val()).trim() != "") {
                    if ($("#PSBatchDateFrom").val().trim() == "" || $("#PSBatchDateTo").val().trim() == "") {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + $("#PSBatchDateTo").val() + ", ";
                    } else {
                        filters += "Request Batch Date: " + $("#PSBatchDateFrom").val() + " - " + $("#PSBatchDateTo").val() + ", ";
                    }
                }

                if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                    if ($("#PSBatchNoTo").val().trim() == "") {
                        filters += "Batch No " + "From: " + $("#PSBatchNoFrom").val() + ", ";
                    }
                    else if ($("#PSBatchNoFrom").val().trim() == "") {
                        filters += "Batch No " + "To: " + $("#PSBatchNoTo").val() + ", ";
                    }
                    else if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                        filters += "Batch No: " + $("#PSBatchNoFrom").val() + " - " + $("#PSBatchNoTo").val() + ", ";
                    }
                }
                filters += self.get_output_item_title();

                if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                    if ($("#CategoryTo").val().trim() == "") {
                        filters += "Category " + "From: " + $("#CategoryFrom").val() + ", ";
                    }
                    else if ($("#CategoryFrom").val().trim() == "") {
                        filters += "Category " + "To: " + $("#CategoryTo").val() + ", ";
                    }
                    else if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                        filters += "Category: " + $("#CategoryFrom").val() + " - " + $("#CategoryTo").val() + ", ";
                    }
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionStdCostEndItem":

                filters += self.get_output_item_title();
                filters += self.get_input_item_title();

                if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                    if ($("#CategoryTo").val().trim() == "") {
                        filters += "Category " + "From: " + $("#CategoryFrom").val() + ", ";
                    }
                    else if ($("#CategoryFrom").val().trim() == "") {
                        filters += "Category " + "To: " + $("#CategoryTo").val() + ", ";
                    }
                    else if (($("#CategoryFrom").val() + $("#CategoryTo").val()).trim() != "") {
                        filters += "Category: " + $("#CategoryFrom").val() + " - " + $("#CategoryTo").val() + ", ";
                    }
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;

            case "ProductionScheduleStatus":
                if (($("#PSTransNoFrom").val() + $("#PSTransNoTo").val()).trim() != "") {
                    if ($("#PSTransNoTo").val().trim() == "") {
                        filters += "Schedule No " + "From: " + $("#PSTransNoFrom").val() + ", ";
                    }
                    else if ($("#PSTransNoFrom").val().trim() == "") {
                        filters += "Schedule No " + "To: " + $("#PSTransNoTo").val() + ", ";
                    }
                    else if (($("#PSTransNoFrom").val() + $("#PSTransNoTo").val()).trim() != "") {
                        filters += "Schedule No: " + $("#PSTransNoFrom").val() + " - " + $("#PSTransNoTo").val() + ", ";
                    }
                }

                if ($("#ProductionGroupName").val() != 0) {
                    filters += "Production Group Name: " + $("#ProductionGroupName").val() + ", ";
                }

                if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                    if ($("#PSBatchNoTo").val().trim() == "") {
                        filters += "Batch No " + "From: " + $("#PSBatchNoFrom").val() + ", ";
                    }
                    else if ($("#PSBatchNoFrom").val().trim() == "") {
                        filters += "Batch No " + "To: " + $("#PSBatchNoTo").val() + ", ";
                    }
                    else if (($("#PSBatchNoFrom").val() + $("#PSBatchNoTo").val()).trim() != "") {
                        filters += "Batch No: " + $("#PSBatchNoFrom").val() + " - " + $("#PSBatchNoTo").val() + ", ";
                    }
                }

                if ($("#OuputItemNameID").val() == "") {
                    if (($("#FromOutputItemNameRange").val() + $("#ToOutputItemNameRange").val()).trim() != "") {
                        if ($("#ToOutputItemNameRange").val().trim() == "") {
                            filters += "Item Name Range " + "From: " + $("#FromOutputItemNameRange").val() + ", ";
                        }
                        else if ($("#FromOutputItemNameRange").val().trim() == "") {
                            filters += "Item Name Range " + "To: " + $("#ToOutputItemNameRange").val() + ", ";
                        }
                        else if (($("#FromOutputItemNameRange").val() + $("#ToOutputItemNameRange").val()).trim() != "") {
                            filters += "Item Name Range: " + $("#FromOutputItemNameRange").val() + " - " + $("#ToOutputItemNameRange").val() + ", ";
                        }
                    }
                }

                if ($("#OuputItemNameID").val() == "") {
                    if (($("#ItemCodeFrom").val() + $("#ItemCodeTo").val()).trim() != "") {
                        if ($("#ItemCodeTo").val().trim() == "") {
                            filters += "Item Code Range " + "From: " + $("#ItemCodeFrom").val() + ", ";
                        }
                        else if ($("#ItemCodeFrom").val().trim() == "") {
                            filters += "Item Code Range " + "To: " + $("#ItemCodeTo").val() + ", ";
                        }
                        else if (($("#ItemCodeFrom").val() + $("#ItemCodeTo").val()).trim() != "") {
                            filters += "Item Code Range: " + $("#ItemCodeFrom").val() + " - " + $("#ItemCodeTo").val() + ", ";
                        }
                    }
                }

                if ($("#OuputItemNameID").val() != 0) {
                    filters += "Item: " + $("#ItemName").val() + ", ";
                }

                if (($("#IssueNoFrom").val() + $("#IssueNoTo").val()).trim() != "") {
                    if ($("#IssueNoTo").val().trim() == "") {
                        filters += "Issue No " + "From: " + $("#IssueNoFrom").val() + ", ";
                    }
                    else if ($("#IssueNoFrom").val().trim() == "") {
                        filters += "Issue No " + "To: " + $("#IssueNoTo").val() + ", ";
                    }
                    else if (($("#IssueNoFrom").val() + $("#IssueNoTo").val()).trim() != "") {
                        filters += "Issue No: " + $("#IssueNoFrom").val() + " - " + $("#IssueNoTo").val() + ", ";
                    }
                }

                if (($("#ReceiptNoFrom").val() + $("#ReceiptNoTo").val()).trim() != "") {
                    if ($("#ReceiptNoTo").val().trim() == "") {
                        filters += "Receipt No " + "From: " + $("#ReceiptNoFrom").val() + ", ";
                    }
                    else if ($("#ReceiptNoFrom").val().trim() == "") {
                        filters += "Receipt No " + "To: " + $("#ReceiptNoTo").val() + ", ";
                    }
                    else if (($("#ReceiptNoFrom").val() + $("#ReceiptNoTo").val()).trim() != "") {
                        filters += "Receipt No: " + $("#ReceiptNoFrom").val() + " - " + $("#ReceiptNoTo").val() + ", ";
                    }
                }

                if (($("#ProductionIssueNoFrom").val() + $("#ProductionIssueNoTo").val()).trim() != "") {
                    if ($("#ProductionIssueNoTo").val().trim() == "") {
                        filters += "Production Issue No " + "From: " + $("#ProductionIssueNoFrom").val() + ", ";
                    }
                    else if ($("#ProductionIssueNoFrom").val().trim() == "") {
                        filters += "Production Issue No " + "To: " + $("#ProductionIssueNoTo").val() + ", ";
                    }
                    else if (($("#ProductionIssueNoFrom").val() + $("#ProductionIssueNoTo").val()).trim() != "") {
                        filters += "Production Issue No: " + $("#ProductionIssueNoFrom").val() + " - " + $("#ProductionIssueNoTo").val() + ", ";
                    }
                }

                if ($("#Status").val() != 0) {
                    filters += "Status: " + $("#Status").val() + ", ";
                }

                if (filters != "") {
                    filters = filters.replace(/,(\s+)?$/, '');
                    filters = "Filters applied: " + filters;
                }

                data += "&Filters=" + filters;
                break;
        }
        console.log(data);
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#report-viewer").html(response);
                ReportHelper.inject_js();
            }
        })
        return false;
    },

    show_production_daymonth_summary: function () {
        self = Manufacturing;
        var report_type = $(".day-month-report-type:checked").val();
        $(".filters").addClass("uk-hidden");
        $("." + report_type).removeClass("uk-hidden");
        $("#ProductionGroupName").val('');
        $("#ProductionGroupID").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#SalesCategoryID").val('');
        $("#ProductionCategoryID").val('');
        $("#BatchTypeID").val('');
    },

    get_fishedgoods_items: function (release) {
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

    set_fishedgoods_items: function (event, item) {
        var self = Manufacturing;
        var Name = item.name;
        var ID = item.id;
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
    },

    get_output_item_title: function () {
        self = Manufacturing;
        var filters = "";
        if ($("#OuputItemNameID").val() == "") {
            if (($("#ItemCodeFrom").val() + $("#ItemCodeTo").val()).trim() != "") {
                if ($("#ItemCodeTo").val().trim() == "") {
                    filters += "Output Item Code " + "From: " + $("#ItemCodeFrom").val() + ", ";
                }
                else if ($("#ItemCodeFrom").val().trim() == "") {
                    filters += "Output Item Code " + "To: " + $("#ItemCodeTo").val() + ", ";
                }
                else if (($("#ItemCodeFrom").val() + $("#ItemCodeTo").val()).trim() != "") {
                    filters += "Output Item Code: " + $("#ItemCodeFrom").val() + " - " + $("#ItemCodeTo").val() + ", ";
                }
            }
        }

        if ($("#OuputItemNameID").val() == "") {
            if (($("#FromOutputItemNameRange").val() + $("#ToOutputItemNameRange").val()).trim() != "") {
                if ($("#ToOutputItemNameRange").val().trim() == "") {
                    filters += "Output Item Name Range " + "From: " + $("#FromOutputItemNameRange").val() + ", ";
                }
                else if ($("#FromOutputItemNameRange").val().trim() == "") {
                    filters += "Output Item Name Range " + "To: " + $("#ToOutputItemNameRange").val() + ", ";
                }
                else if (($("#FromOutputItemNameRange").val() + $("#ToOutputItemNameRange").val()).trim() != "") {
                    filters += "Output Item Name Range: " + $("#FromOutputItemNameRange").val() + " - " + $("#ToOutputItemNameRange").val() + ", ";
                }
            }
        }

        if ($("#OuputItemNameID").val() != 0) {
            filters += "Output Item Name: " + $("#OutputItemName").val() + ", ";
        }
        return filters;
    },

    get_input_item_title: function () {
        self = Manufacturing;
        var filters = "";
        if ($("#InputItemNameID").val() == "") {
            if (($("#InputItemCodeFrom").val() + $("#InputItemCodeTo").val()).trim() != "") {
                if ($("#InputItemCodeTo").val().trim() == "") {
                    filters += "Input Item Code " + "From: " + $("#InputItemCodeFrom").val() + ", ";
                }
                else if ($("#InputItemCodeFrom").val().trim() == "") {
                    filters += "Input Item Code " + "To: " + $("#InputItemCodeTo").val() + ", ";
                }
                else if (($("#InputItemCodeFrom").val() + $("#InputItemCodeTo").val()).trim() != "") {
                    filters += "Input Item Code: " + $("#InputItemCodeFrom").val() + " - " + $("#InputItemCodeTo").val() + ", ";
                }
            }
        }

        if ($("#InputItemNameID").val() == "") {
            if (($("#FromInputItemNameRange").val() + $("#ToInputItemNameRange").val()).trim() != "") {
                if ($("#ToInputItemNameRange").val().trim() == "") {
                    filters += "Input Item Name Range " + "From: " + $("#FromInputItemNameRange").val() + ", ";
                }
                else if ($("#FromInputItemNameRange").val().trim() == "") {
                    filters += "Input Item Name Range " + "To: " + $("#ToInputItemNameRange").val() + ", ";
                }
                else if (($("#FromInputItemNameRange").val() + $("#ToInputItemNameRange").val()).trim() != "") {
                    filters += "Input Item Name Range: " + $("#FromInputItemNameRange").val() + " - " + $("#ToInputItemNameRange").val() + ", ";
                }
            }
        }

        if ($("#InputItemNameID").val() != 0) {
            filters += "Input Item Name: " + $("#InputItemName").val() + ", ";
        }
        return filters;
    },

    get_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Reports/Manufacturing/GetProductionGroups',
            data: {
                ItemHind: $('#ProductionGroupName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item_details: function (event, item) {
        var self = Manufacturing;
        $("#ProductName").val(item.Name);
        $("#GroupID").val(item.id);
    },

    get_output_itemcode: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#OutputItemCode').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_output_itemcode: function (event, item) {
        self = Manufacturing;
        $("#OutputItemCode").val(item.code);
        $("#OuputItemCodeID").val(item.id);
    },
    get_output_itemname: function (release) {

        var area;
        //var type = $("input[name='Type']:checked").val(); 
        area = $("#ItemAutoType").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#OutputItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
        //Table = 'ItemName';
        //$.ajax({
        //    url: '/Reports/Manufacturing/GetAutoComplete',
        //    data: {
        //        Term: $('#OutputItemName').val(),
        //        Table: Table
        //    },
        //    dataType: "json",
        //    type: "POST",
        //    success: function (data) {
        //        release(data);
        //    }
        //});

    },
    set_output_itemname: function (event, item) {
        self = Manufacturing;
        $("#OutputItemName").val(item.Name);
        $("#OuputItemNameID").val(item.id);
    },
    get_input_itemcode: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#InputItemCode').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_input_itemcode: function (event, item) {
        self = Manufacturing;
        $("#InputItemCode").val(item.code);
        $("#InputItemCodeID").val(item.id);
    },
    get_input_itemname: function (release) {

        //Table = 'ItemName';
        //$.ajax({
        //    url: '/Reports/Manufacturing/GetAutoComplete',
        //    data: {
        //        Term: $('#InputItemName').val(),
        //        Table: Table
        //    },
        //    dataType: "json",
        //    type: "POST",
        //    success: function (data) {
        //        release(data);
        //    }
        //});
        var area;
        //var type = $("input[name='Type']:checked").val(); 
        area = $("#ItemAutoType").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#InputItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_items: function (release) {
        var area;
        //var type = $("input[name='Type']:checked").val(); 
        area = $("#ItemAutoType").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#InputItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_item: function (event, item) {
        var self = Manufacturing;
        $("#InputItemName").val(item.Name);
        $("#InputItemNameID").val(item.id);
    },
    get_output_items: function (release) {
        var area;
        //var type = $("input[name='Type']:checked").val(); 
        area = $("#ItemAutoType").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#OutputItemName").val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_output_items: function (event, item) {
        var self = Manufacturing;
        $("#OutputItemName").val(item.Name);
        $("#OuputItemNameID").val(item.id);
    },
    set_input_itemname: function (event, item) {
        self = Manufacturing;
        $("#InputItemName").val(item.Name);
        $("#InputItemNameID").val(item.id);
    },
    get_to_itemcoderange: function () {
        var self = Manufacturing;
        var from_range = $("#FromOutputItemCodeRange").val();
        $.ajax({
            url: '/Reports/Manufacturing/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToOutputItemCodeRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToOutputItemCodeRange").append(html);
            }
        });
    },
    get_to_itemnamerange: function () {
        var self = Manufacturing;
        var from_range = $("#FromOutputItemNameRange").val();
        $.ajax({
            url: '/Reports/Manufacturing/GetItemNameRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToOutputItemNameRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToOutputItemNameRange").append(html);
            }
        });
    },
    get_to_input_itemcoderange: function () {
        var self = Manufacturing;
        var from_range = $("#FromInputItemCodeRange").val();
        $.ajax({
            url: '/Reports/Manufacturing/GetItemRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToInputItemCodeRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToInputItemCodeRange").append(html);
            }
        });
    },
    get_to_input_itemnamerange: function () {
        var self = Manufacturing;
        var from_range = $("#FromInputItemNameRange").val();
        $.ajax({
            url: '/Reports/Manufacturing/GetItemNameRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToInputItemNameRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToInputItemNameRange").append(html);
            }
        });
    },
    get_productSchedule_transNoFrom: function (release) {

        Table = 'ProductSchedule';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#PSTransNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_productSchedule_transNoFrom: function (event, item) {
        self = Manufacturing;
        $("#PSTransNoFrom").val(item.code);
        $("#PSTransNoFromID").val(item.id);
    },
    get_productSchedule_transNoTo: function (release) {

        Table = 'ProductSchedule';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#PSTransNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_productSchedule_transNoTo: function (event, item) {
        self = Manufacturing;
        $("#PSTransNoTo").val(item.code);
        $("#PSTransNoToID").val(item.id);
    },
    get_productSchedule_batchNoFrom: function (release) {

        Table = 'Batch';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#PSBatchNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_productSchedule_batchNoFrom: function (event, item) {
        self = Manufacturing;
        $("#PSBatchNoFrom").val(item.code);
        $("#PSBatchNoFromID").val(item.id)

    },
    get_productSchedule_batchNoTo: function (release) {

        Table = 'Batch';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#PSBatchNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_productSchedule_batchNoTo: function (event, item) {
        self = Manufacturing;
        $("#PSBatchNoTo").val(item.code);
        $("#PSBatchNoToID").val(item.id);

    },
    get_production_groupname: function (release) {

        Table = 'ProductionGroup';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ProductionGroupName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_production_groupname: function (event, item) {
        self = Manufacturing;
        $("#ProductionGroupName").val(item.code);
        $("#ProductionGroupID").val(item.id);
    },

    get_categoryfrom: function (release) {
        Table = 'Category';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#CategoryFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_categoryfrom: function (event, item) {
        self = Manufacturing;
        $("#CategoryFrom").val(item.code);
        $("#CategoryFromID").val(item.id);
    },

    get_categoryto: function (release) {
        Table = 'Category';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#CategoryTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_categoryto: function (event, item) {
        self = Manufacturing;
        $("#CategoryTo").val(item.code);
        $("#CategoryToID").val(item.id);
    },
    get_itemcode_from: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_itemcode_from: function (event, item) {
        self = Manufacturing;
        $("#ItemCodeFrom").val(item.code);
        $("#ItemCodeFromID").val(item.id);
    },
    get_itemcode_to: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_itemcode_to: function (event, item) {
        self = Manufacturing;
        $("#ItemCodeTo").val(item.code);
        $("#ItemCodeToID").val(item.id);
    },
    get_input_itemcode_from: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#InputItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_input_itemcode_from: function (event, item) {
        self = Manufacturing;
        $("#InputItemCodeFrom").val(item.code);
        $("#InputItemCodeFromID").val(item.id);
    },
    get_input_itemcode_to: function (release) {

        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#InputItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_input_itemcode_to: function (event, item) {
        self = Manufacturing;
        $("#InputItemCodeTo").val(item.code);
        $("#InputItemCodeToID").val(item.id);
    },

    get_issueno: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#IssueNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_issueno: function (event, item) {
        self = Manufacturing;
        $("#IssueNoFrom").val(item.code);
        $("#IssueNoFromID").val(item.id);
    },

    get_issuenoTo: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#IssueNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_issuenoTo: function (event, item) {
        self = Manufacturing;
        $("#IssueNoTo").val(item.code);
        $("#IssueNoToID").val(item.id);

        var stock_type = $(".stocktransfer_report_type:checked").val();
        var report_type = $(".stocktransfer-summary:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            if (stock_type != "StockTransferForm") {
                $("#IssueNoFrom").val('');
                $("#IssueNoFromID").val('');
            }
            else {
                $("#IssueNoFrom").val(item.code);
                $("#IssueNoFromID").val(item.id);
            }
            $("#IssueNoFrom").val(item.code);
            $("#IssueNoFromID").val(item.id);
        }
    },

    get_receiptno: function (release) {
        var Table = "StockReceiptNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_receiptno: function (event, item) {
        self = Manufacturing;
        $("#ReceiptNoFrom").val(item.code);
        $("#ReceiptNoFromID").val(item.id);
    },

    get_receiptnoTo: function (release) {
        var Table = "StockReceiptNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_receiptnoTo: function (event, item) {
        self = Manufacturing;
        $("#ReceiptNoTo").val(item.code);
        $("#ReceiptNoToID").val(item.id);

        //var stock_type = $(".stocktransfer_report_type:checked").val();
        //var report_type = $(".stocktransfer-summary:checked").val();
        //if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
        //    if (stock_type != "StockTransferForm") {
        //        $("#IssueNoFrom").val('');
        //        $("#IssueNoFromID").val('');
        //    }
        //    else {
        //        $("#IssueNoFrom").val(item.code);
        //        $("#IssueNoFromID").val(item.id);
        //    }
        //    $("#IssueNoFrom").val(item.code);
        //    $("#IssueNoFromID").val(item.id);
        //}
    },

    get_production_issueno: function (release) {
        var Table = "ProductionIssueNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ProductionIssueNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_production_issueno: function (event, item) {
        self = Manufacturing;
        $("#ProductionIssueNoFrom").val(item.code);
        $("#ProductionIssueNoFromID").val(item.id);
    },

    get_production_issuenoTo: function (release) {
        var Table = "ProductionIssueNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ProductionIssueNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_production_issuenoTo: function (event, item) {
        self = Manufacturing;
        $("#ProductionIssueNoTo").val(item.code);
        $("#ProductionIssueNoToID").val(item.id);

        //var stock_type = $(".stocktransfer_report_type:checked").val();
        //var report_type = $(".stocktransfer-summary:checked").val();
        //if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
        //    if (stock_type != "StockTransferForm") {
        //        $("#IssueNoFrom").val('');
        //        $("#IssueNoFromID").val('');
        //    }
        //    else {
        //        $("#IssueNoFrom").val(item.code);
        //        $("#IssueNoFromID").val(item.id);
        //    }
        //    $("#IssueNoFrom").val(item.code);
        //    $("#IssueNoFromID").val(item.id);
        //}
    },


    refresh: function (event, item) {
        self = Manufacturing;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#ToDate").val(currentdate);
        $("#FromDate").val(findate);
        $("#PSBatchDateTo").val(currentdate);
        $("#PSBatchDateFrom").val(findate);
        $("#FromOutputItemCodeRange").val('');
        $("#ToOutputItemCodeRange").val('');
        $("#FromOutputItemNameRange").val('');
        $("#ToOutputItemNameRange").val('');
        $("#OutputItemCode").val('');
        $("#OuputItemCodeID").val('');
        $("#OutputItemName").val('');
        $("#OuputItemNameID").val('');
        $("#FromInputItemCodeRange").val('');
        $("#ToInputItemCodeRange").val('');
        $("#FromInputItemNameRange").val('');
        $("#ToInputItemNameRange").val('');
        $("#InputItemCode").val('');
        $("#InputItemCodeID").val('');
        $("#InputItemName").val('');
        $("#InputItemNameID").val('');
        $("#PSTransNoFrom").val('');
        $("#PSTransNoFromID").val('');
        $("#PSTransNoTo").val('');
        $("#PSTransNoToID").val('');
        $("#PSBatchNoFrom").val('');
        $("#PSBatchNoFromID").val('');
        $("#PSBatchNoTo").val('');
        $("#PSBatchNoToID").val('');
        $("#ProductionGroupName").val('');
        $("#ProductionGroupID").val('');
        $("#CategoryFrom").val('');
        $("#CategoryFromID").val('');
        $("#CategoryTo").val('');
        $("#CategoryToID").val('');
        $("#ItemCodeFrom").val('');
        $("#ItemCodeFromID").val('');
        $("#ItemCodeTo").val('');
        $("#ItemCodeToID").val('');
        $("#InputItemCodeFrom").val('');
        $("#InputItemCodeFromID").val('');
        $("#InputItemCodeTo").val('');
        $("#InputItemCodeToID").val('');
        $("#StatusFrom").val('');
        $("#StatusTo").val('');
        $("#ReceiptNoFrom").val('');
        $("#ReceiptNoTo").val('');
        $("#ReceiptNoFromID").val('');
        $("#ReceiptNoToID").val('');
        $("#ProductionIssueNoTo").val('');
        $("#ProductionIssueNoFrom").val('');
        $("#ProductionIssueNoToID").val('');
        $("#ProductionIssueNoFromID").val('');
        $("#Status").val('All');
        $("#IssueNoFrom").val('');
        $("#IssueNoTo").val('');
        $("#IssueNoFromID").val('');
        $("#IssueNoToID").val('');
        
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#SalesCategoryID").val('');
        $("#ProductionCategoryID").val('');
        $("#BatchTypeID").val('');
        
    },
    show_production_type: function () {
        self = Manufacturing;
        var type = $(".ProductionType:checked").val();
        if (type == "OutputByItem") {
            $(".input-type").hide();
            $(".output-type").show();
            $(".output-type").removeClass("uk-hidden");
            $(".input-type").addClass("uk-hidden");
            $(".summary").removeClass("uk-hidden");
        }
        else {
            $(".output-type").hide();
            $(".input-type").show();
            $(".input-type").removeClass("uk-hidden");
            $(".output-type").addClass("uk-hidden");
            $(".summary").addClass("uk-hidden");
        }
        self.refresh();
    },
    show_production_type_bybatch: function () {
        self = Manufacturing;
        var type = $(".ProductionTypeByBatch:checked").val();
        if (type == "OutputByBatch") {
            $(".input-type").hide();
            $(".output-type").show();
            $(".output-type").removeClass("uk-hidden");
            $(".input-type").addClass("uk-hidden");
            $(".summary").removeClass("uk-hidden");
        }
        else {
            $(".output-type").hide();
            $(".input-type").show();
            $(".input-type").removeClass("uk-hidden");
            $(".output-type").addClass("uk-hidden");
            $(".summary").addClass("uk-hidden");
        }
        self.refresh();
    },
    show_std_cost: function () {
        self = Manufacturing;
        var type = $(".StdCost:checked").val();
        if (type == "PurchaseItem") {
            $(".input-type").show();
            $(".output-type").hide();
            $(".input-type").removeClass("uk-hidden");
            $(".output-type").addClass("uk-hidden");
        }
        else {
            $(".output-type").show();
            $(".input-type").hide();
            $(".output-type").removeClass("uk-hidden");
            $(".input-type").addClass("uk-hidden");
        }
        self.refresh();
    },
    show_summary_byCategory: function () {
        self = Manufacturing;
        var type = $(".SummaryByCategory:checked").val();
        if (type == "Detail") {
            $(".ratevalue").removeClass("uk-hidden");
        }
        else {
            $(".ratevalue").addClass("uk-hidden");
        }
        self.refresh();
    },
}