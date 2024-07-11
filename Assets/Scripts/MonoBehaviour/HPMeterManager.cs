using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HPMeterManager : MonoBehaviour
{
    /***
     * 
     * 
     This is a demo class to visualize how hp meter will work in future
     *
     *
     ***/

    // DCFFE2
    public RawImage hPOutlineBgImageElement;
    public RawImage hPIndicatorImageElement;
    public RawImage hPOutlineImageElement;
    public TMP_Text hPTextElement;
    public TMP_Text statusTextElement;
    public CurvedText nameLvTextElement;
    public PartyIndicator partyIndicator;

    [Range(-1,1)]
    public int playerCoef;

    public int maxHP;
    public int hP;

    public StatusCondition statusCondition;

    // Start is called before the first frame update
    private void Start()
    {
        hP = maxHP;

        nameLvTextElement.playerCoef = playerCoef;

        RectTransform temp1 = hPIndicatorImageElement.GetComponent<RectTransform>();
        temp1.localScale = new Vector3(temp1.localScale.x, playerCoef, temp1.localScale.z);

        RectTransform temp2 = hPOutlineBgImageElement.GetComponent<RectTransform>();
        temp2.localScale = new Vector3(temp2.localScale.x, playerCoef, temp2.localScale.z);

        RectTransform temp3 = nameLvTextElement.GetComponent<RectTransform>();
        temp3.localPosition = new Vector3(temp3.localPosition.x, playerCoef * 175, temp3.localPosition.z);

        float rot = gameObject.GetComponent<RectTransform>().rotation.eulerAngles.z;

        RectTransform temp4 = hPTextElement.GetComponent<RectTransform>();
        temp4.localPosition = new Vector3(temp4.localPosition.x, playerCoef * 30, temp4.localPosition.z);

        RectTransform temp5 = statusTextElement.GetComponent<RectTransform>();
        temp5.localPosition = new Vector3(temp5.localPosition.x, playerCoef * 7, temp5.localPosition.z);

        if(playerCoef == -1)
        {
            temp4.localRotation = Quaternion.Euler(temp4.localRotation.x, temp4.localRotation.y, playerCoef * rot);
            temp5.localRotation = Quaternion.Euler(temp5.localRotation.x, temp5.localRotation.y, playerCoef * rot);
        }
        else if (playerCoef == 1)
        {
            temp4.localRotation = Quaternion.Euler(temp4.localRotation.x, temp4.localRotation.y, -playerCoef * rot);
            temp5.localRotation = Quaternion.Euler(temp5.localRotation.x, temp5.localRotation.y, -playerCoef * rot);
        }

        SetOutlineColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMeter()
    {
        if (playerCoef == 1)
            hPTextElement.text = $"HP\n{hP}/{maxHP}";
        else if (playerCoef == -1)
            hPTextElement.text = $"{hP}/{maxHP}\nHP";

        float frac = (float)hP / (float)maxHP;
        float rot = gameObject.GetComponent<RectTransform>().rotation.eulerAngles.z;
        float angle_z = playerCoef * (-145 * (frac) + 145 + playerCoef * rot);
        hPIndicatorImageElement.transform.rotation = Quaternion.Euler(0f, 0f, angle_z);

        if (frac == 0)
            hPIndicatorImageElement.color = Hex2Color.Convert("#707070");
        else if (frac <= 0.15)
            hPIndicatorImageElement.color = Hex2Color.Convert("#FF2A00");
        else if (frac <= 0.30)
            hPIndicatorImageElement.color = Hex2Color.Convert("#FFA526");
        else if (frac <= 0.55)
            hPIndicatorImageElement.color = Hex2Color.Convert("#FDF798");
        else if (frac > 0.55)
            hPIndicatorImageElement.color = Hex2Color.Convert("#DCFFE2");

        statusTextElement.text = StatusCondition2String.Convert(statusCondition);

        partyIndicator.Set();
    }

    /*
    public void SetData(PokemonAttributeManager pam)
    {
        StartCoroutine(SetDataHelper(pam));
    }
    */

    public IEnumerator SetData (PocketMonster pam)
    {
        while (pam.currentStats.maxHP == 0)
        {
            yield return null;
        }

        SetMaxHP(pam.currentStats.maxHP);
        SetHP(pam.currentStats.maxHP - pam.damageTaken);
        SetNameLv(pam.pokemonName, pam.level);

        UpdateMeter();
    }

    public void SetHP(int hP)
    {
        this.hP = hP;
        UpdateMeter();
    }

    public IEnumerator SetHPAnimated(int hP)
    {
        while (this.hP > hP)
        {
            yield return new WaitForSeconds(0.01f);
            SetHP(this.hP-1);
        }

        yield return null;
    }

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
    }

    public void SetNameLv(string name, int lv)
    {
        nameLvTextElement.text = $"----------{name.ToUpper()} Lv {lv}----------";
    }

    void SetOutlineColor()
    {
        if (playerCoef == 1)
            hPOutlineImageElement.color = Hex2Color.Convert("#003F80");
        else if (playerCoef == -1)
            hPOutlineImageElement.color = Hex2Color.Convert("#800000");
    }
}