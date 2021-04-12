using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>状态管理（简单的状态机）</summary>
public class StateManager : MonoBehaviour
{
    /// <summary>当前状态</summary>
    public EnumState state { get; private set; } = EnumState.SelectModel;

    public Action<EnumState> onChangeState = null;

    private static StateManager instance;
    public static StateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("_StateManager").AddComponent<StateManager>();
            }
            return instance;
        }
    }


    /// <summary>切换状态</summary>
    public void ChangeState(EnumState state)
    {
        this.state = state;
        if (onChangeState != null) onChangeState(state);
    }


}
