$(document).ready(function () {
    $("#relationshipEditingBox").val($("#profileRelationshipStatusText").text());
    $("#aboutMeEditingBox").val($("#profileAboutMeText").text());
    $("#interestsEditingBox").val($("#profileInterestsText").text());

    if ($("#profilePhoto").width() > 300) {
        $("#profilePhoto").width(300);
    }
});

$(window).bind("load", function () {
    if ($("#profilePhoto").width() > 300) {
        $("#profilePhoto").width(300);
    }
});