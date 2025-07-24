ChartOfAccounts = {
    init: function () {
        var self = ChartOfAccounts;
        self.tree();
        self.bind_events();
    },

    list: function () {
        $list = $('#Accounts-list');
        if ($list.length) {
            $list.find('thead.search th').each(function () {
                var title = $(this).text().trim();
                $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
            });
            $('#Accounts-list tbody tr').on('click', function () {
                var Id = $(this).find("td:eq(0) .ID").val();
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

    bind_events: function () {
        var self = ChartOfAccounts;
        $("body").on("click", ".btnAddAccount", self.show_modal);
        $("#btnAdd").on("click", self.save);
        $("body").on("click", ".btnEditAccount", self.edit_modal);
        $("body").on("click", ".btnCancel", self.delete_confirm);
    },

    show_modal: function () {
        var self = ChartOfAccounts;
        self.clear_items();
        $(".AccountCode").show();
        var ParentID = $(this).data("parent-id");
        var Level = $(this).data("level");
        $("#ParentID").val(ParentID);
        $("#Level").val(Level);
        UIkit.modal("#add-account-head").show();
    },

    tree: function () {
        $("#tA").fancytree({
            checkbox: 0,
            selectMode: 0,
            //imagePath: "/assets/icons/others/",
            autoScroll: 0,
        })
    },

    get_data: function () {
        var self = ChartOfAccounts;
        var data = {};
        data.ID = $("#ID").val(),
        data.AccountID = $("#AccountID").val();
        data.AccountName = $("#AccountName").val();
        data.OpeningAmount = $("#OpeningAmount").val();
        data.ParentID = $("#ParentID").val();
        data.Level = $("#Level").val();
        if ($(".IsManual").prop('checked') == true) {
            data.IsManual = true;
        } else {
            data.IsManual = false;
        }

        return data;
    },

    save: function () {
        var self = ChartOfAccounts;
        self.error_count = 0;
        self.error_count = self.validate_form();
        if (self.error_count > 0) {
            return;
        }
        var data;
        data = self.get_data();
        $.ajax({
            url: '/Masters/ChartOfAccounts/Save',
            data: data,
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    app.show_notice("Saved Successfully");
                    window.location = "/Masters/ChartOfAccounts/Index";
                }
                else {
                    app.show_error("Account Code Already Exist");
                    $(" .btnSave").css({ 'visibility': 'visible' });
                }
            }
        });
    },

    edit_modal: function () {
        var self = ChartOfAccounts;
        $(".AccountCode").hide();
        var AccountName = $(this).data("name");
        var ID = $(this).data("id");
        var OpeningAmount = $(this).data("amount");
        var IsManual = $(this).data("ismanual");
        if (IsManual == "True") {
            $(".IsManual").closest('div').addClass("checked")
        }
        else {
            $(".IsManual").closest('div').removeClass("checked")
        }
        $("#AccountName").val(AccountName);
        $("#ID").val(ID);
        $("#OpeningAmount").val(OpeningAmount);
        UIkit.modal("#add-account-head").show();
    },

    clear_items: function () {
        var self = ChartOfAccounts;
        $("#AccountID").val('');
        $("#AccountName").val('');
        $("#OpeningAmount").val('');
        $(".IsManual").closest('div').removeClass("checked")


    },

    delete_confirm: function () {
        var self = ChartOfAccounts;
        var ID = $(this).data("id");
        app.confirm_cancel("Do you want to remove the item", function () {
            $.ajax({
                url: '/Masters/ChartOfAccounts/IsRemovedItem',
                data: {
                    ID: ID
                },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data.Status == "success") {
                        app.show_notice("Deleted Successfully");
                        window.location = "/Masters/ChartOfAccounts/Create";
                    } else {
                        app.show_error("Child Belongs To This Node So Can't Be Deleted");
                    }
                },
            });
        }, function () {
        })
    },

    validate_form: function () {
        var self = ChartOfAccounts;
        if (self.rules.on_form.length) {
            return form.validate(self.rules.on_form);
        }
        return 0;
    },

    rules: {
        on_form: [
                //{
                //    elements: "#AccountID",
                //    rules: [
                //        { type: form.required, message: "Please enter a Code" }
                //    ],
                //},
                {
                    elements: "#AccountName",
                    rules: [
                        { type: form.required, message: "Please enter a AccountName" }
                    ],
                },
        ],
    }
}


