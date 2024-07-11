using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class PokemonType2Hex
{
    // Start is called before the first frame update
    public static string Convert(PokemonType t)
    {
        switch (t)
        {
            case PokemonType.Normal:
                return "#A8A878";
            case PokemonType.Fighting:
                return "#C03028";
            case PokemonType.Flying:
                return "#A890F0";
            case PokemonType.Poison:
                return "#A040A0";
            case PokemonType.Ground:
                return "#E0C068";
            case PokemonType.Rock:
                return "#B8A038";
            case PokemonType.Bug:
                return "#A8B820";
            case PokemonType.Ghost:
                return "#705898";
            case PokemonType.Steel:
                return "#B8B8D0";
            case PokemonType.Fire:
                return "#F08030";
            case PokemonType.Water:
                return "#6890F0";
            case PokemonType.Grass:
                return "#78C850";
            case PokemonType.Electric:
                return "#F8D030";
            case PokemonType.Psychic:
                return "#F85888";
            case PokemonType.Ice:
                return "#98D8D8";
            case PokemonType.Dragon:
                return "#7038F8";
            case PokemonType.Dark:
                return "#705848";
            case PokemonType.Fairy:
                return "#EE99AC";
            default:
                return "#FFFFFF";
        }
    }
}