/*
 * FSMState.cs
 * MonoBehaviour를 상속받지 않기 때문에 유니티에서 자동으로 업데이트 하지않는다.
 * 다른 상태들의 인터페이스를 제공할 추상클래스
 */
abstract public class FSMState<T>
{
    abstract public void EnterState(T monster);

    abstract public void UpdateState(T monster);

    abstract public void ExitState(T monster);
}
