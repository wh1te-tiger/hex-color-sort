namespace Scripts
{
    public class WinWindow : EndGameWindow
    {
        protected override void OnShow()
        {
            base.OnShow();
            SoundService.PlaySound(SoundType.Win, UiAudioSource.Value);
        }
    }
}