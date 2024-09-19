using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProxyDataAPI.Data;
using ProxyDataAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxyDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly ProxyDbContext _context;

        public ProxyController(ProxyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostProxyData([FromBody] List<Proxy> proxies)
        {
            if (proxies == null || proxies.Count == 0)
            {
                return BadRequest("Dados inválidos");
            }

            await _context.Proxies.AddRangeAsync(proxies);
            await _context.SaveChangesAsync();

            return Ok("Dados salvos com sucesso!");
        }

        [HttpGet]
        public async Task<IActionResult> GetProxies()
        {
            var proxies = await _context.Proxies.ToListAsync();

            if (proxies == null || proxies.Count == 0)
            {
                return NotFound("Nenhum dado captado até o momento.");
            }

            return Ok(proxies);
        }
    }
}
