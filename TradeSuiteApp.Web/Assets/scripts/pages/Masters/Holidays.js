$(function () {
    holiday.init();
});
var freeze_header;
holiday = {
    init: function () {
        holiday.purchase_requisition_list();
        holiday.bind_events();
        freeze_header = $("#location-check-list").FreezeHeader();
    },

    bind_events: function () {
        $("#btnAddGenralHoliday").on('click', holiday.add_item);
    },

    add_item: function () {
        var self = holiday;
        self.error_count = 0;
        self.error_count = self.validate_item();
        if (self.error_count == 0) {
            var SerialNo = $(".prTbody .rowPr").length + 1;
            var html = '  <tr class="rowPr">' +
                        ' <td class="uk-text-center">' + SerialNo + '</td>' +
                        ' <td class="clProduct">' + $("#Date").val() + '</td>' +
                        ' <td class=" clUnit">' + $("#IsRestrictedOrOptional").val() + '</td>' +
                        ' <td class=" clUnit">' + $("#HolidayName").val() + '</td>' +
                        ' <td class=" clUnit">' + $("#checkBoxId").val() + '</td>' +
                        ' <td class="uk-text-center" onclick="$(this).parent().remove()">' +
                        '   <a data-uk-tooltip="{pos:"bottom"}" >' +
                            '   <i class="md-btn-icon-small uk-icon-remove"></i>' +
                            ' </a>' +
                        ' </td>' +
                        '</tr>';
            var $html = $(html);
            app.format($html);
            $(".prTbody").append($html);
            self.clearItemSelectControls();
            $("#ItemName").focus();
            SetMinDateValue();

            freeze_header.resizeHeader();
        }
    },
};