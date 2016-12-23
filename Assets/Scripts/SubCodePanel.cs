using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SubCodePanel : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pnDelete;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        
        if (eventData.pointerDrag == null)
            return;

        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        

        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (this.name == "InitBlock")
        //{
        //    Destroy(eventData.pointerDrag);
        //}
        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
}
