using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DeadOrAlivePaper.WarrantyVoids.DeadOrAlive;
using MoreLinq;
using ReadFASTQ.WarrantyVoids.DNA;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace DNAProc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            var head1 = new FastQStreamReader("../Data/testfileR1.fastq");
            var head2 = new FastQStreamReader("../Data/testfileR2.fastq");
#else
            var head1 = new FastQStreamReader("../Data/SoilFenceLib-UNIS_S1_L001_R1_001.fastq");
            var head2 = new FastQStreamReader("../Data/SoilFenceLib-UNIS_S1_L001_R2_001.fastq");
#endif

            var heads = head1.Zip(head2, (h1, h2) => (Head1: h1, Head2: h2));

            var barcodes = BarcodeDatabase.Load("../Data/forward_T_split_map.txt").Result;
            var barcodeDict = barcodes.ToDictionary(b => b.BarcodeSequence, b => b);

            var primers = barcodes.DistinctBy(b => b.LinkerPrimerSequence)
                .Select(b => (new PrimerScanner(b.LinkerPrimerSequence, 1), new PrimerScanner(b.ReversePrimer, 1)))
                .ToList();
            var timer = new Stopwatch();
            timer.Start();

            var matches = 0;
            var nomatch = 0;
            var counter = 0;
            var expectedMax = 8413098;

            foreach (var head in heads)
                //.Select(d => DNAUtil.FindPrimer(d.Head1, d.Head2, primers))
                //.Where(m => m.valid))
            {
                if (!PrimerResult.TryFind(head.Head1, head.Head2, primers, out var value))
                {
                    nomatch++;
                    continue;
                }

                counter++;
                matches++;
                if (counter % 250000 == 0)
                {
                    Console.WriteLine(
                        $"[{counter * 1.0 / expectedMax:P}] {(matches + nomatch) / (timer.ElapsedMilliseconds / 1000.0)}s/sec");
                }


                Console.WriteLine(
                    $"Match: {value.ForwardMatch.Location}/{value.ForwardMatch.Errors} : {value.ReverseMatch.Location}/{value.ReverseMatch.Errors} : {(value.Reversed ? 1 : 0)} : {value.Forward.Header}");
            }

            timer.Stop();
            Console.WriteLine($"Matches: {matches}\nNoMatch: {nomatch}");
            Console.WriteLine(
                $"Elapsed time: {timer.ElapsedMilliseconds / 1000.0}s, {(matches + nomatch) / (timer.ElapsedMilliseconds / 1000.0)}s/sec");
        }
    }
}
