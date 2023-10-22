using ShoppingCart.Models;
using ShoppingCart.Data;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Data.Repos
{
    public class CountryRepo : IRepository<Country, int>
    {
        private readonly EcommerceContext _context;
        public CountryRepo(EcommerceContext context)
        {
            _context = context;
        }

        public Country Create(Country entity)
        {
            _context.Countries.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Delete(Country entity)
        {
            _context.Countries.Remove(entity);
            _context.SaveChanges();
        }

        public Country Get(int id)
        {
            Country country = _context.Countries.Find(id);
            return country;
        }


        public ICollection<Country> GetAll()
        {
            return _context.Countries.ToHashSet();
        }

        public Country Update(Country entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
