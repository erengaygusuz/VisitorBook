﻿namespace VisitorBook.UI.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public Tuple<string, int> GetHighestCountOfVisitedCityByVisitor { get; set; }
        public Tuple<string, int> GetHighestCountOfVisitedStateByVisitor { get; set; }
        public Tuple<string, double> GetLongestDistanceByVisitorOneTime { get; set; }
        public Tuple<string, double> GetLongestDistanceByVisitorAllTime { get; set; }
    }
}