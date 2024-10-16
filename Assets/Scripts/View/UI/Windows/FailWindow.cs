namespace Scripts
{
    public class FailWindow : EndGameWindow
    {
        protected override void OnShow()
        {
            base.OnShow();
            SoundService.PlaySound(SoundType.Fail, UiAudioSource.Value);
        }
    }
}