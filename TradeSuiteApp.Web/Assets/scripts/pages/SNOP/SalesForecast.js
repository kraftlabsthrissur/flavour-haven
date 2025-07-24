var fh_items;
var list_table;
var SalesForecast = {
   
    init: function () {
        var self = SalesForecast;
        self.bind_events();
        self.forecast_items();
    },
    forecast_items: function () {
        var $list = $('#forecast-items-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/SNOP/SaleForecast/GetSalesForeCastItems/"

             list_table = $list.dataTable({
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
                        data.params = [
                            { Key: "ID", Value: $('#ID').val() },
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
                           + "<input type='hidden' class='ItemID' value='" + row.ItemID + "'>";
                           + "<input type='hidden' class='ItemCode' value='" + row.ItemCode + "'>";
                       }
                   },
                   { "data": "LocationName", "className": "Location" },
                   { "data": "ItemName", "className": "ItemName" },
                   { "data": "ItemCode", "className": "ItemCode" },
                   { "data": "Unit", "className": "Unit" },
                   {
                       "data": "", "searchable": false, "className": "ComputedForecast", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text' size='10' class='ComputedForecast mask-sales-currency' value='" + row.ComputedForecast + "' readonly='readonly' />";
                       }
                   },
                   {
                       "data": "", "searchable": false, "className": "ComputedForecastInKgs", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text'size='10' class='ComputedForecastInKgs mask-sales-currency' value='" + row.ComputedForecastInKgs + "' readonly='readonly' />";
                       }
                   },
                   {
                       "data": "", "searchable": false, "className": "ComputedForecastValue", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text'size='10' class='ComputedForecastValue mask-sales-currency' value='" + row.ComputedForecastValue + "' readonly='readonly' />";
                       }
                   },
                   {
                       "data": "", "searchable": false, "className": "FinalForecast", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text'size='10' class='FinalForecast mask-sales-currency' value='" + row.FinalForecast + "' readonly='readonly' />";
                       }
                   },
                   {
                       "data": "", "searchable": false, "className": "FinalForecastInkgs", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text'size='10' class='FinalForecastInkgs mask-sales-currency' value='" + row.FinalForecastInkgs + "' readonly='readonly' />";
                       }
                   },
                   {
                       "data": "", "searchable": false, "className": "FinalForecastValue", "orderable": false,
                       "render": function (data, type, row, meta) {
                           return "<input type='text'size='10' class='FinalForecastValue mask-sales-currency' value='" + row.FinalForecastValue + "' readonly='readonly' />";
                       }
                   }
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
            $('body').on("click", '.btnProcess', function () {
                list_table.fnDraw();
            });
        }
        $("#btnExport").show();
        $("#upload").show();
    },

    list: function () {
        var $list = $('#sales-forecast-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/SNOP/SaleForecast/GetSalesForeCasts/"

            list_table = $list.dataTable({
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
                   { "data": "Month", "className": "Month" },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var Id = $(this).closest("tr").find("td .ID").val();
                        app.load_content("/SNOP/SaleForecast/Details/" + Id);
                    });
                },
            });
            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    process: function () {
        var self = SalesForecast;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/SNOP/SaleForecast/Process/',
            data: {
                ID: $("#ID").val(),
                TransDate:$("#TransDate").val(),
                TransNo:$("#TransNo").val()
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#ID").val(response.Data);
                }
            }
        });
    },

    bind_events: function () {
        var self = SalesForecast;
        $("body").on("click", ".btnProcess", self.process);
        $("body").on("click", "#btnExport", self.export_forecast);
        $('.btnSave').on('click', self.save_confirm);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
    },

    save_confirm: function () {
        var self = SalesForecast
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save: function () {
        var self = SalesForecast;
        $.ajax({
            url: '/SNOP/SaleForecast/Save',
            data: {
                ID:$("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Sales Forecast Saved Successfully");
                } else {
                    app.show_error(response.Message);
                }
            }
        });
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
            var self = SalesForecast;
            
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
                
                self.populate_uploaded_sales_forecast_List();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    populate_uploaded_sales_forecast_List: function () {
        var self = SalesForecast;
        var file_path = $('#selected-file a').data('path');
       
        altair_helpers.content_preloader_show('md', 'success');
        $.ajax({
            url: '/SNOP/SaleForecast/ReadExcel/',
            data: {
                Path: file_path,
                ID: $("#ID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $("#preloader").hide();
                    altair_helpers.content_preloader_hide();
                    list_table.fnDraw();
                }
            }
        });
    },

    export_forecast: function () {
        var self = SalesForecast;
        window.location = "/Reports/Snop/SalesForecast/?SalesForecastID=" + $("#ID").val();
    },
}