using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp1.Pages;


namespace RegistrationTest
{
    [TestClass]
    public class RegistrationTest
    {
        [TestMethod]
        public void RegistrationTestSuccess()
        {
            Page3 reg = new Page3();
            Assert.IsTrue(reg.RegistrationUser("noid@mail.ru", "123pasw", "Гусева Татьяна Андреевна", 22, "+79776523547"));
            Assert.IsTrue(reg.RegistrationUser("gurtov@mail.ru", "DastErt34", "Ситникова Александра Мариновна", 29, "+79259586542"));
            Assert.IsTrue(reg.RegistrationUser("kotik45@mail.ru", "mailn65ret", "Воронин Даниил Константинович", 27, "+79256598547"));
        }

        [TestMethod]
        public void RegistrationTestFailed()
        {
            Page3 reg1 = new Page3();

            Assert.IsFalse(reg1.RegistrationUser("ivanov@mail.ru", "pass123", "Иванов Иван Иванович", 67, "+79886589563"));
            Assert.IsFalse(reg1.RegistrationUser("angela@mail.ru", "", "Новикова Кристина Ивановна", 37, ""));
            Assert.IsFalse(reg1.RegistrationUser("", "", "", 0, ""));
            Assert.IsFalse(reg1.RegistrationUser("klubnicika@mail.ru", "56desrf45", "Исаева Любовь Валерьевна", 30, "+797745"));
            Assert.IsFalse(reg1.RegistrationUser("sunmail.ru", "3467gtsr", "Батова Анастасия Александровна", 22, "+79775263215"));
            Assert.IsFalse(reg1.RegistrationUser("luna@.ru", "12", "Мартынов Юрий Макарович", 44, "+79774568523"));
        }
    }
}
