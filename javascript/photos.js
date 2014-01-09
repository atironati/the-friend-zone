$(document).ready(function () {
    if ($("#photoViewerText").text().trim().length > 1) {
        $("#grayout").css({
            opacity: 0,
            display: 'inline-block'
        }).animate({ opacity: 1 }, 600);
    } else {
        $("#grayout").css("display", "none");
    };


    $("#grayout").click(function () {
        $("#photoViewerText").text("")
        $("#grayout").css({
            opacity: 1,
            display: 'inline-block'
        }).animate({ opacity: 0 }, 600, function () {
            $("#grayout").css("display", "none");
        });
    });
});

$(window).bind("load", function () {
    if ($("#profilePhoto").width() > 300) {
        $("#profilePhoto").width(300);
    }
});