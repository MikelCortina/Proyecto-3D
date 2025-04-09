using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class LdController : MonoBehaviour
{
    public TMP_InputField MemberID, PlayerScore;
    public string ID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LootLockerSDKManager.StartGuestSession("Player", (response) =>
        {
            if (response.success) 
            {
                Debug.Log("Succes");
            }
            else 
            {
                Debug.Log("Failed");
            }
        });
    }
    public void SubmitScore()
    {
        LootLockerSDKManager.SubmitScore(MemberID.text, int.Parse(PlayerScore.text), ID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succes");
            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

}
