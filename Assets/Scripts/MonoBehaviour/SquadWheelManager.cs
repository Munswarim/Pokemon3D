using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SquadWheelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public BattleSystem battleSystem;
    List<PocketMonster> pokemonList;

    public List<RawImage> wheelBg;
    public List<RawImage> spriteList;

    public TMP_Text nameText;
    public TMP_Text lvText;
    public TMP_Text hPText;
    public TMP_Text statusText;
    
    List<int> positionIndex;
    int[] selectorAngle = { 180, -100, -50, 0, 50, 100 };

    GameObject swg;
    Button excludeButton;

    void Start()
    {
        swg = GameObject.Find("SquadWheelGroup");
        excludeButton = GameObject.Find("ReturnFromSquadButton").GetComponent<Button>();

        foreach (var segmentBg in wheelBg)
        {
            segmentBg.color = DecideColor(0);
        }

        pokemonList = new List<PocketMonster>();
        positionIndex = new List<int>();

        for(int i = 0; i < battleSystem.playerTeam.members.Count; i++)
        {
            pokemonList.Add(battleSystem.playerTeam.members[i]);
            positionIndex.Add(i);
        }

        SetData();
    }

    void SetData()
    {
        for (int i = 0; i < pokemonList.Count; i++)
        {
            wheelBg[i].color = DecideColor(pokemonList[i].NormalizedHP());
            spriteList[i].texture = pokemonList[i].officialArtwork.texture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetData();
        StartCoroutine(Listener());
    }
    IEnumerator Listener()
    {
        if (gameObject.transform.parent.gameObject.activeSelf)
        {
            Vector2 delta = gameObject.transform.position - Input.mousePosition;
            float angle = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;

            // angle += 180;
            angle *= -1;
            // Debug.Log(angle);

            int index = -1;

            if (-135 <= angle && angle < -75)
            {
                index = 1;
            }
            else if (-75 <= angle && angle < -25)
            {
                index = 2;
            }
            else if (-25 <= angle && angle < 25)
            {
                index = 3;
            }
            else if (25 <= angle && angle < 75)
            {
                index = 4;
            }
            else if (75 <= angle && angle < 135)
            {
                index = 5;
            }
            else
            {
                index = 0;
            }

            GameObject.Find("SelectionIndicator").transform.eulerAngles = new Vector3(0, 0, selectorAngle[index]);

            if (index < pokemonList.Count)
            {
                nameText.text = pokemonList[index].pokemonName;
                lvText.text = $"Lv {pokemonList[index].level}";
                hPText.text = $"HP: {pokemonList[index].hP}/{pokemonList[index].currentStats.maxHP}";
                statusText.text = StatusCondition2String.Convert(pokemonList[index].statusCondition);
                if (0 < index && Input.GetMouseButtonDown(0) && !IsPointerOverReturnButton() && pokemonList[index].hP > 0)
                {
                    PocketMonster temp = pokemonList[index];
                    pokemonList[index] = pokemonList[0];
                    pokemonList[0] = temp;

                    swg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);

                    yield return battleSystem.Swap(pokemonList[0]);
                    swg.SetActive(false);
                    swg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                }
            }
            else
            {
                nameText.text = "";
                lvText.text = "Empty";
                hPText.text = "";
                statusText.text = "";
            }
        }
        yield return null;
    }

    Color DecideColor(float frac)
    {
        if (frac == 0)
            return  Hex2Color.Convert("#707070");
        else if (frac <= 0.15)
            return Hex2Color.Convert("#FF2A00");
        else if (frac <= 0.30)
            return Hex2Color.Convert("#FFA526");
        else if (frac <= 0.55)
            return Hex2Color.Convert("#FDF798");
        else
            return Hex2Color.Convert("#DCFFE2");
        
    }

    bool IsPointerOverReturnButton()
    {
        // Check if the pointer is over the excludeButton
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // Ensure that there is a GraphicRaycaster component on the GameObject
        GraphicRaycaster raycaster = swg.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            Debug.LogError("No GraphicRaycaster component found on the GameObject.");
            return false;
        }

        List<RaycastResult> results = new List<RaycastResult>();

        // Ensure the EventSystem.current is not null
        if (EventSystem.current == null)
        {
            Debug.LogError("EventSystem.current is null.");
            return false;
        }

        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == excludeButton.gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
