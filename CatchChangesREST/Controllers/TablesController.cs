using System.Collections.Generic;
using System.Threading.Tasks;
using CatchChangesREST.DataSources;
using Microsoft.AspNetCore.Mvc;
using DataModels;
using DataModels.Models;

namespace CatchChangesREST.Controllers
{
    public class TablesController : Controller
    {
        [Route("tables")]
        [HttpGet]
        public async Task<IReadOnlyList<Table>> GetAllTables() => await RequestBuilder.GetAllTablesAsync();
    }
}