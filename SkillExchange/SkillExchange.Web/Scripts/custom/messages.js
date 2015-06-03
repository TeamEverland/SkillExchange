$(document).ready(function () {
    var $conversationField = $('#conversation');

    // Register messages client functions
    $.connection.messagesHub.client.estimateMessagesCount = function () {
        notifier.server.estimateMessagesCount();
    };

    $.connection.messagesHub.client.estimateMessagesCountForClient = function (client) {
        notifier.server.estimateMessagesCountForClient(client);
    };

    $.connection.messagesHub.client.findMessageRecieverClients = function (reciever, messageId) {
        notifier.server.findMessageRecieverClients(reciever, messageId);
    };

    $.connection.messagesHub.client.appendNewMessage= function (messageId) {
        $.ajax({
            url: '/User/Messages/GetMessage',
            method: 'POST',
            data: { messageId: messageId }
        }).success(function (data) {
            //var currentConversation = $('#' + data['InterlocutorName']);
            var $conversationMessages = $conversationField.find('#conversation-messages');
            //$conversationMessages.append(data['MessageView']);
            $conversationMessages.append(data);
        });
    };

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

