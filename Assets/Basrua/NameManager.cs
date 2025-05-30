using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void GuardarNombre()
    {
        string playerName = nameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
            Debug.Log("Nombre guardado: " + playerName);
        }
    }
}
