public class String2PokemonMoveCategory
{
    public static PokemonMoveCategory Convert(string c)
    {
        switch (c)
        {
            case "physical":
                return PokemonMoveCategory.Physical;
            case "special":
                return PokemonMoveCategory.Special;
            case "status":
                return PokemonMoveCategory.Status;
            default:
                return PokemonMoveCategory.None;
        }
    }
    public static string Convert(PokemonMoveCategory c)
    {
        switch (c)
        {
            case PokemonMoveCategory.Physical:
                return "physical";
            case PokemonMoveCategory.Special:
                return "special";
            case PokemonMoveCategory.Status:
                return "status";
            default:
                return "none";
        }
    }
}