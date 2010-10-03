using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RaffleLib.Security;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using WebLib.Auth;

namespace RaffleLibTests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Can_create_sha256_hash()
        {
            var cleartext = "Corned beef";
            var hash = "bb7247cf4973167d0e4ed9525c8fc15088d30a5d12269dfffd460e49f4674061";

            var result = new Sha256Hasher().Hash(cleartext);

            Assert.AreEqual(hash, result);
        }
    }
}
