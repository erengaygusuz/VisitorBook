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
          return `<div class="w-75 btn-group" role="group">
                        <a href="/city/upsert?id=${data}" class="btn btn-primary mx-2">
                          Edit
                        </a>
                         <a onClick=Delete('/city/delete/${data}') class="btn btn-danger mx-2">
                          Delete
                        </a>
                    </div>`
        },
        width: '20%',
      },
    ],
  })
}
