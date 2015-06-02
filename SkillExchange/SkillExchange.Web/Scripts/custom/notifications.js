var notifier;
$(document).ready(function () {
    $.connection.notificationsHub.client.estimateNotificationsCount = function () {
        notifier.server.estimateNotificationsCount();
    };

    $.connection.notificationsHub.client.estimateNotificationsCountForClient = function (client) {
        notifier.server.estimateNotificationsCountForClient(client);
    };

    $.connection.notificationsHub.client.appendNotificationsCount = function (notificationsCount) {
        $('#notifications-count').text(notificationsCount);
    };

    $.connection.notificationsHub.client.appendNewNotification = function (newTweet) {
        $.ajax({
            url: '/User/Notifications/Notification',
            method: 'POST',
            data: newTweet,
            success: function (data) { $('#notifications-field').prepend(data); }
        });
    };

    $.connection.hub.start().done(function() {
        notifier = $.connection.notificationsHub;
        notifier.client.estimateNotificationsCount();
    });
});