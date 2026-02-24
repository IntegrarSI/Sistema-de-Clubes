using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace BL.Utils
{
    public static partial class Helper
    {
        public static bool validarEmail(string mail)
        {
            if (mail != null && mail.Length > 0)
            {
                try
                {
                    new MailAddress(mail);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
