HealthMonitoring.NET
====================

####..it's comming

* Mobile ready with responsive design, JSON api access, and more thank's to: AngularJS, Twitter BootStrap 3 and ASP.NET MVC 5
 * This is only the back-office for consulting and debugging problems, the logging mechanisms are compatible all the way down to .NET 2.0/ASP.NET and all the way up to .NET 4.5.1/MVC 5

####Via the various logging methods, it can be retrofited to existing installations without any code modification, great for legacy installations. Or various sites/assemblies can be enhanced with custom logging to get a even more detailed picture.


* Server-Side Tracing
  * Just configure web.config and catch all errors from the framework & iis
     * works transparently withthout modifying existing installations or code
     * catches all 400-599 error codes complete with stacktrace, request url, and more
* Server-Side Logging
  * Trace custom errors inside your code, complete stacktrace & custom object dumping
  * Requires a custom provider     
      * included with this project
* Client-Side Logging
  * Catch and trace all errors from window.onerror & jQuery ajax requests
      * No modifications to client-side code needed
  * Trace custom exceptions
     * Requires a custom provider     
         * included with this project

![backoffice](snapshot.png "backoffice")