using System.Collections.Generic;

namespace Ldc.DevTest.Strings.Contracts
{
    public interface IStringProcessor
    {
        ICollection<string> ProcessStrings(IReadOnlyCollection<string> inputStrings);
    }
}
