Payroll = {
    init: function () {
        var self = Payroll;
        self.bind_events();
        self.get_employee_payroll();
    },

    bind_events: function () {
        var self = Payroll;
        $(".btn-next").on("click", self.tab_change);
        $(".location-head").on("ifChanged", self.show_location);
        $('#tabs').on('show.uk.switcher', self.tab_switcher);
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_item);
        $("#").on("ifChanged", self.show_location);
    },
    next_tab: 0,
    tab_switcher: function (event, area) {
        var self = Payroll;
        self.next_tab = $(area).data('id');
        if (self.next_tab    == 6) {
            var previousTab = $("#tabs").find("[data-id='" + (self.next_tab - 1) + "']");
            $(previousTab).addClass('uk-disabled ');
            $(previousTab).find("a").prepend("<i class='uk-icon-check uk-icon-small uk-text-success'></i> ");
            $(previousTab).find("a").addClass("uk-text-success");
            $("#tab-next").addClass("uk-hidden");
            $("#tab-save").removeClass("uk-hidden");
        } else {
            var previousTab = $("#tabs").find("[data-id='" + (self.next_tab - 1) + "']");
            $(previousTab).addClass('uk-disabled ');
            $(previousTab).find("a").prepend("<i class='uk-icon-check uk-icon-small uk-text-success'></i> ");
            $(previousTab).find("a").addClass("uk-text-success");
        }

        
    },
    tab_change: function () {
        var self = Payroll;
        $("#tabs").find("[data-id='" + (self.next_tab + 1) + "']").removeClass('uk-disabled');
        UIkit.switcher('#tabs').show(self.next_tab);
    },
    show_location: function () {
        if ($(this).is(":checked")) {
            $(this).closest('.location-head-wrapper').find('.locations').removeClass('uk-hidden');
        } else {
            $(this).closest('.location-head-wrapper').find('.locations').addClass('uk-hidden');
        }
    },
    get_items: function (release) {
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
    set_item: function (event, item) {
        var self = Payroll;
        console.log(item)
        $("#EmployeeID").val(item.id);
        $("#EmployeeName").val(item.name);
        $("#EmployeeCode").val(item.code);
        $("#Place").val(item.place);
    },
    get_employee_payroll: function () {

        var self = Payroll;
        $.ajax({
            url: '/Masters/Employee/GetEmployeeList',
            dataType: "json",
            type: "POST",
            success: function (employeelist) {
                $.each(employeelist, function (i, Employee) {
                    var $employeelist = '<tr><td class="uk-text-center">' + Employee.ID + '</td>'
                                        + ' <input type="hidden" class="EmployeeID" value="' + Employee.ID + '" /></td>'
                                        + '<td class="EmployeeCode">' + Employee.Code + '</td>'
                                        + '<td class="EmployeeName"><span>' + Employee.Name + '</span></td>'
                                        + '<td class="uk-text-right"><input type="text" class="md-input uk-text-right" value="0" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right" value="0" /></td>'
                                        + '<td class="uk-text-right">0</td>'
                                        + '</tr>';

                    app.format($employeelist);
                    $("#tbl-leave-attendance tbody").append($employeelist);
                });
                $.each(employeelist, function (i, Employee) {
                    var $employeelist = '<tr><td class="uk-text-center">' + Employee.ID + '</td>'
                                        + ' <input type="hidden" class="EmployeeID" value="' + Employee.ID + '" /></td>'
                                        + '<td class="EmployeeCode">' + Employee.Code + '</td>'
                                        + '<td class="EmployeeName"><span>' + Employee.Name + '</span></td>'
                                        + '<td class="uk-text-right">0.00</td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right mask-currency salarycomponents" value="0.00" /></td>'
                                        + '</tr>';
                    
                    app.format($employeelist); 
                    $("#tbl-pay-adjustment tbody").append($employeelist);
                });

                $.each(employeelist, function (i, Employee) {
                    var $employeelist = '<tr><td class="uk-text-center">' + Employee.ID + '</td>'
                                        + ' <input type="hidden" class="EmployeeID" value="' + Employee.ID + '" /></td>'
                                        + '<td class="EmployeeCode">' + Employee.Code + '</td>'
                                        + '<td class="EmployeeName"><span>' + Employee.Name + '</span></td>'
                                        + '<td class="uk-text-right"><input type="text" class="md-input uk-text-right" value=0.00 /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right" value="0.00" /></td>'
                                        + '<td class="uk-text-right"> <input type="text" class="md-input uk-text-right" value="0.00" /></td>'
                                        + '</tr>';

                    app.format($employeelist);
                    $("#tbl-earning-deduction tbody").append($employeelist);
                });

                $.each(employeelist, function (i, Employee) {
                    var $employeelist = '<tr>'
                                        + '<td class="uk-text-center"><input type="checkbox" class="icheckbox icheckbox_md" data-md-icheck  />'
                                        + '<td class="uk-text-center">' + Employee.ID + '</td>'                                        
                                        + '<input type="hidden" class="EmployeeID" value="' + Employee.ID + '" />'
                                        + '<td class="EmployeeCode">' + Employee.Code + '</td>'
                                        + '<td class="EmployeeName"><span>' + Employee.Name + '</span></td>'                                                     
                                        + '</tr>';

                    app.format($employeelist);
                    $("#tbl-run-payroll tbody").append($employeelist);
                });

                var $payrollcomplete = '<tr><td class="">Total Earnings</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '</tr>'                                 
                                       + '<tr><td class="">Total Deductions</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '</tr>'                                  
                                       + '<tr><td class="">Net Total</td>'       
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '<td class="uk-text-right mask-currency">0.00</td>'
                                       + '</tr>';
                app.format($payrollcomplete);
                $("#tbl-complete-payroll tbody").append($payrollcomplete);
            }
        });
    },
};
