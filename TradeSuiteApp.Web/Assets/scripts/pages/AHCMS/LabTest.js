LabTest = {
    init: function () {
        var self = LabTest;
        self.bind_events();
    },

    list: function () {
        var $list = $('#LabTest-list');

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/LabTest/GetLabTestList"

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
                               + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                           + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                           + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                           + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "Date", "className": "Date" },
                   { "data": "PatientCode", "className": "PatientCode" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "TestName", "className": "TestName" },
                   { "data": "Doctor", "className": "Doctor" },

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var AppointmentProcessID = $(this).closest("tr").find("td .AppointmentProcessID").val();
                        app.load_content("/AHCMS/LabTest/Create/?ID=" + AppointmentProcessID + "&PatientID=" + PatientID);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    bind_events: function(){
        var self = LabTest;
        $("body").on('ifChanged', '.chkCheck', self.include_labtest);
        $("body").on("click", ".btnSave", self.save_confirm);
        $("body").on("click", ".SaveAndPrint", self.save_and_print);
        $("body").on("click", ".print", self.printpdf);
    },

    get_data: function () {
        var self = LabTest;
        var data = {};
        data.Items = [];
        var item = {};
        $('#labtest_list tbody tr.included').each(function () {
            item = {};
            item.Date = $("#Date").val();
            item.ID = $(this).find(".ID").val();
            item.ItemID = $(this).find(".ItemID").val();
            item.ObserveValue = $(this).find(".ObserveValue").val();
            item.Status = $(this).find(".Status").val();
            data.Items.push(item);
        });
        return data;
    },

    save: function (IsPrint) {
        var self = LabTest;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/LabTest/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    if (IsPrint == false)
                    {
                        window.location = "/AHCMS/LabTest/Index";
                    }
                    
                }
                else {
                    app.show_error('Failed to create Lab Test');
                }
            }
        });
    },

    include_labtest: function () {
        var self = LabTest
        var row = $(this).closest('tr');
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.ObserveValue').addClass('included').removeAttr('disabled');
            $(this).closest('tr').find('.Status').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('.ObserveValue').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').find('.Status').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
        }
    },
    save_and_print: function () {
        var self = LabTest;
        app.confirm_cancel("Do you want to Save", function () {
            self.save(true);
            self.printpdf();
        }, function () {
        })

    },

    save_confirm: function () {
        var self = LabTest;
        //self.error_count = 0;
        //self.error_count = self.validate_save();
        //if (self.error_count > 0) {
        //    return;
        //}
        app.confirm_cancel("Do you want to Save", function () {
            self.save(false);
        }, function () {
        })
    },

    printpdf: function () {
        var self = LabTest;
        $.ajax({
            url: '/Reports/AHCMS/LabTestResultPrintPDF',
            data: {
                id: $("#AppointmentProcessID").val(),
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
    }
}