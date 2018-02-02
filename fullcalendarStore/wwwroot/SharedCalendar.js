$(document).ready(function() {
	if (!window.tx_kitsharedcalendar_settings) {
		// initialize with defaults
		window.tx_kitsharedcalendar_settings = {
			pageTypeNum: "999",
			baseUrl: "/index.php?id=1",
			elementSelector: "#SharedCalendar",
			aspectRatio: 2,
			firstDay: 0,
			firstHour: 0,
			defaultDate: '',
		};
	}

	var sharedCalendarRender = $(tx_kitsharedcalendar_settings.elementSelector);

	if (sharedCalendarRender === undefined) {
		return;
	}

	var sharedCalendarUrl = window.tx_kitsharedcalendar_settings.baseUrl + '&type=' + tx_kitsharedcalendar_settings.pageTypeNum + '&tx_kitsharedcalendar_sharedcalendar[format]=json&tx_kitsharedcalendar_sharedcalendar[controller]=CalendarEvent&tx_kitsharedcalendar_sharedcalendar[action]=';
	var startDate = moment(window.tx_kitsharedcalendar_settings.defaultDate).isValid() ? moment(window.tx_kitsharedcalendar_settings.defaultDate) : moment();

	sharedCalendarRender.fullCalendar({
		header: {
			left: 'prev,next today',
			center: 'title',
			right: 'month,agendaWeek,agendaDay'
		},
		year: startDate.year(),
		month: startDate.month(),
		date: startDate.date(),
		defaultView: 'agendaWeek',
		aspectRatio: window.tx_kitsharedcalendar_settings.aspectRatio || 2, // Hint: larger numbers make smaller heights
		firstDay: window.tx_kitsharedcalendar_settings.firstDay || 0, // Sunday=0, Monday=1 etc
		firstHour: window.tx_kitsharedcalendar_settings.firstHour || 0,
		axisFormat: 'HH:mm',
		timeFormat: { agenda: 'HH:mm{ - HH:mm}', '': 'HH:mm' },
		dayNames: ['Sonntag', 'Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag'],
		//dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
		dayNamesShort: ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'],
		//dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
		monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
		//monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
		weekNumberTitle: 'KW', // 'W'
		columnFormat: {
		    month: 'ddd',
		    week: 'ddd dd.MM.',
		    day: 'dddd dd.MM.',
		},
		titleFormat: {
		    month: 'MMMM yyyy',                             // September 2009
		    week: "d [MMM] [ yyyy]{ '&#8212;' d MMM yyyy}", // Sep 7 - 13 2009
		    day: 'dddd, d MMM yyyy'                  // Tuesday, Sep 8, 2009
		},
		selectable: true,
		selectHelper: true,
		select: function(start, end, allDay) {
			var title = prompt('Event Title:');
			var eventData;
			if (title) {
				eventData = {
					title: title,
					start: start,
					end: end,
					allDay: allDay,
					feuser: '&lt;&lt;me&gt;&gt;',
				};
				$.post(
					sharedCalendarUrl + "create",
					{
						tx_kitsharedcalendar_sharedcalendar: {
							newCalendarEvent: {
								//id:
								title: eventData.title,
								startdate: moment(eventData.start).format(),
								enddate: moment(eventData.end).format(),
							}
						}
					},
					function(result, status, jqx) {
						sharedCalendarRender.fullCalendar('renderEvent', eventData, true); // stick? = true
					}
				).fail(function() {
					alert("Error on saving the new item.");
				});
			}
			sharedCalendarRender.fullCalendar('unselect');
		},
		events: {
			url: sharedCalendarUrl + "list",
			error: function() {
				$('#script-warning').show();
			}
		},
		startParam: 'tx_kitsharedcalendar_sharedcalendar[start]',
		endParam: 'tx_kitsharedcalendar_sharedcalendar[end]',
		eventRender: function(event, element) {
			// Add username to template
			element.find('.fc-event-title').after('<div class="sharedcal-event-username">'+event.feuser+'</div>');
		}
		// events: [
		// 	{
		// 		id: 999,
		// 		title: 'Repeating Event',
		// 		start: '2014-01-09T16:00:00'
		// 		end: '2014-01-10'
		// 		url: 'http://google.com/',
		// 	},
		// ]
	});
});