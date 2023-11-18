using UnityEngine;

public class Checkpoint : MonoBehaviour
{
  private Animator anim;
  public string id;
  public bool activationStatus;

  private void Start()
  {
    anim = GetComponent<Animator>();
  }

  [ContextMenu("Generate checkpoint id")]
  private void GenerateId()
  {
    id = System.Guid.NewGuid().ToString();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Player>() != null)
      ActivateCheckpoint();
  }

  public void ActivateCheckpoint()
  {
    if (!activationStatus)
      AudioManager.instance.PlaySFX(5, transform);

    activationStatus = true;
    Invoke("SetActiveAnim", .1f);
  }

  private void SetActiveAnim() => anim.SetBool("active", true);
}
