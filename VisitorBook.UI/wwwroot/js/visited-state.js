var dataTable

$(document).ready(function () {
  loadDataTable()
})

function loadDataTable() {
  dataTable = $('#tblData').DataTable({
    ajax: { url: '/visitedstate/getall' },
    columns: [
      {
        data: 'visitor',
        width: '30%',
        render: function (data) {
          return data.name + ' ' + data.surname
        },
      },
      { data: 'state.name', width: '30%' },
      {
        data: 'date',
        width: '20%',
        render: function (data) {
          return moment(data).format('DD/MM/YYYY')
        },
      },
      {
        data: 'id',
        render: function (data) {
          return `<div class="w-75 btn-group" role="group">
                        <a href="/visitedstate/upsert?id=${data}" class="btn btn-primary mx-2">
                          Edit
                        </a>
                         <a onClick=Delete('/visitedstate/delete/${data}') class="btn btn-danger mx-2">
                          Delete
                        </a>
                    </div>`
        },
        width: '20%',
      },
    ],
  })
}
