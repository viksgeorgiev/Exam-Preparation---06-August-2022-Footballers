using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ExportDto;
using Footballers.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportCoachesDto[]? coachesDtos
                = XmlHelper.Deserialize<ImportCoachesDto[]>(xmlString, "Coaches");

            if (coachesDtos != null && coachesDtos.Length > 0)
            {
                foreach (ImportCoachesDto iCoachesDto in coachesDtos)
                {
                    if (!IsValid(iCoachesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(iCoachesDto.Nationality))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Coach coach = new Coach()
                    {
                        Name = iCoachesDto.Name,
                        Nationality = iCoachesDto.Nationality,
                    };

                    foreach (FootballersDto footballersDto in iCoachesDto.Footballers)
                    {
                        if (!IsValid(footballersDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        bool isValidStartDate = DateTime.TryParseExact(footballersDto.ContractStartDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate);
                        bool isValidEndDate = DateTime.TryParseExact(footballersDto.ContractEndDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate);

                        if ((!isValidStartDate) && (!isValidEndDate))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        if (parsedStartDate > parsedEndDate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Footballer footballer = new Footballer()
                        {
                            Name = footballersDto.Name,
                            ContractStartDate = parsedStartDate,
                            ContractEndDate = parsedEndDate,
                            BestSkillType = (BestSkillType)footballersDto.BestSkillType,
                            PositionType = (PositionType)footballersDto.PositionType,
                            Coach = coach,
                        };
                        context.Footballers.Add(footballer);
                    }

                    context.Coaches.Add(coach);
                    sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
                }
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamsDto[]? importTeamsDtos = JsonConvert.DeserializeObject<ImportTeamsDto[]?>(jsonString);

            if (importTeamsDtos != null && importTeamsDtos.Length > 0)
            {
                foreach (ImportTeamsDto teamsDto in importTeamsDtos)
                {
                    if (!IsValid(teamsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (teamsDto.Trophies <= 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Team team = new Team()
                    {
                        Name = teamsDto.Name,
                        Nationality = teamsDto.Nationality,
                        Trophies = teamsDto.Trophies,
                    };

                    int[] validFootballersIds = context.Footballers.Select(f => f.Id).ToArray();

                    foreach (int footballer in teamsDto.Footballers.Distinct())
                    {
                        if (!validFootballersIds.Contains(footballer))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        TeamFootballer teamFootballer = new TeamFootballer()
                        {
                            Team = team,
                            FootballerId = footballer,
                        };

                        context.TeamsFootballers.Add(teamFootballer);
                    }

                    context.Teams.Add(team);
                    sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
                }

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
