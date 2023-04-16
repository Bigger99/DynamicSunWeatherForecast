using DynamicSunWeatherForecast.Database.Models;
using DynamicSunWeatherForecast.Database.Repositiry;
using DynamicSunWeatherForecast.Helpers;
using DynamicSunWeatherForecast.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DynamicSunWeatherForecast.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseRepository _dbContext;


        public HomeController(ILogger<HomeController> logger, IDatabaseRepository databaseRepository)
        {
            _logger = logger;
            _dbContext = databaseRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadАrchive()
        {
            return View();
        }

        public IActionResult WatchArchive(Mounth mounth, string year)
        {
            IEnumerable<Weather> weather;

            if (mounth == Mounth.None && string.IsNullOrEmpty(year))
            {
                return View(_dbContext.GetAllWeather());
            }
            else
            {
                var numYear = 0;

                if (!string.IsNullOrEmpty(year))
                {
                    var isNum = int.TryParse(year, out numYear);

                    if (!isNum || numYear < 0)
                            return BadRequest("Введен неверный формат года");
                }
                
                weather = _dbContext.GetWeather(mounth, numYear);
            }

            return View(weather);
        }

        [HttpPost]
        public async Task<IActionResult> LoadData(List<IFormFile> excelFiles)
        {
            IEnumerable<Weather> result = new List<Weather>();

            if (excelFiles?.Any() ?? false)
            {
                result = ExcelHelper.ReadXlsx(excelFiles, out var errors);
                await _dbContext.SaveWeatherAsync(result);

                if (errors?.Length > 0)
                    return BadRequest(errors.ToString());
            }
            
            return View(nameof(WatchArchive), result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}