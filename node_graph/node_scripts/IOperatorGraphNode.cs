namespace NodeSFX
{
    public interface IOperatorGraphNode
    {
        public object[] Execute(object[] args);
    }
}