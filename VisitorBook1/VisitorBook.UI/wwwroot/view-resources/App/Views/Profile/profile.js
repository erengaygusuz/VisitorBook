$(document).ready(function () {

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

$('#UpdateGeneralInfoForm').validate({
    submitHandler: function (form) {
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
    },
    rules: {
        name: {
            required: true,
            minlength: 3,
            maxlength: 100
        },
        surname: {
            required: true,
            minlength: 3,
            maxlength: 100
        },
        birthDate: {
            required: true
        },
        phoneNumber: {
            required: true,
            phoneUS: true
        },
        gender: {
            required: true
        }
    },
    messages: {
        name: {
            required: "Lütfen ad alanını boş bırakmayınız",
            minlength: jQuery.validator.format("Adınız en az {0} karakter olmalı"),
            maxlength: jQuery.validator.format("Adınız en fazla {0} karakter olabilir")
        },
        surname: {
            required: "Lütfen soyad alanını boş bırakmayınız",
            minlength: jQuery.validator.format("Soyadınız en az {0} karakter olmalı"),
            maxlength: jQuery.validator.format("Soyadınız en fazla {0} karakter olabilir")
        },
        birthDate: {
            required: "Lütfen doğum tarihi alanını boş bırakmayınız"
        },
        phoneNumber: {
            required: "Lütfen telefon numarası alanını boş bırakmayınız",
            phoneUS: "Lütfen geçerli bir telefon numarası giriniz"
        },
        gender: {
            required: "Lütfen cinsiyet alanını boş bırakmayınız"
        }
    },
    errorElement: 'span',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.col-sm-10').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
});

$('#UpdateSecurityInfoForm').validate({
    submitHandler: function (form) {
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
    },
    rules: {
        passwordOld: {
            required: true
        },
        passwordNew: {
            required: true
        },
        passwordNewConfirm: {
            required: true,
            equalTo: "#passwordNew"
        }
    },
    messages: {
        passwordOld: {
            required: "Lütfen eski şifre alanını boş bırakmayınız",
        },
        passwordNew: {
            required: "Lütfen yeni şifre alanını boş bırakmayınız"
        },
        passwordNewConfirm: {
            required: "Lütfen yeni şifre tekrar alanını boş bırakmayınız",
            equalTo: "Girmiş olduğunuz yeni şifreler birbiriyle eşleşmiyor"
        }
    },
    errorElement: 'span',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.col-sm-10').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
});