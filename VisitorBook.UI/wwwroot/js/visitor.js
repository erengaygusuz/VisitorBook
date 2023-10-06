var dataTable

$(document).ready(function () {
  loadDataTable()
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
          return `<div class="w-75 btn-group" role="group">
                        <a href="/visitor/upsert?id=${data}" class="btn btn-primary mx-2">
                          Edit
                        </a>
                         <a onClick=Delete('/visitor/delete/${data}') class="btn btn-danger mx-2">
                          Delete
                        </a>
                  </div>`
        },
        width: '20%',
      },
    ],
  })
}
