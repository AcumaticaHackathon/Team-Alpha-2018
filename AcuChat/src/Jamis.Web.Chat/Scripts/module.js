
(function ($) {
    var siteIconUrl = $('head').find('link[rel=icon]').attr('href');

    function Module(name, baseUrl, widgetUrl, stylesUrls, scriptUrls) {
        this.baseUrl = baseUrl;

        this.name = name;

        var self = this;

        this.log = function (message) {
            console.log("['" + name + "']: " + message);
        };

        this.loadScript = function (scripts) {
            if (scripts.length > 0) {
                var script = scripts.shift();

                $.getScript(baseUrl + script, function () {
                    self.log('Script "' + script + '" loaded');

                    self.loadScript(scripts);
                });
            }
        };

        this.loadStyles = function (urls) {
            for (var i = 0; i < urls.length; i++) {
                var url = urls[i];
                $("<link/>", {
                    rel: "stylesheet",
                    type: "text/css",
                    href: url.startsWith("http") ? url : baseUrl + url
                }).appendTo("head");

                this.log('Style "' + url + '" loaded');
            }
        };

        this.loadWidget = function (url, onComplete) {
            $("<div />").prependTo("body").load(url.startsWith("http") ? url : baseUrl + url, function () {
                self.log('Widget "' + url + '" loaded');
                if (onComplete) onComplete();
            });
        };

        this.resolveUrl = function (url) {
            return url.startsWith("http") ? url : baseUrl + url;
        }

        this.loadStyles(stylesUrls);

        this.loadWidget(widgetUrl, function () {
            self.loadScript(scriptUrls);
        });
    }

    $(document).ready(function () {
        $.chatModule = new Module(
            'JAMIS',
            (siteIconUrl != null) ? siteIconUrl.substring(0, siteIconUrl.indexOf('Icons/site.') - 1) : '',
            '/widgets/jamis/chat/widget.html',
            ['/widgets/jamis/chat/widget.css'],
            ['/scripts/jquery.animation.js',
             '/scripts/jquery.signalR-2.2.1.js',
             '/scripts/jquery.ui-1.12.1.js',
             '/signalr/hubs',
             '/widgets/jamis/chat/widget.js']);
    });

})(jQuery);

//# sourceURL=jamis.web.chat.js