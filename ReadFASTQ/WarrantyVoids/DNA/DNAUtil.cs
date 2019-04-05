using System.Collections.Generic;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace ReadFASTQ.WarrantyVoids.DNA
{
    public static class DNAUtil
    {
        public static (
            FastQSequence forward, 
            FastQSequence reverse,
            PrimerScanner.PrimerMatch forwardMatch,
            PrimerScanner.PrimerMatch reverseMatch,
            bool valid,
            bool hasBeenReversed)
            FindPrimer(FastQSequence h1, FastQSequence h2, List<(PrimerScanner forward, PrimerScanner reverse)> scanners)
        {
            foreach (var scannerPair in scanners)
            {
                var forwardMatch = scannerPair.forward.Match(h1);
                var reverseMatch = scannerPair.reverse.Match(h2);
                var toReverse = false;
                if (forwardMatch == null && reverseMatch == null)
                {
                    forwardMatch = scannerPair.forward.Match(h2);
                    reverseMatch = scannerPair.reverse.Match(h1);
                    toReverse = true;
                }

                var forwardSequence = toReverse ? h1 : h2;
                var backwardSequence = toReverse ? h2 : h1;

                if (forwardMatch != null && reverseMatch != null)
                {
                    return (forwardSequence, backwardSequence, forwardMatch, reverseMatch, true, toReverse);
                }
            }
            return (h1, h2, null, null, false, false);
        }
    }
}