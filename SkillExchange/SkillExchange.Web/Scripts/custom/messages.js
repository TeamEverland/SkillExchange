var notifier;
$(document).ready(function () {
    $.connection.messagesHub.client.estimateMessagesCount = function () {
        notifier.server.estimateMessagesCount();
    };

    $.connection.messagesHub.client.estimateMessagesCountForClient = function (client) {
        notifier.server.estimateMessagesCountForClient(client);
    };

    $.connection.messagesHub.client.appendMessagesCount = function (messagesCount) {
        $('#messages-count').text(messagesCount);
    };

    $.connection.messagesHub.client.appendNewMessages = function (newMessages) {
        $.ajax({
            url: '/User/Notifications/Messages',
            method: 'POST',
            data: newMessages,
            success: function (data) { $('#messages-field').prepend(data); }
        });
    };

    $.connection.hub.start().done(function() {
        notifier = $.connection.messagesHub;
        notifier.client.estimateMessagesCount();
    });
});