using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DaDMemoryLogic : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Image image;
    public PuzzleLevels panels = new PuzzleLevels();
    public GameObject gameScript;
    public int levelNum;
    public float maxDragSpeed = 10f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startPosition = transform.position;
        gameScript = GameObject.Find("MemoryLogic");
    }
    void Update()
    {
        if (gameScript != null)
        {
            panels = gameScript.GetComponent<MemoryLogic>().panels;
            levelNum = gameScript.GetComponent<MemoryLogic>().levelNum;
        }
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
