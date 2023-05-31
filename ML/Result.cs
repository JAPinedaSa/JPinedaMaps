using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {

        public bool Correct { get; set; } //true, false
        public string ErrorMessage { get; set; } // va a mandar el error como un mensaje
        public object Object { get; set; }
        public List<object> Objects { get; set; }
        public Exception Ex { get; set; } // es la que va a identificar los errores ocurridos en el codigo
    }
}
