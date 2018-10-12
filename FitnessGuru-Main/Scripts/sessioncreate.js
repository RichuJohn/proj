var trainerId = $('#trainerId')[0].innerHTML;
var apiUrl = '/api/sessions/GetUpcomingSessionsForCalendar/' + trainerId;

jQuery.extend({
    getValuesForCreate: function (url) {
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

var data = $.getValuesForCreate(apiUrl);

var session = {};
session.desc = "";
session.trainerId = $('#trainerId')[0].innerHTML;

$('#sessionName').focusout(function () {
    session.sessionName = $('#sessionName')[0].value;
});

$('#sessionAt').focusout(function () {
    session.sessionAt = moment($('#sessionAt')[0].value, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm');
});

$('#sessionDesc').focusout(function () {
    session.desc = $('#sessionDesc')[0].value;
});

var sessionCalendar = $("#calendar").fullCalendar({
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
            $('#calendar').fullCalendar('changeView', 'agendaDay', m);
        } else if (view.name === 'agendaDay') {
            $('#sessionAt').val(moment(d).format('YYYY-MM-DD HH:mm'));
            session.sessionAt = moment(d).format('YYYY-MM-DDTHH:mm');
            $('#calendar').fullCalendar('changeView', 'month', m);
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

            if (parseInt(trainerId) === parseInt(item.trainerId) &&
                (sessionAt.isSame(existingSessionStart) ||
                    sessionAt.isAfter(twoHoursBeforeStart) &&
                    sessionAt.isBefore(twoHoursAfterStart))) {
                result = false;
                return false;
            }
        });
        return result;
    },
    "You have a ongoing session during this time");


$.validator.addMethod("createSessionInFutureDate",
    function () {
        // get current time
        var currentTime = moment();

        // get session time
        var sessionAt = moment(session.sessionAt);

        // check whether session time is after two days
        var twoDaysAfter = moment(currentTime).add(48, 'hours');
        return sessionAt.isAfter(twoDaysAfter);

    },
    "Needs to be a day in future, atleast 48 hours after"
);


$.validator.addMethod("validDescription",
    function () {
        return session.desc.length !== 0;
    },
    "Should provide a short description");

$('#newSession').validate({
    submitHandler: function () {
        console.log(session);
        $.ajax({
            url: "/api/sessions/createsession",
            method: "post",
            data: session,
            success: function () {

                $('#sessionName')[0].value = "";
                $('#sessionAt')[0].value = "";
                $('#sessionDesc')[0].value = "";

                session = {};
                session.desc = "";
                session.trainerId = trainerId;
                sessionCalendar.fullCalendar('removeEventSource', data);
                console.log("removing data");
                data = {}
                console.log(data);
                data = $.getValuesForCreate(apiUrl)
                console.log(data);
                sessionCalendar.fullCalendar('addEventSource', data);
            }
        })
            .done(function () {
                toastr.success("Session successfully created. Notification mail sent to all users");
            })
            .fail(function () {
                toastr.error("Something unexpected happened");
            });

        return false;
    },
    invalidHandler: function (form, validator) {
        var errors = validator.numberOfInvalids();
        if (errors) {
            toastr.error("Please fix errors");
        }
    }

});
