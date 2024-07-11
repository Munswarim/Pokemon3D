using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyIndicator : MonoBehaviour
{
    public PokemonParty team;
    public List<RawImage> indicators;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Set()
    {
        for (int i = 0; i < team.members.Count; i++)
        {
            float frac = team.members[i].NormalizedHP();

            if (frac == 0)
                indicators[i].color = Hex2Color.Convert("#707070");
            else if (frac <= 0.15)
                indicators[i].color = Hex2Color.Convert("#FF2A00");
            else if (frac <= 0.30)
                indicators[i].color = Hex2Color.Convert("#FFA526");
            else if (frac <= 0.55)
                indicators[i].color = Hex2Color.Convert("#FDF798");
            else if (frac > 0.55)
                indicators[i].color = Hex2Color.Convert("#DCFFE2");
        }
        for (int i = team.members.Count; i < 6; i++)
        {
            indicators[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
