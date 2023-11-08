var dataTable

$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY')

    var exportBtnText = document.getElementById('TableExportBtnText').value
    var editBtnText = document.getElementById('TableEditBtnText').value
    var deleteBtnText = document.getElementById('TableDeleteBtnText').value
    var editModalTitleText = document.getElementById(
        'VisitorsEditModalTitleText'
    ).value

    var activeLanguage = document.getElementById('ActiveLanguage').value

    var trLanguagePath = '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json'
    var enLanguagePath = '//cdn.datatables.net/plug-ins/1.13.6/i18n/en-GB.json'

    var activeLanguagePath = ''

    if (activeLanguage == 'tr-TR') {
        activeLanguagePath = trLanguagePath
    } else {
        activeLanguagePath = enLanguagePath
    }

    loadDataTable(
        exportBtnText,
        editBtnText,
        deleteBtnText,
        activeLanguagePath,
        editModalTitleText
    )
})

function loadDataTable(
    exportBtnText,
    editBtnText,
    deleteBtnText,
    activeLanguagePath,
    editModalTitleText
) {
    dataTable = $('#tblData').DataTable({
        dom: 'lfrtBip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: exportBtnText,
                filename: 'VisitorBook-Visitors',
                orientation: 'landscape',
                pageSize: 'A4',
                customize: function (doc) {
                    doc.content[1].table.widths = Array(
                        doc.content[1].table.body[0].length + 1
                    )
                        .join('*')
                        .split('')

                    doc.content.splice(0, 1)
                    doc.pageMargins = [20, 20, 20, 20]
                    doc.defaultStyle.fontSize = 12
                    doc.styles.tableHeader.fontSize = 14

                    var objLayout = {}
                    objLayout['hLineWidth'] = function (i) {
                        return 0.5
                    }
                    objLayout['vLineWidth'] = function (i) {
                        return 0.5
                    }
                    objLayout['hLineColor'] = function (i) {
                        return '#aaa'
                    }
                    objLayout['vLineColor'] = function (i) {
                        return '#aaa'
                    }
                    objLayout['paddingLeft'] = function (i) {
                        return 4
                    }
                    objLayout['paddingRight'] = function (i) {
                        return 4
                    }
                    doc.content[0].layout = objLayout
                },
                exportOptions: {
                    columns: [0, 1, 2, 3],
                },
            },
        ],
        language: {
            url: activeLanguagePath,
        },
        initComplete: function () {
            var btns = $('.dt-button, .buttons-pdf, .buttons-html5')
            btns.removeClass()
            btns.addClass('btn btn-success')
            btns.attr('id', 'ExportBtn')

            $('#ExportBtn').prependTo($('#outside'))
        },
        scrollY: 480,
        deferRender: true,
        scroller: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: '/visitor/getall',
            type: 'POST',
            datatype: 'json',
        },
        columns: [
            { data: 'name', width: '30%' },
            { data: 'surname', width: '30%' },
            {
                data: 'birthDate',
                width: '12%',
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY')
                },
            },
            { data: 'gender', width: '8%' },
            {
                data: 'id',
                render: function (data) {
                    return `
                            <div class="d-flex justify-content-around align-items-center">
                               <a onclick="showInPopup('/visitor/edit/${data}', 
                               '${editModalTitleText}')" class="btn btn-warning"> ${editBtnText}</a>
                               <a onclick=deleteRecord('/visitor/delete/${data}') class="btn btn-danger">
                                  ${deleteBtnText}
                               </a>
                            </div>
                           `
                },
                width: '20%',
            },
        ],
        columnDefs: [
            {
                targets: [4],
                orderable: false,
            },
        ],
    })
}

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res)
            $('#form-modal .modal-title').html(title)
            $('#form-modal').modal('show')

            if ($('#citylist option:selected').val() == 0) {
                $('#countylist').hide()
            } else {
                $('#countylist').show()
            }
        },
    })
}

function fillCountyList(cityId, selectedIndex) {
    $.ajax({
        type: 'GET',
        url: '/county/getallbycity?cityId=' + cityId,
        success: function (res) {
            $('#VisitorAddress_CountyId').empty()

            var countySelectionText = document.getElementById(
                'AddOrEditModalCountySelectionText'
            ).value

            $('#VisitorAddress_CountyId').append(
                $('<option disabled value="0">' + countySelectionText + '</option>')
            )

            $.each(res.data, function (index, value) {
                $('#VisitorAddress_CountyId').append(
                    $('<option value="' + value.id + '">' + value.name + '</option>')
                )
            })

            $('#VisitorAddress_CountyId option:eq(' + selectedIndex + ')').prop(
                'selected',
                true
            )
        },
        error: function (err) {
            console.log(err)
        },
    })
}

AddRecord = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal .modal-body').html('')
                    $('#form-modal .modal-title').html('')
                    $('#form-modal').modal('hide')

                    dataTable.ajax.reload()
                    toastr.success(res.message)
                } else {
                    $('#form-modal .modal-body').html(res.html)
                }
            },
            error: function (err) {
                console.log(err)
            },
        })
    } catch (e) {
        console.log(e)
    }

    return false
}

EditRecord = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal .modal-body').html('')
                    $('#form-modal .modal-title').html('')
                    $('#form-modal').modal('hide')

                    dataTable.ajax.reload()
                    toastr.success(res.message)
                } else {
                    $('#form-modal .modal-body').html(res.html)
                }
            },
            error: function (err) {
                console.log(err)
            },
        })
    } catch (e) {
        console.log(e)
    }

    return false
}

function deleteRecord(url) {
    var deleteRequestPopupTitleText = document.getElementById(
        'DeleteRequestPopupTitleText'
    ).value
    var deleteRequestPopupBodyText = document.getElementById(
        'DeleteRequestPopupBodyText'
    ).value
    var deleteRequestPopupConfirmBtnText = document.getElementById(
        'DeleteRequestPopupConfirmBtnText'
    ).value
    var deleteRequestPopupCancelBtnText = document.getElementById(
        'DeleteRequestPopupCancelBtnText'
    ).value

    Swal.fire({
        title: deleteRequestPopupTitleText,
        text: deleteRequestPopupBodyText,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: deleteRequestPopupConfirmBtnText,
        cancelButtonText: deleteRequestPopupCancelBtnText,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (res) {
                    dataTable.ajax.reload()
                    toastr.success(res.message)
                },
                error: function (err) {
                    toastr.error('An Error Occurred While Deleting')
                },
            })
        }
    })

    return false
}

function cityChange() {
    if ($('#citylist option:selected').val() == 0) {
        $('#countylist').hide()
    } else {
        $('#countylist').show()

        var cityId = $('#citylist option:selected').val()

        fillCountyList(cityId, 0)
    }
}
