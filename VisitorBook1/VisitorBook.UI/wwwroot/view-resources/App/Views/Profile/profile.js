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


var message = {
    en: {
        required: "This field is required",
        remote: "Please fix this field",
        email: "Please enter a valid e-mail",
        url: "Please enter a valid URL",
        date: "Please enter a valid date",
        dateISO: "Please enter a valid date (ISO)",
        number: "Please enter a valid number",
        digits: "Please enter only digits",
        creditcard: "Please enter a valid credit card number",
        equalTo: "Please enter the same value again",
        extension: "Please enter a valid extension",
        phone: "Please enter a valid phone number.",
        maxlength: $.validator.format("Please enter no more than {0} characters"),
        minlength: $.validator.format("Please enter at least {0} characters"),
        rangelength: $.validator.format("Please enter a value between {0} and {1} characters long"),
        range: $.validator.format("Please enter a value between {0} and {1}"),
        max: $.validator.format("Please enter a value less than or equal to {0}"),
        min: $.validator.format("Please enter a value greater than or equal to {0}"),
        require_from_group: $.validator.format("Please fill at least {0} field")
    },
    tr: {
        required: "Bu alanın doldurulması zorunludur",
        remote: "Lütfen bu alanı düzeltin",
        email: "Lütfen geçerli bir e-posta adresi giriniz",
        url: "Lütfen geçerli bir web adresi (URL) giriniz",
        date: "Lütfen geçerli bir tarih giriniz",
        dateISO: "Lütfen geçerli bir tarih giriniz(ISO formatında)",
        number: "Lütfen geçerli bir sayı giriniz",
        digits: "Lütfen sadece sayısal karakterler giriniz",
        creditcard: "Lütfen geçerli bir kredi kartı giriniz",
        equalTo: "Lütfen aynı değeri tekrar giriniz",
        extension: "Lütfen geçerli uzantıya sahip bir değer giriniz",
        phone: "Lütfen geçerli bir telefon numarası giriniz",
        maxlength: $.validator.format("Lütfen en fazla {0} karakter uzunluğunda bir değer giriniz"),
        minlength: $.validator.format("Lütfen en az {0} karakter uzunluğunda bir değer giriniz"),
        rangelength: $.validator.format("Lütfen en az {0} ve en fazla {1} uzunluğunda bir değer giriniz"),
        range: $.validator.format("Lütfen {0} ile {1} arasında bir değer giriniz"),
        max: $.validator.format("Lütfen {0} değerine eşit ya da daha küçük bir değer giriniz"),
        min: $.validator.format("Lütfen {0} değerine eşit ya da daha büyük bir değer giriniz"),
        require_from_group: $.validator.format("Lütfen bu alanların en az {0} tanesini doldurunuz")
    }
};

if (document.getElementById('ActiveLanguage').value.substring(0, 2) == "en") {
    $.extend($.validator.messages, message.en);
} else {
    $.extend($.validator.messages, message.tr);
} 

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
            required: true,
            minlength: 5
        },
        passwordNew: {
            required: true,
            minlength: 5
        },
        passwordNewConfirm: {
            required: true,
            minlength: 5,
            equalTo: "#passwordNew"
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