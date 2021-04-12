using UnityEngine;

/// <summary>模型编辑功能</summary>
public class ModelEdit : MonoBehaviour
{
    /// <summary>AR相机</summary>
    private Camera m_Camera;
    /// <summary>是否触摸模型</summary>
    private bool m_IsTouchModel=false;

    private GameObject m_SelectedModel;

    private void Awake()
    {
        m_Camera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        //说明点击在UI上面，
        if (isTouchUI()) return;

        //说明正在放置模型中，不给移动好旋转
        if (StateManager.Instance.state!=EnumState.Main && StateManager.Instance.state!= EnumState.EditModel) return;

        if (Input.GetMouseButtonDown(0))
        {
            bool tempIsTouchModel = isClickModel(Input.mousePosition);

            //说明第一次点击模型
            if (m_IsTouchModel == false && tempIsTouchModel) showEditModeUI();

            m_IsTouchModel = tempIsTouchModel;
        }
    }

    /// <summary>删除选中模型</summary>
    public void DeleteSelectedModel()
    {
        m_IsTouchModel = false;
        if (m_SelectedModel != null) Destroy(m_SelectedModel);
        StateManager.Instance.ChangeState(EnumState.Main);
    }

    /// <summary>编辑完成</summary>
    public void EditComplete()
    {
        m_IsTouchModel = false;
        if (m_SelectedModel != null)
        {
            m_SelectedModel = null;
        }
        StateManager.Instance.ChangeState(EnumState.Main);
    }

    /// <summary>过滤点击层级</summary>
    private LayerMask mask = 1 << 8;
    /// <summary>是否点击模型</summary>
    private bool isClickModel(Vector2 vector2)
    {
        Ray ray = m_Camera.ScreenPointToRay(vector2);
        RaycastHit hitInfo;

        bool isCollider = Physics.Raycast(ray, out hitInfo, 1000, mask);
        if (isCollider)
        {
            m_SelectedModel = hitInfo.transform.gameObject;

            //显示选中特效
        }

        return isCollider;
    }

    /// <summary>判断是否点击在UI上面</summary>
    private bool isTouchUI()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount < 1) return false;
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return true;
        }
        else
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return true;
        }

        return false;
    }

    /// <summary>显示编辑模型UI</summary>
    private void showEditModeUI()
    {
        StateManager.Instance.ChangeState(EnumState.EditModel);
    }

}
