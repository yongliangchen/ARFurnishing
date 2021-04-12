using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>UI管理</summary>
public class UIManager : MonoBehaviour
{
    /// <summary>放置物体脚本</summary>
    private ARTapToPlaceObject m_ARTapToPlace;
    /// <summary>模型编辑脚本</summary>
    private ModelEdit m_ModelEdit;

    /// <summary>主页面板</summary>
    private GameObject m_MainPanel;
    /// <summary>选择模型UI面板</summary>
    private GameObject m_SelectModelsPanel;
    /// <summary>编辑模型UI面板</summary>
    private GameObject m_EditModelPanel;

    /// <summary>所有椅子的模型</summary>
    private Dictionary<string, GameObject> m_DicAllModels = new Dictionary<string, GameObject>();

    /// <summary>关闭选择模型面板按钮</summary>
    private Button m_CloseSelectModelsPanelButton;
    /// <summary>所有模型的存放路径</summary>
    private string m_ModelsPath = "Models";
  
    /// <summary>添加模型按钮</summary>
    private GameObject m_BtnAddModels;

    /// <summary>椅子UI预制体</summary>
    private GameObject m_PreviewPrefab;
    /// <summary>椅子UI的父物体</summary>
    private Transform m_PreviewParent;
    /// <summary>预览图存放路径</summary>
    private string m_PreviewPath = "Preview";

    /// <summary>删除选中模型按钮</summary>
    private Button m_DeleteSelectedModelButton;
    /// <summary>编辑完成按钮</summary>
    private Button m_EditCompleteButton;


    private void Awake()
    {
        m_ARTapToPlace = FindObjectOfType<ARTapToPlaceObject>();
        m_ModelEdit = FindObjectOfType<ModelEdit>();

        StateManager.Instance.onChangeState += onChangeState;

        m_MainPanel = transform.Find("MainPanel").gameObject;
        m_SelectModelsPanel = transform.Find("SelectModelsPanel").gameObject;
        m_EditModelPanel= transform.Find("EditModelPanel").gameObject;

        m_PreviewPrefab = m_SelectModelsPanel.transform.Find("ScrollRect/Item").gameObject;
        m_PreviewParent = m_SelectModelsPanel.transform.Find("ScrollRect/Viewport");
        m_PreviewPrefab.SetActive(false);

        m_CloseSelectModelsPanelButton = m_SelectModelsPanel.transform.Find("BtnCloseSelectModelsPanel").GetComponent<Button>();
        m_CloseSelectModelsPanelButton.onClick.AddListener(() => { StateManager.Instance.ChangeState(EnumState.Main);});

        m_BtnAddModels = transform.Find("MainPanel/BtnAddModels").gameObject;
        m_BtnAddModels.GetComponent<Button>().onClick.AddListener(clickAddModelButton);

        m_DeleteSelectedModelButton = m_EditModelPanel.transform.Find("BtnDelete").GetComponent<Button>();
        m_DeleteSelectedModelButton.onClick.AddListener(() => { m_ModelEdit.DeleteSelectedModel(); });
        m_EditCompleteButton = m_EditModelPanel.transform.Find("BtnConfirm").GetComponent<Button>();
        m_EditCompleteButton.onClick.AddListener(() => { m_ModelEdit.EditComplete(); });

        loadModels();
        createPreviewItem();
        StateManager.Instance.ChangeState(EnumState.SelectModel);
    }

    private void OnDestroy()
    {
        StateManager.Instance.onChangeState -= onChangeState;
    }

    /// <summary>加载模型</summary>
    private void loadModels()
    {
        GameObject[] modelsArr = Resources.LoadAll<GameObject>(m_ModelsPath);
        for(int i=0;i< modelsArr.Length;i++)
        {
            if (m_DicAllModels.ContainsKey(modelsArr[i].name)) continue;
            m_DicAllModels.Add(modelsArr[i].name, modelsArr[i]);
        }
    }

    /// <summary>创建预览图UI</summary>
    private void createPreviewItem()
    {
        Texture2D[] texturesArr = Resources.LoadAll<Texture2D>(m_PreviewPath);

        for (int i = 0; i < texturesArr.Length; i++)
        {
            GameObject item = Instantiate(m_PreviewPrefab, m_PreviewParent);
            item.name = texturesArr[i].name;
            item.SetActive(true);
            item.GetComponent<RawImage>().texture = texturesArr[i];
            Button button = item.GetComponent<Button>();
            if (button == null) button = item.AddComponent<Button>();
            button.onClick.AddListener(() => { selectedModels(item.name); });
        }
    }

    /// <summary>点击添加模型按钮事件</summary>
    private void clickAddModelButton()
    {
        StateManager.Instance.ChangeState(EnumState.SelectModel);
    }

    /// <summary>选中模型</summary>
    private void selectedModels(string modelName)
    {
        GameObject obj = null;
        m_DicAllModels.TryGetValue(modelName, out obj);

        if(obj!=null)
        {
            m_ARTapToPlace.objectToPlace = obj;
            StateManager.Instance.ChangeState(EnumState.CreateModel);
        }
    }

    public void onChangeState(EnumState state)
    {
        m_MainPanel.SetActive(state == EnumState.Main || state == EnumState.CreateModel);
        m_SelectModelsPanel.SetActive(state == EnumState.SelectModel);
        m_EditModelPanel.SetActive(state == EnumState.EditModel);
    }

}

