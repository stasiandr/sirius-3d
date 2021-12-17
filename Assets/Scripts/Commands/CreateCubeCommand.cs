using System;
using MeshTools;
using SceneProvider;

namespace Commands
{
    public class CreateCubeCommand : ICommand
    {
        public void Apply()
        {
            var mesh = MeshGenerator.GenerateCube();
            SceneData.CreateMesh(mesh);
        }

        public void Revert()
        {
            throw new NotImplementedException();
        }
    }
}