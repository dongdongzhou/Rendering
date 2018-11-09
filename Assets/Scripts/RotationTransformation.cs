using UnityEngine;

public class RotationTransformation : Transformation
{
    public Vector3 rotation;

    public override Matrix4x4 Matrix
    {
        get
        {
            float radianX = rotation.x * Mathf.Deg2Rad;
            float radianY = rotation.y * Mathf.Deg2Rad;
            float radianZ = rotation.z * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radianX);
            float cosX = Mathf.Cos(radianX);
            float sinY = Mathf.Sin(radianY);
            float cosY = Mathf.Cos(radianY);
            float sinZ = Mathf.Sin(radianZ);
            float cosZ = Mathf.Cos(radianZ);
            var matrix = new Matrix4x4();
            matrix.SetColumn(0, new Vector4(cosY * cosZ,
                                            cosX * sinZ + sinX * sinY * cosZ,
                                            sinX * sinZ - cosX * sinY * cosZ,
                                            0f));
            matrix.SetColumn(1, new Vector4(-cosY * sinZ,
                                            cosX * cosZ - sinX * sinY * sinZ,
                                            sinX * cosZ + cosX * sinY * sinZ,
                                            0f));
            
            matrix.SetColumn(2, new Vector4(sinY,
                                            -sinX * cosY,
                                            cosX * cosY, 
                                            0f));
            
            matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}