$('form#uploadForm').submit(function (e) {
    e.preventDefault();

    var input = document.getElementById('fileinput'); 
    var files = input.files;
    var formData = new FormData();
    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }

    if (validateFile(input)) {
        var actionurl = e.currentTarget.action;
        $.ajax({
            url: actionurl,
            type: 'post',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data){
                bodyAppend("p", "Sucess: Response - " + data);
            },
            error: function (error) {
                bodyAppend("p", "Error: Response - " + error);
            }
        });
        
    }
});

function validateFile(input) {   

    document.getElementById('results-section').style.display = 'block';

    if (!window.FileReader) {
        bodyAppend("p", "The file API isn't supported on this browser yet.");
        return;
    }

    //var input = document.getElementById('fileinput'); 

    if (!input) {
        bodyAppend("p", "Um, couldn't find the fileinput element.");
    }
    else if (!input.files) {
        bodyAppend("p", "This browser doesn't seem to support the `files` property of file inputs.");
    }
    else if (!input.files[0]) {
        bodyAppend("p", "Please select a file before clicking 'Load'");
    }
    else {
        var file = input.files[0];
        var sizeInMB = (file.size / (1024 * 1024)).toFixed(2);

        if (sizeInMB > 1) {  
            bodyAppend("p", "File " + file.name + " is " + file.size + " bytes in size (" + sizeInMB + " megabytes in size)" + "- File isn't valid");
            return false;
        }
        else {                      
            return true;
        }
        
        var filename = file.val();
        var parts = filename.split('.');
        var extension = parts[parts.length - 1];

        if (extension === "csv" || extension === "xml") {
            bodyAppend("p", "File " + file.name + " is " + file.size + " bytes in size (" + sizeInMB + " megabytes in size) " + "with format " + extension + " - File is valid");
            return true;
        }
        else {
            bodyAppend("p", "File " + file.name + " is " + file.size + " bytes in size (" + sizeInMB + " megabytes in size)" + "- File isn't valid");
            return false;
        }
    }             
}

function bodyAppend(tagName, innerHTML) {
    var div = document.getElementById('results');
    var elm = document.createElement(tagName);
    elm.innerHTML = innerHTML;
    div.appendChild(elm);
}