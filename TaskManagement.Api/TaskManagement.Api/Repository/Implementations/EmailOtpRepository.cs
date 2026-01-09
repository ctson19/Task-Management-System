using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;

namespace TaskManagement.Api.Repository.Implementations
{
    public class EmailOtpRepository : IEmailOtpRepository
    {
        private readonly TaskManagementDbContext _context;

        public EmailOtpRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(EmailOtp otp)
        {
            _context.EmailOtps.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailOtp?> GetValidOtpAsync(string email, string otpCode)
        {
            return await _context.EmailOtps
                .Where(o => o.Email == email && o.OtpCode == otpCode && !o.IsUsed && o.ExpiredAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(EmailOtp otp)
        {
            _context.EmailOtps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}