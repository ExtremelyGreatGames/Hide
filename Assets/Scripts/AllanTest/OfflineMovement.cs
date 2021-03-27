using UnityEngine;

namespace AllanTest
{
    public class OfflineMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;

        void FixedUpdate()
        {
            float hori = Input.GetAxisRaw("Horizontal");
            float verti = Input.GetAxisRaw("Vertical");

            Vector2 moveVals = new Vector2(hori,verti);
            if (hori != 0 || verti != 0) transform.Translate(moveVals.normalized * Time.deltaTime * moveSpeed);
        }
    }
}
