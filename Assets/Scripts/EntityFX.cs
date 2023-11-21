using Cinemachine;
using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
  private Player player;
  private SpriteRenderer sr;

  [Header("Screen shake")]
  private CinemachineImpulseSource screenShake;
  [SerializeField] private float shakeMultiplier;
  public Vector3 shakeSwordImpact;
  public Vector3 shakeHighDamage;

  [Header("After image fx")]
  [SerializeField] private GameObject afterImagePrefab;
  [SerializeField] private float colorLooseRate;
  [SerializeField] private float afterImageCooldown;
  private float afterImageCooldownTimer;

  [Header("FlashFX")]
  [SerializeField] private float flashDuration;
  [SerializeField] private Material hitMat;
  private Material originalMat;

  [Header("Ailment color")]
  [SerializeField] private Color[] chillColor;
  [SerializeField] private Color[] igniteColor;
  [SerializeField] private Color[] shockColor;

  [Header("Ailment particles")]
  [SerializeField] private ParticleSystem igniteFx;
  [SerializeField] private ParticleSystem chillFx;
  [SerializeField] private ParticleSystem shockFx;

  [Header("Hit fx")]
  [SerializeField] private GameObject hitFx;
  [SerializeField] private GameObject criticalHitFx;

  [Space]
  [SerializeField] private ParticleSystem dustFx;

  private void Start()
  {
    sr = GetComponentInChildren<SpriteRenderer>();
    player = PlayerManager.instance.player;
    screenShake = GetComponent<CinemachineImpulseSource>();
    originalMat = sr.material;
  }

  private void Update()
  {
    afterImageCooldownTimer -= Time.deltaTime;
  }

  public void ScreenShake(Vector3 _shakePower)
  {
    screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
    screenShake.GenerateImpulse();
  }

  public void CreateAfterImage()
  {
    if (afterImageCooldownTimer < 0)
    {
      afterImageCooldownTimer = afterImageCooldown;
      GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
      newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
    }
  }

  public void MakeTransparent(bool _transparent)
  {
    if (_transparent)
      sr.color = Color.clear;
    else
      sr.color = Color.white;
  }

  private IEnumerator FlashFX()
  {
    sr.material = hitMat;
    Color currentColor = sr.color;

    sr.color = Color.white;
    yield return new WaitForSeconds(flashDuration);

    sr.color = currentColor;
    sr.material = originalMat;
  }

  private void RedColorBlink()
  {
    if (sr.color != Color.white)
      sr.color = Color.white;
    else
      sr.color = Color.red;
  }

  private void CancelColorChange()
  {
    CancelInvoke();
    sr.color = Color.white;

    igniteFx.Stop();
    chillFx.Stop();
    shockFx.Stop();
  }
  public void IgniteFxFor(float _seconds)
  {
    igniteFx.Play();

    InvokeRepeating("IgniteColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }

  public void ChillFxFor(float _seconds)
  {
    chillFx.Play();

    InvokeRepeating("ChillColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }


  public void ShockFxFor(float _seconds)
  {
    shockFx.Play();

    InvokeRepeating("ShockColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }

  private void IgniteColorFx()
  {
    if (sr.color != igniteColor[0])
      sr.color = igniteColor[0];
    else
      sr.color = igniteColor[1];
  }

  private void ChillColorFx()
  {
    if (sr.color != chillColor[0])
      sr.color = chillColor[0];
    else
      sr.color = chillColor[1];
  }

  private void ShockColorFx()
  {
    if (sr.color != shockColor[0])
      sr.color = shockColor[0];
    else
      sr.color = shockColor[1];
  }

  public void CreateHitFx(Transform _target, bool _critical)
  {

    float zRotation = Random.Range(-90, 90);
    float xPosition = Random.Range(-.5f, .5f);
    float yPosition = Random.Range(-.5f, .5f);

    Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

    GameObject hitPrefab = hitFx;

    if (_critical)
    {
      hitPrefab = criticalHitFx;

      float yRotation = 0;
      zRotation = Random.Range(-45, 45);

      if (GetComponent<Entity>().facingDir == -1)
        yRotation = 180;

      hitFxRotation = new Vector3(0, yRotation, zRotation);
    }

    GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
    newHitFx.transform.Rotate(hitFxRotation);
    Destroy(newHitFx, .5f);
  }

  public void PlayDustFX()
  {
    if (dustFx != null)
      dustFx.Play();
  }
}
