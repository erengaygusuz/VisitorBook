﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepository<VisitedCounty> _visitedCountyRepository;
        private readonly LocationHelper _locationHelper;

        public VisitorStatisticService(IRepository<VisitedCounty> visitedCountyRepository, LocationHelper locationHelper)
        {
            _locationHelper = locationHelper;
            _visitedCountyRepository = visitedCountyRepository;
        }

        public async Task<Tuple<string, int>> GetHighestCountOfVisitedCountyByVisitorAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.County).Include(a => a.Visitor));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var highestCountOfVisitedCountyByVisitor = await groupedVisitedList.Select(a => new
            {
                VisitorInfo = a.First().Visitor.Name + " " + a.First().Visitor.Surname,
                CountOfDistinctVisitedCounty = a.Distinct().Count()
            }).OrderByDescending(a => a.CountOfDistinctVisitedCounty).ThenBy(b => b.VisitorInfo).FirstAsync();

            var tuple = new Tuple<string, int>(highestCountOfVisitedCountyByVisitor.VisitorInfo, highestCountOfVisitedCountyByVisitor.CountOfDistinctVisitedCounty);

            return tuple;
        }

        public async Task<Tuple<string, int>> GetHighestCountOfVisitedCityByVisitorAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.County).Include(a => a.Visitor));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var highestCountOfVisitedCityByVisitor = await groupedVisitedList.Select(a => new
            {
                VisitorInfo = a.First().Visitor.Name + " " + a.First().Visitor.Surname,
                CountOfDistinctVisitedCounty = a.GroupBy(c => c.County.City).Count()
            }).OrderByDescending(a => a.CountOfDistinctVisitedCounty).ThenBy(b => b.VisitorInfo).FirstAsync();

            var tuple = new Tuple<string, int>(highestCountOfVisitedCityByVisitor.VisitorInfo, highestCountOfVisitedCityByVisitor.CountOfDistinctVisitedCounty);

            return tuple;
        }

        public async Task<Tuple<string, double>> GetLongestDistanceByVisitorOneTimeAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.County).Include(a => a.Visitor).ThenInclude(a => a.VisitorAddress).ThenInclude(i => i.County));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var longestDistanceWithVisitorInfo = (await groupedVisitedList.ToListAsync()).Select(a => new
            {
                VisitorInfo = a.First().Visitor.Name + " " + a.First().Visitor.Surname,
                LongestDistance = a.Max(visitedCounty => CalculateDistance(visitedCounty))
            }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            return tuple;
        }

        public async Task<Tuple<string, double>> GetLongestDistanceByVisitorAllTimeAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.Visitor.VisitorAddress != null, include: u => u.Include(a => a.County).Include(a => a.Visitor).ThenInclude(a => a.VisitorAddress).ThenInclude(i => i.County));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.VisitorId);

            var longestDistanceWithVisitorInfo = (await groupedVisitedList.ToListAsync()).Select(a => new
            {
                VisitorInfo = a.First().Visitor.Name + " " + a.First().Visitor.Surname,
                LongestDistance = a.Sum(visitedCounty => CalculateDistance(visitedCounty))
            }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

            var tuple = new Tuple<string, double>(longestDistanceWithVisitorInfo.VisitorInfo, longestDistanceWithVisitorInfo.LongestDistance);

            return tuple;
        }

        private double CalculateDistance(VisitedCounty visitedCounty)
        {
            return Math.Round(_locationHelper.GetDistance(
                   new Location()
                   {
                       Latitude = visitedCounty.Visitor.VisitorAddress.County.Latitude,
                       Longitude = visitedCounty.Visitor.VisitorAddress.County.Longitude
                   },
                   new Location()
                   {
                       Latitude = visitedCounty.County.Latitude,
                       Longitude = visitedCounty.County.Longitude
                   }) / 1000, 2, MidpointRounding.ToEven);
        }
    }
}
