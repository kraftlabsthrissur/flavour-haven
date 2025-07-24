Discharge = {
    init: function () {
        var self = Discharge;
        self.bind_events();
        self.get_discharge_medicine_list();
    },

    list: function () {
        var self = Discharge;
        $('#tabs-discharge-summary').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "DischargeAdvicedPatients":
                $list = $('#adviced-inpatient-list');
                break;
            case "DischargedPatients":
                $list = $('#discharged-inpatient-list');
                break;
            default:
                $list = $('#adviced-inpatient-list');
        }

        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/Discharge/GetDischargeAdvicedInpatientList"

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
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
                               + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
                               + "<input type='hidden' class='DischargeSummaryID' value='" + row.DischargeSummaryID + "'>"
                               + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                               + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                       }
                   },
                   { "data": "AdmissionDate", "className": "AdmissionDate" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "RoomName", "className": "RoomName" },
                   { "data": "Doctor", "className": "Doctor" },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnDischarge' >DischargeSummary</button>";
                         }
                     },
                     {
                         "data": "", "className": "action uk-text-center", "searchable": false,
                         "render": function (data, type, row, meta) {
                             return "<button class='md-btn md-btn-primary btnBilling' >Billing</button>";
                         }
                     },
                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },


    bind_events: function () {
        var self = Discharge;
        $("body").on('click', '.btnDischarge', self.view_discharge_details);
        $("body").on("click", ".btnPrint", self.discharge_summary_printpdf);
        $("body").on('click', '.btnDischarged', self.on_discharge);
       $("body").on('click', '.btnBilling', self.on_billing);
    },

    on_discharge: function () {
        var self = Discharge;
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/Discharge/IsBillPaid",
            data: {
                IPID: IPID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if(response.IsBillPaid==false)
                {
                    app.confirm_cancel("Pending Bills Exist and Do you want to Discharge", function () {
                        self.Discharged();
                    }, function () {
                    })
                }
                else {
                    app.confirm_cancel("Do you want to Discharge", function () {
                        self.Discharged();
                    }, function () {
                    })
                }
            }
        });
        
    },

    Discharged: function () {
        var self = Discharge;
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/Discharge/Discharge",
            data: {
                IPID: IPID
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                        window.location = "/AHCMS/Discharge/Index";
                }
                else {
                    app.show_error('Failed to Discharge');
                }
            }
            
        });

    },
    on_billing: function () {
        //e.stopPropagation();
        var self = Discharge;
        var IPID = $(this).closest('tr').find('.IPID').val();
        var PatientID = $(this).closest('tr').find('.PatientID').val();
        app.load_content("/Sales/ServiceSalesOrder/Create/?IPID=" + IPID);
    },

    get_discharge_medicine_list: function () {
        var self = Discharge;
        var IPID = $('#IPID').val();
        $.ajax({
            url: "/AHCMS/Discharge/MedicineList",
            data: {
                IPID: IPID
            },
            dataType: "html",
            type: "POST",
            success: function (response) {
                $("#medicine-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#medicine-list").append($response);
            }
        });
    },

    view_discharge_details: function () {
        var self = Discharge;
        var IPID = $(this).closest('tr').find('.IPID').val();
        app.load_content("/AHCMS/Discharge/Create/?IPID=" + IPID);
    },

    discharge_summary_printpdf: function () {
        var self = Discharge;
        $.ajax({
            url: '/Reports/AHCMS/DischargeSummaryPrintPdf',
            data: {
                IPID: $("#IPID").val()
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
}