$(function () {
    $.ajax({
        url: '/Profile/GetSkills',
        method: 'GET',
        success: function (data) {
            var skills = $.map(data, function (s) {
                return s.Name;
            });

            $("#search-skills").autocomplete({
                source: skills
            });
        }
    });
});