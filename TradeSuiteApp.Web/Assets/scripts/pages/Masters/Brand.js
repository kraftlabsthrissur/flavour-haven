$(function () {
    var self = Brand;
    Brand.Brand_list();
    Brand.bind_events();
    var image;
    var Path;
   
});


 
    var Brand = {
        Brand_list: function () {
            $Brand_list = $('#Brand-list');
            if ($Brand_list.length) {
                $Brand_list.find('thead.search th').each(function () {
                    var title = $(this).text().trim();
                    $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
                });
                $('#Brand-list tbody tr').on('click', function () {
                    var Id = $(this).find("td:eq(0) .ID").val();
                    window.location = '/Masters/Brand/Details/' + Id;
                });
                altair_md.inputs();
                var Brand_list_table = $Brand_list.dataTable({
                    "bLengthChange": false,
                    "bFilter": true
                });
                Brand_list_table.api().columns().each(function (g, h) {
                    $('thead.search input').on('keyup change', function () {
                        var index = $(this).parent().parent().index();
                        Brand_list_table.api().column(index).search(this.value).draw();
                    });
                });
            }
        },
        bind_events: function () {

            $(".btnUpdate").on('click', Brand.update);
            $(".btnSave").on('click', Brand.save_confirm);
            $("body").on("click", "#btnOKItem", Brand.select_item);
            $("input[type='file'][name='files']").on("change", Brand.update_file_list);
        },

    update_file_list: function () {
        var self = Brand;
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
            url: '/Brand/UploadFile', // Replace with your controller action URL
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
        var self = Brand;
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
        var self = Brand;
        $('.btnSave').css({ 'visibility': 'hidden' });
        var modal = self.get_data();
        $.ajax({
            url: '/Masters/Brand/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "success") {
                    app.show_notice("Brand created successfully");
                    setTimeout(function () {
                        window.location = "/Masters/Brand/Index";
                    }, 1000);
                } else {
                    app.show_error("Failed to create data.");
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    validate_form: function () {
        var self = Brand;
        if (self.rules.on_submit.length) {
            return form.validate(self.rules.on_submit);
        }
        return 0;
    },
    error_count: 0,
    rules: {
        on_submit: [
            {
                elements: "#BrandId",
                rules: [
                    { type: form.required, message: "BrandId is required" },
                ]
            },
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Code is required" },
                ]
            },
            {
                elements: "#BrandName",
                rules: [
                    { type: form.required, message: "BrandName is required" },
                ]
            },
           

        ]
    },


    get_data: function () {
        var model = {
            Id: clean($("#Id").val()),
            BrandId: clean($("#BrandId").val()),
            Code: $("#Code").val(),
            BrandName: $("#BrandName").val(),
            //Path: $("#NegotiationFiles").val(),
            Path: $("#Path").val(),
          
            image: $("#image").val()


        }
        return model;
        },
        update: function () {
            var self = Brand;
            self.error_count = 0;
            self.error_count = self.validate_form();
            if (self.error_count > 0) {
                return;
            }
            $('.btnUpdate').css({ 'visibility': 'hidden' });
            var modal = self.get_data();
            $.ajax({
                url: '/Masters/Brand/Edit',
                data: modal,
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Brand updated successfully");
                        setTimeout(function () {
                            window.location = "/Masters/Brand/Index";
                        }, 1000);
                    } else {
                        app.show_error("Failed to update data.");
                        $('.btnUpdate').css({ 'visibility': 'visible' });
                    }
                },
            });
        },

};

