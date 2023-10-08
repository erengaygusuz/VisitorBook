var dataTable

$(document).ready(function () {
  loadDataTable()
})

function loadDataTable() {
  dataTable = $('#tblData').DataTable({
    ajax: { url: '/state/getall' },
    columns: [
      { data: 'name', width: '25%' },
      { data: 'city.name', width: '25%' },
      { data: 'latitude', width: '15%' },
      { data: 'longitude', width: '15%' },
      {
        data: 'id',
        render: function (data) {
          return `
                   <a onclick="showInPopup('/state/addoredit/${data}', 
                   'Update State')" class="btn btn-warning"> Edit</a>
                   <a onclick=deleteRecord('/state/delete/${data}') class="btn btn-danger">
                     Delete
                   </a>
                 `
        },
        width: '20%',
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

