$(function () {
    var self = Bin;
    Bin.Bin_list();
    Bin.bind_events();
    var image;
    var Path;
   
});


 
    var Bin = {
        Bin_list: function () {
            $Bin_list = $('#Bin-list');
            if ($Bin_list.length) {
                $Bin_list.find('thead.search th').each(function () {
                    var title = $(this).text().trim();
                    $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
                });
                $('#Bin-list tbody tr').on('click', function () {
                    var Id = $(this).find("td:eq(0) .ID").val();
                    window.location = '/Masters/BinMaster/Details/' + Id;
                });
                altair_md.inputs();
                var Bin_list_table = $Bin_list.dataTable({
                    "bLengthChange": false,
                    "bFilter": true
                });
                Bin_list_table.api().columns().each(function (g, h) {
                    $('thead.search input').on('keyup change', function () {
                        var index = $(this).parent().parent().index();
                        Bin_list_table.api().column(index).search(this.value).draw();
                    });
                });
            }
        },
        bind_events: function () {

            $(".btnUpdate").on('click', Bin.update);
            $(".btnSave").on('click', Bin.save_confirm);
            $("body").on("click", "#btnOKItem", Bin.select_item);
            $("input[type='file'][name='files']").on("change", Bin.update_file_list);
        },

    update_file_list: function () {
        var self = Bin;
        var fileInput = $("input[type='file'][name='files']")[0];
        var files = fileInput.files;

        // Check if any files are selected
        if (files.length === 0) {
            alert("Please select a file to upload.");
            return;
        }

        // Create a FormData object to hold the selected file
        var formData = new FormData();
        formData.append("file", files[0]); // Assuming a single file for upload

        // AJAX call to the controller
        $.ajax({
            url: '/Bin/UploadFile', // Replace with your controller action URL
            type: 'POST',
            data: formData,
            processData: false, // Prevent jQuery from processing the data
            contentType: false, // Prevent jQuery from setting the content type
            success: function (response) {
                if (response.success) {
                    alert("File uploaded successfully!");
                    console.log("Uploaded File URL:", response.fileUrl);
                    self.image = response.image;
                    self.Path = response.Path;
                    $("#Path").val(response.Path);

                    $("#image").val(response.image);
                } else {
                    alert("File upload failed: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("An error occurred while uploading the file: " + error);
            }
        });
    },

    save_confirm: function () {
        var self = Bin;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },

    error_count: 0,
    save: function () {
        var self = Bin;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/BinMaster/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Bin created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/BinMaster/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    validate_form: function () {
        var self = Bin;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#BinId",
                rules: [
                    { type: form.required, message: "BinId is required" },
                ]
            },
            {
                elements: "#BinCode",
                rules: [
                    { type: form.required, message: "Code is required" },
                ]
            },
            {
                elements: "#WareHouseID",
                rules: [
                    { type: form.required, message: "WarehouseName is required" },
                ]
            },
           

        ]
    },


    get_data: function () {
        var model = {
            ID: $("#ID").val(),
           BinCode: $("#BinCode").val(),
            WareHouseID: $("#WareHouseID").val(),
            //Path: $("#NegotiationFiles").val(),
           


        }
        return model;
        },
        update: function () {
            var self = Bin;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count > 0) {
                return;
            }
            $('.btnUpdate').css({ 'visibility': 'hidden' });
            var modal = self.get_data();
            $.ajax({
                url: '/Masters/BinMaster/Edit',
                data: modal,
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Bin updated successfully");
                        setTimeout(function () {
                            window.location = "/Masters/BinMaster/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to update data.");
                        $('.btnUpdate').css({ 'visibility': 'visible' });
                    }
                },
            });
        },

};

