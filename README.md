HealthMonitoring.NET
====================

HealthMonitoring is built into .NET since v2.0, and is the simplest solution to track & fix your application's errors.

*At it's base, you don't have to do anything more than just configure a small section in your web.config to get it up and running*

###But there is a lot of other fun stuff you can do with it
* Log the data to a provider of choice
 * For example a database (.NET comes with automatic setup for this already)
  * "C:\Windows\Microsoft.NET\Framework\vX.XX\aspnet_regsql.exe -w" and you're pretty much set!
* Hook a backoffice to that database, and now search and filter your events based on MachineId, SiteId, EventTime, EventOccurence, EventType, and more
* Create a very small class so that you can manually log events of your choice via that same pipeline
 * Everytime you do a try/catch for example
* Have a small script client-side (for websites) which will automatically forward all ajax and window.onerror events to that same pipeline
 * Use the script to manually forward events


###Quick Links

Some quick links to get you up to speed

* http://forums.asp.net/post/1402868.aspx
* http://msdn.microsoft.com/en-us/library/ms998306.aspx
* http://www.manuelabadia.com/blog/PermaLink,guid,77dd4e93-e052-4b65-9b9d-81a17f0e2b6e.aspx
