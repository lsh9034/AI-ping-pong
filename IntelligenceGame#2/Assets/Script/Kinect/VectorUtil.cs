using UnityEngine;

public class VectorUtil
{
    public static Vector2 GetVector2FromVector3(Vector3 vector3)
    {
        return vector3;
    }

    public static Vector3 GetVector3FromVector2(Vector2 vector2)
    {
        return VectorUtil.GetVector3FromVector2(vector2, 0.0F);
    }
    public static Vector3 GetVector3FromVector2(Vector2 vector2, float z)
    {
        return new Vector3(vector2.x, vector2.y, z);
    }
    public static Vector3 GetVector3FromVector4(Vector4 vector4)
    {
        return vector4;
    }

    public static Vector4 GetVector4FromVector3(Vector3 vector3)
    {
        return VectorUtil.GetVector4FromVector3(vector3, 0.0F);
    }
    public static Vector4 GetVector4FromVector3(Vector3 vector3, float w)
    {
        return new Vector4(vector3.x, vector3.y, vector3.z, w);
    }

    public static Vector2 GetVector2FromJoint(Windows.Kinect.Joint joint)
    {
        return VectorUtil.GetVector3FromJoint(joint);
    }
    public static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    public static Vector2 Abs(Vector2 vector2)
    {
        return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
    }
    public static Vector3 Abs(Vector3 vector3)
    {
        return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
    }
    public static Vector4 Abs(Vector4 vector4)
    {
        return new Vector4(Mathf.Abs(vector4.x), Mathf.Abs(vector4.y), Mathf.Abs(vector4.z), Mathf.Abs(vector4.w));
    }
}