$(function () {

    AccessPermission.manage();
});

AccessPermission = {
    manage: function () {
        $currobj = this;


        $(function () { $currobj.init(); });

        this.init = function () {

            $('body').on('change', '#ddlRole,#ddlController', function () {
                $currobj.option_changed(false);
            });
            //$('body').on('change', '#ddlArea', $currobj.module_changed);
            $('body').on('change', '#ddlArea', function () { $currobj.option_changed(true); });


        }

        this.option_changed = function (isUpdateControllerDdl) {
            var successCallback = function (response) {
                //var selectedController = $('#ddlController').val();
                if (response != null) {
                    $('#roleModuleContainer').html(response.RoleModuleListViewStr);
                }
                if (isUpdateControllerDdl) {
                    var optionHtml = '';
                    optionHtml = '<option value="" >Select</option>'
                    $(response.Controllers).each(function (index, value) {
                        optionHtml += '<option value="' + value + '" >' + value + '</option>';
                    });

                    $('#ddlController').html(optionHtml);
                    //$('#ddlController').val(selectedController);
                }
            };

            var data = {
                roleID: $('#ddlRole').val(),
                selectedArea: $('#ddlArea').val(),
                selectedController: $('#ddlController').val(),
            };
            if ($('#ddlRole').val() > 0)
                $currobj.ajaxRequest("GetRoleModuleListViewDetails", data, 'GET', successCallback);
        };

        this.root_url = function () {
            return "/Manage/";
        }

        this.ajaxRequest = function (url, data, requestType, callBack) {
            $.ajax({
                url: $currobj.root_url() + url,
                cache: false,
                type: requestType,
                async: false,
                //traditional: true,
                data: data,
                success: function (successResponse) {
                    //console.log('successResponse');
                    if (callBack != null && callBack != undefined)
                        callBack(successResponse);
                },
                error: function (errResponse) {//Error Occured 
                    //console.log(errResponse);
                }
            });
        }

    }
}
