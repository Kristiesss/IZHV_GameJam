using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterBrowser : MonoBehaviour
{
    public List<CharacterData> allCharacters;
    private List<CharacterData> availableCharacters = new List<CharacterData>();
    private int currentIndex = 0;

    [Header("UI Main Panels")]
    public GameObject selectionPanel; 
    public GameObject popUpResultPanel; 
    public GameObject failPanel;
    public GameObject finalSummaryPanel;

    [Header("Display References")]
    public Image pfpDisplay;
    public TextMeshProUGUI characterDescriptionText; 
    public TextMeshProUGUI friendRequirementsText; 
    public TextMeshProUGUI popUpText; 
    public TextMeshProUGUI finalSummaryText; 
    public Button nextOrFinishButton; 
    public TextMeshProUGUI finalAverageNumberText; 

    [Header("Friend Visuals")]
    public Image currentFriendPfpDisplay; 
    public TextMeshProUGUI currentFriendNameText; 
    public Sprite pfpElvie;
    public Sprite pfpCass;
    public Sprite pfpMillie;

    [Header("Friend Requirements Strings")]
    [TextArea] public string reqElvie;
    [TextArea] public string reqCass;
    [TextArea] public string reqMillie;

    private int currentFriendTurn = 1; 
    private List<int> selectedScores = new List<int>();

    void Start()
    {
        foreach (var c in allCharacters) if (c != null) c.isSelected = false;
        ResetAvailableCharacters();
        UpdateDisplay();
    }

    void Update()
    {
        if (selectionPanel.activeInHierarchy && !popUpResultPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) NextProfile();
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) PreviousProfile();
            if (Input.GetKeyDown(KeyCode.Return)) SelectCharacter();
        }
    }

    void ResetAvailableCharacters()
    {
        availableCharacters.Clear();
        foreach (var c in allCharacters)
        {
            if (c != null && !c.isSelected) availableCharacters.Add(c);
        }
    }

    public void NextProfile() 
    { 
        if (availableCharacters.Count == 0) return;
        currentIndex = (currentIndex + 1) % availableCharacters.Count;
        UpdateDisplay();
    }

    public void PreviousProfile() 
    { 
        if (availableCharacters.Count == 0) return;
        currentIndex--;
        if (currentIndex < 0) currentIndex = availableCharacters.Count - 1;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (availableCharacters.Count == 0) return;
        CharacterData c = availableCharacters[currentIndex];
        
        pfpDisplay.sprite = c.pfp;
        characterDescriptionText.text = $"<b>Name:</b> {c.charName}\n<b>Age:</b> {c.age}\n<b>Hobbies:</b> {c.hobby1}, {c.hobby2}, {c.hobby3}\n<b>Pet:</b> {c.pet}\n<b>Favorite media to consume:</b> {c.favMedia}";

        if (currentFriendTurn == 1)
        {
            friendRequirementsText.text = reqElvie;
            currentFriendNameText.text = "Elvie";
            currentFriendPfpDisplay.sprite = pfpElvie;
        }
        else if (currentFriendTurn == 2)
        {
            friendRequirementsText.text = reqCass;
            currentFriendNameText.text = "Cass";
            currentFriendPfpDisplay.sprite = pfpCass;
        }
        else
        {
            friendRequirementsText.text = reqMillie;
            currentFriendNameText.text = "Millie";
            currentFriendPfpDisplay.sprite = pfpMillie;
        }
    }

    public void SelectCharacter()
    {
        if (availableCharacters.Count == 0 || popUpResultPanel.activeInHierarchy) return;

        CharacterData selected = availableCharacters[currentIndex];
        int score = (currentFriendTurn == 1) ? selected.compatibilityFriend1 : 
                    (currentFriendTurn == 2) ? selected.compatibilityFriend2 : 
                    selected.compatibilityFriend3;

        if (score <= 0)
        {
            selectionPanel.SetActive(false);
            failPanel.SetActive(true);
            return;
        }

        selectedScores.Add(score);
        selected.isSelected = true;

        string fName = (currentFriendTurn == 1) ? "Elvie" : (currentFriendTurn == 2) ? "Cass" : "Millie";
        popUpText.text = $"Compatibility of your pick for {fName} is {score}%";
        nextOrFinishButton.GetComponentInChildren<TextMeshProUGUI>().text = (currentFriendTurn < 3) ? "Select for next friend" : "Finish";
        
        popUpResultPanel.SetActive(true);
    }

    public void OnPopUpButtonClick()
    {
        if (currentFriendTurn < 3)
        {
            currentFriendTurn++;
            popUpResultPanel.SetActive(false);
            PrepareForNextFriend();
        }
        else
        {
            ShowFinalSummary();
        }
    }

    void ShowFinalSummary()
    {
        popUpResultPanel.SetActive(false);
        selectionPanel.SetActive(false);
        
        float average = 0;
        foreach (int s in selectedScores) average += s;
        average /= 3f;

        if (finalAverageNumberText != null)
        {
            finalAverageNumberText.text = Mathf.RoundToInt(average).ToString() + "%";
        }

        if (average == 100) finalSummaryText.text = "Wow you should become a professional match maker with these stats!";
        else if (average > 50) finalSummaryText.text = "You seem to know your friends well. Good job!";
        else finalSummaryText.text = "The average speaks for itself. Better luck next time...";

        finalSummaryPanel.SetActive(true);
    }

    public void PrepareForNextFriend()
    {
        ResetAvailableCharacters();
        currentIndex = 0;
        UpdateDisplay();
    }

}