abstract public class FSMState<T>
{
    abstract public void EnterState(T monster);

    abstract public void UpdateState(T monster);

    abstract public void ExitState(T monster);
}
