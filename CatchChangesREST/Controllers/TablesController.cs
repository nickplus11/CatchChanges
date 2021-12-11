using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("{dataSourceName}/table")]
    public class TablesController : Controller
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TablesController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Route("get/{tableId}")]
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

        [Route("get")]
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

        [Route("update/{tableId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTable(string dataSourceName, string tableId,
            [FromBody] UpdateTableParams tableParams)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var targetTable = await dataSource.GetTableAsync(tableId);
                var newTable = targetTable with { Name = tableParams.NewName };
                await dataSource.TryChangeTableAsync(targetTable, newTable);
                // todo check out cases with rights absence
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