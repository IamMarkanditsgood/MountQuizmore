
public class LoseQuiz : BasicPopup
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
