using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MovesetLoader : MonoBehaviour
{
    public List<Button> moveButtons;

    public BattleSystem battleSystem;

    List<int> moveIndex;

    // Start is called before the first frame update
    void Start()
    {
        // HideAll();
        // StartCoroutine(LoadMoves());
        
        moveIndex = new List<int>();
        for (int i = 0; i<4; i++)
        {
            moveIndex.Add(-1);
        }
    }

    // IEnumerator LoadMoves()
    public void LoadMoves(PocketMonster pam)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, -500);
        gameObject.SetActive(true);
        
        StartCoroutine(LoadMovesHelper(pam));
    }

    IEnumerator LoadMovesHelper(PocketMonster pam)
    {
        while(!pam.Moves_ok())
            yield return null;

        int n = pam.moves.Count;
        Debug.Log($"{pam.pokemonName} knows {n} moves.");

        while (n > 4)
        {
            Debug.Log($"{pam.moves[n-1].moveName} has been forgotten.");
            pam.moves.RemoveAt(n - 1);
            n--;
        }

        int k = 4 - n;

        List<int> btnIdx = new List<int>();

        for (int i = k; i < 4; i++)
            btnIdx.Add(i);

        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(0.25f);
            PokemonMove move = pam.moves[i];
            string hex = PokemonType2Hex.Convert(move.type);
            string moveName = move.moveName;
            int currentPP = move.pP;
            int totalPP = move.maxPP;
            string moveType = String2PokemonType.Convert(move.type);

            string moveCategory;
            if (move.category == PokemonMoveCategory.Physical)
                moveCategory = "physical";
            else if (move.category == PokemonMoveCategory.Special)
                moveCategory = "special";
            else if (move.category == PokemonMoveCategory.Status)
                moveCategory = "status";
            else
                moveCategory = "none";

            int j = btnIdx[i];
            // Debug.Log(j);
            moveIndex[j] = i;

            moveButtons[j].gameObject.SetActive(true);
            moveButtons[j].transform.Find("ThemedBackground").GetComponent<Image>().color = Hex2Color.Convert(hex);
            moveButtons[j].transform.Find("TypeIcon").GetComponent<RawImage>().texture = LoadTextureFromFile($"Assets/Art/type_icon/{moveType}.png");
            moveButtons[j].transform.Find("CategoryIcon").GetComponent<RawImage>().texture = LoadTextureFromFile($"Assets/Art/category_icon/{moveCategory}.png");
            moveButtons[j].transform.Find("MoveNameText").GetComponent<TMP_Text>().text = moveName;
            moveButtons[j].transform.Find("PPText").GetComponent<TMP_Text>().text = $"{currentPP}/{totalPP}";
        }

        for (int i = 0; i < k; i++)
        {
            moveIndex[i] = -1;
            moveButtons[i].gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180, 160);
    }
    public void LoadMoves(PocketMonster pam, int i)
    {
        PokemonMove move = pam.moves[i];

        string hex = PokemonType2Hex.Convert(move.type);
        string moveName = move.moveName;
        int currentPP = move.pP;
        int totalPP = move.maxPP;
        string moveType = String2PokemonType.Convert(move.type);

        string moveCategory;
        if (move.category == PokemonMoveCategory.Physical)
            moveCategory = "physical";
        else if (move.category == PokemonMoveCategory.Special)
            moveCategory = "special";
        else if (move.category == PokemonMoveCategory.Status)
            moveCategory = "status";
        else
            moveCategory = "none";

        moveButtons[i].gameObject.SetActive(true);
        moveButtons[i].transform.Find("ThemedBackground").GetComponent<Image>().color = Hex2Color.Convert(hex);
        moveButtons[i].transform.Find("TypeIcon").GetComponent<RawImage>().texture = LoadTextureFromFile($"Assets/Art/type_icon/{moveType}.png");
        moveButtons[i].transform.Find("CategoryIcon").GetComponent<RawImage>().texture = LoadTextureFromFile($"Assets/Art/category_icon/{moveCategory}.png");
        moveButtons[i].transform.Find("MoveNameText").GetComponent<TMP_Text>().text = moveName;
        moveButtons[i].transform.Find("PPText").GetComponent<TMP_Text>().text = $"{currentPP}/{totalPP}";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideAll()
    {
        foreach(Button btn in moveButtons)
            btn.gameObject.SetActive(false);
    }

    Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // This assumes the fileData is in PNG or JPEG format

        return texture;
    }

    public void GetIndex(Button button)
    {
        int button_index = -1;
        int move_index = -1;

        for (int i = 0; i< 4; i++)
        {
            if (moveButtons[i] ==  button)
            {
                button_index = i;
                move_index = moveIndex[i];
                break;
            }
        }
        Debug.Log($"{button_index} : {move_index}");
        battleSystem.playerMoveIndex = move_index;
        StartCoroutine(Vanish());

        battleSystem.EnemyMove();
    }
    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }
}

