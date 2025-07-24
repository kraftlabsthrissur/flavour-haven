
Journal.add_item_to_grid = function (item) {
    var self = Journal;
    var row_class_name = "";
    if ($('#DebitAccountHeadID').val() > 0 && $('#CreditAccountHeadID').val() > 0) {
        row_class_name = "DebitAccountHeads CreditAccountHeads";
    }
    else if ($('#DebitAccountHeadID').val() > 0) {
        row_class_name = "DebitAccountHeads";
    }
    else if ($('#CreditAccountHeadID').val() > 0) {
        row_class_name = "CreditAccountHeads";
    }
    else {
        row_class_name = "";
    }
    var index, tr;
    index = $("#journal-item-list tbody tr").length + 1;
    tr = '<tr class="' + row_class_name + '">'
            + ' <td class="uk-text-center">' + index + ' </td>'
            + ' <td class="DebitAccountCode">' + item.DebitAccountCode
            + ' <input type="hidden" class="LocationID" value="' + 0 + '" /></td>'
            + ' <input type="hidden" class="DepartmentID" value="' + 0 + '" /></td>'
            + ' <input type="hidden" class="InterCompanyID" value="' + 0 + '" /></td>'
            + ' <input type="hidden" class="EmployeeID" value="' + 0 + '" /></td>'
            + ' <input type="hidden" class="ProjectID" value="' + 0 + '" /></td>'
            + ' <input type="hidden" class="DebitAccountHeadID" value="' + $('#DebitAccountHeadID').val() + '" /></td>'
            + ' <td class="DebitAccountName">' + item.DebitAccountName
            + ' <td class="CreditAccountCode">' + item.CreditAccountCode
            + ' <input type="hidden" class="CreditAccountHeadID" value="' + $('#CreditAccountHeadID').val() + '" /></td>'
            + ' <td class="CreditAccountName">' + item.CreditAccountName
            + ' <td class="DebitAmount mask-currency">' + item.DebitAmount + '</td>'
            + ' <td class="CreditAmount mask-currency">' + item.CreditAmount + '</td>'
            + ' <td class="Remarks">' + item.Remarks + '</td>'
            + ' <td class="uk-text-center">'
            + '     <a class="remove-item">'
            + '         <i class="uk-icon-remove"></i>'
            + '     </a>'
            + ' </td>'
            + '</tr>';

    $("#item-count").val(index);
    var $tr = $(tr);
    app.format($tr);
    $("#journal-item-list tbody").append($tr);
    self.clear_item();
    fh_items.resizeHeader();
};

Journal.rules = {
    on_add_item: [

            {
                elements: "#CreditAccountCode",
                rules: [
                    {
                        type: function (element) {
                            var error = false;
                            var CreditAccountCode = $('#CreditAccountCode').val();
                            var DebitAccountCode = $('#DebitAccountCode').val();
                            return CreditAccountCode != "" || DebitAccountCode != ""

                        }, message: "Please Select Debit Account Number Or Credit Account Number"
                    }

                ]
            },
            {
                elements: "#DebitAccountCode",
                rules: [
                    //{ type: form.required, message: "Please Select Credit Account Number" },
                    {
                        type: function (element) {
                            var error = false;
                            var CreditAccountCode = $('#CreditAccountCode').val();
                            var DebitAccountCode = $('#DebitAccountCode').val();
                            if (CreditAccountCode != "" || DebitAccountCode != "") {
                                return true;
                            }
                        }, message: "Please Select Debit Account Number Or Credit Account Number"
                    }

                ]
            },
            {
                elements: "#CreditAccountHeadID",
                rule: [
                    {
                        type: function (element) {
                            return clean($("#DebitAccountHeadID").val()) == 0 ? form.non_zero(element) && form.required(element) : true;
                        }, message: "Debit Account or Credit Account is required"
                    },
                    {
                        type: function (element) {
                            return clean($("#DebitAccountHeadID").val()) == clean($(element).val()) ? false : true;
                        }, message: "Debit Account and credit acoount should not be equal"
                    }
                ]
            },
            {
                elements: "#DebitAccountHeadID",
                rules: [

                    {
                        type: function (element) {
                            return clean($("#CreditAccountHeadID").val()) == 0 ? form.non_zero(element) && form.required(element) : true;
                        }, message: "Debit Account or Credit Account is required"
                    },
                     {
                         type: function (element) {
                             return clean($("#CreditAccountHeadID").val()) == clean($(element).val()) ? false : true;
                         }, message: "Debit and credit accounts are equal"
                     },
                    {
                        type: function (element) {
                            var row, index, error = false;
                            var CreditAccountHeadID = ($('#CreditAccountHeadID').val());
                            var DebitAccountHeadID = ($('#DebitAccountHeadID').val());


                            index = $("#journal-item-list tbody tr").length;
                            if (index > 0) {
                                $("#journal-item-list tbody tr").each(function () {
                                    row = $(this);
                                    var CreditID = clean($(row).find('.CreditAccountHeadID').val());
                                    var DebitID = clean(($(row).find('.DebitAccountHeadID').val()));
                                    if (DebitID != "" || DebitID != 0) {
                                        if (CreditAccountHeadID == DebitID) {
                                            error = true;
                                        }
                                    }
                                    if (CreditID != "" || CreditID != 0) {
                                        if (DebitAccountHeadID == CreditID) {
                                            error = true;
                                        }
                                    }

                                })
                            } return !error;
                        }, message: 'Please select a different Account Code/Account Name'
                    }

                ]
            },
            {
                elements: "#CreditAmount",
                rules: [
                    {
                        type: function (element) {
                            var error = false;
                            var CreditAccountCode = $('#CreditAccountCode').val();
                            var DebitAccountCode = $('#DebitAccountCode').val();
                            var CreditAmount = clean($("#CreditAmount").val());
                            var DebitAmount = clean($("#DebitAmount").val());
                            if (CreditAccountCode != "") {
                                if (CreditAmount > 0.0) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                            if (DebitAccountCode != "") {
                                if (DebitAmount > 0.0) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                        }, message: "Please Enter Valid Credit/Debit Amount"
                    },
                ]
            },
            {
                elements: "#DebitAmount",
                rules: [
                    {
                        type: function (element) {
                            var error = false;
                            var CreditAccountCode = $('#CreditAccountCode').val();
                            var DebitAccountCode = $('#DebitAccountCode').val();
                            var CreditAmount = clean($("#CreditAmount").val());
                            var DebitAmount = clean($("#DebitAmount").val());
                            if (CreditAccountCode != "") {
                                if (CreditAmount > 0.0) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                            if (DebitAccountCode != "") {
                                if (DebitAmount > 0.0) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                        }, message: "Please Enter Valid Credit/Debit Amount"
                    },
                ]
            },
    ],
    on_submit: [
    {
        elements: "#item-count",
        rules: [
        { type: form.required, message: "Please add atleast one item" },
        {type: form.non_zero, message: "Please add atleast one item"},
        ]
    },

     {
         elements: ".DebitAccountHeads",
         rules: [
           {
               type: function (element) {
                   var debit_count = $("#journal-item-list tbody tr.DebitAccountHeads").length;
                   var credit_count = $("#journal-item-list tbody tr.CreditAccountHeads").length;
                   var error = false;
                   if (debit_count > 1 && credit_count > 1)
                   {
                       error = true;
                   }
                   return !error;
               }, message: "Invalid Journal Entry"
           },
         ]
     },

        {
            elements: "#TotalCreditAmount",
            rules: [
                { type: form.required, message: "Invalid Credit Amount" },
                { type: form.non_zero, message: "Invalid Credit Amount" },
                { type: form.positive, message: "Invalid Credit Amount" },
                      {
                          type: function (element) {
                              var error = false;
                              var TotalCreditAmount = $('#TotalCreditAmount').val();
                              var TotalDebitAmount = $('#TotalDebitAmount').val();
                              if (TotalCreditAmount != TotalDebitAmount)
                                  error = true;
                              return !error;
                          }, message: 'Credit & Debit Amount Must Be Equal'
                      },
            ]
        },
        {
            elements: "#TotalDebitAmount",
            rules: [
                { type: form.required, message: "Invalid Debit Amount" },
                { type: form.non_zero, message: "Invalid Debit Amount" },
                { type: form.positive, message: "Invalid Debit Amount" },
                 {
                     type: function (element) {
                         var error = false;
                         var TotalCreditAmount = $('#TotalCreditAmount').val();
                         var TotalDebitAmount = $('#TotalDebitAmount').val();
                         if (TotalCreditAmount != TotalDebitAmount)
                             error = true;
                         return !error;
                     },
                 },
            ]
        },
    ],
    on_draft: [
   {
       elements: "#item-count",
       rules: [
       { type: form.required, message: "Please add atleast one item" },
       {type: form.non_zero, message: "Please add atleast one item"},
       ]
   },
    ],
    };
