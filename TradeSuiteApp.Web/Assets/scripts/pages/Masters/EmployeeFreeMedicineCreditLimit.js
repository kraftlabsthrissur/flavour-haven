EmployeeFreeMedicineCreditLimit = {

    init: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        self.bind_events();
    },

    list: function () {
        var $list = $('#Free-medicine-creditlimit-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/EmployeeFreeMedicineCreditLimit/GetEmployeeFreeMedicineCreditLimitList"

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
                       }
                   },
                   { "data": "EmployeeCode", "className": "EmployeeCode" },
                   { "data": "EmployeeName", "className": "EmployeeName" },
                   {
                       "data": "NetAmount", "searchable": false, "className": "CreditLimit",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.CreditLimit + "</div>";
                       }
                   },
                    {
                        "data": "NetAmount", "searchable": false, "className": "BalAmount",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.UsedAmount + "</div>";
                        }
                    },
                     {
                         "data": "NetAmount", "searchable": false, "className": "UsedAmount",
                         "render": function (data, type, row, meta) {
                             return "<div class='mask-currency' >" + row.BalAmount + "</div>";
                         }
                     },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        $("body").on("click", "#btnAddItem", self.filter_confirm);
        $("body").on("click", ".btnSave", self.save_confirm);
        $("body").on("ifChanged", ".check-box", self.check_items);
        $('.select-all').on('ifChanged', self.select_results);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        // $("body").on("keyup change", "#tbl-Free-medicine tbody .Amount", self.update_amount);
    },
    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                offset: 0,
                limit: 0,
                EmployeeCategoryID: $('#EmployeeCategoryID').val(),
                DefaultLocationID: $('#LocationID').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_employee: function (event, item) {
        var self = EmployeeFreeMedicineCreditLimit;
        $("#EmployeeID").val(item.id),
        $("#EmployeeName").val(item.name);
        $("#EmployeeCode").val(item.Code);
    },


    select_employee: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#EmployeeName").val(Name);
        $("#EmployeeID").val(ID);
        $("#EmployeeCode").val(Code);
        UIkit.modal($('#select-employee')).hide();
    },

    select_results: function () {
        if ($(this).prop('checked') == true) {
            $(this).closest('table').find('.credit-components').iCheck('check');
        } else {
            $(this).closest('table').find('.credit-components').iCheck('uncheck');
        }
    },

    //update_amount: function () {
    //    var self = EmployeeFreeMedicineCreditLimit;
    //    var row = $(this).closest("tr");
    //    var item = {};
    //    item.Amount = clean($(row).find(".Amount").val());
    //    item.CreditLimit = clean($(row).find(".CreditLimit").val());
    //    item.UsedAmount = clean($(row).find(".UsedAmount").val());
    //    $(row).find(".CreditLimit").val(item.CreditLimit + item.Amount);
    //    $(row).find(".UsedAmount").val(item.CreditLimit + item.Amount);
    //},
    save_confirm: function () {
        var self = EmployeeFreeMedicineCreditLimit
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    Save: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        var data = self.get_data();
        if ($(this).hasClass('btnSavedraft')) {
            data.IsDraft = true;
        }
        var location = "/Masters/EmployeeFreeMedicineCreditLimit/Index";
        $.ajax({
            url: '/Masters/EmployeeFreeMedicineCreditLimit/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                }
            }
        });
    },

    get_data: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        var data = {};
        data.Date = $("#Date").val();
        data.Items = [];
        var item = {};
        $('#tbl-Free-medicine tbody tr.included').each(function () {
            item = {};
            item.EmployeeID = $(this).find(".EmployeeID").val();
            item.Amount = clean($(this).find(".Amount").val());
            item.StartDate = $(this).find(".StartDate").val();
            item.EndDate = $(this).find(".EndDate").val();
            data.Items.push(item);
        });
        return data;
    },

    get_filter_item: function () {
        var length;
        var tr = "";
        var $tr;
        var self = EmployeeFreeMedicineCreditLimit;
        $.ajax({
            url: '/Masters/EmployeeFreeMedicineCreditLimit/GetEmployeeByFilterForFreeMedicineCreditLimit/',
            dataType: "json",
            data: {
                'LocationID': $("#LocationID").val(),
                'EmployeeCategoryID': $("#EmployeeCategoryID").val(),
                'EmployeeID': $("#EmployeeID").val()
            },
            type: "POST",
            success: function (response) {
                length = response.Data.length;
                var StartDate = $("#StartDate").val();
                var EndDate = $("#EndDate").val();
                var Amount = clean($("#Amount").val());
                if (length != 0) {
                    $(response.Data).each(function (i, item) {
                        var slno = (i + 1);
                        tr += '<tr class="included">'
                            + '<td class="sl-no">' + slno + '</td>'
                             + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box credit-components"/>' + '</td>'
                            + '<td>' + item.EmployeeCode + '</td>'
                            + '<td>' + item.EmployeeName + '</td>'
                            + '<td><input type="text" class = "md-input mask-production-qty Amount"  value="' + Amount + '" /></td>'
                            + '<td><input type="text" class = "md-input mask-production-qty CreditLimit" readonly value="' + item.CreditLimit + '" />'
                            + '<input type="hidden" class="EmployeeID" value="' + item.EmployeeID + '"/>'
                            + '<input type="hidden" class="EmployeeCategoryID" value="' + item.EmployeeCategoryID + '"/>'
                            + '</td>'
                            + '<td><input type="text" class = "md-input mask-production-qty BalAmount" readonly value="' + item.BalAmount + '" /></td>'
                            + '<td><input type="text" class = "md-input mask-production-qty UsedAmount" readonly value="' + item.UsedAmount + '" /></td>'
                            + '<td><input type="text" class = "md-input  StartDate" readonly value="' + StartDate + '" />'
                            + '</td>'
                            + '<td> <input type="text" class = "md-input  EndDate" readonly value="' + EndDate + '" /> </td>'
                            + '</tr>';
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $('#tbl-Free-medicine tbody').html($tr);
                    self.count_items();
                }
            },
        });
    },

    filter_confirm: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        var count = $('#tbl-Free-medicine tbody').find('input.check-box:checked').length;
        if (count > 0) {
            app.confirm_cancel("Items will be removed", function () {
                self.get_filter_item();
            }, function () {
            })
        }
        else{
            self.get_filter_item();
        }
    },

    count_items: function () {
        var count = $('#tbl-Free-medicine tbody').find('input.check-box:checked').length;
        var tablecount = $('#tbl-Free-medicine tbody tr').length;
        $('#item-count').val(count);
        if (count == tablecount)
        {
            $('#tbl-Free-medicine .select-all').closest('div').addClass("checked")
        }
        else
        {
            $('#tbl-Free-medicine .select-all').prop("checked", false)
            $('#tbl-Free-medicine .select-all').closest('div').removeClass("checked")
        }
    },

    check_items: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        $("#tbl-Free-medicine tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
            } else {
                $(row).removeClass("included");
            }
        });
        self.count_items();
    },

    validate_form: function () {
        var self = EmployeeFreeMedicineCreditLimit;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
                 {
                     elements: "#item-count",
                     rules: [
                         { type: form.required, message: "Please add Employee" },
                         { type: form.non_zero, message: "Please add Employee" },
                         { type: form.positive, message: "Please add Employee" },
                     ],
                 },
                  {
                      elements: "tbl-Free-medicine tbody tr.included .Amount",
                      rules: [
                          { type: form.required, message: "Invalid Amount" },
                          { type: form.non_zero, message: "Invalid Amount" },
                          { type: form.positive, message: "Invalid Amount" },
                      ]
                  },
                  {
                      elements: ".StartDate",
                      rules: [
                          { type: form.required, message: "Invalid StartDate" },
                      ]
                  },
                  {
                     elements: ".EndDate",
                      rules: [
                            { type: form.required, message: "Invalid EndDate" },
                           
                        ]
                 },
        ]
    },
}