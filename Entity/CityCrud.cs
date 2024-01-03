using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Entity
{
    public class CityCrud
    {
        DataContext _context=new DataContext();
        public List<City> getCities()
        {
            return _context.City.ToList();
        }
        public List<City> getCitiesByMostBlogs()
        {
            var citiesWithBlogCount = _context.BlogPost
             .GroupBy(blogPost => blogPost.CityId)
             .Select(group => new
             {
                 CityId = group.Key,
                 BlogCount = group.Count()
             })
             .OrderByDescending(item => item.BlogCount)
             .Take(5) // İlk 5 şehiri almak için, isteğe bağlı olarak değiştirebilirsiniz
             .ToList();

            var cityIds = citiesWithBlogCount.Select(item => item.CityId).ToList();

            var cities = _context.City
                .Where(city => cityIds.Contains(city.Id))
                .ToList();

            return cities;
        }
        public string GetName(int CityId) {
            return _context.City.Find(CityId).CityName;
        }
    }
}