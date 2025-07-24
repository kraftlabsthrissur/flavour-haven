QC = {
    init: function () {
        var self = QC;
        self.bind_events();
    },

    details: function () {
        var self = QC;
    },

    list: function () {
        var self = QC;
        $('#tabs-qc').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

    },

    tabbed_list: function (type) {
        var self = QC;
        var $list;
        switch (type) {
            case "pending-qc":
                $list = $('#pending-qc-items-list');
                break;
            case "on-going-qc":
                $list = $('#ongoing-qc-items-list');
                break;
            case "completed-qc":
                $list = $('#completed-qc-items-list');
                break;
            default:
                $list = $('#pending-qc-items-list');
        }
        if (type == "completed-qc")
        {
            self.completed_qc_list($list,type);
        }
        else
        {
            self.pending_going_list($list,type);
        }
    },

    pending_going_list: function ($list,type) {
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            var url = "/Manufacturing/QCProduction/GetQCList?type=" + type;
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

                       }
                   },

                   { "data": "ProductionNo", "className": "ProductionNo" },
                   { "data": "ReceiptDate", "className": "ReceiptDate" },
                   { "data": "Item", "className": "Item" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "Unit", "className": "Unit" },
                   
                    {
                        "data": "BatchSize", "searchable": false, "className": "BatchSize",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.BatchSize + "</div>";
                        }
                    },
                    {
                        "data": "AcceptedQuantity", "searchable": false, "className": "AcceptedQuantity",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.AcceptedQuantity + "</div>";
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass("draft");
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content('/Manufacturing/QCProduction/Edit/' + Id);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    completed_qc_list: function ($list,type) {
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);
            var url = "/Manufacturing/QCProduction/GetQCList?type=" + type;
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

                       }
                   },

                   { "data": "ProductionNo", "className": "ProductionNo" },
                   { "data": "ReceiptDate", "className": "ReceiptDate" },
                   { "data": "Item", "className": "Item" },
                   { "data": "BatchNo", "className": "BatchNo" },
                   { "data": "Unit", "className": "Unit" },
                   {
                       "data": "AcceptedQuantity", "searchable": false, "className": "AcceptedQuantity",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.AcceptedQuantity + "</div>";
                       }
                   },
                    {
                        "data": "ApprovedQuantity", "searchable": false, "className": "ApprovedQuantity",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.ApprovedQuantity + "</div>";
                        }
                    },
                    

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.QCStatus);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content('/Manufacturing/QCProduction/Details/' + Id);
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
        $('.approve-qc').on('click', this.approve);
        $('.reject-qc').on('click', this.reject);
        $('.save-qc').on('click', this.save);
        $('.overrule-qc').on('click', this.overrule);
        $('.edit-qc').on('click', this.edit);
        $('.passed').on('ifChanged', QC.validate_failed);
        $('.failed').on('ifChanged', QC.validate_passed);
        $('.select-all').on('ifChanged', QC.select_results);
    },

    select_results: function () {

        if ($(this).prop('checked') == true) {
            if ($(this).hasClass('all-passed')) {
                $(this).closest('th').find('.all-failed').iCheck('uncheck');
                $(this).closest('table').find('.failed').iCheck('uncheck');
                $(this).closest('table').find('.passed').iCheck('check');
            } else {
                $(this).closest('th').find('.all-passed').iCheck('uncheck');
                $(this).closest('table').find('.passed').iCheck('uncheck');
                $(this).closest('table').find('.failed').iCheck('check');
            }
        } else {
            if ($(this).hasClass('all-passed')) {
                $(this).closest('table').find('.passed').iCheck('uncheck');
            } else {
                $(this).closest('table').find('.failed').iCheck('uncheck');
            }
        }
    },

    validate_failed: function () {
        passed = $(this).closest('tr').find('.passed').prop("checked");
        failed = $(this).closest('tr').find('.failed').prop("checked");
        if ((passed == true) && (failed == true)) {
            $(this).closest('tr').find('.failed').iCheck('uncheck');
            $(this).closest('table').find('.select-all').closest('div.icheckbox_md').removeClass('checked');
            $(this).closest('table').find('.select-all').removeAttr('checked');

        }
    },

    validate_passed: function () {
        passed = $(this).closest('tr').find('.passed').prop("checked");
        failed = $(this).closest('tr').find('.failed').prop("checked");
        if ((passed == true) && (failed == true)) {
            $(this).closest('tr').find('.passed').iCheck('uncheck');
            $(this).closest('table').find('.select-all').closest('div.icheckbox_md').removeClass('checked');
            $(this).closest('table').find('.select-all').removeAttr('checked');
        }
    },

    approve: function () {
        var self = QC;
        self.error_count = 0;
        self.validate_form();
        self.validate_results();

        if (self.error_count == 0) {
            if (self.error_count == 0) {
                var data = self.get_data();
                $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'none' });

                $.ajax({
                    url: '/Manufacturing/QCProduction/approve/' + data.QCItem.ID,
                    data: data,
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        if (response.Status == "success") {
                            app.show_notice("Approved successfully");
                            setTimeout(function () {
                                window.location = "/Manufacturing/QCProduction/Index";
                            }, 1000);
                        } else {
                            if (typeof response.data[0].ErrorMessage != "undefined")
                                app.show_error(response.data[0].ErrorMessage);
                            $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'block' });

                        }
                    }
                });
            }
        }
    },

    reject: function () {
        QC.error_count = 0;
        QC.validate_form();
        QC.validate_remark();
        QC.validate_tests();
        if (QC.error_count == 0) {
            var data = QC.get_data();
            $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'none' });
            $.ajax({
                url: '/Manufacturing/QCProduction/reject/' + data.QCItem.ID,
                data: data,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Rejected successfully");
                        setTimeout(function () {
                            window.location = "/Manufacturing/QCProduction/Index";
                        }, 1000);
                    } else {
                        if (typeof response.data[0].ErrorMessage != "undefined")
                            app.show_error(response.data[0].ErrorMessage);
                        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'block' });

                    }
                }
            });
        }
    },

    overrule: function () {
        QC.error_count = 0;
        QC.validate_form();
        QC.validate_remark();
        //  QC.validate_tests();
        if (QC.error_count == 0) {
            var data = QC.get_data();
            $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'none' });
            $.ajax({
                url: '/Manufacturing/QCProduction/overrule/' + data.QCItem.ID,
                data: data,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("Overruled successfully");
                        setTimeout(function () {
                            window.location = "/Manufacturing/QCProduction/Index";
                        }, 1000);
                    } else {
                        if (typeof response.data[0].ErrorMessage != "undefined")
                            app.show_error(response.data[0].ErrorMessage);
                        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'block' });
                    }
                }
            });
        }
    },

    save: function () {
        var data = QC.get_data();
        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'none' });
        $.ajax({
            url: '/Manufacturing/QCProduction/save/' + data.QCItem.ID,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    setTimeout(function () {
                        window.location = "/Manufacturing/QCProduction/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc").css({ 'display': 'block' });

                }
            }
        });
    },

    get_data: function () {
        var data = {
            QCItem: {
                "ID": $('#QCItem_ID').val(),
                "ItemID": $('#QCItem_ItemID').val(),
                "Remarks": $('#qc-remarks').val(),
                "ToWareHouseID": $('#QCItem_ToWareHouseID').val(),
                "QCDate": $('#QCDate').val(),
                "ApprovedQty": $("#ApprovedQty").val().replace(/\,/g, '')
            },
            testResults: [],
        }
        var test_details;
        var actual_result;

        $('.analisys tbody tr').each(function () {
            if ($(this).closest('tr').find('.passed').prop("checked") == true) {
                actual_result = "Passed";
            }
            else if ($(this).closest('tr').find('.failed').prop("checked") == true) {
                actual_result = "Failed";
            }
            else {
                actual_result = "";
            }
            test_details = {
                ID: $(this).find('.ID').val(),
                ActualValue:clean( $(this).find('.qc-test-actual-value').val()),
                ActualResult: actual_result,
                Remarks: $(this).find('.remarks').val()
            }
            data.testResults.push(test_details);
        });
        return data;
    },

    edit: function () {
        console.log('edit');
    },

    error_count: 0,

    validate_results: function () {
        var actual_value;
        var test_name;
        var range_from;
        var range_to;
        var definedresult;
        var is_mandatory;
        var msg = "";
        $('.qc-test-actual-value.enabled').each(function () {
            actual_value = clean($(this).val());
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            range_from = clean($(this).closest('tr').find('.range-from').text());
            range_to = clean($(this).closest('tr').find('.range-to').text());
            is_mandatory = $(this).closest('tr').find('.IsMandatory').val();
            if (is_mandatory == "True") {
                if ((actual_value > range_to) || (actual_value < range_from)) {
                    QC.error_count++;
                    msg += test_name + " should fall between " + range_from + " and " + range_to + "<br/>";
                    app.add_error_class($(this));
                }
            }
        });
        $('.analisysis tbody tr').each(function () {
            passed = $(this).closest('tr').find('.passed').prop("checked");
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            failed = $(this).closest('tr').find('.failed').prop("checked");
            is_mandatory = $(this).closest('tr').find('.IsMandatory').val();
            if (is_mandatory == "True") {
                if ((passed == false) && (failed == false)) {
                    QC.error_count++;
                    app.show_error(test_name + " should have actual value");
                    app.add_error_class($(this));
                }
                else if (passed == failed) {
                    QC.error_count++;
                    app.show_error(test_name + " should have one actual value");
                    app.add_error_class($(this));
                }
            }
        });
        failed_count = $('.analisysis tbody tr .failed:checkbox:checked').length;
        if (failed_count > 0) {
            QC.error_count++;
            app.show_error("All tests are not passed");
            app.add_error_class($(this));

        }


        if (msg != "") {
            app.show_error(msg);
        }
    },

    validate_tests: function () {
        var actual_value;
        var test_name;
        var range_from;
        var range_to;
        var definedresult;
        var msg = "";
        $('.qc-test-actual-value.enabled').each(function () {
            actual_value = clean($(this).val());
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            range_from = clean($(this).closest('tr').find('.range-from').text());
            range_to = clean($(this).closest('tr').find('.range-to').text());
            if (actual_value == "") {
                QC.error_count++;
                msg += "Actual value required for " + test_name + "<br/>";
                app.add_error_class($(this));
            }
        });

        $('.qc-test-actual-result.enabled').each(function () {
            actual_value = $(this).val().trim();
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            if (actual_value == "") {
                QC.error_count++;
                msg += "Actual result required for " + test_name + "<br/>";
                app.add_error_class($(this));
            }
        });
        if (msg != "") {
            app.show_error(msg);
        }
    },

    validate_form: function () {
        if ($('#QCItem_ToWareHouseID').val() == '') {
            QC.error_count++;
            app.show_error("Please select Store");
            app.add_error_class($('#QCItem_ToWareHouseID'));
        }
        if (clean($('#AcceptedQty').val())== 0) {
            QC.error_count++;
            app.show_error("Production not Completed");
            app.add_error_class($('#ApprovedQty'));
        }
        if ($('#ApprovedQty').val().trim() == '') {
            QC.error_count++;
            app.show_error("Please enter approved quantity");
            app.add_error_class($('#ApprovedQty'));
        } else if (clean($('#ApprovedQty').val()) < 0) {
            QC.error_count++;
            app.show_error("Invalid  approved quantity");
            app.add_error_class($('#ApprovedQty'));
        } else if ((clean($('#ApprovedQty').val()) == 0) && (clean($('#AcceptedQty').val())!=0)) {
            QC.error_count++;
            app.show_error("Please enter approved quantity");
            app.add_error_class($('#ApprovedQty'));
        } else if (!$.isNumeric($('#ApprovedQty').val().trim().replace(/\,/g, ''))) {
            QC.error_count++;
            app.show_error("Approved quantity should be numeric");
            app.add_error_class($('#ApprovedQty'));
        } else if (parseFloat($('#ApprovedQty').val().replace(/\,/g, '')) > parseFloat($('#AcceptedQty').val().replace(/\,/g, ''))) {
            QC.error_count++;
            app.show_error("Approved Quantity should not be greater than accepted quantity");
            app.add_error_class($('#ApprovedQty'));
        }
        var today_date = new Date();
        var today = new Date(today_date.getFullYear(), today_date.getMonth(), today_date.getDate());
        var date = new Date();
        var relaxation = $('#QCDate').data('relaxation');
        var relaxation_date = new Date();
        relaxation_date.setDate(date.getDate() + relaxation);
        var relaxation_day = new Date(relaxation_date.getFullYear(), relaxation_date.getMonth(), relaxation_date.getDate());
        var u_date = $('#QCDate').val().split('-');
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
        if (used_date == "Invalid Date") {
            app.show_error("Invalid QC Date");
            app.add_error_class($('#QCDate'));
        } else if (used_date.getTime() < relaxation_day.getTime()) {
            QC.error_count++;
            app.show_error("Invalid QC Date");
            app.add_error_class($('#QCDate'));
        } else if (used_date.getTime() > today.getTime()) {
            QC.error_count++;
            app.show_error("Invalid QC Date");
            app.add_error_class($('#QCDate'));
        }
    },

    validate_remark: function () {
        if ($('#qc-remarks').val().trim() == '') {
            QC.error_count++;
            app.show_error("Please enter remarks");
            app.add_error_class($('#qc-remarks'));
        }
    }
};