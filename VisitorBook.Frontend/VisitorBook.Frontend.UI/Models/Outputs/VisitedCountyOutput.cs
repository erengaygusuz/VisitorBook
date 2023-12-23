﻿namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class VisitedCountyOutput
    {
        public int Id { get; set; }

        public VisitorOutput Visitor { get; set; }

        public CountyOutput County { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
