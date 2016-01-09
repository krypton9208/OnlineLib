$(document).ready(function () {
    $('#file').on('change', function () {
        var value = $(this).val();
        var filename = value.split("\\").pop();
        filename = filename == value ? value.split("/").pop() : filename;
        $('.fakebrowse .btn').text("Browse... " + filename);
    });
});