$(document).ready(function () {
    var $conversationField = $('#conversation');

    $('.conversation-summary').on('click', function() {
        var username = $(this).text().trim();
        $.ajax({
            url: '/User/Messages/Conversation',
            method: 'POST',
            data: { interlocutorName: username }
        }).success(function(data) {
            $conversationField.replaceWith(data);
        });
    });
});