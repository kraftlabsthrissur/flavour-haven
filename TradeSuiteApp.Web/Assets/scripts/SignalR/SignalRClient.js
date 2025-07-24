var SignalRClient = {

    cookie_name: "SignalRUID",

    hub: "serverHub",

    init: function () {
        var self = SignalRClient;
        if (self.get_cookie(self.cookie_name) == "") {
            self.set_cookie(self.cookie_name, "BEGIN");
        }
        self.connect();
    },

    connect: function () {
        var self = SignalRClient;
        self.connection = $.connection.serverHub;
        self.server = self.connection.server;
        self.connection.client.onTickerReceived = function (type, html) {
            console.log("OnTickerReceived", type);
            $html = $(html);
            app.format($html);
            $("#tickers").html($html);
        }
        self.connection.client.onNotificationReceived = function (title, text) {
            console.log("OnNotificationReceived", title, text);
        }
        self.connection.client.error = function (message) {
            console.log("error", message);
        }
        self.connection.client.getToken = function (message) {
            console.log("getToken", message);
        }
    },

    print: {
        cookie_name: "PrintUID",

        text_file: function (file) {
            var self = SignalRClient;
            if (file == "") {
                return false;
            }
            var cookie = self.get_cookie(self.print.cookie_name);
            if (cookie == "" || cookie == "null") {
                self.show_cookie_popup(self.print.cookie_name);
                return false;
            }
            $.connection.hub.start().done(function () {
                self.connection_id = self.connection.connection.id;
                self.set_cookie(self.cookie_name, self.connection_id);
                self.server.printTextFile(file);
            });
        },

    },

    notification: {
        cookie_name: "NotificationUID",

        send: function (title, text, user_ids) {
            var self = SignalRClient;
            $.connection.hub.start().done(function () {
                self.connection_id = self.connection.connection.id;
                self.set_cookie(self.cookie_name, self.connection_id);
                self.server.sendNotification(title, text, user_ids);
            });

        },

        tickers: function (type, html, user_ids) {
            var self = SignalRClient;
            $.connection.hub.start().done(function () {
                self.connection_id = self.connection.connection.id;
                self.set_cookie(self.cookie_name, self.connection_id);
                self.server.sendTickers(type, html, user_ids);
            });
        },

        connect: function () {
            var self = SignalRClient;
            $.connection.hub.start().done(function () {
                self.connection_id = self.connection.connection.id;
                self.set_cookie(self.cookie_name, self.connection_id);
            });
        }
    },

    set_cookie: function (cookie_name, cookie_value) {
        var self = SignalRClient;
        var date = new Date();
        var expiry_date = date.setFullYear(date.getFullYear() + 1);
        document.cookie = cookie_name + "=" + cookie_value + "; expires=" + expiry_date + "; path=/";
    },

    get_cookie: function (cookie_name) {
        var self = SignalRClient;
        return self.search_cookie(cookie_name);
    },

    clear_cookie: function (cookie_name) {
        var self = SignalRClient;
        document.cookie = cookie_name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    },

    search_cookie: function (cookie_name) {
        var match = document.cookie.match(new RegExp('(^| )' + cookie_name + '=([^;]+)'));
        if (match) {
            return match[2];
        }
        else {
            return "";
        }
    },

    show_cookie_popup: function (cookie_name) {
        var self = SignalRClient;
        if ($("#cookie-popup").length == 0) {
            var modal = '<div class="uk-modal " id="cookie-popup" aria-hidden="true" style="display: none; overflow-y: scroll;">'
                    + '<div class="uk-modal-dialog">'
                    + ' <div class="uk-modal-header">'
                    + '  <h3 class="uk-modal-title">Enter print token<i class="material-icons" ></i></h3>'
                    + ' </div>'
                    + ' <div class="uk-modal-content">'
                    + '  <div class="uk-form-row">'
                    + '   <div class="uk-width-medium-6-10">'
                    + '    <label>&nbsp;</label>'
                    + '    <input type="text" class = "md-input label-fixed" id="token-number">'
                    + '   </div>'
                    + '  </div>'
                    + '  <br/>'
                    + ' </div>'
                    + ' <div class="uk-modal-footer uk-text-right">'
                    + '  <button type="button" class="md-btn uk-modal-close">Close</button>'
                    + '  <input type="hidden" value="' + cookie_name + '" id="cookie-name">'
                    + '  <button type="button" class="md-btn md-btn-primary uk-modal-close" id="btnOkTokenNumber">Ok</button>'
                    + ' </div>'
                    + '</div>'
                    + '</div>';
            $("body").append(modal);
            $("body").on("click", "#btnOkTokenNumber", self.set_token_number);
        } else {
            $("#cookie-name").val(cookie_name);
        }
        UIkit.modal("#cookie-popup").show();
    },

    set_token_number: function () {
        var self = SignalRClient;
        var cookie_name = $("#cookie-name").val();
        var token_number = $("#token-number").val();
        self.set_cookie(cookie_name, token_number);
    }
}