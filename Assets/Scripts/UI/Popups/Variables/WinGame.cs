using UnityEngine.UI;

public class WinGame : BasicPopup
{

    public Button restart;

    public override void Subscribe()
    {
        base.Subscribe();
        restart.onClick.AddListener(Restart);
    }
    public override void Unsubscribe()
    {
        base.Subscribe();
        restart.onClick.RemoveListener(Restart);
    }
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

    private void Restart()
    {
        base.Hide();
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }
}
