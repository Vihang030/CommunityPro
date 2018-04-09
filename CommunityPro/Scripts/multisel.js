$('#btnRight').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnLeft').click(function (e) {
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#Right').click(function (e) {
    var selectedOpts = $('#selectedSkills option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#availSkills').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#Left').click(function (e) {
    var selectedOpts = $('#availSkills option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#selectedSkills').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});



$('#qRight').click(function (e) {
    var selectedOpts = $('#selectedQualifications option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#availQualifications').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#qLeft').click(function (e) {
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
    $('#selectedSkills option').prop('selected', true);
    $('#selectedQualifications option').prop('selected', true);
});
