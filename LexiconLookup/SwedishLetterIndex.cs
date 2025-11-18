using System;
using System.Collections.Generic;

namespace LexiconLookup
{
    /// <summary>
    /// Maps Swedish letters (A–Z + ÅÄÖ) to array indices for fast letter set operations.
    /// Blanks ('?' or '*') are handled separately.
    /// </summary>
    public class SwedishLetterIndex : ILetterIndex
    {
        // Swedish alphabet in order
        private static readonly char[] _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ".ToCharArray();

        // Lookup table: maps char to index in the letter array, -1 if invalid
        private readonly int[] _map;

        /// <summary>
        /// Constructor builds the lookup table once
        /// </summary>
        public SwedishLetterIndex()
        {
            _map = new int[char.MaxValue + 1];

            // Initialize all entries to -1
            for (int i = 0; i < _map.Length; i++)
                _map[i] = -1;

            // Map Swedish letters to indices
            for (int index = 0; index < _letters.Length; index++)
            {
                char upper = _letters[index];
                char lower = char.ToLowerInvariant(upper);

                _map[upper] = index;
                _map[lower] = index;
            }
        }

        /// <inheritdoc />
        public int LetterCount => _letters.Length;

        /// <inheritdoc />
        public IEnumerable<char> Letters => _letters; // enumerable of all letters in order

        /// <inheritdoc />
        public int GetIndex(char c)
        {
            return _map[c];
        }

        /// <inheritdoc />
        public bool IsBlank(char c)
        {
            return c == '?' || c == '*';
        }

        /// <inheritdoc />
        public char GetLetter(int index)
        {
            if (index < 0 || index >= _letters.Length)
                throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {_letters.Length - 1}.");
            return _letters[index];
        }
    }
}
