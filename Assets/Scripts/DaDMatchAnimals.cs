using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DaDMatchAnimals : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Image image;
    public Levels panels = new Levels();
    public GameObject gameScript;
    public int levelNum;
    public float maxDragSpeed = 10f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startPosition = transform.position;
        gameScript = GameObject.Find("MatchAnimals");
    }
    void Update()
    {
        if (gameScript != null)
        {
            panels = gameScript.GetComponent<MatchAnimals>().panels;
            levelNum = gameScript.GetComponent<MatchAnimals>().levelNum;
        }   
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta * maxDragSpeed * Time.deltaTime;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;

        if (panels.levels[levelNum-1].isDone == true)
        {
            if (panels.levels[levelNum-1].stars != 2)
            {
                panels.levels[levelNum-1].stars++;
            }

            panels.levels[levelNum-1].isDone = false;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
    }

}
