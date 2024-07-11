using UnityEngine;

public class PokemonMove : ScriptableObject
{
    public int id;
    public string moveName;

    [TextArea] public string description;

    public PokemonType type;
    public PokemonMoveCategory category;
    public int power;
    public int accuracy;
    public int pP;
    public int basePP;
    public int maxPP;
    public int effectChance;
}