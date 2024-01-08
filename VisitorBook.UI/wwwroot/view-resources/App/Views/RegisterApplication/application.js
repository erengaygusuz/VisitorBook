var dataTable

$(document).ready(function () {

    $.fn.dataTable.moment('DD/MM/YYYY')

    var exportBtnText = document.getElementById('TableExportBtnText').value
    var viewBtnText = document.getElementById('TableViewBtnText').value
    var viewModalTitleText = document.getElementById('RegisterApplicationsViewModalTitleText').value

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
        viewBtnText,
        activeLanguagePath,
        viewModalTitleText
    )
})

function loadDataTable(
    exportBtnText,
    viewBtnText,
    activeLanguagePath,
    viewModalTitleText
) {
    dataTable = $('#tblData').DataTable({
        dom: "B<'row'<'col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 'l><'col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12'f>>tr<'row mt-3'<'col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12'i><'col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12'p>>",
        buttons: [
            {
                extend: 'pdfHtml5',
                text: exportBtnText,
                filename: 'VisitorBook-Applications',
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
                    columns: [0, 1, 2, 3, 4, 5],
                },
            },
        ],
        language: {
            url: activeLanguagePath,
        },
        preDrawCallback: function () {
            var btns = $('.dt-button, .buttons-pdf, .buttons-html5')
            btns.removeClass()
            btns.addClass('btn btn-success ml-2 mb-3')
            btns.attr('id', 'ExportBtn')

            $('#ExportBtn').prependTo($('#outside'))
        },
        processing: true,
        serverSide: true,
        filter: true,
        responsive: true,
        ajax: {
            url: '/app/registerapplication/getall',
            type: 'POST',
            datatype: 'json',
        },
        columns: [
            { data: 'user.name', width: '19%' },
            { data: 'user.surname', width: '19%' },
            { data: 'user.email', width: '15%' },
            { data: 'user.username', width: '15%' },
            {
                data: 'createdDate',
                width: '12%',
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY')
                }
            },
            {
                data: 'status',
                render: function (data) {
                    
                    if (data == "Pending") {
                        return `
                        <div class="d-flex justify-content-around align-items-center">
                            <span class="badge badge-warning p-2"><i class="fas fa-clock"></i></span>
                        </div>
                        `;
                        }
                    else if (data == "Approved") {
                        return `
                        <div class="d-flex justify-content-around align-items-center">
                            <span class="badge badge-success p-2"><i class="fas fa-check-circle"></i></span>
                        </div>
                        `;
                        }
                    else {
                        return `
                        <div class="d-flex justify-content-around align-items-center">
                            <span class="badge badge-danger p-2"><i class="fas fa-times-circle"></i></span>
                        </div>
                        `;
                    }
                },
                width: '10%'
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                            <div class="d-flex justify-content-around align-items-center">
                               <a onclick="showInPopup('/app/registerapplication/application/${data}',
                               '${viewModalTitleText}')" class="btn btn-block btn-default btn-sm"> ${viewBtnText}</a>
                            </div>
                           `
                },
                width: '10%',
            },
        ],
        columnDefs: [
            {
                targets: [6],
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
        },
    })
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