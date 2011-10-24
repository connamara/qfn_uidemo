using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using UIDemo.ViewModel;
using UnitTests.Util;

namespace UnitTests
{
    [TestFixture]
    public class ConnectionTests
    {
        [Test]
        public void StartupStateTest()
        {
            UnitTestContext context = new UnitTestContext();
            ConnectionViewModel vm = new ConnectionViewModel(context.App);

            Assert.AreEqual("FIX.4.2:SENDER->TARGET", vm.SessionString);
            Assert.False(vm.IsConnected);
        }

        [Test]
        public void ConnectAndDisconnect()
        {
            UnitTestContext context = new UnitTestContext();
            ConnectionViewModel vm = new ConnectionViewModel(context.App);

            Assert.False(vm.IsConnected);

            vm.ConnectCommand.Execute(null);
            context.Login();
            Assert.True(vm.IsConnected);

            vm.DisconnectCommand.Execute(null);
            context.Logout();
            Assert.False(vm.IsConnected);
        }
    }
}
