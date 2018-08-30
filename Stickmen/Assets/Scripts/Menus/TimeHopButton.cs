public class TimeHopButton : ButtonSelector {

    public TimeBarSlider timeBar;
    public bool foward = true;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        timeBar.HopInTime(foward);
    }
}
