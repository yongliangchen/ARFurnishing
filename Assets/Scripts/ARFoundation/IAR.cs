public interface  IAR
{
    /// <summary>AR初始化完成</summary>
    void OnInitARFinish();
    /// <summary>当设备不支持时候调用</summary>
    void OnUnsupported();
}
