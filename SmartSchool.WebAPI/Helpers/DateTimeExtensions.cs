using System;

namespace SmartSchool.WebAPI.Helpers
{
    public static class DateTimeExtensions
    {
        public static int PegarIdadeAtual(this DateTime datetime)
        {
            var dataAtual = DateTime.UtcNow;
            int age = dataAtual.Year - datetime.Year;


            if(dataAtual < datetime.AddYears(age))
            age--;

            return age;
        }
    }
}