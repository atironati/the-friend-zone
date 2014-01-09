$(document).ready(function () {
    $('#searchForFriendsInput').select(function () {
        $("#searchForFriendsInput").css("color", "black");
        $("#searchForFriendsInput").val("");
    });
    $("#searchForFriendsInput").focus(function () {
        $("#searchForFriendsInput").css("color", "black");
        $("#searchForFriendsInput").val("");
    }).blur(function () {
        if (e.pageY < 100) {

        } else {
            $("#searchForFriendsInput").css("color", "#cccccc");
            $("#searchForFriendsInput").val("Find Friends!");
        }
    });
});