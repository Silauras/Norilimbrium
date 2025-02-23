using System;
using Spells;
using UnityEngine;

public class DirtWallSpell : Spell
{
    public float maxSize = 1.25f;
    public float minSize = 1f;
    public float startSize = 0.85f;

    public float growthSpeed = 2f;
    public float shrinkSpeed = 0.5f;
    public float trembleIntensity = 0.05f;
    public float trembleSpeed = 10f;

    private bool _needToBeDestroyed = false;

    public enum State
    {
        Growing,
        Settling,
        Trembling,
        Idle
    }

    public State currentState = State.Idle;

    private float elapsedTime = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        transform.localScale = Vector3.one * startSize;
        originalPosition = transform.position;
    }

    // may be used instead of update
    public override void ChangeState(int inputIndex)
    {
        switch (inputIndex)
        {
            case 0:
                StartGrowth();
                break;
            case 1:
                StartSettling();
                break;
            case 2:
                StartTrembling();
                break;

            default:
                currentState = State.Idle;
                break;
        }
    }

    public override void FailCasting()
    {
        _needToBeDestroyed = true;
        Debug.Log("Spell cast was failed");
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Growing:
                Grow();
                break;
            case State.Settling:
                Settle();
                break;
            case State.Trembling:
                Tremble();
                break;
        }

        if (_needToBeDestroyed)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void StartGrowth()
    {
        currentState = State.Growing;
        elapsedTime = 0f;
    }

    private void StartSettling()
    {
        currentState = State.Settling;
    }

    private void StartTrembling()
    {
        currentState = State.Trembling;
        elapsedTime = 0f;
    }

    private void Grow()
    {
        if (transform.localScale.x < maxSize)
        {
            transform.localScale += Vector3.one * (growthSpeed * Time.deltaTime);
            transform.position += Vector3.up * (growthSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            transform.position = originalPosition + new Vector3(
                Mathf.Sin(elapsedTime * trembleSpeed) * trembleIntensity,
                0,
                Mathf.Cos(elapsedTime * trembleSpeed) * trembleIntensity
            );
        }
        else
        {
            currentState = State.Idle;
        }
    }

    private void Settle()
    {
        if (transform.localScale.x > minSize)
        {
            transform.localScale -= Vector3.one * (shrinkSpeed * Time.deltaTime);
        }
        else
        {
            currentState = State.Idle;
        }
    }

    private void Tremble()
    {
        elapsedTime += Time.deltaTime;
        transform.position = originalPosition + new Vector3(
            Mathf.Sin(elapsedTime * trembleSpeed) * trembleIntensity,
            0,
            Mathf.Cos(elapsedTime * trembleSpeed) * trembleIntensity
        );
    }
}