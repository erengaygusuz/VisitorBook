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
          return `<div class="w-75 btn-group" role="group">
                        <a href="/state/upsert?id=${data}" class="btn btn-primary mx-2">
                          Edit
                        </a>
                         <a onClick=Delete('/state/delete/${data}') class="btn btn-danger mx-2">
                          Delete
                        </a>
                    </div>`
        },
        width: '20%',
      },
    ],
  })
}
