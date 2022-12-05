function showhide() {
    var x = document.getElementById("txtPassword");
    var y = document.getElementById("show");
    var z = document.getElementById("hide");

    if (x.type === 'password') {
        x.type = "text";
        y.style.display = "block";
        z.style.display = "none";
    }
    else {
        x.type = "password";
        y.style.display = "none";
        z.style.display = "block";
    }
}


function check() {
    var username = document.getElementById("txtUserId").value;
    var password = document.getElementById("txtPassword").value;
    if (username != "" && password != "") {
        document.getElementById("btnLogin").value = "Checking...";
    }
    else {
        alert('Please enter User Id and Password.');
        event.preventDefault();
    }
}