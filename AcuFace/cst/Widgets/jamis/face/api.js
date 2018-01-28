//sourceURL=face-api.js
(function ($, module)
{
    window.top.FaceApi = function ()
    {
        var localMediaStream = null;
        var onComplete = null;


        var video = document.querySelector("#face-api-video");
        var image = document.querySelector("#face-api-image");
        var canvas = document.querySelector("#face-api-canvas");
        var store = document.querySelector("#face-api-store");
        var menu = document.querySelector("#face-api-menu");
        var slider = new Slider(document.getElementById("face-api-window"));

        var self = this;

        $("#face-api-close").click(doCancel);

        $(video).click(doCapture);

        $(image).click(doConfirm);

        this.open = function (callBack)
        {
            onComplete = callBack;
            this.data = null;
            doStart()
        }

        this.close = function ()
        {
            doClose();
        }

        this.start = function (callBack)
        {
            if (navigator)
            {
                navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;

                if (!!(navigator.getUserMedia))
                {
                    navigator.getUserMedia(
                        {
                            video: true,
                            audio: false
                        },
                        function (stream)
                        {
                            video.src = window.URL.createObjectURL(stream);
                            localMediaStream = stream;
                            if (callBack) callBack();
                        },
                        function (e)
                        {
                            module.log("Rejected video capturing!");
                        });
                } else
                {

                    module.log("Browser or dosn't support video capturing or no camera has been detected!");
                }
            }
        }

        this.stop = function ()
        {

            if (localMediaStream)
            {
                var tracks = localMediaStream.getTracks();
                for (var i = 0; i < tracks.length; i++)
                {
                    tracks[i].stop()
                }
            }

            video.pause();
            video.src = '';
            localMediaStream = null;
        }

        this.capture = function ()
        {
            if (localMediaStream)
            {
                var ctx = canvas.getContext('2d');
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                ctx.drawImage(video, 0, 0);
                image.src = canvas.toDataURL('image/webp');
                this.data = canvas.toDataURL('image/png');
                module.log("done capturing a picture!");
                return this.data;
            }

            return null;
        }

        function doStart()
        {
            slider.GoTo(0);
            self.start(function ()
            {
                $("#face-api-widget").addClass("active")
            });
        }

        function doCancel()
        {
            $("#face-api-widget").removeClass("active");
            self.stop();
            self.data = null;
            if (onComplete) onComplete(self);
        }

        function doClose()
        {
            $("#face-api-widget").removeClass("active");
            self.stop();
            if (onComplete) onComplete(self);
        }

        function doCapture()
        {
            self.capture();
            slider.GoTo(1);
        }

        function doConfirm()
        {
            setTimeout(doClose, 1000);
        }
    }

})(jQuery, jQuery.faceModule);

//# sourceURL=face-api.js