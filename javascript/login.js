$(document).ready(function () {
    if ($("#loginErrorBoxText").text().trim().length > 1) {
        $("#loginErrorBox").css({
            opacity: 0,
            display: 'inline-block'
        }).animate({ opacity: 1 }, 600);
    } else {
        $("#loginErrorBox").css("display", "none");
    };
    if ($("#createAccountErrorBoxText").text().trim().length > 1) {
        $("#createAccountErrorBox").css({
            opacity: 0,
            display: 'inline-block'
        }).animate({ opacity: 1 }, 600);
    } else {
        $("#createAccountErrorBox").css("display", "none");
    };
});