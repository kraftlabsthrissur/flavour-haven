$(function () {
    TrialBalance.init();
});

TrialBalance = {
    init: function () {
        var self = TrialBalance;
        self.bind_events();
        ReportHelper.init();
        //self.get_report_type();
    },

    bind_events: function () {
        var self = TrialBalance;
        $('#tds-transaction-no-autocomplete').on('selectitem.uk.autocomplete', self.set_tds_transaction_no);
        $('#suppliername-autocomplete').on('selectitem.uk.autocomplete', self.set_supplier_name);
        $('#tdscode-autocomplete').on('selectitem.uk.autocomplete', self.set_tds_code);
        $('#panno-autocomplete').on('selectitem.uk.autocomplete', self.set_pan_no);
        $('#Refresh').on('click', self.refresh);
    },

    set_tds_transaction_no: function (event, item) {
        self = TrialBalance;
        $("#TransactionNo").val(item.id);
    },

    set_supplier_name: function (event, item) {   // on select auto complete item
        self = TrialBalance;
        $("#SupplierName").val(item.name);
        $("#SupplierID").val(item.id);
    },

    set_tds_code: function (event, item) {   // on select auto complete item
        self = TrialBalance;
        $("#TDSCode").val(item.name);
        $("#TDSCodeID").val(item.id);
    },

    set_pan_no: function (event, item) {   // on select auto complete item
        self = TrialBalance;
        $("#PanNo").val(item.name);
        $("#PanNoID").val(item.id);
    },

    get_filters: function () {
        var self = TrialBalance;
        var filters = "";

        if ($("#TransactionNo").val() != "") {
            filters += "TransactionNo: " + $("#TransactionNo").val() + " , ";
        }

        //filters += self.get_supplier_title();

        if ($("#SupplierID").val() != 0) {
            filters += "Supplier: " + $("#SupplierName").val() + ", ";
        }

        if ($("#LocationID").val() != "") {
            filters += "Location: " + $("#LocationID Option:selected").text() + ", ";
        }

        if ($("#TDSCodeID").val() != 0) {
            filters += "TDS Code: " + $("#TDSCode").val() + ", ";
        }

        if ($("#PanNoID").val() != 0) {
            filters += "Pan No: " + $("#PanNo").val() + ", ";
        }

        if (filters != "") {
            filters = filters.replace(/,(\s+)?$/, '');
            filters = "Filters applied: " + filters;
        }
        return filters;
    },
    refresh: function () {
        var self = TrialBalance;
        var locationID = $("#LocationID").val();
        var currentdate = $("#CurrentDate").val();
        var findate = $("#FinStartDate").val();
        $('#FromDateString').val(findate);
        $('#ToDateString').val(currentdate);
        $('#TransactionNo').val('');
        $('#SupplierName').val('');
        $('#SupplierID').val('');
        $('#LocationID').val('');
        $('#TDSCode').val('');
        $('#TDSCodeID').val('');
        $('#PanNo').val('');
        $('#PanNoID').val('');
    },

    get_supplier_title: function () {
        self = TrialBalance;
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
    }
}

