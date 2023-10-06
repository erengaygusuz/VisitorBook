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
                   <a onClick=Delete('/city/delete/${data}') class="btn btn-danger mx-2">
                     Delete
                   </a>
                 `
        },
        width: '20%',
      },
    ],
  })
}
