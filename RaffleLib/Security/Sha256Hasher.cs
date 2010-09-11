using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace RaffleLib.Security
{
    public class Sha256Hasher : IHasher
    {
        public string Hash(string cleartext)
        {
            return BitConverter.ToString(
                new SHA256CryptoServiceProvider()
                .ComputeHash(System.Text.Encoding.ASCII.GetBytes(cleartext))
                ).Replace("-", "").ToLowerInvariant();
        }
    }
}
