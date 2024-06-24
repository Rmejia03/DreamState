using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinManager : MonoBehaviour
{
    private GameObject selectedSkinPrefab;

    public GameObject femaleSkin;
    public GameObject maleSkin;

    public void SelectFemaleSkin()
    {
        //selectedSkinPrefab = femaleSkin;
        //SaveSelectedSkin();
        PlayerPrefs.SetString("SelectedSkin", "Female");
        PlayerPrefs.Save();
    }

    public void SelectMaleSkin()
    {
        //selectedSkinPrefab = maleSkin;
        //SaveSelectedSkin();
        PlayerPrefs.SetString("SelectedSkin", "Male");
        PlayerPrefs.Save();
    }

    private void SaveSelectedSkin()
    {
        #if UNITY_EDITOR
        if (selectedSkinPrefab != null)
        {
            PrefabUtility.SaveAsPrefabAsset(selectedSkinPrefab, "Assets/selectedSkin.prefab");
        }
        #endif
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerTestingScene");
    }

}
