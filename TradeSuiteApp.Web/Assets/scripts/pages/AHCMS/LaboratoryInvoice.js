LaboratoryInvoice = {
    init: function () {
        var self = LaboratoryInvoice;
        //InternationalPatient.AppoinmentPatientList();
        //$('#appoinment-scheduled-patient-list').SelectTable({
        //    selectFunction: self.select_patient,
        //    modal: "#select-appoinment-scheduled-patient",
        //    initiatingElement: "#PartyName"
        //});

        labtest_list = PatientDiagnosis.lablist();
        item_select_table = $('labtest-list').SelectTable({
            selectFunction: self.select_labtest,
            returnFocus: "#CustomerName",
            modal: "#select-labtest",
            initiatingElement: "#CustomerName"
        });
       // self.lablist();

        self.on_change_patient_type();
        var Type = 'LabInvoice' + $("#PatientTypeID option:selected").text();
        Customer.directsales_customer_list(Type);
        $('#direct-customer-list').SelectTable({
            selectFunction: self.select_patient,
            returnFocus: "#LabTest",
            modal: "#select-direct-customer",
            initiatingElement: "#CustomerName",
            startFocusIndex: 3
        });
        $("#supplier").hide();
        self.bind_events();
        self.get_bank_name();
    },
    rowindex: [],

    list: function () {
        var self = LaboratoryInvoice;
        $('#tabs-labouratoryinvoice').on('change.uk.tab', function (event, active_item, previous_item) {
            if (!active_item.data('tab-loaded')) {
                self.tabbed_list(active_item.data('tab'));
                active_item.data('tab-loaded', true);
            }
        });
    },
    tabbed_list: function (type) {

        var $list;

        switch (type) {
            case "ToBeInvoiced":
                $list = $('#to-be-invoiced-list');
                break;
            case "Invoiced":
                $list = $('#invoiced-list');
                break;
            default:
                $list = $('#to-be-invoiced-list');
        }
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs($list);

            var url = "/AHCMS/LaboratoryInvoice/GetLabTestList?type=" + type;
            var list_table = $list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15,
                "bAutoWidth": false,
                "bServerSide": true,
                "bSortable": false,
                "aaSorting": [[2, "desc"]],
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
                   + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
                   + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
                   + "<input type='hidden' class='PatientLabTestMasterID' value='" + row.PatientLabTestMasterID + "'>"
                   + "<input type='hidden' class='InvoiceID' value='" + row.InvoiceID + "'>"
                   + "<input type='hidden' class='ID' value='" + row.ID + "'>";

                       }
                   },
                   { "data": "TransNo", "className": "TransNo" },
                   { "data": "Date", "className": "Date" },
                   { "data": "PatientCode", "className": "PatientCode" },
                   { "data": "Patient", "className": "Patient" },
                   { "data": "TestName", "className": "TestName" },
                   { "data": "Doctor", "className": "Doctor" }

                ],
                "createdRow": function (row, data, index) {
                    app.format(row);
                },
                "drawCallback": function () {
                    $list.find('tbody td:not(.action)').on('click', function () {
                        var AppointmentProcessID = $(this).closest("tr").find("td .AppointmentProcessID").val();
                        var PatientLabTestMasterID = $(this).closest("tr").find("td .PatientLabTestMasterID").val();
                        var IpID = $(this).closest("tr").find("td .IPID").val();
                        var PatientID = $(this).closest("tr").find("td .PatientID").val();
                        var InvoiceID = $(this).closest("tr").find("td .InvoiceID").val();
                        app.load_content("/AHCMS/LaboratoryInvoice/Create/?ID=" + AppointmentProcessID + "&PatientLabTestMasterID=" + PatientLabTestMasterID + "&IPID=" + IpID + "&PatientID=" + PatientID + "&InvoiceID=" + InvoiceID);
                    });
                },
            });

            $list.find('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        }
    },

    //list: function () {
    //    var $list = $('#LabTest-list');

    //    if ($list.length) {
    //        $list.find('thead.search th').each(function () {
    //            var title = $(this).text().trim();
    //            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
    //        });
    //        altair_md.inputs($list);

    //        var url = "/AHCMS/LaboratoryInvoice/GetLabTestList"

    //        var list_table = $list.dataTable({
    //            "bLengthChange": false,
    //            "bFilter": true,
    //            "pageLength": 15,
    //            "bAutoWidth": false,
    //            "bServerSide": true,
    //            "bSortable": false,
    //            "aaSorting": [[1, "desc"]],
    //            "ajax": {
    //                "url": url,
    //                "type": "POST",
    //                "data": function (data) {
    //                }
    //            },
    //            "aoColumns": [
    //               {
    //                   "data": null,
    //                   "className": "uk-text-center",
    //                   "searchable": false,
    //                   "orderable": false,
    //                   "render": function (data, type, row, meta) {
    //                       return (meta.settings.oAjaxData.start + meta.row + 1)
    //                           + "<input type='hidden' class='AppointmentProcessID' value='" + row.AppointmentProcessID + "'>"
    //                       + "<input type='hidden' class='PatientID' value='" + row.PatientID + "'>"
    //                       + "<input type='hidden' class='IPID' value='" + row.IPID + "'>"
    //                       + "<input type='hidden' class='PatientLabTestMasterID' value='" + row.PatientLabTestMasterID + "'>";
    //                       + "<input type='hidden' class='ID' value='" + row.ID + "'>";

    //                   }
    //               },
    //               { "data": "Date", "className": "Date" },
    //               { "data": "PatientCode", "className": "PatientCode" },
    //               { "data": "Patient", "className": "Patient" },
    //               { "data": "TestName", "className": "TestName" },
    //               { "data": "Doctor", "className": "Doctor" }

    //            ],
    //            "createdRow": function (row, data, index) {
    //                app.format(row);
    //            },
    //            "drawCallback": function () {
    //                $list.find('tbody td:not(.action)').on('click', function () {
    //                    var AppointmentProcessID = $(this).closest("tr").find("td .AppointmentProcessID").val();
    //                    var PatientLabTestMasterID = $(this).closest("tr").find("td .PatientLabTestMasterID").val();
    //                    var IpID = $(this).closest("tr").find("td .IPID").val();
    //                    app.load_content("/AHCMS/LaboratoryInvoice/Create/?ID=" + AppointmentProcessID + "&PatientLabTestMasterID=" + PatientLabTestMasterID + "&IPID=" + IpID);
    //                });
    //            },
    //        });

    //        $list.find('thead.search input').on('keyup change', function () {
    //            var index = $(this).parent().parent().index();
    //            list_table.api().column(index).search(this.value).draw();
    //        });
    //    }
    //},
    select_patient: function () {
        var self = LaboratoryInvoice;
        var radio = $('#direct-customer-list tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var ProcessID = $(row).find(".OPID").val();
        var Age = $(row).find(".Age").val();
        var Gender = $(row).find(".Gender").val();
        var Doctor = $(row).find(".DoctorName").val();
        var Mobile = $(row).find(".MobileNo").val();
        var ipid = $(row).find(".IPID").val();
        var AppointmentProcessID = $(row).find(".AppointmentProcessID").val();
        if (($("#labtest_list tbody tr").length > 0) && (ID != $("#PatientID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#labtest_list tbody').empty();
                $("#Patient").val(Name);
                $("#PatientID").val(ID);
                $("#Age").val(Age);
                $("#Sex").val(Gender);
                $("#Doctor").val(Doctor);
                $("#Mobile").val(Mobile);
                $("#IPID").val(ipid);
                $("#AppointmentProcessID").val(AppointmentProcessID);
                $("#NetAmount").val(0);
            })
        }
        else {
            $("#Patient").val(Name);
            $("#PatientID").val(ID);
            $("#AppointmentProcessID").val(ProcessID);
            $("#Age").val(Age);
            $("#Sex").val(Gender);
            $("#Doctor").val(Doctor);
            $("#Mobile").val(Mobile);
            $("#IPID").val(ipid);
            $("#AppointmentProcessID").val(AppointmentProcessID);
        }
        UIkit.modal($('#select-appoinment-scheduled-patient')).hide();
    },
    clear_grid: function (ID) {
        var self = LaboratoryInvoice;
        if (($("#labtest_list tbody tr").length > 0) && (ID != $("#PatientID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#labtest_list tbody').empty();
            })
        }
    },
    bind_events: function () {
        var self = LaboratoryInvoice;
        $.UIkit.autocomplete($('#customer-autocomplete'), { 'source': self.get_customers, 'minLength': 1 });
        $('#customer-autocomplete').on('selectitem.uk.autocomplete', self.set_customer);
        $("body").on('ifChanged', '.chkCheck', self.include_labtest);
        $("body").on("click", ".btnSave", self.save_confirm);
        $("body").on("click", ".btnSaveInv", self.save_invoice_confirm);
        //$("body").on("click", ".print", self.printpdf);
        $('body').on('click', '#labtest_list tbody tr', '.new-item', self.show_popup);
        $('#labtest_list tbody tr').on("click", self.show_popup);
        $('body').on('click', "#Type", self.show_supplier);
        $("body").on("click", "#btnOk", self.get_labtest_type);
        //$("body").on("click", ".btnSave", self.save_confirm);
        $("#btnOKCustomer").on("click", self.select_patient);
        $.UIkit.autocomplete($('#labtest-autocomplete'), Config.get_labitems);
        $('#labtest-autocomplete').on('selectitem.uk.autocomplete', self.set_lab_test);
        $("body").on("click", "#btn_add_lab_items", self.add_lab_item);
        $("#PatientTypeID ").on('change', self.on_change_patient_type);
        $('body').on('change', "#PaymentModeID ", self.get_bank_name);
        $("#SalesTypeID").on('change', self.on_change_salestype);
        $('body').on('keyup change', "#DiscountAmount ", self.calculate_netamount);

        $("#btn_add_labtests").on("click", self.select_labtest);
       // $("body").on("click", "#btn_add_labtests", PatientDiagnosis.get_data_from_modal);

       $("body").on("ifChanged", ".labtest", self.set_lab_test);


    },

    calculate_netamount: function () {
        var self = LaboratoryInvoice;
        var NetAmount = 0;
        var DiscountAmount = clean($("#DiscountAmount").val());
        $('#labtest_list tbody tr.included').each(function () {
            NetAmount += clean($(this).find('input[name="price"]').val());
        });
        var Amount = NetAmount - DiscountAmount;
        $("#NetAmount").val(Amount);
    },

    get_customers: function (release) {
        var self = LaboratoryInvoice;
        $.ajax({
            url: '/Masters/Customer/GetDirectSalesInvoiceCustomerAutoComplete',
            data: {
                Type: $("#PatientTypeID option:selected").text(),
                Hint: $('#Patient').val(),

            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    set_customer: function (event, item) {
        var self = LaboratoryInvoice;
        if (($("#labtest_list tbody tr").length > 0) && (ID != $("#PatientID").val())) {
            app.confirm("Selected Items will be removed", function () {
                $('#labtest_list tbody').empty();
                $("#Patient").val(item.value);
                $("#PatientID").val(item.id);
                $("#Age").val(item.age);
                $("#Sex").val(item.gender);
                $("#Doctor").val(item.doctorName);
                $("#Mobile").val(item.mobileNo);
                $("#IPID").val(item.ipId);
                $("#NetAmount").val(0);
                $("#AppointmentProcessID").val(item.appointmentProcessId);
            })
        }
        else {
            $("#Patient").val(item.value);
            $("#PatientID").val(item.id);
            $("#AppointmentProcessID").val(item.opId);
            $("#Age").val(item.age);
            $("#Sex").val(item.gender);
            $("#Doctor").val(item.doctorName);
            $("#Mobile").val(item.mobileNo);
            $("#IPID").val(item.ipId);
            $("#AppointmentProcessID").val(item.appointmentProcessId);
        }
    },
    create: function () {
        var self = LaboratoryInvoice;
        self.freeze_headers();
        //$("body").on("click", ".print", self.print);
        $("body").on("click", ".print", self.printinvoice);
    },

    printinvoice: function () {
        var self = LaboratoryInvoice;
        SalesInvoiceID = $("#InvoiceID").val();
        self.printpdf(SalesInvoiceID);
    },

    print: function () {
        var self = LaboratoryInvoice;
        $.ajax({
            url: '/Reports/AHCMS/LabTestPrintPDF',
            data: {
                id: $("#AppointmentProcessID").val(),
                PatientLabTestMasterID: $("#PatientLabTestMasterID").val(),
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

    freeze_headers: function () {
        freeze_header = $("#labtest_list").FreezeHeader();
    },

    printpdf: function (SalesInvoiceID) {
        var self = LaboratoryInvoice;
        $.ajax({
            url: '/Reports/AHCMS/LaboratoryTestPrintPdf',
            data: {
                id: $("#AppointmentProcessID").val(),
                SalesInvoiceID: SalesInvoiceID
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
    show_popup: function () {
        var self = LaboratoryInvoice;
        var row = $(this).closest('tr');
        var Index = $(row).index();
        var SupplierID = row.find('.SupplierID').val();
        var Type = row.find('.Type').val();
        var IssueDate = row.find('.IssueDate').val();
        //$("#SupplierID").val(SupplierID);
        //$("#IssueDate").val(IssueDate);
        $("#Type").val(Type);
        $("#Index").val(Index);
        if (row.find('.chkCheck').is('[disabled=disabled]')) {
            $("#select_type").hide();
        }
        else {
            $("#select_type").show();
            $('#show-modal').trigger('click');
        }

    },
    show_supplier: function () {
        var self = LaboratoryInvoice;
        if ($('#Type :selected').text() == "External") {
            $("#supplier").show();
        }
        else {
            $("#supplier").hide();
        }
    },
    get_labtest_type: function () {
        var self = LaboratoryInvoice;
        var Index = $("#Index").val();
        var rowindex = $('#labtest_list tbody').find('tr').eq(Index);
        var SupplierID = $("#SupplierID").val();
        var type = $("#Type :selected").text();
        var IssueDate = $("#IssueDate").val();
        $(rowindex).find(".Type").val(type);
        $(rowindex).find(".SupplierID").val(SupplierID);
        $(rowindex).find(".IssueDate").val(IssueDate);
        //UIkit.modal($('#show-modal')).hide();
        $("#select_type").hide();
    },
    get_data: function () {
        var self = LaboratoryInvoice;
        var data = {};
        data.AppointmentProcessID = $("#AppointmentProcessID").val();
        data.PatientLabTestID = $("#PatientLabTestID").val();
        data.IPID = $("#IPID").val();
        data.PatientID = $("#PatientID").val();
        data.NetAmount = clean($("#NetAmount").val());
        data.SalesType = $("#SalesTypeID option:selected").text(),
        data.BankID = $("#BankID").val(),
        data.PaymentModeID = $("#PaymentModeID").val(),
        data.SalesTypeID = $("#SalesTypeID").val(),
        data.Date = $("#Date").val(),
        data.DiscountAmount = clean($("#DiscountAmount").val());
        data.Items = [];
        var item = {};
        $('#labtest_list tbody tr.included').each(function () {
            item = {};
            item.IssueDate = $(this).find(".IssueDate").val();
            item.ID = $(this).find(".ID").val();
            item.ItemID = $(this).find(".ItemID").val();
            item.Price = clean($(this).find('input[name="price"]').val());
            item.SupplierID = $(this).find(".SupplierID").val();
            item.LabtestType = $(this).find(".Type").val();
            data.Items.push(item);
        });
        return data;
    },
    get_lab_test_data: function () {
        var self = LaboratoryInvoice;
        var lab_data = {};
        lab_data.Items = [];
        var item = {};
        $('#labtest_list tbody tr.included.new-item').each(function () {
            item = {};
            item.Date = $(this).find(".IssueDate").val();
            item.ItemID = $(this).find(".ItemID").val();
            item.OPID = $(this).find(".OPID").val();
            item.IPID = $(this).find(".IPID").val();
            item.PatientID = $("#PatientID").val();
            lab_data.Items.push(item);
        });
        return lab_data;
    },
    save: function () {
        var self = LaboratoryInvoice;
        data = self.get_data();
        $.ajax({
            url: '/AHCMS/LaboratoryInvoice/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    var SalesInvoiceID = response.SalesInvoiceID;
                    app.show_notice(" Saved Successfully");
                    $('.btnSave').css({ 'visibility': 'hidden' });
                    self.printpdf(SalesInvoiceID);
                    //window.location = "/AHCMS/LaboratoryInvoice/Index";
                    setTimeout(function () {
                        window.location = '/AHCMS/LaboratoryInvoice/Index';
                    }, 20000);
                }
                else {
                    app.show_error('Failed to create LaboratoryInvoice');
                }
            }
        });
    },
    save_invoice: function () {
        var self = LaboratoryInvoice;
        var processID = $("#AppointmentProcessID").val();
        self.save_lab_test(processID);
    },
    save_lab_test: function (processID) {
        var self = LaboratoryInvoice;
        lab_data = self.get_lab_test_data();
        $.ajax({
            url: '/AHCMS/LabTest/SaveLabTest',
            data: lab_data,
            dataType: "json",
            type: "Post",
            success: function (response) {
                if (response.Status == "success") {

                    $.ajax({
                        url: '/AHCMS/LabTest/GetPatientLabTestID',
                        data: {
                            AppoinmentProcessID: processID
                        },
                        dataType: "json",
                        type: "POST",
                        success: function (response) {
                            $(response.data).each(function (i, record) {
                                $("#labtest_list tbody tr.included.new-item").find(".ItemID[value='" + record.ItemID + "']").closest("tr").each(function () {
                                    row = $(this).closest('tr');
                                    $(row).find(".ID").val(record.PatientLabTestID)
                                });
                            });
                            self.save();
                        }
                    });
                }
                else {
                    app.show_error('Failed to create LabTest');
                }
            }
        });
    },
    include_labtest: function () {
        var self = LaboratoryInvoice
        var row = $(this).closest('tr');
        var DiscountAmount = clean($("#DiscountAmount").val());
        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.ObserveValue').addClass('included').removeAttr('disabled');
            $(this).closest('tr').find('.Status').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
            var price = clean($(this).closest('tr').find('input[name="price"]').val());
            var netamount = clean($("#NetAmount").val().slice(1));
            var total = (netamount + price) - DiscountAmount;
            $("#NetAmount").val(total);
            self.count();
        } else {
            $(this).closest('tr').find('.ObserveValue').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').find('.Status').removeClass('included').attr('disabled', 'disabled');
            $(this).closest('tr').removeClass('included');
            var price = clean($(this).closest('tr').find('input[name="price"]').val());
            var netamount = clean($("#NetAmount").val().slice(1));
            var total = (netamount - price) + DiscountAmount;
            $("#NetAmount").val(total);
            self.count();
        }
    },

    save_confirm: function () {
        var self = LaboratoryInvoice;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            if ($('#labtest_list tbody tr.included.new-item').length > 0) {
                self.save_lab_test($("#AppointmentProcessID").val());
            }
            else {
                self.save();
            }
        }, function () {
        })
    },
    save_invoice_confirm: function () {
        var self = LaboratoryInvoice;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save_invoice();
        }, function () {
        })
    },
    validate_save: function () {
        var self = LaboratoryInvoice;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },
    count: function () {
        var self = LaboratoryInvoice;
        index = $("#labtest_list  tr.included").length;
        $("#item-count").val(index);
    },

    rules: {
        on_save: [

                {
                    elements: "#item-count",
                    rules: [
                       { type: form.required, message: "Please add atleast one item" },
                       { type: form.non_zero, message: "Please add atleast one item" },
                    ]
                },
                {
                    elements: "#Patient",
                    rules: [
                       { type: form.required, message: "Please Select Patient" },

                    ]
                },
        ],
        on_add_lab_tests: [
       {
           elements: "#LabTest",
           rules: [
           { type: form.required, message: "Please Select Lab Test" },
           {
               type: function (element) {
                   var error = false;
                   var count = 0;
                   var itemid = $("#LabTestID").val();
                   $("#labtest_list tbody tr.new-item").each(function () {
                       var id = $(this).find(".ItemID").val();
                       if (id == itemid)
                           error = "true";
                   });
                   return !error;
               }, message: "Item already exist"
           },
           ]
       },
        {
            elements: "#Patient",
            rules: [
               { type: form.required, message: "Please Select Patient" },

            ]
        },
        ]
    },
    set_Appointment_Scheduled_patient: function (event, item) {
        var self = LaboratoryInvoice;
        $("#PatientID").val(item.id),
        $("#Patient").val(item.value);
    },
    //set_lab_test: function (event, item) {
    //    var self = LaboratoryInvoice;
    //    $("#LabTest").val(item.value);
    //    $("#LabTestID").val(item.id);
    //    $("#Category").val(item.category);
    //    $("#Price").val(item.price);
    //},

    set_lab_test: function (event, item) {
        var self = LaboratoryInvoice;
        $("#LabTest").val(item.value);
        $("#LabTestID").val(item.id);
        $("#Category").val(item.category);
        $("#Price").val(item.price);
        UIkit.modal($('#select-employee')).hide();

    },

    add_lab_item: function () {
        var self = LaboratoryInvoice;
        var i = 0;
        if (self.validate_lab_tests() > 0) {
            return;
        }
        if ($("#Category").val() == "Lab Test Group") {
            var categoryID = $("#LabTestID").val();
            $.ajax({
                url: "/AHCMS/PatientDiagnosis/GetCategoryWiseLabItems",
                dataType: "json",
                data: {
                    LabTestCategoryID: categoryID
                },
                type: "POST",
                success: function (response) {
                    var item;
                    if (response.Status == "success") {
                        $(response.Data).each(function (i, record) {
                            self.add_item_to_grid(record.ID, record.ItemName, record.Price);
                        });
                    }
                }
            });

        } else {
            var ItemID = $("#LabTestID").val();
            var ItemName = $("#LabTest").val();
            var price = $("#Price").val();
            self.add_item_to_grid(ItemID, ItemName, price);
        }
        //if ($("#Category").val() == "Lab Test Group") {
        //    while (i < self.TestItem.length) {
        //        self.add_item_to_grid(self.TestItem[i].ID, self.TestItem[i].Name);
        //        ++i;
        //    }
        //}
    },
    add_item_to_grid: function (ItemID, ItemName, price) {
        var self = LaboratoryInvoice;
        var IssueDate = $("#Date").val();
        var AppointmentProcessID = $("#AppointmentProcessID").val();
        var IPID = $("#IPID").val();
        var SupplierID = 0;
        var Type = "Internal";
        var content = "";
        var sino = $("#labtest_list tbody tr").length + 1;
        var $content;
        content = '<tr class ="new-item included">'
            + '<td class="uk-text-center">' + sino
            + '<input type="hidden" class="ItemID"value="' + ItemID + '"/>'
            + '<input type="hidden" class="SupplierID"value="' + SupplierID + '"/>'
            + '<input type="hidden" class="Type"value="' + Type + '"/>'
            + '<input type="hidden" class="IssueDate" value="' + IssueDate + '"/>'
            + '<input type="hidden" class="OPID" value="' + AppointmentProcessID + '"/>'
            + '<input type="hidden" class="IPID" value="' + IPID + '"/>'
            + '<input type="hidden" class="ID" value="' + 0 + '"/>'
           // + '<td class="uk-text-center checked width-20" data-md-icheck><input type="checkbox" class="chkCheck" /></td>'
            + '<td class="uk-text-center checked width-20" data-md-icheck><input type="checkbox" class="chkCheck" checked /></td>'
            + '<td><input type="text" class="ItemName md-input label-fixed" disabled="disabled" value="' + ItemName + '" /></td>'
            + '<td><input type="text" name="price" class="md-input price mask-currency" disabled="disabled" value="' + price + '" /></td>'
            + '</tr>';
        $content = $(content);
        app.format($content);
        $('#labtest_list tbody').append($content);
        self.clear_lab_items();
        self.include_labtest();
        self.calculate_netamount();
        //$.ajax({
        //    url: '/AHCMS/LaboratoryInvoice/GetLabTestDetails',
        //    data: {
        //        ItemID: ItemID,
        //        PatientID: $("#PatientID").val()
        //    },
        //    dataType: "json",
        //    type: "GET",
        //    success: function (Response) {
        //        price = Response.data.Price;

        //    }
        //});
    },
    validate_lab_tests: function () {
        var self = LaboratoryInvoice;
        if (self.rules.on_add_lab_tests.length > 0) {
            return form.validate(self.rules.on_add_lab_tests);
        }
        return 0;
    },
    clear_lab_items: function () {
        var self = LaboratoryInvoice;
        $("#LabTestID").val('');
        $("#LabTest").val('');

    },
    //clear_lab_items: function () {
    //    var self = LaboratoryInvoice;
    //    $("#LabTestID").val('');
    //    $("#LabTest").val('');
    //    $("#Category").val('');
    //    $("Price").val('');

    //},
    get_bank_name: function () {
        var self = LaboratoryInvoice;
        var mode;
        var Module = "Receipt"

        if ($("#PaymentModeID option:selected").text().toUpperCase() == "CASH") {
            mode = "Cash";
        }
        else {
            mode = "Bank"
        }

        $.ajax({
            url: '/Masters/Treasury/GetBank',
            data: {
                ModuleName: Module,
                Mode: mode
            },
            dataType: "json",
            type: "GET",
            success: function (response) {
                $("#BankID").html("");
                var html = "";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.BankName + "</option>";
                });
                $("#BankID").append(html);
            }
        });

    },
    on_change_salestype: function () {
        var self = LaboratoryInvoice;
        var SaleType = $("#SalesTypeID option:selected").text();
        var PatientType = 'LabInvoice' + $("#PatientTypeID option:selected").text()

        if (SaleType == "Credit Sale") {
            $(".CreditSale").hide();
        }
        else {
            $(".CreditSale").show();
        }
        Customer.directsales_customer_list(PatientType);
        $('#direct-customer-list').SelectTable({
            selectFunction: self.select_patient,
            returnFocus: "#LabTest",
            modal: "#select-direct-customer",
            initiatingElement: "#CustomerName",
            startFocusIndex: 3
        });
    },
    on_change_patient_type: function () {
        var self = LaboratoryInvoice;
        var PatientType = $("#PatientTypeID option:selected").text();
        var Configvalue=$("#ConfigValue").val();
        if (Configvalue == 1) {
            $("#SalesTypeID").val(1).attr("selected", "selected");
        }
        else
        {
            if (PatientType == "IP") {
                $("#SalesTypeID").val(1).attr("selected", "selected");
                //$("#SalesTypeID").attr("disabled", false);
            }
            else {
                $("#SalesTypeID").val(2).attr("selected", "selected");
                //$("#SalesTypeID").attr("disabled", true);
            }
        }
        self.on_change_salestype();
        if ($("#ID").val() == 0) {
            $("#PatientID").val('');
            $("#Patient").val('');
            $("#labtest_list tbody tr").empty();
        }
    },

    select_labtest: function () {
        var self = LaboratoryInvoice;
        var radio = $('#select-labtest tbody input[type="checkbox"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        var Price = $(row).find(".Price").val();
        var Category = $(row).find(".Category").val();
        $("#LabTest").val(Name);
        $("#LabTestID").val(ID);
        $("#Price").val(Price);
        $("#Category").val(Category);
        
        $("#labtest-list tbody tr.included").each(function () {
            $(this).find('.ItemID').parent('div').removeClass("checked");
            $(this).removeClass('included');
        });

        UIkit.modal($('#select-labtest')).hide();


    },

    //set_labtest: function () {
    //    var self = LaboratoryInvoice;

    //    if ($(this).is(":checked") == true) {
    //        var LabTestID = clean($(this).val());
    //        self.LabTestItems.push({ LabTestID: LabTestID });
    //    }
    //    else {
    //        var LabTestID = clean($(this).val());
    //        var index = self.LabTestItems.indexOf(LabTestID)
    //        self.LabTestItems.splice(index, 1);
    //    }
    //},

 

    //PrescribedTest: [],
    //get_data_from_modal: function () {
    //    var self = PatientDiagnosis;
    //    var data = {};
    //    PrescribedTest = [];
    //    for (var i = 0; i < self.LabItemList2.length; i++) {
    //        var item = {};
    //        item.ID = self.LabItemList2[i].id;
    //        item.Name = self.LabItemList2[i].name;
    //        item.Date = $("#TestDate").val();
    //        self.PrescribedTest.push(item);
    //    }
    //    self.add_data_to_grid();
    //    $("#labtest-list tbody tr.included").each(function () {
    //        $(this).find('.ItemID').parent('div').removeClass("checked");
    //        $(this).removeClass('included');
    //    });
    //    self.PrescribedTest = [];
    //    self.LabItemList = [];
    //    self.LabItemList2 = [];

    //    return data;
    //},

}
