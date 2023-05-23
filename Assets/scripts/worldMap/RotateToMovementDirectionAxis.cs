using UnityEngine;

public class RotateToMovementDirectionAxis : MonoBehaviour
{
    // 最大の回転角速度[deg/s]
    [SerializeField] private float _maxAngularSpeed = Mathf.Infinity;

    // 進行方向に向くのにかかるおおよその時間[s]
    [SerializeField] private float _smoothTime = 0.05f;

    // オブジェクトの正面
    [SerializeField] private Vector3 _forward = Vector3.forward;

    // オブジェクトの上向き
    [SerializeField] private Vector3 _up = Vector3.up;

    // 回転軸
    [SerializeField] private Vector3 _axis = Vector3.up;

    private Transform _transform;

    // 前フレームのワールド位置
    private Vector3 _prevPosition;

    private float _currentAngularVelocity;

    private void Start()
    {
        _transform = transform;

        _prevPosition = _transform.position;
    }

    private void Update()
    {
        // 現在フレームのワールド位置
        var position = _transform.position;

        // 移動量を計算
        var delta = position - _prevPosition;

        // 次のUpdateで使うための前フレーム位置更新
        _prevPosition = position;

        // 静止している状態だと、進行方向を特定できないため回転しない
        if (delta == Vector3.zero)
            return;

        // 回転補正計算
        var offsetRot = Quaternion.Inverse(Quaternion.LookRotation(_forward, _up));

        // ワールド空間の前方ベクトル取得
        var forward = _transform.TransformDirection(_forward);

        // 回転軸と垂直な平面に投影したベクトル計算
        var projectFrom = Vector3.ProjectOnPlane(forward, _axis);
        var projectTo = Vector3.ProjectOnPlane(delta, _axis);

        // 軸周りの角度差を求める
        var diffAngle = Vector3.Angle(projectFrom, projectTo);

        // 現在フレームで回転する角度の計算
        var rotAngle = Mathf.SmoothDampAngle(
            0,
            diffAngle,
            ref _currentAngularVelocity,
            _smoothTime,
            _maxAngularSpeed
        );

        // 軸周りでの回転の開始と終了を計算
        var lookFrom = Quaternion.LookRotation(projectFrom);
        var lookTo = Quaternion.LookRotation(projectTo);

        // 現在フレームにおける回転を計算
        var nextRot = Quaternion.RotateTowards(lookFrom, lookTo, rotAngle) * offsetRot;

        // オブジェクトの回転に反映
        _transform.rotation = nextRot;
    }
}