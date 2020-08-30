function checkFileSize() {
    var input, file;

    document.getElementById('results-section').style.display = 'block';

    if (!window.FileReader) {
        bodyAppend("p", "The file API isn't supported on this browser yet.");
        return;
    }

    input = document.getElementById('fileinput'); 

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
        file = input.files[0];
        var sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
        bodyAppend("p", "File " + file.name + " is " + file.size + " bytes in size (" + sizeInMB + " megabytes in size)");

        if (sizeInMB > 1) {           
            return false;
        }
        else {           
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