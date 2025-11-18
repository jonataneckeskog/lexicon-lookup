using System;
using System.Collections.Generic;
using System.Linq;

namespace LexiconLookup
{
    /// <summary>
    /// Provides direct mapping from characters to letter indices.
    /// Can be initialized with a string of valid letters and a string of blank tiles.
    /// </summary>
    public class LetterIndex
    {
        private readonly HashSet<char> _blanks;
        private readonly Dictionary<char, int> _charToIndex;

        /// <summary>
        /// Creates a new LetterIndex.
        /// </summary>
        /// <param name="validLetters">A string of letters representing the alphabet (order matters).</param>
        /// <param name="blankTiles">A string of characters considered blank tiles.</param>
        public LetterIndex(string validLetters, string blankTiles)
        {
            if (string.IsNullOrEmpty(validLetters))
                throw new ArgumentException("Valid letters cannot be null or empty.", nameof(validLetters));

            _blanks = (blankTiles ?? string.Empty).ToHashSet();

            _charToIndex = new Dictionary<char, int>();
            for (int i = 0; i < validLetters.Length; i++)
            {
                char lower = char.ToLower(validLetters[i]);
                char upper = char.ToUpper(validLetters[i]);

                if (!_charToIndex.ContainsKey(lower))
                    _charToIndex[lower] = i;

                if (!_charToIndex.ContainsKey(upper))
                    _charToIndex[upper] = i;
            }
        }

        /// <summary>
        /// Gets the full mapping of characters to their indices in the alphabet.
        /// Returns -1 for characters not in the alphabet.
        /// </summary>
        public Dictionary<char, int> GetCharToIndex()
        {
            // Return a copy to prevent external modification
            return new Dictionary<char, int>(_charToIndex);
        }

        /// <summary>
        /// Total number of letters in the alphabet.
        /// </summary>
        public int LetterCount => _charToIndex.Count / 2; // Because both upper and lower are stored

        /// <summary>
        /// All letters in the alphabet in order.
        /// </summary>
        public IEnumerable<char> Letters => _charToIndex.Keys
                                                    .Where(c => char.IsLower(c))
                                                    .OrderBy(c => _charToIndex[c]);

        /// <summary>
        /// Gets the letter corresponding to the specified index.
        /// Throws ArgumentOutOfRangeException if the index is invalid.
        /// </summary>
        public char GetLetter(int idx)
        {
            char[] letters = _charToIndex.Keys
                                          .Where(c => char.IsUpper(c))
                                          .OrderBy(c => _charToIndex[c])
                                          .ToArray();

            if (idx < 0 || idx >= letters.Length)
                throw new ArgumentOutOfRangeException(nameof(idx));

            return letters[idx];
        }

        /// <summary>
        /// Returns true if the character is a blank tile.
        /// </summary>
        public bool IsBlank(char c)
        {
            return _blanks.Contains(c);
        }
    }
}