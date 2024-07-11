public class StatusCondition2String
{
    public static string Convert(StatusCondition x)
    {
        switch (x)
        {
            case StatusCondition.Burn:
                return "BRN";
            case StatusCondition.Freeze:
                return "FRZ";
            case StatusCondition.Paralysis:
                return "PAR";
            case StatusCondition.Poison:
                return "PSN";
            case StatusCondition.Sleep:
                return "SLP";
            case StatusCondition.Confused:
                return "CNF";
            case StatusCondition.None:
                return "";
            default:
                return "???";
        }
    }
}