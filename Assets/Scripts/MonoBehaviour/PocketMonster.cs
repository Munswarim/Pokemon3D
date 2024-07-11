using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PocketMonster : MonoBehaviour
{
    [Range(-1,1)]
    public int playerCoef;

    public int id;

    public GameObject model;
    public RawImage officialArtwork;
    
    public RawImage frontImg;
    public RawImage backImg;

    public string pokemonName;
    public string evolvesFrom;
    [TextArea] public string description;

    public PokemonType type1;
    public PokemonType type2;

    public PokemonGender gender;
    public int baseHappiness;
    public int captureRate;
    public GrowthRate growthRate;

    public PokemonNature nature;
    public List<string> ability;

    public List<string> varieties;

    public int level;
    public long exp;

    public PokemonStats baseStats;
    public PokemonStats currentStats;
    public PokemonStats individualValues;
    public PokemonStats effortValues;
    public PokemonStats boostValues;

    public int hP;
    public int damageTaken;
    public StatusCondition statusCondition;

    public Dictionary<string, int> learnableMoves;
    public List<PokemonMove> moves;

    bool movesOk;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        movesOk = false;

        baseStats = ScriptableObject.CreateInstance<PokemonStats>();
        currentStats = ScriptableObject.CreateInstance<PokemonStats>();
        individualValues = ScriptableObject.CreateInstance<PokemonStats>();
        effortValues = ScriptableObject.CreateInstance<PokemonStats>();
        boostValues = ScriptableObject.CreateInstance<PokemonStats>();

        learnableMoves = new Dictionary<string, int>();

        StartCoroutine(PopulateBasicData());
        StartCoroutine(PopulateBattleData());
    }

    void SetCurrentHP()
    {
        if (damageTaken > currentStats.maxHP)
        {
            damageTaken = currentStats.maxHP;
        }
        hP = currentStats.maxHP - damageTaken;
    }

    IEnumerator PopulateBasicData()
    {
        string PokeApiUrl = "https://pokeapi.co/api/v2/pokemon-species/";
        string url = (id != 0) ? $"{PokeApiUrl}{id}" : $"{PokeApiUrl}{pokemonName}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            PokemonAPIBasicData pokemonData = JsonUtility.FromJson<PokemonAPIBasicData>(request.downloadHandler.text);

            id = pokemonData.id;
            pokemonName = pokemonData.name;
            evolvesFrom = pokemonData.evolves_from_species.name;
            baseHappiness = pokemonData.base_happiness;
            captureRate = pokemonData.capture_rate;
            // growthRate = pokemonData.growth_rate.name;

            SetPokemonModel();
            SetGrowthRate(pokemonData);
            SetVarieties(pokemonData);
            SetDescription(pokemonData);
            SetGender(pokemonData);
        }
    }
    void SetVarieties(PokemonAPIBasicData pokemonData)
    {
        varieties.Clear();
        foreach (Varieties variety in pokemonData.varieties)
        {
            varieties.Add(variety.pokemon.name);
        }
    }
    void SetDescription(PokemonAPIBasicData pokemonData)
    {
        string str = "";
        foreach (Flavor_text_entries entry in pokemonData.flavor_text_entries)
        {
            string substr = entry.flavor_text.Replace("POKéMON", "Pokémon").Replace("­- ", "");
            if (entry.language.name == "en" && !str.Contains(substr))
            {
                str += substr + " ";
                // break;
            }
            if (str.Length > 1000)
            {
                break;
            }
        }
        description = str.Replace("\f", " ").Replace("\n", " "); //.Replace("<newline>", "\n");
    }
    void SetGrowthRate(PokemonAPIBasicData pokemonData)
    {
        switch (pokemonData.growth_rate.name)
        {
            case "medium-fast":
                growthRate = new MediumFast();
                break;
            case "erratic":
                growthRate = new Erratic();
                break;
            case "slightly-fast":
                growthRate = new SlightlyFast();
                break;
            case "fluctuating":
                growthRate = new Fluctuating();
                break;
            case "slightly-slow":
                growthRate = new SlightlySlow();
                break;
            case "medium-slow":
                growthRate = new MediumSlow();
                break;
            case "fast":
                growthRate = new Fast();
                break;
            case "slow":
                growthRate = new Slow();
                break;
            default:
                growthRate = new GrowthRate();
                break;
        }
        growthRate.text = pokemonData.growth_rate.name;
        growthRate.LevelReached(level);
    }

    void SetGender(PokemonAPIBasicData pokemonData)
    {
        if (pokemonData.gender_rate < 0)
        {
            gender = PokemonGender.None;
            return;
        }

        System.Random random = new System.Random();
        int polyhedralDice = random.Next(1, 9);

        if (polyhedralDice <= pokemonData.gender_rate)
            gender = PokemonGender.Female;
        else if (polyhedralDice > pokemonData.gender_rate)
            gender = PokemonGender.Male;

    }

    IEnumerator PopulateBattleData()
    {
        string PokeApiUrl = "https://pokeapi.co/api/v2/pokemon/";
        string url = (id != 0) ? $"{PokeApiUrl}{id}" : $"{PokeApiUrl}{pokemonName}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            PokemonAPIBattleData pokemonData = JsonUtility.FromJson<PokemonAPIBattleData>(request.downloadHandler.text.Replace("official-artwork", "official_artwork"));

            id = pokemonData.id;

            StartCoroutine(PopulateOfficialArtData(pokemonData));
            StartCoroutine(Populate2DSpriteData(pokemonData, true));
            StartCoroutine(Populate2DSpriteData(pokemonData, false));

            SetTypes(pokemonData);
            SetAbilities(pokemonData);

            baseStats.maxHP = pokemonData.stats[0].base_stat;
            baseStats.attack = pokemonData.stats[1].base_stat;
            baseStats.defence = pokemonData.stats[2].base_stat;
            baseStats.spAttack = pokemonData.stats[3].base_stat;
            baseStats.spDefence = pokemonData.stats[4].base_stat;
            baseStats.speed = pokemonData.stats[5].base_stat;

            SetIndividualValues();
            // GainEffortValues(maxHP = x, attack = y, defence = z)
            SetCurrentStats();

            SetLearnableMoves(pokemonData);
            Learn4RandomMoves();
        }
    }
    void SetIndividualValues()
    {
        System.Random random = new System.Random();

        individualValues.maxHP = random.Next(0, 32);
        individualValues.attack = random.Next(0, 32);
        individualValues.defence = random.Next(0, 32);
        individualValues.spAttack = random.Next(0, 32);
        individualValues.spDefence = random.Next(0, 32);
        individualValues.speed = random.Next(0, 32);
    }
    void SetCurrentStats()
    {
        currentStats.maxHP = (int)(0.01 * (2 * baseStats.maxHP + individualValues.maxHP + (int)(0.25 * effortValues.maxHP)) * level) + level + 10;
        currentStats.attack = (int)(0.01 * (2 * baseStats.attack + individualValues.attack + (int)(0.25 * effortValues.attack)) * level) + 5; // * NATURE ;
        currentStats.defence = (int)(0.01 * (2 * baseStats.defence + individualValues.defence + (int)(0.25 * effortValues.defence)) * level) + 5; // * NATURE ;
        currentStats.spAttack = (int)(0.01 * (2 * baseStats.spAttack + individualValues.spAttack + (int)(0.25 * effortValues.spAttack)) * level) + 5; // * NATURE ;
        currentStats.spDefence = (int)(0.01 * (2 * baseStats.spDefence + individualValues.spDefence + (int)(0.25 * effortValues.spDefence)) * level) + 5; // * NATURE ;
        currentStats.speed = (int)(0.01 * (2 * baseStats.speed + individualValues.speed + (int)(0.25 * effortValues.speed)) * level) + 5; // * NATURE ;

        hP = currentStats.maxHP - damageTaken;
        growthRate.LevelReached(level);
    }
    void GainEffortValues(int maxHP = 0, int attack = 0, int defence = 0, int spAttack = 0, int spDefence = 0, int speed = 0)
    {
        effortValues.maxHP += maxHP;
        effortValues.attack += attack;
        effortValues.defence += defence;
        effortValues.spAttack += spAttack;
        effortValues.spDefence += spDefence;
        effortValues.speed += speed;
    }
    void SetTypes(PokemonAPIBattleData pokemonData)
    {
        string t1 = pokemonData.types[0].type.name;
        string t2 = (pokemonData.types.Length > 1) ? pokemonData.types[1].type.name : "none";

        type1 = String2PokemonType.Convert(t1);
        type2 = String2PokemonType.Convert(t2);
    }
    void SetAbilities(PokemonAPIBattleData pokemonData)
    {
        ability.Clear();
        foreach (Abilities abilities in pokemonData.abilities)
        {
            ability.Add(abilities.ability.name);
        }
    }
    void SetLearnableMoves(PokemonAPIBattleData pokemonData)
    {
        learnableMoves.Clear();
        foreach (Moves m in pokemonData.moves)
        {
            string move_name = m.move.name;
            int learn_at_Level = m.version_group_details[0].level_learned_at;
            learnableMoves.Add(move_name, learn_at_Level);
        }
        // PrintLeanableMoves();
    }
    void PrintLeanableMoves()
    {
        string str = "";
        foreach (var move in learnableMoves)
        {
            str += $"Lv-{move.Value} : {move.Key}\n";
        }
        Debug.Log(str);
    }
    void Learn4RandomMoves()
    {
        moves.Clear();

        List<string> list = new List<string>(learnableMoves.Keys);

        System.Random random = new System.Random();

        int no_of_moves = random.Next(1, 5);

        for (int i = 0, j = 0; i < no_of_moves; i++)
        {
            int idx = random.Next(0, list.Count);
            // Debug.Log(list[idx]);
            StartCoroutine(PopulateMoveData(list[idx], () =>
            {
                j++;
                if (playerCoef == 1 && j == no_of_moves)
                {
                    foreach(var move in moves)
                        // Debug.Log(move.moveName);
                    // Debug.Log($"learned {no_of_moves} moves");
                    movesOk = true;
                    // ShowMoveNameOnUI();
                }
                if (j == no_of_moves)
                {
                    movesOk = true;
                }
            }));
        }
    }
    public bool Moves_ok()
    {
        return movesOk;
    }
    /*
    void ShowMoveNameOnUI(int i)
    {
        GameObject.Find("UIRefFetcher").GetComponent<UIReference>().movesetButtonGroup.GetComponent<MovesetLoader>().LoadMoves( i);
    }
    void ShowMoveNameOnUI()
    {
        GameObject.Find("UIRefFetcher").GetComponent<UIReference>().movesetButtonGroup.GetComponent<MovesetLoader>().LoadMoves();
    }
    */
    IEnumerator PopulateMoveData(string moveName, System.Action onComplete)
    {
        string PokeApiUrl = "https://pokeapi.co/api/v2/move/";
        string url = $"{PokeApiUrl}{moveName}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            PokemonAPIMoveData moveData = JsonUtility.FromJson<PokemonAPIMoveData>(request.downloadHandler.text);

            PokemonMove pm = ScriptableObject.CreateInstance<PokemonMove>();

            pm.moveName = moveName;
            pm.id = moveData.id;

            foreach (Flavor_text_entries entry in moveData.flavor_text_entries)
                if (entry.language.name == "en")
                    pm.description = entry.flavor_text.Replace("\n", " ");

            pm.type = String2PokemonType.Convert(moveData.type.name);
            pm.category = String2PokemonMoveCategory.Convert(moveData.damage_class.name);
            pm.power = moveData.power;
            pm.accuracy = moveData.accuracy;
            pm.pP = pm.basePP = pm.maxPP = moveData.pp;
            pm.effectChance = moveData.effect_chance;
            moves.Add(pm);

            onComplete?.Invoke();
        }
    }
    IEnumerator Populate2DSpriteData(PokemonAPIBattleData pokemonData, bool front)
    {
        string url = (front) ? pokemonData.sprites.front_default : pokemonData.sprites.back_default;
        // Debug.Log(url);

        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Texture2D artworkTexture = DownloadHandlerTexture.GetContent(request);
            if (front)
                frontImg.texture = artworkTexture;
            else
                backImg.texture = artworkTexture;
        }
    }

    IEnumerator PopulateOfficialArtData(PokemonAPIBattleData pokemonData)
    {
        string url = pokemonData.sprites.other.official_artwork.front_default.Replace("official_artwork", "official-artwork");
        // Debug.Log(url);

        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Texture2D artworkTexture = DownloadHandlerTexture.GetContent(request);
            officialArtwork.texture = artworkTexture;
        }
    }

    void SetPokemonModel()
    {
        Destroy(model);
        model = null;

        string url = $"Assets/Exported/{pokemonName}/{pokemonName}.prefab";
        GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(url);

        if (modelPrefab != null)
        {
            model = Instantiate(modelPrefab, transform.position, transform.rotation);
            model.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError($"{url} not found.");
        }
    }

    public DamageDetails TakeDamage(PokemonMove move, BattleUnit attacker)
    {
        // Formula = ((2 * attacker_level + 10) / 250 * move_power * (attacker_attack / defender_defence) + 2) * Modifier
        // Modifier = Stab * Weather * Critical * Move_effectiveness * Badge * Status_condition * Random(0.85 ~ 1.00)

        System.Random random = new System.Random();

        float rand = (float)(random.NextDouble() * (1.0 - 0.85) + 0.85);
        float moveEffectiveness = MoveEffectiveness.Find(move.type, type1, type2);
        float critical = (random.NextDouble() < 0.0625) ? 2 : 1;

        float attack = (move.category == PokemonMoveCategory.Special) ? (float)attacker.pocketMonster.currentStats.spAttack : (float)attacker.pocketMonster.currentStats.attack;
        float defence = (move.category == PokemonMoveCategory.Special) ? (float)currentStats.spDefence : (float)currentStats.defence;

        float modifier = rand * moveEffectiveness * critical;
        int damage = (int)(((2f * attacker.pocketMonster.level + 10f) / 250f * move.power * attack / defence + 2f) * modifier);

        damageTaken += damage;
        SetCurrentHP();

        var damageDetails = new DamageDetails();
        damageDetails.effectiveness = moveEffectiveness;
        if (critical == 2)
            damageDetails.critical = true;
        if(hP == 0) 
            damageDetails.fainted = true;
        
        return damageDetails;
    }

    public void LevelUp()
    {
        level++;
        SetCurrentStats();
    }

    public void ProperHeal()
    {
        hP = currentStats.maxHP;
        damageTaken = 0;

        foreach(PokemonMove move in moves)
        {
            move.pP = move.maxPP;
        }
    }
    public float NormalizedHP()
    {
        return (float)hP / currentStats.maxHP;
    }
}

public class DamageDetails
{
    public bool fainted;
    public float effectiveness;
    public bool critical;

    public DamageDetails()
    {
        fainted = false;
        effectiveness = 1;
        critical = false;
    }
}