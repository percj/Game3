using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class FindRelevant : MonoBehaviour
{
    public List<Vector2> positionsMom;
    public List<Vector2> positionsChild; 
    public List<float> startPositionsX;
    public Games games;
    public GameObject panelControl;
    public GameObject completePanel;
    public int levelNum;
    public Levels panels = new Levels();
    public Stars stars = new Stars();

    void Start()
    {
        games = panelControl.GetComponent<PanelControl>().gamePanels;
        StartPositions();
    }

    void Update()
    {
        if (levelNum > 0 && levelNum < 5)
        {
            IsImagesMatch(panels.levels[levelNum - 1].images, panels.levels[levelNum - 1].blackImages, panels.levels[levelNum - 1].fixedImages);
            IsGameOver();
        }
    }

    #region UI Functions
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
        levelNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        if (levelNum == 1)
        {
            games.games[0].levels[levelNum].transform.DOMoveX(0, 1f);
        }
        else
        {
            games.games[0].levels[levelNum+3].transform.DOMoveX(0, 1f);
        }
        
    }
    public void Cancel()
    {
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            games.games[0].levels[i].transform.DOMoveX(startPositionsX[i], levelNum);
        }
    }
    public void ToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void NextLevel()
    {
        positionsMom.Clear();
        positionsChild.Clear();
        transform.gameObject.SetActive(true);
        games.games[0].levels[levelNum + 1].transform.DOMoveX(0, 1f);
        levelNum++;
        completePanel.SetActive(false);

    }
    public void Restart()
    {
        positionsMom.Clear();
        positionsChild.Clear();
        ClearGame();
        completePanel.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    #endregion

    #region Game
    public void IsImagesMatch(List<Image> momImages, List<Image> childImages,List<GameObject> familyImages)
    {
        for (int i = 0; i < momImages.Count; i++)
        {
            for (int j = 0; j < childImages.Count; j++)
            {
                if (Vector2.Distance(momImages[i].transform.position, childImages[j].transform.position) < 3)
                {
                    if (momImages[i].name == childImages[j].name)
                    {
                        //TruePanel();
                        momImages[i].gameObject.SetActive(false);
                        childImages[j].transform.position = new Vector2(20000, -20000);
                        childImages[j].gameObject.SetActive(false);
                        familyImages[i].SetActive(true);
                    }
                    else
                    {
                        // FalsePanel();
                    }
                }
            }
        }
    }
    public void IsGameOver()
    {

        bool response = true;
        foreach (Image shape in panels.levels[levelNum - 1].images)
        {
            if (shape.gameObject.activeInHierarchy)
            {
                response = false;
                break;
            }
        }

        if (response)
        {
            panels.levels[levelNum-1].isDone = true;
            if (levelNum == 4)
            {
                StartCoroutine(CompleteCoroutine());
            }
            else if (levelNum == 1 || levelNum == 2 || levelNum == 3)
            {
                StartCoroutine(VoiceCoroutine());
                NextLevel();
            }
            //StartCoroutine(NewLevelCoroutine());

        }


    }
    #endregion

    #region Position Functions
    public void RandomTransform()
    {
        if (positionsChild.Count != 0)
        {
            List<Vector2> tempPositions = new List<Vector2>(positionsChild);

            for (int i = 0; i < panels.levels[levelNum].images.Count; i++)
            {
                int randomIndex = Random.Range(0, tempPositions.Count);
                panels.levels[levelNum].images[i].transform.position = tempPositions[randomIndex];
                tempPositions.RemoveAt(randomIndex);
            }
        }

    }
    public void ClearGame()
    {
        BackPositions();
        for (int i = 0; i < panels.levels[levelNum].images.Count; i++)
        {
            panels.levels[levelNum].images[i].gameObject.SetActive(true);


        }
        for (int i = 0; i < panels.levels[levelNum].blackImages.Count; i++)
        {
            panels.levels[levelNum].blackImages[i].gameObject.SetActive(true);
        }
    }
    public void GetPositions()
    {

        for (int i = 0; i < panels.levels[levelNum].images.Count; i++)
        {
            positionsChild.Add(panels.levels[levelNum].images[i].transform.position);

        }
        for (int i = 0; i < panels.levels[levelNum].blackImages.Count; i++)
        {

            positionsMom.Add(panels.levels[levelNum].blackImages[i].transform.position);
        }


    }
    public void BackPositions()
    {
        if (positionsChild.Count != 0 && positionsMom.Count != 0)
        {
            for (int i = 0; i < positionsChild.Count; i++)
            {
                panels.levels[levelNum].images[i].transform.position = positionsChild[i];
            }
            for (int i = 0; i < positionsMom.Count; i++)
            {
                panels.levels[levelNum].blackImages[i].transform.position = positionsMom[i];
            }
        }


    }
    #endregion

    #region Enumators
    IEnumerator NewLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        foreach (AudioSource item in panels.levels[levelNum].shapeVoice)
        {
            item.gameObject.SetActive(false);
        }
        if (positionsMom.Count != 0)
        {
            for (int i = 0; i < panels.levels[levelNum].blackImages.Count; i++)
            {
                panels.levels[levelNum].blackImages[i].transform.position = positionsMom[i];
                panels.levels[levelNum].blackImages[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < panels.levels[levelNum].images.Count; i++)
        {
            panels.levels[levelNum].images[i].gameObject.SetActive(true);
        }


    }
    IEnumerator CompleteCoroutine()
    {
        yield return new WaitForSeconds(1);
        //completeVoice.SetActive(true);
        completePanel.SetActive(true);
        //gameMusic.volume = 0.1F;
        transform.gameObject.SetActive(false);
    }
    IEnumerator VoiceCoroutine()
    {

        yield return new WaitForSeconds(2);
        //falseVoice.SetActive(false);


    }

    #endregion
}
