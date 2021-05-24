using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scrbll.Api.Models;
using Scrbll.Api.Utils;

namespace Scrbll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameContext _context;
        const int TOKEN_LENGTH = 10;

        public GamesController(GameContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(long id)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.Avatar)
                .Include(g => g.Owner)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET: api/Game/5/owner
        [HttpGet("{id}/owner")]
        public async Task<ActionResult<Player>> GetGameOwner(long id)
        {
            var game = await _context.Games
                .Include(g => g.Owner)
                    .ThenInclude(p => p.Avatar)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }
            return game.Owner;
        }

        // GET: api/token
        [HttpGet("/api/{token}")]
        public async Task<ActionResult<Game>> GetGame(string token)
        {
            var game = await _context.Games
                .Include(g => g.Owner)
                    .ThenInclude(p => p.Avatar)
                .Include(g => g.Players)
                    .ThenInclude(p => p.Avatar)
                .AsNoTracking()
                .SingleAsync(game => game.Token == token);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }


        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(long id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NewGame?alias=me
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/NewGame")]
        public async Task<ActionResult<Game>> NewGame(string alias)
        {
            Player owner = new()
            {
                Alias = alias,
                Avatar = new(),
            };

            Game game = new()
            {
                Token = Randoms.RandomString(TOKEN_LENGTH),
                Owner = owner,
            };

            game.Players.Add(owner);
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { token = game.Token }, game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> NewGame(Game game)
        {
            if (string.IsNullOrEmpty(game.Owner.Alias))
                return BadRequest();
            return await NewGame(game.Owner.Alias);
        }


        //POST /api/Games/1/join/me2 
        [HttpPost("{id}/join/{alias}")]
        public async Task<ActionResult<Game>> JoinGame(long id, string alias)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();
            Player player = new()
            {
                Alias = alias,
                Avatar = new()
            };

            game.Players.Add(player);
            await _context.SaveChangesAsync();
            return await GetGame(id);
        }

        //POST /api/Games/1/join 
        [HttpPost("{id}/join")]
        public async Task<ActionResult<Game>> JoinGame(long id, Player player)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            game.Players.Add(player);
            await _context.SaveChangesAsync();
            return await GetGame(id);
        }

        //POST /api/abc123/join/me2 
        [HttpPost("/api/{token}/join/{alias}")]
        public async Task<ActionResult<Game>> JoinGame(string token, string alias)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Token == token);
            if (game == null)
                return NotFound();

            Player player = new()
            {
                Alias = alias,
                Avatar = new()
            };

            game.Players.Add(player);
            await _context.SaveChangesAsync();
            return await GetGame(game.Id);
        }

        //POST /api/axyz123/join
        [HttpPost("/api/{token}/join")]
        public async Task<ActionResult<Game>> JoinGame(string token, Player player)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Token == token);
            if (game == null)
                return NotFound();

            game.Players.Add(player);
            await _context.SaveChangesAsync();
            return await GetGame(game.Id);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(long id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(long id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
