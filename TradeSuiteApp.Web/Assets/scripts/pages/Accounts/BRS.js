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

    list: function () {
        var $list = $('#brs-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#brs-list tbody tr').on('click', function () {
                var id = $(this).find('.ID').val();
                window.location = '/Accounts/BRS/Details/' + id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        var self = BRS;
        UIkit.uploadSelect($("#select-bankstatement"), self.upload_bank_statement);
        $("#btnshow").on("click", self.show);
        $(".btnSaveAsDraft,.btnSaveAndNew").on("click", self.submit);
        $(".btnSave").on("click", self.save_confirm);
    },

    save_confirm: function () {
        var self = BRS
        app.confirm_cancel("Do you want to Save", function () {
            self.submit();
        }, function () {
        })
    },

    upload_bank_statement: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(xls|xlsx|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File Extension Is InValid - Only Upload EXCEL/CSV");
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
                width = $('#selected-bankstatement').width() - 20;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-bankstatement').html("<a class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</a>");
                        success += record.Name + " " + record.Description + "<br/>";
                        $("#FilePath").val(record.Path);
                        $("#AttachmentID").val(record.ID);
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

    Read_CSV: function (extension) {

        $.ajax({
            url: "/BRS/ConvertCSVtoDataTable",
            data: {
                filepath: $('#FilePath').val(),

            },
            dataType: "json",
            type: "POST",
            success: function (BankStatements) {
                if (BankStatements.Status == "failure") {
                    app.show_error("Incorrect data in excel file");
                }
                else {
                    var $Bank_Statement_list = $('#bank-statement-item-list tbody');
                    $Bank_Statement_list.html('');
                    var tr = '';
                    var $tr;
                    $.each(BankStatements.data, function (i, BankStatement) {
                        tr = " <tr>"
                                    + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                                    + ' <td><input type="text" class="md-input uk-text BankInstrumentNumber" value="' + BankStatement.InstrumentNumber + '" /></td>'
                                    + ' <td><input type="text" class="md-input uk-text uk-icon-calendar past-date date BankInstrumentDate" value="' + BankStatement.InstrumentDate + '"</td>'
                                    + ' <td><input type="text" class="md-input uk-text mask-currency BankCredit" value="' + BankStatement.Debit + '"</td>'
                                    + ' <td><input type="text" class="md-input uk-text mask-currency BankDebit" value="' + BankStatement.Credit + '"</td>'                                  
                                    + "</tr>";
                        $tr = $(tr);
                        app.format($tr);
                        $Bank_Statement_list.append($tr);

                    });

                }
            }
        });
    },

    Read_Excel: function (extension) {

        $.ajax({
            url: "/BRS/ConvertXSLXtoDataTable",
            data: {
                filepath: $('#FilePath').val(),
                extention: extension
            },
            dataType: "json",
            type: "POST",
            success: function (BankStatements) {

                if (BankStatements.Status == "failure") {
                    app.show_error("Incorrect data in excel file");
                }
                else {
                    var $Bank_Statement_list = $('#bank-statement-item-list tbody');
                    $Bank_Statement_list.html('');
                    var tr = '';
                    var $tr;
                    $.each(BankStatements.data, function (i, BankStatement) {
                        tr += " <tr>"
                            + "<td class='uk-text-center'>" + (i + 1) + "</td>"
                            + ' <td><input type="text" class="md-input uk-text BankInstrumentNumber" value="' + BankStatement.InstrumentNumber + '" /></td>'
                            + ' <td><input type="text" class="md-input uk-text uk-icon-calendar past-date date BankInstrumentDate" value="' + BankStatement.InstrumentDate + '"</td>'
                            + ' <td><input type="text" class="md-input uk-text mask-currency BankCredit" value="' + BankStatement.Debit + '"</td>'
                            + ' <td><input type="text" class="md-input uk-text mask-currency BankDebit" value="' + BankStatement.Credit + '"</td>'
                            + "</tr>";
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $Bank_Statement_list.append($tr);
                }
            }
        });
    },

    get_status_as_per_book: function () {
        var Status = $('.status-template').html();
        $.ajax({
            url: '/Accounts/BRS/getStatusAsPerBooks',
            data: {
                FromDate: $("#FromTransactionDate").val(),
                TODate: $("#ToTransactionDate").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (bookentries) {
                var $BRS_list = $('#brs-item-list tbody');
                $BRS_list.html('');
                var tr = '';
                $.each(bookentries, function (i, bookentry) {
                    tr += " <tr>"
                        + ' <td class="BookDocumentNumber">' + bookentry.DocumentNumber + '</td>'
                        + ' <td class="BookInstrumentNumber">' + bookentry.InstrumentNumber + '</td>'
                        + ' <td class="BookInstrumentDate">' + bookentry.InstrumentDate + '</td>'
                        + ' <td class="BookCredit mask-currency">' + bookentry.Credit + '</td>'
                        + ' <td class="BookDebit mask-currency">' + bookentry.Debit + '</td>'
                        + ' <td class="BookBankCharges mask-currency">' + bookentry.BankCharges + '</td>'
                        + '<td><input type="text"  class="md-input uk-text Itemname"/></td>'
                        + '<td class="uk-text-right"><input type="text"  class="md-input uk-text-right EquivalentNumber"/></td>'
                        + "<td><select class='md-input Status'>" +Status + "</select></td>"                                                  
                        + "</tr>";                
                });
                var $tr = $(tr);
                app.format($tr);
                $BRS_list.html($tr);
            }
        });
    },

    show: function () {
        var self = BRS;
        self.error_count = 0;
        self.error_count = self.validate_Item();
        if (self.error_count > 0) {           
            return;
        }       
        var extension = $("#FilePath").val().split('.').pop();
        if (extension == "xlsx") {
            self.Read_Excel(extension);
        }
        else {
            self.Read_CSV(extension);
        }
        self.get_status_as_per_book();

        $("#FromTransactionDate, #ToTransactionDate").removeClass('md-input-danger');
        $("#FromTransactionDate, #ToTransactionDate").closest('.md-input-wrapper').removeClass('md-input-wrapper-danger');
    },

    validate_Item: function () {
        var self = BRS;
        if (self.rules.on_show.length > 0) {
            return form.validate(self.rules.on_show);
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
                {
                    elements: "#FromTransactionDate",
                    rules: [
                        { type: form.required, message: "Invalid FromTransactionDate" },
                    ]
                },
                 {
                     elements: "#ToTransactionDate",
                     rules: [
                          { type: form.required, message: "Invalid ToTransactionDate" },

                     ]
                 },
                 {
                     elements: "#select-bankstatement",
                     rules: [
                          { type: form.required, message: "Please Select any File " },

                     ]
                 },
                
        ],
        on_submit: [
             {
                 elements: "#BankID",
                 rules: [
                      { type: form.required, message: "Please Select Bank" },

                 ]
             },                      
        ],
    },

    validate_form: function () {
        var self = BRS;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    submit: function () {
        var self = BRS;
        
        if ($("#bank-statement-item-list tbody tr").length > 0) {

            var location = "/Accounts/BRS/Index";
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count == 0) {
                var model = self.get_data();
                $(".btnSaveAndNew, .btnSave, .btnSaveAsDraft ").css({ 'display': 'none' });
                if ($(this).hasClass("btnSaveAsDraft")) {
                    model.IsDraft = true;
                }

                if ($(this).hasClass("btnSaveAndNew")) {
                    location = "/Accounts/BRS/Create";
                }

                $.ajax({
                    url: '/Accounts/BRS/Save',
                    data: { model: model },
                    dataType: "json",
                    type: "POST",
                    success: function (data) {
                        if (data == "success") {
                            app.show_notice("BRS Saved successfully");
                            setTimeout(function () {
                                window.location = location;
                            }, 1000);
                        } else {
                            app.show_error("Failed to Save BRS");
                            $(".btnSaveAndNew, .btnSave, .btnSaveAsDraft ").css({ 'display': 'block' });
                        }
                    },
                });
            }
        }else {
            app.show_error("No Data Found");
        }
    },

    get_data: function () {
        var self = BRS;

        var model = {
            ID : $("#ID").val(),
            TransNo: $("#TransNo").val(),
            Date: $("#Date").val(),
            BankID: $("#BankID").val(),           
            FromTransactionDate: $("#FromTransactionDate").val(),
            ToTransactionDate: $("#ToTransactionDate").val(),           
            AttachmentID: $('#AttachmentID').val(),
            FilePath: $('#FilePath').val(),
            FileName: $('#FilePath').val().split('\\').pop(),
        };
        model.Items = self.GetProductList();
        model.Statements = self.GetStatementList();
        return model;
    },

    GetProductList: function () {
        var ProductsArray = [];

        var row;
        $("#brs-item-list tbody tr").each(function () {
            row = $(this);
            var DocumentNumber = $(row).find('.BookDocumentNumber').text();
            var InstrumentNumber = $(row).find('.BookInstrumentNumber').text();
            var InstrumentDate = $(row).find('.BookInstrumentDate').text();
            var Debit = clean($(row).find('.BookCredit').val());
            var Credit = clean($(row).find('.BookDebit').val());
            var BankCharges = clean($(row).find('.BookBankCharges').val());
            var EquivalentBankTransactionNumber = $(row).find('.EquivalentNumber').val();
            var Status = $(row).find('.Status').val();
            ProductsArray.push({
                DocumentNumber: DocumentNumber,
                InstrumentNumber: InstrumentNumber,
                InstrumentDate: InstrumentDate,
                Debit: Debit,
                Credit: Credit,
                BankCharges: BankCharges,
                EquivalentBankTransactionNumber: EquivalentBankTransactionNumber,
                Status: Status,
            });
        })
        return ProductsArray;
    },

    GetStatementList: function () {
        var ProductsArray = [];

        var row;
        $("#bank-statement-item-list tbody tr").each(function () {
            row = $(this);
           
            var InstrumentNumber = $(row).find('.BankInstrumentNumber').val();
            var InstrumentDate = $(row).find('.BankInstrumentDate').val();
            var Debit = clean($(row).find('.BankCredit').val());
            var Credit = clean($(row).find('.BankDebit').val());

            ProductsArray.push({              
                InstrumentNumber: InstrumentNumber,
                InstrumentDate: InstrumentDate,
                Debit: Debit,
                Credit: Credit               
            });
        })
        return ProductsArray;
    },

}


