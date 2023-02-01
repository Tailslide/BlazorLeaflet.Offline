using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline
{
    public class ElementClickedEventArgs : EventArgs
    {
        public string ElementId { get; set; }
    }
}
