using DynamicSunWeatherForecast.Database.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

namespace DynamicSunWeatherForecast.Helpers
{
    public class ExcelHelper
    {
        public static List<Weather> ReadXlsx(List<IFormFile> paths, out StringBuilder errors)
        {
            var localErrors = new StringBuilder();
            IEnumerable<Weather> result = new List<Weather>();

            if (paths.Count > 1)
            {
                Parallel.ForEach(paths, new ParallelOptions { MaxDegreeOfParallelism = 8 }, file =>
                {
                    try
                    {
                        var list = ReadXlsx(file);

                        lock (result)
                            result = result.Concat(list);
                    }
                    catch (Exception ex)
                    {
                        lock (localErrors)
                            localErrors.AppendLine(ex.Message);
                    }
                });
            }
            else
            {
                try
                {
                    result = ReadXlsx(paths.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    localErrors.AppendLine(ex.Message);
                }
            }

            errors = localErrors;
            return result.ToList();
        }

        public static List<Weather> ReadXlsx(IFormFile path) 
        {
            var fileName = path.FileName;

            if (path is null || !IsCorrectFilePath(fileName))
                throw new Exception($"Введён некорректный путь к файлу - {fileName}");

            using var stream = new MemoryStream();
            path.CopyTo(stream);
            stream.Position = 0;
            var workbook = new XSSFWorkbook(stream);

            ISheet? sheet = null;
            var dataRow = new List<Weather>();

            for (int i = 0; i < workbook.Count; i++)
            {
                sheet = workbook.GetSheetAt(i);
                var sheetRowCount = sheet.LastRowNum;
                IRow? currentRow = null;

                for (var j = 4; j < sheetRowCount; j++)
                {
                    currentRow = sheet.GetRow(j);

                    dataRow.Add(new Weather
                    {
                        Date = SafeExecute(() => DateTime.SpecifyKind(DateTime.Parse(currentRow.GetCell(0)?.ToString()?.Trim()), DateTimeKind.Utc)),
                        MoscowTime = SafeExecute(() => TimeSpan.Parse(currentRow.GetCell(1)?.ToString()?.Trim())),
                        AirTemperature = SafeExecute(() => double.Parse(currentRow.GetCell(2)?.ToString()?.Trim())),
                        RelativeAirHumidity = SafeExecute(() => double.Parse(currentRow.GetCell(3)?.ToString()?.Trim())),
                        Td = SafeExecute(() => double.Parse(currentRow.GetCell(4)?.ToString()?.Trim())),
                        AtmospherePressure = SafeExecute(() => double.Parse(currentRow.GetCell(5)?.ToString()?.Trim())),
                        WindFollow = SafeExecute(() => currentRow.GetCell(6)?.ToString()),
                        WindSpeed = SafeExecute(() => double.Parse(currentRow.GetCell(7)?.ToString()?.Trim())),
                        Cloudy = SafeExecute(() => double.Parse(currentRow.GetCell(8)?.ToString()?.Trim())),
                        H = SafeExecute(() => double.Parse(currentRow.GetCell(9)?.ToString()?.Trim())),
                        VV = SafeExecute(() => double.Parse(currentRow.GetCell(10)?.ToString()?.Trim())),
                        WeatherConditions = SafeExecute(() => currentRow.GetCell(11)?.ToString()?.Trim())
                    });

                }
            }

            return dataRow;
        }

        private static T? SafeExecute<T>(Func<T> value)
        {
            try
            {
                return value();
            }
            catch (Exception)
            {
                return default;
            }
        }

        private static bool IsCorrectFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if(!path.EndsWith(".xlsx"))
                return false;

            return true;
        }
    }
}
