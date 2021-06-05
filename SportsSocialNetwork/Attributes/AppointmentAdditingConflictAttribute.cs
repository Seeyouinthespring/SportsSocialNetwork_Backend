using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Attributes
{
    public class AppointmentAdditingConflictAttribute : SwaggerResponse409Attribute
    {
        public const string INCORRECT_SPORT = "incorrectSport";
        public const string INCORRECT_PLAYGROUND = "incorrectPlayground";

        public static readonly Dictionary<string, string> Errors = new Dictionary<string, string>
        {
            {INCORRECT_SPORT, "Выбраный спорт не подход для данной площадки"},
            {INCORRECT_PLAYGROUND, "Данное действие нельзя совершить для коммерческой площадки" }
        };

        public AppointmentAdditingConflictAttribute() : base(Errors)
        {

        }
    }
}
