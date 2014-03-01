using System;
using GraphLabs.Utils;
using NUnit.Framework;

namespace GraphLabs.Tests.Utils
{
    /// <summary> Тесты <see cref="ExpressionUtility"/> </summary>
    [TestFixture]
    public class ExpressionsUtilityTests
    {
        #region Тестовые методы-свойства

        private object SomeProperty { get; set; }

        private void SomeMethod()
        {
            throw new NotImplementedException();
        }

        private object SomeGenericMethodWithReturnValueAndArg<T>(T arg)
        {
            throw new NotImplementedException();
        }

        private class MyClass
        {
            public bool Foo(string a, DateTime b)
            {
                return false;
            }

            public void Foo(string a, DateTime b, bool c)
            {

            }
        }

        #endregion


        /// <summary> NameForMember </summary>
        [Test]
        public void TestNameForMember()
        {
            var name = ExpressionUtility.NameForMember(() => this.SomeProperty);
            Assert.AreEqual("SomeProperty", name);

            name = ExpressionUtility.NameForMember((ExpressionsUtilityTests s) => s.SomeProperty);
            Assert.AreEqual("SomeProperty", name);
        }

        /// <summary> NameForMethod </summary>
        [Test]
        public void TestNameForMethod()
        {
            var name1 = ExpressionUtility.NameForMethod<ExpressionsUtilityTests, int, object>(t => t.SomeGenericMethodWithReturnValueAndArg);
            Assert.AreEqual("SomeGenericMethodWithReturnValueAndArg", name1);

            var name2 = ExpressionUtility.NameForMethod<MyClass, string, DateTime, bool>(i => i.Foo);
            Assert.AreEqual("Foo", name2);

            var myObject = new MyClass();
            var name3 = ExpressionUtility.NameForMethod<string, DateTime, bool>(() => myObject.Foo);
            Assert.AreEqual("Foo", name3);
        }

        /// <summary> NameForAction </summary>
        [Test]
        public void TestNameForAction()
        {
            var name1 = ExpressionUtility.NameForAction(() => this.SomeMethod);
            Assert.AreEqual("SomeMethod", name1);

            var name2 = ExpressionUtility.NameForAction<MyClass, string, DateTime, bool>(i => i.Foo);
            Assert.AreEqual("Foo", name2);

            var myObject = new MyClass();
            var name3 = ExpressionUtility.NameForAction<string, DateTime, bool>(() => myObject.Foo);
            Assert.AreEqual("Foo", name3);
        }
    }
}
