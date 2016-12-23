using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //Lưu danh sách các node đã duyệt qua
    private List<string> ListVisited;

    //Mã danh sách các lệnh cần thực thi
    //1: MoveUp, 2: MoveDown, 3: MoveLeft, 4: MoveRight, 5: TakeTreasure
    List<int> ListAction;
    private GameObject[] arrMines;

    //Các Panel quản lý
    public GameObject pnEditor;
    public GameObject pnSubFunction;
    public GameObject pnSubFunction2;
    public GameObject pnTemp;
    public GameObject ButtonChay;
    public GameObject ButtonAmThanh;
    public GameObject Star;
    public Text txtCode;
    public Text txtScore;
    public Text txtCodeBlocks;
    public Text txtLevelInfo;
    public Sprite btnRun;
    public Sprite btnRestart;
    public Sprite btnAudioOn;
    public Sprite btnAudioOff;
    public Sprite OneStar;
    public Sprite TwoStar;
    public Sprite ThreeStar;

    //Mã lệnh trong ngôn ngữ C++
    private string CodeInCpp = "";

    //Code hợp lệ hay không?
    private bool isCorrect = true;

    public static bool isDragging = false;

    //Kiểm tra màn chơi có dùng tới Function?
    //private bool isHaveFunction = false;
    //private bool isHave2Function = false;

    private bool isRunning = false;

    //Khởi tạo Animation
    Animator animShowCode;
    Animator animHint;
    Animator animCongratulation;
    Animator animFailed;
    Animator animFunction;
    Animator animFunction2;

    //Class quản lý nhân vật (Marine)
    public MarineManager marine;

    //Vị trí ban đầu của Marine
    private Vector2 originPosition;

    //Mảng các đối tượng môi trường (Coin, Mine)
    private GameObject[] arrCoins;

    //Điểm số tối thiểu để đạt 3 sao
    int MScore;
    int MCodeBlocks;
    int levelName;

    public static int score = 0;
    public static int codeBlockUsed = 0;

    //Chỉ số xếp loại sau mỗi màn chơi
    private int star = 1;

    void Start()
    {
        CheckLevelInfo();
        score = 0;

        ListVisited = new List<string>();
        ListAction = new List<int>();

        animShowCode = GameObject.Find("Panel_ShowCode").GetComponent<Animator>();
        animHint = GameObject.Find("Panel_Hint").GetComponent<Animator>();
        animCongratulation = GameObject.Find("Panel_Congratulation").GetComponent<Animator>();
        animFailed = GameObject.Find("Panel_Failed").GetComponent<Animator>();
        animFunction = GameObject.Find("Panel_SubCodeEditor").GetComponent<Animator>();
        if (GameObject.Find("Panel_SubCodeEditor2"))
        {
            animFunction2 = GameObject.Find("Panel_SubCodeEditor2").GetComponent<Animator>();
        }


        originPosition = marine.GetComponent<RectTransform>().anchoredPosition;

        arrCoins = GameObject.FindGameObjectsWithTag("Coin");
        arrMines = GameObject.FindGameObjectsWithTag("Mine");

        animHint.SetBool("isOpen", true);
    }

    void Update()
    {
        CleanCache();
        CheckStatus();
        ShowCode();
    }

    void CheckLevelInfo()
    {
        string[] s = txtLevelInfo.text.Split('#');
        levelName = int.Parse(s[0]);
        MCodeBlocks = int.Parse(s[1]);
        MScore = int.Parse(s[2]);
    }

    #region Button onClick

    public void Menu_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("Scenes/Map_Levels");
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

    public void Hint_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        animHint.SetBool("isOpen", true);
    }

    public void ShowCode_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        animShowCode.SetBool("isOpen", true);
    }

    public void Run_Clicked()
    {
        ResetAll();
        AudioManager.Instance.PlayButtonClick();

        if (isRunning == false)
        {
            if (isCorrect)
            {
                isRunning = true;
                ButtonChay.GetComponent<Image>().sprite = btnRestart;
                StartCoroutine(RunCodeBlocks());
            }
            else
            {
                animShowCode.SetBool("isOpen", true);
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine("RestartGame");
        }

    }

    IEnumerator RestartGame()
    {
        isRunning = false;
        ButtonChay.GetComponent<Image>().sprite = btnRun;
        yield return new WaitForSeconds(1f);
        ResetAll();
    }

    public void CloseShowCode_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        animShowCode.SetBool("isOpen", false);
    }

    public void CloseHint_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        animHint.SetBool("isOpen", false);
    }

    public void TroVe_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("Scenes/Map_Levels");
    }

    public void ChoiLai_Clicked()
    {
        ResetAll();
        AudioManager.Instance.PlayButtonClick();
        AudioManager.Instance.PlayMusicBackground();
        ButtonChay.GetComponent<Image>().sprite = btnRun;
        isRunning = false;
        animFailed.SetBool("isOpen", false);
    }

    public void ChoiLai2_Clicked()
    {
        ResetAll();
        AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("Levels/Level " + levelName.ToString());
    }

    public void TiepTuc_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();
        score = 0;
        if (levelName < 10)
        {
            SceneManager.LoadScene("Levels/Level " + (levelName + 1).ToString());
        }
    }

    public void ShowHideFunction_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();

        if (animFunction.GetBool("isOpen") == true)
        {
            animFunction.SetBool("isOpen", false);
        }
        else
        {
            if (animFunction2)
            {
                if (animFunction2.GetBool("isOpen") == true)
                {
                    animFunction2.SetBool("isOpen", false);
                }
            }

            animFunction.SetBool("isOpen", true);
        }
    }

    public void ShowHideFunction2_Clicked()
    {
        AudioManager.Instance.PlayButtonClick();

        if (animFunction2.GetBool("isOpen") == true)
        {
            animFunction2.SetBool("isOpen", false);
        }
        else
        {
            if (animFunction.GetBool("isOpen") == true)
            {
                animFunction.SetBool("isOpen", false);
            }
            animFunction2.SetBool("isOpen", true);
        }
    }
    #endregion

    #region Chua dat ten
    public bool isVisited(string name)
    {
        foreach (var u in ListVisited)
        {
            if (u.Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Hàm định khoảng trắng trước từng dòng code
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public string CodeSpace(int level)
    {
        string s = "";
        while (level > 0)
        {
            s += "\t";
            level--;
        }
        return s;
    }

    IEnumerator RunCodeBlocks()
    {
        yield return StartCoroutine(RunCode(1, pnEditor));

        if (marine.IsFinish() == false)
        {
            marine.SetAlive(false);
            marine.SetFinish(true);
        }
    }

    GameObject currentBlock;
    public IEnumerator RunCode(int v, GameObject pn)
    {
        if (pn.transform.childCount > 0)
        {
            for (int i = v; i < pn.transform.childCount; i++)
            {
                if (currentBlock)
                {
                    currentBlock.GetComponent<Image>().color = Color.white;
                }
                GameObject cb = pn.transform.GetChild(i).gameObject;
                cb.GetComponent<Image>().color = Color.gray;
                currentBlock = cb;
                if (cb.tag == "Loop")
                {
                    GameObject pnTemp = cb.transform.GetChild(1).transform.GetChild(1).gameObject;
                    int value = int.Parse(cb.transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text);
                    for (int j = 0; j < value; j++)
                    {
                        yield return StartCoroutine(RunCode(0, pnTemp));
                    }
                }
                else
                {
                    if (pn.transform.GetChild(i).tag == "MoveUp")
                    {
                        yield return StartCoroutine(marine.MoveUp_());
                    }
                    else if (pn.transform.GetChild(i).tag == "MoveDown")
                    {
                        yield return StartCoroutine(marine.MoveDown_());
                    }
                    else if (pn.transform.GetChild(i).tag == "MoveLeft")
                    {
                        yield return StartCoroutine(marine.MoveLeft_());
                    }
                    else if (pn.transform.GetChild(i).tag == "MoveRight")
                    {
                        yield return StartCoroutine(marine.MoveRight_());
                    }
                    else
                    {
                        //
                    }
                } 
            }
        }

    }

    public string FromTagToCode(string tag)
    {
        switch (tag)
        {
            case "MoveUp":
                return "DiLen();\n";
            case "MoveDown":
                return "DiXuong();\n";
            case "MoveLeft":
                return "SangTrai();\n";
            case "MoveRight":
                return "SangPhai();\n";
            default:
                break;
        }
        return "\n";
    }

    /// <summary>
    /// Hàm biên dịch CodeBlocks sang Code C <Xem code></Xem>
    /// </summary>
    /// <param name="v"></param>
    /// <param name="pn"></param>
    public void TranslateCodeToCpp(int v, GameObject pn, int lvCode)
    {
        if (pn.transform.childCount > 0)
        {
            for (int i = 0; i < pn.transform.childCount; i++)
            {
                GameObject cb = pn.transform.GetChild(i).gameObject;
                //if (isVisited(cb.name) == false)
                //{
                if (cb.tag == "Loop")
                {
                    if (cb.transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text != "")
                    {
                        int value = int.Parse(cb.transform.GetChild(0).transform.GetChild(1).GetComponent<InputField>().text);
                        CodeInCpp += CodeSpace(lvCode) + "for (int i = 0; i < " + value + "; i++)\n" + CodeSpace(lvCode) + "{\n";
                        pnTemp = cb.transform.GetChild(1).transform.GetChild(1).gameObject;
                        int k = lvCode + 1;
                        TranslateCodeToCpp(0, pnTemp, k);
                        CodeInCpp += CodeSpace(lvCode) + "}\n";
                        isCorrect = true;
                    }
                    else
                    {
                        CodeInCpp += CodeSpace(lvCode) + "//Lỗi: Vòng lặp không có input!!!\n";
                        isCorrect = false;
                    }
                }
                else
                {
                    ListVisited.Add(pn.transform.GetChild(i).name);
                    CodeInCpp += CodeSpace(lvCode) + FromTagToCode(pn.transform.GetChild(i).tag);
                }
                //}
            }
        }

    }

    public void CountCodeBlockUsed()
    {
        codeBlockUsed = 0;
        CountCodeBlockUsed(pnEditor);
        if (GameObject.FindGameObjectsWithTag("Function").Length > 1)
        {
            CountCodeBlockUsed(pnSubFunction);
        }
        if (GameObject.FindGameObjectsWithTag("Function2").Length > 1)
        {
            CountCodeBlockUsed(pnSubFunction2);
        }
    }

    public void CountCodeBlockUsed(GameObject pn)
    {
        if (pn.transform.childCount > 0)
        {
            for (int i = 0; i < pn.transform.childCount; i++)
            {
                GameObject cb = pn.transform.GetChild(i).gameObject;
                codeBlockUsed++;

                if (cb.tag == "Loop")
                {
                    pnTemp = cb.transform.GetChild(1).transform.GetChild(1).gameObject;
                    CountCodeBlockUsed(pnTemp);
                }
                //else if (cb.tag == "If")
                //{
                //    pnTemp = cb.transform.GetChild(1).transform.GetChild(1).gameObject;
                //    CountCodeBlockUsed(pnTemp);
                //}
            }
        }
    }

    public void ShowCode()
    {
        CodeInCpp += "void main()\n{";
        TranslateCodeToCpp(0, pnEditor, 1);
        CodeInCpp += "}\n";

        if (GameObject.FindGameObjectsWithTag("Function").Length > 1 &&
            GameObject.FindGameObjectsWithTag("Function2").Length > 1)
        {
            CodeInCpp += "void HamCon()\n{\n";
            TranslateCodeToCpp(0, pnSubFunction, 1);
            CodeInCpp += "}";

            CodeInCpp += "\nvoid HamCon2()\n{\n";
            TranslateCodeToCpp(0, pnSubFunction2, 1);
            CodeInCpp += "}";
        }
        else if (GameObject.FindGameObjectsWithTag("Function").Length > 1)
        {
            CodeInCpp += "void HamCon()\n{\n";
            TranslateCodeToCpp(0, pnSubFunction, 1);
            CodeInCpp += "}";
        }
        else if (GameObject.FindGameObjectsWithTag("Function2").Length > 1)
        {
            CodeInCpp += "void HamCon2()\n{\n";
            TranslateCodeToCpp(0, pnSubFunction2, 1);
            CodeInCpp += "}";
        }
        txtCode.text = CodeInCpp;
    }

    /// <summary>
    /// Hàm xếp loại sau khi hoàn thành mỗi màn chơi
    /// </summary>
    public void EvaluateRank()
    {
        CountCodeBlockUsed();

        if (score >= MScore && codeBlockUsed <= MCodeBlocks + 1)
        {
            star = 3;
            Star.GetComponent<Image>().sprite = ThreeStar;
        }
        else if (score >= MScore * 0.7 && codeBlockUsed <= MCodeBlocks + 3)
        {
            star = 2;
            Star.GetComponent<Image>().sprite = TwoStar;
        }
        else
        {
            star = 1;
            Star.GetComponent<Image>().sprite = OneStar;
        }

        txtScore.text = score.ToString();
        txtCodeBlocks.text = (codeBlockUsed-1).ToString();
    }

    /// <summary>
    /// Hàm kiểm tra trạng thái của nhân vật
    ///  - IsFinish: Đã hoàn thành màn chơi
    ///  - IsAlive: Va chạm vật cản
    /// </summary>
    public void CheckStatus()
    {
        if (marine.IsFinish())
        {
            marine.SetFinish(false);
            //marine.GetComponent<Animator>().SetBool("isHide", true);
            StopAllCoroutines();

            if (marine.IsAlive())
            {
                EvaluateRank();
                animCongratulation.SetBool("isOpen", true);
                if (SaveManager.getLevelStar(levelName) < star)
                {
                    SaveManager.setLevelStar(levelName, star);
                }

            }
            else
            {
                StartCoroutine("FailedAudioEffect");

                //marine.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    IEnumerator FailedAudioEffect()
    {
        AudioManager.Instance.PlayExplosion();
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayLevelFailed();
        animFailed.SetBool("isOpen", true);
    }

    public void CleanCache()
    {
        CodeInCpp = "";
        //isHaveFunction = false;
        //isHave2FunctiListVisitedon = false;
        ListVisited.Clear();
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        CodeInCpp = "";
        //isHaveFunction = false;
        //isHave2Function = false;
        ListVisited.Clear();
        ListAction.Clear();
        marine.SetFinish(false);
        marine.SetAlive(true);
        if (currentBlock)
        {
            currentBlock.GetComponent<Image>().color = Color.white;
        }

        marine.GetComponent<RectTransform>().anchoredPosition = originPosition;
        marine.GetComponent<Animator>().SetBool("isHide", false);
        marine.GetComponent<Collider2D>().enabled = true;

        foreach (var coin in arrCoins)
        {
            coin.GetComponent<Animator>().SetBool("isAlive", true);
            coin.GetComponent<Collider2D>().enabled = true;
        }

        foreach (var mine in arrMines)
        {
            mine.GetComponent<Animator>().SetBool("isAlive", true);
            mine.GetComponent<Collider2D>().enabled = true;
        }
        score = 0;
        codeBlockUsed = 0;
    }

    #endregion
}