/*
 * This should be a last resort when there is no better way to solve the problem.
*/
namespace DeadManSwitch
{
    public enum ActionDirection
    {
        Incoming,
        Outgoing,
    }

    public enum ActionType
    {
        None,
        EmailMessage,
        TextMessage,
    }

    public enum RecurrenceInterval
    {
        None,
        Daily
    }
}
