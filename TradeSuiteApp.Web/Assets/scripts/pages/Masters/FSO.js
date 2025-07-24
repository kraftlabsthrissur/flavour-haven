
FSO = {
    
    init: function () {
        var self = FSO;
        employee_list = Employee.employee_list();
        item_select_table = $('#employee-list').SelectTable({
            selectFunction: self.select_employee,
            modal: "#select-employee",
            initiatingElement: "#TransferFSOName"
        });
        self.bind_events();
        self.get_existing_customers();
        self.area_manager_check();
        self.zonal_manager_check();
        self.active_check();
        self.regional_manager_check();
        self.sales_manager_check();
        self.fso_list();
        self.fso_managers_list();
        $('#fso-list').SelectTable({
            selectFunction: self.select_fso,
            modal: "#select-fso",
            initiatingElement: "#FsoName"
        });

        $('#fso-managers-list').SelectTable({
            selectFunction: self.select_fso_manager,
            modal: "#select-fso-manager",
            initiatingElement: "#ReportingToName"
        });

        
    },
    
    fso_list: function () {
        var $list = $('#fso-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/FSO/GetFSOList",
                    "type": "POST",
                    "data": function (data) {
                        data.params = [
                            { Key: "ID", Value: $('#ID').val() },
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
                            return "<input type='radio' class='uk-radio FSOID' name='FSOID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "FSOCode", "className": "FSOCode" },
                    { "data": "FSOName", "className": "FSOName" },
                    { "data": "AreaManager", "className": "AreaManager" },
                    { "data": "ZonalManager", "className": "ZonalManager" },
                    { "data": "SalesManager", "className": "SalesManager" },
                    { "data": "RegionalSalesManager", "className": "RegionalSalesManager" },
                    { "data": "RouteName", "className": "RouteName" },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
            });
            $('body').on("change", '#ID', function () {
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
        var $list = $('#fso-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $("body").on('click', '#fso-list tbody td', function () {
                var Id = $(this).closest("tr").find("td:eq(0) .ID").val();
                window.location = '/Masters/FSO/Details/' + Id;
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

    select_employee: function () {
        var self = FSO;
        var radio = $('#select-employee tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".Name").text().trim();
        var Code = $(row).find(".Code").text().trim();
        $("#FSOName").val(Name);
        $("#EmployeeID").val(ID);
        $("#FSOCode").val(Code);
        UIkit.modal($('#select-employee')).hide();
        self.IsFSOExist(ID);
    },

    IsFSOExist: function (Id) {
        var self = FSO;
        $.ajax({
            url: '/Masters/FSO/IsFSOExist',
            data: {
                ID: Id
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                if (result.Data == 1) {
                    app.show_error('Sales Representative already exist and Try to Edit');
                        $("#FSOName").val('');
                        $("#EmployeeID").val('');
                        $("#FSOCode").val('');
                }
            }
        });
    },

    bind_events: function () {
        var self = FSO;
        $.UIkit.autocomplete($('#employee-autocomplete'), { 'source': self.get_employee, 'minLength': 1 });
        $('#employee-autocomplete').on('selectitem.uk.autocomplete', self.set_employee);
        $.UIkit.autocomplete($('#fso-autocomplete'), { 'source': self.get_fso, 'minLength': 1 });
        $('#fso-autocomplete').on('selectitem.uk.autocomplete', self.set_fso);
        $.UIkit.autocomplete($('#fso-managers-autocomplete'), { 'source': self.get_fso_manager, 'minLength': 1 });
        $('#fso-managers-autocomplete').on('selectitem.uk.autocomplete', self.get_fso_manager);

        $("#btnOKEmployee").on("click", self.select_employee);
        $("#btn-ok-fso").on("click", self.select_fso);

        $(".btnSave").on("click", self.save_confirm);
        $("body").on("ifChanged", ".IsActive", self.active_check);
        $("body").on("ifChanged", ".Is-AreaManager", self.area_manager_check);
        $("body").on("ifChanged", ".Is-ZonalManager", self.zonal_manager_check);
        $("body").on("ifChanged", ".Is-RegionalSalesManager", self.regional_manager_check);
        $("body").on("ifChanged", ".Is-SalesManager", self.sales_manager_check);
        $("body").on("click", "#StateID", self.GetDistrict);
        $("body").on("click", "#btnfilter", self.get_filter_item);
        $("body").on("ifChanged", ".check-box", self.check_items);
    },

    active_check: function () {
        var self = FSO;
        if ($(".IsActive").prop('checked') == false && $("#ID").val()>0) {
            self.Inactive_confirm();
        }  else {
            $(".ToDate-hide").hide();
            $(".ToDate-hide").val("");
        }

    },

    Inactive_confirm: function () {
        var self = FSO;
        var ID = $("#ID").val();
            $.ajax({
                url: '/Masters/FSO/InactiveConfirm',
                data: {
                    ID: ID
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success" && response.Data == false) {
                        app.show_error("Please remove customers or fso under this fso");
                        $(".IsActive").closest('div').addClass("checked")
                        $(".IsActive").prop('checked', true)
                    }
                    else
                    {
                        var date = new Date().getDate();
                        var month = new Date().getMonth() + 1;
                        var year = new Date().getFullYear();
                        $(".ToDate-hide").show();
                        $(".ToDate-hide").val(date + "-" + month + "-" + year);
                    }
                },
            });
    },

    regional_manager_check: function () {
        var self = FSO;
        if ($(".Is-RegionalSalesManager").prop('checked') == true) {
            $(".AreaManager-hide").hide();
            $(".ZonalManager-hide").hide();
            $(".RegionalSalesManager-hide").hide();
            $("#AreaManagerID").val("");
            $("#ZonalManagerID").val("");
            $("#RegionalSalesManagerID").val("");
           
        } else {
            $(".ZonalManager-hide").show();
            $(".AreaManager-hide").show();
            $(".RegionalSalesManager-hide").show();
         
        }
    },

    sales_manager_check: function () {
        var self = FSO;
        if ($(".Is-SalesManager").prop('checked') == true) {
            $(".AreaManager-hide").hide();
            $(".ZonalManager-hide").hide();
            $(".RegionalSalesManager-hide").hide();
            $(".SalesManager-hide").hide();
            $("#AreaManagerID").val("");
            $("#ZonalManagerID").val("");
            $("#RegionalSalesManagerID").val("");
            $("#SalesManagerID").val("");
          
        } else {
            $(".ZonalManager-hide").show();
            $(".AreaManager-hide").show();
            $(".RegionalSalesManager-hide").show();
            $(".SalesManager-hide").show();
            $('.hide').show();
         
        }
    },

    zonal_manager_check: function () {
        var self = FSO;
        if ($(".Is-ZonalManager").prop('checked') == true) {
            $(".AreaManager-hide").hide();
            $(".ZonalManager-hide").hide();
            $("#AreaManagerID").val("");
            $("#ZonalManagerID").val("");
          
        } else {
            $(".ZonalManager-hide").show();
            $(".AreaManager-hide").show();
            $(".Is-RegionalSalesManager-hide").show();
            $(".Is-AreaManager-hide").show();
           
        }
    },

    area_manager_check: function () {
        var self = FSO;
        if ($(".Is-AreaManager").prop('checked') == true)
        {
            $(".AreaManager-hide").hide();
            $("#AreaManagerID").val("");
        }
        else
        {
            $(".AreaManager-hide").show();
        }

    },

    get_employee: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#FSOName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    set_employee: function (event, item) {
        var self = FSO;
        console.log(item)
        $("#EmployeeID").val(item.id),
        $("#FSOName").val(item.name);
        $("#FSOCode").val(item.Code);
        self.IsFSOExist(item.id);
    },

    save_confirm: function () {
        var self = FSO
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.Save();
        }, function () {
        })
    },

    Save: function () {
        var self = FSO;
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/FSO/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    window.location = "/Masters/FSO/Index";
                }
                else {
                    app.show_error('Failed to create Sales Representative');
                }
            }
        });
    },

    get_data: function () {
        var self = FSO;
        var data = {};
        data.ID = $("#ID").val();
        data.BusinessCategoryID = $("#BusinessCategoryID").val();
        data.SalesIncentiveCategoryID = $("#SalesIncentiveCategoryID").val();
        data.SalesCategoryID = $("#SalesCategoryID").val();
        data.RouteCode = $("#RouteCode").val();
        data.RouteName = $("#RouteName").val();
        data.ZoneCode = $("#ZoneCode").val();
        data.ZoneName = $("#ZoneName").val();
        data.FSOCode = $("#FSOCode").val();
        data.FSOName = $("#FSOName").val();
        data.EmployeeID = $("#EmployeeID").val();
        data.FromDate = $("#FromDate").val();
        data.ReportingToID = $("#ReportingToID").val();
        if ($(".Is-SalesManager").prop('checked') == true) {
            data.IsSalesManager = true;
            data.SalesManagerID = 0
            data.RegionalSalesManagerID = 0
            data.AreaManagerID = 0
            data.ZonalManagerID=0
        } else {
            data.IsSalesManager = false;
            data.SalesManagerID = $("#SalesManagerID").val();
        }
        if ($(".Is-RegionalSalesManager").prop('checked') == true) {
            data.IsRegionalSalesManager = true;
            data.RegionalSalesManagerID = 0
            data.AreaManagerID = 0
            data.ZonalManagerID = 0
          
        } else {
            data.IsRegionalSalesManager = false;
            data.RegionalSalesManagerID = $("#RegionalSalesManagerID").val();
        }
        if ($(".Is-AreaManager").prop('checked') == true) {
            data.IsAreaManager = true;
            data.AreaManagerID = 0
            
        } else {
            data.IsAreaManager = false;
            data.AreaManagerID = $("#AreaManagerID").val();
        }
        if ($(".Is-ZonalManager").prop('checked') == true) {
            data.IsZonalManager = true;
            data.AreaManagerID = 0
            data.ZonalManagerID = 0
           
        } else {
            data.IsZonalManager = false;
            data.ZonalManagerID = $("#ZonalManagerID").val();
        }
        if ($(".IsActive").prop('checked') == true) {
            data.IsActive = true;
            data.ToDate = "";
        } else {
            data.IsActive = false;
            data.ToDate = $("#ToDate").val();
        }
        var StartDate = $("#StartDate").val();
        data.Items = [];
        var item = {};
        $('#tbl_customers tbody tr.included ').each(function () {
            item = {};
            item.CustomerID = $(this).find(".CustomerID").val();
            item.SalesIncentiveCategoryID = $(this).find(".SalesIncentiveCategoryID").val();
            item.StartDate = StartDate;
            item.StateID = $(this).find(".StateID").val();
            item.DistrictID = $(this).find(".DistrictID").val();
            data.Items.push(item);
        });

      
        return data;
    },

    GetDistrict: function () {
        $.ajax({
            url: '/Masters/District/GetDistrict/',
            dataType: "json",
            type: "GET",
            data: {
                StateID: $('#StateID').val(),
            },
            success: function (response) {
                var html = "<option value >Select</option>";
                $.each(response.data, function (i, record) {
                    html += "<option value='" + record.ID + "'>" + record.Name + "</option>";
                });
                    $("#DistrictID").html("");
                    $("#DistrictID").append(html);
            }
        });
    },

    check_items: function () {
        var self = FSO;
        $("#tbl_customers tbody tr").each(function () {
            var row = $(this).closest('tr');
            if ($(row).find(".check-box").prop("checked") == true) {
                $(row).addClass('included');
            } else {
                $(row).removeClass("included");
            }
        });
        self.count_items();
    },

    count_items: function () {
        var count = $('#tbl_customers tbody').find('input.check-box:checked').length;
        if (count > 0) {
            $("#StartDate").removeAttr("disabled").addClass("enabled");
        }
        else {
            $("#StartDate").attr("disabled", "disabled").removeClass("enabled");
        }
        $('#item-count').val(count);
    },

    get_filter_item: function () {
        var length;
        var tr = "";
        var $tr;
        var self = FSO;
        $.ajax({
            url: '/Masters/FSO/GetCustomersByFilters/',
            dataType: "json",
            data: {
                'StateID': $("#StateID").val(),
                'DistrictID': $("#DistrictID").val(),
                'CustomerCategoryID': $("#CustomerCategoryID").val(),
                'FSOID': $("#TransferFSOID").val(),
                'SalesIncentiveCategoryID' : $("#SalesIncentiveMappingCategoryID").val()
            },
            type: "POST",
            success: function (response) {
                var StateID = $("#StateID").val();
                var State = $("#StateID Option:Selected").text();
                var DistrictID = $("#DistrictID").val();
                var District = $("#DistrictID Option:Selected").text();
                length = response.Data.length;
                if (length != 0) {
                    $(response.Data).each(function (i, item) {
                        var slno =  (i + 1);
                        tr += '<tr class="included">'
                            + '<td class="sl-no">' + slno + '</td>'
                             + '<td class="td-check">' + '<input type="checkbox" name="items" data-md-icheck Checked class="md-input  check-box"/>' + '</td>'
                            + '<td>' + item.CustomerCode + '</td>'
                            + '<td>' + item.CustomerName + '</td>'
                            + '<td>' + item.CustomerCategory + '</td>'
                            + '<td class="SalesIncentiveCategory">' + item.SalesIncentiveCategory
                            + '<input type="hidden" class="CustomerID" value="' + item.CustomerID + '"/>'
                            + '<input type="hidden" class="CustomerCategoryID" value="' + item.CustomerCategoryID + '"/>'
                            + '<input type="hidden" class="SalesIncentiveCategoryID" value="' + item.SalesIncentiveCategoryID + '"/>'
                            + '<input type="hidden" class="DistrictID" value="' + item.DistrictID + '"/>'
                            + '<input type="hidden" class="StateID" value="' + item.StateID + '"/>'
                            + '</td>'
                            + '<td>' + item.State + '</td>'
                            + '<td>' + item.District + '</td>'
                            + '<td>' + item.FSOName + '</td>'
                            + '</tr>';
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $('#tbl_customers tbody').html($tr);
                    self.count_items();
                }
            },
        });
    },

    get_existing_customers: function () {
        var length;
        var tr = "";
        var $tr;
        var self = FSO;
        var ID = clean($("#ID").val());
        if (ID > 0)
        {
        $.ajax({
            url: '/Masters/FSO/GetCustomersByFSO/',
            dataType: "json",
            data: {
                'ID': ID
            },
            type: "POST",
            success: function (response) {
                length = response.Data.length;
                if (length != 0) {
                    $(response.Data).each(function (i, item) {
                        var slno = (i + 1);
                        tr += '<tr class="included">'
                            + '<td class="sl-no">' + slno + '</td>'
                            + '<td>' + item.CustomerCode + '</td>'
                            + '<td>' + item.CustomerName + '</td>'
                            + '<td>' + item.CustomerCategory + '</td>'
                            + '<td class="SalesIncentiveCategory">' + item.SalesIncentiveCategory+ '</td>'
                            + '<td>' + item.State + '</td>'
                            + '<td>' + item.District + '</td>'
                            + '</tr>';
                    });
                    $tr = $(tr);
                    app.format($tr);
                    $('#tbl_fsocustomers tbody').html($tr);
                    self.count_items();
                }
            },
        });
        }
    },

    select_fso: function () {
        var self = FSO;
        var radio = $('#select-fso tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".FSOName").text().trim();
        $("#TransferFSOName").val(Name);
        $("#TransferFSOID").val(ID);
        $("#TransferFSOID").trigger("change");
        UIkit.modal($('#select-fso')).hide();
    },

    get_fso: function (release) {
        $.ajax({
            url: '/Masters/FSO/GetFSOAutoComplete',
            data: {
                Hint: $('#TransferFSOName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },

    set_fso: function (event, item) {
        var self = FSO;
        $("#TransferFSOID").val(item.id),
        $("#TransferFSOName").val(item.FSOName);
    },

    fso_managers_list: function () {
        var $list = $('#fso-managers-list');
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
                "aaSorting": [[1, "asc"]],
                "ajax": {
                    "url": "/Masters/FSO/GetFSOManagersList",
                    "type": "POST",
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
                            return "<input type='radio' class='uk-radio FSOID' name='FSOID' data-md-icheck value='" + row.ID + "' >";
                        }
                    },
                    { "data": "FSOCode", "className": "FSOCode" },
                    { "data": "FSOName", "className": "FSOName" },
                    { "data": "Designation", "className": "Designation" },
                    { "data": "RouteName", "className": "RouteName" },
                ],
                "drawCallback": function () {
                    altair_md.checkbox_radio();
                    $list.trigger("datatable.changed");
                },
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

    select_fso_manager: function () {
        var self = FSO;
        var radio = $('#select-fso-manager tbody input[type="radio"]:checked');
        var row = $(radio).closest("tr");
        var ID = radio.val();
        var Name = $(row).find(".FSOName").text().trim();
        $("#ReportingToName").val(Name);
        $("#ReportingToID").val(ID);
        UIkit.modal($('#select-fso-manager')).hide();
    },

    get_fso_manager: function (release) {
        $.ajax({
            url: '/Masters/FSO/GetFSOManagersAutoComplete',
            data: {
                Hint: $('#ReportingToName').val()
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result);
            }
        });
    },

    set_fso_manager: function (event, item) {
        var self = FSO;
        $("#ReportingToID").val(item.id),
        $("#ReportingToName").val(item.FSOName);
    },


    validate_form: function () {
        var self = FSO;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_save: [
               {
                   elements: "#FromDate",
                   rules: [
                       { type: form.required, message: "Please enter DateOfJoining" },
                      
                   ],
               },
                 {
                     elements: "#EmployeeID",
                     rules: [
                         { type: form.required, message: "Please enter FSOName", alt_element: "#FSOName" },
                         { type: form.non_zero, message: "Please enter FSOName", alt_element: "#FSOName" },
                         { type: form.positive, message: "Please enter FSOName", alt_element: "#FSOName" },
                     ],
                 },
                  {
                      elements: "#StartDate.enabled",
                      rules: [
                          { type: form.required, message: "Please enter StartDate" },
                          { type: form.non_zero, message: "Please enter StartDate" },
                          { type: form.positive, message: "Please enter StartDate" },
                      ],
                  },


        ]
    },
}