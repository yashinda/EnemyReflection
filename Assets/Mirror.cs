using UnityEngine;

public class Mirror : MonoBehaviour
{
    [Header("Компоненты")]
    [Tooltip("Стандартный Plane Unity (нормаль — зеленая стрелка Y)")]
    public Transform mirrorPlane;
    [Tooltip("Камера, которая рендерит отражение")]
    public Camera reflectionCamera;
    [Tooltip("Трансформ самого игрока (его позиция в мире)")]
    public Transform playerTransform;
    
    [Header("Настройки кота")]
    [Tooltip("Минимальная дистанция от кота до зеркала (чтобы камера не вылетала вперед)")]
    public float minDistanceToMirror = 0.3f;
    [Tooltip("Смещение точки взгляда по высоте (высота глаз кота)")]
    public float catEyeHeight = 0.2f;

    void LateUpdate()
    {
        if (mirrorPlane == null || reflectionCamera == null || playerTransform == null)
            return;

        // 1. Физическая нормаль плоскости (зеленая стрелка Y)
        Vector3 normal = mirrorPlane.up; 
        Vector3 positionOnPlane = mirrorPlane.position;

        // Математическое уравнение плоскости Ax + By + Cz + D = 0
        float d = -Vector3.Dot(normal, positionOnPlane);
        Vector4 plane = new Vector4(normal.x, normal.y, normal.z, d);

        // 2. Рассчитываем честную матрицу отражения Householder
        Matrix4x4 reflectionMatrix = Matrix4x4.zero;
        CalculateReflectionMatrix(ref reflectionMatrix, plane);

        // --- РАСЧЕТ ПОЗИЦИИ И ВЗГЛЯДА ЗЕРКАЛЬНОЙ КАМЕРЫ ---
        
        // Переносим зеркальную камеру в отраженную физическую точку за зеркалом
        Vector3 playerPos = playerTransform.position;
        float distanceToPlane = Vector3.Dot(normal, playerPos - positionOnPlane);
        
        if (distanceToPlane < minDistanceToMirror)
        {
            playerPos += normal * (minDistanceToMirror - distanceToPlane);
        }
        // Если глаза персонажа выше центра Pivot, можно добавить смещение по высоте:
        // Vector3 playerPos = playerTransform.position + Vector3.up * 1.7f; 
        
        Vector3 reflectedPos = reflectionMatrix.MultiplyPoint(playerPos);
        reflectionCamera.transform.position = reflectedPos;

        // Физический закон: взгляд направлен на ту же область зеркала
        // Заставляем камеру смотреть из своего положения прямо на центр плоскости зеркала
        Vector3 lookTarget = mirrorPlane.position;
        Vector3 targetForward = (lookTarget - reflectedPos).normalized;
        
        //targetForward.y *= 0.2f;
        //targetForward.y = Mathf.Clamp(targetForward.y, -0.3f, 0.4f);

        // В качестве вектора "верх" используем отраженный мировой вектор Vector3.up
        Vector3 reflectedUp = reflectionMatrix.MultiplyVector(Vector3.up);

        // Назначаем честное вращение
        if (targetForward != Vector3.zero)
        {
            // Считаем базовое вращение зеркала
            Quaternion targetRotation = Quaternion.LookRotation(targetForward, reflectedUp);
            Vector3 eulerAngles = targetRotation.eulerAngles;

            // Переводим угол X из диапазона [0, 360] в диапазон [-180, 180]
            float angleX = eulerAngles.x;
            if (angleX > 180f) angleX -= 360f;

            // Ограничиваем угол X: он не должен падать ниже -5 градусов
            if (angleX < -5f)
            {
                angleX = -5f;
            }

            // Возвращаем угол обратно в систему координат Unity [0, 360] если он отрицательный
            if (angleX < 0f) angleX += 360f;

            // Применяем измененный угол обратно в камеру
            eulerAngles.x = angleX;
            reflectionCamera.transform.rotation = Quaternion.Euler(eulerAngles);
        }

        // 3. Обрезаем всё, что находится ЗА плоскостью зеркала
        Vector4 clipPlane = CameraSpacePlane(reflectionCamera, positionOnPlane, normal, 1.0f);
        reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlane);
    }

    // Математическая матрица Householder Reflection
    private void CalculateReflectionMatrix(ref Matrix4x4 matrix, Vector4 p)
    {
        matrix.m00 = (1F - 2F * p.x * p.x);
        matrix.m01 = (-2F * p.x * p.y);
        matrix.m02 = (-2F * p.x * p.z);
        matrix.m03 = (-2F * p.x * p.w);

        matrix.m10 = (-2F * p.y * p.x);
        matrix.m11 = (1F - 2F * p.y * p.y);
        matrix.m12 = (-2F * p.y * p.z);
        matrix.m13 = (-2F * p.y * p.w);

        matrix.m20 = (-2F * p.z * p.x);
        matrix.m21 = (-2F * p.z * p.y);
        matrix.m22 = (1F - 2F * p.z * p.z);
        matrix.m23 = (-2F * p.z * p.w);

        matrix.m30 = 0F;
        matrix.m31 = 0F;
        matrix.m32 = 0F;
        matrix.m33 = 1F;
    }

    private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cameraPosition = m.MultiplyPoint(pos);
        Vector3 cameraNormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cameraNormal.x, cameraNormal.y, cameraNormal.z, -Vector3.Dot(cameraPosition, cameraNormal));
    }
}
