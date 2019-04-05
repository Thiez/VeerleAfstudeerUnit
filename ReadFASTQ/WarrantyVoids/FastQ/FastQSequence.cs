namespace ReadFASTQ.WarrantyVoids.FastQ
{
    public class FastQSequence
    {
        /// <summary>
        /// Gets or sets a header containing information about this sequence.
        /// </summary>
        public string Header { get; set; }
        
        /// <summary>
        /// Gets or sets the actual sequence
        /// </summary>
        public string Sequence { get; set; }
        
        /// <summary>
        /// Gets or sets optional line
        /// </summary>
        public string Optional { get; set; }
        
        /// <summary>
        /// Gets or sets the quality of the sequence.
        /// </summary>
        public string Quality { get; set; }
    }
}