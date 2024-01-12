$(document).ready(function () {

    bsCustomFileInput.init();

    $('[data-mask]').inputmask()

    $('.select2').select2({
        theme: 'bootstrap4',
        language: document.getElementById('ActiveLanguage').value.substring(0, 2)
    });

    $('#user-birthdate').datetimepicker(
        {
            icons:
            {
                time: 'far fa-clock'
            },
            locale: document.getElementById('ActiveLanguage').value
        });
})

function cityChange() {
    if ($('#citylist option:selected').val() == 0) {
        $('#countylist-box').hide()
    } else {
        $('#countylist-box').show()

        var cityId = $('#citylist option:selected').val()

        fillCountyList(cityId, 0)
    }
}

function fillCountyList(cityId, selectedIndex) {

    $.ajax({
        type: 'GET',
        url: '/app/county/getallbycity?cityId=' + cityId,
        success: function (res) {

            $('#countylist').empty()

            var countySelectionText = document.getElementById(
                'AddOrEditModalCountySelectionText'
            ).value

            $('#countylist').append(
                $('<option disabled value="0">' + countySelectionText + '</option>')
            )

            $.each(res.data, function (index, value) {
                $('#countylist').append(
                    $('<option value="' + value.id + '">' + value.name + '</option>')
                )
            })

            $('#countylist option:eq(' + selectedIndex + ')').prop(
                'selected',
                true
            )
        },
        error: function (err) {
            console.log(err)
        },
    })
}