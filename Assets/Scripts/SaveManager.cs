using UnityEngine;
using System.Collections;

public static class SaveManager
{
    public static int[] LevelStar = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static int audioEnable = 1;

    //Không dùng tới | Chỉ Test
    public static void LoadAllData()
    {
        for (int i = 0; i < 20; i++)
        {
            LevelStar[i] = PlayerPrefs.GetInt("lvStar_" + (i + 1).ToString());
        }
    }

    //Không dùng tới | Chỉ Test
    public static void SaveAllData()
    {
        for (int i = 0; i < 20; i++)
        {
            PlayerPrefs.SetInt("lvStar_" + (i + 1).ToString(), LevelStar[i]);
        }
    }

    public static void setLevelStar(int level, int star)
    {

        PlayerPrefs.SetInt("lvStar_" + level.ToString(), star);

    }

    public static int getLevelStar(int level)
    {
        return PlayerPrefs.GetInt("lvStar_" + level.ToString());
    }

    public static void ResetAll()
    {
        for (int i = 0; i < 20; i++)
        {
            PlayerPrefs.SetInt("lvStar_" + (i + 1).ToString(), 0);
        }
    }
}
