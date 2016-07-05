using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RestBud.Models
{

    public class BearerModel
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
