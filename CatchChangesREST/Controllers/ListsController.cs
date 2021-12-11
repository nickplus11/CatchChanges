using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("{dataSourceName}/list")]
    public class ListsController
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ListsController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Route("get/{listId}")]
        [HttpGet]
        public async Task<ActionResult<ListOfCards>> GetList(string dataSourceName, string listId)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var list = await dataSource.GetListAsync(listId);
                return new ActionResult<ListOfCards>(list);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new EmptyResult();
        }

        [Route("get/all/{tableId}")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ListOfCards>>> GetLists(string dataSourceName, string tableId)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var list = await dataSource.GetAllListsAsync(tableId);
                return new (list);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new EmptyResult();
        }
    }
}