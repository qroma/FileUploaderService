﻿//$('#submibBtn').on('click', function () {
//    console.log("log111");
//    $.ajax({
//        // Your server script to process the upload
//        url: '/home/upload',
//        type: 'POST',

//        // Form data
//        data: new FormData($('#uploadForm')),

//        // Tell jQuery not to process data or worry about content-type
//        // You *must* include these options!
//        cache: false,
//        contentType: false,
//        processData: false,

//        // Custom XMLHttpRequest
//        xhr: function () {
//            var myXhr = $.ajaxSettings.xhr();
//            if (myXhr.upload) {
//                // For handling the progress of the upload
//                myXhr.upload.addEventListener('progress', function (e) {
//                    if (e.lengthComputable) {
//                        $('progress').attr({
//                            value: e.loaded,
//                            max: e.total,
//                        });
//                    }
//                }, false);
//            }
//            return myXhr;
//        }
//    });
//});

//$(':file').on('change', function () {
//    var file = this.files[0];

//    if (file.size > 1024) {
//        alert('max upload size is 1k');
//    }

//    // Also see .name, .type
//});