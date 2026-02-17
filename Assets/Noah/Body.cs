using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Player))]
public class Body : MonoBehaviour
{
    [SerializeField]
    Vector3Int size = Vector3Int.one;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    BodyPart defaultPrefab,
        centralPrefab;

    [Header("Rendering"), Space]
    [SerializeField]
    Material slotMaterial;
    [SerializeField]
    Mesh slotMesh;

    [SerializeField]
    UnityEvent onBodyPartPlacedEvent;

    Player player;
    Dictionary<Vector3Int, BodyPart> bodyParts = new();
    List<Slot> availableSlots = new();

    public int Count => bodyParts.Count;

    public BodyPart CentralPart { get; private set; }
    public Player Player => player;
    public Vector3Int Center => new((int)(size.x * 0.5f), (int)(size.y * 0.5f), (int)(size.z * 0.5f));

    static readonly Dictionary<Vector3Int, Direction> dirToOrientation = new()
    {
         { Vector3Int.right, Direction.Right },
         { Vector3Int.left, Direction.Left },
         { Vector3Int.up, Direction.Up },
         { Vector3Int.down, Direction.Down },
         { Vector3Int.forward, Direction.Forward },
         { Vector3Int.back, Direction.Back },
    };

    private void Awake()
    {
        player = GetComponent<Player>();
        CentralPart = AddPart(Center, centralPrefab);
    }

    public IEnumerable<T> GetPartType<T>() where T : BodyPart
    {
        T currentPart;

        foreach(var part in bodyParts.Values)
            if ((currentPart = part as T) != null)
                yield return currentPart;
    }

    public BodyPart AddPart(Vector3Int coords, BodyPart prefab, Direction direction = Direction.Forward, WeaponDefinition weapon = null)
    {
        if (bodyParts.ContainsKey(coords) || prefab == null)
            return null;

        BodyPart bodyPart = Instantiate(prefab, transform);
        bodyPart.transform.localPosition = CoordsToWorld(coords);
        bodyPart.Initialize(this, weapon, coords, direction);

        bodyParts[coords] = bodyPart;


        // Milton added this, remove and let me know if this breaks something
        if (player?.Build != null)
        {
            // Assign this new BodyPart to any matching WeaponInstances
            foreach (var w in player.Build.weapons)
            {
                if (w.definition.weaponPrefab == prefab && w.weaponObject == null)
                {
                    w.weaponObject = bodyPart;
                    Debug.Log($"[Body] Assigned {bodyPart.name} to {w.definition.weaponName}");
                }
            }
        }

        UpdateAvailableSlots();

        return bodyPart;
    }

    public bool IsValid(Vector3Int coords) =>
        coords.x > -1 && coords.x < size.x && coords.y > -1 && coords.y < size.y && coords.z > -1 && coords.z < size.z;

    public IEnumerable<Slot> GetValidSlots(BodyPart bodyPart)
    {
        foreach (Slot slot in availableSlots)
            if ((slot.directions & bodyPart.AttachTo) != 0)
                yield return slot;
    }

    public bool EnterEditMode(Action<BodyPart> returnCallback, WeaponDefinition weapon)
    {
        BodyPart prefab = weapon?.weaponPrefab ?? defaultPrefab;
        Slot[] validSlots = availableSlots.Where((slot) => (slot.directions & prefab.AttachTo) != 0).ToArray();

        if (validSlots.Length > 0)
        {
            StartCoroutine(EditModeCR(returnCallback, weapon, prefab, validSlots));
            return true;
        }

        return false;
    }

    IEnumerator EditModeCR(Action<BodyPart> returnCallback, WeaponDefinition weapon, BodyPart prefab, Slot[] slots)
    {
        Slot targetSlot;
        Direction targetDirection;
        Touch touch;

        while (true) // EWWWWWWWW
        {
            #region RENDER_SLOTS
            Matrix4x4[] slotMatrices = new Matrix4x4[slots.Length];
            RenderParams rp = new RenderParams(slotMaterial)
            {
                worldBounds = new Bounds(Vector3.zero, Vector3.one * 1000),
                shadowCastingMode = ShadowCastingMode.Off
            };

            for (int i = 0; i < slotMatrices.Length; i++)
                slotMatrices[i] = Matrix4x4.TRS(transform.TransformPoint(CoordsToWorld(slots[i].coords)), transform.rotation, Vector3.one * 0.25f);

            // Render slots in batches of 1023
            for (int i = 0; i < slotMatrices.Length; i += 1023)
            {
                int count = Mathf.Min(1023, slotMatrices.Length - i);
                Matrix4x4[] batch = new Matrix4x4[count];
                Array.Copy(slotMatrices, i, batch, 0, count);
                Graphics.RenderMeshInstanced(rp, slotMesh, 0, batch);
            }
            #endregion

            if (Touch.activeTouches.Count > 0)
            {
                touch = Touch.activeTouches[0]; // First touch

                Vector2 touchPosition = touch.screenPosition;

                // Check touch phase
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    if (TryGetSlotAndOrientation(Camera.main, slots, out Slot slot, out Direction direction, touch.screenPosition))
                    {
                        targetSlot = slot;
                        targetDirection = direction;
                        break;
                    }
                }
            }

            yield return null;
        }

        returnCallback(AddPart(targetSlot.coords, prefab, targetDirection, weapon));
        onBodyPartPlacedEvent.Invoke();
    }

    void UpdateAvailableSlots()
    {
        availableSlots.Clear();
        Dictionary<Vector3Int, Slot> slotMap = new();

        foreach (var kvp in bodyParts)
        {
            Vector3Int partCoords = kvp.Key;
            BodyPart part = kvp.Value;

            foreach (var (dir, orientation) in dirToOrientation)
            {
                Vector3Int slotCoords = partCoords + dir;

                if (!IsValid(slotCoords) || bodyParts.ContainsKey(slotCoords) || (part.AttachFrom & orientation) == 0)
                    continue;

                if (!slotMap.TryGetValue(slotCoords, out Slot slot))
                {
                    slot = new Slot
                    {
                        coords = slotCoords,
                        directions = Direction.None
                    };
                }

                slot.directions |= orientation;
                slotMap[slotCoords] = slot;
            }
        }

        availableSlots.AddRange(slotMap.Values);
    }

    bool TryGetSlotAndOrientation(Camera camera, Slot[] slots, out Slot slot, out Direction orientation, Vector2 cursorPos)
    {
        slot = default;
        orientation = Direction.None;

        if (slots == null || slots.Length == 0)
            return false;

        Ray ray = camera.ScreenPointToRay(cursorPos);

        float closestDistance = float.MaxValue;
        Slot closestSlot = default;

        // Find the slot whose center is closest to the ray
        foreach (var s in slots)
        {
            Vector3 slotWorldPos = transform.TransformPoint(CoordsToWorld(s.coords));

            // Distance from ray to slot center
            Vector3 toSlot = slotWorldPos - ray.origin;
            float projection = Vector3.Dot(toSlot, ray.direction);
            if (projection < 0) continue; // behind camera

            Vector3 closestPoint = ray.origin + ray.direction * projection;
            float distance = Vector3.Distance(closestPoint, slotWorldPos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSlot = s;
            }
        }

        if (closestDistance == float.MaxValue)
            return false;

        slot = closestSlot;

        // Compute click direction relative to slot center in **body-local space**
        Vector3 slotWorldPosFinal = transform.TransformPoint(slot.coords);
        Vector3 clickDirLocal = transform.InverseTransformPoint(ray.origin + ray.direction * closestDistance) - slot.coords;
        clickDirLocal = clickDirLocal.normalized;

        // Pick the cardinal direction with the largest component
        float maxDot = float.NegativeInfinity;
        Direction bestDir = Direction.None;

        foreach (var kvp in dirToOrientation)
        {
            if ((slot.directions & kvp.Value) == 0) continue;

            float dot = Vector3.Dot(kvp.Key, clickDirLocal);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestDir = kvp.Value;
            }
        }

        if (bestDir == Direction.None)
            return false;

        orientation = bestDir;
        return true;
    }

    Vector3 CoordsToWorld(Vector3Int coords) =>
        coords - (Vector3)size * 0.5f + Vector3.one * 0.5f + offset;

    [Serializable]
    public struct Slot
    {
        public Direction directions;
        public Vector3Int coords;

        // Check if a specific orientation is set
        public bool HasOrientation(Direction dir)
        {
            return (directions & dir) == dir;
        }

        // Add an orientation
        public void AddOrientation(Direction dir)
        {
            directions |= dir;
        }

        // Remove an orientation
        public void RemoveOrientation(Direction dir)
        {
            directions &= ~dir;
        }

        // Toggle an orientation
        public void ToggleOrientation(Direction dir)
        {
            directions ^= dir;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(CoordsToWorld(Center), Vector3.one);
    }
#endif
}

[Flags]
public enum Direction
{
    None = 0,
    Forward = 1,
    Right = 2,
    Up = 4,
    Back = 8,
    Left = 16,
    Down = 32
}