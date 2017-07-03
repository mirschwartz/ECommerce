$(function () {
    ClearTableAndRepopulate();

    $('.table').on('click', '.btn-delete', function () {
        var id = $(this).data('id');
        $.post('/home/deleteFromCart', { Id: id }, function () {
            ClearTableAndRepopulate();
        })
    })

    function ClearTableAndRepopulate() {
        $('.table tr:gt(0)').remove();
        $.get('/home/getShoppingCartItems', function (result) {
            result.forEach(function (item) {
                $('.table').append(`<tr><td><div class="col-md-5"><img style="height: 100px;" src="../Images/${item.FileName}" /></div>` +
                   `<div class="col-md-5"><h3>${item.Title}</h3><p>${item.Description}</p></div>` +
                   `<div class="pull-right"><h4>${item.Price}</h4><h4>Quantity: ${item.Quantity}</h4></div><td>` +
                   `<button data-id="${item.Id}" class="btn btn-danger btn-delete">Delete</button></td></tr>`);
            })
        })
    }
})