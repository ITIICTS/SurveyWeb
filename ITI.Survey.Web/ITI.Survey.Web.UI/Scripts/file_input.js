$(function () {

    var App = {

        init: function () {

            App.attachListeners();

        },

        debugApp: function (msg) {

            if (true) {

                debugCon = $("#codeResult");

                debugCon.text(debugCon.text() + " , " + msg);

            }

        },

        attachListeners: function () {

            var self = this;



            $("#startDecode").on("click", function (e) {

                var input = $("#barcodeFileInput");

                alert("test");
                if (input[0].files && input[0].files.length) {

                    var tmpImgURL = URL.createObjectURL(input[0].files[0]);

                    App.decode(tmpImgURL);

                }

            });



            $("#barcodeFileInput").on("change", function (e) {

                c
                alert("test 2");
                $("#codeResult").text(" ");

                var files = e.target.files;
                if (files && files.length > 0) {

                    file = files[0];
                    try {

                        var URL = window.URL || window.webkitURL;

                        var imgURL = URL.createObjectURL(file);

                        $("#picturePreview").attr("src", imgURL);

                    }

                    catch (e) {

                        try {

                            var fileReader = new FileReader();

                            fileReader.onload = function (event) {

                                $("#picturePreview").attr("src", event.target.result);

                            };

                            fileReader.readAsDataURL(file);

                        }
                        catch (e) {

                            $("#codeResult").text("Fail !!! createObjectURL and FileReader are not supported by browser");

                        }

                    }

                }

            });



        },
        detachListeners: function () {

            $("#startDecode").off("click");

        },



        decode: function (src) {

            Quagga.decodeSingle(

                {

                    inputStream: {

                        size: 640,

                        singleChannel: false

                    },

                    locator: {

                        patchSize: "large",

                        halfSample: false

                    },

                    decoder: {

                        readers: ["upc_reader", "code_128_reader", "code_39_reader", "code_39_vin_reader", "ean_8_reader", "ean_reader", "upc_e_reader", "codabar_reader"]

                    },

                    locate: true,
                    src: src

                },

                function (result) {

                    if (result && result.codeResult && result.codeResult.code) {

                        $("#codeResult").text('Code - ' + result.codeResult.code);

                    } else {

                        $("#codeResult").text("unable to read");
                    }

                });

        }

    };

    App.init();

});
