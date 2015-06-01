$(function () {
    $(document).on('click', '.remove-skill-btn', function() {
        var hiddenFields = $(this).parent().find('.state');
        alert(hiddenFields.length);
        $.each(hiddenFields, function () {
            var $element = $(this);
            if ($element.val() === 'Existing') {
                $element.val('ExistingDeleted');
            } else {
                $element.val('NewDeleted');
            }
        });
        $(this).parent().hide();
    });
});