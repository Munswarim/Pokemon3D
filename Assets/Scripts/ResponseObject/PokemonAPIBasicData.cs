[System.Serializable]
public class PokemonAPIBasicData
{
    public int id;
    public string name;
    public int base_happiness;
    public int capture_rate;
    public Evolves_from_species evolves_from_species;
    public Flavor_text_entries[] flavor_text_entries;
    public Growth_rate growth_rate;
    public Varieties[] varieties;
    public int gender_rate;
}
[System.Serializable]
public class Evolves_from_species
{
    public string name;
}
[System.Serializable]
public class Flavor_text_entries
{
    public string flavor_text;
    public Language language;
}
[System.Serializable]
public class Language
{
    public string name;
}
[System.Serializable]
public class Growth_rate
{
    public string name;
}
[System.Serializable]
public class Varieties
{
    public bool is_default;
    public Pokemon pokemon;
}
[System.Serializable]
public class Pokemon
{
    public string name;
}