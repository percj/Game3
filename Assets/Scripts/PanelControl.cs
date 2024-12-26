using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour
{
    public RectTransform[][] panelRects;
    public float padding;
    public Games gamePanels;
    public GameObject gameScript;
    int rowCount;
    int columnCount;
    float screenWidth;
    float screenHeight;
    float totalPanelWidth;
    float totalPanelHeight;

    void Start()
    {
        LayoutPanels();
    }

    void Update()
    {

    }

    void LayoutPanels()
    {
        RectTransform[][] panelRects = new RectTransform[gamePanels.games.Count][];
        for (int i = 0; i < panelRects.Length; i++)
        {
            panelRects[i] = new RectTransform[gamePanels.games[i].levels.Count];
        }
        for (int i = 0; i < gamePanels.games.Count; i++)
        {
            for (int j = 0; j < gamePanels.games[i].levels.Count; j++)
            {
                panelRects[i][j] = gamePanels.games[i].levels[j].GetComponent<RectTransform>();
            }
        }

        rowCount = panelRects.Length;
        columnCount = panelRects[0].Length;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        totalPanelWidth = 0;
        totalPanelHeight = 0;

        for (int i = 0; i < rowCount; i++)
        {
            totalPanelHeight += panelRects[i][0].rect.height;
        }

        for (int j = 0; j < columnCount; j++)
        {
            totalPanelWidth += panelRects[0][j].rect.width;
        }

        float totalHorizontalPadding = (columnCount - 1) * padding;
        float totalVerticalPadding = (rowCount - 1) * padding;
        float startX = -screenWidth / 2 + totalHorizontalPadding / 2;
        float startY = screenHeight / 2 - totalVerticalPadding / 2;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                panelRects[i][j].anchoredPosition = new Vector2(startX + panelRects[i][j].rect.width / 2, startY - panelRects[i][j].rect.height / 2);
                startX += panelRects[i][j].rect.width + padding;
            }
            startY -= panelRects[i][0].rect.height + padding;
            startX = -screenWidth / 2 + totalHorizontalPadding / 2;
        }

        gameScript.SetActive(true);

    }
  

}

#region Serializables

[System.Serializable]
public class Games
{
    public List<LevelList> games;
}

[System.Serializable]
public class LevelList
{
    public List<GameObject> levels;
}


[System.Serializable]
public class Stars
{
    public List<StarList> games;
}
[System.Serializable]
public class StarList
{
    public List<Image> stars;
}
#endregion