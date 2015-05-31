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
});