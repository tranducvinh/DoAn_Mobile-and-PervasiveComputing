using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PanelEditorManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //public CodeBlocks.Spot typeOfItem;
    private string namePanel = "Panel_CodeEditor";
    //private string nameMask = "CodeEditorMask";
    private string nameList = "ListCodeBlocks";

    #region Scroll

    /// <summary>
    /// Bắt sự kiện con trỏ chuột Enter Panel
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
            if (this.name.Equals(nameList))
            {
                d.placeHolderParent = this.transform;
            }
        }
    }

    /// <summary>
    /// Bắt sự kiện con trỏ chuột Exit Panel
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if (this.name.Equals(namePanel))
        {
            CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
            if (d != null && d.placeHolderParent == this.transform)
            {
                d.placeHolderParent = d.parentToReturnTo;
                
            }
        }

        
    }

    public void OnDrop(PointerEventData eventData)
    {
        CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();

        if (d != null)
        {
            if (this.name.Equals(nameList))
            {
                d.parentToReturnTo = this.transform;
            }
        }
    }

    #endregion

    #region Simple

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("ENTER: " + this.name);
    //    if (eventData.pointerDrag == null)
    //        return;

    //    CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
    //    if (d != null)
    //    {
    //        d.placeHolderParent = this.transform;
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.Log("EXIT: " + this.name);
    //    if (eventData.pointerDrag == null)
    //        return;

    //    CodeBlocks d = eventData.pointerDrag.GetComponent<CodeBlocks>();
    //    if (d != null && d.placeHolderParent == this.transform)
    //    {
    //        d.placeHolderParent = d.parentToReturnTo;

    //    }

    //}

    //void OnMouseOver()
    //{
    //    Debug.Log("OnMouseOver called.");
    //}

    //public void OnMove(AxisEventData data)
    //{
    //    Debug.Log("OnMove called.");
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    if (eventData.pointerDrag == null)
    //        return;
    //}
    #endregion
}
