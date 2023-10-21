using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;

namespace VisitorBook.BL.Services
{
    public class VisitorStatisticService
    {
        private readonly IService<County> _countyService;
        private readonly IService<Visitor> _visitorService;

        public VisitorStatisticService(IService<County> countyService, IService<Visitor> visitorService)
        {
            _countyService = countyService;
            _visitorService = visitorService;
        }

        public Tuple<string, int> GetHighestCountOfVisitedCountyByVisitor()
        {
            //var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
            //    include: v => v.Include(a => a.Counties)).GetAwaiter().GetResult();

            //var visitor = visitors.Select(a => 
            //    new 
            //    { 
            //        VisitorInfo = a.Name + " " + a.Surname, 
            //        CountOfDistinctVisitedCounty = a.Counties.Distinct().Count() 
            //    }).OrderByDescending(a => a.CountOfDistinctVisitedCounty).ThenBy(b => b.VisitorInfo).First();

            //var tuple = new Tuple<string, int>(visitor.VisitorInfo, visitor.CountOfDistinctVisitedCounty);

            //return tuple;

            return new Tuple<string, int>("", 0);
        }

        public Tuple<string, int> GetHighestCountOfVisitedCityByVisitor()
        {
            //var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
            //    include: v => v.Include(a => a.Counties).ThenInclude(s => s.City)).GetAwaiter().GetResult();

            //var visitor = visitors.Select(a => 
            //    new 
            //    { 
            //        VisitorInfo = a.Name + " " + a.Surname, 
            //        CountOfDistinctVisitedCity = a.Counties.Select(s => s.City).Distinct().Count() 
            //    }).OrderByDescending(a => a.CountOfDistinctVisitedCity).ThenBy(b => b.VisitorInfo).First();

            //var tuple = new Tuple<string, int>(visitor.VisitorInfo, visitor.CountOfDistinctVisitedCity);

            //return tuple;

            return new Tuple<string, int>("", 0);
        }

        public Tuple<string, double> GetLongestDistanceByVisitorOneTime()
        {
            //var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
            //    include: v => v.Include(a => a.Counties).ThenInclude(s => s.City).Include(b => b.VisitorAddress)).GetAwaiter().GetResult();

            //var longestDistanceWithVisitorInfo = visitors.Select(v =>
            //    new
            //    {
            //        VisitorInfo = v.Name + " " + v.Surname,
            //        LongestDistance = v.Counties.Select(s => CalculateDistance(v, s)).Max()
            //    }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            //var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            //return tuple;

            return new Tuple<string, double>("", 0);
        }

        public Tuple<string, double> GetLongestDistanceByVisitorAllTime()
        {
            //var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null,
            //    include: v => v.Include(a => a.Counties).ThenInclude(s => s.City).Include(b => b.VisitorAddress)).GetAwaiter().GetResult();

            //var longestDistanceWithVisitorInfo = visitors.Select(v => 
            //    new 
            //    { 
            //        VisitorInfo = v.Name + " " + v.Surname, 
            //        LongestDistance = v.Counties.Select(s => CalculateDistance(v, s)).Sum()
            //    }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            //var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            //return tuple;

            return new Tuple<string, double>("", 0);
        }

        private double CalculateDistance(Visitor visitor, County visitedCounty)
        {
            //return Math.Round(LocationHelper.GetDistance(
            //       new Location()
            //       {
            //           Latitude = _countyService.GetAsync(s => s.Id == visitor.VisitorAddress.CountyId).GetAwaiter().GetResult().Latitude,
            //           Longitude = _countyService.GetAsync(s => s.Id == visitor.VisitorAddress.CountyId).GetAwaiter().GetResult().Longitude
            //       },
            //       new Location()
            //       {
            //           Latitude = visitedCounty.Latitude,
            //           Longitude = visitedCounty.Longitude
            //       }) / 1000, 2, MidpointRounding.ToEven);

            return 0;
        }
    }
}
