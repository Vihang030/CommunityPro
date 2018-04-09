
$('#Right').click(function (e) {
    var selectedOpts = $('#selectedQualifications option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#availQualifications').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#Left').click(function (e) {
    var selectedOpts = $('#availQualifications option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#selectedQualifications').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});



$('#btnSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
    $('#selectedQualifications option').prop('selected', true);
});
