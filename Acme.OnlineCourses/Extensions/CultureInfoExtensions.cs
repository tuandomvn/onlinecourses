using System.Globalization;
using Acme.OnlineCourses.Blogs;

namespace Acme.OnlineCourses.Extensions;

public static class CultureInfoExtensions
{
    public static Language ToLanguage(this CultureInfo culture)
    {
        return culture.TwoLetterISOLanguageName.ToLower() switch
        {
            "vi" => Language.vi,
            "en" => Language.en,
            _ => Language.en // Default to English
        };
    }
} 