ProductionIssue.init = function () {
    var self = ProductionIssue;
    ProductionIssueCRUD.createedit();
    ProductionIssue.freeze_headers();
    $('body').on('click', '.btnOpenAdditionalOutput', self.open_additional_output);

    $.UIkit.autocomplete($('#item-autocomplete'), { 'source': self.get_items, 'minLength': 1 });
    $('#item-autocomplete').on('selectitem.uk.autocomplete', self.set_item_details);
    $('body').on('click', '#btnOkAdditionalOutput', self.add_additional_output);
    $("body").on("click", ".remove-item", self.remove_item);
}
ProductionIssue.remove_item = function () {
    var self = purchase_requisition;
    $(this).closest("tr").remove();
    $("#tblOutputProductionIssue tbody tr").each(function (i, record) {
        $(this).children('td').eq(0).html(i + 1);
    });


}
ProductionIssue.add_additional_output = function (release) {
    var self = ProductionIssueCRUD;
    self.error_count = 0;
    self.error_count = self.validate_additional_output();
    if (self.error_count > 0) {
        return;
    }
    var production_sequence = $("#Additional_Production_Sequence").val();
    var start_date = $("#Additional_Production_Sequence").val();
    var item_name = $("#AdditionalOutputItemName").val();
    var item_id = $("#AdditionalOutputItemID").val();
    var start_time = $("#AdditionalStartTime").val();
    var start_date = $("#AdditionalStartDate").val();
    var end_time = $("#AdditioanlEndTime").val();
    var end_date = $("#additionalEndDate").val();
    var receipt_store = $("#AdditionalOutputStore option:selected").text();
    var receipt_store_id = $("#AdditionalOutputStore option:selected").val();
    var actual_output = clean($("#AdditionalActualOutput").val());
    var status = 'completed'
    var tr = '';
    var $items_list = $('#tblOutputProductionIssue tbody');
    var index = $items_list.find('tr').length + 1;
    var count = 0;
    $('#tblOutputProductionIssue tbody tr').each(function () {
        if ($(this).find(".ItemID").val() == item_id)
            count++;
    });
    if (count != 0) {
        app.show_error("Item already in output list");
        return;
    }
    var tr = '<tr data-production-sequence = "' + production_sequence + '">'
                + '   <td class="uk-text-center">' + index + '</td>'
                + '   <td>' + item_name
                + '        <input type="hidden" class="ItemID" value="' + item_id + '" />'
                + '        <input type="hidden" class="ProductionSequence" value="' + production_sequence + '" />'
                + '        <input type="hidden" class="ProductionQC" value="' + false + '" />'
                + '        <input type="hidden" class="ReceiptStore" value="' + receipt_store_id + '" />'
                + '        <input type="hidden" class="StandardBatchSize" value="' + 0 + '" />'
                + '        <input type="hidden" class="ActualBatchSize" value="' + 0 + '" />'
                + '        <input type="hidden" class="SubProduct" value="' + true + '" />'
                + '   </td>'
                + '   <td><input type="text" class="md-input mask-production-qty txtStandardOutput" value="' + actual_output + '" readonly /></td>'
                + '   <td><input type="text" class="md-input mask-production-qty txtActualOutput" value="' + actual_output + '" readonly/></td>'
                + '   <td><input type="text" class="md-input mask-production-qty txtVariance" value="0" readonly /></td>'
                + '   <td>' + receipt_store + '</td>'
                + '   <td>'
                + '        <input type="text" class="md-input label-fixed past-date date txtStartDate" value="' + start_date + '" readonly="readonly" />'
                + '   </td>'
                + '   <td><input type="text" class="md-input time txtStartTime" value="' + start_time + '" readonly="readonly" /></td>'
                + '   <td>'
                + '        <input type="text" class="md-input label-fixed current-date date txtEndDate" value="' + end_date + '" readonly="readonly" />'
                + '   </td>'
                + '   <td><input type="text" class="md-input time txtEndTime" value="' + end_time + '" readonly="readonly" /></td>'
                + '   <td>' + status + '</td>'
                + ' <td class="uk-text-center" onclick="$(this).parent().remove()">'
                + '   <a data-uk-tooltip="{pos:"bottom"}" >'
                + '   <i class="md-btn-icon-small uk-icon-remove"></i>'
                + ' </a>'
                + ' </td>'
                + '</tr>';
    var $tr = $(tr);
    app.format($tr);
    $items_list.append($tr);
    self.clear_additional_output_modal();
    UIkit.modal($('#additional-output')).hide();
}
ProductionIssueCRUD.clear_additional_output_modal = function (release) {
    $("#Additional_Production_Sequence").val('');
    $("#Additional_Production_Sequence").val('');
    $("#AdditionalOutputItemName").val('');
    $("#AdditionalOutputItemID").val('');
    $("#AdditionalStartTime").val('');
    $("#AdditionalStartDate").val('');
    $("#AdditioanlEndTime").val('');
    $("#additionalEndDate").val('');
    $("#AdditionalActualOutput").val('');

}
ProductionIssue.get_items = function (release) {
    $.ajax({
        url: '/Masters/Item/GetStockableItemsForAutoComplete',
        data: {
            Hint: $('#AdditionalOutputItemName').val(),
            ItemCategoryID: $("#ItemCategoryID").val(),
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            release(data);
        }
    });
}

ProductionIssue.set_item_details = function (event, item) {
    $("#AdditionalOutputItemID").val(item.id);
    $("#AdditionalOutputItemName").val(item.name);

}
ProductionIssue.open_additional_output = function () {
    var Production_Sequence = $("#SequenceItemID option:selected").data("production-sequence");
    var startdate = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + Production_Sequence + "]").find('.txtStartDate').val();
    var storeID = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + Production_Sequence + "]").find('ReceiptStore').val();
    var output_length = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + Production_Sequence + "]").length;
    var output_status = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + Production_Sequence + "]").find('.OutputStatus').val();
    var output_item_id = $("#tblOutputProductionIssue tbody").find("tr[data-production-sequence=" + Production_Sequence + "]").find('.OutputStatus').val();

    if (output_length != 0 && output_status == 'started') {
        $("#AdditionalStartDate").val(startdate);
        $("#Additional_Production_Sequence").val(Production_Sequence);
        $("#show-additional-output").trigger('click');
    }

}
ProductionIssue.select_item = function () {
    var self = ProductionIssue;

    var radio = $('#select-item tbody input[type="radio"]:checked');
    var row = $(radio).closest("tr");
    var item = {
        id: radio.val(),
        name: $(row).find(".Name").text().trim(),

    };
    $("#AdditionalOutputItemID").val(item.id);
    $("#AdditionalOutputItemName").val(item.name);
    UIkit.modal($('#select-item')).hide();
}

ProductionIssueCRUD.add_output = function () {
    var self = ProductionIssueCRUD;
    var serial_no = $("#tblOutputProductionIssue tbody tr").length + 1;
    var item_name = $("#SequenceItemID option:selected").text();
    var item_id = $("#SequenceItemID").val();
    var production_sequence = $("#SequenceItemID option:selected").data("production-sequence");
    var standard_output = $("#SequenceItemID option:selected").data("standard-output") * (clean($("#txtActualBatchSize").val()) / clean($("#txtStandardBatchSize").val()));
    var receipt_store = self.generate_ddl_html(self.Stores, true, "ReceiptStore", "", "", self.defaultReceiptStoreID);
    var start_date = $("#txtStartDate").val();
    var start_time = $("#txtStartTime").val();
    var qc = $("#SequenceItemID option:selected").data("production-qc");
    var status = "<select class='md-input OutputStatus' ><option value='started' selected='selected'>Started</option><option value='completed'>Completed</option></select>";
    var tr = '<tr data-production-sequence = "' + production_sequence + '">'
             + '   <td class="uk-text-center">' + serial_no + '</td>'
             + '   <td>' + item_name
             + '        <input type="hidden" class="ItemID" value="' + item_id + '" />'
             + '        <input type="hidden" class="ProductionSequence" value="' + production_sequence + '" />'
             + '        <input type="hidden" class="ProductionQC" value="' + qc + '" />'
             + '        <input type="hidden" class="SubProduct" value="1" />'
             + '        <input type="hidden" class="StandardBatchSize" value="' + $("#txtStandardBatchSize").val() + '" />'
             + '        <input type="hidden" class="ActualBatchSize" value="' + $("#txtActualBatchSize").val() + '" />'
             + '   </td>'
             + '   <td><input type="text" class="md-input mask-production-qty txtStandardOutput" value="' + standard_output + '" readonly /></td>'
             + '   <td><input type="text" class="md-input mask-production-qty txtActualOutput" value="" /></td>'
             + '   <td><input type="text" class="md-input mask-production-qty txtVariance" value="" readonly /></td>'
             + '   <td>' + receipt_store + '</td>'
             + '   <td>'
             + '        <input type="text" class="md-input label-fixed past-date date txtStartDate" value="' + start_date + '" readonly="readonly" />'
             + '   </td>'
             + '   <td><input type="text" class="md-input time txtStartTime" value="' + start_time + '"  /></td>'
             + '   <td>'
             + '       <div class="uk-input-group">'
             + '           <input type="text" class="md-input label-fixed current-date date txtEndDate" value="" />'
             + '           <span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar current-date endDate"></i></span>'
             + '       </div>'
             + '   </td>'
             + '   <td><input type="text" class="md-input time txtEndTime" value="" /></td>'
             + '   <td>' + status + '</td>'
             + '<td></td>'
            + '</tr>';
    tr = $(tr);
    app.format(tr);
    $("#tblOutputProductionIssue tbody").append(tr);
}
