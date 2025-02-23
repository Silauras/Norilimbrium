using System;
using Managers;
using Spells;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatPlayerComponent : MonoBehaviour
{
    public Camera camera;
    public float sphereRadius = 0.1f;
    public float maxRaycastDistance = 10f;
    public GameObject spellPrefab;
    private SpellGUIManager _spellGUIManager;

    private Spell _currentSpell;
    private int _currentSpellInputIndex;
    private bool _holdingFromStart = false;
    private float _pushTimer = 0f;
    private float _releaseTimer = 0f;

    void Start()
    {
        _spellGUIManager = SpellGUIManager.Instance;
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

        if (Input.GetMouseButtonDown(1) && hasHit && _currentSpell is null)
        {
            CreateSpell(spherePosition);
        }
        else if (Input.GetMouseButtonDown(1) && _currentSpell is null)
        {
            _pushTimer = 0f;
            switch (_currentSpell.spellTypeInput[_currentSpellInputIndex])
            {
                case HoldReleaseInput input:
                    switch (input.inputHoldReleaseInputType)
                    {
                        case HoldReleaseInput.HoldReleaseInputType.Release:
                        {
                            if (_releaseTimer < input.durationInSeconds - input.accuracyInSecondsRequired ||
                                _releaseTimer > input.durationInSeconds + input.accuracyInSecondsRequired)
                            {
                                StopSpell();
                                _currentSpell = null;
                            }
                            else
                            {
                                _currentSpellInputIndex++;
                                _spellGUIManager.SetSpellTypeInput(
                                    _currentSpell.spellTypeInput[_currentSpellInputIndex]);
                            }

                            break;
                        }
                        case HoldReleaseInput.HoldReleaseInputType.Hold:
                            StopSpell();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
            }
        }

        # region processMouseTimers

        if (Input.GetMouseButton(1))
        {
            _pushTimer += Time.deltaTime;
        }
        else
        {
            _releaseTimer += Time.deltaTime;
        }

        #endregion

        if (_currentSpell is not null)
        {
            ProcessSpell();
        }

        if (Input.GetMouseButtonUp(1) && _currentSpell is not null)
        {
            _releaseTimer = 0f;
            switch (_currentSpell.spellTypeInput[_currentSpellInputIndex])
            {
                case HoldReleaseInput input:
                    switch (input.inputHoldReleaseInputType)
                    {
                        case HoldReleaseInput.HoldReleaseInputType.Hold:
                        {
                            if (_pushTimer < input.durationInSeconds - input.accuracyInSecondsRequired ||
                                _pushTimer > input.durationInSeconds + input.accuracyInSecondsRequired)
                            {
                                StopSpell();
                                _currentSpell = null;
                            }
                            else
                            {
                                _currentSpellInputIndex++;
                                _spellGUIManager.SetSpellTypeInput(
                                    _currentSpell.spellTypeInput[_currentSpellInputIndex]);
                            }
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
            }
        }
    }

    private void CreateSpell(Vector3 spherePosition)
    {
        Vector3 cubePosition = spherePosition + Vector3.up * 0.5f;
        GameObject currentSpellObject = Instantiate(spellPrefab, cubePosition, Quaternion.identity);
        _currentSpell = currentSpellObject.GetComponent<Spell>();
        _currentSpellInputIndex = 0;
        _spellGUIManager.SetSpellTypeInput(_currentSpell.spellTypeInput[_currentSpellInputIndex]);
        _pushTimer = 0;
        _currentSpell.ChangeState(_currentSpellInputIndex);
    }

    private void ProcessSpell()
    {
        switch (_currentSpell.spellTypeInput[_currentSpellInputIndex])
        {
            case HoldReleaseInput input:
                _spellGUIManager.holdTime = input.durationInSeconds;
                switch (input.inputHoldReleaseInputType)
                {
                    case HoldReleaseInput.HoldReleaseInputType.Hold:
                    {
                        _spellGUIManager.currentTime = _pushTimer;
                        if (_pushTimer > input.durationInSeconds + input.accuracyInSecondsRequired)
                        {
                            StopSpell();
                            _currentSpell = null;
                        }

                        break;
                    }
                    case HoldReleaseInput.HoldReleaseInputType.Release:
                    {
                        _spellGUIManager.currentTime = _releaseTimer;
                        if (_releaseTimer > input.durationInSeconds + input.accuracyInSecondsRequired)
                        {

                            StopSpell();
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
        }
    }

    private void StopSpell()
    {
        _currentSpell.FailCasting();
        _currentSpell = null;
        _spellGUIManager.isSpellingNow = false;
        _spellGUIManager.SetSpellTypeInput(null);
        _spellGUIManager.currentTime = 0f;
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