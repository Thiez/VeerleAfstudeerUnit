using System.Collections.Generic;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace ReadFASTQ.WarrantyVoids.DNA
{
    public readonly struct PrimerResult
    {
        public PrimerResult(FastQSequence forward, FastQSequence reverse, PrimerScanner.PrimerMatch forwardMatch,
            PrimerScanner.PrimerMatch reverseMatch, bool reversed)
        {
            Forward = forward;
            Reverse = reverse;
            ForwardMatch = forwardMatch;
            ReverseMatch = reverseMatch;
            Reversed = reversed;
        }

        public FastQSequence Forward { get; }

        public FastQSequence Reverse { get; }

        public PrimerScanner.PrimerMatch ForwardMatch { get; }

        public PrimerScanner.PrimerMatch ReverseMatch { get; }

        public bool Reversed { get; }

        public static bool TryFind<T>(FastQSequence h1, FastQSequence h2, T scanners, out PrimerResult result)
            where T : IEnumerable<(PrimerScanner forward, PrimerScanner reverse)>
        {
            foreach (var (forward, reverse) in scanners)
            {
                if (forward.TryMatch(h1, out var forwardMatch) && reverse.TryMatch(h2, out var reverseMatch))
                {
                    result = new PrimerResult(h1, h2, forwardMatch, reverseMatch, false);
                    return true;
                }

                if (forward.TryMatch(h2, out forwardMatch) && reverse.TryMatch(h1, out reverseMatch))
                {
                    result = new PrimerResult(h2, h1, forwardMatch, reverseMatch, true);
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
