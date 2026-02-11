using System;
using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(0f, 2.5f)]
    [SerializeField]
    float followSmoothing;

    [SerializeField, Header("Shake Settings"), Space]
    ShakeData[] shakeInfo;

    Transform followTarget;
    Vector3 followOffset;

    private void Awake()
    {
        foreach (var info in shakeInfo)
            info.Initialize();
    }

    private void Start()
    {
        followTarget = GameManager.Instance.Player.transform;
        followOffset = transform.position - followTarget.position;
    }

    private void FixedUpdate()
    {
        if (followTarget != null)
            transform.position = Vector3.Lerp(transform.position,
                followTarget.position + followOffset,
                (1 / followSmoothing) * Time.fixedDeltaTime);
    }

    public void Shake(float amplitudeMult = 1, float frequencyMult = 1, float duration = 0.75f, float freqDamp = 0)
    {
        for (int i = 0; i < shakeInfo.Length; i++)
            StartCoroutine(shakeInfo[i].ShakeCR(duration, freqDamp));
    }

    [Serializable]
    struct ShakeData
    {
        public Transform target;
        public Vector3 positionAmplitude,
            rotationAmplitude;
        public float frequency;

<<<<<<< HEAD
        Vector3 defaultPosition,
=======
       public Vector3 defaultPosition,
>>>>>>> parent of b66c3fd (Merge branch 'main' of https://github.com/TrashyDood/FG25GP2_T12)
            defaultRotation;

        public void Initialize()
        {
            defaultPosition = target.localPosition;
            defaultRotation = target.localEulerAngles;
        }

        public IEnumerator ShakeCR(float duration, float freqDamp)
        {
            if (target != null)
            {
                float time = 0,
                    shakeFactor;

                while (time < duration + Time.deltaTime)
                {
                    shakeFactor = Utils.OscillateDamped(time, duration, frequency, freqDamp);
                    target.SetLocalPositionAndRotation(defaultPosition + positionAmplitude * shakeFactor,
                    Quaternion.Euler(defaultRotation + rotationAmplitude * shakeFactor));
                    time += Time.deltaTime;

                    yield return null;
                }
            }
        }
    }
}
