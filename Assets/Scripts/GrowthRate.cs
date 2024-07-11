using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrowthRate
{
    public string text;
    public long prevLevelUpAtExp;
    public long nextLevelUpAtExp;
    public virtual int ExpAtLevel(int n = 1)
    {
        return -1;
    }
    public void LevelReached(int n = 1)
    {
        prevLevelUpAtExp = ExpAtLevel(n);
        nextLevelUpAtExp = ExpAtLevel(n + 1);
    }

}

public class MediumFast : GrowthRate
{
    public override int ExpAtLevel(int n=1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else
            exp = Math.Pow(n, 3);

        return (int) exp;
    }
}

public class Erratic : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;
        
        if (n < 2)
            exp =  0;
        else if (n < 50)
            exp = (Math.Pow(n, 3) * (100 - n)) / 50;
        else if (n < 68)
            exp = (Math.Pow(n, 3) * (150 - n)) / 100;
        else if (n < 98)
            exp = Math.Pow(n, 3) * (int)((1911 - 10 * n) / 3);
        else
            exp = (Math.Pow(n, 3) * (160 - n)) / 100;
        
        return (int) exp;
    }
}

public class SlightlyFast : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else
            exp = 3 / 4 * Math.Pow(n, 3) + 10 * Math.Pow(n, 2) - 30;

        return (int) exp;
    }
}

public class Fluctuating : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else if (n < 15)
            exp = Math.Pow(n, 3) * ((int)((n + 1) / 3) + 24) / 50;
        else if (n < 36)
            exp = Math.Pow(n, 3) * (n + 14) / 50;
        else
            exp = Math.Pow(n, 3) * ((int)(n/2) + 32) / 50;

        return (int) exp;
    }
}

public class SlightlySlow : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else
            exp = 3 / 4 * Math.Pow(n, 3) + 20 * Math.Pow(n, 2) - 70;

        return (int)exp;
    }
}

public class MediumSlow : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else
            exp = 6/5 * Math.Pow(n, 3) - 15 * Math.Pow(n, 2) + 100 * n - 140;

        return (int) exp;
    }
}

public class Fast : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;
        
        if (n < 2)
            exp = 0;
        else
            exp = 4 * Math.Pow(n, 3) / 5;

        return (int) exp;
    }
}

public class Slow : GrowthRate
{
    public override int ExpAtLevel(int n = 1)
    {
        double exp;

        if (n < 2)
            exp = 0;
        else
            exp = 5 * Math.Pow(n, 3) / 4;

        return (int) exp;
    }
} 