using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonAPIMoveData
{
    public int accuracy;
    public Damage_class damage_class;
    public int effect_chance;
    public Flavor_text_entries[] flavor_text_entries;
    public int id;
    public int power;
    public int pp;
    public Type type;

}
[System.Serializable]
public class Damage_class
{
    public string name;
}
