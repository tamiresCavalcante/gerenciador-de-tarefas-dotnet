using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Utils
{
    public class MDUtils
    {
        public static string GerarHashMD5(string input)
        {
            MD5 md5hash = MD5.Create();
            var bytes = md5hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            foreach(var b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }
    }
}
