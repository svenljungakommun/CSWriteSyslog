/*
------------------------------------------------------------------------------
CSWriteSyslog.cs

Structured JSON Syslog Logger for .NET (TCP)

DESCRIPTION

    CSWriteSyslog provides a lightweight and dependency-free mechanism for
    emitting structured log entries as compact JSON over raw TCP to a syslog
    receiver such as syslog-ng, rsyslog, or Graylog.

    The function is designed for integration into automation scripts, services,
    CLI tools, or back-end processes where centralized logging is required.
    All events are timestamped in UTC and tagged with service/process metadata.

PARAMETERS

    string service       - Logical application or service name.
    string process       - Module, subsystem or script name.
    string action        - Describes what action is being logged (e.g. "Deploy").
    string result        - Outcome of the action (e.g. "Success", "Error").
    string message       - Descriptive free-text message.
    string category      - Optional tag or classification ("Monitoring", "Security").
    string syslogHost    - Destination syslog server (IP or hostname).
    int    syslogPort    - TCP port for syslog receiver (default: 514).
    string user          - Identity of the user (default: Environment.UserName).
    string serverName    - Hostname of the sending system (default: Environment.MachineName).

OUTPUT FORMAT

    {
        "timestamp": "2025-07-20T11:24:00Z",
        "service": "MyApp",
        "process": "Startup",
        "server": "MYSERVER01",
        "action": "Initialize",
        "result": "Success",
        "message": "Initialization complete.",
        "category": "System",
        "user": "AdminUser",
        "version": "1.0"
    }

EXAMPLE

    CSWriteSyslog.Send(
        service: "MyApp",
        process: "Startup",
        action: "Initialize",
        result: "Success",
        message: "Initialization complete.",
        category: "System",
        syslogHost: "192.168.1.10"
    );

NOTES

    Author  : Odd-Arne Haraldsen
    Version : 1.0
    License : MIT
    Tags    : Logging, Syslog, JSON, TCP, Monitoring, .NET

------------------------------------------------------------------------------
*/

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SyslogClient
{
    public class CSWriteSyslog
    {
        /// <summary>
        /// Sends a structured JSON log entry to a syslog server over TCP.
        /// </summary>
        public static void Send(
            string service = "GenericService",
            string process = "GenericProcess",
            string action = "unspecified",
            string result = "undefined",
            string message = "generic message",
            string category = "undefined",
            string syslogHost = "127.0.0.1",
            int syslogPort = 514,
            string user = null,
            string serverName = null)
        {
            try
            {
                var payload = new
                {
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    service = service,
                    process = process,
                    server = serverName ?? Environment.MachineName,
                    action = action,
                    result = result,
                    message = message,
                    category = category,
                    user = user ?? Environment.UserName,
                    version = "1.0"
                };

                string json = JsonSerializer.Serialize(payload);

                using var client = new TcpClient();
                client.Connect(syslogHost, syslogPort);

                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                writer.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[CSWriteSyslog] Logging failed: {ex.Message}");
            }
        }
    }
}
