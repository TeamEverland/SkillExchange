$(function () {
    var addOfferingSkillBtn = $('#add-offering-skill');
    var addSeekingSkillBtn = $('#add-seeking-skill');

    addOfferingSkillBtn.on('click', function() {
        $.ajax({
            url: '/User/Profile/OfferingSkillEditor',
            method: 'GET',
            success: function (data) {
                $('#no-offering-skills-info').remove();
                var $offeringSkillsSection = $('#offering-skills');
                $offeringSkillsSection.append(data);
            }
        });
    });

    addSeekingSkillBtn.on('click', function () {
        $.ajax({
            url: '/User/Profile/SeekingSkillEditor',
            method: 'GET',
            success: function (data) {
                $('#no-seeking-skills-info').remove();
                var $seekingSkillsSection = $('#seeking-skills');
                $seekingSkillsSection.append(data);
            }
        });
    });
});