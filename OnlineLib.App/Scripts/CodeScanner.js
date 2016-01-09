$(document).ready(function() {
    $("#bookid").focus();
    $("#bookid").on("keyup", function () {
        if ($(this).val().length == 13) {
            $("#libUserGuid").focus();
        }
    });
    $("#libUserGuid").on("keyup", function () {
        if ($(this).val().length == 16) {
            $(this).closest("form").submit();
        }
    });
});