using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils.Tests
{
    /// <summary>
    /// Defines a custom data class
    /// </summary>
    class MyData
    {
        public int Id;
        public string Name;

        public MyData(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    [TestClass()]
    public class LimitedStackTests
    {
        [TestMethod()]
        public void LimitedStackConstructorTest()
        {
            var newIntStack = new LimitedStack<int>(4);
            Assert.AreNotEqual(newIntStack, null);
            Assert.AreEqual(newIntStack.Size, 4);

            var newBoolStack = new LimitedStack<bool>(10);
            Assert.AreNotEqual(newBoolStack, null);
            Assert.AreEqual(newBoolStack.Size, 10);

            var newDataStack = new LimitedStack<MyData>(100);
            Assert.AreNotEqual(newDataStack, null);
            Assert.AreEqual(newDataStack.Size, 100);
        }

        [TestMethod()]
        public void InvalidConstructorSizeTest()
        {
            var invalidSizes = new List<int>() { -1, -2, 0 };

            foreach (var invalidSize in invalidSizes)
            {
                try
                {
                    var newIntStack = new LimitedStack<int>(invalidSize);
                    throw new Exception("Should not pass with size of " + invalidSize);
                }
                catch (Exception e)
                {
                    var expectedError = e as ArgumentOutOfRangeException;
                    if (expectedError == null)
                    {
                        throw e;
                    }
                }
            }
        }

        [TestMethod()]
        public void PushAndPeekTest()
        {
            var newIntStack = new LimitedStack<int>(4);
            Assert.AreEqual(newIntStack.Count, 0);

            for (int i = 1; i <= 4; i++)
            {
                newIntStack.Push(i);
                Assert.AreEqual(newIntStack.Count, i);
                var top = newIntStack.Peek();
                Assert.AreEqual(top, i);
            }

            newIntStack.Push(5);
            var afterOverflowTop = newIntStack.Peek();
            Assert.AreEqual(afterOverflowTop, 5);
            Assert.AreEqual(newIntStack.Count, 4);

            for (int i = 5; i >= 2; i--)
            {
                Assert.AreEqual(newIntStack.Pop(), i);
            }

            Assert.AreEqual(newIntStack.Peek(), 0);
            Assert.AreEqual(newIntStack.Count, 0);
        }
    }
}