/// <summary>
/// �洢�������ͼ������ǰ��״̬
/// </summary>
/// <typeparam name="T">object state</typeparam>
public struct ObjectAction<T>
{
    public int type;
    public T startState;
    public T endState;
}