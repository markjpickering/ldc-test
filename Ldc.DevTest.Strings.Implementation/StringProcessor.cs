using Ldc.DevTest.Strings.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ldc.DevTest.Strings.Implementation
{
    public class StringProcessor : IStringProcessor
    {
        private const int MaxInputLength = 15;

        public ICollection<string> ProcessStrings(IReadOnlyCollection<string> inputStrings) =>
            new HashSet<string>(
                inputStrings
                    .Select(s => ProcessSingleString(s))
                    .Where(s => !string.IsNullOrEmpty(s))
                );

        // NOTE: We could have used regular expressions - this would be more efficient
        private string ProcessSingleString(string inputString)
        {
            var processedChars = new StringBuilder();

            if (inputString == null)
                return null;

            RemoveRepeatingChars(inputString.ToArray())
                .Where(c => c!= '4' && c!='_')
                .Select(c => c == '$' ? '£' : c)
                .Take(MaxInputLength)
                .ToList()
                .ForEach(currentChar => processedChars.Append(currentChar));

            return processedChars.ToString();
        }

        public IEnumerable<char> RemoveRepeatingChars(char[] inputChars) =>
            inputChars.Length <= 1
                ? inputChars
                : (inputChars[0], inputChars[1]) switch
                {
                    (var firstChar, var secondChar)
                        when (firstChar == secondChar) =>
                            RemoveRepeatingChars(inputChars[1..]),
                    (var firstChar, _) =>
                        new List<char> { firstChar }.Concat(RemoveRepeatingChars(inputChars[1..]))
                }; 

    }
}
