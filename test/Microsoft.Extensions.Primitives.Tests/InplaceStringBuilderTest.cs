// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Microsoft.Extensions.Primitives
{
    public class InplaceStringBuilderTest
    {
        [Fact]
        public void ToString_ReturnsStringWithAllAppendedValues()
        {
            var s1 = "123";
            var c1 = '4';
            var s2 = "56789";
            var seg = new StringSegment("890123", 2, 2);
            Assert.Equal("01", seg);
            Assert.Equal("01", seg.ToString());
            Assert.Equal(2, seg.Length);


            var formatter = new InplaceStringBuilder();
            formatter.Capacity += s1.Length;
            formatter.Append(s1);
            Assert.Equal(s1, formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += s1.Length + 1;
            formatter.Append(s1);
            formatter.Append(c1);
            Assert.Equal(s1 + c1, formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += s1.Length + 1 + 2;
            formatter.Append(s1);
            formatter.Append(c1);
            formatter.Append(s2, 0, 2);
            Assert.Equal(s1 + c1 + s2.Substring(0, 2), formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += s1.Length + 1 + 3;
            formatter.Append(s1);
            formatter.Append(c1);
            formatter.Append(s2, 0, 2);
            formatter.Append(s2, 4, 1);
            Assert.Equal(s1 + c1 + s2.Substring(0, 2) + s2.Substring(4, 1), formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += s1.Length + 1 + s2.Length + seg.Length;
            formatter.Append(s1);
            formatter.Append(c1);
            formatter.Append(s2, 0, 2);
            formatter.Append(s2, 2, 2);
            formatter.Append(s2, 4, 1);
            formatter.Append(seg);
            Assert.Equal(s1 + c1 + s2 + seg.ToString(), formatter.ToString());
        }

        [Fact]
        public void ToString_ReturnsStringWithAllAppendedValues1()
        {
            var s2 = "56789";

            var formatter = new InplaceStringBuilder();
            formatter.Capacity += 2;
            formatter.Append(s2, 0, 2);
            Assert.Equal(s2.Substring(0,2), formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += 2;
            formatter.Append(s2, 2, 2);
            Assert.Equal(s2.Substring(2, 2), formatter.ToString());

            formatter = new InplaceStringBuilder();
            formatter.Capacity += 1;
            formatter.Append(s2, 4, 1);
            Assert.Equal(s2.Substring(4, 1), formatter.ToString());
        }

        [Fact]
        public void Build_ThrowsIfNotEnoughWritten()
        {
            var formatter = new InplaceStringBuilder(5);
            formatter.Append("123");
            var exception = Assert.Throws<InvalidOperationException>(() => formatter.ToString());
            Assert.Equal("Entire reserved capacity was not used. Capacity: '5', written '3'.", exception.Message);
        }

        [Fact]
        public void Capacity_ThrowsIfAppendWasCalled()
        {
            var formatter = new InplaceStringBuilder(3);
            formatter.Append("123");

            var exception = Assert.Throws<InvalidOperationException>(() => formatter.Capacity = 5);
            Assert.Equal("Cannot change capacity after write started.", exception.Message);
        }

        [Fact]
        public void Append_ThrowsIfNotEnoughSpace()
        {
            var formatter = new InplaceStringBuilder(1);

            var exception = Assert.Throws<InvalidOperationException>(() => formatter.Append("123"));
            Assert.Equal("Not enough capacity to write '3' characters, only '1' left.", exception.Message);
        }
    }
}
