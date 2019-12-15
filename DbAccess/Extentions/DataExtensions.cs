using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Extentions
{
    public static class DataExtensions
    {
        public static DateTime ToUnspecifiedDateTime(this DateTime dt) =>
            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Unspecified);

    }
}
