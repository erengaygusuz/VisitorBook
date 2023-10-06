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
            return `
                   <a onclick="showInPopup('/visitor/addoredit/${data}', 
                   'Update Visitor')" class="btn btn-info text-white"> Edit</a>
                   <a onClick=Delete('/visitor/delete/${data}') class="btn btn-danger mx-2">
                     Delete
                   </a>
                 `
        },
        width: '20%',
      },
    ],
  })
}
