using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.LogService;

public class LogMessage
{
    public virtual EventLevel EventLevel { get; set; }
    public virtual string Message { get; set; }
    public string EventPoint { get; set; }

    public LogMessage()
    {
        this.EventLevel = EventLevel.Info;
        this.EventPoint = string.Empty;
        this.Message = string.Empty;
    }

    public LogMessage(EventLevel eventLevel, string message, string eventPoint)
    {
        EventLevel = eventLevel;
        Message = message;
        EventPoint = eventPoint;
    }

    public override string ToString()
        => this.Message;
}
