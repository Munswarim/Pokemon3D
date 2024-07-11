using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PokemonParty : MonoBehaviour
{
    public List<PocketMonster> members;
    const int capacity = 6;
    int fit;
    public int allowFaints;
    // Start is called before the first frame update
    void Start()
    {
        // allowFaints = members.Count;
        UpdateFitCount();
        foreach(var member in members)
        { 
            // member.Init();
        }
    }

    public PocketMonster GetRandomMember()
    {
        UpdateFitCount();
        return members.Where(x => x.hP > 0).ElementAt(Random.Range(0, fit));
    }
    public void UpdateFitCount()
    {
        fit = members.Where(x => x.hP > 0).Count();
    }
    public PocketMonster GetFirstFitPokemon() 
    {
        UpdateFitCount();
        return members.Where(x => x.hP > 0).FirstOrDefault();
    }
    public void HealAllMembers()
    {
        foreach(var member in members)
        {
            member.ProperHeal();
        }
    }
    public int GetFaintCount()
    {
        return members.Where(x => x.hP == 0).ToList().Count;
    }
    public bool ReachedFaintLimit()
    {
        return allowFaints <= GetFaintCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
