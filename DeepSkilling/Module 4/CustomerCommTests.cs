using System;
using NUnit.Framework;
using Moq;
using CustomerCommLib;

namespace CustomerCommTestsNamespace
{
    [TestFixture]
    public class CustomerCommTests
    {
        private Mock<IMailSender> _mockMailSender;
        private CustomerComm _customerComm;

        [SetUp]
        public void Init()
        {
            // 1. Create Mock object of IMailSender
            _mockMailSender = new Mock<IMailSender>();

            // 2. Configure Mock object: SendMail accepts any two string arguments and returns true
            _mockMailSender.Setup(m => m.SendMail(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(true);

            // 3. Inject dependency (IMailSender) through constructor
            _customerComm = new CustomerComm(_mockMailSender.Object);
        }

        [Test]
        [TestCase(ExpectedResult = true)]
        public bool Test_SendMailToCustomer_ReturnsTrue()
        {
            // 4. Invoke SendMailToCustomer and assert the return value is true
            bool result = _customerComm.SendMailToCustomer();
            return result;
        }

        [Test]
        public void Test_SendMailToCustomer_VerifiesSendMailWasCalled()
        {
            _customerComm.SendMailToCustomer();

            // Verify that SendMail was called once with target customer address and any message
            _mockMailSender.Verify(m => m.SendMail("cust123@abc.com", It.IsAny<string>()), Times.Once());
        }
    }
}
