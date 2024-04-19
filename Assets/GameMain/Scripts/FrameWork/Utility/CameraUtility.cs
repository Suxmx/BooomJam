using UnityEngine;

namespace GameMain
{
    public static class CameraUtility
    {
        public static float WorldToUIWidth(float worldLength,RectTransform canvasRect)
        {
            // 获取主摄像机
            Camera mainCamera = Camera.main;

            // 获取世界坐标下长度为1的两个点
            Vector3 point1 = Vector3.zero;
            Vector3 point2 = Vector3.right * worldLength;

            // 将这两个点转换为屏幕坐标
            Vector3 screenPoint1 = mainCamera.WorldToScreenPoint(point1);
            Vector3 screenPoint2 = mainCamera.WorldToScreenPoint(point2);

            // 将屏幕坐标转换为UI坐标
            Vector2 uiPoint1;
            Vector2 uiPoint2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint1, null, out uiPoint1);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint2, null, out uiPoint2);

            // 计算UI上的距离
            float uiDistance = Vector2.Distance(uiPoint1, uiPoint2);
            return uiDistance;
        }
    }
}