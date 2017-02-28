using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGenerator;

namespace UnitTests
{
    [TestClass]
    public class CodeSerializerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var scanner = new CodeSerializer(null);

            var login = "admin";

            var token = scanner.SerializeCode(login);

            var deserializedLogin = scanner.DeserializeCode(token);

            Assert.AreEqual(login, deserializedLogin);
        }
    }
}
