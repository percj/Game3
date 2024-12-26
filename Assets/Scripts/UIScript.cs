using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class UIScript : MonoBehaviour
{
    #region Describes
    public List<Image> avatars;
    public List<GameObject> gameButtons;
    public Image UIAvatarImage;
    public TMP_InputField nameTextInput;
    public GameObject Home;
    public GameObject AvatarChoice;
    public GameObject GameChoice;
    public GameObject gameMusic;
    public GameObject trLanguage;
    public GameObject enLanguage;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject musicON;
    public GameObject musicOFF;
    public int selectedAvatar;
    public string avatarName;
    public string gameName;
    int screenWidth;
    #endregion

    void Start()
    {
        LoadAvatar();
        screenWidth = Screen.width;
    }
    void Update()
    {

    }


    #region Home Buttons

    public void Play()
    {
        avatarName = nameTextInput.text;
        if (selectedAvatar != 0 && avatarName.Length != 0)
        {
            GameChoice.transform.DOMoveX(0, 1f);
            ShowAvatar();
        }
        else
        {
            AvatarChoicePanelBtn();
        }
        SaveAvatar();
    }

    public void ChangeLanguage()
    {




    }

    public void MusicPlayPause()
    {
        if (gameMusic.activeInHierarchy == true)
        {
            musicOFF.SetActive(true);
            gameMusic.SetActive(false);
        }
        else if (gameMusic.activeInHierarchy == false)
        {
            musicOFF.SetActive(false);
            gameMusic.SetActive(true);
        }
    }

    public void AvatarChoicePanelBtn()
    {
        LoadAvatar();
        if (selectedAvatar == 0)
        {
            selectedAvatar = 5;
        }
        foreach (Image avatar in avatars)
        {
            avatar.gameObject.SetActive(true);
            avatar.color = new Color32(255, 255, 255, 170);

        }
        if (selectedAvatar != 0)
        {
            avatars[selectedAvatar - 1].color = new Color32(255, 255, 255, 255);
            avatars[selectedAvatar - 1].rectTransform.sizeDelta = new Vector2(320, 320);
        }
        AvatarChoice.transform.DOMoveX(0, 1f);
    }

    public void Quit()
    {
        //exitPanel.SetActive(true);

    }

    public void ExitYes()
    {
        Application.Quit();
    }

    public void ExitNo()
    {
        //exitPanel.SetActive(false);
    }

    #endregion

    #region AvatarChoice Buttons

    public void AvatarSelect()
    {
        selectedAvatar = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        SaveAvatar();
        foreach (Image avatar in avatars)
        {
            avatar.color = new Color32(255, 255, 255, 170);
            avatar.rectTransform.sizeDelta = new Vector2(270, 270);
        }
        avatars[selectedAvatar - 1].color = new Color32(255, 255, 255, 255);
        avatars[selectedAvatar - 1].rectTransform.sizeDelta = new Vector2(320, 320);

    }

    public void Cancel()
    {
        GameChoice.transform.DOMoveX(3*screenWidth, 1f);
        AvatarChoice.transform.DOMoveX(3*screenWidth, 1f);
    }

    public void Right()
    {
        foreach (var avatar in avatars)
        {
            avatar.transform.DOMove(new Vector2(avatar.transform.position.x + 370, avatar.transform.position.y), 1);

        }
    }
    public void Left()
    {
        foreach (var avatar in avatars)
        {
            avatar.transform.DOMove(new Vector2(avatar.transform.position.x - 370, avatar.transform.position.y), 1);
        }
    }

    #endregion

    #region Avatar System
    public void ShowAvatar()
    {
        if (selectedAvatar != 0)
        {
            UIAvatarImage.sprite = avatars[selectedAvatar - 1].sprite;
        }
        else
        {
            UIAvatarImage.sprite = avatars[3].sprite;
        }
    }
    public void SaveAvatar()
    {
        PlayerPrefs.SetInt("Avatar", selectedAvatar);
        PlayerPrefs.SetString("Name", avatarName);
        PlayerPrefs.Save();
    }

    public void LoadAvatar()
    {
        selectedAvatar = PlayerPrefs.GetInt("Avatar");
        avatarName = PlayerPrefs.GetString("Name");
        nameTextInput.text = avatarName;
    }
    #endregion

    #region Game Choose Buttons
    public void GotoGameLevels()
    {
        gameName = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(gameName);
    }


    #endregion

}


