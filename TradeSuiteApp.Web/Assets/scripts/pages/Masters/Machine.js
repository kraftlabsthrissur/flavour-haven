machine = {
    init: function () {
        var self = machine;
        self.bind_events();
    },

    list: function () {
        $machine_list = $('#machine-list');
        if ($machine_list.length) {
            $machine_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#machine-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Machine/Details/' + Id;
            });
            altair_md.inputs();
            var machine_list_table = $machine_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            machine_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    machine_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    all_machine_list: function () {

        var $list = $('#machine-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var url = "/Masters/Machine/GetAllMachineList";

            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": true,
                "aaSorting": [[3, "asc"]],
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ProcessID", Value: $('#ddlProductionProcess').val() },
                        ];
                    }
                },
                "aoColumns": [
                    {
                        "data": null,
                        "className": "uk-text-center ",
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
                            return "<input type='radio' class='uk-radio MachineID' name='MachineID' data-md-icheck value='" + row.ID + "' >"
                        }
                    },
                    { "data": "MachineCode", "className": "Code" },
                    { "data": "MachineName", "className": "Name" },
                    { "data": "LoadedMould", "className": "LoadedMould" }
                ],
                createdRow: function (row, data, index) {
                    $(row).addClass(data.Status);
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.trigger("datatable.changed");
                },
            });
            $("body").on('change', '#ddlProductionProcess', function () {
                list_table.fnDraw();
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
            return list_table;
        }
    },
    bind_events: function () {
        $(".btnSave").on('click', machine.save);
    },
    save: function () {
        var self = machine;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Machine/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Machine created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Machine/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },
    get_data : function()
    {
        var modal = {
            ID: $("#ID").val(),
            MachineCode: $('#MachineCode').val(),
            InsulationDate: $('#InsulationDate').val(),
            TypeID: $("#Type").val(),
            Model: $("#Model").val(),
            LocationID: $("#LocationID").val(),
            MachineName: $("#MachineName").val(),
            ProcessID: $("#ProcessID").val(),
            Motor: $("#Motor").val(),
            PowerConsumptionPerHour: $("#PowerConsumptionPerHour").val(),
            SoftwareVersion: $("#SoftwareVersion").val(),
            MachineNumber: $("#MachineNumber").val(),
            Manufacturer: $("#Manufacturer").val(),
            OperatorCount: $("#OperatorCount").val(),
            HelperCount : $("#HelperCount").val(),
            MaintenancePeriod: $("#MaintenancePeriod").val(),
            AverageCostPerHour: $("#AverageCostPerHour").val(),
        }
        return modal;
    },
    validate_form: function () {
        var self = machine;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
    },
    rules: {
        on_submit: [
            {
                elements: "#MachineCode",
                rules: [
                   { type: form.required, message: "MachineCode  is Required" },
                ]
            },

             {
                 elements: "#InceptionDate",
                 rules: [
                    { type: form.required, message: "InceptionDate  is Required" },
                 ]
             },
             {
                 elements: "#Type",
                 rules: [
                    { type: form.required, message: "InceptionDate  is Required" },
                 ]
             },
             {
                 elements: "#MachineName",
                 rules: [
                    { type: form.required, message: "MachineName  is Required" },
                 ]
             },
                {
                    elements: "#MaintenancePeriod",
                    rules: [
                        { type: form.required, message: "MaintenancePeriod  is Required" },
                    ]
                },
                    {
                        elements: "#AverageCostPerHour",
                    rules: [
                        { type: form.required, message: "AverageCostPerHour  is Required" },
                    ]
                },
                {
                    elements: "#AverageEnergyConsuptionPerHour",
                    rules: [
                        { type: form.required, message: "AverageEnergyConsuptionPerHour  is Required" },
                    ]
                },
        ]
    },
   
    
}