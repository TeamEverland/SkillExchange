﻿$(document).ready(function () {
    var $conversationField = $('#conversation');
    var $conversationsSummariesField = $('#conversations');
    var $loadingImage = $('#loading-image');
    $loadingImage.hide();

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

    $.connection.messagesHub.client.appendNewMessage = function (messageId, sender) {
        $.ajax({
            url: '/User/Messages/GetMessage',
            method: 'POST',
            data: { messageId: messageId }
        }).success(function (data) {
            var conversationWithSenderField = $('#conversation-with-' + sender);
            if (conversationWithSenderField.length > 0) {
                var $conversationMessages = $conversationField.find('#conversation-messages');
                $conversationMessages.append(data);
            }
        });
    };

    $.connection.messagesHub.client.appendConversationSummary = function (sender) {
        var $conversationSummary = $conversationsSummariesField.find('#' + sender);
        if ($conversationSummary.length > 0) {
            $conversationSummary.addClass('active');
        } else {
            $.ajax({
                url: '/User/Messages/GetConversationSummary',
                method: 'POST',
                data: { interlocutor: sender }
            }).success(function (data) {
                $conversationsSummariesField.append(data);
            });
        }
    };

    $conversationsSummariesField.on('click', '.conversation-summary', function () {
        $loadingImage.show();
        var interlocutor = $(this).text().trim();
        $(this).removeClass('active');

        $.ajax({
            url: '/User/Messages/Conversation',
            method: 'POST',
            data: { interlocutorName: interlocutor }
        }).success(function (data) {
            $loadingImage.hide();
            $conversationField.html(data);
            var conversationFieldHeight = $conversationField[0]['scrollHeight'];
            $conversationField.animate({ scrollTop:  conversationFieldHeight}, 1000);
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

