
var userId = $("#userId")[0].innerHTML;
var apiUrl = '/api/sessions/GetUpcomingSessionsForMember/' + userId;
var previousCheckedValue = 'All';

// Get the modal
var modal = document.getElementById('myModal');
// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
};

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target === modal) {
        modal.style.display = "none";
    }
};



jQuery.extend({
    getValuesForMemberView: function (url) {
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

var data = $.getValuesForMemberView(apiUrl);
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


var sessionCalendar = $("#memberCalendar").fullCalendar({
    locale: 'au',
    header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaDay'

    },
    defaultView: "month",
    timezone: "local",
    editable: false,
    startEditable: false,
    durationEditable: false,
    resourceEditable: false,
    overlap: true,

    eventLimit: true,
    events: data,
    eventRender: function (event, element) {
        element.qtip({
            content: "Name: " + event.title + "<br>" + "Desc : " + event.desc + "<br>" + "Trainer : " + event.trainerName
        });
        if (event.joined) {
            element.css('background', "seagreen");
        } else {
            element.css('background', "maroon");
        }
    },

    eventClick: function (event, jsEvent, view) {
        $('.modal-content #popUpHeading')[0].innerHTML = event.title;
        var message = event.joined ? "I wouldn't suggest it.. But it's your call" : 'Perfect choice.. I highly recommend this';
        var action = event.joined ? 'Opt Out' : 'Join';
        $('.modal-content #popUpContent')[0].innerHTML = message;
        $('.modal-content #actionToTake')[0].innerHTML = action;
        $('.modal-content #actionToTake').attr('data-action', action);
        $('.modal-content #actionToTake').attr('data-session-id', event.id);
        modal.style.display = "block";
    },


    dayClick: function (date, jsEvent, view) {

        var d = new Date(date);
        var m = moment(d).format("YYYY-MM-DD");
        m = encodeURIComponent(m);
        if (view.name === 'month') {
            $('#memberCalendar').fullCalendar('changeView', 'agendaDay', m);

        }
    }
});


$('.modal-content #actionToTake').on('click',
    function () {
        var button = $(this);
        var action = button.attr('data-action');
        var urlToHit = action === 'Join' ? '/GymMembers/JoinSession/' : '/GymMembers/OptOutFromSession/';
        var message = action === 'Join'
            ? "Are you sure you want to join in this session?"
            : "Are you sure you want to opt out from this session?";
        var successMessage = action === 'Join' ? "Successfully Joined Session" : "Successfully Opted Out from Session";
        var errorMessage = action === 'Join'
            ? "Could not join you to the session"
            : "Could not opt you out from the session";
        var sessionId = button.attr("data-session-id");

        if (action === 'Join') {
            var sessionToJoin = {}
            var clashingSession = {}
            data.forEach(function (item) {
                if (parseInt(item.id) === parseInt(sessionId)) {
                    sessionToJoin = item;
                    return false;
                }
            });

            var sessionAt = moment(sessionToJoin.start, 'YYYY-MM-DDTHH:mm');

            var result = true;
            joinedSessions.forEach(function (item) {
                var existingSessionStart = moment(item.start, 'YYYY-MM-DDTHH:mm');
                var twoHoursBeforeStart = moment(existingSessionStart).add(-2, 'hours');
                var twoHoursAfterStart = moment(existingSessionStart).add(2, 'hours');

                if (parseInt(sessionId) !== parseInt(item.id) &&
                    (sessionAt.isSame(existingSessionStart) ||
                        sessionAt.isAfter(twoHoursBeforeStart) &&
                        sessionAt.isBefore(twoHoursAfterStart))) {
                    clashingSession = item;
                    result = false;
                    return false;
                }
            });

            if (!result) {
                toastr.error("Cannot join. Clashing with session " + clashingSession.title);
                modal.style.display = "none";
                return false;
            }
        }


        if (confirm(message)) {
            $.ajax({
                url: urlToHit + sessionId,
                method: "POST",
                success: function () {
                    toastr.success(successMessage);

                    // search for the session
                    if (previousCheckedValue === 'All') {
                        sessionCalendar.fullCalendar('removeEventSource', data);
                    } else if (previousCheckedValue === 'Joined') {
                        sessionCalendar.fullCalendar('removeEventSource', joinedSessions);
                    } else if (previousCheckedValue === 'Other') {
                        sessionCalendar.fullCalendar('removeEventSource', upcomingSessions);
                    }
                    data.forEach(function (item) {
                        if (parseInt(item.id) === parseInt(sessionId)) {
                            item.joined = !item.joined;
                            return false;
                        }
                    });

                    joinedSessions = [];
                    upcomingSessions = [];

                    data.forEach(function (item) {
                        if (item.joined)
                            joinedSessions.push(item);
                    });

                    data.forEach(function (item) {
                        if (!item.joined)
                            upcomingSessions.push(item);
                    });

                    if (previousCheckedValue === 'All') {
                        sessionCalendar.fullCalendar('addEventSource', data);
                    } else if (previousCheckedValue === 'Joined') {
                        sessionCalendar.fullCalendar('addEventSource', joinedSessions);
                    } else if (previousCheckedValue === 'Other') {
                        sessionCalendar.fullCalendar('addEventSource', upcomingSessions);
                    }

                },
                error: function () {
                    toastr.error(errorMessage);

                }

            });
            modal.style.display = "none";
        } else {
            modal.style.display = "none";
        }
    });


$("input[name='join']").change(function () {

    var value = $('input[name=join]:checked').val();
    if (previousCheckedValue === 'All') {
        sessionCalendar.fullCalendar('removeEventSource', data);
    } else if (previousCheckedValue === 'Joined') {
        sessionCalendar.fullCalendar('removeEventSource', joinedSessions);
    } else if (previousCheckedValue === 'Other') {
        sessionCalendar.fullCalendar('removeEventSource', upcomingSessions);
    }

    if (value === 'All') {
        sessionCalendar.fullCalendar('addEventSource', data);
    } else if (value === 'Joined') {
        sessionCalendar.fullCalendar('addEventSource', joinedSessions);
    } else if (value === 'Other') {
        sessionCalendar.fullCalendar('addEventSource', upcomingSessions);
    }

    previousCheckedValue = value;
});



