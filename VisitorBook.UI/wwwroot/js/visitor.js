var dataTable

$(document).ready(function () {
  $.fn.dataTable.moment('DD/MM/YYYY')

  var exportBtnText = document.getElementById('TableExportBtnText').value
  var editBtnText = document.getElementById('TableEditBtnText').value
  var deleteBtnText = document.getElementById('TableDeleteBtnText').value

  var activeLanguage = document.getElementById('ActiveLanguage').value

  var trLanguagePath = '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json'
  var enLanguagePath = '//cdn.datatables.net/plug-ins/1.13.6/i18n/en-GB.json'

  var activeLanguagePath = ''

  if (activeLanguage == 'tr-TR') {
    activeLanguagePath = trLanguagePath
  } else {
    activeLanguagePath = enLanguagePath
  }

  loadDataTable(exportBtnText, editBtnText, deleteBtnText, activeLanguagePath)
})

function loadDataTable(
  exportBtnText,
  editBtnText,
  deleteBtnText,
  activeLanguagePath
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
    ajax: { url: '/visitor/getall' },
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
                               <a onclick="showInPopup('/visitor/addoredit/${data}', 
                               'Update Visitor')" class="btn btn-warning"> ${editBtnText}</a>
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
        $('#statelist').hide()
      } else {
        $('#statelist').show()
      }
    },
  })
}

function fillStateList(cityId, selectedIndex) {
  $.ajax({
    type: 'GET',
    url: '/state/getallbycity?cityId=' + cityId,
    success: function (res) {
      $('#Visitor_VisitorAddress_StateId').empty()

      var stateSelectionText = document.getElementById(
        'AddOrEditModalStateSelectionText'
      ).value

      $('#Visitor_VisitorAddress_StateId').append(
        $('<option disabled value="0">' + stateSelectionText + '</option>')
      )

      $.each(res.data, function (index, value) {
        $('#Visitor_VisitorAddress_StateId').append(
          $('<option value="' + value.id + '">' + value.name + '</option>')
        )
      })

      $(
        '#Visitor_VisitorAddress_StateId option:eq(' + selectedIndex + ')'
      ).prop('selected', true)
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
  if (confirm('Are you sure to delete this record?')) {
    try {
      $.ajax({
        type: 'DELETE',
        url: url,
        success: function (res) {
          alert(res.entityValue + ' Deleted Successfully')

          dataTable.ajax.reload()
        },
        error: function (err) {
          alert('An Error Occurred While Deleting')

          console.log(err)
        },
      })
    } catch (e) {}
  }

  return false
}

function cityChange() {
  if ($('#citylist option:selected').val() == 0) {
    $('#statelist').hide()
  } else {
    $('#statelist').show()

    var cityId = $('#citylist option:selected').val()

    fillStateList(cityId, 0)
  }
}
