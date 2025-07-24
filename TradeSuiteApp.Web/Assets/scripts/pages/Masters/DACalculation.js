$(function () {
    DA.init();
    //DA.bind_events();
});
DA = {

    init: function () {
        var self = DA;
        if ($("#page_content_inner").hasClass("form-view")) {
            self.view_type = "form";
        } else if ($("#page_content_inner").hasClass("list-view")) {
            self.view_type = "list";
        } else {
            self.view_type = "details";
        }
        if (self.view_type == "list") {
            self.da_calculation_list();
        } else {
            fh_items = $("#da-calculation-list").FreezeHeader();
        }
        self.bind_events();
        
    },

    da_calculation_list: function () {
        $da_calculation_list = $('#da-calculation-list');
        $('#da-calculation-list tbody tr').on('click', function () {
            var id = $(this).find('.ID').val();
            window.location = '/Masters/DACalculations/Details/' + id;
        });
        if ($da_calculation_list.length) {
            $da_calculation_list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            altair_md.inputs();
            var dacalculation_list_table = $da_calculation_list.dataTable({
                "bLengthChange": false,
                "bFilter": true,
                "pageLength": 15
            });
            dacalculation_list_table.api().columns().each(function (g, h) {
                $('thead.search input').on('keyup change', function () {
                    var index = $(this).parent().parent().index();
                    dacalculation_list_table.api().column(index).search(this.value).draw();
                });
            });
        }
    },

    bind_events: function () {
        var self = DA;
        $("#btnAddProduct").on('click', DA.add_item);
        self.common_load();
    },

    add_item: function () {
        var self = DA;
        self.error_count = 0;
        self.error_count = self.validate_item();
        //if (self.error_count > 0) {
        //    sel.clear_Item();
        //    return;
        //}
        self.error_count = self.validate_item();
        if (self.error_count > 0) {
            return;
        }
        var item = {};
        item.EmployeeCategory = $("#EmployeeCategoryID").val() == "" ? "" : $("#EmployeeCategoryID  option:selected").text();
        item.EmployeeStatus = ($("#EmployeeStatusID :selected").text());
        item.PayrollCategory = ($("#PayrollCategoryID :selected").text());
        item.Department = ($("#DepartmentID :selected").text());
        //item.Location = ($("#LocationID : selected").text());
        item.Location = $("#LocationID").val() == "" ? "" : $("#LocationID  option:selected").text();
        item.Month = $("#Month").val();
        item.BasicPoints = clean($("#BasicPoints").val());
        item.AdditionalPoints = clean($("#AdditionalPoints").val());
        item.BasicValue = clean($("#BasicValue").val());
        item.AdditionalValue = clean($("#AdditionalValue").val());
        item.StartDate = $("#StartDate").val();
        item.EndDate = $("#EndDate").val();

        self.add_item_to_grid(item);
        self.clear_Item();
    },
    add_item_to_grid: function (item) {
        var self = DA;
        var index, tr;
        index = $("#da-calculation-list tbody tr").length + 1;
        tr = '<tr>'
        + ' <td class="uk-text-center">' + index + ' </td>'
        + ' <td class="EmployeeCategory">' + item.EmployeeCategory + '</td>'
        + ' <input type="hidden" class="EmployeeCategoryID" value="' + $('#EmployeeCategoryID').val() + '" /></td>'
        + ' <td class="EmpStatus">' + item.EmployeeStatus + '</td>'
        + ' <input type="hidden" class="EmployeeStatusID" value="' + $('#EmployeeStatusID').val() + '" /></td>'
        + ' <td class="PayrollCat">' + item.PayrollCategory + '</td>'
        + ' <input type="hidden" class="PayrollCategoryID" value="' + $('#PayrollCategoryID').val() + '" /></td>'
        + ' <td class="Department">' + item.Department + '</td>'
        + ' <input type="hidden" class="DepartmentID" value="' + $('#DepartmentID').val() + '" /></td>'
        + ' <td class="Location">' + item.Location + '</td>'
        + ' <input type="hidden" class="LocationID" value="' + $('#LocationID').val() + '" /></td>'
        + ' <td class="Month">' + item.Month + '</td>'
        + ' <td class="Amount mask-currency">' + item.BasicPoints + '</td>'
        + ' <td class="Amount mask-currency">' + item.AdditionalPoints + '</td>'
        + ' <td class="Amount mask-currency">' + item.BasicValue + '</td>'
        + ' <td class="Amount mask-currency">' + item.AdditionalValue + '</td>'
        + ' <td class="StartDate">' + item.StartDate + '</td>'
        + ' <td class="EndDate">' + item.EndDate + '</td>'
        + ' <td class="uk-text-center">'
        + '     <a class="remove-item">'
        + '         <i class="uk-icon-remove"></i>'
        + '     </a>'
        + ' </td>'
        + '</tr>';
        $("#item-count").val(index);
        var $tr = $(tr);
        app.format($tr);
        $("#da-calculation-list tbody").append($tr);
        fh_items.resizeHeader();
    },

    clearItemSelectControls: function () {
        //$("#ItemName").val('');
        //$("#ItemID").val('');
    },
    validate_item: function () {
        var self = DA;
        if (self.rules.on_add.length > 0) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
            {
                elements: "#Code",
                rules: [
                    { type: form.required, message: "Please Enter the Code" },
                ]
            },
        ]
    },
    common_load: function () {
        var self = DA;
        var SelectAllOption = [{
            text: "ALL",
            value: 0
        }];
        var SelectAllNoOption = [{
            text: "ALL",
            value: 0
        }
        //{
        //    text: "No",
        //    value: -1
        //}
        ];
        $("#ItemAccountsCategory").prepend(self.common_dropdown(SelectAllOption));
        $("#EmployeeCategoryID").prepend(self.common_dropdown(SelectAllOption));
        $("#EmployeeStatusID").prepend(self.common_dropdown(SelectAllOption));
        $("#PayrollCategoryID").prepend(self.common_dropdown(SelectAllOption));
        $("#DepartmentID").prepend(self.common_dropdown(SelectAllNoOption));
        //$("#ItemTaxCategory").prepend(self.common_dropdown(SelectAllNoOption));
    },
    common_dropdown: function (content) {
        var result = "";
        var selected;
        for (var i = 0; i < content.length; i++) {
            i == 0 && clean($('#ID').val()) == 0 ? selected = "selected='selected'" : selected = "";
            result = result + "<option value='" + content[i].value + "' " + selected + " >" + content[i].text + "</option>";
        }
        return result;
    },
};