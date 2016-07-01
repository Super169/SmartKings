using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKings
{
    class EventLog
    {
        public DateTime eventTime { get; set; }
        public string account { get; set; }
        public string action { get; set; }
        public string msg { get; set;  }
        public string displayTime {  get { return string.Format("{0:MM-dd HH:mm:ss}", this.eventTime); } }
    }
}
