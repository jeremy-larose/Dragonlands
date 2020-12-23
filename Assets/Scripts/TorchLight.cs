using UnityEngine;
using System;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
 
[RequireComponent(typeof (Light))]
public class TorchLight : MonoBehaviour {
 
    public float minLightIntensity = 1.5F;
    public float maxLightIntensity = 4.0F;
 
    public float accelerateTime = 0.15f;
 
    private float _targetIntensity = 1.0f;
    private float _lastIntensity = 1.0f;
 
    private float _timePassed = 0.0f;
 
    private Light _lt;
    private const double Tolerance = 0.0001;
 
    private void Start() {
        _lt = GetComponent<Light>();
        _lastIntensity = _lt.intensity;
        FixedUpdate();
    }
 
    private void FixedUpdate() {
        _timePassed += Time.deltaTime;
        _lt.intensity = Mathf.Lerp(_lastIntensity, _targetIntensity, _timePassed/accelerateTime);
 
        if (Math.Abs(_lt.intensity - _targetIntensity) < Tolerance) {
            _lastIntensity = _lt.intensity;
            _targetIntensity = Random.Range(minLightIntensity, maxLightIntensity);
            _timePassed = 0.0f;
        }
    }
}