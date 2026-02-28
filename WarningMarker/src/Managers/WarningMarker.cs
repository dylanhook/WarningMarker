using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using CameraUtils.Core;

using WarningMarker.Configuration;

namespace WarningMarker
{
    internal class WarningMarker : MonoBehaviour
    {
        private static Mesh _proceduralMesh;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private MaterialPropertyBlock _propertyBlock;
        private static Material _sharedMaterial;

        private Color _baseColor;
        private Vector3 _baseScale;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_propertyBlock != null) return;

            _propertyBlock = new MaterialPropertyBlock();

            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>();

            _meshFilter = gameObject.GetComponent<MeshFilter>();
            if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>();

            if (_proceduralMesh == null)
            {
                _proceduralMesh = CreateProceduralMesh();
            }

            if (_sharedMaterial == null)
            {
                Shader shader = Shader.Find("Sprites/Default");
                if (shader == null) shader = Shader.Find("Unlit/Transparent");
                _sharedMaterial = new Material(shader);
            }

            if (_meshRenderer.sharedMaterial == null)
            {
                _meshRenderer.sharedMaterial = _sharedMaterial;
            }
        }

        private void Update()
        {
            if (_propertyBlock == null) return;

            float time = Time.timeSinceLevelLoad;
            float pulse = (Mathf.Sin(time * 25f) + 1f) * 0.5f;

            float brightness = Mathf.Lerp(1.2f, 3.5f, pulse);
            Color currentColor = _baseColor * brightness;
            currentColor.a = Mathf.Lerp(0.6f, 1.0f, pulse);

            _propertyBlock.SetColor(ColorPropertyId, currentColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock);

            float scaleMult = Mathf.Lerp(0.95f, 1.1f, pulse);
            transform.localScale = _baseScale * scaleMult;
        }

        private static Mesh CreateProceduralMesh()
        {
            var mesh = new Mesh { name = "ProceduralWarningMarker" };

            float z = 0f;

            var vertices = new Vector3[] {
                new Vector3(-0.15f,  0.1f, z),
                new Vector3( 0.15f,  0.1f, z),
                new Vector3( 0.0f,  -0.15f, z),

                new Vector3(-0.25f,  0.4f, z),
                new Vector3( 0.25f,  0.4f, z),
                new Vector3( 0.0f,   0.2f, z),

                new Vector3(-0.2f,  -0.25f, z),
                new Vector3( 0.2f,  -0.25f, z),
                new Vector3( 0.0f,  -0.5f, z),

                new Vector3(-0.45f,  0.15f, z),
                new Vector3(-0.35f,  0.1f, z),
                new Vector3(-0.15f, -0.3f, z),
                new Vector3(-0.25f, -0.25f, z),

                new Vector3( 0.35f,  0.1f, z),
                new Vector3( 0.45f,  0.15f, z),
                new Vector3( 0.25f, -0.25f, z),
                new Vector3( 0.15f, -0.3f, z)
            };

            var triangles = new int[] {
                2, 1, 0,
                5, 4, 3,
                8, 7, 6,
                11, 10, 9,
                11, 9, 12,
                16, 14, 13,
                16, 15, 14
            };

            var uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(
                    (vertices[i].x + 0.45f) / 0.9f,
                    (vertices[i].y + 0.5f) / 0.9f
                );
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            var doubleTriangles = new int[triangles.Length * 2];
            triangles.CopyTo(doubleTriangles, 0);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                doubleTriangles[triangles.Length + i] = triangles[i];
                doubleTriangles[triangles.Length + i + 1] = triangles[i + 2];
                doubleTriangles[triangles.Length + i + 2] = triangles[i + 1];
            }
            mesh.triangles = doubleTriangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        public void Setup(Color color)
        {
            Init();
            _baseColor = color;
            _propertyBlock.SetColor(ColorPropertyId, color);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        [UsedImplicitly]
        public class Pool : MonoMemoryPool<NoteController, WarningMarker>
        {
            [Inject, UsedImplicitly]
            private readonly ColorManager _colorManager = null!;

            private static readonly FieldAccessor<NoteController, NoteMovement>.Accessor NoteMovementAccessor = FieldAccessor<NoteController, NoteMovement>.GetAccessor("_noteMovement");
            private static readonly FieldAccessor<NoteMovement, NoteJump>.Accessor NoteJumpAccessor = FieldAccessor<NoteMovement, NoteJump>.GetAccessor("_jump");
            private static readonly FieldAccessor<NoteJump, IVariableMovementDataProvider>.Accessor MovementProviderAccessor = FieldAccessor<NoteJump, IVariableMovementDataProvider>.GetAccessor("_variableMovementDataProvider");
            private static readonly FieldAccessor<NoteJump, Vector3>.Accessor StartOffsetAccessor = FieldAccessor<NoteJump, Vector3>.GetAccessor("_startOffset");
            private static readonly FieldAccessor<NoteJump, Vector3>.Accessor EndOffsetAccessor = FieldAccessor<NoteJump, Vector3>.GetAccessor("_endOffset");
            private static readonly FieldAccessor<NoteJump, float>.Accessor GravityBaseAccessor = FieldAccessor<NoteJump, float>.GetAccessor("_gravityBase");
            private static readonly FieldAccessor<NoteJump, Quaternion>.Accessor EndRotationAccessor = FieldAccessor<NoteJump, Quaternion>.GetAccessor("_endRotation");

            protected override void Reinitialize(NoteController noteController, WarningMarker item)
            {
                var noteMovement = NoteMovementAccessor(ref noteController);
                var noteJump = NoteJumpAccessor(ref noteMovement);

                var movementProvider = MovementProviderAccessor(ref noteJump);
                var startOffset = StartOffsetAccessor(ref noteJump);
                var endOffset = EndOffsetAccessor(ref noteJump);
                var gravityBase = GravityBaseAccessor(ref noteJump);

                var jumpDuration = movementProvider.jumpDuration;
                var gravity = movementProvider.CalculateCurrentNoteJumpGravity(gravityBase);
                var startPos = movementProvider.moveEndPosition + startOffset;
                var endPos = movementProvider.jumpEndPosition + endOffset;

                var num1 = jumpDuration * 0.5f;
                var startVerticalVelocity = gravity * num1;

                var beatPos = (startPos + endPos) * 0.5f;

                var pos = beatPos;
                pos.x = endPos.x;
                pos.z += PluginConfig.MarkerOffset;
                pos.y = startPos.y + startVerticalVelocity * num1 - gravity * num1 * num1 * 0.5f;

                var size = PluginConfig.MarkerSize;
                item._baseScale = new Vector3(size, size, size);
                item.transform.localScale = item._baseScale;
                var rot = EndRotationAccessor(ref noteJump);
                item.transform.SetPositionAndRotation(pos, rot);

                var color = _colorManager.ColorForType(noteController.noteData.colorType);
                item.Setup(color);
                item.gameObject.SetLayer(PluginConfig.HmdOnly ? VisibilityLayer.HmdOnlyAndReflected : VisibilityLayer.Default);

                if (item._meshFilter != null)
                {
                    item._meshFilter.sharedMesh = _proceduralMesh;
                }
            }
        }
    }
}