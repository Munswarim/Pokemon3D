using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueBox : MonoBehaviour
{
    public TMP_Text dialogueText;

    public void SetDialogue(string dialogue)
    { 
        StartCoroutine(SetDialogueAnimated(dialogue));
    }

    public IEnumerator SetDialogueAnimated(string dialogue)
    {
        dialogueText.text = "";
        foreach (var character in dialogue.ToCharArray()) 
        {
            dialogueText.text += character;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
