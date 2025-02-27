using Code.Scripts.Characters.InventorySystem.Spell;
using Spells;
using UnityEngine;

public class CombatPlayerComponent : MonoBehaviour
{
    public Camera camera;
    public float sphereRadius = 0.1f;
    public float maxRaycastDistance = 10f;
    public SpellData spellPrefab;
    private SpellCaster _spellCaster;
    void Start()
    {
        _spellCaster = GetComponent<SpellCaster>();
    }

    void Update()
    {
        if (!camera)
        {
            Debug.LogWarning("Camera is not assigned!");
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        Ray ray = camera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        Vector3 spherePosition = Vector3.zero;

        bool hasHit = Physics.Raycast(ray, out hit, maxRaycastDistance);
        if (hasHit)
        {
            spherePosition = hit.point;
        }

        Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.green);

        if (Input.GetMouseButtonDown(1) && hasHit)
        {
            Debug.Log(spellPrefab.name);
            Debug.Log(_spellCaster.ToString());
            Debug.Log(transform.position);
            Debug.Log(spherePosition);
            _spellCaster.CastSpell(spellPrefab, transform.position, spherePosition);
        }
    }



    void OnDrawGizmos()
    {
        if (!camera) return;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        Ray ray = camera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        Vector3 spherePosition;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            spherePosition = hit.point;
        }
        else
        {
            spherePosition = ray.origin + ray.direction * maxRaycastDistance;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);
    }
}