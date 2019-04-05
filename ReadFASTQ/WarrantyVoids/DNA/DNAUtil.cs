using System.Collections.Generic;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace ReadFASTQ.WarrantyVoids.DNA
{
    public static class DNAUtil
    {
        public static (
            FastQSequence forward, 
            FastQSequence backward,
            PrimerScanner.PrimerMatch forwardMatch,
            PrimerScanner.PrimerMatch backwardMatch,
            bool valid,
            bool hasBeenReversed)
            FindPrimer(FastQSequence h1, FastQSequence h2, List<(PrimerScanner forward, PrimerScanner backward)> scanners)
        {
            foreach (var scannerPair in scanners)
            {
                var forwardMatch = scannerPair.forward.Match(h1);
                var backwardMatch = scannerPair.backward.Match(h2);
                var toReverse = false;
                if (forwardMatch == null && backwardMatch == null)
                {
                    forwardMatch = scannerPair.forward.Match(h2);
                    backwardMatch = scannerPair.backward.Match(h1);
                    toReverse = true;
                }

                var forwardSequence = toReverse ? h1 : h2;
                var backwardSequence = toReverse ? h2 : h1;

                if (forwardMatch != null && backwardMatch != null)
                {
                    return (forwardSequence, backwardSequence, forwardMatch, backwardMatch, true, toReverse);
                }
            }
            return (h1, h2, null, null, false, false);
        }
    }
}