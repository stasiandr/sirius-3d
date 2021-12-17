namespace Commands
{
    public interface ICommand
    {
        void Apply();
        void Revert();
    }
}
