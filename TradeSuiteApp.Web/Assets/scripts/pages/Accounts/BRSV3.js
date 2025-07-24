$(function () {
    BRS.init();

});
BRS = {
    init: function () {
        var self = BRS;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
       } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.list();
        } 
        self.bind_events();
    },
    
    list : function () {
        var self = BRS;
        $('#tabs-brs').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

tabbed_list : function (type) {

    var $list;

    switch (type) {
        case "to-be--reconcile":
            $list = $('#to-be--reconcile-list');
            break;
        case "reconciled":
            $list = $('#reconciled-list');
            break;
        default:
            $list = $('#to-be--reconcile-list');
    }
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        altair_md.inputs($list);

        var url = "/Accounts/BRS/GetBRSListV3?type=" + type;
        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true,
            "pageLength": 15,
            "bAutoWidth": false,
            "bServerSide": true,
            "bSortable": false,
            "aaSorting": [[2, "desc"]],
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
                           + "<input type='hidden' class='ID' value='" + row.ID + "'>"
                           + "<input type='hidden' class='BankName' value='" + row.BankName + "'>";

                   }
               },

               { "data": "DocumentType", "className": "DocumentType" },
               { "data": "DocumentNo", "className": "DocumentNo" },
               { "data": "AccountName", "className": "AccountName" },
               { "data": "DocumentDate", "className": "DocumentDate" },
               { "data": "BankName", "className": "BankName" },
               {
                   "data": "DebitAmount", "searchable": false, "className": "DebitAmount",
                   "render": function (data, type, row, meta) {
                       return "<div class='mask-currency' >" + row.DebitAmount + "</div>";
                   }
               },
                {
                    "data": "CreditAmount", "searchable": false, "className": "CreditAmount",
                    "render": function (data, type, row, meta) {
                        return "<div class='mask-currency' >" + row.CreditAmount + "</div>";
                    }
                },
                {
                    "data": "ReconciledDate", "searchable": false, "className": "ReconciledDate",
                    "render": function (data, type, row, meta) {
                        if (row.ReconciledDate == "01-Jan-1900") {
                            return "<div class='' > </div>";
                        }
                        else {
                            return "<div class='' >" + row.ReconciledDate + "</div>";
                        }

                    }
                },
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass(data.Status);
                app.format(row);
            },
            "drawCallback": function () {
                $list.find('tbody td:not(.ReconciledDate)').on('click', function () {
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
        var self = BRS;
        UIkit.uploadSelect($("#select-bankstatement"), self.upload_bank_statement);
        $("#btnshow").on("click", self.show);
        $(".btnSaveAsDraft,.btnSaveAndNew").on("click", self.submit);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on('ifChanged', '.chkCheck', self.include_item);
    },

    save_confirm: function () {
        var self = BRS
        app.confirm_cancel("Do you want to Save", function () {
            self.submit();
        }, function () {
        })
    },


    //Latest show function
    show: function () {
        var self = BRS;
        self.error_count = 0;
        self.error_count = self.validate_Item();
        if (self.error_count > 0) {
            return;
        }
        var FromDate = $("#FromTransactionDate").val();
        var ToDate= $("#ToTransactionDate").val();
        var PatientID = $("#PatientID").val();
        var BankID = $("#BankID").val();
        $.ajax({
            url: '/Accounts/BRS/ItemsForBankReconciliation',
            dataType: "html",
            data: {
                BankID: BankID,
                FromDate: FromDate,
                ToDate: ToDate
            },
            type: "POST",
            success: function (response) {
                $("#brs-item-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#brs-item-list tbody").append($response);
            },
        })
    },

    include_item: function () {
        var self = BRS;
        var Debit = clean($(this).closest('tr').find('.Debit').text());
        var Credit = clean($(this).closest('tr').find('.Credit').text());
        var BalAsPerCompanyBooks = clean($('#BalAsPerCompanyBooks').val());
        var DebitAmountNotReflectedInBank = clean($('#DebitAmountNotReflectedInBank').val());
        var CreditAmountNotReflectedInBank = clean($('#CreditAmountNotReflectedInBank').val());
        var BalAsPerBank = clean($('#BalAsPerBank').val());
        
        

        if ($(this).is(":checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('readonly');
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            BalAsPerCompanyBooks = BalAsPerCompanyBooks + Debit - Credit;
            DebitAmountNotReflectedInBank = DebitAmountNotReflectedInBank - Debit;
            CreditAmountNotReflectedInBank = CreditAmountNotReflectedInBank - Credit;
            BalAsPerBank = BalAsPerCompanyBooks + DebitAmountNotReflectedInBank - CreditAmountNotReflectedInBank;
        } else {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').attr("disabled", true);
            $(this).closest('tr').find('input, select').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
            BalAsPerCompanyBooks = BalAsPerCompanyBooks - Debit + Credit;
            DebitAmountNotReflectedInBank = DebitAmountNotReflectedInBank + Debit;
            CreditAmountNotReflectedInBank = CreditAmountNotReflectedInBank + Credit;
            BalAsPerBank = BalAsPerCompanyBooks + DebitAmountNotReflectedInBank - CreditAmountNotReflectedInBank;
        }
        index = $("#brs-item-list tbody tr.included").length;
        $("#item-count").val(index);
        $('#BalAsPerBank').val(BalAsPerBank);
        $('#BalAsPerCompanyBooks').val(BalAsPerCompanyBooks);
        $('#DebitAmountNotReflectedInBank').val(DebitAmountNotReflectedInBank);
        $('#CreditAmountNotReflectedInBank').val(CreditAmountNotReflectedInBank);
        //self.count_items();
    },

    save: function () {
        var self = BRS;
        var modal = self.get_data();
        $.ajax({
            url: '/Accounts/BRS/SaveBankReconciledDateV3',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Bank Reconciliation Statement created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/BRS/CreateV3";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = BRS;
        var data = {};
        data.Items = [];
        var item = {};
        $("#brs-item-list tbody tr.included").each(function () {
            item = {};
            item.DocumentID = clean($(this).find(".DocumentID").val());
            item.DocumentType = $(this).find(".DocumentType").val();
            item.Remarks = $(this).find(".Remarks").val();
            item.ReferenceNo = $(this).find(".ReferenceNo").val();
            item.ReconciledDate = $(this).find(".ReconciledDate").val();
            data.Items.push(item);
        });
        return data;
    },

    save_confirm: function () {
        var self = BRS;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },


    validate_Item: function () {
        var self = BRS;
        if (self.rules.on_show.length > 0) {
            return form.validate(self.rules.on_show);
        }       
        return 0;
    },

    validate_save: function () {
        var self = BRS;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_show: [

                {
                    elements: "#BankID",
                    rules: [
                         { type: form.required, message: "Please Select Bank" },

                    ]
                },
                //{
                //    elements: "#FromTransactionDate",
                //    rules: [
                //        { type: form.required, message: "Invalid FromTransactionDate" },
                //    ]
                //},
                // {
                //     elements: "#ToTransactionDate",
                //     rules: [
                //          { type: form.required, message: "Invalid ToTransactionDate" },

                //     ]
                // }
        ],
        on_save: [

              {
                  elements: "#brs-item-list tbody tr.included td .Remarks",
                  rules: [
                     { type: form.required, message: "Please Add Remarks" },
                     { type: form.non_zero, message: "Please Add Remarks" },
                  ]
              },
              {
                  elements: "#item-count",
                  rules: [
                     { type: form.required, message: "Please Add Atleast One Item" },
                     { type: form.non_zero, message: "Please Add Atleast One Item" },
                  ]
              },
              {
                  elements: "#brs-item-list tbody tr.included td .ReferenceNo",
                  rules: [
                       { type: form.required, message: "Please Add ReferenceNo" },
                       { type: form.non_zero, message: "Please Add ReferenceNo" },
                       
                  ]
              },
              {
                  elements: "#brs-item-list tbody tr.included td .ReconciledDate",
                  rules: [
                       { type: form.required, message: "Please Add ReconciledDate" },
                  ]
              },
        ]
    },






}


