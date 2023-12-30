$(document).ready(function () {
    $('.select2').select2({
        theme: 'bootstrap4',
        language: document.getElementById('ActiveLanguage').value.substring(0, 2)
    });

    $('#user-birthdate').datetimepicker({ icons: { time: 'far fa-clock' } });
})

UpdateGeneralInfo = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                toastr.success(res.message)
            },
            error: function (xhr, err) {
                var err = eval("(" + xhr.responseText + ")");

                toastr.error(err.message)
            }
        })
    } catch (e) {
        console.log(e)
    }

    return false
}

UpdateSecurityInfo = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                toastr.success(res.message)
            },
            error: function (xhr, err) {
                var err = eval("(" + xhr.responseText + ")");

                toastr.error(err.message)
            }
        })
    } catch (e) {
        console.log(e)
    }

    return false
}
