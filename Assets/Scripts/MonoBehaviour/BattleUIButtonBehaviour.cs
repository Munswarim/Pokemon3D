using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BattleUIButtonBehaviour : MonoBehaviour
{
    UIReference UI;
    BattleSystem battleSystem;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("UIRefFetcher").GetComponent<UIReference>();
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>(); ;
    }

    public void FightButton()
    {
        StartCoroutine(FightButtonHelper());
    }
    IEnumerator FightButtonHelper()
    {
        yield return new WaitForSeconds(0.25f);
        UI.playerActionButtonGroup.SetActive(false);
        UI.movesetButtonGroup.SetActive(true);
        battleSystem.PlayerMove();
    }
    public void ReturnButton()
    {
        StartCoroutine (ReturnButtonHelper());
    }
    IEnumerator ReturnButtonHelper()
    {
        yield return new WaitForSeconds(0.25f);
        UI.playerActionButtonGroup.SetActive(true);
        UI.movesetButtonGroup.SetActive(false);
        UI.bagPanel.SetActive(false);
        UI.squadWheelGroup.SetActive(false);
        UI.dialogueBox.SetActive(true);
        battleSystem.PlayerAction();
    }

    public void BagButton()
    {
        StartCoroutine(BagButtonHelper());
    }
    IEnumerator BagButtonHelper()
    {
        yield return new WaitForSeconds(0.25f);
        UI.playerActionButtonGroup.SetActive(false);
        UI.bagPanel.SetActive(true);
        UI.dialogueBox.SetActive(false);
    }

    public void SquadButton()
    {
        StartCoroutine(SquadButtonHelper());
    }
    IEnumerator SquadButtonHelper()
    {
        yield return new WaitForSeconds(0.25f);
        UI.playerActionButtonGroup.SetActive(false);
        UI.squadWheelGroup.SetActive(true);
        UI.dialogueBox.GetComponent<BattleDialogueBox>().SetDialogue("Choose a Pokemon to switch with.");
    }

    public void RunButton()
    {
        StartCoroutine(RunButtonHelper());
    }
    IEnumerator RunButtonHelper()
    {
        yield return new WaitForSeconds(0.25f);
        UI.playerActionButtonGroup.SetActive(false);
        UI.dialogueBox.GetComponent<BattleDialogueBox>().SetDialogue("Got away safely.");
        Application.Quit();
    }
}
