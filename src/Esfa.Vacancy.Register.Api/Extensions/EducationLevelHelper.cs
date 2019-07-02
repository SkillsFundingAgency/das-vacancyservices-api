using Esfa.Vacancy.Api.Types;
using System;

namespace Esfa.Vacancy.Register.Api.Extensions
{
    public static class EducationLevelHelper
    {
        public static bool TryRemapFromInt(int value, out EducationLevel result)
        {
            switch (value)
            {
                case 5:
                    value = (int)EducationLevel.Higher;
                    break;

                case 7:
                    value = (int)EducationLevel.Degree;
                    break;
            }
            if (Enum.IsDefined(typeof(EducationLevel), value))
            {
                result = (EducationLevel)value;
                return true;
            }
            result = (EducationLevel)0;
            return false;
        }

        public static EducationLevel RemapFromInt(int value)
        {
            if (TryRemapFromInt(value, out EducationLevel result))
                return result;
            throw new ArgumentException($"Cannot convert from int {value} to {nameof(EducationLevel)}");
        }
    }

}