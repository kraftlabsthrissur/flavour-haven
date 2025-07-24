ConsultationSchedule = {
    init: function () {
        var self = ConsultationSchedule;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        var length = $("#consultation-schedule-list tbody tr").length;
        $("#item-count").val(length);
        self.bind_events();
    },
    bind_events: function () {
        var self = ConsultationSchedule;
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_doctor, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_doctor);
        $("body").on("click", "#btnAddItems", self.add_Item);
        $("body").on("click", "#btnOKEmployee", self.select_employee);
        $("body").on("click", ".remove-item", self.delete_item);
        $(".btnSave").on("click", self.save_confirm);
    },
    list: function () {
        var $list = $('#schedule-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/ConsultationSchedule/GetConsultationScheduleList"

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
                   { "data": "DoctorName", "className": "DoctorName" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Masters/ConsultationSchedule/Details/" + Id);
                    });
                },
            });

            $list.find('thead.search input').on('textchange', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },
    select_employee: function () {
        var self = ConsultationSchedule;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#DoctorName").val(Name);
        $("#DoctorID").val(ID);
        UIkit.modal($('#select-employee')).hide();

    },
    get_doctor: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#DoctorName').val(),
                EmployeeCategoryID: $('#EmployeeCategoryID').val(),
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
    set_doctor: function (event, item) {
        var self = ConsultationSchedule;
        $("#DoctorID").val(item.id),
        $("#DoctorName").val(item.name);
        UIkit.modal($('#select-employee')).hide();
    },
    add_Item: function () {
        var self = ConsultationSchedule;
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        sino = $('#consultation-schedule-list tbody tr').length + 1;
        var DoctorName = $("#DoctorName").val();
        var DoctorID = $("#DoctorID").val();
        var StartTime = $("#StartTime").val();
        var EndTime = $("#EndTime").val();
        var WeekDay = $("#WeekDay").val();
        var content = "";
        var $content;
        content = '<tr>'
            + '<td class="sl-no uk-text-center">' + sino + '</td>'
            + '</td>'
            + '<td>' + '<input type="text" value="'+WeekDay+'" class="md-input WeekDay " disabled /> ' + '</td>'
            + '<td>' + '<input type="text" value="'+StartTime+'" class="md-input StartTime label-fixed time15" disabled  /> ' + '</td>'
            + '<td>' + '<input type="text" value="'+EndTime+'" class="md-input EndTime label-fixed time15" disabled /> ' + '</td>'
            + '<td>'
            + '<a class="remove-item">'
            + '<i class="uk-icon-remove added"></i>'
            + '</a>'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#consultation-schedule-list tbody').append($content);
        self.clear_data();
        self.count();
    },
    clear_data: function () {
        var self = ConsultationSchedule;
        $("#Time").val('');
        $("#StartTime").val('');
        $("#EndTime").val('');
    },
    count: function () {
        index = $("#consultation-schedule-list tbody").length;
        $("#item-count").val(index);
    },
    delete_item: function () {
        var self = ConsultationSchedule;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#consultation-schedule-list tbody tr .sl-no').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#consultation-schedule-list tbody tr").length);

    },
    save_confirm: function () {
        var self = ConsultationSchedule;
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
    save: function () {
        var self = ConsultationSchedule;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/ConsultationSchedule/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/ConsultationSchedule/Index";
                }
                else {
                    if (typeof response.Message != "undefined")
                        app.show_error(response.Message);
                }
            }
        });
    },
    get_data: function () {
        var self = ConsultationSchedule;
        var data = {};
        data.ID = $("#ID").val()
        data.DoctorID = $("#DoctorID").val(),
        data.TimeSlot = clean($("#TimeSlot").val());
        data.ConsultationFee =  clean($("#ConsultationFee").val());
        data.ConsultationFeeValidity = clean($("#ConsultationFeeValidity").val());
        data.Items = [];
        var item = {};
        $('#consultation-schedule-list tbody tr').each(function () {
            item = {};
            item.WeekDay = $(this).find(".WeekDay").val();
            item.StartTime = $(this).find(".StartTime").val();
            item.EndTime = $(this).find(".EndTime").val();
            data.Items.push(item);
        });
        return data;
    },
    validate_add: function () {
        var self = ConsultationSchedule;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_save: function () {
        var self = ConsultationSchedule;
        if (self.rules.on_save.length > 0) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    rules: {
        on_add: [
             {
                 elements: "#WeekDay",
                 rules: [
                     { type: form.required, message: "Please Select Schedule Day" },
                     {
                         type: function (element) {
                             var error = false;
                             var start_date = $("#StartTime").val();
                             var ends_date = $("#EndTime").val();
                             var st = new Date("January 1, 2020 " + start_date);
                             st = st.getTime();
                             var et = new Date("January 1, 2020 " + ends_date);
                             et = et.getTime();

                             $("#consultation-schedule-list tbody tr").each(function () {
                                 var Sdate = $(this).find(".StartTime").val();
                                 var Edate = $(this).find(".EndTime").val();
                                 var stt = new Date("January 1, 2020 " + Sdate);
                                 stt = stt.getTime();
                                 var endt = new Date("January 1, 2020 " + Edate);
                                 endt = endt.getTime();
                                 if ($(this).find(".WeekDay").val().trim() == $(element).val() && (start_date == Sdate || ends_date == Edate)) {
                                     error = true;
                                 }
                             });
                             return !error;
                         }, message: "already Scheduled"
                     },
                 ]
             },
              {
                  elements: "#StartTime",
                  rules: [
                      { type: form.required, message: "Please enter Start Time" },
                      { type: form.non_zero, message: "Please enter Start Time" },
                  ],
              },
              {
                  elements: "#DoctorID",
                  rules: [
                      { type: form.required, message: "Please Select Doctor" },
                      { type: form.non_zero, message: "Please Select Doctor" },
                  ],
              },
              {
                  elements: "#EndTime",
                  rules: [
                      { type: form.required, message: "Please enter End Time" },
                      { type: form.non_zero, message: "Please enter End Time" },
                  ],
              },

        ],
        on_save: [
            {
                elements: "#DoctorID",
                rules: [
                    { type: form.required, message: "Please Select Doctor" },
                    { type: form.non_zero, message: "Please Select Doctor" },
                ],
            },
             {
                 elements: "#TimeSlot",
                 rules: [
                     { type: form.required, message: "Please enter TimeSlot" },
                     { type: form.non_zero, message: "Please enter TimeSlot" },
                 ],
             },
             {
                 elements: "#item-count",
                 rules: [
                     { type: form.required, message: "Please add atleast one item" },
                     { type: form.non_zero, message: "Please add atleast one item" },
                 ]
             },
        ],
    }
}