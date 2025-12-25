using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject nameInputPanel;
    public GameObject groupchatScene;
    public GameObject gamePlayPanel;

    [Header("Inputs")]
    public TMP_InputField nameInputField;
    public string playerName;

    [Header("Chat Settings")]
    public Transform chatContent; 
    public GameObject leftBubblePrefab;  
    public GameObject rightBubblePrefab;

    [FormerlySerializedAs("elviepfp")] [Header("Profile Pictures")]
    public Sprite elviePfp;
    public Sprite cassPfp;
    public Sprite milliePfp;
    public Sprite playerPfp;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip notificationSound;
    
    public void ShowNameInput()
    {
        mainMenuPanel.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    // This function triggers after clicking the "Continue" button
    public void SaveNameAndStartChat()
    {
        playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName)) playerName = "Player"; // Fallback if name is empty
        
        nameInputPanel.SetActive(false);
        groupchatScene.SetActive(true);
        
        // Starts the timed chat sequence
        StartCoroutine(PlayChatSequence());
    }

    IEnumerator PlayChatSequence()
    {
        SpawnMessage("Elvie", "How was the date?", elviePfp, true);
        yield return new WaitForSeconds(2);

        SpawnMessage("Cass", "Don't even get me started...", cassPfp, true);
        yield return new WaitForSeconds(2);

        SpawnMessage("Millie", "Another decade of " + playerName + " being the only one married... ", milliePfp, true);
        yield return new WaitForSeconds(2);

        SpawnMessage("Cass", "Please help us find love " + playerName + " :(", cassPfp, true);
        yield return new WaitForSeconds(2);

        SpawnMessage("Elvie", "Yeah please " + playerName, elviePfp, true);
        yield return new WaitForSeconds(2);

        SpawnMessage(playerName, "Let me see what I can do...", playerPfp, false);
        yield return new WaitForSeconds(2);
        SwitchToGameplay();
    }

    void SpawnMessage(string senderName, string messageText, Sprite profilePic, bool isLeft)
    {
        // Select the correct prefab based on the sender's side
        GameObject prefabToUse = isLeft ? leftBubblePrefab : rightBubblePrefab;
        
        GameObject newMsg = Instantiate(prefabToUse, chatContent);
        newMsg.transform.localScale = Vector3.one;
        newMsg.transform.localPosition = Vector3.zero;
        newMsg.SetActive(true);

        // Set the message text
        newMsg.GetComponentInChildren<TextMeshProUGUI>().text = "<b>" + senderName + "</b>\n" + messageText;
        
        // Set the profile picture
        newMsg.transform.Find("Avatar").GetComponent<Image>().sprite = profilePic;
        if (audioSource != null && notificationSound != null)
        {
            audioSource.PlayOneShot(notificationSound);
        }
    }
    
    public void SkipIntro()
    {
        StopAllCoroutines(); 
        groupchatScene.SetActive(false);
        gamePlayPanel.SetActive(true);
        Debug.Log("Intro skipped by player.");
    }

    void SwitchToGameplay()
    {
        groupchatScene.SetActive(false);
        gamePlayPanel.SetActive(true);
        Debug.Log("Transitioning to Gameplay!");
    }

    public void QuitGame() 
    {
        Debug.Log("Exiting game...");
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}