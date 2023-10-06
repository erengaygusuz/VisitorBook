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
            return `
                   <a onclick="showInPopup('/visitedstate/addoredit/${data}', 
                   'Update Visited State')" class="btn btn-info text-white"> Edit</a>
                   <a onClick=Delete('/visitedstate/delete/${data}') class="btn btn-danger mx-2">
                     Delete
                   </a>
                 `
        },
        width: '20%',
      },
    ],
  })
}
