var menu = document.getElementById('sidebar');
document.onclick = function (div) {
    if (div.target.id == 'blur') {
        document.getElementById('check').checked = false;
        toggle();
    }
}
function toggle() {
    document.body.classList.toggle('blur');
}