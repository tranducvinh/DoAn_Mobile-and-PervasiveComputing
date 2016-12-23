using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CodeBlocks : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //Lưu vị trí ban đầu của đối tượng trước khi kéo thả
    public Transform parentToReturnTo = null;

    public Transform placeHolderParent = null;

    public GameObject tempZone = null;

    public GameObject pnDelete;

    //Đếm số Block được tạo ra
    private static int countOfCodeBlock = 0;

    //Khoảng trắng xem trước vị trí của đối tượng sau khi kéo thả
    GameObject placeHolder = null;

    /* Các loại khối đối tượng, bao gồm:
    /   - ON_START: Khối khởi tạo ban đầu
    /   - MOVE: Các khối thực hiện chức năng di chuyển
    /   - LOOP: Các khối thực hiện vòng lặp
    *///////
    public enum TypeOfItem { DEFAULT, INIT, ON_START }
    public TypeOfItem typeOfItem;

    void Start()
    {
        pnDelete = GameObject.Find("Panel_Delete");
        placeHolder = GameObject.Find("PlaceHolder");
        tempZone = GameObject.Find("Panel_Temp");
    }

    /// <summary>
    /// Bắt, xử lý sự kiện khi bắt đầu kéo (drag) đối tượng
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayDragDrop();

        //TẠO MỘT BẢN COPY - GameObject
        if (this.typeOfItem == TypeOfItem.INIT)
        {
            GameObject c = null;
            c = Instantiate(this.gameObject) as GameObject;
            c.name = this.name;
            this.name = c.name + " [" + countOfCodeBlock.ToString() + "]";
            countOfCodeBlock++;
            c.transform.SetParent(this.transform.parent);
            c.transform.localScale = new Vector3(1, 1, 1);
            c.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = true;
            pnDelete.GetComponent<CanvasGroup>().alpha = 0;
        }



        ////Tạo khoảng trắng khi Drag
        //placeHolder = new GameObject();
        ////placeHolder.transform.SetParent(this.transform.parent);
        //LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        //le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        //if (this.tag == "If" || this.tag == "IfElse" || this.tag == "Loop")
        //{
        //    le.preferredHeight = 150;
        //}
        //else
        //{
        //    le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        //}
        //le.flexibleWidth = 0;
        //le.flexibleHeight = 0;
        //placeHolder.name = "PlaceHolder";

        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;

        //this.transform.SetParent(this.transform.parent.parent);

        this.transform.SetParent(tempZone.transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;


        //this.transform.GetChild(0).get


        //Hàm tìm tất cả các đối tượng cùng loại
        //DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();

    }

    /// <summary>
    /// Bắt, xử lý sự kiện khi đang trong quá trình kéo (drag) đối tượng
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        GameManager.isDragging = true;
        //this.GetComponent<RectTransform>().anchoredPosition3D
        //Debug.Log("OnDrag: "+placeHolderParent.name);
        if (!placeHolderParent.name.Equals("ListInitBlocks"))
        {
            int newSiblingIndex = placeHolderParent.childCount;
            if (placeHolder.transform.parent != placeHolderParent)
            {
                placeHolder.transform.SetParent(placeHolderParent);
            }

            for (int i = 0; i < placeHolderParent.childCount; i++)
            {
                if (this.transform.position.y > placeHolderParent.GetChild(i).position.y)
                {
                    newSiblingIndex = i;

                    if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    {
                        newSiblingIndex--;
                    }
                    break;
                }
            }

            if (newSiblingIndex != 0)
            {
                placeHolder.transform.SetSiblingIndex(newSiblingIndex + 1);
            }

            //if (placeHolderParent.name.Equals("ListCodeBlocks") && newSiblingIndex == 0)
            //{
            //    placeHolder.transform.SetSiblingIndex(newSiblingIndex+1);
            //}
            //else
            //{
            //    placeHolder.transform.SetSiblingIndex(newSiblingIndex);
            //}

        }
        else
        {
            //placeHolder.transform.SetSiblingIndex(-1);
        }
    }

    /// <summary>
    /// Bắt, xử lý sự kiện khi kết thúc kéo (drag) đối tượng
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.isDragging = false;
        //AudioManager.Instance.PlayDragDrop();

        this.typeOfItem = TypeOfItem.DEFAULT;

        //Block Raycast Panel Delete
        pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = false;
        pnDelete.GetComponent<CanvasGroup>().alpha = 0;

        if (placeHolderParent.name != "Panel_Init")
        {
            this.transform.SetParent(placeHolderParent);
            this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

        placeHolder.transform.SetParent(tempZone.transform);
        placeHolder.transform.position = tempZone.transform.position;
    }
}
