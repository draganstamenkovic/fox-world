using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button levelButton;
    [SerializeField] private TextMeshProUGUI levelName;
    
    public Action<int> LevelButtonClicked;

    public void OnLevelButtonClicked()
    {
        LevelButtonClicked?.Invoke(int.Parse(levelName.text));
    }
}
