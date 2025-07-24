Attendance = {
    init: function () {
        var self = Attendance;
        self.bind_events();
        
    },
    details: function () {
        var self = Attendance;
        var $calendar_selectable = $('#calendar_selectable'),
                    calendarColorsWrapper = $('<div id="calendar_colors_wrapper"></div>');
        var calendarColorPicker = altair_helpers.color_picker(calendarColorsWrapper).prop('outerHTML');
        $("#calendar_selectable").fullCalendar({
            header: {
                left: 'title today',
                center: '',
                right: 'month, prev,next'
            },
            buttonIcons: {
                prev: 'md-left-single-arrow',
                next: 'md-right-single-arrow',
                prevYear: 'md-left-double-arrow',
                nextYear: 'md-right-double-arrow'
            },
            aspectRatio: 2.1,
            defaultDate: moment(),
            selectable: true,
            selectHelper: true,
            select: function (start, end) {
                UIkit.modal.prompt('' +
                    '<h3 class="heading_b uk-margin-medium-bottom">New Event</h3><div class="uk-margin-medium-bottom" id="calendar_colors">' +
                    'Event Color:' +
                    calendarColorPicker +
                    '</div>' +
                    'Event Title:',
                    '', function (newvalue) {
                        if ($.trim(newvalue) !== '') {
                            var eventData,
                                eventColor = $('#calendar_colors_wrapper').find('input').val();
                            eventData = {
                                title: newvalue,
                                start: start,
                                end: end,
                                color: eventColor ? eventColor : ''
                            };
                            $calendar_selectable.fullCalendar('renderEvent', eventData, true); // stick? = true
                            $calendar_selectable.fullCalendar('unselect');
                        }
                    }, {
                        labels: {
                            Ok: 'Add Event'
                        }
                    });
            },
            editable: true,
            eventLimit: true,
            timeFormat: '(HH)(:mm)',
            events: [
                //{
                //    title: 'All Day Event',
                //    start: moment().startOf('month').add(25, 'days').format('YYYY-MM-DD')
                //},
                //{
                //    title: 'Long Event',
                //    start: moment().startOf('month').add(3, 'days').format('YYYY-MM-DD'),
                //    end: moment().startOf('month').add(7, 'days').format('YYYY-MM-DD')
                //},
                //{
                //    id: 999,
                //    title: 'Repeating Event',
                //    start: moment().startOf('month').add(8, 'days').format('YYYY-MM-DD'),
                //    color: '#689f38'
                //},
                //{
                //    id: 999,
                //    title: 'Repeating Event',
                //    start: moment().startOf('month').add(15, 'days').format('YYYY-MM-DD'),
                //    color: '#689f38'
                //},
                //{
                //    title: 'Conference',
                //    start: moment().startOf('day').add(14, 'hours').format('YYYY-MM-DD HH:mm'),
                //    end: moment().startOf('day').add(15, 'hours').format('YYYY-MM-DD HH:mm')
                //},
                //{
                //    title: 'Meeting',
                //    start: moment().startOf('month').add(14, 'days').add(10, 'hours').format('YYYY-MM-DD HH:mm'),
                //    color: '#7b1fa2'
                //},
                //{
                //    title: 'Lunch',
                //    start: moment().startOf('day').add(11, 'hours').format('YYYY-MM-DD HH:mm'),
                //    color: '#d84315'
                //},
                //{
                //    title: 'Meeting',
                //    start: moment().startOf('day').add(8, 'hours').format('YYYY-MM-DD HH:mm'),
                //    color: '#7b1fa2'
                //},
                //{
                //    title: 'Happy Hour',
                //    start: moment().startOf('month').add(1, 'days').format('YYYY-MM-DD')
                //},
                //{
                //    title: 'Dinner',
                //    start: moment().startOf('day').add(19, 'hours').format('YYYY-MM-DD HH:mm')
                //},
                //{
                //    title: 'Birthday Party',
                //    start: moment().startOf('month').add(23, 'days').format('YYYY-MM-DD')
                //},
                //{
                //    title: 'NEW RELEASE (link)',
                //    url: 'http://themeforest.net/user/tzd/portfolio',
                //    start: moment().startOf('month').add(10, 'days').format('YYYY-MM-DD'),
                //    color: '#0097a7'
                //}
                {
                    title: 'Casual Leave',
                    start: moment().startOf('month').add(3, 'days').format('YYYY-MM-DD'),
                    end: moment().startOf('month').add(7, 'days').format('YYYY-MM-DD')
                },
                {
                    title: 'Off',
                    start: moment().startOf('day').add(14, 'hours').format('YYYY-MM-DD HH:mm'),
                   
                    color: '#d84315'
                },
                {
                    title: 'Half Day Leave',
                    start: moment().startOf('month').add(14, 'days').add(10, 'hours').format('YYYY-MM-DD HH:mm'),
                    color: '#7b1fa2'
                },
                {
                    title: 'NEW RELEASE (link)',
                    url: 'http://themeforest.net/user/tzd/portfolio',
                    start: moment().startOf('month').add(10, 'days').format('YYYY-MM-DD'),
                    color: '#0097a7'
                }
            ],
        });
    },
    list: function () {
        if ($("#page_content_inner").hasClass("form-view")) {
            $("#attendance-employee-list").find('span').each(function () {
                $(this).width($(this).closest('td').width() - 10);
                $(this).addClass('label');

            });
        }
    },
    bind_events: function () {
        var self = Attendance
        $('.btnshowItem').on('click', self.show_daily_attendance);
        $('#daily-attendance-employee-list tbody').on('click',self.show_employee_list)
    },
    show_daily_attendance: function () {
        var self = Attendance;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
       
        var PayrollCategory = $("#PayrollCategory").val();
        var Department = $("#Department").val();
        var EmployeeCategory = $("#EmployeeCategory").val();

        $("#daily-attendance-employee-list tbody").empty();
        $("#attendance-day-list tbody").empty();
        self.GetEmployeeList(PayrollCategory, Department, EmployeeCategory);

    },
    GetEmployeeList: function () {
        var self = Attendance;
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
                                        + '<td class="uk-text-right">20 </td>'
                                        + '<td class="uk-text-right"> 10 </td>'
                                        + '</tr>';

                    app.format($employeelist);
                    $("#daily-attendance-employee-list tbody").append($employeelist);
                    self.populate_status()
                });
            }
        });

    },
    populate_status: function () {
        var html = "";
        var statushtml = "";
        var d = new Date();
        var day = d.getDate();

        var Status = $('#StatusTemplate').html();


        for (var i = 0; i < 62; i++) {
            statushtml += '<td><select  class="md-input "style="width:75px">' + Status + '</select></td>';
        }
        html = '<tr>' + statushtml + '</tr>';
        var $htmlStatus = $(html);
        app.format($htmlStatus);
        $("#attendance-day-list tbody").append($htmlStatus);

        $("th:contains(" + day + ")").css("background-color", "#c1c1c1")

        //var table = document.getElementById("attendance-day-list");
        //for (var i = 1; i < table.rows.length; i++) {

        //    table.rows[i].cells[day + 8].style.background = "#c1c1c1";
        //    table.rows[i].cells[day + 9].style.background = "#c1c1c1";
        //}
    },
    validate_form: function () {
        var self = Attendance;
        if (self.rules.on_show_item.length > 0) {
            return form.validate(self.rules.on_show_item);
        }
        return 0;

    },
    rules: {
        on_show_item: [
                        
                           {
                               elements: "#DepartmentID",
                               rules: [
                                        { type: form.required, message: "Department is required" },
                               ]
                           },
                           {
                               elements: "#EmployeeCategoryID",
                               rules: [
                                        { type: form.required, message: "Employee Category is required" },
                               ]
                           },
                           {
                               elements: "#MonthID",
                               rules: [
                                        { type: form.required, message: "Month is required" },
                               ]
                           },
        ],
    },

    show_employee_list: function () {

        var id = $(this).find('.EmployeeID').val();
            window.location = '/Payroll/Attendance/Details/' + id;
        
    },
};