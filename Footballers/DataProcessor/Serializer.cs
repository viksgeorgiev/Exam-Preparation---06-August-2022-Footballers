using System.Globalization;
using Footballers.DataProcessor.ExportDto;
using Footballers.Utilities;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using System.Linq;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context
                .Coaches
                .Where(c => c.Footballers.Any())
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers
                        .Select(f => new ExportFootballersDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            string result = XmlHelper.Serialize(coaches, "Coaches");

            return result;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context
                .Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(tf => tf.Footballer.ContractStartDate >= date)
                        .ToArray()
                        .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                        .ThenBy(tf => tf.Footballer.Name)
                        .Select(f => new
                        {
                            FootballerName = f.Footballer.Name,
                            ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = f.Footballer.BestSkillType.ToString(),
                            PositionType = f.Footballer.PositionType.ToString(),
                        })
                        //.OrderByDescending(f => f.ContractEndDate)
                        //.ThenBy(f => f.FootballerName)
                        .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert.SerializeObject(teams, Formatting.Indented);

            return result;
        }
    }
}
