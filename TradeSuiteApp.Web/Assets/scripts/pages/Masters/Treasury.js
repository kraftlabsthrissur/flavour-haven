$(function () {
    treasury.treasury_list();
    treasury.bind_events();
    treasury.set_ispaymentisreceipt();
})
treasury = {

    treasury_list: function () {
        var $list = $('#treasury-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/Treasury/GetTreasuryList"

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
                    + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                       }
                   },
                   { "data": "Type", "className": "Type" },
                   { "data": "AccountCode", "className": "AccountCode" },
                   //{ "data": "BankName", "className": "BankName" },
                   { "data": "AliasName", "className": "AliasName" },
                   //{ "data": "CoBranchName", "className": "CoBranchName" },
                   //{ "data": "BankBranchName", "className": "BankBranchName" },
                   //{ "data": "AccountType1", "className": "AccountType1" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/Treasury/Details/" + Id);
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
        $.UIkit.autocomplete($('#accountcode-autocomplete'), { 'source': treasury.get_item_AutoComplete, 'minLength': 1 });
        $('#accountcode-autocomplete').on('selectitem.uk.autocomplete', treasury.set_item_details);
        $(".btnUpdate").on('click', treasury.update);
        $(".btnSave").on('click', treasury.save_confirm);
        $('#Type').on('change', treasury.change_type);
    },

    set_ispaymentisreceipt: function () {
        var self = treasury;
        $('#IsPayment').val() == "True" ? $('.IsPayment').iCheck('check') : $('.IsPayment').iCheck('uncheck');
        $('#IsReceipt').val() == "True" ? $('.IsReceipt').iCheck('check') : $('.IsReceipt').iCheck('uncheck');
    },

    save_confirm: function () {
        var self = treasury;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    
    change_type: function () {
        var value = $(this).val();
        if (value == 'Cash') {
            $('#AccountNo').attr("disabled", "disabled").removeClass("enabled");
            $('#AccountNo').val('');           
            $('#IFSC').attr("disabled", "disabled").removeClass("enabled");
            $('#IFSC').val('');
           
        } else {
            $('#AccountNo').removeAttr("disabled", '').addClass("enabled");
            $('#IFSC').removeAttr("disabled", '').addClass("enabled");          

        }
    },
   // error_count: 0,
    save: function () {
        var self = treasury;
        $('.btnSave').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Treasury/Create',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Treasury saved successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Treasury/Index";
                    }, 1000);
                }
                else {
                    app.show_error("exists");
                    //$('.btnSave').css({ 'display': 'block' });
                }
            }
        });
    },
        
        

    update: function () {
        var self = treasury;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        $('.btnUpdate').css({ 'display': 'none' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Treasury/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Treasury updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Treasury/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.update').css({ 'display': 'block' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            ID: $("#ID").val(),
            OpeningAmount: clean($("#OpeningAmount").val()),
            Type: $("#Type").val(),
            BankCode: $("#BankCode").val(),
            AccountCode: $("#AccountCode").val(),
            BankName: $("#BankName").val(),
            AliasName: $("#AliasName").val(),
            CoBranchName: $("#CoBranchName").val(),
            BankBranchName: $("#BankBranchName").val(),
            AccountType1: $("#AccountType1").val(),
            AccountType2: $("#AccountType2").val(),
            AccountNo: $("#AccountNo").val(),
            IFSC: $("#IFSC").val(),
            LocationMappingID: $('.Location').val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            remarks: $("#remarks").val(),
            IsPayment: $(".IsPayment").prop("checked") ? true : false,
            IsReceipt: $(".IsReceipt").prop("checked") ? true : false, 
            
        }
        return model;
    },
    validate_form: function () {
        var self = treasury;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    error_count: 0,
    rules: {
        on_submit: [
           {
               elements: "#Type",
               rules: [
                  { type: form.required, message: "Type is required" },
               ]
           },
            //{
            //    elements: "#BankCode",
            //    rules: [
            //       { type: form.required, message: "BankCode is required" },
            //    ]
            //},
            //{
            //    elements: "#AccountCode",
            //    rules: [
            //        { type: form.required, message: "AccountCode is Required" },
            //    ]
            //},
            //{
            //    elements: "#BankName",
            //    rules: [
            //        { type: form.required, message: "BankName is Required" },
            //    ]
            //},
            {
                elements: "#AliasName",
                rules: [
                   { type: form.required, message: "Name is required" },
                ]
            },
            //{
            //    elements: "#CoBranchName",
            //    rules: [
            //       { type: form.required, message: "CoBranchName is required" },
            //    ]
            //},
            //{
            //    elements: "#AccountType1",
            //    rules: [
            //        { type: form.required, message: "AccountType1 is Required" },
            //    ]
            //},

            //{
            //    elements: "#AccountNo.enabled",
            //    rules: [
            //        { type: form.required, message: "AccountNo is Required" },
            //    ]
            //},
            //{
            //    elements: "#IFSC.enabled",
            //    rules: [
            //       { type: form.required, message: "IFSC is required" },
            //    ]
            //},
            //{
            //    elements: "#BankBranchName",
            //    rules: [
            //        { type: form.required, message: "BankBranchName is Required" }
            //    ]
            //},            
            //{
            //    elements: "#EndDate",
            //    rules: [
            //        { type: form.required, message: "EndDate is Required" },
            //        {
            //            type: function (element) {
            //                var u_date = $(element).val().split('-');
            //                var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
            //                var a = Date.parse(used_date);
            //                var po_date = $('#StartDate').val().split('-');
            //                var po_datesplit = new Date(po_date[2], po_date[1] - 1, po_date[0]);
            //                var date = Date.parse(po_datesplit);
            //                return date <= a
            //            }, message: "End date should be a date on or after start date"
            //        }
            //    ]
            //},
        ]
    },

    get_item_AutoComplete : function (release) {
        var self = treasury;
        $('#AccountID').val('');
        $.ajax({
            url: '/Masters/Treasury/GetAccountCodeAutoComplete',
            data: {
                CodeHint: $('#AccountCode').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_item_details: function (event, item) {
        var self = treasury;
        $("#AccountCode").val(item.Code);
        $("#AccountID").val(item.id); 
        $("#AccountName").val(item.accountname);
    }
}

