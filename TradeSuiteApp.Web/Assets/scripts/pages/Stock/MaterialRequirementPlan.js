MaterialRequirementPlan = {

    init : function(){
        var self = MaterialRequirementPlan;
        self.bind_events();
    },

    bind_events : function(){
        var self = MaterialRequirementPlan;
        $("body").on("click", "#btnFilter", self.filter_items);
        $("body").on("click", ".GenerateRequisition", self.Save);
    },

    filter_items: function () {
        var self = MaterialRequirementPlan;
        self.error_count = 0;
        self.error_count = self.validate_add();
        if (self.error_count > 0) {
            return;
        }
        var FromDate = $("#FromDate").val();
        var ToDate = $("#ToDate").val();
        $.ajax({
            url: '/Stock/MaterialRequirmentPlan/GetMaterialRequirmentPlanList',
            data :{FromDate: FromDate,
                ToDate : ToDate,},
            dataType: "json",
            type: "POST",
            success: function (response) {
                var content = "";
                var $content;
                length = response.Data.length;
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td class="sl-no uk-text-center" width="30">' + slno + '</td>'
                        + '<td class="ItemName" width="100">' + item.ItemName
                        + '<input type="hidden" class="ItemID"value="' + item.ItemID + '"/>'
                        + '<input type="hidden" class="UnitID"value="' + item.UnitID + '"/>'
                        + '<input type="hidden" class="RequiredDate"value="' + item.RequiredDate + '"/>'
                        + '</td>'
                        + '<td class="RequiredQty uk-text-right mask-qty">' + item.RequiredQty + '</td>'
                        + '<td class="AvailableStock uk-text-right mask-qty">' + item.AvailableStock + '</td>'
                        + '<td class="QtyInQC uk-text-right mask-qty">' + item.QtyInQC + '</td>'
                        + '<td class="OrderedQty uk-text-right mask-qty">' + item.OrderedQty + '</td>'
                        + '<td width="50">' + '<input type="text" value=" ' + item.RequestedQty + '" class="md-input RequestedQty uk-text-right mask-qty" />' + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $("#Requirement-plan-list tbody").html($content);
            }
        });
    },

    get_data: function () {
        var self = MaterialRequirementPlan;
        var data = {};
        data.FromDate = $("#FromDate").val(),
        data.ToDate = $("#ToDate").val(),
        data.Items = [];
        var item = {};
        $('#Requirement-plan-list tbody tr').each(function () {
            item = {};
            item.ItemID =clean( $(this).find(".ItemID").val());
            item.RequiredQty = clean($(this).find(".RequiredQty").val());
            item.AvailableStock = clean($(this).find(".AvailableStock").val());
            item.QtyInQC = clean($(this).find(".QtyInQC").val());
            item.OrderedQty =clean( $(this).find(".OrderedQty").val());
            item.RequestedQty = clean($(this).find(".RequestedQty").val());
            item.UnitID = clean($(this).find(".UnitID").val());
            item.RequiredDate = $(this).find(".RequiredDate").val();
            data.Items.push(item);
        });
        return data;
    },

    Save: function () {
        var self = MaterialRequirementPlan;
        //self.error_count = 0;
        //self.error_count = self.validate_form();
        //if (self.error_count > 0) {
        //    return;
        //}
        var data;
        var ID;
        data = self.get_data();
        $.ajax({
            url: '/Stock/MaterialRequirmentPlan/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice(" Saved Successfully");
                    ID = response.Data
                    window.location = '/Purchase/PurchaseRequisition/Details/' + ID;
                }
                else {
                    app.show_error('Failed to create');
                }
            }
        });
    },

    validate_add: function () {
        var self = MaterialRequirementPlan;
        if (self.rules.on_add.length) {
            return form.validate(self.rules.on_add);
        }
        return 0;
    },

    rules: {
        on_add: [
             {
                 elements: "#FromDate",
                 rules: [
                     { type: form.required, message: "Please Select FromDate" },
                     { type: form.non_zero, message: "Please Select FromDate" },
                 ],
             },
              {
                  elements: "#UserLocationID",
                  rules: [
                      { type: form.required, message: "Please Select ToDate" },
                      { type: form.non_zero, message: "Please Select ToDate" },
                  ],
              },

               
        ],
    },

}