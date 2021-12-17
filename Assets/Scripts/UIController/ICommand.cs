namespace UIController
{
    public interface ICommand
    {
        void Apply();
        void Revert();
    }

    public class CreateCubeCommand : ICommand
    {
        void ICommand.Apply()
        {
            MyMesh.MyMesh m = MyMesh.MeshGenerator.GenerateCube();
            SceneProvider.SceneData.CreateMesh(m);
        }
        void ICommand.Revert()
        {

        }
    }
}
