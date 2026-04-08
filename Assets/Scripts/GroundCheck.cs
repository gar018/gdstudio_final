using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded => Physics.Raycast(transform.position, Vector3.down, .15f);
    void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * .15f, isGrounded ? Color.white : Color.red);
    }
}
