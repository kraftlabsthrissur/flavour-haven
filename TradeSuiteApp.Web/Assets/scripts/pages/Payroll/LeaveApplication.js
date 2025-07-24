$(function () {
    LeaveApplication.init();
});
LeaveApplication = {

    init: function () {
        var self = LeaveApplication;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.leave_application_list();
        } else {
            fh_items = $("#leave-application-list").FreezeHeader();
        }
        self.bind_events();
    },
    leave_application_list: function () {
        $leave_application_list = $('#leave-application-list');
        $('#leave-application-list tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Payroll/LeaveApplication/Details/' + id;
        });
        if ($leave_application_list.length) {
            $leave_application_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var leaveapplication_list_table = $leave_application_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            leaveapplication_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    leaveapplication_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        var self = LeaveApplication;
        $("#btnAddProduct").on('click', LeaveApplication.add_item);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': LeaveApplication.get_employees, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', LeaveApplication.set_employees);
    },

    add_item: function () {
        var self = LeaveApplication;
        //self.error_count = 0;
        //self.error_count = self.validate_item();
        //if (self.error_count > 0) {
        //    return;
        //}
        var item = {};
        item.LeaveType = $("#LeaveTypeID").val() == "" ? "" : $("#LeaveTypeID  option:selected").text();
        item.LeaveAvailable = $("#LeaveAvilable").val();
        item.StartDate = $("#StartDate").val();
        //item.Startam = $("#Startforenoon").val();
        item.Startpm = $("#Startafternoon").val();
        item.EndDate = $("#EndDate").val();
        item.Endam = $("#Endforenoon").val();
        //item.Endpm = $("#Endafternoon").val();
        item.NoOfDays = $("#NoOfDays").val();
        item.BalanceAvailable = $("#BalanceAvailable").val();
        item.Reason = $("#Reason").val();
        item.Reason = $("#Reason").val();
        self.add_item_to_grid(item);
        //self.clear_Item();
    },

    add_item_to_grid: function (item) {
        var self = LeaveApplication;
        var index, tr;
        index = $("#leave-application-list tbody tr").length + 1;
        tr = '<tr>'
        + ' <td class="uk-text-center">' + index + ' </td>'
        + ' <td class="LeaveType">' + item.LeaveType + '</td>'
        + ' <input type="hidden" class="LeaveTypeID" value="' + $('#LeaveTypeID').val() + '" /></td>'
        + ' <td class="LeaveAvailable">' + item.LeaveAvailable + '</td>'
        + ' <td class="StartDate">' + item.StartDate + '</td>'
        //+ ' <td class="Startam">' + item.Startam + '</td>'
        + ' <td class="Startpm">' + item.Startpm + '</td>'
        + ' <td class="EndDate">' + item.EndDate + '</td>'
        + ' <td class="Endam">' + item.Endam + '</td>'
        //+ ' <td class="Endpm">' + item.Endpm + '</td>'
        + ' <td class="NoOfDays">' + item.NoOfDays + '</td>'
        + ' <td class="BalanceAvailable">' + item.BalanceAvailable + '</td>'
        + ' <td class="Reason">' + item.Reason + '</td>'
        + ' <td class="uk-text-center">'
        + '     <a class="remove-item">'
        + '         <i class="uk-icon-remove"></i>'
        + '     </a>'
        + ' </td>'
        + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#leave-application-list tbody").append($tr);
        fh_items.resizeHeader();
    },

    //validate_item: function () {
    //    var self = LeaveApplication;
    //    if (self.rules.on_add.length > 0) {
    //        return form.validate(self.rules.on_add);
    //    }
    //    return 0;
    //},

    //rules: {
    //    on_add: [
    //        {
    //            elements: "#EmployeeName",
    //            rules: [
    //                { type: form.required, message: "Please select Employee" },
    //            ]
    //        },
    //    ]
    //},
    get_employees: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_employees: function (event, item) {
        self = LeaveApplication;
        console.log(item)
        var employeeId = item.id;
        $("#EmployeeID").val(item.id),
        $("#Name").val(item.value);
        $("#EmployeeCode").val(item.code);
        $("#Place").val(item.place);
        $("#hdnName").val(item.value);
        $("#hdnNameID").val(item.id);
    },
};