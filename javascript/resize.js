$(document).ready(function () {
    $("#header_right").css("width", window.innerWidth - 417);
});

$(window).resize(function () {
    $("#header_right").css("width", window.innerWidth - 417);
});