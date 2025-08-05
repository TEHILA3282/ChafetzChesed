using System.Net.Http;
using ClosedXML.Excel;
using System.Net.Http.Headers;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.BLL.Interfaces;
using ClosedXML.Excel;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

public class AccountActionsService : IAccountActionsService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountActionsService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task FetchAndParseExcelFromExternalAsync(int institutionId)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://external-system.com/api/excel/file");
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); 

            foreach (var row in rows)
            {
                var action = new AccountAction
                {
                    InstitutionId = institutionId,
                    Zeout = long.Parse(row.Cell(2).GetString()),
                    Seder = int.Parse(row.Cell(3).GetString()),
                    Perut = row.Cell(4).GetString(),
                    Important = int.Parse(row.Cell(5).GetString())
                };

                _context.AccountActions.Add(action);
            }

            await _context.SaveChangesAsync();
        }
    }