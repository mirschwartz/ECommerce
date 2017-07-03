$(function () {
    ClearAndPopulateCategories();
    ClearAndPopulateProducts();

    $('#new-product').on('click', function () {
        $.get('/home/getCategories', function (result) {
            result.forEach(function(category){
                $('#category').append(`<option value="${category.Id}">${category.Name}</option>`);
            })
            $('#new-product-modal').modal();
        })
    })
    $('#new-category').on('click', function () {
        $('#new-category-modal').modal();
    })

    $('#add-category').on('click', function () {
        var categoryName = $('#category-name').val();
        $.post('/admin/addCategories', { categoryName: categoryName }, function () {
            $('#new-category-modal').modal('hide');
            ClearAndPopulateCategories();            
        })
    })

    $('.categories').on('click', '.btn-filter', function () {
        var categoryId = $(this).data('id');
        ClearAndPopulateProducts(categoryId);
    })

    function ClearAndPopulateProducts(category) {
        $('.thumb').remove();
        $.get('/home/getProducts', { CategoryId: category }, function (result) {
            result.forEach(function (product) {
                $('.products').append(`<div class="col-md-4 thumb"><div class="thumbnail">` + 
                    `<img style="max-height: 100px;" src="../Images/${product.ImageFile}" />` +
                    `<div class="caption">` +
                    `<h4 class="pull-right">${product.Price}</h4><a href="/Home/ViewDetail?ProductId=${product.Id}"><h4>${product.Title}</h4></a>` +
                    `<p>${product.Description}</p></div></div></div>`);
            })
        })
    }

    function ClearAndPopulateCategories() {
        $('.list-group').remove();
        $.get('/home/getCategories', function (result) {
            result.forEach(function (category) {
                $('.categories').append(`<div class="list-group"><div class="list-group-item">` +
                    `<button class="btn btn-link btn-filter" data-id="${category.Id}">${category.Name}</button></div></div>`);
            })
        })
    }
})