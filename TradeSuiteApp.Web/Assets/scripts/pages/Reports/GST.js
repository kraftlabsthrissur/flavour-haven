$(function () {
    GST.bind_events();

});

GST = {

    bind_events: function () {
        var self = GST;
        UIkit.autocomplete($('#all-item-autocomplete'), { 'source': GST.get_items, 'minLength': 1 }); //-- GST
        $('#all-item-autocomplete').on('selectitem.uk.autocomplete', GST.set_items);
        // $("#ItemCategoryID").on("change", purchase.change_category);
        $('.type').on('ifChanged', GST.show_gst_report_type);
        $('.report_type').on('ifChanged', GST.show_gst_type);
        //$('.report_type').on('ifChanged', purchase.show_report_type);
        $("#report-filter-submit").on("click", GST.get_report_view);
        $("#FromSupplierTaxSubCategoryRange").on("change", GST.get_supplier_tax_subcategory_to_range);
        $("#FromSupplierRange").on("change", GST.get_supplier_to_range);
        $("#FromCategoryRange").on("change", GST.get_category_to_range);
        $("#FromItemNameRange").on("change", GST.get_item_to_range);
        $('#Refresh').on('click', GST.refresh);

        $.UIkit.autocomplete($('#supplier-sub-tax-category-autocomplete'), { 'source': GST.get_supplier_tax_subcategory, 'minLength': 1 });
        $('#supplier-sub-tax-category-autocomplete').on('selectitem.uk.autocomplete', GST.set_supplier_tax_subcategory);
        $.UIkit.autocomplete($('#supplier-autocomplete'), { 'source': GST.get_suppliers, 'minLength': 1 });
        $('#supplier-autocomplete').on('selectitem.uk.autocomplete', GST.set_supplier_details);
        $.UIkit.autocomplete($('#qcno-autocomplete'), { 'source': GST.get_qcno, 'minLength': 1 });
        $('#qcno-autocomplete').on('selectitem.uk.autocomplete', GST.set_qcno);
        $.UIkit.autocomplete($('#qcnoTo-autocomplete'), { 'source': GST.get_qcnoTo, 'minLength': 1 });
        $('#qcnoTo-autocomplete').on('selectitem.uk.autocomplete', GST.set_qcnoTo);
        $.UIkit.autocomplete($('#grnno-autocomplete'), { 'source': GST.get_grnno, 'minLength': 1 });
        $('#grnno-autocomplete').on('selectitem.uk.autocomplete', GST.set_grnno);
        $.UIkit.autocomplete($('#grnnoTo-autocomplete'), { 'source': GST.get_grnnoTo, 'minLength': 1 });
        $('#grnnoTo-autocomplete').on('selectitem.uk.autocomplete', GST.set_grnnoTo);
        $.UIkit.autocomplete($('#supplier-gstno-autocomplete'), { 'source': GST.get_gstno, 'minLength': 1 });
        $('#supplier-gstno-autocomplete').on('selectitem.uk.autocomplete', GST.set_gstno);
        $("#FromGSTRateRange").on("change", GST.get_gst_to);

    },
    get_gst_to: function () {
        var self = GST;
        var FromGst = clean($("#FromGSTRateRange").val());
        if (FromGst == null || FromGst == "Select") {
            FromGst = 0.0;
        }
        $.ajax({
            url: '/Reports/GST/GetGstToRate/' + FromGst,
            data: { FromGst: FromGst },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $("#ToGSTRateRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.IGSTPercentage + "'>" + record.IGSTPercentage.toFixed(2) + "</option>";
                });
                $("#ToGSTRateRange").append(html);
            }
        });
    },
    refresh: function (event, item) {
        self = GST;
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $("#InvoiceDateTo").val(currentdate);
        $("#InvoiceDateFrom").val(findate);
        $("#QCDateFrom").val();
        $("#QCDateTo").val();
        $("#GRNDateFrom").val();
        $("#GRNDateTo").val();
        $("#FromSupplierTaxSubCategoryRange").val('');
        $("#ToSupplierTaxSubCategoryRange").val('');
        $("#SupplierTaxSubCategory").val('');
        $("#SupplierTaxSubCategoryID").val('');
        $("#FromSupplierRange").val('');
        $("#ToSupplierRange").val('');
        $("#SupplierName").val('');
        $("#SupplierID").val('');
        $("#FromCategoryRange").val('');
        $("#ToCategoryRange").val('');
        $("#ItemCategoryList").val('');
        $("#FromItemNameRange").val('');
        $("#ToItemNameRange").val('');
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#QCNOFrom").val('');
        $("#QCNOFromID").val('');
        $("#QCNOTo").val('');
        $("#QCNOToID").val('');
        $("#GRNNOFrom").val('');
        $("#GRNNOFromID").val('');
        $("#GRNNOTo").val('');
        $("#GRNNOToID").val('');
        $("#FromGSTRateRange").val('');
        $("#ToGSTRateRange").val('');
        $("#FromSupplierGSTNNoRange").val('');
        $("#SupplierGSTNoID").val('');
        $("#SupplierGstNoFrom").val('');
        $("#QCDateFrom").val('');
        $("#QCDateTo").val('');
        $("#GRNDateFrom").val('');
        $("#GRNDateTo").val('');
    },

    get_gstno: function (release) {

        var Table = 'SupplierGSTNo';

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#SupplierGstNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_gstno: function (event, item) {
        self = GST;
        $("#SupplierGstNoFrom").val(item.code);
        $("#SupplierGSTNoID").val(item.id);
    },

    get_grnno: function (release) {

        var Table = 'GRN';

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grnno: function (event, item) {
        self = GST;
        $("#GRNNOFrom").val(item.code);
        $("#GRNNOFromID").val(item.id);
    },
    get_grnnoTo: function (release) {

        var Table = 'GRN';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_grnnoTo: function (event, item) {
        self = GST;
        $("#GRNNOTo").val(item.code);
        $("#GRNNOToID").val(item.id);
    },
    get_qc_grnno: function (release) {

        Table = 'GRN';

        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_qcno: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOFrom').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qcno: function (event, item) {
        self = GST;
        $("#QCNOFrom").val(item.code);
        $("#QCNOFromID").val(item.id);
    },

    get_qcnoTo: function (release) {
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#QCNOTo').val(),
                Table: 'QualityCheck'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_qcnoTo: function (event, item) {
        self = GST;
        $("#QCNOTo").val(item.code);
        $("#QCNOToID").val(item.id);
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
    get_items: function (release) {

        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',
            data: {
                Hint: $("#ItemName").val(),
                Areas: 'All',
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },    
    set_items: function (event, item) {
        var self = GST;
        $("#ItemName").val(item.Name);
        $("#ItemID").val(item.id);
    },
    set_supplier_details: function (event, item) {   // on select auto complete item
        self = GST;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },
    get_supplier_to_range: function () {
        var self = GST;
        var from_range = $("#FromSupplierRange").val();
        $.ajax({
            url: '/Reports/GST/GetSupplierToRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierRange").append(html);
            }
        });
    },
    get_category_to_range: function () {
        var self = GST;
        var from_range = $("#FromCategoryRange").val();
        $.ajax({
            url: '/Reports/GST/GetCategoryToRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToCategoryRange").append(html);
            }
        });
    },
    get_item_to_range: function () {
        var self = GST;
        var from_range = $("#FromItemNameRange").val();
        $.ajax({
            url: '/Reports/GST/GetItemToRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToItemNameRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToItemNameRange").append(html);
            }
        });
    },

    get_supplier_tax_subcategory: function (release) {
        var Table = 'SupplierTaxSubCategory';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#SupplierTaxSubCategory').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_supplier_tax_subcategory: function (event, item) {
        var self = GST;
        $("#SupplierTaxSubCategory").val(item.code);
        $("#SupplierTaxSubCategoryID").val(item.id);

    },

    show_gst_report_type: function () {
        var self = GST;
        var report_type = $(this).val();
        if (report_type == "item-wise") {
            $(".item-wise").show();
        }
        else {
            $(".item-wise").hide();
        }
        self.refresh();
    },

    show_gst_type: function () {
        var self = GST;
        var report_type = $(".type:checked").val();
        if (report_type == "item-wise") {
            $(".item-wise").show();
        }
        else {
            $(".item-wise").hide();
        }
        self.refresh();
    },

    get_report_view: function (e) {
        e.preventDefault();
        self = GST;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        ReportHelper.hide_controls();
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        switch (name) {
            case "GST":

                //if (($("#QCDateFrom").val() + $("#QCDateTo").val()).trim() != "") {
                //    if ($("#QCDateFrom").val().trim() == "" || $("#QCDateTo").val().trim() == "") {
                //        filters += "Request QC Date: " + $("#QCDateFrom").val() + $("#QCDateTo").val() + ", ";
                //    } else {
                //        filters += "Request QC Date: " + $("#QCDateFrom").val() + " - " + $("#QCDateTo").val() + ", ";
                //    }
                //}

                //if (($("#GRNDateFrom").val() + $("#GRNDateTo").val()).trim() != "") {
                //    if ($("#GRNDateFrom").val().trim() == "" || $("#GRNDateTo").val().trim() == "") {
                //        filters += "Request GRN Date: " + $("#GRNDateFrom").val() + $("#GRNDateTo").val() + ", ";
                //    } else {
                //        filters += "Request GRN Date: " + $("#GRNDateFrom").val() + " - " + $("#GRNDateTo").val() + ", ";
                //    }
                //} 

                if ($("#SupplierTaxSubCategory").val() != 0) {
                    filters += "Supplier Tax Sub Category: " + $("#SupplierTaxSubCategory").val() + ", ";
                }

                filters += self.get_item_title();

                filters += self.get_supplier_title();

                //if (($("#QCNOFrom").val() + $("#QCNOTo").val()).trim() != "") {
                //    if ($("#QCNOFrom").val().trim() == "" || $("#QCNOTo").val().trim() == "") {
                //        filters += "QC No: " + $("#QCNOFrom").val() + $("#QCNOTo").val() + ", ";
                //    } else {
                //        filters += "QC No: " + $("#QCNOFrom").val() + " - " + $("#QCNOTo").val() + ", ";
                //    }
                //}

                //if (($("#GRNNOFrom").val() + $("#GRNNOTo").val()).trim() != "") {
                //    if ($("#GRNNOFrom").val().trim() == "" || $("#GRNNOTo").val().trim() == "") {
                //        filters += "GRN No: " + $("#GRNNOFrom").val() + $("#GRNNOTo").val() + ", ";
                //    } else {
                //        filters += "GRN No: " + $("#GRNNOFrom").val() + " - " + $("#GRNNOTo").val() + ", ";
                //    }
                //}

                
                    if (($("#FromGSTRateRange").val() + $("#ToGSTRateRange").val()).trim() != "") {
                        if ($("#ToGSTRateRange").val().trim() == "") {
                            filters += "GST Range " + "From: " + $("#FromGSTRateRange").val() + ", ";
                        }
                        else if ($("#FromGSTRateRange").val().trim() == "") {
                            filters += "GST Range " + "To: " + $("#ToGSTRateRange").val() + ", ";
                        }
                        else if (($("#FromGSTRateRange").val() + $("#ToGSTRateRange").val()).trim() != "") {
                            filters += "GST Range: " + $("#FromGSTRateRange").val() + " - " + $("#ToGSTRateRange").val() + ", ";
                        }
                    }
                
                    if ($("#SupplierGstNoFrom").val() != 0) {
                        filters += "Supplier Gst No: " + $("#SupplierGstNoFrom").val() + ", ";
                    }

                    if (filters != "") {
                        filters = filters.replace(/,(\s+)?$/, '');
                        filters = "Filters applied: " + filters;
                    }
                    data += "&Filters=" + filters;
             
        }
        console.log(data);
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#report-viewer").html(response)
                ReportHelper.inject_js();
            }
        })
        return false;
    },

    get_item_title: function () {
        self = GST;
        var filters = "";
        //if ($("#ItemID").val() == "") {
        //    if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
        //        if ($("#ToItemNameRange").val().trim() == "") {
        //            filters += "Item Name Range " + "From: " + $("#FromItemNameRange").val();
        //        }
        //        else if ($("#FromItemNameRange").val().trim() == "") {
        //            filters += "Item Name Range " + "To: " + $("#ToItemNameRange").val() + ", ";
        //        }
        //        else if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
        //            filters += "Item Name Range: " + $("#FromItemNameRange").val() + " - " + $("#ToItemNameRange").val() + ", ";
        //        }
        //    }
        //}

        if ($("#ItemID").val() != 0) {
            filters += "Item: " + $("#ItemName").val() + ", ";
        }
        return filters;
    },


    get_supplier_title: function () {
        self = GST;
        var filters = "";
        //if ($("#SupplierID").val() == "") {
        //    if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
        //        if ($("#ToSupplierRange").val().trim() == "") {
        //            filters += "Supplier Name Range " + "From: " + $("#FromSupplierRange").val() + ", ";
        //        }
        //        else if ($("#FromSupplierRange").val().trim() == "") {
        //            filters += "Supplier Name Range " + "To: " + $("#ToSupplierRange").val() + ", ";
        //        }
        //        else if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
        //            filters += "Supplier Name Range: " + $("#FromSupplierRange").val() + " - " + $("#ToSupplierRange").val() + ", ";
        //        }
        //    }
        //}

        if ($("#SupplierID").val() != 0) {
            filters += "Supplier: " + $("#SupplierName").val() + ", ";
        }
        return filters;
    },

    get_supplier_tax_subcategory_to_range: function () {
        var self = GST;
        var from_range = $("#FromSupplierTaxSubCategoryRange").val();
        $.ajax({
            url: '/Reports/GST/GetSupplierSubCategoryToRange/',
            dataType: "json",
            type: "GET",
            data: {
                from_range: from_range,
            },
            success: function (response) {
                $("#ToSupplierTaxSubCategoryRange").html("");
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option>" + record.Text + "</option>";
                });
                $("#ToSupplierTaxSubCategoryRange").append(html);
            }
        });
    },
    validate_form: function () {
        var self = GST;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [
            {
                elements: "#QCNOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#QCNOToID').val());
                               var from_id = clean($('#QCNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected QC No To must be greater than or equal to QC No From'
                       },
                ]
            },
            {
                elements: "#GRNNOTo",
                rules: [
                       {
                           type: function (element) {
                               var error = false;
                               var to_id = clean($('#GRNNOToID').val());
                               var from_id = clean($('#GRNNOFromID').val());
                               if (to_id != 0) {
                                   if (to_id < from_id) {
                                       error = true;
                                   }
                               }
                               return !error;
                           }, message: 'Selected GRN No To must be greater than or equal to GRN No From'

                       },
                ]
            },



        ],
    },
};