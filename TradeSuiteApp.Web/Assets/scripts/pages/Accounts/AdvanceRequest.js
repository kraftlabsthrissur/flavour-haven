var freeze_header;
advance_request = {
    init: function () {
        self = advance_request;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            returnFocus: "#ItemName",
            modal: "#select-employee",
            initiatingElement: "#EmployeeName"
        });
        item_list = Item.offical_advance_items_list();
        item_select_table = $('#item-list').SelectTable({
            selectFunction: self.select_item,
            returnFocus: "#txtRqQty",
            modal: "#select-item",
            initiatingElement: "#ItemName",
            startFocusIndex: 3
        });

        freeze_header = $("#advance-request-item-list").FreezeHeader();
        self.bind_events();
    },
    details: function () {
        freeze_header = $("#advance-request-item-list").FreezeHeader();
    },


    list: function () {
        var self = advance_request;
        $('#tabs-advance-request').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });

    },

    tabbed_list: function (type) {
        var self = advance_request;

        var $list;

        switch (type) {
            case "saved":
                $list = $('#saved-list');
                break;
            case "approved":
                $list = $('#approved-list');
                break;
            case "processed":
                $list = $('#processed-list');
                break;
            case "suspended":
                $list = $('#suspended-list');
                break;
            default:
                $list = $('#saved-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Accounts/AdvanceRequest/GetAdvanceRequestListForDataTable?type=" + type;

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[1, "Desc"]],
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
                   { "data": "AdvanceRequestNo", "className": "AdvanceRequestNo" },
                   { "data": "AdvanceRequestDate", "className": "AdvanceRequestDate" },
                   { "data": "EmployeeName", "className": "EmployeeName" },
                   {
                       "data": "Amount", "searchable": false, "className": "Amount",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.Amount + "</div>";
                       }
                   },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/Accounts/AdvanceRequest/Details/" + Id);
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
        var self = advance_request
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': advance_request.get_items, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', advance_request.set_item);
        $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_item_details, 'minLength': 1 });
        $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
        $("#btnAddRequest").on('click', advance_request.add_item);
        $(".btnSaveAndPost").on("click", advance_request.save_confirm);
        $("body").on('ifChanged', '.chkCheck', advance_request.include_item);
        $("#btnOKItem").on("click", advance_request.select_item);
        $("#btnOKEmployee").on("click", advance_request.select_employee);
        $("#AdvanceCategory").on('change', advance_request.Clear_item);
        $("#EmployeeName").on('ifChanged', advance_request.Clear_employee_textbox);
        $('body').on('keyup change', '#advance-request-item-list .amount', self.calculateTotalAmount);
        $("body").on('click', '.btnsuspend', advance_request.confirm_suspend);
        UIkit.uploadSelect($("#select-quotation"), advance_request.selected_quotation_settings);
        $('body').on('click', 'a.remove-quotation', advance_request.remove_quotation);
    },
    remove_quotation: function () {
        $(this).closest('span').remove();
    },
    selected_quotation_settings: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File extension is invalid - only upload WORD/PDF/EXCEL/TXT/CSV/Image File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 30;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-quotation').html("<span><span class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</span><a class='remove-quotation'>X</a></span>");
                        success += record.Name + " " + record.Description + "<br/>";
                    } else {
                        failure += record.Name + " " + record.Description + "<br/>";
                    }
                });
            } else {
                failure = data.Message;
            }
            if (success != "") {
                app.show_notice(success);
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },
    confirm_suspend: function (e) {
        var self = advance_request;
        var Id = $("#ID").val();
        app.confirm("Do you really want to Suspend? This can not be undone.", function () {
            self.Suspend(Id);
        });
    },
    Suspend: function (Id) {
        $.ajax({
            url: '/Accounts/AdvanceRequest/Suspend',
            data: {
                ID: Id,
            },
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.Status == "success") {
                    $(".btnsuspend").css({ 'display': 'none' });
                        app.show_notice("Advance Request Suspended Successfully");
                        setTimeout(function () {
                            window.location = "/Accounts/AdvanceRequest/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to Suspended Advance Request.");
                        $(".btnsuspend").css({ 'display': 'block' });
                    }
            },
        });
    },
    Clear_employee_textbox: function () {
        $("#EmployeeID").val(0);
    },
    calculateTotalAmount: function () {
        var total = 0;
        var totalTDS = 0;
        var totalnetamt = 0;

        $('#advance-request-item-list tbody tr').each(function () {
            var row = $(this);
            var amt = clean(row.find('.amount').val());
            totalnetamt += amt;
        });
        $("#TotalAmount").val(totalnetamt.toFixed(2));
    },

    save_confirm: function () {
        var self = advance_request
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    add_item: function (PrRowClass) {
        var self = advance_request;
        var official = 0;
        if ($("#AdvanceCategory option:selected").text() == "Official") {
            official = 1;
        }
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count == 0) {
            var SerialNo = $("#advance-request-item-list tbody tr").length + 1;
            var html = '<tr class="included">'
                  + '<td class="uk-text-center">' + SerialNo + ' <input type="hidden" class="official included" value="' + official + '"/></td>'
                  + '<td class="uk-text-center checked chkValid" data-md-icheck><input type="checkbox" class="chkCheck"  checked/></td>'
                  + '<td class="employee">' + $("#EmployeeName").val() + '<input type="hidden" class="employeeId included" value="' + $("#EmployeeID").val() + '"/></td>'
                  + '<td class="itemname">' + $("#ItemName").val() + '<input type="hidden" class="itemId included" value="' + $("#ItemID").val() + '"/></td>'
                  + '<td class=""><input type="text" class="md-input mask-currency amount included" value="' + $("#Amount").val() + '"  /> </td>'
                  + '<td><span class="md-input"><input type="text" class="md-input uk-icon-calendar future-date expected_date included" value="' + $("#ExpectedDate").val() + '"  /></td>'
                  + '<td><input type="text" class="md-input remarks included" value="' + $("#Remarks").val() + '"  /></td>'
                  + '</tr>';
            var $html = $(html);
            app.format($html);
            $("#advance-request-item-list tbody").append($html);
            advance_request.clear_item_select_controls();
            advance_request.include_item();

            $("#ItemName").focus();
            freeze_header.resizeHeader();
        }
    },
    get_advance_request_trans_model: function () {
        var ProductsArray = [];
        $("#advance-request-item-list tbody tr.included").each(function () {
            if ($(this).find('.chkValid .chkCheck').is(':checked')) {

                ProductsArray.push({
                    EmployeeID: $(this).find(".employeeId").val(),
                    ItemID: $(this).find(".itemId").val(),
                    Amount: clean($(this).find(".amount").val()),
                    ExpectedDate: $(this).find(".expected_date").val(),
                    Remarks: $(this).find(".remarks").val(),
                    IsOfficial: $(this).find(".official").val() == "1" ? true : false,
                });
            }
        })
        return ProductsArray;
    },
    include_item: function () {
        if ($(this).prop("checked")) {
            $(this).closest('tr').find('input:not(:checkbox), select').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('input, select').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }
        $(this).removeAttr('disabled');
        advance_request.count_items();
    },
    count_items: function () {
        var count = $('#advance-request-item-list tbody').find('.chkCheck:checked').length;
        $('#item-count').val(count);
        advance_request.calculate_total_amount();
    },

    calculate_total_amount: function () {
        var total_amount = 0;
        $("#advance-request-item-list tbody tr.included").each(function () {
            if ($(this).find('.chkValid .chkCheck').is(':checked')) {

                $(this).find('.amount').each(function () {
                    total_amount += clean($(this).val());
                });
            }
        });
        $("#TotalAmount").val(total_amount);
    },
    clear_item_select_controls: function () {
        $("#ItemName").val('');
        $("#ItemID").val('');
        $("#Amount").val('');
        $("#ExpectedDate").val('');
        $("#Remarks").val('');
        $("#EmployeeName").val('');
        $("#EmployeeID").val('');

    },

    set_item_details: function (event, item) {   // on select auto complete item
        $("#ItemName").val(item.name);
        $("#ItemID").val(item.id);
        $("#Amount").focus();
    },

    select_item: function () {
        var self = advance_request;
        var radio = $('#select-item tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        $("#ItemName").val(Name);
        $("#ItemID").val(ID);
        UIkit.modal($('#select-item')).hide();
    },

    get_item_details: function (release) {
        $.ajax({
            url: '/Masters/Item/GetItemsForOfficialAdvanceForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                Type: $("#AdvanceCategory").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    Clear_item: function () {
        $('#ItemName').val('');
        $('#ItemID  ').val('');
    },
    get_data: function () {
        var self = advance_request;

        var model = {

            AdvanceRequestNo: $("#AdvanceRequestNo").val(),
            AdvanceRequestDate: $("#AdvanceRequestDate").val(),
            Amount: clean($("#TotalAmount").val()),
            SelectedQuotationID: $('#selected-quotation .view-file').data('id'),
        };
        return model;
    },

    save: function () {
        var self = advance_request;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var model = self.get_data();
        var trans = self.get_advance_request_trans_model();

        $(".btnSaveAndPost ").css({ 'display': 'none' });
        $.ajax({
            url: '/Accounts/AdvanceRequest/Create',
            data: { model: model, trans: trans },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == "success") {
                    app.show_notice("Advance Request created successfully");
                    setTimeout(function () {
                        window.location = "/Accounts/AdvanceRequest/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create Advance Request.");
                    $(".btnSaveAndPost").css({ 'display': 'block' });
                }
            },
        });
    },
    validate_item: function () {
        var self = advance_request;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },
    validate_form: function () {
        var self = advance_request;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_submit: [
        {
            elements: "#TotalAmount",
            rules: [
                { type: form.non_zero, message: "NetAmount can't be zero" },
                { type: form.required, message: "NetAmount can't be zero" },
            ]
        },
            {
                elements: (".amount.included"),
                rules: [
                     { type: form.positive, message: " Amount must be positive" },
                ]
            },
        ],
        on_add: [
            {
                elements: "#EmployeeID",
                rules: [
                   {
                       type: function (element) {
                           var total = clean($(element).val());
                           return total > 0;
                       }, message: "Please choose an employee"
                   }]
            },

            {
                elements: "#Amount",
                rules: [
                    {
                        type: function (element) {
                            var total = clean($(element).val());
                            return total > 0;
                        }, message: "Please enter advance amount"
                    }
                ]
            },
                 {
                     elements: "#ExpectedDate",
                     rules: [
                         { type: form.required, message: "Please choose an expected date" },
                         { type: form.future_date, message: "Invalid Date" },
                     ]
                 },
                   {
                       elements: "#ItemID",
                       rules: [
                           { type: form.required, message: "Please choose an item", alt_element: "#ItemName" },
                           { type: form.positive, message: "Please choose an item", alt_element: "#ItemName" },
                           { type: form.non_zero, message: "Please choose an item", alt_element: "#ItemName" }
                       ],
                   },

                   {
                       elements: "#AdvanceCategory",
                       rules: [
                           { type: form.required, message: "Please choose advance category" },
                       ]
                   },
        ]
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
        var self = advance_request;
        console.log(item)
        $("#EmployeeID").val(item.id),
        $("#Name").val(item.name);
        $("#Code").val(item.code);
        $("#Place").val(item.place)
    },
    select_employee: function () {
        var self = advance_request;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var place = $(row).find(".Location").text().trim();
        $("#EmployeeName").val(Name);
        $("#EmployeeID").val(ID);
        $("#Code").val(Code);
        $("#Place").val(place);
        UIkit.modal($('#select-employee')).hide();

    },
};