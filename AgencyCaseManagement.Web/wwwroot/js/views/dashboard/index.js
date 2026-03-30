import { Calendar } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';

$(document).ready(() => {
    let $c = $('#calendar')[0]//<-needed to be unwrapped w/ index for fullcalendar library;
    let cal = new Calendar($c, {
        plugins: [dayGridPlugin, timeGridPlugin, listPlugin],
        initialView: 'timeGridDay', //month view
        //events: '/Calendar/FillCalendarDates', <--set up API
        events: 
        {   
            url: '/Dashboard/GetAllMeetingsAsync',
            method: 'GET',
            failure: function (error) {
                console.log('Error fetching events:', error);
            }
        }
        ,        
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        eventClick: function (info) {
            const event = info.event;

            alert(
                "Title: " + event.title + "\n" +
                "Start: " + event.start?.toLocaleString() + "\n" +
                "End: " + (event.end ? event.end.toLocaleString() : "N/A") + "\n" +
                "Case: " + (event.extendedProps.caseTitle || "N/A") + "\n" +
                "Participants: " + event.extendedProps.participantCount + "\n" +
                "Type: " + (event.extendedProps.meetingType || "N/A")
            );
            //^^needs to be enhanced
        }
    });
    cal.render();
});