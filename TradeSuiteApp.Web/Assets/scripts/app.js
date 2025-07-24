$(function () {
    app.init();
});
var CurrentLocationID;


var $switcher = $('#style_switcher'),
    $switcher_toggle = $('#style_switcher_toggle');
var $switcher_horizontal = $('#style_switcher_horizontal'),
    $switcher_toggle_horizontal = $('#style_switcher_toggle_horizontal');

var $switcher_batch = $('#style_switcher_batch'),
    $switcher_toggle_batch = $('#style_switcher_toggle_batch');
app = {
    init: function () {

        app.bind_events();
        app.masked_inputs();
        app.validate();
        app.focus();
        if ($("#current-location select option").length == 1) {
            $("#current-location select").attr("disabled", "disabled");
        }

        CurrentLocationID = $("#current-location select").val();

        $("#preloader").hide().addClass("transparent");
        $('input,textarea').attr('autocomplete', 'off');
        if ($(".menu_section li").length <= 16) {
            $(".submenu_trigger").addClass("act_section");
            $(".submenu_trigger ul").show();
        }
        $("[readonly]").attr("tabindex", -1);
        $("[disabled]").attr("tabindex", -1);

    },


    focus: function () {
        setTimeout(function () {
            $('#page-container input.md-input:visible:not([readonly]):not([disabled]), #page-container select.md-input:visible:not([readonly]):not([disabled]), textarea.md-input:visible:not([readonly]):not([disabled])').eq(0).focus();
        }, 50);
    },
    refresh: function () {
        //this.bind_events();
        //this.masked_inputs();
        //altair_md.inputs();
        //altair_md.checkbox_radio();
    },
    stopPropagation: function (event) {
        console.log(event);
        // alert(event.target);
        // event.preventDefault();
        if (event && event.stopPropagation) {
            event.stopPropagation();
        }
        else if (window.event) {
            window.event.cancelBubble = true;
        }
        else if (window.$.Event.prototype) {
            window.$.Event.prototype.stopPropagation();
        }
        return false;
    },
    format: function (wrapper) {
        if (typeof wrapper != "undefined") {
            this.masked_inputs(wrapper);
            altair_md.inputs($(wrapper));
            altair_md.checkbox_radio($(wrapper).find('[data-md-icheck]'));

            var relaxation = $(this).data('relaxation');
            $(wrapper).find('.uk-icon-calendar.future-date').each(function () {
                if (typeof relaxation == "undefined") {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: 1, format: 'DD-MM-YYYY' });
                } else {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 1, format: 'DD-MM-YYYY' });
                }
                var self = $(this);
                $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                    var element = $(self).closest('.uk-input-group').find('input.md-input');
                    element.val(date.current.format('DD-MM-YYYY'));
                    element.focus();
                });
            });
            $(wrapper).find('.uk-icon-calendar.future-date-only').each(function () {
                if (typeof relaxation == "undefined") {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: 2, format: 'DD-MM-YYYY' });
                } else {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 2, format: 'DD-MM-YYYY' });
                }
                var self = $(this);
                $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                    var element = $(self).closest('.uk-input-group').find('input.md-input');
                    element.val(date.current.format('DD-MM-YYYY'));
                    element.focus();
                });
            });

            $(wrapper).find('.uk-icon-calendar.current-date').each(function () {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: 1, maxDate: 0, format: 'DD-MM-YYYY' });
                var self = $(this);
                $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                    var element = $(self).closest('.uk-input-group').find('input.md-input');
                    element.val(date.current.format('DD-MM-YYYY'));
                    element.focus();
                });
            });

            $(wrapper).find('.uk-icon-calendar.past-date').each(function () {
                var relaxation = $(this).data('relaxation');
                if (typeof relaxation == "undefined") {
                    var date = UIkit.datepicker(UIkit.$(this), { maxDate: 0, format: 'DD-MM-YYYY' });
                } else {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 1, maxDate: 0, format: 'DD-MM-YYYY' });
                }

                var self = $(this);
                $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                    var element = $(self).closest('.uk-input-group').find('input.md-input');
                    element.val(date.current.format('DD-MM-YYYY'));
                    element.focus();
                });
            });

            $(wrapper).find('.uk-icon-calendar.all-date').each(function () {
                var relaxation = $(this).data('relaxation');
                if (typeof relaxation == "undefined") {
                    var date = UIkit.datepicker(UIkit.$(this), { format: 'DD-MM-YYYY' });
                } else {
                    var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation - 1, maxDate: relaxation + 1, format: 'DD-MM-YYYY' });
                }

                var self = $(this);
                $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                    var element = $(self).closest('.uk-input-group').find('input.md-input');
                    element.val(date.current.format('DD-MM-YYYY'));
                    element.focus();
                });
            });
            $(wrapper).find('.time').each(function () {
                $.UIkit.timepicker($(this), { format: '12h' });
            });

            $(wrapper).find('.time15').each(function () {
                $.UIkit.timepicker($(this), { format: '12h', step: '15' });
            });
        }

    },
    //load_popup: function () {
    //    $.ajax({
    //        url: '/Account/Load_PopUp',
    //        //  data: { UserName: $("#LUserName").val(), Password: $("#LPassword").val() },
    //       dataType: "html",
    //        type: "POST",
    //        success: function (response) {

    //             //   $("#LPassword").attr("type", "password");
    //              //  UIkit.modal("#login-popup", { bgclose: false, modal: false, keyboard: false }).show();


    //        }
    //    });

    //},
    load_content: function (url, type) {

        window.location = url;
        // --- for ajax pages --- //
        //if (typeof type == "undefined") {
        //    type = "page";
        //}
        //$.ajax({
        //    url: url,
        //    // dataType: "html",
        //    type: "GET",
        //    success: function (response) {
        //        if (typeof app.jqXHR.responseJSON != "undefined" && typeof app.jqXHR.responseJSON.Status != "undefined") {
        //            if (app.jqXHR.responseJSON.Status == "failure" && app.jqXHR.responseJSON.StatusCode == 302) {
        //                console.log('Session Timeout');
        //            }
        //        } else if (app.jqXHR.getResponseHeader("X-Responded-JSON") != null
        //                && JSON.parse(app.jqXHR.getResponseHeader("X-Responded-JSON")).status == "401") {
        //                console.log('Authentication Timeout');
        //        } else {
        //            var $response = $(response);
        //            app.format($response);
        //            $("#page-container").html($response);
        //            var Title = $("#ViewBagTitle").text();
        //            var Statuses = $("#ViewBagStatuses").html();
        //            if (typeof Title != "undefined") {
        //                $('title').text(Title);
        //            } else {
        //                $('title').text("");
        //            }
        //            if (typeof Statuses != "undefined") {
        //                $("#color-indicator").html(Statuses);
        //            } else {
        //                $("#color-indicator").html("");
        //            }
        //            history.pushState({}, '', url);
        //        }
        //    },
        //    error: function () {
        //        console.log(app.jqXHR);
        //    }
        //});
        // --- for ajax pages --- //
    },
    bind_events: function () {
        // --- for ajax pages --- //
        // On clicking back button, ensure page reload
        //$(document).ready(function ($) {
        //    $(window).on('popstate', function () {
        //        location.reload(true);
        //    });
        //});
        //$('body').on('click', 'a:not(.strict)', function (e) {
        //    e.preventDefault();
        //    var href = $(this).attr('href');
        //    if (href != "" && href != undefined && href != "return:void()") {
        //        app.load_content(href, "page");
        //    }
        //});
        // --- for ajax pages --- //
        $("body").on("keydown", "#LPassword", function (e) {
            if (e.which == 13) {
                $("#LSubmit").trigger("click");
            }
        });
        $("body").on("click", "#LSubmit", function () {
            $.ajax({
                url: '/Account/AjaxLogin',
                data: { UserName: $("#LUserName").val(), Password: $("#LPassword").val() },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.Status == "success") {
                        UIkit.modal("#login-popup").hide();
                        $("#LPassword").val("");
                        $("#LPassword").attr("type", "text");
                    } else {
                        app.show_error(response.Message);
                    }
                }
            });

        });

        $("body").on('click', '.heading_actions .uk-dropdown.uk-dropdown-small .uk-nav li a:contains("Print")', function () {
            window.print();
        });
        $('body').on('change', '#current-location select', function () {
            app.confirm_cancel("Are you sure you want to change the location?", function () {
                var LocationID = $('#current-location select').val();
                $.ajax({
                    url: '/Account/ChangeUserLocation',
                    data: { LocationID: LocationID },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        if (response.Status == "success") {
                            window.location = window.location;
                        }

                    }
                });
                return true;
            }, function () {
                $('#current-location select').val(CurrentLocationID);
                return false;
            });

        });
        $('.view-file').each(function () {
            $(this).width($(this).closest('div').width() - 40);
        });
        $('body').on('click', '.view-file', function () {
            var path = $(this).data('path');
            var url = $(this).data('url');
            window.open(url, '_blank');
        });
        $('.uk-icon-calendar.future-date').each(function () {
            var relaxation = $(this).data('relaxation');
            if (typeof relaxation == "undefined") {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: 1, format: 'DD-MM-YYYY' });
            } else {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 1, format: 'DD-MM-YYYY' });
            }
            var self = $(this);
            $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();
                element.trigger('change');
            });
        });

        $('.uk-icon-calendar.future-date-only').each(function () {
            var relaxation = $(this).data('relaxation');
            if (typeof relaxation == "undefined") {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: 2, format: 'DD-MM-YYYY' });
            } else {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 2, format: 'DD-MM-YYYY' });
            }
            var self = $(this);
            $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();
                element.trigger('change');
            });
        });

        $('.uk-icon-calendar.current-date').each(function () {
            var date = UIkit.datepicker(UIkit.$(this), { minDate: 1, maxDate: 0, format: 'DD-MM-YYYY' });
            var self = $(this);
            $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();

            });
        });

        $('.uk-icon-calendar.past-date').each(function () {
            var relaxation = $(this).data('relaxation');
            if (typeof relaxation == "undefined") {
                var date = UIkit.datepicker(UIkit.$(this), { maxDate: 0, format: 'DD-MM-YYYY' });
            } else {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation + 1, maxDate: 0, format: 'DD-MM-YYYY' });
            }

            var self = $(this);
            $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();
                element.trigger('change');
            });
        });

        $('.uk-icon-calendar.all-date').each(function () {
            var relaxation = $(this).data('relaxation');
            if (typeof relaxation == "undefined") {
                var date = UIkit.datepicker(UIkit.$(this), { format: 'DD-MM-YYYY' });
            } else {
                var date = UIkit.datepicker(UIkit.$(this), { minDate: relaxation - 1, maxDate: relaxation + 1, format: 'DD-MM-YYYY' });
            }

            var self = $(this);
            $(this).off('change.uk.datepicker').on('change.uk.datepicker', function (args) {
                var element = $(self).closest('.uk-input-group').find('input.md-input');
                element.val(date.current.format('DD-MM-YYYY'));
                element.focus();
                element.trigger('change');
            });
        });
        $('.time').each(function () {
            $.UIkit.timepicker($(this), { format: '12h' });
        });

        $('.time15').each(function () {
            $.UIkit.timepicker($(this), { format: '12h', step: '15' });

        });
        $("body").on("click", "#btnOkPrint", app.print);
        $("body").on("ifChanged", "#print-settings-content .option", app.set_print_preferences);
        //$("body").on("keydown", ".uk-table input:visible:not([readonly]):not([disabled]), .uk-table select:visible:not([readonly]):not([disabled])", app.key_down);
        //$("body").on("keydown", " :input:visible:not([readonly]):not([disabled]):first", app.last_tab);
        //$("body").on("keydown", " :input:visible:not([readonly]):not([disabled]):last", app.first_tab);
        //$(".btnSendMail").on("click", app.sendmail);

        $('body').on('click keyup', function (e) {
            if ($switcher_horizontal.hasClass('switcher_active')) {
                if (
                    (!$(e.target).closest($switcher_horizontal).length)
                    || (e.keyCode == 27)
                ) {
                    $switcher_horizontal.removeClass('switcher_active');
                }
            }
        });
        $('body').on('click keyup', function (e) {

            if ($switcher_batch.hasClass('switcher_active')) {
                if (
                    (!$(e.target).closest($switcher_batch).length)
                    || (e.keyCode == 27)
                ) {
                    $switcher_batch.removeClass('switcher_active');
                }
            }
        });
        $('#style_switcher_horizontal_toggle').click(function (e) {
            e.preventDefault();
            $switcher_horizontal.toggleClass('switcher_active');
        });
        $('#style_switcher_batch_toggle').click(function (e) {
            e.preventDefault();
            $switcher_batch.toggleClass('switcher_active');
        });

        $('body').on('click keyup', function (e) {
            if ($switcher.hasClass('switcher_active')) {
                if (
                    (!$(e.target).closest($switcher).length)
                    || (e.keyCode == 27)
                ) {
                    $switcher.removeClass('switcher_active');
                }
            }
        });
        $('#style_switcher_toggle').click(function (e) {
            e.preventDefault();
            $switcher.toggleClass('switcher_active');
        });
    },
    sendmail: function (url, ui, id, subject, body, TomailID) {
        $.ajax({
            url: "/Admin/Mail/SendMailToUser",
            data: {

                url: url,
                ui: ui,
                id: id,
                subject: subject,
                body: body,
                TomailID: TomailID
            },
            dataType: "json",
            type: "get",
            success: function (response) {
                if (response == true) {
                    app.show_notice("mail sent successfully");

                } else {
                    //   app.show_error("some error occured");

                }
            }
        });
    },
    first_tab: function (e) {
        if (e.keyCode == 9 && e.shiftKey == false) {
            e.preventDefault();
            $('#page-container :input:visible:not([readonly]):not([disabled]):first').focus();
            $('html, body').animate({
                scrollTop: 0
            }, 500);
        }
    },

    last_tab: function (e) {
        if (e.keyCode == 9 && e.shiftKey != false) {
            e.preventDefault();
            $('#page-container :input:visible:not([readonly]):not([disabled]):last').focus();
            $('html, body').animate({
                scrollBottom: 0
            }, 500);
        }
    },

    key_down: function (e) {
        var $td = $(this).closest("td");
        var index = $td.index();
        var $this_tr = $td.closest("tr");
        var $next_tr;
        if (e.keyCode == 40) {
            $next_tr = $this_tr.next();
            $next_tr.find("td").eq(index).find('input,select').focus();
        } else if (e.keyCode == 38) {
            $next_tr = $this_tr.prev();
            $next_tr.find("td").eq(index).find('input,select').focus();
        }
    },
    masked_inputs: function (wrapper) {
        $date = typeof wrapper != "undefined" ? $(wrapper).find('.date') : $('.date');
        if ($date.length) {
            $date.attr('data-inputmask', "'alias': 'dd-mm-yyyy'");
            $date.attr('data-inputmask-showmaskonhover', "false");
            $date.attr('placeholder', "dd-mm-yyyy");
            $date.inputmask("date", { yearrange: { minyear: 1900, maxyear: 2200 } });
        }
        $qty = typeof wrapper != "undefined" ? $(wrapper).find('.mask-qty') : $('.mask-qty');
        if ($qty.length) {
            $qty.attr('data-inputmask', "'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2,'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $qty.attr('data-inputmask-showmaskonhover', "false");
            $qty.inputmask();
        }

        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-currency') : $('.mask-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large.mask-currency') : $('.x-large.mask-currency');
        if ($currency.length) {
          //  $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'placeholder': '0'");
           $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");

            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-negative-currency') : $('.mask-negative-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'allowMinus': false,'digitsOptional': false, 'prefix': '-', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-sales4-currency') : $('.mask-sales4-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 4, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-sales3-currency') : $('.mask-sales3-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-sales2-currency') : $('.mask-sales2-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-sales4-currency') : $('.x-large .mask-sales4-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 4, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-sales3-currency') : $('.x-large .mask-sales3-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-sales2-currency') : $('.x-large .mask-sales2-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-sales-currency') : $('.x-large .mask-sales-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-USD-currency') : $('.x-large .mask-USD-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': '$', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-AUD-currency') : $('.x-large .mask-AUD-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': 'A$', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.x-large .mask-EUR-currency') : $('.x-large .mask-EUR-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'prefix': '€', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-production-qty') : $('.mask-production-qty');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 4, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-negative-production-qty') : $('.mask-negative-production-qty');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 4, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-postive') : $('.mask-postive');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 0, 'allowMinus': false,'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $currency = typeof wrapper != "undefined" ? $(wrapper).find('.mask-positive-currency') : $('.mask-positive-currency');
        if ($currency.length) {
            $currency.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 3, 'allowMinus': false,'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $currency.attr('data-inputmask-showmaskonhover', "false");
            $currency.inputmask();
        }
        $numeric = typeof wrapper != "undefined" ? $(wrapper).find('.mask-numeric') : $('.mask-numeric');
        if ($numeric.length) {
            $numeric.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': 0, 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
            $numeric.attr('data-inputmask-showmaskonhover', "false");
            $numeric.inputmask();
        }
    },
    change_decimalplaces: function ($input, DecimalPlaces, prefix = '') {
        var existingCurrencyClass = "";
        var newnormalclass = "";
        var classNamesArray = $input.attr('class').split(' ');
        for (var i = 0; i < classNamesArray.length; i++) {
            if (classNamesArray[i].indexOf('mask-') != -1) {
                existingCurrencyClass = classNamesArray[i];
            }
        }
        if ((DecimalPlaces) > 0 || DecimalPlaces > 0) {
            newnormalclass = 'mask-sales' + DecimalPlaces + '-currency';
        }
        else {
            newnormalclass = 'mask-sales2-currency';
        }
        $input.removeClass(existingCurrencyClass);
        $input.addClass(newnormalclass);
        if (prefix.length > 0) {
            if ($input.length) {
                $input.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': " + DecimalPlaces + ", 'digitsOptional': false, 'prefix': '" + prefix + "', 'placeholder': '0'");
                $input.attr('data-inputmask-showmaskonhover', "false");
                $input.inputmask();
            }
        } else {
            if ($input.length) {
                $input.attr('data-inputmask', "'alias': 'currency', 'groupSeparator': ',', 'autoGroup': true, 'digits': " + DecimalPlaces + ", 'digitsOptional': false, 'prefix': '', 'placeholder': '0'");
                $input.attr('data-inputmask-showmaskonhover', "false");
                $input.inputmask();
            }
        }
        return newnormalclass;
    },
    validate: function () {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());

        $('body').on('keydown change', '.md-input-danger', function () {
            $(this).parent('.md-input-wrapper').removeClass('md-input-wrapper-danger');
            $(this).removeClass('md-input-danger');
        });

        $('body').on('blur', 'input.current-date', function () {
            var date = $(this).val().split('-');
            used_date = new Date(date[2], date[1] - 1, date[0]);
            if (used_date.getTime() != today.getTime()) {
                //  app.add_error_class($(this));
            }
        });
        $('body').on('blur', 'input.future-date', function () {
            var date = $(this).val().split('-');
            used_date = new Date(date[2], date[1] - 1, date[0]);
            if (used_date.getTime() <= today.getTime()) {
                //  app.add_error_class($(this));
            }
        });
        $('body').on('blur', 'input.future-date-only', function () {
            var date = $(this).val().split('-');
            used_date = new Date(date[2], date[1] - 1, date[0]);
            if (used_date.getTime() < today.getTime()) {
                //  app.add_error_class($(this));
            }
        });
        $('body').on('blur', 'input.past-date', function () {
            var date = $(this).val().split('-');
            used_date = new Date(date[2], date[1] - 1, date[0]);
            if (used_date.getTime() > today.getTime()) {
                //  app.add_error_class($(this));
            }
        });
    },
    add_error_class: function (element) {
        $(element).addClass('md-input-danger');
        $(element).parent('.md-input-wrapper').addClass('md-input-wrapper-danger');
    },
    alert: function (message) {
        UIkit.modal.alert(message);
    },
    show_error: function (message) {
        UIkit.notify(message, { status: 'danger', pos: 'bottom-right' });
    },
    show_warning: function (message) {
        UIkit.notify(message, { status: 'warning', pos: 'bottom-right' });
    },
    show_notice: function (message) {
        UIkit.notify(message, { status: 'success', pos: 'bottom-right' });
    },
    confirm: function (message, ok) {
        UIkit.modal.confirm(message, function () {
            ok();
        }, { labels: { Ok: 'Ok', Cancel: 'Cancel' }, stack: true });
    },
    confirm_cancel: function (message, ok, cancel) {
        UIkit.modal.confirm(message, function () {
            ok();
        }, function () {
            cancel();
        }, { labels: { Ok: 'Yes', Cancel: 'No' }, stack: true });
    },
    get_locations: function () {
        $.ajax({
            url: '/Account/GetUserLocations',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    CurrentLocationID = response.CurrentLocationID;
                    var select = $("<select class='md-input'></select>");
                    var options = "";
                    $.each(response.Locations, function (i, record) {
                        options += "<option  " + ((record.ID == response.CurrentLocationID) ? "selected = 'selected'" : "") + " value='" + record.ID + "'>" + record.Name + "</option>";
                    });
                    select.append(options);
                    if (response.Locations.length == 1) {
                        select.attr("disabled", "disabled");
                    }
                    $("#current-location").html(select);
                }
            }
        });
    },
    get_date_time: function (date_string) {
        var date = date_string.split('-');
        var used_date = new Date(date[2], date[1] - 1, date[0]);
        return used_date.getTime();
    },
    get_date: function (date_string) {
        var date = date_string.split('-');
        var used_date = new Date(date[2], date[1] - 1, date[0]);
        return used_date.getDate();
    },
    get_date_diff: function (date_string) {
        var date = date_string.split('-');
        var used_date = new Date(date[2], date[1] - 1, date[0]);
        var c_date = new Date();
        var days = parseInt((c_date - used_date) / (1000 * 60 * 60 * 24));
        return days;
    },
    compare_date_time: function (date_time_string1, date_time_string2) {

        console.log(date_time_string1, date_time_string2);
        if (typeof date_time_string1 == "undefined" || date_time_string1.trim() == "") {
            return "blank1";
        }
        if (typeof date_time_string2 == "undefined" || date_time_string2.trim() == "") {
            return "blank2";
        }

        datetime1 = app.string_to_date(date_time_string1);
        datetime2 = app.string_to_date(date_time_string2);
        if (datetime1 == datetime2) {
            return "equal";
        } else if (datetime1 > datetime2) {
            return "greater";
        } else {
            return "lesser";
        }
    },
    string_to_date: function (date_time_string) {
        if (date_time_string.trim() == "") {
            return false;
        }
        var u_date = date_time_string.replace(" ", "-").replace(" ", "-").replace(":", "-").split('-');
        if (u_date.length == 4) {
            u_date[3] = "0";
            u_date[4] = "0";
            u_date[5] = "AM";
        }
        if ((u_date[5] == "AM" || u_date[5] == "am") && u_date[3] == 12) {
            u_date[3] = "0";
        }
        if ((u_date[5] == "PM" || u_date[5] == "pm") && u_date[3] != 12) {
            u_date[3] = (parseInt(u_date[3]) + 12).toString();
        }
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0], u_date[3], u_date[4]);
        console.log(date_time_string, used_date);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime()
    },
    string_to_date_time: function (date_time_string) {
        var time = app.string_to_date(date_time_string);
        if (time != false) {
            var date = new Date();
            date.setTime(time);
            return date;
        }
        return false;

    },
    print_preview: function (url) {
        //var newWindow = window.open(url, "_blank", "width=800, height=600");
        //newWindow.focus();
        var height = $(document).height() - $("#print-content").offset().top - $("#print-preview-common .uk-modal-footer").height() - 100;
        $("#print-content").html("<iframe id='IFramePDF' src='" + url + "'  ></iframe>");
        //$("#print-content").empty().append("<iframe id = 'IFramePDF' src = '" + url + "' ></iframe >");
        $("#print-content").height(height);
        var $IFrame = $("#IFramePDF");
        $IFrame.css({ 'width': '100%', 'height': '100%' });
        $IFrame.load(function () {
            UIkit.modal("#print-preview-common").show();
        });
    },

    print_invoice: function (url) {
        var height = $(document).height() - $("#print-content").offset().top - $("#print-preview-common .uk-modal-footer").height() - 100;
        $("#print-content").html("<iframe id='IFramePrint' src='" + url + "'  ></iframe>");
        $("#print-content").height(height);
        var iframe = $("#IFramePrint");
        iframe.css({ 'width': '100%', 'height': '100%' });
        iframe.load(function () {
            iframe[0].contentWindow.print();
        });
    },

    set_print_preferences: function () {
        var option = $(this).val();
        if ($(this).is(":checked")) {
            app.print_preferences.push(option);
        } else {
            var index = app.print_preferences.indexOf(option);
            if (index > -1) {
                app.print_preferences.splice(index, 1);
            }
        }
    },
    get_print_preferences: function (callback) {
        var is_preferences_saved = $("#saved-preference").val();
        app.print_preferences = [];
        $("#print-settings-content .option").each(function () {
            if ($(this).prop("checked")) {
                app.print_preferences.push($(this).val());
            }
        });
        if (is_preferences_saved == "1") {
            callback();
        } else {
            UIkit.modal("#print-settings").show();
        }
    },
    print: function () {
        var $IFrame = $("#IFramePDF");
        $IFrame.focus();
        $IFrame[0].contentWindow.print();
    },
    update_session: function () {
        $.ajax({
            url: '/Account/UpdateSessionLog',
            dataType: "json",
            type: "POST",
            success: function (response) {
                console.log(response);
            },
        });
    },
    log_performance: function (area, controller, action) {
        var performance = window.performance.timing;
        var date = new Date();
        var id_string = $("#ID").val();
        $.ajax({
            url: '/Account/LogPerformance',
            data: {
                Area: area,
                ControllerName: controller,
                ActionName: action,
                ID: id_string,
                Duration: date.getTime() - performance.requestStart
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                console.log(response);
            },
        });
    },
    show_loading: function () {
        $("#preloader").show();
        altair_helpers.content_preloader_show('md', 'success');
    },
    hide_loading: function () {
        $("#preloader").hide();
        altair_helpers.content_preloader_hide();
    },

    is_valid_date: function (date_string) {
        if (date_string == "") {
            return false;
        }
        var split = date_string.split('-');
        if (split.length != 3) {
            return false;
        }
        var used_date = new Date(split[2], split[1] - 1, split[0]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return true;
    },

};

form = {
    required: function (element) {
        return $(element).val().trim() != "";
    },
    numeric: function (element) {
        return $.isNumeric(clean($(element).val()));
    },
    positive: function (element) {
        return clean($(element).val()) >= 0;
    },
    negative: function (element) {
        return clean($(element).val()) <= 0;
    },
    non_zero: function (element) {
        return clean($(element).val()) != 0;
    },

    future_time: function (element) {

        var now = new Date();

        if ($(element).val().trim() == "") {
            return true;  // not entered
        }
        var u_date = $(element).val().replace(" ", "-").replace(" ", "-").replace(":", "-").split('-');
        if ((u_date[5] == "AM" || u_date[5] == "am") && u_date[3] == 12) {
            u_date[3] = "0";
        }
        if ((u_date[5] == "PM" || u_date[5] == "pm") && u_date[3] != 12) {
            u_date[3] = (parseInt(u_date[3]) + 12).toString();
        }
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0], u_date[3], u_date[4]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime() >= now.getTime();
    },

    future_date: function (element) {

        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        if ($(element).val().trim() == "") {
            return true;  // not entered
        }
        var u_date = $(element).val().split('-');
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime() >= today.getTime();
    },

    future_date_only: function (element) {

        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        if ($(element).val().trim() == "") {
            return true;  // not entered
        }
        var u_date = $(element).val().split('-');
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime() > today.getTime();
    },
    past_date: function (element) {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        if ($(element).val().trim() == "") {
            return true; // not entered
        }
        var u_date = $(element).val().split('-');
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime() <= today.getTime();
    },
    current_date: function (element) {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        if ($(element).val().trim() == "") {
            return true; // not entered
        }
        var u_date = $(element).val().split('-');
        var used_date = new Date(u_date[2], u_date[1] - 1, u_date[0]);
        if (used_date == "Invalid Date") {
            return false;
        }
        return used_date.getTime() == today.getTime();
    },
    validate: function (rules) {
        var message = "";
        var messages = [];
        var error_count = 0;
        $.each(rules, function (i, item) {
            $(item.elements).each(function (j, element) {
                $.each(item.rules, function (k, rule) {
                    if (typeof rule.type == 'function') {
                        if (!rule.type(element)) {
                            app.add_error_class($(element));
                            if (typeof rule.alt_element != "undefined") {
                                app.add_error_class($(rule.alt_element));
                            }
                            if (typeof rule.message != "undefined") {
                                if (messages.indexOf(rule.message) == -1) {
                                    messages.push(rule.message);
                                }
                            }
                            error_count++;
                            return false;
                        }
                    }
                });
            });
        });
        if (messages.length) {
            $.each(messages, function (i, record) {
                message += record + '<br/>';
            });
            app.show_error(message);
            $(".md-input-danger:visible:first").focus();
        }
        return error_count;
    }
}
app.requests = {};
app.jqXHR;
$(function () {
    $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
        if (app.requests[options.url]) {
            app.requests[options.url].abort();
        }
        app.requests[options.url] = jqXHR;
        app.jqXHR = jqXHR;
    });
    $.ajaxSetup({
        beforeSend: function () {
            app.show_loading();
        },
        complete: function (a) {
            app.hide_loading();
            if (typeof app.jqXHR.responseJSON != "undefined" && typeof app.jqXHR.responseJSON.Status != "undefined") {
                if (app.jqXHR.responseJSON.Status == "failure" && app.jqXHR.responseJSON.StatusCode == 302) {
                    $("#LPassword").attr("type", "password");
                    UIkit.modal("#login-popup", { bgclose: false, modal: false, keyboard: false }).show();
                    // app.load_popup();
                }
            } else if (app.jqXHR.getResponseHeader("X-Responded-JSON") != null
                && JSON.parse(app.jqXHR.getResponseHeader("X-Responded-JSON")).status == "401") {
                $("#LPassword").attr("type", "password");
                UIkit.modal("#login-popup", { bgclose: false, modal: false, keyboard: false }).show();
                //app.load_popup();
            }
        },
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            //Download progress
            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                    //console.log(Math.round(percentComplete * 100) + "%");
                }
            }, false);
            return xhr;
        },
        error: function (x, status, error) {


        },


    });
});

$(function ($) {

    $.fn.SelectTable = function (options) {

        var defaults = {
            // These are the defaults.
            currentRow: 0,
            returnFocus: "",
            startFocusIndex: 3,
            selectFunction: "",
            selectionType: "radio",
            modal: "",
            initiatingElement: "",

        };

        // This is the easiest way to have default options.
        var settings = $.extend(defaults, options);

        var slTable = this;

        var inputs = $(slTable).find("input[type='text']");
        if (inputs.length == 0) {
            inputs = $('<input>');
            $(inputs).attr("type", "text");
            $(inputs).css({ width: "1px", height: "1px", padding: "0px" });
            $(slTable).find('thead tr').children().eq(0).append(inputs);
        }

        this.selectRow = function () {
            var rows = $(slTable).find("tbody tr");
            if (settings.currentRow < 0) {
                settings.currentRow = 0;
            }
            if (settings.currentRow >= $(rows).length - 1) {
                settings.currentRow = $(rows).length - 1;
            }
            if (settings.selectionType == "radio") {
                $(rows).eq(settings.currentRow).find('.iCheck-helper').trigger("click");
            } else {
                $(rows).removeClass("st-selected");
                $(rows).eq(settings.currentRow).addClass("st-selected");
                if (rows.length == 1) {
                    $(rows).eq(settings.currentRow).find('.iCheck-helper').trigger("click");
                }
            }

        }
        this.setFocus = function () {
            if (settings.startFocusIndex >= $(inputs).length) {
                settings.startFocusIndex = $(inputs).length - 1;
            }
            $(inputs).eq(settings.startFocusIndex).focus();
        }
        this.getKeyCode = function (e) {
            var keyCode = e.keyCode || e.which;
            switch (keyCode) {
                case 32: // space
                    var rows = $(slTable).find("tbody tr");
                    $(rows).eq(settings.currentRow).find('.iCheck-helper').trigger("click");
                    break;
                case 33: // page up
                    $(slTable).trigger('previous.page');
                    break;
                case 34: // page down
                    $(slTable).trigger('next.page');
                    break;
                case 37: // left
                    break;
                case 38: // up
                    settings.currentRow--;
                    slTable.selectRow();
                    break;
                case 39: // right
                    break;
                case 40: // down
                    settings.currentRow++;
                    slTable.selectRow();
                    break;
                case 13: // enter
                    if (typeof settings.selectFunction == "function") {
                        settings.selectFunction();
                        setTimeout(function () {
                            $(settings.returnFocus).focus();
                        }, 100);
                        if (settings.modal != "") {
                            UIkit.modal($(settings.modal)).hide();
                        }
                    }
                    break;
                default: return; // exit this handler for other keys
            }
        };

        this.getRow = function (e) {
            var tr = $(e.target).closest('tr');
            settings.currentRow = $(tr).index();
            if (settings.selectionType == "radio") {
                slTable.selectRow();
            } else {
                var rows = $(slTable).find("tbody tr");
                $(rows).eq(settings.currentRow).find('.iCheck-helper').trigger("click");
            }

        };

        this.getParentRow = function (e) {
            var rows = $(slTable).find("tbody tr");
            var tr = $(e.target).closest('tr');
            settings.currentRow = $(tr).index();
            $(rows).removeClass("st-selected");
            $(rows).eq(settings.currentRow).addClass("st-selected");
        };
        this.chooseRow = function (e) {
            var tr = $(e.target).closest('tr');
            settings.currentRow = $(tr).index();
            slTable.selectRow();
            if (settings.selectionType != "radio") {
                var rows = $(slTable).find("tbody tr");
                $(rows).eq(settings.currentRow).find('.iCheck-helper').trigger("click");
            }
            if (typeof settings.selectFunction == "function") {
                settings.selectFunction();
                setTimeout(function () {
                    $(settings.returnFocus).focus();
                }, 100);
                if (settings.modal != "") {
                    UIkit.modal($(settings.modal)).hide();
                }
            }


        }
        this.refresh = function () {
            $(slTable).find("tbody tr").off('click').on('click', this.getRow);
            $(slTable).find("tbody tr").off('dblclick').on('dblclick', this.chooseRow);
            $(slTable).find("tbody tr input[type='radio']").on('ifChanged', this.getParentRow);
            slTable.selectRow();
        };
        $(slTable).on('datatable.changed', function () {
            settings.currentRow = 0;
            slTable.refresh();
        });
        if (settings.modal != "") {
            $(settings.modal).off('show.uk.modal').on('show.uk.modal', function () {
                slTable.setFocus();
                slTable.refresh();
            });
            $(settings.modal).off('hide.uk.modal').on('hide.uk.modal', function () {
                $(inputs).each(function () {
                    if ($(this).val() != '') {
                        $(this).val('').trigger("textchange");
                    }
                });
                $(settings.returnFocus).focus();
            });
        }
        if (settings.initiatingElement != "") {
            $(settings.initiatingElement).keypress(function (e) {
                if (e.which == 13 && settings.modal != "") {
                    UIkit.modal(settings.modal, { center: false }).show();
                }
            });
        }
        $(inputs).keydown(this.getKeyCode);
        return slTable;
    };

}(jQuery));

(function ($) {

    $.event.special.textchange = {

        setup: function (data, namespaces) {
            $(this).data('lastValue', this.contentEditable === 'true' ? $(this).html() : $(this).val());
            $(this).bind('keyup.textchange', $.event.special.textchange.handler);
            $(this).bind('cut.textchange paste.textchange input.textchange', $.event.special.textchange.delayedHandler);
        },

        teardown: function (namespaces) {
            $(this).unbind('.textchange');
        },

        handler: function (event) {
            $.event.special.textchange.triggerIfChanged($(this));
        },

        delayedHandler: function (event) {
            var element = $(this);
            setTimeout(function () {
                $.event.special.textchange.triggerIfChanged(element);
            }, 25);
        },

        triggerIfChanged: function (element) {
            var current = element[0].contentEditable === 'true' ? element.html() : element.val();
            if (current !== element.data('lastValue')) {
                element.trigger('textchange', [element.data('lastValue')]);
                element.data('lastValue', current);
            }
        }
    };

    $.event.special.hastext = {

        setup: function (data, namespaces) {
            $(this).bind('textchange', $.event.special.hastext.handler);
        },

        teardown: function (namespaces) {
            $(this).unbind('textchange', $.event.special.hastext.handler);
        },

        handler: function (event, lastValue) {
            if ((lastValue === '') && lastValue !== $(this).val()) {
                $(this).trigger('hastext');
            }
        }
    };

    $.event.special.notext = {
        setup: function (data, namespaces) {
            $(this).bind('textchange', $.event.special.notext.handler);
        },

        teardown: function (namespaces) {
            $(this).unbind('textchange', $.event.special.notext.handler);
        },

        handler: function (event, lastValue) {
            if ($(this).val() === '' && $(this).val() !== lastValue) {
                $(this).trigger('notext');
            }
        }
    };

})(jQuery);

function clean(value) {
    try {
        value = value.trim();
        if (value == "") {
            return 0;
        }
        value = value.replace("₹", "", "g")
        return parseFloat(value.replace(/\,/g, ''));
    } catch (e) {
        return 0;
    }

}

$(function ($) {
    $.fn.FreezeHeader = function (options) {
        if (this.length == 0) {
            return;
        }
        var self = this;

        var top = this.offset().top;
        if (top == 0) {
            top = $(this).closest('div').offset().top;
        }
        var height = $(window).height() - top - 42;

        if (height < 200) {
            height = 200;
        }
        var defaults = {
            height: height,
        };
        var settings = $.extend(defaults, options);
        var elementID = self.attr("id");
        var wrapperID = "fh-" + elementID;
        var scrollWrapperID = "sfh-" + elementID;
        var cloneHead = self.find("thead").clone();

        // $(cloneHead).find('th').each(function () { $(this).removeAttr('class') });
        self.fhHead = $("<table class='fh-head'></table>");
        self.fhHead.html(cloneHead);
        self.fhHead.find('input[type="checkbox"]').attr("data-md-icheck", "");
        app.format(self.fhHead);
        self.fhHead.addClass(self.attr("class"));

        self.wrap("<div id='" + scrollWrapperID + "'></div>");
        self.scrollWrapper = $('#' + scrollWrapperID);
        self.scrollWrapper.wrap("<div id='" + wrapperID + "'></div>");

        var wrapper = $('#' + wrapperID);
        wrapper.prepend(self.fhHead);
        wrapper.css({ "position": "relative" });
        self.find("thead").css({ "visibility": "hidden" });
        self.css({ "margin": 0 });

        self.resizeHeader = function (scrolldown) {
            self.scrollWrapper.css({ "overflow-y": "scroll", "height": settings.height });
            self.fhHead.css({ "position": "absolute", "top": "0px", "z-index": 99 });
            self.find("thead th").each(function (i) {
                // if (i != self.find("thead th").length - 1) {
                self.fhHead.find("th").eq(i).css({ "width": $(this).width() });
                // }
            });

            self.fhHead.css({ width: self.width() });

            if (typeof scrolldown == "undefined") {
                scrolldown = true;
            }

            if (scrolldown) {
                self.scrollWrapper.animate({ scrollTop: self.scrollWrapper.prop("scrollHeight") }, 50);
            }

            if (self.fhHead.height() != self.find("thead").height()) {
                self.css({ "margin-top": self.fhHead.height() - self.find("thead").height() });
            }
        }
        $(window).on("resize", function (e) {

        });
        self.resizeHeader();
        return self;
    };
}(jQuery));

jQuery.fn.insertAt = function (index, element) {
    var lastIndex = this.children().size();
    if (index == 0) {
        this.prepend(element);
    } else if (lastIndex <= index) {
        this.append(element);
    } else {
        this.children().eq(index).after(element);
    }
    return this;
}

Number.prototype.roundTo = function (numberOfDigits) {
    return parseFloat(this.valueOf().toFixed(numberOfDigits));
};
