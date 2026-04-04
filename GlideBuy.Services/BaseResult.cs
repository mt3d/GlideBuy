namespace GlideBuy.Services
{
    public abstract class BaseResult
    {
        public bool Success => !Errors.Any();

        public IList<string> Errors { get; set; } = new List<string>();

        public virtual void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
