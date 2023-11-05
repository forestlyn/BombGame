/// <summary>
/// 存储操作类型及其操作前后状态
/// </summary>
/// <typeparam name="T">object state</typeparam>
public struct ObjectAction<T>
{
    public int type;
    public T startState;
    public T endState;
}