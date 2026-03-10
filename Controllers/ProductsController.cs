using Microsoft.AspNetCore.Mvc;
using Oliva.Data;

namespace Oliva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ProductsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}