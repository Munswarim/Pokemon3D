using UnityEngine;

public class PokemonStats : ScriptableObject
{
    
    public int maxHP;
    public int attack;
    public int defence;
    public int spAttack;
    public int spDefence;
    public int speed;

    public PokemonStats(int maxHP = 0, int attack = 0, int defence = 0, int spAttack = 0, int spDefence = 0, int speed = 0)
    {
        this.maxHP= maxHP;
        this.attack= attack;
        this.defence= defence;
        this.spAttack= spAttack;
        this.spDefence= spDefence;
        this.speed= speed;
    }
}
