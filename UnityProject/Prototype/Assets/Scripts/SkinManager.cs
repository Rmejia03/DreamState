using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    private GameObject selectedSkinPrefab;

    public GameObject femaleSkin;
    public GameObject maleSkin;

    public void SelectFemaleSkin()
    {
        selectedSkinPrefab = femaleSkin;
        SaveSelectedSkin();
    }

    public void SelectMaleSkin()
    {
        selectedSkinPrefab = maleSkin;
        SaveSelectedSkin();
    }

    private void SaveSelectedSkin()
    {
        if (selectedSkinPrefab != null)
        {
            PrefabUtility.SaveAsPrefabAsset(selectedSkinPrefab, "Assets/selectedSkin.prefab");
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerTestingScene");
    }

}
