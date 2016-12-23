using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    Animator animMainMenu;
    public GameObject ButtonAmThanh;
    public Sprite btnAudioOn;
    public Sprite btnAudioOff;
    public Sprite Empty;
    public Sprite OneStar;
    public Sprite TwoStar;
    public Sprite ThreeStar;

    void Start()
    {
        BidingLevelStar();
    }

    public void BidingLevelStar()
    {
        if (GameObject.Find("MapLevels"))
        {
            GameObject gLevels = GameObject.Find("MapLevels");
            for (int i = 0; i < 20; i++)
            {
                if (SaveManager.getLevelStar(i+1) == 0)
                {
                    gLevels.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = Empty;
                }
                else if (SaveManager.getLevelStar(i+1) == 1)
                {
                    gLevels.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = OneStar;
                }
                else if (SaveManager.getLevelStar(i+1) == 2)
                {
                    gLevels.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = TwoStar;
                }
                else if (SaveManager.getLevelStar(i+1) == 3)
                {
                    gLevels.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = ThreeStar;
                }
            }
        }
    }

    public void BatDau_Clicked()
    {
        //AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("Scenes/Map_Levels");
    }

    public void CaiDat_Clicked()
    {

    }

    public void Thoat_Clicked()
    {
        Application.Quit();
    }

    public void TroVe_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void AmThanh_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();

        AudioManager.Instance.ChangeAudioState();

        if (AudioManager.Instance.audioEnabled == true)
        {
            ButtonAmThanh.GetComponent<Image>().sprite = btnAudioOn;
        }
        else
        {
            ButtonAmThanh.GetComponent<Image>().sprite = btnAudioOff;
        }
    }

    public void Level_Clicked(string tag)
    {
        try
        {
            SceneManager.LoadScene("Levels/Level " + tag.ToString());
        }
        catch (Exception ex)
        {
            Debug.Log("Error!!!" + ex.ToString());
        }
    }
}
