
Fleet = {
    init: function () {
        var self = Fleet;
        self.bind_events();
    },

    list: function () {
        $fleet_list = $('#fleet-list');
        if ($fleet_list.length) {
            $fleet_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#fleet-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
                window.location = '/Masters/Fleet/Details/' + Id;
            });
            altair_md.inputs();
            var fleet_list_table = $fleet_list.dataTable({
                "bLengthChange": false,
                "bFilter": true
            });
            fleet_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    fleet_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        $(".btnSave").on('click', Fleet.save);
        $(".btnUpdate").on('click', Fleet.update);
    },

    update: function () {
        var self = Fleet;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnUpdate').css({ 'visibility': 'hidden' })
        var modal = slf.get_data();
        $.ajax({
            url: '/Masters/Fleet/Edit',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Fleet updated successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Fleet/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to update data.");
                    $('.btnUpdate').css({ 'visibility': 'visible' });
                }
            },
        })
    },

    save: function () {
        var self = Fleet;
        self.errorcount = 0;
        self.errorcount = self.validate_form();
        if (self.errorcount > 0) {
            return;
        }
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Fleet/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("Fleet created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Fleet/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var model = {
            Id: $("#ID").val(),
            vehicleNo: $("#VehicleNo").val(),
            vehicleName: $("#VehicleName").val(),
            driverName: $("#DriverName").val(),
            licenseNo: $("#LicenseNo").val(),
            policyNo: $("#PolicyNo").val(),
            ownerName: $("#OwnerName").val(),
            travellingAgency: $("#TravellingAgency").val(),
            insuranceCompany: $("#InsuranceCompany").val(),
            otherDetails: $("#OtherDetails").val(),
            purchaseDate: $("#PurchaseDate").val(),
            permitExpairyDate: $("#PermitExpairyDate").val(),
            taxExpairyDate: $("#TaxExpairyDate").val(),
            testExpairyDate: $("#TestExpairyDate").val(),
            insuranceExpairyDate: $("#InsuranceExpairyDate").val(),
            bagCapacity: $("#BagCapacity").val(),
            boxCapacity: $("#BoxCapacity").val(),
            canCapacity: $("#CanCapacity").val(),
        }
        return model;
    },

    validate_form: function () {
        var self = Fleet;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },

    rules: {
        on_submit: [
             {
                 elements: "#VehicleNo",
                 rules: [
                    { type: form.required, message: "Vehicle Number is Required" },
                 ]
             },
             {
                 elements: "#VehicleName",
                 rules: [
                    { type: form.required, message: "Vehicle Name is Required" },
                 ]
             },
             {
                 elements: "#DriverName",
                 rules: [
                    { type: form.required, message: "Driver Name is Required" },
                 ]
             },
             {
                 elements: "#LicenseNo",
                 rules: [
                    { type: form.required, message: "License Numberis Required" },
                 ]
             },
             {
                 elements: "#PolicyNo",
                 rules: [
                    { type: form.required, message: "Policy Number is Required" },
                 ]
             },
             {
                 elements: "#OwnerName",
                 rules: [
                    { type: form.required, message: "Owner Name is Required" },
                 ]
             },
             {
                 elements: "#TravellingAgency",
                 rules: [
                    { type: form.required, message: "TravellingAgency Name is Required" },
                 ]
             },
             {
                 elements: "#BagCapacity",
                 rules: [
                    { type: form.required, message: "BagCapacity  is Required" },
                 ]
             },
             {
                 elements: "#BoxCapacity",
                 rules: [
                    { type: form.required, message: "Box Capacity  is Required" },
                 ]
             },
             {
                 elements: "#CanCapacity",
                 rules: [
                    { type: form.required, message: "Can Capacity  is Required" },
                 ]
             },
        ]
    }
}