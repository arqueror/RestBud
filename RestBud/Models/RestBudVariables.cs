using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBud.Models
{
    internal static class RestBudVariables
    {
        public static string UserName { get; set; } = "";
        public static string Password { get; set; } = "";
        public static int ApiVersion { get; set; } = -1;
        public static string BaseApiUrl { get; set; } = string.Empty;
        public static BearerModel TokenModel { get; set; } = null;
    }
}
