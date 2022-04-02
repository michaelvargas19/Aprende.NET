using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Contracts.Middle
{
    public interface ISecurityManager
    {
        string Encrypt(string key, string plainText);
        string Decrypt(string key, string encryptedText);
    }
}
