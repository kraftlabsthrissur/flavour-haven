$(function () {
    PurchaseReturn.bind_events();

});
var is_first_run = true;
PurchaseReturn = {

    bind_events: function () {
        $("#report-filter-form").css({ 'min-height': $(window).height() });
        $('#report-filter-submit').on("click", PurchaseReturn.get_report_view);
        $('#report-filter-reset').on("click", PurchaseReturn.rest_report_view);
        $('.pur-return-type').on('ifChanged', PurchaseReturn.show_pur_report_type);
        $.UIkit.autocomplete($('#PurchaseReturnNOFrom-autocomplete'), { 'source': PurchaseReturn.get_purchase_return_no, 'minLength': 1 });
        $('#PurchaseReturnNOFrom-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_purchase_return_no);

        //$.UIkit.autocomplete($('#PurchaseReturnNOFrom-autocomplete'), { 'source': PurchaseReturn.get_purchase_return_no, 'minLength': 1 });
        //$('#PurchaseReturnNOFrom-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_purchase_return_no);

        //$.UIkit.autocomplete($('#PurchaseReturnNo-autocomplete'), { 'source': PurchaseReturn.get_purchase_return_no, 'minLength': 1 });
        //$('#PurchaseReturnNo-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_purchase_return_no);
        
        $.UIkit.autocomplete($('#PurchaseReturnNoTo-autocomplete'), { 'source': PurchaseReturn.get_purchase_return_no_to, 'minLength': 1 });
        $('#PurchaseReturnNoTo-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_purchase_return_no_to);

        $.UIkit.autocomplete($('#SupplierName-autocomplete'), { 'source': PurchaseReturn.get_suppliers, 'minLength': 1 });
        $('#SupplierName-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_suppliers);
        $.UIkit.autocomplete($('#supplierNameDetailed-autocomplete'), { 'source': PurchaseReturn.get_suppliers, 'minLength': 1 });
        $('#supplierNameDetailed-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_suppliers);

        
        $.UIkit.autocomplete($('#ItemName-autocomplete'), { 'source': PurchaseReturn.get_item_details, 'minLength': 1 });
        $('#ItemName-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_item_details);

        $.UIkit.autocomplete($('#PurcahseReturnGrnNoFrom-autocomplete'), { 'source': PurchaseReturn.get_grnno, 'minLength': 1 });
        $('#PurcahseReturnGrnNoFrom-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_grnno);
        $.UIkit.autocomplete($('#PurcahseReturnGrnNoTo-autocomplete'), { 'source': PurchaseReturn.get_grnnoTo, 'minLength': 1 });
        $('#PurcahseReturnGrnNoTo-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_grnnoTo);
        //$.UIkit.autocomplete($('#PurchaseReturnGrnNo-autocomplete'), { 'source': PurchaseReturn.get_grnno, 'minLength': 1 });
        //$('#PurchaseReturnGrnNo-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_grnno);


        $.UIkit.autocomplete($('#qcno-autocomplete'), { 'source': PurchaseReturn.get_qcno, 'minLength': 1 });
        $('#qcno-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_qcno);
        $.UIkit.autocomplete($('#qcnoTo-autocomplete'), { 'source': PurchaseReturn.get_qcnoTo, 'minLength': 1 });
        $('#qcnoTo-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_qcnoTo);

        //$.UIkit.autocomplete($('#PurchaseReturnqcno-autocomplete'), { 'source': PurchaseReturn.get_qcnoTo, 'minLength': 1 });
        //$('#PurchaseReturnqcno-autocomplete').on('selectitem.uk.autocomplete', PurchaseReturn.set_qcnoTo);
        $("#FromSupplierRange").on("change", PurchaseReturn.get_to_range);
        $("#FromItemNameRange").on("change", PurchaseReturn.get_to_itemrange);

    },

    get_report_view: function (e) {
        e.preventDefault();
        self = PurchaseReturn;
        self.error_count = 0;
       // self.error_count = self.validate_form();
      // if (self.error_count > 0) {
      //     return;
     //   }
        var disabled = $("#report-filter-form").find(':input:disabled').removeAttr('disabled');
        var data = $("#report-filter-form").serialize();
        disabled.attr('disabled', 'disabled');
        var url = $("#report-filter-form").attr("action");
        
        //url = str.split("/");
        var item = url.split("/");
        var name = item[3];
        var filters = "";
        //var type = $(".summary:checked").val();
        ReportHelper.hide_controls();
        switch (name) {
            case "PurchaseReturn":

                if (($("#PurchaseRturnNOFrom").val() + $("#PurchaseRturnNOTo").val()).trim() != "") {
                    if ($("#PurchaseRturnNOFrom").val().trim() == "" || $("#PurchaseRturnNOTo").val().trim() == "") {
                        filters += "Request Return No: " + $("#PurchaseRturnNOFrom").val() + $("#PurchaseRturnNOTo").val() + ", ";
                    } else {
                        filters += "Request Return No: " + $("#PurchaseRturnNOFrom").val() + " - " + $("#PurchaseRturnNOTo").val() + ", ";
                    }
                }

                if (($("#GRNNOFrom").val() + $("#GRNNOTo").val()).trim() != "") {
                    if ($("#GRNNOFrom").val().trim() == "" || $("#GRNNOTo").val().trim() == "") {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + $("#GRNNOTo").val() + ", ";
                    } else {
                        filters += "Request GRN No: " + $("#GRNNOFrom").val() + " - " + $("#GRNNOTo").val() + ", ";
                    }
                }

                if (($("#QCNOFrom").val() + $("#QCNOTo").val()).trim() != "") {
                    if ($("#QCNOFrom").val().trim() == "" || $("#QCNOTo").val().trim() == "") {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + $("#QCNOTo").val() + ", ";
                    } else {
                        filters += "Request QC No: " + $("#QCNOFrom").val() + " - " + $("#QCNOTo").val() + ", ";
                    }
                }

                filters += self.get_supplier_title();

                filters += self.get_item_title();

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
                $("#report-viewer").html(response)
                ReportHelper.inject_js();
            }
        })
        return false;
    },

    get_item_title: function () {
        self = PurchaseReturn;
        var filters = "";
        if ($("#ItemID").val() == "") {
            if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
                if ($("#ToItemNameRange").val().trim() == "") {
                    filters += "Item Name Range " + "From: " + $("#FromItemNameRange").val();
                }
                else if ($("#FromItemNameRange").val().trim() == "") {
                    filters += "Item Name Range " + "To: " + $("#ToItemNameRange").val() + ", ";
                }
                else if (($("#FromItemNameRange").val() + $("#ToItemNameRange").val()).trim() != "") {
                    filters += "Item Name Range: " + $("#FromItemNameRange").val() + " - " + $("#ToItemNameRange").val() + ", ";
                }
            }
        }

        if ($("#ItemID").val() != 0) {
            filters += "Item: " + $("#ItemName").val() + ", ";
        }
        return filters;
    },

    get_supplier_title: function () {
        self = PurchaseReturn;
        var filters = "";
        if ($("#SupplierID").val() == "") {
            if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
                if ($("#ToSupplierRange").val().trim() == "") {
                    filters += "Supplier Name Range " + "From: " + $("#FromSupplierRange").val() + ", ";
                }
                else if ($("#FromSupplierRange").val().trim() == "") {
                    filters += "Supplier Name Range " + "To: " + $("#ToSupplierRange").val() + ", ";
                }
                else if (($("#FromSupplierRange").val() + $("#ToSupplierRange").val()).trim() != "") {
                    filters += "Supplier Name Range: " + $("#FromSupplierRange").val() + " - " + $("#ToSupplierRange").val() + ", ";
                }
            }
        }

        if ($("#SupplierID").val() != 0) {
            filters += "Supplier: " + $("#SupplierName").val() + ", ";
        }
        return filters;
    },


    rest_report_view: function () {
        self = PurchaseReturn;
        $("#PurchaseRturnNOFrom").val('');
        $("#PurchaseRturnNOTo").val(''); 
        $("#PurchaseRturnNO").val('');
        $("#FromSupplierRange").val('');
        $("#ToSupplierRange").val('');
        $("#SupplierName").val('');
        $("#FromItemNameRange").val('');
        $("#ToItemNameRange").val('');
        $("#ItemName").val('');
        $("#GRNNOFrom").val('');
        $("#GRNNOTo").val('');
        $("#QCNOFrom").val('');
        $("#QCNOTo").val('');
        $("#PurchaseReturnNo").val('');
        $("#GRNNO").val('');
        $("#QCNO").val('');
        $("#SupplierNameDetailed").val('');
        $("#purchaseRetunNoFromId").val('');
        $("#purchaseRetunNoTOId").val('');
        $("#purchaseRetunNoFromId").val('');
        $("#SupplierID").val('');
        $("#ItemID").val('');
        $("#GRNNOFromID").val('');
        $("#GRNNOToID").val('');
        $("#GRNNOToID").val('');
        $("#QCNOFromID").val('');
        $("#QCNOToID").val('');

        $("#QCNOToID").val('');
        $("#SupplierID").val('');
        $("#GRNNOToID").val('');
        $("#QCNOFromID").val('');
        $("#QCNOToID").val('');
    },


    show_pur_report_type: function () {
        self = PurchaseReturn;
        if (is_first_run) {
            is_first_run = false;
            setTimeout(function () { is_first_run = true }, 100);
        } else {
            return;
        }        
        var item_type = $(this).val();
        console.log(item_type);
        if (item_type == "Summary") {
            $(".summary").addClass("uk-hidden");
            $(".detail").removeClass("uk-hidden");
            $(".lbl-grn-no").text('GRN No ');
            $(".lbl-qc-no").text('QC No');
            $(".lbl-return-no").text('Purchase Return No');
            self.rest_report_view();
        }
        else {
            $(".summary").removeClass("uk-hidden");
            $(".detail").addClass("uk-hidden");
            $(".lbl-grn-no").text('GRN No From');
            $(".lbl-qc-no").text('QC No From ');
            $(".lbl-return-no").text('Pur. Return NO From');
            self.rest_report_view();
        }
        return false;
    },

    get_purchase_return_no: function (release) {
        var Table;
        Table = 'PurchaseReturn';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PurchaseRturnNOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_purchase_return_no: function (event, item) {
        self = PurchaseReturn;
        console.log(item);
        $("#PurchaseRturnNOFrom").val(item.value);
        $("#purchaseRetunNoFromId").val(item.id);
        var report_type = $(".pur-return-type:checked").val();
        if (report_type == "Detail") {//in detail, only selected po no must be shown so from and to id must be same
            $("#purchaseRetunNoTOId").val(item.code);
            $("#purchaseRetunNoTOId").val(item.id);
        }else {        
            $("#purchaseRetunNoTOId").val('');
            $("#purchaseRetunNoTOId").val('');
        }        
    },

    get_purchase_return_no_to: function (release) {
        var Table;
        Table = 'PurchaseReturn';
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PurchaseRturnNOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_purchase_return_no_to: function (event, item) {
        self = PurchaseReturn;
        console.log(item);
        $("#PurchaseRturnNOTo").val(item.value);
        $("#purchaseRetunNoTOId").val(item.id);
    },

    get_suppliers: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetGRNWiseSupplierForAutoComplete',
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

    set_suppliers: function (event, item) {
        self = PurchaseReturn;
        $("#SupplierName").val(item.value);
        $("#SupplierID").val(item.id);
    },

    // need correction for purchase return report
    get_item_details: function (release) {
        var area = "Stock";      
        $.ajax({
            url: '/Reports/Purchase/GetItemsForAutoComplete',           
            data: {
                Hint: $('#ItemName').val(),
                Areas: area,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    }, 
    set_item_details: function (event, item) {
        $("#ItemName").val(item.value);
        $("#ItemID").val(item.id);
    },

    get_grnno: function (release) {
        self = PurchaseReturn;
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',

            data: {
                Term: $('#GRNNOFrom').val(),
                Table: 'GRN'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_grnno: function (event, item) {
        self = PurchaseReturn;

        $("#GRNNOFrom").val(item.code);
        $("#GRNNOFromID").val(item.id);
        var type = $(".pur-return-type:checked").val();
        if (type == 'Detail') {
            $("#GRNNOTo").val(item.code);
            $("#GRNNOToID").val(item.id);

        } else {
            $("#GRNNOTo").val('');
            $("#GRNNOToID").val('');
        }
    },

    get_grnnoTo: function (release) {
        self = PurchaseReturn;
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#GRNNOTo').val(),
                Table: 'GRN'
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_grnnoTo: function (event, item) {
        self = PurchaseReturn;
        $("#GRNNO").val(item.value);
        $("#GRNNOToID").val(item.id);
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
        self = PurchaseReturn;
        $("#QCNOFrom").val(item.value);       
        $("#QCNOFromID").val(item.id);
        var type = $(".pur-return-type:checked").val();
        if (type == 'Detail') {
            $("#QCNOTo").val(item.value);
            $("#QCNOToID").val(item.id);

        }else{
            $("#QCNOTo").val('');
            $("#QCNOToID").val('');
        }
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
        self = PurchaseReturn;
        $("#QCNOTo").val(item.value);       
        $("#QCNOToID").val(item.id);
    },
    
    get_to_range: function () {
        var self = PurchaseReturn;
        var from_range = $("#FromSupplierRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetRange/',
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

    get_to_itemrange: function () {
        var self = PurchaseReturn;
        var from_range = $("#FromItemNameRange").val();
        $.ajax({
            url: '/Reports/Purchase/GetItemRange/',
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
};