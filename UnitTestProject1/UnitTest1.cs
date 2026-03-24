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
        public void TestMethod1()
        {
            Page2 auth = new Page2();
            Assert.IsTrue(auth.AuthUser("ivanov@mail.ru", "pass123"));
            Assert.IsFalse(auth.AuthUser("murt@mail.ru", "9876pot"));
        }
    }
}
