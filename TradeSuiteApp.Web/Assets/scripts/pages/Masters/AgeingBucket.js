var fh_items;
AgeingBucket = {
    init: function () {
        var self = AgeingBucket;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.list();
        } else {
            fh_items = $("#tbl_ageing_bucket").FreezeHeader();
        }

        if (self.view_type == "form") {
            self.bind_events();
        }
    },
    list: function () {
        $scheme_list = $('#ageing-bucket-list');
        $('#ageing-bucket-list tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Masters/AgeingBucket/Details/' + id;
        });
        if ($scheme_list.length) {
            $scheme_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var list_table = $scheme_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },
    bind_events: function () {
        var self = AgeingBucket;
        $("body").on("click", "#btnadd", self.add_item);
        $("body").on("click", ".btnSave", self.save_confirm);
        $("body").on("click", ".remove-item", self.remove_item);
        $("body").on("change", ".Start", self.rearrange_startday);
        $("body").on("change", ".End", self.rearrange_endday);
    },
    save_confirm: function () {
        var self = AgeingBucket
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },
    save: function () {
        var self = AgeingBucket;
        self.error_count = 0;
        self.error_count = self.validate_submit();
        if (self.error_count > 0) {
            return;
        }
        var model = self.get_data();
        $(".btnSave").css({ 'display': 'none' });
        $.ajax({
            url: '/Masters/AgeingBucket/Create',
            data: { model: model },
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice(data.Message);
                    setTimeout(function () {
                        window.location = "/Masters/AgeingBucket/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $(".btnSave ").css({ 'display': 'block' });
                }
            },
        });
        //}
    },
    add_item: function () {
        var self = AgeingBucket;
        self.error_count = 0;
        self.error_count = self.validate_trans();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.ID = 0;
        item.Description = $("#Start").val() + '- ' + +$("#End").val() + ' Days';
        item.Bucket = $("#Bucket").val();
        item.Start = $("#Start").val();
        item.End = $("#End").val();
        item.Code = $("#Code").val();
        self.add_item_to_grid(item);
    },
    check_days: function () {
        var self = AgeingBucket;
        var count = 0;
        var start = clean($("#Start").val());
        var end = clean($("#End").val());
        var startgrid, endgrid, count = 0;
        var item = {};
        if ($("#tbl_ageing_bucket tbody tr").length == 0) {
            item.ID = 0;
            item.Description = $("#Start").val() + '- ' + +$("#End").val() + ' Days';
            item.Bucket = $("#Bucket").val();
            item.Start = $("#Start").val();
            item.End = $("#End").val();
            self.add_item_to_grid(item);
        }
        else {
            $("#tbl_ageing_bucket tbody tr").each(function () {
                startgrid = clean($(this).find('.Start').val());
                endgrid = clean($(this).find('.End').val());
                if ((start >= startgrid) && (end <= endgrid)) {
                    count++;
                }
            });
            if (count > 0) {
                $("#tbl_ageing_bucket tbody tr").each(function () {
                    startgrid = clean($(this).find('.Start').val());
                    endgrid = clean($(this).find('.End').val());
                    if ((start >= startgrid) && (end <= endgrid)) {
                        $(this).closest("tr").remove();
                        item.ID = 0;
                        item.Description = startgrid + '- ' + (start - 1) + ' Days';
                        item.Bucket = $("#Bucket").val()
                        item.Start = startgrid;;
                        item.End = (start - 1);
                        self.add_item_to_grid(item);
                        item.ID = 0;
                        item.Description = start + '- ' + +end + ' Days';
                        item.Bucket = $("#Bucket").val();
                        item.Start = start;
                        item.End = end;
                        self.add_item_to_grid(item);
                        item.ID = 0;
                        item.Description = (end + 1) + '- ' + +endgrid + ' Days';
                        item.Bucket = $("#Bucket").val();
                        item.Start = (end + 1);
                        item.End = endgrid;
                        self.add_item_to_grid(item);
                    }
                });
            }
            else {
                item.ID = 0;
                item.Description = $("#Start").val() + '- ' + +$("#End").val() + ' Days';
                item.Bucket = $("#Bucket").val();
                item.Start = $("#Start").val();
                item.End = $("#End").val();
                self.add_item_to_grid(item);
            }

        }
        self.clear_data();
        $("#Start").focus();
        self.count_items();
    },
    rearrange_startday: function () {
        var row = (this).closest('tr');
        var next_row = $(row).next('tr');
        var previous_row = $(row).prev('tr');
        var start, end, desc;
        var previous_row_length = previous_row == undefined ? 0 : $(row).prev('tr').length;
        var start_day = clean($(row).find('.Start').val());
        var previous_start_day = previous_row == undefined ? 0 : $(previous_row).find('.Start').val();
        var current_end_day = $(row).find('.End').val();
        if (((start_day > previous_start_day) && (start_day < current_end_day)) || ((start_day > previous_start_day) && (current_end_day == 0))) {
            $(previous_row).find('.End').val(start_day - 1);
            start = clean($(row).find('.Start').val());
            end = clean($(row).find('.End').val());
            desc = start + '- ' + end + ' Days';
            $(row).find('.description').text(desc);


            start = clean($(previous_row).find('.Start').val());
            end = clean($(previous_row).find('.End').val());
            desc = start + '- ' + end + ' Days';
            $(previous_row).find('.description').text(desc);
        }
        else {
            if (previous_row.length != 0)
                app.show_error('Invalid start day');
            else {
                start = clean($(row).find('.Start').val());
                end = clean($(row).find('.End').val());
                desc = start + '- ' + end + ' Days';
                $(row).find('.description').text(desc);
            }
        }

    },
    rearrange_endday: function () {
        var row = (this).closest('tr');
        var next_row = $(row).next('tr');
        var previous_row = $(row).prev('tr');
        var start, end, desc;
        var next_row_length = next_row == undefined ? 0 : $(row).next('tr').length;
        var end_day = clean($(row).find('.End').val());
        var next_end_day = next_row == undefined ? 0 : $(next_row).find('.End').val();
        var current_start_day = $(row).find('.Start').val();
        if ((end_day < next_end_day) && (end_day > current_start_day)) {
            $(next_row).find('.Start').val(end_day + 1);
            start = clean($(row).find('.Start').val());
            end = clean($(row).find('.End').val());
            desc = start + '- ' + end + ' Days';
            $(row).find('.description').text(desc);
            start = clean($(next_row).find('.Start').val());
            end = clean($(next_row).find('.End').val());
            desc = start + '- ' + end + ' Days';
            $(next_row).find('.description').text(desc);
        }
        else {
            if (next_row.length != 0)
                app.show_error('Invalid end day');
            else {
                start = clean($(row).find('.Start').val());
                end = clean($(row).find('.End').val());
                if (end == 0) {
                    desc = 'Above ' + start + ' Days';
                }
                else {
                    desc = start + '- ' + end + ' Days';
                }
                $(row).find('.description').text(desc);
            }
        }

    },

    add_item_to_grid: function (item) {
        var self = AgeingBucket;
        var html = '';
        var desc;
        if (clean(item.End) == 0) {
            desc = 'Above ' + item.Start + ' Days';
        }
        else {
            desc = item.Description;
        }
        var SerialNo = $("#tbl_ageing_bucket tbody tr").length + 1;
        var html = '  <tr class="rowPr">' +
                    ' <td class="uk-text-center index">' + SerialNo
                    + '<input type="hidden" class="ID" value="' + item.ID + '" /></td>'
                    + ' <td class ="Code">' + item.Code + '</td>'
                    + ' <td class="Bucket">' + item.Bucket + '</td>'
                    + ' <td class="uk-text-right StartDay"><input type="text" class="md-input uk-text-right Start mask-numeric" value="' + item.Start + '" /></td>'
                    + ' <td class="uk-text-right EndDay"><input type="text" class="md-input uk-text-right End mask-numeric" value="' + item.End + '" /></td>'
                    + ' <td class="description">' + desc + '</td>'
                    + ' <td class="uk-text-center remove-item" >'
                    + '   <a data-uk-tooltip="{pos:"bottom"}" >'
                    + '   <i class="md-btn-icon-small uk-icon-remove"></i>'
                    + ' </a>'
                    + ' </td>'
                    + '</tr>';
        var $html = $(html);
        app.format($html);
        $("#tbl_ageing_bucket tbody").append($html);
        self.clear_data();
        fh_items.resizeHeader();

    },
    remove_item: function () {
        var self = AgeingBucket;
        var row = (this).closest('tr');
        var next_row = $(row).next('tr');
        var previous_row = $(row).prev('tr');
        var start, end, desc;

        end = clean($(row).find('.End').val());

        if (previous_row.length != 0) {
            start = clean($(previous_row).find('.Start').val());
            if (end == 0) {
                desc = 'Above ' + start + ' Days';
            }
            else {
                desc = start + '- ' + end + ' Days';
            }
            $(previous_row).find('.End').val(end);
            $(previous_row).find('.description').text(desc);
        }
        $(this).closest("tr").remove();
        self.count_items();

    },
    clear_data: function () {
        $("#Start").val('');
        $("#End").val('');
    },
    count_items: function () {
        $("#tbl_ageing_bucket tbody tr").each(function (i, record) {
            $(this).find('.index').text(i + 1);
        });
        $("#item-count").val($("#tbl_ageing_bucket tbody tr").length);

    },

    get_data: function () {
        self = AgeingBucket;
        var model = {
            ID: $("#ID").val(),
            Code:$("#Code").val(),
            Name: $("#Bucket").val(),
            Trans: self.GetTransList(),
        };
        return model;
    },
    GetTransList: function () {
        var ProductsArray = [];
        var i = 0;
        $("tbody tr").each(function () {
            i++;
            ProductsArray.push({
                ID: $(this).find('.ID').val(),
                Name: $(this).find('.description').text(),
                Start: clean($(this).find('.Start').val()),
                End: clean($(this).find('.End').val())
            });
        })
        return ProductsArray;
    },
    validate_trans: function () {
        var self = AgeingBucket;
        if (self.rules.on_trans.length > 0) {
            return form.validate(self.rules.on_trans);
        }
        return 0;
    },
    validate_submit: function () {
        var self = AgeingBucket;
        if (self.rules.on_submit.length > 0) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    rules: {
        on_trans: [
       {
           elements: "#Start",
           rules: [
               { type: form.required, message: "Please enter start days" },

               {
                   type: function (element) {
                       var start = clean($("#Start").val());
                       var count = 0, startgrid;
                       $("#tbl_ageing_bucket tbody tr").each(function () {
                           var row = $(this).closest('tr');
                           startgrid = clean($(row).find('.Start').val());
                           if ((startgrid == start))
                               count++;
                       });
                       return count == 0
                   }, message: "Start day already exist in grid please edit grid"
               },
           ]
       },
       {
           elements: ".Start",
           rules: [
                {
                    type: function (element) {
                        var start = clean($("#Start").val());
                        var count = 0, endgrid;
                        $("#tbl_ageing_bucket tbody tr").each(function () {
                            var row = $(this).closest('tr');
                            endgrid = clean($(row).find('.End').val());
                            if ((endgrid == start) && (start != 0))
                                count++;
                        });
                        return count == 0
                    }, message: "Start day already exist in grid as end day"
                },
           ]
       },
       {
           elements: "#End",
           rules: [
               { type: form.required, message: "Please enter end days" },

               {
                   type: function (element) {
                       var start = clean($("#Start").val());
                       var end = clean($("#End").val());
                       var count = 0;
                       if ((end < start) && (end != 0))
                           count++;
                       return count == 0
                   }, message: "End days must be greater than start days"
               },
               {
                   type: function (element) {
                       var end = clean($("#End").val());
                       var count = 0, endgrid;
                       $("#tbl_ageing_bucket tbody tr").each(function () {
                           var row = $(this).closest('tr');
                           endgrid = clean($(row).find('.End').val());
                           if ((endgrid == end))
                               count++;
                       });
                       return count == 0
                   }, message: "End day already exist in grid please edit grid"
               },
               {
                   type: function (element) {
                       var end = clean($("#End").val());
                       var start = clean($("#Start").val());
                       var count = 0, endgrid, startgrid;
                       $("#tbl_ageing_bucket tbody tr").each(function () {
                           var row = $(this).closest('tr');
                           endgrid = clean($(row).find('.End').val());
                           startgrid = clean($(row).find('.Start').val());
                           if ((end > startgrid && start < endgrid))
                               count++;
                       });
                       return count == 0
                   }, message: "Please edit grid"
               },
           ]
       },
        {
            elements: ".End",
            rules: [
                 {
                     type: function (element) {
                         var end = clean($("#End").val());
                         var count = 0, startgrid;
                         $("#tbl_ageing_bucket tbody tr").each(function () {
                             var row = $(this).closest('tr');
                             startgrid = clean($(row).find('.Start').val());
                             if ((startgrid == end) && (end != 0))
                                 count++;
                         });
                         return count == 0
                     }, message: "End day already exist in grid as start day"
                 },
            ]
        },
       {
           elements: "#Bucket",
           rules: [
               { type: form.required, message: "Please enter bucket" },
           ]
       },

        ],
        on_submit: [

   {
       elements: ".Start",
       rules: [
            {
                type: function (element) {
                    var count = 0, end, start;
                    $("#tbl_ageing_bucket tbody tr").each(function () {
                        var row = $(this).closest('tr');
                        end = clean($(row).find('.End').val());
                        var nextrow = $(row).next('tr');
                        start = clean($(nextrow).find('.Start').val());
                        if (start != (end + 1) && nextrow.length != 0)
                            count++;
                    });
                    return count == 0
                }, message: "Some days are missing, please recheck grid"
            },
       ]
   },
        ],
    }
}