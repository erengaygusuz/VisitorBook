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
        private readonly IService<VisitedState> _visitedStateService;
        private readonly IService<State> _stateService;

        public VisitorStatisticService(IService<VisitedState> visitedStateService, IService<State> stateService)
        {
            _visitedStateService = visitedStateService;
            _stateService = stateService;
        }

        public Tuple<string, int> GetHighestCountOfVisitedStateByVisitor()
        {
            var visitedStatesWithVisitorAndVisitorAddress = _visitedStateService
                .GetAllAsync(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.State).Include(a => a.Visitor)).GetAwaiter().GetResult();

            var groupedVisitedList = visitedStatesWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var highestCountOfVisitedStateByVisitor = groupedVisitedList.ToDictionary(
                value => value.First().Visitor.Name + " " + value.First().Visitor.Surname,
                value => value.Count()).OrderByDescending(a => a.Value).ThenBy(b => b.Key).First(); 

            var tuple = new Tuple<string, int>(highestCountOfVisitedStateByVisitor.Key, highestCountOfVisitedStateByVisitor.Value);

            return tuple;
        }

        public Tuple<string, int> GetHighestCountOfVisitedCityByVisitor()
        {
            var visitedStatesWithVisitorAndVisitorAddress = _visitedStateService
                .GetAllAsync(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.State).ThenInclude(b => b.City).Include(a => a.Visitor)).GetAwaiter().GetResult();

            var groupedVisitedList = visitedStatesWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var highestCountOfVisitedCityByVisitor = groupedVisitedList.ToDictionary(
                value => value.First().Visitor.Name + " " + value.First().Visitor.Surname,
                value => value.GroupBy(c => c.State.City).Count()).OrderByDescending(a => a.Value).ThenBy(b => b.Key).First();

            var tuple = new Tuple<string, int>(highestCountOfVisitedCityByVisitor.Key, highestCountOfVisitedCityByVisitor.Value);

            return tuple;
        }

        public Tuple<string, double> GetLongestDistanceByVisitorOneTime()
        {
            var visitedStatesWithVisitorAndVisitorAddress = _visitedStateService
                .GetAllAsync(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.State).Include(a => a.Visitor)
                .ThenInclude(v => v.VisitorAddress)).GetAwaiter().GetResult();

            var groupedVisitedList = visitedStatesWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var longestDistanceWithVisitorInfo = groupedVisitedList.ToDictionary(
                value => value.First().Visitor.Name + " " + value.First().Visitor.Surname,
                value => (value.Select(visitedState => CalculateDistance(visitedState)).Max())
                ).OrderByDescending(a => a.Value).First();

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.Key, longestDistanceWithVisitorInfo.Value);

            return tuple;
        }

        public Tuple<string, double> GetLongestDistanceByVisitorAllTime()
        {
            var visitedStatesWithVisitorAndVisitorAddress = _visitedStateService
                .GetAllAsync(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.State).Include(a => a.Visitor)
                .ThenInclude(v => v.VisitorAddress)).GetAwaiter().GetResult();

            var groupedVisitedList = visitedStatesWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var longestDistanceWithVisitorInfo = groupedVisitedList.ToDictionary(
                value => value.First().Visitor.Name + " " + value.First().Visitor.Surname, 
                value => (value.Select(visitedState => CalculateDistance(visitedState)).Sum())
                ).OrderByDescending(a => a.Value).First(); 

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.Key, longestDistanceWithVisitorInfo.Value);

            return tuple;
        }

        private double CalculateDistance(VisitedState visitedState)
        {
            return Math.Round(LocationHelper.GetDistance(
                        new Location()
                        {
                            Latitude = _stateService.GetAsync(s => s.Id == visitedState.Visitor.VisitorAddress.StateId).GetAwaiter().GetResult().Latitude,
                            Longitude = _stateService.GetAsync(s => s.Id == visitedState.Visitor.VisitorAddress.StateId).GetAwaiter().GetResult().Longitude
                        },
                        new Location()
                        {
                            Latitude = visitedState.State.Latitude,
                            Longitude = visitedState.State.Longitude
                        }) / 1000, 2, MidpointRounding.ToEven);
        }
    }
}
