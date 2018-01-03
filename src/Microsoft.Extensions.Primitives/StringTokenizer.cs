// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Primitives
{
    /// <summary>
    /// Tokenizes a <c>string</c> into <see cref="StringSegment"/>s.
    /// </summary>
    public struct StringTokenizer :  IEnumerable<StringSegment>
    {
        private readonly StringSegment _value;
        private readonly char[] _separators;

        /// <summary>
        /// Initializes a new instance of <see cref="StringTokenizer"/>.
        /// </summary>
        /// <param name="value">The <c>string</c> to tokenize.</param>
        /// <param name="separators">The characters to tokenize by.</param>
        public StringTokenizer(string value, char[] separators) : this((StringSegment)value, separators)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StringTokenizer"/>.
        /// </summary>
        /// <param name="value">The <c>StringSegment</c> to tokenize.</param>
        /// <param name="separators">The characters to tokenize by.</param>
        public StringTokenizer(StringSegment value, char[] separators)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            if (separators == null)
            {
                throw new ArgumentNullException(nameof(separators));
            }

            _value = value;
            _separators = separators;
        }

        public Enumerator GetEnumerator() => new Enumerator(ref this);

        IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly char[] _separators;

            public Enumerator(ref StringTokenizer tokenizer)
            {
                _value = tokenizer._value;
                _separators = tokenizer._separators;
                Current = default;
                Index = 0;
            }

            /// <summary>
            /// <see cref="StringSegment" /> that represents the value of the string between the
            /// previous and current separator.
            /// </summary>
            public StringSegment Current { get; private set; }

            /// <summary>
            /// Index of the current separator.
            /// </summary>
            public int Index { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_value == null || Index > _value.Length)
                {
                    Current = default(StringSegment);
                    return false;
                }

                var next = _value.IndexOfAny(_separators, Index);
                if (next == -1)
                {
                    // No separator found. Consume the remainder of the string.
                    next = _value.Length;
                }

                Current = _value.Subsegment(Index, next - Index);
                Index = next + 1;

                return true;
            }

            public void Reset()
            {
                Current = default;
                Index = 0;
            }
        }
    }
}
