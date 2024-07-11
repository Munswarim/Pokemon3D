[System.Serializable]
public class PokemonAPIBattleData
{
    public int id;
    public Abilities[] abilities;
    public int base_experience;
    public Moves[] moves;
    public Stats[] stats;
    public Types[] types;
    public Sprites sprites;
}
[System.Serializable]
public class Abilities
{
    public Ability ability;
}
[System.Serializable]
public class Ability
{
    public string name;
    public int slot;
}
[System.Serializable]
public class Moves
{
    public Move move;
    public Version_group_details[] version_group_details;
}
[System.Serializable]
public class Move
{
    public string name;
}
[System.Serializable]
public class Version_group_details
{
    public int level_learned_at;
}
[System.Serializable]
public class Stats
{
    public int base_stat;
}
[System.Serializable]
public class Types
{
    public Type type;
}
[System.Serializable]
public class Type
{
    public string name;
}
[System.Serializable]
public class Sprites
{
    public Other other;
    public string front_default;
    public string back_default;

}
[System.Serializable]
public class Other
{
    public Official_artwork official_artwork;

}
[System.Serializable]
public class Official_artwork
{
    public string front_default;
}