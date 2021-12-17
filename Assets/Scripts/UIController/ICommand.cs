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
            var m = MyMesh.MeshGenerator.GenerateCube();
            //SceneData.CreateMesh(m);
        }
        void ICommand.Revert()
        {

        }
    }
}
