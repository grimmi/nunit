﻿// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.TestData.TestFixtureSourceData
{
    public abstract class TestFixtureSourceTest
    {
        private string Arg;
        private string Expected;

        public TestFixtureSourceTest(string arg, string expected)
        {
            Arg = arg;
            Expected = expected;
        }

        [Test]
        public void CheckSource()
        {
            Assert.That(Arg, Is.EqualTo(Expected));
        }
    }

    public abstract class TestFixtureSourceDivideTest
    {
        private int X;
        private int Y;
        private int Z;

        public TestFixtureSourceDivideTest(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [Test]
        public void CheckSource()
        {
            Assert.That(X / Y, Is.EqualTo(Z));
        }
    }

    [TestFixtureSource("StaticField")]
    public class StaticField_SameClass : TestFixtureSourceTest
    {
        public StaticField_SameClass(string arg) : base(arg, "StaticFieldInClass") { }

        static object[] StaticField = new object[] { "StaticFieldInClass" };
    }

    [TestFixtureSource("StaticProperty")]
    public class StaticProperty_SameClass : TestFixtureSourceTest
    {
        public StaticProperty_SameClass(string arg) : base(arg, "StaticPropertyInClass") { }

        static object[] StaticProperty
        {
            get { return new object[] { new object[] { "StaticPropertyInClass" } }; }
        }
    }

    [TestFixtureSource("StaticMethod")]
    public class StaticMethod_SameClass : TestFixtureSourceTest
    {
        public StaticMethod_SameClass(string arg) : base(arg, "StaticMethodInClass") { }

        static object[] StaticMethod()
        {
            return new object[] { new object[] { "StaticMethodInClass" } };
        }
    }

    [TestFixtureSource("InstanceField")]
    public class InstanceField_SameClass : TestFixtureSourceTest
    {
        public InstanceField_SameClass(string arg) : base(arg, "InstanceFieldInClass") { }

        object[] InstanceField = new object[] { "InstanceFieldInClass" };
    }

    [TestFixtureSource("InstanceProperty")]
    public class InstanceProperty_SameClass : TestFixtureSourceTest
    {
        public InstanceProperty_SameClass(string arg) : base(arg, "InstancePropertyInClass") { }

        object[] InstanceProperty
        {
            get { return new object[] { new object[] { "InstancePropertyInClass" } }; }
        }
    }

    [TestFixtureSource("InstanceMethod")]
    public class InstanceMethod_SameClass : TestFixtureSourceTest
    {
        public InstanceMethod_SameClass(string arg) : base(arg, "InstanceMethodInClass") { }

        object[] InstanceMethod()
        {
            return new object[] { new object[] { "InstanceMethodInClass" } };
        }
    }

    [TestFixtureSource(typeof(SourceData), "StaticField")]
    public class StaticField_DifferentClass : TestFixtureSourceTest
    {
        public StaticField_DifferentClass(string arg) : base(arg, "StaticField") { }
    }

    [TestFixtureSource(typeof(SourceData), "StaticProperty")]
    public class StaticProperty_DifferentClass : TestFixtureSourceTest
    {
        public StaticProperty_DifferentClass(string arg) : base(arg, "StaticProperty") { }
    }

    [TestFixtureSource(typeof(SourceData), "StaticMethod")]
    public class StaticMethod_DifferentClass : TestFixtureSourceTest
    {
        public StaticMethod_DifferentClass(string arg) : base(arg, "StaticMethod") { }
    }

    [TestFixtureSource(typeof(SourceData_IEnumerable))]
    public class IEnumerableSource : TestFixtureSourceTest
    {
        public IEnumerableSource(string arg) : base(arg, "SourceData_IEnumerable") { }
    }

    [TestFixtureSource("MyData")]
    public class SourceReturnsObjectArray : TestFixtureSourceDivideTest
    {
        public SourceReturnsObjectArray(int x, int y, int z) : base(x, y, z) { }

        static IEnumerable MyData()
        {
            yield return new object[] { 12, 4, 3 };
            yield return new object[] { 12, 3, 4 };
            yield return new object[] { 12, 6, 2 };
        }
    }

    [TestFixtureSource("MyData")]
    public class SourceReturnsFixtureParameters : TestFixtureSourceDivideTest
    {
        public SourceReturnsFixtureParameters(int x, int y, int z) : base(x, y, z) { }

        static IEnumerable MyData()
        {
            yield return new TestFixtureParameters(12, 4, 3);
            yield return new TestFixtureParameters(12, 3, 4);
            yield return new TestFixtureParameters(12, 6, 2);
        }
    }

    [TestFixture]
    [TestFixtureSource("MyData")]
    public class ExtraTestFixtureAttributeIsIgnored : TestFixtureSourceDivideTest
    {
        public ExtraTestFixtureAttributeIsIgnored(int x, int y, int z) : base(x, y, z) { }

        static IEnumerable MyData()
        {
            yield return new object[] { 12, 4, 3 };
            yield return new object[] { 12, 3, 4 };
            yield return new object[] { 12, 6, 2 };
        }
    }

    [TestFixture]
    [TestFixtureSource("MyData")]
    [TestFixtureSource("MoreData", Category = "Extra")]
    [TestFixture(12, 12, 1)]
    public class TestFixtureMayUseMultipleSourceAttributes : TestFixtureSourceDivideTest
    {
        public TestFixtureMayUseMultipleSourceAttributes(int n, int d, int q) : base(n, d, q) { }

        static IEnumerable MyData()
        {
            yield return new object[] { 12, 4, 3 };
            yield return new object[] { 12, 3, 4 };
            yield return new object[] { 12, 6, 2 };
        }

        static object[] MoreData = new object[] {
            new object[] { 12, 1, 12 },
            new object[] { 12, 2, 6 } };
    }

    #region Source Data Classes

    class SourceData_IEnumerable : IEnumerable
    {
        public SourceData_IEnumerable()
        {
        }

        public IEnumerator GetEnumerator()
        {
            yield return "SourceData_IEnumerable";
        }
    }

    class SourceData
    {
        static object[] StaticField = new object[] { "StaticField" };

        static object[] StaticProperty
        {
            get { return new object[] { new object[] { "StaticProperty" } }; }
        }

        static object[] StaticMethod()
        {
            return new object[] { new object[] { "StaticMethod" } };
        }
    }

    #endregion
}
