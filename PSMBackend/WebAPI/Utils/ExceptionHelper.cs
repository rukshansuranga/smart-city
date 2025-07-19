using System;
using System.Text;

namespace PSMWebAPI.Utils;

public class ExceptionHelper
{
    public static string FlattenException(Exception exception)
    {
        var stringBuilder = new StringBuilder();
        while (exception != null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            exception = exception.InnerException;
        }
        return stringBuilder.ToString();
    }
}
