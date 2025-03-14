using Microsoft.EntityFrameworkCore;

namespace SpendWise.Services
{
    public class UserTokenService
    {
        private readonly AppDbContext _context;

        public UserTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUserTokenByToken(string token)
        {
            var userToken = await _context.Tokens
                .FirstOrDefaultAsync(t => t.JwtToken == token);
            if (userToken == null)
            {
                return false;
            }

            _context.Tokens.Remove(userToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
