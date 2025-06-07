using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinQuiz : BasicPopup
{
    public override void SetPopup()
    {
    }

    public override void ResetPopup()
    {
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
}
