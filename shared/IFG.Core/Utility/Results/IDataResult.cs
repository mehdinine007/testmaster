namespace IFG.Core.Utility.Results
{
    public interface IDataResult<out T>:IResult
    {
        T Data { get; }
    }
}
