using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputField;
    
    public static MenuManager instance;
    public string playerName;

    private void Awake() {
        if (instance) {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() {
        playerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        # if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        # else
            Application.Quit();
        # endif
    }

    public void Reset() {
        
    }
}
