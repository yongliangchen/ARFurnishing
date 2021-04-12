using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


/// <summary>检查AR支持</summary>
public class CheckARSupport : MonoBehaviour
{
    private UIManager m_UIManager;

    private void Awake()
    {
        m_UIManager = FindObjectOfType<UIManager>();
        StartCoroutine(CheckSupport());
    }

    /// <summary>
    /// 检查设备是否支持AR支持
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckSupport()
    {
        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            Debug.Log("当前设备支持AR，但是AR支持需要安装其他软件!");
            yield return ARSession.Install();
        }
        if (ARSession.state == ARSessionState.Ready)
        {
            Debug.Log("AR已支持并准备就绪!");
        }
        else
        {
            switch (ARSession.state)
            {
                case ARSessionState.Unsupported:

                    m_UIManager.ShowNotice("当前设备不支持AR功能！");
                    Debug.LogWarning(GetType() + "/CheckSupport()/当前设备不支持AR功能！");

                    break;
                case ARSessionState.NeedsInstall:

                    Debug.Log("当前设备支持AR，但是AR支持需要安装其他软件！");
                    break;

            }
        }
    }

}
