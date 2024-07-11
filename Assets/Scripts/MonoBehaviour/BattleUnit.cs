using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [Range(-1,1)]
    public int playerCoef;
    public PocketMonster pocketMonster;
    public void Setup(PocketMonster pocketMonster)
    {
        this.pocketMonster = pocketMonster;

        if(playerCoef == 1)
        {
            pocketMonster.transform.localPosition  = gameObject.transform.localPosition = new Vector3(-1, 0, -4);

            pocketMonster.transform.localRotation = Quaternion.Euler(0, 45, 0);
        }
        else if(playerCoef == -1)
        {
            pocketMonster.transform.localPosition = gameObject.transform.localPosition = new Vector3(4, 0, 1);
            pocketMonster.transform.localRotation = Quaternion.Euler(0, 225, 0);
        }
    }
    public void Retreat()
    {
        Debug.Log(pocketMonster.pokemonName + " retreated.");
        if (playerCoef == 1)
        {
            pocketMonster.transform.localPosition = gameObject.transform.localPosition = new Vector3(-111, -111, -111);
            pocketMonster.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (playerCoef == -1)
        {
            pocketMonster.transform.localPosition = gameObject.transform.localPosition = new Vector3(111, 111, 111);
            pocketMonster.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Faint()
    {
        Debug.Log(pocketMonster.pokemonName + " fainted.");
        if (playerCoef == 1)
        {
            pocketMonster.transform.localPosition = gameObject.transform.localPosition = new Vector3(-100, -100, -100);
            // gameObject.transform.localRotation = Quaternion.Euler(0, 45, 0);
        }
        else if (playerCoef == -1)
        {
            pocketMonster.transform.localPosition = gameObject.transform.localPosition = new Vector3(100, 100, 100);
            // gameObject.transform.localRotation = Quaternion.Euler(0, 225, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
