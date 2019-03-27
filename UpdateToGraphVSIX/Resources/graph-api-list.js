var graph_api_list =
    {
        //get a user https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_get
        'GETAUSER': {
            api_name: 'Get a user',
            api_url: 'https://graph.microsoft.com/v1.0/users/{id | userPrincipalName}',
            method: 'GET',
            request: {
                request_header: {
                    'Authorization': 'Bearer ' + window.sessionStorage.token,
                    "Content-Type": "application/json",
                    "Prefer": 'outlook.timezone="' + Date.timeZone + '"'
                },
                request_body: {}
            },
            response: {
                response_header: {}, response_body: {}
            }
        },

        //create user https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_post_users
        'CREATEUSER': {
            api_name: 'Create User',
            api_url: 'https://graph.microsoft.com/v1.0/users',
            method: 'POST',
            request: {
                request_header: {
                    'Authorization': 'Bearer ' + window.sessionStorage.token,
                    "Content-Type": "application/json",
                    "Prefer": 'outlook.timezone="' + Date.timeZone + '"'
                },
                request_body: {
                    "accountEnabled": true,
                    "displayName": "displayName-value",
                    "mailNickname": "mailNickname-value",
                    "userPrincipalName": "upn-value@tenant-value.onmicrosoft.com",
                    "passwordProfile": {
                        "forceChangePasswordNextSignIn": true,
                        "password": "password-value"
                    }
                }
            },
            response: {
                response_header: {}, response_body: {}
            }
        }
    }
