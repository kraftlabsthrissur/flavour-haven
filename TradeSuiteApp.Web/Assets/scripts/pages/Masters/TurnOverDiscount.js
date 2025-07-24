var fh;
TurnOverDiscounts = {

    init: function () {
        var self = TurnOverDiscounts;
        self.customer_list();
        $('#customer-list').SelectTable({
            selectFunction: self.select_customer,
            returnFocus: "#DespatchDate",
            modal: "#select-customer",
            initiatingElement: "#CustomerName"
        });
        self.bind_events();
        fh = $("#TurnOverDiscounts").FreezeHeader();
    },

    customer_list: function () {
        var $list = $('#customer-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": "/Masters/TurnOverDiscount/GetCustomerListForLocation",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "CustomerLocationID", Value: $('#LocationID').val() },
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
                            return meta.settings.oAjaxData.start + meta.row + 1;
                        }
                    },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return "<input type='radio' class='uk-radio CustomerID' name='CustomerID' data-md-icheck value='" + row.ID + "' >"
                             + "<input type='hidden' class='StateID' value='" + row.StateID + "'>"
                             + "<input type='hidden' class='PriceListID' value='" + row.PriceListID + "'>"
                             + "<input type='hidden' class='CountryID' value='" + row.CountryID + "'>"
                             + "<input type='hidden' class='DistrictID' value='" + row.DistrictID + "'>"
                             + "<input type='hidden' class='CustomerCategoryID' value='" + row.CustomerCategoryID + "'>"
                             + "<input type='hidden' class='SchemeID' value='" + row.SchemeID + "'>"//added by prama
                             + "<input type='hidden' class='IsGSTRegistered' value='" + row.IsGSTRegistered + "'>";
                        }
                    },
                    { "data": "Code", "className": "Code" },
                    { "data": "Name", "className": "Name" },
                    { "data": "Address", "className": "Address" },
                    { "data": "CustomerCategory", "className": "CustomerCategory" },
                    {
                        "data": null,
                        "className": "uk-text-center",
                        "searchable": false,
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            return row.IsGSTRegistered == 1 ? "Yes" : "No";
                        }
                    },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#LocationID', function () {
                list_table.fnDraw();
            });
            $list.on('previous.page', function () {
                list_table.api().page('previous').draw('page');
            });
            $list.on('next.page', function () {
                list_table.api().page('next').draw('page');
            });
            list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('textchange', function (e) {
                    e.preventDefault();
                    var index = $(this).parent().parent().index();
                    list_table.api().column(index).search(this.value).draw();
                });
            });
            return list_table;
        }
    },

    list: function () {
        $list = $('#TurnOverDiscount-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });

            $('#TurnOverDiscount-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/TurnOverDiscount/Details/' + Id;
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
        var self = TurnOverDiscounts;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("#btnOKCustomer").on("click", self.select_customer);
        UIkit.uploadSelect($("#select-file"), self.upload_file);
        $("body").on("click", "#btnAdd", self.add_item);
        $("body").on("click", ".remove-item", self.delete_item);
        $("body").on("ifChanged", ".check-box", self.check);
        $(".btnSave").on("click", self.save);
        $(".templates").on("click", self.download_template);
    },

    select_customer: function () {
        var self = TurnOverDiscounts;
        var radio = $('#select-customer tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var StateID = $(row).find(".StateID").val();
        var IsGSTRegistered = $(row).find(".IsGSTRegistered").val();
        var PriceListID = $(row).find(".PriceListID").val();
        var SchemeID = $(row).find(".SchemeID").val();
        $("#CustomerName").val(Name);
        $("#Code").val(Code);
        $("#CustomerID").val(ID);
        $("#StateID").val(StateID);
        $("#PriceListID").val(PriceListID);
        $("#SchemeID").val(SchemeID);
        $("#IsGSTRegistred").val(IsGSTRegistered.toLowerCase());
        UIkit.modal($('#select-customer')).hide();
    },

    get_customers: function (release) {
        var self = TurnOverDiscounts;

        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
                CustomerCategoryID: $('#CustomerCategoryID').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    set_customer: function (event, item) {
        var self = TurnOverDiscounts;
        $("#CustomerName").val(item.Name);
        $("#CustomerID").val(item.id);
        $("#Code").val(item.code);
        $("#IsGSTRegistered").val(item.isGstRegistered);
        $("#PriceListID").val(item.priceListId);
        $("#SchemeID").val(item.schemeId);
        $("#DespatchDate").focus();
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
            var self = TurnOverDiscounts;
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
                self.populate_uploaded_turnoverdiscounts();
            }
            if (failure != "") {
                app.show_error(failure);
            }
        }
    },

    populate_uploaded_turnoverdiscounts: function () {
        var self = TurnOverDiscounts;
        var file_path = $('#selected-file a').data('path');
        $.ajax({
            url: '/Masters/TurnOverDiscount/ReadExcel/',
            data: { Path: file_path },
            dataType: "json",
            type: "POST",
            success: function (response) {
                var html = "";
                var $html;
                if (response.Status == "success") {
                    $.each(response.Data, function (i, record) {
                        if (record.TurnOverDiscount > 0) {
                            var slno = (i + 1);
                            html += '<tr class="included">'
                                    + '<td class="uk-text-center">' + slno + '</td>'
                                    + '<td class=" td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box"/>' + '</td>'
                                    + '<td class="Code">' + record.Code + '</td>'
                                    + '<td class="CustomerName">' + record.CustomerName + '</td>'
                                    + '<td>' + '<input type="text" value=" ' + record.TurnOverDiscount + '" class="md-input  mask-currency TurnOverDiscount uk-text-right"  />' + '</td>'
                                    + '<td class="FromDate">' + record.FromDate + '</td>'
                                    + '<td class="ToDate">' + record.ToDate + '</td>'
                                    + '<td class="Month">' + record.Month + '</td>'
                                    + '<td class="Location">' + record.Location + '</td>'
                                    + "</tr>";
                        }
                    });
                }
                else {
                    app.show_error(response.Message);
                }
                if (html != "") {
                    $html = $(html);
                    app.format($html);
                    $("#TurnOverDiscounts tbody").html($html);
                }

                self.clear_data();
                fh.resizeHeader();
            }


        });
    },

    add_item: function () {
        var self = TurnOverDiscounts;
        if (self.validate_on_add() > 0) {
            return;
        }
        sino = $('#TurnOverDiscounts tbody tr ').length + 1;
        var Location = $("#LocationID option:selected").text();
        var Code = $("#Code").val();
        var CustomerName = $("#CustomerName").val();
        var TurnOverDiscount = $("#Amount").val();
        var FromDate = $("#FromDate").val();
        var ToDate = $("#ToDate").val();
        var ToDate = $("#ToDate").val();
        var Month = $("#MonthID option:selected").text();
        var content = "";
        var $content;
        content = '<tr class="included">'
            + '<td class="serial-no uk-text-center">' + sino + '</td>'
            + '<td class=" td-check uk-text-center">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box"/>' + '</td>'
            + '<td class="Code" >' + Code + '</td>'
            + '<td class="CustomerName" >' + CustomerName + '</td>'
            + '<td  >' + '<input type="text" value=" ' + TurnOverDiscount + '" class="md-input uk-text-right TurnOverDiscount "  /> ' + '</td>'
            + '<td class="FromDate" >' + FromDate + '</td>'
            + '<td class="ToDate" >' + ToDate + '</td>'
            + '<td class="Month" >' + Month + '</td>'
            + '<td class="Location">' + Location
            + '<input type="hidden" class = "LocationID" value="' + LocationID + '" />'
            + '</td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#TurnOverDiscounts tbody').append($content);
        self.count_items();
        fh.resizeHeader();
        self.clear_data();
    },

    clear_data: function () {
        var self = TurnOverDiscounts;
        $("#LocationID").val('');
        $("#CustomerName").val('');
        $("#Amount").val('');
        $("#ToDate").val('');
        $("#selected-file").val('');

    },

    delete_item: function () {
        var self = TurnOverDiscounts;
        $(this).closest('tr').remove();
        var sino = 0;
        $('#TurnOverDiscounts tbody tr .serial-no ').each(function () {
            sino = sino + 1;
            $(this).text(sino);
        });
        $("#item-count").val($("#TurnOverDiscounts tbody tr").length);

    },

    save: function () {
        var self = TurnOverDiscounts;
        var data;
        index = $("#TurnOverDiscounts tbody .included").length;
        $("#item-count").val(index);
        self.error_count = 0;
        self.error_count = self.validate_on_save();
        if (self.error_count > 0) {
            return;
        }
        data = self.get_data();
        $.ajax({
            url: '/Masters/TurnOverDiscount/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/TurnOverDiscount/Index";
                }
                else {
                    app.show_error('Failed to create TurnOverDiscount');
                }
            }
        });
    },

    get_data: function () {
        var self = TurnOverDiscounts;
        var data = {};
        data.ID = $("#ID").val(),
        data.Date = $("#Date").val(),
        data.Items = [];
        var item = {};
        $('#TurnOverDiscounts tbody tr.included').each(function () {
            item = {};
            item.Code = $(this).find(".Code").text().trim();
            item.CustomerName = $(this).find(".CustomerName").text();
            item.TurnOverDiscount = clean($(this).find(".TurnOverDiscount").val());
            item.FromDate = $(this).find(".FromDate").text();
            item.ToDate = $(this).find(".ToDate").text();
            item.Location = $(this).find(".Location").text();
            item.Month = $(this).find(".Month").text();
            data.Items.push(item);
        });
        return data;
    },

    check: function () {
        var self = TurnOverDiscounts;
        $("#TurnOverDiscounts tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
                $(row).find(".TurnOverDiscount").prop("disabled", false);

            } else {
                $(row).find(".TurnOverDiscount").prop("disabled", true);
                $(row).removeClass('included');
            }
        });
        self.count_items();
    },

    count_items: function () {
        var count = $('#TurnOverDiscount tbody').find('input.check-box:checked').length;
        $('#item-count').val(count);
    },

    download_template: function () {
        var self = TurnOverDiscounts;
        self.error_count = 0;
        self.error_count = self.validate_on_upload();
        if (self.error_count > 0) {
            return;
        }
        var CustomerLocationID = $('#LocationID option:selected').val();
        var Month = $('#MonthID option:selected').text();
        var FromDate = $('#FromDate').val();
        var ToDate = $('#ToDate').val();
        if (CustomerLocationID == "") {
            CustomerLocationID = 0;
        }
        else {
            CustomerLocationID = $('#LocationID option:selected').val();
        }

        window.location = "/Reports/Sales/GetItemForTurnOverDiscount/?CustomerLocationID=" + CustomerLocationID + "&Month=" + Month + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
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
        ],
        on_add: [
            {
                elements: "#CustomerName",
                rules: [
                    { type: form.required, message: "Please choose an Customer" },
                    { type: form.non_zero, message: "Please choose an Customer" },
                ],
            },
             {
                 elements: "#Amount",
                 rules: [
                     { type: form.required, message: "Please choose an Amount" },
                     { type: form.non_zero, message: "Please choose an Amount" },
                 ],
             },
              {
                  elements: "#ToDate",
                  rules: [
                      { type: form.required, message: "Please choose ToDate" },
                      { type: form.non_zero, message: "Please choose ToDate" },
                  ],
              },
                {
                    elements: "#MonthID",
                    rules: [
                        { type: form.required, message: "Please Select Month" },
                        { type: form.non_zero, message: "Please Select Month" },
                    ],
                },
        ],
        on_upload: [
          {
              elements: "#MonthID",
              rules: [
                  { type: form.required, message: "Please Select Month" },
                  { type: form.non_zero, message: "Please Select Month" },
              ],
          },
          {
              elements: "#ToDate",
              rules: [
                  { type: form.required, message: "Please choose ToDate" },
                  { type: form.non_zero, message: "Please choose ToDate" },
              ],
          },
        ],
    },

    validate_on_save: function () {
        var self = TurnOverDiscounts;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    validate_on_add: function () {
        var self = TurnOverDiscounts;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    validate_on_upload: function () {
        var self = TurnOverDiscounts;
        if (self.rules.on_upload.length) {
            return form.validate(self.rules.on_upload);
        }
        return 0;
    },

}