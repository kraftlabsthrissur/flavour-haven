SalesBudget = {

    init: function () {
        var self = SalesBudget;
        self.bind_events();

    },

    list: function () {
        var $list = $('#Sales-Budget-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/Masters/SalesBudget/GetSalesBudgetList"

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
                               + "<input type='hidden' class='ItemID' value='" + row.ItemID + "'>";

                       }
                   },
                   { "data": "ItemCode", "className": "ItemCode" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "Month", "className": "Month" },
                   { "data": "Category", "className": "Category" },
                   { "data": "BatchType", "className": "BatchType" },
                   { "data": "Branch", "className": "Branch" },
                   {
                       "data": "BudgetQtyInNos", "searchable": false, "className": "BudgetQtyInNos",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.BudgetQtyInNos + "</div>";
                       }
                   },
                   {
                       "data": "BudgetQtyInKgs", "searchable": false, "className": "BudgetQtyInKgs",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.BudgetQtyInKgs + "</div>";
                       }
                   },
                    {
                        "data": "BudgetGrossRevenue", "searchable": false, "className": "BudgetGrossRevenue",
                        "render": function (data, type, row, meta) {
                            return "<div class='mask-currency' >" + row.BudgetGrossRevenue + "</div>";
                        }
                    },
                     {
                         "data": "ActualQtyInNos", "searchable": false, "className": "ActualQtyInNos",
                         "render": function (data, type, row, meta) {
                             return "<div class='mask-currency' >" + row.ActualQtyInNos + "</div>";
                         }
                     },
                     {
                         "data": "ActualQtyInKgs", "searchable": false, "className": "ActualQtyInKgs",
                         "render": function (data, type, row, meta) {
                             return "<div class='mask-currency' >" + row.ActualQtyInKgs + "</div>";
                         }
                     },
                   {
                       "data": "ActualGrossRevenue", "searchable": false, "className": "ActualGrossRevenue",
                       "render": function (data, type, row, meta) {
                           return "<div class='mask-currency' >" + row.ActualGrossRevenue + "</div>";
                       }
                   },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
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
        var self = SalesBudget;
        $("body").on("click", "#btn-download-template", self.download_template);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
        $(".btnSave").on("click", self.save_confirm);
    },

    save_confirm: function () {
        var self = SalesBudget;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    download_template: function () {
        var self = SalesBudget;
        var BranchID = $('#LocationID option:selected').val();
        if (BranchID == "") {
            BranchID = 0;
        }
        else {
            BranchID = $('#LocationID option:selected').val();
        }
        window.location = "/Reports/Sales/GetItemForSalesBudget/?BranchID=" + BranchID;


    },

    upload_file: {
        action: '/File/UploadFiles', // Target url for the upload
        allow: '*.(xls|xlsx)', // File filter
        loadstart: function () {
            $("#preloader").show();
            altair_helpers.content_preloader_show('md', 'success');
        },
        notallowed: function () {
            app.show_error("File Extension Is InValid - Only Upload EXCEL File");
        },
        progress: function (percent) {
            // percent = Math.ceil(percent);
            //  bar.css("width", percent + "%").text(percent + "%");
        },
        error: function (error) {
            console.log(error);
        },
        allcomplete: function (response, xhr) {
            var self = SalesBudget;
            $("#preloader").hide();
            altair_helpers.content_preloader_hide();
            var data = $.parseJSON(response)
            var width;
            var success = "";
            var failure = "";
            if (typeof data.Status != "undefined" && data.Status == "Success") {
                width = $('#selected-quotation').width() - 20;
                $(data.Data).each(function (i, record) {
                    if (record.URL != "") {
                        $('#selected-file').html("<a class='view-file' style='width:" + width + "px;' data-id='" + record.ID + "' data-url='" + record.URL + "' data-path='" + record.Path + "'>" + record.Name + "</a>");
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
                self.populate_uploaded_SalesBudget();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    populate_uploaded_SalesBudget: function () {
        var self = SalesBudget;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Masters/SalesBudget/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    //window.location = "/Masters/SalesBudget/Create";
                }
            }
        });
    },

    Save: function () {
        var self = SalesBudget;
        $.ajax({
            url: '/Masters/SalesBudget/Save/',
            data: {},
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/SalesBudget/Index";
                }
                else {
                    app.show_error('Failed to create SalesBudget');
                }
            }
        });
    },

    validate_form: function () {
        var self = SalesBudget;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
            {
                elements: "#select-file",
                rules: [
                    { type: form.required, message: "Please choose a file" }
                ],
            },
        ],
    }

}