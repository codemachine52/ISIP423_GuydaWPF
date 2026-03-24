using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Navigation;
using WpfApp1.Pages;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AuthTest()
        {
            Page2 auth = new Page2();
            Assert.IsTrue(auth.AuthUser("ivanov@mail.ru", "pass123"));
            Assert.IsFalse(auth.AuthUser("murt@mail.ru", "9876pot"));
            Assert.IsFalse(auth.AuthUser("", ""));
            Assert.IsFalse(auth.AuthUser(" ", " "));
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            Page2 auth = new Page2();
            Assert.IsTrue(auth.AuthUser("ivanov@mail.ru", "pass123"));
            Assert.IsTrue(auth.AuthUser("petrova@gmail.com", "anna2023"));
            Assert.IsTrue(auth.AuthUser("sidorov@yandex.ru", "alex17"));
            Assert.IsTrue(auth.AuthUser("maria.k@mail.ru", "maria_pass"));
            Assert.IsTrue(auth.AuthUser("den_nik@mail.ru", "den35"));
            Assert.IsTrue(auth.AuthUser("mur123@mail.ru", "cvpasdw"));
            Assert.IsTrue(auth.AuthUser("238947sahd@gmail.com", "AJhsjdshgfi"));
            Assert.IsTrue(auth.AuthUser("alksdj@mail.ru", "QWERTY1"));
            Assert.IsTrue(auth.AuthUser("jkjgjehgxb@mail.ru", "xcvbnm"));
        }

        [TestMethod]
        public void AuthTestFail()
        {
            Page2 auth = new Page2();

            Assert.IsFalse(auth.AuthUser("ivanov@mail.ru", "notValid"));
            Assert.IsFalse(auth.AuthUser("germi4@mail.ru", "pass45"));
            Assert.IsFalse(auth.AuthUser("", ""));
            Assert.IsFalse(auth.AuthUser("Kotin =’2’", "Kotin =’2’"));
        }
    }
}
