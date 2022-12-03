using Core.Events;

namespace UI.EventBinders
{
    public sealed class CappedIntTextDisplayer : TextDisplayer<CappedIntEvent, (int value, int max)>
    {

    }
}
