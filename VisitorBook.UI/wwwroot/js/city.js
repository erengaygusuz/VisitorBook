var dataTable

$(document).ready(function () {
  loadDataTable()
})

function loadDataTable() {
  dataTable = $('#tblData').DataTable({
    ajax: { url: '/city/getall' },
    columns: [
      { data: 'name', width: '40%' },
      { data: 'code', width: '40%' },
      {
        data: 'id',
        render: function (data) {
          return `
                   <a onclick="showInPopup('/city/addoredit/${data}', 
                   'Update City')" class="btn btn-info text-white"> Edit</a>
                   <a onclick=deleteCity('/city/delete/${data}') class="btn btn-danger mx-2">
                     Delete
                   </a>
                 `
        },
        width: '20%',
      },
    ],
  })
}

AddCity = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');

                    dataTable.ajax.reload();
                }

                else {
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })

    } catch (e) {
        console.log(e);
    }

    return false;
}

function deleteCity(url) {
    if (confirm('Are you sure to delete this record?')) {
        try {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (res) {
                    alert(res.cityName + " City Deleted Successfully");

                    dataTable.ajax.reload();
                },
                error: function (err) {
                    alert("City Could Not Deleted");

                    console.log(err);
                }
            })
        } catch (e) {

        }
    }

    return false;
}
