
Approvals = {
    init: function (Area, Controller, Action, ID) {
        var self = this;
        self.Area = Area;
        self.Controller = Controller;
        self.Action = Action;
        self.ID = ID;
        self.get_approval_process_details(Area, Controller, Action, ID);
        self.bind_events();
    },

    bind_events: function () {
        var self = Approvals;
        $('body').on('click', '#approval-status', self.show_approval_process_details);
        $('body').on('click', '.view-history', self.toggle_history);
        $('body').on('click', '#btnOKApproval', self.do_approval_action);
        $('body').on('click', '#btnOKClarify', self.clarify);
        $("body").on('ifChanged', '.ApprovalStatus', self.set_status);
        $('body').on('hide.uk.modal', '#approval-process', self.clear_form);
        $('body').on('click', '#btnInitiate', self.initiate_approval_request);
    },

    initiate_approval_request: function () {
        var self = Approvals;
        $.ajax({
            url: '/Approvals/Approval/InitiateApprovalRequest',
            data: {
                Area: self.Area,
                Controller: self.Controller,
                Action: "Create",
                ID: self.ID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    self.get_approval_process_details(self.Area, self.Controller, self.Action, self.ID);
                }
            },
        });
    },

    clear_form: function () {
        $("#ApprovalComment").val('');
        $('#ApprovalStatusValue').val('');
        $("#ClarificationUserID").val('');
        $("#ClarificationUserID").addClass("uk-hidden");
        $(".ApprovalStatus:checked").closest('.iradio_md').removeClass('checked');
        $(".ApprovalStatus:checked").removeAttr('checked');
    },

    toggle_history: function () {
        $('#approval-process .history').toggleClass("uk-hidden");
        if ($(this).text() == 'View History') {
            $(this).text('Hide History');
        } else {
            $(this).text('View History');
        }
    },

    show_approval_process_details: function () {
        $("#ClarificationUserID").addClass("uk-hidden");
        UIkit.modal('#approval-process', { center: false }).show();
        $("#ApprovalComment").focus();
    },

    get_approval_process_details: function (Area, Controller, Action, ID) {
        var self = Approvals;
        $.ajax({
            url: '/Approvals/Approval/GetApprovalProcessDetails',
            data: {
                Area: Area,
                Controller: Controller,
                Action: Action,
                ID: ID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {                
                if ($("div#approval-container").length == 0) {
                    var div = $("<div></div>");
                    div.attr("id", "approval-container");
                    $('body').append(div);
                    if ($(response).find('#approval-process-details').length == 1) {
                        self.load_summary(Area, Controller, ID);
                        var a = $('<a>');
                        a.attr('id', 'approval-status');
                        a.attr('class', 'md-btn md-btn-success');
                        a.text('Approval Status');
                        $('.heading_actions:visible').eq(0).prepend(a);
                    }
                }
                var $response = $(response);
                app.format($response);
                $('#approval-container').html($response);
            },
        });
    },

    load_summary: function (Area, Controller, ID) {
        var self = Approvals;
        $.ajax({
            url: '/' + Area + '/' + Controller + '/Summary',
            data: {
                ID: ID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var $response = $(response);
                app.format($response);
                $("#approval-process-details .summary").html($response);
            },
            error: function () {
            }
        })

    },

    clarify: function () {
        var self = Approvals;
        var ClarificationUserID;
        if (self.validate_form_on_clarify() != 0) {
            return;
        }

        ClarificationUserID = $("#ClarificationUserID").val()

        $.ajax({
            url: '/Approvals/Approval/Clarify',
            data: {
                ApprovalID: $("#ApprovalID").val(),
                UserID: $("#UserID").val(),
                ApprovalComment: $("#ApprovalComment").val(),
                Status: "Approved",
                ClarificationUserID: ClarificationUserID,
                Area: self.Area,
                Controller: self.Controller,
                Action: self.Action,
                ID: self.ID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                var html = $(response).html();
                $html = $(html);
                app.format($html);
                $("#approval-process").html($html);
            },
        });
    },

    do_approval_action: function () {
        var self = Approvals;
        var ClarificationUserID;
        if (self.validate_form() == 0) {
            ClarificationUserID = 0
            if ($("#ClarificationUserID").val() != "") {
                ClarificationUserID = $("#ClarificationUserID").val()
            }
            $.ajax({
                url: '/Approvals/Approval/DoApprovalAction',
                data: {
                    ApprovalID: $("#ApprovalID").val(),
                    UserID: $("#UserID").val(),
                    ApprovalComment: $("#ApprovalComment").val(),
                    Status: $('#ApprovalStatusValue').val(),
                    ClarificationUserID: ClarificationUserID,
                    Area: self.Area,
                    Controller: self.Controller,
                    Action: self.Action,
                    ID: self.ID
                },
                dataType: "html",
                type: "POST",
                success: function (response) {
                    var html = $(response).html();
                    $html = $(html);
                    app.format($html);
                    $("#approval-process").html($html);
                },
            });
        }
    },

    set_status: function () {
        if ($(".ApprovalStatus:checked").val() == "Requested Clarification") {
            $("#ClarificationUserID").removeClass("uk-hidden");
        } else {
            $("#ClarificationUserID").addClass("uk-hidden");
        }
        $("#ApprovalStatusValue").val($(".ApprovalStatus:checked").val());
    },

    list: function () {
        var self = Approvals;
        $('#tabs-approvals').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },

    tabbed_list: function (type) {
        var $list;
        if (type == "Approved") {
            $list = $('#approved-items-list');
        } else if (type == "ToBeApproved") {
            $list = $('#to-be-approved-items-list');
        } else if (type == "Rejected") {
            $list = $('#rejected-items-list');
        } else if (type == "Error") {
            $list = $('#multipleflow-items-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            altair_md.inputs($list);

            var url = "/Approvals/Approval/GetApprovalList?type=" + type;

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
                                + "<input type='hidden' class='Area' value='" + row.Area + "'>"
                                + "<input type='hidden' class='Controller' value='" + row.Controller + "'>"
                                + "<input type='hidden' class='Action' value='" + row.Action + "'>"
                                + "<input type='hidden' class='ID' value='" + row.TransID + "'>";
                        }
                    },
                    { "data": "TransNo", "className": "TransNo" },
                    { "data": "Date", "className": "Date" },
                    { "data": "Type", "className": "Type" },
                    { "data": "SupplierName", "className": "SupplierName" },
                    { "data": "ApprovalFlow", "className": "ApprovalFlow", "searchable": false, "orderable": false },
                    {
                        "data": "NetAmount", "className": "NetAmount", "searchable": false,
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.NetAmount + "</div>";
                        }
                    },
                    { "data": "SubmittedBy", "className": "SubmittedBy", "searchable": false },
                    { "data": "LastAction", "className": "LastAction", "searchable": false },
                    { "data": "NextAction", "className": "NextAction", "searchable": false },
                    { "data": "Status", "className": "Status", "searchable": false },
                    { "data": "Remarks", "className": "Remarks", "searchable": false },
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.RowStatus.toLowerCase());
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                    $list.find('tbody td').on('click', function () {
                        var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
                        var Area = $(this).closest("tr").find("td:eq(0) .Area").val();
                        var Controller = $(this).closest("tr").find("td:eq(0) .Controller").val();
                        var Action = $(this).closest("tr").find("td:eq(0) .Action").val();

                        window.location = '/' + Area + '/' + Controller + '/' + Action + '/' + Id;
                    });
                },
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });

            $list.find('thead.search input').on('textchange', function (e) {
                e.preventDefault();
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    validate_form: function () {
        var self = Approvals;
        if (self.rules.on_approval_action.length > 0) {
            return form.validate(self.rules.on_approval_action);
        }
        return 0;
    },

    validate_form_on_clarify: function () {
        var self = Approvals;
        if (self.rules.on_clarify.length > 0) {
            return form.validate(self.rules.on_clarify);
        }
        return 0;
    },

    rules: {
        on_clarify: [
             {
                 elements: "#ApprovalComment",
                 rules: [
                     { type: form.required, message: "Please enter comment" },
                 ]
             },
        ],
        on_approval_action: [
            {
                elements: "#ApprovalComment",
                rules: [
                    { type: form.required, message: "Please enter comment" },
                ]
            },
            {
                elements: "#ApprovalStatusValue",
                rules: [
                    { type: form.required, message: "Please select status" },
                ]
            },
            {
                elements: "#ClarificationUserID:visible",
                rules: [
                    { type: form.required, message: "Please select user" }
                ]
            }
        ]
    }
}