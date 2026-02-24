using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class RecaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
