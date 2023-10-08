var dataTable

$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    loadDataTable();
})

function loadDataTable() {
  dataTable = $('#tblData').DataTable({
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
                   <a onclick="showInPopup('/visitor/addoredit/${data}', 
                   'Update Visitor')" class="btn btn-warning"> Edit</a>
                   <a onclick=deleteRecord('/visitor/delete/${data}') class="btn btn-danger">
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
          console.log(url);

      $('#form-modal .modal-body').html(res)
      $('#form-modal .modal-title').html(title)
        $('#form-modal').modal('show')

        if ($('#citylist option:selected').val() == 0) {
            $('#statelist').hide();
        }
        else {
            $('#statelist').show();
        }
    },
  })
}

function fillStateList(cityId, selectedIndex) {
    $.ajax({
        type: 'GET',
        url: '/state/getallbycity?cityId=' + cityId,
        success: function (res) {

            $('#Visitor_VisitorAddress_StateId').empty();

            $('#Visitor_VisitorAddress_StateId').append(
                $('<option disabled value="0">--Select State--</option>'));

            $.each(res.data, function (index, value) {
                $('#Visitor_VisitorAddress_StateId').append(
                    $('<option value="' + value.id + '">' + value.name + '</option>'))
                }
            );

            $('#Visitor_VisitorAddress_StateId option:eq(' + selectedIndex + ')').prop('selected', true);
        },
        error: function (err) {
            console.log(err)
        }
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
        $('#statelist').hide();
    }
    else {
        $('#statelist').show();

        var cityId = $('#citylist option:selected').val();

        fillStateList(cityId, 0);
    }
}