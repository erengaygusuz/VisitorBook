var stepper

$(document).ready(function () {
    stepper = new Stepper($('.bs-stepper')[0], {
        linear: false,
        animation: true
    })
})