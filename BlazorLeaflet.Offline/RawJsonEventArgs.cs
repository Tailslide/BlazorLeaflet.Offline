using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline
{

    public class RawJsonEventArgs : EventArgs
    {
        public JsonDocument Json{ get; set; }
    }
}
