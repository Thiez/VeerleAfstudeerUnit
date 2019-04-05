using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeadOrAlivePaper.WarrantyVoids.DeadOrAlive;
using ReadFASTQ.WarrantyVoids.FastQ;

namespace DNAProc
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var fileReader = new FastQStreamReader(File.OpenText("../Data/testfileR1.fastq"));
            var barcodes = await BarcodeDatabase.Load("../Data/forward_T_split_map.txt");
            var barcodeDict = barcodes.ToDictionary(b => b.BarcodeSequence, b => b);

            foreach (var value in fileReader.Values())
            {
                Console.WriteLine(value.Header);
            }
        }
    }
}