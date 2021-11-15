using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HomeApi.Models;

namespace HomeApi.Services
{

    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly TodoContext _context;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, TodoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //set a variable for today
                DateTime date = DateTime.Now;
                executionCount++;
                //Check if today id the first in the month
                if (date.Day == 1)
                {
                    var today = DateTime.Today.ToString("d");
                    //make sure that only on deposit will be made on this day
                    if (!_context.Deposits.ToList().Any(deposit => deposit.Name == today))
                    {
                        //This is a basic deposit with the deposit date and amount. Date is in dd-mm-yyyy format
                        Deposit deposit = new Deposit();
                        deposit.Name = $"{today}";
                        deposit.Amount = 750;

                        _context.Deposits.Add(deposit);
                        await _context.SaveChangesAsync();
                    }
                }
                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(3600000, stoppingToken);
            }
        }
    }
}