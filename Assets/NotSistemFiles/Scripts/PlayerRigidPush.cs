using UnityEngine;

public class PlayerRigidPush : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushPower = 5.0f;
    
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody targetBody = collision.rigidbody;

        // Проверяем наличие Rigidbody у цели
        if (targetBody != null && !targetBody.isKinematic)
        {
            // Берем направление движения игрока (его transform.forward)
            Vector3 pushDirection = transform.forward;
            pushDirection.y = 0; // Исключаем подбрасывание вверх

            // Плавно толкаем объект, пока бежим в него
            targetBody.AddForce(pushDirection * pushPower, ForceMode.Force);
        }
    }
}