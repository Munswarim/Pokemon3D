using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class String2PokemonType
{
    public static PokemonType Convert(string t)
    {
        switch (t.ToLower())
        {
            case "normal":
                return PokemonType.Normal;
            case "fighting":
                return PokemonType.Fighting;
            case "flying":
                return PokemonType.Flying;
            case "poison":
                return PokemonType.Poison;
            case "ground":
                return PokemonType.Ground;
            case "rock":
                return PokemonType.Rock;
            case "bug":
                return PokemonType.Bug;
            case "ghost":
                return PokemonType.Ghost;
            case "steel":
                return PokemonType.Steel;
            case "fire":
                return PokemonType.Fire;
            case "water":
                return PokemonType.Water;
            case "grass":
                return PokemonType.Grass;
            case "electric":
                return PokemonType.Electric;
            case "psychic":
                return PokemonType.Psychic;
            case "ice":
                return PokemonType.Ice;
            case "dragon":
                return PokemonType.Dragon;
            case "dark":
                return PokemonType.Dark;
            case "fairy":
                return PokemonType.Fairy;
        }
        return PokemonType.None;
    }

    public static string Convert(PokemonType t)
    {
        switch (t)
        {
            case PokemonType.None:
                return "None";
            case PokemonType.Normal:
                return "Normal";
            case PokemonType.Fighting:
                return "Fighting";
            case PokemonType.Flying:
                return "Flying";
            case PokemonType.Poison:
                return "Poison";
            case PokemonType.Ground:
                return "Ground";
            case PokemonType.Rock:
                return "Rock";
            case PokemonType.Bug:
                return "Bug";
            case PokemonType.Ghost:
                return "Ghost";
            case PokemonType.Steel:
                return "Steel";
            case PokemonType.Fire:
                return "Fire";
            case PokemonType.Water:
                return "Water";
            case PokemonType.Grass:
                return "Grass";
            case PokemonType.Electric:
                return "Electric";
            case PokemonType.Psychic:
                return "Psychic";
            case PokemonType.Ice:
                return "Ice";
            case PokemonType.Dragon:
                return "Dragon";
            case PokemonType.Dark:
                return "Dark";
            case PokemonType.Fairy:
                return "Fairy";
            default:
                return "Undefined";
        }
    }
}