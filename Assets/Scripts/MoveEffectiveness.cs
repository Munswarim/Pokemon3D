using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MoveEffectiveness
{
    static Dictionary<PokemonType, Dictionary<PokemonType, float>> values = new Dictionary<PokemonType, Dictionary<PokemonType, float>>();

    public static void initialization()
    {
        PokemonType[] pokemonTypes = (PokemonType[])Enum.GetValues(typeof(PokemonType));

        foreach (PokemonType attackerType in pokemonTypes)
        {
            values[attackerType] = new Dictionary<PokemonType, float>();
            values[attackerType][PokemonType.None] = 1;
            
        }

        initialization_helper();
    }
    
    static void initialization_helper()
    {
        string filePath = "Assets/CSV/MoveEffectiveness.csv";

        if (!File.Exists(filePath))
            Debug.LogError("File not found.");

        string[] lines = File.ReadAllLines(filePath);

        string[] header = lines[0].Split(',');

        for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
        {
            string[] fields = lines[rowIndex].Split(',');

            string rowName = fields[0];

            for (int columnIndex = 1; columnIndex < header.Length; columnIndex++)
            {
                string columnName = header[columnIndex];

                if (float.TryParse(fields[columnIndex], out float cellValue))
                {
                    PokemonType attackerType = String2PokemonType.Convert(rowName);
                    PokemonType defenderType = String2PokemonType.Convert(columnName);
                    values[attackerType][defenderType] = cellValue;
                }
                else
                {
                    Debug.LogError($"Error parsing cell value at {rowName}, {columnName}.");
                }
            }
        }
    }
    public static float Find(PokemonType attackType, PokemonType defenderType1, PokemonType defenderType2)
    {
        float f1 = values[attackType][defenderType1];
        float f2 = values[attackType][defenderType2];

        /*
         * base case-1: if any one is immune then return 0
         * base case-2: if both are resistant return 0.25
         * base case-3: if both are weakness then return 4
         * base case-4: if one is regular return the other
         * generalization: return the smaller factor
         */

        if (f1 == 0 || f2 == 0) 
            return 0;
        else if (f1 == 0.5 && f2 == 0.5) 
            return 0.25f;
        else if (f1 == 2 && f2 == 2) 
            return 4;
        else if (f1 == 1 && f2 != 1) 
            return f2;
        else if (f1 != 1 && f2 == 1) 
            return f1;
        else 
            return (f1 < f2) ? f1 : f2;
    }
    public static string Remarks(float f)
    {
        switch(f)
        {
            case 0:
                return "of no effect";
            case 0.25f:
                return "not at all effective";
            case 0.5f:
                return "not very effective";
            case 1:
                return "";
            case 2:
                return "super effective";
            case 4:
                return "max effective";
            default:
                return "invalid";
        }
    }
}
