using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrelloObserver;
using TrelloObserver.Models;

namespace CatchChangesREST.Controllers
{
    public class TablesController : Controller
    {
        [Route("tables")]
        [HttpGet]
        public async Task<IReadOnlyList<Table>> GetAllTables() => await RequestBuilder.GetAllTablesAsync();
    }
}