
ApprovalQueue = {
    init: function () {

        ApprovalQueue.bind_events();
    },



    bind_events: function () {
        var self = ApprovalQueue;
        $("body").on('click', "#approval-queue-list tbody td", self.show_detail_page);
        $("body").on('click', "#btnAddItem", self.add_employee);
        $("body").on('click', ".btnSave,.btnSaveNew", self.save);
        $("body").on('click', "md-btn-icon-small uk-icon-remove", self.remove_item);
        $("body").on('click', "md-btn Edit", self.show_detail_page);
        $('body').on('keyup', '.SortOrder', self.change_sort_order);
    },

    details: function () {

    },
    change_sort_order: function () {
        var self = ApprovalQueue;
        var error_count = self.validate_sort_order();
        if (error_count > 0) {
            return;
        }

        var row = $(this).closest('tr');
        var sortorder = clean($(row).find('.SortOrder').val());
        var oldsortorder = clean($(row).find('.OldSortOrder').val());
        var index = clean($(row).find('.index').text());
        var count = 0;
        $('.SortOrder').each(function () {
            var closestrow = $(this).closest('tr');
            var sortorderold = clean($(closestrow).find('.OldSortOrder').val());
            var closestsortorder = clean($(closestrow).find('.SortOrder').val());
            var closestindex = clean($(closestrow).find('.index').text());
            if (sortorderold == sortorder && closestsortorder == sortorder && index != closestindex)
                count++;
        });
        if (count == 0)
            $(row).find('.OldSortOrder').val(sortorder);
        else {
          
            setTimeout(function () {
                $(row).find('.SortOrder').val(oldsortorder);
            }, 200);
            app.show_error("Sort already exist, as a result system revert sotrorder");
        }
        if(count!=0)
        {
           
        }
    },
    add_employee: function () {
        var self = ApprovalQueue;
        var error_count = self.validate_add_employee();
        if (error_count > 0) {
            return;
        }
        var EmployeeID = $("#EmployeeID").val();
        var EmployeeName = $("#EmployeeID option:selected").text();
        var LocationID = $('#LocationID').val();
        var LocationName = $("#LocationID option:selected").text();
        console.log(EmployeeID, EmployeeName);
        var SortOrder = $("#SortOrder").val();
        var serialNo = $("#approval-queue-trans-list tbody tr").length + 1;
        var tr = "<tr>"
            + "<td class='index'>" + serialNo + "<input type ='hidden' class='OldSortOrder' value='" + SortOrder + "'></td>"
            + "<td>" + EmployeeName + "<input type ='hidden' class='EmployeeID ' value='" + EmployeeID + "'>" + "</td>"
            + "<td>" + LocationName
            + "<input type ='hidden' class='LocationID' value='" + LocationID + "'>"
            + "</td>"          
            + '<td><input type="text"  class="md-input uk-text-right SortOrder  " value="' + SortOrder + '" /></td>' 
            + ' <td class="uk-text-center" onclick="$(this).parent().remove()">'
            + '<a data-uk-tooltip="{pos:"bottom"}" >'
            + '<i class="md-btn-icon-small uk-icon-remove"></i>'
            + '</a>'
            + "</td>"
            + "</tr>";
        var $tr = $(tr);
        app.format($tr);
        $("#approval-queue-trans-list tbody").append($tr);
        
        self.count_queue();
        self.clear();
    },

    count_queue: function () {
        var count = $("#approval-queue-trans-list tbody tr").length;
        $('#queue-count').val(count);
    },

    clear: function () {
        $("#EmployeeID").val('');
        $("#SortOrder").val('');
    },

    validate_add_employee: function () {
        var self = ApprovalQueue;
        if (self.rules.on_add_employee.length) {
            return form.validate(self.rules.on_add_employee);
        }
        return 0;
    },

    validate_sort_order: function () {
        var self = ApprovalQueue;
        if (self.rules.on_sort_order.length) {
            return form.validate(self.rules.on_sort_order);
        }
        return 0;
    },

    validate_submit: function () {
        var self = ApprovalQueue;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    list: function () {
        ApprovalQueue.bind_events();

        var $list = $('#approval-queue-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs();

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 10
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    show_detail_page: function () {
        var row = $(this).closest("tr");
        var ID = $(row).find(".ID").val();
        window.location = "/Masters/ApprovalQueue/Details/" + ID;
    },

    save: function () {
        var self = ApprovalQueue;
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        var data = self.get_data();
        var location = "/Masters/ApprovalQueue/Index";
        if ($(this).hasClass('btnSaveNew')) {
            location = "/Masters/ApprovalQueue/Create";
        }
        $(".btnSave,.btnSaveNew").css({ 'display': 'none' });
        $.ajax({
            url: '/Masters/ApprovalQueue/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved successfully");
                    window.location = location;
                } else {
                    app.show_error(response.Message);
                    $(".btnSave,.btnSaveNew").css({ 'display': 'block' });
                }
            }
        });
    },

    get_data: function () {
        var self = ApprovalQueue;
        var data = {};
        data.QueueName = $("#QueueName").val();
        data.LocationID = $("#LocationID").val();
        data.ID = $("#ID").val();
        data.QueueTrans = [];
        var item = {};
        $('#approval-queue-trans-list tbody tr ').each(function () {
            item = {};
            item.UserID = $(this).find(".EmployeeID").val();
            item.SortOrder = $(this).find(".SortOrder").val();
            data.QueueTrans.push(item);
        });
        return data;
    },

    remove_item: function () {
        var self = ApprovalQueue;
        $(this).closest("tr").remove();
        $("#approval-queue-trans-list tbody tr").each(function (i, record) {
            $(this).children('td').eq(0).html(i + 1);
        });
        $("#queue-count").val($("#approval-queue-trans-list tbody tr").length);

    },

    rules: {
        on_add_employee: [
             {
                 elements: "#QueueName",
                 rules: [
                     { type: form.required, message: "Queue Name is Required" },
                 ],
             },
             {
                 elements: "#LocationID",
                 rules: [
                     { type: form.required, message: "Please Select Location" },
                     { type: form.non_zero, message: "Please Select Location" },
                 ],
             },
             {
                 elements: "#EmployeeID",
                 rules: [
                     { type: form.required, message: "Please Select User" },
                     { type: form.non_zero, message: "Please Select User" },
                 ],
             },
             {
                 elements: ".EmployeeID",
                 rules: [
                     {
                         type: function (element) {
                             var row = $(element).closest('tr');
                             var gridemployeeid = clean($(row).find('.EmployeeID').val());
                             var employeeid = clean($("#EmployeeID").val());
                             return gridemployeeid != employeeid
                         }, message: "User Already Exist"
                     }
                 ],
             },
             {
                 elements: ".LocationID",
                 rules: [
                     {
                         type: function (element) {
                             var row = $(element).closest('tr');
                             var gridlocationid = clean($(row).find('.LocationID').val());
                             var locationid = clean($("#LocationID").val());
                             return gridlocationid == locationid
                         }, message: "Location Must Be Same"
                     }
                 ],
             },

             {
                 elements: "#SortOrder",
                 rules: [
                         { type: form.possitive, message: "Sort order should be possitive value" },
                         { type: form.non_zero, message: "Sort order should be zero" },
                         {
                             type: function (element) {
                                 var queuecount = clean($("#queue-count").val()) + 1;
                                 var sortorder = clean($("#SortOrder").val());
                                 return sortorder <= queuecount
                             }, message: "Sort order should not be greater than queue"
                         },
                         {
                             type: function (element) {
                                 var error = false;
                                 $('.SortOrder').each(function () {
                                     var row = $(this).closest('tr');
                                     var sortordergrid = clean($(row).find('.SortOrder').val());
                                     var sortorder = $(element).val();
                                     if (sortordergrid == sortorder)
                                         error = true;
                                 });
                                 return !error;
                             }, message: 'Sort order already exist'
                         },
                 ]
             },
        ],

        on_sort_order: [
             {
                 elements: ".SortOrder",
                 rules: [
                         { type: form.possitive, message: "Sort order should be possitive value" },
                         { type: form.non_zero, message: "Sort order should be zero" },
                         {
                             type: function (element) {
                                 var queuecount = clean($("#queue-count").val());
                                 var row = $(element).closest('tr');
                                 var sortorder = clean($(row).find('.SortOrder').val());
                                 return sortorder <= queuecount
                             }, message: "Sort order should not be greater than queue"
                         },

                 ]
             },
        ],

        on_submit: [
            {
                elements: "#QueueName",
                rules: [
                 { type: form.required, message: "Queue Name is Required" },
                ],
            },

            {
                elements: ".LocationID",
                rules: [
                    {
                        type: function (element) {
                            var row = $(element).closest('tr');
                            var gridlocationid = clean($(row).find('.LocationID').val());
                            var locationid = clean($("#LocationID").val());
                            return gridlocationid == locationid
                        }, message: "Location Must Be Same"
                    }
                ],
            },

            {
                elements: "#queue-count",
                rules: [
                    { type: form.non_zero, message: "Please add atleast one queue" },
                    { type: form.required, message: "Please add atleast one queue" },
                    {
                        type: function (element) {
                            var error = false;
                            $('.SortOrder').each(function () {
                                var row = $(this).closest('tr');
                                var sortordergrid = clean($(row).find('.SortOrder').val());
                                var count = $(element).val();
                                if (sortordergrid >count)
                                    error = true;
                            });
                            return !error;
                        }, message: "Sort order should not be greater than queue"
                    },
                ]
            },
        ],
    },

}
