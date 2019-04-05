using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ReadFASTQ.WarrantyVoids.FastQ
{
    /// <summary>
    ///     A reader to read FASTQ-files.
    /// </summary>
    public class FastQStreamReader : IDisposable
    {
        private TextReader reader;
        
        public FastQStreamReader(TextReader input)
        {
            reader = input;
        }

        public void Dispose()
        {
            reader?.Dispose();
        }

        public IEnumerable<FastQSequence> Values()
        {
            while (true)
            {
                var header = reader.ReadLine();
                var sequence = reader.ReadLine();
                var optional = reader.ReadLine();
                var quality = reader.ReadLine();
                if (header == null ||
                    sequence == null ||
                    optional == null ||
                    quality == null)
                    yield break;
                yield return new FastQSequence
                {
                    Header = header,
                    Optional = optional,
                    Sequence = sequence,
                    Quality = quality
                };
            }
        }
    }
}