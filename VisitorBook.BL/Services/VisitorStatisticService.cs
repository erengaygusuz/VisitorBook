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
        private readonly IService<State> _stateService;
        private readonly IService<Visitor> _visitorService;

        public VisitorStatisticService(IService<State> stateService, IService<Visitor> visitorService)
        {
            _stateService = stateService;
            _visitorService = visitorService;
        }

        public Tuple<string, int> GetHighestCountOfVisitedStateByVisitor()
        {
            var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
                include: v => v.Include(a => a.States)).GetAwaiter().GetResult();

            var visitor = visitors.Select(a => 
                new 
                { 
                    VisitorInfo = a.Name + " " + a.Surname, 
                    CountOfDistinctVisitedState = a.States.Distinct().Count() 
                }).OrderByDescending(a => a.CountOfDistinctVisitedState).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, int>(visitor.VisitorInfo, visitor.CountOfDistinctVisitedState);

            return tuple;
        }

        public Tuple<string, int> GetHighestCountOfVisitedCityByVisitor()
        {
            var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
                include: v => v.Include(a => a.States).ThenInclude(s => s.City)).GetAwaiter().GetResult();

            var visitor = visitors.Select(a => 
                new 
                { 
                    VisitorInfo = a.Name + " " + a.Surname, 
                    CountOfDistinctVisitedCity = a.States.Select(s => s.City).Distinct().Count() 
                }).OrderByDescending(a => a.CountOfDistinctVisitedCity).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, int>(visitor.VisitorInfo, visitor.CountOfDistinctVisitedCity);

            return tuple;
        }

        public Tuple<string, double> GetLongestDistanceByVisitorOneTime()
        {
            var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null, 
                include: v => v.Include(a => a.States).ThenInclude(s => s.City).Include(b => b.VisitorAddress)).GetAwaiter().GetResult();

            var longestDistanceWithVisitorInfo = visitors.Select(v =>
                new
                {
                    VisitorInfo = v.Name + " " + v.Surname,
                    LongestDistance = v.States.Select(s => CalculateDistance(v, s)).Max()
                }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            return tuple;
        }

        public Tuple<string, double> GetLongestDistanceByVisitorAllTime()
        {
            var visitors = _visitorService.GetAllAsync(v => v.VisitorAddress != null,
                include: v => v.Include(a => a.States).ThenInclude(s => s.City).Include(b => b.VisitorAddress)).GetAwaiter().GetResult();

            var longestDistanceWithVisitorInfo = visitors.Select(v => 
                new 
                { 
                    VisitorInfo = v.Name + " " + v.Surname, 
                    LongestDistance = v.States.Select(s => CalculateDistance(v, s)).Sum()
                }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            return tuple;
        }

        private double CalculateDistance(Visitor visitor, State visitedState)
        {
            return Math.Round(LocationHelper.GetDistance(
                   new Location()
                   {
                       Latitude = _stateService.GetAsync(s => s.Id == visitor.VisitorAddress.StateId).GetAwaiter().GetResult().Latitude,
                       Longitude = _stateService.GetAsync(s => s.Id == visitor.VisitorAddress.StateId).GetAwaiter().GetResult().Longitude
                   },
                   new Location()
                   {
                       Latitude = visitedState.Latitude,
                       Longitude = visitedState.Longitude
                   }) / 1000, 2, MidpointRounding.ToEven);
        }
    }
}
