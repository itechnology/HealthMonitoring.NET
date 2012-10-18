#region About
// HealthMonitoring.NET
// 
// Version: 1.00
// Last Modified: 2012-10-19 00:08
// 
// Copyright: Robert Hoffmann / i-Technology.NET
// License: MIT / http://bit.ly/mit-license
#endregion
using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Management;
using System.Web.Script.Serialization;

/// <summary>
/// Helper class which i always use when logging exceptions.
/// You really don't need anything other than HealthMonitoring which is baked in since .NET 2.0
/// 
/// .NET sends all unhandled events to your log provider of choice
/// This here is the server-side class for "handled events"
/// there is also a client-side version for ajax & window.onerror events
/// 
/// Then i use custom backoffice which lists these events sorted & searchable by machineId, siteId, date/time, eventType & occurence
/// ..release is on the todo list ;-)
/// 
/// <seealso cref="http://forums.asp.net/post/1402868.aspx"/>
/// </summary>
public sealed class HealthMonitoring
{
    #region functions
    /// <summary>
    /// Raise an event to be logged
    /// </summary>
    /// <param name="title">A short generic description i.e. An unhandled exception has occurred.</param>
    /// <param name="ex">Exception being passed</param>
    /// <param name="extraInfo">Optional object you can pass to the exception</param>
    /// <param name="memberName">Automatically added by the Framework</param>
    /// <param name="filePath">Automatically added by the Framework</param>
    /// <param name="lineNumber">Automatically added by the Framework</param>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.callermembernameattribute.aspx"/>
    public static void LogException(string title, Exception ex, object extraInfo = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        new Itechnology.HealthMonitoringEvent(title, null, WebEventCodes.WebExtendedBase + 1, ex)
        {
            ExtraInfo  = extraInfo,
            CallerInfo = string.Format("MemberName: {0}\nFilePath: {1}\nLineNumber: {2}", memberName, filePath, lineNumber)
        }
        .Raise();
    }
    #endregion
}

namespace Itechnology
{
    /// <summary>
    /// Mains class for logging exceptions
    /// </summary>
    internal sealed class HealthMonitoringEvent : WebErrorEvent
    {
        private readonly JavaScriptSerializer _serializer;
        public HealthMonitoringEvent(string message, object eventSource, int eventCode, Exception exception) : base(message, eventSource, eventCode, exception)
        {
            ExtractInformation(HttpContext.Current);
            _serializer = new JavaScriptSerializer();
        }

        #region Customization
        public string CallerInfo { get; set; }
        public object ExtraInfo  { get; set; }
        private string Params    { get; set; }

        /// <summary>
        /// INFO: don't keep reference to HttpContext
        /// http://msdn.microsoft.com/en-us/library/ff650832.aspx#AuditingLogging8
        /// http://msdn.microsoft.com/en-us/library/ff650305.aspx#paght000011_step1
        /// </summary>
        public void ExtractInformation(HttpContext ctx)
        {
            if (ctx == null)
            {
                return;
            }

            var variables = new NameValueCollection
                                {
                                    // Don't trigger HttpRequestValidationException. If someone is messing with our site, we wanna see it in the logs !
                                    ctx.Request.Unvalidated.Form,
                                    ctx.Request.ServerVariables
                                };

            foreach (string key in variables)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    Params += string.Concat(key, "=", variables.Get(key), Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// INFO: http://msdn.microsoft.com/en-us/library/ff650305.aspx#paght000011_step1
        /// </summary>
        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);

            formatter.AppendLine("CallerInfo:");
            formatter.AppendLine(CallerInfo);
            formatter.AppendLine("");

            if (ExtraInfo != null)
            {
                try
                {
                    var info = _serializer.Serialize(ExtraInfo);                    
                    if (!string.IsNullOrEmpty(info))
                    {
                        formatter.AppendLine("ExtraInfo:");
                        formatter.AppendLine(info);
                        formatter.AppendLine("");
                    }
                }
                catch (Exception e)
                {
                    HealthMonitoring.LogException("Error serializing: ExtraInfo", e);
                }
            }

            formatter.AppendLine("Params:");
            formatter.AppendLine(Params);
        }
        #endregion
    }
}
