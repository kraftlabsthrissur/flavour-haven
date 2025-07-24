$(function () {
    PriceList.list();
    PriceList.bind_events();
    PriceList.check_PriceList();
});

PriceList = {
    list: function () {
        $list = $('#Price-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            $('#Price-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/PriceList/Details/' + Id;
            });
            altair_md.inputs();
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function () {
        var self = PriceList;
        $("body").on("ifChanged", ".check-box", self.check_PriceList);
        $(".btnSave").on("click", self.Save);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
    },

    check_PriceList: function () {
        var self = PriceList;
        $("#Pricelist tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
                $(row).find(".ISKMRP ").prop("disabled", false);
                $(row).find(".ISKLoosePrice").prop("disabled", false);
                $(row).find(".OSKMRP").prop("disabled", false);
                $(row).find(".OSKLoosePrice").prop("disabled", false);
                $(row).find(".ExportMRP").prop("disabled", false);
                $(row).find(".ExportLoosePrice").prop("disabled", false);

            } else {
                $(row).find(".ISKMRP ").prop("disabled", true);
                $(row).find(".ISKLoosePrice").prop("disabled", true);
                $(row).find(".OSKMRP").prop("disabled", true);
                $(row).find(".OSKLoosePrice").prop("disabled", true);
                $(row).find(".ExportMRP").prop("disabled", true);
                $(row).find(".ExportLoosePrice").prop("disabled", true);
            }
        });
        self.count_items();
    },

    count_items: function () {
        var count = $('#Pricelist tbody').find('input.check-box:checked').length;
        $('#item-count').val(count);
    },

    validate_form: function () {
        var self = PriceList;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
    },

    get_data: function () {
        var self = PriceList;
        var data = {};
        data.Name = $("#Name").val(),
        data.ID = $("#ID").val(), 
        data.FromDate = $("#FromDate").val(),
        data.ToDate = $("#ToDate").val(),
        data.Items = [];
        var item = {};
        $('#Pricelist tbody tr.included').each(function () {
            item = {};
            item.ID = $(this).find(".ID").val();
            item.ISKMRP = clean($(this).find(".ISKMRP").val());
            item.ISKLoosePrice = $(this).find(".ISKLoosePrice").val();
            //item.OSKMRP = $(this).find(".OSKMRP").val();
            //item.OSKLoosePrice = $(this).find(".OSKLoosePrice").val();
            //item.ExportMRP = $(this).find(".ExportMRP").val();
            //item.ExportLoosePrice = $(this).find(".ExportLoosePrice").val();
            item.ItemCode = $(this).find(".ItemCode").text().trim();
            item.ItemName = $(this).find(".ItemName").text();
            data.Items.push(item);
        });
        return data;
    },

    Save: function () {
        var self = PriceList;
        //var tablelength = $('Pricelist tbody tr.included').length;
        //$('#item-count').val(tablelength);
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/PriceList/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/PriceList/Index";
                }
                else {
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
            var self = PriceList;
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
                self.populate_uploaded_PriceList();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    populate_uploaded_PriceList: function () {
        var self = PriceList;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Masters/PriceList/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var html = "";
                var $html;
                if (response.Status == "success") {
                    $.each(response.Data, function (i, record) {
                        //if (record.MRP > 0) {
                            var slno = (i + 1);
                            html += '<tr class="included">'
                                    + "<td>" + slno + "</td>"
                                    + '<td class=" td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box"/>' + '</td>'
                                    + '<td class="ItemCode">' + record.ItemCode + '</td>'
                                    + '<td class="ItemName">' + record.ItemName + '</td>'
                                    + '<td>' + '<input type="text" value=" ' + record.ISKMRP + '" class="md-input ISKMRP uk-text-right" disabled="disabled" />' + '</td>'
                                    + '<td>' + '<input type="text" value=" ' + record.ISKLoosePrice + '" class="md-input ISKLoosePrice uk-text-right" disabled="disabled" />' + '</td>'
                                    + "</tr>";
                        //}
                        });
                    }
                if (html != "") {
                    $html = $(html);
                    app.format($html);
                    $("#Pricelist tbody").html($html);
                    self.count_items();
                }
                
            }
        });
       
    },

    rules: {
        on_submit: [
             {
                 elements: "#Name",
                 rules: [
                     { type: form.required, message: "Please enter Name" }
                 ]
             },
             {
                 elements: "#FromDate",
                 rules: [
                      { type: form.required, message: "Please enter FromDate" }
                 ]
             },
             {
                 elements: "#ToDate",
                 rules: [
                      { type: form.required, message: "Please enter ToDate" }
                 ]
             },
              {
                  elements: "#item-count",
                  rules: [
                      { type: form.required, message: "Please add atleast one item" },
                      { type: form.non_zero, message: "Please add atleast one item" },
                  ],
              }
        ]
    }

   
    }