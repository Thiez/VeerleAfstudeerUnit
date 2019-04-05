using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DeadOrAlivePaper.WarrantyVoids.DeadOrAlive
{
    public class BarcodeDatabase
    {
        public Dictionary<string, BarcodeEntry> Entries { get; }

        public class BarcodeEntry
        {
            public string SampleId { get; set; }
            public string BarcodeSequence { get; set; }
            public string LinkerPrimerSequence { get; set; }
            public string ReversePrimer { get; set; }
            public DnaType Type { get; set; }
            public string Adapter { get; set; }
            public string Treatment { get; set; }
            public string Vegetation { get; set; }
            public string Description { get; set; }
        }

        public enum DnaType
        {
            DNA = 0,
            RNA = 1,
            cDNA = 1,
        }

        public static async Task<List<BarcodeEntry>> Load(string filename)
        {
            var lines = File.OpenText(filename);
            if (!(await lines.ReadLineAsync()).StartsWith('#'))
                throw new InvalidDataException($"File {filename} is not in the right format; expected a comment.");
            string line;
            var list = new List<BarcodeEntry>();
            while (!string.IsNullOrWhiteSpace(line = await lines.ReadLineAsync()))
            {
                var elements = line.Split('\t');
                list.Add(new BarcodeEntry
                {
                    SampleId = elements[0],
                    BarcodeSequence = elements[1],
                    LinkerPrimerSequence = elements[2],
                    ReversePrimer = elements[3],
                    Type = Enum.Parse<DnaType>(elements[4], true),
                    Adapter = elements[5],
                    Treatment = elements[6],
                    Vegetation = elements[7],
                    Description = elements[8],
                });
            }

            return list;
        }
    }
}