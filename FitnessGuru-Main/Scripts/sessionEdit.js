
var editTrainerId = $('#editTrainerId')[0].innerHTML;
var sessionId = $('#sessionId')[0].innerHTML;
var sessionStrength = $('#sessionStrength')[0].innerHTML;
var apiUrl = '/api/sessions/GetUpcomingSessionsForCalendar/' + editTrainerId;


jQuery.extend({
    getValuesForEdit: function (url) {
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
        result.forEach(function (item) {
            item['url'] = '/sessions/edit/' + item.id;
        });
        return result;
    }
});



var data = $.getValuesForEdit(apiUrl);

var session = {};
var currentState = {};

session.id = sessionId;
session.trainerId = editTrainerId;
session.sessionAt = moment($('#sessionAt')[0].value, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm');
currentSavedSessionAt = moment($('#sessionAt')[0].value, 'YYYY-MM-DD HH:mm');
session.sessionName = $('#sessionName')[0].value;
session.desc = $('#sessionDesc')[0].value;
session.isCancelled = $('#sessionCancel')[0].checked;


currentState.id = sessionId;
currentState.trainerId = editTrainerId;
currentState.sessionAt = moment($('#sessionAt')[0].value, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm');
currentState.sessionName = $('#sessionName')[0].value;
currentState.desc = $('#sessionDesc')[0].value;
currentState.isCancelled = $('#sessionCancel')[0].checked;


$('#sessionName').focusout(function () {
    session.sessionName = $('#sessionName')[0].value;
});

$('#sessionAt').focusout(function () {
    session.sessionAt = moment($('#sessionAt')[0].value, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm');
});

$('#sessionDesc').focusout(function () {
    session.desc = $('#sessionDesc')[0].value;
});


$('#sessionCancel').focusout(function () {
    session.isCancelled = $('#sessionCancel')[0].checked;
});

var sessionCalendar = $("#editCalendar").fullCalendar({
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
            content: "Name: " + event.title + "<br>" + "Desc : " + event.desc
        });
    },


    dayClick: function (date, jsEvent, view) {

        var d = new Date(date);
        var m = moment(d).format("YYYY-MM-DD");
        m = encodeURIComponent(m);
        if (view.name === 'month') {
            $('#editCalendar').fullCalendar('changeView', 'agendaDay', m);
        } else if (view.name === 'agendaDay') {
            $('#sessionAt').val(moment(d).format('YYYY-MM-DD HH:mm'));
            session.sessionAt = moment(d).format('YYYY-MM-DDTHH:mm');
            $('#editCalendar').fullCalendar('changeView', 'month', m);
        }
    }
});

$.validator.addMethod("invalidSessionName",
    function () {
        return session.sessionName && session.sessionName.length !== 0;
    },
    "Session Name is required");


$.validator.addMethod("emptySessionAt",
    function () {
        return /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])T(00|0?[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9])(:([0-9]|[0-5][0-9]))?$/
            .test(session.sessionAt);
    },
    "Please enter a valid date and time");

$.validator.addMethod("conflictingSession",
    function () {
        var sessionAt = moment(session.sessionAt);
        var result = true;
        data.forEach(function (item) {

            var existingSessionStart = moment(item.start, 'YYYY-MM-DDTHH:mm');
            var twoHoursBeforeStart = moment(existingSessionStart).add(-2, 'hours');
            var twoHoursAfterStart = moment(existingSessionStart).add(2, 'hours');

            if (parseInt(editTrainerId) === parseInt(item.trainerId) &&
                session.id !== item.id &&
                (sessionAt.isSame(existingSessionStart) ||
                    sessionAt.isAfter(twoHoursBeforeStart) &&
                    sessionAt.isBefore(twoHoursAfterStart))) {
                result = false;
                return false;
            }
        });
        return result;
    },
    "You have a ongoing session clashing with the selected time");


$.validator.addMethod("moveSessionInFutureDate",
    function () {
        // get current time
        var currentTime = moment();
        // get session time
        var sessionAt = moment(session.sessionAt);
        return sessionAt.isAfter(currentTime);

    },
    "The time you selected have already passed"
);


$.validator.addMethod("isSession24HoursAway",
    function () {
        var currentTime = moment();
        var duration = moment.duration(currentSavedSessionAt.diff(currentTime));
        var hours = duration.asHours();
        return hours >= 24;
    },
    "Cannot change time for events less than 24 hours away");

$.validator.addMethod("validDescription",
    function () {
        return session.desc.length !== 0;
    },
    "Should provide a short description");

$('#editSession').validate({
    submitHandler: function () {


        if (_.isEqual(session, currentState)) {
            toastr.warning("Please make some changes to save");
            return false;
        }

        if (session.isCancelled) {
            var confirmMessage = ""
            if (sessionStrength !== 0) {
                confirmMessage =
                    "There are  " + sessionStrength + " members enrolled in this session. Do you really want to cancel it? Joined members will be notified through an email." +
                    " Go ahead if you are sure. This action cannot be reverted";
            } else {
                confirmMessage = "Are you sure you want to cancel this session? This action cannot be reverted";
            }


            if (confirm(confirmMessage)) {
                executeEdit();
            }
        } else {
            executeEdit();
        }

        return false;
    },
    invalidHandler: function (form, validator) {
        var errors = validator.numberOfInvalids();
        if (errors) {
            toastr.error("Please fix errors");
        }
    }

});

var executeEdit = function () {
    $.ajax({
        url: "/api/sessions/updatesession",
        method: "put",
        data: session,
        success: function () {
            sessionCalendar.fullCalendar('removeEventSource', data);
            data = {}
            data = $.getValuesForEdit(apiUrl)
            sessionCalendar.fullCalendar('addEventSource', data);
        }
    })
        .done(function () {
            toastr.success("Session successfully saved");
            currentState = jQuery.extend(true, {}, session);
            if (session.isCancelled) {
                window.location = '/trainers/index';
            }
        })
        .fail(function () {
            toastr.error("Something unexpected happened");
        });
    return false;
};
