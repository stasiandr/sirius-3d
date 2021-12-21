using System.Collections.Generic;
using UnityEngine;

public class RayTracingMaster : MonoBehaviour
{
    public ComputeShader RayTracingShader;

    private RenderTexture _target;

    private Camera _camera;

    public Texture SkyboxTexture;
    public Light DirectionalLight;

    public Vector2 SphereRadius = new Vector2(3.0f, 8.0f);
    public uint SpheresMax = 100;
    public float SpherePlacementRadius = 100.0f;
    
    struct Sphere
    {
        public Vector3 position;
        public float radius;
        public Vector3 albedo;
        public Vector3 specular;
        public float smoothness;
        public Vector3 emission;
    };
    private ComputeBuffer _sphereBuffer;

    struct Triangle
    {
        public Vector3 v0;
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 albedo;
        public Vector3 specular;
        public float smoothness;
        public Vector3 emission;
    };
    private ComputeBuffer _triangleBuffer;

    struct Object
    {
        public Vector3 albedo;
        public Vector3 specular;
        public float smoothness;
        public Vector3 emission;
        public int triangles_start;
        public int triangles_count;
    }
    private ComputeBuffer _ObjectsBuffer;

    private void OnEnable()
    {
        _currentSample = 0;
        SetUpScene2();
    }
    private void OnDisable()
    {
        if (_sphereBuffer != null)
            _sphereBuffer.Release();

        if (_triangleBuffer != null)
            _triangleBuffer.Release();

        if (_ObjectsBuffer != null)
            _ObjectsBuffer.Release();

    }
    public int SphereSeed;
    private void SetUpScene()
    {
        RayTracingShader.SetBool("render_spheres", true);
        Random.InitState(SphereSeed);
        List<Sphere> spheres = new List<Sphere>();
        // Add a number of random spheres
        for (int i = 0; i < SpheresMax; i++)
        {
            Sphere sphere = new Sphere();
            // Radius and radius
            sphere.radius = SphereRadius.x + Random.value * (SphereRadius.y - SphereRadius.x);
            Vector2 randomPos = Random.insideUnitCircle * SpherePlacementRadius;
            sphere.position = new Vector3(randomPos.x, sphere.radius, randomPos.y);
            // Reject spheres that are intersecting others
            foreach (Sphere other in spheres)
            {
                float minDist = sphere.radius + other.radius;
                if (Vector3.SqrMagnitude(sphere.position - other.position) < minDist * minDist)
                    goto SkipSphere;
            }
            // Albedo and specular color
            Color color = Random.ColorHSV();
            bool metal = Random.value < 0.5f;
            bool light_source = Random.value < 0.3f;
            sphere.emission = light_source ? new Vector3(color.r, color.g, color.b) * 2 : Vector3.zero;
            sphere.albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b);
            sphere.specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f;
            sphere.smoothness = metal ? 1 : 0;
            // Add the sphere to the list
            spheres.Add(sphere);
        SkipSphere:
            continue;
        }
        // Assign to compute buffer
        _sphereBuffer = new ComputeBuffer(spheres.Count, 56);
        _sphereBuffer.SetData(spheres);
        List<Triangle> triangles = new List<Triangle>();
        var T = new Triangle();
        T.v0 = new Vector3(0, 0, 50);
        T.v1 = new Vector3(50, 100, 50);
        T.v2 = new Vector3(100, 0, 50);
        T.albedo = new Vector3(0.6f, 0.5f, 0.2f);
        T.specular = new Vector3(0.09f, 0.05f, 0.04f);
        T.emission = new Vector3(1f, 1f, 1f);
        triangles.Add(T);
        _triangleBuffer = new ComputeBuffer(triangles.Count, 76);
        _triangleBuffer.SetData(triangles);
    }
    private void SetUpScene2()
    {
        RayTracingShader.SetBool("render_spheres", false);
        Random.InitState(SphereSeed);
        List<Sphere> spheres = new List<Sphere>();
        spheres.Add(new Sphere());
        _sphereBuffer = new ComputeBuffer(spheres.Count, 56);
        _sphereBuffer.SetData(spheres);
        List<Triangle> triangles = new List<Triangle>();
        List<Object> Objects = new List<Object>();
        foreach (var key in SceneProvider.SceneData.ObjectsByID.Keys)
        {
            var obj = SceneProvider.SceneData.ObjectsByID[key];
            Color color = Random.ColorHSV();
            bool metal = Random.value < 0.5f;
            bool light_source = Random.value < 0.3f;
            var obj_struct = new Object()
            {
                albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b),
                specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f,
                smoothness = metal ? 1 : 0,
                triangles_start = triangles.Count
            };
            var meshF = obj.GetComponent<MeshFilter>();
            var mesh = meshF.mesh;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Triangle T = new Triangle
                {
                    v0 = obj.transform.TransformPoint(mesh.vertices[mesh.triangles[i]]),
                    v1 = obj.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]),
                    v2 = obj.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]),
                    albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b),
                    specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f,
                    smoothness = metal ? 1 : 0
                };
                triangles.Add(T);
            }
            obj_struct.triangles_count = mesh.triangles.Length;
            Objects.Add(obj_struct);
        }
        _triangleBuffer = new ComputeBuffer(triangles.Count, 76);
        _triangleBuffer.SetData(triangles);
        _ObjectsBuffer = new ComputeBuffer(Objects.Count, 48);
        _ObjectsBuffer.SetData(Objects);
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(destination);
    }

    private uint _currentSample = 0;
    private void Update()
    {
        if (transform.hasChanged)
        {
            _currentSample = 0;
            transform.hasChanged = false;
        }
    }

        private void SetShaderParameters()
    {
        Vector3 l = DirectionalLight.transform.forward;
        RayTracingShader.SetFloat("_Seed", Random.value);
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));
        RayTracingShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, DirectionalLight.intensity));
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetBuffer(0, "_Spheres", _sphereBuffer);
        RayTracingShader.SetBuffer(0, "_Triangles", _triangleBuffer);
        RayTracingShader.SetBuffer(0, "_Objects", _ObjectsBuffer);
    }

    private RenderTexture _converged;
    private Material _addMaterial;

    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // Blit the result texture to the screen
        if (_addMaterial == null)
            _addMaterial = new Material(Shader.Find("Hidden/AddShader"));
        _addMaterial.SetFloat("_Sample", _currentSample);
        Graphics.Blit(_target, _converged, _addMaterial);
        Graphics.Blit(_converged, destination);
        _currentSample++;
    }

    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            _currentSample = 0;
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();

            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
        if (_converged == null || _converged.width != Screen.width || _converged.height != Screen.height)
        {
            if (_converged != null)
                _converged.Release();
            _converged = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _converged.enableRandomWrite = true;
            _converged.Create();
        }
    }
}