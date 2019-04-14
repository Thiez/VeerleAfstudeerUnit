using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ReadFASTQ.WarrantyVoids.FastQ
{
    /// <summary>
    ///     A reader to read FASTQ-files.
    /// </summary>
    public class FastQStreamReader : IEnumerable<FastQSequence>
    {
        public FastQStreamReader(string path)
        {
            Path = path;
        }

        /// <summary>
        ///     Gets the name of the file this stream reads from.
        /// </summary>
        public string Path { get; }

        public IEnumerator<FastQSequence> GetEnumerator()
        {
            var fileReader = File.OpenText(Path);
            while (true)
            {
                var header = fileReader.ReadLine();
                var sequence = fileReader.ReadLine();
                var optional = fileReader.ReadLine();
                var quality = fileReader.ReadLine();

                if ((header ?? sequence ?? optional ?? quality) == null)
                {
                    break;
                }

                yield return new FastQSequence
                {
                    Header = header,
                    Optional = optional,
                    Sequence = sequence,
                    Quality = quality
                };
            }

            fileReader.Close();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
