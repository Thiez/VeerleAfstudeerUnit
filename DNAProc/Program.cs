using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeadOrAlivePaper.WarrantyVoids.DeadOrAlive;
using MoreLinq;
using ReadFASTQ.WarrantyVoids.DNA;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace DNAProc
{
    class Program
    {
        async static Task Main(string[] args)
        {
#if DEBUG
            var head1 = new FastQStreamReader("../Data/testfileR1.fastq");
            var head2 = new FastQStreamReader("../Data/testfileR2.fastq");
#else
            var head1 = new FastQStreamReader("../Data/SoilFenceLib-UNIS_S1_L001_R1_001.fastq");
            var head2 = new FastQStreamReader("../Data/SoilFenceLib-UNIS_S1_L001_R2_001.fastq");
#endif

            var heads = head1.Zip(head2, (h1, h2) => (Head1: h1, Head2: h2));
            
            var barcodes = await BarcodeDatabase.Load("../Data/forward_T_split_map.txt");
            var barcodeDict = barcodes.ToDictionary(b => b.BarcodeSequence, b => b);

            var primers = barcodes.DistinctBy(b => b.LinkerPrimerSequence)
                .Select(b => (new PrimerScanner(b.LinkerPrimerSequence, 1), new PrimerScanner(b.ReversePrimer, 1))).ToList();
            var timer = new Stopwatch();
            timer.Start();

            int matches = 0;
            int nomatch = 0;
            int counter = 0;
            int expectedMax = 8413098;
            
            foreach (var value in heads
                .Select(d => DNAUtil.FindPrimer(d.Head1, d.Head2, primers))
                .Where(m => m.valid))
            {
                counter++;
                if (counter % 250000 == 0)
                {
                    Console.WriteLine($"[{(counter*1.0)/expectedMax:P}] {(matches+nomatch)/(timer.ElapsedMilliseconds / 1000.0)}s/sec");
                }
                
                if (value.valid)
                {
                    //Console.WriteLine($"Matched primer {primer.Primer.Primer} on location {primer.Match.Location}.");
                    matches++;
                }
                else
                {
                    //Console.WriteLine($"No match!");
                    nomatch++;
                }
                Console.WriteLine($"Match: {value.forwardMatch.Location}/{value.forwardMatch.Errors} : {value.reverseMatch.Location}/{value.reverseMatch.Errors} : {(value.hasBeenReversed ? 1 : 0)} : {value.forward.Header}");
            }
            
            timer.Stop();
            Console.WriteLine($"Matches: {matches}\nNoMatch: {nomatch}");
            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds/1000.0}s, {(matches+nomatch)/(timer.ElapsedMilliseconds / 1000.0)}s/sec");
        }
    }
}