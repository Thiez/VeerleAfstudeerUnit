using ReadFASTQ.WarrantyVoids.FastQ;

namespace ReadFASTQ.WarrantyVoids.DNA
{
    public class PrimerScanner
    {
        /// <summary>
        ///     Creates a new scanner.
        /// </summary>
        /// <param name="primer">The primer.</param>
        /// <param name="error">The allowable error.</param>
        public PrimerScanner(string primer, int error)
        {
            Primer = primer;
            ErrorBound = error;
        }
        
        /// <summary>
        /// Gets the value this matcher searches for.
        /// </summary>
        public string Primer { get; }
        
        /// <summary>
        /// Gets the amount of errors allowed within the match.
        /// </summary>
        public int ErrorBound { get; }

        /// <summary>
        ///     Matches DNA with the primer.
        /// </summary>
        /// <param name="sequence">The DNA to search for.</param>
        /// <returns>A primer if found, or null if none is found.</returns>
        public PrimerMatch Match(FastQSequence sequence)
        {
            var dna = sequence.Sequence;
            for (int i = 0; i < (dna.Length - Primer.Length); i++)
            {
                int errorCount = 0;
                for (int j = 0; j < Primer.Length; j++)
                {
                    if (dna[i + j] != Primer[j])
                        errorCount++;
                    if (errorCount > ErrorBound)
                        break;
                }
                if (errorCount <= ErrorBound)
                    return new PrimerMatch(i, errorCount);
            }

            return null;
        }

        /// <summary>
        /// A match within a primer.
        /// </summary>
        public class PrimerMatch
        {
            internal PrimerMatch(int location, int errors)
            {
                Location = location;
                Errors = errors;
            }
            
            /// <summary>
            ///     Gets the index of the match, measured from the beginning of the string.
            /// </summary>
            public int Location { get; }
            
            /// <summary>
            ///     Gets the amount of errors found.
            /// </summary>
            public int Errors { get; }
        }
    }
}