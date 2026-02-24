using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Negocio.Utils
{
    public class Encriptacion
    {
        public static string encriptarSHA1(string texto)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] resultado = sha1.ComputeHash(Encoding.UTF8.GetBytes(texto));

            //se conviernte el resultado que viene en bytes[] a hexa (string)
            string textoEncriptado = "";
            foreach (byte x in resultado)
            {
                textoEncriptado += String.Format("{0:x2}", x);
            }

            return textoEncriptado;
        }

        public static string encriptarBase64(string texto)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(texto);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string desencriptarBase64(string texto)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(texto);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
