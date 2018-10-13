
var userId = $("#userId")[0].innerHTML;
var apiUrl = '/api/sessions/GetUpcomingSessionsForMember/' + userId;

jQuery.extend({
    getValuesForMemberIndexView: function (url) {
        var result = null;
        $.ajax({
            url: url,
            type: 'get',
            dataType: 'json',
            async: false,
            success: function (data) {
                result = data;
            }
        });

        return result;
    }
});

var data = $.getValuesForMemberIndexView(apiUrl);
var joinedSessions = [];
data.forEach(function (item) {
    if (item.joined)
        joinedSessions.push(item);
});

var upcomingSessions = [];
data.forEach(function (item) {
    if (!item.joined)
        upcomingSessions.push(item);
});

