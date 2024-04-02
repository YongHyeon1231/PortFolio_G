using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{
    public ObjectType ObjType { get; protected set; }

    void Awake()
    {
        Init();
    }

    bool _init = false;
    public virtual bool Init()
    {
        if (_init)
            return true;

        _init = true;
        return false;
    }

    void Update()
    {
        UpdateController();
    }

    public virtual void UpdateController()
    {

    }
}
