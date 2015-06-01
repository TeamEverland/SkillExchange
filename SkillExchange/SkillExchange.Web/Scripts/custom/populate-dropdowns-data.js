$(function () {
    $.ajax({
        url: '/Profile/GetTowns',
        method: 'GET',
        success: function(data) {
            var $selectElement = $('#towns');
            $(data).each(function () {
                $(document.createElement('option'))
                    .attr('value', this.Id)
                    .text(this.Name)
                    .appendTo($selectElement);
            });
        }
    });

    $.ajax({
        url: '/Profile/GetCategories',
        method: 'GET',
        success: function (data) {
            var $categoriesDropdowns = $(document).find('.categories');
            $.each($categoriesDropdowns, function () {
                var $dropdown = $(this);
                $(data).each(function() {
                    $(document.createElement('option'))
                        .attr('value', this.Id)
                        .text(this.Name)
                        .appendTo($dropdown);
                });
            });
        }
    });
});