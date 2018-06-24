# AccessManagement
Identity Management &amp; Authentication Client/Server Application

## Features of this application:
- Identity Management
- Authentication and Authorization - credential validation, cookie issuing (web apps) &amp; token issuing (web services)
- Machine Key generation for a multi-app shared security environment
- Claims-based security
- User account management


## To Use this application:

1. Open each solution in a different Visual Studio app -- one for IAServer, one for PrimaryWebApp, and one of SecondaryWebApp
2. Build/compile each solution to gather any missing files or dependencies (ctrl + shift + B)
3. Run each app (ctrl + F5 within Visual Studio)
3. The primary and secondary web apps will have their own web interface, and interact with the IAServer
4. Use a web inspector/debugger application such as [Fiddler](https://www.telerik.com/fiddler) to send, receive, and inspect JSON. (Open the .har file for sample usage)
