using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DaDFindRelevant : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Image image;
    public Levels panels = new Levels();
    public int levelNum;
    public float maxDragSpeed = 5f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startPosition = transform.position;
    }
    void Update()
    {
      
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta * maxDragSpeed * Time.deltaTime;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
    }

}
