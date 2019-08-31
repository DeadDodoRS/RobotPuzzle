using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum UIPanels
{
    MainMenu,
    InGame,
}

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject InputFieldPrefab;
    [SerializeField] private GameObject ScrollBarContent;

    [SerializeField] private GameObject LockedScreen;
    [SerializeField] private Text LevelName;
    [SerializeField] private GameObject[] UIList;
    private UIPanels currentUI;

    //For list of command 
    private List<InputField> InputsList = new List<InputField>();
    private Vector2 startPos;
    private float yDelta = 10;

    private Canvas currentPanel;

    [SerializeField]private Animator compliteLevelAnimation;

    private void Awake()
    {
        startPos = new Vector2(0, 110);

        compliteLevelAnimation.enabled = false;
        ChangeLevel(true);

        currentUI = UIPanels.MainMenu;
        SetUIPanel(currentUI);

        GameController.Instance().LevelCompliteAction += LevelCompleteAction;
        GameController.Instance().GameOverAction += GameOverAction;

    }

    private bool isStart = false;

    public void SetUIPanel(UIPanels currentUI)
    {
        if (!isStart)
            isStart = true;
        else
            AudioManager.Instance().Play(AudioClips.Click);

        for (int i = 0; i < UIList.Length; i++)
        {
            if ((int)currentUI == i)
                UIList[i].SetActive(true);
            else
                UIList[i].SetActive(false);
        }
    }

    public void StartGame()
    {
        SetUIPanel(UIPanels.InGame);
        CameraController.Instance().SetGameMode();
    }

    public void ReturnToMainMenu()
    {
        SetUIPanel(UIPanels.MainMenu);
        CameraController.Instance().SetMenuMode();
    }

    public void ChangeLevel(bool isNextLevel)
    {
        GameController.Instance().StartLevel(isNextLevel);
        
        if(isStart)
            AudioManager.Instance().Play(AudioClips.Click);

        //Check needed lockscreen
        if (!GameController.Instance().CurrentLevelIsUnlocked() && LockedScreen.activeSelf == false)
            LockedScreen.SetActive(true);
        else if (LockedScreen.activeSelf == true)
            LockedScreen.SetActive(false);

        LevelName.text = GameController.Instance().CurrentLevelName;
    }


    public void AddCommand_Click()
    {
        AudioManager.Instance().Play(AudioClips.Click);
        InputField newInput = Instantiate(InputFieldPrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<InputField>();
        newInput.transform.parent = ScrollBarContent.transform;
        newInput.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(startPos.x, startPos.y - (InputFieldPrefab.GetComponent<RectTransform>().sizeDelta.y + yDelta) * InputsList.Count);
        InputsList.Add(newInput);
    }

    public void GameOverAction()
    {
        ClearInput();
    }

    public void LevelCompleteAction()
    {
        if (compliteLevelAnimation.enabled)
            compliteLevelAnimation.enabled = false;

        compliteLevelAnimation.Play("LevelComplete", -1, 0f);
        compliteLevelAnimation.enabled = true;
        compliteLevelAnimation.Play("LevelComplete");

        StartCoroutine(WaitChange());
    }

    IEnumerator WaitChange()
    {
        yield return new WaitForSeconds(1f);
        ChangeLevel(true);
    }

    public void ClearInput()
    {
        foreach (InputField input in InputsList)
        {
            input.gameObject.SetActive(false);
        }

        InputsList.Clear();
    }

    public void Run_Click()
    {
        AudioManager.Instance().Play(AudioClips.Click);
        Queue<CharacterCommand> commandList = new Queue<CharacterCommand>();

        foreach (InputField field in InputsList)
            GameController.Instance().AddNewCommand(field.text);

        GameController.Instance().StartCommands();

    }

}
