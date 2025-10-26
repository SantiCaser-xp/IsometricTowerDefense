public class ParamsMemento
{
    public object[] parameters;

    public ParamsMemento(params object[] p)
    {
        parameters = p;
    }
}
