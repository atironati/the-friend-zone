$(document).ready(function () {
    $("#header_right").css("width", window.innerWidth - 417);
    if (window.innerWidth > 1024) {
        var shiftValue = ((window.innerWidth - 1275) / 2) + 360;
        if(shiftValue < 375)
            $("#profileRightSideContainer").css("left", 375);
        else
            $("#profileRightSideContainer").css("left", shiftValue);
    }
});

$(window).resize(function () {
    $("#header_right").css("width", window.innerWidth - 417);

    if (window.innerWidth > 1024) {
        var shiftValue = ((window.innerWidth - 1275) / 2) + 360;
        if (shiftValue < 375)
            $("#profileRightSideContainer").css("left", 375);
        else
            $("#profileRightSideContainer").css("left", shiftValue);
    }
});

$(window).bind("load", function () {
    if ($("#profilePhoto").width() > 300) {
        $("#profilePhoto").width(300);
    }
});