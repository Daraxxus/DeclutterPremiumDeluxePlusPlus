using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFunctions
{
    public static Vector3 With(this Vector3 vec, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vec.x, y ?? vec.y, z ?? vec.z);
    }

    public static List<T> RandomizeList<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    public static List<SaveData.HighScore> QuickSortList(this List<SaveData.HighScore> list)
    {
        System.Random r = new System.Random();
        List<SaveData.HighScore> less = new List<SaveData.HighScore>();
        List<SaveData.HighScore> greater = new List<SaveData.HighScore>();
        if (list.Count <= 1)
            return list;
        int pos = r.Next(list.Count);

        SaveData.HighScore pivot = list[pos];
        list.RemoveAt(pos);
        foreach (SaveData.HighScore player in list)
        {
            if (player.Score <= pivot.Score)
            {
                less.Add(player);
            }
            else
            {
                greater.Add(player);
            }
        }
        return concat(QuickSortList(less), pivot, QuickSortList(greater));
    }

    public static List<SaveData.HighScore> concat(List<SaveData.HighScore> less, SaveData.HighScore pivot, List<SaveData.HighScore> greater)
    {
        List<SaveData.HighScore> sorted = new List<SaveData.HighScore>(less);
        sorted.Add(pivot);
        foreach (SaveData.HighScore i in greater)
        {

            sorted.Add(i);
        }

        return sorted;
    }
}
