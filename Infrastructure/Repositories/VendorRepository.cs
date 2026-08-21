using Microsoft.EntityFrameworkCore;
using Gerald.Core.Entities;
using Gerald.Infrastructure.Data;

namespace Gerald.Infrastructure.Repositories
{
    public class VendorRepository
    {
        private readonly GeraldDbContext _context;

        public VendorRepository(GeraldDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vendor>> GetAllAsync()
        {
            return await _context.Vendors.Include(v => v.Findings).ToListAsync();
        }

        public async Task<Vendor> GetByIdAsync(int id)
        {
            return await _context.Vendors.Include(v => v.Findings).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();
        }
    }
}