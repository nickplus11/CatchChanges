using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("{dataSourceName}/table")]
    public class TablesController : Controller
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TablesController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Microsoft.AspNetCore.Mvc.Route("get/{tableId}")]
        [HttpGet]
        public async Task<ActionResult<Table>> GetTable(string dataSourceName, string tableId)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var table = await dataSource.GetTableAsync(tableId);
                return new ActionResult<Table>(table);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new EmptyResult();
        }

        [Microsoft.AspNetCore.Mvc.Route("get")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Table>>> GetAllTables(string dataSourceName)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var tables = await dataSource.GetAllTablesAsync();
                return new ActionResult<IReadOnlyList<Table>>(tables);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new ActionResult<IReadOnlyList<Table>>(ImmutableList<Table>.Empty);
        }

        [Microsoft.AspNetCore.Mvc.Route("update/{tableId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTable(string dataSourceName, string tableId, [FromBody]JsonElement jsonElement)
        {
            try
            {
                var updateTableParams = HttpHelper.GetModel<UpdateTableParams>(jsonElement.ToString());
                if (updateTableParams is null) throw new Exception("Update table request content has not been read correctly");
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var targetTable = await dataSource.GetTableAsync(tableId);
                var newTable = targetTable with { Name = updateTableParams.NewName };
                await dataSource.TryChangeTableAsync(targetTable, newTable);
                _logger.Trace($"Table name has been changed. From: {targetTable.Name} To: {newTable.Name}");
                return new ContentResult
                {
                    Content = "Table has been changed",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new EmptyResult();
        }
    }
}