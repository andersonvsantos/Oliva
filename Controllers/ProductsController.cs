using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oliva.Data;
using Oliva.Models;

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