public class ActionExecution
{
    public GameAction Action { get; private set; }
    public Entity Executor { get; private set; }
    public Entity Target { get; private set; }

    public ActionExecution(GameAction action, Entity executor, Entity target)
    {
        Action = action;
        Executor = executor;
        Target = target;
    }
}