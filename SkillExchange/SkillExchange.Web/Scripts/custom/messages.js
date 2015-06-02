$(document).ready(function () {
    // Register messages client functions
    $.connection.messagesHub.client.estimateMessagesCount = function () {
        notifier.server.estimateMessagesCount();
    };

    $.connection.messagesHub.client.estimateMessagesCountForClient = function (client) {
        notifier.server.estimateMessagesCountForClient(client);
    };

    var $conversationField = $('#conversation');

    $('.conversation-summary').on('click', function() {
        var interlocutor = $(this).text().trim();
        $(this).removeClass('active');

        $.ajax({
            url: '/User/Messages/Conversation',
            method: 'POST',
            data: { interlocutorName: interlocutor }
        }).success(function(data) {
            $conversationField.html(data);
        });

        $.ajax({
            url: '/User/Profile/LoggedUserGetUsername',
            method: 'POST'
        }).success(function (data) {
            notifier.server.markMessagesAsRead(data, interlocutor);
        });
    });
});

// Clears the message textbox after successful send
function clearInput() {
    $('#message').val('');
}

