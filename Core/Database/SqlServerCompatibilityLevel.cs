namespace Core.Database;

public static class SqlServerCompatibilityLevel
{
    /*
        100: SQL Server 2008
        110: SQL Server 2012
        120: SQL Server 2014
        130: SQL Server 2016
        140: SQL Server 2017
        150: SQL Server 2019
        160: SQL Server 2022
     */

    public const int SqlServer2008 = 100;
    public const int SqlServer2012 = 110;
    public const int SqlServer2014 = 120;
    public const int SqlServer2016 = 130;
    public const int SqlServer2017 = 140;
    public const int SqlServer2019 = 150;
    public const int SqlServer2022 = 160;

    public static int GetByVersion(string version)
    {
        if (version == "2008")
        {
            return SqlServer2008;
        }

        if (version == "2012")
        {
            return SqlServer2012;
        }

        if (version == "2014")
        {
            return SqlServer2014;
        }

        if (version == "2016")
        {
            return SqlServer2016;
        }

        if (version == "2017")
        {
            return SqlServer2017;
        }

        if (version == "2019")
        {
            return SqlServer2019;
        }

        if (version == "2022")
        {
            return SqlServer2022;
        }

        return SqlServer2022;
    }
}