using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MemoryLogic : MonoBehaviour
{
    public List<Image> sticks;
    public List<Image> waterAnimals;
    public List<Image> farmAnimals;
    public List<Image> wildAnimals;
    public List<Image> polarAnimals;

    public List<Image> numberDots;
    public List<Vector2> positionsWhite;
    public List<Vector2> positionsColored;
    public List<Vector2> positionsLines;
    public List<float> startPositionsX;
    public List<Vector2> matchPuzzleVectors;
    public List<Image> matchPuzzleImages;
    public GameObject completePanel;
    public Games games;
    public GameObject panelControl;
    public GameObject line;
    public int levelNum;
    public int stickNum = 4;
    public PuzzleLevels panels = new PuzzleLevels();
    public Stars stars = new Stars();

    void Start()
    {
        games = panelControl.GetComponent<PanelControl>().gamePanels;
        StartPositions();
        GetVectors();
        RandomizeImages();
    }

    void Update()
    {
        if (levelNum == 1)
        {
            ClassificateImages();
        }
        else if (levelNum == 2 || levelNum == 3 || levelNum == 4)
        {
            IsImagesMatch(panels.levels[levelNum - 2].images, panels.levels[levelNum - 2].blackImages, panels.levels[levelNum - 2].lines);
            IsGameOver(panels.levels[levelNum - 2].images);
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
        switch (levelNum)
        {     //Arada bulunan levellerden dolayý 
            case 1:
                games.games[0].levels[1].transform.DOMoveX(0, 1f);
                break;
            case 2:
                games.games[0].levels[2].transform.DOMoveX(0, 1f);
                break;
            case 3:
                games.games[0].levels[5].transform.DOMoveX(0, 1f);
                break;
            case 4:
                games.games[0].levels[6].transform.DOMoveX(0, 1f);
                break;
            case 5:
                games.games[0].levels[9].transform.DOMoveX(0, 1f);
                break;
            default:
                break;
        }
        if (levelNum == 2) { StartCoroutine(PositionCoroutine()); }
        if (levelNum == 4) { line.SetActive(true); }
    }
    public void Cancel()
    {
        line.SetActive(false);
        completePanel.SetActive(false);
        for (int i = 0; i < games.games[0].levels.Count; i++)
        {
            games.games[0].levels[i].transform.DOMoveX(startPositionsX[i], i);
        }
    }
    public void ToHome()
    {
        SceneManager.LoadScene("Home");
        completePanel.SetActive(false);
    }
    public void Restart()
    {
        positionsWhite.Clear();
        positionsColored.Clear();
        positionsLines.Clear();
        ClearGame();
        completePanel.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    public void NextLevel()
    {
        positionsWhite.Clear();
        positionsColored.Clear();
        positionsLines.Clear();
        transform.gameObject.SetActive(true);
        games.games[0].levels[levelNum + 1].transform.DOMoveX(0, 1f);
        levelNum++;
        completePanel.SetActive(false);

    }
    #endregion

    #region Game
    public void IsImagesMatch(List<Image> normalImages, List<Image> blackImages, List<Image> lines)
    {
        for (int i = 0; i < normalImages.Count; i++)
        {
            for (int j = 0; j < blackImages.Count; j++)
            {
                if (Vector2.Distance(normalImages[i].transform.position, blackImages[j].transform.position) < 3)
                {
                    if (normalImages[i].name == blackImages[j].name)
                    {
                        //TruePanel();
                        normalImages[i].gameObject.SetActive(false);
                        blackImages[j].transform.position = new Vector2(20000, -20000);
                        blackImages[j].gameObject.SetActive(false);
                        lines[j].gameObject.SetActive(false);


                    }
                    else
                    {
                        // FalsePanel();
                    }
                }
            }
        }
    }
    public void IsGameOver(List<Image> images)
    {

        bool response = true;
        foreach (Image shape in images)
        {
            if (shape.gameObject.activeInHierarchy)
            {
                response = false;
                break;
            }
        }

        if (response)
        {

           panels.levels[levelNum - 2].isDone = true;
            if (levelNum == 4) {
                StartCoroutine(CompleteCoroutine());
            }
            else if (levelNum == 2 || levelNum == 3)
            {
                StartCoroutine(VoiceCoroutine());
                NextLevel();
            }
           //StartCoroutine(NewLevelCoroutine());

        }


    }
    public void RandomizeImages()
    {
        List<Vector2> tempPositions = new List<Vector2>(matchPuzzleVectors);

        for (int i = 0; i < matchPuzzleImages.Count; i++)
        {
            int randomIndex = Random.Range(0, tempPositions.Count);
            matchPuzzleImages[i].transform.position = tempPositions[randomIndex];
            tempPositions.RemoveAt(randomIndex);
        }
    }
    public void DrawImage()
    {

    }
    public void ClassificateImages()
    {
        for (int j = 0; j < waterAnimals.Count; j++)
        {
            if (Vector2.Distance(waterAnimals[j].transform.position,sticks[0].transform.position) < 1)
            {
                waterAnimals[j].gameObject.GetComponent<DaDMemoryLogic>().enabled = false;
            }
        }
        for (int j = 0; j < farmAnimals.Count; j++)
        {
            if (Vector2.Distance(farmAnimals[j].transform.position, sticks[1].transform.position) < 1)
            {
                farmAnimals[j].gameObject.GetComponent<DaDMemoryLogic>().enabled = false;
            }
        }
        for (int j = 0; j < wildAnimals.Count; j++)
        {
            if (Vector2.Distance(wildAnimals[j].transform.position, sticks[2].transform.position) < 1)
            {
                wildAnimals[j].gameObject.GetComponent<DaDMemoryLogic>().enabled = false;
            }
        }
        for (int j = 0; j < polarAnimals.Count; j++)
        {
            if (Vector2.Distance(polarAnimals[j].transform.position, sticks[3].transform.position) < 1)
            {
                polarAnimals[j].gameObject.GetComponent<DaDMemoryLogic>().enabled = false;
            }
        }
        
    }
    #endregion 

    #region Position Functions
    public void GetVectors()
    {
        for (int i = 0; i < matchPuzzleImages.Count; i++)
        {
            matchPuzzleVectors[i] = matchPuzzleImages[i].transform.position;
        }
    }
    public void RandomTransform()
    {
        if (positionsColored.Count != 0)
        {
            List<Vector2> tempPositions = new List<Vector2>(positionsColored);

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
        for (int i = 0; i < panels.levels[levelNum].lines.Count; i++)
        {
            panels.levels[levelNum].lines[i].gameObject.SetActive(true);
        }
    }
    public void GetPositions()
    {

        for (int i = 0; i < panels.levels[levelNum-2].images.Count; i++)
        {
            positionsColored.Add(panels.levels[levelNum - 2].images[i].transform.position);

        }
        for (int i = 0; i < panels.levels[levelNum - 2].blackImages.Count; i++)
        {
            positionsWhite.Add(panels.levels[levelNum - 2].blackImages[i].transform.position);
        }
        for (int i = 0; i < panels.levels[levelNum - 2].lines.Count; i++)
        {
            positionsLines.Add(panels.levels[levelNum - 2].lines[i].transform.position);
        }


    }
    public void BackPositions()
    {
        if (positionsColored.Count != 0 && positionsWhite.Count != 0 && positionsLines.Count != 0)
        {
            for (int i = 0; i < positionsColored.Count; i++)
            {
                panels.levels[levelNum].images[i].transform.position = positionsColored[i];
            }
            for (int i = 0; i < positionsWhite.Count; i++)
            {
                panels.levels[levelNum].blackImages[i].transform.position = positionsWhite[i];
            }
            for (int i = 0; i < positionsLines.Count; i++)
            {
                panels.levels[levelNum].lines[i].transform.position = positionsLines[i];
            }
        }


    }
    #endregion

    #region Enumators
    IEnumerator NewLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        foreach (AudioSource item in panels.levels[levelNum-2].shapeVoice)
        {
            item.gameObject.SetActive(false);
        }
        if (positionsWhite.Count != 0)
        {
            for (int i = 0; i < panels.levels[levelNum-2].blackImages.Count; i++)
            {
                panels.levels[levelNum - 2].blackImages[i].transform.position = positionsWhite[i];
                panels.levels[levelNum - 2].blackImages[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < panels.levels[levelNum - 2].lines.Count; i++)
            {
                panels.levels[levelNum - 2].lines[i].transform.position = positionsLines[i];
                panels.levels[levelNum - 2].lines[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < panels.levels[levelNum - 2].images.Count; i++)
        {
            panels.levels[levelNum - 2].images[i].gameObject.SetActive(true);
        }


    }
    IEnumerator CompleteCoroutine()
    {
        yield return new WaitForSeconds(1);
        transform.gameObject.SetActive(false);
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
}
    #region Serializables 

[System.Serializable]
public class PuzzleLevels
{
    public List<Level> levels;
}
[System.Serializable]
public class Level
{
    public AudioSource levelVoice;
    public List<Image> images;
    public List<Image> blackImages;
    public List<Image> lines;
    public List<AudioSource> shapeVoice;
    public int stars;
    public bool isDone;
}
#endregion