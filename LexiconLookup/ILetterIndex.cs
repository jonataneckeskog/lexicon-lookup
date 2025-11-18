namespace LexiconLookup
{
    /// <summary>
    /// Provides mapping from a character to a letter index and tracks total letters.
    /// Designed for use in array-based letter sets.
    /// </summary>
    public interface ILetterIndex
    {
        /// <summary>
        /// Maps a character to an index in the letter array.
        /// Returns -1 if the character is not part of the alphabet.
        /// </summary>
        /// <param name="c">The character to map.</param>
        /// <returns>Array index or -1 if invalid.</returns>
        int GetIndex(char c);

        /// <summary>
        /// Maps an index back to its corresponding character.
        /// </summary>
        /// <param name="index">Index in the letter array.</param>
        /// <returns>The corresponding character.</returns>
        char GetLetter(int index);

        /// <summary>
        /// Total number of letters in the array (excluding blanks).
        /// </summary>
        int LetterCount { get; }

        /// <summary>
        /// Checks if a character represents a blank tile/wildcard.
        /// </summary>
        /// <param name="c">Character to check.</param>
        /// <returns>True if blank, false otherwise.</returns>
        bool IsBlank(char c);

        /// <summary>
        /// Enumerates all valid letters in the alphabet.
        /// </summary>
        IEnumerable<char> Letters { get; }
    }
}