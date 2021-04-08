using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Dictionary<string, GameObject> m_DicAllModels = new Dictionary<string, GameObject>();
    private Button m_CloseSelectModelsPanelButton;
    private string m_ModelsPath = "Models";
    private GameObject m_SelectModelsPanel;
    private GameObject m_Item;
    private Transform m_Parent;
    private string m_PreviewPath = "Preview";

    private GameObject m_BtnAddModels;

    private ARTapToPlaceObject m_ARTapToPlace;

    private void Awake()
    {
        m_SelectModelsPanel = transform.Find("SelectModelsPanel").gameObject;
        m_Item = m_SelectModelsPanel.transform.Find("ScrollRect/Item").gameObject;
        m_Parent = m_SelectModelsPanel.transform.Find("ScrollRect/Viewport");
        m_SelectModelsPanel.SetActive(true);
        m_Item.SetActive(false);
        m_CloseSelectModelsPanelButton = m_SelectModelsPanel.transform.Find("BtnCloseSelectModelsPanel").GetComponent<Button>();
        m_CloseSelectModelsPanelButton.onClick.AddListener(() =>
        {
            m_SelectModelsPanel.SetActive(false);
            m_BtnAddModels.SetActive(true);
            m_ARTapToPlace.isDisable = true;
            m_ARTapToPlace.placementIndicator.SetActive(false);
        });

        m_BtnAddModels = transform.Find("BtnAddModels").gameObject;
        m_BtnAddModels.SetActive(false);
        m_BtnAddModels.GetComponent<Button>().onClick.AddListener(clickAddModelButton);

        m_ARTapToPlace = FindObjectOfType<ARTapToPlaceObject>();

        loadModels();
        createItem();
    }

    private void loadModels()
    {
        GameObject[] modelsArr = Resources.LoadAll<GameObject>(m_ModelsPath);
        for(int i=0;i< modelsArr.Length;i++)
        {
            if (m_DicAllModels.ContainsKey(modelsArr[i].name)) continue;
            m_DicAllModels.Add(modelsArr[i].name, modelsArr[i]);
        }
    }

    private void createItem()
    {
        Texture2D[] texturesArr = Resources.LoadAll<Texture2D>(m_PreviewPath);

        for (int i = 0; i < texturesArr.Length; i++)
        {
            GameObject item = Instantiate(m_Item, m_Parent);
            item.name = texturesArr[i].name;
            item.SetActive(true);
            item.GetComponent<RawImage>().texture = texturesArr[i];
            Button button = item.GetComponent<Button>();
            if (button == null) button = item.AddComponent<Button>();
            button.onClick.AddListener(() => { selectedModels(item.name); });
        }
    }

    private void clickAddModelButton()
    {
        m_SelectModelsPanel.SetActive(true);
        m_BtnAddModels.SetActive(false);
        m_ARTapToPlace.isDisable = true;
        m_ARTapToPlace.placementIndicator.SetActive(false);
    }

    /// <summary>选中模型</summary>
    private void selectedModels(string modelName)
    {
        GameObject obj = null;
        m_DicAllModels.TryGetValue(modelName, out obj);

        if(obj!=null)
        {
            m_ARTapToPlace.objectToPlace = obj;
            m_ARTapToPlace.isDisable = false;
            m_SelectModelsPanel.SetActive(false);
        }

        m_BtnAddModels.SetActive(true);
    }

}
