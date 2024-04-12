using UnityEngine;

namespace Controllers
{
    public class CurvePointController : Singleton<CurvePointController>
    {
        public Transform pointPrefab;
        public Transform startPoint;
        public Transform controlPoint; // Điểm điều khiển cho Bezier curve
        public Transform endPoint;
        public int numberOfPoints = 10;


        [SerializeField] private Transform mapPoint;
        [SerializeField] private LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer.startWidth = 0.1f; // Đặt độ dày đầu bắt đầu
            lineRenderer.endWidth = 0.1f; // Đặt độ dày đầu kết thúc
            // _lineRenderer.material = Resources.Load<Material>("Black");
            lineRenderer.startColor = Color.black; // Chọn màu đen cho đầu bắt đầu
            lineRenderer.endColor = Color.black; // Chọn màu đen cho đầu kết thúc

            PlacePointsOnCurve();
        }

        public void BackToHome()
        {
            GameManager.Instance.BackToHome();
        }

        private void PlacePointsOnCurve()
        {
            var positions = new Vector3[numberOfPoints];

            for (var i = 0; i < numberOfPoints; i++)
            {
                var t = i / (numberOfPoints - 1f); // Tính toán tham số t
                positions[i] = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
                var pointInstance = Instantiate(pointPrefab, positions[i], Quaternion.identity, mapPoint);
                pointInstance.name = "Point " + (i + 1);
            }

            lineRenderer.positionCount = numberOfPoints; // Đặt số lượng vị trí cho LineRenderer
            lineRenderer.SetPositions(positions); // Cập nhật các vị trí vào LineRenderer
        }

        private static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // Công thức Bezier quadratic
            return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
        }
    }
}