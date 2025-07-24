PurchaseReturnProcessing = {
    init: function () {
        var self = PurchaseReturnProcessing;
        self.bind_events();
        self.show_hide_date();
        index = $("#purchase-return-item-list tbody tr.included").length;
        $("#item-count").val(index);
    },



    bind_events: function () {
        var self = PurchaseReturnProcessing;
        $("#btnshow").on("click", self.show);
        $(".btnSaveAsDraft,.btnSaveAndNew").on("click", self.submit);
        $(".btnSave").on("click", self.save_confirm);
        $("body").on('ifChanged', '.chkCheck', self.include_item);
        $("body").on('change', '#ProcessingType', self.show_hide_date);
        $("body").on("keyup change", ".ReturnQty", self.on_change_return_qty);
    },

    on_change_return_qty: function () {
        var self = PurchaseReturnProcessing;
        var row = $(this).closest('tr');
        var stock = clean($(row).find('.Stock').val());
        var ReturnQty = clean($(row).find('.ReturnQty').val());
        var PurchaseRate = clean($(row).find('.PurchaseRate').val());
        var InvoiceQty = clean($(row).find('.InvoiceQty ').val());
        var OfferQty = clean($(row).find('.OfferQty').val());
        var IGSTPercentage = clean($(row).find('.IGSTPercentage').val());
        var CGSTPercentage = clean($(row).find('.CGSTPercentage').val());
        var SGSTPercentage = clean($(row).find('.SGSTPercentage').val());
        var acceptedqty = InvoiceQty + OfferQty;

        if (ReturnQty > acceptedqty) {
            app.show_error('Return quantity should be less than or equal to maximum invoice quantity ');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else if (ReturnQty > stock) {
            app.show_error('Return quantity should be less than or equal to stock ');
            app.add_error_class($(row).find('.ReturnQty'));
        }
        else {
            var IsGSTRegistered = $(row).find('.IsGSTRegistered').val();
            var LoationStateID = $("#LoationStateID").val();
            var SupplierStateID = clean($(row).find('.SupplierStateID').val());
            var taxableAmt = (ReturnQty * PurchaseRate);

            var IGSTAmount = 0
            var SGSTAmount = 0
            var CGSTAmount = 0
            var GSTAmount = 0;
            if (IsGSTRegistered == false || IsGSTRegistered == "false") {
                 IGSTAmount = 0
                 SGSTAmount = 0
                 CGSTAmount = 0
                 GSTAmount = 0;
            } else {
                if (SupplierStateID == LoationStateID) {
                    IGSTAmount = 0;
                    SGSTAmount = (taxableAmt*(SGSTPercentage/100));
                    CGSTAmount = (taxableAmt * (CGSTPercentage / 100));
                }
                else {
                    IGSTAmount = (taxableAmt * (IGSTPercentage / 100));
                }
                GSTAmount = IGSTAmount + SGSTAmount + CGSTAmount
            }
            var totalamount = GSTAmount + taxableAmt;
            $(row).find('.Value').val(totalamount.toFixed(2))
            $(row).find('.GSTAmount').val(GSTAmount.toFixed(2))
            $(row).find('.IGSTAmount').val(IGSTAmount.toFixed(2))
            $(row).find('.CGSTAmount').val(CGSTAmount.toFixed(2))
            $(row).find('.SGSTAmount').val(SGSTAmount.toFixed(2))
        }

    },

    save_confirm: function () {
        var self = PurchaseReturnProcessing
        app.confirm_cancel("Do you want to Save", function () {
            self.submit();
        }, function () {
        })
    },

    show_hide_date: function () {
        var self = PurchaseReturnProcessing;
        var ProcessingType = $("#ProcessingType").val();
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        today = dd + '/' + mm + '/' + yyyy;
        var fdd = String(firstDay.getDate()).padStart(2, '0');
        var fmm = String(firstDay.getMonth() + 1).padStart(2, '0'); //January is 0!
        var fyyyy = firstDay.getFullYear();

        firstDay = fdd + '/' + fmm + '/' + fyyyy;
        if (ProcessingType == "NonMovingItems") {
            $(".MovingItems").removeClass("uk-hidden");
            $(".ExpireItems").addClass("uk-hidden");
            $(".SlowMovingItem").addClass("uk-hidden");

            $("#FromTransactionDate").addClass("enabled");
            $("#ToTransactionDate").addClass("enabled");
            $("#AsOnDate").removeClass("enabled");
            $("#Days").removeClass("enabled");

            $("#ToTransactionDate").val(today);
            $("#FromTransactionDate").val(firstDay);
        }
        else if (ProcessingType == "SlowMovingItems")
        {
            $(".MovingItems").addClass("uk-hidden");
            $(".ExpireItems").addClass("uk-hidden");
            $(".SlowMovingItem").removeClass("uk-hidden");

            $("#FromTransactionDate").removeClass("enabled");
            $("#ToTransactionDate").removeClass("enabled");
            $("#AsOnDate").removeClass("enabled");

            $("#Days").addClass("enabled");

        }
        else if (ProcessingType == "Expired") {
            $(".MovingItems").addClass("uk-hidden");
            $(".ExpireItems").addClass("uk-hidden");
            $(".SlowMovingItem").addClass("uk-hidden");

            $("#FromTransactionDate").removeClass("enabled");
            $("#ToTransactionDate").removeClass("enabled");
            $("#AsOnDate").removeClass("enabled");
            $("#Days").removeClass("enabled");
        }
        else {
            $(".MovingItems").addClass("uk-hidden");
            $(".ExpireItems").removeClass("uk-hidden");
            $(".SlowMovingItem").addClass("uk-hidden");

            $("#FromTransactionDate").removeClass("enabled");
            $("#ToTransactionDate").removeClass("enabled");
            $("#AsOnDate").addClass("enabled");
            $("#Days").removeClass("enabled");
            $("#AsOnDate").val(today);
        }
    },

    show: function () {
        var self = PurchaseReturnProcessing;
        self.error_count = 0;
        self.error_count = self.validate_Item();
        if (self.error_count > 0) {
            return;
        }
        var FromDate = $("#FromTransactionDate").val();
        var ToDate = $("#ToTransactionDate").val();
        var AsOnDate = $("#AsOnDate").val();
        var ProcessingType = $("#ProcessingType").val();
        var Days = clean($("#Days").val());
        $.ajax({
            url: '/Purchase/PurchaseReturnProcessing/PurchaseReturnProcessingItem',
            dataType: "html",
            data: {
                ProcessingType: ProcessingType,
                FromDate: FromDate,
                ToDate: ToDate,
                AsOnDate: AsOnDate,
                Days: Days
            },
            type: "POST",
            success: function (response) {
                $("#purchase-return-item-list tbody").empty();
                $response = $(response);
                app.format($response);
                $("#purchase-return-item-list tbody").append($response);
            },
        })
    },

    include_item: function () {
        var self = PurchaseReturnProcessing;

        if ($(this).is(":checked")) {
            $(this).closest('tr').find('.ReturnQty').addClass('included').removeAttr('readonly');
            $(this).closest('tr').find('.ReturnQty').addClass('included').removeAttr('disabled');
            $(this).closest('tr').addClass('included');
        } else {
            $(this).closest('tr').find('.ReturnQty').addClass('included').attr("disabled", true);
            $(this).closest('tr').find('.ReturnQty').removeClass('included').attr('readonly', 'readonly');
            $(this).closest('tr').removeClass('included');
        }
        index = $("#purchase-return-item-list tbody tr.included").length;
        $("#item-count").val(index);
    },

    save: function () {
        var self = PurchaseReturnProcessing;
        var modal = self.get_data();
        $.ajax({
            url: '/Purchase/PurchaseReturnProcessing/Save',
            data: modal,
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data.Status == "Success") {
                    app.show_notice("PurchaseReturnProcessing created successfully");
                    setTimeout(function () {
                        window.location = "/Purchase/PurchaseReturnOrder/Index";
                    }, 1000);
                } else {
                    app.show_error(data.Message);
                    $('.btnSave').css({ 'visibility': 'visible' });
                }
            },
        });
    },

    get_data: function () {
        var self = PurchaseReturnProcessing;
        var data = {};
        data.Items = [];
        var item = {};
        $("#purchase-return-item-list tbody tr.included").each(function () {
            item = {};
            item.SupplierID = clean($(this).find(".SupplierID").val());
            item.ItemID = $(this).find(".ItemID").val();
            item.UnitID = $(this).find(".UnitID").val();
            item.InvoiceID = $(this).find(".InvoiceID").val();
            item.SupplierStateID = $(this).find(".SupplierStateID").val();
            item.IGSTPercentage = $(this).find(".IGSTPercentage").val();
            item.CGSTPercentage = $(this).find(".CGSTPercentage").val();
            item.SGSTPercentage = $(this).find(".SGSTPercentage").val();
            item.BatchID = $(this).find(".BatchID").val();
            item.PurchaseRate = clean($(this).find(".PurchaseRate").val());
            item.GSTAmount = clean($(this).find(".GSTAmount").val());
            item.IGSTAmount = clean($(this).find(".IGSTAmount").val());
            item.SGSTAmount = clean($(this).find(".SGSTAmount").val());
            item.CGSTAmount = clean($(this).find(".CGSTAmount").val());
            item.Value = clean($(this).find(".Value").val());
            item.ReturnQty = clean($(this).find(".ReturnQty").val());
            item.InvoiceTransID = clean($(this).find(".InvoiceTransID").val());
            data.Items.push(item);
        });
        return data;
    },

    save_confirm: function () {
        var self = PurchaseReturnProcessing;
        self.error_count = 0;
        self.error_count = self.validate_save();
        if (self.error_count > 0) {
            return;
        }
        app.confirm_cancel("Do you want to Save", function () {
            self.save();
        }, function () {
        })
    },


    validate_Item: function () {
        var self = PurchaseReturnProcessing;
        if (self.rules.on_show.length > 0) {
            return form.validate(self.rules.on_show);
        }
        return 0;
    },

    validate_save: function () {
        var self = PurchaseReturnProcessing;
        if (self.rules.on_save.length) {
            return form.validate(self.rules.on_save);
        }
        return 0;
    },

    rules: {
        on_show: [

                {
                    elements: "#ProcessingType",
                    rules: [
                         { type: form.required, message: "Please Select ProcessingType" },

                    ]
                },
                 {
                     elements: ".enabled",
                     rules: [
                          { type: form.required, message: "Invalid Date" },
                          {
                              type: function (element) {
                                  var error = false;
                                      var ProcessingType = $('#ProcessingType').val();
                                      var Days = $("#Days").val();
                                      if (ProcessingType == 'SlowMovingItems' && Days<=0)
                                          error = true;
                                  return !error;
                              }, message: 'No Of Days Inventory Held'
                          },

                     ]
                 }
        ],
        on_save: [

              {
                  elements: "#purchase-return-item-list tbody tr.included td .ReurnQty",
                  rules: [
                     { type: form.required, message: "Please Add ReurnQty" },
                     { type: form.non_zero, message: "Please Add ReurnQty" },
                     {
                         type: function (element) {
                             var error = false;
                             $('#purchase-return-item-list tbody tr.included').each(function () {
                                 var row = $(this).closest('tr');
                                 var returnqty = clean($(row).find('.ReurnQty').val());
                                 var AvailableStock = clean($(row).find('.Stock').val());
                                 var accepted = clean($(row).find('.clAcptQty ').val());
                                 if (returnqty > AvailableStock)
                                     error = true;

                             });
                             return !error;
                         }, message: 'Selected item dont have enough stock'
                     },
                     {
                         type: function (element) {
                             var error = false;
                             $('#purchase-return-item-list tbody tr.included').each(function () {
                                 var row = $(this).closest('tr');
                                 var returnqty = clean($(row).find('.ReurnQty').val());
                                 var AvailableStock = clean($(row).find('.Stock').val());
                                 var accepted = (clean($(row).find('.InvoiceQty').val()) + clean($(row).find('.OfferQty').val()));
                                 if (returnqty > accepted)
                                     error = true;

                             });
                             return !error;
                         }, message: 'Return quantity should be less than or equal to total invoice quantity'
                     },
                  ]
              },
              {
                  elements: "#item-count",
                  rules: [
                     { type: form.required, message: "Please Add Atleast One Item" },
                     { type: form.non_zero, message: "Please Add Atleast One Item" },
                  ]
              }
        ]
    },






}