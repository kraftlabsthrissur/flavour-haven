LaboratoryTestResult = {
    init: function () {
        var self = LaboratoryTestResult;
        self.bind_events();

    },


    list: function () {
        var self = LaboratoryTestResult;
        $('#tabs-labtest').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "invoiced":
                $list = $('#invoiced-list');
                break;
            case "completed":
                $list = $('#completed-list');
                break;
            default:
                $list = $('#invoiced-list');
        }
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/LaboratoryTestResult/GetInvoicedLabTestList?type=" + type;
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
                               + "<input type='hidden' class='SalesInvoiceID' value='" + row.SalesInvoiceID + "'>"
                               + "<input type='hidden' class='OPID' value='" + row.OPID + "'>"
                               + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>";

                       }
                   },

                   { "data": "InvoiceNo", "className": "InvoiceNo" },
                   { "data": "InvoiceDate", "className": "InvoiceDate" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "Doctor", "className": "Doctor" },
                   { "data": "GrossAmt", "className": "GrossAmt mask-sales-currency" },

                ],
                "createdRow": function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.ID)').on('click', function () {
                        var SalesInvoiceID = $(this).closest("tr").find("td .SalesInvoiceID").val();
                        var OPID = $(this).closest("tr").find("td .OPID").val();
                        var PatientID = $(this).closest("tr").find("td .PatientID").val();
                        app.load_content("/AHCMS/LaboratoryTestResult/Create/?SalesInvoiceID=" + SalesInvoiceID + "&OPID=" + OPID + "&PatientID=" + PatientID);

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
        var self = LaboratoryTestResult;
        $("body").on('ifChanged', '.check-box', self.include_labtest);
        $("body").on("click", ".btnSave", self.save_confirm);
        $("body").on("click", ".print", self.printpdf);
        UIkit.uploadSelect($(".select-file"), self.selected_lab_settings);
        var settings;
        $(".select-labItem").each(function (i) {
            settings = self.selected_lab_settings(i + 1);
            UIkit.uploadSelect($("#select-labItem-" + (i + 1)), settings);
        });
        var remove;
        $(".remove").each(function (i) {
            remove = self.remove_lab(i + 1);
            UIkit.uploadSelect($("a.remove-quotation-" + (i + 1)), remove);
        });
    },

    remove_lab: function (i) {
        var self = Xray;
        $(".remove-quotation-" + i).closest('span').remove();
    },
    get_data: function () {
        var self = LaboratoryTestResult;
        var data = {};
        data.Items = [];
        var item = {};
        $('#labtest_result_list tbody tr.included').each(function () {
            item = {};
            item.PatientLabTestsID = $(this).find(".PatientLabTestsID").val();
            item.PatientLabTestTransID = $(this).find(".PatientLabTestTransID").val();
            item.ObservedValue = $(this).find(".ObservedValue").val();
            item.ReportedTime = $(this).find(".ReportedTime").val();
            item.CollectedTime = $(this).find(".CollectedTime").val();
            item.Status = $(this).find(".Status").val();
            var Files = $(this).find('.selected-labItem .view-file')
            item.DocumentID = Files.length > 0 ? $(this).find('.selected-labItem .view-file')[0].dataset.id : 0
            data.Items.push(item);
        });
        return data;
    },

    save: function () {
        var self = LaboratoryTestResult;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/LaboratoryTestResult/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/AHCMS/LaboratoryTestResult/Index";
                }
                else {
                    app.show_error('Failed to create LaboratoryTestResult');
                }
            }
        });
    },

    include_labtest: function () {
        var self = LaboratoryTestResult
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.ObservedValue').addClass('included').removeAttr('disabled');
            $(this).closest('tr').find('.Status').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('.ObservedValue').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').find('.Status').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            self.count();
        }
    },

    save_confirm: function () {
        var self = LaboratoryTestResult;
        self.error_count = 0;
        self.count();
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    validate_save: function () {
        var self = LaboratoryTestResult;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    count: function () {
        var self = LaboratoryTestResult;
        index = $("#labtest_result_list tr.included").length;
        $("#item-count").val(index);
    },
    printpdf: function () {
        var self = LaboratoryTestResult;
        $.ajax({
            url: '/Reports/AHCMS/LabTestResultPrintPDF',
            data: {
                ID: $("#OPID").val(),
                SalesInvoiceID: $("#SalesInvoiceID").val(),
                PatientID: $("#PatientID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status = "success") {
                    var url = response.URL;
                    app.print_preview(url);
                }
            }
        });
    },
    selected_lab_settings: function (inc) {
        return {
            action: '/File/UploadFiles', // Target url for the upload
            allow: '*.(txt|doc|docx|pdf|xls|xlsx|jpg|jpeg|gif|png|csv)',  // File filter
            loadstart: function () {

                $("#preloader").show();
                altair_helpers.content_preloader_show('md', 'success');
            },
            notallowed: function () {
                app.show_error("File extension is invalid - only upload PDF/Image File");
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
                    width = $('#selected-xray-' + inc).width() - 30;
                    $(data.Data).each(function (i, record) {
                        if (record.URL != "") {
                            $('#selected-labItem-' + inc).html("<span><span class='view-file' style='width:90%;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name);
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
        }
    },

    rules: {
        on_save: [

                {
                    elements: "#item-count",
                    rules: [
                        { type: form.non_zero, message: "Please add atleast one item" },
                      { type: form.required, message: "Please add atleast one item" },
                    ]
                },
        ]
    },
}