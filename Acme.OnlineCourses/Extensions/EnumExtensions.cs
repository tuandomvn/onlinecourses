using System.ComponentModel;
using System.Reflection;

namespace Acme.OnlineCourses.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var description = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DescriptionAttribute>()?
            .Description;

        return description ?? enumValue.ToString();
    }
}