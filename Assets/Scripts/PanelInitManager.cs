using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PanelInitManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pnDelete;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            if (d.typeOfItem == CodeBlocks.TypeOfItem.INIT)
            {
                pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = true;
                pnDelete.GetComponent<CanvasGroup>().alpha = 0;
            }
            else
            {
                pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = true;
                pnDelete.GetComponent<CanvasGroup>().alpha = 1;
            }

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = false;
            pnDelete.GetComponent<CanvasGroup>().alpha = 0;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameManager.isDragging = false;
        //if (this.name == "InitBlock")
        //{
        //    Destroy(eventData.pointerDrag);
        //}
        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            AudioManager.Instance.PlayDeleteBlock();
            d.parentToReturnTo = this.transform;
            Destroy(eventData.pointerDrag);
            GameObject.Find("PlaceHolder").transform.SetParent(GameObject.Find("Panel_Temp").transform);
            GameObject.Find("PlaceHolder").transform.position = GameObject.Find("Panel_Temp").transform.position;
            pnDelete.GetComponent<CanvasGroup>().blocksRaycasts = false;
            pnDelete.GetComponent<CanvasGroup>().alpha = 0;
        }

        //Destroy(eventData.pointerDrag);

    }
}
