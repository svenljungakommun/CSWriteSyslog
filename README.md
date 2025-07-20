# CSWriteSyslog – Structured JSON Syslog Logger for .NET (TCP)

**CSWriteSyslog** is a compact and dependency-free C# library for logging structured events as JSON over **raw TCP** to a syslog-compatible logging backend.  
Ideal for .NET developers who want lightweight telemetry, centralized monitoring, or automated audit logging.

---

## ✅ Features

- Emits compact JSON log entries
- Raw TCP socket – works with `syslog-ng`, `rsyslog`, `Graylog`, `Fluentd`, etc.
- No external dependencies (pure .NET)
- Suitable for use in services, CLI tools, jobs, or backend applications
- Easy to extend

---

## 🚀 Quick Example

```csharp
using SyslogClient;

CSWriteSyslog.Send(
    service: "MyApp",
    process: "Startup",
    action: "Initialize",
    result: "Success",
    message: "Initialization complete.",
    category: "System",
    syslogHost: "192.168.1.10"
);
````

---

## 📥 Parameters

| Parameter    | Description                             | Default                   |
| ------------ | --------------------------------------- | ------------------------- |
| `service`    | Logical name of the service or system   | `"GenericService"`        |
| `process`    | Script, job or module name              | `"GenericProcess"`        |
| `action`     | Description of the performed action     | `"unspecified"`           |
| `result`     | Result status (e.g., "Success", "Fail") | `"undefined"`             |
| `message`    | Free-text log message                   | `"generic message"`       |
| `category`   | Optional tag/classification             | `"undefined"`             |
| `syslogHost` | Destination syslog server (IP/FQDN)     | `"127.0.0.1"`             |
| `syslogPort` | TCP port on the syslog server           | `514`                     |
| `user`       | User name or identity context           | `Environment.UserName`    |
| `serverName` | Originating hostname                    | `Environment.MachineName` |

---

## 📤 Example Output (JSON)

```json
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
```

---

## 🔧 Customization

You can modify the internal payload in `CSWriteSyslog.cs` to include additional fields such as:

```csharp
severity = "info",
correlationId = Guid.NewGuid().ToString(),
environment = "production"
```

Want to send GELF or NDJSON instead of standard JSON? Swap the formatter.

---

## 📦 Requirements

* .NET 6+, .NET Core 3.1+, or .NET Framework 4.7.2+
* A syslog receiver that supports raw TCP (no TLS assumed)

---
