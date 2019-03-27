//Appointment.Accept
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events/{id}/accept",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
    },
    data: {
        "comment": "comment-value",
        "sendResponse": true
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Appointment.AcceptTentatively
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events/{id}/tentativelyAccept",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
    },
    data: {
        "comment": "comment-value",
        "sendResponse": true
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Appointment.Decline
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events/{id}/decline",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
    },
    data: {
        "comment": "comment-value",
        "sendResponse": true
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Appointment.Bind
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
        "Prefer": 'outlook.timezone = "Eastern Standard Time"'
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Appointment.Save
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
    },
    data: {
        "subject": "Let's go for lunch",
        "body": {
            "contentType": "HTML",
            "content": "Does late morning work for you?"
        },
        "start": {
            "dateTime": "2017-04-15T12:00:00",
            "timeZone": "Pacific Standard Time"
        },
        "end": {
            "dateTime": "2017-04-15T14:00:00",
            "timeZone": "Pacific Standard Time"
        },
        "location": {
            "displayName": "Harry's Bar"
        },
        "attendees": [
            {
                "emailAddress": {
                    "address": "samanthab@contoso.onmicrosoft.com",
                    "name": "Samantha Booth"
                },
                "type": "required"
            }
        ]
    }
    ,
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Appointment.Update
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events/{id}",
    method: "PATCH",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "originalStartTimeZone": "originalStartTimeZone-value",
        "originalEndTimeZone": "originalEndTimeZone-value",
        "responseStatus": {
            "response": "",
            "time": "datetime-value"
        },
        "recurrence": null,
        "iCalUId": "iCalUId-value",
        "reminderMinutesBeforeStart": 99,
        "isReminderOn": true
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//CalendarFolder.FindAppointments
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json",
        "Prefer": 'outlook.timezone = "Eastern Standard Time"'
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//CalendarFolder.Bind
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/calendar",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//EmailMessage.Bind
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/messages/{id}",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//EmailMessage.Send
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/sendMail",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "message": {
            "subject": "Meet for lunch?",
            "body": {
                "contentType": "Text",
                "content": "The new cafeteria is open."
            },
            "toRecipients": [
                {
                    "emailAddress": {
                        "address": "fannyd@contoso.onmicrosoft.com"
                    }
                }
            ],
            "ccRecipients": [
                {
                    "emailAddress": {
                        "address": "danas@contoso.onmicrosoft.com"
                    }
                }
            ]
        },
        "saveToSentItems": "false"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//ExchangeService.DeleteItems
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/events/{id}" || "https://graph.microsoft.com/v1.0/me/messages/{id}",
    method: "Delete",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//ExchangeService.FindFolders
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/calendars",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//ExchangeService.FindItems
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//ExchangeService.GetPeopleInsights
$.ajax({
    url: "https://graph.microsoft.com/v1.0/users/{id | userPrincipalName}",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Folder.Bind
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders/{id}",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Folder.Delete
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders/{id}",
    method: "DELETE",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Folder.FindFolders
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders",
    method: "GET",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Folder.Save
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "displayName": "displayName-value"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Folder.Update
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/mailFolders/{id}",
    method: "PATCH",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "displayName": "displayName-value"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Item.Copy
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/messages/{id}/copy",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "destinationId": "destinationId-value"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Item.Move
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/messages/{id}/move",
    method: "POST",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "destinationId": "destinationId-value"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})
//Item.Update
$.ajax({
    url: "https://graph.microsoft.com/v1.0/me/messages/{id}",
    method: "PATCH",
    headers: {
        'Authorization': 'Bearer {Token}',
        "Content-Type": "application/json"
    },
    data: {
        "subject": "subject-value",
        "body": {
            "contentType": "",
            "content": "content-value"
        },
        "inferenceClassification": "other"
    },
    success: function (res) {
        console.log(res);
    },
    error: function (res) {
        console.error(res);
    }
})