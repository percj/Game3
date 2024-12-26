using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MatchAnimals : MonoBehaviour
{
    public List<Vector2> positionsBlack;
    public List<Vector2> positionsNormal;
    public List<float> startPositionsX;
    public GameObject matchScript;
    public Games games;
    public GameObject completePanel;
    public GameObject txtCompleteName;
    public GameObject panelControl;
    public int levelNum;
    public Levels panels = new Levels();
    public Stars stars = new Stars();
    string avatarName;
   
    void Start()
    {
        games = panelControl.GetComponent<PanelControl>().gamePanels;
        StartPositions();
        LoadAvatar();
    }

  
    void Update()
    {
        if (levelNum > 0)
        {
            IsImagesMatch(panels.levels[levelNum - 1].images, panels.levels[levelNum - 1].blackImages);
            IsGameOver(panels.levels[levelNum - 1].images);
        }
        
    }

    #region UI Functions
    public void ToHome()
    {
        SceneManager.LoadScene("Home");
        completePanel.SetActive(false);
    }
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
    }
    public void OpenLevel()
    {
        positionsBlack.Clear();
        positionsNormal.Clear();
        levelNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        games.games[0].levels[levelNum].transform.DOMoveX(0, 1f);
        StartCoroutine(PositionCoroutine());
        matchScript.SetActive(true);
    }
    public void Cancel()
    {
        for (int i = 0; i < games.games.Count; i++)
        {
            games.games[0].levels[i].transform.DOMoveX(startPositionsX[levelNum], levelNum);
        }
        completePanel.SetActive(false);

    }
    public void Restart()
    {
        positionsBlack.Clear();
        positionsNormal.Clear();
        ClearGame();
        completePanel.SetActive(false);
        matchScript.SetActive(true);
    }
    public void NextLevel()
    {
        positionsBlack.Clear();
        positionsNormal.Clear();
        matchScript.SetActive(true);
        games.games[0].levels[levelNum+1].transform.DOMoveX(0, 1f);
        levelNum++;
        StartCoroutine(PositionCoroutine());
        completePanel.SetActive(false);

    }

    #endregion

    #region Game
    public void IsImagesMatch(List<Image> normalImages, List<Image> blackImages)
    {
        for (int i = 0; i < normalImages.Count; i++)
        {
            for (int j = 0; j < blackImages.Count; j++)
            {
                if (Vector2.Distance(normalImages[i].transform.position, blackImages[j].transform.position) < 1)
                {
                    if (normalImages[i].name == blackImages[j].name)
                    {
                        //TruePanel();
                        normalImages[i].gameObject.SetActive(false);
                        blackImages[j].transform.position = new Vector2(20000,-20000);
                        blackImages[j].gameObject.SetActive(false);
                        
                    }
                    else
                    {
                       // FalsePanel();
                    }
                }
            }
        }
    }
    public void IsGameOver(List<Image> normalImages)
    {
        bool response = true;
        foreach (Image shape in normalImages)
        {
            if (shape.gameObject.activeInHierarchy)
            {
                response = false;            
                break;
            }
        }

        if (response)
        {
            if (panels.levels[levelNum-1].stars >= 2)
            {
                StartCoroutine(CompleteCoroutine());
                panels.levels[levelNum-1].stars = 3;

            }

            else
            {
                panels.levels[levelNum-1].isDone = true;
                StartCoroutine(VoiceCoroutine());
                RandomTransform();

            }
            StartCoroutine(NewLevelCoroutine());

        }


    }
    #endregion

    #region Position Functions
    public void RandomTransform()
    {
        if (positionsNormal.Count != 0)
        {
            List<Vector2> tempPositions = new List<Vector2>(positionsNormal);

            for (int i = 0; i < panels.levels[levelNum-1].images.Count; i++)
            {
                int randomIndex = Random.Range(0, tempPositions.Count);
                panels.levels[levelNum-1].images[i].transform.position = tempPositions[randomIndex];
                tempPositions.RemoveAt(randomIndex);
            }
        }

    }
    public void ClearGame()
    {
        BackPositions();
        for (int i = 0; i < panels.levels[levelNum-1].images.Count; i++)
        {
            panels.levels[levelNum-1].images[i].gameObject.SetActive(true);


        }
        for (int i = 0; i < panels.levels[levelNum].blackImages.Count; i++)
        {
            panels.levels[levelNum-1].blackImages[i].gameObject.SetActive(true);
        }
    }
    public void GetPositions()
    {

        for (int i = 0; i < panels.levels[levelNum-1].images.Count; i++)
        {
            positionsNormal.Add(panels.levels[levelNum-1].images[i].transform.position);

        }
        for (int i = 0; i < panels.levels[levelNum-1].blackImages.Count; i++)
        {

            positionsBlack.Add(panels.levels[levelNum-1].blackImages[i].transform.position);
        }


    }
    public void BackPositions()
    {
        if (positionsNormal.Count != 0 && positionsBlack.Count != 0)
        {
            for (int i = 0; i < positionsNormal.Count; i++)
            {
                panels.levels[levelNum-1].images[i].transform.position = positionsNormal[i];
            }
            for (int i = 0; i < positionsBlack.Count; i++)
            {
                panels.levels[levelNum-1].blackImages[i].transform.position = positionsBlack[i];
            }
        }


    }
    #endregion

    #region Enumators
    IEnumerator NewLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        foreach (AudioSource item in panels.levels[levelNum-1].shapeVoice)
        {
            item.gameObject.SetActive(false);
        }
        if (positionsBlack.Count != 0)
        {
            for (int i = 0; i < panels.levels[levelNum-1].blackImages.Count; i++)
            {
                panels.levels[levelNum-1].blackImages[i].transform.position = positionsBlack[i];
                panels.levels[levelNum-1].blackImages[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < panels.levels[levelNum-1].images.Count; i++)
        {
            panels.levels[levelNum-1].images[i].gameObject.SetActive(true);
        }


    }
    IEnumerator CompleteCoroutine()
    {
        
        yield return new WaitForSeconds(1);
        matchScript.SetActive(false);
        completePanel.SetActive(true);
        
    }
    IEnumerator VoiceCoroutine()
    {

        yield return new WaitForSeconds(2);
        //falseVoice.SetActive(false);


    }
    IEnumerator PositionCoroutine()
    {
        yield return new WaitForSeconds(1);
        GetPositions();
    }

    #endregion

    #region Load PlayerPrefs 
    public void LoadAvatar()
    {
        avatarName = PlayerPrefs.GetString("Name");
        txtCompleteName.GetComponent<TextMeshProUGUI>().text = avatarName;
    }

    #endregion
}

#region Serializables 

[System.Serializable]
public class Levels
{
    public List<Images> levels;
}
[System.Serializable]
public class Images
{
    public AudioSource levelVoice;
    public List<Image> images;
    public List<Image> blackImages;
    public List<GameObject> fixedImages;
    public List<AudioSource> shapeVoice;
    public int stars;
    public bool isDone;
}

#endregion
