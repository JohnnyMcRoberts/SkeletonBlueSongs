using System;

namespace SongsDatabase.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SongsDatabase.Security;



    [TestClass]
    public class SimpleHashTest
    {
        [TestMethod]
        public void TestMethodHashMD5()
        {
            string password = "myP@5sw0rd";  // original password
            string wrongPassword = "password";    // wrong password

            byte[] salt = null;

            string passwordHashMD5 =
                SimpleHash.ComputeHash(password, "MD5", ref salt);

            Assert.IsNotNull(salt);
            Assert.IsTrue(salt.Length > 0);

            Console.WriteLine("COMPUTING HASH VALUES\n");
            Console.WriteLine("MD5   : {0}", passwordHashMD5);

            Assert.IsTrue(
                SimpleHash.VerifyHash(password, "MD5", passwordHashMD5)
                );

            Assert.IsFalse(
                SimpleHash.VerifyHash(wrongPassword, "MD5", passwordHashMD5)
            );
        }

        [TestMethod]
        public void TestMethodHashSHA1()
        {
            string password = "myP@5sw0rd1";  // original password
            string wrongPassword = "password1";    // wrong password

            byte[] salt = null;

            string passwordHashSHA1 =
                SimpleHash.ComputeHash(password, "SHA1", ref salt);

            Assert.IsNotNull(salt);
            Assert.IsTrue(salt.Length > 0);

            Console.WriteLine("COMPUTING HASH VALUES\n");
            Console.WriteLine("SHA1   : {0}", passwordHashSHA1);

            Assert.IsTrue(
                SimpleHash.VerifyHash(password, "SHA1", passwordHashSHA1)
            );

            Assert.IsFalse(
                SimpleHash.VerifyHash(wrongPassword, "SHA1", passwordHashSHA1)
            );
        }

        [TestMethod]
        public void TestMethodHashSHA256()
        {
            string password = "myP@5sw0rd2";  // original password
            string wrongPassword = "password2";    // wrong password

            byte[] salt = null;

            string passwordHashSHA256 =
                SimpleHash.ComputeHash(password, "SHA256", ref salt);

            Assert.IsNotNull(salt);
            Assert.IsTrue(salt.Length > 0);

            Console.WriteLine("COMPUTING HASH VALUES\n");
            Console.WriteLine("SHA256   : {0}", passwordHashSHA256);

            Assert.IsTrue(
                SimpleHash.VerifyHash(password, "SHA256", passwordHashSHA256)
            );

            Assert.IsFalse(
                SimpleHash.VerifyHash(wrongPassword, "SHA256", passwordHashSHA256)
            );
        }

        [TestMethod]
        public void TestMethodHashSHA384()
        {
            string password = "myP@5sw0rd33";  // original password
            string wrongPassword = "password33";    // wrong password

            byte[] salt = null;

            string passwordHashSHA384 =
                SimpleHash.ComputeHash(password, "SHA384", ref salt);

            Assert.IsNotNull(salt);
            Assert.IsTrue(salt.Length > 0);

            Console.WriteLine("COMPUTING HASH VALUES\n");
            Console.WriteLine("SHA384   : {0}", passwordHashSHA384);

            Assert.IsTrue(
                SimpleHash.VerifyHash(password, "SHA384", passwordHashSHA384)
            );

            Assert.IsFalse(
                SimpleHash.VerifyHash(wrongPassword, "SHA384", passwordHashSHA384)
            );
        }

        [TestMethod]
        public void TestMethodHashSHA512()
        {
            string password = "myP@5sw0rd44";  // original password
            string wrongPassword = "password44";    // wrong password

            byte[] salt = null;

            string passwordHashSHA512 =
                SimpleHash.ComputeHash(password, "SHA512", ref salt);

            Assert.IsNotNull(salt);
            Assert.IsTrue(salt.Length > 0);

            Console.WriteLine("COMPUTING HASH VALUES\n");
            Console.WriteLine("SHA512   : {0}", passwordHashSHA512);

            Assert.IsTrue(
                SimpleHash.VerifyHash(password, "SHA512", passwordHashSHA512)
            );

            Assert.IsFalse(
                SimpleHash.VerifyHash(wrongPassword, "SHA512", passwordHashSHA512)
            );
        }

    }
}
