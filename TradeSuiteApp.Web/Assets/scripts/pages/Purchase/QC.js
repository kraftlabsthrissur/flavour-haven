QC = {
    init: function () {
        QC.bind_events();
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
            case "new":
                $list = $('#pending-qc-items-list');
                break;
            case "started":
                $list = $('#ongoing-qc-items-list');
                break;
            case "passed":
                $list = $('#passed-qc-items-list');
                break;
            case "overruled":
                $list = $('#overruled-qc-items-list');
                break;
            case "failed":
                $list = $('#failed-qc-items-list');
                break;
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Purchase/QC/GetQualityCheckList?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "desc"]],
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
                                + "<input type='hidden' class='ID' value='" + row.ID + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "TransDate", "className": "TransDate" },
                    { "data": "GRNNo", "className": "GRNNo" },
                    { "data": "ReceiptDate", "className": "ReceiptDate" },
                    { "data": "ItemName", "className": "ItemName" },
                    { "data": "UnitName", "className": "UnitName" },
                    {
                        "data": "ApprovedQty",
                        "className": "ApprovedQty",
                        "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.ApprovedQty + "</div>";
                        }
                    },
                    { "data": "SupplierName", "className": "SupplierName" },
                    { "data": "DeliveryChallanNo", "className": "DeliveryChallanNo" },
                    { "data": "DeliveryChallanDate", "className": "DeliveryChallanDate" },
                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var row = $(this).closest("tr");
                        var Id = $(row).find("td .ID").val();
                        if ($(row).hasClass("passed") || $(row).hasClass("overruled") || $(row).hasClass("failed")) {
                            app.load_content("/Purchase/QC/Details/" + Id);
                        } else {
                            app.load_content("/Purchase/QC/Edit/" + Id);
                        }

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
        var self = QC;
        $('.approve-qc').on('click', this.approve);
        $('.reject-qc').on('click', this.reject);
        $('.save-qc,.save-draft-qc').on('click', this.save);
        $('.overrule-qc').on('click', this.overrule);
        $('.edit-qc').on('click', this.edit);
        $('.passed').on('ifChanged', QC.validate_failed);
        $('.failed').on('ifChanged', QC.validate_passed);
        $('.select-all').on('ifChanged', QC.select_results);

    },
    select_results: function (e) {
        console.log(e);

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
        QC.error_count = 0;
        QC.validate_form();
      
        if (clean($('#ApprovedQty').val()) <= 0) {
            app.show_error("Invalid approved quantity");
            return;
        }
        if (QC.error_count == 0) {
            QC.validate_tests();
            if (QC.error_count == 0) {
                QC.validate_results();
                if (QC.error_count == 0) {
                    var data = QC.get_data(false);
                    $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc, .overrule-qc ").css({ 'display': 'none' });
                    $.ajax({
                        url: '/Purchase/QC/approve/' + data.QCItem.ID,
                        data: data,
                        dataType: "json",
                        type: "POST",
                        success: function (response) {
                            if (response.Status == "success") {
                                app.show_notice("Approved successfully");
                                setTimeout(function () {
                                    window.location = "/Purchase/QC/Index";
                                }, 1000);
                            } else {
                                if (typeof response.data[0].ErrorMessage != "undefined")
                                    app.show_error(response.data[0].ErrorMessage);
                                $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc, .overrule-qc ").css({ 'display': 'block' });
                            }
                        }
                    });
                }
            }
        }
    },
    reject: function () {
        QC.error_count = 0;
        QC.validate_form();
        QC.validate_remark();
        QC.validate_tests();
        if (clean($('#ApprovedQty').val()) != 0) {
            app.show_error("Approved quantity must be zero for reject");
            return;
        }
        if (QC.error_count == 0) {
            var data = QC.get_data(false);
            $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc ").css({ 'display': 'none' });
            $.ajax({
                url: '/Purchase/QC/reject/' + data.QCItem.ID,
                data: data,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("QC rejected successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/QC/Index";
                        }, 1000);
                    } else {
                        if (typeof response.data[0].ErrorMessage != "undefined")
                            app.show_error(response.data[0].ErrorMessage);
                        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc ").css({ 'display': 'block' });

                    }
                }
            });
        }
    },
    overrule: function () {
        QC.error_count = 0;
        QC.validate_form();
        QC.validate_remark();
        QC.validate_tests();
        if (clean($('#ApprovedQty').val()) <= 0) {
            app.show_error("Invalid approved quantity");
            return;
        }
        if (QC.error_count == 0) {
            var data = QC.get_data(false);
            $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc, .overrule-qc ").css({ 'display': 'none' });
            $.ajax({
                url: '/Purchase/QC/overrule/' + data.QCItem.ID,
                data: data,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        app.show_notice("QC overruled successfully");
                        setTimeout(function () {
                            window.location = "/Purchase/QC/Index";
                        }, 1000);
                    } else {
                        if (typeof response.data[0].ErrorMessage != "undefined")
                            app.show_error(response.data[0].ErrorMessage);
                        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc ").css({ 'display': 'block' });
                    }
                }
            });
        }
    },

    save: function () {
        var self = QC;
        var url;
        if ($(this).hasClass("save-draft-qc")) {
            data = self.get_data(true);
            url = '/Purchase/QC/SaveAsDraft/'
        }
        else {
            data = self.get_data(false);
            url = '/Purchase/QC/save/';
        }
        $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc ").css({ 'display': 'none' });
        $.ajax({
            url: url + data.QCItem.ID,
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("QC created successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/QC/Index";
                    }, 1000);
                } else {
                    if (typeof response.data[0].ErrorMessage != "undefined")
                        app.show_error(response.data[0].ErrorMessage);
                    $(".approve-qc, .reject-qc,.save-qc,.save-draft-qc,.overrule-qc ").css({ 'display': 'block' });

                }
            }
        });
    },
    get_data: function (IsDraft) {
        var actual_result;
        var data = {
            QCItem: {
                "ID": $('#QCItem_ID').val(),
                "ItemID": $('#QCItem_ItemID').val(),
                "Remarks": $('#qc-remarks').val(),
                "ToWareHouseID": $('#QCItem_ToWareHouseID').val(),
                "QCDate": $('#QCDate').val(),
                "ApprovedQty": $("#ApprovedQty").val().replace(/\,/g, ''),
                IsDraft: IsDraft
            },
            testResults: [],
        }

        var test_details;
        $('#qc-chemical-analisys tbody tr').each(function () {
            test_details = {
                ID: $(this).find('.ID').val(),
                ActualValue:clean( $(this).find('.qc-test-actual-value').val()),
                ActualResult: $(this).find('.qc-test-actual-result').val(),
                Remarks: $(this).find('.remarks').val()
            }
            data.testResults.push(test_details);
        });
        $('#qc-physical-analisys tbody tr').each(function () {
            if ($(this).closest('tr').find('.passed').prop("checked") == true) {
                actual_result = 'Passed';
            }
            else if ($(this).closest('tr').find('.failed').prop("checked") == true) {
                actual_result = 'Failed';
            }
            else {
                actual_result = "";
            }
            test_details = {
                ID: $(this).find('.ID').val(),
                ActualResult: actual_result,
                ActualValue: $(this).find('.normal-value').text(),
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
        var passed;
        var failed;
        var is_mandatory;
        var failed_count;
        $('#qc-chemical-analisys .qc-test-actual-value').each(function () {
            actual_value = parseFloat($(this).val().trim().replace(/\,/g, ''));
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            range_from = parseFloat($(this).closest('tr').find('.range-from').text().trim().replace(/\,/g, ''));
            range_to = parseFloat($(this).closest('tr').find('.range-to').text().trim().replace(/\,/g, ''));
            is_mandatory = $(this).closest('tr').find('.IsMandatory').val();
            if (is_mandatory == "True") {
                if (!(actual_value <= range_to && actual_value >= range_from)) {
                    QC.error_count++;
                    app.show_error(test_name + " should fall between " + range_from + " and " + range_to);
                    app.add_error_class($(this));
                }
            }

            else {
                if (actual_value > 0) {
                    if (!(actual_value <= range_to && actual_value >= range_from)) {
                        QC.error_count++;
                        app.show_error(test_name + " should fall between " + range_from + " and " + range_to);
                        app.add_error_class($(this));
                    }
                }
            }
        });

        $('#qc-physical-analisys tbody tr').each(function () {
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

        failed_count = $('#qc-physical-analisys tbody tr .failed:checkbox:checked').length;
        if (failed_count > 0) {
            QC.error_count++;
            app.show_error("All tests are not passed");
            app.add_error_class($(this));

        }
    },
    validate_tests: function () {
        var actual_value;
        var test_name;
        $('#qc-chemical-analisys .qc-test-actual-value').each(function () {
            actual_value = $(this).val().trim().replace(/\,/g, '');
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            if (actual_value == "") {
                QC.error_count++;
                app.show_error("Actual value required for " + test_name);
                app.add_error_class($(this));
            } else if (!$.isNumeric(actual_value)) {
                QC.error_count++;
                app.show_error("Actual value for " + test_name + " should be numeric");
                app.add_error_class($(this));
            }
        });
        $('#qc-physical-analisys .qc-test-actual-result').each(function () {
            actual_value = $(this).val().trim();
            test_name = $(this).closest('tr').find('.test-name').text().trim();
            if (actual_value == "") {
                QC.error_count++;
                app.show_error("Actual value required for " + test_name);
                app.add_error_class($(this));
            }
        });
    },
    validate_form: function () {
        if ($('#QCItem_ToWareHouseID').val() == '') {
            QC.error_count++;
            app.show_error("Please select Store");
            app.add_error_class($('#QCItem_ToWareHouseID'));
        }
        //if ($('#ApprovedQty').val().trim() == '') {
        //    QC.error_count++;
        //    app.show_error("Please enter approved quantity");
        //    app.add_error_class($('#ApprovedQty'));
        //} else if (clean($('#ApprovedQty').val()) == 0) {
        //    QC.error_count++;
        //    app.show_error("Please enter approved quantity");
        //    app.add_error_class($('#ApprovedQty'));
        //} else if (clean($('#ApprovedQty').val()) < 0) {
        //    QC.error_count++;
        //    app.show_error("Invalid approved quantity");
        //    app.add_error_class($('#ApprovedQty'));
        //} else

            if (!$.isNumeric(clean($('#ApprovedQty').val()))) {
            QC.error_count++;
            app.show_error("Enter valid approved quantity");
            app.add_error_class($('#ApprovedQty'));
        } else if (clean($('#ApprovedQty').val()) > clean($('#AcceptedQty').val())) {
            QC.error_count++;
            app.show_error("Approved quantity exceeds accepted quantity");
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
            app.show_error("Invalid QC date");
            app.add_error_class($('#QCDate'));
        } else if (used_date.getTime() < relaxation_day.getTime()) {
            QC.error_count++;
            app.show_error("Invalid QC date");
            app.add_error_class($('#QCDate'));
        } else if (used_date.getTime() > today.getTime()) {
            QC.error_count++;
            app.show_error("Invalid QC date");
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