namespace UIController
{
    public interface ICommand
    {
        void Apply();
        void Revert();
    }
}
