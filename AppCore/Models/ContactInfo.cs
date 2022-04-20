using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class ContactInfo
    {
        public List<Address> address { get; set; }    
        public string name { get; set; }    
        public string surName { get; set; }
        public string company { get; set; }
        public int id { get; set; }
    }
}
