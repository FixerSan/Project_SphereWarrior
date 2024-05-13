using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScene : UIBase
{
    public override bool Init()
    {
        if (!base.Init()) return false;
        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }

    public abstract void RedrawUI();
}
