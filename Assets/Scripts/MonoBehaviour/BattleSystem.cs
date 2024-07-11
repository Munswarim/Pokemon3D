using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public BattleUnit playerUnit;
    public HPMeterManager playerHPMeter;

    public BattleUnit enemyUnit;
    public HPMeterManager enemyHPMeter;

    public BattleDialogueBox dialogueBox;

    public MovesetLoader moveset;

    [Range(-1, 3)]
    public int playerMoveIndex;
    
    [Range(-1, 3)]
    public int enemyMoveIndex;

    BattleState state;

    public PokemonParty playerTeam;
    public PokemonParty enemyTeam;


    // Start is called before the first frame update
    void Start()
    {
        var team1 = GameObject.Find("PlayerTeam").GetComponent<PokemonParty>();
        var team2 = GameObject.Find("EnemyTeam").GetComponent<PokemonParty>();
        SetupBattle(team1, team2);    
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.Busy)
        {
            state = BattleState.PlayerAction;

            Busy();
        }
    }

    public void SetupBattle(PokemonParty playerTeam, PokemonParty enemyTeam)
    {
        this.playerTeam = playerTeam;
        this.enemyTeam = enemyTeam;

        // this.enemyTeam.allowFaints = 2;

        StartCoroutine(SetupBattleHelper());
    }

    IEnumerator SetupBattleHelper()
    {
        yield return null;

        playerUnit.Setup(playerTeam.GetFirstFitPokemon());
        yield return playerHPMeter.SetData(playerUnit.pocketMonster);

        enemyUnit.Setup(enemyTeam.GetRandomMember());
        yield return enemyHPMeter.SetData(enemyUnit.pocketMonster);

        yield return null;
        dialogueBox.SetDialogue($"A wild {enemyUnit.pocketMonster.pokemonName} appeared!");

        yield return new WaitForSeconds(1f);
        dialogueBox.SetDialogue($"You sent out {playerUnit.pocketMonster.pokemonName}!");

        yield return null;
        moveset.LoadMoves(playerUnit.pocketMonster);

        yield return new WaitForSeconds(1f);
        PlayerAction();
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
        playerMoveIndex = enemyMoveIndex = -1;

        dialogueBox.SetDialogue($"Choose action for {playerUnit.pocketMonster.pokemonName}.");

        GameObject.Find("UIRefFetcher").GetComponent<UIReference>().playerActionButtonGroup.SetActive(true);
    }
    public void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogueBox.SetDialogue("Select a move.");
    }
    public void EnemyMove()
    {
        state = BattleState.EnemyMove;
        System.Random random = new System.Random();
        enemyMoveIndex = random.Next(0, enemyUnit.pocketMonster.moves.Count);

        state = BattleState.Busy;
    }
    public void Busy()
    {
        if (playerUnit.pocketMonster.currentStats.speed >= enemyUnit.pocketMonster.currentStats.speed)
        {
            StartCoroutine(PerformPlayerMoveFirst());
        }
        else
        {
            StartCoroutine(PerformEnemyMoveFirst());
        }
    }

    public IEnumerator Swap(PocketMonster pm)
    {
        playerUnit.Retreat();
        yield return new WaitForSeconds(0.25f);
        
        playerUnit.Setup(pm);

        yield return playerHPMeter.SetData(playerUnit.pocketMonster);

        yield return null;
        dialogueBox.SetDialogue($"You sent out {playerUnit.pocketMonster.pokemonName}!");

        yield return new WaitForSeconds(1f);
        moveset.LoadMoves(playerUnit.pocketMonster);

        PlayerAction();

    }

    IEnumerator PerformPlayerMoveFirst()
    {
        int faintCount = enemyTeam.GetFaintCount();
        yield return PerformPlayerMove();
        if (faintCount == enemyTeam.GetFaintCount())
            yield return PerformEnemyMove();
        PlayerAction();
    }
    IEnumerator PerformEnemyMoveFirst()
    {
        int faintCount = playerTeam.GetFaintCount();
        yield return PerformEnemyMove();
        if (faintCount == playerTeam.GetFaintCount())
            yield return PerformPlayerMove();
        PlayerAction();
    }


    IEnumerator PerformPlayerMove()
    {
        yield return null;
        string str = $"{playerUnit.pocketMonster.pokemonName} used {playerUnit.pocketMonster.moves[playerMoveIndex].moveName}";
        Debug.Log(str);
        dialogueBox.SetDialogue(str);

        yield return new WaitForSeconds(1f);
        DamageDetails damageDetails = enemyUnit.pocketMonster.TakeDamage(playerUnit.pocketMonster.moves[playerMoveIndex], playerUnit);
        yield return enemyHPMeter.SetHPAnimated(enemyUnit.pocketMonster.hP);

        string moveRemarks = "";
        if (damageDetails.critical)
            moveRemarks += "A critical Hit! ";
        if (damageDetails.effectiveness != 1)
            moveRemarks += "It's " + MoveEffectiveness.Remarks(damageDetails.effectiveness) + "!";
        if(moveRemarks != "")
        {
            dialogueBox.SetDialogue(moveRemarks);
            yield return new WaitForSeconds(1f);
        }

        playerUnit.pocketMonster.moves[playerMoveIndex].pP--;
        moveset.LoadMoves(playerUnit.pocketMonster);

        if (damageDetails.fainted)
        {
            dialogueBox.SetDialogue($"{enemyUnit.pocketMonster.pokemonName} got knocked out!");
            yield return new WaitForSeconds(2f);

            var nextUnit = enemyTeam.GetRandomMember();

            if (enemyTeam.ReachedFaintLimit() || nextUnit == null)
            {
                dialogueBox.SetDialogue($"Enemy team has been defeated!");
                yield return new WaitForSeconds(5f);
            }
            else
            {
                enemyUnit.Faint();
                enemyUnit.Setup(nextUnit);
                yield return enemyHPMeter.SetData(enemyUnit.pocketMonster);

                yield return null;
                dialogueBox.SetDialogue($"A wild {enemyUnit.pocketMonster.pokemonName} appeared!");
                
                yield return new WaitForSeconds(1f);
            }
        }

    }

    IEnumerator PerformEnemyMove()
    {
        yield return null;
        string str = $"{enemyUnit.pocketMonster.pokemonName} used {enemyUnit.pocketMonster.moves[enemyMoveIndex].moveName}";
        Debug.Log(str);
        dialogueBox.SetDialogue(str);

        yield return new WaitForSeconds(1f);
        DamageDetails damageDetails = playerUnit.pocketMonster.TakeDamage(enemyUnit.pocketMonster.moves[enemyMoveIndex], enemyUnit);
        yield return playerHPMeter.SetHPAnimated(playerUnit.pocketMonster.hP);

        string moveRemarks = "";
        if (damageDetails.critical)
            moveRemarks += "A critical Hit! ";
        if (damageDetails.effectiveness != 1)
            moveRemarks += "It's " + MoveEffectiveness.Remarks(damageDetails.effectiveness) + "!";
        if (moveRemarks != "")
        {
            dialogueBox.SetDialogue(moveRemarks);
            yield return new WaitForSeconds(1f);
        }

        enemyUnit.pocketMonster.moves[enemyMoveIndex].pP--;

        if (damageDetails.fainted)
        {
            dialogueBox.SetDialogue($"{playerUnit.pocketMonster.pokemonName} got knocked out!");
            yield return new WaitForSeconds(2f);

            var nextUnit = playerTeam.GetFirstFitPokemon();

            if (playerTeam.ReachedFaintLimit() || nextUnit == null)
            {
                dialogueBox.SetDialogue($"Player team has been defeated!");
                yield return new WaitForSeconds(5f);
            }
            else
            {
                playerUnit.Faint();
                playerUnit.Setup(nextUnit);
                yield return playerHPMeter.SetData(playerUnit.pocketMonster);

                yield return null;
                dialogueBox.SetDialogue($"You sent out {playerUnit.pocketMonster.pokemonName}!");

                yield return new WaitForSeconds(1f);
                moveset.LoadMoves(playerUnit.pocketMonster);
            }
        }

    }


}
