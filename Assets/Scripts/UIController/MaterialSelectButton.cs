using SceneProvider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelectButton : MonoBehaviour
{
    public void PickMaterial(Material mat)
    {
        SceneData.CurrentMaterial = mat;
    }
}
