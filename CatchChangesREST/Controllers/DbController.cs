using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;

namespace CatchChangesREST.Controllers;

[Route("db")]
public class DbController
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly SourceContext _sourceContext;

    public DbController(SourceContext sourceContext)
    {
        _sourceContext = sourceContext;
    }

    [Route("tables")]
    [HttpGet]
    public async Task<IActionResult> GetTables()
    {
        try
        {
            var tables = _sourceContext.Tables.ToList();
            _logger.Trace($"Received tables from DB. Count: {tables.Count}");
            return new ContentResult
            {
                Content = string.Join("\n", tables),
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }

        return new ContentResult
        {
            Content = "Error occured while attempting to receive data from DB",
            StatusCode = 500
        };
    }

    [Route("table")]
    [HttpPost]
    public async Task<IActionResult> AddTable(string id, string name)
    {
        try
        {
            var table = new TableModel {Id = id, Name = name};
            await _sourceContext.Tables.AddAsync(table);
            await _sourceContext.SaveChangesAsync();
            _logger.Trace($"Added a new table to DB. Id: {table.Id}, Name: {table.Name}");
            return new ContentResult
            {
                Content = $"Added a new table to DB. Id: {table.Id}. Name: {table.Name}",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }

        return new ContentResult
        {
            Content = "Error occured while attempting to add data to DB",
            StatusCode = 500
        };
    }
}