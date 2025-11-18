namespace LexiconLookup
{
    /// <summary>
    /// Represents a set of letters with their frequencies, supporting blank tiles as wildcards.
    /// </summary>
    public class LetterSet
    {
        private int[] _letterCounts;
        private int _blankCount;
        private Dictionary<char, int> _charToIndex;
        private LetterIndex _letterIndex;

        /// <summary>
        /// Creates a LetterSet from a dictionary of letter counts using the specified alphabet mapping.
        /// Blanks are represented by '?' or '*' and counted separately from the letters.
        /// </summary>
        /// <param name="letterCounts">
        /// Dictionary mapping each letter to its count. Letters not in the provided alphabet will cause an exception.
        /// </param>
        /// <param name="letterIndex">
        /// The alphabet mapping to use for indexing letters. If null, a default Swedish alphabet is used.
        /// </param>
        public LetterSet(Dictionary<char, int> letterCounts, LetterIndex? letterIndex = null)
        {
            _letterIndex = letterIndex ?? new LetterIndex("AÅÄBCDEFGHIJKLMNOPQRSTUVWXYZ", "?*");

            _letterCounts = new int[_letterIndex.LetterCount];
            _blankCount = 0;
            _charToIndex = _letterIndex.GetCharToIndex();

            foreach (KeyValuePair<char, int> kvp in letterCounts)
            {
                if (kvp.Value <= 0)
                    continue;

                char letter = char.ToUpperInvariant(kvp.Key);

                if (_letterIndex.IsBlank(letter))
                {
                    _blankCount += kvp.Value;
                }
                else
                {
                    int index = _charToIndex.ContainsKey(letter) ? _charToIndex[letter] : -1;
                    if (index >= 0)
                    {
                        _letterCounts[index] += kvp.Value;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid letter '{kvp.Key}' in letter counts.");
                    }
                }
            }
        }

        /// <summary>
        /// Creates a LetterSet from a string.
        /// </summary>
        /// <param name="letters">String containing letters to count.</param>
        /// <param name="letterIndex">Optional alphabet mapping; defaults to Swedish if null.</param>
        /// <returns>A new LetterSet instance.</returns>
        public static LetterSet FromString(string letters, LetterIndex? letterIndex = null)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();

            foreach (char character in letters)
            {
                char letter = char.ToUpperInvariant(character);

                if (counts.ContainsKey(letter))
                {
                    counts[letter]++;
                }
                else
                {
                    counts[letter] = 1;
                }
            }

            return new LetterSet(counts, letterIndex ?? null);
        }

        /// <summary>
        /// Gets the count of a specific letter in this set.
        /// </summary>
        /// <param name="letter">The letter to query (case-insensitive).</param>
        /// <returns>The count of the letter, or 0 if not present.</returns>
        public int GetCount(char letter)
        {
            int idx = _charToIndex.ContainsKey(letter) ? _charToIndex[letter] : -1;
            return idx >= 0 ? _letterCounts[idx] : 0;
        }

        /// <summary>
        /// Gets the count of blank tiles (wildcards) in this set.
        /// </summary>
        public int BlankCount => _blankCount;

        /// <summary>
        /// Gets all letters in this set.
        /// </summary>
        public IEnumerable<char> Letters
        {
            get
            {
                for (int i = 0; i < _letterCounts.Length; i++)
                {
                    if (_letterCounts[i] > 0)
                        yield return _letterIndex.GetLetter(i);
                }
            }
        }

        /// <summary>Try to consume a real letter tile. Returns true if successful.</summary>
        public bool TryUseLetter(char letter)
        {
            int idx = _charToIndex.TryGetValue(letter, out int index) ? index : -1;
            if (idx >= 0 && _letterCounts[idx] > 0)
            {
                _letterCounts[idx]--;
                return true;
            }
            return false;
        }

        /// <summary>Restore a previously used real letter tile.</summary>
        public void RestoreLetter(char letter)
        {
            int idx = _charToIndex[letter];
            if (idx >= 0)
                _letterCounts[idx]++;
            else
                throw new ArgumentException($"Invalid letter '{letter}' for restoring.");
        }

        /// <summary>Try to consume a blank tile. Returns true if successful.</summary>
        public bool TryUseBlank()
        {
            if (_blankCount > 0)
            {
                _blankCount--;
                return true;
            }
            return false;
        }

        /// <summary>Restore a previously used blank tile.</summary>
        public void RestoreBlank()
        {
            _blankCount++;
        }

        /// <summary> 
        /// Creates a copy of the letter counts for internal use. 
        /// </summary> 
        public Dictionary<char, int> GetLetterCounts()
        {
            Dictionary<char, int> result = new Dictionary<char, int>();
            for (int i = 0; i < _letterCounts.Length; i++)
            {
                if (_letterCounts[i] > 0) result[_letterIndex.GetLetter(i)] = _letterCounts[i];
            }
            return result;
        }
    }
}

