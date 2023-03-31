using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModel
{
	public delegate void MoveEventHandler(object sender, DirectionEnum direction);

    public delegate void ViewChangerEventHandler(object sender, ViewEnum view);

    public delegate void EventHandler<ViewChangedEventArgs>(object sender, ViewEnum view);

    public delegate void ViewEventHandler(object sender, ViewEnum view);
}
