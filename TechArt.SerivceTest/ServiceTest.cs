using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechArt.Base.FW_Shared;

namespace TechArt.SerivceTest
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void DebuggerTest()
        {
            Debugger debugger=new Debugger(true);

            for (int i = 0; i < 10000; i++)
            {
                debugger.AddMsg(i.ToString());
            }

            debugger.Dispose();

            Assert.IsTrue(true);
        }
    }
}
