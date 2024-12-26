using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LearnAnimals : MonoBehaviour
{
    public string imageUrl;
    public List<float> startPositionsX;
    public List<float> startPositionsY;
    public GameObject panelControl;
    public Games games;
    public int horizontalNum = 0;
    public int verticalNum = 0;
    public int levelNum;

    // Start is called before the first frame update
    void Start()
    {
        games = panelControl.GetComponent<PanelControl>().gamePanels;
        StartPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Settings
    public void StartPositions()
    {
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            startPositionsX.Add(0);
        }
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            startPositionsX[i] = games.games[0].levels[i].transform.position.x;
        }
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            startPositionsY.Add(0);
        }
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            startPositionsY[i] = games.games[i].levels[0].transform.position.y;
        }
    }

    public void ToRight()
    {
        horizontalNum++;
        for (int i = 0; i < games.games.Count; i++)
        {
            games.games[i].levels[horizontalNum].transform.DOMoveX(0, 1f);
        }
        if (horizontalNum >= 2)
        {
            for (int i = 0; i < games.games.Count; i++)
            {
                games.games[i].levels[horizontalNum - 2].SetActive(false);
            }
        }


    }

    public void ToLeft()
    {
        for (int i = 0; i < games.games.Count; i++)
        {
            games.games[i].levels[horizontalNum].transform.DOMoveX(startPositionsX[horizontalNum], horizontalNum);
        }
        horizontalNum--;
        if (horizontalNum >= 1)
        {
            for (int i = 0; i < games.games.Count; i++)
            {
                games.games[i].levels[horizontalNum - 1].SetActive(true);
            }
        }

    }

    public void ToDown()
    {
        verticalNum++;
        for (int i = 1; i < games.games.Count; i++)
        {
            games.games[i].levels[horizontalNum].transform.DOMoveY(startPositionsY[i-1], 1f);
        }
    }

    public void ToUp()
    {
        levelNum = 0;
        for (int i = 0; i < games.games.Count; i++)
        {
            games.games[i].levels[horizontalNum].transform.DOMoveY(startPositionsY[i], 1f);
        }
    }

    public void Cancel()
    {
        SceneManager.LoadScene("Home");
    }

    public void OpenLevel()
    {
        levelNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        games.games[levelNum+1].levels[horizontalNum].transform.DOMoveY(0, 1f);
    }

    #endregion

    #region Game
    //Web Request Code
    IEnumerator RequestCoroutine()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Fotoðraf indirilemedi: " + www.error);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(www);

        RawImage rawImage = GetComponent<RawImage>();

        rawImage.texture = texture;
    }

    // LevelNum 2 3 ve 5 için þartlandýrmalar yapýlacak.

    


    #endregion
}
